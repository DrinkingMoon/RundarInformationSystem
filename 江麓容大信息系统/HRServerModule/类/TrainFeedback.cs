using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformManagement;
using ServerModule;
using System.Data;
using GlobalObject;

namespace Service_Peripheral_HR
{
    class TrainFeedback : ITrainFeedback
    {
        public List<View_HR_Train_Feedback> GetListInfo_Feedback(DateTime startTime, DateTime endTime)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.View_HR_Train_Feedback
                          where a.培训开始时间 <= endTime
                          && a.培训结束时间 >= startTime
                          select a;

            return varData.ToList();
        }

        public List<View_HR_Train_FeedbackUser> GetListInfo_FeedbackUser(Guid feedbackID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.View_HR_Train_FeedbackUser
                          where a.FeedbackID == feedbackID
                          select a;

            return varData.ToList();
        }

        public void InsertInfo(HR_Train_Feedback feedback, List<string> lstWorkID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            ctx.HR_Train_Feedback.InsertOnSubmit(feedback);

            foreach (string workID in lstWorkID)
            {
                HR_Train_FeedbackUser user = new HR_Train_FeedbackUser();

                user.FeedbackID = feedback.ID;
                user.WorkID = workID;

                ctx.HR_Train_FeedbackUser.InsertOnSubmit(user);
            }

            ctx.SubmitChanges();
        }

        public void DeleteInfo(string guid)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            Guid temp = new Guid(guid);

            var varData = from a in ctx.HR_Train_FeedbackUser
                          where a.FeedbackID == temp
                          select a;
            var varData1 = from a in ctx.HR_Train_Feedback
                           where a.ID == temp
                           select a;

            if (varData1.Count() > 0)
            {
                if (varData1.First().CreateUser != BasicInfo.LoginID)
                {
                    throw new Exception("您不是编制人，无法删除此记录");
                }
            }

            ctx.HR_Train_FeedbackUser.DeleteAllOnSubmit(varData);
            ctx.HR_Train_Feedback.DeleteAllOnSubmit(varData1);
            ctx.SubmitChanges();
        }
    }
}
