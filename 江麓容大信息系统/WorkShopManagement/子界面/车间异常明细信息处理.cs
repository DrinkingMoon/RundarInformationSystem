using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GlobalObject;
using PlatformManagement;
using Service_Manufacture_WorkShop;
using Expression;
using UniversalControlLibrary;
using ServerModule;

namespace Form_Manufacture_WorkShop
{
    public partial class 车间异常明细信息处理 : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strError = "";

        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 单据号
        /// </summary>
        private string m_strBillNo = "";

        public string StrBillNo
        {
            get { return m_strBillNo; }
            set { m_strBillNo = value; }
        }

        /// <summary>
        /// 车间异常处理服务组件
        /// </summary>
        Service_Manufacture_WorkShop.IMaterialsTransferException m_serverException =
            Service_Manufacture_WorkShop.ServerModuleFactory.GetServerModule<IMaterialsTransferException>();

        /// <summary>
        /// 车间库存管理服务组件
        /// </summary>
        IWorkShopStock m_serverStock = Service_Manufacture_WorkShop.ServerModuleFactory.GetServerModule<IWorkShopStock>();

        public 车间异常明细信息处理(string billNo, bool flag)
        {
            InitializeComponent();

            m_billNoControl = new BillNumberControl(CE_BillTypeEnum.车间异常信息记录表.ToString(), m_serverException);
            m_billMessageServer.BillType = CE_BillTypeEnum.车间异常信息记录表.ToString();
            m_strBillNo = billNo;

            if (!flag)
            {
                btn_Add.Enabled = false;
                btn_Delete.Enabled = false;
                btn_Modify.Enabled = false;
                btnDispose.Visible = false;
            }

            ClearInfo();
            dataGridView1.DataSource = m_serverException.GetListInfo(billNo);
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }

            ClearInfo();

            txtCode.Tag = (int)dataGridView1.CurrentRow.Cells["物品ID"].Value;
            txtCode.Text = (string)dataGridView1.CurrentRow.Cells["图号型号"].Value;
            txtName.Text = (string)dataGridView1.CurrentRow.Cells["物品名称"].Value;
            txtSpec.Text = (string)dataGridView1.CurrentRow.Cells["规格"].Value;

