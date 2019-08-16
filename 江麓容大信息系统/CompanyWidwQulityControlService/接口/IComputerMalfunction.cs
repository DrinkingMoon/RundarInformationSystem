using System;
using System.Data;
using ServerModule;
namespace Service_Peripheral_CompanyQuality
{
    public interface IComputerMalfunction : FlowControlService.IFlowBusinessService
    {
        /// <summary>
        /// 获得单条业务总单信息
        /// </summary>
        /// <param name="billNo">业务号</param>
        /// <returns>返回业务总单信息</returns>
        Business_Composite_ComputerMalfunction GetSingleBillInfo(string billNo);

        /// <summary>
        /// 保存业务信息
        /// </summary>
        /// <param name="billInfo">业务总单信息</param>
        /// <param name="effectValue">业务明细信息</param>
        void SaveInfo(Business_Composite_ComputerMalfunction billInfo);
    }
}
