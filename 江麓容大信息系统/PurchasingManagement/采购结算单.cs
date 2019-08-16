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
using Service_Economic_Purchase;
using FlowControlService;


namespace Form_Economic_Purchase
{
    public partial class 采购结算单 : CustomMainForm
    {
        /// <summary>
        /// 操作权限
        /// </summary>
        AuthorityFlag m_authorityFlag;

        /// <summary>
        /// 流程服务组件
        /// </summary>
        IFlowServer m_serverFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();

        /// <summary>
        /// 服务组件
        /// </summary>
        IProcurementStatement m_serviceStatement = Service_Economic_Purchase.ServerModuleFactory.GetServerModule<IProcurementStatement>();

        public 采购结算单(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
            : base(typeof(采购结算单明细), GlobalObject.CE_BillTypeEnum.采购结算单,
            Service_Economic_Purchase.ServerModuleFactory.GetServerModule<IProcurementStatement>())
        {
            InitializeComponent();

            this.Form_CommonProcessSubmit += new FormCommonProcess.FormSubmit(采购结算单_Form_CommonProcessSubmit);
            this.Form_btnPrint += new DelegateCollection.ButtonClick(采购结算单_Form_btnPrint);

            m_authorityFlag = nodeInfo.Authority;
        }

        bool 采购结算单_Form_CommonProcessSubmit(CustomFlowForm form, string advise)
        {
            try
            {
                Business_Settlement_ProcurementStatement lnqBillInfo =
                    form.ResultInfo as Business_Settlement_ProcurementStatement;

                List<Business_Settlement_ProcurementStatement_Invoice> invoiceInfo =
                    form.ResultList[0] as List<Business_Settlement_ProcurementStatement_Invoice>;
                List<View_Business_Settlement_ProcurementStatementDetail> detailInfo =
                    form.ResultList[1] as List<View_Business_Settlement_ProcurementStatementDetail>;

                this.OperationType = GeneralFunction.StringConvertToEnum<CE_FlowOperationType>(form.ResultList[2].ToString());
                this.BillNo = lnqBillInfo.BillNo;

                switch (this.OperationType)
                {
                    case CE_FlowOperationType.提交:
                        m_serviceStatement.SaveInfo(lnqBillInfo, invoiceInfo, detailInfo);
                        m_serviceStatement.OperatarUnFlowBusiness(lnqBillInfo.BillNo);
                        break;
                    case CE_FlowOperationType.暂存:
                        m_serviceStatement.SaveInfo(lnqBillInfo, invoiceInfo, detailInfo);
                        break;
                    case CE_FlowOperationType.回退:
                        break;
                    case CE_FlowOperationType.未知:
                        break;
                    default:
                        break;
                }

                if (!m_serviceStatement.IsExist(lnqBillInfo.BillNo))
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

        private void 采购结算单_Form_btnPrint(object sender, EventArgs e)
        {
            报表_采购结算单 report = new 报表_采购结算单(this.BillNo, GlobalObject.CE_BillTypeEnum.采购结算单.ToString());
            report.ShowDialog();
        }

        private void btnReAuditing_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.BillNo != null)
                {
                    if (m_serverFlow.GetNowBillStatus(this.BillNo) == "单据完成")
                    {
                        Flow_FlowData flowData =
                            m_serverFlow.GetBusinessOperationInfo(this.BillNo).OrderByDescending(k => k.OperationTime).First();

                        if (UniversalFunction.GetYearAndMonth(ServerTime.Time) != UniversalFunction.GetYearAndMonth(flowData.OperationTime))
                        {
                            MessageDialog.ShowPromptMessage("无法【重审】非当月单据");
                            return;
                        }

                        if (MessageDialog.ShowEnquiryMessage("确定要重审【" + this.BillNo + "】号单据？") == DialogResult.Yes)
                        {
                            m_serverFlow.FlowReback_Finish(this.BillNo, "重审 ", "", 37, null);
                            MessageDialog.ShowPromptMessage("已可以重审【" + this.BillNo + "】号单据");

                            this.RefreshData(this.tabControl1.SelectedTab);
                            this.PositioningRecord(this.BillNo.ToString());
                            return;
                        }
                    }
                    else
                    {
                        MessageDialog.ShowPromptMessage("只能重审已完成的单据");
                        return;
                    }

                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowErrorMessage(ex.Message);
                return;
            }
        }

        private void 采购结算单_Load(object sender, EventArgs e)
        {
            this.ToolStripSeparator_ShowStatus(m_authorityFlag);
        }
    }
}
