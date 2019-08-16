/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  InvoiceServer.cs
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
using DBOperate;
using System.Data.Linq;
using System.Data.Linq.Mapping;


namespace ServerModule
{
    /// <summary>
    /// 发票管理类
    /// </summary>
    class InvoiceServer : BasicServer, IInvoiceServer
    {
        /// <summary>
        /// 基础物品服务组件
        /// </summary>
        IBasicGoodsServer m_basicGoodsServer = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();

        /// <summary>
        /// 获取全部发票记录
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>返回查询到的发票记录表</returns>
        public DataTable GetInvoiceInfo(DateTime startTime, DateTime endTime)
        {
            startTime = startTime.Date;
            endTime = endTime.Date;

            string sql = " select distinct InvoiceCode,TaxRat,PZH," +
                            " case InvoiceType when 0 then '普通发票' else '专用发票' end as InvoiceType, " +
                            " sum(Price) as Price, sum(Tax) as Tax,SUM(Price)+SUM(Tax) as SumPrice," +
                            " CONVERT( varchar(17),Date,120) + '00' as Date," +
                            " Provider from B_Invoice where date between '" + startTime.ToShortDateString() + "' and '" + endTime.ToShortDateString() + " 00:00:00'" +
                            " Group by InvoiceCode,InvoiceType,Provider,TaxRat,CONVERT( varchar(17),Date,120),PZH order by PZH desc";

            return GlobalObject.DatabaseServer.QueryInfo(sql);
        }

        /// <summary>
        /// 获得发票数据集
        /// </summary>
        /// <param name="invoiceCode">发票号</param>
        /// <returns>返回查询到的发票信息</returns>
        public DataTable GetInvoiceInfo(string invoiceCode)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            IQueryable<View_B_Invoice> IQinvoice = from a in dataContxt.View_B_Invoice
                                                     where a.InvoiceCode == invoiceCode
                                                     select a;

            return GlobalObject.GeneralFunction.ConvertToDataTable<View_B_Invoice>(IQinvoice);
        }

