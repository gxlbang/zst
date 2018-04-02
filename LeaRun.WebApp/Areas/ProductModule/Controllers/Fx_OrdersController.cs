/*
 * ����:gxlbang
 * ����:IISWorker
 * CLR�汾��4.0.30319.42000
 * ����ʱ��:2017/11/1 16:52:09
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

namespace LeaRun.WebApp.Areas.ProductModule.Controllers
{
    /// <summary>
    /// Fx_Orders������
    /// </summary>
    public class Fx_OrdersController : PublicController<Fx_Orders>
    {
        Fx_OrdersBll bll = new Fx_OrdersBll();

        /// <summary>
        /// ���ز�Ʒ��Ŀ�б�JSON
        /// </summary>
        /// <param name="jqgridparam">������</param>
        /// <returns></returns>
        public ActionResult GridPageListJson(JqGridParam jqgridparam, string StartTime, string EndTime,
            string Keyword, string Stuts, [DefaultValue(0)]int IsAll)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                //DataTable ListData = bll.GetPageList(ref jqgridparam);
                var ListData = bll.GetPageList1(ref jqgridparam, StartTime, EndTime, Keyword, Stuts, IsAll);
                var newlist = new List<Fx_Orders>();
                foreach (var item in ListData)
                {
                    item.Arddress = item.Province + item.City + item.County + item.Arddress;

                    newlist.Add(item);
                }
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = newlist
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
        /// <param name="entity">ʵ�����</param>
        /// <param name="KeyValue">����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [LoginAuthorize]
        public virtual ActionResult SubmitOrderForm(Fx_Orders entity, string KeyValue)
        {
            try
            {
                int IsOk = 0;
                string Message = KeyValue == "" ? "�����ɹ���" : "�༭�ɹ���";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    Fx_Orders Oldentity = repositoryfactory.Repository().FindEntity(KeyValue);//��ȡû����֮ǰʵ�����
                    entity.Modify(KeyValue);
                    //�����˿�ݵ��ż�����,�Ҷ���״̬��δ����״̬
                    if (!string.IsNullOrEmpty(entity.Ex_NO) && entity.Stuts == 2)
                    {
                        entity.Ex_Time = DateTime.Now;
                        entity.Ex_UserId = ManageProvider.Provider.Current().UserId;
                    }
                    entity.Resutl = GetOrderResult(entity.Stuts??0);
                    IsOk = repositoryfactory.Repository().Update(entity);
                    this.WriteLog(IsOk, entity, Oldentity, KeyValue, Message);
                }
                else
                {
                    entity.Create();
                    IsOk = repositoryfactory.Repository().Insert(entity);
                    this.WriteLog(IsOk, entity, null, KeyValue, Message);
                }
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                this.WriteLog(-1, entity, null, KeyValue, "����ʧ�ܣ�" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "����ʧ�ܣ�" + ex.Message }.ToString());
            }
        }
        /// <summary>
        /// ���Ķ���״̬
        /// </summary>
        /// <param name="Status">״̬��</param>
        /// <returns></returns>
        public ActionResult SetOrders(string KeyValue, int Status)
        {
            Fx_Orders entity = repositoryfactory.Repository().FindEntity(KeyValue);
            entity.Modify(KeyValue);
            entity.Stuts = Status;
            entity.Resutl = GetOrderResult(Status);
            int IsOk = repositoryfactory.Repository().Update(entity);
            return Content(new JsonMessage { Success = IsOk > 0, Code = IsOk.ToString(), Message = "����" + (IsOk > 0 ? "�ɹ�" : "ʧ��") }.ToString());
        }

        private string GetOrderResult(int status)
        {
            switch (status)
            {
                case 1:
                    return "δ����";
                case 2:
                    return "��Ч����";
                case 3:
                    return "�ѷ���";
                case 4:
                    return "�����";
                case 5:
                    return "���˻�";
                case 6:
                    return "����";
                case 7:
                    return "��Ч";
                default:
                    return "δ֪";
            }
        }
    }
}