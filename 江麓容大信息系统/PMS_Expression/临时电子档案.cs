using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using PlatformManagement;
using System.Reflection;
using UniversalControlLibrary;

namespace Expression
{
    public partial class 临时电子档案 : Form
    {
        /// <summary>
        /// 临时电子档案服务
        /// </summary>
        ITempElectronFileServer m_tefServer = ServerModuleFactory.GetServerModule<ITempElectronFileServer>();

        //Func<View_P_TempElectronFile, bool> m_funWhere;
        //Func<View_P_TempElectronFile, string> m_funOrder;

        /// <summary>
        /// 数据结果
        /// </summary>
        IEnumerable<View_P_TempElectronFile> m_dataResult = null;

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_error;

        /// <summary>
        /// 检索模式(字段检索，时间范围)
        /// </summary>
        enum SearchMode { Field, TimeRange }

        /// <summary>
        /// 当前检索模式
        /// </summary>
        SearchMode m_searchMode;

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag = AuthorityFlag.Nothing;

        public 临时电子档案(FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authFlag = nodeInfo.Authority;

            foreach (PropertyInfo item in typeof(View_P_TempElectronFile).GetProperties())
            {
                if (!item.Name.Contains("时间"))
                {
                    cmbDataName.Items.Add(item.Name);
                }
            }

            winFormPage1.PageSize = 2000;
            winFormPage1.RefreshData = new GlobalObject.DelegateCollection.NonArgumentHandle(this.GoToPage);
        }

        private void 临时电子档案_Load(object sender, EventArgs e)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip1, m_authFlag);
        }

        void GoToPage()
        {
            IEnumerable<View_P_TempElectronFile> result = m_tefServer.GetEnumerable<View_P_TempElectronFile>(m_dataResult, null, null, winFormPage1.PageSize, winFormPage1.PageIndex);
            dataGridView1.DataSource = GlobalObject.GeneralFunction.ConvertToDataTable<View_P_TempElectronFile>(result);

            userControlDataLocalizer1.Init(dataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, GlobalObject.BasicInfo.LoginID));
        }

        private void btnSearch1_Click(object sender, EventArgs e)
        {
            if (cmbDataName.SelectedIndex < 0)
            {
                MessageDialog.ShowPromptMessage("请选择要检索的数据后再进行此操作");
                cmbDataName.Focus();
                return;
            }

            if (txtDataValue.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请输入要检索的数据后再进行此操作");
                txtDataValue.Focus();
                return;
            }

            SearchData1();
        }

        /// <summary>
        /// 检索数据
        /// </summary>
        void SearchData1()
        {
            m_dataResult = m_tefServer.GetData(cmbDataName.Text, txtDataValue.Text);
            winFormPage1.Count = m_dataResult.Count();

            if (m_dataResult.Count() != 0)
            {
                winFormPage1.PageIndex = 1;
            }
            else
            {
                MessageDialog.ShowPromptMessage("没有查找到所需的数据");

                if (dataGridView1.Rows.Count == 0)
                    return;
            }

            m_searchMode = SearchMode.Field;

            IEnumerable<View_P_TempElectronFile> result = m_tefServer.GetEnumerable<View_P_TempElectronFile>(m_dataResult, null, null, winFormPage1.PageSize, 1);
            RefreshDataGridView(GlobalObject.GeneralFunction.ConvertToDataTable<View_P_TempElectronFile>(result));
        }

        private void btnSearch2_Click(object sender, EventArgs e)
        {
            m_dataResult = m_tefServer.GetData(dateTimePickerST.Value, dateTimePickerET.Value);
            winFormPage1.Count = m_dataResult.Count();

            if (m_dataResult.Count() != 0)
            {
                winFormPage1.PageIndex = 1;
            }
            else
            {
                MessageDialog.ShowPromptMessage("没有查找到所需的数据");

                if (dataGridView1.Rows.Count == 0)
                    return;
            }

            m_searchMode = SearchMode.TimeRange;

            IEnumerable<View_P_TempElectronFile> result = m_tefServer.GetEnumerable<View_P_TempElectronFile>(m_dataResult, null, null, winFormPage1.PageSize, 1);
            RefreshDataGridView(GlobalObject.GeneralFunction.ConvertToDataTable<View_P_TempElectronFile>(result));
        }

        private void RefreshDataGridView(DataTable dt)
        {
            dataGridView1.DataSource = dt;

            this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridView1_ColumnWidthChanged);
            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);
            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridView1_ColumnWidthChanged);

            userControlDataLocalizer1.Init(dataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, GlobalObject.BasicInfo.LoginID));
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

        private void 临时电子档案_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(this.labelTitle.Text, e.Column);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择要删除行后再进行此操作");
                return;
            }

            List<string> lstGuid = new List<string>(dataGridView1.SelectedRows.Count);

            for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
            {
                DataGridViewCellCollection cells = dataGridView1.SelectedRows[i].Cells;
                lstGuid.Add(cells["序号"].Value.ToString());
            }

            if (!m_tefServer.Delete(lstGuid, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            if (m_searchMode == SearchMode.Field)
                SearchData1();
            else
                btnSearch2_Click(sender, e);
        }

        private void btnSearches_Click(object sender, EventArgs e)
        {
            IAuthorization authorization = PlatformFactory.GetObject<IAuthorization>();
            string businessID = "临时电子档案查询";
            IQueryResult qr = authorization.Query(businessID, null, null, 0);
            List<string> lstFindField = new List<string>();
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

        /// <summary>
        /// 更新零件标识码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdateOnlyCode_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择要修改行后再进行此操作");
                return;
            }

            // 获取新零件标识码
            string newOnlyCode = InputBox.ShowDialog("录入零件标识码", "零件标识码", "");

            DataGridViewCellCollection cells = dataGridView1.SelectedRows[0].Cells;

            if (m_tefServer.UpdateOnlyCode(cells["序号"].Value.ToString(), cells["零部件名称"].Value.ToString(), newOnlyCode, out m_error))
            {
                MessageDialog.ShowPromptMessage("更新成功");

                if (m_searchMode == SearchMode.Field)
                    SearchData1();
                else
                    btnSearch2_Click(sender, e);
            }
            else
            {
                MessageDialog.ShowErrorMessage(m_error);
            }
        }

        private void btnUpdateParentScanCode_Click(object sender, EventArgs e)
        {
            Form改变临时档案分总成编号 form = new Form改变临时档案分总成编号();
            form.ShowDialog();
        }
    }
}
