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
    /// <summary>
    /// 复制产品信息
    /// </summary>
    public partial class FormCopyProductInfo : Form
    {
        /// <summary>
        /// 复制模式枚举
        /// </summary>
        public enum CopyModeEnum
        {
            复制整个产品零件信息,
            复制分总成下属零件信息
        }

        /// <summary>
        /// 复制模式
        /// </summary>
        CopyModeEnum m_copyMode;

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 产品信息服务组件
        /// </summary>
        IProductInfoServer m_productInfoServer = ServerModuleFactory.GetServerModule<IProductInfoServer>();

        /// <summary>
        /// 产品信息
        /// </summary>
        IQueryable<View_P_ProductInfo> m_productInfo;

        /// <summary>
        /// 源产品信息
        /// </summary>
        View_P_ProductInfo m_sourceProductInfo;

        /// <summary>
        /// 源产品信息
        /// </summary>
        public View_P_ProductInfo SourceProductInfo
        {
            get { return m_sourceProductInfo; }
        }

        /// <summary>
        /// 选择的产品信息
        /// </summary>
        public List<View_P_ProductInfo> m_queryProductInfo;

        /// <summary>
        /// 目标产品信息
        /// </summary>
        View_P_ProductInfo m_targetProductInfo = new View_P_ProductInfo();

        /// <summary>
        /// 目标产品信息
        /// </summary>
        public View_P_ProductInfo TargetProductInfo
        {
            get { return m_targetProductInfo; }
        }

        /// <summary>
        /// 分总成名称
        /// </summary>
        public string ParentName
        {
            get
            {
                if (txtSourceProductName.Text == cmbParentName.Text)
                {
                    return cmbParentName.Text + "(返修)";
                }

                return cmbParentName.Text;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="copyMode">复制模式</param>
        public FormCopyProductInfo(CopyModeEnum copyMode)
        {
            InitializeComponent();            
           
            m_copyMode = copyMode;

            #region 获取所有产品编码(产品类型)信息

            if (!m_productInfoServer.GetAllProductInfo(out m_productInfo, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                btnOK.Enabled = false;
                return;
            }

            if (m_productInfo != null)
            {
                foreach (var item in m_productInfo)
                {
                    cmbSourceProductType.Items.Add(item.产品类型编码);
                    //cmbTargetProductType.Items.Add(item.产品类型编码);
                }

                cmbSourceProductType.SelectedIndex = 0;
                //cmbTargetProductType.SelectedIndex = 1;
            }

            #endregion

            if (copyMode == CopyModeEnum.复制整个产品零件信息)
            {
                cmbParentName.Enabled = false;
            }
            else
            {
                cmbParentName.Enabled = true;
            }
        }

        private void cmbSourceProductType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSourceProductName.Text = (from r in m_productInfo
                                         where r.产品类型编码 == cmbSourceProductType.Text
                                         select r.产品类型名称).First();

            if (m_copyMode == CopyModeEnum.复制分总成下属零件信息)
            {
                cmbParentName.Items.Clear();
                cmbParentName.Items.AddRange(
                    ServerModuleFactory.GetServerModule<IAssemblingBom>().GetParentNames(cmbSourceProductType.Text));
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (cmbSourceProductType.SelectedIndex == -1)
            {
                cmbSourceProductType.Focus();
                MessageDialog.ShowErrorMessage("产品类型不允许为空");
                return;
            }

            if (txtTargetProductType.Text.Trim() == "")
            {
                txtTargetProductType.Focus();
                MessageDialog.ShowErrorMessage("产品类型不允许为空");
                return;
            }

            if (txtTargetProductType.Text.Contains(cmbSourceProductType.Text))
            {
                txtTargetProductType.Focus();
                MessageDialog.ShowErrorMessage("源产品类型不允许和目标产品类型相同");
                return;
            }

            if (m_copyMode == CopyModeEnum.复制分总成下属零件信息 && cmbParentName.SelectedIndex == -1)
            {
                cmbParentName.Focus();
                MessageDialog.ShowErrorMessage("总成名称不允许为空");
                return;
            }

            m_sourceProductInfo = m_productInfo.First(p => p.产品类型编码 == cmbSourceProductType.Text);
            m_targetProductInfo = m_productInfo.First(p => p.产品类型编码 == txtTargetProductType.Text);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnFindProduct_Click(object sender, EventArgs e)
        {
            FormProductType form = new FormProductType();
            string productStr = "";

            if (form.ShowDialog() == DialogResult.OK)
            {
                List<View_P_ProductInfo> productList = form.SelectedProduct;
                m_queryProductInfo = productList;

                if (form.SelectedProduct.Count != form.ProductCount)
                {
                    foreach (View_P_ProductInfo item in productList)
                    {
                        productStr += item.产品类型编码 + ",";
                    }

                    productStr = productStr.Substring(0, productStr.Length - 1);
                }
                else
                {
                    productStr = "全部";
                }
            }

            txtTargetProductType.Text = productStr;
        }
    }
}
