/*
* ����:gxlbang
* ����:Ho_HouseImage
* CLR�汾��
* ����ʱ��:2017-11-24 10:55:19
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
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeaRun.WebApp.Areas.HouseModule.Controllers
{
    /// <summary>
    /// Ho_HouseImage������
    /// </summary>
    public class Ho_HouseImageController : PublicController<Ho_HouseImage>
    {
        public override ActionResult Index()
        {
            string _ModuleId = DESEncrypt.Encrypt("1020da1a-b5b4-40c6-bab9-95fe0a0ddc6d");
            CookieHelper.WriteCookie("ModuleId", _ModuleId);
            return View();
        }
        public ActionResult UploadImageIndex()
        {
            return View();
        }

        /// <summary>
        /// ��ȡ����ͼƬ
        /// </summary>
        /// <returns></returns>
        public ActionResult GridPictureListJson(JqGridParam jqgridparam, string Hnumber, string Gnumber)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                Ho_HouseImageBll bll = new Ho_HouseImageBll();
                var ListData = bll.GetPageList(ref jqgridparam, Hnumber, Gnumber);
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
        public ActionResult SubmitUserForm(string KeyValue, Ho_HouseImage model, string BuildFormJson)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = KeyValue == "" ? "�����ɹ���" : "�༭�ɹ���";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    //����¥����ļ�¼
                    var oldModel = database.FindEntity<Ho_HouseImage>(KeyValue);
                    if (oldModel.IsDel == 0) //����ɾ������һ��Ϊ��ͼ
                    {
                        var hmodel = database.FindEntity<Ho_HouseInfo>(oldModel.HouseNumber);
                        switch (oldModel.GroupNumber)
                        {
                            case "5f69b268-504e-44f8-950b-dfda8e00ee19": //Ч��ͼ
                                hmodel.DesignImage = model.ImageUrl;
                                break;
                            case "bc4a267a-3404-4e2e-9053-a0788bd53789": //ʵ��ͼ
                                hmodel.RealImage = model.ImageUrl;
                                break;
                            case "b8af3e9b-aea0-4618-9f66-67987b081731": //����
                                hmodel.HouseImage = model.ImageUrl;
                                break;
                            case "e6b2a2ee-9744-4902-9ad1-d29b06c883ab": //����
                                hmodel.HouseTypeImage = model.ImageUrl;
                                break;
                        }
                        hmodel.Modify(oldModel.HouseNumber);
                        var result = database.Update(hmodel, isOpenTrans);
                    }
                    model.Modify(KeyValue);
                   var idex= database.Update(model, isOpenTrans);
                }
                else //�½�
                {
                    model.Create();
                    var result = database.Insert(model, isOpenTrans);
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
        /// ���� ��JONS
        /// </summary>
        /// <returns></returns>
        public ActionResult TreeJson(string Number)
        {
            IDatabase database = DataFactory.Database();
            string sql = "select Number,Name from Ho_HouseInfo where 1=1";
            if (!StringHelper.IsNullOrEmpty(Number))
            {
                sql += " and Number = '" + Number + "'";
            }
            var houseList = database.FindListBySql<Ho_HouseInfo>(sql);
            List<TreeJsonEntity> TreeList = new List<TreeJsonEntity>();
            foreach (var houseModel in houseList)
            {
                if (houseModel != null)
                {
                    TreeJsonEntity tree = new TreeJsonEntity();
                    tree.id = houseModel.Number;
                    tree.text = houseModel.Name;
                    tree.parentId = "0";
                    tree.Attribute = "Type";
                    tree.AttributeValue = "House";
                    tree.isexpand = true;
                    tree.complete = true;
                    tree.hasChildren = true;
                    tree.img = "/Content/Images/Icon16/house_one.png";
                    TreeList.Add(tree);

                    //ʣ���ĸ�ͼ��
                    var dicList = database.FindList<Base_DataDictionaryDetail>(" and DataDictionaryId = '9ff7250d-bb28-44ac-a721-04bf33325a85'");
                    if (dicList != null)
                    {
                        foreach (var item in dicList)
                        {
                            var treeModel = new TreeJsonEntity()
                            {
                                id = item.DataDictionaryDetailId,
                                text = item.FullName,
                                parentId = houseModel.Number,
                                value = houseModel.Number,
                                Attribute = "Type",
                                AttributeValue = "Group",
                                isexpand = true,
                                complete = true,
                                hasChildren = false,
                                img = "/Content/Images/Icon16/image.png"
                            };
                            TreeList.Add(treeModel);
                        }
                    }

                }
            }
            return Content(TreeList.TreeToJson());
        }


        /// <summary>
        /// ͼƬ�ϴ�-֧�ֶ�ͼ
        /// </summary>
        /// <param name="id">����Դ�</param>
        /// <param name="name">ͼƬ����</param>
        /// <param name="Hnumber">¥�̱��</param>
        /// <param name="Gnumber">ͼ����</param>
        /// <param name="type">�ϴ��ļ�����</param>
        /// <param name="lastModifiedDate">ʱ��</param>
        /// <param name="size">��С</param>
        /// <param name="file">�ϴ����ļ�</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ImageUpload(string id, string name, string Hnumber, string Gnumber, string type, string lastModifiedDate, int size, HttpPostedFileBase file)
        {
            string filePathName = string.Empty;
            var dirtory = Server.MapPath("~/Resource");
            string localPath = Path.Combine(dirtory, "pictures");
            if (Request.Files.Count == 0)
            {
                return Json(new { jsonrpc = 2.0, error = new { code = 102, message = "����ʧ��" }, id = "id" });
            }
            string ex = Path.GetExtension(file.FileName);
            if (!IsImageExtensionName(ex))
            {
                return Json(new { jsonrpc = 2.0, error = new { code = 500, message = "��ͼƬ�ļ�" }, id = "id" });
            }
            filePathName = Guid.NewGuid().ToString("N") + ex;
            if (!System.IO.Directory.Exists(localPath))
            {
                System.IO.Directory.CreateDirectory(localPath);
            }
            var filePath = "/Resource/pictures/" + filePathName;
            file.SaveAs(Path.Combine(localPath, filePathName));
            //����ͼƬ
            if (!StringHelper.IsNullOrEmpty(Hnumber) && !StringHelper.IsNullOrEmpty(Gnumber))
            {
                var model = new Ho_HouseImage() { ImageUrl = filePath, HouseNumber = Hnumber, GroupNumber = Gnumber };
                model.Create();
                model.Orders = 0;
                model.IsDel = 1; // 1�����ɾ��
                IDatabase database = DataFactory.Database();
                var hmodel = database.FindEntity<Ho_HouseInfo>(Hnumber);
                if (hmodel != null) { model.HouseName = hmodel.Name; }
                var dmodel = database.FindEntityByWhere<Base_DataDictionaryDetail>(" and DataDictionaryDetailId = '" + Gnumber + "'");
                if (dmodel != null) { model.GroupName = dmodel.FullName; }
                if (hmodel != null) { model.HouseName = hmodel.Name; }
                int res = database.Insert(model);
            }
            return Json(new
            {
                jsonrpc = "2.0",
                id = id,
                filePath = filePath
            });
        }
        private bool IsImageExtensionName(string ex)
        {
            ex = ex.ToLower();
            if (ex == ".jpg" || ex == ".jpeg" || ex == ".png" || ex == ".bmp" || ex == ".gif")
                return true;
            else
                return false;
        }
    }
}