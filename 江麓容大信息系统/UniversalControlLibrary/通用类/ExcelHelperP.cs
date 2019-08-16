using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.IO;
using System.Text;
using NPOI;
using System.Windows.Forms;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System.Data.OleDb;
using System.Reflection;
using GlobalObject;

namespace UniversalControlLibrary
{
    public static class ExcelHelperP
    {
        ///
        /// 根据Excel列类型获取列的值
        ///
        /// Excel列
        ///
        static string GetCellValue(ICell cell)
        {
            if (cell == null)
                return string.Empty;
            switch (cell.CellType)
            {
                case CellType.Blank:
                    return string.Empty;
                case CellType.Boolean:
                    return cell.BooleanCellValue.ToString();
                case CellType.Error:
                    return cell.ErrorCellValue.ToString();
                case CellType.Numeric:
                    if (DateUtil.IsCellDateFormatted(cell))
                        return DateTime.FromOADate(cell.NumericCellValue).ToString();
                    else
                        return cell.ToString();
                case CellType.Unknown:
                default:
                    return cell.ToString();//This is a trick to get the correct value of the cell. NumericCellValue will return a numeric value no matter the cell value is a date or a number
                case CellType.String:
                    return cell.StringCellValue;
                case CellType.Formula:
                    switch (cell.CachedFormulaResultType)
                    {
                        case CellType.String:
                            string strFORMULA = cell.StringCellValue;
                            if (strFORMULA != null && strFORMULA.Length > 0)
                                return strFORMULA.ToString();
                            else
                                return null;
                        case CellType.Numeric:
                            return Convert.ToString(cell.NumericCellValue);
                        case CellType.Boolean:
                            return Convert.ToString(cell.BooleanCellValue);
                        case CellType.Error:
                            return cell.ErrorCellValue.ToString();
                        default: ; break;
                    }
                    try
                    {
                        HSSFFormulaEvaluator e = new HSSFFormulaEvaluator(cell.Sheet.Workbook);
                        e.EvaluateInCell(cell);
                        return cell.ToString();
                    }
                    catch
                    {
                        return cell.NumericCellValue.ToString();
                    }
            }
        }

        ///
        /// 自动设置Excel列宽
        ///
        /// Excel表
        static void AutoSizeColumns(ISheet sheet)
        {
            if (sheet.PhysicalNumberOfRows > 0)
            {
                IRow headerRow = sheet.GetRow(0);
                for (int i = 0, l = headerRow.LastCellNum; i < l; i++)
                {
                    sheet.AutoSizeColumn(i);
                }
            }
        }

        ///
        /// 保存Excel文档流到文件
        ///
        /// Excel文档流
        /// 文件名
        static void SaveToFile(MemoryStream ms, string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                byte[] data = ms.ToArray();
                fs.Write(data, 0, data.Length);
                fs.Flush();
                data = null;
            }
        }

        ///
        /// DataReader转换成Excel文档流
        ///
        ///
        ///
        static MemoryStream RenderToExcel(IDataReader reader)
        {
            MemoryStream ms = new MemoryStream();
            using (reader)
            {
                IWorkbook workbook = new HSSFWorkbook();
                ISheet sheet = workbook.CreateSheet();

                IRow headerRow = sheet.CreateRow(0);
                int cellCount = reader.FieldCount;
                // handling header.
                for (int i = 0; i < cellCount; i++)
                {
                    headerRow.CreateCell(i).SetCellValue(reader.GetName(i));
                }
                // handling value.
                int rowIndex = 1;
                while (reader.Read())
                {
                    IRow dataRow = sheet.CreateRow(rowIndex);
                    for (int i = 0; i < cellCount; i++)
                    {
                        dataRow.CreateCell(i).SetCellValue(reader[i].ToString());
                    }
                    rowIndex++;
                }
                AutoSizeColumns(sheet);
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
            }
            return ms;
        }

        ///
        /// DataReader转换成Excel文档流，并保存到文件
        ///
        ///
        /// 保存的路径
        static void RenderToExcel(IDataReader reader, string fileName)
        {
            using (MemoryStream ms = RenderToExcel(reader))
            {
                SaveToFile(ms, fileName);
            }
        }

