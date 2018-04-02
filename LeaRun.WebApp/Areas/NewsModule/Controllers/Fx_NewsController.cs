/*
* ����:gxlbang
* ����:Fx_News
* CLR�汾��
* ����ʱ��:2017-11-27 16:13:57
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

namespace LeaRun.WebApp.Areas.NewsModule.Controllers
{
    /// <summary>
    /// Fx_News������
    /// </summary>
    public class Fx_NewsController : PublicController<Fx_News>
    {
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <returns></returns>
        public ActionResult GridPageListJson(JqGridParam jqgridparam, string Keyword, string Number)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                Fx_NewsBll bll = new Fx_NewsBll();
                var ListData = bll.GetPageList(ref jqgridparam, Keyword, Number);
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
        /// ������Ѷ��ʾλ��
        /// </summary>
        /// <param name="Number"></param>
        /// <param name="P"></param>
        /// <returns></returns>
        [HttpPost]
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult SetHot(string Number,string P)
        {
            try
            {
                var Message = "����ʧ�ܡ�";
                int IsOk = 0;
                IDatabase database = DataFactory.Database();
                var model = database.FindEntity<Fx_News>(Number);
                //����״̬Ϊ�෴
                switch (P)
                {
                    case "IsFirst":
                        model.IsFirst = model.IsFirst == 0 ? 1 : 0;
                        break;
                    case "IsHot":
                        model.IsHot = model.IsHot == 0 ? 1 : 0;
                        break;
                    case "IsRec":
                        model.IsRec = model.IsRec == 0 ? 1 : 0;
                        break;
                    case "IsDel":
                        model.IsDel = model.IsDel == 0 ? 1 : 0;
                        break;
                    case "IsShow":
                        model.IsShow = model.IsShow == 0 ? 1 : 0;
                        break;
                    case "IsPic":
                        model.IsPic = model.IsPic == 0 ? 1 : 0;
                        break;
                    case "IsReview":
                        model.IsReview = model.IsReview == 0 ? 1 : 0;
                        break;
                    case "IsPublic":
                        model.IsPublic = model.IsPublic == 0 ? 1 : 0;
                        break;
                    default:
                        break;
                }
                model.Modify(Number);
                IsOk = database.Update(model);
                if (IsOk > 0)
                {
                    Message = "���óɹ���";
                }
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "����ʧ�ܣ�" + ex.Message }.ToString());
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
        public ActionResult SubmitUserForm(string KeyValue, Fx_News model, string BuildFormJson)
        {
            //string ModuleId = DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId"));
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                model.NewsContent = Server.UrlDecode(model.NewsContent);
                string Message = KeyValue == "" ? "�����ɹ���" : "�༭�ɹ���";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    model.StatusStr = model.Status == 1 ? "����" : "����";
                    model.Modify(KeyValue);
                    database.Update(model, isOpenTrans);
                }
                else //�½�
                {
                    model.Create();
                    model.Title = StringHelper.IsNullOrEmpty(model.Title) ? model.NewsName : model.Title;
                    model.NewsKeyword = StringHelper.IsNullOrEmpty(model.NewsKeyword) ? model.NewsName : model.Title;
                    model.NewsDes = StringHelper.IsNullOrEmpty(model.NewsDes) ? model.NewsName : model.Title;
                    model.StatusStr = model.Status == 1 ? "����" : "����";
                    int result = database.Insert(model, isOpenTrans);
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
    }
}