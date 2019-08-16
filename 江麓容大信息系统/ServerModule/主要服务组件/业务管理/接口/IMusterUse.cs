/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IMusterUse.cs
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
    /// 样品耗用管理类接口
    /// </summary>
    public interface IMusterUse:IBasicBillServer
    {
        /// <summary>
        /// 获得库存信息
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <returns>返回库存数</returns>
        decimal GetStockCount(int goodsID, string batchNo);

        /// <summary>
        /// 获得所有单据
        /// </summary>
        /// <returns>返回样品耗用单信息</returns>
        DataTable GetAllBill();

        /// <summary>
        /// 获得指定单据号的明细
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <returns>返回样品耗用单明细信息</returns>
        DataTable GetList(string djh);

        /// <summary>
        /// 获得一条单据
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <returns>返回一条样品耗用单信息</returns>
        S_MusterUseBill GetBill(string djh);

        /// <summary>
        /// 保存对单据明细的修改
        /// </summary>
        /// <param name="djh">样品耗用单号</param>
        /// <param name="storageID">库房ID</param>
        /// <param name="purposeCode">用途编码</param>
        /// <param name="useList">单据明细信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>保存成功返回True，保存失败返回False</returns>
        bool SaveBill(string djh, string storageID, string purposeCode,
            DataTable useList, out string error);

        /// <summary>
        /// 更新单据状态
        /// </summary>
        /// <param name="inMusterUse">样品耗用单信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>更新成功返回True，更新失败返回False</returns>
        bool UpdateBill(S_MusterUseBill inMusterUse, out string error);

        /// <summary>
        /// 报废单据
        /// </summary>
        /// <param name="djh">样品耗用单单号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>报废成功返回True，报废失败返回False</returns>
        bool ScarpBill(string djh, out string error);
    }
}
