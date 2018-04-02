//=====================================================================================
// All Rights Reserved , Copyright @ Learun 2017
// Software Developers @ Learun 2017
//=====================================================================================

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
    [Description("Fx_WebConfig")]
    [PrimaryKey("Number")]
    public class Fx_WebConfig : BaseEntity
    {
        #region 获取/设置 字段值
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
        [DisplayName("Web_Name")]
        public string Web_Name { get; set; }
        /// <summary>
        /// Web_Title
        /// </summary>
        /// <returns></returns>
        [DisplayName("Web_Title")]
        public string Web_Title { get; set; }
        /// <summary>
        /// Web_Keyword
        /// </summary>
        /// <returns></returns>
        [DisplayName("Web_Keyword")]
        public string Web_Keyword { get; set; }
        /// <summary>
        /// Web_Des
        /// </summary>
        /// <returns></returns>
        [DisplayName("Web_Des")]
        public string Web_Des { get; set; }
        /// <summary>
        /// Web_Url
        /// </summary>
        /// <returns></returns>
        [DisplayName("Web_Url")]
        public string Web_Url { get; set; }
        /// <summary>
        /// Web_ICP
        /// </summary>
        /// <returns></returns>
        [DisplayName("Web_ICP")]
        public string Web_ICP { get; set; }
        /// <summary>
        /// IsEmail
        /// </summary>
        /// <returns></returns>
        [DisplayName("IsEmail")]
        public int? IsEmail { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        /// <returns></returns>
        [DisplayName("Email")]
        public string Email { get; set; }
        /// <summary>
        /// Email_Password
        /// </summary>
        /// <returns></returns>
        [DisplayName("Email_Password")]
        public string Email_Password { get; set; }
        /// <summary>
        /// Email_Host
        /// </summary>
        /// <returns></returns>
        [DisplayName("Email_Host")]
        public string Email_Host { get; set; }
        /// <summary>
        /// Email_Port
        /// </summary>
        /// <returns></returns>
        [DisplayName("Email_Port")]
        public int? Email_Port { get; set; }
        /// <summary>
        /// Email_Formart
        /// </summary>
        /// <returns></returns>
        [DisplayName("Email_Formart")]
        public string Email_Formart { get; set; }
        /// <summary>
        /// UserLevel
        /// </summary>
        /// <returns></returns>
        [DisplayName("UserLevel")]
        public int? UserLevel { get; set; }
        /// <summary>
        /// GetOrderNum
        /// </summary>
        /// <returns></returns>
        [DisplayName("GetOrderNum")]
        public int? GetOrderNum { get; set; }
        /// <summary>
        /// TotleOrderNum
        /// </summary>
        /// <returns></returns>
        [DisplayName("TotleOrderNum")]
        public int? TotleOrderNum { get; set; }
        /// <summary>
        /// AppID
        /// </summary>
        /// <returns></returns>
        [DisplayName("AppID")]
        public string AppID { get; set; }
        /// <summary>
        /// AppSecret
        /// </summary>
        /// <returns></returns>
        [DisplayName("AppSecret")]
        public string AppSecret { get; set; }
        /// <summary>
        /// Remark
        /// </summary>
        /// <returns></returns>
        [DisplayName("Remark")]
        public string Remark { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.Number = CommonHelper.GetGuid;
                                            }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.Number = KeyValue;
                                            }
        #endregion
    }
}