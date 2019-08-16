/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IMaterialRejectBill.cs
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
    /// 采购退货单单据状态
    /// </summary>
    public enum MaterialRejectBillBillStatus
    {
        /// <summary>
        /// 新建单据
        /// </summary>
        新建单据,

        /// <summary>
        /// 等待上级领导审核
        /// </summary>
        等待上级领导审核,

        /// <summary>
        /// 等待财务审核
        /// </summary>
        等待财务审核,

        /// <summary>
        /// 等待仓管退货
        /// </summary>
        等待仓管退货,

        /// <summary>
        /// 已完成
        /// </summary>
        已完成
    }

    /// <summary>
    /// 采购退货单管理类接口
    /// </summary>
    public interface IMaterialRejectBill : IBasicService, IBasicBillServer
    {
        /// <summary>
        /// 根据单据信息操作账务信息与库存信息
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="bill">单据信息</param>
        void OpertaionDetailAndStock(DepotManagementDataContext dataContext, S_MaterialRejectBill bill);

        /// <summary>
        /// 获取采购退货单的单据到票标志
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billID">采购退货单号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回采购退货单是否到票标志</returns>
        int SetHavingInvoiceReturn(DepotManagementDataContext ctx, string billID, out string error);

        /// <summary>
        /// 上级领导审核
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="returnBill">返回结果集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        bool AuditBill(string billNo, out IQueryResult returnBill, out string error);

        /// <summary>
        /// 获取采购退货单的单据到票标志
        /// </summary>
        /// <param name="billID">采购退货单号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回采购退货单是否到票标志</returns>
        int SetHavingInvoiceReturn(string billID, out string error);

        /// <summary>
        /// 获取所有采购退货单信息
        /// </summary>
        /// <param name="returnBill">返回的单据信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取领料出库信息</returns>
        bool GetAllBill(out IQueryResult returnBill, out string error);

        /// <summary>
        /// 获取采购退货单视图信息
        /// </summary>
        /// <param name="billNo">采购退货单号</param>
        /// <returns>成功返回获取领料出库信息, 失败返回null</returns>
        View_S_MaterialRejectBill GetBillView(string billNo);

        /// <summary>
        /// 添加采购退货单
        /// </summary>
        /// <param name="bill">单据信息</param>
        /// <param name="returnBill">返回更新后的采购退货单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加采购退货单</returns>
        bool AddBill(S_MaterialRejectBill bill, out IQueryResult returnBill, out string error);
 
        /// <summary>
        /// 删除采购退货单
        /// </summary>
        /// <param name="billNo">采购退货单号</param>
        /// <param name="returnBill">返回更新后重新查询的采购退货单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功删除采购退货单号</returns>
        bool DeleteBill(string billNo, out IQueryResult returnBill, out string error);

        /// <summary>
        /// 修改采购退货单(只修改编制人涉及信息)
        /// </summary>
        /// <param name="bill">单据信息</param>
        /// <param name="returnBill">返回更新后的采购退货单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加采购退货单</returns>
        bool UpdateBill(S_MaterialRejectBill bill, out IQueryResult returnBill, out string error);

        /// <summary>
        /// 退库人提交单据(交给财务审批)
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="returnBill">返回更新后重新查询的采购退货单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加采购退货单</returns>
        bool SubmitNewBill(string billNo, out IQueryResult returnBill, out string error);
        
        /// <summary>
        /// 财务审批单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="name">审批人姓名</param>
        /// <param name="returnBill">返回更新后重新查询的采购退货单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加采购退货单</returns>
        bool FinanceAuthorizeBill(string billNo, string name, out IQueryResult returnBill, out string error);

        /// <summary>
        /// 完成采购退货单
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="storeManager">仓库管理员</param>
        /// <param name="returnBill">返回更新后重新查询的采购退货单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加采购退货单</returns>
        bool FinishBill(string billNo, string storeManager, out IQueryResult returnBill, out string error);

        /// <summary>
        /// 回退单据
        /// </summary>
        /// <param name="djh">回退单据号</param>
        /// <param name="billStatus">回退单据状态</param>
        /// <param name="returnBill">返回的查询结果集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <param name="rebackReason">回退原因</param>
        /// <returns>回退成功返回True，回退失败返回False</returns>
        bool ReturnBill(string djh, string billStatus,
            out IQueryResult returnBill, out string error, string rebackReason);
    }
}
