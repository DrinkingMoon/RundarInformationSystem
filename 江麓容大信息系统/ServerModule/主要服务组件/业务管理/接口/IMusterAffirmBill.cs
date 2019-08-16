/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IMusterAffirmBill.cs
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
    /// 样品管理类接口
    /// </summary>
    public interface IMusterAffirmBill : IBasicBillServer
    {
        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="inMuster">样品单信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>变更成功返回True，变更失败返回False</returns>
        bool SaveInfo(S_MusterAffirmBill inMuster, out string error);

        /// <summary>
        /// 获得全部单据信息
        /// </summary>
        /// <returns>返回查询到的单据信息</returns>
        DataTable GetAllBill();

        /// <summary>
        /// 获得一条单据的全部信息
        /// </summary>
        /// <param name="djh">样品单单号</param>
        /// <returns>返回一条样品单记录</returns>
        S_MusterAffirmBill GetBill(string djh);

        /// <summary>
        /// 更改单据状态
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>变更成功返回True，变更失败返回False</returns>
        bool UpdateBill(string billNo, out string error);

        /// <summary>
        /// 获得样品库中的数量
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <returns>返回样品库的数量</returns>
        decimal GetMusterStockCount(int goodsID, string batchNo);

        /// <summary>
        /// 报废单据
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>报废成功返回True，报废失败返回False</returns>
        bool ScarpBill(string djh, out string error);

        /// <summary>
        /// 回退单据
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <param name="billStatus">回退后的单据状态</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <param name="rebackReason">回退原因</param>
        /// <returns>回退成功返回True，回退失败返回False</returns>
        bool ReturnBill(string djh, string billStatus, out string error, string rebackReason);

        /// <summary>
        /// 获得样品库库存表
        /// </summary>
        /// <param name="flag">是否显示库存为0 的物品(True 显示，False 不显示)</param>
        /// <returns>返回样品库库存表</returns>
        DataTable GetAllMusterStock(bool flag);

        /// <summary>
        /// 获得耗用数
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <returns>返货获得的耗用数</returns>
        decimal GetUseCount(int goodsID, string batchNo);

        /// <summary>
        /// 获得库存数量
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <returns>返回获得的库存数量</returns>
        decimal GetStockCount(int goodsID, string batchNo);

        /// <summary>
        /// 修改样品库库存物品存放位置
        /// </summary>
        /// <param name="inMuster">样品单信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>修改成功返回True，修改失败返回False </returns>
        bool UpdateMusterStockInfo(S_MusterStock inMuster, out string error);
    }
}
