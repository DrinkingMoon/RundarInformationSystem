using System;
using FlowControlService;
using System.Data;
using ServerModule;

namespace Service_Economic_Financial
{
    public interface IBudgetMonth : IFlowBusinessService
    {
        Business_Finance_Budget_Month GetBillSingleInfo(string billNo);

        DataTable GetDetailInfo(Business_Finance_Budget_Month billInfo);

        void OperationBusiness(DataTable detailTable, string billNo);

        void SaveInfo(DataTable detailTable, Business_Finance_Budget_Month billInfo);
    }
}
