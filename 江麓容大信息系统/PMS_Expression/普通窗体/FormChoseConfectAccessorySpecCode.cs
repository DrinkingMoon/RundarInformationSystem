/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  FormChoseConfectAccessorySpecCode.cs
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
    /// 选配零件界面类
    /// </summary>
    public partial class FormChoseConfectAccessorySpecCode : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// P_ChoseConfectTableHead信息表
        /// </summary>
        DataTable m_findTable;

        /// <summary>
        /// 零件选配服务组件
        /// </summary>
        IChoseConfectServer m_choseConfectServer = ServerModuleFactory.GetServerModule<IChoseConfectServer>();

        // <summary>
        /// 编辑框1
        /// </summary>
        TextBox m_textBoxCode;

        /// <summary>
        /// 数据定位控件
        /// </summary>
        UserControlDataLocalizer m_dataLocalizer;

        public FormChoseConfectAccessorySpecCode(TextBox textBoxCode)
        {
            InitializeComponent();
            m_textBoxCode = textBoxCode;
        }

        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormChoseConfectAccessorySpecCode_Load(object sender, EventArgs e)
        { 
            if (!m_choseConfectServer.GetAllChoseConfectTableHead(out m_findTable, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            RefreshDataGridView(m_findTable);

            for (int i = 2; i < dataGridView1.Columns.Count; i++)
            {
                dataGridView1.Columns[i].Visible = false;
            }

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
        void RefreshDataGridView(DataTable table)
        {
            dataGridView1.DataSource = table;
            dataGridView1.Refresh();
        }

        /// <summary>
        /// 选定零件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 选定零件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_textBoxCode != null)
            {
                m_textBoxCode.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
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
            this.选定零件ToolStripMenuItem.PerformClick();
        }
    }
}
