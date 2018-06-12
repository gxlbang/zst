/*
* ����:gxlbang
* ����:Am_UserDeposit
* CLR�汾��
* ����ʱ��:2018-04-17 19:09:05
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
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeaRun.WebApp.Areas.AmmeterModule.Controllers
{
    /// <summary>
    /// Am_UserDeposit������
    /// </summary>
    public class Am_UserDepositController : PublicController<Am_UserDeposit>
    {
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public ActionResult GridPageListJson(JqGridParam jqgridparam, string keywords, int Stuts, string StartTime, string EndTime)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                Am_UserDepositBll bll = new Am_UserDepositBll();
                var ListData = bll.GetPageList(ref jqgridparam, keywords, Stuts, StartTime, EndTime);
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
        public void ExportExcel(string keywords, int Stuts, string StartTime, string EndTime)
        {
            Am_UserDepositBll bll = new Am_UserDepositBll();
            var ListData = bll.GetPageList(keywords, Stuts, StartTime, EndTime);
            var newlist = new List<Am_UserDepositNew>();
            foreach (var item in ListData)
            {
                var model = new Am_UserDepositNew();
                model.Address = item.Address;
                model.Ammeter_Code = item.Ammeter_Code;
                model.Cell = item.Cell;
                model.City = item.City;
                model.County = item.County;
                model.Floor = item.Floor;
                model.Province = item.Province;
                model.Room = item.Room;
                model.CreateTime = item.CreateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                model.Money = item.Money.Value.ToString("0.00");
                model.StatusStr = item.StatusStr;
                model.UserName = item.UserName;
                model.U_Name = item.U_Name;

                newlist.Add(model);
            }
            string[] columns = new string[] { "�û���:UserName", "����:U_Name", "�����:Ammeter_Code",
                "ʡ:Province","��:City","����:County","��ַ:Address","��Ԫ:Cell","¥��:Floor",
                "����:Room", "ʱ��:CreateTime","״̬:StatusStr","Ѻ��:Money" };
            DeriveExcel.ListToExcel<Am_UserDepositNew>(newlist, columns, "Ѻ����Ϣ��" + DateTime.Now.ToString("yyyyMMddHHmmss"));

        }
    }
}