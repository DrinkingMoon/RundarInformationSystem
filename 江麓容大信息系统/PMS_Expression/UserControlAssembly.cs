/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  UserControlAssembly.cs
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
using System.Drawing;
using System.Linq;
using System.Data;
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
    /// 产品装配零件信息附属表组件
    /// </summary>
    public partial class UserControlAssembly : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 服务组件
        /// </summary>
        IProductInfoServer m_productInfoServer = ServerModuleFactory.GetServerModule<IProductInfoServer>();

        /// <summary>
        /// 服务组件
        /// </summary>
        IAssemblyInfoServer m_assemblyServer = ServerModuleFactory.GetServerModule<IAssemblyInfoServer>();

        /// <summary>
        /// Bom附属表(总成信息)
        /// </summary>
        DataTable m_returnTable1;

        /// <summary>
        /// Bom附属表(零部件信息)
        /// </summary>
        DataTable m_returnTable2;

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_error;

        public UserControlAssembly()
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
        private void UserControlAssembly_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControlAssembly_Load(object sender, EventArgs e)
        {
            IQueryable<View_P_ProductInfo> productInfo = null;

            if (!m_productInfoServer.GetAllProductInfo(out productInfo, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }
            else
            {
                productInfo = from r in productInfo
                              where !r.产品类型名称.Contains("返修")
                              select r;

                foreach(var item in productInfo)
                {
                    cmbProductType.Items.Add(item.产品类型编码);
                }

                cmbProductType.SelectedIndex = -1;

                RefreshControl1();
                RefreshControl2();
            }
        }

        /// <summary>
        /// 设定功能键不可用
        /// </summary>
        void SetFunctionButtonEnable()
        {
            btnAdd1.Enabled = false;
            btnDelete1.Enabled = false;
            btnFindCode1.Enabled = false;
            btnAdd2.Enabled = false;
            btnDelete2.Enabled = false;
            btnFindCode2.Enabled = false;
        }

        /// <summary>
        /// 查找BOM中所有总成编码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFindCode1_Click(object sender, EventArgs e)
        {
            FormQueryInfo form = QueryInfoDialog.GetAccessoryInfoDialog(cmbProductType.Text, false, true);

            if (form.ShowDialog() == DialogResult.OK)
            {
                txtCode1.Text = (string)form.GetDataItem("图号型号");
                txtName1.Text = (string)form.GetDataItem("物品名称");
                txtSpec1.Text = (string)form.GetDataItem("规格");
            }
        }

        /// <summary>
        /// 查找所有零部件(非总成)编码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFindCode2_Click(object sender, EventArgs e)
        {
            FormQueryInfo form = QueryInfoDialog.GetAccessoryInfoDialog(cmbProductType.Text, false, false);

            if (form.ShowDialog() == DialogResult.OK)
            {
                txtCode2.Text = (string)form.GetDataItem("图号型号");
                txtName2.Text = (string)form.GetDataItem("物品名称");
                txtSpec2.Text = (string)form.GetDataItem("规格");
            }
        }

        /// <summary>
        /// 添加1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd1_Click(object sender, EventArgs e)
        {
            if (txtCode1.Text == "")
            {
                MessageDialog.ShowPromptMessage("图号型号不允许为空!");
                return;
            }

            if (txtName1.Text == "")
            {
                MessageDialog.ShowPromptMessage("物品名称不允许为空!");
                return;
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[1].Value.ToString() == txtCode1.Text)
                {
                    MessageDialog.ShowPromptMessage("此零件已经存在!");
                    return;
                }
            }

            if (!m_assemblyServer.AddPertainProductBomInfo(cmbProductType.SelectedItem.ToString(), txtCode1.Text,
                                                           txtName1.Text, txtSpec1.Text, 1, out m_returnTable1, out m_err))
            {
                MessageDialog.ShowPromptMessage(m_err);
                return;
            }

            ClearControl1();
            RefreshDataGridView1();
        }

        /// <summary>
        /// 添加2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd2_Click(object sender, EventArgs e)
        {
            if (txtCode2.Text == "")
            {
                MessageDialog.ShowPromptMessage("图号型号不允许为空!");
                return;
            }

            if (txtName2.Text == "")
            {
                MessageDialog.ShowPromptMessage("物品名称不允许为空!");
                return;
            }

            if (!m_assemblyServer.AddPertainProductBomInfo(cmbProductType.SelectedItem.ToString(), txtCode2.Text, 
                                                           txtName2.Text, txtSpec2.Text, 0, out m_returnTable2, out m_err))
            {
                MessageDialog.ShowPromptMessage(m_err);
                return;
            }

            ClearControl2();
            RefreshDataGridView2();
        }

        /// <summary>
        /// 删除1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete1_Click(object sender, EventArgs e)
        {
            int n = dataGridView1.SelectedRows.Count;

            if (n == 0)
            {
                MessageDialog.ShowPromptMessage("请选择需要删除的数据行!");
                return;
            }

            for (int i = 0; i < n; i++)
            {
                string id = dataGridView1.SelectedRows[i].Cells[0].Value.ToString();
                string info = string.Format("您是否确定要删除图号为 [{0}] 的零件信息？", 
                              dataGridView1.SelectedRows[i].Cells["图号型号"].Value.ToString());

                if (MessageBox.Show(info, "消息", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (!m_assemblyServer.DeletePertainProductBomInfo(id, cmbProductType.SelectedItem.ToString(), 
                                                                      0, out m_returnTable1, out m_err))
                    {
                        MessageDialog.ShowPromptMessage(m_err);
                    }
                }
                else
                {
                    break;
                }
            }

            ClearControl1();
            RefreshDataGridView1();
        }

        /// <summary>
        /// 删除2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete2_Click(object sender, EventArgs e)
        {
            int n = dataGridView2.SelectedRows.Count;

            if (n == 0)
            {
                MessageDialog.ShowPromptMessage("请选择需要删除的数据行!");
                return;
            }

            for (int i = 0; i < n; i++)
            {
                string id  = dataGridView2.SelectedRows[i].Cells[0].Value.ToString();
                string info = string.Format("您是否确定要删除图号为 [{0}] 的选配信息？", 
                              dataGridView2.SelectedRows[i].Cells["图号型号"].Value.ToString());

                if (MessageBox.Show(info, "消息", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (!m_assemblyServer.DeletePertainProductBomInfo(id, cmbProductType.SelectedItem.ToString(),
                                                                      0, out m_returnTable2, out m_err))
                    {
                        MessageDialog.ShowPromptMessage(m_err);
                    }
                }
                else
                {
                    break;
                }
            }

            ClearControl2();
            RefreshDataGridView2();
        }

        /// <summary>
        /// 改变产品类型1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comProductType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearControl1();
            ClearControl2();

            if (!m_assemblyServer.GetPertainProductBomInfo("True", cmbProductType.SelectedItem.ToString(), out m_returnTable1, out m_err))
            {
                if (m_err != "没有找到任何数据")
                {
                    MessageDialog.ShowPromptMessage(m_err);
                    return;
                }
            }

            if (!m_assemblyServer.GetPertainProductBomInfo("False", cmbProductType.SelectedItem.ToString(), out m_returnTable2, out m_err))
            {
                if (m_err != "没有找到任何数据")
                {
                    MessageDialog.ShowPromptMessage(m_err);
                    return;
                }
            }

            RefreshDataGridView1();
            RefreshDataGridView2();
        }

        /// <summary>
        /// 清空控件值
        /// </summary>
        void ClearControl1()
        {
            txtCode1.Text = "";
            txtName1.Text = "";
            txtSpec1.Text = "";
        }

        /// <summary>
        /// 清空控件值
        /// </summary>
        void ClearControl2()
        {
            txtCode2.Text = "";
            txtName2.Text = "";
            txtSpec2.Text = "";
        }

        /// <summary>
        /// 刷新DataGridView1
        /// </summary>
        void RefreshDataGridView1()
        {
            dataGridView1.DataSource = m_returnTable1;

            this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            dataGridView1.Refresh();
        }

        /// <summary>
        /// 刷新DataGridView2
        /// </summary>
        void RefreshDataGridView2()
        {
            dataGridView2.DataSource = m_returnTable2;

            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView2);

            dataGridView2.Refresh();
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            RefreshControl1();
        }

        private void dataGridView2_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            RefreshControl2();
        }

        void RefreshControl1()
        {
            ClearControl1();

            if (dataGridView1.CurrentRow != null)
            {
                txtCode1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                txtName1.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                txtSpec1.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            }
            else if (dataGridView1.Rows.Count > 0)
            {
                txtCode1.Text = dataGridView1.Rows[0].Cells[1].Value.ToString();
                txtName1.Text = dataGridView1.Rows[0].Cells[2].Value.ToString();
                txtSpec1.Text = dataGridView1.Rows[0].Cells[3].Value.ToString();
            }
        }

        void RefreshControl2()
        {
            ClearControl2();

            if (dataGridView2.CurrentRow != null)
            {
                txtCode2.Text = dataGridView2.CurrentRow.Cells[1].Value.ToString();
                txtName2.Text = dataGridView2.CurrentRow.Cells[2].Value.ToString();
                txtSpec2.Text = dataGridView2.CurrentRow.Cells[3].Value.ToString();
            }
            else if (dataGridView2.Rows.Count > 0)
            {
                txtCode2.Text = dataGridView2.Rows[0].Cells[1].Value.ToString();
                txtName2.Text = dataGridView2.Rows[0].Cells[2].Value.ToString();
                txtSpec2.Text = dataGridView2.Rows[0].Cells[3].Value.ToString();
            }

        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }
    }
}
