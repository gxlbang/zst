/*
* ����:gxlbang
* ����:Am_Ammeter
* CLR�汾��
* ����ʱ��:2018-04-14 10:54:05
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
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeaRun.WebApp.Areas.AmmeterModule.Controllers
{
    /// <summary>
    /// Am_Ammeter������
    /// </summary>
    public class Am_AmmeterController : PublicController<Am_Ammeter>
    {
        public override ActionResult Index()
        {
            string _ModuleId = DESEncrypt.Encrypt("235ddccb-9114-404b-9632-06715c6ad496");
            CookieHelper.WriteCookie("ModuleId", _ModuleId);
            return View();
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public ActionResult GridPageListJson(JqGridParam jqgridparam,string Number, string keywords, [DefaultValue(-1)]int Stuts, string ProvinceId, string CityId, string CountyId)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                Am_AmmeterBll bll = new Am_AmmeterBll();
                var ListData = bll.GetPageList(ref jqgridparam,Number, keywords, Stuts, ProvinceId, CityId, CountyId);
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
        public void ExportExcel([DefaultValue(-1)]int Stuts, string keywords, string Number, string ProvinceId, string CityId, string CountyId)
        {
            Am_AmmeterBll bll = new Am_AmmeterBll();
            var ListData = bll.GetPageList(keywords,Number, Stuts, ProvinceId, CityId, CountyId);
            var newlist = new List<Am_AmmeterNew>();
            foreach (var item in ListData)
            {
                var model = new Am_AmmeterNew();
                model.Address = item.Address;
                model.AmmeterMoney_Name = item.AmmeterMoney_Name;
                model.AmmeterType_Name = item.AmmeterType_Name;
                model.AM_Code = item.AM_Code;
                model.Cell = item.Cell;
                model.City = item.City;
                model.Collector_Code = item.Collector_Code;
                model.County = item.County;
                model.CreateTime = item.CreateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                model.FirstAlarm = item.FirstAlarm.ToString();
                model.Floor = item.Floor;
                model.Money = item.Money.Value.ToString("0.00");
                model.Province = item.Province;
                model.Room = item.Room;
                model.StatusStr = item.StatusStr;
                model.UserName = item.UserName;
                model.UY_Name = item.UY_Name;
                model.UY_UserName = item.UY_UserName;
                model.U_Name = item.U_Name;

                newlist.Add(model);
            }
            string[] columns = new string[] { "����:AM_Code", "�������:AmmeterType_Name", "�۸�:AmmeterMoney_Name",
                "�ɼ������:Collector_Code","�⻧�ʺ�:UserName","�⻧����:U_Name","ʡ:Province",
                "��:City", "��:County","��Ԫ:Cell","¥��:Floor","����:Room", "��ַ:Address",
                "״̬:StatusStr","��װʱ��:CreateTime", "һ��Ԥ��:FirstAlarm", "������:Money",
                "ҵ���ʺ�:UY_UserName","ҵ������:UY_Name" };
            DeriveExcel.ListToExcel<Am_AmmeterNew>(newlist, columns, "�������" + DateTime.Now.ToString("yyyyMMddHHmmss"));

        }
        /// <summary>
        /// ���� ��JONS
        /// </summary>
        /// <returns></returns>
        public ActionResult TreeJson(string Number)
        {
            IDatabase database = DataFactory.Database();
            string sql = "select Number,CollectorCode from Am_Collector where 1=1";
            //�û��޶�
            if (ManageProvider.Provider.Current().DepartmentId == "��Ӫ��")
            {
                sql += " and UNumber = '" + ManageProvider.Provider.Current().CompanyId + "'";
            }
            if (!StringHelper.IsNullOrEmpty(Number))
            {
                sql += " and Collector_Number = '" + Number + "'";
            }
            var list = database.FindListBySql<Am_Collector>(sql);
            List<TreeJsonEntity> TreeList = new List<TreeJsonEntity>();
            TreeJsonEntity tree = new TreeJsonEntity();
            tree.id = "";
            tree.text = "���вɼ���";
            tree.parentId = "0";
            tree.Attribute = "Type";
            tree.AttributeValue = "Parent";
            tree.isexpand = true;
            tree.complete = true;
            tree.hasChildren = true;
            tree.img = "/Content/Images/Icon16/folder.png";
            TreeList.Add(tree);
            foreach (var item in list)
            {
                if (item != null)
                {
                    TreeJsonEntity tree1 = new TreeJsonEntity();
                    tree1.id = item.Number;
                    tree1.text = item.CollectorCode;
                    tree1.parentId = "";
                    tree1.Attribute = "Type";
                    tree1.AttributeValue = "Collector";
                    tree1.isexpand = true;
                    tree1.complete = true;
                    tree1.hasChildren = false; 
                    tree1.img = "/Content/Images/Icon16/report.png";
                    TreeList.Add(tree1);
                }
            }
            return Content(TreeList.TreeToJson());
        }
    }
}