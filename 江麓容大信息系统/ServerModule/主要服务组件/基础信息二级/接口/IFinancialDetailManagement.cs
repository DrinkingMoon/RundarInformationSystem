using System;

namespace ServerModule
{
    /// <summary>
    /// 账务管理服务接口
    /// </summary>
    public interface IFinancialDetailManagement
    {
        /// <summary>
        /// 处理入库明细业务信息
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="indepotDetailInfo">入库明细信息</param>
        /// <param name="stockInfo">库存信息</param>
        void ProcessInDepotDetail(DepotManagementDataContext ctx, S_InDepotDetailBill indepotDetailInfo, S_Stock stockInfo);

        /// <summary>
        /// 处理出库明细业务信息
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="fetchGoodsDetailInfo">出库明细信息</param>
        /// <param name="stockInfo">库存信息</param>
        void ProcessFetchGoodsDetail(DepotManagementDataContext ctx, S_FetchGoodsDetailBill fetchGoodsDetailInfo, S_Stock stockInfo);
    }
}
