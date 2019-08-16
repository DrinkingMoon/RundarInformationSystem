using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using PlatformManagement;
using GlobalObject;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 供应商质量信息反馈单界面
    /// </summary>
    public partial class 供应质量信息反馈单 : Form
    {
        /// <summary>
        /// 数据集
        /// </summary>
        S_MessMessageFeedback m_lnqMess = new S_MessMessageFeedback();

        /// <summary>
        /// 可供查找的所有字段
        /// </summary>
        string[] m_findField = null;

        /// <summary>
        /// 查找条件窗体
        /// </summary>
        FormConditionFind m_formFindCondition;

        /// <summary>
        /// 数据定位控件
        /// </summary>
        UserControlDataLocalizer m_dataLocalizer;

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 服务类
        /// </summary>
        IMessMessageFeedback m_serverMess = ServerModuleFactory.GetServerModule<IMessMessageFeedback>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 单据号
        /// </summary>
        string m_strDJH;

        /// <summary>
        /// 标志
        /// </summary>
        bool m_blFlag = false;

        public 供应质量信息反馈单(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authFlag = nodeInfo.Authority;

            dtpStartTime.Value = new DateTime(ServerTime.Time.Year, ServerTime.Time.Month, 1);
            dtpEndTime.Value = ServerTime.Time.AddDays(1);
            cmbBillStatus.Text = "全  部";

            RefreshDataGirdView(m_serverMess.GetAllData(cmbBillStatus.Text, dtpStartTime.Value, dtpEndTime.Value));
        }

        private void 供应质量信息反馈单_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authFlag);
        }

        /// <summary>
        /// 权限控制
        /// </summary>
        /// <param name="authorityFlag">权限标志</param>
        void AuthorityControl(PlatformManagement.AuthorityFlag authorityFlag)
        {
            FaceAuthoritySetting.SetVisibly(menuStrip, authorityFlag);
            FaceAuthoritySetting.SetEnable(this.Controls, authorityFlag);
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
                if ((string)dataGridView1.Rows[i].Cells["单据号"].Value == billNo)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                    break;
                }
            }
        }

        /// <summary>
        /// 提交数据
        /// </summary>
        /// <param name="strFlag">提交标志</param>
        private void Update(string strFlag)
        {
            if (m_lnqMess == null)
            {
                MessageDialog.ShowPromptMessage("请填写并保存数据后再提交！");
                return;
            }

            if (m_lnqMess.DJZT != "单据已完成" && m_lnqMess.DJZT != "单据已报废")
            {
                if (!m_serverMess.UpdateData(m_lnqMess,strFlag,out m_err))
                {
                    MessageDialog.ShowErrorMessage(m_err);
                }
                else
                {
                    MessageBox.Show("成功提交","提示");
                }
            }
            else
            {
                MessageBox.Show("请重新确认单据状态","提示");
            }

            RefreshDataGirdView(m_serverMess.GetAllData(cmbBillStatus.Text, dtpStartTime.Value, dtpEndTime.Value));
            PositioningRecord(m_lnqMess.DJH);
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="source">数据集</param>
        void RefreshDataGirdView(DataTable source)
        {
            dataGridView1.DataSource = source;

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

            if (m_dataLocalizer == null)
            {
                m_dataLocalizer = new UserControlDataLocalizer(dataGridView1, this.Name, 
                    UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));

                panelPara.Controls.Add(m_dataLocalizer);
                m_dataLocalizer.Dock = DockStyle.Bottom;
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            m_strDJH = dataGridView1.CurrentRow.Cells["单据号"].Value.ToString();

            if (m_strDJH != "" || m_strDJH != null)
            {
                质量信息反馈单 form = new 质量信息反馈单(m_strDJH);

                form.ShowDialog();
                m_lnqMess = form.LnqMess;
                m_blFlag = false;
            }
        }

        private void 提交单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "等待STA意见")
            {
                Update("不合格品信息");
            }
            else
            {
                MessageDialog.ShowErrorMessage("请重新确认单据状态");
            }
            
        }

        private void sQE意见提交ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "等待STA意见"
                || dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "等待质管部意见")
            {
                Update("STA意见");
            }
            else
            {
                MessageDialog.ShowErrorMessage("请重新确认单据状态");
            }
        }

        private void 验证结果提交ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "等待SQE验证"
                || dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "等待质管部确认")
            {
                Update("SQE验证");
            }
            else
            {
                MessageDialog.ShowErrorMessage("请重新确认单据状态");
            }
        }

        private void 确认结果提交ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "等待质管部确认")
            {
                Update("质管部确认");
            }
            else
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
                PositioningRecord(m_lnqMess.DJH);

                return;
            }
        }

        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshDataGirdView(m_serverMess.GetAllData(cmbBillStatus.Text, dtpStartTime.Value, dtpEndTime.Value));
        }

        private void 导出EXCEL表单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_formFindCondition != null && !m_formFindCondition.SaveFlag)
            {
                m_formFindCondition = null;
            }

            if (m_formFindCondition == null)
            {
                m_formFindCondition = new FormConditionFind(this, m_findField, labelTitle.Text, labelTitle.Text);
            }

            m_formFindCondition.ShowDialog();
        }

        private void 回退单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "等待STA意见" && sQE操作ToolStripMenuItem.Visible == true)
            {
                ReturnBillStatus();
            }

            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "等待质管部意见" && qE操作ToolStripMenuItem.Visible == true)
            {
                ReturnBillStatus();
            }

            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "等待SQE验证" && sQE操作ToolStripMenuItem.Visible == true)
            {
                ReturnBillStatus();
            }

            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "等待质管部确认" && qE操作ToolStripMenuItem.Visible == true)
            {
                ReturnBillStatus();
            }
        }

        private void ReturnBillStatus()
        {
            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() != "单据已完成"
                && dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() != "单据已报废")
            {
                回退单据 form = new 回退单据(CE_BillTypeEnum.供应质量信息反馈单, dataGridView1.CurrentRow.Cells["单据号"].Value.ToString(), 
                    dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString());

                if (form.ShowDialog() == DialogResult.OK)
                {
                    if (MessageBox.Show("您确定要回退此单据吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        if (m_serverMess.ReturnData(form.StrBillID,
                            form.StrBillStatus, out m_err, form.Reason))
                        {
                            MessageDialog.ShowPromptMessage("回退成功");

                        }
                        else
                        {
                            MessageDialog.ShowPromptMessage(m_err);
                        }
                    }

                    RefreshDataGirdView(m_serverMess.GetAllData(cmbBillStatus.Text, dtpStartTime.Value, dtpEndTime.Value));
                    PositioningRecord(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString());
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            RefreshDataGirdView(m_serverMess.GetAllData(cmbBillStatus.Text, dtpStartTime.Value, dtpEndTime.Value));
        }

        private void 新建单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            质量信息反馈单 form = new 质量信息反馈单("");

            form.ShowDialog();
            m_blFlag = true;
            m_lnqMess = form.LnqMess;
        }

        private void 提交新建单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_lnqMess != null && m_blFlag)
            {
                m_lnqMess.QCRQ = ServerTime.Time;
                m_lnqMess.SQERQ = ServerTime.Time;
                m_lnqMess.SQERY = BasicInfo.LoginID;

                if (!m_serverMess.InsertData(m_lnqMess,out m_err))
                {
                    MessageDialog.ShowPromptMessage(m_err);
                }
                else
                {
                    MessageDialog.ShowPromptMessage("提交成功!");
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
            }

            RefreshDataGirdView(m_serverMess.GetAllData(cmbBillStatus.Text, dtpStartTime.Value, dtpEndTime.Value));
            PositioningRecord(m_lnqMess.DJH);
        }

        private void 删除单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() != "单据已完成")
            {

                if (MessageDialog.ShowEnquiryMessage("您是否要删除此单据") == DialogResult.No)
                {
                    return;
                }

                if (!m_serverMess.ScarpData(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString(),out m_err))
                {
                    MessageDialog.ShowPromptMessage(m_err);
                }
                else
                {
                    MessageDialog.ShowPromptMessage("报废成功!");
                }
            }

            RefreshDataGirdView(m_serverMess.GetAllData(cmbBillStatus.Text, dtpStartTime.Value, dtpEndTime.Value));
            PositioningRecord(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString());
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                e.RowBounds.Location.Y,
                dataGridView1.RowHeadersWidth - 4,
                e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                dataGridView1.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dataGridView1.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }
    }
}
