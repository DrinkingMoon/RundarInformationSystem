using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerModule
{
    /// <summary>
    /// 单据服务类必须实现的接口
    /// </summary>
    public interface IBasicBillServer
    {
        /// <summary>
        /// 检查某单据是否存在
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>存在返回true</returns>
        bool IsExist(string billNo);

        /// <summary>
        /// 检查某单据是否存在
        /// </summary>
        /// <param name="dataContxt">数据上下文</param>
        /// <param name="billNo">单据号</param>
        /// <returns>存在返回true</returns>
        bool IsExist(DepotManagementDataContext dataContxt, string billNo);
    }
}
