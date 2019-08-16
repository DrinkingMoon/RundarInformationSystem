using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using ServerModule;
using PlatformManagement;
using UniversalControlLibrary;
using Expression;
using CommonBusinessModule;

namespace Expression
{
    public partial class 下线试验信息管理 : Form
    {
        /// <summary>
        /// 下线试验结果信息服务
        /// </summary>
        IOffLineTest m_testServer = PMS_ServerFactory.GetServerModule<IOffLineTest>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_error;

        /// <summary>
        /// 查询列名
        /// </summary>
        List<string> m_lstFindField = new List<string>();

        /// <summary>
        /// 产品信息服务
        /// </summary>
        private IProductInfoServer m_productInfoServer = PMS_ServerFactory.GetServerModule<IProductInfoServer>();

        /// <summary>
        /// 变速箱箱号
        /// </summary>
        string m_productNumber;

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag;

        public 下线试验信息管理(FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authFlag = nodeInfo.Authority;

            IQueryable<View_P_ProductInfo> productInfo = null;

            if (!m_productInfoServer.GetAllProductInfo(out productInfo, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }
            else
            {
                productInfo = from r in productInfo
                              where !r.产品类型名称.Contains("返修")
                              select r;

                cmbProductCode.DataSource = productInfo;
                cmbProductCode.DisplayMember = "产品类型编码";
                cmbProductCode.ValueMember = "产品类型编码";
            }

            SearchData();
        }

        private void 下线试验信息管理_Load(object sender, EventArgs e)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip1, m_authFlag);
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

        private void 下线试验信息管理_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        /// <summary>
        /// 新建
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNew_Click(object sender, EventArgs e)
        {
            cmbProductCode.SelectedIndex = -1;
            txtProductNumber.Text = "";
            试验合格.Checked = false;
            txtFault.Text = "";
            txtRemark.Text = "";
        }

        /// <summary>
        /// 检查界面上的数据是否有效，为添加、更新、删除操作作准备
        /// </summary>
        /// <returns>有效返回true</returns>
        private bool CheckData()
        {
            m_productNumber = "";

            if (cmbProductCode.Text == "")
            {
                MessageDialog.ShowPromptMessage("请选择产品型号后再进行此操作");

                cmbProductCode.Focus();
                return false;
            }

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(txtProductNumber.Text.Trim()))
            {
                MessageDialog.ShowPromptMessage("请录入产品编号后再进行此操作");

                txtProductNumber.Focus();

                return false;
            }


            // 校验箱号录入是否正确
            m_productNumber = cmbProductCode.Text + " " + txtProductNumber.Text.Trim();

            if (!cmbRepairType.Text.Contains("允许无电子档案") && !m_testServer.CanOffLineTest(m_productNumber, out m_error))
            {
                MessageDialog.ShowPromptMessage(m_error);
                return false;
            }

            if (!SCM_Level02_ServerFactory.GetServerModule<IProductCodeServer>().VerifyProductCodesInfo(
                cmbProductCode.Text, txtProductNumber.Text, GlobalObject.CE_BarCodeType.内部钢印码, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);

                txtProductNumber.Focus();

                return false;
            }

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(txtAssembler.Text))
            {
                MessageDialog.ShowPromptMessage("请选择预装员后再进行此操作");

                return false;
            }

            txtFault.Text = txtFault.Text.Trim();

