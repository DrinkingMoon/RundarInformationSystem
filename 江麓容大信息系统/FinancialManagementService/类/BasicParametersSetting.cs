using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using DBOperate;
using ServerModule;
using GlobalObject;

namespace Service_Economic_Financial
{
    class BasicParametersSetting : Service_Economic_Financial.IBasicParametersSetting
    {
        public DataTable GetTableInfo(string type, string parentCode)
        {
            string error = null;

            Hashtable hsTable = new Hashtable();

            hsTable.Add("@Accounting", type.ToString());
            hsTable.Add("@ParentCode", parentCode == null ? (object)DBNull.Value : parentCode);

            DataTable resultTable = 
                GlobalObject.DatabaseServer.QueryInfoPro("Business_Base_Finance_ParametersSetting_GetInfo", hsTable, out error);

            return resultTable;
        }

        public Business_Base_FinanceSubjects GetFinanceSubjectsSingle(string code)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Business_Base_FinanceSubjects
                          where a.SubjectsCode == code
                          select a;

            if (varData.Count() != 1)
            {
                return null;
            }
            else
            {
                return varData.Single();
            }
        }

        public BASE_MaterialRequisitionPurpose GetMaterialRequisitionPurposeSingle(string code)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.BASE_MaterialRequisitionPurpose
                          where a.Code == code
                          select a;

