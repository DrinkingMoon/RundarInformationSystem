using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using ServerModule;
using GlobalObject;
using Service_Peripheral_HR;
using UniversalControlLibrary;

namespace Form_Peripheral_HR
{
    public partial class 部门调动申请明细 : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_error;

        /// <summary>
        /// 申请编号
        /// </summary>
        string m_billNo;

        /// <summary>
        /// 操作权限
        /// </summary>
        PlatformManagement.AuthorityFlag m_authorityFlag;

        /// <summary>
        /// 可供查找的所有字段
        /// </summary>
        //string[] m_findField = null;

        /// <summary>
        /// 查询结果
        /// </summary>
        //IQueryResult m_queryResult;

        /// <summary>
        /// 最高部门
        /// </summary>
        //string m_highDept;

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 岗位管理服务类
        /// </summary>
        IOperatingPostServer m_PostServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IOperatingPostServer>();

        /// <summary>
        /// 部门信息服务组件
        /// </summary>
        IOrganizationServer m_departmentServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IOrganizationServer>();

        /// <summary>
        /// 岗位调动服务类
        /// </summary>
        IPostChangeServer m_PostChangeServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IPostChangeServer>();

        /// <summary>
        /// 人员档案管理类
        /// </summary>
        IPersonnelArchiveServer m_personnerServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IPersonnelArchiveServer>();

        public 部门调动申请明细(AuthorityFlag authority, string billNo)
        {
            InitializeComponent();

            m_billMessageServer.BillType = "部门调动申请单";
            m_authorityFlag = authority;
            m_billNo = billNo;

            DataTable postDt = m_PostServer.GetOperatingPost(null);

            for (int i = 0; i < postDt.Rows.Count; i++)
            {
                cmbNewWorkPost.Items.Add(postDt.Rows[i]["岗位名称"].ToString());
            }

            IQueryable<View_HR_Dept> m_findDepartment;

            if (m_departmentServer.GetAllDeptInfo(out m_findDepartment, out m_error))
            {
                foreach (var item in m_findDepartment)
                {
                    cmbNewDept.Items.Add(item.部门名称);
                }
            }

            if (m_billNo != "0")
            {
                BindControl();
            }
            else
            {
                ClearControl();
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

        private void 部门调动申请明细_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authorityFlag);
            toolStrip1.Visible = true;
        }

