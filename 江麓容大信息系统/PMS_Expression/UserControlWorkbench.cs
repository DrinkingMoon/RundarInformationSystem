/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  UserControlWorkbench.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2009/06/15
 * 开发平台:  vs2005(c#)
 * 用于    :  生产线管理信息系统
 *----------------------------------------------------------------------------
 * 描述 : 供应商信息界面
 * 其它 :
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2009/07/03 08:02:08 作者: 夏石友 当前版本: V1.00
 *        修改说明: 创建
 ******************************************************************************/
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
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 工位组件
    /// </summary>
    public partial class UserControlWorkbench : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 查找到的符合条件的单位信息
        /// </summary>
        IQueryable<View_P_Workbench> m_findWorkbench;

        /// <summary>
        /// 工位服务组件
        /// </summary>
        IWorkbenchService m_workbenchServer = ServerModuleFactory.GetServerModule<IWorkbenchService>();

        public UserControlWorkbench()
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
        }

        /// <summary>
        /// 改变组件大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControlWorkbench_Resize(object sender, EventArgs e)
        {
            panelLeft.Width = (this.Width - panelCenter.Width) / 2;
            panelRight.Width = this.Width - panelCenter.Width - panelLeft.Width;
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControlWorkbench_Load(object sender, EventArgs e)
        {
            m_findWorkbench = m_workbenchServer.Workbenchs;

            RefreshDataGridView(m_findWorkbench);

            RefreshControl();
            
            txtWorkbench.Focus();
        }

        /// <summary>
        /// 刷新DataGridView
        /// </summary>
        /// <param name="findStore"></param>
        void RefreshDataGridView(IQueryable findWorkbench)
        {
            dataGridView1.DataSource = findWorkbench;
            dataGridView1.Refresh();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!m_workbenchServer.Add(txtWorkbench.Text, "", out m_findWorkbench, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            RefreshDataGridView(m_findWorkbench);
            RefreshControl();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            int n = dataGridView1.SelectedRows.Count;

            if (n == 0)
            {
                MessageDialog.ShowPromptMessage("请选择需要删除的数据行!");
            }
            else if (n == 1)
            {
                string workbench = dataGridView1.CurrentRow.Cells[0].Value.ToString();

                if (MessageBox.Show("您是否确定要删除工位" + dataGridView1.CurrentRow.Cells[0].Value.ToString() + "信息?", "消息", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (!m_workbenchServer.Delete(workbench, out m_findWorkbench, out m_err))
                    {
                        MessageDialog.ShowPromptMessage(m_err);
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                string[] arrayWorkbench = new string[n];

                for (int i = 0; i < n; i++)
                {
                    arrayWorkbench[i] = dataGridView1.SelectedRows[i].Cells[0].Value.ToString();
                }

                if (MessageBox.Show("您是否确定要删除工位信息?", "消息", MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    for (int i = 0; i < arrayWorkbench.Length; i++)
                    {
                        if (!m_workbenchServer.Delete(arrayWorkbench[i], out m_findWorkbench, out m_err))
                        {
                            MessageDialog.ShowPromptMessage(m_err);
                        }
                    }
                }
                else
                {
                    return;
                }
            }

            RefreshDataGridView(m_findWorkbench);
            RefreshControl();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string oldWorkbench = dataGridView1.Rows[0].Cells[0].Value.ToString();

            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择需要修改的数据行!");
                return;
            }
            else if (dataGridView1.SelectedRows.Count > 1)
            {
                MessageDialog.ShowPromptMessage("系统不允许同时修改多行数据!");
                return;
            }
            else
            {
                if (dataGridView1.CurrentRow != null)
                {
                    oldWorkbench = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                }
            }

            if (m_workbenchServer.Update(oldWorkbench, txtWorkbench.Text, "", out m_findWorkbench, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            RefreshDataGridView(m_findWorkbench);
            RefreshControl();
        }

        /// <summary>
        /// 点击DataGridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            RefreshControl();
        }

        /// <summary>
        /// 清空控件值
        /// </summary>
        void ClearControl()
        {
            txtWorkbench.Text = "";
        }

        /// <summary>
        /// 刷新控件
        /// </summary>
        void RefreshControl()
        {
            ClearControl();

            if (dataGridView1.CurrentRow != null)
            {
                txtWorkbench.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            }
            else if (dataGridView1.Rows.Count > 0)
            {
                txtWorkbench.Text = dataGridView1.Rows[0].Cells[0].Value.ToString();
            }
        }
    }
}
