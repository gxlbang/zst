/*
* ����:gxlbang
* ����:Am_AmmeterMoney
* CLR�汾��
* ����ʱ��:2018-04-14 10:54:41
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
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeaRun.WebApp.Areas.AmmeterModule.Controllers
{
    /// <summary>
    /// Am_AmmeterMoney������
    /// </summary>
    public class Am_AmmeterMoneyController : PublicController<Am_AmmeterMoney>
    {
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public ActionResult GridPageListJson(JqGridParam jqgridparam, string Keyword)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                Am_AmmeterMoneyBll bll = new Am_AmmeterMoneyBll();
                var ListData = bll.GetPageList(ref jqgridparam, Keyword);
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
        public ActionResult Edit()
        {
            return View();
        }
        /// <summary>
        /// �ύ��
        /// </summary>
        /// <param name="KeyValue">����ֵ</param>
        /// <param name="pclass">��Ŀ��Ϣ</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SubmitUserForm(string KeyValue, Am_AmmeterMoney model, string BuildFormJson)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = KeyValue == "" ? "�����ɹ���" : "�༭�ɹ���";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    model.Modify(KeyValue);
                    var IsOk = database.Update(model, isOpenTrans);
                    Base_SysLogBll.Instance.WriteLog(KeyValue, OperationType.Update, IsOk > 0 ? "�ɹ�" : "ʧ��", "���" + Message);
                }
                else //�½�
                {
                    model.Create();
                    var IsOk = database.Insert(model, isOpenTrans);
                    Base_SysLogBll.Instance.WriteLog(KeyValue, OperationType.Update, IsOk > 0 ? "�ɹ�" : "ʧ��", "���" + Message);
                }
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "����ʧ�ܣ�" + ex.Message }.ToString());
            }
        }
        /// <summary>
        /// ���ݵ���
        /// </summary>
        public void ExportExcel(string Keyword)
        {
            Am_AmmeterMoneyBll bll = new Am_AmmeterMoneyBll();
            var ListData = bll.GetPageList(Keyword);
            var newlist = new List<Am_AmmeterMoneyNew>();
            foreach (var item in ListData)
            {
                var model = new Am_AmmeterMoneyNew();
                model.Classify = item.Classify.Value==0?"�����ʵ��":"����";
                model.FirstMoney = item.FirstMoney.Value.ToString("0.00");
                model.Name = item.Name;
                model.Remark = item.Remark;
                model.UserName = item.UserName;
                model.UserRealName = item.UserRealName;

                newlist.Add(model);
            }
            string[] columns = new string[] { "����:Name", "�۸�����:Classify", "����(Ԫ/kwh):FirstMoney", "ҵ������:UserRealName",
                "ҵ���ʺ�:UserName", "��ע:Remark" };
            DeriveExcel.ListToExcel<Am_AmmeterMoneyNew>(newlist, columns, "�������" + DateTime.Now.ToString("yyyyMMddHHmmss"));

        }
    }
}