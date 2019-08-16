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
    /// CVT出厂检验记录界面
    /// </summary>
    public partial class CVT出厂检验记录表 : Form
    {
        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 单据号服务
        /// </summary>
        IAssignBillNoServer m_serverBillNo = ServerModuleFactory.GetServerModule<IAssignBillNoServer>();

        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// CVT出厂检验记录管理服务
        /// </summary>
        IProductDeliveryInspectionServer m_serverDeliveryInSpection = ServerModuleFactory.GetServerModule<IProductDeliveryInspectionServer>();

        /// <summary>
        /// 数据定位控件
        /// </summary>
        UserControlDataLocalizer m_dataLocalizer;

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 数据集
        /// </summary>
        P_DeliveryInspection m_lnqDelivery = new P_DeliveryInspection();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        public CVT出厂检验记录表(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_billMessageServer.BillType = "营销入库单";

            m_billNoControl = new BillNumberControl(CE_BillTypeEnum.CVT出厂检验记录表,
                m_serverDeliveryInSpection);

            m_authFlag = nodeInfo.Authority;

            string[] strBillStatus = { "全  部", 
                                     "等待检验",
                                     "等待最终判定",
                                     "已完成"};

            checkBillDateAndStatus1.InsertComBox(strBillStatus);

            checkBillDateAndStatus1.OnCompleteSearch +=
                new GlobalObject.DelegateCollection.NonArgumentHandle(checkBillDateAndStatus1_OnCompleteSearch);

            checkBillDateAndStatus1.dtpStartTime.Value = ServerTime.Time.AddDays(1).AddMonths(-1);
            checkBillDateAndStatus1.dtpEndTime.Value = ServerTime.Time.AddDays(1);

            RefreshDataGirdView(m_serverDeliveryInSpection.GetDeliveryInspection(checkBillDateAndStatus1.dtpStartTime.Value,
                checkBillDateAndStatus1.dtpEndTime.Value, checkBillDateAndStatus1.cmbBillStatus.Text));
        }

        void checkBillDateAndStatus1_OnCompleteSearch()
        {
            RefreshDataGirdView(m_serverDeliveryInSpection.GetDeliveryInspection(checkBillDateAndStatus1.dtpStartTime.Value,
                checkBillDateAndStatus1.dtpEndTime.Value, checkBillDateAndStatus1.cmbBillStatus.Text));
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

                    DataTable dtMessage = UniversalFunction.PositioningOneRecord(msg.MessageContent, "CVT出厂检验记录表");

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
        /// 权限控制
        /// </summary>
        /// <param name="authorityFlag">权限标志</param>
        void AuthorityControl(PlatformManagement.AuthorityFlag authorityFlag)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip1, authorityFlag);
            FaceAuthoritySetting.SetEnable(this.Controls, authorityFlag);
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="source">数据集</param>
        void RefreshDataGirdView(DataTable source)
        {
            dataGridView1.DataSource = source;

            if (m_dataLocalizer == null)
            {
                m_dataLocalizer = new UserControlDataLocalizer(dataGridView1, this.Name,
                    UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
                panelPara.Controls.Add(m_dataLocalizer);
                m_dataLocalizer.Dock = DockStyle.Bottom;
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
        /// 清空数据
        /// </summary>
        void ClearDate()
        {
            txtAssociatedBillNo.Text = "";
            txtBill_ID.Text = "";
            txtDisqualificationCase.Text = "";
            txtProductCode.Text = "";
            txtProductType.Text = "";
            txtRemark.Text = "";
            cmbTechnicalRequirementsName.SelectedIndex = -1;
            cmbTestItemName.SelectedIndex = -1;
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            string strBillID = dataGridView1.CurrentRow.Cells["单据号"].Value.ToString();

            CVT出厂检验记录 form = new CVT出厂检验记录(strBillID, m_authFlag);
            form.ShowDialog();

            RefreshDataGirdView(m_serverDeliveryInSpection.GetDeliveryInspection(checkBillDateAndStatus1.dtpStartTime.Value,
                checkBillDateAndStatus1.dtpEndTime.Value, checkBillDateAndStatus1.cmbBillStatus.Text));
            PositioningRecord(strBillID);
        }

        private void btnBatch_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in dataGridView1.SelectedRows)
            {
                if (item.Cells["单据状态"].Value.ToString() != "已完成")
                {
                    m_lnqDelivery = new P_DeliveryInspection();
                    m_lnqDelivery.DJH = item.Cells["单据号"].Value.ToString();
                    m_lnqDelivery.Verdict = "合格";
                    m_lnqDelivery.Remark = txtRemark.Text;

                    if (!m_serverDeliveryInSpection.UpdateDeliveryInspection(m_lnqDelivery, out m_err))
                    {
                        MessageDialog.ShowPromptMessage(m_err);
                    }
                    else
                    {
                        string strStorage = "";

                        if (m_serverDeliveryInSpection.IsExamine(m_lnqDelivery.DJH, out strStorage))
                        {
                            m_billMessageServer.PassFlowMessage(m_lnqDelivery.DJH,
                                string.Format("{0} 号营销入库单，请仓管员确认", m_lnqDelivery.DJH),
                                m_billMessageServer.GetRoleStringForStorage(strStorage).ToString(), true);
                        }
                    }
                }
            }

            MessageDialog.ShowPromptMessage("执行成功");

            RefreshDataGirdView(m_serverDeliveryInSpection.GetDeliveryInspection(checkBillDateAndStatus1.dtpStartTime.Value,
                checkBillDateAndStatus1.dtpEndTime.Value, checkBillDateAndStatus1.cmbBillStatus.Text));
        }

        /// <summary>
        /// 获取数据集
        /// </summary>
        void GetMessage()
        {
            m_lnqDelivery = new P_DeliveryInspection();

            m_lnqDelivery.AssociatedBillNo = txtAssociatedBillNo.Text;
            m_lnqDelivery.DJH = txtBill_ID.Text;
            m_lnqDelivery.DJZT = lbDJZT.Text;
            m_lnqDelivery.ProductCode = txtProductCode.Text;
            m_lnqDelivery.ProductType = txtProductType.Text;
            m_lnqDelivery.DisqualificationCase = txtDisqualificationCase.Text;
            m_lnqDelivery.DisqualificationItem = Convert.ToInt32(cmbTechnicalRequirementsName.Tag);
            m_lnqDelivery.Remark = txtRemark.Text;

            if (rbYes.Checked)
            {
                m_lnqDelivery.Verdict = rbYes.Text;
            }

            if (rbNo.Checked)
            {
                m_lnqDelivery.Verdict = rbNo.Text;
            }

            if (rbFinalNo.Checked)
            {
                m_lnqDelivery.FinalVerdict = rbFinalNo.Text;
            }

            if (rbFinalYes.Checked)
            {
                m_lnqDelivery.FinalVerdict = rbFinalYes.Text;
            }
        }

        /// <summary>
        /// 检查信息
        /// </summary>
        /// <returns>通过返回True，不通过返回False</returns>
        bool CheckMessage()
        {
            if (!rbYes.Checked && !rbNo.Checked)
            {
                MessageDialog.ShowPromptMessage("请做出【结论】");
                return false;
            }

            if (rbNo.Checked
                && (Convert.ToInt32(cmbTechnicalRequirementsName.Tag) == 0
                || cmbTechnicalRequirementsName.Tag == null))
            {
                MessageDialog.ShowPromptMessage("请选择【不合格的项目】");
                return false;
            }

            return true;
        }

        private void btnOnly_Click(object sender, EventArgs e)
        {
            if (CheckMessage())
            {
                if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() != "已完成")
                {
                    GetMessage();

                    if (!m_serverDeliveryInSpection.UpdateDeliveryInspection(m_lnqDelivery, out m_err))
                    {
                        MessageDialog.ShowPromptMessage(m_err);
                    }
                    else
                    {
                        MessageDialog.ShowPromptMessage("执行成功");

                        string strStorage = "";

                        if (m_serverDeliveryInSpection.IsExamine(m_lnqDelivery.DJH, out strStorage))
                        {
                            m_billMessageServer.PassFlowMessage(m_lnqDelivery.DJH,
                                string.Format("{0} 号营销入库单，请仓管员确认", m_lnqDelivery.DJH),
                                m_billMessageServer.GetRoleStringForStorage(strStorage).ToString(), true);
                        }
                    }
                }
            }

            RefreshDataGirdView(m_serverDeliveryInSpection.GetDeliveryInspection(checkBillDateAndStatus1.dtpStartTime.Value,
                checkBillDateAndStatus1.dtpEndTime.Value, checkBillDateAndStatus1.cmbBillStatus.Text));
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            string strBillID = dataGridView1.CurrentRow.Cells["单据号"].Value.ToString();

            CVT出厂检验记录 form = new CVT出厂检验记录(strBillID, m_authFlag);
            form.ShowDialog();

            RefreshDataGirdView(m_serverDeliveryInSpection.GetDeliveryInspection(checkBillDateAndStatus1.dtpStartTime.Value,
                checkBillDateAndStatus1.dtpEndTime.Value, checkBillDateAndStatus1.cmbBillStatus.Text));
            PositioningRecord(strBillID);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ClearDate();
            RefreshDataGirdView(m_serverDeliveryInSpection.GetDeliveryInspection(checkBillDateAndStatus1.dtpStartTime.Value,
                checkBillDateAndStatus1.dtpEndTime.Value, checkBillDateAndStatus1.cmbBillStatus.Text));
        }

        private void cmbTechnicalRequirementsName_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbTechnicalRequirementsName.Tag =
                m_serverDeliveryInSpection.GetTechnicalRequirementsID(cmbTechnicalRequirementsName.Text.ToString(), txtProductType.Text);
        }

        private void rbNo_CheckedChanged(object sender, EventArgs e)
        {
            if (rbNo.Checked)
            {
                cmbTechnicalRequirementsName.Enabled = true;
                txtDisqualificationCase.Enabled = true;
                cmbTestItemName.Enabled = true;
            }
            else
            {
                cmbTechnicalRequirementsName.Enabled = false;
                txtDisqualificationCase.Enabled = false;
                cmbTestItemName.Enabled = false;
                txtDisqualificationCase.Text = "";
                cmbTechnicalRequirementsName.Text = "";
                cmbTestItemName.Text = "";
                cmbTechnicalRequirementsName.Items.Clear();
            }
        }

        private void cmbTestItemName_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbTestItemName.Tag = m_serverDeliveryInSpection.GetTestItemNameID(cmbTestItemName.Text, txtProductType.Text);
            cmbTechnicalRequirementsName.Items.Clear();

            DataTable dt = m_serverDeliveryInSpection.GetTechnicalRequirements(cmbTestItemName.Text,txtProductType.Text);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cmbTechnicalRequirementsName.Items.Add(dt.Rows[i]["技术要求"].ToString());
            }
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            cmbTestItemName.Items.Clear();
            cmbTechnicalRequirementsName.Items.Clear();

            if (dataGridView1.Rows.Count == 0)
            {
                return;
            }

            ClearDate();

            txtAssociatedBillNo.Text = dataGridView1.CurrentRow.Cells["关联单号"].Value.ToString();
            txtBill_ID.Text = dataGridView1.CurrentRow.Cells["单据号"].Value.ToString();
            txtDisqualificationCase.Text = dataGridView1.CurrentRow.Cells["不合格情况"].Value.ToString();
            txtProductCode.Text = dataGridView1.CurrentRow.Cells["产品编号"].Value.ToString();
            txtProductType.Text = dataGridView1.CurrentRow.Cells["产品型号"].Value.ToString();
            txtRemark.Text = dataGridView1.CurrentRow.Cells["备注"].Value.ToString();
            lbDJZT.Text = dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString();

            if (dataGridView1.CurrentRow.Cells["不合格项目"].Value != DBNull.Value
                || dataGridView1.CurrentRow.Cells["不合格项目"].Value.ToString() != "")
            {
                P_TechnicalRequirements tempRequirements =
                    m_serverDeliveryInSpection.GetTechnicalRequirementsInfo((int)dataGridView1.CurrentRow.Cells["不合格项目ID"].Value);

                if (tempRequirements != null)
                {
                    cmbTechnicalRequirementsName.Tag = tempRequirements.TechnicalID;
                    cmbTechnicalRequirementsName.Text = tempRequirements.TechnicalRequirementsName;

                    P_AllAccreditedTestingItems tempAccredited = m_serverDeliveryInSpection.GetAllAccreditedTestingItemsInfo((int)tempRequirements.TestID);

                    if (tempAccredited != null)
                    {
                        cmbTestItemName.Text = tempAccredited.TestItemName;
                        cmbTestItemName.Tag = tempAccredited.TestID;
                    }
                }
            }

            if (dataGridView1.CurrentRow.Cells["结论"].Value != DBNull.Value
                || dataGridView1.CurrentRow.Cells["结论"].Value.ToString() != "")
            {
                if (dataGridView1.CurrentRow.Cells["结论"].Value.ToString() == rbNo.Text)
                {
                    rbNo.Checked = true;
                }
                else
                {
                    rbYes.Checked = true;
                }
            }
            else
            {
                rbNo.Checked = false;
                rbYes.Checked = false;
            }

            if (dataGridView1.CurrentRow.Cells["最终判定"].Value != DBNull.Value
                || dataGridView1.CurrentRow.Cells["最终判定"].Value.ToString() != "")
            {
                if (dataGridView1.CurrentRow.Cells["最终判定"].Value.ToString() == rbFinalNo.Text)
                {
                    rbFinalNo.Checked = true;
                }
                else
                {
                    rbFinalYes.Checked = true;
                }
            }
            else
            {
                rbFinalNo.Checked = false;
                rbFinalYes.Checked = false;
            }

            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() != "等待最终判定")
            {
                rbYes.Enabled = true;
                rbNo.Enabled = true;
                txtDisqualificationCase.Enabled = true;
                cmbTestItemName.Enabled = true;
                cmbTechnicalRequirementsName.Enabled = true;
                rbFinalNo.Enabled = false;
                rbFinalYes.Enabled = false;
            }
            else
            {
                rbYes.Enabled = false;
                rbNo.Enabled = false;
                txtDisqualificationCase.Enabled = false;
                cmbTestItemName.Enabled = false;
                cmbTechnicalRequirementsName.Enabled = false;
                rbFinalNo.Enabled = true;
                rbFinalYes.Enabled = true;
            }

            DataTable dtNullMessage = m_serverDeliveryInSpection.GetEmptyTable(txtProductType.Text);

            for (int i = 0; i < dtNullMessage.Rows.Count; i++)
            {
                if (i == 0)
                {
                    cmbTestItemName.Items.Add(dtNullMessage.Rows[i]["检测项目"].ToString());
                }
                else
                {
                    if (dtNullMessage.Rows[i]["检测项目"].ToString()
                        != dtNullMessage.Rows[i - 1]["检测项目"].ToString() && i != 0)
                    {
                        cmbTestItemName.Items.Add(dtNullMessage.Rows[i]["检测项目"].ToString());
                    }
                }
            }
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

        private void btnOutExcel_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
        }

        private void btnJudge_Click(object sender, EventArgs e)
        {
            GetMessage();

            if (m_lnqDelivery.FinalVerdict == null || m_lnqDelivery.FinalVerdict == "")
            {
                MessageDialog.ShowPromptMessage("请做出最终判定");
                return;
            }
            else
            {
                if (!m_serverDeliveryInSpection.FinalJudgeBill(m_lnqDelivery, out m_err))
                {
                    MessageDialog.ShowPromptMessage(m_err);
                    return;
                }
                else
                {
                    MessageDialog.ShowPromptMessage("执行成功");

                    string strStorage = "";
                    if (m_serverDeliveryInSpection.IsExamine(m_lnqDelivery.DJH, out strStorage))
                    {
                        m_billMessageServer.PassFlowMessage(m_lnqDelivery.DJH,
                            string.Format("{0} 号营销入库单，请仓管员确认", m_lnqDelivery.DJH),
                            m_billMessageServer.GetRoleStringForStorage(strStorage).ToString(), true);
                    }
                }

                RefreshDataGirdView(m_serverDeliveryInSpection.GetDeliveryInspection(checkBillDateAndStatus1.dtpStartTime.Value,
                checkBillDateAndStatus1.dtpEndTime.Value, checkBillDateAndStatus1.cmbBillStatus.Text));
            }
        }

        private void dataGridView1_DoubleClick_1(object sender, EventArgs e)
        {
            btnFind_Click(sender, e);
        }

        private void CVT出厂检验记录表_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authFlag);
        }

        private void btnCVTFinishSelect_Click(object sender, EventArgs e)
        {
            CVT终检信息查询 frm = new CVT终检信息查询();
            frm.ShowDialog();
        }
    }
}
