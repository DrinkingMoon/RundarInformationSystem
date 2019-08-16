/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IAfterServicePartsApply.cs
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
using ServerModule;
using System.Data;

namespace Service_Peripheral_External
{
    /// <summary>
    /// 售后配件申请接口
    /// </summary>
    public interface IAfterServicePartsApply : IBasicBillServer
    {
        
        /// <summary>
        /// 获得所有单据信息
        /// </summary>
        /// <param name="billStatus">单据状态</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>返回Table</returns>
        DataTable GetAllBillInfo(string billStatus, DateTime startTime, DateTime endTime);

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="afterService">数据集</param>
        /// <param name="listInfo">明细表</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool InsertBill(Out_AfterServicePartsApplyBill afterService, DataTable listInfo, out string error);
        
        /// <summary>
        /// 审核单据
        /// </summary>
        /// <param name="afterService">数据集</param>
        /// <param name="listInfo">明细表</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool VerifyBill(Out_AfterServicePartsApplyBill afterService, DataTable listInfo, out string error);
        
        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool DeleteBill(string billNo, out string error);

        /// <summary>
        /// 获得一条单据信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回数据集</returns>
        Out_AfterServicePartsApplyBill GetBillInfo(string billNo);

        /// <summary>
        /// 获得某一单据明细信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回Table</returns>
        DataTable GetListInfo(string billNo);
    }
}
