/*
* ����:gxlbang
* ����:Ho_HouseImage
* CLR�汾��
* ����ʱ��:2017-11-24 10:55:19
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
    /// Ho_HouseImage
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.11.24 10:55</date>
    /// </author>
    /// </summary>
    public class Ho_HouseImageBll : RepositoryFactory<Ho_HouseImage>
    {
        //��ȡ¥����Ϣ
        public DataTable GetTree() {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  *
                            FROM  Ho_HouseInfo WHERE 1=1 ");
            strSql.Append(" ORDER BY CreateTime DESC");
            return Repository().FindTableBySql(strSql.ToString());
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns></returns>
        public IList<Ho_HouseImage> GetPageList(ref JqGridParam jqgridparam, string Hnumber,
            string Gnumber)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  *
                            FROM  Ho_HouseImage where 1 = 1");
  
            //¥�̱��
            if (!string.IsNullOrEmpty(Hnumber))
            {
                strSql.Append(" AND HouseNumber = @HouseNumber");
                parameter.Add(DbFactory.CreateDbParameter("@HouseNumber", Hnumber));
            }
            //ͼ���ֵ���
            if (!string.IsNullOrEmpty(Gnumber))
            {
                strSql.Append(" AND GroupNumber = @GroupNumber");
                parameter.Add(DbFactory.CreateDbParameter("@GroupNumber", Gnumber));
            }
           
            return Repository().FindListPageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }
    }
}