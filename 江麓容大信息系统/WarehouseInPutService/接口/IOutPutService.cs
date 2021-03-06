﻿using System;
using ServerModule;
using System.Data;
using System.Collections.Generic;

namespace Service_Manufacture_Storage
{
    public interface IOutPutService : FlowControlService.IFlowBusinessService
    {
        /// <summary>
        /// 获得业务明细信息
        /// </summary>
        /// <param name="billNo">业务号</param>
        /// <returns>返回业务明细列表</returns>
        List<ServerModule.View_Business_WarehouseOutPut_OutPutDetail> GetListViewDetailInfo(string billNo);

        /// <summary>
        /// 获得单条业务总单信息
        /// </summary>
        /// <param name="billNo">业务号</param>
        /// <returns>返回业务总单信息</returns>
        Business_WarehouseOutPut_OutPut GetSingleBillInfo(string billNo);

        /// <summary>
        /// 保存业务信息
        /// </summary>
        /// <param name="billInfo">业务总单信息</param>
        /// <param name="detailInfo">业务明细信息</param>
        void SaveInfo(Business_WarehouseOutPut_OutPut billInfo, List<View_Business_WarehouseOutPut_OutPutDetail> detailInfo);

        /// <summary>
        /// 结束业务
        /// </summary>
        /// <param name="billNo">业务编号</param>
        void FinishBill(string billNo);

        DataTable GetReferenceInfo(string billType, string deptCode, bool isRepeat);

        List<View_Business_WarehouseOutPut_OutPutDetail> GetReferenceListViewDetail(string billNo, List<string> listBillNo, bool isRepeat, out string deptCode);
    }
}
