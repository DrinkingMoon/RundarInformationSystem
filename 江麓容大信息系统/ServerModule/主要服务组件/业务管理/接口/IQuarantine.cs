/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IQuarantine.cs
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
using System.Data;

namespace ServerModule
{
    /// <summary>
    /// 产品隔离单管理类
    /// </summary>
    public interface IQuarantine
    {
        /// <summary>
        /// 过滤查询
        /// </summary>
        /// <param name="startTime">起始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="djzt">单据状态</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回表信息</returns>
        DataTable GetAllBill(string startTime, string endTime, string djzt, out string error);

        /// <summary>
        /// 获得单号
        /// </summary>
        /// <returns>返回获取的单号</returns>
        string GetBillID();

        /// <summary>
        /// 根据单据号删除(改变删除状态)
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功</returns>
        bool DeleteBill(string djh, out string error);

        /// <summary>
        /// 单据明细的数据库操作
        /// </summary>
        /// <param name="listInfo">需要操作的数据集</param>
        /// <param name="billID">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回true</returns>
        bool AddList(DataTable listInfo, string billID, out string error);

        /// <summary>
        /// 修改ProductStock表的状态
        /// </summary>
        /// <param name="stockCode">箱体编号</param>
        /// <param name="goodID">物品编号</param>
        /// <param name="flag">是否为正常状态 True 是 False 不是</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回True</returns>
        bool UpdateProductStockStatus(string stockCode, string goodID, bool flag, out string error);

        /// <summary>
        /// 添加主表信息
        /// </summary>
        /// <param name="quarantine">S_QuarantineBill对象</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool AddBill(S_QuarantineBill quarantine, out string error);
       
        /// <summary>
        /// 仓管审核，修改主表状态
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="handle">是否处理标志 字符串</param>
        /// <param name="status">单据状态</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool AuditingBill(string billID, string handle, string status, out string error);

        /// <summary>
        /// 根据单据号查询明细
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回表数据</returns>
        DataTable GetList(string billID, out string error);

        /// <summary>
        /// 修改明细
        /// </summary>
        /// <param name="quarantineList">产品明细对象</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回true</returns>
        bool UpdateList(S_QuarantineList quarantineList, out string error);

        /// <summary>
        /// 通过单据号查到处理状态
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="code">箱体编号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回数据集</returns>
        DataTable GetOperationStatus(string billID, string code, out string error);

        /// <summary>
        /// 查询入库商品信息
        /// </summary>
        /// <param name="goodsID">物品编号</param>
        /// <param name="storageID">库房ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回满足条件的数据集</returns>
        DataTable GetProductStockInfo(string goodsID, string storageID, out string error);

        /// <summary>
        /// 查找产品编号查找信息
        /// </summary>
        /// <param name="goodsID">产品ID</param>
        /// <returns>返回满足条件的数据集</returns>
        DataTable GetProductCodeInfo(string goodsID);

        /// <summary>
        /// 查找已经隔离了的产品
        /// </summary>
        /// <returns>返回满足条件的数据集</returns>
        DataTable GetInsulateGoodsInfo();

        /// <summary>
        /// 通过箱体编号查看单据号
        /// </summary>
        /// <param name="stockCode">箱体编号</param>
        /// <returns>返回单据号</returns>
        DataRow GetBillID(string stockCode);

        /// <summary>
        /// 质管处理，修改表信息
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <param name="disposeName">处理人</param>
        /// <param name="dispose">处理方案</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回True</returns>
        bool HandleBill(string djh, string disposeName, string dispose, out string error);
    }
}
