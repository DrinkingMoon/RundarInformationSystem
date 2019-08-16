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
using FlowControlService;
using Service_Peripheral_HR;

namespace Form_Peripheral_HR
{
    public partial class 培训计划申请表明细 : CustomFlowForm
    {
        IFlowServer _ServiceFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();
        ITrainSurvey _ServiceSurvey = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<ITrainSurvey>();
        ITrainBasicInfo _ServiceBasicInfo = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<ITrainBasicInfo>();

        HR_Train_Plan _LnqPlanInfo = new HR_Train_Plan();
        List<View_HR_Train_PlanCourse> _ListPlanCourse = new List<View_HR_Train_PlanCourse>();
        List<View_HR_Train_PlanUser> _ListPlanUser = new List<View_HR_Train_PlanUser>();
        string _CourseGuid = null;

        public 培训计划申请表明细()
        {
            InitializeComponent();
        }

        public override void LoadFormInfo()
        {
            try
            {
                _LnqPlanInfo = _ServiceSurvey.GetSingleInfo(this.FlowInfo_BillNo);
                lbBillStatus.Text = _ServiceFlow.GetNowBillStatus(this.FlowInfo_BillNo);
                _ListPlanCourse = _ServiceSurvey.GetPlanCourseInfo(this.FlowInfo_BillNo);
                _ListPlanUser = _ServiceSurvey.GetPlanUserInfo(this.FlowInfo_BillNo);
                txtBillNo.Text = this.FlowInfo_BillNo;
                cmbYear.Init();
                cmbPlanType.Init<CE_HR_Train_PlanType>();
                cmbMonth.Init<CE_MonthValue>();

                if (_LnqPlanInfo != null)
                {
                    cmbYear.Text = _LnqPlanInfo.YearValue == null ? 
                        ServerTime.Time.Year.ToString() : ((int)_LnqPlanInfo.YearValue).ToString();
                    cmbPlanType.Text = _LnqPlanInfo.PlanType;
                }

                RefreshDataGrid();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        void RefreshDataGrid()
        {
            if (_ListPlanCourse != null)
            {
                dgv_Course.DataSource = new BindingCollection<View_HR_Train_PlanCourse>(_ListPlanCourse);
                userControlDataLocalizer1.Init(dgv_Course, dgv_Course.Name, null);
            }
        }

        private void dgv_Course_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv_Course.CurrentRow == null)
            {
                return;
            }

            txtAssess.Text = dgv_Course.CurrentRow.Cells["评估方式"].Value.ToString();
            txtCourse.Text = dgv_Course.CurrentRow.Cells["课程名"].Value.ToString();
            txtCourse.Tag = Convert.ToInt32(dgv_Course.CurrentRow.Cells["课程ID"].Value);
            txtLecturer.Text = dgv_Course.CurrentRow.Cells["推荐讲师"].Value.ToString();
            cmbMonth.Text = GlobalObject.GeneralFunction.ValueConvertToEnum<CE_MonthValue>(Convert.ToInt32(dgv_Course.CurrentRow.Cells["月份"].Value)).ToString();
            txtType.Text = dgv_Course.CurrentRow.Cells["课程类型"].Value.ToString();
            numClassHour.Value = Convert.ToDecimal(dgv_Course.CurrentRow.Cells["预计课时"].Value);
            numFund.Value = Convert.ToDecimal(dgv_Course.CurrentRow.Cells["预计经费"].Value);
            chbIsOutSide.Checked = Convert.ToBoolean(dgv_Course.CurrentRow.Cells["外训"].Value);

            DataTable tempTable = _ServiceBasicInfo.GetPostInfo(Convert.ToInt32(txtCourse.Tag));

            List<string> lstName = DataSetHelper.ColumnsToList(tempTable, "岗位名称");
            txtPost.Text = "";
            foreach (string item in lstName)
            {
                txtPost.Text += item + ",";
            }

            int courseID = Convert.ToInt32(txtCourse.Tag);
            Guid guid = new Guid(dgv_Course.CurrentRow.Cells["ID"].Value.ToString());

            List<View_HR_Train_PlanUser> lstCheck = (from a in _ListPlanUser
                                                     where a.PlanCourseID == guid
                                                     select a).ToList();

            List<View_HR_Train_PlanUser> lstSource = _ServiceSurvey.GetPlanUserInfoAll(guid, courseID);

            dgv_User.DataSource = new BindingCollection<View_HR_Train_PlanUser>(lstSource);

            foreach (DataGridViewRow dgvr in dgv_User.Rows)
            {
                dgvr.Cells["选"].Value = false;

                foreach (View_HR_Train_PlanUser user in lstCheck)
                {
                    if (dgvr.Cells["员工编号"].Value.ToString() == user.员工编号)
                    {
                        dgvr.Cells["选"].Value = true;
                    }
                }
            }

            _CourseGuid = dgv_Course.CurrentRow.Cells["ID"].Value.ToString();
        }

        private void dgv_User_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv_User.CurrentRow == null)
            {
                return;
            }

