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
    public partial class 还货清单 : Form
    {
        /// <summary>
        /// 还货服务组件
        /// </summary>
        IProductReturnService m_serverReturnService = ServerModuleFactory.GetServerModule<IProductReturnService>();

        /// <summary>
        /// 单据号
        /// </summary>
        string m_strBillNo = "";

        /// <summary>
        /// 物品ID
        /// </summary>
        int m_intGoodsID;

        /// <summary>
        /// 批次号
        /// </summary>
        string m_strBatchNo = "";

        /// <summary>
        /// 库房代码
        /// </summary>
        string m_strStorageID = "";

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr = "";

        /// <summary>
        /// 供应商
        /// </summary>
        string m_strProvider = "";

        public 还货清单(string stroageID, string billNo, int goodsID, string provider, string batchNo)
        {
            InitializeComponent();
            ListClear();
            m_intGoodsID = goodsID;
            m_strBatchNo = batchNo;
            m_strBillNo = billNo;
            m_strStorageID = stroageID;
            m_strProvider = provider;
        }

        private void 还货清单_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = m_serverReturnService.GetAllInfo(m_strBillNo, m_intGoodsID, m_strProvider, m_strBatchNo);
        }

        /// <summary>
        /// 明细信息清空
        /// </summary>
        void ListClear()
        {
            txtCode.Text = "";
            txtCode.Tag = null;
            txtName.Text = "";
            txtSpec.Text = "";
            txtProvider.Text = "";
            txtBatchNo.Text = "";
            numOperationCount.Value = 0;
            lbStockCount.Text = "";
        }

        private void txtCode_Enter(object sender, EventArgs e)
        {
            txtCode.StrEndSql = " and 数量 > 0 and 借方代码 = '" + BasicInfo.DeptCode + "' and 贷方代码 = '" + m_strStorageID + "'";
        }

        private void txtCode_OnCompleteSearch()
        {
            txtCode.Text = txtCode.DataResult["图号型号"].ToString();
            txtCode.Tag = txtCode.DataResult["物品ID"];
            txtName.Text = txtCode.DataResult["物品名称"].ToString();
            txtSpec.Text = txtCode.DataResult["规格"].ToString();
            lbHYDW.Text = txtCode.DataResult["单位"].ToString();
            lbKCDW.Text = txtCode.DataResult["单位"].ToString();
        }

        private void txtBatchNo_Enter(object sender, EventArgs e)
        {
            txtBatchNo.StrEndSql = " and 数量 > 0 and 借方代码 = '" + BasicInfo.DeptCode + "' and 贷方代码 = '" 
                + m_strStorageID + "' and 物品ID = " + Convert.ToInt32(txtCode.Tag);
        }

        private void txtBatchNo_OnCompleteSearch()
        {
            txtBatchNo.Text = txtBatchNo.DataResult["批次号"].ToString();
            txtProvider.Text = txtBatchNo.DataResult["供应商"].ToString();
            lbStockCount.Text = txtBatchNo.DataResult["数量"].ToString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtCode.Tag == null)
            {
                MessageDialog.ShowPromptMessage("请选择物品");
                return;
            }

            DataTable dtTemp = (DataTable)dataGridView1.DataSource;

            DataRow dr = dtTemp.NewRow();

            dr["图号型号"] = txtCode.Text;
            dr["物品名称"] = txtName.Text;
            dr["规格"] = txtSpec.Text;
            dr["供应商"] = txtProvider.Text;
            dr["物品ID"] = txtCode.Tag;
            dr["批次号"] = txtBatchNo.Text;
            dr["数量"] = numOperationCount.Value;
            dr["备注"] = txtRemark.Text;
            dr["单位"] = lbHYDW.Text;

            dtTemp.Rows.Add(dr);

            dataGridView1.DataSource = dtTemp;
            ListClear();
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (txtCode.Tag == null)
            {
                MessageDialog.ShowPromptMessage("请选择物品");
                return;
            }

            DataTable dtTemp = (DataTable)dataGridView1.DataSource;

            int rowIndex = dataGridView1.CurrentRow.Index;

            dtTemp.Rows[rowIndex]["图号型号"] = txtCode.Text;
            dtTemp.Rows[rowIndex]["物品名称"] = txtName.Text;
            dtTemp.Rows[rowIndex]["供应商"] = txtProvider.Text;
            dtTemp.Rows[rowIndex]["规格"] = txtSpec.Text;
            dtTemp.Rows[rowIndex]["物品ID"] = txtCode.Tag;
            dtTemp.Rows[rowIndex]["批次号"] = txtBatchNo.Text;
            dtTemp.Rows[rowIndex]["数量"] = numOperationCount.Value;
            dtTemp.Rows[rowIndex]["备注"] = txtRemark.Text;
            dtTemp.Rows[rowIndex]["单位"] = lbHYDW.Text;

            dataGridView1.DataSource = dtTemp;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DataTable dtTemp = (DataTable)dataGridView1.DataSource;

            dtTemp.Rows.RemoveAt(dataGridView1.CurrentRow.Index);

            dataGridView1.DataSource = dtTemp;
            ListClear();
        }

        private void btnAudit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPropose_Click(object sender, EventArgs e)
        {
            DataTable tempTable = (DataTable)dataGridView1.DataSource;

            if (tempTable == null || tempTable.Rows.Count == 0)
            {
                return;
            }

            if (m_serverReturnService.SaveInfo(m_strBillNo, m_intGoodsID, m_strProvider, m_strBatchNo, tempTable, out m_strErr))
            {
                MessageDialog.ShowPromptMessage("保存成功");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageDialog.ShowPromptMessage(m_strErr);
            }
        }
    }
}
