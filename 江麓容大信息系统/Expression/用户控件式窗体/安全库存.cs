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
    /// 安全库存界面
    /// </summary>
    public partial class 安全库存 : Form
    {
        #region 成员变量

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_error;

        /// <summary>
        /// 安全库存服务接口
        /// </summary>
        ISafeStockServer m_safeStockServer = ServerModuleFactory.GetServerModule<ISafeStockServer>();

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authorityFlag;

        /// <summary>
        /// 数据定位控件
        /// </summary>
        UserControlDataLocalizer m_dataLocalizer;

        /// <summary>
        /// 设置总成数量对话框（设置单个零件所需的各总成的数量）
        /// </summary>
        //设置总成数量 m_formSetNumberOfProduct;

        #endregion

        #region 构造器


        public 安全库存(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authorityFlag = nodeInfo.Authority;
            RefreshDataGridView();
        }

        #endregion

        #region 方法

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="billNo">定位用的单据号</param>
        void PositioningRecord(string ID)
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
                if ((string)dataGridView1.Rows[i].Cells["物品ID"].Value.ToString() == ID)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                    break;
                }
            }
        }

        /// <summary>
        /// 刷新DataGirdView
        /// </summary>
        private void RefreshDataGridView()
        {
            DataTable dt = m_safeStockServer.GetAllInfo();
            dataGridView1.DataSource = dt;

            if (m_dataLocalizer == null)
            {
                m_dataLocalizer = new UserControlDataLocalizer(dataGridView1, this.Name, UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
                m_dataLocalizer.OnlyLocalize = true;
                panelPara.Controls.Add(m_dataLocalizer);
                m_dataLocalizer.Dock = DockStyle.Bottom;
            }

        }

        /// <summary>
        /// 清空数据
        /// </summary>
        private void ClearDate()
        {
            tbsGoods.Text = "";
            tbsGoods.Tag = -1;
            txtGoodsCode.Text = "";
            txtSpce.Text = "";
            txtRemark.Text = "";
            numSafeStockCount.Value = 0;
        }

        #endregion

        #region 控件方法

        private void btnNew_Click(object sender, EventArgs e)
        {
            RefreshDataGridView();
            ClearDate();
        }

        /// <summary>
        /// 检查数据项
        /// </summary>
        /// <returns>数据成功返回true，失败返回false</returns>
        private bool CheckData()
        {
            if (GlobalObject.GeneralFunction.IsNullOrEmpty(tbsGoods.Text))
            {
                MessageDialog.ShowPromptMessage("请选择物品信息后再进行此操作");
                return false;
            }

            //if (m_formSetNumberOfProduct == null)
            //{
            //    btnSetNumberOfProduct.PerformClick();
            //}

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells["图号型号"].Value.ToString() == txtGoodsCode.Text
                    && dataGridView1.Rows[i].Cells["物品名称"].Value.ToString() == tbsGoods.Text
                    && dataGridView1.Rows[i].Cells["规格"].Value.ToString() == txtSpce.Text)
                {
                    MessageDialog.ShowPromptMessage("不能添加重复物品，请重新核查");
                    return false;
                }
            }

            return true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!CheckData())
            {
                return;
            }

            S_SafeStock lnqSafe = new S_SafeStock();

            lnqSafe.GoodsID = Convert.ToInt32(tbsGoods.Tag);
            lnqSafe.Remark = txtRemark.Text.Replace("总成自动生成", "");
            lnqSafe.SafeStockCount = numSafeStockCount.Value;

            if (!m_safeStockServer.AddInfo(lnqSafe,out m_error))
            {
                MessageDialog.ShowPromptMessage(m_error);
            }
            else
            {
                MessageDialog.ShowPromptMessage("添加成功");
            }

            ClearDate();
            RefreshDataGridView();
            PositioningRecord(lnqSafe.GoodsID.ToString());
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            S_SafeStock lnqSafe = new S_SafeStock();

            lnqSafe.GoodsID = Convert.ToInt32(tbsGoods.Tag);
            lnqSafe.Remark = txtRemark.Text.Replace("总成自动生成", "");
            lnqSafe.SafeStockCount = numSafeStockCount.Value;

            if (!m_safeStockServer.UpdateInfo(lnqSafe, Convert.ToInt32(dataGridView1.CurrentRow.Cells["物品ID"].Value), out m_error))
            {
                MessageDialog.ShowPromptMessage(m_error);
            }
            else
            {
                MessageDialog.ShowPromptMessage("修改成功");
            }

            ClearDate();
            RefreshDataGridView();
            PositioningRecord(lnqSafe.GoodsID.ToString());
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!m_safeStockServer.DeleteInfo(Convert.ToInt32(dataGridView1.CurrentRow.Cells["物品ID"].Value), out m_error))
            {
                MessageDialog.ShowPromptMessage(m_error);
            }
            else
            {
                MessageDialog.ShowPromptMessage("删除成功");
            }

            ClearDate();
            RefreshDataGridView();
        }
        #endregion

        private void 安全库存_Load(object sender, EventArgs e)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip1, m_authorityFlag);
        }

        void tbsGoods_OnCompleteSearch()
        {
            tbsGoods.Text = tbsGoods.DataResult["物品名称"].ToString();
            tbsGoods.Tag = tbsGoods.DataResult["序号"].ToString();
            txtGoodsCode.Text = tbsGoods.DataResult["图号型号"].ToString();
            txtGoodsType.Tag = tbsGoods.DataResult["物品类别"].ToString();
            txtSpce.Text = tbsGoods.DataResult["规格"].ToString();
            lbUnit.Text = tbsGoods.DataResult["单位"].ToString();
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewRow dr = dataGridView1.Rows[i];

                if (Convert.ToDecimal( dataGridView1.Rows[i].Cells["安全库存数"].Value) > 
                    Convert.ToDecimal( dataGridView1.Rows[i].Cells["实际库存数量"].Value))
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                }
            }
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                ClearDate();
                return;
            }
            else
            {
                numSafeStockCount.Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["安全库存数"].Value);
                txtGoodsType.Text = dataGridView1.CurrentRow.Cells["材料类别"].Value.ToString();
                tbsGoods.Text = dataGridView1.CurrentRow.Cells["物品名称"].Value.ToString();
                tbsGoods.Tag = Convert.ToInt32(dataGridView1.CurrentRow.Cells["物品ID"].Value);
                txtGoodsCode.Text = dataGridView1.CurrentRow.Cells["图号型号"].Value.ToString();
                txtSpce.Text = dataGridView1.CurrentRow.Cells["规格"].Value.ToString();
                txtRemark.Text = dataGridView1.CurrentRow.Cells["备注"].Value.ToString();
                lbUnit.Text = dataGridView1.CurrentRow.Cells["单位"].Value.ToString();
            }
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

        private void btnOutDownSafeCount_Click(object sender, EventArgs e)
        {
            DataTable dtDate = (DataTable)dataGridView1.DataSource;

            DataRow[] dr = dtDate.Select("安全库存数 > 实际库存数量");

            DataTable dtPrint = dtDate.Clone();

            for (int i = 0; i < dr.Length; i++)
            {
                dtPrint.ImportRow(dr[i]);
            }

            ExcelHelperP.DataTableToExcel(saveFileDialog1, dtPrint, null);
        }

        private void btnOutExcel_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
        }

        private void btnShowDownSafeCount_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                DataTable dtDate = (DataTable)dataGridView1.DataSource;

                DataRow[] dr = dtDate.Select("安全库存数 > 实际库存数量");

                DataTable dtPrint = dtDate.Clone();

                for (int i = 0; i < dr.Length; i++)
                {
                    dtPrint.ImportRow(dr[i]);
                }

                dataGridView1.DataSource = dtPrint;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshDataGridView();
        }

        private void btnCreateInfo_Click(object sender, EventArgs e)
        {
            由总成自动生成安全库存 form = new 由总成自动生成安全库存();
            form.ShowDialog();

            if (form.DtSafeGoods != null && form.DtSafeGoods.Rows.Count != 0)
            {
                if (!m_safeStockServer.OperationInfo(form.DtSafeGoods,out m_error))
                {
                    MessageDialog.ShowPromptMessage(m_error);
                }
                else
                {
                    MessageDialog.ShowPromptMessage("生成成功");
                }


                RefreshDataGridView();
            }
        }

        private void 比较BOMtoolStripButton_Click(object sender, EventArgs e)
        {
            FormCompareBom frm = new FormCompareBom("安全库存");

            frm.ShowDialog();
        }

        //private void btnSave_Click(object sender, EventArgs e)
        //{
        //    if (dataGridView1.Rows.Count == 0)
        //    {
        //        MessageDialog.ShowPromptMessage("数据为空，请重新核查");
        //        return;
        //    }

        //    DataTable dtSafe = (DataTable)dataGridView1.DataSource;

        //    if (!m_safeStockServer.OperationInfo(dtSafe, out m_error))
        //    {
        //        MessageDialog.ShowPromptMessage(m_error);
        //        return;
        //    }
        //    else
        //    {
        //        MessageDialog.ShowPromptMessage("保存成功");
        //    }

        //    RefreshDataGridView();
        //}

        //private void btnSetNumberOfProduct_Click(object sender, EventArgs e)
        //{
        //    if (m_formSetNumberOfProduct == null)
        //        m_formSetNumberOfProduct = new 设置总成数量(null);

        //    m_formSetNumberOfProduct.ShowDialog();
        //}
    }
}
