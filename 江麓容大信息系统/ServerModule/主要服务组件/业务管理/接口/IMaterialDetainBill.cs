/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IMaterialDetainBill.cs
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
    /// 扣货单管理类接口
    /// </summary>
    public interface IMaterialDetainBill:IBasicService, IBasicBillServer
    {

        /// <summary>
        /// 获得单条记录信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回LINQ单条信息</returns>
        S_MaterialDetainBill GetBill(string billNo);

        /// <summary>
        /// 回退单据
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <param name="billStatus">单据状态</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <param name="rebackReason">回退原因</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool ReturnBill(string djh, string billStatus, out string error, string rebackReason);

        /// <summary>
        /// 获得全部单据信息
        /// </summary>
        /// <returns>返回扣货单表信息</returns>
        DataTable GetAllBill();

        /// <summary>
        /// 添加扣货单
        /// </summary>
        /// <param name="bill">扣货单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回True，否则返回False</returns>
        bool AddBill(S_MaterialDetainBill bill, out string error);

        /// <summary>
        /// 编制人修改扣货单
        /// </summary>
        /// <param name="bill">扣货单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回True，否则返回False</returns>
        bool UpdateBill(S_MaterialDetainBill bill, out string error);

        /// <summary>
        /// 编制人修改子表信息
        /// </summary>
        /// <param name="goods">扣货单明细信息</param>
        /// <param name="storageID">库房ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>修改成功返回True，修改失败返回False</returns>
        bool UpdateList(S_MaterialDetainList goods, string storageID, out string error);

        /// <summary>
        /// 删除子表信息
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        bool DeleteList(string djh, out string error);

        /// <summary>
        /// 删除物品信息
        /// </summary>
        /// <param name="id">要删除的物品ID列表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功的标志</returns>
        bool DeleteGoods(List<long> id, out string error);

        /// <summary>
        /// 删除主表信息
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，否则返回False</returns>
        bool DeleteBill(string billID, out string error);
        
        /// <summary>
        /// SQE确认，修改主表的单据状态
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回True，否则返回False</returns>
        bool SQEConfirmBill(string billNo, out string error);

        /// <summary>
        /// 领导审核，修改主表的单据状态
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回True，否则返回False</returns>
        bool AuditingBill(string billNo, out string error);

        /// <summary>
        /// 质管批准，修改主表的单据状态
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="name">质管人编号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回True，否则返回False</returns>
        bool AuthorizeBill(string billNo, string name, out string error);

        /// <summary>
        /// 采购确认，修改主表的单据状态
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="name">采购确认人编号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回True，否则返回False</returns>
        bool ConfirmBill(string billNo, string name, out string error);

        /// <summary>
        /// 添加子表信息
        /// </summary>
        /// <param name="list">子表数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回True，否则返回False</returns>
        bool AddList(S_MaterialDetainList list, out string error);

        /// <summary>
        /// 获得子表信息
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回满足条件的数据集</returns>
        DataTable GetList(string billID, out string error);

        /// <summary>
        /// 通过物品的ID获得订单信息
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <param name="provider">供货单位</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回满足条件的数据集</returns>
        DataTable GetOrderFormInfo(string goodsID, string batchNo, string provider, out string error);

        /// <summary>
        /// 采购员选择订单号，修改子表信息
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="orderFormID">关联订单号</param>
        /// <param name="batchNo">批次号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回True，否则返回False</returns>
        bool UpdateList(string billID, string goodsID, string orderFormID, string batchNo, out string error);
    }
}
