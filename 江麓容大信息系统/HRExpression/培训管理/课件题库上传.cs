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
    public partial class 课件题库上传 : Form
    {
        HR_Train_Course _CourseInfo = new HR_Train_Course();
        ITrainBasicInfo _ServiceBasicInfo = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<ITrainBasicInfo>();

        public 课件题库上传(HR_Train_Course courseInfo)
        {
            InitializeComponent();
            _CourseInfo = courseInfo;
        }

        private void 课件题库上传_Load(object sender, EventArgs e)
        {
            txtCourseName.Text = _CourseInfo.CourseName;
            DataTable tempTable = _ServiceBasicInfo.GetPostInfo(_CourseInfo.ID);

            List<string> lstName = DataSetHelper.ColumnsToList(tempTable, "岗位名称");

            foreach (string item in lstName)
            {
                txtPost.Text += item + ",";
            }

            numExtraction.Value = _CourseInfo.ExamExtractionRate == null ? 0 : (decimal)_CourseInfo.ExamExtractionRate;
            numPass.Value = _CourseInfo.ExamPassRate == null ? 0 : (decimal)_CourseInfo.ExamPassRate;

            dgv_Courseware.DataSource = _ServiceBasicInfo.GetTable_Ware(_CourseInfo.ID);
            dgv_QuestionBank.DataSource = 
                new BindingCollection<View_HR_Train_QuestionBank>(_ServiceBasicInfo.GetListQuestionBank(_CourseInfo.ID));
        }

        private void btn_Submit_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    Guid guid = Guid.NewGuid();
                    FileOperationService.File_UpLoad(guid, openFileDialog1.FileName,
                            GlobalObject.GeneralFunction.StringConvertToEnum<CE_CommunicationMode>
                            (BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.文件传输方式]));

                    HR_Train_Courseware ware = new HR_Train_Courseware();

                    ware.CourseID = _CourseInfo.ID;
                    ware.CoursewareName = txtCoursewareName.Text;
                    ware.FileUnique = guid;
                    _ServiceBasicInfo.Operation_Courseware(ware);

                    MessageDialog.ShowPromptMessage("添加成功");
                    dgv_Courseware.DataSource = _ServiceBasicInfo.GetTable_Ware(_CourseInfo.ID);
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                if (btn_Delete.Tag == null)
                {
                    return;
                }

                UniversalControlLibrary.FileOperationService.File_Delete(new Guid(btn_Delete.Tag.ToString()),
                    GlobalObject.GeneralFunction.StringConvertToEnum<CE_CommunicationMode>
                    (BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.文件传输方式]));


                HR_Train_Courseware ware = new HR_Train_Courseware();

                ware.ID = Convert.ToInt32(txtCoursewareName.Tag);
                _ServiceBasicInfo.Operation_Courseware(ware);
                MessageDialog.ShowPromptMessage("删除成功");
                dgv_Courseware.DataSource = _ServiceBasicInfo.GetTable_Ware(_CourseInfo.ID);
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void dgv_Courseware_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv_Courseware.CurrentRow == null)
            {
                return;
            }

            txtCoursewareName.Text = dgv_Courseware.CurrentRow.Cells["文件名"].Value.ToString();
            txtCoursewareName.Tag = Convert.ToInt32(dgv_Courseware.CurrentRow.Cells["文件ID"].Value);
            btn_Delete.Tag = dgv_Courseware.CurrentRow.Cells["文件唯一编码"].Value.ToString();
        }

        private void dgv_Courseware_DoubleClick(object sender, EventArgs e)
        {
            if (dgv_Courseware.CurrentRow == null)
            {
                return;
            }

            FileOperationService.File_Look(new Guid(dgv_Courseware.CurrentRow.Cells["文件唯一编码"].Value.ToString()),
                    GlobalObject.GeneralFunction.StringConvertToEnum<CE_CommunicationMode>
                    (BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.文件传输方式]));
        }

        private void btn_QuestionBank_Input_Click(object sender, EventArgs e)
        {

            try
            {
                DataTable dtTemp = ExcelHelperP.RenderFromExcel(openFileDialog1);

                if (dtTemp == null)
                {
                    return;
                }

                if (!dtTemp.Columns.Contains("考题ID"))
                {
                    MessageDialog.ShowPromptMessage("文件中无【考题ID】列");
                    return;
                }

                if (!dtTemp.Columns.Contains("考题类型"))
                {
                    MessageDialog.ShowPromptMessage("文件中无【考题类型】列");
                    return;
                }

                if (!dtTemp.Columns.Contains("考题内容"))
                {
                    MessageDialog.ShowPromptMessage("文件中无【考题内容】列");
                    return;
                }

                if (!dtTemp.Columns.Contains("选项"))
                {
                    MessageDialog.ShowPromptMessage("文件中无【选项】列");
                    return;
                }

                if (!dtTemp.Columns.Contains("选项内容"))
                {
                    MessageDialog.ShowPromptMessage("文件中无【选项内容】列");
                    return;
                }

                if (!dtTemp.Columns.Contains("答案"))
                {
                    MessageDialog.ShowPromptMessage("文件中无【答案】列");
                    return;
                }

                _ServiceBasicInfo.InputQuestionsBank(_CourseInfo.ID, dtTemp);
                MessageDialog.ShowPromptMessage("导入成功");
                dgv_QuestionBank.DataSource =
                    new BindingCollection<View_HR_Train_QuestionBank>(_ServiceBasicInfo.GetListQuestionBank(_CourseInfo.ID));
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void btnDeleteQuestion_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgv_QuestionBank.CurrentRow == null)
                {
                    return;
                }

                foreach (DataGridViewRow dgvr in dgv_QuestionBank.SelectedRows)
                {
                    _ServiceBasicInfo.DeleteQuestion(dgvr.Cells["考题ID"].Value.ToString());
                }

                MessageDialog.ShowPromptMessage("删除成功");
                dgv_QuestionBank.DataSource =
                    new BindingCollection<View_HR_Train_QuestionBank>(_ServiceBasicInfo.GetListQuestionBank(_CourseInfo.ID));
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
                return;
            }
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            _CourseInfo.ExamExtractionRate = numExtraction.Value;
            _CourseInfo.ExamPassRate = numPass.Value;

            _ServiceBasicInfo.UpdateCourseExamInfo(_CourseInfo);
            MessageDialog.ShowPromptMessage("【提取率】、【通过率】变更成功");
        }
    }
}
