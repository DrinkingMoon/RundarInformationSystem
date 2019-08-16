using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using UniversalControlLibrary;
using Service_Economic_Financial;
using System.Collections;
using GlobalObject;

namespace Form_Economic_Financial
{
    /// <summary>
    /// 台帐查询界面
    /// </summary>
    public partial class 台帐查询 : Form
    {
        /// <summary>
        /// 财务报表组件
        /// </summary>
        IGatherBillAndDetailBillServer m_findEstrade = Service_Economic_Financial.ServerModuleFactory.GetServerModule<IGatherBillAndDetailBillServer>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        public 台帐查询()
        {
            InitializeComponent();

            DataTable dt = UniversalFunction.GetStorageTb();

            cmbStroage.Items.Add("全部库房");
            cmb_logistics_Storage.Items.Add("全部库房");

            foreach (DataRow dr in dt.Rows)
            {
                cmbStorage.Items.Add(dr["StorageName"].ToString());
                cmbStroage.Items.Add(dr["StorageName"].ToString());
                cmb_logistics_Storage.Items.Add(dr["StorageName"].ToString());
            }

            cmbStroage.Text = "全部库房";
            cmb_logistics_Storage.Text = "全部库房";

            DtpEndDate.Value = ServerTime.Time.AddDays(1);
            DtpStartDate.Value = DtpEndDate.Value.AddMonths(-1);

            for (int i = 2011; i <= ServerTime.Time.Year; i++)
            {
                cmbYear.Items.Add(i.ToString());
            }
        }

        void txtBatchNo_OnCompleteSearch()
        {
            txtBatchNo.Text = txtBatchNo.DataResult["批次号"].ToString();
            txtSpec.Tag = txtBatchNo.DataResult["供应商编码"].ToString();
        }

        void tbsGoods_OnCompleteSearch()
        {
            if (tbsGoods.DataResult != null)
            {
                tbsGoods.Text = tbsGoods.DataResult["物品名称"].ToString();
                tbsGoods.Tag = tbsGoods.DataResult["序号"].ToString();
                txtGoodsCode.Text = tbsGoods.DataResult["图号型号"].ToString();
                txtSpec.Text = tbsGoods.DataResult["规格"].ToString();
            }
            else
            {
                tbsGoods.Text = "";
                tbsGoods.Tag = null;
                txtGoodsCode.Text = "";
                txtSpec.Text = "";
            }
        }

        private void ShowDate(int GoodsID, DateTime dtStart, DateTime dtEnd)
        {
            try
            {
                DataTable dt = new DataTable();
                string strStorageID = UniversalFunction.GetStorageID(cmbStroage.Text);

                if (strStorageID == "")
                {
                    if (txtBatchNo.Text == "")
                    {
                        if (!m_findEstrade.GetAllGather("Estrade", GoodsID, dtStart, dtEnd, strStorageID,
                            txtBatchNo.Text, out dt, out m_err))
                        {
                            MessageDialog.ShowErrorMessage(m_err);
                            return;
                        }
                    }
                    else
                    {
                        if (!m_findEstrade.GetAllGather("EstradeBatchNo", GoodsID, dtStart, dtEnd, strStorageID,
                            txtBatchNo.Text, out dt, out m_err))
                        {
                            MessageDialog.ShowErrorMessage(m_err);
                            return;
                        }
                    }
                }
                else
                {
                    if (txtBatchNo.Text == "")
                    {
                        if (!m_findEstrade.GetAllGather("EstradeForStorage", GoodsID, dtStart, dtEnd, strStorageID,
                            txtBatchNo.Text, out dt, out m_err))
                        {
                            MessageDialog.ShowErrorMessage(m_err);
                            return;
                        }
                    }
                    else
                    {
                        if (!m_findEstrade.GetAllGather("EstradeBatchNoForStorage", GoodsID, dtStart, dtEnd,
                            strStorageID, txtBatchNo.Text, out dt, out m_err))
                        {
                            MessageDialog.ShowErrorMessage(m_err);
                            return;
                        }
                    }
                }


                dataGridView1.DataSource = dt;
                dataGridView1.Columns["物品ID"].Visible = false;

                if (!GlobalObject.BasicInfo.IsFuzzyContainsRoleName("会计"))
                {
                    dataGridView1.Columns["借方金额"].Visible = false;
                    dataGridView1.Columns["贷方金额"].Visible = false;
                    dataGridView1.Columns["结存金额"].Visible = false;
                    dataGridView1.Columns["单价"].Visible = false;
                }

            }
            catch (Exception ex)
            {
                MessageDialog.ShowErrorMessage(ex.Message);
                dataGridView1.DataSource = null;
                return;
            }
        }

