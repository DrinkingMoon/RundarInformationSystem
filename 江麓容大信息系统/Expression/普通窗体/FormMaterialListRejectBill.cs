using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using AsynSocketService;
using System.Net;
using System.Net.Sockets;
using SocketCommDefiniens;
using ServerRequestProcessorModule;
using GlobalObject;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 采购退货物品详单
    /// </summary>
    public partial class FormMaterialListRejectBill : Form
    {
        #region 成员变量

        /// <summary>
        /// 库房信息服务组件
        /// </summary>
        IStorageInfo m_serverStorageInfo = ServerModuleFactory.GetServerModule<IStorageInfo>();

        /// <summary>
        /// 库房ID 
        /// </summary>
        string m_strStorage;


        /// <summary>
        /// 错误信息
        /// </summary>
        string m_error;

        /// <summary>
        /// 操作模式
        /// </summary>
        CE_BusinessOperateMode m_operateMode;

        /// <summary>
        /// 采购退货单号
        /// </summary>
        string m_billNo;

        /// <summary>
        /// 采购退货单物品清单服务
        /// </summary>
        IMaterialListRejectBill m_goodsServer = ServerModuleFactory.GetServerModule<IMaterialListRejectBill>();

        /// <summary>
        /// 查询到的物品信息集
        /// </summary>
        IEnumerable<View_S_MaterialListRejectBill> m_queryGoodsInfo;

        /// <summary>
        /// 数据定位控件
        /// </summary>
        UserControlDataLocalizer m_dataLocalizer;

        /// <summary>
        /// 供应商
        /// </summary>
        string m_strProvider;

        /// <summary>
        /// 图号型号
        /// </summary>
        string m_strGoodsCode;

        /// <summary>
        /// 物品名称
        /// </summary>
        string m_strGoodsName;

        /// <summary>
        /// 规格
        /// </summary>
        string m_strSpec;

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="operateMode">操作模式</param>
        /// <param name="orderFormNumber">订单号</param>
        /// <param name="billNo">采购退货单号</param>
        public FormMaterialListRejectBill(CE_BusinessOperateMode operateMode, string vProvider, string billNo)
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw |
                ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();

            m_operateMode = operateMode;
            m_strProvider = vProvider;
            m_billNo = billNo;

            if (m_operateMode == CE_BusinessOperateMode.查看)
            {
                foreach (ToolStripItem item in toolStrip1.Items)
                {
                    if (item.Tag != null && item.Tag.ToString().Trim().ToLower() == "view")
                    {
                        item.Visible = true;
                    }
                    else
                    {
                        item.Visible = false;
                    }
                }
            }

            m_queryGoodsInfo = m_goodsServer.GetGoods(m_billNo);
            RefreshDataGridView(m_queryGoodsInfo);

            // 添加数据定位控件
            m_dataLocalizer = new UserControlDataLocalizer(
                dataGridView1, this.Name, UniversalFunction.SelectHideFields(
                this.Name, dataGridView1.Name, BasicInfo.LoginID));

            panelTop.Controls.Add(m_dataLocalizer);
            m_dataLocalizer.Dock = DockStyle.Bottom;

            m_strStorage = m_serverStorageInfo.GetStorageID(billNo, "S_MaterialRejectBill", "Bill_ID");
        }

        /// <summary>
        /// 清除窗体上的信息
        /// </summary>
        void ClearControl()
        {
            txtCode.Text = "";
            txtCode.Tag = null;
            txtName.Text = "";
            txtSpec.Text = "";
            txtProvider.Text = "";

            txtBatchNo.Text = "";

            numAmount.Value = 0;

            txtUnit.Text = "";
            txtDepot.Text = "";
            txtShelf.Text = "";
            txtColumn.Text = "";
            txtLayer.Text = "";

            txtRemark.Text = "";
        }

        /// <summary>
        /// 刷新控件
        /// </summary>
        void RefreshControl()
        {
            ClearControl();

            if (dataGridView1.CurrentRow != null)
            {
                View_S_MaterialListRejectBill goods = GetGoodsInfo(dataGridView1.CurrentRow);

                txtCode.Text = goods.图号型号;
                txtCode.Tag = goods.物品ID;
                txtName.Text = goods.物品名称;
                txtSpec.Text = goods.规格;
                txtProvider.Text = goods.供应商;
                txtAssociateID.Text = goods.关联单号;

                if (!GlobalObject.GeneralFunction.IsNullOrEmpty(goods.批次号))
                {
                    txtBatchNo.Text = goods.批次号;
                }

                numAmount.Value = goods.退货数;

                txtUnit.Text = goods.单位;
                txtDepot.Text = goods.物品类别;
                txtShelf.Text = goods.货架;
                txtColumn.Text = goods.列;
                txtLayer.Text = goods.层;

                txtRemark.Text = goods.备注;
            }
        }

        /// <summary>
        /// 检测有关数据项内容是否正确
        /// </summary>
        /// <returns>返回检测结果</returns>
        bool CheckDataItem()
        {
            if (txtName.Text.Length == 0)
            {
                MessageDialog.ShowPromptMessage("请选择物品信息");
                return false;
            }

            if (numAmount.Value == 0)
            {
                numAmount.Focus();
                MessageDialog.ShowPromptMessage("采购退货数量必须 > 0");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 从界面获取图号、名称、规格
        /// </summary>
        void GetCodeInfoFromForm()
        {
            m_strGoodsCode = txtCode.Text;
            m_strGoodsName = txtName.Text;
            m_strSpec = txtSpec.Text;
        }

        /// <summary>
        /// 从行记录中提取物品对象信息
        /// </summary>
        /// <param name="row">行记录</param>
        /// <returns>提取的物品信息</returns>
        View_S_MaterialListRejectBill GetGoodsInfo(DataGridViewRow row)
        {
            if (row == null)
            {
                return null;
            }

            View_S_MaterialListRejectBill goods = new View_S_MaterialListRejectBill();

            goods.序号 = (long)row.Cells["序号"].Value;
            goods.退货单号 = (string)row.Cells["退货单号"].Value;
            goods.物品ID = (int)row.Cells["物品ID"].Value;
            goods.图号型号 = (string)row.Cells["图号型号"].Value;
            goods.物品名称 = (string)row.Cells["物品名称"].Value;
            goods.规格 = (string)row.Cells["规格"].Value;
            goods.供应商 = (string)row.Cells["供应商"].Value;
            goods.批次号 = (string)row.Cells["批次号"].Value;
            goods.供方批次 = (string)row.Cells["供方批次"].Value;
            goods.退货数 = (decimal)row.Cells["退货数"].Value;
            goods.备注 = (string)row.Cells["备注"].Value;
            goods.关联单号 = (string)row.Cells["关联单号"].Value;

            View_F_GoodsPlanCost basicGoodsInfo = null;

            if (row.Cells["单位"].Value != System.DBNull.Value)
            {
                goods.单位 = (string)row.Cells["单位"].Value;
                goods.物品类别 = (string)row.Cells["物品类别"].Value;
                goods.货架 = (string)row.Cells["货架"].Value;
                goods.列 = (string)row.Cells["列"].Value;
                goods.层 = (string)row.Cells["层"].Value;
            }
            else
            {
                IBasicGoodsServer basicGoodsServer = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();
                basicGoodsInfo = basicGoodsServer.GetGoodsInfo(goods.图号型号, goods.物品名称, goods.规格, out m_error);

                if (!GlobalObject.GeneralFunction.IsNullOrEmpty(m_error))
                {
                    MessageDialog.ShowErrorMessage(m_error);
                    return null;
                }

                goods.单位 = basicGoodsInfo.单位;
            }

            return goods;
        }

        /// <summary>
        /// 检查相同物品
        /// </summary>
        /// <param name="Dt">需要检测的数据集</param>
        /// <param name="goods">物品明细信息</param>
        /// <returns>不相同返回True，相同返回False</returns>
        bool CheckSameGoods(DataTable dt, S_MaterialListRejectBill goods)
        {
            
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["物品ID"].ToString() == goods.GoodsID.ToString()
                    && dt.Rows[i]["批次号"].ToString() == goods.BatchNo.ToString())
                {
                    MessageBox.Show("不能添加同一个物品，请重新确认物品","提示");
                    return false;
                }
            }

            return true;
        }

        #region 刷新数据

        /// <summary>
        /// 刷新DataGridView
        /// </summary>
        /// <param name="goods">物品信息</param>
        void RefreshDataGridView(IEnumerable<View_S_MaterialListRejectBill> goods)
        {
            if (goods == null)
            {
                return;
            }

            this.dataGridView1.Visible = false;
            this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);
            this.dataGridView1.CellEnter -= new DataGridViewCellEventHandler(this.dataGridView1_CellEnter);

            dataGridView1.DataSource = goods;
            dataGridView1.Columns["单价"].Visible = false;
            dataGridView1.Columns["金额"].Visible = false;
            dataGridView1.Columns["总金额"].Visible = false;

            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);
            this.dataGridView1.CellEnter += new DataGridViewCellEventHandler(this.dataGridView1_CellEnter);

            dataGridView1.Refresh();

            dataGridView1.Columns["序号"].Visible = false;
            dataGridView1.Columns["退货单号"].Visible = false;
            dataGridView1.Columns["物品ID"].Visible = false;

            this.dataGridView1.Visible = true;
            lblAmount.Text = goods.Count().ToString();
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="goodsCode">定位用的编码</param>
        /// <param name="goodsName">定位用的名称</param>
        /// <param name="spec">定位用的规格</param>
        void PositioningRecord(string goodsCode, string goodsName, string spec)
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
                if ((string)dataGridView1.Rows[i].Cells["图号型号"].Value == goodsCode &&
                    (string)dataGridView1.Rows[i].Cells["物品名称"].Value == goodsName &&
                    (string)dataGridView1.Rows[i].Cells["规格"].Value == spec)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                    break;
                }
            }
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="rowIndex">定位行号</param>
        void PositioningRecord(int rowIndex)
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

            if (dataGridView1.Rows.Count > 0 && rowIndex < dataGridView1.Rows.Count)
            {
                dataGridView1.FirstDisplayedScrollingRowIndex = rowIndex;
                dataGridView1.CurrentCell = dataGridView1.Rows[rowIndex].Cells[strColName];
            }
        }

        #endregion 刷新数据

        /// <summary>
        /// 查找物品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFindCode_Click(object sender, EventArgs e)
        {
            if (txtAssociateID.Text.Trim() == "")
            {
                MessageBox.Show("请先选择关联单号", "提示");
                return;
            }

            if (m_operateMode != CE_BusinessOperateMode.仓库核实)
            {

                FormQueryInfo form = QueryInfoDialog.GetStoreGoodsInfoDialog(txtAssociateID.Text, m_strProvider, m_strStorage);

                if (form != null && form.ShowDialog() == DialogResult.OK)
                {
                    txtCode.Text = (string)form.GetDataItem("图号型号");
                    txtCode.Tag = (int)form.GetDataItem("物品ID");
                    txtName.Text = (string)form.GetDataItem("物品名称");
                    txtSpec.Text = (string)form.GetDataItem("规格");
                    txtProvider.Text = (string)form.GetDataItem("供货单位");
                    txtProviderBatchNo.Text = (string)form.GetDataItem("供方批次");
                    txtBatchNo.Text = (string)form.GetDataItem("批次号");

                    txtUnit.Text = (string)form.GetDataItem("单位");
                    txtDepot.Text = (string)form.GetDataItem("物品类别名称");
                    txtDepot.Tag = (string)form.GetDataItem("物品类别");

                    txtShelf.Text = (string)form.GetDataItem("货架");
                    txtColumn.Text = (string)form.GetDataItem("列");
                    txtLayer.Text = (string)form.GetDataItem("层");
                }
            }
            else if (m_operateMode != CE_BusinessOperateMode.修改)
            {
                FormQueryInfo form = QueryInfoDialog.GetOrderFormGoodsStockInfoDialog(txtAssociateID.Text, m_strStorage);

                if (form.ShowDialog() == DialogResult.OK)
                {
                    txtCode.Text = form.GetStringDataItem("图号型号");
                    txtCode.Tag = (int)form.GetDataItem("物品ID");
                    txtName.Text = form.GetStringDataItem("物品名称");
                    txtSpec.Text = form.GetStringDataItem("规格");
                    txtProvider.Text = form.GetStringDataItem("供应商");

                    txtBatchNo.Text = form.GetStringDataItem("批次号");

                    txtProviderBatchNo.Text = form.GetStringDataItem("供方批次");
                    txtUnit.Text = form.GetStringDataItem("单位");
                    txtDepot.Text = form.GetStringDataItem("物品类别");
                    txtShelf.Text = form.GetStringDataItem("货架");
                    txtColumn.Text = form.GetStringDataItem("列");
                    txtLayer.Text = form.GetStringDataItem("层");
                }
            }
        }

        private void FormMaterialListRejectBill_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            ClearControl();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!CheckDataItem())
            {
                return;
            }

            if (!m_goodsServer.IsGoodsStockThan((int)txtCode.Tag,txtBatchNo.Text,
                numAmount.Value, txtProvider.Text, m_strStorage))
            {
                MessageDialog.ShowPromptMessage("库存不足，请重新填写库存");
                return;
            }

            S_MaterialListRejectBill goods = new S_MaterialListRejectBill();

            goods.Bill_ID = m_billNo;
            goods.GoodsID = (int)txtCode.Tag;
            goods.Provider = txtProvider.Text;
            goods.ProviderBatchNo = txtProviderBatchNo.Text;
            goods.BatchNo = txtBatchNo.Text;
            goods.Amount = numAmount.Value;
            goods.Remark = txtRemark.Text;
            goods.AssociateID = txtAssociateID.Text;

            IQueryable<View_S_MaterialListRejectBill> IQReject = dataGridView1.DataSource as IQueryable<View_S_MaterialListRejectBill>;
            DataTable dvt = GlobalObject.GeneralFunction.ConvertToDataTable<View_S_MaterialListRejectBill>(IQReject);

            if (CheckSameGoods(dvt, goods))
            {
                if (!m_goodsServer.AddGoods(txtAssociateID.Text, goods, m_strStorage, out m_error))
                {
                    MessageDialog.ShowErrorMessage(m_error);
                    return;
                }
            }

            GetCodeInfoFromForm();
            m_queryGoodsInfo = m_goodsServer.GetGoods(m_billNo);
            RefreshDataGridView(m_queryGoodsInfo);
            PositioningRecord(m_strGoodsCode, m_strGoodsName, m_strSpec);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择要修改的记录后再进行此操作");
                return;
            }
            else if (dataGridView1.SelectedRows.Count > 1)
            {
                MessageDialog.ShowPromptMessage("只能选择要修改的一条记录后再进行此操作");
                return;
            }

            if (!CheckDataItem())
            {
                return;
            }

            S_MaterialListRejectBill goods = new S_MaterialListRejectBill();
            View_S_MaterialListRejectBill viewGoods = GetGoodsInfo(dataGridView1.SelectedRows[0]);

            goods.ID = viewGoods.序号;
            goods.Bill_ID = m_billNo;

            if (txtCode.Tag != null && (int)txtCode.Tag != 0)
            {
                goods.GoodsID = (int)txtCode.Tag;
            }
            else
            {
                goods.GoodsID = viewGoods.物品ID;
            }

            goods.Provider = txtProvider.Text;
            goods.ProviderBatchNo = txtProviderBatchNo.Text;
            goods.BatchNo = txtBatchNo.Text;
            goods.Amount = numAmount.Value;
            goods.Remark = txtRemark.Text;

            if (!m_goodsServer.UpdateGoods(txtAssociateID.Text, goods,m_strStorage, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            GetCodeInfoFromForm();
            m_queryGoodsInfo = m_goodsServer.GetGoods(m_billNo);
            RefreshDataGridView(m_queryGoodsInfo);
            PositioningRecord(m_strGoodsCode, m_strGoodsName, m_strSpec);
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            RefreshControl();

            lblRecordRow.Text = (e.RowIndex + 1).ToString();

            if (m_dataLocalizer != null && e.RowIndex > -1)
            {
                m_dataLocalizer.StartIndex = e.RowIndex;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择要删除的记录后再进行此操作");
                return;
            }

            string info = string.Format("您当前选择了 {0} 条记录, 是否确定删除？", dataGridView1.SelectedRows.Count);

            if (MessageDialog.ShowEnquiryMessage(info) == DialogResult.No)
            {
                return;
            }

            int rowIndex = dataGridView1.SelectedRows[0].Index;
            List<long> lstId = new List<long>(dataGridView1.SelectedRows.Count);

            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                lstId.Add((long)row.Cells["序号"].Value);
            }

            if (!m_goodsServer.DeleteGoods(lstId , out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            m_queryGoodsInfo = m_goodsServer.GetGoods(m_billNo);
            RefreshDataGridView(m_queryGoodsInfo);
            PositioningRecord(rowIndex);
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                View_S_MaterialListRejectBill curGoods = GetGoodsInfo(dataGridView1.Rows[i]);

                if (i != dataGridView1.Rows.Count - 1)
                {
                    View_S_MaterialListRejectBill nextGoods = GetGoodsInfo(dataGridView1.Rows[i+1]);

                    if (curGoods.图号型号 == nextGoods.图号型号 && curGoods.物品名称 == nextGoods.物品名称)
                    {
                        dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.Blue;
                        dataGridView1.Rows[i+1].DefaultCellStyle.ForeColor = Color.Blue;
                    }
                }
            }
        }

        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            if (MessageDialog.ShowEnquiryMessage("您真的想删除物品清单中的所有数据吗？") == DialogResult.Yes)
            {
                if (!m_goodsServer.DeleteGoods(m_billNo, out m_error))
                {
                    MessageDialog.ShowErrorMessage(m_error);
                    return;
                }

                m_queryGoodsInfo = m_goodsServer.GetGoods(m_billNo);
                RefreshDataGridView(m_queryGoodsInfo);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (m_operateMode != CE_BusinessOperateMode.仓库核实)
            {
                FormQueryInfo form = QueryInfoDialog.GetOrderFormInfoDialog(m_strProvider);

                if (DialogResult.OK == form.ShowDialog())
                {
                    txtAssociateID.Text = form.GetDataItem("订单号").ToString();
                }

            }
        }

        private void btnBatchCreate_Click(object sender, EventArgs e)
        {
            退货业务报废物品筛选窗体 Form = new 退货业务报废物品筛选窗体(m_strProvider);
            Form.ShowDialog();

            DataTable dt = Form.DtScrap;

            if (dt == null || dt.Rows.Count == 0)
            {
                return;
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                S_MaterialListRejectBill goods = new S_MaterialListRejectBill();

                goods.Bill_ID = m_billNo;
                goods.GoodsID = Convert.ToInt32(dt.Rows[i]["GoodsID"].ToString());
                goods.Provider = dt.Rows[i]["Provider"].ToString();
                goods.ProviderBatchNo = "";
                goods.BatchNo = dt.Rows[i]["BatchNo"].ToString();
                goods.Amount = Convert.ToDecimal(dt.Rows[i]["Quantity"].ToString());
                goods.Remark = dt.Rows[i]["Reason"].ToString();
                goods.AssociateID = dt.Rows[i]["Bill_ID"].ToString();

                IQueryable<View_S_MaterialListRejectBill> IQReject = dataGridView1.DataSource as IQueryable<View_S_MaterialListRejectBill>;
                DataTable dvt = GlobalObject.GeneralFunction.ConvertToDataTable<View_S_MaterialListRejectBill>(IQReject);

                if (CheckSameGoods(dvt, goods))
                {
                    if (!m_goodsServer.AddGoods(txtAssociateID.Text, goods, m_strStorage, out m_error))
                    {
                        MessageDialog.ShowErrorMessage(m_error);
                        return;
                    }
                }
            }

            GetCodeInfoFromForm();
            m_queryGoodsInfo = m_goodsServer.GetGoods(m_billNo);
            RefreshDataGridView(m_queryGoodsInfo);
        }

        private void btnSelectScraptBill_Click(object sender, EventArgs e)
        {

            对应的隔离单 Form = new 对应的隔离单(txtAssociateID.Text.Trim(), m_strStorage);
            Form.ShowDialog();

            DataTable dt = Form.m_dtIsolation;

            if (dt == null || dt.Rows.Count == 0)
            {
                return;
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                S_MaterialListRejectBill goods = new S_MaterialListRejectBill();

                goods.Bill_ID = m_billNo;
                goods.GoodsID = Convert.ToInt32(dt.Rows[i]["物品ID"].ToString());
                goods.Provider = dt.Rows[i]["供货单位"].ToString();
                goods.ProviderBatchNo = dt.Rows[i]["供方批次号"].ToString();
                goods.BatchNo = dt.Rows[i]["批次号"].ToString();
                goods.Amount = Convert.ToDecimal(dt.Rows[i]["退货数"].ToString());
                goods.Remark = "隔离退货单【" + dt.Rows[i]["隔离单号"].ToString() + "】";
                goods.AssociateID = txtAssociateID.Text;
                IQueryable<View_S_MaterialListRejectBill> IQReject = dataGridView1.DataSource as IQueryable<View_S_MaterialListRejectBill>;
                DataTable dvt = GlobalObject.GeneralFunction.ConvertToDataTable<View_S_MaterialListRejectBill>(IQReject);

                if (CheckSameGoods(dvt, goods))
                {
                    if (!m_goodsServer.AddGoods(txtAssociateID.Text, goods, m_strStorage, out m_error))
                    {
                        MessageDialog.ShowErrorMessage(m_error);
                        return;
                    }
                }
            }

            GetCodeInfoFromForm();
            m_queryGoodsInfo = m_goodsServer.GetGoods(m_billNo);
            RefreshDataGridView(m_queryGoodsInfo);
        }

        private void btnPrintBarCode_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentRow == null)
                {
                    throw new Exception("选择打印的条形码记录行不允许为空!");
                }

                List<View_S_InDepotGoodsBarCodeTable> lstBarCodeInfo = new List<View_S_InDepotGoodsBarCodeTable>();
                IBarCodeServer serviceBarCode = ServerModuleFactory.GetServerModule<IBarCodeServer>();

                foreach (DataGridViewRow dgvr in this.dataGridView1.SelectedRows)
                {
                    View_S_InDepotGoodsBarCodeTable barcode = new View_S_InDepotGoodsBarCodeTable();

                    QueryCondition_Store tempInfo = new QueryCondition_Store();

                    tempInfo.BatchNo = dgvr.Cells["批次号"].Value.ToString();
                    tempInfo.GoodsID = Convert.ToInt32(dgvr.Cells["物品ID"].Value);
                    tempInfo.Provider = m_strProvider;
                    tempInfo.StorageID = m_strStorage;

                    S_Stock stockInfo = UniversalFunction.GetStockInfo(tempInfo);

                    View_F_GoodsPlanCost goodsInfo = UniversalFunction.GetGoodsInfo(tempInfo.GoodsID);

                    barcode.条形码 =
                        serviceBarCode.GetBarCode(stockInfo.GoodsID, stockInfo.BatchNo, stockInfo.StorageID, stockInfo.Provider);

                    barcode.图号型号 = goodsInfo.图号型号;
                    barcode.物品名称 = goodsInfo.物品名称;
                    barcode.规格 = goodsInfo.规格;
                    barcode.供货单位 = stockInfo.Provider;
                    barcode.批次号 = stockInfo.BatchNo;
                    barcode.货架 = stockInfo.ShelfArea;
                    barcode.层 = stockInfo.LayerNumber;
                    barcode.列 = stockInfo.ColumnNumber;
                    barcode.材料类别编码 = dgvr.Cells["退货数"].Value.ToString();
                    barcode.物品ID = stockInfo.GoodsID;

                    lstBarCodeInfo.Add(barcode);
                }

                foreach (var item in lstBarCodeInfo)
                {
                    ServerModule.PrintPartBarcode.PrintBarcodeList(item, Convert.ToDecimal(item.材料类别编码));
                }

                MessageBox.Show("条码全部打印完成");
            }
            catch (Exception ex)
            {
                MessageDialog.ShowErrorMessage(ex.Message);
            }
        }
    }
}
