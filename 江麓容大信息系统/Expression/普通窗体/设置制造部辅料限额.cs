using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using GlobalObject;
using PlatformManagement;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 辅料限额设置界面
    /// </summary>
    public partial class 设置制造部辅料限额 : Form
    {
        /// <summary>
        /// 领料单服务
        /// </summary>
        IMaterialRequisitionServer m_serverMaterialBill = ServerModuleFactory.GetServerModule<IMaterialRequisitionServer>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        public 设置制造部辅料限额()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="billNo">定位用的单据号</param>
        void PositioningRecord(DataRow drinsert)
        {
            string strColName = "";

            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                if (col.Visible)
                {
                    strColName = col.Name;
                    break;
                }
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if ((int)dataGridView1.Rows[i].Cells["物品ID"].Value == Convert.ToInt32( drinsert["物品ID"])
                    && dataGridView1.Rows[i].Cells["部门编码"].Value.ToString() == drinsert["部门编码"].ToString())
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                    break;
                }
            }
        }

        /// <summary>
        /// 绑定datagridview
        /// </summary>
        /// <param name="dtbinding">绑定的数据源</param>
        void BindingDataGridView(DataGridView dgv, UserControlDataLocalizer ud, DataTable dtBinding)
        {
            if (dgv == null || ud == null || dtBinding == null)
            {
                return;
            }

            dgv.DataSource = dtBinding;
            ud.Init(dgv, this.Name, UniversalFunction.SelectHideFields(this.Name, dgv.Name, BasicInfo.LoginID));
        }

        /// <summary>
        /// 检查相同记录
        /// </summary>
        /// <param name="intGoods">物品ID</param>
        /// <param name="strDeptCode">部门编码</param>
        /// <returns>检测通过返回True，否则返回False</returns>
        bool CheckSameGoods(int intGoods,string strDeptCode)
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (Convert.ToInt32( dt.Rows[i]["物品ID"]) == intGoods
                    && dt.Rows[i]["部门编码"].ToString() == strDeptCode)
                {
                    return false;
                }
            }

            return true;
        }

        void txtName_OnCompleteSearch()
        {
            txtName.Text = txtName.DataResult["物品名称"].ToString();
            txtCode.Text = txtName.DataResult["图号型号"].ToString();
            txtSpec.Text = txtName.DataResult["规格"].ToString();
            txtType.Text = txtName.DataResult["物品类别"].ToString();
            txtName.Tag = Convert.ToInt32(txtName.DataResult["序号"]);
            lbDW.Text = txtName.DataResult["单位"].ToString();
        }

        void txtDept_OnCompleteSearch()
        {
            txtDept.Text = txtDept.DataResult["部门名称"].ToString();
            txtDept.Tag = txtDept.DataResult["部门编码"].ToString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!CheckSameGoods(Convert.ToInt32(txtName.Tag),txtDept.Tag.ToString()))
            {
                MessageDialog.ShowPromptMessage("不能添加相同记录");
                return;
            }

            DataTable dt = (DataTable)dataGridView1.DataSource;

            DataRow dr = dt.NewRow();

            dr["物品ID"] = Convert.ToInt32(txtName.Tag);
            dr["物品名称"] = txtName.Text;
            dr["图号型号"] = txtCode.Text;
            dr["规格"] = txtSpec.Text;
            dr["物品类别"] = txtType.Text;
            dr["部门名称"] = txtDept.Text;
            dr["部门编码"] = txtDept.Tag.ToString();
            dr["单位"] = lbDW.Text;
            dr["基数"] = numCount.Value;

            dt.Rows.Add(dr);

            BindingDataGridView(dataGridView1, userControlDataLocalizer1, dt);
            PositioningRecord(dr);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;

            foreach (DataGridViewRow dr in dataGridView1.SelectedRows)
            {
                if (dr.Selected)
                {
                    dt.Rows.RemoveAt(dr.Index);
                }
            }

            BindingDataGridView(dataGridView1, userControlDataLocalizer1, dt);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                return;
            }

            dataGridView1.CurrentRow.Cells["基数"].Value = numCount.Value;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;

            if (!m_serverMaterialBill.SaveDeptPickingToplimit(dt,out m_err))
            {
                MessageDialog.ShowPromptMessage(m_err);
            }
            else
            {
                MessageDialog.ShowPromptMessage("保存成功！");
            }

            BindingDataGridView(dataGridView1, userControlDataLocalizer1, m_serverMaterialBill.GetManufacturingConsumeTable());
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows.Count == 0 || !numCount.Visible)
            {
                return;
            }

            txtCode.Text = dataGridView1.CurrentRow.Cells["图号型号"].Value.ToString();
            txtName.Text = dataGridView1.CurrentRow.Cells["物品名称"].Value.ToString();
            txtSpec.Text = dataGridView1.CurrentRow.Cells["规格"].Value.ToString();
            txtType.Text = dataGridView1.CurrentRow.Cells["物品类别"].Value.ToString();
            txtDept.Text = dataGridView1.CurrentRow.Cells["部门名称"].Value.ToString();
            txtName.Tag = Convert.ToInt32( dataGridView1.CurrentRow.Cells["物品ID"].Value);
            txtDept.Tag = dataGridView1.CurrentRow.Cells["部门编码"].Value.ToString();
            lbDW.Text = dataGridView1.CurrentRow.Cells["单位"].Value.ToString();
            numCount.Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["基数"].Value);
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            DataGridView dgv = new DataGridView();

            foreach (Control cl in tabControl1.SelectedTab.Controls)
            {
                if (cl is DataGridView)
                {
                    dgv = (DataGridView)cl;
                }
            }

            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dgv);
        }

        private void btnCreateQuota_Click(object sender, EventArgs e)
        {
            if (!m_serverMaterialBill.AutogenerationPickingToplimit(numCVTNumber.Value, out m_err))
            {
                MessageDialog.ShowPromptMessage(m_err);
            }
            else
            {
                MessageDialog.ShowPromptMessage("生成成功！");
            }

            BindingDataGridView(dataGridView2, userControlDataLocalizer2, m_serverMaterialBill.GetDeptPickingToplimitTable());
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 设置制造部辅料限额_Load(object sender, EventArgs e)
        {
            BindingDataGridView(dataGridView1, userControlDataLocalizer1, m_serverMaterialBill.GetManufacturingConsumeTable());
            BindingDataGridView(dataGridView2, userControlDataLocalizer2, m_serverMaterialBill.GetDeptPickingToplimitTable());
            BindingDataGridView(dataGridView3, userControlDataLocalizer3, m_serverMaterialBill.GetKievMaterialInfo());
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (Control cl in tabControl1.SelectedTab.Controls)
            {
                if (cl is DataGridView)
                {
                    if ((DataGridView)cl == dataGridView1)
                    {
                        BindingDataGridView(dataGridView1, userControlDataLocalizer1, 
                            m_serverMaterialBill.GetManufacturingConsumeTable()); 
                    }
                    else if ((DataGridView)cl == dataGridView2)
                    {
                        BindingDataGridView(dataGridView2, userControlDataLocalizer2, 
                            m_serverMaterialBill.GetDeptPickingToplimitTable());
                    }
                    else if ((DataGridView)cl == dataGridView3)
                    {
                        BindingDataGridView(dataGridView3, userControlDataLocalizer3, m_serverMaterialBill.GetKievMaterialInfo());
                    }
                }
            }
        }
    }
}
