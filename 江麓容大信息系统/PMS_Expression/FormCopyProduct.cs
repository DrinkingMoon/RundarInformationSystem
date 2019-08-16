using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using Expression;
using UniversalControlLibrary;

namespace Expression
{
    public partial class FormCopyProduct : Form
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
        /// 一次性物料清单
        /// </summary>
        IDisposableGoodsServer m_disposeGoodsServer = ServerModuleFactory.GetServerModule<IDisposableGoodsServer>();

        public FormCopyProduct()
        {
            InitializeComponent();

            IQueryable<View_P_ProductInfo> productInfo = null;

            if (!m_productInfoServer.GetAllProductInfo(out productInfo, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
            }
            else
            {
                foreach (var item in productInfo)
                {
                    if (!item.是否返修专用)
                    {
                        cmbProduct.Items.Add(item.产品类型编码);
                        cmbCopyProduct.Items.Add(item.产品类型编码);
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cmbCopyProduct.Text == "" || cmbProduct.Text == "")
            {
                MessageDialog.ShowPromptMessage("请选择复制的产品型号和复制给某个产品的产品型号！");
                return;
            }

            if (!m_disposeGoodsServer.InsertBatchData(cmbCopyProduct.Text, cmbProduct.Text, out m_error))
            {
                MessageDialog.ShowPromptMessage(m_error);
            }
            else
            {
                MessageDialog.ShowPromptMessage("复制成功！");
            }

            this.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
