using System;
using ServerModule;

namespace Service_Project_Design
{
    /// <summary>
    /// 发料清单单据状态
    /// </summary>
    public enum GoodsEnteringBillStatus
    {
        /// <summary>
        /// 新建单据
        /// </summary>
        新建单据,

        /// <summary>
        /// 等待校对
        /// </summary>
        等待校对,

        /// <summary>
        /// 单据完成
        /// </summary>
        单据完成
    }

    /// <summary>
    /// 物料录入申请单服务接口
    /// </summary>
    public interface IGoodsEnteringBill : FlowControlService.IFlowBusinessService
    {
        /// <summary>
        /// 检测物品信息
        /// </summary>
        /// <param name="goodsInfo">物品信息</param>
        /// <returns>存在返回True,否则返回False</returns>
        void CheckGoodsInfo(View_S_GoodsEnteringBill goodsInfo);

        /// <summary>
        /// 获得明细信息列表
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回List</returns>
        System.Collections.Generic.List<ServerModule.View_S_GoodsEnteringBill> GetListInfo(string billNo);
        
        /// <summary>
        /// 明细编辑
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="listInfo">明细信息列表</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,否则False</returns>
        bool EditListInfo(string billNo, System.Collections.Generic.List<View_S_GoodsEnteringBill> listInfo, out string error);

        /// <summary>
        /// 提交单据
        /// </summary>
        /// <param name="billNo">单号</param>
        /// <param name="listInfo">明细信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,否则False</returns>
        bool SubmitInfo(string billNo, System.Collections.Generic.List<View_S_GoodsEnteringBill> listInfo, out string error);
    }
}
