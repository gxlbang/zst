/*
* ����:gxlbang
* ����:Am_Collector
* CLR�汾��
* ����ʱ��:2018-04-15 17:09:58
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
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace LeaRun.Business
{
    /// <summary>
    /// Am_Collector
    /// <author>
    ///		<name>she</name>
    ///		<date>2018.04.15 17:09</date>
    /// </author>
    /// </summary>
    public class Am_CollectorBll : RepositoryFactory<Am_Collector>
    {
        /// <summary>
        /// ��ȡ�б�-����
        /// </summary>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns></returns>
        public IList<Am_Collector> GetPageList(ref JqGridParam jqgridparam, string keywords, int Stuts, string ProvinceId, string CityId, string CountyId)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  *
                            FROM  Am_Collector where 1=1 ");
            //״̬
            if (Stuts >= 0)
            {
                strSql.Append(" AND STATUS = @Stuts");
                parameter.Add(DbFactory.CreateDbParameter("@Stuts", Stuts));
            }
            //�ؼ���
            if (!string.IsNullOrEmpty(keywords))
            {
                strSql.Append(@" AND (URealName LIKE @keyword 
                                    OR CollectorCode LIKE @keyword 
                                    OR Address LIKE @keyword 
                                    OR UserName LIKE @keyword)");
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
            return Repository().FindListPageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }
        /// <summary>
        /// ��ȡ�б�-����
        /// </summary>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns></returns>
        public IList<Am_Collector> GetPageList(string keywords, int Stuts, string ProvinceId, string CityId, string CountyId)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  *
                            FROM  Am_Collector where 1=1 ");
            //״̬
            if (Stuts >= 0)
            {
                strSql.Append(" AND STATUS = @Stuts");
                parameter.Add(DbFactory.CreateDbParameter("@Stuts", Stuts));
            }
            //�ؼ���
            if (!string.IsNullOrEmpty(keywords))
            {
                strSql.Append(@" AND (URealName LIKE @keyword 
                                    OR CollectorCode LIKE @keyword 
                                    OR Address LIKE @keyword 
                                    OR UserName LIKE @keyword)");
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
            return Repository().FindListBySql(strSql.ToString(), parameter.ToArray());
        }
    }
}