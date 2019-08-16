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

namespace Expression
{
    public partial class 借贷记录信息 : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strError = "";

        /// <summary>
        /// 借还货记录服务组件
        /// </summary>
        ServerModule.IProductLendReturnService m_serverRecord =
            ServerModule.ServerModuleFactory.GetServerModule<ServerModule.IProductLendReturnService>();

        /// <summary>
        /// 查找条件字段列表
        /// </summary>
        List<string> m_lstFindField = new List<string>();

        /// <summary>
        /// 数据集
        /// </summary>
        DataTable m_dtSource = new DataTable();

        public 借贷记录信息()
        {
            InitializeComponent();
            RefrshData();
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        void RefrshData()
        {
            m_dtSource = m_serverRecord.GetRecordInfo();

            //if (!BasicInfo.ListRoles.Contains(CE_RoleEnum.会计.ToString()))
            //{
            //    string tempCode = "";

            //    foreach (string str in m_serverBasic.GetWorkShopCodeRole())
            //    {
            //        tempCode += "'" + str + "',";
            //    }

            //    if (tempCode.Length > 0)
            //    {
            //        tempCode = tempCode.Substring(0, tempCode.Length - 1);
            //        m_dtSource = GlobalObject.DataSetHelper.SiftDataTable(m_dtSource, "借方代码 in (" + tempCode + ")", out m_strError);
            //    }
            //    else
            //    {
            //        m_dtSource = m_dtSource.Clone();
            //    }
            //}

            DataTable source = checkBox1.Checked ? m_dtSource : DataSetHelper.SiftDataTable(m_dtSource, "数量 > 0", out m_strError);

            DataColumnCollection columns = source.Columns;

            if (columns.Count > 0)
            {
                for (int i = 0; i < columns.Count; i++)
                {
                    m_lstFindField.Add(columns[i].ColumnName);
                }
            }

            dataGridView1.DataSource = source;

            dataGridView1.Columns["借方代码"].Visible = false;
            dataGridView1.Columns["贷方代码"].Visible = false;

            userControlDataLocalizer1.Init(this.dataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, this.dataGridView1.Name, BasicInfo.LoginID));
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefrshData();
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
            txtSpec.Text = dataGridView1.CurrentRow.Cells["规格"].Value.ToString();
            txtDebtor.Text = dataGridView1.CurrentRow.Cells["借方"].Value.ToString();
            txtCredit.Text = dataGridView1.CurrentRow.Cells["贷方"].Value.ToString();
            numStockCount.Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["数量"].Value);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            DataTable source = checkBox1.Checked ? m_dtSource : DataSetHelper.SiftDataTable(m_dtSource, "数量 > 0", out m_strError);

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
