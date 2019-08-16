/******************************************************************************
 * ��Ȩ���� (c) 2006-2010, С����ҵ�����ݴ��������ι�˾
 *
 * �ļ�����:  PrintManagementBarcodeForAssembly.cs
 * ����    :  ��ʯ��    �汾: v1.00    ����: 2009/06/15
 * ����ƽ̨:  vs2005(c#)
 * ����    :  �����߹�����Ϣϵͳ
 *----------------------------------------------------------------------------
 * ���� : ��ӡװ������еĹ���������
 * ���� :
 *----------------------------------------------------------------------------
 * ��ʷ��¼:
 *     1. ����ʱ��: 2009/06/15 16:02:08 ����: ��ʯ�� ��ǰ�汾: V1.00
 *        �޸�˵��: ����
 ******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace ServerModule
{
    /// <summary>
    /// ��ӡװ������еĹ���������
    /// </summary>
    internal class PrintBarcodeForAssembly
    {
        /// <summary>
        /// ��ӡ������
        /// </summary>
        /// <param name="barcodeInfo">��������Ϣ</param>
        /// <param name="err">������Ϣ, ���û�������ֵΪnull</param>
        /// <returns>�ɹ�����true, ʧ�ܷ���false</returns>
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
        /// ��ӡ
        /// </summary>
        /// <param name="barcode">װ���ù�����������Ϣ</param>
        /// <param name="err">������Ϣ, ���û�������ֵΪnull</param>
        /// <returns>�ɹ�����true, ʧ�ܷ���false</returns>
        static private bool Print(AssemblyManagementBarcode barcode, out string err)
        {
            // С����ֽ����
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

                    //    width = 22;     // OK, ɨ��ܿ�

                    //    // width = 25;  // OK, ɨ��ܿ�
                    //    // width = 30;  // OK, ɨ��ܿ�

                    //    // width = 15;  // NG, Ч������
                    //    // width = 18;  // NG, Ч������
                    //    // width = 20;  // NG, Ч������

                    //    bRet &= bRet = BarcodePrint.UTC_Barcode(x, y, width, 9, barcodeContent, barcodeType, 0, false);

                    //    bRet &= BarcodePrint.UTC_SetFont("����", 10, true, false, false, false);
                    //    x = x + (width - BarcodePrint.UTC_GetTextWidth(barcodeContent)) / 2;
                    //    bRet &= BarcodePrint.UTC_Text(x, y + 10, barcodeContent, 0);
                    //}
                    //else if (barcodeContent.Length < 15)
                    //{
                    //    y = 4;

                    //    width = 38;

                    //    bRet &= bRet = BarcodePrint.UTC_Barcode(x, y, width, 9, barcodeContent, barcodeType, 0, false);

                    //    bRet &= BarcodePrint.UTC_SetFont("����", 10, true, false, false, false);
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

                    //    bRet &= BarcodePrint.UTC_SetFont("����", 8, true, false, false, false);
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

                        bRet &= BarcodePrint.UTC_SetFont("����", 8, true, false, false, false);
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
                        bRet &= BarcodePrint.UTC_SetFont("����", 10, true, false, false, false);
                        double x1 = x + (width - BarcodePrint.UTC_GetTextWidth(barcode.PartName)) / 2;
                        bRet &= BarcodePrint.UTC_Text(x1, y, barcode.PartName, 0);

                        y += 5;

                        bRet &= bRet = BarcodePrint.UTC_Barcode(x, y, width, 9, barCode, barcodeType, 0, false);
                        bRet &= BarcodePrint.UTC_SetFont("����", 10, true, false, false, false);
                        double x2 = x + (width - BarcodePrint.UTC_GetTextWidth(barCode)) / 2;
                        bRet &= BarcodePrint.UTC_Text(x2, y + 10, barCode, 0);
                    }

                    bRet &= BarcodePrint.UTC_End(); //��ҳ

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
