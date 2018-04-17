/*
* ����:gxlbang
* ����:Am_Task
* CLR�汾��
* ����ʱ��:2018-04-17 19:08:19
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
    /// Am_Task
    /// <author>
    ///		<name>she</name>
    ///		<date>2018.04.17 19:08</date>
    /// </author>
    /// </summary>
    [Description("Am_Task")]
    [PrimaryKey("AmmeterNumber")]
    public class Am_Task : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("Number")]
        public string Number { get; set; }
        /// <summary>
        /// OrderNumber
        /// </summary>
        /// <returns></returns>
        [DisplayName("OrderNumber")]
        public string OrderNumber { get; set; }
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
        /// CollectorNumber
        /// </summary>
        /// <returns></returns>
        [DisplayName("CollectorNumber")]
        public string CollectorNumber { get; set; }
        /// <summary>
        /// CollectorCode
        /// </summary>
        /// <returns></returns>
        [DisplayName("CollectorCode")]
        public string CollectorCode { get; set; }
        /// <summary>
        /// AmmeterNumber
        /// </summary>
        /// <returns></returns>
        [DisplayName("AmmeterNumber")]
        public string AmmeterNumber { get; set; }
        /// <summary>
        /// AmmeterCode
        /// </summary>
        /// <returns></returns>
        [DisplayName("AmmeterCode")]
        public string AmmeterCode { get; set; }
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
        /// CreateTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("CreateTime")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        /// <returns></returns>
        [DisplayName("Status")]
        public int? Status { get; set; }
        /// <summary>
        /// StatusStr
        /// </summary>
        /// <returns></returns>
        [DisplayName("StatusStr")]
        public string StatusStr { get; set; }
        /// <summary>
        /// TaskMark
        /// </summary>
        /// <returns></returns>
        [DisplayName("TaskMark")]
        public string TaskMark { get; set; }
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
            this.AmmeterNumber = CommonHelper.GetGuid;
                                            }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.AmmeterNumber = KeyValue;
                                            }
        #endregion
    }
}