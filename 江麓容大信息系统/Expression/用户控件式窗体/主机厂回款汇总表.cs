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
    /// 主机厂回款汇总表界面
    /// </summary>
    public partial class 主机厂回款汇总表 : Form
    {
        /// <summary>
        /// 客户信息服务组件
        /// </summary>
        IClientServer m_serverClient = ServerModuleFactory.GetServerModule<IClientServer>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr = "";

        /// <summary>
        /// 主机厂报表服务组件
        /// </summary>
        ICommunicateReportBill m_serverComReportBill = ServerModuleFactory.GetServerModule<ICommunicateReportBill>();

        public 主机厂回款汇总表(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            for (int i = 2010; i < 2050; i++)
            {
                cmbYear.Items.Add(i);
                comboBox2.Items.Add(i);
            }

            for (int f = 1; f <= 12; f++)
            {
                cmbMonth.Items.Add(f.ToString("D2"));
                comboBox1.Items.Add(f.ToString("D2"));
            }

            cmbYear.Text = ServerTime.Time.Year.ToString();
            comboBox2.Text = ServerTime.Time.Year.ToString();
            cmbMonth.Text = "01";
            comboBox1.Text = ServerTime.Time.Month.ToString("D2");

        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (txtClient.Tag != null)
            {
                DataTable dt = m_serverComReportBill.GetCommunicateReturnedMoneyBill(cmbYear.Text + cmbMonth.Text,
                    comboBox2.Text + comboBox1.Text, txtClient.Tag.ToString());

                if (dt != null)
                {
                    DataRow dr = dt.NewRow();

                    dr["年月"] = "合计";
                    dr["开票"] = Convert.ToDecimal(dt.Compute("Sum(开票)", ""));
                    dr["挂账"] = Convert.ToDecimal(dt.Compute("Sum(挂账)", ""));
                    dr["应回款"] = Convert.ToDecimal(dt.Compute("Sum(应回款)", ""));
                    dr["回款"] = Convert.ToDecimal(dt.Compute("Sum(回款)", ""));
                    dr["到期未回款"] = Convert.ToDecimal(dt.Compute("Sum(到期未回款)", ""));
                    dr["未到期"] = Convert.ToDecimal(dt.Compute("Sum(未到期)", ""));

                    dt.Rows.Add(dr);

                    dataGridView1.DataSource = dt;

                    userControlDataLocalizer1.Init(dataGridView1, this.Name,
                        UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请选择主机厂！");
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            主机厂报表操作指示窗体 form = new 主机厂报表操作指示窗体();

            form.ShowDialog();

            if (!form.BlFlag)
            {
                return;
            }

            if (!m_serverComReportBill.SetReturnedMoney(form.StrNy, form.StrCommunicate, numericUpDown1.Value, out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
                return;
            }
            else
            {
                MessageDialog.ShowPromptMessage("提交成功");
            }
        }

        private void txtClient_OnCompleteSearch()
        {
            txtClient.Tag = txtClient.DataResult["客户编码"].ToString();
            txtClient.Text = txtClient.DataResult["客户名称"].ToString();
        }
    }
}
