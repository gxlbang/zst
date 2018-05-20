﻿using LeaRun.DataAccess;
using LeaRun.Entity;
using LeaRun.Repository;
using LeaRun.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.ServiceProcess;

namespace ZstDataHandle
{
    public partial class Service1 : ServiceBase
    {
        System.Timers.Timer timer = new System.Timers.Timer();
        System.Timers.Timer BIllTimer = new System.Timers.Timer();
        System.Timers.Timer ReadingTimer = new System.Timers.Timer();
        System.Timers.Timer ReadingHandleTimer = new System.Timers.Timer();

        IDatabase database = DataFactory.Database();

        public Service1()
        {
            InitializeComponent();
            //1.账单生成
            //2.账单推送
            //3.抄表
            //4.电表异步处理

            //前台异步处理
            timer.Elapsed += new System.Timers.ElapsedEventHandler(TimedEvent);
            timer.Interval = 5000;//每5秒执行一次
            timer.Enabled = true;

            //账单生成
            BIllTimer.Elapsed += new System.Timers.ElapsedEventHandler(BIllTimerdEvent);
            BIllTimer.Interval = 5000;//每5秒执行一次
            BIllTimer.Enabled = true;

            //抄表生成
            ReadingTimer.Elapsed += new System.Timers.ElapsedEventHandler(ReadingTimerEvent);
            ReadingTimer.Interval = 1000 * 60 * 5;//每5分钟执行一次
            ReadingTimer.Enabled = true;


            ReadingHandleTimer.Elapsed += new System.Timers.ElapsedEventHandler(ReadingHandleTimerTimerEvent);
            ReadingHandleTimer.Interval = 5000;//每5秒执行一次
            ReadingHandleTimer.Enabled = true;

        }
        /// <summary>
        /// 异步处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimedEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            timer.Stop();
            var taskList = DbHelper.GetTaskList(0);
            foreach (var item in taskList)
            {
                var result = GetResult(item.Number);
                var root = JsonHelper.JonsToList<Root>(result);
                if (root != null)
                {
                    if (root.Count == 1)
                    {
                        var example = root[0];
                        item.TaskMark = example.status;
                        if (example.status == "SUCCESS")
                        {
                            item.Status = 1;
                            item.StatusStr = "成功";
                            if (example.data != null)
                            {
                                if (example.data[0].type==20)
                                {
                                    item.Remark ="剩余电量:" +example.data[0].dsp;
                                }
                                else if (example.data[0].type == 22)
                                {
                                    item.Remark = "剩余金额:" + example.data[0].dsp;
                                }
                                
                            }

                            item.OverTime = DateTime.Parse(example.resolve_time);

                            if (item.OperateType == 8)
                            {
                                List<DbParameter> par = new List<DbParameter>();
                                par.Add(DbFactory.CreateDbParameter("@Number", item.AmmeterNumber));
                                var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and Number =@Number ", par.ToArray());
                                if (ammeter != null && ammeter.Number != null)
                                {
                                    ammeter.Count = 2;
                                    ammeter.Status = 1;
                                    ammeter.StatusStr = "已开户";
                                    ammeter.Acount_Id = null;
                                    database.Update<Am_Ammeter>(ammeter);
                                }
                                item.Remark = "开户成功";

                            }
                            else if (item.OperateType == 4|| item.OperateType == 9)
                            {
                                List<DbParameter> par = new List<DbParameter>();
                                par.Add(DbFactory.CreateDbParameter("@Number", item.AmmeterNumber));
                                var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and Number =@Number ", par.ToArray());
                                if (ammeter != null && ammeter.Number != null)
                                {
                                    ammeter.Count = ammeter.Count + 1;
                                    ammeter.Acount_Id = null;
                                    database.Update<Am_Ammeter>(ammeter);
                                }
                                if (result.Contains("params"))
                                {
                                    var pr = JsonHelper.JonsToList<Root>(result.Replace("params", "paramsContent"));
                                    if (pr[0].paramsContent!=null )
                                    {
                                        item.Remark = "充值成功:"+ pr[0].paramsContent.money+"元";
                                    }
                                    item.TaskMark = result;
                                }

                            }
                            else if (item.OperateType == 1)
                            {
                                item.Remark = "合闸成功";
                            }
                            else if (item.OperateType == 2)
                            {
                                item.Remark = "拉闸成功";
                            }
                        }
                        else if (example.status == "RESPONSE_FAIL")
                        {
                            item.Status = 2;
                            item.StatusStr = "失败";
                            item.OverTime = DateTime.Parse(example.resolve_time);
                            item.Remark = example.error_msg;
                        }
                        else if (example.status == "FAIL")
                        {
                            item.Status = 2;
                            item.StatusStr = "失败";
                            item.OverTime = DateTime.Parse(example.resolve_time);
                        }
                        else if (example.status == "NOTSUPPORT")
                        {
                            item.Status = 2;
                            item.StatusStr = "不支持此功能";
                            item.OverTime = DateTime.Parse(example.resolve_time);
                        }
                        else if (example.status == "TIMEOUT")
                        {
                            item.Status = 2;
                            item.StatusStr = "超时";
                            item.OverTime = DateTime.Parse(example.resolve_time);
                        }
                        else if (example.status == "ACCEPTED")
                        {
                            item.StatusStr = "请求已接受";
                        }
                        else if (example.status == "QUEUE")
                        {
                            item.StatusStr = "调度状态";
                        }
                        else if (example.status == "PROCESSING")
                        {
                            item.StatusStr = "正在处理中";
                        }
                        DbHelper.UpdateTask(item);
                    }
                }
                if (item.Status > 0 && item.Status < 4)
                {
                    var log = DbHelper.GetLog(item.Number);
                    if (log != null)
                    {
                        log.Result = item.StatusStr;
                        DbHelper.UpdateLog(log);
                    }
                    if (root.Count == 1)
                    {
                        var example = root[0];
                        if (example.data != null && example.data.Count > 0)
                        {
                            int type = example.data[0].type;
                            //查询余额
                            if (type == 22)
                            {
                                var ammeter = DbHelper.GetAmmeter(item.AmmeterNumber);
                                if (ammeter != null)
                                {
                                    ammeter.CurrMoney = decimal.Parse(example.data[0].value[0].ToString());
                                    ammeter.CM_Time = DateTime.Parse(example.resolve_time);
                                    DbHelper.UpdateAmmeter(ammeter);
                                }
                            }
                            //查询电量
                            if (type == 20)
                            {
                                var ammeter = DbHelper.GetAmmeter(item.AmmeterNumber);
                                if (ammeter != null)
                                {
                                    ammeter.CurrPower = decimal.Parse(example.data[0].value[0].ToString()).ToString("0.00");
                                    ammeter.CP_Time = DateTime.Parse(example.resolve_time);
                                    DbHelper.UpdateAmmeter(ammeter);
                                }
                            }
                        }
                    }
                }
                if (item.Status==2&&item.OperateType==4)
                {
                    List<DbParameter> par = new List<DbParameter>();
                    par.Add(DbFactory.CreateDbParameter("@Number", item.U_Number));
                    par.Add(DbFactory.CreateDbParameter("@Status", "3"));
                    var user = database.FindEntityByWhere<Ho_PartnerUser>(" and Number =@Number and Status=@Status ", par.ToArray());
                    if (user != null && user.Number != null)
                    {

                    }
                }

            }
            timer.Start();

