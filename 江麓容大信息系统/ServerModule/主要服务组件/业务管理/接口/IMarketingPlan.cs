/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IMarketingPlan.cs
 * 作者    :  曹津彬    版本: v1.00    日期: 2014/01/22
 * 开发平台:  Visual C# 2008
 * 用于    :  仓库管理软件
 *----------------------------------------------------------------------------
 * 描述 : 
 * 其它 :
 *----------------------------------------------------------------------------
 * 公共信息: 参见系统'类帮助文档'
 *----------------------------------------------------------------------------
 * 历史记录:
 ******************************************************************************/
using System;
using System.Data;

namespace ServerModule
{
    /// <summary>
    /// 营销计划管理类接口
    /// </summary>
    public interface IMarketingPlan:IBasicBillServer
    {
        /// <summary>
        /// 上传附件
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="fileNo">附件编号</param>
        void UpdateFilePath(string billNo, string fileNo);

        /// <summary>
        /// 添加父表信息
        /// </summary>
        /// <param name="marketingPlan">计划主表信息</param>
        /// <param name="planList">计划明细表信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>添加成功返回True，添加失败返回False</returns>
        bool AddBill(S_MarketingPlanBill marketingPlan, DataTable planList, out string error);

        /// <summary>
        /// 检查单据是否重复
        /// </summary>
        /// <param name="djh">计划单号</param>
        /// <returns>未重复返回True,重复返回False</returns>
        bool IsRepeatBillID(string djh);

        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="djh">计划单号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        bool DeleteBill(string djh, out string error);

        /// <summary>
        /// 获得父表信息
        /// </summary>
        /// <returns>返回获得的营销计划主表信息</returns>
        DataTable GetAllBill();

        /// <summary>
        /// 获得子表信息
        /// </summary>
        /// <param name="djh">营销计划单号</param>
        /// <returns>返回获得的营销计划明细信息</returns>
        DataTable GetList(string djh);

        /// <summary>
        /// 获得计划年月
        /// </summary>
        /// <returns>返回计划年月</returns>
        string GetYearAndMonth();

        /// <summary>
        /// 更改状态
        /// </summary>
        /// <param name="billStatus">单据状态</param>
        /// <param name="billIDList">计划单号列表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>更改成功返回True，更改失败返回False</returns>
        bool UpdateBill(string billStatus, DataTable billIDList, out string error);

        /// <summary>
        /// 插入交货期数据
        /// </summary>
        /// <param name="marketingPlanDelivery">交货期数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool AddDelivery(S_MarketingPlanDelivery marketingPlanDelivery, out string error);
        
        /// <summary>
        /// 删除营销计划交货期
        /// </summary>
        /// <param name="marketingPlanDelivery">交货期数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool DeleteDelivery(S_MarketingPlanDelivery marketingPlanDelivery, out string error);

        /// <summary>
        /// 获得交货期数据集
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="month">计划月</param>
        /// <returns>返回Table</returns>
        DataTable GetPlanDeliveryInfo(string billID, int goodsID, int month);

        /// <summary>
        /// 获得ToolTip字符串
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="month">计划月</param>
        /// <returns>返回字符串</returns>
        string GetCellToolTipString(string billID, int goodsID, int month);
    }
}
