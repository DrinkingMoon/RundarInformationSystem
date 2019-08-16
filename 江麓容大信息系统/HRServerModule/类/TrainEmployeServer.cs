using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServerModule;
using System.Data;
using PlatformManagement;

namespace Service_Peripheral_HR
{
    /// <summary>
    /// 储备人才库服务类
    /// </summary>
    class TrainEmployeServer : Service_Peripheral_HR.ITrainEmployeServer
    {
        /// <summary>
        /// 查询结果过滤器
        /// </summary>
        string m_queryResultFilter = null;

        /// <summary>
        /// 获取或设置查询结果过滤器
        /// </summary>
        public string QueryResultFilter
        {
            get { return m_queryResultFilter; }
            set { m_queryResultFilter = value; }
        }

        /// <summary>
        /// 获取所有储备人才的信息
        /// </summary>
        /// <param name="returnInfo">储备人才信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回获取到的信息</returns>
        public bool GetAllInfo(out IQueryResult returnInfo, out string error)
        {
            returnInfo = null;
            error = null;

            IAuthorization serverAuthorization = PlatformFactory.GetObject<IAuthorization>();
            IQueryResult qr = null;

            if (QueryResultFilter == null)
            {
                qr = serverAuthorization.Query("查看储备人才库", null);
            }
            else
            {
                qr = serverAuthorization.Query("查看储备人才库", null, QueryResultFilter);
            }

            if (!qr.Succeeded)
            {
                error = qr.Error;
                return false;
            }

            returnInfo = qr;
            return true;
        }

