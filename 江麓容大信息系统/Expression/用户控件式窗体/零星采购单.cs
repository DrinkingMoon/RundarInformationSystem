
/******************************************************************************
 *
 * 文件名称:  零星采购申请单.cs
 * 作者    :  邱瑶       日期: 2013/11/22
 * 开发平台:  vs2008(c#)
 * 用于    :  生产线管理信息系统
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GlobalObject;
using ServerModule;
using PlatformManagement;
using UniversalControlLibrary;

namespace Expression
{
    public partial class 零星采购单 : Form
    {
        /// <summary>
        /// 操作权限
        /// </summary>
        PlatformManagement.AuthorityFlag m_authorityFlag;

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_error;

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 零星采购服务类
        /// </summary>
        IMinorPurchaseBillServer m_minorBillServer = ServerModule.ServerModuleFactory.GetServerModule<IMinorPurchaseBillServer>();

        /// <summary>
        /// 人员档案服务组件
        /// </summary>
        Service_Peripheral_HR.IPersonnelArchiveServer m_personnerServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<Service_Peripheral_HR.IPersonnelArchiveServer>();

        public 零星采购单(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_billMessageServer.BillType = "零星采购申请单";
            m_authorityFlag = nodeInfo.Authority;

            #region 数据筛选
            string[] strBillStatus = {"全部",
                                     MinorPurchaseBillStatus.等待部门负责人审核.ToString(),
                                     MinorPurchaseBillStatus.等待确认日期.ToString(),
                                     MinorPurchaseBillStatus.等待分管领导审核.ToString(),
                                     MinorPurchaseBillStatus.等待财务审核.ToString(),
                                     MinorPurchaseBillStatus.等待总经理审核.ToString(),
                                     MinorPurchaseBillStatus.等待采购工程师确认采购.ToString(),
                                     MinorPurchaseBillStatus.等待确认到货.ToString(),
                                     MinorPurchaseBillStatus.已完成.ToString()};

            checkBillDateAndStatus1.InsertComBox(strBillStatus);
            checkBillDateAndStatus1.dtpStartTime.Value = ServerTime.Time.AddDays(1).AddMonths(-1);
            checkBillDateAndStatus1.dtpEndTime.Value = ServerTime.Time.AddDays(1);

            #endregion

            RefreshDataGridView();
        }

        private void 零星采购单_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authorityFlag);
        }

        /// <summary>
        /// 重新窗体消息处理函数
        /// </summary>
        /// <param name="message">窗体消息</param>
        protected override void DefWndProc(ref Message message)
        {
            switch (message.Msg)
            {
                case WndMsgSender.CloseMsg:
                    break;

                case WndMsgSender.PositioningMsg:
                    WndMsgData msg = new WndMsgData();      //这是创建自定义信息的结构
                    Type dataType = msg.GetType();

                    msg = (WndMsgData)message.GetLParam(dataType);//这里获取的就是作为LParam参数发送来的信息的结构

                    DataTable dtMessage = UniversalFunction.PositioningOneRecord(msg.MessageContent, "零星采购申请单");

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
                    base.DefWndProc(ref message);
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
        /// 定位记录
        /// </summary>
        /// <param name="billNo">定位用的单据号</param>
        void PositioningRecord(string billNo)
        {
            if (Convert.ToInt32(billNo) > 0)
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
                    if (dataGridView1.Rows[i].Cells["单据号"].Value.ToString() == billNo)
                    {
                        dataGridView1.FirstDisplayedScrollingRowIndex = i;
                        dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 刷新
        /// </summary>
        private void RefreshDataGridView()
        {
            m_minorBillServer.QueryResultFilter = QueryFilterControl.GetFilterString(labelTitle.Text)
                     + checkBillDateAndStatus1.GetSqlString("编制日期", "单据状态");

            IQueryResult result;

            if (!m_minorBillServer.GetAllBillInfo(out result, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            if (result.DataCollection == null || result.DataCollection.Tables.Count == 0)
            {
                return;
            }

            dataGridView1.DataSource = result.DataCollection.Tables[0];

            dataGridView1.Columns["编号"].Visible = false;
            dataGridView1.Columns["部门月预算内"].Visible = false;

            this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
               this.dataGridView1_ColumnWidthChanged);

            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            userControlDataLocalizer1.Init(dataGridView1, this.Name,
               UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));

            dataGridView1.Refresh();
        }

        private void 刷新toolStripButton_Click(object sender, EventArgs e)
        {
            RefreshDataGridView();
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        private void checkBillDateAndStatus1_OnCompleteSearch()
        {
            RefreshDataGridView();
        }

        private void 零星采购单_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string billNo = dataGridView1.CurrentRow.Cells["单据号"].Value.ToString();
            零星采购明细 frm = new 零星采购明细(m_authorityFlag, billNo);

            frm.ShowDialog();

            RefreshDataGridView();
            PositioningRecord(billNo);
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            零星采购明细 frm = new 零星采购明细(m_authorityFlag, null);

            frm.ShowDialog();

            RefreshDataGridView();
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Cells["是否紧急"].Value.ToString() == "紧急"
                        && dataGridView1.Rows[i].Cells["单据状态"].Value.ToString() != "已完成")
                    {
                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LightCoral;
                    }
                }
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                IBillReportInfo report = new 报表_零星采购单(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString());
                PrintReportBill print = new PrintReportBill(21.8, 9.31, report);
                (report as 报表_零星采购单).ShowDialog();

                if (m_minorBillServer.UpdatePrintStatus(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString(), out m_error))
                {
                    MessageDialog.ShowPromptMessage("打印成功！");
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请选择一行数据后再进行此操作！");
                return;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                if (dataGridView1.CurrentRow.Cells["编制人"].Value.ToString() == BasicInfo.LoginName)
                {
                    if (MessageDialog.ShowEnquiryMessage("确定删除选中的一条记录吗？") == DialogResult.Yes)
                    {
                        if (!m_minorBillServer.DeleteInfo(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString(), out m_error))
                        {
                            MessageDialog.ShowPromptMessage(m_error);
                            return;
                        }
                        else
                        {
                            m_billMessageServer.DestroyMessage(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString());
                            RefreshDataGridView();
                        }
                    }
                }
                else
                {
                    MessageDialog.ShowPromptMessage("您不的编制人，不能删除此单据！");
                    return;
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请选择一行数据后再进行此操作！");
            }
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            lblBillStatus.Text = dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString();
        }

        private void btnSetFilter_Click(object sender, EventArgs e)
        {
            零星采购申请统计报表 frm = new 零星采购申请统计报表();

            frm.ShowDialog();
        }

        private void 综合查询toolStripButton_Click(object sender, EventArgs e)
        {
            IAuthorization authorization = PlatformFactory.GetObject<IAuthorization>();
            string businessID = "零星采购申请综合查看";
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

        private void dataGridView1_MouseHover(object sender, EventArgs e)
        {
            //if (dataGridView1.HitTest(p.X, p.Y).RowIndex <= 0)
            //{
            //    return;
            //}

            //string temp = "";

            //DataTable tempTable = 
            //    m_minorBillServer.GetListInfo(dataGridView1.Rows[dataGridView1.HitTest(p.X, p.Y).RowIndex].Cells["单据号"].Value.ToString());

            //foreach (DataRow dr in tempTable.Rows)
            //{
            //    temp += dr["物品名称"].ToString() + "\n";
            //}

            //toolTip1.Show(temp, dataGridView1, p);
        }

        private void dataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {

            //if (e.RowIndex < 0 || e.ColumnIndex < 0)
            //{
            //    return;
            //}

            //string temp = "";

            //DataTable tempTable =
            //    m_minorBillServer.GetListInfo(dataGridView1.Rows[e.RowIndex].Cells["单据号"].Value.ToString());

            //foreach (DataRow dr in tempTable.Rows)
            //{
            //    temp += dr["物品名称"].ToString() + "\n";
            //}

            //temp = temp.Substring(0, temp.Length - 1);

            //dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = temp;
        }

        private void 财务toolStripButton_Click(object sender, EventArgs e)
        {
            IAuthorization authorization = PlatformFactory.GetObject<IAuthorization>();
            string businessID = "零星采购申请财务综合查询";
            string[] pare = {dataGridView1.CurrentRow.Cells["单据号"].Value.ToString()};

            IQueryResult qr = authorization.QueryMultParam(businessID, null, pare);
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
    }
}
