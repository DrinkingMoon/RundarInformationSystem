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
    public partial class 供应商应付账款明细 : CustomFlowForm
    {
        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 单据号
        /// </summary>
        private Bus_PurchasingMG_AccountBill _Lnq_BillInfo = new Bus_PurchasingMG_AccountBill();

        /// <summary>
        /// 明细视图数据集合
        /// </summary>
        List<View_Bus_PurchasingMG_AccountBill_Detail> _List_ViewDetail = new List<View_Bus_PurchasingMG_AccountBill_Detail>();

        /// <summary>
        /// 合同信息数据集合
        /// </summary>
        List<Bus_PurchasingMG_AccountBill_Invoice> _List_Invoice = new List<Bus_PurchasingMG_AccountBill_Invoice>();

        /// <summary>
        /// 服务组件
        /// </summary>
        IAccountOperation _Service_Account = Service_Economic_Purchase.ServerModuleFactory.GetServerModule<IAccountOperation>();

        /// <summary>
        /// 流程服务组件
        /// </summary>
        IFlowServer _Service_Flow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();

        public 供应商应付账款明细()
        {
            InitializeComponent();
        }

        private void txtSettlementCompany_OnCompleteSearch()
        {
            if (txtSettlementCompany.DataResult == null)
            {
                return;
            }

            txtSettlementCompany.Text = txtSettlementCompany.DataResult["供应商名称"].ToString();
            txtSettlementCompany.Tag = txtSettlementCompany.DataResult["供应商编码"].ToString();
        }

        private void btnInvoice_Click(object sender, EventArgs e)
        {
            发票信息编辑窗体 frm = new 发票信息编辑窗体(_Lnq_BillInfo.BillNo, _List_Invoice);
            frm.ShowDialog();

            if (frm.ListInvoice != null)
            {
                _List_Invoice = frm.ListInvoice;
                SetInoviceInfo(_List_Invoice);
            }
        }

        void SetInoviceInfo(List<Bus_PurchasingMG_AccountBill_Invoice> inoviceSource)
        {
            txtInvoiceInfo.Text = "";

            foreach (string strDate in inoviceSource.Select(k => ((DateTime)k.InvoiceTime).ToString("yyyy-MM-dd")).ToList())
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
                m_billNoControl = new BillNumberControl(CE_BillTypeEnum.供应商应付账款.ToString(), _Service_Account);
                _Lnq_BillInfo = _Service_Account.GetSingleBillInfo(this.FlowInfo_BillNo);
                SetInfo();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowErrorMessage(ex.Message);
            }
        }

        void SetInfo()
        {
            if (_Lnq_BillInfo != null)
            {
                lbBillStatus.Text = _Service_Flow.GetNowBillStatus(_Lnq_BillInfo.BillNo);

                txtBillNo.Text = _Lnq_BillInfo.BillNo;
                txtVoucherNo.Text = _Lnq_BillInfo.VoucherNo;
                dtpFinanceTime.Value = Convert.ToDateTime( _Lnq_BillInfo.FinanceTime);

                txtSettlementCompany.Text = UniversalFunction.GetProviderInfo(_Lnq_BillInfo.Provider).供应商名称;
                txtSettlementCompany.Tag = _Lnq_BillInfo.Provider;
            }
            else
            {
                lbBillStatus.Text = CE_CommonBillStatus.新建单据.ToString();

                _Lnq_BillInfo = new Bus_PurchasingMG_AccountBill();
                txtBillNo.Text = this.FlowInfo_BillNo;
                _Lnq_BillInfo.BillNo = txtBillNo.Text;
                dtpFinanceTime.Value = ServerTime.Time;
            }

            _List_ViewDetail = _Service_Account.GetListViewDetailInfo(_Lnq_BillInfo.BillNo);
            _List_Invoice = _Service_Account.GetListInvoiceInfo(_Lnq_BillInfo.BillNo);
            RefreshDataGridView(_List_ViewDetail, _List_Invoice);
        }

        void SumPrice()
        {
            decimal dcInPutPrice = 0;
            decimal dcInvoicePrice = 0;

            foreach (DataGridViewRow dgvr in customDataGridView1.Rows)
            {
                dcInPutPrice += Convert.ToDecimal(dgvr.Cells["应付金额"].Value);
                dcInvoicePrice += Convert.ToDecimal(dgvr.Cells["发票金额"].Value);
            }

            lbInPutPrice.Text = Math.Round(dcInPutPrice, 2).ToString();
            lbInvoicePrice.Text = Math.Round(dcInvoicePrice, 2).ToString();
        }

        void RefreshDataGridView(List<View_Bus_PurchasingMG_AccountBill_Detail> detailSource,
            List<Bus_PurchasingMG_AccountBill_Invoice> inoviceSource)
        {
            if (detailSource != null)
            {
                customDataGridView1.Rows.Clear();
                foreach (View_Bus_PurchasingMG_AccountBill_Detail item in detailSource)
                {
                    customDataGridView1.Rows.Add(new object[] {  item.图号型号, item.物品名称, item.规格, item.挂账年月, item.协议单价, item.税率, item.单位, item.应付数量, item.实付数量, 
                        item.应付金额, item.发票金额, item.备注, item.单据号, item.GoodsID});
                }

                SetInoviceInfo(inoviceSource);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dgvr in customDataGridView1.SelectedRows)
            {
                customDataGridView1.Rows.Remove(dgvr);
            }

            DataTable dtInvoice = (DataTable)customDataGridView1.DataSource;
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

        bool CheckData_Submit()
        {
            if (!CheckData_Hold())
            {
                return false;
            }

            if (customDataGridView1.Rows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请添加明细");
                return false;
            }

            if (GlobalObject.GeneralFunction.StringConvertToEnum<CE_CommonBillStatus>(lbBillStatus.Text)
                == CE_CommonBillStatus.等待审核 && txtVoucherNo.Text.Trim().Length == 0)
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

            if (txtInvoiceInfo.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请填写发票信息");
                return false;
            }

            return true;
        }

        private bool 供应商应付账款明细_PanelGetDataInfo(CE_FlowOperationType flowOperationType)
        {
            try
            {
                if (flowOperationType == CE_FlowOperationType.提交)
                {
                    if (!CheckData_Submit())
                    {
                        return false;
                    }

                    if (dtpFinanceTime.Value.Day > 26)
                    {
                        if (MessageDialog.ShowEnquiryMessage("【账单日期】大于26日，点击【是】将提交单据，点击【否】将退出？") == DialogResult.No)
                        {
                            return false;
                        }
                    }
                }
                else if (flowOperationType == CE_FlowOperationType.暂存)
                {
                    if (!CheckData_Hold())
                    {
                        return false;
                    }
                }

                _Lnq_BillInfo = new Bus_PurchasingMG_AccountBill();

                _Lnq_BillInfo.BillNo = txtBillNo.Text;
                _Lnq_BillInfo.Provider = txtSettlementCompany.Tag.ToString();
                _Lnq_BillInfo.VoucherNo = txtVoucherNo.Text;
                _Lnq_BillInfo.FinanceTime = dtpFinanceTime.Value;

                List<View_Bus_PurchasingMG_AccountBill_Detail> listTemp = new List<View_Bus_PurchasingMG_AccountBill_Detail>();

                foreach (DataGridViewRow dgvr in customDataGridView1.Rows)
                {
                    View_Bus_PurchasingMG_AccountBill_Detail detailTemp = new View_Bus_PurchasingMG_AccountBill_Detail();

                    detailTemp.图号型号 = dgvr.Cells["图号型号"].Value == null ? "" : dgvr.Cells["图号型号"].Value.ToString();
                    detailTemp.物品名称 = dgvr.Cells["物品名称"].Value == null ? "" : dgvr.Cells["物品名称"].Value.ToString();
                    detailTemp.规格 = dgvr.Cells["规格"].Value == null ? "" : dgvr.Cells["规格"].Value.ToString();
                    detailTemp.GoodsID = Convert.ToInt32(dgvr.Cells["GoodsID"].Value);
                    detailTemp.单据号 = txtBillNo.Text;
                    detailTemp.发票金额 = Convert.ToDecimal(dgvr.Cells["发票金额"].Value);
                    detailTemp.应付金额 = Convert.ToDecimal(dgvr.Cells["应付金额"].Value);
                    detailTemp.应付数量 = Convert.ToDecimal(dgvr.Cells["应付数量"].Value);
                    detailTemp.实付数量 = Convert.ToDecimal(dgvr.Cells["实付数量"].Value);
                    detailTemp.协议单价 = Convert.ToDecimal(dgvr.Cells["协议单价"].Value);
                    detailTemp.税率 = Convert.ToInt32(dgvr.Cells["税率"].Value);
                    detailTemp.备注 = dgvr.Cells["备注"].Value.ToString();
                    detailTemp.挂账年月 = dgvr.Cells["挂账年月"].Value.ToString();

                    listTemp.Add(detailTemp);
                }

                this.FlowInfo_BillNo = txtBillNo.Text;
                this.ResultInfo = _Lnq_BillInfo;

                this.ResultList = new List<object>();

                this.ResultList.Add(_List_Invoice);
                this.ResultList.Add(listTemp);
                this.ResultList.Add(flowOperationType);
                this.KeyWords = "【供应商】:" + _Lnq_BillInfo.Provider;

                return true;
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
                return false;
            }
        }

        private void customDataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 8)
            {
                decimal unitPrice = Convert.ToDecimal(customDataGridView1.CurrentRow.Cells[4].Value);
                decimal taxRate = Convert.ToDecimal(customDataGridView1.CurrentRow.Cells[5].Value);
                decimal ygCount = Convert.ToDecimal(customDataGridView1.CurrentRow.Cells[7].Value);
                decimal sgCount = Convert.ToDecimal(customDataGridView1.CurrentRow.Cells[8].Value);

                if ((ygCount > 0 && (sgCount > ygCount || sgCount < 0))
                    || (ygCount < 0 && (sgCount < ygCount || sgCount > 0)))
                {
                    sgCount = ygCount;
                }

                decimal resultDecimal = Math.Round(unitPrice / (1 + (taxRate / 100)) * sgCount, 2);

                customDataGridView1.CurrentRow.Cells[8].Value = sgCount;
                customDataGridView1.CurrentRow.Cells[9].Value = resultDecimal;
                customDataGridView1.CurrentRow.Cells[10].Value = resultDecimal;
            }

            SumPrice();
        }

        private void btnInputAccountInfo_Click(object sender, EventArgs e)
        {
            if (txtSettlementCompany.Text.Trim().Length == 0)
            {
                MessageBox.Show("请选择【结算单位】");
                return;
            }

            DataTable dtTemp = _Service_Account.LeadInputAccount(txtSettlementCompany.Tag.ToString());

            DataTable tempTable = new DataTable();

            List<string> lstKeys = new List<string>();

            lstKeys.Add("挂账年月");
            lstKeys.Add("物品ID");

            foreach (string keysName in lstKeys)
            {
                tempTable.Columns.Add(keysName);
            }

            foreach (DataGridViewRow dgvr in customDataGridView1.Rows)
            {
                DataRow drNew = tempTable.NewRow();

                drNew["挂账年月"] = dgvr.Cells["挂账年月"].Value.ToString();
                drNew["物品ID"] = Convert.ToInt32(dgvr.Cells["GoodsID"].Value);

                tempTable.Rows.Add(drNew);
            }

            FormDataTableCheck frm = new FormDataTableCheck(dtTemp, tempTable, lstKeys);

            if (frm.ShowDialog() == DialogResult.OK)
            {
                if (frm._DtResult == null)
                {
                    return;
                }

                foreach (DataRow dr in frm._DtResult.Rows)
                {
                    bool flag = false;

                    foreach (DataGridViewRow dgvr in customDataGridView1.Rows)
                    {
                        if (dgvr.Cells["GoodsID"].Value.ToString() == dr["物品ID"].ToString()
                            && dgvr.Cells["挂账年月"].Value.ToString() == dr["挂账年月"].ToString())
                        {
                            flag = true;
                            break;
                        }
                    }

                    if (!flag)
                    {
                        decimal price = Math.Round((decimal)dr["协议单价"] / (1 + Convert.ToDecimal( dr["税率"]) / 100) * (decimal)dr["应挂数量"], 2);

                        View_F_GoodsPlanCost goodsInfo = UniversalFunction.GetGoodsInfo((int)dr["物品ID"]);

                        customDataGridView1.Rows.Add(new object[] { dr["图号型号"].ToString(), dr["物品名称"].ToString(), dr["规格"].ToString(), 
                                dr["挂账年月"].ToString(), (decimal)dr["协议单价"], (int)dr["税率"], goodsInfo.单位, (decimal)dr["应挂数量"], 
                                (decimal)dr["应挂数量"], price, price, "", txtBillNo.Text, (int)dr["物品ID"]});
                    }
                }
            }

            SumPrice();
        }
    }
}
