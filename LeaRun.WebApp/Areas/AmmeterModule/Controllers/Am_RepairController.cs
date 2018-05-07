/*
* ����:gxlbang
* ����:Am_Repair
* CLR�汾��
* ����ʱ��:2018-04-17 19:07:30
* ��������:
*
* �޸���ʷ��
*
* ����������������������������������������������������������������������������������������������������������������������������������������������������
* ��            Copyright(c) gxlbang ALL rights reserved                    ��
* ����������������������������������������������������������������������������������������������������������������������������������������������������
*/
using LeaRun.Business;
using LeaRun.Entity;
using LeaRun.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeaRun.WebApp.Areas.AmmeterModule.Controllers
{
    /// <summary>
    /// Am_Repair������
    /// </summary>
    public class Am_RepairController : PublicController<Am_Repair>
    {
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public ActionResult GridPageListJson(JqGridParam jqgridparam, string keywords,
            [DefaultValue(-1)]int Status, string ProvinceId, string CityId, string CountyId, string BeginTime, string EndTime)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                Am_RepairBll bll = new Am_RepairBll();
                var ListData = bll.GetPageList(ref jqgridparam, keywords, Status, ProvinceId, CityId, CountyId, BeginTime, EndTime);
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
        /// ��ȡ����ͼƬ
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public ActionResult GetAm_RepairImage(string KeyValue)
        {
            Am_RepairBll bll = new Am_RepairBll();
            return Json(bll.GetImagePageList(KeyValue));
        }
        /// <summary>
        /// ��ñ��޷���
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public ActionResult GetAm_RepairAnswer(string KeyValue)
        {
            Am_RepairBll bll = new Am_RepairBll();
            return Json(bll.GetPageModel(KeyValue));
        }
        /// <summary>
        /// ���ݵ���
        /// </summary>
        public void ExportExcel(string keywords,
            [DefaultValue(-1)]int Status, string ProvinceId, string CityId, string CountyId, string BeginTime, string EndTime)
        {
            Am_RepairBll bll = new Am_RepairBll();
            var ListData = bll.GetPageList(keywords, Status, ProvinceId, CityId, CountyId, BeginTime, EndTime);
            var newlist = new List<Am_RepairNew>();
            foreach (var item in ListData)
            {
                var model = new Am_RepairNew();
                model.Address = item.Address;
                model.AmmeterCode = item.AmmeterCode;
                model.Cell = item.Cell;
                model.City = item.City;
                model.County = item.County;
                model.Floor = item.Floor;
                model.CreateTime = item.CreateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                model.Province = item.Province;
                model.Room = item.Room;
                model.StatusStr = item.StatusStr;
                model.F_Name = item.F_Name;
                model.F_UserName = item.F_UserName;
                model.UserName = item.UserName;
                model.U_Name = item.U_Name;

                newlist.Add(model);
            }
            string[] columns = new string[] { "�����:AmmeterCode", "�⻧:UserName", "�⻧����:U_Name",
                "ҵ��:F_UserName","ҵ������:F_Name","״̬:StatusStr","����ʱ��:CreateTime",
                "ʡ:Province","��:City", "��:County", "��ַ:Address","��Ԫ:Cell","¥��:Floor","����:Room" };
            DeriveExcel.ListToExcel<Am_RepairNew>(newlist, columns, "��������" + DateTime.Now.ToString("yyyyMMddHHmmss"));

        }
    }
}