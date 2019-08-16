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
    /// 领料退库单界面
    /// </summary>
    public partial class 领料退库单 : Form
    {
        #region 成员变量

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_error;

        /// <summary>
        /// 检索到的领料单结果
        /// </summary>
        IQueryResult m_queryResult;

        /// <summary>
        /// 单据服务组件
        /// </summary>
        IMaterialReturnedInTheDepot m_billServer = ServerModuleFactory.GetServerModule<IMaterialReturnedInTheDepot>();

        /// <summary>
        /// 获取单据流消息操作接口
        /// </summary>
        IBillFlowMessage m_billFlowMsg = PlatformFactory.GetObject<IBillFlowMessage>();

        /// <summary>
        /// 退库物品清单
        /// </summary>
        IMaterialListReturnedInTheDepot m_goodsServer = ServerModuleFactory.GetServerModule<IMaterialListReturnedInTheDepot>();

        /// <summary>
        /// 领料单物品清单服务
        /// </summary>
        IMaterialRequisitionGoodsServer m_materialRequisitionGoods = ServerModuleFactory.GetServerModule<IMaterialRequisitionGoodsServer>();

        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 查找条件窗体
        /// </summary>
        FormConditionFind m_formFindCondition;

        /// <summary>
        /// 数据定位控件
        /// </summary>
        UserControlDataLocalizer m_dataLocalizer;

        /// <summary>
        /// 可供查找的所有字段
        /// </summary>
        string[] m_findField = null;

        /// <summary>
        /// 窗体消息发布器
        /// </summary>
        WndMsgSender m_wndMsgSender = new WndMsgSender();

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag;

        #endregion

        public 领料退库单(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_billMessageServer.BillType = "领料退库单";

            m_billNoControl = new BillNumberControl(labelTitle.Text, m_billServer);

            m_authFlag = nodeInfo.Authority;

            DataTable dt = UniversalFunction.GetStorageTb();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cmbStorage.Items.Add(dt.Rows[i]["StorageName"].ToString());
            }

            cmbStorage.SelectedIndex = -1;

            string[] strBillStatus = { "全部", 
                                         MaterialReturnedInTheDepotBillStatus.新建单据.ToString(), 
                                         MaterialReturnedInTheDepotBillStatus.等待主管审核.ToString(),
                                         MaterialReturnedInTheDepotBillStatus.等待质检批准.ToString(),
                                         MaterialReturnedInTheDepotBillStatus.等待仓管退库.ToString(),
                                         MaterialReturnedInTheDepotBillStatus.已完成.ToString()};

            checkBillDateAndStatus1.InsertComBox(strBillStatus);

            checkBillDateAndStatus1.OnCompleteSearch +=
                new GlobalObject.DelegateCollection.NonArgumentHandle(checkBillDateAndStatus1_OnCompleteSearch);

            checkBillDateAndStatus1.dtpStartTime.Value = ServerTime.Time.AddDays(1).AddMonths(-1);
            checkBillDateAndStatus1.dtpEndTime.Value = ServerTime.Time.AddDays(1);

            menuItemReresh_Click(null, null);
        }

        private void 领料退库单_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authFlag);
        }

        void checkBillDateAndStatus1_OnCompleteSearch()
        {
            menuItemReresh_Click(null, null);
        }

        /// <summary>
        /// 权限控制
        /// </summary>
        /// <param name="authorityFlag">权限标志</param>
        void AuthorityControl(PlatformManagement.AuthorityFlag authorityFlag)
        {
            FaceAuthoritySetting.SetVisibly(menuStrip, authorityFlag);
        }

        private void 领料退库单_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
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

                    DataTable dtMessage = UniversalFunction.PositioningOneRecord(msg.MessageContent, "领料退库单");

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

        private void btnFindPurpose_Click(object sender, EventArgs e)
        {
            领料用途 form = new 领料用途();

            if (form.ShowDialog() == DialogResult.OK)
            {
                txtPurpose.Tag = form.SelectedData;
                txtPurpose.Text = form.SelectedData.Purpose;
            }
        }

        #region 刷新数据


        /// <summary>
        /// 清除窗体上的控件残留信息
        /// </summary>
        void ClearForm()
        {
            lblBillStatus.Text = "";
            cmbStorage.SelectedIndex = -1;
            txtBill_ID.Text = "";
            dateTime_BillTime.Value = ServerModule.ServerTime.Time;
            txtPurpose.Text = "";
            txtReturnedReason.Text = "";
            txtDepartment.Text = "";
            txtRemark.Text = "";
            chkIsOnlyForRepair.Enabled = false;
            chkIsOnlyForRepair.Checked = false;
            cmbType.SelectedIndex = -1;
            txtProposer.Text = "";
            txtDepartmentDirector.Text = "";
            txtDepotManager.Text = "";
            txtQualityInputer.Text = "";
            cmbMode.SelectedIndex = -1;
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

        /// <summary>
        /// 检测建单数据项内容是否正确
        /// </summary>
        /// <returns>返回检测结果</returns>
        bool CheckDataItem()
        {
            if (txtPurpose.Text == "")
            {
                txtPurpose.Focus();
                MessageDialog.ShowPromptMessage("请选择初始用途");
                return false;
            }

            if (txtReturnedReason.Text == "")
            {
                txtReturnedReason.Focus();
                MessageDialog.ShowPromptMessage("退库原因不允许为空!");
                return false;
            }

            if (cmbStorage.Text == "")
            {
                cmbStorage.Focus();
                MessageDialog.ShowPromptMessage("所属库房不允许为空!");
                return false;
            }

            if (cmbType.SelectedIndex == -1)
            {
                cmbType.Focus();
                MessageDialog.ShowPromptMessage("请选择退库类别!");
                return false;
            }

            if (cmbMode.Text.Trim() == "")
            {
                cmbMode.Focus();
                MessageDialog.ShowPromptMessage("请选择退库方式!");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 检查用户对指定行记录是否有操作许可
        /// </summary>
        /// <param name="row">要操作的行记录</param>
        /// <returns>允许返回true</returns>
        bool CheckUserOperation(DataGridViewRow row)
        {
            if ((string)row.Cells["申请人编码"].Value == BasicInfo.LoginID)
            {
                return true;
            }
            else
            {
                MessageDialog.ShowPromptMessage("您不是此记录的编制者无法进行此操作");
                return false;
            }
        }

        /// <summary>
        /// 刷新DataGridView
        /// </summary>
        /// <param name="findBill">数据集</param>
        void RefreshDataGridView(IQueryResult findBill)
        {
            ClearForm();

            if (findBill == null)
            {
                return;
            }

            dataGridView1.DataSource = findBill.DataCollection.Tables[0];

            this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridView1_ColumnWidthChanged);
            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);
            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridView1_ColumnWidthChanged);

            #region 隐藏不允许查看的列

            if (findBill.HideFields != null && findBill.HideFields.Length > 0)
            {
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    if (findBill.HideFields.Contains(dataGridView1.Columns[i].Name))
                    {
                        dataGridView1.Columns[i].Visible = false;
                    }
                }
            }

            #endregion

            dataGridView1.Columns["申请部门编码"].Visible = false;
            dataGridView1.Columns["申请人编码"].Visible = false;

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
                m_dataLocalizer = new UserControlDataLocalizer(dataGridView1, this.Name, UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
                panelPara.Controls.Add(m_dataLocalizer);
                m_dataLocalizer.Dock = DockStyle.Bottom;
            }

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
                if ((string)dataGridView1.Rows[i].Cells["退库单号"].Value == billNo)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                    break;
                }
            }
        }

        #endregion 刷新数据

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

        #region 流消息

        /// <summary>
        /// 发送新的流消息
        /// </summary>
        /// <param name="billNo">单据内容</param>
        void SendNewFlowMessage(string billNo)
        {
            try
            {
                string[] roleCodes = m_billMessageServer.GetSuperior(CE_RoleStyleType.上级领导, BasicInfo.LoginID).ToArray();

                Flow_BillFlowMessage msg = new Flow_BillFlowMessage();
                msg.初始发起方用户编码 = BasicInfo.LoginID;
                msg.单据号 = billNo;
                msg.单据类型 = labelTitle.Text;
                msg.单据流水号 = billNo;
                msg.接收方类型 = BillFlowMessage_ReceivedUserType.角色.ToString();
                msg.单据状态 = BillStatus.等待处理.ToString();

                msg.发起方消息 = string.Format("【用途】：{0}  【退库类别】：{1} 【申请人】：{2}  ※※※ 等待【上级领导】处理",
                txtPurpose.Text, cmbType.Text, txtProposer.Text);

                msg.接收方 = "";

                if (roleCodes.Count() > 0)
                {
                    foreach (string role in roleCodes)
                    {
                        msg.接收方 += role + ",";
                    }
                }

                msg.接收方 = msg.接收方.Substring(0, msg.接收方.Length - 1);

                msg.期望的处理完成时间 = null;

                m_billFlowMsg.SendRequestMessage(BasicInfo.LoginID, msg);
            }
            catch (Exception exce)
            {
                MessageDialog.ShowErrorMessage(exce.Message);
            }
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
            msg.发起方消息 = string.Format("【用途】：{0}  【退库类别】：{1} 【申请人】：{2}  ※※※ 等待【{3}】处理",
                txtPurpose.Text, cmbType.Text, txtProposer.Text, receivedRole.ToString());
            msg.接收方类型 = BillFlowMessage_ReceivedUserType.角色.ToString();
            msg.接收方 = receivedRole.ToString();

            msg.期望的处理完成时间 = null;
            m_billFlowMsg.ContinueMessage(BasicInfo.LoginID, msg);

            SendFinishedFlagToMessagePromptForm(billNo);
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

            string content = string.Format("{0} 号领料退库单已经处理完毕", msg.单据流水号);

            List<string> noticeRoles = new List<string>();
            noticeRoles.AddRange(m_billMessageServer.GetDeptDirectorRoleName(dataGridView1.SelectedRows[0].Cells["申请部门"].Value.ToString()));
            noticeRoles.Add(CE_RoleEnum.质量总监.ToString());
            noticeRoles.Add(CE_RoleEnum.质控主管.ToString());
            noticeRoles.Add(CE_RoleEnum.质量工程师.ToString());

            m_billMessageServer.NotifyMessage(msg.单据类型, msg.单据号, content, BasicInfo.LoginID, noticeRoles, null);
            #endregion 发送知会消息
        }

        #endregion 流消息

        private void 新建单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                if (dataGridView1.SelectedRows[0].Cells["单据状态"].Value.ToString() != "新建单据")
                {
                    if (lblBillStatus.Text == MaterialReturnedInTheDepotBillStatus.新建单据.ToString())
                    {
                        MessageDialog.ShowPromptMessage("现在已经处于新建单据状态");
                        return;
                    }
                }

                if (BasicInfo.LoginID == (string)dataGridView1.SelectedRows[0].Cells["申请人编码"].Value)
                {
                    if (lblBillStatus.Text == MaterialReturnedInTheDepotBillStatus.新建单据.ToString())
                    {
                        MessageDialog.ShowPromptMessage("现在已经处于新建单据状态");
                        return;
                    }
                }
            }
            else
            {
                if (lblBillStatus.Text == MaterialReturnedInTheDepotBillStatus.新建单据.ToString())
                {
                    MessageDialog.ShowPromptMessage("现在已经处于新建单据状态");
                    return;
                }
            }

            ClearForm();

            txtBill_ID.Text = m_billNoControl.GetNewBillNo();

            dateTime_BillTime.Value = ServerModule.ServerTime.Time;
            txtProposer.Text = BasicInfo.LoginName;
            txtDepartment.Text = BasicInfo.DeptName;
            lblBillStatus.Text = MaterialReturnedInTheDepotBillStatus.新建单据.ToString();
        }

        private void 设置退库清单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string billNo = txtBill_ID.Text;

            try
            {
                this.dataGridView1.CellEnter -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);

                if (lblBillStatus.Text == MaterialReturnedInTheDepotBillStatus.已完成.ToString())
                {
                    MessageDialog.ShowPromptMessage("您现在不是处于新建单据状态，无法进行此操作");
                    return;
                }

                if (!CheckDataItem())
                {
                    return;
                }

                if (chkIsOnlyForRepair.Enabled && !chkIsOnlyForRepair.Checked)
                {
                    if (MessageDialog.ShowEnquiryMessage("此单未勾选仅限于返修箱用，是否继续？") == DialogResult.No)
                    {
                        return;
                    }
                }

                // 如果此单据存在则检查选择行
                if (m_billServer.IsExist(txtBill_ID.Text))
                {
                    if (!CheckSelectedRow())
                        return;

                    if (!CheckUserOperation(dataGridView1.SelectedRows[0]))
                        return;
                }
                else
                {
                    BASE_MaterialRequisitionPurpose purpose = txtPurpose.Tag as BASE_MaterialRequisitionPurpose;

                    // 如果此单据还不存在则创建
                    S_MaterialReturnedInTheDepot bill = new S_MaterialReturnedInTheDepot();

                    bill.Bill_ID = txtBill_ID.Text;
                    bill.Bill_Time = ServerModule.ServerTime.Time;
                    bill.BillStatus = MaterialReturnedInTheDepotBillStatus.新建单据.ToString();
                    bill.Department = BasicInfo.DeptCode;
                    bill.ReturnType = cmbType.Text;
                    bill.FillInPersonnel = BasicInfo.LoginName;
                    bill.FillInPersonnelCode = BasicInfo.LoginID;
                    bill.PurposeCode = purpose.Code;
                    bill.ReturnReason = txtReturnedReason.Text;
                    bill.Remark = txtRemark.Text;
                    bill.StorageID = UniversalFunction.GetStorageID(cmbStorage.Text);
                    bill.ReturnMode = cmbMode.Text.Trim();
                    bill.IsOnlyForRepair = chkIsOnlyForRepair.Checked;

                    if (!m_billServer.AddBill(bill, out m_queryResult, out m_error))
                    {
                        MessageDialog.ShowErrorMessage(m_error);
                        return;
                    }

                }

                FormMaterialListReturnedInTheDepot form =
                    new FormMaterialListReturnedInTheDepot(CE_BusinessOperateMode.修改, txtBill_ID.Text);

                form.StrReturnMode = cmbMode.Text.Trim();
                form.BlIsOnlyForRepair = chkIsOnlyForRepair.Checked;
                form.ShowDialog();
            }
            finally
            {
                this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
            }

            RefreshDataGridView(m_queryResult);
            PositioningRecord(billNo);

        }

        private void 领料员提交单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
                return;

            if (lblBillStatus.Text != MaterialReturnedInTheDepotBillStatus.新建单据.ToString())
            {
                MessageDialog.ShowPromptMessage("您现在不是处于新建单据状态，无法提交");
                return;
            }

            if (!CheckUserOperation(dataGridView1.SelectedRows[0]))
                return;

            if (!CheckDataItem())
                return;

            string billNo = txtBill_ID.Text;

            if (!m_goodsServer.IsExist(billNo))
            {
                MessageDialog.ShowPromptMessage("您还未设置物品清单，无法提交");
                return;
            }

            if (!m_billServer.SubmitNewBill(billNo, out m_queryResult, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            MessageDialog.ShowPromptMessage("成功提交,等待主管审核!");

            m_billMessageServer.DestroyMessage(billNo);
            SendNewFlowMessage(billNo);
            RefreshDataGridView(m_queryResult);
            PositioningRecord(billNo);
        }

        private void 修改单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
                return;

            if (lblBillStatus.Text != MaterialReturnedInTheDepotBillStatus.新建单据.ToString())
            {
                MessageDialog.ShowPromptMessage("您现在不是处于新建单据状态，无法进行此操作");
                return;
            }

            if (!CheckUserOperation(dataGridView1.SelectedRows[0]))
                return;

            if (!CheckDataItem())
                return;

            BASE_MaterialRequisitionPurpose purpose = txtPurpose.Tag as BASE_MaterialRequisitionPurpose;

            S_MaterialReturnedInTheDepot bill = new S_MaterialReturnedInTheDepot();

            bill.Bill_ID = txtBill_ID.Text;
            bill.ReturnType = cmbType.Text;
            bill.Bill_Time = ServerModule.ServerTime.Time;
            bill.PurposeCode = purpose == null ? txtPurpose.Tag.ToString() : purpose.Code;
            bill.ReturnReason = txtReturnedReason.Text;
            bill.Remark = txtRemark.Text;
            bill.ReturnMode = cmbMode.Text;
            bill.IsOnlyForRepair = chkIsOnlyForRepair.Checked;
            bill.StorageID = UniversalFunction.GetStorageID(cmbStorage.Text);

            if (!m_billServer.UpdateBill(bill, out m_queryResult, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            RefreshDataGridView(m_queryResult);
            PositioningRecord(bill.Bill_ID);
        }

        private void 删除单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
                return;

            if (txtBill_ID.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请选择需要删除的记录");
                return;
            }

            MaterialReturnedInTheDepotBillStatus status = (MaterialReturnedInTheDepotBillStatus)Enum.Parse(
                typeof(MaterialReturnedInTheDepotBillStatus),
                UniversalFunction.GetBillStatus("S_MaterialReturnedInTheDepot", "BillStatus", "Bill_ID", txtBill_ID.Text));

            if (status == MaterialReturnedInTheDepotBillStatus.已完成)
            {
                MessageDialog.ShowPromptMessage("请选择未完成的记录后再进行此操作");
                return;
            }

            if (!CheckUserOperation(dataGridView1.SelectedRows[0]))
                return;

            string info = string.Format("您是否要删除 {0} 退库单时, 删除时同时也会删除此退库单下的所有物品清单, 是否继续？", txtBill_ID.Text);

            if (MessageDialog.ShowEnquiryMessage(info) == DialogResult.No)
            {
                return;
            }

            if (!m_billServer.DeleteBill(txtBill_ID.Text, out m_queryResult, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            m_billNoControl.CancelBill(txtBill_ID.Text);
            DestroyMessage(txtBill_ID.Text);
            RefreshDataGridView(m_queryResult);
        }

        private void 审核ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
                return;

            if (lblBillStatus.Text != MaterialReturnedInTheDepotBillStatus.等待主管审核.ToString())
            {
                MessageDialog.ShowPromptMessage("请选择要审核的记录后再进行此操作");
                return;
            }

            string billNo = txtBill_ID.Text;

            if (!m_billServer.DirectorAuthorizeBill(billNo, BasicInfo.LoginName, out m_queryResult, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            MessageDialog.ShowPromptMessage("成功审核, 等待质检批准");

            string msg = string.Format("{0} 号领料退库单主管已经审核，请质检批准", billNo);

            if (dataGridView1.CurrentRow.Cells["退库类别"].Value.ToString() == "装配退库")
            {
                PassFlowMessage(billNo, msg, CE_RoleEnum.质检员_装配);
            }
            else if (dataGridView1.CurrentRow.Cells["退库类别"].Value.ToString() == "售后退库")
            {
                PassFlowMessage(billNo, msg, CE_RoleEnum.质检员_售后);
            }
            else if (dataGridView1.CurrentRow.Cells["退库类别"].Value.ToString() == "委外退库")
            {
                PassFlowMessage(billNo, msg, CE_RoleEnum.质检员_委外);
            }
            else if (dataGridView1.CurrentRow.Cells["退库类别"].Value.ToString() == "下线退库")
            {
                PassFlowMessage(billNo, msg, CE_RoleEnum.质检员_下线);
            }
            else if (dataGridView1.CurrentRow.Cells["退库类别"].Value.ToString() == "机加退库")
            {
                PassFlowMessage(billNo, msg, CE_RoleEnum.质检员_机加);
            }
            else
            {
                PassFlowMessage(billNo, msg, CE_RoleEnum.质量工程师);
            }

            RefreshDataGridView(m_queryResult);
            PositioningRecord(billNo);
        }

        private void 批准退库ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
                return;

            if (lblBillStatus.Text != MaterialReturnedInTheDepotBillStatus.等待质检批准.ToString())
            {
                MessageDialog.ShowPromptMessage("请选择要质检批准的记录后再进行此操作");
                return;
            }

            string billNo = txtBill_ID.Text;

            if (!m_billServer.QualityAuthorizeBill(billNo, BasicInfo.LoginName, out m_queryResult, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            MessageDialog.ShowPromptMessage("已经批准,等待仓管确认!");

            string msg = string.Format("【用途】：{0}  【退库类别】：{1} 【申请人】：{2}  ※※※ 等待【仓管员】处理",
                txtPurpose.Text, cmbType.Text, txtProposer.Text);

            PassFlowMessage(billNo, msg, m_billMessageServer.GetRoleStringForStorage(dataGridView1.CurrentRow.Cells["库房名称"].Value.ToString()));
            RefreshDataGridView(m_queryResult);
            PositioningRecord(billNo);
        }

        private void 核实退库清单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
                return;

            if (lblBillStatus.Text != MaterialReturnedInTheDepotBillStatus.等待仓管退库.ToString())
            {
                MessageDialog.ShowPromptMessage("请选择要仓管确认的记录后再进行此操作");
                return;
            }

            FormMaterialListReturnedInTheDepot form =
                new FormMaterialListReturnedInTheDepot(CE_BusinessOperateMode.仓库核实, txtBill_ID.Text);
            form.ShowDialog();
        }

        private void 确认退库ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
                return;

            if (lblBillStatus.Text != MaterialReturnedInTheDepotBillStatus.等待仓管退库.ToString())
            {
                MessageDialog.ShowPromptMessage("请选择要确认的记录后再进行此操作");
                return;
            }

            if (MessageDialog.ShowEnquiryMessage("您是否已经审核过退库清单？") == DialogResult.No)
            {
                return;
            }

            string billNo = txtBill_ID.Text;

            if (!m_billServer.FinishBill(billNo, BasicInfo.LoginName, out m_queryResult, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            EndFlowMessage(billNo, string.Format("{0}号领料退库单已完成", billNo));
            RefreshDataGridView(m_queryResult);
            PositioningRecord(billNo);
        }

        private void 表单打印ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
                return;

            if (lblBillStatus.Text != MaterialReturnedInTheDepotBillStatus.已完成.ToString())
            {
                MessageDialog.ShowPromptMessage("请选择已完成的记录后再进行此操作");
                return;
            }

            报表_领料退库单 report = new 报表_领料退库单(txtBill_ID.Text, labelTitle.Text);
            PrintReportBill print = new PrintReportBill(21.8, 9.31, report);
            print.DirectPrintReport();
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            ClearForm();

            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

            chkIsOnlyForRepair.Checked = Convert.ToBoolean(row.Cells["是否仅限于返修箱"].Value);
            txtBill_ID.Text = (string)row.Cells["退库单号"].Value;
            dateTime_BillTime.Value = (DateTime)row.Cells["退库时间"].Value;
            txtProposer.Text = (string)row.Cells["申请人"].Value;
            txtDepartment.Text = (string)row.Cells["申请部门"].Value;
            txtPurpose.Text = (string)row.Cells["初始用途"].Value;
            txtPurpose.Tag = (string)row.Cells["初始用途编码"].Value;
            cmbStorage.Text = UniversalFunction.GetStorageName(row.Cells["库房代码"].Value.ToString());
            lblBillStatus.Text = (string)row.Cells["单据状态"].Value;
            cmbMode.Text = (string)row.Cells["退库方式"].Value;

            if (row.Cells["备注"].Value != System.DBNull.Value)
                txtRemark.Text = (string)row.Cells["备注"].Value;
            else
                txtRemark.Text = "";

            if (row.Cells["退库原因"].Value != System.DBNull.Value)
                txtReturnedReason.Text = (string)row.Cells["退库原因"].Value;
            else
                txtReturnedReason.Text = "";

            if (row.Cells["退库类别"].Value != System.DBNull.Value)
                cmbType.Text = (string)row.Cells["退库类别"].Value;
            else
                cmbType.Text = "";

            if (row.Cells["部门主管签名"].Value != System.DBNull.Value)
                txtDepartmentDirector.Text = (string)row.Cells["部门主管签名"].Value;
            else
                txtDepartmentDirector.Text = "";

            if (row.Cells["质量工程师签名"].Value != System.DBNull.Value)
                txtQualityInputer.Text = (string)row.Cells["质量工程师签名"].Value;
            else
                txtQualityInputer.Text = "";

            if (row.Cells["仓管签名"].Value != System.DBNull.Value)
                txtDepotManager.Text = (string)row.Cells["仓管签名"].Value;
            else
                txtDepotManager.Text = "";

            仓库管理员操作ToolStripMenuItem.Visible = UniversalFunction.CheckStorageAndPersonnel(
                (string)row.Cells["库房代码"].Value);

        }

        private void 查找ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_formFindCondition == null)
            {
                m_formFindCondition = new FormConditionFind(this, m_findField, labelTitle.Text, labelTitle.Text);
            }

            m_formFindCondition.ShowDialog();
        }

        private void menuItemReresh_Click(object sender, EventArgs e)
        {
            m_billServer.QueryResultFilter = QueryFilterControl.GetFilterString(labelTitle.Text)
                + checkBillDateAndStatus1.GetSqlString("退库时间", "单据状态");

            if (!m_billServer.GetAllBill(out m_queryResult, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            RefreshDataGridView(m_queryResult);
        }

        private void 设置数据过滤器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormFilterCondition form = new FormFilterCondition(labelTitle.Text, m_findField, labelTitle.Text);
            form.ShowDialog();

            m_billServer.QueryResultFilter = QueryFilterControl.GetFilterString(labelTitle.Text);
            menuItemReresh.PerformClick();
        }

        private void 查看领料清单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormMaterialListReturnedInTheDepot form =
                new FormMaterialListReturnedInTheDepot(CE_BusinessOperateMode.查看, txtBill_ID.Text);
            form.ShowDialog();
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            查看领料清单ToolStripMenuItem.PerformClick();
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

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        private void 修改退库类别ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
                return;

            if (!CheckDataItem())
                return;

            if (dataGridView1.SelectedRows[0].Cells["质量工程师签名"].Value.ToString() != BasicInfo.LoginName)
            {
                MessageDialog.ShowPromptMessage("只能修改本人签名的记录");
                return;
            }

            S_MaterialReturnedInTheDepot bill = m_billServer.GetBill(txtBill_ID.Text);
            bill.ReturnType = cmbType.Text;

            if (!m_billServer.UpdateBill(bill, out m_queryResult, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
            }

            RefreshDataGridView(m_queryResult);
            PositioningRecord(bill.Bill_ID);
        }

        private void 综合查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IAuthorization authorization = PlatformFactory.GetObject<IAuthorization>();
            string businessID = "领料退库单综合查询";
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

        /// <summary>
        /// 回退单据
        /// </summary>
        private void ReturnBillStatus()
        {
            if (lblBillStatus.Text != "已完成")
            {
                回退单据 form = new 回退单据(CE_BillTypeEnum.领料退库单, txtBill_ID.Text, lblBillStatus.Text);

                if (form.ShowDialog() == DialogResult.OK)
                {
                    if (MessageDialog.ShowEnquiryMessage("您确定要回退此单据吗？") == DialogResult.Yes)
                    {
                        if (m_billServer.ReturnBill(form.StrBillID,
                            form.StrBillStatus, out m_queryResult, out m_error, form.Reason))
                        {
                            MessageDialog.ShowPromptMessage("回退成功");

                        }
                        else
                        {
                            MessageDialog.ShowPromptMessage(m_error);
                        }
                    }
                }

                RefreshDataGridView(m_queryResult);
            }
            else
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
            }
        }

        private void 回退单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (部门主管操作ToolStripMenuItem.Visible == true
                && lblBillStatus.Text == "等待主管审核")
            {
                ReturnBillStatus();
            }

            if (仓库管理员操作ToolStripMenuItem.Visible == true
                && lblBillStatus.Text == "等待仓管退库")
            {
                ReturnBillStatus();
            }

            if (质检操作ToolStripMenuItem.Visible == true
                && lblBillStatus.Text == "等待质检批准")
            {
                ReturnBillStatus();
            }
        }

        private void 统计数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            单据统计 frm = new 单据统计("领料退库单统计");
            frm.Show();
        }

        private void cmbMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMode.Text.Trim() == "返修退库")
            {
                chkIsOnlyForRepair.Enabled = true;
            }
            else
            {
                chkIsOnlyForRepair.Enabled = false;
            }
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dataGridView1.RowCount == 0)
            {
                return;
            }
            else
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(dataGridView1.Rows[i].Cells["是否仅限于返修箱"].Value))
                    {
                        dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.Red;
                    }
                }
            }
        }
    }
}
