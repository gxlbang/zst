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
    public class ProprietorController : Controller
    {
        IDatabase database = DataFactory.Database();
        WebData wbll = new WebData();
        //
        // GET: /Proprietor/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Ammeter(string Number)
        {
            var user = wbll.GetUserInfo(Request);

            var ammeterTpyeList = database.FindList<Am_AmmeterType>("");

            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@UNumber", user.Number));
            var collectorList = database.FindList<Am_Collector>(" and UNumber=@UNumber", parameter.ToArray());

            List<DbParameter> par1 = new List<DbParameter>();
            par1.Add(DbFactory.CreateDbParameter("@UserNumber", user.Number));
            var ammeterMoneyList = database.FindList<Am_AmmeterMoney>(" and UserNumber=@UserNumber", par1.ToArray());
            if (Number!=null && Number!="")
            {
               
            }
            else
            {
                List<DbParameter> par2 = new List<DbParameter>();
                par2.Add(DbFactory.CreateDbParameter("@UY_Number", user.Number));
                par2.Add(DbFactory.CreateDbParameter("@Number", Number));
                var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and UY_Number=@UY_Number and Number=@Number", par2.ToArray());
                if (ammeter!=null && ammeter.Number!=null )
                {
                    return View(ammeter);
                }
            }
            return View();
        }
        [HttpPost]
        public ActionResult AmmeterEidt(Am_Ammeter model)
        {
            var user = wbll.GetUserInfo(Request);
            if (model.Number !=null&& model.Number !="")
            {
                List<DbParameter> par2 = new List<DbParameter>();
                par2.Add(DbFactory.CreateDbParameter("@UY_Number", user.Number));
                par2.Add(DbFactory.CreateDbParameter("@Number", model.Number));
                var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and UY_Number=@UY_Number and Number=@Number", par2.ToArray());
                if (ammeter != null && ammeter.Number != null)
                {
                    ammeter.AmmeterType_Number = model.AmmeterType_Number;
                    ammeter.AmmeterType_Name = model.AmmeterType_Name;
                    ammeter.AmmeterType_Name = model.AmmeterType_Name;
                    ammeter.AmmeterType_Number = model.AmmeterType_Number;
                    ammeter.Address = model.Address;
                    ammeter.AllMoney = model.AllMoney;
                    ammeter.AM_Code = model.AM_Code;
                    ammeter.Cell = model.Cell;
                    ammeter.City = model.City;
                    ammeter.Collector_Code = model.Collector_Code;
                    ammeter.Collector_Number = model.Collector_Number;
                    ammeter.County = model.County;
                    ammeter.CurrMoney = model.CurrMoney;
                    ammeter.CurrPower = model.CurrPower;
                    ammeter.FirstAlarm = model.FirstAlarm;
                    ammeter.Floor = model.Floor;
                    ammeter.HGQBB = model.HGQBB;
                    ammeter.Province = model.Province;
                    ammeter.Room = model.Room;
                    ammeter.SencondAlarm = model.SencondAlarm;
                    ammeter.UpdateTime = DateTime.Now;
                    ammeter.UserName = model.UserName;
                    ammeter.U_Name = model.U_Name;
                    ammeter.U_Number = model.U_Number;
                    ammeter.Remark = model.Remark;

                    var status = database.Update<Am_Ammeter>(ammeter);
                    if (status>0)
                    {
                        return Json(new { res = "Ok", msg = "修改成功" });
                    }
                }
                else
                {

                }
            }
            return Json(new { res = "Ok", msg = "修改成功" });
        }
    }
}
