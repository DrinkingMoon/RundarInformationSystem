/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  UserControlProviderInfo.cs
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
using GlobalObject;
using PlatformManagement;
using UniversalControlLibrary;
using CommonBusinessModule;

namespace Expression
{
    /// <summary>
    /// 供应商界面
    /// </summary>
    public partial class UserControlProvider : Form
    {
        #region variants

        /// <summary>
        /// 原有的员工ID
        /// </summary>
        string m_strOldPersonnelID = "";

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 查找到的符合条件的供应商信息
        /// </summary>
        IQueryable<View_Provider> m_findProvider;

        /// <summary>
        /// 供应商服务组件
        /// </summary>
        IProviderServer m_providerServer = ServerModuleFactory.GetServerModule<IProviderServer>();

        /// <summary>
        /// 用户窗体
        /// </summary>
        FormPersonnel m_formUser;

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="flag">权限标志</param>
        public UserControlProvider()
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw
                | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();

            for (int i = 0; i < 20; i++)
            {
                cmbYear.Items.Add((2012 + i).ToString());
            }
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="msg">定位信息</param>
        void PositioningRecord(string msg)
        {
            string strColName = "";

            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                if (col.Visible)
                {
                    strColName = col.Name;
                    break;
                }
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if ((string)dataGridView1.Rows[i].Cells["供应商编码"].Value.ToString() == msg)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                    break;
                }
            }
        }

        /// <summary>
        /// 改变组件大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControlProvider_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControlProviderInfo_Load(object sender, EventArgs e)
        {
            if (!m_providerServer.GetAllProvider(out m_findProvider, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            cmbYear.SelectedIndex = -1;
            cmbMonth.SelectedIndex = -1;

            RefreshDataGridView(m_findProvider);
            txtCode.Focus();

            FaceAuthoritySetting.SetVisibly(toolStrip1, BasicInfo.GetFunctionTreeNodeInfo(labelTitle.Text).Authority);
            RefreshControl();
        }

        /// <summary>
        /// 刷新DataGridView
        /// </summary>
        /// <param name="findDepartmentBill">数据集</param>
        void RefreshDataGridView(IQueryable findDepartmentBill)
        {
            dataGridView1.DataSource = findDepartmentBill;

            //// 添加数据定位控件
            //if (m_dataLocalizer == null)
            //{
            //    m_dataLocalizer = new UserControlDataLocalizer(dataGridView1, this.Name,
            //        UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));

            //    panelPara.Controls.Add(m_dataLocalizer);
            //    m_dataLocalizer.OnlyLocalize = true;
            //    m_dataLocalizer.Dock = DockStyle.Bottom;
            //    m_dataLocalizer.Visible = true;
            //}

            userControlDataLocalizer1.Init(dataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));

            dataGridView1.Columns["五笔码"].Visible = false;
            dataGridView1.Columns["拼音码"].Visible = false;

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

            string strProvider = txtCode.Text;

            if (!m_providerServer.AddProvider(txtCode.Text, txtName.Text, txtShotName.Text, 
                txtPersonnel.Tag.ToString(), out m_findProvider, out m_err,
                checkBox2.Checked,checkBox1.Checked,chbIsMainDuty.Checked, checkBox3.Checked, cmbClearingForm.Text, cmbYear.Text + cmbMonth.Text))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }
            else
            {
                MessageDialog.ShowPromptMessage("添加成功");
            }

            RefreshDataGridView(m_findProvider);
            RefreshControl();
            PositioningRecord(strProvider);
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

            if (txtPersonnel.Text == "")
            {
                MessageDialog.ShowPromptMessage("责任人不能为空!");
                return false;
            }

            for (int i = 0; i < txtCode.Text.Length; i++)
            {
                if ((int)txtCode.Text[i] > 127)
                {
                    MessageDialog.ShowPromptMessage("【编码】不能含有汉字");
                    return false;
                }
            }

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(cmbClearingForm.Text))
            {
                MessageDialog.ShowPromptMessage("请选择【结算方式】");
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
            else
            {
                string[] arrayCode = new string[n];

                for (int i = 0; i < n; i++)
                {
                    arrayCode[i] = dataGridView1.SelectedRows[i].Cells[0].Value.ToString();
                }

                if (MessageBox.Show("您是否确定要删除供应商与责任人的关系信息?", "消息", MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    for (int i = 0; i < arrayCode.Length; i++)
                    {
                        if (!m_providerServer.DeleteProvider(arrayCode[i],txtPersonnel.Tag.ToString(), out m_findProvider, out m_err))
                        {
                            MessageDialog.ShowPromptMessage(m_err);
                            return;
                        }
                    }

                    MessageDialog.ShowPromptMessage("删除成功");
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
            txtShotName.Text = "";
        }

        /// <summary>
        /// 刷新控件
        /// </summary>
        void RefreshControl()
        {
            ClearControl();

            if (dataGridView1.CurrentRow != null)
            {
                Provider providerInfo = m_providerServer.GetProviderInfo(dataGridView1.CurrentRow.Cells[0].Value.ToString());

                if (!GlobalObject.GeneralFunction.IsNullOrEmpty(providerInfo.AscendYearMonth))
                {
                    cmbMonth.Text = providerInfo.AscendYearMonth.Substring(4, 2);
                    cmbYear.Text = providerInfo.AscendYearMonth.Substring(0, 4);
                }
                else
                {
                    cmbMonth.SelectedIndex = -1;
                    cmbYear.SelectedIndex = -1;
                }

                txtCode.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                txtName.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                cmbClearingForm.Text = dataGridView1.CurrentRow.Cells["挂账方式"].Value == null ? "" : dataGridView1.CurrentRow.Cells["挂账方式"].Value.ToString();

                if (dataGridView1.CurrentRow.Cells[2].Value != null)
                {
                    txtShotName.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                }

                checkBox1.Checked = Convert.ToBoolean(dataGridView1.CurrentRow.Cells["是否在用"].Value);
                checkBox2.Checked = Convert.ToBoolean(dataGridView1.CurrentRow.Cells["零星采购供应商"].Value);
                checkBox3.Checked = Convert.ToBoolean(dataGridView1.CurrentRow.Cells["内部供应商"].Value);

                dataGridView2.DataSource = m_providerServer.GetProviderPrincipal(
                                           dataGridView1.CurrentRow.Cells["供应商编码"].Value.ToString());

                if (dataGridView2.Rows.Count > 0)
                {
                    dataGridView2.Rows[0].Selected = true;
                    txtPersonnel.Text = dataGridView2.CurrentRow.Cells["责任人"].Value.ToString();
                    txtPersonnel.Tag = dataGridView2.CurrentRow.Cells["责任人工号"].Value.ToString();
                    chbIsMainDuty.Checked = Convert.ToBoolean(dataGridView2.CurrentRow.Cells["主要责任人"].Value);
                }
            }
            else if (dataGridView1.Rows.Count > 0)
            {
                Provider providerInfo = m_providerServer.GetProviderInfo(dataGridView1.Rows[0].Cells[0].Value.ToString());

                if (!GlobalObject.GeneralFunction.IsNullOrEmpty(providerInfo.AscendYearMonth))
                {
                    cmbMonth.Text = providerInfo.AscendYearMonth.Substring(4, 2);
                    cmbYear.Text = providerInfo.AscendYearMonth.Substring(0, 4);
                }
                else
                {
                    cmbMonth.SelectedIndex = -1;
                    cmbYear.SelectedIndex = -1;
                }

                txtCode.Text = dataGridView1.Rows[0].Cells[0].Value.ToString();
                txtName.Text = dataGridView1.Rows[0].Cells[1].Value.ToString();
                cmbClearingForm.Text = dataGridView1.CurrentRow.Cells["挂账方式"].Value == null ? "" : dataGridView1.CurrentRow.Cells["挂账方式"].Value.ToString();

                if (dataGridView1.CurrentRow.Cells[2].Value != null)
                {
                    txtShotName.Text = dataGridView1.Rows[0].Cells[2].Value.ToString();
                }

                checkBox1.Checked = Convert.ToBoolean(dataGridView1.CurrentRow.Cells["是否在用"].Value);
                checkBox2.Checked = Convert.ToBoolean(dataGridView1.CurrentRow.Cells["零星采购供应商"].Value);
                checkBox3.Checked = Convert.ToBoolean(dataGridView1.CurrentRow.Cells["内部供应商"].Value);

                dataGridView2.DataSource = m_providerServer.GetProviderPrincipal(
                                           dataGridView1.CurrentRow.Cells["供应商编码"].Value.ToString());

                if (dataGridView2.Rows.Count > 0)
                {
                    dataGridView2.Rows[0].Selected = true;
                    txtPersonnel.Text = dataGridView2.CurrentRow.Cells["责任人"].Value.ToString();
                    txtPersonnel.Tag = dataGridView2.CurrentRow.Cells["责任人工号"].Value.ToString();
                    chbIsMainDuty.Checked = Convert.ToBoolean(dataGridView2.CurrentRow.Cells["主要责任人"].Value);
                }
            }
        }

        private void btnFindBuyer_Click(object sender, EventArgs e)
        {
            m_strOldPersonnelID = txtPersonnel.Tag.ToString();
            m_formUser = null;
            m_formUser = new FormPersonnel(txtPersonnel);
            m_formUser.ShowDialog();
            txtPersonnel.Tag = m_formUser.UserCode;
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtPersonnel.Tag == null || txtPersonnel.Tag.ToString() == "")
            {
                MessageDialog.ShowPromptMessage("请选择供应商责任人");
                return;
            }

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(cmbClearingForm.Text))
            {
                MessageDialog.ShowPromptMessage("请选择【结算方式】");
                return;
            }

            string strProvider = dataGridView1.CurrentRow.Cells["供应商编码"].Value.ToString();

            if (cmbClearingForm.Text == "用量")
            {
                m_providerServer.InsertUnitPriceList(strProvider);
            }

            if (!m_providerServer.UpdataProvider(dataGridView1.CurrentRow.Cells["供应商编码"].Value.ToString(),
                txtName.Text,
                txtShotName.Text,
                checkBox2.Checked,
                checkBox1.Checked,
                chbIsMainDuty.Checked,
                checkBox3.Checked,
                txtPersonnel.Tag.ToString(),
                dataGridView2.CurrentRow.Cells["责任人工号"].Value.ToString(),
                cmbClearingForm.Text, cmbYear.Text + cmbMonth.Text,
                out m_findProvider,out m_err))
            {
                MessageDialog.ShowPromptMessage(m_err);
                return;
            }
            else
            {
                MessageDialog.ShowPromptMessage("修改成功");
            }

            RefreshDataGridView(m_findProvider);
            RefreshControl();
            PositioningRecord(strProvider);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (!m_providerServer.GetAllProvider(out m_findProvider, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            RefreshDataGridView(m_findProvider);
            txtCode.Focus();
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.Rows.Count > 0)
            {
                txtPersonnel.Text = dataGridView2.CurrentRow.Cells["责任人"].Value.ToString();
                txtPersonnel.Tag = dataGridView2.CurrentRow.Cells["责任人工号"].Value.ToString();
                chbIsMainDuty.Checked = Convert.ToBoolean(dataGridView2.CurrentRow.Cells["主要责任人"].Value);
            }

        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                return;
            }

            RefreshControl();
        }

        private void btnOutput_Click(object sender, EventArgs e)
        {
            string strSql = " select * from View_Provider where 是否在用 = 1";
            ExcelHelperP.DataTableToExcel(saveFileDialog1, GlobalObject.DatabaseServer.QueryInfo(strSql), null);
        }
    }
}