        ///
        /// DataTable转换成Excel文档流
        ///
        ///
        ///
        static MemoryStream RenderToExcel(DataTable table, CE_SystemFileType fileType)
        {
            MemoryStream ms = new MemoryStream();
            using (table)
            {
                IWorkbook workbook = null;
                ISheet sheet = null;
                IRow headerRow = null;

                if (fileType == CE_SystemFileType.Excel)
                {
                    #region xls
                    workbook = new HSSFWorkbook();
                    sheet = workbook.CreateSheet();
                    headerRow = sheet.CreateRow(0);

                    // handling header.
                    foreach (DataColumn column in table.Columns)
                    {
                        headerRow.CreateCell(column.Ordinal).SetCellValue(column.Caption);//If Caption not set, returns the ColumnName value
                    }

                    // handling value.
                    int rowIndex = 1;
                    HSSFCellStyle dateStyle = (HSSFCellStyle)workbook.CreateCellStyle();
                    HSSFDataFormat format = (HSSFDataFormat)workbook.CreateDataFormat();
                    dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");
                    foreach (DataRow row in table.Rows)
                    {
                        IRow dataRow = sheet.CreateRow(rowIndex);
                        foreach (DataColumn column in table.Columns)
                        {
                            string drValue = row[column] == null ? "" : row[column].ToString();
                            HSSFCell newCell = (HSSFCell)dataRow.CreateCell(column.Ordinal);
                            switch (column.DataType.ToString())
                            {
                                case "System.String"://字符串类型
                                    newCell.SetCellValue(drValue);
                                    break;
                                case "System.DateTime"://日期类型
                                    DateTime dateV;
                                    DateTime.TryParse(drValue, out dateV);
                                    newCell.SetCellValue(dateV);
                                    newCell.CellStyle = dateStyle;//格式化显示
                                    break;
                                case "System.Boolean"://布尔型
                                    bool boolV = false;
                                    bool.TryParse(drValue, out boolV);
                                    newCell.SetCellValue(boolV);
                                    break;
                                case "System.Int16"://整型
                                case "System.Int32":
                                case "System.Int64":
                                case "System.Byte":
                                    int intV = 0;
                                    int.TryParse(drValue, out intV);
                                    newCell.SetCellValue(intV);
                                    break;
                                case "System.Decimal"://浮点型
                                case "System.Double":
                                    double doubV = 0;
                                    double.TryParse(drValue, out doubV);
                                    newCell.SetCellValue(doubV);
                                    break;
                                case "System.DBNull"://空值处理
                                    newCell.SetCellValue("");
                                    break;
                                default:
                                    newCell.SetCellValue("");
                                    break;
                            }
                        }
                        rowIndex++;
                    }
                    #endregion
                }
                else if (fileType == CE_SystemFileType.Excel2010)
                {
                    #region xlsx
                    workbook = new XSSFWorkbook();
                    sheet = workbook.CreateSheet();
                    headerRow = sheet.CreateRow(0);

                    // handling header.
                    foreach (DataColumn column in table.Columns)
                    {
                        headerRow.CreateCell(column.Ordinal).SetCellValue(column.Caption);//If Caption not set, returns the ColumnName value
                    }

                    // handling value.
                    int rowIndex = 1;
                    XSSFCellStyle dateStyle = (XSSFCellStyle)workbook.CreateCellStyle();
                    XSSFDataFormat format = (XSSFDataFormat)workbook.CreateDataFormat();
                    dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");
                    foreach (DataRow row in table.Rows)
                    {
                        IRow dataRow = sheet.CreateRow(rowIndex);
                        foreach (DataColumn column in table.Columns)
                        {
                            string drValue = row[column] == null ? "" : row[column].ToString();
                            XSSFCell newCell = (XSSFCell)dataRow.CreateCell(column.Ordinal);
                            switch (column.DataType.ToString())
                            {
                                case "System.String"://字符串类型
                                    newCell.SetCellValue(drValue);
                                    break;
                                case "System.DateTime"://日期类型
                                    DateTime dateV;
                                    DateTime.TryParse(drValue, out dateV);
                                    newCell.SetCellValue(dateV);
                                    newCell.CellStyle = dateStyle;//格式化显示
                                    break;
                                case "System.Boolean"://布尔型
                                    bool boolV = false;
                                    bool.TryParse(drValue, out boolV);
                                    newCell.SetCellValue(boolV);
                                    break;
                                case "System.Int16"://整型
                                case "System.Int32":
                                case "System.Int64":
                                case "System.Byte":
                                    int intV = 0;
                                    int.TryParse(drValue, out intV);
                                    newCell.SetCellValue(intV);
                                    break;
                                case "System.Decimal"://浮点型
                                case "System.Double":
                                    double doubV = 0;
                                    double.TryParse(drValue, out doubV);
                                    newCell.SetCellValue(doubV);
                                    break;
                                case "System.DBNull"://空值处理
                                    newCell.SetCellValue("");
                                    break;
                                default:
                                    newCell.SetCellValue("");
                                    break;
                            }
                        }
                        rowIndex++;
                    }
                    #endregion
                }

                AutoSizeColumns(sheet);
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
            }
            return ms;
        }

