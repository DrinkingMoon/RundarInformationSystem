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
using UniversalControlLibrary;

namespace Expression
{
    public partial class BOM历史记录 : Form
    {
        /// <summary>
        /// BOM表操作服务组件
        /// </summary>
        IBomServer m_serverBom = ServerModuleFactory.GetServerModule<IBomServer>();

        public BOM历史记录()
        {
            InitializeComponent();

            #region 获取所有产品编码(产品类型)信息
            cmbProductType.DataSource = m_serverBom.GetAssemblyTypeList();
            #endregion

            cmbProductType.SelectedIndex = -1;
            treeView1.Nodes.Clear();
            dataGridView1.DataSource = null;
        }

        private void cmbProductType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbBOMEdition.DataSource = null;

            DataTable dtTemp = m_serverBom.GetBomBackUpBomEdtion(cmbProductType.Text.Trim());

            cmbBOMEdition.ValueMember = "BOM版次号";
            cmbBOMEdition.DisplayMember = "BOM版次号";
            cmbBOMEdition.DataSource = dtTemp;
        }

        private void cmbBOMEdition_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        void RefreshData()
        {
            dataGridView1.DataSource = null;
            treeView1.Nodes.Clear();

            DataTable dtTemp = m_serverBom.GetBomBackUpInfo(cmbProductType.Text, cmbBOMEdition.Text);
            dataGridView1.DataSource = dtTemp;
            userControlDataLocalizer1.Init(dataGridView1, this.Name, null);

            if (dtTemp != null)
            {
                foreach (DataRow dr in dtTemp.Rows)
                {
                    if (dr["父级总成图号型号"].ToString().Trim().Length == 0)
                    {
                        dr["父级总成图号型号"] = "";
                    }
                }

                GlobalObject.GeneralFunction.LoadTreeViewDt(treeView1, dtTemp, "零件名称", "图号型号", "父级总成图号型号", "父级总成图号型号 = ''");

                treeView1.ExpandAll();
            }

        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }
            else
            {
                txtCode.Text = dataGridView1.CurrentRow.Cells["图号型号"].Value.ToString();
                txtName.Text = dataGridView1.CurrentRow.Cells["零件名称"].Value.ToString();
                txtSpec.Text = dataGridView1.CurrentRow.Cells["规格"].Value.ToString();
                txtVersion.Text = dataGridView1.CurrentRow.Cells["零件版次号"].Value.ToString();
                txtRemark.Text = dataGridView1.CurrentRow.Cells["零件版次号"].Value.ToString();
                txtParentCode.Text = dataGridView1.CurrentRow.Cells["父级总成图号型号"].Value.ToString();
                numBasicCount.Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["基数"].Value);
                cmbAssemblyFlag.Text = dataGridView1.CurrentRow.Cells["总成标志"].Value.ToString();
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
    }
}
