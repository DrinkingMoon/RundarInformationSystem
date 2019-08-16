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
using PlatformManagement;
using Service_Peripheral_External;
using Expression;
using UniversalControlLibrary;

namespace Form_Peripheral_External
{
    public partial class 挂账单 : Form
    {
        /// <summary>
        /// 权限组件
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 错误信息
        /// </summary>
        //string m_error = "";

        /// <summary>
        /// 挂账单服务类
        /// </summary>
        IBuyingBillServer m_buyingServer = Service_Peripheral_External.ServerModuleFactory.GetServerModule<IBuyingBillServer>();

        public 挂账单(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authFlag = nodeInfo.Authority;

            dtpStartTime.Value = ServerTime.Time.AddDays(1).AddMonths(-1);
            dtpEndTime.Value = ServerTime.Time.AddDays(1);

            DataRefresh();
        }

        /// <summary>
        /// 权限控制
        /// </summary>
        /// <param name="authorityFlag">权限标志</param>
        void AuthorityControl(PlatformManagement.AuthorityFlag authorityFlag)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip1, authorityFlag);
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="info">定位用的信息</param>
        void PositioningRecord(string info)
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
                if ((string)dataGridView1.Rows[i].Cells["单号"].Value == info)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                    break;
                }
            }
        }

        /// <summary>
        /// 刷新
        /// </summary>
        private void DataRefresh()
        {
            DataTable dt = new DataTable();

            dt = m_buyingServer.GetAllBillInfo(cmbBillStatus.Text, dtpStartTime.Value, dtpEndTime.Value);

            if (dt != null)
            {
                dataGridView1.DataSource = dt;
            }

            userControlDataLocalizer1.Init(dataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }
            else
            {
                string strBillNo = dataGridView1.CurrentRow.Cells["单号"].Value.ToString();

                挂账单明细 frm = new 挂账单明细(strBillNo, m_authFlag);

                frm.ShowDialog();

                DataRefresh();
                PositioningRecord(strBillNo);
            }
        }

        private void 挂账单_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (dtpEndTime.Value < dtpStartTime.Value)
            {
                MessageDialog.ShowPromptMessage("请重新选择时间！");
                return;
            }

            DataRefresh();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            挂账单明细 frm = new 挂账单明细("", m_authFlag);

            frm.ShowDialog();
        }
    }
}
