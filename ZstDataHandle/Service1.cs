using LeaRun.DataAccess;
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
        IDatabase database = DataFactory.Database();

        public Service1()
        {
            InitializeComponent();
            //1.账单生成
            //2.账单推送
            //3.抄表
            //4.电表异步处理

            timer.Elapsed += new System.Timers.ElapsedEventHandler(TimedEvent);
            timer.Interval = 5000;//每5秒执行一次
            timer.Enabled = true;

            BIllTimer.Elapsed += new System.Timers.ElapsedEventHandler(BIllTimerdEvent);
            BIllTimer.Interval = 5000;//每5秒执行一次
            BIllTimer.Enabled = true;
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
                var result = GetResult(item.OrderNumber);
                var root = LeaRun.Utilities.JsonHelper.JonsToList<Root>(result);
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
                            F_U_Name = item.U_Name,
                            F_U_Number = item.U_Number,
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
                            T_UserName = item.UY_UserName,
                            T_U_Name = item.UY_Name,
                            T_U_Number = item.UY_Number,
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

        public class Root
        {
            public List<data> data { get; set; }
            public string resolve_time { get; set; }
            public string status { get; set; }
            public string opr_id { get; set; }
            public string error_msg { get; set; }
        }
        public class data
        {
            public int type { get; set; }

            public List<string> value { get; set; }

            public string dsp { get; set; }
        }
    }

}
