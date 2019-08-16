using System;
using ServerModule;

namespace Service_Quality_QC
{
    public interface IEightDReport : FlowControlService.IFlowBusinessService
    {
        /// <summary>
        /// 更新文件路径
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="guid">文件编号集字符串</param>
        /// <param name="address">上传文件存放位置</param>
        void UpdateFilePath(string billNo, string guid, string address);

        /// <summary>
        /// 保存业务信息
        /// </summary>
        /// <param name="billInfo">业务总单信息</param>
        void SaveInfo(Bus_Quality_8DReport billInfo);

        /// <summary>
        /// 获得单条业务总单信息
        /// </summary>
        /// <param name="billNo">业务号</param>
        /// <returns>返回业务总单信息</returns>
        Bus_Quality_8DReport GetSingleBillInfo(string billNo);
    }
}
