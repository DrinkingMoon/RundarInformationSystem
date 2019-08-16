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
using PlatformManagement;

namespace Expression
{
    public partial class FormAssemblyMain : Form
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
        /// 通信组件
        /// </summary>
        CommResponseServer m_commResponseServer;

        /// <summary>
        /// 前景色
        /// </summary>
        Color m_foreColor;

        /// <summary>
        /// 功能树节点信息
        /// </summary>
        PlatformManagement.FunctionTreeNodeInfo m_nodeInfo;

        public FormAssemblyMain(FunctionTreeNodeInfo nodeInfo, CommResponseServer commResponseServer)
        {
            InitializeComponent();

            m_commResponseServer = commResponseServer;

            m_nodeInfo = nodeInfo;

            IQueryable<View_P_ProductInfo> productTypeTable = null;

            if (!m_productInfoServer.GetAllProductInfo(out productTypeTable, out m_err))
            {
                阀块装配.Enabled = false;
                行星轮合件装配.Enabled = false;

                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            // 产品类型字典
            Dictionary<string, string> productTypeDic = new Dictionary<string, string>();

            foreach (var item in productTypeTable)
            {
                if (productTypeDic.ContainsKey(item.产品类型名称))
                {
                    productTypeDic[item.产品类型名称] = item.产品类型编码;
                }
                else
                {
                    productTypeDic.Add(item.产品类型名称, item.产品类型编码);
                }
            }

            m_commResponseServer.ProductTypeDic = productTypeDic;
        }

        private void FormAssemblyMain_Load(object sender, EventArgs e)
        {
            FaceAuthoritySetting.SetEnable(this.Controls, m_nodeInfo.Authority);
        }

        private void 整台装配_Click(object sender, EventArgs e)
        {
            任意条码打印窗口 frm = new 任意条码打印窗口();
            frm.ShowDialog();
        }

        private void 阀块装配_Click(object sender, EventArgs e)
        {
            FormPrintSolelyAssemblyPartBarCode form = new FormPrintSolelyAssemblyPartBarCode("液压阀块总成", m_commResponseServer);
            form.ShowDialog();
            form.TopMost = true;
        }

        private void 行星轮合件装配_Click(object sender, EventArgs e)
        {
            FormPrintSolelyAssemblyPartBarCode form = new FormPrintSolelyAssemblyPartBarCode("行星轮合件", m_commResponseServer);
            form.ShowDialog();
            form.TopMost = true;
        }

        private void 各分总成装配_Click(object sender, EventArgs e)
        {
            FormPrintAssemblyBarCode2 form = new FormPrintAssemblyBarCode2(m_commResponseServer);
            form.ShowDialog();
            form.TopMost = true;
        }

        private void lbl整台装配_MouseEnter(object sender, EventArgs e)
        {
            lbl整台装配.Cursor = Cursors.Hand;
            m_foreColor = lbl整台装配.ForeColor;
            lbl整台装配.ForeColor = Color.Red;
        }

        private void lbl整台装配_MouseLeave(object sender, EventArgs e)
        {
            lbl整台装配.Cursor = Cursors.Arrow;
            lbl整台装配.ForeColor = m_foreColor;
        }

        private void btnStartServer_Click(object sender, EventArgs e)
        {
            if (!m_commResponseServer.BeginFlag)
            {
                m_commResponseServer.StartServer();
            }

            if (m_commResponseServer.BeginFlag)
            {
                MessageDialog.ShowPromptMessage("已经启动服务器!");
            }
            else
            {
                MessageDialog.ShowPromptMessage("启动服务器失败，请与管理员联系!");
            }
        }

        private void 各分总成装配重复使用模式_Click(object sender, EventArgs e)
        {
            FormPrintAssemblyBarCodeForRepeatedMode form = new FormPrintAssemblyBarCodeForRepeatedMode(m_commResponseServer);

            form.ShowDialog();
            form.TopMost = true;

        }
    }
}
