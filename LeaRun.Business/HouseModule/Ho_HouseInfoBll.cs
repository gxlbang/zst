/*
* ����:gxlbang
* ����:Ho_HouseInfo
* CLR�汾��
* ����ʱ��:2017-11-24 10:54:56
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
    /// Ho_HouseInfo
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.11.24 10:54</date>
    /// </author>
    /// </summary>
    public class Ho_HouseInfoBll : RepositoryFactory<Ho_HouseInfo>
    {
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns></returns>
        public IList<Ho_HouseInfo> GetPageList(ref JqGridParam jqgridparam, string keyword)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  *
                            FROM  Ho_HouseInfo where 1 = 1");

            //�ؼ���
            if (!string.IsNullOrEmpty(keyword))
            {
                strSql.Append(" AND Name like @Name");
                parameter.Add(DbFactory.CreateDbParameter("@Name", '%' + keyword + '%'));
            }

            return Repository().FindListPageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }
    }
}