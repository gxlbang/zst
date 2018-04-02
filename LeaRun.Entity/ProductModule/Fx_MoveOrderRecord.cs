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
    /// Fx_WebConfig
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.11.13 10:53</date>
    /// </author>
    /// </summary>
    [Description("Fx_MoveOrderRecord")]
    [PrimaryKey("Number")]
    public class Fx_MoveOrderRecord : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("Number")]
        public string Number { get; set; }
        /// <summary>
        /// Web_Name
        /// </summary>
        /// <returns></returns>
        [DisplayName("OrderNumber")]
        public string OrderNumber { get; set; }
        /// <summary>
        /// Web_Title
        /// </summary>
        /// <returns></returns>
        [DisplayName("UserNumber")]
        public string UserNumber { get; set; }
        /// <summary>
        /// Web_Keyword
        /// </summary>
        /// <returns></returns>
        [DisplayName("UserAccount")]
        public string UserAccount { get; set; }
        /// <summary>
        /// Web_Des
        /// </summary>
        /// <returns></returns>
        [DisplayName("UserName")]
        public string UserName { get; set; }
        /// <summary>
        /// Web_Url
        /// </summary>
        /// <returns></returns>
        [DisplayName("MyUser")]
        public string MyUser { get; set; }
        /// <summary>
        /// Web_ICP
        /// </summary>
        /// <returns></returns>
        [DisplayName("MyUserName")]
        public string MyUserName { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        /// <returns></returns>
        [DisplayName("MyAccount")]
        public string MyAccount { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        /// <returns></returns>
        [DisplayName("CreateTime")]
        public DateTime CreateTime { get; set; }
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