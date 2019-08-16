using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PMS_ServerModule;
using ServerModule;

namespace Expression
{
    public partial class 任意条码打印窗口 : Form
    {
        public 任意条码打印窗口()
        {
            InitializeComponent();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintBarcode(txtContent.Text);
        }

        /// <summary>
        /// 打印整车对应条形码（按整车厂要求）
        /// </summary>
        /// <param name="cvtCode">条形码信息</param>
        /// <returns>成功返回true, 失败返回false</returns>
        void PrintBarcode(string code)
        {
            IntPtr hDC = IntPtr.Zero;
            bool bRet = true;
            double x = 42, y = 2, width = 55;

            bRet &= ServerModule.BarcodePrints.UTC_Begin(hDC, false);
            bRet &= ServerModule.BarcodePrints.UTC_SetFont("仿宋_GB2312", 21, true, false, false, false);
            bRet &= bRet = ServerModule.BarcodePrints.UTC_Barcode(x, y, width, 10, code, "CODE128B", 0, false);
            bRet &= ServerModule.BarcodePrints.UTC_SetFont("仿宋_GB2312", 11, true, false, false, false);

            x = x + (width - ServerModule.BarcodePrints.UTC_GetTextWidth(code)) / 2;

            bRet &= ServerModule.BarcodePrints.UTC_Text(x, y + 11, code, 0);
            bRet &= ServerModule.BarcodePrints.UTC_End(); //分页

            if (!bRet)
            {
                throw new Exception("条码打印失败");
            }
        }
    }
}
