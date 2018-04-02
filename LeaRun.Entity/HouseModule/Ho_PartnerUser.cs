/*
* ����:gxlbang
* ����:Ho_PartnerUser
* CLR�汾��
* ����ʱ��:2017-12-05 11:50:47
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
    /// �ϻ���
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.12.05 11:50</date>
    /// </author>
    /// </summary>
    [Description("�ϻ���")]
    [PrimaryKey("Number")]
    public class Ho_PartnerUser : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����")]
        public string Number { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����")]
        public string Name { get; set; }
        /// <summary>
        /// ���֤��
        /// </summary>
        /// <returns></returns>
        [DisplayName("���֤��")]
        public string CardCode { get; set; }
        /// <summary>
        /// �ֻ���
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ֻ���")]
        public string Mobile { get; set; }
        /// <summary>
        /// ΢��id
        /// </summary>
        /// <returns></returns>
        [DisplayName("΢��id")]
        public string OpenId { get; set; }
        /// <summary>
        /// ΢���ʺ�
        /// </summary>
        /// <returns></returns>
        [DisplayName("΢���ʺ�")]
        public string WeiXin { get; set; }
        /// <summary>
        /// ͷ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("ͷ��")]
        public string HeadImg { get; set; }
        /// <summary>
        /// ���֤����
        /// </summary>
        /// <returns></returns>
        [DisplayName("���֤����")]
        public string CodeImg1 { get; set; }
        /// <summary>
        /// ���֤����
        /// </summary>
        /// <returns></returns>
        [DisplayName("���֤����")]
        public string CodeImg2 { get; set; }
        /// <summary>
        /// �ֳ����֤
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ֳ����֤")]
        public string PCardImg { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("����ʱ��")]
        public DateTime? CreatTime { get; set; }
        /// <summary>
        /// �޸�ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�޸�ʱ��")]
        public DateTime? ModifyTime { get; set; }
        /// <summary>
        /// ���ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("���ʱ��")]
        public DateTime? SureTime { get; set; }
        /// <summary>
        /// �����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����")]
        public string SureUser { get; set; }
        /// <summary>
        /// ״̬
        /// </summary>
        /// <returns></returns>
        [DisplayName("״̬")]
        public int? Status { get; set; }
        /// <summary>
        /// ״̬�ַ�
        /// </summary>
        /// <returns></returns>
        [DisplayName("״̬�ַ�")]
        public string StatusStr { get; set; }
        /// <summary>
        /// �ʺ�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ʺ�")]
        public string Accout { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����")]
        public string Password { get; set; }
        /// <summary>
        /// �ϼ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ϼ����")]
        public string ParentNumber { get; set; }
        /// <summary>
        /// �ϼ�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ϼ�����")]
        public string ParentName { get; set; }
        /// <summary>
        /// �Ա�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�Ա�")]
        public string Sex { get; set; }
        /// <summary>
        /// �绰
        /// </summary>
        /// <returns></returns>
        [DisplayName("�绰")]
        public string Phone { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����")]
        public DateTime? Birthday { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����")]
        public string Email { get; set; }
        /// <summary>
        /// ���
        /// </summary>
        /// <returns></returns>
        [DisplayName("���")]
        public string InnerCode { get; set; }
        /// <summary>
        /// �û��㼶
        /// </summary>
        /// <returns></returns>
        [DisplayName("�û��㼶")]
        public string Sign { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [DisplayName("������")]
        public string As_Number { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [DisplayName("��������")]
        public string As_Name { get; set; }
        /// <summary>
        /// ��ע
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ע")]
        public string Remark { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.CreatTime = DateTime.Now;
            this.InnerCode = "0";
            this.ModifyTime = DateTime.Now;
            this.Status = 0;
            this.StatusStr = "�ο�";
            this.SureTime = DateTime.Now;
            this.Number = CommonHelper.GetGuid;
                                            }
        /// <summary>
        /// �༭����
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