using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Service_Peripheral_HR;
using PlatformManagement;
using Expression;
using ServerModule;
using GlobalObject;
using UniversalControlLibrary;

namespace Form_Peripheral_HR
{
    public partial class 课程基础设置 : Form
    {
        ITrainBasicInfo _ServiceBasicInfo = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<ITrainBasicInfo>();

        public 课程基础设置()
        {
            InitializeComponent();
        }

        private void btn_CourseType_Add_Click(object sender, EventArgs e)
        {
            try
            {
                HR_Train_CourseType temp = new HR_Train_CourseType();

                temp.CourseTypeName = txtCourseType.Text;
                _ServiceBasicInfo.Operation_CourseType(temp);
                MessageDialog.ShowPromptMessage("添加成功"); 
                RefreshGrid_CourseType();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void btn_CourseType_Modify_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtCourseType.Tag == null)
                {
                    MessageDialog.ShowPromptMessage("请选择需要修改的记录");
                    return;
                }

                HR_Train_CourseType temp = new HR_Train_CourseType();
                temp.ID = Convert.ToInt32(txtCourseType.Tag);
                temp.CourseTypeName = txtCourseType.Text;
                _ServiceBasicInfo.Operation_CourseType(temp);
                MessageDialog.ShowPromptMessage("修改成功");
                RefreshGrid_CourseType();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void btn_CourseType_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtCourseType.Tag == null)
                {
                    MessageDialog.ShowPromptMessage("请选择需要删除的记录");
                    return;
                }

                HR_Train_CourseType temp = new HR_Train_CourseType();
                temp.ID = Convert.ToInt32(txtCourseType.Tag);

                _ServiceBasicInfo.Operation_CourseType(temp);
                MessageDialog.ShowPromptMessage("删除成功");
                RefreshGrid_CourseType();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void btn_Assess_Add_Click(object sender, EventArgs e)
        {
            try
            {
                HR_Train_AssessType temp = new HR_Train_AssessType();

                temp.AssessTypeName = txtAssessTypeName.Text;
                temp.IsExam = chbIsExam.Checked;
                _ServiceBasicInfo.Operation_AssessType(temp);
                MessageDialog.ShowPromptMessage("添加成功");
                RefreshGrid_AssessType();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void btn_Assess_Modify_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtAssessTypeName.Tag == null)
                {
                    MessageDialog.ShowPromptMessage("请选择需要修改的记录");
                    return;
                }

                HR_Train_AssessType temp = new HR_Train_AssessType();
                temp.ID = Convert.ToInt32(txtAssessTypeName.Tag);
                temp.AssessTypeName = txtAssessTypeName.Text;
                temp.IsExam = chbIsExam.Checked;
                _ServiceBasicInfo.Operation_AssessType(temp);
                MessageDialog.ShowPromptMessage("修改成功");
                RefreshGrid_AssessType();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void btn_Assess_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtAssessTypeName.Tag == null)
                {
                    MessageDialog.ShowPromptMessage("请选择需要删除的记录");
                    return;
                }

                HR_Train_AssessType temp = new HR_Train_AssessType();
                temp.ID = Convert.ToInt32(txtAssessTypeName.Tag);

                _ServiceBasicInfo.Operation_AssessType(temp);
                MessageDialog.ShowPromptMessage("删除成功");
                RefreshGrid_AssessType();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void dgv_CourseType_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv_CourseType.CurrentRow == null)
            {
                return;
            }

