using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using PlatformManagement;
using GlobalObject;
using UniversalControlLibrary;


namespace Expression
{
    /// <summary>
    /// 工装台帐界面
    /// </summary>
    public partial class 工装台帐 : Form
    {
        /// <summary>
        /// 树节点
        /// </summary>
        TreeNode m_trNode;

        /// <summary>
        /// 点击状态
        /// </summary>
        bool m_blIsCheck = true;

        /// <summary>
        /// 查找标志
        /// </summary>
        bool m_blIsFind = false;

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr = "";

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_msgPromulgator = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 服务组件
        /// </summary>
        IFrockStandingBook m_serverFrockStandingBook = PMS_ServerFactory.GetServerModule<IFrockStandingBook>();

        public 工装台帐(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_msgPromulgator.BillType = "工装台帐";

            m_billNoControl = new BillNumberControl(CE_BillTypeEnum.三包外返修处理单, m_serverFrockStandingBook);

            m_authFlag = nodeInfo.Authority;

            RefrshData();

            treeView1.Visible = false;
            treeView1.AllowDrop = false;
            剪切ToolStripMenuItem.Visible = false;
            粘贴ToolStripMenuItem.Visible = false;

            if (BasicInfo.ListRoles.Contains(CE_RoleEnum.工艺人员.ToString()))
            {
                添加ToolStripMenuItem.Visible = true;
                删除ToolStripMenuItem.Visible = true;
            }
            else
            {
                添加ToolStripMenuItem.Visible = false;
                删除ToolStripMenuItem.Visible = false;
            }

            panel2.Dock = DockStyle.Fill;
            panel4.Visible = false;

            DataTable dtTemp = m_serverFrockStandingBook.GetTreeInfo();

            GlobalObject.GeneralFunction.LoadTreeViewDt(treeView1, dtTemp, "FrockName", "FrockNumber", "ParentFrockNumber", "ParentFrockNumber = 'Root'");
        }

