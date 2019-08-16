/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IFrockStandingBook.cs
 * 作者    :  曹津彬    版本: v1.00    日期: 2014/01/22
 * 开发平台:  Visual C# 2008
 * 用于    :  仓库管理软件
 *----------------------------------------------------------------------------
 * 描述 : 
 * 其它 :
 *----------------------------------------------------------------------------
 * 公共信息: 参见系统'类帮助文档'
 *----------------------------------------------------------------------------
 * 历史记录:
 ******************************************************************************/
using System;
using System.Data;
using System.Collections.Generic;

namespace ServerModule
{
    /// <summary>
    /// 工装台帐管理类接口
    /// </summary>
    public interface IFrockStandingBook : IBasicBillServer
    {
        void RecordFrockUseCounts_HomemadePart(DepotManagementDataContext ctx, int goodsID, int inStockCounts);

        void SaveApplicableGoods(string frockNumber, DataTable tableInfo);

        DataTable GetApplicableGoods(string frockNumber);

        /// <summary>
        /// 更新工装工位配置信息
        /// </summary>
        /// <param name="oldFrockOfWorkBench">旧实体集</param>
        /// <param name="newFrockOfWorkBench">新实体集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool UpdateFrockOfWorkBench(S_FrockOfWorkBenchSetting oldFrockOfWorkBench, S_FrockOfWorkBenchSetting newFrockOfWorkBench, out string error);
        
        /// <summary>
        /// 删除工装工位配置信息
        /// </summary>
        /// <param name="frockOfWorkBench">工装工位配置实体集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True ，失败返回False</returns>
        bool DeleteFrockOfWorkBench(S_FrockOfWorkBenchSetting frockOfWorkBench, out string error);
        
        /// <summary>
        /// 添加工装工位配置信息
        /// </summary>
        /// <param name="frockOfWorkBench">工装工位配置实体集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True ，失败返回False</returns>
        bool AddFrockOfWorkBench(S_FrockOfWorkBenchSetting frockOfWorkBench, out string error);

        /// <summary>
        /// 获得工装的工位设置信息
        /// </summary>
        /// <returns>返回Table</returns>
        DataTable GetFrockOfWorkBenchInfo();

        /// <summary>
        /// 保存周期鉴定项目
        /// </summary>
        /// <param name="frockNumber">工装编号</param>
        /// <param name="checkContentItems">字符串列表</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        bool SaveCheckItemContent(string frockNumber, List<string> checkContentItems, out string error);

        /// <summary>
        /// 获得周期鉴定项目
        /// </summary>
        /// <param name="frockNumber">工装编号</param>
        /// <returns>返回Table</returns>
        DataTable GetCheckItemsContent(string frockNumber);

        /// <summary>
        /// 改变子父关系
        /// </summary>
        /// <param name="lnqSelfFrock">自身工装信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool ChangeParentChildRelationships(S_FrockStandingBook lnqSelfFrock, out string error);

        /// <summary>
        /// 获得功能树的信息
        /// </summary>
        /// <returns>返回Table</returns>
        DataTable GetTreeInfo();

        /// <summary>
        /// 获得工装入库单信息
        /// </summary>
        /// <param name="frockNumber">工装编号</param>
        /// <returns>返回DataRow</returns>
        DataRow GetInDepotBillInfo(string frockNumber);

        /// <summary>
        /// 检测工装附属信息是否填写完整
        /// </summary>
        /// <param name="frockNumber">工装编号</param>
        /// <param name="goodsID">物品ID</param>
        /// <returns>不存在数据或者完整返回True，不完整或者数据不唯一返回False</returns>
        bool IsIntactSatelliteInformation(string frockNumber, int goodsID);

        /// <summary>
        /// 删除工装台帐表信息
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="frockNumber">工装编号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool DeleteFrockStandingBook(int goodsID, string frockNumber, out string error);

        /// <summary>
        /// 检查单据数与工装编号业务数是否一致
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="billNo">单据号</param>
        /// <param name="amount">单据的物品数量</param>
        /// <returns>true 一致 false 不一致</returns>
        bool IsOperationCountMateBillCount(int goodsID, string billNo, decimal amount);
        
