using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using GlobalObject;
using PlatformManagement;
using UniversalControlLibrary;

namespace Expression
{
    public partial class 设备维修安装申请单 : Form
    {
        /// <summary>
        /// 设备维修安装服务组件
        /// </summary>
        IDeviceMaintenanceBill m_serverDeviceMaintenance = ServerModuleFactory.GetServerModule<IDeviceMaintenanceBill>();

        /// <summary>
        /// LINQ数据集
        /// </summary>
        S_DeviceMaintenanceBill m_lnqDeviceMaintenance = new S_DeviceMaintenanceBill();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr;

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 人员服务组件
        /// </summary>
        IPersonnelInfoServer m_serverPersonnel = ServerModuleFactory.GetServerModule<IPersonnelInfoServer>();

        /// <summary>
        /// 部门服务组件
        /// </summary>
        IDepartmentServer m_serverDept = ServerModuleFactory.GetServerModule<IDepartmentServer>();

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        public 设备维修安装申请单(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();
            m_billMessageServer.BillType = "设备维修安装申请单";
            m_authFlag = nodeInfo.Authority;
            AuthorityControl(m_authFlag);
            m_billNoControl = new BillNumberControl(CE_BillTypeEnum.设备维修安装申请单, m_serverDeviceMaintenance);

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

                    DataTable dtMessage = UniversalFunction.PositioningOneRecord(msg.MessageContent, "设备维修安装申请单");

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
        /// 获得数据
        /// </summary>
        void GetData()
        {
            m_lnqDeviceMaintenance.Attendant = lbAttendant.Text;
            m_lnqDeviceMaintenance.AttendantDate = lbAttendantDate.Text == "" ? ServerTime.Time : Convert.ToDateTime(lbAttendantDate.Text);
            m_lnqDeviceMaintenance.Bill_ID = txtBillNo.Text;
            m_lnqDeviceMaintenance.BillStatus = lbBillStatus.Text;
            m_lnqDeviceMaintenance.Confirmor = BasicInfo.LoginName;
            m_lnqDeviceMaintenance.ConfirmorDate = ServerTime.Time;
            m_lnqDeviceMaintenance.DeviceCode = txtDeviceCode.Text;
            m_lnqDeviceMaintenance.DeviceName = txtDeviceName.Text;
            m_lnqDeviceMaintenance.FaultDescription = txtFaultDescription.Text;
            m_lnqDeviceMaintenance.MaintenanceCondition = txtMaintenanceCondition.Text;
            m_lnqDeviceMaintenance.Proposer = lbProposer.Text;
            m_lnqDeviceMaintenance.ProposerDate = lbProposerDate.Text == "" ? ServerTime.Time : Convert.ToDateTime(lbProposerDate.Text);
            m_lnqDeviceMaintenance.ReplacementParts = txtReplacementParts.Text;
            m_lnqDeviceMaintenance.Reviewers = lbReviewers.Text;
            m_lnqDeviceMaintenance.ReviewersDate = lbReviewersDate.Text == "" ? ServerTime.Time : Convert.ToDateTime(lbReviewersDate.Text);
            m_lnqDeviceMaintenance.ServiceEvaluation = txtServiceEvaluation.Text;
            m_lnqDeviceMaintenance.UseDept = txtDept.Tag.ToString();

            m_lnqDeviceMaintenance.DeviceDamageTime = dtpDeviceDamageTime.Value;
            m_lnqDeviceMaintenance.DeviceNormalUseTime = dtpDeviceNormalUseTime.Value;


        }

        /// <summary>
        /// 显示数据
        /// </summary>
        void ShowData()
        {
            lbBillStatus.Text = m_lnqDeviceMaintenance.BillStatus;

            dtpDeviceDamageTime.Value = m_lnqDeviceMaintenance.DeviceDamageTime == null ?
                ServerTime.Time : (DateTime)m_lnqDeviceMaintenance.DeviceDamageTime;
            dtpDeviceNormalUseTime.Value = m_lnqDeviceMaintenance.DeviceNormalUseTime == null ?
                ServerTime.Time : (DateTime)m_lnqDeviceMaintenance.DeviceNormalUseTime;

            txtServiceEvaluation.Text = m_lnqDeviceMaintenance.ServiceEvaluation;
            txtReplacementParts.Text = m_lnqDeviceMaintenance.ReplacementParts;
            txtMaintenanceCondition.Text = m_lnqDeviceMaintenance.MaintenanceCondition;
            txtFaultDescription.Text = m_lnqDeviceMaintenance.FaultDescription;
            txtDeviceName.Text = m_lnqDeviceMaintenance.DeviceName;
            txtDeviceCode.Text = m_lnqDeviceMaintenance.DeviceCode;
            txtDept.Tag = m_lnqDeviceMaintenance.UseDept;
            txtDept.Text = m_serverDept.GetDepartmentName(txtDept.Tag.ToString());
            txtBillNo.Text = m_lnqDeviceMaintenance.Bill_ID;

            lbAttendant.Text = m_lnqDeviceMaintenance.Attendant;
            lbAttendantDate.Text = m_lnqDeviceMaintenance.AttendantDate == null ? "" : m_lnqDeviceMaintenance.AttendantDate.ToString();
            lbProposer.Text = m_lnqDeviceMaintenance.Proposer;
            lbProposerDate.Text = m_lnqDeviceMaintenance.ProposerDate == null ? "" : m_lnqDeviceMaintenance.ProposerDate.ToString();
            lbReviewers.Text = m_lnqDeviceMaintenance.Reviewers;
            lbReviewersDate.Text = m_lnqDeviceMaintenance.ReviewersDate == null ? "" : m_lnqDeviceMaintenance.ReviewersDate.ToString();
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        void ClearData()
        {
            m_lnqDeviceMaintenance = new S_DeviceMaintenanceBill();

            txtBillNo.Text = "";
            txtDept.Text = "";
            txtDept.Tag = -1;
            txtDeviceCode.Text = "";
            txtDeviceName.Text = "";
            txtFaultDescription.Text = "";
            txtMaintenanceCondition.Text = "";
            txtReplacementParts.Text = "";
            txtServiceEvaluation.Text = "";
            dtpDeviceNormalUseTime.Value = ServerTime.Time;
            dtpDeviceDamageTime.Value = ServerTime.Time;
            lbAttendant.Text = "";
            lbAttendantDate.Text = "";
            lbBillStatus.Text = "";
            lbProposer.Text = "";
            lbProposerDate.Text = "";
            lbReviewers.Text = "";
            lbReviewersDate.Text = "";
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        void RefreshData()
        {
            dataGridView1.DataSource =
                m_serverDeviceMaintenance.GetAllInfo(checkBillDateAndStatus1.dtpStartTime.Value,
                checkBillDateAndStatus1.dtpEndTime.Value, checkBillDateAndStatus1.cmbBillStatus.Text);

            dataGridView1.Columns["单据号"].Width = 150;

            userControlDataLocalizer1.Init(this.dataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
        }

        /// <summary>
        /// 操作流程
        /// </summary>
        /// <returns></returns>
        bool FlowBill()
        {
            GetData();

            if (m_lnqDeviceMaintenance.BillStatus == "单据已完成")
            {
                MessageDialog.ShowPromptMessage("单据已完成，不能被操作");
                return false;
            }

            if (!m_serverDeviceMaintenance.FlowInfo(m_lnqDeviceMaintenance,out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
                return false;
            }

            return true;
        }

        private void checkBillDateAndStatus1_OnCompleteSearch()
        {
            RefreshData();
        }

        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void 新建单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearData();

            lbBillStatus.Text = "新建单据";
            txtBillNo.Text = m_billNoControl.GetNewBillNo();
            txtDept.Text = BasicInfo.DeptName;
            txtDept.Tag = BasicInfo.DeptCode;
            lbProposer.Text = BasicInfo.LoginName;
            lbProposerDate.Text = ServerTime.Time.ToString();
            dtpDeviceDamageTime.Value = ServerTime.Time;
        }

        private void 提交单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbBillStatus.Text != "新建单据")
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
                return;
            }

            if (FlowBill())
            {
                MessageDialog.ShowPromptMessage("提交成功");

                m_billMessageServer.DestroyMessage(m_lnqDeviceMaintenance.Bill_ID);
                m_billMessageServer.SendNewFlowMessage(m_lnqDeviceMaintenance.Bill_ID,
                    string.Format("{0}号设备维修安装申请单已提交，请设备组人员及时处理", m_lnqDeviceMaintenance.Bill_ID), CE_RoleEnum.设备组员);
            }

            string strBillNo = m_lnqDeviceMaintenance.Bill_ID;
            RefreshData();
            PositioningRecord(strBillNo);
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
                MessageDialog.ShowPromptMessage("您不是此单据的编制人，不能执行删除功能");
                return;
            }

            string strBillNo = dataGridView1.CurrentRow.Cells["单据号"].Value.ToString();

            if (MessageDialog.ShowEnquiryMessage(string.Format("是否要删除{0}号单据",strBillNo)) == DialogResult.Yes)
            {
                m_serverDeviceMaintenance.DeleteBill(strBillNo);
                m_billNoControl.CancelBill(strBillNo);
                m_billMessageServer.DestroyMessage(strBillNo);

                MessageDialog.ShowPromptMessage("删除成功");
            }

            RefreshData();
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }
            else
            {
                ClearData();


                if (dataGridView1.CurrentRow.Cells["设备故障日期"].Value != DBNull.Value)
                {
                    m_lnqDeviceMaintenance.DeviceDamageTime = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["设备故障日期"].Value);
                }

