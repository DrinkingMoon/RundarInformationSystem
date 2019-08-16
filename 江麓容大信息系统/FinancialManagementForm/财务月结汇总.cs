using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using UniversalControlLibrary;
using GlobalObject;
using Service_Economic_Financial;

namespace Form_Economic_Financial
{
    public partial class 财务月结汇总 : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_error;

        /// <summary>
        /// 财务月结
        /// </summary>
        IMonthProcServer m_monthProServer = Service_Economic_Financial.ServerModuleFactory.GetServerModule<IMonthProcServer>();

        /// <summary>
        /// 调整金额
        /// </summary>
        decimal m_adjust;

        /// <summary>
        /// 调整人
        /// </summary>
        string m_adjustor;

        /// <summary>
        /// 调整时间
        /// </summary>
        string m_adjustTime;

        /// <summary>
        /// 数据表
        /// </summary>
        DataTable m_dtGather = new DataTable();

        public 财务月结汇总()
        {
            InitializeComponent();

            cmbStorage.Items.Add("全部库房");

            //DataTable Dt = UniversalFunction.GetStorageTb();

            //for (int i = 0; i < Dt.Rows.Count; i++)
            //{
            //    cmbStorage.Items.Add(Dt.Rows[i]["StorageName"].ToString());
            //}

            cmbStorage.Text = "全部库房";
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();

                if (cmbStorage.Text == "全部库房")
                {
                    if (!m_monthProServer.GetAllGather("pro_B_GoodsListGather",
                        dtpStart.Value.Date, dtpEnd.Value.Date, "", out dt, out m_error))
                    {
                        MessageDialog.ShowErrorMessage(m_error);
                        return;
                    }
                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    m_dtGather = dt;
                }

                DataRow dr = dt.NewRow();

                dr["图号型号"] = "";
                dr["物品名称"] = "总  计";
                dr["规格"] = "";
                dr["物品类别名称"] = "";
                dr["上月结存数量"] = Convert.ToDecimal(dt.Compute("SUM(上月结存数量)", ""));

                dr["上月结存金额"] = Convert.ToDecimal(dt.Compute("SUM(上月结存金额)", ""));

                decimal temp = Convert.ToDecimal(dr["上月结存数量"]) == 0 ? 0
                    : Convert.ToDecimal(dr["上月结存金额"]) / Convert.ToDecimal(dr["上月结存数量"]);

                dr["上月结存单价"] = Math.Round(temp,10);


                dr["本月入库数量"] = Convert.ToDecimal(dt.Compute("SUM(本月入库数量)", ""));
                dr["本月入库金额"] = Convert.ToDecimal(dt.Compute("SUM(本月入库金额)", ""));
                dr["本月入库单价"] = Math.Round(Convert.ToDecimal(dr["本月入库数量"]) == 0 ? 0
                    : Convert.ToDecimal(dr["本月入库金额"]) / Convert.ToDecimal(dr["本月入库数量"]),10);

                dr["本月出库数量"] = Convert.ToDecimal(dt.Compute("SUM(本月出库数量)", ""));
                dr["本月出库金额"] = Convert.ToDecimal(dt.Compute("SUM(本月出库金额)", ""));
                dr["本月出库单价"] = Math.Round(Convert.ToDecimal(dr["本月出库数量"]) == 0 ? 0
                    : Convert.ToDecimal(dr["本月出库金额"]) / Convert.ToDecimal(dr["本月出库数量"]),10);

                dr["本月结存数量"] = Convert.ToDecimal(dt.Compute("SUM(本月结存数量)", ""));
                dr["本月结存金额"] = Convert.ToDecimal(dt.Compute("SUM(本月结存金额)", ""));
                dr["本月结存单价"] = Math.Round(Convert.ToDecimal(dr["本月结存数量"]) == 0 ? 0
                    : Convert.ToDecimal(dr["本月结存金额"]) / Convert.ToDecimal(dr["本月结存数量"]),10);

                dr["财务调整金额"] = Convert.ToDecimal(dt.Compute("SUM(财务调整金额)", ""));
                dr["本月实际结存金额"] = Convert.ToDecimal(dt.Compute("SUM(本月实际结存金额)", ""));

                dt.Rows.Add(dr);

                dataGridView1.DataSource = dt;
                userControlDataLocalizer1.Init(dataGridView1, this.Name,
                    UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));

                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Columns["物品ID"].Visible = false;
                    dataGridView1.Columns[0].ReadOnly = true;
                    dataGridView1.Columns[1].ReadOnly = true;
                    dataGridView1.Columns[2].ReadOnly = true;
                    dataGridView1.Columns[3].ReadOnly = true;
                    dataGridView1.Columns[4].ReadOnly = true;
                    dataGridView1.Columns[5].ReadOnly = true;
                    dataGridView1.Columns[6].ReadOnly = true;
                    dataGridView1.Columns[7].ReadOnly = true;
                    dataGridView1.Columns[8].ReadOnly = true;
                    dataGridView1.Columns[9].ReadOnly = true;
                    dataGridView1.Columns[10].ReadOnly = true;
                    dataGridView1.Columns[11].ReadOnly = true;
                    dataGridView1.Columns[12].ReadOnly = true;
                    dataGridView1.Columns[13].ReadOnly = true;
                    dataGridView1.Columns[14].ReadOnly = true;
                    dataGridView1.Columns[15].ReadOnly = true;
                    dataGridView1.Columns[16].ReadOnly = true;
                    dataGridView1.Columns[18].ReadOnly = true;
                    dataGridView1.Columns[19].ReadOnly = true;
                    dataGridView1.Columns[20].ReadOnly = true;