        private void 部门调动申请明细_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        /// <summary>
        /// 给各控件赋值
        /// </summary>
        void BindControl()
        {
            HR_PostChange postChange = m_PostChangeServer.GetPostChangeByBillNo(Convert.ToInt32(m_billNo), out m_error);

            if (postChange != null)
            {
                lblStatus.Text = postChange.BillStatus;
                txtApplicant.Text = UniversalFunction.GetPersonnelName(postChange.WorkID);
                txtApplicant.Tag = postChange.WorkID;
                txtChangeReason.Text = postChange.ChangeReason;
                txtDormPeople.Text = UniversalFunction.GetPersonnelName(postChange.DormPeople);
                txtFilesPeople.Text = UniversalFunction.GetPersonnelName(postChange.FilesPeople);
                txtGeneralManager.Text = UniversalFunction.GetPersonnelName(postChange.GeneralManager);
                txtGMOpinion.Text = postChange.GM_Opinion;
                txtHRDirector.Text = UniversalFunction.GetPersonnelName(postChange.HR_Director);
                txtHROpinion.Text = postChange.HR_Opinion;
                txtITPeople.Text = UniversalFunction.GetPersonnelName(postChange.ITPeople);
                txtNewDeptDirector.Text = UniversalFunction.GetPersonnelName(postChange.NewDeptDirector);
                txtNewDeptOpinion.Text = postChange.NewDeptOpinion;
                txtNewLearder.Text = UniversalFunction.GetPersonnelName(postChange.NewLearder);
                txtNewLearderOpinion.Text = postChange.NewLearderOpinion;
                txtOldDept.Text = UniversalFunction.GetDeptName(postChange.DeptCode);
                txtOldDeptDirector.Text = UniversalFunction.GetPersonnelName(postChange.OldDeptDirector);
                txtOldDeptOpinion.Text = postChange.OldDeptOpinion;
                txtOldLearder.Text = UniversalFunction.GetPersonnelName(postChange.OldLearder);
                txtOldLearderOpinion.Text = postChange.OldLearderOpinion;
                txtOldWorkPost.Text = m_PostServer.GetOperatingPostByPostCode(postChange.PostID);
                txtTurnOverPeople.Text = UniversalFunction.GetPersonnelName(postChange.TurnOverPeople);

                cmbNewDept.Text = UniversalFunction.GetDeptName(postChange.NewDeptCode);
                cmbNewWorkPost.Text = m_PostServer.GetOperatingPostByPostCode(postChange.NewPostID);

                cbGMAuthorize.Checked = Convert.ToBoolean(postChange.GM_Authorize);
                cbHRAuthorize.Checked = Convert.ToBoolean(postChange.HR_Authorize);
                cbIsDorm.Checked = Convert.ToBoolean(postChange.IsDorm);
                cbIsIT.Checked = Convert.ToBoolean(postChange.IsIT);
                cbIsPersonnelFiles.Checked = Convert.ToBoolean(postChange.IsPersonnelFiles);
                cbIsWorkTurnOver.Checked = Convert.ToBoolean(postChange.IsWorkTurnOver);
                cbNewDeptAuthorize.Checked = Convert.ToBoolean(postChange.NewDeptAuthorize);
                cbNewlearderAuthorize.Checked = Convert.ToBoolean(postChange.NewlearderAuthorize);
                cbOldDeptAuthorize.Checked = Convert.ToBoolean(postChange.OldDeptAuthorize);
                cbOldLearderAuthorize.Checked = Convert.ToBoolean(postChange.OldLearderAuthorize);

                if (postChange.DormDate != null)
                {
                    dtpDate.Value = postChange.Date;
                }

                if (postChange.DormDate != null)
                {
                    dtpDormDate.Value = Convert.ToDateTime(postChange.DormDate);
                }

                if (postChange.EmployedDate != null)
                {
                    dtpEmployedDate.Value = Convert.ToDateTime(postChange.EmployedDate);
                }

                if (postChange.FilesDate != null)
                {
                    dtpFilesDate.Value = Convert.ToDateTime(postChange.FilesDate);
                }

                if (postChange.GM_SignatureDate != null)
                {
                    dtpGMDate.Value = Convert.ToDateTime(postChange.GM_SignatureDate);
                }

                if (postChange.HR_SignatureDate != null)
                {
                    dtpHRSignatureDate.Value = Convert.ToDateTime(postChange.HR_SignatureDate);
                }

                if (postChange.ITDate != null)
                {
                    dtpITDate.Value = Convert.ToDateTime(postChange.ITDate);
                }

                if (postChange.NewDeptSignatureDate != null)
                {
                    dtpNewDeptSignatureDate.Value = Convert.ToDateTime(postChange.NewDeptSignatureDate);
                }

                if (postChange.NewLearderDate != null)
                {
                    dtpNewLearderDate.Value = Convert.ToDateTime(postChange.NewLearderDate);
                }

                if (postChange.OldDeptSignatureDate != null)
                {
                    dtpOldDeptSignatureDate.Value = Convert.ToDateTime(postChange.OldDeptSignatureDate);
                }

                if (postChange.OldLearderDate != null)
                {
                    dtpOldLearderDate.Value = Convert.ToDateTime(postChange.OldLearderDate);
                }

                if (postChange.TurnOverDate != null)
                {
                    dtpTurnOverDate.Value = Convert.ToDateTime(postChange.TurnOverDate);
                }
            }
        }

