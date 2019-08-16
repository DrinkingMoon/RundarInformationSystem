using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using GlobalObject;
using Expression;
using PlatformManagement;
using Service_Quality_File;
using UniversalControlLibrary;

namespace Form_Quality_File
{
    public partial class 文件类别 : Form
    {
        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr;

        /// <summary>
        /// 文件类别服务组件
        /// </summary>
        ISystemFileBasicInfo m_serviceSystemFileSort = Service_Quality_File.ServerModuleFactory.GetServerModule<ISystemFileBasicInfo>();

        /// <summary>
        /// LNQ数据集
        /// </summary>
        FM_FileSort m_lnqFileSort = new FM_FileSort();

        public 文件类别(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authFlag = nodeInfo.Authority;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
        }

        private void 文件分类_Load(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void txtParent_OnCompleteSearch()
        {
            txtParent.Text = txtParent.DataResult["类别名称"].ToString();
            txtParent.Tag = txtParent.DataResult["类别ID"];
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        void RefreshData()
        {
            ClearInfo();
            dataGridView1.DataSource = m_serviceSystemFileSort.GetAllInfo(CE_FileType.体系文件);
        }

        /// <summary>
        /// 获得数据
        /// </summary>
        void GetInfo()
        {
            m_lnqFileSort = new FM_FileSort();
            m_lnqFileSort.ParentID = txtParent.Tag.ToString() == "" ? 0 : Convert.ToInt32(txtParent.Tag);
            m_lnqFileSort.SortName = txtName.Text;
            m_lnqFileSort.SortID = txtName.Tag.ToString() == "" ? 0 : Convert.ToInt32(txtName.Tag);
            m_lnqFileSort.FileType = (int)CE_FileType.体系文件;
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        void ClearInfo()
        {
            txtName.Text = "";
            txtName.Tag = null;
            txtParent.Text = "";
            txtParent.Tag = null;
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }

            txtName.Text = dataGridView1.CurrentRow.Cells["类别名称"].Value.ToString();
            txtName.Tag = dataGridView1.CurrentRow.Cells["类别ID"].Value;
            txtParent.Text = dataGridView1.CurrentRow.Cells["父级类别名称"].Value.ToString();
            txtParent.Tag = dataGridView1.CurrentRow.Cells["父级类别ID"].Value;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            GetInfo();

            if (!m_serviceSystemFileSort.Operator(CE_OperatorMode.添加,m_lnqFileSort,out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
            }
            else
            {
                MessageDialog.ShowPromptMessage("添加成功");
                RefreshData();
            }
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            GetInfo();

            if (!m_serviceSystemFileSort.Operator(CE_OperatorMode.修改, m_lnqFileSort, out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
            }
            else
            {
                MessageDialog.ShowPromptMessage("修改成功");
                RefreshData();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            GetInfo();


            if (MessageDialog.ShowEnquiryMessage("您确定要删除【" + m_lnqFileSort.SortName + "】业务单据吗？") == DialogResult.Yes)
            {
                if (!m_serviceSystemFileSort.Operator(CE_OperatorMode.删除, m_lnqFileSort, out m_strErr))
                {
                    MessageDialog.ShowPromptMessage(m_strErr);
                }
                else
                {
                    MessageDialog.ShowPromptMessage("删除成功");
                    RefreshData();
                }
            }
        }
    }
}