            txtCourseType.Text = dgv_CourseType.CurrentRow.Cells["类型名称"].Value.ToString();
            txtCourseType.Tag = dgv_CourseType.CurrentRow.Cells["类型ID"].Value.ToString();
        }

        private void dgv_AssessType_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv_AssessType.CurrentRow == null)
            {
                return;
            }

            txtAssessTypeName.Text = dgv_AssessType.CurrentRow.Cells["方式名称"].Value.ToString();
            txtAssessTypeName.Tag = dgv_AssessType.CurrentRow.Cells["方式ID"].Value.ToString();
        }

        void PositioningRecord(string info)
        {
            for (int i = 0; i < dgv_Course.Rows.Count; i++)
            {
                if ((string)dgv_Course.Rows[i].Cells["课程名"].Value == info)
                {
                    dgv_Course.CurrentCell = dgv_Course.Rows[i].Cells[1];
                    dgv_Course.FirstDisplayedScrollingRowIndex = i;
                }
            }
        }

        void RefreshGrid_Course()
        {
            if (!BasicInfo.ListRoles.Contains(CE_RoleEnum.人力资源部办公室文员.ToString()))
            {
                dgv_Course.DataSource = _ServiceBasicInfo.GetCourseInfo(null);
            }
            else
            {
                dgv_Course.DataSource = _ServiceBasicInfo.GetCourseInfo_Comm();
            }

            userControlDataLocalizer1.Init(dgv_Course, dgv_Course.Name, null);
        }

        void RefreshGrid_CourseType()
        {
            dgv_CourseType.DataSource = _ServiceBasicInfo.GetTable<HR_Train_CourseType>();
            cmbType.DataSource = DataSetHelper.ColumnsToList(_ServiceBasicInfo.GetTable<HR_Train_CourseType>(), "类型名称");
        }

        void RefreshGrid_AssessType()
        {
            dgv_AssessType.DataSource = _ServiceBasicInfo.GetTable<HR_Train_AssessType>();
            cmbAssess.DataSource = DataSetHelper.ColumnsToList(_ServiceBasicInfo.GetTable<HR_Train_AssessType>(), "方式名称");
        }

        void RefreshGrid_CommCourse()
        {
            dgv_CommCourse.DataSource = _ServiceBasicInfo.GetCourseInfo_Comm();
        }

        HR_Train_Course GetCourseInfo()
        {
            HR_Train_Course temp = new HR_Train_Course();

            HR_Train_AssessType assess = new HR_Train_AssessType();
            assess.AssessTypeName = cmbAssess.Text;
            assess = _ServiceBasicInfo.GetSingleInfo_AssessType(assess);

            HR_Train_CourseType courseType = new HR_Train_CourseType();
            courseType.CourseTypeName = cmbType.Text;
            courseType = _ServiceBasicInfo.GetSingleInfo_CourseType(courseType);

            temp.AssessID = assess.ID;
            temp.ClassHour = numClassHour.Value;
            temp.CourseName = txtCourseName.Text;
            temp.Fund = numFund.Value;

            if (txtCourseName.Tag != null)
            {
                temp.ID = Convert.ToInt32(txtCourseName.Tag);
            }

            temp.IsOutSide = chbIsOutSide.Checked;
            temp.Lecturer = txtLecturer.Text;
            temp.TypeID = courseType.ID;
            temp.Department = BasicInfo.DeptCode;

            return temp;
        }

        private void btn_Course_Add_Click(object sender, EventArgs e)
        {
            try
            {
                HR_Train_Course temp = GetCourseInfo();
                temp.ID = 0;
                _ServiceBasicInfo.Operation_Course(temp);
                MessageDialog.ShowPromptMessage("添加成功");
                RefreshGrid_Course();
                PositioningRecord(txtCourseName.Text);
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void btn_Course_Modify_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtCourseName.Tag == null)
                {
                    MessageDialog.ShowPromptMessage("请选择需要修改的记录");
                    return;
                }

                HR_Train_Course temp = GetCourseInfo();
                _ServiceBasicInfo.Operation_Course(temp);
                MessageDialog.ShowPromptMessage("修改成功");
                RefreshGrid_Course();
                PositioningRecord(txtCourseName.Text);
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void btn_Course_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtCourseName.Tag == null)
                {
                    MessageDialog.ShowPromptMessage("请选择需要删除的记录");
                    return;
                }

                HR_Train_Course temp = new HR_Train_Course();
                temp.ID = Convert.ToInt32(txtCourseName.Tag);

                _ServiceBasicInfo.Operation_Course(temp);
                MessageDialog.ShowPromptMessage("删除成功");
                RefreshGrid_Course();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void btn_Course_WareQuestion_Click(object sender, EventArgs e)
        {
            if (txtCourseName.Tag == null)
            {
                MessageDialog.ShowPromptMessage("请先添加课程，再上传题库与课件");
                return;
            }

            HR_Train_Course temp = new HR_Train_Course();
            temp.ID = Convert.ToInt32(txtCourseName.Tag);

            课件题库上传 frm = new 课件题库上传(_ServiceBasicInfo.GetSingleInfo_Course(temp));
            frm.ShowDialog();
        }

        void RadioButtonChange()
        {
            if (rb_PostRel_CourseToPost.Checked)
            {
                dgv_PostRel_Course.DataSource = _ServiceBasicInfo.GetCourseInfo(null);
                dgv_PostRel_Course_CellEnter(null, null);
            }
            else if (rb_PostRel_PostToCourse.Checked)
            {
                dgv_PostRel_Post.DataSource = _ServiceBasicInfo.GetPostInfo(null);
                userControlDataLocalizer3.Init(this.dgv_PostRel_Post, this.Name, null);
                dgv_PostRel_Post_CellEnter(null, null);
            }
        }

        private void dgv_PostRel_Course_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (rb_PostRel_CourseToPost.Checked)
            {
                if (dgv_PostRel_Course.Rows.Count == 0)
                {
                    return;
                }

                int courseID = Convert.ToInt32(dgv_PostRel_Course.Rows[0].Cells["课程ID4"].Value);

                if (dgv_PostRel_Course.CurrentRow != null)
                {
                    courseID = Convert.ToInt32(dgv_PostRel_Course.CurrentRow.Cells["课程ID4"].Value);
                }

                dgv_PostRel_Post.DataSource = _ServiceBasicInfo.GetPostInfo(courseID);
                userControlDataLocalizer3.Init(this.dgv_PostRel_Post, this.Name, null);
            }
        }

        private void dgv_PostRel_Post_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (rb_PostRel_PostToCourse.Checked)
            {
                if (dgv_PostRel_Post.Rows.Count == 0)
                {
                    return;
                }

                int postID = Convert.ToInt32(dgv_PostRel_Post.Rows[0].Cells["岗位编号"].Value);

                if (dgv_PostRel_Post.CurrentRow != null)
                {
                    postID = Convert.ToInt32(dgv_PostRel_Post.CurrentRow.Cells["岗位编号"].Value);
                }

                dgv_PostRel_Course.DataSource = _ServiceBasicInfo.GetCourseInfo(postID);
            }
        }

        private void btn_PostRel_Set_Click(object sender, EventArgs e)
        {
            DataTable sourceTable = new DataTable();
            DataTable checkTable = new DataTable();
            List<string> lstkeys = new List<string>();

            if (rb_PostRel_CourseToPost.Checked)
            {
                sourceTable = _ServiceBasicInfo.GetPostInfo(null);
                checkTable = (DataTable)dgv_PostRel_Post.DataSource;
                lstkeys.Add("岗位编号");

                FormDataTableCheck frm = new FormDataTableCheck(sourceTable, checkTable, lstkeys);
                frm._BlDateTimeControlShow = false;
                frm._BlIsCheckBox = true;

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    _ServiceBasicInfo.Operation_PostRelation_CourseToPost(frm._DtResult,
                        Convert.ToInt32(dgv_PostRel_Course.CurrentRow.Cells["课程ID4"].Value));
                    dgv_PostRel_Course_CellEnter(null, null);
                }
            }
            else if(rb_PostRel_PostToCourse.Checked)
            {
                sourceTable = _ServiceBasicInfo.GetCourseInfo(null);
                checkTable = (DataTable)dgv_PostRel_Course.DataSource;
                lstkeys.Add("课程ID");

                FormDataTableCheck frm = new FormDataTableCheck(sourceTable, checkTable, lstkeys);
                frm._BlDateTimeControlShow = false;
                frm._BlIsCheckBox = true;

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    _ServiceBasicInfo.Operation_PostRelation_PostToCourse(frm._DtResult,
                        Convert.ToInt32(dgv_PostRel_Post.CurrentRow.Cells["岗位编号"].Value));
                    dgv_PostRel_Post_CellEnter(null, null);
                }
            }
        }

        private void 课程基础设置_Load(object sender, EventArgs e)
        {
            RefreshGrid_CourseType();
            RefreshGrid_AssessType();
            RefreshGrid_Course();

            dgv_CommCourse.DataSource = _ServiceBasicInfo.GetCourseInfo_Comm();

            if (!BasicInfo.ListRoles.Contains(CE_RoleEnum.人力资源部办公室文员.ToString()))
            {
                tabControl1.TabPages.Remove(this.课程类型评估方式);
                tabControl1.TabPages.Remove(this.公司级课程);
            }
            else
            {
                tabControl1.TabPages.Remove(this.配置岗位课程);
            }
        }

        private void dgv_Course_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv_Course.CurrentRow == null)
            {
                return;
            }

            txtCourseName.Text = dgv_Course.CurrentRow.Cells["课程名"].Value.ToString();
            txtCourseName.Tag = Convert.ToInt32(dgv_Course.CurrentRow.Cells["课程ID"].Value);
            txtLecturer.Text = dgv_Course.CurrentRow.Cells["推荐讲师"].Value.ToString();
            chbIsOutSide.Checked = Convert.ToBoolean(dgv_Course.CurrentRow.Cells["外训"].Value);
            cmbAssess.Text = dgv_Course.CurrentRow.Cells["评估方式"].Value.ToString();
            cmbType.Text = dgv_Course.CurrentRow.Cells["课程类型"].Value.ToString();
            numClassHour.Value = Convert.ToDecimal(dgv_Course.CurrentRow.Cells["预计课时"].Value);
            numFund.Value = Convert.ToDecimal(dgv_Course.CurrentRow.Cells["预计经费"].Value);
        }

        private void rb_PostRel_PostToCourse_CheckedChanged(object sender, EventArgs e)
        {
            RadioButtonChange();
        }

        private void rb_PostRel_CourseToPost_CheckedChanged(object sender, EventArgs e)
        {
            RadioButtonChange();
        }

        private void dgv_CommCourse_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv_CommCourse.CurrentRow == null)
            {
                return;
            }

            dgv_DeptCourse.DataSource = 
                _ServiceBasicInfo.GetCourseInfo_Comm_Dept(Convert.ToInt32( dgv_CommCourse.CurrentRow.Cells["课程ID1"].Value));
            userControlDataLocalizer2.Init(this.dgv_DeptCourse, this.Name, null);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgv_CommCourse.CurrentRow == null)
                {
                    return;
                }

                List<int> lstDeptCourseID = new List<int>();

                foreach (DataGridViewRow dgvr in dgv_DeptCourse.Rows)
                {
                    if (Convert.ToBoolean(dgvr.Cells["选"].Value))
                    {
                        lstDeptCourseID.Add(Convert.ToInt32(dgvr.Cells["课程ID2"].Value));
                    }
                }

                _ServiceBasicInfo.Operation_Comm_Rel(Convert.ToInt32(dgv_CommCourse.CurrentRow.Cells["课程ID1"].Value), lstDeptCourseID);
                MessageDialog.ShowPromptMessage("保存成功");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == this.课程类型评估方式)
            {
                RefreshGrid_CourseType();
                RefreshGrid_AssessType();
            }
            else if (tabControl1.SelectedTab == this.课程)
            {
                RefreshGrid_Course();
            }
            else if (tabControl1.SelectedTab == this.配置岗位课程)
            {
                dgv_PostRel_Post.DataSource = _ServiceBasicInfo.GetPostInfo(null);
                userControlDataLocalizer3.Init(this.dgv_PostRel_Post, this.Name, null);
                dgv_PostRel_Post_CellEnter(null, null);
            }
            else if (tabControl1.SelectedTab == this.公司级课程)
            {
                dgv_CommCourse.DataSource = _ServiceBasicInfo.GetCourseInfo_Comm();
            }
            else
            {
                return;
            }

        }

        private void dgv_DeptCourse_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv_DeptCourse.CurrentRow == null)
            {
                return;
            }

            if (dgv_DeptCourse.Columns[e.ColumnIndex].Name == "选")
            {
                dgv_DeptCourse.CurrentRow.Cells["选"].Value = !Convert.ToBoolean(dgv_DeptCourse.CurrentRow.Cells["选"].Value);
            }
        }
    }
}
