/*
* ����:gxlbang
* ����:Am_ChargeItem
* CLR�汾��
* ����ʱ��:2018-04-17 19:05:33
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
    /// Am_ChargeItem
    /// <author>
    ///		<name>she</name>
    ///		<date>2018.04.17 19:05</date>
    /// </author>
    /// </summary>
    [Description("Am_ChargeItem")]
    [PrimaryKey("Number")]
    public class Am_ChargeItem : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("Number")]
        public string Number { get; set; }
        /// <summary>
        /// Title
        /// </summary>
        /// <returns></returns>
        [DisplayName("Title")]
        public string Title { get; set; }
        /// <summary>
        /// Money
        /// </summary>
        /// <returns></returns>
        [DisplayName("Money")]
        public double? Money { get; set; }
        /// <summary>
        /// ChargeType
        /// </summary>
        /// <returns></returns>
        [DisplayName("ChargeType")]
        public int? ChargeType { get; set; }
        /// <summary>
        /// CreateTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("CreateTime")]
        public DateTime? CreateTime { get; set; }
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
            this.CreateTime = DateTime.Now;
            this.Number = CommonHelper.GetGuid;
                                            }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.Number = KeyValue;
                                            }
        #endregion
    }
}