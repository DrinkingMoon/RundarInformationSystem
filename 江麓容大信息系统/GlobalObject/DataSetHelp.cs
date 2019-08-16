/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  DataSetHelp.cs
 * 作者    :  曹津彬    版本: v1.00    日期: 2014/01/22
 * 开发平台:  Visual C# 2008
 * 用于    :  仓库管理软件
 *----------------------------------------------------------------------------
 * 描述 : 
 * 其它 :
 *----------------------------------------------------------------------------
 * 公共信息: 参见系统'类帮助文档'
 *----------------------------------------------------------------------------
 * 历史记录:
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Data.SqlClient;
using System.Xml.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq.Expressions;
using System.Data.OleDb;
using System.Collections;

namespace GlobalObject
{
    /// <summary>
    /// 直接对TABLE进行SELECT,JOIN,GROUPBY等数据库操作，并且返回TABLE
    /// </summary>
    public static class DataSetHelper 
    {
        private class FieldInfo
        {
            public string RelationName;
            public string FieldName;
            public string FieldAlias;
            public string Aggregate;
        }

        private static  DataSet ds = null;
        private static  ArrayList m_FieldInfo;
        private static  string m_FieldList;
        private static  ArrayList GroupByFieldInfo;
        private static  string GroupByFieldList;

        public static DataSet DataSet
        {
            get { return ds; }
        }

        public static List<string> ColumnsToList(DataTable dt, string columnName)
        {
            dt.AcceptChanges();
            List<string> listResult = new List<string>();

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    listResult.Add(dr[columnName].ToString());
                }
            }

