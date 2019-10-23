/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  FormMain.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2009/06/15
 * 开发平台:  vs2005(c#)
 * 用于    :  生产线管理信息系统
 *----------------------------------------------------------------------------
 * 描述 : 系统主界面
 * 其它 :
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2009/07/03 08:02:08 作者: 夏石友 当前版本: V1.00
 *        修改说明: 创建
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using PlatformManagement;
using System.Linq;
using GlobalObject;
using ServerModule;
using Form_Peripheral_HR;
using Form_Peripheral_External;
using Form_Quality_File;
using UniversalControlLibrary;
using Form_Manufacture_WorkShop;
using ProvidersExpression;
using Microsoft.Win32;
using Form_Manufacture_Storage;
using Form_Quality_QC;
using Form_Project_Design;
using Form_Economic_Purchase;
using Form_Economic_Financial;
using Service_TCU_External;
using System.Collections;

namespace Expression
{

    /// <summary>
    /// 系统主界面
    /// </summary>
    public partial class FormMain : Form, IMainForm
    {
        #region variants

        /// <summary>
        /// 功能子窗体
        /// </summary>
        ContainerControl m_childForm;

        /// <summary>
        /// 不可见的功能名称
        /// </summary>
        List<string> m_unvisibleFunctionNames = new List<string>();

        /// <summary>
        /// 已经打开的页面
        /// </summary>
        List<string> m_faTabStripItemList = new List<string>();

        /// <summary>
        /// 新消息列表
        /// </summary>
        List<Flow_BillFlowMessage> m_dataSource = new List<Flow_BillFlowMessage>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 通信组件
        /// </summary>
        CommResponseServer m_commResponseServer;

        /// <summary>
        /// 跑马灯的内容显示操作类
        /// </summary>
        IMarqueeServer m_marquee = ServerModule.ServerModuleFactory.GetServerModule<IMarqueeServer>();

        /// <summary>
        /// Timer控件标志
        /// </summary>
        //bool iconFlag = false;

        /// <summary>
        /// 窗体显示标志
        /// </summary>
        bool showFlag = true;

        #region 消息处理
        /// <summary>
        /// 窗体消息发布器
        /// </summary>
        WndMsgSender m_wndMsgSender = new WndMsgSender();

        /// <summary>
        /// 用于闪烁拖盘图标的计数器
        /// </summary>
        //int m_glintIconCount = 0;

        /// <summary>
        /// 托盘图标
        /// </summary>
        Icon m_notifyIcon;

        /// <summary>
        /// 空图标
        /// </summary>
        Icon m_emptyIcon;

        /// <summary>
        /// 消息提示窗体
        /// </summary>
        FormMessagePrompt m_formMsgPrompt;

        /// <summary>
        /// 消息中心
        /// </summary>
        ContainerControl m_messageCenter;

        /// <summary>
        /// 外部发来的消息
        /// </summary>
        Message m_msg;

        /// <summary>
        /// 消息数据
        /// </summary>
        WndMsgData m_msgData;

        /// <summary>
        /// 上一次收到消息的时间，用于避免日常事务消息、单据消息同时发来时消息中心多次定位记录的现象
        /// </summary>
        DateTime m_preMsgTime;
        #endregion

        /// <summary>
        /// 所有角色功能树节点
        /// </summary>
        List<FunctionTreeNodeInfo> m_listRoleFullFunctionTree;

        /// <summary>
        /// 未授权的节点
        /// </summary>
        List<FunctionTreeNodeInfo> m_unauthorizedNodes = new List<FunctionTreeNodeInfo>();

        #endregion variants

        enum CloseFlag
        {
            退出,
            最小化
        }

        CloseFlag m_CloseFlag = CloseFlag.最小化;

        /// <summary>
        /// 登录
        /// </summary>
        void Login()
        {
            FormLoggingIn formLoggingIn = new FormLoggingIn();
            formLoggingIn.ShowDialog();

            if (!formLoggingIn.LoggingFlag)
            {
                ColumnWidthControl.Save();
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                return;
            }
            else
            {
                m_unauthorizedNodes = new List<FunctionTreeNodeInfo>();
                m_listRoleFullFunctionTree = new List<FunctionTreeNodeInfo>();
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public FormMain()
        {
            #region 初始化全局参数

            List<Type> types = new List<Type>();
            types.Add(typeof(SocketCommDefiniens.Socket_UserInfo));
            types.Add(typeof(SocketCommDefiniens.Socket_FetchMaterial));
            types.Add(typeof(SocketCommDefiniens.Socket_FittingAccessoryInfo));
            types.Add(typeof(SocketCommDefiniens.Socket_FittingAccessoryInfoSum));
            types.Add(typeof(SocketCommDefiniens.Socket_StateInfo));
            types.Add(typeof(SocketCommDefiniens.Sock_ChoseMatchInfo));
            types.Add(typeof(SocketCommDefiniens.Socket_WorkBenchInfo));
            types.Add(typeof(SocketCommDefiniens.WorkbenchPartInfo));
            types.Add(typeof(SocketCommDefiniens.ProductInfo));

            GlobalObject.GlobalParameter.SerializerTypes = types;

            #endregion 初始化全局参数

            Login();
            InitializeComponent();

            SystemEvents.SessionEnded += new SessionEndedEventHandler(SystemEvents_SessionEnded);
            SystemEvents.SessionSwitch += new SessionSwitchEventHandler(SystemEvents_SessionSwitch);
        }

        void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            PlatformManagement.PlatformFactory.GetUserManagement().UpdateOnlineTime(BasicInfo.LoginID, OnlineStatus.Logout);
        }

        void SystemEvents_SessionEnded(object sender, SessionEndedEventArgs e)
        {
            PlatformManagement.PlatformFactory.GetUserManagement().UpdateOnlineTime(BasicInfo.LoginID, OnlineStatus.Logout);
        }

        /// <summary>
        /// 显示即时提示信息
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        void ShowMessageForm(DateTime startTime, DateTime endTime)
        {
            PlatformManagement.IBillFlowMessage server = PlatformManagement.PlatformFactory.GetObject<IBillFlowMessage>();
            IQueryable<Flow_BillFlowMessage> result = server.GetReceivedMessage(BasicInfo.LoginID);

            m_dataSource = (from r in result
                            where r.单据状态 != BillStatus.已完成.ToString() && !r.单据状态.Contains("废")
                            && r.发起时间 >= startTime && r.发起时间 <= endTime
                            orderby r.单据类型 ascending, r.发起时间 ascending
                            select r).ToList();

            if (m_dataSource != null && m_dataSource.Count > 0)
            {
                FormShowMessage frm = new FormShowMessage(m_dataSource.Count, this);
                frm.Show();
            }
        }

        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMain_Load(object sender, EventArgs e)
        {
            this.Text = GlobalParameter.SystemName + "信息化系统";

            PlatformManagement.PlatformFactory.GetUserManagement().UpdateOnlineTime(BasicInfo.LoginID, OnlineStatus.Login);
            View_Auth_User tempLnq = PlatformFactory.GetUserManagement().GetUser(BasicInfo.LoginID);
            ShowMessageForm(tempLnq.下线时间 == null ? ServerTime.Time : (DateTime)tempLnq.下线时间, ServerTime.Time);

            IQueryable<Auth_FunTreeSystem> trees = PlatformFactory.GetObject<IFunctionTreeSystem>().GetFunTreeSystem();

            if (trees == null)
            {
                MessageDialog.ShowPromptMessage("没有功能树信息");
                this.Close();
                return;
            }

            Color windowsColor = new Color();

            switch (GlobalParameter.SystemName)
            {
                case CE_SystemName.湖南容大:
                    windowsColor = SystemColors.Window;
                    break;
                case CE_SystemName.泸州容大:
                    windowsColor = SystemColors.GradientInactiveCaption;
                    break;
                default:
                    break;
            }

            treeView1.BackColor = windowsColor;
            menuStrip1.BackColor = windowsColor;
            lbShow.BackColor = windowsColor;

            lbShow.Text = string.Format("{3}信息化系统  登录人(工号：{0} 姓名：{1} 部门：{2})", 
                BasicInfo.LoginID, BasicInfo.LoginName, BasicInfo.DeptName, GlobalParameter.SystemName);
            BuildRoleFunctionTree(treeView1);

            if (GlobalParameter.OnlyShowAuthorizedNodes)
            {
                treeView1.ExpandAll();
            }
            else
            {
                treeView1.Nodes[0].Expand();

                foreach (TreeNode item in treeView1.Nodes[0].Nodes)
                {
                    if (item.Text != "生产信息化")
                    {
                        item.Expand();
                    }
                }
            }

            if (treeView1.Nodes.Count > 0)
            {
                treeView1.SelectedNode = treeView1.Nodes[0];
            }

            ShowMessageCenter(true);

            功能树显示控制FToolStripMenuItem.Checked = true;
            只显示有权限的功能树ToolStripMenuItem.Checked = GlobalParameter.OnlyShowAuthorizedNodes;
            panelTree.Visible = true;

            m_notifyIcon = notifyIcon1.Icon;

            // 重登录后需要处理先前留下的窗体
            if (m_formMsgPrompt != null)
            {
                m_formMsgPrompt.ExitApp = true;
                m_formMsgPrompt.Close();
            }

            m_formMsgPrompt = new FormMessagePrompt();
            m_formMsgPrompt.Silent = true;

            StapleInfo.MainForm = this;
            StapleInfo.MessagePromptForm = m_formMsgPrompt;

            m_emptyIcon = UniversalControlLibrary.Properties.Resources.LogoT;

            notifyIcon1.Visible = true;

            m_commResponseServer = new CommResponseServer();

            //显示消息窗体ToolStripMenuItem_Click(sender, e);
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
                    m_msg = m;
                    this.Close();
                    break;

                case WndMsgSender.NewFlowMsg:
                    m_msg = m;

                    if (!m_formMsgPrompt.Visible)
                        timerGlint.Enabled = false;

                    break;

                case WndMsgSender.PositioningMsg:
                    if (!静默待处理消息提示框ToolStripMenuItem.Checked)
                    {
                        m_msg = m;
                        DisposeMsg();
                    }
                    else
                    {
                        timerGlint.Enabled = false;
                    }

                    break;

                default:
                    base.DefWndProc(ref m);
                    break;
            }
        }

        /// <summary>
        /// 处理消息
        /// </summary>
        private void DisposeMsg()
        {
            TimeSpan span = ServerTime.Time - m_preMsgTime;

            if (span.Minutes * 60000 + span.Seconds * 1000 + span.Milliseconds < 300)
            {
                return;
            }

            m_preMsgTime = ServerTime.Time;

            Type dataType = m_msgData.GetType();
            m_msgData = (WndMsgData)m_msg.GetLParam(dataType);

            if (this.WindowState == FormWindowState.Minimized || m_msg.Msg == WndMsgSender.PositioningMsg)
            {
                if (m_msgData.MessageType == MessageTypeEnum.知会消息)
                {
                    ShowMessageCenter(true);
                }
            }

            if (this.WindowState != FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Maximized;
            }

            this.Activate();

            switch (m_msg.Msg)
            {
                case WndMsgSender.NewFlowMsg:
                case WndMsgSender.PositioningMsg:
                    m_wndMsgSender.SendMessage(m_messageCenter.Handle, WndMsgSender.PositioningMsg, m_msgData);
                    break;
            }
        }

        /// <summary>
        /// 构建角色功能树
        /// </summary>
        /// <param name="tree">要构建的树</param>
        private void BuildRoleFunctionTree(TreeView tree)
        {
            // 先构建功能树
            BuildFullTree(tree);
        }

        /// <summary>
        /// 构建全功能树
        /// </summary>
        /// <param name="tree">要构建的树</param>
        private void BuildFullTree(TreeView tree)
        {
            List<FunctionTreeNodeInfo> listRoleFullFunctionTree;
            List<FunctionTreeNodeInfo> copyListRoleFullFunctionTree = new List<FunctionTreeNodeInfo>();

            PlatformFactory.GetObject<IRoleFunctionTree>().GetTree(0, BasicInfo.RoleCodes, out listRoleFullFunctionTree, out m_err);

            m_listRoleFullFunctionTree.Clear();

            for (int i = 0; i < listRoleFullFunctionTree.Count; i++)
            {                
                m_listRoleFullFunctionTree.Add(listRoleFullFunctionTree[i].Clone() as FunctionTreeNodeInfo);
            }

            tree.Nodes.Clear();

            for (int i = 0; i < listRoleFullFunctionTree.Count; i++)
            {
                if (listRoleFullFunctionTree[i].IsLeaf)
                {
                    if (!listRoleFullFunctionTree[i].Authority.ToString().Contains(AuthorityFlag.View.ToString()))
                    {
                        if (GlobalParameter.OnlyShowAuthorizedNodes)
                        {
                            listRoleFullFunctionTree.RemoveAt(i--);
                        }
                        else
                        {
                            m_unauthorizedNodes.Add(listRoleFullFunctionTree[i]);
                        }
                    }
                }
            }

            // 是否存在授权子节点
            bool existAuthorityChildNode = false;

            for (int i = 0; i < listRoleFullFunctionTree.Count; i++)
            {
                if (!listRoleFullFunctionTree[i].IsLeaf && listRoleFullFunctionTree[i].Authority == AuthorityFlag.Nothing)
                {
                    existAuthorityChildNode = false;
                    string nodeCode = listRoleFullFunctionTree[i].NodeCode;

                    for (int j = i + 1; j < listRoleFullFunctionTree.Count; j++)
                    {
                        if (listRoleFullFunctionTree[j].NodeCode.Length > listRoleFullFunctionTree[i].NodeCode.Length &&
                            listRoleFullFunctionTree[j].NodeCode.Substring(0, nodeCode.Length) == nodeCode)
                        {
                            if (listRoleFullFunctionTree[j].Authority != AuthorityFlag.Nothing)
                            {
                                existAuthorityChildNode = true;
                                break;
                            }
                        }
                    }

                    if (!existAuthorityChildNode)
                    {
                        if (GlobalParameter.OnlyShowAuthorizedNodes)
                        {
                            listRoleFullFunctionTree.RemoveAt(i--);
                        }
                        else
                        {
                            m_unauthorizedNodes.Add(listRoleFullFunctionTree[i]);
                        }
                    }
                    else
                    {
                        copyListRoleFullFunctionTree.Add(listRoleFullFunctionTree[i].Clone() as FunctionTreeNodeInfo);
                    }
                }
                else
                {
                    copyListRoleFullFunctionTree.Add(listRoleFullFunctionTree[i].Clone() as FunctionTreeNodeInfo);
                }
            }

            BasicInfo.ListRoleFullFunctionTree = copyListRoleFullFunctionTree;

            GenerateTree(tree, listRoleFullFunctionTree);
        }

        /// <summary>
        /// 通过功能树节点信息生成树形图
        /// </summary>
        /// <param name="funTreeNodes">功能树节点集</param>
        private void GenerateTree(TreeView tree, List<FunctionTreeNodeInfo> funTreeNodes)
        {
            if (funTreeNodes.Count == 0)
            {
                return;
            }

            // 添加树的根节点
            FunctionTreeNodeInfo ftNodeInfo = funTreeNodes[0];
            TreeNode prevNode = GenerateTreeNode(ftNodeInfo);

            tree.Nodes.Add(prevNode);
            funTreeNodes.RemoveAt(0);

            while (funTreeNodes.Count > 0)
            {
                if (!GlobalObject.BasicInfo.ListRoles.Contains(CE_RoleEnum.业务系统管理员.ToString()))
                {
                    if (funTreeNodes[0].Name != "部门管理" && funTreeNodes[0].Name != "人员管理")
                    {
                        TreeNode curNode = GenerateTreeNode(funTreeNodes[0]);
                        TreeNodeCollection nodes = GetParentNodeCollection(tree, prevNode, curNode);

                        nodes.Add(curNode);
                        prevNode = curNode;
                    }
                }
                else
                {
                    TreeNode curNode = GenerateTreeNode(funTreeNodes[0]);
                    TreeNodeCollection nodes = GetParentNodeCollection(tree, prevNode, curNode);

                    nodes.Add(curNode);
                    prevNode = curNode;
                }

                funTreeNodes.RemoveAt(0);
            }
        }

        /// <summary>
        /// 根据节点信息创建树节点
        /// </summary>
        /// <param name="nodeInfo">树节点信息</param>
        /// <returns>返回创建的树节点</returns>
        private TreeNode GenerateTreeNode(FunctionTreeNodeInfo nodeInfo)
        {
            TreeNode node = new TreeNode();
            node.Name = nodeInfo.NodeCode;
            node.Text = nodeInfo.Name;
            node.Tag = nodeInfo;
            node.ImageIndex = 1;
            node.SelectedImageIndex = node.ImageIndex;

            return node;
        }

        /// <summary>
        /// 获取当前节点的父节点集合
        /// </summary>
        /// <param name="tree">当前树</param>
        /// <param name="prevNode">最近新增到树中的邻近节点</param>
        /// <param name="node">要获取父节点集合的节点</param>
        /// <returns>返回获取到的父节点集</returns>
        private TreeNodeCollection GetParentNodeCollection(TreeView tree, TreeNode prevNode, TreeNode node)
        {
            if (prevNode.Name.Length < node.Name.Length)
            {
                prevNode.ImageIndex = 0;
                prevNode.SelectedImageIndex = prevNode.ImageIndex;
                return prevNode.Nodes;
            }

            while (prevNode != null)
            {
                if (prevNode.Name.Length == node.Name.Length)
                {
                    break;
                }

                prevNode = prevNode.Parent;
            }

            if (prevNode.Parent == null)
            {
                return tree.Nodes;
            }

            prevNode.Parent.ImageIndex = 0;
            prevNode.Parent.SelectedImageIndex = prevNode.Parent.ImageIndex;
            return prevNode.Parent.Nodes;
        }

        /// <summary>
        /// 查找树节点
        /// </summary>
        /// <param name="tnParent">父节点</param>
        /// <param name="strValue">要查找的文本</param>
        /// <returns>返回查找到的节点, 找不到返回null</returns>
        private TreeNode FindNode(TreeNode tnParent, string strValue)
        {
            if (tnParent == null) return null;

            if (tnParent.Text == strValue)
                return tnParent;

            TreeNode tnRet = null;

            foreach (TreeNode tn in tnParent.Nodes)
            {
                tnRet = FindNode(tn, strValue);
                if (tnRet != null) break;
            }

            return tnRet;
        }

        /// <summary>
        /// 显示消息中心
        /// </summary>
        /// <param name="show">是否显示</param>
        private void ShowMessageCenter(bool show)
        {
            string formName = "消息中心";
            CommControl.FATabStripItem childForm = FindTabControl(formName);

            if (childForm != null)
            {
                childForm.Visible = show;

                for (int i = 0; i < childForm.Controls.Count; i++)
                {
                    childForm.Controls[i].Visible = show;
                }

                if (show)
                {
                    faTabStrip1.SelectedItem = childForm;
                    return;
                }

                if (faTabStrip1.Items.Count > 1)
                {
                    for (int i = 0; i < faTabStrip1.Items.Count; i++)
                    {
                        if (faTabStrip1.Items[i].Name.ToString() != formName)
                        {
                            faTabStrip1.SelectedItem = faTabStrip1.Items[i];
                            return;
                        }
                    }
                }

                return;
            }

            //m_messageCenter = new UserControlMessageCenter();
            m_messageCenter = new UserControlMessageCenter(m_dataSource);
            AddNewTabStripItem(formName, m_messageCenter);
        }

        /// <summary>
        /// 添加新的Tab控件
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="control">添加的控件</param>
        private void AddNewTabStripItem(string title, ContainerControl control)
        {
            CommControl.FATabStripItem newfaTabStripItem = new CommControl.FATabStripItem();
            newfaTabStripItem.Name = title;

            Panel newPanel = new Panel();

            newPanel.Parent = newfaTabStripItem;
            newPanel.Dock = DockStyle.Fill;

            if ((control as Form) != null)
            {
                Form tempForm = control as Form;

                tempForm.FormBorderStyle = FormBorderStyle.None;
                tempForm.TopLevel = false;
                tempForm.Show();
            }

            control.Parent = newPanel;
            control.Dock = DockStyle.Fill;

            faTabStrip1.Items.Add(newfaTabStripItem);

            newfaTabStripItem.Title = newfaTabStripItem.Name;
            newPanel.Visible = true;

            m_faTabStripItemList.Add(newfaTabStripItem.Name);

            faTabStrip1.SelectedItem = newfaTabStripItem;

            newfaTabStripItem.Tag = control;
        }

        /// <summary>
        /// 查找TAB子窗体中是否有指定名称的界面
        /// </summary>
        /// <param name="controlName">子窗体名称</param>
        /// <returns>找到返回子窗体, 否则返回null</returns>
        CommControl.FATabStripItem FindTabControl(string childFormName)
        {
            if (faTabStrip1.Items.Count == 0)
            {
                return null;
            }

            for (int i = 0; i < faTabStrip1.Items.Count; i++)
            {
                if (faTabStrip1.Items[i].Name == childFormName)
                {
                    return faTabStrip1.Items[i];
                }
            }

            return null;
        }

        /// <summary>
        /// 显示单据类窗体
        /// </summary>
        /// <param name="billType">单据类型</param>
        /// <param name="billNo">单据编号</param>
        public void ShowBillForm(string billType, string billNo)
        {
            TreeNode node = FindNode(treeView1.Nodes[0], billType);

            if (node == null)
            {
                return;
            }

            treeView1.SelectedNode = node;

            TreeNodeMouseClickEventArgs args = new TreeNodeMouseClickEventArgs(node, MouseButtons.Left, 2, 0, 0);

            treeView1_NodeMouseDoubleClick(null, args);

            CommControl.FATabStripItem childForm = FindTabControl(billType);

            ContainerControl userControl = childForm.Tag as ContainerControl;

            WndMsgData sendData = new WndMsgData();

            sendData.MessageContent = billNo;
            m_wndMsgSender.SendMessage(userControl.Handle, WndMsgSender.PositioningMsg, sendData);
        }

        /// <summary>
        /// 显示单据类窗体
        /// </summary>
        /// <param name="billType">单据类型</param>
        /// <param name="billNo">单据编号</param>
        /// <param name="faceParams">
        /// 界面接收的参数对象, 只支持简单数据类型、List、全部数据类型为简单类型或列表的结构体或类(列表中的数据为简单类型)
        /// </param>
        public void ShowBillForm(string billType, string billNo, object faceParams)
        {
            TreeNode node = FindNode(treeView1.Nodes[0], billType);

            if (node == null)
            {
                return;
            }

            treeView1.SelectedNode = node;

            TreeNodeMouseClickEventArgs args = new TreeNodeMouseClickEventArgs(node, MouseButtons.Left, 2, 0, 0);

            treeView1_NodeMouseDoubleClick(null, args);

            CommControl.FATabStripItem childForm = FindTabControl(billType);

            ContainerControl userControl = childForm.Tag as ContainerControl;

            WndMsgData sendData = new WndMsgData();

            sendData.MessageContent = billNo;

            if (faceParams != null)
            {
                sendData.ObjectMessage = GeneralFunction.ClassToIntPtr(faceParams, out sendData.BytesOfObjectMessage);
            }
            else
            {
                sendData.ObjectMessage = IntPtr.Zero;
            }

            m_wndMsgSender.SendMessage(userControl.Handle, WndMsgSender.PositioningMsg, sendData);
        }

        /// <summary>
        /// 显示窗体
        /// </summary>
        /// <param name="formName">界面名称(树节点显示的名称)</param>
        /// <param name="msgID">消息编号</param>
        /// <param name="arg">
        /// 传送到界面的参数数据,只支持简单数据类型、List、全部数据类型为简单类型或列表的结构体或类(列表中的数据为简单类型)
        /// </param>
        public void ShowForm(string formName, int msgID, object arg)
        {
            if (msgID < WndMsgSender.CloseMsg)
            {
                MessageDialog.ShowPromptMessage("消息编号不正确，无法执行此操作");
                return;
            }

            TreeNode node = FindNode(treeView1.Nodes[0], formName);

            if (node == null)
            {
                return;
            }

            try
            {
                treeView1.SelectedNode = node;

                TreeNodeMouseClickEventArgs args = new TreeNodeMouseClickEventArgs(node, MouseButtons.Left, 2, 0, 0);

                treeView1_NodeMouseDoubleClick(null, args);

                CommControl.FATabStripItem childForm = FindTabControl(formName);

                ContainerControl userControl = childForm.Tag as ContainerControl;

                WndMsgData sendData = new WndMsgData();

                sendData.ObjectMessage = GeneralFunction.ClassToIntPtr(arg, out sendData.BytesOfObjectMessage);
                sendData.MessageContent = arg.ToString();

                m_wndMsgSender.SendMessage(userControl.Handle, msgID, sendData);
            }
            catch (Exception exce)
            {
                MessageDialog.ShowErrorMessage(exce.Message);
            }
        }

        /// <summary>
        /// 双击左边树结构中任一节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (m_unvisibleFunctionNames.Contains(e.Node.Text))
            {
                MessageDialog.ShowPromptMessage(string.Format("[{0}]功能没有显示界面或您没有查看权限", e.Node.Text));
                return;
            }

            if (m_unauthorizedNodes.Find(p => p.NodeCode == (e.Node.Tag as FunctionTreeNodeInfo).NodeCode) != null)
            {
                MessageDialog.ShowPromptMessage(string.Format("您没有访问【{0}】的权限，如果您业务上需要此功能请与管理员联系",
                    e.Node.Text));
                return;
            }

            if (m_listRoleFullFunctionTree.FindAll(p => p.Name == e.Node.Text).Count > 1)
            {
                MessageDialog.ShowPromptMessage(string.Format("【{0}】功能命名在功能树中出现多个，系统无从选择，请与管理员联系", e.Node.Text));
                return;
            }

            if (e.Node.Text == "生产装配")
            {
                FormAssemblyMain formAssemblyMain = new FormAssemblyMain(e.Node.Tag as FunctionTreeNodeInfo, m_commResponseServer);
                formAssemblyMain.ShowDialog();
                return;
            }
            else if(e.Node.Text == "项目工时表")
            {
                Form_Project_Project.项目工时表 frm = new Form_Project_Project.项目工时表();
                frm.ShowDialog();
                return;
            }

            if (treeView1.SelectedNode != null && treeView1.SelectedNode.Nodes.Count == 0)
            {
                CommControl.FATabStripItem childForm = FindTabControl(e.Node.Text);

                if (childForm != null)
                {
                    faTabStrip1.SelectedItem = childForm;
                    return;
                }

                switch (e.Node.Text)
                {
                    case "供应商管理":
                        m_childForm = new UserControlProvider();
                        break;
                    case "合同信息管理":
                        m_childForm = new UserControlBargainInfo(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "订单信息管理":
                        m_childForm = new UserControlOrderFormInfo(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "新开发供应商管理":
                        m_childForm = new UserControlNewProvider();
                        break;
                    case "合格供应商与零件责任归属管理":
                        m_childForm = new UserControlAccessoryDutyInfoManage(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "客户管理":
                        m_childForm = new UserControlClient();
                        break;
                    case "部门管理":
                        m_childForm = new UserControlDepartment(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "人员管理":
                        m_childForm = new UserControlPersonnel(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "材料类别":
                        m_childForm = new UserControlDepotTypeForPersonnel(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "仓库信息":
                        m_childForm = new UserControlDepot();
                        break;
                    case "明细库存信息":
                        m_childForm = new 仓库_直接入库(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "单位信息":
                        m_childForm = new UserControlUnit(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "报检入库单":
                        m_childForm = new 报检入库单(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "自制件入库单":
                        m_childForm = new 自制件入库单(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "普通入库单":
                        m_childForm = new 产品类普通入库单(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "领料退库单":
                        m_childForm = new 领料退库单(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "领料单":
                        // xsy
                        //DateTime dt = new DateTime(2017, 6, 15);

                        //if (BasicInfo.LoginName == "李殊姝" && ServerTime.Time.Date >= dt)//.ToString().Contains("2017-06-18"))
                        //{
                        //    m_childForm = new 领料单2(e.Node.Tag as FunctionTreeNodeInfo);
                        //}
                        //else
                        {
                            m_childForm = new 领料单(e.Node.Tag as FunctionTreeNodeInfo);
                        }
                        //m_childForm = new 领料单(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "报废单":
                        m_childForm = new 报废单(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "采购退货单":
                        m_childForm = new 采购退货单(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "库存查询":
                        m_childForm = new UserControlPurchaseStore(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "单据打印明细":
                        m_childForm = new 打印单据查询();
                        break;
                    case "基础物品管理":
                        m_childForm = new UserControlPlanCostBill(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "台帐":
                        m_childForm = new 台帐查询();
                        break;
                    case "入库汇总表":
                        m_childForm = new 零部件入库汇总表(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "入库明细表":
                        m_childForm = new 零部件入库明细表(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "领料汇总表":
                        m_childForm = new 零部件领料汇总表(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "领料明细表":
                        m_childForm = new 零部件领料明细表(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "账务查询":
                        m_childForm = new UserControlReceiveSendSaveGatherBill();
                        break;
                    case "零件选配表":
                        m_childForm = new UserControlAccessoryChoseConfect(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "AB类零部件清单":
                        m_childForm = new UserControlGoodsGrade();
                        break;
                    case "条形码管理":
                        m_childForm = new UserControlBarCodeManage(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "设计BOM表":
                        m_childForm = new UserControlBom(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "Bom映射表":
                        m_childForm = new UserControlBomMapping();
                        break;
                    case "Bom附属表":
                        m_childForm = new UserControlAssembly();
                        break;
                    case "装配线用产品类型":
                        m_childForm = new UserControlProductInfo(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "电子档案":
                        m_childForm = new UserControlElectronFile(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "装配BOM":
                        m_childForm = new 装配BOM(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "发票管理":
                        m_childForm = new 发票管理(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "营销入库单":
                        m_childForm = new 营销入库单(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "营销出库单":
                        m_childForm = new 营销出库单(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "营销退货单":
                        m_childForm = new 营销退货单(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "营销退库单":
                        m_childForm = new 营销退库单(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "库房信息":
                        m_childForm = new 库房信息(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "库房调拨单":
                        m_childForm = new 库房调拨单(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "挑选返工返修单":
                        m_childForm = new 挑选返工返修单(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "旧版不合格品隔离处置单":
                        m_childForm = new 不合格品隔离处置单(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "供应质量信息反馈单":
                        m_childForm = new 供应质量信息反馈单(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "生产计划":
                        m_childForm = new 生产计划(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "库房报缺":
                        m_childForm = new 库房报缺查询(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "库房盘点单":
                        m_childForm = new 库房盘点单(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "安全库存":
                        m_childForm = new 安全库存(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "临时电子档案":
                        m_childForm = new 临时电子档案(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "旧版样品确认申请单":
                        m_childForm = new 样品确认单(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "样品耗用单":
                        m_childForm = new 样品耗用单(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "样品库库存查询":
                        m_childForm = new 样品库库存查询(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "装配多批次零件管理":
                        m_childForm = new 装配多批次零件管理(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "技术变更单历史记录":
                        m_childForm = new 技术变更单(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "供应商配额设置":
                        m_childForm = new 供应商配额设置(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "订单核查":
                        m_childForm = new 订单核查(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "营销要货计划":
                        m_childForm = new 营销要货计划(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "采购计划":
                        m_childForm = new 采购计划(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "自动生成入库单":
                        m_childForm = new 自动生成入库单(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "库存物品防锈":
                        m_childForm = new 库存物品防锈(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "产品编码综合查询":
                        m_childForm = new 产品编码综合查询(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "产品进销存表":
                        m_childForm = new 产品进销存表();
                        break;
                    case "Bom表关联零件设置":
                        m_childForm = new Bom表关联零件设置();
                        break;
                    case "重新打印":
                        m_childForm = new 重新打印(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "产品不合格隔离":
                        m_childForm = new 产品不合格隔离(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "单价初始化":
                        m_childForm = new 单价初始化(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "外部库存查询与修改":
                        m_childForm = new 外部库存查询与修改(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "CVT装车信息":
                        m_childForm = new CVT装车信息(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "CVT车客户信息":
                        m_childForm = new CVT车客户信息(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "售后质量信息反馈单":
                        m_childForm = new 售后服务质量反馈单(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "售后服务配件制造申请单":
                        m_childForm = new 售后服务配件制造申请单(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "售后函电处理单":
                        m_childForm = new 售后函电(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "CVT出厂检验记录表":
                        m_childForm = new CVT出厂检验记录表(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "委外报检入库单":
                        m_childForm = new 委外报检入库单(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "产品型号变更单":
                        m_childForm = new 产品型号变更单(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "自制件退货单":
                        m_childForm = new 自制件退货单(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "工装验证报告单":
                        m_childForm = new 工装验证报告单(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "工装台帐":
                        m_childForm = new 工装台帐(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "物料扣货单":
                        m_childForm = new 物料扣货单(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "三包外返修处理单":
                        m_childForm = new 三包外返修处理单(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "自制件工装报检":
                        m_childForm = new 自制件工装报检(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "主机厂挂账汇总表":
                        m_childForm = new 主机厂挂账汇总表(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "主机厂回款汇总表":
                        m_childForm = new 主机厂回款汇总表(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "组织机构":
                        m_childForm = new UserControlHRDepartment(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "岗位管理":
                        m_childForm = new UserControlOperatingPost(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "岗位编制管理":
                        m_childForm = new UserControlNumOfPeople(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "职称/级别管理":
                        m_childForm = new UserControlJobTitle(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "人员简历管理":
                        m_childForm = new UserControlResume(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "人员档案管理":
                        m_childForm = new UserControlPersonnelArchive(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "合同模板管理":
                        m_childForm = new UserControlLaborContractTemplet(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "员工合同管理":
                        m_childForm = new UserControlPersonnelLaborContract(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "人员档案变更历史":
                        m_childForm = new UserControlPersonnelHistory(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "员工合同历史":
                        m_childForm = new UserControlContractHistory(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "部门调动申请单":
                        m_childForm = new UserControlPostChange(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "员工离职申请单":
                        m_childForm = new UserControlDimissionBill(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "出差申请单":
                        m_childForm = new UserControlOnBusiness(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "节假日管理":
                        m_childForm = new UserControlHoliday(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "请假类别管理":
                        m_childForm = new UserControlLeaveType(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "加班申请单":
                        m_childForm = new UserControlOvertimeBill(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "请假申请单":
                        m_childForm = new UserControlLeaveBill(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "员工考勤方案":
                        m_childForm = new UserControlAttendanceScheme(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "排班管理":
                        m_childForm = new UserControlWorkScheduling(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "量检具台帐":
                        m_childForm = new 量检具台帐(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "集体考勤异常设置":
                        m_childForm = new UserControlBatchException(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "考勤机导入人员考勤":
                        m_childForm = new UserControlAttendanceMachine(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "考勤机导入人员考勤历史信息":
                        m_childForm = new UserControlAttendanceSummary(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "质量问题整改处置单":
                        m_childForm = new 质量问题整改处置单(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "物品质保有效期管理":
                        m_childForm = new 物品质保有效期管理(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "考勤异常登记信息":
                        m_childForm = new UserControlTimeException(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "人员考勤流水信息":
                        m_childForm = new UserControlAttendanceDaybook(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "考勤统计":
                        m_childForm = new UserControlAttendanceSummary(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "打卡号与工号映射":
                        m_childForm = new 打卡号与工号映射(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "装配线系统配置管理":
                        m_childForm = new Expression.装配线系统配置管理(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "集成报表":
                        m_childForm = new 集成报表();
                        break;
                    case "点检参数设置":
                        m_childForm = new 点检参数设置(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "二级库房设置":
                        m_childForm = new 二级库房设置();
                        break;
                    case "调运单":
                        m_childForm = new 调运单(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "库存信息":
                        m_childForm = new 库存信息(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "售后配件申请单":
                        m_childForm = new 售后配件申请单(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "下线不合格品放行单":
                        m_childForm = new 下线不合格品放行单(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "下线试验信息管理":
                        m_childForm = new 下线试验信息管理(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "设计BOM历史记录":
                        m_childForm = new 设计BOM历史记录();
                        break;
                    case "工装与工位匹配管理":
                        m_childForm = new 工装与工位匹配管理(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "设备维修安装申请单":
                        m_childForm = new 设备维修安装申请单(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "设计BOM表零件库":
                        m_childForm = new 设计BOM表零件库(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "采购BOM":
                        m_childForm = new 采购BOM(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "最低销售定价":
                        m_childForm = new 最低销售定价(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "销售清单":
                        m_childForm = new 销售清单(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "物流安全库存":
                        m_childForm = new 物流安全库存(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "非产品件检验单":
                        m_childForm = new 非产品件检验单();
                        break;
                    case "一次性物料清单":
                        m_childForm = new 一次性物料清单(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "TCU返修信息管理":
                        m_childForm = new TCU返修信息(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "文件类别":
                        m_childForm = new 文件类别(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "文件结构":
                        m_childForm = new 体系文件结构(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "制度类别":
                        m_childForm = new 制度类别(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "制度文件结构":
                        m_childForm = new 制度文件结构(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "文件审查流程":
                        m_childForm = new 文件审查流程();
                        break;
                    case "文件发布流程":
                        m_childForm = new 文件发布流程();
                        break;
                    case "文件修订废止申请单":
                        m_childForm = new 文件修订废止申请单();
                        break;
                    case "文件发放回收登记表":
                        m_childForm = new 文件发放回收登记表();
                        break;
                    case "文件销毁记录表":
                        m_childForm = new 文件销毁记录表();
                        break;
                    case "销售合同/订单评审":
                        m_childForm = new 销售订单评审(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "挂账单":
                        m_childForm = new 挂账单(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "技术变更处置单":
                        m_childForm = new 技术变更处置单(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "下线返修信息管理":
                        m_childForm = new 下线返修记录管理(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "车间基础信息":
                        m_childForm = new 车间基础信息();
                        break;
                    case "车间库存":
                        m_childForm = new 车间库存();
                        break;
                    case "零星采购申请单":
                        m_childForm = new 零星采购单(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "车间耗用单":
                        m_childForm = new 车间耗用单();
                        break;
                    case "车间调运单":
                        m_childForm = new 车间调运单();
                        break;
                    case "阀块总成检测数据":
                        m_childForm = new 阀块总成检测数据(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "CVT总成检测数据":
                        m_childForm = new CVT总成检测数据(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "车间物料转换单":
                        m_childForm = new 车间物料转换单();
                        break;
                    case "车间流水账":
                        m_childForm = new 车间流水账();
                        break;
                    case "借货单":
                        m_childForm = new 借货单();
                        break;
                    case "还货单":
                        m_childForm = new 还货单();
                        break;
                    case "借贷记录信息":
                        m_childForm = new 借贷记录信息();
                        break;
                    case "零星采购变更处置单":
                        m_childForm = new 零星采购变更处置单(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "外部流水账":
                        m_childForm = new 外部流水账();
                        break;
                    case "供应商档案管理":
                        m_childForm = new 供应商档案管理(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "车间异常信息记录表":
                        m_childForm = new 车间异常信息记录表();
                        break;
                    case "车间箱号查询":
                        m_childForm = new 车间箱号查询();
                        break;
                    case "制度审查流程":
                        m_childForm = new 制度审查流程();
                        break;
                    case "制度发布流程":
                        m_childForm = new 制度发布流程();
                        break;
                    case "制度修订废弃申请流程":
                        m_childForm = new 制度修订废弃申请流程();
                        break;
                    case "制度销毁申请流程":
                        m_childForm = new 制度销毁申请流程();
                        break;
                    case "下线返修扭矩防错设置":
                        m_childForm = new 下线返修扭矩防错设置(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "发料清单":
                        m_childForm = new 发料清单编制主界面();
                        break;
                    case "培训统计":
                        m_childForm = new 培训统计(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "物料录入申请单":
                        m_childForm = new Form_Project_Design.物料录入申请单(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "工具台帐":
                        m_childForm = new 工具台帐();
                        break;
                    case "借贷台帐":
                        m_childForm = new 借贷台帐();
                        break;
                    case "质量数据库":
                        m_childForm = new 质量数据库主界面();
                        break;
                    case "入库申请单":
                        m_childForm = new 入库申请单();
                        break;
                    case "入库单":
                        m_childForm = new 入库单();
                        break;
                    case "到货单":
                        m_childForm = new 到货单();
                        break;
                    case "检验报告":
                        m_childForm = new 检验报告();
                        break;
                    case "判定报告":
                        m_childForm = new 判定报告();
                        break;
                    case "出库申请单":
                        m_childForm = new 出库申请单();
                        break;
                    case "出库单":
                        m_childForm = new 出库单();
                        break;
                    case "财务月结":
                        m_childForm = new 财务月结汇总();
                        break;
                    case "整台份请领单":
                        m_childForm = new 整台份请领单(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "储备人才库":
                        m_childForm = new 储备人才库(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "专家专业人才库":
                        m_childForm = new 专家专业人才信息管理(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "短信功能":
                        m_childForm = new FormShortMsgManagement();
                        break;
                    case "采购结算单":
                        m_childForm = new 采购结算单(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "设计BOM变更单":
                        m_childForm = new 设计BOM变更单();
                        break;
                    case "样品确认申请单":
                        m_childForm = new Form_Project_Project.样品确认申请单();
                        break;
                    case "供应商与零件归属变更单":
                        m_childForm = new 供应商与零件归属变更单();
                        break;
                    case "不合格品隔离处置单":
                        m_childForm = new Form_Quality_QC.不合格品隔离处置单();
                        break;
                    case "车型与TCU软件信息":
                        m_childForm = new Form_Project_Design.TCU_车型与TCU软件信息();
                        break;
                    case "创意提案":
                        m_childForm = new Form_Peripheral_CompanyQuality.创意提案(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "异常单据业务操作":
                        m_childForm = new 异常单据业务操作();
                        break;
                    case "财务基础参数配置界面":
                        m_childForm = new 财务基础参数配置界面();
                        break;
                    case "TCU软件升级":
                        m_childForm = new TCU软件升级();
                        break;
                    case "IT运维申请单":
                        m_childForm = new Form_Peripheral_CompanyQuality.IT运维申请单();
                        break;
                    case "课程基础设置":
                        m_childForm = new Form_Peripheral_HR.课程基础设置();
                        break;
                    case "培训计划申请表":
                        m_childForm = new Form_Peripheral_HR.培训计划申请表();
                        break;
                    case "培训计划":
                        m_childForm = new Form_Peripheral_HR.培训计划();
                        break;
                    case "车间批次管理变更":
                        m_childForm = new Form_Manufacture_WorkShop.车间批次管理变更();
                        break;
                    case "培训计划信息反馈":
                        m_childForm = new Form_Peripheral_HR.培训计划信息反馈();
                        break;
                    case "学习平台":
                        m_childForm = new Form_Peripheral_HR.学习平台();
                        break;
                    case "年度预算申请表":
                        m_childForm = new Form_Economic_Financial.年度预算申请表();
                        break;
                    case "月度预算申请表":
                        m_childForm = new Form_Economic_Financial.月度预算申请表();
                        break;
                    case "MES数据查询":
                        m_childForm = new MES数据查询();
                        break;
                    case "价格清单":
                        m_childForm = new 价格清单();
                        break;
                    case "供应商挂账表":
                        m_childForm = new 供应商挂账表();
                        break;
                    case "供应商应付账款":
                        m_childForm = new 供应商应付账款(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "重点工作":
                        m_childForm = new Form_Peripheral_CompanyQuality.重点工作(e.Node.Tag as FunctionTreeNodeInfo);
                        break;
                    case "重点工作查询":
                        m_childForm = new Form_Peripheral_CompanyQuality.重点工作查询();
                        break;
                    case "纠正预防措施报告":
                        m_childForm = new 纠正预防措施报告();
                        break;
                    case "车间在产":
                        m_childForm = new 车间在产();
                        break;
                    case "生产BOM变更单":
                        m_childForm = new 生产BOM变更单();
                        break;
                    default:
                        m_childForm = null;
                        break;
                }
            }

            if (m_childForm == null || treeView1.SelectedNode.Nodes.Count != 0)
            {
                //MessageDialog.ShowPromptMessage(e.Node.Text + "节点不存在");
                return;
            }

            AddNewTabStripItem(e.Node.Text, m_childForm);

        }

        /// <summary>
        /// 还原faTabStripItem1
        /// </summary>
        void ResetPanelParent()
        {
            m_childForm.Parent = null;
        }

        /// <summary>
        /// 重新登录菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 重新登录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            notifyIcon1.Visible = false;

            PlatformManagement.PlatformFactory.GetUserManagement().UpdateOnlineTime(BasicInfo.LoginID, OnlineStatus.Logout);
            SendCloseMessageToChildForm();
            faTabStrip1.Items.Clear();
            this.Text = "容大信息化系统";

            Login();
            FormMain_Load(sender, e);

            this.Visible = true;
        }

        /// <summary>
        /// 修改密码菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 修改密码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormModificationPassWord form = new FormModificationPassWord();
            form.ShowDialog();
        }

        /// <summary>
        /// 退出系统菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 退出系统ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseForm();
        }

        /// <summary>
        /// 帮助菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageDialog.ShowPromptMessage("此功能暂未实现");

            HelpNavigation.HelpControl = this;
            HelpNavigation.ShowHelp();
        }

        /// <summary>
        /// 关于菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //FormAboutInfo formAboutInfo = new FormAboutInfo();
            //formAboutInfo.Show();
            //formAboutInfo.TopMost = true;
        }

        /// <summary>
        /// 隐藏窗体
        /// </summary>
        void HideForm()
        {
            AnimateChangeStateManager.Animate(this, false);
            showFlag = false;
            this.Hide();
        }

        /// <summary>
        /// 窗体显示
        /// </summary>
        public void ShowForm()
        {
            //显示主窗体
            this.Visible = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            (m_messageCenter as UserControlMessageCenter).btnRefresh_Click(null, null);
            showFlag = true;
            AnimateChangeStateManager.Animate(this, true);
            this.Show();
        }

        /// <summary>
        /// 窗体关闭
        /// </summary>
        void CloseForm()
        {
            if (MessageDialog.ShowEnquiryMessage("在关闭软件之前,请您确认是否已将所编辑的单据全部提交?") == DialogResult.Yes)
            {
                SendCloseMessageToChildForm();

                timerGlint.Enabled = false;

                if (m_commResponseServer != null)
                    m_commResponseServer.CloseServer();

                //设置托盘的提示信息
                this.notifyIcon1.BalloonTipText = "成功退出系统!";
                this.notifyIcon1.BalloonTipTitle = "退出";
                this.notifyIcon1.ShowBalloonTip(1000 * 3);

                PlatformManagement.PlatformFactory.GetUserManagement().UpdateOnlineTime(BasicInfo.LoginID, OnlineStatus.Logout);

                ColumnWidthControl.Save();

                //延迟退出
                Thread.Sleep(1000 * 2);
                //释放托盘图标资源
                this.notifyIcon1.Dispose();
                m_CloseFlag = CloseFlag.退出;
                this.Close();
            }
        }

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && m_CloseFlag == CloseFlag.最小化)
            {
                e.Cancel = true;
                HideForm();
            }
        }

        /// <summary>
        /// 发送关闭消息给各子窗体
        /// </summary>
        private void SendCloseMessageToChildForm()
        {
            WndMsgData sendData = new WndMsgData();

            foreach (CommControl.FATabStripItem item in faTabStrip1.Items)
            {
                ContainerControl userControl = item.Tag as ContainerControl;
                m_wndMsgSender.SendMessage(userControl.Handle, WndMsgSender.CloseMsg, sendData);
            }

            if (!m_formMsgPrompt.IsDisposed)
                m_wndMsgSender.SendMessage(m_formMsgPrompt.Handle, WndMsgSender.CloseMsg, sendData);
        }

        /// <summary>
        /// 设置托盘显示的图标
        /// </summary>
        /// <param name="index">图像列表中图片的索引</param>
        private void setIconImg(int index)
        {
            Image img = this.imageList.Images[index];
            Bitmap b = new Bitmap(img);
            Icon icon = Icon.FromHandle(b.GetHicon());
            this.notifyIcon1.Icon = icon;
        }

        /// <summary>
        /// 关闭闪烁用的定时器
        /// </summary>
        private void CloseGlintTimer()
        {
            timerGlint.Enabled = false;
            notifyIcon1.Icon = m_notifyIcon;
        }

        private void 功能树显示控制FToolStripMenuItem_Click(object sender, EventArgs e)
        {
            功能树显示控制FToolStripMenuItem.Checked = !功能树显示控制FToolStripMenuItem.Checked;
            panelTree.Visible = 功能树显示控制FToolStripMenuItem.Checked;
        }

        private void 设置数据服务器SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormConfigDataServer form = new FormConfigDataServer();
            form.ShowDialog();
        }

        private void 消息中心显示控制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowMessageCenter(消息中心显示控制ToolStripMenuItem.Checked);
        }

        private void menuItemCloseFaTabItem_Click(object sender, EventArgs e)
        {
            CommControl.TabStripItemClosingEventArgs args = new CommControl.TabStripItemClosingEventArgs(faTabStrip1.SelectedItem);
            faTabStrip1_TabStripItemClosing(args);
            faTabStrip1.RemoveTab(faTabStrip1.SelectedItem);
        }

        void newfaTabStripItem_MouseClick(object sender, MouseEventArgs e)
        {
            faTabStrip1.SelectedItem = (sender as CommControl.FATabStripItem);
        }

        private void faTabStrip1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point pt = new Point(e.X, e.Y);

                faTabStrip1.SelectedItem = faTabStrip1.GetTabItemByPoint(pt);
                faTabStrip1.SelectedItem.ContextMenuStrip = this.contextMenuStripTab;
                faTabStrip1.SelectedItem.ContextMenuStrip.Show(faTabStrip1.PointToScreen(pt));
            }
        }

        private void faTabStrip1_MouseLeave(object sender, EventArgs e)
        {
            faTabStrip1.SelectedItem.ContextMenuStrip = null;
        }

        private void faTabStrip1_TabStripItemClosing(CommControl.TabStripItemClosingEventArgs e)
        {
            ContainerControl userControl = e.Item.Tag as ContainerControl;

            WndMsgData sendData = new WndMsgData();
            //sendData.Flag = (IntPtr)100;
            //sendData.Data = "Hello";
            m_wndMsgSender.SendMessage(userControl.Handle, WndMsgSender.CloseMsg, sendData);

            if (e.Item.Name == "消息中心")
            {
                消息中心显示控制ToolStripMenuItem.Checked = false;
                m_messageCenter = null;
            }

            for (int i = 0; i < m_faTabStripItemList.Count; i++)
            {
                if (e.Item.Name == m_faTabStripItemList[i])
                {
                    m_faTabStripItemList.Remove(e.Item.Name);
                    break;
                }
            }
        }

        private void timerGlint_Tick(object sender, EventArgs e)
        {
            ShowMessageForm(ServerTime.Time, ServerTime.Time.AddSeconds( -timerGlint.Interval / 1000));

            //图标闪烁 Modify by cjb on 2014.5.28
            //if (iconFlag)
            //{
            //    this.setIconImg(2);
            //    iconFlag = !iconFlag;
            //}
            //else
            //{
            //    this.setIconImg(3);
            //    iconFlag = !iconFlag;
            //}
        }

        private void notifyIcon1_MouseDown(object sender, MouseEventArgs e)
        {
            //CloseGlintTimer();

            //if (this.WindowState == FormWindowState.Minimized)
            //{
            //    if (m_formMsgPrompt.MsgAmount > 0)
            //    {
            //        显示消息ToolStripMenuItem.PerformClick();
            //    }
            //    else
            //    {
            //        显示主界面ToolStripMenuItem.PerformClick();
            //    }
            //}
            //else
            //{
            //    显示消息ToolStripMenuItem.PerformClick();
            //}
        }

        private void 显示消息窗体ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!m_formMsgPrompt.Visible)
            {
                m_formMsgPrompt.Show(this);
            }
        }

        private void 显示主界面ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowForm();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!showFlag)
            {
                ShowForm();
            }
            else
            {
                HideForm();
            }
        }

        private void menuItemExit_Click(object sender, EventArgs e)
        {
            CloseForm();
        }

        private void 静默待处理消息提示框ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            静默待处理消息提示框ToolStripMenuItem.Checked = !静默待处理消息提示框ToolStripMenuItem.Checked;
            m_formMsgPrompt.Silent = 静默待处理消息提示框ToolStripMenuItem.Checked;
        }

        private void notifyIcon1_MouseMove(object sender, MouseEventArgs e)
        {
            //if (m_formMsgPrompt.MsgAmount == 0)
            //{
            //    notifyIcon1.Text = "湖南容大信息化系统";
            //}
            //else
            //{
            //    notifyIcon1.Text = "您有待处理信息";
            //}
        }

        private void faTabStrip1_TabStripItemSelectionChanged(CommControl.TabStripItemChangedEventArgs e)
        {
            if (e != null && e.Item != null)
                HelpNavigation.Keyword = e.Item.Caption;
        }

        private void 登录任务管理系统ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string ip = GlobalParameter.DataServerIP.Split(new char[] { '.' })[2];

            System.Diagnostics.Process process = System.Diagnostics.Process.Start("IExplore.exe",
                string.Format(@"http://192.168.{0}.7?W={1}&P={2}", ip,
                BasicInfo.LoginID,
                AuthenticationManager.Authentication.EncryptPwd));
        }

        private void 只显示有权限的功能树ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            只显示有权限的功能树ToolStripMenuItem.Checked = !只显示有权限的功能树ToolStripMenuItem.Checked;

            GlobalParameter.OnlyShowAuthorizedNodes = 只显示有权限的功能树ToolStripMenuItem.Checked;

            FormMain_Load(sender, e);

            GlobalParameter.Save();
        }

        private void 导出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<DataGridView> listDataGridView = GetExcelDataGridView(faTabStrip1.SelectedItem);

            List<string> listName = new List<string>();

            foreach (DataGridView dgv in listDataGridView)
            {
                if (dgv.Tag == null || dgv.Tag.ToString().Trim().Length == 0)
                {
                    listName.Add(dgv.Name);
                }
                else
                {
                    listName.Add(dgv.Tag.ToString());
                }
            }

            FormDataComboBox frm = new FormDataComboBox(listName, "需要导出的数据集");

            try
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    foreach (DataGridView dgv in listDataGridView)
                    {
                        if ((dgv.Tag != null && dgv.Tag.ToString() == frm.Result) || dgv.Name == frm.Result)
                        {
                            if (frm.ExportType == CE_DataGridViewItemsType.选中项)
                            {
                                DataGridView tempView = GlobalObject.GeneralFunction.CloneDataGridView_Grid(dgv);

                                foreach (DataGridViewRow var in dgv.SelectedRows)
                                {
                                    DataGridViewRow Dtgvr = var.Clone() as DataGridViewRow;
                                    Dtgvr.DefaultCellStyle = var.DefaultCellStyle.Clone();
                                    Dtgvr.Cells.Clear();

                                    foreach (DataGridViewCell cell in var.Cells)
                                    {
                                        DataGridViewCell Dtgvcell = cell.Clone() as DataGridViewCell;
                                        Dtgvcell.Value = cell.Value;
                                        Dtgvr.Cells.Add(Dtgvcell);
                                    }

                                    if (var.Index % 2 == 0)
                                        Dtgvr.DefaultCellStyle.BackColor = tempView.RowsDefaultCellStyle.BackColor;
                                    tempView.Rows.Add(Dtgvr);
                                }

                                ExcelHelperP.DatagridviewToExcel(saveFileDialog1, tempView);
                            }
                            else if (frm.ExportType == CE_DataGridViewItemsType.全部项)
                            {
                                if (dgv is CustomDataGridView)
                                {
                                    DataTable tempTable1 = ((CustomDataGridView)dgv).GetDataSourceToDataTable();

                                    if (tempTable1 == null)
                                    {
                                        return;
                                    }
                                }

                                ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dgv);
                            }
                            #region
                                //DataTable result = new DataTable();
                                //string fullName = dgv.DataSource.GetType().FullName;

                                //if (fullName.Contains("GlobalObject.BindingCollection"))
                                //{
                                //    //string typeName = fullName.Substring(fullName.IndexOf("[") + 2,
                                //    //    fullName.IndexOf(",") - fullName.IndexOf("[") - 2);

                                //    //string str = System.Environment.CurrentDirectory + @"\BasicServerModule.dll";
                                //    //var asm = System.Reflection.Assembly.LoadFile(str);
                                //    //Type sourceType = asm.GetType(typeName, true);

                                //    //object obj = Activator.CreateInstance(sourceType);

                                //    //result = GeneralFunction.ConvertToDataTable<sourceType>(dgv.DataSource as Array);

                                //}
                                //else if (fullName.Contains("System.Data.Linq.DataQuery"))
                                //{
                                //    result = GeneralFunction.ConvertToDataTable(dgv.DataSource as IQueryable);
                                //}
                                //else
                                //{
                                //    result = (DataTable)dgv.DataSource;
                                //}


                                //ExportExcel(result);
                                #endregion
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage("此界面的" + frm.Result + "未实现导出功能 \r\n" + ex.Message);
                return;
            }
        }

        List<DataGridView> GetExcelDataGridView(Control control)
        {
            List<DataGridView> listDataGridView = new List<DataGridView>();

            foreach (Control cl in control.Controls)
            {
                if (cl is DataGridView)
                {
                    //if (((DataGridView)cl).Tag != null && 
                    //    ((DataGridView)cl).Tag.ToString().ToLower() == AuthorityFlag.ExportFile.ToString().ToLower())
                    //{
                    //    listDataGridView.Add((DataGridView)cl);
                    //}
                    listDataGridView.Add((DataGridView)cl);
                }

                if (cl.Controls.Count > 0)
                {
                    listDataGridView.AddRange(GetExcelDataGridView(cl));
                }
            }

            return listDataGridView;
        }
    }
}