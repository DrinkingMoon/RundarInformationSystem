/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IGatherBillAndDetailBillServer.cs
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

namespace Service_Economic_Financial
{
    /// <summary>
    /// 入库汇总/明细表管理类接口
    /// </summary>
    public interface IGatherBillAndDetailBillServer
    {
        DataTable GetBusDetailInfo(string selectType, DateTime startTime, DateTime endTime, string storageID);

        DataTable GetMonthlyBalanceInfo(string yearMonth, string selectType, string storageID);

        /// <summary>
        /// 添加初始化收发存汇总表初始的上月结存记录
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <param name="counts">数量</param>
        /// <param name="dateTimes">日期</param>
        /// <param name="materialType">领料类型</param>
        /// <param name="returnBill">返回table 数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool AddReceiveSendSaveBalanceTable(string goodsCode, string goodsName, string spec,
            int counts, string dateTimes, string materialType, out DataTable returnBill, out string error);

        /// <summary>
        /// 删除(初始化收发存汇总表)某一初始记录
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="returnBill">返回table数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool DeleteReceiveSendSaveBalanceTable(string id, out DataTable returnBill, out string error);

        /// <summary>
        /// 修改初始化收发存汇总表初始上月结存记录
        /// </summary>
        /// <param name="id">ID序号</param>
        /// <param name="counts">数量</param>
        /// <param name="materialType">领料类型</param>
        /// <param name="returnBill">返回table数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool UpdateReceiveSendSaveBalanceTable(string id, int counts, string materialType,
            out DataTable returnBill, out string error);
        
        /// <summary>
        /// 获取指定日期的新账套的收发存汇总表
        /// </summary>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="storageID">库房ID 如果查询全部库房则为null</param>
        /// <returns>返回获取到的收发存汇总表</returns>
        DataTable GetAllGather_New(DateTime beginDate, DateTime endDate, string storageID);

        /// <summary>
        /// 台帐
        /// </summary>
        /// <param name="productName">查询方式</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="storageID">库房ID</param>
        /// <param name="batchNo">批次号</param>
        /// <param name="showTable">返回table数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool GetAllGather(string productName, int goodsID,
            DateTime startDate, DateTime endDate, string storageID, string batchNo,
            out DataTable showTable, out string error);

        /// <summary>
        /// 获得旧库存批次及数量
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="time">时间</param>
        /// <returns>返回旧库存批次及数量的数据集</returns>
        DataTable GetOldStock(int goodsID, string time);

        /// <summary>
        /// 保存进销存表
        /// </summary>
        /// <param name="showTable">需要存储的数据表</param>
        /// <param name="yearAndMonth">年月 格式为“YYYYMM”</param>
        /// <param name="storageID">库房ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool SaveMarktingGather(DataTable showTable, string yearAndMonth, string storageID, out string error);

        /// <summary>
        /// 检查是否有数据存在
        /// </summary>
        /// <param name="yearAndMonth">查询年月字符串 格式为“YYYYMM”</param>
        /// <param name="storageID">库房ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool IsDataIn(string yearAndMonth, string storageID, out string error);
    }
}
