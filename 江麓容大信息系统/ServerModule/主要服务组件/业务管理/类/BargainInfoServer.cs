/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  BargainInfoServer.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2010/07/17
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
using PlatformManagement;

namespace ServerModule
{
    /// <summary>
    /// 合同信息管理类
    /// </summary>
    class BargainInfoServer : BasicServer, IBargainInfoServer
    {
        /// <summary>
        /// 通过订单号,物品ID获得合同单价
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="orderFormNumber">订单号</param>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回合同单价</returns>
        public decimal GetBargainUnitPrice(DepotManagementDataContext ctx, string orderFormNumber, int goodsID)
        {
            var varData = from a in ctx.B_OrderFormInfo
                          join b in ctx.B_BargainGoods on a.BargainNumber equals b.BargainNumber
                          join c in ctx.B_BargainInfo on b.BargainNumber equals c.BargainNumber
                          where a.OrderFormNumber == orderFormNumber
                          && b.GoodsID == goodsID
                          select new { UnitPrice = b.UnitPrice/(1 + c.Cess/100) };

            if (varData.Count() == 0)
            {
                return 0;
            }
            else
            {
                return varData.First().UnitPrice;
            }
        }

        /// <summary>
        /// 通过订单号,物品ID获得合同单价
        /// </summary>
        /// <param name="orderFormNumber">订单号</param>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回合同单价</returns>
        public decimal GetBargainUnitPrice(string orderFormNumber, int goodsID)
        {
            string strSql = " select UnitPrice/(1 + Cess/100) from B_OrderFormInfo as a " +
                            " inner join B_BargainGoods as b on a.BargainNumber = b.BargainNumber " +
                            " inner join B_BargainInfo as d on d.BargainNumber = b.BargainNumber " +
                            " where a.OrderFormNumber = '" + orderFormNumber + "' and b.GoodsID = " + goodsID;

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dtTemp.Rows.Count == 0)
            {
                return 0;
            }
            else
            {
                return Convert.ToDecimal(dtTemp.Rows[0][0]);
            }
        }

        /// <summary>
        /// 获得最近一次的合同信息
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="provider">供应商编码</param>
        /// <returns>返回获得的数据集</returns>
        public DataRow GetLatelyBargainNumberInfo(int goodsID, string provider)
        {
            string strSql = " select Top 1 * from B_BargainGoods as a inner join B_BargainInfo as c on "+
                            " a.BargainNumber = c.BargainNumber " +
                            " where a.GoodsID = " + goodsID + " and c.Provider = '" + provider 
                            + "' and c.IsConsignOut = 0 order by Date desc";

            DataTable tempTable = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (tempTable.Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return tempTable.Rows[0];
            }
        }

        /// <summary>
        /// 获得合同信息
        /// </summary>
        /// <param name="bargainNumber">合同号</param>
        /// <returns>返货指定合同号的合同信息</returns>
        public DataRow GetBargainInfoDataRow(string bargainNumber)
        {
            string strSql = "select * from B_BargainInfo where BargainNumber = '" + bargainNumber + "'";

            return  GlobalObject.DatabaseServer.QueryInfo(strSql).Rows[0];
        }

        /// <summary>
        /// 获取所有允许查询的合同信息
        /// </summary>
        /// <param name="returnResult">返回的结果</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功的标志</returns>
        public bool GetAllBargainInfo(out IQueryResult returnResult, out string error)
        {
            returnResult = null;
            error = null;

            IAuthorization m_authorization = PlatformFactory.GetObject<IAuthorization>();
            IQueryResult qr = null;

            if (QueryResultFilter == null)
            {
                qr = m_authorization.Query("合同信息普通查询", null);
            }
            else
            {
                qr = m_authorization.Query("合同信息普通查询", null, QueryResultFilter);
            }

            if (!qr.Succeeded)
            {
                error = qr.Error;
                return false;
            }

            returnResult = qr;

            return true;
        }

        /// <summary>
        /// 获取指定的订单信息
        /// </summary>
        /// <param name="bargainNumber">订单号</param>
        /// <returns>成功返回获取到的订单信息, 失败返回null</returns>
        public View_B_BargainInfo GetBargainInfo(string bargainNumber)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.View_B_BargainInfo 
                         join k in dataContxt.B_BargainInfo on r.合同号 equals k.BargainNumber
                         where r.合同号 == bargainNumber && k.IsDisable == false
                         select r;

            if (result.Count() > 0)
            {
                return result.Single();
            }

