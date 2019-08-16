using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using PlatformManagement;
using GlobalObject;
using UniversalControlLibrary;

namespace Expression
{
    public partial class 未到货订单统计 : Form
    {
        IOrderFormInfoServer m_serviceOrderFormInfo = ServerModuleFactory.GetServerModule<IOrderFormInfoServer>();

        public 未到货订单统计()
        {
            InitializeComponent();

            string[] strBillStatus = { "全部","待关闭", "已关闭"};

            checkBillDateAndStatus1.InsertComBox(strBillStatus);

            customDataGridView1.DataSource = m_serviceOrderFormInfo.GetGoodsAfloatOrderForm(null, null, "待关闭", null, 0);
        }

        private void checkBillDateAndStatus1_OnCompleteSearch()
        {
            string strProvider = "";
            int intGoodsID = 0;

            if (txtProvider.Text.Trim().Length != 0)
            {
                strProvider = txtProvider.Tag.ToString();
            }

            if (txtGoodsCode.Text.Trim().Length != 0 || txtGoodsName.Text.Trim().Length != 0 || txtSpec.Text.Trim().Length != 0)
            {
                intGoodsID = Convert.ToInt32(txtGoodsCode.Tag);
            }

            customDataGridView1.DataSource = m_serviceOrderFormInfo.GetGoodsAfloatOrderForm(checkBillDateAndStatus1.dtpStartTime.Value, 
                checkBillDateAndStatus1.dtpEndTime.Value, checkBillDateAndStatus1.cmbBillStatus.Text, strProvider, intGoodsID);
        }

        private void customDataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 10 && e.RowIndex != -1)
            {
                string orderFormNumber = customDataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                int goodsID = (int)customDataGridView1.Rows[e.RowIndex].Cells[11].Value;

                if (customDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "待关闭")
                {
                    customDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "已关闭";
                    m_serviceOrderFormInfo.UpdateOrderFormCloseStatus(orderFormNumber, goodsID, true);
                }
                else
                {
                    customDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "待关闭";
                    m_serviceOrderFormInfo.UpdateOrderFormCloseStatus(orderFormNumber, goodsID, false);
                }
            }
        }

        private void txtGoodsCode_OnCompleteSearch()
        {
            txtGoodsCode.Text = txtGoodsCode.DataResult["图号型号"].ToString();
            txtGoodsName.Text = txtGoodsCode.DataResult["物品名称"].ToString();
            txtSpec.Text = txtGoodsCode.DataResult["规格"].ToString();

            txtGoodsCode.Tag = txtGoodsCode.DataResult["序号"];
        }

        private void txtProvider_OnCompleteSearch()
        {
            txtProvider.Text = txtProvider.DataResult["供应商名称"].ToString();

            txtProvider.Tag = txtProvider.DataResult["供应商编码"];
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtProvider.Text = "";
            txtProvider.Tag = null;
            txtGoodsCode.Text = "";
            txtGoodsName.Text = "";
            txtSpec.Text = "";
            txtGoodsCode.Tag = null;
        }
    }
}
