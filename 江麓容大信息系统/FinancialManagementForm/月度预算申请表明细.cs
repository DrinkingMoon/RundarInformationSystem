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
using FlowControlService;
using Service_Economic_Financial;

namespace Form_Economic_Financial
{
    public partial class 月度预算申请表明细 : CustomFlowForm
    {
        IBudgetYear _ServiceYear = Service_Economic_Financial.ServerModuleFactory.GetServerModule<IBudgetYear>();
        IFlowServer _ServiceFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();
        IBudgetMonth _ServiceMonth = Service_Economic_Financial.ServerModuleFactory.GetServerModule<IBudgetMonth>();

        Business_Finance_Budget_Month _LnqBillInfo = new Business_Finance_Budget_Month();

        public 月度预算申请表明细()
        {
            InitializeComponent();
        }

        public override void LoadFormInfo()
        {
            try
            {
                txtBillNo.Text = this.FlowInfo_BillNo;
                lbBillStatus.Text = _ServiceFlow.GetNowBillStatus(this.FlowInfo_BillNo);

                cmbYearValue.MaxYear = 2025;
                cmbYearValue.MinYear = 2015;
                cmbYearValue.Init();
                
                cmbMonthValue.Init<CE_MonthValue>();

                _LnqBillInfo = _ServiceMonth.GetBillSingleInfo(this.FlowInfo_BillNo);

                if (_LnqBillInfo == null)
                {
                    _LnqBillInfo = new Business_Finance_Budget_Month();

                    _LnqBillInfo.MonthValue = ServerTime.Time.Month;
                    _LnqBillInfo.YearValue = ServerTime.Time.Year;
                    _LnqBillInfo.BillNo = txtBillNo.Text;
                    _LnqBillInfo.DeptCode = UniversalFunction.GetDept_Belonge(BasicInfo.DeptCode).DeptCode;
                }

                cmbYearValue.Text = _LnqBillInfo.YearValue.ToString();
                cmbMonthValue.Text = GlobalObject.GeneralFunction.ValueConvertToEnum<CE_MonthValue>(Convert.ToInt32(_LnqBillInfo.MonthValue)).ToString();

                RefreshDataGridView();

                cmbYearValue.SelectedIndexChanged += new EventHandler(cmbYearValue_SelectedIndexChanged);
                cmbMonthValue.SelectedIndexChanged += new EventHandler(cmbMonthValue_SelectedIndexChanged);

                if (lbBillStatus.Text != CE_CommonBillStatus.新建单据.ToString())
                {
                    cmbMonthValue.Enabled = false;
                    cmbYearValue.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        void RefreshDataGridView()
        {
            DataTable source = _ServiceMonth.GetDetailInfo(_LnqBillInfo);

            if (source != null)
            {
                customDataGridView1.Rows.Clear();

                foreach (DataRow dr in source.Rows)
                {
                    customDataGridView1.Rows.Add(new object[] { 
                        dr["父级科目"].ToString(), dr["预算科目"].ToString(), Convert.ToDecimal(dr["年度预算"]),
                        Convert.ToDecimal(dr["月度预算"]),dr["差异说明(年)"], 
                        Convert.ToDecimal(dr["实际金额"]),dr["差异说明(月)"], dr["科目ID"].ToString()});
                }

                foreach (DataGridViewRow dgvr in customDataGridView1.Rows)
                {
                    decimal yeardec = Convert.ToDecimal(dgvr.Cells["年度预算"].Value);
                    decimal monthdec = Convert.ToDecimal(dgvr.Cells["月度预算"].Value);
                    decimal actarldec = Convert.ToDecimal(dgvr.Cells["实际金额"].Value);

                    if (monthdec > 0 && monthdec >= yeardec * (decimal)1.2)
                    {
                        dgvr.Cells["月度预算"].Style.BackColor = Color.Red;
                    }

                    if (actarldec > 0 && actarldec >= monthdec * (decimal)1.2)
                    {
                        dgvr.Cells["实际金额"].Style.BackColor = Color.Red;
                    }
                }
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            DataTable dtTemp =
                _ServiceYear.GetSynthesizeBudgetInfo((int)_LnqBillInfo.YearValue, _LnqBillInfo.DeptCode);

            FormDataShow frmShow = new FormDataShow(dtTemp);
            frmShow.Show();
        }

        private void cmbYearValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbYearValue.Text.Trim().Length == 0)
            {
                return;
            }

            _LnqBillInfo.YearValue = Convert.ToInt32(cmbYearValue.Text);
            RefreshDataGridView();
        }

        private void cmbMonthValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMonthValue.Text.Trim().Length == 0)
            {
                return;
            }

            _LnqBillInfo.MonthValue = (int)GlobalObject.GeneralFunction.StringConvertToEnum<CE_MonthValue>(cmbMonthValue.Text);
            RefreshDataGridView();
        }

