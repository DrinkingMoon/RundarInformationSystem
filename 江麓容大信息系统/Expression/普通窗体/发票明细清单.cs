using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using GlobalObject;
using PlatformManagement;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 发票明细清单界面
    /// </summary>
    public partial class 发票明细清单 : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 发票号
        /// </summary>
        string m_strInvoice;

        /// <summary>
        /// 供应商编码
        /// </summary>
        string m_strProvider;

        /// <summary>
        /// 供应商名称
        /// </summary>
        string m_strProviderName;

        /// <summary>
        /// 发票类型标志
        /// </summary>
        int m_intInvocieType;

        /// <summary>
        /// 凭证号
        /// </summary>
        string m_strPZH;

        /// <summary>
        /// 标志
        /// </summary>
        bool m_blFalg = false;

        /// <summary>
        /// 供应商服务组件
        /// </summary>
        IProviderServer m_serverProvider = ServerModuleFactory.GetServerModule<IProviderServer>();

        /// <summary>
        /// 发票管理服务组件
        /// </summary>
        IInvoiceServer m_findInvoice = ServerModuleFactory.GetServerModule<IInvoiceServer>();

        /// <summary>
        /// 合同管理服务组件
        /// </summary>
        IBargainInfoServer m_serverBargainInfo = ServerModuleFactory.GetServerModule<IBargainInfoServer>();

        /// <summary>
        /// 发票明细数据集
        /// </summary>
        DataTable m_dtInvoice = new DataTable();

        /// <summary>
        /// 供应商信息
        /// </summary>
        FormQueryInfo m_formProvider;

        public 发票明细清单()
        {
            InitializeComponent();
        }

        public 发票明细清单(string v_Invoice, string v_Provider, string v_ProviderName, 
            int intType, string strPZH, AuthorityFlag m_authFlag)
        {
            InitializeComponent();

            m_strInvoice = v_Invoice;

            m_strProvider = v_Provider;

            m_strProviderName = v_ProviderName;

            m_strPZH = strPZH;

            m_intInvocieType = intType;

            FaceAuthoritySetting.SetEnable(this.Controls, m_authFlag);
            FaceAuthoritySetting.SetVisibly(this.toolStrip1, m_authFlag);

            this.toolStrip1.Visible = true;
        }

        private void 发票明细清单_Load(object sender, EventArgs e)
        {
            DateTime today = ServerTime.Time;
            dtp_Start.Value = new DateTime(today.Year, today.Month,1);
            DataTable DtOrderInfo = new DataTable();
            m_dtInvoice = m_findInvoice.GetInvoiceInfo(m_strInvoice);

            if (m_dtInvoice.Rows.Count == 0)
            {
                m_blFalg = true;
                CreatDtStyle();
            }

            dgv_InvoiceShow.DataSource = m_dtInvoice;
            

            DateTime S_Date = dtp_Start.Value;
            DateTime E_Date = dtp_End.Value;

            txtProvide.Text = m_strProviderName;
            txtProvide.Tag = m_strProvider;

            DtOrderInfo = m_findInvoice.GetBillInfo(S_Date,E_Date,txtProvide.Tag.ToString(),txtOrderNumber.Text);
            dgv_OrderInfo.DataSource = GetProviderName(DtOrderInfo);
            dgv_OrderGoods.ClearSelection();
            dgv_InvoiceShow.ClearSelection();

            this.lbSumPrice.Text = GetSum("Price", m_dtInvoice).ToString();
            this.lbSumTax.Text = GetSum("Tax", m_dtInvoice).ToString();
        }

        private void dgv_OrderInfo_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            InitOrderGoods();
            dgv_OrderGoods.ClearSelection();
            dgv_InvoiceShow.ClearSelection();
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dr in dgv_InvoiceShow.SelectedRows)
            {
                if (dr.Selected)
                {
                    m_dtInvoice.Rows.RemoveAt(dr.Index);
                }
            }

            dgv_InvoiceShow.DataSource = m_dtInvoice;
            dgv_InvoiceShow.ClearSelection();
            InitOrderGoods();
        }

        private void dgv_OrderGoods_DoubleClick(object sender, EventArgs e)
        {
            if (CheckDate(dgv_OrderGoods.CurrentRow,dgv_OrderInfo.CurrentRow))
            {

                DataRow dr = m_dtInvoice.NewRow();
                Decimal dcTaxRat = m_serverBargainInfo.GetBargainCess(dgv_OrderInfo.CurrentRow.Cells["订单号"].Value.ToString());
                bool blFlag = false;

                if (dgv_OrderInfo.CurrentRow.Cells["入库单号"].Value.ToString().Substring(0, 3) == "CTD")
                {
                    //if (!CheckMessage())
                    //{
                    //    MessageBox.Show("请先选择对应的入库单！", "提示");
                    //    return;
                    //}
                    blFlag = true;
                }

                dr["InvoiceCode"] = m_strInvoice;
                dr["GoodsCode"] = dgv_OrderGoods.CurrentRow.Cells["图号型号"].Value.ToString();
                dr["GoodsName"] = dgv_OrderGoods.CurrentRow.Cells["物品名称"].Value.ToString();
                dr["Spec"] = dgv_OrderGoods.CurrentRow.Cells["规格"].Value.ToString();
                dr["Provider"] = m_strProvider;
                dr["Depot"] = dgv_OrderGoods.CurrentRow.Cells["类别"].Value.ToString();
                dr["Count"] = blFlag == true ? (-Convert.ToDecimal( dgv_OrderGoods.CurrentRow.Cells["数量"].Value.ToString())).ToString() :
                    dgv_OrderGoods.CurrentRow.Cells["数量"].Value.ToString();
                dr["Unit"] = dgv_OrderGoods.CurrentRow.Cells["单位"].Value.ToString();
                dr["UnitPrice"] = Convert.ToDecimal(dgv_OrderGoods.CurrentRow.Cells["单价"].Value).ToString();
                dr["OrderNumber"] = dgv_OrderInfo.CurrentRow.Cells["订单号"].Value.ToString();
                dr["Bill_ID"] = dgv_OrderInfo.CurrentRow.Cells["入库单号"].Value.ToString();
                dr["BatchNo"] = dgv_OrderGoods.CurrentRow.Cells["批次号"].Value.ToString();
                dr["InvoiceType"] = m_intInvocieType;
                dr["BeforTax"] = dgv_OrderGoods.CurrentRow.Cells["含税价"].Value.ToString();
                dr["Taxrat"] = Convert.ToInt32((dcTaxRat - 1) * 100);
                dr["Tax"] = blFlag == true? -Math.Round((Convert.ToDecimal(dgv_OrderGoods.CurrentRow.Cells["数量"].Value) *
                            Convert.ToDecimal(dgv_OrderGoods.CurrentRow.Cells["单价"].Value) *
                            (dcTaxRat - 1)), 2) : Math.Round((Convert.ToDecimal(dgv_OrderGoods.CurrentRow.Cells["数量"].Value) *
                            Convert.ToDecimal(dgv_OrderGoods.CurrentRow.Cells["单价"].Value) *
                            (dcTaxRat - 1)), 2);

                dr["Price"] = blFlag == true ? -Math.Round((Convert.ToDecimal(dgv_OrderGoods.CurrentRow.Cells["数量"].Value) *
                    Convert.ToDecimal(dgv_OrderGoods.CurrentRow.Cells["单价"].Value)), 2) : Math.Round(
                    (Convert.ToDecimal(dgv_OrderGoods.CurrentRow.Cells["数量"].Value) *
                    Convert.ToDecimal(dgv_OrderGoods.CurrentRow.Cells["单价"].Value)), 2);
                dr["PZH"] = m_strPZH;

                m_dtInvoice.Rows.Add(dr);

                dgv_InvoiceShow.DataSource = m_dtInvoice;
                dgv_InvoiceShow.ClearSelection();
            }
            else
            {
                MessageBox.Show("不能添加同种物品！","提示");
            }

            btn_find.Focus();
            InitOrderGoods();

            this.lbSumPrice.Text = GetSum("Price", m_dtInvoice).ToString();
            this.lbSumTax.Text = GetSum("Tax", m_dtInvoice).ToString();	
        }

        /// <summary>
        /// 初始化订单物品
        /// </summary>
        private void InitOrderGoods()
        {

            if (dgv_OrderInfo.CurrentRow == null)
            {
                return;
            }

            string v_OrderNumber = dgv_OrderInfo.CurrentRow.Cells["入库单号"].Value.ToString();
            DataTable DtOrderGoods = m_findInvoice.GetGoodsInfo(v_OrderNumber);
            dgv_OrderGoods.DataSource = DtOrderGoods;
        }

        /// <summary>
        /// 创建DataTable样式
        /// </summary>
        private void CreatDtStyle()
        {
            m_dtInvoice.Columns.Add("InvoiceCode");
            m_dtInvoice.Columns.Add("GoodsCode");
            m_dtInvoice.Columns.Add("GoodsName");
            m_dtInvoice.Columns.Add("Spec");
            m_dtInvoice.Columns.Add("Provider");
            m_dtInvoice.Columns.Add("Depot");
            m_dtInvoice.Columns.Add("Count");
            m_dtInvoice.Columns.Add("Unit");
            m_dtInvoice.Columns.Add("UnitPrice");
            m_dtInvoice.Columns.Add("OrderNumber");
            m_dtInvoice.Columns.Add("Bill_ID");
            m_dtInvoice.Columns.Add("BatchNo");
            m_dtInvoice.Columns.Add("InvoiceType");
            m_dtInvoice.Columns.Add("BeforTax");
            m_dtInvoice.Columns.Add("Taxrat");
            m_dtInvoice.Columns.Add("Tax");
            m_dtInvoice.Columns.Add("Price");
            m_dtInvoice.Columns.Add("PZH");
        }

        private void btn_find_Click(object sender, EventArgs e)
        {
            DataTable DtOrderInfo = new DataTable();
            DateTime S_Date = dtp_Start.Value;
            DateTime E_Date = dtp_End.Value;

            if (txtProvide.Text.Trim() == "")
            {
                txtProvide.Tag = "";
            }

            DtOrderInfo = m_findInvoice.GetBillInfo(S_Date, E_Date, txtProvide.Tag.ToString(), txtOrderNumber.Text);
            dgv_OrderInfo.DataSource = GetProviderName(DtOrderInfo);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!m_blFalg)
            {
                string strDateNy = ServerTime.GetMonthlyString(Convert.ToDateTime(m_dtInvoice.Rows[0]["Date"]));

                string strNowNy = ServerTime.GetMonthlyString(ServerTime.Time);

                if (strDateNy != strNowNy)
                {
                    MessageDialog.ShowPromptMessage("不能跨月重新保存发票");
                    return;
                }
            }

            m_dtInvoice = (DataTable)dgv_InvoiceShow.DataSource;

            foreach (DataRow drTemp in m_dtInvoice.Rows)
            {
                drTemp["UnitPrice"] = Convert.ToDecimal(drTemp["Price"]) / Convert.ToDecimal(drTemp["Count"]);
            }

            if (m_findInvoice.DeleteInvoiceInfo(m_strInvoice, out m_err))
            {
                if (m_findInvoice.AddInvoiceInfo(m_dtInvoice, out m_err))
                {
                    if (m_findInvoice.UpdatePrice(m_dtInvoice, out m_err))
                    {
                        MessageBox.Show("保存成功!", "提示");
                    }
                    else
                    {
                        MessageDialog.ShowErrorMessage(m_err);
                    }
                }
                else
                {
                    MessageDialog.ShowErrorMessage(m_err);
                }
            }
            else
            {
                MessageDialog.ShowErrorMessage(m_err);
            }
        }

        /// <summary>
        /// 获得供应商名称
        /// </summary>
        /// <param name="dtNoName">需要获取供应商名称的数据集</param>
        /// <returns>返回已获取到供应商名称的数据集</returns>
        private DataTable GetProviderName(DataTable dtNoName)
        {
            dtNoName.Columns.Add("ProviderName");

            for (int i = 0; i <= dtNoName.Rows.Count - 1; i++)
            {
                dtNoName.Rows[i]["ProviderName"] = m_serverProvider.GetProviderName(dtNoName.Rows[i]["供应商"].ToString());
            }

            return dtNoName;
        }

        private void btnFindProvider_Click(object sender, EventArgs e)
        {
            m_formProvider = QueryInfoDialog.GetProviderInfoDialog();

            if (m_formProvider.ShowDialog() == DialogResult.OK)
            {
                txtProvide.Tag = m_formProvider.GetStringDataItem("供应商编码");
                txtProvide.Text = m_formProvider.GetStringDataItem("简称");
            }

        }

        /// <summary>
        /// 检测数据集
        /// </summary>
        /// <param name="dgvGoodsRow">DataGridRow物品行</param>
        /// <param name="dgvInfoRow">DataGridRow信息行</param>
        /// <returns>检测通过返回True，否则返回false</returns>
        private bool CheckDate(DataGridViewRow dgvGoodsRow,DataGridViewRow dgvInfoRow)
        {

            for (int i = 0; i <= m_dtInvoice.Rows.Count - 1; i++)
            {
                if (m_dtInvoice.Rows[i]["GoodsCode"].ToString() == dgvGoodsRow.Cells["图号型号"].Value.ToString()
                  && m_dtInvoice.Rows[i]["GoodsName"].ToString() == dgvGoodsRow.Cells["物品名称"].Value.ToString()
                  && m_dtInvoice.Rows[i]["Spec"].ToString() == dgvGoodsRow.Cells["规格"].Value.ToString()
                  && m_dtInvoice.Rows[i]["OrderNumber"].ToString() == dgvInfoRow.Cells["订单号"].Value.ToString()
                  && m_dtInvoice.Rows[i]["Bill_ID"].ToString() == dgvInfoRow.Cells["入库单号"].Value.ToString()
                  && m_dtInvoice.Rows[i]["BatchNo"].ToString() == dgvGoodsRow.Cells["批次号"].Value.ToString())
                {
                    return false;
                }
            }

            return true;
        }

        private void 添加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dgvr in dgv_OrderGoods.SelectedRows)
            {
                if (dgvr.Cells["物品名称"].Value != System.DBNull.Value)
                {
                    if (CheckDate(dgvr, dgv_OrderInfo.CurrentRow))
                    {
                        DataRow dr = m_dtInvoice.NewRow();
                        Decimal dcTaxRat = m_serverBargainInfo.GetBargainCess(dgv_OrderInfo.CurrentRow.Cells["订单号"].Value.ToString());

                        dr["InvoiceCode"] = m_strInvoice;
                        dr["GoodsCode"] = dgvr.Cells["图号型号"].Value.ToString();
                        dr["GoodsName"] = dgvr.Cells["物品名称"].Value.ToString();
                        dr["Spec"] = dgvr.Cells["规格"].Value.ToString();
                        dr["Provider"] = m_strProvider;
                        dr["Depot"] = dgvr.Cells["类别"].Value.ToString();
                        dr["Count"] = dgvr.Cells["数量"].Value.ToString();
                        dr["Unit"] = dgvr.Cells["单位"].Value.ToString();
                        dr["UnitPrice"] =  dgvr.Cells["单价"].Value.ToString();
                        dr["OrderNumber"] = dgv_OrderInfo.CurrentRow.Cells["订单号"].Value.ToString();
                        dr["Bill_ID"] = dgv_OrderInfo.CurrentRow.Cells["入库单号"].Value.ToString();
                        dr["BatchNo"] = dgvr.Cells["批次号"].Value.ToString();
                        dr["InvoiceType"] = m_intInvocieType;
                        dr["BeforTax"] = dgv_OrderGoods.CurrentRow.Cells["含税价"].Value.ToString();
                        dr["Taxrat"] = Convert.ToInt32((dcTaxRat - 1) * 100);

                        dr["Tax"] =Math.Round(( Convert.ToDecimal(dgv_OrderGoods.CurrentRow.Cells["数量"].Value) *
                                    Convert.ToDecimal(dgv_OrderGoods.CurrentRow.Cells["单价"].Value) *
                                    (dcTaxRat - 1)),2);

                        dr["Price"] =Math.Round((Convert.ToDecimal(dgv_OrderGoods.CurrentRow.Cells["数量"].Value) *
                                      Convert.ToDecimal(dgv_OrderGoods.CurrentRow.Cells["单价"].Value)),2);
                        dr["PZH"] = m_strPZH;

                        m_dtInvoice.Rows.Add(dr);
                        dgv_InvoiceShow.DataSource = m_dtInvoice;
                        dgv_InvoiceShow.ClearSelection();
                    }
                    else
                    {
                        MessageBox.Show("不能添加同种物品！   [" + dgvr.Cells["物品名称"].Value.ToString() + "]", "提示");
                        break;
                    }
                }
            }

            btn_find.Focus();
            InitOrderGoods();

            this.lbSumPrice.Text = GetSum("Price", m_dtInvoice).ToString();
            this.lbSumTax.Text = GetSum("Tax", m_dtInvoice).ToString();
        }

        private void btnOutExcel_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dgv_InvoiceShow);
        }

        private void dgv_InvoiceShow_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            dgv_InvoiceShow.CurrentRow.Cells["Tax"].Value = Math.Round(
                (Convert.ToDecimal(dgv_InvoiceShow.CurrentRow.Cells["Price"].Value) *
                Convert.ToDecimal(dgv_InvoiceShow.CurrentRow.Cells["Taxrat"].Value) / 100), 2);

            dgv_InvoiceShow.CurrentRow.Cells["UnitPrice"].Value =
                Convert.ToDecimal(dgv_InvoiceShow.CurrentRow.Cells["Price"].Value) /
                Convert.ToDecimal(dgv_InvoiceShow.CurrentRow.Cells["Count"].Value);

            dgv_InvoiceShow.CurrentRow.Cells["BeforTax"].Value =
                (Convert.ToDecimal(dgv_InvoiceShow.CurrentRow.Cells["Tax"].Value) +
                Convert.ToDecimal(dgv_InvoiceShow.CurrentRow.Cells["Price"].Value)) /
                Convert.ToDecimal(dgv_InvoiceShow.CurrentRow.Cells["Count"].Value);

            m_dtInvoice = (DataTable)dgv_InvoiceShow.DataSource;

            this.lbSumPrice.Text = GetSum("Price", m_dtInvoice).ToString();
            this.lbSumTax.Text = GetSum("Tax", m_dtInvoice).ToString();
				
        }

        /// <summary>
        /// 获得合计金额
        /// </summary>
        /// <param name="type">合计金额列的名称</param>
        /// <param name="Dt">合计的数据集</param>
        /// <returns>返回合计金额</returns>
        private Decimal GetSum(string type,DataTable dt)
        {
            Decimal dcmType = 0;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dcmType = dcmType + Convert.ToDecimal(dt.Rows[i][type]);
            }

            return Math.Round(dcmType,2);
        }

        private void dgv_OrderGoods_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgv_InvoiceShow_DataBindingComplete(sender,e);
        }

        private void dgv_InvoiceShow_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            #region  统一调色

            for (int i = 0; i < dgv_InvoiceShow.Rows.Count; i++)
            {
                dgv_InvoiceShow.Rows[i].DefaultCellStyle.BackColor = Color.White;

            }

            for (int k = 0; k < dgv_OrderGoods.Rows.Count; k++)
            {
                dgv_OrderGoods.Rows[k].DefaultCellStyle.BackColor = Color.White;
            }

            #endregion

            #region 选择着色
            for (int i = 0; i <= dgv_InvoiceShow.Rows.Count - 1; i++)
            {
                for (int k = 0; k <= dgv_OrderGoods.Rows.Count - 1; k++)
                {

                    if (dgv_InvoiceShow.Rows[i].Cells["Bill_ID"].Value.ToString()
                        == dgv_OrderInfo.CurrentRow.Cells["入库单号"].Value.ToString()
                        && dgv_InvoiceShow.Rows[i].Cells["GoodsCode"].Value.ToString()
                        == dgv_OrderGoods.Rows[k].Cells["图号型号"].Value.ToString()
                        && dgv_InvoiceShow.Rows[i].Cells["GoodsName"].Value.ToString()
                        == dgv_OrderGoods.Rows[k].Cells["物品名称"].Value.ToString()
                        && dgv_InvoiceShow.Rows[i].Cells["Spec"].Value.ToString()
                        == dgv_OrderGoods.Rows[k].Cells["规格"].Value.ToString()
                        && dgv_InvoiceShow.Rows[i].Cells["BatchNo"].Value.ToString()
                        == dgv_OrderGoods.Rows[k].Cells["批次号"].Value.ToString())
                    {
                        dgv_InvoiceShow.Rows[i].DefaultCellStyle.BackColor = Color.Green;
                        dgv_OrderGoods.Rows[k].DefaultCellStyle.BackColor = Color.Green;
                    }

                }
            }
            #endregion

            dgv_OrderGoods.ClearSelection();

        }

        /// <summary>
        /// 检测数据
        /// </summary>
        /// <returns>检测通过返回True，否则返回False</returns>
        private bool CheckMessage()
        {
            DataTable dt = (DataTable)dgv_InvoiceShow.DataSource;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["GoodsCode"].ToString() == dgv_OrderGoods.CurrentRow.Cells["图号型号"].Value.ToString()
                    && dt.Rows[i]["GoodsName"].ToString() == dgv_OrderGoods.CurrentRow.Cells["物品名称"].Value.ToString()
                    && dt.Rows[i]["Spec"].ToString() == dgv_OrderGoods.CurrentRow.Cells["规格"].Value.ToString())
                {
                    return true;
                }
            }

            return false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
