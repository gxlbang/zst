
using AlipayAndWepaySDK;
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
            if (account!=null&&account.Number!=null )
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
                var statu = database.Update<Am_Charge>(charge);
                if (statu>0)
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
    }
}
