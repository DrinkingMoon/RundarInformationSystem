/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IStoreServer.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2010/07/17
 * 开发平台:  Visual C# 2008
 * 用于    :  仓库管理软件
 *----------------------------------------------------------------------------
 * 描述 : 
 * 其它 :
 *----------------------------------------------------------------------------
 * 公共信息: 参见系统'类帮助文档'
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2010/07/17 8:54:12 作者: 夏石友 当前版本: V1.00
 *        修改说明: 创建
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using GlobalObject;

namespace ServerModule
{
    /// <summary>
    /// 仓库查找条件
    /// </summary>
    public struct StoreQueryCondition
    {
        /// <summary>
        /// GoodsID
        /// </summary>
        public int GoodsID
        {
            get;
            set;
        }

        /// <summary>
        /// StorageID
        /// </summary>
        public string StorageID
        {
            get;
            set;
        }
        
        /// <summary>
        /// GoodsCode
        /// </summary>
        public string GoodsCode
        {
            get;
            set;
        }

        /// <summary>
        /// GoodsName
        /// </summary>
        public string GoodsName
        {
            get;
            set;
        }

        /// <summary>
        /// Spec
        /// </summary>
        public string Spec
        {
            get;
            set;
        }

        /// <summary>
        /// Provider
        /// </summary>
        public string Provider
        {
            get;
            set;
        }

        /// <summary>
        /// BatchNo
        /// </summary>
        public string BatchNo
        {
            get;
            set;
        }

        /// <summary>
        /// Depot
        /// </summary>
        public string Depot
        {
            get;
            set;
        }

        /// <summary>
        /// ShelfArea
        /// </summary>
        public string ShelfArea
        {
            get;
            set;
        }

        /// <summary>
        /// ColumnNumber
        /// </summary>
        public string ColumnNumber
        {
            get;
            set;
        }

        /// <summary>
        /// LayerNumber
        /// </summary>
        public string LayerNumber
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 库存管理类接口
    /// </summary>
    public interface IStoreServer : IBasicService
    {
        /// <summary>
        /// 操作MES系统车间在产
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">单据号</param>
        void Operation_MES_InProduction(DepotManagementDataContext ctx, string billNo);

        /// <summary>
        /// 获得物品单价
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>返回单价</returns>
        decimal GetGoodsUnitPrice(DepotManagementDataContext ctx, int goodsID, string batchNo, string storageID);

        /// <summary>
        /// 出库业务库房操作
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="stockInfo">库存信息对象</param>
        /// <param name="operationType">业务类型</param>
        void OutStore(DepotManagementDataContext dataContext, S_Stock stockInfo, CE_SubsidiaryOperationType operationType);

        /// <summary>
        /// 入库业务库房操作
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="stockInfo">库存信息对象</param>
        /// <param name="operationType">业务类型</param>
        void InStore(DepotManagementDataContext dataContext, S_Stock stockInfo, CE_SubsidiaryOperationType operationType);

        /// <summary>
        /// 单独更改账龄
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="stockInfo">库存信息</param>
        /// <param name="autoSubmitToDatabase">是否直接提交数据库</param>
        void UpdateAging(DepotManagementDataContext ctx, S_Stock stockInfo, bool autoSubmitToDatabase);

        /// <summary>
        /// 获得某物品的当前所有库存
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回库存数量</returns>
        decimal GetGoodsStock(int goodsID);

        /// <summary>
        /// 获取非零库存信息
        /// </summary>
        /// <param name="returnStock">库存信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取库存信息</returns>
        bool GetNoZeroStore(out IQueryable<View_S_Stock> returnStock, out string error);

        /// <summary>
        /// 获得物品库存
        /// </summary>
        /// <param name="goodsID">物品ID 若为0则表示所有</param>
        /// <param name="batchNo">批次号 若为"所有"则表示所有</param>
        /// <param name="provider">供应商编码 若为""则表示所有</param>
        /// <param name="storageID">库房ID 若为""则表示所有</param>
        /// <returns>返回物品库存数</returns>
        View_S_Stock GetGoodsStockInfoView(int goodsID, string batchNo, string provider, string storageID);

