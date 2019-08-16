using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GlobalObject;
using ServerModule;
using UniversalControlLibrary;

namespace Expression
{
    public partial class 借贷台帐 : Form
    {
        /// <summary>
        /// 借贷账存信息列表
        /// </summary>
        List<View_S_ProductLendRecord> m_listMachineAccount = new List<View_S_ProductLendRecord>();

        /// <summary>
        /// 借贷流水账信息列表
        /// </summary>
        List<View_S_ProductLendReturnDetail> m_listDayToDay = new List<View_S_ProductLendReturnDetail>();

        /// <summary>
        /// 借贷服务组件
        /// </summary>
        IProductLendReturnService m_serverLendReturn = ServerModuleFactory.GetServerModule<IProductLendReturnService>();

        public 借贷台帐()
        {
            InitializeComponent();

            DtpEndDate.Value = ServerTime.Time.AddDays(1);
            DtpStartDate.Value = DtpEndDate.Value.AddMonths(-1);

            m_listMachineAccount = m_serverLendReturn.GetListRecordInfo();
            m_listDayToDay = m_serverLendReturn.GetListDetailInfo();

            MachineAccount_RefreshDataGirdView(m_listMachineAccount);
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="source">数据集</param>
        void MachineAccount_RefreshDataGirdView(List<View_S_ProductLendRecord> source)
        {
            dataGridView_MachineAccount.DataSource = new BindingCollection<View_S_ProductLendRecord>(source);
            userControlDataLocalizer1.Init(dataGridView_MachineAccount, this.Name,
                UniversalFunction.SelectHideFields(this.Name, dataGridView_MachineAccount.Name, BasicInfo.LoginID));
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="source">数据集</param>
        void DayToDay_RefreshDataGirdView(List<View_S_ProductLendReturnDetail> source)
        {
            dataGridView_DayToDay.DataSource = new BindingCollection<View_S_ProductLendReturnDetail>(source);
            userControlDataLocalizer1.Init(dataGridView_DayToDay, this.Name,
                UniversalFunction.SelectHideFields(this.Name, dataGridView_DayToDay.Name, BasicInfo.LoginID));
        }

        private void txtGoodsName_OnCompleteSearch()
        {
            if (tabControl1.SelectedTab.Name == "tpMachineAccount")
            {
                txtGoodsName_MachineAccount.Text = txtGoodsName_MachineAccount.DataResult["物品名称"].ToString();
                txtGoodsCode_MachineAccount.Text = txtGoodsName_MachineAccount.DataResult["图号型号"].ToString();
                txtSpec_MachineAccount.Text = txtGoodsName_MachineAccount.DataResult["规格"].ToString();
                txtGoodsName_MachineAccount.Tag = txtGoodsName_MachineAccount.DataResult["序号"];
            }
            else if (tabControl1.SelectedTab.Name == "tpDayToDay")
            {
                txtGoodsName_DayToDay.Text = txtGoodsName_DayToDay.DataResult["物品名称"].ToString();
                txtGoodsCode_DayToDay.Text = txtGoodsName_DayToDay.DataResult["图号型号"].ToString();
                txtSpec_DayToDay.Text = txtGoodsName_DayToDay.DataResult["规格"].ToString();
                txtGoodsName_DayToDay.Tag = txtGoodsName_DayToDay.DataResult["序号"];
            }
        }

        private void btnFindCode_Click(object sender, EventArgs e)
        {
            FormQueryInfo form = QueryInfoDialog.GetPlanCostGoodsDialog();
            form.ShowColumns = new string[] { "图号型号", "物品名称", "规格", "物品类别名称", "序号" };

            if (form != null && DialogResult.OK == form.ShowDialog())
            {
                if (tabControl1.SelectedTab.Name == "tpMachineAccount")
                {
                    txtGoodsCode_MachineAccount.Text = form.GetDataItem("图号型号").ToString();
                    txtGoodsName_MachineAccount.Text = form.GetDataItem("物品名称").ToString();
                    txtSpec_MachineAccount.Text = form.GetDataItem("规格").ToString();
                    txtGoodsName_MachineAccount.Tag = form.GetDataItem("序号").ToString();
                }
                else if (tabControl1.SelectedTab.Name == "tpDayToDay")
                {
                    txtGoodsName_DayToDay.Text = form.GetDataItem("图号型号").ToString();
                    txtGoodsCode_DayToDay.Text = form.GetDataItem("物品名称").ToString();
                    txtSpec_DayToDay.Text = form.GetDataItem("规格").ToString();
                    txtGoodsName_DayToDay.Tag = form.GetDataItem("序号").ToString();
                }
            }
        }

        private void txtDebtor_OnCompleteSearch()
        {
            if (tabControl1.SelectedTab.Name == "tpMachineAccount")
            {
                txtDebtor_MachineAccount.Text = txtDebtor_MachineAccount.DataResult["部门名称"].ToString();
                txtDebtor_MachineAccount.Tag = txtDebtor_MachineAccount.DataResult["部门编码"].ToString();
            }
            else if (tabControl1.SelectedTab.Name == "tpDayToDay")
            {
                txtDebtor_DayToDay.Text = txtDebtor_DayToDay.DataResult["部门名称"].ToString();
                txtDebtor_DayToDay.Tag = txtDebtor_DayToDay.DataResult["部门编码"].ToString();
            }
        }

        private void txtCredit_OnCompleteSearch()
        {
            if (tabControl1.SelectedTab.Name == "tpMachineAccount")
            {
                txtCredit_MachineAccount.Text = txtCredit_MachineAccount.DataResult["部门名称"].ToString();
                txtCredit_MachineAccount.Tag = txtCredit_MachineAccount.DataResult["部门编码"].ToString();
            }
            else if (tabControl1.SelectedTab.Name == "tpDayToDay")
            {
                txtCredit_DayToDay.Text = txtCredit_DayToDay.DataResult["部门名称"].ToString();
                txtCredit_DayToDay.Tag = txtCredit_DayToDay.DataResult["部门编码"].ToString();
            }
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Name == "tpMachineAccount")
            {
                List<View_S_ProductLendRecord> listTemp = m_listMachineAccount;

                if (txtGoodsName_MachineAccount.Text.Trim().Length > 0)
                {
                    listTemp = (from a in listTemp
                                where a.物品ID == Convert.ToInt32(txtGoodsName_MachineAccount.Tag)
                                select a).ToList();
                }
                else if (txtDebtor_MachineAccount.Text.Trim().Length > 0)
                {
                    listTemp = (from a in listTemp
                                where a.借方代码 == txtDebtor_MachineAccount.Tag.ToString()
                                select a).ToList();
                }
                else if (txtCredit_MachineAccount.Text.Trim().Length > 0)
                {
                    listTemp = (from a in listTemp
                                where a.贷方代码 == txtDebtor_MachineAccount.Tag.ToString()
                                select a).ToList();
                }

                MachineAccount_RefreshDataGirdView(listTemp);
            }
            else if (tabControl1.SelectedTab.Name == "tpDayToDay")
            {
                List<View_S_ProductLendReturnDetail> listTemp = m_listDayToDay;

                listTemp = (from a in listTemp
                            where a.日期 >= DtpStartDate.Value && a.日期 <= DtpEndDate.Value
                            select a).ToList();

                if (txtGoodsName_DayToDay.Text.Trim().Length > 0)
                {
                    listTemp = (from a in listTemp
                                where a.物品ID == Convert.ToInt32(txtGoodsName_DayToDay.Tag)
                                select a).ToList();
                }
                else if (txtDebtor_DayToDay.Text.Trim().Length > 0)
                {
                    listTemp = (from a in listTemp
                                where a.借方代码 == txtDebtor_DayToDay.Tag.ToString()
                                select a).ToList();
                }
                else if (txtCredit_DayToDay.Text.Trim().Length > 0)
                {
                    listTemp = (from a in listTemp
                                where a.贷方代码 == txtCredit_DayToDay.Tag.ToString()
                                select a).ToList();
                }

                DayToDay_RefreshDataGirdView(listTemp);
            }
        }

        private void btnOutExcel_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Name == "tpMachineAccount")
            {
                ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView_MachineAccount);
            }
            else if (tabControl1.SelectedTab.Name == "tpDayToDay")
            {
                ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView_DayToDay);
            }
        }
    }
}
