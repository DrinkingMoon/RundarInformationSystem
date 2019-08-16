using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using ServerModule;
using GlobalObject;
using UniversalControlLibrary;

namespace Expression
{
    public partial class 电子档案录入 : Form
    {
        /// <summary>
        /// 电子档案服务组件
        /// </summary>
        IElectronFileServer m_findElectronFile = ServerModuleFactory.GetServerModule<IElectronFileServer>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_error;

        /// <summary>
        /// 上一次的选中节点
        /// </summary>
        TreeNode m_preSelectedNode;

        public 电子档案录入()
        {
            InitializeComponent();
        }

        private void 电子档案录入_Load(object sender, EventArgs e)
        {
            // 单击进入编辑状态
            this.dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
            this.dataGridView1.AutoGenerateColumns = false;

            cmbProductType.SelectedIndex = 0;
            InDate();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (m_preSelectedNode != null)
            {
                m_preSelectedNode.BackColor = treeView1.BackColor;
            }

            m_preSelectedNode = e.Node;
            e.Node.BackColor = Color.Yellow;

            string strCode = "";
            string strParentCode = "";
            string strName = "";
            string strWorkBench = "";

            strCode = ((object[])(treeView1.SelectedNode.Tag))[3].ToString();
            strParentCode = ((object[])(treeView1.SelectedNode.Tag))[2].ToString();
            strName = ((object[])(treeView1.SelectedNode.Tag))[4].ToString();
            strWorkBench = ((object[])(treeView1.SelectedNode.Tag))[14].ToString();

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
                if (strCode == dataGridView1.Rows[i].Cells["GoodsCode"].Value.ToString()
                    && strParentCode == dataGridView1.Rows[i].Cells["ParentCode"].Value.ToString()
                    && strName == dataGridView1.Rows[i].Cells["GoodsName"].Value.ToString()
                    && strWorkBench == dataGridView1.Rows[i].Cells["WorkBench"].Value.ToString())
                {
                    dataGridView1.ClearSelection();
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                }
            }
        }

        public void FindNodeSet_Show(TreeNode tnParent, string strValue,
                                    string strParentValue,string strTxt,string strWorkBench)
        {
            if (tnParent == null)
            { return; }

            if (((object[])(tnParent.Tag))[3].ToString() == strValue
                && ((object[])(tnParent.Tag))[2].ToString() == strParentValue
                && ((object[])(tnParent.Tag))[4].ToString() == strTxt
                && ((object[])(tnParent.Tag))[14].ToString() == strWorkBench)
            {
                treeView1.SelectedNode = tnParent;
                this.treeView1.AfterSelect -= new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
                return;
            }

            foreach (TreeNode tn in tnParent.Nodes)
            {
                tn.Expand();
                FindNodeSet_Show(tn, strValue, strParentValue, strTxt, strWorkBench);
            }
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count ==0 || treeView1.Nodes.Count == 0)
            {
                return;
            }
           

            DataGridViewRow row = dataGridView1.CurrentRow;

            foreach (TreeNode tn in treeView1.Nodes)
            {
                tn.Expand();

                FindNodeSet_Show(tn, 
                    row.Cells["GoodsCode"].Value.ToString(), 
                    row.Cells["ParentCode"].Value.ToString(),
                    row.Cells["GoodsName"].Value.ToString(),
                    row.Cells["WorkBench"].Value.ToString());
            }

            //dataGridView1.CurrentRow.Selected = true;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtProductOnlyCode.Text.Trim() == "")
            {
                MessageBox.Show("请填写产品唯一编码","提示");
                return;
            }

            DataTable dt = (DataTable)dataGridView1.DataSource;

            if (!m_findElectronFile.SaveData(txtProductOnlyCode.Text,dt,BasicInfo.LoginName, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }
            else
            {
                MessageBox.Show("保存成功！","提示");
                ClearDate();
                InDate();
                return;
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            InDate();
        }

        private void InDate()
        {

            DataTable dt = m_findElectronFile.GetTreeTable(txtProductOnlyCode.Text, cmbProductType.Text, out m_error);

            treeView1.Nodes.Clear();
            dataGridView1.DataSource = null;
            GlobalObject.GeneralFunction.LoadTreeViewDt(treeView1, dt, "GoodsName", "GoodsCode", "ParentCode", "ParentCode is null");
            dataGridView1.DataSource = dt;
        }

        private void ClearDate()
        {
            txtProductOnlyCode.Text = "";
        }

        private void btnAllFind_Click(object sender, EventArgs e)
        {
            电子档案全部查询简体窗口 form = new 电子档案全部查询简体窗口();
            form.ShowDialog();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!m_findElectronFile.DeleteData(txtProductOnlyCode.Text, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }
            else
            {
                MessageBox.Show("删除成功！", "提示");
                ClearDate();
                InDate();
                return;
            }
        }
    }
}
