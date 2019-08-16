using System;
namespace Service_Peripheral_HR
{
    /// <summary>
    /// 职称信息管理类
    /// </summary>
    public interface IJobTitleServer
    {
        /// <summary>
        /// 修改或新增职称信息
        /// </summary>
        /// <param name="jobTitle">职称信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool AddAndUpdateJobTitle(ServerModule.HR_JobTitle jobTitle, out string error);

        /// <summary>
        /// 删除职称信息
        /// </summary>
        /// <param name="jobTitle">职称信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool DeleteJobTitle(ServerModule.HR_JobTitle jobTitle, out string error);

        /// <summary>
        /// 获得所有职称信息
        /// </summary>
        /// <returns>返回职称数据集</returns>
        System.Data.DataTable GetJobTitle();

        /// <summary>
        /// 通过职称名称获得职称编号
        /// </summary>
        /// <param name="jobTitleName">职称名称</param>
        /// <returns>返回职称编号</returns>
        int GetJobTitleByJobName(string jobTitleName);

        /// <summary>
        /// 通过职称编号获得职称名称
        /// </summary>
        /// <param name="jobTitleID">职称编号</param>
        /// <returns>返回职称名称</returns>
        string GetJobTitleByJobID(int jobTitleID);

        /// <summary>
        /// 获得所有外部职称信息
        /// </summary>
        /// <returns>返回职称数据集</returns>
        System.Data.DataTable GetJobTitleOut();

        /// <summary>
        /// 获得所有内部级别信息
        /// </summary>
        /// <returns>返回职称数据集</returns>
        System.Data.DataTable GetJobTitleLevel();
    }
}
