using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using GlobalObject;
using PlatformManagement;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 新旧箱号匹配录入界面
    /// </summary>
    public partial class 新旧箱号匹配录入 : Form
    {
        /// <summary>
        /// 产品信息服务组件
        /// </summary>
        IBomServer m_bomService = ServerModuleFactory.GetServerModule<IBomServer>();

        /// <summary>
        /// 服务组件
        /// </summary>
        IProductInfoServer m_productInfoServer = ServerModuleFactory.GetServerModule<IProductInfoServer>();

        /// <summary>
        /// 服务组件
        /// </summary>
        ISellIn m_sell = ServerModuleFactory.GetServerModule<ISellIn>();

        /// <summary>
        /// 数据集
        /// </summary>
        S_NewAndOldProductCodeMatching m_lnqNewAndOld = new S_NewAndOldProductCodeMatching();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 数据组
        /// </summary>
        DataRow m_dr;

        public 新旧箱号匹配录入()
        {
            InitializeComponent();

            dataGridView1.DataSource = m_sell.GetProductCodeMatchingInfo();
            dataGridView1.Columns["序号"].Visible = false;

            List<string> lstProduct = m_bomService.GetAssemblyTypeList();
            cmbNewEdition.DataSource = lstProduct;
            cmbOldEdition.DataSource = lstProduct;
            cmbNewEdition.SelectedIndex = -1;
            cmbOldEdition.SelectedIndex = -1;
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="billNo">定位用的单据号</param>
        void PositioningRecord(DataRow drinsert)
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
                if (dataGridView1.Rows[i].Cells["旧箱型号"].Value.ToString() == drinsert["旧箱型号"].ToString()
                    && dataGridView1.Rows[i].Cells["旧箱号"].Value.ToString() == drinsert["旧箱号"].ToString()
                    && dataGridView1.Rows[i].Cells["新箱型号"].Value.ToString() == drinsert["新箱型号"].ToString()
                    && dataGridView1.Rows[i].Cells["新箱号"].Value.ToString() == drinsert["新箱号"].ToString())
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                    break;
                }
            }
        }

        /// <summary>
        /// 创建新的数据行
        /// </summary>
        void CreateDataRow()
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;

            m_dr = dt.NewRow();

            m_dr["旧箱型号"] = cmbOldEdition.Text;
            m_dr["旧箱号"] = txtOldProductCode.Text;
            m_dr["新箱型号"] = cmbNewEdition.Text;
            m_dr["新箱号"] = txtNewProductCode.Text;
            m_dr["返修批次号"] = txtBatchNo.Text;
            m_dr["备注"] = txtRemark.Text;

        }

        /// <summary>
        /// 检查记录
        /// </summary>
        /// <returns>检测通过返回True，否则返回False</returns>
        bool CheckDate()
        {
            if (txtBatchNo.Text.ToString() == "")
            {
                MessageDialog.ShowPromptMessage("请输入返修批次号");
                return false;
            }

            if (txtNewProductCode.Text.ToString() == "")
            {
                MessageDialog.ShowPromptMessage("请输入新箱号");
                return false;
            }

            if (txtOldProductCode.Text.ToString() == "")
            {
                MessageDialog.ShowPromptMessage("请输入旧箱号");
                return false;
            }

            if (cmbNewEdition.Text.ToString() == "")
            {
                MessageDialog.ShowPromptMessage("请选择新箱型号");
                return false;
            }

            if (cmbOldEdition.Text.ToString() == "")
            {
                MessageDialog.ShowPromptMessage("请选择旧箱型号");
                return false;
            }


            DataTable dt = (DataTable)dataGridView1.DataSource;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["新箱型号"].ToString() == cmbNewEdition.Text
                    && dt.Rows[i]["新箱号"].ToString() == txtNewProductCode.Text
                    && dt.Rows[i]["旧箱型号"].ToString() == cmbOldEdition.Text
                    && dt.Rows[i]["旧箱号"].ToString() == txtOldProductCode.Text)
                {
                    MessageDialog.ShowPromptMessage("不能输入相同记录，请重新核查");
                    return false;
                }

            }

            return true;
        }

        /// <summary>
        /// 获得数据集
        /// </summary>
        void GetMessage()
        {
            m_lnqNewAndOld.BatchNo = txtBatchNo.Text;
            m_lnqNewAndOld.NewEdition = cmbNewEdition.Text;
            m_lnqNewAndOld.NewProductCode = txtNewProductCode.Text;
            m_lnqNewAndOld.OldEdition = cmbOldEdition.Text;
            m_lnqNewAndOld.OldProductCode = txtOldProductCode.Text;
            m_lnqNewAndOld.Remark = txtRemark.Text;
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                return;
            }

            txtBatchNo.Text = dataGridView1.CurrentRow.Cells["返修批次号"].Value.ToString();
            txtNewProductCode.Text = dataGridView1.CurrentRow.Cells["新箱号"].Value.ToString();
            txtOldProductCode.Text = dataGridView1.CurrentRow.Cells["旧箱号"].Value.ToString();
            txtRemark.Text = dataGridView1.CurrentRow.Cells["备注"].Value.ToString();
            cmbNewEdition.Text = dataGridView1.CurrentRow.Cells["新箱型号"].Value.ToString();
            cmbOldEdition.Text = dataGridView1.CurrentRow.Cells["旧箱型号"].Value.ToString();

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (CheckDate())
            {
                GetMessage();
                CreateDataRow();

                if (!m_sell.AddMatchingInfo(m_lnqNewAndOld,out m_err))
                {
                    MessageDialog.ShowPromptMessage(m_err);
                }
                else
                {
                    MessageDialog.ShowPromptMessage("添加成功!");
                }

                dataGridView1.DataSource = m_sell.GetProductCodeMatchingInfo();
                dataGridView1.Columns["序号"].Visible = false;
                PositioningRecord(m_dr);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (CheckDate())
            {
                GetMessage();
                CreateDataRow();

                if (!m_sell.UpdateMatchingInfo(m_lnqNewAndOld,
                    Convert.ToInt32(dataGridView1.CurrentRow.Cells["序号"].Value), out m_err))
                {
                    MessageDialog.ShowPromptMessage(m_err);
                }
                else
                {
                    MessageDialog.ShowPromptMessage("修改成功!");
                }

                dataGridView1.DataSource = m_sell.GetProductCodeMatchingInfo();
                dataGridView1.Columns["序号"].Visible = false;
                PositioningRecord(m_dr);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in dataGridView1.SelectedRows)
            {
                if (!m_sell.DeleteMatchingInfo(
                    Convert.ToInt32(item.Cells["序号"].Value), out m_err))
                {
                    MessageDialog.ShowPromptMessage(m_err);
                }
            }

            MessageDialog.ShowPromptMessage("删除成功!");

            dataGridView1.DataSource = m_sell.GetProductCodeMatchingInfo();
            dataGridView1.Columns["序号"].Visible = false;
        }

        private void btnOutExcel_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
        }

        private void btnInExcel_Click(object sender, EventArgs e)
        {
            DataTable dtTemp = ExcelHelperP.RenderFromExcel(openFileDialog1);

            if (dtTemp == null)
            {
                //MessageDialog.ShowPromptMessage(m_err);
                return;
            }

            if (dtTemp.Rows.Count == 0 ||
                !dtTemp.Columns.Contains("新箱号") ||
                !dtTemp.Columns.Contains("旧箱号") ||
                !dtTemp.Columns.Contains("新箱型号") ||
                !dtTemp.Columns.Contains("旧箱型号") ||
                !dtTemp.Columns.Contains("备注"))
            {
                MessageDialog.ShowPromptMessage(string.Format("{0} 中没有包含所需的信息，无法导入！",
                    openFileDialog1.FileName));
            }
            else
            {
                string strBatchNo = m_sell.GetMatchingBillID();

                for (int i = 0; i < dtTemp.Rows.Count; i++)
                {
                    m_lnqNewAndOld.BatchNo = strBatchNo;
                    m_lnqNewAndOld.NewProductCode = dtTemp.Rows[i]["新箱号"].ToString();
                    m_lnqNewAndOld.OldProductCode = dtTemp.Rows[i]["旧箱号"].ToString();
                    m_lnqNewAndOld.NewEdition = dtTemp.Rows[i]["新箱型号"].ToString();
                    m_lnqNewAndOld.OldEdition = dtTemp.Rows[i]["旧箱型号"].ToString();
                    m_lnqNewAndOld.Remark = dtTemp.Rows[i]["备注"].ToString();

                    if (m_sell.IsSameProductMatchingInfo(m_lnqNewAndOld))
                    {
                        if (!m_sell.AddMatchingInfo(m_lnqNewAndOld, out m_err))
                        {
                            MessageDialog.ShowPromptMessage(m_err);
                        }
                    }
                }

                MessageDialog.ShowPromptMessage("导入成功!");
            }

            dataGridView1.DataSource = m_sell.GetProductCodeMatchingInfo();
            dataGridView1.Columns["序号"].Visible = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCreateNewBatchNo_Click(object sender, EventArgs e)
        {
            txtBatchNo.Text = m_sell.GetMatchingBillID();
        }

    }
}
