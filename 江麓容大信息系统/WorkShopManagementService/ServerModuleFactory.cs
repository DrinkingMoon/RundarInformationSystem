/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  ServerModuleFactory.cs
 * 作者    :  曹津彬    版本: v1.00    日期: 2014/01/22
 * 开发平台:  Visual C# 2008
 * 用于    :  仓库管理软件
 *----------------------------------------------------------------------------
 * 描述 : 
 * 其它 :
 *----------------------------------------------------------------------------
 * 公共信息: 参见系统'类帮助文档'
 *----------------------------------------------------------------------------
 * 历史记录:
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using System.Reflection;
using System.Runtime.Remoting;

namespace Service_Manufacture_WorkShop
{
    /// <summary>
    /// 管理类厂
    /// </summary>
    public class ServerModuleFactory
    {
        /// <summary>
        /// 获取服务组件
        /// </summary>
        /// <returns>返回组件接口</returns>
        public static T GetServerModule<T>()
        {
            string name = typeof(T).ToString();

            try
            {
                int indexOfLastPoint = name.LastIndexOf('.');

                string className;

                if (indexOfLastPoint == -1)
                {
                    className = name.Substring(1);
                }
                else
                {
                    className = name.Substring(0, indexOfLastPoint + 1) + name.Substring(indexOfLastPoint + 2);
                }

                ObjectHandle objectHandle = Activator.CreateInstance(null, className);

                return (T)objectHandle.Unwrap();
            }
            catch (Exception)
            {
                return default(T);
            }
        }
    }
}

