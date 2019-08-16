/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  SystemFileReviewProcess.cs
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
using System.Text.RegularExpressions;
using ServerModule;
    
namespace Service_Quality_File
{
    /// <summary>
    /// 文件审查流程类
    /// </summary>
    class SystemFileReviewProcess:BasicServer, ISystemFileReviewProcess
    {
        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 检查某业务是否存在
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">业务号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.FM_ReviewProcess
                          where a.SDBNo == billNo
                          select a;

            if (varData == null || varData.Count() == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 检查某单据是否存在
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(string billNo)
        {
            string sql = "SELECT * FROM [DepotManagement].[dbo].[FM_ReviewProcess] where SDBNo = '" + billNo + "'";

            System.Data.DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 获得所有流程视图信息
        /// </summary>
        /// <param name="sdbStatus">流程状态</param>
        /// <param name="startTime">开始日期</param>
        /// <param name="endTime">结束日期</param>
        /// <returns>返回Table</returns>
        public DataTable GetAllInfo()
        {

            IAuthorization authorization = PlatformFactory.GetObject<IAuthorization>();
            IQueryResult qr = null;

            if (QueryResultFilter == null)
            {
                qr = authorization.Query("文件体系审查流程权限查询", null);
            }
            else
            {
                qr = authorization.Query("文件体系审查流程权限查询", null, QueryResultFilter);
            }

            return qr.DataCollection.Tables[0];
        }

        /// <summary>
        /// 获得单条流程的LNQ数据集
        /// </summary>
        /// <param name="sdbNo">流程编号</param>
        /// <returns>返回LNQ数据集</returns>
        public FM_ReviewProcess GetInfo(string sdbNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.FM_ReviewProcess
                          where a.SDBNo == sdbNo
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
        /// 获得单条流程的指定确认人的Table
        /// </summary>
        /// <param name="sdbNo">流程编号</param>
        /// <returns>返回Table</returns>
        public DataTable GetListInfoTable(string sdbNo)
        {
            string strSql = "select 姓名 as 指定人,Advise as 意见,OperateTime as 操作日期,FileUnique as 文件唯一编码, PointPersonnel as 工号 " +
                " from FM_ReviewProcessPointListInfo as a inner join View_HR_Personnel as b on a.PointPersonnel = b.工号 where SDBNo = '" + sdbNo + "'";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得单条流程的指定确认人的LNQ数据集合
        /// </summary>
        /// <param name="sdbNo">流程编号</param>
        /// <returns>返回LNQ列表</returns>
        public List<FM_ReviewProcessPointListInfo> GetListInfo(string sdbNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.FM_ReviewProcessPointListInfo
                          where a.SDBNo == sdbNo
                          select a;

            return varData.ToList();
        }

        /// <summary>
        /// 新建流程
        /// </summary>
        /// <param name="reviewProcess">流程主要信息</param>
        /// <param name="pointPersonnel">指定相关确认人</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        public bool AddProcess(FM_ReviewProcess reviewProcess,List<string> pointPersonnel,out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.FM_ReviewProcess
                              where a.SDBNo == reviewProcess.SDBNo
                              select a;

                if (varData.Count() == 0)
                {
                    FM_ReviewProcess lnqTemp = new FM_ReviewProcess();

                    lnqTemp.SDBNo = reviewProcess.SDBNo;
                    lnqTemp.SDBStatus = "等待主管审核";
                    lnqTemp.Remark = reviewProcess.Remark;
                    lnqTemp.Propoer = BasicInfo.LoginName;
                    lnqTemp.PropoerTime = ServerTime.Time;
                    lnqTemp.FileName = reviewProcess.FileName;
                    lnqTemp.FileNo = reviewProcess.FileNo;
                    lnqTemp.FileUnique = reviewProcess.FileUnique;

                    ctx.FM_ReviewProcess.InsertOnSubmit(lnqTemp);
                }
                else if(varData.Count() == 1)
                {

                    FM_ReviewProcess lnqTemp = varData.Single();

                    lnqTemp.SDBNo = reviewProcess.SDBNo;
                    lnqTemp.SDBStatus = "等待主管审核";
                    lnqTemp.Remark = reviewProcess.Remark;
                    lnqTemp.Propoer = BasicInfo.LoginName;
                    lnqTemp.PropoerTime = ServerTime.Time;
                    lnqTemp.FileName = reviewProcess.FileName;
                    lnqTemp.FileNo = reviewProcess.FileNo;
                    lnqTemp.FileUnique = reviewProcess.FileUnique;
                }
                else
                {
                    error = "数据错误";
                    return false;
                }

                var varList = from a in ctx.FM_ReviewProcessPointListInfo
                              where a.SDBNo == reviewProcess.SDBNo
                              select a;

                ctx.FM_ReviewProcessPointListInfo.DeleteAllOnSubmit(varList);

                //var varPersonnel = (from a in ctx.HR_Personnel
                //                   where pointPersonnel.Contains(a.WorkID)
                //                   select a.Dept.Substring(0, 2)).Distinct();

                //PlatformManagement.IDeptManagerRole deptManagerRole = PlatformFactory.GetObject<IDeptManagerRole>();
                //PlatformManagement.IUserManagement userManagerRole = PlatformFactory.GetObject<IUserManagement>();

                //foreach (string item in varPersonnel)
                //{
                //    IQueryable<View_Auth_User> usersInfo = 
                //        userManagerRole.GetUsers(deptManagerRole.GetManagementRole(item, RoleStyle.负责人).Keys.ToArray());

                //    if (usersInfo != null)
                //    {
                //        pointPersonnel.AddRange((from r in usersInfo select r.登录名).ToList());
                //    }
                //}

                foreach (string personnel in pointPersonnel)
                {
                    FM_ReviewProcessPointListInfo lnqPoint = new FM_ReviewProcessPointListInfo();

                    lnqPoint.SDBNo = reviewProcess.SDBNo;
                    lnqPoint.PointPersonnel = personnel;

                    ctx.FM_ReviewProcessPointListInfo.InsertOnSubmit(lnqPoint);
                }

                ctx.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 部门主管审核
        /// </summary>
        /// <param name="reviewProcess">流程主要信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true ，失败返回False</returns>
        public bool AuditProcess(FM_ReviewProcess reviewProcess, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.FM_ReviewProcess
                              where a.SDBNo == reviewProcess.SDBNo
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数据错误";
                    return false;
                }
                else
                {
                    FM_ReviewProcess lnqTemp = varData.Single();

                    lnqTemp.Auditor = BasicInfo.LoginName;
                    lnqTemp.AuditorAdvise = reviewProcess.AuditorAdvise;
                    lnqTemp.AuditorFileUnique = reviewProcess.AuditorFileUnique;
                    lnqTemp.AuditorTime = ServerTime.Time;

                    var varPoint = from a in ctx.FM_ReviewProcessPointListInfo
                                   where a.SDBNo == lnqTemp.SDBNo
                                   && a.OperateTime == null
                                   select a;

                    if (varPoint.Count() == 0)
                    {
                        lnqTemp.SDBStatus = "等待判定";
                    }
                    else
                    {
                        lnqTemp.SDBStatus = "等待相关确认";
                    }
                }

                ctx.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 相关人上传文件路径
        /// </summary>
        /// <param name="guid">文件唯一编码</param>
        /// <param name="sdbNo">流程单号</param>
        public void PointUpLoadFile(Guid guid, string sdbNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.FM_ReviewProcessPointListInfo
                          where a.SDBNo == sdbNo
                          && a.PointPersonnel == BasicInfo.LoginID
                          select a;

            if (varData.Count() == 1)
            {
                FM_ReviewProcessPointListInfo lnqList = varData.Single();

                lnqList.FileUnique = guid;
            }

            ctx.SubmitChanges();
        }

        /// <summary>
        /// 相关人上传意见
        /// </summary>
        /// <param name="advise">意见</param>
        /// <param name="sdbNo">流程单号</param>
        public void PointAdvise(string advise, string sdbNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.FM_ReviewProcessPointListInfo
                          where a.SDBNo == sdbNo
                          && a.PointPersonnel == BasicInfo.LoginID
                          select a;

            if (varData.Count() == 1)
            {
                FM_ReviewProcessPointListInfo lnqList = varData.Single();

                lnqList.Advise = advise;
            }

            ctx.SubmitChanges();
        }

        /// <summary>
        /// 相关人确认流程信息
        /// </summary>
        /// <param name="sdbNo">流程单号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        public bool PointAffirmProcess(string sdbNo, out string error)
        {
            error = null;

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();

            try
            {
                var varData = from a in ctx.FM_ReviewProcessPointListInfo
                              where a.SDBNo == sdbNo
                              && a.PointPersonnel == BasicInfo.LoginID
                              select a;

                if (varData.Count() != 1)
                {
                    throw new Exception("数据错误");
                }
                else
                {
                    FM_ReviewProcessPointListInfo lnqTemp = varData.Single();

                    lnqTemp.OperateTime = ServerTime.Time;
                }

                ctx.SubmitChanges();

                varData = from a in ctx.FM_ReviewProcessPointListInfo
                          where a.SDBNo == sdbNo
                          && a.OperateTime == null
                          select a;

                if (varData.Count() == 0)
                {
                    var varMain = from a in ctx.FM_ReviewProcess
                                  where a.SDBNo == sdbNo
                                  select a;

                    if (varMain.Count() != 1)
                    {
                        throw new Exception("数据错误");
                    }
                    else
                    {
                        FM_ReviewProcess lnqReview = varMain.Single();

                        lnqReview.SDBStatus = "等待判定";
                        m_billMessageServer.PassFlowMessage(sdbNo,
                            string.Format("{0}号文件审查流程已确认，请体系工程师判定", sdbNo),
                            BillFlowMessage_ReceivedUserType.角色, CE_RoleEnum.体系工程师.ToString());
                    }
                }
                else
                {
                    List<string> list = new List<string>();

                    foreach (FM_ReviewProcessPointListInfo item in varData)
                    {
                        if (item.PointPersonnel != BasicInfo.LoginID)
                        {
                            list.Add(item.PointPersonnel);
                        }
                    }

                    m_billMessageServer.PassFlowMessage(sdbNo,
                        string.Format("{0}号文件审查流程已审核，请相关人员确认", sdbNo),
                        BillFlowMessage_ReceivedUserType.用户, list);
                }

                ctx.SubmitChanges();

                ctx.Transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;

                ctx.Transaction.Rollback();
                return false;
            }
        }

        /// <summary>
        /// 判定流程信息
        /// </summary>
        /// <param name="reviewProcess">流程主要信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        public bool JudgeProcess(FM_ReviewProcess reviewProcess, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.FM_ReviewProcess
                              where a.SDBNo == reviewProcess.SDBNo
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数据错误";
                    return false;
                }
                else
                {
                    FM_ReviewProcess lnqTemp = varData.Single();

                    lnqTemp.SDBStatus = "流程已结束";
                    lnqTemp.JudgeTime = ServerTime.Time;
                    lnqTemp.Judge = BasicInfo.LoginName;
                    lnqTemp.JudgeFileUnique = reviewProcess.JudgeFileUnique;
                    lnqTemp.JudgeAdvise = reviewProcess.JudgeAdvise;
                }

                ctx.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 删除流程
        /// </summary>
        /// <param name="sdbNo">流程编号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回false </returns>
        public bool DeleteProcess(string sdbNo, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                string strSql = " select SDBNo,a.FileUnique,FilePath from ( " +
                                " select FileUnique,SDBNo from dbo.FM_ReviewProcess " +
                                " union all " +
                                " select AuditorFileUnique,SDBNo  from dbo.FM_ReviewProcess " +
                                " union all " +
                                " select JudgeFileUnique,SDBNo  from dbo.FM_ReviewProcess " +
                                " union all " +
                                " select FileUnique,SDBNo from  dbo.FM_ReviewProcessPointListInfo) as a " +
                                " inner join FM_FilePath as b on a.FileUnique = b.FileUnique " +
                                " where a.FileUnique is not null and SDBNo = '" + sdbNo + "'";

                DataTable tempDT = GlobalObject.DatabaseServer.QueryInfo(strSql);

                var varData = from a in ctx.FM_ReviewProcess
                              where a.SDBNo == sdbNo
                              select a;

                ctx.FM_ReviewProcess.DeleteAllOnSubmit(varData);

                if (tempDT != null)
                {
                    FileServiceSocket serverFTP = new FileServiceSocket(GlobalObject.GlobalParameter.FTPServerIP,
                        GlobalObject.GlobalParameter.FTPServerAdvancedUser,
                        GlobalObject.GlobalParameter.FTPServerAdvancedPassword);

                    foreach (DataRow dr in tempDT.Rows)
                    {
                        serverFTP.Delete(dr["FilePath"].ToString());

                        var varFileInfo = from a in ctx.FM_FilePath
                                          where a.FileUnique == (Guid)dr["FileUnique"]
                                          select a;

                        ctx.FM_FilePath.DeleteAllOnSubmit(varFileInfo);
                    }
                }

                ctx.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 回退流程
        /// </summary>
        /// <param name="sdbNo">流程编号</param>
        /// <param name="sdbStatus">流程状态</param>
        /// <param name="reviewProcess">数据集</param>
        /// <param name="error">错误信息</param>
        /// <param name="rebackReason">回退原因</param>
        /// <returns>成功返回True,失败返回False</returns>
        public bool ReturnBill(string sdbNo, string sdbStatus, FM_ReviewProcess reviewProcess, out string error, string rebackReason)
        {
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;
            try
            {
                var varData = from a in dataContxt.FM_ReviewProcess
                                   where a.SDBNo == sdbNo
                                   select a;

                string strMsg = "";

                if (varData.Count() == 1)
                {
                    FM_ReviewProcess lnqTemp = varData.Single();

                    strMsg = string.Format("{0}号文件审查流程已回退，请您重新处理单据; 回退原因为" + rebackReason, sdbNo);
                    m_billMessageServer.PassFlowMessage(sdbNo, strMsg, UniversalFunction.GetPersonnelCode(lnqTemp.Propoer), false);

                    lnqTemp.SDBStatus = "新建流程";
                    lnqTemp.Auditor = reviewProcess.Auditor;
                    lnqTemp.AuditorTime = ServerTime.Time;
                    //lnqTemp.AuditorAdvise = reviewProcess.AuditorAdvise;
                    //lnqTemp.AuditorFileUnique = reviewProcess.AuditorFileUnique;

                    dataContxt.SubmitChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;

            }
        }
    }
}
