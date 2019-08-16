/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  PrintPartBarcode.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2010/07/17
 * 开发平台:  Visual C# 2008
 * 用于    :  仓库管理软件
 *----------------------------------------------------------------------------
 * 描述 : 
 * 其它 :
 *----------------------------------------------------------------------------
 * 公共信息: 参见系统'类帮助文档'
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2010/07/17 8:54:12 作者: 夏石友 当前版本: V1.00
 *        修改说明: 创建
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerModule
{
    /// <summary>
    /// 打印零部件条形码
    /// </summary>
    public class PrintPartBarcode
    {
        /// <summary>
        /// 打印实物卡片条形码
        /// </summary>
        /// <param name="cvtNumber">CVT编号</param>
        /// <returns>成功返回true, 失败返回false</returns>
        static public bool PrintBarcodeCVTNumberList_BoxNo(string boxNo)
        {
            IntPtr hDC = IntPtr.Zero;
            bool bRet = true;
            double x, y = 0, width;

            bRet &= BarcodePrint.UTC_Begin(hDC, false);
            bRet &= BarcodePrint.UTC_Rectangle(14, 1, 98, 28, 7);
            x = 16;
            y = 3;

            width = 51;
            x += 16;
            y += 5;
            bRet &= bRet = BarcodePrint.UTC_Barcode(x, y, width, 9, boxNo, "CODE128B", 0, false);

            bRet &= BarcodePrint.UTC_SetFont("黑体", 12, true, false, false, false);
            x = x + (width - BarcodePrint.UTC_GetTextWidth(boxNo)) / 2;
            bRet &= BarcodePrint.UTC_Text(x, y + 10, boxNo, 0);

            bRet &= BarcodePrint.UTC_End(); //分页
            return bRet;
        }

        /// <summary>
        /// 打印实物卡片条形码
        /// </summary>
        /// <param name="cvtNumber">CVT编号</param>
        /// <returns>成功返回true, 失败返回false</returns>
        static public bool PrintBarcodeCVTNumberList(List<string> cvtNumber)
        {
            IntPtr hDC = IntPtr.Zero;
            bool bRet = true;
            double x, y = 0;

            bRet &= BarcodePrint.UTC_Begin(hDC, false);
            bRet &= BarcodePrint.UTC_Rectangle(14, 1, 98, 28, 7);
            bRet &= BarcodePrint.UTC_Line(56, 1, 56, 28, 7);
            x = 16;
            y = 3;

            for (int i = 0; i < cvtNumber.Count; i++)
            {
                if (cvtNumber[i].Length > 10)
                {
                    bRet &= BarcodePrint.UTC_SetFont("黑体", 5, true, false, false, false);
                }
                else
                {
                    bRet &= BarcodePrint.UTC_SetFont("黑体", 20, true, false, false, false);
                }

                if (i < 3)
                {
                    x = 58;

                    if (i != 0)
                    {
                        bRet &= BarcodePrint.UTC_Line(14, y - 2, 98, y - 2, 5);
                    }
                }
                else
                {
                    x = 16;
                }

                if (i == 3)
                {
                    y = 3;
                }

                if (i % 3 == 0)
                {
                    bRet &= BarcodePrint.UTC_Text(x, y - 1, cvtNumber[i], 0);
                }
                else
                {
                    bRet &= BarcodePrint.UTC_Text(x, y - 1, cvtNumber[i], 0);
                }

                y = y + 9;
            }

            bRet &= BarcodePrint.UTC_End(); //分页
            return bRet;
        }

        /// <summary>
        /// 打印实物卡片条形码
        /// </summary>
        /// <param name="barcodeInfo">打印条形码时所需信息</param>
        /// <param name="amount">数量</param>
        /// <returns>成功返回true, 失败返回false</returns>
        static public bool PrintBarcodeList(View_S_InDepotGoodsBarCodeTable barcodeInfo, decimal amount)
        {
            IntPtr hDC = IntPtr.Zero;
            bool bRet = true;
            double x, y, width = 0;

            if (barcodeInfo.条形码.ToString().Length <= 4)
            {
                width = 15;
            }
            else
            {
                width = 30;
            }

            bRet &= BarcodePrint.UTC_Begin(hDC, false);

            bRet &= BarcodePrint.UTC_Rectangle(14, 1, 98, 30, 5);
            x = 15;
            y = 2.0;

            bRet &= BarcodePrint.UTC_SetFont("黑体", 8, true, false, false, false);

            //图号型号
            string goodsCode = "图号/型号：" + barcodeInfo.图号型号;

            bRet &= BarcodePrint.UTC_Text(x, y, goodsCode, 0);

            //产品名称
            string goodsName = "产品名称 ：" + barcodeInfo.物品名称;

            bRet &= BarcodePrint.UTC_Text(x, y + 4, goodsName, 0);

            //规格
            string spec = "规    格 ：" + barcodeInfo.规格;

            if (spec.Length > 34)
            {
                BarcodePrint.UTC_SetFont("黑体", 8, true, false, false, false);

                bRet &= BarcodePrint.UTC_Text(x, y + 8, spec, 0);

                BarcodePrint.UTC_SetFont("黑体", 8, true, false, false, false);
            }
            else if (spec.Length > 29)
            {
                bRet &= BarcodePrint.UTC_Text(x, y + 8, spec, 0);
            }
            else
            {
                bRet &= BarcodePrint.UTC_Text(x, y + 8, spec, 0);
            }

            //供应商
            string provider = "供 应 商 ：" + barcodeInfo.供货单位;

            if (amount > 0)
            {
                provider += "   数量 ：" + amount.ToString("F2");
            }

            bRet &= BarcodePrint.UTC_Text(x, y + 12, provider, 0);

            //批次
            string batchCode = "批    次 ：" + barcodeInfo.批次号;

            if (barcodeInfo.材料类别名称 == "回收件" || barcodeInfo.材料类别名称 == "返修件")
            {
                batchCode += "(" + barcodeInfo.材料类别名称 + ")";
            }

            bRet &= BarcodePrint.UTC_Text(x, y + 16, batchCode, 0);

            //货架
            string shelf = string.Format("货    架 ：{0}  层：{1}  列：{2}", barcodeInfo.货架, barcodeInfo.层, barcodeInfo.列);

            bRet &= BarcodePrint.UTC_Text(x, y + 20, shelf, 0);

            if (barcodeInfo.工位 != null)
            {
                //工位
                string workBench = string.Format("工    位 ：{0}", barcodeInfo.工位);

                bRet &= BarcodePrint.UTC_Text(x, y + 24, workBench, 0);
            }

            if (spec.Length > 20)
            {
                x += 9;
                width -= 2;
            }

            if (barcodeInfo.条形码.ToString().Length < 2)
            {
                x += 5;
            }

            bRet &= bRet = BarcodePrint.UTC_Barcode(x + 52, y + 5, width, 15, barcodeInfo.条形码.ToString(), "CODE128B", 0, false);

            bRet &= BarcodePrint.UTC_SetFont("黑体", 14, true, false, false, false);

            string barCodeText = barcodeInfo.条形码.ToString();

            x = x + 48 + (width - BarcodePrint.UTC_GetTextWidth(barCodeText)) / 2;
            bRet &= BarcodePrint.UTC_Text(x, y + 21, barCodeText, 0);
            bRet &= BarcodePrint.UTC_End(); //分页

            return bRet;
        }

        /// <summary>
        /// 打印实物卡片条形码
        /// </summary>
        /// <param name="barcodeInfo">打印条形码时所需信息</param>
        /// <returns>成功返回true, 失败返回false</returns>
        static public bool PrintBarcodeList(View_S_InDepotGoodsBarCodeTable barcodeInfo)
        {
            return PrintBarcodeList(barcodeInfo, 0);
        }

        /// <summary>
        /// 打印条形码
        /// </summary>
        /// <param name="cvtCode">条形码信息</param>
        /// <returns>成功返回true, 失败返回false</returns>
        static public bool PrintBarcode(string cvtCode)
        {
            IntPtr hDC = IntPtr.Zero;
            bool bRet = true;
            double x, y, width;

            bRet &= BarcodePrint.UTC_Begin(hDC, false);

            bRet &= BarcodePrint.UTC_Rectangle(42, 2, 97, 18, 5);
            bRet &= BarcodePrint.UTC_SetFont("黑体", 21, true, false, false, false);

            x = 44;
            y = 4;

            width = 51;
            bRet &= bRet = BarcodePrint.UTC_Barcode(x, y, width, 9, cvtCode, "CODE128B", 0, false);

            bRet &= BarcodePrint.UTC_SetFont("黑体", 10, true, false, false, false);
            x = x + (width - BarcodePrint.UTC_GetTextWidth(cvtCode)) / 2;
            bRet &= BarcodePrint.UTC_Text(x, y + 10, cvtCode, 0);

            bRet &= BarcodePrint.UTC_End(); //分页

            return bRet;
        }

        /// <summary>
        /// 打印TCU盒的条形码
        /// </summary>
        /// <param name="strCodeFrist">第一个条码信息</param>
        /// <param name="strCodeSecond">第二个条码信息</param>
        /// <returns>成功返回true, 失败返回false</returns>
        static public bool PrintBarcodeBoxTCU(string strCodeFrist, string strCodeSecond)
        {
            IntPtr hDC = IntPtr.Zero;
            bool bRet = true;
            double x, y, width;

            bRet &= BarcodePrint.UTC_Begin(hDC, false);

            bRet &= BarcodePrint.UTC_Rectangle(14, 1, 98, 30, 5);
            bRet &= BarcodePrint.UTC_SetFont("黑体", 21, true, false, false, false);

            //第一个条码
            x = 15;
            y = 2;

            width = 70;
            bRet &= bRet = BarcodePrint.UTC_Barcode(x, y, width, 8, strCodeFrist, "CODE128B", 0, false);

            bRet &= BarcodePrint.UTC_SetFont("黑体", 10, true, false, false, false);
            x = x + (width - BarcodePrint.UTC_GetTextWidth(strCodeFrist)) / 2;
            bRet &= BarcodePrint.UTC_Text(x, y + 10, strCodeFrist, 0);

            //第二个条码
            x = 15;
            y = 15;
            width = 70;
            bRet &= bRet = BarcodePrint.UTC_Barcode(x, y, width, 8, strCodeFrist, "CODE128B", 0, false);

            bRet &= BarcodePrint.UTC_SetFont("黑体", 10, true, false, false, false);
            x = x + (width - BarcodePrint.UTC_GetTextWidth(strCodeFrist)) / 2;
            bRet &= BarcodePrint.UTC_Text(x, y + 10, strCodeFrist, 0);

            bRet &= BarcodePrint.UTC_End(); //分页

            return bRet;
        }

        /// <summary>
        /// 打印TCU箱的条形码
        /// </summary>
        /// <param name="bigBoxTCUCode">条形码信息</param>
        /// <returns>成功返回true, 失败返回false</returns>
        static public bool PrintBarcode_120X30(string code)
        {
            IntPtr hDC = IntPtr.Zero;
            bool bRet = true;
            double x, y, width;

            bRet &= BarcodePrint.UTC_Begin(hDC, false);

            bRet &= BarcodePrint.UTC_Rectangle(10, 1, 98, 29, 5);
            bRet &= BarcodePrint.UTC_SetFont("黑体", 21, true, false, false, false);

            x = 18;
            y = 3;

            width = 72;
            bRet &= bRet = BarcodePrint.UTC_Barcode(x, y, width, 15, code, "CODE128B", 0, false);

            bRet &= BarcodePrint.UTC_SetFont("黑体", 25, true, false, false, false);
            x = x + (width - BarcodePrint.UTC_GetTextWidth(code)) / 2;
            bRet &= BarcodePrint.UTC_Text(x, y + 16, code, 0);

            bRet &= BarcodePrint.UTC_End(); //分页

            return bRet;
        }

        /// <summary>
        /// 打印整车对应条形码（按整车厂要求）
        /// </summary>
        /// <param name="cvtCode">条形码信息</param>
        /// <returns>成功返回true, 失败返回false</returns>
        static public bool PrintBarcodeForVehicle(string cvtCode)
        {
            IntPtr hDC = IntPtr.Zero;
            bool bRet = true;
            double x, y, width;

            bRet &= BarcodePrint.UTC_Begin(hDC, false);

            bRet &= BarcodePrint.UTC_SetFont("仿宋_GB2312", 21, true, false, false, false);

            x = 42;
            y = 2;

            width = 55;
            bRet &= bRet = BarcodePrint.UTC_Barcode(x, y, width, 10, cvtCode, "CODE128B", 0, false);

            bRet &= BarcodePrint.UTC_SetFont("仿宋_GB2312", 11, true, false, false, false);
            x = x + (width - BarcodePrint.UTC_GetTextWidth(cvtCode)) / 2;
            bRet &= BarcodePrint.UTC_Text(x, y + 11, cvtCode, 0);

            bRet &= BarcodePrint.UTC_End(); //分页

            return bRet;
        }

        /// <summary>
        /// 打印条形码
        /// </summary>
        /// <param name="lstText">信息列表</param>
        /// <returns>成功返回true, 失败返回false</returns>
        static public bool PrintBarcode_Common(List<string> lstText)
        {
            IntPtr hDC = IntPtr.Zero;
            bool bRet = true;
            double x, y, z;
            x = 6;
            y = 4;
            z = 10;

            bRet &= BarcodePrint.UTC_Begin(hDC, false);
            bRet &= BarcodePrint.UTC_Rectangle(x - 1, y - 1, 58, y + (z * lstText.Count()) + 1, 5);

            y += 4;

            for (int i = 0; i < lstText.Count; i++)
            {
                bRet &= BarcodePrint.UTC_SetFont("黑体", 12, true, false, false, false);
                bRet &= BarcodePrint.UTC_Text(x, y, lstText[i], 0);
                bRet &= BarcodePrint.UTC_Line(5, y + 8, 58, y + 8, 5);

                y += z;
            }

            bRet &= BarcodePrint.UTC_Line(16, 16, 16, 4 + (z * lstText.Count()), 5);
            bRet &= BarcodePrint.UTC_End(); //分页
            return bRet;
        }
    }
}