            return listResult;
        }

        public static DataTable Where(this DataTable dt, string where)
        {
            DataTable resultDt = dt.Clone();
            DataRow[] resultRows = dt.Select(where);
            foreach (DataRow dr in resultRows) resultDt.Rows.Add(dr.ItemArray);
            return resultDt;
        }

        public static DataTable OrderBy(this DataTable dt, string orderBy)
        {
            dt.DefaultView.Sort = orderBy;
            return dt.DefaultView.ToTable();
        }

        public static List<string> ColumnsToList_Distinct(DataTable dt, string columnName)
        {
            dt.AcceptChanges();
            List<string> listResult = new List<string>();

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (!listResult.Contains(dr[columnName].ToString()))
                    {
                        listResult.Add(dr[columnName].ToString());
                    }
                }
            }

            return listResult;
        }

        public static List<TResult> ToList<TResult>(this DataTable dt) where TResult : class, new()
        {
            List<TResult> list = new List<TResult>();

            if (dt == null)
                return list;

            DataTableEntityBuilder<TResult> eblist = DataTableEntityBuilder<TResult>.CreateBuilder(dt.Rows[0]);

            foreach (DataRow info in dt.Rows)
                list.Add(eblist.Build(info));

            dt.Dispose();
            dt = null;

            return list;
        }

        #region SiftDataTable

        /// <summary>
        /// 筛选TABLE
        /// </summary>
        /// <param name="beSift">数据源Table</param>
        /// <param name="whereSql">筛选表达式</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回筛选后的Table</returns>
        public static DataTable SiftDataTableSingle(DataTable beSift, string columnName, string value)
        {
            beSift.AcceptChanges();

            if (!beSift.Columns.Contains(columnName))
            {
                return beSift;
            }

            var varData = from a in beSift.AsEnumerable()
                          where (a.Field<object>(columnName)).ToString().Contains(value)
                          select a;

            if (varData.Count() == 0)
            {
                return beSift.Clone();
            }
            else
            {
                return varData.CopyToDataTable<DataRow>();
            }
        }

        /// <summary>
        /// 筛选TABLE
        /// </summary>
        /// <param name="beSift">数据源Table</param>
        /// <param name="whereSql">筛选表达式</param>
        /// <returns>返回筛选后的Table</returns>
        public static DataTable SiftDataTable(DataTable beSift, string whereSql)
        {
            try
            {
                beSift.AcceptChanges();

                DataTable dtReasultSift = beSift.Clone();
                DataRow[] drTemp = beSift.Select(whereSql);

                for (int i = 0; i < drTemp.Length; i++)
                {
                    dtReasultSift.ImportRow(drTemp[i]);
                }

                return dtReasultSift;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 筛选TABLE
        /// </summary>
        /// <param name="beSift">数据源Table</param>
        /// <param name="whereSql">筛选表达式</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回筛选后的Table</returns>
        public static DataTable SiftDataTable(DataTable beSift, string whereSql, out string error)
        {
            error = null;

            try
            {
                beSift.AcceptChanges();

                DataTable dtReasultSift = beSift.Clone();
                DataRow[] drTemp = beSift.Select(whereSql);

                for (int i = 0; i < drTemp.Length; i++)
                {
                    dtReasultSift.ImportRow(drTemp[i]);
                }

                return dtReasultSift;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// 筛选TABLE
        /// </summary>
        /// <param name="beSift">数据源Table</param>
        /// <param name="whereSql">筛选表达式</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回筛选后的Table</returns>
        public static DataTable SiftDataTable(DataTable beSift, string whereSql, string sort, out string error)
        {
            error = null;

            try
            {
                beSift.AcceptChanges();
                DataTable dtReasultSift = beSift.Clone();

                DataRow[] drTemp = beSift.Select(whereSql, sort);

                for (int i = 0; i < drTemp.Length; i++)
                {
                    dtReasultSift.ImportRow(drTemp[i]);
                }

                return dtReasultSift;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
        }
        #endregion

        #region Union All Table
        /// <summary>
        /// TABLE进行UnionAll
        /// </summary>
        /// <param name="firstTable">头表</param>
        /// <param name="secondTable">尾表</param>
        /// <returns>返回Table</returns>
        public static DataTable SelectUnionAll(DataTable firstTable, DataTable secondTable)
        {
            firstTable.AcceptChanges();
            secondTable.AcceptChanges();

            if (firstTable.Columns.Count != secondTable.Columns.Count)
            {
                return null;
            }
            else
            {
                DataTable resultTable = firstTable.Clone();

                int columnsCount = resultTable.Columns.Count;

                foreach (DataRow dr in firstTable.Rows)
                {
                    DataRow tempDr = resultTable.NewRow();

                    for (int i = 0; i < columnsCount; i++)
                    {
                        tempDr[i] = dr[i];
                    }

                    resultTable.Rows.Add(tempDr);
                }

                foreach (DataRow dr in secondTable.Rows)
                {
                    DataRow tempDr = resultTable.NewRow();

                    for (int i = 0; i < columnsCount; i++)
                    {
                        tempDr[i] = dr[i];
                    }

                    resultTable.Rows.Add(tempDr);
                }

                return resultTable;
            }
        }

        #endregion

        #region Construction

        //static DataSetHelper()
        //{
        //    ds = null;
        //}

        //public static DataSetHelper(ref DataSet dataSet)
        //{
        //    ds = dataSet;
        //}

        #endregion

        #region private static static Methods

        private static  bool ColumnEqual(object objectA, object objectB)
        {
            if (objectA == DBNull.Value && objectB == DBNull.Value)
            {
                return true;
            }

            if (objectA == DBNull.Value || objectB == DBNull.Value)
            {
                return false;
            }

            return (objectA.Equals(objectB));
        }

        private static bool RowEqual(DataRow rowA, DataRow rowB, DataColumnCollection columns)
        {
            bool result = true;

            for (int i = 0; i < columns.Count; i++)
            {
                result &= ColumnEqual(rowA[columns[i].ColumnName], rowB[columns[i].ColumnName]);
            }

            return result;
        }

        private static void ParseFieldList(string fieldList, bool allowRelation)
        {
            if (m_FieldList == fieldList)
            {
                return;
            }

            m_FieldInfo = new ArrayList();
            m_FieldList = fieldList;

            FieldInfo Field;
            string[] FieldParts;
            string[] Fields = fieldList.Split(',');

            for (int i = 0; i <= Fields.Length - 1; i++)
            {
                Field = new FieldInfo();
                FieldParts = Fields[i].Trim().Split(' ');

                switch (FieldParts.Length)
                {
                    case 1:
                        //to be set at the end of the loop 
                        break;
                    case 2:
                        Field.FieldAlias = FieldParts[1];
                        break;
                    default:
                        return;
                }

                FieldParts = FieldParts[0].Split('.');

                switch (FieldParts.Length)
                {
                    case 1:
                        Field.FieldName = FieldParts[0];
                        break;
                    case 2:
                        if (allowRelation == false)
                        {
                            return;
                        }
                        Field.RelationName = FieldParts[0].Trim();
                        Field.FieldName = FieldParts[1].Trim();
                        break;
                    default:
                        return;
                }

                if (Field.FieldAlias == null)
                {
                    Field.FieldAlias = Field.FieldName;
                }

                m_FieldInfo.Add(Field);
            }
        }

        private static DataTable CreateTable(string tableName, DataTable sourceTable, string fieldList)
        {
            DataTable dt;

            if (fieldList.Trim() == "")
            {
                dt = sourceTable.Clone();
                dt.TableName = tableName;
            }
            else
            {
                dt = new DataTable(tableName);
                ParseFieldList(fieldList, false);
                DataColumn dc;

                foreach (FieldInfo Field in m_FieldInfo)
                {
                    dc = sourceTable.Columns[Field.FieldName];

                    DataColumn column = new DataColumn();

                    column.ColumnName = Field.FieldAlias;
                    column.DataType = dc.DataType;
                    column.MaxLength = dc.MaxLength;
                    column.Expression = dc.Expression;

                    dt.Columns.Add(column);
                }
            }

            if (ds != null)
            {
                ds.Tables.Add(dt);
            }

            return dt;
        }

        private static void InsertInto(DataTable destTable, DataTable sourceTable,
                                string fieldList, string rowFilter, string sort)
        {
            ParseFieldList(fieldList, false);

            DataRow[] rows = sourceTable.Select(rowFilter, sort);
            DataRow destRow;

            foreach (DataRow sourceRow in rows)
            {
                destRow = destTable.NewRow();

                if (fieldList == "")
                {
                    foreach (DataColumn dc in destRow.Table.Columns)
                    {
                        if (dc.Expression == "")
                        {
                            destRow[dc] = sourceRow[dc.ColumnName];
                        }
                    }
                }
                else
                {
                    foreach (FieldInfo field in m_FieldInfo)
                    {
                        destRow[field.FieldAlias] = sourceRow[field.FieldName];
                    }
                }
                destTable.Rows.Add(destRow);
            }
        }

        private static void ParseGroupByFieldList(string FieldList)
        {
            if (GroupByFieldList == FieldList)
            {
                return;
            }

            GroupByFieldInfo = new ArrayList();

            FieldInfo Field;
            string[] FieldParts;
            string[] Fields = FieldList.Split(',');

            for (int i = 0; i <= Fields.Length - 1; i++)
            {
                Field = new FieldInfo();
                FieldParts = Fields[i].Trim().Split(' ');

                switch (FieldParts.Length)
                {
                    case 1:
                        //to be set at the end of the loop 
                        break;
                    case 2:
                        Field.FieldAlias = FieldParts[1];
                        break;
                    default:
                        return;
                }

                FieldParts = FieldParts[0].Split('(');

                switch (FieldParts.Length)
                {
                    case 1:
                        Field.FieldName = FieldParts[0];
                        break;
                    case 2:
                        Field.Aggregate = FieldParts[0].Trim().ToLower();
                        Field.FieldName = FieldParts[1].Trim(' ', ')');
                        break;
                    default:
                        return;
                }

                if (Field.FieldAlias == null)
                {
                    if (Field.Aggregate == null)
                    {
                        Field.FieldAlias = Field.FieldName;
                    }
                    else
                    {
                        Field.FieldAlias = Field.Aggregate + "of" + Field.FieldName;
                    }
                }

                GroupByFieldInfo.Add(Field);
            }
            GroupByFieldList = FieldList;
        }

        private static DataTable CreateGroupByTable(string tableName, DataTable sourceTable, string fieldList)
        {
            if (fieldList == null || fieldList.Length == 0)
            {
                return sourceTable.Clone();
            }
            else
            {
                DataTable dt = new DataTable(tableName);
                ParseGroupByFieldList(fieldList);

                foreach (FieldInfo Field in GroupByFieldInfo)
                {
                    DataColumn dc = sourceTable.Columns[Field.FieldName];

                    if (Field.Aggregate == null)
                    {
                        dt.Columns.Add(Field.FieldAlias, dc.DataType, dc.Expression);
                    }
                    else
                    {
                        dt.Columns.Add(Field.FieldAlias, dc.DataType);
                    }
                }

                if (ds != null)
                {
                    ds.Tables.Add(dt);
                }

                return dt;
            }
        }

        private static void InsertGroupByInto(DataTable destTable, DataTable sourceTable, string fieldList,
                                       string rowFilter, string groupBy)
        {
            if (fieldList == null || fieldList.Length == 0)
            {
                return;
            }

            ParseGroupByFieldList(fieldList);
            ParseFieldList(groupBy, false);

            DataRow[] rows = sourceTable.Select(rowFilter, groupBy);
            DataRow lastSourceRow = null, destRow = null;

            bool sameRow;
            int rowCount = 0;

            foreach (DataRow sourceRow in rows)
            {
                sameRow = false;

                if (lastSourceRow != null)
                {
                    sameRow = true;

                    foreach (FieldInfo Field in m_FieldInfo)
                    {
                        if (!ColumnEqual(lastSourceRow[Field.FieldName], sourceRow[Field.FieldName]))
                        {
                            sameRow = false;
                            break;
                        }
                    }

                    if (!sameRow)
                    {
                        destTable.Rows.Add(destRow);
                    }
                }

                if (!sameRow)
                {
                    destRow = destTable.NewRow();
                    rowCount = 0;
                }

                rowCount += 1;

                foreach (FieldInfo field in GroupByFieldInfo)
                {
                    switch (field.Aggregate == null ? "" : field.Aggregate.ToLower())
                    {
                        case null:
                        case "":
                        case "last":
                            destRow[field.FieldAlias] = sourceRow[field.FieldName];
                            break;
                        case "first":
                            if (rowCount == 1)
                            {
                                destRow[field.FieldAlias] = sourceRow[field.FieldName];
                            }
                            break;
                        case "count":
                            destRow[field.FieldAlias] = rowCount;
                            break;
                        case "sum":
                            destRow[field.FieldAlias] = Add(destRow[field.FieldAlias], sourceRow[field.FieldName]);
                            break;
                        case "max":
                            destRow[field.FieldAlias] = Max(destRow[field.FieldAlias], sourceRow[field.FieldName]);
                            break;
                        case "min":
                            if (rowCount == 1)
                            {
                                destRow[field.FieldAlias] = sourceRow[field.FieldName];
                            }
                            else
                            {
                                destRow[field.FieldAlias] = Min(destRow[field.FieldAlias], sourceRow[field.FieldName]);
                            }
                            break;
                    }
                }

                lastSourceRow = sourceRow;
            }

            if (destRow != null)
            {
                destTable.Rows.Add(destRow);
            }
        }

        private static object Min(object a, object b)
        {
            if ((a is DBNull) || (b is DBNull))
            {
                return DBNull.Value;
            }

            if (((IComparable)a).CompareTo(b) == -1)
            {
                return a;
            }
            else
            {
                return b;
            }
        }

        private static object Max(object a, object b)
        {
            if (a is DBNull)
            {
                return b;
            }

            if (b is DBNull)
            {
                return a;
            }

            if (((IComparable)a).CompareTo(b) == 1)
            {
                return a;
            }
            else
            {
                return b;
            }
        }

        private static object Add(object a, object b)
        {
            if (a is DBNull)
            {
                return b;
            }

            if (b is DBNull)
            {
                return a;
            }
            return (Convert.ToDecimal(a) + Convert.ToDecimal(b));
        }

        private static DataTable CreateJoinTable(string tableName, DataTable sourceTable, string fieldList)
        {
            if (fieldList == null)
            {
                return sourceTable.Clone();
            }
            else
            {
                DataTable dt = new DataTable(tableName);
                ParseFieldList(fieldList, true);

                foreach (FieldInfo field in m_FieldInfo)
                {
                    if (field.RelationName == null)
                    {
                        DataColumn dc = sourceTable.Columns[field.FieldName];
                        dt.Columns.Add(dc.ColumnName, dc.DataType, dc.Expression);
                    }
                    else
                    {
                        DataColumn dc = sourceTable.ParentRelations[field.RelationName].ParentTable.Columns[field.FieldName];
                        dt.Columns.Add(dc.ColumnName, dc.DataType, dc.Expression);
                    }
                }

                if (ds != null)
                {
                    ds.Tables.Add(dt);
                }
                return dt;
            }
        }

        private static void InsertJoinInto(DataTable destTable, DataTable sourceTable,
                                    string fieldList, string rowFilter, string sort)
        {
            if (fieldList == null)
            {
                return;
            }
            else
            {
                ParseFieldList(fieldList, true);
                DataRow[] Rows = sourceTable.Select(rowFilter, sort);

                foreach (DataRow SourceRow in Rows)
                {
                    DataRow DestRow = destTable.NewRow();

                    foreach (FieldInfo Field in m_FieldInfo)
                    {
                        if (Field.RelationName == null)
                        {
                            DestRow[Field.FieldName] = SourceRow[Field.FieldName];
                        }
                        else
                        {
                            DataRow ParentRow = SourceRow.GetParentRow(Field.RelationName);
                            DestRow[Field.FieldName] = ParentRow[Field.FieldName];
                        }
                    }

                    destTable.Rows.Add(DestRow);
                }
            }
        }

        #endregion

        #region SelectDistinct / Distinct

        /**/
        /**/
        /**/
        /// <summary> 
        /// 按照fieldName从sourceTable中选择出不重复的行， 
        /// 相当于select distinct fieldName from sourceTable 
        /// </summary> 
        /// <param name="tableName">表名</param> 
        /// <param name="sourceTable">源DataTable</param> 
        /// <param name="fieldName">列名</param> 
        /// <returns>一个新的不含重复行的DataTable，列只包括fieldName指明的列</returns> 
        public static DataTable SelectDistinct(string tableName, DataTable sourceTable, string fieldName)
        {
            sourceTable.AcceptChanges();
            DataTable dt = new DataTable(tableName);
            dt.Columns.Add(fieldName, sourceTable.Columns[fieldName].DataType);

            object lastValue = null;

            foreach (DataRow dr in sourceTable.Select("", fieldName))
            {
                if (lastValue == null || !(ColumnEqual(lastValue, dr[fieldName])))
                {
                    lastValue = dr[fieldName];
                    dt.Rows.Add(new object[] { lastValue });
                }
            }

            if (ds != null && !ds.Tables.Contains(tableName))
            {
                ds.Tables.Add(dt);
            }
            return dt;
        }

        /**/
        /**/
        /**/
        /// <summary> 
        /// 按照fieldName从sourceTable中选择出不重复的行， 
        /// 相当于select distinct fieldName1,fieldName2,,fieldNamen from sourceTable 
        /// </summary> 
        /// <param name="tableName">表名</param> 
        /// <param name="sourceTable">源DataTable</param> 
        /// <param name="fieldNames">列名数组</param> 
        /// <returns>一个新的不含重复行的DataTable，列只包括fieldNames中指明的列</returns> 
        public static DataTable SelectDistinct(string tableName, DataTable sourceTable, string[] fieldNames)
        {
            sourceTable.AcceptChanges();
            DataTable dt = new DataTable(tableName);
            object[] values = new object[fieldNames.Length];
            string fields = "";

            for (int i = 0; i < fieldNames.Length; i++)
            {
                dt.Columns.Add(fieldNames[i], sourceTable.Columns[fieldNames[i]].DataType);
                fields += fieldNames[i] + ",";
            }

            fields = fields.Remove(fields.Length - 1, 1);
            DataRow lastRow = null;

            foreach (DataRow dr in sourceTable.Select("", fields))
            {
                if (lastRow == null || !(RowEqual(lastRow, dr, dt.Columns)))
                {
                    lastRow = dr;
                    for (int i = 0; i < fieldNames.Length; i++)
                    {
                        values[i] = dr[fieldNames[i]];
                    }
                    dt.Rows.Add(values);
                }
            }

            if (ds != null && !ds.Tables.Contains(tableName))
            {
                ds.Tables.Add(dt);
            }
            return dt;
        }

        /**/
        /**/
        /**/
        /// <summary> 
        /// 按照fieldName从sourceTable中选择出不重复的行， 
        /// 并且包含sourceTable中所有的列。 
        /// </summary> 
        /// <param name="tableName">表名</param> 
        /// <param name="sourceTable">源表</param> 
        /// <param name="fieldName">字段</param> 
        /// <returns>一个新的不含重复行的DataTable</returns> 
        public static DataTable Distinct(string tableName, DataTable sourceTable, string fieldName)
        {
            sourceTable.AcceptChanges();
            DataTable dt = sourceTable.Clone();
            dt.TableName = tableName;

            object lastValue = null;

            foreach (DataRow dr in sourceTable.Select("", fieldName))
            {
                if (lastValue == null || !(ColumnEqual(lastValue, dr[fieldName])))
                {
                    lastValue = dr[fieldName];
                    dt.Rows.Add(dr.ItemArray);
                }
            }

            if (ds != null && !ds.Tables.Contains(tableName))
            {
                ds.Tables.Add(dt);
            }

            return dt;
        }

        /**/
        /**/
        /**/
        /// <summary> 
        /// 按照fieldNames从sourceTable中选择出不重复的行， 
        /// 并且包含sourceTable中所有的列。 
        /// </summary> 
        /// <param name="tableName">表名</param> 
        /// <param name="sourceTable">源表</param> 
        /// <param name="fieldNames">字段</param> 
        /// <returns>一个新的不含重复行的DataTable</returns> 
        public static DataTable Distinct(string tableName, DataTable sourceTable, string[] fieldNames)
        {
            sourceTable.AcceptChanges();
            DataTable dt = sourceTable.Clone();
            dt.TableName = tableName;
            string fields = "";

            for (int i = 0; i < fieldNames.Length; i++)
            {
                fields += fieldNames[i] + ",";
            }

            fields = fields.Remove(fields.Length - 1, 1);
            DataRow lastRow = null;

            foreach (DataRow dr in sourceTable.Select("", fields))
            {
                if (lastRow == null || !(RowEqual(lastRow, dr, dt.Columns)))
                {
                    lastRow = dr;
                    dt.Rows.Add(dr.ItemArray);
                }
            }

            if (ds != null && !ds.Tables.Contains(tableName))
            {
                ds.Tables.Add(dt);
            }
            return dt;
        }

        #endregion

        #region Select Table Into

        /**/
        /**/
        /**/
        /// <summary> 
        /// 按sort排序，按rowFilter过滤sourceTable， 
        /// 复制fieldList中指明的字段的数据到新DataTable，并返回之 
        /// </summary> 
        /// <param name="tableName">表名</param> 
        /// <param name="sourceTable">源表</param> 
        /// <param name="fieldList">字段列表</param> 
        /// <param name="rowFilter">过滤条件</param> 
        /// <param name="sort">排序</param> 
        /// <returns>新DataTable</returns> 
        public static DataTable SelectInto(string tableName, DataTable sourceTable,
                                    string fieldList, string rowFilter, string sort)
        {
            sourceTable.AcceptChanges();
            DataTable dt = CreateTable(tableName, sourceTable, fieldList);

            InsertInto(dt, sourceTable, fieldList, rowFilter, sort);

            return dt;
        }

        #endregion

        #region Group By Table

        /// <summary>
        /// 对一张表进行集合
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="sourceTable">数据源表</param>
        /// <param name="fieldList">字段的字符串集合</param>
        /// <param name="rowFilter">表达式</param>
        /// <param name="groupBy">集合的方式</param>
        /// <returns>返回一个TABLE</returns>
        public static DataTable SelectGroupByInto(string tableName, DataTable sourceTable, string fieldList,
                                           string rowFilter, string groupBy)
        {
            sourceTable.AcceptChanges();
            DataTable dt = CreateGroupByTable(tableName, sourceTable, fieldList);

            InsertGroupByInto(dt, sourceTable, fieldList, rowFilter, groupBy);

            return dt;
        }

        #endregion

        #region Join Tables

        /// <summary>
        /// 关联表
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="sourceTable">数据源的表</param>
        /// <param name="fieldList">字段的字符串集合</param>
        /// <param name="rowFilter">关联的表达式</param>
        /// <param name="sort">排序</param>
        /// <returns>返回一个Table</returns>
        public static DataTable SelectJoinInto(string tableName, DataTable sourceTable, string fieldList, string rowFilter, string sort)
        {
            sourceTable.AcceptChanges();
            DataTable dt = CreateJoinTable(tableName, sourceTable, fieldList);

            InsertJoinInto(dt, sourceTable, fieldList, rowFilter, sort);

            return dt;
        }

        #endregion

        #region Create Table

        /// <summary>
        /// 创建一个Table
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fieldList">字段的字符串</param>
        /// <returns>返回一个TABLE</returns>
        public static DataTable CreateTable(string tableName, string fieldList)
        {
            DataTable dt = new DataTable(tableName);
            DataColumn dc;

            string[] Fields = fieldList.Split(',');
            string[] FieldsParts;
            string Expression;

            foreach (string Field in Fields)
            {
                FieldsParts = Field.Trim().Split(" ".ToCharArray(), 3); // allow for spaces in the expression 
                // add fieldname and datatype 
                if (FieldsParts.Length == 2)
                {
                    dc = dt.Columns.Add(FieldsParts[0].Trim(), Type.GetType("System." + FieldsParts[1].Trim(), true, true));
                    dc.AllowDBNull = true;
                }
                else if (FieldsParts.Length == 3) // add fieldname, datatype, and expression 
                {
                    Expression = FieldsParts[2].Trim();

                    if (Expression.ToUpper() == "REQUIRED")
                    {
                        dc = dt.Columns.Add(FieldsParts[0].Trim(), Type.GetType("System." + FieldsParts[1].Trim(), true, true));
                        dc.AllowDBNull = false;
                    }
                    else
                    {
                        dc = dt.Columns.Add(FieldsParts[0].Trim(), Type.GetType("System." + FieldsParts[1].Trim(), true, true), Expression);
                    }
                }
                else
                {
                    return null;
                }
            }

            if (ds != null)
            {
                ds.Tables.Add(dt);
            }
            return dt;
        }

        /// <summary>
        /// 创建一个Table
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fieldList">字段的字符串</param>
        /// <param name="keyFieldList">关键字段的字符串</param>
        /// <returns>返回一个TABLE</returns>
        public static DataTable CreateTable(string tableName, string fieldList, string keyFieldList)
        {
            DataTable dt = CreateTable(tableName, fieldList);
            string[] KeyFields = keyFieldList.Split(',');

            if (KeyFields.Length > 0)
            {
                DataColumn[] KeyFieldColumns = new DataColumn[KeyFields.Length];
                int i;

                for (i = 1; i == KeyFields.Length - 1; ++i)
                {
                    KeyFieldColumns[i] = dt.Columns[KeyFields[i].Trim()];
                }

                dt.PrimaryKey = KeyFieldColumns;
            }
            return dt;
        }

        #endregion

        #region Match Table

        /// <summary>
        /// 匹配Table,空由字符串'0'表示,返回表字段为【L+关键字段名】，【R+关键字段名】;
        /// </summary>
        /// <param name="leftTable1">左表</param>
        /// <param name="rightTable1">右表</param>
        /// <param name="keyColumnsName">关键字段名</param>
        /// <param name="matchColumnsName">匹配字段名</param>
        /// <returns>返回Table,空由"0"表示</returns>
        public static DataTable MatchTable(DataTable leftTable1, DataTable rightTable1, string keyColumnsName, string matchColumnsName)
        {
            leftTable1.AcceptChanges();
            rightTable1.AcceptChanges();
            DataTable leftTable = leftTable1.Clone();

            leftTable.Columns[keyColumnsName].DataType = typeof(string);
            leftTable.Columns[matchColumnsName].DataType = typeof(string);

            foreach (DataRow dr in leftTable1.Rows)
            {
                DataRow drNew = leftTable.NewRow();

                drNew[keyColumnsName] = dr[keyColumnsName].ToString();
                drNew[matchColumnsName] = dr[matchColumnsName].ToString();

                leftTable.Rows.Add(drNew);
            }

            DataTable rightTable = rightTable1.Clone();

            rightTable.Columns[keyColumnsName].DataType = typeof(string);
            rightTable.Columns[matchColumnsName].DataType = typeof(string);

            foreach (DataRow dr in rightTable1.Rows)
            {
                DataRow drNew = rightTable.NewRow();

                drNew[keyColumnsName] = dr[keyColumnsName].ToString();
                drNew[matchColumnsName] = dr[matchColumnsName].ToString();

                rightTable.Rows.Add(drNew);
            }

            var varData = (from a in
                               (
                                   (from a in leftTable.AsEnumerable()
                                    join b in rightTable.AsEnumerable()
                                    on a.Field<string>(keyColumnsName) equals b.Field<string>(keyColumnsName) into os
                                    from b in os.DefaultIfEmpty()
                                    select new
                                    {
                                        LKey = a.Field<string>(keyColumnsName),
                                        LMatch = a.Field<string>(matchColumnsName),
                                        RKey = b == null ? "0" : b.Field<string>(keyColumnsName),
                                        RMatch = b == null ? "0" : b.Field<string>(matchColumnsName)
                                    }).Union(from a in rightTable.AsEnumerable()
                                             join b in leftTable.AsEnumerable()
                                             on a.Field<string>(keyColumnsName) equals b.Field<string>(keyColumnsName) into os
                                             from b in os.DefaultIfEmpty()
                                             select new
                                             {
                                                 LKey = b == null ? "0" : b.Field<string>(keyColumnsName),
                                                 LMatch = b == null ? "0" : b.Field<string>(matchColumnsName),
                                                 RKey = a.Field<string>(keyColumnsName),
                                                 RMatch = a.Field<string>(matchColumnsName)
                                             }))
                           where a.LKey != a.RKey || a.LMatch != a.RMatch
                           orderby a.RMatch
                           select new
                           {
                               LKey = a.LKey,
                               RKey = a.RKey
                           }).Distinct();

            DataTable infoTable = new DataTable();

            infoTable.Columns.Add("L" + keyColumnsName);
            infoTable.Columns.Add("R" + keyColumnsName);

            foreach (var item in varData)
            {
                DataRow dr = infoTable.NewRow();

                dr["L" + keyColumnsName] = item.LKey;
                dr["R" + keyColumnsName] = item.RKey;

                infoTable.Rows.Add(dr);
            }

            return infoTable;
        }

        #endregion
    }
}
