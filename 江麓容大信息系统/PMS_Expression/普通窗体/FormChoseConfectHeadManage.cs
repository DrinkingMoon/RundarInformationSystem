/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  FormChoseConfectHeadManage.cs
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
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 选配表表头设计类
    /// </summary>
    public partial class FormChoseConfectHeadManage : Form
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
        /// datagridview列排序标志
        /// </summary>
        bool m_columnSortFlag = false;

        /// <summary>
        /// 现有表头的零件编码列表
        /// </summary>
        List<string> m_listAccessoryCode = new List<string>();

        /// <summary>
        /// 文本编辑框1
        /// </summary>
        TextBox m_textCode;

        /// <summary>
        /// 零件选配服务组件
        /// </summary>
        IChoseConfectServer m_choseConfectServer = ServerModuleFactory.GetServerModule <IChoseConfectServer>();

        public FormChoseConfectHeadManage(TextBox textCode)
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw 
                | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);

            UpdateStyles();

            m_textCode = textCode;
        }

        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormChoseConfectHeadManage_Load(object sender, EventArgs e)
        {
            InitDataGridView1();
        }

        /// <summary>
        /// 查找零件编码规格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFindCode_Click(object sender, EventArgs e)
        {
            IAssemblyInfoServer assemblyServer = ServerModuleFactory.GetServerModule<IAssemblyInfoServer>();
            FormQueryInfo form = new FormQueryInfo(assemblyServer.GetAllChoseConfectPartInfo());

            if (DialogResult.OK == form.ShowDialog())
            {
                txtCode.Text = form.GetDataItem("零件编码").ToString();
                txtSpec.Text = form.GetDataItem("规格").ToString();
            }
        }


        /// <summary>
        /// 初始化DataGridView
        /// </summary>
        void InitDataGridView1()
        {
            if (m_choseConfectServer.GetAllChoseConfectTableHead(out m_findTable, out m_err))
            {
                dataGridView1.DataSource = m_findTable;
                dataGridView1.Columns[dataGridView1.Columns.Count - 1].Visible = false;
                dataGridView1.Refresh();

                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    dataGridView1.Columns[i].Width = 150;
                }

                dataGridView1.Columns[1].Width = 260;

                if (m_findTable.Rows.Count > 0)
                {
                    m_listAccessoryCode.Clear();

                    for (int i = 0; i < m_findTable.Rows.Count; i++)
                    {
                        if (!m_listAccessoryCode.Contains(m_findTable.Rows[i][0].ToString() + m_findTable.Rows[i][1].ToString()))
                        {
                            m_listAccessoryCode.Add(m_findTable.Rows[i][0].ToString() + m_findTable.Rows[i][1].ToString());
                        }
                    }

                    UpdataPanelPara();
                }

                if (!m_columnSortFlag)
                {
                    m_columnSortFlag = true;
                }
            }
            else
            {
                MessageDialog.ShowErrorMessage(m_err);

                dataGridView1.DataSource = m_findTable;
                dataGridView1.Refresh();

                ResetPanelPara();

                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
            }
        }

        /// <summary>
        /// 重置右面板参数
        /// </summary>
        void ResetPanelPara()
        {
            txtTableName.Text = "";
            txtCode.Text = "";
            txtSpec.Text = ""; ;
            txtFirstColumnName.Text = "";
            txtSecondColumnName.Text = "";
        }

        /// <summary>
        /// 更新右面板参数
        /// </summary>
        void UpdataPanelPara()
        {
            if (dataGridView1.CurrentRow != null)
            {
                txtCode.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                txtTableName.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                txtFirstColumnName.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                txtSecondColumnName.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            }

            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;
        }

        /// <summary>
        /// 更新右面板参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            UpdataPanelPara();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (CheckChoseConfectHeadInfo())
            {
                if (m_listAccessoryCode.Contains(txtCode.Text + txtSpec.Text))
                {
                    MessageDialog.ShowPromptMessage("该零件选配表的表头信息数据库中已存在,请重新输入!");
                    return;
                }

                if (UpdataChoseConfectHeadInfo())
                {
                    InitDataGridView1();
                }
            }
        }

        /// <summary>
        /// 检测选配表表头信息
        /// </summary>
        /// <returns>返回是否允许添加/修改选配表表头信息</returns>
        bool CheckChoseConfectHeadInfo()
        {
            if (txtCode.Text == "")
            {
                MessageDialog.ShowPromptMessage("零部件编码不能为空!");
                return false;
            }

            if (txtTableName.Text == "")
            {
                MessageDialog.ShowPromptMessage("表名不能为空!");
                return false;
            }

            if (txtFirstColumnName.Text == "")
            {
                MessageDialog.ShowPromptMessage("第1列名称不能为空!");
                return false;
            }

            if (txtSecondColumnName.Text == "")
            {
                MessageDialog.ShowPromptMessage("第2列名称不能为空!");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 更新零件信息
        /// </summary>
        /// <returns>返回是否成功更新零件信息</returns>
        bool UpdataChoseConfectHeadInfo()
        {
            string accessoryCode = txtCode.Text;
            string tableName = txtTableName.Text;
            string firstColumn = txtFirstColumnName.Text;
            string secondColumn = txtSecondColumnName.Text;

            if (!m_choseConfectServer.UpdataAccessoryChoseConfectHead(
                accessoryCode, tableName, firstColumn, secondColumn, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
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
                return;
            }
            else if (n == 1)
            {
                string ChoseControlHead = dataGridView1.CurrentRow.Cells[0].Value.ToString();

                if (MessageBox.Show("您是否确定要删除?", "消息", MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (m_choseConfectServer.DeleteChoseConfectTableHead(txtCode.Text, out m_err))
                    {
                        InitDataGridView1();
                    }
                    else
                    {
                        MessageDialog.ShowErrorMessage(m_err);
                    }
                }
                else
                {
                    return;
                }
            }
            else if (n > 1)
            {
                string[] ChoseControlHeads = new string[n];

                for (int i = 0; i < n; i++)
                {
                    ChoseControlHeads[i] = dataGridView1.SelectedRows[i].Cells[0].Value.ToString();
                }

                if (MessageBox.Show("您是否确定要删除?", "消息", MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string error;

                    for (int i = 0; i < ChoseControlHeads.Length; i++)
                    {
                        if (!m_choseConfectServer.DeleteChoseConfectTableHead(txtCode.Text, out error))
                        {
                            MessageBox.Show(error, "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            InitDataGridView1();

                            return;
                        }
                    }
                }

                InitDataGridView1();
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (CheckChoseConfectHeadInfo())
            {
                if (UpdataChoseConfectHeadInfo())
                {
                    InitDataGridView1();
                }
            }
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormChoseConfectHeadManage_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_textCode != null)
            {
                m_textCode.Text = txtCode.Text;
            }
        }

        private void FormChoseConfectHeadManage_Resize(object sender, EventArgs e)
        {
            panelLeft.Width = (this.Width - panelCenter.Width) / 2;
            panelRight.Width = this.Width - panelCenter.Width - panelLeft.Width;

            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }
    }
}
