using LeaRun.DataAccess;
using LeaRun.Entity;
using LeaRun.Repository;
using LeaRun.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeaRun.WebApp.Controllers
{
    public class CommonController : Controller
    {
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
            IDatabase database = DataFactory.Database();
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
            IDatabase database = DataFactory.Database();
            var list = database.FindList<Am_ContractImage>(" AND AC_Number = '" + KeyValue + "'");
            return Content(list.ToJson());
        }
    }
}
