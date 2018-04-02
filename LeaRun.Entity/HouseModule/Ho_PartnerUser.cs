/*
* 姓名:gxlbang
* 类名:Ho_PartnerUser
* CLR版本：
* 创建时间:2017-12-05 11:50:47
* 功能描述:
*
* 修改历史：
*
* ┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓
* ┃            Copyright(c) gxlbang ALL rights reserved                    ┃
* ┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛
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
    /// 合伙人
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.12.05 11:50</date>
    /// </author>
    /// </summary>
    [Description("合伙人")]
    [PrimaryKey("Number")]
    public class Ho_PartnerUser : BaseEntity
    {
        #region 获取/设置 字段值
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [DisplayName("主键")]
        public string Number { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        /// <returns></returns>
        [DisplayName("姓名")]
        public string Name { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        /// <returns></returns>
        [DisplayName("身份证号")]
        public string CardCode { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        /// <returns></returns>
        [DisplayName("手机号")]
        public string Mobile { get; set; }
        /// <summary>
        /// 微信id
        /// </summary>
        /// <returns></returns>
        [DisplayName("微信id")]
        public string OpenId { get; set; }
        /// <summary>
        /// 微信帐号
        /// </summary>
        /// <returns></returns>
        [DisplayName("微信帐号")]
        public string WeiXin { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        /// <returns></returns>
        [DisplayName("头像")]
        public string HeadImg { get; set; }
        /// <summary>
        /// 身份证正面
        /// </summary>
        /// <returns></returns>
        [DisplayName("身份证正面")]
        public string CodeImg1 { get; set; }
        /// <summary>
        /// 身份证反面
        /// </summary>
        /// <returns></returns>
        [DisplayName("身份证反面")]
        public string CodeImg2 { get; set; }
        /// <summary>
        /// 手持身份证
        /// </summary>
        /// <returns></returns>
        [DisplayName("手持身份证")]
        public string PCardImg { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("创建时间")]
        public DateTime? CreatTime { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("修改时间")]
        public DateTime? ModifyTime { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        /// <returns></returns>
        [DisplayName("审核时间")]
        public DateTime? SureTime { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        /// <returns></returns>
        [DisplayName("审核人")]
        public string SureUser { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        /// <returns></returns>
        [DisplayName("状态")]
        public int? Status { get; set; }
        /// <summary>
        /// 状态字符
        /// </summary>
        /// <returns></returns>
        [DisplayName("状态字符")]
        public string StatusStr { get; set; }
        /// <summary>
        /// 帐号
        /// </summary>
        /// <returns></returns>
        [DisplayName("帐号")]
        public string Accout { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        /// <returns></returns>
        [DisplayName("密码")]
        public string Password { get; set; }
        /// <summary>
        /// 上级编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("上级编号")]
        public string ParentNumber { get; set; }
        /// <summary>
        /// 上级名称
        /// </summary>
        /// <returns></returns>
        [DisplayName("上级名称")]
        public string ParentName { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        /// <returns></returns>
        [DisplayName("性别")]
        public string Sex { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        /// <returns></returns>
        [DisplayName("电话")]
        public string Phone { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        /// <returns></returns>
        [DisplayName("生日")]
        public DateTime? Birthday { get; set; }
        /// <summary>
        /// 电邮
        /// </summary>
        /// <returns></returns>
        [DisplayName("电邮")]
        public string Email { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("编号")]
        public string InnerCode { get; set; }
        /// <summary>
        /// 用户层级
        /// </summary>
        /// <returns></returns>
        [DisplayName("用户层级")]
        public string Sign { get; set; }
        /// <summary>
        /// 助理编号
        /// </summary>
        /// <returns></returns>
        [DisplayName("助理编号")]
        public string As_Number { get; set; }
        /// <summary>
        /// 助理名字
        /// </summary>
        /// <returns></returns>
        [DisplayName("助理名字")]
        public string As_Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [DisplayName("备注")]
        public string Remark { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.CreatTime = DateTime.Now;
            this.InnerCode = "0";
            this.ModifyTime = DateTime.Now;
            this.Status = 0;
            this.StatusStr = "游客";
            this.SureTime = DateTime.Now;
            this.Number = CommonHelper.GetGuid;
                                            }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.ModifyTime = DateTime.Now;
            this.Number = KeyValue;
                                            }
        #endregion
    }
}