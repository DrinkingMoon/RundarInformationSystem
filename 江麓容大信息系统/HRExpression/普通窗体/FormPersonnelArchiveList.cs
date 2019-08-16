﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using PlatformManagement;
using Service_Peripheral_HR;
using System.IO;
using Expression;
using GlobalObject;
using UniversalControlLibrary;

namespace Form_Peripheral_HR
{
    /// <summary>
    /// 员工档案子界面
    /// </summary>
    public partial class FormPersonnelArchiveList : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string error;

        /// <summary>
        /// 文件流
        /// </summary>
        byte[] picbyte;

        /// <summary>
        /// 文件名
        /// </summary>
        string pathName;

        /// <summary>
        /// 修改标志
        /// </summary>
        public bool updateFlag = false;

        /// <summary>
        /// 是否转正标志
        /// </summary>
        //bool m_flag;

        /// <summary>
        /// 权限控制标志
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 查询结果
        /// </summary>
        IQueryResult m_queryResult;

        /// <summary>
        /// 角色管理器
        /// </summary>
        IRoleManagement m_roleManager = PlatformFactory.GetObject<IRoleManagement>();

        /// <summary>
        /// 数据库用户操作类
        /// </summary>
        IUserManagement m_userManager = PlatformFactory.GetObject<IUserManagement>();

        /// <summary>
        /// 人员档案变更数据集
        /// </summary>
        HR_PersonnelArchiveChange m_personnelChange;

        /// <summary>
        /// 人员档案数据集
        /// </summary>
        HR_PersonnelArchive m_personnelArchive;

        /// <summary>
        /// 岗位管理服务类
        /// </summary>
        IOperatingPostServer m_PostServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IOperatingPostServer>();

        /// <summary>
        /// 职称信息管理类
        /// </summary>
        IJobTitleServer m_JobServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IJobTitleServer>();

        /// <summary>
        /// 部门信息服务组件
        /// </summary>
        IOrganizationServer m_departmentServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IOrganizationServer>();

        /// <summary>
        /// 人员档案管理类
        /// </summary>
        IPersonnelArchiveServer m_personnerServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IPersonnelArchiveServer>();

        /// <summary>
        /// 人员简历管理类
        /// </summary>
        IResumeServer m_resumeServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IResumeServer>();

        public FormPersonnelArchiveList(AuthorityFlag authFlag, HR_PersonnelArchiveChange personnelChange,
            HR_PersonnelArchive personnelArchive, string allowTime, string starTime, string endTime,IQueryResult result)
        {
            InitializeComponent();

            if (personnelChange != null)
            {
                m_personnelArchive = personnelArchive;
                m_personnelChange = personnelChange;
                m_queryResult = result;
                m_authFlag = authFlag;
                AuthorityControl(m_authFlag);

                添加toolStripButton1.Visible = false;
                新建toolStripButton1.Visible = false;
                toolStripSeparator4.Visible = false;
                toolStripSeparator1.Visible = false;

                txtAllowTime.Text = allowTime;
                txtStarTime.Text = starTime;
                txtEndTime.Text = endTime;

                BindControl();

                FaceAuthoritySetting.SetVisibly(this.Controls, result.HideFields);
            }
            else
            {
                修改toolStripButton1.Visible = false;
                toolStripSeparator1.Visible = false;

                txtCard.ReadOnly = false;
                txtName.ReadOnly = false;
                txtWorkID.ReadOnly = false;
                txtPostAmount.ReadOnly = false;
                txtTrainingAmount.ReadOnly = false;
                txtChangeAmount.ReadOnly = false;
                dtpBirthday.Value = ServerTime.Time;
                dtpJoinDate.Value = ServerTime.Time;
                dtpBecomeDate.Value = ServerTime.Time;
                dtpTakeJobDate.Value = ServerTime.Time;
                dtpDimissionDate.Value = ServerTime.Time;
            }

            toolStrip1.Visible = true;
        }

