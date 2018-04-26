/*
* ����:gxlbang
* ����:Am_AmDepositDetail
* CLR�汾��
* ����ʱ��:2018-04-17 19:10:15
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
    /// Am_AmDepositDetail������
    /// </summary>
    public class Am_AmDepositDetailController : PublicController<Am_AmDepositDetail>
    {
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public ActionResult GridPageListJson(JqGridParam jqgridparam, string keywords)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                Am_AmDepositDetailBll bll = new Am_AmDepositDetailBll();
                var ListData = bll.GetPageList(ref jqgridparam, keywords);
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
        public void ExportExcel(string keywords)
        {
            Am_AmDepositDetailBll bll = new Am_AmDepositDetailBll();
            var ListData = bll.GetPageList(keywords);
            var newlist = new List<Am_AmDepositDetailNew>();
            foreach (var item in ListData)
            {
                var model = new Am_AmDepositDetailNew();
                model.CreateTime = item.CreateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                model.CurrMoney = item.CurrMoney.Value.ToString("0.00");
                model.Mark = item.Mark;
                model.Money = item.Money.Value.ToString("0.00");
                model.UserName = item.UserName;
                model.U_Name = item.U_Name;

                newlist.Add(model);
            }
            string[] columns = new string[] { "��Ӫ��:U_Name","�ֻ���:UserName","�������:Money",
                "ʣ����:CurrMoney", "����ʱ��:CreateTime","��ע��Ϣ:Mark" };
            DeriveExcel.ListToExcel<Am_AmDepositDetailNew>(newlist, columns, "Ѻ����ϸ����" + DateTime.Now.ToString("yyyyMMddHHmmss"));

        }
    }
}