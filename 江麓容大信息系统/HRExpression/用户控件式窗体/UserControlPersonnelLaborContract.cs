using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Expression;
using Service_Peripheral_HR;
using PlatformManagement;
using ServerModule;
using GlobalObject;
using UniversalControlLibrary;

namespace Form_Peripheral_HR
{
    /// <summary>
    /// 员工合同主界面
    /// </summary>
    public partial class UserControlPersonnelLaborContract : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string error;

        /// <summary>
        /// 员工编号
        /// </summary>
        string m_workID;

        /// <summary>
        /// 可供查找的所有字段
        /// </summary>
        string[] m_findField = null;

        /// <summary>
        /// 操作权限
        /// </summary>
        PlatformManagement.AuthorityFlag m_authorityFlag;

        /// <summary>
        /// 查询结果
        /// </summary>
        IQueryResult m_queryResult;

        /// <summary>
        /// 人员档案管理类
        /// </summary>
        IPersonnelArchiveServer m_personnerServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IPersonnelArchiveServer>();

        /// <summary>
        /// 合同管理服务类
        /// </summary>
        ILaborContractServer m_laborServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<ILaborContractServer>();

        /// <summary>
        /// 员工合同变更信息
        /// </summary>
        HR_PersonnelLaborContractHistory personnelContHistory;

        #region 2013.04.16 夏石友 因为修改权限管理模块后修正
        
        /// <summary>
        /// 获取预警通知类消息操作接口
        /// </summary>
        IWarningNotice m_warningNotice = PlatformFactory.GetObject<IWarningNotice>();

        #endregion

        public UserControlPersonnelLaborContract(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authorityFlag = nodeInfo.Authority;

            RefreshControl();

            txtProposer.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(txtProposer_OnCompleteSearch);
        }

