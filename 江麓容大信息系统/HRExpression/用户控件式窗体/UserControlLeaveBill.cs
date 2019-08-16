using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ServerModule;
using PlatformManagement;
using Expression;
using Service_Peripheral_HR;
using GlobalObject;
using UniversalControlLibrary;

namespace Form_Peripheral_HR
{
    /// <summary>
    /// 请假申请界面
    /// </summary>
    public partial class UserControlLeaveBill : Form
    {
        #region 声明变量
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_error;

        /// <summary>
        /// 请假天数
        /// </summary>
        int m_leaveDays;

        /// <summary>
        /// 最高部门
        /// </summary>
        string m_highDept;

        /// <summary>
        /// 单据编号
        /// </summary>
        string m_billNo;

        /// <summary>
        /// 应上班小时数
        /// </summary>
        double m_workHours;

        /// <summary>
        /// 中午休息小时数
        /// </summary>
        double m_restHours;

        /// <summary>
        /// 应打卡时间
        /// </summary>
        string punchInTime;

        /// <summary>
        /// 下午上班打卡时间
        /// </summary>
        string m_punchInAfternoon;

        /// <summary>
        /// 下午下班打卡时间
        /// </summary>
        string m_punchInAfternoonEnd;

        /// <summary>
        /// 上午下班打卡时间
        /// </summary>
        string m_punchInMorningEnd;

        /// <summary>
        /// 上午上班打卡时间
        /// </summary>
        string m_punchInMorning;

        /// <summary>
        /// 小时数
        /// </summary>
        double m_hours = 0;

        /// <summary>
        /// 一个月的天数
        /// </summary>
        int m_days = 31;

        /// <summary>
        /// 操作权限
        /// </summary>
        PlatformManagement.AuthorityFlag m_authorityFlag;

        /// <summary>
        /// 可供查找的所有字段
        /// </summary>
        string[] m_findField = null;

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 人员档案服务组件
        /// </summary>
        IPersonnelArchiveServer m_personnerServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IPersonnelArchiveServer>();

        /// <summary>
        /// 加班申请操作类
        /// </summary>
        IOverTimeBillServer m_overTimeServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IOverTimeBillServer>();

        /// <summary>
        /// 考勤方案操作类
        /// </summary>
        IAttendanceSchemeServer m_attendanceSchemeServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IAttendanceSchemeServer>();

        /// <summary>
        /// 考勤分析汇总操作接口
        /// </summary>
        IAttendanceSummaryServer m_attendanceServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IAttendanceSummaryServer>();

        /// <summary>
        /// 考勤异常登记操作类
        /// </summary>
        ITimeExceptionServer m_timeExServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<ITimeExceptionServer>();

        /// <summary>
        /// 人员考勤流水账操作类
        /// </summary>
        IAttendanceDaybookServer m_dayBookServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IAttendanceDaybookServer>();

        /// <summary>
        /// 考勤机导入的人员考勤明细表操作类
        /// </summary>
        IAttendanceMachineServer m_attendanceMachineServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IAttendanceMachineServer>();

        /// <summary>
        /// 请假操作类
        /// </summary>
        ILeaveServer m_leaveServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<ILeaveServer>();

        /// <summary>
        /// 节假日管理类
        /// </summary>
        IHolidayServer m_holidayServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IHolidayServer>();

        #endregion

        public UserControlLeaveBill(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_billMessageServer.BillType = "请假申请单";

            m_authorityFlag = nodeInfo.Authority;

            #region 数据筛选
            string[] strBillStatus = {"全部",  LeaveBillStatus.新建单据.ToString(),
                                               LeaveBillStatus.等待主管审核.ToString(),
                                               LeaveBillStatus.等待部门负责人审核.ToString(),
                                               LeaveBillStatus.等待分管领导审批.ToString(),
                                               LeaveBillStatus.等待总经理审批.ToString(),
                                               LeaveBillStatus.等待人力资源复核.ToString(),
                                               LeaveBillStatus.已完成.ToString()};

            checkBillDateAndStatus1.InsertComBox(strBillStatus);

            checkBillDateAndStatus1.OnCompleteSearch +=
                new GlobalObject.DelegateCollection.NonArgumentHandle(checkBillDateAndStatus1_OnCompleteSearch);

            checkBillDateAndStatus1.dtpStartTime.Value = ServerTime.Time.AddDays(1).AddMonths(-1);
            checkBillDateAndStatus1.dtpEndTime.Value = ServerTime.Time.AddDays(1);

            #endregion

            DataTable dt = m_leaveServer.GetLeaveTypeByCode(null);

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cmbLeaveTypeID.Items.Add(dt.Rows[i]["请假类别"].ToString());
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请假类别没有数据，请通知人力资源部添加！");
                return;
            }

            RefreshDataGridView();

            IQueryable<HR_AttendanceScheme> dtScheme = m_attendanceSchemeServer.GetLinqResult();

            string punchInMorning = dtScheme.Take(1).Single().BeginTimeInTheMorning.Value.Hour.ToString() +
                ":" + dtScheme.Take(1).Single().BeginTimeInTheMorning.Value.Minute.ToString();

            string punchInMorningEnd = dtScheme.Take(1).Single().EndTimeInTheMorning.Value.Hour.ToString() +
                ":" + dtScheme.Take(1).Single().EndTimeInTheMorning.Value.Minute.ToString();

            string punchInAfternoonEnd = dtScheme.Take(1).Single().EndTimeInTheAfternoon.Value.Hour.ToString() +
                ":" + dtScheme.Take(1).Single().EndTimeInTheAfternoon.Value.Minute.ToString();

            string punchInAfternoon = dtScheme.Take(1).Single().BeginTimeInTheAfternoon.Value.Hour.ToString() +
                ":" + dtScheme.Take(1).Single().BeginTimeInTheAfternoon.Value.Minute.ToString();

