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
using PlatformManagement;
using GlobalObject;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 普通入库物品详单
    /// </summary>
    public partial class 普通入库物品清单 : Form
    {
        #region 成员变量

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_error;

        /// <summary>
        /// 操作模式
        /// </summary>
        CE_BusinessOperateMode m_operateMode;

        /// <summary>
        /// 普通入库单号
        /// </summary>
        string m_billNo;

        /// <summary>
        /// 保质期服务组件
        /// </summary>
        IGoodsShelfLife m_serverGoodsShelfLife = ServerModuleFactory.GetServerModule<IGoodsShelfLife>();

        /// <summary>
        /// 普通入库单主单服务
        /// </summary>
        IOrdinaryInDepotBillServer m_serverBill = ServerModuleFactory.GetServerModule<IOrdinaryInDepotBillServer>();

        /// <summary>
        /// 普通入库单物品清单服务
        /// </summary>
        IOrdinaryInDepotGoodsBill m_goodsServer = ServerModuleFactory.GetServerModule<IOrdinaryInDepotGoodsBill>();

        /// <summary>
        /// 查询到的物品信息集
        /// </summary>
        IQueryable<View_S_OrdinaryInDepotGoodsBill> m_queryResult;

        /// <summary>
        /// 基础物品服务组件
        /// </summary>
        IBasicGoodsServer m_basicGoodsServer = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();

        /// <summary>
        /// 单位服务
        /// </summary>
        IUnitServer m_unitServer = ServerModuleFactory.GetServerModule<IUnitServer>();

        /// <summary>
        /// 单位服务
        /// </summary>
        IStoreServer m_serverStore = ServerModuleFactory.GetServerModule<IStoreServer>();

        /// <summary>
        /// 数据定位控件
        /// </summary>
        UserControlDataLocalizer m_dataLocalizer;

        /// <summary>
        /// 单据信息
        /// </summary>
        S_OrdinaryInDepotBill m_billInfo;

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="operateMode">操作模式</param>
        /// <param name="billNo">普通入库单号</param>
        public 普通入库物品清单(CE_BusinessOperateMode operateMode, string billNo)
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();

            m_operateMode = operateMode;
            m_billNo = billNo;

            m_billInfo = ServerModuleFactory.GetServerModule<IOrdinaryInDepotBillServer>().GetBill(billNo);

            if (m_operateMode == CE_BusinessOperateMode.仓库核实)
            {
                btnAdd.Enabled = false;
                btnDelete.Enabled = false;
                btnDeleteAll.Enabled = false;
            }
            else
            {
                txtShelf.ReadOnly = true;
                txtColumn.ReadOnly = true;
                txtLayer.ReadOnly = true;

                if (m_operateMode == CE_BusinessOperateMode.查看)
                {
                    toolStrip1.Visible = false;
                }
            }

            m_queryResult = m_goodsServer.GetGoodsViewInfo(m_billNo);

            RefreshDataGridView(m_queryResult);

            StapleInfo.InitUnitComboBox(cmbUnit);

            // 添加数据定位控件
            m_dataLocalizer = new UserControlDataLocalizer(dataGridView1, this.Name, 
                UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));

            panelTop.Controls.Add(m_dataLocalizer);
            
            m_dataLocalizer.Dock = DockStyle.Bottom;

            if (BasicInfo.ListRoles.Contains(CE_RoleEnum.采购账务管理员.ToString())
                || BasicInfo.ListRoles.Contains(CE_RoleEnum.会计.ToString()))
            {
                label19.Visible = true;
                label20.Visible = true;
                numUnitPrice.Visible = true;
                numPrice.Visible = true;
            }
            else
            {
                label19.Visible = false;
                label20.Visible = false;
                numUnitPrice.Visible = false;
                numPrice.Visible = false;
            }
        }

        private void 普通入库物品清单_Resize(object sender, EventArgs e)
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
            try
            {
                ClearControl();

                FormQueryInfo form = QueryInfoDialog.GetOrderFormGoodsDialog(m_billInfo.OrderBill_ID, true);
                IOrderFormInfoServer serviceOrderForm = ServerModuleFactory.GetServerModule<IOrderFormInfoServer>();
                View_B_OrderFormInfo orderInfo = serviceOrderForm.GetOrderFormInfo(m_billInfo.OrderBill_ID);
                if (form != null && form.ShowDialog() == DialogResult.OK)
                {
                    txtCode.Text = (string)form.GetDataItem("图号型号");
                    txtName.Text = (string)form.GetDataItem("物品名称");
                    txtSpec.Text = (string)form.GetDataItem("规格");

                    IBargainGoodsServer serviceGoodsInfo = ServerModuleFactory.GetServerModule<IBargainGoodsServer>();
                    numUnitPrice.Value = serviceGoodsInfo.GetGoodsUnitPrice(m_billInfo.OrderBill_ID, Convert.ToInt32(form.GetDataItem("物品ID")), orderInfo.供货单位);

                    numGoodsAmount.Value = (decimal)form.GetDataItem("订货数量");

                    View_F_GoodsPlanCost planCost = GetBasicGoodsInfo(txtCode.Text, txtName.Text, txtSpec.Text, numUnitPrice.Value);

                    if (planCost != null)
                    {
                        cmbUnit.Text = planCost.单位;
                        numPlanUnitPrice.Value = planCost.单价;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
                return;
            }

        }

        /// <summary>
        /// 清除窗体上的信息
        /// </summary>
        void ClearControl()
        {
            lnklbSingleBill.Text = "";
            txtCode.Text = "";
            txtName.Text = "";
            txtSpec.Text = "";

            txtProviderBatchNo.Text = "";

            numGoodsAmount.Value = 0;

            cmbUnit.SelectedIndex = -1;
            txtShelf.Text = "";
            txtColumn.Text = "";
            txtLayer.Text = "";

            txtRemark.Text = "";

            txtCode.Enabled = true;
            txtName.Enabled = true;
            txtSpec.Enabled = true;

            numUnitPrice.Value = 0;
            numUnitPrice.Enabled = true;

            numPlanUnitPrice.Value = 0;
            numPrice.Value = 0;

            txtTotalPrice.Text = "";
        }

        /// <summary>
        /// 刷新控件
        /// </summary>
        void RefreshControl()
        {
            ClearControl();

            if (dataGridView1.CurrentRow != null)
            {
                View_S_OrdinaryInDepotGoodsBill goods = GetGoodsInfo(dataGridView1.CurrentRow);

                txtCode.Text = goods.图号型号;
                txtName.Text = goods.物品名称;
                txtSpec.Text = goods.规格;
                txtProviderBatchNo.Text = goods.供方批次号;
                txtProviderBatchNo.Tag = goods.批次号;

                numGoodsAmount.Value = (decimal)goods.数量;

                lnklbSingleBill.Text = goods.检验单号;
                numUnitPrice.Value = goods.暂估单价;
                numPrice.Value = goods.暂估金额;
                numPlanUnitPrice.Value = (decimal)goods.计划单价;
                txtTotalPrice.Text = goods.大写金额;

                cmbUnit.Text = goods.单位;
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
                MessageDialog.ShowPromptMessage("请选择或录入物品信息");
                return false;
            }

            if (cmbUnit.SelectedIndex < 0)
            {
                cmbUnit.Focus();
                MessageDialog.ShowPromptMessage("请选择或录入单位信息");
                return false;
            }

            if (numGoodsAmount.Value == 0)
            {
                numGoodsAmount.Focus();
                MessageDialog.ShowPromptMessage("入库数量必须 > 0");
                return false;
            }

            //if (numUnitPrice.Value == 0)
            //{
            //    numUnitPrice.Focus();
            //    MessageDialog.ShowPromptMessage("物品单价必须 > 0");
            //    return false;
            //}

            return true;
        }

        #region 刷新数据

        /// <summary>
        /// 刷新DataGridView
        /// </summary>
        /// <param name="goods">物品信息</param>
        void RefreshDataGridView(IEnumerable<View_S_OrdinaryInDepotGoodsBill> goods)
        {
            ClearControl();

            if (goods == null)
            {
                return;
            }

            this.dataGridView1.Visible = false;
            this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridView1_ColumnWidthChanged);
            this.dataGridView1.CellEnter -= new DataGridViewCellEventHandler(this.dataGridView1_CellEnter);

            dataGridView1.DataSource = goods;

            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridView1_ColumnWidthChanged);
            this.dataGridView1.CellEnter += new DataGridViewCellEventHandler(this.dataGridView1_CellEnter);

            dataGridView1.Refresh();

            this.dataGridView1.Visible = true;

            dataGridView1.Columns["序号"].Visible = false;
            dataGridView1.Columns["入库单号"].Visible = false;
            dataGridView1.Columns["发票单价"].Visible = false;
            dataGridView1.Columns["发票金额"].Visible = false;
            dataGridView1.Columns["计划单价"].Visible = false;
            dataGridView1.Columns["大写金额"].Visible = false;


            if (BasicInfo.ListRoles.Contains(CE_RoleEnum.采购账务管理员.ToString())
                || BasicInfo.ListRoles.Contains(CE_RoleEnum.会计.ToString()))
            {

                dataGridView1.Columns["暂估单价"].Visible = true;
                dataGridView1.Columns["暂估金额"].Visible = true;
            }
            else
            {
                dataGridView1.Columns["暂估单价"].Visible = false;
                dataGridView1.Columns["暂估金额"].Visible = false;
            }
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!CheckDataItem())
            {
                return;
            }

            S_OrdinaryInDepotBill lnqBill = m_serverBill.GetBill(m_billNo);
            S_OrdinaryInDepotGoodsBill goods = new S_OrdinaryInDepotGoodsBill();
            View_F_GoodsPlanCost planCost = GetBasicGoodsInfo(txtCode.Text, txtName.Text, txtSpec.Text, numUnitPrice.Value);

            if (planCost == null)
            {
                return;
            }

            goods.Bill_ID = m_billNo;
            goods.ProviderBatchNo = txtProviderBatchNo.Text;

            if (m_serverGoodsShelfLife.IsShelfLife(planCost.序号))
            {
                goods.BatchNo = m_goodsServer.GetNewBatchNo();
            }
            else
            {
                goods.BatchNo = "";
            }

            goods.Amount = numGoodsAmount.Value;
            goods.UnitPrice = numUnitPrice.Value;
            goods.Price = decimal.Round(goods.UnitPrice * numGoodsAmount.Value, 2);
            goods.AmountInWords = CalculateClass.GetTotalPrice(goods.Price);
            goods.ShelfArea = "";
            goods.ColumnNumber = "";
            goods.LayerNumber = "";
            goods.Remark = txtRemark.Text;
            goods.TestingSingle = lnklbSingleBill.Text;

            goods.GoodsID = planCost.序号;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["图号型号"].Value.ToString() == txtCode.Text 
                    && row.Cells["物品名称"].Value.ToString() == txtName.Text &&
                    row.Cells["规格"].Value.ToString() == txtSpec.Text 
                    && row.Cells["供方批次号"].Value.ToString() == txtProviderBatchNo.Text)
                {
                    MessageDialog.ShowPromptMessage("已经存在相同的物品信息列不允许再进行重复添加！");
                    return;
                }
            }

            if (!m_goodsServer.AddGoods(m_billNo, goods, out m_queryResult, out m_error))
            {

                if (m_error.Contains("不能在具有唯一索引"))
                {
                    m_error = "此物品清单中的物品（同种物品、同一批号）可能已经在其他普通入库单中增加，不允许再重复入库！";
                }

                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            RefreshDataGridView(m_queryResult);
            PositioningRecord(planCost.图号型号, planCost.物品名称, planCost.规格);
        }

        /// <summary>
        /// 获取基础物品信息
        /// </summary>
        /// <param name="code">图号型号</param>
        /// <param name="name">物品名称</param>
        /// <param name="spec">规格</param>
        /// <param name="unitPrice">单价</param>
        /// <returns>成功返回获取到的信息, 失败返回null</returns>
        private View_F_GoodsPlanCost GetBasicGoodsInfo(string code, string name, string spec, decimal unitPrice)
        {
            //如果指定图号、名称、规则的物品不存在时是否自动向基础物品表中增加明录
            //bool autoAddPlanInfo = true;
            View_F_GoodsPlanCost planCost = m_basicGoodsServer.GetGoodsInfo(code, name, spec, out m_error);

            if (planCost == null)
            {
                MessageDialog.ShowErrorMessage("基础物品表中不存在要增加的物品信息，请及时与仓管员联系！");
                return planCost;
            }
            else
            {
                if (planCost.单价 != unitPrice || (cmbUnit.Text != "" && planCost.单位 != cmbUnit.Text))
                {
                    return UpdatePlanPrice(planCost, unitPrice);
                }
            }

            return planCost;
        }

        /// <summary>
        /// 更新基础物品
        /// </summary>
        /// <param name="planCost">要修改的记录</param>
        /// <param name="newPrice">新基础物品</param>
        /// <returns>返回更改后的记录</returns>
        private View_F_GoodsPlanCost UpdatePlanPrice(View_F_GoodsPlanCost planCost, decimal newPrice)
        {
            if (!BasicInfo.IsFuzzyContainsRoleName("库管理员") 
                && (planCost.录入员编码 != BasicInfo.LoginID || planCost.日期 != ServerTime.Time.Date))
            {
                return planCost;
            }

            planCost.单价 = newPrice;

            if (cmbUnit.SelectedIndex < 0)
            {
                cmbUnit.SelectedValue = planCost.单位ID;
            }

            //if (!m_basicGoodsServer.UpdateGoodsPrice(planCost.序号, newPrice, (int)cmbUnit.SelectedValue, BasicInfo.LoginID, out m_error))
            //{
            //    MessageDialog.ShowErrorMessage(m_error);
            //}

            return planCost;
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

            View_F_GoodsPlanCost planCost = GetBasicGoodsInfo(txtCode.Text, txtName.Text, txtSpec.Text, numUnitPrice.Value);

            if (planCost == null)
            {
                return;
            }

            S_OrdinaryInDepotGoodsBill goods = new S_OrdinaryInDepotGoodsBill();
            View_S_OrdinaryInDepotGoodsBill viewGoods = GetGoodsInfo(dataGridView1.SelectedRows[0]);

            goods.ID = viewGoods.序号;
            goods.GoodsID = planCost.序号;
            goods.ProviderBatchNo = txtProviderBatchNo.Text;
            goods.BatchNo = txtProviderBatchNo.Tag.ToString();
            goods.Amount = numGoodsAmount.Value;
            goods.UnitPrice = numUnitPrice.Value;
            goods.Price = Math.Round(numUnitPrice.Value * numGoodsAmount.Value, 2);
            goods.AmountInWords = CalculateClass.GetTotalPrice(goods.Price);
            goods.TestingSingle = lnklbSingleBill.Text;

            if (m_operateMode == CE_BusinessOperateMode.仓库核实)
            {
                goods.ShelfArea = txtShelf.Text;
                goods.ColumnNumber = txtColumn.Text;
                goods.LayerNumber = txtLayer.Text;
            }

            goods.Remark = txtRemark.Text;

            if (!m_goodsServer.UpdateGoods(goods, out m_queryResult, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            int rowIndex = dataGridView1.SelectedRows[0].Index;

            RefreshDataGridView(m_queryResult);
            
            PositioningRecord(rowIndex);
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            RefreshControl();

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
            List<int> lstId = new List<int>(dataGridView1.SelectedRows.Count);

            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                View_F_GoodsPlanCost planCost = GetBasicGoodsInfo(
                    row.Cells["图号型号"].Value.ToString(), row.Cells["物品名称"].Value.ToString(),
                    row.Cells["规格"].Value.ToString(), 0);

                lstId.Add((int)row.Cells["序号"].Value);
            }

            if (!m_goodsServer.DeleteGoods(lstId , out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            m_queryResult = m_goodsServer.GetGoodsViewInfo(m_billNo);

            RefreshDataGridView(m_queryResult);
            
            PositioningRecord(rowIndex);
        }

        /// <summary>
        /// 从行记录中提取物品对象信息
        /// </summary>
        /// <param name="row">行记录</param>
        /// <returns>提取的物品信息</returns>
        private View_S_OrdinaryInDepotGoodsBill GetGoodsInfo(DataGridViewRow row)
        {
            if (row == null)
            {
                return null;
            }

            IQueryable<View_S_OrdinaryInDepotGoodsBill> data = dataGridView1.DataSource as IQueryable<View_S_OrdinaryInDepotGoodsBill>;

            return (from r in data
                   where r.图号型号 == row.Cells["图号型号"].Value.ToString() 
                   && r.物品名称 == row.Cells["物品名称"].Value.ToString() && r.规格 == row.Cells["规格"].Value.ToString() 
                   && r.供方批次号 == row.Cells["供方批次号"].Value.ToString()
                   select r).Single();
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                View_S_OrdinaryInDepotGoodsBill curGoods = GetGoodsInfo(dataGridView1.Rows[i]);

                if (i != dataGridView1.Rows.Count - 1)
                {
                    View_S_OrdinaryInDepotGoodsBill nextGoods = GetGoodsInfo(dataGridView1.Rows[i+1]);

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
                if (!m_goodsServer.DeleteGoods(m_billNo, out m_queryResult, out m_error))
                {
                    MessageDialog.ShowErrorMessage(m_error);
                    return;
                }

                RefreshDataGridView(m_queryResult);
            }
        }

        private void txtCode_TextChanged(object sender, EventArgs e)
        {
            if (txtCode.Text.Trim().Length == 0)
            {
                txtName.ReadOnly = false;
                txtSpec.ReadOnly = false;
                cmbUnit.Enabled = false;
            }
            else
            {
                txtName.ReadOnly = true;
                txtSpec.ReadOnly = true;
                cmbUnit.Enabled = true;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            m_queryResult = m_goodsServer.GetGoodsViewInfo(m_billNo);

            RefreshDataGridView(m_queryResult);
        }

        private void btnAutoGenerate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                MessageDialog.ShowPromptMessage("请删除此清单中的所有物品后再进行此操作！");
                return;
            }

            IOrderFormGoodsServer orderFormGoodsServer = ServerModuleFactory.GetServerModule<IOrderFormGoodsServer>();
            IQueryable<View_B_OrderFormGoods> goodsGroup = null;

            if (!orderFormGoodsServer.GetOrderFormGoods(
                BasicInfo.ListRoles, BasicInfo.LoginID, m_billInfo.OrderBill_ID, out goodsGroup, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            S_OrdinaryInDepotBill lnqBill = m_serverBill.GetBill(m_billNo);
            IOrderFormInfoServer serviceOrderForm = ServerModuleFactory.GetServerModule<IOrderFormInfoServer>();
            View_B_OrderFormInfo orderInfo = serviceOrderForm.GetOrderFormInfo(m_billInfo.OrderBill_ID);

            foreach (var item in goodsGroup)
            {
                if (item.订货数量 == 0)
                {
                    continue;
                }

                S_OrdinaryInDepotGoodsBill goods = new S_OrdinaryInDepotGoodsBill();
                View_F_GoodsPlanCost planCost = GetBasicGoodsInfo(item.图号型号, item.物品名称, item.规格, 0);

                if (planCost == null)
                {
                    return;
                }

                goods.GoodsID = planCost.序号;
                goods.Bill_ID = m_billNo;
                goods.ProviderBatchNo = "";

                if (m_serverGoodsShelfLife.IsShelfLife(planCost.序号))
                {
                    goods.BatchNo = m_goodsServer.GetNewBatchNo();
                }
                else
                {
                    goods.BatchNo = "";
                }

                goods.Amount = item.订货数量;

                IBargainGoodsServer serviceBargainGoods = ServerModuleFactory.GetServerModule<IBargainGoodsServer>();

                goods.UnitPrice = serviceBargainGoods.GetGoodsUnitPrice(orderInfo.订单号, goods.GoodsID, orderInfo.供货单位);
                goods.Price = decimal.Round(goods.UnitPrice * item.订货数量,(int)2);
                goods.AmountInWords = CalculateClass.GetTotalPrice(goods.Price);
                goods.Remark = txtRemark.Text;

                if (!m_goodsServer.AddGoods(m_billNo, goods, out m_queryResult, out m_error))
                {
                    MessageDialog.ShowErrorMessage(m_error);
                    return;
                }
            }

            btnRefresh_Click(sender, e);
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            IGaugeManage serviceGauge = ServerModuleFactory.GetServerModule<IGaugeManage>();

            int intGoodsID = m_basicGoodsServer.GetGoodsID(dataGridView1.CurrentRow.Cells["图号型号"].Value.ToString(),
                dataGridView1.CurrentRow.Cells["物品名称"].Value.ToString(),
                dataGridView1.CurrentRow.Cells["规格"].Value.ToString());

            if (UniversalFunction.GetGoodsType(intGoodsID, m_billInfo.StorageID) == CE_GoodsType.量检具
                && m_operateMode == CE_BusinessOperateMode.仓库核实)
            {
                量检具编号录入窗体 form = new 量检具编号录入窗体(m_billNo, intGoodsID, Convert.ToDecimal(dataGridView1.CurrentRow.Cells["数量"].Value), CE_BusinessBillType.入库, true);
                form.ShowDialog();
            }
        }

        private void btnFindTesting_Click(object sender, EventArgs e)
        {
            FormQueryInfo form = QueryInfoDialog.GetUnProductTestingSingleDialog();

            if (form != null && form.ShowDialog() == DialogResult.OK)
            {
                lnklbSingleBill.Text = form.GetDataItem("单据号").ToString();
            }
        }

        private void lnklbSingleBill_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (lnklbSingleBill.Text == "" || lnklbSingleBill.Text == "检验单号")
            {
                return;
            }

            非产品件检验单明细 frm = new 非产品件检验单明细(lnklbSingleBill.Text);
            frm.BlSaveFlag = true;
            frm.ShowDialog();
        }
    }
}
