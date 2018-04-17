/*
* ����:gxlbang
* ����:Am_Template
* CLR�汾��
* ����ʱ��:2018-04-17 19:08:34
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
    /// Am_Template
    /// <author>
    ///		<name>she</name>
    ///		<date>2018.04.17 19:08</date>
    /// </author>
    /// </summary>
    [Description("Am_Template")]
    [PrimaryKey("Ammeter_Number")]
    public class Am_Template : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("Number")]
        public string Number { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        /// <returns></returns>
        [DisplayName("Name")]
        public string Name { get; set; }
        /// <summary>
        /// Ammeter_Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("Ammeter_Number")]
        public string Ammeter_Number { get; set; }
        /// <summary>
        /// Ammeter_Code
        /// </summary>
        /// <returns></returns>
        [DisplayName("Ammeter_Code")]
        public string Ammeter_Code { get; set; }
        /// <summary>
        /// U_Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("U_Number")]
        public string U_Number { get; set; }
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
        /// OtherFees
        /// </summary>
        /// <returns></returns>
        [DisplayName("OtherFees")]
        public string OtherFees { get; set; }
        /// <summary>
        /// CreateTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("CreateTime")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// UserFromTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("UserFromTime")]
        public DateTime? UserFromTime { get; set; }
        /// <summary>
        /// BillCyc
        /// </summary>
        /// <returns></returns>
        [DisplayName("BillCyc")]
        public string BillCyc { get; set; }
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
            this.Ammeter_Number = CommonHelper.GetGuid;
                                            }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.Ammeter_Number = KeyValue;
                                            }
        #endregion
    }
}