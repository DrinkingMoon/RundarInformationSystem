using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServerModule;
using System.Data;

namespace Service_Economic_Purchase
{
    public interface IPartsBelongProviderChangeService : FlowControlService.IFlowBusinessService
    {

        string GetProviderLV(int goodsID, string provider);

        /// <summary>
        /// 获得合格供应商与零件责任归属信息
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回LNQ信息</returns>
        B_AccessoryDutyInfo GetDutyInfo(int goodsID);

        /// <summary>
        /// 获得业务明细信息
        /// </summary>
        /// <param name="billNo">业务号</param>
        /// <returns>返回业务明细列表</returns>
        List<ServerModule.View_Business_PurchasingMG_PartsBelongPriovderChangeDetail> GetListViewDetailInfo(string billNo);

        /// <summary>
        /// 获得单条业务总单信息
        /// </summary>
        /// <param name="billNo">业务号</param>
        /// <returns>返回业务总单信息</returns>
        Business_PurchasingMG_PartsBelongPriovderChange GetSingleBillInfo(string billNo);

        /// <summary>
        /// 保存业务信息
        /// </summary>
        /// <param name="billInfo">业务总单信息</param>
        /// <param name="detailInfo">业务明细信息</param>
        void SaveInfo(Business_PurchasingMG_PartsBelongPriovderChange billInfo,
            List<View_Business_PurchasingMG_PartsBelongPriovderChangeDetail> detailInfo);

        /// <summary>
        /// 结束业务
        /// </summary>
        /// <param name="billNo">业务编号</param>
        void FinishBill(string billNo);
    }
}
