/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  FormGoods.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2009/06/15
 * 开发平台:  vs2005(c#)
 * 用于    :  生产线管理信息系统
 *----------------------------------------------------------------------------
 * 描述 : 关于界面
 * 其它 :
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2009/07/03 08:02:08 作者: 夏石友 当前版本: V1.00
 *        修改说明: 创建
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
    /// 货物信息界面类
    /// </summary>
    public partial class FormGoods : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 所有供应商信息
        /// </summary>
        IQueryable<View_S_Stock> m_findStore;

        /// <summary>
        /// 服务组件
        /// </summary>
        IStoreServer m_storeServer = ServerModuleFactory.GetServerModule<IStoreServer>();

        /// <summary>
        /// 编辑框1
        /// </summary>
        TextBox m_txtCode;

        /// <summary>
        /// 编辑框2
        /// </summary>
        TextBox m_txtName;

        /// <summary>
        /// 编辑框3
        /// </summary>
        TextBox m_txtSpec;

        /// <summary>
        /// 数据定位控件
        /// </summary>
        UserControlDataLocalizer m_dataLocalizer;

        public FormGoods(TextBox textBoxCode, TextBox textBoxName, TextBox textBoxSpec)
        {
            InitializeComponent();
            m_txtCode = textBoxCode;
            m_txtName = textBoxName;
            m_txtSpec = textBoxSpec;
        }

        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormGoods_Load(object sender, EventArgs e)
        {
            if (!m_storeServer.GetAllStore(null, true, out m_findStore, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            RefreshDataGridView(m_findStore);

            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                dataGridView1.Columns[i].Visible = false;
            }

            dataGridView1.Columns[1].Visible = true;
            dataGridView1.Columns[2].Visible = true;
            dataGridView1.Columns[3].Visible = true;
            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            ColumnWidthControl.SetDataGridView(this.Text, dataGridView1);

            // 添加数据定位控件
            if (m_dataLocalizer == null)
            {
                m_dataLocalizer = new UserControlDataLocalizer(
                    dataGridView1, this.Name, UniversalFunction.SelectHideFields(
                    this.Name, dataGridView1.Name, BasicInfo.LoginID));
                panelTop.Controls.Add(m_dataLocalizer);
                m_dataLocalizer.Dock = DockStyle.Bottom;
                m_dataLocalizer.Visible = true;
            }
        }

        /// <summary>
        /// 刷新DataGridView
        /// </summary>
        /// <param name="findStore"></param>
        void RefreshDataGridView(IQueryable<View_S_Stock> findGoods)
        {
            dataGridView1.DataSource = findGoods;
            dataGridView1.Refresh();
        }

        /// <summary>
        /// 选定货物
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 选定货物ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_txtCode != null)
            {
                m_txtCode.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            }

            if (m_txtName != null)
            {
                m_txtName.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            }

            if (m_txtSpec != null && dataGridView1.CurrentRow.Cells[3].Value != null)
            {
                m_txtSpec.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            }

            this.Close();
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            string strColName = "";

            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                if (col.Visible)
                {
                    strColName = col.Name;
                    break;
                }
            }

            dataGridView1.ClearSelection();
            dataGridView1.CurrentCell = dataGridView1.Rows[e.RowIndex].Cells[strColName];
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            选定货物ToolStripMenuItem.PerformClick();
        }
    }
}
