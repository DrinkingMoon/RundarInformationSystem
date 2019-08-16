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
using CommonBusinessModule;

namespace Expression
{
    /// <summary>
    /// 材料类别管理界面
    /// </summary>
    public partial class UserControlDepotTypeForPersonnel : Form
    {
        /// <summary>
        /// NodeTag数组
        /// </summary>
        DataTable m_dtNodeTag = new DataTable();

        /// <summary>
        /// contxtmenu 添加事件
        /// </summary>
        bool m_blAdd = false;

        /// <summary>
        /// 状态标志
        /// </summary>
        string m_strStatus = "默认";

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err = "";

        /// <summary>
        /// 焦点节点
        /// </summary>
        TreeNode m_tnMySelectedNode = new TreeNode();

        /// <summary>
        /// 焦点节点的父节点
        /// </summary>
        TreeNode m_tnRootNode = new TreeNode();

        /// <summary>
        /// 材料类型
        /// </summary>
        IQueryable<S_Depot> m_findDepotType;

        /// <summary>
        /// 材料与管理人的关系表
        /// </summary>
        IQueryable<S_DepotTypeForPersonnel> m_findDepotForPersonnel;

        /// <summary>
        /// 关系表信息
        /// </summary>
        IQueryable<S_DepotForDtp> m_findDepotForDtp;

        /// <summary>
        /// 材料类型与管理人关系
        /// </summary>
        IDepotTypeForPersonnel m_serverDepotTypeForPersonnel = ServerModuleFactory.GetServerModule<IDepotTypeForPersonnel>();

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authorityFlag;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nodeInfo">功能树节点信息</param>
        public UserControlDepotTypeForPersonnel(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authorityFlag = nodeInfo.Authority;            
        }

        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControlDepotForPersonnel_Load(object sender, EventArgs e)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip1, m_authorityFlag);
            m_dtNodeTag.Columns.Add("String");

