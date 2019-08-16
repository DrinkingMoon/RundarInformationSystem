using System;
namespace Service_Peripheral_HR
{
    /// <summary>
    /// 
    /// </summary>
    public interface IExpertEmployeServer
    {
        /// <summary>
        /// 添加专家专业人才信息
        /// </summary>
        /// <param name="expertEmploye">专家专业人才数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool AddExpertEmploye(ServerModule.HR_ExpertEmploye expertEmploye, out string error);

        /// <summary>
        /// 通过id删除专家专业人才信息
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True失败返回False</returns>
        bool DeleteTrainEmploye(int id, out string error);

        /// <summary>
        /// 获取专家专业人才库的信息
        /// </summary>
        /// <param name="returnInfo">专家专业人才信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回获取到的信息</returns>
        bool GetAllInfo(out PlatformManagement.IQueryResult returnInfo, out string error);

        /// <summary>
        /// 通过ID获取单个专家专业人才库的信息
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回获取到的信息，否则返回null</returns>
        ServerModule.HR_ExpertEmploye GetInfoByID(int id, out string error);

        /// <summary>
        /// 获取或设置查询结果过滤器
        /// </summary>
        string QueryResultFilter { get; set; }

        /// <summary>
        /// 修改专家专业人才信息
        /// </summary>
        /// <param name="expertEmploye">专家专业人才数据集</param>
        /// <param name="id">id</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true,失败返回False</returns>
        bool UpdateExpertEmploye(ServerModule.HR_ExpertEmploye expertEmploye, int id, out string error);
    }
}
