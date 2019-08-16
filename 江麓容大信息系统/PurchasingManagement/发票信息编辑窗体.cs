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
    public partial class 发票信息编辑窗体 : Form
    {
        /// <summary>
        /// 服务组件
        /// </summary>
        IAccountOperation _Service_Account = Service_Economic_Purchase.ServerModuleFactory.GetServerModule<IAccountOperation>();

        /// <summary>
        /// 单据号
        /// </summary>
        string m_billNo;

        private List<Bus_PurchasingMG_AccountBill_Invoice> _List_Invoice = new List<Bus_PurchasingMG_AccountBill_Invoice>();

        public List<Bus_PurchasingMG_AccountBill_Invoice> ListInvoice
        {
            get { return _List_Invoice; }
            set { _List_Invoice = value; }
        }

        public 发票信息编辑窗体(string billNo, List<Bus_PurchasingMG_AccountBill_Invoice> lstInvoice)
        {
            InitializeComponent();

            m_billNo = billNo;
            RefreshDataGridView(lstInvoice);
        }

        void RefreshDataGridView(List<Bus_PurchasingMG_AccountBill_Invoice> source)
        {
            if (source != null)
            {
                customDataGridView1.Rows.Clear();
                foreach (Bus_PurchasingMG_AccountBill_Invoice item in source)
                {
                    customDataGridView1.Rows.Add(new object[] { item.InvoiceNo, item.InvoiceTime, item.BillNo});
                }
            }
        }

        private void 添加ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DataGridViewRow dr = new DataGridViewRow();
            customDataGridView1.Rows.Add(dr);
        }

        private void 删除ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dgvr in customDataGridView1.SelectedRows)
            {
                customDataGridView1.Rows.Remove(dgvr);
            }

            DataTable dtInvoice = (DataTable)customDataGridView1.DataSource;
        }

        private void 发票信息编辑窗体_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("是否要保存？","提示信息",MessageBoxButtons.YesNoCancel);

            if (customDataGridView1.Rows.Count != 0)
            {
                if (result == DialogResult.Yes)
                {
                    customDataGridView1.EndEdit();

                    foreach (DataGridViewRow dgvr in customDataGridView1.Rows)
                    {
                        DateTime dateTemp;

                        if (!DateTime.TryParse(dgvr.Cells["发票日期"].Value == null ? "" : dgvr.Cells["发票日期"].Value.ToString(), out dateTemp))
                        {
                            continue;
                        }

                        Bus_PurchasingMG_AccountBill_Invoice lnqTemp = new Bus_PurchasingMG_AccountBill_Invoice();

                        lnqTemp.BillNo = m_billNo;
                        lnqTemp.InvoiceTime = dateTemp;
                        lnqTemp.InvoiceNo = dgvr.Cells["发票号"].Value.ToString();

                        _List_Invoice.Add(lnqTemp);
                    }
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
                else if (result == DialogResult.No)
                {
                    _List_Invoice = null;
                }
            }
            else
            {
                _List_Invoice = null;
            }
        }
    }
}