        /// <summary>
        /// 获取物品信息
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <returns>返回所查询到的库存视图信息</returns>
        View_S_Stock GetGoodsStockInfo(int goodsID, string batchNo);

        /// <summary>
        /// 操作营销售后已返修待返修库存数量
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="afterServiceStock">LINQ数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool OperationYXAfterService(DepotManagementDataContext dataContext, YX_AfterServiceStock afterServiceStock, out string error);

        /// <summary>
        /// 查找指定的库存信息
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="queryInfo">查询条件</param>
        /// <returns>返回查找到的库存信息</returns>
        S_Stock GetStockInfoOverLoad(DepotManagementDataContext context, StoreQueryCondition queryInfo);

        /// <summary>
        /// 判断物品ID 的批次号是否存在
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <returns>存在返回True，否则返回False</returns>
        bool IsBatchNoOfGoodsExist(int goodsID, string batchNo);
        
        /// <summary>
        /// 检测某物品是否存在于某库房
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>存在返回True，不存在返回False</returns>
        bool IsGoodsInStock(int goodsID, string storageID);

        /// <summary>
        /// 获得某一个物品的库存汇总
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>库存总数量</returns>
        decimal GetGoodsSumCount(int goodsID, string storageID);

        /// <summary>
        /// 获取库存平均价
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <returns>返回物品的平均价</returns>
        decimal GetGoodsAveragePrice(int goodsID, string batchNo);

        /// <summary>
        /// 获得库存物品信息(不包括库存为0)
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="stroageID">库房ID</param>
        /// <param name="flag">显示方式</param>
        /// <returns>返回Table</returns>
        DataTable GetGoodsStockInfo(int goodsID, string stroageID, int flag);

        /// <summary>
        /// 获得物品库存
        /// </summary>
        /// <param name="goodsID">物品ID 若为0则表示所有</param>
        /// <param name="batchNo">批次号 若为"所有"则表示所有</param>
        /// <param name="provider">供应商编码 若为""则表示所有</param>
        /// <param name="storageID">库房ID 若为""则表示所有</param>
        /// <returns>返回物品库存数</returns>
        DataTable GetGoodsStockInfo(int goodsID, string batchNo, string provider, string storageID);

        /// <summary>
        /// 获得物品库存
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <param name="provider">供应商编码</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>返回物品库存数量</returns>
        string GetStockCount(int goodsID, string batchNo, string provider, string storageID);

        /// <summary>
        /// 由隔离单更改库存状态
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="islation">隔离单单据信息</param>
        /// <param name="status">库存状态</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>变更成功返回True，变更失败返回False</returns>
        bool ChangeStockStatus(DepotManagementDataContext context,
            S_IsolationManageBill islation, int status,
            out string error);

        /// <summary>
        /// 由隔离单改变库存数
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="islation">隔离单单据信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>改变成功返回True，改变失败返回False</returns>
        bool ChangeStockCount(DepotManagementDataContext context,
            S_IsolationManageBill islation, out string error);

        /// <summary>
        /// 获取物品信息
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>返回所查询到的库存视图信息</returns>
        DataRow GetGoodsStockInfo(int goodsID, string batchNo,
            string storageID);

        /// <summary>
        /// 克隆库存对象
        /// </summary>
        /// <param name="stockInfo">要克隆的对象</param>
        /// <returns>克隆后的新对象</returns>
        S_Stock Clone(S_Stock stockInfo);

        /// <summary>
        /// 获取库存物品状态表信息
        /// </summary>
        /// <returns>获取到的信息</returns>
        IQueryable<S_StockStatus> GetStoreStatus();
 
        /// <summary>
        /// 查找指定的非唯一性库存信息(批次号使用包含符)
        /// </summary>
        /// <param name="queryInfo">查询条件</param>
        /// <returns>返回查找到的库存信息</returns>
        S_Stock GetStockInfo(StoreQueryCondition queryInfo);