        private void 工装台帐_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authFlag);
        }

        /// <summary>
        /// 重新窗体消息处理函数
        /// </summary>
        /// <param name="m">窗体消息</param>
        protected override void DefWndProc(ref Message m)
        {
            switch (m.Msg)
            {
                //接收自定义消息,放弃未提交的单据号

                case WndMsgSender.CloseMsg:
                    // 放弃未使用的单据号
                    m_billNoControl.CancelBill();
                    break;

                case WndMsgSender.PositioningMsg:
                    WndMsgData msg = new WndMsgData();      //这是创建自定义信息的结构
                    Type dataType = msg.GetType();

                    msg = (WndMsgData)m.GetLParam(dataType);//这里获取的就是作为LParam参数发送来的信息的结构

                    DataTable dtMessage = UniversalFunction.PositioningOneRecord(msg.MessageContent, "工装台帐");

                    if (dtMessage.Rows.Count == 0)
                    {
                        MessageDialog.ShowPromptMessage("未找到相关记录");
                    }
                    else
                    {
                        dataGridView1.DataSource = dtMessage;
                        dataGridView1.Rows[0].Selected = true;
                    }

                    break;

                default:
                    base.DefWndProc(ref m);
                    break;
            }
        }

        /// <summary>
        /// 分离字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="frockNumber">工装编号</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool SeparateString(string str, out int goodsID, out string frockNumber)
        {
            goodsID = 0;

            frockNumber = "";

            for (int i = 0; i < str.Length; i++)
            {
                if (str[i].ToString() == "-")
                {
                    goodsID = Convert.ToInt32(str.Substring(0, i));
                    frockNumber = str.Substring(i + 1);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        void RefrshData()
        {
            dataGridView1.DataSource =
                 m_serverFrockStandingBook.GetAllTable(chkIsShowDispensingScrapInfo.Checked, chkIsShowInStock.Checked, 
                 chkIsShowFinalAssembly.Checked, chkIsShowUsing.Checked);

            dataGridView2.DataSource = m_serverFrockStandingBook.GetBookSynthesizeInfo(chkIsShowFinalAssembly.Checked);

            dataGridView2.Columns["工装图号"].Width = 150;
            dataGridView2.Columns["工装名称"].Width = 150;
            dataGridView2.Columns["物品ID"].Visible = false;

            userControlDataLocalizer1.Init(dataGridView1, this.Name, null);
            userControlDataLocalizer2.Init(dataGridView2, this.Name, null);

        }

        /// <summary>
        /// 权限控制
        /// </summary>
        /// <param name="authorityFlag">权限标志</param>
        void AuthorityControl(PlatformManagement.AuthorityFlag authorityFlag)
        {
            FaceAuthoritySetting.SetEnable(this.Controls, authorityFlag);
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="frocknumber">单号</param>
        /// <param name="gooodsid">物品ID</param>
        void PositioningRecord(string frocknumber, int gooodsid)
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
                if ((string)dataGridView1.Rows[i].Cells["工装编号"].Value == frocknumber
                    && (int)dataGridView1.Rows[i].Cells["物品ID"].Value == gooodsid)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                    break;
                }
            }
        }

        #region TreeView拖动

        private void tvFormel_ItemDrag(object sender, ItemDragEventArgs e)
        {
            TreeNode tn = e.Item as TreeNode;
            //根节点不允许拖放操作
            if ((e.Button == MouseButtons.Left) && (tn != null) && (tn.Parent != null))
            {
                this.treeView1.DoDragDrop(tn, DragDropEffects.Copy | DragDropEffects.Move | DragDropEffects.Link);
            }
        }

        private void tvFormel_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode"))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void tvFormel_DragDrop(object sender, DragEventArgs e)
        {
            TreeNode NewNode;

            if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                Point pt = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));

                TreeNode DestinationNode = ((TreeView)sender).GetNodeAt(pt);

                NewNode = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");

                if (DestinationNode != NewNode)
                {
                    int parentGoodsID = 0;
                    int selfGoodsID = 0;

                    string parentFrockNumber = "";
                    string selfFrockNumber = "";

                    S_FrockStandingBook selfFrock = new S_FrockStandingBook();

                    SeparateString(DestinationNode.Tag.ToString(), out parentGoodsID, out parentFrockNumber);
                    SeparateString(NewNode.Tag.ToString(), out selfGoodsID, out selfFrockNumber);

                    selfFrock.GoodsID = selfGoodsID;
                    selfFrock.FrockNumber = selfFrockNumber;

                    if (parentGoodsID != 0 && parentFrockNumber != "")
                    {
                        selfFrock.ParentGoodsID = parentGoodsID;
                        selfFrock.ParentFrockNumber = parentFrockNumber;
                    }

                    if (!m_serverFrockStandingBook.ChangeParentChildRelationships(selfFrock, out m_strErr))
                    {
                        MessageDialog.ShowPromptMessage(m_strErr);
                        return;
                    }
                    else
                    {
                        DestinationNode.Nodes.Add((TreeNode)NewNode.Clone());

                        DestinationNode.Expand();

                        //删除已经移动的节点
                        NewNode.Remove();
                        RefrshData();
                    }
                }
            }
        }

        #endregion

        private void 添加ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            int goodsID = 0;

            string frockNumber = "";

            SeparateString(treeView1.SelectedNode.Tag.ToString(), out goodsID, out frockNumber);

            工装总成信息 form = new 工装总成信息(goodsID, frockNumber, true, m_authFlag, true);
            form.ShowDialog();

            if (form.BlSave)
            {
                TreeNode tn = treeView1.SelectedNode;

                TreeNode tnNew = new TreeNode();

                tnNew.Text = form.StrFrockName;
                tnNew.Tag = form.StrFrockNumber;

                tn.Nodes.Add(tnNew);
                RefrshData();
            }
        }

        private void 查看ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int goodsID = 0;

            string frockNumber = "";

            if (!SeparateString(treeView1.SelectedNode.Tag.ToString(), out goodsID, out frockNumber))
            {
                return;
            }
            else
            {
                工装总成信息 form = new 工装总成信息(goodsID, frockNumber, true, m_authFlag, false);
                form.ShowDialog();
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int goodsID = 0;

            string frockNumber = "";

            SeparateString(treeView1.SelectedNode.Tag.ToString(), out goodsID, out frockNumber);

            if (MessageDialog.ShowEnquiryMessage("您是否要删除此工装信息") == DialogResult.No)
            {
                return;
            }

            if (!m_serverFrockStandingBook.DeleteFrockStandingBook(goodsID, frockNumber, out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
            }
            else
            {
                MessageDialog.ShowPromptMessage("删除成功");
                treeView1.SelectedNode.Remove();
                RefrshData();
            }
        }

        private void chkIsShowStock_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIsShowStock.Checked)
            {
                panel4.Visible = true;
                panel4.Dock = DockStyle.Fill;
                panel2.Visible = false;
                chkIsShowInStock.Visible = false;
                chkIsShowDispensingScrapInfo.Visible = false;
            }
            else
            {
                panel2.Visible = true;
                panel2.Dock = DockStyle.Fill;
                panel4.Visible = false;
                chkIsShowInStock.Visible = true;
                chkIsShowDispensingScrapInfo.Visible = true;
            }
        }

        private void chkIsShowFinalAssembly_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource =
                m_serverFrockStandingBook.GetAllTable(chkIsShowDispensingScrapInfo.Checked, 
                chkIsShowInStock.Checked, chkIsShowFinalAssembly.Checked, chkIsShowUsing.Checked);

            dataGridView2.DataSource = m_serverFrockStandingBook.GetBookSynthesizeInfo(chkIsShowFinalAssembly.Checked);
        }

        private void chkIsShowInStock_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource =
                m_serverFrockStandingBook.GetAllTable(chkIsShowDispensingScrapInfo.Checked, 
                chkIsShowInStock.Checked, chkIsShowFinalAssembly.Checked, chkIsShowUsing.Checked);
        }

        private void chkIsShowDispensingScrapInfo_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource =
                m_serverFrockStandingBook.GetAllTable(chkIsShowDispensingScrapInfo.Checked, 
                chkIsShowInStock.Checked, chkIsShowFinalAssembly.Checked, chkIsShowUsing.Checked);
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {

            int intGoodsID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["物品ID"].Value);

            string strFrockNumber = dataGridView1.CurrentRow.Cells["工装编号"].Value.ToString();

            工装总成信息 form = new 工装总成信息(intGoodsID, strFrockNumber, true, m_authFlag, false);
            form.ShowDialog();
            RefrshData(); 
            PositioningRecord(strFrockNumber, intGoodsID);
        }

        private void dataGridView2_DoubleClick(object sender, EventArgs e)
        {
            工装明细信息 form = new 工装明细信息(Convert.ToInt32(dataGridView2.CurrentRow.Cells["物品ID"].Value), m_authFlag);
            form.ShowDialog();
            RefrshData();
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || treeView1.Nodes.Count == 0)
            {
                return;
            }

            m_blIsFind = false;

            if (treeView1.SelectedNode != null)
            {
                treeView1.SelectedNode.BackColor = treeView1.BackColor;
            }

            GetNodeValues(treeView1, dataGridView1.Rows[e.RowIndex].Cells["物品ID"].Value.ToString()
                + "-" + dataGridView1.Rows[e.RowIndex].Cells["工装编号"].Value.ToString());
        }


        #region 获取叶子节点值
        /// <summary>
        /// 获取被选中的叶子节点的值
        /// </summary>
        /// <param name="trContrl">功能树</param>
        private void GetNodeValues(TreeView trContrl, string tagString)
        {
            foreach (TreeNode tn in trContrl.Nodes)
            {
                if (m_blIsFind == true)
                {
                    return;
                }

                if (tn.Tag.ToString() == tagString)
                {
                    m_blIsFind = true;
                    m_blIsCheck = true;
                    treeView1.SelectedNode = tn;
                    tn.BackColor = Color.Yellow;
                    return;
                }

                if (tn.Nodes.Count != 0)
                {
                    FindNode(tn, tagString);
                }
            }
        }

        /// <summary>
        /// 查询递归
        /// </summary>
        /// <param name="node">节点</param>
        private void FindNode(TreeNode node, string tagString)
        {
            foreach (TreeNode tn in node.Nodes)
            {
                if (m_blIsFind == true)
                {
                    return;
                }

                if (tn.Tag.ToString() == tagString)
                {
                    m_blIsFind = true;
                    m_blIsCheck = true;
                    treeView1.SelectedNode = tn;
                    tn.BackColor = Color.Yellow;
                    return;
                }

                if (tn.Nodes.Count != 0)
                {
                    FindNode(tn, tagString);
                }
            }
        }
        #endregion

        private void treeView1_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                treeView1.SelectedNode.BackColor = treeView1.BackColor;
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (!m_blIsCheck && dataGridView1.Rows.Count > 0)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {

                    int goodsID = 0;

                    string frockNumber = "";

                    SeparateString(treeView1.SelectedNode.Tag.ToString(), out goodsID, out frockNumber);

                    string strColName = "";

                    foreach (DataGridViewColumn col in dataGridView1.Columns)
                    {
                        if (col.Visible)
                        {
                            strColName = col.Name;
                            break;
                        }
                    }

                    if (dataGridView1.Rows[i].Cells["物品ID"].Value.ToString() == goodsID.ToString() 
                        && dataGridView1.Rows[i].Cells["工装编号"].Value.ToString() == frockNumber)
                    {

                        dataGridView1.ClearSelection();
                        dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                        break;
                    }
                }
            }

            m_blIsCheck = false;
        }

        private void 剪切ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_trNode = treeView1.SelectedNode;
        }

        private void 粘贴ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode DestinationNode = treeView1.SelectedNode;

            if (DestinationNode != m_trNode)
            {
                int parentGoodsID = 0;
                int selfGoodsID = 0;

                string parentFrockNumber = "";
                string selfFrockNumber = "";

                S_FrockStandingBook selfFrock = new S_FrockStandingBook();

                SeparateString(DestinationNode.Tag.ToString(), out parentGoodsID, out parentFrockNumber);
                SeparateString(m_trNode.Tag.ToString(), out selfGoodsID, out selfFrockNumber);

                selfFrock.GoodsID = selfGoodsID;
                selfFrock.FrockNumber = selfFrockNumber;

                if (parentGoodsID != 0 && parentFrockNumber != "")
                {
                    selfFrock.ParentGoodsID = parentGoodsID;
                    selfFrock.ParentFrockNumber = parentFrockNumber;
                }

                if (!m_serverFrockStandingBook.ChangeParentChildRelationships(selfFrock, out m_strErr))
                {
                    MessageDialog.ShowPromptMessage(m_strErr);
                    return;
                }
                else
                {
                    DestinationNode.Nodes.Add((TreeNode)m_trNode.Clone());

                    DestinationNode.Expand();

                    //删除已经移动的节点
                    m_trNode.Remove();
                    RefrshData();
                }

                m_trNode = null;

            }
        }

        private void btnRefrsh_Click(object sender, EventArgs e)
        {
            RefrshData();

            DataTable dtTemp = m_serverFrockStandingBook.GetTreeInfo();

            GlobalObject.GeneralFunction.LoadTreeViewDt(treeView1, dtTemp, "FrockName", "FrockNumber", "ParentFrockNumber", "ParentFrockNumber = 'Root'");
        }

        private void chkIsShowTree_CheckedChanged(object sender, EventArgs e)
        {
            treeView1.Visible = chkIsShowTree.Checked;
        }

        private void chkIsShowUsing_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource =
                m_serverFrockStandingBook.GetAllTable(chkIsShowDispensingScrapInfo.Checked,
                chkIsShowInStock.Checked, chkIsShowFinalAssembly.Checked, chkIsShowUsing.Checked);
        }

    }
}
