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
using PlatformManagement;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 发票管理界面
    /// </summary>
    public partial class 发票管理 : Form
    {
        /// <summary>
        /// 初始化供应商查询窗体
        /// </summary>
        FormQueryInfo m_formProvider;

        /// <summary>
        /// 发票组件
        /// </summary>
        IInvoiceServer m_findVoice = ServerModuleFactory.GetServerModule<IInvoiceServer>();

        /// <summary>
        /// 发票类型
        /// </summary>
        int m_intInvoiceType = 0;

        /// <summary>
        /// 权限组件
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 供应商服务组件
        /// </summary>
        IProviderServer m_serverProvider = ServerModuleFactory.GetServerModule<IProviderServer>();

        public 发票管理(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authFlag = nodeInfo.Authority;
        }

        private void 发票管理_Load(object sender, EventArgs e)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip1, m_authFlag);
            FaceAuthoritySetting.SetEnable(this.Controls, m_authFlag);

            DateTime today = ServerTime.Time;

            dtp_Start.Value = new DateTime(today.Year, today.Month, 1);
            dtp_End.Value = today.AddDays(1);
            cmbInvoiceType.AllowDrop = false;

            dgv_Main.DataSource = GetProviderName(m_findVoice.GetInvoiceInfo(dtp_Start.Value, dtp_End.Value));
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="billNo">定位用的单据号</param>
        void PositioningRecord(string billNo)
        {
            string strColName = "";

            foreach (DataGridViewColumn col in dgv_Main.Columns)
            {
                if (col.Visible)
                {
                    strColName = col.Name;
                    break;
                }
            }

            for (int i = 0; i < dgv_Main.Rows.Count; i++)
            {
                if ((string)dgv_Main.Rows[i].Cells["PZH"].Value == billNo)
                {
                    dgv_Main.FirstDisplayedScrollingRowIndex = i;
                    dgv_Main.CurrentCell = dgv_Main.Rows[i].Cells[strColName];
                    break;
                }
            }
        }

        void txtProvider_OnCompleteSearch()
        {
            txtProvider.Text = txtProvider.DataResult["供应商名称"].ToString();
            txtProvider.Tag = txtProvider.DataResult["供应商编码"].ToString();
        }

        private void dgv_Main_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataTable dtMx = new DataTable();

            if (dgv_Main.CurrentRow.Cells["InvoiceCode"].Value.ToString() == "")
            {
                return;
            }

            dtMx = m_findVoice.GetInvoiceInfo(dgv_Main.CurrentRow.Cells["InvoiceCode"].Value.ToString());
            dgv_Mx.DataSource = dtMx;
            txtInvoice.Text = dgv_Main.CurrentRow.Cells["InvoiceCode"].Value.ToString();
            txtProvider.Text = dgv_Main.CurrentRow.Cells["ProviderName"].Value.ToString();
            txtProvider.Tag = dgv_Main.CurrentRow.Cells["供应商"].Value.ToString();
            cmbInvoiceType.Text = dgv_Main.CurrentRow.Cells["InvoiceType"].Value.ToString();
            txtPZH.Text = dgv_Main.CurrentRow.Cells["PZH"].Value.ToString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (CheckDate())
            {
                if (m_findVoice.GetInvoiceInfo(txtInvoice.Text.Trim()).Rows.Count != 0)
                {
                    MessageBox.Show("录入的发票号重复，请重新录入", "提示");
                    return;
                }

                发票明细清单 FrmFp = new 发票明细清单(txtInvoice.Text.Trim(), txtProvider.Tag.ToString(), 
                                                      txtProvider.Text, m_intInvoiceType, txtPZH.Text, m_authFlag);
                FrmFp.ShowDialog();

                dgv_Main.DataSource = GetProviderName(m_findVoice.GetInvoiceInfo(dtp_Start.Value, dtp_End.Value));
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string strDateNy = ServerTime.GetMonthlyString(Convert.ToDateTime(dgv_Main.CurrentRow.Cells["Date"].Value));

                string strNowNy = ServerTime.GetMonthlyString(ServerTime.Time);

                if (strDateNy != strNowNy)
                {
                    MessageDialog.ShowPromptMessage("不能跨月删除发票");
                    return;
                }

                if (MessageBox.Show("您是否确定要删除发票号为【"+dgv_Main.CurrentRow.Cells["InvoiceCode"].Value.ToString()
                    +"】的发票?", "消息", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (m_findVoice.DeleteInvoiceInfo(dgv_Main.CurrentRow.Cells["InvoiceCode"].Value.ToString(), out m_err))
                    {
                        MessageBox.Show("删除成功!", "提示");
                    }
                    else
                    {
                        MessageBox.Show("删除失败 " + m_err, "提示");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            dgv_Main.DataSource = GetProviderName(m_findVoice.GetInvoiceInfo(dtp_Start.Value, dtp_End.Value));
            dgv_Mx.DataSource = null;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (CheckDate())
            {
                if (m_findVoice.GetInvoiceInfo(txtInvoice.Text.Trim()).Rows.Count != 0 
                    && txtInvoice.Text != dgv_Main.CurrentRow.Cells["InvoiceCode"].Value.ToString())
                {
                    MessageBox.Show("录入的发票号重复，请重新录入", "提示");
                    return;
                }

                if (m_findVoice.UpdateInvoiceInfo(txtInvoice.Text.Trim(), txtProvider.Tag.ToString(), m_intInvoiceType, 
                    dgv_Main.CurrentRow.Cells["InvoiceCode"].Value.ToString(), txtPZH.Text, out m_err))
                {
                    //发票明细清单 FrmFp = new 发票明细清单(txtInvoice.Text.Trim(), txtProvider.Tag.ToString(), txtProvider.Text, intInvoiceType);
                    //FrmFp.ShowDialog();
                    MessageBox.Show("修改成功","提示");
                    dgv_Main.DataSource = GetProviderName(m_findVoice.GetInvoiceInfo(dtp_Start.Value, dtp_End.Value));
                }
                else
                {
                    MessageDialog.ShowErrorMessage(m_err);
                }
            }
        }

        private void dgv_Main_DoubleClick(object sender, EventArgs e)
        {
            string v_Invoice = dgv_Main.CurrentRow.Cells["InvoiceCode"].Value.ToString();
            string v_Provider = dgv_Main.CurrentRow.Cells["供应商"].Value.ToString();
            string v_ProviderName = dgv_Main.CurrentRow.Cells["ProviderName"].Value.ToString();
            string v_PZH = txtPZH.Text;

            int intInvoiceType = dgv_Main.CurrentRow.Cells["InvoiceType"].Value.ToString() == "普通发票" ? 0 : 1;

            if (txtInvoice.Text.Trim() == "")
            {
                MessageBox.Show("请输入发票号", "提示");
            }
            else if (txtProvider.Tag.ToString().Trim() == "" || txtProvider.Text.Trim() == "")
            {
                MessageBox.Show("请输入供应商", "提示");
            }
            else
            {
                发票明细清单 FrmFp = new 发票明细清单(txtInvoice.Text.Trim(), txtProvider.Tag.ToString(), 
                    txtProvider.Text, intInvoiceType, txtPZH.Text, m_authFlag);
                FrmFp.ShowDialog();
            }

            dgv_Main.DataSource = GetProviderName(m_findVoice.GetInvoiceInfo(dtp_Start.Value, dtp_End.Value));
            PositioningRecord(v_PZH);
        }

        private void btnFindProvider_Click(object sender, EventArgs e)
        {
            m_formProvider = QueryInfoDialog.GetProviderInfoDialog();

            if (m_formProvider.ShowDialog() == DialogResult.OK)
            {
                txtProvider.Tag = m_formProvider.GetStringDataItem("供应商编码");
                txtProvider.Text = m_formProvider.GetStringDataItem("简称");
            }
        }

        /// <summary>
        /// 获得供应商名称
        /// </summary>
        /// <param name="dtNoName">需要获取供应商名称的数据集</param>
        /// <returns>返回已获得供应商名称的数据集</returns>
        private DataTable GetProviderName(DataTable dtNoName)
        {
            if (dtNoName != null)
            {
                dtNoName.Columns.Add("ProviderName");

                for (int i = 0; i <= dtNoName.Rows.Count - 1; i++)
                {
                    dtNoName.Rows[i]["ProviderName"] = m_serverProvider.GetProviderName(dtNoName.Rows[i]["Provider"].ToString());
                }

                return dtNoName;
            }

            return null;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            txtInvoice.Text = "";
            txtProvider.Tag = "";
            txtProvider.Text = "";
            cmbInvoiceType.Text = "";
            txtPZH.Text = "";
        }

        /// <summary>
        /// 检查数据
        /// </summary>
        /// <returns>通过返回True，否则返回False</returns>
        private bool CheckDate()
        {
            if (txtInvoice.Text.Trim() == "")
            {
                MessageBox.Show("请输入发票号", "提示");
                return false;
            }
            else if (txtProvider.Tag.ToString().Trim() == "" || txtProvider.Text.Trim() == "")
            {
                MessageBox.Show("请选择供应商", "提示");
                return false;
            }
            else if (cmbInvoiceType.Text == "")
            {
                MessageBox.Show("请选择发票类型", "提示");
                return false;
            }

            if (cmbInvoiceType.Text == "专用发票")
            {
                m_intInvoiceType = 1;
            }
            else
            {
                m_intInvoiceType = 0;
            }

            return true;
        }

        private void btn_find_Click(object sender, EventArgs e)
        {
            dgv_Main.DataSource = GetProviderName(m_findVoice.GetInvoiceInfo(dtp_Start.Value, dtp_End.Value));
        }

        private void 发票管理_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        private void dgv_Main_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                e.RowBounds.Location.Y,
                dgv_Main.RowHeadersWidth - 4,
                e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                dgv_Main.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dgv_Main.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }
    }
}