            if (!试验合格.Checked)
            {
                if (GlobalObject.GeneralFunction.IsNullOrEmpty(txtFault.Text))
                {
                    MessageDialog.ShowPromptMessage("试验不合格时必须录入故障现象");

                    txtFault.Focus();

                    return false;
                }
            }
            else
            {
                if (!GlobalObject.GeneralFunction.IsNullOrEmpty(txtFault.Text))
                {
                    MessageDialog.ShowPromptMessage("试验合格时不允许录入故障现象");

                    txtFault.Focus();

                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 按日期检索数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchData();
        }

        /// <summary>
        /// 检索数据
        /// </summary>
        void SearchData()
        {
            List<View_ZPX_CVTOffLineTestResult> result =
                m_testServer.GetViewData(dateTimePickerST.Value.Date, dateTimePickerET.Value.Date.AddDays(1)).ToList();

            RefreshDataGridView(result);
        }

        /// <summary>
        /// 刷新数据控件
        /// </summary>
        /// <param name="data">数据集</param>
        private void RefreshDataGridView(IEnumerable<View_ZPX_CVTOffLineTestResult> data)
        {
            dataGridView1.DataSource = data.ToList();

            this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(this.labelTitle.Text, e.Column);
        }

        /// <summary>
        /// 从dataGridView中获取选择的数据
        /// </summary>
        /// <returns>返回获取到的数据</returns>
        private View_ZPX_CVTOffLineTestResult GetSelectedData()
        {
            List<View_ZPX_CVTOffLineTestResult> result =
             dataGridView1.DataSource as List<View_ZPX_CVTOffLineTestResult>;

            DataGridViewCellCollection cells = dataGridView1.SelectedRows[0].Cells;
            View_ZPX_CVTOffLineTestResult data = null;

            if (result == null)
            {
                data = new View_ZPX_CVTOffLineTestResult();

                data.编号 = (int)cells["编号"].Value;
                data.产品型号 = (string)cells["产品型号"].Value;
                data.产品箱号 = (string)cells["产品箱号"].Value;
                data.预装员工号 = (string)cells["预装员工号"].Value;
                data.预装员姓名 = (string)cells["预装员姓名"].Value;
                data.故障现象 = (string)cells["故障现象"].Value;
                data.备注 = (string)cells["备注"].Value;
                data.合格标志 = (bool)cells["合格标志"].Value;
                data.记录时间 = (DateTime)cells["记录时间"].Value;
                data.记录员工号 = (string)cells["记录员工号"].Value;
                data.记录员姓名 = (string)cells["记录员姓名"].Value;
                data.检查标志 = (bool)cells["检查标志"].Value;
                data.检查时间 = cells["检查时间"].Value as DateTime?;
                data.类别 = (string)cells["类别"].Value;
            }
            else
            {
                data = result.Single(p => p.编号 == (int)cells["编号"].Value);
            }

            return data;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                return;
            }

            View_ZPX_CVTOffLineTestResult data = GetSelectedData();

            cmbProductCode.Text = data.产品型号;
            txtProductNumber.Text = data.产品箱号;
            txtAssembler.Text = data.预装员姓名;
            txtAssembler.Tag = data.预装员工号;
            txtFault.Text = data.故障现象;
            txtRemark.Text = data.备注;
            试验合格.Checked = data.合格标志;
            cmbRepairType.Text = data.类别;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CheckData())
                    return;

                ZPX_CVTOffLineTestResult data = new ZPX_CVTOffLineTestResult();

                data.ProductType = cmbProductCode.Text;
                data.ProductNumber = txtProductNumber.Text;
                data.Assembler = txtAssembler.Tag.ToString();
                data.Fault = txtFault.Text;
                data.Remark = txtRemark.Text.Trim();
                data.QualifiedFlag = 试验合格.Checked;
                data.UserCode = GlobalObject.BasicInfo.LoginID;
                data.Type = cmbRepairType.Text;

                m_testServer.Add(data);

