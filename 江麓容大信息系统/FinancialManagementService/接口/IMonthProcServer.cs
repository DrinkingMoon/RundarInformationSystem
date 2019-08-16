using System;
namespace Service_Economic_Financial
{
    /// <summary>
    /// 月结执行服务
    /// </summary>
    public interface IMonthProcServer
    {
        /// <summary>
        /// 台帐
        /// </summary>
        /// <param name="productName">查询方式</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="storageID">库房ID</param>
        /// <param name="showTable">返回table数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool GetAllGather(string productName, DateTime startDate, DateTime endDate, 
            string storageID, out System.Data.DataTable showTable, out string error);

        /// <summary>
        /// 修改财务月结汇总的调整金额
        /// </summary>
        /// <param name="yearAndMonth">修改的年月</param>
        /// <param name="dt">数据源</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true失败返回false</returns>
        bool UpdateGather(string yearAndMonth, System.Data.DataTable dt, out string error);
    }
}
