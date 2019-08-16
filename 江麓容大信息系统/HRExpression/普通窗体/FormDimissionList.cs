using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Service_Peripheral_HR;
using ServerModule;
using PlatformManagement;
using GlobalObject;
using Expression;
using UniversalControlLibrary;

namespace Form_Peripheral_HR
{
    /// <summary>
    /// 离职申请子界面
    /// </summary>
    public partial class FormDimissionList : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string error;

        /// <summary>
        /// 编号
        /// </summary>
        int billID;

        /// <summary>
        /// 最高部门
        /// </summary>
        string m_highDept;

        /// <summary>
        /// 所有部门
        /// </summary>
        internal int BillID
        {
            get { return billID; }
            set { billID = value; }
        }

        /// <summary>
        /// 员工离职数据集
        /// </summary>
        private HR_DimissionBill m_DimissionLinq;

        public HR_DimissionBill DimissionLinq
        {
            get { return m_DimissionLinq; }
            set { m_DimissionLinq = value; }
        }

        /// <summary>
        /// 人员档案管理类
        /// </summary>
        IPersonnelArchiveServer m_personnerServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IPersonnelArchiveServer>();

        /// <summary>
        /// 员工离职申请服务类
        /// </summary>
        IDimissionServer m_dimiServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IDimissionServer>();

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        public FormDimissionList(AuthorityFlag authFlag, HR_DimissionBill dimission, string deptName, 
            string deptCode, string workPost, string name,int id)
        {
            InitializeComponent();
            m_billMessageServer.BillType = "员工离职申请单";

            AuthorityControl(authFlag);

            m_DimissionLinq = dimission;

            billID = id;
            txtDept.Text = deptName;
            txtDept.Tag = deptCode;
            txtWorkPost.Text = workPost;
            txtApplicant.Text = name;

            txtReason.ReadOnly = true;
            txtReason.BackColor = Color.White;
            BindControl();
            menuStrip1.Visible = true;
        }

        public FormDimissionList(AuthorityFlag authFlag)
        {
            InitializeComponent();

            m_billMessageServer.BillType = "员工离职申请单";
            AuthorityControl(authFlag);

            txtApplicant.Text = BasicInfo.LoginName;
            txtApplicant.Tag = BasicInfo.LoginID;
            txtDept.Text = BasicInfo.DeptName;
            txtDept.Tag = BasicInfo.DeptCode;
            txtWorkPost.Text = m_personnerServer.GetPersonnelArchiveByNameAndCode(BasicInfo.LoginName, BasicInfo.LoginID);
            dtpDate.Value = ServerTime.Time;
            lblStatus.Text = DimissionBillStatus.新建单据.ToString();
            menuStrip1.Visible = true;

            DataTable dt = m_personnerServer.GetHighestDept(txtApplicant.Tag.ToString());

            if (dt != null && dt.Rows.Count > 0)
            {
                m_highDept = dt.Rows[0]["deptCode"].ToString();
            }
        }

        /// <summary>
        /// 权限控制
        /// </summary>
        /// <param name="authorityFlag">权限标志</param>
        void AuthorityControl(PlatformManagement.AuthorityFlag authorityFlag)
        {
            FaceAuthoritySetting.SetVisibly(menuStrip1, authorityFlag);
        }

