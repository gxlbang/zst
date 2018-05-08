using LeaRun.DataAccess;
using LeaRun.Entity;
using LeaRun.Repository;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace LeaRun.WebApp.Controllers
{
    [UserLoginFilters]
    [UserOperatorFilters]
    public class ProprietorController : Controller
    {
        IDatabase database = DataFactory.Database();
        WebData wbll = new WebData();
        //
        // GET: /Proprietor/

        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 电表
        /// </summary>
        /// <param name="Number"></param>
        /// <returns></returns>
        public ActionResult AmmeterAdd(string Number)
        {
            var user = wbll.GetUserInfo(Request);

            var ammeterTpyeList = database.FindList<Am_AmmeterType>("");
            ViewBag.ammeterTpyeList = ammeterTpyeList;

            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@UNumber", user.Number));
            var collectorList = database.FindList<Am_Collector>(" and UNumber=@UNumber", parameter.ToArray());
            ViewBag.collectorList = collectorList;


            List<DbParameter> par1 = new List<DbParameter>();
            par1.Add(DbFactory.CreateDbParameter("@UserNumber", user.Number));
            var ammeterMoneyList = database.FindList<Am_AmmeterMoney>(" and UserNumber=@UserNumber", par1.ToArray());
            ViewBag.ammeterMoneyList = ammeterMoneyList;

            if (Number != null && Number != "")
            {
                List<DbParameter> par2 = new List<DbParameter>();
                par2.Add(DbFactory.CreateDbParameter("@UY_Number", user.Number));
                par2.Add(DbFactory.CreateDbParameter("@Number", Number));
                var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and UY_Number=@UY_Number and Number=@Number", par2.ToArray());
                if (ammeter != null && ammeter.Number != null)
                {
                    return View(ammeter);
                }
            }
            return View();
        }
        /// <summary>
        /// 电表编辑添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AmmeterEidt(Am_Ammeter model)
        {
            var user = wbll.GetUserInfo(Request);
            if (model.Number != null && model.Number != "")
            {
                List<DbParameter> parAmmeter = new List<DbParameter>();
                parAmmeter.Add(DbFactory.CreateDbParameter("@UY_Number", user.Number));
                parAmmeter.Add(DbFactory.CreateDbParameter("@Number", model.Number));
                var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and UY_Number=@UY_Number and Number=@Number", parAmmeter.ToArray());

                if (ammeter != null && ammeter.Number != null)
                {
                    List<DbParameter> par1 = new List<DbParameter>();
                    par1.Add(DbFactory.CreateDbParameter("@Number", model.AmmeterMoney_Number));
                    par1.Add(DbFactory.CreateDbParameter("@UserNumber", user.Number));
                    var ammeterMoney = database.FindEntityByWhere<Am_AmmeterMoney>(" and Number=@Number ", par1.ToArray());
                    ammeter.AmmeterMoney_Number = ammeterMoney.Number;
                    ammeter.AmmeterMoney_Name = ammeterMoney.Name;

                    List<DbParameter> par2 = new List<DbParameter>();
                    par2.Add(DbFactory.CreateDbParameter("@Number", model.AmmeterType_Number));
                    var ammeterType = database.FindEntityByWhere<Am_AmmeterType>(" and Number=@Number ", par2.ToArray());
                    ammeter.AmmeterType_Number = ammeterType.Number;
                    ammeter.AmmeterType_Name = ammeterType.Name;

                    List<DbParameter> par3 = new List<DbParameter>();
                    par3.Add(DbFactory.CreateDbParameter("@Number", model.Collector_Number));
                    var collector = database.FindEntityByWhere<Am_Collector>(" and Number=@Number ", par3.ToArray());
                    ammeter.Collector_Number = collector.Number;
                    ammeter.Collector_Code = collector.CollectorCode;

                    if (model.UserName != null && model.UserName != "")
                    {
                        ammeter.Status = 1;
                        ammeter.StatusStr = "已开户";

                        List<DbParameter> par4 = new List<DbParameter>();
                        par4.Add(DbFactory.CreateDbParameter("@Account", model.UserName));
                        par4.Add(DbFactory.CreateDbParameter("@Name", model.U_Name));
                        par4.Add(DbFactory.CreateDbParameter("@Status", "3"));//审核已过

                        var partnerUser = database.FindEntityByWhere<Ho_PartnerUser>(" and Account=@Account and  Name=@Name and Status=@Status", par4.ToArray());
                        if (partnerUser != null && partnerUser.Number != null)
                        {
                            ammeter.UserName = partnerUser.Account;
                            ammeter.U_Number = partnerUser.Number;
                            ammeter.U_Name = partnerUser.Name;
                        }
                        else
                        {
                            return Json(new { res = "No", msg = "提交失败，没有找到该用户" });
                        }
                    }


                    ammeter.Address = model.Address;
                    ammeter.AllMoney = model.AllMoney;
                    ammeter.AM_Code = model.AM_Code;
                    ammeter.Cell = model.Cell;
                    ammeter.City = model.City;
                    ammeter.County = model.County;
                    ammeter.CurrMoney = model.CurrMoney;
                    ammeter.CurrPower = model.CurrPower;
                    ammeter.FirstAlarm = model.FirstAlarm;
                    ammeter.Floor = model.Floor;
                    ammeter.HGQBB = model.HGQBB;
                    ammeter.Province = model.Province;
                    ammeter.Room = model.Room;
                    ammeter.SencondAlarm = model.SencondAlarm;
                    ammeter.UpdateTime = DateTime.Now;
                    ammeter.Remark = model.Remark;

                    var status = database.Update<Am_Ammeter>(ammeter);
                    if (status > 0)
                    {
                        return Json(new { res = "Ok", msg = "修改成功" });
                    }
                }
            }
            else
            {
                List<DbParameter> par1 = new List<DbParameter>();
                par1.Add(DbFactory.CreateDbParameter("@Number", model.AmmeterMoney_Number));
                var ammeterMoney = database.FindEntityByWhere<Am_AmmeterMoney>(" and Number=@Number ", par1.ToArray());

                List<DbParameter> par2 = new List<DbParameter>();
                par2.Add(DbFactory.CreateDbParameter("@Number", model.AmmeterType_Number));
                var ammeterType = database.FindEntityByWhere<Am_AmmeterType>(" and Number=@Number ", par2.ToArray());

                List<DbParameter> par3 = new List<DbParameter>();
                par3.Add(DbFactory.CreateDbParameter("@Number", model.Collector_Number));
                var collector = database.FindEntityByWhere<Am_Collector>(" and Number=@Number ", par3.ToArray());


                var insertModel = new Am_Ammeter
                {
                    Address = model.Address,
                    UserTime = DateTime.Now,
                    AllMoney = 0.00,
                    AmmeterMoney_Name = ammeterMoney.Name,
                    AmmeterMoney_Number = model.AmmeterMoney_Number,
                    AmmeterType_Name = ammeterType.Name,
                    AmmeterType_Number = model.AmmeterType_Number,
                    AM_Code = model.AM_Code,
                    Cell = model.Cell,
                    City = collector.City,
                    CM_Time = DateTime.Now,
                    Collector_Code = collector.CollectorCode,
                    Collector_Number = model.Collector_Number,
                    County = collector.County,
                    CP_Time = DateTime.Now,
                    CreateTime = DateTime.Now,
                    CurrMoney = 0.00,
                    CurrPower = "0",
                    FirstAlarm = model.FirstAlarm,
                    Floor = model.Floor,
                    HGQBB = "0",
                    Money = 0.00,
                    Number = Utilities.CommonHelper.GetGuid,
                    Province = collector.Province,
                    Remark = model.Remark,
                    Room = model.Room,
                    SencondAlarm = 0,
                    Status = 0,
                    StatusStr = "未开户",
                    UpdateTime = DateTime.Now,
                    UserName = model.UserName,
                    UY_Name = user.Name,
                    UY_Number = user.Number,
                    UY_UserName = user.Account,
                    U_Name = model.U_Name,
                    U_Number = model.U_Number
                };
                if (model.UserName != null && model.UserName != "")
                {
                    insertModel.Status = 1;
                    insertModel.StatusStr = "已开户";

                    List<DbParameter> par4 = new List<DbParameter>();
                    par4.Add(DbFactory.CreateDbParameter("@Account", model.UserName));
                    par4.Add(DbFactory.CreateDbParameter("@Name", model.U_Name));
                    par4.Add(DbFactory.CreateDbParameter("@Status", "3"));//审核已过

                    var partnerUser = database.FindEntityByWhere<Ho_PartnerUser>(" and Account=@Account and  Name=@Name and Status=@Status", par4.ToArray());
                    if (partnerUser != null && partnerUser.Number != null)
                    {
                        insertModel.UserName = partnerUser.Account;
                        insertModel.U_Number = partnerUser.Number;
                        insertModel.U_Name = partnerUser.Name;
                    }
                    else
                    {
                        return Json(new { res = "No", msg = "提交失败，没有找到该用户" });
                    }
                }

                var status = database.Insert<Am_Ammeter>(insertModel);
                if (status > 0)
                {
                    return Json(new { res = "Ok", msg = "添加成功" });
                }
            }
            return Json(new { res = "No", msg = "提交失败" });
        }

        public ActionResult AmmeterManage()
        {
            return View();
        }
        /// <summary>
        /// 电表列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult AmmeterList(string ammeterCode, string name, int pageIndex = 1, int pageSize = 10)
        {
            var user = wbll.GetUserInfo(Request);
            int recordCount = 0;

            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@UY_Number", user.Number));

            StringBuilder sbWhere = new StringBuilder();
            sbWhere.Append(" and UY_Number=@UY_Number ");

            if (ammeterCode != null && ammeterCode != "")
            {
                parameter.Add(DbFactory.CreateDbParameter("@AM_Code", ammeterCode));
                sbWhere.Append(" and AM_Code=@AM_Code");
            }
            if (name != null && name != "")
            {
                parameter.Add(DbFactory.CreateDbParameter("@U_Name", name));
                sbWhere.Append(" and U_Name=@U_Name");
            }

            var ammeterList = database.FindListPage<Am_Ammeter>(sbWhere.ToString(), parameter.ToArray(), "CreateTime", "desc", pageIndex, pageSize, ref recordCount);
            ViewBag.recordCount = (int)Math.Ceiling(1.0 * recordCount / pageSize); ;
            if (Request.IsAjaxRequest())
            {
                return Json(ammeterList);
            }
            else
            {
                return View();
            }
        }
        /// <summary>
        /// 电表详情
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public ActionResult AmmeterInfo(string number)
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> par2 = new List<DbParameter>();
            par2.Add(DbFactory.CreateDbParameter("@UY_Number", user.Number));
            par2.Add(DbFactory.CreateDbParameter("@Number", number));
            var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and UY_Number=@UY_Number and Number=@Number", par2.ToArray());
            if (ammeter != null && ammeter.Number != null)
            {
                return View(ammeter);
            }
            return View();
        }

        #region 电价

        /// <summary>
        /// 电价列表
        /// </summary>
        /// <returns></returns>
        public ActionResult AmmeterMoneyList()
        {
            var user = wbll.GetUserInfo(Request);

            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@UserNumber", user.Number));

            var ammeterMoneyList = database.FindList<Am_AmmeterMoney>(" and UserNumber=@UserNumber", parameter.ToArray());
            return View(ammeterMoneyList);
        }
        /// <summary>
        /// 电价设置
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AmmeterMoneyEidt(Am_AmmeterMoney model)
        {
            var user = wbll.GetUserInfo(Request);
            if (model.Number != null && model.Number != "")
            {
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@Number", model.Number));
                parameter.Add(DbFactory.CreateDbParameter("@UserNumber", user.Number));
                var ammeterMoney = database.FindEntityByWhere<Am_AmmeterMoney>(" and  Number=@Number and UserNumber=@UserNumber ", parameter.ToArray());
                if (ammeterMoney != null && ammeterMoney.Number != null)
                {
                    ammeterMoney.Classify = model.Classify;
                    ammeterMoney.First = model.First;
                    ammeterMoney.FirstMoney = model.FirstMoney;
                    ammeterMoney.Fourth = model.Fourth;
                    ammeterMoney.FourthMoney = model.FourthMoney;
                    ammeterMoney.Name = model.Name;
                    ammeterMoney.Remark = model.Remark;
                    ammeterMoney.Second = model.Second;
                    ammeterMoney.SecondMoney = model.SecondMoney;
                    ammeterMoney.Third = model.Third;
                    ammeterMoney.ThirdMoney = model.ThirdMoney;

                    var status = database.Update<Am_AmmeterMoney>(ammeterMoney);
                    if (status > 0)
                    {
                        return Json(new { res = "Ok", msg = "修改成功" });
                    }
                }

            }
            else
            {
                model.Number = Utilities.CommonHelper.GetGuid;
                model.Status = 1;
                model.CreateTime = DateTime.Now;
                model.UserName = user.Account;
                model.UserNumber = user.Number;
                model.UserRealName = user.Name;
                model.Classify = 0;
                model.First = 0;
                model.Fourth = 0;
                model.FourthMoney = 0;
                model.Second = 0;
                model.SecondMoney = 0;
                model.Third = 0;
                model.ThirdMoney = 0;
                model.StatusStr = "正常";
                var status = database.Insert<Am_AmmeterMoney>(model);
                if (status > 0)
                {
                    return Json(new { res = "Ok", msg = "添加成功" });
                }
            }
            return Json(new { res = "No", msg = "保存失败" });
        }
        public ActionResult AmmeterMoneyEidt(string number)
        {
            var user = wbll.GetUserInfo(Request);
            if (number != null && number != "")
            {
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@Number", number));
                parameter.Add(DbFactory.CreateDbParameter("@UserNumber", user.Number));
                var ammeterMoney = database.FindEntityByWhere<Am_AmmeterMoney>(" and  Number=@Number and UserNumber=@UserNumber ", parameter.ToArray());
                if (ammeterMoney != null && ammeterMoney.Number != null)
                {
                    return View(ammeterMoney);
                }
            }
            return View();
        }
        /// <summary>
        /// 删除电价
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public ActionResult AmmeterMoneyDel(string number)
        {
            var user = wbll.GetUserInfo(Request);
            if (number != null && number != "")
            {
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@AmmeterMoney_Number", number));


                var count = database.FindCount<Am_Ammeter>(" and AmmeterMoney_Number=@AmmeterMoney_Number", parameter.ToArray());
                if (count > 0)
                {
                    return Json(new { res = "No", msg = "删除失败，有电表在使用该电价" });
                }

                List<DbParameter> par1 = new List<DbParameter>();
                par1.Add(DbFactory.CreateDbParameter("@Number", number));
                par1.Add(DbFactory.CreateDbParameter("@UserNumber", user.Number));
                var ammeterMoney = database.FindEntityByWhere<Am_AmmeterMoney>(" and  Number=@Number and UserNumber=@UserNumber ", par1.ToArray());
                if (ammeterMoney != null && ammeterMoney.Number != null)
                {
                    var status = database.Delete<Am_AmmeterMoney>(ammeterMoney);
                    if (status > 0)
                    {
                        return Json(new { res = "Ok", msg = "删除成功" });
                    }
                }
            }
            return Json(new { res = "No", msg = "删除失败" });
        }
        #endregion



        /// <summary>
        /// 采集器列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult CollectorList(int pageIndex = 1, int pageSize = 10)
        {
            var user = wbll.GetUserInfo(Request);
            int recordCount = 0;

            List<DbParameter> par = new List<DbParameter>();
            par.Add(DbFactory.CreateDbParameter("@UNumber", user.Number));
            var list = database.FindListPage<Am_Collector>(" and UNumber=@UNumber ", par.ToArray(), "Number", "desc", pageIndex, pageSize, ref recordCount);
            ViewBag.recordCount = (int)Math.Ceiling(1.0 * recordCount / pageSize);
            if (list != null)
            {
                foreach (var item in list)
                {
                    List<DbParameter> par1 = new List<DbParameter>();
                    par1.Add(DbFactory.CreateDbParameter("@Collector_Number", item.Number));
                    var count = database.FindCount<Am_Ammeter>(" and Collector_Number=@Collector_Number", par1.ToArray());
                    item.AmCount = count.ToString();
                }
            }
            if (Request.IsAjaxRequest())
            {
                return Json(list);
            }
            else
            {
                return View();
            }
        }
        public ActionResult Collector(string number)
        {
            var user = wbll.GetUserInfo(Request);
            if (number != null && number != "")
            {
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@Number", number));
                parameter.Add(DbFactory.CreateDbParameter("@UNumber", user.Number));
                var collector = database.FindEntityByWhere<Am_Collector>(" and  Number=@Number and UNumber=@UNumber ", parameter.ToArray());
                if (collector != null && collector.Number != null)
                {
                    return View(collector);
                }
            }
            return View();
        }
        /// <summary>
        /// 采集器添加编辑
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult CollectorEdit(Am_Collector model)
        {
            var user = wbll.GetUserInfo(Request);
            Business.Base_ProvinceCityBll bll = new Business.Base_ProvinceCityBll();
            if (model.Number == null || model.Number == "")
            {
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@CollectorCode", model.CollectorCode));

                var isExist = database.FindEntityByWhere<Am_Collector>(" and  CollectorCode=@CollectorCode", parameter.ToArray());
                if (isExist != null && isExist.Number != null)
                {
                    return Json(new { res = "No", msg = "添加失败，已存在采集器" });
                }

                model.Number = Utilities.CommonHelper.GetGuid;
                model.LastConnectTime = DateTime.Now;
                model.CreateTime = DateTime.Now;
                model.STATUS = 0;
                model.StatusStr = "未连接";
                model.UNumber = user.Number;
                model.URealName = user.Name;
                model.UserName = user.Account;
                model.Province = bll.GetProvinceCityName(model.Province);
                model.City = bll.GetProvinceCityName(model.City);
                model.County = bll.GetProvinceCityName(model.County);

                var status = database.Insert<Am_Collector>(model);
                if (status > 0)
                {
                    return Json(new { res = "Ok", msg = "添加成功" });
                }
            }
            else
            {
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@Number", model.Number));
                parameter.Add(DbFactory.CreateDbParameter("@UNumber", user.Number));

                var collerctor = database.FindEntityByWhere<Am_Collector>(" and Number=@Number and UNumber=@UNumber", parameter.ToArray());
                if (collerctor != null && collerctor.Number != null)
                {
                    collerctor.Province = bll.GetProvinceCityName(model.Province);
                    collerctor.City = bll.GetProvinceCityName(model.City);
                    collerctor.County = bll.GetProvinceCityName(model.County);
                    collerctor.Remark = model.Remark;
                    collerctor.UNumber = model.UNumber;
                    collerctor.Address = model.Address;
                    collerctor.CollectorCode = model.CollectorCode;

                    var status = database.Update<Am_Collector>(collerctor);
                    if (status > 0)
                    {
                        return Json(new { res = "Ok", msg = "修改成功" });
                    }
                }
            }
            return Json(new { res = "No", msg = "提交失败" });
        }
        /// <summary>
        /// 电表历史用户
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult AmmeterUser(string Name, int pageIndex = 1, int pageSize = 10)
        {
            var user = wbll.GetUserInfo(Request);
            int recordCount = 0;

            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@UY_Number", user.Number));
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.Append(" and UY_Number=@UY_Number ");
            if (Name != null && Name != "")
            {
                parameter.Add(DbFactory.CreateDbParameter("@U_Name", Name));
                sbWhere.Append(" and U_Name=@U_Name ");
            }
            var ammeterList = database.FindListPage<Am_Ammeter>(sbWhere.ToString(), parameter.ToArray(), "CreateTime", "desc", pageIndex, pageSize, ref recordCount);
            ViewBag.recordCount = (int)Math.Ceiling(1.0 * recordCount / pageSize); ;
            if (Request.IsAjaxRequest())
            {
                return Json(ammeterList);
            }
            else
            {
                return View();
            }
        }
        /// <summary>
        /// 电表历史用户详情
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public ActionResult AmmeterUserDetailsList(string ammeterNumber, int pageIndex = 1, int pageSize = 10)
        {
            var user = wbll.GetUserInfo(Request);
            int recordCount = 0;
            List<DbParameter> par = new List<DbParameter>();
            par.Add(DbFactory.CreateDbParameter("@UY_Number", user.Number));
            par.Add(DbFactory.CreateDbParameter("@Number", ammeterNumber));

            var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and UY_Number=@UY_Number and Number=@Number ", par.ToArray());
            if (ammeter != null && ammeter.Number != null)
            {
                ViewBag.ammeter = ammeter;
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@Ammeter_Number", ammeterNumber));

                var ammeterPermissionList = database.FindListPage<Am_AmmeterPermission>(" and Ammeter_Number=@Ammeter_Number", parameter.ToArray(), "CreateTime", "desc", pageIndex, pageSize, ref recordCount);
                ViewBag.recordCount = (int)Math.Ceiling(1.0 * recordCount / pageSize); ;
                if (Request.IsAjaxRequest())
                {
                    return Json(ammeterPermissionList);
                }
            }
            return View();
        }
        public ActionResult ChargeManage()
        {
            return View();
        }
        public ActionResult BillManageNav()
        {
            return View();
        }
        /// <summary>
        /// 模板页
        /// </summary>
        /// <returns></returns>
        public ActionResult TemplateList(string ammeterNumber, int pageIndex = 1, int pageSize = 10)
        {
            var user = wbll.GetUserInfo(Request);
            int recordCount = 0;

            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@UY_Number", user.Number));
            StringBuilder sbWhere = new StringBuilder();
            sbWhere.Append(" and UY_Number=@UY_Number ");

            if (ammeterNumber != null && ammeterNumber != "")
            {
                parameter.Add(DbFactory.CreateDbParameter("@Ammeter_Number", user.Number));
                sbWhere.Append(" and Ammeter_Number=@Ammeter_Number ");
            }
            sbWhere.Append(" and Number in (select Ammeter_Number from Am_Template ) ");
            var ammeterList = database.FindListPage<Am_Ammeter>(sbWhere.ToString(), parameter.ToArray(), "CreateTime", "desc", pageIndex, pageSize, ref recordCount);

            ViewBag.recordCount = (int)Math.Ceiling(1.0 * recordCount / pageSize); ;
            if (Request.IsAjaxRequest())
            {
                return Json(ammeterList);
            }
            else
            {
                return View();
            }
        }
        /// <summary>
        /// 模板编辑
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public ActionResult Template(string ammeterNumber)
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> par = new List<DbParameter>();
            par.Add(DbFactory.CreateDbParameter("@UY_Number", user.Number));
            par.Add(DbFactory.CreateDbParameter("@Number", ammeterNumber));

            var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and UY_Number=@UY_Number and Number=@Number ", par.ToArray());
            if (ammeter != null && ammeter.Number != null)
            {
                List<DbParameter> par2 = new List<DbParameter>();
                par2.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));
                par2.Add(DbFactory.CreateDbParameter("@Ammeter_Number", ammeter.Number));

                var template = database.FindEntityByWhere<Am_Template>(" and U_Number=@U_Number and Ammeter_Number=@Ammeter_Number", par2.ToArray());
                ViewBag.template = template;
                if (template != null && template.Number != null)
                {
                    List<DbParameter> par3 = new List<DbParameter>();
                    par3.Add(DbFactory.CreateDbParameter("@Template_Number", template.Number));

                    var templateContent = database.FindList<Am_TemplateContent>(" and Template_Number=@Template_Number ");
                    ViewBag.templateContent = templateContent;
                }
                return View(ammeter);
            }
            return View();
        }
        [HttpPost]
        public JsonResult TemplateEidt(List<Am_TemplateContent> result, Am_Template template)
        {
            var user = wbll.GetUserInfo(Request);
            if (template.Number == null || template.Number == "")
            {
                template.Number = Utilities.CommonHelper.GetGuid;
                template.UserFromTime = DateTime.Now;
                template.CreateTime = DateTime.Now;
                template.U_Number = user.Number;
                template.U_Name = user.Name;
                template.UserName = user.Account;

                var status = database.Insert<Am_Template>(template);
                if (status > 0)
                {
                    foreach (var item in result)
                    {
                        if (item.ChargeItem_Title != null && item.ChargeItem_Title != "")
                        {
                            item.Template_Number = template.Number;
                            item.Template_Name = template.Name;
                            database.Insert<Am_TemplateContent>(item);
                        }
                    }
                    return Json(new { res = "Ok", msg = "添加成功" });
                }

            }
            else
            {
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@UY_Number", user.Number));
                parameter.Add(DbFactory.CreateDbParameter("@Number", template.Number));

                var templateModel = database.FindEntityByWhere<Am_Template>(" and Number=@Number and UY_Number=@UY_Number", parameter.ToArray());
                if (templateModel != null && templateModel.Number != null)
                {
                    templateModel.OtherFees = template.OtherFees;
                    if (database.Update<Am_Template>(templateModel) > 0)
                    {
                        List<DbParameter> par1 = new List<DbParameter>();
                        par1.Add(DbFactory.CreateDbParameter("@Template_Number", templateModel.Number));

                        database.Delete<Am_Template>(" Template_Number=@Template_Number", par1.ToArray());
                        foreach (var item in result)
                        {
                            if (item.ChargeItem_Title != null && item.ChargeItem_Title != "")
                            {
                                List<DbParameter> par2 = new List<DbParameter>();
                                par2.Add(DbFactory.CreateDbParameter("@Title", item.ChargeItem_Title));
                                par2.Add(DbFactory.CreateDbParameter("@Number", item.Number));

                                var chargeItem = database.FindEntityByWhere<Am_ChargeItem>(" and Number=@Number and Title=@Title ", par2.ToArray());
                                if (chargeItem != null && chargeItem.Number != null)
                                {
                                    item.Template_Number = template.Number;
                                }
                                else
                                {
                                    item.Template_Number = "0";
                                }
                                item.Template_Name = template.Name;
                                database.Insert<Am_TemplateContent>(item);
                            }
                        }
                    }
                }
            }
            return Json(result);
        }
        /// <summary>
        /// 收费项
        /// </summary>
        /// <returns></returns>
        public ActionResult ChargeItem()
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> par = new List<DbParameter>();
            par.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));

            var chargeItem = database.FindEntityByWhere<Am_ChargeItem>(" and U_Number=@U_Number or U_Number='System'", par.ToArray());
            if (chargeItem != null && chargeItem.Number != null)
            {
                return View(chargeItem);
            }
            return View();
        }
        /// <summary>
        /// 添加收费项
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult ChargeItemAdd(Am_ChargeItem model)
        {
            var user = wbll.GetUserInfo(Request);

            List<DbParameter> par = new List<DbParameter>();
            par.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));
            par.Add(DbFactory.CreateDbParameter("@Title", model.Title));

            var isExistModel = database.FindEntityByWhere<Am_ChargeItem>(" and U_Number=@U_Number or U_Number='System' and Title=@Title", par.ToArray());
            if (isExistModel != null && isExistModel.Number != null)
            {
                model.Number = Utilities.CommonHelper.GetGuid;
                model.UserName = user.Account;
                model.U_Name = user.Name;

                if (database.Insert<Am_ChargeItem>(model) > 0)
                {
                    return Json(new { res = "Ok", msg = "添加成功" });
                }
            }
            return Json(new { res = "No", msg = "添加失败" });
        }
        /// <summary>
        /// 出账账单管理
        /// </summary>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult Billing(int pageSize = 5)
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@F_U_Number", user.Number));

            var pending = database.FindCount<Am_Bill>(" and F_U_Number=@F_U_Number and  Status=0", parameter.ToArray());
            ViewBag.recordCount = (int)Math.Ceiling(1.0 * pending / pageSize);

            var processed = database.FindCount<Am_Bill>(" and F_U_Number=@F_U_Number and  Status=1", parameter.ToArray());
            ViewBag.recordCount1 = (int)Math.Ceiling(1.0 * processed / pageSize);
            return View();
        }
        /// <summary>
        /// 账单列表
        /// </summary>
        /// <param name="type"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult BillingList(int type, int pageIndex = 1, int pageSize = 5)
        {
            var user = wbll.GetUserInfo(Request);
            int recordCount = 0;
            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@F_U_Number", user.Number));
            parameter.Add(DbFactory.CreateDbParameter("@Status", type));

            var billlList = database.FindListPage<Am_Bill>(" and F_U_Number=@F_U_Number and  Status=@Status", parameter.ToArray(), "CreateTime", "desc", pageIndex, pageSize, ref recordCount);
            //ViewBag.recordCount = (int)Math.Ceiling(1.0 * recordCount / pageSize); 
            if (Request.IsAjaxRequest())
            {
                return Json(billlList);
            }
            else
            {
                return View();
            }
        }
        /// <summary>
        /// 出账详情
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public ActionResult BillingDetails(string number)
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@F_U_Number", user.Number));
            parameter.Add(DbFactory.CreateDbParameter("@Number", number));

            var bill = database.FindEntityByWhere<Am_Bill>(" and F_U_Number=@F_U_Number and Number=@Number", parameter.ToArray());
            if (bill != null && bill.Number != null)
            {
                List<DbParameter> par = new List<DbParameter>();
                par.Add(DbFactory.CreateDbParameter("@Bill_Number", bill.Number));
                var billContentList = database.FindList<Am_BillContent>(" and Bill_Number=@Bill_Number", par.ToArray());
                ViewBag.billContentList = billContentList;
                return View(bill);
            }
            return View();
        }
        /// <summary>
        /// 待出账编辑
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public ActionResult BillingEdit(string number)
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@F_U_Number", user.Number));
            parameter.Add(DbFactory.CreateDbParameter("@Number", number));
            parameter.Add(DbFactory.CreateDbParameter("@Status", "0"));

            var bill = database.FindEntityByWhere<Am_Bill>(" and F_U_Number=@F_U_Number and Number=@Number and Status=@Status", parameter.ToArray());
            if (bill != null && bill.Number != null)
            {
                List<DbParameter> par = new List<DbParameter>();
                par.Add(DbFactory.CreateDbParameter("@Bill_Number", bill.Number));
                var billContentList = database.FindList<Am_BillContent>(" and Bill_Number=@Bill_Number", par.ToArray());
                ViewBag.billContentList = billContentList;
                return View(bill);
            }
            return View();
        }
        /// <summary>
        /// 待出账编辑
        /// </summary>
        /// <param name="number"></param>.
        /// <param name="remark"></param>
        /// <param name="billList"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BillingEdit(string number, string remark, List<Am_BillContent> billList)
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@F_U_Number", user.Number));
            parameter.Add(DbFactory.CreateDbParameter("@Number", number));

            var bill = database.FindEntityByWhere<Am_Bill>(" and F_U_Number=@F_U_Number and Number=@Number", parameter.ToArray());
            if (bill != null && bill.Number != null)
            {
                List<DbParameter> par = new List<DbParameter>();
                par.Add(DbFactory.CreateDbParameter("@Bill_Number", bill.Number));
                var billContentList = database.FindList<Am_BillContent>("and Bill_Number=@Bill_Number", par.ToArray());
                if (billContentList.Count()>0&&database.Delete<Am_BillContent>(billContentList.Select(x=>x.Bill_Number).ToArray()) > 0)
                {
                    double total = 0;
                    var inserList =new  List<Am_BillContent>();
                    foreach (var item in billList)
                    {
                        if (item.ChargeItem_ChargeType==null )
                        {
                            item.ChargeItem_ChargeType = 0;
                        }
                        var billContent = new Am_BillContent
                        {
                            Bill_Code = bill.BillCode,
                            Bill_Number = bill.Number,
                            Number = Utilities.CommonHelper.GetGuid,
                            ChargeItem_ChargeType = item.ChargeItem_ChargeType,
                            ChargeItem_Number = item.ChargeItem_Number,
                            ChargeItem_Title = item.ChargeItem_Title,
                            Money = item.Money,
                            Remark = remark,
                            UMark = ""
                        };
                        total += item.Money.Value;
                        inserList.Add(billContent);
                        
                    }
                    if (database.Insert<Am_BillContent>(inserList)>0)
                    {
                        total = total + bill.OtherFees.Value;
                        bill.Money = total;
                        if (database.Update<Am_Bill>(bill) > 0)
                        {
                            return Json(new { res = "Ok", msg = "提交成功" });
                        }
                    } 
                }
            }
            return Json(new { res = "No", msg = "提交失败" });
        }
        /// <summary>
        /// 出账未付款处理
        /// </summary>
        /// <param name="number"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BillingHandle(string number,string remark)
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@F_U_Number", user.Number));
            parameter.Add(DbFactory.CreateDbParameter("@Number", number));
            parameter.Add(DbFactory.CreateDbParameter("@Status", "1"));

            var bill = database.FindEntityByWhere<Am_Bill>(" and F_U_Number=@F_U_Number and Number=@Number and Status=@Status ", parameter.ToArray());
            if (bill != null && bill.Number != null)
            {
                bill.Status = 2;
                bill.StatusStr = "已支付";
                bill.Remark = remark;
                if (database .Update<Am_Bill>(bill)>0)
                {
                    return Json(new { res = "Ok", msg = "提交成功" });
                }
            }
            return Json(new { res = "No", msg = "提交失败" });
        }
        /// <summary>
        /// 关联用户
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public ActionResult BindUser(string number)
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> par = new List<DbParameter>();
            par.Add(DbFactory.CreateDbParameter("@UY_Number", user.Number));
            par.Add(DbFactory.CreateDbParameter("@Number", number));

            var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and UY_Number=@UY_Number and Number=@Number ", par.ToArray());
            if (ammeter != null && ammeter.Number != null)
            {
                List<DbParameter> parAmmeterPermission = new List<DbParameter>();
                parAmmeterPermission.Add(DbFactory.CreateDbParameter("@UY_Number", user.Number));
                parAmmeterPermission.Add(DbFactory.CreateDbParameter("@Ammeter_Number", ammeter.Number));
                parAmmeterPermission.Add(DbFactory.CreateDbParameter("@Status", "1"));
                //关联用户
                var ammeterPermission = database.FindEntityByWhere<Am_AmmeterPermission>(" and UY_Number=@UY_Number and Ammeter_Number=@Ammeter_Number and Status=@Status ", parAmmeterPermission.ToArray());
                if (ammeterPermission!=null&& ammeterPermission.Number!=null )
                {
                    ViewBag.ammeterPermission = ammeterPermission;
                }

                List<DbParameter> par1 = new List<DbParameter>();
                par1.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));
                par1.Add(DbFactory.CreateDbParameter("@Ammeter_Number", number));
                //电表模板
                var template = database.FindEntityByWhere<Am_Template>(" and U_Number=@U_Number and Ammeter_Number=@Ammeter_Number ", par1.ToArray());
                if (template!=null && template.Number !=null )
                {
                    ViewBag.template = template;
                    List<DbParameter> par2 = new List<DbParameter>();
                    par2.Add(DbFactory.CreateDbParameter("@Template_Number", template.Number));
                    var templateContentList = database.FindList<Am_Template>(" and UY_Number=@UY_Number and Ammeter_Number=@Ammeter_Number ", par2.ToArray());
                    //模板内容
                    if (templateContentList != null && templateContentList.Count() >0)
                    {
                        ViewBag.templateContent = templateContentList;
                    }
                }
                else
                {
                    List<DbParameter> par3 = new List<DbParameter>();
                    par3.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));
                    par3.Add(DbFactory.CreateDbParameter("@U_Number2", "System"));
                    //收费项
                    var chargeItemList = database.FindList<Am_ChargeItem>(" and U_Number=@U_Number or U_Number=@U_Number2 ",par3.ToArray());
                    ViewBag.chargeItemList = chargeItemList;
                }
                return View(ammeter);
            }
            return View();
        }
        [HttpPost]
        public ActionResult BindUser(string number ,string phone,string name,string starTime,string endTime,string cycle,string lateFee, List<Am_ChargeItem> itemList)
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> par = new List<DbParameter>();
            par.Add(DbFactory.CreateDbParameter("@UY_Number", user.Number));
            par.Add(DbFactory.CreateDbParameter("@Number", number));

            var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and UY_Number=@UY_Number and Number=@Number ", par.ToArray());
            if (ammeter != null && ammeter.Number != null)
            {
                List<DbParameter> parAmmeterPermission = new List<DbParameter>();
                parAmmeterPermission.Add(DbFactory.CreateDbParameter("@UY_Number", user.Number));
                parAmmeterPermission.Add(DbFactory.CreateDbParameter("@Ammeter_Number", ammeter.Number));
                parAmmeterPermission.Add(DbFactory.CreateDbParameter("@Status", "1"));
                //关联用户
                var ammeterPermission = database.FindEntityByWhere<Am_AmmeterPermission>(" and UY_Number=@UY_Number and Ammeter_Number=@Ammeter_Number and Status=@Status ", parAmmeterPermission.ToArray());
                if (ammeterPermission != null && ammeterPermission.Number != null)
                {
                    if (ammeterPermission.UserName.Equals(phone))
                    {
                        
                    }
                    else
                    {

                    }
                }
                else
                {

                }

            }
                return View();
        }


        #region 报修管理
        /// <summary>
        /// 报修记录
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult RepairManagement(int pageSize = 5)
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));

            var pending = database.FindCount<Am_Repair>(" and F_Number=@U_Number and  Status=0", parameter.ToArray());
            ViewBag.recordCount = (int)Math.Ceiling(1.0 * pending / pageSize);

            var processed = database.FindCount<Am_Repair>(" and F_Number=@U_Number and  Status=1", parameter.ToArray());
            ViewBag.recordCount1 = (int)Math.Ceiling(1.0 * processed / pageSize);
            return View();
        }
        /// <summary>
        /// 获取报修记录
        /// </summary>
        /// <param name="type"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult RepairRecordList(int type, int pageIndex = 1, int pageSize = 5)
        {
            var user = wbll.GetUserInfo(Request);
            int recordCount = 0;
            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));
            parameter.Add(DbFactory.CreateDbParameter("@Status", type));

            var repairlList = database.FindListPage<Am_Repair>(" and F_Number=@U_Number and  Status=@Status", parameter.ToArray(), "CreateTime", "desc", pageIndex, pageSize, ref recordCount);
            //ViewBag.recordCount = (int)Math.Ceiling(1.0 * recordCount / pageSize); ;
            if (Request.IsAjaxRequest())
            {
                return Json(repairlList);
            }
            else
            {
                return View();
            }

        }
        /// <summary>
        /// 报修详情
        /// </summary>
        /// <param name="Number"></param>
        /// <returns></returns>
        public ActionResult RepairInfo(string number)
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> par = new List<DbParameter>();
            par.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));
            par.Add(DbFactory.CreateDbParameter("@Number", number));

            var repair = database.FindEntityByWhere<Am_Repair>(" and Number=@Number and F_Number=@U_Number", par.ToArray());
            if (repair != null && repair.Number != null)
            {
                List<DbParameter> par1 = new List<DbParameter>();
                par1.Add(DbFactory.CreateDbParameter("@Repair_Number", repair.Number));

                var repairImage = database.FindList<Am_RepairImage>(" and  Repair_Number=@Repair_Number ", par1.ToArray());
                ViewBag.repairImage = repairImage;

                List<DbParameter> par2 = new List<DbParameter>();
                par2.Add(DbFactory.CreateDbParameter("@Repair_Number", repair.Number));
                var repairAnswer = database.FindEntityByWhere<Am_RepairAnswer>(" and Repair_Number=@Repair_Number ", par2.ToArray());
                ViewBag.repairAnswer = repairAnswer;

                return View(repair);
            }
            return View();
        }
        /// <summary>
        /// 获取维修师傅
        /// </summary>
        /// <returns></returns>
        public ActionResult GetMyUserWXList()
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> par = new List<DbParameter>();
            par.Add(DbFactory.CreateDbParameter("@As_Number", user.Number));
            var PartnerUserList = database.FindList<Ho_PartnerUser>(" and UserRole = '维修师傅' and As_Number=@As_Number", par.ToArray());
            //if (PartnerUser != null && PartnerUser.Number != null)
            //{
            //    return Json(PartnerUser);
            //}
            //return View();
            return Json(PartnerUserList);
        }
        /// <summary>
        /// 报修处理提交
        /// </summary>
        /// <param name="Number"></param>
        /// <param name="U_Number"></param>
        /// <param name="AContent"></param>
        /// <returns></returns>
        public ActionResult SubmitRepairAnswer(string Number, string U_Number, string AContent)
        {
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                var repair = database.FindEntity<Am_Repair>(Number);
                var wxuser = database.FindEntity<Ho_PartnerUser>(U_Number);
                if (repair != null && repair.Number != null)
                {

                    var answer = new Am_RepairAnswer()
                    {
                        AContent = AContent,
                        Mobile = wxuser.Mobile,
                        CreateTime = DateTime.Now,
                        RepairCode = repair.RepairCode,
                        Repair_Number = repair.Number,
                        STATUS = 1,
                        StatusStr = "已处理",
                        UserName = wxuser.Account,
                        U_Name = wxuser.Name,
                        U_Number = wxuser.Number
                    };
                    answer.Create();
                    var result = database.Insert(answer, isOpenTrans); //添加报修记录
                    //更新报修表
                    repair.Status = 1;
                    repair.StatusStr = "已处理";
                    repair.Modify(repair.Number);
                    repair.RepairCode = null;   //一定要null,因为数据库为自增字段,所以不允许update此字段
                    result += database.Update(repair, isOpenTrans);
                    if (result < 2) //处理失败
                    {
                        database.Rollback();
                        return Json(new { res = "No", msg = "提交失败" });
                    }
                    else
                    {
                        //发送微信通知给师傅


                        database.Commit();
                        return Json(new { res = "Ok", msg = "提交成功" });
                    }
                }
            }
            catch
            {
                isOpenTrans.Rollback();
            }
            return Json(new { res = "No", msg = "提交失败" });
        }
        #endregion
    }
}
