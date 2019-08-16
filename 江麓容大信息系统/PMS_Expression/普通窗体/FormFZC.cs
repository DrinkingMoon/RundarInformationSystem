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
using GlobalObject;

namespace Expression
{
    public partial class FormFZC : Form
    {
        /// <summary>
        /// 防错服务组件
        /// </summary>
        IPreventErrorServer m_preventErrorServer = PMS_ServerFactory.GetServerModule<IPreventErrorServer>();

        /// <summary>
        /// 数据定位控件
        /// </summary>
        UserControlDataLocalizer m_dataLocalizer;

        /// <summary>
        /// 型号列表
        /// </summary>
        List<View_P_AssemblingBom> m_lstAllProduct;

        /// <summary>
        /// 选择的型号
        /// </summary>
        List<View_P_AssemblingBom> m_selectedProduct;

        /// <summary>
        /// 总行数
        /// </summary>
        int m_count;

        /// <summary>
        /// 所有型号
        /// </summary>
        internal List<View_P_AssemblingBom> AllProduct
        {
            get { return m_lstAllProduct; }
            set { m_lstAllProduct = value; }
        }

        /// <summary>
        /// 获取或设置选择的型号
        /// </summary>
        internal List<View_P_AssemblingBom> SelectedProduct
        {
            get { return m_selectedProduct; }
            set { m_selectedProduct = value; }
        }

        /// <summary>
        /// 获取或设置产品型号行数
        /// </summary>
        internal int ProductCount
        {
            get { return m_count; }
            set { m_count = value; }
        }

        string m_productType;

        public FormFZC(string productType)
        {
            InitializeComponent();
            m_productType = productType;
        }

        /// <summary>
        /// 点击选择按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectField_Click(object sender, EventArgs e)
        {
            int index = 0;
            m_selectedProduct = new List<View_P_AssemblingBom>();

            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                if ((bool)item.Cells[0].Value)
                {
                    View_P_AssemblingBom user = (from r in m_lstAllProduct 
                                               where r.父总成名称 == item.Cells[1].Value.ToString() select r).Single();
                    m_selectedProduct.Add(user);
                }

                index++;
            }

            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// 用于确定数据控件中的检查框值已经更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView1.IsCurrentCellDirty)
            {
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

         /// <summary>
        /// 选择所有
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectAll_Click(object sender, EventArgs e)
        {          
            SelectDataRow(true);
        }

        /// <summary>
        /// 根据参数选中界面上的数据
        /// </summary>
        /// <param name="selectedFlag">为真则选中所有数据，为假则清除所有选择</param>
        private void SelectDataRow(bool selectedFlag)
        {
            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                if (item.Visible)
                {
                    item.Cells["选中"].Value = selectedFlag;
                }
            }

            if (dataGridView1.CurrentRow != null)
                dataGridView1.CurrentCell = dataGridView1.CurrentRow.Cells[1];
        }

        /// <summary>
        /// 清除所有选中行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearAll_Click(object sender, EventArgs e)
        {
            SelectDataRow(false);
        }

        /// <summary>
        /// 显示仅选中行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnShowSelectedData_Click(object sender, EventArgs e)
        {
            ShowSelectedDataRow(true);
        }

        /// <summary>
        /// 根据参数显示界面上的数据
        /// </summary>
        /// <param name="selectedFlag">为真则显示所有选中数据，为假则显示所有未选中数据</param>
        private void ShowSelectedDataRow(bool selectedFlag)
        {
            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                if ((bool)item.Cells["选中"].Value)
                {
                    item.Visible = selectedFlag;
                }
                else
                {
                    item.Visible = !selectedFlag;
                }
            }
        }

        /// <summary>
        /// 仅显示未选中信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnShowUnselectedData_Click(object sender, EventArgs e)
        {
            ShowSelectedDataRow(false);
        }

        private void FormFZC_Load(object sender, EventArgs e)
        {
            try
            {
                if (AllProduct == null)
                {
                    List<View_P_AssemblingBom> list = new List<View_P_AssemblingBom>();

                    DataTable dt = m_preventErrorServer.GetAllAssemblingBom(m_productType);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            View_P_AssemblingBom assembling = new View_P_AssemblingBom();

                            assembling.父总成名称 = dt.Rows[i]["分总成名称"].ToString();

                            list.Add(assembling);
                        }
                    }

                    AllProduct = list;
                }

                DataGridViewCheckBoxColumn column = new DataGridViewCheckBoxColumn();

                column.Visible = true;
                column.Name = "选中";
                column.HeaderText = "选中";
                column.ReadOnly = false;

                dataGridView1.Columns.Add(column);

                dataGridView1.Columns.Add("分总成名称", "分总成名称");

                foreach (DataGridViewColumn item in dataGridView1.Columns)
                {
                    if (item.Name != "选中")
                    {
                        item.ReadOnly = true;
                        item.Width = item.HeaderText.Length * (int)this.Font.Size + 100;
                    }
                    else
                    {
                        item.Width = 68;
                        item.ReadOnly = false;
                        item.Frozen = false;
                    }
                }

                bool selectedFlag = false;
                int count = 0;

                foreach (var item in AllProduct)
                {
                    selectedFlag = false;

                    if (SelectedProduct != null && count < SelectedProduct.Count)
                    {
                        if (SelectedProduct.FindIndex(c => c.父总成名称 == item.父总成名称) >= 0)
                        {
                            selectedFlag = true;
                            count++;
                        }
                    }

                    dataGridView1.Rows.Add(new object[] { selectedFlag, item.父总成名称});
                }

                m_count = dataGridView1.Rows.Count;

                if (m_dataLocalizer == null)
                {
                    m_dataLocalizer = new UserControlDataLocalizer(dataGridView1, this.Name,
                        UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));

                    panelTop.Controls.Add(m_dataLocalizer);

                    m_dataLocalizer.Dock = DockStyle.Bottom;
                }
            }
            catch (Exception err)
            {
                MessageDialog.ShowErrorMessage(err.Message);
            }
        }
    }
}
