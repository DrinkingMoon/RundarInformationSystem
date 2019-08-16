/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  FormDepartment.cs
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
    /// 部门界面类
    /// </summary>
    public partial class FormDepartment : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 所有供应商信息
        /// </summary>
        IQueryable<View_Department> m_findDepartment;

        /// <summary>
        /// 部门服务组件
        /// </summary>
        IDepartmentServer m_departmentServer = ServerModuleFactory.GetServerModule<IDepartmentServer>();

        /// <summary>
        /// 编辑框
        /// </summary>
        TextBox m_txtEdit;

        /// <summary>
        /// 部门ID
        /// </summary>
        string m_departmentCode;

        /// <summary>
        /// 获取部门编码
        /// </summary>
        public string DepartmentCode
        {
            get { return m_departmentCode; }
        }

        public FormDepartment(TextBox textBox)
        {
            InitializeComponent();
            m_txtEdit = textBox;
        }

        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormDepartment_Load(object sender, EventArgs e)
        {
            if (!m_departmentServer.GetAllDepartment(out m_findDepartment, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            RefreshDataGridView(m_findDepartment);

            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        /// <summary>
        /// 刷新DataGridView
        /// </summary>
        /// <param name="findProvider">数据集</param>
        void RefreshDataGridView(IQueryable<View_Department> findProvider)
        {
            dataGridView1.DataSource = findProvider;

            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                dataGridView1.Columns[i].Width = 90;
            }

            dataGridView1.Refresh();
        }

        private void 选定部门ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_departmentCode = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            m_txtEdit.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
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
            选定部门ToolStripMenuItem.PerformClick();
        }

    }
}
