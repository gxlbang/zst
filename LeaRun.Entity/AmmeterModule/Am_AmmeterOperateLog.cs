/*
* ����:gxlbang
* ����:Am_AmmeterOperateLog
* CLR�汾��
* ����ʱ��:2018-04-15 14:54:26
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
    /// Am_AmmeterOperateLog
    /// <author>
    ///		<name>she</name>
    ///		<date>2018.04.15 14:54</date>
    /// </author>
    /// </summary>
    [Description("Am_AmmeterOperateLog")]
    [PrimaryKey("Number")]
    public class Am_AmmeterOperateLog : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("Number")]
        public string Number { get; set; }
        /// <summary>
        /// Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("AmmeterNumber")]
        public string AmmeterNumber { get; set; }
        /// <summary>
        /// Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("AmmeterCode")]
        public string AmmeterCode { get; set; }
        /// <summary>
        /// Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("CollectorNumber")]
        public string CollectorNumber { get; set; }
        /// <summary>
        /// Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("CollectorCode")]
        public string CollectorCode { get; set; }
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
        /// TaskNumber
        /// </summary>
        /// <returns></returns>
        [DisplayName("TaskNumber")]
        public string TaskNumber { get; set; }
        /// <summary>
        /// OperateType
        /// </summary>
        /// <returns></returns>
        [DisplayName("OperateType")]
        public int? OperateType { get; set; }
        /// <summary>
        /// OperateTypeStr
        /// </summary>
        /// <returns></returns>
        [DisplayName("OperateTypeStr")]
        public string OperateTypeStr { get; set; }
        /// <summary>
        /// Result
        /// </summary>
        /// <returns></returns>
        [DisplayName("Result")]
        public string Result { get; set; }
        /// <summary>
        /// CreateTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("CreateTime")]
        public DateTime? CreateTime { get; set; }
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