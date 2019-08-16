using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using GlobalObject;
using PlatformManagement;
using UniversalControlLibrary;
using Service_Project_Project;
using FlowControlService;
using Service_Quality_QC;

namespace Form_Project_Project
{
    public partial class 样品确认申请单 : CustomMainForm
    {
        /// <summary>
        /// 流程服务组件
        /// </summary>
        IFlowServer m_serverFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();

        /// <summary>
        /// 服务组件
        /// </summary>
        ISampleApplication m_serviceSample = Service_Project_Project.ServerModuleFactory.GetServerModule<ISampleApplication>();

        public 样品确认申请单()
            : base(typeof(样品确认申请单明细), GlobalObject.CE_BillTypeEnum.样品确认申请单,
                Service_Project_Project.ServerModuleFactory.GetServerModule<ISampleApplication>())
        {
            InitializeComponent();
        }

        void CreateIsolationBusiness(Business_Sample_ConfirmTheApplication lnqSample)
        {
            IRejectIsolationService serviceIsolation =
                Service_Quality_QC.ServerModuleFactory.GetServerModule<IRejectIsolationService>();
            BillNumberControl billNoControl = new BillNumberControl(CE_BillTypeEnum.不合格品隔离处置单.ToString(), serviceIsolation);

            QueryCondition_Store queryInfo = new QueryCondition_Store();
            queryInfo.BatchNo = lnqSample.Purchase_BatchNo;
            queryInfo.GoodsID = lnqSample.Purchase_GoodsID;
            queryInfo.Provider = lnqSample.Purchase_Provider;
            queryInfo.StorageID = lnqSample.Purchase_StorageID;

            List<S_Stock> stockList = UniversalFunction.GetStockInfoList(queryInfo);

            foreach (S_Stock stockInfo in stockList)
            {
                Business_QualityManagement_Isolation lnqIsolation = new Business_QualityManagement_Isolation();

                lnqIsolation.BillNo = billNoControl.GetNewBillNo();
                lnqIsolation.BatchNo = lnqSample.Purchase_BatchNo;
                lnqIsolation.GoodsID = lnqSample.Purchase_GoodsID;
                lnqIsolation.Provider = lnqSample.Purchase_Provider;
                lnqIsolation.StorageID = stockInfo.StorageID;
                lnqIsolation.IsolationReason = lnqSample.Review_RectificationItem_Explain;
                lnqIsolation.GoodsCount = stockInfo.ExistCount;

                string KeyWords = "【" + UniversalFunction.GetGoodsInfo(lnqIsolation.GoodsID).物品名称 + "】【" + lnqIsolation.BatchNo + "】";

                if (!serviceIsolation.IsRepeatIsolation(lnqIsolation.GoodsID, lnqIsolation.BatchNo, lnqIsolation.StorageID))
                {
                    serviceIsolation.SaveInfo(lnqIsolation);
                    serviceIsolation.FinishBill(lnqIsolation.BillNo);
                    m_serverFlow.FlowHold(lnqIsolation.BillNo, lnqIsolation.StorageID, "暂存 由【样品确认申请单】："
                        + lnqSample.BillNo + " 系统自动生成", KeyWords);
                    MessageDialog.ShowPromptMessage("由您填写的相关信息导致此物品已被隔离，且已生成【不合格品隔离处置单】，单号【" 
                        + lnqIsolation.BillNo + "】,且【隔离人】是您本人，请及时处理");
                }
            }
        }

        void OperationAssociationBusiness(Business_Sample_ConfirmTheApplication lnqSample)
        {
            Flow_FlowInfo lnqFlowInfo =
                m_serverFlow.GetNowFlowInfo(m_serverFlow.GetBusinessTypeID(CE_BillTypeEnum.样品确认申请单, null), lnqSample.BillNo);
            
            if (lnqFlowInfo.FlowID == 46 || lnqFlowInfo.FlowID == 62)
            {
                if (m_serviceSample.IsInStore(lnqSample))
                {
                    if (m_serviceSample.IsNeedIsolation(lnqSample))
                    {
                        CreateIsolationBusiness(lnqSample);
                    }
                }
            }
            else if (lnqFlowInfo.FlowID == 49)
            {
                if (m_serviceSample.IsInStore(lnqSample))
                {
                    if (lnqSample.ChargeResult_ResultAffrim_DiposeSuggestion_Surplus == "报废/退货" )
                    {
                        CreateIsolationBusiness(lnqSample);
                    }
                }
            }
        }

        bool 样品确认申请单_Form_CommonProcessSubmit(CustomFlowForm form, string advise)
        {
            try
            {
                Business_Sample_ConfirmTheApplication lnqBillInfo =
                    form.ResultInfo as Business_Sample_ConfirmTheApplication;

                this.OperationType = GeneralFunction.StringConvertToEnum<CE_FlowOperationType>(form.ResultList[0].ToString());
                this.BillNo = lnqBillInfo.BillNo;

                switch (this.OperationType)
                {
                    case CE_FlowOperationType.提交:
                        OperationAssociationBusiness(lnqBillInfo);
                        m_serviceSample.SaveInfo(lnqBillInfo);
                        m_serviceSample.OperatarUnFlowBusiness(lnqBillInfo.BillNo);
                        break;
                    case CE_FlowOperationType.暂存:
                        m_serviceSample.SaveInfo(lnqBillInfo);
                        break;
                    case CE_FlowOperationType.回退:
                        break;
                    case CE_FlowOperationType.未知:
                        break;
                    default:
                        break;
                }

                if (!m_serviceSample.IsExist(lnqBillInfo.BillNo))
                {
                    MessageDialog.ShowPromptMessage("数据为空，保存失败，如需退出，请直接X掉界面");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
                return false;
            }
        }
    }
}
