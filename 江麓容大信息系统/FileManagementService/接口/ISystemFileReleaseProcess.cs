/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  ISystemFileReleaseProcess.cs
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
using ServerModule;

namespace Service_Quality_File
{
    /// <summary>
    /// 发布流程服务类接口
    /// </summary>
    public interface ISystemFileReleaseProcess : IBasicBillServer
    {
        /// <summary>
        /// 回退流程
        /// </summary>
        /// <param name="sdbNo">流程编号</param>
        /// <param name="sdbStatus">流程状态</param>
        /// <param name="releaseProcess">数据集</param>
        /// <param name="error">错误信息</param>
        /// <param name="rebackReason">回退原因</param>
        /// <returns>成功返回True,失败返回False</returns>
        bool ReturnBill(string sdbNo, string sdbStatus, FM_ReleaseProcess releaseProcess, out string error, string rebackReason);

        /// <summary>
        /// 获得所有流程视图信息
        /// </summary>
        /// <param name="sdbStatus">流程状态</param>
        /// <param name="startTime">开始日期</param>
        /// <param name="endTime">结束日期</param>
        /// <returns>返回Table</returns>
        System.Data.DataTable GetAllInfo(string sdbStatus, DateTime startTime, DateTime endTime);

        /// <summary>
        /// 获得单条流程的LNQ数据集
        /// </summary>
        /// <param name="sdbNo">流程编号</param>
        /// <returns>返回LNQ数据集</returns>
        ServerModule.FM_ReleaseProcess GetInfo(string sdbNo);
        
        /// <summary>
        /// 删除流程
        /// </summary>
        /// <param name="sdbNo">流程编号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        bool DeleteProcess(string sdbNo, out string error);

        /// <summary>
        /// 申请流程
        /// </summary>
        /// <param name="releseProcess">LNQ数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        bool AddProcess(FM_ReleaseProcess releseProcess, out string error);
        
        /// <summary>
        /// 审核流程
        /// </summary>
        /// <param name="releseProcess">LNQ数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回false </returns>
        bool AuditProcess(FM_ReleaseProcess releseProcess, out string error);

        /// <summary>
        /// 批准流程
        /// </summary>
        /// <param name="releseProcess">LNQ数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        bool ApproveProcess(FM_ReleaseProcess releseProcess, out string error);
    }
}
