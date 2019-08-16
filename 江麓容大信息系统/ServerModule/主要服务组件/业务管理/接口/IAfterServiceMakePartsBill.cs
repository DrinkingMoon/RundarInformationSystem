/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IAfterServiceMakePartsBill.cs
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
using System.Collections.Generic;

namespace ServerModule
{
    /// <summary>
    /// 售后服务配件制造申请单服务接口
    /// </summary>
    public interface IAfterServiceMakePartsBill:IBasicBillServer
    {
        /// <summary>
        /// 获得单据信息
        /// </summary>
        /// <param name="startTime">检索开始时间</param>
        /// <param name="endTime">检索结束时间</param>
        /// <param name="billStatus">单据状态</param>
        /// <returns>成功返回获取到的单据信息，失败返回null</returns>
        System.Data.DataTable GetBill(DateTime startTime, DateTime endTime, string billStatus);

        /// <summary>
        /// 获得单据信息
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <returns>成功返回获取到的单据信息，失败返回null</returns>
        YX_AfterServiceMakePartsBill GetBill(string billID);
        
        /// <summary>
        /// 获得单据明细
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <returns>成功返回活到的单据明细，失败返回NULL</returns>
        System.Data.DataTable GetList(string billID);
       
        /// <summary>
        /// 批量删除单据信息
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回获取到的单据信息，失败返回null</returns>
        bool DeleteBill(List<string> billID, out string error);

        /// <summary>
        /// 更新单据状态
        /// </summary>
        /// <param name="bill">单据主表信息</param>
        /// <param name="listTable">单据明细信息</param>
        /// <param name="billStatus">单据状态</param>
        /// <param name="error">出错时返回的错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        bool UpdateBill(YX_AfterServiceMakePartsBill bill, 
            System.Data.DataTable listTable, AfterServiceMakePartsBillStatus billStatus, 
            out string error);

        /// <summary>
        /// 自动生成领料单
        /// </summary>
        /// <param name="billID">申请单号</param>
        /// <param name="storageID">库房ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回TRUE，失败返回FALES</returns>
        bool AutogenerationMaterialRequisition(string billID, string storageID,
            out string error);
    }
}
