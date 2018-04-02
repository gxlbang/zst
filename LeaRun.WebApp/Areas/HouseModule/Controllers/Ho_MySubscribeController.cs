/*
* ����:gxlbang
* ����:Ho_MySubscribe
* CLR�汾��
* ����ʱ��:2017-12-05 11:54:11
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
    /// �ҵ�ԤԼ������
    /// </summary>
    public class Ho_MySubscribeController : PublicController<Ho_MySubscribe>
    {
        /// <summary>
        /// ��ҳ
        /// </summary>
        /// <returns></returns>
        //[ManagerPermission(PermissionMode.Enforce)]
        public override ActionResult Index()
        {
            string _ModuleId = DESEncrypt.Encrypt("3015aec6-3b63-4dff-9c4b-a3c4a67e2148");
            CookieHelper.WriteCookie("ModuleId", _ModuleId);
            return View();
        }
        
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public ActionResult GridPageListJson(JqGridParam jqgridparam, string StartTime, string EndTime,
            string Keyword, string Stuts)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                Ho_MySubscribeBll bll = new Ho_MySubscribeBll();
                var ListData = bll.GetPageList(ref jqgridparam, StartTime, EndTime, Keyword, Stuts);
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
        public ActionResult SubmitUserForm(string KeyValue, Ho_MySubscribe model, string BuildFormJson)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                var oldModel = database.FindEntity<Ho_MySubscribe>(KeyValue);
                if (oldModel == null)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "ԤԼ������" }.ToString());
                }
                string Message = KeyValue == "" ? "�����ɹ���" : "�༭�ɹ���";
                //�����ǰ״̬Ϊ��ԤԼ,����Ϊ�Ӵ���
                if (model.Status == 0)
                {
                    model.Status = 1;
                    model.StatusStr = GetStatusStr(model.Status ?? 0);
                }
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    model.ReTime = DateTime.Now;
                    model.ReUserNumber = ManageProvider.Provider.Current().UserId;
                    model.ReUser = ManageProvider.Provider.Current().Account;
                    model.Modify(KeyValue);
                    var IsOk = database.Update(model, isOpenTrans);
                    //�������Ա����ԤԼ�����������,���ҵ����Ϣ״̬����Ϊ����ԤԼ
                    //�ݲ��ڴ˸���
                    if (IsOk > 0 && model.Status == 9)
                    {
                        //�����ҵ�ҵ��-��ֻ̨�е���ɵ�ʱ����ͷ��û�ҵ��Ϊ����ԤԼ
                        //�����ҵ�ҵ��Ho_MyHouseInfoֻ������״̬,����ԤԼ������ԤԼ
                        var mhmodel = database.FindEntity<Ho_MyHouseInfo>(oldModel.MHNumber);
                        mhmodel.Status = 0;
                        mhmodel.StatusStr = "����ԤԼ";
                        mhmodel.Modify(mhmodel.Number);
                        database.Update(mhmodel, isOpenTrans);
                    }
                    Base_SysLogBll.Instance.WriteLog(KeyValue, OperationType.Update, IsOk > 0 ? "�ɹ�" : "ʧ��", "ԤԼ�Ӵ�����");
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
        /// ����ԤԼ״̬
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public ActionResult SetOrders(string KeyValue, int Status)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                Ho_MySubscribe entity = database.FindEntity<Ho_MySubscribe>(KeyValue);
                entity.Modify(KeyValue);
                entity.Status = Status;
                entity.StatusStr = GetStatusStr(Status);
                int IsOk = database.Update(entity, isOpenTrans);
                //�رպ���ɶ��ͷ�
                if (IsOk > 0 && (Status == 9||Status==2))
                {
                    //�����ҵ�ҵ��-��̨��رպ���ɵ�ʱ����ͷ��û�ҵ��Ϊ����ԤԼ
                    //�����ҵ�ҵ��Ho_MyHouseInfoֻ������״̬,����ԤԼ������ԤԼ
                    var mhmodel = database.FindEntity<Ho_MyHouseInfo>(entity.MHNumber);
                    mhmodel.Status = 0;
                    mhmodel.StatusStr = "����ԤԼ";
                    mhmodel.Modify(mhmodel.Number);
                    database.Update(mhmodel, isOpenTrans);
                }
                database.Commit();
                return Content(new JsonMessage { Success = IsOk > 0, Code = IsOk.ToString(), Message = "����" + (IsOk > 0 ? "�ɹ�" : "ʧ��") }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "����ʧ�ܣ�" + ex.Message }.ToString());
            }
        }

        //��ȡ״̬�ַ�
        public string GetStatusStr(int s)
        {
            switch (s)
            {
                case -1:
                    return "ȡ��ԤԼ";
                case 0:
                    return "���ύ";
                case 1:
                    return "�Ӵ���";
                case 2:
                    return "�ر�ԤԼ";
                case 9:
                    return "�����";
                default:
                    return "δ֪";
            }
        }
    }
}