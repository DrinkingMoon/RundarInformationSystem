using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GlobalObject;
using PlatformManagement;
using Service_Manufacture_WorkShop;
using UniversalControlLibrary;
using ServerModule;

namespace Form_Manufacture_WorkShop
{
    public partial class 车间库存 : Form
    {
        /// <summary>
        /// 基础服务组件
        /// </summary>
        IWorkShopBasic m_serverBasic = Service_Manufacture_WorkShop.ServerModuleFactory.GetServerModule<IWorkShopBasic>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strError = "";

        /// <summary>
        /// 车间库房控制服务组件
        /// </summary>
        IWorkShopStock m_serverStock = Service_Manufacture_WorkShop.ServerModuleFactory.GetServerModule<IWorkShopStock>();

        /// <summary>
        /// 查找条件字段列表
        /// </summary>
        List<string> m_lstFindField = new List<string>();

        /// <summary>
        /// 数据集
        /// </summary>
        DataTable m_dtSource = new DataTable();

        public 车间库存()
        {
            InitializeComponent();
            RefrshData();
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        void RefrshData()
        {
            m_dtSource = m_serverStock.GetStockInfo();

            if (!BasicInfo.ListRoles.Contains(CE_RoleEnum.会计.ToString()))
            {
                string tempCode = "";

                foreach (string str in m_serverBasic.GetWorkShopCodeRole())
                {
                    tempCode += "'" + str + "',";
                }

                if (tempCode.Length > 0)
                {
                    tempCode = tempCode.Substring(0, tempCode.Length - 1);
                    m_dtSource = GlobalObject.DataSetHelper.SiftDataTable(m_dtSource, "车间代码 in (" + tempCode + ")", out m_strError);
                }
                else
                {
                    m_dtSource = m_dtSource.Clone();
                }
            }

            DataTable source = checkBox1.Checked ? m_dtSource : DataSetHelper.SiftDataTable(m_dtSource, "库存数量 > 0", out m_strError);

            DataColumnCollection columns = source.Columns;

            if (columns.Count > 0)
            {
                for (int i = 0; i < columns.Count; i++)
                {
                    m_lstFindField.Add(columns[i].ColumnName);
                }
            }

            dataGridView1.DataSource = source;

            userControlDataLocalizer1.Init(this.dataGridView1, this.Name, 
                UniversalFunction.SelectHideFields(this.Name, this.dataGridView1.Name, BasicInfo.LoginID));
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefrshData();
        }

        private void 车间库存_Load(object sender, EventArgs e)
        {
            RefrshData();

            if (BasicInfo.ListRoles.Contains(CE_RoleEnum.会计.ToString()))
            {
                label9.Visible = true;
                numUnitPrice.Visible = true;
            }
            else
            {
                dataGridView1.Columns["单价"].Visible = false;
            }
        }

        private void btnDownLoad_Click(object sender, EventArgs e)
        {
            FormFindCondition formFindCondition = new FormFindCondition(m_lstFindField.ToArray());

            if (formFindCondition.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            DataTable dtTemp = GlobalObject.DataSetHelper.SiftDataTable(m_dtSource,
                formFindCondition.SearchSQL, out m_strError);

            if (dtTemp == null)
            {
                MessageDialog.ShowPromptMessage(m_strError);
            }
            else
            {
                dataGridView1.DataSource = dtTemp;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }

            txtBatchNo.Text = dataGridView1.CurrentRow.Cells["批次号"].Value.ToString();
            txtCode.Text = dataGridView1.CurrentRow.Cells["图号型号"].Value.ToString();
            txtName.Text = dataGridView1.CurrentRow.Cells["物品名称"].Value.ToString();
            txtRemark.Text = dataGridView1.CurrentRow.Cells["备注"].Value.ToString();
            txtSpec.Text = dataGridView1.CurrentRow.Cells["规格"].Value.ToString();
            txtWSCode.Text = dataGridView1.CurrentRow.Cells["车间名称"].Value.ToString();
            numStockCount.Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["库存数量"].Value);
            numUnitPrice.Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["单价"].Value);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            DataTable source = checkBox1.Checked ? m_dtSource : DataSetHelper.SiftDataTable(m_dtSource, "库存数量 > 0", out m_strError);

            DataColumnCollection columns = source.Columns;

            if (columns.Count > 0)
            {
                for (int i = 0; i < columns.Count; i++)
                {
                    m_lstFindField.Add(columns[i].ColumnName);
                }
            }

            dataGridView1.DataSource = source;

            userControlDataLocalizer1.Init(this.dataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, this.dataGridView1.Name, BasicInfo.LoginID));
        }
    }
}
