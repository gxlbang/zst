/*
 * ����:gxlbang
 * ����:Class1
 * CLR�汾��4.0.30319.42000
 * ����ʱ��:2017/11/13 14:42:25
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
    /// Fx_UserPVCount
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.11.15 16:42</date>
    /// </author>
    /// </summary>
    [Description("Fx_UserPVCount")]
    [PrimaryKey("Number")]
    public class Fx_UserPVCount : BaseEntity
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
        /// RealName
        /// </summary>
        /// <returns></returns>
        [DisplayName("RealName")]
        public string RealName { get; set; }
        /// <summary>
        /// IP
        /// </summary>
        /// <returns></returns>
        [DisplayName("IP")]
        public string IP { get; set; }
        /// <summary>
        /// Browser
        /// </summary>
        /// <returns></returns>
        [DisplayName("Browser")]
        public string Browser { get; set; }
        /// <summary>
        /// Sign
        /// </summary>
        /// <returns></returns>
        [DisplayName("Sign")]
        public int? Sign { get; set; }
        /// <summary>
        /// CreateTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("CreateTime")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// OpenId
        /// </summary>
        /// <returns></returns>
        [DisplayName("OpenId")]
        public string OpenId { get; set; }
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
            this.CreateTime = DateTime.Now;
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