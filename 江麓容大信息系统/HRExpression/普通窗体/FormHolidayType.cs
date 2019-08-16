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
using GlobalObject;
using Expression;
using UniversalControlLibrary;

namespace Form_Peripheral_HR
{
    /// <summary>
    /// 设置节假日类别界面
    /// </summary>
    public partial class FormHolidayType : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string error;

        /// <summary>
        /// 可供查找的所有字段
        /// </summary>
        string[] m_findField = null;

        /// <summary>
        /// 节假日管理类
        /// </summary>
        IHolidayServer m_holidayServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IHolidayServer>();

        public FormHolidayType()
        {
            InitializeComponent();

            RefreshDataGridView();
        }

        /// <summary>
        /// 刷新
        /// </summary>
        void RefreshDataGridView()
        {
            DataTable dt = m_holidayServer.GetHolidayType();

            dataGridView1.DataSource = dt;

            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Columns["编号"].Visible = false;
            }

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

        /// <summary>
        /// 检查控件
        /// </summary>
        /// <returns>正确返回True，否则返回False</returns>
        bool CheckControl()
        {
            if (txtTypeName.Text.Trim() == "" || txtRemark.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请填写节假日名称和备注");
                return false;
            }

            //if (!cbIsLegalHolidays.Checked && !cbIsWeekend.Checked)
            //{
            //    if (MessageBox.Show("确定不是周末和法定节假日？", "消息", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            //    {
            //        return false;
            //    }
            //}

            if (!rbIsLegalHolidays.Checked && !rbIsWeekend.Checked && !rbRegular.Checked)
            {
                MessageDialog.ShowPromptMessage("请选择节假日类型！");
                return false;
            }

            return true;
        }

        private void 添加toolStripButton1_Click(object sender, EventArgs e)
        {
            if (!CheckControl())
            {
                return;
            }

            HR_HolidayType type = new HR_HolidayType();

            type.TypeName = txtTypeName.Text;
            type.IsWeekend = rbIsWeekend.Checked;
            type.IsLegalHolidays = rbIsLegalHolidays.Checked;
            type.Remark = txtRemark.Text;
            type.Recorder = BasicInfo.LoginID;
            type.RecordTime = ServerTime.Time;

            if (!m_holidayServer.AddHolidayType(type, out error))
            {
                MessageDialog.ShowPromptMessage(error);
                return;
            }

            RefreshDataGridView();
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtTypeName.Text = dataGridView1.CurrentRow.Cells["节假日名称"].Value.ToString();
            txtRemark.Text = dataGridView1.CurrentRow.Cells["备注"].Value.ToString();
            rbIsWeekend.Checked = Convert.ToBoolean(dataGridView1.CurrentRow.Cells["是否周末"].Value);
            rbIsLegalHolidays.Checked = Convert.ToBoolean(dataGridView1.CurrentRow.Cells["是否法定节假日"].Value);

            if (!rbIsWeekend.Checked && !rbIsLegalHolidays.Checked)
            {
                rbRegular.Checked = true;
            }
        }

        private void 修改toolStripButton2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择要修改的一行！");
                return;
            }

            if (dataGridView1.SelectedRows.Count > 1)
            {
                MessageDialog.ShowPromptMessage("只针对于单行操作！");
                return;
            }

            if (txtTypeName.Text.Trim() == dataGridView1.CurrentRow.Cells["节假日名称"].Value.ToString()
                && txtRemark.Text.Trim() == dataGridView1.CurrentRow.Cells["备注"].Value.ToString()
                && rbIsWeekend.Checked == Convert.ToBoolean(dataGridView1.CurrentRow.Cells["是否周末"].Value)
                && rbIsLegalHolidays.Checked == Convert.ToBoolean(dataGridView1.CurrentRow.Cells["是否法定节假日"].Value))
            {
                MessageDialog.ShowPromptMessage("数据没有任何改变！");
                return;
            }

            HR_HolidayType type = new HR_HolidayType();

            type.ID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["编号"].Value);
            type.TypeName = txtTypeName.Text;
            type.IsWeekend = rbIsWeekend.Checked;
            type.IsLegalHolidays = rbIsLegalHolidays.Checked;
            type.Remark = txtRemark.Text;
            type.Recorder = BasicInfo.LoginID;
            type.RecordTime = ServerTime.Time;

            if (!m_holidayServer.UpdateHolidayType(type, out error))
            {
                MessageDialog.ShowPromptMessage(error);
                return;
            }

            RefreshDataGridView();
        }

        private void 删除toolStripButton3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择要删除的一行！");
                return;
            }

            if (dataGridView1.SelectedRows.Count > 1)
            {
                MessageDialog.ShowPromptMessage("只针对于单行操作！");
                return;
            }

            if (MessageBox.Show("您确定要删除" + dataGridView1.CurrentRow.Cells["节假日名称"].Value.ToString() + "信息？", "消息",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (!m_holidayServer.DeleteHolidayType(Convert.ToInt32(dataGridView1.CurrentRow.Cells["编号"].Value), out error))
                {
                    MessageDialog.ShowPromptMessage(error);
                    return;
                }
            }

            RefreshDataGridView();
        }
    }
}
