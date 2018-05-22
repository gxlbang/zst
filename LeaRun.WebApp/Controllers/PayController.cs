
using AlipayAndWepaySDK;
using AlipayAndWepaySDK.Model;
using LeaRun.DataAccess;
using LeaRun.Entity;
using LeaRun.Repository;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Web.Mvc;

namespace LeaRun.WebApp.Controllers
{
    public class PayController : Controller
    {
        IDatabase database = DataFactory.Database();
        WebData wbll = new WebData();

        //
        // GET: /Pay/

        public ActionResult Index()
        {
            return View();
        }
        // GET: Home
        public ActionResult Index(int paymentType, long orderId)
        {
            switch (paymentType)
            {
                case 1: //微信支付

                    return RedirectToAction("WeChat", "Pay", new { orderId = orderId });

                case 2: //支付宝支付

                    return RedirectToAction("AliPay", "Pay", new { orderId = orderId });

            }

            return RedirectToAction("PaymentReturn", "Payment");
        }
        [Authorize]
        public ActionResult WeChat(long orderId)
        {
            if (orderId == 0)
            {
                return Redirect("~/404Page.html");
            }
            var user = wbll.GetUserInfo(Request);
            var account = database.FindEntityByWhere<Ho_PartnerUser>(" and Number=" + user.Number);
            if (account != null && account.Number != null)
            {
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@U_Number", account.Number));
                parameter.Add(DbFactory.CreateDbParameter("@OrderNumber", orderId));


                var payOrder = database.FindEntityByWhere<Am_Charge>(" and U_Number=@U_Number and @OrderNumber=OrderNumber", parameter.ToArray());
                if (payOrder == null)
                {
                    return Redirect("~/404Page.html");
                }
                else if (payOrder.STATUS > 0)
                {
                    ///已经支付
                    return RedirectToAction("AlreadyPayed", "Payment");
                }

                else
                {
                    WePay _wePay = new WePay();
                    AlipayAndWepaySDK.Model.TransmiParameterModel model = new AlipayAndWepaySDK.Model.TransmiParameterModel();
                    model.orderNo = payOrder.OrderNumber;
                    model.productName = "充值";
                    model.totalFee = 10;
                    model.customerIP = "180.136.144.49";
                    model.openId = account.OpenId;
                    var payUrl = _wePay.BuildWePay(model, AlipayAndWepaySDK.Enum.EnumWePayTradeType.JSAPI);

                    // return Newtonsoft.Json.JsonConvert.SerializeObject(payUrl);
                }
            }
            return View();
        }
        [Authorize]
        [HttpPost]
        public string WeChatJSAPI(double money)
        {
            var user = wbll.GetUserInfo(Request);
            var account = database.FindEntityByWhere<Ho_PartnerUser>(" and Number=" + user.Number);
            if (account != null && account.Number != null)
            {
                var charge = new Am_Charge();
                charge.Number = Utilities.CommonHelper.GetGuid;
                charge.OrderNumber = Guid.NewGuid().ToString().Replace("-", "");
                charge.OutNumber = "";
                charge.STATUS = 0;
                charge.StatusStr = "待支付";
                charge.SucTime = DateTime.Now;
                charge.UserName = account.Name;
                charge.U_Number = account.Number;
                charge.ChargeType = 0;
                charge.ChargeTypeStr = "余额充值";
                charge.CreateTime = DateTime.Now;
                charge.PayType = "微信支付";
                var statu = database.Update<Am_Charge>(charge);
                if (statu > 0)
                {
                    List<DbParameter> parameter = new List<DbParameter>();
                    parameter.Add(DbFactory.CreateDbParameter("@U_Number", account.Number));
                    parameter.Add(DbFactory.CreateDbParameter("@OrderNumber", charge.OrderNumber));


                    var payOrder = database.FindEntityByWhere<Am_Charge>(" and U_Number=@U_Number and @OrderNumber=OrderNumber", parameter.ToArray());
                    if (payOrder == null)
                    {
                        return "没有订单";
                    }
                    else if (payOrder.STATUS > 0)
                    {
                        ///已经支付
                        return "订单已支付";
                    }

                    else
                    {
                        WePay _wePay = new WePay();
                        AlipayAndWepaySDK.Model.TransmiParameterModel model = new AlipayAndWepaySDK.Model.TransmiParameterModel();
                        model.orderNo = payOrder.OrderNumber;
                        model.productName = "充值";
                        model.totalFee = 10;
                        model.customerIP = "180.136.144.49";
                        model.openId = account.OpenId;
                        var payUrl = _wePay.BuildWePay(model, AlipayAndWepaySDK.Enum.EnumWePayTradeType.JSAPI);

                        return Newtonsoft.Json.JsonConvert.SerializeObject(payUrl);
                    }
                }


            }
            return "支付失败!";
        }

