/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  ICompositiveServer.cs
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
using System.Text;
using System.Collections;
using System.Data;
using System.Linq;

namespace ServerModule
{
    /// <summary>
    /// 打印管理类接口
    /// </summary>
    public interface IPrintManagement
    {
        /// <summary>
        /// 添加完成打印的报表/单据
        /// </summary>
        /// <param name="printInfo">打印信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool AddPrintInfo(S_PrintBillTable printInfo, out string error);

        /// <summary>
        /// 检查单据是否已经完成打印
        /// </summary>
        /// <param name="printInfo">打印信息</param>
        /// <param name="message">输出的消息</param>
        /// <returns>已经打印返回true</returns>
        bool IsExist(S_PrintBillTable printInfo, out string printMessage);

        /// <summary>
        /// 获取打印明细信息
        /// </summary>
        /// <param name="beginDate">获取的起始时间</param>
        /// <param name="endDate">获取的结束时间</param>
        /// <param name="dept">打印单据接收部门名称</param>
        /// <returns>返回获取到时间范围内的明细信息</returns>
        System.Linq.IQueryable<View_S_PrintBill> GetPrintInfo(DateTime beginDate, DateTime endDate, string dept);

        /// <summary>
        /// 获取打印单据接收部门名称
        /// </summary>
        /// <returns>返回获取到的接收部门名称</returns>
        System.Linq.IQueryable<string> GetReceivedDeptOfPrintBill();

        /// <summary>
        /// 修改审核状态
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="m_err"></param>
        /// <returns></returns>
        bool SetChecked(DataTable dt, bool blZT, out string m_err);

        /// <summary>
        /// 通过单据号查询单据信息
        /// </summary>
        /// <param name="DJH">单据号</param>
        /// <returns></returns>
        DataTable GetPrintBillTableByDJH(string DJH);

        /// <summary>
        /// 添加重新打印的单据
        /// </summary>
        /// <param name="againprint"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        bool Add_S_AgainPrintBillTable(S_AgainPrintBillTable againprint,out string error);

        /// <summary>
        /// 审核重新打印的单据
        /// </summary>
        /// <param name="print"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        bool UpdateAgainPrintBillTable(string DJH, out string error);

        /// <summary>
        /// 查重新打印表中所有信息
        /// </summary>
        /// <returns>表中所有数据</returns>
        DataTable GetAgainPrintBill(DateTime dtstarttime, DateTime dtendtime);

        /// <summary>
        /// 批准后修改打印状态
        /// </summary>
        /// <param name="DJH">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns></returns>
        bool UpdateAuthorize(string DJH, out string error);
        
        /// <summary>
        /// 按时间查询表信息
        /// </summary>
        /// <returns>返回表中的所有数据</returns>
        DataTable GetAgainPrintBillByTime(string startTime, string endTime);

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="billid"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        bool Del_S_AgainPrintBillTable(int billid, out string error);
    }
}
