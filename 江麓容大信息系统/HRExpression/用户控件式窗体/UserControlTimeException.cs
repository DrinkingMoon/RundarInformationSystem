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
using ServerModule;
using GlobalObject;
using Service_Peripheral_HR;
using UniversalControlLibrary;

namespace Form_Peripheral_HR
{
    public partial class UserControlTimeException : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_error;

        /// <summary>
        /// 可供查找的所有字段
        /// </summary>
        string[] m_findField = null;

        /// <summary>
        /// 查询结果
        /// </summary>
        IQueryResult m_queryResult;

        /// <summary>
        /// 操作权限
        /// </summary>
        AuthorityFlag m_authorityFlag;

        /// <summary>
        /// 考勤机导入的人员考勤明细表操作类
        /// </summary>
        IAttendanceMachineServer m_attendanceMachineServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IAttendanceMachineServer>();

        /// <summary>
        /// 人员考勤流水账操作类
        /// </summary>
        IAttendanceDaybookServer m_dayBookServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IAttendanceDaybookServer>();

        /// <summary>
        /// 考勤异常登记操作类
        /// </summary>
        ITimeExceptionServer m_timeExServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<ITimeExceptionServer>();

        public UserControlTimeException(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authorityFlag = nodeInfo.Authority;

            cmbRealExceptionType.DataSource = (from a in m_attendanceMachineServer.GetExceptionType()
                                               where a.IsBusiness == false
                                               && a.TypeName != CE_HR_AttendanceExceptionType.正常.ToString()
                                               && a.TypeName != CE_HR_AttendanceExceptionType.集体异常.ToString()
                                               && a.TypeName != CE_HR_AttendanceExceptionType.节假.ToString()
                                               select a.TypeName).ToList();
            DateTime dtBegin, dtEnd;

            ServerTime.GetMonthlyBalance(ServerTime.Time, out dtBegin, out dtEnd);

            dtpBeginDate.Value = dtBegin;
            dtpEndDate.Value = dtEnd;

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
        /// 刷新
        /// </summary>
        void RefreshDataGridView()
        {
            if (!m_timeExServer.GetAllBill(out m_queryResult, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            DataTable dt = m_queryResult.DataCollection.Tables[0];
            DataTable dtTemp = dt.Clone();
            bool flag = false;

            if (BasicInfo.RoleCodes.Contains("人力资源部办公室文员"))
            {
                flag = true;
            }

            string strSelect = "异常日期 >= '" + dtpBeginDate.Value + "' and 异常日期 <= '" + dtpEndDate.Value + "'";
            string strTemp = "";

            if (flag)
            {
                if (chbIsShowOperation.Checked)
                {
                    strTemp = " and 人力资源部审核人 is not null";
                }
                else
                {
                    strTemp = " and 人力资源部审核人 is null";
                }
            }
            else
            {

                if (chbIsShowOperation.Checked)
                {
                    strTemp = " and (部门审核人 is not null or 人力资源部审核人 is not null)";
                }
                else
                {
                    strTemp = " and 部门审核人 is null and 人力资源部审核人 is null";
                }
            }


            DataRow[] dr = dt.Select(strSelect + strTemp);

            for (int i = 0; i < dr.Length; i++)
            {
                dtTemp.ImportRow(dr[i]);
            }

            dataGridView1.DataSource = dtTemp;

            if (dt != null && dt.Rows.Count > 0)
            {
                dataGridView1.Columns["序号"].Visible = false;
                dataGridView1.Columns["部门编码"].Visible = false;
            }

            this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
            this.dataGridView1_ColumnWidthChanged);

            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            #region 隐藏不允许查看的列

            if (m_queryResult.HideFields != null && m_queryResult.HideFields.Length > 0)
            {
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    if (m_queryResult.HideFields.Contains(dataGridView1.Columns[i].Name))
                    {
                        dataGridView1.Columns[i].Visible = false;
                    }
                }
            }

            #endregion

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

            foreach (DataGridViewColumn item in dataGridView1.Columns)
            {
                if (item.Name != "选")
                {
                    item.ReadOnly = true;
                }
                else
                {
                    item.ReadOnly = false;
                    item.Frozen = false;
                }
            }

            dataGridView1.Refresh();
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        private void UserControlTimeException_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        private void 选择ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= dataGridView1.SelectedRows.Count - 1; i++)
            {
                dataGridView1.SelectedRows[i].Cells["选"].Value = true;
            }
        }

