/*
* ����:gxlbang
* ����:Ho_SetSubscribe
* CLR�汾��
* ����ʱ��:2017-12-21 10:14:43
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
    /// Ho_SetSubscribe
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.12.21 10:14</date>
    /// </author>
    /// </summary>
    [Description("Ho_SetSubscribe")]
    [PrimaryKey("Number")]
    public class Ho_SetSubscribe : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("Number")]
        public string Number { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����")]
        public int? s_PeopleNum { get; set; }
        /// <summary>
        /// ԤԼʱ��
        /// </summary>
        /// <returns></returns>
        [DisplayName("ԤԼʱ��")]
        public string s_MYTime { get; set; }
        /// <summary>
        /// MS_Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("MS_Number")]
        public string MS_Number { get; set; }
        /// <summary>
        /// �г̰���
        /// </summary>
        /// <returns></returns>
        [DisplayName("�г̰���")]
        public string s_CarOrBus { get; set; }
        /// <summary>
        /// �Ӵ��ص�
        /// </summary>
        /// <returns></returns>
        [DisplayName("�Ӵ��ص�")]
        public string s_Address { get; set; }
        /// <summary>
        /// �Ӵ���
        /// </summary>
        /// <returns></returns>
        [DisplayName("�Ӵ���")]
        public string s_Reception { get; set; }
        /// <summary>
        /// �Ӵ��˵绰
        /// </summary>
        /// <returns></returns>
        [DisplayName("�Ӵ��˵绰")]
        public string s_ReMobile { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [DisplayName("����")]
        public string s_CarType { get; set; }
        /// <summary>
        /// ���ƺ�
        /// </summary>
        /// <returns></returns>
        [DisplayName("���ƺ�")]
        public string s_CarNumer { get; set; }
        /// <summary>
        /// ����ɫ
        /// </summary>
        /// <returns></returns>
        [DisplayName("����ɫ")]
        public string s_CarColor { get; set; }
        /// <summary>
        /// CreateTime
        /// </summary>
        /// <returns></returns>
        [DisplayName("CreateTime")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// ReUserNumber
        /// </summary>
        /// <returns></returns>
        [DisplayName("ReUserNumber")]
        public string ReUserNumber { get; set; }
        /// <summary>
        /// ReUser
        /// </summary>
        /// <returns></returns>
        [DisplayName("ReUser")]
        public string ReUser { get; set; }
        /// <summary>
        /// s_Status
        /// </summary>
        /// <returns></returns>
        [DisplayName("s_Status")]
        public int? s_Status { get; set; }
        /// <summary>
        /// s_StatuStr
        /// </summary>
        /// <returns></returns>
        [DisplayName("s_StatuStr")]
        public string s_StatuStr { get; set; }
        /// <summary>
        /// ��ע
        /// </summary>
        /// <returns></returns>
        [DisplayName("��ע")]
        public string s_Remark { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.CreateTime = DateTime.Now;
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