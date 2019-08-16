/****************************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 * 
 * 文件名称:   DBOperate.cs
 * 
 * 作者    :   Dennis
 * 
 * 版本:       V1.0.703.3
 * 
 * 创建日期:   2009-05-26
 * 
 * 开发平台:   vs2005(c#)
 ****************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Collections;

namespace DBOperate
{
    /// <summary>
    /// SQL数据库操作类
    /// </summary>
    class SQLOperate : IDBOperate
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        internal SQLOperate()
        {
            InitResult();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connCMD">连接字符串</param>
        internal SQLOperate(string connCMD)
        {
            SetConnectionCMD(connCMD);

            InitResult();
        }

        /// <summary>
        /// 连接字符串
        /// </summary>
        private string m_ConnStr = null;

        /// <summary>
        /// 数据库连接
        /// </summary>
        private SqlConnection m_Conn = null;

        /// <summary>
        /// 用于返回值的集合
        /// </summary>
        private Dictionary<OperateCMD, object> m_Result;

        /// <summary>
        /// 初始化返回集合
        /// </summary>
        void InitResult()
        {
            m_Result = new Dictionary<OperateCMD, object>();

            m_Result.Add(OperateCMD.Return_DS, null);

            m_Result.Add(OperateCMD.Return_OperateResult, null);

            m_Result.Add(OperateCMD.Return_Errmsg, null);

            m_Result.Add(OperateCMD.Return_Other, null);
        }

        /// <summary>
        /// 初始化返回集合
        /// </summary>
        void SetResultValue(object ds, object bOperate, object err, object other)
        {
            m_Result[OperateCMD.Return_DS] = ds;

            m_Result[OperateCMD.Return_OperateResult] = bOperate;

            m_Result[OperateCMD.Return_Errmsg] = err;

            m_Result[OperateCMD.Return_Other] = other;
        }

        #region IDBOperate 成员
        /// <summary>
        /// 设置连接字符串
        /// </summary>
        /// <param name="connCMD">连接字符串</param>
        /// <returns>验证结果（验证连接是否合法）</returns>
        public bool SetConnectionCMD(string connCMD)
        {
            using (SqlConnection tmp = new SqlConnection(connCMD))
            {
                try
                {
                    //验证连接字符串是否合法
                    tmp.Open();

                    m_ConnStr = connCMD;

                    m_Conn = new SqlConnection();

                    m_Conn.ConnectionString = m_ConnStr;

                    return true;
                }
                catch (Exception E)
                {
                    Console.WriteLine(E.Message);

                    return false;
                }
            }
        }

        /// <summary>
        /// 事务操作(SQL语句)
        /// </summary>
        /// <param name="sqlCMD">SQL语句</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功，执行结果的数据集</returns>
        public Dictionary<OperateCMD, object> Transaction_SQL(string[] sql)
        {
            if (m_Conn != null)
            {
                SqlTransaction transaction = null;

                try
                {
                    m_Conn.Close();

                    m_Conn.Open();

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        transaction = m_Conn.BeginTransaction();

                        cmd.Connection = m_Conn;

                        cmd.Transaction = transaction;

                        cmd.CommandType = CommandType.Text;

                        if (sql != null)
                        {
                            for (int i = 0; i < sql.Length; i++)
                            {
                                cmd.CommandText = sql[i];

                                int rowcount = cmd.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();

                        SetResultValue(null, true, null, null);

                        return m_Result;
                    }
                }
                catch (Exception err)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }

                    SetResultValue(null, false, err, null);

                    return m_Result;
                }
            }
            else
            {
                SetResultValue(null, false, "空的数据连接", null);

                return m_Result;
            }
        }

        /// <summary>
        /// 事务操作(存储过程)
        /// </summary>
        /// <param name="sp_cmd">存储过程名称</param>
        /// <param name="sp_ht_in">哈希表，对应存储过程传入参数</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功，执行结果的数据集</returns>
        public Dictionary<OperateCMD, object> Transaction_CMD(string sp_cmd, Hashtable sp_ht_in)
        {
            if (m_Conn != null)
            {
                SqlTransaction transaction = null;

                try
                {
                    m_Conn.Close();

                    m_Conn.Open();

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.CommandText = sp_cmd;

                        cmd.Connection = m_Conn;

                        if (sp_ht_in != null)
                        {
                            IDictionaryEnumerator InputEnume = sp_ht_in.GetEnumerator();

                            while (InputEnume.MoveNext())
                            {
                                SqlParameter param = new SqlParameter();

                                param.Direction = ParameterDirection.Input;

                                param.ParameterName = InputEnume.Key.ToString();

                                param.Value = InputEnume.Value;

                                cmd.Parameters.Add(param);
                            }
                        }

                        transaction = m_Conn.BeginTransaction();
                        cmd.Transaction = transaction;

                        object result = cmd.ExecuteScalar();
                        transaction.Commit();

                        SetResultValue(null, true, null, result);

                        return m_Result;
                    }
                }
                catch (Exception err)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }

                    SetResultValue(null, false, err, null);
                    return m_Result;
                }
            }
            else
            {
                SetResultValue(null, false, "空的数据连接", null);
                return m_Result;
            }
        }

        /// <summary>
        /// 事务操作(存储过程)
        /// </summary>
        /// <param name="sp_cmd">存储过程名称</param>
        /// <param name="sp_ht_in">哈希表，对应存储过程传入参数</param>
        /// <param name="sp_ht_out">哈希表，对应存储过程传出参数,传址</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功，执行结果的数据集</returns>
        public Dictionary<OperateCMD, object> Transaction_CMD(string sp_cmd, Hashtable sp_ht_in, ref Hashtable sp_ht_out)
        {
            if (m_Conn != null)
            {
                SqlTransaction transaction = null;

                try
                {
                    m_Conn.Close();

                    m_Conn.Open();

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.CommandText = sp_cmd;

                        cmd.Connection = m_Conn;

                        if (sp_ht_in != null)
                        {
                            IDictionaryEnumerator InputEnume = sp_ht_in.GetEnumerator();

                            while (InputEnume.MoveNext())
                            {
                                SqlParameter param = new SqlParameter();

                                param.Direction = ParameterDirection.Input;

                                param.ParameterName = InputEnume.Key.ToString();

                                param.Value = InputEnume.Value;

                                cmd.Parameters.Add(param);
                            }
                        }


                        if (sp_ht_out != null)
                        {
                            IDictionaryEnumerator OutputEnume = sp_ht_out.GetEnumerator();

                            while (OutputEnume.MoveNext())
                            {
                                SqlParameter param = new SqlParameter();

                                param.Direction = ParameterDirection.Output;

                                param.ParameterName = OutputEnume.Key.ToString();

                                param.Value = OutputEnume.Value;

                                cmd.Parameters.Add(param);
                            }
                        }

                        transaction = m_Conn.BeginTransaction();

                        cmd.Transaction = transaction;

                        object result = cmd.ExecuteScalar();

                        transaction.Commit();

                        if (sp_ht_out != null)
                        {
                            Hashtable P_OutValue = new Hashtable();

                            IDictionaryEnumerator OutValueEnum = sp_ht_out.GetEnumerator();

                            OutValueEnum.Reset();

                            while (OutValueEnum.MoveNext())
                            {
                                P_OutValue.Add(OutValueEnum.Key.ToString(), cmd.Parameters[OutValueEnum.Key.ToString()].Value.ToString());
                            }

                            sp_ht_out = P_OutValue;
                        }

                        SetResultValue(null, true, null, result);

                        return m_Result;
                    }
                }
                catch (Exception err)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }

                    SetResultValue(null, false, err, null);

                    return m_Result;
                }
            }
            else
            {
                SetResultValue(null, false, "空的数据连接", null);

                return m_Result;
            }
        }

        /// <summary>
        /// 运行SQL语句
        /// </summary>
        /// <param name="sqlCMD">需要执行的sql语句</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功</returns>
        public Dictionary<OperateCMD, object> RunProc(string sqlCMD)
        {
            if (m_Conn != null)
            {
                try
                {
                    m_Conn.Close();

                    m_Conn.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlCMD, m_Conn))
                    {
                        int rowcount = cmd.ExecuteNonQuery();

                        SetResultValue(null, true, null, rowcount);

                        return m_Result;
                    }
                }
                catch (Exception err)
                {
                    SetResultValue(null, false, err, null);
                    return m_Result;
                }
            }
            else
            {
                SetResultValue(null, false, "空的数据连接", null);

                return m_Result;
            }
        }

        /// <summary>
        /// 运行SQL语句
        /// </summary>
        /// <param name="sqlCMD">SQL语句</param>
        /// <param name="ds">DataSet对象</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功，执行结果的数据集</returns>
        public Dictionary<OperateCMD, object> RunProc(string sqlCMD, DataSet ds)
        {
            if (m_Conn != null)
            {
                try
                {
                    m_Conn.Close();

                    m_Conn.Open();

                    using (SqlDataAdapter da = new SqlDataAdapter(sqlCMD, m_Conn))
                    {
                        da.SelectCommand.CommandText = sqlCMD;

                        da.SelectCommand.CommandType = CommandType.Text;

                        da.SelectCommand.Connection = m_Conn;

                        int rowcount = da.Fill(ds);

                        if (rowcount == 0)
                        {
                            SetResultValue(null, false, "没有找到任何数据", null);

                            return m_Result;
                        }
                        else
                        {
                            SetResultValue(ds, true, null, rowcount);

                            return m_Result;
                        }
                    }
                }
                catch (Exception err)
                {
                    SetResultValue(null, false, err, null);

                    return m_Result;
                }
            }
            else
            {
                SetResultValue(null, false, "空的数据连接", null);

                return m_Result;
            }
        }

        /// <summary>
        /// 运行SQL语句
        /// </summary>
        /// <param name="sqlCMD">SQL语句</param>
        /// <param name="ds">DataSet对象</param>
        /// <param name="tablename">表名</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功，执行结果的数据集</returns>
        public Dictionary<OperateCMD, object> RunProc(string sqlCMD, System.Data.DataSet ds, string tablename)
        {
            if (m_Conn != null)
            {
                try
                {
                    m_Conn.Close();

                    m_Conn.Open();

                    using (SqlDataAdapter da = new SqlDataAdapter(sqlCMD, m_Conn))
                    {
                        da.SelectCommand.CommandText = sqlCMD;

                        da.SelectCommand.CommandType = CommandType.Text;

                        da.SelectCommand.Connection = m_Conn;

                        int rowcount = da.Fill(ds, tablename);

                        if (rowcount == 0)
                        {
                            SetResultValue(null, false, "没有找到任何数据", null);

                            return m_Result;
                        }
                        else
                        {
                            SetResultValue(ds, true, null, rowcount);

                            return m_Result;
                        }
                    }
                }
                catch (Exception err)
                {
                    SetResultValue(null, false, err, null);

                    return m_Result;
                }
            }
            else
            {
                SetResultValue(null, false, "空的数据连接", null);

                return m_Result;
            }
        }

        /// <summary>
        /// 运行存储过程
        /// </summary>
        /// <param name="sp_cmd">存储过程名称</param>
        /// <param name="ds">DataSet对象</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功，执行结果的数据集</returns>
        public Dictionary<OperateCMD, object> RunProc_CMD(string sp_cmd, System.Data.DataSet ds)
        {
            if (m_Conn != null)
            {
                try
                {
                    m_Conn.Close();

                    m_Conn.Open();

                    using (SqlDataAdapter da = new SqlDataAdapter(sp_cmd, m_Conn))
                    {
                        da.SelectCommand = new SqlCommand();

                        da.SelectCommand.CommandType = CommandType.StoredProcedure;

                        da.SelectCommand.CommandText = sp_cmd;

                        da.SelectCommand.Connection = m_Conn;

                        int rowcount = da.Fill(ds);

                        if (rowcount == 0)
                        {
                            SetResultValue(null, false, "没有找到任何数据", null);

                            return m_Result;
                        }
                        else
                        {
                            SetResultValue(ds, true, null, rowcount);

                            return m_Result;
                        }
                    }
                }
                catch (Exception err)
                {
                    SetResultValue(null, false, err, null);

                    return m_Result;
                }
            }
            else
            {
                SetResultValue(null, false, "空的数据连接", null);

                return m_Result;
            }
        }

        /// <summary>
        /// 运行存储过程(默认超时时间1小时)
        /// </summary>
        /// <param name="sp_cmd">存储过程名称</param>
        /// <param name="ds">DataSet对象</param>
        /// <param name="sp_Input_Params">哈希表，对应存储过程传入参数,keys为参数标记，values为参数值</param>
        public Dictionary<OperateCMD, object> RunProc_CMD(string sp_cmd, DataSet ds, Hashtable sp_Input_Params)
        {
            return RunProc_CMD(sp_cmd, ds, sp_Input_Params, 3600);
        }

        /// <summary>
        /// 运行存储过程
        /// </summary>
        /// <param name="sp_cmd">存储过程名称</param>
        /// <param name="ds">DataSet对象</param>
        /// <param name="sp_Input_Params">哈希表，对应存储过程传入参数,keys为参数标记，values为参数值</param>
        /// <param name="Timeout">执行超时时间，单位：秒</param>
        public Dictionary<OperateCMD, object> RunProc_CMD(string sp_cmd, DataSet ds, Hashtable sp_Input_Params, int timeout)
        {
            if (m_Conn != null)
            {
                try
                {
                    m_Conn.Close();

                    m_Conn.Open();

                    using (SqlDataAdapter da = new SqlDataAdapter(sp_cmd, m_Conn))
                    {
                        da.SelectCommand = new SqlCommand();

                        da.SelectCommand.CommandType = CommandType.StoredProcedure;

                        da.SelectCommand.CommandText = sp_cmd;

                        da.SelectCommand.Connection = m_Conn;

                        da.SelectCommand.CommandTimeout = timeout;

                        IDictionaryEnumerator InputEnume = sp_Input_Params.GetEnumerator();

                        while (InputEnume.MoveNext())
                        {
                            SqlParameter param = new SqlParameter();

                            param.Direction = ParameterDirection.Input;

                            param.ParameterName = InputEnume.Key.ToString();

                            param.Value = InputEnume.Value;

                            da.SelectCommand.Parameters.Add(param);
                        }

                        int rowcount = da.Fill(ds);

                        if (rowcount == 0)
                        {
                            SetResultValue(null, false, "没有找到任何数据", null);

                            return m_Result;
                        }
                        else
                        {
                            SetResultValue(ds, true, null, rowcount);
                            return m_Result;
                        }
                    }
                }
                catch (Exception err)
                {
                    SetResultValue(null, false, err, null);

                    return m_Result;
                }
            }
            else
            {
                SetResultValue(null, false, "空的数据连接", null);

                return m_Result;
            }
        }

        /// <summary>
        /// 运行存储过程
        /// </summary>
        /// <param name="sp_cmd">存储过程名称</param>
        /// <param name="ds">DataSet对象</param>
        /// <param name="sp_Input_Params">哈希表，对应存储过程传入参数</param>
        /// <param name="sp_Output_Params">哈希表，对应存储过程传出参数,传址</param>
        public Dictionary<OperateCMD, object> RunProc_CMD(string sp_cmd, System.Data.DataSet ds, 
            System.Collections.Hashtable sp_Input_Params, ref System.Collections.Hashtable sp_Output_Params)
        {
            if (m_Conn != null)
            {
                try
                {
                    m_Conn.Close();

                    m_Conn.Open();

                    using (SqlDataAdapter da = new SqlDataAdapter(sp_cmd, m_Conn))
                    {
                        da.SelectCommand = new SqlCommand();

                        da.SelectCommand.CommandType = CommandType.StoredProcedure;

                        da.SelectCommand.CommandText = sp_cmd;

                        da.SelectCommand.Connection = m_Conn;

                        if (sp_Input_Params != null)
                        {
                            IDictionaryEnumerator InputEnume = sp_Input_Params.GetEnumerator();

                            while (InputEnume.MoveNext())
                            {
                                SqlParameter param = new SqlParameter();

                                param.Direction = ParameterDirection.Input;

                                param.ParameterName = InputEnume.Key.ToString();

                                param.Value = InputEnume.Value;

                                da.SelectCommand.Parameters.Add(param);
                            }
                        }


                        if (sp_Output_Params != null)
                        {
                            IDictionaryEnumerator OutputEnume = sp_Output_Params.GetEnumerator();

                            while (OutputEnume.MoveNext())
                            {
                                SqlParameter param = new SqlParameter();

                                param.Direction = ParameterDirection.Output;

                                param.ParameterName = OutputEnume.Key.ToString();

                                param.Value = OutputEnume.Value;

                                da.SelectCommand.Parameters.Add(param);
                            }
                        }

                        int rowcount = da.Fill(ds);

                        if (sp_Output_Params != null)
                        {
                            Hashtable P_OutValue = new Hashtable();

                            IDictionaryEnumerator OutValueEnum = sp_Output_Params.GetEnumerator();

                            OutValueEnum.Reset();

                            while (OutValueEnum.MoveNext())
                            {
                                P_OutValue.Add(OutValueEnum.Key.ToString(), 
                                    da.SelectCommand.Parameters[OutValueEnum.Key.ToString()].Value.ToString());
                            }

                            sp_Output_Params = P_OutValue;
                        }

                        if (rowcount == 0)
                        {
                            SetResultValue(null, false, "没有找到任何数据", null);

                            return m_Result;
                        }
                        else
                        {
                            SetResultValue(ds, true, null, rowcount);

                            return m_Result;
                        }
                    }
                }
                catch (Exception err)
                {
                    SetResultValue(null, false, err, null);

                    return m_Result;
                }
            }
            else
            {
                SetResultValue(null, false, "空的数据连接", null);

                return m_Result;
            }
        }

        /// <summary>
        /// 事务操作(存储过程)
        /// </summary>
        /// <param name="sp_cmds">存储过程名称</param>
        /// <param name="sp_ht_in">哈希表，对应存储过程传入参数</param>
        /// <param name="sp_ht_out">哈希表，对应存储过程传出参数,传址</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功，执行结果的数据集</returns>
        public Dictionary<OperateCMD, object> Transaction_CMD(string[] sp_cmds, Hashtable[] sp_ht_in, ref Hashtable[] sp_ht_out)
        {
            if (m_Conn != null)
            {
                SqlTransaction transaction = null;

                try
                {
                    m_Conn.Close();

                    m_Conn.Open();

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Connection = m_Conn;

                        transaction = m_Conn.BeginTransaction();

                        cmd.Transaction = transaction;

                        if (sp_cmds != null)
                        {
                            for (int i = 0; i < sp_cmds.Length; i++)
                            {
                                cmd.CommandText = sp_cmds[i];

                                cmd.Parameters.Clear();

                                if (sp_ht_in[i] != null)
                                {
                                    IDictionaryEnumerator InputEnume = sp_ht_in[i].GetEnumerator();

                                    while (InputEnume.MoveNext())
                                    {
                                        SqlParameter param = new SqlParameter();

                                        param.Direction = ParameterDirection.Input;

                                        param.ParameterName = InputEnume.Key.ToString();

                                        param.Value = InputEnume.Value;

                                        cmd.Parameters.Add(param);
                                    }
                                }

                                if (sp_ht_out[i] != null)
                                {
                                    IDictionaryEnumerator OutputEnume = sp_ht_out[i].GetEnumerator();

                                    while (OutputEnume.MoveNext())
                                    {
                                        SqlParameter param = new SqlParameter();

                                        param.Direction = ParameterDirection.Output;

                                        param.ParameterName = OutputEnume.Key.ToString();

                                        param.Value = OutputEnume.Value;

                                        cmd.Parameters.Add(param);
                                    }
                                }

                                object result = cmd.ExecuteScalar();

                                if (sp_ht_out[i] != null)
                                {
                                    Hashtable P_OutValue = new Hashtable();

                                    IDictionaryEnumerator OutValueEnum = sp_ht_out[i].GetEnumerator();

                                    OutValueEnum.Reset();

                                    while (OutValueEnum.MoveNext())
                                    {
                                        P_OutValue.Add(OutValueEnum.Key.ToString(), 
                                            cmd.Parameters[OutValueEnum.Key.ToString()].Value.ToString());
                                    }

                                    sp_ht_out[i] = P_OutValue;
                                }
                            }

                            transaction.Commit();
                        }

                        SetResultValue(null, true, null, null);

                        return m_Result;
                    }
                }
                catch (Exception err)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }

                    SetResultValue(null, false, err, null);

                    return m_Result;
                }
            }
            else
            {
                SetResultValue(null, false, "空的数据连接", null);

                return m_Result;
            }
        }

        /// <summary>
        /// 运行存储过程（用于添加、修改等不需要返回Table）
        /// </summary>
        /// <param name="sp_cmd">存储过程名称</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功，执行结果的数据集</returns>
        public Dictionary<OperateCMD, object> RunProc_CMD(string sp_cmd)
        {
            if (m_Conn != null)
            {
                try
                {
                    m_Conn.Close();

                    m_Conn.Open();

                    using (SqlDataAdapter da = new SqlDataAdapter(sp_cmd, m_Conn))
                    {
                        da.SelectCommand = new SqlCommand();

                        da.SelectCommand.CommandType = CommandType.StoredProcedure;

                        da.SelectCommand.CommandText = sp_cmd;

                        da.SelectCommand.Connection = m_Conn;

                        object result = da.SelectCommand.ExecuteScalar();

                        SetResultValue(null, true, null, result);

                        return m_Result;

                    }
                }
                catch (Exception err)
                {
                    SetResultValue(null, false, err, null);

                    return m_Result;
                }
            }
            else
            {
                SetResultValue(null, false, "空的数据连接", null);

                return m_Result;
            }
        }

        /// <summary>
        /// 运行存储过程（用于添加、修改等不需要返回Table）
        /// </summary>
        /// <param name="sp_cmd">存储过程名称</param>
        /// <param name="sp_ht_in">哈希表，对应存储过程传入参数,keys为参数标记，values为参数值</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功，执行结果的数据集</returns>
        public Dictionary<OperateCMD, object> RunProc_CMD(string sp_cmd, Hashtable sp_ht_in)
        {
            if (m_Conn != null)
            {
                try
                {
                    m_Conn.Close();

                    m_Conn.Open();

                    using (SqlDataAdapter da = new SqlDataAdapter(sp_cmd, m_Conn))
                    {
                        da.SelectCommand = new SqlCommand();

                        da.SelectCommand.CommandType = CommandType.StoredProcedure;

                        da.SelectCommand.CommandText = sp_cmd;

                        da.SelectCommand.Connection = m_Conn;

                        IDictionaryEnumerator InputEnume = sp_ht_in.GetEnumerator();

                        while (InputEnume.MoveNext())
                        {
                            SqlParameter param = new SqlParameter();

                            param.Direction = ParameterDirection.Input;

                            param.ParameterName = InputEnume.Key.ToString();

                            param.Value = InputEnume.Value;

                            da.SelectCommand.Parameters.Add(param);
                        }

                        object result = da.SelectCommand.ExecuteScalar();

                        SetResultValue(null, true, null, result);

                        return m_Result;
                    }
                }
                catch (Exception err)
                {
                    SetResultValue(null, false, err, null);

                    return m_Result;
                }
            }
            else
            {
                SetResultValue(null, false, "空的数据连接", null);

                return m_Result;
            }
        }

        /// <summary>
        /// 运行存储过程（用于添加、修改等不需要返回Table）
        /// </summary>
        /// <param name="sp_cmd">存储过程名称</param>
        /// <param name="sp_ht_in">哈希表，对应存储过程传入参数</param>
        /// <param name="sp_ht_out">哈希表，对应存储过程传出参数,传址</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功，执行结果的数据集</returns>
        public Dictionary<OperateCMD, object> RunProc_CMD(string sp_cmd, Hashtable sp_ht_in, ref Hashtable sp_ht_out)
        {
            if (m_Conn != null)
            {
                try
                {
                    m_Conn.Close();

                    m_Conn.Open();

                    using (SqlDataAdapter da = new SqlDataAdapter(sp_cmd, m_Conn))
                    {
                        da.SelectCommand = new SqlCommand();

                        da.SelectCommand.CommandType = CommandType.StoredProcedure;

                        da.SelectCommand.CommandText = sp_cmd;

                        da.SelectCommand.Connection = m_Conn;

                        if (sp_ht_in != null)
                        {
                            IDictionaryEnumerator InputEnume = sp_ht_in.GetEnumerator();

                            while (InputEnume.MoveNext())
                            {
                                SqlParameter param = new SqlParameter();

                                param.Direction = ParameterDirection.Input;

                                param.ParameterName = InputEnume.Key.ToString();

                                param.Value = InputEnume.Value;

                                da.SelectCommand.Parameters.Add(param);
                            }
                        }


                        if (sp_ht_out != null)
                        {
                            IDictionaryEnumerator OutputEnume = sp_ht_out.GetEnumerator();

                            while (OutputEnume.MoveNext())
                            {
                                SqlParameter param = new SqlParameter();

                                param.Direction = ParameterDirection.Output;

                                param.ParameterName = OutputEnume.Key.ToString();

                                param.Value = OutputEnume.Value;

                                da.SelectCommand.Parameters.Add(param);
                            }
                        }

                        object result = da.SelectCommand.ExecuteScalar();

                        if (sp_ht_out != null)
                        {
                            Hashtable P_OutValue = new Hashtable();

                            IDictionaryEnumerator OutValueEnum = sp_ht_out.GetEnumerator();

                            OutValueEnum.Reset();

                            while (OutValueEnum.MoveNext())
                            {
                                P_OutValue.Add(OutValueEnum.Key.ToString(),
                                    da.SelectCommand.Parameters[OutValueEnum.Key.ToString()].Value.ToString());
                            }

                            sp_ht_out = P_OutValue;
                        }


                        SetResultValue(null, true, null, result);

                        return m_Result;
                    }
                }
                catch (Exception err)
                {
                    SetResultValue(null, false, err, null);

                    return m_Result;
                }
            }
            else
            {
                SetResultValue(null, false, "空的数据连接", null);

                return m_Result;
            }
        }

        #endregion
    }
}
