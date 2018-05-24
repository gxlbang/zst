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
        public ActionResult Template()
        {
            return View();
        }
        public ActionResult ContractTemplate(string  ammeterNumber)
        {
            List<DbParameter> par = new List<DbParameter>();
            par.Add(DbFactory.CreateDbParameter("@AmmeterNumber", ammeterNumber));

            var ct = database.FindEntityByWhere<Am_ContractTemplate>(" and AmmeterNumber=@AmmeterNumber ",par.ToArray());
            if (ct!=null &&ct.Number !=null )
            {
                
            }
            return Content(ct.ToJson());
        }
    }
}
