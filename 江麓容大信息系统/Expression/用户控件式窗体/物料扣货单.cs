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
    /// 物料扣货单界面
    /// </summary>
    public partial class 物料扣货单 : Form
    {
        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 获取单据流消息操作接口
        /// </summary>
        IBillFlowMessage m_billFlowMsg = PlatformFactory.GetObject<IBillFlowMessage>();

        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 可供查找的所有字段
        /// </summary>
        string[] m_findField = null;

        /// <summary>
        /// 数据定位控件
        /// </summary>
        UserControlDataLocalizer m_dataLocalizer;

        /// <summary>
        /// 窗体消息发布器
        /// </summary>
        WndMsgSender m_wndMsgSender = new WndMsgSender();

        /// <summary>
        /// 物料扣货服务类
        /// </summary>
        IMaterialDetainBill m_billServer = ServerModuleFactory.GetServerModule<IMaterialDetainBill>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr;

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag;

        public 物料扣货单(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_billMessageServer.BillType = "物料扣货单";

            m_billNoControl = new BillNumberControl(labelTitle.Text, m_billServer);

            m_authFlag = nodeInfo.Authority;

            DataTable dt = UniversalFunction.GetStorageTb();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cmbStorage.Items.Add(dt.Rows[i]["StorageName"].ToString());
            }

            cmbStorage.SelectedIndex = -1;

            string[] strBillStatus = { "全部", "等待领导审核", "等待质管批准", "等待SQE确认", "等待采购确认", "单据已完成" };
            checkBillDateAndStatus1.InsertComBox(strBillStatus);

            #region 被要求使用服务器时间 Modify by cjb on 2012.6.15
            checkBillDateAndStatus1.dtpStartTime.Value = ServerTime.Time.AddDays(1).AddMonths(-1);
            checkBillDateAndStatus1.dtpEndTime.Value = ServerTime.Time.AddDays(1);

            #endregion

            menuItemReresh_Click(null, null);
            RefreshDataGridView(m_billServer.GetAllBill());
        }

        private void 物料扣货单_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authFlag);
        }

        /// <summary>
        /// 权限控制
        /// </summary>
        /// <param name="authorityFlag">权限标志</param>
        void AuthorityControl(PlatformManagement.AuthorityFlag authorityFlag)
        {
            FaceAuthoritySetting.SetVisibly(menuStrip, authorityFlag);
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

                    DataTable dtMessage = UniversalFunction.PositioningOneRecord(msg.MessageContent, "物料扣货单");

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

        /// <summary>
        /// 清除窗体上的控件残留信息
        /// </summary>
        void ClearForm()
        {
            lblBillStatus.Text = "";
            cmbStorage.SelectedIndex = -1;
            txtBill_ID.Text = "";
            dateTime_BillTime.Value = ServerModule.ServerTime.Time;
            txtProvider.Text = "";
            txtReason.Text = "";
            txtRemark.Text = "";
            txtProposer.Text = "";
            txtFinanceSignatory.Text = "";
            txtDepotManager.Text = "";
        }

        /// <summary>
        /// 检测建单数据项内容是否正确
        /// </summary>
        /// <returns>返回检测结果</returns>
        bool CheckDataItem()
        {
            if (txtProvider.Text == "")
            {
                txtProvider.Focus();
                MessageDialog.ShowPromptMessage("请选择供应商");
                return false;
            }

            if (txtReason.Text == "")
            {
                txtReason.Focus();
                MessageDialog.ShowPromptMessage("请填写扣货原因!");
                return false;
            }

            if (cmbStorage.Text == "")
            {
                cmbStorage.Focus();
                MessageDialog.ShowPromptMessage("请选择所属库房!");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 查找并刷新数据
        /// </summary>
        private void RefreshData()
        {
            m_billServer.QueryResultFilter = QueryFilterControl.GetFilterString(labelTitle.Text)
                + checkBillDateAndStatus1.GetSqlString("建单时间", "单据状态");
            RefreshDataGridView(m_billServer.GetAllBill());
        }

        /// <summary>
        /// 刷新DataGridView
        /// </summary>
        /// <param name="findBill">数据集</param>
        void RefreshDataGridView(DataTable findBill)
        {
            ClearForm();
            dataGridView1.DataSource = findBill;

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
                panelPara.Controls.Add(m_dataLocalizer);
                m_dataLocalizer.Dock = DockStyle.Bottom;
            }

            dataGridView1.Refresh();
        }

        private void checkBillDateAndStatus1_OnCompleteSearch()
        {
            menuItemReresh_Click(null, null);
        }

        private void menuItemReresh_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void 新建单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                if (dataGridView1.SelectedRows[0].Cells["单据状态"].Value.ToString() != "新建单据")
                {
                    if (lblBillStatus.Text.Trim() == "新建单据")
                    {
                        MessageDialog.ShowPromptMessage("现在已经处于新建单据状态");
                        return;
                    }
                }

                if (BasicInfo.LoginID == (string)dataGridView1.SelectedRows[0].Cells["建单人编号"].Value)
                {
                    if (lblBillStatus.Text.Trim() == "新建单据")
                    {
                        MessageDialog.ShowPromptMessage("现在已经处于新建单据状态");
                        return;
                    }
                }
            }
            else
            {
                if (lblBillStatus.Text.Trim() == "新建单据")
                {
                    MessageDialog.ShowPromptMessage("现在已经处于新建单据状态");
                    return;
                }
            }

            ClearForm();
            txtBill_ID.Text = m_billNoControl.GetNewBillNo();

            dateTime_BillTime.Value = ServerModule.ServerTime.Time;
            txtProposer.Text = BasicInfo.LoginName;
            lblBillStatus.Text = "新建单据";
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (lblBillStatus.Text != "新建单据")
            {
                return;
            }

            FormQueryInfo form = QueryInfoDialog.GetProviderInfoDialog();

            if (DialogResult.OK == form.ShowDialog())
            {
                txtProvider.Text = form.GetDataItem("供应商编码").ToString();
            }
        }

        /// <summary>
        /// 获取S_MaterialDetainBill数据集
        /// </summary>
        /// <returns>返回S_MaterialDetainBill数据集</returns>
        S_MaterialDetainBill GetData()
        {
            S_MaterialDetainBill bill = new S_MaterialDetainBill();

            bill.Bill_ID = txtBill_ID.Text;
            bill.Bill_Time = ServerModule.ServerTime.Time;
            bill.BillStatus = "新建单据";
            bill.Department = BasicInfo.DeptCode;
            bill.FillInPersonName = BasicInfo.LoginName;
            bill.FillInPersonCode = BasicInfo.LoginID;
            bill.Provider = txtProvider.Text;
            bill.Reason = txtReason.Text;
            bill.Remark = txtRemark.Text;
            bill.StorageID = UniversalFunction.GetStorageID(cmbStorage.Text);

            return bill;
        }

        private void 设置清单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.dataGridView1.CellEnter -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);

                if (BasicInfo.LoginName != txtProposer.Text.Trim())
                {
                    MessageDialog.ShowPromptMessage("您不是建单人员，无法进行此操作");
                    return;
                }

                if (lblBillStatus.Text.Trim() == "单据已完成")
                {
                    MessageDialog.ShowPromptMessage("单据已完成，无法进行此操作");
                    return;
                }

                if (!CheckDataItem())
                    return;

                if (!m_billServer.AddBill(GetData(), out m_strErr))
                {
                    MessageDialog.ShowErrorMessage(m_strErr);
                    return;
                }

                string strBillno = txtBill_ID.Text;

                FormMaterialDetainList form = new FormMaterialDetainList(
                    CE_BusinessOperateMode.修改, txtProvider.Text, txtBill_ID.Text,
                    lblBillStatus.Text, UniversalFunction.GetStorageID(cmbStorage.Text));

                form.ShowDialog();

                RefreshDataGridView(m_billServer.GetAllBill());
                PositioningRecord(strBillno);
            }
            finally
            {
                this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
            }

        }

        private void 领料员提交单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((string)dataGridView1.CurrentRow.Cells["建单人编号"].Value != BasicInfo.LoginID)
            {
                MessageDialog.ShowPromptMessage("您不是此记录的编制者无法进行此操作");
                return;
            }

            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择一条记录后再进行此操作");
                return;
            }
            else if (dataGridView1.SelectedRows.Count > 1)
            {
                MessageDialog.ShowPromptMessage("只允许选择一条记录进行处理！");
                return;
            }

            if (lblBillStatus.Text.Trim() != "新建单据")
            {
                MessageDialog.ShowPromptMessage("您现在不是处于新建单据状态，无法提交");
                return;
            }

            if (!CheckDataItem())
                return;

            string billNo = txtBill_ID.Text;

            if (m_billServer.GetList(billNo, out m_strErr).Rows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("您还未设置物品清单，无法提交");
                return;
            }

            if (!m_billServer.UpdateBill(GetData(), out m_strErr))
            {
                MessageDialog.ShowErrorMessage(m_strErr);
                return;
            }

            MessageDialog.ShowPromptMessage("成功提交,等待领导审核!");

            m_billMessageServer.DestroyMessage(billNo);
            SendNewFlowMessage(billNo);
            RefreshDataGridView(m_billServer.GetAllBill());
            PositioningRecord(billNo);
        }

        #region 流消息

        /// <summary>
        /// 发送新的流消息
        /// </summary>
        /// <param name="bill">单据内容</param>
        void SendNewFlowMessage(string billNo)
        {
            Flow_BillFlowMessage msg = new Flow_BillFlowMessage();

            m_billMessageServer.SendNewFlowMessage(billNo, string.Format("{0}号物料扣货单已提交，请上级领导审核", billNo),
                BillFlowMessage_ReceivedUserType.角色,
                m_billMessageServer.GetSuperior(CE_RoleStyleType.上级领导, BasicInfo.LoginID));
        }

        /// <summary>
        /// 传递流消息(走流程)
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="msgContent">消息内容</param>
        /// <param name="receivedRole">接收方角色</param>
        void PassFlowMessage(string billNo, string msgContent, CE_RoleEnum receivedRole)
        {
            Flow_BillFlowMessage msg = m_billFlowMsg.GetMessage(BasicInfo.LoginID, labelTitle.Text, billNo);

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

            SendFinishedFlagToMessagePromptForm(billNo);
        }

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
        /// <param name="billNo">单据号</param>
        /// <param name="msgContent">消息内容</param>
        void EndFlowMessage(string billNo, string msgContent)
        {
            Flow_BillFlowMessage msg = m_billFlowMsg.GetMessage(BasicInfo.LoginID, labelTitle.Text, billNo);

            if (msg == null)
            {
                return;
            }

            m_billFlowMsg.EndMessage(BasicInfo.LoginID, msg.序号, msgContent);
            SendFinishedFlagToMessagePromptForm(billNo);

            #region 发送知会消息

            string content = string.Format("{0} 号物料扣货单已经处理完毕", msg.单据流水号);

            List<string> noticeRoles = new List<string>();
            noticeRoles.AddRange(m_billMessageServer.GetDeptDirectorRoleName(dataGridView1.SelectedRows[0].Cells["部门编码"].Value.ToString()));
            noticeRoles.Add(CE_RoleEnum.质量工程师.ToString());
            noticeRoles.Add(CE_RoleEnum.采购员.ToString());
            noticeRoles.Add(CE_RoleEnum.质管部办公室文员.ToString());

            m_billMessageServer.NotifyMessage(msg.单据类型, msg.单据号, content, BasicInfo.LoginID, noticeRoles, null);

            #endregion 发送知会消息
        }

        #endregion 流消息

        private void 修改单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((string)dataGridView1.CurrentRow.Cells["建单人编号"].Value != BasicInfo.LoginID)
            {
                MessageDialog.ShowPromptMessage("您不是此记录的编制者无法进行此操作");
                return;
            }

            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择一条记录后再进行此操作");
                return;
            }
            else if (dataGridView1.SelectedRows.Count > 1)
            {
                MessageDialog.ShowPromptMessage("只允许选择一条记录进行处理！");
                return;
            }

            if (lblBillStatus.Text.Trim() != "等待质管批准")
            {
                MessageDialog.ShowPromptMessage("您现在不是【等待质管批准】状态，无法进行此操作");
                return;
            }

            if (!CheckDataItem())
                return;

            if (dataGridView1.SelectedRows[0].Cells["供应商"].Value.ToString() != txtProvider.Text)
            {
                if (m_billServer.GetList(txtBill_ID.Text, out m_strErr).Rows.Count > 0)
                {
                    MessageDialog.ShowPromptMessage("已经设置好物品清单时不允许再修改供应商");
                    return;
                }
            }

            string strBillID = txtBill_ID.Text.Trim();

            if (!m_billServer.UpdateBill(GetData(), out m_strErr))
            {
                MessageDialog.ShowErrorMessage(m_strErr);
                return;
            }

            RefreshDataGridView(m_billServer.GetAllBill());
            PositioningRecord(strBillID);
        }

        private void 删除单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((string)dataGridView1.CurrentRow.Cells["建单人编号"].Value != BasicInfo.LoginID)
            {
                MessageDialog.ShowPromptMessage("您不是此记录的编制者无法进行此操作");
                return;
            }

            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择一条记录后再进行此操作");
                return;
            }
            else if (dataGridView1.SelectedRows.Count > 1)
            {
                MessageDialog.ShowPromptMessage("只允许选择一条记录进行处理！");
                return;
            }

            if ((string)dataGridView1.CurrentRow.Cells["单据状态"].Value == "单据已完成")
            {
                MessageDialog.ShowPromptMessage("不能删除已完成的单据！");
                return;
            }

            string billNo = txtBill_ID.Text;
            string info = string.Format("您是否要删除 {0} 扣货单时, 删除时同时也会删除此扣货单下的所有物品清单, 是否继续？", billNo);

            if (MessageDialog.ShowEnquiryMessage(info) == DialogResult.No)
            {
                return;
            }

            if (!m_billServer.DeleteBill(billNo, out m_strErr))
            {
                MessageDialog.ShowErrorMessage(m_strErr);
                return;
            }

            m_billNoControl.CancelBill(txtBill_ID.Text);
            DestroyMessage(billNo);
            RefreshDataGridView(m_billServer.GetAllBill());
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            ClearForm();

            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

            txtBill_ID.Text = (string)row.Cells["单据号"].Value;
            dateTime_BillTime.Value = (DateTime)row.Cells["建单时间"].Value;
            txtProposer.Text = (string)row.Cells["建单人"].Value;
            txtProvider.Text = (string)row.Cells["供应商"].Value;
            lblBillStatus.Text = (string)row.Cells["单据状态"].Value;
            cmbStorage.Text = (string)row.Cells["库房名称"].Value;

            if (row.Cells["备注"].Value != System.DBNull.Value)
                txtRemark.Text = (string)row.Cells["备注"].Value;
            else
                txtRemark.Text = "";

            if (row.Cells["扣货原因"].Value != System.DBNull.Value)
                txtReason.Text = (string)row.Cells["扣货原因"].Value;
            else
                txtReason.Text = "";

            if (row.Cells["质管批准"].Value != System.DBNull.Value)
                txtFinanceSignatory.Text = (string)row.Cells["质管批准"].Value;
            else
                txtFinanceSignatory.Text = "";

            if (row.Cells["采购确认人"].Value != System.DBNull.Value)
                txtDepotManager.Text = (string)row.Cells["采购确认人"].Value;
            else
                txtDepotManager.Text = "";

            if (row.Cells["SQE"].Value != System.DBNull.Value)
                txtSQE.Text = (string)row.Cells["SQE"].Value;
            else
                txtSQE.Text = "";

            上级部门ToolStripMenuItem.Visible =
                UniversalFunction.GetIsManager((string)row.Cells["部门编码"].Value, BasicInfo.LoginID) == "" ? false : true;
            审核单据ToolStripMenuItem.Visible = 上级部门ToolStripMenuItem.Visible;
        }

        private void 查看清单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormMaterialDetainList form = new FormMaterialDetainList(
                CE_BusinessOperateMode.查看, txtProvider.Text, txtBill_ID.Text,
                lblBillStatus.Text, UniversalFunction.GetStorageID(cmbStorage.Text));

            form.ShowDialog();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            查看清单ToolStripMenuItem_Click(sender, e);
        }

        private void 批准ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择一条记录后再进行此操作");
                return;
            }
            else if (dataGridView1.SelectedRows.Count > 1)
            {
                MessageDialog.ShowPromptMessage("只允许选择一条记录进行处理！");
                return;
            }

            if (lblBillStatus.Text != "等待质管批准")
            {
                MessageDialog.ShowPromptMessage("请选择要质管批准的记录后再进行此操作");
                return;
            }

            string billNo = txtBill_ID.Text;

            if (!m_billServer.AuthorizeBill(billNo, BasicInfo.LoginName, out m_strErr))
            {
                MessageDialog.ShowErrorMessage(m_strErr);
                return;
            }

            MessageDialog.ShowPromptMessage("已经批准,等待SQE确认!");

            PassFlowMessage(billNo, string.Format("{0}号物料扣货单已提交，请SQE处理",
                        billNo), CE_RoleEnum.SQE组员);

            RefreshDataGridView(m_billServer.GetAllBill());

            PositioningRecord(billNo);
        }

        private void 采购确认ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择一条记录后再进行此操作");
                return;
            }
            else if (dataGridView1.SelectedRows.Count > 1)
            {
                MessageDialog.ShowPromptMessage("只允许选择一条记录进行处理！");
                return;
            }

            if (lblBillStatus.Text != "等待采购确认")
            {
                MessageDialog.ShowPromptMessage("请选择要采购确认的记录后再进行此操作");
                return;
            }

            string billNo = txtBill_ID.Text;
            DataTable dtList = m_billServer.GetList(billNo, out m_strErr);

            for (int i = 0; i < dtList.Rows.Count; i++)
            {
                if (dtList.Rows[i]["关联订单号"] == System.DBNull.Value || dtList.Rows[i]["关联订单号"].ToString() == "")
                {
                    MessageDialog.ShowPromptMessage("此扣货单下的物料清单中，有物料没有选择订单信息！");
                    return;
                }
            }

            if (!m_billServer.ConfirmBill(billNo, BasicInfo.LoginName, out m_strErr))
            {
                MessageDialog.ShowErrorMessage(m_strErr);
                return;
            }

            MessageDialog.ShowPromptMessage("已经确认,单据完成!");

            EndFlowMessage(billNo, string.Format("{0}号物料扣货单已完成", billNo));

            RefreshDataGridView(m_billServer.GetAllBill());

            PositioningRecord(billNo);
        }

        private void 核实物料信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lblBillStatus.Text != "等待采购确认")
            {
                MessageDialog.ShowPromptMessage("请选择要采购确认的记录后再进行此操作");
                return;
            }

            FormMaterialDetainList form = new FormMaterialDetainList(
                CE_BusinessOperateMode.采购确认, txtProvider.Text, txtBill_ID.Text,
                lblBillStatus.Text, UniversalFunction.GetStorageID(cmbStorage.Text));

            form.ShowDialog();
        }

        private void 审核单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择一条记录后再进行此操作");
                return;
            }
            else if (dataGridView1.SelectedRows.Count > 1)
            {
                MessageDialog.ShowPromptMessage("只允许选择一条记录进行处理！");
                return;
            }

            if (lblBillStatus.Text != "等待领导审核")
            {
                MessageDialog.ShowPromptMessage("请选择等待领导审核的记录后再进行此操作");
                return;
            }

            string billNo = txtBill_ID.Text;

            if (!m_billServer.AuditingBill(billNo, out m_strErr))
            {
                MessageDialog.ShowErrorMessage(m_strErr);
                return;
            }

            MessageDialog.ShowPromptMessage("已经审核,等待质量工程师确认!");

            m_billMessageServer.PassFlowMessage(billNo, string.Format("{0}号物料扣货单已提交，请质量工程师处理", billNo), CE_RoleEnum.质量工程师);

            RefreshDataGridView(m_billServer.GetAllBill());

            PositioningRecord(billNo);
        }

        private void 确认单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择一条记录后再进行此操作");
                return;
            }
            else if (dataGridView1.SelectedRows.Count > 1)
            {
                MessageDialog.ShowPromptMessage("只允许选择一条记录进行处理！");
                return;
            }

            if (lblBillStatus.Text != "等待SQE确认")
            {
                MessageDialog.ShowPromptMessage("请选择等待SQE确认的记录后再进行此操作");
                return;
            }

            string billNo = txtBill_ID.Text;

            if (!m_billServer.SQEConfirmBill(billNo, out m_strErr))
            {
                MessageDialog.ShowErrorMessage(m_strErr);
                return;
            }

            MessageDialog.ShowPromptMessage("已经确认,等待采购确认!");

            m_billMessageServer.PassFlowMessage(billNo, string.Format("{0}号物料扣货单已提交，请采购工程师处理", billNo), CE_RoleEnum.采购员);

            RefreshDataGridView(m_billServer.GetAllBill());

            PositioningRecord(billNo);
        }

        private void 回退单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (上级部门ToolStripMenuItem.Visible == true
                && lblBillStatus.Text == "等待领导审核")
            {
                ReturnBillStatus();
            }

            if (质管操作ToolStripMenuItem.Visible == true
                && lblBillStatus.Text == "等待质管批准")
            {
                ReturnBillStatus();
            }

            if (sQE操作ToolStripMenuItem.Visible == true
                && lblBillStatus.Text == "等待SQE确认")
            {
                ReturnBillStatus();
            }

            if (采购操作ToolStripMenuItem.Visible == true
                && lblBillStatus.Text == "等待采购确认")
            {
                ReturnBillStatus();
            }
        }

        /// <summary>
        /// 回退单据
        /// </summary>
        private void ReturnBillStatus()
        {
            if (lblBillStatus.Text != "单据已完成")
            {
                回退单据 form = new 回退单据(CE_BillTypeEnum.物料扣货单, txtBill_ID.Text, lblBillStatus.Text);

                if (form.ShowDialog() == DialogResult.OK)
                {
                    if (MessageDialog.ShowEnquiryMessage("您确定要回退此单据吗？") == DialogResult.Yes)
                    {
                        if (m_billServer.ReturnBill(form.StrBillID, form.StrBillStatus, out m_strErr, form.Reason))
                        {
                            MessageDialog.ShowPromptMessage("回退成功");
                        }
                        else
                        {
                            MessageDialog.ShowPromptMessage(m_strErr);
                        }
                    }
                }

                RefreshDataGridView(m_billServer.GetAllBill());
            }
            else
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
            }
        }
    }
}
