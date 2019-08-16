using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using GlobalObject;
using PlatformManagement;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 库房调拨单界面
    /// </summary>
    public partial class 库房调拨单 : Form
    {
        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 角色
        /// </summary>
        string m_strPersonnelType;

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 营销入库服务类
        /// </summary>
        ICannibalize m_serverCannibalize = ServerModuleFactory.GetServerModule<ICannibalize>();

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 单据消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_msgPromulgator = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        public 库房调拨单(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_msgPromulgator.BillType = "库房调拨单";

            m_billNoControl = new BillNumberControl(CE_BillTypeEnum.库房调拨单, m_serverCannibalize);

            m_authFlag = nodeInfo.Authority;

            dateTimePicker2.Value = ServerTime.Time.AddDays(1);
        }

        private void 库房调拨单_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authFlag);

            cmbDJ_ZT.SelectedIndex = 0;
            dateTimePicker1.Value = new DateTime(ServerTime.Time.Year, ServerTime.Time.Month, 1);
            RefreshDataGridView();
            m_strPersonnelType = PersonnelType.编制人.ToString();
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
                    break;

                case WndMsgSender.PositioningMsg:
                    WndMsgData msg = new WndMsgData();      //这是创建自定义信息的结构
                    Type dataType = msg.GetType();

                    msg = (WndMsgData)m.GetLParam(dataType);//这里获取的就是作为LParam参数发送来的信息的结构

                    DataTable dtMessage = UniversalFunction.PositioningOneRecord(msg.MessageContent, "库房调拨单");

                    if (dtMessage.Rows.Count == 0)
                    {
                        m_msgPromulgator.DestroyMessage(msg.MessageContent);
                        MessageDialog.ShowPromptMessage("未找到相关记录");
                    }
                    else
                    {
                        dgv_Show.DataSource = dtMessage;
                        dgv_Show.Rows[0].Selected = true;
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
        /// 添加事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            库房调拨明细单 form = new 库房调拨明细单(0, m_authFlag);
            form.ShowDialog();

            RefreshDataGridView();
            PositioningRecord(form.StrDJH);
        }

        /// <summary>
        /// 刷新DataGridView
        /// </summary>
        private void RefreshDataGridView()
        {
            DataTable dt = m_serverCannibalize.GetAllData(dateTimePicker1.Value.ToShortDateString(),
                                                    dateTimePicker2.Value.ToShortDateString(),
                                                    cmbDJ_ZT.Text,out m_err);
            if (dt == null)
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            dgv_Show.DataSource = dt;
        }

        /// <summary>
        /// 双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_Show_DoubleClick(object sender, EventArgs e)
        {
            ShowForm();
        }

        /// <summary>
        /// 更新事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            ShowForm();
        }

        /// <summary>
        /// 查询事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFind_Click(object sender, EventArgs e)
        {
            ShowForm();
        }

        /// <summary>
        /// 删除事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (BasicInfo.LoginName != dgv_Show.CurrentRow.Cells["LRRY"].Value.ToString())
                {
                    MessageDialog.ShowPromptMessage("您不是此记录的编制者无法进行此操作");
                    return;
                }

                if (dgv_Show.CurrentRow.Cells["DJH"].Value.ToString() == "")
                {
                    MessageDialog.ShowPromptMessage("请选择需要删除的记录");
                    return;
                }

                if (UniversalFunction.GetBillStatus("S_CannibalizeBill", "DJZT", "DJH", 
                    dgv_Show.CurrentRow.Cells["DJH"].Value.ToString()) == "已确认")
                {
                    MessageBox.Show("请重新核实单据状态","提示");
                    return;
                }

                if (MessageBox.Show("您是否确定要删除单据号为【" + dgv_Show.CurrentRow.Cells["DJH"].Value.ToString() + "】",
                    "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    m_serverCannibalize.DeleteBill(Convert.ToInt32(dgv_Show.CurrentRow.Cells["ID"].Value));
                    MessageBox.Show("删除成功", "提示");

                    m_msgPromulgator.DestroyMessage(dgv_Show.CurrentRow.Cells["DJH"].Value.ToString());
                    RefreshDataGridView();
                    m_billNoControl.CancelBill(dgv_Show.CurrentRow.Cells["DJH"].Value.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowErrorMessage(ex.Message);
                return;
            }
        }

        /// <summary>
        /// 显示明细窗体
        /// </summary>
        private void ShowForm()
        {
            if (dgv_Show.CurrentRow == null)
            {
                return;
            }

            库房调拨明细单 form = new 库房调拨明细单(Convert.ToInt32(dgv_Show.CurrentRow.Cells["ID"].Value), m_authFlag);
            form.ShowDialog();

            RefreshDataGridView();
            PositioningRecord(form.StrDJH);
        }

        /// <summary>
        /// 检查是否正确选择操作的记录行
        /// </summary>
        /// <returns>正确返回true</returns>
        bool CheckSelectedRow()
        {
            if (dgv_Show.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择一条记录后再进行此操作");
                return false;
            }
            else if (dgv_Show.SelectedRows.Count > 1)
            {
                MessageDialog.ShowPromptMessage("只允许选择一条记录进行处理！");
                return false;
            }

            return true;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
                return;

            if (dgv_Show.CurrentRow.Cells["DJZT"].Value.ToString() != "已确认")
            {
                MessageDialog.ShowPromptMessage("请选择已确认的记录后再进行此操作");
                return;
            }

            IBillReportInfo report = new 报表_调拨单(dgv_Show.CurrentRow.Cells["DJH"].Value.ToString(), labelTitle.Text);
            PrintReportBill print = new PrintReportBill(21.8, 9.31, report);
            print.DirectPrintReport();
        }

        private void 库房调拨单_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="billNo">定位用的单据号</param>
        void PositioningRecord(string billNo)
        {
            string strColName = "";

            foreach (DataGridViewColumn col in dgv_Show.Columns)
            {
                if (col.Visible)
                {
                    strColName = col.Name;
                    break;
                }
            }

            for (int i = 0; i < dgv_Show.Rows.Count; i++)
            {
                if ((string)dgv_Show.Rows[i].Cells["DJH"].Value == billNo)
                {
                    dgv_Show.FirstDisplayedScrollingRowIndex = i;
                    dgv_Show.CurrentCell = dgv_Show.Rows[i].Cells[strColName];
                    break;
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshDataGridView();
        }

        private void dgv_Show_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {

            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                e.RowBounds.Location.Y,
                dgv_Show.RowHeadersWidth - 4,
                e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                dgv_Show.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dgv_Show.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            RefreshDataGridView();
        }
    }
}