        /// <summary>
        /// 获取库存中指定物品的供应商信息
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <returns>成功返回获取到的供应商信息列表, 失败返回null</returns>
        DataTable GetProviderInfo(int goodsID);

        /// <summary>
        /// 获取实际单价
        /// </summary>
        /// <param name="GoodsID">物品ID</param>
        /// <param name="provider">供应商</param>
        /// <param name="batchNo">批次号</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>返回获取到的实际单价</returns>
        decimal GetFactUnitPrice(int GoodsID, string provider, string batchNo,string storageID);

        /// <summary>
        /// 获取某货物的所有库存信息
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>返回获取到的货物库存信息</returns>
        IQueryable<View_S_Stock> GetGoodsStore(string goodsCode, string goodsName, string storageID);

        /// <summary>
        /// 获取某货物的所有库存信息
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>返回获取到的货物库存信息</returns>
        IQueryable<View_S_Stock> GetGoodsStore(string goodsCode, string goodsName, string spec, string storageID);

        /// <summary>
        /// 根据物品ID获得某物品的所有库存信息
        /// </summary>
        /// <param name="intGoodsID"></param>
        /// <returns></returns>
        View_S_Stock GetGoodsStore(int intGoodsID);

        /// <summary>
        /// 获取某图号型号的所有库存信息
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="spec">规格</param>
        /// <param name="returnStock">库存信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取某图号型号的所有库存信息</returns>
        bool GetSomeGoodsCodeStore(string goodsCode, string spec, out DataTable returnStock, out string error);

        /// <summary>
        /// 获取库存信息
        /// </summary>
        /// <param name="findCondition">查找条件</param>
        /// <param name="sequence">排序,True为升序,False为降序</param>
        /// <param name="returnStock">库存信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取库存信息</returns>
        bool GetAllStore(string findCondition, bool sequence, out IQueryable<View_S_Stock> returnStock, out string error);

        /// <summary>
        /// 获取所有仓库零件信息（供点击“图型/图号”按钮时进行查询用）
        /// </summary>
        /// <param name="depotType">仓库类型，取值为：零部件、产品、其他</param>
        /// <param name="groupbyBatchNo">是否要用批次分组</param>
        /// <param name="returnPartInfo">返回获取到的零件信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取信息</returns>
        bool GetAllStorePartInfo(string depotType, bool groupbyBatchNo, out DataTable returnPartInfo, out string error);
        
        /// <summary>
        /// 获取指定订单物品库存信息
        /// </summary>
        /// <param name="orderFormNumber">订单号</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>返回获取到的信息</returns>
        IQueryable<View_OrderFormGoodsStockInfo> GetOrderFormGoodsStockInfo(string orderFormNumber, string storageID);

        /// <summary>
        /// 获得物品库存
        /// </summary>
        /// <param name="goodsID">物品ID 若为0则表示所有</param>
        /// <param name="batchNo">批次号 若为"所有"则表示所有</param>
        /// <param name="provider">供应商编码 若为""则表示所有</param>
        /// <param name="storageID">库房ID 若为""则表示所有</param>
        /// <returns>返回物品库存数</returns>
        View_S_Stock Test(int goodsID, string batchNo, string provider, string storageID);
      
        /// <summary>
        /// 删除库存记录
        /// </summary>
        /// <param name="id">要删除的记录ID</param>
        /// <param name="error">返回的错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        bool DeleteStore(int id, out string error);

        ///// <summary>
        ///// 库存信息处理
        ///// </summary>
        ///// <param name="dataContext">数据上下文</param>
        ///// <param name="autoSubmitToDatabase">是否自动将结果提交到数据库的标志</param>
        ///// <param name="stock">进货信息</param>
        ///// <param name="error">返回的错误信息</param>
        ///// <returns>操作是否成功的标志</returns>
        //bool StoreInfoOperation(DepotManagementDataContext dataContext, bool autoSubmitToDatabase, S_Stock stock, out string error);