        public UserControlPersonnelLaborContract(AuthorityFlag authority, string workID)
        {
            InitializeComponent();

            m_authorityFlag = authority;
            AuthorityControl(m_authorityFlag);

            m_workID = workID;
            View_HR_PersonnelLaborContract personnelContract = m_laborServer.GetPersonnelContarctByWorkID(m_workID,out error);

            if (personnelContract != null)
            {
                txtProposer.Text = personnelContract.员工姓名;
                txtRemark.Text = personnelContract.备注;
                txtTemplet.Text = personnelContract.合同模板;
                cmbStatus.Text = personnelContract.合同状态;
                dtpBeginTime.Value = personnelContract.合同起始时间;
                dtpEndTime.Value = personnelContract.合同终止时间;

                btnFindTemplet.Visible = false;
                txtProposer.Enabled = false;
                txtProposer.BackColor = Color.White;
            }
            else
            {
                for (int i = 0; i < groupBox1.Controls.Count; i++)
                {
                    groupBox1.Controls[i].Visible = false;
                }

                groupBox1.Text = "没有签订合同和协议";
                groupBox1.ForeColor = Color.Red;
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

        /// <summary>
        /// 重新窗体消息处理函数
        /// </summary>
        /// <param name="message">窗体消息</param>
        protected override void DefWndProc(ref Message message)
        {
            switch (message.Msg)
            {
                case WndMsgSender.ShowSpecificData: // 显示特定数据的消息

                    WndMsgData msg1 = new WndMsgData();
                    Type dataType1 = msg1.GetType();

                    msg1 = (WndMsgData)message.GetLParam(dataType1);

                    // 人员工号
                    string workID = (string)GeneralFunction.IntPtrToClass(msg1.ObjectMessage, msg1.BytesOfObjectMessage);

                    PositioningRecord(workID);

                    break;

                case WndMsgSender.WarningNotice:    // 系统预警消息

                    WndMsgData msg2 = new WndMsgData();
                    Type dataType2 = msg2.GetType();

                    msg2 = (WndMsgData)message.GetLParam(dataType2);

                    // 预警数据
                    List<string> lstData = (List<string>)GeneralFunction.IntPtrToClass(msg2.ObjectMessage, msg2.BytesOfObjectMessage);

                    PositioningRecord(lstData[1]);

                    break;

                default:
                    base.DefWndProc(ref message);
                    break;
            }
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="id">定位用的身份证号码</param>
        public void PositioningRecord(string id)
        {
            string strColName = "";

            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                if (col.Visible)
                {
                    strColName = col.Name;
                    break;
                }
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (id != "0" || id != "")
                {
                    if (dataGridView1.Rows[i].Cells["编号"].Value.ToString() == id)
                    {
                        dataGridView1.FirstDisplayedScrollingRowIndex = i;
                        dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 查找模板
        /// </summary>
        private void btnFindTemplet_Click(object sender, EventArgs e)
        {
            FormContractTemplet frm = new FormContractTemplet();
            frm.ShowDialog();

            txtTemplet.Text = frm.TempletType;
            txtTemplet.Tag = frm.TempletID;

            cmbStatus.Items.Clear();

            DataTable dt = m_laborServer.GetContractStatusByType(m_laborServer.GetCategory(txtTemplet.Text,out error));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cmbStatus.Items.Add(dt.Rows[i][0].ToString());
            }
        }

        /// <summary>
        /// 清除控件
        /// </summary>
        void ClearControl()
        {
            txtRemark.Text = "";
            txtTemplet.Text = "";
            txtTemplet.Tag = null;
            txtProposer.Text = "";
            txtProposer.Tag = null;
            cmbStatus.Items.Clear();
            cmbStatus.SelectedIndex = -1;
            dtpBeginTime.Value = ServerTime.Time;
            dtpEndTime.Value = ServerTime.Time;
        }

        /// <summary>
        /// 刷新
        /// </summary>
        void RefreshControl()
        {
            ClearControl();

            DataTable statusDt = m_laborServer.GetContractStatus();

            if (statusDt.Rows.Count > 0)
            {
                for (int i = 0; i < statusDt.Rows.Count; i++)
                {
                    cmbStatus.Items.Add(statusDt.Rows[i]["状态"].ToString());
                }
            }

            if (!m_laborServer.GetAllPersonnelContarct(out m_queryResult, out error))
            {
                MessageDialog.ShowErrorMessage(error);
                return;
            }

            DataTable dt = m_queryResult.DataCollection.Tables[0];
            dataGridView1.DataSource = dt;

            dataGridView1.Columns["员工编号"].Visible = false;

            dataGridView1.Refresh();

            this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            // 添加查询用的列
            if (m_findField == null)
            {
                List<string> lstColumnName = new List<string>();

                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    if (dataGridView1.Columns[i].Visible)
                    {
                        lstColumnName.Add(dataGridView1.Columns[i].Name);
                    }
                }

                m_findField = lstColumnName.ToArray();
            }

            userControlDataLocalizer1.Init(dataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
        }

        /// <summary>
        /// 改变组件大小
        /// </summary>
        private void dataGridView1_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtProposer.Text = dataGridView1.CurrentRow.Cells["员工姓名"].Value.ToString();
            txtProposer.Tag = dataGridView1.CurrentRow.Cells["员工编号"].Value.ToString();
            txtRemark.Text = dataGridView1.CurrentRow.Cells["备注"].Value.ToString();
            txtTemplet.Text = dataGridView1.CurrentRow.Cells["合同模板"].Value.ToString();
            cmbStatus.Text = dataGridView1.CurrentRow.Cells["合同状态"].Value.ToString();
            dtpBeginTime.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["合同起始时间"].Value);
            dtpEndTime.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["合同终止时间"].Value);

            string[] s = dataGridView1.CurrentRow.Cells["合同模板"].Value.ToString().Split(' ');
            txtTemplet.Tag = m_laborServer.GetLaborContractTempletByTypeAndVersion(s[0].ToString(),s[1].ToString());

            personnelContHistory = new HR_PersonnelLaborContractHistory();

            personnelContHistory.BeginTime = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["合同起始时间"].Value);
            personnelContHistory.EndTime = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["合同终止时间"].Value);
            personnelContHistory.LaborContractStatusName = dataGridView1.CurrentRow.Cells["合同状态"].Value.ToString();
            personnelContHistory.LaborContractTypeCode = m_laborServer.GetLaborTypeByTypeName(s[0].ToString(), out error);
            personnelContHistory.LaborContractTypeName = s[0].ToString();
            personnelContHistory.Recorder = BasicInfo.LoginID;
            personnelContHistory.RecordTime = ServerTime.Time;
            personnelContHistory.Remark = dataGridView1.CurrentRow.Cells["备注"].Value.ToString();
            personnelContHistory.Version = Convert.ToDecimal(s[1].ToString());
            personnelContHistory.WorkID = dataGridView1.CurrentRow.Cells["员工编号"].Value.ToString();
        }

        /// <summary>
        /// 初始员工合同的数据集
        /// </summary>
        /// <returns>员工合同</returns>
        HR_PersonnelLaborContract GetPersonnelContract()
        {
            string[] s = txtTemplet.Text.Split(' ');
            HR_PersonnelLaborContract personnelContract = new HR_PersonnelLaborContract();

            personnelContract.WorkID = txtProposer.Tag.ToString();
            personnelContract.LaborContractTempletID = Convert.ToInt32(txtTemplet.Tag);
            personnelContract.LaborContractStatusID = m_laborServer.GetContractStatusByName(cmbStatus.Text);
            personnelContract.Recorder = BasicInfo.LoginID;
            personnelContract.RecordTime = ServerTime.Time;
            personnelContract.Remark = txtRemark.Text;
            personnelContract.BeginTime = dtpBeginTime.Value;
            personnelContract.EndTime = dtpEndTime.Value;

            return personnelContract;
        }

