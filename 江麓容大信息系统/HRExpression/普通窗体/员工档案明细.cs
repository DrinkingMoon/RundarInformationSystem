using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UniversalControlLibrary;
using PlatformManagement;
using ServerModule;
using Service_Peripheral_HR;
using System.IO;
using GlobalObject;

namespace Form_Peripheral_HR
{
    public partial class 员工档案明细 : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_error;

        /// <summary>
        /// 文件流
        /// </summary>
        byte[] m_picbyte;

        /// <summary>
        /// 文件名
        /// </summary>
        string m_pathName;

        /// <summary>
        /// 修改标志
        /// </summary>
        private bool m_updateFlag = false;

        public bool UpdateFlag
        {
            get { return m_updateFlag; }
            set { m_updateFlag = value; }
        }

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
        /// 岗位调动服务类
        /// </summary>
        IPostChangeServer m_PostChangeServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IPostChangeServer>();

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
        /// 储备人才管理类
        /// </summary>
        ITrainEmployeServer m_trainEmployeServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<ITrainEmployeServer>();

        /// <summary>
        /// 获取预警通知类消息操作接口
        /// </summary>
        IWarningNotice m_warningNotice = PlatformFactory.GetObject<IWarningNotice>();

        /// <summary>
        /// 合同管理类
        /// </summary>
        ILaborContractServer m_laborServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<ILaborContractServer>();

        /// <summary>
        /// 人员简历服务类
        /// </summary>
        IResumeServer m_resumeServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IResumeServer>();

        /// <summary>
        /// 是否转正标志
        /// </summary>
        //bool m_flag;

        public 员工档案明细(AuthorityFlag authFlag, HR_PersonnelArchiveChange personnelChange,
            HR_PersonnelArchive personnelArchive, IQueryResult result)
        {
            InitializeComponent();

            m_personnelArchive = personnelArchive;
            m_personnelChange = personnelChange;
            m_queryResult = result;
            m_authFlag = authFlag;
        }

