/*
* ����:gxlbang
* ����:Ho_MySubscribe
* CLR�汾��
* ����ʱ��:2017-12-05 11:54:11
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
    /// �ҵ�ԤԼ
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.12.05 11:54</date>
    /// </author>
    /// </summary>
    [Description("�ҵ�ԤԼ")]
    [PrimaryKey("Number")]
    public class Ho_MySubscribe : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����")]
        public string Number { get; set; }
        /// <summary>
        /// �û����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�û����")]
        public string UNumber { get; set; }
        /// <summary>
        /// �û�����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�û�����")]
        public string UName { get; set; }
        /// <summary>
        /// �û��绰
        /// </summary>
        /// <returns></returns>
        [DisplayName("�û��绰")]
        public string UMobile { get; set; }
        /// <summary>
        /// �ϻ��˱��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ϻ��˱��")]
        public string UCode { get; set; }
        /// <summary>
        /// �ϻ������֤��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ϻ������֤��")]
        public string UCardCode { get; set; }
        /// <summary>
        /// ¥�̱��
        /// </summary>
        /// <returns></returns>
        [DisplayName("¥�̱��")]
        public string HNumber { get; set; }
        /// <summary>
        /// ¥������
        /// </summary>
        /// <returns></returns>
        [DisplayName("¥������")]
        public string HName { get; set; }
        /// <summary>
        /// ¥����ͼ
        /// </summary>
        /// <returns></returns>
        [DisplayName("¥����ͼ")]
        public string HImg { get; set; }
        /// <summary>
        /// �ҵ�ҵ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("�ҵ�ҵ����")]
        public string MHNumber { get; set; }
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
        /// ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����")]
        public int? PeopleNum { get; set; }
        /// <summary>
        /// ԤԼʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("ԤԼʱ��")]
        public string MYTime { get; set; }
        /// <summary>
        /// ԤԼ�ύʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("ԤԼ�ύʱ��")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// ԤԼ����ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("ԤԼ����ʱ��")]
        public DateTime? OverTime { get; set; }
        /// <summary>
        /// �г̰���
        /// </summary>
        /// <returns></returns>
        [DisplayName("�г̰���")]
        public string CarOrBus { get; set; }
        /// <summary>
        /// �Ӵ��ص�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�Ӵ��ص�")]
        public string Address { get; set; }
        /// <summary>
        /// �Ӵ���
        /// </summary>
        /// <returns></returns>
        [DisplayName("�Ӵ���")]
        public string Reception { get; set; }
        /// <summary>
        /// �Ӵ��˵绰
        /// </summary>
        /// <returns></returns>
        [DisplayName("�Ӵ��˵绰")]
        public string ReMobile { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����")]
        public string CarType { get; set; }
        /// <summary>
        /// ���ƺ�
        /// </summary>
        /// <returns></returns>
        [DisplayName("���ƺ�")]
        public string CarNumer { get; set; }
        /// <summary>
        /// ����ɫ
        /// </summary>
        /// <returns></returns>
        [DisplayName("����ɫ")]
        public string CarColor { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("����ʱ��")]
        public DateTime ReTime { get; set; }
        /// <summary>
        /// �����˱��
        /// </summary>
        /// <returns></returns>
        [DisplayName("�����˱��")]
        public string ReUserNumber { get; set; }
        /// <summary>
        /// ����������
        /// </summary>
        /// <returns></returns>
        [DisplayName("����������")]
        public string ReUser { get; set; }
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
            this.CreateTime = DateTime.Now;
            this.ReTime = DateTime.Now;
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