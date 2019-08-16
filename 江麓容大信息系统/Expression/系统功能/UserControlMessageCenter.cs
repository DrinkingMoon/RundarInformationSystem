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
using System.Threading;

namespace Expression
{
    /// <summary>
    /// 消息中心
    /// </summary>
    public partial class UserControlMessageCenter : Form
    {
        object locker = new object();

        Thread thUnsettledBill;

        /// <summary>
        /// 节点类型
        /// </summary>
        enum NodeType { 未知节点, 预警消息节点, 通知类节点, 待处理单据节点, 已处理单据节点, 单据处理后知会, 任务管理节点, 会议提醒节点 }

        /// <summary>
        /// 初始化模式
        /// </summary>
        enum InitMode { 刷新树节点, 刷新数据显示 };

        /// <summary>
        /// 当前节点类型
        /// </summary>
        NodeType m_curNodeType = NodeType.未知节点;

        /// <summary>
        /// 获取通知类消息数据库操作接口
        /// </summary>
        IFlowNoticeManagement m_flowNotice = PlatformFactory.GetObject<IFlowNoticeManagement>();

        /// <summary>
        /// 单据流消息数据库操作接口
        /// </summary>
        IBillFlowMessage m_billMsg = PlatformFactory.GetObject<IBillFlowMessage>();

        /// <summary>
        /// 任务管理消息操作接口
        /// </summary>
        ITaskNotice m_taskNotice = PlatformFactory.GetObject<ITaskNotice>();

        /// <summary>
        /// 当前树节点
        /// </summary>
        TreeNode m_currentNode;

        /// <summary>
        /// 用户管理数据库操作接口
        /// </summary>
        IUserManagement m_user = PlatformFactory.GetObject<IUserManagement>();

        /// <summary>
        /// 窗体消息发布器
        /// </summary>
        WndMsgSender m_wndMsgSender = new WndMsgSender();

        /// <summary>
        /// 新消息列表
        /// </summary>
        List<Flow_BillFlowMessage> m_lstMessage = new List<Flow_BillFlowMessage>();

        /// <summary>
        /// 构造函数
        /// </summary>
        public UserControlMessageCenter()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public UserControlMessageCenter(List<Flow_BillFlowMessage> lstMessage)
        {
            InitializeComponent();

            m_lstMessage = lstMessage;
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControlMessageCenter_Load(object sender, EventArgs e)
        {
            this.dataGridView1.CellEnter -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);

            treeView.ExpandAll();

            treeView.SelectedNode = treeView.Nodes.Find("任务_待处理_单据", true)[0];//treeView.Nodes["通知"].Nodes["日常事务"];

            this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);

            InitTreeNode();
        }

        /// <summary>
        /// 重新窗体消息处理函数
        /// </summary>
        /// <param name="m">窗体消息</param>
        protected override void DefWndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WndMsgSender.PositioningMsg:
                    ////this.Activate();

                    WndMsgData msg = new WndMsgData(); //这是创建自定义信息的结构

                    Type dataType = msg.GetType();
                    msg = (WndMsgData)m.GetLParam(dataType);//这里获取的就是作为LParam参数发送来的信息的结构
                    PositioningMessage(msg);
                    break;

