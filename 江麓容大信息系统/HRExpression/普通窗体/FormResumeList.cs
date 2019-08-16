using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Expression;
using System.IO;
using ServerModule;
using Service_Peripheral_HR;
using GlobalObject;
using PlatformManagement;
using UniversalControlLibrary;

namespace Form_Peripheral_HR
{
    /// <summary>
    /// 人员简历子界面
    /// </summary>
    public partial class FormResumeList : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string error;

        /// <summary>
        /// 简历编号
        /// </summary>
        int resumeID;

        /// <summary>
        /// 文件流
        /// </summary>
        byte[] picbyte;

        /// <summary>
        /// 文件名
        /// </summary>
        string pathName;

        /// <summary>
        /// 权限控制标志
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 人员简历数据集
        /// </summary>
        HR_Resume m_resume;

        /// <summary>
        /// 人员简历管理类
        /// </summary>
        IResumeServer m_resumeServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IResumeServer>();

        public FormResumeList(AuthorityFlag authFlag, HR_Resume resume, string status)
        {
            InitializeComponent();

            m_authFlag = authFlag;

            m_resume = resume;
            toolStrip1.Visible = true;

            if (status != "新建")
            {
                添加toolStripButton1.Visible = false;
                toolStripSeparator4.Visible = false;

                BindControl();

                txtName.ReadOnly = true;
                txtCard.ReadOnly = true;
            }
            else
            {
                txtName.ReadOnly = false;
                txtCard.ReadOnly = false;

                修改toolStripButton1.Visible = false;
                删除toolStripButton1.Visible = false;
                toolStripSeparator1.Visible = false;
                toolStripSeparator2.Visible = false;
            }

            DataTable dt = m_resumeServer.GetResumeStatus();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cmbResumeStatus.Items.Add(dt.Rows[i]["Status"].ToString());
            }
        }

        private void FormResumeList_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authFlag);
        }

        public FormResumeList(AuthorityFlag authFlag, string CardID)
        {
            InitializeComponent();

            m_authFlag = authFlag;

            m_resume = m_resumeServer.GetResumelInfo(CardID);

            if (m_resume != null)
            {
                添加toolStripButton1.Visible = false;
                toolStripSeparator4.Visible = false;

                BindControl();

                txtName.ReadOnly = true;
                txtCard.ReadOnly = true;
            }
            else
            {
                MessageDialog.ShowPromptMessage("没有身份证为【"+CardID+"】的简历信息！");
                this.Close();
                return;
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

        private void 新建toolStripButton1_Click(object sender, EventArgs e)
        {
            ClearControl();
            txtCard.ReadOnly = false;
            txtName.ReadOnly = false;

            添加toolStripButton1.Visible = true;
            toolStripSeparator4.Visible = true;
            修改toolStripButton1.Visible = false;
            删除toolStripButton1.Visible = false;
            toolStripSeparator1.Visible = false;
            toolStripSeparator2.Visible = false;
        }

        void BindControl()
        {
            resumeID = m_resume.ID;
            txtName.Text = m_resume.Name;
            cmbRecruitType.Text = m_resume.RecruitmentType;
            cmbResumeStatus.Text = m_resumeServer.GetResumeStatusByID(m_resume.ResumeStatusID);
            cmbSex.Text = m_resume.Sex;
            dtpBirthday.Value = m_resume.Birthday;
            numAge.Value = m_resume.Age;
            numHeight.Value = m_resume.Height;
            txtNationality.Text = m_resume.Nationality;
            txtRace.Text = m_resume.Race;
            txtBirthplace.Text = m_resume.Birthplace;
            txtParty.Text = m_resume.Party;
            cmbMaritalStatus.Text = m_resume.MaritalStatus;
            txtCard.Text = m_resume.ID_Card;
            txtCollege.Text = m_resume.College;
            txtEducatedDegree.Text = m_resume.EducatedDegree;
            txtEducatedMajor.Text = m_resume.EducatedMajor;
            txtFamilyAddress.Text = m_resume.FamilyAddress;
            txtPostcode.Text = m_resume.Postcode;
            txtEmergencyPhone.Text = m_resume.EmergencyPhone;
            txtSpeciality.Text = m_resume.Speciality;
            txtEnglishLevel.Text = m_resume.EnglishLevel;
            txtComputerLevel.Text = m_resume.ComputerLevel;
            txtPhone.Text = m_resume.Phone;
            cbIsThirdParty.Checked = m_resume.ExistThirdPartyRelation;
            txtThirdParty.Text = m_resume.ThirdPartyRelation;
            numWorkingTenure.Value = m_resume.WorkingTenure;
            txtWorkHistory.Text = m_resume.WorkHistory;
            txtEducatedHistory.Text = m_resume.EducatedHistory;
            txtFamilyMember.Text = m_resume.FamilyMember;
            numDesiredSalary.Value = m_resume.DesiredSalary;

            if (m_resume.Photo != null)
            {
                picbPhoto.Image = m_resume.Photo == null ? null : GetPicture(m_resume.Photo.ToArray());
            }

            if (m_resume.Annex != null)
            {
                picbyte = m_resume.Annex == null ? null : m_resume.Annex.ToArray();
                lblAnnxeName.Text = m_resume.PathName;
            }
            else
            {
                llbLoadAnnex.Visible = false;
            }

            txtRemark.Text = m_resume.Remark;
        }

        private void 添加toolStripButton1_Click(object sender, EventArgs e)
        {
            if (!CheckControl())
            {
                return;
            }

            if (!m_resumeServer.AddResume(GetResumeData(),0, out error))
            {
                MessageDialog.ShowPromptMessage(error);
                return;
            }

            ClearControl();
        }

        private void 修改toolStripButton1_Click(object sender, EventArgs e)
        {
            if (!CheckControl())
            {
                return;
            }

            if (!m_resumeServer.AddResume(GetResumeData(), 1, out error))
            {
                MessageDialog.ShowPromptMessage(error);
                return;
            }

            ClearControl();
        }

        private void 删除toolStripButton1_Click(object sender, EventArgs e)
        {
            if (txtCard.Text.Trim() == null && txtName.Text.Trim() == null)
            {
                MessageDialog.ShowPromptMessage("选择需要删除的数据行！");
                return;
            }

            string card = txtCard.Text.Trim();

            if (MessageBox.Show("您是否确定要删除" + txtCard.Text.Trim()
                + "信息?", "消息", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (!m_resumeServer.DeleteResume(card, resumeID, out error))
                {
                    MessageDialog.ShowPromptMessage(error);
                    return;
                }
            }

            ClearControl();
        }

        /// <summary>
        /// 清空控件
        /// </summary>
        void ClearControl()
        {
            foreach (Control cl in this.panel5.Controls)
            {
                if (cl is TextBox)
                {
                    ((TextBox)cl).Text = "";
                }
            }

            txtEducatedHistory.Text = "";
            txtWorkHistory.Text = "";
            txtFamilyMember.Text = "";
            picbPhoto.Image = null;
            picbyte = null;
        }

        /// <summary>
        /// 检查控件
        /// </summary>
        /// <returns>完成返回True，未完成返回False</returns>
        bool CheckControl()
        {
            if (cbIsThirdParty.Checked && txtThirdParty.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请填写第三方关系！");
                return false;
            }

            if (txtEducatedHistory.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请填写教育经历！");
                return false;
            }

            if (numWorkingTenure.Value > 0 && txtWorkHistory.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请填写工作经历！");
                return false;
            }

            if (txtPhone.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请输入人员手机！");
                return false;
            }

            return true;
        }


        private void llbPhoto_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenFileDialog ofdPic = new OpenFileDialog();

            ofdPic.Filter = "JPG(*.JPG;*.JPEG);gif文件(*.GIF)|*.jpg;*.jpeg;*.gif";
            ofdPic.FilterIndex = 1;
            ofdPic.RestoreDirectory = true;
            ofdPic.FileName = "";

            if (ofdPic.ShowDialog() == DialogResult.OK)
            {
                if (picbPhoto.Image != null)
                {
                    picbPhoto.Image.Dispose();
                    picbPhoto.Image = null;
                }

                string sPicPaht = ofdPic.FileName.ToString();

                FileInfo fiPicInfo = new FileInfo(sPicPaht);

                long lPicLong = fiPicInfo.Length / 1024;
                string sPathName = fiPicInfo.Name;
                string sPicDirectory = fiPicInfo.Directory.ToString();
                string sPicDirectoryPath = fiPicInfo.DirectoryName;

                //如果文件大於60KB，警告    
                if (lPicLong > 60)
                {
                    MessageBox.Show("此文件大小为" + lPicLong + "K；已超过最大限制的60K范围！");
                }
                else
                {
                    picbPhoto.Image = Bitmap.FromFile(sPicPaht);
                }
            }
        }

        /// <summary>
        /// 图像转化二进制
        /// </summary>
        /// <param name="image">图像</param>
        /// <returns>返回二进制</returns>
        private static byte[] GetPicToBinary(Image image)
        {
            MemoryStream ms = new MemoryStream();
            Bitmap bitmap = new Bitmap(image);
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

            byte[] b = ms.ToArray();
            return b;
        }

        /// <summary>
        /// 二进制转化成图像
        /// </summary>
        /// <param name="byPct">二进制</param>
        /// <returns>返回单个图像</returns>
        private static Image GetPicture(byte[] byPct)
        {
            MemoryStream ms = new MemoryStream(byPct);
            ms.Write(byPct, 0, byPct.Length);
            Image img = Bitmap.FromStream(ms);

            ms.Close();
            return img;
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

        /// <summary>
        /// 获得人员简历数据集
        /// </summary>
        /// <returns>返回人员简历集合</returns>
        HR_Resume GetResumeData()
        {
            HR_Resume resume = new HR_Resume();

            resume.Name = txtName.Text;
            resume.RecruitmentType = cmbRecruitType.Text;
            resume.ResumeStatusID = m_resumeServer.GetResumeStatusByStatus(cmbResumeStatus.Text);
            resume.Sex = cmbSex.Text;
            resume.Birthday = dtpBirthday.Value;
            resume.Age = Convert.ToInt32(numAge.Value);
            resume.Height = numHeight.Value;
            resume.Nationality = txtNationality.Text;
            resume.Race = txtRace.Text;
            resume.Birthplace = txtBirthplace.Text;
            resume.Party = txtParty.Text;
            resume.MaritalStatus = cmbMaritalStatus.Text;
            resume.ID_Card = txtCard.Text;
            resume.College = txtCollege.Text;
            resume.EducatedDegree = txtEducatedDegree.Text;
            resume.EducatedMajor = txtEducatedMajor.Text;
            resume.FamilyAddress = txtFamilyAddress.Text;
            resume.Postcode = txtPostcode.Text;
            resume.EmergencyPhone = txtEmergencyPhone.Text;
            resume.Speciality = txtSpeciality.Text;
            resume.EnglishLevel = txtEnglishLevel.Text;
            resume.ComputerLevel = txtComputerLevel.Text;
            resume.Phone = txtPhone.Text;
            resume.ExistThirdPartyRelation = cbIsThirdParty.Checked;
            resume.ThirdPartyRelation = txtThirdParty.Text;
            resume.WorkingTenure = numWorkingTenure.Value;
            resume.WorkHistory = txtWorkHistory.Text;
            resume.EducatedHistory = txtEducatedHistory.Text;
            resume.FamilyMember = txtFamilyMember.Text;
            resume.DesiredSalary = numDesiredSalary.Value;

            if (picbPhoto.Image != null)
            {
                resume.Photo = GetPicToBinary(picbPhoto.Image);
            }

            if (picbyte != null)
            {
                resume.Annex = picbyte;
                resume.PathName = pathName;
            }

            resume.Recorder = BasicInfo.LoginID;
            resume.RecordTime = ServerTime.Time;
            resume.Remark = txtRemark.Text;

            return resume;
        }

        private void FormResumeList_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        private void 档案toolStripButton_Click(object sender, EventArgs e)
        {
            FormPersonnelArchiveList frm = new FormPersonnelArchiveList(m_authFlag,txtCard.Text);
            frm.ShowDialog();
        }
    }
}
