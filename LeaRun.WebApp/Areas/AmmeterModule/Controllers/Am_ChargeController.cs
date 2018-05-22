/*
* ����:gxlbang
* ����:Am_Charge
* CLR�汾��
* ����ʱ��:2018-04-17 10:13:57
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
    /// Am_Charge������
    /// </summary>
    public class Am_ChargeController : PublicController<Am_Charge>
    {
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public ActionResult GridPageListJson(JqGridParam jqgridparam, string keywords,
            int Status, int ChargeType, string BeginTime, string EndTime)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                Am_ChargeBll bll = new Am_ChargeBll();
                var ListData = bll.GetPageList(ref jqgridparam, keywords, Status, ChargeType, BeginTime, EndTime);
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
        public void ExportExcel(string keywords,
            [DefaultValue(-1)]int Status, int ChargeType, string BeginTime, string EndTime)
        {
            Am_ChargeBll bll = new Am_ChargeBll();
            var ListData = bll.GetPageList(keywords, Status, ChargeType, BeginTime, EndTime);
            var newlist = new List<Am_ChargeNew>();
            foreach (var item in ListData)
            {
                var model = new Am_ChargeNew();
                model.AmmeterCode = item.AmmeterCode;
                model.ChargeTypeStr = item.ChargeTypeStr;
                model.CreateTime = item.CreateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                model.Money = item.Money.Value.ToString("0.00");
                model.OrderNumber = item.OrderNumber;
                model.OutNumber = item.OutNumber;
                model.PayType = item.PayType;
                model.StatusStr = item.StatusStr;
                model.SucTime = item.SucTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                model.UserName = item.UserName;
                model.U_Name = item.U_Name;
                newlist.Add(model);
            }
            string[] columns = new string[] { "���:OrderNumber", "�ⲿ���:OutNumber", "����:AmmeterCode",
                "�û�:UserName","����:U_Name","��ֵ����:ChargeTypeStr","֧����ʽ:PayType","��ֵ���:Moeny","״̬:StatusStr",
                "��ֵʱ��:CreateTime","�ɹ�ʱ��:SucTime" };
            DeriveExcel.ListToExcel<Am_ChargeNew>(newlist, columns, "��ֵ����" + DateTime.Now.ToString("yyyyMMddHHmmss"));

        }
    }
}