/*
* ����:gxlbang
* ����:Fx_NewsClass
* CLR�汾��
* ����ʱ��:2017-11-27 15:31:23
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
    /// Fx_NewsClass
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.11.27 15:31</date>
    /// </author>
    /// </summary>
    public class Fx_NewsClassBll : RepositoryFactory<Fx_NewsClass>
    {
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns></returns>
        public IList<Fx_NewsClass> GetPageList(ref JqGridParam jqgridparam, string Keyword, string Number)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  Number,Name,ClassPic,ParenNumber,ParenName,ClassOrder,IsHasChild,
                            StatusStr,Status,Remark 
                            FROM  Fx_NewsClass where 1=1");
            if (string.IsNullOrEmpty(Number) || Number == "x999")//��x999ת����0
            {
                Number = "0";
            }
            strSql.Append(@" AND (ParenNumber = @Number)");
            parameter.Add(DbFactory.CreateDbParameter("@Number", Number));
            if (!string.IsNullOrEmpty(Keyword))
            {
                strSql.Append(@" AND (Name LIKE @keyword
                                    OR Keyword LIKE @keyword)");
                parameter.Add(DbFactory.CreateDbParameter("@keyword", '%' + Keyword + '%'));
            }
            return Repository().FindListPageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }
    }
}