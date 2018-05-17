using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceProcess;

namespace ZstDataHandle
{
    public partial class Service1 : ServiceBase
    {
        System.Timers.Timer timer = new System.Timers.Timer();
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
                if (root!=null )
                {
                    if (root.Count==1)
                    {
                        var example = root[0];
                        item.TaskMark = example.status;
                        if (example.status== "SUCCESS")
                        {
                            item.Status = 1;
                            item.StatusStr = "成功";
                            item.Remark = example.data[0].dsp;
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
                        else if (example.status=="PROCESSING")
                        {
                            item.StatusStr = "正在处理中";
                        }
                        DbHelper.UpdateTask(item);
                    }
                }
                if (item.Status>0&& item.Status<4)
                {
                    var log = DbHelper.GetLog(item.Number);
                    if (log !=null )
                    {
                        log.Result = item.StatusStr;
                        DbHelper.UpdateLog(log);
                    }
                    if (root.Count == 1)
                    {
                        var example = root[0];
                        if (example.data !=null&&example.data.Count>0)
                        {
                            int type = example.data[0].type;
                            //查询余额
                            if (type==22)
                            {
                                var ammeter = DbHelper.GetAmmeter(item.AmmeterNumber);
                                if (ammeter!=null )
                                {
                                    ammeter.CurrMoney = decimal.Parse(example.data[0].value[0].ToString());
                                    ammeter.CM_Time = DateTime.Parse(example.resolve_time);
                                    DbHelper.UpdateAmmeter(ammeter);
                                }
                            }
                            //查询电量
                            if (type==20)
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
