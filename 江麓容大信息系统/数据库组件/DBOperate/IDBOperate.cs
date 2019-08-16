/****************************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 * 
 * 文件名称:   IDBOperate.cs
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
using System.Collections;
using System.Data;

namespace DBOperate
{
    /// <summary>
    /// 操作接口
    /// </summary>
    public interface IDBOperate
    {
        /// <summary>
        /// 设置连接字符串
        /// </summary>
        /// <returns>验证结果（验证连接是否合法）</returns>
        bool SetConnectionCMD(string connCMD);

        /// <summary>
        /// 运行SQL语句
        /// </summary>
        /// <param name="sqlCMD">需要执行的sql语句</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功</returns>
        Dictionary<OperateCMD, object> RunProc(string sqlCMD);

        /// <summary>
        /// 运行SQL语句(返回查询结果)
        /// </summary>
        /// <param name="sqlCMD">SQL语句</param>
        /// <param name="ds">DataSet对象</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功，执行结果的数据集</returns>
        Dictionary<OperateCMD, object> RunProc(string sqlCMD, DataSet ds);

        /// <summary>
        /// 运行SQL语句
        /// </summary>
        /// <param name="sqlCMD">SQL语句</param>
        /// <param name="ds">DataSet对象</param>
        /// <param name="tablename">表名</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功，执行结果的数据集</returns>
        Dictionary<OperateCMD, object> RunProc(string sqlCMD, DataSet ds, string tablename);

        /// <summary>
        /// 运行存储过程（用于查询）
        /// </summary>
        /// <param name="sp_cmd">存储过程名称</param>
        /// <param name="ds">DataSet对象</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功，执行结果的数据集</returns>
        Dictionary<OperateCMD, object> RunProc_CMD(string sp_cmd, DataSet ds);

        /// <summary>
        /// 运行存储过程（用于添加、修改等不需要返回Table）
        /// </summary>
        /// <param name="sp_cmd">存储过程名称</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功，执行结果的数据集</returns>
        Dictionary<OperateCMD, object> RunProc_CMD(string sp_cmd);

        /// <summary>
        /// 运行存储过程（用于添加、修改等不需要返回Table）
        /// </summary>
        /// <param name="sp_cmd">存储过程名称</param>
        /// <param name="sp_ht_in">哈希表，对应存储过程传入参数,keys为参数标记，values为参数值</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功，执行结果的数据集</returns>
        Dictionary<OperateCMD, object> RunProc_CMD(string sp_cmd, Hashtable sp_ht_in);

        /// <summary>
        /// 运行存储过程(默认超时时间1小时)
        /// </summary>
        /// <param name="sp_cmd">存储过程名称</param>
        /// <param name="ds">DataSet对象</param>
        /// <param name="sp_ht_in">哈希表，对应存储过程传入参数,keys为参数标记，values为参数值</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功，执行结果的数据集</returns>
        Dictionary<OperateCMD, object> RunProc_CMD(string sp_cmd, DataSet ds, Hashtable sp_ht_in);
                
        /// <summary>
        /// 运行存储过程
        /// </summary>
        /// <param name="sp_cmd">存储过程名称</param>
        /// <param name="ds">DataSet对象</param>
        /// <param name="sp_Input_Params">哈希表，对应存储过程传入参数,keys为参数标记，values为参数值</param>
        /// <param name="Timeout">执行超时时间，单位：秒</param>
        Dictionary<OperateCMD, object> RunProc_CMD(string sp_cmd, DataSet ds, Hashtable sp_Input_Params, int timeout);

        /// <summary>
        /// 运行存储过程（用于查询）
        /// </summary>
        /// <param name="sp_cmd">存储过程名称</param>
        /// <param name="ds">DataSet对象</param>
        /// <param name="sp_ht_in">哈希表，对应存储过程传入参数</param>
        /// <param name="sp_ht_out">哈希表，对应存储过程传出参数,传址</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功，执行结果的数据集</returns>
        Dictionary<OperateCMD, object> RunProc_CMD(string sp_cmd, DataSet ds, Hashtable sp_ht_in, ref Hashtable sp_ht_out);

        /// <summary>
        /// 运行存储过程（用于添加、修改等不需要返回Table）
        /// </summary>
        /// <param name="sp_cmd">存储过程名称</param>
        /// <param name="sp_ht_in">哈希表，对应存储过程传入参数</param>
        /// <param name="sp_ht_out">哈希表，对应存储过程传出参数,传址</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功，执行结果的数据集</returns>
        Dictionary<OperateCMD, object> RunProc_CMD(string sp_cmd, Hashtable sp_ht_in, ref Hashtable sp_ht_out);

        /// <summary>
        /// 事务操作(存储过程)
        /// </summary>
        /// <param name="sp_cmd">存储过程名称</param>
        /// <param name="sp_ht_in">哈希表，对应存储过程传入参数</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功，执行结果的数据集</returns>
        Dictionary<OperateCMD, object> Transaction_CMD(string sp_cmd, Hashtable sp_ht_in);

        /// <summary>
        /// 事务操作(存储过程)
        /// </summary>
        /// <param name="sp_cmd">存储过程名称</param>
        /// <param name="sp_ht_in">哈希表，对应存储过程传入参数</param>
        /// <param name="sp_ht_out">哈希表，对应存储过程传出参数,传址</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功，执行结果的数据集</returns>
        Dictionary<OperateCMD, object> Transaction_CMD(string sp_cmd, Hashtable sp_ht_in, ref Hashtable sp_ht_out);

        /// <summary>
        /// 事务操作(存储过程)
        /// </summary>
        /// <param name="sp_cmds">存储过程名称</param>
        /// <param name="sp_ht_in">哈希表，对应存储过程传入参数</param>
        /// <param name="sp_ht_out">哈希表，对应存储过程传出参数,传址</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功，执行结果的数据集</returns>
        Dictionary<OperateCMD, object> Transaction_CMD(string[] sp_cmds, Hashtable[] sp_ht_in, ref Hashtable[] sp_ht_out);

        /// <summary>
        /// 事务操作(SQL语句)
        /// </summary>
        /// <param name="sqlCMD">SQL语句</param>
        /// <returns>返回结果集合对应的哈希表,包括执行是否成功，执行结果的数据集</returns>
        Dictionary<OperateCMD, object> Transaction_SQL(string[] sql);
    }

    /// <summary>
    /// 操作类型
    /// </summary>
    public enum OperateCMD
    {
        /// <summary>
        /// 返回数据集
        /// </summary>
        Return_DS,

        /// <summary>
        /// 数据库操作的成功与否
        /// </summary>
        Return_OperateResult,

        /// <summary>
        /// 错误捕获消息
        /// </summary>
        Return_Errmsg,

        /// <summary>
        /// 特殊返回值
        /// </summary>
        Return_Other
    }
}
