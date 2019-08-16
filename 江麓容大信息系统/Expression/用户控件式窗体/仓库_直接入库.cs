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
    /// 明细库存信息界面
    /// </summary>
    public partial class 仓库_直接入库 : Form
    {
        /// <summary>
        /// 库房信息服务组件
        /// </summary>
        IStorageInfo m_serverStorageInfo = ServerModuleFactory.GetServerModule<IStorageInfo>();

        /// <summary>
        /// 库存服务
        /// </summary>
        IStoreServer m_storeServer = ServerModuleFactory.GetServerModule<IStoreServer>();

        /// <summary>
        /// 条形码
        /// </summary>
        IBarCodeServer m_barCodeServer = ServerModuleFactory.GetServerModule<IBarCodeServer>();

        /// <summary>
        /// 基础物品服务
        /// </summary>
        IBasicGoodsServer m_basicGoodsServer = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();

        /// <summary>
        /// 单位服务
        /// </summary>
        IUnitServer m_unitServer = ServerModuleFactory.GetServerModule<IUnitServer>();

        /// <summary>
        /// 数据定位控件
        /// </summary>
        UserControlDataLocalizer m_dataLocalizer;

        /// <summary>
        /// 仓库库存信息
        /// </summary>
        IQueryable<View_S_Stock> m_stockInfo;

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_error;

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authorityFlag;

        /// <summary>
        /// 库存数据集
        /// </summary>
        DataTable m_dtSource = new DataTable();

        public 仓库_直接入库(FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authorityFlag = nodeInfo.Authority;

            StapleInfo.InitUnitComboBox(cmbUnit);
            StapleInfo.InitStoreStateComboBox(cmbGoodsStatus);
            
            DisableControl();

            if ((m_authorityFlag & AuthorityFlag.Edit) == AuthorityFlag.Nothing)
            {
                numFactUnitPrice.Visible = false;
                numFactPrice.Visible = false;
                lblFactPrice.Visible = false;
                lblFactUnitPrice.Visible = false;
            }
            else
            {
                numFactUnitPrice.Visible = true;
                numFactPrice.Visible = true;
                lblFactPrice.Visible = true;
                lblFactUnitPrice.Visible = true;
            }

            QueryStock();

            // 添加数据定位控件
            m_dataLocalizer = new UserControlDataLocalizer(dataGridView1, this.Name, 
                UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
            panelPara.Controls.Add(m_dataLocalizer);
            m_dataLocalizer.Dock = DockStyle.Bottom;

            txtColumn.Enabled = true;
            txtLayer.Enabled = true;
            txtShelf.Enabled = true;
            txtRemark.Enabled = true;

            DataTable dt = UniversalFunction.GetStorageTb();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cmbStorage.Items.Add(dt.Rows[i]["StorageName"].ToString());
            }

            cmbStorage.SelectedIndex = -1;
        }

        private void 仓库_直接入库_Load(object sender, EventArgs e)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip1, m_authorityFlag);
        }

        /// <summary>
        /// 根据操作权限禁用界面元素
        /// </summary>
        private void DisableControl()
        {
            foreach (Control item in panelPara.Controls)
            {
                if (item.Tag == null || item.Tag.ToString() != "会计操作")
                {
                    if (item.GetType() != typeof(Label) && (m_authorityFlag & AuthorityFlag.Add) == AuthorityFlag.Nothing)
                    {
                        item.Enabled = false;
                    }
                }
                else
                {
                    if ((m_authorityFlag & AuthorityFlag.Add) != AuthorityFlag.Nothing)
                    {
                        item.Visible = false;
                    }
                }
            }
        }

        /// <summary>
        /// 查询库存
        /// </summary>
        private void QueryStock()
        {
            if (checkBox1.Checked)
            {
                if (!m_storeServer.GetAllStore(null, true, out m_stockInfo, out m_error))
                {
                    MessageDialog.ShowErrorMessage(m_error);
                    return;
                }
            }
            else
            {
                if (!m_storeServer.GetNoZeroStore(out m_stockInfo, out m_error))
                {
                    MessageDialog.ShowErrorMessage(m_error);
                    return;
                }
            }


            // 用来显示的库存信息
            List<View_S_Stock> showStockInfo = null;
            showStockInfo = m_stockInfo.ToList();

            RefreshDataGridView(showStockInfo);
        }

        /// <summary>
        /// 刷新DataGridView
        /// </summary>
        /// <param name="stockGoodsGroup">数据集</param>
        void RefreshDataGridView(List<View_S_Stock> stockGoodsGroup)
        {
            ClearForm();

            DataTable dtResult = GeneralFunction.ConvertToDataTable(stockGoodsGroup);
            DataTable dtStorageID = m_serverStorageInfo.GetStorageIDAndPersonnel(BasicInfo.LoginID);
            m_dtSource = dtResult.Clone();

            if (dtResult.Rows.Count != 0 
                && dtStorageID != null 
                && dtStorageID.Rows.Count > 0)
            {
                for (int i = 0; i < dtStorageID.Rows.Count; i++)
                {
                    try
                    {
                        DataRow[] dr = dtResult.Select("库房代码 = '" + dtStorageID.Rows[i]["StorageID"] + "'");

                        if (dr.Length > 0)
                        {
                            for (int a = 0; a <= dr.Length - 1; a++)
                            {
                                m_dtSource.ImportRow(dr[a]);
                            }
                        }
                    }
                    catch (Exception exce)
                    {
                        if (!exce.Message.Contains("未找到"))
                        {
                            MessageDialog.ShowErrorMessage(exce.Message);
                            return;
                        }
                    }
                }

                dataGridView1.DataSource = m_dtSource;
            }
            else
            {
                dataGridView1.DataSource = dtResult;
            }

            dataGridView1.Columns["实际单价"].Visible = false;
            dataGridView1.Columns["实际金额"].Visible = false;

            this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            if (!numFactUnitPrice.Visible)
            {
                dataGridView1.Columns["实际单价"].Visible = false;
                dataGridView1.Columns["实际金额"].Visible = false;
            }

            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Columns["序号"].Visible = false;
                dataGridView1.Columns["单位ID"].Visible = false;
            }
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            ClearForm();

            RefreshControl();

            lblRecordRow.Text = (e.RowIndex + 1).ToString();

            if (m_dataLocalizer != null && e.RowIndex > -1)
            {
                m_dataLocalizer.StartIndex = e.RowIndex;
            }
        }

        private void txtCode_TextChanged(object sender, EventArgs e)
        {
            if (txtCode.Text.Length > 0)
            {
                txtName.ReadOnly = true;
                txtSpec.ReadOnly = true;
            }
        }

        /// <summary>
        /// 清除界面
        /// </summary>
        private void ClearForm()
        {
            cmbStorage.SelectedIndex = -1;
            txtCode.Text = "";
            txtName.Text = "";
            txtSpec.Text = "";
            txtProvider.Text = "";
            txtProviderBatchNo.Text = "";
            txtBatchNo.Text = "";
            txtVersion.Text = "";
            numCount.Value = 0;
            txtMaterialType.Text = "";
            txtShelf.Text = "";
            txtColumn.Text = "";
            txtLayer.Text = "";
            txtRemark.Text = "";
            cmbGoodsStatus.SelectedIndex = 0;
            chk_PT.Checked = false;

            cmbUnit.SelectedIndex = -1;
        }

        /// <summary>
        /// 刷新控件
        /// </summary>
        private void RefreshControl()
        {
            if (dataGridView1.Rows.Count == 0)
            {
                return;
            }

            if (dataGridView1.SelectedRows.Count == 0)
            {
                dataGridView1.Rows[0].Selected = true;
            }

            View_S_Stock stockInfo = (from r in m_stockInfo
                                      where r.序号 == (int)dataGridView1.SelectedRows[0].Cells[0].Value
                                      select r).Single();

            txtCode.Text = stockInfo.图号型号;
            txtName.Text = stockInfo.物品名称;
            txtSpec.Text = stockInfo.规格;
            txtProvider.Text = stockInfo.供货单位;
            txtProviderBatchNo.Text = stockInfo.供方批次号;
            txtBatchNo.Text = stockInfo.批次号;
            txtVersion.Text = stockInfo.版次号;
            numCount.Value = (decimal)stockInfo.库存数量;
            txtMaterialType.Text = stockInfo.材料类别编码;
            txtMaterialType.Tag = stockInfo.材料类别编码;
            txtShelf.Text = stockInfo.货架;
            txtColumn.Text = stockInfo.列;
            txtLayer.Text = stockInfo.层;
            numFactUnitPrice.Value = (decimal)stockInfo.实际单价;
            numFactPrice.Value = (decimal) stockInfo.实际金额;
            cmbUnit.Text = stockInfo.单位;
            cmbGoodsStatus.Text = stockInfo.物品状态;
            dateTimeInDepot.Value = stockInfo.入库时间;
            cmbStorage.Text = UniversalFunction.GetStorageName(stockInfo.库房代码);

            if (stockInfo.备注 != null)
            {
                txtRemark.Text = stockInfo.备注;
            }
            else
            {
                txtRemark.Text = "";
            }
        }

        #region 检查数据
        /// <summary>
        /// 检查是否正确选择操作的记录行
        /// </summary>
        /// <returns>正确返回true</returns>
        private bool CheckSelectedRow()
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择一条记录后再进行此操作");
                return false;
            }
            else if (dataGridView1.SelectedRows.Count > 1)
            {
                MessageDialog.ShowPromptMessage("只允许选择一条记录进行处理！");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 检查数据
        /// </summary>
        /// <returns></returns>
        private bool CheckData()
        {
            txtName.Text = txtName.Text.Trim();
            txtCode.Text = txtCode.Text.Trim();
            txtSpec.Text = txtSpec.Text.Trim();

            if (txtName.Text.Length == 0)
            {
                MessageDialog.ShowPromptMessage("物品名称不能为空");
                return false;
            }

            if (txtCode.Text.Trim().Length > 0)
            {
                if (txtBatchNo.Text.Length == 0)
                {
                    MessageDialog.ShowPromptMessage("产品类物品，批次号不能为空");
                    return false;
                }
            }

            if (txtVersion.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("产品类的物品,版本号不能为空");
                return false;
            }

            if (txtProvider.Text.Length == 0)
            {
                txtProvider.Focus();
                MessageDialog.ShowPromptMessage("供货单位不允许为空!");
                return false;
            }

            if (txtMaterialType.Text.Length == 0)
            {
                txtMaterialType.Focus();
                MessageDialog.ShowPromptMessage("材料类别不能为空");
                return false;
            }

            if (cmbStorage.Text == "")
            {
                cmbStorage.Focus();
                MessageDialog.ShowPromptMessage("所属库房不允许为空!");
                return false;
            }

            if (cmbUnit.Text == "")
            {
                cmbUnit.Focus();
                MessageDialog.ShowPromptMessage("单位不允许为空!");
                return false;
            }

            if (numCount.Value == 0)
            {
                numCount.Focus();
                MessageDialog.ShowPromptMessage("库存数量必须 > 0");
                return false;
            }

            return true;
        } 
        #endregion

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            QueryStock();
        }

        /// <summary>
        /// 根据界面信息生成库存对象
        /// </summary>
        /// <returns>生成的库存对象</returns>
        private S_Stock CreateStockObject()
        {
            View_F_GoodsPlanCost info = m_basicGoodsServer.GetGoodsInfo(txtCode.Text, txtName.Text, txtSpec.Text, out m_error);
            S_Stock stockInfo = new S_Stock();

            stockInfo.GoodsID = info.序号;
            stockInfo.GoodsCode = txtCode.Text;
            stockInfo.GoodsName = txtName.Text;
            stockInfo.Spec = txtSpec.Text;
            stockInfo.Provider = txtProvider.Text;
            stockInfo.ProviderBatchNo = txtProviderBatchNo.Text;
            stockInfo.BatchNo = txtBatchNo.Text;
            stockInfo.Version = txtVersion.Text.Trim();
            stockInfo.ShelfArea = txtShelf.Text.Trim().ToUpper();
            stockInfo.ColumnNumber = txtColumn.Text.Trim().ToUpper();
            stockInfo.LayerNumber = txtLayer.Text.Trim().ToUpper();
            stockInfo.ExistCount = numCount.Value;
            stockInfo.Remark = txtRemark.Text;
            stockInfo.UnitPrice = numFactUnitPrice.Value;
            stockInfo.InputPerson = BasicInfo.LoginID;
            stockInfo.GoodsStatus = (int)cmbGoodsStatus.SelectedValue;
            stockInfo.StorageID = UniversalFunction.GetStorageID(cmbStorage.Text);
            
            return stockInfo;
        }

        /// <summary>
        /// 创建新的F_GoodsPlanCost
        /// </summary>
        /// <returns>返回基础物品信息</returns>
        private F_GoodsPlanCost CreateGoodsObject()
        {
            F_GoodsPlanCost goodscost = new F_GoodsPlanCost();

            goodscost.GoodsCode = txtCode.Text;
            goodscost.GoodsName = txtName.Text.Trim();
            goodscost.Spec = txtSpec.Text;
            goodscost.GoodsType = txtMaterialType.Tag as string;
            goodscost.GoodsUnitPrice = numFactUnitPrice.Value;
            goodscost.Remark = "";
            goodscost.UserCode = "";
            goodscost.UnitID = m_unitServer.GetUnitInfo(cmbUnit.Text).序号;
            goodscost.PY = UniversalFunction.GetPYWBCode(goodscost.GoodsName,"PY");
            goodscost.WB = UniversalFunction.GetPYWBCode(goodscost.GoodsName,"WB");

            return goodscost;
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="stockInfo">库存信息</param>
        /// <returns>定位是否成功的标志</returns>
        bool PositioningRecord(S_Stock stockInfo)
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
                if ((string)dataGridView1.Rows[i].Cells["图号型号"].Value == stockInfo.GoodsCode &&
                    (string)dataGridView1.Rows[i].Cells["物品名称"].Value == stockInfo.GoodsName &&
                    (string)dataGridView1.Rows[i].Cells["规格"].Value == stockInfo.Spec &&
                    (string)dataGridView1.Rows[i].Cells["供货单位"].Value == stockInfo.Provider &&
                    (string)dataGridView1.Rows[i].Cells["批次号"].Value == stockInfo.BatchNo &&
                    (string)dataGridView1.Rows[i].Cells["库房代码"].Value == stockInfo.StorageID)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 更新基础物品表
        /// </summary>
        /// <returns>返回操作是否成功的标志</returns>
        private bool UpdateBasicGoodsInfo()
        {
            View_F_GoodsPlanCost info = m_basicGoodsServer.GetGoodsInfo(txtCode.Text, txtName.Text, txtSpec.Text, out m_error);

            if (info != null)
            {
                if (info.物品类别名称 != txtMaterialType.Text || info.单位 != cmbUnit.Text)
                {
                    F_GoodsPlanCost basicGoods = m_basicGoodsServer.GetGoodsInfo(info.序号);

                    basicGoods.GoodsType = txtMaterialType.Tag as string;
                    basicGoods.UnitID = m_unitServer.GetUnitInfo(cmbUnit.Text).序号;

                    //if (!m_basicGoodsServer.UpdateGoods(basicGoods, out m_error))
                    //{
                    //    MessageDialog.ShowErrorMessage(m_error);
                    //    return false;
                    //}
                }
            }

            return true;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
            {
                return;
            }

            if (!CheckData())
            {
                return;
            }

            if (!UpdateBasicGoodsInfo())
            {
                return;
            }

            S_Stock stockInfo = CreateStockObject();
            stockInfo.ID = (int)dataGridView1.CurrentRow.Cells[0].Value;

            if (stockInfo.GoodsCode != dataGridView1.CurrentRow.Cells["图号型号"].Value.ToString().Trim()
                || stockInfo.GoodsName != dataGridView1.CurrentRow.Cells["物品名称"].Value.ToString().Trim()
                || stockInfo.Spec != dataGridView1.CurrentRow.Cells["规格"].Value.ToString().Trim())
            {
                MessageDialog.ShowErrorMessage("不允许修改物品的图号、名称、规格信息！");
                return;
            }

            // 获取到修改行索引
            int rowIndex = dataGridView1.CurrentRow.Index;

            if (!m_storeServer.UpdateStore(stockInfo, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            QueryStock();
            RefreshControl();


            string strColName = "";

            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                if (col.Visible)
                {
                    strColName = col.Name;
                    break;
                }
            }

            // 自动跳到下一行记录
            if (rowIndex != dataGridView1.Rows.Count - 1)
            {
                dataGridView1.CurrentCell = dataGridView1.Rows[rowIndex + 1].Cells[strColName];
            }

            dataGridView1.FirstDisplayedScrollingRowIndex = rowIndex;
            dataGridView1.Focus();
            chk_PT.Enabled = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!CheckData())
            {
                return;
            }

            if (!UpdateBasicGoodsInfo())
            {
                return;
            }

            S_Stock stockInfo = CreateStockObject();

            if (PositioningRecord(stockInfo))
            {
                MessageDialog.ShowPromptMessage("此物品信息已经存在, 不能重复添加");
                return;
            }

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            m_storeServer.InStore(ctx, stockInfo, CE_SubsidiaryOperationType.未知);
            ctx.SubmitChanges();

            QueryStock();
            RefreshControl();
            PositioningRecord(stockInfo);
            chk_PT.Enabled = false;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
            {
                return;
            }

            if (MessageDialog.ShowEnquiryMessage("您是否要删除选择行？") == DialogResult.No)
            {
                return;
            }

            // 获取到修改行索引
            int rowIndex = dataGridView1.CurrentRow.Index;
            int id = (int)dataGridView1.CurrentRow.Cells[0].Value;

            if (!m_storeServer.DeleteStore(id, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            QueryStock();
            RefreshControl();


            string strColName = "";

            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                if (col.Visible)
                {
                    strColName = col.Name;
                    break;
                }
            }

            // 自动跳到下一行记录
            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.FirstDisplayedScrollingRowIndex = rowIndex;
                dataGridView1.CurrentCell = dataGridView1.Rows[rowIndex].Cells[strColName];
            }

            chk_PT.Enabled = false;
        }

        private void btnFindCode_Click(object sender, EventArgs e)
        {
            FormQueryInfo form = QueryInfoDialog.GetPlanCostGoodsDialog(true);

            if (form != null && form.ShowDialog() == DialogResult.OK)
            {
                txtCode.Text = form.GetDataItem("图号型号").ToString();
                txtName.Text = form.GetDataItem("物品名称").ToString();
                txtSpec.Text = form.GetDataItem("规格").ToString();

                View_F_GoodsPlanCost info = m_basicGoodsServer.GetGoodsInfo(txtCode.Text, txtName.Text, txtSpec.Text, out m_error);

                if (info != null)
                {
                    cmbUnit.SelectedValue = info.单位ID;
                    txtMaterialType.Text = info.物品类别名称;
                    txtMaterialType.Tag = info.物品类别;
                }
            }
        }

        private void btnFindProvider_Click(object sender, EventArgs e)
        {
            FormQueryInfo form = QueryInfoDialog.GetProviderInfoDialog();

            if (form.ShowDialog() == DialogResult.OK)
            {
                txtProvider.Text = form.GetDataItem("供应商编码").ToString();
            }
        }

        private void btnFindMaterialType_Click(object sender, EventArgs e)
        {
            FormDepotType form = form = new FormDepotType();
            
            if (form.ShowDialog() == DialogResult.OK)
            {
                txtMaterialType.Text = form.SelectedDepotType.仓库名称;
                txtMaterialType.Tag = form.SelectedDepotType.仓库编码;
                cmbGoodsStatus.Text = "正常";
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            报表_库存信息 report = new 报表_库存信息();
            report.ShowDialog();
        }

        private void btnPrintBarCode_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count < 1)
            {
                MessageDialog.ShowPromptMessage("请选择记录后再打印条形码");
                return;
            }

            List<View_S_InDepotGoodsBarCodeTable> lstBarCodeInfo = new List<View_S_InDepotGoodsBarCodeTable>();

            for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
            {
                string batchCode = dataGridView1.SelectedRows[i].Cells["批次号"].Value.ToString();
                string goodsCode = dataGridView1.SelectedRows[i].Cells["图号型号"].Value.ToString();
                string goodsName = dataGridView1.SelectedRows[i].Cells["物品名称"].Value.ToString();
                string spec = dataGridView1.SelectedRows[i].Cells["规格"].Value.ToString();
                string provider = dataGridView1.SelectedRows[i].Cells["供货单位"].Value.ToString();
                string StorageID = dataGridView1.SelectedRows[i].Cells["库房代码"].Value.ToString();
                View_S_InDepotGoodsBarCodeTable barcode = m_barCodeServer.GetBarCodeInfo(
                                                            goodsCode, goodsName, spec, provider, batchCode,StorageID);

                // 找不到此物品的条形码时生成一个
                if (barcode == null)
                {
                    S_InDepotGoodsBarCodeTable newBarcode = new S_InDepotGoodsBarCodeTable();

                    newBarcode.GoodsID = (int)dataGridView1.SelectedRows[i].Cells["物品ID"].Value;
                    newBarcode.Provider = provider;
                    newBarcode.BatchNo = batchCode;
                    newBarcode.ProductFlag = "0";
                    newBarcode.StorageID = StorageID;

                    if (!m_barCodeServer.Add(newBarcode, out m_error))
                    {
                        MessageDialog.ShowErrorMessage(m_error);
                        return;
                    }

                    barcode = m_barCodeServer.GetBarCodeInfo(goodsCode, goodsName, spec, provider, batchCode,StorageID);
                }

                lstBarCodeInfo.Add(barcode);
            }

            foreach (var item in lstBarCodeInfo)
            {
                ServerModule.PrintPartBarcode.PrintBarcodeList(item);
            }
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewRow dr = dataGridView1.Rows[i];

                if (Convert.ToDecimal(dataGridView1.Rows[i].Cells["库存数量"].Value) < 1)
                    continue;

                if (dataGridView1.Rows[i].Cells["物品状态"].Value.ToString() == "样品")
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                }
                else if (dataGridView1.Rows[i].Cells["物品状态"].Value.ToString() == "隔离")
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                }
                else if (dataGridView1.Rows[i].Cells["物品状态"].Value.ToString() == "仅限于返修箱用")
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.PowderBlue;
                }
            }

            lblAmount.Text = dataGridView1.Rows.Count.ToString();
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == this.dataGridView1.Columns[0].Index)
            {
                DataGridViewRow CurrentRow = this.dataGridView1.Rows[e.RowIndex];
                CurrentRow.HeaderCell.Value = Convert.ToString(e.RowIndex + 1);//显示行号，也可以设置成显示其他信息
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            ClearForm();
            chk_PT.Enabled = true;
        }

        private void chk_PT_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_PT.Checked)
            {
                txtCode.Text = "";
                txtName.Text = "";
                txtSpec.Text = "";
                txtName.ReadOnly = false;
                txtSpec.ReadOnly = false;
                txtCode.ReadOnly = false;
            }
            else
            {
                txtSpec.ReadOnly = true;
                txtName.ReadOnly = true;
                txtCode.ReadOnly = true;
            }
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
        }

        private void 仓库_直接入库_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        private void txtRemark_Enter(object sender, EventArgs e)
        {
            txtRemark.Text = "";
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            QueryStock();
        }

        private void btnStockUpdate_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("您确定要修改此物品？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                S_Stock stockInfo = CreateStockObject();
                stockInfo.ID = (int)dataGridView1.CurrentRow.Cells[0].Value;

                if (stockInfo.GoodsCode != dataGridView1.CurrentRow.Cells["图号型号"].Value.ToString()
                    || stockInfo.GoodsName != dataGridView1.CurrentRow.Cells["物品名称"].Value.ToString()
                    || stockInfo.Spec != dataGridView1.CurrentRow.Cells["规格"].Value.ToString())
                {
                    MessageDialog.ShowErrorMessage("不允许修改物品的图号、名称、规格信息！");
                    return;
                }

                // 获取到修改行索引
                int rowIndex = dataGridView1.CurrentRow.Index;

                if (!m_storeServer.UpdateStore(stockInfo, out m_error))
                {
                    MessageDialog.ShowErrorMessage(m_error);
                }
                else
                {
                    MessageBox.Show("修改成功", "提示");
                }

                QueryStock();
            }
        }
    }
}
