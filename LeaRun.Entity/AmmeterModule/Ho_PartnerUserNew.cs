/*
* ����:gxlbang
* ����:Ho_PartnerUser
* CLR�汾��
* ����ʱ��:2018-04-14 10:32:53
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
    /// Ho_PartnerUser
    /// <author>
    ///		<name>she</name>
    ///		<date>2018.04.14 10:32</date>
    /// </author>
    /// </summary>
    [Description("Ho_PartnerUser")]
    public class Ho_PartnerUserNew : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// Name
        /// </summary>
        /// <returns></returns>
        [DisplayName("Name")]
        public string Name { get; set; }
        /// <summary>
        /// CardCode
        /// </summary>
        /// <returns></returns>
        [DisplayName("CardCode")]
        public string CardCode { get; set; }
        /// <summary>
        /// Mobile
        /// </summary>
        /// <returns></returns>
        [DisplayName("Mobile")]
        public string Mobile { get; set; }
        /// <summary>
        /// UserRole
        /// </summary>
        /// <returns></returns>
        [DisplayName("UserRole")]
        public string UserRole { get; set; }
        /// <summary>
        /// Money
        /// </summary>
        /// <returns></returns>
        [DisplayName("Money")]
        public string Money { get; set; }
        /// <summary>
        /// Address
        /// </summary>
        /// <returns></returns>
        [DisplayName("Address")]
        public string Address { get; set; }
        /// <summary>
        /// CreatTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("CreatTime")]
        public string CreatTime { get; set; }
        /// <summary>
        /// StatusStr
        /// </summary>
        /// <returns></returns>
        [DisplayName("StatusStr")]
        public string StatusStr { get; set; }
        /// <summary>
        /// Remark
        /// </summary>
        /// <returns></returns>
        [DisplayName("Remark")]
        public string Remark { get; set; }
        #endregion
    }
}