                RefreshDataGridView(m_testServer.GetViewData(new View_ZPX_CVTOffLineTestResult() { 编号 = data.ID }));
            }
            catch (Exception exce)
            {
                MessageDialog.ShowErrorMessage(exce.Message);
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!CheckData())
                return;

            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择要操作的记录后再进行此操作");
                return;
            }

            View_ZPX_CVTOffLineTestResult data = GetSelectedData();

            data.产品型号 = cmbProductCode.Text;
            data.产品箱号 = txtProductNumber.Text;
            data.预装员工号 = txtAssembler.Tag.ToString();
            data.故障现象 = txtFault.Text;
            data.备注 = txtRemark.Text.Trim();
            data.合格标志 = 试验合格.Checked;
            data.类别 = cmbRepairType.Text;

            try
            {
                m_testServer.Update(data);
                MessageDialog.ShowPromptMessage("执行成功");
                RefreshDataGridView(m_testServer.GetViewData(new View_ZPX_CVTOffLineTestResult() { 编号 = data.编号}));
            }
            catch (Exception exce)
            {
                MessageDialog.ShowErrorMessage(exce.Message);
            }
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAuditing_Click(object sender, EventArgs e)
        {
            if (!CheckData())
                return;

            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择要审核的行后再进行此操作");
                return;
            }

            if (MessageDialog.ShowEnquiryMessage("您确定要审核选择的记录吗？") == DialogResult.No)
                return;

            try
            {
                for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
                {
                    m_testServer.Auditing((int)dataGridView1.SelectedRows[i].Cells["编号"].Value);
                }

                SearchData();
            }
            catch (Exception exce)
            {
                MessageDialog.ShowErrorMessage(exce.Message);
            }
        }

        private void btnAdvSearch_Click(object sender, EventArgs e)
        {
            IAuthorization authorization = PlatformFactory.GetObject<IAuthorization>();
            string businessID = "下线试验结果综合查询";
            IQueryResult qr = null;

            if (m_lstFindField.Count == 0)
            {
                qr = authorization.Query(businessID, null, null, 0);

                DataColumnCollection columns = qr.DataCollection.Tables[0].Columns;

                if (qr.Succeeded && columns.Count > 0)
                {
                    for (int i = 0; i < columns.Count; i++)
                    {
                        m_lstFindField.Add(columns[i].ColumnName);
                    }
                }
            }

            FormFindCondition formFindCondition = new FormFindCondition(m_lstFindField.ToArray());

            if (formFindCondition.ShowDialog() != DialogResult.OK)
                return;

            qr = authorization.Query(businessID, null, formFindCondition.SearchSQL, -1);

            dataGridView1.DataSource = qr.DataCollection.Tables[0];
            dataGridView1.Refresh();
        }

        /// <summary>
        /// 检索实体数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearchProduct_Click(object sender, EventArgs e)
        {
            View_ZPX_CVTOffLineTestResult data = new View_ZPX_CVTOffLineTestResult();

            data.产品型号 = cmbProductCode.Text;
            data.产品箱号 = GlobalObject.GeneralFunction.IsNullOrEmpty(txtProductNumber.Text) ? null : txtProductNumber.Text;

            RefreshDataGridView(m_testServer.GetViewData(data));
        }

        /// <summary>
        /// 更新说明
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdateRemark_Click(object sender, EventArgs e)
        {
            if (!CheckData())
                return;

            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择要操作的记录后再进行此操作");
                return;
            }

            View_ZPX_CVTOffLineTestResult data = GetSelectedData();

            data.备注 = txtRemark.Text.Trim();

            try
            {
                m_testServer.UpdateRemark(data);

                MessageDialog.ShowPromptMessage("执行成功");

                RefreshDataGridView(m_testServer.GetViewData(new View_ZPX_CVTOffLineTestResult() { 编号 = data.编号 }));
            }
            catch (Exception exce)
            {
                MessageDialog.ShowErrorMessage(exce.Message);
            }
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            FormPersonnel form = new FormPersonnel(txtAssembler, GlobalObject.BasicInfo.DeptCode, "姓名");
            form.ShowDialog();

            txtAssembler.Tag = form.UserCode;
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            // 校验箱号录入是否正确
            m_productNumber = cmbProductCode.Text + " " + txtProductNumber.Text.Trim();

            if (m_testServer.CanOffLineTest(m_productNumber, out m_error))
            {
                MessageDialog.ShowPromptMessage("检测通过");
            }
            else
            {
                MessageDialog.ShowPromptMessage(m_error);
            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            FormDataShow form = new FormDataShow(m_testServer.GetLogInfo());

            form.Show();
        }

        /// <summary>
        /// 强制允许进行试验
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnForcedToSkip_Click(object sender, EventArgs e)
        {
            强制试验记录 form = new 强制试验记录();

            form.ShowDialog();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentRow == null)
                {
                    MessageDialog.ShowPromptMessage("请选择要操作的记录后再进行此操作");
                    return;
                }

                View_ZPX_CVTOffLineTestResult data = new View_ZPX_CVTOffLineTestResult();

                data.编号 = (int)dataGridView1.CurrentRow.Cells["编号"].Value;
                data.产品型号 = dataGridView1.CurrentRow.Cells["产品型号"].Value.ToString();
                data.产品箱号 = dataGridView1.CurrentRow.Cells["产品箱号"].Value.ToString();

                if (MessageDialog.ShowEnquiryMessage("是否要删除【产品型号】：" + data.产品型号 
                    + " 【产品箱号】：" + data.产品箱号 + "？") == DialogResult.No)
                {
                    return;
                }

                m_testServer.Delete(data);
                MessageDialog.ShowPromptMessage("删除成功");
                SearchData();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }

        }
    }
}