            if (dgv_User.Columns[e.ColumnIndex].Name == "选")
            {
                dgv_User.CurrentRow.Cells["选"].Value = !Convert.ToBoolean(dgv_User.CurrentRow.Cells["选"].Value);
            }
        }

        private void txtCourse_OnCompleteSearch()
        {
            txtCourse.Text = txtCourse.DataResult["课程名"].ToString();
            txtCourse.Tag = Convert.ToInt32(txtCourse.DataResult["课程ID"]);

            View_HR_Train_Course tempInfo = _ServiceBasicInfo.GetSingleCourseInfo(Convert.ToInt32(txtCourse.Tag));

            txtLecturer.Text = tempInfo.推荐讲师;
            txtType.Text = tempInfo.课程类型;
            txtAssess.Text = tempInfo.评估方式;
            numClassHour.Value = (decimal)tempInfo.预计经费;
            numFund.Value = (decimal)tempInfo.预计课时;
            chbIsOutSide.Checked = (bool)tempInfo.外训;

            DataTable tempTable = _ServiceBasicInfo.GetPostInfo(Convert.ToInt32(txtCourse.Tag));

            List<string> lstName = DataSetHelper.ColumnsToList(tempTable, "岗位名称");

            txtPost.Text = "";
            foreach (string item in lstName)
            {
                txtPost.Text += item + ",";
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (cmbMonth.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请选择【计划月份】");
                return;
            }

            if (txtCourse.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请选择【培训课程】");
                return;
            }

            View_HR_Train_PlanCourse tempInfo = new View_HR_Train_PlanCourse();

            tempInfo.ID = Guid.NewGuid();
            tempInfo.单据号 = this.FlowInfo_BillNo;
            tempInfo.课程ID = Convert.ToInt32(txtCourse.Tag);
            tempInfo.课程类型 = txtType.Text;
            tempInfo.课程名 = txtCourse.Text;
            tempInfo.评估方式 = txtAssess.Text;
            tempInfo.所属部门 = BasicInfo.DeptCode;
            tempInfo.推荐讲师 = txtLecturer.Text;
            tempInfo.外训 = chbIsOutSide.Checked;
            tempInfo.预计经费 = numClassHour.Value;
            tempInfo.预计课时 = numFund.Value;
            tempInfo.月份 = Convert.ToInt32(cmbMonth.SelectedValue);

            _ListPlanCourse.Add(tempInfo);
            RefreshDataGrid();
        }

        private void txtCourse_Enter(object sender, EventArgs e)
        {
            txtCourse.StrEndSql += " and dbo.fun_get_BelongDept_Value(所属部门) = dbo.fun_get_BelongDept_Value('" + BasicInfo.DeptCode + "')";
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgv_Course.CurrentRow == null)
            {
                return;
            }

            Guid guid = new Guid(dgv_Course.CurrentRow.Cells["ID"].Value.ToString());

            if (_ListPlanCourse != null)
            {
                _ListPlanCourse.RemoveAll(k => k.ID == guid);
            }

            if (_ListPlanUser != null)
            {
                _ListPlanUser.RemoveAll(k => k.PlanCourseID == guid);
            }

            RefreshDataGrid();
        }

        private void dgv_User_Leave(object sender, EventArgs e)
        {
            if (_CourseGuid != null)
            {
                if (_ListPlanUser != null)
                {
                    Guid guidCourse = new Guid(_CourseGuid);
                    _ListPlanUser.RemoveAll(k => k.PlanCourseID == guidCourse);

                    foreach (DataGridViewRow dgvr in dgv_User.Rows)
                    {
                        if (Convert.ToBoolean(dgvr.Cells["选"].Value))
                        {
                            View_HR_Train_PlanUser planUser = new View_HR_Train_PlanUser();

                            planUser.PlanCourseID = guidCourse;
                            planUser.岗位 = dgvr.Cells["岗位"].Value.ToString();
                            planUser.员工编号 = dgvr.Cells["员工编号"].Value.ToString();
                            planUser.员工姓名 = dgvr.Cells["员工姓名"].Value.ToString();

                            _ListPlanUser.Add(planUser);
                        }
                    }
                }
            }
        }

        private bool 培训计划申请表明细_PanelGetDataInfo(CE_FlowOperationType flowOperationType)
        {
            try
            {
                if (cmbYear.Text.Trim().Length == 0)
                {
                    MessageDialog.ShowPromptMessage("请选择【计划年份】");
                    return false;
                }

                if (cmbPlanType.Text.Trim().Length == 0)
                {
                    MessageDialog.ShowPromptMessage("请选择【计划类型】");
                    return false;
                }

                _LnqPlanInfo = new HR_Train_Plan();

                _LnqPlanInfo.BillNo = txtBillNo.Text;
                _LnqPlanInfo.YearValue = Convert.ToInt32(cmbYear.Text);
                _LnqPlanInfo.Department = BasicInfo.DeptCode;
                _LnqPlanInfo.PlanType = cmbPlanType.Text;

                this.ResultInfo = _LnqPlanInfo;
                this.FlowOperationType = flowOperationType;

                if (_ListPlanCourse.Count() == 0)
                {
                    MessageDialog.ShowPromptMessage("未添加任何【课程计划】");
                    return false;
                }

                if (_ListPlanUser.Count() == 0)
                {
                    MessageDialog.ShowPromptMessage("未选择任何【员工】参与培训");
                    return false;
                }

                this.ResultList.Add(_ListPlanCourse);
                this.ResultList.Add(_ListPlanUser);

                this.KeyWords = "【"+ UniversalFunction.GetDept_Belonge(BasicInfo.DeptCode).DeptName +"】的【"+ cmbYear.Text +"年"+ cmbMonth.Text +"】培训计划";
                return true;
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
                return false;
            }

        }
    }
}
