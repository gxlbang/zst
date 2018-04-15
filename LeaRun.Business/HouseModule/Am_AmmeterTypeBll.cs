/*
* 姓名:gxlbang
* 类名:Am_AmmeterType
* CLR版本：
* 创建时间:2018-04-11 17:19:30
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
    /// Am_AmmeterType
    /// <author>
    ///		<name>she</name>
    ///		<date>2018.04.11 17:19</date>
    /// </author>
    /// </summary>
    public class Am_AmmeterTypeBll : RepositoryFactory<Am_AmmeterType>
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="jqgridparam">分页条件</param>
        /// <returns></returns>
        public IList<Am_AmmeterType> GetPageList(ref JqGridParam jqgridparam, string keyword)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT  *
                            FROM  Am_AmmeterType where 1=1 ");
            //关键字
            if (!string.IsNullOrEmpty(keyword))
            {
                strSql.Append(@" AND (Name LIKE @keyword)");
                parameter.Add(DbFactory.CreateDbParameter("@keyword", '%' + keyword + '%'));
            }
            return Repository().FindListPageBySql(strSql.ToString(), parameter.ToArray(), ref jqgridparam);
        }
    }
}