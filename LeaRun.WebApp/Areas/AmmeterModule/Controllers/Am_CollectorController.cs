/*
* 姓名:gxlbang
* 类名:Am_Collector
* CLR版本：
* 创建时间:2018-04-15 17:09:58
* 功能描述:
*
* 修改历史：
*
* ┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓
* ┃            Copyright(c) gxlbang ALL rights reserved                    ┃
* ┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛
*/
using LeaRun.Business;
using LeaRun.Entity;
using LeaRun.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeaRun.WebApp.Areas.AmmeterModule.Controllers
{
    /// <summary>
    /// Am_Collector控制器
    /// </summary>
    public class Am_CollectorController : PublicController<Am_Collector>
    {
        /// <summary>
        /// 搜索
        /// </summary>
        /// <returns></returns>
        public ActionResult GridPageListJson(JqGridParam jqgridparam, string keywords, int Stuts, string ProvinceId, string CityId, string CountyId)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                Am_CollectorBll bll = new Am_CollectorBll();
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
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 数据导出
        /// </summary>
        public void ExportExcel(int Stuts, string keywords, string ProvinceId, string CityId, string CountyId)
        {
            Am_CollectorBll bll = new Am_CollectorBll();
            var ListData = bll.GetPageList(keywords, Stuts, ProvinceId, CityId, CountyId);
            var newlist = new List<Am_CollectorNew>();
            foreach (var item in ListData)
            {
                var model = new Am_CollectorNew();
                model.Address = item.Address;
                model.AmCount = item.AmCount;
                model.City = item.City;
                model.CollectorCode = item.CollectorCode;
                model.County = item.County;
                model.CreateTime = item.CreateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                model.LastConnectTime = item.LastConnectTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                model.Province = item.Province;
                model.Remark = GetDays(item.LastConnectTime.Value) + "天";
                model.StatusStr = item.StatusStr;
                model.URealName = item.URealName;
                model.UserName = item.UserName;

                newlist.Add(model);
            }
            string[] columns = new string[] { "业主帐号:UserName", "业主姓名:URealName", "采集器编号:CollectorCode", "省:Province",
                "市:City", "区:County", "地址:Address", "状态:StatusStr","电表数:AmCount", "创建时间:CreateTime", "上次连接:LastConnectTime",
                "累积在线时间:Remark" };
            DeriveExcel.ListToExcel<Am_CollectorNew>(newlist, columns, "会员数据" + DateTime.Now.ToString("yyyyMMddHHmmss"));

        }

        public int GetDays(DateTime date)
        {
            var time = DateTime.Now - date;
            var day = time.Days;
            return day;
        }
    }
}