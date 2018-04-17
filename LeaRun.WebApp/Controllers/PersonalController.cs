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
            if (account!=null && account.Number !=null )
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
            if (account!=null &&account .Number!=null )
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
            var account = database.FindEntityByWhere<Ho_HouseInfo>(" and Number='" + user.Number + "'");
            if (account!=null &&account.Number!=null)
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
            var balanDetailList = database.FindListPage<Am_MoneyDetail>("Number", "desc", pageIndex, pageSize,ref recordCount);
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
        public ActionResult BalanceRecharge(double money,int type )
        {
            var user = wbll.GetUserInfo(Request);
            var account = database.FindEntityByWhere<Ho_HouseInfo>(" and Number='" + user.Number + "'");
            if (account != null && account.Number != null)
            {
                 
                 //Am_Charge
            }
            return View();
        }
    }
}
