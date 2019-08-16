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
    /// CVT装车信息界面
    /// </summary>
    public partial class CVT装车信息 : Form
    {
        /// <summary>
        /// 数据集
        /// </summary>
        YX_LoadingInfo m_lnqLoadingInfo = new YX_LoadingInfo();

        /// <summary>
        /// 服务组件
        /// </summary>
        ICVTTruckLoadingInformation m_serverCVTLoading =
            ServerModuleFactory.GetServerModule<ICVTTruckLoadingInformation>();

        /// <summary>
        /// 服务组件
        /// </summary>
        IProductListServer m_serverProductList =
            ServerModuleFactory.GetServerModule<IProductListServer>();

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

        public CVT装车信息(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();
            m_authFlag = nodeInfo.Authority;
            txtProduct.OnCompleteSearch +=
                new GlobalObject.DelegateCollection.NonArgumentHandle(txtProductCode_OnCompleteSearch);
            RefreshDataGirdView(m_serverCVTLoading.GetLoadingInfo());

            DataTable dtCarModel = m_serverProductList.GetMotorcycleType();

            for (int i = 0; i < dtCarModel.Rows.Count; i++)
            {
                cmbCarModel.Items.Add(dtCarModel.Rows[i]["CarModel"].ToString());
            }

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

            userControlDataLocalizer.Init(dataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));

            this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);
        }

        /// <summary>
        /// 获得信息
        /// </summary>
        void GetMessage()
        {
            m_lnqLoadingInfo.ID = Convert.ToInt32(dtpDate.Tag);
            m_lnqLoadingInfo.CVTNumber = txtCVTNumber.Text.Trim();
            m_lnqLoadingInfo.Date = dtpDate.Value.Date;
            m_lnqLoadingInfo.ProductID = Convert.ToInt32(txtProduct.Tag);
            m_lnqLoadingInfo.Remark = txtRemark.Text;
            m_lnqLoadingInfo.VehicleShelfNumber = txtVehicleShelfNumber.Text;
            m_lnqLoadingInfo.CarModelID = m_serverProductList.GetMotorcycleType(cmbCarModel.Text);
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        void ClearDate()
        {
            txtCVTNumber.Text = "";
            txtProduct.Text = "";
            txtRemark.Text = "";
            txtVehicleShelfNumber.Text = "";
            txtProduct.Tag = null;
            dtpDate.Value = ServerTime.Time;
            dtpDate.Tag = null;

        }

        void txtProductCode_OnCompleteSearch()
        {
            txtProduct.Text = txtProduct.DataResult["物品名称"].ToString();
            txtProduct.Tag = Convert.ToInt32(txtProduct.DataResult["序号"]);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            GetMessage();

            if (!m_serverCVTLoading.InsertIntoLoadingInfo(m_lnqLoadingInfo, out m_strError))
            {
                MessageDialog.ShowPromptMessage(m_strError);
                return;
            }
            else
            {
                MessageDialog.ShowPromptMessage("添加成功");
            }

            ClearDate();
            RefreshDataGirdView(m_serverCVTLoading.GetLoadingInfo());
        }

        private void btnOutExcel_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshDataGirdView(m_serverCVTLoading.GetLoadingInfo());
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            GetMessage();

            if (!m_serverCVTLoading.UpdateLoadingInfo(m_lnqLoadingInfo, out m_strError))
            {
                MessageDialog.ShowPromptMessage(m_strError);
                return;
            }
            else
            {
                MessageDialog.ShowPromptMessage("修改成功");
            }

            ClearDate();
            RefreshDataGirdView(m_serverCVTLoading.GetLoadingInfo());
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            GetMessage();

            if (!m_serverCVTLoading.DeleteLoadingInfo(m_lnqLoadingInfo, out m_strError))
            {
                MessageDialog.ShowPromptMessage(m_strError);
                return;
            }
            else
            {
                MessageDialog.ShowPromptMessage("删除成功");
            }

            ClearDate();
            RefreshDataGirdView(m_serverCVTLoading.GetLoadingInfo());
        }

        /// <summary>
        /// 检测数据集
        /// </summary>
        /// <param name="checkList">需要检测的数据集</param>
        /// <returns>通过返回True，不通过返回False</returns>
        bool CheckTable(DataTable checkList)
        {
            if (!checkList.Columns.Contains("装车日期"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【装车日期】信息");
                return false;
            }

            if (!checkList.Columns.Contains("车架号"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【销售日期】信息");
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

            if (!checkList.Columns.Contains("备注"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【备注】信息");
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

            dtpDate.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["装车日期"].Value).Date;
            dtpDate.Tag = Convert.ToInt32(dataGridView1.CurrentRow.Cells["序号"].Value);
            txtCVTNumber.Text = dataGridView1.CurrentRow.Cells["CVT编号"].Value.ToString();
            txtProduct.Text = dataGridView1.CurrentRow.Cells["CVT型号"].Value.ToString();
            txtRemark.Text = dataGridView1.CurrentRow.Cells["备注"].Value.ToString();
            txtVehicleShelfNumber.Text = dataGridView1.CurrentRow.Cells["车架号"].Value.ToString();
            txtProduct.Tag = dataGridView1.CurrentRow.Cells["物品ID"].Value.ToString();
            cmbCarModel.Text = dataGridView1.CurrentRow.Cells["车型号"].Value.ToString();
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(this.labelTitle.Text, e.Column);
        }

        private void btnLoadExcel_Click(object sender, EventArgs e)
        {
            DataTable dtTemp = ExcelHelperP.RenderFromExcel(openFileDialog1);

            if (dtTemp == null)
            {
                //MessageDialog.ShowPromptMessage(m_strError);
                return;
            }

            if (CheckTable(dtTemp))
            {
                if (!m_serverCVTLoading.BatchInsertLoadingInfo(dtTemp, out m_strError))
                {
                    MessageDialog.ShowPromptMessage(m_strError);
                    return;
                }
                else
                {
                    string strMessage = "";

                    if (m_strError != null)
                    {
                        strMessage = "\r\n以下车架号的记录日期格式不正确"
                            + m_strError + "\r\n系统已默认为【2099-12-31】，请自行修改";
                    }

                    MessageDialog.ShowPromptMessage("导入成功" + strMessage);
                }
            }

            ClearDate();
            RefreshDataGirdView(m_serverCVTLoading.GetLoadingInfo());
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

        private void CVT装车信息_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authFlag);
        }
    }
}
