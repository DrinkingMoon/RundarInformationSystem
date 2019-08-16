using System;
using System.Collections.Generic;
using ServerModule;

namespace Service_Peripheral_HR
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITrainLearn
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        System.Data.DataTable GetTree_Course();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        List<Guid> GetRandomQuestion(int courseID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="history"></param>
        void RecordExamHistory(ref HR_Train_ExamHistory history);
    }
}
