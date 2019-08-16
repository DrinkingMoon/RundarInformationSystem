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

namespace Service_Project_Project
{
    class SampleApplication :  BasicServer, ISampleApplication
    {
        decimal _InDepotCount = 0;

        /// <summary>
        /// 单据服务组件
        /// </summary>
        IAssignBillNoServer _assignBillNoService = ServerModule.ServerModuleFactory.GetServerModule<IAssignBillNoServer>();

        /// <summary>
        /// 流程服务组件
        /// </summary>
        IFlowServer _serviceFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();

        /// <summary>
        /// 合同模块服务组件
        /// </summary>
        IBargainInfoServer _serviceBargainInfo = ServerModule.ServerModuleFactory.GetServerModule<IBargainInfoServer>();

        /// <summary>
        /// 更新文件路径
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="guid">文件编号集字符串</param>
        public void UpdateFilePath(string billNo, string guid)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Business_Sample_ConfirmTheApplication
                          where a.BillNo == billNo
                          select a;

            if (varData.Count() == 1)
            {
                varData.Single().Inspect_ReportFile = guid;
            }

            ctx.SubmitChanges();
        }

        /// <summary>
        /// 获得单条业务总单信息
        /// </summary>
        /// <param name="billNo">业务号</param>
        /// <returns>返回业务总单信息</returns>
        public Business_Sample_ConfirmTheApplication GetSingleBillInfo(string billNo)
        {
            if (billNo == null)
            {
                return null;
            }

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Business_Sample_ConfirmTheApplication
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
        /// 删除业务
        /// </summary>
        /// <param name="billNo">业务号</param>
        public void DeleteInfo(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();

            IFlowServer serverFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();
            BillNumberControl billNoControl = new BillNumberControl(CE_BillTypeEnum.样品确认申请单.ToString(), this);

            try
            {
                var varData4 = from a in ctx.S_Stock
                               where a.BatchNo == billNo
                               && a.ExistCount > 0
                               select a;

                if (varData4.Count() > 0)
                {
                    throw new Exception("存在库存，无法删除");
                }

                var varData = from a in ctx.Business_Sample_ConfirmTheApplication
                              where a.BillNo == billNo
                              select a;

                if (varData.Count() == 1)
                {
                    Business_Sample_ConfirmTheApplication tempInfo = varData.Single();

                    var varData1 = from a in ctx.S_MusterStock
                                   where a.BatchNo == tempInfo.Purchase_BatchNo
                                   && a.GoodsID == tempInfo.Purchase_GoodsID
                                   select a;

                    ctx.S_MusterStock.DeleteAllOnSubmit(varData1);

                    var varData2 = from a in ctx.S_MusterUseBill
                                   join b in ctx.S_MusterUseList
                                   on a.DJH equals b.DJH
                                   where b.GoodsID == tempInfo.Purchase_GoodsID
                                   && b.BatchNo == tempInfo.Purchase_BatchNo
                                   select a;

                    ctx.S_MusterUseBill.DeleteAllOnSubmit(varData2);

                    if (tempInfo.Inspect_ReportFile != null && tempInfo.Inspect_ReportFile.Trim().Length > 0)
                    {
                        string[] pathArray = tempInfo.Inspect_ReportFile.Split(',');

                        for (int i = 0; i < pathArray.Length; i++)
                        {
                            UniversalControlLibrary.FileOperationService.File_Delete(new Guid(pathArray[i]),
                                GlobalObject.GeneralFunction.StringConvertToEnum<CE_CommunicationMode>(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.文件传输方式]));
                        }
                    }
                }

                ctx.Business_Sample_ConfirmTheApplication.DeleteAllOnSubmit(varData);
                ctx.SubmitChanges();
                serverFlow.FlowDelete(ctx, billNo);

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
        /// 保存业务信息
        /// </summary>
        /// <param name="billInfo">业务总单信息</param>
        public void SaveInfo(Business_Sample_ConfirmTheApplication billInfo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();

            IOrderFormInfoServer serivceOrderForm = ServerModule.ServerModuleFactory.GetServerModule<IOrderFormInfoServer>();

            try
            {
                var varData = from a in ctx.Business_Sample_ConfirmTheApplication
                              where a.BillNo == billInfo.BillNo
                              select a;

                if (varData.Count() == 0)
                {
                    if (billInfo.Purchase_OrderFormNo.Trim().Length > 0
                        && serivceOrderForm.GetOrderFormInfo(ctx, billInfo.Purchase_OrderFormNo.Trim()) == null)
                    {
                        throw new Exception("订单号：【"+ billInfo.Purchase_OrderFormNo +"】 不存在");
                    }

                    ctx.Business_Sample_ConfirmTheApplication.InsertOnSubmit(billInfo);
                }
                else if (varData.Count() == 1)
                {
                    Business_Sample_ConfirmTheApplication lnqBillInfo = varData.Single();

                    Flow_FlowInfo info =
                        _serviceFlow.GetNowFlowInfo(_serviceFlow.GetBusinessTypeID(CE_BillTypeEnum.样品确认申请单, null), 
                        lnqBillInfo.BillNo);

                    switch (info.FlowID)
                    {
                        case 42:

                            if (billInfo.Purchase_OrderFormNo.Trim().Length > 0
                                && serivceOrderForm.GetOrderFormInfo(ctx, billInfo.Purchase_OrderFormNo.Trim()) == null)
                            {
                                throw new Exception("订单号：【" + billInfo.Purchase_OrderFormNo + "】 不存在");
                            }

                            lnqBillInfo.Purchase_BatchNo = billInfo.Purchase_BatchNo;
                            lnqBillInfo.Purchase_BillType = billInfo.Purchase_BillType;
                            lnqBillInfo.Purchase_Change_BillNo = billInfo.Purchase_Change_BillNo;
                            lnqBillInfo.Purchase_Change_Reason = billInfo.Purchase_Change_Reason;
                            lnqBillInfo.Purchase_Change_Type = billInfo.Purchase_Change_Type;
                            lnqBillInfo.Purchase_GoodsCount_Send = billInfo.Purchase_GoodsCount_Send;
                            lnqBillInfo.Purchase_GoodsID = billInfo.Purchase_GoodsID;
                            lnqBillInfo.Purchase_IsPay = billInfo.Purchase_IsPay;
                            lnqBillInfo.Purchase_OrderFormNo = billInfo.Purchase_OrderFormNo;
                            lnqBillInfo.Purchase_Provider = billInfo.Purchase_Provider;
                            lnqBillInfo.Purchase_ProviderBatchNo = billInfo.Purchase_ProviderBatchNo;
                            lnqBillInfo.Purchase_SampleType = billInfo.Purchase_SampleType;
                            lnqBillInfo.Purchase_SendSampleReason = billInfo.Purchase_SendSampleReason;
                            lnqBillInfo.Purchase_SendSampleTime = billInfo.Purchase_SendSampleTime;
                            lnqBillInfo.Purchase_StorageID = billInfo.Purchase_StorageID;
                            lnqBillInfo.Purchase_Version = billInfo.Purchase_Version;
                            break;
                        case 43:
                            lnqBillInfo.Finance_RawMaterialCost = billInfo.Finance_RawMaterialCost;
                            break;
                        case 44:
                            lnqBillInfo.Store_GoodsCount_AOG = billInfo.Store_GoodsCount_AOG;
                            lnqBillInfo.Store_IsProperPacking = billInfo.Store_IsProperPacking;
                            lnqBillInfo.Store_Packing_IrrCause = billInfo.Store_Packing_IrrCause;
                            lnqBillInfo.Store_Put_Area = billInfo.Store_Put_Area;
                            lnqBillInfo.Store_Put_AreaNo = billInfo.Store_Put_AreaNo;
                            lnqBillInfo.Store_Put_LayerNo = billInfo.Store_Put_LayerNo;
                            break;
                        case 63:
                            lnqBillInfo.Store_Put_Area = billInfo.Store_Put_Area;
                            lnqBillInfo.Store_Put_AreaNo = billInfo.Store_Put_AreaNo;
                            lnqBillInfo.Store_Put_LayerNo = billInfo.Store_Put_LayerNo;
                            break;
                        case 45:
                            lnqBillInfo.Inspect_GoodsCount_Scarp = billInfo.Inspect_GoodsCount_Scarp;
                            lnqBillInfo.Inspect_IrrItemExplain = billInfo.Inspect_IrrItemExplain;
                            lnqBillInfo.Inspect_ReportFile = billInfo.Inspect_ReportFile;
                            lnqBillInfo.Inspect_ReportNo = billInfo.Inspect_ReportNo;
                            lnqBillInfo.Inspect_Result = billInfo.Inspect_Result;
                            lnqBillInfo.Inspect_IsProperPacking = billInfo.Inspect_IsProperPacking;
                            lnqBillInfo.Inspect_Packing_IrrCause = billInfo.Inspect_Packing_IrrCause;
                            lnqBillInfo.Inspect_SupplierReport_IsExist = billInfo.Inspect_SupplierReport_IsExist;
                            break;
                        case 46:
                        case 62:
                            lnqBillInfo.Review_InspectResult = billInfo.Review_InspectResult;
                            lnqBillInfo.Review_InspectResult_ReWork_DisqualificationCount = billInfo.Review_InspectResult_ReWork_DisqualificationCount;
                            lnqBillInfo.Review_InspectResult_ReWork_QualificationCount = billInfo.Review_InspectResult_ReWork_QualificationCount;
                            lnqBillInfo.Review_IrrItem_Concession = billInfo.Review_IrrItem_Concession;
                            lnqBillInfo.Review_IrrItem_Rectification = billInfo.Review_IrrItem_Rectification;
                            lnqBillInfo.Review_ItemReview = billInfo.Review_ItemReview;
                            lnqBillInfo.Review_RectificationItem_Explain = billInfo.Review_RectificationItem_Explain;
                            lnqBillInfo.Review_RectificationRequest_IsExist = billInfo.Review_RectificationRequest_IsExist;
                            lnqBillInfo.Review_IsPassTestResult = billInfo.Review_IsPassTestResult;
                            break;
                        case 47:
                            lnqBillInfo.SQE_ProvidorFeedbackBill_IsExist = billInfo.SQE_ProvidorFeedbackBill_IsExist;
                            lnqBillInfo.SQE_ProvidorFeedbackBillNo = billInfo.SQE_ProvidorFeedbackBillNo;
                            lnqBillInfo.SQE_SampleDisposeType_DisqualificationCount = billInfo.SQE_SampleDisposeType_DisqualificationCount;
                            lnqBillInfo.SQE_SampleDisposeType_QualificationCount = billInfo.SQE_SampleDisposeType_QualificationCount;
                            break;
                        case 48:
                            lnqBillInfo.TestResult_Experiment_ReportNo = billInfo.TestResult_Experiment_ReportNo;
                            lnqBillInfo.TestResult_Experiment_Result = billInfo.TestResult_Experiment_Result;
                            lnqBillInfo.TestResult_ExperimentCVTNo = billInfo.TestResult_ExperimentCVTNo;
                            lnqBillInfo.TestResult_ResultAffrim = billInfo.TestResult_ResultAffrim;
                            lnqBillInfo.TestResult_ResultAffrim_DiposeSuggestion_Bined = billInfo.TestResult_ResultAffrim_DiposeSuggestion_Bined;
                            lnqBillInfo.TestResult_ResultAffrim_DiposeSuggestion_Surplus = billInfo.TestResult_ResultAffrim_DiposeSuggestion_Surplus;
                            lnqBillInfo.TestResult_TrialAssembly_Explain = billInfo.TestResult_TrialAssembly_Explain;
                            lnqBillInfo.TestResult_TrialAssembly_Explain_PartCount = billInfo.TestResult_TrialAssembly_Explain_PartCount;
                            lnqBillInfo.TestResult_TrialAssembly_Result = billInfo.TestResult_TrialAssembly_Result;
                            lnqBillInfo.TestResult_TrialAssembly_ResultExplain = billInfo.TestResult_TrialAssembly_ResultExplain;
                            break;
                        case 49:
                            lnqBillInfo.ChargeResult_ResultAffrim = billInfo.ChargeResult_ResultAffrim;
                            lnqBillInfo.ChargeResult_ResultAffrim_DiposeSuggestion_Bined = billInfo.ChargeResult_ResultAffrim_DiposeSuggestion_Bined;
                            lnqBillInfo.ChargeResult_ResultAffrim_DiposeSuggestion_Surplus = billInfo.ChargeResult_ResultAffrim_DiposeSuggestion_Surplus;
                            lnqBillInfo.ChargeResult_SampleVerify = billInfo.ChargeResult_SampleVerify;
                            break;
                        case 50:
                            lnqBillInfo.QC_PPAPDispose = billInfo.QC_PPAPDispose;
                            lnqBillInfo.QC_Suggestion = billInfo.QC_Suggestion;
                            break;
                        case 69:
                            lnqBillInfo.Store_GoodsCount_InDepot = billInfo.Store_GoodsCount_InDepot;
                            break;
                        default:
                            break;
                    }

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
        /// 检查某业务是否存在
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">业务号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.Business_Sample_ConfirmTheApplication
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

            var varData = from a in ctx.Business_Sample_ConfirmTheApplication
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
        /// 是否已经入库
        /// </summary>
        /// <param name="sampleInfo">样品信息</param>
        /// <returns>True 入库， False 出库</returns>
        public bool IsInStore(Business_Sample_ConfirmTheApplication sampleInfo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            return IsInStore(ctx, sampleInfo);
 
        }

        /// <summary>
        /// 是否已经入库
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="sampleInfo">样品信息</param>
        /// <returns>True 入库， False 出库</returns>
        bool IsInStore(DepotManagementDataContext ctx, Business_Sample_ConfirmTheApplication sampleInfo)
        {
            var varData = from a in ctx.S_InDepotDetailBill
                          where a.GoodsID == sampleInfo.Purchase_GoodsID
                          && a.BatchNo == sampleInfo.Purchase_BatchNo
                          && (a.InDepotBillID.Contains(CE_BatchNoPrefix.BJD.ToString())
                          || a.InDepotBillID.Contains(CE_BatchNoPrefix.WJD.ToString())
                          || a.InDepotBillID.Contains(CE_BatchNoPrefix.ZRD.ToString())
                          || a.InDepotBillID.Contains(CE_BatchNoPrefix.PR.ToString())
                          || a.InDepotBillID.Contains(CE_BatchNoPrefix.HJD.ToString()))
                          select a;

            var varStock = from a in ctx.S_Stock
                           where a.GoodsID == sampleInfo.Purchase_GoodsID
                           && a.BatchNo == sampleInfo.Purchase_BatchNo
                           select a;

            if (varData.Count() > 0 && varStock.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 是否需要隔离
        /// </summary>
        /// <param name="sampleInfo">单据信息</param>
        /// <returns>需要返回True,不需要 返回 False</returns>
        public bool IsNeedIsolation(Business_Sample_ConfirmTheApplication sampleInfo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            FlowControlService.IFlowServer serviceFlow = FlowControlService.ServerModuleFactory.GetServerModule<FlowControlService.IFlowServer>();

            var varData1 = from a in ctx.Business_Sample_ConfirmTheApplication
                           where a.BillNo == sampleInfo.BillNo
                           select a;

            if (varData1.Count() != 1)
            {
                throw new Exception("数据不存在");
            }

            Business_Sample_ConfirmTheApplication tempBillInfo = varData1.Single();

            if (tempBillInfo.Review_InspectResult.Trim().Length == 0 || sampleInfo.Review_InspectResult.Trim().Length == 0)
            {
                return false;
            }

            if (tempBillInfo.Review_InspectResult != sampleInfo.Review_InspectResult)
            {
                List<string> lstResult2 = new List<string>();

                lstResult2.Add("退货");
                lstResult2.Add("报废");

                if (tempBillInfo.Review_InspectResult == "让步接收"
                    && lstResult2.Contains(sampleInfo.Review_InspectResult))
                {
                    return true;
                }

                lstResult2 = new List<string>();

                lstResult2.Add("返工/返修");
                lstResult2.Add("退货");
                lstResult2.Add("报废");

                if (tempBillInfo.Review_InspectResult == "合格"
                    && lstResult2.Contains(sampleInfo.Review_InspectResult))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 获得操作节点信息
        /// </summary>
        /// <param name="sampleInfo">单据信息</param>
        /// <returns>返回信息字典</returns>
        Dictionary<string, Flow_FlowData> GetNodeInfo(Business_Sample_ConfirmTheApplication sampleInfo)
        {
            Dictionary<string, Flow_FlowData> dicTemp = new Dictionary<string, Flow_FlowData>();
            List<Flow_FlowData> listTemp = _serviceFlow.GetBusinessOperationInfo(sampleInfo.BillNo);
            Flow_FlowData tempInfo = new Flow_FlowData();

            if (listTemp.Where(k => k.FlowID == 42).Count() > 0)
            {
                listTemp.Where(k => k.FlowID == 42).First().OperationPersonnel =
                    UniversalFunction.GetPersonnelName(listTemp.Where(k => k.FlowID == 42).First().OperationPersonnel);
                dicTemp.Add("SQ", listTemp.Where(k => k.FlowID == 42).First());
            }

            if (listTemp.Where(k => k.FlowID == 43).Count() > 0)
            {
                listTemp.Where(k => k.FlowID == 43).First().OperationPersonnel =
                    UniversalFunction.GetPersonnelName(listTemp.Where(k => k.FlowID == 43).First().OperationPersonnel);
                dicTemp.Add("CW", listTemp.Where(k => k.FlowID == 43).First());
            }

            if (listTemp.Where(k => k.FlowID == 44).Count() > 0)
            {
                listTemp.Where(k => k.FlowID == 44).First().OperationPersonnel =
                    UniversalFunction.GetPersonnelName(listTemp.Where(k => k.FlowID == 44).First().OperationPersonnel);
                dicTemp.Add("DH", listTemp.Where(k => k.FlowID == 44).First());
            }

            if (listTemp.Where(k => k.FlowID == 45).Count() > 0)
            {
                listTemp.Where(k => k.FlowID == 45).First().OperationPersonnel =
                    UniversalFunction.GetPersonnelName(listTemp.Where(k => k.FlowID == 45).First().OperationPersonnel);
                dicTemp.Add("JY", listTemp.Where(k => k.FlowID == 45).First());
            }

            Flow_FlowData flowDataInfo_RK = new Flow_FlowData();
            flowDataInfo_RK.OperationPersonnel = UniversalFunction.GetPersonnelName(BasicInfo.LoginID);
            flowDataInfo_RK.OperationTime = ServerTime.Time;
            dicTemp.Add("RK", flowDataInfo_RK);

            return dicTemp;
        }

        /// <summary>
        /// 操作流程以外的业务
        /// </summary>
        /// <param name="billNo">单据号</param>
        public void OperatarUnFlowBusiness(string billNo)
        {
            string error = "";

            Flow_FlowInfo flowInfo = _serviceFlow.GetNowFlowInfo(_serviceFlow.GetBusinessTypeID(CE_BillTypeEnum.样品确认申请单, null), billNo);

            if (flowInfo == null)
            {
                throw new Exception("单据流程信息为空，请重新确认");
            }

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            dataContxt.Connection.Open();
            dataContxt.Transaction = dataContxt.Connection.BeginTransaction();

            try
            {
                Business_Sample_ConfirmTheApplication sampleInfo = GetSingleBillInfo(billNo);

                if (sampleInfo == null)
                {
                    throw new Exception("获取单据信息失败");
                }

                Dictionary<string, Flow_FlowData> dicTemp = GetNodeInfo(sampleInfo);

                switch (flowInfo.FlowID)
                {

                    case 44://确认到货
                        if (!InsertNewBarCode(sampleInfo, out error))
                        {
                            throw new Exception(error);
                        }

                        break;
                    case 69:

                        List<string> lstResult = new List<string>();

                        lstResult.Add("合格");
                        lstResult.Add("让步接收");
                        lstResult.Add("返工/返修");

                        if (lstResult.Contains(sampleInfo.Review_InspectResult))
                        {
                            OperationStoreBusiness(dataContxt, sampleInfo);
                        }

                        break;
                    default:
                        break;
                }

                dataContxt.SubmitChanges();
                dataContxt.Transaction.Commit();
            }
            catch (Exception ex)
            {
                dataContxt.Transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }

        #region 相关业务处理

        /// <summary>
        /// 修改存放位置
        /// </summary>
        /// <param name="dataContxt">数据上下文</param>
        /// <param name="sampleInfo">单据信息</param>
        void UpdateStoreWhere(DepotManagementDataContext dataContxt, Business_Sample_ConfirmTheApplication sampleInfo)
        {
            var varData = from a in dataContxt.S_Stock
                          where a.BatchNo == sampleInfo.Purchase_BatchNo
                          && a.GoodsID == sampleInfo.Purchase_GoodsID
                          && a.StorageID == sampleInfo.Purchase_StorageID
                          && a.Provider == sampleInfo.Purchase_Provider
                          select a;

            if (varData.Count() == 1)
            {
                S_Stock temp = varData.Single();

                temp.ShelfArea = sampleInfo.Store_Put_Area;
                temp.ColumnNumber = sampleInfo.Store_Put_AreaNo;
                temp.LayerNumber = sampleInfo.Store_Put_LayerNo;

                dataContxt.SubmitChanges();
            }
            else
            {
                throw new Exception("此批次的库存信息有误");
            }
        }

        /// <summary>
        /// 操作直接退库退货业务流程
        /// </summary>
        /// <param name="dataContxt">数据上下文</param>
        /// <param name="sampleInfo">单据信息</param>
        /// <param name="dicFlowDataInfo">节点字典</param>
        void OperationReturnBusiness(DepotManagementDataContext dataContxt, Business_Sample_ConfirmTheApplication sampleInfo, 
            Dictionary<string, Flow_FlowData> dicFlowDataInfo)
        {
            if (IsInStore(dataContxt, sampleInfo))
            {
                InsertReturnBill(dataContxt, sampleInfo);
                InsertRejectBill(dataContxt, sampleInfo, dicFlowDataInfo);

                dataContxt.SubmitChanges();
            }
        }

        /// <summary>
        /// 创建调拨单
        /// </summary>
        /// <param name="dataContxt">数据上下文</param>
        /// <param name="sampleInfo">样品单信息</param>
        /// <param name="stockInfo">库存信息</param>
        void CreateCannibalizeBill(DepotManagementDataContext dataContxt, Business_Sample_ConfirmTheApplication sampleInfo, S_Stock stockInfo)
        {
            try
            {
                ICannibalize serverCannibalize = ServerModuleFactory.GetServerModule<ICannibalize>();
                BillNumberControl billNoControl = new BillNumberControl(CE_BillTypeEnum.库房调拨单, serverCannibalize);
                IBillMessagePromulgatorServer billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();
                billMessageServer.BillType = CE_BillTypeEnum.库房调拨单.ToString();

                S_CannibalizeBill lnqBill = new S_CannibalizeBill();

                int intDJID = 0;

                lnqBill.DJH = billNoControl.GetNewBillNo();
                lnqBill.LRRY = BasicInfo.LoginID;
                lnqBill.LRRQ = ServerTime.Time;
                lnqBill.DJZT = "已检测";
                lnqBill.Remark = "由于样品单【" + sampleInfo.BillNo + "】更改库房";
                lnqBill.Price = 0;
                lnqBill.OutStoreRoom = stockInfo.StorageID;
                lnqBill.InStoreRoom = sampleInfo.Purchase_StorageID;

                dataContxt.S_CannibalizeBill.InsertOnSubmit(lnqBill);
                dataContxt.SubmitChanges();

                var varData = from a in dataContxt.S_CannibalizeBill
                              where a.DJH == lnqBill.DJH
                              select a;

                if (varData.Count() == 1)
                {
                    intDJID = varData.Single().ID;
                }
                else
                {
                    throw new Exception("数据为空或者不唯一");
                }

                List<S_CannibalizeList> lstDetail = new List<S_CannibalizeList>();
                S_CannibalizeList detail = new S_CannibalizeList();

                detail.BatchNo = sampleInfo.Purchase_BatchNo;
                detail.Count = stockInfo.ExistCount;
                detail.GoodsID = stockInfo.GoodsID;
                detail.Price = stockInfo.Price;
                detail.Provider = stockInfo.Provider;
                detail.Remark = "";
                detail.RepairStatus = null;
                detail.UnitPrice = stockInfo.UnitPrice;

                lstDetail.Add(detail);
                serverCannibalize.SaveBillList(dataContxt, lstDetail, intDJID, lnqBill.DJH);
                billMessageServer.DestroyMessage(lnqBill.DJH);
                billMessageServer.SendNewFlowMessage(lnqBill.DJH,
                    string.Format("调出库房:【"
                    + UniversalFunction.GetStorageName(lnqBill.OutStoreRoom) + "】， 调入库房：【"
                    + UniversalFunction.GetStorageName(lnqBill.InStoreRoom) + "】，原因：由于样品单【"
                    + sampleInfo.BillNo + "】变更库房，请处理", lnqBill.DJH),
                    BillFlowMessage_ReceivedUserType.角色, CE_RoleEnum.会计.ToString());
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 处理自动生成业务
        /// </summary>
        /// <param name="dataContxt">数据上下文</param>
        /// <param name="sampleInfo">单据信息</param>
        void OperationStoreBusiness(DepotManagementDataContext dataContxt, Business_Sample_ConfirmTheApplication sampleInfo)
        {
            Dictionary<string, Flow_FlowData> dicTemp = GetNodeInfo(sampleInfo);
            string error = "";

            try
            {
                _InDepotCount = 0;

                var varData = from a in dataContxt.S_InDepotDetailBill
                              where a.GoodsID == sampleInfo.Purchase_GoodsID
                              && a.BatchNo == sampleInfo.Purchase_BatchNo
                              && a.StorageID == sampleInfo.Purchase_StorageID
                              //&& a.InDepotBillID.Contains("BJD")
                              select a;

                if (varData.Count() != 0)
                {
                    if (varData.First().Provider != sampleInfo.Purchase_Provider)
                    {
                        throw new Exception("【供应商】与第一次入库记录的【供应商】不一致");
                    }

                    if (varData.First().StorageID != sampleInfo.Purchase_StorageID)
                    {
                        throw new Exception("【入库库房】与第一次入库记录的【入库库房】不一致");
                    }

                    decimal inDepotCount = varData.Sum(k => k.InDepotCount).Value;

                    if (inDepotCount > sampleInfo.Store_GoodsCount_InDepot.Value)
                    {
                        throw new Exception("此物品已经入库 ，【入库数量】："+ inDepotCount.ToString() +"，请填写【采购退货单】");
                    }
                    else if (inDepotCount < sampleInfo.Store_GoodsCount_InDepot.Value)
                    {
                        _InDepotCount = sampleInfo.Store_GoodsCount_InDepot.Value - inDepotCount;
                    }
                    else
                    {
                        return;
                    }
                }

                if (sampleInfo.Purchase_Provider == CE_WorkShopCode.JJCJ.ToString())
                {
                    if (!IsMaterialRequisitionCount(sampleInfo.Purchase_ProviderBatchNo))
                    {
                        throw new Exception("此单据的供方批次未经过领料，无法生成自制件入库单");
                    }

                    if (sampleInfo.Purchase_StorageID != ((int)CE_StorageName.自制半成品库).ToString("D2") 
                        && sampleInfo.Purchase_StorageID != ((int)CE_StorageName.自制半成品样品库).ToString("D2"))
                    {
                        throw new Exception("此单据的入库库房必须为自制半成品库房");
                    }

                    //自制件报检入库
                    if (!InsertHomemadePartInDepotBill(dataContxt, sampleInfo, dicTemp, out error))
                    {
                        throw new Exception(error);
                    }

                    dataContxt.SubmitChanges();
                }
                else
                {
                    //插入入库
                    if (sampleInfo.Purchase_BillType.Contains("委外"))
                    {
                        //委外报检入库
                        if (!InsertCheckInDepotOutsourcingBill(dataContxt, sampleInfo, dicTemp, out error))
                        {
                            throw new Exception(error);
                        }

                        dataContxt.SubmitChanges();
                    }
                    else
                    {
                        //正常报检入库
                        if (!InsertCheckInDepotBill(dataContxt, sampleInfo, dicTemp, out error))
                        {
                            throw new Exception(error);
                        }

                        dataContxt.SubmitChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 生成采购退货单
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="sampleInfo">单据信息</param>
        /// <param name="dicFlowDataInfo">节点字典</param>
        void InsertRejectBill(DepotManagementDataContext context, Business_Sample_ConfirmTheApplication sampleInfo, 
            Dictionary<string, Flow_FlowData> dicFlowDataInfo)
        {
            string error = null;
            IMaterialRejectBill serverRejectBill = ServerModule.ServerModuleFactory.GetServerModule<IMaterialRejectBill>();
            IMaterialListRejectBill serverRejectListBill = ServerModule.ServerModuleFactory.GetServerModule<IMaterialListRejectBill>();

            var varData = from a in context.S_InDepotDetailBill
                          where a.GoodsID == sampleInfo.Purchase_GoodsID
                          && a.BatchNo == sampleInfo.Purchase_BatchNo
                          && (a.InDepotBillID.Contains(CE_BatchNoPrefix.BJD.ToString())
                          || a.InDepotBillID.Contains(CE_BatchNoPrefix.WJD.ToString())
                          || a.InDepotBillID.Contains(CE_BatchNoPrefix.ZRD.ToString())
                          || a.InDepotBillID.Contains(CE_BatchNoPrefix.PR.ToString())
                          || a.InDepotBillID.Contains(CE_BatchNoPrefix.HJD.ToString()))
                          select a;

            if (varData.Count() == 1)
            {
                S_InDepotDetailBill indepotInfo = varData.Single();

                View_HR_Personnel personnelInfo = UniversalFunction.GetPersonnelInfo(dicFlowDataInfo["SQ"].OperationPersonnel);

                S_MaterialRejectBill bill = new S_MaterialRejectBill();

                bill.Bill_ID = _assignBillNoService.AssignNewNo(serverRejectBill, CE_BillTypeEnum.采购退货单.ToString());
                bill.Bill_Time = ServerTime.Time;
                bill.BillStatus = MaterialRejectBillBillStatus.已完成.ToString();
                bill.Department = personnelInfo.部门编码;
                bill.FillInPersonnel = personnelInfo.姓名;
                bill.FillInPersonnelCode = personnelInfo.工号;
                bill.Provider = sampleInfo.Purchase_Provider;
                bill.Reason = "样品确认申请单【" + sampleInfo.BillNo + "】 判定不合格退货";
                bill.Remark = "系统自动生成";
                bill.BillType = "总仓库退货单";
                bill.StorageID = indepotInfo.StorageID;
                bill.OutDepotDate = ServerTime.Time;

                context.S_MaterialRejectBill.InsertOnSubmit(bill);
                context.SubmitChanges();


                //插入业务明细信息
                S_MaterialListRejectBill goods = new S_MaterialListRejectBill();

                goods.Bill_ID = bill.Bill_ID;
                goods.GoodsID = sampleInfo.Purchase_GoodsID;
                goods.Provider = sampleInfo.Purchase_Provider;
                goods.ProviderBatchNo = sampleInfo.Purchase_ProviderBatchNo;
                goods.BatchNo = sampleInfo.Purchase_BatchNo;
                goods.Amount = (decimal)indepotInfo.InDepotCount;
                goods.Remark = "";
                goods.AssociateID = sampleInfo.Purchase_OrderFormNo;

                if (!serverRejectListBill.SetPriceInfo(goods.AssociateID, goods, bill.StorageID, out error))
                {
                    throw new Exception(error);
                }

                context.S_MaterialListRejectBill.InsertOnSubmit(goods);
                context.SubmitChanges();


                serverRejectBill.OpertaionDetailAndStock(context, bill);
                context.SubmitChanges();
            }
            else
            {
                throw new Exception("此批次的物品多次入库，操作失败");
            }
        }

        /// <summary>
        /// 生成领料退库单
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="sampleInfo">单据信息</param>
        void InsertReturnBill(DepotManagementDataContext context, Business_Sample_ConfirmTheApplication sampleInfo)
        {
            IMaterialReturnedInTheDepot serverReturnedBill = ServerModule.ServerModuleFactory.GetServerModule<IMaterialReturnedInTheDepot>();

            var varData = from a in context.S_MaterialRequisition
                          join b in context.S_MaterialRequisitionGoods
                          on a.Bill_ID equals b.Bill_ID
                          where a.BillStatus == "已出库" && b.GoodsID == sampleInfo.Purchase_GoodsID
                          && b.BatchNo == sampleInfo.Purchase_BatchNo
                          select a;

            if (varData.Count() > 0)
            {
                foreach (S_MaterialRequisition bill in varData)
                {
                    //操作主表
                    S_MaterialReturnedInTheDepot returnBill = new S_MaterialReturnedInTheDepot();

                    returnBill.Bill_ID = _assignBillNoService.AssignNewNo(serverReturnedBill,
                        CE_BillTypeEnum.领料退库单.ToString());
                    returnBill.Bill_Time = ServerTime.Time;
                    returnBill.BillStatus = MaterialReturnedInTheDepotBillStatus.已完成.ToString();
                    returnBill.Department = bill.Department;
                    returnBill.ReturnType = "其他退库";//退库类别
                    returnBill.FillInPersonnel = bill.FillInPersonnel;
                    returnBill.FillInPersonnelCode = bill.FillInPersonnelCode;
                    returnBill.DepartmentDirector = bill.DepartmentDirector;
                    returnBill.QualityInputer = "";
                    returnBill.DepotManager = bill.DepotManager;
                    returnBill.PurposeCode = bill.PurposeCode;
                    returnBill.ReturnReason = "样品确认申请单【" + sampleInfo.BillNo + "】 判定不合格退货";
                    returnBill.Remark = "系统自动生成";
                    returnBill.StorageID = bill.StorageID;
                    returnBill.ReturnMode = "领料退库";//退库方式
                    returnBill.IsOnlyForRepair = false;
                    returnBill.InDepotDate = ServerTime.Time;

                    context.S_MaterialReturnedInTheDepot.InsertOnSubmit(returnBill);
                    context.SubmitChanges();

                    //操作明细
                    var varData1 = from a in context.S_MaterialRequisitionGoods
                                   where a.Bill_ID == bill.Bill_ID
                                   && a.GoodsID == sampleInfo.Purchase_GoodsID
                                   && a.BatchNo == sampleInfo.Purchase_BatchNo
                                   select a;

                    S_MaterialRequisitionGoods goodsInfo = varData1.Single();

                    QueryCondition_Store queryInfo = new QueryCondition_Store();

                    queryInfo.BatchNo = sampleInfo.Purchase_BatchNo;
                    queryInfo.GoodsID = sampleInfo.Purchase_GoodsID;
                    queryInfo.StorageID = bill.StorageID;

                    S_Stock stockInfo = UniversalFunction.GetStockInfo(context, queryInfo);

                    S_MaterialListReturnedInTheDepot detailInfo = new S_MaterialListReturnedInTheDepot();

                    detailInfo.BatchNo = sampleInfo.Purchase_BatchNo;
                    detailInfo.Bill_ID = returnBill.Bill_ID;
                    detailInfo.ColumnNumber = stockInfo.ColumnNumber;
                    detailInfo.Depot = stockInfo.Depot;
                    detailInfo.GoodsID = sampleInfo.Purchase_GoodsID;
                    detailInfo.LayerNumber = stockInfo.LayerNumber;
                    detailInfo.Provider = stockInfo.Provider;
                    detailInfo.ProviderBatchNo = sampleInfo.Purchase_ProviderBatchNo;
                    detailInfo.Remark = "";
                    detailInfo.ReturnedAmount = goodsInfo.RealCount;
                    detailInfo.ShelfArea = stockInfo.ShelfArea;

                    context.S_MaterialListReturnedInTheDepot.InsertOnSubmit(detailInfo);
                    context.SubmitChanges();

                    serverReturnedBill.OpertaionDetailAndStock(context, returnBill);
                    context.SubmitChanges();
                }
            }
        }

        /// <summary>
        /// 插入正常报检入库表中
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="sampleInfo">样品单信息</param>
        /// <param name="dicFlowDataInfo">流程节点信息字典</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>插入成功返回True，插入失败返回False</returns>
        bool InsertCheckInDepotBill(DepotManagementDataContext context, Business_Sample_ConfirmTheApplication sampleInfo,
            Dictionary<string, Flow_FlowData> dicFlowDataInfo, out string error)
        {
            ICheckOutInDepotServer serverCheckOutInDepot = ServerModule.ServerModuleFactory.GetServerModule<ICheckOutInDepotServer>();

            try
            {
                error = null;

                F_GoodsPlanCost lnqGoods = ServerModule.ServerModuleFactory.GetServerModule<IBasicGoodsServer>().GetGoodsInfo(Convert.ToInt32(sampleInfo.Purchase_GoodsID));

                string strBillID = serverCheckOutInDepot.GetNextBillNo(1);
                decimal dcBargainUnitPrice = _serviceBargainInfo.GetBargainUnitPrice(sampleInfo.Purchase_OrderFormNo,
                    Convert.ToInt32(sampleInfo.Purchase_GoodsID));

                S_CheckOutInDepotBill lnqCheckBill = new S_CheckOutInDepotBill();

                lnqCheckBill.ArriveGoods_Time = dicFlowDataInfo["DH"].OperationTime;
                lnqCheckBill.BatchNo = sampleInfo.Purchase_BatchNo;
                lnqCheckBill.Bill_ID = strBillID;
                lnqCheckBill.Bill_Time = dicFlowDataInfo["SQ"].OperationTime;
                lnqCheckBill.BillStatus = "已入库";;
                lnqCheckBill.DeclareWastrelCount = Convert.ToInt32(sampleInfo.Inspect_GoodsCount_Scarp);
                lnqCheckBill.DepotManagerAffirmCount = Convert.ToInt32(sampleInfo.Store_GoodsCount_AOG);
                lnqCheckBill.Buyer = dicFlowDataInfo["SQ"].OperationPersonnel;
                lnqCheckBill.Checker = dicFlowDataInfo["JY"].OperationPersonnel;
                lnqCheckBill.CheckOutGoodsType = 1;
                lnqCheckBill.CheckoutJoinGoods_Time = dicFlowDataInfo["JY"].OperationTime;
                lnqCheckBill.CheckoutReport_ID = sampleInfo.Inspect_ReportNo;
                lnqCheckBill.CheckTime = dicFlowDataInfo["JY"].OperationTime;
                lnqCheckBill.ColumnNumber = sampleInfo.Store_Put_AreaNo;

                if (sampleInfo.Review_InspectResult == "让步接收")
                {
                    lnqCheckBill.ConcessionCount = Convert.ToInt32(sampleInfo.Store_GoodsCount_AOG) - lnqCheckBill.DeclareWastrelCount;
                }
                else
                {
                    lnqCheckBill.ConcessionCount = 0;
                }

                lnqCheckBill.ConfirmAmountSignatory = dicFlowDataInfo["RK"].OperationPersonnel;
                lnqCheckBill.DeclareCount = Convert.ToInt32(sampleInfo.Purchase_GoodsCount_Send);

                if (lnqCheckBill.DeclareCount == 0)
                {
                    error = "入库数量不能为0";
                    return false;
                }

                lnqCheckBill.DeclarePersonnel = dicFlowDataInfo["SQ"].OperationPersonnel;
                lnqCheckBill.DeclarePersonnelCode = UniversalFunction.GetPersonnelCode(dicFlowDataInfo["SQ"].OperationPersonnel);
                lnqCheckBill.DepotAffirmanceTime = dicFlowDataInfo["DH"].OperationTime;
                lnqCheckBill.DepotManager = dicFlowDataInfo["RK"].OperationPersonnel;
                lnqCheckBill.Depot = lnqGoods.GoodsType;

                if (sampleInfo.Review_InspectResult == "返工/返修")
                {
                    lnqCheckBill.EligibleCount = sampleInfo.Purchase_SampleType == "PPAP样件" ?
                        Convert.ToInt32(sampleInfo.SQE_SampleDisposeType_QualificationCount) :
                        Convert.ToInt32(sampleInfo.Review_InspectResult_ReWork_QualificationCount);
                }
                else if (sampleInfo.Review_InspectResult == "合格")
                {
                    lnqCheckBill.EligibleCount = Convert.ToInt32(sampleInfo.Store_GoodsCount_AOG) - lnqCheckBill.DeclareWastrelCount;
                }
                else
                {
                    lnqCheckBill.EligibleCount = 0;
                }

                lnqCheckBill.GoodsID = Convert.ToInt32(sampleInfo.Purchase_GoodsID);

                lnqCheckBill.InDepotCount = _InDepotCount != 0 ? (int)_InDepotCount :
                    lnqCheckBill.EligibleCount + lnqCheckBill.DeclareWastrelCount + lnqCheckBill.ConcessionCount;

                lnqCheckBill.InDepotTime = ServerTime.Time;
                lnqCheckBill.IsExigenceCheck = false;
                lnqCheckBill.HavingInvoice = false;
                lnqCheckBill.InvoicePrice = 0;
                lnqCheckBill.LayerNumber = sampleInfo.Store_Put_LayerNo;
                lnqCheckBill.OrderFormNumber = sampleInfo.Purchase_OrderFormNo;
                lnqCheckBill.PeremptorilyEmit = false;
                lnqCheckBill.PlanPrice = sampleInfo.Purchase_IsPay == true ?
                    Convert.ToDecimal(lnqGoods.GoodsUnitPrice) * Convert.ToDecimal(lnqCheckBill.InDepotCount) : 0;
                lnqCheckBill.PlanUnitPrice = sampleInfo.Purchase_IsPay == true ?
                    Convert.ToDecimal(lnqGoods.GoodsUnitPrice) : 0;

                lnqCheckBill.UnitPrice = sampleInfo.Purchase_IsPay == true ? dcBargainUnitPrice : 0;
                lnqCheckBill.Price = sampleInfo.Purchase_IsPay == true ?
                    Math.Round(Convert.ToDecimal(lnqCheckBill.InDepotCount) * dcBargainUnitPrice, 2) : 0;

                lnqCheckBill.Provider = sampleInfo.Purchase_Provider;
                lnqCheckBill.ProviderBatchNo = sampleInfo.Purchase_ProviderBatchNo;
                lnqCheckBill.QualityInfo = sampleInfo.Inspect_Result == true ? "合格" : "不合格";
                lnqCheckBill.QualityInputer = dicFlowDataInfo["JY"].OperationPersonnel;

                if (sampleInfo.Review_InspectResult == "返工/返修")
                {
                    lnqCheckBill.ReimbursementCount = sampleInfo.Purchase_SampleType == "PPAP样件" ?
                        Convert.ToInt32(sampleInfo.SQE_SampleDisposeType_DisqualificationCount) :
                        Convert.ToInt32(sampleInfo.Review_InspectResult_ReWork_DisqualificationCount);
                }
                else
                {
                    lnqCheckBill.ReimbursementCount = 0;
                }

                lnqCheckBill.ReimbursementCount = sampleInfo.Purchase_SampleType == "PPAP样件" ?
                    Convert.ToInt32(sampleInfo.SQE_SampleDisposeType_DisqualificationCount) : 
                    Convert.ToInt32(sampleInfo.Review_InspectResult_ReWork_DisqualificationCount);
                lnqCheckBill.ShelfArea = sampleInfo.Store_Put_Area;
                lnqCheckBill.StorageID = sampleInfo.Purchase_StorageID;
                lnqCheckBill.TFFlag = false;

                lnqCheckBill.TotalPrice = sampleInfo.Purchase_IsPay == true ? CalculateClass.GetTotalPrice(lnqCheckBill.Price) : "零";

                lnqCheckBill.UnitInvoicePrice = 0;
                lnqCheckBill.Remark = "由样品单 [" + sampleInfo.BillNo + "] 生成的报检单";
                lnqCheckBill.Version = sampleInfo.Purchase_Version;

                if (UniversalFunction.GetStorageInfo_View(lnqCheckBill.StorageID).零成本控制)
                {
                    throw new Exception("【零成本】库房，无法通过【报检入库单】入库");
                }

                context.S_CheckOutInDepotBill.InsertOnSubmit(lnqCheckBill);
                serverCheckOutInDepot.OpertaionDetailAndStock(context, lnqCheckBill);

                context.SubmitChanges();

                string mrBillNo = "";

                if ((int)lnqCheckBill.DeclareWastrelCount > 0 && lnqCheckBill.InDepotCount > 0 && _InDepotCount == 0)
                {
                    if (!serverCheckOutInDepot.InsertIntoMaterialRequisition(context, lnqCheckBill, out mrBillNo, out error))
                    {
                        throw new Exception(error);
                    }
                }

                context.SubmitChanges();
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
        /// <param name="sampleInfo">样品单信息</param>
        /// <param name="dicFlowDataInfo">流程节点信息字典</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>插入成功返回True，插入失败返回False</returns>
        bool InsertCheckInDepotOutsourcingBill(DepotManagementDataContext context, Business_Sample_ConfirmTheApplication sampleInfo,
            Dictionary<string, Flow_FlowData> dicFlowDataInfo, out string error)
        {
            ICheckOutInDepotForOutsourcingServer serverOutsourcingServer =
                ServerModule.ServerModuleFactory.GetServerModule<ICheckOutInDepotForOutsourcingServer>();

            F_GoodsPlanCost lnqGoods = ServerModule.ServerModuleFactory.GetServerModule<IBasicGoodsServer>().GetGoodsInfo(Convert.ToInt32(sampleInfo.Purchase_GoodsID));

            try
            {
                error = null;

                string strBillID = _assignBillNoService.AssignNewNo(serverOutsourcingServer, CE_BillTypeEnum.委外报检入库单.ToString());
                decimal dcBargainUnitPrice = _serviceBargainInfo.GetBargainUnitPrice(context, sampleInfo.Purchase_OrderFormNo, 
                    Convert.ToInt32(sampleInfo.Purchase_GoodsID));

                S_CheckOutInDepotForOutsourcingBill lnqCheckBill = new S_CheckOutInDepotForOutsourcingBill();

                lnqCheckBill.ArrivePersonnel = dicFlowDataInfo["DH"].OperationPersonnel;
                lnqCheckBill.ArriveTime = dicFlowDataInfo["DH"].OperationTime;
                lnqCheckBill.BatchNo = sampleInfo.Purchase_BatchNo;
                lnqCheckBill.Bill_ID = strBillID;
                lnqCheckBill.BillStatus = "已入库";
                lnqCheckBill.DeclareWastrelCount = (decimal)sampleInfo.Inspect_GoodsCount_Scarp;
                lnqCheckBill.DepotManagerAffirmCount = (decimal)sampleInfo.Store_GoodsCount_AOG;
                lnqCheckBill.Checker = dicFlowDataInfo["JY"].Advise;
                lnqCheckBill.CheckoutReport_ID = sampleInfo.Inspect_ReportNo;
                lnqCheckBill.ColumnNumber = sampleInfo.Store_Put_AreaNo;

                if (sampleInfo.Review_InspectResult == "让步接收")
                {
                    lnqCheckBill.ConcessionCount = Convert.ToInt32(sampleInfo.Store_GoodsCount_AOG) - lnqCheckBill.DeclareWastrelCount;
                }
                else
                {
                    lnqCheckBill.ConcessionCount = 0;
                }

                lnqCheckBill.DeclareCount = sampleInfo.Purchase_GoodsCount_Send;
                lnqCheckBill.DeclarePersonnel = dicFlowDataInfo["SQ"].OperationPersonnel;
                lnqCheckBill.DeclareTime = dicFlowDataInfo["SQ"].OperationTime;
                lnqCheckBill.Depot = lnqGoods.GoodsType;
                lnqCheckBill.FinancePersonnel = dicFlowDataInfo.Keys.Contains("CW") ? dicFlowDataInfo["CW"].OperationPersonnel : null;
                lnqCheckBill.FinanceTime = dicFlowDataInfo.Keys.Contains("CW") ? (DateTime?)dicFlowDataInfo["CW"].OperationTime : null;

                if (sampleInfo.Review_InspectResult == "返工/返修")
                {
                    lnqCheckBill.EligibleCount = sampleInfo.Purchase_SampleType == "PPAP样件" ?
                        Convert.ToInt32(sampleInfo.SQE_SampleDisposeType_QualificationCount) :
                        Convert.ToInt32(sampleInfo.Review_InspectResult_ReWork_QualificationCount);
                }
                else if (sampleInfo.Review_InspectResult == "合格")
                {
                    lnqCheckBill.EligibleCount = Convert.ToInt32(sampleInfo.Store_GoodsCount_AOG) - lnqCheckBill.DeclareWastrelCount;
                }
                else
                {
                    lnqCheckBill.EligibleCount = 0;
                }

                lnqCheckBill.GoodsID = Convert.ToInt32(sampleInfo.Purchase_GoodsID);

                lnqCheckBill.InDepotCount = _InDepotCount != 0 ? _InDepotCount :
                    lnqCheckBill.EligibleCount + lnqCheckBill.DeclareWastrelCount + lnqCheckBill.ConcessionCount;

                lnqCheckBill.ManagerTime = ServerTime.Time;
                lnqCheckBill.ManagerPersonnel = dicFlowDataInfo["RK"].OperationPersonnel;
                lnqCheckBill.IsExigenceCheck = false;
                lnqCheckBill.HavingInvoice = false;
                lnqCheckBill.InvoicePrice = 0;
                lnqCheckBill.LayerNumber = sampleInfo.Store_Put_LayerNo;
                lnqCheckBill.OrderFormNumber = sampleInfo.Purchase_OrderFormNo;
                lnqCheckBill.QualityPersonnel = dicFlowDataInfo["JY"].OperationPersonnel;
                lnqCheckBill.QualityTime = dicFlowDataInfo["JY"].OperationTime;
                lnqCheckBill.OutsourcingUnitPrice = sampleInfo.Purchase_IsPay == true ? dcBargainUnitPrice : 0;
                lnqCheckBill.RawMaterialPrice = sampleInfo.Purchase_IsPay == true ? (decimal)sampleInfo.Finance_RawMaterialCost : 0;
                lnqCheckBill.PeremptorilyEmit = false;
                lnqCheckBill.UnitPrice = sampleInfo.Purchase_IsPay == true ?
                      sampleInfo.Purchase_BillType.Contains("材料费") == true ?
                      dcBargainUnitPrice + (decimal)sampleInfo.Finance_RawMaterialCost : dcBargainUnitPrice : 0;

                lnqCheckBill.Price = sampleInfo.Purchase_IsPay == true ?
                    sampleInfo.Purchase_BillType.Contains("材料费") == true ?
                    lnqCheckBill.InDepotCount * (dcBargainUnitPrice + (decimal)sampleInfo.Finance_RawMaterialCost) :
                    lnqCheckBill.InDepotCount * dcBargainUnitPrice : 0;

                lnqCheckBill.Provider = sampleInfo.Purchase_Provider;
                lnqCheckBill.ProviderBatchNo = sampleInfo.Purchase_ProviderBatchNo;
                lnqCheckBill.QualityInfo = sampleInfo.Inspect_Result == true ? "合格" : "不合格";

                if (sampleInfo.Review_InspectResult == "返工/返修")
                {
                    lnqCheckBill.ReimbursementCount = sampleInfo.Purchase_SampleType == "PPAP样件" ?
                        Convert.ToInt32(sampleInfo.SQE_SampleDisposeType_DisqualificationCount) :
                        Convert.ToInt32(sampleInfo.Review_InspectResult_ReWork_DisqualificationCount);
                }
                else
                {
                    lnqCheckBill.ReimbursementCount = 0;
                }

                lnqCheckBill.ShelfArea = sampleInfo.Store_Put_Area;
                lnqCheckBill.StorageID = sampleInfo.Purchase_StorageID;
                lnqCheckBill.UnitInvoicePrice = 0;
                lnqCheckBill.IsIncludeRawMaterial = sampleInfo.Purchase_BillType.Contains("材料费");
                lnqCheckBill.Remark = "由样品单 [" + sampleInfo.BillNo + "] 生成的委外报检单";
                lnqCheckBill.Version = sampleInfo.Purchase_Version;

                context.S_CheckOutInDepotForOutsourcingBill.InsertOnSubmit(lnqCheckBill);

                serverOutsourcingServer.OpertaionDetailAndStock(context, lnqCheckBill);
                _assignBillNoService.UseBillNo(CE_BillTypeEnum.委外报检入库单.ToString(), strBillID);

                context.SubmitChanges();

                //若勾选了“包含原材料费”并且报废数大于0，则插入报废单
                if (lnqCheckBill.IsExigenceCheck && lnqCheckBill.DeclareWastrelCount > 0 && _InDepotCount == 0)
                {
                    if (!serverOutsourcingServer.AddScrapBill(context, lnqCheckBill, out error))
                    {
                        throw new Exception(error);
                    }
                }

                context.SubmitChanges();

                string mrBillNo = "";
                if ((int)lnqCheckBill.DeclareWastrelCount > 0 && lnqCheckBill.InDepotCount > 0 && _InDepotCount == 0)
                {
                    if (!serverOutsourcingServer.InsertIntoMaterialRequisition(context, lnqCheckBill, out mrBillNo, out error))
                    {
                        throw new Exception(error);
                    }
                }

                context.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 自制件入库单
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="inMuster">样品确认单LINQ数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        bool InsertHomemadePartInDepotBill(DepotManagementDataContext context, Business_Sample_ConfirmTheApplication sample,
            Dictionary<string, Flow_FlowData> dicFlowDataInfo, out string error)
        {
            IHomemadePartInDepotServer serverHomemade = ServerModule.ServerModuleFactory.GetServerModule<IHomemadePartInDepotServer>();
            error = null;
            try
            {

                F_GoodsPlanCost lnqGoods = 
                    ServerModule.ServerModuleFactory.GetServerModule<IBasicGoodsServer>().GetGoodsInfo(Convert.ToInt32(sample.Purchase_GoodsID));
                string strBillID = _assignBillNoService.AssignNewNo(serverHomemade, CE_BillTypeEnum.自制件入库单.ToString());
                S_HomemadePartBill lnqHomemade = new S_HomemadePartBill();

                lnqHomemade.BatchNo = sample.Purchase_BatchNo;
                lnqHomemade.Bill_ID = strBillID;
                lnqHomemade.Bill_Time = dicFlowDataInfo["SQ"].OperationTime;
                lnqHomemade.BillStatus = "已入库";
                lnqHomemade.Checker = dicFlowDataInfo["JY"].OperationPersonnel;
                lnqHomemade.CheckoutJoinGoods_Time = dicFlowDataInfo["JY"].OperationTime;
                lnqHomemade.CheckoutReport_ID = sample.Inspect_ReportNo;
                lnqHomemade.ColumnNumber = sample.Store_Put_AreaNo;

                if (sample.Review_InspectResult == "让步接收")
                {
                    lnqHomemade.ConcessionCount = Convert.ToInt32(sample.Store_GoodsCount_AOG);
                }
                else
                {
                    lnqHomemade.ConcessionCount = 0;
                }

                lnqHomemade.ConfirmAmountSignatory = dicFlowDataInfo["DH"].OperationPersonnel;
                lnqHomemade.DeclareCount = (int)sample.Purchase_GoodsCount_Send;

                if (lnqHomemade.DeclareCount == 0)
                {
                    error = "入库数量不能为0";
                    return false;
                }

                lnqHomemade.DeclarePersonnel = dicFlowDataInfo["SQ"].OperationPersonnel;
                lnqHomemade.DeclarePersonnelCode = UniversalFunction.GetPersonnelCode(dicFlowDataInfo["SQ"].OperationPersonnel);
                lnqHomemade.DeclareWastrelCount = Convert.ToInt32( sample.Inspect_GoodsCount_Scarp);
                lnqHomemade.DepotManager = dicFlowDataInfo["RK"].OperationPersonnel;
                lnqHomemade.DepotManagerAffirmCount = (int)sample.Store_GoodsCount_AOG;

                if (sample.Review_InspectResult == "返工/返修")
                {
                    lnqHomemade.EligibleCount = sample.Purchase_SampleType == "PPAP样件" ?
                        Convert.ToInt32(sample.SQE_SampleDisposeType_QualificationCount) :
                        Convert.ToInt32(sample.Review_InspectResult_ReWork_QualificationCount);
                }
                else if (sample.Review_InspectResult == "合格")
                {
                    lnqHomemade.EligibleCount = Convert.ToInt32(sample.Store_GoodsCount_AOG);
                }
                else
                {
                    lnqHomemade.EligibleCount = 0;
                }

                lnqHomemade.GoodsID = Convert.ToInt32(sample.Purchase_GoodsID);

                lnqHomemade.InDepotCount = _InDepotCount != 0 ? (int)_InDepotCount :
                    lnqHomemade.EligibleCount + lnqHomemade.DeclareWastrelCount + lnqHomemade.ConcessionCount;

                lnqHomemade.InDepotTime = ServerTime.Time;
                lnqHomemade.LayerNumber = sample.Store_Put_LayerNo;
                lnqHomemade.PlanPrice = 0;
                lnqHomemade.PlanUnitPrice = 0;
                lnqHomemade.Price = 0;
                lnqHomemade.Provider = sample.Purchase_Provider;
                lnqHomemade.ProviderBatchNo = sample.Purchase_ProviderBatchNo;
                lnqHomemade.QualityInfo = sample.Inspect_Result == true ? "合格" : "不合格";
                lnqHomemade.QualityInputer = dicFlowDataInfo["JY"].OperationPersonnel;

                if (sample.Review_InspectResult == "返工/返修")
                {
                    lnqHomemade.ReimbursementCount = sample.Purchase_SampleType == "PPAP样件" ?
                        Convert.ToInt32(sample.SQE_SampleDisposeType_DisqualificationCount) :
                        Convert.ToInt32(sample.Review_InspectResult_ReWork_DisqualificationCount);
                }
                else
                {
                    lnqHomemade.ReimbursementCount = 0;
                }

                lnqHomemade.Remark = "由样品单 [" + sample.BillNo + "] 生成的自制件入库单";
                lnqHomemade.ShelfArea = sample.Store_Put_Area;
                lnqHomemade.StorageID = sample.Purchase_StorageID;
                lnqHomemade.TotalPrice = "零元整";
                lnqHomemade.UnitPrice = 0;

                context.S_HomemadePartBill.InsertOnSubmit(lnqHomemade);
                serverHomemade.OpertaionDetailAndStock(context, lnqHomemade);

                _assignBillNoService.UseBillNo(CE_BillTypeEnum.自制件入库单.ToString(), strBillID);
                context.SubmitChanges();

                string mrBillNo = "";
                if ((int)lnqHomemade.DeclareWastrelCount > 0 && lnqHomemade.InDepotCount > 0 && _InDepotCount == 0)
                {
                    if (!serverHomemade.InsertIntoMaterialRequisition(context, lnqHomemade, out mrBillNo, out error))
                    {
                        throw new Exception(error);
                    }
                }

                context.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

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

            batchNo = batchNo.Replace("\r\n", "/");

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
        /// 获得耗用数
        /// </summary>
        /// <param name="sample">单据信息</param>
        /// <returns>返回耗用数</returns>
        public decimal GetUseCount(Business_Sample_ConfirmTheApplication sample)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.S_FetchGoodsDetailBill
                          where a.GoodsID == sample.Purchase_GoodsID
                          && a.BatchNo == sample.Purchase_BatchNo
                          select a;

            if (varData.Count() > 0)
            {
                return varData.Sum(k => Convert.ToDecimal(k.FetchCount));
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 插入条形码
        /// </summary>
        /// <param name="inMuster">样品单信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>插入成功返回True，插入失败返回False</returns>
        bool InsertNewBarCode(Business_Sample_ConfirmTheApplication sample, out string error)
        {
            error = null;

            IBarCodeServer barCodeService = ServerModule.ServerModuleFactory.GetServerModule<IBarCodeServer>();

            // 2014-05-20 夏石友，判断条形码是否存在时增加了供应商参数
            if (!barCodeService.IsExists(Convert.ToInt32(sample.Purchase_GoodsID),
                sample.Purchase_StorageID, sample.Purchase_BatchNo, sample.Purchase_Provider))
            {
                S_InDepotGoodsBarCodeTable newBarcode = new S_InDepotGoodsBarCodeTable();

                newBarcode.GoodsID = Convert.ToInt32(sample.Purchase_GoodsID);
                newBarcode.Provider = sample.Purchase_Provider;
                newBarcode.BatchNo = sample.Purchase_BatchNo;
                newBarcode.ProductFlag = "0";
                newBarcode.StorageID = sample.Purchase_StorageID;

                if (!barCodeService.Add(newBarcode, out error))
                {
                    return false;
                }
            }

            BASE_Storage stroageInfo = UniversalFunction.GetStorageInfo(sample.Purchase_StorageID);

            if (!stroageInfo.WorkShopCurrentAccount)
            {
                return true;
            }


            View_F_GoodsPlanCost goodsInfo = UniversalFunction.GetGoodsInfo(sample.Purchase_GoodsID);
            string strTemp =
                GlobalObject.GeneralFunction.ListToString<string>(UniversalFunction.GetApplicableMode_CGBOM(sample.Purchase_GoodsID), ",");

            List<string> lstText = new List<string>();

            lstText.Add(" 物品标识（part list）");
            lstText.Add("名称 " + goodsInfo.物品名称);
            lstText.Add("图号 " + goodsInfo.图号型号);
            lstText.Add("型号 " + strTemp);
            lstText.Add("批号 " + sample.Purchase_BatchNo);
            lstText.Add("数量 " + sample.Store_GoodsCount_AOG.ToString() + "※供应商 " + sample.Purchase_Provider);
            lstText.Add("日期 " + ServerTime.Time.ToShortDateString());

            ServerModule.PrintPartBarcode.PrintBarcode_Common(lstText);
            return true;
        }

        #endregion
    }
}
