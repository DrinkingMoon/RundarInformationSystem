using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TaskManagementServer;
using GlobalObject;
using ServerModule;
using PlatformManagement;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 会议管理窗体
    /// </summary>
    public partial class FormMeetingManagement : Form
    {
        /// <summary>
        /// 为日常会议提供业务操作的服务接口
        /// </summary>
        IMeetingServer m_meetingServer = TaskObjectFactory.GetOperator<IMeetingServer>();

        /// <summary>
        /// 数据源
        /// </summary>
        DataTable m_dataSource = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public FormMeetingManagement()
        {
            InitializeComponent();

            AuthorityControl();

            DateTime dt = ServerTime.Time;

            RefreshData(dt.Date, dt.Date.AddDays(7));
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="meetingID">会议编号</param>
        /// <param name="lstAdditionalInfo">附加信息</param>
        public FormMeetingManagement(string meetingID, List<string> lstAdditionalInfo)
        {
            InitializeComponent();

            AuthorityControl();

            RefreshData(Convert.ToDateTime(lstAdditionalInfo[0]).AddDays(-3), Convert.ToDateTime(lstAdditionalInfo[1]).AddDays(3));

            PositioningRecord(meetingID);
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
                    if (dataGridView1.Rows[i].Cells["会议编号"].Value.ToString() == id)
                    {
                        dataGridView1.FirstDisplayedScrollingRowIndex = i;
                        dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 权限控制
        /// </summary>
        void AuthorityControl()
        {
            if (!BasicInfo.IsFuzzyContainsRoleName("会议发布员"))
            {
                foreach (var item in toolStrip1.Items)
                {
                    if (((ToolStripItem)item).Tag != null && ((ToolStripItem)item).Tag.ToString().ToUpper() != "VIEW")
                    {
                        ((ToolStripItem)item).Enabled = false;
                    }
                }
            }
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        void RefreshData()
        {
            RefreshData(dateTimePickerST.Value, dateTimePickerET.Value);
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="beginTime">会议开始时间，只取日期部分</param>
        /// <param name="endTime">会议结束时间，只取日期部分</param>
        void RefreshData(DateTime beginTime, DateTime endTime)
        {
            dateTimePickerST.Value = beginTime;
            dateTimePickerET.Value = endTime;

            m_dataSource = m_meetingServer.GetMeetingData(beginTime, endTime);

            dataGridView1.DataSource = GetFilterData(m_dataSource);

            userControlDataLocalizer1.Init(dataGridView1, this.Name,
                    UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));

            this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);
        }

        private void FormMeetingManagement_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            FormMeeting form = new FormMeeting();

            form.ShowDialog();

            if (form.ChangeFlag && form.MeetingData != null)
            {
                RefreshData();

                PositioningRecord(form.MeetingData.会议编号);
            }
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
            {
                return;
            }

            DataGridViewCellCollection cells = dataGridView1.Rows[e.RowIndex].Cells;

            lblBillStatus.Text = cells["会议状态"].Value.ToString();
            txtID.Text = cells["会议编号"].Value.ToString();
            txtTitle.Text = cells["标题"].Value.ToString();
            txtDeclarePersonnel.Text = cells["创建人姓名"].Value.ToString();
            txtHost.Text = cells["主持人姓名"].Value.ToString();
            txt与会人员.Text = cells["与会人员"].Value.ToString();
            txt与会资源.Text = cells["会议资源"].Value.ToString();

            dtpkCreateTime.Value = (DateTime)cells["创建日期"].Value;
            dtpkBeginTime.Value = (DateTime)cells["开始时间"].Value;
            dtpkEndTime.Value = (DateTime)cells["结束时间"].Value;

            List<string> lstPerson = new List<string>();

            lstPerson.AddRange(txt与会人员.Text.Split(new char[] { ','}));

            if (!lstPerson.Contains(txtDeclarePersonnel.Text))
            {
                lstPerson.Add(txtDeclarePersonnel.Text);
            }

            if (!lstPerson.Contains(txtHost.Text))
            {
                lstPerson.Add(txtHost.Text);
            }

            lblPersonAmount.Text = lstPerson.Count.ToString();
        }

        /// <summary>
        /// 更新选择的记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择要操作的记录后再进行此操作");
                return;
            }

            DataRow row = m_dataSource.Select(string.Format("会议编号 = '{0}'", dataGridView1.SelectedRows[0].Cells["会议编号"].Value))[0];

            FormMeeting form = new FormMeeting(FormMeeting.OperateMode.Update, row);

            form.ShowDialog();

            if (form.ChangeFlag)
            {
                RefreshData();

                PositioningRecord(form.MeetingData.会议编号);
            }
        }

        /// <summary>
        /// 删除选择的数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择要操作的记录后再进行此操作");
                return;
            }

            if (DialogResult.Yes != MessageDialog.ShowEnquiryMessage("您真的要删除指定记录吗？"))
            {
                return;
            }

            if (dataGridView1.SelectedRows[0].Cells["创建人工号"].Value.ToString() != BasicInfo.LoginID)
            {
                MessageDialog.ShowPromptMessage("抱歉，只能删除您本人创建的会议！");
                return;
            }

            try
            {
                m_meetingServer.Delete(dataGridView1.SelectedRows[0].Cells["会议编号"].Value.ToString());

                RefreshData();
            }
            catch (Exception exce)
            {
                MessageDialog.ShowErrorMessage(exce.Message);
            }
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefreshData_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        /// <summary>
        /// 检索指定数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        /// <summary>
        /// 获取过滤的数据源
        /// </summary>
        /// <param name="source">源数据</param>
        /// <returns>返回过滤后的数据表</returns>
        private DataTable GetFilterData(DataTable source)
        {
            if (source == null || source.Rows.Count == 0)
            {
                return null;
            }

            DataTable dt = source;
            DataTable dataSource = dt.Clone();
            string filterExpression = "";

            if (chk所有.Checked)
            {
                filterExpression = "";
            }
            else
            {
                if (chk待发布会议.Checked)
                {
                    if (filterExpression.Length == 0)
                        filterExpression = "会议状态 = '待发'";
                    else
                        filterExpression += " or (会议状态 = '待发')";
                }

                if (chk已撤销会议.Checked)
                {
                    if (filterExpression.Length == 0)
                        filterExpression = "会议状态 = '撤销'";
                    else
                        filterExpression += " or (会议状态 = '撤销')";
                }

                if (chk已召开会议.Checked)
                {
                    if (filterExpression.Length == 0)
                        filterExpression = string.Format("会议状态 = '已发' and 开始时间 < '{0}'", ServerTime.Time);
                    else
                        filterExpression += string.Format("or (会议状态 = '已发' and 开始时间 < '{0}')", ServerTime.Time);
                }

                if (chk已发布未召开会议.Checked)
                {
                    if (filterExpression.Length == 0)
                        filterExpression = string.Format("会议状态 = '已发' and 开始时间 > '{0}'", ServerTime.Time);
                    else
                        filterExpression += string.Format("or (会议状态 = '已发' and 开始时间 > '{0}')", ServerTime.Time);
                }

                if (chkMySelf.Checked)
                {
                    if (filterExpression.Length == 0)
                        filterExpression = string.Format("创建人姓名 = '{0}' or 主持人姓名 = '{0}' or 与会人员 like '%{0}%'", BasicInfo.LoginName);
                    else
                        filterExpression += string.Format("创建人姓名 = '{0}' or 主持人姓名 = '{0}' or 与会人员 like '%{0}%'", BasicInfo.LoginName);
                }

                if (GlobalObject.GeneralFunction.IsNullOrEmpty(filterExpression))
                {
                    dt = null;
                }
            }

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(filterExpression))
            {
                dataSource = dt;
            }
            else
            {
                DataRow[] rows = dt.Select(filterExpression);

                foreach (var row in rows)
                {
                    dataSource.ImportRow(row);
                }
            }

            return dataSource;
        }

        private void chk显示控制_CheckedChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
            {
                return;
            }

            FormViewData form = new FormViewData(dataGridView1.Columns, dataGridView1.Rows[e.RowIndex]);
            form.ShowDialog();
        }

        private void btnAdvSearch_Click(object sender, EventArgs e)
        {
            IAuthorization authorization = PlatformFactory.GetObject<IAuthorization>();
            string businessID = "会议信息普通查询";
            IQueryResult qr = authorization.QueryMultParam("会议信息普通查询", null, new object[] { BasicInfo.LoginID });
            List<string> lstFindField = new List<string>();

            if (qr.DataCollection == null || qr.DataCollection.Tables.Count == 0)
            {
                return;
            }

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

        private void txt与会资源_MouseEnter(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(txt与会资源, txt与会资源.Text);
        }

        private void txt与会人员_MouseEnter(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(txt与会人员, txt与会人员.Text);
        }

        /// <summary>
        /// 设置会议实际完成时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetRealFinishedTime_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择要操作的记录后再进行此操作");
                return;
            }

            if (dataGridView1.SelectedRows[0].Cells["创建人工号"].Value.ToString() != BasicInfo.LoginID)
            {
                MessageDialog.ShowPromptMessage("抱歉，只能修改您本人创建的会议！");
                return;
            }

            if (DialogResult.Yes != MessageDialog.ShowEnquiryMessage(
                string.Format("您真的要设置指定记录的完成时间为【{0}】吗？", dtpkEndTime.Value)))
            {
                return;
            }

            try
            {
                m_meetingServer.SetRealFinishedTime(dataGridView1.SelectedRows[0].Cells["会议编号"].Value.ToString(), dtpkEndTime.Value);

                MessageDialog.ShowPromptMessage("成功完成修改");

                RefreshData();
            }
            catch (Exception exce)
            {
                MessageDialog.ShowErrorMessage(exce.Message);
            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择要操作的记录后再进行此操作");
                return;
            }

            DataRow row = m_dataSource.Select(string.Format("会议编号 = '{0}'", dataGridView1.SelectedRows[0].Cells["会议编号"].Value))[0];

            FormMeeting form = new FormMeeting(FormMeeting.OperateMode.View, row);

            form.ShowDialog();

            RefreshData();
        }
    }
}