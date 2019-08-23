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
    /// 采购退货单界面
    /// </summary>
    public partial class 采购退货单 : Form
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
        IMaterialRejectBill m_billServer = ServerModuleFactory.GetServerModule<IMaterialRejectBill>();

        /// <summary>
        /// 获取单据流消息操作接口
        /// </summary>
        IBillFlowMessage m_billFlowMsg = PlatformFactory.GetObject<IBillFlowMessage>();

        /// <summary>
        /// 退货物品清单
        /// </summary>
        IMaterialListRejectBill m_goodsServer = ServerModuleFactory.GetServerModule<IMaterialListRejectBill>();

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

        public 采购退货单(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_billMessageServer.BillType = "采购退货单";

            m_billNoControl = new BillNumberControl(labelTitle.Text, m_billServer);

            m_authFlag = nodeInfo.Authority;

            DataTable dt = UniversalFunction.GetStorageTb();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cmbStorage.Items.Add(dt.Rows[i]["StorageName"].ToString());
            }

            cmbStorage.SelectedIndex = -1;
            cmbBillType.Text = "总仓库退货单";

            string[] strBillStatus = { "全部", 
                                         MaterialRejectBillBillStatus.新建单据.ToString(), 
                                         MaterialRejectBillBillStatus.等待财务审核.ToString(),
                                     MaterialRejectBillBillStatus.等待仓管退货.ToString(),
                                     MaterialRejectBillBillStatus.已完成.ToString()};

            checkBillDateAndStatus1.InsertComBox(strBillStatus);

            checkBillDateAndStatus1.dtpStartTime.Value = ServerTime.Time.AddDays(1).AddMonths(-1);
            checkBillDateAndStatus1.dtpEndTime.Value = ServerTime.Time.AddDays(1);

            menuItemReresh_Click(null, null);
        }

        private void 采购退货单_Load(object sender, EventArgs e)
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

        private void 采购退货单_Resize(object sender, EventArgs e)
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

                    DataTable dtMessage = UniversalFunction.PositioningOneRecord(msg.MessageContent, "采购退货单");

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
        /// 查找并刷新数据
        /// </summary>
        private void RefreshData()
        {
            m_billServer.QueryResultFilter = QueryFilterControl.GetFilterString(labelTitle.Text)
                + checkBillDateAndStatus1.GetSqlString("退货时间", "单据状态");

            if (!m_billServer.GetAllBill(out m_queryResult, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            RefreshDataGridView(m_queryResult);
        }

        private void btnFindOrderForm_Click(object sender, EventArgs e)
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
            txtProvider.Text = "";
            txtReason.Text = "";
            txtDepartment.Text = "";
            txtRemark.Text = "";

            txtProposer.Text = "";
            txtFinanceSignatory.Text = "";
            txtDepotManager.Text = "";
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
            if (txtProvider.Text == "")
            {
                txtProvider.Focus();
                MessageDialog.ShowPromptMessage("请选择供应商");
                return false;
            }

            if (cmbBillType.Text.Trim() == "")
            {
                cmbBillType.Focus();
                MessageDialog.ShowPromptMessage("请选择单据类型");
                return false;
            }

            if (txtReason.Text == "")
            {
                txtReason.Focus();
                MessageDialog.ShowPromptMessage("退货原因不允许为空!");
                return false;
            }

            if (cmbStorage.Text == "")
            {
                cmbStorage.Focus();
                MessageDialog.ShowPromptMessage("所属库房不允许为空!");
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

            if (findBill.DataCollection == null || findBill.DataCollection.Tables.Count == 0)
            {
                return;
            }

            dataGridView1.DataSource = findBill.DataCollection.Tables[0];

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
                if ((string)dataGridView1.Rows[i].Cells["退货单号"].Value == billNo)
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
        /// <param name="bill">单据内容</param>
        void SendNewFlowMessage(string billNo)
        {
            m_billMessageServer.SendNewFlowMessage(billNo, string.Format("【供应商】：{0}  【退货原因】: {1}   ※※※ 等待【上级领导】处理", txtProvider.Text, txtReason.Text), 
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

            string content = string.Format("{0} 号采购退货单已经处理完毕", msg.单据流水号);

            List<string> noticeRoles = new List<string>();

            noticeRoles.AddRange(m_billMessageServer.GetDeptDirectorRoleName(
                dataGridView1.SelectedRows[0].Cells["申请部门编码"].Value.ToString()));
            noticeRoles.Add(CE_RoleEnum.会计.ToString());
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
                    if (lblBillStatus.Text == MaterialRejectBillBillStatus.新建单据.ToString())
                    {
                        MessageDialog.ShowPromptMessage("现在已经处于新建单据状态");
                        return;
                    }
                }

                if (BasicInfo.LoginID == (string)dataGridView1.SelectedRows[0].Cells["申请人编码"].Value)
                {
                    if (lblBillStatus.Text == MaterialRejectBillBillStatus.新建单据.ToString())
                    {
                        MessageDialog.ShowPromptMessage("现在已经处于新建单据状态");
                        return;
                    }
                }
            }
            else
            {
                if (lblBillStatus.Text == MaterialRejectBillBillStatus.新建单据.ToString())
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
            lblBillStatus.Text = MaterialRejectBillBillStatus.新建单据.ToString();
        }

        private void 设置退货清单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string billNo = txtBill_ID.Text;

            try
            {
                this.dataGridView1.CellEnter -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);

                if (lblBillStatus.Text != MaterialRejectBillBillStatus.新建单据.ToString())
                {
                    MessageDialog.ShowPromptMessage("您现在不是处于新建单据状态，无法进行此操作");
                    return;
                }

                if (!CheckDataItem())
                    return;

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
                    BASE_MaterialRequisitionPurpose purpose = txtProvider.Tag as BASE_MaterialRequisitionPurpose;

                    // 如果此单据还不存在则创建
                    S_MaterialRejectBill bill = new S_MaterialRejectBill();

                    bill.Bill_ID = txtBill_ID.Text;
                    bill.Bill_Time = ServerModule.ServerTime.Time;
                    bill.BillStatus = MaterialRejectBillBillStatus.新建单据.ToString();
                    bill.Department = BasicInfo.DeptCode;
                    bill.FillInPersonnel = BasicInfo.LoginName;
                    bill.FillInPersonnelCode = BasicInfo.LoginID;
                    bill.Provider = txtProvider.Text;
                    bill.Reason = txtReason.Text;
                    bill.Remark = txtRemark.Text;
                    bill.BillType = cmbBillType.Text;
                    bill.StorageID = UniversalFunction.GetStorageID(cmbStorage.Text);

                    if (!m_billServer.AddBill(bill, out m_queryResult, out m_error))
                    {
                        MessageDialog.ShowErrorMessage(m_error);
                        return;
                    }
                }

                FormMaterialListRejectBill form =
                    new FormMaterialListRejectBill(CE_BusinessOperateMode.修改, txtProvider.Text, txtBill_ID.Text);
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

            if (lblBillStatus.Text != MaterialRejectBillBillStatus.新建单据.ToString())
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

            MessageDialog.ShowPromptMessage("成功提交,等待上级领导审核!");

            m_billMessageServer.DestroyMessage(billNo);
            SendNewFlowMessage(billNo);
            RefreshDataGridView(m_queryResult);
            PositioningRecord(billNo);
        }

        private void 修改单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
                return;

            if (lblBillStatus.Text != MaterialRejectBillBillStatus.新建单据.ToString() &&
                lblBillStatus.Text != MaterialRejectBillBillStatus.等待财务审核.ToString())
            {
                MessageDialog.ShowPromptMessage("您现在不是处于新建单据状态或者等待财务审核状态，无法进行此操作");
                return;
            }

            if (!CheckUserOperation(dataGridView1.SelectedRows[0]))
                return;

            if (!CheckDataItem())
                return;

            if (dataGridView1.SelectedRows[0].Cells["供应商"].Value.ToString() != txtProvider.Text)
            {
                if (m_goodsServer.IsExist(txtBill_ID.Text))
                {
                    MessageDialog.ShowPromptMessage("已经设置好物品清单时不允许再修改供应商");
                    return;
                }
            }

            if (dataGridView1.SelectedRows[0].Cells["单据类型"].Value.ToString() != cmbBillType.Text)
            {
                if (m_goodsServer.IsExist(txtBill_ID.Text))
                {
                    MessageDialog.ShowPromptMessage("已经设置好物品清单时不允许再修改单据类型");
                    return;
                }
            }

            S_MaterialRejectBill bill = new S_MaterialRejectBill();

            bill.Bill_ID = txtBill_ID.Text;
            bill.Bill_Time = ServerModule.ServerTime.Time;
            bill.Provider = txtProvider.Text;
            bill.Reason = txtReason.Text;
            bill.Remark = txtRemark.Text;
            bill.BillType = cmbBillType.Text;
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

            MaterialRejectBillBillStatus status =
                (MaterialRejectBillBillStatus)Enum.Parse(typeof(MaterialRejectBillBillStatus),
                UniversalFunction.GetBillStatus("S_MaterialRejectBill", "BillStatus", "Bill_ID", txtBill_ID.Text));
                //(MaterialRejectBillBillStatus)Enum.Parse(typeof(MaterialRejectBillBillStatus), dataGridView1.SelectedRows[0].Cells["单据状态"].Value.ToString());

            if (status == MaterialRejectBillBillStatus.已完成)
            {
                MessageDialog.ShowPromptMessage("请选择未完成的记录后再进行此操作");
                return;
            }

            if (!CheckUserOperation(dataGridView1.SelectedRows[0]))
                return;

            string billNo = txtBill_ID.Text;
            string info = string.Format("您是否要删除 {0} 退货单时, 删除时同时也会删除此退货单下的所有物品清单, 是否继续？", billNo);

            if (MessageDialog.ShowEnquiryMessage(info) == DialogResult.No)
            {
                return;
            }

            if (!m_billServer.DeleteBill(billNo, out m_queryResult, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            m_billNoControl.CancelBill(txtBill_ID.Text);
            DestroyMessage(billNo);
            RefreshDataGridView(m_queryResult);
        }

        private void 批准退货ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
                return;

            if (lblBillStatus.Text != MaterialRejectBillBillStatus.等待财务审核.ToString())
            {
                MessageDialog.ShowPromptMessage("请选择要财务审核的记录后再进行此操作");
                return;
            }

            string billNo = txtBill_ID.Text;

            if (!m_billServer.FinanceAuthorizeBill(billNo, BasicInfo.LoginName, out m_queryResult, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            MessageDialog.ShowPromptMessage("已经批准,等待仓管确认!");

            m_billMessageServer.PassFlowMessage(billNo, string.Format("【供应商】：{0}  【退货原因】: {1}   ※※※ 等待【仓管】处理",
                txtProvider.Text, txtReason.Text),
                        m_billMessageServer.GetRoleStringForStorage(cmbStorage.Text));

            RefreshDataGridView(m_queryResult);
            
            PositioningRecord(billNo);
        }

        private void 核实退货清单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
                return;

            if (lblBillStatus.Text != MaterialRejectBillBillStatus.等待仓管退货.ToString())
            {
                MessageDialog.ShowPromptMessage("请选择要仓管退货的记录后再进行此操作");
                return;
            }

            FormMaterialListRejectBill form =
                new FormMaterialListRejectBill(CE_BusinessOperateMode.仓库核实, txtProvider.Text, txtBill_ID.Text);
            form.ShowDialog();
        }

        private void 仓管退货ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
                return;

            if (lblBillStatus.Text != MaterialRejectBillBillStatus.等待仓管退货.ToString())
            {
                MessageDialog.ShowPromptMessage("请选择要仓管退货的记录后再进行此操作");
                return;
            }

            if (MessageDialog.ShowEnquiryMessage("您是否已经审核过退货清单？") == DialogResult.No)
            {
                return;
            }

            if (!m_billServer.FinishBill(txtBill_ID.Text, BasicInfo.LoginName, out m_queryResult, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            string billNo = txtBill_ID.Text;

            EndFlowMessage(billNo, string.Format("{0}号采购退货单已完成", billNo));
            MessageDialog.ShowPromptMessage("已成功退货");
            RefreshDataGridView(m_queryResult);
            PositioningRecord(billNo);
        }

        private void 表单打印ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
                return;

            if (lblBillStatus.Text != MaterialRejectBillBillStatus.已完成.ToString())
            {
                MessageDialog.ShowPromptMessage("请选择已完成的记录后再进行此操作");
                return;
            }

            报表_采购退货单 report = new 报表_采购退货单(txtBill_ID.Text, labelTitle.Text);
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

            txtBill_ID.Text = (string)row.Cells["退货单号"].Value;
            dateTime_BillTime.Value = (DateTime)row.Cells["退货时间"].Value;
            txtProposer.Text = (string)row.Cells["申请人"].Value;
            txtDepartment.Text = (string)row.Cells["申请部门名称"].Value;
            txtDepartment.Tag = (string)row.Cells["申请部门编码"].Value;
            txtProvider.Text = (string)row.Cells["供应商"].Value;
            cmbBillType.Text = (string)row.Cells["单据类型"].Value;
            lblBillStatus.Text = (string)row.Cells["单据状态"].Value;
            cmbStorage.Text = UniversalFunction.GetStorageName(row.Cells["库房代码"].Value.ToString());

            if (row.Cells["备注"].Value != System.DBNull.Value)
                txtRemark.Text = (string)row.Cells["备注"].Value;
            else
                txtRemark.Text = "";

            if (row.Cells["退货原因"].Value != System.DBNull.Value)
                txtReason.Text = (string)row.Cells["退货原因"].Value;
            else
                txtReason.Text = "";

            if (row.Cells["财务签名"].Value != System.DBNull.Value)
                txtFinanceSignatory.Text = (string)row.Cells["财务签名"].Value;
            else
                txtFinanceSignatory.Text = "";

            if (row.Cells["仓管签名"].Value != System.DBNull.Value)
                txtDepotManager.Text = (string)row.Cells["仓管签名"].Value;
            else
                txtDepotManager.Text = "";

            仓库管理员操作ToolStripMenuItem.Visible = 
                UniversalFunction.CheckStorageAndPersonnel((string)row.Cells["库房代码"].Value);

            上级领导操作ToolStripMenuItem.Visible = 
                UniversalFunction.GetIsManager((string)row.Cells["申请部门编码"].Value, BasicInfo.LoginID) == "" ? false : true;
            审核通过ToolStripMenuItem.Visible = 上级领导操作ToolStripMenuItem.Visible;
        }

        private void 查找ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_formFindCondition == null)
            {
                m_formFindCondition = new FormConditionFind(this, m_findField, labelTitle.Text, labelTitle.Text);
            }

            m_formFindCondition.ShowDialog();
        }

        private void 综合查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IAuthorization authorization = PlatformFactory.GetObject<IAuthorization>();
            string businessID = "采购退货单综合查询";
            IQueryResult qr = authorization.Query(businessID, null, null, 0);
            List<string> lstFindField = new List<string>();

            if (qr.DataCollection == null || qr.DataCollection.Tables.Count == 0)
            {
                return;
            }

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

        private void menuItemReresh_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void 设置数据过滤器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormFilterCondition form = new FormFilterCondition(labelTitle.Text, m_findField, labelTitle.Text);
            form.ShowDialog();
            RefreshData();
        }

        private void 查看领料清单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormMaterialListRejectBill form =
                new FormMaterialListRejectBill(CE_BusinessOperateMode.查看, txtProvider.Text, txtBill_ID.Text);
            form.ShowDialog();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
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

        private void 报废退货查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            退货业务报废物品筛选窗体 Form = new 退货业务报废物品筛选窗体("");
            Form.ShowDialog();
        }

        private void 隔离退货查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            对应的隔离单 Form = new 对应的隔离单("",cmbStorage.Text);
            Form.ShowDialog();
        }

        private void 回退单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (财务操作ToolStripMenuItem.Visible == true
                && lblBillStatus.Text == "等待财务审核")
            {
                ReturnBillStatus();
            }

            if (仓库管理员操作ToolStripMenuItem.Visible == true
                && lblBillStatus.Text == "等待仓管退货")
            {
                ReturnBillStatus();
            }
        }

        private void ReturnBillStatus()
        {
            if (lblBillStatus.Text != "已完成")
            {
                回退单据 form = new 回退单据(CE_BillTypeEnum.采购退货单, txtBill_ID.Text, lblBillStatus.Text);

                if (form.ShowDialog() == DialogResult.OK)
                {
                    if (MessageBox.Show("您确定要回退此单据吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
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

                    RefreshDataGridView(m_queryResult);
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
            }
        }

        private void 审核通过ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
                return;

            if (lblBillStatus.Text != MaterialRejectBillBillStatus.等待上级领导审核.ToString())
            {
                MessageDialog.ShowPromptMessage("请选择要上级领导审核的记录后再进行此操作");
                return;
            }

            string billNo = txtBill_ID.Text;

            if (!m_billServer.AuditBill(billNo, out m_queryResult, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            MessageDialog.ShowPromptMessage("已经审核,等待财务审核!");

            m_billMessageServer.PassFlowMessage(billNo, string.Format("【供应商】：{0}  【退货原因】: {1}   ※※※ 等待【会计】处理", txtProvider.Text, txtReason.Text), CE_RoleEnum.会计);

            RefreshDataGridView(m_queryResult);

            PositioningRecord(billNo);
        }
    }
}
