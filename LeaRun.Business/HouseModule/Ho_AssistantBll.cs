/*
* ����:gxlbang
* ����:Ho_Assistant
* CLR�汾��
* ����ʱ��:2017-11-23 17:54:34
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
    /// Ho_Assistant
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.11.23 17:54</date>
    /// </author>
    /// </summary>
    public class Ho_AssistantBll : RepositoryFactory<Ho_Assistant>
    {
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns></returns>
        public IList<Ho_Assistant> GetPageList(ref JqGridParam jqgridparam, string Keyword)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  * 
                            FROM  Ho_Assistant where 1=1");
            if (!string.IsNullOrEmpty(Keyword))
            {
                strSql.Append(@" AND (Name LIKE @keyword
                                    OR Weixin LIKE @keyword 
                                    OR Mobile LIKE @keyword)");
                parameter.Add(DbFactory.CreateDbParameter("@keyword", '%' + Keyword + '%'));
            }
            return Repository().FindListPageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }
    }
}