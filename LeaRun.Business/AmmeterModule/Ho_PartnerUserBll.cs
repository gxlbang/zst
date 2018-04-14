/*
* ����:gxlbang
* ����:Ho_PartnerUser
* CLR�汾��
* ����ʱ��:2017-12-05 11:50:47
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
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace LeaRun.Business
{
    /// <summary>
    /// �ϻ���
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.12.05 11:50</date>
    /// </author>
    /// </summary>
    public class Ho_PartnerUserBll : RepositoryFactory<Ho_PartnerUser>
    {
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns></returns>
        public IList<Ho_PartnerUser> GetPageList(ref JqGridParam jqgridparam, string keyword, string Role, int Stuts)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  *
                            FROM  Ho_PartnerUser where 1=1 ");
            //״̬
            if (Stuts >= 0)
            {
                strSql.Append(" AND Status = @Stuts");
                parameter.Add(DbFactory.CreateDbParameter("@Stuts", Stuts));
            }
            //�ؼ���
            if (!string.IsNullOrEmpty(keyword))
            {
                strSql.Append(@" AND (Name LIKE @keyword
                                    OR CardCode LIKE @keyword
                                    OR Mobile LIKE @keyword
                                    OR WeiXin LIKE @keyword
                                    OR InnerCode LIKE @keyword)");
                parameter.Add(DbFactory.CreateDbParameter("@keyword", '%' + keyword + '%'));
            }
            //��ɫ
            if (!string.IsNullOrEmpty(Role))
            {
                strSql.Append(" AND UserRoleNumber = @UserRoleNumber");
                parameter.Add(DbFactory.CreateDbParameter("@UserRoleNumber", Role));
            }
            return Repository().FindListPageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }
    }
}