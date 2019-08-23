
/******************************************************************************
 *
 * 文件名称:  零星采购变更处置单.cs
 * 作者    :  邱瑶       日期: 2014/2/12
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
using ServerModule;
using UniversalControlLibrary;
using PlatformManagement;
using GlobalObject;

namespace Expression
{
    public partial class 零星采购变更处置单 : Form
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
        /// 关联单号
        /// </summary>
        string m_associateBillNo;

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 零星采购服务类
        /// </summary>
        IMinorPurchaseBillServer m_minorBillServer = ServerModule.ServerModuleFactory.GetServerModule<IMinorPurchaseBillServer>();

        /// <summary>
        /// 零星采购变更处置单数据集
        /// </summary>
        B_MinorPurchaseChangeBill m_changMinorBill = new B_MinorPurchaseChangeBill();

        public 零星采购变更处置单(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authorityFlag = nodeInfo.Authority;
            string[] strBillStatus = {"全部","等待请购人确认","等待主管审核","已完成"};

            checkBillDateAndStatus1.InsertComBox(strBillStatus);
            checkBillDateAndStatus1.dtpStartTime.Value = ServerTime.Time.AddDays(1).AddMonths(-1);
            checkBillDateAndStatus1.dtpEndTime.Value = ServerTime.Time.AddDays(1);

            RefreshDataGridView();
        }

        private void checkBillDateAndStatus1_OnCompleteSearch()
        {
            RefreshDataGridView();
        }

        private void 零星采购变更处置单_Load(object sender, EventArgs e)
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

                    DataTable dtMessage = UniversalFunction.PositioningOneRecord(msg.MessageContent, "零星采购变更处置单");

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
                    if (dataGridView1.Rows[i].Cells["编号"].Value.ToString() == billNo)
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
            DataTable dt = m_minorBillServer.GetMinorPurchaseChangeBill(checkBillDateAndStatus1.dtpStartTime.Value,
                checkBillDateAndStatus1.dtpEndTime.Value, checkBillDateAndStatus1.cmbBillStatus.Text);

            if (dt != null)
            {
                dataGridView1.DataSource = dt;
            }

            this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
               this.dataGridView1_ColumnWidthChanged);

            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            userControlDataLocalizer1.Init(dataGridView1, this.Name,
               UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));

            dataGridView1.Refresh();
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        private void 零星采购变更处置单_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        private void btnBillNo_Click(object sender, EventArgs e)
        {
            FormQueryInfo frm = QueryInfoDialog.GetMinorPurchaseInfo();

            if (frm.ShowDialog() == DialogResult.OK)
            {
                txtAssociateBillNo.Text = (string)frm.GetDataItem("单据号");

                m_associateBillNo = txtAssociateBillNo.Text;
            }
        }

        private void btnOldGoodsInfo_Click(object sender, EventArgs e)
        {
            if (txtAssociateBillNo.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请先选择关联的采购申请单，再进行此操作！");
                return;
            }

            FormQueryInfo frm = QueryInfoDialog.GetMinorPurchaseList(m_associateBillNo);

            if (frm.ShowDialog() == DialogResult.OK)
            {
                txtOldGoodsCode.Text = (string)frm.GetDataItem("图号型号");
                txtOldGoodsName.Text = (string)frm.GetDataItem("物品名称");
                txtOldGoodsSpec.Text = (string)frm.GetDataItem("规格");
            }
        }

        private void btnFindCode_Click(object sender, EventArgs e)
        {
            DataTable dtGood = (DataTable)dataGridView1.DataSource;

            FormQueryInfo form = QueryInfoDialog.GetAllGoodsInfo();

            if (form != null && form.ShowDialog() == DialogResult.OK)
            {
                txtGoodsName.ReadOnly = true;
                txtGoodsCode.ReadOnly = true;
                txtSpec.ReadOnly = true;

                txtGoodsCode.Text = form.GetDataItem("图号型号").ToString();
                txtGoodsName.Text = form.GetDataItem("物品名称").ToString();
                txtSpec.Text = form.GetDataItem("规格").ToString();
            }
        }

        private void cbHand_CheckedChanged(object sender, EventArgs e)
        {
            if (cbHand.Checked)
            {
                if (MessageDialog.ShowEnquiryMessage("确定已经查找过零件，并且系统中没有需要请购的零件吗？") 
                    == DialogResult.No)
                {
                    cbHand.Checked = false;
                    return;
                }

                txtGoodsCode.ReadOnly = false;
                txtGoodsName.ReadOnly = false;
                txtSpec.ReadOnly = false;
                btnFindCode.Enabled = false;
            }
            else
            {
                txtGoodsCode.ReadOnly = true;
                txtGoodsName.ReadOnly = true;
                txtSpec.ReadOnly = true;
                btnFindCode.Enabled = true;
            }
        }

        /// <summary>
        /// 检测控件输入的正确性
        /// </summary>
        /// <returns></returns>
        bool CheckControl()
        {
            if (txtAssociateBillNo.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请选择关联的采购申请单！");
                return false;
            }

            if (txtOldGoodsCode.Text.Trim() == "" && txtOldGoodsName.Text.Trim() == ""
                && txtOldGoodsSpec.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请选择需要更改的零件！");
                return false;
            }

            if (txtGoodsCode.Text.Trim() == "" && txtGoodsName.Text.Trim() == ""
                && txtSpec.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请输入更新的零件信息");
                return false;
            }

            if (cbHand.Checked)
            {
                if (MessageDialog.ShowEnquiryMessage("所购买零件在之前从未购买过吗？") != DialogResult.Yes)
                {
                    return false;
                }
            }

            if (txtReason.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请输入零件变更原因！");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 获取数据信息
        /// </summary>
        void GetMeaage()
        {
            m_changMinorBill.AssociateBillNo = txtAssociateBillNo.Text.Trim();
            m_changMinorBill.ChangeReason = txtReason.Text.Trim();
            m_changMinorBill.NewGoodsCode = txtGoodsCode.Text.Trim();
            m_changMinorBill.NewGoodsName = txtGoodsName.Text.Trim();
            m_changMinorBill.NewGoodsSpec = txtSpec.Text.Trim();
            m_changMinorBill.OldGoodsCode = txtOldGoodsCode.Text.Trim();
            m_changMinorBill.OldGoodsName = txtOldGoodsName.Text.Trim();
            m_changMinorBill.OldGoodsSpec = txtOldGoodsSpec.Text.Trim();
        }

        private void 提交toolStripButton_Click(object sender, EventArgs e)
        {
            if (!CheckControl())
            {
                return;
            }

            GetMeaage();

            m_changMinorBill.Applicant = BasicInfo.LoginID;
            m_changMinorBill.ApplicantDate = ServerTime.Time;
            m_changMinorBill.BillStatus = "等待请购人确认";

            if (!m_minorBillServer.InsertChangeBill(m_changMinorBill, out m_error))
            {
                MessageDialog.ShowPromptMessage(m_error);
                return;
            }
            else
            {
                MessageDialog.ShowPromptMessage("提交成功！");
            }

            btnNew_Click(null, null);
            RefreshDataGridView();

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
                if (dataGridView1.Rows[i].Cells["关联单号"].Value.ToString() == m_changMinorBill.AssociateBillNo
                    && dataGridView1.Rows[i].Cells["旧零件图号型号"].Value.ToString() == m_changMinorBill.OldGoodsCode
                    && dataGridView1.Rows[i].Cells["旧零件物品名称"].Value.ToString() == m_changMinorBill.OldGoodsName
                    && dataGridView1.Rows[i].Cells["旧零件规格"].Value.ToString() == m_changMinorBill.OldGoodsSpec)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                    break;
                }
            }
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtAssociateBillNo.Text = dataGridView1.CurrentRow.Cells["关联单号"].Value.ToString();
            txtApplicant.Text = dataGridView1.CurrentRow.Cells["申请变更人"].Value.ToString();
            txtConfirmor.Text = dataGridView1.CurrentRow.Cells["请购人"].Value.ToString();
            txtDeptDirector.Text = dataGridView1.CurrentRow.Cells["部门主管"].Value.ToString();
            txtGoodsCode.Text = dataGridView1.CurrentRow.Cells["更新件图号型号"].Value.ToString();
            txtGoodsName.Text = dataGridView1.CurrentRow.Cells["更新件物品名称"].Value.ToString();
            txtOldGoodsCode.Text = dataGridView1.CurrentRow.Cells["旧零件图号型号"].Value.ToString();
            txtOldGoodsName.Text = dataGridView1.CurrentRow.Cells["旧零件物品名称"].Value.ToString();
            txtOldGoodsSpec.Text = dataGridView1.CurrentRow.Cells["旧零件规格"].Value.ToString();
            txtReason.Text = dataGridView1.CurrentRow.Cells["变更原因"].Value.ToString();
            txtSpec.Text = dataGridView1.CurrentRow.Cells["更新件规格"].Value.ToString();

            dtpApplicantDate.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["申请日期"].Value
                == DBNull.Value ? ServerTime.Time : dataGridView1.CurrentRow.Cells["申请日期"].Value);
            dtpConfirmDate.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["确认日期"].Value
                == DBNull.Value ? ServerTime.Time : dataGridView1.CurrentRow.Cells["确认日期"].Value);
            dtpDeptDirectDate.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["审核日期"].Value
                == DBNull.Value ? ServerTime.Time : dataGridView1.CurrentRow.Cells["审核日期"].Value);

            lblBillStatus.Text = dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            txtSpec.Text = "";
            txtReason.Text = "";
            txtOldGoodsSpec.Text = "";
            txtOldGoodsName.Text = "";
            txtOldGoodsCode.Text = "";
            txtGoodsName.Text = "";
            txtGoodsCode.Text = "";
            txtDeptDirector.Text = "";
            txtConfirmor.Text = "";
            txtAssociateBillNo.Text = "";
            txtApplicant.Text = BasicInfo.LoginName;

            DateTime time = ServerTime.Time;

            dtpApplicantDate.Value = time;
            dtpConfirmDate.Value = time;
            dtpDeptDirectDate.Value = time;

            lblBillStatus.Text = "新建单据";
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                if (dataGridView1.CurrentRow.Cells["单机状态"].Value.ToString() != "已完成")
                {
                    if (MessageDialog.ShowEnquiryMessage("确定删除选中的数据吗？") == DialogResult.Yes)
                    {
                        if (m_minorBillServer.DeleteChangeBill(Convert.ToInt32(dataGridView1.CurrentRow.Cells["编号"].Value), out m_error))
                        {
                            MessageDialog.ShowPromptMessage("删除成功！");
                        }
                        else
                        {
                            MessageDialog.ShowPromptMessage(m_error);
                            return;
                        }

                        btnNew_Click(null, null);
                        RefreshDataGridView();
                    }
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请选择一行数据后再进行此操作！");
            }
        }

        private void 确认toolStripButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                string applicant = m_minorBillServer.GetBillInfo(dataGridView1.CurrentRow.Cells["关联单号"].Value.ToString()).Applicant;

                if (applicant == BasicInfo.LoginID)
                {
                    if (MessageDialog.ShowEnquiryMessage("确认购买更新件吗？") == DialogResult.Yes)
                    {
                        string billNo = dataGridView1.CurrentRow.Cells["编号"].Value.ToString();

                        m_changMinorBill.Confirmor = BasicInfo.LoginID;
                        m_changMinorBill.ConfirmDate = ServerTime.Time;
                        m_changMinorBill.BillStatus = "等待负责人审核";
                        m_changMinorBill.AssociateBillNo = dataGridView1.CurrentRow.Cells["关联单号"].Value.ToString();
                        m_changMinorBill.OldGoodsCode = dataGridView1.CurrentRow.Cells["旧零件图号型号"].Value.ToString();
                        m_changMinorBill.OldGoodsName = dataGridView1.CurrentRow.Cells["旧零件物品名称"].Value.ToString();
                        m_changMinorBill.OldGoodsSpec = dataGridView1.CurrentRow.Cells["旧零件规格"].Value.ToString();

                        if (m_minorBillServer.UpdateChangeBill(m_changMinorBill, out m_error))
                        {
                            MessageDialog.ShowPromptMessage("确认成功！");
                        }
                        else
                        {
                            MessageDialog.ShowPromptMessage(m_error);
                            return;
                        }
                        
                        RefreshDataGridView();
                        PositioningRecord(billNo);
                    }
                }
                else
                {
                    MessageDialog.ShowPromptMessage("只有请购人【" + UniversalFunction.GetPersonnelName(applicant) + "】才能确认！");
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请选择一行数据后再进行此操作！");
            }
        }

        private void 审核toolStripButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
                {
                    m_changMinorBill.AssociateBillNo = dataGridView1.SelectedRows[i].Cells["关联单号"].Value.ToString();
                    m_changMinorBill.ChangeReason = dataGridView1.SelectedRows[i].Cells["变更原因"].Value.ToString();
                    m_changMinorBill.NewGoodsCode = dataGridView1.SelectedRows[i].Cells["更新件图号型号"].Value.ToString();
                    m_changMinorBill.NewGoodsName = dataGridView1.SelectedRows[i].Cells["更新件物品名称"].Value.ToString();
                    m_changMinorBill.NewGoodsSpec = dataGridView1.SelectedRows[i].Cells["更新件规格"].Value.ToString();
                    m_changMinorBill.OldGoodsCode = dataGridView1.SelectedRows[i].Cells["旧零件图号型号"].Value.ToString();
                    m_changMinorBill.OldGoodsName = dataGridView1.SelectedRows[i].Cells["旧零件物品名称"].Value.ToString();
                    m_changMinorBill.OldGoodsSpec = dataGridView1.SelectedRows[i].Cells["旧零件规格"].Value.ToString();

                    m_changMinorBill.BillStatus = "已完成";
                    m_changMinorBill.DeptDirectDate = ServerTime.Time;
                    m_changMinorBill.DeptDirector = BasicInfo.LoginID;

                    if (m_minorBillServer.UpdateChangeBill(m_changMinorBill, out m_error))
                    {
                        continue;
                    }
                    else
                    {
                        MessageDialog.ShowPromptMessage(m_error);
                        break;
                    }
                }

                RefreshDataGridView();
            }
            else
            {
                MessageDialog.ShowPromptMessage("请选择需要操作的数据行！");
            }
        }

        private void 刷新toolStripButton_Click(object sender, EventArgs e)
        {
            RefreshDataGridView();
        }
    }
}
