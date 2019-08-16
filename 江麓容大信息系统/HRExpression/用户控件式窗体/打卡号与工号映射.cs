using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Expression;
using PlatformManagement;
using Service_Peripheral_HR;
using ServerModule;
using GlobalObject;
using UniversalControlLibrary;

namespace Form_Peripheral_HR
{
    /// <summary>
    /// 打卡号与工号映射界面
    /// </summary>
    public partial class 打卡号与工号映射 : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string error;

        /// <summary>
        /// 可供查找的所有字段
        /// </summary>
        string[] m_findField = null;

        /// <summary>
        /// 权限组件
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 人员档案操作接口类
        /// </summary>
        IPersonnelArchiveServer m_personnelServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IPersonnelArchiveServer>();

        public 打卡号与工号映射(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authFlag = nodeInfo.Authority;
            RefreshDataGridView();
        }

        /// <summary>
        /// 权限控制
        /// </summary>
        /// <param name="authorityFlag">权限标志</param>
        void AuthorityControl(PlatformManagement.AuthorityFlag authorityFlag)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip1, authorityFlag);
        }

        private void 打卡号与工号映射_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        /// <summary>
        /// 刷新
        /// </summary>
        void RefreshDataGridView()
        {
            DataTable dt = m_personnelServer.GetCardIDWorkIDMapping();
            dataGridView1.DataSource = dt;

            this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
               this.dataGridView1_ColumnWidthChanged);

            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            // 添加查询用的列
            if (m_findField == null)
            {
                List<string> lstColumnName = new List<string>();

                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    if (dataGridView1.Columns[i].Visible)
                    {
                        lstColumnName.Add(dataGridView1.Columns[i].Name);
                    }
                }

                m_findField = lstColumnName.ToArray();
            }

            userControlDataLocalizer1.Init(dataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));

            dataGridView1.Refresh();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtCardID.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请输入打卡号！");
                return;
            }

            if (txtWorkID.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请输入员工工号！");
                return;
            }

            HR_CardID_WorkID_Mapping card = new HR_CardID_WorkID_Mapping();

            card.CardID = txtCardID.Text;
            card.WorkID = txtWorkID.Text;

            if (!m_personnelServer.AddCardIDWorkIDMapping(card, out error))
            {
                MessageDialog.ShowPromptMessage(error);
                return;
            }

            RefreshDataGridView();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                if (MessageBox.Show("确定要删除选中的行？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (!m_personnelServer.DeleteCardIDWorkIDMapping(dataGridView1.CurrentRow.Cells["打卡号"].Value.ToString(),
                        dataGridView1.CurrentRow.Cells["员工工号"].Value.ToString(), out error))
                    {
                        MessageDialog.ShowPromptMessage(error);
                        return;
                    }
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请选择需要删除的数据行！");
            }

            RefreshDataGridView();
        }

        private void 刷新toolStripButton_Click(object sender, EventArgs e)
        {
            RefreshDataGridView();
        }

        private void 打卡号与工号映射_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authFlag);
        }
    }
}
