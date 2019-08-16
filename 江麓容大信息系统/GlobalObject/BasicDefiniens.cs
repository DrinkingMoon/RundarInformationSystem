using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformManagement
{
    /// <summary>
    /// 表达式操作枚举
    /// </summary>
    public enum ExpressionOperation
    {
        大于,
        小于,
        等于,
        不等于,
        大于等于,
        小于等于,

        /// <summary>
        /// 用于模糊查询
        /// </summary>
        包含,

        是,

        不是
    }

    ///// <summary>
    ///// 权限基础信息
    ///// </summary>
    //public class AuthorityBasicInfo
    //{
    //    /// <summary>
    //    /// 获取业务操作集合
    //    /// </summary>
    //    /// <returns>返回获取到的业务操作集合</returns>
    //    public static string[] GetBusinessOperation()
    //    {
    //        return new string[] { "查询", "添加", "更新", "删除" };
    //    }

    //    /// <summary>
    //    /// 获取表达式操作集合
    //    /// </summary>
    //    /// <returns>返回获取到的表达式操作集合</returns>
    //    public static string[] GetExpressionOperation()
    //    {
    //        return new string[] {">", ">=", "<", "<=", "=", "!=", "in", "like", "is", "is not" };
    //    }

    //    /// <summary>
    //    /// 获取逻辑操作集合
    //    /// </summary>
    //    /// <returns>返回获取到的逻辑操作集合</returns>
    //    public static string[] GetLogicOperation()
    //    {
    //        return new string[] { "AND", "OR" };
    //    }
    //}

    /// <summary>
    /// 表达式操作符转换器
    /// </summary>
    /// <remarks>将枚举转换成对应的SQL字符</remarks>
    public class ExpressionOperationConverter
    {
        /// <summary>
        /// 将表达式操作符转换为数据库可理解的字符串
        /// </summary>
        /// <param name="eo">表达式操作符</param>
        /// <returns>字符串</returns>
        public static string ToString(ExpressionOperation eo)
        {
            switch (eo)
            {
                case ExpressionOperation.大于:
                    return ">";
                case ExpressionOperation.小于:
                    return "<";
                case ExpressionOperation.等于:
                    return "=";
                case ExpressionOperation.不等于:
                    return "!=";
                case ExpressionOperation.大于等于:
                    return ">=";
                case ExpressionOperation.小于等于:
                    return "<=";
                case ExpressionOperation.包含:
                    return "like";
                case ExpressionOperation.是:
                    return "is";
                case ExpressionOperation.不是:
                    return "is not";
                default:
                    throw new Exception("未知表达式操作符");
            }
        }
    }
    /// <summary>
    /// 逻辑操作枚举
    /// </summary>
    enum LogicOperation
    {
        AND,
        OR
    }
}
