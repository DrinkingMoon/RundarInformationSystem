using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using Service_Peripheral_HR;
using ServerModule;
using Expression;
using GlobalObject;
using UniversalControlLibrary;
using CommonBusinessModule;

namespace Form_Peripheral_HR
{
    /// <summary>
    /// 出差申请表子界面
    /// </summary>
    public partial class FormSchedule : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string error;

        /// <summary>
        /// 应打卡时间
        /// </summary>
        string punchInTime = "";

        /// <summary>
        /// 应上班小时数
        /// </summary>
        double m_workHours = 8;

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
        /// 单据编号
        /// </summary>
        string m_billNo;

        /// <summary>
        /// 最高部门
        /// </summary>
        string m_highDept;

        /// <summary>
        /// 获取单据编号
        /// </summary>
        public string BillNo
        {
            get { return m_billNo; }
        }

        /// <summary>
        /// 行程安排选中的行
        /// </summary>
        int m_dataGridViewSelectRow;

        /// <summary>
        /// 权限控制标志
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 部门信息服务组件
        /// </summary>
        IOrganizationServer m_departmentServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IOrganizationServer>();

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 岗位管理服务类
        /// </summary>
        IOperatingPostServer m_PostServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IOperatingPostServer>();

        /// <summary>
        /// 人员档案管理类
        /// </summary>
        IPersonnelArchiveServer m_personnerServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IPersonnelArchiveServer>();

        /// <summary>
        /// 考勤异常登记操作类
        /// </summary>
        ITimeExceptionServer m_timeExServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<ITimeExceptionServer>();

        /// <summary>
        /// 人员考勤流水账操作类
        /// </summary>
        IAttendanceDaybookServer m_dayBookServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IAttendanceDaybookServer>();

        /// <summary>
        /// 考勤方案操作类
        /// </summary>
        IAttendanceSchemeServer m_attendanceSchemeServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IAttendanceSchemeServer>();

        /// <summary>
        /// 考勤机导入的人员考勤明细表操作类
        /// </summary>
        IAttendanceMachineServer m_attendanceMachineServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IAttendanceMachineServer>();

        /// <summary>
        /// 出差申请表操作类
        /// </summary>
        IOnBusinessBillServer m_onBusinessServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IOnBusinessBillServer>();

        public FormSchedule(AuthorityFlag authFlag, int billID)
        {
            InitializeComponent();

            IQueryable<HR_AttendanceScheme> dtScheme = m_attendanceSchemeServer.GetLinqResult();

            m_punchInMorning = dtScheme.Take(1).Single().BeginTimeInTheMorning.Value.Hour.ToString() +
                ":" + dtScheme.Take(1).Single().EndTimeInTheAfternoon.Value.Minute.ToString();

            m_punchInMorningEnd = dtScheme.Take(1).Single().EndTimeInTheMorning.Value.Hour.ToString() +
                ":" + dtScheme.Take(1).Single().EndTimeInTheAfternoon.Value.Minute.ToString();

            m_punchInAfternoonEnd = dtScheme.Take(1).Single().EndTimeInTheAfternoon.Value.Hour.ToString() +
                ":" + dtScheme.Take(1).Single().EndTimeInTheAfternoon.Value.Minute.ToString();

            m_punchInAfternoon = dtScheme.Take(1).Single().BeginTimeInTheAfternoon.Value.Hour.ToString() +
                ":" + dtScheme.Take(1).Single().BeginTimeInTheAfternoon.Value.Minute.ToString();

            m_hours = 8;

            if (billID != 0)
            {
                m_billNo = billID.ToString();
                BindControl(m_billNo);
            }
            else
            {
                txtApplicant.Text = BasicInfo.LoginName;
                txtApplicant.Tag = BasicInfo.LoginID;
                txtApplicantDept.Text = BasicInfo.DeptName;
                txtApplicantDept.Tag = BasicInfo.DeptCode;
                lblStatus.Text = OnBusinessBillStatus.新建单据.ToString();

                cbInBudget.Visible = false;
                cbAuthorize.Visible = false;
                dtpETD.Value = ServerTime.Time;
                dtpETR.Value = ServerTime.Time;
            }

            m_billMessageServer.BillType = "出差申请单";
            m_authFlag = authFlag;
            menuStrip1.Visible = true;

            DataTable dt = m_personnerServer.GetHighestDept(txtApplicant.Tag.ToString());

            if (dt != null && dt.Rows.Count > 0)
            {
                m_highDept = dt.Rows[0]["deptCode"].ToString();
            }

            if (lblStatus.Text.Trim() == OnBusinessBillStatus.等待销差人确认.ToString())
            {
                dtpETD.Enabled = false;
                dtpETR.Enabled = false;
            }

            if (lblStatus.Text.Trim() != OnBusinessBillStatus.等待部门负责人审核.ToString() &&
                lblStatus.Text.Trim() != OnBusinessBillStatus.等待出差结果说明.ToString() &&
                lblStatus.Text.Trim() != OnBusinessBillStatus.等待分管领导审批.ToString() &&
                lblStatus.Text.Trim() != OnBusinessBillStatus.等待随行人员部门确认.ToString() &&
                lblStatus.Text.Trim() != OnBusinessBillStatus.等待销差人确认.ToString() &&
                lblStatus.Text.Trim() != OnBusinessBillStatus.等待总经理批准.ToString() &&
                lblStatus.Text.Trim() != OnBusinessBillStatus.审批未通过.ToString() &&
                lblStatus.Text.Trim() != OnBusinessBillStatus.新建单据.ToString() &&
                lblStatus.Text.Trim() != OnBusinessBillStatus.已完成.ToString())
            {
                高级负责人审批ToolStripMenuItem.Visible = true;
                string dept = UniversalFunction.GetDeptName(m_billMessageServer.GetHighestDeptCode(
                    UniversalFunction.GetPersonnelInfo(txtApplicant.Tag.ToString()).部门编码));

                高级负责人审批ToolStripMenuItem.Text = dept + "负责人审批";
                部门主管审核ToolStripMenuItem.Visible = false;
            }
            else
            {
                高级负责人审批ToolStripMenuItem.Visible = false;
                高级负责人审批ToolStripMenuItem.Enabled = false;
            }
        }

