using System;
using ServerModule;
using System.Data;
using System.Collections.Generic;

namespace Service_Quality_QC
{
    public interface IInspectionReportService : FlowControlService.IFlowBusinessService
    {
        System.Collections.Generic.List<ServerModule.View_Business_InspectionJudge_InspectionReport_Item> GetListViewDetailInfo(string billNo);
        ServerModule.Business_InspectionJudge_InspectionReport GetSingleBillInfo(string billNo);
        void SaveInfo(ServerModule.Business_InspectionJudge_InspectionReport billInfo, 
            System.Collections.Generic.List<ServerModule.View_Business_InspectionJudge_InspectionReport_Item> detailInfo);

        DataTable GetReferenceInfo(bool isRepeat);
    }
}
