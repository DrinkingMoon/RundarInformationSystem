using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ServerModule;

namespace Service_Peripheral_HR
{
    /// <summary>
    /// 职称信息管理类
    /// </summary>
    class JobTitleServer : Service_Peripheral_HR.IJobTitleServer
    {
        /// <summary>
        /// 获得所有职称信息
        /// </summary>
        /// <returns>返回职称数据集</returns>
        public DataTable GetJobTitle()
        {
            string sql = "select JobTitleID 职称编号, JobTitle 职称名称,IsInternalJobTitle 是否是外部职称, Recorder 记录人员, RecordTime 记录时间 from HR_JobTitle";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 获得所有外部职称信息
        /// </summary>
        /// <returns>返回职称数据集</returns>
        public DataTable GetJobTitleOut()
        {
            string sql = "select JobTitleID 职称编号, JobTitle 职称名称,IsInternalJobTitle 是否是外部职称, Recorder 记录人员, RecordTime 记录时间 "+
                        " from HR_JobTitle where IsInternalJobTitle='1'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 获得所有内部级别信息
        /// </summary>
        /// <returns>返回职称数据集</returns>
        public DataTable GetJobTitleLevel()
        {
            string sql = "select JobTitleID 职称编号, JobTitle 职称名称,IsInternalJobTitle 是否是外部职称, Recorder 记录人员, RecordTime 记录时间 " +
                        " from HR_JobTitle where IsInternalJobTitle='0'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 通过职称名称获得职称编号
        /// </summary>
        /// <param name="jobTitleName">职称名称</param>
        /// <returns>返回职称编号</returns>
        public int GetJobTitleByJobName(string jobTitleName)
        {
            string sql = "select JobTitleID from HR_JobTitle where JobTitle='"+jobTitleName+"'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return Convert.ToInt32(dt.Rows[0]["JobTitleID"].ToString());
        }

        /// <summary>
        /// 通过职称编号获得职称名称
        /// </summary>
        /// <param name="jobTitleID">职称编号</param>
        /// <returns>返回职称名称</returns>
        public string GetJobTitleByJobID(int jobTitleID)
        {
            string sql = "select JobTitle from HR_JobTitle where JobTitleID='" + jobTitleID + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt.Rows[0]["JobTitle"].ToString();
        }

        /// <summary>
        /// 新增职称信息
        /// </summary>
        /// <param name="jobTitle">职称信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool AddAndUpdateJobTitle(HR_JobTitle jobTitle, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_JobTitle
                             where a.JobTitle == jobTitle.JobTitle && a.IsInternalJobTitle == jobTitle.IsInternalJobTitle
                             select a;
            
                if (result.Count() != 0)
                {
                    error = "已经存在【"+jobTitle.JobTitle+"】！";
                    return false;
                }

                else
                {
                    dataContxt.HR_JobTitle.InsertOnSubmit(jobTitle);
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
        /// 删除职称信息
        /// </summary>
        /// <param name="jobTitle">职称信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool DeleteJobTitle(HR_JobTitle jobTitle, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_JobTitle
                             where a.JobTitleID == jobTitle.JobTitleID
                             select a;
                //修改
                if (result.Count() != 1)
                {
                    error = "信息有误，请重新确定";
                    return false;
                }

                dataContxt.HR_JobTitle.DeleteAllOnSubmit(result);
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
