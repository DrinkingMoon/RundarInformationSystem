/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  ICannibalize.cs
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
using System.Collections.Generic;
using System.Data;

namespace Service_Manufacture_WorkShop
{
    /// <summary>
    /// 车间调运单服务接口
    /// </summary>
    public interface ICannibalize : IBasicBillServer
    {
        /// <summary>
        /// 回退单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="billStatus">回退后的单据状态</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <param name="rebackReason">回退原因</param>
        /// <returns>回退成功返回True，回退失败返回False</returns>
        bool ReturnBill(string billNo, CannibalizeBillStatus billStatus, out string error, string rebackReason);

        /// <summary>
        /// 获得操作库房信息列表
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回列表信息</returns>
        List<WS_CannibalizeWSCode> GetOperationWSCode(string billNo);

        /// <summary>
        /// 确认单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="list">明细信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        bool AffirmBill(string billNo, System.Data.DataTable list, out string error);

        /// <summary>
        /// 审核单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        bool AuditBill(string billNo, out string error);

        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        bool DeleteBill(string billNo, out string error);

        /// <summary>
        /// 获得单据信息
        /// </summary>
        /// <returns>返回单据信息</returns>
        System.Data.DataTable GetBillInfo();

        /// <summary>
        /// 获得单条单据信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回单据信息</returns>
        ServerModule.WS_CannibalizeBill GetBillSingle(string billNo);

        /// <summary>
        /// 获得单据明细
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>成功返回True,失败返回False</returns>
        System.Data.DataTable GetListInfo(string billNo);

        /// <summary>
        /// 申请单据
        /// </summary>
        /// <param name="bill">单据信息</param>
        /// <param name="listWSCode">操作车间列表信息</param>
        /// <param name="list">明细信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        bool ProposeBill(WS_CannibalizeBill bill, List<WS_CannibalizeWSCode> listWSCode, DataTable list, out string error);
    }
}
