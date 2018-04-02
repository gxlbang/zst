using LeaRun.Business;
using LeaRun.DataAccess;
using LeaRun.Entity;
using LeaRun.Repository;
using LeaRun.Utilities;
using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Web.Mvc;

namespace LeaRun.WebApp.Areas.ProductModule.Controllers
{
    /// <summary>
    /// Fx_WebConfig������
    /// </summary>
    public class WebConfigController : PublicController<Fx_WebConfig>
    {

        Fx_WebConfigBll bll = new Fx_WebConfigBll();

        /// <summary>
        /// �����á�������վ�����б�JSON
        /// </summary>
        /// <param name="jqgridparam">������</param>
        /// <returns></returns>
        public ActionResult GridPageListJson(JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable ListData = bll.GetPageList(ref jqgridparam);
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = ListData,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "�쳣����" + ex.Message);
                return null;
            }
        }      
    }
}