using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using ServerModule;
using GlobalObject;
using UniversalControlLibrary;

namespace Expression
{
    public partial class 采购BOM : Form
    {
        /// <summary>
        /// 安全库存服务组件
        /// </summary>
        ISafeStockServer m_serverSaftStock = ServerModuleFactory.GetServerModule<ISafeStockServer>();

        /// <summary>
        /// 产品型号与数量构成的字典
        /// </summary>
        Dictionary<string, int> m_dicNumber = new Dictionary<string, int>();

        /// <summary>
        /// 采购物料清单服务
        /// </summary>
        ICBOMServer m_serverCBOM = ServerModuleFactory.GetServerModule<ICBOMServer>();

        /// <summary>
        /// 产品信息服务接口
        /// </summary>
        IProductListServer m_productListServer = ServerModuleFactory.GetServerModule<IProductListServer>();

        IBomServer _bomService = ServerModuleFactory.GetServerModule<IBomServer>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strError;

        /// <summary>
        /// 设置总成数量对话框（设置单个零件所需的各总成的数量）
        /// </summary>
        设置总成数量 m_formSetNumberOfProduct;

        /// <summary>
        /// FunctionTreeNodeInfo
        /// </summary>
        FunctionTreeNodeInfo m_nodeInfo;

        public 采购BOM(FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();
            RefreshData();
            m_nodeInfo = nodeInfo;
        }

