/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  SystemFileReleaseProcess.cs
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
    /// 发布流程服务类
    /// </summary>
    class SystemFileReleaseProcess:BasicServer, ISystemFileReleaseProcess
    {
        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 文件基础信息服务组件
        /// </summary>
        ISystemFileBasicInfo m_serverFileBasicInfo = Service_Quality_File.ServerModuleFactory.GetServerModule<ISystemFileBasicInfo>();

        /// <summary>
        /// FTP服务组件
        /// </summary>
        FileServiceSocket m_serverFTP = new FileServiceSocket(GlobalObject.GlobalParameter.FTPServerIP,
            GlobalObject.GlobalParameter.FTPServerAdvancedUser,
            GlobalObject.GlobalParameter.FTPServerAdvancedPassword);

        /// <summary>
        /// 检查某业务是否存在
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">业务号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.FM_ReleaseProcess
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
            string sql = "SELECT * FROM [DepotManagement].[dbo].[FM_ReleaseProcess] where SDBNo = '" + billNo + "'";

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
        public DataTable GetAllInfo(string sdbStatus, DateTime startTime, DateTime endTime)
        {
            string strSql = "select * from View_FM_ReleaseProcess where 申请日期 >= '" + startTime + "' and 申请日期 <= '" + endTime + "'";

            if (sdbStatus != "全部")
            {
                strSql += " and 流程状态 = '" + sdbStatus + "'";
            }

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得单条流程的LNQ数据集
        /// </summary>
        /// <param name="sdbNo">流程编号</param>
        /// <returns>返回LNQ数据集</returns>
        public FM_ReleaseProcess GetInfo(string sdbNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.FM_ReleaseProcess
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
        /// 删除流程
        /// </summary>
        /// <param name="sdbNo">流程编号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        public bool DeleteProcess(string sdbNo, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                string strSql = "select SDBNo,b.FileUnique,FilePath from dbo.FM_ReleaseProcess as a " +
                    " inner join dbo.FM_FilePath as b on a.FileUnique = b.FileUnique where SDBNo = '" + sdbNo + "' ";

                DataTable tempDT = GlobalObject.DatabaseServer.QueryInfo(strSql);

                var varData = from a in ctx.FM_ReleaseProcess
                              where a.SDBNo == sdbNo
                              select a;

                ctx.FM_ReleaseProcess.DeleteAllOnSubmit(varData);

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
        /// 对输入值得LNQ进行赋值
        /// </summary>
        /// <param name="releseProcess">输入值</param>
        /// <param name="inputInfo">输出值</param>
        void AssignmentAddProcess(FM_ReleaseProcess releseProcess,ref FM_ReleaseProcess inputInfo)
        {
            inputInfo.SDBNo = releseProcess.SDBNo;
            inputInfo.SDBStatus = "等待审核";
            inputInfo.SortID = releseProcess.SortID;
            inputInfo.Remark = releseProcess.Remark;
            inputInfo.Propoer = BasicInfo.LoginName;
            inputInfo.PropoerTime = ServerTime.Time;
            inputInfo.FileUnique = releseProcess.FileUnique;
            inputInfo.FileNo = releseProcess.FileNo;
            inputInfo.FileName = releseProcess.FileName;
            inputInfo.Department = releseProcess.Department;
            inputInfo.ReplaceFileID = releseProcess.ReplaceFileID;
            inputInfo.Version = releseProcess.Version;
        }

        /// <summary>
        /// 申请流程
        /// </summary>
        /// <param name="releseProcess">LNQ数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        public bool AddProcess(FM_ReleaseProcess releseProcess,out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.FM_ReleaseProcess
                              where a.SDBNo == releseProcess.SDBNo
                              select a;


                FM_ReleaseProcess lnqTemp = new FM_ReleaseProcess();

                if (varData.Count() > 1)
                {
                    error = "数据错误";
                    return false;
                }
                else if (varData.Count() == 1)
                {
                    lnqTemp = varData.Single();
                    AssignmentAddProcess(releseProcess, ref lnqTemp);
                }
                else
                {
                    AssignmentAddProcess(releseProcess, ref lnqTemp);
                    ctx.FM_ReleaseProcess.InsertOnSubmit(lnqTemp);
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
        /// 审核流程
        /// </summary>
        /// <param name="releseProcess">LNQ数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回false </returns>
        public bool AuditProcess(FM_ReleaseProcess releseProcess, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.FM_ReleaseProcess
                              where a.SDBNo == releseProcess.SDBNo
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数据错误";
                    return false;
                }
                else
                {
                    FM_ReleaseProcess lnqTemp = varData.Single();

                    lnqTemp.SDBStatus = "等待批准";
                    lnqTemp.Auditor = BasicInfo.LoginName;
                    lnqTemp.AuditorAdvise = releseProcess.AuditorAdvise;
                    lnqTemp.AuditorTime = ServerTime.Time;
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
        /// 替换文件的FTP操作
        /// </summary>
        /// <param name="ctx">LINQ数据上下文</param>
        /// <param name="fileID">文件ID</param>
        void RepalceFile(DepotManagementDataContext ctx, int fileID)
        {

            var varDORRList = from a in ctx.FM_DistributionOfRecyclingRegisterList
                              where a.FileID == fileID
                              select a;

            m_serverFileBasicInfo.OperatorFTPSystemFile(ctx, fileID, varDORRList.Count() == 0 ? 11 : 10);
            //strVersion = (Convert.ToDouble(fileVersion) + 0.1).ToString();
        }

        /// <summary>
        /// 批准流程
        /// </summary>
        /// <param name="releseProcess">LNQ数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        public bool ApproveProcess(FM_ReleaseProcess releseProcess,out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.FM_ReleaseProcess
                              where a.SDBNo == releseProcess.SDBNo
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数据错误";
                    return false;
                }
                else
                {
                    FM_ReleaseProcess lnqTemp = varData.Single();

                    lnqTemp.SDBStatus = "流程已结束";
                    lnqTemp.Approver = BasicInfo.LoginName;
                    lnqTemp.ApproverAdvise = releseProcess.ApproverAdvise;
                    lnqTemp.ApproverTime = ServerTime.Time;

                    if (lnqTemp.ReplaceFileID == null)
                    {

                        DataTable dtTemp = m_serverFileBasicInfo.GetFilesInfo(lnqTemp.FileNo, null);

                        if (dtTemp.Rows.Count != 0)
                        {
                            RepalceFile(ctx, Convert.ToInt32(dtTemp.Rows[0]["FileID"]));
                        }
                    }
                    else
                    {

                        RepalceFile(ctx, Convert.ToInt32(lnqTemp.ReplaceFileID));
                    }


                    FM_FileList lnqFile = new FM_FileList();

                    lnqFile.Department = lnqTemp.Department;
                    lnqFile.FileName = lnqTemp.FileName;
                    lnqFile.FileNo = lnqTemp.FileNo;
                    lnqFile.FileUnique = lnqTemp.FileUnique;
                    lnqFile.SortID = lnqTemp.SortID;
                    lnqFile.Version = lnqTemp.Version;

                    ctx.FM_FileList.InsertOnSubmit(lnqFile);

                    if (m_serverFTP.Errormessage.Length != 0)
                    {
                        throw new Exception(m_serverFTP.Errormessage);
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
        /// <param name="releaseProcess">数据集</param>
        /// <param name="error">错误信息</param>
        /// <param name="rebackReason">回退原因</param>
        /// <returns>成功返回True,失败返回False</returns>
        public bool ReturnBill(string sdbNo, string sdbStatus, FM_ReleaseProcess releaseProcess, out string error, string rebackReason)
        {
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;
            try
            {
                var varData = from a in dataContxt.FM_ReleaseProcess
                              where a.SDBNo == sdbNo
                              select a;

                string strMsg = "";

                if (varData.Count() == 1)
                {
                    FM_ReleaseProcess lnqTemp = varData.Single();

                    lnqTemp.SDBStatus = sdbStatus;

                    switch (sdbStatus)
                    {
                        case "新建流程":

                            strMsg = string.Format("{0}号文件审查流程已回退，请您重新处理单据; 回退原因为" + rebackReason, sdbNo);
                            m_billMessageServer.PassFlowMessage(sdbNo, strMsg, UniversalFunction.GetPersonnelCode(lnqTemp.Propoer), false);

                            //lnqTemp.AuditorAdvise = releaseProcess.AuditorAdvise;
                            //lnqTemp.ApproverAdvise = releaseProcess.ApproverAdvise;
                            break;
                        case "等待审核":

                            strMsg = string.Format("{0}号文件审查流程已回退，请您重新处理单据; 回退原因为" + rebackReason, sdbNo);
                            m_billMessageServer.PassFlowMessage(sdbNo, strMsg, UniversalFunction.GetPersonnelCode(lnqTemp.Auditor), false);

                            //lnqTemp.AuditorAdvise = releaseProcess.AuditorAdvise;
                            //lnqTemp.ApproverAdvise = releaseProcess.ApproverAdvise;
                            break;
                        default:
                            break;
                    }
                }


                dataContxt.SubmitChanges();

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
