/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  BargainGoodsServer.cs
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

namespace ServerModule
{
    /// <summary>
    /// 合同物品信息管理类
    /// </summary>
    class BargainGoodsServer : BasicServer,  IBargainGoodsServer
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
        /// 判断是否可以操作
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="bargainNumber">合同号</param>
        /// <returns>可以操作 返回True，不可操作 返回False</returns>
        public bool IsOperator(int goodsID, string bargainNumber)
        {
            string strSql = @"select * from (
                            select Distinct a.BargainNumber,b.GoodsID
                            from B_OrderFormInfo as a inner join B_OrderFormGoods as b on a.OrderFormNumber = b.OrderFormNumber
                            inner join 
                            (select Distinct GoodsID,OrderBill_ID from (
                            select a.Bill_ID,BillStatus,OrderBill_ID,GoodsID 
                            from S_OrdinaryInDepotBill as a inner join S_OrdinaryInDepotGoodsBill as b on a.Bill_ID = b.Bill_ID
                            Union all
                            select Bill_ID,BillStatus,OrderFormNumber,GoodsID from S_CheckOutInDepotBill
                            Union all
                            select DJH,DJZT,OrderFormNumber,GoodsID from S_MusterAffirmBill
                            Union all
                            select Bill_ID,BillStatus,OrderFormNumber,GoodsID from S_CheckOutInDepotForOutsourcingBill) as a 
                            where BillStatus not in ('已报废') and OrderBill_ID <> '' and OrderBill_ID is not null) as d
                            on b.GoodsID = d.GoodsID and b.OrderFormNumber = d.OrderBill_ID) as a 
                            where GoodsID = " + goodsID +
                            " and BargainNumber = '" + bargainNumber + "'";

            DataTable tempTable = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (tempTable.Rows.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取合同物品表中某一产品单价
        /// </summary>
        /// <param name="bargainNumber">合同物品号</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="provider">供应商</param>
        /// <returns>返回获取到的单价</returns>
        public decimal GetGoodsUnitPrice(string bargainNumber, int goodsID, string provider)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            DateTime dateTemp = ServerTime.Time;

            var varData = from a in dataContxt.Bus_PurchasingMG_UnitPriceList
                          where a.Provider == provider
                          && a.GoodsID == goodsID
                          && a.ValidityStart <= dateTemp
                          && a.ValidityEnd >= dateTemp
                          select a;

            if (varData.Count() > 0)
            {
                Bus_PurchasingMG_UnitPriceList tempList = varData
                    .OrderByDescending(k => k.ValidityStart)
                    .ThenByDescending(k => k.VersionNo).First();

                decimal unitPrice = (decimal)tempList.UnitPrice;
                int rate = (int)tempList.Rate;

                return unitPrice / (1m + rate / 100.0m);
            }

            var goodsGroup = from r in dataContxt.B_BargainGoods
                             where r.BargainNumber == bargainNumber && r.GoodsID == goodsID
                             select r;

            if (goodsGroup.Count() == 0)
            {
                var varData1 = from a in dataContxt.B_OrderFormInfo
                               where a.BargainNumber == bargainNumber
                               || a.OrderFormNumber == bargainNumber
                               select a;

                if (varData1.Count() > 0)
                {
                    bargainNumber = varData1.First().BargainNumber;

                    if (bargainNumber == "价格清单")
                    {
                        throw new Exception("无法获取物品单价");
                    }

                    goodsGroup = from r in dataContxt.B_BargainGoods
                                 where r.BargainNumber == bargainNumber && r.GoodsID == goodsID
                                 select r;
                }
                else
                {
                    throw new Exception("无法获取物品单价");
                }
            }

            B_BargainInfo bargain = (from r in dataContxt.B_BargainInfo
                          where r.BargainNumber == bargainNumber
                          select r).Single();

            if (bargain.IsOverseas)
            {
                return (decimal)goodsGroup.Single().UnitPrice;
            }
            else
            {
                return goodsGroup.Single().UnitPrice / (1m + bargain.Cess / 100.0m);
            }
        }

        /// <summary>
        /// 获取合同物品信息
        /// </summary>
        /// <param name="bargainNumber">合同号</param>
        /// <param name="returnBargainGoods">返回查询到的信息</param>
        /// <param name="error">错误信息, 没有则为null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool GetBargainGoods(string bargainNumber, out IQueryable<View_B_BargainGoods> returnBargainGoods, out string error)
        {
            error = null;
            returnBargainGoods = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                returnBargainGoods = from c in dataContxt.View_B_BargainGoods
                                     where c.合同号 == bargainNumber
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
        /// 添加合同物品信息
        /// </summary>
        /// <param name="bargainGoods">合同物品信息</param>
        /// <param name="returnBargainGoods">返回查询到的合同物品信息</param>
        /// <param name="error">错误信息, 没有则为null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool AddBargainGoods(B_BargainGoods bargainGoods, out IQueryable<View_B_BargainGoods> returnBargainGoods, out string error)
        {
            returnBargainGoods = null;
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                if (bargainGoods.GoodsID == 0)
                {
                    error = "请录入物品";
                    return false;
                }

                var varBill = from a in dataContxt.B_BargainInfo
                              where a.BargainNumber == bargainGoods.BargainNumber
                              select a;

                if (varBill.Count() != 1)
                {
                    error = "请先添加合同，再添加物品";
                    return false;
                }

                dataContxt.B_BargainGoods.InsertOnSubmit(bargainGoods);

                if (AutoSubmitToDatabase)
                {
                    dataContxt.SubmitChanges();
                }

                return GetBargainGoods(bargainGoods.BargainNumber, out returnBargainGoods, out error);
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }            
        }