        /// <summary>
        /// 清空控件
        /// </summary>
        void ClearControl()
        {
            txtApplicant.Text = BasicInfo.LoginName;
            txtApplicant.Tag = BasicInfo.LoginID;
            txtChangeReason.Text = "";
            txtGeneralManager.Text = "";
            txtGMOpinion.Text = "";
            txtHRDirector.Text = "";
            txtHROpinion.Text = "";
            txtNewDeptDirector.Text = "";
            txtNewDeptOpinion.Text = "";
            txtDormPeople.Text = "";
            txtFilesPeople.Text = "";
            txtITPeople.Text = "";
            txtNewLearder.Text = "";
            txtNewLearderOpinion.Text = "";
            txtOldLearder.Text = "";
            txtOldLearderOpinion.Text = "";
            txtTurnOverPeople.Text = "";
            txtOldDept.Text = BasicInfo.DeptName;
            txtOldDept.Tag = BasicInfo.DeptCode;
            txtOldDeptDirector.Text = "";
            txtOldDeptOpinion.Text = "";

            DataTable postDt = m_PostServer.GetOperatingPost(null);

            for (int i = 0; i < postDt.Rows.Count; i++)
            {
                cmbNewWorkPost.Items.Add(postDt.Rows[i]["岗位名称"].ToString());
            }

            IQueryable<View_HR_Dept> m_findDepartment;

            if (m_departmentServer.GetAllDeptInfo(out m_findDepartment, out m_error))
            {
                foreach (var item in m_findDepartment)
                {
                    cmbNewDept.Items.Add(item.部门名称);
                }
            }

            lblStatus.Text = PostChangeStatus.新建单据.ToString();
            
            txtOldWorkPost.Text = m_personnerServer.GetPersonnelArchiveByNameAndCode(BasicInfo.LoginName, BasicInfo.LoginID);

            if (txtOldWorkPost.Text.Trim() != "")
            {
                txtOldWorkPost.Tag = m_PostServer.GetOperatingPostByPostName(txtOldWorkPost.Text.Trim()).岗位编号;
            }

            cmbNewDept.SelectedIndex = -1;
            cmbNewWorkPost.SelectedIndex = -1;

            cbNewDeptAuthorize.Checked = false;
            cbOldDeptAuthorize.Checked = false;
            cbGMAuthorize.Checked = false;
            cbHRAuthorize.Checked = false;
            cbIsDorm.Checked = false;
            cbIsIT.Checked = false;
            cbIsPersonnelFiles.Checked = false;
            cbIsWorkTurnOver.Checked = false;
            cbNewlearderAuthorize.Checked = false;
            cbOldLearderAuthorize.Checked = false;           
        }

        private void 提交toolStripButton1_Click(object sender, EventArgs e)
        {
            if (txtApplicant.Tag.ToString() == BasicInfo.LoginID)
            {
                if (cmbNewDept.SelectedIndex == -1 || cmbNewWorkPost.SelectedIndex == -1)
                {
                    MessageDialog.ShowPromptMessage("请选择调入部门和所申请的岗位！");
                    return;
                }

                if (txtChangeReason.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请选择调动原因！");
                    return;
                }

                m_billNo = m_PostChangeServer.AddPostChange(GetPostChange(), out m_error).ToString();

                if (m_billNo == "0")
                {
                    MessageDialog.ShowPromptMessage(m_error);
                    return;
                }

                m_billMessageServer.DestroyMessage(m_billNo.ToString());
                m_billMessageServer.SendNewFlowMessage(m_billNo.ToString(), string.Format("{0}号部门调动申请，请调出部门负责人审核", m_billNo),
                    BillFlowMessage_ReceivedUserType.角色, m_billMessageServer.GetDeptPrincipalRoleName(txtOldDept.Tag.ToString()).ToList());

                this.Close();
            }
            else
            {
                MessageDialog.ShowPromptMessage("您不是申请人本人，不能进行此操作！");
            }
        }

