/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  BarcodeForAssemblyLine.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2009/06/11
 * 开发平台:  vs2005(c#)
 * 用于    :  生产线管理信息系统
 *----------------------------------------------------------------------------
 * 描述 : 条形码管理组件(生成装配过程中的管理条形码等)
 * 其它 :
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2009/06/11 15:00:40 作者: 夏石友 当前版本: V1.00
 *        修改说明: 创建
 ******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DBOperate;
using System.Collections;
using System.Diagnostics;

namespace ServerModule
{
    /// <summary>
    /// 用于装配管理的条形码信息
    /// </summary>
    /// <remarks>1个本类型对应指定产品、零部件的一组管理条形码, 包含了为打印需要而增添的其它信息, 如：零部件名称等</remarks>
    public class AssemblyManagementBarcode
    {
        #region member variant

        /// <summary>
        /// 产品编码
        /// </summary>
        string m_productCode;

        /// <summary>
        /// 零部件名称(总成名称)
        /// </summary>
        string m_partName;

        /// <summary>
        /// 零部件代码
        /// </summary>
        string m_partCode;

        /// <summary>
        /// 构建条形码的规则
        /// </summary>
        string m_buildRule;

        /// <summary>
        /// 用于生成条形码的数据
        /// </summary>
        object[] m_surDatas;

        /// <summary>
        /// 生成好的条形码数据
        /// </summary>
        string[] m_barcodes;

        /// <summary>
        /// 流水码
        /// </summary>
        int m_serialNumber;

        #endregion member variant

        #region Properties

        /// <summary>
        /// 设置或获取产品编码
        /// </summary>
        public string ProductCode
        {
            set
            {
                Debug.Assert(value != null && value.Length > 0);
                m_productCode = value;
            }
            get
            {
                return m_productCode;
            }
        }

        /// <summary>
        /// 获取或设置零部件代码
        /// </summary>
        public string PartCode
        {
            get { return m_partCode; }
            set { m_partCode = value; }
        }

        /// <summary>
        /// 设置或获取零部件名称(总成名称)
        /// </summary>
        public string PartName
        {
            set
            {
                Debug.Assert(value != null && value.Length > 0);
                m_partName = value;
            }
            get
            {
                return m_partName;
            }
        }

        /// <summary>
        /// 设置或获取构建条形码的规则
        /// </summary>
        internal string BuildRule
        {
            set
            {
                Debug.Assert(value != null && value.Length > 0);
                m_buildRule = value;
            }
            get
            {
                return m_buildRule;
            }
        }

        /// <summary>
        /// 设置或获取流水码
        /// </summary>
        internal int SerialNumber
        {
            set
            {
                Debug.Assert(value > 0);
                m_serialNumber = value;
            }
            get
            {
                return m_serialNumber;
            }
        }

        /// <summary>
        /// 生成好的条形码数据
        /// </summary>
        public string[] Barcode
        {
            get
            {
                return m_barcodes;
            }
        }

        /// <summary>
        /// 获取用于生成条形码的数据
        /// </summary>
        internal object[] SourceData
        {
            get
            {
                return m_surDatas;
            }
        }

        #endregion Properties

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="surDataAmount">用于生成条形码的源数据个数</param>
        /// <param name="barcodeAmount">要生成的条形码数量</param>
        public AssemblyManagementBarcode(int surDataAmount, int barcodeAmount)
        {
            m_surDatas = new object[surDataAmount];
            m_barcodes = new string[barcodeAmount];
        }
    }

    /// <summary>
    /// 装配线条形码类
    /// </summary>
    /// <remarks>生成装配时所需的管理用条形码等</remarks>
    internal class BarcodeForAssemblyLine : IBarcodeForAssemblyLine
    {
        /// <summary>
        /// 数据库操作接口
        /// </summary>
        IDBOperate m_dbOperate = CommentParameter.GetDBOperatorOfDepotManagement();

        #region public method

        /// <summary>
        /// 获取指定产品的总成名称
        /// </summary>
        /// <param name="productCode">产品编码</param>
        /// <param name="names">获取到的总成名称</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool GetAssemblyName(string productCode, List<string> names, out string error)
        {
            error = null;

            Hashtable paramTable = new Hashtable();
            paramTable.Add("@ProductCode", productCode);

            DataSet ds = new DataSet();
            Dictionary<OperateCMD, object> dicOperateCMD = m_dbOperate.RunProc_CMD("SelP_AssemblyName", ds, paramTable);

            if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
            {
                error = Convert.ToString(dicOperateCMD[OperateCMD.Return_Errmsg]);
                return false;
            }

            if (ds.Tables == null || ds.Tables[0].Rows.Count == 0)
            {
                error = "没有找到总成信息";
                return false;
            }

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                names.Add(ds.Tables[0].Rows[i][0].ToString());
            }
            
            return true;
        }

        /// <summary>
        /// 获取当前即将打印的总成编码流水码
        /// </summary>
        /// <param name="productCode">产品编码</param>
        /// <param name="serialNo">获取到的流水码</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool GetCurAssemblySerialNo(string productCode, out string serialNo, out string error)
        {
            error = null;
            serialNo = null;

            Hashtable paramTable = new Hashtable();
            paramTable.Add("@ProductCode", productCode);
            paramTable.Add("@AssemblyName", DBNull.Value);   // 如果为整台份装配则此参数为null

            DataSet ds = new DataSet();
            Dictionary<OperateCMD, object> dicOperateCMD = m_dbOperate.RunProc_CMD("SelP_AssemblyLineSerialNo", ds, paramTable);

            if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
            {
                error = Convert.ToString(dicOperateCMD[OperateCMD.Return_Errmsg]);
                return false;
            }

            serialNo = ds.Tables[0].Rows[0][0].ToString();
            return true;
        }

        /// <summary>
        /// 获取当前即将打印的独立装配总成编码流水码
        /// </summary>
        /// <param name="productCode">产品编码</param>
        /// <param name="partName">零件名称</param>
        /// <param name="serialNo">获取到的流水码</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool GetSolelyAssemblySerialNo(string productCode, string partName, out string serialNo, out string error)
        {
            error = null;
            serialNo = null;

            Hashtable paramTable = new Hashtable();
            paramTable.Add("@ProductCode", productCode);    // 产品编码
            paramTable.Add("@AssemblyName", partName);   // 总成名称

            DataSet ds = new DataSet();
            Dictionary<OperateCMD, object> dicOperateCMD = m_dbOperate.RunProc_CMD("SelP_AssemblyLineSerialNo", ds, paramTable);

            if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
            {
                error = Convert.ToString(dicOperateCMD[OperateCMD.Return_Errmsg]);
                return false;
            }

            serialNo = ds.Tables[0].Rows[0][0].ToString(); 
            return true;
        }

        /// <summary>
        /// 批量生成条形码(生成条形码成功后会自动增加数据库中的装配管理条形码表中各零部件的流水号)
        /// </summary>
        /// <param name="productTypeCode">产品类型编码</param>
        /// <param name="assemblyName">总成名称, 仅对独立装配有效，整台装配时此值必须为NULL</param>
        /// <param name="productAmount">本班次生产的产品数量,直接影响生成的条码数</param>
        /// <param name="batchBarcodeInf">批量生成的条形码信息(可能会包含产品编码、零部件名称等其它附加信息), 如果操作失败输出值为null</param>
        /// <param name="err">错误信息, 如果没有则输出值为null</param>
        /// <returns>成功返回true, 失败返回false</returns>
        public bool BatchGenerateBarcode(string productTypeCode, string assemblyName, int productAmount, out BatchBarcodeInf batchBarcodeInf, out string err)
        {
            return BatchGenerateBarcode(productTypeCode, assemblyName, productAmount, null, out batchBarcodeInf, out err);
        }

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
        public bool BatchGenerateBarcode(string productTypeCode, string assemblyName, int productAmount, string remark, out BatchBarcodeInf batchBarcodeInf, out string err)
        {
            Debug.Assert(productTypeCode != null && productTypeCode.Length > 0, "参数 'productCode' 不能为 null 或空串");
            Debug.Assert(productAmount > 0, "参数 'productAmount' 必须 > 0");

            batchBarcodeInf = null;
            err = null;

            if (productTypeCode == null || productTypeCode.Length == 0)
            {
                err = "参数 'productCode' 不能为 null 或空串";
                return false;
            }

            if (productAmount < 1)
            {
                err = "参数 'productAmount' 必须 > 0";
                return false;
            }

            // 执行存储过程从数据库中获取数据表
            Hashtable paramTable = new Hashtable();

            paramTable.Add("@ProductTypeCode", productTypeCode);
            paramTable.Add("@UpdateAmount", productAmount);
            paramTable.Add("@IsRepairMode", productTypeCode.Contains(" FX") ? 1 : 0);
            paramTable.Add("@UserCode", GlobalObject.BasicInfo.LoginID);
            paramTable.Add("@Remark", GlobalObject.GeneralFunction.IsNullOrEmpty(remark) ? "打印装配条形码" : remark);

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(assemblyName))
                paramTable.Add("@AssemblyName", DBNull.Value);
            else
                paramTable.Add("@AssemblyName", assemblyName);

            DataSet ds = new DataSet();

            if (!AccessDB.ExecuteDbProcedure("ZPX_AssignAssemblyLineBarcode", paramTable, ds, out err))
            {
                return false;
            }

            DataTable dt = ds.Tables[0];

            if (dt == null || dt.Rows.Count == 0)
            {
                err = "没有获取到装配条码信息";
                return false;
            }

            // 创建存储生成的管理条形码信息的对象
            BatchBarcodeInf managementBarcode = new BatchBarcodeInf();

            // 从数据表中提取生成条形码的源数据
            ExtractSourceDataFormDT(productTypeCode, productAmount, dt, managementBarcode);

            DateTime dt1 = ServerModule.ServerTime.Time;
            DateTime barDate = dt1;

            if (dt1.Day > 25)
            {
                if (dt1.Month == 12)
                    barDate = new DateTime(dt1.Year + 1, 1, 1);
                else
                    barDate = new DateTime(dt1.Year, dt1.Month + 1, 1);
            }

            // 根据规则生成条形码
            foreach (KeyValuePair<string, AssemblyManagementBarcode> var in managementBarcode.Barcodes)
            {
                BuildBarcode(productTypeCode, productAmount, barDate, var.Value);
            }

            // 输出的条形码信息
            batchBarcodeInf = managementBarcode;

            return true;
        }

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
        public bool BatchGenerateBarcode(string productTypeCode, string assemblyName, int productAmount, 
            int beginSerialNumber, DateTime dateTimeValue, out BatchBarcodeInf batchBarcodeInf, out string err)
        {
            Debug.Assert(productTypeCode != null && productTypeCode.Length > 0, "参数 'productCode' 不能为 null 或空串");
            Debug.Assert(productAmount > 0, "参数 'productAmount' 必须 > 0");

            batchBarcodeInf = null;
            err = null;

            if (productTypeCode == null || productTypeCode.Length == 0)
            {
                err = "参数 'productCode' 不能为 null 或空串";
                return false;
            }

            if (productAmount < 1)
            {
                err = "参数 'productAmount' 必须 > 0";
                return false;
            }

            // 执行存储过程从数据库中获取数据表
            Hashtable paramTable = new Hashtable();

            paramTable.Add("@productCode", productTypeCode);

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(assemblyName))
                paramTable.Add("@AssemblyName", DBNull.Value);
            else
                paramTable.Add("@AssemblyName", assemblyName);

            DataSet ds = new DataSet();

            if (!AccessDB.ExecuteDbProcedure("SelP_AssemblyLineBarcode", paramTable, ds, out err))
            {
                return false;
            }

            DataTable dt = ds.Tables[0];

            if (dt == null || dt.Rows.Count == 0)
            {
                err = "没有获取到装配条码信息";
                return false;
            }

            // 创建存储生成的管理条形码信息的对象
            BatchBarcodeInf managementBarcode = new BatchBarcodeInf();

            // 从数据表中提取生成条形码的源数据
            ExtractSourceDataFormDT(productTypeCode, productAmount, dt, managementBarcode);

            int serialNumber = beginSerialNumber;

            // 根据规则生成条形码
            foreach (KeyValuePair<string, AssemblyManagementBarcode> var in managementBarcode.Barcodes)
            {
                var.Value.SerialNumber = serialNumber++;
                BuildBarcode(productTypeCode, productAmount, dateTimeValue, var.Value);
            }

            // 输出的条形码信息
            batchBarcodeInf = managementBarcode;

            return true;
        }
                
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
        public bool BatchGenerateBarcodeForRepeatedMode(
            string productTypeCode, string assemblyName, int productAmount,
            int beginSerialNumber, out BatchBarcodeInf batchBarcodeInf, out string err)
        {
            Debug.Assert(productTypeCode != null && productTypeCode.Length > 0, "参数 'productCode' 不能为 null 或空串");
            Debug.Assert(productAmount > 0, "参数 'productAmount' 必须 > 0");

            batchBarcodeInf = null;
            err = null;

            if (productTypeCode == null || productTypeCode.Length == 0)
            {
                err = "参数 'productCode' 不能为 null 或空串";
                return false;
            }

            if (productAmount < 1)
            {
                err = "参数 'productAmount' 必须 > 0";
                return false;
            }

            // 执行存储过程从数据库中获取数据表
            Hashtable paramTable = new Hashtable();

            paramTable.Add("@productCode", productTypeCode);

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(assemblyName))
                paramTable.Add("@AssemblyName", DBNull.Value);
            else
                paramTable.Add("@AssemblyName", assemblyName);

            DataSet ds = new DataSet();

            if (!AccessDB.ExecuteDbProcedure("SelP_AssemblyLineBarcode", paramTable, ds, out err))
            {
                return false;
            }

            DataTable dt = ds.Tables[0];

            if (dt == null || dt.Rows.Count == 0)
            {
                err = "没有获取到装配条码信息";
                return false;
            }

            // 创建存储生成的管理条形码信息的对象
            BatchBarcodeInf managementBarcode = new BatchBarcodeInf();

            // 从数据表中提取生成条形码的源数据
            ExtractSourceDataFormDT(productTypeCode, productAmount, dt, managementBarcode);

            int serialNumber = beginSerialNumber;

            // 根据规则生成条形码
            foreach (KeyValuePair<string, AssemblyManagementBarcode> var in managementBarcode.Barcodes)
            {
                var.Value.SerialNumber = serialNumber++;

                for (int i = 0; i < productAmount; i++)
                {                    
                    var.Value.Barcode[i] = string.Format("{0} {1:D3}", var.Value.PartCode, beginSerialNumber + i);
                }
            }

            // 输出的条形码信息
            batchBarcodeInf = managementBarcode;
            
            return true;
        }

        /// <summary>
        /// 批量打印条形码
        /// </summary>
        /// <param name="batchBarcodeInf">批量生成的管理条形码信息</param>
        /// <param name="err">错误信息, 如果没有则输出值为null</param>
        /// <returns>成功返回true, 失败返回false</returns>
        public bool PrintBarcodes(BatchBarcodeInf batchBarcodeInf, out string err)
        {
            Debug.Assert(batchBarcodeInf != null, "参数 'batchBarcodeInf' 不能为 null");

            err = null;

            if (batchBarcodeInf == null)
            {
                err = "batchBarcodeInf 不能为 null";
                return false;
            }

            PrintProductBarcodeInfo infoServer = new PrintProductBarcodeInfo();

            foreach (var item in batchBarcodeInf.Barcodes)
            {
                foreach (string barcode in item.Value.Barcode)
                {
                    if (!infoServer.Add(barcode, out err))
                    {
                        return false;
                    }
                }
            }

            return PrintBarcodeForAssembly.PrintBarcode(batchBarcodeInf, out err);
        }

        /// <summary>
        /// 获取零部件的条形码
        /// </summary>
        /// <param name="partCodes">要获取条形码的零部件</param>
        /// <param name="barcodes">零部件代码与条形码配对的字典, 如果某零部件没有获取到条形码则该零部件value值为null</param>
        /// <param name="err">错误信息, 如果没有则输出值为null</param>
        /// <returns>成功返回true, 失败返回false</returns>
        public bool GetBarcodeOfParts(string[] partCodes, out Dictionary<string, string> barcodes, out string err)
        {
            Debug.Assert(partCodes != null && partCodes.Length > 0, "参数 'partCodes' 不能为 null");

            barcodes = null;
            err = null;

            if (partCodes == null)
            {
                err = "partCodes 不能为 null";
                return false;
            }

            string strPartCodes = "";

            for (int i = 0; i < partCodes.Length; i++)
            {
                strPartCodes = strPartCodes.Insert(strPartCodes.Length, partCodes[i]);

                if (i != partCodes.Length - 1)
                {
                    strPartCodes = strPartCodes.Insert(strPartCodes.Length, ",");
                }
            }

            // 执行存储过程从数据库中获取数据表
            Hashtable paramTable = new Hashtable();

            paramTable.Add("@PartCodes", strPartCodes);

            DataSet ds = new DataSet();

            if (!AccessDB.ExecuteDbProcedure("SelectPartBarcode", paramTable, ds, out err))
            {
                return false;
            }

            DataTable dt = ds.Tables[0];

            if (dt == null || dt.Rows.Count == 0)
            {
                err = "没有获取到装配条码信息";
                return false;
            }

            barcodes = new Dictionary<string, string>(dt.Rows.Count);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                barcodes.Add(dt.Rows[i][0].ToString(), dt.Rows[i][1].ToString());
            }

            return true;
        }

        #endregion public method

        #region private method

        /// <summary>
        /// 从数据表中提取生成条形码的源数据
        /// </summary>
        /// <param name="productCode">产品代码</param>
        /// <param name="productAmount">本班次生产的产品数量,直接影响生成的条码数</param>
        /// <param name="dt">数据表</param>
        /// <param name="managementBarcode">提取到的用于生成管理条形码的数据</param>
        private void ExtractSourceDataFormDT(string productCode, int productAmount, DataTable dt, BatchBarcodeInf managementBarcode)
        {
            // 列名
            string colName;

            // 数据值
            object value;

            // 设置主总成代码(从DB中获取的主总成代码为空或空串)
            bool bSetUnitCode = false;

            // 从数据库中获取要生成管理条形码的数据项及信息
            for (int row = 0; row < dt.Rows.Count; row++)
            {
                AssemblyManagementBarcode amb = new AssemblyManagementBarcode(dt.Columns.Count, productAmount);

                for (int col = 0; col < dt.Columns.Count; col++)
                {
                    colName = dt.Columns[col].Caption;
                    value = dt.Rows[row][col];

                    if (colName.CompareTo("PartCode") == 0)
                    {
                        if (!bSetUnitCode)
                        {
                            // 设置主总成代码
                            if (value == null || value.ToString().Length == 0)
                            {
                                dt.Rows[row][col] = productCode;
                            }

                            bSetUnitCode = true;
                        }

                        amb.PartCode = dt.Rows[row][col].ToString();
                    }
                    else if (amb.PartName == null && colName.CompareTo("PartName") == 0)
                    {
                        amb.PartName = value.ToString();
                    }
                    else if (amb.ProductCode == null && colName.CompareTo("ProductType") == 0)
                    {
                        amb.ProductCode = value.ToString();
                    }
                    else if (colName.CompareTo("BuildRule") == 0)
                    {
                        amb.BuildRule = value.ToString();
                    }
                    else if (colName.CompareTo("SerialNumber") == 0)
                    {
                        amb.SerialNumber = Convert.ToInt32(value);
                    }

                    amb.SourceData[col] = value;
                }

                // 如果是主总成则其零部件代码为空或空串
                managementBarcode.Barcodes.Add(amb.PartName, amb);
            }
        }

        /// <summary>
        /// 生成条形码
        /// </summary>
        /// <param name="productCode">产品代码</param>
        /// <param name="productAmount">本班次生产的产品数量,直接影响生成的条码数</param>
        /// <param name="dateTimeValue">日期</param>
        /// <param name="barcodes">生成的条形码信息</param>
        private void BuildBarcode(string productCode, int productAmount, DateTime dateTimeValue, AssemblyManagementBarcode barcodes)
        {
            barcodes.BuildRule = barcodes.BuildRule.ToUpper();

            for (int i = 0; i < productAmount; i++)
            {
                barcodes.SourceData[5] = barcodes.SerialNumber++;

                if (productCode == "RDC15FK-1500000"
                    || productCode == "RDC18FC-1500000"
                    || productCode == "RDC18FC-A")
                {
                    DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
                    barcodes.BuildRule = ctx.Fun_get_ProductCode_DongFengXiaoKang(dateTimeValue);
                    barcodes.SourceData[9] = barcodes.BuildRule;
                    ctx.SubmitChanges();

                    #region 夏石友 2016-11-18 修改

                    barcodes.Barcode[i] = string.Format("{0} {1}{2:D5}",
                        productCode, barcodes.BuildRule, barcodes.SourceData[5]);

                    //barcodes.Barcode[i] = string.Format("{0} {1}{2:D5}",
                    //    productCode.Substring(0, productCode.IndexOf("-")), barcodes.BuildRule, barcodes.SourceData[5]);

                    #endregion                    
                }
                else
                {
                    if (barcodes.BuildRule.Contains("[YYYYMM]"))
                    {
                        barcodes.BuildRule = barcodes.BuildRule.Replace("[YYYYMM]", "{0}");
                        barcodes.SourceData[0] = dateTimeValue.ToString("yyyyMM");
                        barcodes.SourceData[9] = barcodes.BuildRule;
                    }
                    else if (barcodes.BuildRule.Contains("[YYMM]"))
                    {
                        barcodes.BuildRule = barcodes.BuildRule.Replace("[YYMM]", "{0}");
                        barcodes.SourceData[0] = dateTimeValue.ToString("yyMM");
                        barcodes.SourceData[9] = barcodes.BuildRule;
                    }
                    else if (barcodes.BuildRule.Contains("[YYMMDD]"))
                    {
                        barcodes.BuildRule = barcodes.BuildRule.Replace("[YYMMDD]", "{0}");
                        barcodes.SourceData[0] = dateTimeValue.ToString("yyMMdd");
                        barcodes.SourceData[9] = barcodes.BuildRule;
                    }

                    barcodes.Barcode[i] = string.Format(barcodes.BuildRule, barcodes.SourceData);
                }
            }
        }

        /// <summary>
        /// 更新数据库中用于生成管理条形码的流水号数据(流水号在在以前的基础上+产品数量)
        /// </summary>
        /// <param name="assemblyName">总成名称, 仅对独立装配有效，整台装配时此值必须为NULL</param>
        /// <param name="batchBarcodeInf">条形码信息</param>
        /// <param name="productAmount">产品数量</param>
        /// <param name="err">错误信息, 如果没有则输出值为null</param>
        /// <returns>成功返回true, 失败返回false</returns>
        private bool UpdateSerialNumber(string assemblyName, BatchBarcodeInf batchBarcodeInf, int productAmount, out string err)
        {
            Debug.Assert(batchBarcodeInf != null && batchBarcodeInf.Barcodes != null, "参数 'batchBarcodeInf' 不能为 null");
            Debug.Assert(productAmount > 0, "参数 'productAmount' 必须 > 0");

            err = null;

            if (batchBarcodeInf == null || batchBarcodeInf.Barcodes == null)
            {
                err = "参数 'batchBarcodeInf' 不能为 null";
                return false;
            }

            if (productAmount < 1)
            {
                err = "参数 'productAmount' 必须 > 0";
                return false;
            }

            // 执行存储过程从数据库中获取数据表
            Hashtable paramTable = new Hashtable();

            int productTypeID = 0;

            foreach (KeyValuePair<string, AssemblyManagementBarcode> var in batchBarcodeInf.Barcodes)
            {
                productTypeID = Convert.ToInt32(var.Value.SourceData[1]);
                break;
            }

            paramTable.Add("@ProductTypeID", productTypeID);

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(assemblyName))
                paramTable.Add("@AssemblyName", DBNull.Value);
            else
                paramTable.Add("@AssemblyName", assemblyName);

            paramTable.Add("@UpdateAmount", productAmount);

            if (!AccessDB.ExecuteDbProcedure("UpdateSerialNoOfAssemblyLineBarcode", paramTable, out err))
            {
                return false;
            }

            return true;
        }

        #endregion private method
    }
}
