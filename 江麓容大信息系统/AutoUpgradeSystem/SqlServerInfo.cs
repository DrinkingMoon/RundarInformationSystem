using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Sql;
using System.Data;

namespace AutoUpgradeSystem
{
    /// <summary>
    /// SQL 服务器信息
    /// </summary>
    public sealed class SqlServerInfo
    {
        static public DataTable GetSqlServer()
        {
            SqlDataSourceEnumerator sqlEnumerator = SqlDataSourceEnumerator.Instance;
            DataTable table = sqlEnumerator.GetDataSources();

            string[] colNames = { "服务器名", "实例名", "是否集成验证", "数据库版本" };

            for (int i = 0; i < table.Columns.Count; i++)
            {
                table.Columns[i].ColumnName = colNames[i];
            }

            return table;
        }
    } 
}
