using System;
using ServerModule;
using System.Data;
using PlatformManagement;

namespace Service_Peripheral_HR
{
    /// <summary>
    /// 人员简历管理类
    /// </summary>
    public interface IResumeServer
    {
        /// <summary>
        /// 获取人员简历信息
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns>返回获取到的职员信息, 获取不到返回null</returns>
        HR_Resume GetResumelInfo(int id);

        /// <summary>
        /// 获取所有储备人才的信息
        /// </summary>
        /// <param name="returnInfo">储备人才信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回获取到的信息</returns>
        bool GetAllInfo(out IQueryResult returnInfo, out string error);

        /// <summary>
        /// 添加/更新人员简历
        /// </summary>
        /// <param name="resume">人员简历数据集</param>
        /// <param name="status">状态(1为修改，0为新增)</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool AddResume(ServerModule.HR_Resume resume,int status, out string error);

        /// <summary>
        /// 删除人员简历信息
        /// </summary>
        /// <param name="card">人员身份证号</param>
        /// <param name="resumeID">简历编号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回false</returns>
        bool DeleteResume(string card, int resumeID, out string error);

         /// <summary>
        /// 通过编号获得人员简历信息
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns>返回人员简历数据集</returns>
        DataTable GetResume(string id);

        /// <summary>
        /// 获取人员简历信息
        /// </summary>
        /// <param name="card">身份证</param>
        /// <returns>返回获取到的职员信息, 获取不到返回null</returns>
        HR_Resume GetResumelInfo(string card);

        /// <summary>
        /// 获得人员简历信息
        /// </summary>
        /// <param name="starTime">起始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>返回人员简历数据集</returns>
        System.Data.DataTable GetResume(string starTime, string endTime);

        /// <summary>
        /// 获得简历状态
        /// </summary>
        /// <returns>简历状态数据集</returns>
        System.Data.DataTable GetResumeStatus();

        /// <summary>
        /// 通过ID获得简历状态
        /// </summary>
        /// <param name="statusCode">状态ID</param>
        /// <returns>返回对应的简历状态</returns>
        string GetResumeStatusByID(int statusCode);

        ///<summary>
        ///通过状态获得状态ID 
        /// </summary>
        /// <param name="status">简历状态</param>
        /// <returns>返回对应的ID</returns>
        int GetResumeStatusByStatus(string status);
    }
}
