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
    public class CommonController : Controller
    {
        IDatabase database = DataFactory.Database();
        /// <summary>
        /// 合同查看
        /// </summary>
        /// <returns></returns>
        public ActionResult Contract()
        {
            return View();
        }
        /// <summary>
        /// 合同查看
        /// </summary>
        /// <returns></returns>
        public ActionResult GetContract(string KeyValue)
        {
            var model = database.FindEntity<Am_Contract>(KeyValue);
            return Content(model.ToJson());
        }
        /// <summary>
        /// 获取合同附件
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public ActionResult GetContractImage(string KeyValue)
        {
            var list = database.FindList<Am_ContractImage>(" AND AC_Number = '" + KeyValue + "'");
            return Content(list.ToJson());
        }
        public ActionResult Template(string ammeterNumber)
        {
            return View();
        }
        public ActionResult ContractTemplate(string ammeterNumber)
        {
            List<DbParameter> par = new List<DbParameter>();
            par.Add(DbFactory.CreateDbParameter("@AmmeterNumber", ammeterNumber));

            var ct = database.FindEntityByWhere<Am_ContractTemplate>(" and AmmeterNumber=@AmmeterNumber ", par.ToArray());
            if (ct == null || ct.Number == null)
            {
                //电表用户关系
                List<DbParameter> par1 = new List<DbParameter>();
                par1.Add(DbFactory.CreateDbParameter("@Ammeter_Number", ammeterNumber));
                par1.Add(DbFactory.CreateDbParameter("@Status", "1"));
                var ap = database.FindEntityByWhere<Am_AmmeterPermission>(" and Ammeter_Number=@Ammeter_Number and Status=@Status", par1.ToArray());

                //电表
                List<DbParameter> par2 = new List<DbParameter>();
                par2.Add(DbFactory.CreateDbParameter("@Number", ammeterNumber));
                var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and Number=@Number", par2.ToArray());

                //电表模板
                List<DbParameter> par3 = new List<DbParameter>();
                par3.Add(DbFactory.CreateDbParameter("@Ammeter_Number", ammeterNumber));
                var template = database.FindEntityByWhere<Am_Template>(" and Ammeter_Number=@Ammeter_Number", par3.ToArray());

                //电表模板内容
                List<DbParameter> par4 = new List<DbParameter>();
                par4.Add(DbFactory.CreateDbParameter("@Template_Number", template.Number));
                var content = database.FindList<Am_TemplateContent>(" and Template_Number=@Template_Number", par4.ToArray());

                ct = new Am_ContractTemplate
                {
                    Number = null,
                    AmmeterNumber = ammeter.Number,
                    Address = ammeter.Address,
                    AmmeterCode = ammeter.AM_Code,
                    Am_Money = null,
                    BackMoney = null,
                    BackTime = null,
                    BankCode = null,
                    BankInfo = null,
                    BankUserName = null,
                    Cell = ammeter.Cell,
                    City = ammeter.City,
                    ContractPaht = null,
                    County = ammeter.County,
                    CreateTime = DateTime.Now,
                    CycleTime = ap.BillCyc,
                    DepositMoney = content.FirstOrDefault(o => o.ChargeItem_Title == "押金").Money,
                    Money = content.FirstOrDefault(o => o.ChargeItem_Title == "租金").Money,
                    DepositMoneyStr = RMBHelper.CmycurD(decimal.Parse(content.FirstOrDefault(o => o.ChargeItem_Title == "押金").Money.Value.ToString("0.00"))),
                    Floor = ammeter.Floor,
                    GarbageMoney = null,
                    GoMoney = null,
                    GoOnTime = null,
                    GoTime = null,
                    HallNum = null,
                    HoName = null,
                    HoUserMobile = ammeter.UserName,
                    UserName = ammeter.UserName,
                    HoUserName = ammeter.U_Name,
                    U_Name = ammeter.U_Name,
                    HouseSize = null,
                    KitchenNum = null,
                    MoneyStr = RMBHelper.CmycurD(decimal.Parse(content.FirstOrDefault(o => o.ChargeItem_Title == "租金").Money.Value.ToString("0.00"))),
                    NetMoney = null,
                    PenaltyMoney = null,
                    PenaltyTime = null,
                    PropertyMoeny = null,
                    Province = ammeter.Province,
                    Remark = "",
                    RentMoneyTime = null,
                    Room = ammeter.Room,
                    RoomNum = null,
                    ToiletNum = null,
                    TotalMoney = content.FirstOrDefault(o => o.ChargeItem_Title == "租金").Money,
                    TotalMoneyStr = RMBHelper.CmycurD(decimal.Parse(content.FirstOrDefault(o => o.ChargeItem_Title == "租金").Money.Value.ToString("0.00"))),
                    Useing = null,
                    UseingSize = null,
                    U_Number = ammeter.U_Number,
                    WaterMoney = null
                };
            }
            return Content(ct.ToJson());
        }
    }
}
