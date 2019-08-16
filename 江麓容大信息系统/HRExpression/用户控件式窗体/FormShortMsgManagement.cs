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
using TaskServerModule;

namespace Form_Peripheral_HR
{
    /// <summary>
    /// 短信管理窗体
    /// </summary>
    public partial class FormShortMsgManagement : Form
    {
        /// <summary>
        /// 为短信业务操作的服务接口
        /// </summary>
        IShowMessageServer m_shortMsgServer = TaskObjectFactory.GetOperator<IShowMessageServer>();

        /// <summary>
        /// 数据源
        /// </summary>
        List<View_Task_ShortMessage> m_dataSource = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public FormShortMsgManagement()
        {
            InitializeComponent();

            AuthorityControl();

            cmbShortMsgType.DataSource = m_shortMsgServer.GetShortMsgType();
            cmbShortMsgType.DisplayMember = "TypeName";
            cmbShortMsgType.ValueMember = "TypeID";

            DateTime dt = ServerTime.Time;

            RefreshData(dt.Date, dt.Date.AddDays(7));
        }

        ///// <summary>
        ///// 构造函数
        ///// </summary>
        ///// <param name="meetingID">短信编号</param>
        ///// <param name="lstAdditionalInfo">附加信息</param>
        //public FormShortMsgManagement(string meetingID, List<string> lstAdditionalInfo)
        //{
        //    InitializeComponent();

        //    AuthorityControl();

        //    RefreshData(Convert.ToDateTime(lstAdditionalInfo[0]).AddDays(-3), Convert.ToDateTime(lstAdditionalInfo[1]).AddDays(3));

        //    PositioningRecord(meetingID);
        //}

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
                    if (dataGridView1.Rows[i].Cells["短信编号"].Value.ToString() == id)
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
            if (!BasicInfo.IsFuzzyContainsRoleName("短信发布员"))
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
        /// <param name="beginTime">短信开始时间，只取日期部分</param>
        /// <param name="endTime">短信结束时间，只取日期部分</param>
        void RefreshData(DateTime beginTime, DateTime endTime)
        {
            dateTimePickerST.Value = beginTime;
            dateTimePickerET.Value = endTime;

            m_dataSource = m_shortMsgServer.GetData(beginTime, endTime);

            dataGridView1.DataSource = new BindingCollection<View_Task_ShortMessage>(GetFilterData(m_dataSource));

            userControlDataLocalizer1.Init(dataGridView1, this.Name,
                    UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));