            double workHours = (Convert.ToDateTime(punchInMorningEnd) - Convert.ToDateTime(punchInMorning)).Hours;
            workHours += Convert.ToDouble((Convert.ToDateTime(punchInMorningEnd) - Convert.ToDateTime(punchInMorning)).Minutes) / 60;
            workHours += (Convert.ToDateTime(punchInAfternoonEnd) - Convert.ToDateTime(punchInAfternoon)).Hours;
            workHours += Convert.ToDouble((Convert.ToDateTime(punchInAfternoonEnd) - Convert.ToDateTime(punchInAfternoon)).Minutes) / 60;
            double restHours = (Convert.ToDateTime(punchInAfternoon) - Convert.ToDateTime(punchInMorningEnd)).Hours;
            restHours += Convert.ToDouble((Convert.ToDateTime(punchInAfternoon) - Convert.ToDateTime(punchInMorningEnd)).Minutes) / 60;

            m_punchInMorning = punchInMorning;
            m_punchInMorningEnd = punchInMorningEnd;
            m_punchInAfternoonEnd = punchInAfternoonEnd;
            m_punchInAfternoon = punchInAfternoon;
            m_workHours = workHours;
            m_restHours = restHours;
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

                    DataTable dtMessage = UniversalFunction.PositioningOneRecord(msg.MessageContent, "请假申请单");

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
        /// 刷新
        /// </summary>
        private void RefreshDataGridView()
        {
            m_leaveServer.QueryResultFilter = QueryFilterControl.GetFilterString(labelTitle.Text)
                      + checkBillDateAndStatus1.GetSqlString("申请日期", "单据状态");

            IQueryResult result;

            if (!m_leaveServer.GetAllLeaveBill(out result, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            dataGridView1.DataSource = result.DataCollection.Tables[0];

            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Columns["员工编号"].Visible = false;
                dataGridView1.Columns["部门编码"].Visible = false;
            }

            this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            // 添加查询用的列
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

            dataGridView1.Refresh();
        }

        private void 综合查询toolStripButton3_Click(object sender, EventArgs e)
        {
            IAuthorization authorization = PlatformFactory.GetObject<IAuthorization>();
            string businessID = "请假单综合查询";
            IQueryResult qr = authorization.Query(businessID, null, null, 0);
            List<string> lstFindField = new List<string>();
            DataColumnCollection columns = qr.DataCollection.Tables[0].Columns;

            if (qr.Succeeded && columns.Count > 0)
            {
                for (int i = 0; i < columns.Count; i++)
                {
                    lstFindField.Add(columns[i].ColumnName);
                }
            }

            FormConditionFind formFindCondition = new FormConditionFind(this, lstFindField.ToArray(), businessID, labelTitle.Text);
            formFindCondition.ShowDialog();
        }

        private void 人力补单toolStripButton_Click(object sender, EventArgs e)
        {
            人力补单toolStripButton.Checked = !人力补单toolStripButton.Checked;

            if (人力补单toolStripButton.Checked)
            {
                txtApplicant.ReadOnly = false;
                txtApplicant.ShowResultForm = true;
            }
            else
            {
                txtApplicant.ReadOnly = true;
                txtApplicant.ShowResultForm = false;
            }
        }

        private void 人力资源toolStripButton3_Click(object sender, EventArgs e)
        {
            if (lblStatus.Text.Trim() != LeaveBillStatus.等待人力资源复核.ToString())
            {
                MessageDialog.ShowPromptMessage("请确认单据状态！");
                return;
            }

            string[] type = cmbLeaveTypeID.Text.Split(' ');

            if (type[1] != dataGridView1.CurrentRow.Cells["请假类别"].Value.ToString())
            {
                if (MessageDialog.ShowEnquiryMessage("确定修改申请人的请假类别吗？") == DialogResult.No)
                {
                    return;
                }
            }

            //if (!CheckContrl())
            //{
            //    return;
            //}

            HR_LeaveBill leave = new HR_LeaveBill();

            leave.ID = Convert.ToInt32(m_billNo);
            leave.HR_Signature = BasicInfo.LoginID;
            leave.HR_SignatureDate = ServerTime.Time;
            leave.BillStatus = LeaveBillStatus.已完成.ToString();
            leave.LeaveTypeID = type[0];

            if (!m_leaveServer.UpdateLeave(leave, "人力资源部复审", out m_error))
            {
                MessageDialog.ShowPromptMessage(m_error);
                return;
            }

            List<string> noticeRoles = new List<string>();
            List<string> noticeUser = new List<string>();

            noticeRoles.Add(m_billMessageServer.GetDeptDirectorRoleName(m_highDept)[0]);
            noticeUser.Add(txtApplicant.Tag.ToString());

            m_billMessageServer.EndFlowMessage(m_billNo, string.Format("{0} 号请假申请已经处理完毕", m_billNo), noticeRoles, noticeUser);

            RefreshDataGridView();
            PositioningRecord(m_billNo);
        }

        private void 刷新toolStripButton1_Click(object sender, EventArgs e)
        {
            RefreshDataGridView();
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }

            try
            {

                txtApplicant.Text = dataGridView1.CurrentRow.Cells["申请人"].Value.ToString();
                txtApplicant.Tag = dataGridView1.CurrentRow.Cells["员工编号"].Value.ToString();
                txtDeptDirector.Text = dataGridView1.CurrentRow.Cells["部门主管"].Value.ToString();
                txtDeptPrincipal.Text = dataGridView1.CurrentRow.Cells["部门负责人"].Value.ToString();
                txtGeneralManager.Text = dataGridView1.CurrentRow.Cells["总经理"].Value.ToString();
                txtHR.Text = dataGridView1.CurrentRow.Cells["人力资源"].Value.ToString();
                txtLeader.Text = dataGridView1.CurrentRow.Cells["分管领导"].Value.ToString();
                txtOtherExplanation.Text = dataGridView1.CurrentRow.Cells["其他说明"].Value.ToString();
                txtReason.Text = dataGridView1.CurrentRow.Cells["请假原因"].Value.ToString();
                txtUnexcusedReason.Text = dataGridView1.CurrentRow.Cells["不批准理由"].Value.ToString();

                dtpBeginTime.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["请假时间"].Value);

                if (dataGridView1.CurrentRow.Cells["申请日期"].Value.ToString() != "")
                {
                    dtpDate.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["申请日期"].Value);
                }
                else
                {
                    dtpDate.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["请假时间"].Value);
                }