            //业务逻辑代码
        }
        /// <summary>
        /// 账单生成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BIllTimerdEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            BIllTimer.Stop();
            var config = database.FindEntityByWhere<Fx_WebConfig>("");

            if (config != null && config.BillDate > 0)
            {
                var time = DateTime.Now.Date.AddDays(config.BillDate.Value);
                var pList = DbHelper.GetAmmeterPermissionList(time);
                foreach (var item in pList)
                {
                    var ammeter = DbHelper.GetAmmeter(item.Ammeter_Number);
                    if (ammeter != null)
                    {
                        List<DbParameter> par = new List<DbParameter>();
                        par.Add(DbFactory.CreateDbParameter("@Ammeter_Number", ammeter.Number));


                        var templateContentList = database.FindList<Am_TemplateContent>(" and  Template_Number =(select top(1) number from  Am_Template  where Ammeter_Number=@Ammeter_Number ) ", par.ToArray());
                        double money = 0.00;
                        foreach (var centent in templateContentList)
                        {
                            //一次性账单
                            if (centent.ChargeItem_ChargeType == 1)
                            {
                                if (item.BeginTime == item.LastPayBill)
                                {
                                    money = money + centent.Money.Value;
                                }
                            }
                            else
                            {
                                money = money + centent.Money.Value;
                            }
                        }
                        //推送时间
                        var sendTime = DateTime.Now;
                        if (item.BeginTime == item.LastPayBill)
                        {
                            sendTime = item.BeginTime.Value;
                        }
                        else
                        {
                            sendTime = item.LastPayBill.Value.AddDays(-config.SendBillDate.Value);
                        }

                        var bill = new LeaRun.Entity.Am_Bill
                        {
                            Address = ammeter.Address,
                            AmmeterCode = item.Ammeter_Code,
                            AmmeterNumber = item.Ammeter_Number,
                            Cell = ammeter.Cell,
                            City = ammeter.City,
                            County = ammeter.County,
                            CreateTime = DateTime.Now,
                            Floor = ammeter.Floor,
                            F_UserName = item.UserName,
                            F_U_Name = item.UY_Name,
                            F_U_Number = item.UY_Number,
                            Money = money,
                            Number = CommonHelper.GetGuid,
                            OtherFees = 0,
                            PayTime = DateTime.Now,
                            Province = ammeter.Province,
                            Remark = "",
                            Room = ammeter.Room,
                            SendTime = sendTime,
                            Status = 0,
                            StatusStr = "待支付",
                            T_UserName = item.U_Name,
                            T_U_Name = item.UY_UserName,
                            T_U_Number = item.U_Number,
                            BeginTime = item.LastPayBill,
                            EndTime = item.LastPayBill.Value.AddMonths(item.BillCyc.Value)
                        };
                        var count = 100000 + database.FindCount<Am_Bill>() + 1;
                        var code = item.LastPayBill.Value.ToString("yyyyMMdd") + "-" + count;
                        bill.BillCode = code;
                        if (database.Insert<Am_Bill>(bill) > 0)
                        {
                            //账单收费项
                            foreach (var content in templateContentList)
                            {
                                var contentModel = new Am_BillContent
                                {
                                    ChargeItem_ChargeType = content.ChargeItem_ChargeType,
                                    ChargeItem_Number = content.ChargeItem_Number,
                                    ChargeItem_Title = content.ChargeItem_Title,
                                    Money = content.Money,
                                    Bill_Code = bill.BillCode,
                                    Bill_Number = bill.Number,
                                    Number = CommonHelper.GetGuid,
                                    Remark = "",
                                    UMark = ""
                                };
                                database.Insert<Am_BillContent>(contentModel);
                            }

                            if (item.BeginTime != item.LastPayBill)
                            {
                                item.LastPayBill = item.LastPayBill.Value.AddMonths(item.BillCyc.Value);
                                database.Update<Am_AmmeterPermission>(item);
                            }
                        }
                    }


                }
            }
            BIllTimer.Start();
        }

        /// <summary>
        /// 抄表任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReadingTimerEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            ReadingTimer.Stop();
            var ampsList = database.FindList<Am_AmmeterPermission>(" and Status=1 ");
            foreach (var item in ampsList)
            {
                List<DbParameter> parAmmeter = new List<DbParameter>();
                parAmmeter.Add(DbFactory.CreateDbParameter("@Number", item.Ammeter_Number));
                var ammeter = database.FindEntityByWhere<Am_Ammeter>(" and Number=@Number ", parAmmeter.ToArray());
                if (ammeter != null && ammeter.Number != null)
                {
                    Reading(ammeter, 20);
                    Reading(ammeter, 22);
                }
            }
            ReadingTimer.Start();
        }
        /// <summary>
        /// 后台抄表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReadingHandleTimerTimerEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            ReadingHandleTimer.Stop();
            var taskList = database.FindList<Am_BackstageTask>(" and Status=0");
            foreach (var item in taskList)
            {
                var result = GetResult(item.OrderNumber);
                var root = JsonHelper.JonsToList<Root>(result);
                if (root != null)
                {
                    if (root.Count == 1)
                    {
                        var example = root[0];
                        item.TaskMark = example.status;
                        if (example.status == "SUCCESS")
                        {
                            item.Status = 1;
                            item.StatusStr = "成功";
                            if (example.data != null)
                            {
                                item.Remark = example.data[0].dsp;
                            }

                            item.OverTime = DateTime.Parse(example.resolve_time);
                        }
                        else if (example.status == "FAIL")
                        {
                            item.Status = 2;
                            item.StatusStr = "失败";
                            item.OverTime = DateTime.Parse(example.resolve_time);
                        }
                        else if (example.status == "NOTSUPPORT")
                        {
                            item.Status = 2;
                            item.StatusStr = "不支持此功能";
                            item.OverTime = DateTime.Parse(example.resolve_time);
                        }
                        else if (example.status == "TIMEOUT")
                        {
                            item.Status = 2;
                            item.StatusStr = "超时";
                            item.OverTime = DateTime.Parse(example.resolve_time);
                        }
                        else if (example.status == "ACCEPTED")
                        {
                            item.StatusStr = "请求已接受";
                        }
                        else if (example.status == "QUEUE")
                        {
                            item.StatusStr = "调度状态";
                        }
                        else if (example.status == "PROCESSING")
                        {
                            item.StatusStr = "正在处理中";
                        }
                        database.Update<Am_BackstageTask>(item);
                    }
                }
                if (item.Status > 0 && item.Status < 4)
                {
                    var log = DbHelper.GetLog(item.Number);
                    if (log != null)
                    {
                        log.Result = item.StatusStr;
                        DbHelper.UpdateLog(log);
                    }
                    if (root.Count == 1)
                    {
                        var example = root[0];
                        if (example.data != null && example.data.Count > 0)
                        {
                            int type = example.data[0].type;
                            //查询余额
                            if (type == 22)
                            {
                                var ammeter = DbHelper.GetAmmeter(item.AmmeterNumber);
                                if (ammeter != null)
                                {
                                    ammeter.CurrMoney = decimal.Parse(example.data[0].value[0].ToString());
                                    ammeter.CM_Time = DateTime.Parse(example.resolve_time);
                                    DbHelper.UpdateAmmeter(ammeter);
                                }
                            }
                            //查询电量
                            if (type == 20)
                            {
                                var ammeter = DbHelper.GetAmmeter(item.AmmeterNumber);
                                if (ammeter != null)
                                {
                                    ammeter.CurrPower = decimal.Parse(example.data[0].value[0].ToString()).ToString("0.00");
                                    ammeter.CP_Time = DateTime.Parse(example.resolve_time);
                                    DbHelper.UpdateAmmeter(ammeter);
                                }
                            }
                        }
                    }
                }

            }
            ReadingHandleTimer.Start();
        }
        /// <summary>
        /// 抄表
        /// </summary>
        /// <param name="ammeter"></param>
        /// <param name="type"></param>
        private void Reading(Am_Ammeter ammeter, int type)
        {
            var result = ReadAmmeter(ammeter.Collector_Code, ammeter.AM_Code, type.ToString());
            if (result.suc)
            {
                var task = new Am_BackstageTask
                {
                    Number = CommonHelper.GetGuid,
                    AmmeterCode = ammeter.AM_Code,
                    AmmeterNumber = ammeter.Number,
                    CollectorCode = ammeter.Collector_Code,
                    CollectorNumber = ammeter.Collector_Number,
                    CreateTime = DateTime.Now,
                    OperateType = 0,
                    OperateTypeStr = "",
                    OrderNumber = result.opr_id,
                    OverTime = DateTime.Now,
                    Remark = "",
                    Status = 0,
                    StatusStr = "队列中",
                    TaskMark = "",
                    UserName = "System",
                    U_Name = "System",
                    U_Number = "System"
                };
                if (type == 20)
                {
                    task.OperateType = 5;
                    task.OperateTypeStr = "剩余电量";
                    database.Insert<Am_BackstageTask>(task);

                }
                else if (type == 22)
                {
                    task.OperateType = 6;
                    task.OperateTypeStr = "剩余金额";
                    database.Insert<Am_BackstageTask>(task);
                }
            }
        }




        /// <summary>
        /// 获取操作结果
        /// </summary>
        /// <param name="opr_id"></param>
        /// <returns></returns>
        public static string GetResult(string opr_id)
        {
            AmmeterSDK.MainApi api = new AmmeterSDK.MainApi();
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            Dictionary<string, object> paramssMap = new Dictionary<string, object>();
            paramssMap.Add("opr_id", opr_id);
            list.Add(paramssMap);
            var result = api.Request(AmmeterSDK.ApiUrl.OPRATIONSTATUS, list, false);
            return result;
        }

        protected override void OnStart(string[] args)
        {
            this.WriteLog("至善堂数据处理：【服务启动】");
        }

        protected override void OnStop()
        {
            this.WriteLog("至善堂数据处理：【服务停止】");
        }
        protected override void OnShutdown()
        {
            this.WriteLog("至善堂数据处理：【计算机关闭】");
        }
        #region 记录日志
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="msg"></param>
        private void WriteLog(string msg)
        {

            //string path = @"C:\log.txt";

            //该日志文件会存在windows服务程序目录下
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\log.txt";
            FileInfo file = new FileInfo(path);
            if (!file.Exists)
            {
                FileStream fs;
                fs = File.Create(path);
                fs.Close();
            }

            using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine(DateTime.Now.ToString() + "   " + msg);
                }
            }
        }
        #endregion


        /// <summary>
        /// 抄表
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="address"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Result ReadAmmeter(string cid, string address, string type)
        {
            Result r = new Result();
            string opr_id = LeaRun.Utilities.CommonHelper.GetGuid;
            AmmeterSDK.MainApi api = new AmmeterSDK.MainApi();
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            Dictionary<string, object> paramssMap = new Dictionary<string, object>();
            paramssMap.Add("cid", cid);
            paramssMap.Add("address", address);
            paramssMap.Add("type", type);
            paramssMap.Add("time_out", "0");
            paramssMap.Add("opr_id", opr_id);
            paramssMap.Add("must_online", true);
            list.Add(paramssMap);
            var result = api.Request(AmmeterSDK.ApiUrl.GETAMMETERMONEY, list, true);

            var data = LeaRun.Utilities.JsonHelper.JsonToDataTable(result);
            if (data.Rows.Count > 0)
            {
                var status = data.Rows[0]["status"].ToString();
                if (status == "SUCCESS")
                {
                    r.suc = true;
                    r.result = "操作成功";
                    r.opr_id = opr_id;
                }
                else
                {
                    r.suc = false;
                    r.result = data.Rows[0]["error_msg"].ToString();
                }
            }
            return r;
        }

        public class Root
        {
            public List<data> data { get; set; }
            public string resolve_time { get; set; }
            public string status { get; set; }
            public string opr_id { get; set; }
            public string error_msg { get; set; }
            public paramsContent paramsContent { get; set; }
        }
        public class data
        {
            public int type { get; set; }

            public List<string> value { get; set; }

            public string dsp { get; set; }
        }
        public class Result
        {
            public bool suc { get; set; }
            public string result { get; set; }
            public string opr_id { get; set; }
        }
        public class paramsContent
    {
            public int account_id { get; set; }

            public int count { get; set; }

            public int money { get; set; }
        }
    }

}