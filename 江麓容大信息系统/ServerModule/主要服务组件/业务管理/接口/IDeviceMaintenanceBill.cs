/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IDeviceMaintenanceBill.cs
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
    /// 设备维修安装申请服务接口
    /// </summary>
    public interface IDeviceMaintenanceBill : IBasicBillServer
    {
        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        void DeleteBill(string billNo);

        /// <summary>
        /// 流程管理
        /// </summary>
        /// <param name="deviceMaintenanceBill">LINQ数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool FlowInfo(ServerModule.S_DeviceMaintenanceBill deviceMaintenanceBill, out string error);

        /// <summary>
        /// 获得全部信息
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="billStatus">单据状态</param>
        /// <returns>返回Table</returns>
        System.Data.DataTable GetAllInfo(DateTime startTime, DateTime endTime, string billStatus);
    }
}
