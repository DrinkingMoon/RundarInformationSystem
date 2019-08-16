using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using GlobalObject;
using PlatformManagement;
using UniversalControlLibrary;
using Service_Project_Project;
using FlowControlService;
using System.Globalization;

namespace Form_Project_Project
{
    public partial class 项目工时表 : Form
    {
        ITimesheets _ServiceTimesheets = Service_Project_Project.ServerModuleFactory.GetServerModule<ITimesheets>();

        public 项目工时表()
        {
            InitializeComponent();
        }

        void ClearInfo()
        {
            cmbItemName.Text = "";
            numElapsedTime.Value = 0;
            dtpItemDate.Value = ServerTime.Time;
            txtDescription.Text = "";

            if (BasicInfo.DeptCode == "GH")
            {
                btnSetPersonnel.Visible = true;
            }
        }

        bool CheckInfo()
        {
            if (txtExecUser.Text.Trim().Length == 0 || txtExecUser.Tag == null || txtExecUser.Tag.ToString().Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请选择【执行人】");
                return false;
            }

            if (cmbItemName.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请选择【项目名称】或者自行填写");
                return false;
            }

            if (numElapsedTime.Value == 0)
            {
                MessageDialog.ShowPromptMessage("请填写【项目工时】");
                return false;
            }

            //if (!CheckDate())
            //{
            //    MessageDialog.ShowPromptMessage("【项目工作日期】超出填写日期范围");
            //    return false;
            //}

            return true;
        }

        Business_Project_Timesheets GetInfo()
        {
            Business_Project_Timesheets result = new Business_Project_Timesheets();

            result.ElapsedTime = numElapsedTime.Value;
            result.ItemDescription = txtDescription.Text;
            result.ItemName = cmbItemName.Text;
            result.ItemDate = dtpItemDate.Value.Date;
            result.RecordPersonnel = BasicInfo.LoginID;
            result.RecordDate = ServerTime.Time;
            result.ExecUser = txtExecUser.Tag.ToString();

            return result;
        }

        private void 项目工时表_Load(object sender, EventArgs e)
        {
            ClearInfo();
            customDataGridView1.DataSource = _ServiceTimesheets.GetInfo(BasicInfo.LoginID);

            txtExecUser.Text = UniversalFunction.GetPersonnelName(BasicInfo.LoginID);
            txtExecUser.Tag = BasicInfo.LoginID;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CheckInfo())
                {
                    return;
                }

                Business_Project_Timesheets timesheets = GetInfo();

                if (_ServiceTimesheets.IsOverTime(timesheets))
                {
                    MessageDialog.ShowPromptMessage("【项目工作日期】累计【项目工时】超过24小时");
                    return;
                }

                if (_ServiceTimesheets.IsRepeat(timesheets))
                {
                    MessageDialog.ShowPromptMessage("【项目工作日期】已存在一条【"+ timesheets.ItemName +"】记录");
                    return;
                }

                if (MessageDialog.ShowEnquiryMessage("请确认您填写的信息是否【提交】？") == DialogResult.Yes)
                {
                    _ServiceTimesheets.OperationInfo(CE_OperatorMode.添加, timesheets);
                    MessageDialog.ShowPromptMessage("提交成功");
                    ClearInfo();
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowErrorMessage(ex.Message);
            }

            customDataGridView1.DataSource = _ServiceTimesheets.GetInfo(BasicInfo.LoginID);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (customDataGridView1.CurrentRow == null)
                {
                    return;
                }

                if (!GlobalObject.GeneralFunction.IsInSameWeek(Convert.ToDateTime(customDataGridView1.CurrentRow.Cells["项目工作日期"].Value), ServerTime.Time))
                {
                    MessageDialog.ShowPromptMessage("不能【删除】，【项目工作日期】与当前日期不在同一周的记录");
                    return;
                }

                Business_Project_Timesheets timesheets = new Business_Project_Timesheets();

                timesheets.ID = Convert.ToInt32(customDataGridView1.CurrentRow.Cells["序号"].Value);
                if (MessageDialog.ShowEnquiryMessage("是否要【删除】您当前选中的记录？") == DialogResult.Yes)
                {
                    _ServiceTimesheets.OperationInfo(CE_OperatorMode.删除, timesheets);
                    MessageDialog.ShowPromptMessage("删除成功");
                    ClearInfo();
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowErrorMessage(ex.Message);
            }

            customDataGridView1.DataSource = _ServiceTimesheets.GetInfo(BasicInfo.LoginID);
        }

        private void btnSetPersonnel_Click(object sender, EventArgs e)
        {
            项目工时表填写人员设置 frm = new 项目工时表填写人员设置();
            frm.ShowDialog();
        }

        bool CheckDate()
        {
            DateTime startDate, endDate;

            GregorianCalendar gc = new GregorianCalendar();
            int weekOfYear = gc.GetWeekOfYear(ServerTime.Time.Date, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            int intOfWeek = Convert.ToInt32(ServerTime.Time.DayOfWeek);

            if (intOfWeek > 5)
            {
                startDate = ServerTime.Time.Date.AddDays(5 - intOfWeek);
                endDate = startDate.AddDays(7);
            }
            else
            {
                endDate = ServerTime.Time.Date.AddDays(5 - intOfWeek);
                startDate = endDate.AddDays(-7);
            }

            if (dtpItemDate.Value.Date <= endDate && dtpItemDate.Value.Date > startDate)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void txtExecUser_OnCompleteSearch()
        {
            txtExecUser.Tag = txtExecUser.DataResult["员工编号"].ToString();
        }

        private void txtExecUser_Enter(object sender, EventArgs e)
        {
            txtExecUser.StrEndSql = " and 员工编号 in (select WorkID from Business_Project_Timesheets_Personnel)";
        }
    }
}
