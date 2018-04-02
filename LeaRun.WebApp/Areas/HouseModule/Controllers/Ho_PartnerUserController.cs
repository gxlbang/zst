/*
* ����:gxlbang
* ����:Ho_PartnerUser
* CLR�汾��
* ����ʱ��:2017-12-05 11:50:47
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

namespace LeaRun.WebApp.Areas.HouseModule.Controllers
{
    /// <summary>
    /// �ϻ��˿�����
    /// </summary>
    public class Ho_PartnerUserController : PublicController<Ho_PartnerUser>
    {
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public ActionResult GridPageListJson(JqGridParam jqgridparam, string keyword)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                Ho_PartnerUserBll bll = new Ho_PartnerUserBll();
                var ListData = bll.GetPageList(ref jqgridparam, keyword);
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
        /// �ύ��
        /// </summary>
        /// <param name="KeyValue">����ֵ</param>
        /// <param name="pclass">��Ŀ��Ϣ</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SubmitUserForm(string KeyValue, Ho_PartnerUser model, string BuildFormJson)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = KeyValue == "" ? "�����ɹ���" : "�༭�ɹ���";
                model.StatusStr = GetStrutsStr(model.Status.Value);
                var result = "���";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    var oldmodel = database.FindEntity<Ho_PartnerUser>(KeyValue);
                    //if (oldmodel != null && oldmodel.Status == 0 && model.Status == 1) //��һ�����
                    //{
                    model.SureTime = DateTime.Now;
                    model.SureUser = ManageProvider.Provider.Current().Account;
                    result = "���";
                    //}
                    //�ύ˽������
                    var asmodel = database.FindEntity<Ho_Assistant>(model.As_Number);
                    if (asmodel != null)
                    {
                        model.As_Name = asmodel.Name;
                    }
                    model.Modify(KeyValue);
                    var IsOk = database.Update(model, isOpenTrans);
                    Base_SysLogBll.Instance.WriteLog(KeyValue, OperationType.Update, IsOk > 0 ? "�ɹ�" : "ʧ��", "�ϻ���" + result);
                }
                else //�½�
                {
                    model.Create();
                    database.Insert(model, isOpenTrans);
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
        /// ˽�������б�
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAssistant()
        {
            IDatabase database = DataFactory.Database();
            var list = database.FindList<Ho_Assistant>();
            return Content(list.ToJson());
        }
        //���úϻ���
        public ActionResult DisableUser(string KeyValue) {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = KeyValue == "" ? "�����ɹ���" : "�༭�ɹ���";
                var result = "����";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    var oldmodel = database.FindEntity<Ho_PartnerUser>(KeyValue);
                    oldmodel.Status = 9;
                    oldmodel.StatusStr = "������";
                    oldmodel.Modify(KeyValue);
                    var IsOk = database.Update(oldmodel, isOpenTrans);
                    Base_SysLogBll.Instance.WriteLog(KeyValue, OperationType.Update, IsOk > 0 ? "�ɹ�" : "ʧ��", "�ϻ���" + result);
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

        //״̬�ַ�ת��
        public string GetStrutsStr(int struts)
        {
            switch (struts)
            {
                case 0:
                    return "�ο�";
                case 1:
                    return "�����";
                case 2:
                    return "δ��֤";
                case 3:
                    return "����֤";
                case 9:
                    return "������";
                default:
                    return "δ֪";
            }
        }
    }
}