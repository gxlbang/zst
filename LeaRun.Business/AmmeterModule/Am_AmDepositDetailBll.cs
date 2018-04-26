/*
* ����:gxlbang
* ����:Am_AmDepositDetail
* CLR�汾��
* ����ʱ��:2018-04-17 19:10:15
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
    /// Am_AmDepositDetail
    /// <author>
    ///		<name>she</name>
    ///		<date>2018.04.17 19:10</date>
    /// </author>
    /// </summary>
    public class Am_AmDepositDetailBll : RepositoryFactory<Am_AmDepositDetail>
    {
        /// <summary>
        /// ��ȡ�б�-����
        /// </summary>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns></returns>
        public IList<Am_AmDepositDetail> GetPageList(ref JqGridParam jqgridparam, string keywords)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  *
                            FROM  Am_AmDepositDetail where 1=1 ");
            //�ؼ���
            if (!string.IsNullOrEmpty(keywords))
            {
                strSql.Append(@" AND (UserName LIKE @keyword 
                                    OR U_Name LIKE @keyword 
                                    OR Mark LIKE @keyword)");
                parameter.Add(DbFactory.CreateDbParameter("@keyword", '%' + keywords + '%'));
            }
            return Repository().FindListPageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }
        /// <summary>
        /// ��ȡ�б�-����
        /// </summary>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns></returns>
        public IList<Am_AmDepositDetail> GetPageList(string keywords)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  *
                            FROM  Am_AmDepositDetail where 1=1 ");
            //�ؼ���
            if (!string.IsNullOrEmpty(keywords))
            {
                strSql.Append(@" AND (UserName LIKE @keyword 
                                    OR U_Name LIKE @keyword 
                                    OR Mark LIKE @keyword)");
                parameter.Add(DbFactory.CreateDbParameter("@keyword", '%' + keywords + '%'));
            }
            
            return Repository().FindListBySql(strSql.ToString(), parameter.ToArray());
        }
    }
}