        /// <summary>
        /// 权限控制
        /// </summary>
        /// <param name="authorityFlag">权限标志</param>
        void AuthorityControl(PlatformManagement.AuthorityFlag authorityFlag)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip1, authorityFlag);
            FaceAuthoritySetting.SetEnable(this.Controls, authorityFlag);
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        public void PositioningRecord(int goodsID)
        {
            string strColName = "";

            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                if (col.Visible)
                {
                    strColName = col.Name;
                    break;
                }
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if ((int)dataGridView1.Rows[i].Cells["GoodsID"].Value == goodsID)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                    break;
                }
            }
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        void ClearData()
        {
            tbsGoods.Tag = null;
            tbsGoods.Text = "";
            txtGoodsCode.Text = "";
            txtSpce.Text = "";
            numSafeStockCount.Value = 0;
            m_dicNumber = new Dictionary<string, int>();
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        void RefreshData()
        {
            dataGridView1.DataSource = m_serverCBOM.GetAllInfo();

            dataGridView1.Columns["图号型号"].Width = 200;
            dataGridView1.Columns["物品名称"].Width = 200;

            for (int i = 5; i < dataGridView1.Columns.Count; i++)
            {
                dataGridView1.Columns[i].Width = 140;
            }

            userControlDataLocalizer1.Init(this.dataGridView1, this.Name, 
                UniversalFunction.SelectHideFields(this.Name, this.dataGridView1.Name, BasicInfo.LoginID));
        }

        /// <summary>
        /// 操作数据库
        /// </summary>
        void OperatorInfo(CE_OperatorMode operatorMode)
        {
            try
            {
                m_serverCBOM.OperatorInfo(operatorMode, Convert.ToInt32(tbsGoods.Tag), numSafeStockCount.Value, m_dicNumber);
                MessageDialog.ShowPromptMessage(operatorMode.ToString() + "成功");
                RefreshData();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void btnSetUsage_Click(object sender, EventArgs e)
        {
            DataGridViewRow dataGridViewRow = new DataGridViewRow();

            foreach (DataGridViewRow dgvr in dataGridView1.Rows)
            {
                if (dgvr.Cells["GoodsID"].Value.ToString() == tbsGoods.Tag.ToString())
                {
                    dataGridViewRow = dgvr;
                }
            }

            m_formSetNumberOfProduct = new 设置总成数量(dataGridViewRow);

            m_formSetNumberOfProduct.ShowDialog();

            if (m_formSetNumberOfProduct.DicNumberOfProduct.Count != 0)
            {
                m_dicNumber = m_formSetNumberOfProduct.DicNumberOfProduct;
            }
        }

        private void btnCreateInfo_Click(object sender, EventArgs e)
        {
            由总成自动生成安全库存 form = new 由总成自动生成安全库存();
            form.ShowDialog();

            if (form.DtSafeGoods != null && form.DtSafeGoods.Rows.Count != 0)
            {
                if (!m_serverSaftStock.UpdateSafeStockInfo(form.DtSafeGoods,out m_strError))
                {
                    MessageDialog.ShowPromptMessage(m_strError);
                }
                else
                {
                    MessageDialog.ShowPromptMessage("生成成功");
                    RefreshData();
                }
            }
        }

        private void tbsGoods_OnCompleteSearch()
        {
            tbsGoods.Text = tbsGoods.DataResult["物品名称"].ToString();
            tbsGoods.Tag = tbsGoods.DataResult["序号"];
            txtGoodsCode.Text = tbsGoods.DataResult["图号型号"].ToString();
            txtSpce.Text = tbsGoods.DataResult["规格"].ToString();
            lbUnit.Text = tbsGoods.DataResult["单位"].ToString();
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }
            else
            {
                tbsGoods.Text = dataGridView1.CurrentRow.Cells["物品名称"].Value.ToString();
                tbsGoods.Tag = dataGridView1.CurrentRow.Cells["GoodsID"].Value;
                txtGoodsCode.Text = dataGridView1.CurrentRow.Cells["图号型号"].Value.ToString();
                txtSpce.Text = dataGridView1.CurrentRow.Cells["规格"].Value.ToString();
                lbUnit.Text = dataGridView1.CurrentRow.Cells["单位"].Value.ToString();
                numSafeStockCount.Value = Convert.ToDecimal( dataGridView1.CurrentRow.Cells["安全库存"].Value);

                DataTable dt = m_productListServer.GetProductInfo();

                m_dicNumber = new Dictionary<string, int>();

                foreach (DataRow dr in dt.Rows)
                {
                    m_dicNumber.Add(dr["产品编码"].ToString(), 
                        Convert.ToInt32(dataGridView1.CurrentRow.Cells[dr["产品编码"].ToString()].Value));
                }
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            ClearData();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            OperatorInfo(CE_OperatorMode.添加);

            if (MessageDialog.ShowEnquiryMessage("是否要新增物品的供应商并设置供应商配额？") == DialogResult.Yes)
            {
                供应商配额设置 form = new 供应商配额设置(m_nodeInfo, Convert.ToInt32(tbsGoods.Tag));
                form.ShowDialog();
            }

            MessageDialog.ShowPromptMessage("提醒：如有需要请及时设置此物品的采购公式");
            PositioningRecord(Convert.ToInt32(tbsGoods.Tag));
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            OperatorInfo(CE_OperatorMode.修改);

            if (MessageDialog.ShowEnquiryMessage("是否要修改物品的供应商并设置供应商配额？") == DialogResult.Yes)
            {
                供应商配额设置 form = new 供应商配额设置(m_nodeInfo, Convert.ToInt32(tbsGoods.Tag));
                form.ShowDialog();
            }

            MessageDialog.ShowPromptMessage("提醒：如有需要请及时设置此物品的采购公式");
            PositioningRecord(Convert.ToInt32(tbsGoods.Tag));
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            OperatorInfo(CE_OperatorMode.删除);
        }

        private void CBOM客户物料清单_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_nodeInfo.Authority);
        }

        private void btnOutExcel_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void btnOutExcelInfo_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DataTableToExcel(saveFileDialog1, m_serverCBOM.GetSynthesisInfo(), null);
        }

        private void btnInExcel_Click(object sender, EventArgs e)
        {
            DataTable dtTemp = ExcelHelperP.RenderFromExcel(openFileDialog1);

            if (dtTemp == null)
            {
                MessageDialog.ShowPromptMessage("文件无效，转换失败");
                return;
            }

            if (!dtTemp.Columns.Contains("图号型号"))
            {
                MessageDialog.ShowPromptMessage("无【图号型号】列");
                return;
            }

            if (!dtTemp.Columns.Contains("物品名称"))
            {
                MessageDialog.ShowPromptMessage("无【物品名称】列");
                return;
            }

            if (!dtTemp.Columns.Contains("规格"))
            {
                MessageDialog.ShowPromptMessage("无【规格】列");
                return;
            }

            if (!dtTemp.Columns.Contains("基数"))
            {
                MessageDialog.ShowPromptMessage("无【基数】列");
                return;
            }

            if (!dtTemp.Columns.Contains("总成型号"))
            {
                MessageDialog.ShowPromptMessage("无【总成型号】列");
                return;
            }

            if (!dtTemp.Columns.Contains("安全库存"))
            {
                MessageDialog.ShowPromptMessage("无【安全库存】列");
                return;
            }

            try
            {
                m_serverCBOM.BatchInsertCGBom(dtTemp);
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
                return;
            }

            RefreshData();
            MessageDialog.ShowPromptMessage("导入完成");
        }
    }
}
