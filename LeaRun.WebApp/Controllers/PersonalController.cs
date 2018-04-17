using AlipayAndWepaySDK;
using Extensions;
using LeaRun.DataAccess;
using LeaRun.Entity;
using LeaRun.Repository;
using LeaRun.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeaRun.WebApp.Controllers
{
    //[UserLoginFilters]
    public class PersonalController : Controller
    {
        IDatabase database = DataFactory.Database();
        WebData wbll = new WebData();

        //
        // GET: /Personal/

        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 完善信息
        /// </summary>
        /// <returns></returns>
        public ActionResult Perfect()
        {
            return View();
        }
        /// <summary>
        /// 提交完善信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Perfect(Ho_PartnerUser model)
        {
            var user = wbll.GetUserInfo(Request);
            var account = database.FindEntityByWhere<Ho_PartnerUser>(" and Number=" + user.Number);
            if (account != null && account.Number != null)
            {
                account.Name = model.Name;
                account.Sex = model.Sex;
                account.CardCode = model.CardCode;
                account.CodeImg1 = model.CodeImg1;
                account.CodeImg2 = model.CodeImg2;
                account.Address = model.Address;
                account.PayPassword = model.PayPassword;
                account.Status = 1;
                var statu = database.Update<Ho_PartnerUser>(account);
                if (statu > 0)
                {
                    return Json(new { res = "Ok", msg = "提交成功!" });
                }
            }
            return Json(new { res = "On", msg = "提交失败!" });
        }
        /// <summary>
        /// 个人信息
        /// </summary>
        /// <returns></returns>
        public ActionResult Information()
        {
            var user = wbll.GetUserInfo(Request);
            var account = database.FindEntityByWhere<Ho_PartnerUser>(" and Number=" + user.Number);
            if (account != null && account.Number != null)
            {
                return View(account);
            }
            return View();
        }
        /// <summary>
        /// 我的余额
        /// </summary>
        /// <returns></returns>
        public ActionResult MyBalance()
        {
            var user = wbll.GetUserInfo(Request);
            var account = database.FindEntityByWhere<Ho_PartnerUser>(" and Number='" + user.Number + "'");
            if (account != null && account.Number != null)
            {
                ViewBag.Balance = account.Money;
            }
            return View();
        }
        /// <summary>
        /// 余额明细
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult MyBalanceDetail(int pageIndex = 1, int pageSize = 10)
        {
            var user = wbll.GetUserInfo(Request);
            int recordCount = 0;
            var balanDetailList = database.FindListPage<Am_MoneyDetail>("Number", "desc", pageIndex, pageSize, ref recordCount);
            ViewBag.recordCount = (int)Math.Ceiling(1.0 * recordCount / pageSize); ;
            if (Request.IsAjaxRequest())
            {
                return Json(balanDetailList);
            }
            else
            {
                return View();
            }
        }
        /// <summary>
        /// 余额充值
        /// </summary>
        /// <returns></returns>
        public ActionResult BalanceRecharge()
        {
            return View();
        }
        [HttpPost]
        public ActionResult BalanceRecharge(double money, int type)
        {
            var user = wbll.GetUserInfo(Request);
            var account = database.FindEntityByWhere<Ho_HouseInfo>(" and Number='" + user.Number + "'");
            if (account != null && account.Number != null)
            {

                //Am_Charge
            }
            return View();
        }
        /// <summary>
        /// 我的提现
        /// </summary>
        /// <returns></returns>
        public ActionResult Withdraw()
        {
            var user = wbll.GetUserInfo(Request);
            var account = database.FindEntityByWhere<Ho_PartnerUser>(" and Number='" + user.Number + "'");
            if (account != null && account.Number != null)
            {
                ViewBag.Balance = account.Money;
            }
            return View();
        }
        [HttpPost]
        public ActionResult Withdraw(double money, string paw)
        {
            var user = wbll.GetUserInfo(Request);
            var account = database.FindEntityByWhere<Ho_PartnerUser>(" and Number='" + user.Number + "'");
            if (account != null && account.Number != null && account.Status == 3)
            {

            }
            return View();
        }
        /// <summary>
        /// 我的银行卡
        /// </summary>
        /// <returns></returns>
        public ActionResult BankCard()
        {
            var user = wbll.GetUserInfo(Request);
            var bank = database.FindEntityByWhere<Am_BankInfo>(" and Number='" + user.Number + "'");
            if (bank != null && bank.Number != null)
            {
                return View(bank);
            }
            return View();
        }
        /// <summary>
        /// 添加银行卡
        /// </summary>
        /// <returns></returns>
        public ActionResult AddBandCard()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="validCode"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddBandCard(Am_BankInfo model, string validCode)
        {
            var user = wbll.GetUserInfo(Request);
            string realCode = Utilities.DESEncrypt.Decrypt(CookieHelper.GetCookie("WebCode"));
            if (StringHelper.IsNullOrEmpty(validCode) || validCode != realCode)
            {
                return Json(new { res = "On", msg = "验证码错误！" });
            }
            var account = database.FindEntityByWhere<Ho_HouseInfo>(" and Number='" + user.Number + "'");
            if (account != null && account.Number != null)
            {
                var bank = new Am_BankInfo();
                bank.Number = CommonHelper.GetGuid;
                bank.Remark = "";
                bank.UserName = "";
                bank.U_Name = account.Name;
                bank.U_Number = account.Number;
                bank.BankAddress = model.BankAddress;
                bank.BankCode = model.BankCode;
                bank.BankName = model.BankName;
                bank.CreateTime = DateTime.Now;

                int status = database.Insert<Am_BankInfo>(bank);
                if (status > 0)
                {
                    return Json(new { res = "Ok", msg = "绑定成功" });
                }

            }

            return Json(new { res = "On", msg = "绑定失败" });
        }

        /// <summary>
        /// 我的电表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult MyAmmeter(int pageIndex = 1, int pageSize = 10)
        {
            var user = wbll.GetUserInfo(Request);
            int recordCount = 0;
            var ammeterList = database.FindListPage<Am_Ammeter>("Number", "desc", pageIndex, pageSize, ref recordCount);
            ViewBag.recordCount = (int)Math.Ceiling(1.0 * recordCount / pageSize); ;
            if (Request.IsAjaxRequest())
            {
                return Json(ammeterList);
            }
            else
            {
                return View();
            }
        }
        /// <summary>
        /// 电表详情
        /// </summary>
        /// <param name="Number"></param>
        /// <returns></returns>
        public ActionResult AmmeterDetails(string Number)
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));
            parameter.Add(DbFactory.CreateDbParameter("@Number", Number));

            var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and Number=@Number and U_Number=@U_Number", parameter.ToArray());
            if (ammeter != null && ammeter.Number != null)
            {
                return View(ammeter);
            }
            return View();
        }
        /// <summary>
        /// 电表充值
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public ActionResult AmmeterRecharge(string number)
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));
            parameter.Add(DbFactory.CreateDbParameter("@Number", number));

            var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and Number=@Number and U_Number=@U_Number", parameter.ToArray());
            if (ammeter != null && ammeter.Number != null)
            {
                return View(ammeter);
            }
            return View();
        }
        /// <summary>
        /// 电费充值提交
        /// </summary>
        /// <param name="number"></param>
        /// <param name="money"></param>
        /// <param name="type"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [HttpPost]
        public string AmmeterRecharge(string number, double money, int type, string pwd)
        {
            var user = wbll.GetUserInfo(Request);

            var account = database.FindEntityByWhere<Ho_PartnerUser>(" and Number='" + user.Number + "'");
            if (account != null && account.Number != null && account.Status == 3)
            {
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));
                parameter.Add(DbFactory.CreateDbParameter("@Number", number));
                var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and Number=@Number and U_Number=@U_Number", parameter.ToArray());
                if (ammeter != null && ammeter.Number != null)
                {
                    if (type == 1)
                    {
                        if (!PasswordHash.ValidatePassword(pwd, account?.Password))
                        {
                            return Json(new { res = "On", msg = "支付密码错误!" }).ToJson();
                        }
                    }
                    var charge = new Am_Charge();
                    charge.Number = Utilities.CommonHelper.GetGuid;
                    charge.OrderNumber = Guid.NewGuid().ToString().Replace("-", "");
                    charge.OutNumber = "";
                    charge.STATUS = 0;
                    charge.StatusStr = "待支付";
                    charge.SucTime = DateTime.Now;
                    charge.UserName = user.Name;
                    charge.U_Number = user.Number;
                    charge.ChargeType = 1;
                    charge.ChargeTypeStr = "电费充值";
                    charge.CreateTime = DateTime.Now;
                    charge.AmmeterNumber = ammeter.Number;
                    charge.AmmeterCode = ammeter.AM_Code;

                    if (type == 0)
                    {
                        charge.PayType = "微信支付";
                        var status = database.Insert<Am_Charge>(charge);
                        if (status > 0)
                        {
                            return WePay(user, account, charge);
                        }
                    }
                    else if (type == 1)
                    {
                        charge.PayType = "余额支付";
                        var status = database.Insert<Am_Charge>(charge);
                        if (status > 0)
                        {
                            if (money > account.Money)
                            {
                                return Json(new { res = "On", msg = "余额不足，请先充值!" }).ToJson();
                            }
                            account.Money = account.Money - money;
                            var accStatus = database.Update<Ho_PartnerUser>(account);
                            if (accStatus > 0)
                            {
                                charge.STATUS = 1;
                                charge.StatusStr = "支付成功";
                                var st = database.Update<Am_Charge>(charge);
                                if (st > 0)
                                {
                                    return Json(new { res = "Ok", msg = "充值成功" }).ToJson();
                                }
                            }
                        }
                    }
                }
            }

            return Json(new { res = "On", msg = "充值失败!" }).ToJson();
        }
        /// <summary>
        /// 缴费_微信支付
        /// </summary>
        /// <param name="user"></param>
        /// <param name="account"></param>
        /// <param name="charge"></param>
        /// <returns></returns>
        private string WePay(Ho_PartnerUser user, Ho_PartnerUser account, Am_Charge charge)
        {
            List<DbParameter> parameter2 = new List<DbParameter>();
            parameter2.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));
            parameter2.Add(DbFactory.CreateDbParameter("@OrderNumber", charge.OrderNumber));


            var payOrder = database.FindEntityByWhere<Am_Charge>(" and U_Number=@U_Number and @OrderNumber=OrderNumber", parameter2.ToArray());
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
        /// <summary>
        /// 缴费记录
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult AmmeterPayCost(int pageIndex = 1, int pageSize = 10)
        {
            var user = wbll.GetUserInfo(Request);
            int recordCount = 0;
            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));
            parameter.Add(DbFactory.CreateDbParameter("@STATUS", "1"));

            var chargeList = database.FindListPage<Am_Charge>(" and U_Number=@U_Number and STATUS=@STATUS", parameter.ToArray(), "Number", "desc", pageIndex, pageSize, ref recordCount);
            ViewBag.recordCount = (int)Math.Ceiling(1.0 * recordCount / pageSize); ;
            if (Request.IsAjaxRequest())
            {
                return Json(chargeList);
            }
            else
            {
                return View();
            }
        }
        public ActionResult MyNewBill()
        {

            return View();
        }

 

    }
}
