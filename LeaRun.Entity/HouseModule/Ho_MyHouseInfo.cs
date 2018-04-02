/*
* ����:gxlbang
* ����:Ho_MyHouseInfo
* CLR�汾��
* ����ʱ��:2017-11-27 14:43:52
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
    /// Ho_MyHouseInfo
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.11.27 14:43</date>
    /// </author>
    /// </summary>
    [Description("Ho_MyHouseInfo")]
    [PrimaryKey("Number")]
    public class Ho_MyHouseInfo : BaseEntity
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
        /// UName
        /// </summary>
        /// <returns></returns>
        [DisplayName("UName")]
        public string UName { get; set; }
        /// <summary>
        /// HNumber
        /// </summary>
        /// <returns></returns>
        [DisplayName("HNumber")]
        public string HNumber { get; set; }
        /// <summary>
        /// HName
        /// </summary>
        /// <returns></returns>
        [DisplayName("HName")]
        public string HName { get; set; }
        /// <summary>
        /// Himg
        /// </summary>
        /// <returns></returns>
        [DisplayName("Himg")]
        public string Himg { get; set; }
        /// <summary>
        /// HMoney
        /// </summary>
        /// <returns></returns>
        [DisplayName("HMoney")]
        public double? HMoney { get; set; }
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
            this.CreateTime = DateTime.Now;
            this.Status = 0;
            this.StatusStr = "����ԤԼ";
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