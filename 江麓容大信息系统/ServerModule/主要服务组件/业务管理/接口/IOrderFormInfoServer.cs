/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IOrderFormInfoServer.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2010/09/15
 * 开发平台:  Visual C# 2008
 * 用于    :  仓库管理软件
 *----------------------------------------------------------------------------
 * 描述 : 
 * 其它 :
 *----------------------------------------------------------------------------
 * 公共信息: 参见系统'类帮助文档'
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2010/09/15 作者: 夏石友 当前版本: V1.00
 *        修改说明: 创建
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using GlobalObject;

namespace ServerModule
{
    /// <summary>
    /// 订单信息管理类接口
    /// </summary>
    public interface IOrderFormInfoServer
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        string GetOrderNo(string provider);

        /// <summary>
        /// 获取订单信息
        /// </summary>
        /// <param name="listRole">角色列表</param>
        /// <param name="loginName">登录名</param>
        /// <param name="billType">单据类型</param>
        /// <param name="returnOrderFormInfo">返回查询到的订单信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool GetAllOrderFormInfo(List<string> listRole, string loginName, CE_BillTypeEnum billType, out IQueryable<View_B_OrderFormInfo> returnOrderFormInfo,
            out string error);

        /// <summary>
        /// 查找订单中 是否有此合同记录
        /// </summary>
        /// <param name="bargainNumber">合同号</param>
        /// <returns>True 存在 False 不存在</returns>
        bool FindBargainCount(string bargainNumber);

        /// <summary>
        /// 获取订单类型信息
        /// </summary>
        /// <returns>返回获取到的订单类型信息</returns>
        List<B_OrderFormType> GetOrderFormType();

        /// <summary>
        /// 获取指定订单的订货员编码
        /// </summary>
        /// <param name="orderFormNumber">订单号</param>
        /// <returns>返回获取到的订货员编码</returns>
        string GetBuyerCode(string orderFormNumber);

        /// <summary>
        /// 获取指定的订单信息
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="orderFormNumber">订单号</param>
        /// <returns>成功返回获取到的订单信息, 失败返回null</returns>
        View_B_OrderFormInfo GetOrderFormInfo(DepotManagementDataContext ctx, string orderFormNumber);

        /// <summary>
        /// 获取指定的订单信息
        /// </summary>
        /// <param name="orderFormNumber">订单号</param>
        /// <returns>成功返回获取到的订单信息, 失败返回null</returns>
        View_B_OrderFormInfo GetOrderFormInfo(string orderFormNumber);

        /// <summary>
        /// 获取指定合同的订单信息集
        /// </summary>
        /// <param name="bargainNumber">合同号</param>
        /// <returns>成功返回获取到的订单信息, 失败返回null</returns>
        IQueryable<View_B_OrderFormInfo> GetOrderFormCollection(string bargainNumber);

        /// <summary>
        /// 获取订单综合查询结果
        /// </summary>
        /// <param name="listRole">用户角色列表</param>
        /// <param name="loginName">用户登录名</param>
        /// <returns>合同综合查询结果</returns>
        IQueryable<View_B_OrderFormAnalyzer> GetOrderFormAnalyzer(List<string> listRole, string loginName);

        /// <summary>
        /// 获取订单信息
        /// </summary>
        /// <param name="listRole">角色列表</param>
        /// <param name="loginName">登录名</param>
        /// <param name="returnOrderFormInfo">返回查询到的订单信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool GetAllOrderFormInfo(List<string> listRole, string loginName, out IQueryable<View_B_OrderFormInfo> returnOrderFormInfo, out string error);

        /// <summary>
        /// 添加订单信息
        /// </summary>
        /// <param name="listRole">角色列表</param>
        /// <param name="loginName">登录名</param>
        /// <param name="orderFormInfo">订单信息</param>
        /// <param name="returnOrderFormInfo">返回查询到的订单信息</param>
        /// <param name="error">错误信息, 没有则为null</param>
        /// <returns>操作是否成功的标志</returns>
        bool AddOrderFormInfo(List<string> listRole, string loginName, B_OrderFormInfo orderFormInfo, 
            out IQueryable<View_B_OrderFormInfo> returnOrderFormInfo, out string error);

        /// <summary>
        /// 修改订单合同号(由合同信息表更新时级联调用此方法进行更新, 此方法不提交改变而由调用者提交)
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="oldNumber">旧合同号</param>
        /// <param name="newNumber">新合同号</param>
        void UpdateBargainNumber(DepotManagementDataContext dataContext, string oldNumber, string newNumber);

        /// <summary>
        /// 修改订单信息
        /// </summary>
        /// <param name="listRole">角色列表</param>
        /// <param name="loginName">登录名</param>
        /// <param name="oldOrderFormNumber">旧订单号</param>
        /// <param name="orderFormInfo">订单信息</param>
        /// <param name="returnOrderFormInfo">返回查询到的订单信息</param>
        /// <param name="error">错误信息, 没有则为null</param>
        /// <returns>操作是否成功的标志</returns>
        bool UpdateOrderFormInfo(List<string> listRole, string loginName, string oldOrderFormNumber, B_OrderFormInfo orderFormInfo, out IQueryable<View_B_OrderFormInfo> returnOrderFormInfo, out string error);

        /// <summary>
        /// 删除订单信息
        /// </summary>
        /// <param name="listRole">角色列表</param>
        /// <param name="loginName">登录名</param>
        /// <param name="orderFormNumber">订单编号</param>
        /// <param name="returnOrderFormInfo">返回查询到的订单信息</param>
        /// <param name="error">错误信息, 没有则为null</param>
        /// <returns>操作是否成功的标志</returns>
        bool DeleteOrderFormInfo(List<string> listRole, string loginName, string orderFormNumber, out IQueryable<View_B_OrderFormInfo> returnOrderFormInfo, out string error);

        /// <summary>
        /// 查找普通入库单或报检入库单中是否有此订单号记录并且入库单中包含指定参数的物品
        /// </summary>
        /// <param name="orderBillID">订单号</param>
        /// <param name="goodsCode">要查找物品的图号</param>
        /// <param name="goodsName">要查找物品的名称</param>
        /// <param name="spec">规格</param>
        /// <returns>查找到返回true，否则返回false</returns>
        bool FindInDepotOrderNumberCount(string orderBillID, string goodsCode, string goodsName, string spec);
        
        /// <summary>
        /// 查找未报废的普通入库单或未报废的报检入库单中是否有此订单记录
        /// </summary>
        /// <param name="orderBillID">订单号</param>
        /// <returns>查找到返回true，否则返回false</returns>
        bool FindInDepotOrderGoodsCount(string orderBillID);
 
        /// <summary>
        /// 审核订单
        /// </summary>
        /// <param name="auditingPersonnel">审核人工号</param>
        /// <param name="orderFormNumber">审核订单号</param>
        /// <param name="error">错误信息, 没有则为null</param>
        /// <returns>操作是否成功的标志</returns>
        bool Auditing(string auditingPersonnel, string orderFormNumber, out string error);

        /// <summary>
        /// 检查订单是否已审核
        /// </summary>
        /// <param name="orderFormNumber">订单号</param>
        /// <returns>审核了返回true，否则返回false</returns>
        bool CheckDate(string orderFormNumber);
        

        /// <summary>
        /// 获得未到货订单列表
        /// </summary>
        /// <param name="startTime">起始时间</param>
        /// <param name="endTime">截止时间</param>
        /// <param name="status">状态</param>
        /// <param name="provider">供应商</param>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回Table</returns>
        DataTable GetGoodsAfloatOrderForm(DateTime? startTime, DateTime? endTime, string status, string provider, int goodsID);

        /// <summary>
        /// 变更订单状态
        /// </summary>
        /// <param name="orderFormNumber">订单号</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="status">变更状态</param>
        void UpdateOrderFormCloseStatus(string orderFormNumber, int goodsID, bool status);
    }
}
