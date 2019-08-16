using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ServerModule;

namespace FlowControlService
{
    public interface IFlowBusinessService : IBasicBillServer
    {
        /// <summary>
        /// 删除业务
        /// </summary>
        /// <param name="billNo">业务号</param>
        void DeleteInfo(string billNo);
    }
}
