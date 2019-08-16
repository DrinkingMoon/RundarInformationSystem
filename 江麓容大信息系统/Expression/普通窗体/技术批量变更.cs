using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Expression
{
    /// <summary>
    /// 技术变更单批量变更界面
    /// </summary>
    public partial class 技术批量变更 : Form
    {
        /// <summary>
        /// 数据集
        /// </summary>
        private DataTable m_dtOut;

        public DataTable DtOut
        {
            get { return m_dtOut; }
            set { m_dtOut = value; }
        }

        public 技术批量变更()
        {
            InitializeComponent();

            RefreshDataGrid();

        }

        /// <summary>
        /// 刷新数据集
        /// </summary>
        void RefreshDataGrid()
        {
            string strSql = " select  0 as 选,GoodsCode as 产品类型编码, GoodsName 产品类型名称 , Spec as 规格 "+
                            " from F_GoodsAttributeRecord as a inner join F_GoodsPlanCost as b on a.GoodsID = b.ID "+
                            " where a.AttributeID in (" + (int)GlobalObject.CE_GoodsAttributeName.CVT + "," 
                            + (int)GlobalObject.CE_GoodsAttributeName.TCU + ") and AttributeValue = '"+ bool.TrueString +"'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            dataGridView1.DataSource = dt;
        }

        private void 技术批量变更_FormClosing(object sender, FormClosingEventArgs e)
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;

            m_dtOut = dt.Clone();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["选"].ToString() != "0")
                {
                    DataRow drNew = m_dtOut.NewRow();
                    drNew["产品类型编码"] = dt.Rows[i]["产品类型编码"].ToString();
                    m_dtOut.Rows.Add(drNew);
                }
            }
        }
    }
}
