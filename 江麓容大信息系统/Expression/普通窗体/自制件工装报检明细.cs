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
using CommonBusinessModule;

namespace Expression
{
    /// <summary>
    /// 自制件工装报检明细界面
    /// </summary>
    public partial class 自制件工装报检明细 : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr;

        /// <summary>
        /// 操作模式
        /// </summary>
        CE_BusinessOperateMode m_operateMode;

        /// <summary>
        /// 自制件工装入库单号
        /// </summary>
        string m_billNo;

        /// <summary>
        /// 自制件工装报检物品清单服务
        /// </summary>
        IFrockIndepotBill m_goodsServer = ServerModuleFactory.GetServerModule<IFrockIndepotBill>();

        /// <summary>
        /// 查询到的物品信息集
        /// </summary>
        IQueryable<View_S_FrockInDepotGoodsBill> m_queryResultGoods;

        /// <summary>
        /// 计划成本服务组件
        /// </summary>
        IBasicGoodsServer m_planCostServer = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();

        /// <summary>
        /// 单位服务
        /// </summary>
        IUnitServer m_unitServer = ServerModuleFactory.GetServerModule<IUnitServer>();

        /// <summary>
        /// 数据定位控件
        /// </summary>
        UserControlDataLocalizer m_dataLocalizer;

        /// <summary>
        /// 单据信息
        /// </summary>
        S_FrockInDepotBill m_billInfo;

        /// <summary>
        /// 查询结果
        /// </summary>
        IQueryResult m_queryResult;

        /// <summary>
        /// 是否生成工装台账
        /// </summary>
        private bool m_blIsCreateFrockStandingBook = false;

        public bool BlIsCreateFrockStandingBook
        {
            get { return m_blIsCreateFrockStandingBook; }
            set { m_blIsCreateFrockStandingBook = value; }
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

        public 自制件工装报检明细(CE_BusinessOperateMode operateMode, string billNo)
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();

            m_operateMode = operateMode;
            m_billNo = billNo;
            txtBill_ID.Text = m_billNo;

            S_FrockInDepotBill tempBill = m_goodsServer.GetBill(billNo);

            m_lnqWSCode = tempBill == null ?
                m_serverWSBasic.GetPersonnelWorkShop(BasicInfo.LoginID) :
                m_serverWSBasic.GetPersonnelWorkShop(tempBill.JJRYID);

            if (m_operateMode == CE_BusinessOperateMode.仓库核实)
            {
                新建toolStripButton1.Visible = false;
                btnAdd.Enabled = false;
                btnDelete.Enabled = false;
                btnDeleteAll.Enabled = false;
                btnFindCode.Enabled = false;
                numGoodsAmount.Enabled = false;
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
                else if (m_operateMode == CE_BusinessOperateMode.新建)
                {
                    dateTime_BillTime.Value = ServerModule.ServerTime.Time;
                    txtJJRY.Text = BasicInfo.LoginName;
                    toolStripSeparator2.Visible = false;
                    toolStripSeparator3.Visible = false;
                    toolStripSeparator7.Visible = false;
                    toolStripSeparatorDelete.Visible = false;
                    toolStripSeparator1.Visible = false;
                    groupBox1.Enabled = false;
                    btnAdd.Visible = false;
                    btnDelete.Visible = false;
                    btnDeleteAll.Visible = false;
                    btnUpdate.Visible = false;
                }
                else
                {
                    新建toolStripButton1.Visible = false;
                }
            }

            DataTable dt = UniversalFunction.GetStorageTb();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cmbStorage.Items.Add(dt.Rows[i]["StorageName"].ToString());
            }

            cmbStorage.SelectedIndex = -1;

            StapleInfo.InitUnitComboBox(cmbUnit);