            return null;
        }

        public decimal GetTaxRate(string bargainNumber)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.View_B_BargainInfo
                         join k in dataContxt.B_BargainInfo on r.合同号 equals k.BargainNumber
                         where r.合同号 == bargainNumber
                         select r;

            if (result.Count() > 0)
            {
                return result.Single().税率;
            }

            return 0;
        }

        /// <summary>
        /// 获取指定合同的采购员编码
        /// </summary>
        /// <param name="bargainNumber">合同号</param>
        /// <returns>返回获取到的采购员编码</returns>
        public string GetBuyerCode(string bargainNumber)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.B_BargainInfo 
                         where r.BargainNumber == bargainNumber 
                         select r.Buyer;

            if (result.Count() == 0)
            {
                return "";
            }
            else
            {
                return result.First();
            }
        }

        /// <summary>
        /// 添加合同信息
        /// </summary>
        /// <param name="bargainInfo">合同信息</param>
        /// <param name="returnBargainInfo">返回查询到的合同信息</param>
        /// <param name="error">错误信息, 没有则为null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool AddBargainInfo(B_BargainInfo bargainInfo, out IQueryResult returnBargainInfo, out string error)
        {
            returnBargainInfo = null;
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                dataContxt.B_BargainInfo.InsertOnSubmit(bargainInfo);
                dataContxt.SubmitChanges();

                return GetAllBargainInfo(out returnBargainInfo, out error);
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 修改合同信息
        /// </summary>
        /// <param name="oldBargainNumber">旧合同号</param>
        /// <param name="bargainInfo">合同信息</param>
        /// <param name="returnBargainInfo">返回查询到的合同信息</param>
        /// <param name="error">错误信息, 没有则为null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool UpdateBargainInfo(string oldBargainNumber, B_BargainInfo bargainInfo, 
            out IQueryResult returnBargainInfo, out string error)
        {
            returnBargainInfo = null;
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from r in dataContxt.B_BargainInfo 
                             where r.BargainNumber == oldBargainNumber 
                             select r;

                if (result.Count() == 0)
                {
                    error = string.Format("找不到合同号为 [{0}] 的信息！", oldBargainNumber);
                    return false;
                }

                B_BargainInfo record = result.Single();
                IBargainGoodsServer goodsServer = ServerModuleFactory.GetServerModule<IBargainGoodsServer>();

                if (oldBargainNumber != bargainInfo.BargainNumber)
                {
                    IQueryable<View_B_BargainGoods> returnBargainGoods;

                    if (!goodsServer.GetBargainGoods(oldBargainNumber, out returnBargainGoods, out error))
                    {
                        return false;
                    }

                    goodsServer.AutoSubmitToDatabase = false;

                    foreach (var item in returnBargainGoods)
                    {
                        if (!goodsServer.UpdateBargainNumber(dataContxt, item.序号, bargainInfo.BargainNumber, out error))
                        {
                            return false;
                        }
                    }

                    #region 更新订单信息表中的合同编号

                    IOrderFormInfoServer orderFormServer = ServerModuleFactory.GetServerModule<IOrderFormInfoServer>();
                    orderFormServer.UpdateBargainNumber(dataContxt, oldBargainNumber, bargainInfo.BargainNumber);

                    #endregion

                    dataContxt.B_BargainInfo.DeleteOnSubmit(record);
                    record = new B_BargainInfo();
                }

                record.BargainNumber = bargainInfo.BargainNumber;
                record.Provider = bargainInfo.Provider;
                record.Buyer = bargainInfo.Buyer;
                record.Cess = bargainInfo.Cess;
                record.Date = bargainInfo.Date;
                record.InputPerson = bargainInfo.InputPerson;
                record.LaisonMode = bargainInfo.LaisonMode;
                record.ProviderLinkman = bargainInfo.ProviderLinkman;
                record.IsOverseas = bargainInfo.IsOverseas;
                record.IsConsignOut = bargainInfo.IsConsignOut;
                record.Remark = bargainInfo.Remark;
                record.AuditDate = bargainInfo.AuditDate;

                if (oldBargainNumber != bargainInfo.BargainNumber)
                {
                    dataContxt.B_BargainInfo.InsertOnSubmit(record);
                }

                dataContxt.SubmitChanges();

                goodsServer.AutoSubmitToDatabase = true;

                return GetAllBargainInfo(out returnBargainInfo, out error);
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 删除合同信息
        /// </summary>
        /// <param name="bargainNumber">合同编号</param>
        /// <param name="returnBargainInfo">返回查询到的合同信息</param>
        /// <param name="error">错误信息, 没有则为null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool DeleteBargainInfo(string bargainNumber, out IQueryResult returnBargainInfo, out string error)
        {
            error = null;
            returnBargainInfo = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;
                IBargainGoodsServer goodsServer = ServerModuleFactory.GetServerModule<IBargainGoodsServer>();
                IQueryable<View_B_BargainGoods> returnBargainGoods;

                if (!goodsServer.GetBargainGoods(bargainNumber, out returnBargainGoods, out error))
                {
                    return false;
                }

                #region 检查此合同的订单信息

                IOrderFormInfoServer orderFormServer = ServerModuleFactory.GetServerModule<IOrderFormInfoServer>();

                if (orderFormServer.GetOrderFormCollection(bargainNumber).Count() > 0)
                {
                    error = string.Format("合同 [{0}] 还包含有订单信息无法进行删除，请将所有此合同包含的订单信息全部删除后才能进行此操作！", 
                        bargainNumber);
                    return false;
                }

                #endregion

                if (returnBargainGoods != null && returnBargainGoods.Count() > 0)
                {
                    error = string.Format("合同 [{0}] 还包含有零件信息无法进行删除，请将所有此合同包含的零件信息全部删除后才能进行此操作！", 
                        bargainNumber);
                    return false;
                }

                Table<B_BargainInfo> table = dataContxt.GetTable<B_BargainInfo>();

                var delRow = from c in table 
                             where c.BargainNumber == bargainNumber 
                             select c;

                table.DeleteAllOnSubmit(delRow);
                dataContxt.SubmitChanges();

                return GetAllBargainInfo(out returnBargainInfo, out error);
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 审核合同
        /// </summary>
        /// <param name="auditingPersonnel">审核人</param>
        /// <param name="bargainNumber">合同号</param>
        /// <param name="error">出错时返回错误信息，否则为null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool AuditingBargainInfo(string auditingPersonnel, string bargainNumber, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;
                
                var varData = from a in dataContxt.B_BargainInfo
                              where a.BargainNumber == bargainNumber
                              select a;

                if (varData.Count() == 0)
                {
                    error = "无记录";
                    return false;
                }

                B_BargainInfo lnqbargin = varData.Single();

                lnqbargin.AuditPersonnel = auditingPersonnel;
                lnqbargin.AuditDate = ServerTime.Time;

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
        /// 检查是否已审核
        /// </summary>
        /// <param name="bargainNumber">合同号</param>
        /// <returns>已经审核返回true</returns>
        public bool IsAudited(string bargainNumber)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from a in dataContxt.B_BargainInfo
                          where a.BargainNumber == bargainNumber
                          select a;

            if (result.Count() == 0)
            {
                return false;
            }

            return result.First().AuditDate == null;
        }

        /// <summary>
        /// 获得税率
        /// </summary>
        /// <param name="orderFormNumber">订单号</param>
        /// <returns>返回税率</returns>
        public decimal GetBargainCess(string orderFormNumber)
        {
            string sSql = "select top 1 cess  from B_BargainInfo as a inner join B_OrderFormInfo as b "+
                          " on a.BargainNumber = b.BargainNumber  where b.OrderFormNumber = '" + orderFormNumber + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sSql);

            if (dt.Rows.Count == 0)
            {
                return 1;
            }

            return 1 + (Convert.ToDecimal(dt.Rows[0][0]) / 100);
        }

        public void DisableInfo(string bargainNumber)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.B_BargainInfo
                          where a.BargainNumber == bargainNumber
                          select a;

            if (varData.Count() == 1)
            {
                varData.Single().IsDisable = true;
            }

            ctx.SubmitChanges();
        }

        #region 夏石友，2012-07-18，将报检入库单中的此功能移动到此，原方法名：CheckBargainIsConsignOut

        /// <summary>
        /// 检查订单对应的合同是否为委外合同
        /// </summary>
        /// <param name="orderFormNumber">订单号</param>
        /// <returns>是返回true, 否返回false</returns>
        public bool IsConsignOutBargain(string orderFormNumber)
        {
            if (orderFormNumber == "")
            {
                return true;
            }

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var result = from r in ctx.B_BargainInfo
                         join o in ctx.B_OrderFormInfo on r.BargainNumber equals o.BargainNumber
                         where o.OrderFormNumber == orderFormNumber
                         select r;

            if (result.Count() == 0)
            {
                return false;
            }
            else
            {
                return result.First().IsConsignOut;
            }
        }

        /// <summary>
        /// 检查订单对应的合同是否为海外合同
        /// </summary>
        /// <param name="orderFormNumber">订单号</param>
        /// <returns>是返回true, 否返回false</returns>
        public bool IsOverseasBargain(string orderFormNumber)
        {
            if (orderFormNumber == "")
            {
                return true;
            }

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var result = from r in ctx.B_BargainInfo
                         join o in ctx.B_OrderFormInfo on r.BargainNumber equals o.BargainNumber
                         where o.OrderFormNumber == orderFormNumber
                         select r;

            if (result.Count() == 0)
            {
                return false;
            }
            else
            {
                return result.First().IsOverseas;
            }
        }

        #endregion
    }
}
