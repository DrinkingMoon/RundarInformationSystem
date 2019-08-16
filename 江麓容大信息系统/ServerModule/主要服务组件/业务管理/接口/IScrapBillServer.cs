using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using DBOperate;
using PlatformManagement;
using GlobalObject;

namespace ServerModule
{
    /// <summary>
    /// 报废单单据状态
    /// </summary>
    public enum ScrapBillStatus
    {
        /// <summary>
        /// 新建单据
        /// </summary>
        新建单据,

        /// <summary>
        /// 等待主管审核
        /// </summary>
        等待主管审核,

        /// <summary>
        /// 等待质检批准
        /// </summary>
        等待质检批准,

        /// <summary>
        /// 等待SQE处理意见
        /// </summary>
        等待SQE处理意见,

        /// <summary>
        /// 等待仓管确认
        /// </summary>
        等待仓管确认,

        /// <summary>
        /// 已完成
        /// </summary>
        已完成
    }

    /// <summary>
    /// 报废单服务
    /// </summary>
    public interface IScrapBillServer : IBasicService, IBasicBillServer
    {
        /// <summary>
        /// 获得订单号
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <param name="provider">供应商</param>
        /// <returns>返回字符串</returns>
        string GetOrderForm(DepotManagementDataContext ctx, int goodsID, string batchNo, string provider);

        /// <summary>
        /// 获取单据信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>成功返回获取到的单据信息,失败返回null</returns>
        S_ScrapBill GetBill(string billNo);

       /// <summary>
        /// 获取指定时间范围内的所有报废单信息
        /// </summary>
        /// <param name="start">开始时间</param>
        /// <param name="end">终止时间</param>
        /// <returns>返回所有的报废单</returns>
        DataTable GetScrapBill(DateTime start, DateTime end);

        /// <summary>
        /// 获取单据视图信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>成功返回获取到的单据信息,失败返回null</returns>
        View_S_ScrapBill GetBillView(string billNo);

        /// <summary>
        /// 获取所有日期范围内单据信息
        /// </summary>
        /// <param name="startTime">起始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="returnBill">查询到的单据信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取报废信息</returns>
        bool GetAllBill(DateTime startTime, DateTime endTime, out IQueryResult returnBill, out string error);

        /// <summary>
        /// 获取所有单据信息
        /// </summary>
        /// <param name="returnBill">查询到的单据信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取报废信息</returns>
        bool GetAllBill(out IQueryResult returnBill, out string error);

        /// <summary>
        /// 获取所有已经完成并且需要冲抵领料单的单据信息
        /// </summary>
        /// <param name="returnBill">查询到的单据信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取报废信息</returns>
        bool GetAllBillForFetchGoods(out IQueryResult returnBill, out string error);

        /// <summary>
        /// 添加单据
        /// </summary>
        /// <param name="bill">单据信息</param>
        /// <param name="dataContxt">数据上下文</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool AddBill(DepotManagementDataContext dataContxt,S_ScrapBill bill, out string error);

        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="flag">操作标志</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool DeleteBill(string billNo, bool flag, out string error);

        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="dataContxt">LINQ 数据库上下文</param>
        /// <param name="billNo">单据号</param>
        /// <param name="flag">【此参数已作废】是否设置单据为报废状态（不真正删除单据）</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool DeleteBill(DepotManagementDataContext dataContxt, string billNo, bool flag, out string error);

        /// <summary>
        /// 修改单据(仅更新报废类别、原因、是否冲抵领料单)
        /// </summary>
        /// <param name="bill">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool UpdateBill(S_ScrapBill bill,  out string error);

        /// <summary>
        /// 提交单据(交给主管审批)
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加领料单</returns>
        bool SubmitNewBill(string billNo, out string error);

        /// <summary>
        /// 主管审批单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="name">主管姓名</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool DirectorAuthorizeBill(string billNo, string name, out string error);

        /// <summary>
        /// 提交质量信息
        /// </summary>
        /// <param name="billID">单据编号</param>
        /// <param name="qualityInfo">质量信息, 只取其中质量部分</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool SubmitQualityInfo(string billID, S_ScrapBill qualityInfo, out string error);
        
        /// <summary>
        /// 完成单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="storeManager">仓库管理员</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool FinishBill(string billNo, string storeManager, out string error);

        /// <summary>
        /// 回退单据
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <param name="billStatus">单据状态</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <param name="rebackReason">回退原因</param>
        /// <returns>操作是否成功的标志</returns>
        bool ReturnBill(string djh, string billStatus, out string error, string rebackReason);

        /// <summary>
        /// 提交SQE信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="lstGoods">报废物品清单</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool SubmitSQEMessage(string billNo, List<View_S_ScrapGoods> lstGoods, out string error);
    }
}
