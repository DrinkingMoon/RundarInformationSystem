/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  OrderFormInfoServer.cs
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
    /// 订单信息管理类
    /// </summary>
    class OrderFormInfoServer : BasicServer, IOrderFormInfoServer
    {
        public string GetOrderNo(string provider)
        {
            string temp1 = "PLN" + ServerTime.Time.Year.ToString("D4") 
                + ServerTime.Time.Month.ToString("D2") 
                + ServerTime.Time.Day.ToString("D2");

            string strSql = "select Top 1 * from (select SUBSTRING(OrderFormNumber,12,3) as OrderNo from B_OrderFormInfo "
                + " where OrderFormNumber like '" + temp1 + "%' and OrderFormNumber like '%" + provider + "') as a order by OrderNo desc ";

            DataTable tempTable = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (tempTable.Rows.Count == 0)
            {
                return temp1 + "001" + "_" + provider;
            }
            else
            {
                string temp2 = (Convert.ToInt32(tempTable.Rows[0][0]) + 1).ToString("D3");

                return temp1 + temp2 + "_" + provider;
            }
        }

        /// <summary>
        /// 查找订单中 是否有此合同记录
        /// </summary>
        /// <param name="bargainNumber">合同号</param>
        /// <returns>True 存在 False 不存在</returns>
        public bool FindBargainCount(string bargainNumber)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var varOrderFormInfo = from a in dataContxt.B_OrderFormInfo
                            where a.BargainNumber == bargainNumber
                            select a;

            if (varOrderFormInfo.Count() != 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 获取订单类型信息
        /// </summary>
        /// <returns>返回获取到的订单类型信息</returns>
        public List<B_OrderFormType> GetOrderFormType()
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            return (from r in dataContxt.B_OrderFormType 
                    select r).ToList();
        }

        /// <summary>
        /// 获取指定订单的订货员编码
        /// </summary>
        /// <param name="orderFormNumber">订单号</param>
        /// <returns>返回获取到的订货员编码</returns>
        public string GetBuyerCode(string orderFormNumber)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.B_OrderFormInfo 
                         where r.OrderFormNumber == orderFormNumber 
                         select r.Buyer;

            return result.First();
        }

        /// <summary>
        /// 获取指定的订单信息
        /// </summary>
        /// <param name="orderFormNumber">订单号</param>
        /// <returns>成功返回获取到的订单信息, 失败返回null</returns>
        public View_B_OrderFormInfo GetOrderFormInfo(string orderFormNumber)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            return GetOrderFormInfo(dataContxt, orderFormNumber);
        }

        /// <summary>
        /// 获取指定的订单信息
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="orderFormNumber">订单号</param>
        /// <returns>成功返回获取到的订单信息, 失败返回null</returns>
        public View_B_OrderFormInfo GetOrderFormInfo(DepotManagementDataContext ctx, string orderFormNumber)
        {
            var result = from r in ctx.View_B_OrderFormInfo 
                         where r.订单号 == orderFormNumber 
                         select r;

            if (result.Count() > 0)
            {
                return result.Single();
            }

            return null;
        }

        /// <summary>
        /// 获取指定合同的订单信息集
        /// </summary>
        /// <param name="bargainNumber">合同号</param>
        /// <returns>成功返回获取到的订单信息, 失败返回null</returns>
        public IQueryable<View_B_OrderFormInfo> GetOrderFormCollection(string bargainNumber)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.View_B_OrderFormInfo 
                         where r.合同号 == bargainNumber 
                         select r;

            return result;
        }

        /// <summary>
        /// 获取订单信息
        /// </summary>
        /// <param name="listRole">角色列表</param>
        /// <param name="loginName">登录名</param>
        /// <param name="billType">单据类型</param>
        /// <param name="returnOrderFormInfo">返回查询到的订单信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool GetAllOrderFormInfo(List<string> listRole, string loginName, CE_BillTypeEnum billType, out IQueryable<View_B_OrderFormInfo> returnOrderFormInfo,
            out string error)
        {
            returnOrderFormInfo = null;
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                var orderFormInfo = from a in dataContxt.View_B_OrderFormInfo
                                    select a;

                if (listRole.Contains(CE_RoleEnum.采购账务管理员.ToString())
                    || listRole.Contains(CE_RoleEnum.采购主管.ToString()))
                {
                    returnOrderFormInfo = from c in orderFormInfo orderby c.订货日期 descending select c;
                    return true;
                }

                switch (billType)
                {
                    case CE_BillTypeEnum.样品确认申请单:
                        orderFormInfo = (from a in orderFormInfo
                                         join b in dataContxt.ProviderPrincipal on a.供货单位 equals b.Provider
                                         where b.PrincipalWorkId == loginName
                                         select a).Distinct();
                        break;
                    case CE_BillTypeEnum.报检入库单:

                        orderFormInfo = (from a in orderFormInfo
                                         join b in dataContxt.ProviderPrincipal on a.供货单位 equals b.Provider
                                         where b.PrincipalWorkId == loginName
                                         select a).Distinct();

                        break;
                    case CE_BillTypeEnum.委外报检入库单:

                        orderFormInfo = (from a in orderFormInfo
                                         join b in dataContxt.ProviderPrincipal on a.供货单位 equals b.Provider
                                         where b.PrincipalWorkId == loginName
                                         select a).Distinct();
                        break;
                    case CE_BillTypeEnum.普通入库单:
                        break;
                    default:
                        break;
                }

                returnOrderFormInfo = orderFormInfo
                    .OrderByDescending(k => k.订货日期)
                    .ThenByDescending(k => k.订单号).AsQueryable<View_B_OrderFormInfo>();
                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 获取订单信息
        /// </summary>
        /// <param name="listRole">角色列表</param>
        /// <param name="loginName">登录名</param>
        /// <param name="returnOrderFormInfo">返回查询到的订单信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool GetAllOrderFormInfo(List<string> listRole, string loginName, out IQueryable<View_B_OrderFormInfo> returnOrderFormInfo, 
            out string error)
        {
            returnOrderFormInfo = null;                
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                Table<View_B_OrderFormInfo> table = dataContxt.GetTable<View_B_OrderFormInfo>();

                var orderFormInfo = (from a in table 
                                     join b in dataContxt.ProviderPrincipal on a.供货单位 equals b.Provider
                                     where b.PrincipalWorkId == loginName
                                     select a).Distinct().OrderByDescending(r => r.订货日期);
                returnOrderFormInfo = orderFormInfo.AsQueryable<View_B_OrderFormInfo>();
                //returnOrderFormInfo = from c in table where c.权限控制用登录名 == loginName select c;

                if (returnOrderFormInfo.Count() == 0 
                    || listRole.Contains(CE_RoleEnum.采购账务管理员.ToString()) 
                    || listRole.Contains(CE_RoleEnum.采购主管.ToString()))
                {
                    returnOrderFormInfo = (from c in table select c).OrderByDescending(r => r.订货日期);
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
        /// 获取订单综合查询结果
        /// </summary>
        /// <param name="listRole">用户角色列表</param>
        /// <param name="loginName">用户登录名</param>
        /// <returns>合同综合查询结果</returns>
        public IQueryable<View_B_OrderFormAnalyzer> GetOrderFormAnalyzer(List<string> listRole, string loginName)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;
            Table<View_B_OrderFormAnalyzer> table = dataContxt.GetTable<View_B_OrderFormAnalyzer>();

            if (listRole.Contains(CE_RoleEnum.业务系统管理员.ToString()) || listRole.Contains(CE_RoleEnum.采购账务管理员.ToString())
                || listRole.Contains(CE_RoleEnum.采购主管.ToString()) || listRole.Contains(CE_RoleEnum.会计.ToString()))
            {
                return from c in table select c;
            }
            else
            {
                return from c in table where c.权限控制用登录名 == loginName select c;
            }
        }

        /// <summary>
        /// 添加订单信息
        /// </summary>
        /// <param name="listRole">角色列表</param>
        /// <param name="loginName">登录名</param>
        /// <param name="orderFormInfo">订单信息</param>
        /// <param name="returnOrderFormInfo">返回查询到的订单信息</param>
        /// <param name="error">错误信息, 没有则为null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool AddOrderFormInfo(List<string> listRole, string loginName, B_OrderFormInfo orderFormInfo, 
            out IQueryable<View_B_OrderFormInfo> returnOrderFormInfo, out string error)
        {
            returnOrderFormInfo = null;
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                dataContxt.B_OrderFormInfo.InsertOnSubmit(orderFormInfo);
                dataContxt.SubmitChanges();

                return GetAllOrderFormInfo(listRole, loginName, out returnOrderFormInfo, out error);
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 修改订单合同号(由合同信息表更新时级联调用此方法进行更新, 此方法不提交改变而由调用者提交)
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="oldNumber">旧合同号</param>
        /// <param name="newNumber">新合同号</param>
        public void UpdateBargainNumber(DepotManagementDataContext dataContext, string oldNumber, string newNumber)
        {
            var result = from r in dataContext.B_OrderFormInfo 
                         where r.BargainNumber == oldNumber 
                         select r;

            foreach (var item in result)
            {
                item.BargainNumber = newNumber;
            }
        }

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
        public bool UpdateOrderFormInfo(List<string> listRole, string loginName, string oldOrderFormNumber,
            B_OrderFormInfo orderFormInfo, out IQueryable<View_B_OrderFormInfo> returnOrderFormInfo, out string error)
        {
            returnOrderFormInfo = null;
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                var result = from r in dataContxt.B_OrderFormInfo where r.OrderFormNumber == oldOrderFormNumber select r;

                if (result.Count() == 0)
                {
                    error = string.Format("找不到订单号为 [{0}] 的信息！", oldOrderFormNumber);
                    return false;
                }

                B_OrderFormInfo record = result.Single();

                IOrderFormGoodsServer goodsServer = ServerModuleFactory.GetServerModule<IOrderFormGoodsServer>();

                if (oldOrderFormNumber != orderFormInfo.OrderFormNumber)
                {
                    IQueryable<View_B_OrderFormGoods> returnOrderFormGoods;

                    if (!goodsServer.GetOrderFormGoods(listRole, loginName, oldOrderFormNumber,
                        out returnOrderFormGoods, out error))
                    {
                        return false;
                    }

                    goodsServer.AutoSubmitToDatabase = false;

                    foreach (var item in returnOrderFormGoods)
                    {
                        if (!goodsServer.UpdateOrderFormNumber(dataContxt, listRole, loginName, item.序号,
                            orderFormInfo.OrderFormNumber, out error))
                        {
                            return false;
                        }
                    }

                    dataContxt.B_OrderFormInfo.DeleteOnSubmit(record);
                    record = new B_OrderFormInfo();
                }

                record.OrderFormNumber = orderFormInfo.OrderFormNumber;
                record.Provider = orderFormInfo.Provider;
                record.Buyer = orderFormInfo.Buyer;
                record.BargainNumber = orderFormInfo.BargainNumber;
                record.TypeID = orderFormInfo.TypeID;
                record.InputPerson = orderFormInfo.InputPerson;
                record.ProviderLinkman = orderFormInfo.ProviderLinkman;
                record.ProviderEmail = orderFormInfo.ProviderEmail;
                record.ProviderPhone = orderFormInfo.ProviderPhone;
                record.ProviderFax = orderFormInfo.ProviderFax;
                record.Remark = orderFormInfo.Remark;
                record.TypeID = orderFormInfo.TypeID;
                record.CreateDate = orderFormInfo.CreateDate;

                if (oldOrderFormNumber != orderFormInfo.OrderFormNumber)
                {
                    dataContxt.B_OrderFormInfo.InsertOnSubmit(record);
                }

                dataContxt.SubmitChanges();

                goodsServer.AutoSubmitToDatabase = true;

                return GetAllOrderFormInfo(listRole, loginName, out returnOrderFormInfo, out error);
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 删除订单信息
        /// </summary>
        /// <param name="listRole">角色列表</param>
        /// <param name="loginName">登录名</param>
        /// <param name="orderFormNumber">订单编号</param>
        /// <param name="returnOrderFormInfo">返回查询到的订单信息</param>
        /// <param name="error">错误信息, 没有则为null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool DeleteOrderFormInfo(List<string> listRole, string loginName, string orderFormNumber, 
            out IQueryable<View_B_OrderFormInfo> returnOrderFormInfo, out string error)
        {
            error = null;
            returnOrderFormInfo = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                IOrderFormGoodsServer goodsServer = ServerModuleFactory.GetServerModule<IOrderFormGoodsServer>();
                IQueryable<View_B_OrderFormGoods> returnOrderFormGoods;

                if (!goodsServer.GetOrderFormGoods(listRole, loginName, orderFormNumber, out returnOrderFormGoods, out error))
                {
                    return false;
                }

                if (returnOrderFormGoods != null && returnOrderFormGoods.Count() > 0)
                {
                    error = string.Format("订单 [{0}] 还包含有物品信息无法进行删除，请将所有此订单包含的物品信息全部删除后才能进行此操作！", orderFormNumber);
                    return false;
                }

                Table<B_OrderFormInfo> table = dataContxt.GetTable<B_OrderFormInfo>();
                var delRow = from c in table where c.OrderFormNumber == orderFormNumber select c;

                table.DeleteAllOnSubmit(delRow);
                dataContxt.SubmitChanges();

                return GetAllOrderFormInfo(listRole, loginName, out returnOrderFormInfo, out error);
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 查找普通入库单或报检入库单中是否有此订单号记录并且入库单中包含指定参数的物品
        /// </summary>
        /// <param name="orderBillID">订单号</param>
        /// <param name="goodsCode">要查找物品的图号</param>
        /// <param name="goodsName">要查找物品的名称</param>
        /// <param name="spec">规格</param>
        /// <returns>查找到返回true，否则返回false</returns>
        public bool FindInDepotOrderNumberCount(string orderBillID, string goodsCode, string goodsName, string spec)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var varOrdinaryBill = (from a in dataContxt.S_OrdinaryInDepotBill
                             where a.OrderBill_ID == orderBillID
                             select a).ToArray();

            if (varOrdinaryBill.Count() != 0)
            {
                for (int i = 0; i < varOrdinaryBill.Length; i++)
                {
                    var data = from a in dataContxt.View_S_OrdinaryInDepotGoodsBill
                               where a.入库单号 == varOrdinaryBill[i].Bill_ID
                                   && a.图号型号 == goodsCode && a.物品名称 == goodsName
                                   && a.规格 == spec
                               select a;

                    if (data.Count() != 0)
                    {
                        return true;
                    }
                }
            }

            var varCheckOutInDepotBill = (from a in dataContxt.S_CheckOutInDepotBill
                             where a.OrderFormNumber == orderBillID
                             select a).ToArray();

            if (varCheckOutInDepotBill.Count() != 0)
            {
                for (int i = 0; i < varCheckOutInDepotBill.Length; i++)
                {
                    var data = from a in dataContxt.View_S_CheckOutInDepotBill
                               where a.入库单号 == varCheckOutInDepotBill[i].Bill_ID
                                   && a.图号型号 == goodsCode && a.物品名称 == goodsName
                                   && a.规格 == spec
                               select a;

                    if (data.Count() != 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 查找未报废的普通入库单或未报废的报检入库单中是否有此订单记录
        /// </summary>
        /// <param name="orderBillID">订单号</param>
        /// <returns>查找到返回true，否则返回false</returns>
        public bool FindInDepotOrderGoodsCount(string orderBillID)
        {
            string strSql =  @"select * from (
                            select a.Bill_ID,BillStatus,OrderBill_ID,GoodsID 
                            from S_OrdinaryInDepotBill as a inner join S_OrdinaryInDepotGoodsBill as b on a.Bill_ID = b.Bill_ID
                            Union all
                            select Bill_ID,BillStatus,OrderFormNumber,GoodsID from S_CheckOutInDepotBill
                            Union all
                            select DJH,DJZT,OrderFormNumber,GoodsID from S_MusterAffirmBill
                            Union all
                            select Bill_ID,BillStatus,OrderFormNumber,GoodsID from S_CheckOutInDepotForOutsourcingBill) as a 
                            where BillStatus not in ('已报废') and OrderBill_ID = '" + orderBillID + "'";

            DataTable tempTable = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (tempTable.Rows.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 审核订单
        /// </summary>
        /// <param name="auditingPersonnel">审核人工号</param>
        /// <param name="orderFormNumber">审核订单号</param>
        /// <param name="error">错误信息, 没有则为null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool Auditing(string auditingPersonnel, string orderFormNumber, out string error)
        {
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                var varData = from a in dataContxt.B_OrderFormInfo
                              where a.OrderFormNumber == orderFormNumber
                              select a;

                if (varData.Count() == 0)
                {
                    error = "无记录";
                    return false;
                }

                B_OrderFormInfo lnqOrderForm = varData.Single();

                lnqOrderForm.DepartmentDirector = auditingPersonnel;

                dataContxt.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 检查订单是否已审核
        /// </summary>
        /// <param name="orderFormNumber">订单号</param>
        /// <returns>审核了返回true，否则返回false</returns>
        public bool CheckDate(string orderFormNumber)
        {
            string strSql = "select DepartmentDirector from B_OrderFormInfo where OrderFormNumber = '" + orderFormNumber + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dt.Rows.Count == 0)
            {
                return false;
            }
            else if (dt.Rows[0][0].ToString().Trim() == "")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 获得未到货订单列表
        /// </summary>
        /// <param name="startTime">起始时间</param>
        /// <param name="endTime">截止时间</param>
        /// <param name="status">状态</param>
        /// <param name="provider">供应商</param>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回Table</returns>
        public DataTable GetGoodsAfloatOrderForm(DateTime? startTime, DateTime? endTime, string status, string provider, int goodsID)
        {
            DataTable resultTable = new DataTable();

            string strSql = " select 供应商, 订单号, 创建日期, 图号型号, 物品名称, 规格, 要求到货日期, 订单数量, 已到货数量, 未到货数量, 状态, 物品ID " +
                            " from View_Report_OrderFormArriveCondition where 供应商 in (select Provider from ProviderPrincipal  "+
                            " where PrincipalWorkId = '" + BasicInfo.LoginID + "')";

            if (status != "全部")
            {
                strSql += " and 状态 = '"+ status +"'";
            }

            if (startTime != null && endTime != null)
            {
                strSql += " and 创建日期 >= '" + startTime.Value.ToShortDateString()
                    + "' and 创建日期 <= '" + endTime.Value.ToShortDateString() + "'";
            }

            if (provider != null && provider != "")
            {
                strSql += " and 供应商 = '"+ provider +"'";
            }

            if (goodsID != 0)
            {
                strSql += " and 物品ID = " + goodsID;
            }

            strSql += " order by 创建日期 desc";

            resultTable = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return resultTable;
        }

        /// <summary>
        /// 变更订单状态
        /// </summary>
        /// <param name="orderFormNumber">订单号</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="status">变更状态</param>
        public void UpdateOrderFormCloseStatus(string orderFormNumber, int goodsID, bool status)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.B_OrderFormGoods
                          where a.OrderFormNumber == orderFormNumber
                          && a.GoodsID == goodsID
                          select a;

            if (varData.Count() == 1)
            {
                B_OrderFormGoods lnqGoods = varData.Single();

                lnqGoods.IsClose = status;
            }

            ctx.SubmitChanges();
        }
    }
}
