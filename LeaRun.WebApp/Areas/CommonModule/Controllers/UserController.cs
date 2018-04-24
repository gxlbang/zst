using LeaRun.Business;
using LeaRun.DataAccess;
using LeaRun.Entity;
using LeaRun.Repository;
using LeaRun.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace LeaRun.WebApp.Areas.CommonModule.Controllers
{
    /// <summary>
    /// �û����������
    /// </summary>
    public class UserController : PublicController<Base_User>
    {
        Base_UserBll base_userbll = new Base_UserBll();
        Base_CompanyBll base_companybll = new Base_CompanyBll();
        Base_ObjectUserRelationBll base_objectuserrelationbll = new Base_ObjectUserRelationBll();

        #region �û�����
        /// <summary>
        /// ��ѯǰ��50���û���Ϣ������JSON��
        /// </summary>
        /// <param name="keywords">��ѯ�ؼ���</param>
        /// <returns></returns>
        public ActionResult Autocomplete(string keywords)
        {
            DataTable ListData = base_userbll.OptionUserList(keywords);
            return Content(ListData.ToJson());
        }
        /// <summary>
        /// ���û����������û��б�JSON
        /// </summary>
        /// <param name="keywords">��ѯ�ؼ���</param>
        /// <param name="CompanyId">��˾ID</param>
        /// <param name="DepartmentId">����ID</param>
        /// <param name="jqgridparam">������</param>
        /// <returns></returns>
        public ActionResult GridPageListJson(string keywords, string CompanyId, string DepartmentId, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable ListData = base_userbll.GetPageList(ManageProvider.Provider.Current().UserId, keywords, CompanyId, DepartmentId, ref jqgridparam);
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = ListData,
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
        /// ���û������ύ��
        /// </summary>
        /// <param name="KeyValue">����ֵ</param>
        /// <param name="base_user">�û���Ϣ</param>
        /// <param name="base_employee">Ա����Ϣ</param>
        /// <param name="BuildFormJson">�Զ����</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitUserForm(string KeyValue, Base_User base_user, Base_Employee base_employee, string BuildFormJson)
        {
            string ModuleId = DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId"));
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = KeyValue == "" ? "�����ɹ���" : "�༭�ɹ���";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    //ԭ����Ȩ���޸ı�����Ϣ - ��Ϊ����
                    //if (KeyValue == ManageProvider.Provider.Current().UserId)
                    //{
                    //    throw new Exception("��Ȩ�ޱ༭������Ϣ");
                    //}
                    base_user.Modify(KeyValue);
                    //base_employee.Modify(KeyValue);
                    database.Update(base_user, isOpenTrans);
                    //database.Update(base_employee, isOpenTrans);
                }
                else //�½��û�
                {
                    base_user.Create();
                    base_user.SortCode = CommonHelper.GetInt(BaseFactory.BaseHelper().GetSortCode<Base_User>("SortCode"));
                    //����Ϊ�ϼ�id
                    base_user.InnerUser = ManageProvider.Provider.Current().InnerUser + 1; //�û��㼶
                    base_user.DepartmentId = ManageProvider.Provider.Current().UserName;//�ϼ��û�����
                    base_user.CompanyId = ManageProvider.Provider.Current().Account; //�ϼ��û��ʺ�
                    base_user.Code = ManageProvider.Provider.Current().UserId; //�ϼ��û���id
                    database.Insert(base_user, isOpenTrans);
                    //Ȩ�޷���-�����ϼ�Ȩ��
                    CopyUserRight(ManageProvider.Provider.Current().UserId, base_user.UserId, isOpenTrans);
                    Base_DataScopePermissionBll.Instance.AddScopeDefault(ModuleId, ManageProvider.Provider.Current().UserId, base_user.UserId, isOpenTrans);
                }
                Base_FormAttributeBll.Instance.SaveBuildForm(BuildFormJson, base_user.UserId, ModuleId, isOpenTrans);
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
            Fx_WebConfigBll configbll = new Fx_WebConfigBll();
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
        /// ��ȡ�û�ְԱ��Ϣ���󷵻�JSON
        /// </summary>
        /// <param name="KeyValue">����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SetUserForm(string KeyValue)
        {
            Base_User base_user = DataFactory.Database().FindEntity<Base_User>(KeyValue);
            if (base_user == null)
            {
                return Content("");
            }
            //Base_Employee base_employee = DataFactory.Database().FindEntity<Base_Employee>(KeyValue);
            //Base_Company base_company = DataFactory.Database().FindEntity<Base_Company>(base_user.CompanyId);
            string strJson = base_user.ToJson();
            //��˾
            //strJson = strJson.Insert(1, "\"CompanyName\":\"" + base_company.FullName + "\",");
            ////Ա����Ϣ
            //strJson = strJson.Insert(1, base_employee.ToJson().Replace("{", "").Replace("}", "") + ",");
            //�Զ���
            strJson = strJson.Insert(1, Base_FormAttributeBll.Instance.GetBuildForm(KeyValue));
            return Content(strJson);
        }
        #endregion

        #region �޸ĵ�¼����
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        public ActionResult ResetPassword()
        {
            ViewBag.Account = Request["Account"];
            return View();
        }
        /// <summary>
        /// �޸�����
        /// </summary>
        /// <param name="KeyValue">����</param>
        /// <param name="Password">������</param>
        /// <returns></returns>
        public ActionResult ResetPasswordSubmit(string KeyValue, string Password)
        {
            try
            {
                int IsOk = 0;
                Base_User base_user = new Base_User();
                base_user.UserId = KeyValue;
                base_user.ModifyDate = DateTime.Now;
                base_user.ModifyUserId = ManageProvider.Provider.Current().UserId;
                base_user.ModifyUserName = ManageProvider.Provider.Current().UserName;
                base_user.Secretkey = Md5Helper.MD5Make(CommonHelper.CreateNo(),"", 16).ToLower();
                base_user.Password = Password;
                IsOk = repositoryfactory.Repository().Update(base_user);
                Base_SysLogBll.Instance.WriteLog(KeyValue, OperationType.Update, IsOk.ToString(), "�޸�����");
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = "�����޸ĳɹ���" }.ToString());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog(KeyValue, OperationType.Update, "-1", "�����޸�ʧ�ܣ�" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "�����޸�ʧ�ܣ�" + ex.Message }.ToString());
            }
        }
        #endregion

        #region �û���ɫ
        /// <summary>
        /// �û���ɫ
        /// </summary>
        /// <returns></returns>
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult UserRole()
        {
            return View();
        }
        /// <summary>
        /// �����û���ɫ
        /// </summary>
        /// <param name="CompanyId">��˾ID</param>
        /// <param name="UserId">�û�Id</param>
        /// <returns></returns>
        public ActionResult UserRoleList(string CompanyId, string UserId)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dt = base_userbll.UserRoleList(CompanyId, UserId);
            foreach (DataRow dr in dt.Rows)
            {
                string strchecked = "";
                if (!string.IsNullOrEmpty(dr["objectid"].ToString()))//�ж��Ƿ�ѡ��
                {
                    strchecked = "selected";
                }
                sb.Append("<li title=\"" + dr["fullname"] + "(" + dr["code"] + ")" + "\" class=\"" + strchecked + "\">");
                sb.Append("<a id=\"" + dr["RoleId"] + "\"><img src=\"../../Content/Images/Icon16/role.png \">" + dr["fullname"] + "</a><i></i>");
                sb.Append("</li>");
            }
            return Content(sb.ToString());
        }
        /// <summary>
        /// �û���ɫ - �ύ����
        /// </summary>
        /// <param name="UserId">�û�ID</param>
        /// <param name="ObjectId">��ɫid:1,2,3,4,5,6</param>
        /// <returns></returns>
        public ActionResult UserRoleSubmit(string UserId, string ObjectId)
        {
            try
            {
                string[] array = ObjectId.Split(',');
                int IsOk = base_objectuserrelationbll.BatchAddObject(UserId, array, "2");
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = "�����ɹ���" }.ToString());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "����ʧ�ܣ�����" + ex.Message }.ToString());
            }
        }
        #endregion

        #region ��������
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        public ActionResult PersonCenter()
        {
            if (ManageProvider.Provider.Current().Gender == "��")
            {
                ViewBag.imgGender = "man.png";
            }
            else
            {
                ViewBag.imgGender = "woman.png";
            }
            ViewBag.strUserInfo = ManageProvider.Provider.Current().UserName + "��" + ManageProvider.Provider.Current().Account + "��";
            return View();
        }
        /// <summary>
        /// ��֤������
        /// </summary>
        /// <param name="OldPassword"></param>
        /// <returns></returns>
        public ActionResult ValidationOldPassword(string OldPassword)
        {
            if (ManageProvider.Provider.Current().Account == "System" || ManageProvider.Provider.Current().Account == "guest")
            {
                return Content(new JsonMessage { Success = true, Code = "0", Message = "��ǰ�˻������޸�����" }.ToString());
            }
            OldPassword = Md5Helper.MD5Make(OldPassword,"", 32).ToLower();
            if (OldPassword != ManageProvider.Provider.Current().Password)
            {
                return Content(new JsonMessage { Success = true, Code = "0", Message = "ԭ�����������������" }.ToString());
            }
            else
            {
                return Content(new JsonMessage { Success = true, Code = "1", Message = "ͨ����Ϣ��֤" }.ToString());
            }
        }
        #endregion
    }
}