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
    /// 领料退库物品详单
    /// </summary>
    public partial class FormMaterialListReturnedInTheDepot : Form
    {
        #region 成员变量

        /// <summary>
        /// 库房服务组件
        /// </summary>
        IStoreServer m_serverStock = ServerModuleFactory.GetServerModule<IStoreServer>();

        /// <summary>
        /// 车间耗用服务组件
        /// </summary>
        Service_Manufacture_WorkShop.IMaterialsTransfer m_serverMaterials = 
            Service_Manufacture_WorkShop.ServerModuleFactory.GetServerModule<Service_Manufacture_WorkShop.IMaterialsTransfer>();

        /// <summary>
        /// 基础物品服务组件
        /// </summary>
        IBasicGoodsServer m_serverBasicGoods = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();

        /// <summary>
        /// 箱体编码服务组件
        /// </summary>
        IProductCodeServer m_serverProductCode = ServerModuleFactory.GetServerModule<IProductCodeServer>();

        /// <summary>
        /// 库房信息服务组件
        /// </summary>
        IStorageInfo m_serverStorageInfo = ServerModuleFactory.GetServerModule<IStorageInfo>();

        /// <summary>
        /// 营销出库服务类
        /// </summary>
        ISellIn m_findSellIn = ServerModuleFactory.GetServerModule<ISellIn>();

        /// <summary>
        /// 库房代码
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
        /// 领料退库单号
        /// </summary>
        string m_billNo;

        /// <summary>
        /// 工装台帐服务组件
        /// </summary>
        IFrockStandingBook m_serverStandingBook = PMS_ServerFactory.GetServerModule<IFrockStandingBook>();

        /// <summary>
        /// 领料退库单物品清单服务
        /// </summary>
        IMaterialListReturnedInTheDepot m_goodsServer = ServerModuleFactory.GetServerModule<IMaterialListReturnedInTheDepot>();

        /// <summary>
        /// 查询到的物品信息集
        /// </summary>
        IEnumerable<View_S_MaterialListReturnedInTheDepot> m_queryGoodsInfo;

        /// <summary>
        /// 数据定位控件
        /// </summary>
        UserControlDataLocalizer m_dataLocalizer;

        /// <summary>
        /// 提示批次号是否正确
        /// </summary>
        bool m_promptBatchNo;

        /// <summary>
        /// 图号型号
        /// </summary>
        string m_goodsCode;

        /// <summary>
        /// 物品名称
        /// </summary>
        string m_goodsName;

        /// <summary>
        /// 规格
        /// </summary>
        string m_spec;

        /// <summary>
        /// 退库方式
        /// </summary>
        private string m_strReturnMode;

        public string StrReturnMode
        {
            get { return m_strReturnMode; }
            set { m_strReturnMode = value; }
        }

        /// <summary>
        /// 是否仅限于返修箱
        /// </summary>
        private bool m_blIsOnlyForRepair;

        public bool BlIsOnlyForRepair
        {
            get { return m_blIsOnlyForRepair; }
            set { m_blIsOnlyForRepair = value; }
        }

        /// <summary>
        /// 车间管理基础服务
        /// </summary>
        Service_Manufacture_WorkShop.IWorkShopBasic m_serverWSBasic =
            Service_Manufacture_WorkShop.ServerModuleFactory.GetServerModule<Service_Manufacture_WorkShop.IWorkShopBasic>();

        /// <summary>
        /// 车间管理信息
        /// </summary>
        WS_WorkShopCode m_lnqWSCode = new WS_WorkShopCode();

        /// <summary>
        /// 退库单服务组件
        /// </summary>
        IMaterialReturnedInTheDepot m_serverBill = ServerModuleFactory.GetServerModule<IMaterialReturnedInTheDepot>();

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="operateMode">操作模式</param>
        /// <param name="billNo">领料退库单号</param>
        public FormMaterialListReturnedInTheDepot(CE_BusinessOperateMode operateMode, string billNo)
        {
            InitializeComponent();

            S_MaterialReturnedInTheDepot tempBill = m_serverBill.GetBill(billNo);

            m_lnqWSCode = tempBill == null ?
                m_serverWSBasic.GetPersonnelWorkShop(BasicInfo.LoginID) :
                m_serverWSBasic.GetPersonnelWorkShop(tempBill.FillInPersonnelCode);

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | 
                ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();

            m_operateMode = operateMode;
            m_billNo = billNo;

            if (m_operateMode == CE_BusinessOperateMode.查看)
            {
                toolStrip1.Visible = false;
            }
            else if (m_operateMode == CE_BusinessOperateMode.仓库核实)
            {
                btnAdd.Enabled = false;
                btnDelete.Enabled = false;
                btnDeleteAll.Enabled = false;

                txtShelf.ReadOnly = false;
                txtColumn.ReadOnly = false;
                txtLayer.ReadOnly = false;
            }

            m_queryGoodsInfo = m_goodsServer.GetGoods(m_billNo);
            RefreshDataGridView(m_queryGoodsInfo);

            // 添加数据定位控件
            m_dataLocalizer = new UserControlDataLocalizer(
                dataGridView1, this.Name, UniversalFunction.SelectHideFields(
                this.Name, dataGridView1.Name, BasicInfo.LoginID));

            panelTop.Controls.Add(m_dataLocalizer);
            m_dataLocalizer.Dock = DockStyle.Bottom;

            m_strStorage = m_serverStorageInfo.GetStorageID(billNo, "S_MaterialReturnedInTheDepot", "Bill_ID");

            if (m_strStorage == "05")
            {
                label11.Visible = true;
                cmbProductStatus.Visible = true;
            }

            if (!Convert.ToBoolean(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.开启车间管理模块]) || m_strStorage == "11")
            {
                btnProvider.Visible = false;
                btnBatchNo.Visible = false;
            }
        }

        private void FormMaterialListReturnedInTheDepot_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        /// <summary>
        /// 查找物品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFindCode_Click(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.开启车间管理模块]) && m_lnqWSCode != null && m_strStorage != "11")
            {
                FormQueryInfo form = QueryInfoDialog.GetPlanCostGoodsDialog();

                if (form.ShowDialog() == DialogResult.OK)
                {
                    txtCode.Text = (string)form.GetDataItem("图号型号");
                    txtCode.Tag = (int)form.GetDataItem("序号");
                    txtName.Text = (string)form.GetDataItem("物品名称");
                    txtSpec.Text = (string)form.GetDataItem("规格");
                    txtUnit.Text = (string)form.GetDataItem("单位");
                    txtMaterialType.Text = (string)form.GetDataItem("物品类别名称");
                    txtMaterialType.Tag = (string)form.GetDataItem("物品类别");
                }
            }
            else
            {
                FormQueryInfo form = QueryInfoDialog.GetStoreGoodsInfoDialog(CE_BillTypeEnum.领料退库单, true, m_strStorage);

                if (form.ShowDialog() == DialogResult.OK)
                {
                    txtCode.Text = (string)form.GetDataItem("图号型号");
                    txtCode.Tag = (int)form.GetDataItem("物品ID");
                    txtName.Text = (string)form.GetDataItem("物品名称");
                    txtSpec.Text = (string)form.GetDataItem("规格");
                    txtProvider.Text = (string)form.GetDataItem("供货单位");
                    txtProviderBatchNo.Text = form.GetDataItem("供方批次号").ToString();

                    if (m_strReturnMode == "返修退库")
                    {
                        txtBatchNo.Text = "系统自动生成";
                    }
                    else
                    {
                        txtBatchNo.Text = (string)form.GetDataItem("批次号");
                    }

                    txtUnit.Text = (string)form.GetDataItem("单位");
                    txtMaterialType.Text = (string)form.GetDataItem("材料类别");
                    txtMaterialType.Tag = (string)form.GetDataItem("材料类别编码");

                    if (m_operateMode == CE_BusinessOperateMode.仓库核实)
                    {
                        txtShelf.Text = (string)form.GetDataItem("货架");
                        txtColumn.Text = (string)form.GetDataItem("列");
                        txtLayer.Text = (string)form.GetDataItem("层");
                    }
                }
            }
        }

        /// <summary>
        /// 从界面获取图号、名称、规格
        /// </summary>
        void GetCodeInfoFromForm()
        {
            m_goodsCode = txtCode.Text;
            m_goodsName = txtName.Text;
            m_spec = txtSpec.Text;
        }

        /// <summary>
        /// 清除窗体上的信息
        /// </summary>
        void ClearControl()
        {
            m_promptBatchNo = false;

            txtCode.Text = "";
            txtCode.Tag = null;
            txtName.Text = "";
            txtSpec.Text = "";
            txtProvider.Text = "";
            txtProviderBatchNo.Text = "";
            txtBatchNo.Text = "";

            numReturnedCount.Value = 0;

            txtUnit.Text = "";
            txtMaterialType.Text = "";
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
                View_S_MaterialListReturnedInTheDepot goods = GetGoodsInfo(dataGridView1.CurrentRow);

                if (goods.返修状态.ToString() == "")
                {
                    cmbProductStatus.SelectedIndex = -1;
                }
                else
                {
                    if ((bool)goods.返修状态)
                    {
                        cmbProductStatus.Text = "已返修";
                    }
                    else
                    {
                        cmbProductStatus.Text = "待返修";
                    }
                }

                txtCode.Text = goods.图号型号;
                txtCode.Tag = goods.物品ID;
                txtName.Text = goods.物品名称;
                txtSpec.Text = goods.规格;
                txtProvider.Text = goods.供应商;
                txtProviderBatchNo.Text = goods.供方批次号;

                if (!GlobalObject.GeneralFunction.IsNullOrEmpty(goods.批次号))
                {
                    txtBatchNo.Text = goods.批次号;
                }

                numReturnedCount.Value = (decimal)goods.退库数;

                txtUnit.Text = goods.单位;
                txtMaterialType.Text = goods.材料类别名称;
                txtMaterialType.Tag = goods.材料类别编码;
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

            //检测是否为售后库房的总成并且要求必须选择产品状态
            if (Convert.ToBoolean(UniversalFunction.GetGoodsAttributeInfo( Convert.ToInt32(txtCode.Tag), CE_GoodsAttributeName.CVT))
                && cmbProductStatus.Text.Trim() == "" && m_strStorage == "05")
            {
                cmbProductStatus.Focus();
                MessageDialog.ShowPromptMessage("请选择产品状态");
                return false;
            }
            
            if (numReturnedCount.Value == 0)
            {
                numReturnedCount.Focus();
                MessageDialog.ShowPromptMessage("领料退库数量必须 > 0");
                return false;
            }

            if (Convert.ToBoolean(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.开启车间管理模块]) && txtBatchNo.Text.Trim().Length == 0 && m_lnqWSCode != null)
            {
                Service_Manufacture_WorkShop.IWorkShopStock serverStock = 
                    Service_Manufacture_WorkShop.ServerModuleFactory.GetServerModule<Service_Manufacture_WorkShop.IWorkShopStock>();

                WS_WorkShopStock tempLnq = serverStock.GetStockSingleInfo(m_lnqWSCode.WSCode, (int)txtCode.Tag, "");

                if (tempLnq == null)
                {
                    btnBatchNo.Focus();
                    MessageDialog.ShowPromptMessage("批次号不能为空");
                    return false;
                }
            }

            if (m_promptBatchNo)
            {
                if (MessageDialog.ShowEnquiryMessage("批次号通常的表示方式为4位年、2位月、6位流水码，您确定您输入的批次号是正确的吗？") == DialogResult.No)
                {
                    txtBatchNo.Focus();
                    return false;
                }
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (txtBatchNo.Text.Trim() == dataGridView1.Rows[i].Cells["批次号"].Value.ToString()
                    && (int)txtCode.Tag == Convert.ToInt32( dataGridView1.Rows[i].Cells["物品ID"].Value)
                    && i != dataGridView1.CurrentRow.Index)
                {
                    MessageDialog.ShowPromptMessage("不可重复添加同批次同一种物品");
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
        void RefreshDataGridView(IEnumerable<View_S_MaterialListReturnedInTheDepot> goods)
        {
            if (goods == null)
            {
                return;
            }

            this.dataGridView1.Visible = false;
            this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                                                     this.dataGridView1_ColumnWidthChanged);
            this.dataGridView1.CellEnter -= new DataGridViewCellEventHandler(this.dataGridView1_CellEnter);

            dataGridView1.DataSource = goods; //GlobalObject.GeneralFunction.ConvertToDataTable<View_S_MaterialListReturnedInTheDepot>(goods);
            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                                                     this.dataGridView1_ColumnWidthChanged);
            this.dataGridView1.CellEnter += new DataGridViewCellEventHandler(this.dataGridView1_CellEnter);

            dataGridView1.Refresh();

            dataGridView1.Columns["序号"].Visible = false;
            dataGridView1.Columns["退库单号"].Visible = false;
            dataGridView1.Columns["材料类别编码"].Visible = false;
            dataGridView1.Columns["物品ID"].Visible = false;

            this.dataGridView1.Visible = true;
            lblAmount.Text = goods.Count().ToString();
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
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

        private void btnNew_Click(object sender, EventArgs e)
        {
            ClearControl();
        }

        /// <summary>
        /// 获得生成的批次号
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回批次号</returns>
        string GetBatchNo(int goodsID)
        {
            string result = "";

            if (m_blIsOnlyForRepair)
            {
                result = "FT" + ServerTime.Time.Year.ToString() + goodsID.ToString("D6");
            }
            else
            {
                result = "ZT" + ServerTime.Time.Year.ToString() + goodsID.ToString("D6");
            }

            return result;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!CheckDataItem())
            {
                return;
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (m_strReturnMode == "返修退库")
                {
                    if ((int)txtCode.Tag == Convert.ToInt32(dataGridView1.Rows[i].Cells["物品ID"].Value))
                    {
                        MessageDialog.ShowPromptMessage("不可重复添加同一种物品");
                        return;
                    }
                }
                else
                {
                    if (txtBatchNo.Text.Trim() == dataGridView1.Rows[i].Cells["批次号"].Value.ToString()
                        && (int)txtCode.Tag == Convert.ToInt32(dataGridView1.Rows[i].Cells["物品ID"].Value))
                    {
                        MessageDialog.ShowPromptMessage("不可重复添加同批次同一种物品");
                        return;
                    }
                }

            }

            S_MaterialListReturnedInTheDepot goods = new S_MaterialListReturnedInTheDepot();

            goods.Bill_ID = m_billNo;
            goods.GoodsID = (int)txtCode.Tag;
            goods.Provider = txtProvider.Text;
            goods.ProviderBatchNo = txtProviderBatchNo.Text;
            goods.BatchNo = txtBatchNo.Text == "系统自动生成" ? GetBatchNo(goods.GoodsID) : txtBatchNo.Text;
            goods.ReturnedAmount = numReturnedCount.Value;
            goods.Remark = txtRemark.Text;
            goods.ShelfArea = "";
            goods.ColumnNumber = "";
            goods.LayerNumber = "";

            //产品状态 设置 2012.3.30 by cjb
            if (cmbProductStatus.Text.Trim() != "")
            {
                if (cmbProductStatus.Text.Trim() == "已返修")
                {
                    goods.RepairStatus = true;
                }
                else
                {
                    goods.RepairStatus = false;
                }
            }

            if (!m_goodsServer.AddGoods(goods, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            GetCodeInfoFromForm();
            m_queryGoodsInfo = m_goodsServer.GetGoods(m_billNo);
            RefreshDataGridView(m_queryGoodsInfo);
            PositioningRecord(m_goodsCode, m_goodsName, m_spec);
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

            S_MaterialListReturnedInTheDepot goods = new S_MaterialListReturnedInTheDepot();
            View_S_MaterialListReturnedInTheDepot viewGoods = GetGoodsInfo(dataGridView1.SelectedRows[0]);

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
            goods.ReturnedAmount = numReturnedCount.Value;
            goods.Remark = txtRemark.Text;

            //产品状态 设置 2012.3.30 by cjb
            if (cmbProductStatus.Text.Trim() != "")
            {
                if (cmbProductStatus.Text.Trim() == "已返修")
                {
                    goods.RepairStatus = true;
                }
                else
                {
                    goods.RepairStatus = false;
                }
            }

            if (m_operateMode == CE_BusinessOperateMode.仓库核实)
            {
                goods.ShelfArea = txtShelf.Text;
                goods.ColumnNumber = txtColumn.Text;
                goods.LayerNumber = txtLayer.Text;
            }
            else
            {
                goods.ColumnNumber = "";
                goods.LayerNumber = "";
                goods.ShelfArea = "";
            }


            if (!m_goodsServer.UpdateGoods(goods, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            GetCodeInfoFromForm();
            m_queryGoodsInfo = m_goodsServer.GetGoods(m_billNo);
            RefreshDataGridView(m_queryGoodsInfo);
            PositioningRecord(m_goodsCode, m_goodsName, m_spec);
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

        /// <summary>
        /// 从行记录中提取物品对象信息
        /// </summary>
        /// <param name="row">行记录</param>
        /// <returns>提取的物品信息</returns>
        private View_S_MaterialListReturnedInTheDepot GetGoodsInfo(DataGridViewRow row)
        {
            if (row == null)
            {
                return null;
            }

            View_S_MaterialListReturnedInTheDepot goods = new View_S_MaterialListReturnedInTheDepot();

            if (row.Cells["返修状态"].Value != null && row.Cells["返修状态"].Value.ToString() != "")
            {
                goods.返修状态 = (bool)row.Cells["返修状态"].Value;
            }

            goods.序号 = (long)row.Cells["序号"].Value;
            goods.退库单号 = (string)row.Cells["退库单号"].Value;
            goods.物品ID = (int)row.Cells["物品ID"].Value;
            goods.图号型号 = (string)row.Cells["图号型号"].Value;
            goods.物品名称 = (string)row.Cells["物品名称"].Value;
            goods.规格 = (string)row.Cells["规格"].Value;
            goods.供应商 = (string)row.Cells["供应商"].Value;
            goods.供方批次号 = (string)row.Cells["供方批次号"].Value;
            goods.批次号 = (string)row.Cells["批次号"].Value;
            goods.退库数 = (decimal)row.Cells["退库数"].Value;
            goods.单位 = (string)row.Cells["单位"].Value;
            goods.材料类别编码 = (string)row.Cells["材料类别编码"].Value;
            goods.材料类别名称 = (string)row.Cells["材料类别名称"].Value;
            goods.货架 = (string)row.Cells["货架"].Value;
            goods.列 = (string)row.Cells["列"].Value;
            goods.层 = (string)row.Cells["层"].Value;
            goods.备注 = (string)row.Cells["备注"].Value;

            return goods;
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                View_S_MaterialListReturnedInTheDepot curGoods = GetGoodsInfo(dataGridView1.Rows[i]);

                if (i != dataGridView1.Rows.Count - 1)
                {
                    View_S_MaterialListReturnedInTheDepot nextGoods = GetGoodsInfo(dataGridView1.Rows[i+1]);

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

        private void btnFindDepot_Click(object sender, EventArgs e)
        {
            if (m_operateMode == CE_BusinessOperateMode.仓库核实)
            {
                FormDepotType form = new FormDepotType();

                if (form.ShowDialog() == DialogResult.OK)
                {
                    txtMaterialType.Text = form.SelectedDepotType.仓库名称;
                    txtMaterialType.Tag = form.SelectedDepotType.仓库编码;
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("只有仓库管理员才可以使用此功能");
            }
        }

        private void txtBatchNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (txtBatchNo.Text.Trim().Length != 12)
            {
                m_promptBatchNo = true;
            }
            else
            {
                m_promptBatchNo = false;
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }


            View_S_MaterialReturnedInTheDepot lnqMaterialReturn = m_goodsServer.GetBillView(m_billNo);
            int intGoodsID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["物品ID"].Value);

            switch (UniversalFunction.GetGoodsType(intGoodsID,m_strStorage))
            {
                case CE_GoodsType.CVT:
                case CE_GoodsType.TCU:
                    BarCodeInfo tempInfo = new BarCodeInfo();

                    tempInfo.BatchNo = dataGridView1.CurrentRow.Cells["批次号"].Value.ToString();
                    tempInfo.Count = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["退库数"].Value);
                    tempInfo.GoodsCode = dataGridView1.CurrentRow.Cells["图号型号"].Value.ToString();
                    tempInfo.GoodsID = intGoodsID;
                    tempInfo.GoodsName = dataGridView1.CurrentRow.Cells["物品名称"].Value.ToString();
                    tempInfo.Remark = dataGridView1.CurrentRow.Cells["备注"].Value.ToString();
                    tempInfo.Spec = dataGridView1.CurrentRow.Cells["规格"].Value.ToString();

                    bool blCheck = true;

                    if (m_operateMode == CE_BusinessOperateMode.查看)
                    {
                        blCheck = false;
                    }
                    else
                    {
                        if (lnqMaterialReturn.单据状态 != "等待仓管退库")
                        {
                            blCheck = false;
                        }
                    }

                    IMaterialReturnedInTheDepot serverBill = ServerModuleFactory.GetServerModule<IMaterialReturnedInTheDepot>();

                    View_S_MaterialReturnedInTheDepot tempLnq = serverBill.GetBillView(m_billNo);

                    CE_BusinessType tempType = CE_BusinessType.库房业务;

                    Service_Manufacture_WorkShop.IWorkShopBasic serverWSBasic =
                        Service_Manufacture_WorkShop.ServerModuleFactory.GetServerModule<Service_Manufacture_WorkShop.IWorkShopBasic>();

                    WS_WorkShopCode tempWSCode = serverWSBasic.GetPersonnelWorkShop(tempLnq.申请人编码);

                    Dictionary<string, string> tempDic = new Dictionary<string, string>();

                    tempDic.Add(m_strStorage, CE_MarketingType.领料退库.ToString());

                    if (tempWSCode != null)
                    {
                        tempType = CE_BusinessType.综合业务;
                        tempDic.Add(tempWSCode.WSCode, CE_SubsidiaryOperationType.领料退库.ToString());
                    }

                    产品编号 formCode = new 产品编号(tempInfo, tempType, m_billNo, blCheck, tempDic);

                    if (m_strStorage == "05")
                    {
                        if (dataGridView1.CurrentRow.Cells["返修状态"].Value == null
                            || dataGridView1.CurrentRow.Cells["返修状态"].Value.ToString() == "")
                        {
                            MessageDialog.ShowPromptMessage("请选择产品的返修状态");
                            return;
                        }
                        else
                        {
                            formCode.BlIsRepaired = (bool)dataGridView1.CurrentRow.Cells["返修状态"].Value;
                        }
                    }

                    formCode.ShowDialog();
                    break;
                case CE_GoodsType.工装:
                    工装编号录入窗体 form = new 工装编号录入窗体(m_billNo, intGoodsID, CE_BusinessBillType.领料退库, lnqMaterialReturn.单据状态 == "等待仓管退库" ? true : false);
                    form.StartPosition = FormStartPosition.CenterScreen;
                    form.ShowDialog();
                    break;
                case CE_GoodsType.量检具:
                    量检具编号录入窗体 formLJY = new 量检具编号录入窗体(m_billNo, intGoodsID, 
                        Convert.ToDecimal(dataGridView1.CurrentRow.Cells["退库数"].Value),  CE_BusinessBillType.领料退库,
                        m_operateMode == CE_BusinessOperateMode.仓库核实 ? true : false);

                    formLJY.ShowDialog();
                    break;
                case CE_GoodsType.零件:
                    break;
                case CE_GoodsType.未知物品:
                    break;
                default:
                    break;
            }
        }

        private void btnBatchCreate_Click(object sender, EventArgs e)
        {
            FormDataTableCheck frm =
                new FormDataTableCheck(m_goodsServer.GetBatchCreatList("领料", ServerTime.Time.AddDays(-20), ServerTime.Time));
            frm.OnFormDataTableCheckFind += new GlobalObject.DelegateCollection.FormDataTableCheckFindDelegate(frm_OnFormDataTableCheckFind1);
            frm._BlDateTimeControlShow = true;

            if (frm.ShowDialog() == DialogResult.OK)
            {
                string strBillIDGather = "";

                foreach (DataRow dr in frm._DtResult.Rows)
                {
                    strBillIDGather += "'" + dr["领料单号"].ToString() + "',";
	            }

                strBillIDGather = "(" + strBillIDGather.Remove(strBillIDGather.Length - 1) + ")";

                if (!m_goodsServer.BatchCreateList("领料", m_billNo, strBillIDGather, out m_error))
                {
                    MessageDialog.ShowPromptMessage(m_error);
                }
                else
                {
                    MessageDialog.ShowPromptMessage("生成成功");
                    this.Close();
                }

                m_queryGoodsInfo = m_goodsServer.GetGoods(m_billNo);
                RefreshDataGridView(m_queryGoodsInfo);
            }
        }

        DataTable frm_OnFormDataTableCheckFind1(DateTime startTime, DateTime endTime)
        {
            return m_goodsServer.GetBatchCreatList("领料", startTime, endTime);
        }

        private void btnBatchNo_Click(object sender, EventArgs e)
        {
            if (m_lnqWSCode == null)
            {
                return;
            }

            FormQueryInfo form = QueryInfoDialog.GetWorkShopBatchNoInfo(Convert.ToInt32(txtCode.Tag), m_lnqWSCode.WSCode);

            if (form.ShowDialog() == DialogResult.OK)
            {
                txtBatchNo.Text = (string)form.GetDataItem("批次号");
                txtBatchNo.Tag = form.GetDataItem("库存数量");

                StoreQueryCondition condition = new StoreQueryCondition();

                condition.GoodsID = Convert.ToInt32(txtCode.Tag);
                condition.BatchNo = txtBatchNo.Text;
                condition.StorageID = m_strStorage;

                S_Stock tempStock = m_serverStock.GetStockInfo(condition);

                if (tempStock != null)
                {
                    txtProvider.Text = tempStock.Provider;
                    txtProviderBatchNo.Text = tempStock.ProviderBatchNo;
                }
                else
                {
                    txtProvider.Text = "";
                }
            }
        }

        private void btnMaterialsTransfer_Click(object sender, EventArgs e)
        {
            if (m_lnqWSCode == null)
            {
                return;
            }

            FormDataTableCheck frm =
                new FormDataTableCheck(m_serverMaterials.GetMaterialsTransferInfo(m_lnqWSCode.WSCode));
            frm.OnFormDataTableCheckFind += new GlobalObject.DelegateCollection.FormDataTableCheckFindDelegate(frm_OnFormDataTableCheckFind);
            frm._BlDateTimeControlShow = true;

            if (frm.ShowDialog() == DialogResult.OK)
            {
                List<string> listRequisition = DataSetHelper.ColumnsToList_Distinct(frm._DtResult, "单据号");

                DataTable tempTable =
                    m_serverMaterials.SumMaterialsTransferGoods(listRequisition,
                    (int)CE_SubsidiaryOperationType.物料转换后,(int)CE_SubsidiaryOperationType.领料退库,
                    m_lnqWSCode.WSCode);

                foreach (DataRow dr in tempTable.Rows)
                {
                    S_MaterialListReturnedInTheDepot goods = new S_MaterialListReturnedInTheDepot();

                    goods.Bill_ID = m_billNo;
                    goods.GoodsID = Convert.ToInt32(dr["物品ID"]);

                    StoreQueryCondition condition = new StoreQueryCondition();

                    condition.GoodsID = Convert.ToInt32(txtCode.Tag);
                    condition.BatchNo = dr["批次号"].ToString();
                    condition.StorageID = m_strStorage;

                    S_Stock tempStock = m_serverStock.GetStockInfo(condition);

                    if (tempStock != null)
                    {
                        goods.Provider = tempStock.Provider;
                        goods.ProviderBatchNo = tempStock.ProviderBatchNo;
                    }
                    else
                    {
                        goods.Provider = "";
                        goods.ProviderBatchNo = "";
                    }

                    goods.BatchNo = dr["批次号"].ToString();
                    goods.ReturnedAmount = Convert.ToDecimal( dr["数量"]);
                    goods.Remark = txtRemark.Text;
                    goods.ShelfArea = "";
                    goods.ColumnNumber = "";
                    goods.LayerNumber = "";

                    //产品状态 设置 2012.3.30 by cjb
                    if (cmbProductStatus.Text.Trim() != "")
                    {
                        if (cmbProductStatus.Text.Trim() == "已返修")
                        {
                            goods.RepairStatus = true;
                        }
                        else
                        {
                            goods.RepairStatus = false;
                        }
                    }

                    if (!m_goodsServer.AddGoods(goods, out m_error))
                    {
                        MessageDialog.ShowErrorMessage(m_error);
                        return;
                    }
                }

                m_queryGoodsInfo = m_goodsServer.GetGoods(m_billNo);
                RefreshDataGridView(m_queryGoodsInfo);
            }
        }

        DataTable frm_OnFormDataTableCheckFind(DateTime startTime, DateTime endTime)
        {
            return m_serverMaterials.GetMaterialsTransferInfo(m_lnqWSCode.WSCode, startTime, endTime);
        }

        private void btnProvider_Click(object sender, EventArgs e)
        {
            FormQueryInfo formProvider = QueryInfoDialog.GetProviderInfoDialog();

            if (formProvider.ShowDialog() == DialogResult.OK)
            {
                txtProvider.Text = formProvider.GetStringDataItem("供应商编码");
            }
        }

        private void btnInputExcel_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtTemp = ExcelHelperP.RenderFromExcel(openFileDialog1);

                if (dtTemp == null)
                {
                    return;
                }

                if (!dtTemp.Columns.Contains("图号型号"))
                {
                    MessageDialog.ShowPromptMessage("文件无【图号型号】列名");
                    return;
                }

                if (!dtTemp.Columns.Contains("物品名称"))
                {
                    MessageDialog.ShowPromptMessage("文件无【物品名称】列名");
                    return;
                }

                if (!dtTemp.Columns.Contains("规格"))
                {
                    MessageDialog.ShowPromptMessage("文件无【规格】列名");
                    return;
                }

                if (!dtTemp.Columns.Contains("批次号"))
                {
                    MessageDialog.ShowPromptMessage("文件无【批次号】列名");
                    return;
                }

                if (!dtTemp.Columns.Contains("数量"))
                {
                    MessageDialog.ShowPromptMessage("文件无【数量】列名");
                    return;
                }

                m_goodsServer.InsertInfoExcel(m_billNo, dtTemp);
                MessageDialog.ShowPromptMessage("导入成功");
                m_queryGoodsInfo = m_goodsServer.GetGoods(m_billNo);
                RefreshDataGridView(m_queryGoodsInfo);

            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
                return;
            }
        }
    }
}
