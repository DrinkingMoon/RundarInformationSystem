using System;
using FlowControlService;
using ServerModule;
using System.Collections.Generic;
using System.Data;

namespace Service_Economic_Financial
{
    public interface IBudgetYear : IFlowBusinessService
    {
        DataTable GetSynthesizeBudgetInfo(int yearInt, string deptCode);

        void SaveInfo(DataTable detailTable, Business_Finance_Budget_Year billInfo);

        void OperationBusiness(string billNo);

        Business_Finance_Budget_Year GetBillSingleInfo(string billNo);

        DataTable GetDetailInfo(string billNo);
    }
}