        /// <summary>
        /// 控件赋值
        /// </summary>
        void BindControl()
        {
            txtApplicant.Tag = m_DimissionLinq.WorkID;
            txtDeptDirector.Text = UniversalFunction.GetPersonnelName(m_DimissionLinq.DeptSignature);
            txtDeptOpinion.Text = m_DimissionLinq.DeptOpinion;
            txtGMOpinion.Text = m_DimissionLinq.GM_Opinion;
            txtGMSignature.Text = UniversalFunction.GetPersonnelName(m_DimissionLinq.GM_Signature);
            txtHROpinion.Text = m_DimissionLinq.HR_Opinion;
            txtHRSignature.Text = UniversalFunction.GetPersonnelName(m_DimissionLinq.HR_Signature);
            txtLeaderOpinion.Text = m_DimissionLinq.LeaderOpinion;
            txtLeaderSignature.Text = UniversalFunction.GetPersonnelName(m_DimissionLinq.LeaderSignature);
            txtReason.Text = m_DimissionLinq.Reason;
            cbDeptAuthorize.Checked = Convert.ToBoolean(m_DimissionLinq.DeptAuthorize);
            cbGMAuthorize.Checked = Convert.ToBoolean(m_DimissionLinq.GM_Authorize);
            cbLeaderAuthorize.Checked = Convert.ToBoolean(m_DimissionLinq.LeaderAuthorize);
            dtpDate.Value = m_DimissionLinq.Date;
            dtpDeptSignatureDate.Value = Convert.ToDateTime(m_DimissionLinq.DeptSignatureDate);
            dtpGMSignatureDate.Value = Convert.ToDateTime(m_DimissionLinq.GM_SignatureDate);
            dtpHRDate.Value = Convert.ToDateTime(m_DimissionLinq.HR_SignatureDate);
            dtpLeaderSignatureDate.Value = Convert.ToDateTime(m_DimissionLinq.LeaderSignatureDate);
            lblStatus.Text = m_DimissionLinq.BillStatus;

            if (m_DimissionLinq.AllowDimissionDate != null)
            {
                dtpAllowDate.Value = Convert.ToDateTime(m_DimissionLinq.AllowDimissionDate);
            }
            else 
            {
                dtpAllowDate.Value = ServerTime.Time;
            }

            DataTable dt = m_personnerServer.GetHighestDept(txtApplicant.Tag.ToString());

            if (dt != null && dt.Rows.Count > 0)
            {
                m_highDept = dt.Rows[0]["deptCode"].ToString();
            }
        }

        /// <summary>
        /// 清空窗体控件
        /// </summary>
        void ClearControl()
        {
            txtDeptDirector.Text = "";
            txtDeptOpinion.Text = "";
            txtGMOpinion.Text = "";
            txtGMSignature.Text = "";
            txtHROpinion.Text = "";
            txtHRSignature.Text = "";
            txtLeaderOpinion.Text = "";
            txtLeaderSignature.Text = "";
            txtReason.Text = "";
            cbDeptAuthorize.Checked = false;
            cbGMAuthorize.Checked = false;
            cbLeaderAuthorize.Checked = false;
            dtpAllowDate.Value = ServerTime.Time;
            dtpDate.Value = ServerTime.Time;
            dtpDeptSignatureDate.Value = ServerTime.Time;
            dtpGMSignatureDate.Value = ServerTime.Time;
            dtpHRDate.Value = ServerTime.Time;
            dtpLeaderSignatureDate.Value = ServerTime.Time;
            lblStatus.Text = DimissionBillStatus.新建单据.ToString();
            txtApplicant.Text = BasicInfo.LoginName;
            txtApplicant.Tag = BasicInfo.LoginID;
            txtDept.Text = BasicInfo.DeptName;
            txtDept.Tag = BasicInfo.DeptCode;
            txtWorkPost.Text = m_personnerServer.GetPersonnelArchiveByNameAndCode(BasicInfo.LoginName, BasicInfo.LoginID);
        }

        private void 新建申请ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearControl();

