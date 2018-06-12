/*
* 姓名:gxlbang
* 类名:Am_UserDeposit
* CLR版本：
* 创建时间:2018-04-17 19:09:05
* 功能描述:
*
* 修改历史：
*
* ┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓
* ┃            Copyright(c) gxlbang ALL rights reserved                    ┃
* ┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛
*/
using LeaRun.DataAccess;
using LeaRun.Entity;
using LeaRun.Repository;
using LeaRun.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace LeaRun.Business
{
    /// <summary>
    /// Am_UserDeposit
    /// <author>
    ///		<name>she</name>
    ///		<date>2018.04.17 19:09</date>
    /// </author>
    /// </summary>
    public class Am_UserDepositBll : RepositoryFactory<Am_UserDeposit>
    {
        /// <summary>
        /// 获取列表-导出
        /// </summary>
        /// <param name="jqgridparam">分页条件</param>
        /// <returns></returns>
        public IList<Am_UserDeposit> GetPageList(ref JqGridParam jqgridparam, string keywords, int Stuts, string StartTime, string EndTime)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  *
                            FROM  Am_UserDeposit where 1=1 ");
            //状态
            if (Stuts >= 0)
            {
                strSql.Append(" AND Status = @Stuts");
                parameter.Add(DbFactory.CreateDbParameter("@Stuts", Stuts));
            }
            //关键字
            if (!string.IsNullOrEmpty(keywords))
            {
                strSql.Append(@" AND (UserName LIKE @keyword 
                                    OR U_Name LIKE @keyword 
                                     OR Province LIKE @keyword 
                                     OR City LIKE @keyword 
                                     OR County LIKE @keyword 
                                     OR Address LIKE @keyword 
                                     OR Cell LIKE @keyword 
                                     OR Floor LIKE @keyword 
                                     OR Room LIKE @keyword 
                                    OR Ammeter_Code LIKE @keyword)");
                parameter.Add(DbFactory.CreateDbParameter("@keyword", '%' + keywords + '%'));
            }
            //开始时间
            if (!string.IsNullOrEmpty(StartTime))
            {
                strSql.Append(" AND CreateTime > @StartTime");
                parameter.Add(DbFactory.CreateDbParameter("@StartTime", Convert.ToDateTime(StartTime).ToString("yyyy-MM-dd") + " 00:00:00"));
            }
            //结束时间
            if (!string.IsNullOrEmpty(EndTime))
            {
                strSql.Append(" AND CreateTime < @EndTime");
                parameter.Add(DbFactory.CreateDbParameter("@EndTime", Convert.ToDateTime(EndTime).AddDays(1).ToString("yyyy-MM-dd") + " 00:00:00"));
            }
            return Repository().FindListPageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }
        /// <summary>
        /// 获取列表-导出
        /// </summary>
        /// <param name="jqgridparam">分页条件</param>
        /// <returns></returns>
        public IList<Am_UserDeposit> GetPageList(string keywords, int Stuts, string StartTime, string EndTime)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  *
                            FROM  Am_UserDeposit where 1=1 ");
            //状态
            if (Stuts >= 0)
            {
                strSql.Append(" AND Status = @Stuts");
                parameter.Add(DbFactory.CreateDbParameter("@Stuts", Stuts));
            }
            //关键字
            if (!string.IsNullOrEmpty(keywords))
            {
                strSql.Append(@" AND (UserName LIKE @keyword 
                                    OR U_Name LIKE @keyword 
                                     OR Province LIKE @keyword 
                                     OR City LIKE @keyword 
                                     OR County LIKE @keyword 
                                     OR Address LIKE @keyword 
                                     OR Cell LIKE @keyword 
                                     OR Floor LIKE @keyword 
                                     OR Room LIKE @keyword 
                                    OR Ammeter_Code LIKE @keyword)");
                parameter.Add(DbFactory.CreateDbParameter("@keyword", '%' + keywords + '%'));
            }
            //开始时间
            if (!string.IsNullOrEmpty(StartTime))
            {
                strSql.Append(" AND CreateTime > @StartTime");
                parameter.Add(DbFactory.CreateDbParameter("@StartTime", Convert.ToDateTime(StartTime).ToString("yyyy-MM-dd") + " 00:00:00"));
            }
            //结束时间
            if (!string.IsNullOrEmpty(EndTime))
            {
                strSql.Append(" AND CreateTime < @EndTime");
                parameter.Add(DbFactory.CreateDbParameter("@EndTime", Convert.ToDateTime(EndTime).AddDays(1).ToString("yyyy-MM-dd") + " 00:00:00"));
            }
            return Repository().FindListBySql(strSql.ToString(), parameter.ToArray());
        }
    }
}