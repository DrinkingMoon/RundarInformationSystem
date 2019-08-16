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
    class TrainBasicInfo : ITrainBasicInfo
    {
        public DataTable GetTable<T>()
        {
            string strSql = "select * from View_"+ typeof(T).Name;

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        public DataTable GetTable_Ware(int courseID)
        {
            string strSql = "select * from View_HR_Train_Courseware where 课程ID = " + courseID;

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        public DataTable GetTable_QuestionBank(int courseID)
        {
            string strSql = "select * from View_HR_Train_QuestionBank where 课程ID = " + courseID;

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        public DataTable GetPostInfo(int? courseID)
        {
            string strSql = "";

            if (courseID == null)
            {
                strSql = "select a.* from View_HR_OperatingPost as a inner join HR_Dept as b on " +
                    " a.所属部门 = b.DeptName ";

                if (!BasicInfo.ListRoles.Contains(CE_RoleEnum.人力资源部办公室文员.ToString()))
                {
                    strSql += " where dbo.fun_get_BelongDept_Value(b.DeptCode) =  dbo.fun_get_BelongDept_Value('" + BasicInfo.DeptCode + "')";
                }
            }
            else
            {
                strSql = "select a.* from View_HR_OperatingPost as a " +
                " inner join HR_Train_PostRelation as b on a.岗位编号 = b.PostID where b.CourseID = " + (int)courseID;
            }

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        public DataTable GetCourseInfo_Comm()
        {

            string strSql = "select * from View_HR_Train_Course where 所属部门 is null";
            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        public DataTable GetCourseInfo_Comm_Dept(int courseID)
        {

            string strSql = " select 1 as 选, dbo.fun_get_Department(所属部门) as 责任部门, * " +
                            " from View_HR_Train_Course  "+
                            " where 课程ID in (select CourseID from HR_Train_Rel_CommCourse where CommCourseID = "+ courseID +") "+
                            " Union all "+
                            " select 0 as 选, dbo.fun_get_Department(所属部门) as 责任部门, *  " +
                            " from View_HR_Train_Course  " +
                            " where 课程ID not in (select CourseID from HR_Train_Rel_CommCourse) and 所属部门 is not null order by 所属部门,课程类型";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        public DataTable GetCourseInfo(int? postID)
        {
            string strSql = "";

            if (postID == null)
            {
                strSql = "select * from View_HR_Train_Course";

                if (!BasicInfo.ListRoles.Contains(CE_RoleEnum.人力资源部办公室文员.ToString()))
                {
                    strSql += " where dbo.fun_get_BelongDept_Value(所属部门) =  dbo.fun_get_BelongDept_Value('" + BasicInfo.DeptCode + "')";
                }
            }
            else
            {
                strSql = "select a.* from View_HR_Train_Course as a " +
                    " inner join HR_Train_PostRelation as b on a.课程ID = b.CourseID where b.PostID = " + postID;

                if (!BasicInfo.ListRoles.Contains(CE_RoleEnum.人力资源部办公室文员.ToString()))
                {
                    strSql += " and dbo.fun_get_BelongDept_Value(a.所属部门) =  dbo.fun_get_BelongDept_Value('" + BasicInfo.DeptCode + "')";
                }
            }

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        public HR_Train_AssessType GetSingleInfo_AssessType(HR_Train_AssessType info)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.HR_Train_AssessType
                          where a.ID == info.ID || a.AssessTypeName == info.AssessTypeName
                          select a;

            if (varData.Count() == 1)
            {
                return varData.Single();
            }
            else
            {
                return null;
            }
        }

        public HR_Train_Course GetSingleInfo_Course(HR_Train_Course info)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.HR_Train_Course
                          where a.ID == info.ID || a.CourseName == info.CourseName
                          select a;

            if (varData.Count() == 1)
            {
                return varData.Single();
            }
            else
            {
                return null;
            }
        }

        public HR_Train_CourseType GetSingleInfo_CourseType(HR_Train_CourseType info)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.HR_Train_CourseType
                          where a.ID == info.ID || a.CourseTypeName == info.CourseTypeName
                          select a;

            if (varData.Count() == 1)
            {
                return varData.Single();
            }
            else
            {
                return null;
            }
        }

        public void Operation_Course(HR_Train_Course obj)
        {
            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                HR_Train_Course temp = new HR_Train_Course();

                var varData = from a in ctx.HR_Train_Course
                              where a.ID == obj.ID
                              select a;

                if (varData.Count() == 0)
                {
                    if (!BasicInfo.ListRoles.Contains(CE_RoleEnum.人力资源部办公室文员.ToString()))
                    {
                        obj.Department = UniversalFunction.GetDept_Belonge(ctx, BasicInfo.DeptCode).DeptCode;
                    }
                    else
                    {
                        obj.Department = null;
                    }

                    ctx.HR_Train_Course.InsertOnSubmit(obj);
                }
                else if (varData.Count() == 1)
                {
                    if (obj.CourseName == null)
                    {
                        ctx.HR_Train_Course.DeleteOnSubmit(varData.Single());
                    }
                    else
                    {
                        temp = varData.Single();

                        temp.AssessID = obj.AssessID;
                        temp.ClassHour = obj.ClassHour;
                        temp.CourseName = obj.CourseName;
                        temp.Fund = obj.Fund;
                        temp.IsOutSide = obj.IsOutSide;
                        temp.Lecturer = obj.Lecturer;
                        temp.TypeID = obj.TypeID;

                        if (!BasicInfo.ListRoles.Contains(CE_RoleEnum.人力资源部办公室文员.ToString()))
                        {
                            temp.Department = UniversalFunction.GetDept_Belonge(ctx, BasicInfo.DeptCode).DeptCode;
                        }
                        else
                        {
                            temp.Department = null;
                        }
                    }
                }
                else
                {
                    throw new Exception("记录数不唯一");
                }

                ctx.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Operation_CourseType(HR_Train_CourseType obj)
        {
            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                HR_Train_CourseType temp = new HR_Train_CourseType();

                var varData = from a in ctx.HR_Train_CourseType
                              where a.ID == obj.ID
                              select a;

                if (varData.Count() == 0)
                {
                    if (obj.ID == 0)
                    {
                        var varkk = from a in ctx.HR_Train_CourseType
                                    where a.CourseTypeName == obj.CourseTypeName
                                    select a;

                        if (varkk.Count() > 0)
                        {
                            throw new Exception("【名称】："+ obj.CourseTypeName +"  重复");
                        }
                    }

                    ctx.HR_Train_CourseType.InsertOnSubmit(obj);
                }
                else if (varData.Count() == 1)
                {
                    if (obj.CourseTypeName == null)
                    {
                        ctx.HR_Train_CourseType.DeleteOnSubmit(varData.Single());
                    }
                    else
                    {
                        temp = varData.Single();

                        temp.CourseTypeName = obj.CourseTypeName;
                    }
                }
                else
                {
                    throw new Exception("记录数不唯一");
                }

                ctx.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Operation_Courseware(HR_Train_Courseware obj)
        {
            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                HR_Train_Courseware temp = new HR_Train_Courseware();

                var varData = from a in ctx.HR_Train_Courseware
                              where a.ID == obj.ID
                              select a;

                if (varData.Count() == 0)
                {
                    ctx.HR_Train_Courseware.InsertOnSubmit(obj);
                }
                else if (varData.Count() == 1)
                {
                    if (obj.CoursewareName == null)
                    {
                        ctx.HR_Train_Courseware.DeleteOnSubmit(varData.Single());
                    }
                    else
                    {
                        temp = varData.Single();

                        temp.CoursewareName = obj.CoursewareName;
                        temp.CourseID = obj.CourseID;
                        temp.FileUnique = obj.FileUnique;
                    }
                }
                else
                {
                    throw new Exception("记录数不唯一");
                }

                ctx.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Operation_AssessType(HR_Train_AssessType obj)
        {
            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                HR_Train_AssessType temp = new HR_Train_AssessType();

                var varData = from a in ctx.HR_Train_AssessType
                              where a.ID == obj.ID
                              select a;

                if (varData.Count() == 0)
                {
                    if (obj.ID == 0)
                    {
                        var varkk = from a in ctx.HR_Train_AssessType
                                    where a.AssessTypeName == obj.AssessTypeName
                                    select a;

                        if (varkk.Count() > 0)
                        {
                            throw new Exception("【名称】：" + obj.AssessTypeName + "  重复");
                        }
                    }

                    ctx.HR_Train_AssessType.InsertOnSubmit(obj);
                }
                else if (varData.Count() == 1)
                {
                    if (obj.AssessTypeName == null)
                    {
                        ctx.HR_Train_AssessType.DeleteOnSubmit(varData.Single());
                    }
                    else
                    {
                        temp = varData.Single();

                        temp.AssessTypeName = obj.AssessTypeName;
                        temp.IsExam = obj.IsExam;
                    }
                }
                else
                {
                    throw new Exception("记录数不唯一");
                }

                ctx.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Operation_QuestionBank(HR_Train_QuestionBank obj)
        {
            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                HR_Train_QuestionBank temp = new HR_Train_QuestionBank();

                var varData = from a in ctx.HR_Train_QuestionBank
                              where a.ID == obj.ID
                              select a;

                if (varData.Count() == 0)
                {
                    ctx.HR_Train_QuestionBank.InsertOnSubmit(obj);
                }
                else if (varData.Count() == 1)
                {
                    if (obj.Type == null)
                    {
                        ctx.HR_Train_QuestionBank.DeleteOnSubmit(varData.Single());
                    }
                    else
                    {
                        temp = varData.Single();

                        temp.Answer = obj.Answer;
                        temp.CourseID = obj.CourseID;
                        temp.Questions = obj.Questions;
                        temp.Type = obj.Type;
                    }
                }
                else
                {
                    throw new Exception("记录数不唯一");
                }

                ctx.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Operation_PostRelation_PostToCourse(DataTable sourceTable, int postID)
        {
            try
            {
                if (postID == 0)
                {
                    throw new Exception("数据为空无法保存");
                }

                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                HR_Train_PostRelation temp = new HR_Train_PostRelation();

                var varData = from a in ctx.HR_Train_PostRelation
                              where a.PostID == postID
                              select a;

                ctx.HR_Train_PostRelation.DeleteAllOnSubmit(varData);

                if (sourceTable != null)
                {
                    foreach (DataRow dr in sourceTable.Rows)
                    {
                        HR_Train_PostRelation rel = new HR_Train_PostRelation();

                        rel.PostID = postID;
                        rel.CourseID = Convert.ToInt32(dr["课程ID"]);

                        ctx.HR_Train_PostRelation.InsertOnSubmit(rel);
                    }
                }

                ctx.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Operation_PostRelation_CourseToPost(DataTable sourceTable, int courseID)
        {
            try
            {
                if (courseID == 0)
                {
                    throw new Exception("数据为空无法保存");
                }

                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                HR_Train_PostRelation temp = new HR_Train_PostRelation();

                var varData = from a in ctx.HR_Train_PostRelation
                              where a.CourseID == courseID
                              select a;

                ctx.HR_Train_PostRelation.DeleteAllOnSubmit(varData);

                if (sourceTable != null)
                {
                    foreach (DataRow dr in sourceTable.Rows)
                    {
                        HR_Train_PostRelation rel = new HR_Train_PostRelation();

                        rel.CourseID = courseID;
                        rel.PostID = Convert.ToInt32(dr["岗位编号"]);

                        ctx.HR_Train_PostRelation.InsertOnSubmit(rel);
                    }
                }

                ctx.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Operation_Comm_Rel(int commCourseID, List<int> lstDeptCourse)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            try
            {
                var varData = from a in ctx.HR_Train_Rel_CommCourse
                              where a.CommCourseID == commCourseID
                              select a;

                ctx.HR_Train_Rel_CommCourse.DeleteAllOnSubmit(varData);

                foreach (int item in lstDeptCourse)
                {
                    HR_Train_Rel_CommCourse temp = new HR_Train_Rel_CommCourse();

                    temp.ID = Guid.NewGuid();
                    temp.CommCourseID = commCourseID;
                    temp.CourseID = item;

                    ctx.HR_Train_Rel_CommCourse.InsertOnSubmit(temp);
                }

                ctx.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Operation_Ware(HR_Train_Courseware obj)
        {
            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.HR_Train_Courseware
                              where a.ID == obj.ID
                              select a;

                if (varData.Count() == 1)
                {
                    ctx.HR_Train_Courseware.DeleteAllOnSubmit(varData);
                }
                else if (varData.Count() == 0)
                {
                    ctx.HR_Train_Courseware.InsertOnSubmit(obj);
                }
                else
                {
                    throw new Exception("数据不唯一");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public View_HR_Train_Course GetSingleCourseInfo(int courseID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.View_HR_Train_Course
                          where a.课程ID == courseID
                          select a;

            if (varData.Count() == 1)
            {
                return varData.Single();
            }
            else
            {
                return null;
            }
        }

        public List<string> GetCourse_User(int courseID)
        {
            string error = null;

            Hashtable hsTable = new Hashtable();
            hsTable.Add("@CourseID", courseID);
            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfoPro("HR_Train_GetCourseUser", hsTable, out error);

            return DataSetHelper.ColumnsToList_Distinct(dtTemp, "WorkID");
        }

        public void InputQuestionsBank(int courseID, DataTable questionsTable)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();

            try
            {
                questionsTable = DataSetHelper.OrderBy(questionsTable, "考题ID,选项");

                List<string> lstQuestions = DataSetHelper.ColumnsToList_Distinct(questionsTable, "考题ID");

                foreach (string questionsID in lstQuestions)
                {
                    DataTable tempQuestions = DataSetHelper.SiftDataTable(questionsTable, "考题ID = '" + questionsID + "'");

                    if (tempQuestions.Rows.Count > 0)
                    {
                        CE_HR_Train_QuesitonsType quesType = 
                            GeneralFunction.StringConvertToEnum<CE_HR_Train_QuesitonsType>(tempQuestions.Rows[0]["考题类型"].ToString());

                        HR_Train_QuestionBank bank = new HR_Train_QuestionBank();
                        switch (quesType)
                        {
                            case CE_HR_Train_QuesitonsType.判断题:
                                bank.Answer = tempQuestions.Rows[0]["答案"].ToString().ToUpper();
                                bank.CourseID = courseID;
                                bank.ID = Guid.NewGuid();
                                bank.Questions = tempQuestions.Rows[0]["考题内容"].ToString();
                                bank.Type = tempQuestions.Rows[0]["考题类型"].ToString();

                                ctx.HR_Train_QuestionBank.InsertOnSubmit(bank);

                                break;
                            case CE_HR_Train_QuesitonsType.单选题:
                            case CE_HR_Train_QuesitonsType.多选题:

                                Guid guid = Guid.NewGuid();

                                bank.Answer = tempQuestions.Rows[0]["答案"].ToString().ToUpper();
                                bank.CourseID = courseID;
                                bank.ID = guid;
                                bank.Questions = tempQuestions.Rows[0]["考题内容"].ToString();
                                bank.Type = tempQuestions.Rows[0]["考题类型"].ToString();

                                ctx.HR_Train_QuestionBank.InsertOnSubmit(bank);
                                ctx.SubmitChanges();

                                foreach (DataRow dr in tempQuestions.Rows)
                                {
                                    HR_Train_QuestionBank_Option option = new HR_Train_QuestionBank_Option();

                                    option.OptionContent = dr["选项内容"].ToString();
                                    option.OptionTag = dr["选项"].ToString().ToUpper();
                                    option.QuestionID = guid;

                                    ctx.HR_Train_QuestionBank_Option.InsertOnSubmit(option);
                                }

                                ctx.SubmitChanges();
                                break;
                            default:
                                break;
                        }
                    }
                }

                ctx.SubmitChanges();
                ctx.Transaction.Commit();
            }
            catch (Exception ex)
            {
                ctx.Transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }

        public List<View_HR_Train_QuestionBank> GetListQuestionBank(int courseID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.View_HR_Train_QuestionBank
                          where a.课程ID == courseID
                          select a;

            return varData.ToList();
        }

        public void DeleteQuestion(string guid)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.HR_Train_QuestionBank_Option
                          where a.QuestionID == new Guid(guid)
                          select a;

            var varData1 = from a in ctx.HR_Train_QuestionBank
                           where a.ID == new Guid(guid)
                           select a;

            ctx.HR_Train_QuestionBank_Option.DeleteAllOnSubmit(varData);
            ctx.HR_Train_QuestionBank.DeleteAllOnSubmit(varData1);
            ctx.SubmitChanges();
        }

        public List<View_HR_Train_QuestionBank> GetListQuestionBank(Guid guid)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.View_HR_Train_QuestionBank
                          where a.考题ID == guid
                          orderby a.选项
                          select a;

            return varData.ToList();
        }

        public void UpdateCourseExamInfo(HR_Train_Course courseInfo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.HR_Train_Course
                          where a.ID == courseInfo.ID
                          select a;

            if (varData.Count() == 1)
            {
                varData.Single().ExamExtractionRate = courseInfo.ExamExtractionRate;
                varData.Single().ExamPassRate = courseInfo.ExamPassRate;
            }

            ctx.SubmitChanges();
        }

        public HR_Train_QuestionBank GetBankInfo(Guid guid)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.HR_Train_QuestionBank
                          where a.ID == guid
                          select a;

            if (varData.Count() == 1)
            {
                return varData.Single();
            }
            else
            {
                return null;
            }
        }

        public HR_Train_Course GetSingleCourseTableInfo(int courseID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.HR_Train_Course
                          where a.ID == courseID
                          select a;

            if (varData.Count() == 1)
            {
                return varData.Single();
            }
            else
            {
                return null;
            }
        }
    }
}
