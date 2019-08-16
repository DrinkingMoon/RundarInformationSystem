using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServerModule;
using System.Data;

namespace Service_Quality_QC
{
    public interface IRejectIsolationService : FlowControlService.IFlowBusinessService
    {
        /// <summary>
        /// 获得单据补充信息列表
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回Table</returns>
        DataTable GetSupplementeMessageInfo(string billNo);

        /// <summary>
        /// 添加补充信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="msg">补充信息</param>
        void AddSupplementMessage(string billNo, string msg);

        /// <summary>
        /// 修改物品库存状态
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billInfo">单据信息</param>
        /// <param name="goodsStatus">物品状态</param>
        void UpdateStockGoodsStatus(DepotManagementDataContext ctx, Business_QualityManagement_Isolation billInfo, int goodsStatus);

        bool IsRepeatIsolation(int goodsID, string batchNo, string storageID);

        /// <summary>
        /// 更新文件路径
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="guid">文件编号集字符串</param>
        void UpdateFilePath(string billNo, string guid);

        void FinishBill(string billNo);

        ServerModule.Business_QualityManagement_Isolation GetSingleBillInfo(string billNo);

        void SaveInfo(ServerModule.Business_QualityManagement_Isolation billInfo);
    }
}
