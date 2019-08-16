/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  BarcodePrint.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2009/06/15
 * 开发平台:  vs2005(c#)
 * 用于    :  生产线管理信息系统
 *----------------------------------------------------------------------------
 * 描述 : 条形码打印组件(用于将条码打印出来)
 * 其它 :
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2009/06/15 14:52:18 作者: 夏石友 当前版本: V1.00
 *        修改说明: 创建
 ******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace ServerModule
{
    /// <summary>
    /// 条形码打印机(用于将条码打印出来)
    /// </summary>
    public class BarcodePrints  
    {
        /// <summary>
        /// 打印初始化
        /// </summary>
        /// <param name="vcHDC">图形设备句柄</param>
        /// <param name="bPreview">是否需要预览</param>
        /// <returns>成功返回true, 失败返回false</returns>
        [DllImport(@"GMSatoPrn.dll")]
        public static extern bool UTC_Begin(IntPtr vcHDC, bool bPreview);

        /// <summary>
        /// 画线
        /// </summary>
        /// <param name="x1">左端点x坐标</param>
        /// <param name="y1">左端点y坐标</param>
        /// <param name="x2">右端点x坐标</param>
        /// <param name="y2">右端点y坐标</param>
        /// <param name="penWidth">线宽</param>
        /// <returns>成功返回true, 失败返回false</returns>
        [DllImport(@"GMSatoPrn.dll")]
        public static extern bool UTC_Line(double x1, double y1, double x2, double y2, int penWidth);

        /// <summary>
        /// 画矩形
        /// </summary>
        /// <param name="x1">左上角x坐标</param>
        /// <param name="y1">左上角y坐标</param>
        /// <param name="x2">右下角x坐标</param>
        /// <param name="y2">右下角y坐标</param>
        /// <param name="penWidth">线宽</param>
        /// <returns>成功返回true, 失败返回false</returns>
        [DllImport(@"GMSatoPrn.dll")]
        public static extern bool UTC_Rectangle(double x1, double y1, double x2, double y2, int penWidth);

        /// <summary>
        /// 设置字体
        /// </summary>
        /// <param name="fontName">字体名称</param>
        /// <param name="fontSize">字体尺寸</param>
        /// <param name="bold">是否粗体的标志</param>
        /// <param name="italic">是否斜体的标志</param>
        /// <param name="underline">是否下划线的标志</param>
        /// <param name="strikeOut"></param>
        /// <returns>成功返回true, 失败返回false</returns>
        [DllImport(@"GMSatoPrn.dll")]
        public static extern bool UTC_SetFont(string fontName, int fontSize, bool bold, bool italic, bool underline, bool strikeOut);

        /// <summary>
        /// 打印文本
        /// </summary>
        /// <param name="x">左上角x坐标</param>
        /// <param name="y">左上角y坐标</param>
        /// <param name="text">要打印的文本</param>
        /// <param name="angle">文本角度</param>
        /// <returns>成功返回true, 失败返回false</returns>
        [DllImport(@"GMSatoPrn.dll")]
        public static extern bool UTC_Text(double x, double y, string text, int angle);

        /// <summary>
        /// 打印条形码
        /// </summary>
        /// <param name="x">左上角x坐标</param>
        /// <param name="y">左上角y坐标</param>
        /// <param name="width">条形码宽度</param>
        /// <param name="height">条形码高度</param>
        /// <param name="barcodeData">条形码数据</param>
        /// <param name="barcodeType">条形码类型</param>
        /// <param name="angle">条形码角度</param>
        /// <param name="isReadable">是否是可读的</param>
        /// <returns>成功返回true, 失败返回false</returns>
        [DllImport(@"GMSatoPrn.dll")]
        public static extern bool UTC_Barcode(double x, double y, double width, double height, string barcodeData, string barcodeType, int angle, bool isReadable);

        /// <summary>
        /// 获取文字宽度
        /// </summary>
        /// <param name="text">文本</param>
        /// <returns>成功返回true, 失败返回false</returns>
        [DllImport(@"GMSatoPrn.dll")]
        public static extern double UTC_GetTextWidth(string text);

        /// <summary>
        /// 获取文字高度
        /// </summary>
        /// <param name="text">文本</param>
        /// <returns>成功返回true, 失败返回false</returns>
        [DllImport(@"GMSatoPrn.dll")]
        public static extern double UTC_GetTextHeight(string text);

        /// <summary>
        /// 获取条码宽度
        /// </summary>
        /// <param name="width">宽度</param>
        /// <param name="barcodeType">条形码类型</param>
        /// <returns>成功返回true, 失败返回false</returns>
        [DllImport(@"GMSatoPrn.dll")]
        public static extern double UTC_GetBarWidth(double width, string barcodeType);

        /// <summary>
        /// 设置条码(可能是比例)
        /// </summary>
        /// <param name="text">文本</param>
        /// <returns>成功返回true, 失败返回false</returns>
        [DllImport(@"GMSatoPrn.dll")]
        public static extern double UTC_SetBCRatio(string text);

        /// <summary>
        /// 打印结束(起分页效果)
        /// </summary>
        /// <returns>成功返回true, 失败返回false</returns>
        [DllImport(@"GMSatoPrn.dll")]
        public static extern bool UTC_End();
    }
}
