/*
* ����:gxlbang
* ����:Am_RentBill
* CLR�汾��
* ����ʱ��:2018-04-19 11:10:17
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
    /// Am_RentBill
    /// <author>
    ///		<name>she</name>
    ///		<date>2018.04.19 11:10</date>
    /// </author>
    /// </summary>
    [Description("Am_RentBill")]
    [PrimaryKey("RentNumber")]
    public class Am_RentBill : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("Number")]
        public string Number { get; set; }
        /// <summary>
        /// RentNumber
        /// </summary>
        /// <returns></returns>
        [DisplayName("RentNumber")]
        public string RentNumber { get; set; }
        /// <summary>
        /// ChargeItem_Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("ChargeItem_Number")]
        public string ChargeItem_Number { get; set; }
        /// <summary>
        /// ChargeItem_Title
        /// </summary>
        /// <returns></returns>
        [DisplayName("ChargeItem_Title")]
        public string ChargeItem_Title { get; set; }
        /// <summary>
        /// ChargeItem_ChargeType
        /// </summary>
        /// <returns></returns>
        [DisplayName("ChargeItem_ChargeType")]
        public string ChargeItem_ChargeType { get; set; }
        /// <summary>
        /// Money
        /// </summary>
        /// <returns></returns>
        [DisplayName("Money")]
        public double? Money { get;  set; }
        /// <summary>
        /// UMark
        /// </summary>
        /// <returns></returns>
        [DisplayName("UMark")]
        public string UMark { get; set; }
        /// <summary>
        /// Remark
        /// </summary>
        /// <returns></returns>
        [DisplayName("Remark")]
        public string Remark { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.RentNumber = CommonHelper.GetGuid;
                                            }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.RentNumber = KeyValue;
                                            }
        #endregion
    }
}