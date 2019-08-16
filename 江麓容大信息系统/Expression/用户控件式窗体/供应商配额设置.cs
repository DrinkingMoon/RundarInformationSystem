using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using ServerModule;
using GlobalObject;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 供应商配额设置界面
    /// </summary>
    public partial class 供应商配额设置 : Form
    {
        /// <summary>
        /// 产品信息服务组件
        /// </summary>
        IBomServer m_serverBom = ServerModuleFactory.GetServerModule<IBomServer>();

        /// <summary>
        /// 供应商责任人服务组件
        /// </summary>
        IAccessoryDutyInfoManageServer m_serverProviderDuty = ServerModuleFactory.GetServerModule<IAccessoryDutyInfoManageServer>();

        /// <summary>
        /// 数据定位控件
        /// </summary>
        UserControlDataLocalizer m_dataLocalizer;

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 服务组件
        /// </summary>
        IGoodsLeastPackAndStock m_GoodsLeast = ServerModuleFactory.GetServerModule<IGoodsLeastPackAndStock>();

        /// <summary>
        /// 服务组件
        /// </summary>
        IProductInfoServer m_productInfoServer = ServerModuleFactory.GetServerModule<IProductInfoServer>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 数据集
        /// </summary>
        B_GoodsLeastPackAndStock m_lnqLeast = new B_GoodsLeastPackAndStock();

        /// <summary>
        /// 供应商窗体
        /// </summary>
        FormQueryInfo m_formProvider;

        public 供应商配额设置(FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authFlag = nodeInfo.Authority;
            RefreshDataGirdView(m_GoodsLeast.GetAllInfo());
        }


        public 供应商配额设置(FunctionTreeNodeInfo nodeInfo, int goodsID)
        {
            InitializeComponent();

            m_authFlag = nodeInfo.Authority;
            RefreshDataGirdView(m_GoodsLeast.GetAllInfo());
            PositioningRecord(goodsID);
        }

        private void 供应商配额设置_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authFlag);
        }

        void txtCode_OnCompleteSearch()
        {
            txtCode.Text = txtCode.DataResult["图号型号"].ToString();
            txtCode.Tag = txtCode.DataResult["序号"].ToString();
            txtName.Text = txtCode.DataResult["物品名称"].ToString();
            txtSpec.Text = txtCode.DataResult["规格"].ToString();
            lb_DW_Pack.Text = txtCode.DataResult["单位"].ToString();
            lb_DW_Stock.Text = txtCode.DataResult["单位"].ToString();
        }

        /// <summary>
        /// 获得数据
        /// </summary>
        void GetMessage()
        {
            m_lnqLeast.GoodsID = txtCode.Tag == null ? 0 : Convert.ToInt32(txtCode.Tag);
            m_lnqLeast.ID = txtName.Tag == null ? 0 : Convert.ToInt32(txtName.Tag);
            m_lnqLeast.LeastPack = Convert.ToDecimal(txtLeastPack.Text);
            m_lnqLeast.LeastStock = Convert.ToDecimal(txtLeastStock.Text);
            m_lnqLeast.StockQuota = Convert.ToInt32(txtStockQuota.Text);
            m_lnqLeast.Provider = txtProvider.Text;
            m_lnqLeast.ProductDay = Convert.ToInt32(txtProductDay.Text);
            m_lnqLeast.ProviderLv = Convert.ToInt32(cmbProviderLv.Text);
        }

        /// <summary>
        /// 清除数据
        /// </summary>
        void Clear()
        {
            txtBuyer.Text = "";
            txtCode.Text = "";
            txtLeastPack.Text = "0";
            txtLeastStock.Text = "0";
            txtStockQuota.Text = "0";
            txtName.Text = "";
            txtProvider.Text = "";
            cmbProviderLv.Text = "1";
            txtSpec.Text = "";
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
        /// 刷新
        /// </summary>
        /// <param name="source">数据集</param>
        void RefreshDataGirdView(DataTable source)
        {
            dataGridView1.DataSource = source;

            if (m_dataLocalizer == null)
            {
                m_dataLocalizer = new UserControlDataLocalizer(dataGridView1, this.Name, 
                    UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));

                m_dataLocalizer.OnlyLocalize = true;
                panelPara.Controls.Add(m_dataLocalizer);
                m_dataLocalizer.Dock = DockStyle.Bottom;
            }
        }

        /// <summary>
        /// 检查数据
        /// </summary>
        /// <returns>通过返回True，否则返回False</returns>
        bool CheckMessage()
        {
            if (m_lnqLeast.GoodsID.ToString() == "0")
            {
                MessageDialog.ShowPromptMessage("请选择物品");
                return false;
            }

            if (m_lnqLeast.Provider.ToString() == "")
            {
                MessageDialog.ShowPromptMessage("请选择供应商");
                return false;
            }

            if (m_lnqLeast.LeastPack.ToString() == "0")
            {
                MessageDialog.ShowPromptMessage("最小包装数不能为0");
                return false;
            }

            if (m_lnqLeast.LeastStock.ToString() == "0")
            {
                MessageDialog.ShowPromptMessage("最小采购数不能为0");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 检查相同的数据
        /// </summary>
        /// <returns>相同返回False，否则返回True-</returns>
        bool CheckSameMessage(string strFlag)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (strFlag == "添加")
                {
                    if (m_lnqLeast.GoodsID.ToString() == dataGridView1.Rows[i].Cells["物品ID"].Value.ToString()
                        && m_lnqLeast.Provider.ToString() == dataGridView1.Rows[i].Cells["供应商"].Value.ToString())
                    {
                        MessageDialog.ShowPromptMessage("不能针对同一个物品添加同一个供应商");
                        return false;
                    }
                }
                else
                {
                    if (m_lnqLeast.GoodsID.ToString() == dataGridView1.Rows[i].Cells["物品ID"].Value.ToString()
                        && m_lnqLeast.Provider.ToString() == dataGridView1.Rows[i].Cells["供应商"].Value.ToString()
                        && m_lnqLeast.ID.ToString() != dataGridView1.Rows[i].Cells["序号"].Value.ToString())
                    {
                        MessageDialog.ShowPromptMessage("不能针对同一个物品添加同一个供应商");
                        return false;
                    }
                }
            }

            return true;
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.RowCount == 0)
            {
                return;
            }
            else
            {
                txtSpec.Text = dataGridView1.CurrentRow.Cells["规格"].Value.ToString();
                txtProvider.Text = dataGridView1.CurrentRow.Cells["供应商"].Value.ToString();
                txtName.Text = dataGridView1.CurrentRow.Cells["物品名称"].Value.ToString();
                txtName.Tag = dataGridView1.CurrentRow.Cells["序号"].Value.ToString();
                txtLeastStock.Text = dataGridView1.CurrentRow.Cells["最小采购数"].Value.ToString();
                txtLeastPack.Text = dataGridView1.CurrentRow.Cells["最小包装数"].Value.ToString();
                txtStockQuota.Text = dataGridView1.CurrentRow.Cells["采购份额"].Value.ToString();
                txtCode.Text = dataGridView1.CurrentRow.Cells["图号型号"].Value.ToString();
                txtBuyer.Text = dataGridView1.CurrentRow.Cells["采购员"].Value.ToString();
                txtCode.Tag = dataGridView1.CurrentRow.Cells["物品ID"].Value.ToString();
                lb_DW_Pack.Text = dataGridView1.CurrentRow.Cells["单位"].Value.ToString();
                lb_DW_Stock.Text = dataGridView1.CurrentRow.Cells["单位"].Value.ToString();
                cmbProviderLv.Text = dataGridView1.CurrentRow.Cells["供应商等级"].Value.ToString();
                txtProductDay.Text = dataGridView1.CurrentRow.Cells["生产周期"].Value.ToString();
            }
        }

        private void btnFindProvider_Click(object sender, EventArgs e)
        {
            m_formProvider = QueryInfoDialog.GetProviderInfoDialog();

            if (m_formProvider.ShowDialog() == DialogResult.OK)
            {
                txtProvider.Text = m_formProvider.GetStringDataItem("供应商编码");
                txtBuyer.Text = m_serverProviderDuty.GetProviderPrincipal(txtProvider.Text);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            GetMessage();

            if (!CheckMessage())
            {
                return;
            }

            if (!CheckSameMessage("添加"))
            {
                return;
            }
            else
            {
                if (!m_GoodsLeast.AddInfo(m_lnqLeast,out m_err))
                {
                    MessageDialog.ShowPromptMessage(m_err);
                }
                else
                {
                    MessageDialog.ShowPromptMessage("添加成功");
                }
            }

            RefreshDataGirdView(m_GoodsLeast.GetAllInfo());
            PositioningRecord(m_GoodsLeast.GetMaxID());
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
                if ((string)dataGridView1.Rows[i].Cells["序号"].Value.ToString() == billNo)
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
        /// <param name="billNo">定位用的单据号</param>
        void PositioningRecord(int goodsID)
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
                if ((int)dataGridView1.Rows[i].Cells["物品ID"].Value == goodsID)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                    break;
                }
            }
        }

        private void btnRefer_Click(object sender, EventArgs e)
        {
            RefreshDataGirdView(m_GoodsLeast.GetAllInfo());
            //Clear();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            GetMessage();

            if (!m_GoodsLeast.DeleteInfo(Convert.ToInt32(m_lnqLeast.ID),out m_err))
            {
                MessageDialog.ShowPromptMessage(m_err);
            }
            else
            {
                MessageDialog.ShowPromptMessage("删除成功");
            }

            RefreshDataGirdView(m_GoodsLeast.GetAllInfo());
            PositioningRecord(m_GoodsLeast.GetMaxID());
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string strID = dataGridView1.CurrentRow.Cells["序号"].Value.ToString();

            GetMessage();

            if (!CheckMessage())
            {
                return;
            }

            if (!CheckSameMessage("修改"))
            {
                return;
            }
            else
            {
                if (!m_GoodsLeast.UpdateInfo(m_lnqLeast, out m_err))
                {
                    MessageDialog.ShowPromptMessage(m_err);
                }
                else
                {
                    MessageDialog.ShowPromptMessage("修改成功");
                }
            }

            RefreshDataGirdView(m_GoodsLeast.GetAllInfo());
            PositioningRecord(strID);
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void txtCode_Enter(object sender, EventArgs e)
        {
            txtCode.StrEndSql = " and 序号 in (select distinct GoodsID from CG_CBOM)";
        }

        private void 比较BOMtoolStripButton_Click(object sender, EventArgs e)
        {
            FormCompareBom frm = new FormCompareBom("供应商配额");

            frm.ShowDialog();
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                e.RowBounds.Location.Y,
                dataGridView1.RowHeadersWidth - 4,
                e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                dataGridView1.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dataGridView1.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }
    }
}
