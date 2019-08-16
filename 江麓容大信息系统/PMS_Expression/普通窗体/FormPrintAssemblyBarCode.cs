/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  FormPrintAssemblyBarCode.cs
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
    public partial class FormPrintAssemblyBarCode : Form
    {
        /// <summary>
        /// 装配线条形码管理组件接口
        /// </summary>
        IBarcodeForAssemblyLine m_barCode = BarcodeFactoryForAssemblyLine.Instance;

        /// <summary>
        /// 产品装配简码
        /// </summary>
        string m_productTypeCode;

        /// <summary>
        /// 通信组件
        /// </summary>
        CommResponseServer m_commResponseServer;

        public FormPrintAssemblyBarCode(string productTypeName, string productTypeCode, CommResponseServer commServer)
        {
            InitializeComponent();
            m_commResponseServer = commServer;
            txtPrductTypeName.Text = productTypeName;
            m_productTypeCode = productTypeCode;

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

            if (!m_barCode.BatchGenerateBarcode(m_productTypeCode, null, printCount, out batchInfo, out error))
            {
                string msg = "服务器准备工作失败" + error + "!";
                MessageDialog.ShowPromptMessage(msg);
                return;
            }

            if (!m_barCode.PrintBarcodes(batchInfo, out error))
            {
                MessageDialog.ShowErrorMessage(error);
                return;
            }

            MessageDialog.ShowPromptMessage("服务器准备工作完成!");

            InitSerialNo();

            m_commResponseServer.StartServer();
        }

        void InitSerialNo()
        {
            string serialNo;
            string error;

            if (m_barCode.GetCurAssemblySerialNo(m_productTypeCode, out serialNo, out error))
            {
                txtSerialNo.Text = serialNo;
                btnPrint.Enabled = true;
            }
            else
            {
                btnPrint.Enabled = false;
                txtSerialNo.Text = "";
                string msg = "";

                if (error == "没有找到任何数据")
                {
                    msg = "数据库中无" + m_productTypeCode + "产品的总成条形码记录!";
                }

                MessageDialog.ShowPromptMessage(msg);
            }
        }
    }
}
