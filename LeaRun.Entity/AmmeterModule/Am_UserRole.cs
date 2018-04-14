/*
* ����:gxlbang
* ����:Am_UserRole
* CLR�汾��
* ����ʱ��:2018-04-14 10:44:20
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
    /// Am_UserRole
    /// <author>
    ///		<name>she</name>
    ///		<date>2018.04.14 10:44</date>
    /// </author>
    /// </summary>
    [Description("Am_UserRole")]
    [PrimaryKey("Number")]
    public class Am_UserRole : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("Number")]
        public string Number { get; set; }
        /// <summary>
        /// RoleName
        /// </summary>
        /// <returns></returns>
        [DisplayName("RoleName")]
        public string RoleName { get; set; }
        /// <summary>
        /// RoleMark
        /// </summary>
        /// <returns></returns>
        [DisplayName("RoleMark")]
        public string RoleMark { get; set; }
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