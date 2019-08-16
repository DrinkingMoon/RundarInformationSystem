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
    public partial class 非产品件检验单 : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr = "";

        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 非产品件检验单服务组件
        /// </summary>
        IUnProductTestingSingle m_serverUnProductTesting = ServerModuleFactory.GetServerModule<IUnProductTestingSingle>();

        public 非产品件检验单()
        {
            InitializeComponent();

            m_billMessageServer.BillType = "非产品件检验单";

            m_billNoControl = new BillNumberControl(CE_BillTypeEnum.非产品件检验单, m_serverUnProductTesting);

            string[] strBillStatus = { "全部", 
                                     "新建单据", 
                                     "等待检验要求",
                                     "等待检验",
                                     "等待验证要求",
                                     "等待验证",
                                     "等待最终判定",
                                     "单据已完成"};

            checkBillDateAndStatus1.InsertComBox(strBillStatus);

            checkBillDateAndStatus1.dtpStartTime.Value = ServerTime.Time.AddDays(1).AddMonths(-1);
            checkBillDateAndStatus1.dtpEndTime.Value = ServerTime.Time.AddDays(1);
            RefreshData();
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
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
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

                    DataTable dtMessage = UniversalFunction.PositioningOneRecord(msg.MessageContent, "非产品件检验单");

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

        /// <summary>
        /// 刷新数据
        /// </summary>
        void RefreshData()
        {
            dataGridView1.DataSource = m_serverUnProductTesting.GetAllInfo(checkBillDateAndStatus1.dtpStartTime.Value, 
                checkBillDateAndStatus1.dtpEndTime.Value, checkBillDateAndStatus1.cmbBillStatus.Text);

            userControlDataLocalizer1.Init(dataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
        }

        /// <summary>
        /// 提交单据流程
        /// </summary>
        /// <returns>成功返回True,失败返回False</returns>
        bool FlowBill()
        {
            MessageDialog.ShowPromptMessage("请选择下一步流程需要知会或操作的人员");

            FormSelectPersonnel2 frm = new FormSelectPersonnel2();
            frm.ShowDialog();

            if (frm.ShowDialog() != DialogResult.OK)
            {
                return false;
            }

            List<string> lis = new List<string>();

            foreach (PersonnelBasicInfo pbi in frm.SelectedNotifyPersonnelInfo.PersonnelBasicInfoList)
            {
                if (pbi.工号 != null && pbi.工号.Length > 0)
                {
                    lis.Add(pbi.工号);
                }
            }

            string strBillNo = dataGridView1.CurrentRow.Cells["单据号"].Value.ToString();
            string strBillStatus = dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString();

            if (!m_serverUnProductTesting.FlowBill(strBillNo,out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
                return false;
            }
            else
            {
                switch (strBillStatus)
                {
                    case "新建单据":
                        m_billMessageServer.SendNewFlowMessage(strBillNo, string.Format("{0} 号非产品件检验单单已提交，请处理", strBillNo),
                            BillFlowMessage_ReceivedUserType.用户, lis);
                        break;
                    case "等待检验要求":
                        m_billMessageServer.PassFlowMessage(strBillNo, string.Format("{0} 号非产品件检验单单已提交检验要求，请处理", strBillNo),
                            BillFlowMessage_ReceivedUserType.用户, lis);
                        break;
                    case "等待检验":
                        m_billMessageServer.PassFlowMessage(strBillNo, string.Format("{0} 号非产品件检验单单已检验，请处理", strBillNo),
                            BillFlowMessage_ReceivedUserType.用户, lis);
                        break;
                    case "等待验证要求":
                        m_billMessageServer.PassFlowMessage(strBillNo, string.Format("{0} 号非产品件检验单单已提交验证要求，请处理", strBillNo),
                            BillFlowMessage_ReceivedUserType.用户, lis);
                        break;
                    case "等待验证":
                        m_billMessageServer.PassFlowMessage(strBillNo, string.Format("{0} 号非产品件检验单单已验证，请处理", strBillNo),
                            BillFlowMessage_ReceivedUserType.用户, lis);
                        break;
                    case "等待最终判定":
                        m_billMessageServer.EndFlowMessage(strBillNo, string.Format("{0} 号非产品件检验单单已完成", strBillNo),
                            null, lis);
                        break;
                    default:
                        break;
                }

                MessageDialog.ShowPromptMessage("操作成功");

                RefreshData();
                PositioningRecord(strBillNo);
                return true;
            }
        }

        private void checkBillDateAndStatus1_OnCompleteSearch()
        {
            RefreshData();
        }

        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            string strBillNo = dataGridView1.CurrentRow.Cells["单据号"].Value.ToString();

            非产品件检验单明细 frm = new 非产品件检验单明细(strBillNo);
            frm.ShowDialog();

            if (frm.BlSaveFlag)
            {
                RefreshData();
                PositioningRecord(frm.StrBillNo);
            }
        }

        private void 新建单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            非产品件检验单明细 frm = new 非产品件检验单明细(null);
            frm.ShowDialog();

            if (frm.BlSaveFlag)
            {
                RefreshData();
                PositioningRecord(frm.StrBillNo);
            }
        }

        private void 提交单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() != "新建单据")
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
                return;
            }
            else
            {
                FlowBill();
            }
        }

        private void 提交检验要求ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() != "等待检验要求")
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
                return;
            }
            else
            {
                FlowBill();
            }
        }

        private void 提交验证要求ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() != "等待验证要求")
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
                return;
            }
            else
            {
                FlowBill();
            }
        }

        private void 提交检验结果ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() != "等待检验")
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
                return;
            }
            else
            {
                FlowBill();
            }
        }

        private void 提交验证结果ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() != "等待验证")
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
                return;
            }
            else
            {
                FlowBill();
            }
        }

        private void 提交最终判定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "单据已完成")
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
                return;
            }
            else
            {
                FlowBill();
            }
        }

        private void 删除单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "单据已完成")
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
                return;
            }

            if (dataGridView1.CurrentRow.Cells["申请人"].Value.ToString() != BasicInfo.LoginName)
            {
                MessageDialog.ShowPromptMessage("删除单据需由申请人工号进行此操作");
                return;
            }
            
            string strBillNo = dataGridView1.CurrentRow.Cells["单据号"].Value.ToString();

            if (MessageDialog.ShowEnquiryMessage("是否要删除【"+strBillNo+"】号单据?") == DialogResult.Yes)
            {
                m_serverUnProductTesting.DeleteBill(strBillNo);
                m_billNoControl.CancelBill(strBillNo);
                m_billMessageServer.DestroyMessage(strBillNo);

                MessageDialog.ShowPromptMessage("删除成功");

                RefreshData();
            }
        }
    }
}
