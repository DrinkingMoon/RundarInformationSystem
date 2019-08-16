using System;
using ServerModule;
using System.Collections.Generic;
using System.Data;

namespace Service_Quality_QC
{
    public interface IJudgeReport : FlowControlService.IFlowBusinessService
    {
        System.Collections.Generic.List<ServerModule.View_Business_InspectionJudge_JudgeReport_Item> GetListViewItemInfo(string billNo);

        List<View_Business_InspectionJudge_JudgeReportDetail> GetListViewDetailInfo(string billNo);

        ServerModule.Business_InspectionJudge_JudgeReport GetSingleBillInfo(string billNo);

        void SaveInfo(ServerModule.Business_InspectionJudge_JudgeReport billInfo, 
            System.Collections.Generic.List<ServerModule.View_Business_InspectionJudge_JudgeReport_Item> itemInfo, 
            List<View_Business_InspectionJudge_JudgeReportDetail> detailInfo);

        DataTable GetReferenceInfo(bool isRepeat);

        List<View_Business_InspectionJudge_JudgeReportDetail> GetJudgeReportDetail<T>(string billNo, string judgeReportBillNo, List<T> listDetail);
    }
}
