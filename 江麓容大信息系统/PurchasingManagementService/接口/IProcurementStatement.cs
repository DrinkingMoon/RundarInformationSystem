using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServerModule;
using System.Data;
using GlobalObject;

namespace Service_Economic_Purchase
{
    public interface IProcurementStatement : FlowControlService.IFlowBusinessService
    {
        /// <summary>
        /// 获得单条业务总单信息
        /// </summary>
        /// <param name="billNo">业务号</param>
        /// <returns>返回业务总单信息</returns>
        Business_Settlement_ProcurementStatement GetSingleBillInfo(string billNo);

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="billInfo">单据信息</param>
        /// <param name="invoiceInfo">发票信息列表</param>
        /// <param name="detailInfo">明细信息列表</param>
        void SaveInfo(Business_Settlement_ProcurementStatement billInfo, List<Business_Settlement_ProcurementStatement_Invoice> invoiceInfo,
            List<View_Business_Settlement_ProcurementStatementDetail> detailInfo);

        /// <summary>
        /// 获得业务明细信息
        /// </summary>
        /// <param name="billNo">业务号</param>
        /// <returns>返回业务明细列表</returns>
        List<View_Business_Settlement_ProcurementStatementDetail> GetListViewDetailInfo(string billNo);

        /// <summary>
        /// 获得发票信息
        /// </summary>
        /// <param name="billNo">业务号</param>
        /// <returns>返回业务明细列表</returns>
        List<Business_Settlement_ProcurementStatement_Invoice> GetListInvoiceInfo(string billNo);

        /// <summary>
        /// 导入单据明细
        /// </summary>
        /// <param name="lstBillNo">单据号列表</param>
        /// <param name="billType">单据类型</param>
        /// <returns>返回Table</returns>
        DataTable LeadInDetail(List<string> lstBillNo, CE_ProcurementStatementBillTypeEnum billType);

        /// <summary>
        /// 导入入库单号信息
        /// </summary>
        /// <param name="provider">供应商</param>
        /// <param name="startTime">起始时间</param>
        /// <param name="endTime">截止时间</param>
        /// <returns>返回Table</returns>
        DataTable LeadInInPutBillInfo(string provider, DateTime startTime, DateTime endTime);
        
        /// <summary>
        /// 操作流程以外的业务
        /// </summary>
        /// <param name="billNo">单据号</param>
        void OperatarUnFlowBusiness(string billNo);
    }
}
