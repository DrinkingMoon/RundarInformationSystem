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
using CommonBusinessModule;

namespace Expression
{
    /// <summary>
    /// 自制件入库单界面
    /// </summary>
    public partial class 自制件入库单 : Form
    {
        /// <summary>
        /// 单据消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_msgPromulgator = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 查找到的符合条件的库存信息
        /// </summary>
        IQueryResult m_findBill;

        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 自制件入库单服务组件
        /// </summary>
        IHomemadePartInDepotServer m_billServer = ServerModuleFactory.GetServerModule<IHomemadePartInDepotServer>();

        /// <summary>
        /// 获取基础物品服务组件
        /// </summary>
        IBasicGoodsServer m_basicGoodsServer = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();

        /// <summary>
        /// 获取单据流消息操作接口
        /// </summary>
        IBillFlowMessage m_billFlowMsg = PlatformFactory.GetObject<IBillFlowMessage>();

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
        AuthorityFlag m_authFlag = AuthorityFlag.Nothing;

        /// <summary>
        /// 车间管理基础服务
        /// </summary>
        Service_Manufacture_WorkShop.IWorkShopBasic m_serverWSBasic =
            Service_Manufacture_WorkShop.ServerModuleFactory.GetServerModule<Service_Manufacture_WorkShop.IWorkShopBasic>();

        /// <summary>
        /// 车间管理信息
        /// </summary>
        WS_WorkShopCode m_lnqWSCode = new WS_WorkShopCode();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nodeInfo">功能树节点信息</param>
        public 自制件入库单(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_msgPromulgator.BillType = "自制件入库单";
            m_billNoControl = new BillNumberControl(CE_BillTypeEnum.自制件入库单.ToString(), m_billServer);

            m_authFlag = nodeInfo.Authority;

            cmbStorage.DataSource = UniversalFunction.GetListStorageInfo().Where(k => k.ZeroCostFlag == true).ToList();

            cmbStorage.ValueMember = "StorageID";
            cmbStorage.DisplayMember = "StorageName";
            cmbStorage.SelectedText = CE_StorageName.自制半成品库.ToString();

            //DataTable dt = UniversalFunction.GetStorageTb();

            //DataRow[] drList = dt.Select("StorageID in ('01','03','08','10')");
            //DataTable dtTemp = dt.Clone();

            //foreach (DataRow dr in drList)
            //{
            //    dtTemp.ImportRow(dr);
            //}

            //cmbStorage.DataSource = dtTemp;

            checkBillDateAndStatus1.InsertComBox(typeof(HomemadeBillStatus));

            checkBillDateAndStatus1.OnCompleteSearch +=
                new GlobalObject.DelegateCollection.NonArgumentHandle(checkBillDateAndStatus1_OnCompleteSearch);


            S_HomemadePartBill tempBill = m_billServer.GetBill(txtBill_ID.Text);

            m_lnqWSCode = tempBill == null ?
                m_serverWSBasic.GetPersonnelWorkShop(BasicInfo.LoginID) :
                m_serverWSBasic.GetPersonnelWorkShop(tempBill.DeclarePersonnelCode);

            btnBatchNo.Visible = m_lnqWSCode == null ? false : true;


            #region 被要求使用服务器时间 Modify by cjb on 2012.6.15
            checkBillDateAndStatus1.dtpStartTime.Value = ServerTime.Time.AddDays(1).AddMonths(-1);
            checkBillDateAndStatus1.dtpEndTime.Value = ServerTime.Time.AddDays(1);

            #endregion

            btnRefresh_Click(null, null);

            if (!Convert.ToBoolean(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.开启车间管理模块]))
            {
                btnBatchNo.Visible = false;
            }
        }

