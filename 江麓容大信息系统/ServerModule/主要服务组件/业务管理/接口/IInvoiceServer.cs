/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IInvoiceServer.cs
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
    /// 发票管理类接口
    /// </summary>
    public interface IInvoiceServer
    {
        /// <summary>
        /// 删除发票记录
        /// </summary>
        /// <param name="invoiceCode">发票号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        bool DeleteInvoiceInfo(string invoiceCode, out string error);

        /// <summary>
        /// 添加发票记录
        /// </summary>
        /// <param name="invoiceTable">需要添加的发票信息数据</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>添加成功返回True，添加失败返回False</returns>
        bool AddInvoiceInfo(DataTable invoiceTable, out string error);

        /// <summary>
        /// 获得一张发票的数据集
        /// </summary>
        /// <param name="invoiceCode">发票号</param>
        /// <returns>返回查询到的发票信息</returns>
        DataTable GetInvoiceInfo(string invoiceCode);

        /// <summary>
        /// 获取全部发票记录
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>返回查询到的发票记录表</returns>
        DataTable GetInvoiceInfo(DateTime startTime, DateTime endTime);

        /// <summary>
        /// 获得对应的单据数据集
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="provider">供应商编码</param>
        /// <param name="orderNumber">订单号</param>
        /// <returns>返回获取对应的单据数据集</returns>
        DataTable GetBillInfo(DateTime startTime, DateTime endTime, string provider, string orderNumber);

        /// <summary>
        /// 获得对应的物品明细
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <returns>返回对应的物品单据明细</returns>
        DataTable GetGoodsInfo(string billID);

        /// <summary>
        /// 更新出入库的金额
        /// </summary>
        /// <param name="invoiceTable">需要更新的数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>更新成功True，更新失败False</returns>
        bool UpdatePrice(DataTable invoiceTable, out string error);

        /// <summary>
        /// 修改发票记录
        /// </summary>
        /// <param name="invoiceCode">新发票号</param>
        /// <param name="provide">供应商</param>
        /// <param name="invoiceType">1:专用发票，0:非专用发票</param>
        /// <param name="oldCode">旧发票号</param>
        /// <param name="pzh">新凭证号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>修改成功返回True，修改失败返回False</returns>
        bool UpdateInvoiceInfo(string invoiceCode, string provide, int invoiceType, string oldCode, string pzh, out string error);
    }
}
