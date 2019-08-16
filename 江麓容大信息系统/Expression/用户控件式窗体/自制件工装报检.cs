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
    /// <summary>
    /// 自制件工装报检界面
    /// </summary>
    public partial class 自制件工装报检 : Form
    {
        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr;

        /// <summary>
        /// 查询结果
        /// </summary>
        IQueryResult m_queryResult;

        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 获取单据流消息操作接口
        /// </summary>
        IBillFlowMessage m_billFlowMsg = PlatformFactory.GetObject<IBillFlowMessage>();

        /// <summary>
        /// 自制件工装报检服务组件
        /// </summary>
        IFrockIndepotBill m_billServer = ServerModuleFactory.GetServerModule<IFrockIndepotBill>();

        /// <summary>
        /// 计划成本服务组件
        /// </summary>
        IBasicGoodsServer m_planCostServer = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();

        /// <summary>
        /// 查找条件窗体
        /// </summary>
        FormConditionFind m_formFindCondition;

        /// <summary>
        /// 可供查找的所有字段
        /// </summary>
        string[] m_findField = null;

        /// <summary>
        /// 是否生成工装报检
        /// </summary>
        bool m_blFlag = false;

        /// <summary>
        /// 数据定位控件
        /// </summary>
        UserControlDataLocalizer m_dataLocalizer;

        /// <summary>
        /// 窗体消息发布器
        /// </summary>
        WndMsgSender m_wndMsgSender = new WndMsgSender();

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authorityFlag;

        /// <summary>
        /// 单据号
        /// </summary>
        string m_strBillID;

        public 自制件工装报检(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_billMessageServer.BillType = "自制件工装报检";

            m_billNoControl = new BillNumberControl(labelTitle.Text, m_billServer);// new BillNumberControl(labelTitle.Text, m_billServer);

            string[] strBillStatus = { "全部", "新建单据", "等待工装验证", "等待入库", "已入库", "已报废" };

            checkBillDateAndStatus1.InsertComBox(strBillStatus);

            checkBillDateAndStatus1.OnCompleteSearch +=
                new GlobalObject.DelegateCollection.NonArgumentHandle(checkBillDateAndStatus1_OnCompleteSearch);

            m_authorityFlag = nodeInfo.Authority;

            btnRefresh_Click(null, null);

            提交单据ToolStripMenuItem.Visible = true;
        }

        private void 自制件工装报检_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authorityFlag);
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
                    // 放弃未使用的单据号
                    m_billNoControl.CancelBill();
                    break;
                case WndMsgSender.PositioningMsg:
                    WndMsgData msg = new WndMsgData();   //这是创建自定义信息的结构
                    Type dataType = msg.GetType();

                    msg = (WndMsgData)m.GetLParam(dataType);//这里获取的就是作为LParam参数发送来的信息的结构

                    DataTable dtMessage = UniversalFunction.PositioningOneRecord(msg.MessageContent, "自制件工装报检");

                    if (dtMessage.Rows.Count == 0)
                    {
                        m_billMessageServer.DestroyMessage(msg.MessageContent);
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

        private void 自制件工装报检_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        /// <summary>
        /// 权限控制
        /// </summary>
        /// <param name="authorityFlag">权限标志</param>
        void AuthorityControl(PlatformManagement.AuthorityFlag authorityFlag)
        {
            FaceAuthoritySetting.SetVisibly(menuStrip, authorityFlag);
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            自制件工装报检明细 form = new 自制件工装报检明细(
                CE_BusinessOperateMode.查看, dataGridView1.CurrentRow.Cells[0].Value.ToString());
            form.ShowDialog();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            m_billServer.QueryResultFilter = QueryFilterControl.GetFilterString(labelTitle.Text)
                + checkBillDateAndStatus1.GetSqlString("单据提交时间", "单据状态");

            if (!m_billServer.GetAllBill(out m_queryResult, out m_strErr))
            {
                MessageDialog.ShowErrorMessage(m_strErr);
                return;
            }

            RefreshDataGridView(m_queryResult);
        }

        void checkBillDateAndStatus1_OnCompleteSearch()
        {
            btnRefresh_Click(null, null);
        }

        /// <summary>
        /// 刷新DataGridView
        /// </summary>
        /// <param name="findBill">数据集</param>
        void RefreshDataGridView(IQueryResult findBill)
        {
            dataGridView1.DataSource = findBill.DataCollection.Tables[0];

            // 添加查询用的列
            if (m_findField == null)
            {
                List<string> lstColumnName = new List<string>();

                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    if (dataGridView1.Columns[i].Visible)
                    {
                        lstColumnName.Add(dataGridView1.Columns[i].Name);
                    }
                }

                m_findField = lstColumnName.ToArray();
            }

            if (m_dataLocalizer == null)
            {
                m_dataLocalizer = new UserControlDataLocalizer(dataGridView1, this.Name,
                    UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));

                checkBillDateAndStatus1.Controls.Add(m_dataLocalizer);
                m_dataLocalizer.Dock = DockStyle.Bottom;
            }

            this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            dataGridView1.Columns["库房代码"].Visible = false;
            dataGridView1.Columns["机加人员编码"].Visible = false;
            dataGridView1.Columns["仓管员编码"].Visible = false;
            dataGridView1.Columns["申请人编码"].Visible = false;
            dataGridView1.Columns["设计人编码"].Visible = false;

            dataGridView1.Refresh();
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="billNo">定位用的单据号</param>
        void PositioningRecord(string billNo)
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
                if ((string)dataGridView1.Rows[i].Cells["单据号"].Value == billNo)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                    break;
                }
            }
        }

        #region 流消息

        /// <summary>
        /// 发送完成标志信息到消息提示窗体
        /// </summary>
        /// <param name="billNo">单据编号</param>
        private void SendFinishedFlagToMessagePromptForm(string billNo)
        {
            WndMsgData msgData = new WndMsgData();

            msgData.MessageType = MessageTypeEnum.单据消息;
            msgData.MessageContent = string.Format("{0},{1}", labelTitle.Text, billNo);

            m_wndMsgSender.SendMessage(StapleInfo.MessagePromptForm.Handle, WndMsgSender.FinishedMsg, msgData);
        }

        /// <summary>
        /// 发送新的流消息
        /// </summary>
        /// <param name="billNo">单据编号</param>
        /// <param name="role">消息发送到的角色</param>
        /// <param name="msgContent">消息内容</param>
        void SendNewFlowMessage(string billNo, CE_RoleEnum role, string msgContent)
        {
            Flow_BillFlowMessage msg = new Flow_BillFlowMessage();

            msg.初始发起方用户编码 = BasicInfo.LoginID;
            msg.单据号 = billNo;
            msg.单据类型 = labelTitle.Text;
            msg.单据流水号 = billNo;
            msg.接收方类型 = BillFlowMessage_ReceivedUserType.角色.ToString();
            msg.单据状态 = BillStatus.等待处理.ToString();
            msg.发起方消息 = msgContent;

            msg.接收方 = role.ToString();

            msg.期望的处理完成时间 = null;
            m_billFlowMsg.SendRequestMessage(BasicInfo.LoginID, msg);
        }

        /// <summary>
        /// 传递流消息(走流程)
        /// </summary>
        /// <param name="msgContent">消息内容</param>
        /// <param name="receivedRole">接收方角色</param>
        void PassFlowMessage(string msgContent, CE_RoleEnum receivedRole)
        {
            Flow_BillFlowMessage msg = m_billFlowMsg.GetMessage(BasicInfo.LoginID, labelTitle.Text, m_strBillID);

            if (msg == null)
            {
                return;
            }

            msg.发起方用户编码 = BasicInfo.LoginID;
            msg.发起方消息 = msgContent;
            msg.接收方类型 = BillFlowMessage_ReceivedUserType.角色.ToString();
            msg.接收方 = receivedRole.ToString();

            msg.期望的处理完成时间 = null;
            m_billFlowMsg.ContinueMessage(BasicInfo.LoginID, msg);
            SendFinishedFlagToMessagePromptForm(m_strBillID);
        }

        /// <summary>
        /// 销毁消息
        /// </summary>
        /// <param name="billNo">单据号</param>
        void DestroyMessage(string billNo)
        {
            Flow_BillFlowMessage msg = m_billFlowMsg.GetMessage(BasicInfo.LoginID, labelTitle.Text, billNo);

            if (msg == null)
            {
                return;
            }

            m_billFlowMsg.DestroyMessage(BasicInfo.LoginID, msg.序号);
            SendFinishedFlagToMessagePromptForm(billNo);
        }

        /// <summary>
        /// 结束流消息(流程已经走完)
        /// </summary>
        /// <param name="msgContent">消息内容</param>
        void EndFlowMessage(string msgContent)
        {
            Flow_BillFlowMessage msg = m_billFlowMsg.GetMessage(BasicInfo.LoginID, labelTitle.Text, m_strBillID);

            if (msg == null)
            {
                return;
            }

            m_billFlowMsg.EndMessage(BasicInfo.LoginID, msg.序号, msgContent);
            SendFinishedFlagToMessagePromptForm(m_strBillID);

            #region 发送知会消息

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} 号自制件工装报检已经处理完毕, ", msg.单据流水号);

            List<string> noticeRoles = new List<string>();

            noticeRoles.Add(CE_RoleEnum.工装管理员.ToString());

            List<string> noticeUsers = new List<string>();

            noticeUsers.Add(msg.初始发起方用户编码);

            m_billMessageServer.NotifyMessage(msg.单据类型, msg.单据号, sb.ToString(), BasicInfo.LoginID, noticeRoles, noticeUsers);

            #endregion 发送知会消息
        }

        #endregion 流消息

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            m_strBillID = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            lblBillStatus.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();

            仓库管理员操作ToolStripMenuItem.Visible = 
                UniversalFunction.CheckStorageAndPersonnel(dataGridView1.CurrentRow.Cells["库房代码"].Value.ToString());
        }

        private void 新建单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lblBillStatus.Text = OrdinaryInDepotBillStatus.新建单据.ToString();
            m_strBillID = m_billNoControl.GetNewBillNo();

            自制件工装报检明细 form = new 自制件工装报检明细(CE_BusinessOperateMode.新建, m_strBillID);
            form.ShowDialog();

            this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);

            PositioningRecord(m_strBillID);
            btnRefresh_Click(sender, e);

            m_blFlag = form.cbForck.Checked;
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

        private void 核实物品清单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lblBillStatus.Text.Equals("等待入库"))
            {
                自制件工装报检明细 form = new 自制件工装报检明细(CE_BusinessOperateMode.仓库核实, m_strBillID);
                form.ShowDialog();
            }
            else
            {
                MessageDialog.ShowPromptMessage("此状态下不能进行核实！");
                return;
            }

            RefreshDataGridView(m_queryResult);
            PositioningRecord(m_strBillID);
        }

        private void 查找ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_formFindCondition == null)
            {
                m_formFindCondition = new FormConditionFind(this, m_findField, labelTitle.Text, labelTitle.Text);
            }

            m_formFindCondition.ShowDialog();
        }

        private void 提交单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lblBillStatus.Text != OrdinaryInDepotBillStatus.新建单据.ToString())
            {
                MessageDialog.ShowPromptMessage("您现在不是处于新建单据状态，无法提交");
                return;
            }

            if (!CheckSelectedRow())
            {
                return;
            }

            string billNo = dataGridView1.CurrentRow.Cells[0].Value.ToString();

            if (!m_billServer.IsExist(billNo))
            {
                MessageDialog.ShowPromptMessage("您还未设置物品清单，无法提交");
                return;
            }

            IPersonnelInfoServer server = ServerModuleFactory.GetServerModule<IPersonnelInfoServer>();
            CE_RoleEnum role = CE_RoleEnum.工装管理员;
            string flowMsg = null;

            if (!m_billServer.SubmitNewBill(billNo, m_blFlag, out m_queryResult, out m_strErr))
            {
                MessageDialog.ShowErrorMessage(m_strErr);
                return;
            }

            m_billMessageServer.DestroyMessage(billNo);
            SendNewFlowMessage(billNo, role, flowMsg);
            RefreshDataGridView(m_queryResult);
            PositioningRecord(billNo);

            MessageDialog.ShowPromptMessage("【" + billNo + "】号单据提交成功！");
        }

        private void 物品入库ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
            {
                return;
            }

            if (!lblBillStatus.Text.Equals("等待入库"))
            {
                MessageDialog.ShowPromptMessage("请选择等待入库的记录后再进行此操作！");
                return;
            }

            if (MessageDialog.ShowEnquiryMessage("您确定已经核实好物品清单了吗？") == DialogResult.No)
            {
                return;
            }

            string billNo = m_strBillID;

            S_FrockInDepotBill inDepotInfo = new S_FrockInDepotBill();

            inDepotInfo.Bill_ID = billNo;
            inDepotInfo.DepotManager = BasicInfo.LoginID;
            inDepotInfo.Bill_Status = "已入库";
            inDepotInfo.InDepotDate = ServerTime.Time;

            if (!m_billServer.SubmitInDepotInfo(inDepotInfo, out m_queryResult, out m_strErr))
            {
                MessageDialog.ShowErrorMessage(m_strErr);
                return;
            }

            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("{0} 号单据已经入库, ", billNo);

            EndFlowMessage(sb.ToString());
            RefreshDataGridView(m_queryResult);
            PositioningRecord(billNo);

            MessageDialog.ShowPromptMessage("成功将零件入库!");
        }

        private void 修改单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
                return;

            if (!lblBillStatus.Text.Equals("新建单据"))
            {
                MessageDialog.ShowPromptMessage("您现在不是处于新建单据状态，无法进行此操作");
                return;
            }

            自制件工装报检明细 form = new 自制件工装报检明细(CE_BusinessOperateMode.修改, m_strBillID);
            form.ShowDialog();

            RefreshDataGridView(m_queryResult);
            PositioningRecord(m_strBillID);
        }

        private void 删除单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
                return;

            if (lblBillStatus.Text.Equals("已入库"))
            {
                MessageDialog.ShowPromptMessage("请选择未完成的记录后再进行此操作");
                return;
            }

            string info = string.Format("您是否要删除 {0} 自制件工装报检单据时, 同时也会删除此单据下的所有物品清单, 是否继续？", m_strBillID);

            if (MessageDialog.ShowEnquiryMessage(info) == DialogResult.No)
            {
                return;
            }

            if (!m_billServer.DeleteBill(m_strBillID, out m_queryResult, out m_strErr))
            {
                MessageDialog.ShowErrorMessage(m_strErr);
                return;
            }

            m_billNoControl.CancelBill(m_strBillID);
            DestroyMessage(m_strBillID);
            RefreshDataGridView(m_queryResult);
        }

        private void 打印单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择一条已入库记录后再行此操作");
                return;
            }

            if (dataGridView1.SelectedRows[0].Cells["单据状态"].Value.ToString() != "已入库")
            {
                MessageDialog.ShowPromptMessage("当前单据没有入库不允许进行打印");
                return;
            }

            报表_自制件工装报检单 report = new 报表_自制件工装报检单(dataGridView1.CurrentRow.Cells[0].Value.ToString(), labelTitle.Text, true);
            PrintReportBill print = new PrintReportBill(21.8, 9.31, report);
            print.DirectPrintReport();
        }
    }
}
