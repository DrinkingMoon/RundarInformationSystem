/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  SystemFileRevisedAbolishedBill.cs
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
    /// 文件修订废止申请单服务组件
    /// </summary>
    class SystemFileRevisedAbolishedBill : ISystemFileRevisedAbolishedBill
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
            var varData = from a in ctx.FM_RevisedAbolishedBill
                          where a.BillNo == billNo
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
            string sql = "SELECT * FROM [DepotManagement].[dbo].[FM_RevisedAbolishedBill] where BillNo = '" + billNo + "'";

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
            string strSql = "select * from View_FM_RevisedAbolishedBill where 申请日期 >= '" + startTime + "' and 申请日期 <= '" + endTime + "'";

            if (sdbStatus != "全部")
            {
                strSql += " and 单据状态 = '" + sdbStatus + "'";
            }

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得单条LINQ数据集
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回LINQ数据集</returns>
        public FM_RevisedAbolishedBill GetInfo(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.FM_RevisedAbolishedBill
                          where a.BillNo == billNo
                          select a;

            if (varData.Count() != 1)
            {
                return null;
            }
            else
            {
                return varData.Single();
            }
        }

        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        public bool DeleteInfo(string billNo, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.FM_RevisedAbolishedBill
                              where a.BillNo == billNo
                              select a;

                ctx.FM_RevisedAbolishedBill.DeleteAllOnSubmit(varData);
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
        /// 赋值
        /// </summary>
        /// <param name="raBill">输入值</param>
        /// <param name="inputInfo">输出值</param>
        void AssignmentInfo(FM_RevisedAbolishedBill raBill, ref FM_RevisedAbolishedBill inputInfo)
        {
            inputInfo.BillNo = raBill.BillNo;
            inputInfo.BillStatus = "等待审核";
            inputInfo.FileID = raBill.FileID;
            inputInfo.OperationFlag = raBill.OperationFlag;
            inputInfo.Propose = BasicInfo.LoginName;
            inputInfo.ProposeContent = raBill.ProposeContent;
            inputInfo.ProposeTime = ServerTime.Time;
        }

        /// <summary>
        /// 申请流程
        /// </summary>
        /// <param name="releseProcess">LNQ数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        public bool AddInfo(FM_RevisedAbolishedBill raBill, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.FM_RevisedAbolishedBill
                              where a.BillNo == raBill.BillNo
                              select a;

                FM_RevisedAbolishedBill lnqTemp = new FM_RevisedAbolishedBill();

                if (varData.Count() > 1)
                {
                    error = "数据错误";
                    return false;
                }
                else if (varData.Count() == 1)
                {
                    lnqTemp = varData.Single();
                    AssignmentInfo(raBill, ref lnqTemp);
                }
                else
                {
                    AssignmentInfo(raBill, ref lnqTemp);
                    ctx.FM_RevisedAbolishedBill.InsertOnSubmit(lnqTemp);
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
        public bool AuditInfo(FM_RevisedAbolishedBill raBill, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.FM_RevisedAbolishedBill
                              where a.BillNo == raBill.BillNo
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数据错误";
                    return false;
                }
                else
                {
                    FM_RevisedAbolishedBill lnqTemp = varData.Single();

                    lnqTemp.BillStatus = "等待批准";
                    lnqTemp.Auditor = BasicInfo.LoginName;
                    lnqTemp.AuditorAdvise = raBill.AuditorAdvise;
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
        /// 批准流程
        /// </summary>
        /// <param name="releseProcess">LNQ数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        public bool ApproveInfo(FM_RevisedAbolishedBill raBill, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.FM_RevisedAbolishedBill
                              where a.BillNo == raBill.BillNo
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数据错误";
                    return false;
                }
                else
                {
                    FM_RevisedAbolishedBill lnqTemp = varData.Single();

                    lnqTemp.BillStatus = "单据已完成";
                    lnqTemp.ApproverAdvise = raBill.ApproverAdvise;
                    lnqTemp.Approver = BasicInfo.LoginName;
                    lnqTemp.ApproverTime = ServerTime.Time;

                    if (lnqTemp.OperationFlag)
                    {
                        var varRegisterList = from a in ctx.FM_DistributionOfRecyclingRegisterList
                                              where a.FileID == lnqTemp.FileID 
                                              && a.RecoverPersonnel != null
                                              && a.RecoverPersonnel.Trim().Length != 0
                                              select a;

                        if (varRegisterList.Count() != 0)
                        {
                            m_serverFileBasicInfo.OperatorFTPSystemFile(ctx, lnqTemp.FileID, 10);
                        }
                        else
                        {
                            m_serverFileBasicInfo.OperatorFTPSystemFile(ctx, lnqTemp.FileID, 11);
                        }
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
        public bool ReturnBill(string billNo, string billStatus, FM_RevisedAbolishedBill revisedAbolishedBill, out string error, string rebackReason)
        {
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;
            try
            {
                var varData = from a in dataContxt.FM_RevisedAbolishedBill
                              where a.BillNo == billNo
                              select a;

                string strMsg = "";

                if (varData.Count() == 1)
                {
                    FM_RevisedAbolishedBill lnqTemp = varData.Single();

                    lnqTemp.BillStatus = billStatus;

                    switch (billStatus)
                    {
                        case "新建单据":

                            strMsg = string.Format("{0}号文件审查流程已回退，请您重新处理单据; 回退原因为" + rebackReason, billNo);
                            m_billMessageServer.PassFlowMessage(billNo, strMsg, UniversalFunction.GetPersonnelCode(lnqTemp.Propose), false);

                            //lnqTemp.AuditorAdvise = revisedAbolishedBill.AuditorAdvise;
                            //lnqTemp.ApproverAdvise = revisedAbolishedBill.ApproverAdvise;
                            break;
                        case "等待审核":

                            strMsg = string.Format("{0}号文件审查流程已回退，请您重新处理单据; 回退原因为" + rebackReason, billNo);
                            m_billMessageServer.PassFlowMessage(billNo, strMsg, UniversalFunction.GetPersonnelCode(lnqTemp.Auditor), false);

                            //lnqTemp.AuditorAdvise = revisedAbolishedBill.AuditorAdvise;
                            //lnqTemp.ApproverAdvise = revisedAbolishedBill.ApproverAdvise;
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
