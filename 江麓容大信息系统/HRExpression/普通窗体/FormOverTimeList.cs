using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Expression;
using ServerModule;
using PlatformManagement;
using Service_Peripheral_HR;
using GlobalObject;
using UniversalControlLibrary;
using System.Collections;

namespace Form_Peripheral_HR
{
    /// <summary>
    /// 加班申请界面
    /// </summary>
    public partial class FormOverTimeList : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string error;

        /// <summary>
        /// 单据编号
        /// </summary>
        string m_billNo;

        /// <summary>
        /// 最高部门
        /// </summary>
        string m_highDept;

        /// <summary>
        /// 中午休息小时数
        /// </summary>
        double m_restHours;

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
        /// 
        /// </summary>
        //bool m_flag = false;

        /// <summary>
        /// 操作权限
        /// </summary>
        PlatformManagement.AuthorityFlag m_authorityFlag;

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 节假日管理类
        /// </summary>
        IHolidayServer m_holidayServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IHolidayServer>();

        /// <summary>
        /// 人员档案服务组件
        /// </summary>
        IPersonnelArchiveServer m_personnerServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IPersonnelArchiveServer>();

        /// <summary>
        /// 考勤方案操作类
        /// </summary>
        IAttendanceSchemeServer m_attendanceSchemeServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IAttendanceSchemeServer>();

        /// <summary>
        /// 岗位管理服务类
        /// </summary>
        IOperatingPostServer m_PostServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IOperatingPostServer>();

        /// <summary>
        /// 考勤分析汇总操作接口
        /// </summary>
        IAttendanceSummaryServer m_attendanceServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IAttendanceSummaryServer>();

        /// <summary>
        /// 加班申请操作类
        /// </summary>
        IOverTimeBillServer m_overTimeServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IOverTimeBillServer>();

        public FormOverTimeList(AuthorityFlag authFlag, int billID)
        {
            InitializeComponent();

            IQueryable<HR_AttendanceScheme> dtScheme = m_attendanceSchemeServer.GetLinqResult();

            string punchInMorning = dtScheme.Take(1).Single().BeginTimeInTheMorning.Value.Hour.ToString() +
                ":" + dtScheme.Take(1).Single().BeginTimeInTheMorning.Value.Minute.ToString();

            string punchInMorningEnd = dtScheme.Take(1).Single().EndTimeInTheMorning.Value.Hour.ToString() +
                ":" + dtScheme.Take(1).Single().EndTimeInTheMorning.Value.Minute.ToString();

            string punchInAfternoonEnd = dtScheme.Take(1).Single().EndTimeInTheAfternoon.Value.Hour.ToString() +
                ":" + dtScheme.Take(1).Single().EndTimeInTheAfternoon.Value.Minute.ToString();

            string punchInAfternoon = dtScheme.Take(1).Single().BeginTimeInTheAfternoon.Value.Hour.ToString() +
                ":" + dtScheme.Take(1).Single().BeginTimeInTheAfternoon.Value.Minute.ToString();

            double restHours = (Convert.ToDateTime(punchInAfternoon) - Convert.ToDateTime(punchInMorningEnd)).Hours;
            restHours += Convert.ToDouble((Convert.ToDateTime(punchInAfternoon) - Convert.ToDateTime(punchInMorningEnd)).Minutes) / 60;

            m_punchInMorning = punchInMorning;
            m_punchInMorningEnd = punchInMorningEnd;
            m_punchInAfternoonEnd = punchInAfternoonEnd;
            m_punchInAfternoon = punchInAfternoon;
            m_restHours = restHours;

            m_billMessageServer.BillType = "加班申请单";

            m_authorityFlag = authFlag;

            if (billID != 0)
            {
                m_billNo = billID.ToString();

                BindControl(m_billNo);
            }
            else
            {
                txtApplicant.Text = BasicInfo.LoginName;
                txtApplicant.Tag = BasicInfo.LoginID;
                lblStatus.Text = OverTimeBillStatus.新建单据.ToString();
                cbAuthorize.Visible = false;
                cbVerify.Visible = false;
                numVerifyHours.Visible = false;
                lblVerifyHours.Visible = false;

                DateTime time = ServerTime.Time;

                if (!m_overTimeServer.IsChooseDoubleRest(UniversalFunction.GetPersonnelInfo(txtApplicant.Tag.ToString()).职位编码.ToString(), "", ""))
                {
                    if (Convert.ToDateTime(dtpEndTime.Value.ToShortTimeString()) > Convert.ToDateTime(m_punchInAfternoonEnd))
                    {
                        if ((dtpEndTime.Value.Month > 9 || dtpEndTime.Value.Month < 5))
                        {
                            dtpBeginTime.Value = Convert.ToDateTime(dtpBeginTime.Value.ToShortDateString() + " " + "17:30:00");
                        }
                        else if (dtpEndTime.Value.Month >= 5)
                        {
                            dtpBeginTime.Value = Convert.ToDateTime(dtpBeginTime.Value.ToShortDateString() + " " + "18:30:00");
                        }
                    }
                }

                dtpDate.Value = time;
            }

            DataTable dt = m_personnerServer.GetHighestDept(txtApplicant.Tag.ToString());

            if (dt != null && dt.Rows.Count > 0)
            {
                m_highDept = dt.Rows[0]["deptCode"].ToString();
            }

            人力toolStripButton.Visible = false;
            toolStripSeparator4.Visible = false;

            dtpBeginTime.ValueChanged += new EventHandler(DateTimePick_ValueChanged);
            dtpEndTime.ValueChanged += new EventHandler(DateTimePick_ValueChanged);
        }

