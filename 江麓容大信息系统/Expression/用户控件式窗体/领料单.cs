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
    /// 领料单界面
    /// </summary>
    public partial class 领料单 : Form
    {
        #region 成员变量

        /// <summary>
        /// BOM表服务组件
        /// </summary>
        IBomServer _bomService = ServerModuleFactory.GetServerModule<IBomServer>();

        /// <summary>
        /// 服务组件
        /// </summary>
        IProductInfoServer m_productInfoServer = ServerModuleFactory.GetServerModule<IProductInfoServer>();

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_error;

        /// <summary>
        /// 领料单服务
        /// </summary>
        IMaterialRequisitionServer m_billServer = ServerModuleFactory.GetServerModule<IMaterialRequisitionServer>();

        /// <summary>
        /// 领料单物品清单服务
        /// </summary>
        IMaterialRequisitionGoodsServer m_goodsServer = ServerModuleFactory.GetServerModule<IMaterialRequisitionGoodsServer>();

        /// <summary>
        /// 人员信息服务
        /// </summary>
        IPersonnelInfoServer m_personnelServer = ServerModuleFactory.GetServerModule<IPersonnelInfoServer>();

        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 获取单据流消息操作接口
        /// </summary>
        IBillFlowMessage m_billFlowMsg = PlatformFactory.GetObject<IBillFlowMessage>();

        /// <summary>
        /// 检索到的领料单结果
        /// </summary>
        IQueryResult m_queryResult;

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
        /// 库存服务
        /// </summary>
        IStoreServer m_storeServer = ServerModuleFactory.GetServerModule<IStoreServer>();

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 领料用途
        /// </summary>
        string m_purposeType;

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nodeInfo">功能树节点信息</param>
        public 领料单(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_billMessageServer.BillType = "领料单";
            m_billNoControl = new BillNumberControl(labelTitle.Text, m_billServer);

            m_authFlag = nodeInfo.Authority;

            string[] strBillStatus = { "全部", 
                                         MaterialRequisitionBillStatus.新建单据.ToString(), 
                                         MaterialRequisitionBillStatus.等待主管审核.ToString(),
                                         MaterialRequisitionBillStatus.等待部门领导批准.ToString(),
                                         MaterialRequisitionBillStatus.等待出库.ToString(),
                                         MaterialRequisitionBillStatus.已出库.ToString()};

            checkBillDateAndStatus1.InsertComBox(strBillStatus);
            #region 被要求使用服务器时间 Modify by cjb on 2012.6.15

            checkBillDateAndStatus1.dtpStartTime.Value = ServerTime.Time.AddDays(1).AddMonths(-1);
            checkBillDateAndStatus1.dtpEndTime.Value = ServerTime.Time.AddDays(1);

            #endregion

            InitForm();

            刷新数据ToolStripMenuItem.PerformClick();

            DataTable dt = UniversalFunction.GetStorageTb();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cmbStorage.Items.Add(dt.Rows[i]["StorageName"].ToString());
            }

            cmbStorage.SelectedIndex = -1;
        }

        private void 领料单_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authFlag);
        }

        void checkBillDateAndStatus1_OnCompleteSearch()
        {
            menuItemReresh_Click(null, null);
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
                    WndMsgData msg = new WndMsgData();      //这是创建自定义信息的结构
                    Type dataType = msg.GetType();

                    msg = (WndMsgData)m.GetLParam(dataType);//这里获取的就是作为LParam参数发送来的信息的结构

                    DataTable dtMessage = UniversalFunction.PositioningOneRecord(msg.MessageContent, "领料单");

                    if (dtMessage.Rows.Count == 0)
                    {
                        m_billMessageServer.DestroyMessage(msg.MessageContent);
                        MessageDialog.ShowPromptMessage("未找到相关记录");
                    }
                    else
                    {
                        dataGridView1.DataSource = dtMessage;

                        string strColName = "";

                        foreach (DataGridViewColumn col in dataGridView1.Columns)
                        {
                            if (col.Visible)
                            {
                                strColName = col.Name;
                                break;
                            }
                        }

                        dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells[strColName];
                    }

                    break;

                default:
                    base.DefWndProc(ref m);
                    break;
            }
        }

        /// <summary>
        /// 权限控制
        /// </summary>
        /// <param name="authorityFlag">权限标志</param>
        void AuthorityControl(PlatformManagement.AuthorityFlag authorityFlag)
        {
            FaceAuthoritySetting.SetVisibly(menuStrip, authorityFlag);

            if ((authorityFlag & AuthorityFlag.StockIn) != AuthorityFlag.Nothing)
            {
                仓库管理员操作ToolStripMenuItem.Visible = true;
                仓库管理员操作ToolStripMenuItem.Enabled = true;
            }
        }

        /// <summary>
        /// 初始化界面
        /// </summary>
        void InitForm()
        {
            List<string> lstAsscembly = _bomService.GetAssemblyTypeList();

            if (lstAsscembly != null)
            {
                cmbProductType.DataSource = lstAsscembly;
            }

            cmbProductType.SelectedIndex = -1;

            #region 获取领料类型

            cmbFetchType.Items.AddRange(Enum.GetNames(typeof(FetchGoodsType)));

            #endregion
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
            S_MaterialRequisition lnqBillInfo = m_billServer.GetBill(row.Cells["领料单号"].Value.ToString());

            if (lnqBillInfo.FillInPersonnelCode == BasicInfo.LoginID)
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
        /// 检测有关数据项内容是否正确
        /// </summary>
        /// <returns>返回检测结果</returns>
        bool CheckDataItem()
        {
            if (txtBill_ID.Text.Length == 0)
            {
                MessageDialog.ShowPromptMessage("单据号不允许为空!");
                return false;
            }

            if (txtPurpose.Text == "")
            {
                txtPurpose.Focus();
                MessageDialog.ShowPromptMessage("请选择单据用途");
                return false;
            }

            if (cmbStorage.Text == "")
            {
                cmbStorage.Focus();
                MessageDialog.ShowPromptMessage("所属库房不允许为空");
                return false;
            }

            #region 2012.6.19 夏石友，根据张风堂要求备注为必填项

            txtRemark.Text = txtRemark.Text.Trim();

            if (txtRemark.Text == "")
            {
                txtRemark.Focus();
                MessageDialog.ShowPromptMessage("备注不允许为空");
                return false;
            }

            #endregion

            if (cmbReliantBillType.SelectedIndex > -1)
            {
                if (txtReliantBillNo.Text == "")
                {
                    txtReliantBillNo.Focus();
                    MessageDialog.ShowPromptMessage("请选择关联单据单号");
                    return false;
                }
            }

            if (cmbFetchType.SelectedIndex < 0)
            {
                cmbFetchType.Focus();
                MessageDialog.ShowPromptMessage("请选择领料类型");
                return false;
            }

            if (cmbFetchType.Text != FetchGoodsType.零星领料.ToString())
            {
                if (numRequestCount.Value == 0)
                {
                    numRequestCount.Focus();
                    MessageDialog.ShowPromptMessage("总成数量必须 > 0");
                    return false;
                }
            }
            else
            {
                if (cmbProductType.SelectedIndex >= 0 || numRequestCount.Value > 0)
                {
                    MessageDialog.ShowPromptMessage("零星领料时不允许设置产品类型和总成数量");
                }

                cmbProductType.SelectedIndex = -1;
                cmbProductType.Enabled = false;
                numRequestCount.Value = 0;
                numRequestCount.Enabled = false;
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

            DataTable dataSource = findBill.DataCollection.Tables[0];

            dataGridView1.DataSource = dataSource;

            this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            dataGridView1.Columns["领料部门"].Visible = false;
            dataGridView1.Columns["编制人编码"].Visible = false;

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

            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Columns["用途编码"].Visible = false;
            }

            dataGridView1.Refresh();
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="billNo">定位用的单据号</param>
        void PositioningRecord(string billNo)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if ((string)dataGridView1.Rows[i].Cells["领料单号"].Value == billNo)
                {
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[1];
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                }
            }
        }

        #endregion 刷新数据

        private void 新建单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                if (dataGridView1.SelectedRows[0].Cells["单据状态"].Value.ToString() != "新建单据")
                {
                    if (lblBillStatus.Text == MaterialRequisitionBillStatus.新建单据.ToString())
                    {
                        MessageDialog.ShowPromptMessage("现在已经处于新建单据状态");
                        return;
                    }
                }

                if (BasicInfo.LoginID == (string)dataGridView1.SelectedRows[0].Cells["编制人编码"].Value)
                {
                    if (lblBillStatus.Text == MaterialRequisitionBillStatus.新建单据.ToString())
                    {
                        MessageDialog.ShowPromptMessage("现在已经处于新建单据状态");
                        return;
                    }
                }
            }
            else
            {
                if (lblBillStatus.Text == MaterialRequisitionBillStatus.新建单据.ToString())
                {
                    MessageDialog.ShowPromptMessage("现在已经处于新建单据状态");
                    return;
                }
            }

            ClearForm();
            txtBill_ID.Text = m_billNoControl.GetNewBillNo();
            txtDepartment.Text = BasicInfo.DeptName;
            txtDepartment.Tag = BasicInfo.DeptCode;
            lblBillStatus.Text = MaterialRequisitionBillStatus.新建单据.ToString();
        }

        private void 领料员提交单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
                return;

            if (lblBillStatus.Text != MaterialRequisitionBillStatus.新建单据.ToString())
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
                MessageDialog.ShowPromptMessage("您还未设置领料物品清单，无法提交");
                return;
            }

            if (!m_billServer.SubmitNewBill(billNo, out m_queryResult, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            m_billMessageServer.DestroyMessage(billNo);
            m_billMessageServer.SendNewFlowMessage(billNo,
                string.Format("【用途】：{0} 【申请人】：{1}  ※※※ 等待【上级领导】处理",
                txtPurpose.Text, txtFillInPersonnel.Text), BillFlowMessage_ReceivedUserType.角色,
                m_billMessageServer.GetSuperior(CE_RoleStyleType.所有上级领导, BasicInfo.LoginID));

            RefreshDataGridView(m_queryResult);
            PositioningRecord(billNo);

            MessageDialog.ShowPromptMessage("成功提交,等待主管审核!");
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

            MaterialRequisitionBillStatus status =
                (MaterialRequisitionBillStatus)Enum.Parse(typeof(MaterialRequisitionBillStatus),
                UniversalFunction.GetBillStatus("S_MaterialRequisition", "BillStatus", "Bill_ID", txtBill_ID.Text));

            if (status == MaterialRequisitionBillStatus.已出库)
            {
                MessageDialog.ShowPromptMessage("请选择未出库的记录后再进行此操作");
                return;
            }

            if (!CheckUserOperation(dataGridView1.SelectedRows[0]))
                return;

            string info = string.Format("您是否要删除 {0} 领料单时, 删除时同时也会删除此领料单下的所有物品清单, 是否继续？", txtBill_ID.Text);

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
            m_billMessageServer.DestroyMessage(txtBill_ID.Text);

            RefreshDataGridView(m_queryResult);
        }

        /// <summary>
        /// 清除窗体上的信息
        /// </summary>
        void ClearForm()
        {
            cmbStorage.SelectedIndex = -1;
            chkPrint.Checked = false;
            txtBill_ID.Text = "";
            dateTime_BillTime.Value = ServerModule.ServerTime.Time;
            txtPurpose.Text = "";
            txtReliantBillNo.Text = "";
            txtDepartment.Text = "";
            txtDepartment.Tag = null;
            cmbFetchType.SelectedIndex = -1;
            cmbReliantBillType.SelectedIndex = -1;
            cmbProductType.SelectedIndex = -1;
            numRequestCount.Value = 0;

            txtRemark.Text = "";

            lblBillStatus.Text = "";

            txtFillInPersonnel.Text = "";
            txtDepartmentDirector.Text = "";
            txtDepotManager.Text = "";
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            try
            {
                ClearForm();

                this.cmbFetchType.SelectedIndexChanged -= new System.EventHandler(this.cmbFetchType_SelectedIndexChanged);

                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                txtBill_ID.Text = (string)row.Cells["领料单号"].Value;
                dateTime_BillTime.Value = (DateTime)row.Cells["日期"].Value;

                string reliantBillType = (string)row.Cells["关联单据"].Value;

                if (GlobalObject.GeneralFunction.IsNullOrEmpty(reliantBillType))
                {
                    cmbReliantBillType.SelectedIndex = -1;
                }
                else
                {
                    cmbReliantBillType.Text = (string)row.Cells["关联单据"].Value;
                }

                txtReliantBillNo.Text = (string)row.Cells["关联单号"].Value;
                txtDepartment.Text = (string)row.Cells["部门名称"].Value;
                txtDepartment.Tag = (string)row.Cells["领料部门"].Value;
                cmbFetchType.Text = (string)row.Cells["领料类型"].Value;

                cmbStorage.Text = UniversalFunction.GetStorageName(row.Cells["库房代码"].Value.ToString());

                if (cmbFetchType.Text != FetchGoodsType.零星领料.ToString())
                {
                    cmbProductType.Text = (string)row.Cells["产品类型"].Value;
                    numRequestCount.Value = (int)row.Cells["领料台数"].Value;
                }
                else
                {
                    cmbProductType.SelectedIndex = -1;
                    numRequestCount.Value = 0;
                }

                txtPurpose.Text = (string)row.Cells["用途说明"].Value;
                txtPurpose.Tag = (string)row.Cells["用途编码"].Value;
                m_purposeType = txtPurpose.Text;

                lblBillStatus.Text = (string)row.Cells["单据状态"].Value;

                if (row.Cells["备注"].Value != System.DBNull.Value)
                    txtRemark.Text = (string)row.Cells["备注"].Value;

                if (row.Cells["编制人"].Value != System.DBNull.Value)
                    txtFillInPersonnel.Text = (string)row.Cells["编制人"].Value;

                if (row.Cells["部门主管签名"].Value != System.DBNull.Value)
                    txtDepartmentDirector.Text = (string)row.Cells["部门主管签名"].Value;

                if (row.Cells["仓管签名"].Value != System.DBNull.Value)
                    txtDepotManager.Text = (string)row.Cells["仓管签名"].Value;

                if (row.Cells["是否已打印"].Value != System.DBNull.Value)
                    chkPrint.Checked = (bool)row.Cells["是否已打印"].Value;

                仓库管理员操作ToolStripMenuItem.Visible = UniversalFunction.CheckStorageAndPersonnel(
                    (string)row.Cells["库房代码"].Value);

                if (txtReliantBillNo.Text.Trim().Length > 0 && txtReliantBillNo.Text.Trim().Substring(0, 3) == "WMD")
                {
                    领料员提交单据ToolStripMenuItem.Visible = false;
                }
                else
                {
                    领料员提交单据ToolStripMenuItem.Visible = true;
                }

            }
            finally
            {
                this.cmbFetchType.SelectedIndexChanged += new System.EventHandler(this.cmbFetchType_SelectedIndexChanged);
            }
        }

        private void 设置领料清单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string billNo = txtBill_ID.Text;

            try
            {
                this.dataGridView1.CellEnter -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);

                if (lblBillStatus.Text == MaterialRequisitionBillStatus.已出库.ToString())
                {
                    MessageDialog.ShowPromptMessage("您现在的单据状态，无法进行此操作");
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
                    // 如果此单据还不存在则创建
                    S_MaterialRequisition bill = new S_MaterialRequisition();

                    bill.Bill_ID = txtBill_ID.Text;
                    bill.Bill_Time = ServerModule.ServerTime.Time;
                    bill.BillStatus = MaterialRequisitionBillStatus.新建单据.ToString();
                    bill.Department = txtDepartment.Tag as string;
                    bill.AssociatedBillType = cmbReliantBillType.Text;
                    bill.AssociatedBillNo = txtReliantBillNo.Text;
                    bill.PurposeCode = txtPurpose.Tag.ToString();
                    bill.FetchType = cmbFetchType.Text;
                    bill.FillInPersonnelCode = BasicInfo.LoginID;
                    bill.FillInPersonnel = BasicInfo.LoginName;
                    bill.Remark = txtRemark.Text;
                    bill.StorageID = UniversalFunction.GetStorageID(cmbStorage.Text);

                    if (cmbFetchType.Text != FetchGoodsType.零星领料.ToString())
                    {
                        bill.ProductType = cmbProductType.Text;
                        bill.FetchCount = Convert.ToInt32(numRequestCount.Value);
                    }
                    else
                    {
                        bill.ProductType = "";
                    }

                    if (!m_billServer.AddBill(bill, out m_queryResult, out m_error))
                    {
                        MessageDialog.ShowErrorMessage(m_error);
                        return;
                    }
                }

                FormFetchGoods form = new FormFetchGoods(FormFetchGoods.OperateMode.新建, txtBill_ID.Text, txtReliantBillNo.Text);
                form.m_strFlag = lblBillStatus.Text;

                if (cmbFetchType.Text == FetchGoodsType.整台领料.ToString() || cmbFetchType.Text == FetchGoodsType.整台领料不含后补充.ToString())
                {
                    form.GenerateGoodsBill(GlobalObject.GeneralFunction.StringConvertToEnum<FetchGoodsType>(cmbFetchType.Text),
                        cmbProductType.Text, Convert.ToInt32(numRequestCount.Value));
                }
                else if (cmbFetchType.Text == FetchGoodsType.阀块领料.ToString())
                {
                    form.GenerateAssemblyGoodsBill("液压阀块总成", Convert.ToInt32(numRequestCount.Value), cmbProductType.Text);
                }
                else if (cmbFetchType.Text == FetchGoodsType.行星轮合件领料.ToString())
                {
                    form.GenerateAssemblyGoodsBill("行星轮合件", Convert.ToInt32(numRequestCount.Value), cmbProductType.Text);
                }
                else if (cmbFetchType.Text == FetchGoodsType.油底壳领料.ToString())
                {
                    form.GenerateAssemblyGoodsBill("油底壳总成", Convert.ToInt32(numRequestCount.Value), cmbProductType.Text);
                }
                else if (!GlobalObject.GeneralFunction.IsNullOrEmpty(txtReliantBillNo.Text.Trim()))
                {
                    form.GenerateGoodsBill(txtReliantBillNo.Text);
                }

                form.ShowDialog();

                if (!m_goodsServer.IsExist(txtBill_ID.Text))
                {
                    if (MessageDialog.ShowEnquiryMessage("您没有设置物品清单，是否要删除已经创建的单据？") == DialogResult.Yes)
                    {
                        if (!m_billServer.DeleteBill(txtBill_ID.Text, out m_queryResult, out m_error))
                        {
                            MessageDialog.ShowErrorMessage(m_error);
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
            finally
            {
                this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
            }

            if (!m_billServer.GetAllBill(out m_queryResult, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            RefreshDataGridView(m_queryResult);
            PositioningRecord(billNo);
        }

        private void cmbFetchType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFetchType.Text != FetchGoodsType.零星领料.ToString())
            {
                if (cmbFetchType.Text == FetchGoodsType.整台领料.ToString())
                    cmbProductType.Enabled = true;

                numRequestCount.Enabled = true;
            }
            else
            {
                cmbProductType.SelectedIndex = -1;
                cmbProductType.Enabled = false;
                numRequestCount.Value = 0;
                numRequestCount.Enabled = false;
            }
        }

        private void 修改单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
                return;

            if (lblBillStatus.Text != MaterialRequisitionBillStatus.新建单据.ToString() &&
                lblBillStatus.Text != MaterialRequisitionBillStatus.等待主管审核.ToString())
            {
                MessageDialog.ShowPromptMessage("您现在的单据状态，无法进行此操作");
                return;
            }

            if (!CheckUserOperation(dataGridView1.SelectedRows[0]))
                return;

            if (!CheckDataItem())
                return;

            // 检查关联单据类型或关联单据号是否发生改变
            if (dataGridView1.SelectedRows[0].Cells["关联单据"].Value.ToString() != cmbReliantBillType.Text ||
                dataGridView1.SelectedRows[0].Cells["关联单号"].Value.ToString() != txtReliantBillNo.Text)
            {
                if (m_goodsServer.IsExist(txtBill_ID.Text))
                {
                    m_error = string.Format("{0}领料单已经设置了物品清单，不允许再修改关联单据或关联单号，请删除此单据所有物品后再进行此操作",
                        txtBill_ID.Text);
                    MessageDialog.ShowPromptMessage(m_error);
                    return;
                }
            }

            S_MaterialRequisition bill = new S_MaterialRequisition();

            bill.Bill_ID = txtBill_ID.Text;
            bill.FetchType = cmbFetchType.Text;
            bill.PurposeCode = txtPurpose.Tag.ToString();
            bill.AssociatedBillType = cmbReliantBillType.Text;
            bill.AssociatedBillNo = txtReliantBillNo.Text;
            bill.FetchCount = Convert.ToInt32(numRequestCount.Value);
            bill.ProductType = cmbProductType.Text;
            bill.Remark = txtRemark.Text;
            bill.StorageID = UniversalFunction.GetStorageID(cmbStorage.Text);

            if (!m_billServer.UpdateBill(bill, out m_queryResult, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            RefreshDataGridView(m_queryResult);
            PositioningRecord(bill.Bill_ID);
        }

        private void 修改用途及备注ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
                return;

            if (!CheckDataItem())
                return;

            if (lblBillStatus.Text == MaterialRequisitionBillStatus.已出库.ToString())
            {
                MessageDialog.ShowPromptMessage("单据入库后无法进行此操作");
                return;
            }

            if (!CheckUserOperation(dataGridView1.SelectedRows[0]))
                return;

            string billNo = txtBill_ID.Text;

            if (txtPurpose.Tag.ToString() == "")
            {
                MessageDialog.ShowPromptMessage("操作失败，无法获取到单据用途！");
                return;
            }

            if (!m_billServer.UpdateBill(billNo, txtPurpose.Tag.ToString(), txtRemark.Text, out m_queryResult, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            RefreshDataGridView(m_queryResult);
            PositioningRecord(billNo);
        }

        private void 查看领料清单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择要查看的记录后再进行此操作");
                return;
            }
            else if (dataGridView1.SelectedRows.Count > 1)
            {
                MessageDialog.ShowPromptMessage("只允许选择一条记录进行此操作！");
                return;
            }

            FormFetchGoods form = new FormFetchGoods(FormFetchGoods.OperateMode.查看, txtBill_ID.Text, txtReliantBillNo.Text);
            form.m_strFlag = lblBillStatus.Text;
            form.ShowDialog();
        }

        private void 审核ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
                return;

            if (lblBillStatus.Text != MaterialRequisitionBillStatus.等待主管审核.ToString())
            {
                MessageDialog.ShowPromptMessage("请选择要审核的记录后再进行此操作");
                return;
            }

            bool flag = false;

            List<string> lstRoleName =
                m_billMessageServer.GetSuperior(CE_RoleStyleType.负责人, dataGridView1.SelectedRows[0].Cells["编制人编码"].Value.ToString());

            foreach (string roleName in lstRoleName)
            {
                if (BasicInfo.ListRoles.Contains(roleName))
                {
                    flag = true;
                    break;
                }
            }

            if (!flag)
            {
                MessageDialog.ShowPromptMessage("请选择您下属人员提交的记录后再进行此操作,并且您必须是部门负责人");
                return;
            }

            string billNo = txtBill_ID.Text;

            MaterialRequisitionBillStatus eumnBillStatus = MaterialRequisitionBillStatus.新建单据;

            if (!m_billServer.AuthorizeBill(billNo, BasicInfo.LoginName, out eumnBillStatus, out m_queryResult, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }
            else
            {
                if (eumnBillStatus == MaterialRequisitionBillStatus.等待部门领导批准)
                {
                    string msg = string.Format("【用途】：{0} 【申请人】：{1}  ※※※ 等待【部门负责人】处理",
                txtPurpose.Text, txtFillInPersonnel.Text);
                    m_billMessageServer.PassFlowMessage(billNo, msg, CE_RoleEnum.制造负责人.ToString(), true);
                    MessageDialog.ShowPromptMessage("成功审核, 等待部门负责人批准");
                }
                else if (eumnBillStatus == MaterialRequisitionBillStatus.等待工艺人员批准)
                {
                    string msg = string.Format("【用途】：{0} 【申请人】：{1}  ※※※ 等待【工艺人员】处理",
                txtPurpose.Text, txtFillInPersonnel.Text);
                    m_billMessageServer.PassFlowMessage(billNo, msg, CE_RoleEnum.工艺人员.ToString(), true);
                    MessageDialog.ShowPromptMessage("成功审核, 等待工艺人员批准");
                }
                else
                {
                    string msg = string.Format("【用途】：{0} 【申请人】：{1}  ※※※ 等待【仓管员】处理",
                txtPurpose.Text, txtFillInPersonnel.Text);
                    m_billMessageServer.PassFlowMessage(billNo, msg,
                        m_billMessageServer.GetRoleStringForStorage(cmbStorage.Text).ToString(), true);
                    MessageDialog.ShowPromptMessage("成功审核, 等待仓库确认");
                }
            }

            RefreshDataGridView(m_queryResult);
            PositioningRecord(billNo);
        }

        private void 核实领料清单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
                return;

            DataGridViewRow row = dataGridView1.SelectedRows[0];
            FormFetchGoods form = new FormFetchGoods(FormFetchGoods.OperateMode.仓库核实,
                row.Cells["领料单号"].Value.ToString(), row.Cells["关联单号"].Value.ToString());

            form.m_strFlag = lblBillStatus.Text;

            if (cmbFetchType.Text == FetchGoodsType.整台领料.ToString())
            {
                form.ReportTitle = "CVT 整台领料物料清单";
            }
            else if (cmbFetchType.Text == FetchGoodsType.阀块领料.ToString())
            {
                form.ReportTitle = "阀块整台领料物料清单";
            }
            else if (cmbFetchType.Text == FetchGoodsType.行星轮合件领料.ToString())
            {
                form.ReportTitle = "行星轮合件领料物料清单";
            }
            else if (cmbFetchType.Text == FetchGoodsType.油底壳领料.ToString())
            {
                form.ReportTitle = "油底壳领料物料清单";
            }

            form.ShowDialog();
        }

        #region 2017-9-20 夏石友，出库时需检测物料状态

        /// <summary>
        /// 检查是否存在隔离物料，存在则提示
        /// </summary>
        /// <param name="billNo">领料单号</param>
        /// <returns>通过返回true，不允许通过返回false</returns>
        private bool CheckIsolationGoods(string billNo)
        {
            List<View_S_MaterialRequisitionGoods> lstGoods = m_billServer.IsExistsIsolationGoods(billNo);

            if (lstGoods.Count > 0)
            {
                StringBuilder sb = new StringBuilder("领料单中存在以下隔离物料信息，是否仍然继续出库？隔离物料信息：");

                sb.AppendLine();

                int index = 1;

                foreach (var item in lstGoods)
                {
                    sb.AppendFormat("{0}. 图号【{1}】、名称【{2}】、规格【{3}】、批次【{4}】",
                        index, item.图号型号, item.物品名称, item.规格, item.批次号);

                    sb.AppendLine();
                }

                if (MessageDialog.ShowEnquiryMessage(sb.ToString()) == DialogResult.Yes)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }
        #endregion

        private void 确认出库ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CheckSelectedRow())
                    return;

                string billNo = txtBill_ID.Text;

                if (lblBillStatus.Text != MaterialRequisitionBillStatus.等待出库.ToString())
                {
                    MessageDialog.ShowPromptMessage("请选择要出库的记录后再进行此操作");
                    return;
                }

                #region 对版次号的提示

                if (dataGridView1.CurrentRow.Cells["领料部门"].Value.ToString().Contains("ZZ")
                    && Convert.ToDecimal(dataGridView1.CurrentRow.Cells["领料台数"].Value) > 0
                    && txtPurpose.Text.Contains("装配用"))
                {
                    DataTable dt = m_billServer.CheckGoodsVersion(dataGridView1.CurrentRow.Cells["领料单号"].Value.ToString());

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        if (MessageDialog.ShowEnquiryMessage("此整台份领料单中的物品的版次号与BOM表中的规定的版次号不符，是否需要导出EXCEL？") == DialogResult.Yes)
                        {
                            ExcelHelperP.DataTableToExcel(saveFileDialog1, dt, null);
                        }
                    }
                }

                #endregion

                #region 2017-9-20 夏石友，出库时需检测物料状态
                if (!CheckIsolationGoods(billNo))
                {
                    return;
                }
                #endregion

                if (m_billServer.FinishBill(billNo, BasicInfo.LoginName, out m_queryResult, out m_error))
                {
                    m_billMessageServer.EndFlowMessage(billNo, string.Format("{0}号领料单已成功领料", billNo), null, null);

                    MessageDialog.ShowPromptMessage("出库成功");

                    RefreshDataGridView(m_queryResult);
                    PositioningRecord(billNo);
                }
                else
                {
                    MessageDialog.ShowErrorMessage(m_error);
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            dataGridView1.ClearSelection();
            dataGridView1.Rows[e.RowIndex].Selected = true;
            dataGridView1.CurrentCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            查看领料清单ToolStripMenuItem.PerformClick();
        }

        private void 表单打印ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IBillReportInfo report = null;
            int index = 0;

            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                MaterialRequisitionBillStatus status = (MaterialRequisitionBillStatus)
                    Enum.Parse(typeof(MaterialRequisitionBillStatus), row.Cells["单据状态"].Value.ToString());

                if (status != MaterialRequisitionBillStatus.已出库)
                {
                    continue;
                }

                if (row.Cells["关联单据"].Value.ToString() == "三包外返修处置单")
                {
                    report = new 报表_三包外领料(row.Cells["领料单号"].Value.ToString(), labelTitle.Text);

                }
                else
                {
                    if (row.Cells["领料类型"].Value.ToString() != FetchGoodsType.零星领料.ToString())
                    {
                        report = new 报表_整台领料单(row.Cells["领料单号"].Value.ToString(), labelTitle.Text);
                    }
                    else
                    {
                        report = new 报表_领料单(row.Cells["领料单号"].Value.ToString(), labelTitle.Text);
                    }
                }

                PrintReportBill print = new PrintReportBill(21.8, 9.31, report);

                if (index++ > 0)
                {
                    print.ShowPrintDialog = false;
                }

                print.DirectPrintReport();
            }
        }

        private void btnFindScrapBill_Click(object sender, EventArgs e)
        {
            if (cmbReliantBillType.Text == "报废单")
            {
                FormQueryInfo dialog = QueryInfoDialog.GetScrapBillDialogForFetchGoods();

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtReliantBillNo.Text = dialog.GetStringDataItem("报废单号");
                }
            }
            else if (cmbReliantBillType.Text == "领料退库单")
            {
                FormQueryInfo dialog = QueryInfoDialog.GetReturnedInDepotBillDialogForFetchGoods();

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtReliantBillNo.Text = dialog.GetStringDataItem("退库单号");
                }
            }
            else if (cmbReliantBillType.Text == "三包外返修处置单")
            {
                FormQueryInfo dialog = QueryInfoDialog.GetThreePacketsOfTheRepairForFetchGoods();

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtReliantBillNo.Text = dialog.GetStringDataItem("单据号");
                }
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

        private void 综合查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IAuthorization authorization = PlatformFactory.GetObject<IAuthorization>();
            string businessID = "领料单综合查询";
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

        private void btnFindPurpose_Click(object sender, EventArgs e)
        {
            领料用途 form = new 领料用途();

            if (form.ShowDialog() == DialogResult.OK)
            {
                txtPurpose.Tag = form.SelectedData.Code;
                txtPurpose.Text = form.SelectedData.Purpose;

                if (form.SelectedData.RemindWord != null && form.SelectedData.RemindWord.Trim().Length > 0)
                {
                    MessageDialog.ShowPromptMessage(form.SelectedData.RemindWord);
                }
            }
        }

        private void menuItemReresh_Click(object sender, EventArgs e)
        {
            m_billServer.QueryResultFilter = QueryFilterControl.GetFilterString(labelTitle.Text)
                + checkBillDateAndStatus1.GetSqlString("日期", "单据状态");

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
            刷新数据ToolStripMenuItem.PerformClick();
        }

        private void 自动刷新数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            自动刷新数据ToolStripMenuItem.Checked = !自动刷新数据ToolStripMenuItem.Checked;

            if (自动刷新数据ToolStripMenuItem.Checked)
            {
                try
                {
                    int second = 1000 * Convert.ToInt32(InputBox.ShowDialog("请输入自动刷新时间(单位：秒)", "时间：", "10"));

                    if (second < 10000)
                    {
                        throw new Exception("");
                    }

                    timerRefresh.Interval = second;
                    timerRefresh.Enabled = true;
                }
                catch (Exception exce)
                {
                    Console.WriteLine(exce.Message);
                    自动刷新数据ToolStripMenuItem.Checked = false;
                    MessageDialog.ShowPromptMessage("您输入的数据格式不正确，请输入 >= 10 的数字!");
                }
            }
            else
            {
                timerRefresh.Enabled = false;
            }

            if (自动刷新数据ToolStripMenuItem.Checked)
            {
                自动刷新数据ToolStripMenuItem.ForeColor = Color.Red;
            }
            else
            {
                自动刷新数据ToolStripMenuItem.ForeColor = Color.Black;
            }
        }

        private void timerRefresh_Tick(object sender, EventArgs e)
        {
            menuItemReresh_Click(sender, e);
        }

        private void 修改备注menuItem_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
                return;

            if (lblBillStatus.Text == MaterialRequisitionBillStatus.已出库.ToString())
            {
                MessageDialog.ShowPromptMessage("单据入库后无法进行此操作");
                return;
            }

            string billNo = txtBill_ID.Text;

            if (!m_billServer.UpdateBill(billNo, null, txtRemark.Text, out m_queryResult, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            RefreshDataGridView(m_queryResult);
            PositioningRecord(billNo);
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(this.labelTitle.Text, e.Column);
        }

        private void 领料单_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        private void 打印整台领料清单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
                return;

            if (lblBillStatus.Text != MaterialRequisitionBillStatus.已出库.ToString())
            {
                MessageDialog.ShowPromptMessage("请选择已经出库的记录后再进行此操作");
                return;
            }

            string title = "";

            if (cmbFetchType.Text == FetchGoodsType.整台领料.ToString())
                title = "CVT 整台领料物料清单";
            else if (cmbFetchType.Text == FetchGoodsType.阀块领料.ToString())
                title = "阀块整台领料物料清单";
            else if (cmbFetchType.Text == FetchGoodsType.行星轮合件领料.ToString())
                title = "行星轮合件领料物料清单";
            else if (cmbFetchType.Text == FetchGoodsType.油底壳领料.ToString())
                title = "油底壳领料物料清单";
            else if (dataGridView1.CurrentRow.Cells["关联单据"].Value.ToString() == "三包外返修处置单")
                title = "三包外返修领料单";
            else
            {
                MessageDialog.ShowPromptMessage("零星领料直接选择“表单打印”菜单项即可！");
                return;
            }

            报表_领料单物品清单 report = new 报表_领料单物品清单(txtBill_ID.Text, title);
            report.ShowDialog();
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

        private void 设置发料清单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            发料清单设置 form = new 发料清单设置();
            form.ShowDialog();
        }

        private void 查询未领用的报废物品ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            未领用的报废物品的查询窗体 form = new 未领用的报废物品的查询窗体(BasicInfo.LoginName);
            form.ShowDialog();
        }

        private void 回退单据ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (部门主管操作ToolStripMenuItem.Visible == true
                && lblBillStatus.Text == "等待主管审核")
            {
                ReturnBillStatus();
            }
            else if (仓库管理员操作ToolStripMenuItem.Visible == true
                && lblBillStatus.Text == "等待出库")
            {
                ReturnBillStatus();
            }
        }

        /// <summary>
        /// 回退单据
        /// </summary>
        private void ReturnBillStatus()
        {
            if (lblBillStatus.Text != "已出库")
            {
                回退单据 form = new 回退单据(CE_BillTypeEnum.领料单, txtBill_ID.Text, lblBillStatus.Text);

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

                        RefreshDataGridView(m_queryResult);
                    }
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
            }
        }

        private void 统计数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            单据统计 frm = new 单据统计("领料单统计");
            frm.Show();
        }

        /// <summary>
        /// 从数据行中获取单据信息
        /// </summary>
        /// <param name="row">数据行</param>
        /// <param name="billNo">单据号</param>
        /// <param name="billStatus">单据状态</param>
        private void GetBillInfo(DataGridViewRow row, out string billNo, out string billStatus)
        {
            billNo = row.Cells["领料单号"].Value.ToString();
            billStatus = row.Cells["单据状态"].Value.ToString();
        }

        /// <summary>
        /// 供建单员打印物品条形码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 打印领料单物品条形码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
            {
                return;
            }

            string billNo;
            string billStatus;

            GetBillInfo(dataGridView1.SelectedRows[0], out billNo, out billStatus);

            if (billStatus != MaterialRequisitionBillStatus.已出库.ToString())
            {
                MessageDialog.ShowPromptMessage("您现在的单据状态，无法进行此操作");
                return;
            }

            FormFetchGoods form = new FormFetchGoods(FormFetchGoods.OperateMode.打印条形码, billNo, "");
            form.m_strFlag = lblBillStatus.Text;
            form.ShowDialog();
        }

        private void 设置制造部辅料限额ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            设置制造部辅料限额 form = new 设置制造部辅料限额();
            form.ShowDialog();
        }

        private void 批准ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
                return;

            if (lblBillStatus.Text != MaterialRequisitionBillStatus.等待部门领导批准.ToString())
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
                return;
            }

            string billNo = txtBill_ID.Text;

            if (!m_billServer.AuthorizBill(billNo, out m_queryResult, out m_error))
            {
                MessageDialog.ShowPromptMessage(m_error);
                return;
            }

            string msg = string.Format("【用途】：{0} 【申请人】：{1}  ※※※ 等待【仓管员】处理",
                txtPurpose.Text, txtFillInPersonnel.Text);

            m_billMessageServer.PassFlowMessage(billNo, msg,
                        m_billMessageServer.GetRoleStringForStorage(cmbStorage.Text).ToString(), true);

            MessageDialog.ShowPromptMessage("成功批准, 等待仓库确认");

            RefreshDataGridView(m_queryResult);
            PositioningRecord(billNo);
        }

        private void 批准通过ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
                return;

            if (lblBillStatus.Text != MaterialRequisitionBillStatus.等待工艺人员批准.ToString())
            {
                MessageDialog.ShowPromptMessage("请选择要审核的记录后再进行此操作");
                return;
            }

            string billNo = txtBill_ID.Text;

            if (!m_billServer.TechnologistBill(billNo, out m_queryResult, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            string msg = string.Format("【用途】：{0} 【申请人】：{1}  ※※※ 等待【仓管员】处理",
                txtPurpose.Text, txtFillInPersonnel.Text);

            m_billMessageServer.PassFlowMessage(billNo, msg,
                        m_billMessageServer.GetRoleStringForStorage(cmbStorage.Text).ToString(), true);

            MessageDialog.ShowPromptMessage("成功批准, 等待仓库确认");

            RefreshDataGridView(m_queryResult);
            PositioningRecord(billNo);
        }

        private void 用途确认toolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lblBillStatus.Text != MaterialRequisitionBillStatus.已出库.ToString())
            {
                FormExplanation frm = new FormExplanation();

                frm.ShowDialog();

                if (frm.Explanation != null && frm.Explanation != "")
                {
                    _Sys_Log log = new _Sys_Log();

                    log.Date = ServerTime.Time;
                    log.EventInfo = "把原来的【" + m_purposeType + "】更改为【" + txtPurpose.Text + "】,原因：" + frm.Explanation;
                    log.EventType = m_purposeType;
                    log.HostName = "领料单：" + txtBill_ID.Text;
                    log.LoginName = BasicInfo.LoginName;

                    if (!m_billServer.InsertSysLog(log, out m_error))
                    {
                        MessageDialog.ShowPromptMessage(m_error);
                        return;
                    }

                    if (!m_billServer.UpdateBill(txtBill_ID.Text, txtPurpose.Tag.ToString(), out m_queryResult, out m_error))
                    {
                        MessageDialog.ShowPromptMessage(m_error);
                        return;
                    }
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("单据已经完成！");
            }

            RefreshDataGridView(m_queryResult);
            PositioningRecord(txtBill_ID.Text);
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            #region 已添加新的整台份请领单 Modify by cjb on 2015.1.22
            //foreach (DataGridViewRow dgvr in dataGridView1.Rows)
            //{
            //    if (dgvr.Cells["关联单据"].Value.ToString() == BillTypeEnum.领料单.ToString()
            //        && dgvr.Cells["单据状态"].Value.ToString() != MaterialRequisitionBillStatus.已出库.ToString())
            //    {
            //        dgvr.DefaultCellStyle.BackColor = Color.LightBlue;

            //        IEnumerable<View_S_MaterialRequisitionGoods> listGoodsInfo = m_goodsServer.GetGoods(dgvr.Cells["领料单号"].Value.ToString());

            //        foreach (View_S_MaterialRequisitionGoods goodsInfo in listGoodsInfo)
            //        {
            //            IQueryable<View_S_Stock> queryable = m_storeServer.GetGoodsStoreNorml(goodsInfo.图号型号, 
            //                goodsInfo.物品名称, goodsInfo.规格,
            //                dgvr.Cells["库房代码"].Value.ToString());

            //            if (goodsInfo.请领数 > Convert.ToDecimal((from a in queryable select a.库存数量).Sum()))
            //            {
            //                dgvr.DefaultCellStyle.BackColor = Color.Red;
            //            }
            //        }
            //    }
            //}
            #endregion
        }

        void PrintSummarySheet(string sheetName)
        {
            FormDateTime frmDateTime = new FormDateTime("请选择导出的年月");
            frmDateTime.DateTimeFormInit += new GlobalObject.DelegateCollection.FormInit(frmDateTime_DateTimeFormInit);
            frmDateTime.ShowDialog();

            DataTable tempTable = m_billServer.GetSummarySheet(frmDateTime.SelectedTime.Year.ToString() +
                frmDateTime.SelectedTime.Month.ToString("D2"), sheetName);

            ExcelHelperP.DataTableToExcel(saveFileDialog1, tempTable, null);
        }

        void frmDateTime_DateTimeFormInit(Form form)
        {
            foreach (Control col in form.Controls)
            {
                if (col is DateTimePicker)
                {
                    ((DateTimePicker)col).CustomFormat = "yyyy-MM";
                }
            }
        }

        private void 新箱装配ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintSummarySheet("新箱装配");
        }

        private void btnBoardPick_Click(object sender, EventArgs e)
        {
            看板生成领料单 frm = new 看板生成领料单();
            frm.ShowDialog();
            menuItemReresh_Click(null, null);
        }

    }
}
