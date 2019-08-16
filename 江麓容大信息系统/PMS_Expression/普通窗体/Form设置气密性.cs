using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using UniversalControlLibrary;

namespace Expression
{
    public partial class Form设置气密性 : Form
    {
        /// <summary>
        /// 产品名称
        /// </summary>
        string m_productName;

        /// <summary>
        /// 产品类型编码
        /// </summary>
        string m_productTypeCode;

        /// <summary>
        /// 要设置气密性的产品编号列表
        /// </summary>
        private List<string> m_lstProductNumber = new List<string>();

        /// <summary>
        /// 电子档案服务组件
        /// </summary>
        IElectronFileServer m_electronFilesServer = ServerModuleFactory.GetServerModule<IElectronFileServer>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_error;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="productTypeCode">产品类型编号</param>
        /// <param name="productName">产品名称</param>
        /// <param name="lstProductNumber">要设置气密性的产品编号列表</param>
        public Form设置气密性(string productTypeCode, string productName, List<string> lstProductNumber)
        {
            InitializeComponent();

            if (lstProductNumber == null || lstProductNumber.Count == 0)
            {
                throw new Exception("设置气密性时产品编号不能为空");
            }

            m_productTypeCode = productTypeCode;
            m_productName = productName;
            m_lstProductNumber = lstProductNumber;

            // 单击进入编辑状态
            this.dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
            this.dataGridView1.AutoGenerateColumns = false;

            dataGridView1.Columns["气密性"].ValueType = typeof(decimal);

            foreach (var number in lstProductNumber)
            {
                dataGridView1.Rows.Add(new object[] { number, 0.0m });
            }            
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].ValueType == typeof(decimal))
            {
                e.Cancel = true;
            }
            else if (dataGridView1.Columns[e.ColumnIndex].ValueType == typeof(int))
            {
                e.Cancel = true;
            }
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridViewColumnCollection columns = this.dataGridView1.Columns;

            switch (columns[this.dataGridView1.CurrentCell.ColumnIndex].Name)
            {
                case "气密性":

                    if (e.Control is TextBox)
                    {
                        TextBox tb = e.Control as TextBox;
                        tb.KeyPress -= new KeyPressEventHandler(tb_KeyPress);
                        tb.KeyPress += new KeyPressEventHandler(tb_KeyPress);
                    }

                    break;
            }
        }

        void tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            DataGridViewColumnCollection columns = this.dataGridView1.Columns;
            string name = columns[this.dataGridView1.CurrentCell.ColumnIndex].Name;

            if (name != "气密性")
            {
                return;
            }

            if (!(char.IsDigit(e.KeyChar)))
            {
                Keys key = (Keys)e.KeyChar;

                if (!(key == Keys.Back || key == Keys.Delete))
                {
                    e.Handled = true;
                }
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("没有要保存的数据");
                return;
            }

            dataGridView1.CurrentCell = null;

            Dictionary<string, decimal> dicAirImpermeability = new Dictionary<string, decimal>();

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[0].Value == null)
                {
                    continue;
                }

                dicAirImpermeability.Add(dataGridView1.Rows[i].Cells[0].Value.ToString(), (decimal)dataGridView1.Rows[i].Cells[1].Value);
            }

            if (m_electronFilesServer.SaveAirImpermeability(m_productTypeCode, m_productName, dicAirImpermeability, "Z19", "快速返修", out m_error))
            {
                MessageDialog.ShowPromptMessage("成功保存数据");
                this.Close();
            }
            else
            {
                MessageDialog.ShowErrorMessage(m_error);
            }
        }
    }
}
