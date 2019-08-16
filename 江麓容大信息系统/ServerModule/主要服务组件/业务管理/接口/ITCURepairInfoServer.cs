using System;
namespace ServerModule
{
    /// <summary>
    /// TCU返修信息服务接口
    /// </summary>
    public interface ITCURepairInfoServer
    {
        /// <summary>
        /// 质管确认修改TCU返修信息
        /// </summary>
        /// <param name="billNo">单号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true，失败返回false</returns>
        bool ConfirmUpdateData(string billNo, out string error);

        /// <summary>
        /// 通过单号删除TCU返修信息
        /// </summary>
        /// <param name="billNo">单号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true，失败返回false</returns>
        bool DeleteData(string billNo, out string error);
        /// <summary>
        /// 获取所有返修信息
        /// </summary>
        /// <param name="result">结果集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool GetAllData(out PlatformManagement.IQueryResult result, out string error);

        /// <summary>
        /// 通过单号获取一条信息
        /// </summary>
        /// <param name="billNo">单号</param>
        /// <returns>成功返回TCU_RepairInfo数据集，失败返回null</returns>
        ServerModule.TCU_RepairInfo GetDataByBillNo(string billNo);

        /// <summary>
        /// 添加一条TCU返修信息
        /// </summary>
        /// <param name="repairInfo">TCU返修信息数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true，失败返回false</returns>
        bool InsertData(ServerModule.TCU_RepairInfo repairInfo, out string error);

        /// <summary>
        /// 获取或设置查询结果过滤器
        /// </summary>
        string QueryResultFilter { get; set; }

        /// <summary>
        /// 通过单号修改TCU返修信息(先删后增)
        /// </summary>
        /// <param name="repairInfo">TCU返修信息数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true，失败返回false</returns>
        bool UpdateData(ServerModule.TCU_RepairInfo repairInfo, out string error);

        /// <summary>
        /// 获得最大编号
        /// </summary>
        /// <returns>成功返回最大编号，失败返回null</returns>
        string GetMaxBillNo();
    }
}
