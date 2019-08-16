/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IsolationManageBill.cs
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
    /// 不合格品隔离单据管理类
    /// </summary>
    class IsolationManageBill:BasicServer, ServerModule.IIsolationManageBill
    {
        /// <summary>
        /// 库存信息服务组件
        /// </summary>
        IStoreServer m_serverStore = ServerModuleFactory.GetServerModule<IStoreServer>();

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 单据编号分配服务
        /// </summary>
        IAssignBillNoServer m_assignBill = ServerModuleFactory.GetServerModule<IAssignBillNoServer>();

        /// <summary>
        /// 清除隔离单数据
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="billNo">关联的采购退货单单号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>清除成功返回True，清除失败返回False</returns>
        public bool ClearBillDate(DepotManagementDataContext context, string billNo, out string error)
        {
            error = null;

            try
            {
                var varData = from a in context.S_IsolationManageBill
                              where a.AssociateRejectBillID == billNo
                              select a;

                if (varData.Count() > 0)
                {
                    foreach (var item in varData)
                    {
                        S_IsolationManageBill lnqIsolation = item;

                        lnqIsolation.DJZT = "等待采购退货";
                        lnqIsolation.AssociateRejectBillID = null;
                    }
                }

            }
            catch (Exception ex)
            {
                error = ex.Message;

                return false;
                throw;
            }

            return true;
        }

        /// <summary>
        /// 检查某业务是否存在
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">业务号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.S_IsolationManageBill
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
            string sql = "SELECT * FROM [DepotManagement].[dbo].[S_IsolationManageBill] where DJH = '" + billNo + "'";

            System.Data.DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 获取全部单据信息
        /// </summary>
        /// <param name="strSelect">选择信息</param>
        /// <returns>返回隔离单单据信息</returns>
        public List<View_S_IsolationManageBill> GetAllBill(string strSelect)
        {
            if (strSelect == null || strSelect == "")
            {
                strSelect = "";
            }
            else
            {
                strSelect = " and " + strSelect;
            }

            string strSql = "select * from View_S_IsolationManageBill where  1 = 1 " + strSelect + " order by 单据号 desc";

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            return ctx.ExecuteQuery<View_S_IsolationManageBill>(strSql, new object[] { }).ToList();
        }

        /// <summary>
        /// 新建单据
        /// </summary>
        /// <param name="isolation">不合格品隔离处置单信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool InsertNewDate(S_IsolationManageBill isolation, out string error)
        {
            error = null;
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                var varData = from a in dataContxt.S_IsolationManageBill
                              where a.DJH == isolation.DJH
                              select a;

                if (varData.Count() == 0)
                {
                    if (!m_serverStore.ChangeStockStatus(dataContxt, isolation, 3, out error))
                    {
                        return false;
                    }

                    dataContxt.S_IsolationManageBill.InsertOnSubmit(isolation);
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
        /// 提交单据（可重复完成）
        /// </summary>
        /// <param name="isolation">不合格品隔离处置单信息</param>
        /// <param name="flag">标志 True 等待隔离原因 False 等待主管审核</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool UpdateBill(S_IsolationManageBill isolation, bool flag, out string error)
        {
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                if (isolation.DJZT == "新建单据")
                {
                    if (!InsertNewDate(isolation, out error))
                    {
                        return false;
                    }
                }

                var varData = from a in dataContxt.S_IsolationManageBill
                              where a.DJH == isolation.DJH
                              select a;

                if (varData.Count() != 1)
                {
                    error = "记录不唯一";
                    return false;
                }
                else
                {
                    S_IsolationManageBill lnqIsolation = varData.Single();

                    lnqIsolation.DJZT = flag == true ? "等待隔离原因": "等待主管审核";
                    lnqIsolation.Amount = isolation.Amount;
                    lnqIsolation.GoodsID = isolation.GoodsID;
                    lnqIsolation.BatchNo = isolation.BatchNo;
                    lnqIsolation.StorageID = isolation.StorageID;
                    lnqIsolation.Provider = isolation.Provider;

                    if (!flag)
                    {
                        lnqIsolation.IsolateMeansAndAsk = isolation.IsolateMeansAndAsk;
                        lnqIsolation.IsolateReason = isolation.IsolateReason;
                    }

                    lnqIsolation.CLBM = isolation.CLBM;
                    lnqIsolation.LRRY = BasicInfo.LoginID;
                    lnqIsolation.LRRQ = ServerTime.Time;
                    lnqIsolation.QC_FQS = 0;
                    lnqIsolation.QC_BFS = 0;
                    lnqIsolation.QC_HGS = 0;
                    lnqIsolation.QC_RBS = 0;
                    lnqIsolation.QC_THS = 0;
                    lnqIsolation.SQE_BHGS = 0;
                    lnqIsolation.SQE_CLGS = 0;
                    lnqIsolation.SQE_HGS = 0;
                    lnqIsolation.SHRQ = null;
                    lnqIsolation.SHRY = null;
                    lnqIsolation.DCRQ = null;
                    lnqIsolation.DCRY = null;
                    lnqIsolation.DRRQ = null;
                    lnqIsolation.DRRY = null;
                    lnqIsolation.CLRQ = null;
                    lnqIsolation.CLRY = null;
                    lnqIsolation.JYRQ = null;
                    lnqIsolation.JYRY = null;
                    lnqIsolation.SQETHRQ = null;
                    lnqIsolation.SQETHRY = null;

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
        /// 将隔离品调入库房，确认解除隔离
        /// </summary>
        /// <param name="billID">隔离单号</param>
        /// <param name="message">否认说明</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool AffrimBill(string billID,string message,out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var varData = from a in dataContxt.S_IsolationManageBill
                              where a.DJH == billID
                              select a;

                if (varData.Count() != 0)
                {
                    error = "数据不唯一或者为空";
                    return false;
                }
                else
                {
                    S_IsolationManageBill lnqBill = varData.Single();

                    lnqBill.DJZT = "等待处理结果";
                    lnqBill.QRRQ = ServerTime.Time;
                    lnqBill.QRRY = BasicInfo.LoginID;
                    lnqBill.QRSM = message;
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
        /// 提交信息至数据库，按流程更新单据状态
        /// </summary>
        /// <param name="needBillStatus">要求的单据状态</param>
        /// <param name="isolation">单据信息</param>
        /// <param name="error">出现错误时返回的错误信息，没有错误返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool UpdateBill(string needBillStatus, S_IsolationManageBill isolation, out string error)
        {
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            dataContxt.Connection.Open();
            dataContxt.Transaction = dataContxt.Connection.BeginTransaction();

            try
            {
                var varData = from a in dataContxt.S_IsolationManageBill
                              where a.DJH == isolation.DJH
                              select a;
                
                if (varData.Count() != 1)
                {
                    error = "记录不唯一";
                    return false;
                }
                else
                {
                    S_IsolationManageBill lnqIsolation = varData.Single();

                    if (lnqIsolation.DJZT != needBillStatus)
                    {
                        error = "单据当前状态为 [" + lnqIsolation.DJZT + "] 与要求的单据状态 [" + needBillStatus + "] 不一致";
                        return false;
                    }

                    if (lnqIsolation.DJZT != isolation.DJZT)
                    {
                        error = "单据状态错误，请重新刷新单据确认单据状态";
                        return false;
                    }

                    switch (lnqIsolation.DJZT)
                    {
                        case "等待隔离原因":
                            lnqIsolation.IsolateMeansAndAsk = isolation.IsolateMeansAndAsk;
                            lnqIsolation.IsolateReason = isolation.IsolateReason;

                            if (isolation.IsOutsourcing)
                            {
                                lnqIsolation.DJZT = "等待领料";
                            }
                            else
                            {
                                lnqIsolation.DJZT = "等待主管审核";
                            }
                            
                            lnqIsolation.QERQ = ServerTime.Time;
                            lnqIsolation.QERY = BasicInfo.LoginID;
                            lnqIsolation.IsOutsourcing = isolation.IsOutsourcing;
                            break;

                        case "等待主管审核":
                            lnqIsolation.DJZT = "等待仓管调出";
                            lnqIsolation.SHRY = BasicInfo.LoginID;
                            lnqIsolation.SHRQ = ServerTime.Time;
                            break;

                        case "等待仓管调出":
                            lnqIsolation.DJZT = "等待处理结果";
                            lnqIsolation.DCRY = BasicInfo.LoginID;
                            lnqIsolation.DCRQ = ServerTime.Time;
                            break;

                        case "等待处理结果":
                            lnqIsolation.DJZT = "等待质检结果";
                            lnqIsolation.CLRY = BasicInfo.LoginID;
                            lnqIsolation.CLRQ = ServerTime.Time;
                            lnqIsolation.RejectMode = isolation.RejectMode;
                            lnqIsolation.SQE_BHGS = isolation.SQE_BHGS;
                            lnqIsolation.SQE_CLGS = isolation.SQE_CLGS;
                            lnqIsolation.SQE_HGS = isolation.SQE_HGS;
                            break;

                        case "等待质检结果":
                            lnqIsolation.DJZT = "等待质管主管确认";
                            lnqIsolation.JYRY = BasicInfo.LoginID;
                            lnqIsolation.JYRQ = ServerTime.Time;
                            lnqIsolation.QC_FQS = isolation.QC_FQS;
                            lnqIsolation.QC_BFS = isolation.QC_BFS;
                            lnqIsolation.QC_HGS = isolation.QC_HGS;
                            lnqIsolation.QC_RBS = isolation.QC_RBS;
                            lnqIsolation.QC_THS = isolation.QC_THS;
                            break;

                        case "等待质管主管确认":

                            if (isolation.QC_THS > 0)
                            {
                                lnqIsolation.DJZT = "等待采购退货";
                            }
                            else
                            {
                                lnqIsolation.DJZT = "等待仓管调入";
                            }

                            lnqIsolation.QRRY = BasicInfo.LoginID;
                            lnqIsolation.QRRQ = ServerTime.Time;
                            lnqIsolation.QRSM = isolation.QRSM;
                            break;

                        case "等待仓管调入":

                            if (lnqIsolation.DJZT == "单据已完成")
                            {
                                error = "单据不能重复确认";
                                return false;
                            }

                            lnqIsolation.DJZT = "单据已完成";
                            lnqIsolation.DRRY = BasicInfo.LoginID;
                            lnqIsolation.DRRQ = ServerTime.Time;

                            if (!m_serverStore.ChangeStockStatus(dataContxt, isolation, 0, out error))
                            {
                                return false;
                            }

                            if ((decimal)lnqIsolation.QC_FQS > 0)
                            {
                                if (!CreateMeterialRequisition(dataContxt, isolation,"废弃数", out error))
                                {
                                    return false;
                                }
                            }

                            if ((decimal)lnqIsolation.QC_BFS > 0)
                            {
                                if (!CreateMeterialRequisition(dataContxt, isolation, "报废数", out error))
                                {
                                    return false;
                                }
                            }

                            break;

                        case "单据已完成":
                            lnqIsolation.RejectMode = isolation.RejectMode;
                            lnqIsolation.SQETHRY = BasicInfo.LoginID;
                            lnqIsolation.SQETHRQ = ServerTime.Time;
                            break;

                        default:
                            break;
                    }

                }

                dataContxt.SubmitChanges();
                dataContxt.Transaction.Commit();
            }
            catch (Exception ex)
            {
                error = ex.Message;
                dataContxt.Transaction.Rollback();
                return false;
            }

            return true;
        }

        /// <summary>
        /// 自动生成领料单
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="isolation">隔离单单据信息</param>
        /// <param name="flag">是否为废弃数，若为废弃数则用“废弃数”表示</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>自动生成成功返回True，自动生成失败返回False</returns>
        private bool CreateMeterialRequisition(DepotManagementDataContext context,
            S_IsolationManageBill isolation, string flag, out string error)
        {
            error = null;

            MaterialRequisitionServer serverMaterialBill = new MaterialRequisitionServer();

            try
            {
                string strBillID = m_assignBill.AssignNewNo(serverMaterialBill,CE_BillTypeEnum.领料单.ToString());

                var varIsolation = from a in context.S_IsolationManageBill
                                   where a.DJH == isolation.DJH
                                   select a;

                S_IsolationManageBill lnqNewIsolation = new S_IsolationManageBill();

                if (varIsolation.Count() != 1)
                {
                    error = "数据不唯一或者为空";
                    return false;
                }
                else
                {
                    lnqNewIsolation = varIsolation.Single();
                }

                #region 领料总单
                S_MaterialRequisition lnqMater = new S_MaterialRequisition();

                lnqMater.Bill_Time = ServerTime.Time;
                lnqMater.Bill_ID = strBillID;
                lnqMater.BillStatus = "已出库";
                lnqMater.Department = "ZK";
                lnqMater.DepartmentDirector = "";
                lnqMater.DepotManager = "";
                lnqMater.FetchCount = 0;
                lnqMater.FetchType = "零星领料";
                lnqMater.FillInPersonnel = flag == "废弃数" ? "陈岁年" : UniversalFunction.GetPersonnelName(lnqNewIsolation.JYRY);
                //质管部要求[废弃数]变更编制人为陈岁年 2012.3.15
                lnqMater.FillInPersonnelCode = flag == "废弃数" ? "0621" : lnqNewIsolation.JYRY;
                //质管部要求[废弃数]变更编制人为陈岁年 2012.3.15
                lnqMater.OutDepotDate = ServerTime.Time;
                lnqMater.ProductType = "";
                lnqMater.PurposeCode = UniversalFunction.GetPurpose(CE_PickingPurposeProperty.破坏性检测).Code;
                lnqMater.Remark = flag == "废弃数" ?
                    "不合格品隔离处置单废弃处理，关联的隔离单号:  " + isolation.DJH
                    : "不合格品隔离处置单检测报废，关联的隔离单号:  " + isolation.DJH;
                lnqMater.StorageID = isolation.StorageID;
                lnqMater.AssociatedBillNo = "";
                lnqMater.AssociatedBillType = "";

                if (!serverMaterialBill.AutoCreateBill(context, lnqMater, out error))
                {
                    return false;
                }

                #endregion 

                #region 领料单明细
                S_MaterialRequisitionGoods lnqMaterGoods = new S_MaterialRequisitionGoods();

                var varMaterialStock = from a in context.S_Stock
                                       where a.GoodsID == isolation.GoodsID
                                       && a.BatchNo == isolation.BatchNo
                                       && a.StorageID == isolation.StorageID
                                       && a.Provider == isolation.Provider
                                       select a;

                S_Stock lnqStock = new S_Stock();

                if (varMaterialStock.Count() != 1)
                {
                    error = "数据不唯一或者为空";
                    return false;
                }
                else
                {
                    lnqStock = varMaterialStock.Single();
                }

                lnqMaterGoods.BasicCount = 0;
                lnqMaterGoods.BatchNo = isolation.BatchNo;
                lnqMaterGoods.Bill_ID = strBillID;
                lnqMaterGoods.GoodsID = (int)isolation.GoodsID;
                lnqMaterGoods.ProviderCode = isolation.Provider;
                lnqMaterGoods.RealCount = flag == "废弃数" ? isolation.QC_FQS : isolation.QC_BFS;
                lnqMaterGoods.Remark = flag == "废弃数" ?
                    "不合格品隔离处置单废弃处理，关联的隔离单号:  " + isolation.DJH
                    : "不合格品隔离处置单检测报废，关联的隔离单号:  " + isolation.DJH;
                lnqMaterGoods.RequestCount = flag == "废弃数" ? isolation.QC_FQS : isolation.QC_BFS;
                lnqMaterGoods.ShowPosition = 1;

                MaterialRequisitionGoodsServer serverMaterialGoods = new MaterialRequisitionGoodsServer();

                if (!serverMaterialGoods.AutoCreateGoods(context, lnqMaterGoods, out error))
                {
                    return false;
                }

                context.SubmitChanges();

                #endregion

                serverMaterialBill.OpertaionDetailAndStock(context, lnqMater);
                context.SubmitChanges();

                m_assignBill.UseBillNo(CE_BillTypeEnum.领料单.ToString(), strBillID);

                return true;

            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
                
            }
        }

        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        public bool ScrapBill(string billNo, out string error)
        {
            error = null;
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                var varIsolation = from a in dataContxt.S_IsolationManageBill
                                   where a.DJH == billNo
                                   select a;

                if (varIsolation.Count() != 1)
                {
                    error = "记录不唯一";
                    return false;
                }
                else
                {
                    S_IsolationManageBill Isalation = varIsolation.Single();
                    dataContxt.S_IsolationManageBill.DeleteAllOnSubmit(varIsolation);

                    if (!m_serverStore.ChangeStockStatus(dataContxt, Isalation, 0, out error))
                    {
                        return false;
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
        /// 回退单据
        /// </summary>
        /// <param name="djh">隔离单单号</param>
        /// <param name="billStatus">回退后的单据状态</param>
        /// <param name="isolation">隔离单单据信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <param name="rebackReason">回退原因</param>
        /// <returns>回退成功返回True，回退失败返回False</returns>
        public bool ReturnBill(string djh, string billStatus,
            S_IsolationManageBill isolation, out string error, string rebackReason)
        {
            error = null;
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                var varIsolation = from a in dataContxt.S_IsolationManageBill
                              where a.DJH == djh
                              select a;
                string strMsg = "";

                if (varIsolation.Count() == 1)
                {
                    S_IsolationManageBill lnqIsolation = varIsolation.Single();

                    switch (billStatus)
                    {
                        case "新建单据":

                            strMsg = string.Format("{0}号不合格品隔离处置单已回退，请您重新处理单据; 回退原因为" + rebackReason, djh);
                            m_billMessageServer.PassFlowMessage(djh, strMsg,lnqIsolation.LRRY, false);

                            lnqIsolation.DJZT = "新建单据";
                            lnqIsolation.SHRQ = null;
                            lnqIsolation.SHRY = null;
                            lnqIsolation.DCRQ = null;
                            lnqIsolation.DCRY = null;
                            lnqIsolation.CLRQ = null;
                            lnqIsolation.CLRY = null;
                            lnqIsolation.JYRY = null;
                            lnqIsolation.JYRQ = null;
                            lnqIsolation.DRRQ = null;
                            lnqIsolation.DRRY = null;
                            lnqIsolation.QRRQ = null;
                            lnqIsolation.QRRY = null;
                            //lnqIsolation.QRSM = null;
                            //lnqIsolation.RejectMode = null;

                            break;
                        case "等待主管审核":

                            strMsg = string.Format("{0}号不合格品隔离处置单已回退，请您重新处理单据; 回退原因为" + rebackReason, djh);
                            m_billMessageServer.PassFlowMessage(djh, strMsg,lnqIsolation.SHRY, false);

                            lnqIsolation.DJZT = "等待主管审核";
                            lnqIsolation.SHRQ = null;
                            lnqIsolation.SHRY = null;
                            lnqIsolation.DCRQ = null;
                            lnqIsolation.DCRY = null;
                            lnqIsolation.CLRQ = null;
                            lnqIsolation.CLRY = null;
                            lnqIsolation.JYRY = null;
                            lnqIsolation.JYRQ = null;
                            lnqIsolation.DRRQ = null;
                            lnqIsolation.DRRY = null;
                            lnqIsolation.QRRQ = null;
                            lnqIsolation.QRRY = null;
                            //lnqIsolation.QRSM = null;
                            //lnqIsolation.RejectMode = null;

                            break;
                        case "等待仓管调出":

                            strMsg = string.Format("{0}号不合格品隔离处置单已回退，请您重新处理单据; 回退原因为" + rebackReason, djh);
                            m_billMessageServer.PassFlowMessage(djh, strMsg,lnqIsolation.DCRY, false);

                            lnqIsolation.DJZT = "等待仓管调出";
                            lnqIsolation.DCRQ = null;
                            lnqIsolation.DCRY = null;
                            lnqIsolation.CLRQ = null;
                            lnqIsolation.CLRY = null;
                            lnqIsolation.JYRY = null;
                            lnqIsolation.JYRQ = null;
                            lnqIsolation.DRRQ = null;
                            lnqIsolation.DRRY = null;
                            lnqIsolation.QRRQ = null;
                            lnqIsolation.QRRY = null;
                            //lnqIsolation.QRSM = null;
                            //lnqIsolation.RejectMode = null;

                            break;
                        case "等待处理结果":

                            strMsg = string.Format("{0}号不合格品隔离处置单已回退，请您重新处理单据; 回退原因为" + rebackReason, djh);
                            m_billMessageServer.PassFlowMessage(djh, strMsg,lnqIsolation.CLRY, false);

                            lnqIsolation.DJZT = "等待处理结果";

                            lnqIsolation.CLRQ = null;
                            lnqIsolation.CLRY = null;
                            lnqIsolation.JYRY = null;
                            lnqIsolation.JYRQ = null;
                            lnqIsolation.DRRQ = null;
                            lnqIsolation.DRRY = null;
                            lnqIsolation.QRRQ = null;
                            lnqIsolation.QRRY = null;
                            //lnqIsolation.QRSM = null;
                            //lnqIsolation.RejectMode = null;
                            break;
                        case "等待质检结果":

                            strMsg = string.Format("{0}号不合格品隔离处置单已回退，请您重新处理单据; 回退原因为" + rebackReason, djh);
                            m_billMessageServer.PassFlowMessage(djh, strMsg,lnqIsolation.JYRY, false);

                            lnqIsolation.DJZT = "等待质检结果";

                            lnqIsolation.JYRY = null;
                            lnqIsolation.JYRQ = null;
                            lnqIsolation.DRRQ = null;
                            lnqIsolation.DRRY = null;
                            lnqIsolation.QRRQ = null;
                            lnqIsolation.QRRY = null;
                            //lnqIsolation.QRSM = null;

                            strMsg = string.Format("{0}号不合格品隔离处置单已回退，请您重新处理单据; 回退原因为" + rebackReason, djh);
                            m_billMessageServer.PassFlowMessage(djh, strMsg, CE_RoleEnum.检验员.ToString(), true);

                            break;
                        case "等待质管主管确认":

                            strMsg = string.Format("{0}号不合格品隔离处置单已回退，请您重新处理单据; 回退原因为" + rebackReason, djh);
                            m_billMessageServer.PassFlowMessage(djh, strMsg,lnqIsolation.DRRY, false);

                            lnqIsolation.DJZT = "等待质管主管确认";

                            lnqIsolation.DRRQ = null;
                            lnqIsolation.DRRY = null;
                            lnqIsolation.QRRQ = null;
                            lnqIsolation.QRRY = null;
                            //lnqIsolation.QRSM = null;

                            break;
                        case "等待采购退货":

                            lnqIsolation.DJZT = "等待采购退货";
                            strMsg = string.Format("{0}号不合格品隔离处置单已回退，请您重新处理单据; 回退原因为" + rebackReason, djh);
                            m_billMessageServer.PassFlowMessage(djh, strMsg, CE_RoleEnum.采购员.ToString(), true);

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
        /// 获得隔离单的关联单据号
        /// </summary>
        /// <param name="billID">隔离单号</param>
        /// <returns>返回获得的关联单据号</returns>
        public string GetAssociateBillID(string billID)
        {
            string strSql = "select AssociateRejectBillID from S_IsolationManageBill where DJH = '"+ billID +"'";

            return  GlobalObject.DatabaseServer.QueryInfo(strSql).Rows[0][0].ToString();
        }

        /// <summary>
        /// 清除单个单据(隔离单)
        /// </summary>
        /// <param name="context">上下文数据集</param>
        /// <param name="billNo">单据号</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">物品批次</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool ClearGoodsDate(DepotManagementDataContext context,
            string billNo, int goodsID, string batchNo, out string error)
        {
            error = null;

            try
            {
                var varIsolation = from a in context.S_IsolationManageBill
                              where a.GoodsID == goodsID
                              && a.BatchNo == batchNo
                              && a.AssociateRejectBillID == billNo
                              && a.DJZT == "等待采购退货"
                              select a;

                if (varIsolation.Count() == 1)
                {
                    S_IsolationManageBill lnqIsolation = varIsolation.Single();

                    lnqIsolation.AssociateRejectBillID = null;
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
        /// 更改关联单据号
        /// </summary>
        /// <param name="context">上下文数据集</param>
        /// <param name="billNo">单据号</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool UpdateAssicotaeBillID(DepotManagementDataContext context,
            string billNo, int goodsID, string batchNo, out string error)
        {
            error = null;

            try
            {
                var varData = from a in context.S_IsolationManageBill
                              where a.GoodsID == goodsID
                              && a.BatchNo == batchNo
                              && a.DJZT == "等待采购退货"
                              select a;

                if (varData.Count() == 1)
                {
                    S_IsolationManageBill lnqIsolation = varData.Single();

                    lnqIsolation.AssociateRejectBillID = billNo;
                }

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
                throw;
            }
        }
    }
}
