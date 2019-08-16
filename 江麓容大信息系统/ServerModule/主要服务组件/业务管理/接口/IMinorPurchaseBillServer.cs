using System;
using PlatformManagement;
using System.Data;
namespace ServerModule
{
    /// <summary>
    /// 单据状态
    /// </summary>
    public enum MinorPurchaseBillStatus
    {
        /// <summary>
        /// 新建单据
        /// </summary>
        新建单据,

        /// <summary>
        /// 等待仓管确认
        /// </summary>
        等待仓管确认,

        /// <summary>
        /// 等待部门负责人
        /// </summary>
        等待部门负责人审核,

        /// <summary>
        /// 等待确认日期
        /// </summary>
        等待确认日期,

        /// <summary>
        /// 等待高级负责人审批
        /// </summary>
        等待高级负责人审批,

        /// <summary>
        /// 等待分管领导审批
        /// </summary>
        等待分管领导审核,

        /// <summary>
        /// 等待财务审批
        /// </summary>
        等待财务审核,

        /// <summary>
        /// 等待总审核
        /// </summary>
        等待总经理审核,

        /// <summary>
        /// 等待采购分配采购
        /// </summary>
        等待采购部调配人员,

        /// <summary>
        /// 等待采购工程师确认采购
        /// </summary>
        等待采购工程师确认采购,

        /// <summary>
        /// 等待采购工程师确认到货
        /// </summary>
        等待确认到货,

        /// <summary>
        /// 已完成
        /// </summary>
        已完成
    }

    /// <summary>
    /// 零星采购单服务接口
    /// </summary>
    public interface IMinorPurchaseBillServer
    {
        /// <summary>
        /// 获得合计金额
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回金额</returns>
        decimal GetSumPrice(string billNo);

        /// <summary>
        /// 获得引用的合同信息
        /// </summary>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <returns>返回Table</returns>
        DataTable GetBargainRelate(DateTime startDate, DateTime endDate);

        /// <summary>
        /// 获得明细单条记录
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <returns>返回单条LNQ记录</returns>
        B_MinorPurchaseList GetListSingle(string billNo, string goodsCode, string goodsName, string spec);

        /// <summary>
        /// 获取或设置查询结果过滤器
        /// </summary>
        string QueryResultFilter { get; set; }

        /// <summary>
        /// 获得单据列表
        /// </summary>
        /// <param name="returnInfo">零星采购信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool GetAllBillInfo(out IQueryResult returnInfo, out string error);

        /// <summary>
        /// 获得单条信息
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <returns>返回LINQ数据集</returns>
        ServerModule.B_MinorPurchaseBill GetBillInfo(string billID);

        /// <summary>
        /// 获得明细信息
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <returns>返回Table</returns>
        System.Data.DataTable GetListInfo(string billID);

        /// <summary>
        /// 获得已到货的明细信息
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <returns>返回Table</returns>
        DataTable GetFinishListInfo(string billID);

        /// <summary>
        /// 提交申请
        /// </summary>
        /// <param name="minorBill">单据信息数据集</param>
        /// <param name="listInfo">明细信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool InsertBill(ServerModule.B_MinorPurchaseBill minorBill, System.Data.DataTable listInfo, out string error);

        /// <summary>
        /// 操作业务
        /// </summary>
        /// <param name="minorBill">单据信息数据集</param>
        /// <param name="listInfo">单据明细信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool OperationInfo(ServerModule.B_MinorPurchaseBill minorBill, System.Data.DataTable listInfo, out string error);

        /// <summary>
        /// 删除零星采购单
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool DeleteInfo(string billNo, out string error);

        /// <summary>
        /// 回退单据
        /// </summary>
        /// <param name="strDJH">单据号</param>
        /// <param name="strBillStatus">单据状态</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <param name="strRebackReason">回退原因</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool ReturnBill(string strDJH, string strBillStatus, out string error, string strRebackReason);

        /// <summary>
        /// 通过物品ID获得物品库存信息
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回库存信息</returns>
        string GetGoodsStock(int goodsID);

        /// <summary>
        /// 新增采购变更处置单
        /// </summary>
        /// <param name="changBill">采购变更处置信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>新增成功返回true，失败返回false</returns>
        bool InsertChangeBill(B_MinorPurchaseChangeBill changBill, out string error);

        /// <summary>
        /// 删除采购变更处置单
        /// </summary>
        /// <param name="id">编号</param>
        /// <param name="error">错误信息</param>
        /// <returns>删除成功返回true，失败返回false</returns>
        bool DeleteChangeBill(int id, out string error);

        /// <summary>
        /// 修改采购变更处置单
        /// </summary>
        /// <param name="changBill">采购变更处置信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>新增成功返回true，失败返回false</returns>
        bool UpdateChangeBill(B_MinorPurchaseChangeBill changBill, out string error);

        /// <summary>
        /// 获得时间范围内的采购变更处置单的所有信息
        /// </summary>
        /// <param name="starTime">起始日期</param>
        /// <param name="endTime">结束日期</param>
        /// <param name="status">单据状态</param>
        /// <returns>返回数据集</returns>
        System.Data.DataTable GetMinorPurchaseChangeBill(DateTime starTime, DateTime endTime, string status);

        /// <summary>
        /// 通过关联号查询变更单是否走完
        /// </summary>
        /// <param name="associateBillNo">关联单号</param>
        /// <returns>完成返回true，未完成返回false</returns>
        bool IsFinishMinorPur(string associateBillNo);

        /// <summary>
        /// 删除采购变更处置单
        /// </summary>
        /// <param name="associateBillNo">关联单号</param>
        /// <param name="error">错误信息</param>
        /// <returns>删除成功返回true，失败返回false</returns>
        bool DeleteChangeBill(string associateBillNo, out string error);

        /// <summary>
        /// 获得采购申请单的统计报表
        /// </summary>
        /// <param name="startYear">查询起始日期的年</param>
        /// <param name="startMonth">查询起始日期的月</param>
        /// <param name="endYear">查询截止日期的年</param>
        /// <param name="endMonth">查询截止日期的月</param>
        /// <param name="dept">需要查询的部门</param>
        /// <returns>返回统计报表</returns>
        System.Data.DataTable GetStatisticsTable(string startYear, string startMonth, string endYear, string endMonth, string dept);

        /// <summary>
        /// 修改单据中已打印的状态
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>修改成功返回true失败返回false</returns>
        bool UpdatePrintStatus(string billNo, out string error);

        /// <summary>
        /// 在审批通过后，采购部文员分配采购工程师
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="workID">采购工程师工号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true失败返回false</returns>
        bool UpdateMinorPurchaseBill(string billNo, string workID, out string error);

        /// <summary>
        /// 通过用途编号获得用途名称
        /// </summary>
        /// <param name="purposeCode">用途编号</param>
        /// <returns>成功返回用途名称，失败返回Null</returns>
        string GetPurposeNameByCode(string purposeCode);

        /// <summary>
        /// 同一零件近期是否购买过
        /// </summary>
        /// <param name="goodsID">零件ID</param>
        /// <param name="billNo">单据号</param>
        /// <returns>返回数据集</returns>
        DataTable GetPart(int goodsID, string billNo);
    }
}
