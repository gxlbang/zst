using Extensions;
using LeaRun.DataAccess;
using LeaRun.DataAccess.Common;
using LeaRun.Entity;
using LeaRun.Repository;
using LeaRun.Utilities;
using LeaRun.WebApp.ViewsModel.Account;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeaRun.WebApp.Controllers
{
    public class AccountController : Controller
    {
        IDatabase database = DataFactory.Database();
        private static readonly string LoginReturnUrlCookieName = "longin_return_url";
        public ActionResult Register()
        {
           
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Register model)
        {
            if (ModelState.IsValid)
            {
                string realCode = Utilities.DESEncrypt.Decrypt(CookieHelper.GetCookie("WebCode"));
                if (StringHelper.IsNullOrEmpty(model.ValidCode) || model.ValidCode != realCode)
                {
                    return Json(new { res = "No", msg = "验证码错误" });
                }
                if (model.Password !=model .ConfirmPassword)
                {
                    return Json(new { res = "No", msg = "两次密码不一致" });
                }
                var accountIsMobile = database.FindEntity<Ho_PartnerUser>(" and Accout='" + model.Name+"'");
                if (accountIsMobile != null&&accountIsMobile.Number!=null )
                {
                    return Json(new { res = "No", msg = "已存在用户" });
                }

                var insertModel = new Ho_PartnerUser();
                insertModel.Account = model.Name;
                insertModel.Number = CommonHelper.GetGuid;
                insertModel.Password = PasswordHash.CreateHash(model.Password);
                insertModel.Mobile = model.Name;
                insertModel.CreatTime = DateTime.Now;
                insertModel.ModifyTime = DateTime.Now;
                insertModel.SureTime = DateTime.Now;
                insertModel.Money = 0.00;
                insertModel.FreezeMoney = 0.00;
                insertModel.Status = 0;
                insertModel.Mobile = model.Name;
                insertModel.StatusStr = "新注册";
                insertModel.Birthday = DateTime.Now;
                var role = database.FindEntityByWhere<Am_UserRole>(" and RoleName='普通会员'");
                if (role!=null &&role.Number !=null)
                {
                    insertModel.UserRole = role.RoleName;
                    insertModel.UserRoleNumber = role.Number;
                    var num = database.Insert<Ho_PartnerUser>(insertModel);
                    if (num > 0)
                    {
                        CookieHelper.WriteCookie("WebCode", null);
                        return Json(new { res = "Ok", msg = "注册成功" });
                    }
                }
                
            }
            return Json(new { res = "No", msg = "注册失败" });
        }


        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginPwd(string name, string pwd)
        {
            if (ModelState.IsValid)
            {
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@Account", name));
                var account = database.FindEntityByWhere<Ho_PartnerUser>(" and Account=@Account" ,parameter.ToArray());
                if (account != null 
                    && account.Number != null 
                    && account.Status == 3 
                    && PasswordHash.ValidatePassword(pwd, account?.Password))
                {
                    // 抽取用户信息
                    string Md5 = Md5Helper.MD5(account.Number + account.OpenId + Request.UserHostAddress + Request.Browser.Type + Request.Browser.ClrVersion.ToString() + "2017", 16);

                    string str = account.Number + "&" + account.OpenId + "&" + Request.UserHostAddress + "&" + Request.Browser.Type
                        + "&" + Request.Browser.ClrVersion.ToString() + "&" + Md5;

                    str = Utilities.DESEncrypt.Encrypt(str);
                    CookieHelper.WriteCookie("WebUserInfo", str);
                    //获取cookie并判断是否有跳转URL
                    HttpCookie cookie = Request.Cookies[LoginReturnUrlCookieName];
                    string returnUrl = "";
                    if (cookie != null)
                    {
                        returnUrl = cookie.Value;
                        DeleteCookie(LoginReturnUrlCookieName);
                    }

                    return Json(new { res = "Ok", Msg = "登录成功", returnUrl = returnUrl });
                }
                else
                {
                    Json(new { res = "No", msg = "用户名或密码不对" });
                }
            }
            return Json(new { res = "No", msg = "登录失败" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginCode(string name, string validCode)
        {
            if (ModelState.IsValid)
            {
                string realCode = Utilities.DESEncrypt.Decrypt(CookieHelper.GetCookie("WebCode"));
                if (StringHelper.IsNullOrEmpty(validCode) || validCode != realCode)
                {
                    return Json(new { res = "No", msg = "验证码错误" });
                }
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@Account", name));
                var account = database.FindEntityByWhere<Ho_PartnerUser>(" and Account=@Account", parameter.ToArray());
                if (account != null&&account.Number !=null &&account.Status==3)
                {
                    // 抽取用户信息
                    string Md5 = Md5Helper.MD5(account.Number + account.OpenId + Request.UserHostAddress + Request.Browser.Type + Request.Browser.ClrVersion.ToString() + "2017", 16);
                    string str = account.Number + "&" + account.OpenId + "&" + Request.UserHostAddress + "&" + Request.Browser.Type
                        + "&" + Request.Browser.ClrVersion.ToString() + "&" + Md5;
                    str = Utilities.DESEncrypt.Encrypt(str);
                    CookieHelper.WriteCookie("WebUserInfo", str);
                    //获取cookie并判断是否有跳转URL
                    HttpCookie cookie = Request.Cookies[LoginReturnUrlCookieName];
                    string returnUrl = "";
                    if (cookie != null)
                    {
                        returnUrl = cookie.Value;
                        DeleteCookie(LoginReturnUrlCookieName);
                    }

                    return Json(new { res = "Ok", Msg = "登录成功", returnUrl = returnUrl });
                }
                else
                {
                    Json(new { res = "No", msg = "用户名错误" });
                }
            }
            return Json(new { res = "No", msg = "登录失败" });
        }
        public void DeleteCookie(string cookieName)
        {
            //清空特定的Cookie
            HttpCookie cookie = new HttpCookie(cookieName, "") { Expires = new DateTime(1999, 1, 1) };
            System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
            System.Web.HttpContext.Current.Request.Cookies.Remove(cookieName);
        }

        public ActionResult GetCode(string Phone)
        {
            if (StringHelper.IsNullOrEmpty(Phone) || Phone.Length < 11)
            {
                return Json(new { res = "No", msg = "手机号码错误" });
            }
            //发短信接口
            Random r = new Random();
            string rstr = r.Next(1010, 9999).ToString();
            Qcloud.Sms.SmsSingleSender sendSms = new Qcloud.Sms.SmsSingleSender(1400035202, "8f01b47120a413a0c2315eca0a5c1ad3");
            Qcloud.Sms.SmsSingleSenderResult sendResult = new Qcloud.Sms.SmsSingleSenderResult();
            sendResult = sendSms.Send(0, "86", Phone, "您的验证码为：" + rstr + "，请于5分钟内填写。如非本人操作，请忽略本短信。", "", "");
            if (sendResult.result.Equals(0))//到时换为判断是否发送成功
            {
                string str = Utilities.DESEncrypt.Encrypt(rstr);
                CookieHelper.WriteCookie("WebCode", str);
                return Json(new { res = "Ok", msg = "发送成功" });
            }
            else
            {
                return Json(new { res = "No", msg = "发送失败" });
            }
        }
    }
}