/*
* 姓名:gxlbang
* 类名:Ho_PartnerUser
* CLR版本：
* 创建时间:2017-12-05 11:50:47
* 功能描述:
*
* 修改历史：
*
* ┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓
* ┃            Copyright(c) gxlbang ALL rights reserved                    ┃
* ┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛
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
    /// 合伙人
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.12.05 11:50</date>
    /// </author>
    /// </summary>
    public class Ho_PartnerUserBll : RepositoryFactory<Ho_PartnerUser>
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="jqgridparam">分页条件</param>
        /// <returns></returns>
        public IList<Ho_PartnerUser> GetPageList(ref JqGridParam jqgridparam, string keyword)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  *
                            FROM  Ho_PartnerUser where Status > 0");

            //关键字
            if (!string.IsNullOrEmpty(keyword))
            {
                strSql.Append(@" AND (Name LIKE @keyword
                                    OR CardCode LIKE @keyword
                                    OR Mobile LIKE @keyword
                                    OR WeiXin LIKE @keyword
                                    OR InnerCode LIKE @keyword)");
                parameter.Add(DbFactory.CreateDbParameter("@keyword", '%' + keyword + '%'));
            }

            return Repository().FindListPageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }
    }
}