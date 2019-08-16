using System;
using GlobalObject;
using System.Collections.Generic;

namespace ServerModule
{
    /// <summary>
    /// 系统日志操作组件接口
    /// </summary>
    public interface ISystemLogServer
    {
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="mode">操作模式</param>
        /// <param name="operationContent">操作对象</param>
        /// <param name="NoUpdatedContent">操作前对象</param>
        void RecordLog<T>(GlobalObject.CE_OperatorMode mode, T operationContent, T NoUpdatedContent);

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="mode">操作模式</param>
        /// <param name="tObj">操作对象</param>
        /// <param name="listContent">操作内容日志列表</param>
        void RecordLog<T>(CE_OperatorMode mode, T tObj, List<SystemLog_Content> listContent);

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="mode">操作模式</param>
        /// <param name="explainContent">操作说明</param>
        /// <param name="singlePrimaryKeyContent">单一主键值</param>
        void RecordLog<T>(CE_OperatorMode mode, string explainContent, object singlePrimaryKeyContent);

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="mode">操作模式</param>
        /// <param name="explainContent">操作说明</param>
        void RecordLog<T>(CE_OperatorMode mode, string explainContent);

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="explainContent">操作说明</param>
        void RecordLog<T>(string explainContent);

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="explainContent">操作说明</param>
        void RecordLog(string explainContent);
    }
}
