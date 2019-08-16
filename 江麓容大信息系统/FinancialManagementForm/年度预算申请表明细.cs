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
    public partial class 年度预算申请表明细 : CustomFlowForm
    {
        IFlowServer _ServiceFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();
        IBudgetYear _ServiceYear = Service_Economic_Financial.ServerModuleFactory.GetServerModule<IBudgetYear>();
        IBasicParametersSetting _ServiceBasic = Service_Economic_Financial.ServerModuleFactory.GetServerModule<IBasicParametersSetting>();

        public 年度预算申请表明细()
        {
            InitializeComponent();
            customContextMenuStrip_Edit1.IsAddFirstRow = true;
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

                this.预算科目.m_OnCompleteSearch += new DelegateCollection.NonArgumentHandle(预算科目_m_OnCompleteSearch);

                Business_Finance_Budget_Year billInfo = _ServiceYear.GetBillSingleInfo(this.FlowInfo_BillNo);

                if (billInfo != null)
                {
                    cmbYearValue.Text = billInfo.YearValue.ToString();
                }

                DataTable sourceTable = _ServiceYear.GetDetailInfo(this.FlowInfo_BillNo);
                RefreshDataGridView(sourceTable);

            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        void RefreshDataGridView(DataTable source)
        {
            if (source != null)
            {
                customDataGridView1.Rows.Clear();

                foreach (DataRow dr in source.Rows)
                {
                    customDataGridView1.Rows.Add(new object[] { 
                        dr["父级科目"].ToString(), dr["预算科目"].ToString(), Convert.ToDecimal(dr["1月"]),
                        Convert.ToDecimal(dr["2月"]), Convert.ToDecimal(dr["3月"]), Convert.ToDecimal(dr["4月"]),
                        Convert.ToDecimal(dr["5月"]), Convert.ToDecimal(dr["6月"]), Convert.ToDecimal(dr["7月"]),
                        Convert.ToDecimal(dr["8月"]), Convert.ToDecimal(dr["9月"]), Convert.ToDecimal(dr["10月"]), 
                        Convert.ToDecimal(dr["11月"]), Convert.ToDecimal(dr["12月"]), 
                        (dr["合计"] == null || dr["合计"].ToString().Trim().Length == 0 ) ? 0 : Convert.ToDecimal(dr["合计"]), 
                        dr["科目ID"].ToString()});
                }

                foreach (DataGridViewColumn col in customDataGridView1.Columns)
                {
                    if (col is DataGridViewNumericUpDownColumn)
                    {
                        foreach (DataGridViewRow dr in customDataGridView1.Rows)
                        {
                            if (dr.Cells["预算科目"].Value.ToString() == "合计")
                            {
                                customDataGridView1.Rows[dr.Index].Cells[col.Index].ReadOnly = true;
                            }
                        }
                    }
                }

                GetSumPrice();
            }
        }

        void 预算科目_m_OnCompleteSearch()
        {
            DataRow dataResult = this.预算科目.DataResult;

            if (dataResult != null)
            {
                foreach (DataGridViewRow dr in customDataGridView1.Rows)
                {
                    if (dr.Cells["科目ID"].Value == null)
                    {
                        continue;
                    }

                    if (dr.Cells["科目ID"].Value.ToString() == dataResult["科目代码"].ToString())
                    {
                        MessageDialog.ShowPromptMessage("存在相同项，不能添加重复零件");
                        customDataGridView1.CurrentRow.Cells["父级科目"].Value = "";
                        customDataGridView1.CurrentRow.Cells["预算科目"].Value = "";
                        customDataGridView1.CurrentRow.Cells["科目ID"].Value = "";
                        return;
                    }
                }

                customDataGridView1.CurrentRow.Cells["父级科目"].Value = dataResult["父级科目"];
                customDataGridView1.CurrentRow.Cells["预算科目"].Value = dataResult["科目名称"];
                customDataGridView1.CurrentRow.Cells["科目ID"].Value = dataResult["科目代码"];
            }
        }

        DataTable GetDetail()
        {
            DataTable result = _ServiceYear.GetDetailInfo(this.FlowInfo_BillNo).Clone();

            if (customDataGridView1.Rows.Count > 0)
            {
                foreach (DataGridViewRow dgvr in customDataGridView1.Rows)
                {
                    if (dgvr.Cells["科目ID"].Value == null || dgvr.Cells["科目ID"].Value.ToString().Trim().Length == 0)
                    {
                        continue;
                    }

                    DataRow dr = result.NewRow();

                    dr["父级科目"] = dgvr.Cells["父级科目"].Value;
                    dr["预算科目"] = dgvr.Cells["预算科目"].Value;
                    dr["1月"] = dgvr.Cells["一月"].Value;
                    dr["2月"] = dgvr.Cells["二月"].Value;
                    dr["3月"] = dgvr.Cells["三月"].Value;
                    dr["4月"] = dgvr.Cells["四月"].Value;
                    dr["5月"] = dgvr.Cells["五月"].Value;
                    dr["6月"] = dgvr.Cells["六月"].Value;
                    dr["7月"] = dgvr.Cells["七月"].Value;
                    dr["8月"] = dgvr.Cells["八月"].Value;
                    dr["9月"] = dgvr.Cells["九月"].Value;
                    dr["10月"] = dgvr.Cells["十月"].Value;
                    dr["11月"] = dgvr.Cells["十一月"].Value;
                    dr["12月"] = dgvr.Cells["十二月"].Value;
                    dr["合计"] = dgvr.Cells["合计"].Value == null ? 0 : dgvr.Cells["合计"].Value;
                    dr["科目ID"] = dgvr.Cells["科目ID"].Value;

                    result.Rows.Add(dr);
                }
            }

            return result;
        }

        private bool 年度预算申请表明细_PanelGetDataInfo(CE_FlowOperationType flowOperationType)
        {
            try
            {
                if (cmbYearValue.Text.Trim().Length == 0)
                {
                    throw new Exception("请填写【预算年份】");
                }

                if (customDataGridView1.Rows.Count == 0)
                {
                    throw new Exception("请录入【明细】信息");
                }

                Business_Finance_Budget_Year yearInfo = new Business_Finance_Budget_Year();

                yearInfo.BillNo = txtBillNo.Text;
                yearInfo.YearValue = Convert.ToInt32(cmbYearValue.Text);

                this.ResultInfo = yearInfo;
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

        private void customContextMenuStrip_Edit1__InputEvent(DataTable dtTemp)
        {
            DataTable dtResult = dtTemp.Clone();

            foreach (DataRow dr in dtTemp.Rows)
            {
                if (dr["科目ID"] == null || GlobalObject.GeneralFunction.IsNullOrEmpty(dr["科目ID"].ToString()))
                {
                    Business_Base_Finance_Budget_ProjectItem budget = _ServiceBasic.GetBudgetProjectInfo(dr["父级科目"] == null ? null : dr["父级科目"].ToString(),
                        dr["预算科目"] == null ? null : dr["预算科目"].ToString());

                    if (budget != null && !GlobalObject.GeneralFunction.IsNullOrEmpty(budget.ProjectID))
                    {
                        dr["科目ID"] = budget.ProjectID;
                    }
                }

                if (dr["科目ID"] != null && !GlobalObject.GeneralFunction.IsNullOrEmpty(dr["科目ID"].ToString()))
                {
                    DataRow drNew = dtResult.NewRow();

                    drNew["父级科目"] = dr["父级科目"];
                    drNew["预算科目"] = dr["预算科目"];
                    drNew["1月"] = dr["1月"] == null || GlobalObject.GeneralFunction.IsNullOrEmpty(dr["1月"].ToString()) ? 0 : Convert.ToDecimal(dr["1月"]);
                    drNew["2月"] = dr["2月"] == null || GlobalObject.GeneralFunction.IsNullOrEmpty(dr["2月"].ToString()) ? 0 : Convert.ToDecimal(dr["2月"]);
                    drNew["3月"] = dr["3月"] == null || GlobalObject.GeneralFunction.IsNullOrEmpty(dr["3月"].ToString()) ? 0 : Convert.ToDecimal(dr["3月"]);
                    drNew["4月"] = dr["4月"] == null || GlobalObject.GeneralFunction.IsNullOrEmpty(dr["4月"].ToString()) ? 0 : Convert.ToDecimal(dr["4月"]);
                    drNew["5月"] = dr["5月"] == null || GlobalObject.GeneralFunction.IsNullOrEmpty(dr["5月"].ToString()) ? 0 : Convert.ToDecimal(dr["5月"]);
                    drNew["6月"] = dr["6月"] == null || GlobalObject.GeneralFunction.IsNullOrEmpty(dr["6月"].ToString()) ? 0 : Convert.ToDecimal(dr["6月"]);
                    drNew["7月"] = dr["7月"] == null || GlobalObject.GeneralFunction.IsNullOrEmpty(dr["7月"].ToString()) ? 0 : Convert.ToDecimal(dr["7月"]);
                    drNew["8月"] = dr["8月"] == null || GlobalObject.GeneralFunction.IsNullOrEmpty(dr["8月"].ToString()) ? 0 : Convert.ToDecimal(dr["8月"]);
                    drNew["9月"] = dr["9月"] == null || GlobalObject.GeneralFunction.IsNullOrEmpty(dr["9月"].ToString()) ? 0 : Convert.ToDecimal(dr["9月"]);
                    drNew["10月"] = dr["10月"] == null || GlobalObject.GeneralFunction.IsNullOrEmpty(dr["10月"].ToString()) ? 0 : Convert.ToDecimal(dr["10月"]);
                    drNew["11月"] = dr["11月"] == null || GlobalObject.GeneralFunction.IsNullOrEmpty(dr["11月"].ToString()) ? 0 : Convert.ToDecimal(dr["11月"]);
                    drNew["12月"] = dr["12月"] == null || GlobalObject.GeneralFunction.IsNullOrEmpty(dr["12月"].ToString()) ? 0 : Convert.ToDecimal(dr["12月"]);

                    drNew["合计"] = Convert.ToDecimal(drNew["1月"]) + Convert.ToDecimal(drNew["2月"]) + Convert.ToDecimal(drNew["3月"]) +
                        Convert.ToDecimal(drNew["4月"]) + Convert.ToDecimal(drNew["5月"]) + Convert.ToDecimal(drNew["6月"]) +
                        Convert.ToDecimal(drNew["7月"]) + Convert.ToDecimal(drNew["8月"]) + Convert.ToDecimal(drNew["9月"]) +
                        Convert.ToDecimal(drNew["10月"]) + Convert.ToDecimal(drNew["11月"]) + Convert.ToDecimal(drNew["12月"]);

                    drNew["科目ID"] = dr["科目ID"];

                    dtResult.Rows.Add(drNew);
                }
            }

            DataRow drSum = dtResult.NewRow();

            drSum["预算科目"] = "合计";
            drSum["1月"] = GetSumPrice(dtResult, "1月");
            drSum["2月"] = GetSumPrice(dtResult, "2月");
            drSum["3月"] = GetSumPrice(dtResult, "3月");
            drSum["4月"] = GetSumPrice(dtResult, "4月");
            drSum["5月"] = GetSumPrice(dtResult, "5月");
            drSum["6月"] = GetSumPrice(dtResult, "6月");
            drSum["7月"] = GetSumPrice(dtResult, "7月");
            drSum["8月"] = GetSumPrice(dtResult, "8月");
            drSum["9月"] = GetSumPrice(dtResult, "9月");
            drSum["10月"] = GetSumPrice(dtResult, "10月");
            drSum["11月"] = GetSumPrice(dtResult, "11月");
            drSum["12月"] = GetSumPrice(dtResult, "12月");
            drSum["合计"] = GetSumPrice(dtResult, "合计");

            dtResult.Rows.Add(drSum);

            RefreshDataGridView(dtResult);
        }

        decimal GetSumPrice(DataTable dtTest, string columnName)
        {
            decimal result = 0;

            foreach (DataRow dr in dtTest.Rows)
            {
                result += dr[columnName] == null || GlobalObject.GeneralFunction.IsNullOrEmpty(dr[columnName].ToString()) ? 0 : Convert.ToDecimal(dr[columnName]);
            }

            return result;
        }

        void GetSumPrice()
        {
            foreach (DataGridViewColumn col in customDataGridView1.Columns)
            {
                if (col is DataGridViewNumericUpDownColumn)
                {
                    decimal columnSum = 0;
                    foreach (DataGridViewRow dr in customDataGridView1.Rows)
                    {
                        if (dr.Cells["预算科目"].Value.ToString() != "合计")
                        {
                            columnSum += dr.Cells[col.Index].Value == null ?
                                0 : Convert.ToDecimal(dr.Cells[col.Index].Value);
                        }
                    }

                    customDataGridView1.Rows[customDataGridView1.Rows.Count - 1].Cells[col.Index].Value = columnSum;
                }
            }

            foreach (DataGridViewRow dr in customDataGridView1.Rows)
            {
                decimal sum = 0;

                sum += Convert.ToDecimal(customDataGridView1.Rows[dr.Index].Cells["一月"].Value);
                sum += Convert.ToDecimal(customDataGridView1.Rows[dr.Index].Cells["二月"].Value);
                sum += Convert.ToDecimal(customDataGridView1.Rows[dr.Index].Cells["三月"].Value);
                sum += Convert.ToDecimal(customDataGridView1.Rows[dr.Index].Cells["四月"].Value);
                sum += Convert.ToDecimal(customDataGridView1.Rows[dr.Index].Cells["五月"].Value);
                sum += Convert.ToDecimal(customDataGridView1.Rows[dr.Index].Cells["六月"].Value);
                sum += Convert.ToDecimal(customDataGridView1.Rows[dr.Index].Cells["七月"].Value);
                sum += Convert.ToDecimal(customDataGridView1.Rows[dr.Index].Cells["八月"].Value);
                sum += Convert.ToDecimal(customDataGridView1.Rows[dr.Index].Cells["九月"].Value);
                sum += Convert.ToDecimal(customDataGridView1.Rows[dr.Index].Cells["十月"].Value);
                sum += Convert.ToDecimal(customDataGridView1.Rows[dr.Index].Cells["十一月"].Value);
                sum += Convert.ToDecimal(customDataGridView1.Rows[dr.Index].Cells["十二月"].Value);

                customDataGridView1.Rows[dr.Index].Cells["合计"].Value = sum;
            }
        }

        private void customDataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewCell dgvCell = customDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];

                if (dgvCell is DataGridViewNumericUpDownCell)
                {
                    decimal sum = 0;
                    decimal columnSum = 0;
                    decimal allSum = 0;

                    sum += Convert.ToDecimal(customDataGridView1.Rows[dgvCell.RowIndex].Cells["一月"].Value);
                    sum += Convert.ToDecimal(customDataGridView1.Rows[dgvCell.RowIndex].Cells["二月"].Value);
                    sum += Convert.ToDecimal(customDataGridView1.Rows[dgvCell.RowIndex].Cells["三月"].Value);
                    sum += Convert.ToDecimal(customDataGridView1.Rows[dgvCell.RowIndex].Cells["四月"].Value);
                    sum += Convert.ToDecimal(customDataGridView1.Rows[dgvCell.RowIndex].Cells["五月"].Value);
                    sum += Convert.ToDecimal(customDataGridView1.Rows[dgvCell.RowIndex].Cells["六月"].Value);
                    sum += Convert.ToDecimal(customDataGridView1.Rows[dgvCell.RowIndex].Cells["七月"].Value);
                    sum += Convert.ToDecimal(customDataGridView1.Rows[dgvCell.RowIndex].Cells["八月"].Value);
                    sum += Convert.ToDecimal(customDataGridView1.Rows[dgvCell.RowIndex].Cells["九月"].Value);
                    sum += Convert.ToDecimal(customDataGridView1.Rows[dgvCell.RowIndex].Cells["十月"].Value);
                    sum += Convert.ToDecimal(customDataGridView1.Rows[dgvCell.RowIndex].Cells["十一月"].Value);
                    sum += Convert.ToDecimal(customDataGridView1.Rows[dgvCell.RowIndex].Cells["十二月"].Value);

                    customDataGridView1.Rows[dgvCell.RowIndex].Cells["合计"].Value = sum;

                    foreach (DataGridViewRow dgvr in customDataGridView1.Rows)
                    {
                        if (dgvr.Cells["预算科目"].Value.ToString() != "合计")
                        {
                            columnSum += dgvr.Cells[dgvCell.ColumnIndex].Value == null ?
                                0 : Convert.ToDecimal(dgvr.Cells[dgvCell.ColumnIndex].Value);

                            allSum += dgvr.Cells["合计"].Value == null ?
                                0 : Convert.ToDecimal(dgvr.Cells["合计"].Value);
                        }
                    }

                    customDataGridView1.Rows[customDataGridView1.Rows.Count - 1].Cells[dgvCell.ColumnIndex].Value = columnSum;
                    customDataGridView1.Rows[customDataGridView1.Rows.Count - 1].Cells["合计"].Value = allSum;
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void customContextMenuStrip_Edit1__DeleteEvent(ref DataGridViewRow dgvr)
        {
            GetSumPrice();
        }
    }
}