        private bool 月度预算申请表明细_PanelGetDataInfo(CE_FlowOperationType flowOperationType)
        {
            try
            {
                if (cmbYearValue.Text.Trim().Length == 0)
                {
                    throw new Exception("请填写【预算年份】");
                }

                if (cmbMonthValue.Text.Trim().Length == 0)
                {
                    throw new Exception("请填写【预算月份】");
                }

                if (customDataGridView1.Rows.Count == 0)
                {
                    throw new Exception("请录入【明细】信息");
                }

                this.ResultInfo = _LnqBillInfo;
                this.FlowOperationType = flowOperationType;

                this.ResultList.Add(GetDetail());
                return true;
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
                return false;
            }
        }

        DataTable GetDetail()
        {
            DataTable result = _ServiceMonth.GetDetailInfo(_LnqBillInfo).Clone();

            if (customDataGridView1.Rows.Count > 0)
            {
                foreach (DataGridViewRow dgvr in customDataGridView1.Rows)
                {
                    if (GlobalObject.GeneralFunction.IsNullOrEmpty(dgvr.Cells["父级科目"].Value.ToString()) 
                        || dgvr.Cells["科目ID"].Value == null 
                        || dgvr.Cells["科目ID"].Value.ToString().Trim().Length == 0)
                    {
                        continue;
                    }

                    DataRow dr = result.NewRow();

                    dr["父级科目"] = dgvr.Cells["父级科目"].Value;
                    dr["预算科目"] = dgvr.Cells["预算科目"].Value;
                    dr["年度预算"] = dgvr.Cells["年度预算"].Value;
                    dr["月度预算"] = dgvr.Cells["月度预算"].Value;
                    dr["差异说明(年)"] = dgvr.Cells["差异说明年"].Value;
                    dr["实际金额"] = dgvr.Cells["实际金额"].Value;
                    dr["差异说明(月)"] = dgvr.Cells["差异说明月"].Value;
                    dr["科目ID"] = dgvr.Cells["科目ID"].Value;

                    result.Rows.Add(dr);
                }
            }

            return result;
        }

        private void customDataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell dgvCell = customDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];

            if (dgvCell is DataGridViewNumericUpDownCell)
            {
                decimal columnSum = 0;
                decimal allSum = 0;

                foreach (DataGridViewRow dgvr in customDataGridView1.Rows)
                {
                    if (dgvr.Cells["预算科目"].Value.ToString() != "合计")
                    {
                        columnSum += dgvr.Cells["月度预算"].Value == null ?
                            0 : Convert.ToDecimal(dgvr.Cells["月度预算"].Value);

                        allSum += dgvr.Cells["实际金额"].Value == null ?
                            0 : Convert.ToDecimal(dgvr.Cells["实际金额"].Value);
                    }

                    customDataGridView1.Rows[customDataGridView1.Rows.Count - 1].Cells["月度预算"].Value = columnSum;
                    customDataGridView1.Rows[customDataGridView1.Rows.Count - 1].Cells["实际金额"].Value = allSum;
                }

                if (customDataGridView1.Columns[e.ColumnIndex].Name == "月度预算")
                {
                    decimal yeardec = Convert.ToDecimal(customDataGridView1.Rows[e.RowIndex].Cells["年度预算"].Value);
                    decimal monthdec = Convert.ToDecimal(customDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);

                    if (monthdec > 0 && monthdec >= yeardec * (decimal)1.2)
                    {
                        dgvCell.Style.BackColor = Color.Red;
                    }
                }
                else if (customDataGridView1.Columns[e.ColumnIndex].Name == "实际金额")
                {
                    decimal monthdec = Convert.ToDecimal(customDataGridView1.Rows[e.RowIndex].Cells["月度预算"].Value);
                    decimal actarldec = Convert.ToDecimal(customDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);

                    if (actarldec > 0 && actarldec >= monthdec * (decimal)1.2)
                    {
                        dgvCell.Style.BackColor = Color.Red;
                    }
                }
            }
        }
    }
}
