/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  ConnectionBoxOfDbObject.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2009/06/15
 * 开发平台:  vs2005(c#)
 * 用于    :  生产线管理信息系统
 *----------------------------------------------------------------------------
 * 描述 : 数据库对象连接盒, 用于初始化数据库组件的连接参数
 * 其它 :
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2009/06/15 16:02:08 作者: 夏石友 当前版本: V1.00
 *        修改说明: 创建
 ******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using DBOperate;
using System.Collections;
using System.Data;
using System.Diagnostics;

namespace DBOperate
{
    /// <summary>
    /// 数据库对象连接盒
    /// </summary>
    public static class AccessDB
    {
        #region variants

        /// <summary>
        /// 数据库名称
        /// </summary>
        static string m_databaseName = "DepotManagement";

        /// <summary>
        /// 数据库操作接口
        /// </summary>
        static IDBOperate m_dboperate;

        /// <summary>
        /// 数据库空值
        /// </summary>
        static DBNull m_dbNull = DBNull.Value;

        #endregion

        #region properties

        /// <summary>
        /// 数据库名称
        /// </summary>
        public static string DatabaseName
        {
            get { return m_databaseName; }
            set { m_databaseName = value; }
        }

        #endregion

        /// <summary>
        /// 设置当前使用的数据库操作接口
        /// </summary>
        /// <param name="dbName">数据库名称</param>
        /// <returns>成功则返回true，失败返回false</returns>
        public static bool SetDBOperate(string dbName)
        {
            m_dboperate = DBFactory.Init(DBFactory.DBType.SQL, GetDBConnString(dbName), dbName);
            return m_dboperate == null;
        }

        /// <summary>
        /// 获取由属性指定的数据库的操作接口
        /// </summary>
        /// <returns>成功则返回获取到的操作操作，失败返回null</returns>
        public static IDBOperate GetIDBOperate()
        {
            m_dboperate = DBFactory.Init(DBFactory.DBType.SQL, GetDBConnString(m_databaseName), DatabaseName);
            return m_dboperate;
        }

        /// <summary>
        /// 获取数据库操作接口
        /// </summary>
        /// <param name="dbName">数据库名称</param>
        /// <returns>成功则返回获取到的操作操作，失败返回null</returns>
        public static IDBOperate GetIDBOperate(string dbName)
        {
            IDBOperate dboperate = DBFactory.Init(DBFactory.DBType.SQL, GetDBConnString(dbName), dbName);
            return dboperate;
        }

        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        /// <param name="dbName">数据库名称</param>
        /// <returns>获取到的数据库连接字符串</returns>
        static string GetDBConnString(string dbName)
        {
            if (dbName == "DepotManagement")
                return GlobalObject.GlobalParameter.StorehouseConnectionString;
            else if (dbName == "PlatformService")
                return GlobalObject.GlobalParameter.PlatformServiceConnectionString;
            else
                throw new Exception("未知的数据库名称：" + dbName);
        }

        /// <summary>
        /// 设置存储过程参数
        /// </summary>
        /// <param name="paramNames">参数名数组</param>
        /// <param name="paramValues">参数值数组</param>
        /// <returns>返回参数名称与参数值匹配好的哈希表</returns>
        public static Hashtable SetSPParam(string[] paramNames, object[] paramValues)
        {
            Debug.Assert(paramNames != null && paramNames.Length == paramValues.Length);

            Hashtable htParams = new Hashtable();

            for (int i = 0; i < paramNames.Length; i++)
            {
                if (paramValues[i] == null)
                {
                    htParams.Add(paramNames[i], m_dbNull);
                }
                else
                {
                    htParams.Add(paramNames[i], paramValues[i]);
                }
            }

            return htParams;
        }

        /// <summary>
        /// 设置存储过程参数
        /// </summary>
        /// <param name="lstParamName">参数名列表</param>
        /// <param name="lstParamValue">参数值列表</param>
        /// <returns>返回参数名称与参数值匹配好的哈希表</returns>
        public static Hashtable SetSPParam(List<string> lstParamName, List<object> lstParamValue)
        {
            Debug.Assert(lstParamName != null && lstParamName.Count == lstParamValue.Count);

            Hashtable htParams = new Hashtable();

            for (int i = 0; i < lstParamName.Count; i++)
            {
                if (lstParamValue[i] == null)
                {
                    htParams.Add(lstParamName[i], m_dbNull);
                }
                else
                {
                    htParams.Add(lstParamName[i], lstParamValue[i]);
                }
            }

            return htParams;
        }

        /// <summary>
        /// 设置存储过程参数
        /// </summary>
        /// <param name="dicParam">参数字典</param>
        /// <returns>返回参数名称与参数值匹配好的哈希表</returns>
        public static Hashtable SetSPParam(Dictionary<string, object> dicParam)
        {
            Debug.Assert(dicParam != null && dicParam.Count > 0);

            Hashtable htParams = new Hashtable();

            foreach (KeyValuePair<string, object> param in dicParam)
            {
                if (param.Value == null)
                {
                    htParams.Add(param.Key, m_dbNull);
                }
                else
                {
                    htParams.Add(param.Key, param.Value);
                }
            }

            return htParams;
        }

        /// <summary>
        /// 执行带参数的查询存储过程
        /// </summary>
        /// <param name="procedureName">存储过程名称</param>
        /// <param name="paramTable">参数表</param>
        /// <param name="ds">返回的数据集</param>
        /// <param name="err">错误信息, 没有错误该值为null</param>
        /// <returns>成功返回true</returns>
        public static bool ExecuteDbProcedure(string procedureName, Hashtable paramTable, DataSet ds, out string err)
        {
            err = null;

            if (m_dboperate == null)
            {
                GetIDBOperate();
            }

            Dictionary<OperateCMD, object> dicOperateCMD = m_dboperate.RunProc_CMD(procedureName, ds, paramTable);

            return GetResult(dicOperateCMD, out err);
        }

        /// <summary>
        /// 执行带参数的编辑类存储过程
        /// </summary>
        /// <param name="procedureName">存储过程名称</param>
        /// <param name="paramTable">参数表</param>
        /// <param name="err">错误信息, 没有错误该值为null</param>
        /// <returns>成功返回true</returns>
        public static bool ExecuteDbProcedure(string procedureName, Hashtable paramTable, out string err)
        {
            err = null;

            if (m_dboperate == null)
            {
                GetIDBOperate();
            }

            Dictionary<OperateCMD, object> dicOperateCMD = m_dboperate.RunProc_CMD(procedureName, paramTable);

            return GetResult(dicOperateCMD, out err);
        }

        /// <summary>
        /// 执行不带参数的编辑类存储过程
        /// </summary>
        /// <param name="procedureName">存储过程名称</param>
        /// <param name="err">错误信息, 没有错误该值为null</param>
        /// <returns>成功返回true</returns>
        public static bool ExecuteDbProcedure(string procedureName, out string err)
        {
            err = null;

            if (m_dboperate == null)
            {
                GetIDBOperate();
            }

            Dictionary<OperateCMD, object> dicOperateCMD = m_dboperate.RunProc_CMD(procedureName);

            return GetResult(dicOperateCMD, out err);
        }

        /// <summary>
        /// 执行不带参数的查询存储过程
        /// </summary>
        /// <param name="procedureName">存储过程名称</param>
        /// <param name="ds">返回的数据集</param>
        /// <param name="err">错误信息, 没有错误该值为null</param>
        /// <returns>成功返回true</returns>
        public static bool ExecuteDbProcedure(string procedureName, DataSet ds, out string err)
        {
            err = null;

            if (m_dboperate == null)
            {
                GetIDBOperate();
            }

            Dictionary<OperateCMD, object> dicOperateCMD = m_dboperate.RunProc_CMD(procedureName, ds);
            return GetResult(dicOperateCMD, out err);
        }

        /// <summary>
        /// 获取操作结果
        /// </summary>
        /// <param name="dicOperateCMD">操作命令字典</param>
        /// <param name="err">错误信息, 没有错误该值为null</param>
        /// <returns>成功返回true</returns>
        public static bool GetResult(Dictionary<OperateCMD, object> dicOperateCMD, out string err)
        {
            err = null;

            if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
            {
                if (dicOperateCMD[OperateCMD.Return_Errmsg] != null)
                {
                    err = dicOperateCMD[OperateCMD.Return_Errmsg].ToString();
                    return false;
                }
            }

            return true;
        }
    }
}
