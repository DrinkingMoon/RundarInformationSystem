/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IOrdinaryInDepotBillServer.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2010/09/27
 * 开发平台:  Visual C# 2008
 * 用于    :  仓库管理软件
 *----------------------------------------------------------------------------
 * 描述 : 
 * 其它 :
 *----------------------------------------------------------------------------
 * 公共信息: 参见系统'类帮助文档'
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2010/09/27 作者: 夏石友 当前版本: V1.00
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
    /// 普通入库单单据状态
    /// </summary>
    public enum OrdinaryInDepotBillStatus
    {
        /// <summary>
        /// 新建单据
        /// </summary>
        新建单据,

        /// <summary>
        /// 等待质检
        /// </summary>
        等待质检,

        /// <summary>
        /// 等待工装验证
        /// </summary>
        等待工装验证,

        /// <summary>
        /// 等待入库
        /// </summary>
        等待入库,

        /// <summary>
        /// 等待工装验证报告
        /// </summary>
        等待工装验证报告,

        /// <summary>
        /// 已报废
        /// </summary>
        已报废,

        /// <summary>
        /// 已入库
        /// </summary>
        已入库
    }

    /// <summary>
    /// 普通入库单管理类接口
    /// </summary>
    public interface IOrdinaryInDepotBillServer : IBasicService, IBasicBillServer
    {

        /// <summary>
        /// 获取普通入库单的单据到票标志
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billID">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回普通入库单是否到票的标志</returns>
        int GetHavingInvoice(DepotManagementDataContext ctx, string billID, out string error);

        /// <summary>
        /// 获取普通入库单的单据到票标志
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回普通入库单是否到票的标志</returns>
        int GetHavingInvoice(string billID, out string error);

        /// <summary>
        /// 获取普通入库单信息
        /// </summary>
        /// <param name="billNo">单据编号</param>
        /// <returns>成功返回获取到的单据信息,失败返回null</returns>
        S_OrdinaryInDepotBill GetBill(string billNo);

        /// <summary>
        /// 获取普通入库单信息
        /// </summary>
        /// <param name="returnInfo">入库单</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool GetAllBill(out IQueryResult returnInfo, out string error);

        /// <summary>
        /// 添加普通入库单
        /// </summary>
        /// <param name="bill">单据信息</param>
        /// <param name="returnInfo">添加完毕后查询数据库的返回结果</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool AddBill(S_OrdinaryInDepotBill bill, out IQueryResult returnInfo, out string error);

        /// <summary>
        /// 采购员提交单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="status">提交后的单据状态</param>
        /// <param name="returnInfo">返回更新后重新查询的领料单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加领料单</returns>
        bool SubmitNewBill(string billNo, OrdinaryInDepotBillStatus status, out IQueryResult returnInfo, out string error);
   
        /// <summary>
        /// 提交质量信息
        /// </summary>
        /// <param name="billInfo">取单据中质量信息部分</param>
        /// <param name="returnInfo">添加完毕后查询数据库的返回结果</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作结果</returns>
        bool SubmitQualityInfo(S_OrdinaryInDepotBill billInfo, out IQueryResult returnInfo, out string error);

        /// <summary>
        /// 提交工装验证信息
        /// </summary>
        /// <param name="billInfo">取单据中工装验证信息部分</param>
        /// <param name="returnInfo">添加完毕后查询数据库的返回结果</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作结果</returns>
        bool SubmitMachineValidationInfo(S_OrdinaryInDepotBill billInfo, out IQueryResult returnInfo, out string error);

        /// <summary>
        /// 提交入库信息
        /// </summary>
        /// <param name="inDepotInfo">入库信息, 只取其中入库部分</param>
        /// <param name="returnInfo">添加完毕后查询数据库的返回结果</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool SubmitInDepotInfo(S_OrdinaryInDepotBill inDepotInfo, out IQueryResult returnInfo, out string error);

        /// <summary>
        /// 修改单据
        /// </summary>
        /// <param name="bill">单据号</param>
        /// <param name="returnInfo">修改完毕后查询数据库的返回结果</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool UpdateBill(S_OrdinaryInDepotBill bill, out IQueryResult returnInfo, out string error);

        /// <summary>
        /// 更新指定单据物品类别
        /// </summary>
        /// <param name="billNo">单据编号</param>
        /// <param name="depotType">物品类别</param>
        /// <param name="returnInfo">操作完毕后查询数据库的返回结果</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool UpdateGoodsType(string billNo, string depotType, out IQueryResult returnInfo, out string error);

        /// <summary>
        /// 删除普通入库单
        /// </summary>
        /// <param name="billNo">入库单号</param>
        /// <param name="returnInfo">普通入库单</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool DeleteBill(string billNo, out IQueryResult returnInfo, out string error);

        /// <summary>
        /// 回退单据
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <param name="billStatus">单据状态</param>
        /// <param name="returnBill">返回回退后查询到的单据信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <param name="rebackReason">回退原因</param>
        /// <returns>回退成功返回True，回退失败返回False</returns>
        bool ReturnBill(string djh, string billStatus,
            out IQueryResult returnBill, out string error, string rebackReason);
    }
}
