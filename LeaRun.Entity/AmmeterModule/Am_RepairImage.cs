/*
* ����:gxlbang
* ����:Am_RepairImage
* CLR�汾��
* ����ʱ��:2018-04-17 19:07:58
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
    /// Am_RepairImage
    /// <author>
    ///		<name>she</name>
    ///		<date>2018.04.17 19:07</date>
    /// </author>
    /// </summary>
    [Description("Am_RepairImage")]
    [PrimaryKey("Repair_Number")]
    public class Am_RepairImage : BaseEntity
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
        /// ImagePath
        /// </summary>
        /// <returns></returns>
        [DisplayName("ImagePath")]
        public string ImagePath { get; set; }
        /// <summary>
        /// ImageMark
        /// </summary>
        /// <returns></returns>
        [DisplayName("ImageMark")]
        public string ImageMark { get; set; }
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