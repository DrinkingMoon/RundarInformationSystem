using System;

namespace ServerModule
{
    /// <summary>
    /// 自制件退货单服务接口
    /// </summary>
    public interface IHomemadeRejectBill : IBasicService, IBasicBillServer
    {
        /// <summary>
        /// 添加自制件退货单
        /// </summary>
        /// <param name="bill">单据信息</param>
        /// <param name="returnBill">返回更新后的自制件退货单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加自制件退货单</returns>
        bool AddBill(S_HomemadeRejectBill bill, out PlatformManagement.IQueryResult returnBill, out string error);

        /// <summary>
        /// 获得所有的自制件
        /// </summary>
        /// <returns>返回数据集</returns>
        System.Data.DataTable GetHomemadePartInfo();

        /// <summary>
        /// 删除自制件退货单
        /// </summary>
        /// <param name="billNo">退货单号</param>
        /// <param name="returnBill">返回更新后重新查询的自制件退货单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功删除退货单号</returns>
        bool DeleteBill(string billNo, out PlatformManagement.IQueryResult returnBill, out string error);

        /// <summary>
        /// 财务审批单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="name">审批人姓名</param>
        /// <param name="returnBill">返回更新后重新查询的自制件退货单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加自制件退货单</returns>
        bool FinanceAuthorizeBill(string billNo, string name, out PlatformManagement.IQueryResult returnBill, out string error);

        /// <summary>
        /// 完成自制件退货单
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="storeManager">仓库管理员</param>
        /// <param name="returnBill">返回更新后重新查询的自制件退货单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加采购退货单</returns>
        bool FinishBill(string billNo, string storeManager, out PlatformManagement.IQueryResult returnBill, out string error);

        /// <summary>
        /// 获取自制件退货单信息
        /// </summary>
        /// <param name="returnBill">返回的自制件退货单信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取领料出库信息</returns>
        bool GetAllBill(out PlatformManagement.IQueryResult returnBill, out string error);

        /// <summary>
        /// 获取自制件退货单视图信息
        /// </summary>
        /// <param name="billNo">退货单号</param>
        /// <returns>成功返回获取领料出库信息, 失败返回null</returns>
        System.Data.DataTable GetBillView(string billNo);

        /// <summary>
        /// 回退单据
        /// </summary>
        /// <param name="strDJH">单据号</param>
        /// <param name="strBillStatus">单据状态</param>
        /// <param name="returnBill">返回更新后重新查询的自制件退货单数据集</param>
        /// <param name="strRebackReason">回退原因</param>
        /// <param name="m_err">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool ReturnBill(string strDJH, string strBillStatus, out PlatformManagement.IQueryResult returnBill, out string m_err, string strRebackReason);

        /// <summary>
        /// 退货人提交单据(交给财务审批)
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="returnBill">返回更新后重新查询的自制件退货单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加自制件退货单</returns>
        bool SubmitNewBill(string billNo, out PlatformManagement.IQueryResult returnBill, out string error);

        /// <summary>
        /// 修改自制件退货单(只修改编制人涉及信息)
        /// </summary>
        /// <param name="bill">单据信息</param>
        /// <param name="returnBill">返回更新后的自制件退货单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加自制件退货单</returns>
        bool UpdateBill(S_HomemadeRejectBill bill, out PlatformManagement.IQueryResult returnBill, out string error);
    }
}
