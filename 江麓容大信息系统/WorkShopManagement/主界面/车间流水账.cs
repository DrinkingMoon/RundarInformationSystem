using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Service_Manufacture_WorkShop;
using UniversalControlLibrary;
using ServerModule;

namespace Form_Manufacture_WorkShop
{
    public partial class 车间流水账 : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strError = "";

        /// <summary>
        /// 车间管理基础信息服务组件
        /// </summary>
        IWorkShopBasic m_serverWSBasic = Service_Manufacture_WorkShop.ServerModuleFactory.GetServerModule<IWorkShopBasic>();

        /// <summary>
        /// 车间管理基础信息服务组件
        /// </summary>
        IWorkShopStock m_serverWSStock = Service_Manufacture_WorkShop.ServerModuleFactory.GetServerModule<IWorkShopStock>();

        public 车间流水账()
        {
            InitializeComponent();

            DataTable tempTable = m_serverWSBasic.GetWorkShopBasicInfo();

            DataRow tempRow = tempTable.NewRow();

            tempRow["车间名称"] = "所有车间";
            tempRow["车间编码"] = "";
            tempTable.Rows.Add(tempRow);

            cmbWSCode.DataSource = tempTable;

            cmbWSCode.DisplayMember = "车间名称";
            cmbWSCode.ValueMember = "车间编码";

            DtpStartDate.Value = ServerTime.Time.AddMonths(-1);
            DtpEndDate.Value = ServerTime.Time;
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            DataTable dtResult = m_serverWSStock.QueryRunningAccount(Convert.ToInt32(txtCode.Tag), 
                txtBatchNo.Text.Trim(),cmbWSCode.SelectedValue.ToString(), 
                DtpStartDate.Value, 
                DtpEndDate.Value, out m_strError);

            if (dtResult == null)
            {
                MessageDialog.ShowPromptMessage(m_strError);
            }
            else
            {

                if (dtResult.Rows.Count == 0)
                {
                    label9.Text = "无数据";
                    MessageDialog.ShowPromptMessage("无数据");
                }
                else
                {
                    object obj = dtResult.Compute("Sum(数量)", "");
                    dtResult.Rows[dtResult.Rows.Count - 1]["数量"] = obj.ToString() == "" ? 0 : obj;

                    if ((Convert.ToDecimal(dtResult.Rows[0]["结存"]) + 
                        Convert.ToDecimal(dtResult.Rows[dtResult.Rows.Count - 1]["数量"])) !=
                        Convert.ToDecimal(dtResult.Rows[dtResult.Rows.Count - 1 ]["结存"]))
                    {
                        label9.Text = "此记录账目错误";
                    }
                    else
                    {
                        label9.Text = "此记录账目正确";
                    }
                }

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

        private void txtBatchNo_Enter(object sender, EventArgs e)
        {
            if (txtCode.Tag != null)
            {
                txtBatchNo.StrEndSql = " and 物品ID = " + txtCode.Tag.ToString();
            }
        }
    }
}
