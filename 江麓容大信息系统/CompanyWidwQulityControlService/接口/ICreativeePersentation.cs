using System;
using ServerModule;
using System.Collections.Generic;
using System.Data;

namespace Service_Peripheral_CompanyQuality
{
    /// <summary>
    /// 附件类别
    /// </summary>
    public enum SelfSimpleEnum_CreativeePersentation
    {
        Before,
        After
    }

    public interface ICreativeePersentation : FlowControlService.IFlowBusinessService
    {
        /// <summary>
        /// 获得经济效果信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回信息</returns>
        Business_CWQC_CreativePersentation_EffectValue GetInfo_EffectValue(string billNo);

        /// <summary>
        /// 直接录入单独操作业务流程
        /// </summary>
        /// <param name="billNo">单据号</param>
        void DirectAdd(string billNo);

        /// <summary>
        /// 附件上传
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="fileNo">附件唯一标识码</param>
        /// <param name="simple">附件类型</param>
        void UpdateFilePath(string billNo, string fileNo, SelfSimpleEnum_CreativeePersentation simple);

        /// <summary>
        /// 获得参考信息
        /// </summary>
        /// <param name="type">类型信息</param>
        /// <returns>返回Table</returns>
        DataTable GetReferenceInfo(string type);

        /// <summary>
        /// 获得单条信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回信息</returns>
        ServerModule.Business_CWQC_CreativePersentation GetSingleBillInfo(string billNo);

        /// <summary>
        /// 保存业务信息
        /// </summary>
        /// <param name="billInfo">业务总单信息</param>
        /// <param name="effectValue">业务明细信息</param>
        void SaveInfo(ServerModule.Business_CWQC_CreativePersentation billInfo, ServerModule.Business_CWQC_CreativePersentation_EffectValue effectValue);
    }
}