            txtReason.ReadOnly = false;
        }

        private void 提交申请ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lblStatus.Text.Trim() == DimissionBillStatus.新建单据.ToString())
            {
                if (txtReason.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请填写离职原因！");
                    return;
                }

                HR_DimissionBill dimission = new HR_DimissionBill();

                dimission.Date = dtpDate.Value;
                dimission.Dept = txtDept.Tag.ToString();
                dimission.WorkPost = txtWorkPost.Text;
                dimission.WorkID = txtApplicant.Tag.ToString();
                dimission.Reason = txtReason.Text;
                dimission.DeptAuthorize = false;
                dimission.LeaderAuthorize = false;
                dimission.GM_Authorize = false;
                //dimission.AllowDimissionDate = ServerTime.Time.AddDays(1);
                dimission.DeptSignatureDate = ServerTime.Time;
                dimission.HR_SignatureDate = ServerTime.Time;
                dimission.LeaderSignatureDate = ServerTime.Time;
                dimission.GM_SignatureDate = ServerTime.Time;
                dimission.BillStatus = DimissionBillStatus.等待部门负责人审核.ToString();

                billID = Convert.ToInt32(m_dimiServer.AddAndUpdateDimission(dimission, "提交申请", out error));

                if (billID == 0)
                {
                    MessageDialog.ShowPromptMessage(error);
                    return;
                }

                //m_billMessageServer.SendNewFlowMessage(billID.ToString(), string.Format("{0}号离职申请，请主管审核",billID.ToString()),
                //    BillFlowMessage_ReceivedUserType.角色,m_billMessageServer.GetDeptPrincipalRoleName(m_highDept)[0]);

                m_billMessageServer.SendNewFlowMessage(billID.ToString(), string.Format("{0}号离职申请，请主管审核", billID),
                   BillFlowMessage_ReceivedUserType.角色, m_billMessageServer.GetDeptPrincipalRoleName(m_personnerServer.GetPersonnelViewInfo(txtApplicant.Tag.ToString()).部门编号).ToList());
            }
            else
            {
                MessageDialog.ShowPromptMessage("请确认单据状态！");
                return;
            }

            this.Close();
        }

        private void 审批ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lblStatus.Text.Trim() == DimissionBillStatus.等待部门负责人审核.ToString())
            {
                if (txtDeptOpinion.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请填写部门负责人意见！");
                    return;
                }

                HR_DimissionBill dimission = new HR_DimissionBill();

                dimission.DeptOpinion = txtDeptOpinion.Text;
                dimission.DeptSignature = BasicInfo.LoginID;
                dimission.DeptSignatureDate = ServerTime.Time;
                dimission.AllowDimissionDate = dtpAllowDate.Value;
                dimission.WorkID = txtApplicant.Tag.ToString();
                dimission.Date = dtpDate.Value;
                dimission.Reason = txtReason.Text;

                if (!cbDeptAuthorize.Checked)
                {
                    if (MessageBox.Show("您是否同意 " + txtApplicant.Text.Trim()
                                   + " 的离职申请?", "消息", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cbDeptAuthorize.Checked = true;
                    }
                    else
                    {
                        cbDeptAuthorize.Checked = false;
                    }
                }

                dimission.DeptAuthorize = cbDeptAuthorize.Checked;
                billID = Convert.ToInt32(m_dimiServer.AddAndUpdateDimission(dimission, "部门负责人审批", out error));

                if (billID == 0)
                {
                    MessageDialog.ShowPromptMessage(error);
                    return;
                }

                if (cbDeptAuthorize.Checked)
                {
                    string msg = string.Format("{0} 号离职申请已由主管审核，请人力资源部主管审批", billID.ToString());
                    m_billMessageServer.PassFlowMessage(billID.ToString(), msg, CE_RoleEnum.人力资源主管.ToString(), true);
                }
                else
                {
                    string msg = string.Format("{0} 号离职申请部门负责人不同意离职申请,单据完成", billID.ToString());
                    List<string> listUser = new List<string>();

                    listUser.Add(txtApplicant.Tag.ToString());
                    m_billMessageServer.EndFlowMessage(billID.ToString(), msg, null, listUser);
                }

                MessageDialog.ShowPromptMessage("部门负责人审核成功！");
            }
            else
            {
                MessageDialog.ShowPromptMessage("请确认单据状态！");
                return;
            }

            this.Close();
        }

        private void 审批ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (lblStatus.Text.Trim() == DimissionBillStatus.等待人力资源审阅.ToString())
            {
                if (txtHROpinion.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请填写人力资源部意见！");
                    return;
                }

                HR_DimissionBill dimission = new HR_DimissionBill();

                dimission.HR_Opinion = txtHROpinion.Text;
                dimission.HR_Signature = txtHRSignature.Text;
                dimission.HR_SignatureDate = dtpHRDate.Value;
                dimission.WorkID = txtApplicant.Tag.ToString();
                dimission.Date = dtpDate.Value;
                dimission.Reason = txtReason.Text;

                billID = Convert.ToInt32(m_dimiServer.AddAndUpdateDimission(dimission, "人力资源部审批", out error));

                if (billID == 0)
                {
                    MessageDialog.ShowPromptMessage(error);
                    return;
                }

                MessageDialog.ShowPromptMessage("人力资源部审批成功！等待分管领导审核");

                m_billMessageServer.PassFlowMessage(billID.ToString(), string.Format("{0}号离职申请单，请分管领导审核", billID.ToString()),
                    BillFlowMessage_ReceivedUserType.角色,
                       m_billMessageServer.GetDeptLeaderRoleName(m_personnerServer.GetPersonnelViewInfo(txtApplicant.Tag.ToString()).部门编号).ToList());
            }
            else
            {
                MessageDialog.ShowPromptMessage("请确认单据状态！");
                return;
            }

            this.Close();
        }

        private void 审批ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (lblStatus.Text.Trim() == DimissionBillStatus.等待分管领导审核.ToString())
            {
                if (txtLeaderOpinion.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请填写分管领导意见！");
                    return;
                }

                HR_DimissionBill dimission = new HR_DimissionBill();

                dimission.LeaderOpinion = txtLeaderOpinion.Text;
                dimission.LeaderSignature = BasicInfo.LoginID;
                dimission.LeaderSignatureDate = ServerTime.Time;
                dimission.WorkID = txtApplicant.Tag.ToString();
                dimission.Date = dtpDate.Value;
                dimission.Reason = txtReason.Text;

                if (!cbLeaderAuthorize.Checked)
                {
                    if (MessageBox.Show("您是否同意 " + txtApplicant.Text.Trim()
                                      + " 的离职申请?", "消息", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cbLeaderAuthorize.Checked = true;
                    }
                    else
                    {
                        cbLeaderAuthorize.Checked = false;
                    }

                }

                dimission.LeaderAuthorize = cbLeaderAuthorize.Checked;
                billID = Convert.ToInt32(m_dimiServer.AddAndUpdateDimission(dimission, "分管领导审批", out error));

                if (billID == 0)
                {
                    MessageDialog.ShowPromptMessage(error);
                    return;
                }

                if (cbLeaderAuthorize.Checked)
                {
                    string msg = string.Format("{0} 号离职申请已由分管领导审批，请总经理审核", billID.ToString());
                    m_billMessageServer.PassFlowMessage(billID.ToString(), msg, CE_RoleEnum.人力资源主管.ToString(), true);
                }
                else
                {
                    string msg = string.Format("{0} 号离职申请分管领导不同意离职申请,单据完成", billID.ToString());
                    List<string> listUser = new List<string>();

                    listUser.Add(txtApplicant.Tag.ToString());
                    m_billMessageServer.EndFlowMessage(billID.ToString(), msg, null, listUser);
                }

                MessageDialog.ShowPromptMessage("分管领导审批成功！");
            }
            else
            {
                MessageDialog.ShowPromptMessage("请确认单据状态！");
                return;
            }

            this.Close();
        }

        private void 批准ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lblStatus.Text.Trim() == DimissionBillStatus.等待总经理批准.ToString())
            {
                if (txtGMOpinion.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请填写总经理意见！");
                    return;
                }

                HR_DimissionBill dimission = new HR_DimissionBill();

                dimission.GM_Opinion = txtGMOpinion.Text;
                dimission.GM_Signature = BasicInfo.LoginID;
                dimission.GM_SignatureDate = ServerTime.Time;
                dimission.WorkID = txtApplicant.Tag.ToString();
                dimission.AllowDimissionDate = dtpAllowDate.Value;
                dimission.Date = dtpDate.Value;
                dimission.Reason = txtReason.Text;

                if (!cbGMAuthorize.Checked)
                {
                    if (MessageBox.Show("您是否同意 " + txtApplicant.Text.Trim()
                                      + " 的离职申请?", "消息", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cbGMAuthorize.Checked = true;
                    }
                    else
                    {
                        cbGMAuthorize.Checked = false;
                    }

                }

                dimission.GM_Authorize = cbGMAuthorize.Checked;
                billID = Convert.ToInt32(m_dimiServer.AddAndUpdateDimission(dimission, "总经理批准", out error));

                if (billID == 0)
                {
                    MessageDialog.ShowPromptMessage(error);
                    return;
                }

                List<string> noticeUser = new List<string>();

                noticeUser.Add(txtApplicant.Tag.ToString());

                List<string> noticeRole = new List<string>();

                noticeRole.Add(CE_RoleEnum.人力资源主管.ToString());

                m_billMessageServer.EndFlowMessage(billID.ToString(), string.Format("{0} 号离职申请已经处理完毕", billID), noticeRole, noticeUser);
            }
            else
            {
                MessageDialog.ShowPromptMessage("请确认单据状态！");
                return;
            }

            this.Close();
        }

        private void FormDimissionList_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lblStatus.Text.Trim() != DimissionBillStatus.已完成.ToString())
            {
                if (MessageBox.Show("您是否确定要删除选中行的信息?", "消息",
                   MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (!m_dimiServer.DeleteDimission(txtApplicant.Tag.ToString(), dtpDate.Value, out error))
                    {
                        MessageDialog.ShowPromptMessage(error);
                        return;
                    }

                    m_billMessageServer.BillType = "员工离职申请单";
                    m_billMessageServer.DestroyMessage(billID.ToString());

                    this.Close();
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("单据已完成，不能删除！");
            }
        }

    }
}
