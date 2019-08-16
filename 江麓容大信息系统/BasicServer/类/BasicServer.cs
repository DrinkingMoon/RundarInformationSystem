using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBOperate;

namespace ServerModule
{
    /// <summary>
    /// 服务类基类
    /// </summary>
    public abstract class BasicServer : ServerModule.IBasicService
    {
        /// <summary>
        /// 查询结果过滤器
        /// </summary>
        string m_queryResultFilter = null;

        /// <summary>
        /// 获取或设置查询结果过滤器
        /// </summary>
        public string QueryResultFilter
        {
            get { return m_queryResultFilter; }
            set { m_queryResultFilter = value; }
        }

        /// <summary>
        /// 数据库操作接口
        /// </summary>
        protected static IDBOperate m_dbOperate = CommentParameter.GetDBOperatorOfDepotManagement();

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
        public IEnumerable<T> GetEnumerable<T>(IEnumerable<T> collection, Func<T, bool> funWhere, 
            Func<T, string> funOrder, int pageSize, int pageIndex)
        {
            if (funWhere == null && funOrder == null)
            {
                var result = collection.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                return result;
            }
            else if (funWhere == null && funOrder != null)
            {
                var result = collection.OrderBy(funOrder).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                return result;
            }
            else if (funWhere != null && funOrder == null)
            {
                var result = collection.Where(funWhere).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                return result;
            }
            else
            {
                var result = collection.Where(funWhere).OrderBy(funOrder).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                return result;
            }
        }
    }
}
