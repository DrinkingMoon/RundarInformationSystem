using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using UniversalControlLibrary;
using ServerModule;
using Service_Peripheral_HR;
using GlobalObject;

namespace Form_Peripheral_HR
{
    public partial class 培训计划信息反馈 : Form
    {
        ITrainBasicInfo _ServiceBasic = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<ITrainBasicInfo>();

        ITrainPlanCollect _ServiceCollect = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<ITrainPlanCollect>();

        ITrainFeedback _ServiceFeedback = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<ITrainFeedback>();

        public 培训计划信息反馈()
        {
            InitializeComponent();

            checkBillDateAndStatus1.dtpStartTime.Value = ServerTime.Time.Date.AddMonths(-1);
            checkBillDateAndStatus1.dtpEndTime.Value = ServerTime.Time.Date;

            dtpStartTime.Value = ServerTime.Time.Date;
            dtpEndTime.Value = ServerTime.Time.Date;
        }

        private void btnCourse_Click(object sender, EventArgs e)
        {
            if (cmbYearValue.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请选择【培训年份】");
                return;
            }

            FormDataTableCheck frm = new FormDataTableCheck(_ServiceCollect.GetCourseInfo(Convert.ToInt32(cmbYearValue.Text)));
            frm._BlDateTimeControlShow = false;
            frm._BlIsCheckBox = false;

            if (frm.ShowDialog() == DialogResult.OK)
            {
                int courseID = Convert.ToInt32(frm._DtResult.Rows[0]["课程ID"]);
                View_HR_Train_Course courseInfo = _ServiceBasic.GetSingleCourseInfo(courseID);

                if (courseInfo != null)
                {
                    chbIsOutSide.Checked = (bool)courseInfo.外训;
                    txtClassHour.Text = courseInfo.预计课时.ToString();
                    txtLecturer.Text = courseInfo.推荐讲师;
                    numFund.Value = (decimal)courseInfo.预计经费;
                    txtCourse.Text = courseInfo.课程名;
                    txtCourse.Tag = courseInfo.课程ID;
                    btnCourse.Tag = frm._DtResult.Rows[0]["ID"].ToString();
                }
            }
        }

        private void 培训计划信息反馈_Load(object sender, EventArgs e)
        {
            cmbYearValue.Init();
            RefreshDataGridView();
        }

        void RefreshDataGridView()
        {
            dgv_User.Rows.Clear();
            dgv_Course.Rows.Clear();
            List<View_HR_Train_Feedback> lstTemp = _ServiceFeedback.GetListInfo_Feedback(checkBillDateAndStatus1.dtpStartTime.Value.Date,
                checkBillDateAndStatus1.dtpEndTime.Value.Date);
            dgv_Course.DataSource = new BindingCollection<View_HR_Train_Feedback>(lstTemp);
        }

        private void checkBillDateAndStatus1_OnCompleteSearch()
        {
            RefreshDataGridView();
        }

        private void dgv_Course_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv_Course.CurrentRow == null)
            {
                return;
            }

            Guid guid = new Guid(dgv_Course.CurrentRow.Cells["ID"].Value.ToString());
            List<View_HR_Train_FeedbackUser> lstTemp = _ServiceFeedback.GetListInfo_FeedbackUser(guid);
            dgv_User.DataSource = new BindingCollection<View_HR_Train_FeedbackUser>(lstTemp);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtCourse.Text.Trim().Length == 0)
                {
                    MessageDialog.ShowPromptMessage("请选择【培训课程】");
                    return;
                }

                if (btnSetUser.Tag == null)
                {
                    MessageDialog.ShowPromptMessage("请选择【参加培训人员】");
                    return;
                }

                if (dtpEndTime.Value < dtpStartTime.Value)
                {
                    MessageDialog.ShowPromptMessage("【培训开始时间】须小于【培训结束时间】");
                    return;
                }

                HR_Train_Feedback feedback = new HR_Train_Feedback();

                feedback.CourseID = Convert.ToInt32(txtCourse.Tag);
                feedback.CreateTime = ServerTime.Time;
                feedback.CreateUser = BasicInfo.LoginID;
                feedback.EndTime = dtpEndTime.Value;
                feedback.Fund = numFund.Value;
                feedback.ID = Guid.NewGuid();
                feedback.Lecturer = txtLecturer.Text;
                feedback.StartTime = dtpStartTime.Value;

                _ServiceFeedback.InsertInfo(feedback, btnSetUser.Tag as List<string>);
                MessageDialog.ShowPromptMessage("提交成功");
                RefreshDataGridView();

                txtCourse.Text = "";
                txtCourse.Tag = null;
                btnCourse.Tag = null;
                txtClassHour.Text = "";
                txtLecturer.Text = "";
                numFund.Value = 0;
                btnSetUser.Tag = null;
                
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgv_Course.CurrentRow == null)
                {
                    return;
                }

                if (MessageDialog.ShowEnquiryMessage("您是否要【删除】课程名：【"
                    + dgv_Course.CurrentRow.Cells["课程名"].Value.ToString() +"】的记录？") == DialogResult.Yes)
                {
                    _ServiceFeedback.DeleteInfo(dgv_Course.CurrentRow.Cells["ID"].Value.ToString());
                    MessageDialog.ShowPromptMessage("删除成功");
                    RefreshDataGridView();
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void btnSetUser_Click(object sender, EventArgs e)
        {
            if (txtCourse.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请选择【课程】");
                return;
            }

            FormDataTableCheck frm = new FormDataTableCheck(_ServiceCollect.GetUserInfo(btnCourse.Tag.ToString()));
            frm._BlDateTimeControlShow = false;
            frm._BlIsCheckBox = true;

            if (frm.ShowDialog() == DialogResult.OK)
            {
                List<string> lstWork = DataSetHelper.ColumnsToList_Distinct(frm._DtResult, "工号");
                btnSetUser.Tag = lstWork;
            }
        }
    }
}
