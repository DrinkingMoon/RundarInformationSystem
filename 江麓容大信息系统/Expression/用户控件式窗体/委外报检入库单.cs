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
    /// 委外报检入库单界面
    /// </summary>
    public partial class 委外报检入库单 : Form
    {
        /// <summary>
        /// BOM表信息服务组件
        /// </summary>
        IBomServer m_serverBom = ServerModuleFactory.GetServerModule<IBomServer>();

        /// <summary>
        /// 库存信息服务组件
        /// </summary>
        IStoreServer m_serverStore = ServerModuleFactory.GetServerModule<IStoreServer>();

        /// <summary>
        /// 基础物品服务组件
        /// </summary>
        IBasicGoodsServer m_serverBasicGoods = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();

        /// <summary>
        /// 获取计划价格服务组件
        /// </summary>
        IBasicGoodsServer m_planCostBillServer = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();

        /// <summary>
        /// 条形码
        /// </summary>
        IBarCodeServer m_barCodeServer = ServerModuleFactory.GetServerModule<IBarCodeServer>();

        /// <summary>
        /// 可供查找的所有字段
        /// </summary>
        string[] m_findField = null;

        /// <summary>
        /// 查找条件窗体
        /// </summary>
        FormConditionFind m_formFindCondition;

        /// <summary>
        /// 服务组件
        /// </summary>
        ICheckOutInDepotForOutsourcingServer m_serverCheckOutInDepotForOutsourcing = ServerModuleFactory.GetServerModule<ICheckOutInDepotForOutsourcingServer>();

        /// <summary>
        /// 数据集
        /// </summary>
        S_CheckOutInDepotForOutsourcingBill m_lnqBill = new S_CheckOutInDepotForOutsourcingBill();

        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag;

        public 委外报检入库单(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_billMessageServer.BillType = "委外报检入库单";

            m_billNoControl = new BillNumberControl(CE_BillTypeEnum.委外报检入库单, m_serverCheckOutInDepotForOutsourcing);

            m_authFlag = nodeInfo.Authority;

            // 添加计量单位
            cmbUnit.Items.AddRange(StapleInfo.Units);

            if (cmbUnit.Items.Count > 0)
            {
                cmbUnit.SelectedIndex = 0;
            }

            DataTable dt = UniversalFunction.GetStorageTb();
            dt = DataSetHelper.SiftDataTable(dt, "ZeroCostFlag = 0", out m_err);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cmbStorage.Items.Add(dt.Rows[i]["StorageName"].ToString());
            }

            cmbStorage.SelectedIndex = -1;

            string[] strBillStatus = { "全  部", 
                                     "新建单据", 
                                     "等待财务批准",
                                     "等待仓管确认到货",
                                     "等待质检机检验",
                                     "等待质检电检验",
                                     "等待入库",
                                     "已入库"};

            checkBillDateAndStatus1.InsertComBox(strBillStatus);
            checkBillDateAndStatus1.OnCompleteSearch +=
                new GlobalObject.DelegateCollection.NonArgumentHandle(checkBillDateAndStatus1_OnCompleteSearch);

            #region 被要求使用服务器时间 Modify by cjb on 2012.6.15
            checkBillDateAndStatus1.dtpStartTime.Value = ServerTime.Time.AddDays(1).AddMonths(-1);
            checkBillDateAndStatus1.dtpEndTime.Value = ServerTime.Time.AddDays(1);

            #endregion

            numOutsourcingUnitPrice.Enabled = 采购员操作ToolStripMenuItem.Visible;

            RefreshData();
        }

        private void 委外报检入库单_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authFlag);


            if (BasicInfo.ListRoles.Contains(CE_RoleEnum.采购账务管理员.ToString())
                || BasicInfo.ListRoles.Contains(CE_RoleEnum.会计.ToString()))
            {
                label32.Visible = true;
                label23.Visible = true;
                label24.Visible = true;
                numUnitPrice.Visible = true;
                numPrice.Visible = true;
                numOutsourcingUnitPrice.Visible = true;
            }
            else
            {
                label32.Visible = false;
                label23.Visible = false;
                label24.Visible = false;
                numUnitPrice.Visible = false;
                numPrice.Visible = false;
                numOutsourcingUnitPrice.Visible = false;
            }
        }

        void checkBillDateAndStatus1_OnCompleteSearch()
        {
            RefreshData();
        }

        /// <summary>
        /// 查找并刷新数据
        /// </summary>
        private void RefreshData()
        {
            ClearMessage();

            m_serverCheckOutInDepotForOutsourcing.QueryResultFilter = QueryFilterControl.GetFilterString(labelTitle.Text)
                + checkBillDateAndStatus1.GetSqlString("报检时间", "单据状态");

            IQueryResult result;

            if (!m_serverCheckOutInDepotForOutsourcing.GetBill(out result, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            RefreshDataGirdView(result);
        }

        /// <summary>
        /// 获取数据源
        /// </summary>
        /// <param name="findBill">查询的数据结果</param>
        /// <returns>返回数据表</returns>
        private DataTable GetDataSource(IQueryResult findBill)
        {
            if (findBill == null || !findBill.Succeeded || findBill.DataCollection == null || findBill.DataCollection.Tables.Count == 0)
            {
                return null;
            }

            DataTable dt = findBill.DataCollection.Tables[0];
            DataTable dataSource = dt.Clone();
            string filterExpression = "";

            if (仅显示未确认到货数据.Checked)
            {
                filterExpression = "单据状态 = '等待仓管确认到货'";
            }

            if (仅显示等待质检数据.Checked)
            {
                if (filterExpression.Length == 0)
                    filterExpression = "单据状态 like '%等待质检%'";
                else
                    filterExpression += " or (单据状态 like '%等待质检%')";
            }

            if (仅显示等待入库数据.Checked)
            {
                if (filterExpression.Length == 0)
                    filterExpression = "单据状态 = '等待入库'";
                else
                    filterExpression += " or (单据状态 = '等待入库')";
            }

            if (仅显示已入库数据.Checked)
            {
                if (filterExpression.Length == 0)
                    filterExpression = "单据状态 = '已入库'";
                else
                    filterExpression += " or (单据状态 = '已入库')";
            }

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(filterExpression))
            {
                dataSource = dt;
            }
            else
            {
                DataRow[] rows = dt.Select(filterExpression);

                foreach (var row in rows)
                {
                    dataSource.ImportRow(row);
                }
            }

            return dataSource;
        }

        /// <summary>
        /// 获得数据
        /// </summary>
        void GetMessage()
        {
            m_lnqBill.ArrivePersonnel = BasicInfo.LoginName;
            m_lnqBill.OrderFormNumber = txtOrderFormNumber.Text;
            m_lnqBill.BatchNo = txtBatchNo.Text;
            m_lnqBill.Bill_ID = txtBill_ID.Text;
            m_lnqBill.BillStatus = lblBillStatus.Text;
            m_lnqBill.Checker = txtChecker.Text;
            m_lnqBill.CheckoutReport_ID = txtCheckoutReportID.Text;
            m_lnqBill.ColumnNumber = txtColumn.Text;
            m_lnqBill.ConcessionCount = numConcessionCount.Value;
            m_lnqBill.DeclareCount = numDeclareCount.Value;
            m_lnqBill.DeclarePersonnel = lbDeclarePersonnel.Text;
            m_lnqBill.DeclareTime = dateTime_BillTime.Value;
            m_lnqBill.DeclareWastrelCount = numDeclareWastrelCount.Value;
            m_lnqBill.Depot = txtMaterialType.Text;
            m_lnqBill.DepotManagerAffirmCount = numAffirmCount.Value;
            m_lnqBill.EligibleCount = numEligibleCount.Value;
            m_lnqBill.FinancePersonnel = BasicInfo.LoginName;
            m_lnqBill.GoodsID = Convert.ToInt32(txtCode.Tag);
            m_lnqBill.InDepotCount = numInDepotCount.Value;
            m_lnqBill.IsExigenceCheck = chkIsExigenceCheck.Checked;
            m_lnqBill.IsIncludeRawMaterial = chkIsIncludeRawMaterial.Checked;
            m_lnqBill.IsOnlyForRepairFlag = chkOnlyForRepair.Checked;
            m_lnqBill.LayerNumber = txtLayer.Text;
            m_lnqBill.ManagerPersonnel = txtDepotManager.Text;
            m_lnqBill.OutsourcingUnitPrice = numOutsourcingUnitPrice.Value;
            m_lnqBill.PeremptorilyEmit = chk紧急放行.Checked;
            m_lnqBill.Price = numPrice.Value;
            m_lnqBill.Provider = txtProvider.Text;
            m_lnqBill.ProviderBatchNo = txtProviderBatchNo.Text;
            m_lnqBill.QualityInfo = txtQualityInfo.Text;
            m_lnqBill.QualityPersonnel = txtQualityManager.Text;
            m_lnqBill.QualityTime = dateTime_CheckTime.Value;
            m_lnqBill.RawMaterialPrice = numRawMaterialPrice.Value;
            m_lnqBill.ReimbursementCount = numReimbursementCount.Value;
            m_lnqBill.Remark = txtRemark.Text;
            m_lnqBill.ShelfArea = txtShelf.Text;
            m_lnqBill.StorageID = UniversalFunction.GetStorageID(cmbStorage.Text);
            m_lnqBill.UnitPrice = numUnitPrice.Value;
            m_lnqBill.Version = cbVersion.Text;
        }

        /// <summary>
        /// 显示数据
        /// </summary>
        void ShowMessage()
        {
            View_F_GoodsPlanCost tempGoodsLnq = UniversalFunction.GetGoodsInfo(m_lnqBill.GoodsID);

            if (tempGoodsLnq == null)
            {
                MessageDialog.ShowPromptMessage("系统中无此物品信息");
            }

            lblBillStatus.Text = m_lnqBill.BillStatus;
            lbDeclarePersonnel.Text = m_lnqBill.DeclarePersonnel;

            txtCode.Tag = m_lnqBill.GoodsID;
            cbVersion.Text = m_lnqBill.Version;
            txtSpec.Text = tempGoodsLnq.规格;
            txtShelf.Text = m_lnqBill.ShelfArea;
            txtRemark.Text = m_lnqBill.Remark;
            txtQualityManager.Text = m_lnqBill.QualityPersonnel;
            txtQualityInfo.Text = m_lnqBill.QualityInfo;
            txtProviderBatchNo.Text = m_lnqBill.ProviderBatchNo;
            txtProvider.Text = m_lnqBill.Provider;
            txtName.Text = tempGoodsLnq.物品名称;
            txtMaterialType.Text = m_lnqBill.Depot;
            txtLayer.Text = m_lnqBill.LayerNumber;
            txtDepotManager.Text = m_lnqBill.ManagerPersonnel;
            txtColumn.Text = m_lnqBill.ColumnNumber;
            txtCode.Text = tempGoodsLnq.图号型号;
            txtCheckoutReportID.Text = m_lnqBill.CheckoutReport_ID;
            txtChecker.Text = m_lnqBill.Checker;
            txtBill_ID.Text = m_lnqBill.Bill_ID;
            txtBatchNo.Text = m_lnqBill.BatchNo;
            txtOrderFormNumber.Text = m_lnqBill.OrderFormNumber;

            cmbStorage.Text = UniversalFunction.GetStorageName(m_lnqBill.StorageID);
            cmbUnit.Text = tempGoodsLnq.单位;

            numAffirmCount.Value = m_lnqBill.DepotManagerAffirmCount;
            numConcessionCount.Value = m_lnqBill.ConcessionCount;
            numDeclareCount.Value = m_lnqBill.DeclareCount;
            numDeclareWastrelCount.Value = m_lnqBill.DeclareWastrelCount;
            numEligibleCount.Value = m_lnqBill.EligibleCount;
            numInDepotCount.Value = m_lnqBill.InDepotCount;
            numOutsourcingUnitPrice.Value = m_lnqBill.OutsourcingUnitPrice;
            numPrice.Value = m_lnqBill.Price;
            numRawMaterialPrice.Value = m_lnqBill.RawMaterialPrice;
            numReimbursementCount.Value = m_lnqBill.ReimbursementCount;
            numUnitPrice.Value = m_lnqBill.UnitPrice;

            chkIsExigenceCheck.Checked = m_lnqBill.IsExigenceCheck;
            chkIsIncludeRawMaterial.Checked = m_lnqBill.IsIncludeRawMaterial;
            chkOnlyForRepair.Checked = m_lnqBill.IsOnlyForRepairFlag == null ? false : (bool)m_lnqBill.IsOnlyForRepairFlag;
            chk紧急放行.Checked = m_lnqBill.PeremptorilyEmit;

            dateTime_BillTime.Value = m_lnqBill.DeclareTime == null ? ServerTime.Time : Convert.ToDateTime(m_lnqBill.DeclareTime);
            dateTime_CheckTime.Value = m_lnqBill.QualityTime == null ? ServerTime.Time : Convert.ToDateTime(m_lnqBill.QualityTime);
        }

        /// <summary>
        /// 初始化界面
        /// </summary>
        void ClearMessage()
        {
            txtOrderFormNumber.Text = "";
            txtBatchNo.Text = "";
            txtBill_ID.Text = "";
            txtChecker.Text = "";
            txtCheckoutReportID.Text = "";
            txtCode.Text = "";
            txtColumn.Text = "";
            txtDepotManager.Text = "";
            txtLayer.Text = "";
            txtMaterialType.Text = "";
            txtName.Text = "";
            txtProvider.Text = "";
            txtProviderBatchNo.Text = "";
            txtQualityInfo.Text = "";
            txtQualityManager.Text = "";
            txtRemark.Text = "";
            txtShelf.Text = "";
            txtSpec.Text = "";
            cbVersion.Text = "";

            cmbStorage.SelectedIndex = -1;
            cmbUnit.SelectedIndex = -1;

            numAffirmCount.Value = 0;
            numConcessionCount.Value = 0;
            numDeclareCount.Value = 0;
            numDeclareWastrelCount.Value = 0;
            numEligibleCount.Value = 0;
            numInDepotCount.Value = 0;
            numOutsourcingUnitPrice.Value = 0;
            numPrice.Value = 0;
            numRawMaterialPrice.Value = 0;
            numReimbursementCount.Value = 0;
            numUnitPrice.Value = 0;

            chkIsExigenceCheck.Checked = false;
            chkIsIncludeRawMaterial.Checked = false;
            chkOnlyForRepair.Checked = false;
            chk紧急放行.Checked = false;

            dateTime_BillTime.Value = ServerTime.Time;
            dateTime_CheckTime.Value = ServerTime.Time;
        }

        /// <summary>
        /// 设置金额
        /// </summary>
        void SetPrice()
        {
            numUnitPrice.Value = numOutsourcingUnitPrice.Value + numRawMaterialPrice.Value;

            if (numInDepotCount.Value == 0)
            {
                if (numAffirmCount.Value == 0)
                {
                    numPrice.Value = Math.Round(numUnitPrice.Value * numDeclareCount.Value, 2);
                }
                else
                {
                    numPrice.Value = Math.Round(numUnitPrice.Value * numAffirmCount.Value, 2);
                }
            }
            else
            {
                numPrice.Value = Math.Round(numUnitPrice.Value * numInDepotCount.Value, 2);
            }
        }

        /// <summary>
        /// 权限控制
        /// </summary>
        /// <param name="authorityFlag">权限标志</param>
        void AuthorityControl(PlatformManagement.AuthorityFlag authorityFlag)
        {
            FaceAuthoritySetting.SetVisibly(menuStrip, authorityFlag);
            FaceAuthoritySetting.SetEnable(this.Controls, authorityFlag);

            if (((authorityFlag & AuthorityFlag.ConfirmArrival) != AuthorityFlag.Nothing) ||
                ((authorityFlag & AuthorityFlag.StockIn) != AuthorityFlag.Nothing))
            {
                仓库管理员操作ToolStripMenuItem.Visible = true;
            }
        }

        /// <summary>
        /// 隐藏控件
        /// </summary>
        void HideControl()
        {
            //if (dataGridView1.CurrentRow.Cells["金额"].Visible)
            //{
            //    label6.Visible = true;
            //    label32.Visible = true;
            //    label23.Visible = true;
            //    label24.Visible = true;
            //    numRawMaterialPrice.Visible = true;
            //    numOutsourcingUnitPrice.Visible = true;
            //    numUnitPrice.Visible = true;
            //    numPrice.Visible = true;
            //}
            //else
            //{
            //    label6.Visible = false;
            //    label32.Visible = false;
            //    label23.Visible = false;
            //    label24.Visible = false;
            //    numRawMaterialPrice.Visible = false;
            //    numOutsourcingUnitPrice.Visible = false;
            //    numUnitPrice.Visible = false;
            //    numPrice.Visible = false;
            //}
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="source">数据集</param>
        void RefreshDataGirdView(IQueryResult findBill)
        {
            dataGridView1.DataSource = GetDataSource(findBill);

            if (BasicInfo.ListRoles.Contains(CE_RoleEnum.采购账务管理员.ToString())
                || BasicInfo.ListRoles.Contains(CE_RoleEnum.会计.ToString()))
            {
                dataGridView1.Columns["单价"].Visible = true;
                dataGridView1.Columns["金额"].Visible = true;
                dataGridView1.Columns["委外费"].Visible = true;
            }
            else
            {
                dataGridView1.Columns["单价"].Visible = false;
                dataGridView1.Columns["金额"].Visible = false;
                dataGridView1.Columns["委外费"].Visible = false;
            }

            this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);


            #region 隐藏不允许查看的列

            if (findBill.HideFields != null && findBill.HideFields.Length != 0)
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

            userControlDataLocalizer1.Init(dataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
        }

        /// <summary>
        /// 检测有关采购数据项内容是否正确
        /// </summary>
        /// <returns>返回检测结果</returns>
        bool CheckDataItem()
        {
            if (txtOrderFormNumber.Text.Length == 0)
            {
                txtOrderFormNumber.Focus();
                MessageDialog.ShowPromptMessage(@"订单号不允许为空");
                return false;
            }

            if (cbVersion.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("版次号不能为空");
                return false;
            }

            if (cbVersion.SelectedIndex > 0 && txtRemark.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("所选择的版次号不是最新的版次号，请在‘备注’中说明原因！");
                return false;
            }

            if (numDeclareCount.Value == 0)
            {
                numDeclareCount.Focus();
                MessageDialog.ShowPromptMessage("报检数不允许为0!");
                return false;
            }

            if (txtName.Text == "")
            {
                txtName.Focus();
                MessageDialog.ShowPromptMessage("物品名称不允许为空!");
                return false;
            }

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(txtMaterialType.Text))
            {
                MessageDialog.ShowPromptMessage("获取不到材料类别, 请与管理员联系");
                return false;
            }

            if (txtProvider.Text == "")
            {
                txtProvider.Focus();
                MessageDialog.ShowPromptMessage("供货单位不允许为空!");
                return false;
            }

            if (cmbUnit.Text == "")
            {
                cmbUnit.Focus();
                MessageDialog.ShowPromptMessage("单位不允许为空!");
                return false;
            }

            if (cmbStorage.Text == "")
            {
                cmbStorage.Focus();
                MessageDialog.ShowPromptMessage("所属库房不允许为空!");
                return false;
            }

            if (numUnitPrice.Value == 0)
            {
                MessageDialog.ShowPromptMessage("单价不可为0,请重新确认单价");
                return false;
            }

            if (txtProviderBatchNo.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("供方批次号不可为空");
                return false;
            }

            return true;
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

                    DataTable dtMessage = UniversalFunction.PositioningOneRecord(msg.MessageContent, "委外报检入库单");

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

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void 仅显示未确认到货数据_Click(object sender, EventArgs e)
        {
            仅显示未确认到货数据.Checked = !仅显示未确认到货数据.Checked;
            btnRefresh_Click(sender, e);
        }

        private void 仅显示等待质检数据_Click(object sender, EventArgs e)
        {
            仅显示等待质检数据.Checked = !仅显示等待质检数据.Checked;
            btnRefresh_Click(sender, e);
        }

        private void 仅显示等待入库数据_Click(object sender, EventArgs e)
        {
            仅显示等待入库数据.Checked = !仅显示等待入库数据.Checked;
            btnRefresh_Click(sender, e);
        }

        private void 仅显示已入库数据_Click(object sender, EventArgs e)
        {
            仅显示已入库数据.Checked = !仅显示已入库数据.Checked;
            btnRefresh_Click(sender, e);
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.RowCount == 0)
            {
                return;
            }

            m_lnqBill = m_serverCheckOutInDepotForOutsourcing.GetBill(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString());

            仓库管理员操作ToolStripMenuItem.Visible = 
                UniversalFunction.CheckStorageAndPersonnel(dataGridView1.CurrentRow.Cells["库房ID"].Value.ToString());
            ShowMessage();

            if (财务操作ToolStripMenuItem.Visible && chkIsIncludeRawMaterial.Checked)
            {
                numRawMaterialPrice.Enabled = true;
            }
            else
            {
                numRawMaterialPrice.Enabled = false;
            }

            HideControl();

            chkPrint.Checked = dataGridView1.CurrentRow.Cells["是否已打印"].Value.ToString() == "" ? false :
                Convert.ToBoolean(dataGridView1.CurrentRow.Cells["是否已打印"].Value);


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

        private void 新建单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearMessage();

            string billno = m_billNoControl.GetNewBillNo();
            lblBillStatus.Text = "新建单据";
            txtBill_ID.Text = billno;
            txtBatchNo.Text = billno;
            lbDeclarePersonnel.Text = BasicInfo.LoginName;

        }

        private void 采购员提交单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lblBillStatus.Text != "新建单据"
                && lblBillStatus.Text != "等待财务批准"
                && lblBillStatus.Text != "等待仓管确认到货")
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
                return;

            }

            if (!CheckDataItem())
            {
                return;
            }

            lbDeclarePersonnel.Text = BasicInfo.LoginName;

            GetMessage();

            if (!m_serverCheckOutInDepotForOutsourcing.AddBill(m_lnqBill, out m_err))
            {
                MessageDialog.ShowPromptMessage(m_err);
            }
            else
            {
                m_billMessageServer.DestroyMessage(m_lnqBill.Bill_ID);

                if (m_lnqBill.IsIncludeRawMaterial)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("物品【{0}】，批次号【{1}】  ※※※ 等待【会计】处理", txtName.Text, txtBatchNo.Text);
                    m_billMessageServer.SendNewFlowMessage(m_lnqBill.Bill_ID, sb.ToString(), 
                        BillFlowMessage_ReceivedUserType.角色, CE_RoleEnum.会计.ToString());
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("物品【{0}】，批次号【{1}】  ※※※ 等待【仓管员】处理", txtName.Text, txtBatchNo.Text);
                    m_billMessageServer.SendNewFlowMessage(m_lnqBill.Bill_ID, sb.ToString(), 
                        BillFlowMessage_ReceivedUserType.角色, m_billMessageServer.GetRoleStringForStorage(m_lnqBill.StorageID).ToString());
                }

                MessageDialog.ShowPromptMessage("提交成功");
            }

            RefreshData();
            PositioningRecord(m_lnqBill.Bill_ID);
        }

        private void 提交质检信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lblBillStatus.Text != "等待质检机检验"
                && lblBillStatus.Text != "等待质检电检验")
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
                return;
            }

            if (dateTime_CheckTime.Value < dateTime_BillTime.Value)
            {
                dateTime_CheckTime.Focus();
                MessageDialog.ShowPromptMessage("检验入库时间必须 >= 报检时间");
                return;
            }

            if (numEligibleCount.Value != numAffirmCount.Value && txtQualityInfo.Text == "")
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
                + numDeclareWastrelCount.Value != numAffirmCount.Value)
            {
                numEligibleCount.Focus();
                MessageDialog.ShowPromptMessage("合格数 + 让步数 + 退货数 + 报废数 之和不等于确认到货数量");
                return;
            }

            if (txtChecker.Text == "")
            {
                txtChecker.Focus();
                MessageDialog.ShowPromptMessage("检验人员不能为空");
                return;
            }

            if (numConcessionCount.Value > 0 || numReimbursementCount.Value > 0)
            {
                不合格品信息 form = new 不合格品信息(txtBill_ID.Text);
                form.ShowDialog();
                if (!form.BlFlag)
                {
                    MessageBox.Show("请完整填写不合格信息单，并且保存！", "提示");
                    return;
                }
            }

            txtQualityManager.Text = BasicInfo.LoginName;

            GetMessage();

            if (!m_serverCheckOutInDepotForOutsourcing.UpdateBill(m_lnqBill, out m_err))
            {
                MessageDialog.ShowPromptMessage(m_err);
            }
            else
            {
                List<string> strList = null;

                strList.Add(CE_RoleEnum.质检室主管.ToString());
                strList.Add(CE_RoleEnum.质控负责人.ToString());

                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("物品【{0}】，批次号【{1}】  ※※※ 等待【质量主管】处理", txtName.Text, txtBatchNo.Text);
                m_billMessageServer.PassFlowMessage(m_lnqBill.Bill_ID, sb.ToString(),BillFlowMessage_ReceivedUserType.角色,
                    strList);

                MessageDialog.ShowPromptMessage("提交成功");
            }

            RefreshData();
            PositioningRecord(m_lnqBill.Bill_ID);
        }

        private void 财务批准ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lblBillStatus.Text != "等待财务批准")
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
                return;
            }

            if (numRawMaterialPrice.Value == 0)
            {
                MessageDialog.ShowPromptMessage("原材料费不可为0");
                return;
            }


            numPrice.Value = numDeclareCount.Value * numUnitPrice.Value;

            GetMessage();

            if (!m_serverCheckOutInDepotForOutsourcing.UpdateBill(m_lnqBill, out m_err))
            {
                MessageDialog.ShowPromptMessage(m_err);
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("物品【{0}】，批次号【{1}】  ※※※ 等待【仓管员】处理", txtName.Text, txtBatchNo.Text);
                m_billMessageServer.PassFlowMessage(m_lnqBill.Bill_ID, sb.ToString(), CE_RoleEnum.检验员.ToString(), true);

                MessageDialog.ShowPromptMessage("提交成功");
            }

            RefreshData();
            PositioningRecord(m_lnqBill.Bill_ID);
        }

        private void 确认到货数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lblBillStatus.Text != "等待仓管确认到货")
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
                return;
            }

            if (numAffirmCount.Value == 0m || numAffirmCount.Value > numDeclareCount.Value)
            {
                numAffirmCount.Focus();
                MessageDialog.ShowPromptMessage("确认到货数必须 > 0 且 <= 报检数");
                return;
            }

            numPrice.Value = numAffirmCount.Value * numUnitPrice.Value;

            GetMessage();

            m_lnqBill.QualityInfo = "等待质检机检验";

            if (!m_serverCheckOutInDepotForOutsourcing.UpdateBill(m_lnqBill, out m_err))
            {
                MessageDialog.ShowPromptMessage(m_err);
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("物品【{0}】，批次号【{1}】  ※※※ 等待【检验员】处理", txtName.Text, txtBatchNo.Text);
                m_billMessageServer.PassFlowMessage(m_lnqBill.Bill_ID, sb.ToString(), CE_RoleEnum.机械检验组长.ToString(), true);

                MessageDialog.ShowPromptMessage("提交成功");
            }

            RefreshData();
            PositioningRecord(m_lnqBill.Bill_ID);
        }

        private void 零件入库ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lblBillStatus.Text != "等待入库")
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
                return;
            }

            if (numEligibleCount.Value + numConcessionCount.Value > 0)
            {
                if (numInDepotCount.Value != numEligibleCount.Value + numConcessionCount.Value + numDeclareWastrelCount.Value)
                {
                    numInDepotCount.Focus();
                    MessageDialog.ShowPromptMessage("入库数必须= 合格数量 + 让步数量 + 报废数量");
                    return;
                }
            }

            if (numInDepotCount.Value > 0)
            {
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
            }

            numPrice.Value = numInDepotCount.Value * numUnitPrice.Value;

            GetMessage();

            if (!m_serverCheckOutInDepotForOutsourcing.UpdateBill(m_lnqBill, out m_err))
            {
                MessageDialog.ShowPromptMessage(m_err);
            }
            else
            {
                m_billNoControl.UseBill(m_lnqBill.Bill_ID);

                #region 发送知会消息


                List<string> noticeRoles = new List<string>();

                noticeRoles.Add(CE_RoleEnum.质量工程师.ToString());
                noticeRoles.Add(CE_RoleEnum.质控主管.ToString());
                noticeRoles.Add(m_billMessageServer.GetRoleStringForStorage(m_lnqBill.StorageID).ToString());
                noticeRoles.Add(CE_RoleEnum.检验员.ToString());

                if (m_lnqBill.IsIncludeRawMaterial)
                {
                    noticeRoles.Add(CE_RoleEnum.会计.ToString());
                }


                m_billMessageServer.EndFlowMessage(m_lnqBill.Bill_ID,
                    string.Format("{0} 号委外报检入库单已经处理完毕", m_lnqBill.Bill_ID),
                    noticeRoles, null);

                #endregion 发送知会消息


                MessageDialog.ShowPromptMessage("提交成功");

            }

            RefreshData();
            PositioningRecord(m_lnqBill.Bill_ID);
        }

        private void btnFindOrderForm_Click(object sender, EventArgs e)
        {
            if (lblBillStatus.Text != "新建单据")
            {
                return;
            }

            string personnel = "";

            if (BasicInfo.ListRoles.Contains(CE_RoleEnum.业务系统管理员.ToString()) || BasicInfo.ListRoles.Contains(CE_RoleEnum.采购主管.ToString()))
            {
                personnel = "全部";
            }
            else
            {
                personnel = BasicInfo.LoginName;
            }

            FormQueryInfo form = QueryInfoDialog.GetOrderFormInfoDialog(CE_BillTypeEnum.委外报检入库单);

            if (DialogResult.OK == form.ShowDialog())
            {
                txtCode.Text = "";
                txtName.Text = "";
                txtSpec.Text = "";

                txtOrderFormNumber.Text = form.GetDataItem("订单号").ToString();
                txtProvider.Text = form.GetDataItem("供货单位").ToString();
            }
        }

        private void btnFindCode_Click(object sender, EventArgs e)
        {
            if (txtOrderFormNumber.Text.Length == 0)
            {
                txtOrderFormNumber.Focus();
                MessageDialog.ShowPromptMessage(@"请先选择订单号后再进行此操作！");
                return;
            }

            FormQueryInfo form = QueryInfoDialog.GetOrderFormGoodsDialog(txtOrderFormNumber.Text, true);

            if (form == null || form.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            txtCode.Tag = (int)form.GetDataItem("物品ID");
            txtCode.Text = form.GetDataItem("图号型号").ToString();
            txtName.Text = form.GetDataItem("物品名称").ToString();
            txtSpec.Text = form.GetDataItem("规格").ToString();

            #region 获取物品ID
            View_F_GoodsPlanCost basicGoodsInfo =
                m_planCostBillServer.GetGoodsInfo(txtCode.Text, txtName.Text, txtSpec.Text, out m_err);

            if (!GlobalObject.GeneralFunction.IsNullOrEmpty(m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            txtMaterialType.Text = basicGoodsInfo.物品类别;
            cmbUnit.Text = basicGoodsInfo.单位;
            #endregion

            #region 获取单价

            IOrderFormGoodsServer orderFormGoodsServer = ServerModuleFactory.GetServerModule<IOrderFormGoodsServer>();

            decimal dcUnitPrice = orderFormGoodsServer.GetGoodsUnitPrice(txtOrderFormNumber.Text, (int)basicGoodsInfo.序号);

            numUnitPrice.Value = dcUnitPrice;
            numOutsourcingUnitPrice.Value = dcUnitPrice;

            #endregion

            DataRow dr = m_serverBom.GetBomInfo(txtCode.Text.Trim(), txtName.Text.Trim());

            if (dr == null)
            {
                cbVersion.Text = "";
            }
            else
            {
                DataTable dtVersion = m_serverBom.GetVersion(txtCode.Text.Trim(), txtName.Text.Trim());

                if (dtVersion.Rows.Count > 0)
                {
                    cbVersion.DataSource = dtVersion;
                    cbVersion.DisplayMember = "旧零件版次号";
                    cbVersion.ValueMember = "旧零件版次号";
                    cbVersion.DropDownStyle = ComboBoxStyle.DropDownList;
                }
                else
                    cbVersion.DropDownStyle = ComboBoxStyle.DropDown;
            }
        }

        private void numAffirmCount_ValueChanged(object sender, EventArgs e)
        {
            SetPrice();
        }

        private void numDeclareCount_ValueChanged(object sender, EventArgs e)
        {
            SetPrice();
        }

        private void numRawMaterialPrice_ValueChanged(object sender, EventArgs e)
        {
            SetPrice();
        }

        private void numOutsourcingUnitPrice_ValueChanged(object sender, EventArgs e)
        {
            SetPrice();
        }

        private void numInDepotCount_ValueChanged(object sender, EventArgs e)
        {
            SetPrice();
        }

        private void 报废单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lblBillStatus.Text == "单据已完成")
            {
                MessageDialog.ShowPromptMessage("已完成的单据不能删除");
                return;
            }

            string billno = dataGridView1.CurrentRow.Cells["单据号"].Value.ToString();

            if (MessageDialog.ShowEnquiryMessage("您是否要删除此单据") == DialogResult.No)
            {
                return;
            }

            if (!m_serverCheckOutInDepotForOutsourcing.DeleteBill(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString(), out m_err))
            {
                MessageDialog.ShowPromptMessage(m_err);
                return;
            }
            else
            {
                m_billMessageServer.DestroyMessage(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString());
                m_billNoControl.CancelBill(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString());
                MessageDialog.ShowPromptMessage("删除成功");
            }

            RefreshData();
            PositioningRecord(billno);
        }

        private void btnFindChecker_Click(object sender, EventArgs e)
        {
            FormPersonnel form = new FormPersonnel(txtChecker, BasicInfo.DeptCode, "姓名");
            form.ShowDialog();
        }

        private void btnPrintBarCode_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count < 1)
            {
                MessageDialog.ShowPromptMessage("请选择已入库的记录后再打印条形码");
                return;
            }

            List<View_S_InDepotGoodsBarCodeTable> lstBarCodeInfo = new List<View_S_InDepotGoodsBarCodeTable>();

            for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
            {
                string goodsCode = dataGridView1.SelectedRows[i].Cells["图号型号"].Value.ToString();
                string goodsName = dataGridView1.SelectedRows[i].Cells["物品名称"].Value.ToString();
                string spec = dataGridView1.SelectedRows[i].Cells["规格"].Value.ToString();
                string provider = dataGridView1.SelectedRows[i].Cells["供应商编码"].Value.ToString();
                string batchCode = dataGridView1.SelectedRows[i].Cells["批次号"].Value.ToString();
                string StorageID = dataGridView1.SelectedRows[i].Cells["库房ID"].Value.ToString();

                IBarCodeServer server = ServerModuleFactory.GetServerModule<IBarCodeServer>();
                View_S_InDepotGoodsBarCodeTable barcode = server.GetBarCodeInfo(goodsCode, goodsName, spec, provider, batchCode, StorageID);

                if (barcode == null)
                {
                    S_InDepotGoodsBarCodeTable newBarcode = new S_InDepotGoodsBarCodeTable();

                    newBarcode.GoodsID = m_planCostBillServer.GetGoodsID(goodsCode, goodsName, spec);
                    newBarcode.Provider = provider;
                    newBarcode.BatchNo = batchCode;
                    newBarcode.ProductFlag = "0";
                    newBarcode.StorageID = StorageID;

                    if (!m_barCodeServer.Add(newBarcode, out m_err))
                    {
                        MessageDialog.ShowErrorMessage(m_err);
                        return;
                    }

                    barcode = m_barCodeServer.GetBarCodeInfo(goodsCode, goodsName, spec, provider, batchCode, StorageID);
                }

                lstBarCodeInfo.Add(barcode);
            }

            foreach (var item in lstBarCodeInfo)
            {
                ServerModule.PrintPartBarcode.PrintBarcodeList(item);
            }
        }

        private void 表单打印ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择至少一条已入库记录后再行此操作");
                return;
            }

            int index = 0;

            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                string status = row.Cells["单据状态"].Value.ToString();

                if (status != "已入库")
                {
                    continue;
                }

                string reportTitle = "委外报检入库单";

                报表_委外报检入库单 report = new 报表_委外报检入库单(row.Cells["单据号"].Value.ToString(), labelTitle.Text, reportTitle);
                report.ShowDialog();

                PrintReportBill print = new PrintReportBill(21.8, 9.31, report);

                if (index++ > 0)
                {
                    print.ShowPrintDialog = false;
                }

                print.DirectPrintReport();
            }
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dataGridView1.ClearSelection();

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewRow row = dataGridView1.Rows[i];

                if (row.Cells["单据状态"].Value.ToString() == "已入库")
                {
                    row.DefaultCellStyle.ForeColor = Color.Green;
                }

                if (row.Cells["退货数"].Value != System.DBNull.Value && row.Cells["退货数"].Value != null)
                {
                    int value = Convert.ToInt32(row.Cells["退货数"].Value);

                    if (value > 0)
                    {
                        row.DefaultCellStyle.ForeColor = Color.Orange;
                    }
                }

                if ((bool)row.Cells["是否紧急放行"].Value)
                {
                    row.DefaultCellStyle.BackColor = Color.Yellow;
                }

                if ((bool)row.Cells["是否紧急报检"].Value)
                {
                    row.DefaultCellStyle.ForeColor = Color.Red;
                }
            }
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            FormViewData form = new FormViewData(dataGridView1.Columns, dataGridView1.Rows[e.RowIndex]);
            form.ShowDialog();
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

        private void 回退单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (财务操作ToolStripMenuItem.Visible == true
                && lblBillStatus.Text == "等待财务批准")
            {
                ReturnBillStatus();
            }
            else if (仓库管理员操作ToolStripMenuItem.Visible == true
                && lblBillStatus.Text == "等待仓管确认到货")
            {
                ReturnBillStatus();
            }
            else if (质检员操作ToolStripMenuItem.Visible == true
                && (lblBillStatus.Text == "等待质检机检验" || lblBillStatus.Text == "等待质检电检验"))
            {
                ReturnBillStatus();
            }
            else if (仓库管理员操作ToolStripMenuItem.Visible == true
                && lblBillStatus.Text == "等待入库")
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
                回退单据 form = new 回退单据(CE_BillTypeEnum.委外报检入库单, txtBill_ID.Text, lblBillStatus.Text);

                if (form.ShowDialog() == DialogResult.OK)
                {
                    if (MessageBox.Show("您确定要回退此单据吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        if (m_serverCheckOutInDepotForOutsourcing.ReturnBill(form.StrBillID, form.StrBillStatus, out m_err, form.Reason))
                        {
                            MessageDialog.ShowPromptMessage("回退成功");
                            RefreshData();
                            PositioningRecord(form.StrBillID);
                        }
                        else
                        {
                            MessageDialog.ShowPromptMessage(m_err);
                        }

                        Refresh();
                    }
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
            }
        }

        private void 审核通过ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lblBillStatus.Text != "等待质量主管审核")
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
                return;
            }

            GetMessage();

            if (!m_serverCheckOutInDepotForOutsourcing.UpdateBill(m_lnqBill, out m_err))
            {
                MessageDialog.ShowPromptMessage(m_err);
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("物品【{0}】，批次号【{1}】  ※※※ 等待【仓管员】处理", txtName.Text, txtBatchNo.Text);
                m_billMessageServer.PassFlowMessage(m_lnqBill.Bill_ID, sb.ToString(), m_billMessageServer.GetRoleStringForStorage(m_lnqBill.StorageID).ToString(), true);

                MessageDialog.ShowPromptMessage("提交成功");
            }

            RefreshData();
            PositioningRecord(m_lnqBill.Bill_ID);
        }
    }
}
