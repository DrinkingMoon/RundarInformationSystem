using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using ServerModule;
using GlobalObject;
using UniversalControlLibrary;

namespace Expression
{
    public partial class 装配BOM : Form
    {
        #region 成员变量

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 服务组件
        /// </summary>
        IProductInfoServer m_productInfoServer = ServerModuleFactory.GetServerModule<IProductInfoServer>();

        /// <summary>
        /// BOM 服务组件
        /// </summary>
        IBomServer m_bomServer = ServerModuleFactory.GetServerModule<IBomServer>();

        /// <summary>
        /// 装配BOM 服务组件
        /// </summary>
        IAssemblingBom m_assemblingBom = ServerModuleFactory.GetServerModule<IAssemblingBom>();

        /// <summary>
        /// 服务组件
        /// </summary>
        IWorkbenchService m_workbenchServer = ServerModuleFactory.GetServerModule<IWorkbenchService>();

        /// <summary>
        /// 装配BOM信息
        /// </summary>
        List<View_P_AssemblingBom> m_lstAssemblingBom;

        /// <summary>
        /// 装配BOM备份信息
        /// </summary>
        List<View_P_AssemblingBom> m_lstAssemblingBomBackup;

        /// <summary>
        /// 产品信息
        /// </summary>
        IQueryable<View_P_ProductInfo> m_productInfo;

        /// <summary>
        /// 数据定位控件
        /// </summary>
        //UserControlDataLocalizer m_dataLocalizer;

        /// <summary>
        /// 上一次的选中节点
        /// </summary>
        TreeNode m_preSelectedNode;

        /// <summary>
        /// 通过数据显示控件行来选择树节点的标志
        /// </summary>
        bool m_selecteTreeNodeFromDataGridViewRow;

        /// <summary>
        /// 产品BOM信息
        /// </summary>
        List<View_P_ProductBom> m_productBomInfo;

        /// <summary>
        /// 授权标志
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 选择的行索引
        /// </summary>
        int m_selectionIdx = -1;

        #endregion

        public 装配BOM(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authFlag = nodeInfo.Authority;

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();

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

            this.cmbProductType.SelectedIndexChanged += new System.EventHandler(this.cmbProductType_SelectedIndexChanged);

            if (cmbProductType.Items.Count > 0)
            {
                cmbProductType.SelectedIndex = 0;
            }
        }

