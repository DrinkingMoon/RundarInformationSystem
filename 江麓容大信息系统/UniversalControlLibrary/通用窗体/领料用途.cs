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

namespace UniversalControlLibrary
{
    /// <summary>
    /// 领料用途选择对话框
    /// </summary>
    public partial class 领料用途 : Form
    {
        /// <summary>
        /// 领料单用途服务
        /// </summary>
        IMaterialRequisitionPurposeServer m_purposeServer = 
            ServerModuleFactory.GetServerModule<IMaterialRequisitionPurposeServer>();

        /// <summary>
        /// 用于自动完成用的数据源
        /// </summary>
        List<string> m_autoCompleteSource = new List<string>();

        /// <summary>
        /// 获取选中的数据
        /// </summary>
        public BASE_MaterialRequisitionPurpose SelectedData
        {
            get;
            set;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public 领料用途()
        {
            InitializeComponent();

            InitTreeView(m_purposeServer.GetAllPurpose().ToList());

            string[] dataItems = m_autoCompleteSource.ToArray();
            cmbCode.DataSource = m_autoCompleteSource;
            cmbCode.AutoCompleteSource = AutoCompleteSource.ListItems;

            PositioningNode(cmbCode.Text);
        }

        /// <summary>
        /// 用信息列表初始化树
        /// </summary>
        /// <param name="lstDepotInfo">材料类别信息</param>
        void InitTreeView(List<BASE_MaterialRequisitionPurpose> lstDepotInfo)
        {
            for (; lstDepotInfo.Count > 0;)
            {
                TreeNode node = GenerateNode(lstDepotInfo[0]);
                treeView1.Nodes.Add(node);

                if (!lstDepotInfo[0].IsEnd)
                {
                    lstDepotInfo.RemoveAt(0);
                    BuildTreeView(node, lstDepotInfo);
                }
                else
                {
                    lstDepotInfo.RemoveAt(0);
                }
            }

            treeView1.ExpandAll();
        }

        /// <summary>
        /// 生成树节点
        /// </summary>
        /// <param name="info">用来生成树节点的信息</param>
        /// <returns>返回生成的树节点</returns>
        TreeNode GenerateNode(BASE_MaterialRequisitionPurpose info)
        {
            TreeNode node = new TreeNode();
            node.Name = string.Format("{0} {1}", info.Code, info.Purpose);
            node.Text = node.Name;
            node.Tag = info;

            if (info.IsEnd)
            {
                node.ContextMenuStrip = contextMenuStrip;
                m_autoCompleteSource.Add(node.Text);
            }
            else
            {
                node.ForeColor = Color.Tomato;
            }

            return node;
        }

        void BuildTreeView(TreeNode parentNode, List<BASE_MaterialRequisitionPurpose> lstDepotInfo)
        {
            BASE_MaterialRequisitionPurpose parentInfo = parentNode.Tag as BASE_MaterialRequisitionPurpose;

            for (int i = 0; i < lstDepotInfo.Count; i++)
            {
                if (parentInfo.Code.Length + 2 != lstDepotInfo[i].Code.Length)
                    break;

                TreeNode node = GenerateNode(lstDepotInfo[i]);
                parentNode.Nodes.Add(node);

                lstDepotInfo.RemoveAt(i);
                i--;

                if (!(node.Tag as BASE_MaterialRequisitionPurpose).IsEnd)
                {
                    BuildTreeView(node, lstDepotInfo);
                }
            }
        }

        /// <summary>
        /// 检查数据
        /// </summary>
        /// <param name="data">要检查的数据项</param>
        /// <returns>正确返回true</returns>
        bool CheckData(BASE_MaterialRequisitionPurpose data)
        {
            if (data == null || !data.IsEnd)
            {
                MessageDialog.ShowPromptMessage("必须要选择最终分类(即末节点), 而不允许选择大类");
                return false;
            }
            else if (data.Code == "1602")
            {
                MessageDialog.ShowPromptMessage("此选项不可选择");
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 双击节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Nodes.Count > 0)
            {
                MessageDialog.ShowPromptMessage("只能选择叶子节点");
                return;
            }

            SelectedData = e.Node.Tag as BASE_MaterialRequisitionPurpose;

            if (CheckData(SelectedData))
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        /// <summary>
        /// 右击树节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                treeView1.SelectedNode = e.Node;
            }
        }

        /// <summary>
        /// 点击选中菜单项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemSelectType_Click(object sender, EventArgs e)
        {
            if (CheckData(SelectedData))
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        /// <summary>
        /// 根据树节点名称定位节点
        /// </summary>
        /// <param name="name">节点名称</param>
        private void PositioningNode(string name)
        {
            if (GlobalObject.GeneralFunction.IsNullOrEmpty(name))
            {
                return;
            }

            TreeNode[] nodes = treeView1.Nodes.Find(name, true);

            if (nodes == null || nodes.Length == 0)
            {
                MessageDialog.ShowPromptMessage(string.Format("没有找到你要的信息：{0}", name));
                return;
            }

            treeView1.Focus();
            treeView1.SelectedNode = nodes[0];
        }

        private void cmbCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
            {
                PositioningNode(cmbCode.Text);
            }
        }

        private void cmbCode_Leave(object sender, EventArgs e)
        {
            PositioningNode(cmbCode.Text);
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (SelectedData == null)
            {
                MessageDialog.ShowPromptMessage(string.Format("没有找到你要的信息：{0}", cmbCode.Text));
                return;
            }

            if (CheckData(SelectedData))
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Nodes.Count == 0)
            {
                SelectedData = e.Node.Tag as BASE_MaterialRequisitionPurpose;
                cmbCode.Text = e.Node.Text;
            }
            else
            {
                SelectedData = null;
                cmbCode.Text = "";
            }
        }
    }
}