        private void FormSchedule_Load(object sender, EventArgs e)
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
        }

        /// <summary>
        /// 为控件赋值
        /// </summary>
        /// <param name="billID">单据号</param>
        void BindControl(string billID)
        {
            DataTable dt = m_onBusinessServer.GetOnBusinessBillByID(Convert.ToInt32(m_billNo));

            lblStatus.Text = dt.Rows[0]["单据状态"].ToString();
            txtApplicantDept.Tag = dt.Rows[0]["部门编码"].ToString();
            txtApplicantDept.Text = dt.Rows[0]["申请部门"].ToString();
            txtApplicant.Text = dt.Rows[0]["申请人姓名"].ToString();
            txtApplicant.Tag = dt.Rows[0]["员工编号"].ToString();
            txtConfirmor.Text = dt.Rows[0]["销差确认人"].ToString();
            txtDeptPrincipal.Text = dt.Rows[0]["部门负责人"].ToString();
            txtGeneralManager.Text = dt.Rows[0]["总经理"].ToString();
            txtLeader.Text = dt.Rows[0]["分管领导"].ToString();
            txtPurpose.Text = dt.Rows[0]["出差目的"].ToString();
            txtResult.Text = dt.Rows[0]["出差结果说明"].ToString();
            cbAuthorize.Checked = Convert.ToBoolean(dt.Rows[0]["是否批准"].ToString());
            
            if (dt.Rows[0]["是否借款"].ToString() == "是")
            {
                cbBorrowing.Checked = true;
            }
            else
            {
                cbBorrowing.Checked = false;
            }

            if (dt.Rows[0]["是否在预算范围内"].ToString() == "是")
            {
                cbInBudget.Checked = true;
            }
            else
            {
                cbInBudget.Checked = false;
            }

            if (dt.Rows[0]["销差确认时间"].ToString() != "")
            {
                dtpConfirmorDate.Value = Convert.ToDateTime(dt.Rows[0]["销差确认时间"].ToString());
            }

            dtpDate.Value = Convert.ToDateTime(dt.Rows[0]["申请时间"].ToString());
            dtpETD.Value = Convert.ToDateTime(dt.Rows[0]["预定出发时间"].ToString());
            dtpETD.Checked = true;
            dtpETR.Value = Convert.ToDateTime(dt.Rows[0]["预定返程时间"].ToString());
            dtpETR.Checked = true;
            numBorrowingAmount.Value = Convert.ToDecimal(dt.Rows[0]["借款金额"].ToString());
            numExprense.Value = Convert.ToDecimal(dt.Rows[0]["预计招待费"].ToString());
            numOtherExprense.Value = Convert.ToDecimal(dt.Rows[0]["预计其他费用"].ToString());
            dtpRealBeginTime.Value = Convert.ToDateTime(dt.Rows[0]["实际出发时间"].ToString());
            dtpRealEndTime.Value = Convert.ToDateTime(dt.Rows[0]["实际返程时间"].ToString());

            if (dt.Rows[0]["签定时间"].ToString() != "")
            {
                dtpDeptDate.Value = Convert.ToDateTime(dt.Rows[0]["签定时间"].ToString());
            }
            else
            {
                dtpDeptDate.Value = ServerTime.Time;
            }

            if (dt.Rows[0]["领导签定时间"].ToString() != "")
            {
                dtpLeaderDate.Value = Convert.ToDateTime(dt.Rows[0]["领导签定时间"].ToString());
            }
            else
            {
                dtpLeaderDate.Value = ServerTime.Time;
            }

            if (dt.Rows[0]["总经理签定时间"].ToString() != "")
            {
                dtpGMDate.Value = Convert.ToDateTime(dt.Rows[0]["总经理签定时间"].ToString());
            }
            else
            {
                dtpGMDate.Value = ServerTime.Time;
            }

            if (dt.Rows[0]["销差确认时间"].ToString() != "")
            {
                dtpConfirmorDate.Value = Convert.ToDateTime(dt.Rows[0]["销差确认时间"].ToString());
            }
            else
            {
                dtpConfirmorDate.Value = ServerTime.Time;
            }

            if (lblStatus.Text == OnBusinessBillStatus.等待销差人确认.ToString())
            {
                dtpRealBeginTime.Value = dtpETD.Value;
                dtpRealEndTime.Value = dtpETR.Value;
            }

            //if (dt.Rows[0]["交通工具"].ToString().Contains("飞机"))
            //{
            //    cbPlane.Checked = true;
            //}

            //if (dt.Rows[0]["交通工具"].ToString().Contains("火车"))
            //{
            //    cbTrain.Checked = true;
            //}

            //if (dt.Rows[0]["交通工具"].ToString().Contains("轮船"))
            //{
            //    cbSteamer.Checked = true;
            //}

            //if (dt.Rows[0]["交通工具"].ToString().Contains("其他"))
            //{
            //    cbOther.Checked = true;
            //}

            DataTable personnelDt = m_onBusinessServer.GetOnBusinessPersonnel(m_billNo);

            if (personnelDt != null && personnelDt.Rows.Count > 0)
            {
                dataGridView2.Rows.Clear();
                for (int i = 0; i < personnelDt.Rows.Count; i++)
                {
                    DataGridViewRow row = new DataGridViewRow();

                    dataGridView2.Rows.Add(new object[] { "", personnelDt.Rows[i]["员工编号"].ToString(),personnelDt.Rows[i]["员工姓名"].ToString(), personnelDt.Rows[i]["人员类别"].ToString(),
                    personnelDt.Rows[i]["随行人员部门确认"].ToString(),personnelDt.Rows[i]["随行人员部门确认时间"].ToString()});
                }

                for (int i = 0; i < dataGridView2.Columns.Count; i++)
                {
                    dataGridView2.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
            }

            DataTable scheduleDt = m_onBusinessServer.GetOnBusinessSchedule(m_billNo);

            if (scheduleDt != null && scheduleDt.Rows.Count > 0)
            {
                dataGridView1.Rows.Clear();
                for (int i = 0; i < scheduleDt.Rows.Count; i++)
                {
                    DataGridViewRow row = new DataGridViewRow();

                    dataGridView1.Rows.Add(new object[] {scheduleDt.Rows[i]["起始时间"].ToString(),scheduleDt.Rows[i]["截止时间"].ToString(), scheduleDt.Rows[i]["地点"].ToString(),scheduleDt.Rows[i]["交通工具"].ToString(),
                    scheduleDt.Rows[i]["工作内容"].ToString(),scheduleDt.Rows[i]["联系人"].ToString(),scheduleDt.Rows[i]["备注"].ToString()});
                }

                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }

            DataGridViewColumnCollection columns = this.dataGridView2.Columns;

            switch (columns[e.ColumnIndex].Name)
            {
                case "选择人员":
                    btnFindCode(dataGridView2.Rows[e.RowIndex]);
                    break;
            }
        }

        /// <summary>
        /// 查找人员
        /// </summary>
        private void btnFindCode(DataGridViewRow row)
        {
            FormPersonnel form = new FormPersonnel(row.Cells["员工姓名"], "员工姓名");
            form.ShowDialog();

            row.Cells["员工编号"].Value = form.UserCode;
            row.Cells["员工姓名"].Value = form.UserName;
            row.Cells["人员类别"].Value = "出差人员";
        }

        /// <summary>
        /// 检查控件
        /// </summary>
        /// <returns>完整返回True，否则返回False</returns>
        bool CheckControl()
        {
            if (dtpETD.Value > dtpETR.Value)
            {
                MessageDialog.ShowPromptMessage("【预计出差时间】必须小于【预计返程时间】");
                return false;
            }

            if (MessageDialog.ShowEnquiryMessage("请确认您的\r\n【预计出差时间】："
                + dtpETD.Value.ToString("yyyy-MM-dd HH:mm") + "\r\n【预计返程时间】："
                + dtpETR.Value.ToString("yyyy-MM-dd HH:mm")) == DialogResult.No)
            {
                return false;
            }

            if (txtPurpose.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请填写出差目的！");
                return false;
            }

            if (!dtpETR.Checked)
            {
                MessageDialog.ShowPromptMessage("请填写预计出差返程时间！");
                return false;
            }

            if (!dtpETD.Checked)
            {
                MessageDialog.ShowPromptMessage("请填写预计出差起始时间！");
                return false;
            }

            if (dtpETR.Value < dtpETD.Value)
            {
                MessageDialog.ShowPromptMessage("返程时间小于出发时间，请更正后提交！");
                return false;
            }

            if (dataGridView1.Rows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请填写行程安排！");
                return false;
            }

            if (dataGridView2.Rows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请填写出差人员！");
                return false;
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewCellCollection cells = dataGridView1.Rows[i].Cells;

                if (cells["起始时间"].Value == null)
                {
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells["起始时间"];
                    dataGridView1.BeginEdit(true);

                    MessageDialog.ShowPromptMessage(string.Format("第 {0} 行, 请填写起始时间", i + 1));
                    return false;
                }

                if (cells["截止时间"].Value == null)
                {
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells["截止时间"];
                    dataGridView1.BeginEdit(true);

                    MessageDialog.ShowPromptMessage(string.Format("第 {0} 行, 请填写截止时间", i + 1));
                    return false;
                }

                if (cells["地点"].Value == null || GlobalObject.GeneralFunction.IsNullOrEmpty(cells["地点"].Value.ToString()))
                {
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells["地点"];
                    dataGridView1.BeginEdit(true);

                    MessageDialog.ShowPromptMessage(string.Format("第 {0} 行, 请填写地点", i + 1));
                    return false;
                }

                if (cells["交通工具"].Value == null)
                {
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells["交通工具"];
                    dataGridView1.BeginEdit(true);

                    MessageDialog.ShowPromptMessage(string.Format("第 {0} 行, 请选择交通工具", i + 1));
                    return false;
                }

                if (cells["工作内容"].Value == null || GlobalObject.GeneralFunction.IsNullOrEmpty(cells["工作内容"].Value.ToString()))
                {
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells["工作内容"];
                    dataGridView1.BeginEdit(true);

                    MessageDialog.ShowPromptMessage(string.Format("第 {0} 行, 请填写工作内容", i + 1));
                    return false;
                }

                if (cells["联系人"].Value == null || GlobalObject.GeneralFunction.IsNullOrEmpty(cells["联系人"].Value.ToString()))
                {
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells["联系人"];
                    dataGridView1.BeginEdit(true);

                    MessageDialog.ShowPromptMessage(string.Format("第 {0} 行, 请填写联系人", i + 1));
                    return false;
                }
            }

            bool flag = false;

            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                DataGridViewCellCollection cells = dataGridView2.Rows[i].Cells;

                if (cells["人员类别"].Value == null || GlobalObject.GeneralFunction.IsNullOrEmpty(cells["人员类别"].Value.ToString()))
                {
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells["人员类别"];
                    dataGridView1.BeginEdit(true);

                    MessageDialog.ShowPromptMessage(string.Format("第 {0} 行, 请选择人员类别", i + 1));
                    return false;
                }

                if (BasicInfo.LoginName == cells["员工姓名"].Value.ToString())
                {
                    flag = true;
                }
            }

            if (!flag)
            {
                if (MessageDialog.ShowEnquiryMessage("出差人员是否包含申请人本人吗？") == DialogResult.Yes)
                {
                    return false;
                }
            }

            //if (!cbOther.Checked && !cbPlane.Checked && !cbSteamer.Checked && !cbTrain.Checked)
            //{
            //    MessageDialog.ShowPromptMessage("请选择交通工具！");
            //    return false;
            //}

            return true;
        }

        private void 提交单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (lblStatus.Text.Trim() != OnBusinessBillStatus.新建单据.ToString())
                {
                    MessageDialog.ShowPromptMessage("请确认单据状态！");
                    return;
                }

                if (!CheckControl())
                {
                    return;
                }

                List<HR_OnBusinessPersonnel> lstPersonnel = new List<HR_OnBusinessPersonnel>(dataGridView2.Rows.Count);
                List<HR_OnBusinessSchedule> lstSchedule = new List<HR_OnBusinessSchedule>(dataGridView1.Rows.Count);
                bool flag = false;
                bool flagGG = false;

                #region 2019-05-30 夏石友，根据集团文件高管也需要出差申请单
                bool flag出差人员中包含高管 = false;
                bool flag出差人员中包含非高管 = false;
                #endregion

                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    HR_OnBusinessPersonnel personnel = new HR_OnBusinessPersonnel();
                    DataGridViewCellCollection cells = dataGridView2.Rows[i].Cells;

                    personnel.WorkID = dataGridView2.Rows[i].Cells["员工编号"].Value.ToString();

                    if (dataGridView2.Rows[i].Cells["人员类别"].Value.ToString() == "随行人员")
                    {
                        flag = true;

                        if (UniversalFunction.GetPersonnelInfo(personnel.WorkID).部门名称 == "高管")
                        {
                            flagGG = true;
                        }
                    }
                    #region 2019-05-30 夏石友，根据集团文件高管也需要出差申请单
                    else if (dataGridView2.Rows[i].Cells["人员类别"].Value.ToString() == "出差人员")
                    {
                        if (UniversalFunction.GetPersonnelInfo(personnel.WorkID).部门名称 == "高管")
                        {
                            flag出差人员中包含高管 = true;
                        }
                        else
                        {
                            flag出差人员中包含非高管 = true;
                        }
                    }
                    #endregion

                    personnel.PersonnelType = dataGridView2.Rows[i].Cells["人员类别"].Value.ToString();
                    lstPersonnel.Add(personnel);
                }

                //#region 2019-05-30 夏石友，根据集团文件高管也需要出差申请单
                //if (flag出差人员中包含高管 && flag出差人员中包含非高管)
                //{
                //    MessageDialog.ShowPromptMessage("出差人员中不允许既包含高管又包含员工，高管请单独开出差申请单");
                //    return;
                //}
                //#endregion

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    HR_OnBusinessSchedule schedule = new HR_OnBusinessSchedule();
                    DataGridViewCellCollection cells = dataGridView1.Rows[i].Cells;

                    schedule.WorkContent = dataGridView1.Rows[i].Cells["工作内容"].Value.ToString();

                    try
                    {
                        schedule.StartTime = Convert.ToDateTime(dataGridView1.Rows[i].Cells["起始时间"].Value);
                        schedule.EndTime = Convert.ToDateTime(dataGridView1.Rows[i].Cells["截止时间"].Value);
                    }
                    catch (Exception)
                    {
                        MessageDialog.ShowPromptMessage("行程安排中的【时间】输入格式不正确，应为XXXX-XX-XX");
                        return;
                    }

                    if (dataGridView1.Rows[i].Cells["备注"].Value == null)
                    {
                        schedule.Remark = " ";
                    }
                    else
                    {
                        schedule.Remark = dataGridView1.Rows[i].Cells["备注"].Value.ToString();
                    }

                    schedule.Place = dataGridView1.Rows[i].Cells["地点"].Value.ToString();
                    schedule.Contact = dataGridView1.Rows[i].Cells["联系人"].Value.ToString();
                    schedule.Vehicle = dataGridView1.Rows[i].Cells["交通工具"].Value.ToString();

                    lstSchedule.Add(schedule);
                }

                HR_OnBusinessBill onBusiness = new HR_OnBusinessBill();

                onBusiness.Applicant = txtApplicant.Tag.ToString();
                onBusiness.ApplicantDate = ServerTime.Time;
                onBusiness.Authorize = false;
                onBusiness.BorrowingAmount = numBorrowingAmount.Value;
                onBusiness.EntertainmentExprense = numExprense.Value;
                onBusiness.ETD = dtpETD.Value;
                onBusiness.ETC = dtpETR.Value;
                onBusiness.IsBorrowing = cbBorrowing.Checked;
                onBusiness.WithinBudget = cbInBudget.Checked;
                onBusiness.OtherExprense = numOtherExprense.Value;
                onBusiness.Purpose = txtPurpose.Text;
                onBusiness.RealBeginTime = dtpETD.Value;
                onBusiness.RealEndTime = dtpETR.Value;
                onBusiness.Result = "";
                onBusiness.UpperBorrowing = CalculateClass.GetTotalPrice(numBorrowingAmount.Value);

                IQueryable<View_HR_PersonnelArchive> directorGroup = m_personnerServer.GetDeptDirector(txtApplicantDept.Tag.ToString(), "1");
                IQueryable<View_HR_PersonnelArchive> directorGroup2 = m_personnerServer.GetDeptDirector(txtApplicantDept.Tag.ToString(), "2");

                if (flag && !flagGG)
                {
                    onBusiness.BillStatus = OnBusinessBillStatus.等待随行人员部门确认.ToString();
                }
                else
                {
                    onBusiness.BillStatus = OnBusinessBillStatus.等待部门负责人审核.ToString();
                }

                if (!flag && directorGroup != null && directorGroup.Count() > 0)
                {
                    foreach (var item in directorGroup)
                    {
                        if (BasicInfo.LoginID == item.员工编号)
                        {
                            onBusiness.BillStatus = OnBusinessBillStatus.等待分管领导审批.ToString();
                            break;
                        }
                    }
                }

                if (!flag && directorGroup2 != null && directorGroup2.Count() > 0)
                {
                    foreach (var item in directorGroup2)
                    {
                        if (BasicInfo.LoginID == item.员工编号)
                        {
                            onBusiness.BillStatus = OnBusinessBillStatus.等待总经理批准.ToString();
                            break;
                        }
                    }
                }

                #region 2019-05-30 夏石友，根据集团文件高管也需要出差申请单
                if (flag出差人员中包含高管 && !flag出差人员中包含非高管)
                {
                    onBusiness.BillStatus = OnBusinessBillStatus.等待总经理批准.ToString();
                }
                #endregion

                m_billNo = m_onBusinessServer.AddOnBusinessBill(onBusiness, lstPersonnel, lstSchedule, out error).ToString();

                if (Convert.ToInt32(m_billNo) < 0)
                {
                    MessageDialog.ShowPromptMessage(error);
                    return;
                }
                else
                {
                    MessageDialog.ShowPromptMessage("新增成功！");
                    m_billMessageServer.DestroyMessage(m_billNo);

                    if (onBusiness.BillStatus.Equals(OnBusinessBillStatus.等待分管领导审批.ToString()))
                    {
                        m_billMessageServer.SendNewFlowMessage(m_billNo, string.Format("{0}号出差申请单，请分管领导审核", m_billNo),
                       BillFlowMessage_ReceivedUserType.角色, m_billMessageServer.GetDeptLeaderRoleName(m_personnerServer.GetPersonnelViewInfo(txtApplicant.Tag.ToString()).部门编号).ToList());
                    }
                    else if (onBusiness.BillStatus.Equals(OnBusinessBillStatus.等待总经理批准.ToString()))
                    {
                        m_billMessageServer.SendNewFlowMessage(m_billNo,
                            string.Format("{0}号出差申请单，请总经理批准", m_billNo), CE_RoleEnum.总经理);
                    }
                    else if (onBusiness.BillStatus == OnBusinessBillStatus.等待随行人员部门确认.ToString())
                    {
                        List<string> list = new List<string>();

                        foreach (var item in lstPersonnel)
                        {
                            if (item.PersonnelType == "随行人员")
                            {
                                DataTable dt = m_personnerServer.GetHighestDept(item.WorkID);

                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    string dept = dt.Rows[0]["deptCode"].ToString();
                                    list = m_billMessageServer.GetDeptPrincipalRoleName(dept).ToList();
                                }
                            }
                        }

                        if (flagGG)
                        {
                            m_billMessageServer.SendNewFlowMessage(m_billNo, string.Format("{0}号出差申请单，请随行人员部门负责人确认", m_billNo),
                                BillFlowMessage_ReceivedUserType.角色, CE_RoleEnum.总经理.ToString());
                        }
                        else
                        {
                            m_billMessageServer.SendNewFlowMessage(m_billNo, string.Format("{0}号出差申请单，请随行人员部门负责人确认", m_billNo),
                                BillFlowMessage_ReceivedUserType.角色, list);
                        }
                    }
                    else
                    {
                        m_billMessageServer.SendNewFlowMessage(m_billNo, string.Format("{0}号出差申请单，请部门负责人审核", m_billNo),
                            BillFlowMessage_ReceivedUserType.角色, m_billMessageServer.GetDeptPrincipalRoleName(
                            m_personnerServer.GetPersonnelViewInfo(txtApplicant.Tag.ToString()).部门编号).ToList());
                    }

                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void 修改单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lblStatus.Text.Trim() == OnBusinessBillStatus.已完成.ToString())
            {
                MessageDialog.ShowPromptMessage("单据已经完成，不能修改！");
                return;
            }

            if (!CheckControl())
            {
                return;
            }

            List<HR_OnBusinessPersonnel> lstPersonnel = new List<HR_OnBusinessPersonnel>(dataGridView2.Rows.Count);
            List<HR_OnBusinessSchedule> lstSchedule = new List<HR_OnBusinessSchedule>(dataGridView1.Rows.Count);

            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                HR_OnBusinessPersonnel personnel = new HR_OnBusinessPersonnel();
                DataGridViewCellCollection cells = dataGridView2.Rows[i].Cells;

                personnel.WorkID = dataGridView2.Rows[i].Cells["员工编号"].Value.ToString();
                personnel.PersonnelType = dataGridView2.Rows[i].Cells["人员类别"].Value.ToString();

                lstPersonnel.Add(personnel);
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                HR_OnBusinessSchedule schedule = new HR_OnBusinessSchedule();
                DataGridViewCellCollection cells = dataGridView1.Rows[i].Cells;

                schedule.WorkContent = dataGridView1.Rows[i].Cells["工作内容"].Value.ToString();

                try
                {
                    schedule.StartTime = Convert.ToDateTime(dataGridView1.Rows[i].Cells["起始时间"].Value);
                    schedule.EndTime = Convert.ToDateTime(dataGridView1.Rows[i].Cells["截止时间"].Value);
                }
                catch (Exception)
                {
                    MessageDialog.ShowPromptMessage("行程安排中的【时间】输入格式不正确，应为XXXX-XX-XX");
                    return;
                }

                if (dataGridView1.Rows[i].Cells["备注"].Value == null)
                {
                    schedule.Remark = "无";
                }
                else
                {
                    schedule.Remark = dataGridView1.Rows[i].Cells["备注"].Value.ToString();
                }

                schedule.Place = dataGridView1.Rows[i].Cells["地点"].Value.ToString();
                schedule.Contact = dataGridView1.Rows[i].Cells["联系人"].Value.ToString();
                schedule.Vehicle = dataGridView1.Rows[i].Cells["交通工具"].Value.ToString();

                lstSchedule.Add(schedule);
            }

            HR_OnBusinessBill onBusiness = new HR_OnBusinessBill();

            onBusiness.Applicant = txtApplicant.Tag.ToString();
            onBusiness.ApplicantDate = dtpDate.Value;
            onBusiness.Authorize = false;
            onBusiness.BorrowingAmount = numBorrowingAmount.Value;
            onBusiness.EntertainmentExprense = numExprense.Value;
            onBusiness.ETD = dtpETD.Value;
            onBusiness.ETC = dtpETR.Value;
            onBusiness.IsBorrowing = cbBorrowing.Checked;
            onBusiness.OtherExprense = numOtherExprense.Value;
            onBusiness.Purpose = txtPurpose.Text;
            onBusiness.WithinBudget = false;
            onBusiness.RealBeginTime = dtpETD.Value;
            onBusiness.RealEndTime = dtpETR.Value;

            if (!m_onBusinessServer.UpdateOnBusinessBill(onBusiness, lstPersonnel, lstSchedule, Convert.ToInt32(m_billNo), out error))
            {
                MessageDialog.ShowPromptMessage(error);
                return;
            }
            else
            {
                MessageDialog.ShowPromptMessage("修改成功！");
                this.Close();
            }
        }

        private void 提交出差结果ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            error = null;

            if (lblStatus.Text.Trim() != OnBusinessBillStatus.等待出差结果说明.ToString())
            {
                MessageDialog.ShowPromptMessage("请确认单据状态！");
                return;
            }

            DataTable tempTable =  GlobalObject.DataSetHelper.SiftDataTable(m_onBusinessServer.GetOnBusinessPersonnel(m_billNo), 
                "人员类别 = '出差人员'", out error);

            List<string> lstTemp = GlobalObject.DataSetHelper.ColumnsToList(tempTable, "员工编号");
            lstTemp.Add(txtApplicant.Tag.ToString());

            if (!lstTemp.Contains(BasicInfo.LoginID))
            {
                MessageDialog.ShowPromptMessage("只有单据申请人或者出差人员才能进行此操作！");
                return;
            }

            //if (txtApplicant.Tag.ToString() != BasicInfo.LoginID)
            //{
            //    MessageDialog.ShowPromptMessage("只有单据申请人才能进行此操作！");
            //    return;
            //}

            if (txtResult.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请填写出差结果说明！");
                return;
            }

            HR_OnBusinessBill bill = new HR_OnBusinessBill();

            bill.Result = txtResult.Text;
            bill.ID = Convert.ToInt32(m_billNo);
            bill.BillStatus = OnBusinessBillStatus.等待销差人确认.ToString();

            if (!m_onBusinessServer.UpdateOnBusinessBill(bill, "出差结果说明", out error))
            {
                MessageDialog.ShowPromptMessage(error);
                return;
            }

            string msg = string.Format("{0} 号出差申请单出差报告完成,请销差人确认", m_billNo);
            m_billMessageServer.PassFlowMessage(m_billNo, msg, BillFlowMessage_ReceivedUserType.用户, txtApplicant.Tag.ToString());

            this.Close();
        }

        private void 部门主管审核ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lblStatus.Text.Trim() != OnBusinessBillStatus.等待部门负责人审核.ToString())
            {
                MessageDialog.ShowPromptMessage("请确认单据状态！");
                return;
            }

            IQueryable<View_HR_PersonnelArchive> directorGroup = m_personnerServer.GetDeptDirector(
                m_personnerServer.GetPersonnelInfo(txtApplicant.Tag.ToString()).Dept, "1");
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

            if (!flag)
            {
                MessageDialog.ShowPromptMessage("您不是申请人的部门负责人（或者您没有审核权限）！");
            }

            if (!cbAuthorize.Checked)
            {
                if (MessageDialog.ShowEnquiryMessage("您是否同意【" + txtApplicant.Text.Trim() + "】本次的出差?") == DialogResult.Yes)
                {
                    cbAuthorize.Checked = true;
                }
            }

            HR_OnBusinessBill bill = new HR_OnBusinessBill();

            bill.DeptPrincipal = BasicInfo.LoginID;
            bill.DeptSignatureDate = ServerTime.Time;
            bill.WithinBudget = cbInBudget.Checked;
            bill.ID = Convert.ToInt32(m_billNo);
            bill.Authorize = cbAuthorize.Checked;

            if (!cbAuthorize.Checked)
            {
                bill.BillStatus = OnBusinessBillStatus.审批未通过.ToString();
            }
            else
            {
                bill.BillStatus = OnBusinessBillStatus.等待分管领导审批.ToString();
            }

            if (!m_onBusinessServer.UpdateOnBusinessBill(bill, "部门主管审批", out error))
            {
                MessageDialog.ShowPromptMessage(error);
                return;
            }

            MessageDialog.ShowPromptMessage("部门负责人审核成功！");

            m_billMessageServer.PassFlowMessage(m_billNo, string.Format("{0}号出差申请单，请分管领导审核", m_billNo),
                BillFlowMessage_ReceivedUserType.角色,
                      m_billMessageServer.GetDeptLeaderRoleName(
                      m_personnerServer.GetPersonnelViewInfo(txtApplicant.Tag.ToString()).部门编号).ToList());

            this.Close();
        }

        private void 分管领导审批ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lblStatus.Text.Trim() != OnBusinessBillStatus.等待分管领导审批.ToString())
            {
                MessageDialog.ShowPromptMessage("请确认单据状态！");
                return;
            }

            IQueryable<View_HR_PersonnelArchive> directorGroup = m_personnerServer.GetDeptDirector(
                UniversalFunction.GetPersonnelInfo(txtApplicant.Tag.ToString()).部门编码, "2");

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

            if (!flag)
            {
                MessageDialog.ShowPromptMessage("请" + UniversalFunction.GetPersonnelInfo(txtApplicant.Tag.ToString()).部门名称 + "的分管领导审批！");
                return;
            }
            
            if (!cbAuthorize.Checked)
            {
                if (MessageDialog.ShowEnquiryMessage("您是否同意【" + txtApplicant.Text.Trim() + "】本次的出差?") == DialogResult.Yes)
                {
                    cbAuthorize.Checked = true;
                }
            }

            HR_OnBusinessBill bill = new HR_OnBusinessBill();

            bill.LeaderSignature = BasicInfo.LoginID;
            bill.LeaderSignatureDate = ServerTime.Time;
            bill.Authorize = cbAuthorize.Checked;
            bill.ID = Convert.ToInt32(m_billNo); 

            if (!cbAuthorize.Checked)
            {
                bill.BillStatus = OnBusinessBillStatus.审批未通过.ToString();
            }
            else
            {
                bill.BillStatus = OnBusinessBillStatus.等待出差结果说明.ToString();

                List<HR_OnBusinessPersonnel> lstPersonnel = m_onBusinessServer.GetPersonnel(bill.ID);

                if (lstPersonnel != null)
                {
                    foreach (HR_OnBusinessPersonnel per in lstPersonnel)
                    {
                        if (per.PersonnelType == "出差人员" 
                            && UniversalFunction.GetPersonnelInfo(per.WorkID).部门名称 == "高管")
                        {
                            bill.BillStatus = OnBusinessBillStatus.等待总经理批准.ToString();
                            break;
                        }
                    }
                }
            }

            if (!m_onBusinessServer.UpdateOnBusinessBill(bill, "分管领导审批", out error))
            {
                MessageDialog.ShowPromptMessage(error);
                return;
            }

            string msg;

            switch (GeneralFunction.StringConvertToEnum<OnBusinessBillStatus>(bill.BillStatus))
            {
                case OnBusinessBillStatus.审批未通过:

                    msg = string.Format("{0} 号出差申请分管领导审批未通过！", m_billNo);
                    List<string> noticeRoles = new List<string>();

                    noticeRoles.Add(txtApplicant.Tag.ToString());
                    m_billMessageServer.EndFlowMessage(m_billNo, string.Format("{0} 号出差申请已经处理完毕", m_billNo), null, noticeRoles);
                    break;
                case OnBusinessBillStatus.等待出差结果说明:

                    msg = string.Format("{0} 号出差申请分管领导审批成功,请出差人员回来后填写出差报告", m_billNo);
                    m_billMessageServer.PassFlowMessage(m_billNo, msg, BillFlowMessage_ReceivedUserType.用户, txtApplicant.Tag.ToString());
                    break;
                case OnBusinessBillStatus.等待总经理批准:

                    m_billMessageServer.SendNewFlowMessage(m_billNo,
                        string.Format("{0}号出差申请单，请总经理批准", m_billNo), CE_RoleEnum.总经理);
                    break;
                default:
                    break;
            }

            this.Close();
        }

        private void 总经理审批ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lblStatus.Text.Trim() != OnBusinessBillStatus.等待总经理批准.ToString())
            {
                MessageDialog.ShowPromptMessage("请确认单据状态！");
                return;
            }

            if (!cbAuthorize.Checked)
            {
                if (MessageDialog.ShowEnquiryMessage("您是否同意【" + txtApplicant.Text.Trim()+ "】提交的出差申请单?") == DialogResult.Yes)
                {
                    cbAuthorize.Checked = true;
                }
            }

            HR_OnBusinessBill bill = new HR_OnBusinessBill();

            bill.GeneralManager = BasicInfo.LoginID;
            bill.GM_SignatureDate = ServerTime.Time;
            bill.ID = Convert.ToInt32(m_billNo);

            if (cbAuthorize.Checked)
            {
                bill.BillStatus = OnBusinessBillStatus.等待出差结果说明.ToString();
            }
            else
            {
                bill.BillStatus = OnBusinessBillStatus.审批未通过.ToString();
            }

            bill.Authorize = cbAuthorize.Checked;

            if (!m_onBusinessServer.UpdateOnBusinessBill(bill, "总经理审批", out error))
            {
                MessageDialog.ShowPromptMessage(error);
                return;
            }

            if (bill.BillStatus.Equals(OnBusinessBillStatus.等待出差结果说明.ToString()))
            {
                DataTable dt = m_onBusinessServer.GetOnBusinessPersonnel(m_billNo);
                int type = m_attendanceMachineServer.GetExceptionTypeID(ExceptionType.出差.ToString());

                if (dt != null && dt.Rows.Count > 0)
                {
                    MessageDialog.ShowPromptMessage("总经理审批成功！");
                    string msg = string.Format("{0} 号出差申请总经理审批成功，请出差人员回来后填写出差报告", m_billNo);
                    m_billMessageServer.PassFlowMessage(m_billNo, msg, BillFlowMessage_ReceivedUserType.用户, txtApplicant.Tag.ToString());
                }
            }
            else
            {
                string msg = string.Format("{0} 号出差申请总经理审批未通过！", m_billNo);
                List<string> noticeRoles = new List<string>();

                noticeRoles.Add(txtApplicant.Tag.ToString());
                m_billMessageServer.EndFlowMessage(m_billNo, string.Format("{0} 号出差申请已经处理完毕", m_billNo), null, noticeRoles);
            }

            this.Close();
        }

        private void 随行人员部门负责人确认ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lblStatus.Text.Trim() != OnBusinessBillStatus.等待随行人员部门确认.ToString())
            {
                MessageDialog.ShowPromptMessage("请确认单据状态！");
                return;
            }

            HR_OnBusinessPersonnel personnel = new HR_OnBusinessPersonnel();

            DataTable dt = m_onBusinessServer.GetOnBusinessPersonnel(m_billNo);
            bool b = false;

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["人员类别"].ToString() == "随行人员")
                    {
                        IQueryable<View_HR_PersonnelArchive> directorGroup1 = m_personnerServer.GetDeptDirector(
                            m_personnerServer.GetPersonnelInfo(dt.Rows[i]["员工编号"].ToString()).Dept, "0");
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
                        
                        if(!flagPri)
                        {
                            directorGroup1 = m_personnerServer.GetDeptDirector(
                            m_personnerServer.GetPersonnelInfo(dt.Rows[i]["员工编号"].ToString()).Dept, "1");

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
                        }

                        if (flagPri)
                        {
                            b = true;
                            personnel.DeptPrincipal = BasicInfo.LoginName;
                            personnel.DeptSignatureDate = ServerTime.Time;
                            personnel.WorkID = dt.Rows[i]["员工编号"].ToString();

                            if (!m_onBusinessServer.UpdateOnBusinessPersonnel(personnel, Convert.ToInt32(m_billNo), out error))
                            {
                                MessageDialog.ShowPromptMessage(error);
                                return;
                            }
                        }
                    }
                }
            }

            if (b)
            {
                HR_OnBusinessBill bill = new HR_OnBusinessBill();

                bill.ID = Convert.ToInt32(m_billNo);

                dt = m_onBusinessServer.GetOnBusinessPersonnel(m_billNo);

                if (dt != null && dt.Rows.Count > 0)
                {
                    bool flag = false;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["人员类别"].ToString() == "随行人员" && dt.Rows[i]["随行人员部门确认"].ToString() == "")
                        {
                            flag = true;
                            break;
                        }
                    }

                    if (flag)
                    {
                        bill.BillStatus = OnBusinessBillStatus.等待随行人员部门确认.ToString();
                    }
                    else
                    {
                        bill.BillStatus = OnBusinessBillStatus.等待部门负责人审核.ToString();

                        if (!m_onBusinessServer.UpdateOnBusinessBill(bill, "随行人员部门确认", out error))
                        {
                            MessageDialog.ShowPromptMessage(error);
                            return;
                        }

                        MessageDialog.ShowPromptMessage("随行人员部门确认成功！");
                        string msg = string.Format("{0} 号出差申请单随行人员部门确认成功，等待部门负责人审核", m_billNo);
                        m_billMessageServer.PassFlowMessage(m_billNo, msg, BillFlowMessage_ReceivedUserType.角色,
                            m_billMessageServer.GetDeptPrincipalRoleName(m_personnerServer.GetPersonnelViewInfo(txtApplicant.Tag.ToString()).部门编号).ToList());
                    }
                }
            }

            this.Close();
        }

        private void 销差人确认ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lblStatus.Text.Trim() != OnBusinessBillStatus.等待销差人确认.ToString())
            {
                MessageDialog.ShowPromptMessage("请确认单据状态！");
                return;
            }

            if (dtpRealBeginTime.Value > dtpRealEndTime.Value)
            {
                MessageDialog.ShowPromptMessage("【实际出差时间】必须小于【实际返程时间】");
                return;
            }

            if (MessageDialog.ShowEnquiryMessage("请确认您的\r\n【实际出差时间】："
                + dtpRealBeginTime.Value.ToString("yyyy-MM-dd HH:mm") + "\r\n【实际返程时间】："
                + dtpRealEndTime.Value.ToString("yyyy-MM-dd HH:mm")) == DialogResult.No)
            {
                return;
            }

            if (dtpRealEndTime.Value <= dtpRealBeginTime.Value)
            {
                MessageDialog.ShowPromptMessage("实际返回时间小于等于实际出差时间！");
                return;
            }

            HR_OnBusinessBill bill = new HR_OnBusinessBill();

            bill.Confirmor = BasicInfo.LoginID;
            bill.ConfirmorDate = ServerTime.Time;
            bill.RealBeginTime = dtpRealBeginTime.Value;
            bill.RealEndTime = dtpRealEndTime.Value;
            bill.ID = Convert.ToInt32(m_billNo);
            bill.BillStatus = OnBusinessBillStatus.已完成.ToString();

            if (!m_onBusinessServer.UpdateOnBusinessBill(bill, "销差人确认", out error))
            {
                MessageDialog.ShowPromptMessage(error);
                return;
            }

            MessageDialog.ShowPromptMessage("确认成功！");
            List<string> noticeUser = new List<string>();
            noticeUser.Add(txtApplicant.Tag.ToString());
            m_billMessageServer.EndFlowMessage(m_billNo, string.Format("{0} 号出差申请已经处理完毕", m_billNo), null, noticeUser);

            this.Close();
        }

        private void 添加行程安排ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dgvr in dataGridView1.Rows)
            {
                if (Convert.ToDateTime(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells["起始时间"].Value) >
                    Convert.ToDateTime(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells["截止时间"].Value))
                {
                    MessageDialog.ShowPromptMessage("第" + dgvr.Index + 1 + "行【行程】，【起始时间】必须小于【截止时间】");
                    return;
                }
            }

            DataGridViewRow dr = new DataGridViewRow();
            dataGridView1.Rows.Add(dr);

            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells["起始时间"].Value = dtpETD.Value;
            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells["截止时间"].Value = dtpETD.Value;

        }

        private void 删除行程安排ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.RemoveAt(m_dataGridViewSelectRow - 1);
        }

        private void 添加出差人员ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewRow dr = new DataGridViewRow();
            dataGridView2.Rows.Add(dr);
        }

        private void 删除出差人员ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            for (int i = dataGridView2.SelectedRows.Count; i > 0; i--)
            {
                dataGridView2.Rows.Remove(dataGridView2.SelectedRows[i - 1]);
            }
        }

        private void dtpRealBeginTime_ValueChanged(object sender, EventArgs e)
        {
            GetRealHours();

            if (dtpRealBeginTime.Value.Hour >= 8 && dtpRealBeginTime.Value.Hour < 13)
            {
                punchInTime = m_punchInMorning;
            }
            else if (dtpRealBeginTime.Value.Hour >= 13)
            {
                punchInTime = m_punchInAfternoonEnd;
            }
        }

        /// <summary>
        /// 获得实际出差时间
        /// </summary>
        /// <returns>返回出差的小时数</returns>
        private void GetRealHours()
        {
            int days = (dtpRealEndTime.Value - dtpRealBeginTime.Value).Days;
            double hours = days * m_workHours;
            double tempHours = ((dtpRealEndTime.Value - dtpRealBeginTime.Value).Hours);

            hours = ((dtpRealEndTime.Value - dtpRealBeginTime.Value).Hours);

            if (hours > 4)
            {
                if (dtpRealBeginTime.Value.Month > 4 && dtpRealBeginTime.Value.Month < 10)
                {
                    hours = hours - 1.5;
                }
                else
                {
                    hours = hours - 1;
                }
            }

            if ((dtpRealEndTime.Value - dtpRealBeginTime.Value).Minutes >= 30 && (dtpRealEndTime.Value.Minute != dtpRealBeginTime.Value.Minute))
            {
                hours = hours + 0.5;
            }
            else if ((dtpRealEndTime.Value - dtpRealBeginTime.Value).Minutes < 30 && (dtpRealEndTime.Value.Minute != dtpRealBeginTime.Value.Minute))
            {
                hours = hours - 1;
                hours = hours + 0.5;
            }

            m_hours = hours;
        }

        private void dtpRealEndTime_ValueChanged(object sender, EventArgs e)
        {
            GetRealHours();
        }

        /// <summary>
        /// 获得预计出差时间
        /// </summary>
        /// <returns>返回出差的小时数</returns>
        private void GetPredictHours()
        {
            int days = (dtpETR.Value - dtpETD.Value).Days;
            double hours = days * m_workHours;
            double tempHours = ((dtpETR.Value - dtpETD.Value).Hours);

            hours = ((dtpETR.Value - dtpETD.Value).Hours);

            if (hours > 4)
            {
                if (dtpETD.Value.Month > 4 && dtpETD.Value.Month < 10)
                {
                    hours = hours - 1.5;
                }
                else
                {
                    hours = hours - 1;
                }
            }

            if ((dtpETR.Value - dtpETD.Value).Minutes >= 30 && (dtpETR.Value.Minute != dtpETD.Value.Minute))
            {
                hours = hours + 0.5;
            }
            else if ((dtpETR.Value - dtpETD.Value).Minutes < 30 && (dtpETR.Value.Minute != dtpETD.Value.Minute))
            {
                hours = hours - 1;
                hours = hours + 0.5;
            }

            m_hours = hours;
        }

        private void dtpETD_ValueChanged(object sender, EventArgs e)
        {
            GetPredictHours();
        }

        private void dtpETR_ValueChanged(object sender, EventArgs e)
        {
            GetPredictHours();
        }

        private void 打印ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (BasicInfo.LoginID != txtApplicant.Tag.ToString())
            //{
            //    MessageDialog.ShowPromptMessage("只有申请人本人可以打印此单据！");
            //    return;
            //}

            if (lblStatus.Text.Trim() != LeaveBillStatus.新建单据.ToString())
            {

                IBillReportInfo report = new 报表_出差申请单(Convert.ToInt32(m_billNo));

                PrintReportBill print = new PrintReportBill(21.8,9.31,report);
                (report as 报表_出差申请单).ShowDialog();
            }
            else
            {
                MessageDialog.ShowPromptMessage("请先提交单据，然后再打印！");
                return;
            }
        }

        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView1.IsCurrentCellDirty)
            {
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dataGridView2_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView2.IsCurrentCellDirty)
            {
                dataGridView2.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageDialog.ShowPromptMessage("请在【申请人操作】中【添加行程安排】");
                return;
            }

            m_dataGridViewSelectRow = dataGridView1.CurrentRow.Index + 1;
        }

        private void 高级负责人审批ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string deptCode = m_billMessageServer.GetHighestDeptCode(UniversalFunction.GetPersonnelInfo(txtApplicant.Tag.ToString()).部门编码);

            IQueryable<View_HR_PersonnelArchive> directorGroup = m_personnerServer.GetDeptDirector(deptCode, "1");
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
                HR_OnBusinessBill bill = new HR_OnBusinessBill();

                bill.DeptPrincipal = BasicInfo.LoginID;
                bill.DeptSignatureDate = ServerTime.Time;
                bill.WithinBudget = cbInBudget.Checked;
                bill.ID = Convert.ToInt32(m_billNo);
                bill.BillStatus = OnBusinessBillStatus.等待分管领导审批.ToString();

                if (!m_onBusinessServer.UpdateOnBusinessBill(bill, "部门主管审批", out error))
                {
                    MessageDialog.ShowPromptMessage(error);
                    return;
                }

                MessageDialog.ShowPromptMessage("部门负责人审核成功！");

                m_billMessageServer.PassFlowMessage(m_billNo, string.Format("{0}号出差申请单，请分管领导审核", m_billNo),
                   BillFlowMessage_ReceivedUserType.角色,
                         m_billMessageServer.GetDeptLeaderRoleName(m_personnerServer.GetPersonnelViewInfo(txtApplicant.Tag.ToString()).部门编号).ToList());
            }
            else
            {
                MessageDialog.ShowPromptMessage("您不是【" + UniversalFunction.GetDeptName(deptCode) + "的负责人！】");
                return;
            }

            this.Close();
        }
    }
}
