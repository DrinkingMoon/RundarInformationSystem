/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  Report.cs
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
    /// 查询类型
    /// </summary>
    public enum Report_FindType
    {
        /// <summary>
        /// 存储过程
        /// </summary>
        存储过程,

        /// <summary>
        /// 业务授权
        /// </summary>
        业务授权,

        /// <summary>
        /// 视图
        /// </summary>
        视图
    }

    /// <summary>
    /// 报表服务类
    /// </summary>
    class Report : BasicServer, IReport
    {
        /// <summary>
        /// 获得查询条件与内容
        /// </summary>
        /// <param name="reportCode">报表编号</param>
        /// <returns>返回Table</returns>
        public DataTable GetFindInfo(string reportCode)
        {
            string strSql = " select FieldName as 查询字段, '' as 查询内容, FieldFormat as 查询字段格式, ParameterName as 参数名, ParameterType as 参数类型 " +
                            " from BASE_IntegrationReportList where reportCode = '" + reportCode + "'";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 检测是否有重复记录
        /// </summary>
        /// <param name="reportCode">报表编码</param>
        /// <returns>重复返回True，不重复返回False</returns>
        bool CheckInfoIsRepeat(string reportCode)
        {
            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            var varData = from a in dataContext.BASE_IntegrationReport
                          where a.ReportCode == reportCode
                          select a;

            if (varData.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获得新号码
        /// </summary>
        /// <param name="reportCode">报表编码</param>
        /// <returns>返回新的报表编码</returns>
        public string GetNewReportCode(string reportCode)
        {
            string strSql = "select Max(ReportCode) from BASE_IntegrationReport where ReportCode like '" 
                + reportCode + "%' and ReportCode <> '" + reportCode + "'";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dtTemp == null 
                || dtTemp.Rows.Count == 0 
                || dtTemp.Rows[0][0].ToString() == "")
            {
                return reportCode + "01";
            }
            else
            {
                string strTemp = dtTemp.Rows[0][0].ToString().Substring(0, reportCode.Length + 2);
                int intTemp = Convert.ToInt32(strTemp.Substring(strTemp.Length - 2, 2)) + 1;

                return reportCode + intTemp.ToString("D2");
            }
        }

        /// <summary>
        /// 报废单报表
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>返回Table</returns>
        public DataTable ScrapReport(DateTime startTime, DateTime endTime)
        {
            string strSql = "select * from S_ScrapBill where BillTime > '"+ startTime +"' and BillTime < '"+ endTime +"'";
            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得树的表数据
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <returns>返回Table</returns>
        public DataTable GetReportTree(string deptCode)
        {
            string strSql = "select * from BASE_IntegrationReport";

            if (!GeneralFunction.IsNullOrEmpty(deptCode))
            {
                strSql = " select * from BASE_IntegrationReport where ReportCode = '01' " +
                         " union all select * from BASE_IntegrationReport " +
                         " where ReportCode like ''+ (select ReportCode from BASE_IntegrationReport " +
                         " where KeyName = dbo.fun_get_BelongDept_Value('"+ deptCode +"') or KeyName = '"+ deptCode +"') +'%'" +
                         " union all select * from BASE_IntegrationReport  where ReportCode like '%'+ "+
                         " (select ReportCode from BASE_IntegrationReport  where Len(ReportCode) = 4 and ReportName like '通用%' "+
                         " ) +'%' and ReportCode not like '010506%'";
            }

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            dtTemp.Columns.Add("RootSign");

            for (int i = 0; i <= dtTemp.Rows.Count - 1; i++)
            {
                int intSlength = dtTemp.Rows[i]["ReportCode"].ToString().Length;

                if (intSlength > 2)
                {
                    dtTemp.Rows[i]["RootSign"] = dtTemp.Rows[i]["ReportCode"].ToString().Substring(0, intSlength - 2);
                }
                else
                {
                    dtTemp.Rows[i]["RootSign"] = "Root";
                }
            }

            return dtTemp;
        }

        /// <summary>
        /// 执行存储过程查询
        /// </summary>
        /// <param name="reportCode">报表编号</param>
        /// <param name="infoList">查询内容</param>
        /// <param name="error">错误信息</param>
        /// <returns>返回Table</returns>
        public DataTable QueryInfo(string reportCode,DataTable infoList,out string error)
        {
            error = null;
            DataTable result = new DataTable();

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.BASE_IntegrationReport
                              where a.ReportCode == reportCode
                              select a;

                BASE_IntegrationReport lnqintegration = new BASE_IntegrationReport();

                if (varData.Count() == 1)
                {
                    lnqintegration = varData.Single();
                }
                else
                {
                    error = "数据不唯一或者为空";
                    return null;
                }

                Report_FindType findType = GlobalObject.GeneralFunction.StringConvertToEnum<Report_FindType>(lnqintegration.FindType);

                switch (findType)
                {
                    case Report_FindType.存储过程:

                        Hashtable parameters = new Hashtable();

                        if (infoList != null)
                        {
                            for (int i = 0; i < infoList.Rows.Count; i++)
                            {
                                if (infoList.Rows[i]["参数类型"].ToString() == "DateTime" && 
                                    (infoList.Rows[i]["查询内容"] == null || infoList.Rows[i]["查询内容"].ToString().Trim().Length == 0))
                                {
                                    infoList.Rows[i]["查询内容"] = ServerTime.Time.ToString().Trim();
                                }

                                parameters.Add(infoList.Rows[i]["参数名"].ToString(),
                                    GlobalObject.GeneralFunction.ChangeType(infoList.Rows[i]["查询内容"].ToString().Trim(),
                                    infoList.Rows[i]["参数类型"].ToString()));
                            }
                        }
                        else
                        {
                            var varList = (from a in dataContext.BASE_IntegrationReportList
                                           where a.ReportCode == reportCode
                                           select a).ToList();

                            if (varList.Count() == 2
                                && varList[0].ParameterType == "DateTime" 
                                && varList[0].ParameterType == "DateTime")
                            {
                                DateTime time = ServerTime.Time.Date;

                                parameters.Add(varList[0].ParameterName, Convert.ToDateTime(time.AddYears(-1).ToShortDateString() + " 00:00:00"));
                                parameters.Add(varList[1].ParameterName, Convert.ToDateTime(time.ToShortDateString() + " 23:59:59"));
                            }
                        }

                        result = GlobalObject.DatabaseServer.QueryInfoPro(lnqintegration.ProcedureName, parameters, out error);
                        break;
                    case Report_FindType.业务授权:
 
                        IAuthorization authorization = PlatformFactory.GetObject<IAuthorization>();
                        IQueryResult qr = null;
                        string strWhere = null;

                        if (infoList != null && infoList.Rows.Count > 0)
                        {
                            strWhere = "日期 >= '" + infoList.Rows[0]["查询内容"].ToString().Trim()
                                + "' and 日期 <= '" + infoList.Rows[1]["查询内容"].ToString().Trim() + "'";
                        }

                        qr = authorization.Query(lnqintegration.ProcedureName, null, strWhere);

                        if (qr.Succeeded)
                        {
                            result = qr.DataCollection.Tables[0];
                        }
                        break;
                    case Report_FindType.视图:

                        string strSql = "select * from " + lnqintegration.ProcedureName + " where 1 = 1";

                        if (infoList != null && infoList.Rows.Count > 0)
                        {
                            for (int i = 0; i < infoList.Rows.Count; i++)
                            {
                                if (infoList.Rows[i]["查询内容"].ToString().Trim().Length > 0)
                                {
                                    strSql = strSql + " and " + infoList.Rows[i]["参数名"].ToString() + " = '"
                                        + infoList.Rows[i]["查询内容"].ToString() + "'";
                                }
                            }
                        }

                        result = GlobalObject.DatabaseServer.QueryInfo(strSql);
                        break;
                    default:
                        break;
                }

                return result;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
        }

        public List<BASE_IntegrationReportList> GetIntegrationReportList(string reportCode)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.BASE_IntegrationReportList
                          where a.ReportCode == reportCode
                          select a;

            return varData.ToList();
        }

        /// <summary>
        /// 获得报表信息
        /// </summary>
        /// <param name="reportCode">报表编号</param>
        /// <returns>成功返回数据，失败返回Null</returns>
        public BASE_IntegrationReport GetReportInfo(string reportCode)
        {
            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            var varData = from a in dataContext.BASE_IntegrationReport
                          where a.ReportCode == reportCode
                          select a;

            if (varData.Count() == 1)
            {
                return varData.Single();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获得报表明细信息
        /// </summary>
        /// <param name="reportCode">报表编号</param>
        /// <returns>返回Table</returns>
        public DataTable GetReportInfoList(string reportCode)
        {
            string strSql = " SELECT FieldName as 查询字段,FieldFormat as 查询字段格式, ParameterName as 参数名, ParameterType  as 参数类型 " +
                            " from  BASE_IntegrationReportList where  ReportCode = '" + reportCode + "'";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 保存报表设置信息
        /// </summary>
        /// <param name="integrationReport">报表数据集</param>
        /// <param name="reportInfoList">字段数据表</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool SaveReportInfo(BASE_IntegrationReport integrationReport,DataTable reportInfoList, out string error)
        {

            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;
            error = null;

            try
            {
                dataContext.Connection.Open();
                dataContext.Transaction = dataContext.Connection.BeginTransaction();

                if (integrationReport.ID == 0)
                {
                    if (CheckInfoIsRepeat(integrationReport.ReportCode))
                    {
                        error = "报表编码重复，请重新录入";
                        return false;
                    }

                    dataContext.BASE_IntegrationReport.InsertOnSubmit(integrationReport);
                }
                else
                {
                    var varData = from a in dataContext.BASE_IntegrationReport
                                  where a.ID == integrationReport.ID
                                  select a;

                    if (varData.Count() == 1)
                    {
                        BASE_IntegrationReport lnqIntegration = varData.Single();

                        lnqIntegration.PrintName = integrationReport.PrintName;
                        lnqIntegration.ProcedureName = integrationReport.ProcedureName;
                        lnqIntegration.ReportCode = integrationReport.ReportCode;
                        lnqIntegration.ReportName = integrationReport.ReportName;
                        lnqIntegration.FindType = integrationReport.FindType;
                    }
                    else
                    {
                        error = "数据为空或者不唯一";
                        return false;
                    }
                }

                if (!SaveReportInfoList(dataContext, integrationReport.ReportCode, reportInfoList, out error))
                {
                    return false;
                }

                dataContext.SubmitChanges();

                dataContext.Transaction.Commit();

                return true;
            }
            catch (Exception ex)
            {
                dataContext.Transaction.Rollback();
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 保存报表信息明细
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="reportCode">报表编码</param>
        /// <param name="reportInfoList">明细表信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True ,失败返回False</returns>
        bool SaveReportInfoList(DepotManagementDataContext dataContext,string reportCode, DataTable reportInfoList, out string error)
        {
            error = null;

            try
            {
                var varData = from a in dataContext.BASE_IntegrationReportList
                              where a.ReportCode == reportCode
                              select a;

                dataContext.BASE_IntegrationReportList.DeleteAllOnSubmit(varData);

                for (int i = 0; i < reportInfoList.Rows.Count; i++)
                {
                    BASE_IntegrationReportList lnqReportList = new BASE_IntegrationReportList();

                    lnqReportList.ReportCode = reportCode;
                    lnqReportList.ParameterName = reportInfoList.Rows[i]["参数名"].ToString();
                    lnqReportList.ParameterType = reportInfoList.Rows[i]["参数类型"].ToString();
                    lnqReportList.FieldName = reportInfoList.Rows[i]["查询字段"].ToString();
                    lnqReportList.FieldFormat = reportInfoList.Rows[i]["查询字段格式"].ToString();

                    dataContext.BASE_IntegrationReportList.InsertOnSubmit(lnqReportList);
                }

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 删除报表信息
        /// </summary>
        /// <param name="reportCode">报表编号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool DeleteReportInfo(string reportCode,out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.BASE_IntegrationReport
                              where a.ReportCode.Substring(0,reportCode.Length) == reportCode
                              select a;

                dataContext.BASE_IntegrationReport.DeleteAllOnSubmit(varData);

                var varDataList = from a in dataContext.BASE_IntegrationReportList
                                  where a.ReportCode.Substring(0, reportCode.Length) == reportCode
                                  select a;

                dataContext.BASE_IntegrationReportList.DeleteAllOnSubmit(varDataList);

                dataContext.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
                
            }

        }

        public List<BASE_IntegrationReport> GetBusinessSelect(CE_BillTypeEnum billType)
        {
            string error = "";
            List<BASE_IntegrationReport> listReport = new List<BASE_IntegrationReport>();

            Hashtable hsTable = new Hashtable();

            hsTable.Add("@BusineesName", billType.ToString());

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfoPro("BASE_IntegrationReport_GetBusinessSelect", hsTable, out error);

            if (dtTemp != null || dtTemp.Rows.Count > 0 || dtTemp.Rows[0][0].ToString().Trim().Length > 0)
            {
                foreach (DataRow dr in dtTemp.Rows)
                {
                    BASE_IntegrationReport report = new BASE_IntegrationReport();

                    report.FindType = dr["FindType"].ToString();
                    report.PrintName = dr["PrintName"].ToString();
                    report.ProcedureName = dr["ProcedureName"].ToString();
                    report.ReportCode = dr["ReportCode"].ToString();
                    report.ReportName = dr["ReportName"].ToString();

                    listReport.Add(report);
                }
            }

            return listReport;
            
        }
    }
}
