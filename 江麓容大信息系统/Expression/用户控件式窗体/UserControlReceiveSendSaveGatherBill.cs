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
using Service_Economic_Financial;

namespace Expression
{
    /// <summary>
    /// 材料收发存汇总表组件
    /// </summary>
    public partial class UserControlReceiveSendSaveGatherBill : Form
    {
        /// <summary>
        /// 收发存服务组件
        /// </summary>
        IGatherBillAndDetailBillServer m_findGatherBill = Service_Economic_Financial.ServerModuleFactory.GetServerModule<IGatherBillAndDetailBillServer>();

        public UserControlReceiveSendSaveGatherBill()
        {
            InitializeComponent();

            DataTable dt = UniversalFunction.GetStorageTb();

            cmbStorage.DataSource = dt;

            cmbStorage.ValueMember = "StorageID";
            cmbStorage.DisplayMember = "StorageName";

            cmbStorage.SelectedIndex = -1;

            for (int i = 0; i < 20; i++)
            {
                cmbYear.Items.Add((2012 + i).ToString());
            }

            DataTable dt1 = UniversalFunction.GetStorageTb();

            DataRow dr = dt1.NewRow();

            dr["StorageID"] = "00";
            dr["StorageName"] = "系统库房";

            dt1.Rows.Add(dr);

            cmbDG.DataSource = dt1;
            cmbDG.ValueMember = "StorageID";
            cmbDG.DisplayMember = "StorageName";

            cmbDG.SelectedIndex = -1;
        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (GlobalObject.GeneralFunction.IsNullOrEmpty(cmbYear.Text))
                {
                    throw new Exception("请选择【年份】");
                }

                if (GlobalObject.GeneralFunction.IsNullOrEmpty(cmbMonth.Text))
                {
                    throw new Exception("请选择【月份】");
                }

                if (rbSingleStorage.Checked && GlobalObject.GeneralFunction.IsNullOrEmpty(cmbStorage.Text))
                {
                    throw new Exception("请选择需要查询的【库房】");
                }

                string yearMonth = cmbYear.Text + cmbMonth.Text;

                Service_Economic_Financial.IBasicParametersSetting serviceSetting = Service_Economic_Financial.ServerModuleFactory.GetServerModule<IBasicParametersSetting>();


                string storageID = rbSingleStorage.Checked ? cmbStorage.SelectedValue.ToString() : "00";

                foreach (TabPage tp in tabControl2.TabPages)
                {
                    foreach (Control cl in tp.Controls)
                    {
                        if (cl is CustomDataGridView)
                        {
                            if (serviceSetting.GetCount("财务月结", yearMonth).Count() == 0)
                            {
                                ((CustomDataGridView)cl).DataSource = null;
                                return;
                            }

                            ((CustomDataGridView)cl).DataSource = m_findGatherBill.GetMonthlyBalanceInfo(yearMonth, tp.Text, storageID);

                            foreach (Control clusdl in tp.Controls)
                            {
                                if (clusdl is UniversalControlLibrary.UserControlDataLocalizer)
                                {
                                    ((UniversalControlLibrary.UserControlDataLocalizer)clusdl).Init((CustomDataGridView)cl, this.Name, null);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowErrorMessage(ex.Message);
            }
        }

        private void btnOutExcel_Click(object sender, EventArgs e)
        {
            foreach (Control cl in tabControl1.SelectedTab.Controls)
            {
                if (cl is TabControl)
                {
                    foreach (Control clSon in ((TabControl)cl).SelectedTab.Controls)
                    {
                        if (clSon is CustomDataGridView)
                        {
                            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, (DataGridView)clSon);
                        }
                    }
                }
                else if (cl is CustomDataGridView)
                {
                    ExcelHelperP.DatagridviewToExcel(saveFileDialog1, (DataGridView)cl);
                }
            }
        }

        private void rbSingleStorage_CheckedChanged(object sender, EventArgs e)
        {
            cmbStorage.Enabled = rbSingleStorage.Checked;
        }

        private void rbDG_CheckedChanged(object sender, EventArgs e)
        {
            cmbDG.Enabled = rbDG.Checked;
        }

        private void btnOutput_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, (DataGridView)dgv_BusDetail);
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                if (GlobalObject.GeneralFunction.IsNullOrEmpty(cmbSelectType.Text))
                {
                    throw new Exception("请选择【查询类型】");
                }

                if (rbDG.Checked && GlobalObject.GeneralFunction.IsNullOrEmpty(cmbDG.Text))
                {
                    throw new Exception("请选择【库房】");
                }

                if (dtpEndTime.Value < dtpStartTime.Value)
                {
                    throw new Exception("【起始时间】必须大于【截止时间】");
                }

                string storageID = null;

                if (rbZW.Checked)
                {
                    storageID = "";
                }
                else if(rbDG.Checked)
                {
                    storageID = cmbDG.SelectedValue.ToString();
                }

                dgv_BusDetail.DataSource = 
                    m_findGatherBill.GetBusDetailInfo(cmbSelectType.Text, dtpStartTime.Value.Date, dtpEndTime.Value.Date, storageID);
                ucdlBusDetail.Init(this.dgv_BusDetail, this.Name, null);

            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void UserControlReceiveSendSaveGatherBill_Load(object sender, EventArgs e)
        {
            string[] fileNameAccount = new string[] { "年月", "图号型号", "物品名称", "规格", "供货单位", 
                "批次号", "库房名称", "物品ID", "实际单价", "实际金额", "入库时间" , "物品状态",
                "材料类别编码", "材料类别名称"};

            FormConditionFind fcfAccount = new FormConditionFind(this.月度结存, fileNameAccount, this.月度结存.Name, this.Name + this.月度结存.Name);

            fcfAccount.TopLevel = false;
            fcfAccount.Dock = DockStyle.Fill;
            fcfAccount.FormBorderStyle = FormBorderStyle.None;
            fcfAccount.Show();

            fcfAccount.Parent = this.月度结存;
        }

    }
}
