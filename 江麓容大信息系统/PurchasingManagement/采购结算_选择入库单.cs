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

namespace Form_Economic_Purchase
{
    public partial class 采购结算_选择入库单 : Form
    {
        /// <summary>
        /// 服务组件
        /// </summary>
        IProcurementStatement m_serverStatement = Service_Economic_Purchase.ServerModuleFactory.GetServerModule<IProcurementStatement>();

        /// <summary>
        /// 供应商代码
        /// </summary>
        string m_strProvider = "";

        /// <summary>
        /// 单据号列表
        /// </summary>
        private List<string> m_lstBillNo = new List<string>();

        public List<string> LstBillNo
        {
            get { return m_lstBillNo; }
            set { m_lstBillNo = value; }
        }

        public 采购结算_选择入库单(string provider)
        {
            InitializeComponent();

            m_strProvider = provider;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            customDataGridView1.DataSource = m_serverStatement.LeadInInPutBillInfo(m_strProvider, dateTimePicker1.Value, dateTimePicker2.Value);
            userControlDataLocalizer1.Init(customDataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, customDataGridView1.Name, BasicInfo.LoginID));
        }

        private void 采购结算_选择入库单_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("是否确定？", "提示信息", MessageBoxButtons.YesNoCancel);

            if (customDataGridView1.Rows.Count != 0)
            {
                if (result == DialogResult.Yes)
                {
                    foreach (DataGridViewRow dgvr in customDataGridView1.Rows)
                    {
                        if (Convert.ToBoolean(dgvr.Cells["选"].Value))
                        {
                            m_lstBillNo.Add(dgvr.Cells["入库单号"].Value.ToString());
                        }
                    }

                    if (m_lstBillNo.Count == 0)
                    {
                        m_lstBillNo = null;
                    }
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
                else if (result == DialogResult.No)
                {
                    m_lstBillNo = null;
                }
            }
            else
            {
                m_lstBillNo = null;
            }
        }

        private void customDataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (customDataGridView1.CurrentRow == null)
            {
                return;
            }
            else
            {
                if (customDataGridView1.Columns[e.ColumnIndex].Name == "选")
                {
                    customDataGridView1.Rows[e.RowIndex].Cells["选"].Value = !Convert.ToBoolean(customDataGridView1.Rows[e.RowIndex].Cells["选"].Value);
                }
            }
        }

        private void customDataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            ServerModule.IInvoiceServer serviceInvoice = ServerModule.ServerModuleFactory.GetServerModule<ServerModule.IInvoiceServer>();

            if (customDataGridView1.CurrentRow == null)
            {
                return;
            }

            customDataGridView2.DataSource = 
                serviceInvoice.GetGoodsInfo(customDataGridView1.CurrentRow.Cells["入库单号"].Value.ToString());
        }

        private void btnIntegrativeQuery_Click(object sender, EventArgs e)
        {
            IAuthorization authorization = PlatformFactory.GetObject<IAuthorization>();
            string businessID = "订单入库单综合查询";
            string[] pare = { m_strProvider, dateTimePicker1.Value.ToShortDateString(), dateTimePicker2.Value.ToShortDateString() };
            IQueryResult qr = authorization.QueryMultParam(businessID, pare);
            List<string> lstFindField = new List<string>();
            DataColumnCollection columns = qr.DataCollection.Tables[0].Columns;

            if (qr.Succeeded && columns.Count > 0)
            {
                for (int i = 0; i < columns.Count; i++)
                {
                    lstFindField.Add(columns[i].ColumnName);
                }
            }

            FormConditionFind formFindCondition = new FormConditionFind(this, lstFindField.ToArray(), businessID, "订单入库单综合查询" , pare);
            formFindCondition.ShowDialog();
        }

        private void 选择ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dgrv in customDataGridView1.SelectedRows)
            {
                dgrv.Cells["选"].Value = true;
            }
        }

        private void 取消ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dgrv in customDataGridView1.SelectedRows)
            {
                dgrv.Cells["选"].Value = false;
            }
        }

        private void 全选ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dgrv in customDataGridView1.Rows)
            {
                dgrv.Cells["选"].Value = true;
            }
        }

        private void 全消ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dgrv in customDataGridView1.Rows)
            {
                dgrv.Cells["选"].Value = false;
            }
        }
    }
}
