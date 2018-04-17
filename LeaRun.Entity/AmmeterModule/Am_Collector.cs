/*
* ����:gxlbang
* ����:Am_Collector
* CLR�汾��
* ����ʱ��:2018-04-15 17:09:58
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
    /// Am_Collector
    /// <author>
    ///		<name>she</name>
    ///		<date>2018.04.15 17:09</date>
    /// </author>
    /// </summary>
    [Description("Am_Collector")]
    [PrimaryKey("Number")]
    public class Am_Collector : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("Number")]
        public string Number { get; set; }
        /// <summary>
        /// UNumber
        /// </summary>
        /// <returns></returns>
        [DisplayName("UNumber")]
        public string UNumber { get; set; }
        /// <summary>
        /// UserName
        /// </summary>
        /// <returns></returns>
        [DisplayName("UserName")]
        public string UserName { get; set; }
        /// <summary>
        /// URealName
        /// </summary>
        /// <returns></returns>
        [DisplayName("URealName")]
        public string URealName { get; set; }
        /// <summary>
        /// CollectorCode
        /// </summary>
        /// <returns></returns>
        [DisplayName("CollectorCode")]
        public string CollectorCode { get; set; }
        /// <summary>
        /// Province
        /// </summary>
        /// <returns></returns>
        [DisplayName("Province")]
        public string Province { get; set; }
        /// <summary>
        /// City
        /// </summary>
        /// <returns></returns>
        [DisplayName("City")]
        public string City { get; set; }
        /// <summary>
        /// County
        /// </summary>
        /// <returns></returns>
        [DisplayName("County")]
        public string County { get; set; }
        /// <summary>
        /// Address
        /// </summary>
        /// <returns></returns>
        [DisplayName("Address")]
        public string Address { get; set; }
        /// <summary>
        /// AmCount
        /// </summary>
        /// <returns></returns>
        [DisplayName("AmCount")]
        public string AmCount { get; set; }
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
        /// CreateTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("CreateTime")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// LastConnectTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("LastConnectTime")]
        public DateTime? LastConnectTime { get; set; }
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