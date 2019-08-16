/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IUnProductTestingSingle.cs
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

namespace ServerModule
{
    /// <summary>
    /// 非产品件检验单服务接口
    /// </summary>
    public interface IUnProductTestingSingle : IBasicBillServer
    {
        
        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        void DeleteBill(string billNo);

        /// <summary>
        /// 单据流程
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool FlowBill(string billNo, out string error);

        /// <summary>
        /// 获得所有单据信息
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="billStatus">单据状态</param>
        /// <returns>返回Table</returns>
        System.Data.DataTable GetAllInfo(DateTime startTime, DateTime endTime, string billStatus);

        /// <summary>
        /// 获得检验单据信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回LINQ数据集</returns>
        ServerModule.ZL_UnProductTestingSingleBill GetInfo(string billNo);

        /// <summary>
        /// 获得检验验证记录信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="type">类型</param>
        /// <returns>返回Table</returns>
        System.Data.DataTable GetListInfo(string billNo, string type);

        /// <summary>
        /// 保存检验验证记录
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">单据号</param>
        /// <param name="infoTable">检验验证记录信息集</param>
        /// <param name="type">记录类型：检验，验证</param>
        void SaveDataTableInfo(ServerModule.DepotManagementDataContext ctx, string billNo, System.Data.DataTable infoTable, string type);

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="lnqBill">LINQ数据集</param>
        /// <param name="InspectionTable">检验记录表</param>
        /// <param name="ProvingTable">验证记录表</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool SaveInfo(ServerModule.ZL_UnProductTestingSingleBill lnqBill, System.Data.DataTable InspectionTable, System.Data.DataTable ProvingTable, out string error);
    }
}
