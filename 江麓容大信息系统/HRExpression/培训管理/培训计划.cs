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
using Service_Peripheral_HR;

namespace Form_Peripheral_HR
{
    public partial class 培训计划 : Form
    {
        ITrainPlanCollect _ServiceCollect = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<ITrainPlanCollect>();
        string _CourseGuid;

        ITrainSurvey _ServiceSurvey = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<ITrainSurvey>();

        public 培训计划()
        {
            InitializeComponent();
        }

        private void 培训计划_Load(object sender, EventArgs e)
        {
            for (int i = 2010; i < 2050; i++)
            {
                cmbYear.Items.Add(i.ToString());
            }

            List<string> lstTemp = GlobalObject.GeneralFunction.GetEumnList(typeof(CE_HR_Train_PlanType));
            cmbPlanType.DataSource = lstTemp;
        }

        private void cmbPlanType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPlanType.Text == CE_HR_Train_PlanType.年度培训计划.ToString())
            {
                label2.Visible = false;
                cmbPlanBillNo.Visible = false;
                btnCollect.Visible = true;
            }
            else if (cmbPlanType.Text == CE_HR_Train_PlanType.临时培训计划.ToString())
            {
                btnCollect.Visible = false;
                label2.Visible = true;
                cmbPlanBillNo.Visible = true;

                if (cmbYear.Text.Trim().Length != 0)
                {
                    cmbPlanBillNo.DataSource = _ServiceSurvey.GetBillNoList_Temp(Convert.ToInt32(cmbYear.Text));
                }
            }
        }

        private void dgv_Course_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv_Course.CurrentRow == null)
            {
                return;
            }

            if (_CourseGuid == dgv_Course.CurrentRow.Cells["ID"].Value.ToString())
            {
                return;
            }

            Guid guid = new Guid(dgv_Course.CurrentRow.Cells["ID"].Value.ToString());
            int courseID = Convert.ToInt32(dgv_Course.CurrentRow.Cells["课程ID"].Value);

            List<View_HR_Train_PlanCollectUser> lstSource = _ServiceCollect.GetUserInfoAll(guid, courseID);

            List<Guid> lstTemp = new List<Guid>();
            lstTemp.Add(guid);

            List<View_HR_Train_PlanCollectUser> lstCheck = _ServiceCollect.GetUserInfo(lstTemp);

            dgv_User.DataSource = new BindingCollection<View_HR_Train_PlanCollectUser>(lstSource);
            userControl_User.Init(this.dgv_User, this.dgv_User.Name, null);

            foreach (DataGridViewRow dgvr in dgv_User.Rows)
            {
                dgvr.Cells["选"].Value = false;

                foreach (View_HR_Train_PlanCollectUser user in lstCheck)
                {
                    if (dgvr.Cells["工号"].Value.ToString() == user.工号)
                    {
                        dgvr.Cells["选"].Value = true;
                    }
                }
            }

            _CourseGuid = dgv_Course.CurrentRow.Cells["ID"].Value.ToString();
        }

        void RefreshViewCourse(CE_HR_Train_PlanType planType, int yearValue, string planBillNo)
        {
            List<View_HR_Train_PlanCollect> lstCollect = _ServiceCollect.GetCourseInfo(planType, yearValue, planBillNo);

            dgv_Course.DataSource = new BindingCollection<View_HR_Train_PlanCollect>(lstCollect);
            userControl_Course.Init(this.dgv_Course, this.dgv_Course.Name, null);
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (cmbPlanType.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请选择【计划类型】");
                return;
            }

            if (cmbYear.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请选择【计划年份】");
                return;
            }

            if (cmbPlanType.Text == CE_HR_Train_PlanType.临时培训计划.ToString()
                && cmbPlanBillNo.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("【临时培训计划】请选择【计划单号】");
                return;
            }

            RefreshViewCourse(GlobalObject.GeneralFunction.StringConvertToEnum<CE_HR_Train_PlanType>(cmbPlanType.Text), 
                Convert.ToInt32(cmbYear.Text), cmbPlanBillNo.Text);
        }

        private void cmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPlanBillNo.Text == CE_HR_Train_PlanType.临时培训计划.ToString() 
                && cmbYear.Text.Trim().Length != 0)
            {
                cmbPlanBillNo.DataSource = _ServiceSurvey.GetBillNoList_Temp(Convert.ToInt32(cmbYear.Text));
            }
        }

        private void btnCollect_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbYear.Text.Trim().Length == 0)
                {
                    throw new Exception("请选择【计划年份】");
                }

                FormDataTableCheck frm = new FormDataTableCheck(_ServiceSurvey.GetBillInfo_Year(Convert.ToInt32(cmbYear.Text)));

                frm._BlDateTimeControlShow = false;
                frm._BlIsCheckBox = true;

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    if (frm._DtResult != null && frm._DtResult.Rows.Count > 0)
                    {
                        List<string> lstBillNo = DataSetHelper.ColumnsToList_Distinct(frm._DtResult, "单据号");
                        _ServiceCollect.GenerateCollectPlan_Year(lstBillNo, Convert.ToInt32(cmbYear.Text));
                        RefreshViewCourse(CE_HR_Train_PlanType.年度培训计划, Convert.ToInt32(cmbYear.Text), null);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                List<View_HR_Train_PlanCollect> lstCollect = new List<View_HR_Train_PlanCollect>();
                foreach (DataGridViewRow dgvr in dgv_Course.Rows)
                {
                    View_HR_Train_PlanCollect temp = new View_HR_Train_PlanCollect();

                    temp.ID = new Guid(dgvr.Cells["ID"].Value.ToString());
                    temp.月份 = Convert.ToInt32(dgvr.Cells["月份"].Value);
                    lstCollect.Add(temp);
                }

                _ServiceCollect.SaveCollect(lstCollect);
                MessageDialog.ShowPromptMessage("保存成功");
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
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

        private void dgv_User_Leave(object sender, EventArgs e)
        {
            if (_CourseGuid != null)
            {
                List<string> lstTemp = new List<string>();

                foreach (DataGridViewRow dgvr in dgv_User.Rows)
                {
                    if (Convert.ToBoolean(dgvr.Cells["选"].Value))
                    {
                        lstTemp.Add(dgvr.Cells["工号"].Value.ToString());
                    }
                }

                _ServiceCollect.SaveUser(lstTemp, new Guid(_CourseGuid));
            }
        }
    }
}
