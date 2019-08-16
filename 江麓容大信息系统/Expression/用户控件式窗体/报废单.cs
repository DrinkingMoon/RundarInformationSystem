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
    /// 报废单界面
    /// </summary>
    public partial class 报废单 : Form
    {
        #region 成员变量

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_error;

        /// <summary>
        /// 报废单服务组件m_scrapBillServer
        /// </summary>
        IScrapBillServer m_billServer = ServerModuleFactory.GetServerModule<IScrapBillServer>();

        /// <summary>
        /// 报废物品服务组件m_scrapGoodsServer
        /// </summary>
        IScrapGoodsServer m_goodsServer = ServerModuleFactory.GetServerModule<IScrapGoodsServer>();

        /// <summary>
        /// 部门服务
        /// </summary>
        IDepartmentServer m_deptServer = ServerModuleFactory.GetServerModule<IDepartmentServer>();

        /// <summary>
        /// 报废类别
        /// </summary>
        IDeclareWastrelType m_declareWastrelTyp = ServerModuleFactory.GetServerModule<IDeclareWastrelType>();

        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 检索到的单据结果
        /// </summary>
        IQueryResult m_queryResult;

        /// <summary>
        /// 操作权限
        /// </summary>
        AuthorityFlag m_authorityFlag;

        /// <summary>
        /// 可供查找的所有字段
        /// </summary>
        string[] m_findField = null;

        /// <summary>
        /// 数据定位控件
        /// </summary>
        UserControlDataLocalizer m_dataLocalizer;

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_msgPromulgator = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        #endregion 成员变量

        public 报废单(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_msgPromulgator.BillType = "报废单";

            m_billNoControl = new BillNumberControl(labelTitle.Text, m_billServer);

            m_authorityFlag = nodeInfo.Authority;

            dateTimePickerST.Value = ServerModule.ServerTime.Time;
            dateTimePickerET.Value = ServerModule.ServerTime.Time.AddDays(1);

            m_billServer.QueryResultFilter = QueryFilterControl.GetFilterString(labelTitle.Text);
            RefreshData();
        }

        private void 报废单_Load(object sender, EventArgs e)
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
                //接收自定义消息,放弃未提交的单据号
                case WndMsgSender.CloseMsg:
                    // 放弃未使用的单据号
                    m_billNoControl.CancelBill();
                    break;

                case WndMsgSender.PositioningMsg:
                    WndMsgData msg = new WndMsgData();      //这是创建自定义信息的结构
                    Type dataType = msg.GetType();

                    msg = (WndMsgData)m.GetLParam(dataType);//这里获取的就是作为LParam参数发送来的信息的结构

                    DataTable dtMessage = UniversalFunction.PositioningOneRecord(msg.MessageContent, "报废单");

                    //if (msg.ObjectMessage != IntPtr.Zero)
                    //{
                    //    List<string> d = (List<string>)GeneralFunction.IntPtrToClass(msg.ObjectMessage, msg.BytesOfObjectMessage);
                    //}

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
        /// 权限控制
        /// </summary>
        /// <param name="authorityFlag">权限标志</param>
        void AuthorityControl(PlatformManagement.AuthorityFlag authorityFlag)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip1, authorityFlag);
        }

        /// <summary>
        /// 查找并刷新数据
        /// </summary>
        private void RefreshData()
        {
            if (!m_billServer.GetAllBill(out m_queryResult, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            RefreshDataGridView(m_queryResult);
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
        /// <param name="row">选中记录行</param>
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

        #endregion 数据检测

        #region 刷新数据

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

            if (chk显示所有单据.Checked)
            {
                filterExpression = "";
            }
            else
            {
                if (chk显示新建单据.Checked)
                {
                    if (filterExpression.Length == 0)
                        filterExpression = "单据状态 = '新建单据'";
                    else
                        filterExpression += " or (单据状态 = '新建单据')";
                }

                if (chk显示待审核单据.Checked)
                {
                    if (filterExpression.Length == 0)
                        filterExpression = "单据状态 = '等待主管审核'";
                    else
                        filterExpression += " or (单据状态 = '等待主管审核')";
                }

                if (chk显示待质检单据.Checked)
                {
                    if (filterExpression.Length == 0)
                        filterExpression = "单据状态 = '等待质检批准'";
                    else
                        filterExpression += " or (单据状态 = '等待质检批准')";
                }

                if (chk显示等待SQE处理单据.Checked)
                {
                    if (filterExpression.Length == 0)
                        filterExpression = "单据状态 = '等待SQE处理意见'";
                    else
                        filterExpression += " or (单据状态 = '等待SQE处理意见')";
                }

                if (chk显示待仓库确认单据.Checked)
                {
                    if (filterExpression.Length == 0)
                        filterExpression = "单据状态 = '等待仓管确认'";
                    else
                        filterExpression += " or (单据状态 = '等待仓管确认')";
                }

                if (chk显示已完成单据.Checked)
                {
                    if (filterExpression.Length == 0)
                        filterExpression = "单据状态 = '已完成'";
                    else
                        filterExpression += " or (单据状态 = '已完成')";
                }

                if (GlobalObject.GeneralFunction.IsNullOrEmpty(filterExpression))
                {
                    dt = null;
                }
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
        /// 刷新DataGridView
        /// </summary>
        /// <param name="findBill">数据集</param>
        void RefreshDataGridView(IQueryResult findBill)
        {
            ClearForm();
            dataGridView1.DataSource = GetDataSource(findBill);

            if (dataGridView1.DataSource == null)
            {
                return;
            }

            this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            #region 隐藏不允许查看的列

            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                if (findBill.HideFields.Contains(dataGridView1.Columns[i].Name))
                {
                    dataGridView1.Columns[i].Visible = false;
                }
            }

            #endregion

            dataGridView1.Columns["申请人编码"].Visible = false;
            dataGridView1.Columns["申请部门编码"].Visible = false;
            dataGridView1.Columns["知会检验名编码"].Visible = false;

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
        /// 清除窗体上的控件残留信息
        /// </summary>
        void ClearForm()
        {
            lblBillStatus.Text = "";

            txtBill_ID.Text = "";
            dateTime_BillTime.Value = ServerModule.ServerTime.Time;
            dateTimePicker批准.Value = dateTime_BillTime.Value;
            dateTimePicker仓库确认.Value = dateTime_BillTime.Value;

            txtChecker.Text = "";
            txtDeclareDepartment.Text = "";
            txtDeclarePersonnel.Text = "";
            txtDepartmentDirector.Text = "";
            txtDepotManager.Text = "";
            txtSanction.Text = "";
            txtRemark.Text = "";
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="billNo">定位用的单据号</param>
        public void PositioningRecord(string billNo)
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
                if ((string)dataGridView1.Rows[i].Cells["报废单号"].Value == billNo)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                    break;
                }
            }
        }

        #endregion 刷新数据

        #region DataGridView 事件
        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            ClearForm();

            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

            txtBill_ID.Text = (string)row.Cells["报废单号"].Value;
            dateTime_BillTime.Value = (DateTime)row.Cells["报废时间"].Value;

            if (row.Cells["批准时间"].Value != System.DBNull.Value)
                dateTimePicker批准.Value = (DateTime)row.Cells["批准时间"].Value;

            if (row.Cells["仓库确认时间"].Value != System.DBNull.Value)
                dateTimePicker仓库确认.Value = (DateTime)row.Cells["仓库确认时间"].Value;

            txtDeclareDepartment.Text = (string)row.Cells["申请部门"].Value;
            txtDeclarePersonnel.Text = (string)row.Cells["申请人签名"].Value;

            if (row.Cells["部门主管签名"].Value != System.DBNull.Value)
                txtDepartmentDirector.Text = (string)row.Cells["部门主管签名"].Value;

            if (row.Cells["检验员"].Value != System.DBNull.Value)
                txtChecker.Text = (string)row.Cells["检验员"].Value;

            if (row.Cells["审批人签名"].Value != System.DBNull.Value)
                txtSanction.Text = (string)row.Cells["审批人签名"].Value;

            if (row.Cells["仓管签名"].Value != System.DBNull.Value)
                txtDepotManager.Text = (string)row.Cells["仓管签名"].Value;

            if (row.Cells["备注"].Value != System.DBNull.Value)
                txtRemark.Text = (string)row.Cells["备注"].Value;

            lblBillStatus.Text = (string)row.Cells["单据状态"].Value;
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            查看物品清单ToolStripMenuItem_Click(sender, e);
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
        #endregion

        /// <summary>
        /// 打印单据
        /// </summary>
        /// <param name="billNo">单据编号</param>
        public void PrintBill(string billNo, string billStatus)
        {
            if (billStatus != ScrapBillStatus.已完成.ToString())
            {
                MessageDialog.ShowPromptMessage("请选择已完成的记录后再进行此操作");
                return;
            }

            报表_报废单 report = new 报表_报废单(txtBill_ID.Text, labelTitle.Text);
            PrintReportBill print = new PrintReportBill(22.0, 9.31, report);
            print.DirectPrintReport();
        }

        private void 表单打印_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
                return;

            PrintBill(txtBill_ID.Text, lblBillStatus.Text);
        }

        public void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        public void DeleteBill(string billNo, string billStatus)
        {
            ScrapBillStatus status = (ScrapBillStatus)Enum.Parse(typeof(ScrapBillStatus), billStatus);

            if (status == ScrapBillStatus.已完成)
            {
                MessageDialog.ShowPromptMessage("请选择未完成的记录后再进行此操作");
                return;
            }

            string info = string.Format("您是否要删除 {0} 报废单时, 删除时同时也会删除此报废单下的所有物品清单, 是否继续？", billNo);

            if (MessageDialog.ShowEnquiryMessage(info) == DialogResult.No)
            {
                return;
            }

            if (!m_billServer.DeleteBill(billNo, true, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            m_billNoControl.CancelBill(billNo);
            m_msgPromulgator.DestroyMessage(billNo);
        }

        private void 高级检索_Click(object sender, EventArgs e)
        {
            IAuthorization authorization = PlatformFactory.GetObject<IAuthorization>();
            string businessID = "报废单综合查询";
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

        private void 设置数据过滤器_Click(object sender, EventArgs e)
        {
            FormFilterCondition form = new FormFilterCondition(labelTitle.Text, m_findField, labelTitle.Text);
            form.ShowDialog();

            m_billServer.QueryResultFilter = QueryFilterControl.GetFilterString(labelTitle.Text);
            RefreshData();
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            报废单清单 form = new 报废单清单(m_authorityFlag, this, null);
            form.ShowDialog();

            m_billServer.GetAllBill(out m_queryResult, out m_error);
            RefreshDataGridView(m_queryResult);
            PositioningRecord(form.BillNo);
        }

        private void 查看物品清单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
            {
                return;
            }

            string strBillID = txtBill_ID.Text;

            报废单清单 form = new 报废单清单(m_authorityFlag, this, txtBill_ID.Text);
            form.ShowDialog();

            m_billServer.GetAllBill(out m_queryResult, out m_error);
            RefreshDataGridView(m_queryResult);
            PositioningRecord(strBillID);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
            {
                return;
            }

            if (!this.CheckUserOperation(dataGridView1.SelectedRows[0]))
            {
                return;
            }

            string strBillID = txtBill_ID.Text;

            报废单清单 form = new 报废单清单(m_authorityFlag, this, txtBill_ID.Text);
            form.ShowDialog();

            m_billServer.GetAllBill(out m_queryResult, out m_error);
            RefreshDataGridView(m_queryResult);
            PositioningRecord(strBillID);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
                return;

            if (!this.CheckUserOperation(dataGridView1.SelectedRows[0]))
            {
                return;
            }

            if (txtBill_ID.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请选择需要删除的记录");
                return;
            }

            IMaterialRequisitionServer mrs = ServerModuleFactory.GetServerModule<IMaterialRequisitionServer>();

            if (mrs.IsExistAssociatedBill(txtBill_ID.Text))
            {
                MessageDialog.ShowPromptMessage("已经有领料单使用了此报废单号不允许再进行此操作!\n如需删除此报废单，请先删除关联此报废单的所有领料单");
                return;
            }

            if (UniversalFunction.GetBillStatus("S_ScrapBill", "BillStatus", "Bill_ID",
                txtBill_ID.Text) == ScrapBillStatus.已完成.ToString())
            {
                MessageDialog.ShowPromptMessage("不能删除已完成单据！");
                return;
            }

            if (MessageDialog.ShowEnquiryMessage("您是否要删除此单据") == DialogResult.No)
            {
                return;
            }

            DeleteBill(txtBill_ID.Text, lblBillStatus.Text);

            m_billServer.GetAllBill(out m_queryResult, out m_error);
            RefreshDataGridView(m_queryResult);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            m_billServer.GetAllBill(dateTimePickerST.Value, dateTimePickerET.Value, out m_queryResult, out m_error);
            RefreshDataGridView(m_queryResult);
        }

        private void chk显示控制_CheckedChanged(object sender, EventArgs e)
        {
            //btnRefresh_Click(sender, e);
            RefreshDataGridView(m_queryResult);
        }

        private void tlsb单据统计_Click(object sender, EventArgs e)
        {
            单据统计 frm = new 单据统计("报废单统计");
            frm.Show();
        }
    }
}
