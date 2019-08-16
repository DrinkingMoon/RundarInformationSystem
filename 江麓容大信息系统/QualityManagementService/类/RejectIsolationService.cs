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
using ServerModule;
using FlowControlService;

namespace Service_Quality_QC
{
    class RejectIsolationService : IRejectIsolationService
    {
        /// <summary>
        /// 流程服务组件
        /// </summary>
        IFlowServer _serviceFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();

        /// <summary>
        /// 单据编号分配服务
        /// </summary>
        IAssignBillNoServer _assignBill = ServerModule.ServerModuleFactory.GetServerModule<IAssignBillNoServer>();

        /// <summary>
        /// 获得单条业务总单信息
        /// </summary>
        /// <param name="billNo">业务号</param>
        /// <returns>返回业务总单信息</returns>
        public Business_QualityManagement_Isolation GetSingleBillInfo(string billNo)
        {
            if (billNo == null)
            {
                return null;
            }

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Business_QualityManagement_Isolation
                          where a.BillNo == billNo
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
        /// 保存业务信息
        /// </summary>
        /// <param name="billInfo">业务总单信息</param>
        /// <param name="detailInfo">业务明细信息</param>
        public void SaveInfo(Business_QualityManagement_Isolation billInfo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();

            try
            {

                var varData = from a in ctx.Business_QualityManagement_Isolation
                              where a.BillNo == billInfo.BillNo
                              select a;

                if (varData.Count() == 1)
                {
                    Flow_FlowInfo info =
                        _serviceFlow.GetNowFlowInfo(_serviceFlow.GetBusinessTypeID(CE_BillTypeEnum.不合格品隔离处置单, null),
                        billInfo.BillNo);

                    Business_QualityManagement_Isolation lnqBill = varData.Single();

                    switch (info.FlowID)
                    {
                        case 57:

                            if (lnqBill.GoodsID != billInfo.GoodsID 
                                ||lnqBill.BatchNo != billInfo.BatchNo
                                ||lnqBill.StorageID != billInfo.StorageID
                                ||lnqBill.Provider != billInfo.Provider)
                            {
                                if (IsRepeatIsolation(ctx, billInfo.GoodsID, billInfo.BatchNo, billInfo.StorageID))
                                {
                                    throw new Exception("批次号【" + billInfo.BatchNo + "】已隔离，不能重复隔离");
                                }
                            }

                            lnqBill.BatchNo = billInfo.BatchNo;
                            lnqBill.BillNo = billInfo.BillNo;
                            lnqBill.GoodsCount = billInfo.GoodsCount;
                            lnqBill.GoodsID = billInfo.GoodsID;
                            lnqBill.Provider = billInfo.Provider;
                            lnqBill.StorageID = billInfo.StorageID;
                            lnqBill.IsolationReason = billInfo.IsolationReason;
                            break;
                        case 58:

                            lnqBill.ProcessMethodRequire = billInfo.ProcessMethodRequire;
                            lnqBill.ReturnProcess = billInfo.ReturnProcess;
                            lnqBill.WorkHours = billInfo.WorkHours;
                            lnqBill.PH_DisqualifiendCount = billInfo.PH_DisqualifiendCount;
                            lnqBill.PH_QualifiedCount = billInfo.PH_QualifiedCount;
                            break;
                        case 59:
                            lnqBill.QC_DisqualifiedCount = billInfo.QC_DisqualifiedCount;
                            lnqBill.QC_QualifiedCount = billInfo.QC_QualifiedCount;
                            lnqBill.QC_ConcessionCount = billInfo.QC_ConcessionCount;
                            lnqBill.QC_ScraptCount = billInfo.QC_ScraptCount;
                            lnqBill.ReportFile = billInfo.ReportFile;
                            break;
                        default:
                            break;
                    }
                }
                else if (varData.Count() == 0)
                {
                    if (IsRepeatIsolation(ctx, billInfo.GoodsID, billInfo.BatchNo, billInfo.StorageID))
                    {
                        throw new Exception("批次号【" + billInfo.BatchNo + "】已隔离，不能重复隔离");
                    }

                    ctx.Business_QualityManagement_Isolation.InsertOnSubmit(billInfo);
                }
                else
                {
                    throw new Exception("单据数据不唯一");
                }

                ctx.SubmitChanges();
                ctx.Transaction.Commit();
            }
            catch (Exception ex)
            {
                ctx.Transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 结束业务
        /// </summary>
        /// <param name="billNo">业务编号</param>
        public void FinishBill(string billNo)
        {
            IFlowServer serviceFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();

            Flow_FlowInfo flowInfo =
                _serviceFlow.GetNowFlowInfo(_serviceFlow.GetBusinessTypeID(CE_BillTypeEnum.不合格品隔离处置单, null), billNo);

            if (flowInfo == null)
            {
                throw new Exception("单据状态为空，请重新确认");
            }

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();

            try
            {
                Business_QualityManagement_Isolation billInfo = GetSingleBillInfo(billNo);

                if (billInfo == null || billInfo.BillNo.Length == 0)
                {
                    throw new Exception("此单据不存在");
                }

                switch (flowInfo.FlowID)
                {
                    case 57:
                        UpdateStockGoodsStatus(ctx, billInfo, (int)CE_StockGoodsStatus.隔离);
                        break;
                    case 59:
                        if (billInfo.QC_ScraptCount != null && billInfo.QC_ScraptCount > 0)
                        {
                            InsertIntoMaterialRequisition(ctx, billInfo);
                        }

                        if ((decimal)billInfo.QC_QualifiedCount == billInfo.GoodsCount)
                        {
                            UpdateStockGoodsStatus(ctx, billInfo, (int)CE_StockGoodsStatus.正常);
                        }

                        break;
                    case 60:

                        QueryCondition_Store stockQuery = new QueryCondition_Store();

                        stockQuery.BatchNo = billInfo.BatchNo;
                        stockQuery.GoodsID = billInfo.GoodsID;
                        stockQuery.StorageID = billInfo.StorageID;
                        stockQuery.Provider = billInfo.Provider;

                        S_Stock stockInfo = UniversalFunction.GetStockInfo(ctx, stockQuery);

                        if (stockInfo == null)
                        {
                            throw new Exception("获取库存信息失败");
                        }

                        if (billInfo.QC_ConcessionCount == null || billInfo.QC_QualifiedCount == null)
                        {
                            if (stockInfo.ExistCount != billInfo.PH_QualifiedCount)
                            {
                                throw new Exception("此物品的当前库存为【" + stockInfo.ExistCount + "】不等于【处理人】的【合格数】," +
                                    "请根据处理方式要求采购员开【领料单】或者【采购退货单】减库存");
                            }
                        }
                        else
                        {
                            if (stockInfo.ExistCount != billInfo.QC_ConcessionCount + billInfo.QC_QualifiedCount)
                            {
                                throw new Exception("此物品的当前库存为【" + stockInfo.ExistCount + "】不等于【QC】的【让步数】+【合格数】," +
                                    "请根据处理方式要求采购员开【领料单】或者【采购退货单】减库存");
                            }
                        }

                        UpdateStockGoodsStatus(ctx, billInfo, (int)CE_StockGoodsStatus.正常);
                        break;
                    default:
                        break;
                }

                ctx.SubmitChanges();
                ctx.Transaction.Commit();
            }
            catch (Exception ex)
            {
                ctx.Transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 有检测废的物品直接生成领料单
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billInfo">单据信息</param>
        void InsertIntoMaterialRequisition(DepotManagementDataContext ctx, Business_QualityManagement_Isolation billInfo)
        {
            string error = null;
            string billNo = null;

            ServerModule.IMaterialRequisitionServer serverMaterialBill =
                ServerModule.ServerModuleFactory.GetServerModule<ServerModule.IMaterialRequisitionServer>();

            try
            {

                billNo = _assignBill.AssignNewNo(serverMaterialBill, CE_BillTypeEnum.领料单.ToString());

                var varData = from a in ctx.S_MaterialRequisition
                              where a.Bill_ID == billNo
                              select a;

                S_MaterialRequisition lnqMaterial = null;

                List<string> listPersonnel = UniversalFunction.GetStorageOrStationPrincipal(billInfo.StorageID);

                if (listPersonnel.Contains("0008"))
                {
                    listPersonnel.Remove("0008");
                }

                if (listPersonnel.Contains("0417"))
                {
                    listPersonnel.Remove("0417");
                }

                if (varData.Count() != 0)
                {
                    error = string.Format("自动生成的报废物品领料单单号 {0} 已被占用，请尝试重新进行此操作"+
                        "，再三出现无法生成可用的单号时与管理员联系", billNo);
                    throw new Exception(error);
                }
                else
                {
                    lnqMaterial = new S_MaterialRequisition();

                    lnqMaterial.Bill_ID = billNo;
                    lnqMaterial.Bill_Time = ServerModule.ServerTime.Time;
                    lnqMaterial.AssociatedBillNo = billInfo.BillNo;
                    lnqMaterial.AssociatedBillType = "不合格品隔离处置单";
                    lnqMaterial.BillStatus = "已出库";
                    lnqMaterial.Department = "ZK03";
                    lnqMaterial.DepartmentDirector = "";
                    lnqMaterial.DepotManager = UniversalFunction.GetPersonnelInfo(listPersonnel[0]).姓名;
                    lnqMaterial.FetchCount = 0;
                    lnqMaterial.FetchType = "零星领料";
                    lnqMaterial.FillInPersonnel = UniversalFunction.GetPersonnelInfo(BasicInfo.LoginID).姓名;
                    lnqMaterial.FillInPersonnelCode = BasicInfo.LoginID;
                    lnqMaterial.ProductType = "";
                    lnqMaterial.PurposeCode = UniversalFunction.GetPurpose(CE_PickingPurposeProperty.破坏性检测).Code;
                    lnqMaterial.Remark = "因入库零件进行了破坏性检测，由系统自动生成的破坏件领料单，对应单据号：" + billInfo.BillNo;
                    lnqMaterial.StorageID = billInfo.StorageID;
                    lnqMaterial.OutDepotDate = ServerTime.Time;

                    if (!serverMaterialBill.AutoCreateBill(ctx, lnqMaterial, out error))
                    {
                        throw new Exception(error);
                    }

                    S_MaterialRequisitionGoods lnqMaterialGoods = new S_MaterialRequisitionGoods();

                    lnqMaterialGoods.Bill_ID = billNo;
                    lnqMaterialGoods.BasicCount = 0;
                    lnqMaterialGoods.BatchNo = billInfo.BatchNo;
                    lnqMaterialGoods.GoodsID = billInfo.GoodsID;
                    lnqMaterialGoods.ProviderCode = billInfo.Provider;
                    lnqMaterialGoods.RealCount = Convert.ToDecimal(billInfo.QC_ScraptCount);
                    lnqMaterialGoods.Remark = "";
                    lnqMaterialGoods.RequestCount = Convert.ToDecimal(billInfo.QC_ScraptCount);
                    lnqMaterialGoods.ShowPosition = 1;

                    ServerModule.IMaterialRequisitionGoodsServer serverMaterialGoods =
                        ServerModule.ServerModuleFactory.GetServerModule<ServerModule.IMaterialRequisitionGoodsServer>();

                    if (!serverMaterialGoods.AutoCreateGoods(ctx, lnqMaterialGoods, out error))
                    {
                        throw new Exception(error);
                    }

                    ctx.SubmitChanges();

                    if (!serverMaterialBill.FinishBill(ctx, lnqMaterial.Bill_ID, "", out error))
                    {
                        throw new Exception(error);
                    }

                    ctx.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                serverMaterialBill.DeleteBill(billNo, out error);
                throw new Exception(ex.Message);
            }
        }

        public bool IsRepeatIsolation(int goodsID, string batchNo, string storageID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            bool result = IsRepeatIsolation(ctx, goodsID, batchNo, storageID);
            ctx.SubmitChanges();

            return result;
        }

        bool IsRepeatIsolation(DepotManagementDataContext ctx, int goodsID, string batchNo, string storageID)
        {
            var varData = from a in ctx.Business_QualityManagement_Isolation
                          join b in ctx.View_Flow_FlowInfo on a.BillNo equals b.业务编号
                          where b.业务当前流程 != "完成" && a.GoodsID == goodsID && a.BatchNo == batchNo
                          && a.StorageID == storageID
                          select a;

            if (varData.Count() == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 修改物品库存状态
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billInfo">单据信息</param>
        /// <param name="goodsStatus">物品状态</param>
        public void UpdateStockGoodsStatus(DepotManagementDataContext ctx, Business_QualityManagement_Isolation billInfo, int goodsStatus)
        {
            var varData = from a in ctx.S_Stock
                          where a.BatchNo == billInfo.BatchNo
                          && a.GoodsID == billInfo.GoodsID
                          && a.Provider == billInfo.Provider
                          && a.StorageID == billInfo.StorageID
                          select a;

            if (varData.Count() == 1)
            {
                S_Stock tempStok = varData.Single();
                tempStok.GoodsStatus = goodsStatus;
            }
            else
            {
                throw new Exception("库存信息有误，操作失败");
            }
        }

        /// <summary>
        /// 删除业务
        /// </summary>
        /// <param name="billNo">业务号</param>
        public void DeleteInfo(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();

            IFlowServer serverFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();
            BillNumberControl billNoControl = new BillNumberControl(CE_BillTypeEnum.不合格品隔离处置单.ToString(), this);

            try
            {
                var varData = from a in ctx.Business_QualityManagement_Isolation
                              where a.BillNo == billNo
                              select a;

                UpdateStockGoodsStatus(ctx, varData.Single(), (int)CE_StockGoodsStatus.正常);

                ctx.Business_QualityManagement_Isolation.DeleteAllOnSubmit(varData);
                serverFlow.FlowDelete(ctx, billNo);

                ctx.SubmitChanges();
                ctx.Transaction.Commit();
                billNoControl.CancelBill(billNo);

            }
            catch (Exception ex)
            {
                ctx.Transaction.Rollback();
                throw new Exception(ex.Message);
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
            var varData = from a in ctx.Business_QualityManagement_Isolation
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
        /// 检查某业务是否存在
        /// </summary>
        /// <param name="billNo">业务号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Business_QualityManagement_Isolation
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
        /// 更新文件路径
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="guid">文件编号集字符串</param>
        public void UpdateFilePath(string billNo, string guid)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Business_QualityManagement_Isolation
                          where a.BillNo == billNo
                          select a;

            if (varData.Count() == 1)
            {
                varData.Single().ReportFile = guid;
            }

            ctx.SubmitChanges();
        }

        /// <summary>
        /// 添加补充信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="msg">补充信息</param>
        public void AddSupplementMessage(string billNo, string msg)
        {
            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                Business_QualityManagement_Isolation_SupplementMessage info = 
                    new Business_QualityManagement_Isolation_SupplementMessage();

                info.F_Id = Guid.NewGuid().ToString();
                info.CreateUser = BasicInfo.LoginID;
                info.CreateTime = ServerTime.Time;
                info.BillNo = billNo;
                info.SupplementMessage = msg;

                ctx.Business_QualityManagement_Isolation_SupplementMessage.InsertOnSubmit(info);
                ctx.SubmitChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获得单据补充信息列表
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回Table</returns>
        public DataTable GetSupplementeMessageInfo(string billNo)
        {
            try
            {
                string strSql = " select SupplementMessage as 补充信息, dbo.fun_get_personnelName(CreateUser) as 补充人, CreateTime as 补充日期 " +
                                " from Business_QualityManagement_Isolation_SupplementMessage where BillNo = '" + billNo + "' order by CreateTime";

                return GlobalObject.DatabaseServer.QueryInfo(strSql);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
