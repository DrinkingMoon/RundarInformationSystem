/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IFrockProvingReport.cs
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


namespace ServerModule
{
    /// <summary>
    /// 工装验证报告管理类接口
    /// </summary>
    public interface IFrockProvingReport : IBasicBillServer
    {
        /// <summary>
        /// 获得指定单据数据
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <returns>返回一条工装检验报告的数据集</returns>
        S_FrockProvingReport GetBill(string djh);

        /// <summary>
        /// 获得全部单据信息
        /// </summary>
        /// <param name="billStatus">单据状态,若为“全  部”则显示所有单据信息</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>返回工装检验报告视图信息</returns>
        DataTable GetBill(string billStatus, DateTime startTime, DateTime endTime);

        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool DeleteBill(string djh, out string error);

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
        /// 插入新的数据
        /// </summary>
        /// <param name="frockProvingReport">数据集</param>
        /// <param name="attachedTable">检验验证内容表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool AddBill(S_FrockProvingReport frockProvingReport, DataTable attachedTable, out string error);

        /// <summary>
        /// 获得附属表的信息
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <param name="attachedType">附属信息内容类型 （检验，验证）</param>
        /// <returns>返回附属表的数据集</returns>
        System.Data.DataTable GetAttachedTable(string djh, string attachedType);

        /// <summary>
        /// 单据流程
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="status">操作状态</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool UpdateBill(string billNo, string status, out string error);

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="frockProvingReport">LINQ 数据集</param>
        /// <param name="attachedTable">检测信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        bool SaveInfo(S_FrockProvingReport frockProvingReport, DataTable attachedTable, out string error);
    }
}
