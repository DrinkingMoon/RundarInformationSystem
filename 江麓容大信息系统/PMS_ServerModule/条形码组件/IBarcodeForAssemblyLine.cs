/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IBarcodeForAssemblyLine.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2009/06/13
 * 开发平台:  vs2005(c#)
 * 用于    :  生产线管理信息系统
 *----------------------------------------------------------------------------
 * 描述 : 装配线条形码管理组件接口(生成装配过程中的管理条形码等)
 * 其它 :
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2009/06/13 15:00:40 作者: 夏石友 当前版本: V1.00
 *        修改说明: 创建
 ******************************************************************************/

using System;
using System.Collections.Generic;

namespace ServerModule
{
    /// <summary>
    /// 批量条形码信息
    /// </summary>
    public class BatchBarcodeInf
    {
        /// <summary>
        /// 条码链
        /// </summary>
        /// <remarks>条码信息字典, 1个字典中有一组总成的条码信息</remarks>
        Dictionary<string, AssemblyManagementBarcode> m_dicBarcode;

        /// <summary>
        /// 批量条形码信息
        /// </summary>
        public Dictionary<string, AssemblyManagementBarcode> Barcodes
        {
            get
            {
                return m_dicBarcode;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public BatchBarcodeInf()
        {
            m_dicBarcode = new Dictionary<string, AssemblyManagementBarcode>();
        }
    }


    /// <summary>
    /// 装配线条形码管理组件接口
    /// </summary>
    public interface IBarcodeForAssemblyLine
    {
        /// <summary>
        /// 获取当前即将打印的总成编码流水码
        /// </summary>
        /// <param name="productCode">产品编码</param>
        /// <param name="serialNo">获取到的流水码</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool GetCurAssemblySerialNo(string productCode, out string serialNo, out string error);

        /// <summary>
        /// 获取当前即将打印的独立装配总成编码流水码
        /// </summary>
        /// <param name="productCode">产品编码</param>
        /// <param name="partName">零件名称</param>
        /// <param name="serialNo">获取到的流水码</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool GetSolelyAssemblySerialNo(string productCode, string partName, out string serialNo, out string error);

        /// <summary>
        /// 获取指定产品的总成名称
        /// </summary>
        /// <param name="productCode">产品编码</param>
        /// <param name="names">获取到的总成名称</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool GetAssemblyName(string productCode, List<string> names, out string error);

        /// <summary>
        /// 批量生成条形码(生成条形码成功后会自动增加数据库中的装配管理条形码表中各零部件的流水号)
        /// </summary>
        /// <param name="productTypeCode">产品类型编码</param>
        /// <param name="assemblyName">总成名称, 仅对独立装配有效，整台装配时此值必须为NULL</param>
        /// <param name="productAmount">本班次生产的产品数量,直接影响生成的条码数</param>
        /// <param name="batchBarcodeInf">批量生成的条形码信息(可能会包含产品编码、零部件名称等其它附加信息), 如果操作失败输出值为null</param>
        /// <param name="err">错误信息, 如果没有则输出值为null</param>
        /// <returns>成功返回true, 失败返回false</returns>
        bool BatchGenerateBarcode(string productTypeCode, string assemblyName, int productAmount, out BatchBarcodeInf batchBarcodeInf, out string err);

        /// <summary>
        /// 批量生成条形码(生成条形码成功后会自动增加数据库中的装配管理条形码表中各零部件的流水号)
        /// </summary>
        /// <param name="productTypeCode">产品类型编码</param>
        /// <param name="assemblyName">总成名称, 仅对独立装配有效，整台装配时此值必须为NULL</param>
        /// <param name="productAmount">本班次生产的产品数量,直接影响生成的条码数</param>
        /// <param name="remark">备注信息</param>
        /// <param name="batchBarcodeInf">批量生成的条形码信息(可能会包含产品编码、零部件名称等其它附加信息), 如果操作失败输出值为null</param>
        /// <param name="err">错误信息, 如果没有则输出值为null</param>
        /// <returns>成功返回true, 失败返回false</returns>
        bool BatchGenerateBarcode(string productTypeCode, string assemblyName, int productAmount, string remark, out BatchBarcodeInf batchBarcodeInf, out string err);

        /// <summary>
        /// 根据指定的流水号批量生成条形码(不更改数据库中装配管理条形码表中各零部件的流水号)
        /// </summary>
        /// <param name="productTypeCode">产品类型编码</param>
        /// <param name="assemblyName">总成名称, 仅对独立装配有效，整台装配时此值必须为NULL</param>
        /// <param name="productAmount">本班次生产的产品数量,直接影响生成的条码数</param>
        /// <param name="beginSerialNumber">起始流水码</param>
        /// <param name="dateTimeValue">日期</param>
        /// <param name="batchBarcodeInf">批量生成的条形码信息(可能会包含产品编码、零部件名称等其它附加信息), 如果操作失败输出值为null</param>
        /// <param name="err">错误信息, 如果没有则输出值为null</param>
        /// <returns>成功返回true, 失败返回false</returns>
        bool BatchGenerateBarcode(
            string productTypeCode, string assemblyName, int productAmount,
            int beginSerialNumber, DateTime dateTimeValue, out BatchBarcodeInf batchBarcodeInf, out string err);
                
        /// <summary>
        /// 根据指定的流水号批量生成重复使用的条形码(不更改数据库中装配管理条形码表中各零部件的流水号)
        /// </summary>
        /// <param name="productTypeCode">产品类型编码</param>
        /// <param name="assemblyName">总成名称, 仅对独立装配有效，整台装配时此值必须为NULL</param>
        /// <param name="productAmount">本班次生产的产品数量,直接影响生成的条码数</param>
        /// <param name="beginSerialNumber">起始流水码</param>
        /// <param name="batchBarcodeInf">批量生成的条形码信息(可能会包含产品编码、零部件名称等其它附加信息), 如果操作失败输出值为null</param>
        /// <param name="err">错误信息, 如果没有则输出值为null</param>
        /// <returns>成功返回true, 失败返回false</returns>
        bool BatchGenerateBarcodeForRepeatedMode(
            string productTypeCode, string assemblyName, int productAmount,
            int beginSerialNumber, out BatchBarcodeInf batchBarcodeInf, out string err);

        /// <summary>
        /// 获取零部件的条形码
        /// </summary>
        /// <param name="partCodes">要获取条形码的零部件</param>
        /// <param name="barcodes">零部件代码与条形码配对的字典, 如果某零部件没有获取到条形码则该零部件value值为null</param>
        /// <param name="err">错误信息, 如果没有则输出值为null</param>
        /// <returns>成功返回true, 失败返回false</returns>
        bool GetBarcodeOfParts(string[] partCodes, out System.Collections.Generic.Dictionary<string, string> barcodes, out string err);

        /// <summary>
        /// 批量打印条形码
        /// </summary>
        /// <param name="batchBarcodeInf">批量生成的管理条形码信息</param>
        /// <param name="err">错误信息, 如果没有则输出值为null</param>
        /// <returns>成功返回true, 失败返回false</returns>
        bool PrintBarcodes(BatchBarcodeInf batchBarcodeInf, out string err);
    }
}
