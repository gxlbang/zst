/*
* ����:gxlbang
* ����:Am_Ammeter
* CLR�汾��
* ����ʱ��:2018-04-14 10:54:05
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
    /// Am_Ammeter
    /// <author>
    ///		<name>she</name>
    ///		<date>2018.04.14 10:54</date>
    /// </author>
    /// </summary>
    public class Am_AmmeterBll : RepositoryFactory<Am_Ammeter>
    {
        /// <summary>
        /// ��ȡ�б�-����
        /// </summary>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns></returns>
        public IList<Am_Ammeter> GetPageList(ref JqGridParam jqgridparam,string Number, string keywords, int Stuts, string ProvinceId, string CityId, string CountyId)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  *
                            FROM  Am_Ammeter where 1=1 ");
            //�û��޶�
            if (ManageProvider.Provider.Current().DepartmentId == "��Ӫ��")
            {
                strSql.Append(@" AND UY_Number = @UY_Number");
                parameter.Add(DbFactory.CreateDbParameter("@UY_Number", ManageProvider.Provider.Current().CompanyId));
            }
            //״̬
            if (Stuts >= 0)
            {
                strSql.Append(" AND Status = @Stuts");
                parameter.Add(DbFactory.CreateDbParameter("@Stuts", Stuts));
            }
            //�ɼ������
            if (!string.IsNullOrEmpty(Number))
            {
                strSql.Append(" AND Collector_Number = @Collector_Number");
                parameter.Add(DbFactory.CreateDbParameter("@Collector_Number", Number));
            }
            //�ؼ���
            if (!string.IsNullOrEmpty(keywords))
            {
                strSql.Append(@" AND (AM_Code LIKE @keyword 
                                    OR AmmeterType_Name LIKE @keyword 
                                    OR AmmeterMoney_Name LIKE @keyword 
                                    OR U_Name LIKE @keyword 
                                    OR UY_UserName LIKE @keyword 
                                    OR UY_Name LIKE @keyword 
                                    OR Cell LIKE @keyword 
                                    OR Floor LIKE @keyword 
                                    OR Room LIKE @keyword 
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
        public IList<Am_Ammeter> GetPageList(string keywords,string Number, int Stuts, string ProvinceId, string CityId, string CountyId)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  *
                            FROM  Am_Ammeter where 1=1 ");
            //�û��޶�
            if (ManageProvider.Provider.Current().DepartmentId == "��Ӫ��")
            {
                strSql.Append(@" AND UY_Number = @UY_Number");
                parameter.Add(DbFactory.CreateDbParameter("@UY_Number", ManageProvider.Provider.Current().CompanyId));
            }
            //״̬
            if (Stuts >= 0)
            {
                strSql.Append(" AND Status = @Stuts");
                parameter.Add(DbFactory.CreateDbParameter("@Stuts", Stuts));
            }
            //�ɼ������
            if (!string.IsNullOrEmpty(Number))
            {
                strSql.Append(" AND Collector_Number = @Collector_Number");
                parameter.Add(DbFactory.CreateDbParameter("@Collector_Number", Number));
            }
            //�ؼ���
            if (!string.IsNullOrEmpty(keywords))
            {
                strSql.Append(@" AND (AM_Code LIKE @keyword 
                                    OR AmmeterType_Name LIKE @keyword 
                                    OR AmmeterMoney_Name LIKE @keyword 
                                    OR U_Name LIKE @keyword 
                                    OR UY_UserName LIKE @keyword 
                                    OR UY_Name LIKE @keyword 
                                    OR Cell LIKE @keyword 
                                    OR Floor LIKE @keyword 
                                    OR Room LIKE @keyword 
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