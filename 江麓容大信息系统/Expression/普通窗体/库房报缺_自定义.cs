using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using PlatformManagement;
using GlobalObject;
using UniversalControlLibrary;

namespace Expression
{
    public partial class 库房报缺_自定义 : Form
    {
        /// <summary>
        /// 库房报缺服务组件
        /// </summary>
        IStockLack m_serverLack = ServerModuleFactory.GetServerModule<IStockLack>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr = "";

        public 库房报缺_自定义()
        {
            InitializeComponent();
            BindingMainInfo();
        }

        void BindingMainInfo()
        {
            dataGridView1.DataSource = m_serverLack.GetCustomTemplatesMain();
            dataGridView1.Columns["ID"].Visible = false;
        }

        void BindingListInfo(int listID)
        {
            dataGridView2.DataSource = m_serverLack.GetCustomTemplatesList(listID);
            dataGridView2.Columns["ID"].Visible = false;
            dataGridView2.Columns["物品ID"].Visible = false;
            dataGridView2.Columns["模板ID"].Visible = false;
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="msg">定位信息</param>
        void PositioningRecord1(string name)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if ((string)dataGridView1.Rows[i].Cells["模板名称"].Value == name)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells["模板名称"];
                }
            }
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="msg">定位信息</param>
        void PositioningRecord2(int goodsID)
        {
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                if ((int)dataGridView2.Rows[i].Cells["物品ID"].Value == goodsID)
                {
                    dataGridView2.FirstDisplayedScrollingRowIndex = i;
                    dataGridView2.CurrentCell = dataGridView2.Rows[i].Cells["物品名称"];
                }
            }
        }

        void MainOperation(CE_OperatorMode mode)
        {
            S_StockLackCustomTemplates tempLnq = new S_StockLackCustomTemplates();

            tempLnq.ID = dataGridView1.CurrentRow == null ? 0 : Convert.ToInt32(dataGridView1.CurrentRow.Cells["ID"].Value);
            tempLnq.TemplatesName = txtTemplatesName.Text;

            if (!m_serverLack.OperationMain(mode, tempLnq, out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
                return;
            }
            else
            {
                MessageDialog.ShowPromptMessage(mode.ToString() + "成功");
            }

            BindingMainInfo();
            PositioningRecord1(tempLnq.TemplatesName);
        }

        void ListOperation(CE_OperatorMode mode)
        {
            S_StockLackCustomTemplatesList tempLnq = new S_StockLackCustomTemplatesList();

            tempLnq.ID = dataGridView2.CurrentRow == null ? 0 : Convert.ToInt32(dataGridView2.CurrentRow.Cells["ID"].Value);
            tempLnq.Counts = numOperationCount.Value;
            tempLnq.GoodsID = Convert.ToInt32(txtCode.Tag);
            tempLnq.ListID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ID"].Value);

            if (!m_serverLack.OperationList(mode, tempLnq, out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
                return;
            }
            else
            {
                MessageDialog.ShowPromptMessage(mode.ToString() + "成功");
            }

            BindingListInfo(tempLnq.ListID);
            PositioningRecord2(tempLnq.GoodsID);
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }

            txtTemplatesName.Text = dataGridView1.CurrentRow.Cells["模板名称"].Value.ToString();

            txtCode.Text = "";
            txtName.Text = "";
            txtSpec.Text = "";
            numOperationCount.Value = 0;
            txtCode.Tag = null;

            BindingListInfo(Convert.ToInt32(dataGridView1.CurrentRow.Cells["ID"].Value));
        }

        private void dataGridView2_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.CurrentRow == null)
            {
                return;
            }

            txtCode.Text = dataGridView2.CurrentRow.Cells["图号型号"].Value.ToString();
            txtCode.Tag = Convert.ToInt32(dataGridView2.CurrentRow.Cells["物品ID"].Value);
            txtName.Text = dataGridView2.CurrentRow.Cells["物品名称"].Value.ToString();
            txtSpec.Text = dataGridView2.CurrentRow.Cells["规格"].Value.ToString();
            numOperationCount.Value = Convert.ToDecimal(dataGridView2.CurrentRow.Cells["基数"].Value);
        }

        private void txtCode_OnCompleteSearch()
        {
            txtCode.Tag = txtCode.DataResult["序号"];
            txtName.Text = txtCode.DataResult["物品名称"].ToString();
            txtSpec.Text = txtCode.DataResult["规格"].ToString();
            txtCode.Text = txtCode.DataResult["图号型号"].ToString();
        }

        private void btn_Add_Main_Click(object sender, EventArgs e)
        {
            MainOperation(CE_OperatorMode.添加);
        }

        private void btn_Modify_Main_Click(object sender, EventArgs e)
        {
            MainOperation(CE_OperatorMode.修改);
        }

        private void btn_Delete_Main_Click(object sender, EventArgs e)
        {
            if (MessageDialog.ShowEnquiryMessage("是否要删除当前选中的记录行?") == DialogResult.Yes)
            {
                MainOperation(CE_OperatorMode.删除);
            }
        }

        private void btn_Add_List_Click(object sender, EventArgs e)
        {
            ListOperation(CE_OperatorMode.添加);
        }

        private void btn_Modify_List_Click(object sender, EventArgs e)
        {
            ListOperation(CE_OperatorMode.修改);
        }

        private void btn_Delete_List_Click(object sender, EventArgs e)
        {
            if (MessageDialog.ShowEnquiryMessage("是否要删除当前选中的记录行?") == DialogResult.Yes)
            {
                ListOperation(CE_OperatorMode.删除);
            }
        }
    }
}
