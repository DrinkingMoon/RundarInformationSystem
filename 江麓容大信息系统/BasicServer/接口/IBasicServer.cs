using System;
using System.Collections.Generic;

namespace ServerModule
{
    /// <summary>
    /// 基础服务接口
    /// </summary>
    public interface IBasicService
    {
        /// <summary>
        /// 获取或设置查询结果过滤器
        /// </summary>
        string QueryResultFilter { get; set; }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="collection">数据集</param>
        /// <param name="funWhere">where条件</param>
        /// <param name="funOrder">排序</param>
        /// <param name="pageSize">页尺寸</param>
        /// <param name="pageIndex">页索引</param>
        /// <returns>返回得到的数据</returns>
        IEnumerable<T> GetEnumerable<T>(IEnumerable<T> collection, Func<T, bool> funWhere, Func<T, string> funOrder, int pageSize, int pageIndex);
    }
}