        public FormPersonnelArchiveList(AuthorityFlag authFlag,string cardID)
        {
            InitializeComponent();

            m_authFlag = authFlag;
            AuthorityControl(m_authFlag);

            m_personnelArchive = m_personnerServer.GetPersonnelArchiveByCardID(cardID);
            
            if (m_personnelArchive != null)
            {
                BindControl();

                新建toolStripButton1.Visible = true;
            }
            else
            {
                MessageDialog.ShowPromptMessage("没有身份证为【"+cardID+"】的员工档案信息！");
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
            txtWorkID.ReadOnly = false;
            txtPostAmount.ReadOnly = false;
            txtTrainingAmount.ReadOnly = false;
            txtChangeAmount.ReadOnly = false;
            txtPostAmount.Text = "0";
            txtTrainingAmount.Text = "0";
            txtChangeAmount.Text = "0";
        }

        private void 添加toolStripButton1_Click(object sender, EventArgs e)
        {
            if (!CheckControl())
            {
                return;
            }

            //if (!m_personnerServer.AddPersonnelArchive(GetPersonnelArchiveData(), out error))
            //{
            //    MessageDialog.ShowPromptMessage(error);
            //    return;
            //}

            IQueryable<View_Auth_Role> authRole = m_roleManager.GetAllRoles();
            string roleCode = "";

            foreach (var item in authRole)
            {
                if (item.角色名称 == CE_RoleEnum.普通操作员.ToString())
                {
                    roleCode = item.角色编码;
                }
            }

            Auth_User userInfo = new Auth_User();

            userInfo.LoginName = txtWorkID.Text.Trim();
            userInfo.Password = "1";
            userInfo.TrueName = txtName.Text.Trim();
            userInfo.Dept = m_departmentServer.GetDeptCode(cmbDept.Text);
            userInfo.IsActived = false;
            userInfo.IsAdmin = false;
            userInfo.AuthenticationMode = "密码认证";
            userInfo.IsLocked = false;
            userInfo.CreateDate = ServerTime.Time;
            userInfo.DestroyFlag = false;
            userInfo.Remarks = "通过人员档案自动添加";

            if (m_userManager.AddUser(userInfo))
            {
                if (!m_roleManager.AddUserInRole(roleCode, txtWorkID.Text))
                {
                    MessageDialog.ShowPromptMessage("员工信档案添加成功，角色分配失败！");
                }
            }

            updateFlag = true;
            this.Close();
        }

        private void 修改toolStripButton1_Click(object sender, EventArgs e)
        {
            if (!CheckControl())
            {
                return;
            }

            if (!m_personnerServer.UpdatePersonnelArchive(m_personnelChange, GetPersonnelArchiveData(), null , out error))
            {
                MessageDialog.ShowPromptMessage(error);
                return;
            }

            updateFlag = true;
            this.Close();
        }

        /// <summary>
        /// 窗体加载
        /// </summary>
        private void FormPersonnelArchiveList_Load(object sender, EventArgs e)
        {
            DataTable postDt = m_PostServer.GetOperatingPost(null);

            for (int i = 0; i < postDt.Rows.Count; i++)
            {
                cmbWorkPost.Items.Add(postDt.Rows[i]["岗位名称"].ToString());
            }

            DataTable jobDt = m_JobServer.GetJobTitleOut();

            for (int i = 0; i < jobDt.Rows.Count; i++)
            {
                cmbJobTitle.Items.Add(jobDt.Rows[i]["职称名称"].ToString());
            }

            DataTable levelDt = m_JobServer.GetJobTitleLevel();

            for (int i = 0; i < levelDt.Rows.Count; i++)
            {
                cmbLevel.Items.Add(levelDt.Rows[i]["职称名称"].ToString());
            }

            DataTable statusDt = m_personnerServer.GetPersonnelStatus();

            for (int i = 0; i < statusDt.Rows.Count; i++)
            {
                cmbStatus.Items.Add(statusDt.Rows[i]["status"].ToString());
            }

            IQueryable<View_HR_Dept> m_findDepartment;

            if (m_departmentServer.GetAllDeptInfo(out m_findDepartment, out error))
            {
                foreach (var item in m_findDepartment)
                {
                    cmbDept.Items.Add(item.部门名称);
                }
            }
        }

        /// <summary>
        /// 改变组件大小
        /// </summary>
        private void FormPersonnelArchiveList_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
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

            picbPhoto.Image = null;
            picbyte = null;
            lblAnnexName.Text = "";
        }

        /// <summary>
        /// 刷新
        /// </summary>
        void RefreshControl()
        {
            ClearControl();
        }

        #region 附件上传和下载
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

