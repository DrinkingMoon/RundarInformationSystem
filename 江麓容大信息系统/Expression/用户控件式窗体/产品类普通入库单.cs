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
using CommonBusinessModule;

namespace Expression
{
    /// <summary>
    /// 普通入库单界面
    /// </summary>
    public partial class 产品类普通入库单 : Form
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
        /// 普通入库单服务组件
        /// </summary>
        IOrdinaryInDepotBillServer m_billServer = ServerModuleFactory.GetServerModule<IOrdinaryInDepotBillServer>();

        /// <summary>
        /// 物品清单服务组件
        /// </summary>
        IOrdinaryInDepotGoodsBill m_goodsServer = ServerModuleFactory.GetServerModule<IOrdinaryInDepotGoodsBill>();

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

        #endregion 成员变量

        public 产品类普通入库单(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_billMessageServer.BillType = "普通入库单";

            m_billNoControl = new BillNumberControl(labelTitle.Text, m_billServer);

            string[] strBillStatus = { "全部", 
                                         OrdinaryInDepotBillStatus.新建单据.ToString(), 
                                         OrdinaryInDepotBillStatus.等待工装验证报告.ToString(), 
                                     OrdinaryInDepotBillStatus.等待质检.ToString(), 
                                     OrdinaryInDepotBillStatus.等待入库.ToString(), 
                                     OrdinaryInDepotBillStatus.已入库.ToString()};

            checkBillDateAndStatus1.InsertComBox(strBillStatus);
            checkBillDateAndStatus1.OnCompleteSearch +=
                new GlobalObject.DelegateCollection.NonArgumentHandle(checkBillDateAndStatus1_OnCompleteSearch);

            checkBillDateAndStatus1.dtpStartTime.Value = ServerTime.Time.AddDays(1).AddMonths(-1);
            checkBillDateAndStatus1.dtpEndTime.Value = ServerTime.Time.AddDays(1);

            m_authorityFlag = nodeInfo.Authority;

            btnRefresh_Click(null, null);
            DataTable dt = UniversalFunction.GetStorageTb();
            dt = DataSetHelper.SiftDataTable(dt, "ZeroCostFlag = 0", out m_error);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cmbStorage.Items.Add(dt.Rows[i]["StorageName"].ToString());
            }
            cmbStorage.SelectedIndex = -1;
        }

        private void 产品类普通入库单_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authorityFlag);
        }

        void checkBillDateAndStatus1_OnCompleteSearch()
        {
            btnRefresh_Click(null, null);
        }

        private void 产品类普通入库单_Resize(object sender, EventArgs e)
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

                    DataTable dtMessage = UniversalFunction.PositioningOneRecord(msg.MessageContent, "普通入库单");

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
        /// 清除窗体上的控件残留信息
        /// </summary>
        void ClearForm()
        {
            cmbStorage.SelectedIndex = -1;
            lblBillStatus.Text = "";
            chkQualityAffirmance.Checked = false;
            chkNeedMachineManager.Checked = false;
            radNo.Checked = false;
            radYes.Checked = false;
            chkAllowInDepot.Checked = false;
            txtMachineValidationID.Text = "";
            cmbDeviceType.SelectedIndex = -1;

            txtOrderFormNumber.Text = "";
            txtBill_ID.Text = "";
            dateTime_BillTime.Value = ServerModule.ServerTime.Time;     // 入库日期
            txtProvider.Text = "";                                      // 供应商编码

            btnFindMaterialType.Enabled = true;
            txtMaterialType.Text = "";                                  // 仓库名就是材料类别
            txtRemark.Text = "";

            txtBuyer.Text = "";
            txtProposer.Text = "";
            txtDesigner.Text = "";
            txtChecker.Text = "";
            txtDepotManager.Text = "";
            txtMachineManager.Text = "";

            txtProvider.Tag = 0;
        }

        #region 数据检测

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
        /// 检查用户对指定行记录是否有操作许可
        /// </summary>
        /// <param name="row">要操作的行记录</param>
        /// <returns>允许返回true</returns>
        bool CheckUserOperation(DataGridViewRow row)
        {
            if ((string)row.Cells["采购员编码"].Value.ToString().Trim() == BasicInfo.LoginID)
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
        /// 检测有关采购数据项内容是否正确
        /// </summary>
        /// <returns>返回检测结果</returns>
        bool CheckDataItem()
        {
            if (txtOrderFormNumber.Text == "")
            {
                txtOrderFormNumber.Focus();
                MessageDialog.ShowPromptMessage("订单号不允许为空!");
                return false;
            }

            if (txtMaterialType.Text.Length == 0)
            {
                txtMaterialType.Focus();
                MessageDialog.ShowPromptMessage("材料类别不允许为空!");
                return false;
            }

            if (txtProposer.Text == "")
            {
                txtProposer.Focus();
                MessageDialog.ShowPromptMessage("请指定物品申请人!");
                return false;
            }

            if (chkNeedMachineManager.Checked)
            {
                if (cmbDeviceType.SelectedIndex == -1)
                {
                    cmbDeviceType.Focus();
                    MessageDialog.ShowPromptMessage("请选择工装类别!");
                    return false;
                }
            }

            if (m_goodsServer.IsExistFrock(txtBill_ID.Text) && txtMaterialType.Text != "工装")
            {
                MessageDialog.ShowPromptMessage("入库物品中包含工装，【材料类别】需选择【工装】");
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

        #endregion 数据检测

        #region 刷新数据

        /// <summary>
        /// 刷新DataGridView
        /// </summary>
        /// <param name="findBill">数据集</param>
        void RefreshDataGridView(IQueryResult findBill)
        {
            ClearForm();
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
                panelPara.Controls.Add(m_dataLocalizer);
                m_dataLocalizer.Dock = DockStyle.Bottom;
            }

            this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            dataGridView1.Columns["材料编码"].Visible = false;
            dataGridView1.Columns["采购员编码"].Visible = false;
            dataGridView1.Columns["质检员编码"].Visible = false;
            dataGridView1.Columns["仓管员编码"].Visible = false;
            dataGridView1.Columns["申请人编码"].Visible = false;
            dataGridView1.Columns["设计人编码"].Visible = false;
            dataGridView1.Columns["工装管理员编码"].Visible = false;
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
                if ((string)dataGridView1.Rows[i].Cells["入库单号"].Value == billNo)
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
            Flow_BillFlowMessage msg = m_billFlowMsg.GetMessage(BasicInfo.LoginID, labelTitle.Text, txtBill_ID.Text);

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
            SendFinishedFlagToMessagePromptForm(txtBill_ID.Text);
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
            Flow_BillFlowMessage msg = m_billFlowMsg.GetMessage(BasicInfo.LoginID, labelTitle.Text, txtBill_ID.Text);

            if (msg == null)
            {
                return;
            }

            m_billFlowMsg.EndMessage(BasicInfo.LoginID, msg.序号, msgContent);
            SendFinishedFlagToMessagePromptForm(txtBill_ID.Text);

            #region 发送知会消息

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} 号普通入库单已经处理完毕, ", msg.单据流水号);

            List<string> noticeRoles = new List<string>();
            noticeRoles.Add(CE_RoleEnum.SQE组长.ToString());
            noticeRoles.Add(CE_RoleEnum.SQE组员.ToString());
            noticeRoles.Add(CE_RoleEnum.质量工程师.ToString());
            noticeRoles.Add(CE_RoleEnum.采购主管.ToString());

            List<string> noticeUsers = new List<string>();
            noticeUsers.Add(msg.初始发起方用户编码);

            m_billMessageServer.NotifyMessage(msg.单据类型, msg.单据号, sb.ToString(), BasicInfo.LoginID, noticeRoles, noticeUsers);

            #endregion 发送知会消息
        }

        #endregion 流消息

        /// <summary>
        /// 使能或禁止采购员编辑
        /// </summary>
        /// <param name="enable">使能与否标志</param>
        private void EnableBuyerEdit(bool enable)
        {
            btnFindOrderForm.Enabled = enable;
            btnFindMaterialType.Enabled = enable;
            btnFindProposer.Enabled = enable;
            btnFindDesigner.Enabled = enable;
            chkQualityAffirmance.Enabled = enable;
            chkNeedMachineManager.Enabled = enable;
        }

        private void 新建单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                if (dataGridView1.SelectedRows[0].Cells["单据状态"].Value.ToString() != OrdinaryInDepotBillStatus.新建单据.ToString())
                {
                    if (lblBillStatus.Text == OrdinaryInDepotBillStatus.新建单据.ToString())
                    {
                        MessageDialog.ShowPromptMessage("现在已经处于新建单据状态");
                        return;
                    }
                }

                if (BasicInfo.LoginID == (string)dataGridView1.SelectedRows[0].Cells["采购员编码"].Value)
                {
                    if (lblBillStatus.Text == OrdinaryInDepotBillStatus.新建单据.ToString())
                    {
                        MessageDialog.ShowPromptMessage("现在已经处于新建单据状态");
                        return;
                    }
                }
            }
            else
            {
                if (lblBillStatus.Text == OrdinaryInDepotBillStatus.新建单据.ToString())
                {
                    MessageDialog.ShowPromptMessage("现在已经处于新建单据状态");
                    return;
                }
            }

            ClearForm();
            EnableBuyerEdit(true);
            txtBill_ID.Text = m_billNoControl.GetNewBillNo();

            dateTime_BillTime.Value = ServerModule.ServerTime.Time;
            lblBillStatus.Text = OrdinaryInDepotBillStatus.新建单据.ToString();
        }

        private void 设置物品清单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string billNo = txtBill_ID.Text;

            try
            {
                this.dataGridView1.CellEnter -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);

                if (lblBillStatus.Text != MaterialRequisitionBillStatus.新建单据.ToString())
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
                    S_OrdinaryInDepotBill bill = new S_OrdinaryInDepotBill();

                    bill.Bill_ID = txtBill_ID.Text;
                    bill.Bill_Time = dateTime_BillTime.Value;               // 入库日期
                    bill.OrderBill_ID = txtOrderFormNumber.Text;            // 订单号
                    bill.Cess = Convert.ToInt32(txtProvider.Tag);
                    bill.NeedQualityAffirmance = chkQualityAffirmance.Checked;  // 需质检确认
                    bill.NeedMachineManager = chkNeedMachineManager.Checked;
                    bill.AskDepartment = txtAskDepartment.Text;

                    if (bill.NeedMachineManager)
                    {
                        bill.DeviceType = cmbDeviceType.Text;
                    }

                    bill.QualityEligibilityFlag = false;
                    bill.MachineValidationID = "";
                    bill.BuyerCode = BasicInfo.LoginID;                     // 采购员编码
                    bill.Proposer = txtProposer.Tag as string;
                    bill.Checker = "0000";
                    bill.MachineManager = "0000";
                    bill.DepotManager = "0000";
                    bill.StorageID = UniversalFunction.GetStorageID(cmbStorage.Text);

                    if (txtDesigner.Text == "")
                    {
                        bill.Designer = "0000";
                    }
                    else
                    {
                        bill.Designer = txtDesigner.Tag as string;
                    }

                    bill.Provider = txtProvider.Text;                       // 供应商编码
                    bill.Depot = txtMaterialType.Tag as string;             // 仓库名就是材料类别
                    bill.Remark = txtRemark.Text;
                    bill.BillStatus = OrdinaryInDepotBillStatus.新建单据.ToString();

                    if (!m_billServer.AddBill(bill, out m_queryResult, out m_error))
                    {
                        MessageDialog.ShowErrorMessage(m_error);
                        return;
                    }

                }

                普通入库物品清单 form = new 普通入库物品清单(CE_BusinessOperateMode.修改, txtBill_ID.Text);
                form.ShowDialog();

                if (!m_goodsServer.IsExist(txtBill_ID.Text))
                {
                    if (MessageDialog.ShowEnquiryMessage("您没有设置物品清单，是否要删除已经创建的单据？") == DialogResult.Yes)
                    {
                        if (!m_billServer.DeleteBill(txtBill_ID.Text, out m_queryResult, out m_error))
                        {
                            MessageDialog.ShowErrorMessage(m_error);
                        }
                        else
                        {
                            RefreshDataGridView(m_queryResult);
                        }
                    }
                }
            }
            finally
            {
                this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
            }

            RefreshDataGridView(m_queryResult);
            PositioningRecord(billNo);
        }

        private void 采购员提交单据ToolStripMenuItem_Click(object sender, EventArgs e)
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

            if (!CheckDataItem())
            {
                return;
            }

            if (!CheckUserOperation(dataGridView1.SelectedRows[0]))
                return;

            string billNo = txtBill_ID.Text;

            if (!m_goodsServer.IsExist(billNo))
            {
                MessageDialog.ShowPromptMessage("您还未设置物品清单，无法提交");
                return;
            }

            IPersonnelInfoServer server = ServerModuleFactory.GetServerModule<IPersonnelInfoServer>();
            OrdinaryInDepotBillStatus status;
            CE_RoleEnum role;

            string flowMsg = null;
            string msg = null;


            if (txtMaterialType.Text == CE_GoodsType.工装.ToString())
            {
                msg = "成功提交,等待工艺人员检验!";
                role = CE_RoleEnum.未知;//工艺人员要求不看信息
                status = OrdinaryInDepotBillStatus.等待工装验证报告;
                flowMsg = string.Format("【订单号】:{0} 【物品申请人】:{1}   ※※※ 等待【工艺人员】处理",
                    txtOrderFormNumber.Text, txtProposer.Text);
            }
            else
            {
                if (chkQualityAffirmance.Checked)
                {
                    msg = "成功提交,等待质量验收!";
                    role = CE_RoleEnum.检验员;
                    status = OrdinaryInDepotBillStatus.等待质检;
                    flowMsg = string.Format("【订单号】:{0} 【物品申请人】:{1}   ※※※ 等待【质检员】处理",
                        txtOrderFormNumber.Text, txtProposer.Text);
                }
                else
                {
                    msg = "成功提交,等待仓管入库!";
                    role = m_billMessageServer.GetRoleStringForStorage(cmbStorage.Text);
                    status = OrdinaryInDepotBillStatus.等待入库;
                    flowMsg = string.Format("【订单号】:{0} 【物品申请人】:{1}   ※※※ 等待【仓管员】处理",
                        txtOrderFormNumber.Text, txtProposer.Text);
                }
            }

            if (!m_billServer.SubmitNewBill(billNo, status, out m_queryResult, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            m_billMessageServer.DestroyMessage(billNo);
            SendNewFlowMessage(billNo, role, flowMsg);
            RefreshDataGridView(m_queryResult);
            PositioningRecord(billNo);

            MessageDialog.ShowPromptMessage(msg);
        }

        private void 质量验收ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
            {
                return;
            }

            OrdinaryInDepotBillStatus status = (OrdinaryInDepotBillStatus)Enum.Parse(
                typeof(OrdinaryInDepotBillStatus), dataGridView1.SelectedRows[0].Cells["单据状态"].Value.ToString());

            if (status != OrdinaryInDepotBillStatus.等待质检)
            {
                MessageDialog.ShowPromptMessage("请选择等待质检的记录后再进行此操作");
                return;
            }

            if (!radYes.Checked && !radNo.Checked)
            {
                MessageDialog.ShowPromptMessage("请选择质量是否合格后再进行此操作");
                return;
            }

            string billNo = txtBill_ID.Text;

            S_OrdinaryInDepotBill bill = new S_OrdinaryInDepotBill();

            bill.Bill_ID = billNo;
            bill.QualityEligibilityFlag = radYes.Checked;
            bill.Checker = BasicInfo.LoginID;
            bill.MassCheckDate = ServerTime.Time;

            if (chkNeedMachineManager.Checked)
            {
                bill.BillStatus = OrdinaryInDepotBillStatus.等待工装验证.ToString();
            }
            else
            {
                bill.BillStatus = OrdinaryInDepotBillStatus.等待入库.ToString();
            }

            if (!m_billServer.SubmitQualityInfo(bill, out m_queryResult, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            StringBuilder sb = new StringBuilder();

            if (chkNeedMachineManager.Checked)
            {
                sb.AppendFormat("【订单号】:{0} 【物品申请人】:{1}   ※※※ 等待【工装管理员】处理",
                        txtOrderFormNumber.Text, txtProposer.Text);
                PassFlowMessage(sb.ToString(), CE_RoleEnum.工装管理员);

                MessageDialog.ShowPromptMessage("成功提交,等待工装验证!");
            }
            else
            {
                sb.AppendFormat("【订单号】:{0} 【物品申请人】:{1}   ※※※ 等待【仓管员】处理",
                        txtOrderFormNumber.Text, txtProposer.Text);
                PassFlowMessage(sb.ToString(),
                        m_billMessageServer.GetRoleStringForStorage(cmbStorage.Text));

                MessageDialog.ShowPromptMessage("成功提交,等待仓管入库!");
            }

            RefreshDataGridView(m_queryResult);
            PositioningRecord(billNo);
        }

        private void 物品入库ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
            {
                return;
            }

            if (dataGridView1.SelectedRows[0].Cells["单据状态"].Value.ToString() != OrdinaryInDepotBillStatus.等待入库.ToString())
            {
                MessageDialog.ShowPromptMessage("请选择等待入库的记录后再进行此操作！");
                return;
            }

            if (MessageDialog.ShowEnquiryMessage("您确定已经核实好物品清单了吗？") == DialogResult.No)
            {
                return;
            }

            string billNo = txtBill_ID.Text;

            S_OrdinaryInDepotBill inDepotInfo = new S_OrdinaryInDepotBill();

            inDepotInfo.Bill_ID = billNo;
            inDepotInfo.DepotManager = BasicInfo.LoginID;
            inDepotInfo.BillStatus = CheckInDepotBillStatus.已入库.ToString();
            inDepotInfo.InDepotDate = ServerTime.Time;

            if (!m_billServer.SubmitInDepotInfo(inDepotInfo, out m_queryResult, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} 号单据已经入库, ", billNo);

            EndFlowMessage(sb.ToString());
            RefreshDataGridView(m_queryResult);
            PositioningRecord(billNo);

            MessageDialog.ShowPromptMessage("成功将零件入库!");
        }

        private void btnFindOrderForm_Click(object sender, EventArgs e)
        {
            if (lblBillStatus.Text != OrdinaryInDepotBillStatus.新建单据.ToString())
            {
                return;
            }

            FormQueryInfo form = QueryInfoDialog.GetOrderFormInfoDialog(CE_BillTypeEnum.普通入库单);

            if (DialogResult.OK == form.ShowDialog())
            {
                txtOrderFormNumber.Text = form.GetDataItem("订单号").ToString();
                txtProvider.Text = form.GetDataItem("供货单位").ToString();

                IOrderFormInfoServer orderFormServer = ServerModuleFactory.GetServerModule<IOrderFormInfoServer>();
                IBargainInfoServer bargainServer = ServerModuleFactory.GetServerModule<IBargainInfoServer>();
            }
        }

        private void btnFindMaterialType_Click(object sender, EventArgs e)
        {
            if (lblBillStatus.Text != OrdinaryInDepotBillStatus.新建单据.ToString())
            {
                return;
            }

            FormDepotType form = new FormDepotType();

            if (form.ShowDialog() == DialogResult.OK)
            {
                txtMaterialType.Text = form.SelectedDepotType.仓库名称;
                txtMaterialType.Tag = form.SelectedDepotType.仓库编码;
            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            查看物品清单ToolStripMenuItem.PerformClick();
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

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            ClearForm();

            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

            txtBill_ID.Text = (string)row.Cells["入库单号"].Value;
            txtOrderFormNumber.Text = (string)row.Cells["订单号"].Value;
            dateTime_BillTime.Value = row.Cells["入库时间"].Value.ToString() == "" ? ServerTime.Time : (DateTime)row.Cells["入库时间"].Value;
            txtProvider.Text = (string)row.Cells["供应商编码"].Value;
            txtBuyer.Text = (string)row.Cells["采购员签名"].Value;
            txtProposer.Text = (string)row.Cells["申请人"].Value;
            txtProposer.Tag = (string)row.Cells["申请人编码"].Value;

            txtMaterialType.Text = (string)row.Cells["材料类别"].Value;
            txtMaterialType.Tag = (string)row.Cells["材料编码"].Value;

            txtChecker.Text = (string)row.Cells["质量签名"].Value;
            txtChecker.Tag = (string)row.Cells["质检员编码"].Value;

            txtMachineManager.Text = (string)row.Cells["工装管理员签名"].Value;
            txtMachineManager.Tag = (string)row.Cells["工装管理员编码"].Value;

            txtDesigner.Text = (string)row.Cells["设计工程师"].Value;
            txtDesigner.Tag = (string)row.Cells["设计人编码"].Value;
            cmbStorage.Text = UniversalFunction.GetStorageName(row.Cells["库房代码"].Value.ToString());

            chkAllowInDepot.Checked = (bool)row.Cells["是否允许入库"].Value;

            if (row.Cells["工装验证报告编号"].Value != System.DBNull.Value)
            {
                txtMachineValidationID.Text = (string)row.Cells["工装验证报告编号"].Value;
            }

            if (row.Cells["工装类别"].Value != System.DBNull.Value)
            {
                cmbDeviceType.Text = (string)row.Cells["工装类别"].Value;
            }

            if (row.Cells["申请部门"].Value != System.DBNull.Value)
            {
                txtAskDepartment.Text = (string)row.Cells["申请部门"].Value;
            }

            if (row.Cells["质检合格标志"].Value != System.DBNull.Value)
            {
                if ((bool)row.Cells["质检合格标志"].Value)
                {
                    radYes.Checked = true;
                }
                else
                {
                    radNo.Checked = true;
                }
            }

            txtDepotManager.Text = (string)row.Cells["仓管签名"].Value;
            chkQualityAffirmance.Checked = (bool)row.Cells["需质检员确认"].Value;
            chkNeedMachineManager.Checked = (bool)row.Cells["需工装管理员验证"].Value;

            if (row.Cells["备注"].Value != System.DBNull.Value)
                txtRemark.Text = (string)row.Cells["备注"].Value;


            lbSumJe.Text = m_goodsServer.GetSumJE(txtBill_ID.Text);

            lblBillStatus.Text = (string)row.Cells["单据状态"].Value;

            仓库管理员操作ToolStripMenuItem.Visible = UniversalFunction.CheckStorageAndPersonnel(
                (string)row.Cells["库房代码"].Value);

            if (lblBillStatus.Text != OrdinaryInDepotBillStatus.新建单据.ToString())
            {
                EnableBuyerEdit(false);

                if (lblBillStatus.Text == OrdinaryInDepotBillStatus.等待质检.ToString())
                {
                    radNo.Enabled = true;
                    radYes.Enabled = true;
                }
                else
                {
                    radNo.Enabled = false;
                    radYes.Enabled = false;
                }

                if (lblBillStatus.Text == OrdinaryInDepotBillStatus.等待工装验证.ToString())
                {
                    txtMachineValidationID.ReadOnly = false;
                    chkAllowInDepot.Enabled = true;
                }
                else
                {
                    txtMachineValidationID.ReadOnly = true;
                    chkAllowInDepot.Enabled = false;
                }
            }
            else
            {
                EnableBuyerEdit(true);
            }
        }

        private void 修正材料类别ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
            {
                return;
            }

            if (dataGridView1.SelectedRows[0].Cells["单据状态"].Value.ToString() != OrdinaryInDepotBillStatus.等待入库.ToString())
            {
                MessageDialog.ShowPromptMessage("请选择等待入库的记录后再进行此操作！");
                return;
            }

            if (MessageDialog.ShowEnquiryMessage("此操作将会修改此单据所有物品在计划价格表中的材料类别，是否继续？") == DialogResult.No)
            {
                return;
            }

            FormDepotType form = new FormDepotType();
            string billNo = txtOrderFormNumber.Text;

            if (form.ShowDialog() == DialogResult.OK)
            {
                txtMaterialType.Text = form.SelectedDepotType.仓库名称;
                txtMaterialType.Tag = form.SelectedDepotType.仓库编码;

                if (!m_billServer.UpdateGoodsType(txtBill_ID.Text, form.SelectedDepotType.仓库编码, out m_queryResult, out m_error))
                {
                    MessageDialog.ShowErrorMessage(m_error);
                }
                else
                {
                    RefreshDataGridView(m_queryResult);
                    PositioningRecord(billNo);
                }
            }
        }

        private void 打印单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择至少一条已入库记录后再行此操作");
                return;
            }

            int index = 0;

            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                if (row.Cells["单据状态"].Value.ToString() != OrdinaryInDepotBillStatus.已入库.ToString())
                {
                    continue;
                }

                IBillReportInfo report = null;

                if ((bool)row.Cells["需工装管理员验证"].Value)
                {
                    report = new 报表_普通入库单_工装验证类(row.Cells[0].Value.ToString(), labelTitle.Text, true);
                }
                else
                {
                    report = new 报表_普通入库单(row.Cells[0].Value.ToString(), labelTitle.Text, true);
                }

                PrintReportBill print = new PrintReportBill(21.8, 9.31, report);

                if (index++ > 0)
                {
                    print.ShowPrintDialog = false;
                }

                print.DirectPrintReport();
            }
        }

        private void 查找ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_formFindCondition == null)
            {
                m_formFindCondition = new FormConditionFind(this, m_findField, labelTitle.Text, labelTitle.Text);
            }

            m_formFindCondition.ShowDialog();
        }

        private void 修改单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
                return;

            if (lblBillStatus.Text != OrdinaryInDepotBillStatus.新建单据.ToString())
            {
                MessageDialog.ShowPromptMessage("您现在不是处于新建单据状态，无法进行此操作");
                return;
            }

            if (!CheckUserOperation(dataGridView1.SelectedRows[0]))
                return;

            if (!CheckDataItem())
                return;

            S_OrdinaryInDepotBill bill = new S_OrdinaryInDepotBill();

            bill.Bill_ID = txtBill_ID.Text;
            bill.Bill_Time = ServerModule.ServerTime.Time;
            //bill.OrderBill_ID = txtOrderFormNumber.Text;
            bill.Depot = txtMaterialType.Tag as string;
            bill.Checker = txtChecker.Tag as string;
            bill.NeedMachineManager = chkNeedMachineManager.Checked;
            bill.NeedQualityAffirmance = chkQualityAffirmance.Checked;
            bill.Remark = txtRemark.Text;
            bill.StorageID = UniversalFunction.GetStorageID(cmbStorage.Text);

            if (txtOrderFormNumber.Text != dataGridView1.SelectedRows[0].Cells["订单号"].Value.ToString())
            {
                if (m_goodsServer.IsExist(txtBill_ID.Text))
                {
                    MessageDialog.ShowPromptMessage("已经设置了物品清单时不允许再修改订单号, 您可以删除物品清单中的所有物品后再进行此操作！");
                    return;
                }
            }

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

            OrdinaryInDepotBillStatus status = (OrdinaryInDepotBillStatus)Enum.Parse(typeof(OrdinaryInDepotBillStatus),
                UniversalFunction.GetBillStatus("S_OrdinaryInDepotBill", "BillStatus", "Bill_ID", txtBill_ID.Text));

            if (status == OrdinaryInDepotBillStatus.已入库 || status == OrdinaryInDepotBillStatus.已报废)
            {
                MessageDialog.ShowPromptMessage("请选择未完成的记录后再进行此操作");
                return;
            }

            if (!CheckUserOperation(dataGridView1.SelectedRows[0]))
                return;

            string info = string.Format("您是否要删除 {0} 入库单时, 删除时同时也会删除此单据下的所有物品清单, 是否继续？", txtBill_ID.Text);

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

        private void 查看物品清单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
            {
                return;
            }

            普通入库物品清单 form = new 普通入库物品清单(CE_BusinessOperateMode.查看, txtBill_ID.Text);
            form.ShowDialog();
        }

        private void 核实物品清单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
            {
                return;
            }

            普通入库物品清单 form = new 普通入库物品清单(CE_BusinessOperateMode.仓库核实, txtBill_ID.Text);
            form.ShowDialog();
        }

        private void btnFindProposer_Click(object sender, EventArgs e)
        {
            FormPersonnel form = new FormPersonnel(txtProposer, "姓名");
            form.ShowDialog();

            txtProposer.Tag = form.UserCode;
        }

        private void btnFindDesigner_Click(object sender, EventArgs e)
        {
            FormPersonnel form = new FormPersonnel(txtDesigner, "姓名");
            form.ShowDialog();

            txtDesigner.Tag = form.UserCode;
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

        private void 提交验证信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
            {
                return;
            }

            OrdinaryInDepotBillStatus status = (OrdinaryInDepotBillStatus)Enum.Parse(
                typeof(OrdinaryInDepotBillStatus), dataGridView1.SelectedRows[0].Cells["单据状态"].Value.ToString());

            if (status != OrdinaryInDepotBillStatus.等待工装验证)
            {
                MessageDialog.ShowPromptMessage("请选择等待工装验证的记录后再进行此操作");
                return;
            }

            if (txtMachineValidationID.Text.Trim() == "")
            {
                txtMachineValidationID.Focus();
                MessageDialog.ShowPromptMessage("请输入工装验证报告编号后再进行此操作");
                return;
            }

            string billNo = txtBill_ID.Text;

            S_OrdinaryInDepotBill bill = new S_OrdinaryInDepotBill();

            bill.Bill_ID = billNo;
            bill.MachineValidationID = txtMachineValidationID.Text;
            bill.MachineManager = BasicInfo.LoginID;
            bill.AllowInDepot = chkAllowInDepot.Checked;
            bill.BillStatus = OrdinaryInDepotBillStatus.等待入库.ToString();

            if (!m_billServer.SubmitMachineValidationInfo(bill, out m_queryResult, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            MessageDialog.ShowPromptMessage("成功提交,等待仓管入库!");

            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("【订单号】:{0} 【物品申请人】:{1}   ※※※ 等待【仓管员】处理",
                        txtOrderFormNumber.Text, txtProposer.Text);

            PassFlowMessage(sb.ToString(),
                        m_billMessageServer.GetRoleStringForStorage(cmbStorage.Text));
            RefreshDataGridView(m_queryResult);
            PositioningRecord(billNo);
        }

        private void 设置数据过滤器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormFilterCondition form = new FormFilterCondition(labelTitle.Text, m_findField, labelTitle.Text);
            form.ShowDialog();

            m_billServer.QueryResultFilter = QueryFilterControl.GetFilterString(labelTitle.Text);

            if (m_billServer.GetAllBill(out m_queryResult, out m_error))
            {
                RefreshDataGridView(m_queryResult);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            m_billServer.QueryResultFilter = QueryFilterControl.GetFilterString(labelTitle.Text)
                + checkBillDateAndStatus1.GetSqlString("单据提交时间", "单据状态");

            if (!m_billServer.GetAllBill(out m_queryResult, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            RefreshDataGridView(m_queryResult);
        }

        private void chkNeedMachineManager_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNeedMachineManager.Checked && !chkQualityAffirmance.Checked)
            {
                MessageDialog.ShowPromptMessage("需要工装管理员验证的物品一定要通过质检");
                chkQualityAffirmance.Checked = true;
            }
        }

        private void chkQualityAffirmance_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkQualityAffirmance.Checked)
            {
                chkNeedMachineManager.Checked = false;
            }
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        private void 综合查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IAuthorization authorization = PlatformFactory.GetObject<IAuthorization>();
            string businessID = "普通入库单综合查询";
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

        private void 回退单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (质检员操作ToolStripMenuItem.Visible == true
                && lblBillStatus.Text == "等待质检")
            {
                ReturnBillStatus();
            }
            else if (仓库管理员操作ToolStripMenuItem.Visible == true
                && lblBillStatus.Text == "等待入库")
            {
                ReturnBillStatus();
            }
            else if (工装管理员操作ToolStripMenuItem.Visible == true
                && lblBillStatus.Text == "等待工装验证")
            {
                ReturnBillStatus();
            }
            else if (BasicInfo.ListRoles.Contains(CE_RoleEnum.工艺人员.ToString())
                && lblBillStatus.Text == "等待工装验证报告")
            {
                ReturnBillStatus();
            }
        }

        /// <summary>
        /// 回退单据
        /// </summary>
        private void ReturnBillStatus()
        {
            if (lblBillStatus.Text != "已入库")
            {
                回退单据 form = new 回退单据(CE_BillTypeEnum.普通入库单, txtBill_ID.Text, lblBillStatus.Text);

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

        private void btnAskDepartment_Click(object sender, EventArgs e)
        {
            FormQueryInfo dialog = QueryInfoDialog.GetDepartment();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtAskDepartment.Text = dialog.GetStringDataItem("部门名称");
            }
        }
    }
}
