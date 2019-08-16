using System;
using System.Collections.Generic;
using GlobalObject;
using System.Data;
using ServerModule;

namespace FlowControlService
{
    /// <summary>
    /// 流程服务组件
    /// </summary>
    public interface IFlowServer
    {
        /// <summary>
        /// 获得流程单据信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回流程单据信息对象</returns>
        Flow_FlowBillData GetBillData(string billNo);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="billType"></param>
        /// <returns></returns>
        List<string> GetBusinessInfoVersion(CE_BillTypeEnum billType);

        /// <summary>
        /// 获得当前业务流程信息对象
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回对象</returns>
        Flow_FlowInfo GetNowFlowInfo(string billNo);

        /// <summary>
        /// 获得对应业务的单据信息
        /// </summary>
        /// <param name="billType">业务类型</param>
        /// <param name="version">版本号</param>
        /// <param name="lstbusinessStatus"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="findType"></param>
        /// <param name="billNo"></param>
        /// <returns>返回Table</returns>
        DataTable ShowBusinessAllInfo(CE_BillTypeEnum billType, string version,
            string[] lstbusinessStatus, DateTime startDate, DateTime endDate, string findType, string billNo);

        /// <summary>
        /// 获得流程单据信息
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">单据号</param>
        /// <returns>返回流程单据信息对象</returns>
        Flow_FlowBillData GetBillData(DepotManagementDataContext ctx, string billNo);

        /// <summary>
        /// 回退已完成的单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="advise">他人意见</param>
        /// <param name="storageIDOrWorkShopCode">库房/车间编号</param>
        /// <param name="flowID">流程ID</param>
        /// <param name="keyWords">关键字</param>
        void FlowReback_Finish(string billNo, string advise, string storageIDOrWorkShopCode, int flowID, string keyWords);

        /// <summary>
        /// 获得每个流程节点的信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回信息列表</returns>
        List<Flow_FlowData> GetBusinessOperationInfo(string billNo);

        /// <summary>
        /// 获得某个操作节点的操作信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="flowID">流程ID</param>
        /// <returns>返回List</returns>
        List<Flow_FlowData> GetBusinessOperationInfo(string billNo, int flowID);

        /// <summary>
        /// 获得流程信息
        /// </summary>
        /// <param name="billType">单据类型</param>
        /// <param name="version">版本号</param>
        /// <returns>返回流程信息</returns>
        Flow_BusinessInfo GetBusinessInfo(CE_BillTypeEnum billType, string version);

        /// <summary>
        /// 获得下一个流程的状态
        /// </summary>
        /// <param name="billNo"></param>
        /// <returns></returns>
        string GetNextBillStatus(string billNo);

        /// <summary>
        /// 是否要指定人或者角色
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="isParallel">是否并行</param>
        /// <returns>需要返回True,不需要返回False</returns>
        bool IsPointPersonnel(string billNo, out bool isParallel);

        /// <summary>
        /// 完成引用单据的消息传递
        /// </summary>
        /// <param name="billNo"></param>
        /// <param name="lstReferenceBillNo"></param>
        void FlowFinishReferenceSendMessage(string billNo, List<string> lstReferenceBillNo);

        /// <summary>
        /// 强制删除业务单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        void FlowForceDelete(string billNo);

        /// <summary>
        /// 获得某个操作节点的操作信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="businessStatus">业务状态</param>
        /// <returns>返回List</returns>
        List<Flow_FlowData> GetBusinessOperationInfo(string billNo, string businessStatus);

        /// <summary>
        /// 获得业务流程信息对象列表
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回对象列表</returns>
        List<Flow_FlowInfo> GetListFlowInfo(string billNo);

        /// <summary>
        /// 流程回退
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="advise">他人意见</param>
        /// <param name="storageIDOrWorkShopCode">库房/车间编号</param>
        /// <param name="flowID">流程ID</param>
        /// <param name="keyWords">关键字</param>
        void FlowReback(string billNo, string advise, string storageIDOrWorkShopCode, int flowID, string keyWords);

        /// <summary>
        /// 判断单据是否存在
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>存在返回True,不存在返回False</returns>
        bool IsExist(string billNo);

        /// <summary>
        /// 流程暂存
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="storageIDOrWorkShopCode">库房/车间编号</param>
        /// <param name="advise">他人意见</param>
        /// <param name="keyWords">关键字</param>
        void FlowHold(string billNo, string storageIDOrWorkShopCode, string advise, string keyWords);

        /// <summary>
        /// 删除业务单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        void FlowDelete(DepotManagementDataContext ctx, string billNo);

        /// <summary>
        /// 获得单据状态
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回字符串</returns>
        string GetNowBillStatus(string billNo);

        /// <summary>
        /// 获得执行流程
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="businessTypeID"></param>
        /// <returns>返回字典：流程顺序，流程名，是否已执行标志</returns>
        Dictionary<int, Dictionary<string, bool>> GetExcuteFlowInfo(string billNo, int businessTypeID);

        /// <summary>
        /// 获得当前业务流程信息对象
        /// </summary>
        /// <param name="businessTypeID">业务ID</param>
        /// <param name="billNo">单据号</param>
        /// <returns>返回对象</returns>
        Flow_FlowInfo GetNowFlowInfo(int businessTypeID, string billNo);

        /// <summary>
        /// 获得业务ID
        /// </summary>
        /// <param name="billType">业务类型</param>
        /// <param name="version">版本号，可为空</param>
        /// <returns>返回业务ID</returns>
        int GetBusinessTypeID(CE_BillTypeEnum billType, string version);

        /// <summary>
        /// 流程传输主方法(默认)
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="advise">操作人员建议</param>
        /// <param name="storageIDOrWorkShopCode">库房/车间编码</param>
        /// <param name="notifyPersonnel">指定知会人员列表</param>
        /// <param name="keyWords">关键字</param>
        void FlowPass(string billNo, string advise, string storageIDOrWorkShopCode, NotifyPersonnelInfo notifyPersonnel, string keyWords);

        /// <summary>
        /// 获得业务状态列表
        /// </summary>
        /// <param name="billType">业务类型</param>
        /// <param name="version">版本号 可为空</param>
        /// <returns>返回列表</returns>
        List<string> GetBusinessStatus(CE_BillTypeEnum billType, string version);

        /// <summary>
        /// 获得历史操作信息对象列表
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回对象列表</returns>
        List<GlobalObject.CommonProcessInfo> GetFlowData(string billNo);

        ///// <summary>
        ///// 获得业务流程名列表
        ///// </summary>
        ///// <param name="nameAndVersion">业务名+版本信息</param>
        ///// <returns>返回列表</returns>
        //List<string> GetBusinessAllAuthority(string nameAndVersion);

        ///// <summary>
        ///// 获得业务信息对象
        ///// </summary>
        ///// <param name="nameAndVersion">业务名+版本信息</param>
        ///// <returns>返回对象</returns>
        //Flow_BusinessInfo GetBusinessInfo(string nameAndVersion);

        ///// <summary>
        ///// 智能获得流程名称
        ///// </summary>
        ///// <param name="nameAndVersion">业务名+版本信息</param>
        ///// <param name="billNo">单据号</param>
        ///// <returns>返回流程名称字符串</returns>
        //string GetBillNoAuthority(string nameAndVersion, string billNo);
    }
}