        /// <summary>
        /// 上传附件
        /// </summary>
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
                    picbyte = new byte[ms.Length];
                    ms.Position = 0;
                    ms.Read(picbyte, 0, Convert.ToInt32(ms.Length));
                    ms.Close();
                }
            }
        }

        /// <summary>
        /// 下载附件
        /// </summary>
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
        /// 插入图片
        /// </summary>
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
        #endregion

        /// <summary>
        /// 绑定控件
        /// </summary>
        void BindControl()
        {
            txtWorkID.Text = m_personnelArchive.WorkID;
            txtName.Text = m_personnelArchive.Name;
            cmbWorkPost.Text = m_PostServer.GetOperatingPostByPostCode(m_personnelArchive.WorkPost);
            cmbJobTitle.Text =m_JobServer.GetJobTitleByJobID(Convert.ToInt32(m_personnelArchive.JobTitleID));
            cmbLevel.Text = m_JobServer.GetJobTitleByJobID(Convert.ToInt32(m_personnelArchive.JobLevelID));
            cmbSex.Text = m_personnelArchive.Sex;
            cmbDept.Text = m_departmentServer.GetDeptByDeptCode(m_personnelArchive.Dept).部门名称;
            cmbStatus.Text = m_personnerServer.GetStatusByID(Convert.ToInt32(m_personnelArchive.PersonnelStatus));
            dtpBirthday.Value = Convert.ToDateTime(m_personnelArchive.Birthday);
            txtNationality.Text = m_personnelArchive.Nationality;
            txtRace.Text = m_personnelArchive.Race;
            txtBirthplace.Text = m_personnelArchive.Birthplace;
            txtParty.Text = m_personnelArchive.Party;
            txtCard.Text = m_personnelArchive.ID_Card;
            txtCollege.Text = m_personnelArchive.College;
            txtEducatedDegree.Text = m_personnelArchive.EducatedDegree;
            txtEducatedMajor.Text = m_personnelArchive.EducatedMajor;
            txtFamilyAddress.Text = m_personnelArchive.FamilyAddress;
            txtPostcode.Text = m_personnelArchive.PostCode;
            txtPhone.Text = m_personnelArchive.Phone;
            txtSpeciality.Text = m_personnelArchive.Speciality;
            txtMobilePhone.Text = m_personnelArchive.MobilePhone;
            txtChangeAmount.Text = m_personnelArchive.ChangeAmount.ToString();
            txtTrainingAmount.Text = m_personnelArchive.TrainingAmount.ToString();
            txtPostAmount.Text = m_personnelArchive.ChangePostAmount.ToString();
            txtBank.Text = m_personnelArchive.Bank;
            txtBankAccount.Text = m_personnelArchive.BankAccount;
            txtQQ.Text = m_personnelArchive.QQ;
            txtEmail.Text = m_personnelArchive.Email;
            txtHobby.Text = m_personnelArchive.Hobby;
            txtSSNumber.Text = m_personnelArchive.SocietySecurityNumber;
            txtResume.Text = m_personnelArchive.ResumeID.ToString();
            txtJobNature.Text = m_personnelArchive.JobNature;
            txtGraduationYear.Text = m_personnelArchive.GraduationYear.ToString();
            txtLengthOfSchooling.Text = m_personnelArchive.LengthOfSchooling;
            cmbMaritalStatus.Text = m_personnelArchive.MaritalStatus;
            cbCore.Checked = Convert.ToBoolean(m_personnelArchive.IsCore);

            if (m_personnelArchive.JoinDate.ToString() != "")
            {
                dtpJoinDate.Value = Convert.ToDateTime(m_personnelArchive.JoinDate);
            }

            if (m_personnelArchive.DimissionDate.ToString() != "")
            {
                dtpDimissionDate.Value = Convert.ToDateTime(m_personnelArchive.DimissionDate);
            }
            else
            {
                dtpDimissionDate.Checked = false;
            }

            if (m_personnelArchive.BecomeRegularEmployeeDate.ToString() != "")
            {
                dtpBecomeDate.Value = Convert.ToDateTime(m_personnelArchive.BecomeRegularEmployeeDate);
            }
            else
            {
                dtpBecomeDate.Checked = false;
            }

            if (m_personnelArchive.TakeJobDate.ToString() != "")
            {
                dtpTakeJobDate.Value = Convert.ToDateTime(m_personnelArchive.TakeJobDate);
            }
            else
            {
                dtpTakeJobDate.Checked = false;
            }

            if (m_personnelArchive.Photo != null)
            {
                picbPhoto.Image = m_personnelArchive.Photo == null ? null : GetPicture(m_personnelArchive.Photo.ToArray());
            }

            if (m_personnelArchive.Annex != null)
            {
                picbyte = m_personnelArchive.Annex == null ? null : m_personnelArchive.Annex.ToArray();
                pathName = m_personnelArchive.AnnexName;
                lblAnnexName.Text = pathName;
            }
            else
            {
                llbLoadAnnex.Visible = false;
                lblAnnexName.Visible = false;
            }

            txtRemark.Text = m_personnelArchive.Remark;

            txtName.ReadOnly = true;
            txtCard.ReadOnly = true;
            txtWorkID.ReadOnly = true;
        }

        /// <summary>
        /// 检查控件
        /// </summary>
        /// <returns>没问题返回True，有问题返回False</returns>
        bool CheckControl()
        {
            if (txtWorkID.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请填写员工编号！");
                return false;
            }

            //if (txtWorkID.Text.Length != 4)
            //{
            //    MessageDialog.ShowPromptMessage("员工编号有误，请重新输入！");
            //    return false;
            //}

            if (cmbStatus.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请选择人员状态！");
                return false;
            }

            if (cmbDept.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请选择部门！");
                return false;
            }

            if (cmbWorkPost.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请选择职位！");
                return false;
            }

            if (cmbJobTitle.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请选择职称！");
                return false;
            }

            if (cmbLevel.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请选择级别！");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 获得人员档案数据集
        /// </summary>
        /// <returns>返回人员档案集合</returns>
        HR_PersonnelArchive GetPersonnelArchiveData()
        {
            HR_PersonnelArchive personnel = new HR_PersonnelArchive();

            personnel.WorkID = txtWorkID.Text;
            personnel.Name = txtName.Text;
            personnel.Sex = cmbSex.Text; 
            personnel.JoinDate = dtpJoinDate.Value;
            personnel.Birthday = dtpBirthday.Value;
            personnel.Nationality = txtNationality.Text;
            personnel.Race = txtRace.Text;
            personnel.Birthplace = txtBirthplace.Text;
            personnel.Party = txtParty.Text;
            personnel.ID_Card = txtCard.Text;
            personnel.College = txtCollege.Text;
            personnel.EducatedDegree = txtEducatedDegree.Text;
            personnel.EducatedMajor = txtEducatedMajor.Text;
            personnel.FamilyAddress = txtFamilyAddress.Text;
            personnel.PostCode = txtPostcode.Text;
            personnel.Phone = txtPhone.Text;
            personnel.Speciality = txtSpeciality.Text;
            personnel.MobilePhone = txtMobilePhone.Text;
            personnel.Hobby = txtHobby.Text;
            personnel.QQ = txtQQ.Text;
            personnel.Email = txtEmail.Text;
            personnel.Bank = txtBank.Text;
            personnel.BankAccount = txtBankAccount.Text;
            personnel.SocietySecurityNumber = txtSSNumber.Text;
            personnel.TrainingAmount = Convert.ToInt32(txtTrainingAmount.Text);
            personnel.ChangePostAmount = Convert.ToInt32(txtPostAmount.Text);
            personnel.ChangeAmount = Convert.ToInt32(txtChangeAmount.Text);
            personnel.JobNature = txtJobNature.Text;
            personnel.LengthOfSchooling = txtLengthOfSchooling.Text;
            personnel.MaritalStatus = cmbMaritalStatus.Text;

            if (dtpBecomeDate.Checked)
            {
                personnel.BecomeRegularEmployeeDate = dtpBecomeDate.Value;
                //m_flag = true;
            }

            if (dtpTakeJobDate.Checked)
            {
                personnel.TakeJobDate = dtpTakeJobDate.Value;
            }

            personnel.ArchivePosition = txtArchivePosition.Text;
            personnel.PersonnelStatus = m_personnerServer.GetStatusByName(cmbStatus.Text);
            personnel.Dept = m_departmentServer.GetDeptCode(cmbDept.Text);
            personnel.WorkPost = m_PostServer.GetOperatingPostByPostName(cmbWorkPost.Text).岗位编号;
            personnel.JobTitleID = m_JobServer.GetJobTitleByJobName(cmbJobTitle.Text);

            if (dtpDimissionDate.Checked)
            {
                personnel.DimissionDate = dtpDimissionDate.Value;
            }

            personnel.IsCore = cbCore.Checked;
            personnel.JobLevelID = m_JobServer.GetJobTitleByJobName(cmbLevel.Text);

            if (txtGraduationYear.Text.Trim() == "")
            {
                personnel.GraduationYear = 0;
            }
            else
            {
                personnel.GraduationYear = Convert.ToInt32(txtGraduationYear.Text);
            }

            if (picbPhoto.Image != null)
            {
                personnel.Photo = GetPicToBinary(picbPhoto.Image);
            }

            if (picbyte != null)
            {
                personnel.Annex = picbyte;
                personnel.AnnexName = pathName;
            }

            if (txtResume.Text.Trim() != "")
            {
                personnel.ResumeID = Convert.ToInt32(txtResume.Text);
            }

            personnel.Recorder = BasicInfo.LoginID;
            personnel.RecordTime = ServerTime.Time;
            personnel.Remark = txtRemark.Text;
            personnel.PY = UniversalFunction.GetPYWBCode(txtName.Text, "PY");
            personnel.WB = UniversalFunction.GetPYWBCode(txtName.Text, "WB");

            return personnel;
        }

        private void 打印toolStripButton1_Click(object sender, EventArgs e)
        {
            IBillReportInfo report = new 报表_人员档案(txtWorkID.Text.Trim());

            PrintReportBill print = new PrintReportBill(21.8, 9.31, report);
            (report as 报表_人员档案).ShowDialog();
        }

        private void 历史toolStripButton1_Click(object sender, EventArgs e)
        {
            this.Close();

            ((IMainForm)StapleInfo.MainForm).ShowForm("人员档案变更历史", WndMsgSender.ShowSpecificData, txtWorkID.Text);
        }

        private void 查看简历toolStripButton1_Click(object sender, EventArgs e)
        {
            FormResumeList frm = new FormResumeList(m_authFlag,txtCard.Text);
            frm.ShowDialog();
        }

        private void txtResume_DoubleClick(object sender, EventArgs e)
        {
            FormPersonnelResume frm = new FormPersonnelResume(txtResume,"编号");
            frm.ShowDialog();

            DataTable dt = m_resumeServer.GetResume(frm.UserCode);

            if (dt.Rows.Count == 1)
            {
                txtName.Text = dt.Rows[0]["姓名"].ToString();
                txtNationality.Text = dt.Rows[0]["国籍"].ToString();
                txtParty.Text = dt.Rows[0]["政治面貌"].ToString();
                txtFamilyAddress.Text = dt.Rows[0]["家庭住址"].ToString();
                txtCollege.Text = dt.Rows[0]["毕业院校"].ToString();
                txtEducatedDegree.Text = dt.Rows[0]["学历"].ToString();
                txtEducatedMajor.Text = dt.Rows[0]["专业"].ToString();
                txtCard.Text = dt.Rows[0]["身份证"].ToString();
                txtBirthplace.Text = dt.Rows[0]["籍贯"].ToString();
                txtRace.Text = dt.Rows[0]["民族"].ToString();
                cmbSex.Text = dt.Rows[0]["性别"].ToString();
                dtpBirthday.Value = Convert.ToDateTime(dt.Rows[0]["出生日期"].ToString());
                txtPostcode.Text = dt.Rows[0]["邮编"].ToString();
                cmbMaritalStatus.Text = dt.Rows[0]["婚姻状况"].ToString();
                txtResume.Text = frm.UserCode;

                if (dt.Rows[0]["照片"].ToString() != "")
                {
                    picbPhoto.Image = GetPicture(dt.Rows[0]["照片"] as byte[]);
                }

                if (dt.Rows[0]["附件"].ToString() != "")
                {
                    picbyte = dt.Rows[0]["附件"] as byte[];
                    pathName = dt.Rows[0]["附件名"].ToString();
                    lblAnnexName.Text = pathName;
                }
                else
                {
                    llbLoadAnnex.Visible = false;
                    lblAnnexName.Visible = false;
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("简历信息有误，请手动输入");
            }
        }
    }
}