                    //dataGridView1.Columns[17].Name  财务调整金额
                }

                dataGridView1.Columns[0].Frozen = true;
                dataGridView1.Columns[1].Frozen = true;
                dataGridView1.Columns[2].Frozen = true;
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;
            string yearAndMonth = dataGridView1.Rows[1].Cells["年月"].Value.ToString();

            if (dt != null && dt.Rows.Count > 0)
            {
                bool b = false;

                for (int i = 0; i < m_dtGather.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        if (m_dtGather.Rows[i]["物品ID"] == dt.Rows[j]["物品ID"])
                        {
                            if (m_dtGather.Rows[i]["财务调整金额"] != dt.Rows[j]["财务调整金额"])
                            {
                                b = true;
                                break;
                            }
                        }
                    }

                    if (b)
                    {
                        break;
                    }
                }

                if (!b)
                {
                    MessageDialog.ShowPromptMessage("没有数据变更，无需保存！");
                }
                else
                {
                    if (!m_monthProServer.UpdateGather(yearAndMonth, dt, out m_error))
                    {
                        MessageDialog.ShowPromptMessage(m_error);
                    }
                    else
                    {
                        MessageDialog.ShowPromptMessage("保存成功！");
                        m_dtGather = dt;
                    }
                }
            }
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                m_adjust = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["财务调整金额"].Value);
                m_adjustor = dataGridView1.CurrentRow.Cells["财务调整人"].Value.ToString();
                m_adjustTime = dataGridView1.CurrentRow.Cells["调整日期"].Value.ToString();
            }
        }

        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView1.IsCurrentCellDirty)
            {
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (m_adjust != Convert.ToDecimal(dataGridView1.CurrentRow.Cells["财务调整金额"].Value))
                {
                    dataGridView1.CurrentRow.Cells["本月实际结存金额"].Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["本月结存金额"].Value)
                               + Convert.ToDecimal(dataGridView1.Rows[e.RowIndex].Cells["财务调整金额"].Value);

                    dataGridView1.CurrentRow.Cells["财务调整人"].Value = BasicInfo.LoginName;
                    dataGridView1.CurrentRow.Cells["调整日期"].Value = ServerTime.Time;
                }
                else
                {
                    dataGridView1.CurrentRow.Cells["财务调整人"].Value = m_adjustor;
                    dataGridView1.CurrentRow.Cells["调整日期"].Value = m_adjustTime;
                }
            }
            catch (Exception)
            {
                dataGridView1.CurrentRow.Cells["本月实际结存金额"].Value = 0;
            }
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }
    }
}
