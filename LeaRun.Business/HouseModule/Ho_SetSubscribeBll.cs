/*
* ����:gxlbang
* ����:Ho_SetSubscribe
* CLR�汾��
* ����ʱ��:2017-12-19 15:42:01
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
    /// Ho_SetSubscribe
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.12.19 15:42</date>
    /// </author>
    /// </summary>
    public class Ho_SetSubscribeBll : RepositoryFactory<Ho_SetSubscribe>
    {
        // <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns></returns>
        public IList<Ho_SetSubscribe> GetPageList(ref JqGridParam jqgridparam, string KeyValue, string Keyword)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  *
                            FROM  Ho_SetSubscribe where 1 = 1");
            //�������
            if (!string.IsNullOrEmpty(KeyValue))
            {
                strSql.Append(" AND MS_Number = @MS_Number");
                parameter.Add(DbFactory.CreateDbParameter("@MS_Number", KeyValue));
            }
            //�ؼ���
            if (!string.IsNullOrEmpty(Keyword))
            {
                strSql.Append(@" AND (s_CarOrBus LIKE @keyword
                                    OR s_Address LIKE @keyword
                                    OR s_Reception LIKE @keyword
                                    OR s_CarType LIKE @keyword
                                    OR s_CarNumer LIKE @keyword
                                    OR s_CarColor LIKE @keyword)");
                parameter.Add(DbFactory.CreateDbParameter("@keyword", '%' + Keyword + '%'));
            }
            return Repository().FindListPageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }
    }
}