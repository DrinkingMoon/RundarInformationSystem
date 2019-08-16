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
using UniversalControlLibrary;

namespace Expression
{
    public partial class 批量移交变速箱 : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_error;

        /// <summary>
        /// 移交时间
        /// </summary>
        DateTime m_dt = ServerTime.Time;

        /// <summary>
        /// 电子档案数据服务
        /// </summary>
        IElectronFileServer m_electronFileServer = ServerModuleFactory.GetServerModule<IElectronFileServer>();

        /// <summary>
        /// 产品编号服务
        /// </summary>
        private IProductCodeServer m_productCodeServer = ServerModuleFactory.GetServerModule<IProductCodeServer>();

        public 批量移交变速箱()
        {
            InitializeComponent();
        }

        private void 批量移交变速箱_Load(object sender, EventArgs e)
        {
            string[] productType = null;

            if (!ServerModuleFactory.GetServerModule<IProductInfoServer>().GetAllProductType(out productType, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);

                this.Close();
                return;
            }

            List<string> lstProductType = productType.ToList();

            lstProductType.RemoveAll(p => p.Contains(" FX"));

            cmbCVTType.Items.AddRange(lstProductType.ToArray());
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (cmbCVTType.Text == "")
            {
                MessageDialog.ShowPromptMessage("请选择产品型号后再进行此操作");
                return;
            }

            if (txtCVTNumber.Text == "")
            {
                MessageDialog.ShowPromptMessage("请录入箱号后再进行此操作");
                return;
            }

            txtCVTNumber.Text = txtCVTNumber.Text.Trim().ToUpper();

            if (!m_productCodeServer.VerifyProductCodesInfo(
                cmbCVTType.Text, txtCVTNumber.Text, GlobalObject.CE_BarCodeType.内部钢印码, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                txtCVTNumber.Focus();
                return;
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[0].Value.ToString() == cmbCVTType.Text && 
                    dataGridView1.Rows[i].Cells[1].Value.ToString() == txtCVTNumber.Text)
                {
                    MessageDialog.ShowPromptMessage(string.Format("数据显示控件中已经存在 {0} {1} 的记录,不允许重复添加", cmbCVTType.Text, txtCVTNumber.Text));
                    return;
                }
            }

            dataGridView1.Rows.Add(new object[] { cmbCVTType.Text, txtCVTNumber.Text, BasicInfo.LoginID, m_dt, "装配车间" });
        }

        /// <summary>
        /// 批量提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("没有数据需要提交");
                return;
            }

            List<string> lstProductNumber = new List<string>();

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                lstProductNumber.Add(dataGridView1.Rows[i].Cells[0].Value.ToString() + " " 
                    + dataGridView1.Rows[i].Cells[1].Value.ToString());
            }

            if (!m_electronFileServer.SaveCVTHandoverInfo(lstProductNumber, "装配车间", out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
            }
            else
            {
                MessageDialog.ShowPromptMessage("操作成功");

                cmbCVTType.SelectedIndex = -1;
                cmbCVTType.Focus();

                txtCVTNumber.Text = "";
                dataGridView1.Rows.Clear();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选中至少一条记录后再进行此操作");
                return;
            }

            for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
            {
                dataGridView1.Rows.Remove(dataGridView1.SelectedRows[i--]);
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

        /// <summary>
        /// 从营销单据中导入箱号信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImportBill_Click(object sender, EventArgs e)
        {
            string billNo = InputBox.ShowDialog("营销单据号", "请输入营销单据号", "YXTH");

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(billNo))
            {
                MessageDialog.ShowPromptMessage("请输入营销单据号后再进行此操作");
                return;
            }

            DataTable dt = m_electronFileServer.GetProductNumberFromSellBill(billNo);

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int index = 0; index < dt.Rows.Count; index++)
                {
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        if (dataGridView1.Rows[i].Cells[0].Value.ToString() == dt.Rows[index][0].ToString() &&
                            dataGridView1.Rows[i].Cells[1].Value.ToString() == dt.Rows[index][1].ToString())
                        {
                            MessageDialog.ShowPromptMessage(string.Format("数据显示控件中已经存在 {0} {1} 的记录,不允许重复添加",
                                dt.Rows[index][0].ToString(), dt.Rows[index][1].ToString()));

                            return;
                        }
                    }

                    dataGridView1.Rows.Add(new object[] { 
                        dt.Rows[index][0].ToString(), dt.Rows[index][1].ToString(), BasicInfo.LoginID, m_dt, "装配车间" });

                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("没有找到此单据的信息, 请检查单据号是否正确");
            }
        }
    }
}
