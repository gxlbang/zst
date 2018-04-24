/*
* ����:gxlbang
* ����:Am_UserGetMoneyToBank
* CLR�汾��
* ����ʱ��:2018-04-17 18:54:04
* ��������:
*
* �޸���ʷ��
*
* ����������������������������������������������������������������������������������������������������������������������������������������������������
* ��            Copyright(c) gxlbang ALL rights reserved                    ��
* ����������������������������������������������������������������������������������������������������������������������������������������������������
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
    /// Am_UserGetMoneyToBank
    /// <author>
    ///		<name>she</name>
    ///		<date>2018.04.17 18:54</date>
    /// </author>
    /// </summary>
    public class Am_UserGetMoneyToBankBll : RepositoryFactory<Am_UserGetMoneyToBank>
    {
        /// <summary>
        /// ��ȡ�б�-����
        /// </summary>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns></returns>
        public IList<Am_UserGetMoneyToBank> GetPageList(ref JqGridParam jqgridparam, string keywords, int Stuts, string StartTime, string EndTime)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  *
                            FROM  Am_UserGetMoneyToBank where 1=1 ");
            //״̬
            if (Stuts >= 0)
            {
                strSql.Append(" AND Status = @Stuts");
                parameter.Add(DbFactory.CreateDbParameter("@Stuts", Stuts));
            }
            //�ؼ���
            if (!string.IsNullOrEmpty(keywords))
            {
                strSql.Append(@" AND (UserName LIKE @keyword 
                                    OR U_Name LIKE @keyword 
                                    OR BankName LIKE @keyword)");
                parameter.Add(DbFactory.CreateDbParameter("@keyword", '%' + keywords + '%'));
            }
            //��ʼʱ��
            if (!string.IsNullOrEmpty(StartTime))
            {
                strSql.Append(" AND CreateTime > @StartTime");
                parameter.Add(DbFactory.CreateDbParameter("@StartTime", Convert.ToDateTime(StartTime).ToString("yyyy-MM-dd") + " 00:00:00"));
            }
            //����ʱ��
            if (!string.IsNullOrEmpty(EndTime))
            {
                strSql.Append(" AND CreateTime < @EndTime");
                parameter.Add(DbFactory.CreateDbParameter("@EndTime", Convert.ToDateTime(EndTime).AddDays(1).ToString("yyyy-MM-dd") + " 00:00:00"));
            }
            return Repository().FindListPageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }
        /// <summary>
        /// ��ȡ�б�-����
        /// </summary>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns></returns>
        public IList<Am_UserGetMoneyToBank> GetPageList(string keywords, int Stuts, string StartTime, string EndTime)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  *
                            FROM  Am_UserGetMoneyToBank where 1=1 ");
            //״̬
            if (Stuts >= 0)
            {
                strSql.Append(" AND Status = @Stuts");
                parameter.Add(DbFactory.CreateDbParameter("@Stuts", Stuts));
            }
            //�ؼ���
            if (!string.IsNullOrEmpty(keywords))
            {
                strSql.Append(@" AND (UserName LIKE @keyword 
                                    OR U_Name LIKE @keyword 
                                    OR BankName LIKE @keyword)");
                parameter.Add(DbFactory.CreateDbParameter("@keyword", '%' + keywords + '%'));
            }
            //��ʼʱ��
            if (!string.IsNullOrEmpty(StartTime))
            {
                strSql.Append(" AND CreateTime > @StartTime");
                parameter.Add(DbFactory.CreateDbParameter("@StartTime", Convert.ToDateTime(StartTime).ToString("yyyy-MM-dd") + " 00:00:00"));
            }
            //����ʱ��
            if (!string.IsNullOrEmpty(EndTime))
            {
                strSql.Append(" AND CreateTime < @EndTime");
                parameter.Add(DbFactory.CreateDbParameter("@EndTime", Convert.ToDateTime(EndTime).AddDays(1).ToString("yyyy-MM-dd") + " 00:00:00"));
            }
            return Repository().FindListBySql(strSql.ToString(), parameter.ToArray());
        }
    }
}