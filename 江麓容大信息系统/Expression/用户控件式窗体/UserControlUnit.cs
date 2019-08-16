/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  UserControlUnit.cs
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
using PlatformManagement;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 单位组件
    /// </summary>
    public partial class UserControlUnit : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 查找到的符合条件的单位信息
        /// </summary>
        IQueryable<View_S_Unit> m_findUnit;

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authorityFlag;

        /// <summary>
        /// 服务组件
        /// </summary>
        IUnitServer m_unitServer = ServerModuleFactory.GetServerModule<IUnitServer>();

        public UserControlUnit(FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authorityFlag = nodeInfo.Authority;

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
        }

        /// <summary>
        /// 改变组件大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControlUnit_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControlUnit_Load(object sender, EventArgs e)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip1, m_authorityFlag);
            btnRefresh_Click(sender, e);
        }

        /// <summary>
        /// 刷新DataGridView
        /// </summary>
        /// <param name="findStore"></param>
        void RefreshDataGridView(IQueryable findUnit)
        {
            dataGridView1.DataSource = findUnit;
            dataGridView1.Refresh();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            ClearControl();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtUnit.Text == "")
            {
                MessageDialog.ShowErrorMessage("单位不允许为空");
                return;
            }

            string spec = null;

            if (txtSpec.Text.Trim().Length > 0)
            {
                spec = txtSpec.Text.Trim();
            }

            if (!m_unitServer.AddUnit(txtUnit.Text.Trim(), spec, chbIsDisable.Checked, out m_findUnit, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            RefreshDataGridView(m_findUnit);
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
                int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);

                if (MessageBox.Show("您是否确定要删除单位" + dataGridView1.CurrentRow.Cells[1].Value.ToString() + "信息?", "消息", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (!m_unitServer.DeleteUnit(id, out m_findUnit, out m_err))
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
                int[] arrayID = new int[n];

                for (int i = 0; i < n; i++)
                {
                    arrayID[i] = Convert.ToInt32(dataGridView1.SelectedRows[i].Cells[0].Value);
                }

                if (MessageBox.Show("您是否确定要删除单位信息?", "消息", MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    for (int i = 0; i < arrayID.Length; i++)
                    {
                        if (!m_unitServer.DeleteUnit(arrayID[i], out m_findUnit, out m_err))
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

            RefreshDataGridView(m_findUnit);
            RefreshControl();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(dataGridView1.Rows[0].Cells[0].Value);

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
                    id = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
                }
            }

            if (txtUnit.Text == "")
            {
                MessageDialog.ShowErrorMessage("单位不允许为空");
                return;
            }

            if (!m_unitServer.UpdateUnit(id, txtUnit.Text.Trim(), txtSpec.Text.Trim(), chbIsDisable.Checked, out m_findUnit, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            RefreshDataGridView(m_findUnit);
            RefreshControl();
        }

        /// <summary>
        /// 清空控件值
        /// </summary>
        void ClearControl()
        {
            txtUnit.Text = "";
            txtSpec.Text = "";
        }

        /// <summary>
        /// 刷新控件
        /// </summary>
        void RefreshControl()
        {
            ClearControl();

            if (dataGridView1.CurrentRow != null)
            {
                txtSpec.Text = dataGridView1.CurrentRow.Cells["规格"].Value.ToString();
                txtUnit.Text = dataGridView1.CurrentRow.Cells["单位"].Value.ToString();
                chbIsDisable.Checked = (bool)dataGridView1.CurrentRow.Cells["停用"].Value;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (!m_unitServer.GetAllUnit(out m_findUnit, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            RefreshDataGridView(m_findUnit);

            dataGridView1.Columns[0].Visible = false;
            RefreshControl();
            txtUnit.Focus();
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            RefreshControl();
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
