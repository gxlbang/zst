/*
* ����:gxlbang
* ����:Fx_WebAdv
* CLR�汾��
* ����ʱ��:2018-04-10 12:09:44
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
    /// Fx_WebAdv
    /// <author>
    ///		<name>she</name>
    ///		<date>2018.04.10 12:09</date>
    /// </author>
    /// </summary>
    public class Fx_WebAdvBll : RepositoryFactory<Fx_WebAdv>
    {
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns></returns>
        public IList<Fx_WebAdv> GetPageList(ref JqGridParam jqgridparam, string keyword)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  *
                            FROM  Fx_WebAdv where 1=1 ");
            //�ؼ���
            if (!string.IsNullOrEmpty(keyword))
            {
                strSql.Append(@" AND (Title LIKE @keyword
                                    OR AdvDes LIKE @keyword)");
                parameter.Add(DbFactory.CreateDbParameter("@keyword", '%' + keyword + '%'));
            }
            return Repository().FindListPageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }
    }
}