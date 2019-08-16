using System;
using System.Data;

namespace ServerModule
{
    /// <summary>
    /// 下线不合格品放行单服务组件接口
    /// </summary>
    public interface IProductReleases : IBasicBillServer
    {
        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        void DeleteBill(string billNo);

        /// <summary>
        /// 获得全部单据
        /// </summary>
        /// <param name="startTime">起始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="billStatus">单据状态</param>
        /// <param name="flagIsNew">是否显示最新信息 True :是 False :否</param>
        /// <returns>返回获得的全部单据信息</returns>
        DataTable GetAllBill(DateTime startTime, DateTime endTime, string billStatus, bool flagIsNew);

        /// <summary>
        /// 提交单据
        /// </summary>
        /// <param name="produtctReleases">LINQ实体集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        bool SubmitBill(ServerModule.ZL_ProductReleases produtctReleases, out string error);
    }
}
