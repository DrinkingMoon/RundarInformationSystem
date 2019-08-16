using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerModule
{
    /// <summary>
    /// 包含状态的数据
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public class StateData<T>
    {
        /// <summary>
        /// 零件数据
        /// </summary>
        public T Data
        {
            get;
            set;
        }

        /// <summary>
        /// 数据状态枚举
        /// </summary>
        public enum DataStatusEnum 
        { 
            /// <summary>
            /// Nothing
            /// </summary>
            Nothing,
 
            /// <summary>
            /// Add
            /// </summary>
            Add, 

            /// <summary>
            /// Delete
            /// </summary>
            Delete, 

            /// <summary>
            /// Update
            /// </summary>
            Update 
        };

        /// <summary>
        /// 数据状态
        /// </summary>
        public DataStatusEnum DataStatus
        {
            get;
            set;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="data">零件信息数据</param>
        public StateData(T data)
        {
            Data = data;
        }
    }
}