                if (dataGridView1.CurrentRow.Cells["设备可正常使用日期"].Value != DBNull.Value)
                {
                    m_lnqDeviceMaintenance.DeviceNormalUseTime = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["设备可正常使用日期"].Value);
                }

                m_lnqDeviceMaintenance.Attendant = dataGridView1.CurrentRow.Cells["维修指派人"].Value.ToString();

                if (dataGridView1.CurrentRow.Cells["维修指派日期"].Value != DBNull.Value)
                {
                    m_lnqDeviceMaintenance.AttendantDate = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["维修指派日期"].Value);
                }

                m_lnqDeviceMaintenance.Bill_ID = dataGridView1.CurrentRow.Cells["单据号"].Value.ToString();
                m_lnqDeviceMaintenance.BillStatus = dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString();
                m_lnqDeviceMaintenance.Confirmor = dataGridView1.CurrentRow.Cells["确认人"].Value.ToString();

                if (dataGridView1.CurrentRow.Cells["确认日期"].Value != DBNull.Value)
                {
                    m_lnqDeviceMaintenance.ConfirmorDate = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["确认日期"].Value);
                }

                m_lnqDeviceMaintenance.DeviceCode = dataGridView1.CurrentRow.Cells["设备型号"].Value.ToString();
                m_lnqDeviceMaintenance.DeviceName = dataGridView1.CurrentRow.Cells["设备名称"].Value.ToString();
                m_lnqDeviceMaintenance.FaultDescription = dataGridView1.CurrentRow.Cells["故障现象(安装内容)"].Value.ToString();
                m_lnqDeviceMaintenance.MaintenanceCondition = dataGridView1.CurrentRow.Cells["维修(安装)情况"].Value.ToString();
                m_lnqDeviceMaintenance.Proposer = dataGridView1.CurrentRow.Cells["申请人"].Value.ToString();

                if (dataGridView1.CurrentRow.Cells["申请日期"].Value != DBNull.Value)
                {
                    m_lnqDeviceMaintenance.ProposerDate = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["申请日期"].Value);
                }

                m_lnqDeviceMaintenance.ReplacementParts = dataGridView1.CurrentRow.Cells["更换配件"].Value.ToString();
                m_lnqDeviceMaintenance.Reviewers = dataGridView1.CurrentRow.Cells["评价人"].Value.ToString();

                if (dataGridView1.CurrentRow.Cells["评价日期"].Value != DBNull.Value)
                {
                    m_lnqDeviceMaintenance.ReviewersDate = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["评价日期"].Value);
                }

                m_lnqDeviceMaintenance.ServiceEvaluation = dataGridView1.CurrentRow.Cells["服务评价"].Value.ToString();
                m_lnqDeviceMaintenance.UseDept = m_serverDept.GetDepartmentCode( dataGridView1.CurrentRow.Cells["设备使用部门"].Value.ToString());

                if (m_lnqDeviceMaintenance.UseDept == BasicInfo.DeptCode)
                {
                    评价人操作ToolStripMenuItem.Visible = true;
                }
                else
                {
                    评价人操作ToolStripMenuItem.Visible = false;
                }

                ShowData();
            }
        }

        private void 提交维修记录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbBillStatus.Text != "等待维修")
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
                return;
            }

            if (FlowBill())
            {
                MessageDialog.ShowPromptMessage("提交成功");

                string strWorkID = UniversalFunction.GetPersonnelCode(m_lnqDeviceMaintenance.Proposer);

                m_billMessageServer.PassFlowMessage(m_lnqDeviceMaintenance.Bill_ID,
                    string.Format("{0}号设备维修安装申请单已维修，请使用部门进行评价", m_lnqDeviceMaintenance.Bill_ID), 
                    BillFlowMessage_ReceivedUserType.用户, strWorkID);
            }

            string strBillNo = m_lnqDeviceMaintenance.Bill_ID;
            RefreshData();
            PositioningRecord(strBillNo);
        }

        private void 提交评价信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbBillStatus.Text != "等待评价")
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
                return;
            }

            if (FlowBill())
            {
                MessageDialog.ShowPromptMessage("提交成功");

                m_billMessageServer.PassFlowMessage(m_lnqDeviceMaintenance.Bill_ID,
                    string.Format("{0}号设备维修安装申请单已评价，请设备组组长确认", m_lnqDeviceMaintenance.Bill_ID),
                    CE_RoleEnum.设备组长);
            }

            string strBillNo = m_lnqDeviceMaintenance.Bill_ID;
            RefreshData();
            PositioningRecord(strBillNo);
        }

        private void 确认单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbBillStatus.Text != "等待确认")
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
                return;
            }

            if (FlowBill())
            {
                MessageDialog.ShowPromptMessage("提交成功");

                List<string> listRole = new List<string>();

                listRole.Add(CE_RoleEnum.设备组长.ToString());
                listRole.Add(CE_RoleEnum.设备组员.ToString());

                m_billMessageServer.EndFlowMessage(m_lnqDeviceMaintenance.Bill_ID,
                    string.Format("{0}号设备维修安装申请单已评价，请设备组组长确认", m_lnqDeviceMaintenance.Bill_ID),
                    listRole, null);

                m_billNoControl.UseBill(m_lnqDeviceMaintenance.Bill_ID);
            }

            string strBillNo = m_lnqDeviceMaintenance.Bill_ID;
            RefreshData();
            PositioningRecord(strBillNo);
        }

        private void 设备维修安装申请单_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        private void 导出EXCELToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
        }

        private void 设备维修安装申请单_Load(object sender, EventArgs e)
        {
            string[] strBillStatus = { "全部", 
                                     "新建单据", 
                                     "等待维修",
                                     "等待评价",
                                     "等待确认",
                                     "单据已完成"};

            checkBillDateAndStatus1.InsertComBox(strBillStatus);
            ClearData();
            RefreshData();
            menuStrip.Visible = true;
        }
    }
}