        /// <summary>
        /// 添加发票记录
        /// </summary>
        /// <param name="invoiceTable">需要添加的发票信息数据</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>添加成功返回True，添加失败返回False</returns>
        public bool AddInvoiceInfo(DataTable invoiceTable, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;
                List<B_Invoice> lstInvoice = new List<B_Invoice>();

                if (invoiceTable.Rows.Count == 0)
                {
                    return false;
                }
                else
                {
                    for (int i = 0; i <= invoiceTable.Rows.Count - 1; i++)
                    {

                        B_Invoice lnqInvoice = new B_Invoice();

                        int intGoodsID = m_basicGoodsServer.GetGoodsID(
                            invoiceTable.Rows[i]["GoodsCode"].ToString(),
                            invoiceTable.Rows[i]["GoodsName"].ToString(),
                            invoiceTable.Rows[i]["Spec"].ToString());

                        lnqInvoice.InvoiceCode = invoiceTable.Rows[i]["InvoiceCode"].ToString();//发票号
                        lnqInvoice.GoodsID = intGoodsID;
                        lnqInvoice.Provider = invoiceTable.Rows[i]["Provider"].ToString();//供应商
                        lnqInvoice.Count = Convert.ToDecimal(invoiceTable.Rows[i]["Count"]);//数量
                        lnqInvoice.UnitPrice = Convert.ToDecimal(invoiceTable.Rows[i]["UnitPrice"]);//单价
                        lnqInvoice.OrderNumber = invoiceTable.Rows[i]["OrderNumber"].ToString();//订单号
                        lnqInvoice.Bill_ID = invoiceTable.Rows[i]["Bill_ID"].ToString();//入库单号
                        lnqInvoice.BatchNo = invoiceTable.Rows[i]["BatchNo"].ToString();//批次号
                        lnqInvoice.InvoiceType = Convert.ToInt32(invoiceTable.Rows[i]["InvoiceType"]);//发票类型
                        lnqInvoice.Date = ServerTime.Time;//日期
                        lnqInvoice.BeforTax = Convert.ToDecimal(invoiceTable.Rows[i]["BeforTax"]);//含税单价
                        lnqInvoice.TaxRat = Convert.ToInt32(invoiceTable.Rows[i]["TaxRat"]);//税率
                        lnqInvoice.Tax = Convert.ToDecimal(invoiceTable.Rows[i]["Tax"]);//税额
                        lnqInvoice.Price = Convert.ToDecimal(invoiceTable.Rows[i]["Price"]);//金额
                        lnqInvoice.PZH = invoiceTable.Rows[i]["PZH"].ToString();//凭证号

                        lstInvoice.Add(lnqInvoice);
                    }
                }

                dataContxt.B_Invoice.InsertAllOnSubmit(lstInvoice);
                dataContxt.SubmitChanges();

                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 删除发票记录
        /// </summary>
        /// <param name="invoiceCode">发票号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        public bool DeleteInvoiceInfo(string invoiceCode, out string error)
        {
            error = null;

            string str = "0";
            int count = 0;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                OrdinaryInDepotBillServer serverOrdinaryBill = new OrdinaryInDepotBillServer();

                MaterialRejectBill serverMaterialRejectBill = new MaterialRejectBill();

                string strSql = "select * from B_Invoice where InvoiceCode = '" + invoiceCode + "'";

                DataTable dtInvoice = GlobalObject.DatabaseServer.QueryInfo(strSql);

                for (int i = 0; i <= dtInvoice.Rows.Count - 1; i++)
                {
                    count = i;

                    decimal dcOldUnitPrice = 0;

                    if (!GlobalObject.GeneralFunction.IsNullOrEmpty(error))
                    {
                        return false;
                    }

                    str = "1";

                    #region 改变入库表的单价(普通入库或者报检入库)

                    var varCheckOutInDepot = from a in dataContxt.S_CheckOutInDepotBill
                                 where a.Bill_ID == dtInvoice.Rows[i]["Bill_ID"].ToString()
                                 && a.GoodsID == Convert.ToInt32(dtInvoice.Rows[i]["GoodsID"].ToString())
                                 && a.BatchNo == dtInvoice.Rows[i]["BatchNo"].ToString()
                                 select a;

                    if (varCheckOutInDepot.Count() != 0)
                    {
                        S_CheckOutInDepotBill lnqCheckOutInDepot = varCheckOutInDepot.Single();

                        lnqCheckOutInDepot.UnitInvoicePrice = 0;
                        lnqCheckOutInDepot.InvoicePrice = 0;
                        lnqCheckOutInDepot.HavingInvoice = false;
                        dcOldUnitPrice = lnqCheckOutInDepot.UnitPrice;
                        dataContxt.SubmitChanges();
                    }
                    else
                    {
                        var varOrdinaryGoods = from a in dataContxt.S_OrdinaryInDepotGoodsBill
                                       where a.Bill_ID == dtInvoice.Rows[i]["Bill_ID"].ToString()
                                       && a.GoodsID == Convert.ToInt32(dtInvoice.Rows[i]["GoodsID"].ToString())
                                       && a.BatchNo == dtInvoice.Rows[i]["BatchNo"].ToString()
                                       select a;

                        if (varOrdinaryGoods.Count() != 0)
                        {
                            S_OrdinaryInDepotGoodsBill lnqOrdinaryGoods = varOrdinaryGoods.Single();

                            lnqOrdinaryGoods.InvoiceUnitPrice = 0;
                            lnqOrdinaryGoods.InvoicePrice = 0;
                            lnqOrdinaryGoods.HavingInvoice = false;

                            dcOldUnitPrice = lnqOrdinaryGoods.UnitPrice;
                            dataContxt.SubmitChanges();

                            int intFlag = serverOrdinaryBill.GetHavingInvoice(dtInvoice.Rows[i]["Bill_ID"].ToString(), out error);

                            if (intFlag == 4)
                            {
                                return false;
                            }
                            else
                            {
                                var varOrdinaryBill = from a in dataContxt.S_OrdinaryInDepotBill
                                              where a.Bill_ID == dtInvoice.Rows[i]["Bill_ID"].ToString()
                                              select a;

                                if (varOrdinaryBill.Count() != 0)
                                {
                                    S_OrdinaryInDepotBill lnqOrdinaryBill = varOrdinaryBill.Single();
                                    lnqOrdinaryBill.InvoiceStatus = intFlag;

                                    dataContxt.SubmitChanges();
                                }
                            }
                        }
                        else
                        {
                            var varRejectList = from a in dataContxt.S_MaterialListRejectBill
                                           where a.Bill_ID == dtInvoice.Rows[i]["Bill_ID"].ToString()
                                           && a.GoodsID == Convert.ToInt32(dtInvoice.Rows[i]["GoodsID"].ToString())
                                           && a.BatchNo == dtInvoice.Rows[i]["BatchNo"].ToString()
                                           select a;

                            if (varRejectList.Count() != 0)
                            {
                                S_MaterialListRejectBill lnqMaterialList = varRejectList.Single();

                                lnqMaterialList.InvoiceUnitPrice = 0;
                                lnqMaterialList.InvoicePrice = 0;
                                lnqMaterialList.HavingInvoice = false;

                                dcOldUnitPrice = lnqMaterialList.UnitPrice;

                                dataContxt.SubmitChanges();

                                int intFlag = serverMaterialRejectBill.SetHavingInvoiceReturn(dtInvoice.Rows[i]["Bill_ID"].ToString(), out error);

                                if (intFlag == 4)
                                {
                                    return false;
                                }
                                else
                                {
                                    var varRejectBill = from a in dataContxt.S_MaterialRejectBill
                                                     where a.Bill_ID == dtInvoice.Rows[i]["Bill_ID"].ToString()
                                                     select a;

                                    if (varRejectBill.Count() != 0)
                                    {
                                        S_MaterialRejectBill lnqMaterialBill = varRejectBill.Single();
                                        lnqMaterialBill.InvoiceFlag = intFlag;

                                        dataContxt.SubmitChanges();
                                    }
                                }
                            }
                            else
                            {

                                var varOutsourcing = from a in dataContxt.S_CheckOutInDepotForOutsourcingBill
                                                     where a.Bill_ID == dtInvoice.Rows[i]["Bill_ID"].ToString()
                                                     && a.GoodsID == Convert.ToInt32(dtInvoice.Rows[i]["GoodsID"].ToString())
                                                     && a.BatchNo == dtInvoice.Rows[i]["BatchNo"].ToString()
                                                     select a;

                                if (varOutsourcing.Count() != 0)
                                {
                                    S_CheckOutInDepotForOutsourcingBill lnqOutsourcing = varOutsourcing.Single();

                                    lnqOutsourcing.UnitInvoicePrice = 0;
                                    lnqOutsourcing.InvoicePrice = 0;
                                    lnqOutsourcing.HavingInvoice = false;
                                    dcOldUnitPrice = lnqOutsourcing.UnitPrice;

                                    dataContxt.SubmitChanges();
                                }
                            }
                        }
                    }

                    #endregion

                    str = "2";

                    #region 改变入库明细表金额

                    var varInDepotDetail = from b in dataContxt.S_InDepotDetailBill
                                   where b.GoodsID == Convert.ToInt32(dtInvoice.Rows[i]["GoodsID"].ToString())
                                   && b.InDepotBillID.Contains(dtInvoice.Rows[i]["Bill_ID"].ToString())
                                   && b.BatchNo == dtInvoice.Rows[i]["BatchNo"].ToString()
                                   select b;

                    if (varInDepotDetail.Count() != 0)
                    {

                        if (varInDepotDetail.Count() == 1)
                        {
                            S_InDepotDetailBill lnqInDepotDetailSingle = varInDepotDetail.Single();

                            lnqInDepotDetailSingle.InvoiceUnitPrice = 0;
                            lnqInDepotDetailSingle.InvoicePrice = 0;
                            lnqInDepotDetailSingle.FactUnitPrice = dcOldUnitPrice;
                            lnqInDepotDetailSingle.FactPrice = Math.Round( dcOldUnitPrice * Convert.ToDecimal(lnqInDepotDetailSingle.InDepotCount),2);
                        }
                        else
                        {
                            var varInDepotDetailList = from a in varInDepotDetail
                                           where a.InDepotBillID == dtInvoice.Rows[i]["Bill_ID"].ToString()
                                           select a;

                            S_InDepotDetailBill lnqInDepotDetailData = varInDepotDetailList.Single();

                            lnqInDepotDetailData.InvoiceUnitPrice = 0;
                            lnqInDepotDetailData.InvoicePrice = 0;
                            lnqInDepotDetailData.FactUnitPrice = dcOldUnitPrice;
                            lnqInDepotDetailData.FactPrice = Math.Round(dcOldUnitPrice
                                * Convert.ToDecimal(lnqInDepotDetailData.InDepotCount), 2);

                            var varData7 = from a in varInDepotDetail
                                               where a.InDepotBillID != dtInvoice.Rows[i]["Bill_ID"].ToString()
                                               select a;

                            dataContxt.S_InDepotDetailBill.DeleteAllOnSubmit(varData7);
                        }

                        dataContxt.SubmitChanges();
                    }
                    #endregion
                }

                var varInvoice = from a in dataContxt.B_Invoice
                                 where a.InvoiceCode == invoiceCode
                                 select a;

                if (varInvoice.Count() > 0)
                {
                    dataContxt.B_Invoice.DeleteAllOnSubmit(varInvoice);
                    dataContxt.SubmitChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message + str + count.ToString();
                return false;
            }
        }

        /// <summary>
        /// 修改发票记录
        /// </summary>
        /// <param name="invoiceCode">新发票号</param>
        /// <param name="provide">供应商</param>
        /// <param name="invoiceType">发票类型 1:专用发票，0:非专用发票</param>
        /// <param name="oldCode">旧发票号</param>
        /// <param name="pzh">新凭证号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>修改成功返回True，修改失败返回False</returns>
        public bool UpdateInvoiceInfo(string invoiceCode, string provide, int invoiceType, string oldCode, string pzh, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var varData = from a in dataContxt.B_Invoice
                              where a.InvoiceCode == oldCode
                              select a;

                foreach (var ar in varData)
                {
                    ar.InvoiceCode = invoiceCode;
                    ar.InvoiceType = invoiceType;
                    ar.Provider = provide;
                    ar.PZH = pzh;
                }

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
        /// 获得对应的单据数据集
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="provider">供应商编码</param>
        /// <param name="orderNumber">订单号</param>
        /// <returns>返回获取对应的单据数据集</returns>
        public DataTable GetBillInfo(DateTime startTime, DateTime endTime, string provider, string orderNumber)
        {
            string sql = " select distinct * from (" +
                          " select distinct Bill_id as 入库单号,orderformnumber as 订单号,provider as 供应商,Bill_Time as 日期" +
                          " from S_CheckOutInDepotBill where billstatus = '已入库' and HavingInvoice = 0 " +
                          " union " +
                          " select distinct Bill_id as 入库单号,OrderFormNumber as 订单号,provider as 供应商,ManagerTime as 日期 " +
                          " from S_CheckOutInDepotForOutsourcingBill where BillStatus = '已入库' and HavingInvoice = 0 " +
                          " union " +
                          " select distinct a.Bill_id as 入库单号,OrderBill_id as 订单号,provider as 供应商,Bill_Time as 日期" +
                          " from S_OrdinaryInDepotBill as a inner join  S_OrdinaryInDepotGoodsBill as b on a.Bill_ID = b.Bill_ID " +
                          " where billstatus = '已入库' and HavingInvoice = 0  " +
                          " union " +
                          " (select distinct a.Bill_Id as 入库单号,AssociateID as 订单号," +
                          " a.provider as 供应商,Bill_Time as 日期 from S_MaterialRejectBill as a" +
                          " inner join S_MaterialListRejectBill as b on a.Bill_ID = b.Bill_ID " +
                          " where a.BillStatus = '已完成' and BillType = '总仓库退货单'  and HavingInvoice = 0 ) " +
                          " ) as a where 日期 between '" + startTime.ToShortDateString() + "' and '" + endTime.ToShortDateString() + " 00:00:00'";

            DataTable dtTemp1 = GlobalObject.DatabaseServer.QueryInfo(sql);

            DataTable dtTemp2 = dtTemp1.Clone();

            if (provider != "" && orderNumber != "")
            {

                DataRow[] dr = dtTemp1.Select("供应商 = '" + provider + "' and 订单号 = '" + orderNumber + "'");

                for (int i = 0; i <= dr.Length - 1; i++)
                {
                    dtTemp2.Rows.Add(dr[i].ItemArray);
                }
            }
            else
            {
                if (provider != "")
                {
                    DataRow[] dr = dtTemp1.Select("供应商 = '" + provider + "'");

                    for (int i = 0; i <= dr.Length - 1; i++)
                    {
                        dtTemp2.Rows.Add(dr[i].ItemArray);
                    }

                }
                else if (orderNumber != "")
                {
                    DataRow[] dr = dtTemp1.Select("订单号 = '" + orderNumber + "'");

                    for (int i = 0; i <= dr.Length - 1; i++)
                    {
                        dtTemp2.Rows.Add(dr[i].ItemArray);
                    }

                }
                else
                {
                    dtTemp2 = dtTemp1.Copy();
                }
            }

            return dtTemp2;
        }

        /// <summary>
        /// 获得对应的物品明细
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <returns>返回对应的物品单据明细</returns>
        public DataTable GetGoodsInfo(string billID)
        {
            //报检入库单
            string sql = " select a.图号型号,a.物品名称,a.规格," +
                          " a.入库数量 as 数量,'件' as 单位," +
                          " cast(cast(b.UnitPrice as decimal(18,4))/cast((b.Cess/100+1) as decimal(18,2)) as decimal(18,10))  as 单价, " +
                          " b.UnitPrice as 含税价,a.仓库 as 类别,a.批次号 as 批次号" +
                          " from View_S_CheckOutInDepotBill as a " +
                          " inner join View_B_BargainAndOrderForm as b " +
                          " on a.订单号 = b.OrderFormNumber " +
                          " and a.物品ID = b.GoodsID " +
                          " and a.供货单位 = b.Provider " +
                          " and a.是否有发票 = 0 " +
                          " where 入库单号 = '" + billID + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            //普通入库单
            if (dt.Rows.Count == 0)
            {
                sql = " select b.图号型号,b.物品名称,b.规格,b.数量,b.单位, " +
                       " cast(cast(c.UnitPrice as decimal(18,4))/cast((c.Cess/100+1) as decimal(18,2)) as decimal(18,10))  as 单价, " +
                       "  c.UnitPrice as 含税价,物品类别 as 类别,批次号 " +
                       " from View_S_OrdinaryInDepotBill as a " +
                       " inner join View_S_OrdinaryInDepotGoodsBill as b " +
                       " on a.入库单号 = b.入库单号 " +
                       " inner join View_B_BargainAndOrderForm as c " +
                       " on a.订单号 = c.OrderFormNumber " +
                       " and b.物品ID = c.GoodsID " +
                       " and a.供应商编码 = c.Provider" +
                       " and b.是否有发票 = 0 " +
                       " where a.入库单号 = '" + billID + "'";

                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = GlobalObject.GlobalParameter.StorehouseConnectionString;
                    conn.Open();

                    SqlCommand command = new SqlCommand(sql);
                    command.Connection = conn;

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dt);
                }
            }

            //退货单
            if (dt.Rows.Count == 0)
            {
                sql = " select b.图号型号,b.物品名称,b.规格,b.退货数 as 数量,b.单位, " +
                       " cast(cast(c.UnitPrice as decimal(18,4))/cast((c.Cess/100+1) as decimal(18,2)) as decimal(18,10))  as 单价, " +
                       "  c.UnitPrice as 含税价,物品类别 as 类别,批次号 " +
                       " from View_S_MaterialRejectBill as a " +
                       " inner join View_S_MaterialListRejectBill as b " +
                       " on a.退货单号 = b.退货单号 " +
                       " inner join View_B_BargainAndOrderForm as c " +
                       " on b.关联单号 = c.OrderFormNumber " +
                       " and b.物品ID = c.GoodsID " +
                       " and a.供应商 = c.Provider" +
                       " and b.是否有发票 = 0 " +
                       " where a.退货单号 = '" + billID + "'";

                dt = GlobalObject.DatabaseServer.QueryInfo(sql);
            }

            //委外报检入库单
            if (dt.Rows.Count == 0)
            {
                sql = " select 图号型号,物品名称,规格,a.入库数 as 数量, 单位, 单价," +
                              " cast(cast(a.单价 as decimal(18,4)) * cast((b.Cess/100+1) as decimal(18,2)) as decimal(18,10))  as 含税价, " +
                              " 材料类别 as 类别,批次号" +
                              " from View_S_CheckOutInDepotForOutsourcingBill as a " +
                              " inner join View_B_BargainAndOrderForm as b " +
                              " on a.关联订单号 = b.OrderFormNumber " +
                              " and a.物品ID = b.GoodsID " +
                              " and a.供应商编码 = b.Provider " +
                              " and a.是否有发票 = 0 " +
                              " where 单据号 = '" + billID + "'";

                dt = GlobalObject.DatabaseServer.QueryInfo(sql);
            }

            return dt;
        }

