using System;
using System.Collections.Generic;
using ServerModule;
using GlobalObject;
using System.Data;

namespace Service_Peripheral_HR
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITrainPlanCollect
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="yearValue"></param>
        /// <returns></returns>
        DataTable GetCourseInfo(int yearValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        DataTable GetUserInfo(string guid);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="billNo"></param>
        void GenerateCollectPlan_Temp(string billNo);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstBillNo"></param>
        /// <param name="yearValue"></param>
        void GenerateCollectPlan_Year(List<string> lstBillNo, int yearValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="planType"></param>
        /// <param name="yearValue"></param>
        /// <param name="planBillNo"></param>
        /// <returns></returns>
        List<View_HR_Train_PlanCollect> GetCourseInfo(CE_HR_Train_PlanType planType, int yearValue, string planBillNo);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="planCourseID"></param>
        /// <param name="courseID"></param>
        /// <returns></returns>
        List<View_HR_Train_PlanCollectUser> GetUserInfoAll(Guid planCourseID, int courseID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstGuid"></param>
        /// <returns></returns>
        List<View_HR_Train_PlanCollectUser> GetUserInfo(List<Guid> lstGuid);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstPlan"></param>
        void SaveCollect(List<View_HR_Train_PlanCollect> lstPlan);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lstWork"></param>
        /// <param name="guid"></param>
        void SaveUser(List<string> lstWork, Guid guid);
    }
}
