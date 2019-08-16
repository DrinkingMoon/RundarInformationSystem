using System;
using ServerModule;
using System.Data;
using System.Collections.Generic;

namespace Service_Peripheral_HR
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITrainBasicInfo
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        HR_Train_Course GetSingleCourseTableInfo(int courseID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        HR_Train_QuestionBank GetBankInfo(Guid guid);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="courseInfo"></param>
        void UpdateCourseExamInfo(HR_Train_Course courseInfo);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        List<View_HR_Train_QuestionBank> GetListQuestionBank(Guid guid);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        void DeleteQuestion(string guid);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="courseID"></param>
        /// <param name="questionsTable"></param>
        void InputQuestionsBank(int courseID, DataTable questionsTable);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        List<View_HR_Train_QuestionBank> GetListQuestionBank(int courseID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        List<string> GetCourse_User(int courseID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commCourseID"></param>
        /// <param name="lstDeptCourse"></param>
        void Operation_Comm_Rel(int commCourseID, List<int> lstDeptCourse);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        DataTable GetCourseInfo_Comm_Dept(int courseID);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        DataTable GetCourseInfo_Comm();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        DataTable GetTable_Ware(int courseID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        DataTable GetTable_QuestionBank(int courseID);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        System.Data.DataTable GetTable<T>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        void Operation_AssessType(ServerModule.HR_Train_AssessType obj);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        void Operation_Course(ServerModule.HR_Train_Course obj);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        void Operation_CourseType(ServerModule.HR_Train_CourseType obj);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        void Operation_Courseware(ServerModule.HR_Train_Courseware obj);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        void Operation_QuestionBank(ServerModule.HR_Train_QuestionBank obj);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        HR_Train_AssessType GetSingleInfo_AssessType(HR_Train_AssessType info);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        HR_Train_Course GetSingleInfo_Course(HR_Train_Course info);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        HR_Train_CourseType GetSingleInfo_CourseType(HR_Train_CourseType info);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CourseID"></param>
        /// <returns></returns>
        DataTable GetPostInfo(int? CourseID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postID"></param>
        /// <returns></returns>
        DataTable GetCourseInfo(int? postID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceTable"></param>
        /// <param name="postID"></param>
        void Operation_PostRelation_PostToCourse(DataTable sourceTable, int postID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceTable"></param>
        /// <param name="courseID"></param>
        void Operation_PostRelation_CourseToPost(DataTable sourceTable, int courseID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        View_HR_Train_Course GetSingleCourseInfo(int courseID);
    }
}
