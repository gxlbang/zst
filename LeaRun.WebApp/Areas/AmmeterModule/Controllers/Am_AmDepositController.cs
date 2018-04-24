/*
* ����:gxlbang
* ����:Am_AmDeposit
* CLR�汾��
* ����ʱ��:2018-04-17 19:10:01
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
    /// Am_AmDeposit������
    /// </summary>
    public class Am_AmDepositController : PublicController<Am_AmDeposit>
    {
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public ActionResult GridPageListJson(JqGridParam jqgridparam, string keywords, [DefaultValue(-1)]int Stuts, string ProvinceId, string CityId, string CountyId)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                Am_AmDepositBll bll = new Am_AmDepositBll();
                var ListData = bll.GetPageList(ref jqgridparam, keywords, Stuts, ProvinceId, CityId, CountyId);
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
        public void ExportExcel([DefaultValue(-1)]int Stuts, string keywords, string ProvinceId, string CityId, string CountyId)
        {
            Am_AmDepositBll bll = new Am_AmDepositBll();
            var ListData = bll.GetPageList(keywords, Stuts, ProvinceId, CityId, CountyId);
            var newlist = new List<Am_AmDepositNew>();
            foreach (var item in ListData)
            {
                var model = new Am_AmDepositNew();
                model.Address = item.Address;
                model.Ammeter_Code = item.Ammeter_Code;
                model.Cell = item.Cell;
                model.City = item.City;
                model.County = item.County;
                model.CreateTime = item.CreateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                model.CurrMoeny = item.CurrMoeny.ToString();
                model.Floor = item.Floor;
                model.Money = item.Money.Value.ToString("0.00");
                model.Province = item.Province;
                model.Room = item.Room;
                model.StatusStr = item.StatusStr;
                model.U_Name = item.U_Name;
                model.UpdateTime = item.UpdateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");

                newlist.Add(model);
            }
            string[] columns = new string[] { "��Ӫ��:U_Name","����:Ammeter_Code","ʡ:Province",
                "��:City", "��:County","��ַ:Address","��Ԫ:Cell","¥��:Floor","����:Room", 
                "Ѻ��ʱ��:CreateTime", "״̬:StatusStr","Ѻ����:Money", "ʣ����:CurrMoeny",
                "������ʱ��:UpdateTime" };
            DeriveExcel.ListToExcel<Am_AmDepositNew>(newlist, columns, "Ѻ������" + DateTime.Now.ToString("yyyyMMddHHmmss"));

        }
    }
}