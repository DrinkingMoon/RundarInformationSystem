using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using Expression;
using Service_Peripheral_External;
using UniversalControlLibrary;

namespace Form_Peripheral_External
{
    public partial class 二级库房设置 : Form
    {
        /// <summary>
        /// 数据集
        /// </summary>
        Out_StockInfo m_lnqStockInfo = new Out_StockInfo();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr = "";

        /// <summary>
        /// 二级库房服务组件
        /// </summary>
        ISecondStorageInfo m_serverStorageInfo = Service_Peripheral_External.ServerModuleFactory.GetServerModule<ISecondStorageInfo>();

        public 二级库房设置()
        {
            InitializeComponent();
            DataRefresh();
        }

        private void 二级库房设置_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 刷新
        /// </summary>
        void DataRefresh()
        {
            dataGridView1.DataSource = m_serverStorageInfo.GetSecondStorageInfo();
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="secStorageID">定位用的信息</param>
        void PositioningRecord(string secStorageID)
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
                if ((string)dataGridView1.Rows[i].Cells["库房编码"].Value == secStorageID)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                    break;
                }
            }
        }

        /// <summary>
        /// 获得数据
        /// </summary>
        void GetMessage()
        {
            m_lnqStockInfo.Remark = txtRemark.Text;
            m_lnqStockInfo.SecStorageID = txtStorageID.Text;
            m_lnqStockInfo.SecStorageName = txtStorageName.Text;
        }

        private void btAddStorage_Click(object sender, EventArgs e)
        {
            GetMessage();

            if (!m_serverStorageInfo.InsertInfo(m_lnqStockInfo,out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
            }
            else
            {
                MessageDialog.ShowPromptMessage("添加成功");
            }

            DataRefresh();
            PositioningRecord(m_lnqStockInfo.SecStorageID);
        }

        private void btDeleteStorage_Click(object sender, EventArgs e)
        {
            if (!m_serverStorageInfo.DeleteInfo(dataGridView1.CurrentRow.Cells["库房编码"].Value.ToString(),out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);

            }
            else
            {
                MessageDialog.ShowPromptMessage("删除成功");
            }

            DataRefresh();
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }
            else
            {
                txtRemark.Text = dataGridView1.CurrentRow.Cells["备注"].Value.ToString();
                txtStorageID.Text = dataGridView1.CurrentRow.Cells["库房编码"].Value.ToString();
                txtStorageName.Text = dataGridView1.CurrentRow.Cells["库房名称"].Value.ToString();
            }
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            GetMessage();

            if (!m_serverStorageInfo.ModifyInfo(dataGridView1.CurrentRow.Cells["库房编码"].Value.ToString(), 
                m_lnqStockInfo, out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
            }
            else
            {
                MessageDialog.ShowPromptMessage("修改成功");
            }

            DataRefresh();
            PositioningRecord(m_lnqStockInfo.SecStorageID);
        }
    }
}
