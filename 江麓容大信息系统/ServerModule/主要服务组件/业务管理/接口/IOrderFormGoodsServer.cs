/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IOrderFormGoodsServer.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2010/09/17
 * 开发平台:  Visual C# 2008
 * 用于    :  仓库管理软件
 *----------------------------------------------------------------------------
 * 描述 : 
 * 其它 :
 *----------------------------------------------------------------------------
 * 公共信息: 参见系统'类帮助文档'
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2010/07/17 8:54:12 作者: 夏石友 当前版本: V1.00
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

namespace ServerModule
{
    /// <summary>
    /// 订单物品信息管理类接口
    /// </summary>
    public interface IOrderFormGoodsServer : IBasicService
    {
        /// <summary>
        /// 获取或设置是否自动将结果提交到数据库的标志，默认为true
        /// </summary>
        bool AutoSubmitToDatabase
        {
            get;
            set;
        }

        /// <summary>
        /// 获取订单物品表中某一产品单价
        /// </summary>
        /// <param name="orderFormNumber">订单物品号</param>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回获取到的单价</returns>
        decimal GetGoodsUnitPrice(string orderFormNumber, int goodsID);

        /// <summary>
        /// 获取指定合同所有的订单物品信息
        /// </summary>
        /// <param name="bargainNumber">合同号</param>
        /// <returns>返回查询到的信息</returns>
        IQueryable<View_B_OrderFormGoods> GetOrderFormGoods(string bargainNumber);

        /// <summary>
        /// 获取订单物品信息
        /// </summary>
        /// <param name="listRole">角色列表</param>
        /// <param name="loginName">登录名</param>
        /// <param name="returnOrderFormGoods">返回查询到的信息</param>
        /// <param name="error">错误信息, 没有则为null</param>
        /// <returns>操作是否成功的标志</returns>
        bool GetAllOrderFormGoods(List<string> listRole, string loginName, 
            out IQueryable<View_B_OrderFormGoods> returnOrderFormGoods, out string error);

        /// <summary>
        /// 获取订单物品信息
        /// </summary>
        /// <param name="listRole">角色列表</param>
        /// <param name="loginName">登录名</param>
        /// <param name="orderFormNumber">订单号</param>
        /// <param name="returnOrderFormGoods">返回查询到的信息</param>
        /// <param name="error">错误信息, 没有则为null</param>
        /// <returns>操作是否成功的标志</returns>
        bool GetOrderFormGoods(List<string> listRole, string loginName, string orderFormNumber, out IQueryable<View_B_OrderFormGoods> returnOrderFormGoods, out string error);

        /// <summary>
        /// 添加订单物品信息
        /// </summary>
        /// <param name="listRole">角色列表</param>
        /// <param name="loginID">登录ID</param>
        /// <param name="orderFormInfo">订单物品信息</param>
        /// <param name="returnOrderFormGoods">返回查询到的订单物品信息</param>
        /// <param name="error">错误信息, 没有则为null</param>
        /// <returns>操作是否成功的标志</returns>
        bool AddOrderFormGoods(List<string> listRole, string loginID, B_OrderFormGoods orderFormInfo, out IQueryable<View_B_OrderFormGoods> returnOrderFormGoods, out string error);

        /// <summary>
        /// 修改订单物品信息
        /// </summary>
        /// <param name="listRole">角色列表</param>
        /// <param name="loginName">登录名</param>
        /// <param name="id">订单物品ID</param>
        /// <param name="newGoods">新订单物品</param>
        /// <param name="returnOrderFormGoods">返回查询到的订单物品信息</param>
        /// <param name="error">错误信息, 没有则为null</param>
        /// <returns>操作是否成功的标志</returns>
        bool UpdateOrderFormGoods(List<string> listRole, string loginName, int id, B_OrderFormGoods newGoods, out IQueryable<View_B_OrderFormGoods> returnOrderFormGoods, out string error);

        /// <summary>
        /// 修改订单物品信息
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="listRole">角色列表</param>
        /// <param name="loginName">登录名</param>
        /// <param name="id">订单物品ID</param>
        /// <param name="newGoods">新订单物品</param>
        /// <param name="returnOrderFormGoods">返回查询到的订单物品信息</param>
        /// <param name="error">错误信息, 没有则为null</param>
        /// <returns>操作是否成功的标志</returns>
        bool UpdateOrderFormGoods(DepotManagementDataContext dataContext, List<string> listRole,
            string loginName, int id, B_OrderFormGoods newGoods, 
            out IQueryable<View_B_OrderFormGoods> returnOrderFormGoods, out string error);

        /// <summary>
        /// 修改订单物品信息中的订单号
        /// </summary>
        /// <param name="listRole">角色列表</param>
        /// <param name="loginName">登录名</param>
        /// <param name="id">订单物品ID</param>
        /// <param name="newOrderFormNumber">新订单号</param>
        /// <param name="error">错误信息, 没有则为null</param>
        /// <returns>操作是否成功的标志</returns>
        bool UpdateOrderFormNumber(List<string> listRole, string loginName, int id, string newOrderFormNumber, out string error);
        
        /// <summary>
        /// 修改订单物品信息中的订单号
        /// </summary>
        /// <param name="dataContext">数据库上下文</param>
        /// <param name="listRole">角色列表</param>
        /// <param name="loginName">登录名</param>
        /// <param name="id">订单物品ID</param>
        /// <param name="newOrderFormNumber">新订单号</param>
        /// <param name="error">错误信息, 没有则为null</param>
        /// <returns>操作是否成功的标志</returns>
        bool UpdateOrderFormNumber(DepotManagementDataContext dataContext, List<string> listRole, string loginName, int id, string newOrderFormNumber, out string error);

        /// <summary>
        /// 删除订单物品信息
        /// </summary>
        /// <param name="listRole">角色列表</param>
        /// <param name="loginName">登录名</param>
        /// <param name="id">订单物品ID</param>
        /// <param name="returnOrderFormGoods">返回查询到的订单物品信息</param>
        /// <param name="error">错误信息, 没有则为null</param>
        /// <returns>操作是否成功的标志</returns>
        bool DeleteOrderFormGoods(List<string> listRole, string loginName, int id, out IQueryable<View_B_OrderFormGoods> returnOrderFormGoods, out string error);
    }
}
