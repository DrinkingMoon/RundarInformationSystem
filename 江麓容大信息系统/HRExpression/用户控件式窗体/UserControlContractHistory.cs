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
    /// 员工合同变更历史界面
    /// </summary>
    public partial class UserControlContractHistory : Form
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
        /// 操作权限
        /// </summary>
        PlatformManagement.AuthorityFlag m_authorityFlag;

        /// <summary>
        /// 查询结果
        /// </summary>
        IQueryResult m_queryResult;

        /// <summary>
        /// 合同管理服务类
        /// </summary>
        ILaborContractServer m_laborServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<ILaborContractServer>();

        public UserControlContractHistory(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authorityFlag = nodeInfo.Authority;

            RefreshControl();
        }

        /// <summary>
        /// 权限控制
        /// </summary>
        /// <param name="authorityFlag">权限标志</param>
        void AuthorityControl(PlatformManagement.AuthorityFlag authorityFlag)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip1, authorityFlag);
        }

        /// <summary>
        /// 刷新
        /// </summary>
        private void RefreshControl()
        {
            if (!m_laborServer.GetAllContarctHistory(out m_queryResult, out error))
            {
                MessageDialog.ShowErrorMessage(error);
                return;
            }

            DataTable dt = m_queryResult.DataCollection.Tables[0];
            dataGridView1.DataSource = dt;

            if (dt != null && dt.Rows.Count > 0)
            {
                dataGridView1.Columns["编号"].Visible = false;
                dataGridView1.Columns["附件"].Visible = false;
                dataGridView1.Columns["附件名"].Visible = false;
            }

            dataGridView1.Refresh();

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
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtProposer.Text = dataGridView1.CurrentRow.Cells["员工姓名"].Value.ToString();
            txtProposer.Tag = dataGridView1.CurrentRow.Cells["员工编号"].Value.ToString();
            txtRemark.Text = dataGridView1.CurrentRow.Cells["备注"].Value.ToString();
            txtTemplet.Text = dataGridView1.CurrentRow.Cells["合同类别"].Value.ToString()+
                              " " + dataGridView1.CurrentRow.Cells["合同版本"].Value.ToString();
            cmbStatus.Text = dataGridView1.CurrentRow.Cells["合同状态"].Value.ToString();
            dtpBeginTime.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["合同起始时间"].Value);
            dtpEndTime.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["合同终止时间"].Value);
        }

        private void 综合查询toolStripButton3_Click(object sender, EventArgs e)
        {
            IAuthorization authorization = PlatformFactory.GetObject<IAuthorization>();
            string businessID = "员工合同历史查询";
            IQueryResult qr = authorization.Query(businessID, null, null, 0);
            List<string> lstFindField = new List<string>();
            DataColumnCollection columns = qr.DataCollection.Tables[0].Columns;

            if (qr.Succeeded && columns.Count > 0)
            {
                for (int i = 0; i < columns.Count; i++)
                {
                    lstFindField.Add(columns[i].ColumnName);
                }
            }

            FormConditionFind formFindCondition = new FormConditionFind(this, lstFindField.ToArray(), businessID, labelTitle.Text);
            formFindCondition.ShowDialog();
        }

        private void 刷新toolStripButton1_Click(object sender, EventArgs e)
        {
            RefreshControl();
        }

        private void userControlDataLocalizer1_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        private void UserControlContractHistory_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authorityFlag);
        }
    }
}
