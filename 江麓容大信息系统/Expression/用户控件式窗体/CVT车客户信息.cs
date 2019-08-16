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
using WebServerModule2;
using UniversalControlLibrary;


namespace Expression
{
    /// <summary>
    /// CVT客户信息界面
    /// </summary>
    public partial class CVT车客户信息 : Form
    {
        /// <summary>
        /// 数据集
        /// </summary>
        YX_CVTCustomerInformation m_lnqCVTCustomerInformation = 
            new YX_CVTCustomerInformation();

        /// <summary>
        /// 服务组件
        /// </summary>
        ICVTCustomerInformationServer m_serverCVTCustomer =
            ServerModuleFactory.GetServerModule<ICVTCustomerInformationServer>();

        /// <summary>
        /// 服务组件
        /// </summary>
        IProductListServer m_serverProductList =
            ServerModuleFactory.GetServerModule<IProductListServer>();

        /// <summary>
        /// 服务组件
        /// </summary>
        IServiceFeedBack2 m_serverFeedback =
            ServerModuleFactory2.GetServerModule<IServiceFeedBack2>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strError;

        /// <summary>
        /// 数据表
        /// </summary>
        DataTable m_dtDS = new DataTable();

        /// <summary>
        /// 权限组件
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 查找条件字段列表
        /// </summary>
        List<string> m_lstFindField = new List<string>();

        public CVT车客户信息(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authFlag = nodeInfo.Authority;

            DataTable dtProvince = UniversalFunction.GetProvinceTable();

            for (int i = 0; i < dtProvince.Rows.Count; i++)
            {
                cmbProvince.Items.Add(dtProvince.Rows[i]["ProName"].ToString());
            }

            DataTable dtCarModel = m_serverProductList.GetMotorcycleType();

            for (int i = 0; i < dtCarModel.Rows.Count; i++)
            {
                cmbCarModel.Items.Add(dtCarModel.Rows[i]["CarModel"].ToString());
            }

            txtProductCode.OnCompleteSearch += 
                new GlobalObject.DelegateCollection.NonArgumentHandle(txtProductCode_OnCompleteSearch);

            RefreshDataGirdView(m_serverCVTCustomer.GetCVTCustomerInformation());
        }

