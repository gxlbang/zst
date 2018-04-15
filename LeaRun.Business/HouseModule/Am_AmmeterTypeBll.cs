/*
* ����:gxlbang
* ����:Am_AmmeterType
* CLR�汾��
* ����ʱ��:2018-04-11 17:19:30
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
    /// Am_AmmeterType
    /// <author>
    ///		<name>she</name>
    ///		<date>2018.04.11 17:19</date>
    /// </author>
    /// </summary>
    public class Am_AmmeterTypeBll : RepositoryFactory<Am_AmmeterType>
    {
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns></returns>
        public IList<Am_AmmeterType> GetPageList(ref JqGridParam jqgridparam, string keyword)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  *
                            FROM  Am_AmmeterType where 1=1 ");
            //�ؼ���
            if (!string.IsNullOrEmpty(keyword))
            {
                strSql.Append(@" AND (Name LIKE @keyword)");
                parameter.Add(DbFactory.CreateDbParameter("@keyword", '%' + keyword + '%'));
            }
            return Repository().FindListPageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }
    }
}