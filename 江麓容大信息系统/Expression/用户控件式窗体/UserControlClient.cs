/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  UserControlClient.cs
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
    /// 客户组件
    /// </summary>
    public partial class UserControlClient : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 查找到的符合条件的客户信息
        /// </summary>
        IQueryable<View_Client> m_findClient;

        /// <summary>
        /// 部门服务组件
        /// </summary>
        IDepartmentServer m_serverDepartment = ServerModuleFactory.GetServerModule<IDepartmentServer>();

        /// <summary>
        /// 服务组件
        /// </summary>
        IClientServer m_clientServer = ServerModuleFactory.GetServerModule<IClientServer>();

        /// <summary>
        /// 数据定位控件
        /// </summary>
        UserControlDataLocalizer m_dataLocalizer;

        /// <summary>
        /// 客户管理数据集
        /// </summary>
        Client m_clientInfo = new Client();

        public UserControlClient()
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
        private void UserControlClient_Resize(object sender, EventArgs e)
        {
            //panelLeft.Width = (this.Width - panelCenter.Width) / 2;
            //panelRight.Width = this.Width - panelCenter.Width - panelLeft.Width;
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControlClient_Load(object sender, EventArgs e)
        {
            if (!m_clientServer.GetAllClient(out m_findClient, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            RefreshDataGridView(m_findClient);

            txtCode.Focus();

            // 添加数据定位控件
            if (m_dataLocalizer == null)
            {
                m_dataLocalizer = new UserControlDataLocalizer(dataGridView1, this.Name, 
                    UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
                this.panelPara.Controls.Add(m_dataLocalizer);
                m_dataLocalizer.Dock = DockStyle.Bottom;
                m_dataLocalizer.Visible = true;
            }
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="msg">定位用的单据号</param>
        public void PositioningRecord(string msg)
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
                if ((string)dataGridView1.Rows[i].Cells["客户编码"].Value == msg)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                    break;
                }
            }
        }

        /// <summary>
        /// 刷新DataGridView
        /// </summary>
        /// <param name="findStore">数据集</param>
        void RefreshDataGridView(IQueryable findClient)
        {
            dataGridView1.DataSource = findClient;

            this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            dataGridView1.Refresh();
        }

        Client GetClientInfo()
        {
            m_clientInfo.Address = txtAddr.Text;
            m_clientInfo.ClientCode = txtCode.Text;
            m_clientInfo.ClientName = txtName.Text;
            m_clientInfo.Linkman = txtLinkman.Text;
            m_clientInfo.Phone = txtPhone.Text;
            m_clientInfo.Remark = txtRemark.Text;
            m_clientInfo.Province = txtProvince.Text;
            m_clientInfo.ServiceArea = txtServiceArea.Text;
            m_clientInfo.Principal = txtPrincipal.Text;
            m_clientInfo.IsSecStorage = chbIsSecStorage.Checked;
            m_clientInfo.AllName = txtAllName.Text;

            return m_clientInfo;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (m_serverDepartment.GetDepartmentName(txtCode.Text) != "")
            {
                MessageDialog.ShowErrorMessage("此编码被禁用，请选择填写编码");
                return;
            }

            if (chbIsSecStorage.Checked && txtPrincipal.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请为服务站/4S店/二级库房指定【负责人】");
                return;
            }

            if (!m_clientServer.AddClient(GetClientInfo(), out m_findClient, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            if (!m_clientServer.GetAllClient(out m_findClient, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            RefreshDataGridView(m_findClient);
            PositioningRecord(m_clientInfo.ClientCode);
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
                string clientCode = dataGridView1.CurrentRow.Cells[0].Value.ToString();

                if (MessageBox.Show("您是否确定要删除客户" + dataGridView1.CurrentRow.Cells[1].Value.ToString() + "信息?", "消息",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (!m_clientServer.DeleteClient(clientCode, out m_findClient, out m_err))
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

                if (MessageBox.Show("您是否确定要删除客户信息?", "消息", MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    for (int i = 0; i < arrayCode.Length; i++)
                    {
                        if (!m_clientServer.DeleteClient(arrayCode[i], out m_findClient, out m_err))
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

            RefreshDataGridView(m_findClient);

            RefreshControl();
        }

        /// <summary>
        /// 清空控件值
        /// </summary>
        void ClearControl()
        {
            txtCode.Text = "";
            txtName.Text = "";
            txtRemark.Text = "";
            chbIsSecStorage.Checked = false;
            txtAddr.Text = "";
            txtLinkman.Text = "";
            txtPhone.Text = "";
            txtPrincipal.Text = "";
            txtProvince.Text = "";
            txtServiceArea.Text = "";
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string clientCode = dataGridView1.Rows[0].Cells[0].Value.ToString();

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
                    clientCode = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                }
            }

            if (clientCode != txtCode.Text)
            {
                MessageDialog.ShowPromptMessage("客户编码不允许修改!");
                txtCode.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                return;
            }


            if (chbIsSecStorage.Checked && txtPrincipal.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请为服务站/4S店/二级库房指定【负责人】");
                return;
            }

            Client lnqClient = new Client();

            lnqClient.ClientCode = dataGridView1.CurrentRow.Cells["客户编码"].Value.ToString();
            lnqClient.ClientName = dataGridView1.CurrentRow.Cells["客户名称"].Value.ToString();
            lnqClient.IsSecStorage = Convert.ToBoolean(dataGridView1.CurrentRow.Cells["是否为服务站点_4S店_二级库房"].Value.ToString());

            if (!m_clientServer.UpdateClient(GetClientInfo(),lnqClient, out m_findClient, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            RefreshDataGridView(m_findClient);
            PositioningRecord(lnqClient.ClientCode);
            RefreshControl();
        }

        /// <summary>
        /// DataGridView单元格获取到焦点事件
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
                txtCode.Text = dataGridView1.CurrentRow.Cells["客户编码"].Value.ToString();
                txtName.Text = dataGridView1.CurrentRow.Cells["客户名称"].Value.ToString();
                chbIsSecStorage.Checked = Convert.ToBoolean(dataGridView1.CurrentRow.Cells["是否为服务站点_4S店_二级库房"].Value.ToString());


                if (dataGridView1.CurrentRow.Cells["客户全称"].Value != null)
                {
                    txtAllName.Text = dataGridView1.CurrentRow.Cells["客户全称"].Value.ToString();
                }

                if (dataGridView1.CurrentRow.Cells["联系电话"].Value != null)
                {
                    txtPhone.Text = dataGridView1.CurrentRow.Cells["联系电话"].Value.ToString();
                }

                if (dataGridView1.CurrentRow.Cells["省份"].Value != null)
                {
                    txtProvince.Text = dataGridView1.CurrentRow.Cells["省份"].Value.ToString();
                }

                if (dataGridView1.CurrentRow.Cells["服务区域"].Value != null)
                {
                    txtServiceArea.Text = dataGridView1.CurrentRow.Cells["服务区域"].Value.ToString();
                }

                if (dataGridView1.CurrentRow.Cells["负责人"].Value != null)
                {
                    txtPrincipal.Text = dataGridView1.CurrentRow.Cells["负责人"].Value.ToString();
                }

                if (dataGridView1.CurrentRow.Cells["详细地址"].Value != null)
                {
                    txtAddr.Text = dataGridView1.CurrentRow.Cells["详细地址"].Value.ToString();
                }

                if (dataGridView1.CurrentRow.Cells["联系人"].Value != null)
                {
                    txtLinkman.Text = dataGridView1.CurrentRow.Cells["联系人"].Value.ToString();
                }

                if (dataGridView1.CurrentRow.Cells["备注"].Value != null)
                {
                    txtRemark.Text = dataGridView1.CurrentRow.Cells["备注"].Value.ToString();
                }
            }
            else if (dataGridView1.Rows.Count > 0)
            {
                txtCode.Text = dataGridView1.Rows[0].Cells["客户编码"].Value.ToString();
                txtName.Text = dataGridView1.Rows[0].Cells["客户名称"].Value.ToString();
                chbIsSecStorage.Checked = Convert.ToBoolean(dataGridView1.CurrentRow.Cells["是否为服务站点/4S店/二级库房"].Value.ToString());

                if (dataGridView1.Rows[0].Cells["客户全称"].Value != null)
                {
                    txtAllName.Text = dataGridView1.Rows[0].Cells["客户全称"].Value.ToString();
                }

                if (dataGridView1.Rows[0].Cells["联系电话"].Value != null)
                {
                    txtPhone.Text = dataGridView1.Rows[0].Cells["联系电话"].Value.ToString();
                }

                if (dataGridView1.Rows[0].Cells["省份"].Value != null)
                {
                    txtProvince.Text = dataGridView1.Rows[0].Cells["省份"].Value.ToString();
                }

                if (dataGridView1.Rows[0].Cells["服务区域"].Value != null)
                {
                    txtServiceArea.Text = dataGridView1.Rows[0].Cells["服务区域"].Value.ToString();
                }

                if (dataGridView1.Rows[0].Cells["负责人"].Value != null)
                {
                    txtPrincipal.Text = dataGridView1.Rows[0].Cells["负责人"].Value.ToString();
                }

                if (dataGridView1.Rows[0].Cells["详细地址"].Value != null)
                {
                    txtAddr.Text = dataGridView1.Rows[0].Cells["详细地址"].Value.ToString();
                }

                if (dataGridView1.Rows[0].Cells["联系人"].Value != null)
                {
                    txtLinkman.Text = dataGridView1.Rows[0].Cells["联系人"].Value.ToString();
                }

                if (dataGridView1.Rows[0].Cells["备注"].Value != null)
                {
                    txtRemark.Text = dataGridView1.Rows[0].Cells["备注"].Value.ToString();
                }
            }
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        private void btnRefreshBargain_Click(object sender, EventArgs e)
        {
            if (!m_clientServer.GetAllClient(out m_findClient, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            RefreshDataGridView(m_findClient);
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