                dtpEndTime.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["终止时间"].Value);

                if (dataGridView1.CurrentRow.Cells["签定时间"].Value.ToString() != "")
                {
                    dtpDeptDirectorDate.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["签定时间"].Value);
                }

                if (dataGridView1.CurrentRow.Cells["负责人签定时间"].Value.ToString() != "")
                {
                    dtpDeptPrincipalDate.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["负责人签定时间"].Value);
                }

                if (dataGridView1.CurrentRow.Cells["总经理签定时间"].Value.ToString() != "")
                {
                    dtpGMDate.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["总经理签定时间"].Value);
                }

                if (dataGridView1.CurrentRow.Cells["人力资源签定时间"].Value.ToString() != "")
                {
                    dtpHRDate.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["人力资源签定时间"].Value);
                }

                if (dataGridView1.CurrentRow.Cells["领导签定时间"].Value.ToString() != "")
                {
                    dtpLeaderDate.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["领导签定时间"].Value);
                }

                cmbLeaveTypeID.Text = m_leaveServer.GetLeaveTypeByCode_Show(
                    dataGridView1.CurrentRow.Cells["请假类别"].Value.ToString()).Rows[0]["请假类别"].ToString();
                cbAuthorize.Checked = Convert.ToBoolean(dataGridView1.CurrentRow.Cells["是否批准"].Value);
                lblStatus.Text = dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString();
                m_billNo = dataGridView1.CurrentRow.Cells["单据号"].Value.ToString();
                txtHours.Text = dataGridView1.CurrentRow.Cells["实际小时数"].Value.ToString();
                m_hours = Convert.ToDouble(txtHours.Text);
                cbAuthorize.Visible = true;
                lbUnexcusedReason.Visible = true;
                txtUnexcusedReason.Visible = true;

                DataTable dt = m_personnerServer.GetHighestDept(txtApplicant.Tag.ToString());

                if (dt != null && dt.Rows.Count > 0)
                {
                    m_highDept = dt.Rows[0]["deptCode"].ToString();
                }

                m_days = GetDays();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
                throw;
            }
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Cells["单据状态"].Value.ToString() != "已完成")
                    {
                        dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.Red;
                    }
                }
            }
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        private void 总经理toolStripButton2_Click(object sender, EventArgs e)
        {
            if (lblStatus.Text.Trim() != LeaveBillStatus.等待总经理审批.ToString())
            {
                MessageDialog.ShowPromptMessage("请确认单据状态！");
                return;
            }

            HR_LeaveBill leave = new HR_LeaveBill();

            leave.ID = Convert.ToInt32(m_billNo);
            leave.GeneralManager = BasicInfo.LoginID;
            leave.GM_SignatureDate = ServerTime.Time;

            if (!cbAuthorize.Checked)
            {
                if (MessageDialog.ShowEnquiryMessage("您是否批准【" + txtApplicant.Text + "】的请假申请？") == DialogResult.No)
                {
                    if (txtUnexcusedReason.Text == "")
                    {
                        MessageDialog.ShowPromptMessage("请述明不批准的原因！");
                        return;
                    }
                }
                else
                {
                    cbAuthorize.Checked = true;
                }
            }

            leave.Authorize = cbAuthorize.Checked;

            string[] type = m_leaveServer.GetLeaveTypeByCode(
                    dataGridView1.CurrentRow.Cells["请假类别"].Value.ToString()).Rows[0]["请假类别"].ToString().Split(' ');
            DataTable dt = m_leaveServer.GetLeaveType(type[0]);

            if (Convert.ToBoolean(dt.Rows[0]["是否需附件证明"].ToString()) || type[0].Equals("0014"))
            {
                leave.BillStatus = LeaveBillStatus.等待人力资源复核.ToString();
            }
            else
            {
                leave.BillStatus = LeaveBillStatus.已完成.ToString();
            }

            if (!m_leaveServer.UpdateLeave(leave, "总经理审批", out m_error))
            {
                MessageDialog.ShowPromptMessage(m_error);
                return;
            }

            string msg = string.Format("{0} 号请假单总经理审批成功", m_billNo);

            if (leave.BillStatus.Equals(LeaveBillStatus.等待人力资源复核.ToString()))
            {
                m_billMessageServer.PassFlowMessage(m_billNo, msg + "等待人力资源部复核", BillFlowMessage_ReceivedUserType.角色, CE_RoleEnum.人力资源部办公室文员.ToString());
            }
            else if (leave.BillStatus.Equals(LeaveBillStatus.已完成.ToString()))
            {
                List<string> noticeRoles = new List<string>();
                List<string> noticeUser = new List<string>();

                noticeRoles.Add(m_billMessageServer.GetDeptDirectorRoleName(m_highDept)[0]);
                noticeUser.Add(txtApplicant.Tag.ToString());

                m_billMessageServer.EndFlowMessage(m_billNo, string.Format("{0} 号请假申请已经处理完毕", m_billNo), noticeRoles, noticeUser);
            }

            RefreshDataGridView();
            PositioningRecord(m_billNo);
        }

        private void dtpEndTime_ValueChanged(object sender, EventArgs e)
        {
            //在没有设置夏季时间却要请假(暂时)
            if (dtpEndTime.Value.Month > 4 && dtpEndTime.Value.Month < 10)
            {
                m_punchInAfternoon = "13:30:00";
                m_punchInAfternoonEnd = "18:00:00";
                m_restHours = 1.5;
            }
            else if (dtpEndTime.Value.Month > 9 || dtpEndTime.Value.Month < 5)
            {
                m_punchInAfternoon = "13:00:00";
                m_punchInAfternoonEnd = "17:30:00";
                m_restHours = 1;
            }
            
            HR_AttendanceSetting personnelSetting = m_attendanceSchemeServer.GetAttendanceSettingByWorkID(txtApplicant.Tag.ToString());
            HR_AttendanceScheme scheme = m_attendanceSchemeServer.GetAttendanceSchemeByCode(personnelSetting.SchemeCode);

            if (!scheme.SchemeName.Contains("排班"))
            {
                if (Convert.ToDateTime(dtpEndTime.Value.ToShortTimeString()) > Convert.ToDateTime(m_punchInAfternoonEnd))
                {
                    dtpEndTime.Value = Convert.ToDateTime(dtpEndTime.Value.ToShortDateString() + " " + m_punchInAfternoonEnd);
                }

                if (Convert.ToDateTime(dtpEndTime.Value.ToShortTimeString()) < Convert.ToDateTime(m_punchInMorning))
                {
                    dtpEndTime.Value = Convert.ToDateTime(dtpEndTime.Value.ToShortDateString() + " " + m_punchInMorning);
                }

                if (Convert.ToDateTime(dtpEndTime.Value.ToShortTimeString()) > Convert.ToDateTime(m_punchInMorningEnd)
                    && Convert.ToDateTime(dtpEndTime.Value.ToShortTimeString()) < Convert.ToDateTime(m_punchInAfternoon))
                {
                    dtpEndTime.Value = Convert.ToDateTime(dtpEndTime.Value.ToShortDateString() + " " + m_punchInMorningEnd);
                }
            }

            txtHours.Text = GetHours_By_CJB();
            m_days = GetDays();
        }

        private void dtpBeginTime_ValueChanged(object sender, EventArgs e)
        {
            //在没有设置夏季时间却要请假（暂时）
            if (dtpBeginTime.Value.Month > 4 && dtpBeginTime.Value.Month < 10)
            {
                m_punchInAfternoon = "13:30:00";
                m_punchInAfternoonEnd = "18:00:00";
                m_restHours = 1.5;
            }
            else if (dtpEndTime.Value.Month > 9 || dtpEndTime.Value.Month < 5)
            {
                m_punchInAfternoon = "13:00:00";
                m_punchInAfternoonEnd = "17:30:00";
                m_restHours = 1;
            }

            dtpBeginTime.Value = Convert.ToDateTime(dtpBeginTime.Value.Year + "-" + dtpBeginTime.Value.Month + "-" +
                dtpBeginTime.Value.Day + " " + dtpBeginTime.Value.Hour + ":" + dtpBeginTime.Value.Minute + ":" + "00");

            HR_AttendanceSetting personnelSetting = m_attendanceSchemeServer.GetAttendanceSettingByWorkID(txtApplicant.Tag.ToString());
            HR_AttendanceScheme scheme = m_attendanceSchemeServer.GetAttendanceSchemeByCode(personnelSetting.SchemeCode);

            if (!scheme.SchemeName.Contains("排班"))
            {

                if (Convert.ToDateTime(dtpBeginTime.Value.ToShortTimeString()) < Convert.ToDateTime(m_punchInMorning))
                {
                    dtpBeginTime.Value = Convert.ToDateTime(dtpBeginTime.Value.ToShortDateString() + " " + m_punchInMorning);
                }

                if (Convert.ToDateTime(dtpBeginTime.Value.ToShortTimeString()) > Convert.ToDateTime(m_punchInAfternoonEnd))
                {
                    dtpBeginTime.Value = Convert.ToDateTime(dtpBeginTime.Value.ToShortDateString() + " " + m_punchInAfternoonEnd);
                }

                if (Convert.ToDateTime(dtpBeginTime.Value.ToShortTimeString()) > Convert.ToDateTime(m_punchInMorningEnd)
                    && Convert.ToDateTime(dtpBeginTime.Value.ToShortTimeString()) < Convert.ToDateTime(m_punchInAfternoon))
                {
                    dtpBeginTime.Value = Convert.ToDateTime(dtpBeginTime.Value.ToShortDateString() + " " + m_punchInMorningEnd);
                }
            }

            if (dtpBeginTime.Value.Hour >= 8 && dtpBeginTime.Value.Hour < 13)
            {
                punchInTime = m_punchInMorning;
            }
            else if (dtpBeginTime.Value.Hour >= 13)
            {
                punchInTime = m_punchInAfternoonEnd;
            }

            txtHours.Text = GetHours_By_CJB();
            m_days = GetDays();
        }

        private void checkBillDateAndStatus1_OnCompleteSearch()
        {
            刷新toolStripButton1_Click(null, null);
        }

        private void 负责人toolStripButton1_Click(object sender, EventArgs e)
        {
            if (lblStatus.Text.Trim() != LeaveBillStatus.等待部门负责人审核.ToString())
            {
                MessageDialog.ShowPromptMessage("请确认单据状态！");
                return;
            }

            HR_LeaveBill leave = new HR_LeaveBill();

            leave.ID = Convert.ToInt32(m_billNo);

            IQueryable<View_HR_PersonnelArchive> directorGroup1 = m_personnerServer.GetDeptDirector(
                m_personnerServer.GetPersonnelInfo(txtApplicant.Tag.ToString()).Dept, "2");
            bool flagPri = false;

            if (directorGroup1 != null && directorGroup1.Count() > 0)
            {
                foreach (var item in directorGroup1)
                {
                    if (BasicInfo.LoginID == item.员工编号)
                    {
                        flagPri = true;

                        break;
                    }
                }
            }

            if (flagPri && Convert.ToDouble(txtHours.Text) > m_workHours * 5)
            {
                leave.BillStatus = LeaveBillStatus.等待总经理审批.ToString();
                leave.Leader = BasicInfo.LoginID;
                leave.LeaderSignatureDate = ServerTime.Time;
            }
            else if (!flagPri && Convert.ToDouble(txtHours.Text) > m_workHours)
            {
                leave.BillStatus = LeaveBillStatus.等待分管领导审批.ToString();
            }
            else
            {
                if (!cbAuthorize.Checked)
                {
                    if (MessageDialog.ShowEnquiryMessage("您是否批准【" + txtApplicant.Text + "】的请假申请？") == DialogResult.No)
                    {
                        if (txtUnexcusedReason.Text == "")
                        {
                            MessageDialog.ShowPromptMessage("请述明不批准的原因！");
                            return;
                        }
                    }
                    else
                    {
                        cbAuthorize.Checked = true;
                    }
                }

                string[] type = m_leaveServer.GetLeaveTypeByCode(
                    dataGridView1.CurrentRow.Cells["请假类别"].Value.ToString()).Rows[0]["请假类别"].ToString().Split(' ');
                DataTable dt = m_leaveServer.GetLeaveType(type[0]);

                if (Convert.ToBoolean(dt.Rows[0]["是否需附件证明"].ToString()) || type[0].Equals("0014"))
                {
                    leave.BillStatus = LeaveBillStatus.等待人力资源复核.ToString();
                }
                else
                {
                    leave.BillStatus = LeaveBillStatus.已完成.ToString();
                }
            }

            leave.Authorize = cbAuthorize.Checked;

            leave.DeptPrincipal = BasicInfo.LoginID;
            leave.DeptPrincipalSignatureDate = ServerTime.Time;

            if (!m_leaveServer.UpdateLeave(leave, "部门负责人审批", out m_error))
            {
                MessageDialog.ShowPromptMessage(m_error);
                return;
            }

            string msg = string.Format("{0} 号请假单部门负责人复审成功", m_billNo);

            if (leave.BillStatus.Equals(LeaveBillStatus.等待人力资源复核.ToString()))
            {
                m_billMessageServer.PassFlowMessage(m_billNo, msg + "等待人力资源部复核",
                    BillFlowMessage_ReceivedUserType.角色, CE_RoleEnum.人力资源部办公室文员.ToString());
            }
            else if (leave.BillStatus.Equals(LeaveBillStatus.已完成.ToString()))
            {
                List<string> noticeUser = new List<string>();

                noticeUser.Add(txtApplicant.Tag.ToString());

                m_billMessageServer.EndFlowMessage(m_billNo, string.Format("{0} 号请假申请已经处理完毕", m_billNo), null, noticeUser);
            }
            else
            {
                m_billMessageServer.PassFlowMessage(m_billNo, string.Format("{0}号请假申请单，请分管领导审核", m_billNo),
                    BillFlowMessage_ReceivedUserType.角色,
                       m_billMessageServer.GetDeptLeaderRoleName(m_personnerServer.GetPersonnelViewInfo(
                       txtApplicant.Tag.ToString()).部门编号).ToList());
            }

            RefreshDataGridView();
            PositioningRecord(m_billNo);
        }

        private void 修改单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dtpBeginTime.Value.Month == ServerTime.Time.Month)
                {
                    m_leaveServer.Check_LeaveType(cmbLeaveTypeID.Text.Split(' ')[0], 
                        Convert.ToDecimal(txtHours.Text), Convert.ToInt32(m_billNo));

                    HR_LeaveBill leave = new HR_LeaveBill();

                    leave.ID = Convert.ToInt32(m_billNo);
                    leave.Applicant = txtApplicant.Tag.ToString();
                    leave.Date = ServerTime.Time;
                    leave.BeginTime = dtpBeginTime.Value;
                    leave.EndTime = dtpEndTime.Value;
                    leave.OtherExplanation = txtOtherExplanation.Text;
                    leave.RealHours = Convert.ToDouble(txtHours.Text);

                    if (Convert.ToDouble(txtHours.Text) > m_hours)
                    {
                        MessageDialog.ShowPromptMessage("请假时间大于原始请假的时间,请重新确认");
                        return;
                    }
                    else
                    {
                        leave.BillStatus = lblStatus.Text;

                        if (!m_leaveServer.AddLeaveBill(leave, out m_error))
                        {
                            MessageDialog.ShowPromptMessage(m_error);
                            return;
                        }

                        MessageDialog.ShowPromptMessage("修改成功！");
                    }
                }
                else
                {
                    MessageDialog.ShowPromptMessage("只能修改本月单据的请假单！");
                    return;
                }

                RefreshDataGridView();
                PositioningRecord(m_billNo);
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                if (MessageBox.Show("是否删除选中的请假单？", "提示", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                if (lblStatus.Text.Trim() != LeaveBillStatus.已完成.ToString())
                {
                    if (!m_leaveServer.DeleteLeaveBill(Convert.ToInt32(m_billNo), out m_error))
                    {
                        MessageDialog.ShowPromptMessage(m_error);
                        return;
                    }

                    m_billMessageServer.BillType = "请假申请单";
                    m_billMessageServer.DestroyMessage(m_billNo);
                }
                else
                {
                    MessageDialog.ShowPromptMessage("单据已完成，不能删除！");
                    return;
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请选择需要删除的一行数据！");
                return;
            }

            RefreshDataGridView();

            if (dataGridView1.Rows.Count == 0)
            {
                ClearControl();
            }
        }

        private void 主管审核toolStripButton1_Click(object sender, EventArgs e)
        {
            if (lblStatus.Text.Trim() != LeaveBillStatus.等待主管审核.ToString())
            {
                MessageDialog.ShowPromptMessage("请确认单据状态！");
                return;
            }

            IQueryable<View_HR_PersonnelArchive> directorGroup = m_personnerServer.GetDeptDirector(
                m_personnerServer.GetPersonnelInfo(txtApplicant.Tag.ToString()).Dept, "0");
            bool flag = false;

            if (directorGroup != null && directorGroup.Count() > 0)
            {
                foreach (var item in directorGroup)
                {
                    if (BasicInfo.LoginID == item.员工编号)
                    {
                        flag = true;
                        break;
                    }
                }
            }

            if (flag)
            {
                HR_LeaveBill leave = new HR_LeaveBill();

                leave.ID = Convert.ToInt32(m_billNo);
                leave.DeptDirector = BasicInfo.LoginID;
                leave.DeptDirectorSignatureDate = ServerTime.Time;
                leave.Authorize = cbAuthorize.Checked;
                leave.BillStatus = LeaveBillStatus.等待部门负责人审核.ToString();

                if (!m_leaveServer.UpdateLeave(leave, "部门主管审批", out m_error))
                {
                    MessageDialog.ShowPromptMessage(m_error);
                    return;
                }

                string msg = string.Format("{0} 号请假单主管审核成功,请部门负责人复审", m_billNo);
                m_billMessageServer.PassFlowMessage(m_billNo, msg,
                         BillFlowMessage_ReceivedUserType.角色, m_billMessageServer.GetDeptPrincipalRoleName(
                         m_personnerServer.GetPersonnelViewInfo(txtApplicant.Tag.ToString()).部门编号).ToList());

                RefreshDataGridView();
                PositioningRecord(m_billNo);
            }
            else
            {
                MessageDialog.ShowPromptMessage("您不是申请人的部门主管（或者您没有审核权限），请等待部门主管审核！");
                return;
            }
        }

        private void 删除单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lblStatus.Text.Trim() == LeaveBillStatus.已完成.ToString())
            {
                MessageDialog.ShowPromptMessage("单据已经完成不能删除！");
                return;
            }

            if (BasicInfo.LoginID == txtApplicant.Tag.ToString() || 人力补单toolStripButton.Checked)
            {
                if (dataGridView1.SelectedRows.Count == 1)
                {
                    if (MessageBox.Show("是否删除选中的请假单？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }

                    if (!m_leaveServer.DeleteLeaveBill(Convert.ToInt32(m_billNo), out m_error))
                    {
                        MessageDialog.ShowPromptMessage(m_error);
                        return;
                    }

                    MessageDialog.ShowPromptMessage("删除成功");
                    m_billMessageServer.BillType = "请假申请单";
                    m_billMessageServer.DestroyMessage(m_billNo);
                }
                else
                {
                    MessageDialog.ShowPromptMessage("请选择需要删除的一行数据！");
                    return;
                }

                RefreshDataGridView();
            }
            else
            {
                MessageDialog.ShowPromptMessage("只有【" + txtApplicant.Text + "】本人才可以删除此单据");
            }

            if (dataGridView1.Rows.Count == 0)
            {
                ClearControl();
            }
        }

        private void 新建单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearControl();

            cbAuthorize.Visible = false;
            lbUnexcusedReason.Visible = false;
            txtUnexcusedReason.Visible = false;
        }

        private void 提交单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lblStatus.Text.Trim() != LeaveBillStatus.新建单据.ToString())
            {
                MessageDialog.ShowPromptMessage("请确认单据状态");
                return;
            }

            if (dtpBeginTime.Value > dtpEndTime.Value)
            {
                MessageDialog.ShowPromptMessage("【起始日期】不能大于【结束日期】");
                return;
            }

            string[] type = cmbLeaveTypeID.Text.Split(' ');

            if (!CheckContrl())
            {
                return;
            }

            HR_LeaveBill leave = new HR_LeaveBill();

            leave.Applicant = txtApplicant.Tag.ToString();
            leave.Date = ServerTime.Time;
            leave.Authorize = cbAuthorize.Checked;
            leave.BeginTime = dtpBeginTime.Value;
            leave.EndTime = dtpEndTime.Value;
            leave.OtherExplanation = txtOtherExplanation.Text;
            leave.Reason = txtReason.Text;
            leave.LeaveTypeID = type[0];
            leave.RealHours = Convert.ToDouble(txtHours.Text);
            leave.Authorize = false;
            bool isDeptDirector = false;
            IQueryable<View_HR_PersonnelArchive> directorGroup = m_personnerServer.GetDeptDirector(
                m_personnerServer.GetPersonnelInfo(txtApplicant.Tag.ToString()).Dept, "0");
            bool flag = false;

            if (directorGroup != null && directorGroup.Count() > 0)
            {
                isDeptDirector = true;
                foreach (var item in directorGroup)
                {
                    if (txtApplicant.Tag.ToString() == item.员工编号)
                    {
                        flag = true;
                        break;
                    }
                }
            }

            IQueryable<View_HR_PersonnelArchive> directorGroup1 = m_personnerServer.GetDeptDirector(
                m_personnerServer.GetPersonnelInfo(txtApplicant.Tag.ToString()).Dept, "1");
            bool flagPri = false;

            if (directorGroup1 != null && directorGroup1.Count() > 0)
            {
                foreach (var item in directorGroup1)
                {
                    if (txtApplicant.Tag.ToString() == item.员工编号)
                    {
                        flagPri = true;
                        break;
                    }
                }
            }

            if (!flag && !flagPri)
            {
                if (isDeptDirector)
                {
                    leave.BillStatus = OverTimeBillStatus.等待主管审核.ToString();
                }
                else
                {
                    leave.BillStatus = OverTimeBillStatus.等待部门负责人审核.ToString();
                }
            }
            else if (flag && !flagPri)
            {
                leave.BillStatus = OverTimeBillStatus.等待部门负责人审核.ToString();
            }
            else
            {
                leave.BillStatus = OverTimeBillStatus.等待分管领导审批.ToString();
            }

            if (!m_leaveServer.AddLeaveBill(leave, out m_error))
            {
                MessageDialog.ShowPromptMessage(m_error);
                return;
            }

            m_billNo = m_leaveServer.GetMaxBillNo().ToString();
            string positionBillNo = m_billNo;
            m_billMessageServer.DestroyMessage(m_billNo);

            if (leave.BillStatus.Equals(OverTimeBillStatus.等待分管领导审批.ToString()))
            {
                m_billMessageServer.SendNewFlowMessage(m_billNo, string.Format("{0}号请假申请单，请分管领导审核", m_billNo),
                    BillFlowMessage_ReceivedUserType.角色, m_billMessageServer.GetDeptLeaderRoleName(
                    m_personnerServer.GetPersonnelViewInfo(txtApplicant.Tag.ToString()).部门编号).ToList());
            }
            else if (leave.BillStatus == OverTimeBillStatus.等待部门负责人审核.ToString())
            {
                m_billMessageServer.SendNewFlowMessage(m_billNo, string.Format("{0}号请假申请单，请部门负责人审核", m_billNo),
                    BillFlowMessage_ReceivedUserType.角色, m_billMessageServer.GetDeptPrincipalRoleName(
                    m_personnerServer.GetPersonnelViewInfo(txtApplicant.Tag.ToString()).部门编号).ToList());
            }
            else
            {
                m_billMessageServer.SendNewFlowMessage(m_billNo, string.Format("{0}号请假申请单，请主管审核", m_billNo),
                    BillFlowMessage_ReceivedUserType.角色, m_billMessageServer.GetDeptDirectorRoleName(
                    m_personnerServer.GetPersonnelViewInfo(txtApplicant.Tag.ToString()).部门编号).ToList());
            }

            MessageDialog.ShowPromptMessage("新增成功！");
            RefreshDataGridView();
            PositioningRecord(positionBillNo);
        }

        private void 分管领导审批toolStripButton2_Click(object sender, EventArgs e)
        {
            if (lblStatus.Text.Trim() != LeaveBillStatus.等待分管领导审批.ToString())
            {
                MessageDialog.ShowPromptMessage("请确认单据状态！");
                return;
            }

            HR_LeaveBill leave = new HR_LeaveBill();

            leave.ID = Convert.ToInt32(m_billNo);
            leave.Leader = BasicInfo.LoginID;
            leave.LeaderSignatureDate = ServerTime.Time;

            if (Convert.ToDouble(txtHours.Text) > m_workHours * 5)
            {
                leave.BillStatus = LeaveBillStatus.等待总经理审批.ToString();
            }
            else
            {
                if (!cbAuthorize.Checked)
                {
                    if (MessageDialog.ShowEnquiryMessage("您是否批准【" + txtApplicant.Text + "】的请假申请？") == DialogResult.No)
                    {
                        if (txtUnexcusedReason.Text == "")
                        {
                            MessageDialog.ShowPromptMessage("请述明不批准的原因！");
                            return;
                        }
                    }
                    else
                    {
                        cbAuthorize.Checked = true;
                    }
                }

                string[] type = m_leaveServer.GetLeaveTypeByCode(
                     dataGridView1.CurrentRow.Cells["请假类别"].Value.ToString()).Rows[0]["请假类别"].ToString().Split(' ');
                DataTable dt = m_leaveServer.GetLeaveType(type[0]);

                if (Convert.ToBoolean(dt.Rows[0]["是否需附件证明"].ToString()) || type[0].Equals("0014"))
                {
                    leave.BillStatus = LeaveBillStatus.等待人力资源复核.ToString();
                }
                else
                {
                    leave.BillStatus = LeaveBillStatus.已完成.ToString();
                }
            }

            leave.Authorize = cbAuthorize.Checked;

            if (!m_leaveServer.UpdateLeave(leave, "分管领导审批", out m_error))
            {
                MessageDialog.ShowPromptMessage(m_error);
                return;
            }

            string msg = string.Format("{0} 号请假单分管领导审核成功", m_billNo);

            if (leave.BillStatus.Equals(LeaveBillStatus.等待人力资源复核.ToString()))
            {
                m_billMessageServer.PassFlowMessage(m_billNo, msg + "等待人力资源部复核",
                    BillFlowMessage_ReceivedUserType.角色, CE_RoleEnum.人力资源部办公室文员.ToString());
            }
            else if (leave.BillStatus.Equals(LeaveBillStatus.已完成.ToString()))
            {
                List<string> noticeUser = new List<string>();
                List<string> noticeRole = new List<string>();

                noticeUser.Add(txtApplicant.Tag.ToString());
                m_billMessageServer.EndFlowMessage(m_billNo, string.Format("{0} 号请假申请已经处理完毕", m_billNo), noticeRole, noticeUser);
            }
            else
            {
                m_billMessageServer.PassFlowMessage(m_billNo, string.Format("{0}号请假申请单，请总经理批准", m_billNo),
                       CE_RoleEnum.总经理.ToString(), true);
            }

            RefreshDataGridView();
            PositioningRecord(m_billNo);
        }

        private void UserControlLeaveBill_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authorityFlag);

            txtApplicant.Text = BasicInfo.LoginName;
            txtApplicant.Tag = BasicInfo.LoginID;
        }

        /// <summary>
        /// 初始化控件
        /// </summary>
        void ClearControl()
        {
            txtApplicant.Text = BasicInfo.LoginName;
            txtApplicant.Tag = BasicInfo.LoginID;
            txtOtherExplanation.Text = "";
            txtReason.Text = "";
            txtDeptDirector.Text = "";
            txtDeptPrincipal.Text = "";
            txtLeader.Text = "";
            txtGeneralManager.Text = "";
            txtUnexcusedReason.Text = "";
            txtHR.Text = "";

            dtpBeginTime.Value = Convert.ToDateTime(
                ServerTime.Time.ToShortDateString() + " " + ServerTime.Time.Hour + ":" + ServerTime.Time.Minute + ":00");
            dtpEndTime.Value = Convert.ToDateTime(
                ServerTime.Time.ToShortDateString() + " " + ServerTime.Time.Hour + ":" + ServerTime.Time.Minute + ":00");
            dtpDate.Value = ServerTime.Time;

            cbAuthorize.Checked = false;
            cmbLeaveTypeID.Text = "";
            lblStatus.Text = LeaveBillStatus.新建单据.ToString();

            DataTable dt = m_personnerServer.GetHighestDept(txtApplicant.Tag.ToString());

            if (dt != null && dt.Rows.Count > 0)
            {
                m_highDept = dt.Rows[0]["deptCode"].ToString();
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
                    if (dataGridView1.Rows[i].Cells["单据号"].Value.ToString() == billNo)
                    {
                        dataGridView1.FirstDisplayedScrollingRowIndex = i;
                        dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 改变控件大小
        /// </summary>
        private void UserControlLeaveBill_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        /// <summary>
        /// 检查控件
        /// </summary>
        /// <returns>信息正确返回True，失败返回False</returns>
        bool CheckContrl()
        {
            try
            {
                if (MessageDialog.ShowEnquiryMessage("请确认您的\r\n【请假起始时间】："
                    + dtpBeginTime.Value.ToString("yyyy-MM-dd HH:mm") + "\r\n【请假结束时间】："
                    + dtpEndTime.Value.ToString("yyyy-MM-dd HH:mm")) == DialogResult.No)
                {
                    return false;
                }

                if (cmbLeaveTypeID.Text == "")
                {
                    MessageDialog.ShowPromptMessage("请选择请假类别！");
                    return false;
                }

                if (cmbLeaveTypeID.Text == "其他" && txtOtherExplanation.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请假类别选择的是‘其他’，请在后面填写请假类别！");
                    return false;
                }

                if (txtReason.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请填写请假原因！");
                    return false;
                }

                if (dtpBeginTime.Value > dtpEndTime.Value)
                {
                    MessageDialog.ShowPromptMessage("请假起始时间大于终止时间，请重新选择！");
                    return false;
                }

                if (txtHours.Text.Trim() == "0")
                {
                    MessageDialog.ShowPromptMessage("时间选择有误，请重新选择！");
                    return false;
                }

                if ((ServerTime.Time - dtpBeginTime.Value).Days > 4
                    && lblStatus.Text != LeaveBillStatus.等待人力资源复核.ToString()
                    && !人力补单toolStripButton.Checked)
                {
                    string starTemp = dtpBeginTime.Value.ToShortDateString() + " " + m_punchInMorning;
                    DataTable dt = m_holidayServer.GetHolidayDays(Convert.ToDateTime(starTemp), ServerTime.Time);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        int days = Convert.ToInt32(dt.Rows[0]["days"] == DBNull.Value ? "0" : dt.Rows[0]["days"]);

                        if (days > 0)
                        {
                            if ((ServerTime.Time - dtpBeginTime.Value).Days > 3 + days)
                            {
                                MessageDialog.ShowPromptMessage("只能补三天内的请假单！");
                                return false;
                            }
                        }
                        else
                        {
                            MessageDialog.ShowPromptMessage("只能补三天内的请假单！");
                            return false;
                        }
                    }
                    else
                    {
                        MessageDialog.ShowPromptMessage("只能补三天内的请假单！");
                        return false;
                    }
                }

                m_leaveServer.Check_LeaveType(cmbLeaveTypeID.Text.Split(' ')[0], Convert.ToDecimal(txtHours.Text), null);
                return true;
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 获得一个月的天数
        /// </summary>
        /// <returns>返回天数</returns>
        private int GetDays()
        {
            int days = 31;

            if (dtpBeginTime.Value.Month == 2)
            {
                days = 28;
            }
            else if (dtpBeginTime.Value.Month == 4 || dtpBeginTime.Value.Month == 6
                || dtpBeginTime.Value.Month == 9 || dtpBeginTime.Value.Month == 11)
            {
                days = 30;
            }

            return days;
        }

        //Modify by cjb on 2015.9.1
        string GetHours_By_CJB()
        {
            if (dtpEndTime.Value <= dtpBeginTime.Value)
            {
                return "0";
            }

            m_leaveDays = (dtpEndTime.Value - dtpBeginTime.Value).Days;

            if (dtpEndTime.Value.Month > 9 || dtpEndTime.Value.Month < 5)
            {
                m_restHours = 1;

                m_punchInAfternoon = "13:00:00";
                m_punchInAfternoonEnd = "17:30:00";
            }
            else if (dtpEndTime.Value.Month > 4 && dtpEndTime.Value.Month < 10)
            {
                m_restHours = 1.5;

                m_punchInAfternoon = "13:30:00";
                m_punchInAfternoonEnd = "18:00:00";
            } 

            m_workHours = 8;

            double hours = m_leaveDays * m_workHours;
            hours = hours + GetHours_By_CJB_1(dtpBeginTime.Value.AddDays(m_leaveDays), dtpEndTime.Value);

            return hours.ToString();
        }

        //Modify by cjb on 2015.9.1
        double GetHours_By_CJB_1(DateTime startDate, DateTime endDate)
        {
            double resultHours = 0;

            if ((endDate - startDate).Minutes > 30 && (endDate.Minute != startDate.Minute))
            {
                resultHours = 1;
            }
            else if ((endDate - startDate).Minutes <= 30 && (endDate.Minute != startDate.Minute))
            {
                resultHours = 0.5;
            }

            if (startDate.Day != endDate.Day)
            {
                double startHours = (Convert.ToDateTime(m_punchInAfternoonEnd) - Convert.ToDateTime(startDate.ToShortTimeString())).Hours;

                if (Convert.ToDateTime(startDate.ToShortTimeString()) >= Convert.ToDateTime(m_punchInMorning)
                    && Convert.ToDateTime(startDate.ToShortTimeString()) <= Convert.ToDateTime(m_punchInMorningEnd))
                {
                    startHours -= m_restHours;
                }

                double endHours = (Convert.ToDateTime(endDate.ToShortTimeString()) - Convert.ToDateTime(m_punchInMorning)).Hours;

                if (Convert.ToDateTime(endDate.ToShortTimeString()) >= Convert.ToDateTime(m_punchInAfternoon)
                    && Convert.ToDateTime(endDate.ToShortTimeString()) <= Convert.ToDateTime(m_punchInAfternoonEnd))
                {
                    endHours -= m_restHours;
                }

                resultHours = resultHours + startHours + endHours;
            }
            else
            {
                resultHours = resultHours + (endDate - startDate).Hours;

                if (Convert.ToDateTime(startDate.ToShortTimeString()) <= Convert.ToDateTime(m_punchInMorningEnd)
                    && Convert.ToDateTime(endDate.ToShortTimeString()) >= Convert.ToDateTime(m_punchInAfternoon))
                {
                    resultHours -= m_restHours;
                }
            }

            if (resultHours < 0)
            {
                resultHours = (endDate - startDate).TotalHours;
            }

            return resultHours;
        }

        private void txtApplicant_OnCompleteSearch()
        {
            txtApplicant.Tag = txtApplicant.DataResult["工号"].ToString();
        }
    }
}
