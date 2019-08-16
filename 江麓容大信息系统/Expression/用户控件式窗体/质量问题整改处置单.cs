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
    /// 质量问题整改处置单界面
    /// </summary>
    public partial class 质量问题整改处置单 : Form
    {
        /// <summary>
        /// 人员服务组件
        /// </summary>
        IPersonnelInfoServer m_serverPersonnel = ServerModuleFactory.GetServerModule<IPersonnelInfoServer>();

        /// <summary>
        /// 部门服务组件
        /// </summary>
        IDepartmentServer m_serverDepartment = ServerModuleFactory.GetServerModule<IDepartmentServer>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr;

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 可供查找的所有字段
        /// </summary>
        string[] m_findField = null;

        /// <summary>
        /// 数据集
        /// </summary>
        ZL_QualityProblemRectificationDisposalBill m_lnqQuality = new ZL_QualityProblemRectificationDisposalBill();

        /// <summary>
        /// 质量问题整改服务组件
        /// </summary>
        IQualityProblemRectificationDisposalBill m_serverQualityProblem =
            ServerModuleFactory.GetServerModule<IQualityProblemRectificationDisposalBill>();

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag = AuthorityFlag.Nothing;

        public 质量问题整改处置单(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_billMessageServer.BillType = "质量问题整改处置单";

            m_authFlag = nodeInfo.Authority;

            string[] strBillStatus = { "全  部", 
                                     "新建单据",
                                     "等待发起部门确认",
                                     "等待分析判定",
                                     "等待分析判定确认",
                                     "等待整改措施",
                                     "等待整改措施确认",
                                     "等待效果确认",
                                     "等待最终确认",
                                     "已完成"};

            checkBillDateAndStatus1.InsertComBox(strBillStatus);

            RefreshDataGirdView(m_serverQualityProblem.GetAllBill(checkBillDateAndStatus1.dtpStartTime.Value,
                checkBillDateAndStatus1.dtpEndTime.Value, checkBillDateAndStatus1.cmbBillStatus.Text));
        }

        private void 质量问题整改处置单_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authFlag);
        }

        /// <summary>
        /// 权限控制
        /// </summary>
        /// <param name="authorityFlag">权限标志</param>
        void AuthorityControl(PlatformManagement.AuthorityFlag authorityFlag)
        {
            FaceAuthoritySetting.SetVisibly(menuStrip1, authorityFlag);
            FaceAuthoritySetting.SetEnable(this.Controls, authorityFlag);
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
                    m_serverQualityProblem.DeleteExcessSchedule(out m_strErr);
                    // 放弃未使用的单据号
                    break;

                case WndMsgSender.PositioningMsg:
                    WndMsgData msg = new WndMsgData();      //这是创建自定义信息的结构
                    Type dataType = msg.GetType();

                    msg = (WndMsgData)m.GetLParam(dataType);//这里获取的就是作为LParam参数发送来的信息的结构

                    DataTable dtMessage = UniversalFunction.PositioningOneRecord(msg.MessageContent, "质量问题整改处置单");

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
        /// <param name="source">数据集</param>
        void RefreshDataGirdView(DataTable source)
        {
            dataGridView1.DataSource = source;

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
        /// 提交数据
        /// </summary>
        bool UpdateBill()
        {
            if (m_lnqQuality.BillStatus != "已完成")
            {
                if (m_lnqQuality == null)
                {
                    m_lnqQuality.Bill_ID = dataGridView1.CurrentRow.Cells["单据号"].Value.ToString();
                    m_lnqQuality.BillStatus = dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString();
                }

                if (!m_serverQualityProblem.UpdateBill(m_lnqQuality.Bill_ID, out m_strErr))
                {
                    MessageDialog.ShowErrorMessage(m_strErr);
                    return false;
                }
                else
                {
                    MessageBox.Show("成功提交", "提示");
                }
            }
            else
            {
                MessageBox.Show("请重新确认单据状态", "提示");
                return false;
            }

            RefreshDataGirdView(m_serverQualityProblem.GetAllBill(checkBillDateAndStatus1.dtpStartTime.Value,
                checkBillDateAndStatus1.dtpEndTime.Value, checkBillDateAndStatus1.cmbBillStatus.Text));

            PositioningRecord(m_lnqQuality.Bill_ID);

            return true;
        }

        private void 新建单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_lnqQuality = null;

            质量问题整改处置明细 form = new 质量问题整改处置明细("");
            form.ShowDialog();

            m_lnqQuality = form.LnqQualityProblem;

            RefreshDataGirdView(m_serverQualityProblem.GetAllBill(checkBillDateAndStatus1.dtpStartTime.Value,
                checkBillDateAndStatus1.dtpEndTime.Value, checkBillDateAndStatus1.cmbBillStatus.Text));

            PositioningRecord(m_lnqQuality.Bill_ID);
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                return;
            }

            if (dataGridView1.CurrentRow.Cells["单据号"].Value.ToString() == "")
            {
                return;
            }

            m_lnqQuality = null;

            质量问题整改处置明细 form = new 质量问题整改处置明细(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString());
            form.ShowDialog();
            m_lnqQuality = form.LnqQualityProblem;

            RefreshDataGirdView(m_serverQualityProblem.GetAllBill(checkBillDateAndStatus1.dtpStartTime.Value,
                checkBillDateAndStatus1.dtpEndTime.Value, checkBillDateAndStatus1.cmbBillStatus.Text));
            PositioningRecord(m_lnqQuality.Bill_ID);
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }
            else
            {
                if (dataGridView1.CurrentRow.Cells["编制人"].Value.ToString() == BasicInfo.LoginName)
                {
                    提交最终确认ToolStripMenuItem.Visible = true;
                }
                else
                {
                    提交最终确认ToolStripMenuItem.Visible = false;
                }

                IQueryable<View_HR_Personnel> IQViewHrPersonnel = m_serverPersonnel.GetFuzzyDeptDirector(
                    m_serverDepartment.GetDeptInfoFromPersonnelInfo(
                    dataGridView1.CurrentRow.Cells["编制人"].Value.ToString()).Rows[0]["DepartmentCode"].ToString());

                确认发起单据ToolStripMenuItem.Visible = false;

                foreach (var item in IQViewHrPersonnel)
                {
                    if (item.工号 == BasicInfo.LoginID)
                    {
                        确认发起单据ToolStripMenuItem.Visible = true;
                    }
                }

                指定责任人ToolStripMenuItem.Visible = false;
                提交整改措施信息ToolStripMenuItem.Visible = false;
                确认整改措施信息ToolStripMenuItem.Visible = false;

                if (dataGridView1.CurrentRow.Cells["责任人"].Value.ToString() == BasicInfo.LoginName)
                {
                    提交整改措施信息ToolStripMenuItem.Visible = true;
                }

                IQViewHrPersonnel = m_serverPersonnel.GetFuzzyDeptDirector(
                    m_serverDepartment.GetDepartmentCode(dataGridView1.CurrentRow.Cells["责任部门"].Value.ToString()));

                foreach (var item in IQViewHrPersonnel)
                {
                    if (item.工号 == BasicInfo.LoginID)
                    {
                        确认整改措施信息ToolStripMenuItem.Visible = true;
                        指定责任人ToolStripMenuItem.Visible = true;
                    }
                }
            }
        }

        private void checkBillDateAndStatus1_OnCompleteSearch()
        {
            RefreshDataGirdView(m_serverQualityProblem.GetAllBill(checkBillDateAndStatus1.dtpStartTime.Value,
                checkBillDateAndStatus1.dtpEndTime.Value, checkBillDateAndStatus1.cmbBillStatus.Text));
        }

        private void 确认发起单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "等待发起部门确认")
            {
                if (!UpdateBill())
                {
                    return;
                }

                string msg = string.Format("{0} 号质量问题整改处置单已由发起部门确认完毕，请质量工程师分析判定", m_lnqQuality.Bill_ID);
                m_billMessageServer.PassFlowMessage(m_lnqQuality.Bill_ID, msg, CE_RoleEnum.质量工程师.ToString(), true);
            }
            else
            {
                MessageDialog.ShowErrorMessage("请重新确认单据状态");
            }
        }

        private void 提交分析判定结果ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "等待分析判定")
            {
                if (!UpdateBill())
                {
                    return;
                }

                string msg = string.Format("{0} 号质量问题整改处置单已分析判定完毕，请质量部门确认", m_lnqQuality.Bill_ID);
                m_billMessageServer.PassFlowMessage(m_lnqQuality.Bill_ID, msg, CE_RoleEnum.质量工程师.ToString(), true);
            }
            else
            {
                MessageDialog.ShowErrorMessage("请重新确认单据状态");
            }
        }

        private void 确认分析判定结果ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "等待分析判定确认")
            {
                if (!UpdateBill())
                {
                    return;
                }

                string msg = string.Format("{0} 号质量问题整改处置单已分析判定确认完毕，请责任部门领导指定责任人", m_lnqQuality.Bill_ID);
                IQueryable<View_HR_Personnel> IQViewPersonnel =
                    m_serverPersonnel.GetDeptDirector(m_serverDepartment.GetDepartmentCode(m_lnqQuality.RelevantDepartment));
                List<string> lisStr = new List<string>();

                foreach (View_HR_Personnel item in IQViewPersonnel)
                {
                    lisStr.Add(item.工号);
                }

                m_billMessageServer.PassFlowMessage(m_lnqQuality.Bill_ID, msg, BillFlowMessage_ReceivedUserType.用户, lisStr);
            }
            else
            {
                MessageDialog.ShowErrorMessage("请重新确认单据状态");
            }
        }

        private void 提交整改措施信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "等待整改措施")
            {
                if (!UpdateBill())
                {
                    return;
                }

                string msg = string.Format("{0} 号质量问题整改处置单已提交整改措施，请责任部门确认整改措施", m_lnqQuality.Bill_ID);
                View_HR_Personnel lnqViewPersonnel =
                    m_serverPersonnel.GetDeptDirector(m_serverDepartment.GetDepartmentCode(m_lnqQuality.RelevantDepartment)).First();
                m_billMessageServer.PassFlowMessage(m_lnqQuality.Bill_ID, msg, lnqViewPersonnel.工号, false);
            }
            else
            {
                MessageDialog.ShowErrorMessage("请重新确认单据状态");
            }
        }

        private void 确认整改措施信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "等待整改措施确认")
            {
                if (!UpdateBill())
                {
                    return;
                }

                string msg = string.Format("{0} 号质量问题整改处置单已确认整改措施，请质量工程师确认效果", m_lnqQuality.Bill_ID);
                m_billMessageServer.PassFlowMessage(m_lnqQuality.Bill_ID, msg, CE_RoleEnum.质量工程师.ToString(), true);
            }
            else
            {
                MessageDialog.ShowErrorMessage("请重新确认单据状态");
            }
        }

        private void 提交效果确认信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "等待效果确认")
            {
                if (!UpdateBill())
                {
                    return;
                }

                string msg = string.Format("{0} 号质量问题整改处置单已确认效果，请编制人最终确认", m_lnqQuality.Bill_ID);
                m_billMessageServer.PassFlowMessage(m_lnqQuality.Bill_ID, msg,
                    m_serverPersonnel.GetPersonnelViewInfo(ServerModule.PersonnelDefiniens.ParameterType.姓名,
                    m_lnqQuality.HappenFillInPersonnel).First().工号, false);
            }
            else
            {
                MessageDialog.ShowErrorMessage("请重新确认单据状态");
            }
        }

        private void 提交确认结果ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "等待最终确认")
            {
                if (!UpdateBill())
                {
                    return;
                }

                #region 发送知会消息

                List<string> noticeRoles = new List<string>();

                noticeRoles.Add(CE_RoleEnum.质量工程师.ToString());

                m_billMessageServer.EndFlowMessage(m_lnqQuality.Bill_ID,
                    string.Format("{0} 号不合格品隔离处置单已经处理完毕", m_lnqQuality.Bill_ID),
                    noticeRoles, null);

                #endregion 发送知会消息
            }
            else
            {
                MessageDialog.ShowErrorMessage("请重新确认单据状态");
            }
        }

        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshDataGirdView(m_serverQualityProblem.GetAllBill(checkBillDateAndStatus1.dtpStartTime.Value,
                checkBillDateAndStatus1.dtpEndTime.Value, checkBillDateAndStatus1.cmbBillStatus.Text));
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

        private void 删除单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells["编制人"].Value.ToString() != BasicInfo.LoginName)
            {
                MessageDialog.ShowPromptMessage("不能对编制人不是本人的单据进行此操作");
                return;
            }
            else
            {
                if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "已完成")
                {
                    MessageDialog.ShowPromptMessage("不能删除已完成的单据");
                }

                if (MessageDialog.ShowEnquiryMessage("您是否要删除此单据") == DialogResult.No)
                {
                    return;
                }

                if (!m_serverQualityProblem.DeleteBill(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString(), out m_strErr))
                {
                    MessageDialog.ShowPromptMessage(m_strErr);
                }
                else
                {
                    m_billMessageServer.DestroyMessage(m_lnqQuality.Bill_ID);
                    MessageDialog.ShowPromptMessage("删除成功");
                }

                RefreshDataGirdView(m_serverQualityProblem.GetAllBill(
                        checkBillDateAndStatus1.dtpStartTime.Value, checkBillDateAndStatus1.dtpEndTime.Value, checkBillDateAndStatus1.cmbBillStatus.Text));
            }
        }

        private void 指定责任人ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "等待指定责任人")
            {
                if (!UpdateBill())
                {
                    return;
                }

                string msg = string.Format("{0} 号质量问题整改处置单已指定责任人，请责任人提出整改措施", m_lnqQuality.Bill_ID);

                List<string> lisStr = new List<string>();
                lisStr.Add(m_serverPersonnel.GetPersonnelInfoFromName(m_serverDepartment.GetDepartmentCode(m_lnqQuality.RelevantDepartment),
                    m_lnqQuality.Responsible).工号);

                m_billMessageServer.PassFlowMessage(m_lnqQuality.Bill_ID, msg, BillFlowMessage_ReceivedUserType.用户, lisStr);
            }
            else
            {
                MessageDialog.ShowErrorMessage("请重新确认单据状态");
            }
        }

        private void 提交整改处置单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "新建单据")
            {
                if (!UpdateBill())
                {
                    return;
                }

                m_billMessageServer.DestroyMessage(m_lnqQuality.Bill_ID);
                m_billMessageServer.SendNewFlowMessage(m_lnqQuality.Bill_ID, string.Format("{0} 号质量问题整改处置单，请发起部门确认", m_lnqQuality.Bill_ID),
                    BillFlowMessage_ReceivedUserType.角色,
                    m_billMessageServer.GetSuperior(CE_RoleStyleType.上级领导, BasicInfo.LoginID));

                RefreshDataGirdView(m_serverQualityProblem.GetAllBill(
                        checkBillDateAndStatus1.dtpStartTime.Value, checkBillDateAndStatus1.dtpEndTime.Value, checkBillDateAndStatus1.cmbBillStatus.Text));

                PositioningRecord(m_lnqQuality.Bill_ID);
            }
            else
            {
                MessageDialog.ShowErrorMessage("请重新确认单据状态");
            }
        }
    }
}
