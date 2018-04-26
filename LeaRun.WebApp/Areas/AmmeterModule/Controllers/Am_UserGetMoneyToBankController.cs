/*
* ����:gxlbang
* ����:Am_UserGetMoneyToBank
* CLR�汾��
* ����ʱ��:2018-04-17 18:54:04
* ��������:
*
* �޸���ʷ��
*
* ����������������������������������������������������������������������������������������������������������������������������������������������������
* ��            Copyright(c) gxlbang ALL rights reserved                    ��
* ����������������������������������������������������������������������������������������������������������������������������������������������������
*/
using LeaRun.Business;
using LeaRun.DataAccess;
using LeaRun.Entity;
using LeaRun.Repository;
using LeaRun.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LeaRun.WebApp.Areas.AmmeterModule.Controllers
{
    /// <summary>
    /// Am_UserGetMoneyToBank������
    /// </summary>
    public class Am_UserGetMoneyToBankController : PublicController<Am_UserGetMoneyToBank>
    {
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public ActionResult GridPageListJson(JqGridParam jqgridparam, string keywords, int Stuts, string StartTime, string EndTime)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                Am_UserGetMoneyToBankBll bll = new Am_UserGetMoneyToBankBll();
                var ListData = bll.GetPageList(ref jqgridparam, keywords, Stuts, StartTime, EndTime);
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = ListData
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "�쳣����" + ex.Message);
                return null;
            }
        }
        /// <summary>
        /// ���ݵ���
        /// </summary>
        public void ExportExcel(string keywords, int Stuts, string StartTime, string EndTime)
        {
            Am_UserGetMoneyToBankBll bll = new Am_UserGetMoneyToBankBll();
            var ListData = bll.GetPageList(keywords, Stuts, StartTime, EndTime);
            var newlist = new List<Am_UserGetMoneyToBankNew>();
            foreach (var item in ListData)
            {
                var model = new Am_UserGetMoneyToBankNew();
                model.BankAddress = item.BankAddress;
                model.BankCharge = item.BankCharge.Value.ToString("0.00");
                model.BankCode = item.BankCode;
                model.BankName = item.BankName;
                model.CreateTime = item.CreateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                model.Money = item.Money.Value.ToString("0.00");
                model.PayTime = item.PayTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                model.RealMoney = item.RealMoney.Value.ToString("0.00");
                model.Remark = item.Remark;
                model.StatusStr = item.StatusStr;
                model.UserName = item.UserName;
                model.U_Name = item.U_Name;

                newlist.Add(model);
            }
            string[] columns = new string[] { "�û���:UserName", "����:U_Name", "���ֽ��:Money",
                "������:BankCharge","ʵ�ʵ���:RealMoney","��������:BankName",
                "���п���:BankCode", "������:BankAddress","״̬:StatusStr","����ʱ��:CreateTime","֧��ʱ��:PayTime", "��ע:Remark" };
            DeriveExcel.ListToExcel<Am_UserGetMoneyToBankNew>(newlist, columns, "�����б�" + DateTime.Now.ToString("yyyyMMddHHmmss"));

        }
        /// <summary>
        /// ��������״̬
        /// </summary>
        /// <returns></returns>
        public ActionResult PayToBank(string KeyValue, int Status, string StatusStr)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                Am_UserGetMoneyToBank entity = repositoryfactory.Repository().FindEntity(KeyValue);
                if (entity == null || string.IsNullOrEmpty(entity.Number))
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "�����쳣" }.ToString());
                }
                if (entity.Status != 0)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "����״̬������" }.ToString());
                }
                entity.Modify(KeyValue);
                entity.Status = Status;
                entity.StatusStr = StatusStr;
                entity.PayTime = DateTime.Now;

                int IsOk = database.Update(entity, isOpenTrans); //��������״̬
                Base_SysLogBll.Instance.WriteLog(KeyValue, OperationType.Update, IsOk > 0 ? "�ɹ�" : "ʧ��", "���ֲ���");
                //�����û���Ϣ���Ѻ����
                var usermodel = database.FindEntity<Ho_PartnerUser>(entity.U_Number);
                if (usermodel == null || string.IsNullOrEmpty(usermodel.Number))
                {
                    database.Rollback();
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "�����쳣" }.ToString());
                }
                if (Status == 9) //�˻�
                {
                    usermodel.Money += entity.Money;
                    usermodel.Modify(usermodel.Number);
                    database.Update(usermodel, isOpenTrans); //�����û���Ϣ��
                                                             //���Ѻ�𷵻���¼
                    var recordModel = new Am_MoneyDetail()
                    {
                        CreateTime = DateTime.Now,
                        CurrMoney = usermodel.Money,
                        CreateUserId = ManageProvider.Provider.Current().UserId,
                        CreateUserName = ManageProvider.Provider.Current().UserName,
                        OperateType = 6,
                        OperateTypeStr = "����ȡ��",
                        Money = entity.Money,
                        UserName = entity.UserName,
                        U_Name = entity.U_Name,
                        U_Number = entity.U_Number
                    };
                    recordModel.Create();
                    database.Insert(recordModel, isOpenTrans); //��ӷ�����¼
                }
                else //���ֳɹ�
                {
                    if (usermodel.FreezeMoney > 0) //����Ҫ��Ѻ��
                    {
                        var money = entity.BankCharge;
                        //��������Ľ�����
                        if (entity.BankCharge > usermodel.FreezeMoney)
                        {
                            money = usermodel.FreezeMoney;
                        }
                        usermodel.FreezeMoney -= money; //�۳�������1:1����
                        usermodel.Money += money;
                        usermodel.Modify(usermodel.Number);
                        database.Update(usermodel, isOpenTrans); //�����û���Ϣ��
                                                                 //���Ѻ�𷵻���¼
                        var recordModel = new Am_AmDepositDetail()
                        {
                            CreateTime = DateTime.Now,
                            CurrMoney = usermodel.FreezeMoney,
                            Mark = "Ѻ��1:1����",
                            Money = money,
                            UserName = entity.UserName,
                            U_Name = entity.U_Name,
                            U_Number = entity.U_Number
                        };
                        recordModel.Create();
                        database.Insert(recordModel, isOpenTrans); //��ӷ�����¼
                    }
                }
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = "�����ɹ�" }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "����ʧ�ܣ�" + ex.Message }.ToString());
            }
        }
    }
}