        /// <summary>
        /// 权限控制
        /// </summary>
        /// <param name="authorityFlag">权限标志</param>
        void AuthorityControl(PlatformManagement.AuthorityFlag authorityFlag)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip2, authorityFlag);
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
            m_picbyte = null;
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
                    m_pathName = ofdPic.SafeFileName;
                    Stream ms = ofdPic.OpenFile();
                    m_picbyte = new byte[ms.Length];
                    ms.Position = 0;
                    ms.Read(m_picbyte, 0, Convert.ToInt32(ms.Length));
                    ms.Close();
                }
            }
        }

        /// <summary>
        /// 下载附件
        /// </summary>
        private void llbLoadAnnex_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            byte[] by = m_picbyte;
            string filepath = "";//保存路径

            FolderBrowserDialog folder = new FolderBrowserDialog();
            OpenFileDialog ofdSelectPic = new OpenFileDialog();

            if (folder.ShowDialog() == DialogResult.OK)
            {
                filepath = folder.SelectedPath + "\\" + m_pathName;

                FileStream fs = new FileStream(filepath, FileMode.CreateNew);
                BinaryWriter bw = new BinaryWriter(fs);

                bw.Write(m_picbyte, 0, m_picbyte.Length);
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
            cmbDept.Text = m_departmentServer.GetDeptByDeptCode(m_personnelArchive.Dept).部门名称;
            cmbWorkPost.Text = m_PostServer.GetOperatingPostByPostCode(m_personnelArchive.WorkPost);
            cmbJobTitle.Text = m_JobServer.GetJobTitleByJobID(Convert.ToInt32(m_personnelArchive.JobLevelID));
            cmbLevel.Text = m_JobServer.GetJobTitleByJobID(Convert.ToInt32(m_personnelArchive.JobLevelID));
            cmbSex.Text = m_personnelArchive.Sex;            
            cmbStatus.Text = m_personnerServer.GetStatusByID(Convert.ToInt32(m_personnelArchive.PersonnelStatus));
            dtpBirthday.Value = Convert.ToDateTime(m_personnelArchive.Birthday);
            txtNationality.Text = m_personnelArchive.Nationality;
            txtRace.Text = m_personnelArchive.Race;
            txtBirthplace.Text = m_personnelArchive.Birthplace;
            txtParty.Text = m_personnelArchive.Party;
            txtCard.Text = m_personnelArchive.ID_Card;
            txtCollege.Text = m_personnelArchive.College;
            cmbEducatedDegree.Text = m_personnelArchive.EducatedDegree;
            txtEducatedMajor.Text = m_personnelArchive.EducatedMajor;
            txtFamilyAddress.Text = m_personnelArchive.FamilyAddress;
            txtPhone.Text = m_personnelArchive.Phone;
            txtSpeciality.Text = m_personnelArchive.Speciality;
            txtMobilePhone.Text = m_personnelArchive.MobilePhone;
            txtQQ.Text = m_personnelArchive.QQ;
            txtEmail.Text = m_personnelArchive.Email;
            txtHobby.Text = m_personnelArchive.Hobby;
            txtResume.Text = m_personnelArchive.ResumeID.ToString();
            txtJobNature.Text = m_personnelArchive.JobNature;
            txtGraduationYear.Text = m_personnelArchive.GraduationYear.ToString();
            cmbLengthOfSchooling.Text = m_personnelArchive.LengthOfSchooling;
            cmbMaritalStatus.Text = m_personnelArchive.MaritalStatus;
            cbCore.Checked = Convert.ToBoolean(m_personnelArchive.IsCore);

            if (m_personnelArchive.JoinDate.ToString() != "")
            {
                dtpJoinDate.Value = Convert.ToDateTime(m_personnelArchive.JoinDate);
            }

            if (m_personnelArchive.DimissionDate.ToString() != "")
            {
                dtpDimissionDate.Checked = true;
                dtpDimissionDate.Value = Convert.ToDateTime(m_personnelArchive.DimissionDate);
            }
            else
            {
                dtpDimissionDate.Checked = false;
            }

            if (m_personnelArchive.BecomeRegularEmployeeDate.ToString() != "")
            {
                dtpBecomeDate.Value = Convert.ToDateTime(m_personnelArchive.BecomeRegularEmployeeDate);
                dtpBecomeDate.Checked = true;
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
                m_picbyte = m_personnelArchive.Annex == null ? null : m_personnelArchive.Annex.ToArray();
                m_pathName = m_personnelArchive.AnnexName;
                lblAnnexName.Text = m_pathName;
            }
            else
            {
                llbLoadAnnex.Visible = false;
                lblAnnexName.Visible = false;
            }

            txtRemark.Text = m_personnelArchive.Remark;
            txtRelation.Text = m_personnelArchive.Relation;
            txtRelationName.Text = m_personnelArchive.RelationName;
            cbIsRelation.Checked = Convert.ToBoolean(m_personnelArchive.IsRelation);

            txtCard.ReadOnly = true;
            txtWorkID.ReadOnly = true;

            DataTable dt = m_personnerServer.GetArchiveList(txtWorkID.Text);

            if (dt != null && dt.Rows.Count > 0)
            {
                txtRewardPunish.Text = dt.Rows[0]["RewardPunish"].ToString();
                txtRegularization.Text = dt.Rows[0]["Regularization"].ToString();
                txtPerformance.Text = dt.Rows[0]["Performance"].ToString();
                txtDimission.Text = dt.Rows[0]["Dimission"].ToString();
                txtDimissionView.Text = dt.Rows[0]["DimissionView"].ToString();
                txtMedicalHistory.Text = dt.Rows[0]["MedicalHistory"].ToString();
                cbMedicalHistory.Checked = Convert.ToBoolean(dt.Rows[0]["IsMedicalHistory"]);
                txtInMedicalHistory.Text = dt.Rows[0]["InMedicalHistory"].ToString();
            }

            //通过储备人才编号获得家庭、教育、工作经验
            if (txtResume.Text.Trim() != "")
            {
                dt = m_trainEmployeServer.GetWorkHistory(Convert.ToInt32(txtResume.Text));

                if (dt != null && dt.Rows.Count > 0)
                {
                    txtWorkHistory.Text = "";

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        txtWorkHistory.Text += dt.Rows[i]["工作开始时间"].ToString() + "--" + dt.Rows[i]["截止时间"].ToString() + " " +
                             dt.Rows[i]["公司名称"].ToString() + " " + dt.Rows[i]["工作岗位"].ToString() + " " + dt.Rows[i]["月酬"].ToString() + "\r\n";
                    }
                }

                dt = m_trainEmployeServer.GetEducatedHistory(Convert.ToInt32(txtResume.Text));

                if (dt != null && dt.Rows.Count > 0)
                {
                    txtEducatedHistory.Text = "";

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        txtEducatedHistory.Text += dt.Rows[i]["学习开始时间"].ToString() + "--" + dt.Rows[i]["学习截止时间"].ToString() + " " +
                             dt.Rows[i]["学校"].ToString() + " " + dt.Rows[i]["专业"].ToString() + " " + dt.Rows[i]["学历"].ToString() + "\r\n";
                    }
                }

                dt = m_trainEmployeServer.GetFamilyMember(Convert.ToInt32(txtResume.Text));

                if (dt != null && dt.Rows.Count > 0)
                {
                    txtFamilyMember.Text = "";

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        txtFamilyMember.Text += dt.Rows[i]["称谓"].ToString() + " " + dt.Rows[i]["姓名"].ToString()
                            + " " + dt.Rows[i]["单位"].ToString() + "\r\n";
                    }
                }
            }

            if (txtWorkHistory.Text.Trim() == "" && txtEducatedHistory.Text.Trim() == "" && txtFamilyMember.Text.Trim() == "")
            {
                HR_Resume resume = m_resumeServer.GetResumelInfo(txtCard.Text);

                if (resume != null)
                {
                    txtWorkHistory.Text = resume.WorkHistory;
                    txtEducatedHistory.Text = resume.EducatedHistory;
                    txtFamilyMember.Text = resume.FamilyMember;
                }
            }

            //获得岗位调动的记录
            dt = m_PostChangeServer.GetPostChangeByWorkID(txtWorkID.Text, out m_error);

            if (dt != null && dt.Rows.Count > 0)
            {
                txtPostChange.Text = "";

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    txtPostChange.Text += dt.Rows[i]["申请日期"].ToString() + " 由 " + dt.Rows[i]["原部门"].ToString()
                        + " 的 " + dt.Rows[i]["原工作岗位"].ToString() + " 调入到 " + dt.Rows[i]["申请部门"].ToString()
                        + " 的 " + dt.Rows[i]["申请岗位"].ToString() + " 调动原因：" + dt.Rows[i]["调动原因"].ToString() + "\r\n";
                }
            }

            //获得合同签订的记录
            dt = m_laborServer.GetPersonnelContarctByWorkID(txtWorkID.Text);

            if (dt != null && dt.Rows.Count > 0)
            {
                txtContract.Text = "";

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    txtContract.Text += dt.Rows[i]["合同起始时间"].ToString() + " -- " + dt.Rows[i]["合同终止时间"].ToString()
                        + "  " + dt.Rows[i]["合同模板"].ToString() + "  " + dt.Rows[i]["合同状态"].ToString() + "\r\n";
                }
            }
        }

        /// <summary>
        /// 检查控件
        /// </summary>
        /// <returns>没问题返回True，有问题返回False</returns>
        bool CheckControl()
        {
            //if (txtWorkID.Text.Length != 4)
            //{
            //    MessageDialog.ShowPromptMessage("请重新输入员工编号！");
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

            if (cmbStatus.Text == "离职" && !dtpDimissionDate.Checked)
            {
                MessageDialog.ShowPromptMessage("请输入员工离职日期！");
                return false;
            }

            if (cmbEducatedDegree.SelectedIndex == -1)
            {
                MessageDialog.ShowPromptMessage("请选择文化程度！");
                return false;
            }

            if (cmbLengthOfSchooling.SelectedIndex == -1)
            {
                MessageDialog.ShowPromptMessage("请选择学制！");
                return false;
            }

            if (dtpDimissionDate.Checked && txtDimission.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请录入离职原因！");
                return false;
            }

            if (txtMobilePhone.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请输入员工手机号码！");
                return false;
            }
            else if (txtMobilePhone.Text.Trim().Length != 11)
            {
                MessageDialog.ShowPromptMessage("员工手机号码不正确，请重新输入！");
                return false;
            }

            if (cbIsRelation.Checked && txtRelationName.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请录入在司相关证明！");
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
            personnel.EducatedDegree = cmbEducatedDegree.Text;
            personnel.EducatedMajor = txtEducatedMajor.Text;
            personnel.FamilyAddress = txtFamilyAddress.Text;
            personnel.Phone = txtPhone.Text;
            personnel.Speciality = txtSpeciality.Text;
            personnel.MobilePhone = txtMobilePhone.Text;
            personnel.Hobby = txtHobby.Text;
            personnel.QQ = txtQQ.Text;
            personnel.Email = txtEmail.Text;
            personnel.JobNature = txtJobNature.Text;
            personnel.LengthOfSchooling = cmbLengthOfSchooling.Text;
            personnel.MaritalStatus = cmbMaritalStatus.Text;
            personnel.IsRelation = cbIsRelation.Checked;
            personnel.Relation = txtRelation.Text;
            personnel.RelationName = txtRelationName.Text;

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
                personnel.PersonnelStatus = m_personnerServer.GetStatusByName("离职");
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

            if (m_picbyte != null)
            {
                personnel.Annex = m_picbyte;
                personnel.AnnexName = m_pathName;
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

        /// <summary>
        /// 获得人员档案中奖罚等信息的数据集
        /// </summary>
        /// <returns>返回人员档案子表集合</returns>
        HR_PersonnelArchiveList GetPersonnelListData()
        {
            HR_PersonnelArchiveList list = new HR_PersonnelArchiveList();

            list.WorkID = txtWorkID.Text;
            list.Dimission = txtDimission.Text;
            list.DimissionView = txtDimissionView.Text;
            list.InMedicalHistory = txtInMedicalHistory.Text;
            list.IsMedicalHistory = cbMedicalHistory.Checked;
            list.MedicalHistory = txtMedicalHistory.Text;
            list.Performance = txtPerformance.Text;
            list.Regularization = txtRegularization.Text;
            list.RewardPunish = txtRewardPunish.Text;

            return list;
        }

        private void 打印toolStripButton1_Click(object sender, EventArgs e)
        {
            IBillReportInfo report = new 报表_人员档案(txtWorkID.Text.Trim());

            PrintReportBill print = new PrintReportBill(21.8, 9.31, report);
            (report as 报表_人员档案).ShowDialog();
        }

        private void 员工档案明细_Load(object sender, EventArgs e)
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

            if (m_departmentServer.GetAllDeptInfo(out m_findDepartment, out m_error))
            {
                foreach (var item in m_findDepartment)
                {
                    cmbDept.Items.Add(item.部门名称);
                }
            }

            if (m_personnelChange != null)
            {
                AuthorityControl(m_authFlag);
                BindControl();
                FaceAuthoritySetting.SetVisibly(this.Controls, m_queryResult.HideFields);
            }
            else
            {
                修改toolStripButton1.Visible = false;

                txtCard.ReadOnly = false;
                txtName.ReadOnly = false;
                txtWorkID.ReadOnly = false;
                dtpBirthday.Value = ServerTime.Time;
                dtpJoinDate.Value = ServerTime.Time;
                dtpBecomeDate.Value = ServerTime.Time;
                dtpTakeJobDate.Value = ServerTime.Time;
                dtpDimissionDate.Value = ServerTime.Time;
            }

            toolStrip2.Visible = true;
        }

        private void 修改toolStripButton1_Click(object sender, EventArgs e)
        {
            if (!CheckControl())
            {
                return;
            }

            if (!m_personnerServer.UpdatePersonnelArchive(m_personnelChange, GetPersonnelArchiveData(), GetPersonnelListData(), out m_error))
            {
                MessageDialog.ShowPromptMessage(m_error);
                return;
            }

            m_updateFlag = true;
            this.Close();
        }

        private void 添加toolStripButton1_Click(object sender, EventArgs e)
        {
            if (!CheckControl())
            {
                return;
            }

            if (!m_personnerServer.AddPersonnelArchive(GetPersonnelArchiveData(),GetPersonnelListData(), out m_error))
            {
                MessageDialog.ShowPromptMessage(m_error);
                return;
            }

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
            userInfo.Handset = txtMobilePhone.Text.Trim();

            if (m_userManager.AddUser(userInfo))
            {
                if (!m_roleManager.AddUserInRole(roleCode, txtWorkID.Text))
                {
                    MessageDialog.ShowPromptMessage("员工信档案添加成功，角色分配失败！");
                }

                Flow_WarningNotice warning = new Flow_WarningNotice();

                warning.标题 = "合同等待新签";
                warning.发送方 = "系统";
                warning.发送时间 = ServerTime.Time;
                warning.附加信息1 = "员工合同管理";
                warning.附加信息2 = txtWorkID.Text;
                warning.附加信息3 = "0";
                warning.附加信息4 = "";
                warning.附加信息5 = "";
                warning.附加信息6 = "";
                warning.附加信息7 = "";
                warning.附加信息8 = "";
                warning.接收方 = "人力资源部办公室文员";
                warning.接收方类型 = "角色";
                warning.来源 = "人力资源管理系统";
                warning.内容 = "【"+txtName.Text+"】员工【合同类】等待新签！";
                warning.优先级 = "高";
                warning.状态 = "未读";

                m_warningNotice.SendWarningNotice(warning);
            }

            m_updateFlag = true;
            this.Close();
        }

        private void 新建toolStripButton1_Click(object sender, EventArgs e)
        {
            ClearControl();

            txtCard.ReadOnly = false;
            txtName.ReadOnly = false;
            txtWorkID.ReadOnly = false;
        }

        private void txtResume_DoubleClick(object sender, EventArgs e)
        {
            FormPersonnelResume frm = new FormPersonnelResume(txtResume, "编号");
            frm.ShowDialog();

            HR_Resume resumeInfo = m_resumeServer.GetResumelInfo(Convert.ToInt32( frm.UserCode));

            if (resumeInfo != null)
            {
                txtName.Text = resumeInfo.Name;
                txtNationality.Text = resumeInfo.Nationality;
                txtParty.Text = resumeInfo.Party;
                txtFamilyAddress.Text = resumeInfo.FamilyAddress;
                txtCollege.Text = resumeInfo.College;
                cmbEducatedDegree.Text = resumeInfo.EducatedDegree;
                txtEducatedMajor.Text = resumeInfo.EducatedMajor;
                txtCard.Text = resumeInfo.ID_Card;
                txtBirthplace.Text = resumeInfo.Birthplace;
                txtRace.Text = resumeInfo.Race;
                cmbSex.Text = resumeInfo.Sex;
                dtpBirthday.Value = Convert.ToDateTime(resumeInfo.Birthday);
                cmbMaritalStatus.Text = resumeInfo.MaritalStatus;
                txtMobilePhone.Text = resumeInfo.Phone;
                txtResume.Text = frm.UserCode;
                txtPhone.Text = resumeInfo.EmergencyPhone;

                txtWorkHistory.Text = resumeInfo.WorkHistory;
                txtEducatedHistory.Text = resumeInfo.EducatedHistory;
                txtFamilyMember.Text = resumeInfo.FamilyMember;

                llbLoadAnnex.Visible = false;
                lblAnnexName.Visible = false;
            }
            else
            {
                MessageDialog.ShowPromptMessage("简历信息有误，请手动输入");
            }

            #region 人才库
            //HR_TrainEmploye trainEmploye = m_trainEmployeServer.GetInfoByID(Convert.ToInt32(frm.UserCode), out m_error);

            //if (trainEmploye != null)
            //{
            //    txtName.Text = trainEmploye.Name;
            //    txtNationality.Text = trainEmploye.Nationality;
            //    txtParty.Text = trainEmploye.Party;
            //    txtFamilyAddress.Text = trainEmploye.Address;
            //    txtCollege.Text = trainEmploye.College;
            //    cmbEducatedDegree.Text = trainEmploye.EducatedDegree;
            //    txtEducatedMajor.Text = trainEmploye.EducatedMajor;
            //    txtCard.Text = trainEmploye.ID_Card;
            //    txtBirthplace.Text = trainEmploye.Birthplace;
            //    txtRace.Text = trainEmploye.Race;
            //    cmbSex.Text = trainEmploye.Sex;
            //    dtpBirthday.Value = Convert.ToDateTime(trainEmploye.Birthday);
            //    cmbMaritalStatus.Text = trainEmploye.MaritalStatus;
            //    txtMobilePhone.Text = trainEmploye.Phone;
            //    txtResume.Text = frm.UserCode;
            //    txtPhone.Text = trainEmploye.EmergencyPhone;

            //    DataTable dt = m_trainEmployeServer.GetWorkHistory(Convert.ToInt32(frm.UserCode));

            //    if (dt != null && dt.Rows.Count > 0)
            //    {
            //        txtWorkHistory.Text = "";

            //        for (int i = 0; i < dt.Rows.Count; i++)
            //        {
            //            txtWorkHistory.Text += dt.Rows[i]["工作开始时间"].ToString() + "--" + dt.Rows[i]["截止时间"].ToString() + " " +
            //                 dt.Rows[i]["公司名称"].ToString() + " " + dt.Rows[i]["工作岗位"].ToString() + " " + dt.Rows[i]["月酬"].ToString() + "\r\n";
            //        }
            //    }

            //    dt = m_trainEmployeServer.GetEducatedHistory(Convert.ToInt32(frm.UserCode));

            //    if (dt != null && dt.Rows.Count > 0)
            //    {
            //        txtEducatedHistory.Text = "";

            //        for (int i = 0; i < dt.Rows.Count; i++)
            //        {
            //            txtEducatedHistory.Text += dt.Rows[i]["学习开始时间"].ToString() + "--" + dt.Rows[i]["学习截止时间"].ToString() + " " +
            //                 dt.Rows[i]["学校"].ToString() + " " + dt.Rows[i]["专业"].ToString() + " " + dt.Rows[i]["学历"].ToString() + "\r\n";
            //        }
            //    }

            //    dt = m_trainEmployeServer.GetFamilyMember(Convert.ToInt32(frm.UserCode));

            //    if (dt != null && dt.Rows.Count > 0)
            //    {
            //        txtFamilyMember.Text = "";

            //        for (int i = 0; i < dt.Rows.Count; i++)
            //        {
            //            txtFamilyMember.Text += dt.Rows[i]["称谓"].ToString() + " " + dt.Rows[i]["姓名"].ToString()
            //                + " " + dt.Rows[i]["单位"].ToString() + "\r\n";
            //        }
            //    }

            //    llbLoadAnnex.Visible = false;
            //    lblAnnexName.Visible = false;
            //}
            //else
            //{
            //    MessageDialog.ShowPromptMessage("简历信息有误，请手动输入");
            //}
            #endregion
        }

        private void dtpBirthday_ValueChanged(object sender, EventArgs e)
        {
            txtNum.Text = (ServerTime.Time.Year - dtpBirthday.Value.Year).ToString();
        }

        private void cbIsRelation_CheckedChanged(object sender, EventArgs e)
        {
            if (cbIsRelation.Checked)
            {
                lblRelation.Visible = true;
                lblRelationName.Visible = true;
                txtRelation.Visible = true;
                txtRelationName.Visible = true;
            }
            else
            {
                lblRelation.Visible = false;
                lblRelationName.Visible = false;
                txtRelation.Visible = false;
                txtRelationName.Visible = false;
            }
        }

        private void cmbDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbWorkPost.Items.Clear();

            DataTable postDt = m_PostServer.GetOperatingPost(cmbDept.Text);

            for (int i = 0; i < postDt.Rows.Count; i++)
            {
                cmbWorkPost.Items.Add(postDt.Rows[i]["岗位名称"].ToString());
            }
        }
    }
}
