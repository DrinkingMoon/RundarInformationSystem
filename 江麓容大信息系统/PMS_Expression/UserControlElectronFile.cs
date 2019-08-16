/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  UserControlElectronFile.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2009/06/15
 * 开发平台:  vs2005(c#)
 * 用于    :  生产线管理信息系统
 *----------------------------------------------------------------------------
 * 描述 : 电子档案界面
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
using System.Text;
using System.Windows.Forms;
using ServerModule;
using PlatformManagement;
using System.Linq;
using GlobalObject;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 电子档案组件
    /// </summary>
    public partial class UserControlElectronFile : Form
    {
        #region variants

        /// <summary>
        /// 查询范围起始值 
        /// </summary>
        //string m_strFirst;

        /// <summary>
        /// 查询范围终止值
        /// </summary>
        //string m_strEnd;

        /// <summary>
        /// 查询条件
        /// </summary>
        //int m_type;

        /// <summary>
        /// 是否点击开始查询按钮标志
        /// </summary>
        bool m_find = false;

        /// <summary>
        /// 数据库中所有符合查询条件的记录表
        /// </summary>
        DataTable m_allTable;

        /// <summary>
        /// 每页显示的行数
        /// </summary>
        int m_pageSize = 25;

        /// <summary>
        /// 当前页号
        /// </summary>
        int m_pageCurrent = 1;

        /// <summary>
        /// 电子档案服务组件
        /// </summary>
        IElectronFileServer m_serverModule = ServerModuleFactory.GetServerModule<IElectronFileServer>();

        /// <summary>
        /// 时间查询范围界面
        /// </summary>
        //FormChoseDateRange m_frmChoseRange;

        /// <summary>
        /// 供应商对应的所有批号列表
        /// </summary>
        List<string> m_providerBatchNoList = new List<string>();

        /// <summary>
        /// 当前产品编号
        /// </summary>
        string m_curProductCode = "";

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// dataGridView1列刷新标志
        /// </summary>
        bool m_refreshFlag = false;

        /// <summary>
        /// 数据定位控件
        /// </summary>
        UserControlDataLocalizer m_dataLocalizer;

        /// <summary>
        /// 功能树节点信息
        /// </summary>
        PlatformManagement.FunctionTreeNodeInfo m_nodeInfo;

        /// <summary>
        /// 分总成电子档案记录列表
        /// </summary>
        List<P_ElectronFile> m_parentElectronWords = null;

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nodeInfo">功能树节点信息</param>
        public UserControlElectronFile(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_nodeInfo = nodeInfo;
        }

        /// <summary>
        /// 查找电子档案产品树
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFindProductTree_Click(object sender, EventArgs e)
        {
            if (txtProductCode.Text == "")
            {
                MessageDialog.ShowPromptMessage("产品编号不允许为空!");
                return;
            }

            m_curProductCode = txtProductCode.Text;
            InitTreeView(txtProductCode.Text);
        }

        /// <summary>
        /// 初始化TreeView
        /// </summary>
        /// <param name="productCode">产品编号</param>
        void InitTreeView(string productCode)
        {
            IQueryable<P_ElectronFile> eRecords;
            string error;

            treeView1.Nodes.Clear();

            if (m_serverModule.GetElectronFile(productCode, out eRecords, out error))
            {
                // 在后续剔除分总成的过程中防止剔除零部件编码为空的零件，如：白锌平垫
                List<string> lstParentCode = (from r in eRecords
                                              orderby r.ParentCode
                                              where r.ParentCode != ""
                                              select r.ParentCode).Distinct().ToList();

                m_parentElectronWords = (from r in eRecords
                                         where lstParentCode.Contains(r.GoodsCode)
                                         orderby r.GoodsCode, Convert.ToDateTime(r.FittingTime)
                                         select r).ToList();

                var rootGroup = from r in eRecords
                                where r.ParentCode == ""
                                select r;

                if (rootGroup.Count() > 1)
                {
                    MessageDialog.ShowErrorMessage(string.Format("箱号为[{0}] 的变速箱存在多个根产品信息记录，请及时反馈给系统管理员", 
                        productCode));
                    return;
                }
                else if (rootGroup.Count() == 0)
                {
                    MessageDialog.ShowErrorMessage(string.Format("箱号为[{0}] 的变速箱没有大总成记录，无法进行此查询请使用“综合查询”，其共有 {1} 条返修记录", 
                        productCode, eRecords.Count()));

                    return;
                }

                P_ElectronFile root = rootGroup.First();

                TreeNode rootNode = new TreeNode();

                rootNode.Name = root.GoodsCode;
                rootNode.Text = root.GoodsName;
                rootNode.ToolTipText = root.GoodsName + "," + root.GoodsCode + "," + root.Spec;
                rootNode.Tag = root;
                treeView1.Nodes.Add(rootNode);
                m_parentElectronWords.Remove(root);

                for (int i = 0; i < m_parentElectronWords.Count; i++)
                {
                    P_ElectronFile item = m_parentElectronWords[i];

                    // 剔除零部件编码为空的零件，如：白锌平垫
                    if (item.GoodsCode == "")
                    {
                        continue;
                    }

                    if (item.ParentName != root.GoodsName)
                    {
                        lstParentCode.Remove(item.GoodsCode);
                        continue;
                    }

                    TreeNode parentNode = new TreeNode();

                    parentNode.Name = item.GoodsCode;
                    parentNode.Text = item.GoodsName;
                    parentNode.ToolTipText = item.GoodsName + "," + item.GoodsCode + "," + item.Spec;
                    parentNode.Tag = item;

                    rootNode.Nodes.Add(parentNode);
                }

                List<P_ElectronFile> lstEF = (from r in eRecords
                                              where !lstParentCode.Contains(r.GoodsCode)
                                              orderby r.ParentName, r.ParentScanCode, r.GoodsName, r.GoodsCode, r.GoodsOnlyCode
                                              select r).ToList();

                for (int i = 0; i < rootNode.Nodes.Count; i++)
                {
                    RecursionBuildTreeView(rootNode.Nodes[i], m_parentElectronWords, lstEF);
                }

                RecursionBuildTreeView(rootNode, m_parentElectronWords, lstEF);
            }
            else
            {
                MessageDialog.ShowErrorMessage(error);
                return;
            }

            foreach (TreeNode item in treeView1.Nodes)
            {
                if (item.Nodes.Count > 0)
                {
                    item.Expand();
                }
            }
        }

        /// <summary>
        /// 递归生成电子档案的树型结构
        /// </summary>
        /// <param name="parentNode">父总成节点</param>
        /// <param name="parentRecords">父总成信息列表</param>
        /// <param name="eRecords">电子档案信息</param>
        void RecursionBuildTreeView(TreeNode parentNode, List<P_ElectronFile> parentRecords, List<P_ElectronFile> eRecords)
        {
            P_ElectronFile parentInfo = parentNode.Tag as P_ElectronFile;
            bool find = false;

            for (int i = 0; i < eRecords.Count; i++)
            {
                if (parentInfo.GoodsName != eRecords[i].ParentName)
                    continue;

                #region 是否找到更匹配的父节点（父总成扫描码=父节点零件标识码）

                find = false;

                foreach (P_ElectronFile item in parentRecords)
                {
                    if (item != parentInfo && item.GoodsCode == eRecords[i].ParentCode &&
                        item.GoodsOnlyCode == eRecords[i].ParentScanCode)
                    {
                        find = true;
                    }
                }

                #endregion

                if (find)
                {
                    continue;
                }

                TreeNode node = new TreeNode();

                node.Name = eRecords[i].GoodsCode;
                node.Text = eRecords[i].GoodsName;
                node.ToolTipText = eRecords[i].Spec;
                node.Tag = eRecords[i];
                parentNode.Nodes.Add(node);

                eRecords.RemoveAt(i);
                i = -1;

                RecursionBuildTreeView(node, parentRecords, eRecords);
            }
        }

        /// <summary>
        /// 更改选定treeView1内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                P_ElectronFile data = treeView1.SelectedNode.Tag as P_ElectronFile;

                if (!m_serverModule.GetElectronFile(m_curProductCode, data.ParentCode, data.GoodsOnlyCode,
                    data.GoodsCode, data.Spec, out m_allTable, out m_err))
                {
                    MessageDialog.ShowErrorMessage(m_err);
                    return;
                }

                // 相同分总成数量（对于返修时整体替换分总成的情况）
                int parentCount = 0;

                foreach (P_ElectronFile item in m_parentElectronWords)
                {
                    if (data.GoodsName == item.GoodsName)
                    {
                        parentCount++;
                    }
                }

                if (m_allTable != null && treeView1.SelectedNode.Parent != null && (parentCount > 1 || parentCount == 0))
                {
                    for (int i = 0; i < m_allTable.Rows.Count; i++)
                    {
                        string parentScanCode = m_allTable.Rows[i].Field<string>("父总成扫描码");
                        string goodsOnlyCode = m_allTable.Rows[i].Field<string>("零件标识码");

                        // parentCount > 1 表示当前选择节点是总成节点且有多个相同的总成节点(表示有总成被替换过)
                        // parentCount = 0 表示当前选择节点是零件节点
                        if ((parentCount > 1 && parentScanCode != data.GoodsOnlyCode && parentScanCode != data.GoodsCode && 
                            data.GoodsOnlyCode != goodsOnlyCode) ||
                            (parentCount == 0 && parentScanCode != data.ParentScanCode))
                            m_allTable.Rows.RemoveAt(i--);
                    }
                }

                RefreshDataGridView();
            }
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="table"></param>
        void RefreshDataGridView()
        {
            bindingSource1.DataSource = m_allTable;

            bindingNavigator1.BindingSource = bindingSource1;

            dataGridView1.DataSource = bindingSource1;

            if (m_allTable != null)
            {
                dataGridView1.Columns[0].Visible = false;
            }

            if (!m_refreshFlag)
            {
                m_refreshFlag = true;
            }

            this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            if (m_dataLocalizer == null)
            {
                m_dataLocalizer = new UserControlDataLocalizer(dataGridView1, this.Name, 
                    UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
                panelLocalizer.Controls.Add(m_dataLocalizer);
                m_dataLocalizer.Dock = DockStyle.Bottom;
            }

            dataGridView1.Refresh();
        }

        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControlElectronFile_Load(object sender, EventArgs e)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip1, m_nodeInfo.Authority);
            FaceAuthoritySetting.SetVisibly(contextMenuStrip1, m_nodeInfo.Authority);

            cmbFindFashion.SelectedIndex = 0;
            cmbFindConditon.SelectedIndex = 0;
            pnlRange2.Visible = true;
            pnlRange1.Visible = false;
            txtCondition.Focus();
        }

        /// <summary>
        /// 改变查询方式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comFindFashion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFindFashion.SelectedIndex == 0)
            {
                panelTree.Enabled = true;
                panelOther.Enabled = false;
                m_find = false;
            }
            else
            {
                panelTree.Enabled = false;
                panelOther.Enabled = true;
            }
        }

        /// <summary>
        /// 查询条件下拉框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comFindConditon_DropDownClosed(object sender, EventArgs e)
        {
            if (cmbFindConditon.SelectedIndex == 0 || cmbFindConditon.SelectedIndex == 1)
            {
                pnlRange2.Visible = true;
                pnlRange1.Visible = false;
                lbFindTitle2.Text = "请输入" + cmbFindConditon.SelectedItem.ToString() + "字段：";
                Recover2();
            }
            else
            {
                pnlRange2.Visible = false;
                pnlRange1.Visible = true;
                lbFindTitle1.Text = "请输入" + cmbFindConditon.SelectedItem.ToString() + "字段：";
                Recover1();
            }
        }

        /// <summary>
        /// 清除输入内容
        /// </summary>
        void Recover1()
        {
            txtBegin.Clear();
            txtEnd.Clear();
            btnDate.Focus();
        }

        /// <summary>
        /// 清除输入内容
        /// </summary>
        void Recover2()
        {
            txtCondition.Clear();
            txtCondition.Focus();
        }

        /// <summary>
        /// 开始查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBeginFind_Click(object sender, EventArgs e)
        {
            m_pageCurrent = 1;

            if (OtherFashionFind())
            {
                m_find = true;
            }
        }

        /// <summary>
        /// 其他查询方式查询
        /// </summary>
        bool OtherFashionFind()
        {
            if (cmbFindConditon.SelectedIndex == 0 || cmbFindConditon.SelectedIndex == 1) //按零件标识码查询
            {
                if (txtCondition.Text == "")
                {
                    string msg = "请输入" + cmbFindConditon.SelectedItem.ToString() + "字段内容!";
                    MessageDialog.ShowPromptMessage(msg);
                    return false;
                }

                if (!m_serverModule.GetEspeciallyElectronFile(cmbFindConditon.SelectedIndex, 
                    txtCondition.Text.ToUpper(), m_pageSize, m_pageCurrent, out m_allTable, out m_err))
                {
                    MessageDialog.ShowErrorMessage(m_err);
                    return false;
                }

                RefreshDataGridView();
            }
            else if (cmbFindConditon.SelectedIndex == 2)
            {
                if (txtBegin.Text == "" || txtEnd.Text == "")
                {
                    string msg = "请输入完整" + cmbFindConditon.SelectedItem.ToString() + "字段范围!";
                    MessageDialog.ShowPromptMessage(msg);
                    return false;
                }

                if (!m_serverModule.GetEspeciallyElectronFile(cmbFindConditon.SelectedIndex, 
                    txtBegin.Text.ToUpper(), txtEnd.Text.ToUpper(), m_pageSize, m_pageCurrent, out m_allTable, out m_err))
                {
                    MessageDialog.ShowErrorMessage(m_err);
                    return false;
                }

                RefreshDataGridView();
            }
            else
            {
                if (!m_serverModule.GetEspeciallyElectronFile(cmbFindConditon.SelectedIndex, 
                    txtBegin.Text.ToUpper(), txtEnd.Text.ToUpper(), m_pageSize, m_pageCurrent, out m_allTable, out m_err))
                {
                    MessageDialog.ShowErrorMessage(m_err);
                    return false;
                }

                RefreshDataGridView();
            }

            return true;
        }

        /// <summary>
        /// 起止日期
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDate_Click(object sender, EventArgs e)
        {
            MessageDialog.ShowPromptMessage("请使用综合查询功能");
        }

        /// <summary>
        /// 菜单点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bindingNavigator1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            #region 首页

            if (e.ClickedItem.Text == "首页")
            {
                if (m_find == true)
                {
                    if (m_pageCurrent == 1)
                    {
                        MessageDialog.ShowPromptMessage("当前页即为首页!");
                    }
                    else
                    {
                        m_pageCurrent = 1;
                        OtherFashionFind();
                    }
                }
                else
                {
                    MessageDialog.ShowPromptMessage("请先点击“确定”按钮进行查找!");
                }
            }

            #endregion

            #region 末页

            if (e.ClickedItem.Text == "末页")
            {
                if (m_find == true)
                {
                    int totalCount = m_serverModule.GetIndexEspeciallyElectronFile(txtCondition.Text, cmbFindConditon.SelectedIndex);

                    if (cmbFindConditon.SelectedIndex == 2 || cmbFindConditon.SelectedIndex == 3)
                    {
                        totalCount = m_serverModule.GetIndexEspeciallyElectronFile(txtBegin.Text, txtEnd.Text, cmbFindConditon.SelectedIndex);
                    }

                    if (totalCount == 0)
                    {
                        MessageDialog.ShowPromptMessage("数据库中查无记录!");
                    }
                    else
                    {
                        int endPage;

                        if (totalCount % m_pageSize > 0)
                        {
                            endPage = totalCount / m_pageSize + 1;
                        }
                        else
                        {
                            endPage = totalCount / m_pageSize;
                        }

                        if (endPage == m_pageCurrent)
                        {
                            MessageDialog.ShowPromptMessage("当前已经为末页!");
                        }
                        else
                        {
                            m_pageCurrent = endPage;

                            FindLastEspeciallyElectronFile();
                        }
                    }
                }
                else
                {
                    MessageDialog.ShowPromptMessage("请先点击“确定”按钮进行查找!");
                }
            }

            #endregion

            #region 上一页

            if (e.ClickedItem.Text == "上一页")
            {
                if (m_find == true)
                {
                    if (m_pageCurrent == 1)
                    {
                        MessageDialog.ShowPromptMessage("当前已经为首页，您可点击“下一页”查看其他记录!");
                    }
                    else
                    {
                        m_pageCurrent--;
                        OtherFashionFind();
                    }
                }
                else
                {
                    MessageDialog.ShowPromptMessage("请先点击“确定”按钮进行查找!");
                }
            }

            #endregion

            #region 下一页

            if (e.ClickedItem.Text == "下一页")
            {
                if (m_find == true)
                {
                    int totalCount = m_serverModule.GetIndexEspeciallyElectronFile(txtCondition.Text, cmbFindConditon.SelectedIndex);

                    if (cmbFindConditon.SelectedIndex == 2 || cmbFindConditon.SelectedIndex == 3)
                    {
                        totalCount = m_serverModule.GetIndexEspeciallyElectronFile(txtBegin.Text, txtEnd.Text, cmbFindConditon.SelectedIndex);
                    }

                    int endPage;

                    if (totalCount % m_pageSize > 0)
                    {
                        endPage = totalCount / m_pageSize + 1;
                    }
                    else
                    {
                        endPage = totalCount / m_pageSize;
                    }

                    if (endPage == m_pageCurrent)
                    {
                        MessageDialog.ShowPromptMessage("当前已经为末页，您可点击“上一页”查看其他记录!");
                    }
                    else
                    {
                        m_pageCurrent++;
                        OtherFashionFind();
                    }
                }
                else
                {
                    MessageDialog.ShowPromptMessage("请先点击“确定”按钮进行查找!");
                }
            }

            #endregion
        }

        /// <summary>
        /// 查找末页
        /// </summary>
        /// <returns></returns>
        void FindLastEspeciallyElectronFile()
        {
            if (cmbFindConditon.SelectedIndex == 0 || cmbFindConditon.SelectedIndex == 1) //按零件标识码查询
            {
                if (!m_serverModule.GetLastEspeciallyElectronFile(txtCondition.Text, m_pageSize, 
                    cmbFindConditon.SelectedIndex, out m_allTable, out m_err))
                {
                    MessageDialog.ShowErrorMessage(m_err);
                    return;
                }

                RefreshDataGridView();
            }
            else
            {
                if (!m_serverModule.GetLastEspeciallyElectronFile(txtBegin.Text, txtEnd.Text, 
                    m_pageSize, cmbFindConditon.SelectedIndex, out m_allTable, out m_err))
                {
                    MessageDialog.ShowErrorMessage(m_err);
                    return;
                }

                RefreshDataGridView();
            }
        }

        /// <summary>
        /// 设置起止日期
        /// </summary>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        public void SetDate(string beginDate, string endDate)
        {
            txtBegin.Text = beginDate;
            txtEnd.Text = endDate;
        }

        /// <summary>
        /// 提交检验信息
        /// </summary>
        void ReferCheckUser()
        {
            //int n = dataGridView1.SelectedRows.Count;

            //if (n == 1)
            //{
            //    #region

            //    string productCode = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            //    string parentCode = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            //    string partCode = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            //    string partName = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            //    string uintCode = dataGridView1.CurrentRow.Cells[6].Value.ToString();
            //    string counts = dataGridView1.CurrentRow.Cells[7].Value.ToString();
            //    string providerCode = dataGridView1.CurrentRow.Cells[8].Value.ToString();
            //    string outDepotCount = dataGridView1.CurrentRow.Cells[9].Value.ToString();
            //    string batchNumber = dataGridView1.CurrentRow.Cells[10].Value.ToString();
            //    string workbench = dataGridView1.CurrentRow.Cells[11].Value.ToString();
            //    string washState = dataGridView1.CurrentRow.Cells[12].Value.ToString();
            //    string checkData = dataGridView1.CurrentRow.Cells[13].Value.ToString();
            //    string factData = dataGridView1.CurrentRow.Cells[14].Value.ToString();
            //    string fittingUserCode = dataGridView1.CurrentRow.Cells[15].Value.ToString();
            //    string fittingTime = dataGridView1.CurrentRow.Cells[16].Value.ToString();
            //    string checkUserCode = txtCheckUser.Text;
            //    string remark = dataGridView1.CurrentRow.Cells[18].Value.ToString();
            //    string id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            //    string accessoryCode = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            //    string error;

            //    #endregion

            //    if (m_serverModule.AddElectronFile(productCode, parentCode, partCode, partName, uintCode, counts, providerCode, outDepotCount, batchNumber,
            //        workbench, washState, checkData, factData, fittingUserCode, fittingTime, checkUserCode, remark, id, accessoryCode, out error))
            //    {
            //        Init();
            //    }
            //    else
            //    {
            //        MessageBox.Show(error, "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        return;
            //    }

            //}
            //else if (n > 1)
            //{
            //    #region

            //    string[] productCode = new string[n];
            //    string[] parentCode = new string[n];
            //    string[] partCode = new string[n];
            //    string[] partName = new string[n];
            //    string[] uintCode = new string[n];
            //    string[] counts = new string[n];
            //    string[] providerCode = new string[n];
            //    string[] outDepotCount = new string[n];
            //    string[] batchNumber = new string[n];
            //    string[] workbench = new string[n];
            //    string[] washState = new string[n];
            //    string[] checkData = new string[n];
            //    string[] factData = new string[n];
            //    string[] fittingUserCode = new string[n];
            //    string[] fittingTime = new string[n];
            //    string[] checkUserCode = new string[n];
            //    string[] remark = new string[n];
            //    string[] id = new string[n];
            //    string[] accessoryCode = new string[n];

            //    #endregion

            //    for (int i = 0; i < n; i++)
            //    {
            //        #region

            //        productCode[i] = dataGridView1.SelectedRows[i].Cells[1].Value.ToString();
            //        parentCode[i] = dataGridView1.SelectedRows[i].Cells[2].Value.ToString();
            //        partCode[i] = dataGridView1.SelectedRows[i].Cells[3].Value.ToString();
            //        partName[i] = dataGridView1.SelectedRows[i].Cells[5].Value.ToString();
            //        uintCode[i] = dataGridView1.SelectedRows[i].Cells[6].Value.ToString();
            //        counts[i] = dataGridView1.SelectedRows[i].Cells[7].Value.ToString();
            //        providerCode[i] = dataGridView1.SelectedRows[i].Cells[8].Value.ToString();
            //        outDepotCount[i] = dataGridView1.SelectedRows[i].Cells[9].Value.ToString();
            //        batchNumber[i] = dataGridView1.SelectedRows[i].Cells[10].Value.ToString();
            //        workbench[i] = dataGridView1.SelectedRows[i].Cells[11].Value.ToString();
            //        washState[i] = dataGridView1.SelectedRows[i].Cells[12].Value.ToString();
            //        checkData[i] = dataGridView1.SelectedRows[i].Cells[13].Value.ToString();
            //        factData[i] = dataGridView1.SelectedRows[i].Cells[14].Value.ToString();
            //        fittingUserCode[i] = dataGridView1.SelectedRows[i].Cells[15].Value.ToString();
            //        fittingTime[i] = dataGridView1.SelectedRows[i].Cells[16].Value.ToString();
            //        checkUserCode[i] = txtCheckUser.Text;
            //        remark[i] = dataGridView1.SelectedRows[i].Cells[18].Value.ToString();
            //        id[i] = dataGridView1.SelectedRows[i].Cells[0].Value.ToString();
            //        accessoryCode[i] = dataGridView1.SelectedRows[i].Cells[4].Value.ToString();

            //        #endregion
            //    }

            //    string error;

            //    for (int i = 0; i < n; i++)
            //    {
            //        if (m_serverModule.AddElectronFile(productCode[i], parentCode[i], partCode[i], partName[i], uintCode[i], counts[i], providerCode[i],
            //            outDepotCount[i], batchNumber[i], workbench[i], washState[i], checkData[i], factData[i], fittingUserCode[i], fittingTime[i],
            //            checkUserCode[i], remark[i], id[i], accessoryCode[i], out error))
            //        {
            //            Init();
            //        }
            //        else
            //        {
            //            MessageBox.Show(error, "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //            return;
            //        }
            //    }
            //}
        }

        /// <summary>
        /// 右键菜单点击修改记录行事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 修改电子档案_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowErrorMessage("请选择一行记录后再进行此操作");
                return;
            }

            if (!dataGridView1.Columns.Contains("序号"))
            {
                MessageDialog.ShowErrorMessage("当前查询方式获取到的数据没有“序号”列，无法进行此操作，请换一种查询方式后再进行此操作");
                return;
            }

            FormModificateElectronFile form = new FormModificateElectronFile((long)dataGridView1.SelectedRows[0].Cells["序号"].Value);
            form.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            电子档案录入 form = new 电子档案录入();
            form.ShowDialog();
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            IAuthorization authorization = PlatformFactory.GetObject<IAuthorization>();
            string businessID = "电子档案查询";
            IQueryResult qr = authorization.Query(businessID, null, null, 0);
            List<string> lstFindField = new List<string>();
            DataColumnCollection columns = qr.DataCollection.Tables[0].Columns;

            if (qr.Succeeded && columns.Count > 0)
            {
                for (int i = 0; i < columns.Count; i++)
                {
                    lstFindField.Add(columns[i].ColumnName);
                }
            }

            FormConditionFind formFindCondition = new FormConditionFind(this, lstFindField.ToArray(), businessID, labelTitle.Text);
            formFindCondition.ShowDialog();
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
            else if (dataGridView1.SelectedRows.Count > 1)
            {
                MessageDialog.ShowPromptMessage("只允许选择一条记录进行处理！");
                return false;
            }

            return true;
        }

        private void 提交返修信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
                return;

            int index = dataGridView1.SelectedRows[0].Index;

            录入电子档案返修信息 form = new 录入电子档案返修信息(
                录入电子档案返修信息.UpdateModeEnum.返修, ((DataTable)bindingSource1.DataSource).Rows[index]);

            form.CloseChildFormEvent += new GlobalObject.DelegateCollection.CloseFormDelegate(this.关闭录入电子档案返修信息窗体事件);
            form.Show();
        }

        private void 关闭录入电子档案返修信息窗体事件(DialogResult dialogResult)
        {
            if (dialogResult == DialogResult.OK)
            {
                TreeViewEventArgs args = new TreeViewEventArgs(treeView1.SelectedNode);
                treeView1_AfterSelect(null, args);
            }
        }

        /// <summary>
        /// 变速箱维修信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRepairCVT_Click(object sender, EventArgs e)
        {
            变更变速箱箱号 form = new 变更变速箱箱号(m_nodeInfo);
            form.Show();
        }

        /// <summary>
        /// 将临时电子档案转换成正式电子档案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConvertEF_Click(object sender, EventArgs e)
        {
            转换临时电子档案 form = new 转换临时电子档案();
            form.ShowDialog();
        }

        private void 添加调整信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
                return;

            int index = dataGridView1.SelectedRows[0].Index;

            录入电子档案返修信息 form = new 录入电子档案返修信息(
                录入电子档案返修信息.UpdateModeEnum.调整, ((DataTable)bindingSource1.DataSource).Rows[index]);

            form.CloseChildFormEvent += new GlobalObject.DelegateCollection.CloseFormDelegate(this.关闭录入电子档案返修信息窗体事件);
            form.Show();
        }

        /// <summary>
        /// 根据已有电子档案为模板生成指定箱号电子档案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCopyEF_Click(object sender, EventArgs e)
        {
            拷贝电子档案信息 form = new 拷贝电子档案信息();
            form.ShowDialog();
        }

        private void UserControlElectronFile_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        /// <summary>
        /// 双击数据显示控件以便捷的方式查看数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            FormViewData form = new FormViewData(dataGridView1.Columns, dataGridView1.Rows[e.RowIndex]);
            form.ShowDialog();
        }

        /// <summary>
        /// 快速维修当前变速箱
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFastRepairCVT_Click(object sender, EventArgs e)
        {
            快速返修变速箱 form = new 快速返修变速箱();
            form.Show(this);
        }

        private void 查看数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择一行数据后再进行此操作");
                return;
            }

            FormViewData form = new FormViewData(dataGridView1.Columns, dataGridView1.Rows[dataGridView1.SelectedRows[0].Index]);
            form.ShowDialog();
        }

        /// <summary>
        /// 查看变速箱重量信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearchWeight_Click(object sender, EventArgs e)
        {
            查看变速箱称重信息 form = new 查看变速箱称重信息();

            form.Show();
        }

        /// <summary>
        /// 查看变速箱交接信息(变速箱从装配车间与下线车间之间的交接)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearchCVTHandoverInfo_Click(object sender, EventArgs e)
        {
            查看变速箱交接信息 form = new 查看变速箱交接信息();

            form.Show();
        }

        /// <summary>
        /// 批量移交变速箱(变速箱从装配车间与下线车间之间的交接)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 批量移交变速箱_Click(object sender, EventArgs e)
        {
            批量移交变速箱 form = new 批量移交变速箱();

            form.Show();
        }

        /// <summary>
        /// 批量变更变速箱箱号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConvertBatchedCVT_Click(object sender, EventArgs e)
        {
            批量变更变速箱箱号 form = new 批量变更变速箱箱号();

            form.Show();
        }

        /// <summary>
        /// 统计零件装配数量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 统计零件装配数量ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            查看零件装配数量 form = new 查看零件装配数量();

            form.Show();
        }
    }
}
