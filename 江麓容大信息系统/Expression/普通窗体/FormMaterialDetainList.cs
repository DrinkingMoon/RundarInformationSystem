using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GlobalObject;
using ServerModule;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 物料扣货单明细界面
    /// </summary>
    public partial class FormMaterialDetainList : Form
    {

        /// <summary>
        /// 库房代码
        /// </summary>
        string m_strStorageID = "";

        /// <summary>
        /// 单据状态
        /// </summary>
        string m_strBillStatus;

        /// <summary>
        /// 扣货单据号
        /// </summary>
        string m_strBillID;

        /// <summary>
        /// 数据定位控件
        /// </summary>
        UserControlDataLocalizer m_dataLocalizer;

        /// <summary>
        /// 物料扣货服务类
        /// </summary>
        IMaterialDetainBill m_goodsServer = ServerModuleFactory.GetServerModule<IMaterialDetainBill>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr;

        /// <summary>
        /// 模式状态
        /// </summary>
        CE_BusinessOperateMode m_operateMode;

        /// <summary>
        /// 车间管理基础服务
        /// </summary>
        Service_Manufacture_WorkShop.IWorkShopBasic m_serverWSBasic =
            Service_Manufacture_WorkShop.ServerModuleFactory.GetServerModule<Service_Manufacture_WorkShop.IWorkShopBasic>();

        /// <summary>
        /// 车间管理信息
        /// </summary>
        WS_WorkShopCode m_lnqWSCode = new WS_WorkShopCode();

        public FormMaterialDetainList(CE_BusinessOperateMode operateMode, string vProvider, string billNo,string status,string Storage)
        {
            InitializeComponent();
            m_strBillStatus = status;
            m_strBillID = billNo;

            S_MaterialDetainBill tempBill = m_goodsServer.GetBill(billNo);

            m_lnqWSCode = tempBill == null ?
                m_serverWSBasic.GetPersonnelWorkShop(BasicInfo.LoginID) :
                m_serverWSBasic.GetPersonnelWorkShop(tempBill.FillInPersonCode);

            if (BasicInfo.LoginRole == CE_RoleEnum.采购员.ToString())
            {
                btnSearch.Visible = true;
            }

            if (m_strBillStatus.Equals("新建单据"))
            {
                btnUpdate.Visible = true;
            }

            m_operateMode = operateMode;

            if (operateMode == CE_BusinessOperateMode.查看)
            {
                toolStrip1.Visible = false;
            }
            else if (operateMode == CE_BusinessOperateMode.采购确认)
            {
                if (m_strBillStatus.Equals("等待采购确认"))
                {
                    btnUpdate.Visible = true;
                    btnSearch.Visible = true;
                    btnAdd.Visible = false;
                    btnDelete.Visible = false;
                    btnDeleteAll.Visible = false;
                    btnNew.Visible = false;                    
                }
                else
                {
                    toolStrip1.Visible = false;
                }
            }
            
            RefreshDataGridView(m_goodsServer.GetList(m_strBillID, out m_strErr));

            m_dataLocalizer = new UserControlDataLocalizer(
                dataGridView1, this.Name, UniversalFunction.SelectHideFields(
                this.Name, dataGridView1.Name, BasicInfo.LoginID));

            panelTop.Controls.Add(m_dataLocalizer);
            m_dataLocalizer.Dock = DockStyle.Bottom;

            txtProvider.Text = vProvider;
            m_strStorageID = Storage;

            if (!Convert.ToBoolean(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.开启车间管理模块]))
            {
                txtCode.FindItem = TextBoxShow.FindType.所有物品批次;
                txtCode.Enter += new EventHandler(txtCode_Enter);
                txtBatchNo.Enabled = false;
            }
        }

        /// <summary>
        /// 清除窗体上的信息
        /// </summary>
        void ClearControl()
        {
            txtCode.Text = "";
            txtName.Text = "";
            txtName.Tag = null;
            txtSpec.Text = "";
            txtBatchNo.Text = "";
            numAmount.Value = 0;
            txtUnit.Text = "";
            txtRemark.Text = "";
        }

        /// <summary>
        /// 检查相同物品
        /// </summary>
        /// <param name="Dt">需要检测的数据集</param>
        /// <param name="goods">物品明细信息</param>
        /// <returns>不相同返回True，相同返回False</returns>
        bool CheckSameGoods(DataTable dt, S_MaterialDetainList goods)
        {

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["物品ID"].ToString() == goods.GoodsID.ToString()
                    && dt.Rows[i]["批次号"].ToString() == goods.BatchNo.ToString())
                {
                    MessageBox.Show("不能添加同一个物品，请重新确认物品", "提示");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="billNo">定位用的单据号</param>
        void PositioningRecord(string billNo)
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
                if ((string)dataGridView1.Rows[i].Cells["扣货单号"].Value == billNo)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                    break;
                }
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
                MessageDialog.ShowPromptMessage("物料扣货数量必须 > 0");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 刷新DataGridView
        /// </summary>
        /// <param name="goods">物品信息</param>
        void RefreshDataGridView(DataTable goods)
        {
            if (goods == null)
            {
                return;
            }

            this.dataGridView1.Visible = false;
            this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);
            this.dataGridView1.CellEnter -= new DataGridViewCellEventHandler(this.dataGridView1_CellEnter);

            dataGridView1.DataSource = goods; //GlobalObject.GeneralFunction.ConvertToDataTable<S_MaterialDetainList>(goods);
            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);
            this.dataGridView1.CellEnter += new DataGridViewCellEventHandler(this.dataGridView1_CellEnter);

            dataGridView1.Refresh();

            dataGridView1.Columns["序号"].Visible = false;
            dataGridView1.Columns["扣货单号"].Visible = false;
            dataGridView1.Columns["物品ID"].Visible = false;

            this.dataGridView1.Visible = true;
            lblAmount.Text = goods.Rows.Count.ToString();
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        private void FormMaterialDetainList_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            ClearControl();
        }

        private void txtCode_OnCompleteSearch()
        {
            txtCode.Text = txtCode.DataResult["图号型号"].ToString();
            txtName.Text = txtCode.DataResult["物品名称"].ToString();
            txtName.Tag =
                !Convert.ToBoolean(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.开启车间管理模块]) ? txtCode.DataResult["物品ID"].ToString() :
                txtCode.DataResult["序号"].ToString();
            txtSpec.Text = txtCode.DataResult["规格"].ToString();
            txtUnit.Text = txtCode.DataResult["单位"].ToString();
        }

        private void txtCode_Enter(object sender, EventArgs e)
        {
            string strSql = " and 供货单位='" + txtProvider.Text.Trim() + "' and 库房代码 = '" + m_strStorageID + "'";
            txtCode.StrEndSql = strSql;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!CheckDataItem())
            {
                return;
            }

            S_MaterialDetainList goods = new S_MaterialDetainList();

            goods.Bill_ID = m_strBillID;
            goods.GoodsID = Convert.ToInt32(txtName.Tag.ToString());
            goods.Provider = txtProvider.Text;
            goods.BatchNo = txtBatchNo.Text;
            goods.Amount = numAmount.Value;
            goods.Remark = txtRemark.Text;

            DataTable dvt = (DataTable)dataGridView1.DataSource;

            if (CheckSameGoods(dvt, goods))
            {
                if (!m_goodsServer.AddList(goods, out m_strErr))
                {
                    MessageDialog.ShowErrorMessage(m_strErr);
                    return;
                }
            }

            RefreshDataGridView(m_goodsServer.GetList(m_strBillID,out m_strErr));
            PositioningRecord(m_strBillID);
            ClearControl();
        }

        /// <summary>
        /// 从行记录中提取物品对象信息
        /// </summary>
        /// <param name="row">行记录</param>
        /// <returns>提取的物品信息</returns>
        View_S_MaterialDetainList GetGoodsInfo(DataGridViewRow row)
        {
            if (row == null)
            {
                return null;
            }

            View_S_MaterialDetainList goods = new View_S_MaterialDetainList();

            goods.序号 = (int)row.Cells["序号"].Value;
            goods.扣货单号 = (string)row.Cells["扣货单号"].Value;
            goods.物品ID = (int)row.Cells["物品ID"].Value;
            goods.图号型号 = (string)row.Cells["图号型号"].Value;
            goods.物品名称 = (string)row.Cells["物品名称"].Value;
            goods.规格 = (string)row.Cells["规格"].Value;
            goods.供应商 = (string)row.Cells["供应商"].Value;
            goods.批次号 = (string)row.Cells["批次号"].Value;
            goods.扣货数 = (decimal)row.Cells["扣货数"].Value;
            goods.备注 = (string)row.Cells["备注"].Value;
            goods.单位 = row.Cells["单位"].Value.ToString();
            goods.关联订单号 = (string)row.Cells["关联订单号"].Value.ToString();

            View_F_GoodsPlanCost basicGoodsInfo = null;

            IBasicGoodsServer basicGoodsServer = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();

            basicGoodsInfo = basicGoodsServer.GetGoodsInfo(goods.图号型号, goods.物品名称, goods.规格, out m_strErr);

            if (!GlobalObject.GeneralFunction.IsNullOrEmpty(m_strErr))
            {
                MessageDialog.ShowErrorMessage(m_strErr);
                return null;
            }

            return goods;
        }

        /// <summary>
        /// 刷新控件
        /// </summary>
        void RefreshControl()
        {
            ClearControl();

            if (dataGridView1.CurrentRow != null)
            {
                View_S_MaterialDetainList goods = GetGoodsInfo(dataGridView1.CurrentRow);

                txtCode.Text = goods.图号型号;
                txtName.Tag = goods.物品ID;
                txtName.Text = goods.物品名称;
                txtSpec.Text = goods.规格;
                txtProvider.Text = goods.供应商;
                txtAssociateID.Text = goods.关联订单号;

                if (!GlobalObject.GeneralFunction.IsNullOrEmpty(goods.批次号))
                {
                    txtBatchNo.Text = goods.批次号;
                }

                numAmount.Value = (decimal)goods.扣货数;

                txtUnit.Text = goods.单位;
                txtRemark.Text = goods.备注;
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
                lstId.Add(Convert.ToInt64(row.Cells["序号"].Value.ToString()));
            }

            if (!m_goodsServer.DeleteGoods(lstId,out m_strErr))
            {
                MessageDialog.ShowErrorMessage(m_strErr);
                return;
            }

            RefreshDataGridView(m_goodsServer.GetList(m_strBillID,out m_strErr));
            PositioningRecord(m_strBillID);
            ClearControl();
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (m_operateMode == CE_BusinessOperateMode.采购确认)
            {
                DataTable dataTable = m_goodsServer.GetOrderFormInfo(txtName.Tag.ToString(), txtBatchNo.Text, txtProvider.Text, out m_strErr);

                FormQueryInfo form = new FormQueryInfo(dataTable);

                if (DialogResult.OK == form.ShowDialog())
                {
                    txtAssociateID.Text = form.GetDataItem("订单号").ToString();
                }
            }
        }

        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            string info = string.Format("您是否确定删除{0}号单据的物料清单？", m_strBillID);

            if (MessageDialog.ShowEnquiryMessage(info) == DialogResult.No)
            {
                return;
            }

            if (!m_goodsServer.DeleteList(m_strBillID, out m_strErr))
            {
                MessageDialog.ShowErrorMessage(m_strErr);
                return;
            }

            RefreshDataGridView(m_goodsServer.GetList(m_strBillID, out m_strErr));
            PositioningRecord(m_strBillID);
            ClearControl();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择一条记录后再进行此操作");
                return;
            }

            if (dataGridView1.SelectedRows.Count > 1)
            {
                MessageDialog.ShowPromptMessage("只允许选择一条记录进行处理！");
                return;
            }

            if (BasicInfo.ListRoles.Contains(CE_RoleEnum.采购员.ToString()) && m_operateMode == CE_BusinessOperateMode.采购确认)
            {
                if (txtAssociateID.Text.Trim() != "")
                {
                    if (!m_goodsServer.UpdateList(m_strBillID, txtName.Tag.ToString(),
                        txtAssociateID.Text, txtBatchNo.Text.Trim(), out m_strErr))
                    {
                        MessageDialog.ShowErrorMessage(m_strErr);
                        return;
                    }
                }
                else
                {
                    MessageDialog.ShowPromptMessage("请选择关联订单号！");
                }
            }
            else if(m_strBillStatus.Equals("新建单据"))
            {
                S_MaterialDetainList goods = new S_MaterialDetainList();
                View_S_MaterialDetainList viewGoods = GetGoodsInfo(dataGridView1.SelectedRows[0]);

                goods.ID = viewGoods.序号;
                goods.Bill_ID = m_strBillID;

                if (txtName.Tag != null && (int)txtName.Tag != 0)
                {
                    goods.GoodsID = (int)txtName.Tag;
                }
                else
                {
                    goods.GoodsID = viewGoods.物品ID;
                }

                goods.Provider = txtProvider.Text;
                goods.BatchNo = txtBatchNo.Text;
                goods.Amount = numAmount.Value;
                goods.Remark = txtRemark.Text;

                if (!m_goodsServer.UpdateList(goods, m_strStorageID, out m_strErr))
                {
                    MessageDialog.ShowErrorMessage(m_strErr);
                    return;
                }
            }

            RefreshDataGridView(m_goodsServer.GetList(m_strBillID, out m_strErr));
            PositioningRecord(m_strBillID);
            ClearControl();
        }

        private void txtBatchNo_OnCompleteSearch()
        {
            txtBatchNo.Text = txtBatchNo.DataResult["批次号"].ToString();
        }

        private void txtBatchNo_Enter(object sender, EventArgs e)
        {
            txtBatchNo.StrEndSql = " and 物品ID = " + Convert.ToInt32(txtName.Tag) + " and 车间代码 = '" + m_lnqWSCode.WSCode + "'";
        }

        private void FormMaterialDetainList_Load(object sender, EventArgs e)
        {
            //if (m_lnqWSCode == null)
            //{
            //    MessageDialog.ShowPromptMessage("非车间人员不能填写扣货单");
            //    this.Close();
            //}
        }

        private void txtBatchNo_Click(object sender, EventArgs e)
        {
            txtBatchNo.StrEndSql = " and 物品ID = " + Convert.ToInt32(txtName.Tag) + " and 车间代码 = '" + m_lnqWSCode.WSCode + "'";
        }
    }
}
