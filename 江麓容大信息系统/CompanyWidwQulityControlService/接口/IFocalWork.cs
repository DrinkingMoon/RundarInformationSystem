using System;
using ServerModule;
using System.Data;
using System.Collections.Generic;
namespace Service_Peripheral_CompanyQuality
{
    public interface IFocalWork : FlowControlService.IFlowBusinessService
    {
        DataTable GetTable_FocalWork(string yearMonth);

        void OpertionInfo(string billNo);

        Bus_FocalWork_MonthlyProgress_Content GetSingle_Content(string focalWorkId, string yearMonth);

        DateTime GetEndDate(string keyValue);

        List<Bus_FocalWork_MonthlyProgress_Content> GetList_ProgressContent(Bus_FocalWork_MonthlyProgress billInfo);

        List<Bus_FocalWork_MonthlyProgress_KeyPoint> GetList_ProgressKeyPoint(Bus_FocalWork_MonthlyProgress billInfo,
            List<Bus_FocalWork_MonthlyProgress_Content> lstContent);

        List<Bus_FocalWork_MonthlyProgress_KeyPoint> GetList_ProgressKeyPoint(string billNo);
        List<Bus_FocalWork_MonthlyProgress_Content> GetList_ProgressContent(string billNo);

        Bus_FocalWork_MonthlyProgress GetSingleBillInfo(string billNo);
        void SaveInfo(Bus_FocalWork_MonthlyProgress billInfo, List<Bus_FocalWork_MonthlyProgress_Content> lstContent,
            List<Bus_FocalWork_MonthlyProgress_KeyPoint> lstKeyPoint);
        void DeleteKeyPoint(string keyValue);
        void DeleteFocalWork(string keyValue);

        void SaveKeyPoint(Bus_FocalWork_KeyPoint keyPoint);
        void SaveFocalWork(Bus_FocalWork focalWork);
        DataTable GetTable_KeyPoint(string keyValue);
        DataTable GetTable_FocalWork();
        Bus_FocalWork GetSingle_FocalWork(string keyValue);
        Bus_FocalWork_KeyPoint GetSingle_KeyPoint(string keyValue);

    }
}