        /// <summary>
        /// 通过ID获取单个储备人才的信息
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回获取到的信息，否则返回null</returns>
        public HR_TrainEmploye GetInfoByID(int id,out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_TrainEmploye
                             where a.ID == id
                             select a;

                if (result.Count() == 1)
                {
                    return result.Single();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// 通过人才编号获得工作经验信息
        /// </summary>
        /// <param name="id">单据号</param>
        /// <returns>返回数据集</returns>
        public DataTable GetWorkHistory(int id)
        {
            string sql = "select * from View_HR_WorkHistory where 编号=" + id;

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 通过人才编号获得教育经验信息
        /// </summary>
        /// <param name="id">单据号</param>
        /// <returns>返回数据集</returns>
        public DataTable GetEducatedHistory(int id)
        {
            string sql = "select * from View_HR_EducatedHistory where 编号=" + id;

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 通过人才编号获得家庭成员信息
        /// </summary>
        /// <param name="id">单据号</param>
        /// <returns>返回数据集</returns>
        public DataTable GetFamilyMember(int id)
        {
            string sql = "select * from View_HR_FamilyMember where 编号=" + id;

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 添加储备人才信息
        /// </summary>
        /// <param name="trainEmploye">储备人才数据集</param>
        /// <param name="edeucate">教育经历</param>
        /// <param name="family">家庭成员</param>
        /// <param name="workHistory">工作经验</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool AddTrainEmploye(HR_TrainEmploye trainEmploye,List<HR_WorkHistory> workHistory,
            List<HR_EducatedHistory> edeucate,List<HR_FamilyMember> family,out string error)
        {
            error = "";
            int id = 0;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_TrainEmploye
                             where a.ID_Card == trainEmploye.ID_Card
                             select a;

                if (result.Count() == 0)
                {
                    dataContxt.HR_TrainEmploye.InsertOnSubmit(trainEmploye); 
                    dataContxt.SubmitChanges();
                }
                else
                {
                    error = "身份证为【" + trainEmploye.ID_Card + "】，姓名叫【" + trainEmploye.Name + "】的人员已经存在！";
                    return false;
                }

                var resultList = from a in dataContxt.HR_TrainEmploye
                             where a.ID_Card == trainEmploye.ID_Card
                             select a;

                if (resultList.Count() == 1)
                {
                    id = resultList.Single().ID;

                    foreach (var item in workHistory)
                    {
                        item.EmployeID = id;

                        dataContxt.HR_WorkHistory.InsertOnSubmit(item);
                    }

                    foreach (var item in edeucate)
                    {
                        item.EmployeID = id;

                        dataContxt.HR_EducatedHistory.InsertOnSubmit(item);
                    }

                    foreach (var item in family)
                    {
                        item.EmployeID = id;

                        dataContxt.HR_FamilyMember.InsertOnSubmit(item);
                    }

                    dataContxt.SubmitChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 修改储备人才信息
        /// </summary>
        /// <param name="trainEmploye">储备人才数据集</param>
        /// <param name="workHistory">工作经验</param>
        /// <param name="edeucate">教育经历</param>
        /// <param name="family">家庭成员</param>
        /// <param name="id">id</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true,失败返回False</returns>
        public bool UpdateTrainEmploye(HR_TrainEmploye trainEmploye, List<HR_WorkHistory> workHistory,
              List<HR_EducatedHistory> edeucate, List<HR_FamilyMember> family, int id, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_TrainEmploye
                                 where a.ID == id
                                 select a;

                if (result.Count() != 1)
                {
                    error = "信息有误，请查证后再操作！";
                    return false;
                }
                else
                {

                    HR_TrainEmploye trainEmployeList = result.Single();

                    trainEmployeList.Address = trainEmploye.Address;
                    trainEmployeList.Age = trainEmploye.Age;
                    trainEmployeList.Anne = trainEmploye.Anne;
                    trainEmployeList.Birthday = trainEmploye.Birthday;
                    trainEmployeList.Birthplace = trainEmploye.Birthplace;
                    trainEmployeList.College = trainEmploye.College;
                    trainEmployeList.ComputerLevel = trainEmploye.ComputerLevel;
                    trainEmployeList.DesiredSalary = trainEmploye.DesiredSalary;
                    trainEmployeList.EducatedDegree = trainEmploye.EducatedDegree;
                    trainEmployeList.EducatedMajor = trainEmploye.EducatedMajor;
                    trainEmployeList.EmergencyPhone = trainEmploye.EmergencyPhone;
                    trainEmployeList.EnglishLevel = trainEmploye.EnglishLevel;
                    trainEmployeList.Evaluate = trainEmploye.Evaluate;
                    trainEmployeList.FileName = trainEmploye.FileName;
                    trainEmployeList.Height = trainEmploye.Height;
                    trainEmployeList.ID_Card = trainEmploye.ID_Card;
                    trainEmployeList.InterviewDate = trainEmploye.InterviewDate;
                    trainEmployeList.IsThirdParty = trainEmploye.IsThirdParty;
                    trainEmployeList.JobYears = trainEmploye.JobYears;
                    trainEmployeList.MaritalStatus = trainEmploye.MaritalStatus;
                    trainEmployeList.Name = trainEmploye.Name;
                    trainEmployeList.Nationality = trainEmploye.Nationality;
                    trainEmployeList.Party = trainEmploye.Party;
                    trainEmployeList.PersonType = trainEmploye.PersonType;
                    trainEmployeList.Phone = trainEmploye.Phone;
                    trainEmployeList.Race = trainEmploye.Race;
                    trainEmployeList.Recorder = trainEmploye.Recorder;
                    trainEmployeList.RecordTime = trainEmploye.RecordTime;
                    trainEmployeList.RecruitType = trainEmploye.RecruitType;
                    trainEmployeList.Remark = trainEmploye.Remark;
                    trainEmployeList.ResumeStatus = trainEmploye.ResumeStatus;
                    trainEmployeList.Sex = trainEmploye.Sex;
                    trainEmployeList.Speciality = trainEmploye.Speciality;
                    trainEmployeList.TakeJobDate = trainEmploye.TakeJobDate;
                    trainEmployeList.ThirdParty = trainEmploye.ThirdParty;
                }

                var resultList = from c in dataContxt.HR_WorkHistory
                             where c.EmployeID == id
                             select c;

                if (resultList.Count() > 0)
                {
                    dataContxt.HR_WorkHistory.DeleteAllOnSubmit(resultList);
                }

                foreach (var item in workHistory)
                {
                    item.EmployeID = id;

                    dataContxt.HR_WorkHistory.InsertOnSubmit(item);
                }

                var resultEducated = from e in dataContxt.HR_EducatedHistory
                                     where e.EmployeID == id
                                     select e;

                if (resultEducated.Count() > 0)
                {
                    dataContxt.HR_EducatedHistory.DeleteAllOnSubmit(resultEducated);
                }

                foreach (var item in edeucate)
                {
                    item.EmployeID = id;

                    dataContxt.HR_EducatedHistory.InsertOnSubmit(item);
                }

                var resultFamily = from f in dataContxt.HR_FamilyMember
                                   where f.EmployeID == id
                                   select f;

                if (resultFamily.Count() > 0)
                {
                    dataContxt.HR_FamilyMember.DeleteAllOnSubmit(resultFamily);
                }

                foreach (var item in family)
                {
                    item.EmployeID = id;

                    dataContxt.HR_FamilyMember.InsertOnSubmit(item);
                }

                dataContxt.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 通过id删除储备人才信息
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True失败返回False</returns>
        public bool DeleteTrainEmploye(int id, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_TrainEmploye
                             where a.ID == id
                             select a;

                dataContxt.HR_TrainEmploye.DeleteAllOnSubmit(result);

                var resultWork = from b in dataContxt.HR_WorkHistory
                                      where b.EmployeID == id
                                      select b;

                if (resultWork.Count() > 0)
                {
                    dataContxt.HR_WorkHistory.DeleteAllOnSubmit(resultWork);
                }

                var resultEducated = from c in dataContxt.HR_EducatedHistory
                                     where c.EmployeID == id
                                     select c;

                if (resultEducated.Count() > 0)
                {
                    dataContxt.HR_EducatedHistory.DeleteAllOnSubmit(resultEducated);
                }

                var resultFamily = from e in dataContxt.HR_FamilyMember
                                   where e.EmployeID == id
                                   select e;

                if (resultFamily.Count() > 0)
                {
                    dataContxt.HR_FamilyMember.DeleteAllOnSubmit(resultFamily);
                }

                dataContxt.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }
    }
}
