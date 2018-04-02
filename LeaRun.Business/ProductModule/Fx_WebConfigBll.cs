/*
 * ����:gxlbang
 * ����:Class1
 * CLR�汾��4.0.30319.42000
 * ����ʱ��:2017/11/13 14:42:25
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
using System.Data;
using System.Data.Common;
using System.Text;

namespace LeaRun.Business
{
    /// <summary>
    /// Fx_WebConfig
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.10.25 12:56</date>
    /// </author>
    /// </summary>
    public class Fx_WebConfigBll : RepositoryFactory<Fx_WebConfig>
    {
        /// <summary>
        /// ��ȡ�û��б�
        /// </summary>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns></returns>
        public DataTable GetPageList(ref JqGridParam jqgridparam)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  *
                            FROM  Fx_WebConfig");
            return Repository().FindTablePageBySql(strSql.ToString(), null, ref jqgridparam);
        }
        /// <summary>
        /// ��ȡ��վ������Ϣ-���ݿ�
        /// </summary>
        /// <returns></returns>
        public Fx_WebConfig GetConfig()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM Fx_WebConfig");
            return (Repository().FindListBySql(strSql.ToString(), null) == null || Repository().FindListBySql(strSql.ToString(), null).Count < 1) ? null : Repository().FindListBySql(strSql.ToString(), null)[0];
        }
    }
}