        private void FormOverTimeList_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authorityFlag);
        }

        /// <summary>
        /// 为控件赋值
        /// </summary>
        /// <param name="BillNo"></param>
        private void BindControl(string m_billNo)
        {
            DataTable dtTemp = m_overTimeServer.GetOverTimeBillByID(m_billNo);

            if (dtTemp != null && dtTemp.Rows.Count > 0)
            {

                txtErrand.Text = dtTemp.Rows[0]["任务描述"].ToString();
                txtApplicant.Text = dtTemp.Rows[0]["申请人"].ToString();
                txtApplicant.Tag = dtTemp.Rows[0]["员工编号"].ToString();
                txtDeptDirector.Text = UniversalFunction.GetPersonnelName( dtTemp.Rows[0]["部门主管"].ToString());
                txtDeptPrincipal.Text = UniversalFunction.GetPersonnelName( dtTemp.Rows[0]["部门负责人"].ToString());
                txtVerify.Text = UniversalFunction.GetPersonnelName(dtTemp.Rows[0]["确认完成人"].ToString());
                txtLeader.Text = UniversalFunction.GetPersonnelName(dtTemp.Rows[0]["分管领导"].ToString());
                txtNumOfPersonnel.Text = dtTemp.Rows[0]["加班人数"].ToString();
                cmbCompensateMode.Text = dtTemp.Rows[0]["补偿方式"].ToString();
                cbAuthorize.Checked = Convert.ToBoolean(dtTemp.Rows[0]["是否批准"].ToString());
                numHours.Value = Convert.ToDecimal(dtTemp.Rows[0]["加班小时"].ToString());
                dtpDate.Value = Convert.ToDateTime(dtTemp.Rows[0]["申请时间"].ToString());
                dtpBeginTime.Value = Convert.ToDateTime(dtTemp.Rows[0]["开始时间"].ToString());
                cbVerify.Checked = Convert.ToBoolean(dtTemp.Rows[0]["是否确认完成"].ToString());

                cmbOvertimeAddress.Text = dtTemp.Rows[0]["加班地点"].ToString();

                if (dtTemp.Rows[0]["加班结束时间"].ToString() != "")
                {
                    dtpEndTime.Value = Convert.ToDateTime(dtTemp.Rows[0]["加班结束时间"].ToString());
                }

                if (dtTemp.Rows[0]["确认加班小时数"].ToString() != "")
                {
                    numVerifyHours.Value = Convert.ToDecimal(dtTemp.Rows[0]["确认加班小时数"].ToString());
                }
                else 
                {
                    numVerifyHours.Value = Convert.ToDecimal(dtTemp.Rows[0]["加班小时"].ToString());
                }

                if (dtTemp.Rows[0]["确认完成时间"].ToString() != "")
                {
                    dtpVerify.Value = Convert.ToDateTime(dtTemp.Rows[0]["确认完成时间"].ToString());
                }

                if (dtTemp.Rows[0]["主管签字日期"].ToString() != "")
                {
                    dtpDeptDirectorDate.Value = Convert.ToDateTime(dtTemp.Rows[0]["主管签字日期"].ToString());
                }

                if (dtTemp.Rows[0]["负责人签字日期"].ToString() != "")
                {
                    dtpDeptPrincipalDate.Value = Convert.ToDateTime(dtTemp.Rows[0]["负责人签字日期"].ToString());
                }

                if (dtTemp.Rows[0]["领导签字日期"].ToString() != "")
                {
                    dtpLeaderDate.Value = Convert.ToDateTime(dtTemp.Rows[0]["领导签字日期"].ToString());
                }

                lblStatus.Text = dtTemp.Rows[0]["单据状态"].ToString();
                m_billNo = dtTemp.Rows[0]["单据号"].ToString();

                dataGridView2.DataSource = m_overTimeServer.GetOverTimePersonnelByID(m_billNo);
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

        ///// <summary>
        ///// 初始化控件
        ///// </summary>
        //void ClearControl()
        //{
        //    txtErrand.Text = "";
        //    txtApplicant.Text = BasicInfo.LoginName;
        //    txtApplicant.Tag = BasicInfo.LoginID;
        //    txtNumOfPersonnel.Text = "0";
        //    cmbCompensateMode.Text = "";
        //    cbAuthorize.Checked = false;
        //    numHours.Value = 2;
        //    txtRealHours.Text = "0";
        //    cmbCompensateMode.Enabled = true;
        //    m_billNo = "";

        //    if (dataGridView2.Rows.Count > 0)
        //    {
        //        dataGridView2.Rows.Clear();
        //    }
        //}
        private void 添加人员ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cmbCompensateMode.SelectedIndex == -1)
            {
                MessageDialog.ShowPromptMessage("请先选择“补偿方式”！");
                return;
            }
            
            FormSelectPersonnel form = new FormSelectPersonnel("员工");
            form.DeptCode = BasicInfo.DeptCode;

            if (dataGridView2.Rows.Count > 0)
            {
                List<View_SelectPersonnel> list = new List<View_SelectPersonnel>();

                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    View_SelectPersonnel person = new View_SelectPersonnel();

                    person.员工编号 = dataGridView2.Rows[i].Cells["员工编号"].Value.ToString();
                    list.Add(person);
                }

                form.SelectedUser = list;
            }

            if (form.ShowDialog() == DialogResult.OK)
            {
                dataGridView2.DataSource = form.SelectedUser;
            }

            txtNumOfPersonnel.Text = dataGridView2.Rows.Count.ToString();

            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                View_SelectPersonnel person = new View_SelectPersonnel();

                person.员工编号 = dataGridView2.Rows[i].Cells["员工编号"].Value.ToString();

                #region 2013-08-23 邱瑶 人力重新定义了加班，以前所有选择加班补偿方式的约束代码都取消，任何时间，任何员工都可以选择

                //if (m_personnerServer.GetPersonnelInfo(person.员工编号).WorkPost.ToString() == "393"
                //    || m_personnerServer.GetPersonnelInfo(person.员工编号).WorkPost.ToString() == "385")
                //{
                //    cmbCompensateMode.SelectedIndex = 0;
                //    cmbCompensateMode.Enabled = true;
                //}
                //else
                //{
                //    bool b = m_overTimeServer.IsChooseDoubleRest(m_personnerServer.GetPersonnelInfo(person.员工编号).WorkPost.ToString(),
                //        m_personnerServer.GetPersonnelInfo(person.员工编号).Dept.ToString(), person.员工编号);

                //    if (b)
                //    {
                //        cmbCompensateMode.SelectedIndex = 0;
                //        cmbCompensateMode.Enabled = false;
                //    }
                //}
                #endregion                
            }
        }

        /// <summary>
        /// 检查控件
        /// </summary>
        /// <returns>信息正确返回True，失败返回False</returns>
        bool CheckControl()
        {
            if (dtpEndTime.Value <= dtpBeginTime.Value)
            {
                MessageDialog.ShowPromptMessage("【起始时间】必须小于【结束时间】");
                return false;
            }

            if (MessageDialog.ShowEnquiryMessage("请确认您的\r\n【加班起始时间】："
                + dtpBeginTime.Value.ToString("yyyy-MM-dd HH:mm") + "\r\n【加班结束时间】："
                + dtpEndTime.Value.ToString("yyyy-MM-dd HH:mm")) == DialogResult.No)
            {
                return false;
            }

            if (txtErrand.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请填写加班任务描述！");
                return false;
            }

            if (numHours.Value == 0)
            {
                MessageDialog.ShowPromptMessage("请填写需要加班小时！");
                return false;
            }

            if (cmbCompensateMode.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请选择加班补偿方式！");
                return false;
            }

            if (cmbOvertimeAddress.SelectedIndex == -1)
            {
                MessageDialog.ShowPromptMessage("请选择加班地点！");
                return false;
            }

            if (dataGridView2.Rows.Count > 0)
            {
                DateTime starDate = ServerTime.Time;
                DateTime endDate = ServerTime.Time;

                if (cmbCompensateMode.SelectedIndex == 0)
                {
                    string month = "";

                    if (dtpBeginTime.Value.Month < 10)
                    {
                        month = "0" + dtpBeginTime.Value.Month;
                    }
                    else
                    {
                        month = dtpBeginTime.Value.Month.ToString();
                    }

                    starDate = Convert.ToDateTime(dtpBeginTime.Value.Year.ToString() + "-" + month + "-" + "01");
                    endDate = Convert.ToDateTime(dtpBeginTime.Value.Year.ToString() + "-" + month + "-" + GetDays());

                    if (m_highDept.Contains("ZZ") || m_highDept == "SC")
                    {
                        if (dtpBeginTime.Value.Month == 1)
                        {
                            starDate = Convert.ToDateTime((dtpBeginTime.Value.Year - 1).ToString() + "-12-26");
                            endDate = Convert.ToDateTime(dtpBeginTime.Value.Year.ToString() + "-" + month + "-" + "26");
                        }
                        else
                        {
                            starDate = Convert.ToDateTime(dtpBeginTime.Value.Year.ToString() + "-" + (dtpBeginTime.Value.Month - 1).ToString() + "-" + "26");
                            endDate = Convert.ToDateTime(dtpBeginTime.Value.Year.ToString() + "-" + month + "-" + "26");
                        }
                    }
                }

                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    DataGridViewCellCollection cells = dataGridView2.Rows[i].Cells;

                    if (cells["员工编号"].Value == null || GlobalObject.GeneralFunction.IsNullOrEmpty(cells["员工编号"].Value.ToString()))
                    {
                        MessageDialog.ShowPromptMessage(string.Format("第 {0} 行, 请选择加班人员", i + 1));
                        return false;
                    }

                    if (cmbCompensateMode.SelectedIndex == 0)
                    {
                        if (!m_attendanceServer.GetAllowMobileVacationHour(cells["员工编号"].Value.ToString()))
                        {
                            if (m_overTimeServer.GetMonthRealHour(cells["员工编号"].Value.ToString(), starDate, endDate) <= 0)
                            {
                                MessageDialog.ShowPromptMessage(UniversalFunction.GetPersonnelName(cells["员工编号"].Value.ToString()) 
                                    + "有欠班，只能选择调休！");
                                return false;
                            }
                        }
                    }
                }
            }
            else if (dataGridView2.Rows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择加班人员");
                return false;
            }

            IQueryable<View_HR_PersonnelArchive> directorGroup = m_personnerServer.GetDeptDirector(
                m_personnerServer.GetPersonnelInfo(txtApplicant.Tag.ToString()).Dept, "2");
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
                MessageDialog.ShowPromptMessage("您是分管领导，不用填写加班单！");
                return false;
            }

            if ((ServerTime.Time - dtpBeginTime.Value).Days > 3 && !BasicInfo.ListRoles.Contains(CE_RoleEnum.人力资源部办公室文员.ToString()))
            {
                string starTemp = dtpBeginTime.Value.ToShortDateString() + " " + "08:30";

                DataTable dt = m_holidayServer.GetHolidayDays(Convert.ToDateTime(starTemp),ServerTime.Time);

                if (dt != null && dt.Rows.Count > 0)
                {
                    int days = Convert.ToInt32(dt.Rows[0]["days"] == DBNull.Value ? "0" : dt.Rows[0]["days"]);

                    if (days > 0)
                    {
                        if ((ServerTime.Time - dtpBeginTime.Value).Days > 3 + days)
                        {
                            MessageDialog.ShowPromptMessage("只能补三天内的加班单！");
                            return false;
                        }
                    }
                    else
                    {
                        MessageDialog.ShowPromptMessage("只能补三天内的加班单！");
                        return false;
                    }
                }
                else
                {
                    MessageDialog.ShowPromptMessage("只能补三天内的加班单！");
                    return false;
                }
            }

            HR_AttendanceSetting attendanceSet = m_attendanceSchemeServer.GetAttendanceSettingByWorkID(BasicInfo.LoginID);
            string[] schemeCode = attendanceSet.SchemeCode.Split(' ');
            string mode = m_attendanceSchemeServer.GetAttendanceSchemeByCode(schemeCode[0]).AttendanceMode;

            Hashtable hsTable = new Hashtable();

            hsTable.Add("@ParentCode", "ZZGC");
            hsTable.Add("@WorkID", BasicInfo.LoginID);

            DataTable tempTable = GlobalObject.DatabaseServer.QueryInfoPro("HR_Personnel_GetParentDept", hsTable, out error);

            #region 2017.10.27 夏石友， 向菲菲将工务人员、车间人员修改为非自然月排班考勤后出现必须加班2小时才可以填写加班单的现象
            //if (mode.Contains("非自然月考勤"))
            if (mode.Contains("非自然月考勤") || mode.Contains("非自然月排班考勤") || tempTable.Rows.Count > 0)
            #endregion
            {
                if (numHours.Value < 1)
                {
                    MessageDialog.ShowPromptMessage("加班1小时以上才可以填写加班申请单！");
                    return false;
                }
            }
            else
            {
                if (numHours.Value < 2)
                {
                    MessageDialog.ShowPromptMessage("加班2小时以上才可以填写加班申请单！");
                    return false;
                }
            }

            return true;
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

        private void 提交单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lblStatus.Text.Trim() == OverTimeBillStatus.新建单据.ToString())
            {
                if (!CheckControl())
                {
                    return;
                }

                List<HR_OvertimePersonnel> lstPersonnel = new List<HR_OvertimePersonnel>(dataGridView2.Rows.Count);

                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    HR_OvertimePersonnel personnel = new HR_OvertimePersonnel();
                    DataGridViewCellCollection cells = dataGridView2.Rows[i].Cells;

                    personnel.WorkID = dataGridView2.Rows[i].Cells["员工编号"].Value.ToString();
                    lstPersonnel.Add(personnel);
                }
                
                HR_OvertimeBill overTimeBill = new HR_OvertimeBill();

                overTimeBill.Applicant = txtApplicant.Tag.ToString();
                overTimeBill.Authorize = false;
                overTimeBill.BeginTime = dtpBeginTime.Value;
                overTimeBill.CompensateMode = cmbCompensateMode.Text;
                overTimeBill.Date = ServerTime.Time;
                overTimeBill.Errand = txtErrand.Text;
                overTimeBill.Hours = numHours.Value;
                overTimeBill.RealHours = Convert.ToDouble(numHours.Value);
                overTimeBill.NumberOfPersonnel = Convert.ToInt32(txtNumOfPersonnel.Text);
                overTimeBill.VerifyFinish = false;
                overTimeBill.EndTime = dtpEndTime.Value;
                overTimeBill.OvertimeAddress = cmbOvertimeAddress.Text;

                IQueryable<View_HR_PersonnelArchive> directorGroup1 = m_personnerServer.GetDeptDirector(
                    m_personnerServer.GetPersonnelInfo(txtApplicant.Tag.ToString()).Dept,"1");

                bool flagPri = false;//判断申请人是不是负责人
                bool isDeptDirector = false;//申请部门有没有主管

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

                IQueryable<View_HR_PersonnelArchive> directorGroup = m_personnerServer.GetDeptDirector(
                    m_personnerServer.GetPersonnelInfo(txtApplicant.Tag.ToString()).Dept, "0");
                bool flag = false;//判断申请人是不是主管

                if (directorGroup != null && directorGroup.Count() > 0)
                {
                    isDeptDirector = true;

                    foreach (var item in directorGroup)
                    {
                        if (BasicInfo.LoginID == item.员工编号)
                        {
                            flag = true;

                            break;
                        }
                    }
                }

                if (!flag && !flagPri)
                {
                    if (isDeptDirector)
                    {
                        overTimeBill.BillStatus = OverTimeBillStatus.等待主管审核.ToString();
                    }
                    else
                    {
                        overTimeBill.BillStatus = OverTimeBillStatus.等待部门负责人审核.ToString();
                    }
                }
                else if (flag && !flagPri)
                {
                    overTimeBill.BillStatus = OverTimeBillStatus.等待部门负责人审核.ToString();
                }
                else
                {
                    overTimeBill.BillStatus = OverTimeBillStatus.等待分管领导审批.ToString();
                }

                m_billNo = m_overTimeServer.AddOverTimeBill(overTimeBill, lstPersonnel, out error).ToString();

                if (Convert.ToInt32(m_billNo) < 0)
                {
                    MessageDialog.ShowPromptMessage(error);
                    return;
                }
                else
                {
                    MessageDialog.ShowPromptMessage("新增成功！");

                    m_billMessageServer.DestroyMessage(m_billNo);

                    if (overTimeBill.BillStatus.Equals(OverTimeBillStatus.等待主管审核.ToString()))
                    {
                        m_billMessageServer.SendNewFlowMessage(m_billNo, string.Format("{0}号加班申请单，请主管审核", m_billNo),
                            BillFlowMessage_ReceivedUserType.角色,
                            m_billMessageServer.GetDeptDirectorRoleName(m_personnerServer.GetPersonnelViewInfo(
                            txtApplicant.Tag.ToString()).部门编号).ToList());
                    }
                    else if (overTimeBill.BillStatus == OverTimeBillStatus.等待部门负责人审核.ToString())
                    {
                        m_billMessageServer.SendNewFlowMessage(m_billNo, string.Format("{0}号加班申请单，请部门负责人审核", m_billNo),
                            BillFlowMessage_ReceivedUserType.角色, m_billMessageServer.GetDeptPrincipalRoleName(
                            m_personnerServer.GetPersonnelViewInfo(txtApplicant.Tag.ToString()).部门编号).ToList());
                    }
                    else
                    {
                        m_billMessageServer.SendNewFlowMessage(m_billNo, string.Format("{0}号加班申请单，请分管领导审核", m_billNo),
                       BillFlowMessage_ReceivedUserType.角色, m_billMessageServer.GetDeptLeaderRoleName(
                       m_personnerServer.GetPersonnelViewInfo(txtApplicant.Tag.ToString()).部门编号).ToList());
                    }
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请确认单据状态！");
                return;
            }
            this.Close();
        }

        private void 修改单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (BasicInfo.LoginID == txtApplicant.Tag.ToString())
            {
                if (lblStatus.Text == "已完成")
                {
                    MessageDialog.ShowPromptMessage("单据已完成，不能修改！");
                    return;
                }

                if (!CheckControl())
                {
                    return;
                }

                List<HR_OvertimePersonnel> lstPersonnel = new List<HR_OvertimePersonnel>(dataGridView2.Rows.Count);

                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    HR_OvertimePersonnel personnel = new HR_OvertimePersonnel();
                    DataGridViewCellCollection cells = dataGridView2.Rows[i].Cells;

                    personnel.WorkID = dataGridView2.Rows[i].Cells["员工编号"].Value.ToString();

                    lstPersonnel.Add(personnel);
                }

                HR_OvertimeBill overTimeBill = new HR_OvertimeBill();

                overTimeBill.ID = Convert.ToInt32(m_billNo);
                overTimeBill.Authorize = false;
                overTimeBill.BeginTime = dtpBeginTime.Value;
                overTimeBill.CompensateMode = cmbCompensateMode.Text;
                overTimeBill.Date = dtpDate.Value;
                overTimeBill.OvertimeAddress = cmbOvertimeAddress.Text;
                overTimeBill.Errand = txtErrand.Text;
                overTimeBill.Hours = numHours.Value;
                overTimeBill.RealHours = Convert.ToDouble(numHours.Value);
                overTimeBill.NumberOfPersonnel = Convert.ToInt32(txtNumOfPersonnel.Text);
                overTimeBill.EndTime = dtpEndTime.Value;
                overTimeBill.VerifyHours = numVerifyHours.Value;

                IQueryable<View_HR_PersonnelArchive> directorGroup1 = m_personnerServer.GetDeptDirector(
                   m_personnerServer.GetPersonnelInfo(txtApplicant.Tag.ToString()).Dept, "1");

                bool flagPri = false;//判断申请人是不是负责人
                bool isDeptDirector = false;//申请部门有没有主管

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

                IQueryable<View_HR_PersonnelArchive> directorGroup = m_personnerServer.GetDeptDirector(
                    m_personnerServer.GetPersonnelInfo(txtApplicant.Tag.ToString()).Dept, "0");
                bool flag = false;//判断申请人是不是主管

                if (directorGroup != null && directorGroup.Count() > 0)
                {
                    isDeptDirector = true;

                    foreach (var item in directorGroup)
                    {
                        if (BasicInfo.LoginID == item.员工编号)
                        {
                            flag = true;

                            break;
                        }
                    }
                }

                if (!flag && !flagPri)
                {
                    if (isDeptDirector)
                    {
                        overTimeBill.BillStatus = OverTimeBillStatus.等待主管审核.ToString();
                    }
                    else
                    {
                        overTimeBill.BillStatus = OverTimeBillStatus.等待部门负责人审核.ToString();
                    }
                }
                else if (flag && !flagPri)
                {
                    overTimeBill.BillStatus = OverTimeBillStatus.等待部门负责人审核.ToString();
                }
                else
                {
                    overTimeBill.BillStatus = OverTimeBillStatus.等待分管领导审批.ToString();
                }

                if (!m_overTimeServer.UpdateOverTimeBill(overTimeBill, lstPersonnel, Convert.ToInt32(m_billNo), out error))
                {
                    MessageDialog.ShowPromptMessage(error);
                    return;
                }
                    MessageDialog.ShowPromptMessage("修改成功！");

                    m_billMessageServer.DestroyMessage(m_billNo);

                    if (overTimeBill.BillStatus.Equals(OverTimeBillStatus.等待主管审核.ToString()))
                    {
                        m_billMessageServer.PassFlowMessage(m_billNo, string.Format("{0}号加班申请单，请主管审核", m_billNo),
                            BillFlowMessage_ReceivedUserType.角色,
                            m_billMessageServer.GetDeptDirectorRoleName(m_personnerServer.GetPersonnelViewInfo(
                            txtApplicant.Tag.ToString()).部门编号).ToList());
                    }
                    else if (overTimeBill.BillStatus == OverTimeBillStatus.等待部门负责人审核.ToString())
                    {
                        m_billMessageServer.PassFlowMessage(m_billNo, string.Format("{0}号加班申请单，请部门负责人审核", m_billNo),
                            BillFlowMessage_ReceivedUserType.角色, m_billMessageServer.GetDeptPrincipalRoleName(
                            m_personnerServer.GetPersonnelViewInfo(txtApplicant.Tag.ToString()).部门编号).ToList());
                    }
                    else
                    {
                        m_billMessageServer.PassFlowMessage(m_billNo, string.Format("{0}号加班申请单，请分管领导审核", m_billNo),
                       BillFlowMessage_ReceivedUserType.角色, m_billMessageServer.GetDeptLeaderRoleName(
                       m_personnerServer.GetPersonnelViewInfo(txtApplicant.Tag.ToString()).部门编号).ToList());
                    }
            }

            this.Close();
        }

        private void 主管审核toolStripButton_Click(object sender, EventArgs e)
        {
            if (lblStatus.Text.Trim() == OverTimeBillStatus.等待主管审核.ToString())
            {
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
                    HR_OvertimeBill overTime = new HR_OvertimeBill();

                    overTime.ID = Convert.ToInt32(m_billNo);
                    overTime.Date = Convert.ToDateTime(dtpDate.Value.ToShortDateString());
                    overTime.CompensateMode = cmbCompensateMode.Text;
                    overTime.Hours = numHours.Value;
                    overTime.DeptDirector = BasicInfo.LoginID;
                    overTime.DeptDirectorSignatureDate = ServerTime.Time;
                    overTime.BillStatus = OverTimeBillStatus.等待部门负责人审核.ToString();

                    if (!m_overTimeServer.UpdateOverTimeBill(overTime, "部门主管审批", out error))
                    {
                        MessageDialog.ShowPromptMessage(error); 
                        return;
                    }

                    string msg = string.Format("{0} 号加班单主管审核成功,请部门负责人审批", m_billNo);
                    m_billMessageServer.PassFlowMessage(m_billNo, msg, BillFlowMessage_ReceivedUserType.角色, 
                        m_billMessageServer.GetDeptPrincipalRoleName(m_personnerServer.GetPersonnelViewInfo(
                        txtApplicant.Tag.ToString()).部门编号).ToList());

                    MessageDialog.ShowPromptMessage("主管审核成功！");
                    this.Close();
                }
                else
                {
                    MessageDialog.ShowPromptMessage("您不是申请人的部门主管，请等待部门主管审核！");
                    return;
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请确认单据状态！");
            }
        }

        private void 负责人toolStripButton_Click(object sender, EventArgs e)
        {
            if (lblStatus.Text.Trim() == OverTimeBillStatus.等待部门负责人审核.ToString())
            {
                HR_OvertimeBill overTime = new HR_OvertimeBill();

                overTime.ID = Convert.ToInt32(m_billNo);
                overTime.DeptPrincipal = BasicInfo.LoginID;
                overTime.DeptPrincipalSignatureDate = ServerTime.Time;
                overTime.RealHours = Convert.ToDouble(numVerifyHours.Value);
                overTime.VerifyHours = numVerifyHours.Value;

                if (!cbAuthorize.Checked)
                {
                    if (MessageBox.Show("您是否同意【" + txtApplicant.Text + "】的加班申请？", "消息", 
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cbAuthorize.Checked = true;
                    }
                }

                overTime.Authorize = cbAuthorize.Checked;

                IQueryable<View_HR_PersonnelArchive> directorGroup = m_personnerServer.GetDeptDirector(
                    m_personnerServer.GetPersonnelInfo(txtApplicant.Tag.ToString()).Dept,"1");
                bool flag = false;

                if (directorGroup != null && directorGroup.Count() > 0)
                {
                    foreach (var item in directorGroup)
                    {
                        if (txtApplicant.Tag.ToString() == item.员工编号)
                        {
                            flag = true;
                            break;
                        }
                    }
                }

                if (!cbAuthorize.Checked)
                {
                    overTime.BillStatus = OverTimeBillStatus.已完成.ToString();
                }
                else if (!flag)
                {
                    if (ServerTime.Time > dtpBeginTime.Value)
                    {
                        overTime.BillStatus = OverTimeBillStatus.已完成.ToString();
                    }
                    else
                    {
                        overTime.BillStatus = OverTimeBillStatus.确认加班完成情况.ToString();
                    }
                }
                else
                {
                    overTime.BillStatus = OverTimeBillStatus.等待分管领导审批.ToString();
                }

                if (!m_overTimeServer.UpdateOverTimeBill(overTime, "部门负责人审批", out error))
                {
                    MessageDialog.ShowPromptMessage(error);
                    return;
                }

                if (overTime.BillStatus == OverTimeBillStatus.已完成.ToString())
                {
                    if (!cbAuthorize.Checked)
                    {
                        string msg = string.Format("{0} 号加班申请单已经处理完毕，部门负责人审批未批准", m_billNo);
                        List<string> noticeUser = new List<string>();

                        noticeUser.Add(txtApplicant.Tag.ToString());

                        m_billMessageServer.EndFlowMessage(m_billNo, msg, null, noticeUser);

                        MessageDialog.ShowPromptMessage("加班申请单已经处理完毕，部门负责人审批未批准!");
                        this.Close();
                    }
                    else
                    {
                        string msg = string.Format("{0} 号加班申请单已经处理完毕", m_billNo);
                        List<string> noticeUser = new List<string>(); 

                        noticeUser.Add(txtApplicant.Tag.ToString());

                        m_billMessageServer.EndFlowMessage(m_billNo, msg, null, noticeUser);

                        MessageDialog.ShowPromptMessage("加班申请单已经处理完毕!");
                        this.Close();
                    }
                }
                else if (overTime.BillStatus == OverTimeBillStatus.确认加班完成情况.ToString())
                {
                    string msg = string.Format("{0} 号加班单部门负责人审批成功，等待确认加班完成情况", m_billNo);
                    m_billMessageServer.PassFlowMessage(m_billNo, msg, BillFlowMessage_ReceivedUserType.用户, BasicInfo.LoginID);
                }
                else
                {
                    string msg = string.Format("{0} 号加班单部门负责人审批成功,请分管领导人审批", m_billNo);
                    m_billMessageServer.PassFlowMessage(m_billNo, msg, BillFlowMessage_ReceivedUserType.角色, 
                        m_billMessageServer.GetDeptLeaderRoleName(m_personnerServer.GetPersonnelViewInfo(
                        txtApplicant.Tag.ToString()).部门编号).ToList());
                }

                this.Close();
            }
            else
            {
                MessageDialog.ShowPromptMessage("请确认单据状态！");
            }
        }

        private void 分管领导审批toolStripButton2_Click(object sender, EventArgs e)
        {
            if (lblStatus.Text.Trim() == OverTimeBillStatus.等待分管领导审批.ToString())
            {
                HR_OvertimeBill overTime = new HR_OvertimeBill();

                overTime.ID = Convert.ToInt32(m_billNo);
                overTime.Leader = BasicInfo.LoginID;
                overTime.LeaderSignatureDate = ServerTime.Time;
                overTime.RealHours = Convert.ToDouble(numHours.Value);

                if (!cbAuthorize.Checked)
                {
                    if (MessageBox.Show("您是否同意【" + txtApplicant.Text + "】的加班申请？", "消息", 
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cbAuthorize.Checked = true;
                    }
                }

                overTime.Authorize = cbAuthorize.Checked;
                overTime.BillStatus = OverTimeBillStatus.已完成.ToString();

                if (!m_overTimeServer.UpdateOverTimeBill(overTime, "分管领导审批", out error))
                {
                    MessageDialog.ShowPromptMessage(error);
                    return;
                }

                if (overTime.BillStatus == OverTimeBillStatus.已完成.ToString())
                {
                    if (!cbAuthorize.Checked)
                    {
                        string msg = string.Format("{0} 号加班申请已经处理完毕，分管领导审批未批准", m_billNo);
                        List<string> noticeUser = new List<string>();

                        noticeUser.Add(txtApplicant.Tag.ToString());

                        m_billMessageServer.EndFlowMessage(m_billNo, msg, null, noticeUser);

                        MessageDialog.ShowPromptMessage("加班申请已经处理完毕，审批未批准!");
                    }
                    else
                    {
                        string msg = string.Format("{0} 号加班申请单已经处理完毕", m_billNo);
                        List<string> noticeUser = new List<string>();
                        noticeUser.Add(txtApplicant.Tag.ToString());

                        m_billMessageServer.EndFlowMessage(m_billNo, msg, null, noticeUser);

                        MessageDialog.ShowPromptMessage("加班申请单已经处理完毕!");
                        this.Close();
                    }
                }
                else
                {
                    string msg = string.Format("{0} 号加班单分管领导人审批成功，等待确认加班完成情况", m_billNo);
                    m_billMessageServer.PassFlowMessage(m_billNo, msg, BillFlowMessage_ReceivedUserType.用户, BasicInfo.LoginID);

                    MessageDialog.ShowPromptMessage("分管领导人审批成功!");
                }

                this.Close();
            }
            else
            {
                MessageDialog.ShowPromptMessage("请确认单据状态！");
            }
        }

        private void 人力toolStripButton_Click(object sender, EventArgs e)
        {
            if (lblStatus.Text.Trim() == OverTimeBillStatus.等待人力资源复核.ToString())
            {
                HR_OvertimeBill overTime = new HR_OvertimeBill();

                overTime.ID = Convert.ToInt32(m_billNo);
                overTime.HR_Signature = BasicInfo.LoginID;
                overTime.HR_SignatureDate = ServerTime.Time;

                overTime.BillStatus = OverTimeBillStatus.确认加班完成情况.ToString();

                if (!m_overTimeServer.UpdateOverTimeBill(overTime, "人力资源", out error))
                {
                    MessageDialog.ShowPromptMessage(error);
                    return;
                }

                string msg = string.Format("{0} 号加班单人力资源复核成功，等待确认加班完成情况", m_billNo);
                m_billMessageServer.PassFlowMessage(m_billNo, msg, BillFlowMessage_ReceivedUserType.角色, 
                    m_billMessageServer.GetDeptLeaderRoleName(m_personnerServer.GetPersonnelViewInfo(
                    txtApplicant.Tag.ToString()).部门编号).ToList());
                this.Close();
            }
            else
            {
                MessageDialog.ShowPromptMessage("请确认单据状态！");
            }
        }

        private void 完成情况toolStripButton_Click(object sender, EventArgs e)
        {
            if (lblStatus.Text.Trim() == OverTimeBillStatus.确认加班完成情况.ToString())
            {
                if (BasicInfo.LoginID != txtApplicant.Tag.ToString())
                {
                    IQueryable<View_HR_PersonnelArchive> directorGroup = m_personnerServer.GetDeptDirector(
                        m_personnerServer.GetPersonnelInfo(txtApplicant.Tag.ToString()).Dept, "0");
                    IQueryable<View_HR_PersonnelArchive> directorGroup1 = m_personnerServer.GetDeptDirector(
                        m_personnerServer.GetPersonnelInfo(txtApplicant.Tag.ToString()).Dept, "1");
                    IQueryable<View_HR_PersonnelArchive> directorGroup2 = m_personnerServer.GetDeptDirector(
                        m_personnerServer.GetPersonnelInfo(txtApplicant.Tag.ToString()).Dept, "2");
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
                    
                    if (directorGroup1 != null && directorGroup1.Count() > 0)
                    {
                        foreach (var item in directorGroup1)
                        {
                            if (BasicInfo.LoginID == item.员工编号)
                            {
                                flag = true;
                                break;
                            }
                        }
                    }
                    
                    if (directorGroup2 != null && directorGroup2.Count() > 0)
                    {
                        foreach (var item in directorGroup2)
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
                        if (!cbVerify.Checked)
                        {
                            if (MessageBox.Show("您是否确定【" + txtApplicant.Text + "】的加班完成？", "消息", 
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                cbAuthorize.Checked = true;
                            }
                        }

                        HR_OvertimeBill overTime = new HR_OvertimeBill();

                        overTime.ID = Convert.ToInt32(m_billNo);
                        overTime.VerifyFinish = cbVerify.Checked;
                        overTime.Verifier = BasicInfo.LoginID;
                        overTime.VerifyHours = numVerifyHours.Value;
                        overTime.VerifySignaturDate = ServerTime.Time;
                        overTime.BillStatus = OverTimeBillStatus.已完成.ToString();

                        if (!m_overTimeServer.UpdateOverTimeBill(overTime, "确认加班完成情况", out error))
                        {
                            MessageDialog.ShowPromptMessage(error);
                            return;
                        }

                        string msg = string.Format("{0} 号加班单确认加班完成情况成功", m_billNo);
                        List<string> noticeUser = new List<string>();

                        noticeUser.Add(txtApplicant.Tag.ToString());

                        m_billMessageServer.EndFlowMessage(m_billNo, string.Format("{0} 号加班申请已经处理完毕", m_billNo), null, noticeUser);

                        MessageDialog.ShowPromptMessage("确认加班完成情况成功!");
                        this.Close();
                    }
                    else
                    {
                        MessageDialog.ShowPromptMessage("只有部门主管、负责人、分管领导才可以对单据进行确认！");
                        return;
                    }
                }
                else
                {
                    MessageDialog.ShowPromptMessage("只有部门主管、负责人、分管领导才可以对单据进行确认！");
                    return;
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请确认单据状态！");
            }

            this.Close();
        }

        void DateTimePick_ValueChanged(object sender, EventArgs e)
        {
            double hours = (dtpEndTime.Value - dtpBeginTime.Value).Hours;
            int days = (dtpEndTime.Value - dtpBeginTime.Value).Days;

            if (days > 0)
            {
                hours += 24 * days;
            }

            if ((dtpEndTime.Value - dtpBeginTime.Value).Minutes >= 30)
            {
                hours += 0.5;
            }

            if (Convert.ToDateTime(dtpBeginTime.Value.ToShortTimeString()) < Convert.ToDateTime(m_punchInMorningEnd)
                && Convert.ToDateTime(dtpEndTime.Value.ToShortTimeString()) > Convert.ToDateTime(m_punchInAfternoon))
            {
                hours = hours - m_restHours;
            }

            numHours.Value = Convert.ToDecimal(hours);
            numVerifyHours.Value = Convert.ToDecimal(hours);
        }
    }
}
