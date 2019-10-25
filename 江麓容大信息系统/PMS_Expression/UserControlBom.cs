/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  UserControlBom.cs
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
using PlatformManagement;
using UniversalControlLibrary;

namespace Expression
{
  
    /// <summary>
    /// Bom组件
    /// </summary>
    public partial class UserControlBom : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// Bom
        /// </summary>
        Dictionary<string, List<Bom>> m_dicBom;

        /// <summary>
        /// 产品类型父总成零部件编码字典, 用于检查是否存在“父总成编码+零件编码+规格”都一致的零件（防止添加、更新时出现问题）
        /// </summary>
        Dictionary<string, List<string>> m_dicParentAssemblyPartCode = new Dictionary<string, List<string>>();

        /// <summary>
        /// 服务组件
        /// </summary>
        IProductInfoServer m_productInfoServer = ServerModuleFactory.GetServerModule<IProductInfoServer>();

        /// <summary>
        /// 服务组件
        /// </summary>
        IBomServer m_bomServer = ServerModuleFactory.GetServerModule<IBomServer>();

        /// <summary>
        /// 发生了改变的BOM信息，记录添加或删除的节点
        /// </summary>
        List<Bom> m_updatedBomInfo = new List<Bom>();

        /// <summary>
        /// 数据定位控件
        /// </summary>
        UserControlDataLocalizer m_dataLocalizer;

        /// <summary>
        /// 上一次的选中节点
        /// </summary>
        TreeNode m_preSelectedNode;

        /// <summary>
        /// 通过数据显示控件行来选择树节点的标志
        /// </summary>
        bool m_selecteTreeNodeFromDataGridViewRow;

        /// <summary>
        /// Bom
        /// </summary>
        Dictionary<string, List<Bom>> DicBom
        {
            get { return m_dicBom; }
            set { m_dicBom = value; }
        }

        /// <summary>
        /// 权限组件
        /// </summary>
        AuthorityFlag m_authFlag;

