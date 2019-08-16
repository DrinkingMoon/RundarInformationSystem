using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using System.IO;
using UniversalControlLibrary;
using ServerModule;
using GlobalObject;
using Service_Peripheral_HR;

namespace Form_Peripheral_HR
{
    public partial class 储备人才库明细 : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_error;

        /// <summary>
        /// 序号
        /// </summary>
        int m_id;

        /// <summary>
        /// 文件流
        /// </summary>
        byte[] picbyte;

        /// <summary>
        /// 文件名
        /// </summary>
        string pathName;

        /// <summary>
        /// 选中的行
        /// </summary>
        int m_dataGridViewSelectRow;

        /// <summary>
        /// 获得系统当前时间
        /// </summary>
        DateTime m_date = ServerTime.Time;

        /// <summary>
        /// 储备人才管理类
        /// </summary>
        ITrainEmployeServer m_trainEmployeServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<ITrainEmployeServer>();

        public 储备人才库明细(int id)
        {
            InitializeComponent();

            m_id = id;

            if (m_id != 0)
            {
                BindControl();
            }
        }

        /// <summary>
        /// 绑定控件
        /// </summary>
        private void BindControl()
        {
            HR_TrainEmploye trainEmploye = m_trainEmployeServer.GetInfoByID(m_id,out m_error);

            txtAddress.Text = trainEmploye.Address;
            numAge.Value = Convert.ToDecimal(trainEmploye.Age);
            dtpBirthday.Value = Convert.ToDateTime(trainEmploye.Birthday);
            txtBirthplace.Text = trainEmploye.Birthplace;
            txtCollege.Text = trainEmploye.College;
            txtComputerLevel.Text = trainEmploye.ComputerLevel;
            numDesiredSalary.Value = Convert.ToDecimal(trainEmploye.DesiredSalary);
            txtEducatedDegree.Text = trainEmploye.EducatedDegree;
            txtEducatedMajor.Text = trainEmploye.EducatedMajor;
            txtEmergencyPhone.Text = trainEmploye.EmergencyPhone;
            txtEnglishLevel.Text = trainEmploye.EnglishLevel;
            txtEvaluate.Text = trainEmploye.Evaluate;
            cmbResumeStatus.Text = trainEmploye.ResumeStatus;
            numHeight.Value = Convert.ToDecimal(trainEmploye.Height);
            txtCard.Text = trainEmploye.ID_Card;
            txtPhone.Text = trainEmploye.Phone;

            if (trainEmploye.InterviewDate!=null)
            {
                dtpInterviewDate.Value = Convert.ToDateTime(trainEmploye.InterviewDate);
            }

            cbIsThirdParty.Checked = Convert.ToBoolean(trainEmploye.IsThirdParty);
            numJobYears.Value = Convert.ToDecimal(trainEmploye.JobYears);
            cmbMaritalStatus.Text = trainEmploye.MaritalStatus;
            txtName.Text = trainEmploye.Name;
            txtNationality.Text = trainEmploye.Nationality;
            txtParty.Text = trainEmploye.Party;
            cmbPersonType.Text = trainEmploye.PersonType;
            cmbSex.Text = trainEmploye.Sex;
            txtRace.Text = trainEmploye.Race;
            cmbRecruitType.Text = trainEmploye.RecruitType;
            txtSpeciality.Text = trainEmploye.Speciality;
            dtpTakeJobDate.Value = Convert.ToDateTime(trainEmploye.TakeJobDate);
            txtThirdParty.Text = trainEmploye.ThirdParty;

            if (trainEmploye.Anne != null)
            {
                picbyte = trainEmploye.Anne == null ? null : trainEmploye.Anne.ToArray();
                lblAnnxeName.Text = trainEmploye.FileName;
            }

            txtRemark.Text = trainEmploye.Remark;

            DataTable dtWorkHistory = m_trainEmployeServer.GetWorkHistory(m_id);

            if (dtWorkHistory != null && dtWorkHistory.Rows.Count > 0)
            {
                dgvWorkHistory.DataSource = dtWorkHistory;

                if (dgvWorkHistory.Rows.Count > 0)
                {
                    dgvWorkHistory.Columns["编号"].Visible = false;
                }
            }

            DataTable dtEducated = m_trainEmployeServer.GetEducatedHistory(m_id);

            if (dtEducated != null && dtEducated.Rows.Count > 0)
            {
                dgvEducatedHistory.DataSource = dtEducated;

                if (dgvEducatedHistory.Rows.Count > 0)
                {
                    dgvEducatedHistory.Columns["编号2"].Visible = false;
                }
            }

            DataTable dtFamilyMember = m_trainEmployeServer.GetFamilyMember(m_id);

            if (dtFamilyMember != null && dtFamilyMember.Rows.Count > 0)
            {
                dgvFamilyMember.DataSource = dtFamilyMember;

                if (dgvFamilyMember.Rows.Count > 0)
                {
                    dgvFamilyMember.Columns["编号3"].Visible = false;
                }
            }
        }

