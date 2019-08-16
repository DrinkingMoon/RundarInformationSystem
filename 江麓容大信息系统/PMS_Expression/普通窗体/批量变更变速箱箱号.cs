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
    public partial class 批量变更变速箱箱号 : Form
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
        /// 产品编码服务
        /// </summary>
        IProductCodeServer m_productCodeServer = SCM_Level02_ServerFactory.GetServerModule<IProductCodeServer>();

        /// <summary>
        /// 变速箱箱号变更服务
        /// </summary>
        IConvertCVTNumber m_convertCVTServer = ServerModuleFactory.GetServerModule<IConvertCVTNumber>();

        /// <summary>
        /// 记录产品条形码是否打印信息的服务
        /// </summary>
        IPrintProductBarcodeInfo m_productPrintInfoServer = ServerModuleFactory.GetServerModule<IPrintProductBarcodeInfo>();

        public 批量变更变速箱箱号()
        {
            InitializeComponent();
        }

        private void 批量变更变速箱箱号_Load(object sender, EventArgs e)
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

            cmbOldCVTType.Items.AddRange(lstProductType.ToArray());
            cmbNewCVTType.Items.AddRange(lstProductType.ToArray());

            this.cmbOldCVTType.SelectedIndexChanged -= new System.EventHandler(this.cmbCVTType_SelectedIndexChanged);
            this.cmbNewCVTType.SelectedIndexChanged -= new System.EventHandler(this.cmbCVTType_SelectedIndexChanged);

            cmbOldCVTType.Text = "RDC15-FB";
            cmbNewCVTType.Text = "RDC15-FB";

            this.cmbOldCVTType.SelectedIndexChanged += new System.EventHandler(this.cmbCVTType_SelectedIndexChanged);
            this.cmbNewCVTType.SelectedIndexChanged += new System.EventHandler(this.cmbCVTType_SelectedIndexChanged);
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            if (txtNewCVTNumber.Text == "")
            {
                MessageDialog.ShowPromptMessage("请录入新箱箱号后再进行此操作");
                txtNewCVTNumber.Focus();
                return;
            }

            if (dataGridView1.SelectedRows.Count != 1)
            {
                MessageDialog.ShowPromptMessage("请选择一行记录行后再进行此操作");
                return;
            }

            txtNewCVTNumber.Text = txtNewCVTNumber.Text.Trim().ToUpper();

            // 检测录入的新箱箱号格式是否正确
            if (!m_productCodeServer.VerifyProductCodesInfo(cmbNewCVTType.Text, txtNewCVTNumber.Text, GlobalObject.CE_BarCodeType.内部钢印码, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);

                txtNewCVTNumber.SelectAll();
                txtNewCVTNumber.Focus();

                return;
            }

            if (!chkIsNewCVT.Checked && txtNewCVTNumber.Text.Substring(txtNewCVTNumber.Text.Length - 5, 1) != "F")
            {
                MessageDialog.ShowPromptMessage("返修箱没有包含F");
                txtNewCVTNumber.Focus();
                return;
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (i != dataGridView1.SelectedRows[0].Index && dataGridView1.Rows[i].Cells[1].Value.ToString() == txtNewCVTNumber.Text)
                {
                    MessageDialog.ShowPromptMessage(string.Format("数据显示控件中已经存在 {0} 的箱号,不允许重复添加", txtNewCVTNumber.Text));
                    return;
                }
            }

            dataGridView1.SelectedRows[0].Cells["新箱箱号"].Value = txtNewCVTNumber.Text;
            dataGridView1.SelectedRows[0].Cells["变更模式"].Value = "手动模式";
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

            txtReason.Text = txtReason.Text.Trim();

            if (txtReason.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请录入变更原因后在进行此操作");
                txtReason.Focus();
                return;
            }

            // 手动模式变更的信息
            List<ZPX_ConvertedCVTNumber> lstManualMode = new List<ZPX_ConvertedCVTNumber>();

            // 自动动模式变更的信息
            List<ZPX_ConvertedCVTNumber> lstAutoMode = new List<ZPX_ConvertedCVTNumber>();

            DateTime date = ServerTime.Time;

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewCellCollection cells = dataGridView1.Rows[i].Cells;
                string newCVTNumber = cells["新箱箱号"].Value.ToString();
                string oldCVTNumber = cells["旧箱箱号"].Value.ToString();

                #region 2013-09-18 夏石友，新箱未入库型号变更
                //if (m_convertCVTServer.IsNewCVT(cmbOldCVTType.Text, oldCVTNumber))
                //{
                //    MessageDialog.ShowErrorMessage(string.Format("旧箱箱号 {0} 还没有进行营销业务，不允许进行此操作", oldCVTNumber));
                //    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[0];
                //    return;
                //}
                #endregion

                if (m_convertCVTServer.IsExists(ConvertCVTNumber_CheckEnum.检查旧箱信息, cmbOldCVTType.Text, oldCVTNumber))
                {
                    MessageDialog.ShowErrorMessage(string.Format("旧箱箱号 {0} 已经变更过，不允许进行此操作", oldCVTNumber));

                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[0];

                    return;
                }

                if (!m_convertCVTServer.IsExists(ConvertCVTNumber_CheckEnum.检查旧箱档案信息, cmbOldCVTType.Text, oldCVTNumber))
                {
                    MessageDialog.ShowErrorMessage(string.Format("电子档案中不存在旧箱箱号 {0} 的信息，不允许进行此操作", oldCVTNumber));

                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[0];

                    return;
                }

                if (cells["变更模式"].Value.ToString() == "手动模式")
                {
                    if (!m_convertCVTServer.IsNewCVT(cmbNewCVTType.Text, newCVTNumber))
                    {
                        MessageDialog.ShowErrorMessage(string.Format("新箱箱号 {0} 已经被使用过，不允许进行此操作", newCVTNumber));

                        dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[0];

                        return;
                    }

                    if (m_convertCVTServer.IsExists(ConvertCVTNumber_CheckEnum.检查新箱档案信息, cmbNewCVTType.Text, newCVTNumber))
                    {
                        MessageDialog.ShowErrorMessage("新箱号电子档案中已经存在，不允许再进行此操作");

                        dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[0];

                        return;
                    }

                    if (m_convertCVTServer.IsExists(ConvertCVTNumber_CheckEnum.检查新旧箱信息, cmbNewCVTType.Text, newCVTNumber))
                    {
                        MessageDialog.ShowErrorMessage(string.Format("新箱箱号 {0} 已经变更过，不允许进行此操作", newCVTNumber));

                        dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[0];

                        return;
                    }

                    if (!ServerModuleFactory.GetServerModule<IPrintProductBarcodeInfo>().IsExists(cmbNewCVTType.Text + " " + newCVTNumber))
                    {
                        MessageDialog.ShowErrorMessage(string.Format("新箱箱号 {0} 还未分配，手动模式时新箱箱号必须是已经打过条形码的，否则不允许进行此操作",
                            newCVTNumber));

                        dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[0];

                        return;
                    }
                }
                else
                {
                    MessageDialog.ShowPromptMessage("当前不允许存在“自动模式”变更方式，必须手动变更，人为设置新箱箱号(可打印条形码分配箱号)");
                    return;
                }

                ZPX_ConvertedCVTNumber data = new ZPX_ConvertedCVTNumber();

                data.Date = date;
                data.IsZeroKilometre = chkIsNewCVT.Checked;
                data.NewProductNumber = newCVTNumber;
                data.NewProductType = cmbNewCVTType.Text + (chkIsNewCVT.Checked ? "" : " FX");
                data.OldProductNumber = cells["旧箱箱号"].Value.ToString();
                data.OldProductType = cmbOldCVTType.Text;
                data.UserCode = BasicInfo.LoginID;
                data.Remark = string.Format("批量变更：{0}", txtReason.Text);

                if (cells["变更模式"].Value.ToString() == "手动模式")
                {
                    lstManualMode.Add(data);
                }
                else
                {
                    lstAutoMode.Add(data);
                }
            }

            if (MessageDialog.ShowEnquiryMessage("变更箱号时将会为新箱号创建电子档案，复制旧箱号数据。是否继续？") == DialogResult.No)
            {
                return;
            }

            if (!m_convertCVTServer.BatchConvertCVTNumber("手动模式", lstManualMode, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            if (!m_convertCVTServer.BatchConvertCVTNumber("自动模式", lstAutoMode, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            MessageDialog.ShowPromptMessage("批量变更箱号成功");

            txtNewCVTNumber.Text = "";

            btnSave.Enabled = false;

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewCellCollection cells = dataGridView1.Rows[i].Cells;

                if (cells["变更模式"].Value.ToString() == "自动模式")
                {
                    cells["新箱箱号"].Value = lstAutoMode[i].NewProductNumber;
                    cells["变更模式"].Value += " 已分配箱号";

                    dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.Blue;
                }
            }
        }

        /// <summary>
        /// 删除选中行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选中至少一条记录后再进行此操作");
                return;
            }

            for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
            {
                dataGridView1.Rows.Remove(dataGridView1.SelectedRows[i]);
            }
        }

        /// <summary>
        /// 绘制行号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

            MessageDialog.ShowPromptMessage("如果营销单据中存在非旧箱型号的变速箱则自动忽略，只保留与旧箱型号相同的变速箱箱号");

            DataTable dt = m_electronFileServer.GetProductNumberFromSellBill(billNo);

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int index = 0; index < dt.Rows.Count; index++)
                {
                    if (dt.Rows[index][0].ToString() != cmbOldCVTType.Text)
                    {
                        continue;
                    }

                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        if (dataGridView1.Rows[i].Cells[0].Value.ToString() == dt.Rows[index][1].ToString() ||
                            dataGridView1.Rows[i].Cells[1].Value.ToString() == dt.Rows[index][1].ToString())
                        {
                            MessageDialog.ShowPromptMessage(string.Format("数据显示控件中已经存在 {0} {1} 的记录,不允许重复添加",
                                dt.Rows[index][0].ToString(), dt.Rows[index][1].ToString()));

                            dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[0];

                            return;
                        }
                    }

                    dataGridView1.Rows.Add(new object[] { dt.Rows[index][1].ToString(), "", BasicInfo.LoginID, m_dt, "自动模式" });
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("没有找到此单据的信息, 请检查单据号是否正确");
            }
        }

        /// <summary>
        /// 旧箱或新箱型号发生变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbCVTType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MessageDialog.ShowEnquiryMessage("您要变更产品型号吗? 变更产品型号将会放弃先前操作,是否继续?") == DialogResult.Yes)
            {
                dataGridView1.Rows.Clear();

                btnSave.Enabled = true;
            }
        }

        /// <summary>
        /// 清除所有数据显示控件中的记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();

            btnSave.Enabled = true;
        }

        /// <summary>
        /// 是否变更为新箱箱号检查框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkIsNewCVT_CheckedChanged(object sender, EventArgs e)
        {
            string showInfo = null;

            if (chkIsNewCVT.Checked)
            {
                showInfo = "您确定当前变更为新箱箱号吗（箱号不带“F”）？";
            }
            else
            {
                showInfo = "您确定当前变更为返修箱箱号吗（箱号带“F”）？";
            }

            if (MessageDialog.ShowEnquiryMessage(showInfo) != DialogResult.Yes)
            {
                this.chkIsNewCVT.CheckedChanged -= new System.EventHandler(this.chkIsNewCVT_CheckedChanged);

                chkIsNewCVT.Checked = !chkIsNewCVT.Checked;

                this.chkIsNewCVT.CheckedChanged += new System.EventHandler(this.chkIsNewCVT_CheckedChanged);
            }
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtOldCVTNumber.Text == "")
            {
                MessageDialog.ShowPromptMessage("请录入旧箱箱号后再进行此操作");
                txtOldCVTNumber.Focus();
                return;
            }

            if (txtNewCVTNumber.Text == "")
            {
                MessageDialog.ShowPromptMessage("请录入新箱号后再进行此操作");
                txtNewCVTNumber.Focus();
                return;
            }

            txtOldCVTNumber.Text = txtOldCVTNumber.Text.Trim().ToUpper();
            txtNewCVTNumber.Text = txtNewCVTNumber.Text.Trim().ToUpper();

            // 检测录入的旧箱箱号格式是否正确
            if (!m_productCodeServer.VerifyProductCodesInfo(cmbOldCVTType.Text, txtOldCVTNumber.Text, GlobalObject.CE_BarCodeType.内部钢印码, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);

                txtOldCVTNumber.SelectAll();
                txtNewCVTNumber.Focus();

                return;
            }

            // 检测录入的新箱箱号格式是否正确
            if (!m_productCodeServer.VerifyProductCodesInfo(cmbNewCVTType.Text, txtNewCVTNumber.Text, GlobalObject.CE_BarCodeType.内部钢印码, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);

                txtNewCVTNumber.SelectAll();
                txtNewCVTNumber.Focus();

                return;
            }

            if (!chkIsNewCVT.Checked && txtNewCVTNumber.Text.Substring(txtNewCVTNumber.Text.Length - 5, 1) != "F")
            {
                MessageDialog.ShowPromptMessage("返修箱没有包含F");
                txtNewCVTNumber.Focus();
                return;
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[0].Value.ToString() == txtOldCVTNumber.Text ||
                    dataGridView1.Rows[i].Cells[1].Value.ToString() == txtOldCVTNumber.Text)
                {
                    MessageDialog.ShowPromptMessage(string.Format("数据显示控件中已经存在 {0} 旧箱箱号,不允许重复添加", txtOldCVTNumber.Text));
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[0];
                    return;
                }

                if (dataGridView1.Rows[i].Cells[0].Value.ToString() == txtNewCVTNumber.Text ||
                    dataGridView1.Rows[i].Cells[1].Value.ToString() == txtNewCVTNumber.Text)
                {
                    MessageDialog.ShowPromptMessage(string.Format("数据显示控件中已经存在 {0} 新箱箱号,不允许重复添加", txtNewCVTNumber.Text));
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[0];
                    return;
                }
            }

            dataGridView1.Rows.Add(new object[] { txtOldCVTNumber.Text, txtNewCVTNumber.Text, BasicInfo.LoginID, m_dt, "手动模式" });
        }

        private void btnInput_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageDialog.ShowEnquiryMessage("请确认所导入的EXCEL文件第一列为所需要导入的编号信息且列名为【旧箱箱号】，第二列为所需要导入的编号信息且列名为【新箱箱号】") == DialogResult.No)
                {
                    return;
                }

                DataTable dtTemp = ExcelHelperP.RenderFromExcel(openFileDialog1);

                if (dtTemp == null)
                {
                    return;
                }
                else
                {
                    foreach (DataRow dr in dtTemp.Rows)
                    {
                        if (dr[0].ToString().Trim().Length != 0 && dr[1].ToString().Trim().Length != 0)
                        {
                            txtOldCVTNumber.Text = dr[0].ToString();
                            txtNewCVTNumber.Text = dr[1].ToString();
                            btnAdd_Click(sender, e);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
                return;
            }

        }
    }
}
