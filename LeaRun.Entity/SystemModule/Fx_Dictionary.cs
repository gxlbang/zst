/*
* ����:gxlbang
* ����:Fx_Dictionary
* CLR�汾��
* ����ʱ��:2018-04-15 12:02:22
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
    /// Fx_Dictionary
    /// <author>
    ///		<name>she</name>
    ///		<date>2018.04.15 12:02</date>
    /// </author>
    /// </summary>
    [Description("Fx_Dictionary")]
    [PrimaryKey("Number")]
    public class Fx_Dictionary : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("Number")]
        public string Number { get; set; }
        /// <summary>
        /// Dic_Code
        /// </summary>
        /// <returns></returns>
        [DisplayName("Dic_Code")]
        public string Dic_Code { get; set; }
        /// <summary>
        /// Dic_Name
        /// </summary>
        /// <returns></returns>
        [DisplayName("Dic_Name")]
        public string Dic_Name { get; set; }
        /// <summary>
        /// Dic_Class
        /// </summary>
        /// <returns></returns>
        [DisplayName("Dic_Class")]
        public string Dic_Class { get; set; }
        /// <summary>
        /// Dic_Value
        /// </summary>
        /// <returns></returns>
        [DisplayName("Dic_Value")]
        public string Dic_Value { get; set; }
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