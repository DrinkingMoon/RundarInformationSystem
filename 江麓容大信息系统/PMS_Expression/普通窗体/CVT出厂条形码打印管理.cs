using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using PlatformManagement;
using UniversalControlLibrary;

namespace Expression
{
    public partial class CVT出厂条形码打印管理 : Form
    {
        /// <summary>
        /// 产品条形码服务接口
        /// </summary>
        IProductBarcodeServer m_productBarcodeServer = ServerModuleFactory.GetServerModule<IProductBarcodeServer>();
                
        /// <summary>
        /// 授权标志
        /// </summary>
        AuthorityFlag m_authorityFlag;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="authFlag">授权标志</param>
        public CVT出厂条形码打印管理(AuthorityFlag authFlag)
        {
            InitializeComponent();

            m_authorityFlag = authFlag;

            dateTimePicker1.Value = DateTime.Now.Date.AddDays(-31);
            dateTimePicker2.Value = DateTime.Now.Date.AddDays(1);

            RefreshDataGridView();
        }

        /// <summary>
        /// 窗体加载
        /// 在构造函数中工具栏始终为不可见，需要在此处设置（原因不明）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CVT出厂条形码打印管理_Load(object sender, EventArgs e)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip1, m_authorityFlag);
        }
                
        /// <summary>
        /// 刷新DataGridView
        /// </summary>
        void RefreshDataGridView()
        {
            if (rbtnPrintSetting.Checked)
            {
                IQueryable<View_P_PrintBillForVehicleBarcode> data = m_productBarcodeServer.GetPrintSetting(
                 dateTimePicker1.Value, dateTimePicker2.Value);

                if (cmbFindCondition.Text == "未打印")
                {
                    data = from r in data
                           where r.是否已经打印 == false
                           select r;
                }
                else if (cmbFindCondition.Text == "已打印")
                {
                    data = from r in data
                           where r.是否已经打印 == true
                           select r;
                }

                dataGridView1.DataSource = data;
            }
            else
            {
                IQueryable<View_P_PrintLogForVehicleBarcode> data = m_productBarcodeServer.GetPrintLog(
                dateTimePicker1.Value, dateTimePicker2.Value);

                dataGridView1.DataSource = data;
            }

            this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);
        }

        /// <summary>
        /// 检查是否正确选择操作的记录行
        /// </summary>
        /// <returns>正确返回true</returns>
        bool CheckSelectedRow()
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择一条记录后再进行此操作");
                return false;
            }
            else if (dataGridView1.SelectedRows.Count > 1)
            {
                MessageDialog.ShowPromptMessage("只允许选择一条记录进行处理！");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 打印整车对应产品总成条形码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (!rbtnPrintSetting.Checked)
            {
                return;
            }

            if (!CheckSelectedRow())
            {
                return;
            }

            打印CVT出厂条形码 dialog = new 打印CVT出厂条形码(m_authorityFlag,
                m_productBarcodeServer.GetPrintSetting(Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["打印编号"].Value)));

            dialog.Show();

            RefreshDataGridView();
        }

        /// <summary>
        /// 新建条形码打印信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNew_Click(object sender, EventArgs e)
        {
            打印CVT出厂条形码 dialog = new 打印CVT出厂条形码(m_authorityFlag, null);
            dialog.Show();
        }

        private void CVT出厂条形码打印管理_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshDataGridView();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
            {
                return;
            }

            if (dataGridView1.SelectedRows[0].Cells["工号"].Value.ToString() != GlobalObject.BasicInfo.LoginID)
            {
                MessageDialog.ShowPromptMessage("您不是该行记录的设置人员，不允许删除此数据项");
                return;
            }

            if ((bool)dataGridView1.SelectedRows[0].Cells["是否已经打印"].Value)
            {
                MessageDialog.ShowPromptMessage("不允许删除已经打印过的数据项");
                return;
            }

            string error;

            if (m_productBarcodeServer.DeletePrintSetting((int)dataGridView1.SelectedRows[0].Cells["打印编号"].Value, out error))
            {
                RefreshDataGridView();
            }
            else
            {
                MessageDialog.ShowErrorMessage(error);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefreshDataGridView();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnPrint_Click(sender, e);
        }

        private void rbtnPrintSetting_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnPrintSetting.Checked)
            {
                btnDelete.Enabled = true;
                btnPrint.Enabled = true;
                cmbFindCondition.Enabled = true;
            }
            else
            {
                btnDelete.Enabled = false;
                btnPrint.Enabled = false;
                cmbFindCondition.Enabled = false;
            }

            RefreshDataGridView();
        }
    }
}