                default:
                    base.DefWndProc(ref m);
                    break;
            }
        }

        /// <summary>
        /// 定位消息
        /// </summary>
        /// <param name="msg"></param>
        private void PositioningMessage(WndMsgData msg)
        {
            long msgId = Convert.ToInt64(msg.MessageContent);

            if (msg.MessageType == MessageTypeEnum.单据消息)
            {
                treeView.SelectedNode = treeView.Nodes.Find("任务_待处理_单据", true)[0];
            }
            else if (msg.MessageType == MessageTypeEnum.知会消息)
            {
                if (msg.NoticeSource == NoticeSource.单据处理后知会)
                {
                    treeView.SelectedNode = treeView.Nodes.Find("通知_单据处理后知会", true)[0];
                }
                else if (msg.NoticeSource == NoticeSource.日常事务)
                {
                    treeView.SelectedNode = treeView.Nodes.Find("通知_日常事务", true)[0];
                }
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

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if ((long)dataGridView1.Rows[i].Cells["序号"].Value == msgId)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                    break;
                }
            }

            if (msg.MessageType == MessageTypeEnum.单据消息 || (msg.MessageType == MessageTypeEnum.知会消息
                && msg.NoticeSource == NoticeSource.单据处理后知会))
                定位单据ToolStripMenuItem_Click(null, null);
        }

        /// <summary>
        /// 选择树节点后触发的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                m_currentNode = e.Node;

                if (e.Node.Nodes.Count == 0)
                {
                    lblSelectedNode.Text = e.Node.Parent.Text + e.Node.Text;
                }

                InitForm(InitMode.刷新数据显示, e.Node);
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// 清除窗体内容
        /// </summary>
        private void ClearForm()
        {
            dataGridView1.DataSource = null;
        }

        /// <summary>
        /// 初始化树节点（更新每种消息的数量显示）
        /// </summary>
        private void InitTreeNode()
        {
            foreach (TreeNode node in treeView.Nodes)
            {
                InitTreeNode(node);
            }
        }

        private void InitTreeNode(TreeNode node)
        {
            if (node.Nodes.Count == 0)
            {
                return;
                //InitForm(InitMode.刷新树节点, node);
            }
            else
            {
                foreach (TreeNode item in node.Nodes)
                {
                    InitTreeNode(item);
                }
            }
        }

        /// <summary>
        /// 初始化界面
        /// </summary>
        /// <param name="mode">初始化模式</param>
        /// <param name="node">树节点</param>
        private void InitForm(InitMode mode, TreeNode node)
        {
            if (m_flowNotice == null)
            {
                m_flowNotice = PlatformFactory.GetObject<IFlowNoticeManagement>();
                m_billMsg = PlatformFactory.GetObject<IBillFlowMessage>();
            }

            if (mode == InitMode.刷新数据显示)
            {
                lblTitle.Text = "";
                txtContent.Text = "";
                contextMenuStrip1.Enabled = false;
            }

            switch (node.Name)
            {
                case "通知_预警消息":
                    InitWarningNotice(mode);
                    break;
                case "通知_会议提醒":
                    InitMeetingNotice(mode);
                    break;
                case "通知_日常事务":
                    InitDailyAffair(mode);
                    break;
                case "通知_单据处理后知会":
                    InitBillNoticeMessage(mode);

                    if (mode == InitMode.刷新数据显示)
                    {
                        contextMenuStrip1.Enabled = true;
                    }
                    break;
                case "任务_待处理_单据":
                    ThreadPool.QueueUserWorkItem((WaitCallback)delegate
                    {
                        lock (locker)
                        {
                            InitUnsettledBill(mode);
                        }
                    });

                    if (mode == InitMode.刷新数据显示)
                    {
                        contextMenuStrip1.Enabled = true;
                    }
                    break;
                case "任务_已处理_单据":
                    InitFinishedBill(mode);

                    if (mode == InitMode.刷新数据显示)
                    {
                        contextMenuStrip1.Enabled = true;
                    }

                    break;
                case "任务_待处理_任务管理":
                    InitTaskMsg(mode);
                    break;
                default:
                    if (mode == InitMode.刷新数据显示)
                    {
                        ClearForm();
                    }
                    break;
            }

            if (mode == InitMode.刷新数据显示)
            {
                dataGridView1.Refresh();

                if (dataGridView1.Columns.Contains("序号"))
                    dataGridView1.Columns["序号"].Visible = false;

                treeView.Focus();
            }

            userControlDataLocalizer1.Init(dataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
        }

        /// <summary>
        /// 处理通知类消息
        /// </summary>
        /// <param name="mode">初始化模式</param>
        /// <param name="source">消息源</param>
        private void InitNodiceMessage(InitMode mode, NoticeSource source)
        {
            List<Flow_Notice> dataSource =
                (from r in m_flowNotice.GetNotice(BasicInfo.LoginID, source)
                 orderby r.标题, r.发送时间, r.发送人
                 select r).ToList();

            if (source == NoticeSource.日常事务)
            {
                treeView.Nodes.Find("通知_日常事务", true)[0].Text = string.Format("日常事务({0})", dataSource.Count);
            }
            else if (source == NoticeSource.单据处理后知会)
            {
                treeView.Nodes.Find("通知_单据处理后知会", true)[0].Text = string.Format("单据处理后知会({0})", dataSource.Count);
            }

            if (mode == InitMode.刷新数据显示)
            {
                dataGridView1.DataSource = new BindingCollection<Flow_Notice>(dataSource);

                DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();

                column.Visible = true;
                column.Name = "天数";
                column.HeaderText = "天数";
                column.ValueType = typeof(int);
                column.ReadOnly = true;
                column.Width = 40;

                dataGridView1.Columns[1].Width = 60;
                dataGridView1.Columns[2].Width = 80;
                dataGridView1.Columns[4].Width = 80;
                dataGridView1.Columns[5].Width = 80;

                dataGridView1.Columns.Insert(0, column);

                column = new DataGridViewTextBoxColumn();

                column.Visible = true;
                column.Name = "接收人姓名";
                column.HeaderText = "接收人姓名";
                column.ValueType = typeof(string);
                column.ReadOnly = true;
                dataGridView1.Columns.Insert(5, column);

                dataGridView1.Tag = dataSource;

                dataGridView1.Columns["发送人"].Visible = false;
                dataGridView1.Columns["接收人"].Visible = false;

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Columns.Contains("接收方类型")
                        && dataGridView1.Columns.Contains("接收方姓名")
                        && dataGridView1.Columns.Contains("接收方")
                        && dataGridView1.Columns.Contains("状态"))
                    {
                        dataGridView1.Rows[i].Cells["接收人姓名"].Value =
                            m_user.GetUser(dataGridView1.Rows[i].Cells["接收人"].Value.ToString()).姓名;

                        dataGridView1.Rows[i].Cells["状态"].Value = dataGridView1.Rows[i].Cells["状态"].Value.ToString();
                        dataGridView1.Rows[i].Cells["天数"].Value = (ServerModule.ServerTime.Time -
                                                                    (DateTime)dataGridView1.Rows[i].Cells["发送时间"].Value).Days;
                    }
                }
            }
        }

        /// <summary>
        /// 处理日常事务
        /// </summary>
        /// <param name="mode">初始化模式</param>
        private void InitDailyAffair(InitMode mode)
        {
            if (mode == InitMode.刷新数据显示)
            {
                m_curNodeType = NodeType.通知类节点;
            }

            InitNodiceMessage(mode, NoticeSource.日常事务);
        }

        /// <summary>
        /// 处理单据处理后知会
        /// </summary>
        /// <param name="mode">初始化模式</param>
        private void InitBillNoticeMessage(InitMode mode)
        {
            if (mode == InitMode.刷新数据显示)
            {
                m_curNodeType = NodeType.单据处理后知会;
            }

            InitNodiceMessage(mode, NoticeSource.单据处理后知会);
        }

        /// <summary>
        /// 处理预警消息
        /// </summary>
        /// <param name="mode">初始化模式</param>
        private void InitMeetingNotice(InitMode mode)
        {
            List<Flow_WarningNotice> dataSource = (from r in PlatformFactory.GetObject<IWarningNotice>().GetWarningNotice(BasicInfo.LoginID)
                                                   where r.附加信息1 == "会议管理" && ServerTime.Time < Convert.ToDateTime(r.附加信息3)
                                                   orderby r.附加信息3, r.附加信息4
                                                   select r).ToList();

            treeView.Nodes.Find("通知_会议提醒", true)[0].Text = string.Format("会议提醒({0})", dataSource.Count);

            if (mode == InitMode.刷新数据显示)
            {
                m_curNodeType = NodeType.会议提醒节点;

                dataGridView1.DataSource = new BindingCollection<Flow_WarningNotice>(dataSource);

                DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();

                dataGridView1.Columns[0].Width = 40;
                dataGridView1.Columns[1].Width = 80;
                dataGridView1.Columns[3].Width = 80;
                dataGridView1.Columns[4].Width = 80;

                column.Visible = true;
                column.Name = "接收方姓名";
                column.HeaderText = "接收方姓名";
                column.ValueType = typeof(string);
                column.ReadOnly = true;
                dataGridView1.Columns.Insert(4, column);

                dataGridView1.Tag = dataSource;

                dataGridView1.Columns["接收方"].Visible = false;
                dataGridView1.Columns["接收方类型"].Visible = false;

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewCellCollection cells = dataGridView1.Rows[i].Cells;

                    if (dataGridView1.Columns.Contains("接收方类型")
                        && dataGridView1.Columns.Contains("接收方姓名")
                        && dataGridView1.Columns.Contains("接收方")
                        && dataGridView1.Columns.Contains("状态"))
                    {
                        if (cells["接收方类型"].Value.ToString() == BillFlowMessage_ReceivedUserType.用户.ToString())
                        {
                            cells["接收方姓名"].Value =
                                m_user.GetUser(cells["接收方"].Value.ToString()).姓名;
                        }

                        cells["状态"].Value = cells["状态"].Value.ToString();
                    }
                }
            }
        }

        /// <summary>
        /// 处理预警消息
        /// </summary>
        /// <param name="mode">初始化模式</param>
        private void InitWarningNotice(InitMode mode)
        {
            List<Flow_WarningNotice> dataSource = (from r in PlatformFactory.GetObject<IWarningNotice>().GetWarningNotice(BasicInfo.LoginID)
                                                   where r.附加信息1 != "会议管理"
                                                   orderby r.标题, r.发送时间
                                                   select r).ToList();

            treeView.Nodes.Find("通知_预警消息", true)[0].Text = string.Format("预警消息({0})", dataSource.Count);

            if (mode == InitMode.刷新数据显示)
            {
                m_curNodeType = NodeType.预警消息节点;

                dataGridView1.DataSource = new BindingCollection<Flow_WarningNotice>(dataSource);

                DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();

                column.Visible = true;
                column.Name = "天数";
                column.HeaderText = "天数";
                column.ValueType = typeof(int);
                column.ReadOnly = true;
                column.Width = 40;

                dataGridView1.Columns[0].Width = 40;
                dataGridView1.Columns[1].Width = 80;
                dataGridView1.Columns[3].Width = 80;
                dataGridView1.Columns[4].Width = 80;

                dataGridView1.Columns.Insert(0, column);

                column = new DataGridViewTextBoxColumn();

                column.Visible = true;
                column.Name = "接收方姓名";
                column.HeaderText = "接收方姓名";
                column.ValueType = typeof(string);
                column.ReadOnly = true;
                dataGridView1.Columns.Insert(5, column);

                dataGridView1.Tag = dataSource;

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewCellCollection cells = dataGridView1.Rows[i].Cells;

                    if (dataGridView1.Columns.Contains("接收方类型")
                        && dataGridView1.Columns.Contains("接收方姓名")
                        && dataGridView1.Columns.Contains("接收方")
                        && dataGridView1.Columns.Contains("天数")
                        && dataGridView1.Columns.Contains("发送时间")
                        && dataGridView1.Columns.Contains("状态"))
                    {
                        if (cells["接收方类型"].Value.ToString() == BillFlowMessage_ReceivedUserType.用户.ToString())
                        {
                            cells["接收方姓名"].Value =
                                m_user.GetUser(cells["接收方"].Value.ToString()).姓名;
                        }

                        cells["状态"].Value = cells["状态"].Value.ToString();
                        cells["天数"].Value = (ServerModule.ServerTime.Time - (DateTime)cells["发送时间"].Value).Days;
                    }
                }
            }
        }

        /// <summary>
        /// 初始化未处理单据
        /// </summary>
        /// <param name="mode">初始化模式</param>
        private void InitUnsettledBill(InitMode mode)
        {
            IQueryable<Flow_BillFlowMessage> result = m_billMsg.GetReceivedMessage(BasicInfo.LoginID);

            result = from a in result
                     where a.单据状态 != BillStatus.已完成.ToString() && !a.单据状态.Contains("废")
                     select a;

            List<string> lstBillNo = result.Select(k => k.单据号).Distinct().ToList();

            m_lstMessage = (from a in m_lstMessage
                            where lstBillNo.Contains(a.单据号)
                            select a).ToList();

            lstBillNo = m_lstMessage.Select(k => k.单据号).Distinct().ToList();

            List<Flow_BillFlowMessage> dataSource = (from r in result
                                                     where !lstBillNo.Contains(r.单据号)
                                                     orderby r.单据类型 ascending, r.发起时间 ascending
                                                     select r).ToList();

            List<Flow_BillFlowMessage> bindSource = new List<Flow_BillFlowMessage>();

            bindSource.AddRange(m_lstMessage);
            bindSource.AddRange(dataSource);

            treeView.BeginInvoke(new Action(delegate
                {
                    treeView.Nodes.Find("任务_待处理_单据", true)[0].Text = string.Format("单据({0})", bindSource.Count);
                }));

            if (mode == InitMode.刷新数据显示)
            {
                m_curNodeType = NodeType.待处理单据节点;

                dataGridView1.Invoke(new Action(delegate
                {
                    dataGridView1.Columns.Clear();
                    dataGridView1.DataSource = new BindingCollection<Flow_BillFlowMessage>(bindSource);
                    DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();

                    column.Visible = true;
                    column.Name = "天数";
                    column.HeaderText = "天数";
                    column.ValueType = typeof(int);
                    column.ReadOnly = true;
                    column.Width = 40;

                    dataGridView1.Columns["序号"].Width = 60;
                    dataGridView1.Columns["单据类型"].Width = 120;
                    dataGridView1.Columns["单据号"].Width = 110;
                    dataGridView1.Columns["发起方消息"].Width = 450;

                    dataGridView1.Columns.Insert(0, column);

                    dataGridView1.Tag = bindSource;

                    dataGridView1.Columns["单据流水号"].Visible = false;
                    dataGridView1.Columns["发起方用户信息"].Visible = false;
                    dataGridView1.Columns["初始发起方用户编码"].Visible = false;
                    dataGridView1.Columns["发起方用户编码"].Visible = false;
                    dataGridView1.Columns["期望的处理完成时间"].Visible = false;
                    dataGridView1.Columns["接收方类型"].Visible = false;
                    dataGridView1.Columns["单据状态"].Visible = false;
                    dataGridView1.Columns["附加信息1"].Visible = false;
                    dataGridView1.Columns["附加信息2"].Visible = false;
                    dataGridView1.Columns["附加信息3"].Visible = false;
                    dataGridView1.Columns["附加信息4"].Visible = false;
                    dataGridView1.Columns["附加信息5"].Visible = false;
                    dataGridView1.Columns["附加信息6"].Visible = false;
                    dataGridView1.Columns["附加信息7"].Visible = false;
                    dataGridView1.Columns["附加信息8"].Visible = false;
                }));

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    dataGridView1.Rows[i].Cells["天数"].Value = (ServerModule.ServerTime.Time -
                        (DateTime)dataGridView1.Rows[i].Cells["发起时间"].Value).Days;

                    if (m_lstMessage.Where(k => k.单据号 == dataGridView1.Rows[i].Cells["单据号"].Value.ToString()).Count() > 0)
                    {
                        dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.Red;
                    }
                }
            }
        }

        /// <summary>
        /// 初始化已处理单据
        /// </summary>
        /// <param name="mode">初始化模式</param>
        private void InitFinishedBill(InitMode mode)
        {
            IQueryable<Flow_BillFlowMessage> result = m_billMsg.GetMessage(BasicInfo.LoginID);

            List<Flow_BillFlowMessage> dataSource =
                (from r in result
                 where r.单据状态 == BillStatus.已完成.ToString()
                 orderby r.单据类型, r.发起时间
                 select r).ToList();

            treeView.Nodes.Find("任务_已处理_单据", true)[0].Text = string.Format("单据({0})", dataSource.Count);

            if (mode == InitMode.刷新数据显示)
            {
                m_curNodeType = NodeType.已处理单据节点;

                dataGridView1.DataSource = new BindingCollection<Flow_BillFlowMessage>(dataSource);

                dataGridView1.Columns[0].Width = 60;
                dataGridView1.Columns[1].Width = 80;
                dataGridView1.Columns[3].Width = 80;
                dataGridView1.Columns[4].Width = 80;

                dataGridView1.Tag = dataSource;
            }
        }

        /// <summary>
        /// 初始化任务消息
        /// </summary>
        /// <param name="mode">初始化模式</param>
        private void InitTaskMsg(InitMode mode)
        {
            List<View_Flow_TaskNotice> dataSource = m_taskNotice.GetTaskNotice(BasicInfo.LoginID).ToList();

            treeView.Nodes.Find("任务_待处理_任务管理", true)[0].Text = string.Format("任务管理({0})", dataSource.Count());

            if (mode == InitMode.刷新数据显示)
            {
                m_curNodeType = NodeType.任务管理节点;

                dataGridView1.DataSource = new BindingCollection<View_Flow_TaskNotice>(dataSource);

                DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();

                column.Visible = true;
                column.Name = "天数";
                column.HeaderText = "天数";
                column.ValueType = typeof(int);
                column.ReadOnly = true;
                column.Width = 40;

                dataGridView1.Columns.Insert(0, column);

                dataGridView1.Tag = dataSource;

                dataGridView1.Columns["序号"].Visible = false;

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    dataGridView1.Rows[i].Cells["天数"].Value = (ServerModule.ServerTime.Time -
                        (DateTime)dataGridView1.Rows[i].Cells["发送时间"].Value).Days;

                    if ((int)dataGridView1.Rows[i].Cells["天数"].Value > 3)
                    {
                        dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.Red;
                    }
                    else if ((int)dataGridView1.Rows[i].Cells["天数"].Value > 1)
                    {
                        dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.FromArgb(255, 128, 128);
                    }
                }

                dataGridView1.Tag = dataSource;
            }

            if (dataSource.Count > 0)
            {
                contextMenuStrip1.Enabled = true;
            }
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnRefresh_Click(object sender, EventArgs e)
        {
            if (m_currentNode == null)
            {
                MessageDialog.ShowPromptMessage("请选择相应的树节点后再使用此功能！");
                return;
            }

            InitForm(InitMode.刷新数据显示, treeView.SelectedNode);
        }

        /// <summary>
        /// 新建通知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreateNewNotice_Click(object sender, EventArgs e)
        {
            FormUpdateNotice form = new FormUpdateNotice(NoticeSource.日常事务, null);
            form.ShowDialog();

            InitForm(InitMode.刷新数据显示, m_currentNode);
        }

        /// <summary>
        /// 检测是否可以进行此操作
        /// </summary>
        /// <param name="sender">触发事件的控件</param>
        /// <returns>允许操作返回true</returns>
        private bool CheckOperation(ToolStripDropDownItem sender)
        {
            if (m_currentNode.Parent != null && m_currentNode.Parent.Text == "通知")
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    if (sender.Text != "批示已阅" && m_curNodeType == NodeType.预警消息节点)
                    {
                        MessageDialog.ShowPromptMessage("只能操作非预警类通知消息");
                        return false;
                    }

                    return true;
                }
                else
                {
                    MessageDialog.ShowPromptMessage("请选择要操作行后再执行此功能！");
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("只能操作通知类消息！");
            }

            return false;
        }

        /// <summary>
        /// 修改通知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModifyNotice_Click(object sender, EventArgs e)
        {
            if (CheckOperation(sender as ToolStripDropDownItem))
            {
                try
                {
                    Flow_Notice notice = (dataGridView1.Tag as List<Flow_Notice>)[dataGridView1.SelectedRows[0].Index];
                    FormUpdateNotice form = new FormUpdateNotice(NoticeSource.日常事务, notice);

                    form.ShowDialog();

                    InitForm(InitMode.刷新数据显示, m_currentNode);
                }
                catch (Exception err)
                {
                    MessageDialog.ShowErrorMessage(err.Message);
                }
            }
        }

        /// <summary>
        /// 将通知类消息批示已阅
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReadNotice_Click(object sender, EventArgs e)
        {
            if (CheckOperation(sender as ToolStripDropDownItem))
            {
                try
                {
                    for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
                    {
                        if ((dataGridView1.Tag as List<Flow_Notice>) != null)
                        {
                            Flow_Notice notice = (dataGridView1.Tag as List<Flow_Notice>)[dataGridView1.SelectedRows[i].Index];
                            m_flowNotice.ReadNotice(BasicInfo.LoginID, notice.序号);

                            WndMsgData msgData = new WndMsgData();

                            msgData.MessageType = MessageTypeEnum.知会消息;
                            msgData.NoticeSource = (NoticeSource)Enum.Parse(typeof(NoticeSource), notice.来源);
                            msgData.MessageContent = notice.序号.ToString();

                            m_wndMsgSender.SendMessage(StapleInfo.MessagePromptForm.Handle, WndMsgSender.FinishedMsg, msgData);
                        }
                        else if ((dataGridView1.Tag as List<Flow_WarningNotice>) != null)
                        {
                            Flow_WarningNotice notice = (dataGridView1.Tag as List<Flow_WarningNotice>)[dataGridView1.SelectedRows[i].Index];
                            PlatformFactory.GetObject<IWarningNotice>().ReadWarningNotice(BasicInfo.LoginID, notice.序号);
                        }
                    }

                    InitForm(InitMode.刷新数据显示, m_currentNode);
                }
                catch (Exception err)
                {
                    MessageDialog.ShowErrorMessage(err.Message);
                }
            }
        }

        /// <summary>
        /// 删除消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (CheckOperation(sender as ToolStripDropDownItem))
            {
                try
                {
                    for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
                    {
                        Flow_Notice notice = (dataGridView1.Tag as List<Flow_Notice>)[dataGridView1.SelectedRows[i].Index];
                        m_flowNotice.DeleteNotice(BasicInfo.LoginID, notice.序号);
                    }

                    InitForm(InitMode.刷新数据显示, m_currentNode);
                }
                catch (Exception err)
                {
                    MessageDialog.ShowErrorMessage(err.Message);
                }
            }
        }

        /// <summary>
        /// 处理单据类消息内容
        /// </summary>
        /// <param name="e"></param>
        private void DisposeBillMsgContent(DataGridViewCellEventArgs e)
        {
            // 信息提示
            StringBuilder sb = new StringBuilder();
            Dictionary<string, string> dicMsg = new Dictionary<string, string>(dataGridView1.Columns.Count);

            string[] showOrder = { "发起方用户编码", "发起方用户信息", "发起方消息", "发起时间", "接收方" };
            int[] enterCount = { 0, 2, 2, 0, 0 };

            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[i].Value == null)
                    dicMsg.Add(dataGridView1.Columns[i].Name, "");
                else
                    dicMsg.Add(dataGridView1.Columns[i].Name, dataGridView1.Rows[e.RowIndex].Cells[i].Value.ToString());
            }

            int index = 0;

            foreach (var item in showOrder)
            {
                if (enterCount[index] > 0)
                {
                    sb.AppendLine(string.Format("{0}：{1}", item, dicMsg[item]));

                    for (int i = 0; i < enterCount[index] - 1; i++)
                    {
                        sb.AppendLine();
                    }
                }
                else
                {
                    sb.Append(string.Format("{0}：{1}\t", item, dicMsg[item]));
                }

                index++;
            }

            txtContent.Text = sb.ToString();
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }

            if (e != null && (e.RowIndex < 0 || e.ColumnIndex < 0))
            {
                return;
            }

            if (dataGridView1.CurrentRow != null)
            {
                lblRecordRow.Text = (dataGridView1.CurrentRow.Index + 1).ToString();

                if (m_curNodeType == NodeType.通知类节点 || m_curNodeType == NodeType.预警消息节点 ||
                    m_curNodeType == NodeType.会议提醒节点 || m_curNodeType == NodeType.单据处理后知会)
                {
                    lblTitle.Text = dataGridView1.CurrentRow.Cells["标题"].Value.ToString();
                    txtContent.Text = dataGridView1.CurrentRow.Cells["内容"].Value.ToString();
                }
                else if (m_curNodeType == NodeType.待处理单据节点 || m_curNodeType == NodeType.已处理单据节点)
                {
                    lblTitle.Text = dataGridView1.CurrentRow.Cells["单据类型"].Value.ToString() + "    " +
                        dataGridView1.CurrentRow.Cells["单据流水号"].Value.ToString();

                    DisposeBillMsgContent(e);
                }
                else if (m_curNodeType == NodeType.任务管理节点)
                {
                    lblTitle.Text = dataGridView1.CurrentRow.Cells["标题"].Value.ToString();

                    txtContent.Text = string.Format("{0} 于 {1} 给您发来了任务消息：{2}",
                        dataGridView1.CurrentRow.Cells["发送人"].Value.ToString(),
                        dataGridView1.CurrentRow.Cells["发送时间"].Value.ToString(),
                        dataGridView1.CurrentRow.Cells["消息内容"].Value.ToString());
                }
            }
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            lblAmount.Text = dataGridView1.RowCount.ToString();

            //if (m_curNodeType == NodeType.预警消息节点)
            //{
            //    for (int i = 0; i < dataGridView1.Rows.Count; i++)
            //    {
            //        dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.Red;
            //    }
            //}
            //else
            //{
            //    if (dataGridView1.Columns.Contains("天数"))
            //    {
            //        dataGridView1.Columns["单据流水号"].Visible = false;
            //        dataGridView1.Columns["发起方用户信息"].Visible = false;
            //        dataGridView1.Columns["初始发起方用户编码"].Visible = false;
            //        dataGridView1.Columns["发起方用户编码"].Visible = false;
            //        dataGridView1.Columns["期望的处理完成时间"].Visible = false;
            //        dataGridView1.Columns["接收方类型"].Visible = false;

            //        if (m_curNodeType != NodeType.任务管理节点)
            //        {
            //            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            //            {
            //                dataGridView1.Rows[i].Cells["天数"].Value = (ServerModule.ServerTime.Time -
            //                    (DateTime)dataGridView1.Rows[i].Cells["发起时间"].Value).Days;

            //                if (m_lstMessage.Where(k => k.单据号 == dataGridView1.Rows[i].Cells["单据号"].Value.ToString()).Count() > 0)
            //                {
            //                    dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.Red;
            //                }
            //            }
            //        }
            //        else
            //        {
            //            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            //            {
            //                dataGridView1.Rows[i].Cells["天数"].Value = (ServerModule.ServerTime.Time -
            //                    (DateTime)dataGridView1.Rows[i].Cells["发送时间"].Value).Days;
            //            }
            //        }
            //    }
            //}
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            定位单据ToolStripMenuItem_Click(sender, e);
        }

        private void 定位单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }

            if (e != null && e is DataGridViewCellEventArgs)
            {
                if (((DataGridViewCellEventArgs)e).RowIndex < 0 || ((DataGridViewCellEventArgs)e).ColumnIndex < 0)
                {
                    return;
                }
            }

            DataGridViewCellCollection cells = dataGridView1.CurrentRow.Cells;

            if (dataGridView1.Columns.Contains("单据类型"))
            {
                if (dataGridView1.Columns.Contains("附加信息1") && cells["附加信息1"].Value != null && cells["附加信息1"].Value.ToString() != "")
                {
                    List<string> lstData = new List<string>();

                    for (int i = 2; i <= 8; i++)
                    {
                        lstData.Add(cells["附加信息" + i.ToString()].Value.ToString());
                    }

                    ((FormMain)StapleInfo.MainForm).ShowBillForm(cells["附加信息1"].Value.ToString(),
                        cells["附加信息2"].Value.ToString(), lstData);
                }
                else
                {
                    ((FormMain)StapleInfo.MainForm).ShowBillForm(cells["单据类型"].Value.ToString(),
                        cells["单据流水号"].Value.ToString());
                }
            }
            else if (m_curNodeType == NodeType.单据处理后知会)
            {
                if (dataGridView1.Columns.Contains("单据流水号") && cells["单据流水号"].Value != null)
                {
                    if (dataGridView1.Columns.Contains("附加信息1") && cells["附加信息1"].Value != null && cells["附加信息1"].Value.ToString() != "")
                    {
                        List<string> lstData = new List<string>();

                        for (int i = 2; i <= 8; i++)
                        {
                            lstData.Add(cells["附加信息" + i.ToString()].Value.ToString());
                        }

                        ((FormMain)StapleInfo.MainForm).ShowBillForm(cells["附加信息1"].Value.ToString(),
                            cells["附加信息2"].Value.ToString(), lstData);
                    }
                    else
                    {
                        ((FormMain)StapleInfo.MainForm).ShowBillForm(cells["标题"].Value.ToString(),
                            cells["单据流水号"].Value.ToString());

                        #region 双击知会消息则自动批示已阅 2013.1.31

                        btnReadNotice_Click(批示已阅ToolStripMenuItem, e);

                        #endregion 双击知会消息则自动批示已阅
                    }
                }
            }
            else if (m_curNodeType == NodeType.预警消息节点)
            {
                List<string> lstData = new List<string>();

                for (int i = 2; i <= 8; i++)
                {
                    lstData.Add(cells["附加信息" + i.ToString()].Value.ToString());
                }

                ((FormMain)StapleInfo.MainForm).ShowForm(cells["附加信息1"].Value.ToString(), WndMsgSender.WarningNotice, lstData);
            }
            else if (m_curNodeType == NodeType.会议提醒节点)
            {
                List<string> lstData = new List<string>();

                for (int i = 3; i <= 8; i++)
                {
                    lstData.Add(cells["附加信息" + i.ToString()].Value.ToString());
                }

                FormMeetingManagement form = new FormMeetingManagement(cells["附加信息1"].Value.ToString(), lstData);

                form.ShowDialog();
            }
            else if (m_curNodeType == NodeType.任务管理节点)
            {
                string ip = GlobalParameter.DataServerIP.Split(new char[] { '.' })[2];

                System.Diagnostics.Process process = System.Diagnostics.Process.Start("IExplore.exe",
                    string.Format(@"http://192.168.{0}.7?W={1}&P={2}&T={3}", ip, BasicInfo.LoginID, AuthenticationManager.Authentication.EncryptPwd,
                    cells["任务编号"].Value.ToString()));
            }
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }

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
        /// 刷新数据用的定时器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerRefreshData_Tick(object sender, EventArgs e)
        {
            InitTreeNode();

            InitForm(InitMode.刷新数据显示, m_currentNode);
        }

        private void 批示全部ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.SelectAll();

            btnReadNotice_Click(sender, e);
        }

        private void 会议管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormMeetingManagement formMeeting = new FormMeetingManagement();

            formMeeting.Show();
        }
    }
}
