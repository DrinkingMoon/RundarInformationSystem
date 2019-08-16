/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  FormPrintAssemblyBarCodeForRepeatedMode.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2009/06/15
 * 开发平台:  vs2005(c#)
 * 用于    :  生产线管理信息系统
 *----------------------------------------------------------------------------
 * 描述 : 关于界面
 * 其它 :
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2009/07/03 08:02:08 作者: 夏石友 当前版本: V1.00
 *        修改说明: 创建
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using PlatformManagement;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 打印总成条形码界面（重复使用模式）
    /// </summary>
    public partial class FormPrintAssemblyBarCodeForRepeatedMode : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 服务组件
        /// </summary>
        IProductInfoServer m_productInfoServer = ServerModuleFactory.GetServerModule<IProductInfoServer>();

        /// <summary>
        /// 装配线条形码管理组件接口
        /// </summary>
        IBarcodeForAssemblyLine m_barCode = BarcodeFactoryForAssemblyLine.Instance;

        /// <summary>
        /// 产品信息
        /// </summary>
        IQueryable<View_P_ProductInfo> m_productInfo = null;

        /// <summary>
        /// 产品装配简码
        /// </summary>
        string m_productTypeCode;

        /// <summary>
        /// 独立分装类别：阀块、行星轮合件
        /// </summary>
        string m_assemblyType;

        /// <summary>
        /// 通信组件
        /// </summary>
        CommResponseServer m_commResponseServer;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="assemblyType">独立分装类别：阀块、行星轮合件</param>
        /// <param name="commServer"></param>
        public FormPrintAssemblyBarCodeForRepeatedMode(CommResponseServer commServer)
        {
            InitializeComponent();

            m_commResponseServer = commServer;

            m_productInfoServer.GetAllProductInfo(out m_productInfo, out m_err);

            this.cmbPrductType.SelectedIndexChanged -= new System.EventHandler(this.cmbPrductType_SelectedIndexChanged);

            cmbPrductType.DataSource = m_productInfo;

            this.cmbPrductType.SelectedIndexChanged += new System.EventHandler(this.cmbPrductType_SelectedIndexChanged);

            cmbPrductType.DisplayMember = "产品类型编码";
            cmbPrductType.ValueMember = "产品类型编码";
            cmbPrductType.SelectedIndex = 0;

            m_productTypeCode = cmbPrductType.SelectedValue.ToString();

            lblProductName.Text = (cmbPrductType.SelectedItem as View_P_ProductInfo).产品类型名称;

            InitAssemblyName();
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            List<string> childAssembly = new List<string>();
            int printCount = (int)(numCount.Value);
            BatchBarcodeInf batchInfo = null;

            string error;
            int beginSerialNumber = 0;

            try
            {
                beginSerialNumber = Convert.ToInt32(txtSerialNo.Text);
            }
            catch (Exception)
            {
                MessageDialog.ShowErrorMessage("不正确的流水号！");
                return;
            }

            if (!m_barCode.BatchGenerateBarcodeForRepeatedMode(
                m_productTypeCode, m_assemblyType, printCount, beginSerialNumber, out batchInfo, out error))
            {
                MessageDialog.ShowPromptMessage(error);
                return;
            }

            if (!m_barCode.PrintBarcodes(batchInfo, out error))
            {
                MessageDialog.ShowErrorMessage(error);
            }
        }

        /// <summary>
        ///  初始化总成名称
        /// </summary>
        void InitAssemblyName()
        {
            List<string> names = new List<string>();

            m_barCode.GetAssemblyName(m_productTypeCode, names, out m_err);

            // 移除产品总成记录
            names.Remove(lblProductName.Text);

            cmbAssemblyName.Items.Clear();
            cmbAssemblyName.Items.AddRange(names.ToArray());
            cmbAssemblyName.SelectedIndex = 0;
        }

        private void cmbPrductType_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_productTypeCode = cmbPrductType.SelectedValue.ToString();

            lblProductName.Text = (cmbPrductType.SelectedItem as View_P_ProductInfo).产品类型名称;

            InitAssemblyName();
        }

        private void cmbAssemblyName_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_assemblyType = cmbAssemblyName.Text;
        }
    }
}
