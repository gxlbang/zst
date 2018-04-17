/*
* ����:gxlbang
* ����:Am_RepairAnswer
* CLR�汾��
* ����ʱ��:2018-04-17 19:07:44
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
    /// Am_RepairAnswer
    /// <author>
    ///		<name>she</name>
    ///		<date>2018.04.17 19:07</date>
    /// </author>
    /// </summary>
    [Description("Am_RepairAnswer")]
    [PrimaryKey("Repair_Number")]
    public class Am_RepairAnswer : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("Number")]
        public string Number { get; set; }
        /// <summary>
        /// Repair_Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("Repair_Number")]
        public string Repair_Number { get; set; }
        /// <summary>
        /// AContent
        /// </summary>
        /// <returns></returns>
        [DisplayName("AContent")]
        public string AContent { get; set; }
        /// <summary>
        /// CreateTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("CreateTime")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// STATUS
        /// </summary>
        /// <returns></returns>
        [DisplayName("STATUS")]
        public int? STATUS { get; set; }
        /// <summary>
        /// StatusStr
        /// </summary>
        /// <returns></returns>
        [DisplayName("StatusStr")]
        public string StatusStr { get; set; }
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
        /// Mobile
        /// </summary>
        /// <returns></returns>
        [DisplayName("Mobile")]
        public string Mobile { get; set; }
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
            this.Repair_Number = CommonHelper.GetGuid;
                                            }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.Repair_Number = KeyValue;
                                            }
        #endregion
    }
}