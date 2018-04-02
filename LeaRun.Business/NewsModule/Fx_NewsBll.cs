/*
* ����:gxlbang
* ����:Fx_News
* CLR�汾��
* ����ʱ��:2017-11-27 15:32:27
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
    /// Fx_News
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.11.27 15:32</date>
    /// </author>
    /// </summary>
    public class Fx_NewsBll : RepositoryFactory<Fx_News>
    {
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns></returns>
        public IList<Fx_News> GetPageList(ref JqGridParam jqgridparam, string Keyword, string Number)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  Number,NewsName,NewsPic,NewsClassNumber,NewsClassName,IsDel,IsFirst,
                            IsHot,IsRec,NewsOrder,NewsHit,StatusStr,CreateTime 
                            FROM  Fx_News where IsShow=1 and IsDel = 0"); //��ʾ,�Ҳ���ɾ��
            if (!string.IsNullOrEmpty(Number))//��x999ת����0
            {
                strSql.Append(@" AND (NewsClassNumber = @Number)");
                parameter.Add(DbFactory.CreateDbParameter("@Number", Number));
            }
            if (!string.IsNullOrEmpty(Keyword))
            {
                strSql.Append(@" AND (NewsName LIKE @keyword
                                    OR NewsKeyword LIKE @keyword)");
                parameter.Add(DbFactory.CreateDbParameter("@keyword", '%' + Keyword + '%'));
            }
            return Repository().FindListPageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }
    }
}