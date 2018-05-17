/*
* ����:gxlbang
* ����:Am_AmmeterPermission
* CLR�汾��
* ����ʱ��:2018-04-15 14:54:05
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
    /// Am_AmmeterPermission
    /// <author>
    ///		<name>she</name>
    ///		<date>2018.04.15 14:54</date>
    /// </author>
    /// </summary>
    [Description("Am_AmmeterPermission")]
    [PrimaryKey("Number")]
    public class Am_AmmeterPermission : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("Number")]
        public string Number { get; set; }
        /// <summary>
        /// �����
        /// </summary>
        /// <returns></returns>
        [DisplayName("Ammeter_Number")]
        public string Ammeter_Number { get; set; }
        /// <summary>
        /// Ammeter_Code
        /// </summary>
        /// <returns></returns>
        [DisplayName("Ammeter_Code")]
        public string Ammeter_Code { get; set; }
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
        /// LeaveTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("LeaveTime")]
        public DateTime? LeaveTime { get; set; }
        /// <summary>
        /// UY_Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("UY_Number")]
        public string UY_Number { get; set; }
        /// <summary>
        /// UY_UserName
        /// </summary>
        /// <returns></returns>
        [DisplayName("UY_UserName")]
        public string UY_UserName { get; set; }
        /// <summary>
        /// UY_Name
        /// </summary>
        /// <returns></returns>
        [DisplayName("UY_Name")]
        public string UY_Name { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("BeginTime")]
        public DateTime? BeginTime { get; set; }
        /// <summary>
        /// ��������LastPayBill
        /// </summary>
        /// <returns></returns>
        [DisplayName("EndTime")]
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// �ϴ��˵���������
        /// </summary>
        /// <returns></returns>
        [DisplayName("LastPayBill")]
        public DateTime? LastPayBill { get; set; }
        /// <summary>
        /// �˵�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("BillCyc")]
        public int? BillCyc { get; set; }
        
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
            this.Ammeter_Number = CommonHelper.GetGuid;
                                            }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="KeyValue"></param>
        public override void Modify(string KeyValue)
        {
            this.Ammeter_Number = KeyValue;
                                            }
        #endregion
    }
}