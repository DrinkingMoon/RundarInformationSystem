using System;
using PlatformManagement;
namespace Service_Peripheral_HR
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITrainEmployeServer
    {
        /// <summary>
        /// 添加储备人才信息
        /// </summary>
        /// <param name="trainEmploye">储备人才数据集</param>
        /// <param name="edeucate">教育经历</param>
        /// <param name="family">家庭成员</param>
        /// <param name="workHistory">工作经验</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool AddTrainEmploye(ServerModule.HR_TrainEmploye trainEmploye, 
            System.Collections.Generic.List<ServerModule.HR_WorkHistory> workHistory, 
            System.Collections.Generic.List<ServerModule.HR_EducatedHistory> edeucate, 
            System.Collections.Generic.List<ServerModule.HR_FamilyMember> family, out string error);

        /// <summary>
        /// 通过id删除储备人才信息
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True失败返回False</returns>
        bool DeleteTrainEmploye(int id, out string error);

        /// <summary>
        /// 获取所有储备人才的信息
        /// </summary>
        /// <param name="returnInfo">储备人才信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回获取到的信息</returns>
        bool GetAllInfo(out IQueryResult returnInfo, out string error);

        /// <summary>
        /// 通过人才编号获得教育经验信息
        /// </summary>
        /// <param name="id">单据号</param>
        /// <returns>返回数据集</returns>
        System.Data.DataTable GetEducatedHistory(int id);

        /// <summary>
        /// 通过人才编号获得家庭成员信息
        /// </summary>
        /// <param name="id">单据号</param>
        /// <returns>返回数据集</returns>
        System.Data.DataTable GetFamilyMember(int id);

        /// <summary>
        /// 通过ID获取单个储备人才的信息
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回获取到的信息，否则返回null</returns>
        ServerModule.HR_TrainEmploye GetInfoByID(int id, out string error);

        /// <summary>
        /// 通过人才编号获得工作经验信息
        /// </summary>
        /// <param name="id">单据号</param>
        /// <returns>返回数据集</returns>
        System.Data.DataTable GetWorkHistory(int id);

        /// <summary>
        /// 获取或设置查询结果过滤器
        /// </summary>
        string QueryResultFilter { get; set; }

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
        bool UpdateTrainEmploye(ServerModule.HR_TrainEmploye trainEmploye, 
            System.Collections.Generic.List<ServerModule.HR_WorkHistory> workHistory, 
            System.Collections.Generic.List<ServerModule.HR_EducatedHistory> edeucate, 
            System.Collections.Generic.List<ServerModule.HR_FamilyMember> family, int id, out string error);
    }
}