            m_findDepotType = m_serverDepotTypeForPersonnel.GetDepotTypeBill();
            GlobalObject.GeneralFunction.LoadTreeViewDt(treeView1,
                           m_serverDepotTypeForPersonnel.ChangeDataTable(
                           GlobalObject.GeneralFunction.ConvertToDataTable<S_Depot>(m_findDepotType)),
                           "DepotName", "DepotCode", "RootSign", "RootSign = 'Root'");
            RefreshControl();
        }

        private void UserControlDepotTypeForPersonnel_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }
        
        #region 树的操作

        #region 设置CheckBox
        /// <summary>
        /// 递归节点
        /// </summary>
        /// <param name="tn">节点</param>
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
        /// <param name="node">节点</param>
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
        /// <param name="node">节点</param>
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


        #region 设置叶子节点的Check状态
        /// <summary>
        /// 字符串分割并且开始递归
        /// </summary>
        /// <param name="tvControl">功能树</param>
        /// <param name="Dt">子节点信息</param>
        public void FindNodeSet(TreeView tvControl, DataTable dt)
        {
            for (int i = 0; i<= dt.Rows.Count-1;i++ )
            {
                foreach (TreeNode tn in tvControl.Nodes)
                {
                    tn.Expand();
                    FindNodeSet_Show(tn, dt.Rows[i]["DepotCode"].ToString());
                }
            }
        }

        /// <summary>
        /// 递归设置
        /// </summary>
        /// <param name="tnParent">根节点</param>
        /// <param name="strValue">节点信息</param>
        public void FindNodeSet_Show(TreeNode tnParent, string strValue)
        {
            if (tnParent == null)
            { return; }

            if (tnParent.Tag.ToString() == strValue)
            { 
                tnParent.Checked = true;
                SetNodeStyle(tnParent.Parent);
            }

            foreach (TreeNode tn in tnParent.Nodes)
            {
                tn.Expand();
                FindNodeSet_Show(tn, strValue);
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
        /// <param name="tnParent">父节点</param>
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

        #endregion

        #region 添加 删除 修改

        /// <summary>
        /// 新增TOOLSTRIP
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 新增ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                m_strStatus = "新增";

                TreeNode node = treeView1.SelectedNode;
                TreeNode NewNode = new TreeNode();

                NewNode.Text = "新类别";
                NewNode.Tag = node.Tag.ToString() + "00";

                node.Nodes.Add(NewNode);
                m_tnMySelectedNode = NewNode; 
                修改ToolStripMenuItem_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        /// <summary>
        /// 修改TOOLSTRIP
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_tnMySelectedNode != null && m_tnMySelectedNode.Parent != null && m_tnMySelectedNode.Nodes.Count == 0)
            {
                if (m_strStatus == "默认")
                {
                    m_strStatus = "修改";
                }

                treeView1.SelectedNode = m_tnMySelectedNode;
                treeView1.LabelEdit = true;

                if (!m_tnMySelectedNode.IsEditing)
                {
                    m_tnMySelectedNode.BeginEdit();
                }

                m_blAdd = true;
            }
            else
            {
                MessageBox.Show("顶级类型不允许被编辑", "提示");
            }
        }

        /// <summary>
        /// 删除TOOLSTRIP
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                TreeNode Node = treeView1.SelectedNode;

                if (!m_serverDepotTypeForPersonnel.FindMessgeForDtp(Node.Tag.ToString(),out m_err))
                {
                    if (MessageDialog.ShowEnquiryMessage("是否要删除[" + Node.Text + "],同时会删除此类型的下级所有类型,是否要继续？") 
                        == DialogResult.Yes)
                    {
                        if (!m_serverDepotTypeForPersonnel.DeleteBill(Node.Tag.ToString(), out m_err))
                        {
                            MessageDialog.ShowErrorMessage(m_err);
                            return;
                        }
                        else
                        {
                            treeView1.Nodes.Remove(Node);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("此记录存在关系表不能删除，如要删除请更改关系表" + m_err, "提示");
                    return;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 添加按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckDate())
                {
                    if (!m_serverDepotTypeForPersonnel.AddDepotForPersonnel(CreateLinqDepotForPersonnel(), out m_err))
                    {
                        MessageDialog.ShowErrorMessage(m_err);
                        return;
                    }
                    else
                    {
                        if (!m_serverDepotTypeForPersonnel.AddDtp(CreateLinqDepotForDtp(),out m_err))
                        {
                            MessageDialog.ShowErrorMessage(m_err);
                            return;
                        }
                        else
                        {
                            MessageBox.Show("类型添加成功！", "提示");
                        }
                    }

                    RefreshControl();
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 修改按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckDate())
                {
                    if (!m_serverDepotTypeForPersonnel.UpdateDepotForPersonnel(CreateLinqDepotForPersonnel(),txtZlID.Text, out m_err))
                    {
                        MessageDialog.ShowErrorMessage(m_err);
                        return;
                    }
                    else
                    {
                        if (!m_serverDepotTypeForPersonnel.UpdateDtp(CreateLinqDepotForDtp(), txtZlID.Text, out m_err))
                        {
                            MessageDialog.ShowErrorMessage(m_err);
                            return;
                        }
                        else
                        {
                            MessageBox.Show("类型修改成功！", "提示");
                        }
                    }

                    RefreshControl();
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 删除按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtZlID.Text != "")
                {
                    if (!m_serverDepotTypeForPersonnel.DeleteDepotForPersonnel(txtZlID.Text, out m_err))
                    {
                        MessageDialog.ShowErrorMessage(m_err);
                        return;
                    }
                    else
                    {
                        if (!m_serverDepotTypeForPersonnel.DeleteDtp(txtZlID.Text, out m_err))
                        {
                            MessageDialog.ShowErrorMessage(m_err);
                            return;
                        }
                        else
                        {
                            MessageBox.Show("类型删除成功！", "提示");
                        }
                    }

                    RefreshControl();
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region 方法


        /// <summary>
        /// 填充Depot Linq值
        /// </summary>
        /// <param name="NowNode">树节点</param>
        /// <returns>返回材料类别信息列表</returns>
        private S_Depot CreateLinqDepot(TreeNode tnNowNode)
        {
            S_Depot SaveDepot = new S_Depot();

            SaveDepot.DepotCode = tnNowNode.Tag.ToString();
            SaveDepot.DepotName = tnNowNode.Text;
            SaveDepot.DepotGrade = Convert.ToByte( tnNowNode.Tag.ToString().Length/2 -1 );
            SaveDepot.IsEnd = tnNowNode.Nodes.Count == 0 ? true : false;
            return SaveDepot;
        }

        /// <summary>
        /// 填充DepotForPersonnel Linq值
        /// </summary>
        /// <returns>返回材料类别责任人数据列表</returns>
        private S_DepotTypeForPersonnel CreateLinqDepotForPersonnel()
        {
            S_DepotTypeForPersonnel depotforpersonnel = new S_DepotTypeForPersonnel();

            depotforpersonnel.ZlID = txtZlID.Text;
            depotforpersonnel.ZlName = txtZlName.Text;

            if (txtPersonnelName.DataResult == null)
            {
                depotforpersonnel.PersonnelID = txtPersonnelName.Tag.ToString();
            }
            else
            {
                depotforpersonnel.PersonnelID = txtPersonnelName["工号"].ToString();
            }

            depotforpersonnel.PersonnelName = txtPersonnelName.Text;
            return depotforpersonnel;
        }

        /// <summary>
        /// 获得Linq集合
        /// </summary>
        /// <returns>返回材料类别信息列表</returns>
        private List<S_DepotForDtp> CreateLinqDepotForDtp()
        {
            GetNodeValues(treeView1);
            List<S_DepotForDtp> LisDfd = new List<S_DepotForDtp>();

            if (m_dtNodeTag.Rows.Count == 0)
            {
                return null;
            }

            else
            {
                for (int i = 0; i <= m_dtNodeTag.Rows.Count - 1; i++)
                {
                    S_DepotForDtp dfd = new S_DepotForDtp();

                    dfd.DtpCode = txtZlID.Text;
                    dfd.DepotCode = m_dtNodeTag.Rows[i][0].ToString();
                    LisDfd.Add(dfd);
                }

                return LisDfd;
            }
        }

        /// <summary>
        /// 刷新控件
        /// </summary>
        private void RefreshControl()
        {
            ClearDate();
            m_findDepotForPersonnel = m_serverDepotTypeForPersonnel.GetDepotForPersonnel();
            dataGridView1.DataSource = m_findDepotForPersonnel;
            dataGridView1.Refresh();
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        private void ClearDate()
        {
            txtPersonnelName.Text = "";
            txtPersonnelName.Tag = null;
            txtZlID.Text = "";
            txtZlName.Text = "";
            SetRoot(treeView1);
            m_dtNodeTag.Clear();
        }

        /// <summary>
        /// 检查数据
        /// </summary>
        /// <returns></returns>
        private bool CheckDate()
        {
            if(txtZlID.Text=="")
            {
                MessageBox.Show("请录入材料类别编码！","提示");
                return false;
            }

            if (txtZlName.Text == "")
            {
                MessageBox.Show("请录入材料类别名称！", "提示");
                return false;
            }

            if (txtPersonnelName.Text == "")
            {
                MessageBox.Show("请录入材料管理人！", "提示");
                return false;
            }

            if (txtPersonnelName.Tag.ToString() == "")
            {
                MessageBox.Show("请重新录入材料管理人，该管理人工号为空！", "提示");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 检查重复的Node的TAG
        /// </summary>
        /// <param name="node">节点</param>
        /// <param name="nodeText">节点信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>有重复NODE返回False</returns>
        private bool FindOldNode(TreeNode node, string nodeText, out string error)
        {
            foreach (TreeNode tn in node.Parent.Nodes)
            {
                if (tn.Tag.ToString() == nodeText && tn != node)
                {
                    error = "材料编码重复，请重新录入材料名称";
                    return false;
                }
            }

            error = "";
            return true;
        }

        #endregion 

        #region 控件事件

        /// <summary>
        /// 获取人员信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFindPersonnel_Click(object sender, EventArgs e)
        {
            FormPersonnel form = new FormPersonnel(txtPersonnelName);
            form.ShowDialog();

            txtPersonnelName.Tag = form.UserCode;
        }

        /// <summary>
        /// DataGridView双击设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            SetRoot(treeView1);

            if (dataGridView1.Rows[0].Cells[0].Value == null)
            {
                return;
            }
            try
            {
                DataTable dttb = new DataTable();

                txtZlID.Text = dataGridView1.Rows[e.RowIndex].Cells["ZlID"].Value.ToString();
                txtZlName.Text = dataGridView1.Rows[e.RowIndex].Cells["ZlName"].Value.ToString();
                txtPersonnelName.Text = dataGridView1.Rows[e.RowIndex].Cells["PersonnelName"].Value.ToString();
                txtPersonnelName.Tag = dataGridView1.Rows[e.RowIndex].Cells["PersonnelID"].Value.ToString();
                m_findDepotForDtp = m_serverDepotTypeForPersonnel.GetDtp(dataGridView1.Rows[e.RowIndex].Cells["ZlID"].Value.ToString());

                dttb = GlobalObject.GeneralFunction.ConvertToDataTable<S_DepotForDtp>(m_findDepotForDtp);

                if (dttb.Rows.Count != 0 )
                {
                    FindNodeSet(treeView1, dttb);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// TREEVIEW 的鼠标按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        {
            m_tnMySelectedNode = treeView1.GetNodeAt(e.X, e.Y);
        }

        /// <summary>
        /// treeview文本编辑后
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Label != null)
            {
                if (e.Label.Length > 0)
                {
                    if (e.Label.IndexOfAny(new char[] { '@', '.', ',', '!' }) == -1)
                    {
                        if (m_blAdd == true)
                        {
                            string code = m_tnMySelectedNode.Tag.ToString();
                            m_tnMySelectedNode.Tag = m_tnMySelectedNode.Parent.Tag.ToString() +
                                         ServerModule.PYMnumber.GetPYString(e.Label.ToString().Substring(0, 2));
                            if (!FindOldNode(m_tnMySelectedNode, m_tnMySelectedNode.Tag.ToString(), out m_err))
                            {
                                e.CancelEdit = true;
                                MessageBox.Show(m_err, "提示");
                                e.Node.BeginEdit();
                            }
                            else
                            {
                                m_tnMySelectedNode.Text = e.Label.ToString();

                                if (m_strStatus == "新增")
                                {
                                    if (!m_serverDepotTypeForPersonnel.AddBill(CreateLinqDepot(m_tnMySelectedNode), out m_err))
                                    {
                                        e.CancelEdit = true;
                                        MessageDialog.ShowErrorMessage(m_err);
                                        e.Node.BeginEdit();
                                    }
                                    else
                                    {
                                        MessageBox.Show("类型添加成功！", "提示");
                                    }
                                }
                                else if (m_strStatus == "修改")
                                {
                                    if (!m_serverDepotTypeForPersonnel.FindMessgeForDtp(code, out m_err))
                                    {
                                        if (!m_serverDepotTypeForPersonnel.UpdateBill(CreateLinqDepot(m_tnMySelectedNode), code, out m_err))
                                        {
                                            e.CancelEdit = true;
                                            MessageDialog.ShowErrorMessage(m_err);
                                            e.Node.BeginEdit();
                                        }
                                        else
                                        {
                                            MessageBox.Show("类型修改成功！", "提示");
                                            e.Node.EndEdit(false);
                                        }
                                    }
                                    else
                                    {
                                        e.CancelEdit = true;
                                        MessageBox.Show("此记录存在关系表不能修改，如要修改请更改关系表"+m_err, "提示");
                                        e.Node.BeginEdit();
                                    }
                                }
                            }

                            m_strStatus = "默认";
                            m_blAdd = false;
                        }
                    }
                    else
                    {
                        e.CancelEdit = true;
                        MessageBox.Show("Invalid tree node label.\n" +
                           "The invalid characters are: '@','.', ',', '!'",
                           "Node Label Edit");
                        e.Node.BeginEdit();
                    }
                }
                else
                {
                    e.CancelEdit = true;
                    MessageBox.Show("名称不能为空",
                       "提示");
                    e.Node.BeginEdit();
                }

                this.treeView1.LabelEdit = false;
            }
        }

        /// <summary>
        /// treeview控件的check事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action != TreeViewAction.Unknown)
            {
                SetNodeCheckStatus(e.Node, e.Node.Checked);
                SetNodeStyle(e.Node);
            }
        }

        #endregion

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