        private void 储备人才库明细_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        private void dgvWorkHistory_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
        }

        private void dgvEducatedHistory_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
        }

        private void dgvFamilyMember_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
        }

        private void llbAddAnnex_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenFileDialog ofdPic = new OpenFileDialog();

            ofdPic.FilterIndex = 1;
            ofdPic.RestoreDirectory = true;
            ofdPic.FileName = "";

            if (ofdPic.ShowDialog() == DialogResult.OK)
            {
                string sPicPaht = ofdPic.FileName.ToString();

                FileInfo fiPicInfo = new FileInfo(sPicPaht);

                long lPicLong = fiPicInfo.Length / 1024;
                string sPicName = fiPicInfo.Name;
                string sPicDirectory = fiPicInfo.Directory.ToString();
                string sPicDirectoryPath = fiPicInfo.DirectoryName;

                //如果文件大於150KB，警告    
                if (lPicLong > 150)
                {
                    MessageBox.Show("此文件大小为" + lPicLong + "K；已超过最大限制的150K范围！");
                }
                else
                {
                    pathName = ofdPic.SafeFileName;
                    Stream ms = ofdPic.OpenFile();
                    //picSignature.Image = Image.FromFile(filepath);
                    picbyte = new byte[ms.Length];
                    ms.Position = 0;
                    ms.Read(picbyte, 0, Convert.ToInt32(ms.Length));
                    ms.Close();

                    lblAnnxeName.Text = pathName;
                }
            }
        }

        private void llbLoadAnnex_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            byte[] by = picbyte;
            string filepath = "";//保存路径

            FolderBrowserDialog folder = new FolderBrowserDialog();
            OpenFileDialog ofdSelectPic = new OpenFileDialog();

            if (folder.ShowDialog() == DialogResult.OK)
            {
                filepath = folder.SelectedPath + "\\" + pathName;

                FileStream fs = new FileStream(filepath, FileMode.CreateNew);
                BinaryWriter bw = new BinaryWriter(fs);

                bw.Write(picbyte, 0, picbyte.Length);
                bw.Close();
                fs.Close();
                MessageBox.Show("下载成功,路径：" + filepath);
            }
            else
            {
                MessageBox.Show("请选择下载路径");
            }
        }

        private void dgvWorkHistory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            m_dataGridViewSelectRow = dgvWorkHistory.CurrentRow.Index + 1;
        }

        private void dgvWorkHistory_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvWorkHistory.IsCurrentCellDirty)
            {
                dgvWorkHistory.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dgvEducatedHistory_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvEducatedHistory.IsCurrentCellDirty)
            {
                dgvEducatedHistory.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dgvFamilyMember_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvFamilyMember.IsCurrentCellDirty)
            {
                dgvFamilyMember.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void 添加工作经历ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewRow dr = new DataGridViewRow();
            dgvWorkHistory.Rows.Add(dr);

            dgvWorkHistory.Rows[dgvWorkHistory.Rows.Count - 1].Cells["月酬"].Value = 0;
        }

        private void 删除工作经历ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (m_dataGridViewSelectRow > 0)
            {
                dgvWorkHistory.Rows.RemoveAt(m_dataGridViewSelectRow - 1);
            }
        }

        private void 添加教育培训ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewRow dr = new DataGridViewRow();
            dgvEducatedHistory.Rows.Add(dr);
        }

        private void 删除教育培训ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (m_dataGridViewSelectRow > 0)
            {
                dgvEducatedHistory.Rows.RemoveAt(m_dataGridViewSelectRow - 1);
            }
        }

        private void 添加家庭成员toolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewRow dr = new DataGridViewRow();
            dgvFamilyMember.Rows.Add(dr);
        }

        private void 删除家庭成员toolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_dataGridViewSelectRow > 0)
            {
                dgvFamilyMember.Rows.RemoveAt(m_dataGridViewSelectRow - 1);
            }
        }

         /// <summary>
        /// 检查控件
        /// </summary>
        /// <returns>完成返回True，未完成返回False</returns>
        bool CheckControl()
        {
            if (txtName.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请输入姓名！");
                return false;
            }

            if (cmbSex.SelectedIndex == -1)
            {
                MessageDialog.ShowPromptMessage("请选择性别！");
                return false;
            }

            if (dtpBirthday.Value.Year + 15 >= m_date.Year)
            {
                MessageDialog.ShowPromptMessage("请选择正确的出生年月！");
                return false;
            }

            if (txtPhone.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请输入手机号！");
                return false;
            }

            if (txtBirthplace.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请输入籍贯！");
                return false;
            }

            if (txtCard.Text.Trim() == "" || txtCard.Text.Length < 18)
            {
                MessageDialog.ShowPromptMessage("请输入正确的身份证号！");
                return false;
            }

            if (cmbPersonType.SelectedIndex == -1)
            {
                MessageDialog.ShowPromptMessage("请选择人才类型！");
                return false;
            }

            if (txtAddress.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请输入现居住地！");
                return false;
            }

            if (txtCollege.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请输入毕业院校！");
                return false;
            }

            if (txtEducatedDegree.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请输入学历！");
                return false;
            }

            if (txtSpeciality.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请输入擅长领域！");
                return false;
            }

            if (dtpInterviewDate.Checked && txtEvaluate.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请输入公司评价！");
                return false;
            }

            if (dgvEducatedHistory.Rows.Count <= 0)
            {
                MessageDialog.ShowPromptMessage("请输入教育/培训经历！");
                return false;
            }

            if (dtpTakeJobDate.Value.Year != m_date.Year && dgvWorkHistory.Rows.Count <= 0)
            {
                MessageDialog.ShowPromptMessage("请输入工作经历！");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 获得储备人才的数据集
        /// </summary>
        /// <returns>返回储备人才集合</returns>
        HR_TrainEmploye GetTrainEmployeData()
        {
            HR_TrainEmploye trainEmploye = new HR_TrainEmploye();

            trainEmploye.Address = txtAddress.Text.Trim();
            trainEmploye.Age = Convert.ToInt32(numAge.Value);
            trainEmploye.Birthday = dtpBirthday.Value;
            trainEmploye.Birthplace = txtBirthplace.Text;
            trainEmploye.College = txtCollege.Text.Trim();
            trainEmploye.ComputerLevel = txtComputerLevel.Text.Trim();
            trainEmploye.DesiredSalary = numDesiredSalary.Value;
            trainEmploye.EducatedDegree = txtEducatedDegree.Text.Trim();
            trainEmploye.EducatedMajor = txtEducatedMajor.Text.Trim();
            trainEmploye.EmergencyPhone = txtEmergencyPhone.Text.Trim();
            trainEmploye.EnglishLevel = txtEnglishLevel.Text.Trim();
            trainEmploye.Evaluate = txtEvaluate.Text.Trim();
            trainEmploye.ResumeStatus = cmbResumeStatus.Text;
            trainEmploye.Height = numHeight.Value;
            trainEmploye.ID_Card = txtCard.Text.Trim();

            if (dtpInterviewDate.Checked)
            {
                trainEmploye.InterviewDate = dtpInterviewDate.Value;
            }

            trainEmploye.IsThirdParty = cbIsThirdParty.Checked;
            trainEmploye.JobYears = Convert.ToInt32(numJobYears.Value);
            trainEmploye.MaritalStatus = cmbMaritalStatus.Text;
            trainEmploye.Name = txtName.Text;
            trainEmploye.Nationality = txtNationality.Text.Trim();
            trainEmploye.Party = txtParty.Text;
            trainEmploye.PersonType = cmbPersonType.Text;
            trainEmploye.Phone = txtPhone.Text;
            trainEmploye.Sex = cmbSex.Text;
            trainEmploye.Race = txtRace.Text;
            trainEmploye.RecruitType = cmbRecruitType.Text;
            trainEmploye.Speciality = txtSpeciality.Text.Trim();
            trainEmploye.TakeJobDate = dtpTakeJobDate.Value;
            trainEmploye.ThirdParty = txtThirdParty.Text.Trim();

            if (picbyte != null)
            {
                trainEmploye.Anne = picbyte;
                trainEmploye.FileName = pathName;
            }

            trainEmploye.Recorder = BasicInfo.LoginID;
            trainEmploye.RecordTime = ServerTime.Time;
            trainEmploye.Remark = txtRemark.Text;

            return trainEmploye;
        }

        /// <summary>
        /// 获得工作经验数据集
        /// </summary>
        /// <returns></returns>
        List<HR_WorkHistory> GetWorkHistory()
        {
            List<HR_WorkHistory> workList = new List<HR_WorkHistory>();

            for (int i = 0; i < dgvWorkHistory.Rows.Count; i++)
            {
                HR_WorkHistory workHistory = new HR_WorkHistory();
                DataGridViewCellCollection cells = dgvWorkHistory.Rows[i].Cells;

                workHistory.CompanyName = dgvWorkHistory.Rows[i].Cells["公司名称"].Value.ToString();
                workHistory.Pay = dgvWorkHistory.Rows[i].Cells["月酬"].Value.ToString();
                workHistory.Post = dgvWorkHistory.Rows[i].Cells["工作岗位"].Value.ToString();
                workHistory.StartTime = dgvWorkHistory.Rows[i].Cells["工作开始时间"].Value.ToString();
                workHistory.EndTime = dgvWorkHistory.Rows[i].Cells["截止时间"].Value.ToString();

                workList.Add(workHistory);
            }

            return workList;
        }

        /// <summary>
        /// 获得教育培训数据集
        /// </summary>
        /// <returns></returns>
        List<HR_EducatedHistory> GetEducatedHistory()
        {
            List<HR_EducatedHistory> educatedList = new List<HR_EducatedHistory>();

            for (int i = 0; i < dgvEducatedHistory.Rows.Count; i++)
            {
                HR_EducatedHistory educatedHistory = new HR_EducatedHistory();
                DataGridViewCellCollection cells = dgvEducatedHistory.Rows[i].Cells;

                educatedHistory.Diploma = dgvEducatedHistory.Rows[i].Cells["学历"].Value.ToString();
                educatedHistory.StartTime = dgvEducatedHistory.Rows[i].Cells["学习开始时间"].Value.ToString();
                educatedHistory.EndTime = dgvEducatedHistory.Rows[i].Cells["学习截止时间"].Value.ToString();
                educatedHistory.Major = dgvEducatedHistory.Rows[i].Cells["专业"].Value.ToString();
                educatedHistory.SchoolName = dgvEducatedHistory.Rows[i].Cells["学校"].Value.ToString();

                educatedList.Add(educatedHistory);
            }

            return educatedList;
        }

        /// <summary>
        /// 获得家庭成员数据集
        /// </summary>
        /// <returns></returns>
        List<HR_FamilyMember> GetFamilyMember()
        {
            List<HR_FamilyMember> familyList = new List<HR_FamilyMember>();

            for (int i = 0; i < dgvEducatedHistory.Rows.Count; i++)
            {
                HR_FamilyMember familyMember = new HR_FamilyMember();
                DataGridViewCellCollection cells = dgvFamilyMember.Rows[i].Cells;

                familyMember.Appellation = dgvFamilyMember.Rows[i].Cells["称谓"].Value.ToString();
                familyMember.CompanyName = dgvFamilyMember.Rows[i].Cells["单位"].Value.ToString();
                familyMember.Name = dgvFamilyMember.Rows[i].Cells["姓名"].Value.ToString();

                familyList.Add(familyMember);
            }

            return familyList;
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckControl())
            {
                return;
            }

            try
            {
                if (!m_trainEmployeServer.AddTrainEmploye(GetTrainEmployeData(), GetWorkHistory(),
                    GetEducatedHistory(), GetFamilyMember(), out m_error))
                {
                    MessageDialog.ShowPromptMessage(m_error);
                }
                else
                {
                    MessageDialog.ShowPromptMessage("保存成功！");
                    this.Close();
                }
            }
            catch (Exception)
            {                
                throw;
            }
        }

        private void 修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckControl())
            {
                return;
            }

            try
            {
                if (!m_trainEmployeServer.UpdateTrainEmploye(GetTrainEmployeData(), GetWorkHistory(),
                    GetEducatedHistory(), GetFamilyMember(),m_id, out m_error))
                {
                    MessageDialog.ShowPromptMessage(m_error);
                }
                else
                {
                    MessageDialog.ShowPromptMessage("修改成功！");
                    this.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void dtpBirthday_ValueChanged(object sender, EventArgs e)
        {
            numAge.Value = m_date.Year - dtpBirthday.Value.Year;
        }

        private void dtpTakeJobDate_ValueChanged(object sender, EventArgs e)
        {
            numJobYears.Value = m_date.Year - dtpTakeJobDate.Value.Year;
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageDialog.ShowEnquiryMessage("确定删除【" + txtName.Text + "】的信息吗？") == DialogResult.Yes)
            {
                if (!m_trainEmployeServer.DeleteTrainEmploye(m_id, out m_error))
                {
                    MessageDialog.ShowPromptMessage(m_error);
                }
                else
                {
                    MessageDialog.ShowPromptMessage("删除成功！");
                    ClearControl();
                }
            }
        }

        /// <summary>
        /// 清空控件
        /// </summary>
        private void ClearControl()
        {
            txtAddress.Text = "";
            numAge.Value = 0;
            dtpBirthday.Value = m_date;
            txtBirthplace.Text = "";
            txtCollege.Text = "";
            txtComputerLevel.Text = "";
            numDesiredSalary.Value = 0;
            txtEducatedDegree.Text = "";
            txtEducatedMajor.Text = "";
            txtEmergencyPhone.Text = "";
            txtEnglishLevel.Text = "";
            txtEvaluate.Text = "";
            cmbResumeStatus.SelectedIndex = -1;
            numHeight.Value = 0;
            txtCard.Text = "";
            dtpInterviewDate.Value = m_date;
            cbIsThirdParty.Checked = true;
            numJobYears.Value = 0;
            cmbMaritalStatus.SelectedIndex = -1;
            txtName.Text = "";
            txtNationality.Text = "";
            txtParty.Text = "";
            cmbPersonType.SelectedIndex = -1;
            cmbSex.SelectedIndex = -1;
            txtRace.Text = "";
            cmbRecruitType.SelectedIndex = -1;
            txtSpeciality.Text = "";
            dtpTakeJobDate.Value = m_date;
            txtThirdParty.Text = "";
            picbyte = null;
            lblAnnxeName.Text = "无";
            txtRemark.Text = "";

            DataTable dtWorkHistory = dgvWorkHistory.DataSource as DataTable;

            if (dtWorkHistory.Rows.Count > 0)
            {
                for (int i = 0; i < dtWorkHistory.Rows.Count; i++)
                {
                    dtWorkHistory.Rows.RemoveAt(i);
                }

                dgvWorkHistory.DataSource = dtWorkHistory;
            }

            DataTable dtEducated = dgvEducatedHistory.DataSource as DataTable;

            if (dtEducated.Rows.Count > 0)
            {
                for (int i = 0; i < dtEducated.Rows.Count; i++)
                {
                    dtEducated.Rows.RemoveAt(i);
                }

                dgvEducatedHistory.DataSource = dtEducated;
            }

            DataTable dtFamilyMember = dgvFamilyMember.DataSource as DataTable;

            if (dtFamilyMember.Rows.Count > 0)
            {
                for (int i = 0; i < dtFamilyMember.Rows.Count; i++)
                {
                    dtFamilyMember.Rows.RemoveAt(i);
                }

                dgvFamilyMember.DataSource = dtFamilyMember;
            }
        }

        private void 重置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearControl();
        }
    }
}
