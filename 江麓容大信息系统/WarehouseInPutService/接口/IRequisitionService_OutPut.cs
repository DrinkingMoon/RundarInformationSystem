using System;
using System.Collections.Generic;
using ServerModule;
using System.Data;
using GlobalObject;

namespace Service_Manufacture_Storage
{
    /// <summary>
    /// 入库业务申请单服务类接口
    /// </summary>
    public interface IRequisitionService_OutPut : FlowControlService.IFlowBusinessService
    {
        /// <summary>
        /// 获得业务明细信息
        /// </summary>
        /// <param name="billNo">业务号</param>
        /// <returns>返回业务明细列表</returns>
        List<ServerModule.View_Business_WarehouseOutPut_RequisitionDetail> GetListViewDetailInfo(string billNo);

        /// <summary>
        /// 获得单条业务总单信息
        /// </summary>
        /// <param name="billNo">业务号</param>
        /// <returns>返回业务总单信息</returns>
        Business_WarehouseOutPut_Requisition GetSingleBillInfo(string billNo);

        /// <summary>
        /// 保存业务信息
        /// </summary>
        /// <param name="billInfo">业务总单信息</param>
        /// <param name="detailInfo">业务明细信息</param>
        void SaveInfo(Business_WarehouseOutPut_Requisition billInfo, List<View_Business_WarehouseOutPut_RequisitionDetail> detailInfo);

        List<Business_WarehouseOutPut_Requisition> GetListBillInfo(List<string> listBillNo);

        DataTable GetListViewDetial(string billNo, int? goodsID, string batchNo, string provider);

        //List<View_Business_WarehouseOutPut_RequisitionDetail> GetListViewDetail_OrderForm(string billNo, List<string> listOrderForm);
    }
}
