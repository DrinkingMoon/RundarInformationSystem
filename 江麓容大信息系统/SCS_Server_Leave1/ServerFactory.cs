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
    /// 供应链管理（supply chain management）第一层，服务类厂
    /// </summary>
    public class SCM_Level01_ServerFactory
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

            if (typeof(T) == typeof(IBasicGoodsServer))
            {
                IBasicGoodsServer serverModule = new BasicGoodsServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IDepotTypeForPersonnel))
            {
                IDepotTypeForPersonnel serverModule = new DepotTypeForPersonnel();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IPersonnelInfoServer))
            {
                IPersonnelInfoServer serverModule = new PersonnelInfoServer();
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
