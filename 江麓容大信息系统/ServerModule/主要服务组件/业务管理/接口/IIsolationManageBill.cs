/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IIsolationManageBill.cs
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
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using DBOperate;
using PlatformManagement;
using GlobalObject;

namespace ServerModule
{
    /// <summary>
    /// 不合格品隔离单据管理类接口
    /// </summary>
    public interface IIsolationManageBill : IBasicService, IBasicBillServer
    {
        /// <summary>
        /// 获取全部单据信息
        /// </summary>
        /// <param name="strSelect">选择信息</param>
        /// <returns>返回隔离单单据信息</returns>
        List<View_S_IsolationManageBill> GetAllBill(string strSelect);

        /// <summary>
        /// 清除隔离单数据
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="billNo">关联的采购退货单单号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>清除成功返回True，清除失败返回False</returns>
        bool ClearBillDate(DepotManagementDataContext context, string billNo, out string error);

        /// <summary>
        /// 提交单据（可重复完成）
        /// </summary>
        /// <param name="isolation">不合格品隔离处置单信息</param>
        /// <param name="flag">标志 True 等待隔离原因 False 等待主管审核</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool UpdateBill(S_IsolationManageBill isolation, bool flag, out string error);

        /// <summary>
        /// 提交信息至数据库，按流程更新单据状态
        /// </summary>
        /// <param name="needBillStatus">要求的单据状态</param>
        /// <param name="isolation">单据信息</param>
        /// <param name="error">出现错误时返回的错误信息，没有错误返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool UpdateBill(string needBillStatus, S_IsolationManageBill isolation, out string error);

        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        bool ScrapBill(string billNo, out string error);

        /// <summary>
        /// 回退单据
        /// </summary>
        /// <param name="djh">隔离单单号</param>
        /// <param name="billStatus">回退后的单据状态</param>
        /// <param name="isolation">隔离单单据信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <param name="rebackReason">回退原因</param>
        /// <returns>回退成功返回True，回退失败返回False</returns>
        bool ReturnBill(string djh, string billStatus, S_IsolationManageBill isolation, out string error, string rebackReason);

        /// <summary>
        /// 获得隔离单的关联单据号
        /// </summary>
        /// <param name="billID">隔离单号</param>
        /// <returns>返回获得的关联单据号</returns>
        string GetAssociateBillID(string billID);

        /// <summary>
        /// 将隔离品调入库房，确认解除隔离
        /// </summary>
        /// <param name="billID">隔离单号</param>
        /// <param name="message">否认说明</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool AffrimBill(string billID, string message, out string error);

        /// <summary>
        /// 清除单个单据(隔离单)
        /// </summary>
        /// <param name="context">上下文数据集</param>
        /// <param name="billNo">单据号</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">物品批次</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool ClearGoodsDate(DepotManagementDataContext context,
            string billNo, int goodsID, string batchNo, out string error);

        /// <summary>
        /// 更改关联单据号
        /// </summary>
        /// <param name="context">上下文数据集</param>
        /// <param name="billNo">单据号</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool UpdateAssicotaeBillID(DepotManagementDataContext context,
            string billNo, int goodsID, string batchNo, out string error);
    }
}
