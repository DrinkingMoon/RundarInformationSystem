using System;
using System.Collections.Generic;
using ServerModule;
using System.Data;

namespace Service_Peripheral_HR
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITrainSurvey : FlowControlService.IFlowBusinessService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="billNo"></param>
        void OperationBusiness(string billNo);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="yearValue"></param>
        /// <returns></returns>
        DataTable GetBillInfo_Year(int yearValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="yearValue"></param>
        /// <returns></returns>
        List<string> GetBillNoList_Temp(int yearValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="planInfo"></param>
        /// <param name="lstPlanCourse"></param>
        /// <param name="lstUser"></param>
        void SaveInfo(HR_Train_Plan planInfo, List<View_HR_Train_PlanCourse> lstPlanCourse, List<View_HR_Train_PlanUser> lstUser);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="billNo"></param>
        /// <returns></returns>
        HR_Train_Plan GetSingleInfo(string billNo);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="billNo"></param>
        /// <returns></returns>
        List<View_HR_Train_PlanCourse> GetPlanCourseInfo(string billNo);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="billNo"></param>
        /// <returns></returns>
        List<View_HR_Train_PlanUser> GetPlanUserInfo(string billNo);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="planCourseID"></param>
        /// <param name="courseID"></param>
        /// <returns></returns>
        List<View_HR_Train_PlanUser> GetPlanUserInfoAll(Guid planCourseID, int courseID);
    }
}
