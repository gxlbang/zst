/*
* ����:gxlbang
* ����:Am_Bill
* CLR�汾��
* ����ʱ��:2018-04-17 18:56:53
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
    /// Am_Bill
    /// <author>
    ///		<name>she</name>
    ///		<date>2018.04.17 18:56</date>
    /// </author>
    /// </summary>
    public class Am_BillBll : RepositoryFactory<Am_Bill>
    {
        /// <summary>
        /// ��ȡ�б�-����
        /// </summary>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns></returns>
        public IList<Am_Bill> GetPageList(ref JqGridParam jqgridparam, string keywords, int Stuts, string ProvinceId,
            string CityId, string CountyId, string BeginTime, string EndTime)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  *
                            FROM  Am_Bill where 1=1 ");
            //״̬
            if (Stuts >= 0)
            {
                strSql.Append(" AND Status = @Stuts");
                parameter.Add(DbFactory.CreateDbParameter("@Stuts", Stuts));
            }
            //�ؼ���
            if (!string.IsNullOrEmpty(keywords))
            {
                strSql.Append(@" AND (BillCode LIKE @keyword 
                                    OR AmmeterCode LIKE @keyword 
                                    OR F_UserName LIKE @keyword 
                                    OR F_U_Name LIKE @keyword 
                                    OR T_UserName LIKE @keyword 
                                    OR T_U_Name LIKE @keyword 
                                    OR Cell LIKE @keyword 
                                    OR Floor LIKE @keyword 
                                    OR Room LIKE @keyword 
                                    OR Address LIKE @keyword)");
                parameter.Add(DbFactory.CreateDbParameter("@keyword", '%' + keywords + '%'));
            }
            //ʡ
            if (!string.IsNullOrEmpty(ProvinceId))
            {
                strSql.Append(" AND Province = @Province");
                parameter.Add(DbFactory.CreateDbParameter("@Province", ProvinceId));
            }
            //��
            if (!string.IsNullOrEmpty(CityId))
            {
                strSql.Append(" AND City = @City");
                parameter.Add(DbFactory.CreateDbParameter("@City", CityId));
            }
            //����
            if (!string.IsNullOrEmpty(CountyId))
            {
                strSql.Append(" AND County = @County");
                parameter.Add(DbFactory.CreateDbParameter("@County", CountyId));
            }
            //��ʼʱ��
            if (!string.IsNullOrEmpty(BeginTime))
            {
                strSql.Append(" AND CreateTime > @StartTime");
                parameter.Add(DbFactory.CreateDbParameter("@StartTime", Convert.ToDateTime(BeginTime).ToString("yyyy-MM-dd") + " 00:00:00"));
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
        public IList<Am_BillContent> GetPageList(string Number)
        {
            IDatabase database = DataFactory.Database();
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            //strSql.Append(@"SELECT  *
            //                FROM  Am_BillContent where 1=1 ");
            //�˵����
            strSql.Append(" AND Bill_Number = @Bill_Number");
            parameter.Add(DbFactory.CreateDbParameter("@Bill_Number", Number));
            return database.FindList<Am_BillContent>(strSql.ToString(), parameter.ToArray());
        }
        /// <summary>
        /// ��ȡ�б�-����
        /// </summary>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns></returns>
        public IList<Am_Bill> GetPageList(string keywords, int Stuts, string ProvinceId,
            string CityId, string CountyId, string BeginTime, string EndTime)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  *
                            FROM  Am_Bill where 1=1 ");
            //״̬
            if (Stuts >= 0)
            {
                strSql.Append(" AND Status = @Stuts");
                parameter.Add(DbFactory.CreateDbParameter("@Stuts", Stuts));
            }
            //�ؼ���
            if (!string.IsNullOrEmpty(keywords))
            {
                strSql.Append(@" AND (BillCode LIKE @keyword 
                                    OR AmmeterCode LIKE @keyword 
                                    OR F_UserName LIKE @keyword 
                                    OR F_U_Name LIKE @keyword 
                                    OR T_UserName LIKE @keyword 
                                    OR T_U_Name LIKE @keyword 
                                    OR Cell LIKE @keyword 
                                    OR Floor LIKE @keyword 
                                    OR Room LIKE @keyword 
                                    OR Address LIKE @keyword)");
                parameter.Add(DbFactory.CreateDbParameter("@keyword", '%' + keywords + '%'));
            }
            //ʡ
            if (!string.IsNullOrEmpty(ProvinceId))
            {
                strSql.Append(" AND Province = @Province");
                parameter.Add(DbFactory.CreateDbParameter("@Province", ProvinceId));
            }
            //��
            if (!string.IsNullOrEmpty(CityId))
            {
                strSql.Append(" AND City = @City");
                parameter.Add(DbFactory.CreateDbParameter("@City", CityId));
            }
            //����
            if (!string.IsNullOrEmpty(CountyId))
            {
                strSql.Append(" AND County = @County");
                parameter.Add(DbFactory.CreateDbParameter("@County", CountyId));
            }
            //��ʼʱ��
            if (!string.IsNullOrEmpty(BeginTime))
            {
                strSql.Append(" AND CreateTime > @StartTime");
                parameter.Add(DbFactory.CreateDbParameter("@StartTime", Convert.ToDateTime(BeginTime).ToString("yyyy-MM-dd") + " 00:00:00"));
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