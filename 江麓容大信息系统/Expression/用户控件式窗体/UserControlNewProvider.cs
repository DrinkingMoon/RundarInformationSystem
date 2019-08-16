/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  UserControlNewProvider.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2009/06/15
 * 开发平台:  vs2005(c#)
 * 用于    :  生产线管理信息系统
 *----------------------------------------------------------------------------
 * 描述 : 电子档案界面
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
using GlobalObject;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 新开发供应商组件
    /// </summary>
    public partial class UserControlNewProvider : Form
    {
        #region variants

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 查找到的符合条件的供应商信息
        /// </summary>
        IQueryable<View_B_NewProvider> m_findProvider;

        /// <summary>
        /// 供应商服务组件
        /// </summary>
        IProviderServer m_providerServer = ServerModuleFactory.GetServerModule<IProviderServer>();

        /// <summary>
        /// 数据定位控件
        /// </summary>
        UserControlDataLocalizer m_dataLocalizer;

        #endregion

        public UserControlNewProvider()
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
        private void UserControlNewProvider_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
            panelCenter.Location = new Point((this.Width - panelCenter.Width) / 2, panelCenter.Location.Y);
        }

        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControlNewProvider_Load(object sender, EventArgs e)
        {
            if (!m_providerServer.GetAllNewProvider(out m_findProvider, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            RefreshDataGridView(m_findProvider);
            int[] colWidths = { 120, 240, 140 };

            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                dataGridView1.Columns[i].Width = colWidths[i];
            }

            txtCode.Focus();

            RefreshControl();
        }

        /// <summary>
        /// 刷新DataGridView
        /// </summary>
        /// <param name="findStore"></param>
        void RefreshDataGridView(IQueryable findDepartmentBill)
        {
            dataGridView1.DataSource = findDepartmentBill;

            // 添加数据定位控件
            if (m_dataLocalizer == null)
            {
                m_dataLocalizer = new UserControlDataLocalizer(dataGridView1, this.Name, 
                    UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));

                panelPara.Controls.Add(m_dataLocalizer);
                m_dataLocalizer.Dock = DockStyle.Bottom;
                m_dataLocalizer.Visible = true;
            }

            this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            dataGridView1.Refresh();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!CheckDataItem())
            {
                return;
            }

            if (!m_providerServer.AddNewProvider(txtCode.Text, txtName.Text, txtRemark.Text, out m_findProvider, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            RefreshDataGridView(m_findProvider);
            RefreshControl();
        }

        /// <summary>
        /// 检测是否允许添加/修改供应商
        /// </summary> 
        bool CheckDataItem()
        {
            if (txtCode.Text == "")
            {
                MessageDialog.ShowPromptMessage("供应商编码不能为空!");
                return false;
            }

            if (txtName.Text == "")
            {
                MessageDialog.ShowPromptMessage("供应商名称不能为空!");
                return false;
            }


            return true;
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
                string providerCode = dataGridView1.CurrentRow.Cells[0].Value.ToString();

                if (MessageBox.Show("您是否确定要删除新开发供应商" + providerCode + "信息?", "消息", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (!m_providerServer.DeleteNewProvider(providerCode, out m_findProvider, out m_err))
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
                string[] arrayCode = new string[n];

                for (int i = 0; i < n; i++)
                {
                    arrayCode[i] = dataGridView1.SelectedRows[i].Cells[0].Value.ToString();
                }

                if (MessageBox.Show("您是否确定要删除新开发供应商信息?", "消息", MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    for (int i = 0; i < arrayCode.Length; i++)
                    {
                        if (!m_providerServer.DeleteNewProvider(arrayCode[i], out m_findProvider, out m_err))
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

            RefreshDataGridView(m_findProvider);

            RefreshControl();
        }

        /// <summary>
        /// 重置右面板参数
        /// </summary>
        void ClearControl()
        {
            txtCode.Text = "";
            txtName.Text = "";
            txtRemark.Text = "";
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string providerCode = dataGridView1.Rows[0].Cells[0].Value.ToString();

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
                    providerCode = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                }
            }

            if (providerCode != txtCode.Text)
            {
                MessageDialog.ShowPromptMessage("供应商编码不允许修改!");
                txtCode.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                return;
            }

            if (!m_providerServer.UpdataNewProvider(txtCode.Text, txtName.Text, txtRemark.Text, out m_findProvider, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            RefreshDataGridView(m_findProvider);
            RefreshControl();
        }

        /// <summary>
        /// dataGridView1单元格获取焦点事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            RefreshControl();
        }

        /// <summary>
        /// 刷新控件
        /// </summary>
        void RefreshControl()
        {
            ClearControl();

            if (dataGridView1.CurrentRow != null)
            {
                txtCode.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                txtName.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();

                if (dataGridView1.CurrentRow.Cells[2].Value != null)
                {
                    txtRemark.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                }
            }
            else if (dataGridView1.Rows.Count > 0)
            {
                txtCode.Text = dataGridView1.Rows[0].Cells[0].Value.ToString();
                txtName.Text = dataGridView1.Rows[0].Cells[1].Value.ToString();

                if (dataGridView1.CurrentRow.Cells[2].Value != null)
                {
                    txtRemark.Text = dataGridView1.Rows[0].Cells[2].Value.ToString();
                }
            }
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
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