        private void 台帐查询_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        private void txtBatchNo_Enter(object sender, EventArgs e)
        {
            string strfilter = " and 物品ID = " + Convert.ToInt32(tbsGoods.Tag);

            if (cmbStroage.Text != "全部库房")
            {
                strfilter = strfilter + " and 库房代码 = '"
                    + UniversalFunction.GetStorageID(cmbStroage.Text) + "'";
            }

            txtBatchNo.StrEndSql = strfilter;
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells["结存数量"].Value.ToString().Trim() != ""
                && txtBatchNo.Text.Trim() == "")
            {
                台帐的库存查询 form = new 台帐的库存查询(
                    dataGridView1.CurrentRow.Cells["操作时间"].Value.ToString(),
                    Convert.ToInt32(tbsGoods.Tag));
                form.ShowDialog();
            }
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {

            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                e.RowBounds.Location.Y,
                dataGridView1.RowHeadersWidth - 4,
                e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                dataGridView1.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dataGridView1.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private void btnFindCode_Click(object sender, EventArgs e)
        {
            FormQueryInfo form = QueryInfoDialog.GetPlanCostGoodsDialog();
            form.ShowColumns = new string[] { "图号型号", "物品名称", "规格", "物品类别名称", "序号" };

            if (form != null && DialogResult.OK == form.ShowDialog())
            {
                txtGoodsCode.Text = form.GetDataItem("图号型号").ToString();
                tbsGoods.Text = form.GetDataItem("物品名称").ToString();
                txtSpec.Text = form.GetDataItem("规格").ToString();
                tbsGoods.Tag = form.GetDataItem("序号").ToString();
            }
        }

        private void btnOutExcel_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
        }

        private void cmbStroage_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBatchNo.Text = "";
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            decimal dcmQC = 0;
            decimal dcmRK = 0;
            decimal dcmCK = 0;
            decimal dcmQM = 0;

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells["单据号"].Value.ToString() == "期初")
                {
                    if (dataGridView1.Rows[i].Cells["结存数量"].Value.ToString() != "")
                    {
                        dcmQC = Convert.ToDecimal(dataGridView1.Rows[i].Cells["结存数量"].Value);
                    }
                }
                else if (dataGridView1.Rows[i].Cells["单据号"].Value.ToString() == "期末")
                {
                    if (dataGridView1.Rows[i].Cells["借方数量"].Value.ToString() != "")
                    {
                        dcmRK = Convert.ToDecimal(dataGridView1.Rows[i].Cells["借方数量"].Value);
                    }

                    if (dataGridView1.Rows[i].Cells["贷方数量"].Value.ToString() != "")
                    {
                        dcmCK = Convert.ToDecimal(dataGridView1.Rows[i].Cells["贷方数量"].Value);
                    }

                    if (dataGridView1.Rows[i].Cells["结存数量"].Value.ToString() != "")
                    {
                        dcmQM = Convert.ToDecimal(dataGridView1.Rows[i].Cells["结存数量"].Value);
                    }
                }
            }

            if (dcmQM != dcmQC + dcmRK - dcmCK)
            {
                label9.Text = "此记录账目错误";
            }
            else
            {
                label9.Text = "此记录账目正确";
            }
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            label9.Text = "无消息";
            ShowDate(Convert.ToInt32(tbsGoods.Tag), DtpStartDate.Value.Date, DtpEndDate.Value.Date);
        }

        private void rbDG_CheckedChanged(object sender, EventArgs e)
        {
            cmbStorage.Enabled = rbDG.Checked;
        }

        private void btnOutput_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, customDataGridView1);
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            string error = null;

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(cmbYear.Text) 
                || GlobalObject.GeneralFunction.IsNullOrEmpty(cmbMonth.Text) 
                || (GlobalObject.GeneralFunction.IsNullOrEmpty(cmbStorage.Text) && rbDG.Checked))
            {
                throw new Exception("请选择【年份】、【月份】、【所属库房】");
            }

            string yearMonth = cmbYear.Text + cmbMonth.Text;
            string typeName = "";

            foreach (Control cl in panel2.Controls)
            {
                if (cl is RadioButton && ((RadioButton)cl).Checked)
                {
                    typeName = ((RadioButton)cl).Text;
                }
            }

            typeName = typeName == "单个库房" ? UniversalFunction.GetStorageID(cmbStorage.Text) : typeName;

            string selectType = "";

            foreach (Control cl in panel3.Controls)
            {
                if (cl is RadioButton && ((RadioButton)cl).Checked)
                {
                    selectType = ((RadioButton)cl).Text;
                }
            }

            Hashtable hs = new Hashtable();

            hs.Add("@YearMonth", yearMonth);
            hs.Add("@TypeName", typeName);
            hs.Add("@SelectType", selectType);

            DataTable tempTable = 
                GlobalObject.DatabaseServer.QueryInfoPro("MonthlyBalanceSelect_GoodsCount", hs, out error);

            customDataGridView1.DataSourceBinding.DataSource = tempTable;
        }

        private void btn_logistics_Output_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dgvlogistics);
        }

        private void btn_logistics_Select_Click(object sender, EventArgs e)
        {
            string error = null;

            if (dtp_logistics_startTime.Value > dtp_logistics_endTime.Value)
            {
                MessageDialog.ShowPromptMessage("【起始时间】不能大于【截止时间】");
                return;
            }

            Hashtable hs = new Hashtable();

            hs.Add("@StartDate", dtp_logistics_startTime.Value);
            hs.Add("@EndDate", dtp_logistics_endTime.Value);
            hs.Add("@StorageID", UniversalFunction.GetStorageID(cmb_logistics_Storage.Text));

            DataTable tempTable = DatabaseServer.QueryInfoPro("Bus_Logistics_Select", hs, out error);

            dgvlogistics.DataSourceBinding.DataSource = tempTable;
        }
    }
}
