using System;
using ServerModule;
using GlobalObject;
using System.Collections.Generic;
using System.Data;

namespace Service_Economic_Financial
{
    public interface IBasicParametersSetting
    {
        bool IsExistAccountBill(string yearMonth);

        List<Sys_RunLog> GetCount(string selectType, string yearMonth);

        DataTable GetRunLog();

        void Operation_BudgetProject(CE_OperatorMode mode, Business_Base_Finance_Budget_ProjectItem info);

        System.Data.DataTable GetTableInfo(string type, string parentCode);

        Business_Base_FinanceSubjects GetFinanceSubjectsSingle(string code);

        BASE_MaterialRequisitionPurpose GetMaterialRequisitionPurposeSingle(string code);

        void Operation_FinanceSubjects(CE_OperatorMode mode, Business_Base_FinanceSubjects info);

        void Operation_MaterialRequisitionPurpose(CE_OperatorMode mode, BASE_MaterialRequisitionPurpose info, string parentCode);

        void Operation_StorageInfo(CE_OperatorMode mode, BASE_Storage info,
            Business_Base_FinanceRelationInfo_Subjects_Storage info_Relation);

        void Operation_SubjectsPurpose(CE_OperatorMode mode, Business_Base_FinanceRelationInfo_Subjects_Purpose info);

        Business_Base_Finance_Budget_ProjectItem GetBudgetProjectInfo(string parentName, string itemName);

        void OperationMonthlyBalance(string operationType, string yearMonth, DateTime startTime, DateTime endTime);

        List<DateTime?> GetListTime(string selectType, string yearMonth);
    }
}