        ///
        /// DataTable转换成Excel文档流
        ///
        ///
        ///
        static MemoryStream DatagridviewToExcel(DataGridView myDgv, CE_SystemFileType fileType)
        {
            MemoryStream ms = new MemoryStream();

            IWorkbook workbook = null;
            ISheet sheet = null;
            IRow headerRow = null;

            try
            {
                if (fileType == CE_SystemFileType.Excel)
                {
                    #region xls
                    workbook = new HSSFWorkbook();
                    sheet = workbook.CreateSheet();
                    headerRow = sheet.CreateRow(0);

                    // handling header.
                    int columnIndex = 0;
                    foreach (DataGridViewColumn column in myDgv.Columns)
                    {
                        if (!column.Visible)
                        {
                            continue;
                        }

                        headerRow.CreateCell(columnIndex).SetCellValue(column.HeaderText);//If Caption not set, returns the ColumnName value
                        columnIndex++;
                    }

                    // handling value.
                    HSSFCellStyle dateStyle = (HSSFCellStyle)workbook.CreateCellStyle();
                    HSSFDataFormat format = (HSSFDataFormat)workbook.CreateDataFormat();
                    dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");
                    foreach (DataGridViewRow row in myDgv.Rows)
                    {
                        IRow dataRow = sheet.CreateRow(row.Index + 1);
                        columnIndex = 0;
                        foreach (DataGridViewColumn column in myDgv.Columns)
                        {
                            if (!column.Visible)
                            {
                                continue;
                            }
                               string drValue = myDgv.Rows[row.Index].Cells[column.Index].Value == null || !column.Visible ? "" :
                                myDgv.Rows[row.Index].Cells[column.Index].Value.ToString();
                            HSSFCell newCell = (HSSFCell)dataRow.CreateCell(columnIndex);

                            if (column.ValueType == null)
                            {
                                column.ValueType = typeof(string);
                            }

                            switch (column.ValueType.ToString())
                            {
                                case "System.String"://字符串类型
                                    newCell.SetCellValue(drValue);
                                    break;
                                case "System.DateTime"://日期类型
                                    DateTime dateV;
                                    DateTime.TryParse(drValue, out dateV);
                                    newCell.SetCellValue(dateV);
                                    newCell.CellStyle = dateStyle;//格式化显示
                                    break;
                                case "System.Boolean"://布尔型
                                    bool boolV = false;
                                    bool.TryParse(drValue, out boolV);
                                    newCell.SetCellValue(boolV);
                                    break;
                                case "System.Int16"://整型
                                case "System.Int32":
                                case "System.Int64":
                                case "System.Byte":
                                    int intV = 0;
                                    int.TryParse(drValue, out intV);
                                    newCell.SetCellValue(intV);
                                    break;
                                case "System.Decimal"://浮点型
                                case "System.Double":
                                    double doubV = 0;
                                    double.TryParse(drValue, out doubV);
                                    newCell.SetCellValue(doubV);
                                    break;
                                case "System.DBNull"://空值处理
                                    newCell.SetCellValue("");
                                    break;
                                default:
                                    newCell.SetCellValue("");
                                    break;
                            }

                            columnIndex++;
                        }
                    }
                    #endregion
                }
                else if (fileType == CE_SystemFileType.Excel2010)
                {
                    #region xlsx
                    workbook = new XSSFWorkbook();
                    sheet = workbook.CreateSheet();
                    headerRow = sheet.CreateRow(0);

                    // handling header.
                    int columnIndex = 0;
                    foreach (DataGridViewColumn column in myDgv.Columns)
                    {
                        if (!column.Visible)
                        {
                            continue;
                        }

                        headerRow.CreateCell(columnIndex).SetCellValue(column.HeaderText);//If Caption not set, returns the ColumnName value
                        columnIndex++;
                    }

                    // handling value.
                    XSSFCellStyle dateStyle = (XSSFCellStyle)workbook.CreateCellStyle();
                    XSSFDataFormat format = (XSSFDataFormat)workbook.CreateDataFormat();
                    dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");
                    foreach (DataGridViewRow row in myDgv.Rows)
                    {
                        IRow dataRow = sheet.CreateRow(row.Index + 1);
                        columnIndex = 0;
                        foreach (DataGridViewColumn column in myDgv.Columns)
                        {
                            if (!column.Visible)
                            {
                                continue;
                            }

                            string drValue = myDgv.Rows[row.Index].Cells[column.Index].Value == null || !column.Visible ? "" :
                                myDgv.Rows[row.Index].Cells[column.Index].Value.ToString();
                            XSSFCell newCell = (XSSFCell)dataRow.CreateCell(columnIndex);

                            if (column.ValueType == null)
                            {
                                column.ValueType = typeof(string);
                            }

                            switch (column.ValueType.ToString())
                            {
                                case "System.String"://字符串类型
                                    newCell.SetCellValue(drValue);
                                    break;
                                case "System.DateTime"://日期类型
                                    DateTime dateV;
                                    DateTime.TryParse(drValue, out dateV);
                                    newCell.SetCellValue(dateV);
                                    newCell.CellStyle = dateStyle;//格式化显示
                                    break;
                                case "System.Boolean"://布尔型
                                    bool boolV = false;
                                    bool.TryParse(drValue, out boolV);
                                    newCell.SetCellValue(boolV);
                                    break;
                                case "System.Int16"://整型
                                case "System.Int32":
                                case "System.Int64":
                                case "System.Byte":
                                    int intV = 0;
                                    int.TryParse(drValue, out intV);
                                    newCell.SetCellValue(intV);
                                    break;
                                case "System.Decimal"://浮点型
                                case "System.Double":
                                    double doubV = 0;
                                    double.TryParse(drValue, out doubV);
                                    newCell.SetCellValue(doubV);
                                    break;
                                case "System.DBNull"://空值处理
                                    newCell.SetCellValue("");
                                    break;
                                default:
                                    newCell.SetCellValue("");
                                    break;
                            }

                            columnIndex++;
                        }
                    }
                    #endregion
                }

                AutoSizeColumns(sheet);
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;

                return ms;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        ///// 
        ///// 导出DataTable中的数据
        ///// </summary>
        ///// <param name="fileName">保存路径</param>
        ///// <param name="dt">DataTable数据</param>
        ///// <returns>是否成功</returns>
        //public static bool Export(string fileName, DataTable dt)
        //{
        //    bool isSuccess = false;
        //    Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
        //    excel.Visible = false;
        //    object ms = Type.Missing;
        //    Microsoft.Office.Interop.Excel.Workbook wk = excel.Workbooks.Add(ms);
        //    Microsoft.Office.Interop.Excel.Worksheet ws = wk.Worksheets[1] as Microsoft.Office.Interop.Excel.Worksheet;

        //    for (int i = 0; i < dt.Columns.Count; i++)
        //    {
        //        ws.Cells[1, i + 1] = dt.Columns[i].ColumnName;
        //    }

        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        for (int j = 0; j < dt.Columns.Count; j++)
        //        {
        //            ws.Cells[i + 2, j + 1] = dt.Rows[i][j].ToString();
        //        }
        //    }
        //    try
        //    {
        //        wk.SaveAs(fileName, ms, ms, ms, ms, ms, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlShared, ms, ms, ms, ms, ms);
        //        isSuccess = true;
        //    }
        //    catch (Exception)
        //    {
        //        isSuccess = false;
        //    }
        //    excel.Quit();
        //    return isSuccess;
        //}

        ///
        /// DataTable转换成Excel文档流，并保存到文件
        ///
        ///
        /// 保存的路径
        public static void DataTableToExcel(string fileName, DataTable table, string[] hideColumns)
        {
            DataTable dtTemp = table.Copy();

            if (hideColumns != null)
            {
                foreach (string hideColumnName in hideColumns)
                {
                    dtTemp.Columns.Remove(hideColumnName);
                }
            }

            using (MemoryStream ms = RenderToExcel(dtTemp, GlobalObject.GeneralFunction.GetDocumentType(fileName)))
            {
                SaveToFile(ms, fileName);
            }
        }

        ///
        /// DataTable转换成Excel文档流，并保存到文件
        ///
        ///
        /// 保存的路径
        public static void DataTableToExcel(SaveFileDialog saveDlg, DataTable table, string[] hideColumns)
        {
            if (table == null || table.Rows.Count == 0)
            {
                return;
            }

            saveDlg.AddExtension = true;
            saveDlg.CheckPathExists = true;
            saveDlg.DefaultExt = ".xls";
            saveDlg.Filter = "Excel 文件 (*.xls, *.xlsx)|*.xls;*.xlsx";
            saveDlg.ShowHelp = false;
            saveDlg.Title = "请选择导出路径及文件名称";

            if (saveDlg.ShowDialog() == DialogResult.OK)
            {
                DataTable dtTemp = table.Copy();

                if (hideColumns != null)
                {
                    foreach (string hideColumnName in hideColumns)
                    {
                        dtTemp.Columns.Remove(hideColumnName);
                    }
                }

                using (MemoryStream ms = RenderToExcel(dtTemp, GlobalObject.GeneralFunction.GetDocumentType(saveDlg.FileName)))
                {
                    SaveToFile(ms, saveDlg.FileName);
                }

                if (MessageBox.Show("文件保存完成！\r\n" + saveDlg.FileName + "\r\n    要现在打开文件吗？",
                    "导出成功", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(saveDlg.FileName);
                }
            }
        }

        ///
        ///
        ///
        /// DataGridview转换成Excel文档流，并保存到文件
        /// 保存的路径及文件名
        public static void DatagridviewToExcel(string fileName, DataGridView myDgv)
        {
            using (MemoryStream ms = DatagridviewToExcel(myDgv, GlobalObject.GeneralFunction.GetDocumentType(fileName)))
            {
                SaveToFile(ms, fileName);
            }
        }

        ///
        ///
        ///
        /// DataGridview转换成Excel文档流，并保存到文件
        /// 保存的路径及文件名
        public static void DatagridviewToExcel(SaveFileDialog saveDlg, DataGridView myDgv)
        {
            if (myDgv == null || myDgv.Rows.Count == 0)
            {
                return;
            }

            saveDlg.AddExtension = true;
            saveDlg.CheckPathExists = true;
            saveDlg.DefaultExt = ".xls";
            saveDlg.Filter = "Excel 文件 (*.xls, *.xlsx)|*.xls;*.xlsx";
            saveDlg.ShowHelp = false;
            saveDlg.Title = "请选择导出路径及文件名称";

            if (saveDlg.ShowDialog() == DialogResult.OK)
            {
                using (MemoryStream ms = DatagridviewToExcel(myDgv, GlobalObject.GeneralFunction.GetDocumentType(saveDlg.FileName)))
                {
                    SaveToFile(ms, saveDlg.FileName);
                }

                if (MessageBox.Show("文件保存完成！\r\n" + saveDlg.FileName + "\r\n    要现在打开文件吗？",
                    "导出成功", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(saveDlg.FileName);
                }
            }
        }

        ///
        /// Excel文档流是否有数据
        ///
        /// Excel文档流
        ///
        public static bool HasData(Stream excelFileStream)
        {
            return HasData(excelFileStream, 0);
        }

        ///
        /// Excel文档流是否有数据
        ///
        /// Excel文档流
        /// 表索引号，如第一个表为0
        ///
        public static bool HasData(Stream excelFileStream, int sheetIndex)
        {
            using (excelFileStream)
            {
                IWorkbook workbook = new HSSFWorkbook(excelFileStream);
                if (workbook.NumberOfSheets > 0)
                {
                    if (sheetIndex < workbook.NumberOfSheets)
                    {
                        ISheet sheet = workbook.GetSheetAt(sheetIndex);
                        return sheet.PhysicalNumberOfRows > 0;
                    }
                }
            }
            return false;
        }

        ///// 
        ///// 导入EXCEL返回TABLE
        ///// </summary>
        ///// <param name="fileName">文件名</param>
        ///// <param name="error">出错时返回错误信息，无错时返回null</param>
        ///// <returns>返回Table</returns>
        //public static DataTable RenderFromExcel(string fileName)
        //{
        //    string error = null;

        //    try
        //    {
        //        if (fileName.Trim().Length > 0)
        //        {
        //            if (!System.IO.File.Exists(fileName))
        //            {
        //                error = "不存在的数据文件：" + fileName;
        //                return null;
        //            }
        //        }

        //        if (GlobalObject.GeneralFunction.IsNullOrEmpty(fileName))
        //        {
        //            error = "请选择要导入的文件后再进行此操作！";
        //            return null;
        //        }

        //        // 创建一个数据链接
        //        string strConn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + ";Extended Properties='Excel 12.0; HDR=Yes; IMEX=1'";

        //        OleDbConnection myConn = new OleDbConnection(strConn);

        //        string strComm = "Select * from [Sheet1$A:CV]";

        //        myConn.Open();

        //        // 打开数据链接，得到一个数据集
        //        OleDbDataAdapter myCommand = new OleDbDataAdapter(strComm, strConn);

        //        // 创建一个DataSet对象
        //        DataSet ds = new DataSet();

        //        // 得到自己的DataSet对象
        //        myCommand.Fill(ds, "[Sheet1$]");
        //        myCommand.Dispose();
        //        myConn.Close();
        //        myConn.Dispose();

        //        return ds.Tables[0];
        //    }
        //    catch (Exception exce)
        //    {
        //        throw new Exception(exce.Message);
        //    }
        //}

        ///
        /// Excel文档流转换成DataTable
        /// 默认转换Excel的第一个表
        /// 第一行必须为标题行
        ///
        /// Excel文档流
        ///
        public static DataTable RenderFromExcel(Stream excelFileStream)
        {
            DataTable table = null;
            try
            {
                using (excelFileStream)
                {
                    IWorkbook workbook = null;
                    GlobalObject.CE_SystemFileType fileType =
                        GlobalObject.GeneralFunction.GetDocumentType(((System.IO.FileStream)excelFileStream).Name);

                    if (fileType == GlobalObject.CE_SystemFileType.Excel)
                    {
                        workbook = new HSSFWorkbook(excelFileStream);
                    }
                    else if (fileType == GlobalObject.CE_SystemFileType.Excel2010)
                    {
                        workbook = new XSSFWorkbook(excelFileStream);
                    }

                    ISheet sheet = workbook.GetSheetAt(0);//默认转换Excel的第一个表

                    table = RenderFromExcel(sheet, 0);// 第一行必须为标题行 标题行索引号 为0 
                }

                return table;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 将制定sheet中的数据导出到datatable中
        /// </summary>
        /// <param name="sheet">需要导出的sheet</param>
        /// <param name="HeaderRowIndex">列头所在行号，-1表示没有列头</param>
        /// <returns></returns>
        static DataTable ImportDt(ISheet sheet, int HeaderRowIndex, bool needHeader)
        {
            DataTable table = new DataTable();
            IRow headerRow;
            int cellCount;

            if (HeaderRowIndex < 0 || !needHeader)
            {
                headerRow = sheet.GetRow(0);
                cellCount = headerRow.LastCellNum;

                for (int i = headerRow.FirstCellNum; i <= cellCount; i++)
                {
                    DataColumn column = new DataColumn(Convert.ToString(i));
                    table.Columns.Add(column);
                }
            }
            else
            {
                headerRow = sheet.GetRow(HeaderRowIndex);

                if (headerRow == null)
                {
                    throw new Exception("无【列名】");
                }

                cellCount = headerRow.LastCellNum;

                for (int i = headerRow.FirstCellNum; i <= cellCount; i++)
                {
                    if (headerRow.GetCell(i) == null)
                    {
                        if (table.Columns.IndexOf(Convert.ToString(i)) > 0)
                        {
                            DataColumn column = new DataColumn(Convert.ToString("重复列名" + i));
                            table.Columns.Add(column);
                        }
                        else
                        {
                            DataColumn column = new DataColumn(Convert.ToString(i));
                            table.Columns.Add(column);
                        }

                    }
                    else if (table.Columns.IndexOf(headerRow.GetCell(i).ToString()) > 0)
                    {
                        DataColumn column = new DataColumn(Convert.ToString("重复列名" + i));
                        table.Columns.Add(column);
                    }
                    else
                    {
                        DataColumn column = new DataColumn(headerRow.GetCell(i).ToString());
                        table.Columns.Add(column);
                    }
                }
            }

            int rowCount = sheet.LastRowNum;

            for (int i = (HeaderRowIndex + 1); i <= sheet.LastRowNum; i++)
            {
                IRow row;

                if (sheet.GetRow(i) == null)
                {
                    row = sheet.CreateRow(i);
                }
                else
                {
                    row = sheet.GetRow(i);
                }

                DataRow dataRow = table.NewRow();

                for (int j = row.FirstCellNum; j <= cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                    {
                        switch (row.GetCell(j).CellType)
                        {
                            case CellType.String:
                                string str = row.GetCell(j).StringCellValue;
                                if (str != null && str.Length > 0)
                                {
                                    dataRow[j] = str.ToString();
                                }
                                else
                                {
                                    dataRow[j] = null;
                                }
                                break;
                            case CellType.Numeric:
                                if (DateUtil.IsCellDateFormatted(row.GetCell(j)))
                                {
                                    dataRow[j] = DateTime.FromOADate(row.GetCell(j).NumericCellValue);
                                }
                                else
                                {
                                    dataRow[j] = Convert.ToDouble(row.GetCell(j).NumericCellValue);
                                }
                                break;
                            case CellType.Boolean:
                                dataRow[j] = Convert.ToString(row.GetCell(j).BooleanCellValue);
                                break;
                            case CellType.Error:
                                dataRow[j] = Convert.ToString(row.GetCell(j).ErrorCellValue);
                                break;
                            case CellType.Formula:
                                switch (row.GetCell(j).CachedFormulaResultType)
                                {
                                    case CellType.String:
                                        string strFORMULA = row.GetCell(j).StringCellValue;
                                        if (strFORMULA != null && strFORMULA.Length > 0)
                                        {
                                            dataRow[j] = strFORMULA.ToString();
                                        }
                                        else
                                        {
                                            dataRow[j] = null;
                                        }
                                        break;
                                    case CellType.Numeric:
                                        dataRow[j] = Convert.ToString(row.GetCell(j).NumericCellValue);
                                        break;
                                    case CellType.Boolean:
                                        dataRow[j] = Convert.ToString(row.GetCell(j).BooleanCellValue);
                                        break;
                                    case CellType.Error:
                                        dataRow[j] = Convert.ToString(row.GetCell(j).ErrorCellValue);
                                        break;
                                    default:
                                        dataRow[j] = "";
                                        break;
                                }
                                break;
                            default:
                                dataRow[j] = "";
                                break;
                        }
                    }
                }

                table.Rows.Add(dataRow);
            }

            return table;
        }

        ///
        /// Excel文档流转换成DataTable
        /// 默认转换Excel的第一个表
        /// 第一行必须为标题行
        ///
        /// Excel文档流
        ///
        public static DataTable RenderFromExcel(OpenFileDialog openDlg)
        {
            //DataTable table = null;

            openDlg.AddExtension = true;
            openDlg.CheckPathExists = true;
            openDlg.DefaultExt = ".xls";
            openDlg.Filter = "Excel 文件 (*.xls, *.xlsx)|*.xls;*.xlsx";
            openDlg.ShowHelp = false;
            openDlg.Title = "请选择导入路径及文件名称";

            DialogResult result = openDlg.ShowDialog();

            if (result == DialogResult.Cancel || result == DialogResult.No)
            {
                return null;
            }

            try
            {
                Stream excelFileStream = openDlg.OpenFile();

                using (excelFileStream)
                {
                    #region 2017.09.29 可以支持2003\2007\2010等多种EXCEL格式
                    DataTable dt = new DataTable();
                    IWorkbook wb;

                    if (openDlg.FileName.ToLower().Equals(".xls"))
                    {
                        wb = new HSSFWorkbook(excelFileStream);
                    }
                    else
                    {
                        wb = WorkbookFactory.Create(excelFileStream);
                    }

                    ISheet sheet = wb.GetSheetAt(0);
                    dt = ImportDt(sheet, 0, true);
                    return dt;

                    // 下面内容为原来旧模式，启用以上功能后屏蔽
                    //IWorkbook workbook = null;
                    //GlobalObject.CE_SystemFileType fileType =
                    //    GlobalObject.GeneralFunction.GetDocumentType(((System.IO.FileStream)excelFileStream).Name);

                    //if (fileType == GlobalObject.CE_SystemFileType.Excel)
                    //{
                    //    workbook = new HSSFWorkbook(excelFileStream);
                    //}
                    //else if (fileType == GlobalObject.CE_SystemFileType.Excel2010)
                    //{
                    //    workbook = new XSSFWorkbook(excelFileStream);
                    //}

                    //ISheet sheet = workbook.GetSheetAt(0);//默认转换Excel的第一个表

                    //table = RenderFromExcel(sheet, 0);// 第一行必须为标题行 标题行索引号 为0
                    #endregion
                }

                //return table;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        ///
        /// Excel表格转换成DataTable
        ///
        /// 表格
        /// 标题行索引号，如第一行为0
        ///
        private static DataTable RenderFromExcel(ISheet sheet, int headerRowIndex)
        {
            DataTable table = new DataTable();
            IRow headerRow = sheet.GetRow(headerRowIndex);
            int cellCount = headerRow.LastCellNum;//LastCellNum = PhysicalNumberOfCells
            int rowCount = sheet.LastRowNum;//LastRowNum = PhysicalNumberOfRows - 1
            //handling header.
            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                DataColumn column = new DataColumn(headerRow.GetCell(i) == null ? "" : headerRow.GetCell(i).StringCellValue);
                table.Columns.Add(column);
            }
            for (int i = (sheet.FirstRowNum + 1 + headerRowIndex); i <= rowCount; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow dataRow = table.NewRow();
                if (row != null)
                {
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null)
                            dataRow[j] = GetCellValue(row.GetCell(j));
                    }
                }
                table.Rows.Add(dataRow);
            }
            return table;
        }

        ///
        /// Excel文档导入到数据库
        /// 默认取Excel的第一个表
        /// 第一行必须为标题行
        ///
        /// Excel文档流
        /// 插入语句
        /// 更新到数据库的方法
        ///
        public static int RenderToDb(Stream excelFileStream, string insertSql, GlobalObject.DelegateCollection.DBAction dbAction)
        {
            return RenderToDb(excelFileStream, insertSql, dbAction, 0, 0);
        }

        ///
        /// Excel文档导入到数据库
        ///
        /// Excel文档流
        /// 插入语句
        /// 更新到数据库的方法
        /// 表索引号，如第一个表为0
        /// 标题行索引号，如第一行为0
        ///
        public static int RenderToDb(Stream excelFileStream, string insertSql, GlobalObject.DelegateCollection.DBAction dbAction, int sheetIndex, int headerRowIndex)
        {
            int rowAffected = 0;
            using (excelFileStream)
            {
                IWorkbook workbook = new HSSFWorkbook(excelFileStream);
                ISheet sheet = workbook.GetSheetAt(sheetIndex);

                StringBuilder builder = new StringBuilder();
                IRow headerRow = sheet.GetRow(headerRowIndex);
                int cellCount = headerRow.LastCellNum;//LastCellNum = PhysicalNumberOfCells
                int rowCount = sheet.LastRowNum;//LastRowNum = PhysicalNumberOfRows - 1
                for (int i = (sheet.FirstRowNum + 1); i <= rowCount; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row != null)
                    {
                        builder.Append(insertSql);
                        builder.Append(" values (");
                        for (int j = row.FirstCellNum; j < cellCount; j++)
                        {
                            builder.AppendFormat("'{0}',", GetCellValue(row.GetCell(j)).Replace("'", "''"));
                        }
                        builder.Length = builder.Length - 1;
                        builder.Append(");");
                    }

                    if ((i % 50 == 0 || i == rowCount) && builder.Length > 0)
                    {
                        //每50条记录一次批量插入到数据库
                        rowAffected += dbAction(builder.ToString());
                        builder.Length = 0;
                    }
                }
            }
            return rowAffected;
        }
    }
}
