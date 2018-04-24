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
        public ActionResult Ammeter(string Number)
        {
            var user = wbll.GetUserInfo(Request);

            var ammeterTpyeList = database.FindList<Am_AmmeterType>("");

            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@UNumber", user.Number));
            var collectorList = database.FindList<Am_Collector>(" and UNumber=@UNumber", parameter.ToArray());

            List<DbParameter> par1 = new List<DbParameter>();
            par1.Add(DbFactory.CreateDbParameter("@UserNumber", user.Number));
            var ammeterMoneyList = database.FindList<Am_AmmeterMoney>(" and UserNumber=@UserNumber", par1.ToArray());
            if (Number != null && Number != "")
            {

            }
            else
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
                List<DbParameter> par2 = new List<DbParameter>();
                par2.Add(DbFactory.CreateDbParameter("@UY_Number", user.Number));
                par2.Add(DbFactory.CreateDbParameter("@Number", model.Number));
                var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and UY_Number=@UY_Number and Number=@Number", par2.ToArray());
                if (ammeter != null && ammeter.Number != null)
                {
                    ammeter.AmmeterType_Number = model.AmmeterType_Number;
                    ammeter.AmmeterType_Name = model.AmmeterType_Name;
                    ammeter.AmmeterType_Name = model.AmmeterType_Name;
                    ammeter.AmmeterType_Number = model.AmmeterType_Number;
                    ammeter.Address = model.Address;
                    ammeter.AllMoney = model.AllMoney;
                    ammeter.AM_Code = model.AM_Code;
                    ammeter.Cell = model.Cell;
                    ammeter.City = model.City;
                    ammeter.Collector_Code = model.Collector_Code;
                    ammeter.Collector_Number = model.Collector_Number;
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
                    ammeter.UserName = model.UserName;
                    ammeter.U_Name = model.U_Name;
                    ammeter.U_Number = model.U_Number;
                    ammeter.Remark = model.Remark;

                    var status = database.Update<Am_Ammeter>(ammeter);
                    if (status > 0)
                    {
                        return Json(new { res = "Ok", msg = "修改成功" });
                    }
                }
                else
                {
                    model.Number = Utilities.CommonHelper.GetGuid;
                    model.CreateTime = DateTime.Now;
                    model.Status = 1;
                    model.CM_Time = DateTime.Now;
                    model.CP_Time = DateTime.Now;
                    model.UpdateTime = DateTime.Now;
                    model.UY_Name = user.Name;
                    model.UY_Number = user.Number;
                    model.UY_UserName = user.Accout;

                    var status = database.Insert<Am_Ammeter>(model);
                    if (status > 0)
                    {
                        return Json(new { res = "Ok", msg = "添加成功" });
                    }
                }
            }
            return Json(new { res = "Ok", msg = "修改成功" });
        }

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
                model.UserName = user.Accout;
                model.UserNumber = user.Number;
                model.UserRealName = user.Name;
                var status = database.Insert<Am_AmmeterMoney>(model);
                if (status > 0)
                {
                    return Json(new { res = "Ok", msg = "添加成功" });
                }
            }
            return Json(new { res = "No", msg = "保存失败" });
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
                parameter.Add(DbFactory.CreateDbParameter("@AmmeterMoneyNumber", user.Number));

                var count = database.FindCount<Am_AmmeterMoney>(" and AmmeterMoneyNumber=@AmmeterMoneyNumber", parameter.ToArray());
                if (count>0)
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
                    if (status>0)
                    {
                        return Json(new { res = "Ok", msg = "删除成功" });
                    }
                }
            }
            return Json(new { res = "No", msg = "删除失败" });
        }
        /// <summary>
        /// 电表列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult AmmeterList(int pageIndex = 1, int pageSize = 10)
        {
            var user = wbll.GetUserInfo(Request);
            int recordCount = 0;

            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@UY_Number", user.Number));

            StringBuilder sbWhere = new StringBuilder();
            sbWhere.Append(" and UY_Number=@UY_Number ");

            var ammeterList = database.FindListPage<Am_Ammeter>(sbWhere.ToString (), parameter.ToArray(),"Number", "desc", pageIndex, pageSize, ref recordCount);
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
        public ActionResult AmmeterInfo(string  number )
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
            if (list != null)
            {
                foreach (var item in list)
                {
                    List<DbParameter> par1 = new List<DbParameter>();
                    par1.Add(DbFactory.CreateDbParameter("@CollectorNumber", item.Number));
                    var count = database.FindCount<Am_Ammeter>(" and CollectorNumber=@CollectorNumber",par1.ToArray());
                    item.AmCount = count.ToString ();
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
        public ActionResult Collector()
        {
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
            if (model.Number ==null ||model .Number=="")
            {
                model.Number = Utilities.CommonHelper.GetGuid;
                model.LastConnectTime = DateTime.Now;
                model.CreateTime = DateTime.Now;
                model.STATUS = 0;
                model.StatusStr = "未连接";
                model.UNumber = user.Number;
                model.URealName = user.Name;
                model.UserName = user.Accout;

                var status = database.Insert<Am_Collector>(model);
                if (status>0)
                {
                    return Json(new { res = "Ok", msg = "添加成功" });
                }
            }
            else
            {
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter("@Number", model.Number));
                parameter.Add(DbFactory.CreateDbParameter("@UNumber", model.Number));

                var collerctor = database.FindEntityByWhere<Am_Collector>(" and Number=@Number and UNumber=@UNumber",parameter.ToArray());
                if (collerctor!=null && collerctor.Number !=null )
                {
                    collerctor.Province = model.Province;
                    collerctor.Remark = model.Remark;
                    collerctor.UNumber = model.UNumber;

                    collerctor.Address = model.Address;
                    collerctor.City = model.City;
                    collerctor.CollectorCode = model.CollectorCode;
                    collerctor.County = model.County;
                    var status = database.Update<Am_Collector>(collerctor);
                    if (status>0)
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
            parameter.Add(DbFactory.CreateDbParameter("@U_Name", Name));


            StringBuilder sbWhere = new StringBuilder();
            sbWhere.Append(" and UY_Number=@UY_Number ");
            sbWhere.Append(" and U_Name=@U_Name ");


            var ammeterList = database.FindListPage<Am_AmmeterPermission>(sbWhere.ToString(), parameter.ToArray(), "Number", "desc", pageIndex, pageSize, ref recordCount);
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
        public ActionResult AmmeterUserDetails(string number)
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@UY_Number", user.Number));
            parameter.Add(DbFactory.CreateDbParameter("@Number", number));
            var ammeter = database.FindEntityByWhere<Am_AmmeterPermission>(" and UY_Number=@UY_Number and Number=@Number", parameter.ToArray());
            if (ammeter != null && ammeter.Number != null)
            {
                return View(ammeter);
            }
            return View();
        }
        /// <summary>
        /// 模板页
        /// </summary>
        /// <returns></returns>
        public ActionResult Template(int pageIndex = 1, int pageSize = 10)
        {
             
            var user = wbll.GetUserInfo(Request);
            int recordCount = 0;
 
            List<DbParameter> parameter = new List<DbParameter>();
            parameter.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));


            StringBuilder sbWhere = new StringBuilder();
            sbWhere.Append(" and U_Number=@U_Number ");


            var templateList = database.FindListPage<Am_Template>(sbWhere.ToString(), parameter.ToArray(), "Number", "desc", pageIndex, pageSize, ref recordCount);
            ViewBag.recordCount = (int)Math.Ceiling(1.0 * recordCount / pageSize); ;
            if (Request.IsAjaxRequest())
            {
                return Json(templateList);
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
        public ActionResult Template(string number)
        {
            var user = wbll.GetUserInfo(Request);
            List<DbParameter> par2 = new List<DbParameter>();
            par2.Add(DbFactory.CreateDbParameter("@U_Number", user.Number));
            par2.Add(DbFactory.CreateDbParameter("@Number", number));

            var template = database.FindEntityByWhere<Am_Template>(" and U_Number=@U_Number and Number=@Number", par2.ToArray());
            if (template != null && template.Number != null)
            {
                List<DbParameter> par3 = new List<DbParameter>();
                par3.Add(DbFactory.CreateDbParameter("@Template_Number", template.Number));

                var templateContent = database.FindList<Am_TemplateContent>(" and Template_Number=@Template_Number ");
                ViewBag.templateContent = templateContent;
                return View(template);
            }
            return View();
        }
        [HttpPost]
        public JsonResult TemplateEidt(List<Am_TemplateContent> result,Am_Template template)
        {
            var user = wbll.GetUserInfo(Request);
            if (template.Number==null||template .Number=="" )
            {
                template.Number= Utilities.CommonHelper.GetGuid;
                template.UserFromTime = DateTime.Now;
                template.CreateTime = DateTime.Now;
                template.U_Number = user.Number;
                template.U_Name = user.Name;
                template.UserName = user.Accout;

                var status = database.Insert<Am_Template>(template);
                if (status>0)
                {
                    foreach (var item in result)
                    {
                        if (item.ChargeItem_Title!=null &&item .ChargeItem_Title != "")
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
                if (templateModel!=null && templateModel.Number !=null )
                {
                    templateModel.OtherFees = template.OtherFees;
                    if (database.Update<Am_Template>(templateModel)>0)
                    {
                        List<DbParameter> par1 = new List<DbParameter>();
                        par1.Add(DbFactory.CreateDbParameter("@Template_Number", templateModel.Number));
                         
                        database.Delete<Am_Template>(" Template_Number=@Template_Number",par1.ToArray());
                        foreach (var item in result)
                        {
                            if (item.ChargeItem_Title != null && item.ChargeItem_Title != "")
                            {
                                List<DbParameter> par2 = new List<DbParameter>();
                                par2.Add(DbFactory.CreateDbParameter("@Title", item.ChargeItem_Title));
                                par2.Add(DbFactory.CreateDbParameter("@Number", item.Number));

                                var chargeItem = database.FindEntityByWhere<Am_ChargeItem>(" and Number=@Number and Title=@Title ", par2.ToArray());
                                if (chargeItem!=null && chargeItem.Number !=null )
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
                model.UserName = user.Accout;
                model.U_Name = user.Name;

                if (database.Insert<Am_ChargeItem>(model)>0)
                {
                    return Json(new { res = "Ok", msg = "添加成功" });
                }
            }
            return Json(new { res = "No", msg = "添加失败" });
        }

        
    }
}
