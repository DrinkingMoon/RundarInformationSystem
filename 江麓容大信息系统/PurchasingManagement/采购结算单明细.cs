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
    public partial class 采购结算单明细 : CustomFlowForm
    {
        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 单据号
        /// </summary>
        private Business_Settlement_ProcurementStatement m_lnqBillInfo = new Business_Settlement_ProcurementStatement();

        public Business_Settlement_ProcurementStatement LnqBillInfo
        {
            get { return m_lnqBillInfo; }
            set { m_lnqBillInfo = value; }
        }

        /// <summary>
        /// 明细视图数据集合
        /// </summary>
        List<View_Business_Settlement_ProcurementStatementDetail> m_listViewDetail =
            new List<View_Business_Settlement_ProcurementStatementDetail>();

        /// <summary>
        /// 合同信息数据集合
        /// </summary>
        List<Business_Settlement_ProcurementStatement_Invoice> m_listInvoice =
            new List<Business_Settlement_ProcurementStatement_Invoice>();

        /// <summary>
        /// 服务组件
        /// </summary>
        IProcurementStatement m_serviceStatement = Service_Economic_Purchase.ServerModuleFactory.GetServerModule<IProcurementStatement>();

        /// <summary>
        /// 流程服务组件
        /// </summary>
        IFlowServer m_serverFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();

        public 采购结算单明细()
        {
            InitializeComponent();
        }

        public void InitForm()
        {
            cmbBillType.DataSource = GlobalObject.GeneralFunction.GetEumnList(typeof(GlobalObject.CE_ProcurementStatementBillTypeEnum));

            List<string> lstInvoiceType = GlobalObject.GeneralFunction.GetEumnList(typeof(GlobalObject.CE_InvoiceTypeEnum));
            cmbInvoiceType.DataSource = lstInvoiceType;
        }

        private void txtSettlementCompany_OnCompleteSearch()
        {
            txtSettlementCompany.Text = txtSettlementCompany.DataResult["供应商名称"].ToString();
            txtSettlementCompany.Tag = txtSettlementCompany.DataResult["供应商编码"].ToString();
        }

        private void cmbInvoiceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbTaxRate.Items.Clear();
            if (GlobalObject.GeneralFunction.StringConvertToEnum<CE_InvoiceTypeEnum>(cmbInvoiceType.Text) == 
                CE_InvoiceTypeEnum.普通发票)
            {
                cmbTaxRate.Items.Add("17.00");
                cmbTaxRate.Items.Add("6.00");
                cmbTaxRate.Items.Add("4.00");
                cmbTaxRate.Items.Add("3.00");
                cmbTaxRate.Items.Add("0.00");
            }
            else if (GlobalObject.GeneralFunction.StringConvertToEnum<CE_InvoiceTypeEnum>(cmbInvoiceType.Text) == 
                CE_InvoiceTypeEnum.专用发票)
            {
                cmbTaxRate.Items.Add("17.00");
                cmbTaxRate.Items.Add("6.00");
                cmbTaxRate.Items.Add("3.00");
                cmbTaxRate.Items.Add("0.00");
            }
        }

        private void btnInvoice_Click(object sender, EventArgs e)
        {
            //发票信息编辑窗体 frm = new 发票信息编辑窗体(m_lnqBillInfo.BillNo, m_listInvoice);
            //frm.ShowDialog();

            //if (frm.ListInvoice != null)
            //{
            //    m_listInvoice = frm.ListInvoice;
            //    SetInoviceInfo(m_listInvoice);
            //}
        }

        void SetInoviceInfo(List<Business_Settlement_ProcurementStatement_Invoice> inoviceSource)
        {
            txtInvoiceInfo.Text = "";

            foreach (string strDate in inoviceSource.Select(k => k.InvoiceDate.ToString("yyyy-MM-dd")).ToList())
            {
                txtInvoiceInfo.Text += strDate + "     ";
            }

            txtInvoiceInfo.Text += "\r\n\r\n";


            foreach (string strInvoiceNo in inoviceSource.Select(k => k.InvoiceNo).ToList())
            {
                txtInvoiceInfo.Text += strInvoiceNo + "       ";
            }
        }

        public override void LoadFormInfo()
        {
            try
            {
                m_billNoControl = new BillNumberControl(CE_BillTypeEnum.采购结算单.ToString(), m_serviceStatement);
                m_lnqBillInfo = m_serviceStatement.GetSingleBillInfo(this.FlowInfo_BillNo);
                SetInfo();

                if (GlobalObject.GeneralFunction.StringConvertToEnum<CE_ProcurementStatementBillTypeEnum>(cmbBillType.Text) !=
                    CE_ProcurementStatementBillTypeEnum.零星采购加工)
                {
                    cmbTaxRate.Enabled = false;
                }
                else
                {
                    cmbTaxRate.Enabled = true;
                }

                if (GlobalObject.GeneralFunction.StringConvertToEnum<CE_CommonBillStatus>(lbBillStatus.Text) == CE_CommonBillStatus.等待审核)
                {
                    groupBox1.Enabled = false;

                    差异说明.ReadOnly = false;
                    发票单价.ReadOnly = false;
                }

                if (GlobalObject.GeneralFunction.StringConvertToEnum<CE_CommonBillStatus>(lbBillStatus.Text) == CE_CommonBillStatus.新建单据)
                {
                    panel1.Enabled = false;

                    差异说明.ReadOnly = false;
                    发票单价.ReadOnly = false;
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowErrorMessage(ex.Message);
            }
        }

        void SetInfo()
        {
            InitForm();

            if (m_lnqBillInfo != null)
            {
                lbBillStatus.Text = m_serverFlow.GetNowBillStatus(m_lnqBillInfo.BillNo);

                txtBillNo.Text = m_lnqBillInfo.BillNo;
                cmbBillType.Text = m_lnqBillInfo.BillType;
                cmbInvoiceType.Text = m_lnqBillInfo.InvoiceType;
                cmbTaxRate.Text = m_lnqBillInfo.TaxRate.ToString();
                txtAccoutingDocumentNo.Text = m_lnqBillInfo.AccoutingDocumentNo;

                txtSettlementCompany.Text = UniversalFunction.GetProviderInfo(m_lnqBillInfo.SettlementCompany).供应商名称;
                txtSettlementCompany.Tag = m_lnqBillInfo.SettlementCompany;
            }
            else
            {
                lbBillStatus.Text = CE_CommonBillStatus.新建单据.ToString();

                m_lnqBillInfo = new Business_Settlement_ProcurementStatement();
                txtBillNo.Text = this.FlowInfo_BillNo;
                m_lnqBillInfo.BillNo = txtBillNo.Text;
            }

            m_listViewDetail = m_serviceStatement.GetListViewDetailInfo(m_lnqBillInfo.BillNo);
            m_listInvoice = m_serviceStatement.GetListInvoiceInfo(m_lnqBillInfo.BillNo);
            RefreshDataGridView(m_listViewDetail, m_listInvoice);
        }

        void SumPrice()
        {
            decimal dcInPutPrice = 0;
            decimal dcTotalTaxPrice = 0;
            decimal dcInvoicePrice = 0;

            foreach (DataGridViewRow dgvr in customDataGridView1.Rows)
            {
                dcInPutPrice += (decimal)dgvr.Cells["入库金额"].Value;
                dcInvoicePrice += Convert.ToDecimal(dgvr.Cells["发票金额"].Value);

                if (GlobalObject.GeneralFunction.StringConvertToEnum<CE_ProcurementStatementBillTypeEnum>(cmbBillType.Text) !=
                    CE_ProcurementStatementBillTypeEnum.委托加工物资)
                {
                    dcTotalTaxPrice += Convert.ToDecimal(dgvr.Cells["价税合计"].Value);
                }
                else
                {
                    dcTotalTaxPrice += Convert.ToDecimal(dgvr.Cells["发票金额"].Value);
                }
            }

            lbInPutPrice.Text = Math.Round(dcInPutPrice, 2).ToString();
            lbTotalTaxPrice.Text = Math.Round(dcTotalTaxPrice, 2).ToString();
            lbInvoicePrice.Text = Math.Round(dcInvoicePrice, 2).ToString();
        }

        void RefreshDataGridView(List<View_Business_Settlement_ProcurementStatementDetail> detailSource, 
            List<Business_Settlement_ProcurementStatement_Invoice> inoviceSource)
        {
            if (detailSource != null)
            {
                customDataGridView1.Rows.Clear();
                foreach (View_Business_Settlement_ProcurementStatementDetail item in detailSource)
                {
                    customDataGridView1.Rows.Add(new object[] {  item.入库单号, item.零件图号, item.零件名称, item.规格, item.批次号, item.入库数量, item.入库单价, 
                        item.单件委托材料, item.单件加工费, item.入库金额, item.委托加工材料, item.发票单价, item.税额, item.价税合计, item.发票金额, 
                        item.合同申请单号, item.差异说明, item.物品ID , item.单据号});
                }

                SetInoviceInfo(inoviceSource);
            }
        }

        private void cmbBillType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (customDataGridView1.Columns.Count == 0)
            {
                return;
            }

            CE_ProcurementStatementBillTypeEnum billType = 
                GlobalObject.GeneralFunction.StringConvertToEnum<CE_ProcurementStatementBillTypeEnum>(cmbBillType.Text);

            if (billType == CE_ProcurementStatementBillTypeEnum.零星采购加工)
            {
                //cmbInvoiceType.Enabled = true;
                cmbTaxRate.Enabled = true;
            }
            else
            {
                //cmbInvoiceType.Enabled = false;
                cmbTaxRate.Enabled = false;
            }

            if (billType == CE_ProcurementStatementBillTypeEnum.委托加工物资)
            {
                customDataGridView1.Columns["入库单价_不含税"].Visible = false;
                customDataGridView1.Columns["单件委托材料"].Visible = true;
                customDataGridView1.Columns["单件加工费"].Visible = true;
                customDataGridView1.Columns["委托加工材料"].Visible = true;
                //customDataGridView1.Columns["税额"].Visible = false;
                //customDataGridView1.Columns["价税合计"].Visible = false;
            }
            else
            {

                customDataGridView1.Columns["入库单价_不含税"].Visible = true;
                customDataGridView1.Columns["单件委托材料"].Visible = false;
                customDataGridView1.Columns["单件加工费"].Visible = false;
                customDataGridView1.Columns["委托加工材料"].Visible = false;
                //customDataGridView1.Columns["税额"].Visible = true;
                //customDataGridView1.Columns["价税合计"].Visible = true;
            }
        }

        private void btnInPutBill_Click(object sender, EventArgs e)
        {
            if (txtSettlementCompany.Text.Trim().Length == 0)
            {
                MessageBox.Show("请选择结算单位");
                return;
            }

            采购结算_选择入库单 frm = new 采购结算_选择入库单(txtSettlementCompany.Tag.ToString());
            frm.ShowDialog();

            if (frm.LstBillNo != null)
            {
                DataTable dtTemp = m_serviceStatement.LeadInDetail(frm.LstBillNo,
                    GlobalObject.GeneralFunction.StringConvertToEnum<CE_ProcurementStatementBillTypeEnum>(cmbBillType.Text));

                if (dtTemp != null && dtTemp.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtTemp.Rows)
                    {
                        bool flag = false;

                        foreach (DataGridViewRow dgvr in customDataGridView1.Rows)
                        {
                            if (dgvr.Cells["物品ID"].Value.ToString() == dr["物品ID"].ToString() 
                                && dgvr.Cells["入库单号"].Value.ToString() == dr["入库单号"].ToString()
                                && dgvr.Cells["批次号"].Value.ToString() == dr["批次号"].ToString())
                            {
                                flag = true;
                                break;
                            }
                        }

                        if (!flag)
                        {
                            customDataGridView1.Rows.Add(new object[] { dr["入库单号"].ToString(), dr["零件图号"].ToString(), dr["零件名称"].ToString(), 
                                dr["规格"].ToString(), dr["批次号"].ToString(),(decimal)dr["入库数量"], (decimal)dr["入库单价"], 
                                (decimal)dr["单件委托材料"], (decimal)dr["单件加工费"], (decimal)dr["入库金额"], 
                                (decimal)dr["委托加工材料"], (decimal)dr["发票单价"], (decimal)dr["税额"], (decimal)dr["价税合计"], (decimal)dr["发票金额"], 
                                dr["合同申请单号"].ToString(), dr["差异说明"].ToString(), (int)dr["物品ID"], txtBillNo.Text });
                        }
                    }
                }
            }

            if (customDataGridView1.Rows.Count > 0)
            {
                if (GlobalObject.GeneralFunction.StringConvertToEnum<CE_ProcurementStatementBillTypeEnum>(cmbBillType.Text) != 
                    CE_ProcurementStatementBillTypeEnum.零星采购加工)
                {
                    IBargainInfoServer service_BargainInfo = ServerModule.ServerModuleFactory.GetServerModule<ServerModule.IBargainInfoServer>();
                    cmbTaxRate.Text = 
                        service_BargainInfo.GetTaxRate(customDataGridView1.Rows[0].Cells["执行合同号_申请单"].Value.ToString()).ToString();
                }
            }

            SumPrice();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dgvr in customDataGridView1.SelectedRows)
            {
                customDataGridView1.Rows.Remove(dgvr);
            }

            DataTable dtInvoice = (DataTable)customDataGridView1.DataSource;
        }

        private void customDataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 11)
            {
                if (GlobalObject.GeneralFunction.StringConvertToEnum<CE_ProcurementStatementBillTypeEnum>(cmbBillType.Text) !=
                    CE_ProcurementStatementBillTypeEnum.委托加工物资)
                {
                    customDataGridView1.CurrentRow.Cells[14].Value = Math.Round(
                        Convert.ToDecimal(customDataGridView1.CurrentRow.Cells[11].Value) * Convert.ToDecimal(customDataGridView1.CurrentRow.Cells[5].Value), 2);
                    customDataGridView1.CurrentRow.Cells[12].Value = Math.Round(
                        Convert.ToDecimal(customDataGridView1.CurrentRow.Cells[14].Value) * (Convert.ToDecimal(cmbTaxRate.Text) / 100), 2);
                    customDataGridView1.CurrentRow.Cells[13].Value =
                        Convert.ToDecimal(customDataGridView1.CurrentRow.Cells[14].Value) + Convert.ToDecimal(customDataGridView1.CurrentRow.Cells[12].Value);
                }
                else
                {
                    customDataGridView1.CurrentRow.Cells[14].Value = Math.Round(
                        Convert.ToDecimal(customDataGridView1.CurrentRow.Cells[11].Value) * Convert.ToDecimal(customDataGridView1.CurrentRow.Cells[5].Value), 2);
                    customDataGridView1.CurrentRow.Cells[12].Value = Math.Round(
                        Convert.ToDecimal(customDataGridView1.CurrentRow.Cells[14].Value) * (Convert.ToDecimal(cmbTaxRate.Text) / 100), 2);
                    customDataGridView1.CurrentRow.Cells[13].Value =
                        Convert.ToDecimal(customDataGridView1.CurrentRow.Cells[14].Value) + Convert.ToDecimal(customDataGridView1.CurrentRow.Cells[12].Value);
                    customDataGridView1.CurrentRow.Cells[10].Value = Math.Round(
                        Convert.ToDecimal(customDataGridView1.CurrentRow.Cells[11].Value) * Convert.ToDecimal(customDataGridView1.CurrentRow.Cells[5].Value) *
                        (1 + Convert.ToDecimal(cmbTaxRate.Text) / 100), 2);
                }
            }

            SumPrice();
        }

        bool CheckData_Submit()
        {
            if (txtSettlementCompany.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请选择结算单位");
                return false;
            }

            if (txtInvoiceInfo.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请填写发票信息");
                return false;
            }

            if (cmbBillType.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请选择业务类型");
                return false;
            }

            if (cmbInvoiceType.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请选择发票类型");
                return false;
            }

            if (cmbTaxRate.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请选择税率");
                return false;
            }

            if (customDataGridView1.Rows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请添加明细");
                return false;
            }

            if (GlobalObject.GeneralFunction.StringConvertToEnum<CE_CommonBillStatus>(lbBillStatus.Text) 
                == CE_CommonBillStatus.等待审核 && txtAccoutingDocumentNo.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请填写凭证号");
                return false;
            }

            return true;
        }

        bool CheckData_Hold()
        {
            if (txtSettlementCompany.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请选择结算单位");
                return false;
            }

            if (cmbTaxRate.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请选择税率");
                return false;
            }

            if (customDataGridView1.Rows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请添加明细");
                return false;
            }

            return true;
        }

        private bool customPanel1_PanelGetDataInfo(CE_FlowOperationType flowOperationType)
        {
            try
            {
                if (flowOperationType == CE_FlowOperationType.提交)
                {
                    if (!CheckData_Submit())
                    {
                        return false;
                    }

                    if (GlobalObject.GeneralFunction.StringConvertToEnum<CE_CommonBillStatus>(lbBillStatus.Text) == CE_CommonBillStatus.等待审核)
                    {
                        CE_ProcurementStatementBillTypeEnum billType = 
                            GlobalObject.GeneralFunction.StringConvertToEnum<CE_ProcurementStatementBillTypeEnum>(cmbBillType.Text);

                        foreach (DataGridViewRow dgvr in customDataGridView1.Rows)
                        {
                            if (billType != CE_ProcurementStatementBillTypeEnum.委托加工物资)
                            {
                                if (Convert.ToDecimal( dgvr.Cells["入库金额"].Value) != Convert.ToDecimal(dgvr.Cells["发票金额"].Value) 
                                    && dgvr.Cells["差异说明"].Value.ToString().Trim().Length == 0)
                                {
                                    MessageDialog.ShowPromptMessage("入库金额与发票金额不一致的记录的差异说明不能为空");
                                    return false;
                                }
                            }
                            else
                            {
                                decimal dcTemp = Convert.ToDecimal(dgvr.Cells["入库数量"].Value) * Convert.ToDecimal( dgvr.Cells["单件委托材料"].Value);

                                if (Convert.ToDecimal( dgvr.Cells["入库金额"].Value) != (Convert.ToDecimal(dgvr.Cells["发票金额"].Value) + dcTemp)
                                    && dgvr.Cells["差异说明"].Value.ToString().Trim().Length == 0)
                                {
                                    MessageDialog.ShowPromptMessage("入库金额与(发票金额 + 入库数量 * 单件委托材料) 不一致的记录的差异说明不能为空");
                                    return false;
                                }
                            }
                        }
                    }
                }
                else if(flowOperationType == CE_FlowOperationType.暂存)
                {
                    if (!CheckData_Hold())
                    {
                        return false;
                    }
                }

                m_lnqBillInfo = new Business_Settlement_ProcurementStatement();

                m_lnqBillInfo.BillNo = txtBillNo.Text;
                m_lnqBillInfo.BillType = cmbBillType.Text;
                m_lnqBillInfo.InvoiceType = cmbInvoiceType.Text;
                m_lnqBillInfo.SettlementCompany = txtSettlementCompany.Tag.ToString();
                m_lnqBillInfo.TaxRate = Convert.ToDecimal( cmbTaxRate.Text);
                m_lnqBillInfo.AccoutingDocumentNo = txtAccoutingDocumentNo.Text;

                List<View_Business_Settlement_ProcurementStatementDetail> listTemp = new List<View_Business_Settlement_ProcurementStatementDetail>();

                foreach (DataGridViewRow dgvr in customDataGridView1.Rows)
                {
                    View_Business_Settlement_ProcurementStatementDetail detailTemp = new View_Business_Settlement_ProcurementStatementDetail();

                    detailTemp.差异说明 = dgvr.Cells["差异说明"].Value == null ? "" : dgvr.Cells["差异说明"].Value.ToString();
                    detailTemp.零件名称 = dgvr.Cells["零件名称"].Value == null ? "" : dgvr.Cells["零件名称"].Value.ToString();
                    detailTemp.物品ID = Convert.ToInt32(dgvr.Cells["物品ID"].Value);
                    detailTemp.零件图号 = dgvr.Cells["零件图号"].Value == null ? "" : dgvr.Cells["零件图号"].Value.ToString();
                    detailTemp.规格 = dgvr.Cells["规格"].Value == null ? "" : dgvr.Cells["规格"].Value.ToString();
                    detailTemp.单据号 = txtBillNo.Text;
                    detailTemp.批次号 = dgvr.Cells["批次号"].Value == null ? "" : dgvr.Cells["批次号"].Value.ToString();
                    detailTemp.单件加工费 = (decimal?)dgvr.Cells["单件加工费"].Value;
                    detailTemp.单件委托材料 = (decimal?)dgvr.Cells["单件委托材料"].Value;
                    detailTemp.发票单价 = Convert.ToDecimal(dgvr.Cells["发票单价"].Value);
                    detailTemp.发票金额 = Convert.ToDecimal(dgvr.Cells["发票金额"].Value);
                    detailTemp.合同申请单号 = dgvr.Cells["执行合同号_申请单"].Value == null ? "" : dgvr.Cells["执行合同号_申请单"].Value.ToString();
                    detailTemp.价税合计 = (decimal?)dgvr.Cells["价税合计"].Value;
                    detailTemp.入库单号 = dgvr.Cells["入库单号"].Value == null ? "" : dgvr.Cells["入库单号"].Value.ToString();
                    detailTemp.入库单价 = (decimal)dgvr.Cells["入库单价_不含税"].Value;
                    detailTemp.入库金额 = (decimal)dgvr.Cells["入库金额"].Value;
                    detailTemp.入库数量 = (decimal)dgvr.Cells["入库数量"].Value;
                    detailTemp.税额 = (decimal)dgvr.Cells["税额"].Value;
                    detailTemp.委托加工材料 = (decimal)dgvr.Cells["委托加工材料"].Value;

                    listTemp.Add(detailTemp);
                }

                this.FlowInfo_BillNo = txtBillNo.Text;
                this.ResultInfo = m_lnqBillInfo;

                this.ResultList = new List<object>();

                this.ResultList.Add(m_listInvoice);
                this.ResultList.Add(listTemp);
                this.ResultList.Add(flowOperationType);
                this.KeyWords = "【供应商】:" + m_lnqBillInfo.SettlementCompany;

                return true;
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
                return false;
            }
        }

        private void customDataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (txtSettlementCompany.Text.Trim().Length != 0)
            {
                txtSettlementCompany.Enabled = customDataGridView1.Rows.Count == 0 ? true : false;
            }

            SumPrice();
        }

        private void customDataGridView1_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (txtSettlementCompany.Text.Trim().Length != 0)
            {
                txtSettlementCompany.Enabled = customDataGridView1.Rows.Count == 0 ? true : false;
            }

            SumPrice();
        }
    }
}
