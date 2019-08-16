/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  CheckOutInDepotForOutsourcingServer.cs
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
    /// 委外报检入库管理类
    /// </summary>
    class CheckOutInDepotForOutsourcingServer:BasicServer, ServerModule.ICheckOutInDepotForOutsourcingServer
    {
        /// <summary>
        /// 部门信息服务组件
        /// </summary>
        IDepartmentServer m_serverDepartment = ServerModuleFactory.GetServerModule<IDepartmentServer>();

        /// <summary>
        /// 基础物品服务组件
        /// </summary>
        IBasicGoodsServer m_serverBasicGoods = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();

        /// <summary>
        /// 人员信息服务
        /// </summary>
        IPersonnelInfoServer m_personnelInfoServer = ServerModuleFactory.GetServerModule<IPersonnelInfoServer>();

        /// <summary>
        /// 条形码服务
        /// </summary>
        IBarCodeServer m_barCodeServer = ServerModuleFactory.GetServerModule<IBarCodeServer>();

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 单据编号分配服务
        /// </summary>
        IAssignBillNoServer m_assignBill = ServerModuleFactory.GetServerModule<IAssignBillNoServer>();

        /// <summary>
        /// 单据唯一码
        /// </summary>
        int m_billUniqueID = -1;

        /// <summary>
        /// 获得一条记录
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回LINQ S_CheckOutInDepotForOutsourcingBill 视图</returns>
        public S_CheckOutInDepotForOutsourcingBill GetBill(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.S_CheckOutInDepotForOutsourcingBill
                          where a.Bill_ID == billNo
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
        /// 检查某业务是否存在
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">业务号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.S_CheckOutInDepotForOutsourcingBill
                          where a.Bill_ID == billNo
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
            string sql = "SELECT * FROM [DepotManagement].[dbo].[S_CheckOutInDepotForOutsourcingBill] "+
                "where Bill_ID = '" + billNo + "'";

            System.Data.DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 查询显示的数据
        /// </summary>
        /// <param name="returnBill">返回 IQueryResult 数据集 </param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>查询成功返回True，查询失败返回False</returns>
        public bool GetBill(out IQueryResult returnBill, out string error)
        {
            returnBill = null;
            error = null;

            IAuthorization m_authorization = PlatformFactory.GetObject<IAuthorization>();
            IQueryResult qr = null;

            if (QueryResultFilter == null)
            {
                qr = m_authorization.Query("委外报检入库单查询", null);
            }
            else
            {
                qr = m_authorization.Query("委外报检入库单查询", null, QueryResultFilter);
            }

            if (!qr.Succeeded)
            {
                error = qr.Error;
                return false;
            }

            if (m_billUniqueID < 0)
            {
                IBillTypeServer server = ServerModuleFactory.GetServerModule<IBillTypeServer>();
                BASE_BillType billType = server.GetBillTypeFromName("报检入库单");

                if (billType == null)
                {
                    error = "获取不到单据类型信息";
                    return false;
                }

                m_billUniqueID = billType.UniqueID;
            }

            returnBill = qr;
            return true;
        }

        /// <summary>
        /// 插入信息
        /// </summary>
        /// <param name="outSourcing">Linq数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>插入成功返回True，插入失败返回False</returns>
        public bool AddBill(S_CheckOutInDepotForOutsourcingBill outSourcing,out string error)
        {
            error = null;

            try
            {
                S_CheckOutInDepotForOutsourcingBill lnqOutsouring = new S_CheckOutInDepotForOutsourcingBill();

                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.S_CheckOutInDepotForOutsourcingBill
                              where a.Bill_ID == outSourcing.Bill_ID
                              select a;

                if (varData.Count() == 1)
                {
                    lnqOutsouring = varData.Single();

                    lnqOutsouring.Bill_ID = outSourcing.Bill_ID;
                    lnqOutsouring.BillStatus = outSourcing.IsIncludeRawMaterial == true ? "等待财务批准" : "等待仓管确认到货";
                    lnqOutsouring.OrderFormNumber = outSourcing.OrderFormNumber;
                    lnqOutsouring.GoodsID = outSourcing.GoodsID;
                    lnqOutsouring.Depot = outSourcing.Depot;
                    lnqOutsouring.BatchNo = outSourcing.BatchNo;
                    lnqOutsouring.Provider = outSourcing.Provider;
                    lnqOutsouring.ProviderBatchNo = outSourcing.ProviderBatchNo;
                    lnqOutsouring.DeclareCount = outSourcing.DeclareCount;
                    lnqOutsouring.UnitPrice = outSourcing.UnitPrice;
                    lnqOutsouring.Price = outSourcing.Price;
                    lnqOutsouring.Remark = outSourcing.Remark;
                    lnqOutsouring.StorageID = outSourcing.StorageID;
                    lnqOutsouring.Version = outSourcing.Version;
                    lnqOutsouring.IsExigenceCheck = outSourcing.IsExigenceCheck;
                    lnqOutsouring.IsIncludeRawMaterial = outSourcing.IsIncludeRawMaterial;
                    lnqOutsouring.RawMaterialPrice = outSourcing.RawMaterialPrice;
                    lnqOutsouring.DeclarePersonnel = outSourcing.DeclarePersonnel;
                    lnqOutsouring.DeclareTime = outSourcing.DeclareTime;
                    lnqOutsouring.OutsourcingUnitPrice = outSourcing.OutsourcingUnitPrice;
                }
                else if (varData.Count() == 0)
                {
                    lnqOutsouring = new S_CheckOutInDepotForOutsourcingBill();

                    lnqOutsouring.Bill_ID = outSourcing.Bill_ID;
                    lnqOutsouring.BillStatus = outSourcing.IsIncludeRawMaterial == true ? "等待财务批准" : "等待仓管确认到货";
                    lnqOutsouring.OrderFormNumber = outSourcing.OrderFormNumber;
                    lnqOutsouring.GoodsID = outSourcing.GoodsID;
                    lnqOutsouring.Depot = outSourcing.Depot;
                    lnqOutsouring.BatchNo = outSourcing.BatchNo;
                    lnqOutsouring.Provider = outSourcing.Provider;
                    lnqOutsouring.ProviderBatchNo = outSourcing.ProviderBatchNo;
                    lnqOutsouring.DeclareCount = outSourcing.DeclareCount;
                    lnqOutsouring.UnitPrice = outSourcing.UnitPrice;
                    lnqOutsouring.Price = outSourcing.Price;
                    lnqOutsouring.Remark = outSourcing.Remark;
                    lnqOutsouring.StorageID = outSourcing.StorageID;
                    lnqOutsouring.Version = outSourcing.Version;
                    lnqOutsouring.IsExigenceCheck = outSourcing.IsExigenceCheck;
                    lnqOutsouring.IsIncludeRawMaterial = outSourcing.IsIncludeRawMaterial;
                    lnqOutsouring.RawMaterialPrice = outSourcing.RawMaterialPrice;
                    lnqOutsouring.DeclarePersonnel = outSourcing.DeclarePersonnel;
                    lnqOutsouring.DeclareTime = outSourcing.DeclareTime;
                    lnqOutsouring.OutsourcingUnitPrice = outSourcing.OutsourcingUnitPrice;

                    ctx.S_CheckOutInDepotForOutsourcingBill.InsertOnSubmit(lnqOutsouring);
                }
                else
                {
                    error = "数据不唯一";
                    return false;
                }

                if (GlobalObject.GeneralFunction.IsNullOrEmpty(lnqOutsouring.Bill_ID))
                {
                    throw new Exception("【单据号】获取失败，请重新再试");
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
        /// 更新单据状态
        /// </summary>
        /// <param name="outSourcing">Linq操作数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>更新成功返回True，更新失败返回False</returns>
        public bool UpdateBill(S_CheckOutInDepotForOutsourcingBill outSourcing, out string error)
        {
            error = null;

            string mrBillNo = null;

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();
            try
            {
                var varData = from a in ctx.S_CheckOutInDepotForOutsourcingBill
                              where a.Bill_ID == outSourcing.Bill_ID
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数据为空或者不唯一";
                    throw new Exception(error);
                }
                else
                {
                    S_CheckOutInDepotForOutsourcingBill lnqOutsourcing = varData.Single();

                    if (lnqOutsourcing.BillStatus != outSourcing.BillStatus)
                    {
                        error = "单据状态错误，请重新刷新单据确认单据状态";
                        throw new Exception(error);
                    }

                    switch (lnqOutsourcing.BillStatus )
                    {
                        case "等待财务批准":
                            lnqOutsourcing.BillStatus = "等待仓管确认到货";
                            lnqOutsourcing.UnitPrice = outSourcing.UnitPrice;
                            lnqOutsourcing.Price = outSourcing.Price;
                            lnqOutsourcing.RawMaterialPrice = outSourcing.RawMaterialPrice;
                            lnqOutsourcing.FinancePersonnel = BasicInfo.LoginName;
                            lnqOutsourcing.FinanceTime = ServerTime.Time;
                            break;
                        case "等待仓管确认到货":
                            lnqOutsourcing.BillStatus = outSourcing.QualityInfo;
                            lnqOutsourcing.DepotManagerAffirmCount = outSourcing.DepotManagerAffirmCount;
                            lnqOutsourcing.Price = outSourcing.Price;
                            lnqOutsourcing.ArrivePersonnel = BasicInfo.LoginName;
                            lnqOutsourcing.ArriveTime = ServerTime.Time;
                            break;
                        case "等待质检机检验":
                            lnqOutsourcing.BillStatus = "等待质量主管审核";
                            lnqOutsourcing.CheckoutReport_ID = outSourcing.CheckoutReport_ID;
                            lnqOutsourcing.Checker = outSourcing.Checker;
                            lnqOutsourcing.EligibleCount = outSourcing.EligibleCount;
                            lnqOutsourcing.ConcessionCount = outSourcing.ConcessionCount;
                            lnqOutsourcing.ReimbursementCount = outSourcing.ReimbursementCount;
                            lnqOutsourcing.DeclareWastrelCount = outSourcing.DeclareWastrelCount;
                            lnqOutsourcing.QualityInfo = outSourcing.QualityInfo;
                            lnqOutsourcing.PeremptorilyEmit = outSourcing.PeremptorilyEmit;
                            lnqOutsourcing.QualityPersonnel = BasicInfo.LoginName;
                            lnqOutsourcing.QualityTime = ServerTime.Time;
                            break;
                        case "等待质检电检验":
                            lnqOutsourcing.BillStatus = "等待质量主管审核";
                            lnqOutsourcing.CheckoutReport_ID = outSourcing.CheckoutReport_ID;
                            lnqOutsourcing.Checker = outSourcing.Checker;
                            lnqOutsourcing.EligibleCount = outSourcing.EligibleCount;
                            lnqOutsourcing.ConcessionCount = outSourcing.ConcessionCount;
                            lnqOutsourcing.ReimbursementCount = outSourcing.ReimbursementCount;
                            lnqOutsourcing.DeclareWastrelCount = outSourcing.DeclareWastrelCount;
                            lnqOutsourcing.QualityInfo = outSourcing.QualityInfo;
                            lnqOutsourcing.PeremptorilyEmit = outSourcing.PeremptorilyEmit;
                            lnqOutsourcing.QualityPersonnel = BasicInfo.LoginName;
                            lnqOutsourcing.QualityTime = ServerTime.Time;
                            break;
                        case "等待质量主管审核":
                            lnqOutsourcing.BillStatus = "等待入库";
                            lnqOutsourcing.IsOnlyForRepairFlag = outSourcing.IsOnlyForRepairFlag;
                            lnqOutsourcing.QASupervisor = BasicInfo.LoginName;
                            lnqOutsourcing.QASupervisorTime = ServerTime.Time;
                            break;
                        case "等待入库":
                            lnqOutsourcing.BillStatus = "已入库";
                            lnqOutsourcing.Price = outSourcing.Price;
                            lnqOutsourcing.InDepotCount = outSourcing.InDepotCount;
                            lnqOutsourcing.ShelfArea = outSourcing.ShelfArea;
                            lnqOutsourcing.ColumnNumber = outSourcing.ColumnNumber;
                            lnqOutsourcing.LayerNumber = outSourcing.LayerNumber;
                            lnqOutsourcing.ManagerPersonnel = BasicInfo.LoginName;
                            lnqOutsourcing.ManagerTime = ServerTime.Time;

                            // 添加信息到入库明细表
                            OpertaionDetailAndStock(ctx, lnqOutsourcing);

                            //若勾选了“包含原材料费”并且报废数大于0，则插入报废单
                            if (outSourcing.IsExigenceCheck && outSourcing.DeclareWastrelCount > 0)
                            {
                                if (!AddScrapBill(ctx, lnqOutsourcing, out error))
                                {
                                    throw new Exception(error);
                                }
                            }

                            ctx.SubmitChanges();

                            if ((int)outSourcing.DeclareWastrelCount > 0 && outSourcing.InDepotCount > 0)
                            {
                                if (!InsertIntoMaterialRequisition(ctx, outSourcing, out mrBillNo, out error))
                                {
                                    m_assignBill.CancelBillNo(CE_BillTypeEnum.领料单.ToString(), mrBillNo);
                                    ReturnBillInDepot(lnqOutsourcing.Bill_ID, mrBillNo, out error);
                                    throw new Exception(error);
                                }
                            }

                            break;
                        default:
                            break;
                    }

                    ctx.SubmitChanges();
                }

                ctx.Transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                ctx.Transaction.Rollback();
                error = ex.Message;
                return false;
                
            }
        }

        /// <summary>
        /// 回退单据
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="mrBillID">领料单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>回退成功返回True，回退失败返回False</returns>
        private bool ReturnBillInDepot(string billID, string mrBillID, out string error)
        {
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                int intGoodsID = 0;
                string strBatchNo = "";
                decimal dcCount = 0;
                decimal dcWastrel = 0;
                //报检单状态修改
                var varData1 = from a in dataContxt.S_CheckOutInDepotForOutsourcingBill
                                where a.Bill_ID == billID
                                select a;

                if (varData1.Count() == 1)
                {
                    S_CheckOutInDepotForOutsourcingBill lnqCheck = varData1.Single();

                    intGoodsID = Convert.ToInt32(lnqCheck.GoodsID);
                    strBatchNo = lnqCheck.BatchNo.ToString();
                    dcCount = Convert.ToDecimal(lnqCheck.EligibleCount);
                    dcWastrel = Convert.ToDecimal(lnqCheck.DeclareWastrelCount);
                    lnqCheck.BillStatus = "等待入库";
                }

                //库存记录的删除/修改
                var varData2 = from a in dataContxt.S_Stock
                                where a.GoodsID == intGoodsID
                                && a.BatchNo == strBatchNo
                                select a;

                if (varData2.Count() == 1)
                {
                    S_Stock lnqStock = varData2.Single();

                    if (Convert.ToDecimal(lnqStock.ExistCount) == dcCount)
                    {
                        dataContxt.S_Stock.DeleteOnSubmit(lnqStock);
                    }
                    else
                    {
                        lnqStock.ExistCount = Convert.ToDecimal(lnqStock.ExistCount) - dcCount;
                    }
                }

                //入库明细表记录删除
                var varData3 = from a in dataContxt.S_InDepotDetailBill
                                where a.InDepotBillID == billID
                                && a.BatchNo == strBatchNo
                                && a.GoodsID == intGoodsID
                                select a;

                if (varData3.Count() == 1)
                {
                    dataContxt.S_InDepotDetailBill.DeleteOnSubmit(varData3.Single());
                }

                //领料单主表删除
                var varData4 = from a in dataContxt.S_MaterialRequisition
                                where a.Bill_ID == mrBillID
                                && a.PurposeCode == UniversalFunction.GetPurpose(CE_PickingPurposeProperty.破坏性检测).Code
                                select a;

                if (varData4.Count() == 1)
                {
                    dataContxt.S_MaterialRequisition.DeleteOnSubmit(varData4.Single());
                }

                //领料单此表删除
                var varData5 = from a in dataContxt.S_MaterialRequisitionGoods
                                where a.Bill_ID == mrBillID
                                && a.GoodsID == intGoodsID
                                && a.BatchNo == strBatchNo
                                select a;

                if (varData5.Count() == 1)
                {
                    dataContxt.S_MaterialRequisitionGoods.DeleteOnSubmit(varData5.Single());
                }

                //出库明细表记录删除
                var varData6 = from a in dataContxt.S_FetchGoodsDetailBill
                                where a.FetchBIllID == mrBillID
                                && a.BatchNo == strBatchNo
                                && a.GoodsID == intGoodsID
                                select a;

                if (varData6.Count() == 1)
                {
                    dataContxt.S_FetchGoodsDetailBill.DeleteOnSubmit(varData6.Single());
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
        /// 有检测废的物品直接生成领料单
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="inDepotInfo">报检单信息</param>
        /// <param name="mrBillNo">分配的领料单单号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>生成成功返回True，生成失败返回False</returns>
        public bool InsertIntoMaterialRequisition(DepotManagementDataContext ctx, S_CheckOutInDepotForOutsourcingBill inDepotInfo, 
            out string mrBillNo, out string error)
        {
            error = null;
            mrBillNo = null;
            string billNo = null;

            MaterialRequisitionServer serverMaterialBill = new MaterialRequisitionServer();

            try
            {

                billNo = m_assignBill.AssignNewNo(serverMaterialBill, CE_BillTypeEnum.领料单.ToString());
                mrBillNo = billNo;

                var varData = from a in ctx.S_MaterialRequisition
                              where a.Bill_ID == billNo
                              select a;

                S_MaterialRequisition lnqMaterial = null;


                if (varData.Count() != 0)
                {
                    error = string.Format("自动生成的报废物品领料单单号 {0} 已被占用，请尝试重新进行此操作，再三出现无法生成可用的单号时与管理员联系", billNo);
                    return false;
                }
                else
                {
                    lnqMaterial = new S_MaterialRequisition();

                    lnqMaterial.Bill_ID = billNo;
                    lnqMaterial.Bill_Time = ServerModule.ServerTime.Time;
                    lnqMaterial.AssociatedBillNo = inDepotInfo.Bill_ID;
                    lnqMaterial.AssociatedBillType = "";
                    lnqMaterial.BillStatus = "已出库";
                    lnqMaterial.Department = "ZK03";
                    lnqMaterial.DepartmentDirector = "";
                    lnqMaterial.DepotManager = inDepotInfo.ManagerPersonnel;
                    lnqMaterial.FetchCount = 0;
                    lnqMaterial.FetchType = "零星领料";
                    lnqMaterial.FillInPersonnel = inDepotInfo.QualityPersonnel;
                    lnqMaterial.FillInPersonnelCode = 
                        UniversalFunction.GetPersonnelInfo(inDepotInfo.QualityPersonnel, CE_HR_PersonnelStatus.在职).工号;
                    lnqMaterial.ProductType = "";
                    lnqMaterial.PurposeCode = UniversalFunction.GetPurpose(CE_PickingPurposeProperty.破坏性检测).Code;
                    lnqMaterial.Remark = "因入库零件进行了破坏性检测，由系统自动生成的破坏件领料单，对应单据号：" + inDepotInfo.Bill_ID;
                    lnqMaterial.StorageID = inDepotInfo.StorageID;
                    lnqMaterial.OutDepotDate = ServerTime.Time;

                    if (!serverMaterialBill.AutoCreateBill(ctx, lnqMaterial, out error))
                    {
                        return false;
                    }

                    S_MaterialRequisitionGoods lnqMaterialGoods = new S_MaterialRequisitionGoods();

                    lnqMaterialGoods.Bill_ID = billNo;
                    lnqMaterialGoods.BasicCount = 0;
                    lnqMaterialGoods.BatchNo = inDepotInfo.BatchNo;
                    lnqMaterialGoods.GoodsID = inDepotInfo.GoodsID;
                    lnqMaterialGoods.ProviderCode = inDepotInfo.Provider;
                    lnqMaterialGoods.RealCount = Convert.ToDecimal(inDepotInfo.DeclareWastrelCount);
                    lnqMaterialGoods.Remark = "";
                    lnqMaterialGoods.RequestCount = Convert.ToDecimal(inDepotInfo.DeclareWastrelCount);
                    lnqMaterialGoods.ShowPosition = 1;

                    MaterialRequisitionGoodsServer serverMaterialGoods = new MaterialRequisitionGoodsServer();

                    if (!serverMaterialGoods.AutoCreateGoods(ctx, lnqMaterialGoods, out error))
                    {
                        return false;
                    }

                    ctx.SubmitChanges();

                    if (!serverMaterialBill.FinishBill(ctx, lnqMaterial.Bill_ID, "", out error))
                    {
                        throw new Exception(error);
                    }

                    ctx.SubmitChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                serverMaterialBill.DeleteBill(billNo, out error);
                error = ex.Message;
                return false;
            }
        }


        /// <summary>
        /// 根据单据信息操作账务信息与库存信息
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="bill">单据信息</param>
        public void OpertaionDetailAndStock(DepotManagementDataContext dataContext, S_CheckOutInDepotForOutsourcingBill bill)
        {
            IFinancialDetailManagement serverDetail =
                ServerModule.ServerModuleFactory.GetServerModule<IFinancialDetailManagement>();

            S_InDepotDetailBill detailInfo = AssignDetailInfo(dataContext, bill);
            S_Stock stockInfo = AssignStockInfo(dataContext, bill);

            if (detailInfo == null || stockInfo == null)
            {
                throw new Exception("获取账务信息或者库存信息失败");
            }

            if (bill.DeclareCount < bill.InDepotCount)
            {
                throw new Exception("入库数不能大于报检数");
            }

            serverDetail.ProcessInDepotDetail(dataContext, detailInfo, stockInfo);
        }

        /// <summary>
        /// 赋值库存信息
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="outSourcing">单据信息</param>
        /// <returns>返回库存信息对象</returns>
        S_Stock AssignStockInfo(DepotManagementDataContext ctx, S_CheckOutInDepotForOutsourcingBill outSourcing)
        {
            View_F_GoodsPlanCost tempGoodsLnq = UniversalFunction.GetGoodsInfo(ctx, outSourcing.GoodsID);

            if (tempGoodsLnq == null)
            {
                throw new Exception("无此物品信息");
            }

            S_Stock stock = new S_Stock();

            stock.GoodsID = outSourcing.GoodsID;
            stock.GoodsCode = tempGoodsLnq.图号型号;
            stock.GoodsName = tempGoodsLnq.物品名称;
            stock.Unit = tempGoodsLnq.单位;
            stock.Spec = tempGoodsLnq.规格;
            stock.Provider = outSourcing.Provider;
            stock.ProviderBatchNo = outSourcing.ProviderBatchNo;
            stock.BatchNo = outSourcing.BatchNo;
            stock.ShelfArea = outSourcing.ShelfArea;
            stock.ColumnNumber = outSourcing.ColumnNumber;
            stock.LayerNumber = outSourcing.LayerNumber;
            stock.ExistCount = outSourcing.InDepotCount;
            stock.Date = ServerModule.ServerTime.Time;
            stock.UnitPrice = outSourcing.UnitPrice;
            stock.StorageID = outSourcing.StorageID;
            stock.Version = outSourcing.Version;
            stock.GoodsStatus = outSourcing.IsOnlyForRepairFlag == true ? 6 : 0;

            return stock;
        }

        /// <summary>
        /// 赋值账务信息
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="outSourcing">单据信息</param>
        /// <returns>返回账务信息</returns>
        S_InDepotDetailBill AssignDetailInfo(DepotManagementDataContext ctx, S_CheckOutInDepotForOutsourcingBill outSourcing)
        {
            S_InDepotDetailBill detailBill = new S_InDepotDetailBill();
            var varData = from a in ctx.S_InDepotDetailBill
                          where a.InDepotBillID == outSourcing.Bill_ID
                          select a;

            if (varData.Count() != 0)
            {
                throw new Exception("数据不唯一");
            }
            else
            {
                IBillTypeServer server = ServerModuleFactory.GetServerModule<IBillTypeServer>();
                BASE_BillType billType = server.GetBillTypeFromName("委外报检入库单");

                if (billType == null)
                {
                    throw new Exception("获取不到单据类型信息");
                }

                detailBill.ID = Guid.NewGuid();
                detailBill.BillTime = ServerTime.Time;
                detailBill.FillInPersonnel = outSourcing.DeclarePersonnel;
                detailBill.Department = UniversalFunction.GetPersonnelInfo(ctx, outSourcing.DeclarePersonnel).部门名称;
                detailBill.FactPrice = Math.Round(outSourcing.UnitPrice * outSourcing.InDepotCount, 2);
                detailBill.FactUnitPrice = outSourcing.UnitPrice;
                detailBill.GoodsID = outSourcing.GoodsID;
                detailBill.BatchNo = outSourcing.BatchNo;
                detailBill.InDepotBillID = outSourcing.Bill_ID;
                detailBill.InDepotCount = outSourcing.InDepotCount;
                detailBill.Price = Math.Round(outSourcing.UnitPrice * outSourcing.InDepotCount, 2);
                detailBill.UnitPrice = outSourcing.UnitPrice;
                detailBill.OperationType = (int)CE_SubsidiaryOperationType.委外报检入库;
                detailBill.Provider = outSourcing.Provider;
                detailBill.StorageID = outSourcing.StorageID;
                detailBill.AffrimPersonnel = outSourcing.ManagerPersonnel;
                detailBill.FillInDate = outSourcing.DeclareTime;
            }

            return detailBill;
        }

        /// <summary>
        /// 插入报废单
        /// </summary>
        /// <param name="ctx">LINQ数据上下文</param>
        /// <param name="outSourcing">Linq操作数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>插入成功返回True，插入失败返回False</returns>
        public bool AddScrapBill(DepotManagementDataContext ctx, 
            S_CheckOutInDepotForOutsourcingBill outSourcing, out string error)
        {
            error = null;
            string strBillID = "";

            try
            {
                ScrapBillServer serverScrapBill = new ScrapBillServer();

                S_ScrapBill lnqBill = new S_ScrapBill();

                strBillID = m_assignBill.AssignNewNo(serverScrapBill,CE_BillTypeEnum.报废单.ToString());

                lnqBill.AuthorizeTime = ServerTime.Time;
                lnqBill.Bill_ID = strBillID;
                lnqBill.Bill_Time = ServerTime.Time;
                lnqBill.BillStatus = "已完成";
                lnqBill.Checker = outSourcing.Checker;
                lnqBill.DeclareDepartment = m_serverDepartment.GetDeptInfoFromPersonnelInfo(
                    outSourcing.DeclarePersonnel).Rows[0]["DepartmentCode"].ToString();
                lnqBill.DepartmentDirector = "";
                lnqBill.DepotManager = outSourcing.ManagerPersonnel;
                lnqBill.DepotTime = ServerTime.Time;
                lnqBill.FillInPersonnel = outSourcing.DeclarePersonnel;
                lnqBill.FillInPersonnelCode = UniversalFunction.GetPersonnelCode(outSourcing.DeclarePersonnel);
                lnqBill.NotifyChecker = UniversalFunction.GetPersonnelCode(outSourcing.Checker);
                lnqBill.ProductType = "";
                lnqBill.Remark = "关联委外报检入库单【"+ outSourcing.Bill_ID +"】,自动生成";
                lnqBill.Sanction = outSourcing.Checker;

                ctx.S_ScrapBill.InsertOnSubmit(lnqBill);

                S_ScrapGoods lnqGoods = new S_ScrapGoods();

                lnqGoods.BatchNo = outSourcing.BatchNo;
                lnqGoods.Bill_ID = strBillID;
                lnqGoods.GoodsID = outSourcing.GoodsID;
                lnqGoods.Provider = outSourcing.Provider;
                lnqGoods.Quantity = outSourcing.DeclareWastrelCount;
                lnqGoods.Reason = "检测报废，关联委外报检入库单【" + outSourcing.Bill_ID + "】,自动生成";
                lnqGoods.Remark = "检测报废，关联委外报检入库单【" + outSourcing.Bill_ID + "】,自动生成";
                lnqGoods.ResponsibilityDepartment = "GYS";
                lnqGoods.ResponsibilityProvider = outSourcing.Provider;
                lnqGoods.ResponsibilityType = "返 修 废";
                lnqGoods.UnitPrice = 0;
                lnqGoods.WastrelType = 9;
                lnqGoods.WorkingHours = 0;

                ctx.S_ScrapGoods.InsertOnSubmit(lnqGoods);

                m_assignBill.UseBillNo(CE_BillTypeEnum.报废单.ToString(), strBillID);

                return true;
            }
            catch (Exception ex)
            {
                m_assignBill.CancelBillNo(CE_BillTypeEnum.报废单.ToString(),strBillID);
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
        public bool DeleteBill(string billNo,out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.S_CheckOutInDepotForOutsourcingBill
                              where a.Bill_ID == billNo
                              select a;

                ctx.S_CheckOutInDepotForOutsourcingBill.DeleteAllOnSubmit(varData);
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
        /// 回退单据
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <param name="billStatus">单据状态</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <param name="rebackReason">回退原因</param>
        /// <returns>回退成功返回True，回退失败返回False</returns>
        public bool ReturnBill(string djh, string billStatus, 
            out string error, string rebackReason)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.S_CheckOutInDepotForOutsourcingBill
                              where a.Bill_ID == djh
                              select a;

                string strMsg = "";

                if (varData.Count() == 1)
                {
                    S_CheckOutInDepotForOutsourcingBill lnqOutsourcing = varData.Single();

                    switch (billStatus)
                    {
                        case "新建单据":

                            strMsg = string.Format("{0}号委外报检入库单已回退，请您重新处理单据; 回退原因为" + rebackReason, djh);
                            m_billMessageServer.PassFlowMessage(djh, strMsg,
                                UniversalFunction.GetPersonnelCode(lnqOutsourcing.DeclarePersonnel), false);

                            lnqOutsourcing.BillStatus = "新建单据";

                            //lnqOutsourcing.CheckoutReport_ID = null;
                            //lnqOutsourcing.Checker = null;
                            //lnqOutsourcing.EligibleCount = 0;
                            //lnqOutsourcing.ConcessionCount = 0;
                            //lnqOutsourcing.ReimbursementCount = 0;
                            //lnqOutsourcing.DeclareWastrelCount = 0;
                            //lnqOutsourcing.QualityInfo = null;
                            //lnqOutsourcing.PeremptorilyEmit = false;
                            lnqOutsourcing.QualityPersonnel = null;
                            lnqOutsourcing.QualityTime = null;

                            //lnqOutsourcing.DepotManagerAffirmCount = 0;
                            lnqOutsourcing.ArrivePersonnel = null;
                            lnqOutsourcing.ArriveTime = null;

                            //lnqOutsourcing.Price = Math.Round(lnqOutsourcing.OutsourcingUnitPrice * lnqOutsourcing.DeclareCount, 2);
                            //lnqOutsourcing.UnitPrice = lnqOutsourcing.OutsourcingUnitPrice;
                            //lnqOutsourcing.RawMaterialPrice = 0;
                            lnqOutsourcing.FinancePersonnel = null;
                            lnqOutsourcing.FinanceTime = null;

                            break;
                        case "等待财务批准":
                            strMsg = string.Format("{0}号委外报检入库单已回退，请您重新处理单据; 回退原因为" + rebackReason, djh);
                            m_billMessageServer.PassFlowMessage(djh, strMsg,
                                UniversalFunction.GetPersonnelCode(lnqOutsourcing.FinancePersonnel), false);

                            lnqOutsourcing.BillStatus = "等待财务批准";

                            //lnqOutsourcing.CheckoutReport_ID = null;
                            //lnqOutsourcing.Checker = null;
                            //lnqOutsourcing.EligibleCount = 0;
                            //lnqOutsourcing.ConcessionCount = 0;
                            //lnqOutsourcing.ReimbursementCount = 0;
                            //lnqOutsourcing.DeclareWastrelCount = 0;
                            //lnqOutsourcing.QualityInfo = null;
                            //lnqOutsourcing.PeremptorilyEmit = false;
                            lnqOutsourcing.QualityPersonnel = null;
                            lnqOutsourcing.QualityTime = null;

                            //lnqOutsourcing.DepotManagerAffirmCount = 0;
                            lnqOutsourcing.ArrivePersonnel = null;
                            lnqOutsourcing.ArriveTime = null;

                            //lnqOutsourcing.Price = Math.Round(lnqOutsourcing.OutsourcingUnitPrice * lnqOutsourcing.DeclareCount, 2);
                            //lnqOutsourcing.UnitPrice = lnqOutsourcing.OutsourcingUnitPrice;
                            //lnqOutsourcing.RawMaterialPrice = 0;
                            lnqOutsourcing.FinancePersonnel = null;
                            lnqOutsourcing.FinanceTime = null;

                            break;
                        case "等待仓管确认到货":
                            strMsg = string.Format("{0}号委外报检入库单已回退，请您重新处理单据; 回退原因为" + rebackReason, djh);
                            m_billMessageServer.PassFlowMessage(djh, strMsg,
                                UniversalFunction.GetPersonnelCode(lnqOutsourcing.ArrivePersonnel), false);

                            lnqOutsourcing.BillStatus = "等待仓管确认到货";

                            //lnqOutsourcing.CheckoutReport_ID = null;
                            //lnqOutsourcing.Checker = null;
                            //lnqOutsourcing.EligibleCount = 0;
                            //lnqOutsourcing.ConcessionCount = 0;
                            //lnqOutsourcing.ReimbursementCount = 0;
                            //lnqOutsourcing.DeclareWastrelCount = 0;
                            //lnqOutsourcing.QualityInfo = null;
                            //lnqOutsourcing.PeremptorilyEmit = false;
                            lnqOutsourcing.QualityPersonnel = null;
                            lnqOutsourcing.QualityTime = null;

                            //lnqOutsourcing.DepotManagerAffirmCount = 0; 
                            //lnqOutsourcing.Price = Math.Round(lnqOutsourcing.UnitPrice * lnqOutsourcing.DeclareCount, 2);
                            lnqOutsourcing.ArrivePersonnel = null;
                            lnqOutsourcing.ArriveTime = null;

                            break;
                        case "等待质检机检验":
                            strMsg = string.Format("{0}号委外报检入库单已回退，请您重新处理单据; 回退原因为" + rebackReason, djh);
                            m_billMessageServer.PassFlowMessage(djh, strMsg,
                                UniversalFunction.GetPersonnelCode(lnqOutsourcing.QualityPersonnel), false);

                            lnqOutsourcing.BillStatus = "等待质检机检验";
                            //lnqOutsourcing.CheckoutReport_ID = null;
                            //lnqOutsourcing.Checker = null;
                            //lnqOutsourcing.EligibleCount = 0;
                            //lnqOutsourcing.ConcessionCount = 0;
                            //lnqOutsourcing.ReimbursementCount = 0;
                            //lnqOutsourcing.DeclareWastrelCount = 0;
                            //lnqOutsourcing.QualityInfo = null;
                            //lnqOutsourcing.PeremptorilyEmit = false;
                            lnqOutsourcing.QualityPersonnel = null;
                            lnqOutsourcing.QualityTime = null;

                            break;
                        case "等待质检电检验":

                            strMsg = string.Format("{0}号委外报检入库单已回退，请您重新处理单据; 回退原因为" + rebackReason, djh);
                            m_billMessageServer.PassFlowMessage(djh, strMsg,
                                UniversalFunction.GetPersonnelCode(lnqOutsourcing.QualityPersonnel), false);

                            lnqOutsourcing.BillStatus = "等待质检电检验";
                            //lnqOutsourcing.CheckoutReport_ID = null;
                            //lnqOutsourcing.Checker = null;
                            //lnqOutsourcing.EligibleCount = 0;
                            //lnqOutsourcing.ConcessionCount = 0;
                            //lnqOutsourcing.ReimbursementCount = 0;
                            //lnqOutsourcing.DeclareWastrelCount = 0;
                            //lnqOutsourcing.QualityInfo = null;
                            //lnqOutsourcing.PeremptorilyEmit = false;
                            lnqOutsourcing.QualityPersonnel = null;
                            lnqOutsourcing.QualityTime = null;

                            break;
                        default:
                            break;
                    }

                    dataContext.SubmitChanges();
                    return true;
                }
                else
                {
                    error = "数据不唯一或者为空";
                    return false;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }
    }
}
