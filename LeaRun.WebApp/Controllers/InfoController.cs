using Extensions;
using LeaRun.DataAccess;
using LeaRun.Entity;
using LeaRun.Repository;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeaRun.WebApp.Controllers
{
    public class InfoController : Controller
    {
        IDatabase database = DataFactory.Database();
        WebData wbll = new WebData();
        //
        // GET: /Info/

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
        /// 个人信息
        /// </summary>
        /// <returns></returns>
        public ActionResult Information()
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@Number", user.Number));

            var account = database.FindEntityByWhere<Ho_PartnerUser>(" and Number=@Number", parameter.ToArray());
            if (account != null && account.Number != null)
            {
                return View(account);
            }
            return View();
        }

        /// <summary>
        /// 提交完善信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Perfect(Ho_PartnerUser model, string ConfirmPayPassword)
        {
            var user = wbll.GetUserInfo(Request);
            if (user != null && user.Number != null)
            {
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@Number", user.Number));

                var account = database.FindEntityByWhere<Ho_PartnerUser>(" and Number=@Number", parameter.ToArray());
                if (account != null && account.Number != null)
                {
                    if (model.PayPassword != ConfirmPayPassword)
                    {
                        return Json(new { res = "No", msg = "密码不一致" });
                    }
                    account.PayPassword = PasswordHash.CreateHash(model.PayPassword);
                    account.Name = model.Name;
                    account.Sex = model.Sex;
                    account.CardCode = model.CardCode;
                    account.CodeImg1 = model.CodeImg1;
                    account.CodeImg2 = model.CodeImg2;
                    account.Address = model.Address;
                    account.Status = 1;
                    var statu = database.Update<Ho_PartnerUser>(account);
                    if (statu > 0)
                    {
                        return Json(new { res = "Ok", msg = "提交成功" });
                    }
                }
            }

            return Json(new { res = "No", msg = "提交失败" });
        }

    }
}
