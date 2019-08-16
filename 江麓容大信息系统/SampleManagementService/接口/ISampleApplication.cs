using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServerModule;
using System.Data;
using GlobalObject;

namespace Service_Project_Project
{
    public interface ISampleApplication : FlowControlService.IFlowBusinessService
    {

        bool IsInStore(Business_Sample_ConfirmTheApplication sampleInfo);

        /// <summary>
        /// 是否需要隔离
        /// </summary>
        /// <param name="sampleInfo">单据信息</param>
        /// <returns>需要返回True,不需要 返回 False</returns>
        bool IsNeedIsolation(Business_Sample_ConfirmTheApplication sampleInfo);

        /// <summary>
        /// 获得耗用数
        /// </summary>
        /// <param name="sample">单据信息</param>
        /// <returns>返回耗用数</returns>
        decimal GetUseCount(Business_Sample_ConfirmTheApplication sample);

        /// <summary>
        /// 更新文件路径
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="guid">文件编号集字符串</param>
        void UpdateFilePath(string billNo, string guid);

        /// <summary>
        /// 获得单条业务总单信息
        /// </summary>
        /// <param name="billNo">业务号</param>
        /// <returns>返回业务总单信息</returns>
        Business_Sample_ConfirmTheApplication GetSingleBillInfo(string billNo);

        /// <summary>
        /// 操作流程以外的业务
        /// </summary>
        /// <param name="billNo">单据号</param>
        void OperatarUnFlowBusiness(string billNo);
        
        /// <summary>
        /// 保存业务信息
        /// </summary>
        /// <param name="billInfo">业务总单信息</param>
        void SaveInfo(Business_Sample_ConfirmTheApplication billInfo);

    }
}