        ///// <summary>
        ///// 仓库领料
        ///// </summary>
        ///// <param name="context">数据上下文</param>
        ///// <param name="stockInfo">要更新的库存记录(用于库存定位)</param>
        ///// <param name="amount">更新的数量</param>
        ///// <param name="error">出错时返回错误信息，无错时返回null</param>
        ///// <returns>操作是否成功的标志</returns>
        //bool TakingMaterials(DepotManagementDataContext context, S_Stock stockInfo, decimal amount, out string error);

        /// <summary>
        /// 更新库存信息
        /// </summary>
        /// <param name="stockInfo">库存信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功的标志</returns>
        bool UpdateStore(S_Stock stockInfo, out string error);

        /// <summary>
        /// 查询全部货物库存
        /// </summary>
        /// <param name="table">查询到的库存信息表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool GetAllStore(out DataTable table, out string error);

        /// <summary>
        /// 获取用户管辖仓库的信息
        /// </summary>
        /// <param name="WorkCode">工号</param>
        /// <returns>返回获取到的仓库记录</returns>
        IQueryable<View_S_DepotForPersonnel> GetDepotForPersonnel(string WorkCode);

        /// <summary>
        /// 获取某货物的所有库存信息
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>返回获取到的货物库存信息</returns>
        IQueryable<View_S_Stock> GetGoodsStoreNorml(string goodsCode, 
            string goodsName, string spec,string storageID);
        
        /// <summary>
        /// 获取某货物的仅仅针对于三包外领料的物品信息
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>返回获取到的货物库存信息</returns>
        IQueryable<View_S_Stock> GetGoodsStoreOnlyForSBW(string goodsCode,
            string goodsName, string spec, string storageID);
        
        /// <summary>
        /// 获取某货物的仅仅针对于整台份领料的物品信息
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>返回获取到的货物库存信息</returns>
        IQueryable<View_S_Stock> GetGoodsStoreOnlyForAssembly(int goodsID, string storageID);

        /// <summary>
        /// 获取某货物的仅仅针对于整台份领料的物品信息
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>返回获取到的货物库存信息</returns>
        IQueryable<View_S_Stock> GetGoodsStoreOnlyForAssembly(string goodsCode,
            string goodsName, string spec, string storageID);

        /// <summary>
        /// 获取某货物的仅仅针对于整台份领料的混装物品信息
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>返回获取到的货物库存信息</returns>
        IQueryable<View_S_Stock> GetGoodsStoreOnlyForJumbly(string goodsCode,
            string goodsName, string spec, string storageID);

        /// <summary>
        /// 获取某货物的仅限于返修箱用的信息
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>返回获取到的货物库存信息</returns>
        IQueryable<View_S_Stock> GetGoodsStoreOnlyForRepair(string goodsCode,
            string goodsName, string spec, string storageID);

        /// <summary>
        /// 获取某货物的仅限于售后备件的信息
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>返回获取到的货物库存信息</returns>
        IQueryable<View_S_Stock> GetGoodsStoreOnlyForAttachment(string goodsCode,
            string goodsName, string spec, string storageID);

        /// <summary>
        /// 获取某货物的样品的信息
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>返回获取到的货物库存信息</returns>
        IQueryable<View_S_Stock> GetGoodsStoreMuster(string goodsCode,
            string goodsName, string spec, string storageID);

        /// <summary>
        /// 获得库存物品实际平均价
        /// </summary>
        /// <param name="flag">True为View_S_Stock； False为View_S_InOutSaveStock </param>
        /// <param name="yearAndMonth">年月字符串</param>
        /// <returns>返回获取的库存物品平均价信息</returns>
        DataTable GetStockAveragePrice(bool flag, string yearAndMonth);

        /// <summary>
        /// 更改库存单价
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="price">金额</param>
        /// <param name="flag">True为View_S_Stock； False为View_S_InOutSaveStock</param>
        /// <param name="yearAndMonth">年月字符串</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>更改成功返回True，更改失败返回False</returns>
        bool ChangeAveragePrice(int goodsID, decimal price,
            bool flag, string yearAndMonth, out string error);

        /// <summary>
        /// 获得物品单价
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>返回单价</returns>
        decimal GetGoodsUnitPrice(int goodsID, string batchNo, string storageID);
    }
}
