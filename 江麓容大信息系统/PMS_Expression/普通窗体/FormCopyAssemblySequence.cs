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
    /// 复制装配顺序
    /// </summary>
    public partial class FormCopyAssemblySequence : Form
    {
        /// <summary>
        /// 复制模式枚举
        /// </summary>
        public enum CopyModeEnum
        {
            复制整个产品装配顺序,
            复制指定工位装配顺序
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
        /// 服务组件
        /// </summary>
        IWorkbenchService m_workbenchServer = ServerModuleFactory.GetServerModule<IWorkbenchService>();
        
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
        /// 目标产品信息
        /// </summary>
        View_P_ProductInfo m_targetProductInfo;

        /// <summary>
        /// 目标产品信息
        /// </summary>
        public View_P_ProductInfo TargetProductInfo
        {
            get { return m_targetProductInfo; }
        }

        /// <summary>
        /// 工位
        /// </summary>
        public string Workbench
        {
            get
            {
                return cmbWorkbench.Text;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="copyMode">复制模式</param>
        public FormCopyAssemblySequence(CopyModeEnum copyMode)
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
                    cmbTargetProductType.Items.Add(item.产品类型编码);
                }

                cmbSourceProductType.SelectedIndex = 0;
                cmbTargetProductType.SelectedIndex = 1;
            }

            #endregion

            if (copyMode == CopyModeEnum.复制整个产品装配顺序)
            {
                cmbWorkbench.Enabled = false;
            }
            else
            {
                cmbWorkbench.Enabled = true;
            }

            #region 获取工位
            IQueryable<View_P_Workbench> workbench = m_workbenchServer.Workbenchs;

            if (workbench.Count() > 0)
            {
                cmbWorkbench.Items.AddRange((from r in workbench select r.工位).ToArray());
            }
            else
            {
                MessageDialog.ShowErrorMessage("没有获取到工位信息");
                btnOK.Enabled = false;
            }
            #endregion
        }

        private void cmbSourceProductType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSourceProductName.Text = (from r in m_productInfo
                                         where r.产品类型编码 == cmbSourceProductType.Text
                                         select r.产品类型名称).First();

            if (m_copyMode == CopyModeEnum.复制指定工位装配顺序)
            {
                cmbWorkbench.Items.Clear();
                cmbWorkbench.Items.AddRange(
                    ServerModuleFactory.GetServerModule<IAssemblingBom>().GetWorkbenchs(cmbSourceProductType.Text));
            }
        }

        private void cmbTargetProductType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtTargetProductName.Text = (from r in m_productInfo
                                         where r.产品类型编码 == cmbTargetProductType.Text
                                         select r.产品类型名称).First();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (cmbSourceProductType.SelectedIndex == -1)
            {
                cmbSourceProductType.Focus();
                MessageDialog.ShowErrorMessage("产品类型不允许为空");
                return;
            }

            if (cmbTargetProductType.SelectedIndex == -1)
            {
                cmbTargetProductType.Focus();
                MessageDialog.ShowErrorMessage("产品类型不允许为空");
                return;
            }

            if (cmbSourceProductType.Text == cmbTargetProductType.Text)
            {
                cmbTargetProductType.Focus();
                MessageDialog.ShowErrorMessage("源产品类型不允许和目标产品类型相同");
                return;
            }

            if (m_copyMode == CopyModeEnum.复制指定工位装配顺序 && cmbWorkbench.SelectedIndex == -1)
            {
                cmbWorkbench.Focus();
                MessageDialog.ShowErrorMessage("总成名称不允许为空");
                return;
            }

            m_sourceProductInfo = m_productInfo.First(p => p.产品类型编码 == cmbSourceProductType.Text);
            m_targetProductInfo = m_productInfo.First(p => p.产品类型编码 == cmbTargetProductType.Text);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