        /// <summary>
        /// 获取岗位调动数据集
        /// </summary>
        /// <returns>返回岗位调动数据集</returns>
        private HR_PostChange GetPostChange()
        {
            try
            {
                HR_PostChange postChange = new HR_PostChange();

                postChange.BillStatus = PostChangeStatus.等待调出部门负责人审核.ToString();
                postChange.ChangeReason = txtChangeReason.Text;
                postChange.Date = ServerTime.Time;
                postChange.DeptCode = BasicInfo.DeptCode;
                postChange.PostID = Convert.ToInt32(txtOldWorkPost.Tag);
                postChange.EmployedDate = dtpEmployedDate.Value;
                postChange.NewDeptCode = m_departmentServer.GetDeptCode(cmbNewDept.Text);
                postChange.NewPostID = m_PostServer.GetOperatingPostByPostName(cmbNewWorkPost.Text).岗位编号;
                postChange.WorkID = BasicInfo.LoginID;
                postChange.GM_Authorize = false;
                postChange.HR_Authorize = false;
                postChange.NewDeptAuthorize = false;
                postChange.NewlearderAuthorize = false;
                postChange.OldDeptAuthorize = false;
                postChange.OldLearderAuthorize = false;
                postChange.IsDorm = false;
                postChange.IsIT = false;
                postChange.IsPersonnelFiles = false;
                postChange.IsWorkTurnOver = false;

                return postChange;
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
                return null;
            }
        }

        private void 修改toolStripButton6_Click(object sender, EventArgs e)
        {
            if (txtApplicant.Tag.ToString() == BasicInfo.LoginID)
            {
                if (cmbNewDept.SelectedIndex == -1 || cmbNewWorkPost.SelectedIndex == -1)
                {
                    MessageDialog.ShowPromptMessage("请选择调入部门和所申请的岗位！");
                    return;
                }

                if (txtChangeReason.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请选择调动原因！");
                    return;
                }

                bool flag = m_PostChangeServer.UpdatePostChange(GetPostChange(), out m_error);

                if (!flag)
                {
                    MessageDialog.ShowPromptMessage(m_error);
                    return;
                }

                m_billMessageServer.PassFlowMessage(m_billNo.ToString(), string.Format("{0}号部门调动申请，请调出部门负责人审核", m_billNo),
                    BillFlowMessage_ReceivedUserType.角色, m_billMessageServer.GetDeptPrincipalRoleName(txtOldDept.Tag.ToString()).ToList());

                this.Close();
            }
        }

