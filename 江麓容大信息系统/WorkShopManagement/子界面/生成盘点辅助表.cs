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
using Service_Manufacture_WorkShop;
using GlobalObject;

namespace Form_Manufacture_WorkShop
{
    public partial class 生成盘点辅助表 : Form
    {
        IInProductReport _Service_InProcutReport = Service_Manufacture_WorkShop.ServerModuleFactory.GetServerModule<IInProductReport>();

        public 生成盘点辅助表()
        {
            InitializeComponent();
        }

        private void 生成盘点辅助表_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                cmbYear.Items.Add(ServerTime.Time.Year - i);
            }

            cmbYear.Text = ServerTime.Time.Year.ToString();
            cmbMonth.Text = ServerTime.Time.Month.ToString("D2");
        }

        private void txtGoodsCode_OnCompleteSearch()
        {
            if (txtGoodsCode.DataResult == null)
            {
                return;
            }

            txtGoodsCode.Text = txtGoodsCode.DataResult["图号型号"].ToString();
            txtGoodsCode.Tag = txtGoodsCode.DataResult["序号"];

            txtGoodsName.Text = txtGoodsCode.DataResult["物品名称"].ToString();
            txtSpec.Text = txtGoodsCode.DataResult["规格"].ToString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtGoodsCode.Tag == null)
                {
                    throw new Exception("请选择【产品】");
                }

                if (numProductCount.Value <= 0)
                {
                    throw new Exception("请填写【数量】");
                }

                foreach (DataGridViewRow dgvr in customDataGridView1.Rows)
                {
                    if (dgvr.Cells["物品ID"].Value.ToString() == txtGoodsCode.Tag.ToString())
                    {
                        throw new Exception("不能重复添加相同产品");
                    }
                }

                customDataGridView1.Rows.Add(new object[]{txtGoodsCode.Text, txtGoodsName.Text, txtSpec.Text, numProductCount.Value, txtGoodsCode.Tag});

            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (customDataGridView1.CurrentRow == null)
            {
                return;
            }

            customDataGridView1.Rows.Remove(customDataGridView1.CurrentRow);
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbYear.Text.Trim().Length == 0)
                {
                    throw new Exception("请选择【年份】");
                }

                if (cmbMonth.Text.Trim().Length == 0)
                {
                    throw new Exception("请选择【月份】");
                }

                if (customDataGridView1.Rows.Count == 0)
                {
                    throw new Exception("请添加【产品】");
                }

                DataTable dtProduct = new DataTable();

                dtProduct.Columns.Add("GoodsID");
                dtProduct.Columns.Add("GoodsCount");

                foreach (DataGridViewRow dgvr in customDataGridView1.Rows)
                {
                    DataRow dr = dtProduct.NewRow();

                    dr["GoodsID"] = Convert.ToInt32(dgvr.Cells["物品ID"].Value);
                    dr["GoodsCount"] = Convert.ToDecimal(dgvr.Cells["产品数量"].Value);

                    dtProduct.Rows.Add(dr);
                }

                List<object> lstObj = new List<object>();

                lstObj.Add(cmbYear.Text + cmbMonth.Text);
                lstObj.Add(dtpEndTime.Value);
                lstObj.Add(dtProduct);

                _Service_InProcutReport.CreateInProductReport(lstObj);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }
    }
}
