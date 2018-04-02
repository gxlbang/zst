/*
* ����:gxlbang
* ����:Ho_HouseImage
* CLR�汾��
* ����ʱ��:2017-11-27 10:31:27
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
    /// Ho_HouseImage
    /// <author>
    ///		<name>she</name>
    ///		<date>2017.11.27 10:31</date>
    /// </author>
    /// </summary>
    [Description("Ho_HouseImage")]
    [PrimaryKey("Number")]
    public class Ho_HouseImage : BaseEntity
    {
        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// Number
        /// </summary>
        /// <returns></returns>
        [DisplayName("Number")]
        public string Number { get; set; }
        /// <summary>
        /// HouseNumber
        /// </summary>
        /// <returns></returns>
        [DisplayName("HouseNumber")]
        public string HouseNumber { get; set; }
        /// <summary>
        /// HouseName
        /// </summary>
        /// <returns></returns>
        [DisplayName("HouseName")]
        public string HouseName { get; set; }
        /// <summary>
        /// GroupNumber
        /// </summary>
        /// <returns></returns>
        [DisplayName("GroupNumber")]
        public string GroupNumber { get; set; }
        /// <summary>
        /// GroupName
        /// </summary>
        /// <returns></returns>
        [DisplayName("GroupName")]
        public string GroupName { get; set; }
        /// <summary>
        /// ImageUrl
        /// </summary>
        /// <returns></returns>
        [DisplayName("ImageUrl")]
        public string ImageUrl { get; set; }
        /// <summary>
        /// ImageTitle
        /// </summary>
        /// <returns></returns>
        [DisplayName("ImageTitle")]
        public string ImageTitle { get; set; }
        /// <summary>
        /// ImageDes
        /// </summary>
        /// <returns></returns>
        [DisplayName("ImageDes")]
        public string ImageDes { get; set; }
        /// <summary>
        /// Orders
        /// </summary>
        /// <returns></returns>
        [DisplayName("Orders")]
        public int? Orders { get; set; }
        /// <summary>
        /// IsDel
        /// </summary>
        /// <returns></returns>
        [DisplayName("IsDel")]
        public int? IsDel { get; set; }
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