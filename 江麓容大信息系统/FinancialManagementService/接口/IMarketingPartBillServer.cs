using System;
using System.Collections.Generic;
using ServerModule;

namespace Service_Economic_Financial
{
    /// <summary>
    /// 销售清单服务接口
    /// </summary>
    public interface IMarketingPartBillServer
    {
        /// <summary>
        /// 获取或设置查询结果过滤器
        /// </summary>
        string QueryResultFilter { get; set; }

        /// <summary>
        /// 获取所有信息
        /// </summary>
        /// <param name="returnInfo">销售清单信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool GetAllBill(out PlatformManagement.IQueryResult returnInfo, out string error);

        /// <summary>
        /// 通过单据号获取主表
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回数据集</returns>
        System.Data.DataTable GetDataByBillNo(string billNo);

        /// <summary>
        /// 通过单据号获取明细信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回数据集</returns>
        List<View_S_MarketintPartList> GetListDataByBillNo(string billNo);

        /// <summary>
        /// 修改销售清单
        /// </summary>
        /// <param name="marketPartBill">销售清单主表信息</param>
        /// <param name="marketPritList">销售清单子表信息</param>
        /// <param name="role">操作角色</param>
        /// <param name="error">错误信息</param>
        /// <returns>修改成功返回True否则返回False</returns>
        bool UpdateData(S_MarketingPartBill marketPartBill, List<View_S_MarketintPartList> marketPritList, string role, out string error);

        /// <summary>
        /// 打印单据添加打印次数，记录日志
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功反复True，失败返回false</returns>
        bool PrintUpodateData(string billNo, out string error);

        /// <summary>
        /// 回退单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="strBillStatus">单据状态</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <param name="strRebackReason">回退原因</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool ReturnBill(string billNo, string strBillStatus, out string error, string strRebackReason);

        /// <summary>
        /// 通过单据号获得操作日志
        /// </summary>
        /// <param name="BillNo">单据号</param>
        /// <returns>返回操作日志</returns>
        System.Data.DataTable GetSystemLog(string BillNo);

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="startTime">起始时间</param>
        /// <param name="endTime">终止时间</param>
        /// <param name="status">单据状态</param>
        /// <returns>返回数据集</returns>
        List<View_销售清单零件单价查询> GetExcelData(DateTime startTime, DateTime endTime, string status);
    }
}