        private void 装配BOM_Load(object sender, EventArgs e)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip1, m_authFlag);
        }

        /// <summary>
        /// 改变组件大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControlBomMapping_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        private void RefreshData()
        {
            if (cmbProductType.Items.Count > 0)
            {
                ResetBomPanelPara();

                m_productBomInfo = m_bomServer.GetBom(cmbProductType.Text).ToList();

                txtProductName.Text = (from r in m_productInfo 
                                       where r.产品类型编码 == cmbProductType.Text 
                                       select r.产品类型名称).First();

                m_lstAssemblingBom = m_assemblingBom.GetAssemblingBom(cmbProductType.Text);

                View_P_AssemblingBom[] buffer = new View_P_AssemblingBom[m_lstAssemblingBom.Count];

                m_lstAssemblingBom.CopyTo(buffer);

                m_lstAssemblingBomBackup = buffer.ToList();

                InitViewData(m_lstAssemblingBom);
            }
        }

        private void cmbProductType_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        /// <summary>
        /// 初始化视图数据
        /// </summary>
        /// <param name="lstAssemblingBom">获取到的装配BOM信息列表</param>
        void InitViewData(List<View_P_AssemblingBom> lstAssemblingBom)
        {
            if (m_lstAssemblingBom == null || m_lstAssemblingBom.Count == 0)
            {
                treeView1.Nodes.Clear();
                dataGridView1.DataSource = null;
                return;
            }

            dataGridView1.DataSource = GlobalObject.GeneralFunction.ConvertToDataTable(m_lstAssemblingBom);

            dataGridView1.Columns["序号"].Visible = false;

            for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
            {
                this.dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            this.treeView1.AfterSelect -= new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);

            RefreshTreeView(lstAssemblingBom);

            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);

            if (treeView1.Nodes.Count > 0)
            {
                treeView1.SelectedNode = treeView1.Nodes[0];
            }

            userControlDataLocalizer1.Init(dataGridView1, this.Name, UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
        }

        /// <summary>
        /// 生成树节点
        /// </summary>
        /// <param name="info">装配BOM信息</param>
        /// <returns>返回生成的树节点</returns>
        TreeNode CreateTreeNode(View_P_AssemblingBom info)
        {
            TreeNode node = new TreeNode();

            node.Tag = info;
            node.Text = info.零件名称;
            node.Name = info.零件编码 + info.规格;

            Console.WriteLine("{0} \t {1}", node.Text, node.Name);

            return node;
        }

        /// <summary>
        /// 刷新树视图
        /// </summary>
        /// <param name="lstAssemblingBom">获取到的装配BOM信息列表</param>
        void RefreshTreeView(List<View_P_AssemblingBom> lstAssemblingBom)
        {
            treeView1.Nodes.Clear();

            View_P_AssemblingBom productPart = lstAssemblingBom.Find(p => p.零件编码 == cmbProductType.Text);

            if (productPart == null)
            {
                MessageDialog.ShowErrorMessage("获取不到【" + cmbProductType.Text + "】根节点信息，无法继续");
                return;
            }

            TreeNode node = CreateTreeNode(productPart);

            treeView1.Nodes.Add(node);

            lstAssemblingBom.Remove(productPart);
            RecursionBuildTreeView(node, lstAssemblingBom);

            //ChangeBomPanelPara(treeView1.Nodes[0]);

            node.Expand();
        }

        /// <summary>
        /// 递归生成Bom表的树型结构
        /// </summary>
        /// <param name="parentNode">父节点</param>
        /// <param name="lstAssemblingBom">装配BOM信息</param>
        void RecursionBuildTreeView(TreeNode parentNode, List<View_P_AssemblingBom> lstAssemblingBom)
        {
            if (parentNode == null || lstAssemblingBom.Count == 0)
            {
                return;
            }

            View_P_AssemblingBom parentInfo = parentNode.Tag as View_P_AssemblingBom;
            List<View_P_AssemblingBom> assemblyPart = lstAssemblingBom.FindAll(p => p.是否总成 && p.父总成编码 == parentInfo.零件编码);

            //if (assemblyPart.Count == 0)
            //{
            //    List<View_P_AssemblingBom> leafagePart = lstAssemblingBom.FindAll(p => !p.是否总成 && p.父总成编码 == parentInfo.零件编码);
            //    foreach (var item in leafagePart)
            //    {
            //        TreeNode node = CreateTreeNode(item);
            //        parentNode.Nodes.Add(node);
            //    }

            //    lstAssemblingBom.RemoveAll(p => !p.是否总成 && p.父总成编码 == parentInfo.零件编码);
            //}

            for (int i = 0; i < assemblyPart.Count; i++)
            {
                TreeNode node = CreateTreeNode(assemblyPart[0]);

                parentNode.Nodes.Add(node);
                lstAssemblingBom.Remove(assemblyPart[0]);
                assemblyPart.RemoveAt(0);
                i--;

                RecursionBuildTreeView(node, lstAssemblingBom);
            }

            List<View_P_AssemblingBom> leafPart = lstAssemblingBom.FindAll(p => !p.是否总成 
                                                  && p.父总成编码 == parentInfo.零件编码);

            foreach (var item in leafPart)
            {
                TreeNode leafNode = CreateTreeNode(item);
                parentNode.Nodes.Add(leafNode);
            }

            lstAssemblingBom.RemoveAll(p => !p.是否总成 && p.父总成编码 == parentInfo.零件编码);
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ResetBomPanelPara();
            ChangePanelPara(e.Node);

            if (m_preSelectedNode != null)
            {
                m_preSelectedNode.BackColor = treeView1.BackColor;
            }

            m_preSelectedNode = e.Node;
            e.Node.BackColor = Color.Yellow;

            if (!m_selecteTreeNodeFromDataGridViewRow && dataGridView1.Rows.Count > 0)
            {
                View_P_AssemblingBom bom = treeView1.SelectedNode.Tag as View_P_AssemblingBom;

                if (bom.父总成编码 == null)
                {
                    bom.父总成编码 = "";
                }

                int visibleColumn = StapleInfo.GetVisibleColumn(dataGridView1);

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Cells["父总成编码"].Value.ToString() 
                        == bom.父总成编码 && dataGridView1.Rows[i].Cells["零件编码"].Value.ToString() == bom.零件编码)
                    {
                        dataGridView1.ClearSelection();
                        dataGridView1.Rows[i].Selected = true;
                        dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[visibleColumn];

                        break;
                    }
                }
            }

            m_selecteTreeNodeFromDataGridViewRow = false;
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
            numAssemblyCount.Value = 0;
            chk缺料.Checked = false;
            chkCleanout.Checked = false;
            chkIsAdapting.Checked = false;
            chkIsAssemblyPart.Checked = false;
            cmbWorkBench.SelectedIndex = -1;
            txtRemark.Text = "";
            dateTimePicker1.Value = ServerTime.Time;
        }

        /// <summary>
        /// 更改BOM面板参数的显示
        /// </summary>
        /// <param name="node">用于更新显示的节点</param>
        void ChangePanelPara(TreeNode node)
        {
            View_P_AssemblingBom bom = node.Tag as View_P_AssemblingBom;

            if (!GlobalObject.GeneralFunction.IsNullOrEmpty(bom.父总成编码))
            {
                txtParentName.Text = bom.父总成名称;
                txtParentCode.Text = bom.父总成编码;
            }

            txtCode.Text = bom.零件编码;
            txtName.Text = bom.零件名称;
            txtSpec.Text = bom.规格;

            numAssemblyCount.Value = Convert.ToDecimal(bom.装配数量);
            chk缺料.Checked = (bool)bom.是否缺料;
            chkCleanout.Checked = bom.是否清洗;
            chkIsAdapting.Checked = bom.是否选配零件;
            chkIsAssemblyPart.Checked = bom.是否总成;

            if (!GlobalObject.GeneralFunction.IsNullOrEmpty(bom.工位))
            {
                cmbWorkBench.Text = bom.工位;
            }

            if (!GlobalObject.GeneralFunction.IsNullOrEmpty(bom.备注))
            {
                txtRemark.Text = bom.备注;
            }

            dateTimePicker1.Value = bom.录入日期;
        }

        /// <summary>
        /// 更改BOM面板参数的显示
        /// </summary>
        void ChangePanelPara()
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                return;
            }

            ResetBomPanelPara();

            DataGridViewRow row = dataGridView1.SelectedRows[0];

            if (row.Cells["父总成编码"].Value != System.DBNull.Value)
            {
                txtParentCode.Text = row.Cells["父总成编码"].Value.ToString();
            }

            if (row.Cells["父总成名称"].Value != System.DBNull.Value)
            {
                txtParentName.Text = row.Cells["父总成名称"].Value.ToString();
            }

            txtCode.Text = row.Cells["零件编码"].Value.ToString();
            txtName.Text = row.Cells["零件名称"].Value.ToString();
            txtSpec.Text = row.Cells["规格"].Value.ToString();
            numAssemblyCount.Value = (int)row.Cells["装配数量"].Value;

            try
            {
                numBasicCount.Value = (int)row.Cells["基数"].Value;
            }
            catch (Exception)
            {
                numBasicCount.Value = 1;
            }

            chk缺料.Checked = (bool)row.Cells["是否缺料"].Value;
            chkCleanout.Checked = (bool)row.Cells["是否清洗"].Value;
            chkIsAdapting.Checked = (bool)row.Cells["是否选配零件"].Value;
            chkIsAssemblyPart.Checked = (bool)row.Cells["是否总成"].Value;
            chkRasterProofing.Checked = (bool)row.Cells["需光栅防错"].Value;

            if (row.Cells["工位"].Value != System.DBNull.Value)
            {
                cmbWorkBench.Text = row.Cells["工位"].Value.ToString();
            }

            if (row.Cells["装配顺序"].Value != System.DBNull.Value)
            {
                numOrderNo.Value = (int)row.Cells["装配顺序"].Value;
            }

            if (row.Cells["备注"].Value != System.DBNull.Value)
            {
                txtRemark.Text = row.Cells["备注"].Value.ToString();
            }

            dateTimePicker1.Value = (DateTime)row.Cells["录入日期"].Value;
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || treeView1.Nodes.Count == 0)
            {
                return;
            }

            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
            string name = row.Cells["零件编码"].Value.ToString() + row.Cells["规格"].Value.ToString();
            TreeNode[] findNodes = treeView1.Nodes.Find(name, true);

            foreach (TreeNode node in findNodes)
            {
                View_P_AssemblingBom bom = node.Tag as View_P_AssemblingBom;

                if (bom.父总成编码 == row.Cells["父总成编码"].Value.ToString())
                {
                    m_selecteTreeNodeFromDataGridViewRow = true;
                    treeView1.SelectedNode = node;
                    break;
                }
            }

            ChangePanelPara();
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

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        private void btnFindCode_Click(object sender, EventArgs e)
        {
            if (sender == btnFindCode)
            {
                string productType = cmbProductType.Text;

                if (productType.Contains(" FX"))
                {
                    productType = productType.Replace(" FX", "");
                }

                FormQueryInfo form = QueryInfoDialog.GetAccessoryInfoDialog(productType, true, false);

                if (form == null || form.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                txtCode.Text = form.GetDataItem("图号型号").ToString();
                txtName.Text = form.GetDataItem("物品名称").ToString();
                txtSpec.Text = form.GetDataItem("规格").ToString();
            }
            else
            {
                FormQueryInfo form = QueryInfoDialog.GetPlanCostGoodsDialog();//QueryInfoDialog.GetAccessoryInfoDialog();

                if (form == null || form.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                txtCode.Text = form.GetDataItem("图号型号").ToString();
                txtName.Text = form.GetDataItem("物品名称").ToString();
                txtSpec.Text = form.GetDataItem("规格").ToString();
            }
        }

        private void btnFindParentPart_Click(object sender, EventArgs e)
        {
            string productType = cmbProductType.Text;

            if (productType.Contains(" FX"))
            {
                productType = productType.Replace(" FX", "");
            }

            FormQueryInfo form = QueryInfoDialog.GetAccessoryInfoDialog(productType, false, true);

            if (form == null || form.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            txtParentCode.Text = form.GetDataItem("图号型号").ToString();
            txtParentName.Text = form.GetDataItem("物品名称").ToString();

            if (cmbProductType.Text.Contains(txtParentCode.Text))
            {
                txtParentCode.Text = cmbProductType.Text;
                txtParentName.Text = txtProductName.Text;
            }
        }

        private void btnNewInfo_Click(object sender, EventArgs e)
        {
            ResetBomPanelPara();
        }

        /// <summary>
        /// 根据界面信息创建装配BOM数据对象
        /// </summary>
        /// <returns>返回创建的对象</returns>
        private P_AssemblingBom CreateAssemblingBom()
        {
            P_AssemblingBom info = new P_AssemblingBom();

            info.ProductCode = cmbProductType.Text;
            info.ParentCode = txtParentCode.Text;
            info.ParentName = txtParentName.Text;
            info.PartCode = txtCode.Text;
            info.PartName = txtName.Text;
            info.Spec = txtSpec.Text;
            info.AssemblyFlag = chkIsAssemblyPart.Checked;
            info.IsMaterialShortage = chk缺料.Checked;
            info.NeedToClean = chkCleanout.Checked;
            info.IsAdaptingPart = chkIsAdapting.Checked;
            info.RasterProofing = chkRasterProofing.Checked;
            info.FittingCounts = Convert.ToInt32(numAssemblyCount.Value);
            info.Remarks = txtRemark.Text;
            info.Workbench = cmbWorkBench.Text;
            info.UserCode = BasicInfo.LoginID;
            info.OrderNo = Convert.ToInt32(numOrderNo.Value);

            return info;
        }

        /// <summary>
        /// 检查是否正确选择操作的记录行
        /// </summary>
        /// <returns>正确返回true</returns>
        bool CheckSelectedRow()
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择一条记录后再进行此操作");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 检查数据项是否正确
        /// </summary>
        /// <returns>正确返回true</returns>
        private bool CheckDataItem()
        {
            if (GlobalObject.GeneralFunction.IsNullOrEmpty(cmbProductType.Text))
            {
                cmbProductType.Focus();
                MessageDialog.ShowPromptMessage("请选择产品");
                return false;
            }

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(txtParentCode.Text))
            {
                MessageDialog.ShowPromptMessage("请选择父总成");
                btnFindParentPart.PerformClick();
                return false;
            }

            #region 检查录入的父总成是否是装配BOM中的零件

            m_lstAssemblingBom = m_assemblingBom.GetAssemblingBom(cmbProductType.Text);

            View_P_AssemblingBom info = m_lstAssemblingBom.Find(p => p.是否总成 && p.零件编码 == txtParentCode.Text);

            //if (info == null)
            //{
            //    MessageDialog.ShowErrorMessage(@"您录入的父总成编码不是当前装配BOM中的总成/分总成零件");
            //    return false;
            //}

            if (info != null && txtParentName.Text != info.零件名称)
            {
                MessageDialog.ShowErrorMessage("您录入的父总成名称与父总成编码不匹配");
                return false;
            }

            #endregion
            
            if (GlobalObject.GeneralFunction.IsNullOrEmpty(txtCode.Text) && GlobalObject.GeneralFunction.IsNullOrEmpty(txtName.Text))
            {
                MessageDialog.ShowPromptMessage("请选择零件");
                btnFindCode.PerformClick();
                return false;
            }

            if (txtCode.Text == txtParentCode.Text)
            {
                MessageDialog.ShowPromptMessage("父总成与子零件不能是同一个零件");
                txtCode.Focus();
                return false;
            }

            if (cmbWorkBench.SelectedIndex == -1)
            {
                MessageDialog.ShowPromptMessage("请选择工位");
                cmbWorkBench.Focus();
                return false;
            }

            if (numAssemblyCount.Value == 0)
            {
                MessageDialog.ShowPromptMessage("装配数据必须 > 0");
                numAssemblyCount.Focus();
                return false;
            }

            if (txtCode.Text.Contains("VIR_") && !chkIsAssemblyPart.Checked)
            {
                MessageDialog.ShowPromptMessage("对于虚拟总成零件必须选择为总成");
                chkIsAssemblyPart.Focus();
                return false;
            }

            #region 2012.3.6 考虑后处理线返修专用软件需要设置装配顺序而增加

            //if (cmbProductType.Text.Contains(" FX") &&
            //    m_lstAssemblingBom.FindIndex(p => p.父总成编码 == txtParentCode.Text && (p.零件编码 != txtCode.Text || p.零件名称 != txtName.Text ||
            //        p.规格 != txtSpec.Text) && p.装配顺序 == Convert.ToInt32(numOrderNo.Value)) != -1)
            //{
            //    MessageDialog.ShowPromptMessage("此装配顺序已经存在");
            //    numOrderNo.Focus();
            //    return false;
            //}
            
            #endregion

            return true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!CheckDataItem())
            {
                return;
            }

            if (dataGridView1.Rows.Count > 0)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewCellCollection cells = dataGridView1.Rows[i].Cells;

                    if (cells["父总成编码"].Value.ToString() == txtParentCode.Text && cells["零件编码"].Value.ToString() == txtCode.Text
                        && cells["规格"].Value.ToString() == txtSpec.Text && cells["工位"].Value.ToString() == cmbWorkBench.Text)
                    {
                        MessageDialog.ShowPromptMessage("不允许进行此操作，当前装配BOM信息中已经存在父总成编码、零件编码、规格、工位完全相同的信息！");
                        return;
                    }
                }
            }

            P_AssemblingBom info = CreateAssemblingBom();

            if (!m_assemblingBom.Add(info, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            m_lstAssemblingBom = m_assemblingBom.GetAssemblingBom(cmbProductType.Text);
            InitViewData(m_lstAssemblingBom);
            PositioningRecord(info.ParentCode);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
            {
                return;
            }

            string prompt = string.Format("您确定要删除【{0}】下的【{1}】零件吗？", 
                dataGridView1.SelectedRows[0].Cells["父总成名称"].Value.ToString(),
                dataGridView1.SelectedRows[0].Cells["零件名称"].Value.ToString());

            if (MessageDialog.ShowEnquiryMessage(prompt) == DialogResult.No)
                return;

            int id = (int)dataGridView1.SelectedRows[0].Cells["序号"].Value;
            string parentCode = dataGridView1.SelectedRows[0].Cells["父总成编码"].Value.ToString();

            if (!m_assemblingBom.Delete(id, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            m_lstAssemblingBom = m_assemblingBom.GetAssemblingBom(cmbProductType.Text);

            InitViewData(m_lstAssemblingBom);

            PositioningRecord(parentCode);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow() || !CheckDataItem())
            {
                return;
            }

            string prompt = string.Format("您确定要更新【{0}】下的【{1}】零件吗？",
                dataGridView1.SelectedRows[0].Cells["父总成名称"].Value.ToString(),
                dataGridView1.SelectedRows[0].Cells["零件名称"].Value.ToString());

            if (MessageDialog.ShowEnquiryMessage(prompt) == DialogResult.No)
                return;

            int id = (int)dataGridView1.SelectedRows[0].Cells["序号"].Value;
            P_AssemblingBom info = CreateAssemblingBom();

            if (!m_assemblingBom.Update(id, info, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            m_lstAssemblingBom = m_assemblingBom.GetAssemblingBom(cmbProductType.Text);

            InitViewData(m_lstAssemblingBom);

            PositioningRecord(info.ParentCode);
        }

        private void btn生成虚拟总成零件信息_Click(object sender, EventArgs e)
        {
            FormVirtualPartInfo form = new FormVirtualPartInfo();

            if (form.ShowDialog() == DialogResult.OK)
            {
                this.txtCode.Text = form.PartCode;
                this.txtName.Text = form.PartName;
                this.chkIsAssemblyPart.Checked = true;
            }
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
                View_P_AssemblingBom bom = treeView1.SelectedNode.Tag as View_P_AssemblingBom;
                int assemblyAmount = 0;

                foreach (var item in m_lstAssemblingBom)
                {
                    if (item.零件编码 == bom.零件编码 && item.规格 == bom.规格)
                    {
                        assemblyAmount += item.装配数量;
                    }
                }

                if (assemblyAmount != bom.基数)
                {
                    if (assemblyAmount == 0)
                        MessageDialog.ShowPromptMessage("此零件没有映射信息，无法匹配！");
                    else
                        MessageDialog.ShowErrorMessage(
                            string.Format("此零件设计基数为：{0}，装配BOM中此零件装配数之和为：{1}，不相匹配，请修改正确否则电子档案数据将不正确！", bom.基数, assemblyAmount));
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

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="parentCode">定位到的父总成编码</param>
        void PositioningRecord(string parentCode)
        {
            if (GlobalObject.GeneralFunction.IsNullOrEmpty(parentCode))
            {
                return;
            }

            TreeNode[] nodes = treeView1.Nodes.Find(parentCode, true);

            if (nodes != null && nodes.Length > 0)
                treeView1.SelectedNode = nodes[0];
        }

        /// <summary>
        /// 将在产品BOM中查不到的零件以红色标出来
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (chkTestBom.Checked)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (m_productBomInfo.FindIndex(p =>
                        p.零部件编码 == dataGridView1.Rows[i].Cells["零件编码"].Value.ToString() &&
                        p.零部件名称 == dataGridView1.Rows[i].Cells["零件名称"].Value.ToString()) == -1)
                    {
                        dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.Red;
                    }
                }
            }
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        /// <summary>
        /// 复制指定产品类型装配BOM
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCopy_Click(object sender, EventArgs e)
        {
            FormCopyProductInfo form = new FormCopyProductInfo(FormCopyProductInfo.CopyModeEnum.复制整个产品零件信息);

            if (form.ShowDialog() == DialogResult.OK)
            {
                if (!m_assemblingBom.CopyBomData(form.SourceProductInfo.产品类型编码, form.TargetProductInfo.产品类型编码, out m_err))
                {
                    MessageDialog.ShowErrorMessage(m_err);
                }
                else
                {
                    MessageDialog.ShowPromptMessage("复制成功");
                }
            }
        }

        /// <summary>
        /// 检测零件是否存在于设计BOM
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkTestBom_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTestBom.Checked)
            {
                RefreshData();
            }
        }

        /// <summary>
        /// 复制指定分总成所有下属零件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 复制分总成_Click(object sender, EventArgs e)
        {
            FormCopyProductInfo form = new FormCopyProductInfo(FormCopyProductInfo.CopyModeEnum.复制分总成下属零件信息);

            if (form.ShowDialog() == DialogResult.OK)
            {
                bool flag = false;

                foreach (View_P_ProductInfo item in form.m_queryProductInfo)
                {
                    if (!m_assemblingBom.CopyBomData(form.SourceProductInfo.产品类型编码,
                       item.产品类型编码, form.ParentName, out m_err))
                    {
                        flag = true;
                        m_err += " "+m_err;
                        continue;
                    }
                }

                if (flag)
                {
                    MessageDialog.ShowPromptMessage(m_err);
                }
                else
                {
                    MessageDialog.ShowPromptMessage("复制成功");
                }
            }
        }

        private void 设置分总成下所有子零件工位号ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateWorkBench(false);
        }

        /// <summary>
        /// 更新指定分总成下所有零件工位号
        /// </summary>
        /// <param name="updateParentPart">是否一并更新分总成零件</param>
        private void UpdateWorkBench(bool updateParentPart)
        {
            if (treeView1.SelectedNode == null)
            {
                MessageDialog.ShowPromptMessage("请选择分总成树节点后再进行此操作");
                return;
            }

            if (MessageDialog.ShowEnquiryMessage("您确定要进行此操作吗？") == DialogResult.No)
                return;

            string parentName = (treeView1.SelectedNode.Tag as View_P_AssemblingBom).零件名称;

            View_P_AssemblingBom result = m_lstAssemblingBomBackup.Find(p => p.是否总成 == true && p.零件名称 == parentName);

            if (result == null)
            {
                MessageDialog.ShowPromptMessage(parentName + " 不是总成");
            }
            else
            {
                string workBench = InputBox.ShowDialog("输入工位号", "工位号", "").Trim().ToUpper();

                if (workBench == "")
                {
                    return;
                }
                else
                {
                    if (!cmbWorkBench.Items.Contains(workBench))
                    {
                        MessageDialog.ShowPromptMessage("不存在 " + workBench + " 工位");
                        return;
                    }

                    if (m_assemblingBom.UpdateWorkBench(cmbProductType.Text, parentName, workBench, updateParentPart, out m_err))
                    {
                        MessageDialog.ShowPromptMessage("操作成功");

                        btnRefresh_Click(null, null);
                    }
                    else
                    {
                        MessageDialog.ShowErrorMessage(m_err);
                    }
                }
            }
        }

        private void 删除分总成下所有子零件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null)
            {
                MessageDialog.ShowPromptMessage("请选择分总成树节点后再进行此操作");
                return;
            }

            if (MessageDialog.ShowEnquiryMessage("您确定要进行此操作吗？") == DialogResult.No)
                return;

            string parentName = (treeView1.SelectedNode.Tag as View_P_AssemblingBom).零件名称;

            View_P_AssemblingBom result = m_lstAssemblingBomBackup.Find(p => p.是否总成 == true && p.零件名称 == parentName);

            if (result == null)
            {
                MessageDialog.ShowPromptMessage(parentName + " 不是总成");
            }
            else
            {
                if (m_assemblingBom.DeletePart(cmbProductType.Text, parentName, out m_err))
                {
                    MessageDialog.ShowPromptMessage("操作成功");
                    RefreshData();
                }
                else
                {
                    MessageDialog.ShowErrorMessage(m_err);
                }
            }
        }

        private void 设置包含分总成在内及下属所有子零件工位号_Click(object sender, EventArgs e)
        {
            UpdateWorkBench(true);
        }

        private void dataGridView1_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.Clicks < 2) && (e.Button == MouseButtons.Left))
            {
                if ((e.ColumnIndex == -1) && (e.RowIndex > -1))
                {
                    m_selectionIdx = e.RowIndex;
                    dataGridView1.DoDragDrop(dataGridView1.Rows[e.RowIndex], DragDropEffects.Move);
                }
            }
        }

        private void dataGridView1_DragDrop(object sender, DragEventArgs e)
        {
            int idx = GetRowFromPoint(e.X, e.Y);

            if (idx < 0 || m_selectionIdx == idx)
                return;

            if (e.Data.GetDataPresent(typeof(DataGridViewRow)))
            {
                DataGridViewRow row = (DataGridViewRow)e.Data.GetData(typeof(DataGridViewRow));
                DataTable dt = (DataTable)dataGridView1.DataSource;
               
                var tempRow = dt.NewRow();

                tempRow.ItemArray = dt.Rows[m_selectionIdx].ItemArray;

                this.dataGridView1.CellEnter -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);

                dt.Rows.RemoveAt(m_selectionIdx);

                dt.Rows.InsertAt(tempRow, idx);

                this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);

                m_selectionIdx = idx;

                dataGridView1.Rows[m_selectionIdx].Selected = true;

                int visibleColumn = StapleInfo.GetVisibleColumn(dataGridView1);

                dataGridView1.CurrentCell = dataGridView1.Rows[m_selectionIdx].Cells[visibleColumn];
            }
        }

        /// <summary>
        /// 根据鼠标按键被释放时的鼠标位置计算行序号
        /// </summary>
        /// <param name="x">鼠标x轴</param>
        /// <param name="y">鼠标y轴</param>
        /// <returns>返回获取的行号</returns>
        private int GetRowFromPoint(int x, int y)
        {
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                Rectangle rec = dataGridView1.GetRowDisplayRectangle(i, false);

                if (dataGridView1.RectangleToScreen(rec).Contains(x, y))
                    return i;
            }

            return -1;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            //if ((dataGridView1.Rows.Count > 0) && (dataGridView1.SelectedRows.Count > 0) && (dataGridView1.SelectedRows[0].Index != m_selectionIdx))
            //{
            //    if (dataGridView1.Rows.Count <= m_selectionIdx)
            //        m_selectionIdx = dataGridView1.Rows.Count - 1;

            //    if (m_selectionIdx > -1)
            //    {
            //        dataGridView1.Rows[m_selectionIdx].Selected = true;

            //int visibleColumn = StapleInfo.GetVisibleColumn(dataGridView1);

            //        dataGridView1.CurrentCell = dataGridView1.Rows[m_selectionIdx].Cells[visibleColumn];
            //    }
            //}
        }

        private void dataGridView1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void btnSaveOrderNo_Click(object sender, EventArgs e)
        {
            List<View_P_AssemblingBom> lstData = new List<View_P_AssemblingBom>();

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewCellCollection cells = dataGridView1.Rows[i].Cells;

                if ((bool)cells["需光栅防错"].Value)
                {
                    lstData.Add(m_lstAssemblingBomBackup.Find(p =>
                        p.父总成编码 == cells["父总成编码"].Value.ToString() &&
                        p.工位 == cells["工位"].Value.ToString() &&
                        p.零件名称 == cells["零件名称"].Value.ToString() &&
                        p.规格 == cells["规格"].Value.ToString()));
                }
            }

            if (lstData.Count > 0)
            {
                lstData = lstData.OrderBy(p => p.工位).ToList();

                try
                {
                    m_assemblingBom.SaveOrderNo(lstData);

                    RefreshData();
                }
                catch (Exception exce)
                {
                    MessageDialog.ShowErrorMessage(exce.Message);
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("没有【需光栅防错】的零件，保存无效。");
            }
        }

        /// <summary>
        /// 复制装配顺序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCopyAssemblySequence_Click(object sender, EventArgs e)
        {
            FormCopyAssemblySequence form = new FormCopyAssemblySequence(FormCopyAssemblySequence.CopyModeEnum.复制指定工位装配顺序);

            if (form.ShowDialog() == DialogResult.OK)
            {
                if (!m_assemblingBom.CopyAssemblySequence(form.SourceProductInfo.产品类型编码,
                    form.TargetProductInfo.产品类型编码, form.Workbench, out m_err))
                {
                    MessageDialog.ShowErrorMessage(m_err);
                }
                else
                {
                    MessageDialog.ShowPromptMessage("复制成功");

                    RefreshData();
                }
            }
        }

        private void 防错toolStripButton_Click(object sender, EventArgs e)
        {
            设置防错信息 frm = new 设置防错信息();

            frm.ShowDialog();
        }

        private void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择表格中的零件后再进行此操作");
                return;
            }

            FormProductType form = new FormProductType();

            if (form.ShowDialog() == DialogResult.OK)
            {
                List<View_P_ProductInfo> productList = form.SelectedProduct;

                foreach (View_P_ProductInfo item in productList)
                {
                    if (item.产品类型编码 == dataGridView1.CurrentRow.Cells["产品编码"].Value.ToString())
                    {
                        continue;
                    }

                    if (m_assemblingBom.IsExistGoodsWorkbench(item.产品类型编码,
                        dataGridView1.CurrentRow.Cells["零件编码"].Value.ToString(),
                        dataGridView1.CurrentRow.Cells["工位"].Value.ToString()))
                    {
                        continue;
                    }

                    P_AssemblingBom assembile = new P_AssemblingBom();

                    assembile.AssemblyFlag = (bool)dataGridView1.CurrentRow.Cells["是否总成"].Value;
                    assembile.Date = ServerTime.Time;
                    assembile.FittingCounts = Convert.ToInt32(dataGridView1.CurrentRow.Cells["装配数量"].Value);
                    assembile.IsAdaptingPart = (bool)dataGridView1.CurrentRow.Cells["是否选配零件"].Value;
                    assembile.NeedToClean = (bool)dataGridView1.CurrentRow.Cells["是否清洗"].Value;
                    assembile.OrderNo = (int)dataGridView1.CurrentRow.Cells["装配顺序"].Value;
                    assembile.ParentCode = (string)dataGridView1.CurrentRow.Cells["父总成编码"].Value;
                    assembile.ParentName = (string)dataGridView1.CurrentRow.Cells["父总成名称"].Value;
                    assembile.PartCode = (string)dataGridView1.CurrentRow.Cells["零件编码"].Value;
                    assembile.PartName = (string)dataGridView1.CurrentRow.Cells["零件名称"].Value;
                    assembile.ProductCode = item.产品类型编码;
                    assembile.RasterProofing = (bool)dataGridView1.CurrentRow.Cells["需光栅防错"].Value;
                    assembile.Remarks = (string)dataGridView1.CurrentRow.Cells["备注"].Value;
                    assembile.Spec = (string)dataGridView1.CurrentRow.Cells["规格"].Value;
                    assembile.UserCode = BasicInfo.LoginID;
                    assembile.Workbench = (string)dataGridView1.CurrentRow.Cells["工位"].Value;

                    if (!m_assemblingBom.Add(assembile, out m_err))
                    {
                        MessageDialog.ShowErrorMessage(m_err);
                        return;
                    }             
                }

                InitViewData(m_lstAssemblingBom);
            }
        }

        private void 可替换toolStripButton_Click(object sender, EventArgs e)
        {
            可替换工位信息 frm = new 可替换工位信息();

            frm.ShowDialog();
        }
    }
}
