/*
* ����:gxlbang
* ����:Fx_WebNotice
* CLR�汾��
* ����ʱ��:2017-11-28 08:46:20
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
    /// Fx_WebNotice
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.11.28 08:46</date>
    /// </author>
    /// </summary>
    [Description("Fx_WebNotice")]
    [PrimaryKey("Number")]
    public class Fx_WebNotice : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("Number")]
        public string Number { get; set; }
        /// <summary>
        /// Notice_Title
        /// </summary>
        /// <returns></returns>
        [DisplayName("Notice_Title")]
        public string Notice_Title { get; set; }
        /// <summary>
        /// Notice_Content
        /// </summary>
        /// <returns></returns>
        [DisplayName("Notice_Content")]
        public string Notice_Content { get; set; }
        /// <summary>
        /// Notice_Level
        /// </summary>
        /// <returns></returns>
        [DisplayName("Notice_Level")]
        public int? Notice_Level { get; set; }
        /// <summary>
        /// CreateTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("CreateTime")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// OverTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("OverTime")]
        public DateTime? OverTime { get; set; }
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