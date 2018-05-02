/*
* ����:gxlbang
* ����:Am_ContractTemplateImage
* CLR�汾��
* ����ʱ��:2018-05-02 16:52:13
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
    /// Am_ContractTemplateImage
    /// <author>
    ///		<name>she</name>
    ///		<date>2018.05.02 16:52</date>
    /// </author>
    /// </summary>
    [Description("Am_ContractTemplateImage")]
    [PrimaryKey("ACT_Number")]
    public class Am_ContractTemplateImage : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("Number")]
        public string Number { get; set; }
        /// <summary>
        /// ACT_Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("ACT_Number")]
        public string ACT_Number { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        /// <returns></returns>
        [DisplayName("Name")]
        public string Name { get; set; }
        /// <summary>
        /// Num
        /// </summary>
        /// <returns></returns>
        [DisplayName("Num")]
        public int? Num { get; set; }
        /// <summary>
        /// Price
        /// </summary>
        /// <returns></returns>
        [DisplayName("Price")]
        public double? Price { get; set; }
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
            this.ACT_Number = CommonHelper.GetGuid;
                                            }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.ACT_Number = KeyValue;
                                            }
        #endregion
    }
}