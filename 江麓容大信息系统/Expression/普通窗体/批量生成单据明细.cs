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
using System.Collections;
using PlatformManagement;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 批量生成单据明细界面
    /// </summary>
    public partial class 批量生成单据明细 : Form
    {
        /// <summary>
        /// 领料退库服务组件
        /// </summary>
        IMaterialListReturnedInTheDepot m_serverMaterialReturnList = 
            ServerModuleFactory.GetServerModule<IMaterialListReturnedInTheDepot>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr;

        /// <summary>
        /// 单据号
        /// </summary>
        string m_strBillID;

        /// <summary>
        /// 生成后的TABLE
        /// </summary>
        private DataTable m_dtBatchCreate = new DataTable();

        public DataTable DtBatchCreate
        {
            get { return m_dtBatchCreate; }
            set { m_dtBatchCreate = value; }
        }

        /// <summary>
        /// 显示单据类型 （领料，退库）
        /// </summary>
        string m_strSelectType = "";

        public 批量生成单据明细(string strselecttype,string strbillid)
        {
            InitializeComponent();

            m_strSelectType = strselecttype;

            m_strBillID = strbillid;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource =
                m_serverMaterialReturnList.GetBatchCreatList(m_strSelectType, dtpStart.Value, dtpEnd.Value);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                return;
            }
            else
            {
                dataGridView1.CurrentRow.Cells["选"].Value = !Convert.ToBoolean(dataGridView1.CurrentRow.Cells["选"].Value);
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;

            string strBillIDGather = "";

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (Convert.ToBoolean( dt.Rows[i]["选"]))
                {
                    strBillIDGather += "'" + dt.Rows[i][m_strSelectType + "单号"].ToString() + "',";
                }
            }

            strBillIDGather = "(" + strBillIDGather.Remove(strBillIDGather.Length - 1) + ")";


            if (!m_serverMaterialReturnList.BatchCreateList(m_strSelectType, m_strBillID, strBillIDGather, out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
            }
            else
            {
                MessageDialog.ShowPromptMessage("生成成功");
                this.Close();
            }
        }
    }
}
