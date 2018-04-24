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
using System.Text;
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
                    return Json(new { res = "Ok", msg = "提交成功" });
                }
            }
            return Json(new { res = "No", msg = "提交失败" });
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
        /// <summary>
        /// 提现申请
        /// </summary>
        /// <param name="money"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Withdraw(double money, string pwd)
        {
            var user = wbll.GetUserInfo(Request);
            var account = database.FindEntityByWhere<Ho_PartnerUser>(" and Number='" + user.Number + "'");
            if (account != null && account.Number != null && account.Status == 3)
            {
                if (account.PayPassword == null || account.PayPassword == "")
                {
                    return Json(new { res = "No", msg = "请先设置支付密码" });
                }
                if (!PasswordHash.ValidatePassword(pwd, account?.PayPassword))
                {
                    return Json(new { res = "No", msg = "支付密码错误" });
                }
                if (money > account.Money)
                {
                    return Json(new { res = "No", msg = "超出提现金额" });
                }

                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));
                var bank = database.FindEntityByWhere<Am_BankInfo>(" and U_Number=@U_Number", parameter.ToArray());
                if (bank == null || bank.Number == null)
                {
                    return Json(new { res = "No", msg = "请先绑定银行卡" });
                }

                var moneyToBank = new Am_UserGetMoneyToBank
                {
                    Number = CommonHelper.GetGuid,
                    Money = money,
                    PayTime = DateTime.Now,
                    RealMoney = account.Money,
                    Status = 0,
                    StatusStr = "提现申请",
                    U_Number = account.Number,
                    U_Name = account.Name,
                    BankAddress = bank.BankAddress,
                    BankCode = bank.BankCode,
                    BankName = bank.BankName,
                    BankCharge = 0,
                    CreateTime = DateTime.Now,
                    Remark = "",
                    UserName = account.Account
                };
                var status = database.Insert<Am_UserGetMoneyToBank>(moneyToBank);
                if (status > 0)
                {
                    return Json(new { res = "Ok", msg = "成功提交申请" });
                }
            }
            return Json(new { res = "No", msg = "提交失败" });
        }
        /// <summary>
        /// 提现列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult WithdrawList(int pageIndex = 1, int pageSize = 10)
        {
            var user = wbll.GetUserInfo(Request);
            int recordCount = 0;
            var list = database.FindListPage<Am_UserGetMoneyToBank>("Number", "desc", pageIndex, pageSize, ref recordCount);
            ViewBag.recordCount = (int)Math.Ceiling(1.0 * recordCount / pageSize); ;
            if (Request.IsAjaxRequest())
            {
                return Json(list);
            }
            else
            {
                return View();
            }
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
                return Json(new { res = "No", msg = "验证码错误" });
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

            return Json(new { res = "No", msg = "绑定失败" });
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
            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));

            var ammeterList = database.FindListPage<Am_Ammeter>(" and U_Number=@U_Number",parameter.ToArray(),"Number", "desc", pageIndex, pageSize, ref recordCount);
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
                        if (account.PayPassword == null || account.PayPassword == "")
                        {
                            return Json(new { res = "No", msg = "请先设置支付密码" }).ToJson();
                        }
                        if (!PasswordHash.ValidatePassword(pwd, account?.PayPassword))
                        {
                            return Json(new { res = "No", msg = "支付密码错误" }).ToJson();
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
                    charge.ChargeType = 2;
                    charge.ChargeTypeStr = "电费充值";
                    charge.CreateTime = DateTime.Now;
                    charge.AmmeterNumber = ammeter.Number;
                    charge.AmmeterCode = ammeter.AM_Code;
                    charge.Moeny = money;

                    if (type == 0)
                    {
                        charge.PayType = "微信支付";
                        var status = database.Insert<Am_Charge>(charge);
                        if (status > 0)
                        {
                            return WePay(user, account, charge, "充值");
                        }
                    }
                    else if (type == 1)
                    {
                        charge.PayType = "余额支付";
                        if (money > account.Money)
                        {
                            return Json(new { res = "No", msg = "余额不足，请先充值" }).ToJson();
                        }
                        var status = database.Insert<Am_Charge>(charge);
                        if (status > 0)
                        {
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

            return Json(new { res = "No", msg = "充值失败" }).ToJson();
        }
        /// <summary>
        /// 缴费_微信支付
        /// </summary>
        /// <param name="user"></param>
        /// <param name="account"></param>
        /// <param name="charge"></param>
        /// <returns></returns>
        private string WePay(Ho_PartnerUser user, Ho_PartnerUser account, Am_Charge charge, string title)
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
                model.productName = title;
                model.totalFee = int.Parse((charge.Moeny * 100).ToString());
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
        /// <summary>
        /// 我的新账单
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult MyNewBill(int pageIndex = 1, int pageSize = 10)
        {
            var user = wbll.GetUserInfo(Request);
            int recordCount = 0;
            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@T_U_Number", user.Number));
            parameter.Add(DbFactory.CreateDbParameter("@Status", "1"));

            var billList = database.FindListPage<Am_Bill>(" and T_U_Number=@T_U_Number and Status=@Status ", parameter.ToArray(), "Number", "desc", pageIndex, pageSize, ref recordCount);
            ViewBag.recordCount = (int)Math.Ceiling(1.0 * recordCount / pageSize); ;
            if (Request.IsAjaxRequest())
            {
                return Json(billList);
            }
            else
            {
                return View();
            }
        }
        /// <summary>
        /// 新账单详情
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public ActionResult NewBillDetails(string number)
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@T_U_Number", user.Number));
            parameter.Add(DbFactory.CreateDbParameter("@Status", "1"));
            parameter.Add(DbFactory.CreateDbParameter("@Number", number));

            var bill = database.FindEntityByWhere<Am_Bill>(" and T_U_Number=@T_U_Number and Status=@Status and Number=@Number", parameter.ToArray());
            if (bill != null && bill.Number != null)
            {
                List<DbParameter> par1 = new List<DbParameter>();
                par1.Add(DbFactory.CreateDbParameter("@Bill_Number", bill.Number));

                var billContentList = database.FindList<Am_BillContent>(" and Bill_Number=@Bill_Number ", par1.ToArray());
                ViewBag.content = billContentList;
            }
            return View(bill);
        }
        /// <summary>
        /// 账单缴费
        /// </summary>
        /// <param name="number"></param>
        /// <param name="type"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [HttpPost]
        public string PayNewBill(string number, int type, string pwd)
        {
            var user = wbll.GetUserInfo(Request);
            var account = database.FindEntityByWhere<Ho_PartnerUser>(" and Number='" + user.Number + "'");
            if (account != null && account.Number != null && account.Status == 3)
            {
                if (type == 1)
                {
                    if (account.PayPassword == null || account.PayPassword == "")
                    {
                        return Json(new { res = "No", msg = "请先设置支付密码" }).ToJson();
                    }
                    if (!PasswordHash.ValidatePassword(pwd, account?.PayPassword))
                    {
                        return Json(new { res = "No", msg = "支付密码错误" }).ToJson();
                    }
                }
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@T_U_Number", user.Number));
                parameter.Add(DbFactory.CreateDbParameter("@Status", "1"));
                parameter.Add(DbFactory.CreateDbParameter("@Number", number));

                var bill = database.FindEntityByWhere<Am_Bill>(" and T_U_Number=@T_U_Number and Status=@Status and Number=@Number", parameter.ToArray());
                if (bill != null && bill.Number != null)
                {
                    var charge = new Am_Charge();
                    charge.Number = Utilities.CommonHelper.GetGuid;
                    charge.OrderNumber = Guid.NewGuid().ToString().Replace("-", "");
                    charge.OutNumber = "";
                    charge.STATUS = 0;
                    charge.StatusStr = "待支付";
                    charge.SucTime = DateTime.Now;
                    charge.UserName = user.Name;
                    charge.U_Number = user.Number;
                    charge.ChargeType = 3;
                    charge.ChargeTypeStr = "账单支付";
                    charge.CreateTime = DateTime.Now;
                    charge.AmmeterNumber = "";
                    charge.AmmeterCode = "";
                    charge.Moeny = bill.Money;

                    if (type == 0)
                    {
                        charge.PayType = "微信缴费";
                        var status = database.Insert<Am_Charge>(charge);
                        if (status > 0)
                        {
                            return WePay(user, account, charge, "账单缴费");
                        }
                    }
                    else if (type == 1)
                    {
                        charge.PayType = "余额缴费";
                        if (bill.Money > account.Money)
                        {
                            return Json(new { res = "No", msg = "余额不足，请先充值" }).ToJson();
                        }
                        var status = database.Insert<Am_Charge>(charge);
                        if (status > 0)
                        {
                            account.Money = account.Money - bill.Money;
                            var accStatus = database.Update<Ho_PartnerUser>(account);
                            if (accStatus > 0)
                            {
                                charge.STATUS = 1;
                                charge.StatusStr = "支付成功";
                                var st = database.Update<Am_Charge>(charge);
                                if (st > 0)
                                {
                                    return Json(new { res = "Ok", msg = "缴费成功" }).ToJson();
                                }
                            }
                        }
                    }
                }
            }
            return Json(new { res = "No", msg = "缴费失败" }).ToJson();
        }
        /// <summary>
        /// 历史账单查询
        /// </summary>
        /// <param name="ammeterNumber"></param>
        /// <param name="star"></param>
        /// <param name="end"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult OldBill(string ammeterNumber, DateTime star, DateTime end, int pageIndex = 1, int pageSize = 10)
        {
            var user = wbll.GetUserInfo(Request);
            int recordCount = 0;
            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@F_U_Number", user.Number));
            parameter.Add(DbFactory.CreateDbParameter("@StarTime", star));
            parameter.Add(DbFactory.CreateDbParameter("@EndTime", end));

            StringBuilder whereSb = new StringBuilder();
            if (ammeterNumber != null || ammeterNumber != "")
            {
                whereSb.Append(" and AmmeterNumber=@AmmeterNumber");
                parameter.Add(DbFactory.CreateDbParameter("@AmmeterNumber", ammeterNumber));
            }
            whereSb.Append(" and SendTime>=@StarTime");
            whereSb.Append(" and SendTime<=@EndTime");


            var billList = database.FindListPage<Am_Bill>(whereSb.ToString(), parameter.ToArray(), "Number", "desc", pageIndex, pageSize, ref recordCount);
            ViewBag.recordCount = (int)Math.Ceiling(1.0 * recordCount / pageSize); ;
            if (Request.IsAjaxRequest())
            {
                return Json(billList);
            }
            else
            {
                return View();
            }
        }

        public ActionResult Repair()
        {
            return View();
        }
        /// <summary>
        /// 报修电表列表
        /// </summary>
        /// <returns></returns>
        public ActionResult RepairList()
        {
            var user = wbll.GetUserInfo(Request);

            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));
            parameter.Add(DbFactory.CreateDbParameter("@Status", "1"));

            var list = new List<Am_Ammeter>();
            var apList = database.FindList<Am_AmmeterPermission>(" and U_Number=@U_Number and Status=@Status ", parameter.ToArray());
            if (apList.Count() > 0)
            {
                foreach (var item in apList)
                {
                    List<DbParameter> par = new List<DbParameter>();
                    par.Add(DbFactory.CreateDbParameter("@Number", item.Ammeter_Number));

                    var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and Number=@Number", par.ToArray());
                    if (ammeter != null && ammeter.Number != null)
                    {
                        list.Add(ammeter);
                    }
                }
            }
            return View(list);
        }
        /// <summary>
        /// 报修添加页面
        /// </summary>
        /// <param name="ammeterNumber"></param>
        /// <returns></returns>
        public ActionResult RepairAdd(string ammeterNumber)
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> par = new List<DbParameter>();
            par.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));
            par.Add(DbFactory.CreateDbParameter("@Number", ammeterNumber));

            var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and Number=@Number and U_Number=@U_Number", par.ToArray());
            if (ammeter != null && ammeter.Number != null)
            {
                return View(ammeter);
            }
            return View();
        }
        /// <summary>
        /// 报修提交
        /// </summary>
        /// <param name="repair"></param>
        /// <param name="imgList"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RepairAdd(Am_Repair repair, List<string> imgList)
        {
            var user = wbll.GetUserInfo(Request);
            if (imgList.Count > 5)
            {
                return Json(new { res = "No", msg = "图片超出，最多5张" });
            }

            List<DbParameter> par = new List<DbParameter>();
            par.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));
            par.Add(DbFactory.CreateDbParameter("@Number", repair.AmmeterNumber));

            var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and Number=@Number and U_Number=@U_Number", par.ToArray());
            if (ammeter != null && ammeter.Number != null)
            {
                repair.Number = Utilities.CommonHelper.GetGuid;
                repair.CreateTime = DateTime.Now;
                repair.U_Number = user.Number;
                repair.U_Name = user.Name;
                repair.Status = 0;
                repair.StatusStr = "提交报修";
                repair.F_Number = ammeter.UY_Number;
                repair.F_UserName = ammeter.UY_UserName;

                var status = database.Insert<Am_Repair>(repair);
                if (status > 0)
                {
                    List<Am_RepairImage> repairImageList = new List<Am_RepairImage>();
                    foreach (var item in imgList)
                    {
                        var model = new Am_RepairImage
                        {
                            Number = Utilities.CommonHelper.GetGuid,
                            ImagePath = item,
                            Repair_Number = repair.Number,
                            ImageMark = "",
                            Remark = ""
                        };
                        repairImageList.Add(model);
                    }
                    database.Insert<Am_RepairImage>(repairImageList);
                    return Json(new { res = "Ok", msg = "提交成功" });
                }
            }
            return Json(new { res = "No", msg = "提交失败" });
        }
        /// <summary>
        /// 报修记录
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult RepairRecord(int pageIndex = 1, int pageSize = 10)
        {
            var user = wbll.GetUserInfo(Request);

            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));

            int recordCount = 0;
            var repairList = database.FindListPage<Am_Repair>(" and U_Number=@U_Number", parameter.ToArray(), "Number", "desc", pageIndex, pageSize, ref recordCount);
            ViewBag.recordCount = (int)Math.Ceiling(1.0 * recordCount / pageSize); ;
            if (Request.IsAjaxRequest())
            {
                return Json(repairList);
            }
            else
            {
                return View();
            }
        }
        /// <summary>
        /// 报修详情
        /// </summary>
        /// <param name="Number"></param>
        /// <returns></returns>
        public ActionResult RepairInfo(string Number)
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> par = new List<DbParameter>();
            par.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));
            par.Add(DbFactory.CreateDbParameter("@Number", Number));

            var repair = database.FindEntityByWhere<Am_Repair>(" and Number=@Number and U_Number=@U_Number", par.ToArray());
            if (repair != null && repair.Number != null)
            {
                return View(repair);
            }
            return View();
        }
        /// <summary>
        /// 我的押金
        /// </summary>
        /// <returns></returns>
        public ActionResult MyDeposit()
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> par = new List<DbParameter>();
            par.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));
            par.Add(DbFactory.CreateDbParameter("@Status", "1"));

            var depositList = database.FindList<Am_UserDeposit>(" and Number=@Number and Status=@Status", par.ToArray());
            if (depositList != null && depositList.Count() > 0)
            {
                return View(depositList);
            }
            return View();
        }
        /// <summary>
        /// 退租
        /// </summary>
        /// <returns></returns>
        public ActionResult Rent()
        {
            var user = wbll.GetUserInfo(Request);

            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));
            parameter.Add(DbFactory.CreateDbParameter("@Status", "1"));

            var list = new List<Am_Ammeter>();
            var apList = database.FindList<Am_AmmeterPermission>(" and U_Number=@U_Number and Status=@Status ", parameter.ToArray());
            if (apList.Count() > 0)
            {
                foreach (var item in apList)
                {
                    List<DbParameter> par = new List<DbParameter>();
                    par.Add(DbFactory.CreateDbParameter("@Number", item.Ammeter_Number));

                    var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and Number=@Number", par.ToArray());
                    if (ammeter != null && ammeter.Number != null)
                    {
                        list.Add(ammeter);
                    }
                }
            }
            return View(list);
        }
        /// <summary>
        /// 提交退租
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RentCreate(string number)
        {
            var user = wbll.GetUserInfo(Request);

            List<DbParameter> par = new List<DbParameter>();
            par.Add(DbFactory.CreateDbParameter("@Number", number));

            var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and Number=@Number", par.ToArray());
            if (ammeter != null && ammeter.Number != null)
            {
                var rent = new Am_Rent
                {
                    Address = ammeter.Address,
                    AmmeterCode = ammeter.AM_Code,
                    AmmeterNumber = ammeter.Number,
                    Cell = ammeter.Cell,
                    Number = Utilities.CommonHelper.GetGuid,
                    City = ammeter.City,
                    CollectorCode = ammeter.Collector_Code,
                    CollectorNumber = ammeter.Collector_Number,
                    County = ammeter.County,
                    CreateTime = DateTime.Now,
                    Floor = ammeter.Floor,
                    FMark = "",
                    F_Number = ammeter.UY_Number,
                    F_UserName = ammeter.UY_UserName,
                    Money = 0.00,
                    Province = ammeter.Province,
                    Remark = "",
                    Room = ammeter.Room,
                    Status = 0,
                    StatusStr = "申请退租",
                    SucTime = DateTime.Now,
                    UserMark = "",
                    UserName = user.Accout,
                    U_Name = user.Name,
                    U_Number = user.Number
                };
                var status = database.Insert<Am_Rent>(rent);
                if (status>0)
                {
                    return Json(new { res = "Ok", msg = "提交成功" });
                }
            }

            return Json(new { res = "No", msg = "提交失败" });
        }
        /// <summary>
        /// 退租记录
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult RentList(int pageIndex = 1, int pageSize = 10)
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));
            parameter.Add(DbFactory.CreateDbParameter("@Status", "1"));

            int recordCount = 0;
            var rentList = database.FindListPage<Am_Rent>(" and U_Number=@U_Number and Status=@Status", parameter.ToArray(), "Number", "desc", pageIndex, pageSize, ref recordCount);
            ViewBag.recordCount = (int)Math.Ceiling(1.0 * recordCount / pageSize);
            if (Request.IsAjaxRequest())
            {
                return Json(rentList);
            }
            else
            {
                return View();
            }
        }
        /// <summary>
        ///退租详情 
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public ActionResult RentDetails(string number )
        {
            var user = wbll.GetUserInfo(Request);
            List < DbParameter > par = new List<DbParameter>();
            par.Add(DbFactory.CreateDbParameter("@Number", number));
            par.Add(DbFactory.CreateDbParameter("@U_Number",user.Number));

            var rent = database.FindEntityByWhere<Am_Rent>(" and Number=@Number and U_Number=@U_Number", par.ToArray());
            if (rent != null && rent.Number != null)
            {
                List<DbParameter> par1 = new List<DbParameter>();
                par1.Add(DbFactory.CreateDbParameter("@RentNumber", rent.Number));
                var rentBillList = database.FindList<Am_RentBill>(" and RentNumber=@RentNumber",par1.ToArray());
            }
            return View(rent);
        }
    }
}
