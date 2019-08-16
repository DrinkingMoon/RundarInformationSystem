using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting;

namespace TaskManagementServer
{
    /// <summary>
    /// 任务对象类厂
    /// </summary>
    public static class TaskObjectFactory
    {
        /// 获取业务操作接口
        /// </summary>
        /// <typeparam name="T">获取操作接口的类型</typeparam>
        /// <returns>返回组件接口</returns>
        public static T GetOperator<T>()
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
