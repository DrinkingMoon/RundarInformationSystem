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

namespace Expression
{
    /// <summary>
    /// 材料类别选择界面
    /// </summary>
    public partial class 类别选择窗体 : Form
    {
        /// <summary>
        /// NodeTag数组
        /// </summary>
        private DataTable m_dtNodeTag = new DataTable();

        public DataTable DtNodeTag
        {
            get { return m_dtNodeTag; }
            set { m_dtNodeTag = value; }
        }

        /// <summary>
        /// 材料类型与管理人关系
        /// </summary>
        IDepotTypeForPersonnel m_serverDepotTypeForPersonnel = ServerModuleFactory.GetServerModule<IDepotTypeForPersonnel>();

        /// <summary>
        /// 材料类型
        /// </summary>
        IQueryable<S_Depot> m_findDepotType;

        public 类别选择窗体()
        {
            InitializeComponent();

            m_dtNodeTag.Columns.Add("String");

            m_findDepotType = m_serverDepotTypeForPersonnel.GetDepotTypeBill();

            GlobalObject.GeneralFunction.LoadTreeViewDt(treeView1,
               m_serverDepotTypeForPersonnel.ChangeDataTable(GlobalObject.GeneralFunction.ConvertToDataTable<S_Depot>(m_findDepotType)),
               "DepotName", "DepotCode", "RootSign", "RootSign = 'Root'");

            treeView1.ExpandAll();
        }

        #region 树的操作

        #region 设置CheckBox
        /// <summary>
        /// 递归节点
        /// </summary>
        /// <param name="tn">树节点</param>
        /// <param name="blChecked">选择状态</param>
        private void SetNodeCheckStatus(TreeNode tn, bool blChecked)
        {

            if (tn == null) return;

            foreach (TreeNode tnChild in tn.Nodes)
            {

                tnChild.Checked = blChecked;
                SetNodeCheckStatus(tnChild, blChecked);

            }

            TreeNode tnParent = tn;
        }

        /// <summary>
        /// 设置check状态
        /// </summary>
        /// <param name="node">树节点</param>
        private void SetNodeStyle(TreeNode node)
        {
            int nNodeCount = 0;
            if (node.Nodes.Count != 0)
            {
                foreach (TreeNode tnTemp in node.Nodes)
                {
                    if (tnTemp.Checked == true)

                        nNodeCount++;
                }

                if (nNodeCount == node.Nodes.Count)
                {
                    node.Checked = true;
                    node.ExpandAll();
                    node.ForeColor = Color.Black;
                }
                else if (nNodeCount == 0)
                {
                    node.Checked = false;
                    node.ForeColor = Color.Black;
                }
                else
                {
                    node.Checked = true;
                    node.ForeColor = Color.Gray;
                }
            }
            //当前节点选择完后，判断父节点的状态，调用此方法递归。
            if (node.Parent != null)
                SetNodeStyle(node.Parent);
        }
        #endregion


        #region 获取叶子节点值
        /// <summary>
        /// 获取被选中的叶子节点的值
        /// </summary>
        /// <param name="trContrl">功能树</param>
        private void GetNodeValues(TreeView trContrl)
        {
            foreach (TreeNode tn in trContrl.Nodes)
            {
                if (tn.Nodes.Count == 0 && tn.Checked == true)
                {
                    DataRow dr = m_dtNodeTag.NewRow();
                    dr[0] = tn.Tag.ToString();
                    m_dtNodeTag.Rows.Add(dr);
                }
                else
                {
                    FindNode(tn);
                }
            }
        }

        /// <summary>
        /// 查询递归
        /// </summary>
        /// <param name="node">树节点</param>
        private void FindNode(TreeNode node)
        {
            foreach (TreeNode tn in node.Nodes)
            {
                if (tn.Nodes.Count == 0 && tn.Checked == true)
                {
                    DataRow dr = m_dtNodeTag.NewRow();
                    dr[0] = tn.Tag.ToString();
                    m_dtNodeTag.Rows.Add(dr);
                }
                else
                {
                    FindNode(tn);
                }
            }
        }
        #endregion


        #region 清空节点Check状态
        /// <summary>
        /// 根节点起始
        /// </summary>
        /// <param name="tvControl">功能树</param>
        public void SetRoot(TreeView tvControl)
        {
            foreach (TreeNode tn in tvControl.Nodes)
            {
                tn.Collapse();
                SetNode(tn);
                tn.Checked = false;
            }
        }

        /// <summary>
        /// 子节点递归
        /// </summary>
        /// <param name="tnParent">父级节点</param>
        public void SetNode(TreeNode tnParent)
        {
            if (tnParent == null)
            { return; }

            foreach (TreeNode tn in tnParent.Nodes)
            {
                tn.Collapse();
                SetNode(tn);
                tn.Checked = false;
            }
        }

        #endregion

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action != TreeViewAction.Unknown)
            {
                SetNodeCheckStatus(e.Node, e.Node.Checked);
                SetNodeStyle(e.Node);
            }
        }

        #endregion

        private void btnOK_Click(object sender, EventArgs e)
        {
            GetNodeValues(treeView1);
            this.Close();
        }

    }
}
