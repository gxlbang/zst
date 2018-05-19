/*
* ����:gxlbang
* ����:Am_Collector
* CLR�汾��
* ����ʱ��:2018-04-15 17:09:58
* ��������:
*
* �޸���ʷ��
*
* ����������������������������������������������������������������������������������������������������������������������������������������������������
* ��            Copyright(c) gxlbang ALL rights reserved                    ��
* ����������������������������������������������������������������������������������������������������������������������������������������������������
*/
using LeaRun.Business;
using LeaRun.DataAccess;
using LeaRun.Entity;
using LeaRun.Repository;
using LeaRun.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace LeaRun.WebApp.Areas.AmmeterModule.Controllers
{
    /// <summary>
    /// Am_Collector������
    /// </summary>
    public class Am_CollectorController : PublicController<Am_Collector>
    {
        /// <summary>
        /// ɾ�����
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public ActionResult DeleteCollector(string KeyValue)
        {
            var Message = "ɾ��ʧ�ܡ�";
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                var acount = database.FindCountBySql("select count(*) from Am_Ammeter where Collector_Number = '" + KeyValue + "' and Status != 9");
                if (acount > 0)
                {
                    Message = "������ʹ�õĵ��,����ɾ��!";
                    WriteLog(1, KeyValue, Message);
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = Message }.ToString());
                }
                else
                {
                    var model = database.FindEntity<Am_Collector>(KeyValue);
                    if (model == null && string.IsNullOrEmpty(model.Number))
                    {
                        Message = "�����쳣";
                    }
                    model.STATUS = 9;
                    model.StatusStr = "��ɾ��";
                    model.Modify(model.Number);
                    if (database.Update(model, isOpenTrans) < 1)
                    {
                        isOpenTrans.Rollback();
                    }
                    isOpenTrans.Commit();
                    WriteLog(1, KeyValue, Message);
                    return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
                }
            }
            catch (Exception ex)
            {
                isOpenTrans.Rollback();
                WriteLog(-1, KeyValue, "����ʧ�ܣ�" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "����ʧ�ܣ�" + ex.Message }.ToString());
            }
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public ActionResult GridPageListJson(JqGridParam jqgridparam, string keywords, [DefaultValue(-1)]int Stuts, string ProvinceId, string CityId, string CountyId)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                Am_CollectorBll bll = new Am_CollectorBll();
                var ListData = bll.GetPageList(ref jqgridparam, keywords, Stuts, ProvinceId, CityId, CountyId);
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = ListData
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "�쳣����" + ex.Message);
                return null;
            }
        }
        /// <summary>
        /// ���ݵ���
        /// </summary>
        public void ExportExcel([DefaultValue(-1)]int Stuts, string keywords, string ProvinceId, string CityId, string CountyId)
        {
            Am_CollectorBll bll = new Am_CollectorBll();
            var ListData = bll.GetPageList(keywords, Stuts, ProvinceId, CityId, CountyId);
            var newlist = new List<Am_CollectorNew>();
            foreach (var item in ListData)
            {
                var model = new Am_CollectorNew();
                model.Address = item.Address;
                model.AmCount = item.AmCount.ToString();
                model.City = item.City;
                model.CollectorCode = item.CollectorCode;
                model.County = item.County;
                model.CreateTime = item.CreateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                model.LastConnectTime = item.LastConnectTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                model.Province = item.Province;
                model.Remark = GetDays(item.LastConnectTime.Value) + "��";
                model.StatusStr = item.StatusStr;
                model.URealName = item.URealName;
                model.UserName = item.UserName;

                newlist.Add(model);
            }
            string[] columns = new string[] { "ҵ���ʺ�:UserName", "ҵ������:URealName", "�ɼ������:CollectorCode", "ʡ:Province",
                "��:City", "��:County", "��ַ:Address", "״̬:StatusStr","�����:AmCount", "����ʱ��:CreateTime", "�ϴ�����:LastConnectTime",
                "�ۻ�����ʱ��:Remark" };
            DeriveExcel.ListToExcel<Am_CollectorNew>(newlist, columns, "��Ա����" + DateTime.Now.ToString("yyyyMMddHHmmss"));

        }

        public int GetDays(DateTime date)
        {
            var time = DateTime.Now - date;
            var day = time.Days;
            return day;
        }

        public ActionResult AddCollect()
        {
            AmmeterSDK.MainApi api = new AmmeterSDK.MainApi();
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            Dictionary<string, object> paramssMap = new Dictionary<string, object>();
            paramssMap.Add("cid", "18015210009");
            list.Add(paramssMap);
            return Json(api.Request(AmmeterSDK.ApiUrl.COLLECTORADD, list, false));
        }
    }
}