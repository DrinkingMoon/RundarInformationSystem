using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using GlobalObject;
using Expression;
using PlatformManagement;
using Service_Peripheral_External;
using UniversalControlLibrary;

namespace Form_Peripheral_External
{
    public partial class 外部流水账 : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strError = "";

        /// <summary>
        /// 车间管理基础信息服务组件
        /// </summary>
        IBusinessOperation m_serverWSBasic = Service_Peripheral_External.ServerModuleFactory.GetServerModule<IBusinessOperation>();

        public 外部流水账()
        {
            InitializeComponent();

            DtpStartDate.Value = ServerTime.Time.AddMonths(-1);
            DtpEndDate.Value = ServerTime.Time;
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            DataTable dtResult = m_serverWSBasic.QueryRunningAccount(Convert.ToInt32(txtCode.Tag), txtStorage.Tag == null ? "" : txtStorage.Tag.ToString(),
                DtpStartDate.Value, DtpEndDate.Value, out m_strError);

            if (dtResult == null)
            {
                if (dataGridView1.DataSource != null)
                {
                    dataGridView1.DataSource = ((DataTable)dataGridView1.DataSource).Clone();
                }

                MessageDialog.ShowPromptMessage(m_strError);
            }
            else
            {
                dataGridView1.DataSource = dtResult;
            }
        }

        private void btnOutExcel_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
        }

        private void txtCode_OnCompleteSearch()
        {
            txtCode.Text = txtCode.DataResult["图号型号"].ToString();
            txtCode.Tag = txtCode.DataResult["序号"];
            txtName.Text = txtCode.DataResult["物品名称"].ToString();
            txtSpec.Text = txtCode.DataResult["规格"].ToString();
        }

        private void txtStorage_OnCompleteSearch()
        {
            txtStorage.Tag = txtStorage.DataResult["编码"].ToString();
        }
    }
}
