/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IReport.cs
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
    /// 报表服务类接口
    /// </summary>
    public interface IReport
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportCode"></param>
        /// <returns></returns>
        List<BASE_IntegrationReportList> GetIntegrationReportList(string reportCode);

        /// <summary>
        /// 执行存储过程查询
        /// </summary>
        /// <param name="reportCode">报表编号</param>
        /// <param name="infoList">查询内容</param>
        /// <param name="error">错误信息</param>
        /// <returns>返回Table</returns>
        DataTable QueryInfo(string reportCode, DataTable infoList, out string error);

        /// <summary>
        /// 获得查询条件与内容
        /// </summary>
        /// <param name="reportCode">报表编号</param>
        /// <returns>返回Table</returns>
        DataTable GetFindInfo(string reportCode);

        /// <summary>
        /// 获得新号码
        /// </summary>
        /// <param name="reportCode">报表编码</param>
        /// <returns>返回新的报表编码</returns>
        string GetNewReportCode(string reportCode);
        
        /// <summary>
        /// 删除报表信息
        /// </summary>
        /// <param name="reportCode">报表编号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool DeleteReportInfo(string reportCode, out string error);

        /// <summary>
        /// 获得报表信息
        /// </summary>
        /// <param name="reportCode">报表编号</param>
        /// <returns>成功返回数据，失败返回Null</returns>
        BASE_IntegrationReport GetReportInfo(string reportCode);

        /// <summary>
        /// 获得报表明细信息
        /// </summary>
        /// <param name="reportCode">报表编号</param>
        /// <returns>返回Table</returns>
        DataTable GetReportInfoList(string reportCode);

        /// <summary>
        /// 保存报表设置信息
        /// </summary>
        /// <param name="integrationReport">报表数据集</param>
        /// <param name="reportInfoList">字段数据表</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool SaveReportInfo(BASE_IntegrationReport integrationReport, DataTable reportInfoList, out string error);

        /// <summary>
        /// 报废单报表
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>返回Table</returns>
        DataTable ScrapReport(DateTime startTime, DateTime endTime);
        
        /// <summary>
        /// 获得树的表数据
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <returns>返回Table</returns>
        DataTable GetReportTree(string deptCode);

        /// <summary>
        /// 获得业务报表列表
        /// </summary>
        /// <param name="billType">业务类型</param>
        /// <returns>返回列表</returns>
        List<BASE_IntegrationReport> GetBusinessSelect(CE_BillTypeEnum billType);
    }
}
