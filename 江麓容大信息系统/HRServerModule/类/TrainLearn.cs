using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformManagement;
using ServerModule;
using System.Data;
using GlobalObject;
using FlowControlService;
using System.Collections;

namespace Service_Peripheral_HR
{
    class TrainLearn : ITrainLearn
    {
        public DataTable GetTree_Course()
        {
            try
            {
                string error = null;
                Hashtable hs = new Hashtable();

                hs.Add("@WorkID", BasicInfo.LoginID);

                DataTable tempTable = GlobalObject.DatabaseServer.QueryInfoPro("HR_Train_LearnCourseTree", hs, out error);

                return tempTable;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Guid> GetRandomQuestion(int courseID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.HR_Train_QuestionBank
                          where a.CourseID == courseID
                          orderby Guid.NewGuid()
                          select a.ID;

            ITrainBasicInfo serviceBasic = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<ITrainBasicInfo>();

            HR_Train_Course courseInfo = serviceBasic.GetSingleCourseTableInfo(courseID);

            if (courseInfo == null)
            {
                throw new Exception("获取数据出错");
            }

            int percentValue = courseInfo.ExamExtractionRate == null ? 0 : (int)courseInfo.ExamExtractionRate;
            int rowCount = varData.Count() * percentValue / 100;

            return varData.Take(rowCount).ToList();
        }

        public void RecordExamHistory(ref HR_Train_ExamHistory history)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            ITrainBasicInfo serviceBasic = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<ITrainBasicInfo>();
            HR_Train_Course courseInfo = serviceBasic.GetSingleCourseTableInfo((int)history.CourseID);

            if (courseInfo == null)
            {
                throw new Exception("获取数据失败");
            }

            if (courseInfo.ExamPassRate == null)
            {
                history.IsPass = true;
            }
            else
            {
                history.IsPass = history.ExamScore >= courseInfo.ExamPassRate;
            }

            history.ExamDate = ServerTime.Time;
            history.WorkID = BasicInfo.LoginID;

            ctx.HR_Train_ExamHistory.InsertOnSubmit(history);
            ctx.SubmitChanges();
        }
    }
}
