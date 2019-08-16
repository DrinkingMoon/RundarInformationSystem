using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using PlatformManagement;
using GlobalObject;
using UniversalControlLibrary;

namespace Expression
{
    public partial class 工装与工位匹配管理 : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr;

        /// <summary>
        /// 工装台帐服务组件
        /// </summary>
        IFrockStandingBook m_serverFrockStandingBook = PMS_ServerFactory.GetServerModule<IFrockStandingBook>();

        /// <summary>
        /// 实体集
        /// </summary>
        S_FrockOfWorkBenchSetting m_lnqFrockOfWorkBenchSet = new S_FrockOfWorkBenchSetting();

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag;

        public 工装与工位匹配管理(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authFlag = nodeInfo.Authority;

            RefreshData();
        }

        private void 工装与工位匹配管理_Load(object sender, EventArgs e)
        {
            FaceAuthoritySetting.SetEnable(this.Controls, m_authFlag);
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        void RefreshData()
        {
            dataGridView1.DataSource = m_serverFrockStandingBook.GetFrockOfWorkBenchInfo();

            userControlDataLocalizer1.Init(dataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
        }

        void GetMessage()
        {
            m_lnqFrockOfWorkBenchSet.FrockNumber = txtFrockNumber.Text;
            m_lnqFrockOfWorkBenchSet.WorkBench = txtWorkBench.Text;
        }

        private void btnContentNew_Click(object sender, EventArgs e)
        {
            this.txtCode.Text = "";
            this.txtFrockNumber.Text = "";
            txtName.Text = "";
            txtWorkBench.Text = "";
        }

        private void btnContentAdd_Click(object sender, EventArgs e)
        {
            GetMessage();

            if (!m_serverFrockStandingBook.AddFrockOfWorkBench(m_lnqFrockOfWorkBenchSet, out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
            }
            else
            {
                MessageDialog.ShowPromptMessage("添加成功");
            }

            RefreshData();
        }

        private void btnContentDelete_Click(object sender, EventArgs e)
        {
            GetMessage();

            if (MessageDialog.ShowEnquiryMessage("您确定要删除吗？") == DialogResult.Yes)
            {
                if (!m_serverFrockStandingBook.DeleteFrockOfWorkBench(m_lnqFrockOfWorkBenchSet, out m_strErr))
                {
                    MessageDialog.ShowPromptMessage(m_strErr);
                }
                else
                {
                    MessageDialog.ShowPromptMessage("删除成功");
                }
            }

            RefreshData();
        }

        private void btnContentUpdate_Click(object sender, EventArgs e)
        {
            GetMessage();

            S_FrockOfWorkBenchSetting lnqOldData = new S_FrockOfWorkBenchSetting();

            lnqOldData.WorkBench = dataGridView1.CurrentRow.Cells["工位"].Value.ToString();
            lnqOldData.FrockNumber = dataGridView1.CurrentRow.Cells["工装编号"].Value.ToString();

            if (!m_serverFrockStandingBook.UpdateFrockOfWorkBench(lnqOldData, m_lnqFrockOfWorkBenchSet, out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
            }
            else
            {
                MessageDialog.ShowPromptMessage("更新成功");
            }

            RefreshData();
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }
            else
            {
                txtCode.Text = dataGridView1.CurrentRow.Cells["工装图号"].Value.ToString();
                txtFrockNumber.Text = dataGridView1.CurrentRow.Cells["工装编号"].Value.ToString();
                txtName.Text = dataGridView1.CurrentRow.Cells["工装名称"].Value.ToString();
                txtWorkBench.Text = dataGridView1.CurrentRow.Cells["工位"].Value.ToString();
            }
        }

        private void txtName_OnCompleteSearch()
        {
            txtCode.Text = txtName.DataResult["工装图号"].ToString();
            txtName.Text = txtName.DataResult["工装名称"].ToString();
            txtFrockNumber.Text = txtName.DataResult["工装编号"].ToString();
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

        private void txtName_Enter(object sender, EventArgs e)
        {
            txtName.StrEndSql = " and 工装编号 in (select FrockNumber from S_FrockStandingBook where IdentifyCycleType = '计数') ";
        }
    }
}
