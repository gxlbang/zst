/*
* ����:gxlbang
* ����:Am_MoneyDetail
* CLR�汾��
* ����ʱ��:2018-04-15 14:49:30
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
    /// Am_MoneyDetail
    /// <author>
    ///		<name>she</name>
    ///		<date>2018.04.15 14:49</date>
    /// </author>
    /// </summary>
    [Description("Am_MoneyDetailNew")]
    public class Am_MoneyDetailNew : BaseEntity
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
        /// OperateTypeStr
        /// </summary>
        /// <returns></returns>
        [DisplayName("OperateTypeStr")]
        public string OperateTypeStr { get; set; }
        /// <summary>
        /// Money
        /// </summary>
        /// <returns></returns>
        [DisplayName("Money")]
        public string Money { get; set; }
        /// <summary>
        /// CreateTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("CreateTime")]
        public string CreateTime { get; set; }
        /// <summary>
        /// CurrMoney
        /// </summary>
        /// <returns></returns>
        [DisplayName("CurrMoney")]
        public string CurrMoney { get; set; }
        /// <summary>
        /// Remark
        /// </summary>
        /// <returns></returns>
        [DisplayName("Remark")]
        public string Remark { get; set; }
        #endregion
    }
}