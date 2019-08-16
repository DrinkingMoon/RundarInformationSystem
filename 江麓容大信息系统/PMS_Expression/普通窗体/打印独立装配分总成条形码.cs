/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  FormPrintSolelyAssemblyPartBarCode.cs
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
    /// 打印总成条形码界面
    /// </summary>
    public partial class FormPrintSolelyAssemblyPartBarCode : Form
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
        public FormPrintSolelyAssemblyPartBarCode(string assemblyType, CommResponseServer commServer)
        {
            InitializeComponent();

            m_assemblyType = assemblyType;
            txtAssemblyName.Text = m_assemblyType;
            m_commResponseServer = commServer;

            m_productInfoServer.GetAllProductInfo(out m_productInfo, out m_err);

            this.cmbPrductType.SelectedIndexChanged -= new System.EventHandler(this.cmbPrductType_SelectedIndexChanged);

            cmbPrductType.DataSource = m_productInfo;

            this.cmbPrductType.SelectedIndexChanged += new System.EventHandler(this.cmbPrductType_SelectedIndexChanged);

            cmbPrductType.DisplayMember = "产品类型编码";
            cmbPrductType.ValueMember = "产品类型编码";
            cmbPrductType.SelectedIndex = 0;
            m_productTypeCode = cmbPrductType.SelectedValue.ToString();

            dateTimePicker1.Value = ServerTime.Time;

            InitSerialNo();
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            int printCount = (int)(numCount.Value);
            BatchBarcodeInf batchInfo = null;
            string error;
            List<string> childAssembly = new List<string>();

            int beginSerialNumber = 0;

            try
            {
                beginSerialNumber = Convert.ToInt32(txtSerialNo.Text);
            }
            catch (Exception exce)
            {
                MessageDialog.ShowErrorMessage("不正确的流水号！" + exce.Message);
                return;
            }

            if (!m_barCode.BatchGenerateBarcode(m_productTypeCode, m_assemblyType, printCount, beginSerialNumber,
                dateTimePicker1.Value, out batchInfo, out error))
            {
                MessageDialog.ShowPromptMessage(error);
                return;
            }

            if (!m_barCode.PrintBarcodes(batchInfo, out error))
            {
                MessageDialog.ShowErrorMessage(error);
            }

            InitSerialNo();
        }

        void InitSerialNo()
        {
            string serialNo;
            string error;

            if (m_barCode.GetSolelyAssemblySerialNo(m_productTypeCode, m_assemblyType, out serialNo, out error))
            {
                txtSerialNo.Text = serialNo;
                btnPrint.Enabled = true;
            }
            else
            {
                btnPrint.Enabled = false;
                txtSerialNo.Text = "";
                string msg = error;

                if (error == "没有找到任何数据")
                {
                    msg = "数据库中无" + m_productTypeCode + "产品的总成条形码记录!";
                }

                MessageDialog.ShowPromptMessage(msg);
            }
        }

        private void cmbPrductType_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_productTypeCode = cmbPrductType.SelectedValue.ToString();
            InitSerialNo();
        }
    }
}
