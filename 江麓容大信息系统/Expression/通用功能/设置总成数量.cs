using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;

namespace Expression
{
    public partial class 设置总成数量 : Form
    {
        /// <summary>
        /// DataGridViewRow
        /// </summary>
        DataGridViewRow m_dgvr = new DataGridViewRow();

        /// <summary>
        /// 产品信息服务接口
        /// </summary>
        IProductListServer m_productListServer = ServerModuleFactory.GetServerModule<IProductListServer>();

        /// <summary>
        /// 产品型号与数量构成的字典
        /// </summary>
        Dictionary<string, int> m_dicNumber = new Dictionary<string,int>();

        /// <summary>
        /// 产品型号与数量构成的字典
        /// </summary>
        public Dictionary<string, int> DicNumberOfProduct
        {
          get { return m_dicNumber; }
          set { m_dicNumber = value; }
        }

        public 设置总成数量(DataGridViewRow dgvr)
        {
            InitializeComponent();

            m_dgvr = dgvr;
        }

        private void 设置总成数量_Load(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                // 单击进入编辑状态
                this.dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
                this.dataGridView1.AutoGenerateColumns = false;
                this.dataGridView1.ReadOnly = false;

                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    if (dataGridView1.Columns[i].Name != "数量")
                        this.dataGridView1.Columns[i].ReadOnly = true;
                }

                dataGridView1.Columns["数量"].ValueType = typeof(int);

                DataTable dt = m_productListServer.GetProductInfo();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (m_dgvr == null || m_dgvr.DataGridView == null)
                    {
                        dataGridView1.Rows.Add(new object[] 
                        { 
                            dt.Rows[i]["产品编码"].ToString(), 
                            dt.Rows[i]["产品名称"].ToString(), 
                            0 
                        });
                    }
                    else
                    {
                        dataGridView1.Rows.Add(new object[] 
                        { 
                            dt.Rows[i]["产品编码"].ToString(), 
                            dt.Rows[i]["产品名称"].ToString(), 
                            Convert.ToDecimal(m_dgvr.Cells[dt.Rows[i]["产品编码"].ToString()].Value) 
                        });
                    }

                }
            }
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].ValueType == typeof(int))
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// 用于确定数据控件中的值已经更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView1.IsCurrentCellDirty)
            {
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
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

        private void btnOK_Click(object sender, EventArgs e)
        {
            m_dicNumber = new Dictionary<string, int>();

            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                m_dicNumber.Add(dr.Cells[0].Value.ToString(), Convert.ToInt32(dr.Cells[2].Value));
            }

            this.Close();
        }
    }
}
