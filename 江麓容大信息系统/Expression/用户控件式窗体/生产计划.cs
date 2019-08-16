using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using PlatformManagement;
using GlobalObject;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 生产计划界面
    /// </summary>
    public partial class 生产计划 : Form
    {
        /// <summary>
        /// 服务
        /// </summary>
        IProductPlan m_serverPdPlan = ServerModuleFactory.GetServerModule<IProductPlan>();

        /// <summary>
        /// 明细TB
        /// </summary>
        DataTable m_dtMx = new DataTable();

        /// <summary>
        /// 服务LNQ
        /// </summary>
        S_ProductPlan m_lnqPdPlan = new S_ProductPlan();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        public 生产计划(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();
        }

        private void 生产计划_Load(object sender, EventArgs e)
        {
            ClearDate();
            foreach (TabPage item in tabControl1.TabPages)
            {
                foreach (Control cl in item.Controls)
                {
                    if (cl is DataGridView)
                    {
                        ((DataGridView)cl).DataSource = m_serverPdPlan.GetAllBill(item.Name);

                        ((DataGridView)cl).Columns["计划类别"].Visible = false;
                        ((DataGridView)cl).Columns["单据号"].Visible = false;
                        ((DataGridView)cl).Columns["单据状态"].Visible = false;
                    }
                }
            }
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="billNo">定位用的单据号</param>
        void PositioningRecord(string billNo)
        {
            DataGridView dgv = new DataGridView();

            foreach (Control cl in tabControl1.SelectedTab.Controls)
            {
                if (cl is DataGridView)
                {
                    dgv = (DataGridView)cl;
                }
            }

            string strColName = "";

            foreach (DataGridViewColumn col in dgv.Columns)
            {
                if (col.Visible)
                {
                    strColName = col.Name;
                    break;
                }
            }

            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                if ((string)dgv.Rows[i].Cells["单据号"].Value == billNo)
                {
                    dgv.FirstDisplayedScrollingRowIndex = i;
                    dgv.CurrentCell = dgv.Rows[i].Cells[strColName];
                    break;
                }
            }
        }

        void tbsCode_OnCompleteSearch()
        {
            tbsCode.Tag = Convert.ToInt32(tbsCode.DataResult["序号"]);
            tbsCode.Text = tbsCode.DataResult["图号型号"].ToString();
            txtName.Text = tbsCode.DataResult["物品名称"].ToString();
            txtSpec.Text = tbsCode.DataResult["规格"].ToString();
            lbdw.Text = tbsCode.DataResult["单位"].ToString();
            numCount.Focus();
        }

        string GetBillNo()
        {
            string strQZ = "";

            switch (cbPlanType.Text)
            {
                case "月计划":
                    strQZ = "M";
                    break;
                case "日计划":
                    strQZ = "D";
                    break;
                default:
                    break;
            }

            string result = strQZ + dtpPlanTime.Value.Year.ToString() + 
                dtpPlanTime.Value.Month.ToString("D2") + dtpPlanTime.Value.Day.ToString("D2");

            return result;
        }

        /// <summary>
        /// 获得信息
        /// </summary>
        private void GetMessage()
        {
            m_dtMx = (DataTable)dataGridViewList.DataSource;

            m_lnqPdPlan.DJH = GetBillNo();
            m_lnqPdPlan.PlanTime = Convert.ToDateTime(dtpPlanTime.Value.ToShortDateString());
            m_lnqPdPlan.DJZT = dtpPlanTime.Tag == null ? "" : dtpPlanTime.Tag.ToString();
            m_lnqPdPlan.PlanType = cbPlanType.Text;
            m_lnqPdPlan.Remark = txtBillRemark.Text;
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="source">数据集</param>
        void RefreshDataGirdView()
        {
            foreach (Control cl in tabControl1.SelectedTab.Controls)
            {
                if (cl is DataGridView)
                {
                    ((DataGridView)cl).DataSource = m_serverPdPlan.GetAllBill(tabControl1.SelectedTab.Name);

                    ((DataGridView)cl).Columns["计划类别"].Visible = false;
                    ((DataGridView)cl).Columns["单据号"].Visible = false;
                    ((DataGridView)cl).Columns["单据状态"].Visible = false;
                }
            }
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        private void ClearDate()
        {
            txtName.Text = "";
            txtSpec.Text = "";
            tbsCode.Text = "";
            tbsCode.Tag = -1;
            dataGridViewList.DataSource = m_serverPdPlan.GetList("");
            lbdw.Text = "";
            numCount.Value = 0;
            dtpPlanTime.Value = ServerTime.Time;
        }

        /// <summary>
        /// 检查数据
        /// </summary>
        /// <returns>通过返回True，否则False</returns>
        private bool CheckDate()
        {
            if (cbPlanType.Text == "月计划")
            {
                if (m_serverPdPlan.IsRepeatPlanDate(Convert.ToDateTime(dtpPlanTime.Value.ToShortDateString()), m_lnqPdPlan.PlanType))
                {
                    MessageDialog.ShowErrorMessage("此天计划已经生成，请重新计划日期");
                    dtpPlanTime.Value = ServerTime.Time;
                    dtpPlanTime.Focus();
                    return false;
                }

                DateTime tempDateTime = ServerTime.Time;

                if (Convert.ToDateTime(ServerTime.Time.ToShortDateString()) < Convert.ToDateTime(dtpPlanTime.Value.ToShortDateString()))
                {
                    DateTime tempDt = Convert.ToDateTime(ServerTime.Time.Year + "-" + ServerTime.Time.Month + "-" + Convert.ToInt32(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.月结日]) + " 00:00:00");

                    if (ServerTime.Time.Day <= Convert.ToInt32(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.月结日]))
                    {
                        tempDt = tempDt.AddMonths(-1);
                    }

                    if (Convert.ToDateTime(dtpPlanTime.Value.ToShortDateString()) < tempDt)
                    {
                        MessageDialog.ShowPromptMessage("本月不能做上月计划");
                        dtpPlanTime.Focus();
                        return false;
                    }
                }
            }
            else if (cbPlanType.Text == "日计划")
            {
                if (dtpPlanTime.Value.Date <= ServerTime.Time.Date)
                {
                    if (m_serverPdPlan.IsRepeatPlanDate(Convert.ToDateTime(dtpPlanTime.Value.ToShortDateString()), m_lnqPdPlan.PlanType))
                    {
                        MessageDialog.ShowErrorMessage("此天计划已经生成，请重新计划日期");
                        dtpPlanTime.Value = ServerTime.Time;
                        dtpPlanTime.Focus();
                        return false;
                    }
                }
            }

            if (cbPlanType.Text == "" || cbPlanType.Text == null)
            {
                MessageDialog.ShowPromptMessage("请选择计划类别！");
                cbPlanType.Focus();
                return false;
            }

            if (m_dtMx.Rows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请录入起码一条生产计划明细,台数可以为0!");
                return false;
            }

            return true;
        }

        private void dataGridView_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = new DataGridView();

            foreach (Control cl in tabControl1.SelectedTab.Controls)
            {
                if (cl is DataGridView)
                {
                    dgv = (DataGridView)cl;
                }
            }

            if (dgv.Rows.Count == 0)
            {
                dataGridViewList.DataSource = null;
                return;
            }
            else
            {
                dtpPlanTime.Value = Convert.ToDateTime(dgv.CurrentRow.Cells["计划日期"].Value);
                txtBillRemark.Text = dgv.CurrentRow.Cells["备注"].Value.ToString();
                cbPlanType.Text = dgv.CurrentRow.Cells["计划类别"].Value.ToString();

                dataGridViewList.DataSource = m_serverPdPlan.GetList(dgv.CurrentRow.Cells["单据号"].Value.ToString());
            }
        }

        private void dataGridView2_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewList.Rows.Count == 0)
            {
                return;
            }
            else
            {
                tbsCode.Text = dataGridViewList.CurrentRow.Cells["图号型号"].Value.ToString();
                tbsCode.Tag = Convert.ToInt32(dataGridViewList.CurrentRow.Cells["物品ID"].Value);
                txtName.Text = dataGridViewList.CurrentRow.Cells["物品名称"].Value.ToString();
                txtSpec.Text = dataGridViewList.CurrentRow.Cells["规格"].Value.ToString();
                numCount.Value = Convert.ToDecimal(dataGridViewList.CurrentRow.Cells["数量"].Value.ToString());
                lbdw.Text = dataGridViewList.CurrentRow.Cells["单位"].Value.ToString();
                cmbType.Text = dataGridViewList.CurrentRow.Cells["类型"].Value.ToString();
            }
        }

        private void 新建单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearDate();
        }

        private void 提交单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cbPlanType.Tag != null && cbPlanType.Tag.ToString() == "单据已完成")
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
                return;
            }

            GetMessage();

            if (!CheckDate())
            {
                return;
            }

            m_lnqPdPlan.DJH = GetBillNo();

            if (m_serverPdPlan.AddBill(m_lnqPdPlan, m_dtMx, out m_err))
            {
                MessageDialog.ShowPromptMessage("提交成功！");
            }
            else
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            RefreshDataGirdView();
            PositioningRecord(m_lnqPdPlan.DJH);
        }

        private void 删除单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridView dgv = new DataGridView();

            foreach (Control cl in tabControl1.SelectedTab.Controls)
            {
                if (cl is DataGridView)
                {
                    dgv = (DataGridView)cl;
                }
            }

            if (BasicInfo.LoginName != dgv.CurrentRow.Cells["编制人"].Value.ToString())
            {
                MessageDialog.ShowPromptMessage("您不是此记录的编制者无法进行此操作");
                return;
            }

            if (tabControl1.SelectedTab.Name == "月计划" && (dgv.CurrentRow.Cells["单据状态"].Value.ToString() == "单据已完成"
                || dgv.CurrentRow.Cells["单据状态"].Value.ToString() == "单据已被变更"))
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
                return;
            }

            if (MessageDialog.ShowEnquiryMessage("您是否要删除此单据") == DialogResult.No)
            {
                return;
            }

            if (m_serverPdPlan.DeleteBill(dgv.CurrentRow.Cells["单据号"].Value.ToString(), out m_err))
            {
                MessageDialog.ShowPromptMessage("删除成功");
            }
            else
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            ClearDate();
            RefreshDataGirdView();
        }

        private void 刷新数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshDataGirdView();
            PositioningRecord(m_lnqPdPlan.DJH);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dataGridViewList.DataSource == null ? m_serverPdPlan.GetList("") : (DataTable)dataGridViewList.DataSource;

            if (cmbType.Text == "")
            {
                MessageDialog.ShowPromptMessage("请选择类型");
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["物品ID"].ToString() == tbsCode.Tag.ToString()
                    && dt.Rows[i]["类型"].ToString() == cmbType.Text)
                {
                    MessageDialog.ShowErrorMessage("不能重复录入，否则会蓝屏");
                    return;
                }
            }

            DataRow dr = dt.NewRow();

            dr["图号型号"] = tbsCode.Text;
            dr["物品ID"] = Convert.ToInt32(tbsCode.Tag);
            dr["物品名称"] = txtName.Text;
            dr["规格"] = txtSpec.Text;
            dr["数量"] = numCount.Value;
            dr["单位"] = lbdw.Text;
            dr["类型"] = cmbType.Text;
            dr["DJH"] = GetBillNo();

            dt.Rows.Add(dr);
            dataGridViewList.DataSource = dt;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dataGridViewList.DataSource;

            foreach (DataGridViewRow dr in dataGridViewList.SelectedRows)
            {
                if (dr.Selected)
                {
                    dt.Rows.RemoveAt(dr.Index);
                }
            }

            dataGridViewList.DataSource = dt;
        }

        private void cbPlanType_SelectedIndexChanged(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabControl1.TabPages[cbPlanType.Text];
            ClearDate();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearDate();
            cbPlanType.Text = tabControl1.SelectedTab.Name;
        }
    }
}
