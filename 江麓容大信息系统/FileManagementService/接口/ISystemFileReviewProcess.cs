/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  ISystemFileReviewProcess.cs
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
    /// 审查流程服务接口
    /// </summary>
    public interface ISystemFileReviewProcess : IBasicBillServer,IBasicService
    {
        
        /// <summary>
        /// 相关人上传意见
        /// </summary>
        /// <param name="advise">意见</param>
        /// <param name="sdbNo">流程单号</param>
        void PointAdvise(string advise, string sdbNo);

        /// <summary>
        /// 上传文件路径
        /// </summary>
        /// <param name="guid">文件唯一编码</param>
        /// <param name="sdbNo">流程单号</param>
        void PointUpLoadFile(Guid guid, string sdbNo);

        /// <summary>
        /// 删除流程
        /// </summary>
        /// <param name="sdbNo">流程编号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回false </returns>
        bool DeleteProcess(string sdbNo, out string error);

        /// <summary>
        /// 新建流程
        /// </summary>
        /// <param name="reviewProcess">流程主要信息</param>
        /// <param name="pointPersonnel">指定相关确认人</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        bool AddProcess(ServerModule.FM_ReviewProcess reviewProcess, System.Collections.Generic.List<string> pointPersonnel, out string error);

        /// <summary>
        /// 部门主管审核
        /// </summary>
        /// <param name="reviewProcess">流程主要信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true ，失败返回False</returns>
        bool AuditProcess(ServerModule.FM_ReviewProcess reviewProcess, out string error);

        /// <summary>
        /// 获得所有流程视图信息
        /// </summary>
        /// <returns>返回Table</returns>
        System.Data.DataTable GetAllInfo();

        /// <summary>
        /// 获得单条流程的LNQ数据集
        /// </summary>
        /// <param name="sdbNo">流程编号</param>
        /// <returns>返回LNQ数据集</returns>
        ServerModule.FM_ReviewProcess GetInfo(string sdbNo);

        /// <summary>
        /// 获得单条流程的指定确认人的LNQ数据集合
        /// </summary>
        /// <param name="sdbNo">流程编号</param>
        /// <returns>返回LNQ列表</returns>
        System.Collections.Generic.List<ServerModule.FM_ReviewProcessPointListInfo> GetListInfo(string sdbNo);

        /// <summary>
        /// 获得单条流程的指定确认人的Table
        /// </summary>
        /// <param name="sdbNo">流程编号</param>
        /// <returns>返回Table</returns>
        System.Data.DataTable GetListInfoTable(string sdbNo);

        /// <summary>
        /// 判定流程信息
        /// </summary>
        /// <param name="reviewProcess">流程主要信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        bool JudgeProcess(ServerModule.FM_ReviewProcess reviewProcess, out string error);

        /// <summary>
        /// 相关人确认流程信息
        /// </summary>
        /// <param name="sdbNo">流程单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        bool PointAffirmProcess(string sdbNo, out string error);
        
        /// <summary>
        /// 回退流程
        /// </summary>
        /// <param name="sdbNo">流程编号</param>
        /// <param name="sdbStatus">流程状态</param>
        /// <param name="reviewProcess">数据集</param>
        /// <param name="error">错误信息</param>
        /// <param name="rebackReason">回退原因</param>
        /// <returns>成功返回True,失败返回False</returns>
        bool ReturnBill(string sdbNo, string sdbStatus, FM_ReviewProcess reviewProcess, out string error, string rebackReason);
    }
}
