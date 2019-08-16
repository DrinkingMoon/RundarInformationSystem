/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  FormUnit.cs
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
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 单位界面
    /// </summary>
    public partial class FormUnit : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 查找到的符合条件的库存信息
        /// </summary>
        IQueryable<View_S_Unit> m_findUnit;

        /// <summary>
        /// 单位服务组件
        /// </summary>
        IUnitServer m_unitServer = ServerModuleFactory.GetServerModule<IUnitServer>();

        /// <summary>
        /// 编辑框
        /// </summary>
        TextBox m_textBox;

        public FormUnit(TextBox textBox)
        {
            InitializeComponent();
            m_textBox = textBox;
        }

        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormUnit_Load(object sender, EventArgs e)
        {
            if (!m_unitServer.GetAllUnit(out m_findUnit, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            m_findUnit = from a in m_findUnit
                         where a.停用 == false
                         select a;

            RefreshDataGridView(m_findUnit);
            dataGridView1.Columns[0].Visible = false;
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

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 选定单位ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_textBox.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
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
            选定单位ToolStripMenuItem.PerformClick();
        }
    }
}
