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
    public partial class 拷贝电子档案信息 : Form
    {
        /// <summary>
        /// 产品信息服务
        /// </summary>
        private IProductInfoServer m_productInfoServer = ServerModuleFactory.GetServerModule<IProductInfoServer>();

        /// <summary>
        /// 电子档案服务组件
        /// </summary>
        private IElectronFileServer m_electronFileServer = ServerModuleFactory.GetServerModule<IElectronFileServer>();

        /// <summary>
        /// 错误信息
        /// </summary>
        private string m_error;

        public 拷贝电子档案信息()
        {
            InitializeComponent();

            string[] productType = null;

            if (!m_productInfoServer.GetAllProductType(out productType, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            List<string> lstProductType = productType.ToList();

            lstProductType.RemoveAll(p => p.Contains(" FX"));

            cmbCVTType.Items.Clear();

            cmbCVTType.Items.AddRange(lstProductType.ToArray());

            cmbCVTType.SelectedIndex = 0;
        }

        /// <summary>
        /// 根据装配BOM生成电子档案信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (!ServerModuleFactory.GetServerModule<IProductCodeServer>().VerifyProductCodesInfo(
                cmbCVTType.Text, txtCVTNumber.Text, GlobalObject.CE_BarCodeType.内部钢印码, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                txtCVTNumber.Focus();
                return;
            }

            string newProductCode = cmbCVTType.Text + " " + txtCVTNumber.Text;

            if (!m_electronFileServer.GenerateElectronFile(cmbCVTType.Text, newProductCode, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }
            else
            {
                MessageDialog.ShowPromptMessage("操作成功");
            }
        }
    }
}
