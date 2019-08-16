/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  ICheckOutInDepotForOutsourcingServer.cs
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
    /// 委外报检入库单管理类接口
    /// </summary>
    public interface ICheckOutInDepotForOutsourcingServer : IBasicBillServer, IBasicService
    {

        /// <summary>
        /// 有检测废的物品直接生成领料单
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="inDepotInfo">报检单信息</param>
        /// <param name="mrBillNo">分配的领料单单号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>生成成功返回True，生成失败返回False</returns>
        bool InsertIntoMaterialRequisition(DepotManagementDataContext ctx, S_CheckOutInDepotForOutsourcingBill inDepotInfo,
            out string mrBillNo, out string error);

        /// <summary>
        /// 插入报废单
        /// </summary>
        /// <param name="ctx">LINQ数据上下文</param>
        /// <param name="outSourcing">Linq操作数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>插入成功返回True，插入失败返回False</returns>
        bool AddScrapBill(DepotManagementDataContext ctx,
            S_CheckOutInDepotForOutsourcingBill outSourcing, out string error);

        /// <summary>
        /// 根据单据信息操作账务信息与库存信息
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="bill">单据信息</param>
        void OpertaionDetailAndStock(DepotManagementDataContext dataContext, S_CheckOutInDepotForOutsourcingBill bill);

        /// <summary>
        /// 查询显示的数据
        /// </summary>
        /// <param name="returnBill">返回 IQueryResult 数据集 </param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>查询成功返回True，查询失败返回False</returns>
        bool GetBill(out IQueryResult returnBill, out string error);

        /// <summary>
        /// 插入信息
        /// </summary>
        /// <param name="outSourcing">Linq数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>插入成功返回True，插入失败返回False</returns>
        bool AddBill(S_CheckOutInDepotForOutsourcingBill outSourcing, out string error);

        /// <summary>
        /// 更新单据状态
        /// </summary>
        /// <param name="outSourcing">Linq操作数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>更新成功返回True，更新失败返回False</returns>
        bool UpdateBill(S_CheckOutInDepotForOutsourcingBill outSourcing, out string error);

        /// <summary>
        /// 获得一条记录
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回LINQ S_CheckOutInDepotForOutsourcingBill 视图</returns>
        S_CheckOutInDepotForOutsourcingBill GetBill(string billNo);

        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        bool DeleteBill(string billNo, out string error);

        /// <summary>
        /// 回退单据
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <param name="billStatus">单据状态</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <param name="rebackReason">回退原因</param>
        /// <returns>回退成功返回True，回退失败返回False</returns>
        bool ReturnBill(string djh, string billStatus,
            out string error, string rebackReason);
    }
}
