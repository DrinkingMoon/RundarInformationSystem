/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  MusterAffirmBill.cs
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
    /// 样品管理类
    /// </summary>
    class MusterAffirmBill:BasicServer, IMusterAffirmBill
    {
        /// <summary>
        /// BOM表信息服务组件
        /// </summary>
        IBomServer m_serverBom = ServerModuleFactory.GetServerModule<IBomServer>();

        /// <summary>
        /// 部门信息服务组件
        /// </summary>
        IDepartmentServer m_serverDepartment = ServerModuleFactory.GetServerModule<IDepartmentServer>();

        /// <summary>
        /// 单位服务组件
        /// </summary>
        IUnitServer m_serverUnit = ServerModuleFactory.GetServerModule<IUnitServer>();

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 单据编号分配服务
        /// </summary>
        IAssignBillNoServer m_serverAssginBillNo = ServerModuleFactory.GetServerModule<IAssignBillNoServer>();

        /// <summary>
        /// 获取计划价格服务组件
        /// </summary>
        IBasicGoodsServer m_serverBasicGoods = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();

        /// <summary>
        /// 条形码
        /// </summary>
        IBarCodeServer m_barCodeServer = ServerModuleFactory.GetServerModule<IBarCodeServer>();

        /// <summary>
        /// 检查某业务是否存在
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">业务号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.S_MusterAffirmBill
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
            string sql = "SELECT * FROM [DepotManagement].[dbo].[S_MusterAffirmBill] where DJH = '" + billNo + "'";

            System.Data.DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 获得全部单据信息
        /// </summary>
        /// <returns>返回查询到的单据信息</returns>
        public DataTable GetAllBill()
        {
            string strSql = "select * from View_S_MusterAffirmBill order by 单据号 desc";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dt;
        }

        /// <summary>
        /// 获得一条单据的全部信息
        /// </summary>
        /// <param name="djh">样品单单号</param>
        /// <returns>返回一条样品单记录</returns>
        public S_MusterAffirmBill GetBill(string djh)
        {
            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            var varData = from a in dataContext.S_MusterAffirmBill
                          where a.DJH == djh
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
        /// 新建单据
        /// </summary>
        /// <param name="musterAffirmBill">样品单据信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns></returns>
        bool CreateBill(S_MusterAffirmBill musterAffirmBill,out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.S_MusterAffirmBill
                              where a.DJH == musterAffirmBill.DJH
                              select a;

                if (varData.Count() != 0)
                {
                    error = "此单号已被占用请重新填写单据";
                    return false;
                }

                S_MusterAffirmBill lnqMuster = new S_MusterAffirmBill();

                lnqMuster.DJH = musterAffirmBill.DJH;
                lnqMuster.DJZT = "新建单据";
                lnqMuster.CreatBillTime = ServerTime.Time;
                lnqMuster.GoodsID = musterAffirmBill.GoodsID;
                lnqMuster.BatchNo = musterAffirmBill.BatchNo;
                lnqMuster.Provider = musterAffirmBill.Provider;
                lnqMuster.MusterType = musterAffirmBill.MusterType;
                lnqMuster.OrderFormNumber = musterAffirmBill.OrderFormNumber;
                lnqMuster.IsPay = musterAffirmBill.IsPay;
                lnqMuster.GiveMusterCount = musterAffirmBill.GiveMusterCount;
                lnqMuster.Version = musterAffirmBill.Version;
                lnqMuster.SendCount = musterAffirmBill.SendCount;
                lnqMuster.SQR = BasicInfo.LoginName;
                lnqMuster.SQRQ = ServerTime.Time;
                lnqMuster.StorageID = musterAffirmBill.StorageID;
                lnqMuster.Remark = musterAffirmBill.Remark;
                lnqMuster.IsOutsourcing = musterAffirmBill.IsOutsourcing;
                lnqMuster.IsIncludeRawMaterial = musterAffirmBill.IsIncludeRawMaterial;
                lnqMuster.IsBlank = musterAffirmBill.IsBlank;
                lnqMuster.ProviderBatchNo = musterAffirmBill.ProviderBatchNo;

                dataContext.S_MusterAffirmBill.InsertOnSubmit(lnqMuster);

                dataContext.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="inMuster">样品单信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>保存成功返回True，保存失败返回False</returns>
        public bool SaveInfo(S_MusterAffirmBill inMuster, out string error)
        {
            error = null;

            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            try
            {
                var varData = from a in dataContext.S_MusterAffirmBill
                              where a.DJH == inMuster.DJH
                              select a;

                if (varData.Count() == 0)
                {
                    if (!CreateBill(inMuster,out error))
                    {
                        throw new Exception(error);
                    }

                }
                else if (varData.Count() > 1)
                {
                    error = "数位不唯一";
                    throw new Exception(error);
                    
                }
                else
                {
                    S_MusterAffirmBill lnqMuster = varData.Single();

                    if (lnqMuster.DJZT != inMuster.DJZT)
                    {
                        error = "单据状态错误，请重新刷新单据确认单据状态";
                        return false;
                    }

                    switch (lnqMuster.DJZT)
                    {
                        case "新建单据":

                            lnqMuster.IsIncludeRawMaterial = inMuster.IsIncludeRawMaterial;
                            lnqMuster.IsBlank = inMuster.IsBlank;
                            lnqMuster.IsOutsourcing = inMuster.IsOutsourcing;
                            lnqMuster.GoodsID = inMuster.GoodsID;
                            lnqMuster.BatchNo = inMuster.BatchNo;
                            lnqMuster.Provider = inMuster.Provider;
                            lnqMuster.MusterType = inMuster.MusterType;
                            lnqMuster.OrderFormNumber = inMuster.OrderFormNumber;
                            lnqMuster.IsPay = inMuster.IsPay;
                            lnqMuster.GiveMusterCount = inMuster.GiveMusterCount;
                            lnqMuster.Version = inMuster.Version;
                            lnqMuster.SendCount = inMuster.SendCount;
                            lnqMuster.SQR = BasicInfo.LoginName;
                            lnqMuster.SQRQ = ServerTime.Time;
                            lnqMuster.StorageID = inMuster.StorageID;
                            lnqMuster.Remark = inMuster.Remark;
                            lnqMuster.ProviderBatchNo = inMuster.ProviderBatchNo;

                            S_MusterAffirmBill lnqTemp = GetBill(inMuster.ProviderBatchNo);

                            if (lnqTemp != null)
                            {
                                lnqMuster.MusterType = lnqTemp.MusterType;
                            }

                            break;
                        case "等待主管审核":
                            lnqMuster.ChargeAuditingPersonnel = BasicInfo.LoginName;
                            lnqMuster.ChargeAuditingTime = ServerTime.Time;

                            break;
                        case "等待财务确认":
                            lnqMuster.RawMaterialPrice = inMuster.RawMaterialPrice;
                            lnqMuster.Financer = BasicInfo.LoginName;
                            lnqMuster.FinanceTime = ServerTime.Time;

                            break;
                        case "等待仓管确认到货":
                            lnqMuster.MusterCount = inMuster.MusterCount;
                            lnqMuster.AffirmGoodsPersonnel = BasicInfo.LoginName;
                            lnqMuster.AffirmGoodsTime = ServerTime.Time;
                            break;
                        case "等待检验":

                            if (inMuster.Checker == null || inMuster.Checker.Trim().Length == 0)
                            {
                                throw new Exception("请选择检验员");
                            }

                            lnqMuster.FeederReport = inMuster.FeederReport;
                            lnqMuster.CheckReport = inMuster.CheckReport;
                            lnqMuster.CheckResult = inMuster.CheckResult;
                            lnqMuster.MusterPack = inMuster.MusterPack;
                            lnqMuster.CheckScarpCount = inMuster.CheckScarpCount;
                            lnqMuster.Checker = inMuster.Checker;
                            lnqMuster.JYR = BasicInfo.LoginName;
                            lnqMuster.JYRQ = ServerTime.Time;

                            break;
                        case "等待确认检验信息":
                            lnqMuster.SQEExplain = inMuster.SQEExplain;
                            lnqMuster.SQE = BasicInfo.LoginName;
                            lnqMuster.SQERQ = ServerTime.Time;
                            break;
                        case "等待工艺工程师评审":

                            lnqMuster.CraftMusterCareful = inMuster.CraftMusterCareful;
                            lnqMuster.CraftMusterCarefulResult = inMuster.CraftMusterCarefulResult;
                            lnqMuster.CraftMusterCarefulResultReport = inMuster.CraftMusterCarefulResultReport;
                            lnqMuster.CraftMusterCarefulPersonnel = BasicInfo.LoginName;
                            lnqMuster.CraftMusterCarefulDate = ServerTime.Time;
                            break;
                        case "等待零件工程师评审":

                            lnqMuster.MusterCareful = inMuster.MusterCareful;
                            lnqMuster.MusterCarefulResult = inMuster.MusterCarefulResult;
                            lnqMuster.MusterCarefulResultReport = inMuster.MusterCarefulResultReport;
                            lnqMuster.PSR = BasicInfo.LoginName;
                            lnqMuster.PSRQ = ServerTime.Time;
                            break;

                        case "等待项目经理确认":
                            lnqMuster.AffirmResult = inMuster.AffirmResult;
                            lnqMuster.JLR = BasicInfo.LoginName;
                            lnqMuster.JLRQ = ServerTime.Time;
                            break;

                        case "等待试验结果":
                            lnqMuster.EngineerAffirmResult = inMuster.EngineerAffirmResult;
                            lnqMuster.EngineerMind = inMuster.EngineerMind;
                            lnqMuster.EngineerRemainMusterDispose = inMuster.EngineerRemainMusterDispose;
                            lnqMuster.EngineerTestMusterCVTDispose = inMuster.EngineerTestMusterCVTDispose;
                            lnqMuster.TestAssemblyNumber = inMuster.TestAssemblyNumber;
                            lnqMuster.TestResult = inMuster.TestResult;
                            lnqMuster.SYR = BasicInfo.LoginName;
                            lnqMuster.SYRQ = ServerTime.Time;
                            break;

                        case "等待主管确认":
                            lnqMuster.IsEligbility = inMuster.IsEligbility;
                            lnqMuster.SatrapAffirmResult = inMuster.SatrapAffirmResult;
                            lnqMuster.SatrapMind = inMuster.SatrapMind;
                            lnqMuster.SatrapRemainMusterDispose = inMuster.SatrapRemainMusterDispose;
                            lnqMuster.SatrapTestMusterCVTDispose = inMuster.SatrapTestMusterCVTDispose;
                            lnqMuster.ZGR = BasicInfo.LoginName;
                            lnqMuster.ZGRQ = ServerTime.Time;
                            break;

                        case "等待SQE处理":
                            lnqMuster.EjectableCount = inMuster.EjectableCount;
                            lnqMuster.ScrapCount = Convert.ToInt32(GetUseCount(Convert.ToInt32(lnqMuster.GoodsID), lnqMuster.BatchNo));
                            lnqMuster.EligbilityCount = Convert.ToInt32(Convert.ToDecimal(lnqMuster.MusterCount) -
                                Convert.ToDecimal(lnqMuster.ScrapCount) -
                                Convert.ToDecimal(lnqMuster.EjectableCount));

                            lnqMuster.ScrapDisposeMode = inMuster.ScrapDisposeMode;
                            lnqMuster.CLR = BasicInfo.LoginName;
                            lnqMuster.CLRQ = ServerTime.Time;
                            break;

                        case "等待仓管确认入库":
                            lnqMuster.KFR = BasicInfo.LoginName;
                            lnqMuster.KFRQ = ServerTime.Time;
                            lnqMuster.ShelfArea = inMuster.ShelfArea == null ? "" : inMuster.ShelfArea;
                            lnqMuster.ColumnNumber = inMuster.ColumnNumber == null ? "" : inMuster.ColumnNumber;
                            lnqMuster.LayerNumber = inMuster.LayerNumber == null ? "" : inMuster.LayerNumber;

                            lnqMuster.ScrapCount = Convert.ToInt32(GetUseCount(Convert.ToInt32(lnqMuster.GoodsID), lnqMuster.BatchNo));
                            lnqMuster.EligbilityCount = Convert.ToInt32(Convert.ToDecimal(lnqMuster.MusterCount) -
                                Convert.ToDecimal(lnqMuster.ScrapCount) - 
                                Convert.ToDecimal(lnqMuster.EjectableCount));

                            break;
                        default:
                            break;
                    }

                    dataContext.SubmitChanges();

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
        /// 更改单据状态
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>变更成功返回True，变更失败返回False</returns>
        public bool UpdateBill(string billNo, out string error)
        {
            error = null;

            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            dataContext.Connection.Open();
            dataContext.Transaction = dataContext.Connection.BeginTransaction();

            try
            {
                var varData = from a in dataContext.S_MusterAffirmBill
                              where a.DJH == billNo
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数位不唯一或者为空";
                    throw new Exception(error);
                }
                else
                {
                    S_MusterAffirmBill lnqMuster = varData.Single();

                    switch (lnqMuster.DJZT)
                    {
                        case "新建单据":

                            if (Convert.ToDecimal(lnqMuster.SendCount) > 50)
                            {
                                lnqMuster.DJZT = "等待主管审核";
                            }
                            else
                            {
                                if (lnqMuster.IsOutsourcing && lnqMuster.IsIncludeRawMaterial)
                                {
                                    lnqMuster.DJZT = "等待财务确认";
                                }
                                else
                                {
                                    lnqMuster.DJZT = "等待仓管确认到货";
                                }
                            }

                            lnqMuster.SQR = BasicInfo.LoginName;
                            lnqMuster.SQRQ = ServerTime.Time;

                            break;
                        case "等待主管审核":
                            lnqMuster.ChargeAuditingPersonnel = BasicInfo.LoginName;
                            lnqMuster.ChargeAuditingTime = ServerTime.Time;

                            if (lnqMuster.IsOutsourcing && lnqMuster.IsIncludeRawMaterial)
                            {
                                lnqMuster.DJZT = "等待财务确认";
                            }
                            else
                            {
                                lnqMuster.DJZT = "等待仓管确认到货";
                            }
                            
                            break;
                        case "等待财务确认":
                            lnqMuster.Financer = BasicInfo.LoginName;
                            lnqMuster.FinanceTime = ServerTime.Time;
                            lnqMuster.DJZT = "等待仓管确认到货";

                            break;
                        case "等待仓管确认到货":

                            lnqMuster.DJZT = "等待检验";
                            lnqMuster.AffirmGoodsPersonnel = BasicInfo.LoginName;
                            lnqMuster.AffirmGoodsTime = ServerTime.Time;

                            if (!InsertNewBarCode(lnqMuster, out error))
                            {
                                throw new Exception(error);
                            }

                            break;
                        case "等待检验":
                            lnqMuster.DJZT = "等待确认检验信息";
                            lnqMuster.JYR = BasicInfo.LoginName;
                            lnqMuster.JYRQ = ServerTime.Time;

                            if (!InsertMusterStock(dataContext, lnqMuster, out error))
                            {
                                throw new Exception(error);
                            }

                            dataContext.SubmitChanges();

                            if (Convert.ToDecimal(lnqMuster.CheckScarpCount) > 0)
                            {
                                if (!CreatMusterUseBill(dataContext, lnqMuster, out error))
                                {
                                    throw new Exception(error);
                                }
                            }

                            dataContext.SubmitChanges();

                            break;
                        case "等待确认检验信息":
                            if (lnqMuster.IsBlank)
                            {
                                lnqMuster.DJZT = "等待工艺工程师评审";
                            }
                            else
                            {
                                lnqMuster.DJZT = "等待零件工程师评审";
                            }

                            lnqMuster.SQE = BasicInfo.LoginName;
                            lnqMuster.SQERQ = ServerTime.Time;
                            break;

                        case "等待工艺工程师评审":

                            lnqMuster.DJZT = "等待零件工程师评审";
                            lnqMuster.CraftMusterCarefulPersonnel = BasicInfo.LoginName;
                            lnqMuster.CraftMusterCarefulDate = ServerTime.Time;
                            break;

                        case "等待零件工程师评审":

                            if (lnqMuster.IsBlank)
                            {
                                lnqMuster.DJZT = "等待SQE处理";

                                if (lnqMuster.MusterCarefulResult == "重样送样")
                                {
                                    lnqMuster.SatrapRemainMusterDispose = "报废/退货";
                                }
                                else
                                {
                                    lnqMuster.SatrapRemainMusterDispose = "入样品库";
                                }
                            }
                            else
                            {

                                if (lnqMuster.MusterCarefulResult == "重样送样" || lnqMuster.MusterCarefulResult == "入库")
                                {
                                    lnqMuster.DJZT = "等待主管确认";
                                }
                                else
                                {
                                    lnqMuster.DJZT = lnqMuster.StorageID == "03" ? "等待试验结果" : "等待项目经理确认";
                                }

                            }

                            lnqMuster.PSR = BasicInfo.LoginName;
                            lnqMuster.PSRQ = ServerTime.Time;
                            break;

                        case "等待项目经理确认":

                            lnqMuster.DJZT = "等待试验结果";
                            lnqMuster.JLR = BasicInfo.LoginName;
                            lnqMuster.JLRQ = ServerTime.Time;
                            break;

                        case "等待试验结果":

                            lnqMuster.DJZT = "等待主管确认";
                            lnqMuster.SYR = BasicInfo.LoginName;
                            lnqMuster.SYRQ = ServerTime.Time;
                            break;

                        case "等待主管确认":

                            if (lnqMuster.SatrapRemainMusterDispose == null)
                            {
                                error = "请选择处理方式";
                                return false;
                            }

                            lnqMuster.DJZT = "等待SQE处理";
                            lnqMuster.ZGR = BasicInfo.LoginName;
                            lnqMuster.ZGRQ = ServerTime.Time;
                            break;

                        case "等待SQE处理":

                            if(lnqMuster.EjectableCount == null)
                            {
                                lnqMuster.EjectableCount = 0;
                            }

                            lnqMuster.ScrapCount = Convert.ToInt32(GetUseCount(Convert.ToInt32(lnqMuster.GoodsID), lnqMuster.BatchNo));
                            lnqMuster.EligbilityCount = Convert.ToInt32( 
                                Convert.ToDecimal(lnqMuster.MusterCount) -
                                Convert.ToDecimal(lnqMuster.ScrapCount) -
                                Convert.ToDecimal(lnqMuster.EjectableCount));

                            lnqMuster.DJZT = "等待仓管确认入库";
                            lnqMuster.CLR = BasicInfo.LoginName;
                            lnqMuster.CLRQ = ServerTime.Time;
                            break;

                        case "等待仓管确认入库":
                            
                            if (lnqMuster.DJZT == "单据已完成")
                            {
                                error = "单据不能重复确认";
                                throw new Exception(error);
                            }

                            lnqMuster.DJZT = "单据已完成";
                            lnqMuster.KFR = BasicInfo.LoginName;
                            lnqMuster.KFRQ = ServerTime.Time;
                            lnqMuster.ScrapCount = Convert.ToInt32(GetUseCount(Convert.ToInt32(lnqMuster.GoodsID), lnqMuster.BatchNo));
                            lnqMuster.EligbilityCount = Convert.ToInt32(
                                Convert.ToDecimal(lnqMuster.MusterCount) -
                                Convert.ToDecimal(lnqMuster.ScrapCount) -
                                Convert.ToDecimal(lnqMuster.EjectableCount));

                            if (lnqMuster.SatrapRemainMusterDispose == "报废/退货")
                            {
                                if (!ClearMusterStock(dataContext, Convert.ToInt32(lnqMuster.GoodsID),
                                    lnqMuster.BatchNo, lnqMuster.StorageID, out error))
                                {
                                    throw new Exception(error);
                                }

                                dataContext.SubmitChanges();
                            }
                            else
                            {
                                if (((bool)lnqMuster.IsPay || lnqMuster.SatrapRemainMusterDispose == "入产品库") 
                                    //机加车间自制件入库，检测领料单是否领用过此供方批次
                                    || (lnqMuster.Provider == CE_WorkShopCode.JJCJ.ToString() && IsMaterialRequisitionCount(lnqMuster.ProviderBatchNo)))
                                {
                                    if (lnqMuster.SatrapRemainMusterDispose != "报废/退货")
                                    {
                                        if (lnqMuster.Provider == CE_WorkShopCode.JJCJ.ToString())
                                        {
                                            //自制件报检入库
                                            if (!InsertHomemadePartInDepotBill(dataContext, lnqMuster, out error))
                                            {
                                                throw new Exception(error);
                                            }

                                            dataContext.SubmitChanges();
                                        }
                                        else
                                        {
                                            //插入入库
                                            if (lnqMuster.IsOutsourcing)
                                            {
                                                //委外报检入库
                                                if (!InsertCheckInDepotOutsourcingBill(dataContext, lnqMuster, out error))
                                                {
                                                    throw new Exception(error);
                                                }

                                                dataContext.SubmitChanges();
                                            }
                                            else
                                            {
                                                //正常报检入库
                                                if (!InsertCheckInDepotBill(dataContext, lnqMuster, out error))
                                                {
                                                    throw new Exception(error);
                                                }

                                                dataContext.SubmitChanges();
                                            }
                                        }

                                        dataContext.SubmitChanges();

                                        //插入领料
                                        if (!InsertMaterialRequisition(dataContext, lnqMuster, out error))
                                        {
                                            throw new Exception(error);
                                        }


                                        if (lnqMuster.SatrapRemainMusterDispose == "入样品库")
                                        {
                                            UpdateGoodsStatus(dataContext, lnqMuster);
                                        }

                                        dataContext.SubmitChanges();
                                    }

                                    //清空样品库库存
                                    if (!ClearMusterStock(dataContext, Convert.ToInt32(lnqMuster.GoodsID), lnqMuster.
                                        BatchNo, lnqMuster.StorageID, out error))
                                    {
                                        throw new Exception(error);
                                    }

                                    dataContext.SubmitChanges();
                                }
                            } 

                            break;
                        default:
                            break;
                    }

                    dataContext.SubmitChanges();

                }

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

        void UpdateGoodsStatus(DepotManagementDataContext ctx, S_MusterAffirmBill billInfo)
        {
            var varData = from a in ctx.S_Stock
                          where a.GoodsID == billInfo.GoodsID
                          && a.BatchNo == billInfo.BatchNo
                          select a;


            if (varData.Count() == 1)
            {
                varData.Single().GoodsStatus = 2;
            }

            ctx.SubmitChanges();
        }

        #region 自动生成业务流

        /// <summary>
        /// 判断批次号是否领过料
        /// </summary>
        /// <param name="batchNo">批次号</param>
        /// <returns>领过返回True，否则返回False</returns>
        bool IsMaterialRequisitionCount(string batchNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            if (batchNo == null || batchNo.Trim().Length == 0)
            {
                return false;
            }

            List<string> list = batchNo.Split(new char[] { '/', '\\' }).ToList();

            if (list == null || list.Count == 0)
            {
                return false;
            }

            foreach (string str in list)
            {
                var varData = from a in ctx.S_MaterialRequisitionGoods
                              where a.BatchNo == str
                              select a;

                if (varData.Count() > 0)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 自制件入库单
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="inMuster">样品确认单LINQ数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        bool InsertHomemadePartInDepotBill(DepotManagementDataContext context, S_MusterAffirmBill inMuster, out string error)
        {
            HomemadePartInDepotServer serverHomemade = new HomemadePartInDepotServer();
            error = null;
            try
            {
                F_GoodsPlanCost lnqGoods = ServerModuleFactory.GetServerModule<IBasicGoodsServer>().GetGoodsInfo(Convert.ToInt32(inMuster.GoodsID));

                string strBillID = m_serverAssginBillNo.AssignNewNo(serverHomemade, CE_BillTypeEnum.自制件入库单.ToString());
                int operationCount = Convert.ToInt32(inMuster.EligbilityCount) + Convert.ToInt32(inMuster.ScrapCount);

                S_HomemadePartBill lnqHomemade = new S_HomemadePartBill();

                lnqHomemade.BatchNo = inMuster.BatchNo;
                lnqHomemade.Bill_ID = strBillID;
                lnqHomemade.Bill_Time = Convert.ToDateTime( inMuster.CreatBillTime);
                lnqHomemade.BillStatus = "已入库";
                lnqHomemade.Checker = inMuster.JYR;
                lnqHomemade.CheckoutJoinGoods_Time = inMuster.JYRQ;
                lnqHomemade.CheckoutReport_ID = inMuster.CheckReport;
                lnqHomemade.ColumnNumber = inMuster.ColumnNumber;
                lnqHomemade.ConcessionCount = 0;
                lnqHomemade.ConfirmAmountSignatory = inMuster.AffirmGoodsPersonnel;
                lnqHomemade.DeclareCount = operationCount;

                if (lnqHomemade.DeclareCount == 0)
                {
                    error = "入库数量不能为0";
                    return false;
                }

                lnqHomemade.DeclarePersonnel = inMuster.SQR;
                lnqHomemade.DeclarePersonnelCode = UniversalFunction.GetPersonnelCode(inMuster.SQR);
                lnqHomemade.DeclareWastrelCount = 0;
                lnqHomemade.DepotManager = inMuster.KFR;
                lnqHomemade.DepotManagerAffirmCount = operationCount;
                lnqHomemade.EligibleCount = operationCount;
                lnqHomemade.GoodsID = Convert.ToInt32( inMuster.GoodsID);
                lnqHomemade.InDepotCount = operationCount;
                lnqHomemade.InDepotTime = ServerTime.Time;
                lnqHomemade.LayerNumber = inMuster.LayerNumber;
                lnqHomemade.PlanPrice = 0;
                lnqHomemade.PlanUnitPrice = 0;
                lnqHomemade.Price = 0;
                lnqHomemade.Provider = inMuster.Provider;
                lnqHomemade.ProviderBatchNo = inMuster.ProviderBatchNo;
                lnqHomemade.QualityInfo = inMuster.CheckResult;
                lnqHomemade.QualityInputer = inMuster.JYR;
                lnqHomemade.ReimbursementCount = 0;
                lnqHomemade.Remark = "由样品单 [" + inMuster.DJH + "] 转产品的自制件入库单";
                lnqHomemade.ShelfArea = inMuster.ShelfArea;
                lnqHomemade.StorageID = inMuster.StorageID;
                lnqHomemade.TotalPrice = "零元整";
                lnqHomemade.UnitPrice = 0;

                context.S_HomemadePartBill.InsertOnSubmit(lnqHomemade);

                serverHomemade.OpertaionDetailAndStock(context, lnqHomemade);
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
                
            }

        }

        /// <summary>
        /// 插入委外报检入库表中
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="inMuster">样品单信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>插入成功返回True，插入失败返回False</returns>
        private bool InsertCheckInDepotOutsourcingBill(DepotManagementDataContext context, S_MusterAffirmBill inMuster, out string error)
        {
            BargainInfoServer serverBargainInfo = new BargainInfoServer();

            CheckOutInDepotForOutsourcingServer serverOutsourcingServer = new CheckOutInDepotForOutsourcingServer();
            try
            {
                error = null;

                F_GoodsPlanCost lnqGoods = ServerModuleFactory.GetServerModule<IBasicGoodsServer>().GetGoodsInfo(Convert.ToInt32(inMuster.GoodsID));

                string strBillID = m_serverAssginBillNo.AssignNewNo(serverOutsourcingServer, CE_BillTypeEnum.委外报检入库单.ToString());
                decimal dcBargainUnitPrice = serverBargainInfo.GetBargainUnitPrice(inMuster.OrderFormNumber, Convert.ToInt32(inMuster.GoodsID));
                int operationCount = Convert.ToInt32(inMuster.EligbilityCount) + Convert.ToInt32(inMuster.ScrapCount);

                S_CheckOutInDepotForOutsourcingBill lnqCheckBill = new S_CheckOutInDepotForOutsourcingBill();

                lnqCheckBill.ArrivePersonnel = inMuster.AffirmGoodsPersonnel;
                lnqCheckBill.ArriveTime = inMuster.AffirmGoodsTime;
                lnqCheckBill.BatchNo = inMuster.BatchNo;
                lnqCheckBill.Bill_ID = strBillID;
                lnqCheckBill.BillStatus = "已入库";
                lnqCheckBill.Checker = inMuster.JYR;
                lnqCheckBill.CheckoutReport_ID = inMuster.CheckReport;
                lnqCheckBill.ColumnNumber = inMuster.ColumnNumber;
                lnqCheckBill.ConcessionCount = 0;
                lnqCheckBill.DeclareCount = operationCount;
                lnqCheckBill.DeclarePersonnel = inMuster.SQR;
                lnqCheckBill.DeclareTime = inMuster.SQRQ;
                lnqCheckBill.DeclareWastrelCount = 0;
                lnqCheckBill.DepotManagerAffirmCount = operationCount;
                lnqCheckBill.Depot = lnqGoods.GoodsType;
                lnqCheckBill.FinancePersonnel = inMuster.Financer;
                lnqCheckBill.FinanceTime = inMuster.FinanceTime;
                lnqCheckBill.EligibleCount = operationCount;
                lnqCheckBill.GoodsID = Convert.ToInt32(inMuster.GoodsID);
                lnqCheckBill.InDepotCount = operationCount;
                lnqCheckBill.ManagerTime = ServerTime.Time;
                lnqCheckBill.ManagerPersonnel = inMuster.KFR;
                lnqCheckBill.IsExigenceCheck = false;
                lnqCheckBill.HavingInvoice = false;
                lnqCheckBill.InvoicePrice = 0;
                lnqCheckBill.LayerNumber = inMuster.LayerNumber;
                lnqCheckBill.OrderFormNumber = inMuster.OrderFormNumber;
                lnqCheckBill.QualityPersonnel = inMuster.JYR;
                lnqCheckBill.QualityTime = inMuster.JYRQ;
                lnqCheckBill.OutsourcingUnitPrice = inMuster.IsPay == true ? dcBargainUnitPrice : 0;
                lnqCheckBill.RawMaterialPrice = inMuster.IsPay == true ? inMuster.RawMaterialPrice : 0;
                lnqCheckBill.PeremptorilyEmit = false;
                lnqCheckBill.UnitPrice = inMuster.IsPay == true ?
                      inMuster.IsIncludeRawMaterial == true ?
                      dcBargainUnitPrice + inMuster.RawMaterialPrice : dcBargainUnitPrice : 0;

                lnqCheckBill.Price = inMuster.IsPay == true ?
                    inMuster.IsIncludeRawMaterial == true ?
                    Convert.ToDecimal(operationCount) * (dcBargainUnitPrice + inMuster.RawMaterialPrice) :
                    Convert.ToDecimal(operationCount) * dcBargainUnitPrice : 0;

                lnqCheckBill.Provider = inMuster.Provider;
                lnqCheckBill.ProviderBatchNo = inMuster.ProviderBatchNo;
                lnqCheckBill.QualityInfo = inMuster.CheckResult;
                lnqCheckBill.ReimbursementCount = 0;
                lnqCheckBill.ShelfArea = inMuster.ShelfArea;
                lnqCheckBill.StorageID = inMuster.StorageID;
                lnqCheckBill.UnitInvoicePrice = 0;
                lnqCheckBill.IsIncludeRawMaterial = inMuster.IsIncludeRawMaterial;
                lnqCheckBill.Remark = "由样品单 [" + inMuster.DJH + "] 转产品的委外报检单";

                DataRow dr = m_serverBom.GetBomInfo(lnqGoods.GoodsCode, lnqGoods.GoodsName);

                if (dr == null)
                {
                    lnqCheckBill.Version = "";
                }
                else
                {
                    lnqCheckBill.Version = dr["Version"].ToString();
                }

                context.S_CheckOutInDepotForOutsourcingBill.InsertOnSubmit(lnqCheckBill);

                serverOutsourcingServer.OpertaionDetailAndStock(context, lnqCheckBill);
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 插入正常报检入库表中
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="inMuster">样品单信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>插入成功返回True，插入失败返回False</returns>
        private bool InsertCheckInDepotBill(DepotManagementDataContext context, S_MusterAffirmBill inMuster, out string error)
        {

            BargainInfoServer serverBargainInfo = new BargainInfoServer();
            CheckOutInDepotServer serverCheckOutInDepot = new CheckOutInDepotServer();
            try
            {
                error = null;

                F_GoodsPlanCost lnqGoods = ServerModuleFactory.GetServerModule<IBasicGoodsServer>().GetGoodsInfo(Convert.ToInt32(inMuster.GoodsID));

                string strBillID = serverCheckOutInDepot.GetNextBillNo(1);
                decimal dcBargainUnitPrice = serverBargainInfo.GetBargainUnitPrice(inMuster.OrderFormNumber,
                    Convert.ToInt32(inMuster.GoodsID));
                int operationCount = Convert.ToInt32(inMuster.EligbilityCount) + Convert.ToInt32(inMuster.ScrapCount);

                S_CheckOutInDepotBill lnqCheckBill = new S_CheckOutInDepotBill();

                lnqCheckBill.ArriveGoods_Time = Convert.ToDateTime(inMuster.SQERQ);
                lnqCheckBill.BatchNo = inMuster.BatchNo;
                lnqCheckBill.Bill_ID = strBillID;
                lnqCheckBill.Bill_Time = Convert.ToDateTime(inMuster.CreatBillTime);
                lnqCheckBill.BillStatus = "已入库";
                lnqCheckBill.Buyer = inMuster.SQR;
                lnqCheckBill.Checker = inMuster.JYR;
                lnqCheckBill.CheckOutGoodsType = 1;
                lnqCheckBill.CheckoutJoinGoods_Time = Convert.ToDateTime( inMuster.JYRQ);
                lnqCheckBill.CheckoutReport_ID = inMuster.CheckReport;
                lnqCheckBill.CheckTime = Convert.ToDateTime(inMuster.JYRQ);
                lnqCheckBill.ColumnNumber = inMuster.ColumnNumber;
                lnqCheckBill.ConcessionCount = 0;
                lnqCheckBill.ConfirmAmountSignatory = inMuster.KFR;
                lnqCheckBill.DeclareCount = operationCount;

                if (lnqCheckBill.DeclareCount == 0)
                {
                    error = "入库数量不能为0";
                    return false;
                }

                lnqCheckBill.DeclarePersonnel = inMuster.SQR;
                lnqCheckBill.DeclarePersonnelCode = UniversalFunction.GetPersonnelCode(inMuster.SQR);
                lnqCheckBill.DeclareWastrelCount = 0;
                lnqCheckBill.DepotAffirmanceTime = Convert.ToDateTime(inMuster.AffirmGoodsTime);
                lnqCheckBill.DepotManager = inMuster.KFR;
                lnqCheckBill.DepotManagerAffirmCount = lnqCheckBill.DeclareCount;
                lnqCheckBill.Depot = lnqGoods.GoodsType;
                lnqCheckBill.EligibleCount = lnqCheckBill.DeclareCount;
                lnqCheckBill.GoodsID = Convert.ToInt32( inMuster.GoodsID);
                lnqCheckBill.InDepotCount = lnqCheckBill.DeclareCount;
                lnqCheckBill.InDepotTime = ServerTime.Time;
                lnqCheckBill.IsExigenceCheck = false;
                lnqCheckBill.HavingInvoice = false;
                lnqCheckBill.InvoicePrice = 0;
                lnqCheckBill.LayerNumber = inMuster.LayerNumber;
                lnqCheckBill.OrderFormNumber = inMuster.OrderFormNumber;
                lnqCheckBill.PeremptorilyEmit = false;
                lnqCheckBill.PlanPrice = inMuster.IsPay == true ?
                    Convert.ToDecimal(lnqGoods.GoodsUnitPrice) * Convert.ToDecimal(lnqCheckBill.DeclareCount) : 0;
                lnqCheckBill.PlanUnitPrice = inMuster.IsPay == true ? 
                    Convert.ToDecimal(lnqGoods.GoodsUnitPrice) : 0;
                lnqCheckBill.UnitPrice = inMuster.IsPay == true ? dcBargainUnitPrice : 0;
                lnqCheckBill.Price = inMuster.IsPay == true ? 
                    Math.Round(Convert.ToDecimal(lnqCheckBill.DeclareCount) * dcBargainUnitPrice, 2) : 0;
                lnqCheckBill.Provider = inMuster.Provider;
                lnqCheckBill.ProviderBatchNo = inMuster.ProviderBatchNo;
                lnqCheckBill.QualityInfo = inMuster.CheckResult;
                lnqCheckBill.QualityInputer = inMuster.JYR;
                lnqCheckBill.ReimbursementCount = 0;
                lnqCheckBill.ShelfArea = inMuster.ShelfArea;
                lnqCheckBill.StorageID = inMuster.StorageID;
                lnqCheckBill.TFFlag = false;
                lnqCheckBill.TotalPrice = inMuster.IsPay == true ? 
                    CalculateClass.GetTotalPrice(Convert.ToDecimal(lnqCheckBill.DeclareCount) * dcBargainUnitPrice) : "零";
                lnqCheckBill.UnitInvoicePrice = 0;
                lnqCheckBill.Remark = "由样品单 ["+ inMuster.DJH +"] 转产品的报检单";

                DataRow dr = m_serverBom.GetBomInfo(lnqGoods.GoodsCode, lnqGoods.GoodsName);

                if (dr == null)
                {
                    lnqCheckBill.Version = "";
                }
                else
                {
                    lnqCheckBill.Version = dr["Version"].ToString();
                }

                if (UniversalFunction.GetStorageInfo_View(lnqCheckBill.StorageID).零成本控制)
                {
                    throw new Exception("【零成本】库房，无法通过【报检入库单】入库");
                }

                context.S_CheckOutInDepotBill.InsertOnSubmit(lnqCheckBill);

                serverCheckOutInDepot.OpertaionDetailAndStock(context, lnqCheckBill);

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 清空样品库库存
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <param name="storageID">库房ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        private bool ClearMusterStock(DepotManagementDataContext context, int goodsID,string batchNo,string storageID, out string error)
        {
            try
            {
                error = null;

                var varData = from a in context.S_MusterStock
                              where a.GoodsID == goodsID 
                              && a.BatchNo == batchNo
                              && a.StrorageID == storageID
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数据不唯一或者为空";
                    return false;
                }
                else
                {
                    S_MusterStock lnqMuster = varData.Single();
                    lnqMuster.Count = 0;
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
        /// 插入领料单
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="inMuster">样品单信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>插入成功返回True，插入失败返回False</returns>
        private bool InsertMaterialRequisition(DepotManagementDataContext context, S_MusterAffirmBill inMuster,out string error)
        {
            MaterialRequisitionServer serverMaterialBill = new MaterialRequisitionServer();
            IProductLendReturnService serverLendReturn = ServerModuleFactory.GetServerModule<IProductLendReturnService>();

            try
            {
                error = null;

                DataTable dt = GetUseCount(Convert.ToInt32(inMuster.GoodsID), 
                    inMuster.BatchNo,inMuster.StorageID);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string strBillID = m_serverAssginBillNo.AssignNewNo(serverMaterialBill,CE_BillTypeEnum.领料单.ToString());

                    S_MaterialRequisition lnqMaterial = new S_MaterialRequisition();

                    lnqMaterial.Bill_ID = strBillID;
                    lnqMaterial.Bill_Time = ServerModule.ServerTime.Time;
                    lnqMaterial.AssociatedBillNo = "";
                    lnqMaterial.AssociatedBillType = "";
                    lnqMaterial.BillStatus = "已出库";
                    lnqMaterial.Department = m_serverDepartment.GetDeptInfoFromPersonnelInfo(
                        dt.Rows[i]["SQR"].ToString()).Rows[0]["DepartmentCode"].ToString();
                    lnqMaterial.DepartmentDirector = dt.Rows[i]["SHR"].ToString();
                    lnqMaterial.DepotManager = inMuster.KFR;
                    lnqMaterial.FetchCount = 0;
                    lnqMaterial.FetchType = "零星领料";
                    lnqMaterial.FillInPersonnel = dt.Rows[i]["SQR"].ToString();
                    lnqMaterial.FillInPersonnelCode = UniversalFunction.GetPersonnelCode(dt.Rows[i]["SQR"].ToString());
                    lnqMaterial.ProductType = "";
                    lnqMaterial.PurposeCode = dt.Rows[i]["PurposeCode"].ToString();
                    lnqMaterial.Remark = "由样品耗损单自动生成，对应的样品确认申请单号为" + inMuster.DJH;
                    lnqMaterial.StorageID = inMuster.StorageID;
                    lnqMaterial.OutDepotDate = ServerTime.Time;

                    if (!serverMaterialBill.AutoCreateBill(context, lnqMaterial, out error))
                    {
                        return false;
                    }

                    context.SubmitChanges();

                    S_MaterialRequisitionGoods lnqMaterialGoods = new S_MaterialRequisitionGoods();

                    lnqMaterialGoods.Bill_ID = strBillID;
                    lnqMaterialGoods.BasicCount = 0;
                    lnqMaterialGoods.BatchNo = inMuster.BatchNo;
                    lnqMaterialGoods.GoodsID = Convert.ToInt32(inMuster.GoodsID);
                    lnqMaterialGoods.ProviderCode = inMuster.Provider;
                    lnqMaterialGoods.RealCount = Convert.ToDecimal(dt.Rows[i]["Count"].ToString());
                    lnqMaterialGoods.Remark = "";
                    lnqMaterialGoods.RequestCount = Convert.ToDecimal(dt.Rows[i]["Count"].ToString());
                    lnqMaterialGoods.ShowPosition = 1;

                    MaterialRequisitionGoodsServer serverMaterialGoods = new MaterialRequisitionGoodsServer();

                    if (!serverMaterialGoods.AutoCreateGoods(context, lnqMaterialGoods, out error))
                    {
                        return false;
                    }

                    context.SubmitChanges();
                    
                    S_ProductLendRecord tempRecord =
                        serverLendReturn.GetStockSingleInfo(context, BasicInfo.DeptCode, lnqMaterial.StorageID, lnqMaterialGoods.GoodsID,
                        lnqMaterialGoods.BatchNo, lnqMaterialGoods.ProviderCode);

                    if (tempRecord != null)
                    {
                        S_MaterialRequisitionProductReturnList tempLnq = new S_MaterialRequisitionProductReturnList();

                        tempLnq.ReturnGoodsID = lnqMaterialGoods.GoodsID;
                        tempLnq.ReturnBatchNo = lnqMaterialGoods.BatchNo;
                        tempLnq.ReturnProvider = lnqMaterialGoods.ProviderCode;
                        tempLnq.BillNo = lnqMaterialGoods.Bill_ID;
                        tempLnq.GoodsID = lnqMaterialGoods.GoodsID;
                        tempLnq.Provider = lnqMaterialGoods.ProviderCode;
                        tempLnq.OperatorCount = lnqMaterialGoods.RealCount;
                        tempLnq.BatchNo = lnqMaterialGoods.BatchNo;
                        tempLnq.Remark = "系统自动生成单据号【" + inMuster.DJH + "】";

                        context.S_MaterialRequisitionProductReturnList.InsertOnSubmit(tempLnq);
                        context.SubmitChanges();
                    }

                    serverMaterialBill.OpertaionDetailAndStock(context, lnqMaterial);
                    context.SubmitChanges();

                    m_serverAssginBillNo.UseBillNo(CE_BillTypeEnum.领料单.ToString(), strBillID);
                }

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        #endregion

        /// <summary>
        /// 插入条形码
        /// </summary>
        /// <param name="inMuster">样品单信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>插入成功返回True，插入失败返回False</returns>
        private bool InsertNewBarCode(S_MusterAffirmBill inMuster,out string error)
        {
            error = null;

            // 2014-05-20 夏石友，判断条形码是否存在时增加了供应商参数
            if (!m_barCodeServer.IsExists(Convert.ToInt32(inMuster.GoodsID), 
                inMuster.StorageID, inMuster.BatchNo, inMuster.Provider))
            {
                S_InDepotGoodsBarCodeTable newBarcode = new S_InDepotGoodsBarCodeTable();

                newBarcode.GoodsID = Convert.ToInt32(inMuster.GoodsID);
                newBarcode.Provider = inMuster.Provider;
                newBarcode.BatchNo = inMuster.BatchNo;
                newBarcode.ProductFlag = "0";
                newBarcode.StorageID = inMuster.StorageID;

                if (!m_barCodeServer.Add(newBarcode, out error))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 插入样品库表
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="inMuster">样品单信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>插入成功返回True，插入失败返回False</returns>
        private bool InsertMusterStock(DepotManagementDataContext context, S_MusterAffirmBill inMuster, out string error)
        {
            try
            {
                error = null;

                S_MusterStock lnqMusterStock = new S_MusterStock();

                lnqMusterStock.BatchNo = inMuster.BatchNo;
                lnqMusterStock.Count = Convert.ToDecimal(inMuster.MusterCount) -
                     Convert.ToDecimal(inMuster.CheckScarpCount);

                lnqMusterStock.GoodsID = inMuster.GoodsID;
                lnqMusterStock.Provider = inMuster.Provider;
                lnqMusterStock.StrorageID = inMuster.StorageID;
                lnqMusterStock.Version = inMuster.Version;

                context.S_MusterStock.InsertOnSubmit(lnqMusterStock);

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 获得样品耗用单Table
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>返回获得的样品耗用单信息</returns>
        private DataTable GetUseCount(int goodsID, string batchNo,string storageID)
        {
            string strSql = " select SQR, SHR, PurposeCode, sum(Count) as Count from S_MusterUseBill as a "+
                            " inner join S_MusterUseList as b on a.DJH = b.DJH " +
                            " where GoodsID = " + goodsID + " and BatchNo = '" + batchNo + "' "+
                            " and StorageID = '" + storageID + "' and DJZT = '单据已完成'" +
                            " group by GoodsID,BatchNo,SQR,StorageID,SHR,PurposeCode";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dt;
        }

        /// <summary>
        /// 获得样品库中的数量
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <returns>返回样品库的数量</returns>
        public decimal GetMusterStockCount(int goodsID,string batchNo)
        {
            string strSql = "select * from S_MusterStock where GoodsID = "+ goodsID 
                +" and BatchNo = '"+ batchNo +"'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return Convert.ToDecimal(dt.Rows[0]["Count"]);
        }

        /// <summary>
        /// 报废单据
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>报废成功返回True，报废失败返回False</returns>
        public bool ScarpBill(string djh,out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.S_MusterAffirmBill
                              where a.DJH == djh
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数据不唯一或者为空";
                    return false;
                }
                else
                {
                    S_MusterAffirmBill lnqbill = varData.Single();

                    if (!DeleteUseBill(dataContext, Convert.ToInt32(lnqbill.GoodsID),
                        lnqbill.BatchNo, lnqbill.StorageID, out error))
                    {
                        return false;
                    }

                    if (!DeleteMusterStock(dataContext, Convert.ToInt32(lnqbill.GoodsID),
                        lnqbill.BatchNo, lnqbill.StorageID, out error))
                    {
                        return false;
                    }

                    if (lnqbill.DJZT == "单据已完成")
                    {
                        error = "单据已完成不能报废";
                        return false;
                    }

                    dataContext.S_MusterAffirmBill.DeleteOnSubmit(lnqbill);
                }

                dataContext.SubmitChanges();

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
        /// <param name="billStatus">回退后的单据状态</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <param name="rebackReason">回退原因</param>
        /// <returns>回退成功返回True，回退失败返回False</returns>
        public bool ReturnBill(string djh, string billStatus, out string error, string rebackReason)
        {
            try
            {
                error = null;

                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.S_MusterAffirmBill
                              where a.DJH == djh
                              select a;

                string strMsg = "";


                if (varData.Count() != 1)
                {
                    error = "数据不唯一或者为空";
                    return false;
                }
                else
                {
                    S_MusterAffirmBill lnqMuster = varData.Single();
                    string tempStatus = lnqMuster.DJZT;

                    switch (billStatus)
                    {
                        case "新建单据":

                            strMsg = string.Format("{0}号样品确认申请单已回退，请您重新处理单据; 回退原因为" 
                                + rebackReason, djh);

                            m_billMessageServer.PassFlowMessage(djh, strMsg,
                                UniversalFunction.GetPersonnelCode(lnqMuster.SQR), false);

                            if (!DeleteUseBill(dataContext, Convert.ToInt32(lnqMuster.GoodsID),
                                lnqMuster.BatchNo, lnqMuster.StorageID, out error))
                            {
                                return false;
                            }

                            if (!DeleteMusterStock(dataContext, Convert.ToInt32(lnqMuster.GoodsID),
                                lnqMuster.BatchNo, lnqMuster.StorageID, out error))
                            {
                                return false;
                            }

                            lnqMuster.DJZT = "新建单据";

                            lnqMuster.ChargeAuditingPersonnel = null;
                            lnqMuster.ChargeAuditingTime = null;

                            //lnqMuster.MusterCount = null;
                            lnqMuster.AffirmGoodsPersonnel = null;
                            lnqMuster.AffirmGoodsTime = null;

                            //lnqMuster.CheckScarpCount = null;
                            //lnqMuster.FeederReport = null;
                            //lnqMuster.CheckReport = null;
                            //lnqMuster.CheckResult = null;
                            //lnqMuster.MusterPack = null;
                            lnqMuster.JYR = null;
                            lnqMuster.JYRQ = null;

                            //lnqMuster.SQEExplain = null;
                            lnqMuster.SQE = null;
                            lnqMuster.SQERQ = null;

                            //lnqMuster.CraftMusterCareful = null;
                            lnqMuster.CraftMusterCarefulDate = null;
                            lnqMuster.CraftMusterCarefulPersonnel = null;
                            //lnqMuster.CraftMusterCarefulResult = null;
                            //lnqMuster.CraftMusterCarefulResultReport = null;

                            //lnqMuster.MusterCareful = null;
                            //lnqMuster.MusterCarefulResult = null;
                            //lnqMuster.MusterCarefulResultReport = null;
                            lnqMuster.PSR = null;
                            lnqMuster.PSRQ = null;

                            //lnqMuster.AffirmResult = null;
                            lnqMuster.JLR = null;
                            lnqMuster.JLRQ = null;

                            //lnqMuster.EngineerAffirmResult = null;
                            //lnqMuster.EngineerMind = null;
                            //lnqMuster.EngineerRemainMusterDispose = null;
                            //lnqMuster.EngineerTestMusterCVTDispose = null;
                            //lnqMuster.TestAssemblyNumber = null;
                            //lnqMuster.TestResult = null;
                            lnqMuster.SYR = null;
                            lnqMuster.SYRQ = null;

                            //lnqMuster.IsEligbility = null;
                            //lnqMuster.SatrapAffirmResult = null;
                            //lnqMuster.SatrapMind = null;
                            //lnqMuster.SatrapRemainMusterDispose = null;
                            //lnqMuster.SatrapTestMusterCVTDispose = null;
                            lnqMuster.ZGR = null;
                            lnqMuster.ZGRQ = null;

                            //lnqMuster.ScrapCount = null;
                            //lnqMuster.ScrapDisposeMode = null;
                            //lnqMuster.EligbilityCount = null;
                            //lnqMuster.EjectableCount = null;
                            lnqMuster.CLR = null;
                            lnqMuster.CLRQ = null;

                            break;

                        case "等待仓管确认到货":

                            strMsg = string.Format("{0}号样品确认申请单已回退，请您重新处理单据; 回退原因为" 
                                + rebackReason, djh);

                            m_billMessageServer.PassFlowMessage(djh, strMsg,
                                UniversalFunction.GetPersonnelCode(lnqMuster.AffirmGoodsPersonnel), false);

                            if (!DeleteMusterStock(dataContext, Convert.ToInt32(lnqMuster.GoodsID),
                                lnqMuster.BatchNo, lnqMuster.StorageID, out error))
                            {
                                return false;
                            }

                            if (!DeleteUseBill(dataContext, Convert.ToInt32(lnqMuster.GoodsID),
                                lnqMuster.BatchNo, lnqMuster.StorageID, out error))
                            {
                                return false;
                            }

                            lnqMuster.DJZT = "等待仓管确认到货";

                            //lnqMuster.MusterCount = null;
                            lnqMuster.AffirmGoodsPersonnel = null;
                            lnqMuster.AffirmGoodsTime = null;

                            //lnqMuster.CheckScarpCount = null;
                            //lnqMuster.FeederReport = null;
                            //lnqMuster.CheckReport = null;
                            //lnqMuster.CheckResult = null;
                            //lnqMuster.MusterPack = null;
                            lnqMuster.JYR = null;
                            lnqMuster.JYRQ = null;

                            //lnqMuster.SQEExplain = null;
                            lnqMuster.SQE = null;
                            lnqMuster.SQERQ = null;

                            //lnqMuster.CraftMusterCareful = null;
                            lnqMuster.CraftMusterCarefulDate = null;
                            lnqMuster.CraftMusterCarefulPersonnel = null;
                            //lnqMuster.CraftMusterCarefulResult = null;
                            //lnqMuster.CraftMusterCarefulResultReport = null;

                            //lnqMuster.MusterCareful = null;
                            //lnqMuster.MusterCarefulResult = null;
                            //lnqMuster.MusterCarefulResultReport = null;
                            lnqMuster.PSR = null;
                            lnqMuster.PSRQ = null;

                            //lnqMuster.AffirmResult = null;
                            lnqMuster.JLR = null;
                            lnqMuster.JLRQ = null;

                            //lnqMuster.EngineerAffirmResult = null;
                            //lnqMuster.EngineerMind = null;
                            //lnqMuster.EngineerRemainMusterDispose = null;
                            //lnqMuster.EngineerTestMusterCVTDispose = null;
                            //lnqMuster.TestAssemblyNumber = null;
                            //lnqMuster.TestResult = null;
                            lnqMuster.SYR = null;
                            lnqMuster.SYRQ = null;

                            //lnqMuster.IsEligbility = null;
                            //lnqMuster.SatrapAffirmResult = null;
                            //lnqMuster.SatrapMind = null;
                            //lnqMuster.SatrapRemainMusterDispose = null;
                            //lnqMuster.SatrapTestMusterCVTDispose = null;
                            lnqMuster.ZGR = null;
                            lnqMuster.ZGRQ = null;

                            //lnqMuster.ScrapCount = null;
                            //lnqMuster.ScrapDisposeMode = null;
                            //lnqMuster.EligbilityCount = null;
                            //lnqMuster.EjectableCount = null;
                            lnqMuster.CLR = null;
                            lnqMuster.CLRQ = null;

                            break;

                        case "等待检验":

                            strMsg = string.Format("{0}号样品确认申请单已回退，请您重新处理单据; 回退原因为" 
                                + rebackReason, djh);
                            m_billMessageServer.PassFlowMessage(djh, strMsg,
                                UniversalFunction.GetPersonnelCode(lnqMuster.JYR), false);


                            if (!DeleteMusterStock(dataContext, Convert.ToInt32(lnqMuster.GoodsID),
                                lnqMuster.BatchNo, lnqMuster.StorageID, out error))
                            {
                                return false;
                            }

                            if (!DeleteUseBill(dataContext, Convert.ToInt32(lnqMuster.GoodsID),
                                lnqMuster.BatchNo, lnqMuster.StorageID, out error))
                            {
                                return false;
                            }

                            lnqMuster.DJZT = "等待检验";

                            //lnqMuster.CheckScarpCount = null;
                            //lnqMuster.FeederReport = null;
                            //lnqMuster.CheckReport = null;
                            //lnqMuster.CheckResult = null;
                            //lnqMuster.MusterPack = null;
                            lnqMuster.JYR = null;
                            lnqMuster.JYRQ = null;

                            //lnqMuster.SQEExplain = null;
                            lnqMuster.SQE = null;
                            lnqMuster.SQERQ = null;

                            //lnqMuster.CraftMusterCareful = null;
                            lnqMuster.CraftMusterCarefulDate = null;
                            lnqMuster.CraftMusterCarefulPersonnel = null;
                            //lnqMuster.CraftMusterCarefulResult = null;
                            //lnqMuster.CraftMusterCarefulResultReport = null;

                            //lnqMuster.MusterCareful = null;
                            //lnqMuster.MusterCarefulResult = null;
                            //lnqMuster.MusterCarefulResultReport = null;
                            lnqMuster.PSR = null;
                            lnqMuster.PSRQ = null;

                            //lnqMuster.AffirmResult = null;
                            lnqMuster.JLR = null;
                            lnqMuster.JLRQ = null;

                            //lnqMuster.EngineerAffirmResult = null;
                            //lnqMuster.EngineerMind = null;
                            //lnqMuster.EngineerRemainMusterDispose = null;
                            //lnqMuster.EngineerTestMusterCVTDispose = null;
                            //lnqMuster.TestAssemblyNumber = null;
                            //lnqMuster.TestResult = null;
                            lnqMuster.SYR = null;
                            lnqMuster.SYRQ = null;

                            //lnqMuster.IsEligbility = null;
                            //lnqMuster.SatrapAffirmResult = null;
                            //lnqMuster.SatrapMind = null;
                            //lnqMuster.SatrapRemainMusterDispose = null;
                            //lnqMuster.SatrapTestMusterCVTDispose = null;
                            lnqMuster.ZGR = null;
                            lnqMuster.ZGRQ = null;

                            //lnqMuster.ScrapCount = null;
                            //lnqMuster.ScrapDisposeMode = null;
                            //lnqMuster.EligbilityCount = null;
                            //lnqMuster.EjectableCount = null;
                            lnqMuster.CLR = null;
                            lnqMuster.CLRQ = null;

                            break;
                        case "等待确认检验信息":

                            strMsg = string.Format("{0}号样品确认申请单已回退，请您重新处理单据; 回退原因为" 
                                + rebackReason, djh);

                            m_billMessageServer.PassFlowMessage(djh, strMsg,
                                UniversalFunction.GetPersonnelCode(lnqMuster.SQE), false);

                            lnqMuster.DJZT = "等待确认检验信息";

                            //lnqMuster.SQEExplain = null;
                            lnqMuster.SQE = null;
                            lnqMuster.SQERQ = null;

                            //lnqMuster.CraftMusterCareful = null;
                            lnqMuster.CraftMusterCarefulDate = null;
                            lnqMuster.CraftMusterCarefulPersonnel = null;
                            //lnqMuster.CraftMusterCarefulResult = null;
                            //lnqMuster.CraftMusterCarefulResultReport = null;

                            //lnqMuster.MusterCareful = null;
                            //lnqMuster.MusterCarefulResult = null;
                            //lnqMuster.MusterCarefulResultReport = null;
                            lnqMuster.PSR = null;
                            lnqMuster.PSRQ = null;

                            //lnqMuster.AffirmResult = null;
                            lnqMuster.JLR = null;
                            lnqMuster.JLRQ = null;

                            //lnqMuster.EngineerAffirmResult = null;
                            //lnqMuster.EngineerMind = null;
                            //lnqMuster.EngineerRemainMusterDispose = null;
                            //lnqMuster.EngineerTestMusterCVTDispose = null;
                            //lnqMuster.TestAssemblyNumber = null;
                            //lnqMuster.TestResult = null;
                            lnqMuster.SYR = null;
                            lnqMuster.SYRQ = null;

                            //lnqMuster.IsEligbility = null;
                            //lnqMuster.SatrapAffirmResult = null;
                            //lnqMuster.SatrapMind = null;
                            //lnqMuster.SatrapRemainMusterDispose = null;
                            //lnqMuster.SatrapTestMusterCVTDispose = null;
                            lnqMuster.ZGR = null;
                            lnqMuster.ZGRQ = null;

                            //lnqMuster.ScrapCount = null;
                            //lnqMuster.ScrapDisposeMode = null;
                            //lnqMuster.EligbilityCount = null;
                            //lnqMuster.EjectableCount = null;
                            lnqMuster.CLR = null;
                            lnqMuster.CLRQ = null;
                            break;


                        case "等待工艺工程师评审":

                            strMsg = string.Format("{0}号样品确认申请单已回退，请您重新处理单据; 回退原因为"
                                + rebackReason, djh);

                            m_billMessageServer.PassFlowMessage(djh, strMsg,
                                UniversalFunction.GetPersonnelCode(lnqMuster.PSR), false);

                            lnqMuster.DJZT = "等待工艺工程师评审";

                            //lnqMuster.CraftMusterCareful = null;
                            lnqMuster.CraftMusterCarefulDate = null;
                            lnqMuster.CraftMusterCarefulPersonnel = null;
                            //lnqMuster.CraftMusterCarefulResult = null;
                            //lnqMuster.CraftMusterCarefulResultReport = null;

                            //lnqMuster.MusterCareful = null;
                            //lnqMuster.MusterCarefulResult = null;
                            //lnqMuster.MusterCarefulResultReport = null;
                            lnqMuster.PSR = null;
                            lnqMuster.PSRQ = null;

                            //lnqMuster.AffirmResult = null;
                            lnqMuster.JLR = null;
                            lnqMuster.JLRQ = null;

                            //lnqMuster.EngineerAffirmResult = null;
                            //lnqMuster.EngineerMind = null;
                            //lnqMuster.EngineerRemainMusterDispose = null;
                            //lnqMuster.EngineerTestMusterCVTDispose = null;
                            //lnqMuster.TestAssemblyNumber = null;
                            //lnqMuster.TestResult = null;
                            lnqMuster.SYR = null;
                            lnqMuster.SYRQ = null;

                            //lnqMuster.IsEligbility = null;
                            //lnqMuster.SatrapAffirmResult = null;
                            //lnqMuster.SatrapMind = null;
                            //lnqMuster.SatrapRemainMusterDispose = null;
                            //lnqMuster.SatrapTestMusterCVTDispose = null;
                            lnqMuster.ZGR = null;
                            lnqMuster.ZGRQ = null;

                            //lnqMuster.ScrapCount = null;
                            //lnqMuster.ScrapDisposeMode = null;
                            //lnqMuster.EligbilityCount = null;
                            //lnqMuster.EjectableCount = null;
                            lnqMuster.CLR = null;
                            lnqMuster.CLRQ = null;
                            break;

                        case "等待零件工程师评审":

                            strMsg = string.Format("{0}号样品确认申请单已回退，请您重新处理单据; 回退原因为" 
                                + rebackReason, djh);

                            m_billMessageServer.PassFlowMessage(djh, strMsg,
                                UniversalFunction.GetPersonnelCode(lnqMuster.PSR), false);

                            lnqMuster.DJZT = "等待零件工程师评审";

                            //lnqMuster.MusterCareful = null;
                            //lnqMuster.MusterCarefulResult = null;
                            //lnqMuster.MusterCarefulResultReport = null;
                            lnqMuster.PSR = null;
                            lnqMuster.PSRQ = null;

                            //lnqMuster.AffirmResult = null;
                            lnqMuster.JLR = null;
                            lnqMuster.JLRQ = null;

                            //lnqMuster.EngineerAffirmResult = null;
                            //lnqMuster.EngineerMind = null;
                            //lnqMuster.EngineerRemainMusterDispose = null;
                            //lnqMuster.EngineerTestMusterCVTDispose = null;
                            //lnqMuster.TestAssemblyNumber = null;
                            //lnqMuster.TestResult = null;
                            lnqMuster.SYR = null;
                            lnqMuster.SYRQ = null;

                            //lnqMuster.IsEligbility = null;
                            //lnqMuster.SatrapAffirmResult = null;
                            //lnqMuster.SatrapMind = null;
                            //lnqMuster.SatrapRemainMusterDispose = null;
                            //lnqMuster.SatrapTestMusterCVTDispose = null;
                            lnqMuster.ZGR = null;
                            lnqMuster.ZGRQ = null;

                            //lnqMuster.ScrapCount = null;
                            //lnqMuster.ScrapDisposeMode = null;
                            //lnqMuster.EligbilityCount = null;
                            //lnqMuster.EjectableCount = null;
                            lnqMuster.CLR = null;
                            lnqMuster.CLRQ = null;
                            break;
                        case "等待项目经理确认":

                            strMsg = string.Format("{0}号样品确认申请单已回退，请您重新处理单据; 回退原因为" 
                                + rebackReason, djh);

                            m_billMessageServer.PassFlowMessage(djh, strMsg,
                                UniversalFunction.GetPersonnelCode(lnqMuster.JLR), false);

                            lnqMuster.DJZT = "等待项目经理确认";

                            //lnqMuster.AffirmResult = null;
                            lnqMuster.JLR = null;
                            lnqMuster.JLRQ = null;

                            //lnqMuster.EngineerAffirmResult = null;
                            //lnqMuster.EngineerMind = null;
                            //lnqMuster.EngineerRemainMusterDispose = null;
                            //lnqMuster.EngineerTestMusterCVTDispose = null;
                            //lnqMuster.TestAssemblyNumber = null;
                            //lnqMuster.TestResult = null;
                            lnqMuster.SYR = null;
                            lnqMuster.SYRQ = null;

                            //lnqMuster.IsEligbility = null;
                            //lnqMuster.SatrapAffirmResult = null;
                            //lnqMuster.SatrapMind = null;
                            //lnqMuster.SatrapRemainMusterDispose = null;
                            //lnqMuster.SatrapTestMusterCVTDispose = null;
                            lnqMuster.ZGR = null;
                            lnqMuster.ZGRQ = null;

                            //lnqMuster.ScrapCount = null;
                            //lnqMuster.ScrapDisposeMode = null;
                            //lnqMuster.EligbilityCount = null;
                            //lnqMuster.EjectableCount = null;
                            lnqMuster.CLR = null;
                            lnqMuster.CLRQ = null;
                            break;
                        case "等待试验结果":

                            strMsg = string.Format("{0}号样品确认申请单已回退，请您重新处理单据; 回退原因为" 
                                + rebackReason, djh);

                            m_billMessageServer.PassFlowMessage(djh, strMsg,
                                UniversalFunction.GetPersonnelCode(lnqMuster.SYR), false);

                            lnqMuster.DJZT = "等待试验结果";

                            //lnqMuster.EngineerAffirmResult = null;
                            //lnqMuster.EngineerMind = null;
                            //lnqMuster.EngineerRemainMusterDispose = null;
                            //lnqMuster.EngineerTestMusterCVTDispose = null;
                            //lnqMuster.TestAssemblyNumber = null;
                            //lnqMuster.TestResult = null;
                            lnqMuster.SYR = null;
                            lnqMuster.SYRQ = null;

                            //lnqMuster.IsEligbility = null;
                            //lnqMuster.SatrapAffirmResult = null;
                            //lnqMuster.SatrapMind = null;
                            //lnqMuster.SatrapRemainMusterDispose = null;
                            //lnqMuster.SatrapTestMusterCVTDispose = null;
                            lnqMuster.ZGR = null;
                            lnqMuster.ZGRQ = null;

                            //lnqMuster.ScrapCount = null;
                            //lnqMuster.ScrapDisposeMode = null;
                            //lnqMuster.EligbilityCount = null;
                            //lnqMuster.EjectableCount = null;
                            lnqMuster.CLR = null;
                            lnqMuster.CLRQ = null;

                            break;
                        case "等待主管确认":

                            strMsg = string.Format("{0}号样品确认申请单已回退，请您重新处理单据; 回退原因为" 
                                + rebackReason, djh);

                            m_billMessageServer.PassFlowMessage(djh, strMsg,
                                UniversalFunction.GetPersonnelCode(lnqMuster.ZGR), false);

                            lnqMuster.DJZT = "等待主管确认";

                            //lnqMuster.IsEligbility = null;
                            //lnqMuster.SatrapAffirmResult = null;
                            //lnqMuster.SatrapMind = null;
                            //lnqMuster.SatrapRemainMusterDispose = null;
                            //lnqMuster.SatrapTestMusterCVTDispose = null;
                            lnqMuster.ZGR = null;
                            lnqMuster.ZGRQ = null;

                            //lnqMuster.ScrapCount = null;
                            //lnqMuster.ScrapDisposeMode = null;
                            //lnqMuster.EligbilityCount = null;
                            //lnqMuster.EjectableCount = null;
                            lnqMuster.CLR = null;
                            lnqMuster.CLRQ = null;

                            break;
                        case "等待SQE处理":

                            strMsg = string.Format("{0}号样品确认申请单已回退，请您重新处理单据; 回退原因为" 
                                + rebackReason, djh);

                            m_billMessageServer.PassFlowMessage(djh, strMsg,
                                UniversalFunction.GetPersonnelCode(lnqMuster.CLR), false);

                            lnqMuster.DJZT = "等待SQE处理";

                            //lnqMuster.ScrapCount = null;
                            //lnqMuster.ScrapDisposeMode = null;
                            //lnqMuster.EligbilityCount = null;
                            //lnqMuster.EjectableCount = null;
                            lnqMuster.CLR = null;
                            lnqMuster.CLRQ = null;
                            break;
                        default:
                            break;
                    }

                    dataContext.SubmitChanges();

                    ISystemLogServer serverLog = ServerModuleFactory.GetServerModule<ISystemLogServer>();
                    string messge = string.Format("【单据状态】从【{0}】回退到【{1}】,回退原因:{2}",tempStatus, billStatus, rebackReason);
                    serverLog.RecordLog<S_MusterAffirmBill>(CE_OperatorMode.修改, messge, lnqMuster.DJH);
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
        /// 删除耗用表中的数据
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <param name="storageID">库房ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True,删除失败返回False</returns>
        private bool DeleteUseBill(DepotManagementDataContext context, int goodsID,string batchNo,string storageID,out string error)
        {
            try
            {
                error = null;

                string strSql = "select a.DJH as DJH, a.DJZT as DJZT from S_MusterUseBill as a " +
                    " inner join S_MusterUseList as b on a.DJH = b.DJH "+
                    " where b.GoodsID = "+ goodsID + " and b.BatchNo = '"+ batchNo +"'" +
                    " and a.StorageID = '"+ storageID +"'";


                DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["DJZT"].ToString() == "单据已完成")
                    {
                        throw new Exception("物品已耗用，无法回退");
                    }

                    var varBill = from a in context.S_MusterUseBill
                                  where a.DJH == dt.Rows[i]["DJH"].ToString()
                                  select a;

                    context.S_MusterUseBill.DeleteAllOnSubmit(varBill);
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
        /// 删除样品库库存信息
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <param name="storageID">库房ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        private bool DeleteMusterStock(DepotManagementDataContext context, int goodsID, string batchNo, string storageID, out string error)
        {
            try
            {
                error = null;

                var varData = from a in context.S_MusterStock
                              where a.StrorageID == storageID
                              && a.GoodsID == goodsID
                              && a.BatchNo == batchNo
                              select a;

                context.S_MusterStock.DeleteAllOnSubmit(varData);

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 获得样品库库存表
        /// </summary>
        /// <param name="flag">是否显示库存为0 的物品(True 显示，False 不显示)</param>
        /// <returns>返回样品库库存表</returns>
        public DataTable GetAllMusterStock(bool flag)
        {
            string strSql = "select * from View_S_MusterStock where 1=1 ";

            if (!flag)
            {
                strSql += " and 数量 <> 0";
            }

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dt;
        }

        /// <summary>
        /// 插入报废数的耗用单
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="inMuster">样品单信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>插入成功返回True，插入失败返回False</returns>
        private bool CreatMusterUseBill(DepotManagementDataContext context, S_MusterAffirmBill inMuster,out string error)
        {
            MusterUse serverMusterUse = new MusterUse();

            try
            {
                error = null;

                string djh = m_serverAssginBillNo.AssignNewNo(serverMusterUse, CE_BillTypeEnum.样品耗用单.ToString());

                S_MusterUseBill lnqUseBill = new S_MusterUseBill();

                lnqUseBill.DJH = djh;
                lnqUseBill.DJZT = "单据已完成";
                lnqUseBill.SQR = BasicInfo.LoginName;
                lnqUseBill.SQRQ = ServerTime.Time;
                lnqUseBill.StorageID = inMuster.StorageID;
                lnqUseBill.PurposeCode = UniversalFunction.GetPurpose(CE_PickingPurposeProperty.破坏性检测).Code;

                context.S_MusterUseBill.InsertOnSubmit(lnqUseBill);

                S_MusterUseList lnqUseList = new S_MusterUseList();

                lnqUseList.BatchNo = inMuster.BatchNo;
                lnqUseList.Count = inMuster.CheckScarpCount;
                lnqUseList.DJH = djh;
                lnqUseList.GoodsID = Convert.ToInt32(inMuster.GoodsID);
                lnqUseList.Remark = "由样品确认单【" + inMuster.DJH + "】自动生成，用于检测报废";

                context.S_MusterUseList.InsertOnSubmit(lnqUseList);

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 获得耗用数
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <returns>返货获得的耗用数</returns>
        public decimal GetUseCount(int goodsID,string batchNo)
        {
            string strSql = "select Sum(Count) from S_MusterUseList as a inner join S_MusterUseBill "+
                            " as b on a.DJH = b.DJH  where DJZT = '单据已完成' and "+
                            " GoodsID = " + goodsID + " and BatchNo = '" + batchNo + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dt.Rows.Count == 0)
            {
                return 0;
            }
            else
            {
                if (dt.Rows[0][0].ToString() == "")
                {
                    return 0;
                }
                else
                {
                    return Convert.ToDecimal(dt.Rows[0][0].ToString());
                }
            }
        }

        /// <summary>
        /// 修改样品库库存物品存放位置
        /// </summary>
        /// <param name="inMuster">样品单信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>修改成功返回True，修改失败返回False </returns>
        public bool UpdateMusterStockInfo(S_MusterStock inMuster,out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext contxt = CommentParameter.DepotDataContext;

                var varData = from a in contxt.S_MusterStock
                              where a.GoodsID == inMuster.GoodsID
                              && a.BatchNo == inMuster.BatchNo
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数据不唯一";
                    return false;
                }
                else
                {
                    varData.Single().ColumnNumber = inMuster.ColumnNumber;
                    varData.Single().ShelfAarea = inMuster.ShelfAarea;
                    varData.Single().LayerNumber = inMuster.LayerNumber;
                }

                contxt.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 获得库存数量
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <returns>返回获得的库存数量</returns>
        public decimal GetStockCount(int goodsID, string batchNo)
        {
            string strSql = "select Count from S_MusterStock where"+
                            " GoodsID = "+ goodsID +" and BatchNo = '"+ batchNo +"'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dt.Rows.Count == 0)
            {
                return 0;
            }
            else
            {
                if (dt.Rows[0][0].ToString() == "")
                {
                    return 0;
                }
                else
                {
                    return Convert.ToDecimal(dt.Rows[0][0].ToString());
                }
            }
        }
    }
}
