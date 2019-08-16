using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using Expression;
using GlobalObject;
using ServerModule;
using Service_Peripheral_HR;
using UniversalControlLibrary;

namespace Form_Peripheral_HR
{
    /// <summary>
    /// 集体考勤异常信息界面
    /// </summary>
    public partial class UserControlBatchException : Form
    {
        /// <summary>
        /// 异常号
        /// </summary>
        string m_billNo;

        /// <summary>
        /// 可供查找的所有字段
        /// </summary>
        string[] m_findField = null;

        /// <summary>
        /// 操作权限
        /// </summary>
        AuthorityFlag m_authorityFlag;

        /// <summary>
        /// 集体考勤异常操作类
        /// </summary>
        IBatchExceptionServer m_batchServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IBatchExceptionServer>();

        public UserControlBatchException(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();
            m_authorityFlag = nodeInfo.Authority;
            RefreshDataGridView();
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
        /// 改变组件大小
        /// </summary>
        private void UserControlBatchException_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="billNo">定位用的单据号</param>
        void PositioningRecord(string billNo)
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
                if (dataGridView1.Rows[i].Cells["异常号"].Value.ToString() == billNo)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                    break;
                }
            }
        }

        /// <summary>
        /// 刷新
        /// </summary>
        void RefreshDataGridView()
        {
            DataTable dt = m_batchServer.GetAllInfo(null);
            dataGridView1.DataSource = dt;
            dataGridView1.Columns["员工编号"].Visible = false;

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

            dataGridView1.Refresh();
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            m_billNo = dataGridView1.CurrentRow.Cells["异常号"].Value.ToString();
            txtDescription.Text = dataGridView1.CurrentRow.Cells["异常描述"].Value.ToString();
            txtDirector.Text = dataGridView1.CurrentRow.Cells["主管签字"].Value.ToString();
            txtRecorder.Text = dataGridView1.CurrentRow.Cells["记录员"].Value.ToString();
            txtRecorder.Tag = dataGridView1.CurrentRow.Cells["员工编号"].Value.ToString();

            dtpBeginTime.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["异常开始时间"].Value);
            dtpDate.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["异常日期"].Value);
            dtpEndTime.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["异常截止时间"].Value);
            dtpRecordTime.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["记录时间"].Value);

            if (dataGridView1.CurrentRow.Cells["主管签字时间"].Value.ToString() != "")
            {
                dtpSignatureDate.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["主管签字时间"].Value);
            }
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Cells["主管签字"].Value.ToString() == "")
                    {
                        dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.Red;
                    }
                }
            }
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        /// <summary>
        /// 初始化控件
        /// </summary>
        void ClearControl()
        {
            txtDescription.Text = "";
            txtDirector.Text = "";
            txtRecorder.Text = BasicInfo.LoginName;
            txtRecorder.Tag = BasicInfo.LoginID;

            dtpBeginTime.Value = ServerTime.Time;
            dtpDate.Value = ServerTime.Time;
            dtpEndTime.Value = ServerTime.Time;
            dtpRecordTime.Value = ServerTime.Time;
            dtpSignatureDate.Value = ServerTime.Time;
        }

        private void 新建toolStripButton1_Click(object sender, EventArgs e)
        {
            ClearControl();
        }

        private void 刷新toolStripButton1_Click(object sender, EventArgs e)
        {
            RefreshDataGridView();
        }

        private void 添加toolStripButton1_Click(object sender, EventArgs e)
        {
            if (txtDirector.Text.Trim() != "")
            {
                MessageDialog.ShowPromptMessage("异常信息主管已经审核，不能重复添加");
                return;
            }

            if (txtDescription.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请填写异常描述");
                return;
            }

            if (dtpBeginTime.Value > dtpEndTime.Value)
            {
                MessageDialog.ShowPromptMessage("异常起始时间不能大于截止时间");
                return;
            }

            MessageDialog.ShowPromptMessage("开始设置【相关人员】");
            UniversalControlLibrary.FormSelectPersonnel2 frm = new FormSelectPersonnel2();

            if (frm.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            if (frm.SelectedNotifyPersonnelInfo.UserType != BillFlowMessage_ReceivedUserType.用户.ToString())
            {
                return;
            }

            List<PersonnelBasicInfo> lstPersonnelInfo = frm.SelectedNotifyPersonnelInfo.PersonnelBasicInfoList;

            HR_BatchException batchException = new HR_BatchException();

            batchException.BeginTime = dtpBeginTime.Value;
            batchException.Date = dtpDate.Value;
            batchException.EndTime = dtpEndTime.Value;
            batchException.RecordTime = dtpRecordTime.Value;
            batchException.Recorder = BasicInfo.LoginID;
            batchException.Description = txtDescription.Text;

            try
            {
                m_batchServer.AddBatchException(batchException, lstPersonnelInfo);
                MessageDialog.ShowPromptMessage("提交成功");
                RefreshDataGridView();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void 删除toolStripButton2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }

            try
            {
                m_batchServer.DeleteBatchException((int)dataGridView1.CurrentRow.Cells["异常号"].Value);
                MessageDialog.ShowPromptMessage("删除成功");
                RefreshDataGridView();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void 审核toolStripButton1_Click(object sender, EventArgs e)
        {
            if (txtDirector.Text.Trim() != "")
            {
                MessageDialog.ShowPromptMessage("异常信息主管已经审核!");
                return;
            }

            HR_BatchException batchException = new HR_BatchException();

            batchException.ID = Convert.ToInt32(m_billNo);
            batchException.HR_Director = BasicInfo.LoginID;
            batchException.HR_SignatureDate = ServerTime.Time;

            try
            {
                m_batchServer.AuditingBatchException(batchException);
                MessageDialog.ShowPromptMessage("审核成功");
                RefreshDataGridView();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void UserControlBatchException_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authorityFlag);
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }

            DataTable dtTemp = m_batchServer.GetDetailPersonnel( Convert.ToInt32( dataGridView1.CurrentRow.Cells["异常号"].Value));

            if (dtTemp == null || dtTemp.Rows.Count == 0)
            {
                return;
            }

            UniversalControlLibrary.FormDataShow frm = new FormDataShow(dtTemp);
            frm.ShowDialog();

        }
    }
}
