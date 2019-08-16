using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using System.Runtime.Remoting;

namespace Service_Peripheral_External
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