            if (varData.Count() != 1)
            {
                return null;
            }
            else
            {
                return varData.Single();
            }
        }

        public void Operation_BudgetProject(CE_OperatorMode mode, Business_Base_Finance_Budget_ProjectItem info)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Business_Base_Finance_Budget_ProjectItem
                          where a.ProjectID == info.ProjectID
                          select a;

            switch (mode)
            {
                case CE_OperatorMode.添加:

                    ctx.Business_Base_Finance_Budget_ProjectItem.InsertOnSubmit(info);

                    break;
                case CE_OperatorMode.修改:

                    if (varData.Count() != 1)
                    {
                        throw new Exception("【代码】：" + info.ProjectID + "为空或者不唯一, 无法录入");
                    }

                    Business_Base_Finance_Budget_ProjectItem temp = varData.Single();

                    temp.ProjectName = info.ProjectName;
                    temp.PerentProjectID = info.PerentProjectID;

                    break;
                case CE_OperatorMode.删除:

                    if (varData.Count() != 1)
                    {
                        throw new Exception("【代码】：" + info.ProjectID + "为空或者不唯一, 无法录入");
                    }

                    Business_Base_Finance_Budget_ProjectItem temp1 = varData.Single();

                    temp1.IsDisable = true;
                    break;
                default:
                    break;
            }

            ctx.SubmitChanges();
        }

        public void Operation_FinanceSubjects(CE_OperatorMode mode, Business_Base_FinanceSubjects info)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Business_Base_FinanceSubjects
                          where a.SubjectsCode == info.SubjectsCode
                          select a;

            switch (mode)
            {
                case CE_OperatorMode.添加:

                    if (varData.Count() > 0)
                    {
                        throw new Exception("【代码】："+ info.SubjectsCode + "重复, 无法录入");
                    }

                    ctx.Business_Base_FinanceSubjects.InsertOnSubmit(info);
                    break;
                case CE_OperatorMode.修改:

                    if (varData.Count() != 1)
                    {
                        throw new Exception("【代码】：" + info.SubjectsCode + "为空或者不唯一, 无法录入");
                    }

                    Business_Base_FinanceSubjects temp = varData.Single();

                    temp.SubjectsName = info.SubjectsName;
                    temp.ParentCode = info.ParentCode;

                    break;
                case CE_OperatorMode.删除:

                    var varData1 = from a in ctx.Business_Base_FinanceRelationInfo_Subjects_Purpose
                                   where a.SubjectsCode == info.SubjectsCode
                                   select a;

                    var varData2 = from a in ctx.Business_Base_FinanceRelationInfo_Subjects_Storage
                                   where a.SubjectsCode == info.SubjectsCode
                                   select a;

                    if (varData1.Count() > 0 || varData2.Count() > 0)
                    {
                        throw new Exception("存在关联关系，无法删除");
                    }

                    ctx.Business_Base_FinanceSubjects.DeleteAllOnSubmit(varData);
                    break;
                default:
                    break;
            }

            ctx.SubmitChanges();
        }

        public void Operation_MaterialRequisitionPurpose(CE_OperatorMode mode, BASE_MaterialRequisitionPurpose info, string parentCode)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.BASE_MaterialRequisitionPurpose
                          where a.Code == info.Code
                          select a;

            BASE_MaterialRequisitionPurpose temp = new BASE_MaterialRequisitionPurpose();

            switch (mode)
            {
                case CE_OperatorMode.添加:

                    if (varData.Count() > 0)
                    {
                        throw new Exception("【代码】：" + info.Code + "重复, 无法录入");
                    }

                    temp.Inventory = info.Inventory;
                    temp.DestructiveInspection = info.DestructiveInspection;
                    temp.ApplicableDepartment = info.ApplicableDepartment;
                    temp.RemindWord = info.RemindWord;
                    temp.ThreeOutSideFit = info.ThreeOutSideFit;
                    temp.ThreeOutSideRepair = info.ThreeOutSideRepair;
                    info.UpdateDate = ServerTime.Time;
                    info.UpdatePerson = BasicInfo.LoginID;
                    info.IsEnd = true;
                    info.IsDisable = true;

                    varData = from a in ctx.BASE_MaterialRequisitionPurpose
                              where a.Code == parentCode
                              select a;

                    if (varData.Count() == 1)
                    {
                        temp = varData.Single();
                        temp.IsEnd = false;
                    }

                    ctx.BASE_MaterialRequisitionPurpose.InsertOnSubmit(info);
                    break;
                case CE_OperatorMode.修改:

                    var varData2 = from a in ctx.BASE_MaterialRequisitionPurpose
                                   where a.Code == info.Code
                                   select a;

                    if (varData2.Count() > 0)
                    {
                        temp = varData.Single();

                        temp.Purpose = info.Purpose;
                        temp.Inventory = info.Inventory;
                        temp.DestructiveInspection = info.DestructiveInspection;
                        temp.ApplicableDepartment = info.ApplicableDepartment;
                        temp.RemindWord = info.RemindWord;
                        temp.ThreeOutSideFit = info.ThreeOutSideFit;
                        temp.ThreeOutSideRepair = info.ThreeOutSideRepair;
                        temp.IsDisable = true;
                        temp.UpdatePerson = BasicInfo.LoginID;
                        temp.UpdateDate = ServerTime.Time;
                    }
                    break;
                case CE_OperatorMode.删除:

                    var varData1 = from a in ctx.S_MaterialRequisition
                                   where a.PurposeCode == info.Code
                                   select a;

                    if (varData1.Count() > 0)
                    {
                        temp = varData.Single();
                        temp.IsDisable = false;
                    }
                    else
                    {
                        ctx.BASE_MaterialRequisitionPurpose.DeleteAllOnSubmit(varData);
                    }
                    break;
                default:
                    break;
            }

            ctx.SubmitChanges();
        }

        public void Operation_StorageInfo(CE_OperatorMode mode, BASE_Storage info, Business_Base_FinanceRelationInfo_Subjects_Storage info_Relation)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.BASE_Storage
                          where a.StorageID == info.StorageID
                          select a;

            switch (mode)
            {
                case CE_OperatorMode.添加:

                    if (varData.Count() > 0)
                    {
                        throw new Exception("【代码】：" + info.StorageID + "重复, 无法录入");
                    }

                    ctx.BASE_Storage.InsertOnSubmit(info);
                    ctx.Business_Base_FinanceRelationInfo_Subjects_Storage.InsertOnSubmit(info_Relation);

                    break;
                case CE_OperatorMode.修改:

                    if (varData.Count() != 1)
                    {
                        throw new Exception("【代码】：" + info.StorageID + "为空或者不唯一, 无法录入");
                    }

                    BASE_Storage temp = varData.Single();

                    temp.StorageName = info.StorageName;
                    temp.StorageLv = 1;

                    temp.Aftermarket = info.Aftermarket;
                    temp.AftermarketParts = info.AftermarketParts;
                    temp.AssemblyWarehouse = info.AssemblyWarehouse;
                    temp.FinancialAccountingFlag = info.FinancialAccountingFlag;
                    temp.PartInPlanCalculation = info.PartInPlanCalculation;
                    temp.SingleFinancialAccountingFlag = info.SingleFinancialAccountingFlag;
                    temp.WorkShopCurrentAccount = temp.WorkShopCurrentAccount;
                    temp.ZeroCostFlag = temp.ZeroCostFlag;

                    var varDataX = from a in ctx.Business_Base_FinanceRelationInfo_Subjects_Storage
                                   where a.StorageID == info.StorageID
                                   select a;

                    ctx.Business_Base_FinanceRelationInfo_Subjects_Storage.DeleteAllOnSubmit(varDataX);
                    ctx.Business_Base_FinanceRelationInfo_Subjects_Storage.InsertOnSubmit(info_Relation);

                    break;
                case CE_OperatorMode.删除:

                    var varData1 = from a in ctx.S_Stock
                                   where a.StorageID == info.StorageID
                                   select a;

                    var varData2 = from a in ctx.S_InDepotDetailBill
                                   where a.StorageID == info.StorageID
                                   select a;

                    var varData3 = from a in ctx.S_FetchGoodsDetailBill
                                   where a.StorageID == info.StorageID
                                   select a;

                    if (varData1.Count() > 0 || varData2.Count() > 0 || varData3.Count() > 0)
                    {
                        throw new Exception("已产生业务，无法删除");
                    }

                    ctx.BASE_Storage.DeleteAllOnSubmit(varData);

                    varDataX = from a in ctx.Business_Base_FinanceRelationInfo_Subjects_Storage
                               where a.StorageID == info.StorageID
                               select a;

                    ctx.Business_Base_FinanceRelationInfo_Subjects_Storage.DeleteAllOnSubmit(varDataX);

                    break;
                default:
                    break;
            }

            ctx.SubmitChanges();
        }

        public void Operation_SubjectsPurpose(CE_OperatorMode mode, Business_Base_FinanceRelationInfo_Subjects_Purpose info)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Business_Base_FinanceRelationInfo_Subjects_Purpose
                          where a.SubjectsCode == info.SubjectsCode
                          && a.PurposeCode == info.PurposeCode
                          select a;

            switch (mode)
            {
                case CE_OperatorMode.添加:

                    if (varData.Count() > 0)
                    {
                        throw new Exception("【代码】：" + info.SubjectsCode + "重复, 无法录入");
                    }

                    ctx.Business_Base_FinanceRelationInfo_Subjects_Purpose.InsertOnSubmit(info);
                    break;
                case CE_OperatorMode.删除:
                    ctx.Business_Base_FinanceRelationInfo_Subjects_Purpose.DeleteAllOnSubmit(varData);
                    break;
                default:
                    break;
            }

            ctx.SubmitChanges();
        }

        public Business_Base_Finance_Budget_ProjectItem GetBudgetProjectInfo(string parentName, string itemName)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(itemName))
            {
                return default(Business_Base_Finance_Budget_ProjectItem);
            }

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(parentName.Trim()))
            {
                var varData = from a in ctx.Business_Base_Finance_Budget_ProjectItem
                              where a.ProjectName == itemName
                              select a;

                if (varData.Count() == 1)
                {
                    return varData.Single();
                }

                return default(Business_Base_Finance_Budget_ProjectItem);
            }
            else
            {
                var varData = from a in ctx.Business_Base_Finance_Budget_ProjectItem
                              join b in ctx.Business_Base_Finance_Budget_ProjectItem
                              on a.PerentProjectID equals b.ProjectID
                              where a.ProjectName == itemName
                              && b.ProjectName == parentName
                              select a;

                if (varData.Count() == 1)
                {
                    return varData.Single();
                }

                return default(Business_Base_Finance_Budget_ProjectItem);
            }
        }

        public void OperationMonthlyBalance(string operationType, string yearMonth, DateTime startTime, DateTime endTime)
        {
            try
            {
                string error = null;

                switch (operationType)
                {
                    case "财务月结":

                        using (DepotManagementDataContext ctx = CommentParameter.DepotDataContext)
                        {
                            var varData = from a in ctx.Sys_RunLog
                                          where a.YearMonth == yearMonth
                                          && a.RecordType == "供应商挂账表"
                                          select a;

                            if (varData.Count() == 0)
                            {
                                throw new Exception("未生成【供应商挂账单】，无法进行财务月结");
                            }

                            DateTime tempDate = Convert.ToDateTime( yearMonth.Substring(0, 4) + "-" + yearMonth.Substring(4, 2) + "-01").AddMonths(-1);
                            string prevYearMonth = tempDate.Year.ToString() + tempDate.Month.ToString("D2");

                            var varData1 = from a in ctx.Sys_RunLog
                                           where a.YearMonth == prevYearMonth
                                          && a.RecordType == "财务月结"
                                          select a;

                            if (varData1.Count() == 0)
                            {
                                throw new Exception("【"+ prevYearMonth +"】未进行月结，无法进行【"+ yearMonth +"】财务月结");
                            }
                        }

                        Hashtable hsTableFinance = new Hashtable();

                        hsTableFinance.Add("@YearMonth", yearMonth);
                        hsTableFinance.Add("@StartTime", startTime);
                        hsTableFinance.Add("@EndTime", endTime);
                        hsTableFinance.Add("@User", BasicInfo.LoginID);

                        DataTable tempTableFinance = GlobalObject.DatabaseServer.QueryInfoPro("MonthlyBalance", hsTableFinance, out error);
                        break;
                    case "供应商挂账表":

                        using (DepotManagementDataContext ctx = CommentParameter.DepotDataContext)
                        {
                            DateTime tempDate = Convert.ToDateTime(yearMonth.Substring(0, 4) + "-" + yearMonth.Substring(4, 2) + "-01").AddMonths(-1);
                            string prevYearMonth = tempDate.Year.ToString() + tempDate.Month.ToString("D2");

                            var varData1 = from a in ctx.Sys_RunLog
                                           where a.YearMonth == prevYearMonth
                                          && a.RecordType == "供应商挂账表"
                                           select a;

                            if (varData1.Count() == 0)
                            {
                                throw new Exception("【" + prevYearMonth + "】供应商挂账表未生成，无法生成【" + yearMonth + "】供应商挂账表");
                            }
                        }

                        Hashtable hsTableProvider = new Hashtable();

                        hsTableProvider.Add("@YearMonth", yearMonth);
                        hsTableProvider.Add("@StartTime", startTime);
                        hsTableProvider.Add("@EndTime", endTime);
                        hsTableProvider.Add("@User", BasicInfo.LoginID);

                        DataTable tempTableProvider = GlobalObject.DatabaseServer.QueryInfoPro("MonthlyBalance_Account_Provider", hsTableProvider, out error);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<DateTime?> GetListTime(string selectType, string yearMonth)
        {
            List<DateTime?> lstResult = new List<DateTime?>();

            using (DepotManagementDataContext ctx = CommentParameter.DepotDataContext)
            {
                var varNextData = from a in ctx.Sys_RunLog
                                  where a.YearMonth == ctx.Fun_get_NextYearMonth(yearMonth)
                                  && a.RecordType == selectType
                                  select a;

                var varNowData = from a in ctx.Sys_RunLog
                                 where a.YearMonth == yearMonth
                                  && a.RecordType == selectType
                                 select a;

                var varPrevData = from a in ctx.Sys_RunLog
                                  where a.YearMonth == ctx.Fun_get_PrevYearMonth(yearMonth)
                                  && a.RecordType == selectType
                                  select a;

                if (varNowData.Count() > 0)
                {
                    if (varNextData.Count() > 0)
                    {
                        lstResult.Add(varNowData.ToList().OrderByDescending(k => k.RecordTime).First().StartTime);
                        lstResult.Add(varNowData.ToList().OrderByDescending(k => k.RecordTime).First().EndTime);

                        return lstResult;
                    }
                }

                lstResult.Add(varPrevData.ToList().OrderByDescending(k => k.RecordTime).First().EndTime);
                lstResult.Add(null);

            }

            return lstResult;
        }

        public DataTable GetRunLog()
        {
            string strSql = "select RecordType as 操作类型, YearMonth 操作年月, StartTime as 起始日期, EndTime as 截止日期, "+ 
                            " case when b.Name is null then '系统生成' else b.Name end as 操作人, a.RecordTime as 操作日期 "+
                            " from Sys_RunLog as a left join HR_PersonnelArchive as b on a.RecordUser = b.WorkID order by a.RecordTime desc";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        public List<Sys_RunLog> GetCount(string selectType, string yearMonth)
        {
            using (DepotManagementDataContext ctx = CommentParameter.DepotDataContext)
            {
                var varNowData = from a in ctx.Sys_RunLog
                                 where a.YearMonth == yearMonth
                                  && a.RecordType == selectType
                                 select a;

                return varNowData.ToList();
            }
        }

        public bool IsExistAccountBill(string yearMonth)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Bus_PurchasingMG_AccountBill_Detail
                          where a.YearMonth == yearMonth
                          select a;

            if (varData.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
