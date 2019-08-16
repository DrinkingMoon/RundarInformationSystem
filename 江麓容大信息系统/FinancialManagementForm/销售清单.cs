using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using GlobalObject;
using PlatformManagement;
using UniversalControlLibrary;
using Service_Economic_Financial;

namespace Form_Economic_Financial
{
    /// <summary>
    /// 销售清单
    /// </summary>
    public partial class 销售清单 : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_error;

        /// <summary>
        /// 单据号
        /// </summary>
        string m_billNo;

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 销售清单服务类
        /// </summary>
        IMarketingPartBillServer m_marketPartBillServer = Service_Economic_Financial.ServerModuleFactory.GetServerModule<IMarketingPartBillServer>();

        /// <summary>
        /// 单据消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_msgPromulgator = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        public 销售清单(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authFlag = nodeInfo.Authority;

            #region 数据筛选
            string[] strBillStatus = { "全部", "等待销售人员确认", "等待主管审核", "等待财务审核", "已完成" };

            checkBillDateAndStatus1.InsertComBox(strBillStatus);

            checkBillDateAndStatus1.OnCompleteSearch +=
                new GlobalObject.DelegateCollection.NonArgumentHandle(checkBillDateAndStatus1_OnCompleteSearch);

            checkBillDateAndStatus1.dtpStartTime.Value = ServerTime.Time.AddDays(1).AddMonths(-1);
            checkBillDateAndStatus1.dtpEndTime.Value = ServerTime.Time.AddDays(1);

            #endregion

            RefreshDataGridView();
            m_msgPromulgator.BillType = "销售清单";
        }

        private void 销售清单_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authFlag);
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
        /// 重新窗体消息处理函数
        /// </summary>
        /// <param name="m">窗体消息</param>
        protected override void DefWndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WndMsgSender.PositioningMsg:
                    WndMsgData msg = new WndMsgData();      //这是创建自定义信息的结构
                    Type dataType = msg.GetType();

                    msg = (WndMsgData)m.GetLParam(dataType);//这里获取的就是作为LParam参数发送来的信息的结构

                    DataTable dtMessage = UniversalFunction.PositioningOneRecord(msg.MessageContent, "销售清单");

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
        /// 刷新
        /// </summary>
        void RefreshDataGridView()
        {
            m_marketPartBillServer.QueryResultFilter = QueryFilterControl.GetFilterString(labelTitle.Text)
                      + checkBillDateAndStatus1.GetSqlString("销售时间", "单据状态");

            IQueryResult result;

            if (!m_marketPartBillServer.GetAllBill(out result, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            dataGridView1.DataSource = result.DataCollection.Tables[0];

            this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
               this.dataGridView1_ColumnWidthChanged);

            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            userControlDataLocalizer1.Init(dataGridView1, this.Name,
               UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));

            foreach (DataGridViewColumn item in dataGridView1.Columns)
            {
                if (item.Name != "选")
                {
                    item.ReadOnly = true;
                }
                else
                {
                    item.ReadOnly = false;
                    item.Frozen = false;
                }
            }

            dataGridView1.Columns["ClientID"].Visible = false;
            dataGridView1.Columns["价格套用的整车厂"].Visible = false;
            dataGridView1.Columns["价格套用的整车厂编码"].Visible = false;
        }

        private void 刷新toolStripButton_Click(object sender, EventArgs e)
        {
            RefreshDataGridView();
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            RefreshDataGridView();
        }

        private void 打印toolStripButton_Click(object sender, EventArgs e)
        {
            bool flag = false;
            string billNo = "";
            string clientName = "";

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dataGridView1.Rows[i].Cells["选"].Value))
                {
                    if (flag)
                    {
                        MessageDialog.ShowPromptMessage("只能选中一行数据！");
                        return;
                    }
                    else
                    {

                        billNo = dataGridView1.Rows[i].Cells["单据号"].Value.ToString();
                        clientName = dataGridView1.Rows[i].Cells["客户名称"].Value.ToString();

                        if (dataGridView1.Rows[i].Cells["单据状态"].Value.ToString() == "已完成")
                        {
                            flag = true;
                        }
                        else
                        {
                            MessageDialog.ShowPromptMessage("请确认单据状态！");
                            return;
                        }
                    }
                }
            }

            if (flag)
            {
                m_marketPartBillServer.PrintUpodateData(billNo, out m_error);

                //报表_销售清单 report = new 报表_销售清单(billNo, clientName);
                //report.ShowDialog();
            }
            else
            {
                MessageDialog.ShowPromptMessage("请选择一行数据进行打印！");
            }
        }

        private void 选择打印toolStripButton_Click(object sender, EventArgs e)
        {
            bool flag = false;
            string billNo = "";

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dataGridView1.Rows[i].Cells["选"].Value))
                {
                    if (dataGridView1.Rows[i].Cells["单据状态"].Value.ToString() == "已完成")
                    {
                        flag = true;
                    }
                    else
                    {
                        MessageDialog.ShowPromptMessage("请确认单据状态！");
                        return;
                    }
                    
                    if (billNo == "")
                    {
                        billNo += dataGridView1.Rows[i].Cells["单据号"].Value.ToString() + "";
                    }
                    else
                    {
                        billNo += "','" + dataGridView1.Rows[i].Cells["单据号"].Value.ToString() + "";
                    }
                }
            }

            if (flag)
            {
                m_billNo = billNo;
                m_marketPartBillServer.PrintUpodateData(m_billNo, out m_error);

                //报表_销售清单 report = new 报表_销售清单(m_billNo,"终端客户");
                //report.ShowDialog();
            }
            else
            {
                MessageDialog.ShowPromptMessage("请勾选需要打印的销售清单！");
            }
        }

        private void 选择ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string billNo = "";

            for (int i = 0; i <= dataGridView1.SelectedRows.Count - 1; i++)
            {
                dataGridView1.SelectedRows[i].Cells["选"].Value = true;

                if (i == 0)
                {
                    billNo += dataGridView1.SelectedRows[i].Cells["单据号"].Value.ToString() + "',";
                }
                else
                {
                    billNo += "'" + dataGridView1.SelectedRows[i].Cells["单据号"].Value.ToString() + "";
                }
            }

            m_billNo = billNo;
        }
        
        private void 全消ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells["选"].Value = false;
            }

            m_billNo = "";
        }

        private void 销售清单_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string billNo = dataGridView1.CurrentRow.Cells["单据号"].Value.ToString();

            销售清单明细 frm = new 销售清单明细(m_authFlag, billNo);
            frm.ShowDialog();
            
            RefreshDataGridView();
            PositioningRecord(billNo);
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dataGridView1.CurrentCell.ColumnIndex == 0)
            {
                if (Convert.ToBoolean(dataGridView1.CurrentRow.Cells["选"].Value))
                {
                    dataGridView1.CurrentRow.Cells["选"].Value = false;
                }
                else
                {
                    dataGridView1.CurrentRow.Cells["选"].Value = true;
                }
            }
        }

        private void 日志toolStripButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }

            FormShowYXLowestPriceError frm = new FormShowYXLowestPriceError(null,dataGridView1.CurrentRow.Cells["单据号"].Value.ToString());

            frm.ShowDialog();
        }

        private void checkBillDateAndStatus1_OnCompleteSearch()
        {
            RefreshDataGridView();
        }

        private void toolStripButton导出_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = m_marketPartBillServer.GetExcelData(
                checkBillDateAndStatus1.dtpStartTime.Value, checkBillDateAndStatus1.dtpEndTime.Value,
                checkBillDateAndStatus1.cmbBillStatus.Text);

            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
            RefreshDataGridView();
        }
    }
}
