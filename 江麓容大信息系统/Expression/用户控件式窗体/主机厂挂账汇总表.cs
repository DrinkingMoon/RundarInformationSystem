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
using Form_Economic_Financial;

namespace Expression
{
    /// <summary>
    /// 主机厂挂账汇总表界面
    /// </summary>
    public partial class 主机厂挂账汇总表 : Form
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

        public 主机厂挂账汇总表(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            for (int i = 2010; i < 2050; i++)
            {
                cmbYear.Items.Add(i);
            }

            for (int f = 1; f <= 12; f++)
            {
                cmbMonth.Items.Add(f.ToString("D2"));
            }

            cmbYear.Text = ServerTime.Time.Year.ToString();
            cmbMonth.Text = ServerTime.Time.Month.ToString("D2");
        }

        private void 主机厂与系统的零件匹配设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            主机厂与系统零件匹配设置 form = new 主机厂与系统零件匹配设置();
            form.ShowDialog();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            DataTable dt = m_serverComReportBill.GetSignTheBill(cmbYear.Text + cmbMonth.Text,txtClient.Tag.ToString());

            if (dt != null )
            {
                DataRow dr = dt.NewRow();

                dr["物品名称"] = "合    计";
                dr["上期未挂账数"] = Convert.ToDecimal(dt.Compute("Sum(上期未挂账数)", ""));
                dr["本期送货数"] = Convert.ToDecimal(dt.Compute("Sum(本期送货数)", ""));
                dr["本期送货金额"] = Convert.ToDecimal(dt.Compute("Sum(本期送货金额)", ""));
                dr["本期退货数"] = Convert.ToDecimal(dt.Compute("Sum(本期退货数)", ""));
                dr["本期退货金额"] = Convert.ToDecimal(dt.Compute("Sum(本期退货金额)", ""));
                dr["实挂数量"] = Convert.ToDecimal(dt.Compute("Sum(实挂数量)", ""));
                dr["实挂金额"] = Convert.ToDecimal(dt.Compute("Sum(实挂金额)", ""));
                dr["本期未挂账数"] = Convert.ToDecimal(dt.Compute("Sum(本期未挂账数)", ""));

                dt.Rows.Add(dr);

                dataGridView1.DataSource = dt;

                userControlDataLocalizer1.Init(dataGridView1, this.Name,
                    UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
            }
        }

        private void btnIn_Click(object sender, EventArgs e)
        {
            主机厂报表操作指示窗体 form = new 主机厂报表操作指示窗体();
            form.ShowDialog();

            if (!form.BlFlag)
            {
                return;
            }

            DataTable dtTemp = ExcelHelperP.RenderFromExcel(openFileDialog1);

            if (dtTemp == null)
            {
                //MessageDialog.ShowPromptMessage(m_strErr);
                return;
            }

            if (dtTemp.Rows.Count == 0 ||
                dtTemp.Columns[0].ColumnName != "图号型号" ||
                dtTemp.Columns[1].ColumnName != "物品名称" ||
                dtTemp.Columns[2].ColumnName != "协议单价" ||
                dtTemp.Columns[3].ColumnName != "实挂数量")
            {
                MessageDialog.ShowPromptMessage(string.Format("{0} 中没有包含所需的信息，无法导入！",
                    openFileDialog1.FileName));
            }
            else
            {
                if (!m_serverComReportBill.UpdateSignTheBill(form.StrCommunicate, dtTemp, out m_strErr))
                {
                    MessageDialog.ShowPromptMessage(m_strErr);
                }
                else
                {
                    MessageDialog.ShowPromptMessage("导入成功");
                }
            }
        }

        private void btnOut_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            主机厂报表操作指示窗体 form = new 主机厂报表操作指示窗体();
            form.ShowDialog();

            if (!form.BlFlag)
            {
                return;
            }

            m_serverComReportBill.SetSingTheBillPrice(form.StrNy, form.StrCommunicate);
        }

        private void txtClient_OnCompleteSearch()
        {
            txtClient.Tag = txtClient.DataResult["客户编码"].ToString();
            txtClient.Text = txtClient.DataResult["客户名称"].ToString();
        }
    }
}
