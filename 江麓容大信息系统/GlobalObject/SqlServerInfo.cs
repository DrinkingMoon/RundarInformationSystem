using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Sql;
using System.Data;
using System.Data.SqlClient;

namespace GlobalObject
{
    /// <summary>
    /// 数据库常用功能集成类
    /// </summary>
    public sealed class DatabaseServer
    {
        /// <summary>
        ///  执行存储过程返回得到的数据库表
        /// </summary>
        /// <param name="sp_cmd">存储过程名称</param>
        /// <param name="sp_Input_Params">哈希数据集</param>
        /// <returns>成功返回查询到的数据表, 失败返回null</returns>
        static public DataTable QueryInfoPro(string sp_cmd,System.Collections.Hashtable sp_Input_Params,out string error)
        {
            error = null;

            try
            {
                DataTable tempTable = new DataTable();

                using (SqlConnection sqlconn = new SqlConnection(GlobalObject.GlobalParameter.StorehouseConnectionString))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(sp_cmd, sqlconn))
                    {
                        DataSet ds = new DataSet();

                        da.SelectCommand = new SqlCommand();

                        da.SelectCommand.CommandType = CommandType.StoredProcedure;
                        da.SelectCommand.CommandText = sp_cmd;
                        da.SelectCommand.Connection = sqlconn;
                        da.SelectCommand.CommandTimeout = 40000;

                        if (sp_Input_Params != null)
                        {
                            System.Collections.IDictionaryEnumerator InputEnume = sp_Input_Params.GetEnumerator();

                            while (InputEnume.MoveNext())
                            {
                                SqlParameter param = new SqlParameter();

                                param.Direction = ParameterDirection.Input;
                                param.ParameterName = InputEnume.Key.ToString();
                                param.Value = InputEnume.Value;

                                da.SelectCommand.Parameters.Add(param);
                            }
                        }

                        int rowcount = da.Fill(ds);

                        if (rowcount == 0 && ds != null && ds.Tables.Count == 0)
                        {
                            error = "没有找到任何数据";
                        }
                        else
                        {
                            tempTable = ds.Tables[ds.Tables.Count - 1];
                        }
                    }
                }

                return tempTable;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 执行SQL查询语句返回得到的数据库表
        /// </summary>
        /// <param name="strSql">SQL查询语句</param>
        /// <returns>成功返回查询到的数据表, 失败返回null</returns>
        static public DataTable QueryInfo(string strSql)
        {
            try
            {
                using (SqlConnection sqlconn = new SqlConnection(GlobalObject.GlobalParameter.StorehouseConnectionString))
                {
                    SqlDataAdapter sqlda = new SqlDataAdapter(strSql, sqlconn.ConnectionString);

                    DataTable dt = new DataTable();
                    sqlda.Fill(dt);

                    return dt;
                }
            }
            catch (Exception exce)
            {
                Console.WriteLine(exce.Message);
                return null;
            }
        }

        /// <summary>
        /// 执行SQL查询语句返回得到的数据库表
        /// </summary>
        /// <param name="strSql">SQL查询语句</param>
        /// <returns>成功返回查询到的数据表, 失败返回null</returns>
        static public DataTable WebQueryInfo(string strSql)
        {
            if (strSql == null || strSql.Trim().Length < 6 || strSql.Trim().Substring(0, 6).ToLower() != "select")
            {
                return null;
            }

            try
            {
                using (SqlConnection sqlconn = new SqlConnection(GlobalObject.GlobalParameter.WebServerConnectionString))
                {
                    DataTable dt = new DataTable();

                    SqlDataAdapter sqlda = new SqlDataAdapter(strSql, sqlconn.ConnectionString);
                    sqlda.Fill(dt);

                    return dt;
                }
            }
            catch (Exception exce)
            {
                Console.WriteLine(exce.Message);
                return null;
            }
        }

        /// <summary>
        /// 获取数据库服务器信息
        /// </summary>
        /// <returns>返回获取到的信息</returns>
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

        /// <summary>
        /// 测试连接数据库
        /// </summary>
        static public void IsConnection()
        {
            using (SqlConnection sqlConn = new SqlConnection(GlobalObject.GlobalParameter.StorehouseConnectionString))
            {
                try
                {
                    sqlConn.Open();
                }
                catch (Exception)
                {
                    throw new Exception("连接数据库失败，连接字符串：" + GlobalObject.GlobalParameter.StorehouseConnectionString);
                }
            }
        }
    } 
}