        private void 添加toolStripButton1_Click(object sender, EventArgs e)
        {
            if (txtProposer.Text.Trim() == "" || txtTemplet.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请选择员工和合同模板！");
                return;
            }

            if (cmbStatus.SelectedIndex == -1)
            {
                MessageDialog.ShowPromptMessage("请选择合同状态！");
                return;
            }

            if (!m_laborServer.AddPersonnelContract(GetPersonnelContract(), out error))
            {
                MessageDialog.ShowPromptMessage(error);
                return;
            }

            Dictionary<string, string> dicParams = new Dictionary<string, string>();

            dicParams.Add("附加信息1", "员工合同管理");
            dicParams.Add("附加信息2", txtProposer.Tag.ToString());
            dicParams.Add("附加信息3", "0");

            List<Flow_WarningNotice> notices = PlatformFactory.GetObject<IWarningNotice>().GetWarningNotice(dicParams);

            if (notices != null)
            {
                try
                {
                    m_warningNotice.ReadWarningNotice(BasicInfo.LoginID, notices[0].序号);
                }
                catch (Exception)
                {
                    RefreshControl();
                }
                finally
                {
                    RefreshControl();
                }
            }       

            RefreshControl();
        }

        private void 刷新toolStripButton1_Click(object sender, EventArgs e)
        {
            RefreshControl();
        }

        private void txtProposer_OnCompleteSearch()
        {
            txtProposer.Text = txtProposer.DataResult["员工姓名"].ToString();
            txtProposer.Tag = txtProposer.DataResult["员工编号"].ToString();
        }

        private void 导出toolStripButton3_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
        }

        private void 综合查询toolStripButton3_Click(object sender, EventArgs e)
        {
            IAuthorization authorization = PlatformFactory.GetObject<IAuthorization>();
            string businessID = "员工合同管理";
            IQueryResult qr = authorization.Query(businessID, null, null, 0);
            List<string> lstFindField = new List<string>();
            DataColumnCollection columns = qr.DataCollection.Tables[0].Columns;

            if (qr.Succeeded && columns.Count > 0)
            {
                for (int i = 0; i < columns.Count; i++)
                {
                    lstFindField.Add(columns[i].ColumnName);
                }
            }

            FormConditionFind formFindCondition = new FormConditionFind(this, lstFindField.ToArray(), businessID, labelTitle.Text);
            formFindCondition.ShowDialog();
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        private void UserControlPersonnelLaborContract_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authorityFlag);
        }

        private void 修改toolStripButton_Click(object sender, EventArgs e)
        {
            if (txtProposer.Text.Trim() == "" || txtTemplet.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请选择员工和合同模板！");
                return;
            }

            if (cmbStatus.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请选择合同状态！");
                return;
            }

            string[] s = txtTemplet.Text.Split(' ');

            bool b = m_laborServer.GetContractStatusFlagByID(
                m_laborServer.GetContractStatusByName(cmbStatus.Text).ToString());
            int billNo = Convert.ToInt32(dataGridView1.CurrentRow.Cells["编号"].Value.ToString());

            if (!m_laborServer.UpdatePersonnelContract(personnelContHistory, GetPersonnelContract(), b, billNo, out error))
            {
                MessageDialog.ShowPromptMessage(error);
                return;
            }

            MessageDialog.ShowPromptMessage("修改成功！");

            Dictionary<string, string> dicParams = new Dictionary<string, string>();

            dicParams.Add("附加信息1", "员工合同管理");
            dicParams.Add("附加信息2", txtProposer.Tag.ToString());
            dicParams.Add("附加信息3", dataGridView1.CurrentRow.Cells["编号"].Value.ToString());

            List<Flow_WarningNotice> notices = PlatformFactory.GetObject<IWarningNotice>().GetWarningNotice(dicParams);

            if (notices != null && notices.Count() > 0)
            {
                m_warningNotice.ReadWarningNotice(BasicInfo.LoginID, notices[0].序号);
            }

            RefreshControl();
        }

        private void 变更合同状态toolStripButton_Click(object sender, EventArgs e)
        {
            FormContractStatus frm = new FormContractStatus();
            frm.ShowDialog();
            RefreshControl();
        }
    }
}
