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
    /// 报检入库单界面
    /// </summary>
    public partial class 报检入库单 : Form
    {
        /// <summary>
        /// BOM表信息服务组件
        /// </summary>
        IBomServer m_serverBom = ServerModuleFactory.GetServerModule<IBomServer>();

        /// <summary>
        /// 挑返单服务
        /// </summary>
        ICheckReturnRepair m_checkReturnRepair = ServerModuleFactory.GetServerModule<ICheckReturnRepair>();

        /// <summary>
        /// 条形码
        /// </summary>
        IBarCodeServer m_barCodeServer = ServerModuleFactory.GetServerModule<IBarCodeServer>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 查找到的符合条件的库存信息
        /// </summary>
        //IQueryResult m_findBill;

        /// <summary>
        /// 报检入库单服务组件
        /// </summary>
        ICheckOutInDepotServer m_billServer = ServerModuleFactory.GetServerModule<ICheckOutInDepotServer>();

        /// <summary>
        /// 获取计划价格服务组件
        /// </summary>
        IBasicGoodsServer m_planCostBillServer = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();

        /// <summary>
        /// 库存服务
        /// </summary>
        IStoreServer m_storeServer = ServerModuleFactory.GetServerModule<IStoreServer>();

        /// <summary>
        /// 获取单据流消息操作接口
        /// </summary>
        IBillFlowMessage m_billFlowMsg = PlatformFactory.GetObject<IBillFlowMessage>();

        /// <summary>
        /// 液力变矩器信息服务（将厂家提供的数据导入数据库中）
        /// </summary>
        ITorqueConverterInfoServer m_torqueConverterServer = ServerModuleFactory.GetServerModule<ITorqueConverterInfoServer>();

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
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 报检物品类别ID
        /// </summary>
        /// <remarks>1:采购件, 2:回收件, 3:返修件</remarks>
        int m_checkOutGoodsType = 1;

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_msgPromulgator = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_msgPromulgatorTF = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nodeInfo">功能树节点信息</param>
        public 报检入库单(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_msgPromulgator.BillType = CE_BillTypeEnum.报检入库单.ToString();
            m_msgPromulgatorTF.BillType = CE_BillTypeEnum.挑选返工返修单.ToString();

            m_authFlag = nodeInfo.Authority;

            string[] strBillStatus = { "全部", 
                                         CheckInDepotBillStatus.新建单据.ToString(), 
                                         CheckInDepotBillStatus.等待确认到货数.ToString(), 
                                         CheckInDepotBillStatus.等待质检机检验.ToString(), 
                                         CheckInDepotBillStatus.等待质检电检验.ToString(), 
                                         CheckInDepotBillStatus.等待挑返.ToString(), 
                                         CheckInDepotBillStatus.等待入库.ToString(), 
                                         CheckInDepotBillStatus.回退_采购单据有误.ToString(), 
                                         CheckInDepotBillStatus.回退_确认到货有误.ToString(), 
                                         CheckInDepotBillStatus.回退_质检电信息有误.ToString(), 
                                         CheckInDepotBillStatus.回退_质检机信息有误.ToString(), 
                                         CheckInDepotBillStatus.已入库.ToString(),
                                         CheckInDepotBillStatus.已报废.ToString()};

            checkBillDateAndStatus1.InsertComBox(strBillStatus);

            checkBillDateAndStatus1.OnCompleteSearch +=
                new GlobalObject.DelegateCollection.NonArgumentHandle(checkBillDateAndStatus1_OnCompleteSearch);

            checkBillDateAndStatus1.dtpStartTime.Value = ServerTime.Time.AddDays(1).AddMonths(-1);
            checkBillDateAndStatus1.dtpEndTime.Value = ServerTime.Time.AddDays(1);

            // 添加计量单位
            cmbUnit.Items.AddRange(StapleInfo.Units);

            if (cmbUnit.Items.Count > 0)
            {
                cmbUnit.SelectedIndex = 0;
            }

            RefreshData();

            DataTable dt = UniversalFunction.GetStorageTb();

            dt = DataSetHelper.SiftDataTable(dt, "ZeroCostFlag = 0", out m_err);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cmbStorage.Items.Add(dt.Rows[i]["StorageName"].ToString());
            }

            cmbStorage.SelectedIndex = -1;
        }

        private void 报检入库单_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authFlag);

            if (BasicInfo.ListRoles.Contains(CE_RoleEnum.采购账务管理员.ToString())
                || BasicInfo.ListRoles.Contains(CE_RoleEnum.会计.ToString()))
            {
                label23.Visible = true;
                label24.Visible = true;
                numUnitPrice.Visible = true;
                numPrice.Visible = true;
            }
            else
            {
                label23.Visible = false;
                label24.Visible = false;
                numUnitPrice.Visible = false;
                numPrice.Visible = false;
            }
        }

        void checkBillDateAndStatus1_OnCompleteSearch()
        {
            btnRefresh_Click(null, null);
        }

        private void 报检入库单_Resize(object sender, EventArgs e)
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
            FaceAuthoritySetting.SetEnable(this.Controls, authorityFlag);

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

                    DataTable dtMessage = UniversalFunction.PositioningOneRecord(msg.MessageContent, "报检入库单");

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
        /// 查找并刷新数据
        /// </summary>
        private void RefreshData()
        {
            m_billServer.QueryResultFilter = QueryFilterControl.GetFilterString(labelTitle.Text)
                + checkBillDateAndStatus1.GetSqlString("报检日期", "单据状态");

            //IQueryResult result;

            //if (!m_billServer.GetAllBill(out result, out m_err))
            //{
            //    MessageDialog.ShowErrorMessage(m_err);
            //    return;
            //}

            RefreshDataGridView(m_billServer.GetAllBill());
        }

        /// <summary>
        /// 清除窗体上的控件残留信息
        /// </summary>
        void ClearForm()
        {
            cmbStorage.SelectedIndex = -1;
            chkPrint.Checked = false;
            chk紧急放行.Enabled = false;
            chk_TF.Enabled = false;
            lbl质检完成时间.Text = "";
            lbl仓库入库时间.Text = "";
            cmbUnit.Enabled = false;

            numUnitPrice.Enabled = false;
            numPrice.Enabled = false;

            lblBillStatus.Text = "";
            cbVersion.Text = "";
            cbVersion.DataSource = null;
            txtOrderFormNumber.Text = "";
            txtBill_ID.Text = "";
            dateTime_ArriveTime.Value = ServerModule.ServerTime.Time;      // 到货日期
            numDeclareCount.Value = 0;                     // 报检数
            txtProvider.Text = "";                         // 供应商编码
            txtProviderBatchNo.Text = "";                  // 供应商批次
            txtCode.Text = "";
            txtName.Text = "";
            txtSpec.Text = "";
            cmbUnit.Text = "";                              // 计量单位
            txtMaterialType.Text = "";                      // 仓库名就是材料类别
            txtRemark.Text = "";
            txtChecker.Text = "";
            txtBatchNo.Text = "";                           // xsy

            numUnitPrice.Value = 0;
            numPlanUnitPrice.Value = 0;
            numPrice.Value = 0;
            txtTotalPrice.Text = "";

            numAffirmCount.Value = 0;
            numInDepotCount.Value = 0;
            txtShelf.Text = "";
            txtColumn.Text = "";
            txtLayer.Text = "";

            dateTime_CheckTime.Value = dateTime_ArriveTime.Value;
            txtQualityInfo.Text = "";
            txtCheckoutReportID.Text = "";
            numEligibleCount.Value = 0;
            numDeclareWastrelCount.Value = 0;
            numConcessionCount.Value = 0;
            numReimbursementCount.Value = 0;
        }

        private void 新建单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                if (BasicInfo.LoginID == (string)dataGridView1.CurrentRow.Cells["报检人编码"].Value)
                {
                    if (lblBillStatus.Text == "新建单据")
                    {
                        MessageDialog.ShowPromptMessage("现在已经处于新建单据状态");
                        return;
                    }
                }
            }

            ClearForm();

            m_checkOutGoodsType = 1;
            lblCheckOutGoodsType.Text = "采购件";
            txtOrderFormNumber.Enabled = true;
            btnFindOrderForm.Enabled = true;
            txtQualityManager.Text = "";
            txtBill_ID.Text = "系统自动生成";
            txtBatchNo.Text = "系统自动生成";
            lblBillStatus.Text = "新建单据";

            if (BasicInfo.LoginID == "0033")
            {
                numUnitPrice.Enabled = true;
                numPrice.Enabled = true;
            }
        }

        private void 新建回收物品单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                if (BasicInfo.LoginID == (string)dataGridView1.CurrentRow.Cells["报检人编码"].Value)
                {
                    if (lblBillStatus.Text == "新建单据")
                    {
                        MessageDialog.ShowPromptMessage("现在已经处于新建单据状态");
                        return;
                    }
                }
            }

            ClearForm();

            m_checkOutGoodsType = 2;
            lblCheckOutGoodsType.Text = "回收件";
            txtOrderFormNumber.Enabled = false;
            txtProvider.Text = "SYS_SHHS";
            btnFindOrderForm.Enabled = false;
            txtBatchNo.ReadOnly = true;

            txtBill_ID.Text = "系统自动生成";
            txtBatchNo.Text = "系统自动生成";
            lblBillStatus.Text = "新建单据";
        }

        private void 新建返修物品单据MenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                if (BasicInfo.LoginID == (string)dataGridView1.CurrentRow.Cells["报检人编码"].Value)
                {
                    if (lblBillStatus.Text == "新建单据")
                    {
                        MessageDialog.ShowPromptMessage("现在已经处于新建单据状态");
                        return;
                    }
                }
            }

            ClearForm();

            m_checkOutGoodsType = 3;
            lblCheckOutGoodsType.Text = "返修件";
            txtOrderFormNumber.Enabled = false;
            txtProvider.Text = "SYS_SHFX";
            btnFindOrderForm.Enabled = false;
            txtBatchNo.ReadOnly = true;

            txtBill_ID.Text = "系统自动生成";
            txtBatchNo.Text = "系统自动生成";
            lblBillStatus.Text = "新建单据";
        }

        #region 数据检测

        /// <summary>
        /// 判断单据类型
        /// </summary>
        void GetOutGoodsTypes()
        {
            string strType = txtBill_ID.Text.Substring(0, 3);

            switch (strType)
            {
                case "BJD":
                    m_checkOutGoodsType = 1;
                    break;
                case "HJD":
                    m_checkOutGoodsType = 2;
                    break;
                case "FJD":
                    m_checkOutGoodsType = 3;
                    break;
                default:
                    break;
            }
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
        /// 检测有关采购数据项内容是否正确
        /// </summary>
        /// <returns>返回检测结果</returns>
        bool CheckDataItem()
        {
            if (m_checkOutGoodsType == 1)
            {
                if (txtOrderFormNumber.Text.Length == 0)
                {
                    txtOrderFormNumber.Focus();
                    MessageDialog.ShowPromptMessage(@"订单号不允许为空");
                    return false;
                }
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

            if (dateTime_ArriveTime.Value > ServerTime.Time)
            {
                dateTime_ArriveTime.Focus();
                MessageDialog.ShowPromptMessage("到货日期有误!");
                return false;
            }

            if (txtProvider.Text == "")
            {
                txtProvider.Focus();
                MessageDialog.ShowPromptMessage("供货单位不允许为空!");
                return false;
            }

            if (txtBatchNo.Text == "" || txtBatchNo.Text == "输入OA中的批次号")
            {
                txtBatchNo.Focus();
                MessageDialog.ShowPromptMessage("试运行期间请输入OA中的批次号");
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

            if (txtProviderBatchNo.Text.Trim().Length == 0)
            {
                txtProviderBatchNo.Focus();
                MessageDialog.ShowPromptMessage("供方批次号不允许为空!");
                return false;
            }

            // 当物品为赠送的时候，单价为0被允许，Modify by cjb on 2012.11.21
            //if (numUnitPrice.Value == 0)
            //{
            //    MessageDialog.ShowPromptMessage("单价不可为0,请重新确认单价");
            //    return false;
            //}

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

            DataTable dataSource = GetDataSource(findBill);

            dataGridView1.DataSource = dataSource;

            if (BasicInfo.ListRoles.Contains(CE_RoleEnum.采购账务管理员.ToString())
                || BasicInfo.ListRoles.Contains(CE_RoleEnum.会计.ToString()))
            {
                dataGridView1.Columns["单价"].Visible = true;
                dataGridView1.Columns["金额"].Visible = true;
                dataGridView1.Columns["大写金额"].Visible = true;
                dataGridView1.Columns["计划单价"].Visible = true;
                dataGridView1.Columns["计划金额"].Visible = true;
            }
            else
            {
                dataGridView1.Columns["单价"].Visible = false;
                dataGridView1.Columns["金额"].Visible = false;
                dataGridView1.Columns["大写金额"].Visible = false;
                dataGridView1.Columns["计划单价"].Visible = false;
                dataGridView1.Columns["计划金额"].Visible = false;
            }

            if (dataSource == null)
            {
                return;
            }

            this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            dataGridView1.Columns["物品ID"].Visible = false;
            dataGridView1.Columns["单位ID"].Visible = false;

            #region 隐藏不允许查看的列

            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                if (findBill.HideFields.Contains(dataGridView1.Columns[i].Name))
                {
                    dataGridView1.Columns[i].Visible = false;
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
                filterExpression = "单据状态 = '等待确认到货数'";
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

        private void 提交单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetOutGoodsTypes();

            if (!CheckDataItem())
            {
                return;
            }

            if (lblBillStatus.Text != "新建单据")
            {
                MessageDialog.ShowPromptMessage("您现在不是处于新建单据状态，无法提交，如果是回退后重新提交请选择“修改单据”功能");
                return;
            }

            if (m_billServer.GetBill(txtBill_ID.Text) != null)//|| m_billServer.GetBill(txtBill_ID.Text).单据状态 != "新建单据")
            {
                MessageDialog.ShowPromptMessage("您现在不是处于新建单据状态，无法提交，如果是回退后重新提交请选择“修改单据”功能");
                return;
            }

            if (dataGridView1.SelectedRows.Count > 0 && txtBill_ID.Text != "系统自动生成")
            {
                if (!CheckUserOperation(dataGridView1.CurrentRow))
                {
                    return;
                }
            }

            S_CheckOutInDepotBill bill = new S_CheckOutInDepotBill();

            bill.Bill_ID = txtBill_ID.Text;
            bill.OrderFormNumber = txtOrderFormNumber.Text;
            bill.ArriveGoods_Time = dateTime_ArriveTime.Value;      // 到货日期
            bill.Bill_Time = ServerTime.Time;                       // 报检日期
            bill.BillStatus = CheckInDepotBillStatus.等待确认到货数.ToString();
            bill.Buyer = BasicInfo.LoginName;                       // 采购员签名
            bill.DeclarePersonnelCode = BasicInfo.LoginID;          // 报检员编码
            bill.DeclarePersonnel = BasicInfo.LoginName;            // 报检员签名
            bill.DeclareCount = Convert.ToInt32(numDeclareCount.Value);              // 报检数
            bill.Provider = txtProvider.Text;                       // 供应商编码
            bill.ProviderBatchNo = txtProviderBatchNo.Text;       // 供应商批次
            bill.GoodsID = (int)txtCode.Tag;
            bill.BatchNo = txtBatchNo.Text;                         // xsy, 没有废除OA前暂用
            bill.Remark = txtRemark.Text;
            bill.CheckOutGoodsType = m_checkOutGoodsType;
            bill.OnlyForRepairFlag = chkOnlyForRepair.Checked;
            bill.UnitPrice = numUnitPrice.Value;
            bill.Price = decimal.Round(bill.UnitPrice * numDeclareCount.Value, 2);
            bill.PlanUnitPrice = numPlanUnitPrice.Value;
            bill.PlanPrice = decimal.Round(bill.PlanUnitPrice * numDeclareCount.Value, 2);
            bill.TotalPrice = CalculateClass.GetTotalPrice(bill.Price);
            bill.StorageID = UniversalFunction.GetStorageID(cmbStorage.Text);
            bill.Version = cbVersion.Text.Trim();
            bill.IsExigenceCheck = chkIsExigenceCheck.Checked;
            bill.UnitInvoicePrice = 0;
            bill.InvoicePrice = 0;

            //取消此功能 modify by cjb on 2013.1.14
            //if (txtName.Text == "液力变矩器总成")
            //{
            //    导入液力变矩器厂家数据 form = new 导入液力变矩器厂家数据();

            //    if (form.ShowDialog() != DialogResult.OK || !form.Complete)
            //    {
            //        MessageDialog.ShowPromptMessage("没有导入液力变矩器信息，不允许提交操作！");
            //        return;
            //    }

            //    string billNo = m_billServer.GetNextBillNo(m_checkOutGoodsType);

            //    if (!m_torqueConverterServer.ImportInfo(form.Data, billNo, billNo, out m_err))
            //    {
            //        MessageDialog.ShowErrorMessage(m_err);
            //        return;
            //    }
            //}

            try
            {
                m_err = null;

                if (!m_billServer.AddBill(bill, out m_err))
                {
                    MessageDialog.ShowErrorMessage(m_err);
                    return;
                }

                MessageDialog.ShowPromptMessage("成功提交,等待仓管确认到货数!");
            }
            finally
            {
                if (m_err != null)
                {
                    if (!m_torqueConverterServer.DeleteInfo(bill.Bill_ID, out m_err))
                    {
                        MessageDialog.ShowErrorMessage(m_err);
                    }
                }
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("物品【{0}】，批次号【{1}】  ※※※ 等待【仓库管理人员】处理", txtName.Text, bill.BatchNo);
            m_msgPromulgator.DestroyMessage(bill.Bill_ID);
            m_msgPromulgator.SendNewFlowMessage(bill.Bill_ID, sb.ToString(), CE_RoleEnum.制造仓库收货员);

            RefreshDataGridView(m_billServer.GetAllBill());
            PositioningRecord(bill.Bill_ID);
        }

        /// <summary>
        /// 查询订单信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFindOrderForm_Click(object sender, EventArgs e)
        {
            if (lblBillStatus.Text != "新建单据" && lblBillStatus.Text != "回退_采购单据有误")
            {
                return;
            }

            FormQueryInfo form = QueryInfoDialog.GetOrderFormInfoDialog(CE_BillTypeEnum.报检入库单);

            if (DialogResult.OK == form.ShowDialog())
            {
                txtCode.Text = "";
                txtName.Text = "";
                txtSpec.Text = "";

                txtOrderFormNumber.Text = form.GetDataItem("订单号").ToString();
                txtProvider.Text = form.GetDataItem("供货单位").ToString();
            }

            IBargainInfoServer bargainServer = ServerModuleFactory.GetServerModule<IBargainInfoServer>();

            // 判断订单对应的合同是否为委外合同 或 委外合同
            // 甘习枚 要求 只有海外合同才可录入单价 Modify by cjb on 2013.5.6
            if (bargainServer.IsOverseasBargain(txtOrderFormNumber.Text))
            {
                numUnitPrice.ReadOnly = false;
                numUnitPrice.Enabled = true;
            }
            else
            {
                numUnitPrice.ReadOnly = true;
                numUnitPrice.Enabled = false;
            }
        }

        /// <summary>
        /// 查询图号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFindCode_Click(object sender, EventArgs e)
        {
            if (lblBillStatus.Text != "新建单据" && lblBillStatus.Text != "回退_采购单据有误")
            {
                return;
            }

            if (m_checkOutGoodsType != 1)
            {
                FindReclaimedGoods();
            }
            else
            {
                FindNormalGoods();
            }

            if (txtSpec.Text.Contains("返修") || txtSpec.Text.Contains("委外"))
            {
                numUnitPrice.ReadOnly = false;
                numUnitPrice.Enabled = true;
            }
        }

        /// <summary>
        /// 查找回收、返修物品信息
        /// </summary>
        private void FindReclaimedGoods()
        {
            FormQueryInfo dialog = QueryInfoDialog.GetPlanCostGoodsDialog(true);

            if (dialog != null && dialog.ShowDialog() == DialogResult.OK)
            {
                txtCode.Tag = (int)dialog.GetDataItem("序号");
                txtCode.Text = dialog.GetDataItem("图号型号").ToString();
                txtName.Text = dialog.GetDataItem("物品名称").ToString();
                txtSpec.Text = dialog.GetDataItem("规格").ToString();

                if (txtCode.Text == "")
                {
                    txtName.Text = "";
                    txtSpec.Text = "";

                    MessageDialog.ShowPromptMessage("请选择具有图号的物品");
                    return;
                }

                DataRow dr = m_serverBom.GetBomInfo(txtCode.Text.Trim(), txtName.Text.Trim());

                if (dr == null)
                {
                    cbVersion.Text = "";
                }
                else
                {
                    cbVersion.Text = dr["Version"].ToString();
                }
            }
            else
            {
                return;
            }

            //View_F_GoodsPlanCost basicGoodsInfo = m_planCostBillServer.GetGoodsInfoView((int)txtCode.Tag);

            View_S_Stock lnqViewStock = m_storeServer.GetGoodsStore((int)txtCode.Tag);

            if (m_checkOutGoodsType == 2)
            {
                txtMaterialType.Text = "ZZHS";
            }
            else if (m_checkOutGoodsType == 3)
            {
                txtMaterialType.Text = "ZZFX";
            }

            if (lblCheckOutGoodsType.Text == "采购件")
            {
                numUnitPrice.Value = lnqViewStock.实际单价;
                numPrice.Value = decimal.Round(numDeclareCount.Value * numUnitPrice.Value, 2);
            }

            cmbUnit.Text = lnqViewStock.单位;
        }

        /// <summary>
        /// 查找普通报检入库单物品(采购用)
        /// </summary>
        private void FindNormalGoods()
        {
            if (txtOrderFormNumber.Text.Length == 0)
            {
                txtOrderFormNumber.Focus();
                MessageDialog.ShowPromptMessage(@"请先选择订单/合同号后再进行此操作！");
                return;
            }

            if (numDeclareCount.Value == 0)
            {
                numDeclareCount.Focus();
                MessageDialog.ShowPromptMessage("请先录入报检数量后再进行此操作！");
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

            B_AccessoryDutyInfo lnqAcc = new B_AccessoryDutyInfo();

            lnqAcc.GoodsID = Convert.ToInt32(txtCode.Tag);
            lnqAcc.ProviderA = txtProvider.Text;

            if (!ServerModuleFactory.GetServerModule<IAccessoryDutyInfoManageServer>().IsSafeProvider(lnqAcc, out m_err))
            {
                MessageDialog.ShowPromptMessage("此物品与供应商未通过合格供应商审核");
                txtCode.Text = "";
                txtName.Text = "";
                txtSpec.Text = "";
                return;
            }

            try
            {
                #region 获取单价

                IOrderFormGoodsServer orderFormGoodsServer = ServerModuleFactory.GetServerModule<IOrderFormGoodsServer>();

                numUnitPrice.Value = orderFormGoodsServer.GetGoodsUnitPrice(txtOrderFormNumber.Text, (int)txtCode.Tag);
                numPrice.Value = decimal.Round(numUnitPrice.Value * numDeclareCount.Value, 2);
                txtTotalPrice.Text = CalculateClass.GetTotalPrice(numPrice.Value);

                #endregion

                #region 获取计划价格

                View_F_GoodsPlanCost basicGoodsInfo = m_planCostBillServer.GetGoodsInfo(txtCode.Text, txtName.Text, txtSpec.Text, out m_err);

                if (!GlobalObject.GeneralFunction.IsNullOrEmpty(m_err))
                {
                    MessageDialog.ShowErrorMessage(m_err);
                    return;
                }

                txtCode.Tag = (int)basicGoodsInfo.序号;
                numPlanUnitPrice.Value = basicGoodsInfo.单价;
                txtMaterialType.Text = basicGoodsInfo.物品类别;

                #endregion
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
                txtCode.Text = "";
                txtName.Text = "";
                txtSpec.Text = "";
                txtCode.Tag = null;
                return;
            }

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
                }
                else
                {
                    cbVersion.DataSource = null;
                }
            }
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            ClearForm();

            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

            chk紧急放行.Checked = (bool)row.Cells["是否紧急放行"].Value;
            chkOnlyForRepair.Checked = (bool)row.Cells["是否仅限于返修箱用"].Value;
            txtBill_ID.Text = (string)row.Cells["入库单号"].Value;
            txtOrderFormNumber.Text = (string)row.Cells["订单号"].Value;
            dateTime_BillTime.Value = (DateTime)row.Cells["报检日期"].Value;
            txtProvider.Text = (string)row.Cells["供货单位"].Value;
            dateTime_ArriveTime.Value = (DateTime)row.Cells["到货日期"].Value;
            txtBatchNo.Text = (string)row.Cells["批次号"].Value;
            chk_TF.Checked = (bool)row.Cells["是否挑返"].Value;
            chkIsExigenceCheck.Checked = (bool)row.Cells["是否紧急报检"].Value;

            if (row.Cells["供方批次号"].Value != null)
                txtProviderBatchNo.Text = row.Cells["供方批次号"].Value.ToString();
            else
                txtProviderBatchNo.Text = "";

            txtName.Text = (string)row.Cells["物品名称"].Value;
            txtCode.Text = (string)row.Cells["图号型号"].Value;
            txtCode.Tag = (int)row.Cells["物品ID"].Value;
            txtSpec.Text = (string)row.Cells["规格"].Value;
            cmbUnit.Text = (string)row.Cells["单位"].Value;
            cmbStorage.Text = UniversalFunction.GetStorageName(row.Cells["库房代码"].Value.ToString());

            if (row.Cells["检验报告编号"].Value != System.DBNull.Value)
                txtCheckoutReportID.Text = (string)row.Cells["检验报告编号"].Value;
            else
                txtCheckoutReportID.Text = "";

            if (row.Cells["检验日期"].Value != System.DBNull.Value)
            {
                dateTime_CheckTime.Value = (DateTime)row.Cells["检验日期"].Value;
            }
            else
            {
                dateTime_CheckTime.Value = ServerTime.Time;
            }

            numDeclareCount.Value = (int)row.Cells["报检数量"].Value;
            numEligibleCount.Value = (int)row.Cells["合格数量"].Value;
            numDeclareWastrelCount.Value = (int)row.Cells["报废数量"].Value;
            numConcessionCount.Value = (int)row.Cells["让步数量"].Value;
            numReimbursementCount.Value = (int)row.Cells["退货数量"].Value;

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

            if (dataGridView1.Columns["大写金额"].Visible)
            {
                txtTotalPrice.Text = (string)row.Cells["大写金额"].Value;
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

            if (row.Cells["仓库确认数量"].Value != System.DBNull.Value)
                numAffirmCount.Value = (int)row.Cells["仓库确认数量"].Value;
            else
                numAffirmCount.Value = 0;

            if ((int)row.Cells["入库数量"].Value != 0)
                numInDepotCount.Value = (int)row.Cells["入库数量"].Value;
            else
            {
                numInDepotCount.Value = numEligibleCount.Value + numConcessionCount.Value + numDeclareWastrelCount.Value;
            }

            lblBillStatus.Text = (string)row.Cells["单据状态"].Value;

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

            if (row.Cells["检验日期"].Value != System.DBNull.Value)
            {
                lbl质检完成时间.Text = row.Cells["检验日期"].Value.ToString();
            }

            if (row.Cells["入库时间"].Value != System.DBNull.Value)
            {
                lbl仓库入库时间.Text = (string)row.Cells["入库时间"].Value.ToString();
            }

            if (row.Cells["是否已打印"].Value != System.DBNull.Value)
            {
                chkPrint.Checked = (bool)row.Cells["是否已打印"].Value;
            }

            chk_TF.Checked = (bool)row.Cells["是否挑返"].Value;
            lblCheckOutGoodsType.Text = (string)row.Cells["物品类别"].Value;
            cbVersion.Text = (string)row.Cells["版次号"].Value;

            if ((m_authFlag & AuthorityFlag.Auditing) != AuthorityFlag.Nothing &&
                (lblBillStatus.Text == CheckInDepotBillStatus.等待质检电检验.ToString() ||
                lblBillStatus.Text == CheckInDepotBillStatus.等待质检机检验.ToString()
                || lblBillStatus.Text == CheckInDepotBillStatus.回退_质检电信息有误.ToString()
                || lblBillStatus.Text == CheckInDepotBillStatus.回退_质检机信息有误.ToString()))
            {
                chk紧急放行.Enabled = true;
                chk_TF.Enabled = true;
            }
            else
            {
                chk紧急放行.Enabled = false;
                chk_TF.Enabled = false;
            }

            仓库管理员操作ToolStripMenuItem.Visible
                = UniversalFunction.CheckStorageAndPersonnel((string)row.Cells["库房代码"].Value);

            IBargainInfoServer bargainServer = ServerModuleFactory.GetServerModule<IBargainInfoServer>();

            // 判断订单对应的合同是否为委外合同 或 委外合同
            if (bargainServer.IsOverseasBargain(txtOrderFormNumber.Text))
            {
                numUnitPrice.ReadOnly = false;
                numUnitPrice.Enabled = true;
            }
            else
            {
                numUnitPrice.ReadOnly = true;
                numUnitPrice.Enabled = false;
            }

            if (txtSpec.Text.Contains("返修") || txtSpec.Text.Contains("委外"))
            {
                numUnitPrice.ReadOnly = false;
                numUnitPrice.Enabled = true;
            }

        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            //if (e.RowIndex < 0 || e.ColumnIndex < 0)
            //{
            //    return;
            //}

            //string strColName = "";

            //foreach (DataGridViewColumn col in dataGridView1.Columns)
            //{
            //    if (col.Visible)
            //    {
            //        strColName = col.Name;
            //        break;
            //    }
            //}

            //dataGridView1.ClearSelection();
            //dataGridView1.CurrentCell = dataGridView1.Rows[e.RowIndex].Cells[strColName];
        }

        private void 确认到货数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
            {
                return;
            }

            CheckInDepotBillStatus status = (CheckInDepotBillStatus)Enum.Parse(typeof(CheckInDepotBillStatus),
                dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString());

            if ((status == CheckInDepotBillStatus.等待确认到货数 || status == CheckInDepotBillStatus.回退_确认到货有误))
            {
                if (numAffirmCount.Value == 0m || numAffirmCount.Value > numDeclareCount.Value)
                {
                    numAffirmCount.Focus();
                    MessageDialog.ShowPromptMessage("确认到货数必须 > 0 且 <= 报检数");
                    return;
                }

                string billNo = txtBill_ID.Text;

                CheckInDepotBillStatus statusSetting = CheckInDepotBillStatus.等待质检机检验;

                if (!m_billServer.AffirmGoodsAmount(billNo, BasicInfo.LoginName, Convert.ToInt32(numAffirmCount.Value),
                    statusSetting.ToString(), out m_err))
                {
                    MessageDialog.ShowErrorMessage(m_err);
                    return;
                }

                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("物品【{0}】，批次号【{1}】  ※※※ 等待【检验员】处理", txtName.Text, txtBatchNo.Text);
                m_msgPromulgator.PassFlowMessage(billNo, sb.ToString(), CE_RoleEnum.机械检验组长);

                MessageDialog.ShowPromptMessage("成功提交,等待零件质检!");

                RefreshDataGridView(m_billServer.GetAllBill());
                PositioningRecord(billNo);
            }
            else
            {
                MessageDialog.ShowPromptMessage("请选择等待确认到货的记录后再进行此操作！");
            }
        }

        private void 提交质检信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
            {
                return;
            }

            CheckInDepotBillStatus status = (CheckInDepotBillStatus)Enum.Parse(typeof(CheckInDepotBillStatus),
                dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString());

            if ((status != CheckInDepotBillStatus.等待质检机检验
                && status != CheckInDepotBillStatus.等待质检电检验
                && status != CheckInDepotBillStatus.回退_质检机信息有误
                && status != CheckInDepotBillStatus.回退_质检电信息有误))
            {
                MessageDialog.ShowPromptMessage("请选择等待质检的记录后再进行此操作");
                return;
            }

            dateTime_CheckTime.Value = ServerTime.Time;

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

            if (numEligibleCount.Value + numConcessionCount.Value + numReimbursementCount.Value +
                numDeclareWastrelCount.Value != numAffirmCount.Value)
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

            if (numConcessionCount.Value > 0 || numReimbursementCount.Value > 0 || chk_TF.Checked == true)
            {
                不合格品信息 form = new 不合格品信息(txtBill_ID.Text);

                form.ShowDialog();

                if (!form.BlFlag)
                {
                    MessageBox.Show("请完整填写不合格信息单，并且保存！", "提示");
                    return;
                }
            }

            S_CheckOutInDepotBill qualityInfo = new S_CheckOutInDepotBill();

            if (txtChecker.Text == "")
            {
                qualityInfo.Checker = txtChecker.Text;
                qualityInfo.CheckoutReport_ID = txtCheckoutReportID.Text;
                qualityInfo.CheckoutJoinGoods_Time = dateTime_CheckTime.Value;
            }
            else
            {
                qualityInfo.PeremptorilyEmit = chk紧急放行.Checked;
                qualityInfo.QualityInputer = BasicInfo.LoginName;
                qualityInfo.QualityInfo = txtQualityInfo.Text;
                qualityInfo.EligibleCount = Convert.ToInt32(numEligibleCount.Value);
                qualityInfo.DeclareWastrelCount = Convert.ToInt32(numDeclareWastrelCount.Value);
                qualityInfo.ConcessionCount = Convert.ToInt32(numConcessionCount.Value);
                qualityInfo.ReimbursementCount = Convert.ToInt32(numReimbursementCount.Value);
                qualityInfo.TFFlag = chk_TF.Checked;
            }

            if (!chk_TF.Checked)
            {
                qualityInfo.BillStatus = CheckInDepotBillStatus.等待入库.ToString();
            }
            else
            {
                qualityInfo.BillStatus = CheckInDepotBillStatus.等待挑返.ToString();

                string strTFDJH = "";

                if (!m_checkReturnRepair.Create(txtBill_ID.Text, BasicInfo.LoginID, out m_err, out strTFDJH))
                {
                    MessageDialog.ShowErrorMessage(m_err);
                    return;
                }
                else
                {
                    m_msgPromulgator.DestroyMessage(strTFDJH);
                    m_msgPromulgatorTF.SendNewFlowMessage(strTFDJH,
                        string.Format("{0} 号挑选返工返修单，请STA处理", strTFDJH),
                        CE_RoleEnum.SQE组员);
                }
            }

            qualityInfo.Bill_ID = txtBill_ID.Text;

            if (!m_billServer.SubmitQualityInfo(qualityInfo, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            if (!chk_TF.Checked)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("物品【{0}】，批次号【{1}】  ※※※ 等待【仓库管理人员】处理", txtName.Text, txtBatchNo.Text);
                m_msgPromulgator.PassFlowMessage(qualityInfo.Bill_ID, sb.ToString(),
                        m_msgPromulgator.GetRoleStringForStorage(cmbStorage.Text).ToString(), true);
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("物品【{0}】，批次号【{1}】  ※※※ 等待【质检员】挑返处理", txtName.Text, txtBatchNo.Text);
                m_msgPromulgator.PassFlowMessage(qualityInfo.Bill_ID, sb.ToString(), CE_RoleEnum.机械检验组长.ToString(), true);
            }

            MessageDialog.ShowPromptMessage("成功提交,等待仓管将零件入库!");

            RefreshDataGridView(m_billServer.GetAllBill());
            PositioningRecord(qualityInfo.Bill_ID);
        }

        private void 零件入库ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
            {
                return;
            }

            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() != CheckInDepotBillStatus.等待入库.ToString())
            {
                MessageDialog.ShowPromptMessage("请选择等待入库的记录后再进行此操作！");
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

            S_CheckOutInDepotBill inDepotInfo = new S_CheckOutInDepotBill();

            inDepotInfo.DepotManager = BasicInfo.LoginName;
            inDepotInfo.InDepotCount = Convert.ToInt32(numInDepotCount.Value);
            inDepotInfo.ShelfArea = txtShelf.Text.Trim();
            inDepotInfo.ColumnNumber = txtColumn.Text.Trim();
            inDepotInfo.LayerNumber = txtLayer.Text.Trim();
            inDepotInfo.BillStatus = CheckInDepotBillStatus.已入库.ToString();

            string billNo = txtBill_ID.Text;

            if (!m_billServer.SubmitInDepotInfo(billNo, inDepotInfo, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            StringBuilder sb = new StringBuilder();

            if (numReimbursementCount.Value == 0)
            {
                sb.AppendFormat("{0} 号报检入库单已入库, ", billNo);
                sb.AppendFormat("此单据物品：图号【{0}】，名称【{1}】，规格【{2}】", txtCode.Text, txtName.Text, txtSpec.Text);

                MessageDialog.ShowPromptMessage("成功将零件入库!");
            }
            else if (inDepotInfo.InDepotCount == 0)
            {
                sb.AppendFormat("{0} 号报检入库单已处理完毕, {1} 件物品全部不合格, 需要采购退货。", billNo,
                                inDepotInfo.DepotManagerAffirmCount);
                sb.AppendLine();
                sb.AppendFormat("此单据物品：图号【{0}】，名称【{1}】，规格【{2}】", txtCode.Text, txtName.Text, txtSpec.Text);

                MessageDialog.ShowPromptMessage("将知会采购进行退货处理!");
            }
            else if (inDepotInfo.InDepotCount > 0)
            {
                sb.AppendFormat("{0} 号报检入库单已入库 {1} 件, 其中合格 {2} 件, 让步 {3} 件, 退货 {4} 件, 有不合格物品, 需要采购退货。",
                    billNo, inDepotInfo.InDepotCount, inDepotInfo.EligibleCount, inDepotInfo.ConcessionCount,
                    inDepotInfo.ReimbursementCount);

                sb.AppendLine();
                sb.AppendFormat("此单据物品：图号【{0}】，名称【{1}】，规格【{2}】", txtCode.Text, txtName.Text, txtSpec.Text);

                MessageDialog.ShowPromptMessage(string.Format("成功将入库 {0} 件物品, 退货部分将知会采购进行处理!",
                                                inDepotInfo.InDepotCount));
            }

            #region 发送知会消息

            List<string> noticeRoles = new List<string>();
            noticeRoles.Add(CE_RoleEnum.SQE组长.ToString());
            noticeRoles.Add(CE_RoleEnum.SQE组员.ToString());
            noticeRoles.Add(CE_RoleEnum.质量工程师.ToString());

            m_msgPromulgator.EndFlowMessage(billNo, sb.ToString(), noticeRoles, null);

            #endregion 发送知会消息

            //m_billServer.GetAllBill(out m_findBill, out m_err);
            RefreshDataGridView(m_billServer.GetAllBill());
            PositioningRecord(billNo);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            m_billServer.QueryResultFilter = QueryFilterControl.GetFilterString(labelTitle.Text)
                + checkBillDateAndStatus1.GetSqlString("报检日期", "单据状态");

            RefreshDataGridView(m_billServer.GetAllBill());
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
                MessageDialog.ShowPromptMessage("请选择已入库的记录后再打印条形码");
                return;
            }

            List<View_S_InDepotGoodsBarCodeTable> lstBarCodeInfo = new List<View_S_InDepotGoodsBarCodeTable>();

            for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
            {
                //if (dataGridView1.SelectedRows[i].Cells["单据状态"].Value.ToString() != CheckInDepotBillStatus.已入库.ToString())
                //{
                //    string msg = "第 " + dataGridView1.SelectedRows[i].Cells["入库单号"].Value.ToString() + " 号单据仓管员尚未入库,系统将不打印该项条形码!";
                //    MessageDialog.ShowPromptMessage(msg);
                //    continue;
                //}

                string goodsCode = dataGridView1.SelectedRows[i].Cells["图号型号"].Value.ToString();
                string goodsName = dataGridView1.SelectedRows[i].Cells["物品名称"].Value.ToString();
                string spec = dataGridView1.SelectedRows[i].Cells["规格"].Value.ToString();
                string provider = dataGridView1.SelectedRows[i].Cells["供货单位"].Value.ToString();
                string batchCode = dataGridView1.SelectedRows[i].Cells["批次号"].Value.ToString();
                string StorageID = dataGridView1.SelectedRows[i].Cells["库房代码"].Value.ToString();

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
                    //string msg = "数据库中不存在第 " + dataGridView1.SelectedRows[i].Cells["入库单号"].Value.ToString() + " 号单据的零件条形码!";
                    //MessageDialog.ShowPromptMessage(msg);
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
        private void 采购表单打印ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择一条已入库记录后再行此操作");
                return;
            }

            CheckInDepotBillStatus status = (CheckInDepotBillStatus)Enum.Parse(typeof(CheckInDepotBillStatus),
                dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString());

            if (status != CheckInDepotBillStatus.已入库)
            {
                MessageDialog.ShowPromptMessage("当前单据没有入库不允许进行打印");
                return;
            }

            //if (!CheckUserOperation(dataGridView1.CurrentRow))
            //{
            //    return;
            //}

            报表_报检入库单 report = new 报表_报检入库单(dataGridView1.CurrentRow.Cells[0].Value.ToString(), labelTitle.Text, "采购件报检入库单");
            report.ShowDialog();
            //PrintReportBill print = new PrintReportBill(21.8, 8.9, report);
            //print.DirectPrintReport();
        }

        /// <summary>
        /// 报表(不包含金额)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 仓管表单打印ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择至少一条已入库记录后再行此操作");
                return;
            }

            int index = 0;

            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                CheckInDepotBillStatus status = (CheckInDepotBillStatus)Enum.Parse(
                    typeof(CheckInDepotBillStatus), row.Cells["单据状态"].Value.ToString());

                if (status != CheckInDepotBillStatus.已入库)
                {
                    continue;
                }

                string reportTitle = "采购件报检入库单";

                if (row.Cells["物品类别"].Value.ToString() == "回收件")
                {
                    reportTitle = "回收件报检入库单";
                }
                else if (row.Cells["物品类别"].Value.ToString() == "返修件")
                {
                    reportTitle = "返修件报检入库单";
                }

                报表_报检入库单 report = new 报表_报检入库单(row.Cells["入库单号"].Value.ToString(), labelTitle.Text, reportTitle);
                PrintReportBill print = new PrintReportBill(21.8, 9.31, report);

                if (index++ > 0)
                {
                    print.ShowPrintDialog = false;
                }

                print.DirectPrintReport();
            }
        }

        private void 回退ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckInDepotBillStatus status = (CheckInDepotBillStatus)Enum.Parse(typeof(CheckInDepotBillStatus),
                dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString());

            if (dataGridView1.SelectedRows.Count == 0 || status == CheckInDepotBillStatus.已入库)
            {
                MessageDialog.ShowPromptMessage("请选择没有入库的记录后再进行此操作！");
                return;
            }
            else if (dataGridView1.SelectedRows.Count > 1)
            {
                MessageDialog.ShowPromptMessage("系统不支持多条记录同时回退,请逐条操作！");
                return;
            }

            string billID = dataGridView1.CurrentRow.Cells["入库单号"].Value.ToString();
            bool validInfo = false;

            if (status == CheckInDepotBillStatus.等待确认到货数 || status == CheckInDepotBillStatus.回退_确认到货有误)
            {
                if (!BasicInfo.ListRoles.Contains(CE_RoleEnum.制造仓库收货员.ToString()))
                {
                    MessageDialog.ShowPromptMessage("您无权回退单据的当前步骤");
                    return;
                }

                validInfo = true;
            }
            else if (status == CheckInDepotBillStatus.等待质检机检验 || status == CheckInDepotBillStatus.回退_质检机信息有误)
            {
                if (!BasicInfo.ListRoles.Contains(CE_RoleEnum.机械检验组长.ToString()))
                {
                    MessageDialog.ShowPromptMessage("您无权回退单据的当前步骤");
                    return;
                }

                validInfo = true;
            }
            else if (status == CheckInDepotBillStatus.等待质检电检验 || status == CheckInDepotBillStatus.回退_质检电信息有误)
            {
                if (!BasicInfo.ListRoles.Contains(CE_RoleEnum.电子检验组长.ToString()))
                {
                    MessageDialog.ShowPromptMessage("您无权回退单据的当前步骤");
                    return;
                }

                validInfo = true;
            }
            else if (status == CheckInDepotBillStatus.等待入库)
            {
                if (!BasicInfo.ListRoles.Contains(
                        m_msgPromulgator.GetRoleStringForStorage(cmbStorage.Text).ToString()))
                {
                    MessageDialog.ShowPromptMessage("您无权回退单据的当前步骤");
                    return;
                }

                validInfo = true;
            }
            else if (status == CheckInDepotBillStatus.回退_采购单据有误)
            {
                MessageDialog.ShowPromptMessage("当前记录已经回退到起始流程位置，不能再进行回退");
                return;
            }

            if (validInfo)
            {
                int intStatusFlag = 0;

                if (status == CheckInDepotBillStatus.等待入库)
                {
                    DialogResult dr = MessageBox.Show("点击“是”此单据回退质检（机），点击“否”回退质检（电）", "提示",
                        MessageBoxButtons.YesNoCancel);

                    if (dr.ToString() == "No")
                    {
                        intStatusFlag = 0;
                    }
                    else if (dr.ToString() == "Yes")
                    {
                        intStatusFlag = 1;
                    }
                    else
                    {
                        return;
                    }

                    if (!m_checkReturnRepair.ScrapAllBill(billID, out m_err))
                    {
                        MessageDialog.ShowErrorMessage(m_err);
                        return;
                    }
                }

                string reason = InputBox.ShowDialog("请输入回退原因", "原因：", "");

                if (reason == null || reason == "")
                {
                    MessageDialog.ShowPromptMessage("请输入回退原因");
                    return;
                }

                if (!m_billServer.RebackBill(billID, reason, intStatusFlag, out m_err))
                {
                    MessageDialog.ShowErrorMessage(m_err);
                    return;
                }

                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("物品【{0}】，批次号【{1}】  ※※※ 已回退", txtName.Text, txtBatchNo.Text);
                m_msgPromulgator.RebackFlowMessage(billID, sb.ToString());

                RefreshDataGridView(m_billServer.GetAllBill());
                MessageDialog.ShowPromptMessage("成功回退！");
            }
        }

        private void 修改单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetOutGoodsTypes();

            CheckInDepotBillStatus status = (CheckInDepotBillStatus)Enum.Parse(typeof(CheckInDepotBillStatus),
                dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString());

            if (status != CheckInDepotBillStatus.回退_采购单据有误)
            {
                MessageDialog.ShowPromptMessage("当前状态不允许修改单据");
                return;
            }

            if (!CheckDataItem())
            {
                return;
            }

            if (!CheckUserOperation(dataGridView1.CurrentRow))
            {
                return;
            }

            S_CheckOutInDepotBill bill = new S_CheckOutInDepotBill();

            bill.Bill_ID = txtBill_ID.Text;
            bill.OrderFormNumber = txtOrderFormNumber.Text;
            bill.ArriveGoods_Time = dateTime_ArriveTime.Value;      // 到货日期
            bill.Bill_Time = ServerModule.ServerTime.Time;                          // 报检日期
            bill.BillStatus = CheckInDepotBillStatus.等待确认到货数.ToString();
            bill.Buyer = BasicInfo.LoginName;                       // 采购员签名
            bill.DeclarePersonnelCode = BasicInfo.LoginID;          // 报检员编码
            bill.DeclarePersonnel = BasicInfo.LoginName;            // 报检员签名
            bill.DeclareCount = Convert.ToInt32(numDeclareCount.Value);              // 报检数
            bill.Provider = txtProvider.Text;                       // 供应商编码
            bill.ProviderBatchNo = txtProviderBatchNo.Text;       // 供应商批次
            bill.GoodsID = (int)txtCode.Tag;
            //bill.Depot = txtMaterialType.Text;                    // 仓库名就是材料类别, 现在自动从计划价格表中获取
            bill.Remark = txtRemark.Text;
            bill.BatchNo = txtBatchNo.Text;                       // 停止OA前暂用, xsy
            bill.UnitPrice = numUnitPrice.Value;
            bill.Price = decimal.Round(bill.UnitPrice * numDeclareCount.Value, 2);
            bill.PlanUnitPrice = numPlanUnitPrice.Value;
            bill.PlanPrice = decimal.Round(bill.PlanUnitPrice * numDeclareCount.Value, 2);
            bill.TotalPrice = CalculateClass.GetTotalPrice(bill.Price);
            bill.StorageID = UniversalFunction.GetStorageID(cmbStorage.Text);
            bill.Version = cbVersion.Text.Trim();
            bill.IsExigenceCheck = chkIsExigenceCheck.Checked;
            bill.OnlyForRepairFlag = chkOnlyForRepair.Checked;

            if (!m_billServer.UpdateBill(bill, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("物品【{0}】，批次号【{1}】  ※※※ 等待【仓库管理人员】挑返处理", txtName.Text, txtBatchNo.Text);
            m_msgPromulgator.PassFlowMessage(bill.Bill_ID, sb.ToString(), CE_RoleEnum.制造仓库收货员.ToString(), true);

            //m_billServer.GetAllBill(out m_findBill, out m_err);
            RefreshDataGridView(m_billServer.GetAllBill());
            PositioningRecord(bill.Bill_ID);

            MessageDialog.ShowPromptMessage("成功提交,等待仓管确认到货数!");
        }

        private void btnFindChecker_Click(object sender, EventArgs e)
        {
            FormPersonnel form = new FormPersonnel(txtChecker, BasicInfo.DeptCode, "姓名");
            form.ShowDialog();
        }

        private void 设置数据过滤器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormFilterCondition form = new FormFilterCondition(labelTitle.Text, m_findField, labelTitle.Text);
            form.ShowDialog();

            m_billServer.QueryResultFilter = QueryFilterControl.GetFilterString(labelTitle.Text)
                + checkBillDateAndStatus1.GetSqlString("报检日期", "单据状态");
            RefreshData();
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

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dataGridView1.ClearSelection();

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewRow row = dataGridView1.Rows[i];

                if (row.Cells["单据状态"].Value.ToString() == CheckInDepotBillStatus.已入库.ToString())
                {
                    row.DefaultCellStyle.ForeColor = Color.Green; //Color.FromArgb(255, 128, 128);
                }

                if (row.Cells["退货数量"].Value != System.DBNull.Value && row.Cells["退货数量"].Value != null)
                {
                    int value = (int)row.Cells["退货数量"].Value;

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

        private void 启动定时刷新_Click(object sender, EventArgs e)
        {
            启动定时刷新.Checked = !启动定时刷新.Checked;
            int second = Convert.ToInt32(txtRefreshTime.Text);

            if (second < 10)
            {
                txtRefreshTime.Text = "10";
                启动定时刷新.Checked = false;
                timerRefresh.Enabled = false;

                MessageDialog.ShowPromptMessage("刷新时间请输入 >= 10的数");
                return;
            }

            timerRefresh.Interval = 1000 * second;
            timerRefresh.Enabled = 启动定时刷新.Checked;
        }

        private void timerRefresh_Tick(object sender, EventArgs e)
        {
            btnRefresh_Click(sender, e);
        }

        private void txtRefreshTime_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar)))
            {
                Keys key = (Keys)e.KeyChar;

                if (e.KeyChar == '.')
                {
                    e.Handled = true;
                }
                else if (!(key == Keys.Back || key == Keys.Delete))
                {
                    e.Handled = true;
                }
            }
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

        private void 报废单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string billNo = txtBill_ID.Text;

            if (!CheckUserOperation(dataGridView1.CurrentRow))
            {
                return;
            }

            if (txtBill_ID.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请选择需要删除的记录");
                return;
            }

            if (UniversalFunction.GetBillStatus("S_CheckOutInDepotBill", "BillStatus", "Bill_ID", billNo) == "已入库")
            {
                MessageBox.Show("此单据已入库，不可删除", "提示");
                return;
            }
            else
            {

                if (MessageDialog.ShowEnquiryMessage("您是否要报废此单据") == DialogResult.No)
                {
                    return;
                }


                if (!m_billServer.ScrapBill(billNo, out m_err))
                {
                    MessageDialog.ShowErrorMessage(m_err);
                    return;
                }

                if (!m_checkReturnRepair.ScrapAllBill(billNo, out m_err))
                {
                    MessageDialog.ShowErrorMessage(m_err);
                    return;
                }

                if (!m_torqueConverterServer.DeleteInfo(billNo, out m_err))
                {
                    MessageDialog.ShowErrorMessage(m_err);
                }
            }

            m_msgPromulgator.DestroyMessage(billNo);

            //m_billServer.GetAllBill(out m_findBill, out m_err);
            RefreshDataGridView(m_billServer.GetAllBill());
            PositioningRecord(billNo.ToString());
        }

        private void sQE退货处理建议ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (numReimbursementCount.Value > 0 && lblBillStatus.Text == "已入库")
            {
                退货处理建议 form = new 退货处理建议(txtBill_ID.Text);
                form.ShowDialog();
            }
            else
            {
                MessageBox.Show("请重新确认单据状态以及退货数量", "提示");
            }
        }

        private void numUnitPrice_Leave(object sender, EventArgs e)
        {
            numPrice.Value = numUnitPrice.Value * numDeclareCount.Value;
        }

        private void 提交检验报告ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
            {
                return;
            }

            CheckInDepotBillStatus status = (CheckInDepotBillStatus)Enum.Parse(typeof(CheckInDepotBillStatus),
                dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString());

            if ((status != CheckInDepotBillStatus.等待质检机检验
                && status != CheckInDepotBillStatus.等待质检电检验
                && status != CheckInDepotBillStatus.回退_质检机信息有误
                && status != CheckInDepotBillStatus.回退_质检电信息有误))
            {
                MessageDialog.ShowPromptMessage("请选择等待质检的记录后再进行此操作");
                return;
            }

            if (txtCheckoutReportID.Text == "")
            {
                txtCheckoutReportID.Focus();
                MessageDialog.ShowPromptMessage("检验报告编号不能为空");
                return;
            }

            if (txtChecker.Text == "")
            {
                txtChecker.Focus();
                MessageDialog.ShowPromptMessage("检验人员不能为空");
                return;
            }

            S_CheckOutInDepotBill qualityInfo = new S_CheckOutInDepotBill();

            qualityInfo.Bill_ID = txtBill_ID.Text;
            qualityInfo.Checker = txtChecker.Text;
            qualityInfo.CheckoutReport_ID = txtCheckoutReportID.Text;

            if (!m_billServer.SubmitReportInfo(qualityInfo, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            MessageDialog.ShowPromptMessage("成功提交");

            RefreshDataGridView(m_billServer.GetAllBill());
            PositioningRecord(qualityInfo.Bill_ID);
        }
    }
}
