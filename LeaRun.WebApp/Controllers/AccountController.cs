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
    public class AccountController: Controller
    {
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(Register model)
        {
            if (ModelState.IsValid)
            {
                string realCode = Utilities.DESEncrypt.Decrypt(CookieHelper.GetCookie("WebCode"));
                if (StringHelper.IsNullOrEmpty(model.ValidCode) || model.ValidCode != realCode)
                {
                    return Json(new { res = "On", msg = "验证码错误！" });
                }
                IDatabase database = DataFactory.Database();
                var account = database.FindEntityByWhere<Ho_PartnerUser>(" and 1=1");
                if (account!=null )
                {
                    return Json(new { res = "On", msg = "已存在用户！" });
                }
                var insertModel = new Ho_PartnerUser();
                insertModel.Accout = model.Name;
                insertModel.Password = model.Password;


                return Json(new { res = "Ok", msg = "注册成功" });
            }
            return View();
        }
    }
}