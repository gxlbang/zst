using LeaRun.DataAccess;
using LeaRun.Entity;
using LeaRun.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeaRun.WebApp.Controllers
{
    [UserLoginFilters]
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
            if (account!=null )
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
                if (statu>0)
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
            if (account!=null )
            {
                return View(account);
            }
            return View();
        }

    }
}
