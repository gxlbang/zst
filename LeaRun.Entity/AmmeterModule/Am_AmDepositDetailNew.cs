/*
* ����:gxlbang
* ����:Am_AmDepositDetail
* CLR�汾��
* ����ʱ��:2018-04-17 19:10:14
* ��������:
*
* �޸���ʷ��
*
* ����������������������������������������������������������������������������������������������������������������������������������������������������
* ��            Copyright(c) gxlbang ALL rights reserved                    ��
* ����������������������������������������������������������������������������������������������������������������������������������������������������
*/
using LeaRun.DataAccess.Attributes;
using LeaRun.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LeaRun.Entity
{
    /// <summary>
    /// Am_AmDepositDetail
    /// <author>
    ///		<name>she</name>
    ///		<date>2018.04.17 19:10</date>
    /// </author>
    /// </summary>
    [Description("Am_AmDepositDetailNew")]
    public class Am_AmDepositDetailNew : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// UserName
        /// </summary>
        /// <returns></returns>
        [DisplayName("UserName")]
        public string UserName { get; set; }
        /// <summary>
        /// U_Name
        /// </summary>
        /// <returns></returns>
        [DisplayName("U_Name")]
        public string U_Name { get; set; }
        /// <summary>
        /// Money
        /// </summary>
        /// <returns></returns>
        [DisplayName("Money")]
        public string Money { get; set; }
        /// <summary>
        /// CurrMoney
        /// </summary>
        /// <returns></returns>
        [DisplayName("CurrMoney")]
        public string CurrMoney { get; set; }
        /// <summary>
        /// Mark
        /// </summary>
        /// <returns></returns>
        [DisplayName("Mark")]
        public string Mark { get; set; }
        /// <summary>
        /// CreateTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("CreateTime")]
        public string CreateTime { get; set; }
        #endregion
    }
}