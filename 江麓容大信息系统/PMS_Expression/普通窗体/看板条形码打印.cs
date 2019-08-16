using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using GlobalObject;

namespace Expression
{
    public partial class 看板条形码打印 : Form
    {
        IBarCodeServer _serviceBarCode = ServerModule.ServerModuleFactory.GetServerModule<IBarCodeServer>();

        public 看板条形码打印()
        {
            InitializeComponent();
        }

        private void txtGoodsCode_OnCompleteSearch()
        {
            txtGoodsName.Text = txtGoodsCode.DataResult["物品名称"].ToString();
            txtGoodsCode.Tag = txtGoodsCode.DataResult["序号"];
            txtSpec.Text = txtGoodsCode.DataResult["规格"].ToString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string code = Convert.ToInt32(txtGoodsCode.Tag).ToString("D6") + Convert.ToInt32(numGoodsCount.Value).ToString("D5");

                P_PrintBoardForVehicleBarcode barCode = new P_PrintBoardForVehicleBarcode();

                barCode.BarCode = code;
                barCode.GoodsCount = numGoodsCount.Value;
                barCode.GoodsID = Convert.ToInt32(txtGoodsCode.Tag);
                barCode.PrintCount = numPrintCount.Value;

                _serviceBarCode.AddBoardBarCodeRecord(barCode);

                customDataGridView1.Rows.Add(new object[]{0, barCode.BarCode, txtGoodsCode.Text, 
                txtGoodsName.Text, txtSpec.Text, barCode.GoodsCount, barCode.PrintCount, barCode.GoodsID});
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dgvr in customDataGridView1.SelectedRows)
            {
                try
                {
                    _serviceBarCode.DeleteBoardBarCodeRecord(dgvr.Cells["条形码"].Value.ToString());
                    customDataGridView1.Rows.Remove(dgvr);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("请确认已勾选需要【执行打印任务】的记录", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                foreach (DataGridViewRow dgvr in customDataGridView1.Rows)
                {
                    if (Convert.ToBoolean(dgvr.Cells["选"].Value))
                    {
                        for (int i = 0; i < Convert.ToInt32(dgvr.Cells["打印数量"].Value); i++)
                        {
                            ServerModule.PrintPartBarcode.PrintBarcode_120X30(dgvr.Cells["条形码"].Value.ToString());
                        }
                    }
                }
            }
        }

        private void 看板条形码打印_Load(object sender, EventArgs e)
        {
            DataTable recordeTable = _serviceBarCode.ShowBoardBarCodeRecord();

            foreach (DataRow dr in recordeTable.Rows)
            {
                customDataGridView1.Rows.Add(new object[]{0, dr["条形码"].ToString(),
                    dr["图号型号"].ToString(), dr["物品名称"].ToString(), dr["规格"].ToString(),
                    Convert.ToDecimal(dr["数量"]), Convert.ToDecimal(dr["打印数量"]),  Convert.ToInt32( dr["物品ID"])});
            }

            userControlDataLocalizer1.Init(customDataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, customDataGridView1.Name, BasicInfo.LoginID));
        }

        private void customDataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                customDataGridView1.CurrentRow.Cells["选"].Value = 
                    !Convert.ToBoolean(customDataGridView1.CurrentRow.Cells["选"].Value);
            }
        }

        private void 选择ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (customDataGridView1.Rows != null && customDataGridView1.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow dgvr in customDataGridView1.SelectedRows)
                {
                    dgvr.Cells["选"].Value = true;
                }
            }
        }

        private void 全选ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (customDataGridView1.Rows != null && customDataGridView1.Rows.Count > 0)
            {
                foreach (DataGridViewRow dgvr in customDataGridView1.Rows)
                {
                    dgvr.Cells["选"].Value = true;
                }
            }
        }

        private void 全消ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (customDataGridView1.Rows != null && customDataGridView1.Rows.Count > 0)
            {
                foreach (DataGridViewRow dgvr in customDataGridView1.Rows)
                {
                    dgvr.Cells["选"].Value = false;
                }
            }
        }
    }
}