        /// <summary>
        /// 修改合同物品信息
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="newGoods">新合同物品</param>
        /// <param name="returnBargainGoods">返回查询到的合同物品信息</param>
        /// <param name="error">错误信息, 没有则为null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool UpdateBargainGoods(int goodsID, B_BargainGoods newGoods, 
            out IQueryable<View_B_BargainGoods> returnBargainGoods, out string error)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            return UpdateBargainGoods(dataContxt, goodsID, newGoods, out returnBargainGoods, out error);
        }

        /// <summary>
        /// 修改合同物品信息
        /// </summary>
        /// <param name="dataContext">LINQ 数据库上下文</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="newGoods">新合同物品</param>
        /// <param name="returnBargainGoods">返回查询到的合同物品信息</param>
        /// <param name="error">错误信息, 没有则为null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool UpdateBargainGoods(DepotManagementDataContext dataContext, int goodsID,
            B_BargainGoods newGoods, out IQueryable<View_B_BargainGoods> returnBargainGoods, out string error)
        {
            returnBargainGoods = null;
            error = null;

            try
            {
                var result = from r in dataContext.B_BargainGoods
                             where r.GoodsID == goodsID
                             && r.BargainNumber == newGoods.BargainNumber
                             select r;

                if (result.Count() == 0)
                {
                    error = "找不到指定物品信息，无法进行此操作！";
                    return false;
                }

                B_BargainGoods record = result.Single();

                record.BargainNumber = newGoods.BargainNumber;
                record.UnitPrice = newGoods.UnitPrice;
                record.Amount = newGoods.Amount;
                record.Remark = newGoods.Remark;

                if (AutoSubmitToDatabase)
                {
                    dataContext.SubmitChanges();
                }

                return GetBargainGoods(newGoods.BargainNumber, out returnBargainGoods, out error);
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 修改合同物品信息中的合同号
        /// </summary>
        /// <param name="id">合同物品ID</param>
        /// <param name="newBargainNumber">新合同号</param>
        /// <param name="error">错误信息, 没有则为null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool UpdateBargainNumber(int id, string newBargainNumber, out string error)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            return UpdateBargainNumber(dataContxt, id, newBargainNumber, out error);
        }

        /// <summary>
        /// 修改合同物品信息中的合同号
        /// </summary>
        /// <param name="dataContext">数据库上下文</param>
        /// <param name="id">合同物品ID</param>
        /// <param name="newBargainNumber">新合同号</param>
        /// <param name="error">错误信息, 没有则为null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool UpdateBargainNumber(DepotManagementDataContext dataContext, int id, string newBargainNumber, out string error)
        {
            error = null;

            try
            {
                var result = from r in dataContext.B_BargainGoods
                             where r.ID == id
                             select r;

                if (result.Count() == 0)
                {
                    error = "找不到指定物品信息，无法进行此操作！";
                    return false;
                }

                B_BargainGoods record = result.Single();

                record.BargainNumber = newBargainNumber;

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
        /// 操作零星采购单相关业务
        /// </summary>
        /// <param name="bargainBillNo">合同号</param>
        /// <param name="minorPurchaseBillNo">零星采购单号</param>
        public void OperatorMinorPurchase(string bargainBillNo, string minorPurchaseBillNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            try
            {
                var varData = from a in ctx.B_BargainInfo
                              where a.BargainNumber == bargainBillNo
                              select a;

                if (varData.Count() != 1)
                {
                    throw new Exception("数据不唯一");
                }

                varData.Single().MinorPurchaseBillNo = minorPurchaseBillNo;

                var varGoods = from a in ctx.B_BargainGoods
                               where a.BargainNumber == bargainBillNo
                               select a;

                ctx.B_BargainGoods.DeleteAllOnSubmit(varGoods);
                ctx.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 删除合同物品信息
        /// </summary>
        /// <param name="bargainNumber">合同号</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="returnBargainGoods">返回查询到的合同物品信息</param>
        /// <param name="error">错误信息, 没有则为null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool DeleteBargainGoods(string bargainNumber, int goodsID, 
            out IQueryable<View_B_BargainGoods> returnBargainGoods, out string error)
        {
            returnBargainGoods = null;
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;
                Table<B_BargainGoods> table = dataContxt.GetTable<B_BargainGoods>();

                var result = from r in dataContxt.B_BargainGoods
                             where r.GoodsID == goodsID
                             && r.BargainNumber == bargainNumber
                             select r;

                if (result.Count() == 0)
                {
                    error = "找不到指定物品信息，无法进行此操作！";
                    return false;
                }

                dataContxt.B_BargainGoods.DeleteAllOnSubmit(result);

                var temp = from a in dataContxt.B_BargainGoods
                           where a.BargainNumber == bargainNumber
                           select a;

                if (temp == null || temp.Count() == 0)
                {
                    var temp1 = from a in dataContxt.B_BargainInfo
                                where a.BargainNumber == bargainNumber
                                select a;

                    temp1.Single().MinorPurchaseBillNo = null;
                }

                dataContxt.SubmitChanges();

                return GetBargainGoods(bargainNumber, out returnBargainGoods, out error);
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 获取指定合同号对应的物品信息
        /// </summary>
        /// <param name="bargainNumber">合同号</param>
        /// <returns>指定合同号对应的物品信息</returns>
        public IQueryable<B_BargainGoods> GetBargainGoodsInfo(string bargainNumber)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            return from a in dataContxt.B_BargainGoods
                   where a.BargainNumber == bargainNumber
                   select a;
        }
    }
}
