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
    /// 售后服务配件制造申请单界面
    /// </summary>
    public partial class 售后服务配件制造申请单 : Form
    {
        /// <summary>
        /// 权限组件
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer =
            BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 单据消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_msgPromulgator = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 获取单据流消息操作接口
        /// </summary>
        IBillFlowMessage m_billFlowMsg = PlatformFactory.GetObject<IBillFlowMessage>();

        /// <summary>
        /// 售后服务配件制造申请单服务接口
        /// </summary>
        IAfterServiceMakePartsBill m_billServer = ServerModuleFactory.GetServerModule<IAfterServiceMakePartsBill>();

        /// <summary>
        /// 数据定位控件
        /// </summary>
        UserControlDataLocalizer m_dataLocalizer;

        /// <summary>
        /// 窗体消息发布器
        /// </summary>
        WndMsgSender m_wndMsgSender = new WndMsgSender();

        public 售后服务配件制造申请单(FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_msgPromulgator.BillType = "售后服务配件制造申请单";

            m_authFlag = nodeInfo.Authority;
            m_billNoControl = new BillNumberControl(CE_BillTypeEnum.售后服务备件制造申请单, m_billServer);

            dateTimeBegin.Value = new DateTime(ServerTime.Time.Year, ServerTime.Time.Month, 1);
            dateTimeEnd.Value = ServerTime.Time.AddDays(1);
            cmbDJ_ZT.Text = "全  部";

            RefreshData();
        }

        private void 售后服务配件制造申请单_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authFlag);
        }

        /// <summary>
        /// 权限控制
        /// </summary>
        /// <param name="authorityFlag">权限标志</param>
        private void AuthorityControl(PlatformManagement.AuthorityFlag authorityFlag)
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
                case WndMsgSender.CloseMsg:
                    break;
                case WndMsgSender.PositioningMsg:
                    WndMsgData msg = new WndMsgData();   //这是创建自定义信息的结构
                    Type dataType = msg.GetType();

                    msg = (WndMsgData)m.GetLParam(dataType);//这里获取的就是作为LParam参数发送来的信息的结构

                    DataTable dtMessage = UniversalFunction.PositioningOneRecord(msg.MessageContent, "售后服务备件制造申请单");

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
        private void PositioningRecord(string billNo)
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
        /// 刷新数据
        /// </summary>
        private void RefreshData()
        {
            dataGridView1.DataSource = m_billServer.GetBill(dateTimeBegin.Value, dateTimeEnd.Value,cmbDJ_ZT.Text);

            this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);
            
            if (m_dataLocalizer == null)
            {
                m_dataLocalizer = new UserControlDataLocalizer(dataGridView1, this.Name, 
                    UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
                panelTop.Controls.Add(m_dataLocalizer);
                m_dataLocalizer.Dock = DockStyle.Bottom;
            }

            dataGridView1.Refresh();
        }

        /// <summary>
        /// 新建单据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            售后服务配件制造申请单明细单 form = new 售后服务配件制造申请单明细单(null, m_authFlag);
            form.ShowDialog();

            RefreshData();
            PositioningRecord(form.Bill);
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        private void 售后服务配件制造申请单_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
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
        /// 检查是否正确选择操作的记录行
        /// </summary>
        /// <returns>正确返回true</returns>
        private bool CheckSelectedRow()
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
        /// 删除单据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择一条记录后再进行此操作");
                return;
            }

            List<string> lstBillID = new List<string>();

            for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
            {
                if (dataGridView1.SelectedRows[i].Cells["单据状态"].Value.ToString() != "已完成"
                    && dataGridView1.SelectedRows[i].Cells["申请人"].Value.ToString() == BasicInfo.LoginName)
                {
                    lstBillID.Add((string)dataGridView1.SelectedRows[i].Cells["单据号"].Value);
                }
            }

            if (MessageDialog.ShowEnquiryMessage("您是否要删除此单据") == DialogResult.No)
            {
                return;
            }

            if (!m_billServer.DeleteBill(lstBillID, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
            }
            else
            {
                foreach (string item in lstBillID)
                {
                    m_billNoControl.CancelBill(item.ToString());
                    m_msgPromulgator.DestroyMessage(item.ToString());
                }

                RefreshData();

            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
            {
                return;
            }

            售后服务配件制造申请单明细单 form = new 售后服务配件制造申请单明细单(
                (string)dataGridView1.SelectedRows[0].Cells["单据号"].Value, m_authFlag);
            form.ShowDialog();

            RefreshData();
            PositioningRecord(form.Bill);
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
            {
                return;
            }

            售后服务配件制造申请单明细单 form = new 售后服务配件制造申请单明细单(
                (string)dataGridView1.SelectedRows[0].Cells["单据号"].Value, m_authFlag);
            form.ShowDialog();

            RefreshData();
            PositioningRecord(form.Bill);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            RefreshData();
        }
    }
}
