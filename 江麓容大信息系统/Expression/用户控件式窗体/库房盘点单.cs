using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using GlobalObject;
using PlatformManagement;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 库房盘点单
    /// </summary>
    public partial class 库房盘点单 : Form
    {
        /// <summary>
        /// 部门服务组件
        /// </summary>
        IDepartmentServer m_serverDepartment = ServerModuleFactory.GetServerModule<IDepartmentServer>();

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 服务组件
        /// </summary>
        IStoreageCheck m_serverStroageCheck = ServerModuleFactory.GetServerModule<IStoreageCheck>();

        /// <summary>
        /// 数据定位控件
        /// </summary>
        UserControlDataLocalizer m_dataLocalizer;

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 类别信息
        /// </summary>
        DataTable m_dtDepot = new DataTable();

        /// <summary>
        /// 明细表信息
        /// </summary>
        DataTable m_dtMx = new DataTable();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 数据集
        /// </summary>
        S_StorageCheck m_lnqCheck = new S_StorageCheck();

        public 库房盘点单(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_billMessageServer.BillType = "库房盘点单";

            m_billNoControl = new BillNumberControl(CE_BillTypeEnum.库房盘点单, m_serverStroageCheck);

            m_authFlag = nodeInfo.Authority;

            DataTable dt = UniversalFunction.GetStorageTb();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cmbStorage.Items.Add(dt.Rows[i]["StorageName"].ToString());
            }

            cmbStorage.SelectedIndex = -1;
            RefreshDataGirdView(m_serverStroageCheck.GetAllBill());
        }

        private void 库房盘点单_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authFlag);
            dataGridView1_CellEnter(null, null);
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

                    DataTable dtMessage = UniversalFunction.PositioningOneRecord(msg.MessageContent, "库房盘点单");

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
        /// 权限控制
        /// </summary>
        /// <param name="authorityFlag">权限标志</param>
        void AuthorityControl(PlatformManagement.AuthorityFlag authorityFlag)
        {
            FaceAuthoritySetting.SetVisibly(menuStrip, authorityFlag);
            FaceAuthoritySetting.SetEnable(this.Controls, authorityFlag);
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="source">数据集</param>
        void RefreshDataGirdView(DataTable source)
        {
            txtBill_ID.Text = "";
            cmbStorage.SelectedIndex = -1;
            cmbPDFS.SelectedIndex = -1;
            dataGridView1.DataSource = source;

            dataGridView1.Columns["单据号"].Width = 120;
            dataGridView1.Columns["单据状态"].Width = 120;
            dataGridView1.Columns["所属库房"].Width = 120;

            if (m_dataLocalizer == null)
            {
                m_dataLocalizer = new UserControlDataLocalizer(dataGridView1, this.Name, 
                    UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
                panelPara.Controls.Add(m_dataLocalizer);
                m_dataLocalizer.Dock = DockStyle.Bottom;
            }
        }

        /// <summary>
        /// 检查数据
        /// </summary>
        /// <returns>检测通过返回True，否则返回False</returns>
        private bool CheckDate()
        {

            if (txtBill_ID.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("单据号不能为空");
                return false;
            }

            if (cmbPDFS.Text == "")
            {
                MessageDialog.ShowPromptMessage("请选择盘点方式!");
                cmbPDFS.Focus();
                return false;
            }

            if (cmbStorage.Text == "")
            {
                MessageDialog.ShowPromptMessage("请选择所属库房");
                cmbStorage.Focus();
                return false;
            }

            if (dataGridView1.CurrentRow != null && m_serverStroageCheck.IsRepeat(UniversalFunction.GetStorageID(cmbStorage.Text), 
                dataGridView1.CurrentRow.Cells["单据号"].Value.ToString()))
            {
                MessageDialog.ShowPromptMessage("您已重复建单，请删除以前的新建单据，再建单");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 获得信息
        /// </summary>
        private void GetMessage()
        {
            m_lnqCheck.DJH = txtBill_ID.Text;
            m_lnqCheck.DJFS = cmbPDFS.Text;
            m_lnqCheck.StorageID = UniversalFunction.GetStorageID(cmbStorage.Text);
        }

        /// <summary>
        /// 数据库操作
        /// </summary>
        /// <param name="strFlag">标志状态</param>
        private void UpdateStatus(string strFlag)
        {
            if (lblBillStatus.Text != "单据已完成" && lblBillStatus.Text != "单据已报废")
            {
                GetMessage();

                if (m_serverStroageCheck.UpdateBill(m_lnqCheck.DJH,strFlag,out m_err))
                {
                    if (strFlag == "等待仓管确认")
                    {
                        m_billNoControl.UseBill(m_lnqCheck.DJH);
                    }
                    MessageDialog.ShowPromptMessage("操作成功");

                }
                else
                {
                    MessageDialog.ShowPromptMessage(m_err);
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
            }

            RefreshDataGirdView(m_serverStroageCheck.GetAllBill());
            PositioningRecord(m_lnqCheck.DJH);
        }

        private void 设置盘点明细ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CheckDate())
            {
                if (lblBillStatus.Text == "单据已完成")
                {
                    MessageDialog.ShowPromptMessage("单据已完成不能设置盘点明细");
                    return;
                }

                if (cmbPDFS.Text == "分类别盘点"
                    && lblBillStatus.Text == "新建单据"
                    && m_serverStroageCheck.GetList(txtBill_ID.Text).Count == 0)
                {
                    MessageDialog.ShowPromptMessage("请先设置分类！");
                    类别选择窗体 form = new 类别选择窗体();
                    form.ShowDialog();
                    m_dtDepot = form.DtNodeTag;
                }

                if (m_serverStroageCheck.GetBill(txtBill_ID.Text) == null)
                {
                    GetMessage();
                    if (!m_serverStroageCheck.AddBill(m_lnqCheck, null, out m_err))
                    {
                        MessageDialog.ShowPromptMessage(m_err);
                    }
                }


                //Show界面
                库房盘点表 formPD = new 库房盘点表(txtBill_ID.Text,m_dtDepot, m_authFlag,"0");
                formPD.ShowDialog();

                m_dtMx = formPD.m_dtMx;

                //对明细表的信息进行更新
                if (formPD.m_intFlag == 1)
                {
                    //添加一条主表信息
                    GetMessage();

                    if (!m_serverStroageCheck.AddBill(m_lnqCheck, m_dtMx, out m_err))
                    {
                        MessageDialog.ShowPromptMessage(m_err);
                    }
                }

                RefreshDataGirdView(m_serverStroageCheck.GetAllBill());
                PositioningRecord(m_lnqCheck.DJH);
            }
        }

        private void 新建单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cmbStorage.SelectedIndex = -1;
            cmbPDFS.SelectedIndex = -1;
            lblBillStatus.Text = "新建单据";
            txtBill_ID.Text = m_billNoControl.GetNewBillNo();
        }

        private void 提交单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lblBillStatus.Text == "新建单据")
            {
                UpdateStatus(lblBillStatus.Text);
                m_billMessageServer.DestroyMessage(txtBill_ID.Text);
                m_billMessageServer.SendNewFlowMessage(txtBill_ID.Text,
                    string.Format("{0} 号库房盘点单，请主管审核", txtBill_ID.Text), BillFlowMessage_ReceivedUserType.角色,
                    m_billMessageServer.GetSuperior(CE_RoleStyleType.上级领导, BasicInfo.LoginID));
            }
            else
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
            }
        }

        private void 部门主管审核ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lblBillStatus.Text == "等待主管审核")
            {
                UpdateStatus(lblBillStatus.Text);
                string msg = string.Format("{0} 号库房盘点单已由主管审核，请负责人批准", m_lnqCheck.DJH);

                m_billMessageServer.PassFlowMessage(m_lnqCheck.DJH, msg, CE_RoleEnum.生产管理部负责人.ToString(), true);
            }
            else
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
            }
        }

        private void 批准通过ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lblBillStatus.Text == "等待财务批准")
            {
                UpdateStatus(lblBillStatus.Text);

                string msg = string.Format("{0} 号库房盘点单已由分管领导批准，请仓管确认", m_lnqCheck.DJH);

                m_billMessageServer.PassFlowMessage(m_lnqCheck.DJH, msg, 
                    m_billMessageServer.GetRoleStringForStorage(cmbStorage.Text).ToString(), true);
            }
            else
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
            }
        }

        private void 盘点确认ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lblBillStatus.Text == "等待仓管确认")
            {
                UpdateStatus(lblBillStatus.Text);

                #region 发送知会消息

                List<string> noticeRoles = new List<string>();

                string strDept = m_serverDepartment.GetDeptInfoFromPersonnelInfo(
                    m_serverStroageCheck.GetBill(txtBill_ID.Text).BZRY).Rows[0]["DepartmentCode"].ToString();

                noticeRoles.AddRange(m_billMessageServer.GetDeptDirectorRoleName(strDept));

                noticeRoles.Add(CE_RoleEnum.制造分管领导.ToString());
                noticeRoles.Add(m_billMessageServer.GetRoleStringForStorage(cmbStorage.Text).ToString());
                noticeRoles.Add(CE_RoleEnum.财务主管.ToString());
                noticeRoles.Add(CE_RoleEnum.会计.ToString());

                m_billMessageServer.EndFlowMessage(m_lnqCheck.DJH,
                    string.Format("{0} 号库房盘点单已经处理完毕", m_lnqCheck.DJH),
                    noticeRoles, null);

                #endregion 发送知会消息
               
            }
            else
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
            }
        }

        private void 删除单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (BasicInfo.LoginName != dataGridView1.CurrentRow.Cells["编制人员"].Value.ToString())
            {
                MessageDialog.ShowPromptMessage("您不是此记录的编制者无法进行此操作");
                return;
            }

            if (dataGridView1.CurrentRow.Cells["单据号"].Value.ToString() == "")
            {
                MessageDialog.ShowPromptMessage("请选择需要删除的记录");
                return;
            }

            if (UniversalFunction.GetBillStatus("S_StorageCheck", "DJZT", "DJH",
                    dataGridView1.CurrentRow.Cells["单据号"].Value.ToString()) != "单据已完成")
            {
                GetMessage();

                if (MessageDialog.ShowEnquiryMessage("您是否要删除此单据") == DialogResult.No)
                {
                    return;
                }

                if (m_serverStroageCheck.DeleteBill(m_lnqCheck.DJH, out m_err))
                {
                    m_billNoControl.CancelBill(m_lnqCheck.DJH);
                    MessageDialog.ShowPromptMessage("操作成功");
                    m_billMessageServer.DestroyMessage(m_lnqCheck.DJH);
                }
                else
                {
                    MessageDialog.ShowPromptMessage(m_err);
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
            }

            RefreshDataGirdView(m_serverStroageCheck.GetAllBill());
            PositioningRecord(m_lnqCheck.DJH);
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.DataSource == null || dataGridView1.CurrentRow == null)
            {
                return;
            }
            else
            {
                仓管员操作ToolStripMenuItem.Visible = UniversalFunction.CheckStorageAndPersonnel(
                    dataGridView1.CurrentRow.Cells["库房代码"].Value.ToString());
                lblBillStatus.Text = dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString();
                txtBill_ID.Text = dataGridView1.CurrentRow.Cells["单据号"].Value.ToString();
                cmbPDFS.Text = dataGridView1.CurrentRow.Cells["单据方式"].Value.ToString();
                cmbStorage.Text = dataGridView1.CurrentRow.Cells["所属库房"].Value.ToString();
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            库房盘点表 formPD = new 库房盘点表(txtBill_ID.Text, null, m_authFlag,"1");
            formPD.ShowDialog();
        }

        private void 打印单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IBillReportInfo report = new 报表_盘点单(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString(), labelTitle.Text);
            PrintReportBill print = new PrintReportBill(21, 29.7, report);
            print.DirectPrintReport();
        }

        private void 分管领导批准ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lblBillStatus.Text == "等待负责人批准")
            {
                UpdateStatus(lblBillStatus.Text);
            }
            else
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
            }
        }

        private void 刷新数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshDataGirdView(m_serverStroageCheck.GetAllBill());
            PositioningRecord(m_lnqCheck.DJH);
        }

        private void lblBillStatus_TextChanged(object sender, EventArgs e)
        {
            if (lblBillStatus.Text.Trim() == "单据已完成")
            {
                cmbStorage.Enabled = false;
                cmbPDFS.Enabled = false;
            }
            else
            {
                cmbStorage.Enabled = true;
                cmbPDFS.Enabled = true;
            }
        }
    }
}
