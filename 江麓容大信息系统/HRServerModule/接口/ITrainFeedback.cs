using System;
using ServerModule;
using System.Collections.Generic;

namespace Service_Peripheral_HR
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITrainFeedback
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        System.Collections.Generic.List<ServerModule.View_HR_Train_Feedback> GetListInfo_Feedback(DateTime startTime, DateTime endTime);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="feedbackID"></param>
        /// <returns></returns>
        System.Collections.Generic.List<ServerModule.View_HR_Train_FeedbackUser> GetListInfo_FeedbackUser(Guid feedbackID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="feedback"></param>
        /// <param name="lstWorkID"></param>
        void InsertInfo(HR_Train_Feedback feedback, List<string> lstWorkID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        void DeleteInfo(string guid);
    }
}