        /// <summary>
        /// 微信支付异步通知接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult WepayWebNotify()
        {
            //var _logger = ObjectContainer.Resolve<ILoggerFactory>().Create(GetType().FullName);
            WePayReturnModel payResult = new WePayReturnModel();
            WePay _wePay = new WePay();
            var result = _wePay.VerifyNotify(Request, out payResult);

            // _logger.Info("resultXml:" + payResult.RequestForm);


            if (result)
            {
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@OrderNumber", payResult.OutTradeNo));
                parameter.Add(DbFactory.CreateDbParameter("@STATUS", "0"));
                var order = database.FindEntityByWhere<Am_Charge>(" and OrderNumber=@OrderNumber and STATUS=@STATUS ", parameter.ToArray());
                if (order != null && order.Number != null)
                {
                    if (payResult.TotalFee == Decimal.Parse(order.Money.ToString()))
                    {
                        //var orderCommand = new Commands.TradeCenter.UpdateOrderStatusCommand(order.OrderId, 1);
                        //var status = await _commandService.SendAsync(orderCommand);
                        //if (status.Status == ECommon.IO.AsyncTaskStatus.Success)
                        //{
                        //    return Content(payResult.ReturnXml);
                        //}
                        if (order.ChargeType == 1)
                        {
                            List<DbParameter> par = new List<DbParameter>();
                            par.Add(DbFactory.CreateDbParameter("@Number", order.U_Number));
                            var accout = database.FindEntityByWhere<Ho_PartnerUser>(" and Number=@Number", par.ToArray());
                            if (accout != null && accout.Number != null)
                            {
                                order.OutNumber = payResult.TradeNo;
                                order.STATUS = 1;
                                order.StatusStr = "充值成功";
                                order.SucTime = DateTime.Now;
                                database.Update<Am_Charge>(order);

                                accout.Money = accout.Money + order.Money;
                                var status = database.Update<Ho_PartnerUser>(accout);
                                if (status > 0)
                                {
                                    return Content(payResult.ReturnXml);
                                }
                            }
                        }
                        else if (order.ChargeType == 2)
                        {
                            order.OutNumber = payResult.TradeNo;
                            order.STATUS = 1;
                            order.StatusStr = "充值成功";
                            order.SucTime = DateTime.Now;
                            database.Update<Am_Charge>(order);

                            List<DbParameter> par = new List<DbParameter>();
                            par.Add(DbFactory.CreateDbParameter("@Number", order.AmmeterNumber));
                            var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and Number=@Number", par.ToArray());
                            var item = CommonClass.AmmeterApi.AmmeterRecharge(ammeter.Collector_Code, ammeter.AM_Code, ammeter.Acount_Id.Value, ammeter.Count.Value, int.Parse(order.Money.ToString()));
                            if (item.suc)
                            {
                                var task = new Am_Task
                                {
                                    AmmeterCode = ammeter.AM_Code,
                                    AmmeterNumber = ammeter.Number,
                                    Number = item.opr_id,
                                    CollectorCode = ammeter.Collector_Code,
                                    CollectorNumber = ammeter.Collector_Number,
                                    CreateTime = DateTime.Now,
                                    OperateType = 4,
                                    OperateTypeStr = "微信充值",
                                    OrderNumber = order.OrderNumber,
                                    OverTime = DateTime.Now,
                                    Remark = "",
                                    Status = 0,
                                    StatusStr = "队列中",
                                    TaskMark = "",
                                    UserName = order.UserName,
                                    U_Name = order.U_Name,
                                    U_Number = order.U_Number,
                                    Money = order.Money
                                };
                                database.Insert<Am_Task>(task);

                            }
                            return Content(payResult.ReturnXml);
                        }
                        else if (order.ChargeType == 3)
                        {
                            order.OutNumber = payResult.TradeNo;
                            order.STATUS = 1;
                            order.StatusStr = "缴费成功";
                            order.SucTime = DateTime.Now;
                            database.Update<Am_Charge>(order);

                            List<DbParameter> par = new List<DbParameter>();
                            par.Add(DbFactory.CreateDbParameter("@Number", order.ObjectNumber));
                            par.Add(DbFactory.CreateDbParameter("@Status", "1"));
                            var bill = database.FindEntityByWhere<Am_Bill>(" and Number=@Number and Status=@Status ", par.ToArray());
                            if (bill != null && bill.Number != null)
                            {
                                bill.Status = 2;
                                bill.StatusStr = "已支付";
                                bill.PayTime = DateTime.Now;
                                database.Update<Am_Bill>(bill);
                                return Content(payResult.ReturnXml);
                            }
                        }
                    }
                }


            }
            return Content(BuildWepayReturnXml("FAIL", ""));
        }

        private string BuildWepayReturnXml(string code, string returnMsg)
        {
            return string.Format("<xml><return_code><![CDATA[{0}]]></return_code><return_msg><![CDATA[{1}]]></return_msg></xml>", code, returnMsg);
        }
    }
}
