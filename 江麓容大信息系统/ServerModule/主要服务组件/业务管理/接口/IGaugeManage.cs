using System;
using System.Data;
using GlobalObject;
namespace ServerModule
{
    /// <summary>
    /// 
    /// </summary>
    public interface IGaugeManage
    {
        /// <summary>
        /// 获取单个量检具信息对象
        /// </summary>
        /// <param name="code">量检具编号</param>
        /// <returns>返回单条对象信息</returns>
        S_GaugeStandingBook GetSingleInfo(string code);

        /// <summary>
        /// 获取文件列表
        /// </summary>
        /// <param name="gaugeCode">量检具编号</param>
        /// <returns>返回Table</returns>
        DataTable GetTable_FilesInfo(string gaugeCode);

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="fileInfo">文件对象</param>
        void UpLoadFileInfo(Bus_Gauge_Files fileInfo);

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="keyValue">对象唯一编码</param>
        void DeleteFileInfo(string keyValue);

        /// <summary>
        /// 删除量检具
        /// </summary>
        /// <param name="gaugeCoding">量检具编号</param>
        void DeleteInfo(string gaugeCoding);

        /// <summary>
        /// 根据单据号获得某个物品的量检具编号数据集
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回一条工装台帐的数据集</returns>
        DataTable GetGaugeCodingFromBillNo(string billNo, int goodsID);

        /// <summary>
        /// 获得量检具台帐信息
        /// </summary>
        /// <returns>返回量检具台帐信息</returns>
        DataTable GetGaugeAllInfo(bool lyFlag, bool bfFlag);

        /// <summary>
        /// 操作量检具台账
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">单据号</param>
        /// <param name="type">业务类型</param>
        /// <param name="operationType">操作类型编号</param>
        void OperationGaugeStandingBook(DepotManagementDataContext ctx, string billNo, CE_MarketingType type, int operationType);

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="gaugeStandingBook">量检具台账单条对象</param>
        /// <param name="mode">操作类型</param>
        void SaveInfo(S_GaugeStandingBook gaugeStandingBook, CE_OperatorMode mode);

        /// <summary>
        /// 批量插入量检具业务表
        /// </summary>
        /// <param name="billNo">业务单据号</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="gaugeCodingTable">单据业务的量检具编号数据集</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        void UpdateGaugeOperation(string billNo, int goodsID, DataTable gaugeCodingTable);
    }
}