            dataGridView2.DataSource = m_serverException.GetDisposeInfo((int)dataGridView1.CurrentRow.Cells["ID"].Value);
            
        }

        private void dataGridView2_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.CurrentRow == null)
            {
                ClearInfo();
                return;
            }

            txtCode.Tag = dataGridView2.CurrentRow.Cells["物品ID"].Value;
            txtCode.Text = dataGridView2.CurrentRow.Cells["图号型号"].Value.ToString();
            txtName.Text = dataGridView2.CurrentRow.Cells["物品名称"].Value.ToString();
            txtSpec.Text = dataGridView2.CurrentRow.Cells["规格"].Value.ToString();
            txtBatchNo.Text = dataGridView2.CurrentRow.Cells["批次号"].Value.ToString();
            numOperationCount.Value = Convert.ToDecimal( dataGridView2.CurrentRow.Cells["数量"].Value);
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="msg">定位信息</param>
        void PositioningRecord(int goodsID, string batchNo)
        {
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                if ((int)dataGridView2.Rows[i].Cells["物品ID"].Value == goodsID
                    && (string)dataGridView2.Rows[i].Cells["批次号"].Value == batchNo)
                {
                    dataGridView2.FirstDisplayedScrollingRowIndex = i;
                    dataGridView2.CurrentCell = dataGridView2.Rows[i].Cells["物品ID"];
                }
            }
        }

        void GetGoodsInfo()
        {
            if (txtCode.Tag == null)
            {
                lbHYDW.Text = "";
                lbKCDW.Text = "";
                return;
            }

            View_F_GoodsPlanCost tempLnq = UniversalFunction.GetGoodsInfo(Convert.ToInt32(txtCode.Tag));

            if (tempLnq == null)
            {
                lbHYDW.Text = "";
                lbKCDW.Text = "";
                return;
            }

            lbHYDW.Text = tempLnq.单位;
            lbKCDW.Text = tempLnq.单位;

            WS_WorkShopStock tempStock = 
                m_serverStock.GetStockSingleInfo(CE_WorkShopCode.ZPCJ.ToString(), Convert.ToInt32(txtCode.Tag), txtBatchNo.Text);

            if (tempStock == null)
            {
                lbStockCount.Text = "0";
            }
            else
            {
                lbStockCount.Text = tempStock.StockCount.ToString();
            }
        }

        void ClearInfo()
        {
            txtCode.Tag = null;
            txtCode.Text = "";
            txtName.Text = "";
            txtSpec.Text = "";
            txtBatchNo.Text = "";
            numOperationCount.Value = 0;

            lbHYDW.Text = "";
            lbKCDW.Text = "";
            lbStockCount.Text = "";
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            GetGoodsInfo();
        }

        private void txtCode_TextChanged(object sender, EventArgs e)
        {
            GetGoodsInfo();
        }

        private void txtBatchNo_TextChanged(object sender, EventArgs e)
        {
            GetGoodsInfo();
        }

        private void txtSpec_TextChanged(object sender, EventArgs e)
        {
            GetGoodsInfo();
        }

        private void txtBatchNo_Enter(object sender, EventArgs e)
        {
            txtBatchNo.StrEndSql = " and 物品ID = " + Convert.ToInt32(txtCode.Tag) + " and 车间代码 = '"
                + CE_WorkShopCode.ZPCJ.ToString() + "'";
        }

        private void txtCode_OnCompleteSearch()
        {
            txtCode.Tag = Convert.ToInt32(txtCode.DataResult["物品ID"]);
            txtName.Text = txtCode.DataResult["物品名称"].ToString();
            txtSpec.Text = txtCode.DataResult["规格"].ToString();
        }

        void OperationInfo(CE_OperatorMode mode)
        {
            WS_MaterialsTransferExceptionListDispose tempLnq = new WS_MaterialsTransferExceptionListDispose();

            tempLnq.ID = dataGridView2.CurrentRow == null ? 0 : Convert.ToInt32(dataGridView2.CurrentRow.Cells["ID"].Value);
            tempLnq.BatchNo = txtBatchNo.Text;
            tempLnq.Counts = numOperationCount.Value;
            tempLnq.GoodsID = Convert.ToInt32(txtCode.Tag);
            tempLnq.ListID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ID"].Value);

            m_serverException.OperationDisposeInfo(mode, tempLnq, out m_strError);
            dataGridView2.DataSource = m_serverException.GetDisposeInfo((int)dataGridView1.CurrentRow.Cells["ID"].Value);

            switch (mode)
            {
                case CE_OperatorMode.添加:
                case CE_OperatorMode.修改:

                    PositioningRecord(tempLnq.GoodsID, tempLnq.BatchNo);
                    break;
                case CE_OperatorMode.删除:
                    break;
                default:
                    break;
            }
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            OperationInfo(CE_OperatorMode.添加);
        }

        private void btn_Modify_Click(object sender, EventArgs e)
        {
            OperationInfo(CE_OperatorMode.修改);
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            OperationInfo(CE_OperatorMode.删除);
        }

        private void txtCode_Enter(object sender, EventArgs e)
        {
            txtCode.StrEndSql = " and 车间代码 = '" + CE_WorkShopCode.ZPCJ.ToString() + "'";
        }

        private void btnDispose_Click(object sender, EventArgs e)
        {
            if (m_serverException.ExcuteDisposeInfo(m_strBillNo, out m_strBillNo))
            {
                MessageDialog.ShowPromptMessage("处理成功");
                this.Close();
            }
            else
            {
                MessageDialog.ShowPromptMessage(m_strBillNo);
                return;
            }
        }
    }
}
