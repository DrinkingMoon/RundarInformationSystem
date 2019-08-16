using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using GlobalObject;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 自制件退货明细界面
    /// </summary>
    public partial class FormHomemadeRejectlist : Form
    {
        /// <summary>
        /// 库房信息服务组件
        /// </summary>
        IStorageInfo m_serverStorageInfo = ServerModuleFactory.GetServerModule<IStorageInfo>();

        /// <summary>
        /// 库存服务
        /// </summary>
        IStoreServer m_serverStore = ServerModuleFactory.GetServerModule<IStoreServer>();

        /// <summary>
        /// 操作模式
        /// </summary>
        public enum OperateMode { 查看, 修改, 仓库核实 }

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr;

        /// <summary>
        /// 操作模式
        /// </summary>
        OperateMode m_operateMode;

        /// <summary>
        /// 自制件退货单号
        /// </summary>
        string m_strBillID;

        /// <summary>
        /// 自制件退货单物品清单服务
        /// </summary>
        IHomemadeRejectList m_goodsServer = ServerModuleFactory.GetServerModule<IHomemadeRejectList>();

        /// <summary>
        /// 查询到的物品信息集
        /// </summary>
        DataTable m_queryGoodsInfo = new DataTable();

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

        /// <summary>
        /// 库房
        /// </summary>
        string m_strStorage;

        public FormHomemadeRejectlist(OperateMode operateMode, string vProvider, string billNo)
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | 
                ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();

            m_operateMode = operateMode;
            m_strProvider = vProvider;
            m_strBillID = billNo;

            if (m_operateMode == OperateMode.查看)
            {
                toolStrip1.Visible = false;
            }

            m_queryGoodsInfo = m_goodsServer.GetBillView(m_strBillID);
            RefreshDataGridView(m_queryGoodsInfo);

            // 添加数据定位控件
            m_dataLocalizer = new UserControlDataLocalizer(
                dataGridView1, this.Name, UniversalFunction.SelectHideFields(
                this.Name, dataGridView1.Name, BasicInfo.LoginID));
            txtProvider.Text = m_strProvider;
            panelTop.Controls.Add(m_dataLocalizer);
            m_dataLocalizer.Dock = DockStyle.Bottom;
            m_strStorage = m_serverStorageInfo.GetStorageID(billNo, "S_HomemadeRejectBill", "Bill_ID");
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
            this.dataGridView1.CellEnter -= new DataGridViewCellEventHandler(this.dataGridView1_CellEnter);

            dataGridView1.DataSource = goods;
            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

            this.dataGridView1.CellEnter += new DataGridViewCellEventHandler(this.dataGridView1_CellEnter);

            dataGridView1.Refresh();

            dataGridView1.Columns["序号"].Visible = false;
            dataGridView1.Columns["退货单号"].Visible = false;
            dataGridView1.Columns["物品ID"].Visible = false;

            this.dataGridView1.Visible = true;

            if (goods.Rows.Count > 0)
            {
                lblAmount.Text = Convert.ToString(goods.Rows.Count);
            }
            else
            {
                lblAmount.Text = "0";
            }
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

            lblRecordRow.Text = (e.RowIndex + 1).ToString();

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
                View_S_HomemadeRejectList goods = GetGoodsInfo(dataGridView1.CurrentRow);

                txtCode.Text = goods.图号型号;
                txtCode.Tag = goods.物品ID;
                txtName.Text = goods.物品名称;
                txtSpec.Text = goods.规格;
                txtProvider.Text = goods.供应商;
                txtProviderBatchNo.Text = goods.供方批次;

                if (!string.IsNullOrEmpty(goods.批次号))
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

        private void btnFindCode_Click(object sender, EventArgs e)
        {
            if (m_operateMode != OperateMode.仓库核实)
            {
                FormQueryInfo form = QueryInfoDialog.GetPlanCostGoodsDialogSift(" and AttributeID = "
                    + (int)CE_GoodsAttributeName.自制件 + " and AttributeValue = '" + bool.TrueString + "'");

                if (form != null && form.ShowDialog() == DialogResult.OK)
                {
                    txtCode.Text = form.GetDataItem("图号型号").ToString();
                    txtName.Text = form.GetDataItem("物品名称").ToString();
                    txtSpec.Text = form.GetDataItem("规格").ToString();
                    txtUnit.Text = form.GetStringDataItem("单位");
                    txtCode.Tag = form.GetDataItem("序号");
                    //txtProvider.Text = form.GetStringDataItem("供应商");
                    txtBatchNo.Text = form.GetStringDataItem("批次号");
                    txtProviderBatchNo.Text = form.GetStringDataItem("供方批次");
                }
            }
        }

        /// <summary>
        /// 从行记录中提取物品对象信息
        /// </summary>
        /// <param name="row">行记录</param>
        /// <returns>提取的物品信息</returns>
        View_S_HomemadeRejectList GetGoodsInfo(DataGridViewRow row)
        {
            if (row == null)
            {
                return null;
            }

            View_S_HomemadeRejectList goods = new View_S_HomemadeRejectList();

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

            View_F_GoodsPlanCost basicGoodsInfo = null;

            if (row.Cells["单位"].Value != System.DBNull.Value)
            {
                goods.单位 = (string)row.Cells["单位"].Value;
                goods.物品类别 = (string)row.Cells["物品类别"].Value;
                goods.货架 = Convert.ToString(row.Cells["货架"].Value);
                goods.列 = Convert.ToString(row.Cells["列"].Value);
                goods.层 = Convert.ToString(row.Cells["层"].Value);
            }
            else
            {
                IBasicGoodsServer basicGoodsServer = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();

                basicGoodsInfo = basicGoodsServer.GetGoodsInfo(goods.图号型号, goods.物品名称, goods.规格, out m_strErr);

                if (!string.IsNullOrEmpty(m_strErr))
                {
                    MessageDialog.ShowErrorMessage(m_strErr);
                    return null;
                }

                goods.单位 = basicGoodsInfo.单位;
            }

            return goods;
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
            if (txtBatchNo.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请选择物品批次号");
                return false;
            }

            if (numAmount.Value == 0)
            {
                numAmount.Focus();

                MessageDialog.ShowPromptMessage("退货数量必须 > 0");
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

            S_HomemadeRejectList goods = new S_HomemadeRejectList();

            View_S_HomemadeRejectList viewGoods = GetGoodsInfo(dataGridView1.SelectedRows[0]);

            goods.ID = viewGoods.序号;
            goods.Bill_ID = m_strBillID;

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

            if (!m_goodsServer.UpdateGoods(goods, m_strStorage, out m_strErr))
            {
                MessageDialog.ShowErrorMessage(m_strErr);
                return;
            }

            GetCodeInfoFromForm();

            m_queryGoodsInfo = m_goodsServer.GetBillView(m_strBillID);//.GetGoods(BillNo);
            RefreshDataGridView(m_queryGoodsInfo);
            PositioningRecord(m_strGoodsCode, m_strGoodsName, m_strSpec);
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
            txtBatchNo.Text = "";
            txtUnit.Text = "";
            txtDepot.Text = "";
            txtShelf.Text = "";
            txtColumn.Text = "";
            txtLayer.Text = "";
            txtRemark.Text = "";
            numAmount.Value = 0;
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

            S_HomemadeRejectList goods = new S_HomemadeRejectList();

            goods.Bill_ID = m_strBillID;
            goods.GoodsID = (int)txtCode.Tag;
            goods.Provider = txtProvider.Text;
            goods.ProviderBatchNo = txtProviderBatchNo.Text;
            goods.BatchNo = txtBatchNo.Text;
            goods.Amount = numAmount.Value;
            goods.Remark = txtRemark.Text;
            DataTable dvt = m_goodsServer.GetBillView(m_strBillID);

            if (CheckSameGoods(dvt,goods))
            {
                if (!m_goodsServer.AddGoods(goods, m_strStorage, out m_strErr))
                {
                    MessageDialog.ShowErrorMessage(m_strErr);
                    return;
                }
            }

            GetCodeInfoFromForm();

            m_queryGoodsInfo = m_goodsServer.GetBillView(m_strBillID);
            RefreshDataGridView(m_queryGoodsInfo);
            PositioningRecord(m_strGoodsCode, m_strGoodsName, m_strSpec);
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

            if (!m_goodsServer.DeleteGoods(lstId, out m_strErr))
            {
                MessageDialog.ShowErrorMessage(m_strErr);
                return;
            }

            m_queryGoodsInfo = m_goodsServer.GetBillView(m_strBillID);
            RefreshDataGridView(m_queryGoodsInfo);
            PositioningRecord(rowIndex);
        }

        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            if (MessageDialog.ShowEnquiryMessage("您真的想删除物品清单中的所有数据吗？") == DialogResult.Yes)
            {
                if (!m_goodsServer.DeleteGoods(m_strBillID, out m_strErr))
                {
                    MessageDialog.ShowErrorMessage(m_strErr);
                    return;
                }

                m_queryGoodsInfo = m_goodsServer.GetBillView(m_strBillID);
                RefreshDataGridView(m_queryGoodsInfo);
            }
        }

        /// <summary>
        /// 检查相同物品
        /// </summary>
        /// <param name="Dt">需要检测的数据集</param>
        /// <param name="goods">自制件明细细细</param>
        /// <returns>无相同的返回True，否则返回False</returns>
        bool CheckSameGoods(DataTable dt,S_HomemadeRejectList goods)
        {
            if (dt.Rows.Count > 0)
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
            }

            return true;
        }

        private void txtBatchNo_OnCompleteSearch()
        {
            txtBatchNo.Text = txtBatchNo.DataResult["批次号"].ToString();
            txtProviderBatchNo.Text = txtBatchNo.DataResult["供方批次"].ToString();
        }

        private void txtBatchNo_Enter(object sender, EventArgs e)
        {
            if (txtCode.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请先选择图号型号！");
                return;
            }

            string strSql = " and a.GoodsID='" + txtCode.Tag + "' and a.StorageID = '" + m_strStorage
                + "' and a.Provider = '" + m_strProvider + "'";

            txtBatchNo.StrEndSql = strSql;
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
                S_HomemadeRejectList goods = new S_HomemadeRejectList();

                goods.Bill_ID = m_strBillID;
                goods.GoodsID = Convert.ToInt32(dt.Rows[i]["GoodsID"].ToString());
                goods.Provider = dt.Rows[i]["Provider"].ToString();
                goods.ProviderBatchNo = "";
                goods.BatchNo = dt.Rows[i]["BatchNo"].ToString();
                goods.Amount = Convert.ToDecimal(dt.Rows[i]["Quantity"].ToString());
                goods.Remark = dt.Rows[i]["Reason"].ToString();

                DataTable dvt = m_goodsServer.GetBillView(m_strBillID);

                if (CheckSameGoods(dvt, goods))
                {
                    if (!m_goodsServer.AddGoods(goods, m_strStorage, out m_strErr))
                    {
                        MessageDialog.ShowErrorMessage(m_strErr);
                        return;
                    }
                }
            }

            GetCodeInfoFromForm();
            m_queryGoodsInfo = m_goodsServer.GetBillView(m_strBillID);
            RefreshDataGridView(m_queryGoodsInfo);
        }
    }
}