        /// <summary>
        /// 获取台帐综合信息
        /// </summary>
        /// <param name="isFinalAssembly">仅显示总装标志 True 显示 False 全部</param>
        /// <returns>返回Table</returns>
        DataTable GetBookSynthesizeInfo(bool isFinalAssembly);

        /// <summary>
        /// 批量插入工装业务表
        /// </summary>
        /// <param name="billNo">业务单据号</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="frockNumberTable">单据业务的工装编码数据集</param>
        /// <param name="businessType">业务类型</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool UpdateFrockOperation(string billNo, int goodsID, DataTable frockNumberTable, GlobalObject.CE_BusinessBillType businessType, out string error);

        /// <summary>
        /// 根据单据号获得某个物品的工装编号数据集
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回一条工装台帐的数据集</returns>
        DataTable GetFrockNumberFromBillNo(string billNo, int goodsID);

        /// <summary>
        /// 更新工装台帐的库存状态
        /// </summary>
        /// <param name="ctx">LINQ数据上下文</param>
        /// <param name="frockTable">需要更新的数据表</param>
        /// <param name="isStock">是否在库</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool UpdateFrockStandingBookStock(DepotManagementDataContext ctx, DataTable frockTable, bool isStock, out string error);

        /// <summary>
        /// 根据普通入库单号删除工装台帐与工装验证报告单
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">普通入库单号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool DeleteFrockOrdinaryInDepotBill(DepotManagementDataContext ctx, string billNo, out string error);

        /// <summary>
        /// 获得新的工装编号
        /// </summary>
        /// <returns>返回工装编号</returns>
        string GetNewFrockNumber();

        /// <summary>
        /// 获得所有信息
        /// </summary>
        /// <param name="scrapFlag">报废标志 True 报废 False 未报废</param>
        /// <param name="isInStock">在库标志 True 显示 False 不显示</param>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回所有不是分装的工装信息</returns>
        DataTable GetAllTable(bool scrapFlag, bool isInStock, int goodsID);

        /// <summary>
        /// 获得所有信息
        /// </summary>
        /// <param name="scrapFlag">报废标志 True 报废 False 未报废</param>
        /// <param name="isInStock">在库标志 True 显示 False 不显示</param>
        /// <param name="isFinalAssembly">仅显示总装标志 True 显示 False 全部</param>
        /// <param name="isUsing">仅显示在用 True 显示,False 全部</param>
        /// <returns>返回所有不是分装的工装信息</returns>
        DataTable GetAllTable(bool scrapFlag, bool isInStock, bool isFinalAssembly, bool isUsing);

        /// <summary>
        /// 获得一条记录的数据集
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="frockNumber">工装编号</param>
        /// <returns>返回null表示数据不唯一,否则返回一条数据集</returns>
        S_FrockStandingBook GetBookInfo(int goodsID, string frockNumber);

        /// <summary>
        /// 获得指定工装的检验报告单
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="frockNumber">工装编号</param>
        /// <returns>返回指定工装的检验报告</returns>
        DataTable GetProvingReport(int goodsID, string frockNumber);

        /// <summary>
        /// 获得指定工装的维修信息
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="frockNumber">工装编号</param>
        /// <returns>返回指定工装的维修信息</returns>
        DataTable GetServiceTable(int goodsID, string frockNumber);

        /// <summary>
        /// 获得指定工装的分装信息
        /// </summary>
        /// <param name="goodsID">父级物品ID</param>
        /// <param name="frockNumber">父级工装编号</param>
        /// <param name="scrapFlag">报废标志</param>
        /// <returns>返回指定工装的分装信息 </returns>
        DataTable GetSplitCharging(int goodsID, string frockNumber, bool scrapFlag);

        /// <summary>
        /// 更新工装台帐信息
        /// </summary>
        /// <param name="frockStandingBook">工装台帐数据集</param>
        /// <param name="serviceTable">维修信息表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool UpdateFrockStandingBook(S_FrockStandingBook frockStandingBook, DataTable serviceTable, out string error);

        /// <summary>
        /// 获得指定工装的业务信息
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="frockNumber">工装编号</param>
        /// <returns>返回指定工装的业务信息</returns>
        DataTable GetFrockOperation(int goodsID, string frockNumber);
    }
}
