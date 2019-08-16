/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  ServerModuleFactory.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2010/07/17
 * 开发平台:  Visual C# 2008
 * 用于    :  仓库管理软件
 *----------------------------------------------------------------------------
 * 描述 : 
 * 其它 :
 *----------------------------------------------------------------------------
 * 公共信息: 参见系统'类帮助文档'
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2010/07/17 8:54:12 作者: 夏石友 当前版本: V1.00
 *        修改说明: 创建
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;

namespace ServerModule
{
    /// <summary>
    /// 服务类厂
    /// </summary>
    public class BasicServerFactory
    {
        /// <summary>
        /// 用于保证服务组件实例的唯一性
        /// </summary>
        static Hashtable m_hashTable = null;

        /// <summary>
        /// 获取服务组件
        /// </summary>
        /// <returns>返回组件接口</returns>
        public static T GetServerModule<T>()
        {
            string name = typeof(T).ToString();
                
            m_hashTable = new Hashtable();

            if (typeof(T) == typeof(IBarCodeServer))
            {
                IBarCodeServer serverModule = new BarCodeServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(ISearchParamsServer))
            {
                ISearchParamsServer serverModule = new SearchParamsServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IComprehensiveOperation))
            {
                IComprehensiveOperation serverModule = new ComprehensiveOperation();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IPrintManagement))
            {
                IPrintManagement serverModule = new PrintManagement();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IBillMessagePromulgatorServer))
            {
                IBillMessagePromulgatorServer serverModule = new BillMessagePromulgatorServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IProductParts))
            {
                IProductParts serverModule = new ProductParts();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IBillTypeServer))
            {
                IBillTypeServer serverModule = new BillTypeServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IAssignBillNoServer))
            {
                IAssignBillNoServer serverModule = new AssignBillNoServer();
                m_hashTable.Add(name, serverModule);
            }

            if (m_hashTable.ContainsKey(name))
            {
                return (T)m_hashTable[name];
            }

            return default(T);
        }
    }
}
