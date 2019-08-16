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
    /// 打印单据查询界面
    /// </summary>
    public partial class 打印单据查询 : Form
    {
        /// <summary>
        /// 打印单据服务组件
        /// </summary>
        IPrintManagement m_printManagement = BasicServerFactory.GetServerModule<IPrintManagement>();

        /// <summary>
        /// 数据定位控件
        /// </summary>
        UserControlDataLocalizer m_dataLocalizer;

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        public 打印单据查询()
        {
            InitializeComponent();

            cmbDept.Items.AddRange(m_printManagement.GetReceivedDeptOfPrintBill().ToArray());

            if (!BasicInfo.ListRoles.Contains(CE_RoleEnum.会计.ToString()))
            {
                审核ToolStripMenuItem.Enabled = false;
                取消审核ToolStripMenuItem.Enabled = false;
            }
        }

        private void 打印单据查询_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        private void ShowDate(DateTime dtStart,DateTime dtEnd)
        {
            try
            {
                DataTable dt = new DataTable();
                IQueryable<View_S_PrintBill> printInfo = m_printManagement.GetPrintInfo(dtStart, dtEnd, cmbDept.Text);

                if (printInfo.Count() > 0)
                {
                    dataGridView1.DataSource = GlobalObject.GeneralFunction.ConvertToDataTable<View_S_PrintBill>(printInfo);
                }
                else
                {
                    dataGridView1.DataSource = null;
                }

                if (dataGridView1.Rows.Count > 0)
                {
                    if (m_dataLocalizer == null)
                    {
                        m_dataLocalizer = new UserControlDataLocalizer(dataGridView1, this.Name, 
                            UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
                        panelTop.Controls.Add(m_dataLocalizer);
                        m_dataLocalizer.Dock = DockStyle.Bottom;
                    }

                    panelTop.Visible = true;

                    this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                        this.dataGridView1_ColumnWidthChanged);

                    ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

                    this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                        this.dataGridView1_ColumnWidthChanged);
                }
                else
                {
                    panelTop.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowErrorMessage(ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ShowDate(DtpStartDate.Value.Date, DtpEndDate.Value.Date);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            报表_单据打印表 report = new 报表_单据打印表(DtpStartDate.Value.Date, DtpEndDate.Value.Date, cmbDept.Text);
            report.ShowDialog();
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(this.labelTitle.Text, e.Column);
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

        private void 审核ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckMode(true,"审核成功！");
        }

        private void 取消审核ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckMode(false,"取消审核成功！");
        }

        /// <summary>
        /// 修改审核状态
        /// </summary>
        /// <param name="blStatus">单据状态</param>
        /// <param name="strMessage">单据信息</param>
        private void CheckMode(bool blStatus,string strMessage)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("DJH");

            foreach (DataGridViewRow dvr in dataGridView1.SelectedRows)
            {
                DataRow dr = dt.NewRow();
                dr[0] = dvr.Cells["单据编号"].Value.ToString();
                dt.Rows.Add(dr);
            }

            if (m_printManagement.SetChecked(dt, blStatus,out m_err))
            {
                MessageBox.Show(strMessage, "提示");
            }
            else
            {
                MessageDialog.ShowErrorMessage(m_err);
            }

            ShowDate(DtpStartDate.Value.Date, DtpEndDate.Value.Date);
        }
    }
}
