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
using LeaRun.Utilities.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeaRun.WebApp.Areas.AmmeterModule.Controllers
{
    /// <summary>
    /// �û�������
    /// </summary>
    public class Ho_PartnerUserController : PublicController<Ho_PartnerUser>
    {
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public ActionResult GridPageListJson(JqGridParam jqgridparam, string Role, string Keyword, int Stuts)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                Ho_PartnerUserBll bll = new Ho_PartnerUserBll();
                var ListData = bll.GetPageList(ref jqgridparam, Keyword, Role, Stuts);
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
        public ActionResult SubmitUserForm(string KeyValue, Ho_PartnerUser model, string BuildFormJson)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = KeyValue == "" ? "�����ɹ���" : "�༭�ɹ���";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    if (model.UserRole == "��Ӫ��")//ѡ������Ӫ�̲����
                    {
                        var usermodelNum = database.FindCount<Base_User>(" and Account = '" + model.Account + "'");
                        if (usermodelNum < 1)
                        {
                            //��������Ӫ��,��Ҫ����̨�û������һ���ʺ�
                            var user = new Base_User()
                            {
                                Account = model.Account,
                                Password = "123456",
                                RealName = model.Name,
                                Mobile = model.Account,
                                SortCode = CommonHelper.GetInt(BaseFactory.BaseHelper().GetSortCode<Base_User>("SortCode")),
                                InnerUser = 3,
                                DepartmentId = "����ɾ��",
                                CompanyId = "BaseUser",
                                Code = "bd548d5b-1783-4582-9007-bb5c87803679"
                            };
                            user.Create();
                            user.Password = model.Password;
                            database.Insert(user, isOpenTrans);
                            //Ȩ�޷���-�����ϼ�Ȩ��-bd548d5b-1783-4582-9007-bb5c87803679(���û�����ɾ��)
                            CopyUserRight("bd548d5b-1783-4582-9007-bb5c87803679", user.UserId, isOpenTrans);
                        }
                    }
                    model.Modify(KeyValue);

                    var IsOk = database.Update(model, isOpenTrans);
                    Base_SysLogBll.Instance.WriteLog(KeyValue, OperationType.Update, IsOk > 0 ? "�ɹ�" : "ʧ��", "�û�" + Message);
                }
                else //�½�
                {
                    //����ֻ��ź����֤�ŵ�Ψһ��
                    var accountIsMobile = database.FindEntity<Ho_PartnerUser>(" and Account='" + model.Account + "'");
                    if (accountIsMobile != null && accountIsMobile.Number != null)
                    {
                        return Content(new JsonMessage { Success = false, Code = "1", Message = "�ֻ������Ѵ���" }.ToString());
                    }
                    if (!string.IsNullOrEmpty(model.CardCode))
                    {
                        var accountIsCardCode = database.FindEntity<Ho_PartnerUser>(" and CardCode='" + model.CardCode + "'");
                        if (accountIsCardCode != null && accountIsCardCode.Number != null)
                        {
                            return Content(new JsonMessage { Success = false, Code = "1", Message = "���֤�����Ѵ���" }.ToString());
                        }
                    }
                    if (model.UserRole == "��Ӫ��")//ѡ������Ӫ�̲����
                    {
                        //��������Ӫ��,��Ҫ����̨�û������һ���ʺ�
                        var user = new Base_User()
                        {
                            Account = model.Account,
                            Password = model.Password,
                            RealName = model.Name,
                            Mobile = model.Account,
                            SortCode = CommonHelper.GetInt(BaseFactory.BaseHelper().GetSortCode<Base_User>("SortCode")),
                            InnerUser = 2,
                            DepartmentId = "����ɾ��",
                            CompanyId = "BaseUser",
                            Code = "bd548d5b-1783-4582-9007-bb5c87803679"
                        };
                        user.Create();
                        database.Insert(user, isOpenTrans);
                        //Ȩ�޷���-�����ϼ�Ȩ��-bd548d5b-1783-4582-9007-bb5c87803679(���û�����ɾ��)
                        CopyUserRight("bd548d5b-1783-4582-9007-bb5c87803679", user.UserId, isOpenTrans);
                    }
                    model.Password = PasswordHash.CreateHash(model.Password);
                    model.Create();
                    var IsOk = database.Insert(model, isOpenTrans);

                    Base_SysLogBll.Instance.WriteLog(KeyValue, OperationType.Update, IsOk > 0 ? "�ɹ�" : "ʧ��", "�û�" + Message);
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
        /// �����ϼ�Ȩ�޸��¼�
        /// </summary>
        /// <param name="OldUserId">�ϼ�id</param>
        /// <param name="ObjectId">�û�id</param>
        /// <param name="isOpenTrans"></param>
        public void CopyUserRight(string OldUserId, string ObjectId, DbTransaction isOpenTrans = null)
        {
            IDatabase database = DataFactory.Database();
            try
            {
                //ģ��Ȩ��
                var entityList = database.FindListBySql<Base_ModulePermission>("select * from Base_ModulePermission where ObjectId = '" + OldUserId + "'");
                foreach (var entity in entityList)
                {
                    entity.Create();
                    entity.ObjectId = ObjectId;
                    if (isOpenTrans != null)
                    {
                        database.Insert(entity, isOpenTrans);
                    }
                    else
                    {
                        database.Insert(entity);
                    }
                }
                //��ťȨ��
                var entityList1 = database.FindListBySql<Base_ButtonPermission>("select * from Base_ButtonPermission where ObjectId = '" + OldUserId + "'");
                foreach (var entity1 in entityList1)
                {
                    entity1.Create();
                    entity1.ObjectId = ObjectId;
                    if (isOpenTrans != null)
                    {
                        database.Insert(entity1, isOpenTrans);
                    }
                    else
                    {
                        database.Insert(entity1);
                    }
                }
                //��ͼȨ��
                var entityList2 = database.FindListBySql<Base_ViewPermission>("select * from Base_ViewPermission where ObjectId = '" + OldUserId + "'");
                foreach (var entity2 in entityList2)
                {
                    entity2.Create();
                    entity2.ObjectId = ObjectId;
                    if (isOpenTrans != null)
                    {
                        database.Insert(entity2, isOpenTrans);
                    }
                    else
                    {
                        database.Insert(entity2);
                    }
                }
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "�쳣����" + ex.Message);
            }
        }
        /// <summary>
        /// �û���ɫ
        /// </summary>
        /// <returns></returns>
        public ActionResult GetUserRole()
        {
            IDatabase database = DataFactory.Database();
            var list = database.FindList<Am_UserRole>();
            return Content(list.ToJson());
        }
        //�����û�
        public ActionResult DisableUser(string KeyValue)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = KeyValue == "" ? "�����ɹ���" : "�༭�ɹ���";
                var result = "����";
                if (!string.IsNullOrEmpty(KeyValue))//�༭
                {
                    var oldmodel = database.FindEntity<Ho_PartnerUser>(KeyValue);
                    oldmodel.Status = 9;
                    oldmodel.StatusStr = "������";
                    oldmodel.Modify(KeyValue);
                    var IsOk = database.Update(oldmodel, isOpenTrans);
                    Base_SysLogBll.Instance.WriteLog(KeyValue, OperationType.Update, IsOk > 0 ? "�ɹ�" : "ʧ��", "�û�" + result);
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
                    return "��ע��";
                case 1:
                    return "���ύ";
                case 2:
                    return "��ͨ��";
                case 3:
                    return "�����";
                case 9:
                    return "������";
                default:
                    return "δ֪";
            }
        }

        /// <summary>
        /// ���ݵ���
        /// </summary>
        public void ExportExcel(int Stuts, string Keyword, string Role)
        {
            Ho_PartnerUserBll bll = new Ho_PartnerUserBll();
            var ListData = bll.GetPageList(Keyword, Role, Stuts);
            var newlist = new List<Ho_PartnerUserNew>();
            foreach (var item in ListData)
            {
                var model = new Ho_PartnerUserNew();
                model.Address = item.Address;
                model.CardCode = item.CardCode;
                model.CreatTime = item.CreatTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                model.Account = item.Account;
                model.Money = item.Money.Value.ToString("0.00");
                model.Name = item.Name;
                model.Remark = item.Remark;
                model.StatusStr = item.StatusStr;
                model.UserRole = item.UserRole;

                newlist.Add(model);
            }
            string[] columns = new string[] { "����:Name", "���֤��:CardCode", "�ֻ���:Account", "��ɫ:UserRole",
                "���:Money", "��ַ:Address", "����ʱ��:CreatTime", "״̬:StatusStr", "��ע:Remark" };
            DeriveExcel.ListToExcel<Ho_PartnerUserNew>(newlist, columns, "��Ա����" + DateTime.Now.ToString("yyyyMMddHHmmss"));

        }
    }
}