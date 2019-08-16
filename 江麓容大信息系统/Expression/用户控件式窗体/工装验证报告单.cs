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
    /// 工装验证报告单界面
    /// </summary>
    public partial class 工装验证报告单 : Form
    {
        /// <summary>
        /// 工装台帐服务组件
        /// </summary>
        IFrockStandingBook m_serverFrockBook = PMS_ServerFactory.GetServerModule<IFrockStandingBook>();

        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err = "";

        /// <summary>
        /// 服务组件
        /// </summary>
        IFrockProvingReport m_serverFrock = ServerModuleFactory.GetServerModule<IFrockProvingReport>();

        /// <summary>
        /// 数据集
        /// </summary>
        S_FrockProvingReport m_lnqFrock;

        /// <summary>
        /// 附属表
        /// </summary>
        DataTable m_dtAttached;

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_msgPromulgator = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_msgPromulgatorBook = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        public 工装验证报告单(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_msgPromulgator.BillType = "工装验证报告单";

            m_msgPromulgatorBook.BillType = "工装台帐";

            m_authFlag = nodeInfo.Authority;

            m_billNoControl = new BillNumberControl(CE_BillTypeEnum.工装验证报告单, m_serverFrock);
            string[] strBillStatus = { "全  部", 
                                     "新建单据", 
                                     "等待检验要求",
                                     "等待检验",
                                     "等待验证要求",
                                     "等待验证",
                                     "等待结论",
                                     "等待最终审核",
                                     "单据已完成"};

            checkBillDateAndStatus1.InsertComBox(strBillStatus);

            checkBillDateAndStatus1.dtpStartTime.Value = ServerTime.Time.AddDays(1).AddMonths(-1);
            checkBillDateAndStatus1.dtpEndTime.Value = ServerTime.Time.AddDays(1);

            RefreshDataGirdView();
        }

        private void 工装验证报告单_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authFlag);

            验证员操作ToolStripMenuItem.Visible = false;
            提交验证要求ToolStripMenuItem.Visible = false;
        }

        void checkBillDateAndStatus1_OnCompleteSearch()
        {
            RefreshDataGirdView();
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
                    // 放弃未使用的单据号
                    m_billNoControl.CancelBill();
                    break;

                case WndMsgSender.PositioningMsg:
                    WndMsgData msg = new WndMsgData();      //这是创建自定义信息的结构
                    Type dataType = msg.GetType();

                    msg = (WndMsgData)m.GetLParam(dataType);//这里获取的就是作为LParam参数发送来的信息的结构

                    DataTable dtMessage = UniversalFunction.PositioningOneRecord(msg.MessageContent, "工装验证报告单");

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
        /// 刷新
        /// </summary>
        void RefreshDataGirdView()
        {
            dataGridView1.DataSource = m_serverFrock.GetBill(checkBillDateAndStatus1.cmbBillStatus.Text, 
                checkBillDateAndStatus1.dtpStartTime.Value, checkBillDateAndStatus1.dtpEndTime.Value);

            userControlDataLocalizer1.Init(dataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
        }

        /// <summary>
        /// 获得子窗体的所有信息
        /// </summary>
        /// <param name="djh">单据号</param>
        bool GetMessage(string djh)
        {
            工装验证报告 form = new 工装验证报告(djh);
            form.ShowDialog();

            m_dtAttached = form.DtAttached;
            m_lnqFrock = form.LnqFrock;

            if (m_lnqFrock == null)
            {
                return false;
            }

            PositioningRecord(m_lnqFrock.DJH);
            return true;
        }

        /// <summary>
        /// 更新信息
        /// </summary>
        /// <param name="djzt">单据状态</param>
        /// <returns>操作成功返回True，否则返回False</returns>
        bool UpdateMessage(string djzt)
        {
            if (!m_serverFrock.UpdateBill(m_lnqFrock.DJH, djzt, out m_err))
            {
                MessageDialog.ShowPromptMessage(m_err);
                return false;
            }
            else
            {
                MessageDialog.ShowPromptMessage("提交成功");
                return true;
            }
        }

        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshDataGirdView();
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            GetMessage(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString());
        }

        private void 新建单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!GetMessage(""))
            {
                return;
            }

            if (!m_serverFrock.AddBill(m_lnqFrock, m_dtAttached, out m_err))
            {
                MessageDialog.ShowPromptMessage(m_err);
                return;
            }
            else
            {
                m_msgPromulgator.DestroyMessage(m_lnqFrock.DJH);
                m_msgPromulgator.SendNewFlowMessage(m_lnqFrock.DJH,
                    string.Format("{0}  ※※※ 请【{1}】处理",
                    UniversalFunction.GetGoodsMessage(m_lnqFrock.GoodsID), CE_RoleEnum.计量工程师.ToString()),
                    BillFlowMessage_ReceivedUserType.角色, CE_RoleEnum.计量工程师.ToString());

                RefreshDataGirdView();
                PositioningRecord(m_lnqFrock.DJH);
            }
        }

        private void 提交检验要求ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_lnqFrock.DJZT == "新建单据" || m_lnqFrock.DJZT == "等待检验要求")
            {
                bool blflag = false;

                for (int i = 0; i < m_dtAttached.Rows.Count; i++)
                {
                    if (m_dtAttached.Rows[i]["AttachedType"].ToString() == "检验")
                    {
                        blflag = true;
                    }
                }

                if (!blflag)
                {
                    MessageDialog.ShowPromptMessage("请添加检验要求并保存后再提交");
                    return;
                }

                if (UpdateMessage("等待检验要求"))
                {

                    m_msgPromulgator.PassFlowMessage(m_lnqFrock.DJH,
                    string.Format("{0}  ※※※ 请【{1}】处理",
                    UniversalFunction.GetGoodsMessage(m_lnqFrock.GoodsID), CE_RoleEnum.计量工程师.ToString()), 
                    CE_RoleEnum.计量工程师.ToString(), true);

                    RefreshDataGirdView();
                    PositioningRecord(m_lnqFrock.DJH);
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
                return;
            }
        }

        private void 提交验证要求ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_lnqFrock.DJZT == "等待验证要求")
            {
                bool blflag = false;

                for (int i = 0; i < m_dtAttached.Rows.Count; i++)
                {
                    if (m_dtAttached.Rows[i]["AttachedType"].ToString() == "验证")
                    {
                        blflag = true;
                    }
                }

                if (!blflag)
                {
                    MessageDialog.ShowPromptMessage("请添加验证要求并保存后再提交");
                    return;
                }

                MessageDialog.ShowPromptMessage("请选择验证人员");

                FormSelectPersonnel2 frm = new FormSelectPersonnel2();

                if (frm.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                List<string> list = new List<string>();
                string lst = "";

                foreach (PersonnelBasicInfo pbi in frm.SelectedNotifyPersonnelInfo.PersonnelBasicInfoList)
                {
                    if (pbi.工号 != null && pbi.工号.Length > 0)
                    {
                        list.Add(pbi.工号);
                        lst += pbi.姓名;
                    }
                }

                if (UpdateMessage("等待验证要求"))
                {

                    m_msgPromulgator.PassFlowMessage(m_lnqFrock.DJH,
                    string.Format("{0}  ※※※ 请【{1}】处理",
                    UniversalFunction.GetGoodsMessage(m_lnqFrock.GoodsID), lst),
                    BillFlowMessage_ReceivedUserType.用户, list);

                    RefreshDataGirdView();

                    PositioningRecord(m_lnqFrock.DJH);
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
                return;
            }
        }

        private void 提交结论ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageDialog.ShowEnquiryMessage("您是否要提交结论") == DialogResult.No)
            {
                return;
            }

            if (m_lnqFrock.DJZT != "单据已完成")
            {
                if (m_lnqFrock.FinalVerdict.ToString().Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请填写结论");
                    return;
                }

                if (UpdateMessage("等待结论"))
                {
                    m_msgPromulgator.PassFlowMessage(m_lnqFrock.DJH,
                    string.Format("{0}  ※※※ 请【{1}】处理",
                    UniversalFunction.GetGoodsMessage(m_lnqFrock.GoodsID), CE_RoleEnum.工艺组长.ToString()), 
                    CE_RoleEnum.工艺组长.ToString(), true);

                    RefreshDataGirdView();
                    PositioningRecord(m_lnqFrock.DJH);
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
                return;
            }
        }

        private void 提交检验结果ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_lnqFrock.DJZT == "等待检验")
            {
                if (UpdateMessage("等待检验"))
                {
                    m_msgPromulgator.PassFlowMessage(m_lnqFrock.DJH,
                    string.Format("{0}  ※※※ 请【{1}】处理",
                    UniversalFunction.GetGoodsMessage(m_lnqFrock.GoodsID), CE_RoleEnum.工艺人员.ToString()), 
                    CE_RoleEnum.工艺人员.ToString(), true);

                    RefreshDataGirdView();
                    PositioningRecord(m_lnqFrock.DJH);
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
                return;
            }
        }

        private void 提交验证结果ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_lnqFrock.DJZT == "等待验证")
            {
                if (UpdateMessage("等待验证"))
                {
                    m_msgPromulgator.PassFlowMessage(m_lnqFrock.DJH,
                    string.Format("{0}  ※※※ 请【{1}】处理",
                    UniversalFunction.GetGoodsMessage(m_lnqFrock.GoodsID), CE_RoleEnum.工艺人员.ToString()), 
                    CE_RoleEnum.工艺人员.ToString(), true);

                    RefreshDataGirdView();
                    PositioningRecord(m_lnqFrock.DJH);
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
                return;
            }
        }

        private void 删除单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() != "单据已完成")
            {

                if (MessageDialog.ShowEnquiryMessage("您是否要删除此单据") == DialogResult.No)
                {
                    return;
                }

                if (!m_serverFrock.DeleteBill(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString(), out m_err))
                {
                    MessageDialog.ShowPromptMessage(m_err);
                    return;
                }
                else
                {
                    MessageDialog.ShowPromptMessage("删除成功");

                    m_billNoControl.CancelBill();
                    m_msgPromulgator.DestroyMessage(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString());
                    RefreshDataGirdView();
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
                return;
            }
        }

        private void 回退单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (工艺员操作ToolStripMenuItem.Visible == true
                && dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "等待检验要求")
            {
                ReturnBillStatus();
            }
            else if (检验员操作ToolStripMenuItem.Visible == true
                && dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "等待检验")
            {
                ReturnBillStatus();
            }
            //else if (工艺员操作ToolStripMenuItem.Visible == true
            //    && dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "等待验证要求")
            //{
            //    ReturnBillStatus();
            //}
            //else if (验证员操作ToolStripMenuItem.Visible == true
            //    && dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "等待验证")
            //{
            //    ReturnBillStatus();
            //}
            else if (工艺员操作ToolStripMenuItem.Visible == true
                && dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "等待结论")
            {
                ReturnBillStatus();
            }
            else if (工艺主管操作ToolStripMenuItem.Visible == true
                && dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "等待最终审核")
            {
                ReturnBillStatus();
            }
        }

        /// <summary>
        /// 回退单据
        /// </summary>
        private void ReturnBillStatus()
        {
            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() != "单据已完成")
            {
                回退单据 form = new 回退单据(CE_BillTypeEnum.工装验证报告单,
                    dataGridView1.CurrentRow.Cells["单据号"].Value.ToString(),
                    dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString());

                if (form.ShowDialog() == DialogResult.OK)
                {
                    if (MessageBox.Show("您确定要回退此单据吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        if (m_serverFrock.ReturnBill(form.StrBillID,
                            form.StrBillStatus, out m_err, form.Reason))
                        {
                            MessageDialog.ShowPromptMessage("回退成功");

                        }
                        else
                        {
                            MessageDialog.ShowPromptMessage(m_err);
                        }
                    }

                    RefreshDataGirdView();
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
            }
        }

        private void 打印报告ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            报表_工装验证报告单 report = new 报表_工装验证报告单(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString(), labelTitle.Text);
            PrintReportBill print = new PrintReportBill(21, 29.7, report);
            print.DirectPrintReport();
        }

        private void 导出EXCELToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
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

        private void 最终审核ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_lnqFrock.DJZT == "等待最终审核")
            {
                if (UpdateMessage("等待最终审核"))
                {
                    #region 发送知会消息

                    List<string> noticeRoles = new List<string>();
                    noticeRoles.Add(CE_RoleEnum.工艺人员.ToString());
                    noticeRoles.Add(CE_RoleEnum.制造信息员.ToString());
                    noticeRoles.Add(CE_RoleEnum.计量工程师.ToString());
                    noticeRoles.Add(CE_RoleEnum.采购员.ToString());

                    m_msgPromulgator.EndFlowMessage(m_lnqFrock.DJH,
                        string.Format("{0} 号工装验证报告单已经处理完毕", m_lnqFrock.DJH),
                        noticeRoles, null);

                    #endregion 发送知会消息


                    //销毁工装台帐的提醒消息
                    if (m_lnqFrock.BillType == "周期鉴定")
                    {
                        List<string> listRoles = new List<string>();

                        listRoles.Add(CE_RoleEnum.工艺人员.ToString());
                        listRoles.Add(CE_RoleEnum.装配用工装管理员.ToString());
                        listRoles.Add(CE_RoleEnum.机加用工装管理员.ToString());
                        listRoles.Add(CE_RoleEnum.计量工程师.ToString());

                        m_msgPromulgatorBook.EndFlowMessage(m_lnqFrock.FrockNumber,
                            string.Format("{0}号工装已完成周期鉴定", m_lnqFrock.FrockNumber), listRoles, null);
                    }


                    RefreshDataGirdView();
                    PositioningRecord(m_lnqFrock.DJH);
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
                return;
            }
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            bool visible = UniversalFunction.IsOperator(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString());

            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "等待验证" && visible)
            {
                验证员操作ToolStripMenuItem.Visible = true;
                提交验证结果ToolStripMenuItem.Visible = true;
            }
            else
            {
                验证员操作ToolStripMenuItem.Visible = false;
                提交验证结果ToolStripMenuItem.Visible = false;
            }

            if (dataGridView1.CurrentRow != null)
            {
                m_lnqFrock = m_serverFrock.GetBill(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString());
            }
        }
    }
}
