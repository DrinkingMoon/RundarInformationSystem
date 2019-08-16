/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  ICheckReturnRepair.cs
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
    /// 挑选返修返工单业务操作服务接口
    /// </summary>
    public interface ICheckReturnRepair : IBasicBillServer
    {
        /// <summary>
        /// 创建挑返单
        /// </summary>
        /// <param name="djh">报检单单据号</param>
        /// <param name="logID">挑返单创建人工号</param>
        /// <param name="error">出错时返回错误信息</param>
        /// <param name="tfDJH">挑返单单据号</param>
        /// <returns>操作是否成功的标志</returns>
        bool Create(string djh, string logID, out string error, out string tfDJH);

        /// <summary>
        /// 获取全部单据
        /// </summary>
        /// <param name="billStatus">单据状态,若为“全  部”则显示所有单据信息</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>返回查询到的挑返单的单据信息</returns>
        DataTable GetAllBill(string billStatus, DateTime startTime, DateTime endTime);        

        /// <summary>
        /// 获得单条数据
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <returns>返回一条挑返单的单据信息</returns>
        DataRow GetData(string djh);

        /// <summary>
        /// 根据单据状态更新相应的单据信息
        /// </summary>
        /// <param name="bill">Linq挑返单的数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>处理成功返回True，处理失败返回False</returns>
        bool UpdateBill(S_CheckReturnRepairBill bill, out string error);

        /// <summary>
        /// 报废单据
        /// </summary>
        /// <param name="inReturn">Linq挑返单数据集</param>
        /// <param name="flag">标志 True 等待质检机检验 False 等待质检电检验</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>报废成功返回True，报废失败返回False</returns>
        bool ScrapBill(S_CheckReturnRepairBill inReturn, bool flag,
            out string error);

        /// <summary>
        /// 编制人提交单据
        /// </summary>
        /// <param name="inReturn">Linq挑返单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>处理成功返回True，处理失败返回False</returns>
        bool SubmitBill(S_CheckReturnRepairBill inReturn, out string error);

        /// <summary>
        /// 回退单据
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <param name="billStatus">操作类型(单据状态)</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <param name="rebackReason">回退原因</param>
        /// <returns>回退成功返回True，回退失败返回False</returns>
        bool ReturnBill(string djh, string billStatus, out string error, string rebackReason);

        #region 夏石友，2012-07-18，将报检入库单中的此功能移动到此，原方法名：ScrapAllBill

        /// <summary>
        /// 报废入库单单号对应的所有挑返单
        /// </summary>
        /// <param name="inDepotBillID">入库单单号</param>
        /// <param name="error">出错时返回错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        bool ScrapAllBill(string inDepotBillID, out string error);

        #endregion
    }
}