        /// <summary>
        /// 更新出入库的金额
        /// </summary>
        /// <param name="invoiceTable">需要更新的数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>更新成功True，更新失败False</returns>
        public bool UpdatePrice(DataTable invoiceTable, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;
                OrdinaryInDepotBillServer serverOrdinaryBill = new OrdinaryInDepotBillServer();
                MaterialRejectBill serverMaterialRejectBill = new MaterialRejectBill();
                DateTime dtStart = new DateTime();
                DateTime dtEnd = new DateTime();

                //获得当前日期的月结起始日期与结束日期
                ServerTime.GetMonthlyBalance(ServerTime.Time, out dtStart, out dtEnd);

                for (int i = 0; i <= invoiceTable.Rows.Count - 1; i++)
                {
                    string code = invoiceTable.Rows[i]["GoodsCode"].ToString();
                    string name = invoiceTable.Rows[i]["GoodsName"].ToString();
                    string spec = invoiceTable.Rows[i]["Spec"].ToString();

                    View_F_GoodsPlanCost basicGoods = m_basicGoodsServer.GetGoodsInfo(code, name, spec, out error);

                    if (!GlobalObject.GeneralFunction.IsNullOrEmpty(error))
                    {
                        return false;
                    }

                    #region 改变入库表的单价(普通入库或者报检入库)

                    var varCheckOutInDepot = from a in dataContxt.S_CheckOutInDepotBill
                                 where a.Bill_ID == invoiceTable.Rows[i]["Bill_ID"].ToString()
                                 && a.GoodsID == basicGoods.序号
                                 && a.BatchNo == invoiceTable.Rows[i]["BatchNo"].ToString()
                                 select a;

                    //报检入库单单价修改
                    if (varCheckOutInDepot.Count() != 0)
                    {
                        S_CheckOutInDepotBill lnqCheckOutInDepotBill = varCheckOutInDepot.Single();

                        lnqCheckOutInDepotBill.UnitInvoicePrice = Convert.ToDecimal(invoiceTable.Rows[i]["UnitPrice"]);
                        lnqCheckOutInDepotBill.InvoicePrice = Convert.ToDecimal(invoiceTable.Rows[i]["Price"]);

                        lnqCheckOutInDepotBill.HavingInvoice = true;
                        dataContxt.SubmitChanges();
                    }
                    else
                    {
                        int intGoodsID = m_basicGoodsServer.GetGoodsID(invoiceTable.Rows[i]["GoodsCode"].ToString(),
                                                            invoiceTable.Rows[i]["GoodsName"].ToString(),
                                                            invoiceTable.Rows[i]["Spec"].ToString());

                        var varOrdinaryGoods = from a in dataContxt.S_OrdinaryInDepotGoodsBill
                                       where a.Bill_ID == invoiceTable.Rows[i]["Bill_ID"].ToString()
                                       && a.GoodsID == intGoodsID
                                       && a.BatchNo == invoiceTable.Rows[i]["BatchNo"].ToString()
                                       select a;

                        //普通入库单单价修改
                        if (varOrdinaryGoods.Count() != 0)
                        {
                            S_OrdinaryInDepotGoodsBill lnqOrdinaryGoods = varOrdinaryGoods.Single();

                            lnqOrdinaryGoods.InvoiceUnitPrice = Convert.ToDecimal(invoiceTable.Rows[i]["UnitPrice"]);
                            lnqOrdinaryGoods.InvoicePrice = Convert.ToDecimal(invoiceTable.Rows[i]["Price"]);

                            lnqOrdinaryGoods.HavingInvoice = true;
                            dataContxt.SubmitChanges();

                            int intFlag = serverOrdinaryBill.GetHavingInvoice(invoiceTable.Rows[i]["Bill_ID"].ToString(), out error);

                            if (intFlag == 4)
                            {
                                return false;
                            }
                            else
                            {
                                var varOrdinaryBill = from a in dataContxt.S_OrdinaryInDepotBill
                                               where a.Bill_ID == invoiceTable.Rows[i]["Bill_ID"].ToString()
                                               select a;

                                if (varOrdinaryBill.Count() != 0)
                                {
                                    S_OrdinaryInDepotBill lnqOrdinaryBill = varOrdinaryBill.Single();

                                    lnqOrdinaryBill.InvoiceStatus = intFlag;
                                    dataContxt.SubmitChanges();
                                }

                            }

                        }//采购退货单单价修改
                        else
                        {
                            intGoodsID = m_basicGoodsServer.GetGoodsID(invoiceTable.Rows[i]["GoodsCode"].ToString(),
                                    invoiceTable.Rows[i]["GoodsName"].ToString(),
                                    invoiceTable.Rows[i]["Spec"].ToString());

                            var varRejectList = from a in dataContxt.S_MaterialListRejectBill
                                           where a.Bill_ID == invoiceTable.Rows[i]["Bill_ID"].ToString()
                                           && a.GoodsID == intGoodsID
                                           && a.BatchNo == invoiceTable.Rows[i]["BatchNo"].ToString()
                                           select a;

                            if (varRejectList.Count() != 0)
                            {
                                S_MaterialListRejectBill lnqMaterialList = varRejectList.Single();

                                lnqMaterialList.InvoiceUnitPrice = Convert.ToDecimal(invoiceTable.Rows[i]["UnitPrice"]);
                                lnqMaterialList.InvoicePrice = Convert.ToDecimal(invoiceTable.Rows[i]["Price"]);

                                lnqMaterialList.HavingInvoice = true;
                                dataContxt.SubmitChanges();

                                int intFlag = serverMaterialRejectBill.SetHavingInvoiceReturn(invoiceTable.Rows[i]["Bill_ID"].ToString(), out error);

                                if (intFlag == 4)
                                {
                                    return false;
                                }
                                else
                                {
                                    var varReject = from a in dataContxt.S_MaterialRejectBill
                                                     where a.Bill_ID == invoiceTable.Rows[i]["Bill_ID"].ToString()
                                                     select a;

                                    if (varReject.Count() != 0)
                                    {
                                        S_MaterialRejectBill lnqMaterialBill = varReject.Single();

                                        lnqMaterialBill.InvoiceFlag = intFlag;
                                        dataContxt.SubmitChanges();
                                    }
                                }
                            }
                            else
                            {
                                intGoodsID = m_basicGoodsServer.GetGoodsID(invoiceTable.Rows[i]["GoodsCode"].ToString(),
                                                        invoiceTable.Rows[i]["GoodsName"].ToString(),
                                                        invoiceTable.Rows[i]["Spec"].ToString());

                                var varOutsourcing = from a in dataContxt.S_CheckOutInDepotForOutsourcingBill
                                                where a.Bill_ID == invoiceTable.Rows[i]["Bill_ID"].ToString()
                                                && a.GoodsID == intGoodsID
                                                && a.BatchNo == invoiceTable.Rows[i]["BatchNo"].ToString()
                                                select a;

                                //委外报检入库单单价修改
                                if (varOutsourcing.Count() != 0)
                                {
                                    S_CheckOutInDepotForOutsourcingBill lnqOutsourcing = varOutsourcing.Single();

                                    lnqOutsourcing.UnitInvoicePrice = Convert.ToDecimal(invoiceTable.Rows[i]["UnitPrice"]);
                                    lnqOutsourcing.InvoicePrice = Convert.ToDecimal(invoiceTable.Rows[i]["Price"]);

                                    lnqOutsourcing.HavingInvoice = true;
                                    dataContxt.SubmitChanges();
                                }
                            }
                        }
                    }

                    #endregion

                    #region 改变入库明细表金额
                    var varInDepotBill = from b in dataContxt.S_InDepotDetailBill
                                          where b.GoodsID == basicGoods.序号
                                          && b.InDepotBillID == invoiceTable.Rows[i]["Bill_ID"].ToString()
                                          && b.BatchNo == invoiceTable.Rows[i]["BatchNo"].ToString()
                                          select b;

                    if (varInDepotBill.Count() == 1)
                    {
                        S_InDepotDetailBill lnqInDepotBill = varInDepotBill.Single();

                        lnqInDepotBill.InvoiceUnitPrice = Convert.ToDecimal(invoiceTable.Rows[i]["UnitPrice"]);
                        lnqInDepotBill.InvoicePrice = Convert.ToDecimal(invoiceTable.Rows[i]["Price"]);

                        if (lnqInDepotBill.FactPrice != Convert.ToDecimal(invoiceTable.Rows[i]["Price"]))
                        {

                            //当查询的记录不在当月的结算日期范围内，插入红冲单据与对冲单据
                            if (lnqInDepotBill.BillTime < dtStart || lnqInDepotBill.BillTime > dtEnd)
                            {
                                var varDetail = from d in dataContxt.S_InDepotDetailBill
                                                      where d.GoodsID == basicGoods.序号
                                                      && d.InDepotBillID.Contains(invoiceTable.Rows[i]["Bill_ID"].ToString())
                                                      && d.BatchNo == invoiceTable.Rows[i]["BatchNo"].ToString()
                                                      && d.BillTime >= dtStart && d.BillTime <= dtEnd
                                                      select d;

                                //判断是否已经在当前结算日期范围内插入了红冲与对冲数据
                                if (varDetail.Count() != 0)
                                {
                                    foreach (var item in varDetail)
                                    {
                                        //针对已经插入的对冲数据进行修改
                                        if (item.InDepotBillID.Contains("(对冲单据)"))
                                        {
                                            item.FactPrice = Convert.ToDecimal(invoiceTable.Rows[i]["Price"]);
                                            item.FactUnitPrice = Convert.ToDecimal(invoiceTable.Rows[i]["UnitPrice"]);
                                        }
                                    }
                                }//对没有插入的红冲与对冲的记录进行插入
                                else
                                {
                                    //插一条原始的负记录（红冲单据）
                                    S_InDepotDetailBill lnqOldInDepotBill = new S_InDepotDetailBill();

                                    lnqOldInDepotBill.ID = Guid.NewGuid();
                                    lnqOldInDepotBill.InDepotBillID = lnqInDepotBill.InDepotBillID + "(红冲单据)";
                                    lnqOldInDepotBill.BatchNo = lnqInDepotBill.BatchNo;
                                    lnqOldInDepotBill.BillTime = ServerTime.Time;
                                    lnqOldInDepotBill.Department = lnqInDepotBill.Department;
                                    lnqOldInDepotBill.FactUnitPrice = lnqInDepotBill.FactUnitPrice;
                                    lnqOldInDepotBill.FactPrice = -lnqInDepotBill.FactPrice;
                                    lnqOldInDepotBill.FillInPersonnel = lnqInDepotBill.FillInPersonnel;
                                    lnqOldInDepotBill.GoodsID = lnqInDepotBill.GoodsID;
                                    lnqOldInDepotBill.InDepotCount = -lnqInDepotBill.InDepotCount;
                                    lnqOldInDepotBill.Price = -lnqInDepotBill.Price;
                                    lnqOldInDepotBill.OperationType = (int)GlobalObject.CE_SubsidiaryOperationType.财务红冲;
                                    lnqOldInDepotBill.Provider = lnqInDepotBill.Provider;
                                    lnqOldInDepotBill.Remark = lnqInDepotBill.Remark;
                                    lnqOldInDepotBill.StorageID = lnqInDepotBill.StorageID;
                                    lnqOldInDepotBill.UnitPrice = lnqInDepotBill.UnitPrice;
                                    lnqOldInDepotBill.FillInDate = lnqInDepotBill.FillInDate;
                                    lnqOldInDepotBill.AffrimPersonnel = lnqInDepotBill.AffrimPersonnel;

                                    IFinancialDetailManagement serverDetail =
                                        ServerModule.ServerModuleFactory.GetServerModule<IFinancialDetailManagement>();
                                    serverDetail.ProcessInDepotDetail(dataContxt, lnqOldInDepotBill, null);

                                    //插一条新的正记录（对冲单据）
                                    S_InDepotDetailBill lnqNewInDepotBill = new S_InDepotDetailBill();

                                    lnqNewInDepotBill.ID = Guid.NewGuid();
                                    lnqNewInDepotBill.InDepotBillID = lnqInDepotBill.InDepotBillID + "(对冲单据)";
                                    lnqNewInDepotBill.BatchNo = lnqInDepotBill.BatchNo;
                                    lnqNewInDepotBill.BillTime = ServerTime.Time;
                                    lnqNewInDepotBill.Department = lnqInDepotBill.Department;
                                    lnqNewInDepotBill.FactUnitPrice = Convert.ToDecimal(invoiceTable.Rows[i]["UnitPrice"]);
                                    lnqNewInDepotBill.FactPrice = Convert.ToDecimal(invoiceTable.Rows[i]["Price"]);
                                    lnqNewInDepotBill.FillInPersonnel = lnqInDepotBill.FillInPersonnel;
                                    lnqNewInDepotBill.GoodsID = lnqInDepotBill.GoodsID;
                                    lnqNewInDepotBill.InDepotCount = lnqInDepotBill.InDepotCount;
                                    lnqNewInDepotBill.Price = lnqInDepotBill.Price;
                                    lnqNewInDepotBill.OperationType = (int)GlobalObject.CE_SubsidiaryOperationType.财务对冲;
                                    lnqNewInDepotBill.Provider = lnqInDepotBill.Provider;
                                    lnqNewInDepotBill.Remark = lnqInDepotBill.Remark;
                                    lnqNewInDepotBill.StorageID = lnqInDepotBill.StorageID;
                                    lnqNewInDepotBill.UnitPrice = lnqInDepotBill.UnitPrice;
                                    lnqNewInDepotBill.FillInDate = lnqInDepotBill.FillInDate;
                                    lnqNewInDepotBill.AffrimPersonnel = lnqInDepotBill.AffrimPersonnel;

                                    serverDetail.ProcessInDepotDetail(dataContxt, lnqNewInDepotBill, null);
                                }
                            }
                            else
                            {
                                lnqInDepotBill.FactPrice = Convert.ToDecimal(invoiceTable.Rows[i]["Price"]);
                                lnqInDepotBill.FactUnitPrice = Convert.ToDecimal(invoiceTable.Rows[i]["UnitPrice"]);
                            }
                        }

                        dataContxt.SubmitChanges();
                    }
                    #endregion
                }

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }
    }
}