        private void 调出审批toolStripButton2_Click(object sender, EventArgs e)
        {
            if (lblStatus.Text == PostChangeStatus.等待调出部门负责人审核.ToString())
            {
                if (txtOldDeptOpinion.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请填写【调出部门负责人意见】！");
                    return;
                }

                if (!cbOldDeptAuthorize.Checked)
                {
                    if (MessageDialog.ShowEnquiryMessage("您同意【" + txtApplicant.Text + "】调出此岗位吗？") == DialogResult.Yes)
                    {
                        cbOldDeptAuthorize.Checked = true;
                    }
                }

                if (!m_PostChangeServer.UpdateOldDept(Convert.ToInt32(m_billNo), txtOldDeptOpinion.Text.Trim(), cbOldDeptAuthorize.Checked, out m_error))
                {
                    MessageDialog.ShowPromptMessage(m_error);
                    return;
                }
                else
                {
                    m_billMessageServer.PassFlowMessage(m_billNo.ToString(), string.Format("{0}号部门调动申请，请调出部门分管领导审核", m_billNo),
                   BillFlowMessage_ReceivedUserType.角色, m_billMessageServer.GetDeptLeaderRoleName(txtOldDept.Tag.ToString()).ToList());

                    MessageDialog.ShowPromptMessage("审批成功！");
                    this.Close();
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请确认单据状态！");
                return;
            }
        }

        private void 调出分管toolStripButton_Click(object sender, EventArgs e)
        {
            if (lblStatus.Text == PostChangeStatus.等待调出分管领导审核.ToString())
            {
                if (txtOldLearderOpinion.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请填写【调出部门分管领导意见】！");
                    return;
                }

                if (!cbOldLearderAuthorize.Checked)
                {
                    if (MessageDialog.ShowEnquiryMessage("您同意【" + txtApplicant.Text + "】调出此岗位吗？") == DialogResult.Yes)
                    {
                        cbOldLearderAuthorize.Checked = true;
                    }
                }

                if (!m_PostChangeServer.UpdateOldLearder(Convert.ToInt32(m_billNo), txtOldLearderOpinion.Text.Trim(), cbOldLearderAuthorize.Checked, out m_error))
                {
                    MessageDialog.ShowPromptMessage(m_error);
                    return;
                }
                else
                {
                    MessageDialog.ShowPromptMessage("审批成功！");

                    m_billMessageServer.PassFlowMessage(m_billNo.ToString(), string.Format("{0}号部门调动申请，请调入部门负责人审核", m_billNo),
                   BillFlowMessage_ReceivedUserType.角色, m_billMessageServer.GetDeptPrincipalRoleName(cmbNewDept.Text).ToList());

                    this.Close();
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请确认单据状态！");
                return;
            }
        }

        private void 调入审批toolStripButton3_Click(object sender, EventArgs e)
        {
            if (lblStatus.Text == PostChangeStatus.等待调入部门负责人审核.ToString())
            {
                if (txtNewDeptDirector.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请填写【调入部门负责人意见】！");
                    return;
                }

                if (!cbNewDeptAuthorize.Checked)
                {
                    if (MessageDialog.ShowEnquiryMessage("您同意【" + txtApplicant.Text + "】调入此岗位吗？") == DialogResult.Yes)
                    {
                        cbNewDeptAuthorize.Checked = true;
                    }
                }

                if (!m_PostChangeServer.UpdateNewDept(Convert.ToInt32(m_billNo), txtNewDeptDirector.Text.Trim(), cbNewDeptAuthorize.Checked, out m_error))
                {
                    MessageDialog.ShowPromptMessage(m_error);
                    return;
                }
                else
                {
                    MessageDialog.ShowPromptMessage("审批成功！");

                    m_billMessageServer.PassFlowMessage(m_billNo.ToString(), string.Format("{0}号部门调动申请，请调出部门分管领导审核", m_billNo),
                   BillFlowMessage_ReceivedUserType.角色, m_billMessageServer.GetDeptLeaderRoleName(cmbNewDept.Text).ToList());

                    this.Close();
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请确认单据状态！");
                return;
            }
        }

        private void 调入分管toolStripButton_Click(object sender, EventArgs e)
        {
            if (lblStatus.Text == PostChangeStatus.等待调入分管领导审核.ToString())
            {
                if (txtNewLearderOpinion.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请填写【调入分管领导意见】！");
                    return;
                }

                if (!cbNewlearderAuthorize.Checked)
                {
                    if (MessageDialog.ShowEnquiryMessage("您同意【" + txtApplicant.Text + "】调入此岗位吗？") == DialogResult.Yes)
                    {
                        cbNewlearderAuthorize.Checked = true;
                    }
                }

                if (!m_PostChangeServer.UpdateNewLearder(Convert.ToInt32(m_billNo), txtNewLearderOpinion.Text.Trim(), cbNewlearderAuthorize.Checked, out m_error))
                {
                    MessageDialog.ShowPromptMessage(m_error);
                    return;
                }
                else
                {
                    MessageDialog.ShowPromptMessage("审批成功！");

                    m_billMessageServer.PassFlowMessage(m_billNo.ToString(), string.Format("{0}号部门调动申请，请公司办负责人审核", m_billNo),
                   BillFlowMessage_ReceivedUserType.角色, CE_RoleEnum.公司办负责人.ToString());

                    this.Close();
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请确认单据状态！");
                return;
            }
        }

        private void 资源部审批toolStripButton4_Click(object sender, EventArgs e)
        {
            if (lblStatus.Text == PostChangeStatus.等待公司办负责人审核.ToString())
            {
                if (txtHROpinion.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请填写【公司办负责人意见】！");
                    return;
                }

                if (!cbHRAuthorize.Checked)
                {
                    if (MessageDialog.ShowEnquiryMessage("您同意【" + txtApplicant.Text + "】调入此岗位吗？") == DialogResult.Yes)
                    {
                        cbHRAuthorize.Checked = true;
                    }
                }

                if (!m_PostChangeServer.UpdateHRAuthor(Convert.ToInt32(m_billNo), txtHROpinion.Text.Trim(), cbHRAuthorize.Checked, out m_error))
                {
                    MessageDialog.ShowPromptMessage(m_error);
                    return;
                }
                else
                {
                    MessageDialog.ShowPromptMessage("审批成功！");

                    m_billMessageServer.PassFlowMessage(m_billNo.ToString(), string.Format("{0}号部门调动申请，请调出部门负责人审核", m_billNo),
                   BillFlowMessage_ReceivedUserType.角色, CE_RoleEnum.总经理.ToString());

                    this.Close();
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请确认单据状态！");
                return;
            }
        }

        private void 批准toolStripButton5_Click(object sender, EventArgs e)
        {
            if (lblStatus.Text == PostChangeStatus.等待总经理批准.ToString())
            {
                if (txtGMOpinion.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请填写【总经理意见】！");
                    return;
                }

                if (!cbGMAuthorize.Checked)
                {
                    if (MessageDialog.ShowEnquiryMessage("您同意【" + txtApplicant.Text + "】调入此岗位吗？") == DialogResult.Yes)
                    {
                        cbGMAuthorize.Checked = true;
                    }
                }

                if (!m_PostChangeServer.UpdateGMAuthor(Convert.ToInt32(m_billNo), txtGMOpinion.Text.Trim(), cbGMAuthorize.Checked, out m_error))
                {
                    MessageDialog.ShowPromptMessage(m_error);
                    return;
                }
                else
                {
                    MessageDialog.ShowPromptMessage("审批成功！");
                    this.Close();
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请确认单据状态！");
                return;
            }
        }

        private void 移交确认toolStripButton_Click(object sender, EventArgs e)
        {
            int postID = m_PostServer.GetOperatingPostByPostName(cmbNewWorkPost.Text).岗位编号;
            HR_PersonnelArchiveChange personnelChange = GetChangeData();

            if (lblStatus.Text.Trim() == PostChangeStatus.等待原工作移交.ToString())
            {
                if (!m_PostChangeServer.UpdateWorkTurnOver(Convert.ToInt32(m_billNo), cbIsWorkTurnOver.Checked,
                    txtApplicant.Text, txtApplicant.Tag.ToString(), personnelChange, cmbNewDept.Text, postID, out m_error))
                {
                    MessageDialog.ShowPromptMessage(m_error);
                }
                else
                {
                    MessageDialog.ShowPromptMessage("提交成功");
                    this.Close();
                }
            }
            else if (lblStatus.Text.Trim() == PostChangeStatus.等待人事档案调动.ToString())
            {
                if (!m_PostChangeServer.UpdateWorkTurnOver(Convert.ToInt32(m_billNo), cbIsPersonnelFiles.Checked,
                    txtApplicant.Text, txtApplicant.Tag.ToString(), personnelChange, cmbNewDept.Text, postID, out m_error))
                {
                    MessageDialog.ShowPromptMessage(m_error);
                }
                else
                {
                    MessageDialog.ShowPromptMessage("提交成功");
                    this.Close();
                }
            }
            else if (lblStatus.Text.Trim() == PostChangeStatus.等待信息化人员确认.ToString())
            {
                if (!m_PostChangeServer.UpdateWorkTurnOver(Convert.ToInt32(m_billNo), cbIsIT.Checked,
                    txtApplicant.Text, txtApplicant.Tag.ToString(), personnelChange, cmbNewDept.Text, postID, out m_error))
                {
                    MessageDialog.ShowPromptMessage(m_error);
                }
                else
                {
                    MessageDialog.ShowPromptMessage("提交成功");
                    this.Close();
                }
            }
            else if (lblStatus.Text.Trim() == PostChangeStatus.等待固定资产人员确认.ToString())
            {
                if (!m_PostChangeServer.UpdateWorkTurnOver(Convert.ToInt32(m_billNo), cbIsDorm.Checked,
                    txtApplicant.Text, txtApplicant.Tag.ToString(), personnelChange, cmbNewDept.Text, postID, out m_error))
                {
                    MessageDialog.ShowPromptMessage(m_error);
                }
                else
                {
                    MessageDialog.ShowPromptMessage("提交成功");
                    this.Close();
                }
            }
        }

        /// <summary>
        /// 获得人员变更历史
        /// </summary>
        /// <returns>变更历史数据集</returns>
        HR_PersonnelArchiveChange GetChangeData()
        {
            try
            {
                HR_PersonnelArchiveChange personnelChange = new HR_PersonnelArchiveChange();
                View_HR_PersonnelArchive dt = m_personnerServer.GetPersonnelViewInfo(txtApplicant.Tag.ToString());

                if (dt != null)
                {
                    personnelChange.WorkID = dt.员工编号;
                    personnelChange.Name = dt.员工姓名;
                    personnelChange.WorkPost = dt.岗位;
                    personnelChange.JobTitle = dt.外部职称;
                    personnelChange.JobLevel = dt.内部级别;
                    personnelChange.IsCore = dt.是否核心员工;
                    personnelChange.Sex = dt.性别;
                    personnelChange.DeptName = dt.部门;
                    personnelChange.Dept = dt.部门编号;
                    personnelChange.Birthday = dt.出生日期;
                    personnelChange.Nationality = dt.国籍;
                    personnelChange.Race = dt.民族;
                    personnelChange.Birthplace = dt.籍贯;
                    personnelChange.Party = dt.政治面貌;
                    personnelChange.ID_Card = dt.身份证;
                    personnelChange.College = dt.毕业院校;
                    personnelChange.EducatedDegree = dt.文化程度;
                    personnelChange.EducatedMajor = dt.专业;
                    personnelChange.FamilyAddress = dt.家庭地址;
                    //personnelChange.PostCode = dt.邮编;
                    personnelChange.Phone = dt.电话;
                    personnelChange.Speciality = dt.特长;
                    personnelChange.MobilePhone = dt.手机;
                    //personnelChange.TrainingAmount = dt.培训次数;
                    //personnelChange.ChangePostAmount = dt.调动次数;
                    //personnelChange.Bank = dt.开户银行;
                    //personnelChange.BankAccount = dt.银行账号;
                    personnelChange.QQ = dt.QQ号码;
                    personnelChange.Email = dt.电子邮箱;
                    personnelChange.Hobby = dt.爱好;
                    //personnelChange.SocietySecurityNumber = dt.社会保障号;
                    personnelChange.LengthOfSchooling = dt.学制;
                    personnelChange.JobNature = dt.工作性质;
                    personnelChange.BecomeRegularEmployeeDate = dt.转正日期;
                    personnelChange.GraduationYear = dt.毕业年份;

                    if (dt.照片 != null)
                    {
                        personnelChange.Photo = dt.照片;
                    }

                    if (dt.附件 != null)
                    {
                        personnelChange.Annex = dt.附件;
                        personnelChange.AnnexName = dt.附件名;
                    }

                    if (dt.关联编号.ToString() != "")
                    {
                        personnelChange.ResumeID = dt.关联编号;
                    }

                    personnelChange.Remark = dt.备注;
                    personnelChange.ChangerCode = BasicInfo.LoginID;
                    personnelChange.ChangeTime = ServerTime.Time;
                }

                return personnelChange;
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
                return null;
            }
        }

        private void cmbNewDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbNewWorkPost.Items.Clear();

            DataTable postDt = m_PostServer.GetOperatingPost(cmbNewDept.Text);

            for (int i = 0; i < postDt.Rows.Count; i++)
            {
                cmbNewWorkPost.Items.Add(postDt.Rows[i]["岗位名称"].ToString());
            }
        }
    }
}
