using LeaRun.Business;
using LeaRun.DataAccess;
using LeaRun.Entity;
using LeaRun.Entity.EntityModel;
using LeaRun.Repository;
using LeaRun.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeaRun.WebApp.Areas.ProductModule.Controllers
{
    /// <summary>
    /// Fx_Product������
    /// </summary>
    public class Fx_ProductController : PublicController<Fx_Product>
    {
        Fx_ProductBll bll = new Fx_ProductBll();

        /// <summary>
        /// ���ز�Ʒ�б�JSON
        /// </summary>
        /// <param name="jqgridparam">������</param>
        /// <returns></returns>
        public ActionResult GridPageListJson(JqGridParam jqgridparam,string Keyword,string ProductClass)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                var ListData = bll.GetPageList1(ref jqgridparam, Keyword, ProductClass);
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
        public ActionResult SubmitUserForm(string KeyValue, Fx_Product pclass, string BuildFormJson)
        {
            string ModuleId = DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId"));
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = KeyValue == "" ? "�����ɹ���" : "�༭�ɹ���";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    pclass.Modify(KeyValue);
                    database.Update(pclass, isOpenTrans);
                }
                else //�½�
                {
                    pclass.Create();
                    var result = database.Insert(pclass, isOpenTrans);
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
        /// ��ȡ��Ʒ���󷵻�JSON
        /// </summary>
        /// <param name="KeyValue">����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SetPclassForm(string KeyValue)
        {
            Fx_ProductClass base_user = DataFactory.Database().FindEntity<Fx_ProductClass>(KeyValue);
            if (base_user == null)
            {
                return Content("");
            }
            base_user.ClassUrl = "http://" + Request.Url.Host + base_user.ClassUrl;
            string strJson = base_user.ToJson();
            strJson = strJson.Insert(1, Base_FormAttributeBll.Instance.GetBuildForm(KeyValue));
            return Content(strJson);
        }
        /// <summary>
        /// ��ȡ��Ŀ�б�
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public ActionResult SetProductClassForm(string KeyValue)
        {
            var list = DataFactory.Database().FindListBySql<Fx_ProductClass>("select Number,ClassName from Fx_ProductClass");
            return Content(list.ToJson());
        }
        #region �ļ��ϴ���ɾ��
        /// <summary>
        /// �ļ��ϴ�
        /// </summary>
        /// <param name="Filedata"></param>
        /// <returns></returns>
        public ActionResult ImgUpload(HttpPostedFileBase Filedata)
        {
            try
            {
                FileProperty fileproperty = new FileProperty();
                //û���ļ��ϴ���ֱ�ӷ���
                if (Filedata == null || string.IsNullOrEmpty(Filedata.FileName) || Filedata.ContentLength == 0)
                {
                    return HttpNotFound();
                }
                //��ȡ�ļ������ļ���(��������·��)
                //�ļ����·����ʽ��/Resource/Document/Email/{����}/{guid}.{��׺��}
                //���磺/Resource/Document/Email/20130913/43CA215D947F8C1F1DDFCED383C4D706.jpg
                string fileGuid = CommonHelper.GetGuid;
                long filesize = Filedata.ContentLength;
                string FileEextension = Path.GetExtension(Filedata.FileName);
                string uploadDate = DateTime.Now.ToString("yyyyMMdd");

                string virtualPath = string.Format("/Resource/ProductImages/{0}/{1}{2}", uploadDate, fileGuid, FileEextension);
                string fullFileName = this.Server.MapPath(virtualPath);
                //�����ļ��У������ļ�
                string path = Path.GetDirectoryName(fullFileName);
                Directory.CreateDirectory(path);
                if (!System.IO.File.Exists(fullFileName))
                {
                    Filedata.SaveAs(fullFileName);
                    fileproperty.Id = fileGuid;
                    fileproperty.Name = Filedata.FileName;
                    fileproperty.Eextension = FileEextension;
                    fileproperty.CreateDate = DateTime.Now;
                    fileproperty.Path = virtualPath;
                    fileproperty.Size = SizeHelper.CountSize(filesize);
                }
                return Content(fileproperty.ToJson());
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// ɾ���ļ�
        /// </summary>
        /// <param name="Path">·��</param>
        /// <returns></returns>
        public ActionResult ImgDeleteFile(string Path)
        {
            try
            {
                string FilePath = this.Server.MapPath(Path);
                if (System.IO.File.Exists(FilePath))
                    System.IO.File.Delete(FilePath);
                return Content(new JsonMessage { Success = true, Code = "1", Message = "ɾ���ɹ�" }.ToString());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "����ʧ�ܣ�" + ex.Message }.ToString());
            }
        }
        #endregion
    }
}