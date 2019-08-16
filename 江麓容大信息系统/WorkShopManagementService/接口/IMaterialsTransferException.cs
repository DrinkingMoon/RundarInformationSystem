using System;
using ServerModule;

namespace Service_Manufacture_WorkShop
{
    /// <summary>
    /// 自动生成转换异常信息处理类接口
    /// </summary>
    public interface IMaterialsTransferException : IBasicBillServer
    {
        /// <summary>
        /// 获得所有异常信息
        /// </summary>
        /// <returns>返回Table</returns>
        System.Data.DataTable GetBillInfo();

        /// <summary>
        /// 获得异常处理所有信息
        /// </summary>
        /// <param name="listID">异常信息ID</param>
        /// <returns>返回Table</returns>
        System.Data.DataTable GetDisposeInfo(int listID);

        /// <summary>
        /// 获得单条异常的所有明细信息
        /// </summary>
        /// <param name="billNo">单号</param>
        /// <returns>返回Table</returns>
        System.Data.DataTable GetListInfo(string billNo);

        /// <summary>
        /// 获得单条异常信息
        /// </summary>
        /// <param name="billNo">单号</param>
        /// <returns>返回Table</returns>
        ServerModule.WS_MaterialsTransferExceptionBill GetSingleBillInfo(string billNo);

        /// <summary>
        /// 获得单条异常的单条明细信息
        /// </summary>
        /// <param name="billNo">单号</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <returns>返回单条LINQ</returns>
        ServerModule.WS_MaterialsTransferExceptionList GetSingleListInfo(string billNo, int goodsID, string batchNo);

        /// <summary>
        /// 处理异常信息
        /// </summary>
        /// <param name="mode">操作模式</param>
        /// <param name="operationDispose">处理信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        bool OperationDisposeInfo(GlobalObject.CE_OperatorMode mode, ServerModule.WS_MaterialsTransferExceptionListDispose operationDispose, 
            out string error);

        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="billNo">单号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True , 失败返回False</returns>
        bool DeleteBill(string billNo, out string error);

        /// <summary>
        /// 插入异常信息
        /// </summary>
        /// <param name="billNo">单号</param>
        /// <returns>成功返回True ,失败返回False</returns>
        bool InsertExceptionInfo(string billNo, out string error);

        /// <summary>
        /// 执行异常信息处理
        /// </summary>
        /// <param name="billNo">单号</param>
        /// <returns>成功返回True ,失败返回False</returns>
        bool ExcuteDisposeInfo(string billNo, out string error);
    }
}