        private void CVT车客户信息_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authFlag);
        }

        /// <summary>
        /// 权限控制
        /// </summary>
        /// <param name="authorityFlag">权限标志</param>
        void AuthorityControl(PlatformManagement.AuthorityFlag authorityFlag)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip1, authorityFlag);
            FaceAuthoritySetting.SetEnable(this.Controls, authorityFlag);
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="source">数据集</param>
        void RefreshDataGirdView(DataTable source)
        {
            m_dtDS = source;

            m_lstFindField.Clear();

            DataColumnCollection columns = source.Columns;

            if (columns.Count > 0)
            {
                for (int i = 0; i < columns.Count; i++)
                {
                    m_lstFindField.Add(columns[i].ColumnName);
                }
            }
            
            dataGridView1.DataSource = source;

            lbCount.Text = dataGridView1.Rows.Count.ToString();

            dataGridView1.Columns["序号"].Visible = false;
            dataGridView1.Columns["物品ID"].Visible = false;
            dataGridView1.Columns["销售日期"].Width = 80;
            dataGridView1.Columns["省份"].Width = 40;
            dataGridView1.Columns["客户名称"].Width = 70;
            dataGridView1.Columns["CVT型号"].Width = 80;
            dataGridView1.Columns["车架号"].Width = 130;

            userControlDataLocalizer.Init(dataGridView1, this.Name, 
                UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
        }

        /// <summary>
        /// 获得数据
        /// </summary>
        void GetMessage()
        {
            m_lnqCVTCustomerInformation.ID = Convert.ToInt32( dtpSellDate.Tag);
            m_lnqCVTCustomerInformation.ClientName = txtClintName.Text.Trim();
            m_lnqCVTCustomerInformation.CVTNumber = txtCVTNumber.Text.Trim();
            m_lnqCVTCustomerInformation.DealerName = txtDealerName.Text.Trim();
            m_lnqCVTCustomerInformation.FullAddress = txtFullAddress.Text.Trim();
            m_lnqCVTCustomerInformation.PhoneNumber = txtPhoneNumber.Text.Trim();
            m_lnqCVTCustomerInformation.ProductID = Convert.ToInt32(txtProductCode.Tag);
            m_lnqCVTCustomerInformation.Remark = txtRemark.Text.Trim();
            m_lnqCVTCustomerInformation.SellDate = dtpSellDate.Value.Date;
            m_lnqCVTCustomerInformation.SiteCity = cmbCity.Text;
            m_lnqCVTCustomerInformation.SiteProvince = cmbProvince.Text;
            m_lnqCVTCustomerInformation.VehicleShelfNumber = txtVehicleShelfNumber.Text.Trim();
            m_lnqCVTCustomerInformation.PY = UniversalFunction.GetPYWBCode(txtClintName.Text, "PY");
            m_lnqCVTCustomerInformation.WB = UniversalFunction.GetPYWBCode(txtClintName.Text, "WB");
            m_lnqCVTCustomerInformation.CarModelID = m_serverProductList.GetMotorcycleType(cmbCarModel.Text);
            m_lnqCVTCustomerInformation.VKT = Convert.ToInt32(numVKT.Value);
            m_lnqCVTCustomerInformation.OverTheReason = txtOverTheReason.Text.ToString().Trim();
            m_lnqCVTCustomerInformation.ProofNo = txtProof.Text;
        }  
                                
        void txtProductCode_OnCompleteSearch()
        {
            txtProductCode.Text = txtProductCode.DataResult["图号型号"].ToString();
            txtProductCode.Tag = Convert.ToInt32(txtProductCode.DataResult["序号"]);
        }

        private void cmbProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtCity = UniversalFunction.GetCityUnderlingProvince(cmbProvince.Text);

            cmbCity.Items.Clear();

            for (int i = 0; i < dtCity.Rows.Count; i++)
            {
                cmbCity.Items.Add(dtCity.Rows[i]["城市"].ToString());
            }
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        void ClearDate()
        {
            dtpSellDate.Value = ServerTime.Time;
            txtClintName.Text = "";
            txtCVTNumber.Text = "";
            txtDealerName.Text = "";
            txtFullAddress.Text = "";
            txtPhoneNumber.Text = "";
            txtProductCode.Text = "";
            txtProductCode.Tag = null;
            dtpSellDate.Tag = null;
            txtRemark.Text = "";
            txtVehicleShelfNumber.Text = "";
            txtProof.Text = "";
        }

        /// <summary>
        /// 检查信息的正确性
        /// </summary>
        /// <returns>完全正确返回True否则返回False</returns>
        bool CheckControl()
        {
            if (txtProductCode.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请选择CVT型号！");
                return false;
            }

            if (cmbCarModel.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请选择车型！");
                return false;
            }

            return true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!CheckControl())
            {
                return;
            }

            GetMessage();

            if (!m_serverCVTCustomer.InsertCVTCustomerInformation(m_lnqCVTCustomerInformation,
                out m_strError))
            {
                MessageDialog.ShowPromptMessage(m_strError);
                return;
            }
            else
            {
                MessageDialog.ShowPromptMessage("添加成功");
            }

            ClearDate();
            RefreshDataGirdView(m_serverCVTCustomer.GetCVTCustomerInformation());
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshDataGirdView(m_serverCVTCustomer.GetCVTCustomerInformation());
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!CheckControl())
            {
                return;
            }

            GetMessage();

            if (!m_serverCVTCustomer.UpdateCVTCustomerInformation(m_lnqCVTCustomerInformation, 
                out m_strError))
            {
                MessageDialog.ShowPromptMessage(m_strError);
                return;
            }
            else
            {
                MessageDialog.ShowPromptMessage("修改成功");
            }

            ClearDate();
            RefreshDataGirdView(m_serverCVTCustomer.GetCVTCustomerInformation());
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in dataGridView1.SelectedRows)
            {
                m_lnqCVTCustomerInformation.ID = Convert.ToInt32(item.Cells["序号"].Value);

                if (!m_serverCVTCustomer.DeleteCVTCustomerInformation(m_lnqCVTCustomerInformation, out m_strError))
                {
                    MessageDialog.ShowPromptMessage(m_strError);
                    return;
                }
            }

            MessageDialog.ShowPromptMessage("删除成功!");

            ClearDate();
            RefreshDataGirdView(m_serverCVTCustomer.GetCVTCustomerInformation());
        }

        private void btnOutExcel_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
        }

        /// <summary>
        /// 检测数据集
        /// </summary>
        /// <param name="checkList">需要检测的数据集</param>
        /// <returns>通过返回True，不通过返回False</returns>
        bool CheckTable(DataTable checkList)
        {
            if (!checkList.Columns.Contains("销售日期"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【销售日期】信息");
                return false;
            }

            if (!checkList.Columns.Contains("省份"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【省份】信息");
                return false;
            }

            if (!checkList.Columns.Contains("车辆所在地"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【车辆所在地】信息");
                return false;
            }

            if (!checkList.Columns.Contains("经销商名称"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【经销商名称】信息");
                return false;
            }

            if (!checkList.Columns.Contains("车架号"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【销售日期】信息");
                return false;
            }

            if (!checkList.Columns.Contains("客户名称"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【客户名称】信息");
                return false;
            }

            if (!checkList.Columns.Contains("联系电话"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【联系电话】信息");
                return false;
            }

            if (!checkList.Columns.Contains("详细地址"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【详细地址】信息");
                return false;
            }

            if (!checkList.Columns.Contains("CVT编号"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【CVT编号】信息");
                return false;
            }

            if (!checkList.Columns.Contains("CVT型号"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【CVT型号】信息");
                return false;
            }

            if (!checkList.Columns.Contains("车型"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【车型】信息");
                return false;
            }

            if (!checkList.Columns.Contains("三包凭证号"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【三包凭证号】信息");
                return false;
            }

            return true;
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                return;
            }

            if (!m_serverFeedback.GetAfterServicerByUserInfo(dataGridView1.CurrentRow.Cells["车架号"].Value.ToString())[0].ToString().Equals("0")
                || !m_serverFeedback.GetServiceFeedBackByUserInfo(
                dataGridView1.CurrentRow.Cells["车架号"].Value.ToString())[0].ToString().Equals("0"))
            {
                txtClintName.ReadOnly = false;
                txtCVTNumber.ReadOnly = false;
                txtDealerName.ReadOnly = true;
                txtFullAddress.ReadOnly = false;
                txtPhoneNumber.ReadOnly = false;
                txtProductCode.ReadOnly = true;
                txtRemark.ReadOnly = true;
                txtVehicleShelfNumber.ReadOnly = true;
                cmbProvince.Enabled = true;
                cmbCity.Enabled = true;
                dtpSellDate.Enabled = true;
                //cmbCarModel.Enabled = false;
            }
            else
            {
                txtClintName.ReadOnly = false;
                txtCVTNumber.ReadOnly = false;
                txtDealerName.ReadOnly = false;
                txtFullAddress.ReadOnly = false;
                txtPhoneNumber.ReadOnly = false;
                txtProductCode.ReadOnly = false;
                txtProductCode.ReadOnly = false;
                txtRemark.ReadOnly = false;
                txtVehicleShelfNumber.ReadOnly = false;
                cmbProvince.Enabled = true;
                cmbCity.Enabled = true;
                dtpSellDate.Enabled = true;
                dtpSellDate.Enabled = true;
                //cmbCarModel.Enabled = true;
            }

            txtClintName.Text = dataGridView1.CurrentRow.Cells["客户名称"].Value.ToString();
            txtCVTNumber.Text = dataGridView1.CurrentRow.Cells["CVT编号"].Value.ToString();
            txtDealerName.Text = dataGridView1.CurrentRow.Cells["经销商名称"].Value.ToString();
            txtFullAddress.Text = dataGridView1.CurrentRow.Cells["详细地址"].Value.ToString();
            txtPhoneNumber.Text = dataGridView1.CurrentRow.Cells["联系电话"].Value.ToString();
            txtProductCode.Text = dataGridView1.CurrentRow.Cells["CVT型号"].Value.ToString();
            txtProductCode.Tag = Convert.ToInt32(dataGridView1.CurrentRow.Cells["物品ID"].Value);
            txtRemark.Text = dataGridView1.CurrentRow.Cells["备注"].Value.ToString();
            txtVehicleShelfNumber.Text = dataGridView1.CurrentRow.Cells["车架号"].Value.ToString();
            cmbProvince.Text = dataGridView1.CurrentRow.Cells["省份"].Value.ToString().Trim();
            cmbCity.Text = dataGridView1.CurrentRow.Cells["车辆所在地"].Value.ToString().Trim();
            dtpSellDate.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["销售日期"].Value).Date;
            dtpSellDate.Tag = Convert.ToInt32(dataGridView1.CurrentRow.Cells["序号"].Value);
            cmbCarModel.Text = dataGridView1.CurrentRow.Cells["车型"].Value.ToString().Trim();
            numVKT.Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["行使里程"].Value);
            txtOverTheReason.Text = dataGridView1.CurrentRow.Cells["过保原因记录"].Value.ToString();
            txtProof.Text = dataGridView1.CurrentRow.Cells["三包凭证号"].Value.ToString();
        }

        private void btnInExcel_Click(object sender, EventArgs e)
        {
            DataTable dtTemp = ExcelHelperP.RenderFromExcel(openFileDialog1);

            if (dtTemp == null)
            {
                //MessageDialog.ShowPromptMessage(m_strError);
                return;
            }

            if (CheckTable(dtTemp))
            {
                if (!m_serverCVTCustomer.BatchInsertCVTCustomerInformation(dtTemp, out m_strError))
                {
                    MessageDialog.ShowPromptMessage(m_strError);
                    return;
                }
                else
                {
                    string strMessage = "";

                    if (m_strError != null)
                    {
                        strMessage = "\n以下车架号的记录日期格式不正确"
                            + m_strError + "\n系统已默认为【2099-12-31】，请自行修改";
                    }

                    MessageDialog.ShowPromptMessage("导入成功" + strMessage);
                }
            }

            ClearDate();
            RefreshDataGirdView(m_serverCVTCustomer.GetCVTCustomerInformation());
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            FormFindCondition formFindCondition = new FormFindCondition(m_lstFindField.ToArray());

            if (formFindCondition.ShowDialog() != DialogResult.OK)
            {
                return;
            }


            DataTable dtTemp = GlobalObject.DataSetHelper.SiftDataTable(m_dtDS,
                formFindCondition.SearchSQL, out m_strError);

            if (dtTemp == null)
            {
                MessageDialog.ShowPromptMessage(m_strError);
            }
            else
            {
                dataGridView1.DataSource = dtTemp;
            }
        }

        private void cVT客户历史信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CVT客户历史信息 form = new CVT客户历史信息("CVT客户历史信息",dataGridView1.CurrentRow.Cells["车架号"].Value.ToString());

            form.Show();
        }

        private void 车辆维修记录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CVT客户历史信息 form = new CVT客户历史信息("车辆维修记录", dataGridView1.CurrentRow.Cells["车架号"].Value.ToString());

            form.Show();
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dataGridView1.Columns.Count == 0)
            {
                return;
            }
            else
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Cells["过保原因记录"].Value.ToString().Length == 0)
                    {
                        if (dataGridView1.Rows[i].Cells["1年以上(或2万公里以上)"].Value.ToString() == "是")
                        {
                            dataGridView1.Rows[i].Cells["1年以上(或2万公里以上)"].Style.BackColor = Color.Red;
                        }

                        if (dataGridView1.Rows[i].Cells["2年以上(或4万公里以上)"].Value.ToString() == "是")
                        {
                            dataGridView1.Rows[i].Cells["2年以上(或4万公里以上)"].Style.BackColor = Color.Red;
                        }
                        
                        if (dataGridView1.Rows[i].Cells["3年以上(或6万公里以上)"].Value.ToString() == "是")
                        {
                            dataGridView1.Rows[i].Cells["3年以上(或6万公里以上)"].Style.BackColor = Color.Red;
                        }
                        
                        if (dataGridView1.Rows[i].Cells["5年以上(或10万公里以上)"].Value.ToString() == "是")
                        {
                            dataGridView1.Rows[i].Cells["5年以上(或10万公里以上)"].Style.BackColor = Color.Red;
                        }
                    }
                    else
                    {
                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    }
                }
            }
        }

        private void btnCompositeQuery_Click(object sender, EventArgs e)
        {
            IAuthorization authorization = PlatformFactory.GetObject<IAuthorization>();
            string businessID = "营销出库综合查询";
            IQueryResult qr = authorization.Query(businessID, null, null, 0);
            List<string> lstFindField = new List<string>();

            if (qr.DataCollection == null || qr.DataCollection.Tables.Count == 0)
            {
                return;
            }

            DataColumnCollection columns = qr.DataCollection.Tables[0].Columns;

            if (qr.Succeeded && columns.Count > 0)
            {
                for (int i = 0; i < columns.Count; i++)
                {
                    lstFindField.Add(columns[i].ColumnName);
                }
            }

            FormConditionFind formFindCondition = new FormConditionFind(this, lstFindField.ToArray(), businessID, labelTitle.Text);
            formFindCondition.ShowDialog();
        }

        private void 新建toolStripButton_Click(object sender, EventArgs e)
        {
            ClearDate();
            txtClintName.ReadOnly = false;
            txtCVTNumber.ReadOnly = false;
            txtDealerName.ReadOnly = false;
            txtFullAddress.ReadOnly = false;
            txtPhoneNumber.ReadOnly = false;
            txtProductCode.ReadOnly = false;
            txtProductCode.ReadOnly = false;
            txtRemark.ReadOnly = false;
            txtVehicleShelfNumber.ReadOnly = false;
            cmbProvince.Enabled = true;
            cmbCity.Enabled = true;
            dtpSellDate.Enabled = true;
            dtpSellDate.Enabled = true;
            cmbCarModel.Enabled = true;
            numVKT.Enabled = true;
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

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            m_serverCVTCustomer.BatchMatchingCVTNumber();

            MessageDialog.ShowPromptMessage("匹配完成");

            RefreshDataGirdView(m_serverCVTCustomer.GetCVTCustomerInformation());
        }
    }
}
