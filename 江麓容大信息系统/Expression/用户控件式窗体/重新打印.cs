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
    /// 重新打印界面
    /// </summary>
    public partial class 重新打印 : Form
    {
        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 打印信息服务
        /// </summary>
        IPrintManagement m_serverPrintManagement = BasicServerFactory.GetServerModule<IPrintManagement>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr;

        /// <summary>
        /// 获取单据流消息操作接口
        /// </summary>
        IBillFlowMessage m_billFlowMsg = PlatformFactory.GetObject<IBillFlowMessage>();

        /// <summary>
        /// 窗体消息发布器
        /// </summary>
        WndMsgSender m_wndMsgSender = new WndMsgSender();

        /// <summary>
        /// 显示标志
        /// </summary>
        bool m_bFlag = true;

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag = AuthorityFlag.Nothing;

        public 重新打印(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authFlag = nodeInfo.Authority;

            dateTime_startTime.Value = new DateTime(ServerTime.Time.Year, ServerTime.Time.Month, 1);
            dateTime_endTime.Value = ServerTime.Time.AddDays(1);

            DataBind();
        }

        private void 重新打印_Load(object sender, EventArgs e)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip1, m_authFlag);

            DataBind();
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
                    break;

                case WndMsgSender.PositioningMsg:
                    WndMsgData msg = new WndMsgData();      //这是创建自定义信息的结构
                    Type dataType = msg.GetType();

                    msg = (WndMsgData)m.GetLParam(dataType);//这里获取的就是作为LParam参数发送来的信息的结构

                    DataTable dtMessage = UniversalFunction.PositioningOneRecord(msg.MessageContent, "重新打印");

                    if (dtMessage.Rows.Count == 0)
                    {
                        Flow_BillFlowMessage billmsg = m_billFlowMsg.GetMessage(BasicInfo.LoginID, "重新打印", msg.MessageContent);

                        m_billFlowMsg.DestroyMessage(BasicInfo.LoginID, billmsg.序号);

                        MessageDialog.ShowPromptMessage("未找到相关记录");
                    }
                    else
                    {
                        dgvShowInfo.DataSource = dtMessage;
                        dgvShowInfo.Rows[0].Selected = true;
                    }

                    break;

                default:
                    base.DefWndProc(ref m);
                    break;
            }
        }

        /// <summary>
        /// 添加需重新打印的单据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtBill_ID.Text == string.Empty)
            {
                MessageDialog.ShowPromptMessage("请填写单据号！");
                return;
            }

            if (txtPrintRemark.Text == string.Empty)
            {
                MessageDialog.ShowPromptMessage("请填写重打原因！");
                return;
            }

            DataTable dt = m_serverPrintManagement.GetPrintBillTableByDJH(txtBill_ID.Text);

            if (dt.Rows.Count > 0)
            {
                S_AgainPrintBillTable againprint = new S_AgainPrintBillTable();

                againprint.Bill_ID = txtBill_ID.Text;
                againprint.PrintPersonnelCode = BasicInfo.LoginID;
                againprint.PrintPersonnelName = BasicInfo.LoginName;
                againprint.PrintPersonnelDepartment = BasicInfo.DeptName;
                againprint.PrintDateTime = ServerTime.Time;
                againprint.Remark = txtPrintRemark.Text;

                bool b = m_serverPrintManagement.Add_S_AgainPrintBillTable(againprint, out m_strErr);

                if (b)
                {
                    MessageBox.Show("提交成功！等待主管审核");

                    Flow_BillFlowMessage msg = m_billFlowMsg.GetMessage(BasicInfo.LoginID, labelTitle.Text, txtBill_ID.Text);

                    if (msg != null)
                    {
                        m_billFlowMsg.DestroyMessage(BasicInfo.LoginID, msg.序号);
                    }

                    SendNewFlowMessage(txtBill_ID.Text);
                }
                else
                {
                    MessageDialog.ShowErrorMessage(m_strErr);
                }
            }
            else
            {
                MessageDialog.ShowErrorMessage("单据号不正确 或 此单据号还没有打印过！");
            }
            
        }

        /// <summary>
        /// 绑定DataGridView
        /// </summary>
        public void DataBind()
        {
            DataTable dt = m_serverPrintManagement.GetAgainPrintBill(dateTime_startTime.Value, dateTime_endTime.Value);
            dgvShowInfo.DataSource = dt;

            m_bFlag = true;

            if (dt == null)
            {
                return;
            }

            dgvShowInfo.Columns["序号"].Visible = false;
            dgvShowInfo.Columns["申请人编号"].Visible = false;

            dateTime_startTime.Value = new DateTime(ServerTime.Time.Year, ServerTime.Time.Month, 1);
            dateTime_endTime.Value = ServerTime.Time.AddDays(1);

            userControlDataLocalizer1.Init(dgvShowInfo, this.Name,
               UniversalFunction.SelectHideFields(this.Name, dgvShowInfo.Name, BasicInfo.LoginID));

            dgvShowInfo.Refresh();
        }

        #region 消息流
        /// <summary>
        /// 发送新的流消息
        /// </summary>
        /// <param name="billNo">单据号</param>
        void SendNewFlowMessage(string billNo)
        {
            try
            {
                Flow_BillFlowMessage msg = new Flow_BillFlowMessage();

                msg.初始发起方用户编码 = BasicInfo.LoginID;
                msg.单据号 = billNo;
                msg.单据类型 = labelTitle.Text;
                msg.单据流水号 = billNo;
                msg.接收方类型 = BillFlowMessage_ReceivedUserType.角色.ToString();
                msg.单据状态 = BillStatus.等待处理.ToString();

                msg.发起方消息 = string.Format("{0} 号单据重新打印，请主管审核", billNo);

                string[] roleCodes = m_billMessageServer.GetDeptDirectorRoleName(BasicInfo.DeptCode);

                msg.接收方 = "";
                if (roleCodes.Count() > 0)
                {
                    foreach (string role in roleCodes)
                    {
                        msg.接收方 += role + ",";
                    }
                }

                msg.接收方 = msg.接收方.Substring(0, msg.接收方.Length - 1);
                msg.期望的处理完成时间 = null;

                m_billFlowMsg.SendRequestMessage(BasicInfo.LoginID, msg);
            }
            catch (Exception exce)
            {
                Console.WriteLine(exce.Message);
            }
        }

        /// <summary>
        /// 传递流消息(走流程)
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="msgContent">消息内容</param>
        /// <param name="receivedRole">接收方角色</param>
        void PassFlowMessage(string billNo, string msgContent, CE_RoleEnum receivedRole)
        {
            Flow_BillFlowMessage msg = m_billFlowMsg.GetMessage(BasicInfo.LoginID, labelTitle.Text, billNo);

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
            SendFinishedFlagToMessagePromptForm(billNo);
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

                List<string> noticeUsers = new List<string>();
                noticeUsers.Add(msg.初始发起方用户编码);

                m_billMessageServer.NotifyMessage(msg.单据类型, msg.单据号, msgContent, BasicInfo.LoginID, null, noticeUsers);

                #endregion 发送知会消息
            }
            catch (Exception exce)
            {
                Console.WriteLine(exce.Message);
            }
        }

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

        #endregion

        /// <summary>
        /// toolscriptbutton的审核按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAuditing_Click(object sender, EventArgs e)
        {
            bool b = m_serverPrintManagement.UpdateAgainPrintBillTable(dgvShowInfo.CurrentRow.Cells["序号"].Value.ToString(), out m_strErr);

            if (b)
            {
                string msg = string.Format("{0} 号单据重新打印主管已审核,等待财务批准", txtBill_ID.Text);

                MessageDialog.ShowPromptMessage("成功审核!");

                PassFlowMessage(txtBill_ID.Text, msg, CE_RoleEnum.财务主管);
            }
            else
            {
                MessageDialog.ShowErrorMessage(m_strErr);
            }

            DataBind();
        }

        /// <summary>
        /// 财务批准
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAuthorize_Click(object sender, EventArgs e)
        {
            bool b = m_serverPrintManagement.UpdateAuthorize(dgvShowInfo.CurrentRow.Cells["序号"].Value.ToString(), out m_strErr);

            if (b)
            {
                string msg = string.Format("{0} 号单据重新打印财务批准", txtBill_ID.Text);

                MessageDialog.ShowPromptMessage("批准成功!");

                EndFlowMessage(msg);
            }
            else
            {
                MessageDialog.ShowErrorMessage(m_strErr);
            }

            DataBind();
        }

        /// <summary>
        /// 通过单据号查询该单据的信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (txtBill_ID.Text == "")
            {
                DataBind();
            }
            else
            {
                DataTable dt = m_serverPrintManagement.GetPrintBillTableByDJH(txtBill_ID.Text);

                if (dt.Rows.Count == 0)
                {
                    MessageDialog.ShowPromptMessage("查找不到【" + txtBill_ID.Text + "】相关的单据信息！");
                }

                dgvShowInfo.DataSource = dt;
                m_bFlag = false;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string start = dateTime_startTime.Value.ToShortDateString();
            string end = dateTime_endTime.Value.ToShortDateString();

            DataTable dt = m_serverPrintManagement.GetAgainPrintBillByTime(start, end);

            if (dt.Rows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("查找不到指定日期范围内的单据信息！");
            }

            dgvShowInfo.DataSource = dt;
            m_bFlag = true;
        }

        private void dgvShowInfo_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                e.RowBounds.Location.Y,
                dgvShowInfo.RowHeadersWidth - 4,
                e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                dgvShowInfo.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dgvShowInfo.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private void 重新打印_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        private void dgvShowInfo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            txtBill_ID.Text = dgvShowInfo.Rows[e.RowIndex].Cells["单据号"].Value.ToString();

            if (m_bFlag)
            {
                txtPrintRemark.Text = dgvShowInfo.Rows[e.RowIndex].Cells["重新打印原因"].Value.ToString();
            }
        }

        /// <summary>
        /// 单据编制本人删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlsbDelete_Click(object sender, EventArgs e)
        {
            if (dgvShowInfo.CurrentRow == null)
            {
                return;
            }

            if (dgvShowInfo.CurrentRow.Cells["申请人姓名"].Value.ToString().Equals(BasicInfo.LoginName))
            {
                string strID = dgvShowInfo.CurrentRow.Cells["序号"].Value.ToString();

                if (strID == null || strID == "")
                {
                    MessageDialog.ShowPromptMessage("请选中需要删除的记录");
                    return;
                }
                else
                {
                    if (!m_serverPrintManagement.Del_S_AgainPrintBillTable(Convert.ToInt32(strID), out m_strErr))
                    {
                        MessageDialog.ShowPromptMessage(m_strErr);
                    }
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("您不是申请人，不能删除别人的单据！");
            }

            DataBind();
        }

        private void dgvShowInfo_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dgvShowInfo.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvShowInfo.Rows)
                {
                    if (row.Cells["审核状态"].Value.ToString() == "False" || row.Cells["财务批准"].Value.ToString() == "False")
                    {
                        row.DefaultCellStyle.ForeColor = Color.Red;
                    }
                }
            }
        }
    }
}
