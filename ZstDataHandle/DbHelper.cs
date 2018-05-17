using Dapper;
using LeaRun.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZstDataHandle
{
    public class DbHelper
    {
        private static SqlConnection GetOpenConnection()
        {
            var conn = new SqlConnection("server=120.78.12.92;database=AmmeterDB;uid=sa;pwd=qqq111...");
            conn.Open();
            return conn;
        }
        public static List<Am_Task> GetTaskList(int Status)
        {
            using (var conn = GetOpenConnection())
            {
                var sql = "select * from Am_Task  where Status=@Status";
                return conn.Query<Am_Task>(sql, new { Status = Status }).ToList();
            }
        }
        public static int UpdateTask(Am_Task task)
        {
            using (var conn = GetOpenConnection())
            {
                var sql = "update Am_Task set Status=@Status,StatusStr=@StatusStr,OverTime=@OverTime,Remark=@Remark  where Number=@Number ";
                return conn.Execute(sql, task);
            }
        }

        public static Am_AmmeterOperateLog GetLog(string TaskNumber)
        {
            using (var conn = GetOpenConnection())
            {
                var sql = "select * from Am_AmmeterOperateLog  where TaskNumber=@TaskNumber";
                return conn.Query<Am_AmmeterOperateLog>(sql, new { TaskNumber = TaskNumber }).FirstOrDefault();
            }
        }
        public static int UpdateLog(Am_AmmeterOperateLog Log)
        {
            using (var conn = GetOpenConnection())
            {
                var sql = "update Am_AmmeterOperateLog set Result=@Result  where Number=@Number ";
                return conn.Execute(sql, Log);
            }
        }
        public static Am_Ammeter GetAmmeter(string Number)
        {
            using (var conn = GetOpenConnection())
            {
                var sql = "select * from Am_Ammeter  where Number=@Number";
                return conn.Query<Am_Ammeter>(sql, new { Number = Number }).FirstOrDefault();
            }
        }
        public static int UpdateAmmeter(Am_Ammeter ammeter)
        {
            using (var conn = GetOpenConnection())
            {
                var sql = "update Am_Ammeter set CurrMoney=@CurrMoney,CM_Time=@CM_Time,CurrPower=@CurrPower,CP_Time=@CP_Time  where Number=@Number ";
                return conn.Execute(sql, ammeter);
            }
        }
    }
}
