/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  CheckReturnRepair.cs
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
    /// 挑选返工返修服务组件
    /// </summary>
    class CheckReturnRepair : BasicServer, ServerModule.ICheckReturnRepair
    {
        /// <summary>
        /// 单据编号分配服务
        /// </summary>
        IAssignBillNoServer m_assignBill = ServerModuleFactory.GetServerModule<IAssignBillNoServer>();

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 创建挑返单
        /// </summary>
        /// <param name="djh">报检单单据号</param>
        /// <param name="logID">挑返单创建人工号</param>
        /// <param name="error">出错时返回错误信息</param>
        /// <param name="tfDJH">挑返单单据号</param>
        /// <returns>操作是否成功的标志</returns>
        public bool Create(string djh, string logID, out string error, out string tfDJH)
        {
            error = null;

            tfDJH = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var varData = from a in dataContxt.S_CheckReturnRepairBill
                              where a.InDepotBillID == djh && a.DJZT != "单据已报废"
                              select a;

                if (varData.Count() > 0)
                {
                    error = "此批次的挑选返工返修单已存在";
                    return false;
                }


                S_CheckReturnRepairBill lnqReturn = new S_CheckReturnRepairBill();

                lnqReturn.DJH = m_assignBill.AssignNewNo(this, CE_BillTypeEnum.挑选返工返修单.ToString());
                lnqReturn.InDepotBillID = djh;
                lnqReturn.DJZT = "新建单据";
                lnqReturn.CJRQ = ServerTime.Time;
                lnqReturn.CJRY = logID;

                dataContxt.S_CheckReturnRepairBill.InsertOnSubmit(lnqReturn);

                dataContxt.SubmitChanges();

                tfDJH = lnqReturn.DJH;

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 检查某业务是否存在
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">业务号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.S_CheckReturnRepairBill
                          where a.DJH == billNo
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
            string sql = "SELECT * FROM [DepotManagement].[dbo].[S_CheckReturnRepairBill] where DJH = '" + billNo + "'";

            System.Data.DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取全部单据
        /// </summary>
        /// <param name="billStatus">单据状态,若为“全  部”则显示所有单据信息</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>返回查询到的挑返单的单据信息</returns>
        public DataTable GetAllBill(string billStatus, DateTime startTime, DateTime endTime)
        {
            string strSelect = "";

            if (billStatus != "全  部")
            {
                strSelect += "单据状态 = '" + billStatus + "' and ";
            }

            strSelect += "创建日期 >= '" + startTime + "' and 创建日期 <= '" + endTime + "'";

            string strSql = "select * from View_S_CheckReturnRepairBill where " + strSelect + " order by 单据号 desc";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得单条数据
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <returns>返回一条挑返单的单据信息</returns>
        public DataRow GetData(string djh)
        {
            string strSql = "select * from View_S_CheckReturnRepairBill where 单据号 = '" + djh + "'";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dtTemp.Rows.Count != 1)
            {
                return null;
            }
            else
            {
                return dtTemp.Rows[0];
            }
        }

        /// <summary>
        /// 编制人提交单据
        /// </summary>
        /// <param name="inReturn">Linq挑返单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>处理成功返回True，处理失败返回False</returns>
        public bool SubmitBill(S_CheckReturnRepairBill inReturn, out string error)
        {
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                var varData = from a in dataContxt.S_CheckReturnRepairBill
                              where a.DJH == inReturn.DJH
                              select a;

                if (varData.Count() != 1)
                {
                    error = "记录不唯一";
                    return false;
                }
                else
                {
                    S_CheckReturnRepairBill lnqReturn = varData.Single();

                    lnqReturn.ReturnReason = inReturn.ReturnReason;
                    lnqReturn.ReturnMeansAndAsk = inReturn.ReturnMeansAndAsk;
                    lnqReturn.ReturnManHour = inReturn.ReturnManHour;
                    lnqReturn.SQE_Hour = inReturn.SQE_Hour;
                    lnqReturn.DJZT = "等待处理结果";
                    lnqReturn.SQERY = inReturn.SQERY;
                    lnqReturn.SQERQ = ServerTime.Time;
                    lnqReturn.QC_BFS = 0;
                    lnqReturn.QC_HGS = 0;
                    lnqReturn.QC_RBS = 0;
                    lnqReturn.QC_THS = 0;
                    lnqReturn.SQE_BHGS = 0;
                    lnqReturn.SQE_HGS = 0;
                    lnqReturn.SQEJGRQ = null;
                    lnqReturn.SQEJGRY = null;
                    lnqReturn.SHRQ = null;
                    lnqReturn.SHRY = null;
                    lnqReturn.QCRQ = null;
                    lnqReturn.QCRY = null;

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

        /// <summary>
        /// 操作数据库
        /// </summary>
        /// <param name="bill">Linq挑返单的数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>处理成功返回True，处理失败返回False</returns>
        public bool UpdateBill(S_CheckReturnRepairBill bill, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var varData = from a in dataContxt.S_CheckReturnRepairBill
                              where a.DJH == bill.DJH
                              select a;

                if (varData.Count() != 1)
                {
                    error = "记录不唯一";
                    return false;
                }
                else
                {
                    S_CheckReturnRepairBill lnqReturn = varData.Single();

                    if (bill.DJZT != lnqReturn.DJZT)
                    {
                        error = "单据状态错误，请重新刷新单据确认单据状态";
                        return false;
                    }

                    switch (lnqReturn.DJZT)
                    {
                        case "等待处理结果":
                            lnqReturn.SQE_BHGS = bill.SQE_BHGS;
                            lnqReturn.SQE_HGS = bill.SQE_HGS;
                            lnqReturn.DJZT = "等待审批确认";
                            lnqReturn.SQEJGRY = bill.SQEJGRY;
                            lnqReturn.SQEJGRQ = ServerTime.Time;
                            break;

                        case "等待审批确认":
                            lnqReturn.DJZT = "等待检验结果";
                            lnqReturn.SHRY = bill.SHRY;
                            lnqReturn.SHRQ = ServerTime.Time;
                            break;

                        case "等待检验结果":

                            if (UpdateInDepotBill(dataContxt, bill, out error))
                            {
                                lnqReturn.QC_BFS = bill.QC_BFS;
                                lnqReturn.QC_HGS = bill.QC_HGS;
                                lnqReturn.QC_RBS = bill.QC_RBS;
                                lnqReturn.QC_THS = bill.QC_THS;
                                lnqReturn.DJZT = "单据已完成";
                                lnqReturn.QCRY = bill.QCRY;
                                lnqReturn.QCRQ = ServerTime.Time;
                            }
                            else
                            {
                                return false;
                            }

                            break;
                        default:
                            break;
                    }

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

        /// <summary>
        /// 更新报检入库单（记录合格数、报废数、退货数等，并更改单据状态）
        /// </summary>
        /// <param name="dataContext">LINQ数据上下文</param>
        /// <param name="bill">Linq挑返单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>更改成功返回True，更改失败返回False</returns>
        private bool UpdateInDepotBill(DepotManagementDataContext dataContext, S_CheckReturnRepairBill bill, out string error)
        {
            try
            {
                error = null;

                var varData = from a in dataContext.S_CheckOutInDepotBill
                              where a.Bill_ID == bill.InDepotBillID
                              select a;

                if (varData.Count() != 1)
                {
                    error = "记录不唯一";
                    return false;
                }
                else
                {
                    S_CheckOutInDepotBill lnqCheck = varData.Single();

                    lnqCheck.EligibleCount = (int)bill.QC_HGS;
                    lnqCheck.ConcessionCount = (int)bill.QC_RBS;
                    lnqCheck.ReimbursementCount = (int)bill.QC_THS;
                    lnqCheck.DeclareWastrelCount = (int)bill.QC_BFS;
                    lnqCheck.BillStatus = "等待入库";
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
        /// 报废单据
        /// </summary>
        /// <param name="inReturn">Linq挑返单数据集</param>
        /// <param name="flag">标志 True 等待质检机检验 False 等待质检电检验</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>报废成功返回True，报废失败返回False</returns>
        public bool ScrapBill(S_CheckReturnRepairBill inReturn, bool flag, out string error)
        {
            error = null;

            try
            {

                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var varDataFst = from a in dataContxt.S_CheckReturnRepairBill
                                 where a.DJH == inReturn.DJH
                                 select a;

                if (varDataFst.Count() != 1)
                {
                    error = "记录不唯一";
                    return false;
                }
                else
                {
                    dataContxt.S_CheckReturnRepairBill.DeleteAllOnSubmit(varDataFst);
                }

                var varDataSec = from a in dataContxt.S_CheckOutInDepotBill
                                 where a.Bill_ID == inReturn.InDepotBillID
                                 select a;

                if (varDataSec.Count() != 1)
                {
                    error = "记录不唯一";
                    return false;
                }
                else
                {
                    S_CheckOutInDepotBill lnqCheck = varDataSec.Single();

                    lnqCheck.EligibleCount = 0;
                    lnqCheck.ConcessionCount = 0;
                    lnqCheck.ReimbursementCount = 0;
                    lnqCheck.DeclareWastrelCount = 0;
                    lnqCheck.BillStatus = flag == true ? "等待质检机检验" : "等待质检电检验";
                    lnqCheck.TFFlag = false;
                    lnqCheck.QualityInputer = null;
                    lnqCheck.CheckTime = null;
                    lnqCheck.CheckoutJoinGoods_Time = null;
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

        /// <summary>
        /// 回退单据
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <param name="billStatus">操作类型(单据状态)</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <param name="rebackReason">回退原因</param>
        /// <returns>回退成功返回True，回退失败返回False</returns>
        public bool ReturnBill(string djh, string billStatus, out string error, string rebackReason)
        {
            error = null;
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                var varData = from a in dataContxt.S_CheckReturnRepairBill
                              where a.DJH == djh
                              select a;

                string strMsg = "";

                if (varData.Count() == 1)
                {
                    S_CheckReturnRepairBill lnqReturn = varData.Single();

                    switch (billStatus)
                    {
                        case "新建单据":

                            strMsg = string.Format("{0}号挑选返工返修单已回退，请您重新处理单据; 回退原因为" + rebackReason, djh);
                            m_billMessageServer.PassFlowMessage(djh, strMsg, lnqReturn.SQERY, false);

                            lnqReturn.DJZT = "新建单据";
                            lnqReturn.SHRY = null;
                            lnqReturn.SHRQ = null;
                            lnqReturn.SQERQ = null;
                            lnqReturn.SQERY = null;
                            lnqReturn.QCRQ = null;
                            lnqReturn.QCRY = null;

                            break;
                        case "等待处理结果":

                            strMsg = string.Format("{0}号挑选返工返修单已回退，请您重新处理单据; 回退原因为" + rebackReason, djh);
                            m_billMessageServer.PassFlowMessage(djh, strMsg, lnqReturn.SQEJGRY, false);

                            lnqReturn.DJZT = "等待处理结果";
                            lnqReturn.SHRY = null;
                            lnqReturn.SHRQ = null;
                            lnqReturn.SQERQ = null;
                            lnqReturn.SQERY = null;
                            lnqReturn.QCRQ = null;
                            lnqReturn.QCRY = null;

                            break;
                        case "等待审批确认":

                            strMsg = string.Format("{0}号挑选返工返修单已回退，请您重新处理单据; 回退原因为" + rebackReason, djh);
                            m_billMessageServer.PassFlowMessage(djh, strMsg, lnqReturn.SHRY, false);

                            lnqReturn.DJZT = "等待审批确认";
                            lnqReturn.SHRY = null;
                            lnqReturn.SHRQ = null;
                            lnqReturn.QCRQ = null;
                            lnqReturn.QCRY = null;
                            break;

                        default:
                            break;
                    }

                    dataContxt.SubmitChanges();
                    return true;
                }
                else
                {
                    error = "数据集不唯一";
                    return false;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;

            }
        }

        #region 夏石友，2012-07-18，将报检入库单中的此功能移动到此，原方法名：ScrapAllBill

        /// <summary>
        /// 报废入库单单号对应的所有挑返单
        /// </summary>
        /// <param name="inDepotBillID">入库单单号</param>
        /// <param name="error">出错时返回错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        public bool ScrapAllBill(string inDepotBillID, out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var varData = from a in dataContxt.S_CheckReturnRepairBill
                              where a.InDepotBillID == inDepotBillID
                              select a;

                IBillMessagePromulgatorServer msgServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

                foreach (var item in varData)
                {
                    S_CheckReturnRepairBill lnqReturn = item;

                    lnqReturn.DJZT = "单据已报废";

                    msgServer.DestroyMessage(item.DJH);
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

        #endregion
    }
}
