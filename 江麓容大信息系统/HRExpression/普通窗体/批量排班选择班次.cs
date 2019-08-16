using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Service_Peripheral_HR;
using Expression;
using UniversalControlLibrary;

namespace Form_Peripheral_HR
{
    public partial class 批量排班选择班次 : Form
    {
        /// <summary>
        /// 选择的排班编码
        /// </summary>
        string m_scheduleCode;

        /// <summary>
        /// 选择的排班编码
        /// </summary>
        public string ScheduleCode
        {
            get { return m_scheduleCode; }
            set { m_scheduleCode = value; }
        }

        /// <summary>
        /// 排班服务类
        /// </summary>
        IWorkSchedulingServer m_workSchedulingServer = ServerModuleFactory.GetServerModule<IWorkSchedulingServer>();

        public 批量排班选择班次()
        {
            InitializeComponent();

            DataTable dt = m_workSchedulingServer.GetWorkSchedulingDefinition();

            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cmbSchedule.Items.Add(dt.Rows[i]["定义编码"].ToString() + " " + dt.Rows[i]["定义名称"].ToString());
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cmbSchedule.SelectedIndex >= 0)
            {
                m_scheduleCode = cmbSchedule.Text.Split(' ')[0];

                this.Close();
            }
            else
            {
                MessageDialog.ShowPromptMessage("请选择需排班的班次！");
                return;
            }
        }
    }
}