        private void 取消ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= dataGridView1.SelectedRows.Count - 1; i++)
            {
                dataGridView1.SelectedRows[i].Cells["选"].Value = false;
            }
        }

        private void 全消ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells["选"].Value = false;
            }
        }

        private void 全选ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells["选"].Value = true;
            }
        }

        private void 提交toolStripButton1_Click(object sender, EventArgs e)
        {
            bool flag = false;

            if (cmbRealExceptionType.Text == "")
            {
                MessageDialog.ShowPromptMessage("请选择真实的异常类型！");
                return;
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {

                if (Convert.ToBoolean(dataGridView1.Rows[i].Cells["选"].Value))
                {
                    flag = true;
                    HR_TimeException exception = new HR_TimeException();

                    exception.ID = Convert.ToInt32(dataGridView1.Rows[i].Cells["序号"].Value);
                    exception.Date = Convert.ToDateTime(dataGridView1.Rows[i].Cells["异常日期"].Value);
                    exception.ExceptionDescription = txtDescription.Text;
                    exception.RealExceptionType = m_attendanceMachineServer.GetExceptionTypeID(cmbRealExceptionType.Text);

                    if (!m_timeExServer.UpdateTimeException(exception, "员工自己", out m_error))
                    {
                        MessageDialog.ShowPromptMessage(m_error);
                        return;
                    }
                }
            }

            if (!flag)
            {
                MessageDialog.ShowPromptMessage("请勾选需要修改的数据行");
                return;
            }

            RefreshDataGridView();
        }

        private void 审核toolStripButton_Click(object sender, EventArgs e)
        {
            bool flag = false;

            if (cmbRealExceptionType.Text == "")
            {
                MessageDialog.ShowPromptMessage("请选择真实的异常类型！");
                return;
            }

            if (txtHRAuthorize.Text.Trim() != "")
            {
                MessageDialog.ShowPromptMessage("人力资源已经审核，您不能再审核！");
                return;
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dataGridView1.Rows[i].Cells["选"].Value))
                {
                    flag = true;
                    HR_TimeException exception = new HR_TimeException();

                    exception.ID = Convert.ToInt32(dataGridView1.Rows[i].Cells["序号"].Value);
                    exception.Date = Convert.ToDateTime(dataGridView1.Rows[i].Cells["异常日期"].Value);
                    exception.ExceptionDescription = txtDescription.Text;
                    exception.RealExceptionType = m_attendanceMachineServer.GetExceptionTypeID(cmbRealExceptionType.Text);
                    exception.DeptAuditor = BasicInfo.LoginID;
                    exception.DeptAuditorSignatureDate = ServerTime.Time;
                    exception.WorkID = dataGridView1.Rows[i].Cells["员工编号"].Value.ToString();

                    if (!m_timeExServer.UpdateTimeException(exception, "部门审核", out m_error))
                    {
                        MessageDialog.ShowPromptMessage(m_error);
                        return;
                    }
                }
            }

            if (!flag)
            {
                MessageDialog.ShowPromptMessage("请勾选需要修改的数据行");
                return;
            }
            MessageDialog.ShowPromptMessage("部门审核成功！");
            RefreshDataGridView();
        }

        private void 人力资源toolStripButton_Click(object sender, EventArgs e)
        {
            bool flag = false;

            if (cmbRealExceptionType.Text == "")
            {
                MessageDialog.ShowPromptMessage("请选择真实的异常类型！");
                return;
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dataGridView1.Rows[i].Cells["选"].Value))
                {
                    HR_TimeException exception = new HR_TimeException();

                    exception.ID = Convert.ToInt32(dataGridView1.Rows[i].Cells["序号"].Value);
                    exception.Date = Convert.ToDateTime(dataGridView1.Rows[i].Cells["异常日期"].Value).Date;
                    exception.HR_Signature = BasicInfo.LoginID;
                    exception.HR_SignatureDate = ServerTime.Time.ToString();
                    exception.WorkID = dataGridView1.Rows[i].Cells["员工编号"].Value.ToString();
                    exception.ExceptionDescription = dataGridView1.Rows[i].Cells["异常描述"].Value.ToString();
                    exception.RealExceptionType = m_attendanceMachineServer.GetExceptionTypeID(dataGridView1.Rows[i].Cells["实际异常类型"].Value.ToString());

                    if (!m_timeExServer.UpdateTimeException(exception, "人力资源审核", out m_error))
                    {
                        MessageDialog.ShowPromptMessage(m_error);
                        return;
                    }
                }
            }

            if (!flag)
            {
                MessageDialog.ShowPromptMessage("请勾选需要修改的数据行");
                return;
            }

            MessageDialog.ShowPromptMessage("人力资源审核成功！");

            RefreshDataGridView();
        }

        private void 刷新toolStripButton1_Click(object sender, EventArgs e)
        {
            RefreshDataGridView();
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtName.Text = dataGridView1.CurrentRow.Cells["员工姓名"].Value.ToString();
            txtWorkID.Text = dataGridView1.CurrentRow.Cells["员工编号"].Value.ToString();
            txtExceptionType.Text = dataGridView1.CurrentRow.Cells["考勤异常类型"].Value.ToString();
            txtDescription.Text = dataGridView1.CurrentRow.Cells["异常描述"].Value.ToString();
            txtDeptAuditor.Text = dataGridView1.CurrentRow.Cells["部门审核人"].Value.ToString();
            txtHRAuthorize.Text = dataGridView1.CurrentRow.Cells["人力资源部审核人"].Value.ToString();
            cmbRealExceptionType.Text = dataGridView1.CurrentRow.Cells["实际异常类型"].Value.ToString();

            if (dataGridView1.CurrentRow.Cells["部门审核日期"].Value.ToString() != "")
            {
                dtpDeptDate.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["部门审核日期"].Value);
            }

            if (dataGridView1.CurrentRow.Cells["人力资源部审核日期"].Value.ToString() != "")
            {
                dtpHRDate.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["人力资源部审核日期"].Value);
            }

            if (dataGridView1.CurrentRow.Cells["应打卡时间"].Value.ToString() != "")
            {
                string strTime = dataGridView1.CurrentRow.Cells["应打卡时间"].Value.ToString();

                if (strTime.IndexOf(" ") > 0)
                {
                    dtpObjectClockInTime.Value = Convert.ToDateTime( strTime.Substring(strTime.LastIndexOf(" "), 
                        strTime.Length - strTime.LastIndexOf(" ")));
                }
                else
                {
                    dtpObjectClockInTime.Value = Convert.ToDateTime(strTime);
                }
            }

            if (dataGridView1.CurrentRow.Cells["实际打卡时间"].Value.ToString() != "")
            {
                string strTime = dataGridView1.CurrentRow.Cells["实际打卡时间"].Value.ToString();

                if (strTime.IndexOf(" ") > 0)
                {
                    txtRealClockInTime.Text = strTime.Substring(strTime.LastIndexOf(" "),
                        strTime.Length - strTime.LastIndexOf(" "));
                }
                else
                {
                    txtRealClockInTime.Text = strTime;
                }
            }
            else
            {
                txtRealClockInTime.Text = "";
            }

            if (dataGridView1.CurrentRow.Cells["异常日期"].Value.ToString() != "")
            {
                dtpExceptionDate.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["异常日期"].Value);
            }
        }

        private void 综合toolStripButton_Click(object sender, EventArgs e)
        {
            IAuthorization authorization = PlatformFactory.GetObject<IAuthorization>();
            string businessID = "考勤异常登记管理";
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

        private void btnOK_Click(object sender, EventArgs e)
        {
            RefreshDataGridView();
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Cells["考勤异常类型"].Value.ToString() == "旷工")
                    {
                        dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.Red;
                    }

                    if (dataGridView1.Rows[i].Cells["部门审核人"].Value.ToString() != "")
                    {
                        dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.Green;
                    }

                    if (dataGridView1.Rows[i].Cells["人力资源部审核人"].Value.ToString() != "")
                    {
                        dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.OrangeRed;
                    }
                }
            }            
        }

        private void UserControlTimeException_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authorityFlag);

            if (BasicInfo.ListRoles.Contains(CE_RoleEnum.人力资源部办公室文员.ToString()))
            {
                人力资源toolStripButton.Visible = false;
                提交toolStripButton1.Visible = false;
                审核toolStripButton.Visible = false;
            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //string dayBookID = dataGridView1.CurrentRow.Cells["流水号"].Value.ToString();

            //FormAttendanceDayBookList frm = new FormAttendanceDayBookList(dayBookID);
            //frm.ShowDialog();
        }

        private void 强制toolStripButton_Click(object sender, EventArgs e)
        {
            if (MessageDialog.ShowEnquiryMessage("无论关联单是否已经批准，都强制性的处理为当前的真实考勤类别？") == DialogResult.Yes)
            {
                if (cmbRealExceptionType.Text == "")
                {
                    MessageDialog.ShowPromptMessage("请选择真实的异常类型！");
                    return;
                }

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(dataGridView1.Rows[i].Cells["选"].Value))
                    {
                        HR_TimeException exception = new HR_TimeException();

                        exception.ID = Convert.ToInt32(dataGridView1.Rows[i].Cells["序号"].Value);
                        exception.Date = Convert.ToDateTime(dataGridView1.Rows[i].Cells["异常日期"].Value).Date;
                        exception.HR_Signature = BasicInfo.LoginID;
                        exception.HR_SignatureDate = ServerTime.Time.ToString();
                        exception.WorkID = dataGridView1.Rows[i].Cells["员工编号"].Value.ToString();
                        exception.ExceptionDescription = BasicInfo.LoginName + " 强制处理 " + txtDescription.Text;
                        exception.RealExceptionType = m_attendanceMachineServer.GetExceptionTypeID(cmbRealExceptionType.Text);

                        if (!m_timeExServer.UpdateTimeException(exception, out m_error))
                        {
                            MessageDialog.ShowPromptMessage(m_error);
                            return;
                        }
                    }
                }

                RefreshDataGridView();
            }
        }

        private void dtpBeginDate_ValueChanged(object sender, EventArgs e)
        {

        }

        private void chbIsShowOperation_CheckedChanged(object sender, EventArgs e)
        {
            RefreshDataGridView();

            if (BasicInfo.ListRoles.Contains(CE_RoleEnum.人力资源部办公室文员.ToString()))
            {
                if (chbIsShowOperation.Checked)
                {
                    人力资源toolStripButton.Visible = true;
                    强制toolStripButton.Visible = false;
                }
                else
                {
                    人力资源toolStripButton.Visible = false;
                    强制toolStripButton.Visible = true;
                }
            }
        }
    }
}
