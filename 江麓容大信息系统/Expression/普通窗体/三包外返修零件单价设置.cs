using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using ServerModule;
using GlobalObject;
using System.Data.OleDb;
using UniversalControlLibrary;


namespace Expression
{
    /// <summary>
    /// 三包外返修零件单价设置界面
    /// </summary>
    public partial class 三包外返修零件单价设置 : Form
    {
        /// <summary>
        /// 基础物品服务组件
        /// </summary>
        IBasicGoodsServer m_serverBasicGoods = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();

        /// <summary>
        /// 数据集
        /// </summary>
        YX_ThreePacketsOfTheRepairGoodsUnitPrice m_lnqThreePackets = new YX_ThreePacketsOfTheRepairGoodsUnitPrice();

        /// <summary>
        /// 三包外返修服务组件
        /// </summary>
        IThreePacketsOfTheRepairBill m_serverThreePacketsOfTheRepairBill = 
            ServerModuleFactory.GetServerModule<IThreePacketsOfTheRepairBill>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr;

        public 三包外返修零件单价设置()
        {
            InitializeComponent();

            dataGridView1.DataSource = m_serverThreePacketsOfTheRepairBill.GetGoodsUnitPriceInfo();

            userControlDataLocalizer1.Init(dataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
        }

        /// <summary>
        /// 获得信息
        /// </summary>
        void GetMessage()
        {
            m_lnqThreePackets.GoodsID = Convert.ToInt32(txtName.Tag);
            m_lnqThreePackets.UnitPrice = numPrice.Value;
        }

        private void btnIn_Click(object sender, EventArgs e)
        {
            DataTable dtTemp = ExcelHelperP.RenderFromExcel(openFileDialog1);

            if (dtTemp == null)
            {
                //MessageDialog.ShowPromptMessage(m_strErr);
                return;
            }

            if (dtTemp.Rows.Count == 0 ||
                dtTemp.Columns[0].ColumnName != "物品ID" ||
                dtTemp.Columns[1].ColumnName != "图号型号" ||
                dtTemp.Columns[2].ColumnName != "物品名称" ||
                dtTemp.Columns[3].ColumnName != "规格" ||
                dtTemp.Columns[4].ColumnName != "单位" ||
                dtTemp.Columns[5].ColumnName != "单价")
            {
                MessageDialog.ShowPromptMessage(string.Format("{0} 中没有包含所需的信息，无法导入！",
                    openFileDialog1.FileName));
            }
            else
            {
                for (int i = 0; i < dtTemp.Rows.Count; i++)
                {
                    View_F_GoodsPlanCost tempGoodsLnq = UniversalFunction.GetGoodsInfo(dtTemp.Rows[i]["图号型号"].ToString(),
                        dtTemp.Rows[i]["物品名称"].ToString(), dtTemp.Rows[i]["规格"].ToString());

                    if (tempGoodsLnq != null)
                    {
                        dtTemp.Rows[i]["物品ID"] = tempGoodsLnq.序号;
                        dtTemp.Rows[i]["单位"] = tempGoodsLnq.单位;
                    }
                    else
                    {
                        dtTemp.Rows.RemoveAt(i);
                    }
                }

                if (!m_serverThreePacketsOfTheRepairBill.UpdateAllGoodsUnitPrice(
                    MessageBox.Show("是否删除原有记录","提示",MessageBoxButtons.YesNo) == DialogResult.Yes? true : false,
                    dtTemp,out m_strErr))
                {
                    MessageDialog.ShowPromptMessage(m_strErr);
                }

                dataGridView1.DataSource = m_serverThreePacketsOfTheRepairBill.GetGoodsUnitPriceInfo();

                userControlDataLocalizer1.Init(dataGridView1, this.Name,
                    UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));

                MessageDialog.ShowPromptMessage("导入成功");
            }
        }

        private void btnOut_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
        }

        private void txtName_OnCompleteSearch()
        {
            txtName.Text = txtName.DataResult["物品名称"].ToString();
            txtCode.Text = txtName.DataResult["图号型号"].ToString();
            txtSpec.Text = txtName.DataResult["规格"].ToString();
            txtName.Tag = Convert.ToInt32(txtName.DataResult["序号"].ToString());
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            GetMessage();

            if (!m_serverThreePacketsOfTheRepairBill.OperationGoodsUnitPrice(0,m_lnqThreePackets,0,out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
            }
            else
            {
                MessageDialog.ShowPromptMessage("添加成功");
            }

            dataGridView1.DataSource = m_serverThreePacketsOfTheRepairBill.GetGoodsUnitPriceInfo();

            userControlDataLocalizer1.Init(dataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            GetMessage();

            if (!m_serverThreePacketsOfTheRepairBill.OperationGoodsUnitPrice(1, m_lnqThreePackets, 
                Convert.ToInt32( dataGridView1.CurrentRow.Cells["物品ID"].Value), out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
            }
            else
            {
                MessageDialog.ShowPromptMessage("修改成功");
            }

            dataGridView1.DataSource = m_serverThreePacketsOfTheRepairBill.GetGoodsUnitPriceInfo();

            userControlDataLocalizer1.Init(dataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!m_serverThreePacketsOfTheRepairBill.OperationGoodsUnitPrice(2, null,
                Convert.ToInt32(dataGridView1.CurrentRow.Cells["物品ID"].Value), out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
            }
            else
            {
                MessageDialog.ShowPromptMessage("删除成功");
            }

            dataGridView1.DataSource = m_serverThreePacketsOfTheRepairBill.GetGoodsUnitPriceInfo();

            userControlDataLocalizer1.Init(dataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                txtName.Text = dataGridView1.CurrentRow.Cells["物品名称"].Value.ToString();
                txtCode.Text = dataGridView1.CurrentRow.Cells["图号型号"].Value.ToString();
                txtSpec.Text = dataGridView1.CurrentRow.Cells["规格"].Value.ToString();
                numPrice.Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["单价"].Value);
                txtName.Tag = Convert.ToInt32(dataGridView1.CurrentRow.Cells["物品ID"].Value);
            }
        }
    }
}