            this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);
        }

        private void FormWiseManagement_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            FormShortMessage form = new FormShortMessage();

            form.ShowDialog();

            if (form.ChangeFlag && form.ShortMessage != null)
            {
                RefreshData();

                PositioningRecord(form.ShortMessage.短信编号);
            }
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
            {
                return;
            }

            DataGridViewCellCollection cells = dataGridView1.Rows[e.RowIndex].Cells;

            lblBillStatus.Text = cells["状态"].Value.ToString();
            txtID.Text = cells["短信编号"].Value.ToString();
            txtContent.Text = cells["短信内容"].Value.ToString();
            txtDeclarePersonnel.Text = cells["编制人姓名"].Value.ToString();
            txtReceiver.Text = cells["接收人姓名"].Value.ToString();
            txtMobileNo.Text = cells["接收人手机号"].Value.ToString();
            txtRemark.Text = cells["备注"].Value.ToString();

            dtpkCreateTime.Value = (DateTime)cells["编制时间"].Value;
            dtpkSendTime.Value = (DateTime)cells["发送时间"].Value;

            cmbShortMsgType.Text = cells["短信类别"].Value.ToString();
        }

        /// <summary>
        /// 检查数据是否正确
        /// </summary>
        /// <returns>正确返回true</returns>
        private bool CheckData()
        {
            if (GlobalObject.GeneralFunction.IsNullOrEmpty(txtContent.Text))
            {
                MessageDialog.ShowPromptMessage("请输入短信内容");
                txtContent.Focus();
                return false;
            }

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(txtReceiver.Text))
            {
                MessageDialog.ShowPromptMessage("请输入接收人姓名");
                txtReceiver.Focus();
                return false;
            }

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(txtMobileNo.Text))
            {
                MessageDialog.ShowPromptMessage("请输入手机号码");
                txtMobileNo.Focus();
                return false;
            }

            if (!dtpkSendTime.Checked)
            {
                MessageDialog.ShowPromptMessage("请选择短信发送时间");
                dtpkSendTime.Focus();
                return false;
            }
            else
            {
                if (dtpkSendTime.Value < ServerTime.Time.AddMinutes(5))
                {
                    MessageDialog.ShowPromptMessage("短信发送时间必须是当前时间5分钟后");
                    dtpkSendTime.Focus();
                    return false;
                }
            }

            return true;
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

            if (DialogResult.Yes != MessageDialog.ShowEnquiryMessage("您真的要修改指定记录吗？"))
            {
                return;
            }

            BindingCollection<View_Task_ShortMessage> source = dataGridView1.DataSource as BindingCollection<View_Task_ShortMessage>;

            View_Task_ShortMessage msg = source.First(p => p.短信编号 == dataGridView1.SelectedRows[0].Cells["短信编号"].Value.ToString());

            if (msg != null && msg.编制人工号 != null && msg.编制人工号 != BasicInfo.LoginID)
            {
                MessageDialog.ShowPromptMessage("您不是此记录的编制人，不能进行此操作");
                return;
            }

            msg.短信内容 = txtContent.Text;
            msg.接收人手机号 = txtMobileNo.Text;
            msg.接收人姓名 = txtReceiver.Text;
            msg.发送时间 = dtpkSendTime.Value;
            msg.短信类别 = cmbShortMsgType.Text;

            try
            {
                m_shortMsgServer.Update(msg.短信编号, msg);

                MessageDialog.ShowPromptMessage("更新成功");

                RefreshData();

                PositioningRecord(msg.短信编号);
            }
            catch (Exception exce)
            {
                MessageDialog.ShowErrorMessage(exce.Message);
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

            List<string> lstID = new List<string>();

            for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
            {
                if (dataGridView1.SelectedRows[i].Cells["编制人工号"].Value.ToString() != BasicInfo.LoginID)
                {
                    MessageDialog.ShowPromptMessage("抱歉，只能删除您本人创建的短信！");
                    return;
                }

                if (dataGridView1.SelectedRows[i].Cells["状态"].Value.ToString() == ShortMessageStatus.已发送.ToString())
                {
                    continue;
                }

                lstID.Add(dataGridView1.SelectedRows[i].Cells["短信编号"].Value.ToString());
            }

            try
            {
                m_shortMsgServer.Delete(lstID);

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
        private List<View_Task_ShortMessage> GetFilterData(List<View_Task_ShortMessage> source)
        {
            if (source == null || source.Count == 0)
            {
                return source;
            }

            List<View_Task_ShortMessage> data = new List<View_Task_ShortMessage>();

            if (chk所有.Checked)
            {
                return source;
            }
            else
            {
                if (chk暂存.Checked)
                {
                    data = source.FindAll(p => p.状态 == "暂存");
                }

                if (chk待发送.Checked)
                {
                    if (data.Count == 0)
                        data = source.FindAll(p => p.状态 == "待发送");
                    else
                        data.AddRange(source.FindAll(p => p.状态 == "待发送"));
                }

                if (chk已发送.Checked)
                {
                    if (data.Count == 0)
                        data = source.FindAll(p => p.状态 == "已发送");
                    else
                        data.AddRange(source.FindAll(p => p.状态 == "已发送"));
                }
            }

            return data;
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

        /// <summary>
        /// 发布选择的
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPublish_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择要操作的记录后再进行此操作");
                return;
            }

            if (DialogResult.Yes != MessageDialog.ShowEnquiryMessage("您真的要发布指定记录吗？"))
            {
                return;
            }

            List<string> lstID = new List<string>();

            for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
            {
                if (dataGridView1.SelectedRows[i].Cells["编制人工号"].Value.ToString() != BasicInfo.LoginID)
                {
                    MessageDialog.ShowPromptMessage("抱歉，只能发布您本人创建的短信！");
                    return;
                }

                if (dataGridView1.SelectedRows[i].Cells["状态"].Value.ToString() == ShortMessageStatus.已发送.ToString())
                {
                    continue;
                }

                lstID.Add(dataGridView1.SelectedRows[i].Cells["短信编号"].Value.ToString());
            }

            try
            {
                m_shortMsgServer.Update(lstID, ShortMessageStatus.待发送);

                RefreshData();
            }
            catch (Exception exce)
            {
                MessageDialog.ShowErrorMessage(exce.Message);
            }
        }

        private void btn撤销_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择要操作的记录后再进行此操作");
                return;
            }

            if (DialogResult.Yes != MessageDialog.ShowEnquiryMessage("您真的要撤销发布的记录吗？"))
            {
                return;
            }

            List<string> lstID = new List<string>();

            for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
            {
                if (dataGridView1.SelectedRows[i].Cells["编制人工号"].Value.ToString() != BasicInfo.LoginID)
                {
                    MessageDialog.ShowPromptMessage("抱歉，只能撤销您本人创建的短信！");
                    return;
                }

                if (dataGridView1.SelectedRows[i].Cells["状态"].Value.ToString() == ShortMessageStatus.已发送.ToString())
                {
                    continue;
                }

                lstID.Add(dataGridView1.SelectedRows[i].Cells["短信编号"].Value.ToString());
            }

            try
            {
                m_shortMsgServer.Update(lstID, ShortMessageStatus.暂存);

                RefreshData();
            }
            catch (Exception exce)
            {
                MessageDialog.ShowErrorMessage(exce.Message);
            }
        }
    }
}