using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ServerModule;
using PlatformManagement;

namespace Service_Peripheral_HR
{
    /// <summary>
    /// 人员简历管理类
    /// </summary>
    class ResumeServer : Service_Peripheral_HR.IResumeServer
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
        /// 获得简历状态
        /// </summary>
        /// <returns>简历状态数据集</returns>
        public DataTable GetResumeStatus()
        {
            string sql = "select * from HR_ResumeStatus";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 通过ID获得简历状态
        /// </summary>
        /// <param name="statusCode">状态ID</param>
        /// <returns>返回对应的简历状态</returns>
        public string GetResumeStatusByID(int statusCode)
        {
            string sql = "select Status from HR_ResumeStatus where ID=" + statusCode;

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt.Rows[0]["Status"].ToString();
        }

        /// <summary>
        ///通过状态获得状态ID 
        /// </summary>
        /// <param name="status">简历状态</param>
        /// <returns>返回对应的ID</returns>
        public int GetResumeStatusByStatus(string status)
        {
            string sql = "select ID from HR_ResumeStatus where Status='" + status + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return Convert.ToInt32(dt.Rows[0]["ID"].ToString());
        }

        /// <summary>
        /// 获得人员简历信息
        /// </summary>
        /// <param name="starTime">起始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>返回人员简历数据集</returns>
        public DataTable GetResume(string starTime,string endTime)
        {
            string sql = "select * from View_HR_Resume";

            if (starTime != null)
            {
                sql += " where 记录时间>='" + starTime + "' and 记录时间<='" + endTime + "'";
            }

            sql += " order by 记录时间 desc";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 通过编号获得人员简历信息
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns>返回人员简历数据集</returns>
        public DataTable GetResume(string id)
        {
            string sql = "select * from View_HR_Resume where 编号='" + id + "'";

            sql += " order by 记录时间 desc";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 获取人员简历信息
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns>返回获取到的职员信息, 获取不到返回null</returns>
        public HR_Resume GetResumelInfo(int id)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.HR_Resume
                         where r.ID == id
                         select r;

            if (result.Count() > 0)
            {
                return result.Single();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取人员简历信息
        /// </summary>
        /// <param name="card">身份证</param>
        /// <returns>返回获取到的职员信息, 获取不到返回null</returns>
        public HR_Resume GetResumelInfo(string card)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.HR_Resume
                         where r.ID_Card == card
                         select r;

            if (result.Count() > 0)
            {
                return result.Single();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 添加/更新人员简历
        /// </summary>
        /// <param name="resume">人员简历数据集</param>
        /// <param name="status">状态(1为修改，0为新增)</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool AddResume(HR_Resume resume,int status, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_Resume
                             where a.ID_Card == resume.ID_Card
                             select a;

                if (result.Count() != 0)
                {
                    if (status == 1)//1 代表修改；0 代表新增
                    {
                        HR_Resume resumeList = result.Single();

                        resumeList.RecruitmentType = resume.RecruitmentType;
                        resumeList.ResumeStatusID = resume.ResumeStatusID;
                        resumeList.Sex = resume.Sex;
                        resumeList.Birthday = resume.Birthday;
                        resumeList.Age = resume.Age;
                        resumeList.Height = resume.Height;
                        resumeList.Nationality = resume.Nationality;
                        resumeList.Race = resume.Race;
                        resumeList.Birthplace = resume.Birthplace;
                        resumeList.Party = resume.Party;
                        resumeList.MaritalStatus = resume.MaritalStatus;
                        resumeList.College = resume.College;
                        resumeList.EducatedDegree = resume.EducatedDegree;
                        resumeList.EducatedMajor = resume.EducatedMajor;
                        resumeList.FamilyAddress = resume.FamilyAddress;
                        resumeList.Postcode = resume.Postcode;
                        resumeList.Photo = resume.Photo;
                        resumeList.EmergencyPhone = resume.EmergencyPhone;
                        resumeList.Speciality = resume.Speciality;
                        resumeList.EnglishLevel = resume.EnglishLevel;
                        resumeList.ComputerLevel = resume.ComputerLevel;
                        resumeList.ExistThirdPartyRelation = resume.ExistThirdPartyRelation;
                        resumeList.ThirdPartyRelation = resume.ThirdPartyRelation;
                        resumeList.WorkingTenure = resume.WorkingTenure;
                        resumeList.DesiredSalary = resume.DesiredSalary;
                        resumeList.Phone = resume.Phone;
                        resumeList.Annex = resume.Annex;
                        resumeList.PathName = resume.PathName;
                        resumeList.Remark = resume.Remark;
                        resumeList.EducatedHistory = resume.EducatedHistory;
                        resumeList.WorkHistory = resume.WorkHistory;
                        resumeList.FamilyMember = resume.FamilyMember;
                        resumeList.Recorder = resume.Recorder;
                        resumeList.RecordTime = resume.RecordTime;
                    }
                    else
                    {
                        error = "身份证为【"+resume.ID_Card+"】，姓名叫【"+resume.Name+"】的人员已经存在！";
                        return false;
                    }
                }
                else
                {
                    dataContxt.HR_Resume.InsertOnSubmit(resume);
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
        /// 删除人员简历信息
        /// </summary>
        /// <param name="card">人员身份证号</param>
        /// <param name="resumeID">简历编号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回false</returns>
        public bool DeleteResume(string card,int resumeID,out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var resultPersonnel = from c in dataContxt.HR_PersonnelArchive
                                      where c.ResumeID == resumeID
                                      select c;

                if (resultPersonnel.Count() > 0)
                {
                    error = "身份证为【" + card + "】的" + resumeID + "号简历关联到了人员档案，不能删除！";
                    return false;
                }

                var result = from a in dataContxt.HR_Resume
                             where a.ID_Card == card
                             select a;

                if (result.Count() != 1)
                {
                    error = "信息有误，不能删除！";
                    return false;
                }

                dataContxt.HR_Resume.DeleteAllOnSubmit(result);
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
                qr = serverAuthorization.Query("查看人员简历", null);
            }
            else
            {
                qr = serverAuthorization.Query("查看人员简历", null, QueryResultFilter);
            }

            if (!qr.Succeeded)
            {
                error = qr.Error;
                return false;
            }

            returnInfo = qr;
            return true;
        }
    }
}
