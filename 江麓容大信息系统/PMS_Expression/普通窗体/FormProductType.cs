/******************************************************************************
 *
 * 文件名称:  FormProductType.cs
 * 作者    :  邱瑶       日期: 2014/03/20
 * 开发平台:  vs2008(c#)
 * 用于    :  装配线管理信息系统
 ******************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UniversalControlLibrary;
using ServerModule;
using GlobalObject;

namespace Expression
{
    /// <summary>
    /// 用于多选产品型号的窗体
    /// </summary>
    public partial class FormProductType : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_error;

        /// <summary>
        /// 产品信息管理服务组件
        /// </summary>
        IProductInfoServer m_productInfoServer = ServerModuleFactory.GetServerModule<IProductInfoServer>();

        /// <summary>
        /// 数据定位控件
        /// </summary>
        UserControlDataLocalizer m_dataLocalizer;

        /// <summary>
        /// 型号列表
        /// </summary>
        List<View_P_ProductInfo> m_lstAllProduct;

        /// <summary>
        /// 选择的型号
        /// </summary>
        List<View_P_ProductInfo> m_selectedProduct;

        /// <summary>
        /// 总行数
        /// </summary>
        int m_count;

        /// <summary>
        /// 所有型号
        /// </summary>
        internal List<View_P_ProductInfo> AllProduct
        {
            get { return m_lstAllProduct; }
            set { m_lstAllProduct = value; }
        }

        /// <summary>
        /// 获取或设置选择的型号
        /// </summary>
        internal List<View_P_ProductInfo> SelectedProduct
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

        public FormProductType()
        {
            InitializeComponent();
        }

        private void FormProductType_Load(object sender, EventArgs e)
        {
            AllProduct = null;
            dataGridView1.Rows.Clear();

            try
            {
                if (AllProduct == null)
                {
                    IQueryable<View_P_ProductInfo> productInfo = null;

                    if (!m_productInfoServer.GetAllProductInfo(out productInfo, out m_error))
                    {
                        MessageDialog.ShowErrorMessage(m_error);
                    }

                    AllProduct = productInfo.ToList();
                }

                DataGridViewCheckBoxColumn column = new DataGridViewCheckBoxColumn();

                column.Visible = true;
                column.Name = "选中";
                column.HeaderText = "选中";
                column.ReadOnly = false;

                dataGridView1.Columns.Add(column);

                dataGridView1.Columns.Add("序号", "序号");
                dataGridView1.Columns.Add("产品类型编码", "产品类型编码");
                dataGridView1.Columns.Add("产品类型名称", "产品类型名称");
                dataGridView1.Columns.Add("产品装配简码", "产品装配简码");
                dataGridView1.Columns.Add("是否返修专用", "是否返修专用");
                dataGridView1.Columns.Add("备注", "备注");

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
                        if (SelectedProduct.FindIndex(c => c.产品类型编码 == item.产品类型编码) >= 0)
                        {
                            selectedFlag = true;
                            count++;
                        }
                    }

                    dataGridView1.Rows.Add(new object[] { selectedFlag, item.序号, item.产品类型编码, item.产品类型名称, 
                        item.产品装配简码, item.是否返修专用, item.备注 });
                }

                m_count = dataGridView1.Rows.Count;

                if (m_dataLocalizer == null)
                {
                    m_dataLocalizer = new UserControlDataLocalizer(dataGridView1, this.Name,
                        UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));

                    panelTop.Controls.Add(m_dataLocalizer);

                    m_dataLocalizer.Dock = DockStyle.Bottom;
                }

                dataGridView1.Columns["序号"].Visible = false;
            }
            catch (Exception err)
            {
                MessageDialog.ShowErrorMessage(err.Message);
            }
        }

        /// <summary>
        /// 点击选择按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectField_Click(object sender, EventArgs e)
        {
            int index = 0;
            m_selectedProduct = new List<View_P_ProductInfo>();

            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                if ((bool)item.Cells[0].Value)
                {
                    View_P_ProductInfo user = (from r in m_lstAllProduct 
                                               where r.产品类型编码 == item.Cells[2].Value.ToString() 
                                               select r).Single();
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
                dataGridView1.CurrentCell = dataGridView1.CurrentRow.Cells[2];
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

        private void 剔除toolStripButton_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            AllProduct = null;

            try
            {
                if (AllProduct == null)
                {
                    IQueryable<View_P_ProductInfo> productInfo = null;

                    if (!m_productInfoServer.GetRemovedTCU(out productInfo, out m_error))
                    {
                        MessageDialog.ShowErrorMessage(m_error);
                    }

                    AllProduct = productInfo.ToList();
                }

                DataGridViewCheckBoxColumn column = new DataGridViewCheckBoxColumn();

                column.Visible = true;
                column.Name = "选中";
                column.HeaderText = "选中";
                column.ReadOnly = false;

                dataGridView1.Columns.Add(column);

                dataGridView1.Columns.Add("序号", "序号");
                dataGridView1.Columns.Add("产品类型编码", "产品类型编码");
                dataGridView1.Columns.Add("产品类型名称", "产品类型名称");
                dataGridView1.Columns.Add("产品装配简码", "产品装配简码");
                dataGridView1.Columns.Add("是否返修专用", "是否返修专用");
                dataGridView1.Columns.Add("备注", "备注");

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
                        if (SelectedProduct.FindIndex(c => c.产品类型编码 == item.产品类型编码) >= 0)
                        {
                            selectedFlag = true;
                            count++;
                        }
                    }

                    dataGridView1.Rows.Add(new object[] { selectedFlag, item.序号, item.产品类型编码, item.产品类型名称, 
                        item.产品装配简码, item.是否返修专用, item.备注 });
                }

                m_count = dataGridView1.Rows.Count;

                if (m_dataLocalizer == null)
                {
                    m_dataLocalizer = new UserControlDataLocalizer(dataGridView1, this.Name,
                        UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));

                    panelTop.Controls.Add(m_dataLocalizer);

                    m_dataLocalizer.Dock = DockStyle.Bottom;
                }

                dataGridView1.Columns["序号"].Visible = false;
            }
            catch (Exception err)
            {
                MessageDialog.ShowErrorMessage(err.Message);
            }
        }

        private void 显示所有toolStripButton_Click(object sender, EventArgs e)
        {
            FormProductType_Load(null,null);
        }
    }
}