            // 添加数据定位控件
            m_dataLocalizer = new UserControlDataLocalizer(dataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
            groupBox1.Controls.Add(m_dataLocalizer);
            m_dataLocalizer.Dock = DockStyle.Bottom;


            m_billInfo = m_goodsServer.GetBill(billNo);

            if (m_billInfo != null)
            {
                txtJJRY.Text = UniversalFunction.GetPersonnelName(m_billInfo.JJRYID);
                dateTime_BillTime.Value = m_billInfo.Bill_Time;
                cmbStorage.Text = UniversalFunction.GetStorageName(m_billInfo.StorageID);
                txtProposer.Text = UniversalFunction.GetPersonnelName(m_billInfo.ProposerID);
                txtDesigner.Text = UniversalFunction.GetPersonnelName(m_billInfo.DesignerID);
                txtDepotManager.Text = UniversalFunction.GetPersonnelName(m_billInfo.DepotManager);
                txtRemark.Text = m_billInfo.Remark;

                m_queryResultGoods = m_goodsServer.GetGoodsInfo(billNo);

                if (m_queryResultGoods != null)
                {
                    RefreshDataGridView(m_queryResultGoods);
                }
            }

            if (!Convert.ToBoolean(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.开启车间管理模块]))
            {
                btnBatchNo.Visible = false;
            }
        }

        private void 自制件工装报检明细_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        /// <summary>
        /// 清除窗体上的信息
        /// </summary>
        void ClearControl()
        {
            txtCode.Text = "";
            txtName.Text = "";
            txtSpec.Text = "";

            numGoodsAmount.Value = 0;

            cmbUnit.SelectedIndex = -1;
            txtShelf.Text = "";
            txtColumn.Text = "";
            txtLayer.Text = "";

            txtRemark.Text = "";

            txtCode.Enabled = true;
            txtName.Enabled = true;
            txtSpec.Enabled = true;
        }

        private void btnFindCode_Click(object sender, EventArgs e)
        {
            ClearControl();

            FormQueryInfo form = QueryInfoDialog.GetPlanCostGoodsDialog();

            if (form != null && form.ShowDialog() == DialogResult.OK)
            {
                txtCode.Text = (string)form.GetDataItem("图号型号");
                txtName.Text = (string)form.GetDataItem("物品名称");
                txtSpec.Text = (string)form.GetDataItem("规格");
                txtCode.Tag = (int)form.GetDataItem("序号");

                if (txtSpec.Text.Contains("自制件"))
                {
                    View_F_GoodsPlanCost planCost = m_planCostServer.GetGoodsInfo(txtCode.Text, txtName.Text, txtSpec.Text, out m_strErr);

                    if (planCost != null)
                    {
                        cmbUnit.Text = planCost.单位;
                    }
                }
                else
                {
                    txtCode.Text = "";
                    txtName.Text = "";
                    txtSpec.Text = "";

                    MessageDialog.ShowPromptMessage("您选择的物品不是属于自制件，【需在物品规格中填写“自制件”字样】，请重新选择或修改物品基础信息的规格信息！");
                    return;
                }
            }
        }

        /// <summary>
        /// 删除计划价格
        /// </summary>
        /// <param name="planCost">要删除的记录</param>
        private void DeletePlanPrice(View_F_GoodsPlanCost planCost)
        {
            if (planCost.录入员编码 == BasicInfo.LoginID && planCost.日期 == ServerTime.Time.Date)
            {
                if (!m_planCostServer.DeleteGoods(planCost.序号, out m_strErr))
                {
                    MessageDialog.ShowErrorMessage(m_strErr);
                }
            }
        }

        /// <summary>
        /// 更新计划价格
        /// </summary>
        /// <param name="planCost">要修改的记录</param>
        /// <param name="newPrice">新计划价格</param>
        /// <returns>返回更改后的记录</returns>
        private View_F_GoodsPlanCost UpdatePlanPrice(View_F_GoodsPlanCost planCost, decimal newPrice)
        {
            if (!BasicInfo.IsFuzzyContainsRoleName("库管理员") 
                && (planCost.录入员编码 != BasicInfo.LoginID 
                || planCost.日期 != ServerTime.Time.Date))
            {
                return planCost;
            }

            planCost.单价 = newPrice;

            if (cmbUnit.SelectedIndex < 0)
            {
                cmbUnit.SelectedValue = planCost.单位ID;
            }

            //if (!m_planCostServer.UpdateGoodsPrice(planCost.序号, newPrice, (int)cmbUnit.SelectedValue, BasicInfo.LoginID, out m_strErr))
            //{
            //    MessageDialog.ShowErrorMessage(m_strErr);
            //}

            return planCost;
        }

        /// <summary>
        /// 根据权限控制窗体
        /// </summary>
        void ControlUse()
        {
            groupBox1.Enabled = false;
        }

        /// <summary>
        /// 检测有关采购数据项内容是否正确
        /// </summary>
        /// <returns>返回检测结果</returns>
        bool CheckDataItem()
        {
            if (txtProposer.Text == "")
            {
                txtProposer.Focus();
                MessageDialog.ShowPromptMessage("请指定物品申请人!");
                return false;
            }
            else if (cmbStorage.Text == "")
            {
                cmbStorage.Focus();
                MessageDialog.ShowPromptMessage("所属库房不允许为空!");
                return false;
            }

            return true;
        }

        private void 新建toolStripButton1_Click(object sender, EventArgs e)
        {
            string billNo = txtBill_ID.Text;

            this.dataGridView1.CellEnter -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);

            if (!CheckDataItem())
                return;

            // 如果此单据存在则检查选择行
            if (m_goodsServer.IsExist(txtBill_ID.Text))
            {
                //if (!CheckSelectedRow())
                //    return;
            }
            else
            {
                S_FrockInDepotBill bill = new S_FrockInDepotBill();

                bill.Bill_ID = txtBill_ID.Text;
                bill.Bill_Time = dateTime_BillTime.Value;
                bill.JJRYID = BasicInfo.LoginID;
                bill.ProposerID = txtProposer.Tag as string;
                bill.DesignerID = "0000";
                bill.DepotManager = "0000";
                bill.StorageID = UniversalFunction.GetStorageID(cmbStorage.Text);

                if (txtDesigner.Text == "")
                {
                    bill.DesignerID = "0000";
                }
                else
                {
                    bill.DesignerID = txtDesigner.Tag as string;
                }

                bill.Provider = txtProvider.Text;                       // 供应商编码
                bill.Depot = txtMaterialType.Text;            // 仓库名就是材料类别
                bill.Remark = txtRemark.Text;

                bill.Bill_Status = OrdinaryInDepotBillStatus.新建单据.ToString();

                if (cbForck.Checked)
                {
                    m_blIsCreateFrockStandingBook = true;
                }
                else
                {
                    if (MessageDialog.ShowEnquiryMessage("是否确定单据不生成工装台账？") == DialogResult.Yes)
                    {
                        m_blIsCreateFrockStandingBook = false;
                    }
                    else
                    {
                        m_blIsCreateFrockStandingBook = true;
                        cbForck.Checked = true;
                    }
                }

                if (!m_goodsServer.AddBill(bill, out m_queryResult, out m_strErr))
                {
                    MessageDialog.ShowErrorMessage(m_strErr);
                    return;
                }

                groupBox1.Enabled = true;
                groupBox1.Enabled = true;
                toolStripSeparator2.Visible = true;
                toolStripSeparator3.Visible = true;
                toolStripSeparator7.Visible = true;
                toolStripSeparatorDelete.Visible = true;
                toolStripSeparator1.Visible = true;
                btnAdd.Visible = true;
                btnDelete.Visible = true;
                btnDeleteAll.Visible = true;
                btnUpdate.Visible = true;
                新建toolStripButton1.Visible = false;
            }
        }

        private void btnFindProposer_Click(object sender, EventArgs e)
        {
            FormPersonnel form = new FormPersonnel(txtProposer, "姓名");
            form.ShowDialog();

            txtProposer.Tag = form.UserCode;
        }

        private void btnFindDesigner_Click(object sender, EventArgs e)
        {
            FormPersonnel form = new FormPersonnel(txtDesigner, "姓名");
            form.ShowDialog();

            txtDesigner.Tag = form.UserCode;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!CheckDataItem())
            {
                return;
            }

            if (Convert.ToBoolean(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.开启车间管理模块]) && txtBatchNo.Text.Trim().Length == 0)
            {
                btnBatchNo.Focus();
                MessageDialog.ShowPromptMessage("批次号不能为空!");
                return;
            }

            int planCode = m_planCostServer.GetGoodsID(txtCode.Text, txtName.Text, txtSpec.Text, "030502", 
                (int)cmbUnit.SelectedValue, "自制件工装报检【"+m_billNo+"】自动生成", out m_strErr);

            S_FrockInDepotGoodsBill goods = new S_FrockInDepotGoodsBill();
            View_F_GoodsPlanCost planCost = m_planCostServer.GetGoodsInfo(txtCode.Text, txtName.Text, txtSpec.Text, out m_strErr);

            if (planCost == null)
            {
                return;
            }

            goods.Bill_ID = m_billNo;
            goods.BatchNo = txtBatchNo.Text;
            goods.Amount = numGoodsAmount.Value;
            goods.ShelfArea = "";
            goods.ColumnNumber = "";
            goods.LayerNumber = "";
            goods.Remark = txtRemarkList.Text;
            goods.StorageID = UniversalFunction.GetStorageID(cmbStorage.Text);
            goods.GoodsID = planCost.序号;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["图号型号"].Value.ToString() == txtCode.Text && row.Cells["物品名称"].Value.ToString() == txtName.Text &&
                    row.Cells["规格"].Value.ToString() == txtSpec.Text)
                {
                    MessageDialog.ShowPromptMessage("已经存在相同的物品信息列不允许再进行重复添加！");
                    return;
                }
            }

            if (!m_goodsServer.AddGoods(m_billNo, goods, out m_queryResultGoods, out m_strErr))
            {
                MessageDialog.ShowErrorMessage(m_strErr);
                return;
            }

            RefreshDataGridView(m_queryResultGoods);
            PositioningRecord(planCost.图号型号, planCost.物品名称, planCost.规格);
        }

        /// <summary>
        /// 刷新DataGridView
        /// </summary>
        /// <param name="goods">物品信息</param>
        void RefreshDataGridView(IEnumerable<View_S_FrockInDepotGoodsBill> goods)
        {
            ClearControl();

            if (goods == null)
            {
                return;
            }

            this.dataGridView1.Visible = false;
            this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                                                     this.dataGridView1_ColumnWidthChanged);
            this.dataGridView1.CellEnter -= new DataGridViewCellEventHandler(this.dataGridView1_CellEnter);

            dataGridView1.DataSource = goods; //GlobalObject.GeneralFunction.ConvertToDataTable<View_S_OrdinaryInDepotGoodsBill>(goods);
            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                                                     this.dataGridView1_ColumnWidthChanged);
            this.dataGridView1.CellEnter += new DataGridViewCellEventHandler(this.dataGridView1_CellEnter);

            dataGridView1.Refresh();

            dataGridView1.Columns["序号"].Visible = false;
            dataGridView1.Columns["单据号"].Visible = false;
            dataGridView1.Columns["库房代码"].Visible = false;
            dataGridView1.Columns["物品类别"].Visible = false;

            this.dataGridView1.Visible = true;
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

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            RefreshControl();

            if (m_dataLocalizer != null && e.RowIndex > -1)
            {
                m_dataLocalizer.StartIndex = e.RowIndex;
            }
        }

        /// <summary>
        /// 刷新控件
        /// </summary>
        void RefreshControl()
        {
            ClearControl();

            if (dataGridView1.CurrentRow != null)
            {
                View_S_FrockInDepotGoodsBill goods = GetGoodsInfo(dataGridView1.CurrentRow);

                txtCode.Text = goods.图号型号;
                txtName.Text = goods.物品名称;
                txtSpec.Text = goods.规格;
                numGoodsAmount.Value = (decimal)goods.数量;
                cmbUnit.Text = goods.单位;
                txtShelf.Text = goods.货架;
                txtColumn.Text = goods.列;
                txtLayer.Text = goods.层;
                txtRemark.Text = goods.备注;
            }
        }

        /// <summary>
        /// 从行记录中提取物品对象信息
        /// </summary>
        /// <param name="row">行记录</param>
        /// <returns>提取的物品信息</returns>
        private View_S_FrockInDepotGoodsBill GetGoodsInfo(DataGridViewRow row)
        {
            if (row == null)
            {
                return null;
            }

            IQueryable<View_S_FrockInDepotGoodsBill> data = dataGridView1.DataSource as IQueryable<View_S_FrockInDepotGoodsBill>;

            return (from r in data
                    where r.图号型号 == row.Cells["图号型号"].Value.ToString() 
                         && r.物品名称 == row.Cells["物品名称"].Value.ToString() 
                         && r.规格 == row.Cells["规格"].Value.ToString() 
                    select r).Single();
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
                View_F_GoodsPlanCost planCost = m_planCostServer.GetGoodsInfo(
                    row.Cells["图号型号"].Value.ToString(), row.Cells["物品名称"].Value.ToString(),
                    row.Cells["规格"].Value.ToString(), out m_strErr);
                    // 自制件工装报检物品清单与计划价格表中的物品是外键连接，删除了计划价格中的信息后就自动删除了清单中的信息
                    lstId.Add((int)row.Cells["序号"].Value);
            }

            if (!m_goodsServer.DeleteGoods(lstId, out m_strErr))
            {
                MessageDialog.ShowErrorMessage(m_strErr);
                return;
            }

            m_queryResultGoods = m_goodsServer.GetGoodsViewInfo(m_billNo);
            RefreshDataGridView(m_queryResultGoods);
            PositioningRecord(rowIndex);
        }

        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            if (MessageDialog.ShowEnquiryMessage("您真的想删除物品清单中的所有数据吗？") == DialogResult.Yes)
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    View_F_GoodsPlanCost planCost = m_planCostServer.GetGoodsInfo(
                        row.Cells["图号型号"].Value.ToString(), row.Cells["物品名称"].Value.ToString(),
                        row.Cells["规格"].Value.ToString(), out m_strErr);
                    DeletePlanPrice(planCost);
                }

                if (!m_goodsServer.DeleteGoods(m_billNo, out m_queryResultGoods, out m_strErr))
                {
                    MessageDialog.ShowErrorMessage(m_strErr);
                    return;
                }

                RefreshDataGridView(m_queryResultGoods);
            }
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

            if (Convert.ToBoolean(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.开启车间管理模块]) && txtBatchNo.Text.Trim().Length == 0)
            {
                btnBatchNo.Focus();
                MessageDialog.ShowPromptMessage("批次号不能为空!");
                return;
            }

            View_F_GoodsPlanCost planCost = m_planCostServer.GetGoodsInfo(txtCode.Text, txtName.Text, txtSpec.Text, out m_strErr);

            if (planCost == null)
            {
                return;
            }

            S_FrockInDepotGoodsBill goods = new S_FrockInDepotGoodsBill();
            View_S_FrockInDepotGoodsBill viewGoods = GetGoodsInfo(dataGridView1.SelectedRows[0]);

            goods.ID = viewGoods.序号;
            goods.GoodsID = planCost.序号;
            goods.Amount = numGoodsAmount.Value;

            if (m_operateMode == CE_BusinessOperateMode.仓库核实)
            {
                goods.ShelfArea = txtShelf.Text;
                goods.ColumnNumber = txtColumn.Text;
                goods.LayerNumber = txtLayer.Text;
            }

            goods.Remark = txtRemarkList.Text;

            if (!m_goodsServer.UpdateGoods(goods, out m_queryResultGoods, out m_strErr))
            {
                MessageDialog.ShowErrorMessage(m_strErr);
                return;
            }

            int rowIndex = dataGridView1.SelectedRows[0].Index;

            RefreshDataGridView(m_queryResultGoods);
            PositioningRecord(rowIndex);
        }

        private void 自制件工装报检明细_Load(object sender, EventArgs e)
        {
            if (m_lnqWSCode == null)
            {
                BillNumberControl billNoControl = new BillNumberControl(CE_BillTypeEnum.自制件工装报检.ToString(), m_goodsServer);
                billNoControl.CancelBill(m_billNo);
                MessageDialog.ShowPromptMessage("非车间人员不能填写自制件工装入库单");
                this.Close();
            }
        }

        private void btnBatchNo_Click(object sender, EventArgs e)
        {
            FormQueryInfo form = QueryInfoDialog.GetWorkShopBatchNoInfo(Convert.ToInt32(txtCode.Tag), m_lnqWSCode.WSCode);

            if (form.ShowDialog() == DialogResult.OK)
            {
                txtBatchNo.Text = (string)form.GetDataItem("批次号");
            }
        }
    }
}
