using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using ServerModule;
using GlobalObject;
using System.Data.OleDb;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 单价初始化界面
    /// </summary>
    public partial class 单价初始化 : Form
    {
        /// <summary>
        /// 库存服务组件
        /// </summary>
        IStoreServer m_serverStore = ServerModuleFactory.GetServerModule<IStoreServer>();

        /// <summary>
        /// 数据定位控件
        /// </summary>
        UserControlDataLocalizer m_dataLocalizer;

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 数据表
        /// </summary>
        bool  m_blFlag = false;

        /// <summary>
        /// 权限组件
        /// </summary>
        AuthorityFlag m_authFlag;

        public 单价初始化(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authFlag = nodeInfo.Authority;

            RefreshDataGirdView(m_serverStore.GetStockAveragePrice(true, ""));
            InsertCombox();

            cmbMonth.Text = ServerTime.Time.Month.ToString("D2");
            cmbYear.Text = ServerTime.Time.Year.ToString("D4");
        }

        private void 单价初始化_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authFlag);
        }

        /// <summary>
        /// 权限控制
        /// </summary>
        /// <param name="authorityFlag">权限标志</param>
        void AuthorityControl(PlatformManagement.AuthorityFlag authorityFlag)
        {
            FaceAuthoritySetting.SetVisibly(menuStrip1, authorityFlag);
            FaceAuthoritySetting.SetEnable(this.Controls, authorityFlag);
        }

        /// <summary>
        /// COMBOX 插入数据
        /// </summary>
        void InsertCombox()
        {
            for (int i = 2011; i < 2050; i++)
            {
                cmbYear.Items.Add(i);
            }

            for (int f = 1; f <= 12; f++)
            {
                cmbMonth.Items.Add(f.ToString("D2"));
            }
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="source">数据集</param>
        void RefreshDataGirdView(DataTable source)
        {
            dataGridView1.DataSource = source;

            if (m_dataLocalizer == null)
            {
                m_dataLocalizer = new UserControlDataLocalizer(dataGridView1, this.Name, 
                    UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));

                m_dataLocalizer.OnlyLocalize = true;
                panel2.Controls.Add(m_dataLocalizer);
                m_dataLocalizer.Dock = DockStyle.Bottom;
            }
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="billNo">定位用的单据号</param>
        void PositioningRecord(string billNo)
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
                if ((string)dataGridView1.Rows[i].Cells["物品ID"].Value.ToString() == billNo)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                    break;
                }
            }
        }

        /// <summary>
        /// 检测信息
        /// </summary>
        void CheckMessage()
        {
            if (!rdbNowStockUnitPrice.Checked && cmbYear.Text + cmbMonth.Text == "")
            {
                MessageDialog.ShowPromptMessage("请选择具体年月,再进行操作！");
            }
        }

        void txtName_OnCompleteSearch()
        {
            txtName.Text = txtName.DataResult["物品名称"].ToString();
            txtCode.Text = txtName.DataResult["图号型号"].ToString();
            txtSpec.Text = txtName.DataResult["规格"].ToString();
            txtName.Tag = Convert.ToInt32(txtName.DataResult["物品ID"].ToString());
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                txtName.Text = dataGridView1.CurrentRow.Cells["物品名称"].Value.ToString();
                txtCode.Text = dataGridView1.CurrentRow.Cells["图号型号"].Value.ToString();
                txtSpec.Text = dataGridView1.CurrentRow.Cells["规格"].Value.ToString();
                numPrice.Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["物品实际平均价"].Value);
                txtName.Tag = Convert.ToInt32(dataGridView1.CurrentRow.Cells["物品ID"].Value);
            }
        }

        private void 物品平均价校对ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckMessage();

            int intGoodsID = Convert.ToInt32(txtName.Tag);
            
            if (!m_blFlag)
            {
                if (txtName.Tag == null || txtName.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请选择物品");
                }
                else
                {
                    if (!m_serverStore.ChangeAveragePrice(Convert.ToInt32(txtName.Tag), numPrice.Value, 
                        rdbNowStockUnitPrice.Checked,
                        cmbYear.Text + cmbMonth.Text, out m_err))
                    {
                        MessageDialog.ShowPromptMessage(m_err);
                    }
                    else
                    {
                        MessageDialog.ShowPromptMessage("校对成功");
                    }
                }
            }
            else
            {
                DataTable dtAverageDate = (DataTable)dataGridView1.DataSource;

                for (int i = 0; i < dtAverageDate.Rows.Count; i++)
                {
                    if (!m_serverStore.ChangeAveragePrice(Convert.ToInt32(dtAverageDate.Rows[i]["物品ID"]),
                        Convert.ToDecimal(dtAverageDate.Rows[i]["物品实际平均价"]), rdbNowStockUnitPrice.Checked,
                        cmbYear.Text + cmbMonth.Text, out m_err))
                    {
                        MessageDialog.ShowPromptMessage(m_err);
                    }
                }

                MessageDialog.ShowPromptMessage("校对成功");
                m_blFlag = false;
            }


            RefreshDataGirdView(m_serverStore.GetStockAveragePrice(rdbNowStockUnitPrice.Checked, cmbYear.Text + cmbMonth.Text));

            if (txtName.Tag != null && txtName.Tag.ToString() != "")
            {
                PositioningRecord(intGoodsID.ToString());
            }
        }

        private void 导出EXCELToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
        }

        private void 导入EXCELToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataTable dtTemp = ExcelHelperP.RenderFromExcel(openFileDialog1);

            if (dtTemp == null)
            {
                //MessageDialog.ShowPromptMessage(m_err);
                return;
            }

            if (dtTemp.Rows.Count == 0 ||
                dtTemp.Columns[0].ColumnName != "物品ID" ||
                dtTemp.Columns[1].ColumnName != "图号型号" ||
                dtTemp.Columns[2].ColumnName != "物品名称" ||
                dtTemp.Columns[3].ColumnName != "规格" ||
                dtTemp.Columns[4].ColumnName != "物品实际平均价")
            {
                MessageDialog.ShowPromptMessage(string.Format("{0} 中没有包含所需的信息，无法导入！",
                    openFileDialog1.FileName));
            }
            else
            {
                m_blFlag = true;
                dataGridView1.DataSource = dtTemp;
                MessageDialog.ShowPromptMessage("导入成功请点击【物品平均价校对】进行校对");
            }
        }

        private void 刷新数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckMessage();
            RefreshDataGirdView(m_serverStore.GetStockAveragePrice(rdbNowStockUnitPrice.Checked, cmbYear.Text + cmbMonth.Text));
        }

        private void rdbNowStockUnitPrice_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbNowStockUnitPrice.Checked)
            {
                cmbMonth.Enabled = false;
                cmbYear.Enabled = false;
                cmbYear.Text = "";
                cmbMonth.Text = "";
            }
            else
            {
                cmbMonth.Enabled = true;
                cmbYear.Enabled = true;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            CheckMessage();
            RefreshDataGirdView(m_serverStore.GetStockAveragePrice(rdbNowStockUnitPrice.Checked, cmbYear.Text + cmbMonth.Text));
        }

        private void cmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckMessage();
            RefreshDataGirdView(m_serverStore.GetStockAveragePrice(rdbNowStockUnitPrice.Checked, cmbYear.Text + cmbMonth.Text));
        }

        private void cmbMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckMessage();
            RefreshDataGirdView(m_serverStore.GetStockAveragePrice(rdbNowStockUnitPrice.Checked, cmbYear.Text + cmbMonth.Text));
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
    }
}
