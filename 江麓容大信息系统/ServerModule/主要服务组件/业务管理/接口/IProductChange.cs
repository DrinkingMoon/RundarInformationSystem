/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IProductChange.cs
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
    /// 产品型号变更管理类接口
    /// </summary>
    public interface IProductChange:IBasicBillServer
    {
        /// <summary>
        /// 添加父表信息
        /// </summary>
        /// <param name="inProductChange">产品型号变更信息</param>
        /// <param name="planList">明细信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>添加成功返回True，添加失败返回False</returns>
        bool AddBill(P_ProductChangeBill inProductChange, DataTable planList, out string error);

        /// <summary>
        /// 检查单据是否重复
        /// </summary>
        /// <param name="djh">变更单号</param>
        /// <returns>未重复返回True，重复返回False</returns>
        bool IsRepeatBillID(string djh);

        /// <summary>
        /// 获得父表信息
        /// </summary>
        /// <returns>返回产品型号变更信息</returns>
        DataTable GetAllBill();

        /// <summary>
        /// 获得子表信息
        /// </summary>
        /// <param name="djh">变更单号</param>
        /// <returns>返回明细表信息</returns>
        DataTable GetList(string djh);

        /// <summary>
        /// 修改父表信息
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="inProductChange">产品型号变更信息</param>
        /// <param name="planList">明细信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>修改成功返回True，修改失败返回False</returns>
        bool UpdateBill(DepotManagementDataContext ctx, P_ProductChangeBill inProductChange,
            DataTable planList, out string error);

        /// <summary>
        /// 删除表信息
        /// </summary>
        /// <param name="djh">变更单号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        bool DeleteBill(string djh, out string error);

        /// <summary>
        /// 修改父表信息，质管批准
        /// </summary>
        /// <param name="djh">变更单号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>修改成功返回True，修改失败返回False</returns>
        bool AuthorizeBill(string djh, out string error);

        /// <summary>
        /// 修改父表信息，主管审核
        /// </summary>
        /// <param name="djh">变更单号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>修改成功返回True，修改失败返回False</returns>
        bool AuditingBill(string djh, out string error);
    }
}
