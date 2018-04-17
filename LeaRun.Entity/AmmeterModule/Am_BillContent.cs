/*
* ����:gxlbang
* ����:Am_BillContent
* CLR�汾��
* ����ʱ��:2018-04-17 19:04:14
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
    /// Am_BillContent
    /// <author>
    ///		<name>she</name>
    ///		<date>2018.04.17 19:04</date>
    /// </author>
    /// </summary>
    [Description("Am_BillContent")]
    [PrimaryKey("Bill_Number")]
    public class Am_BillContent : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("Number")]
        public string Number { get; set; }
        /// <summary>
        /// Bill_Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("Bill_Number")]
        public string Bill_Number { get; set; }
        /// <summary>
        /// Bill_Code
        /// </summary>
        /// <returns></returns>
        [DisplayName("Bill_Code")]
        public string Bill_Code { get; set; }
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
        public int? ChargeItem_ChargeType { get; set; }
        /// <summary>
        /// Money
        /// </summary>
        /// <returns></returns>
        [DisplayName("Money")]
        public double? Money { get; set; }
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
            this.Bill_Number = CommonHelper.GetGuid;
                                            }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.Bill_Number = KeyValue;
                                            }
        #endregion
    }
}