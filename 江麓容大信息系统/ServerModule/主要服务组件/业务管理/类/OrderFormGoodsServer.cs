/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  OrderFormGoodsServer.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2010/09/03
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
using System.Text;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using DBOperate;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using GlobalObject;

namespace ServerModule
{
    /// <summary>
    /// 订单物品信息管理类
    /// </summary>
    class OrderFormGoodsServer : BasicServer, IOrderFormGoodsServer
    {
        /// <summary>
        /// 是否自动将结果提交到数据库的标志
        /// </summary>
        bool m_autoSubmitToDatabase = true;

        /// <summary>
        /// 获取或设置是否自动将结果提交到数据库的标志，默认为true
        /// </summary>
        public bool AutoSubmitToDatabase
        {
            get { return m_autoSubmitToDatabase; }
            set { m_autoSubmitToDatabase = value; }
        }

        /// <summary>
        /// 获取订单物品表中某一产品单价
        /// </summary>
        /// <param name="orderFormNumber">订单物品号</param>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回获取到的单价</returns>
        public decimal GetGoodsUnitPrice(string orderFormNumber, int goodsID)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.B_OrderFormInfo
                         where r.OrderFormNumber == orderFormNumber
                         select r;

            if (result.Count() == 0)
            {
                throw new Exception("找不到订单物品中对应的物品，无法获取物品单价！");
            }

            var varData = from a in dataContxt.B_OrderFormInfo
                          where a.OrderFormNumber == orderFormNumber
                          select a;

            IBargainGoodsServer bargainGoodsServer = ServerModuleFactory.GetServerModule<IBargainGoodsServer>();
            return bargainGoodsServer.GetGoodsUnitPrice(result.Single().BargainNumber, goodsID, varData.Single().Provider);
        }

