/*
* ����:gxlbang
* ����:Ho_CityInfo
* CLR�汾��
* ����ʱ��:2017-12-01 11:54:25
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
    /// Ho_CityInfo
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.12.01 11:54</date>
    /// </author>
    /// </summary>
    public class Ho_CityInfoBll : RepositoryFactory<Ho_CityInfo>
    {
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns></returns>
        public IList<Ho_CityInfo> GetPageList(ref JqGridParam jqgridparam, string keyword)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  *
                            FROM  Ho_CityInfo where 1 = 1");

            //�ؼ���
            if (!string.IsNullOrEmpty(keyword))
            {
                strSql.Append(" AND City like @Name");
                parameter.Add(DbFactory.CreateDbParameter("@Name", '%' + keyword + '%'));
            }

            return Repository().FindListPageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }
    }
}