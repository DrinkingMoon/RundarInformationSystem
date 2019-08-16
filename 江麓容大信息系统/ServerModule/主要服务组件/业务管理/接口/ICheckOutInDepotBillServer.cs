/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  ICheckOutInDepotBillServer.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2010/09/17
 * 开发平台:  Visual C# 2008
 * 用于    :  仓库管理软件
 *----------------------------------------------------------------------------
 * 描述 : 
 * 其它 :
 *----------------------------------------------------------------------------
 * 公共信息: 参见系统'类帮助文档'
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2010/09/17 8:54:12 作者: 夏石友 当前版本: V1.00
 *        修改说明: 创建
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using PlatformManagement;

namespace ServerModule
{
    /// <summary>
    /// 报检入库单单据状态
    /// </summary>
    public enum CheckInDepotBillStatus
    {
        /// <summary>
        /// 新建单据
        /// </summary>
        新建单据,

        /// <summary>
        /// 等待确认到货数
        /// </summary>
        等待确认到货数,

        /// <summary>
        /// 等待质检机检验
        /// </summary>
        等待质检机检验,

        /// <summary>
        /// 等待质检电检验
        /// </summary>
        等待质检电检验,

        /// <summary>
        /// 等待入库
        /// </summary>
        等待入库,

        /// <summary>
        /// 等待挑返
        /// </summary>
        等待挑返,

        /// <summary>
        /// 已入库
        /// </summary>
        已入库,

        /// <summary>
        /// 已报废
        /// </summary>
        已报废,

        /// <summary>
        /// 撤消
        /// </summary>
        撤消,

        /// <summary>
        /// 回退_采购单据有误
        /// </summary>
        回退_采购单据有误,

        /// <summary>
        /// 回退_确认到货有误
        /// </summary>
        回退_确认到货有误,

        /// <summary>
        /// 回退_质检电信息有误
        /// </summary>
        回退_质检电信息有误,

        /// <summary>
        /// 回退_质检机信息有误
        /// </summary>
        回退_质检机信息有误,
    }

    /// <summary>
    /// 报检入库单关联单据类别
    /// </summary>
    public enum CheckInDepotAssociatedBillType
    {
        /// <summary>
        /// 订单
        /// </summary>
        订单,
        /// <summary>
        /// 合同
        /// </summary>
        合同
    }

    /// <summary>
    /// 报检入库单管理类接口
    /// </summary>
    public interface ICheckOutInDepotServer : IBasicService
    {
        /// <summary>
        /// 有检测废的物品直接生成领料单
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="inDepotInfo">报检单信息</param>
        /// <param name="mrBillNo">分配的领料单单号</param>
        /// <param name="error">出错时返回错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        bool InsertIntoMaterialRequisition(DepotManagementDataContext ctx, S_CheckOutInDepotBill inDepotInfo,
            out string mrBillNo, out string error);

        /// <summary>
        /// 根据单据信息操作账务信息与库存信息
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="bill">单据信息</param>
        void OpertaionDetailAndStock(DepotManagementDataContext dataContext, S_CheckOutInDepotBill bill);

        /// <summary>
        /// 提交检验报告
        /// </summary>
        /// <param name="qualityInfo">质量信息, 只取其中质量部分</param>
        /// <param name="error">出错时返回错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        bool SubmitReportInfo(S_CheckOutInDepotBill qualityInfo, out string error);

        /// <summary>
        /// 获取报检单视图信息
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <returns>返回一条报检单的单据信息</returns>
        View_S_CheckOutInDepotBill GetBill(string djh);

        /// <summary>
        /// 由物品ID，批次号获得报检入库单据号
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <returns>返回单据号</returns>
        string GetBillNo(int goodsID, string batchNo);

        /// <summary>
        /// 插入WEB 数据库
        /// </summary>
        /// <param name="bill">单据信息</param>
        /// <param name="error">出错时返回错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        bool InsertWebData(S_CheckOutInDepotBill bill, out string error);

        /// <summary>
        /// 获取新的入库单号
        /// </summary>
        /// <param name="checkOutGoodsType">报检物品类别</param>
        /// <returns>返回获取到的新的入库单号</returns>
        string GetNextBillNo(int checkOutGoodsType);

        /// <summary>
        /// 获取报检入库单信息
        /// </summary>
        /// <returns>返回是否成功获取库存信息</returns>
        IQueryResult GetAllBill();

        /// <summary>
        /// 获取指定物品的单据信息
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <returns>返回获取到的单据信息</returns>
        IQueryable<View_S_CheckOutInDepotBill> GetBill(string goodsCode, string goodsName, string spec);

        /// <summary>
        /// 检查报检入库单中是否存在此物品相关信息
        /// </summary>
        /// <param name="id">物品ID</param>
        /// <returns>存在返回true, 不存在返回false</returns>
        bool IsExist(int id);

        /// <summary>
        /// 添加报检入库单
        /// </summary>
        /// <param name="bill">单据信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作结果</returns>
        bool AddBill(S_CheckOutInDepotBill bill, out string error);

        /// <summary>
        /// 采购员更新报检入库单
        /// </summary>
        /// <param name="updateBill">单据信息</param>
        /// <param name="error">出错时返回错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        bool UpdateBill(S_CheckOutInDepotBill updateBill, out string error);

        /// <summary>
        /// 确认到货数
        /// </summary>
        /// <param name="billID">单据编号</param>
        /// <param name="confirmAmountSignatory">仓库收货员签名</param>
        /// <param name="goodsAmount">货物数量</param>
        /// <param name="billStatusInfo">单据状态消息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作结果</returns>
        bool AffirmGoodsAmount(string billID, string confirmAmountSignatory, int goodsAmount, string billStatusInfo, out string error);

        /// <summary>
        /// 提交质量信息
        /// </summary>
        /// <param name="qualityInfo">质量信息, 只取其中质量部分</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作结果</returns>
        bool SubmitQualityInfo(S_CheckOutInDepotBill qualityInfo, out string error);

        /// <summary>
        /// 提交入库信息
        /// </summary>
        /// <param name="billID">单据编号</param>
        /// <param name="inDepotInfo">入库信息, 只取其中入库部分</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作结果</returns>
        bool SubmitInDepotInfo(string billID, S_CheckOutInDepotBill inDepotInfo, out string error);

        /// <summary>
        /// 删除报检入库单
        /// </summary>
        /// <param name="billNo">入库单号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功删除报检入库单号</returns>
        bool DeleteBill(string billNo, out string error);

        /// <summary>
        /// 逐级回退单据
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="rebackReason">回退原因</param>
        /// <param name="statusFlag">状态标志 0：回退_质检电信息有误；1：回退_质检机信息有误</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool RebackBill(string billID, string rebackReason, int statusFlag, out string error);

        /// <summary>
        /// 报废单据
        /// </summary>
        /// <param name="billID">要报废的单据编号</param>
        /// <param name="error">出错时返回错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        bool ScrapBill(string billID, out string error);

        /// <summary>
        /// 更新退货建议数据
        /// </summary>
        /// <param name="djh">报检单单号</param>
        /// <param name="rejectMode">退货建议</param>
        /// <param name="error">出错时返回错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        bool UpdateRejectMode(string djh, string rejectMode, out string error);

        /// <summary>
        /// 获得退货建议方式
        /// </summary>
        /// <param name="djh">报检单单号</param>
        /// <returns>成功则返回获取到的退货建议，失败返回空串</returns>
        string GetRejectMode(string djh);
    }
}