        private void 自制件入库单_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authFlag);
        }

        void checkBillDateAndStatus1_OnCompleteSearch()
        {
            btnRefresh_Click(null, null);
        }

        private void 自制件入库单_Resize(object sender, EventArgs e)
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

            if (((authorityFlag & AuthorityFlag.ConfirmArrival) != AuthorityFlag.Nothing) ||
                ((authorityFlag & AuthorityFlag.StockIn) != AuthorityFlag.Nothing))
            {
                仓库管理员操作ToolStripMenuItem.Visible = true;
            }
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
                    break;
                case WndMsgSender.PositioningMsg:
                    WndMsgData msg = new WndMsgData();   //这是创建自定义信息的结构
                    Type dataType = msg.GetType();

                    msg = (WndMsgData)m.GetLParam(dataType);//这里获取的就是作为LParam参数发送来的信息的结构

                    DataTable dtMessage = UniversalFunction.PositioningOneRecord(msg.MessageContent, "自制件入库单");

                    if (dtMessage.Rows.Count == 0)
                    {
                        m_msgPromulgator.DestroyMessage(msg.MessageContent);
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
            lblBillStatus.Text = "";
            txtBill_ID.Text = "";
            cmbStorage.SelectedIndex = -1;
            numDeclareCount.Value = 0;                     // 自制件数
            txtProviderBatchNo.Text = "";                  // 供应商批次
            txtCode.Text = "";
            txtName.Text = "";
            txtSpec.Text = "";
            txtUnit.Text = "";                              // 计量单位
            txtMaterialType.Text = "";                      // 仓库名就是材料类别
            txtRemark.Text = "";
            txtChecker.Text = "";
            txtBatchNo.Text = "";                           // xsy

            numUnitPrice.Value = 0;
            numPlanUnitPrice.Value = 0;
            numPrice.Value = 0;

            numInDepotCount.Value = 0;
            txtShelf.Text = "";
            txtColumn.Text = "";
            txtLayer.Text = "";

            dateTime_CheckTime.Value = ServerTime.Time;
            txtQualityInfo.Text = "";
            txtCheckoutReportID.Text = "";
            numEligibleCount.Value = 0;
            numConcessionCount.Value = 0;
            numReimbursementCount.Value = 0;
        }

        private void 新建单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                if (dataGridView1.SelectedRows[0].Cells["单据状态"].Value.ToString() != "新建单据")
                {
                    if (lblBillStatus.Text == "新建单据")
                    {
                        MessageDialog.ShowPromptMessage("现在已经处于新建单据状态");
                        return;
                    }
                }

                if (BasicInfo.LoginID == (string)dataGridView1.SelectedRows[0].Cells["报检人编码"].Value)
                {
                    if (lblBillStatus.Text == "新建单据")
                    {
                        MessageDialog.ShowPromptMessage("现在已经处于新建单据状态");
                        return;
                    }
                }
            }
            else
            {
                if (lblBillStatus.Text == "新建单据")
                {
                    MessageDialog.ShowPromptMessage("现在已经处于新建单据状态");
                    return;
                }
            }

            ClearForm();
            cmbStorage.Text = CE_StorageName.自制半成品库.ToString();
            txtBill_ID.Text = "系统自动生成";

            if (!Convert.ToBoolean(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.开启车间管理模块]))
            {
                txtBatchNo.Text = "系统自动生成";
            }
            
            lblBillStatus.Text = "新建单据";
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
            if ((string)row.Cells["报检人编码"].Value == BasicInfo.LoginID)
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
            if (numDeclareCount.Value == 0)
            {
                numDeclareCount.Focus();
                MessageDialog.ShowPromptMessage("自制件数不允许为0!");
                return false;
            }

            if (cmbStorage.Text == "")
            {
                cmbStorage.Focus();
                MessageDialog.ShowPromptMessage("所属库房不允许为空!");
                return false;
            }

            if (txtProvider.Text == "")
            {
                txtProvider.Focus();
                MessageDialog.ShowPromptMessage("供货单位不允许为空!,请选择供货单位");
                return false;
            }

            if (txtProviderBatchNo.Text.Trim().Length == 0)
            {
                txtProviderBatchNo.Focus();
                MessageDialog.ShowPromptMessage("供方批次号不允许为空!");
                return false;
            }

            if (txtUnit.Text == "")
            {
                txtUnit.Focus();
                MessageDialog.ShowPromptMessage("单位不允许为空!");
                return false;
            }

            if (txtBatchNo.Text == "")
            {
                txtBatchNo.Focus();
                MessageDialog.ShowPromptMessage("批次号不允许为空!");
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

            #region 隐藏不允许查看的列

            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                if (findBill.HideFields.Contains(dataGridView1.Columns[i].Name))
                {
                    dataGridView1.Columns[i].Visible = false;
                }
            }

            #endregion

            dataGridView1.Columns["物品ID"].Visible = false;

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
        /// <param name="bill">单据内容</param>
        void SendNewFlowMessage(S_HomemadePartBill bill)
        {
            Flow_BillFlowMessage msg = new Flow_BillFlowMessage();

            msg.初始发起方用户编码 = BasicInfo.LoginID;
            msg.单据号 = bill.Bill_ID;
            msg.单据类型 = labelTitle.Text;
            msg.单据流水号 = bill.Bill_ID;
            msg.接收方类型 = BillFlowMessage_ReceivedUserType.角色.ToString();
            msg.单据状态 = BillStatus.等待处理.ToString();

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("物品【{0}】，批次号【{1}】  ※※※ 等待【检验员】处理", txtName.Text, txtBatchNo.Text);

            msg.发起方消息 = sb.ToString();
            msg.接收方 = CE_RoleEnum.自制件检验组长.ToString();

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
            try
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
            catch (Exception exce)
            {
                Console.WriteLine(exce.Message);
            }
        }

        /// <summary>
        /// 回退流消息
        /// </summary>
        /// <param name="msgContent">消息内容</param>
        void RebackFlowMessage(string msgContent)
        {
            try
            {
                Flow_BillFlowMessage msg = m_billFlowMsg.GetMessage(BasicInfo.LoginID, labelTitle.Text, txtBill_ID.Text);

                if (msg == null)
                {
                    return;
                }

                m_billFlowMsg.RebackMessage(BasicInfo.LoginID, msg.序号, msgContent);
                SendFinishedFlagToMessagePromptForm(txtBill_ID.Text);

            }
            catch (Exception exce)
            {
                Console.WriteLine(exce.Message);
            }
        }

        /// <summary>
        /// 结束流消息(流程已经走完)
        /// </summary>
        /// <param name="msgContent">消息内容</param>
        void EndFlowMessage(string msgContent)
        {
            try
            {
                Flow_BillFlowMessage msg = m_billFlowMsg.GetMessage(BasicInfo.LoginID, labelTitle.Text, txtBill_ID.Text);

                if (msg == null)
                {
                    return;
                }

                m_billFlowMsg.EndMessage(BasicInfo.LoginID, msg.序号, msgContent);
                SendFinishedFlagToMessagePromptForm(txtBill_ID.Text);

                #region 发送知会消息

                List<string> noticeRoles = new List<string>();
                noticeRoles.Add(CE_RoleEnum.质量工程师.ToString());

                List<string> noticeUsers = new List<string>();
                noticeUsers.Add(msg.初始发起方用户编码);

                m_msgPromulgator.NotifyMessage(msg.单据类型, msg.单据号, msgContent, BasicInfo.LoginID, noticeRoles, null);
                #endregion 发送知会消息

            }
            catch (Exception exce)
            {
                Console.WriteLine(exce.Message);
            }
        }

        #endregion 流消息

        private void 提交单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckDataItem())
            {
                return;
            }

            if (lblBillStatus.Text != "新建单据")
            {
                MessageDialog.ShowPromptMessage("您现在不是处于新建单据状态，无法提交，如果是回退后重新提交请选择“修改单据”功能");
                return;
            }

            S_HomemadePartBill bill = new S_HomemadePartBill();

            bill.Bill_Time = ServerTime.Time;                       // 报检日期
            bill.BillStatus = HomemadeBillStatus.等待质检.ToString();
            bill.DeclarePersonnelCode = BasicInfo.LoginID;          // 自制件员编码
            bill.DeclarePersonnel = BasicInfo.LoginName;            // 自制件员签名
            bill.DeclareCount = Convert.ToInt32(numDeclareCount.Value);              // 自制件数
            bill.Provider = txtProvider.Tag.ToString();                       // 供应商编码
            bill.ProviderBatchNo = txtProviderBatchNo.Text;       // 供应商批次
            bill.GoodsID = (int)txtCode.Tag;
            bill.Remark = txtRemark.Text;
            bill.BatchNo = txtBatchNo.Text;
            bill.StorageID = UniversalFunction.GetStorageID(cmbStorage.Text);

            if (!m_billServer.AddBill(bill, out m_findBill, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            MessageDialog.ShowPromptMessage("成功提交,等待质检!");

            m_msgPromulgator.DestroyMessage(bill.Bill_ID);
            SendNewFlowMessage(bill);
            RefreshDataGridView(m_findBill);
        }

        /// <summary>
        /// 查询图号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFindCode_Click(object sender, EventArgs e)
        {
            if (lblBillStatus.Text != "新建单据")
            {
                return;
            }

            FormQueryInfo form = QueryInfoDialog.GetPlanCostGoodsDialogSift(" and AttributeID = " 
                + (int)CE_GoodsAttributeName.自制件 + " and AttributeValue = '"+ bool.TrueString +"'");

            if (form != null && form.ShowDialog() == DialogResult.OK)
            {
                txtCode.Text = form.GetDataItem("图号型号").ToString();
                txtName.Text = form.GetDataItem("物品名称").ToString();
                txtSpec.Text = form.GetDataItem("规格").ToString();
                txtUnit.Text = form.GetStringDataItem("单位");
                txtCode.Tag = form.GetDataItem("序号");
                txtMaterialType.Text = form.GetStringDataItem("物品类别");
            }
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
            dateTime_BillTime.Value = (DateTime)row.Cells["报检日期"].Value;
            txtProvider.Text = UniversalFunction.GetProviderInfo((string)row.Cells["供货单位"].Value).供应商名称;
            txtProvider.Tag = (string)row.Cells["供货单位"].Value;
            txtBatchNo.Text = (string)row.Cells["批次号"].Value;

            if (row.Cells["供方批次号"].Value != null)
                txtProviderBatchNo.Text = row.Cells["供方批次号"].Value.ToString();
            else
                txtProviderBatchNo.Text = "";

            txtName.Text = (string)row.Cells["物品名称"].Value;
            txtCode.Text = (string)row.Cells["图号型号"].Value;
            txtCode.Tag = (int)row.Cells["物品ID"].Value;
            txtSpec.Text = (string)row.Cells["规格"].Value;
            txtUnit.Text = (string)row.Cells["单位"].Value;
            cmbStorage.Text = UniversalFunction.GetStorageName(row.Cells["库房代码"].Value.ToString());

            if (row.Cells["检验报告编号"].Value != System.DBNull.Value)
                txtCheckoutReportID.Text = (string)row.Cells["检验报告编号"].Value;
            else
                txtCheckoutReportID.Text = "";

            if (row.Cells["检验入库时间"].Value != System.DBNull.Value)
            {
                dateTime_CheckTime.Value = (DateTime)row.Cells["检验入库时间"].Value;
            }
            else
            {
                dateTime_CheckTime.Value = ServerTime.Time;
            }

            numDeclareCount.Value = (int)row.Cells["报检数量"].Value;
            numEligibleCount.Value = (int)row.Cells["合格数量"].Value;
            numConcessionCount.Value = (int)row.Cells["让步数量"].Value;
            numReimbursementCount.Value = (int)row.Cells["退货数量"].Value;
            numDeclareWastrelCount.Value = (int)row.Cells["报废数量"].Value;

            if (row.Cells["质量信息"].Value != System.DBNull.Value)
                txtQualityInfo.Text = (string)row.Cells["质量信息"].Value;
            else
                txtQualityInfo.Text = "";

            if (row.Cells["检验员"].Value != System.DBNull.Value)
                txtChecker.Text = (string)row.Cells["检验员"].Value;
            else
                txtQualityInfo.Text = "";

            if (row.Cells["仓管签名"].Value != System.DBNull.Value)
                txtDepotManager.Text = (string)row.Cells["仓管签名"].Value;
            else
                txtDepotManager.Text = "";

            if (row.Cells["质量签名"].Value != System.DBNull.Value)
                txtQualityManager.Text = (string)row.Cells["质量签名"].Value;
            else
                txtQualityManager.Text = "";

            if (row.Cells["备注"].Value != System.DBNull.Value)
                txtRemark.Text = (string)row.Cells["备注"].Value;
            else
                txtRemark.Text = "";

            if (dataGridView1.Columns["单价"].Visible)
            {
                numUnitPrice.Value = (decimal)row.Cells["单价"].Value;
            }

            if (dataGridView1.Columns["金额"].Visible)
            {
                numPrice.Value = (decimal)row.Cells["金额"].Value;
            }

            if (dataGridView1.Columns["计划单价"].Visible)
            {
                numPlanUnitPrice.Value = (decimal)row.Cells["计划单价"].Value;
            }

            txtMaterialType.Text = (string)row.Cells["仓库"].Value;

            if (row.Cells["货架"].Value != System.DBNull.Value)
                txtShelf.Text = (string)row.Cells["货架"].Value;
            else
                txtShelf.Text = "";

            if (row.Cells["列"].Value != System.DBNull.Value)
                txtColumn.Text = (string)row.Cells["列"].Value;
            else
                txtColumn.Text = "";

            if (row.Cells["层"].Value != System.DBNull.Value)
                txtLayer.Text = (string)row.Cells["层"].Value;
            else
                txtLayer.Text = "";

            if ((int)row.Cells["入库数量"].Value != 0)
                numInDepotCount.Value = (int)row.Cells["入库数量"].Value;
            else
            {
                numInDepotCount.Value = numEligibleCount.Value + numConcessionCount.Value + numDeclareWastrelCount.Value;
            }

            lblBillStatus.Text = (string)row.Cells["单据状态"].Value;

            仓库管理员操作ToolStripMenuItem.Visible = 
                UniversalFunction.CheckStorageAndPersonnel((string)row.Cells["库房代码"].Value);

            if (row.Cells["回退理由"].Value != System.DBNull.Value)
            {
                lblBackReason.ForeColor = Color.Red;
                lblBackReason.Text = (string)row.Cells["回退理由"].Value;
            }
            else
            {
                lblBackReason.ForeColor = Color.Black;
                lblBackReason.Text = "";
            }
        }

        private void 提交质检信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
            {
                return;
            }

            HomemadeBillStatus status = (HomemadeBillStatus)Enum.Parse(typeof(HomemadeBillStatus),
                dataGridView1.SelectedRows[0].Cells["单据状态"].Value.ToString());

            if (status != HomemadeBillStatus.等待质检 && status != HomemadeBillStatus.回退_质检信息有误)
            {
                MessageDialog.ShowPromptMessage("请选择等待质检的记录后再进行此操作");
                return;
            }

            dateTime_CheckTime.Value = ServerTime.Time;

            if (dateTime_CheckTime.Value < dateTime_BillTime.Value)
            {
                dateTime_CheckTime.Focus();
                MessageDialog.ShowPromptMessage("检验入库时间必须 >= 自制件时间");
                return;
            }

            if (txtQualityInfo.Text == "")
            {
                txtQualityInfo.Focus();
                MessageDialog.ShowPromptMessage("质量信息不能为空");
                return;
            }

            if (txtCheckoutReportID.Text == "")
            {
                txtCheckoutReportID.Focus();
                MessageDialog.ShowPromptMessage("检验报告编号不能为空");
                return;
            }

            if (numEligibleCount.Value + numConcessionCount.Value + numReimbursementCount.Value
                + numDeclareWastrelCount.Value != numDeclareCount.Value)
            {
                numEligibleCount.Focus();
                MessageDialog.ShowPromptMessage("合格数 + 让步数 + 退货数 + 报废数 之和不等于自制件数量");
                return;
            }

            if (txtChecker.Text == "")
            {
                txtChecker.Focus();
                MessageDialog.ShowPromptMessage("检验人员不能为空");

                return;
            }

            S_HomemadePartBill qualityInfo = new S_HomemadePartBill();

            qualityInfo.Checker = txtChecker.Text;
            qualityInfo.QualityInputer = BasicInfo.LoginName;
            qualityInfo.CheckoutJoinGoods_Time = dateTime_CheckTime.Value;
            qualityInfo.CheckoutReport_ID = txtCheckoutReportID.Text;
            qualityInfo.QualityInfo = txtQualityInfo.Text;
            qualityInfo.EligibleCount = Convert.ToInt32(numEligibleCount.Value);
            qualityInfo.ConcessionCount = Convert.ToInt32(numConcessionCount.Value);
            qualityInfo.ReimbursementCount = Convert.ToInt32(numReimbursementCount.Value);
            qualityInfo.DeclareWastrelCount = Convert.ToInt32(numDeclareWastrelCount.Value);
            qualityInfo.BillStatus = HomemadeBillStatus.等待入库.ToString();

            string billNo = txtBill_ID.Text;

            if (!m_billServer.SubmitQualityInfo(billNo, qualityInfo, out m_findBill, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            MessageDialog.ShowPromptMessage("成功提交,等待仓管将零件入库!");

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("物品【{0}】，批次号【{1}】  ※※※ 等待【仓管员】处理", txtName.Text, txtBatchNo.Text);

            PassFlowMessage(sb.ToString(),
                        m_msgPromulgator.GetRoleStringForStorage(cmbStorage.Text));
            RefreshDataGridView(m_findBill);
            PositioningRecord(billNo);
        }

        private void 零件入库ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0 ||
                dataGridView1.SelectedRows[0].Cells["单据状态"].Value.ToString() != HomemadeBillStatus.等待入库.ToString())
            {
                MessageDialog.ShowPromptMessage("请选择等待入库的记录后再进行此操作！");
                return;
            }

            if (dataGridView1.SelectedRows.Count > 1)
            {
                MessageDialog.ShowPromptMessage("只允许选择一条记录进行处理！");
                return;
            }


            if (txtShelf.Text == "")
            {
                txtShelf.Focus();
                MessageDialog.ShowPromptMessage("区域信息不能为空");

                return;
            }

            if (txtColumn.Text == "")
            {
                txtColumn.Focus();
                MessageDialog.ShowPromptMessage("区号信息不能为空");

                return;
            }

            if (txtLayer.Text == "")
            {
                txtLayer.Focus();
                MessageDialog.ShowPromptMessage("层号信息不能为空");

                return;
            }

            S_HomemadePartBill inDepotInfo = new S_HomemadePartBill();

            inDepotInfo.DepotManager = BasicInfo.LoginName;
            inDepotInfo.DepotManagerAffirmCount = Convert.ToInt32(numDeclareCount.Value);
            inDepotInfo.InDepotCount = Convert.ToInt32(numInDepotCount.Value);
            inDepotInfo.ShelfArea = txtShelf.Text;
            inDepotInfo.ColumnNumber = txtColumn.Text;
            inDepotInfo.LayerNumber = txtLayer.Text;
            inDepotInfo.BillStatus = HomemadeBillStatus.已入库.ToString();

            string billNo = txtBill_ID.Text;

            if (!m_billServer.SubmitInDepotInfo(billNo, inDepotInfo, out m_findBill, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("{0} 号自制件入库单仓库已经完成, ", billNo);
            sb.AppendFormat("此单据物品：图号【{0}】，名称【{1}】，规格【{2}】", txtCode.Text, txtName.Text, txtSpec.Text);

            EndFlowMessage(sb.ToString());
            RefreshDataGridView(m_findBill);
            PositioningRecord(billNo);

            MessageDialog.ShowPromptMessage("成功将零件入库!");
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            m_billServer.QueryResultFilter = QueryFilterControl.GetFilterString(labelTitle.Text)
                + checkBillDateAndStatus1.GetSqlString("报检日期", "单据状态");

            if (!m_billServer.GetAllBill(out m_findBill, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            RefreshDataGridView(m_findBill);
        }

        private void 查找ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_formFindCondition != null && !m_formFindCondition.SaveFlag)
            {
                m_formFindCondition = null;
            }

            if (m_formFindCondition == null)
            {
                m_formFindCondition = new FormConditionFind(this, m_findField, labelTitle.Text, labelTitle.Text);
            }

            m_formFindCondition.ShowDialog();
        }

        /// <summary>
        /// 打印条形码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrintBarCode_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count < 1)
            {
                MessageDialog.ShowPromptMessage("请选择至少一条记录后再进行此操作");
                return;
            }

            List<View_S_InDepotGoodsBarCodeTable> lstBarCodeInfo = new List<View_S_InDepotGoodsBarCodeTable>();

            for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
            {
                string goodsCode = dataGridView1.SelectedRows[i].Cells["图号型号"].Value.ToString();
                string goodsName = dataGridView1.SelectedRows[i].Cells["物品名称"].Value.ToString();
                string spec = dataGridView1.SelectedRows[i].Cells["规格"].Value.ToString();
                string provider = dataGridView1.SelectedRows[i].Cells["供货单位"].Value.ToString();
                string batchCode = dataGridView1.SelectedRows[i].Cells["批次号"].Value.ToString();
                string storageID = dataGridView1.SelectedRows[i].Cells["库房代码"].Value.ToString();

                IBarCodeServer server = ServerModuleFactory.GetServerModule<IBarCodeServer>();
                View_S_InDepotGoodsBarCodeTable barcode = server.GetBarCodeInfo(goodsCode, goodsName, spec, provider, batchCode, storageID);
                IBarCodeServer barCodeServer = ServerModuleFactory.GetServerModule<IBarCodeServer>();

                if (barcode == null)
                {
                    S_InDepotGoodsBarCodeTable newBarcode = new S_InDepotGoodsBarCodeTable();

                    newBarcode.GoodsID = m_basicGoodsServer.GetGoodsID(goodsCode, goodsName, spec);
                    newBarcode.Provider = provider;
                    newBarcode.BatchNo = batchCode;
                    newBarcode.ProductFlag = "0";
                    newBarcode.StorageID = storageID;

                    if (!barCodeServer.Add(newBarcode, out m_err))
                    {
                        MessageDialog.ShowErrorMessage(m_err);
                        return;
                    }

                    barcode = barCodeServer.GetBarCodeInfo(goodsCode, goodsName, spec, provider, batchCode, storageID);
                }

                lstBarCodeInfo.Add(barcode);
            }

            foreach (var item in lstBarCodeInfo)
            {
                ServerModule.PrintPartBarcode.PrintBarcodeList(item);
            }
        }

        /// <summary>
        /// 报表（包含金额）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 表单打印ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择一条已入库记录后再行此操作");
                return;
            }

            HomemadeBillStatus status = (HomemadeBillStatus)Enum.Parse(typeof(HomemadeBillStatus),
                dataGridView1.SelectedRows[0].Cells["单据状态"].Value.ToString());

            if (status != HomemadeBillStatus.已入库)
            {
                MessageDialog.ShowPromptMessage("当前单据没有入库不允许进行打印");
                return;
            }

            报表_自制件入库单 report = new 报表_自制件入库单(dataGridView1.CurrentRow.Cells[0].Value.ToString(), labelTitle.Text);
            PrintReportBill print = new PrintReportBill(21.8, 9.31, report);
            print.DirectPrintReport();
        }

        private void 回退ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HomemadeBillStatus status = (HomemadeBillStatus)Enum.Parse(typeof(HomemadeBillStatus),
                dataGridView1.SelectedRows[0].Cells["单据状态"].Value.ToString());

            if (dataGridView1.SelectedRows.Count == 0 || status == HomemadeBillStatus.已入库)
            {
                MessageDialog.ShowPromptMessage("请选择没有入库的记录后再进行此操作！");
                return;
            }
            else if (dataGridView1.SelectedRows.Count > 1)
            {
                MessageDialog.ShowPromptMessage("系统不支持多条记录同时回退,请逐条操作！");
                return;
            }

            string billID = dataGridView1.SelectedRows[0].Cells["入库单号"].Value.ToString();
            bool validInfo = false;

            if (status == HomemadeBillStatus.等待质检 || status == HomemadeBillStatus.回退_质检信息有误)
            {
                if (!BasicInfo.ListRoles.Contains(CE_RoleEnum.自制件检验组长.ToString()))
                {
                    MessageDialog.ShowPromptMessage("您无权回退单据的当前步骤");
                    return;
                }

                validInfo = true;
            }
            else if (status == HomemadeBillStatus.等待入库)
            {
                if (!BasicInfo.ListRoles.Contains(
                        m_msgPromulgator.GetRoleStringForStorage(cmbStorage.Text).ToString()))
                {
                    MessageDialog.ShowPromptMessage("您无权回退单据的当前步骤");
                    return;
                }

                validInfo = true;
            }
            else if (status.ToString().Contains("回退"))
            {
                MessageDialog.ShowPromptMessage("当前记录已经处于回退状态，不能重复回退");
                return;
            }

            if (validInfo)
            {
                string reason = InputBox.ShowDialog("请输入回退原因", "原因：", "");

                if (reason == null || reason == "")
                {
                    MessageDialog.ShowPromptMessage("请输入回退原因");
                    return;
                }

                if (!m_billServer.RebackBill(billID, reason, out m_findBill, out m_err))
                {
                    MessageDialog.ShowErrorMessage(m_err);
                    return;
                }

                RebackFlowMessage(reason);
                RefreshDataGridView(m_findBill);

                MessageDialog.ShowPromptMessage("成功回退！");
            }
        }

        private void 修改单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HomemadeBillStatus status = (HomemadeBillStatus)Enum.Parse(typeof(HomemadeBillStatus),
                dataGridView1.SelectedRows[0].Cells["单据状态"].Value.ToString());

            if (status != HomemadeBillStatus.回退_编制单据有误)
            {
                MessageDialog.ShowPromptMessage("当前状态不允许修改单据");
                return;
            }

            if (!CheckDataItem())
            {
                return;
            }

            S_HomemadePartBill bill = new S_HomemadePartBill();

            bill.Bill_ID = txtBill_ID.Text;
            bill.Bill_Time = ServerModule.ServerTime.Time;          // 报检日期
            bill.BillStatus = HomemadeBillStatus.等待质检.ToString();
            bill.DeclarePersonnelCode = BasicInfo.LoginID;          // 自制件员编码
            bill.DeclarePersonnel = BasicInfo.LoginName;            // 自制件员签名
            bill.DeclareCount = Convert.ToInt32(numDeclareCount.Value);              // 自制件数
            bill.Provider = txtProvider.Tag.ToString();                       // 供应商编码
            bill.ProviderBatchNo = txtProviderBatchNo.Text;       // 供应商批次
            bill.GoodsID = (int)txtCode.Tag;
            bill.Remark = txtRemark.Text;
            bill.BatchNo = txtBatchNo.Text;
            bill.StorageID = UniversalFunction.GetStorageID(cmbStorage.Text);

            if (!m_billServer.UpdateBill(bill, out m_findBill, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("物品【{0}】，批次号【{1}】  ※※※ 等待【检验员】处理", txtName.Text, txtBatchNo.Text);
            PassFlowMessage(sb.ToString(), CE_RoleEnum.自制件检验组长);
            RefreshDataGridView(m_findBill);
            PositioningRecord(bill.Bill_ID);

            MessageDialog.ShowPromptMessage("成功提交,等待重新质检!");
        }

        private void btnFindChecker_Click(object sender, EventArgs e)
        {
            if (!BasicInfo.ListRoles.Contains(CE_RoleEnum.自制件检验组长.ToString())
                && !BasicInfo.ListRoles.Contains(CE_RoleEnum.电子检验组长.ToString()))
            {
                return;
            }

            FormPersonnel form = new FormPersonnel(txtChecker, BasicInfo.DeptCode, "姓名");
            form.ShowDialog();
        }

        private void 设置数据过滤器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormFilterCondition form = new FormFilterCondition(labelTitle.Text, m_findField, labelTitle.Text);
            form.ShowDialog();

            m_billServer.QueryResultFilter = QueryFilterControl.GetFilterString(labelTitle.Text);
            btnRefresh.PerformClick();
        }

        private void numAffirmCount_ValueChanged(object sender, EventArgs e)
        {
            numEligibleCount.Value = numDeclareCount.Value;
        }

        private void 删除单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (BasicInfo.LoginName != dataGridView1.CurrentRow.Cells["报检签名"].Value.ToString())
            {
                MessageDialog.ShowPromptMessage("只有此单据编制人才可执行删除操作！");
            }

            if (txtBill_ID.Text == "")
            {
                MessageDialog.ShowPromptMessage("请选择需要删除的记录");
            }

            if (MessageDialog.ShowEnquiryMessage("您是否要删除此单据") == DialogResult.Yes)
            {
                if (UniversalFunction.GetBillStatus("S_HomemadePartBill", "BillStatus", "Bill_ID", txtBill_ID.Text) != "已入库")
                {
                    if (m_billServer.ScrapBill(txtBill_ID.Text, out m_findBill, out m_err))
                    {
                        m_billNoControl.CancelBill(txtBill_ID.Text);
                        m_msgPromulgator.DestroyMessage(txtBill_ID.Text);
                        MessageDialog.ShowPromptMessage("删除成功!");
                    }
                    else
                    {
                        MessageDialog.ShowErrorMessage(m_err);
                    }
                }
                else
                {
                    MessageDialog.ShowErrorMessage("此单据已入库，不可报废");
                }
            }

            RefreshDataGridView(m_findBill);
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

        private void btnBatchNo_Click(object sender, EventArgs e)
        {
            FormQueryInfo form = QueryInfoDialog.GetWorkShopBatchNoInfo(Convert.ToInt32(txtCode.Tag), m_lnqWSCode.WSCode);

            if (form.ShowDialog() == DialogResult.OK)
            {
                txtBatchNo.Text = (string)form.GetDataItem("批次号");
            }
        }

        private void txtProvider_Enter(object sender, EventArgs e)
        {
            txtProvider.StrEndSql = " and ProviderCode in ('JJCJ','SYS_CPKF','SYS_TCUCJ')";
        }

        private void txtProvider_OnCompleteSearch()
        {
            txtProvider.Text = txtProvider.DataResult["供应商名称"].ToString();
            txtProvider.Tag = txtProvider.DataResult["供应商编码"].ToString();
        }
    }
}
