using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;

namespace UniversalControlLibrary
{
    /// <summary>
    /// 仓库类别(材料类别)对话框
    /// </summary>
    public partial class FormDepotType : Form
    {
        /// <summary>
        /// 仓库信息服务
        /// </summary>
        IMaterialTypeServer m_depotServer = ServerModuleFactory.GetServerModule<IMaterialTypeServer>();

        /// <summary>
        /// 用于自动完成用的数据源
        /// </summary>
        List<string> m_autoCompleteSource = new List<string>();

        /// <summary>
        /// 获取选中的仓库类别
        /// </summary>
        public View_S_Depot SelectedDepotType
        {
            get;
            set;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public FormDepotType()
        {
            InitializeComponent();

            IQueryable<View_S_Depot> depot = null;
            string error = null;

            if (m_depotServer.GetAllMaterialType(out depot, out error))
            {
                InitTreeView(depot.ToList());
            }

            string[] dataItems = m_autoCompleteSource.ToArray();

            cmbCode.DataSource = m_autoCompleteSource;
            cmbCode.AutoCompleteSource = AutoCompleteSource.ListItems;

            PositioningNode(cmbCode.Text);
        }

        /// <summary>
        /// 用信息列表初始化树
        /// </summary>
        /// <param name="lstDepotInfo">材料类别信息</param>
        void InitTreeView(List<View_S_Depot> lstDepotInfo)
        {
            TreeNode rootNode = new TreeNode();
            rootNode.Text = "材料类别";
            rootNode.Name = rootNode.Text;

            treeView1.Nodes.Add(rootNode);

            for (;lstDepotInfo.Count > 0;)
            {
                TreeNode node = GenerateNode(lstDepotInfo[0]);
                rootNode.Nodes.Add(node);
                lstDepotInfo.RemoveAt(0);

                BuildTreeView(node, lstDepotInfo);
            }
            
            treeView1.ExpandAll();
        }

        /// <summary>
        /// 生成树节点
        /// </summary>
        /// <param name="depotInfo">仓库信息</param>
        /// <returns>返回生成的树节点</returns>
        TreeNode GenerateNode(View_S_Depot depotInfo)
        {
            TreeNode node = new TreeNode();
            node.Name = string.Format("{0} {1}", depotInfo.仓库编码, depotInfo.仓库名称);
            node.Text = node.Name;
            node.Tag = depotInfo;

            if (depotInfo.是否末级)
            {
                node.ContextMenuStrip = contextMenuStrip;
                m_autoCompleteSource.Add(node.Text);
            }

            return node;
        }

        /// <summary>
        /// 创建功能树
        /// </summary>
        /// <param name="parentNode">父级节点</param>
        /// <param name="lstDepotInfo">材料类别列表</param>
        void BuildTreeView(TreeNode parentNode, List<View_S_Depot> lstDepotInfo)
        {
            View_S_Depot parentInfo = parentNode.Tag as View_S_Depot;

            for (int i = 0; i < lstDepotInfo.Count; i++)
            {
                if (parentInfo.仓库编码.Length + 2 != lstDepotInfo[i].仓库编码.Length)
                    break;

                TreeNode node = GenerateNode(lstDepotInfo[i]);
                parentNode.Nodes.Add(node);

                lstDepotInfo.RemoveAt(i);
                i--;

                if (!(node.Tag as View_S_Depot).是否末级)
                {
                    BuildTreeView(node, lstDepotInfo);
                }
            }
        }

        ///// <summary>
        ///// 双击节点
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        //{
        //    if (e.Node.Nodes.Count > 0)
        //    {
        //        MessageDialog.ShowPromptMessage("只能选择叶子节点");
        //        return;
        //    }

        //    SelectedDepotType = e.Node.Tag as View_S_Depot;
        //    this.DialogResult = DialogResult.OK;
        //}

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
            this.DialogResult = DialogResult.OK;
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

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (SelectedDepotType == null)
            {
                MessageDialog.ShowPromptMessage(string.Format("没有找到你要的信息：{0}", cmbCode.Text));
                return;
            }

            DialogResult = DialogResult.OK;
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Nodes.Count == 0)
            {
                SelectedDepotType = e.Node.Tag as View_S_Depot;
            }
            else
            {
                SelectedDepotType = null;
            }
        }
    }
}
