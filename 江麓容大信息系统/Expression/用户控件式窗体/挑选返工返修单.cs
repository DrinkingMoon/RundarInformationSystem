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
    /// 挑选返工返修单界面
    /// </summary>
    public partial class 挑选返工返修单 : Form
    {
        /// <summary>
        /// 报检入库服务组件
        /// </summary>
        ICheckOutInDepotServer m_serverCheckOutInDepot = ServerModuleFactory.GetServerModule<ICheckOutInDepotServer>();

        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 信息组
        /// </summary>
        S_CheckReturnRepairBill m_lnqReturn = new S_CheckReturnRepairBill();

        /// <summary>
        /// 可供查找的所有字段
        /// </summary>
        string[] m_findField = null;

        /// <summary>
        /// 查找条件窗体
        /// </summary>
        FormConditionFind m_formFindCondition;

        /// <summary>
        /// 数据定位控件
        /// </summary>
        UserControlDataLocalizer m_dataLocalizer;

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 服务类
        /// </summary>
        ICheckReturnRepair m_serverCheckReturnRepair = ServerModuleFactory.GetServerModule<ICheckReturnRepair>();

        /// <summary>
        /// 单据号
        /// </summary>
        string m_strDJH;

        /// <summary>
        /// SQE信息
        /// </summary>
        DataRow m_drSQE;

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_msgPromulgatorInDept = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_msgPromulgator = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        public 挑选返工返修单(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_msgPromulgator.BillType = CE_BillTypeEnum.挑选返工返修单.ToString();
            m_msgPromulgatorInDept.BillType = CE_BillTypeEnum.报检入库单.ToString();

            m_billNoControl = new BillNumberControl(CE_BillTypeEnum.挑选返工返修单, m_serverCheckReturnRepair);


            m_authFlag = nodeInfo.Authority;

            string[] strBillStatus = { "全  部", 
                                     "新建单据", 
                                     "等待处理结果",
                                     "等待审批确认",
                                     "等待检验结果",
                                     "单据已完成"};

            checkBillDateAndStatus1.InsertComBox(strBillStatus);

            checkBillDateAndStatus1.OnCompleteSearch +=
                new GlobalObject.DelegateCollection.NonArgumentHandle(checkBillDateAndStatus1_OnCompleteSearch);

            #region 被要求使用服务器时间 Modify by cjb on 2012.6.15
            checkBillDateAndStatus1.dtpStartTime.Value = ServerTime.Time.AddDays(1).AddMonths(-1);
            checkBillDateAndStatus1.dtpEndTime.Value = ServerTime.Time.AddDays(1);

            #endregion


            RefreshDataGirdView(m_serverCheckReturnRepair.GetAllBill(checkBillDateAndStatus1.cmbBillStatus.Text,
                checkBillDateAndStatus1.dtpStartTime.Value, checkBillDateAndStatus1.dtpEndTime.Value));

            ClearMessage();
        }

        private void 挑选返工返修单_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authFlag);
        }

        void checkBillDateAndStatus1_OnCompleteSearch()
        {
            RefreshDataGirdView(m_serverCheckReturnRepair.GetAllBill(checkBillDateAndStatus1.cmbBillStatus.Text,
                checkBillDateAndStatus1.dtpStartTime.Value, checkBillDateAndStatus1.dtpEndTime.Value));
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="source">数据集</param>
        void RefreshDataGirdView(DataTable source)
        {
            m_drSQE = source.NewRow();
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

            if (m_dataLocalizer == null)
            {
                m_dataLocalizer = new UserControlDataLocalizer(dataGridView1, this.Name,
                    UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
                panelPara.Controls.Add(m_dataLocalizer);
                m_dataLocalizer.Dock = DockStyle.Bottom;
            }
        }

        /// <summary>
        /// 权限控制
        /// </summary>
        /// <param name="authorityFlag">权限标志</param>
        void AuthorityControl(PlatformManagement.AuthorityFlag authorityFlag)
        {
            FaceAuthoritySetting.SetVisibly(menuStrip, authorityFlag);
            FaceAuthoritySetting.SetEnable(this.Controls, authorityFlag);
        }

        /// <summary>
        /// 获取数据集
        /// </summary>
        private void GetAllTheMaessage()
        {
            m_lnqReturn.DJH = lbDJH.Text;
            m_lnqReturn.InDepotBillID = txtBill_ID.Text;
            m_lnqReturn.SQE_Hour = NudSQE_TFGS.Value;
            m_lnqReturn.SQE_HGS = NudSQE_HG.Value;
            m_lnqReturn.SQE_BHGS = NudSQE_BHG.Value;
            m_lnqReturn.QC_BFS = NudQC_BF.Value;
            m_lnqReturn.QC_HGS = NudQC_HG.Value;
            m_lnqReturn.QC_RBS = NudQC_RB.Value;
            m_lnqReturn.QC_THS = NudQC_TH.Value;
            m_lnqReturn.DJZT = lbDJZT.Text;
            m_lnqReturn.ReturnReason = m_drSQE["挑返原因"].ToString();
            m_lnqReturn.ReturnMeansAndAsk = m_drSQE["挑返方法及要求"].ToString();
            m_lnqReturn.ReturnManHour = m_drSQE["挑返损耗及工时"].ToString();
            m_lnqReturn.CJRY = BasicInfo.LoginID;
            m_lnqReturn.SQERY = BasicInfo.LoginID;
            m_lnqReturn.SHRY = BasicInfo.LoginID;
            m_lnqReturn.SQEJGRY = BasicInfo.LoginID;
            m_lnqReturn.QCRY = BasicInfo.LoginID;
        }

        /// <summary>
        /// 操作数据库
        /// </summary>
        private void DataForMessage()
        {
            GetAllTheMaessage();

            if (m_serverCheckReturnRepair.UpdateBill(m_lnqReturn, out m_err))
            {
                if (m_lnqReturn.DJZT == "等待检验结果")
                {
                    m_billNoControl.UseBill(m_lnqReturn.DJH);
                }
                MessageBox.Show("提交成功！", "提示");
            }
            else
            {
                MessageDialog.ShowErrorMessage(m_err);
            }

            RefreshDataGirdView(m_serverCheckReturnRepair.GetAllBill(checkBillDateAndStatus1.cmbBillStatus.Text,
                checkBillDateAndStatus1.dtpStartTime.Value, checkBillDateAndStatus1.dtpEndTime.Value));
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        private void ClearMessage()
        {
            lbDJZT.Text = "";
            lbDJH.Text = "";
            lbCLJG.Text = "";
            lbSH.Text = "";
            lbJYJG.Text = "";
            lbCJ.Text = "";
            lbBZ.Text = "";

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

                    DataTable dtMessage = UniversalFunction.PositioningOneRecord(msg.MessageContent, "挑选返工返修单");

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

        private void 编制单据提交ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbDJZT.Text == "新建单据")
            {
                GetAllTheMaessage();

                if (m_lnqReturn.ReturnReason.ToString().Trim() == ""
                    || m_lnqReturn.ReturnManHour.ToString().Trim() == ""
                    || m_lnqReturn.ReturnMeansAndAsk.ToString().Trim() == "")
                {
                    MessageBox.Show("请完整填写挑返信息", "提示");
                    return;
                }

                if (m_serverCheckReturnRepair.SubmitBill(m_lnqReturn, out m_err))
                {
                    MessageBox.Show("提交成功！", "提示");

                    View_S_CheckOutInDepotBill bill = m_serverCheckOutInDepot.GetBill(m_lnqReturn.InDepotBillID);

                    m_msgPromulgator.PassFlowMessage(m_lnqReturn.DJH,
                        string.Format("【入库单号】：{0} 【图号型号】：{1} 【物品名称】：{2} 【规格】：{3}【供货单位】：{4}【批次号】：{5}  ※※※ 等待【{6}】处理",
                        bill.入库单号, bill.图号型号, bill.物品名称, bill.规格, bill.供货单位, bill.批次号, CE_RoleEnum.采购员.ToString()),
                        CE_RoleEnum.采购员.ToString(), true);
                }
                else
                {
                    MessageDialog.ShowErrorMessage(m_err);
                }

                RefreshDataGirdView(m_serverCheckReturnRepair.GetAllBill(checkBillDateAndStatus1.cmbBillStatus.Text,
                checkBillDateAndStatus1.dtpStartTime.Value, checkBillDateAndStatus1.dtpEndTime.Value));
            }
            else
            {
                MessageBox.Show("请重新确认单据状态", "提示");
            }

            PositioningRecord(m_lnqReturn.DJH);
        }

        private void 处理结果ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbDJZT.Text == "等待审批确认")
            {
                DataForMessage();
                View_S_CheckOutInDepotBill bill = m_serverCheckOutInDepot.GetBill(m_lnqReturn.InDepotBillID);

                m_msgPromulgator.PassFlowMessage(m_lnqReturn.DJH,
                    string.Format("【入库单号】：{0} 【图号型号】：{1} 【物品名称】：{2} 【规格】：{3}【供货单位】：{4}【批次号】：{5}  ※※※ 等待【{6}】处理",
                    bill.入库单号, bill.图号型号, bill.物品名称, bill.规格, bill.供货单位, bill.批次号, CE_RoleEnum.检验员.ToString()),
                    CE_RoleEnum.检验员.ToString(), true);
            }
            else
            {
                MessageBox.Show("请重新确认单据状态", "提示");
            }

            PositioningRecord(m_lnqReturn.DJH);
        }

        private void 单据审批确认ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbDJZT.Text == "等待处理结果")
            {
                if (Convert.ToDecimal(txtDeclareCount.Text) !=
                    (decimal)NudSQE_BHG.Value
                    + (decimal)NudSQE_HG.Value)
                {
                    MessageBox.Show("【合格数 + 不合格数 = 挑返数】 请重新确认", "提示");
                    return;
                }

                DataForMessage();
                View_S_CheckOutInDepotBill bill = m_serverCheckOutInDepot.GetBill(m_lnqReturn.InDepotBillID);

                m_msgPromulgator.PassFlowMessage(m_lnqReturn.DJH,
                    string.Format("【入库单号】：{0} 【图号型号】：{1} 【物品名称】：{2} 【规格】：{3}【供货单位】：{4}【批次号】：{5}  ※※※ 等待【{6}】处理",
                    bill.入库单号, bill.图号型号, bill.物品名称, bill.规格, bill.供货单位, bill.批次号, CE_RoleEnum.SQE组员.ToString()),
                    CE_RoleEnum.SQE组员.ToString(), true);

            }
            else
            {
                MessageBox.Show("请重新确认单据状态", "提示");
            }

            PositioningRecord(m_lnqReturn.DJH);
        }

        private void 检验结果提交ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbDJZT.Text == "等待检验结果")
            {
                if (Convert.ToDecimal(txtDeclareCount.Text) !=
                    (decimal)NudQC_BF.Value
                    + (decimal)NudQC_HG.Value
                    + (decimal)NudQC_RB.Value
                    + (decimal)NudQC_TH.Value)
                {
                    MessageBox.Show("【合格数 + 让步数 + 退货数 + 报废数 = 挑返数】 请重新确认", "提示");
                    return;
                }

                View_S_CheckOutInDepotBill bill = m_serverCheckOutInDepot.GetBill(m_lnqReturn.InDepotBillID);
                DataForMessage();

                #region 发送知会消息

                List<string> noticeRoles = new List<string>();
                noticeRoles.Add(CE_RoleEnum.检验员.ToString());
                noticeRoles.Add(CE_RoleEnum.采购员.ToString());
                noticeRoles.Add(CE_RoleEnum.SQE组员.ToString());

                m_msgPromulgator.EndFlowMessage(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString(),
                    string.Format("{0} 号挑选返工返修单已经处理完毕",
                    dataGridView1.CurrentRow.Cells["单据号"].Value.ToString()),
                    noticeRoles, null);

                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("{0} 号报检入库单已经进行挑返, 等待仓库入库, ", txtBill_ID.Text);
                sb.AppendFormat("此单据涉及物品：(图号[{0}]，名称[{1}]，规格[{2}]", txtCode.Text, txtName.Text, txtSpec.Text);

                m_msgPromulgatorInDept.PassFlowMessage(m_lnqReturn.InDepotBillID, sb.ToString(),
                        m_msgPromulgator.GetRoleStringForStorage(bill.库房代码).ToString(), true);

                #endregion 发送知会消息
            }
            else
            {
                MessageBox.Show("请重新确认单据状态", "提示");
            }

            PositioningRecord(m_lnqReturn.DJH);
        }

        private void btnWrite_Click(object sender, EventArgs e)
        {
            信息填写栏 form = new 信息填写栏(lbDJH.Text);
            form.ShowDialog();

            if (form.BlFlag)
            {
                m_drSQE = form.DrMasssage;
            }
        }

        private void 报废单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells["单据号"].Value.ToString() == "")
            {
                MessageDialog.ShowPromptMessage("请选择需要删除的记录");
                return;
            }

            string strDJZT = UniversalFunction.GetBillStatus("S_CheckReturnRepairBill", "DJZT", "DJH",
                dataGridView1.CurrentRow.Cells["单据号"].Value.ToString());

            if (strDJZT != "单据已完成")
            {
                DialogResult dr = MessageBox.Show("报检单回退至（质检机）点（是）,回退至（质检电）点（否）", "提示",
                    MessageBoxButtons.YesNoCancel);

                GetAllTheMaessage();

                if (MessageDialog.ShowEnquiryMessage("您是否要删除此单据") == DialogResult.No)
                {
                    return;
                }

                if (m_serverCheckReturnRepair.ScrapBill(m_lnqReturn, Convert.ToBoolean(dr), out m_err))
                {
                    m_billNoControl.CancelBill(m_lnqReturn.DJH);
                    MessageBox.Show("报废成功", "提示");
                    m_msgPromulgator.DestroyMessage(m_lnqReturn.DJH);
                }
                else
                {
                    MessageDialog.ShowErrorMessage(m_err);
                }


                RefreshDataGirdView(m_serverCheckReturnRepair.GetAllBill(checkBillDateAndStatus1.cmbBillStatus.Text,
                checkBillDateAndStatus1.dtpStartTime.Value, checkBillDateAndStatus1.dtpEndTime.Value));

            }

            PositioningRecord(m_lnqReturn.DJH);
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.RowCount == 0)
            {
                return;
            }

            #region 挑返单信息
            lbCJ.Text = dataGridView1.CurrentRow.Cells["创建人"].Value.ToString();
            lbBZ.Text = dataGridView1.CurrentRow.Cells["编制人"].Value.ToString();
            lbSH.Text = dataGridView1.CurrentRow.Cells["审核人"].Value.ToString();
            lbCLJG.Text = dataGridView1.CurrentRow.Cells["处理人"].Value.ToString();
            lbJYJG.Text = dataGridView1.CurrentRow.Cells["检验人"].Value.ToString();
            NudQC_BF.Value = (decimal)dataGridView1.CurrentRow.Cells["QC报废数"].Value;
            NudQC_HG.Value = (decimal)dataGridView1.CurrentRow.Cells["QC合格数"].Value;
            NudQC_RB.Value = (decimal)dataGridView1.CurrentRow.Cells["QC让步数"].Value;
            NudQC_TH.Value = (decimal)dataGridView1.CurrentRow.Cells["QC退货数"].Value;
            NudSQE_BHG.Value = (decimal)dataGridView1.CurrentRow.Cells["采购工程师不合格数"].Value;
            NudSQE_HG.Value = (decimal)dataGridView1.CurrentRow.Cells["采购工程师合格数"].Value;
            NudSQE_TFGS.Value = (decimal)dataGridView1.CurrentRow.Cells["SQE工时"].Value;
            lbDJZT.Text = dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString();
            m_strDJH = dataGridView1.CurrentRow.Cells["单据号"].Value.ToString();
            lbDJH.Text = m_strDJH;

            m_drSQE["挑返原因"] = dataGridView1.CurrentRow.Cells["挑返原因"].Value.ToString();
            m_drSQE["挑返方法及要求"] = dataGridView1.CurrentRow.Cells["挑返方法及要求"].Value.ToString();
            m_drSQE["挑返损耗及工时"] = dataGridView1.CurrentRow.Cells["挑返损耗及工时"].Value.ToString();
            #endregion

            #region 获取报检单信息

            View_S_CheckOutInDepotBill bill = m_serverCheckOutInDepot.GetBill(dataGridView1.CurrentRow.Cells["关联入库单号"].Value.ToString());

            if (bill == null)
            {
                return;
            }

            txtBill_ID.Text = bill.入库单号;
            txtOrderFormNumber.Text = bill.订单号;
            txtBatchNo.Text = bill.批次号;

            if (bill.供方批次号 != null)
            {
                txtProviderBatchNo.Text = bill.供方批次号;
            }
            else
            {
                txtProviderBatchNo.Text = "";
            }

            if (!GlobalObject.GeneralFunction.IsNullOrEmpty(bill.备注))
            {
                txtRemark.Text = bill.备注;
            }
            else
            {
                txtRemark.Text = "";
            }

            txtDeclareCount.Text = bill.报检数量.ToString();
            txtName.Text = bill.物品名称;
            txtCode.Text = bill.图号型号;
            txtCode.Tag = bill.物品ID;
            txtSpec.Text = bill.规格;
            lbdw.Text = bill.单位;
            txtMaterialType.Text = bill.仓库;
            txtProvider.Text = bill.供货单位;
            txtStorage.Text = UniversalFunction.GetStorageName(bill.库房代码.ToString());

            #endregion
        }

        private void 刷新数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshDataGirdView(m_serverCheckReturnRepair.GetAllBill(checkBillDateAndStatus1.cmbBillStatus.Text,
                        checkBillDateAndStatus1.dtpStartTime.Value, checkBillDateAndStatus1.dtpEndTime.Value));

            ClearMessage();
        }

        private void 打印单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() != "单据已完成")
            {
                MessageDialog.ShowPromptMessage("请选择已确认的记录后再进行此操作");
                return;
            }

            IBillReportInfo report = new 报表_挑返单(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString(), labelTitle.Text);
            PrintReportBill print = new PrintReportBill(21, 29.7, report);
            print.DirectPrintReport();
        }

        private void 导出EXCEL表单ToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void 回退单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbDJZT.Text == "等待审批确认" && SQE操作ToolStripMenuItem.Visible == true)
            {
                ReturnBillStatus();
            }

            if (lbDJZT.Text == "等待处理结果" && SQE主管操作ToolStripMenuItem.Visible == true)
            {
                ReturnBillStatus();
            }

            if (lbDJZT.Text == "等待检验结果" && QC操作ToolStripMenuItem.Visible == true)
            {
                ReturnBillStatus();
            }
        }

        private void ReturnBillStatus()
        {
            if (lbDJZT.Text != "已完成")
            {
                回退单据 form = new 回退单据(CE_BillTypeEnum.挑选返工返修单, lbDJH.Text, lbDJZT.Text);

                if (form.ShowDialog() == DialogResult.OK)
                {
                    if (MessageBox.Show("您确定要回退此单据吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        if (m_serverCheckReturnRepair.ReturnBill(form.StrBillID,
                            form.StrBillStatus, out m_err, form.Reason))
                        {
                            MessageDialog.ShowPromptMessage("回退成功");

                        }
                        else
                        {
                            MessageDialog.ShowPromptMessage(m_err);
                        }
                    }

                    RefreshDataGirdView(m_serverCheckReturnRepair.GetAllBill(checkBillDateAndStatus1.cmbBillStatus.Text,
                        checkBillDateAndStatus1.dtpStartTime.Value, checkBillDateAndStatus1.dtpEndTime.Value));
                    PositioningRecord(form.StrBillID);
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            RefreshDataGirdView(m_serverCheckReturnRepair.GetAllBill(checkBillDateAndStatus1.cmbBillStatus.Text,
                        checkBillDateAndStatus1.dtpStartTime.Value, checkBillDateAndStatus1.dtpEndTime.Value));

            ClearMessage();
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
    }
}