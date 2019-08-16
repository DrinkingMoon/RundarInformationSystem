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

namespace Expression
{
    /// <summary>
    /// 订单核查界面
    /// </summary>
    public partial class 订单核查 : Form
    {
        /// <summary>
        /// 服务
        /// </summary>
        IOrderFormAffrim m_orderFormAffrim = ServerModuleFactory.GetServerModule<IOrderFormAffrim>();

        /// <summary>
        /// 数据定位控件
        /// </summary>
        UserControlDataLocalizer m_dataLocalizer;

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag;

        public 订单核查(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();
            m_authFlag = nodeInfo.Authority;
            
            dateTimePicker1.Value = ServerTime.Time.AddMonths(-12);
            dateTimePicker2.Value = ServerTime.Time.AddDays(1);
            comboBox1.SelectedIndex = 0;

            RefreshDataGirdView();
        }

        private void 订单核查_Load(object sender, EventArgs e)
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
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="source">数据集</param>
        void RefreshDataGirdView()
        {
            DataTable dt = new DataTable();

            dt = m_orderFormAffrim.GetAllInfo(dateTimePicker1.Value, dateTimePicker2.Value, comboBox1.Text);

            dataGridView1.DataSource = dt;

            if (m_dataLocalizer == null)
            {
                m_dataLocalizer = new UserControlDataLocalizer(dataGridView1, this.Name, 
                    UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));

                m_dataLocalizer.OnlyLocalize = true;
                panelPara.Controls.Add(m_dataLocalizer);
                m_dataLocalizer.Dock = DockStyle.Bottom;
            }
        }

        private void 设置订单信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }

            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() != "单据已完成")
            {
                订单核查清单 form = new 订单核查清单(dataGridView1.CurrentRow.Cells["年月"].Value.ToString(),
                    m_authFlag, true);
                form.ShowDialog();
            }
            else
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
            }

            RefreshDataGirdView();

        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }

            订单核查清单 form = new 订单核查清单(dataGridView1.CurrentRow.Cells["年月"].Value.ToString(),
                m_authFlag, false);
            form.ShowDialog();
            RefreshDataGirdView();
        }

        private void 刷新数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshDataGirdView();
        }

        private void 审核通过ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }
            else
            {
                if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "等待审核")
                {
                    if (!m_orderFormAffrim.UpdateBill(dataGridView1.CurrentRow.Cells["年月"].Value.ToString(), out m_err))
                    {
                        MessageDialog.ShowPromptMessage(m_err);
                    }
                    else
                    {
                        MessageDialog.ShowPromptMessage("审核成功");
                    }
                }
                else
                {
                    MessageDialog.ShowPromptMessage("请重新确认单据状态");
                }

                RefreshDataGirdView();
            }
        }

        private void 发布订单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }
            else
            {
                if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "等待发布")
                {
                    if (!m_orderFormAffrim.UpdateBill(dataGridView1.CurrentRow.Cells["年月"].Value.ToString(), out m_err))
                    {
                        MessageDialog.ShowPromptMessage(m_err);
                    }
                    else
                    {
                        MessageDialog.ShowPromptMessage("发布成功");
                    }
                }
                else
                {
                    MessageDialog.ShowPromptMessage("请重新确认单据状态");
                }

                RefreshDataGirdView();
            }
        }

        private void 删除单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }
            else
            {
                if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "单据已完成")
                {
                    if (!m_orderFormAffrim.DeleteAllInfo(dataGridView1.CurrentRow.Cells["年月"].Value.ToString(), out m_err))
                    {
                        MessageDialog.ShowPromptMessage(m_err);
                    }
                    else
                    {
                        MessageDialog.ShowPromptMessage("删除成功");
                    }
                }
                else
                {
                    MessageDialog.ShowPromptMessage("请重新确认单据状态");
                }

                RefreshDataGirdView();
            }
        }

        private void 导出ExcleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
        }

        private void 新建单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            订单核查清单 form = new 订单核查清单("", m_authFlag, true);
            form.ShowDialog();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            RefreshDataGirdView();
        }
    }
}
