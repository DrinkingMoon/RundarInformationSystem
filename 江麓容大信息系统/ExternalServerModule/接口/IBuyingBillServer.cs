
using System;
namespace Service_Peripheral_External
{
    /// <summary>
    /// 挂账单服务接口
    /// </summary>
    public interface IBuyingBillServer
    {
        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool DeleteBill(int billNo, out string error);

        /// <summary>
        /// 获得单据列表
        /// </summary>
        /// <param name="billStatus">单据状态</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>返回Table</returns>
        System.Data.DataTable GetAllBillInfo(string billStatus, DateTime startTime, DateTime endTime);

        /// <summary>
        /// 获得单条信息
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <returns>返回LINQ数据集</returns>
        ServerModule.View_Out_BuyingBill GetBillInfo(int billID);

        /// <summary>
        /// 获得明细信息
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <returns>返回Table</returns>
        System.Data.DataTable GetListInfo(int billID);

        /// <summary>
        /// 获得明细信息
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回LINQ数据集</returns>
        System.Linq.IQueryable<ServerModule.Out_BuyingList> GetListInfo(int goodsID, int billID);

        /// <summary>
        /// 获得账务库房ID
        /// </summary>
        /// <param name="SecStorageID">库房编号</param>
        /// <param name="goodsID">物品ID</param>
        /// <returns>成功返回满足条件的库房ID，失败返回NULL</returns>
        string GetStockInfo(string SecStorageID, int goodsID);

        /// <summary>
        /// 提交申请
        /// </summary>
        /// <param name="buyingBill">单据信息数据集</param>
        /// <param name="listInfo">明细信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool InsertBill(ServerModule.Out_BuyingBill buyingBill, System.Data.DataTable listInfo, out string error);

        /// <summary>
        /// 操作业务
        /// </summary>
        /// <param name="buyingBill">单据信息数据集</param>
        /// <param name="listInfo">单据明细信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool OperationInfo(ServerModule.Out_BuyingBill buyingBill, System.Data.DataTable listInfo, out string error);
    }
}
