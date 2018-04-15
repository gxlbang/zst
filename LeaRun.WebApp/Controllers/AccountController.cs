using Extensions;
using LeaRun.DataAccess;
using LeaRun.DataAccess.Common;
using LeaRun.Entity;
using LeaRun.Repository;
using LeaRun.Utilities;
using LeaRun.WebApp.ViewsModel.Account;
using System;
using System.Collections.Generic;
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
                    return Json(new { res = "On", msg = "验证码错误！" });
                }
                var account = database.FindEntity<Ho_PartnerUser>(" and Mobile=" + model.Name);
                if (account != null&&account.Number!=null )
                {
                    return Json(new { res = "On", msg = "已存在用户！" });
                }
                var insertModel = new Ho_PartnerUser();
                insertModel.Number = CommonHelper.GetGuid;
                insertModel.Password = PasswordHash.CreateHash(model.Password);
                insertModel.Mobile = model.Name;
                insertModel.CreatTime = DateTime.Now;
                insertModel.ModifyTime = DateTime.Now;
                insertModel.SureTime = DateTime.Now;
                insertModel.Money = 0.00;
                insertModel.FreezeMoney = 0.00;
                insertModel.Status = 0;
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
            return Json(new { res = "On", msg = "注册失败！" });
        }


        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string name, string pwd, string code)
        {
            if (ModelState.IsValid)
            {
                var account = database.FindEntityByWhere<Ho_PartnerUser>(" and Mobile=" + name);
                if (account != null && PasswordHash.ValidatePassword(pwd, account?.Password))
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
                    Json(new { res = "On", msg = "用户名或密码不对！" });
                }
            }
            return Json(new { res = "On", msg = "登录失败！" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string name, string validCode)
        {
            if (ModelState.IsValid)
            {
                string realCode = Utilities.DESEncrypt.Decrypt(CookieHelper.GetCookie("WebCode"));
                if (StringHelper.IsNullOrEmpty(validCode) || validCode != realCode)
                {
                    return Json(new { res = "On", msg = "验证码错误！" });
                }
                var account = database.FindEntityByWhere<Ho_PartnerUser>(" and Mobile=" + name);
                if (account != null)
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
                    Json(new { res = "On", msg = "用户名错误！" });
                }
            }
            return Json(new { res = "On", msg = "登录失败！" });
        }
        public void DeleteCookie(string cookieName)
        {
            //清空特定的Cookie
            HttpCookie cookie = new HttpCookie(cookieName, "") { Expires = new DateTime(1999, 1, 1) };
            System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
            System.Web.HttpContext.Current.Request.Cookies.Remove(cookieName);
        }
    }
}