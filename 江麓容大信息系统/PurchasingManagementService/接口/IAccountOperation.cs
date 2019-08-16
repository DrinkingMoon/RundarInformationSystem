using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServerModule;
using System.Data;
using GlobalObject;

namespace Service_Economic_Purchase
{
    public interface IAccountOperation : FlowControlService.IFlowBusinessService
    {
        void DeleteInfo_Force(string billNo);

        void SetFinanceTime(string billNo, DateTime financeTime);

        DataTable GetTable(string yearMonth, SelectType selectType);

        void SubmitList(SelectType selectType, string provider, string yearMonth);

        DataTable GetTable_Detail(string provider, string yearMonth);

        /// <summary>
        /// 获得单条业务总单信息
        /// </summary>
        /// <param name="billNo">业务号</param>
        /// <returns>返回业务总单信息</returns>
        Bus_PurchasingMG_AccountBill GetSingleBillInfo(string billNo);

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="billInfo">单据信息</param>
        /// <param name="invoiceInfo">发票信息列表</param>
        /// <param name="detailInfo">明细信息列表</param>
        void SaveInfo(Bus_PurchasingMG_AccountBill billInfo,
            List<Bus_PurchasingMG_AccountBill_Invoice> invoiceInfo,
            List<View_Bus_PurchasingMG_AccountBill_Detail> detailInfo);

        /// <summary>
        /// 获得业务明细信息
        /// </summary>
        /// <param name="billNo">业务号</param>
        /// <returns>返回业务明细列表</returns>
        List<View_Bus_PurchasingMG_AccountBill_Detail> GetListViewDetailInfo(string billNo);

        /// <summary>
        /// 获得发票信息
        /// </summary>
        /// <param name="billNo">业务号</param>
        /// <returns>返回业务明细列表</returns>
        List<Bus_PurchasingMG_AccountBill_Invoice> GetListInvoiceInfo(string billNo);

        /// <summary>
        /// 操作流程以外的业务
        /// </summary>
        /// <param name="billNo">单据号</param>
        void OperatarUnFlowBusiness(string billNo);

        DataTable LeadInputAccount(string provider);
    }
}
