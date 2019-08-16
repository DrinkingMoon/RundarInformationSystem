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
    public partial class 物流安全库存 : Form
    {
        /// <summary>
        /// 物流安全库存服务组件
        /// </summary>
        ILogisticSafeStock m_serverLogistic = ServerModuleFactory.GetServerModule<ILogisticSafeStock>();

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authorityFlag;

        /// <summary>
        /// LINQ数据集
        /// </summary>
        S_LogisticSafeStock m_lnqLogistc = new S_LogisticSafeStock();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr;

        public 物流安全库存(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();
            m_authorityFlag = nodeInfo.Authority;
            RefreshData();
        }

        private void tbsGoods_OnCompleteSearch()
        {
            tbsGoods.Text = tbsGoods.DataResult["物品名称"].ToString();
            tbsGoods.Tag = Convert.ToInt32(tbsGoods.DataResult["序号"]);
            txtGoodsCode.Text = tbsGoods.DataResult["图号型号"].ToString();
            txtSpce.Text = tbsGoods.DataResult["规格"].ToString();
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        void RefreshData()
        {
            dataGridView1.DataSource = m_serverLogistic.GetInfo();

            userControlDataLocalizer1.Init(this.dataGridView1, this.Name, 
                UniversalFunction.SelectHideFields(this.Name, this.dataGridView1.Name, BasicInfo.LoginID));
        }

        /// <summary>
        /// 获得信息
        /// </summary>
        void GetMessage()
        {
            m_lnqLogistc = new S_LogisticSafeStock();

            m_lnqLogistc.GoodsID = Convert.ToInt32(tbsGoods.Tag);
            m_lnqLogistc.MaxValue = numMaxValue.Value;
            m_lnqLogistc.MinValue = numMinValue.Value;
            m_lnqLogistc.Remark = txtRemark.Text;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            GetMessage();

            if (!m_serverLogistic.AddInfo(m_lnqLogistc,out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
            }
            else
            {
                MessageDialog.ShowPromptMessage("添加成功");
            }

            RefreshData();
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            GetMessage();

            if (!m_serverLogistic.UpdateInfo(Convert.ToInt32(dataGridView1.CurrentRow.Cells["物品ID"].Value), m_lnqLogistc, out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
            }
            else
            {
                MessageDialog.ShowPromptMessage("更新成功");
            }

            RefreshData();
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }
            else
            {
                tbsGoods.Text = dataGridView1.CurrentRow.Cells["物品名称"].Value.ToString();
                tbsGoods.Tag = Convert.ToInt32(dataGridView1.CurrentRow.Cells["物品ID"].Value);
                txtGoodsCode.Text = dataGridView1.CurrentRow.Cells["图号型号"].Value.ToString();
                txtSpce.Text = dataGridView1.CurrentRow.Cells["规格"].Value.ToString();
                numMaxValue.Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["上限"].Value);
                numMinValue.Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["下限"].Value);
                txtRemark.Text = dataGridView1.CurrentRow.Cells["备注"].Value.ToString();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            GetMessage();

            if (!m_serverLogistic.DeleteInfo(Convert.ToInt32(dataGridView1.CurrentRow.Cells["物品ID"].Value), out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
            }
            else
            {
                MessageDialog.ShowPromptMessage("删除成功");
            }

            RefreshData();
        }

        private void btnOutExcel_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
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

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewRow dr = dataGridView1.Rows[i];

                if (Convert.ToDecimal(dataGridView1.Rows[i].Cells["下限"].Value) != 0 
                    && Convert.ToDecimal(dataGridView1.Rows[i].Cells["下限"].Value) >
                    Convert.ToDecimal(dataGridView1.Rows[i].Cells["实际正常库存"].Value))
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                }

                if (Convert.ToDecimal(dataGridView1.Rows[i].Cells["上限"].Value) != 0 
                    && Convert.ToDecimal(dataGridView1.Rows[i].Cells["上限"].Value) <
                    Convert.ToDecimal(dataGridView1.Rows[i].Cells["实际正常库存"].Value))
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Green;
                }
            }
        }

        private void 物流安全库存_Load(object sender, EventArgs e)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip1, m_authorityFlag);
        }

        /// <summary>
        /// 数据过滤
        /// </summary>
        void DataFiltering()
        {
            RefreshData();

            DataTable dtTemp = (DataTable)dataGridView1.DataSource;

            DataRow[] drList = null;

            if (rbLessMin.Checked)
            {
                drList = dtTemp.Select(" 下限 <> 0 and 下限 > 实际正常库存");
            }

            if (rbMoreMax.Checked)
            {
                drList = dtTemp.Select(" 上限 <> 0 and 上限 < 实际正常库存");
            }

            if (rbNomarl.Checked)
            {
                drList = dtTemp.Select(" (下限 <> 0 and 下限 < 实际正常库存) or (上限 <> 0 and 上限 > 实际正常库存) or  (实际正常库存 > 下限 and 实际正常库存 < 上限)");
            }

            if (drList != null)
            {
                DataTable dtTemp1 = dtTemp.Clone();

                for (int i = 0; i < drList.Length; i++)
                {
                    dtTemp1.ImportRow(drList[i]);
                }

                dataGridView1.DataSource = dtTemp1;
            }
        }

        private void rbAll_CheckedChanged(object sender, EventArgs e)
        {
            DataFiltering();
        }

        private void rbNomarl_CheckedChanged(object sender, EventArgs e)
        {
            DataFiltering();
        }

        private void rbMoreMax_CheckedChanged(object sender, EventArgs e)
        {
            DataFiltering();
        }

        private void rbLessMin_CheckedChanged(object sender, EventArgs e)
        {
            DataFiltering();
        }
    }
}