        /// <summary>
        /// 获取订单物品信息
        /// </summary>
        /// <param name="listRole">角色列表</param>
        /// <param name="loginName">登录名</param>
        /// <param name="returnOrderFormGoods">返回查询到的信息</param>
        /// <param name="error">错误信息, 没有则为null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool GetAllOrderFormGoods(List<string> listRole, string loginName, 
            out IQueryable<View_B_OrderFormGoods> returnOrderFormGoods, out string error)
        {
            returnOrderFormGoods = null;
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                Table<View_B_OrderFormGoods> table = dataContxt.GetTable<View_B_OrderFormGoods>();

                if (listRole.Contains(CE_RoleEnum.采购账务管理员.ToString()) || listRole.Contains(CE_RoleEnum.SQE组长.ToString()) || 
                    listRole.Contains(CE_RoleEnum.采购主管.ToString()) || listRole.Contains(CE_RoleEnum.会计.ToString()))
                {
                    returnOrderFormGoods = from c in table select c;
                }
                else
                {
                    var orderForm = from r in dataContxt.View_B_OrderFormInfo
                                  where r.权限控制用登录名 == loginName
                                  select r.订单号;

                    returnOrderFormGoods = from c in table where orderForm.Contains(c.订单号) select c;
                }

                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 获取指定合同所有的订单物品信息
        /// </summary>
        /// <param name="bargainNumber">合同号</param>
        /// <returns>返回查询到的信息</returns>
        public IQueryable<View_B_OrderFormGoods> GetOrderFormGoods(string bargainNumber)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            return from r in ctx.View_B_OrderFormGoods 
                   join b in ctx.B_OrderFormInfo
                   on r.订单号 equals b.OrderFormNumber
                   where b.BargainNumber == bargainNumber 
                   select r;
        }

        /// <summary>
        /// 获取订单物品信息
        /// </summary>
        /// <param name="listRole">角色列表</param>
        /// <param name="loginID">登录名</param>
        /// <param name="orderFormNumber">订单号</param>
        /// <param name="returnOrderFormGoods">返回查询到的信息</param>
        /// <param name="error">错误信息, 没有则为null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool GetOrderFormGoods(List<string> listRole, string loginID, string orderFormNumber,
            out IQueryable<View_B_OrderFormGoods> returnOrderFormGoods, out string error)
        {
            returnOrderFormGoods = null;
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                Table<View_B_OrderFormGoods> table = dataContxt.GetTable<View_B_OrderFormGoods>();

                returnOrderFormGoods = from c in table
                                       where c.订单号 == orderFormNumber
                                       select c;

                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 添加订单物品信息
        /// </summary>
        /// <param name="listRole">角色列表</param>
        /// <param name="loginID">登录ID</param>
        /// <param name="orderFormGoods">订单物品信息</param>
        /// <param name="returnOrderFormGoods">返回查询到的订单物品信息</param>
        /// <param name="error">错误信息, 没有则为null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool AddOrderFormGoods(List<string> listRole, string loginID, B_OrderFormGoods orderFormGoods, 
            out IQueryable<View_B_OrderFormGoods> returnOrderFormGoods, out string error)
        {
            returnOrderFormGoods = null;
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                dataContxt.B_OrderFormGoods.InsertOnSubmit(orderFormGoods);

                if (AutoSubmitToDatabase)
                {
                    dataContxt.SubmitChanges();
                }

                if (!GetOrderFormGoods(listRole, loginID, orderFormGoods.OrderFormNumber, out returnOrderFormGoods, out error))
                {
                    return false;
                }

                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

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
        public bool UpdateOrderFormGoods(List<string> listRole, string loginName, int id, B_OrderFormGoods newGoods, 
            out IQueryable<View_B_OrderFormGoods> returnOrderFormGoods, out string error)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            return UpdateOrderFormGoods(dataContxt, listRole, loginName, id, newGoods, out returnOrderFormGoods, out error);
        }

        /// <summary>
        /// 修改订单物品信息
        /// </summary>
        /// <param name="dataContext">LINQ 数据库上下文</param>
        /// <param name="listRole">角色列表</param>
        /// <param name="loginName">登录名</param>
        /// <param name="id">订单物品ID</param>
        /// <param name="newGoods">新订单物品</param>
        /// <param name="returnOrderFormGoods">返回查询到的订单物品信息</param>
        /// <param name="error">错误信息, 没有则为null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool UpdateOrderFormGoods(DepotManagementDataContext dataContext, List<string> listRole, string loginName, 
            int id, B_OrderFormGoods newGoods, out IQueryable<View_B_OrderFormGoods> returnOrderFormGoods, out string error)
        {
            returnOrderFormGoods = null;
            error = null;

            try
            {
                var result = from r in dataContext.B_OrderFormGoods
                             where r.ID == id
                             select r;

                if (result.Count() == 0)
                {
                    error = "找不到指定物品信息，无法进行此操作！";
                    return false;
                }

                B_OrderFormGoods record = result.Single();

                record.OrderFormNumber = newGoods.OrderFormNumber;
                record.ArrivalDate = newGoods.ArrivalDate;
                record.GoodsID = newGoods.GoodsID;
                record.Remark = newGoods.Remark;
                record.Amount = newGoods.Amount;

                if (AutoSubmitToDatabase)
                {
                    dataContext.SubmitChanges();
                }

                return GetOrderFormGoods(listRole, loginName, newGoods.OrderFormNumber, out returnOrderFormGoods, out error);
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 修改订单物品信息中的订单号
        /// </summary>
        /// <param name="listRole">角色列表</param>
        /// <param name="loginName">登录名</param>
        /// <param name="id">订单物品ID</param>
        /// <param name="newOrderFormNumber">新订单号</param>
        /// <param name="error">错误信息, 没有则为null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool UpdateOrderFormNumber(List<string> listRole, string loginName, int id, string newOrderFormNumber, out string error)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            return UpdateOrderFormNumber(dataContxt, listRole, loginName, id, newOrderFormNumber, out error);
        }

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
        public bool UpdateOrderFormNumber(DepotManagementDataContext dataContext, List<string> listRole, 
            string loginName, int id, string newOrderFormNumber, out string error)
        {
            try
            {
                error = null;

                var result = from r in dataContext.B_OrderFormGoods
                             where r.ID == id
                             select r;

                if (result.Count() == 0)
                {
                    error = "找不到指定物品信息，无法进行此操作！";
                    return false;
                }

                B_OrderFormGoods record = result.Single();

                record.OrderFormNumber = newOrderFormNumber;

                if (AutoSubmitToDatabase)
                {
                    dataContext.SubmitChanges();
                }

                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 删除订单物品信息
        /// </summary>
        /// <param name="listRole">角色列表</param>
        /// <param name="loginName">登录名</param>
        /// <param name="id">订单物品ID</param>
        /// <param name="returnOrderFormGoods">返回查询到的订单物品信息</param>
        /// <param name="error">错误信息, 没有则为null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool DeleteOrderFormGoods(List<string> listRole, string loginName, int id, 
            out IQueryable<View_B_OrderFormGoods> returnOrderFormGoods, out string error)
        {
            returnOrderFormGoods = null;
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                Table<B_OrderFormGoods> table = dataContxt.GetTable<B_OrderFormGoods>();

                var result = from r in dataContxt.B_OrderFormGoods
                             where r.ID == id
                             select r;

                if (result.Count() == 0)
                {
                    error = "找不到指定物品信息，无法进行此操作！";
                    return false;
                }

                string orderFormNumber = result.Single().OrderFormNumber;

                table.DeleteAllOnSubmit(result);

                if (AutoSubmitToDatabase)
                {
                    dataContxt.SubmitChanges();
                }

                return GetOrderFormGoods(listRole, loginName, orderFormNumber, out returnOrderFormGoods, out error);
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }
    }
}
