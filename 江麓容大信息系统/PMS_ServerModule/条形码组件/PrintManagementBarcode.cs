/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  PrintManagementBarcodeForAssembly.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2009/06/15
 * 开发平台:  vs2005(c#)
 * 用于    :  生产线管理信息系统
 *----------------------------------------------------------------------------
 * 描述 : 打印装配过程中的管理条形码
 * 其它 :
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2009/06/15 16:02:08 作者: 夏石友 当前版本: V1.00
 *        修改说明: 创建
 ******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace ServerModule
{
    /// <summary>
    /// 打印装配过程中的管理条形码
    /// </summary>
    internal class PrintBarcodeForAssembly
    {
        /// <summary>
        /// 打印条形码
        /// </summary>
        /// <param name="barcodeInfo">条形码信息</param>
        /// <param name="err">错误信息, 如果没有则输出值为null</param>
        /// <returns>成功返回true, 失败返回false</returns>
        static public bool PrintBarcode(BatchBarcodeInf barcodeInfo, out string err)
        {
            err = null;

            foreach (KeyValuePair<string, AssemblyManagementBarcode> var in barcodeInfo.Barcodes)
            {
                if (!Print(var.Value, out err))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="barcode">装配用管理条形码信息</param>
        /// <param name="err">错误信息, 如果没有则输出值为null</param>
        /// <returns>成功返回true, 失败返回false</returns>
        static private bool Print(AssemblyManagementBarcode barcode, out string err)
        {
            // 小条码纸适用
            err = null;

            IntPtr hDC = IntPtr.Zero;
            bool bRet = true;
            double x = 50;
            double y = 0;
            double width = 42;
            string barcodeType = "ETN-128";

            try
            {
                foreach (string barcodeContent in barcode.Barcode)
                {
                    //x = 45;
                    //bRet &= BarcodePrint.UTC_Begin(hDC, false);

                    //if (barcodeContent.Length <= 7)
                    //{
                    //    y = 4;

                    //    width = 22;     // OK, 扫描很快

                    //    // width = 25;  // OK, 扫描很快
                    //    // width = 30;  // OK, 扫描很快

                    //    // width = 15;  // NG, 效果不好
                    //    // width = 18;  // NG, 效果不好
                    //    // width = 20;  // NG, 效果不好

                    //    bRet &= bRet = BarcodePrint.UTC_Barcode(x, y, width, 9, barcodeContent, barcodeType, 0, false);

                    //    bRet &= BarcodePrint.UTC_SetFont("黑体", 10, true, false, false, false);
                    //    x = x + (width - BarcodePrint.UTC_GetTextWidth(barcodeContent)) / 2;
                    //    bRet &= BarcodePrint.UTC_Text(x, y + 10, barcodeContent, 0);
                    //}
                    //else if (barcodeContent.Length < 15)
                    //{
                    //    y = 4;

                    //    width = 38;

                    //    bRet &= bRet = BarcodePrint.UTC_Barcode(x, y, width, 9, barcodeContent, barcodeType, 0, false);

                    //    bRet &= BarcodePrint.UTC_SetFont("黑体", 10, true, false, false, false);
                    //    x = x + (width - BarcodePrint.UTC_GetTextWidth(barcodeContent)) / 2;
                    //    bRet &= BarcodePrint.UTC_Text(x, y + 10, barcodeContent, 0);
                    //}
                    //else
                    //{
                    //    y = 1;

                    //    width = 40;

                    //    int len = barcodeContent.Length / 2;
                    //    string data1 = barcodeContent.Substring(0, len);
                    //    string data2 = barcodeContent.Substring(len, barcodeContent.Length - len);

                    //    bRet &= bRet = BarcodePrint.UTC_Barcode(x, y, width, 6, data1, barcodeType, 0, false);
                    //    bRet &= bRet = BarcodePrint.UTC_Barcode(x, y + 8, width, 6, data2, barcodeType, 0, false);

                    //    bRet &= BarcodePrint.UTC_SetFont("黑体", 8, true, false, false, false);
                    //    x = x + (width - BarcodePrint.UTC_GetTextWidth(barcodeContent)) / 2;
                    //    bRet &= BarcodePrint.UTC_Text(x, y + 15, barcodeContent, 0);
                    //}

                    if (barcode.PartCode.Contains("RDC"))
                    {
                        x = 35;
                        y = 6;

                        bRet &= BarcodePrint.UTC_Begin(hDC, false);
                        bRet &= bRet = BarcodePrint.UTC_Barcode(x, y, 50, 9,
                            barcode.PartCode, barcodeType, 0, false);

                        y += 11;

                        bRet &= bRet = BarcodePrint.UTC_Barcode(x, y, 40, 9,
                            barcodeContent.Replace(barcode.PartCode, "").Trim(), barcodeType, 0, false);

                        y += 11;

                        bRet &= BarcodePrint.UTC_SetFont("黑体", 8, true, false, false, false);
                        x = x + (width - BarcodePrint.UTC_GetTextWidth(barcodeContent)) / 2;
                        bRet &= BarcodePrint.UTC_Text(x, y, barcodeContent, 0);
                    }
                    else
                    {
                        string barCode = barcodeContent;
                        x = 35;
                        y = 6;
                        width = 38;

                        bRet &= BarcodePrint.UTC_Begin(hDC, false);
                        bRet &= BarcodePrint.UTC_SetFont("黑体", 10, true, false, false, false);
                        double x1 = x + (width - BarcodePrint.UTC_GetTextWidth(barcode.PartName)) / 2;
                        bRet &= BarcodePrint.UTC_Text(x1, y, barcode.PartName, 0);

                        y += 5;

                        bRet &= bRet = BarcodePrint.UTC_Barcode(x, y, width, 9, barCode, barcodeType, 0, false);
                        bRet &= BarcodePrint.UTC_SetFont("黑体", 10, true, false, false, false);
                        double x2 = x + (width - BarcodePrint.UTC_GetTextWidth(barCode)) / 2;
                        bRet &= BarcodePrint.UTC_Text(x2, y + 10, barCode, 0);
                    }

                    bRet &= BarcodePrint.UTC_End(); //分页

                    if (!bRet)
                    {
                        return bRet;
                    }
                }
            }
            catch (Exception exce)
            {
                err = exce.Message;
                bRet = false;
            }

            return bRet;
        }
    }
}
