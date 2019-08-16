/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IManeuverServer.cs
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
using ServerModule;

namespace Service_Peripheral_External
{
    /// <summary>
    /// 调运单接口
    /// </summary>
    public interface IManeuverServer : IBasicBillServer
    {

        /// <summary>
        /// 提交申请
        /// </summary>
        /// <param name="maneuverBill">单据信息数据集</param>
        /// <param name="listInfo">明细信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool InsertBill(Out_ManeuverBill maneuverBill, System.Data.DataTable listInfo, out string error);
        
        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool DeleteBill(string billNo, out string error);

        /// <summary>
        /// 获得单条信息
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <returns>返回LINQ数据集</returns>
        System.Data.DataTable GetAllBillInfo(string billStatus, DateTime startTime, DateTime endTime);

        /// <summary>
        /// 获得单条信息
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <returns>返回LINQ数据集</returns>
        ServerModule.Out_ManeuverBill GetBillInfo(string billID);

        /// <summary>
        /// 获得明细信息
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <returns>返回Table</returns>
        System.Data.DataTable GetListInfo(string billID);

        /// <summary>
        /// 操作业务
        /// </summary>
        /// <param name="maneuverBill">单据信息数据集</param>
        /// <param name="listInfo">单据明细信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool OperationInfo(ServerModule.Out_ManeuverBill maneuverBill, System.Data.DataTable listInfo, out string error);
    }
}
