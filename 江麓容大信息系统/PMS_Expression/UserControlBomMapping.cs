/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  UserControlBomMapping.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2010/09/06
 * 开发平台:  vs2005(c#)
 * 用于    :  生产线管理信息系统
 *----------------------------------------------------------------------------
 * 描述 : 用于零件在设计BOM与装配工序间时对应关系的映射
 * 其它 :
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2010/09/06 16:30:08 作者: 夏石友 当前版本: V1.00
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
    /// BomMapping组件
    /// </summary>
    public partial class UserControlBomMapping : Form
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
        /// 产品类型父总成零件编码字典, 用于检查是否存在“父总成编码+零件编码+规格”都一致的零件（防止添加、更新时出现问题）
        /// </summary>
        Dictionary<string, List<string>> m_dicParentAssemblyPartCode = new Dictionary<string, List<string>>();

        /// <summary>
        /// 服务组件
        /// </summary>
        IProductInfoServer m_productInfoServer = ServerModuleFactory.GetServerModule<IProductInfoServer>();

        /// <summary>
        /// BOM 服务组件
        /// </summary>
        IBomServer m_bomServer = ServerModuleFactory.GetServerModule<IBomServer>();

        /// <summary>
        /// BOM 映射服务组件
        /// </summary>
        IBomMappingServer m_bomMappingServer = ServerModuleFactory.GetServerModule<IBomMappingServer>();

        /// <summary>
        /// 服务组件
        /// </summary>
        IWorkbenchService m_workbenchServer = ServerModuleFactory.GetServerModule<IWorkbenchService>();

        /// <summary>
        /// 产品信息
        /// </summary>
        IQueryable<View_P_ProductInfo> m_productInfo;

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

        public UserControlBomMapping()
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
        }

        /// <summary>
        /// 改变组件大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControlBomMapping_Resize(object sender, EventArgs e)
        {
            //panelLeft.Width = (this.Width - panelCenter.Width) / 2;
            //panelRight.Width = this.Width - panelCenter.Width - panelLeft.Width;
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControlBomMapping_Load(object sender, EventArgs e)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip1, BasicInfo.GetFunctionTreeNodeInfo(labelTitle.Text).Authority);

            this.cmbProductType.SelectedIndexChanged -= new System.EventHandler(this.cmbProductType_SelectedIndexChanged);

            #region 获取所有产品编码(产品类型)信息

            if (!m_productInfoServer.GetAllProductInfo(out m_productInfo, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);

                btnAdd.Enabled = false;
                btnDelete.Enabled = false;
                return;
            }

            if (m_productInfo != null)
            {
                foreach (var item in m_productInfo)
                {
                    cmbProductType.Items.Add(item.产品类型编码);
                }
            }

            #endregion

            #region 获取工位

            IQueryable<View_P_Workbench> workbench = m_workbenchServer.Workbenchs;
            
            if (workbench.Count() > 0)
            {
                cmbWorkBench.Items.AddRange((from r in workbench select r.工位).ToArray());
            }
            else
            {
                btnAdd.Enabled = false;
                btnDelete.Enabled = false;

                MessageDialog.ShowErrorMessage("没有获取到工位信息");
                return;
            }

            #endregion

            //InitViewData(cmbProductType.SelectedItem.ToString());

            this.cmbProductType.SelectedIndexChanged += new System.EventHandler(this.cmbProductType_SelectedIndexChanged);

            if (cmbProductType.Items.Count > 0)
            {
                cmbProductType.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 刷新 DataGridView
        /// </summary>
        /// <param name="productName">产品名称</param>
        void RefreshDataGridView(string productName)
        {
            try
            {
                this.dataGridView1.CellEnter -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);

                ResetAssemblyPanelParams();

                dataGridView1.DataSource = m_bomMappingServer.GetBomMapping(productName);
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

                ChangeBomMappingPanelPara();
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
        /// 定位记录
        /// </summary>
        /// <param name="parentCode">定位用的父总成编码</param>
        /// <param name="partCode">定位用的零件编码</param>
        void PositioningRecord(string parentCode, string partCode)
        {
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
                if ((string)dataGridView1.Rows[i].Cells["父总成编码"].Value == parentCode &&
                    (string)dataGridView1.Rows[i].Cells["零件编码"].Value == partCode)
                {
                    if (i != dataGridView1.Rows.Count - 1)
                    {
                        dataGridView1.CurrentCell = dataGridView1.Rows[i + 1].Cells[strColName];
                    }

                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                }
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

            if (m_bomServer.GetBom(edition, out tempDic, out m_err))
            {
                DicBom = tempDic;
            }
            else
            {
                MessageDialog.ShowErrorMessage(m_err);
                treeView1.Nodes.Clear();
                ResetBomPanelPara();
                return;
            }

            treeView1.Nodes.Clear();

            if (m_bomServer.GetBom(edition, out DicBomTable, out error))
            {
                List<Bom> listBom = DicBomTable[edition];

                for (int i = 0; i < listBom.Count; i++)
                {
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

                ChangeBomPanelPara(treeView1.Nodes[0]);
            }
            else
            {
                MessageDialog.ShowErrorMessage(error);

                ResetBomPanelPara();
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
                return;
            }

            treeView1.ExpandAll();
        }

        /// <summary>
        /// 初始化视图数据(更新树及DataGridView)
        /// </summary>
        /// <param name="edition">版本号</param>
        void InitViewData(string edition)
        {
            RefreshDataGridView(txtProductName.Text);

            this.treeView1.AfterSelect -= new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            RefreshTreeView(edition);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);

            if (treeView1.Nodes.Count > 0)
            {
                treeView1.SelectedNode = treeView1.Nodes[0];
            }

            // 获取父总成名称
            string[] assemblyCodes;
            string[] assemblyNames;

            cmbAssemblyParentName.Items.Clear();

            if (m_bomServer.GetAssemblyInfo(edition, out assemblyCodes, out assemblyNames))
            {
                cmbAssemblyParentName.Items.AddRange(assemblyNames);
            }
        }

        /// <summary>
        /// 重置BOM面板参数
        /// </summary>
        void ResetBomPanelPara()
        {
            txtParentCode.Text = "";
            txtParentName.Text = "";
            txtCode.Text = "";
            txtName.Text = "";
            txtSpec.Text = "";
            numBasicCount.Value = 0;
        }

        /// <summary>
        /// 重置装配信息面板参数
        /// </summary>
        void ResetAssemblyPanelParams()
        {
            cmbAssemblyParentName.SelectedIndex = -1;
            cmbWorkBench.SelectedIndex = -1;
            cmbWorkBench.Text = "";
            numAssemblyCount.Value = 0;
            txtAssemblyRemark.Text = "";
            dateTimePicker1.Value = ServerModule.ServerTime.Time;
        }

        /// <summary>
        /// 递归生成Bom表的树型结构
        /// </summary>
        /// <param name="parentNode">父总成编码</param>
        /// <param name="dicBomTable">BomMapping字典</param>
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
        /// 更改BOM面板参数的显示
        /// </summary>
        /// <param name="node">用于更新显示的节点</param>
        void ChangeBomPanelPara(TreeNode node)
        {
            Bom bom = node.Tag as Bom;

            if (!GlobalObject.GeneralFunction.IsNullOrEmpty(bom.ParentCode))
            {
                Bom parentBom = node.Parent.Tag as Bom;

                txtParentName.Text = parentBom.PartName;
                txtParentCode.Text = parentBom.PartCode;
            }
            else
            {
                txtParentCode.Text = "";
                txtParentName.Text = "";
            }

            txtCode.Text = bom.PartCode;
            txtName.Text = bom.PartName;
            txtSpec.Text = bom.Spec;

            numBasicCount.Value = Convert.ToDecimal(bom.Counts);
        }

        /// <summary>
        /// 更改BOM 映射面板参数的显示
        /// </summary>
        /// <param name="node">用于更新显示的节点</param>
        void ChangeBomMappingPanelPara()
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                return;
            }

            DataGridViewRow row = dataGridView1.SelectedRows[0];

            cmbAssemblyParentName.Text = row.Cells["装配线总成名称"].Value.ToString();
            numAssemblyCount.Value = (int)row.Cells["装配数"].Value;

            if ((bool)row.Cells["是否清洗"].Value)
            {
                cmbCleanout.SelectedIndex = 1;
            }
            else
            {
                cmbCleanout.SelectedIndex = 0;
            }

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(row.Cells["工位"].Value.ToString()))
            {
                cmbWorkBench.SelectedIndex = -1;
            }
            else
            {
                cmbWorkBench.Text = row.Cells["工位"].Value.ToString();
            }

            if (row.Cells["备注"].Value != null)
            {
                txtAssemblyRemark.Text = row.Cells["备注"].Value.ToString();
            }
            else
            {
                txtAssemblyRemark.Text = "";
            }

            dateTimePicker1.Value = (DateTime)row.Cells["变更日期"].Value;
        }

        /// <summary>
        /// 更改选定treeView1内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ResetBomPanelPara();
            ChangeBomPanelPara(e.Node);
            ResetAssemblyPanelParams();

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
                    if (dataGridView1.Rows[i].Cells["父总成编码"].Value.ToString() == bom.ParentCode 
                        && dataGridView1.Rows[i].Cells["零件编码"].Value.ToString() == bom.PartCode)
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
            if (cmbProductType.Items.Count > 0)
            {
                txtProductName.Text = (from r in m_productInfo 
                                       where r.产品类型编码 == cmbProductType.Text 
                                       select r.产品类型名称).First();
                InitViewData(cmbProductType.SelectedItem.ToString());
            }
        }

        /// <summary>
        /// 添加BomMapping信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!CheckData())
            {
                return;
            }

            try
            {
                P_ProductBomMapping bomMapping = new P_ProductBomMapping();

                bomMapping.ProductName = txtProductName.Text;
                bomMapping.ParentCode = txtParentCode.Text;
                bomMapping.ParentName = txtParentName.Text;
                bomMapping.PartCode = txtCode.Text;
                bomMapping.PartName = txtName.Text;
                bomMapping.Spec = txtSpec.Text;
                bomMapping.PartCounts = Convert.ToInt32(numBasicCount.Value);
                bomMapping.FittingParentName = cmbAssemblyParentName.Text;
                bomMapping.FittingCounts = Convert.ToInt32(numAssemblyCount.Value);
                bomMapping.Date = ServerModule.ServerTime.Time;
                bomMapping.UserCode = BasicInfo.LoginID;
                bomMapping.Workbench = cmbWorkBench.Text;
                bomMapping.NeedToClean = cmbCleanout.SelectedIndex > 0;
                bomMapping.Remarks = txtAssemblyRemark.Text;

                m_bomMappingServer.AddBomMapping(bomMapping);

                RefreshDataGridView(txtProductName.Text);

                PositioningRecord(bomMapping.ParentCode, bomMapping.PartCode);
            }
            catch (Exception err)
            {
                MessageDialog.ShowErrorMessage(err.Message);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows == null)
            {
                MessageDialog.ShowPromptMessage("请选择需要删除的记录后再进行此操作!");
                return;
            }

            try
            {
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    string info = string.Format("您真的要删除父总成名称为[{0}]，零件编码为[{1}]的映射记录吗？", 
                                  row.Cells["父总成名称"].Value, row.Cells["零件编码"].Value);

                    if (MessageDialog.ShowEnquiryMessage(info) == DialogResult.No)
                    {
                        return;
                    }


                    m_bomMappingServer.DeleteBomMapping(Convert.ToInt32(row.Cells["序号"].Value));
                }

                RefreshDataGridView(txtProductName.Text);
            }
            catch (Exception err)
            {
                MessageDialog.ShowErrorMessage(err.Message);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows == null)
            {
                MessageDialog.ShowPromptMessage("请选择需要删除的记录后再进行此操作!");
                return;
            }

            if (!CheckData())
            {
                return;
            }

            try
            {
                P_ProductBomMapping bomMapping = new P_ProductBomMapping();

                bomMapping.ProductName = txtProductName.Text;
                bomMapping.ParentCode = txtParentCode.Text;
                bomMapping.ParentName = txtParentName.Text;
                bomMapping.PartCode = txtCode.Text;
                bomMapping.PartName = txtName.Text;
                bomMapping.Spec = txtSpec.Text;
                bomMapping.PartCounts = Convert.ToInt32(numBasicCount.Value);
                bomMapping.FittingParentName = cmbAssemblyParentName.Text;
                bomMapping.FittingCounts = Convert.ToInt32(numAssemblyCount.Value);
                bomMapping.Date = ServerModule.ServerTime.Time;
                bomMapping.UserCode = BasicInfo.LoginID;
                bomMapping.Workbench = cmbWorkBench.Text;
                bomMapping.NeedToClean = cmbCleanout.SelectedIndex > 0;
                bomMapping.Remarks = txtAssemblyRemark.Text;

                m_bomMappingServer.UpdateBomMapping(Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["序号"].Value), bomMapping);
                RefreshDataGridView(txtProductName.Text);
                PositioningRecord(bomMapping.ParentCode, bomMapping.PartCode);
            }
            catch (Exception err)
            {
                MessageDialog.ShowErrorMessage(err.Message);
            }
        }

        /// <summary>
        /// 检查数据是否符合要求
        /// </summary>
        /// <returns>返回是否允许修改映射数据</returns>
        bool CheckData()
        {
            #region 检验数据项不能为空

            txtParentCode.Text = txtParentCode.Text.Trim();
            txtParentName.Text = txtParentName.Text.Trim();
            txtCode.Text = txtCode.Text.Trim();
            txtAssemblyRemark.Text = txtAssemblyRemark.Text.Trim();

            if (txtParentCode.Text == "")
            {
                MessageDialog.ShowPromptMessage("不允许对父总成编码为空的零部件进行映射!");
                return false;
            }

            if (txtCode.Text == "")
            {
                MessageDialog.ShowPromptMessage("零件编码不能为空!");
                return false;
            }

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

            if (cmbAssemblyParentName.SelectedIndex < 0)
            {
                cmbAssemblyParentName.Focus();
                MessageDialog.ShowPromptMessage("装配父总成名称不能为空!");
                return false;
            }

            if (cmbWorkBench.SelectedIndex < 0)
            {
                cmbWorkBench.Focus();
                MessageDialog.ShowPromptMessage("请选择工位!");
                return false;
            }

            if (numAssemblyCount.Value == 0)
            {
                numAssemblyCount.Focus();
                MessageDialog.ShowPromptMessage("装配数量必须 > 0");
                return false;
            }

            if (numAssemblyCount.Value > numBasicCount.Value)
            {
                numAssemblyCount.Focus();
                MessageDialog.ShowPromptMessage("装配数量必须 <= 零件基数");
                return false;
            }

            if (cmbCleanout.SelectedIndex < 0)
            {
                cmbCleanout.Focus();
                MessageDialog.ShowPromptMessage("请选择是否清洗");
                return false;
            }

            #endregion

            return true;
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
            TreeNode[] findNodes = treeView1.Nodes.Find(row.Cells["零件编码"].Value.ToString(), true);

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

            ChangeBomMappingPanelPara();
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
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

                for (int col = 0; col < dataGridView1.Columns.Count; col++)
                {
                    sb.Append(row.Cells[col].Value);
                    sb.Append("\t");
                }

                sb.AppendLine();
            }

            Clipboard.SetDataObject(sb.ToString());
        }

        /// <summary>
        /// 检测设计与装配基数是否吻合
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCheckBasicCount_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null)
            {
                MessageDialog.ShowPromptMessage("请选择要检测的树节点后再进行此操作!");
                return;
            }

            try
            {
                Bom bom = treeView1.SelectedNode.Tag as Bom;
                int assemblyAmount = 0;

                if (!m_bomMappingServer.IsCoincideBasicCount(bom.Counts, txtProductName.Text, bom.ParentCode, bom.PartCode, out assemblyAmount))
                {
                    if (assemblyAmount == 0)
                        MessageDialog.ShowPromptMessage("此零件没有映射信息，无法匹配！");
                    else
                        MessageDialog.ShowErrorMessage(string.Format("此零件设计基数为：{0}，映射表中此零件装配数之和为：{1}，不相匹配，请修改正确否则电子档案数据将不正确！", bom.Counts, assemblyAmount));
                }
                else
                {
                    MessageDialog.ShowPromptMessage("匹配成功，完全正确！");
                }
            }
            catch (Exception err)
            {
                MessageDialog.ShowErrorMessage(err.Message);
            }
        }
    }
}
