/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  UserControlGoodsGrade.cs
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
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// AB类零件信息表组件
    /// </summary>
    public partial class UserControlGoodsGrade : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 查找到的符合条件的单位信息
        /// </summary>
        IQueryable<View_Q_GoodsGradeTable> m_findTable;

        /// <summary>
        /// 服务组件
        /// </summary>
        IGoodsGradeServer m_goodsGradeServer = ServerModuleFactory.GetServerModule<IGoodsGradeServer>();

        public UserControlGoodsGrade()
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
        }

        private void UserControlGoodsGrade_Resize(object sender, EventArgs e)
        {
            panelLeft.Width = (this.Width - panelCenter.Width) / 2;
            panelRight.Width = this.Width - panelCenter.Width - panelLeft.Width;
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        private void UserControlGoodsGrade_Load(object sender, EventArgs e)
        {
            if (!m_goodsGradeServer.GetAllGoodsGradeTable(out m_findTable, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            RefreshDataGridView(m_findTable);
            RefreshControl();
        }

        /// <summary>
        /// 刷新DataGridView
        /// </summary>
        /// <param name="findStore"></param>
        void RefreshDataGridView(IQueryable findTable)
        {
            dataGridView1.DataSource = findTable;

            this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            dataGridView1.Refresh();

            userControlDataLocalizer1.Init(dataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, GlobalObject.BasicInfo.LoginID));
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtGoodsType.Text == "")
            {
                MessageDialog.ShowPromptMessage("类别不允许为空!");
                return;
            }

            if (txtCode.Text == "")
            {
                MessageDialog.ShowPromptMessage("图号型号不允许为空!");
                return;
            }

            if (txtName.Text == "")
            {
                MessageDialog.ShowPromptMessage("名称不允许为空!");
                return;
            }

            if (cmbGoodsGrade.SelectedItem == null)
            {
                MessageDialog.ShowPromptMessage("难度分级不允许为空!");
                return;
            }

            DataTable table = null;

            if (!m_goodsGradeServer.AddGoodsGrade(txtGoodsType.Text, txtCode.Text, txtSpec.Text, 
                txtName.Text, cmbGoodsGrade.SelectedItem.ToString(), -1, out table, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            dataGridView1.DataSource = table;
            dataGridView1.Refresh();
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

                if (MessageBox.Show("您是否确定要删除该行信息?", "消息", MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (!m_goodsGradeServer.DeleteGoodsGrade(id, out m_findTable, out m_err))
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

                if (MessageBox.Show("您是否确定要删除该行信息?", "消息", MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    for (int i = 0; i < arrayID.Length; i++)
                    {
                        if (!m_goodsGradeServer.DeleteGoodsGrade(arrayID[i], out m_findTable, out m_err))
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

            RefreshDataGridView(m_findTable);
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

            DataTable table = null;

            if (!m_goodsGradeServer.AddGoodsGrade(txtGoodsType.Text, txtCode.Text, txtSpec.Text,
                txtName.Text, cmbGoodsGrade.SelectedItem.ToString(), id, out table, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            dataGridView1.DataSource = table;
            dataGridView1.Refresh();
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
            txtGoodsType.Text = "";
            txtCode.Text = "";
            txtSpec.Text = "";
            txtName.Text = "";
            cmbGoodsGrade.SelectedItem = null;
        }

        /// <summary>
        /// 刷新控件
        /// </summary>
        void RefreshControl()
        {
            ClearControl();

            if (dataGridView1.CurrentRow != null)
            {
                txtGoodsType.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                txtCode.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();

                if (dataGridView1.CurrentRow.Cells[3].Value != null)
                {
                    txtSpec.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                }
                
                txtName.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();

                for (int i = 0; i < cmbGoodsGrade.Items.Count; i++)
                {
                    if (cmbGoodsGrade.Items[i].ToString() == dataGridView1.CurrentRow.Cells[5].Value.ToString())
                    {
                        cmbGoodsGrade.SelectedIndex = i;
                        break;
                    }
                }
            }
            else if (dataGridView1.Rows.Count > 0)
            {
                txtGoodsType.Text = dataGridView1.Rows[0].Cells[1].Value.ToString();
                txtCode.Text = dataGridView1.Rows[0].Cells[2].Value.ToString();

                if (dataGridView1.Rows[0].Cells[3].Value != null)
                {
                    txtSpec.Text = dataGridView1.Rows[0].Cells[3].Value.ToString();
                }

                txtName.Text = dataGridView1.Rows[0].Cells[4].Value.ToString();

                for (int i = 0; i < cmbGoodsGrade.Items.Count; i++)
                {
                    if (cmbGoodsGrade.Items[i].ToString() == dataGridView1.Rows[0].Cells[5].Value.ToString())
                    {
                        cmbGoodsGrade.SelectedIndex = i;
                        break;
                    }
                }
            }
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