        public UserControlBom(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authFlag = nodeInfo.Authority;

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
        }

        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControlBom_Load(object sender, EventArgs e)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip1, m_authFlag);

            this.cmbProductType.SelectedIndexChanged -= new System.EventHandler(this.cmbProductType_SelectedIndexChanged);

            #region 获取所有产品编码(产品类型)信息

            List<string> lstAsscembly = m_bomServer.GetAssemblyTypeList();

            if (lstAsscembly != null)
            {
                cmbProductType.DataSource = lstAsscembly;

                if (cmbProductType.Items.Count > 0)
                {
                    cmbProductType.SelectedIndex = -1;
                }
            }
            #endregion

            this.cmbProductType.SelectedIndexChanged += new System.EventHandler(this.cmbProductType_SelectedIndexChanged);
        }

        /// <summary>
        /// 改变组件大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControlBom_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        /// <summary>
        /// 刷新DataGridView
        /// </summary>
        /// <param name="edition">版本</param>
        void RefreshDataGridView()
        {
            try
            {
                this.dataGridView1.CellEnter -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
                DataTable dataTable;

                if (!m_bomServer.GetBom(cmbProductType.Text, cmbDBOMVersion.Text, out dataTable, out m_err))
                {
                    MessageDialog.ShowErrorMessage(m_err);
                    return;
                }

                dataGridView1.DataSource = dataTable;
                dataGridView1.Columns["序号"].Visible = false;

                // 添加数据定位控件
                if (m_dataLocalizer == null)
                {
                    m_dataLocalizer = new UserControlDataLocalizer(dataGridView1, this.Name, 
                        UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));

                    panelSearch.Controls.Add(m_dataLocalizer);
                    m_dataLocalizer.Dock = DockStyle.Bottom;
                    m_dataLocalizer.Visible = true;
                }

                this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                    this.dataGridView1_ColumnWidthChanged);

                ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

                this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                    this.dataGridView1_ColumnWidthChanged);

                dataGridView1.Refresh();
            }
            catch (Exception err)
            {
                MessageDialog.ShowErrorMessage(err.Message);
            }
            finally
            {
                this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
            }
        }

        /// <summary>
        /// 刷新树视图
        /// </summary>
        /// <param name="edition"></param>
        void RefreshTreeView()
        {
            string edition = cmbProductType.Text;

            string error;
            Dictionary<string, List<Bom>> DicBomTable;
            Dictionary<string, List<Bom>> tempDic;

            m_updatedBomInfo.Clear();

            if (m_bomServer.GetBom(cmbProductType.Text, cmbDBOMVersion.Text, out tempDic, out m_err))
            {
                DicBom = tempDic;
            }
            else
            {
                if (m_dicParentAssemblyPartCode.ContainsKey(edition))
                {
                    m_dicParentAssemblyPartCode[edition].Clear();
                }
                else
                {
                    List<string> list = new List<string>();
                    m_dicParentAssemblyPartCode.Add(edition, list);
                }

                MessageDialog.ShowErrorMessage(m_err);

                treeView1.Nodes.Clear();
                ResetPanelPara();
                return;
            }

            treeView1.Nodes.Clear();

            if (m_dicParentAssemblyPartCode.ContainsKey(edition))
            {
                m_dicParentAssemblyPartCode[edition].Clear();
            }

            if (m_bomServer.GetBom(cmbProductType.Text, cmbDBOMVersion.Text, out DicBomTable, out error))
            {
                List<Bom> listBom = DicBomTable[edition];

                for (int i = 0; i < listBom.Count; i++)
                {
                    if (!m_dicParentAssemblyPartCode.ContainsKey(edition))
                    {
                        List<string> list = new List<string>();

                        list.Add(listBom[i].ParentCode + listBom[i].PartCode + listBom[i].Spec);
                        m_dicParentAssemblyPartCode.Add(edition, list);
                    }
                    else
                    {
                        m_dicParentAssemblyPartCode[edition].Add(listBom[i].ParentCode + listBom[i].PartCode + listBom[i].Spec);
                    }

                    if (listBom[i].ParentCode == "")
                    {
                        TreeNode node = new TreeNode();

                        node.Name = listBom[i].PartCode;
                        node.Text = listBom[i].PartName;
                        node.ToolTipText = listBom[i].Spec;
                        node.Tag = listBom[i];
                        treeView1.Nodes.Add(node);
                        DicBomTable[edition].RemoveAt(i--);
                    }

                }

                for (int i = 0; i < treeView1.Nodes.Count; i++)
                {
                    RecursionBuildTreeView(treeView1.Nodes[i], DicBomTable, edition);
                }

                if (treeView1.Nodes.Count > 0)
                {
                    ChangePara(treeView1.Nodes[0]);
                }

            }
            else
            {
                MessageDialog.ShowErrorMessage(error);

                ResetPanelPara();
                return;
            }

            treeView1.ExpandAll();
        }

        /// <summary>
        /// 初始化树视图
        /// </summary>
        /// <param name="edition">版本号</param>
        void InitTreeView()
        {
            if (cmbProductType.Text.Trim().Length == 0 || cmbDBOMVersion.Text.Trim().Length == 0)
            {
                return;
            }

            RefreshDataGridView();

            this.treeView1.AfterSelect -= new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);

            RefreshTreeView();

            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);

            if (treeView1.Nodes.Count > 0)
            {
                treeView1.SelectedNode = treeView1.Nodes[0];
            }
        }

        /// <summary>
        /// 重置右面板参数
        /// </summary>
        void ResetPanelPara()
        {
            txtParentCode.Text = "";
            txtCode.Text = "";
            txtName.Text = "";
            txtSpec.Text = "";
            txtVersion.Text = "";
            numBasicCount.Value = 0;
            txtRemark.Text = "";
        }

        /// <summary>
        /// 递归生成Bom表的树型结构
        /// </summary>
        /// <param name="parentNode">父总成编码</param>
        /// <param name="dicBomTable">Bom字典</param>
        /// <param name="edition">版本号</param>
        void RecursionBuildTreeView(TreeNode parentNode, Dictionary<string, List<Bom>> dicBomTable, string edition)
        {
            for (int i = 0; i < dicBomTable[edition].Count; i++)
            {
                Bom bom = dicBomTable[edition][i];

                if (parentNode.Name == bom.ParentCode)
                {
                    TreeNode node = new TreeNode();

                    node.Name = bom.PartCode;
                    node.Text = bom.PartName;
                    node.ToolTipText = bom.Spec;
                    node.Tag = bom;
                    parentNode.Nodes.Add(node);
                    dicBomTable[edition].RemoveAt(i);

                    i = -1;

                    if (bom.AssemblyFlag)
                        RecursionBuildTreeView(node, dicBomTable, edition);
                }
            }
        }

        /// <summary>
        /// 更改右边参数的显示
        /// </summary>
        /// <param name="node">用于更新显示的节点</param>
        void ChangePara(TreeNode node)
        {
            Bom bom = node.Tag as Bom;

            if (bom.ParentCode != null)
                txtParentCode.Text = bom.ParentCode;

            txtCode.Text = bom.PartCode;
            txtName.Text = bom.PartName;
            txtSpec.Text = bom.Spec;
            txtVersion.Text = bom.Version;

            numBasicCount.Value = Convert.ToDecimal(bom.Counts);
        }

        /// <summary>
        /// 更改选定treeView1内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ResetPanelPara();
            ChangePara(e.Node);

            if (m_preSelectedNode != null)
            {
                m_preSelectedNode.BackColor = treeView1.BackColor;
            }

            m_preSelectedNode = e.Node;
            e.Node.BackColor = Color.Yellow;

            if (!m_selecteTreeNodeFromDataGridViewRow && dataGridView1.Rows.Count > 0)
            {
                Bom bom = treeView1.SelectedNode.Tag as Bom;

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
                    if (dataGridView1.Rows[i].Cells["父总成编码"].Value.ToString() 
                        == bom.ParentCode && dataGridView1.Rows[i].Cells["零部件编码"].Value.ToString() == bom.PartCode)
                    {
                        dataGridView1.ClearSelection();
                        dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                        break;
                    }
                }
            }

            m_selecteTreeNodeFromDataGridViewRow = false;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            InitTreeView();
        }

        /// <summary>
        /// 改变产品类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbProductType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbDBOMVersion.DataSource = null;
            cmbDBOMVersion.Items.Clear();

            if (cmbProductType.Text.Trim().Length == 0)
            {
                return;
            }

            DataTable dtTemp = m_bomServer.GetBomBackUpBomEdtion(cmbProductType.Text.Trim());

            cmbDBOMVersion.ValueMember = "设计BOM版本";
            cmbDBOMVersion.DisplayMember = "设计BOM版本";
            cmbDBOMVersion.DataSource = dtTemp;
            cmbDBOMVersion.SelectedIndex = 0;
        }

        /// <summary>
        /// 添加待加节点的父节点位于树中的位置
        /// </summary>
        /// <param name="edition">版本号</param>
        /// <param name="node">待查节点</param>
        /// <param name="addNode">要添加的节点</param>
        /// <param name="flag">是否找到添加的子节点</param>
        void FindAddNode(string edition, TreeNode node, TreeNode addNode, out bool flag)
        {
            flag = false;

            for (int i = 0; i < node.Nodes.Count; i++)
            {
                if (node.Nodes[i].Parent.Name == txtParentCode.Text)
                {
                    if (!node.Nodes[i].Parent.Nodes.Contains(addNode))
                    {
                        node.Nodes[i].Parent.Nodes.Add(addNode);
                        flag = true;
                        break;
                    }
                }

                if (!flag)
                {
                    if (node.Nodes[i].Nodes.Count > 0)
                    {
                        FindAddNode(edition, node.Nodes[i], addNode, out flag);
                    }
                    else
                    {
                        if (node.Nodes[i].Name == txtParentCode.Text)
                        {
                            if (!node.Nodes[i].Nodes.Contains(addNode))
                            {
                                node.Nodes[i].Nodes.Add(addNode);
                                flag = true;
                                return;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 查找子节点
        /// </summary>
        /// <param name="edition">版本号</param>
        /// <param name="node">树中某一节点</param>
        void FindChildNode(string edition, TreeNode node)
        {
            for (int i = 0; i < node.Nodes.Count; i++)
            {
                if (node.Nodes[i].Nodes.Count > 0)
                {
                    FindChildNode(edition, node.Nodes[i]);
                }
                else
                {
                    for (int j = 0; j < DicBom[edition].Count; j++)
                    {
                        if (DicBom[edition][j].ParentCode == node.Nodes[i].Parent.Name 
                            && DicBom[edition][j].PartCode == node.Nodes[i].Name 
                            && DicBom[edition][j].Spec == node.Nodes[i].ToolTipText)
                        {
                            if (m_dicParentAssemblyPartCode.ContainsKey(edition))
                            {
                                m_dicParentAssemblyPartCode[edition].Remove(node.Nodes[i].Parent.Name 
                                    + node.Nodes[i].Name + node.Nodes[i].ToolTipText);
                            }

                            DicBom[edition].RemoveAt(j);
                        }
                    }

                    node.Nodes[i].Remove();
                }
            }
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            treeView1.SelectedNode = e.Node;
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            string strColName = "";

            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                if (col.Visible)
                {
                    strColName = col.Name;
                    break;
                }
            }
            dataGridView1.ClearSelection();
            dataGridView1.CurrentCell = dataGridView1.Rows[e.RowIndex].Cells[strColName];
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || treeView1.Nodes.Count == 0)
            {
                return;
            }

            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
            TreeNode[] findNodes = treeView1.Nodes.Find(row.Cells["零部件编码"].Value.ToString(), true);

            foreach (TreeNode node in findNodes)
            {
                Bom bom = node.Tag as Bom;

                if (bom.ParentCode == row.Cells["父总成编码"].Value.ToString())
                {
                    m_selecteTreeNodeFromDataGridViewRow = true;
                    treeView1.SelectedNode = node;
                    break;
                }
            }
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
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

        private void cmbDBOMVersion_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitTreeView();
        }
    }
}
