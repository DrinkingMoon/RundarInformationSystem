/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IQualityProblemRectificationDisposalBill.cs
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
    /// 质量问题整改处置单接口类
    /// </summary>
    public interface IQualityProblemRectificationDisposalBill : IBasicBillServer
    {
        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="qualityProblem">数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool SaveInfo(ZL_QualityProblemRectificationDisposalBill qualityProblem, out string error);

        /// <summary>
        /// 保存搭车分析计划表的分析结果信息
        /// </summary>
        /// <param name="assemblingAnalysis">数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool SaveAnalysisResult(ZL_AssemblingAnalysisSchedule assemblingAnalysis, out string error);

        /// <summary>
        /// 审核搭车分析计划表的分析结果
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool AuditingAnalysisResult(string billID, out string error);

        /// <summary>
        /// 对各种计划表审核记录
        /// </summary>
        /// <param name="scheduleType">计划表类型：试验验证计划表，新品开发计划表，搭车分析计划表</param>
        /// <param name="billID">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool AuditingSundrySchedule(string scheduleType, string billID, out string error);

        /// <summary>
        /// 获得单据号
        /// </summary>
        /// <param name="scheduleType">表类型:质量整改处置单,试验验证计划表,新品开发计划表,搭车分析计划表</param>
        /// <returns>返回单据号</returns>
        string GetBillID(string scheduleType);

        /// <summary>
        /// 获得记录集
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="billStatus">单据状态</param>
        /// <returns>返回获得的记录集</returns>
        DataTable GetAllBill(DateTime startTime, DateTime endTime, string billStatus);
        
        /// <summary>
        /// 获得某一条试验验证计划表明细信息
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <returns>返回一条明细信息</returns>
        ZL_ExperimentsSchedule GetExperimentsMessage(string billID);
        
        /// <summary>
        /// 获得某一条搭车分析计划表明细信息
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <returns>返回一条明细信息</returns>
        ZL_AssemblingAnalysisSchedule GetAssemblingAnalysisMessage(string billID);
        
        /// <summary>
        /// 获得某一条新品开发计划表明细信息
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <returns>返回一条明细信息</returns>
        ZL_NewProductDevelopmentSchedule GetNewProductDevelopmentMessage(string billID);

        /// <summary>
        /// 获得某一条质量问题整改处置单明细信息
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <returns>返回一条明细信息</returns>
        ZL_QualityProblemRectificationDisposalBill GetQualityProblemMessage(string billID);

        /// <summary>
        /// 插入新数据
        /// </summary>
        /// <param name="qualityProblem">数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool InsertBill(ZL_QualityProblemRectificationDisposalBill qualityProblem, out string error);

        /// <summary>
        /// 流程管理
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool UpdateBill(string billNo, out string error);

        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="billID">需要删除的单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool DeleteBill(string billID, out string error);

        /// <summary>
        /// 对各种计划表插入新记录
        /// </summary>
        /// <param name="scheduleType">计划表类型：试验验证计划表，新品开发计划表，搭车分析计划表</param>
        /// <param name="billID">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool InsertSundrySchedule(string scheduleType, string billID, out string error);

        /// <summary>
        /// 对各种计划表删除记录
        /// </summary>
        /// <param name="scheduleType">计划表类型：试验验证计划表，新品开发计划表，搭车分析计划表</param>
        /// <param name="billID">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool DeleteSundrySchedule(string scheduleType, string billID, out string error);

        /// <summary>
        /// 删除过剩的计划表记录
        /// </summary>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool DeleteExcessSchedule(out string error);

        /// <summary>
        /// 获得试验验证计划表的试验步骤明细
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <returns>返回Table</returns>
        DataTable GetExperimentsScheduleList(string billID);

        /// <summary>
        /// 获得新品开发计划表的开发步骤明细
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <returns>返回Table</returns>
        DataTable GetNewProductDevelopmentScheduleList(string billID);

        /// <summary>
        /// 保存试验验证计划表的信息
        /// </summary>
        /// <param name="experiment">数据集</param>
        /// <param name="listInfo">具体步骤列表</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool SaveExperimentsScheduleInfo(ZL_ExperimentsSchedule experiment, DataTable listInfo, out string error);

        /// <summary>
        /// 保存搭车分析计划表信息
        /// </summary>
        /// <param name="assemblingAnalysis">数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool SaveAssemblingAnalysisScheduleInfo(ZL_AssemblingAnalysisSchedule assemblingAnalysis, out string error);

        /// <summary>
        /// 保存试验验证计划表的信息
        /// </summary>
        /// <param name="newProductDevelopment">数据集</param>
        /// <param name="listInfo">具体开发过程列表</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool SaveNewProductDevelopmentScheduleInfo(ZL_NewProductDevelopmentSchedule newProductDevelopment, DataTable listInfo, out string error);
    }
}
