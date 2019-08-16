using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Expression;
using ServerModule;
using Service_Peripheral_External;
using UniversalControlLibrary;

namespace Form_Peripheral_External
{
    public partial class 唯一标识码录入窗体 : Form
    {
        /// <summary>
        /// 调运单数据集
        /// </summary>
        Out_ManeuverList m_lnqManeuverList = new Out_ManeuverList();

        /// <summary>
        /// 标识码服务
        /// </summary>
        IUniqueIdentifier m_serverIdentifier = Service_Peripheral_External.ServerModuleFactory.GetServerModule<IUniqueIdentifier>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr = "";

        public 唯一标识码录入窗体(Out_ManeuverList maneuverList, bool IsOperation)
        {
            InitializeComponent();

            m_lnqManeuverList = maneuverList;

            if (!IsOperation)
            {
                btnAdd.Visible = false;
                btnDelete.Visible = false;
                btnSubmit.Visible = false;
            }

            dataGridView1.DataSource = m_serverIdentifier.GetInfo(m_lnqManeuverList.Bill_ID, m_lnqManeuverList.GoodsID, m_lnqManeuverList.StorageID);

            F_GoodsPlanCost lnqGoodsInfo = IntegrativeQuery.QueryGoodsInfo(m_lnqManeuverList.GoodsID);

            txtGoodsCode.Text = lnqGoodsInfo.GoodsCode;
            txtGoodsName.Text = lnqGoodsInfo.GoodsName;
            txtSpec.Text = lnqGoodsInfo.Spec;
            txtOperationCount.Text = m_lnqManeuverList.ShipperCount.ToString();
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            DataTable dtTemp = (DataTable)dataGridView1.DataSource;

            if (dataGridView1.Rows.Count == Convert.ToDecimal(txtOperationCount.Text))
            {
                MessageDialog.ShowPromptMessage("已超出应录入的标识码数量");
                return;
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (txtIdentifier.Text == dataGridView1.Rows[i].Cells[0].ToString())
                {
                    MessageDialog.ShowPromptMessage("不能录入重复的标识码");
                    return;
                }
            }

            DataRow dr = dtTemp.NewRow();

            dr["Identifier"] = txtIdentifier.Text;

            dtTemp.Rows.Add(dr);

            dataGridView1.DataSource = dtTemp;

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
                if ((string)dataGridView1.Rows[i].Cells["标识码"].Value == txtIdentifier.Text)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                    break;
                }
            }

            txtIdentifier.Focus();
            txtIdentifier.SelectAll();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }
            else
            {
                DataTable dtTemp = (DataTable)dataGridView1.DataSource;

                foreach (DataGridViewRow dr in dataGridView1.SelectedRows)
                {
                    if (dr.Selected)
                    {
                        dtTemp.Rows.RemoveAt(dr.Index);
                    }
                }

                dataGridView1.DataSource = dtTemp;
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count != Convert.ToDecimal(txtOperationCount.Text))
            {
                MessageDialog.ShowPromptMessage("请录入对应数量的标识码");
                return;
            }

            if (!m_serverIdentifier.SubmitInfo(m_lnqManeuverList.Bill_ID,m_lnqManeuverList.GoodsID, m_lnqManeuverList.StorageID, 
                (DataTable)dataGridView1.DataSource,out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
            }
            else
            {
                MessageDialog.ShowPromptMessage("已提交");
                this.Close();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
