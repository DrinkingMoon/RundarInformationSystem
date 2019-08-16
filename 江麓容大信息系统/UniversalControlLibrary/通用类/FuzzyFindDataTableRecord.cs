using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GlobalObject;
using System.Windows.Forms;

namespace UniversalControlLibrary
{
    /// <summary>
    /// 用模糊查询方式查找DataTable中的指定记录
    /// </summary>
    public static class FuzzyFindDataTableRecord
    {
        /// <summary>
        /// 从数据源中用模糊查询方式查找记录
        /// </summary>
        /// <param name="sourceTable">源数据表</param>
        /// <param name="columnName">要查找的列名称</param>
        /// <param name="findData">要查找的数据</param>
        /// <returns>返回查找到的记录集</returns>
        static public DataTable FindRecord(DataTable sourceTable, string columnName, string findData)
        {
            if (sourceTable == null)
            {
                return null;
            }

            DataTable table = sourceTable.Copy();
            int colIndex = table.Columns.IndexOf(columnName);

            if (colIndex == -1)
            {
                return table;
            }

            findData = findData.ToUpper();

            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (!table.Rows[i].ItemArray[colIndex].ToString().ToUpper().Contains(findData))
                {
                    table.Rows.RemoveAt(i);
                    i--;
                }
            }

            return table;
        }

        /// <summary>
        /// 从数据源中用模糊查询方式查找记录
        /// </summary>
        /// <param name="sourceTable">源数据</param>
        /// <param name="columnName">要查找的列名称</param>
        /// <param name="findData">要查找的数据</param>
        /// <returns>返回查找到的记录集</returns>
        static public DataTable FindRecord<T>(IQueryable<T> sourceTable, string columnName, string findData)
        {
            if (sourceTable == null)
            {
                return null;
            }

            DataTable table = GeneralFunction.ConvertToDataTable<T>(sourceTable);
            return FindRecord(table, columnName, findData);
        }
    }
}
