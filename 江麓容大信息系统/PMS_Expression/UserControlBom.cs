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
        /// BOM表是否发生了变化的标志
        /// </summary>
        bool m_changeFlag = false;

        /// <summary>
        /// 老的Bom版本
        /// </summary>
        string m_oldEdition = "";

        /// <summary>
        /// 所有需要选配的零件编码列表
        /// </summary>
        //List<string> m_choseConfectAccessoryCodeList = new List<string>();

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
                    cmbProductType.SelectedIndex = 0;

                    InitTreeView(cmbProductType.SelectedItem.ToString());
                }
            }
            #endregion

            this.cmbProductType.SelectedIndexChanged += new System.EventHandler(this.cmbProductType_SelectedIndexChanged);
        }

        /// <summary>
        /// 重新窗体消息处理函数
        /// </summary>
        /// <param name="m">窗体消息</param>
        protected override void DefWndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WndMsgSender.CloseMsg:
                    if (m_changeFlag)
                    {
                        if (MessageDialog.ShowEnquiryMessage("是否存储更新后的Bom表?") == DialogResult.Yes)
                        {
                            SaveBom();
                        }
                    }
                    break;

                default:
                    base.DefWndProc(ref m);
                    break;
            }
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
        void RefreshDataGridView(string edition)
        {
            try
            {
                this.dataGridView1.CellEnter -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
                DataTable dataTable;

                if (!m_bomServer.GetBom(edition, out dataTable, out m_err))
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
        void RefreshTreeView(string edition)
        {
            string error;
            Dictionary<string, List<Bom>> DicBomTable;
            Dictionary<string, List<Bom>> tempDic;

            m_changeFlag = false;
            m_updatedBomInfo.Clear();

            if (m_bomServer.GetBom(edition, out tempDic, out m_err))
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

            if (m_bomServer.GetBom(edition, out DicBomTable, out error))
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
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
                return;
            }

            treeView1.ExpandAll();
        }

        /// <summary>
        /// 初始化树视图
        /// </summary>
        /// <param name="edition">版本号</param>
        void InitTreeView(string edition)
        {
            RefreshDataGridView(edition);

            this.treeView1.AfterSelect -= new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);

            RefreshTreeView(edition);

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
            cmbAssemblyFlag.SelectedItem = null;
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

            if (!bom.AssemblyFlag)
            {
                cmbAssemblyFlag.SelectedIndex = 0;

                this.cmbSetAssemblyFlag.SelectedIndexChanged -= new System.EventHandler(this.cmbSetAssemblyFlag_SelectedIndexChanged);
                cmbSetAssemblyFlag.SelectedIndex = 0;
                this.cmbSetAssemblyFlag.SelectedIndexChanged += new System.EventHandler(this.cmbSetAssemblyFlag_SelectedIndexChanged);
            }
            else if (bom.AssemblyFlag)
            {
                cmbAssemblyFlag.SelectedIndex = 1;

                this.cmbSetAssemblyFlag.SelectedIndexChanged -= new System.EventHandler(this.cmbSetAssemblyFlag_SelectedIndexChanged);
                cmbSetAssemblyFlag.SelectedIndex = 1;
                this.cmbSetAssemblyFlag.SelectedIndexChanged += new System.EventHandler(this.cmbSetAssemblyFlag_SelectedIndexChanged);
            }
            else
            {
                cmbAssemblyFlag.SelectedItem = null;
            }
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
            cmbProductType_SelectedIndexChanged(sender, e);
        }

        /// <summary>
        /// 改变产品类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbProductType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_changeFlag)
            {
                if (DialogResult.OK == MessageDialog.ShowEnquiryMessage("您对BOM进行了修改，是否保存您的修改内容？"))
                {
                    btnSave.PerformClick();
                }
            }

            if (cmbProductType.Items.Count > 0)
            {
                m_oldEdition = cmbProductType.SelectedItem.ToString();
                InitTreeView(cmbProductType.SelectedItem.ToString());
            }
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (CheckAddBom())
            {
                AddBom(cmbProductType.SelectedItem.ToString());
                btnSave.Enabled = true;
                btnCancle.Enabled = true;
            }
        }

        /// <summary>
        /// 添加检测
        /// </summary>
        /// <returns>返回是否允许添加零件</returns>
        bool CheckAddBom()
        {
            #region 检验数据项不能为空

            txtCode.Text = txtCode.Text.Trim();

            if (txtCode.Text == "")
            {
                MessageDialog.ShowPromptMessage("零部件编码不能为空!");
                return false;
            }

            if (txtVersion.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("零部件版次号不能为空!");
                return false;
            }

            txtName.Text = txtName.Text.Trim();

            if (txtName.Text == "")
            {
                MessageDialog.ShowPromptMessage("零部件名称不能为空!");
                return false;
            }

            if (numBasicCount.Value <= 0)
            {
                MessageDialog.ShowPromptMessage("基数不能为0!");
                return false;
            }

            if (cmbAssemblyFlag.SelectedItem == null || cmbAssemblyFlag.SelectedItem.ToString() == "")
            {
                MessageDialog.ShowPromptMessage("总成标志不能为空!");
                return false;
            }

            #endregion

            txtParentCode.Text = txtParentCode.Text.Trim();

            if (treeView1.Nodes.Count == 0)
            {
                if (txtParentCode.Text.Length > 0)
                {
                    txtParentCode.SelectAll();
                    txtParentCode.Focus();
                    MessageDialog.ShowPromptMessage("添加根节点时父总成只能为空!");
                    return false;
                }
                else if (cmbAssemblyFlag.SelectedIndex == 0)
                {
                    cmbAssemblyFlag.Focus();
                    MessageDialog.ShowPromptMessage("添加根节点时总成标志只能为父总成!");
                    return false;
                }
            }
            #region 检验当前树中是否存在父总成及零部件编码都相同的零部件

            if (m_dicParentAssemblyPartCode.ContainsKey(cmbProductType.SelectedItem.ToString()))
            {
                if (m_dicParentAssemblyPartCode[cmbProductType.SelectedItem.ToString()].Contains(txtParentCode.Text 
                    + txtCode.Text + txtSpec.Text))
                {
                    MessageDialog.ShowPromptMessage("不能添加父总成及零部件编码都相同的零部件!");
                    return false;
                }
            }

            #endregion

            //if ((txtParentCode.Text.Trim().Length > 0 && treeView1.SelectedNode.Parent == null) || (treeView1.SelectedNode.Parent != null && txtParentCode.Text.Trim().Length == 0)
            if (txtParentCode.Text.Trim().Length > 0 && treeView1.Nodes.Count > 0)
            {
                TreeNode[] parentNodes = treeView1.Nodes.Find(txtParentCode.Text, true);

                if (parentNodes.Count() == 0)
                {
                    MessageDialog.ShowPromptMessage(string.Format("没有找到您所指定的父总成图号为 [{0}] 节点，无法进行此操作！", 
                                                    txtParentCode.Text));
                    return false;
                }
            }

            List<Bom> listBom;

            if (DicBom != null && DicBom.ContainsKey(cmbProductType.SelectedItem.ToString()))
            {
                listBom = DicBom[cmbProductType.SelectedItem.ToString()];
            }
            else
            {
                listBom = new List<Bom>();

                if (DicBom == null)
                {
                    DicBom = new Dictionary<string, List<Bom>>();
                }

                DicBom.Add(cmbProductType.SelectedItem.ToString(), listBom);
            }

            #region 检验当前添加的项的父节点是否为非总成节点

            for (int i = 0; i < listBom.Count; i++)
            {
                if (listBom[i].PartCode == txtParentCode.Text)
                {
                    if (!listBom[i].AssemblyFlag)
                    {
                        MessageDialog.ShowPromptMessage("当前节点为非总成节点,不允许继续添加子节点!");
                        return false;
                    }

                    break;
                }
            }

            #endregion

            #region 检验是否在一节点下添加与自身相同编码的零部件

            if (txtParentCode.Text == txtCode.Text)
            {
                MessageDialog.ShowPromptMessage("不允许在一节点下添加与自身相同编码的零部件!");
                return false;
            }

            #endregion

            #region 检测添加的数据项为顶部节点的情况下是否将当前待添加的节点替换左树结构

            if (txtParentCode.Text == "" && treeView1.Nodes.Count != 0)
            {
                if (MessageBox.Show("树型结构根节点数不能大于1,是否将当前待添加的节点替换左树结构?", "警告", MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DicBom[cmbProductType.SelectedItem.ToString()].Clear();

                    if (m_dicParentAssemblyPartCode.ContainsKey(cmbProductType.SelectedItem.ToString()))
                    {
                        m_dicParentAssemblyPartCode[cmbProductType.SelectedItem.ToString()].Clear();
                    }

                    treeView1.Nodes.Clear();
                }
            }

            #endregion

            #region 检验是否在树结构中添加相同编码的总成

            for (int i = 0; i < listBom.Count; i++)
            {
                if (listBom[i].PartCode == txtCode.Text && cmbAssemblyFlag.SelectedIndex == 1)
                {
                    MessageDialog.ShowPromptMessage("在树型结构中不允许添加相同编码的总成!");
                    return false;
                }
            }

            #endregion

            return true;
        }

        /// <summary>
        /// 往更新列表中添加变更的BOM节点
        /// </summary>
        /// <param name="bom">BOM节点信息</param>
        /// <param name="status">更新的状态</param>
        void AddUpdatedBomInfo(Bom bom, Bom.Status status)
        {
            var result = from r in m_updatedBomInfo 
                         where r.ParentCode == bom.ParentCode && r.PartCode == bom.PartCode 
                         select r;

            if (result.Count() == 1)
            {
                Bom existBom = result.Single();

                if (existBom.StatusFlag == Bom.Status.Add)
                {
                    if (status == Bom.Status.Remove)
                    {
                        m_updatedBomInfo.Remove(existBom);
                    }

                }
                else
                {
                    // 原来为更新状态现在为删除状态
                    Bom cloneBom = existBom.Clone();
                    cloneBom.StatusFlag = status;
                    m_updatedBomInfo.Add(cloneBom);
                }
            }
            else
            {
                bom.StatusFlag = status;
                m_updatedBomInfo.Add(bom);
            }

            if (m_updatedBomInfo.Count > 0)
            {
                m_changeFlag = true;
            }
        }

        /// <summary>
        /// 添加Bom信息
        /// </summary>
        /// <param name="edition">版本号</param>
        /// <returns>返回是否成功添加Bom信息</returns>
        bool AddBom(string editions)
        {
            string parentCode = txtParentCode.Text.Trim();
            string partCode = txtCode.Text;
            string spec = txtSpec.Text;
            string partName = txtName.Text;
            int counts = Convert.ToInt32(numBasicCount.Value);
            bool assemblyFlag = cmbAssemblyFlag.SelectedIndex > 0;
            string remark = txtRemark.Text;
            string userCode = BasicInfo.LoginID;
            DateTime fillDate = ServerModule.ServerTime.Time;
            string version = txtVersion.Text.Trim();

            TreeNode node = new TreeNode();
            node.Name = partCode;
            node.Text = partName;
            node.ToolTipText = spec;

            bool Addflag = false;

            if (treeView1.Nodes.Count > 0)
            {
                if (treeView1.Nodes[0].Nodes.Count > 0)
                {
                    TreeNode[] parentNode = treeView1.Nodes.Find(parentCode, true);

                    if (parentCode != null)
                    {
                        parentNode[0].Nodes.Add(node);
                        Addflag = true;
                    }
                }
                else
                {
                    if (treeView1.Nodes[0].Name == txtParentCode.Text)
                    {
                        treeView1.Nodes[0].Nodes.Add(node);
                        Addflag = true;
                    }
                }
            }
            else
            {
                treeView1.Nodes.Add(node);
                Addflag = true;
            }

            if (Addflag)
            {
                Bom bom = new Bom(0, parentCode, partCode, partName, spec, 
                    counts, assemblyFlag, remark, userCode, fillDate,version);
                node.Tag = bom;

                DicBom[editions].Add(bom);
                AddUpdatedBomInfo(bom, Bom.Status.Add);

                if (m_dicParentAssemblyPartCode.ContainsKey(editions))
                {
                    m_dicParentAssemblyPartCode[editions].Add(parentCode + partCode + spec);
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("父总成编码错误,树型结构表中不存在该编码的节点!");
            }

            return true;
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
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                string edition = cmbProductType.SelectedItem.ToString();

                while (treeView1.SelectedNode.Nodes.Count > 0)
                {
                    FindChildNode(edition, treeView1.SelectedNode);
                }

                Bom bom = treeView1.SelectedNode.Tag as Bom;

                m_dicParentAssemblyPartCode[edition].Remove(bom.ParentCode + bom.PartCode + bom.Spec);

                Bom findBom = (from r in DicBom[cmbProductType.Text] 
                               where r.ParentCode == bom.ParentCode && r.PartCode == bom.PartCode && r.Spec == bom.Spec 
                               select r).Single();

                AddUpdatedBomInfo(bom, Bom.Status.Remove);

                DicBom[edition].Remove(findBom);

                treeView1.SelectedNode.Remove();
                btnSave.Enabled = true;
                btnCancle.Enabled = true;
            }
            else
            {
                MessageDialog.ShowPromptMessage("请在树型结构表中选择需要删除的节点!");
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

                            AddUpdatedBomInfo(DicBom[edition][j], Bom.Status.Remove);
                            DicBom[edition].RemoveAt(j);
                        }
                    }

                    node.Nodes[i].Remove();
                }
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null)
            {
                MessageDialog.ShowPromptMessage("请选择要修改的节点后再进行此操作！");
                return;
            }

            if (!CheckUpdateBom())
            {
                return;
            }

            Bom bom = treeView1.SelectedNode.Tag as Bom;

            string editions = cmbProductType.Text;

            if (m_dicParentAssemblyPartCode.ContainsKey(editions))
            {
                if (m_dicParentAssemblyPartCode[editions].Contains(bom.ParentCode + bom.PartCode + bom.Spec))
                {
                    m_dicParentAssemblyPartCode[editions].Remove(bom.ParentCode + bom.PartCode + bom.Spec);
                    m_dicParentAssemblyPartCode[editions].Add(txtParentCode.Text + txtCode.Text + txtSpec.Text);
                }
            }

            Bom findBom = (from r in DicBom[cmbProductType.Text] 
                           where r.ParentCode == bom.ParentCode && r.PartCode == bom.PartCode && r.Spec == bom.Spec 
                           select r).Single();

            bom.PartCode = txtCode.Text;
            bom.ParentCode = txtParentCode.Text;
            bom.PartName = txtName.Text;
            bom.Spec = txtSpec.Text;
            bom.Remark = txtRemark.Text;
            bom.AssemblyFlag = cmbAssemblyFlag.SelectedIndex > 0;
            bom.Counts = (int)numBasicCount.Value;
            bom.UserCode = BasicInfo.LoginID;
            bom.FillData = ServerModule.ServerTime.Time;
            bom.Version = txtVersion.Text.Trim();

            findBom.PartCode = txtCode.Text;
            findBom.ParentCode = txtParentCode.Text;
            findBom.PartName = txtName.Text;
            findBom.Spec = txtSpec.Text;
            findBom.Remark = txtRemark.Text;
            findBom.AssemblyFlag = cmbAssemblyFlag.SelectedIndex > 0;
            findBom.Counts = (int)numBasicCount.Value;
            findBom.UserCode = BasicInfo.LoginID;
            findBom.FillData = ServerModule.ServerTime.Time;
            findBom.Version = txtVersion.Text.Trim();

            AddUpdatedBomInfo(bom, Bom.Status.Update);

            treeView1.SelectedNode.Name = txtCode.Text;
            treeView1.SelectedNode.Text = txtName.Text;

            //MessageDialog.ShowPromptMessage("BOM表数据不允许进行修改，只支持增加、删除方式，如果确实需要修改请联系系统管理员！");
        }

        /// <summary>
        /// 修改检测
        /// </summary>
        /// <returns>返回是否允许修改零件</returns>
        bool CheckUpdateBom()
        {
            #region 检验数据项不能为空

            if (txtCode.Text == "")
            {
                MessageDialog.ShowPromptMessage("零部件编码不能为空!");
                return false;
            }

            if (txtVersion.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("零部件版次号不能为空!");
                return false;
            }

            if (txtName.Text == "")
            {
                MessageDialog.ShowPromptMessage("零部件名称不能为空!");
                return false;
            }

            //if (numBasicCount.Value <= 0)
            //{
            //    MessageDialog.ShowPromptMessage("基数不能为0!");
            //    return false;
            //}

            if (cmbAssemblyFlag.SelectedItem == null || cmbAssemblyFlag.SelectedItem.ToString() == "")
            {
                MessageDialog.ShowPromptMessage("总成标志不能为空!");
                return false;
            }

            #endregion

            TreeNode currentNode = treeView1.SelectedNode;
            Bom bom = treeView1.SelectedNode.Tag as Bom;

            if (cmbAssemblyFlag.SelectedIndex != Convert.ToInt32(bom.AssemblyFlag) && currentNode.Nodes.Count > 0)
            {
                MessageDialog.ShowPromptMessage("非叶子节点不允许修改总成标志，将此节点下的子节点全部删除后才允许进行此操作！");
                return false;
            }

            if ((bom.ParentCode == null && txtParentCode.Text.Trim().Length > 0) 
                || (bom.ParentCode != null && bom.ParentCode != txtParentCode.Text) )
            {
                MessageDialog.ShowPromptMessage("不允许修改父总成编号！");
                return false;
            }

            #region 检验是否在一节点下添加与自身相同编码的零部件

            if (txtParentCode.Text == txtCode.Text)
            {
                MessageDialog.ShowErrorMessage("不允许在一节点下存在与自身相同编码的零部件!");
                return false;
            }

            #endregion

            List<Bom> listBom = DicBom[cmbProductType.Text];
            TreeNode[] parentNodes = treeView1.Nodes.Find(txtParentCode.Text, true);

            #region 检验当前修改后的节点的父总成编码是否存在,是否会引起在非总成节点下存在节点的错误情况
            if (parentNodes != null && parentNodes.Count() > 0)
            {
                TreeNode[] findSameNameNode = parentNodes[0].Nodes.Find(txtCode.Text, false);

                if (findSameNameNode != null && findSameNameNode.Count() > 0 && findSameNameNode[0] != currentNode)
                {
                    MessageDialog.ShowErrorMessage(string.Format("{0} 节点下已经存在图号为 {1} 的子节点，不允许再重复添加！", 
                                                   txtParentCode.Text, txtCode.Text));
                    return false;
                }
            }
            #endregion

            return true;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (m_changeFlag)
            {
                SaveBom();
            }
            else
            {
                MessageDialog.ShowPromptMessage("BOM没有发生任何变化，不需要保存！");
            }
        }

        /// <summary>
        /// 存储Bom
        /// </summary>
        void SaveBom()
        {
            string edition = cmbProductType.SelectedItem.ToString();

            if (!m_bomServer.UpdateBom(edition, m_updatedBomInfo, out m_err))
            {
                MessageDialog.ShowPromptMessage(m_err);
                InitTreeView(edition);
            }
            else
            {
                InitTreeView(edition);
                MessageDialog.ShowPromptMessage("成功保存BOM信息！");
            }

            m_updatedBomInfo.Clear();
            m_changeFlag = false;
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancle_Click(object sender, EventArgs e)
        {
            if (m_changeFlag && MessageBox.Show("您是否保存Bom?", "提示", MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                btnSave.PerformClick();
            }
            else
            {
                InitTreeView(cmbProductType.SelectedItem.ToString());
            }
        }

        /// <summary>
        /// 修改选中节点总成标志，直接应用到数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbSetAssemblyFlag_SelectedIndexChanged(object sender, EventArgs e)
        {
            TreeNode currentNode = treeView1.SelectedNode;

            if (currentNode == null)
            {
                MessageDialog.ShowPromptMessage("请选择好要修改的节点后再进行此操作！");
                return;
            }
            else if (currentNode.Nodes.Count > 0)
            {
                MessageDialog.ShowPromptMessage("非叶子节点不允许修改总成标志，将此节点下的子节点全部删除后才允许进行此操作！");
                return;
            }

            Bom bom = currentNode.Tag as Bom;
            Bom updateBom = bom;

            string info = string.Format("您确定要修改图号为：[{0}]，零件名称为：[{1}] 的节点的总成标志？", bom.PartCode, bom.PartName);

            if (MessageDialog.ShowEnquiryMessage(info) == DialogResult.Yes)
            {
                // 如果更新的节点位于更新列表中则直接在更新列表中更新，暂不更新到数据库，否则直接更新到数据库
                var result = from r in m_updatedBomInfo 
                             where r.ParentCode == bom.ParentCode && r.PartCode == bom.PartCode 
                             select r;

                if (result.Count() > 0)
                {
                    bom = result.Single();
                    bom.AssemblyFlag = cmbSetAssemblyFlag.SelectedIndex > 0;
                    bom.UserCode = BasicInfo.LoginID;
                    bom.FillData = ServerModule.ServerTime.Time;

                    ChangePara(currentNode);
                    return;
                }

                bom.UserCode = BasicInfo.LoginID;
                bom.FillData = ServerModule.ServerTime.Time;

                bom.AssemblyFlag = cmbSetAssemblyFlag.SelectedIndex > 0;
                ChangePara(currentNode);
                //if (!m_bomServer.UpdateBom(cmbProductType.Text, bom, cmbSetAssemblyFlag.SelectedIndex == 1, out m_err))
                //{
                //    MessageDialog.ShowErrorMessage(m_err);
                //}
                //else
                //{
                //    bom.AssemblyFlag = cmbSetAssemblyFlag.SelectedIndex > 0;
                //    ChangePara(currentNode);
                //}
            }
        }

        private void UserControlBom_Enter(object sender, EventArgs e)
        {
            //MessageDialog.ShowPromptMessage("Test");
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

        private void 复制数据表数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择数据表中的一行后再进行此操作！");
                return;
            }

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
            {
                DataGridViewRow row = dataGridView1.SelectedRows[i];

                for (int j = 1; j < 6; j++)
                {
                    sb.Append(row.Cells[j].Value);
                    sb.Append("\t");
                }
            }

            sb.AppendLine();
            Clipboard.SetDataObject(sb.ToString()); 
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

    }
}
