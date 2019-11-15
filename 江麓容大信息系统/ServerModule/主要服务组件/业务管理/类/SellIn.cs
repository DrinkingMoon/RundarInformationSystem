/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  SellIn.cs
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
using System.Text.RegularExpressions;
using Service_Peripheral_HR;

namespace ServerModule
{
    /// <summary>
    /// 营销管理类
    /// </summary>
    class SellIn : BasicServer, ServerModule.ISellIn
    {
        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 产品箱号服务组件
        /// </summary>
        IProductCodeServer m_serverProductCode = ServerModuleFactory.GetServerModule<IProductCodeServer>();

        /// <summary>
        /// 库存服务组件
        /// </summary>
        IStoreServer m_serverStore = ServerModuleFactory.GetServerModule<IStoreServer>();

        /// <summary>
        /// 营销产品信息服务组件
        /// </summary>
        IProductListServer m_serverProductList = ServerModuleFactory.GetServerModule<IProductListServer>();

        /// <summary>
        /// 基础物品服务组件
        /// </summary>
        IBasicGoodsServer m_serverBasicGoods = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();

        /// <summary>
        /// 单据编号分配服务
        /// </summary>
        IAssignBillNoServer m_assignBill = ServerModuleFactory.GetServerModule<IAssignBillNoServer>();

        /// <summary>
        /// 人员档案服务组件
        /// </summary>
        IPersonnelArchiveServer m_personnerServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IPersonnelArchiveServer>();

        /// <summary>
        /// 部门服务组件
        /// </summary>
        IDepartmentServer m_serverDepartment = ServerModuleFactory.GetServerModule<IDepartmentServer>();

        /// <summary>
        /// 检查某业务是否存在
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">业务号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.S_MarketingBill
                          where a.DJH == billNo
                          select a;

            if (varData == null || varData.Count() == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 检查某单据是否存在
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(string billNo)
        {
            string sql = "SELECT * FROM [DepotManagement].[dbo].[S_MarketingBill] where DJH = '" + billNo + "'";

            System.Data.DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 检测单据是否存在异常箱号
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>True: 存在 False : 不存在</returns>
        public bool IsExistAbnomalProductCode(string billNo)
        {
            string error = null;

            Hashtable hsTable = new Hashtable();

            hsTable.Add("@BillNo", billNo);

            DataTable tempTable = GlobalObject.DatabaseServer.QueryInfoPro("S_Marketing_GreenLightInfo", hsTable, out error);

            if (tempTable != null && tempTable.Rows.Count > 0)
            {
                return true;
            }

            return false;
        }

        bool IsExistProductCode(string productCode, int goodsID)
        {
            using (DepotManagementDataContext ctx = CommentParameter.DepotDataContext)
            {
                var varData = from a in ctx.ProductStock
                              where a.ProductCode == productCode
                              && a.GoodsID == goodsID
                              select a;

                if (varData.Count() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 添加特殊放行记录
        /// </summary>
        /// <param name="lstInfo">对象列表</param>
        public void AddList_ProductCodesGreenLight(List<ProductsCode_GreenLight> lstInfo)
        {
            using (DepotManagementDataContext ctx = CommentParameter.DepotDataContext)
            {
                ctx.ProductsCode_GreenLight.InsertAllOnSubmit(lstInfo);
                ctx.SubmitChanges();
            }
        }

        /// <summary>
        /// 单据明细的数据库操作
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="ListInfo">明细信息</param>
        /// <param name="djID">单据ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        private bool DataInSellInList(DepotManagementDataContext context, DataTable ListInfo, int djID, out string error)
        {
            error = null;

            if (DeleteSellInList(context, djID, out error))
            {
                try
                {
                    List<S_MarketingList> lisList = new List<S_MarketingList>();

                    if (ListInfo.Rows.Count == 0)
                    {
                        return false;
                    }
                    else
                    {
                        for (int i = 0; i < ListInfo.Rows.Count; i++)
                        {
                            S_MarketingList lnqList = new S_MarketingList();

                            string strYWLX = GetBillInfo(djID).YWLX;

                            lnqList.DJ_ID = djID;
                            lnqList.CPID = ListInfo.Rows[i]["CPID"].ToString();
                            lnqList.BatchNo = ListInfo.Rows[i]["BatchNo"].ToString();
                            lnqList.UnitPrice = Convert.ToDecimal(ListInfo.Rows[i]["UnitPrice"]);
                            lnqList.Count = Convert.ToDecimal(ListInfo.Rows[i]["Count"]);
                            lnqList.Price = Convert.ToDecimal(ListInfo.Rows[i]["Price"]);
                            lnqList.ReMark = ListInfo.Rows[i]["Remark"].ToString();
                            lnqList.Provider = ListInfo.Rows[i]["Provider"].ToString();

                            if (strYWLX == "出库")
                            {
                                lnqList.SellUnitPrice = Convert.ToDecimal(ListInfo.Rows[i]["SellUnitPrice"]);
                            }
                            else if (strYWLX == "入库")
                            {
                                lnqList.Version = ListInfo.Rows[i]["Version"].ToString();
                            }
                            else if (strYWLX == "退货" && ListInfo.Rows[i]["RepairStatus"].ToString().Trim().Length > 0)
                            {
                                lnqList.RepairStatus = ListInfo.Rows[i]["RepairStatus"].ToString() == "已返修" ? true : false;
                            }

                            lisList.Add(lnqList);
                        }

                        context.S_MarketingList.InsertAllOnSubmit(lisList);
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除单据明细
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="djID">单据ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        private bool DeleteSellInList(DepotManagementDataContext context, int djID, out string error)
        {
            error = null;

            try
            {
                var varData = from a in context.S_MarketingList
                              where a.DJ_ID == djID
                              select a;

                if (varData.Count() != 0)
                {
                    context.S_MarketingList.DeleteAllOnSubmit(varData);
                    context.SubmitChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 获得单据号
        /// </summary>
        /// <returns>返回最大单据ID</returns>
        public int GetBillID()
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var idMax = (from a in ctx.S_MarketingBill
                          select a.ID).Max();

            return idMax;
        }

        /// <summary>
        /// 单据的数据库操作
        /// </summary>
        /// <param name="listInfo">明细信息</param>
        /// <param name="billInfo">单据信息，若ID=0则添加，否则更新</param>
        /// <param name="marketingType">单据类型</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool UpdateBill(DataTable listInfo, DataRow billInfo, string marketingType, out string error)
        {
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            if (Convert.ToInt32(billInfo["ID"]) == 0)//添加
            {
                try
                {
                    S_MarketingBill lnqBill = new S_MarketingBill();

                    lnqBill.DJH = billInfo["DJH"].ToString();
                    lnqBill.ObjectDept = billInfo["ObjectDept"].ToString();
                    lnqBill.LRRY = billInfo["LRRY"].ToString();
                    lnqBill.Date = Convert.ToDateTime(billInfo["Date"]);
                    lnqBill.YWLX = marketingType;
                    lnqBill.DJZT_FLAG = "已保存";
                    lnqBill.Remark = billInfo["Remark"].ToString();
                    lnqBill.Price = Convert.ToDecimal(billInfo["Price"]);
                    lnqBill.YWFS = billInfo["YWFS"].ToString();
                    lnqBill.StorageID = billInfo["StorageID"].ToString();
                    lnqBill.LRKS = billInfo["LRKS"].ToString();

                    dataContxt.S_MarketingBill.InsertOnSubmit(lnqBill);
                    dataContxt.SubmitChanges();

                    DataInSellInList(dataContxt, listInfo, GetBillID(), out error);

                    if (!DeleteRemainProductCode(dataContxt, listInfo, billInfo["DJH"].ToString(), out error))
                    {
                        return false;
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
            else //更新
            {
                try
                {
                    var varData = from a in dataContxt.S_MarketingBill
                                  where a.ID == Convert.ToInt32(billInfo["ID"])
                                  select a;

                    if (varData.Count() != 0)
                    {
                        S_MarketingBill lnqBill = varData.Single();

                        lnqBill.ObjectDept = billInfo["ObjectDept"].ToString();
                        lnqBill.LRRY = billInfo["LRRY"].ToString();
                        lnqBill.Date = Convert.ToDateTime(billInfo["Date"].ToString());
                        lnqBill.KFRY = null;
                        lnqBill.AffirmDate = null;
                        lnqBill.SHRY = null;
                        lnqBill.CheckDate = null;
                        lnqBill.JYRY = null;
                        lnqBill.ShDate = null;
                        lnqBill.Price = Convert.ToDecimal(billInfo["Price"]);
                        lnqBill.DJZT_FLAG = "已保存";
                        lnqBill.StorageID = billInfo["StorageID"].ToString();
                        lnqBill.YWFS = billInfo["YWFS"].ToString();
                        dataContxt.SubmitChanges();

                        DataInSellInList(dataContxt, listInfo, Convert.ToInt32(billInfo["ID"]), out error);

                        if (!DeleteRemainProductCode(dataContxt, listInfo, billInfo["DJH"].ToString(), out error))
                        {
                            return false;
                        }

                        dataContxt.SubmitChanges();
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

        /// <summary>
        /// 获得单据明细
        /// </summary>
        /// <param name="djID">单据ID</param>
        /// <returns>返回获取的单据明细信息</returns>
        public DataTable GetList(int djID)
        {
            string strSql = "select a.DJ_ID,a.CPID,b.图号型号 as GoodsCode,b.物品名称 as GoodsName,b.规格 as Spec," +
                            " a.Provider,b.物品类别 as Depot,a.SellUnitPrice, a.UnitPrice,a.Price,a.Count," +
                            " b.单位 as Unit,a.BatchNo,a.Remark,a.Version,case when  a.RepairStatus = 1  " +
                            " then '已返修' when a.RepairStatus = 0 then '待返修' else ''" +
                            " end as RepairStatus from S_MarketingList as a inner join " +
                            " View_F_GoodsPlanCost as b on a.CPID = b.序号 " +
                            " where DJ_ID = " + djID;

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            string djh = "";

            strSql = "select * from S_MarketingBill where ID = " + djID;

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dtTemp.Rows.Count == 0)
            {
                if (!djh.Contains("YXCK"))
                {
                    dt.Columns.Remove("SellUnitPrice");
                }
            }
            else
            {
                djh = dtTemp.Rows[0]["DJH"].ToString();

                if (!djh.Contains("YXCK"))
                {
                    dt.Columns.Remove("SellUnitPrice");
                }

            }

            return dt;
        }

        /// <summary>
        /// 权限过滤查询
        /// </summary>
        /// <param name="type">查询类型 ("入库","出库","退库","退货")</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="djzt">单据状态</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回查询到的数据信息</returns>
        public DataTable GetAllBill(string type, string startDate, string endDate, string djzt, out string error)
        {
            error = null;

            IAuthorization authorization = PlatformFactory.GetObject<IAuthorization>();
            IQueryResult qr = null;

            switch (type)
            {
                case "入库":
                    qr = authorization.Query("营销入库查询", null);
                    break;
                case "出库":
                    qr = authorization.Query("营销出库查询", null);
                    break;
                case "退库":
                    qr = authorization.Query("营销退库查询", null);
                    break;
                case "退货":
                    qr = authorization.Query("营销退货查询", null);
                    break;
                default:
                    break;
            }

            if (!qr.Succeeded)
            {
                error = qr.Error;
                return null;
            }

            DataRow[] dr;

            if (qr.DataCollection == null || qr.DataCollection.Tables.Count == 0)
            {
                return null;
            }

            if (djzt == "全  部")
            {
                dr = qr.DataCollection.Tables[0].Select("date >= '" + startDate
                    + " 00:00:00' and date <='" + endDate
                    + " 00:00:00' ", "DJH desc");
            }
            else
            {
                dr = qr.DataCollection.Tables[0].Select("date >= '" + startDate
                    + " 00:00:00' and date <='" + endDate
                    + " 00:00:00' and DJZT_Flag = '" + djzt + "' ", "DJH desc");
            }

            DataTable dt = qr.DataCollection.Tables[0].Clone();

            for (int i = 0; i < dr.Length; i++)
            {
                dt.ImportRow(dr[i]);
            }

            return dt;
        }

        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="djID">单据ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        public bool DeleteBill(int djID, out string error)
        {                   
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                string strSql = " select * from S_MarketingList as a inner join S_MarketingBill as b on a.DJ_ID = b.ID" +
                                " where DJ_ID = " + djID;
                DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

                var result = from a in dataContxt.Out_ManeuverBill
                             where a.AssociatedBillNo == dt.Rows[0]["DJH"].ToString()
                             select a;

                if (result.Count() == 1)
                {
                    if (result.Single().BillStatus != "等待出库")
                    {
                        error = "关联调运单，不能删除！";
                        return false;
                    }
                    else
                    {
                        dataContxt.Out_ManeuverBill.DeleteAllOnSubmit(result);
                    }
                }

                if (!DeleteSellInList(dataContxt, djID, out error))
                {
                    return false;
                }

                var varData = from a in dataContxt.S_MarketingBill
                              where a.ID == djID
                              select a;

                if (varData.Count() > 0)
                {
                    dataContxt.S_MarketingBill.DeleteOnSubmit(varData.Single());
                }

                if (dt.Rows.Count != 0)
                {
                    if (!DeleteProductCode(dataContxt, dt, dt.Rows[0]["DJH"].ToString(), out error))
                    {
                        return false;
                    }
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
        /// 仓管确认
        /// </summary>
        /// <param name="djID">单据ID</param>
        /// <param name="marketingType">单据类型</param>
        /// <param name="listInfo">操作的数据信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>确认成功返回True，确认失败返回False</returns>
        public bool AffrimBill(int djID, CE_MarketingType marketingType, DataTable listInfo, out string error)
        {
            error = null;
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            dataContxt.Connection.Open();
            dataContxt.Transaction = dataContxt.Connection.BeginTransaction();

            S_MarketingBill lnqMarketingBill = new S_MarketingBill();
            Out_ManeuverBill lnqManeuver = new Out_ManeuverBill();

            try
            {
                var varData = from a in dataContxt.S_MarketingBill
                              where a.ID == djID
                              select a;


                if (varData.Count() != 0)
                {
                    lnqMarketingBill = varData.Single();

                    if (!DataInSellInList(dataContxt, listInfo, Convert.ToInt32(lnqMarketingBill.ID), out error))
                    {
                        throw new Exception(error);
                    }

                    dataContxt.SubmitChanges();

                    if (lnqMarketingBill.DJZT_FLAG == "已确认")
                    {
                        error = "单据不能重复确认";
                        throw new Exception(error);
                    }

                    lnqMarketingBill.DJZT_FLAG = "已确认";
                    lnqMarketingBill.KFRY = BasicInfo.LoginID;
                    lnqMarketingBill.AffirmDate = ServerTime.Time;

                    //操作总成库存状态
                    var varList = from a in dataContxt.S_MarketingList
                                  where a.DJ_ID == lnqMarketingBill.ID
                                  select a;

                    foreach (var item in varList)
                    {
                        var varTempData = from a in dataContxt.ProductsCodes
                                          where a.DJH == lnqMarketingBill.DJH
                                          && a.GoodsID == Convert.ToInt32( item.CPID)
                                          select a;
                        foreach (var codeItem in varTempData)
                        {
                            if (!IsProductCodeOperationStandard(marketingType.ToString(), 
                                typeof(CE_MarketingType), Convert.ToInt32(item.CPID), 
                                codeItem.ProductCode, lnqMarketingBill.StorageID, out error))
                            {
                                throw new Exception(error);
                            }
                        }

                        bool blIsRepaired = false;

                        if (lnqMarketingBill.StorageID == "05" && lnqMarketingBill.YWLX == "入库")
                        {
                            blIsRepaired = true;
                        }

                        if (!m_serverProductCode.UpdateProductStock(dataContxt, lnqMarketingBill.DJH, marketingType.ToString(), 
                            lnqMarketingBill.StorageID, blIsRepaired, Convert.ToInt32(item.CPID), out error))
                        {
                            throw new Exception(error);
                        }
                    }

                    //操作明细账与库存信息
                    OperationDetailAndStock(dataContxt, lnqMarketingBill, marketingType);

                    dataContxt.SubmitChanges();

                }
                else
                {
                    error = "业务表无记录";
                    throw new Exception(error);
                }

                dataContxt.SubmitChanges();
                dataContxt.Transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                dataContxt.Transaction.Rollback();
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 获得产品返修状态
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <returns>返回产品返修状态</returns>
        bool GetConnibalizeGoodsProductStatus(string billNo, int goodsID, string batchNo)
        {
            string strSql = "select * from S_CannibalizeBill as a inner join S_CannibalizeList as b on a.ID = b.DJ_ID where DJH = '"
                + billNo + "' and GoodsID = " + goodsID + " and BatchNo = '" + batchNo + "'";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dtTemp.Rows.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 操作产品库存(仅限于调拨)
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="djh">单据号</param>
        /// <param name="marketingType">单据类型</param>
        /// <param name="outStorageID">调出库房</param>
        /// <param name="inStorageID">调入库房</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool UpdateProductStock(DepotManagementDataContext context, string djh, string marketingType, string outStorageID, string inStorageID, out string error)
        {
            try
            {
                error = null;

                string strSql = "select * from ProductsCodes where DJH = '" + djh + "'";

                DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

                #region 调入
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //更新流水码业务表的是否已确认的状态
                    var varCode = from b in context.ProductsCodes
                                  where b.ID == Convert.ToInt32(dt.Rows[i]["ID"].ToString())
                                  select b;

                    if (varCode.Count() != 1)
                    {
                        error = "数据不唯一";
                        return false;
                    }

                    ProductsCodes lnqCode = varCode.Single();

                    lnqCode.IsUse = true;

                    //更新或者添加流水码库存表
                    var varData = from a in context.ProductStock
                                  where a.ProductCode == dt.Rows[i]["ProductCode"].ToString()
                                  && a.StorageID == inStorageID
                                  && a.GoodsID == Convert.ToInt32(dt.Rows[i]["GoodsID"])
                                  //&& a.BoxNo == dt.Rows[i]["BoxNo"].ToString()
                                  select a;

                    if (varData.Count() == 0)
                    {
                        ProductStock lnqProductStock = new ProductStock();

                        lnqProductStock.StorageID = inStorageID;
                        lnqProductStock.ProductCode = dt.Rows[i]["ProductCode"].ToString();
                        lnqProductStock.ProductStatus = "调入";
                        lnqProductStock.GoodsID = Convert.ToInt32(dt.Rows[i]["GoodsID"]);
                        lnqProductStock.BoxNo = dt.Rows[i]["BoxNo"].ToString();
                        lnqProductStock.IsNatural = true;
                        lnqProductStock.RepairStatus = GetConnibalizeGoodsProductStatus(djh, Convert.ToInt32(dt.Rows[i]["GoodsID"]), "");

                        context.ProductStock.InsertOnSubmit(lnqProductStock);
                    }
                    else if (varData.Count() == 1)
                    {
                        ProductStock lnqProductStock = varData.Single();

                        lnqProductStock.ProductStatus = "调入";
                    }
                    else
                    {
                        error = "数据不唯一";
                        return false;
                    }
                }
                #endregion

                #region 调出

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //更新流水码业务表的是否已确认的状态
                    var varCode = from b in context.ProductsCodes
                                  where b.ID == Convert.ToInt32(dt.Rows[i]["ID"].ToString())
                                  select b;

                    if (varCode.Count() != 1)
                    {
                        error = "数据不唯一";
                        return false;
                    }

                    ProductsCodes lnqCode = varCode.Single();
                    lnqCode.IsUse = true;

                    //更新或者添加流水码库存表
                    var varData = from a in context.ProductStock
                                  where a.ProductCode == dt.Rows[i]["ProductCode"].ToString()
                                  && a.StorageID == outStorageID
                                  && a.GoodsID == Convert.ToInt32(dt.Rows[i]["GoodsID"])
                                  //&& a.BoxNo == dt.Rows[i]["BoxNo"].ToString()
                                  select a;

                    if (varData.Count() == 0)
                    {
                        ProductStock lnqProductStock = new ProductStock();

                        lnqProductStock.StorageID = outStorageID;
                        lnqProductStock.ProductCode = dt.Rows[i]["ProductCode"].ToString();
                        lnqProductStock.ProductStatus = "调出";
                        lnqProductStock.GoodsID = Convert.ToInt32(dt.Rows[i]["GoodsID"]);
                        lnqProductStock.BoxNo = dt.Rows[i]["BoxNo"].ToString();
                        lnqProductStock.IsNatural = true;
                        lnqProductStock.RepairStatus = GetConnibalizeGoodsProductStatus(djh, Convert.ToInt32(dt.Rows[i]["GoodsID"]), "");

                        context.ProductStock.InsertOnSubmit(lnqProductStock);
                    }
                    else if (varData.Count() == 1)
                    {
                        ProductStock lnqProductStock = varData.Single();

                        lnqProductStock.ProductStatus = "调出";
                    }
                    else
                    {
                        error = "数据不唯一";
                        return false;
                    }
                }
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 操作账务信息与库存信息
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="bill">单据信息</param>
        /// <param name="operationType">单据类型</param>
        void OperationDetailAndStock(DepotManagementDataContext context, S_MarketingBill bill, 
            CE_MarketingType operationType)
        {
            string error = "";
            IStoreServer serverStore = ServerModuleFactory.GetServerModule<IStoreServer>();
            bool blRepairStatus = true;

            var varTempData = from a in context.S_MarketingBill
                              where a.ID == bill.ID
                              select a;

            if (varTempData.Count() != 1)
            {
                throw new Exception("数据不唯一");
            }

            switch (operationType)
            {
                case CE_MarketingType.入库:
                    blRepairStatus = true;
                    OperationDetailAndStock_In(context, bill);
                    break;
                case CE_MarketingType.退货:
                    blRepairStatus = false;
                    OperationDetailAndStock_In(context, bill);
                    break;
                case CE_MarketingType.出库:
                case CE_MarketingType.退库:
                    OperationDetailAndStock_Out(context, bill);
                    break;
                default:
                    break;
            }

            var varData = from a in context.S_MarketingList
                          where a.DJ_ID == bill.ID
                          select a;

            foreach (var item in varData)
            {
                YX_AfterServiceStock lnqAfterSerive = new YX_AfterServiceStock();

                lnqAfterSerive.GoodsID = Convert.ToInt32(item.CPID);
                lnqAfterSerive.OperationCount = blRepairStatus == true ? item.Count : -item.Count;
                lnqAfterSerive.RepairStatus = blRepairStatus == true ? blRepairStatus : Convert.ToBoolean(item.RepairStatus);
                lnqAfterSerive.StorageID = varTempData.Single().StorageID;

                if (!serverStore.OperationYXAfterService(context, lnqAfterSerive, out error))
                {
                    throw new Exception();
                }
            }
        }

        /// <summary>
        /// 赋值账务信息_入
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="bill">单据信息</param>
        /// <param name="item">明细信息</param>
        /// <returns>返回账务信息对象</returns>
        S_InDepotDetailBill AssignDetailInfo_In(DepotManagementDataContext context, S_MarketingBill bill, View_S_MarketingBill_V3 item)
        {
            S_InDepotDetailBill lnqInDepot = new S_InDepotDetailBill();

            lnqInDepot.ID = Guid.NewGuid();
            lnqInDepot.InDepotBillID = item.DJH;
            lnqInDepot.BillTime = ServerTime.Time;
            lnqInDepot.GoodsID = Convert.ToInt32(item.CPID);
            lnqInDepot.Provider = item.Provider;
            lnqInDepot.UnitPrice = item.UnitPrice;
            lnqInDepot.Price = item.Price;
            lnqInDepot.InDepotCount = item.YWLX == "入库" ? item.Count : -item.Count;
            lnqInDepot.Department = item.ObjectName;
            lnqInDepot.FillInPersonnel = item.LRRY;
            lnqInDepot.OperationType = item.YWLX == "入库" ? 
                (int)CE_SubsidiaryOperationType.营销入库 : (int)CE_SubsidiaryOperationType.营销退货;
            lnqInDepot.FactPrice = Math.Round(item.UnitPrice * Convert.ToDecimal(lnqInDepot.InDepotCount), 2);
            lnqInDepot.FactUnitPrice = item.UnitPrice;
            lnqInDepot.Remark = item.ReMark;
            lnqInDepot.BatchNo = item.BatchNo;
            lnqInDepot.StorageID = item.StorageID;
            lnqInDepot.FillInDate = item.Date;
            lnqInDepot.AffrimPersonnel = item.KFRY;

            return lnqInDepot;
        }

        /// <summary>
        /// 赋值账务信息_出
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="bill">单据信息</param>
        /// <param name="item">明细信息</param>
        /// <returns>返回账务信息对象</returns>
        S_FetchGoodsDetailBill AssignDetailInfo_Out(DepotManagementDataContext context, S_MarketingBill bill, View_S_MarketingBill_V3 item)
        {
            S_FetchGoodsDetailBill lnqOutDepot = new S_FetchGoodsDetailBill();

            lnqOutDepot.ID = Guid.NewGuid();
            lnqOutDepot.FetchBIllID = item.DJH;
            lnqOutDepot.BillTime = ServerTime.Time;
            lnqOutDepot.GoodsID = Convert.ToInt32(item.CPID);
            lnqOutDepot.Provider = item.Provider;
            lnqOutDepot.ProviderBatchNo = "";
            lnqOutDepot.BatchNo = item.BatchNo;
            lnqOutDepot.UnitPrice = item.UnitPrice;
            lnqOutDepot.FetchCount = item.YWLX == "出库" ? item.Count : -item.Count;
            lnqOutDepot.Price = item.YWLX == "出库" ? item.Price : -item.Price;
            lnqOutDepot.Using = "营销" + item.YWFS;
            lnqOutDepot.Department = item.Department;
            lnqOutDepot.FillInPersonnel = item.LRRY;
            lnqOutDepot.DepartDirector = item.SHRY;
            lnqOutDepot.DepotManager = item.KFRY;
            lnqOutDepot.OperationType = item.YWLX == "出库" ? 
                (int)CE_SubsidiaryOperationType.营销出库 : (int)CE_SubsidiaryOperationType.营销退库;
            lnqOutDepot.Remark = item.ReMark;
            lnqOutDepot.StorageID = item.StorageID;
            lnqOutDepot.FillInDate = item.Date;

            return lnqOutDepot;
        }

        /// <summary>
        /// 赋值库存信息
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="tempBillLnq">单据信息</param>
        /// <param name="item">明细信息</param>
        /// <returns>返回库存信息对象</returns>
        S_Stock AssignStockInfo(DepotManagementDataContext context, S_MarketingBill tempBillLnq, View_S_MarketingBill_V3 item)
        {
            S_Stock tempLnqStock = new S_Stock();

            tempLnqStock.GoodsID = Convert.ToInt32(item.CPID);
            tempLnqStock.BatchNo = item.BatchNo;
            tempLnqStock.UnitPrice = item.UnitPrice;

            if (tempBillLnq.YWLX == "入库" || tempBillLnq.YWLX == "退库")
            {
                tempLnqStock.ExistCount = item.Count;
            }
            else
            {
                tempLnqStock.ExistCount = -item.Count;
            }

            tempLnqStock.ExistCount = item.Count;
            tempLnqStock.Date = ServerTime.Time;
            tempLnqStock.Provider = item.Provider;
            tempLnqStock.StorageID = tempBillLnq.StorageID;
            tempLnqStock.InputPerson = BasicInfo.LoginID;

            return tempLnqStock;
        }

        /// <summary>
        /// 根据单据信息操作账务信息与库存信息_出
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="bill">单据信息</param>
        public void OperationDetailAndStock_Out(DepotManagementDataContext dataContext, S_MarketingBill bill)
        {
            IFinancialDetailManagement serverDetail =
                ServerModule.ServerModuleFactory.GetServerModule<IFinancialDetailManagement>();

            var result = from r in dataContext.View_S_MarketingBill_V3
                         where r.DJH == bill.DJH
                         select r;

            foreach (var item in result)
            {
                S_FetchGoodsDetailBill detailInfo = AssignDetailInfo_Out(dataContext, bill, item);
                S_Stock stockInfo = AssignStockInfo(dataContext, bill, item);

                if (detailInfo == null || stockInfo == null)
                {
                    throw new Exception("获取账务信息或者库存信息失败");
                }

                serverDetail.ProcessFetchGoodsDetail(dataContext, detailInfo, stockInfo);
            }
        }

        /// <summary>
        /// 根据单据信息操作账务信息与库存信息_入
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="bill">单据信息</param>
        public void OperationDetailAndStock_In(DepotManagementDataContext dataContext, S_MarketingBill bill)
        {
            IFinancialDetailManagement serverDetail =
                ServerModule.ServerModuleFactory.GetServerModule<IFinancialDetailManagement>();

            var result = from r in dataContext.View_S_MarketingBill_V3
                         where r.DJH == bill.DJH
                         select r;

            foreach (var item in result)
            {
                S_InDepotDetailBill detailInfo = AssignDetailInfo_In(dataContext, bill, item);
                S_Stock stockInfo = AssignStockInfo(dataContext, bill, item);

                if (detailInfo == null || stockInfo == null)
                {
                    throw new Exception("获取账务信息或者库存信息失败");
                }

                serverDetail.ProcessInDepotDetail(dataContext, detailInfo, stockInfo);
            }
        }

        /// <summary>
        /// 变更单据状态（审核）
        /// </summary>
        /// <param name="djID">单据ID</param>
        /// <param name="remark">备注</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回false</returns>
        public bool AuditingBill(int djID, string remark, out string error)
        {
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;
            dataContxt.Connection.Open();
            dataContxt.Transaction = dataContxt.Connection.BeginTransaction();

            try
            {
                var varData = from a in dataContxt.S_MarketingBill
                              where a.ID == djID
                              select a;

                if (varData.Count() == 0)
                {
                    error = "无记录";
                }
                else
                {
                    S_MarketingBill lnqMarkBill = varData.Single();

                    lnqMarkBill.SHRY = BasicInfo.LoginID;
                    lnqMarkBill.Remark = remark;
                    lnqMarkBill.ShDate = ServerTime.Time;
                    lnqMarkBill.DJZT_FLAG = "已审核";

                    CE_MarketingType tempEnum = GlobalObject.GeneralFunction.StringConvertToEnum<CE_MarketingType>(lnqMarkBill.YWLX);

                    switch (tempEnum)
                    {
                        case CE_MarketingType.出库:
                            string billNo = GetBill("", djID).Rows[0]["DJH"].ToString();
                            DataTable dt = new BaseModule_Economic.CommonClass().GetDataByAssociatedNo(billNo);
                            if (dt != null && dt.Rows.Count > 0 && dt.Rows[0]["单据状态"].ToString() != "已完成")
                            {
                                lnqMarkBill.DJZT_FLAG = "等待财务审核";

                            }
                            break;
                        case CE_MarketingType.入库:
                            IProductDeliveryInspectionServer serverDeliveryInSpection = 
                                ServerModuleFactory.GetServerModule<IProductDeliveryInspectionServer>();

                            if (!serverDeliveryInSpection.ManageDeliveryInspection(dataContxt, lnqMarkBill.DJH, out error))
                            {
                                throw new Exception(error);
                            }

                            break;
                        case CE_MarketingType.退库:
                            var varManeuver = from a in dataContxt.Out_ManeuverBill
                                              where a.AssociatedBillNo == lnqMarkBill.DJH
                                              select a;

                            if (varManeuver.Count() == 1)
                            {
                                Out_ManeuverBill lnqManeuver = varManeuver.Single();

                                lnqManeuver.BillStatus = "等待出库";
                                lnqManeuver.Verify = BasicInfo.LoginName;
                                lnqManeuver.VerifyTime = ServerTime.Time;

                                m_billMessageServer.PassFlowMessage(lnqManeuver.Bill_ID,
                                    string.Format("{0}号调运单已审核，请发货方出库", lnqManeuver.Bill_ID), BillFlowMessage_ReceivedUserType.用户,
                                    UniversalFunction.GetStorageOrStationPrincipal(lnqManeuver.OutStorageID));
                            }
                            break;
                        default:
                            break;
                    }

                    dataContxt.SubmitChanges();
                }

                dataContxt.Transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                dataContxt.Transaction.Rollback();
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 获得单据信息
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <param name="djID">单据ID</param>
        /// <returns>返回单据信息</returns>
        public DataTable GetBill(string djh, int djID)
        {
            string strSql = "select * from S_MarketingBill where DJH = '" + djh + "' or ID = " + djID;

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dt;
        }

        /// <summary>
        /// 获得批次号
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="marketingType">单据类型</param>
        /// <returns>返回批次号</returns>
        public string GetBatchNo(int goodsID, string marketingType)
        {
            string strMark = "";
            string strNewordernumber = "";

            if (marketingType == "退货")
            {
                strMark = "TH";
            }

            if (Convert.ToBoolean(UniversalFunction.GetGoodsAttributeInfo(goodsID, CE_GoodsAttributeName.TCU)))
            {
                strNewordernumber = strMark + "TCU" + ServerTime.Time.Year.ToString() + ServerTime.Time.Month.ToString("D2");

                string strSql = "select max(BatchNo) from S_MarketingList where BatchNo like '"
                            + strNewordernumber +
                            "%' and CPID = '" + goodsID + "'";

                DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

                if (dt.Rows[0][0].ToString() == "")
                {
                    strNewordernumber = strNewordernumber + "0001";
                }
                else
                {
                    string strTemp = dt.Rows[0][0].ToString();
                    int len = strTemp.Length;
                    string strSet = strTemp.Substring(0, 9 + strMark.Length);

                    strTemp = (Convert.ToInt32(strTemp.Substring(9 + strMark.Length, 4)) + 1).ToString("D4");
                    strNewordernumber = strSet + strTemp;
                }
            }
            else
            {
                strNewordernumber = strMark + "CP" + ServerTime.Time.Year.ToString() + ServerTime.Time.Month.ToString("D2");

                string sSql = "select max(BatchNo) from S_MarketingList where BatchNo like '"
                            + strNewordernumber +
                            "%' and CPID = '" + goodsID + "'";

                DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sSql);

                if (dt.Rows[0][0].ToString() == "")
                {
                    strNewordernumber = strNewordernumber + "0001";
                }
                else
                {
                    string strTemp = dt.Rows[0][0].ToString();
                    int len = strTemp.Length;
                    string strSet = strTemp.Substring(0, 8 + strMark.Length);

                    strTemp = (Convert.ToInt32(strTemp.Substring(8 + strMark.Length, 4)) + 1).ToString("D4");
                    strNewordernumber = strSet + strTemp;
                }
            }

            return strNewordernumber;
        }

        /// <summary>
        /// 获得箱子批次
        /// </summary>
        /// <param name="prefix">前缀</param>
        /// <returns>返回箱体批次号</returns>
        public string GetBoxNo(string prefix)
        {
            string strNewnumber = "";

            string strSql = "select Max(BoxNo) from ProductsCodes where BoxNo like '" + prefix + "" + ServerTime.Time.Year.ToString() + "%'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dt.Rows.Count == 0)
            {
                strNewnumber = prefix + ServerTime.Time.Year.ToString() + ServerTime.Time.Month.ToString("D2") + "0001";
            }
            else
            {
                if (dt.Rows[0][0].ToString() == "")
                {
                    strNewnumber = prefix + ServerTime.Time.Year.ToString() + ServerTime.Time.Month.ToString("D2") + "0001";
                }
                else
                {
                    int intMax = Convert.ToInt32(dt.Rows[0][0].ToString().Substring(9, 4)) + 1;

                    strNewnumber = prefix + ServerTime.Time.Year.ToString() + ServerTime.Time.Month.ToString("D2") + intMax.ToString("D4");
                }
            }

            return strNewnumber;
        }

        /// <summary>
        /// 删除流水码表中的数据
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="showTable">信息列表</param>
        /// <param name="djh">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        private bool DeleteProductCode(DepotManagementDataContext context, DataTable showTable, string djh, out string error)
        {
            error = null;

            try
            {
                for (int i = 0; i < showTable.Rows.Count; i++)
                {
                    var varData = from a in context.ProductsCodes
                                  where a.GoodsID == Convert.ToInt32(showTable.Rows[i]["CPID"])
                                  && a.DJH == djh
                                  //单据号为批次号
                                  select a;

                    if (varData.Count() != 0)
                    {
                        context.ProductsCodes.DeleteAllOnSubmit(varData);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 删除多余的流水码信息
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="showTable">信息列表</param>
        /// <param name="djh">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        public bool DeleteRemainProductCode(DepotManagementDataContext context, DataTable showTable, string djh, out string error)
        {
            error = null;

            try
            {
                string strSql = "select distinct GoodsID from ProductsCodes where DJH = '" + djh + "'";
                DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow[] dr = showTable.Select("CPID = '" + Convert.ToInt32(dt.Rows[i]["GoodsID"]) + "'");

                    if (dr.Length == 0)
                    {
                        var varData = from a in context.ProductsCodes
                                      where a.GoodsID == Convert.ToInt32(dt.Rows[i]["GoodsID"])
                                      && a.DJH == djh
                                      select a;

                        context.ProductsCodes.DeleteAllOnSubmit(varData);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 编辑检验状态
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <param name="remark">备注</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>编辑成功返回True，编辑失败返回False</returns>
        public bool ExamineBill(string djh, string remark, out string error)
        {
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                var varData = from a in dataContxt.S_MarketingBill
                              where a.DJH == djh
                              select a;

                if (varData.Count() != 0)
                {
                    S_MarketingBill lnqMarketing = varData.Single();

                    lnqMarketing.DJZT_FLAG = "已检验";
                    lnqMarketing.JYRY = BasicInfo.LoginID;
                    lnqMarketing.Remark = remark;
                    lnqMarketing.CheckDate = ServerTime.Time;

                    dataContxt.SubmitChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 编辑复审状态
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <param name="remark">备注</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>编辑成功返回True，编辑失败返回False</returns>
        public bool RetrialBill(string djh, string remark, out string error)
        {
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                var varData = from a in dataContxt.S_MarketingBill
                              where a.DJH == djh
                              select a;

                if (varData.Count() != 0)
                {
                    S_MarketingBill lnqMarketing = varData.Single();

                    if (lnqMarketing.YWLX == "出库")
                    {
                        lnqMarketing.DJZT_FLAG = "已审核";
                    }
                    else
                    {
                        lnqMarketing.DJZT_FLAG = "已复审";
                    }

                    lnqMarketing.FSRY = BasicInfo.LoginID;
                    lnqMarketing.Remark = remark;
                    lnqMarketing.FSRQ = ServerTime.Time;

                    dataContxt.SubmitChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 获得单条单据信息
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="djID">单据ID</param>
        /// <returns>返回单条单据信息</returns>
        S_MarketingBill GetBillInfo(DepotManagementDataContext ctx, int djID)
        {
            var varData = from a in ctx.S_MarketingBill
                          where a.ID == djID
                          select a;

            if (varData.Count() != 1)
            {
                throw new Exception("单据信息不存在或者不唯一");
            }
            else
            {
                return varData.Single();
            }
        }

        /// <summary>
        /// 获得单条单据信息
        /// </summary>
        /// <param name="djID">单据ID</param>
        /// <returns>返回单条单据信息</returns>
        S_MarketingBill GetBillInfo(int djID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.S_MarketingBill
                          where a.ID == djID
                          select a;

            if (varData.Count() != 1)
            {
                throw new Exception("单据信息不存在或者不唯一");
            }
            else
            {
                return varData.Single();
            }
        }

        /// <summary>
        /// 检查库存产品返修状态是否为已返修
        /// </summary>
        /// <param name="productCode">产品编号</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>为已返修返回True,不在库房或者不为已返修返回False</returns>
        public bool IsRepaired(string productCode, int goodsID, string storageID)
        {
            string strSql = "select * from ProductStock where ProductCode = '" + productCode
                    + "' and StorageID = '"
                    + storageID + "' and GoodsID = " + goodsID;

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dt.Rows.Count == 0)
            {
                return false;
            }
            else
            {
                if (Convert.ToBoolean(dt.Rows[0]["RepairStatus"]))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 检查库存产品状态是否为正常
        /// </summary>
        /// <param name="productCode">产品编号</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>为正常返回True，不在库房或者非正常返回False</returns>
        public bool IsNatural(string productCode, int goodsID, string storageID)
        {
            string strSql = "select * from ProductStock where ProductCode = '" + productCode
                + "' and StorageID = '"
                + storageID + "' and GoodsID = " + goodsID;

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dt.Rows.Count == 0)
            {
                return false;
            }
            else
            {
                if (Convert.ToBoolean(dt.Rows[0]["IsNatural"]))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 查询对应的编码状态
        /// </summary>
        /// <param name="storageID">库房ID</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="type">查询类型 ("库房","下线","TCU")</param>
        /// <param name="productCode">产品编码</param>
        /// <param name="version">版次号</param>
        /// <returns>返回查询结果集</returns>
        public DataTable GetStockProductCodeInfo(string storageID, int goodsID, string type, string productCode, string version)
        {
            //string strSql = "select b.图号型号 as 产品型号,a.ProductCode as 产品编码,ProductStatus as 产品来源, " +
            //    " YWFS as 业务方式, AffirmDate as 产品来源日期," +
            //    " IsNatural as 是否正常,StorageName as 库存名称,a.GoodsID as 产品ID,a.StorageID as 库房ID, " +
            //    " case when RepairStatus = 1 then '已返修' else '待返修' end as 返修状态  " +
            //    " from (select ProductCode,GoodsID,StorageID,YWFS,IsNatural,AffirmDate,ProductStatus,Version,RepairStatus  from  " +
            //    " (select a.ProductCode,a.GoodsID,a.StorageID,case when AffirmDate is null then '1990-1-1' else AffirmDate end as AffirmDate," +
            //    " case when YWFS is null then '未知' else YWFS end as YWFS,c.DJH,IsNatural,ProductStatus,Version,RepairStatus  " +
            //    " from ProductStock as a left join ProductsCodes as b on a.GoodsID = b.GoodsID  " +
            //    " and a.ProductCode = b.ProductCode " +
            //    " left join (select AffirmDate,YWFS,DJH,StorageID from S_MarketingBill " +
            //    " where DJZT_Flag = '已确认'  union all " +
            //    " select  Bill_Time,'领料退库',Bill_ID,StorageID from S_MaterialReturnedInTheDepot " +
            //    " where BillStatus = '已完成' union all " +
            //    " select  Bill_Time,'领料',Bill_ID,StorageID from S_MaterialRequisition " +
            //    " where BillStatus = '已出库' union all " +
            //    " select KFRQ,'调入',DJH,InStoreRoom from S_CannibalizeBill " +
            //    " where DJZT = '已确认' union all " +
            //    " select KFRQ,'调出',DJH,OutStoreRoom from S_CannibalizeBill " +
            //    " where DJZT = '已确认') as c on b.DJH = c.DJH and c.StorageID = a.StorageID  " +
            //    " where a.StorageID = '" + storageID + "' and a.GoodsID = " + goodsID + ") as a " +
            //    " where AffirmDate = (select Max(AffirmDate) from ( " +
            //    " select a.ProductCode,a.GoodsID,a.StorageID,case when AffirmDate is null then '1990-1-1' else AffirmDate end as AffirmDate," +
            //    " case when YWFS is null then '未知' else YWFS end as YWFS,c.DJH ,IsNatural,ProductStatus,Version  " +
            //    " from ProductStock as a left join ProductsCodes as b on a.GoodsID = b.GoodsID  " +
            //    " and a.ProductCode = b.ProductCode" +
            //    " left join (select AffirmDate,YWFS,DJH,StorageID from S_MarketingBill " +
            //    " where DJZT_Flag = '已确认'  union all " +
            //    " select  Bill_Time,'领料退库',Bill_ID,StorageID from S_MaterialReturnedInTheDepot " +
            //    " where BillStatus = '已完成' union all " +
            //    " select  Bill_Time,'领料',Bill_ID,StorageID from S_MaterialRequisition " +
            //    " where BillStatus = '已出库' union all " +
            //    " select KFRQ,'调入',DJH,InStoreRoom from S_CannibalizeBill " +
            //    " where DJZT = '已确认' union all " +
            //    " select KFRQ,'调出',DJH,OutStoreRoom from S_CannibalizeBill " +
            //    " where DJZT = '已确认') as c on b.DJH = c.DJH and c.StorageID = a.StorageID  " +
            //    " where a.StorageID = '" + storageID + "' and a.GoodsID = " + goodsID + ") as b  " +
            //    " where b.ProductCode = a.ProductCode and b.GoodsID = a.GoodsID  " +
            //    " and b.StorageID = a.StorageID)) as a inner join View_F_GoodsPlanCost as b on a.GoodsID = b.序号 " +
            //    " inner join Base_Storage as c on a.StorageID = c.StorageID where 1=1 ";

            //if (productCode != "")
            //{
            //    strSql += " and a.ProductCode = '" + productCode + "'";
            //}
            //else
            //{
            //    if (type == "库房")
            //    {
            //        strSql += " and ProductStatus in ('入库','退库','领料退库','调入') ";
            //    }
            //    else if (type == "下线")
            //    {
            //        strSql += " and ProductStatus = '退货'";
            //    }
            //    else if (type == "TCU")
            //    {
            //        strSql += " and Version = '" + version + "'";
            //    }
            //}

            string error = null;

            Hashtable hsTable = new Hashtable();

            hsTable.Add("@StorageID", storageID);
            hsTable.Add("@GoodsID", goodsID);
            hsTable.Add("@Type", type);
            hsTable.Add("@ProductCode", productCode);
            hsTable.Add("@Version", version);

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfoPro("ProductCode_IntegratedQuery", hsTable, out error);

            return dtTemp;
        }

        /// <summary>
        /// 获得下线库存
        /// </summary>
        /// <param name="storageName">库房名称</param>
        /// <returns>返回查询的下线库存信息</returns>
        public DataTable GetInsertingCoilStockInfo(string storageName)
        {
            string strStorageID = "";

            if (storageName.Contains("售后"))
            {
                strStorageID = "05";
            }
            else if (storageName.Contains("成品"))
            {
                strStorageID = "02";
            }

            string strSql = "select b.物品名称 as 产品名称,b.图号型号 as 产品型号,b.规格," +
                " Count(*) as 数量,StorageName as 库房名称,a.GoodsID as 物品ID " +
                " from ProductStock as a  " +
                " inner join View_F_GoodsPlanCost as b on a.GoodsID = b.序号 " +
                " inner join BASE_Storage as c on a.StorageID = c.StorageID " +
                " where ProductStatus = '退货' and a.StorageID = '" + strStorageID + "' " +
                " group by a.GoodsID, b.图号型号, b.物品名称, b.规格, StorageName, a.StorageID";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dtTemp;
        }

        /// <summary>
        /// 获得库存的产品编号总数
        /// </summary>
        /// <param name="storageID">库房ID</param>
        /// <returns>返回查询到的库存产品编号信息</returns>
        public DataTable GetStockProductCodeCountInfo(string storageID)
        {
            string strSql = "select distinct b.物品名称 as 产品名称,b.图号型号 as 产品型号, b.规格, " +
                    " Count(ProductCode) as 数量,StorageName as 库房名称, a.GoodsID as 物品ID from ProductStock " +
                    " as a inner join View_F_GoodsPlanCost as b on a.GoodsID = b.序号 " +
                    " inner join BASE_Storage as c on a.StorageID = c.StorageID " +
                    " where ProductStatus in ('入库','退库','领料退库','调入') ";

            if (storageID != "")
            {
                strSql += " and a.StorageID = '" + storageID + "'";
            }

            strSql += " Group by a.StorageID, b.物品名称 ,b.图号型号,b.规格, StorageName, a.GoodsID";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dt;
        }

        /// <summary>
        /// 查询出厂检验数据
        /// </summary>
        /// <param name="productCode">产品编号</param>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回查询的出厂检验信息</returns>
        public DataTable GetDeliveryInspectionInfo(string productCode, int goodsID)
        {
            string strSql = "select a.检测项目, 技术要求, b.不合格情况 as 检测情况, 判定 " +
                " from View_P_DeliveryInspectionItems as a inner join " +
                " View_P_DeliveryInspection as b on a.单据号 = b.单据号 " +
                " inner join View_F_GoodsPlanCost as c on c.图号型号 = b.产品型号 " +
                " where c.序号 = " + goodsID + " and b.产品编号 = '" + productCode + "'";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 查询产品编号的业务
        /// </summary>
        /// <param name="productCode">产品编号</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>返回查询到的产品编号业务信息</returns>
        public DataTable GetProductCodeOperationInfo(string productCode, int goodsID, string storageID)
        {
            string strSql = @" select *, dbo.fun_get_StorageName(库房代码) as 业务库房 from  
                            (select b.DJH as 单据号,ProductCode as 箱体编码, YWFS as 业务方式,YWLX as 业务类型,   
		                            objectDept as 对象部门,lrks as 录入科室,DJZT_FLAG as 单据状态,  lrry as 录入人,  
		                            AffirmDate as 单据日期,GoodsID as 物品ID ,IsUse ,库房代码 
                            from ProductsCodes as a    inner join View_S_MarketingBill as b on a.DJH = b.DJH  
                            union all
                            select b.DJH as 单据号,ProductCode as 箱体编码, '调拨' as 业务方式,'调入' as 业务类型,   
		                            InStoreRoom as 对象部门,OutStoreRoom as 录入科室,DJZT as 单据状态,  LRRY as 录入人,  
		                            KFRQ as 单据日期,b.GoodsID as 物品ID ,IsUse,c.StorageID  
                            from ProductsCodes as a inner join View_S_Cannibalize as b inner join dbo.BASE_Storage as c 
                            on b.InStoreRoom = c.StorageName on a.DJH = b.DJH 
                            union all
                            select b.DJH as 单据号,ProductCode as 箱体编码, '调拨' as 业务方式,'调出' as 业务类型,   
		                            OutStoreRoom as 对象部门,InStoreRoom as 录入科室,DJZT as 单据状态,  LRRY as 录入人,  
		                            KFRQ as 单据日期,b.GoodsID as 物品ID ,IsUse,c.StorageID  
                            from ProductsCodes as a inner join View_S_Cannibalize as b inner join dbo.BASE_Storage as c 
                            on b.OutStoreRoom = c.StorageName on a.DJH = b.DJH 
                            union all  
                            select  领料单号 as 单据号,ProductCode as 箱体编码,   '领料' as 业务方式,'领料' as 业务类型,   
		                            库房名称 as 对象部门,部门名称 as 录入科室,单据状态,  编制人 as 录入人,  出库时间 as 单据日期,
		                            GoodsID as 物品ID ,IsUse ,库房代码
                            from ProductsCodes as a   inner join View_S_MaterialRequisition as b on a.DJH = b.领料单号 
                            union all  
                            select  退库单号 as 单据号,ProductCode as 箱体编码,   '领料退库' as 业务方式,'领料退库' as 业务类型,   
		                            库房名称 as 对象部门,申请部门 as 录入科室,单据状态,  申请人 as 录入人,  
		                            退库时间 as 单据日期,GoodsID as 物品ID ,IsUse ,库房代码
                            from ProductsCodes as a    inner join View_S_MaterialReturnedInTheDepot as b on a.DJH = b.退库单号
                            union all  
                            select  报废单号 as 单据号,ProductCode as 箱体编码,   '报废' as 业务方式,'报废' as 业务类型, ";

            strSql += " dbo.fun_get_StorageName('" + storageID + "') as 对象部门,申请部门 as 录入科室,单据状态,  申请人签名 as 录入人, " +
                " 报废时间 as 单据日期, GoodsID as 物品ID ,IsUse ,'" + storageID + "' as  库房代码 from ProductsCodes as a   inner join View_S_ScrapBill " +
                " as b on a.DJH = b.报废单号) as a   where 物品ID = " + goodsID + " and 箱体编码 = '" + productCode + "'and IsUse = 1 ";

            if (storageID != "")
            {
                strSql += "  and 库房代码 = '" + storageID + "' ";
            }

            strSql += " order by 单据日期 ";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            dt.Columns.Remove("物品ID");
            dt.Columns.Remove("IsUse");

            return dt;
        }

        /// <summary>
        /// 判断是否打印单据
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <returns>已打印返回True，未打印返回False</returns>
        public bool IsPrint(string djh)
        {
            string strSql = "select * from S_PrintBillTable where Bill_ID = '" + djh + "' and PrintFlag = 1";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dt != null && dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获得整盒的TCU
        /// </summary>
        /// <param name="boxNo">TCU盒号</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>返回查询到的整盒TCU信息</returns>
        public DataTable GetBoxInfo(string boxNo, string storageID)
        {
            string strSql = "select distinct ProductCode, BoxNo from ProductStock where BoxNo = '" + boxNo 
                + "' and ProductStatus in ('入库','退库','领料退库','调入', '出库') and StorageID = '" + storageID + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dt;
        }

        /// <summary>
        /// 获得箱子编号
        /// </summary>
        /// <param name="productCode">产品编号</param>
        /// <returns>返回查询到的箱子编号</returns>
        public string GetHoldBoxNo(string productCode)
        {
            string strSql = "select * from ProductStock where ProductCode = '" + productCode + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dt.Rows.Count == 0)
            {
                return "";
            }
            else
            {
                return dt.Rows[0]["BoxNo"].ToString();
            }
        }

        /// <summary>
        /// 获得外部库存数
        /// </summary>
        /// <returns>返回查询的外部库存数的信息</returns>
        public DataTable GetOutStockInfo()
        {
            string strSql = "select * from View_S_OutStock";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dtTemp;
        }

        /// <summary>
        /// 删除一条外部库存记录
        /// </summary>
        /// <param name="outStock">外部库存信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        public bool DeleteOutStockInfo(S_OutStock outStock, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.S_OutStock
                              where a.GoodsID == outStock.GoodsID
                              && a.StorageID == outStock.StorageID
                              select a;

                dataContext.S_OutStock.DeleteAllOnSubmit(varData);

                dataContext.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 更新外部库存数
        /// </summary>
        /// <param name="outStock">外部库存信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>更新成功返回True，更新失败返回False</returns>
        public bool UpdateOutStockInfo(S_OutStock outStock, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.S_OutStock
                              where a.GoodsID == outStock.GoodsID
                              && a.StorageID == outStock.StorageID
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数据为空或者不唯一";
                    return false;
                }
                else
                {
                    S_OutStock lnqSotck = varData.Single();

                    lnqSotck.Stock = outStock.Stock;

                    if (!InsertTheUpdateOutStock(dataContext, outStock, out error))
                    {
                        return false;
                    }

                    dataContext.SubmitChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 添加外部库存数据
        /// </summary>
        /// <param name="outStock">外部库存信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>添加成功返回True，添加失败返回False</returns>
        public bool AddOutStockInfo(S_OutStock outStock, out string error)
        {
            error = null;
            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.S_OutStock
                              where a.GoodsID == outStock.GoodsID
                              && a.StorageID == outStock.StorageID
                              select a;

                if (varData.Count() != 0)
                {
                    error = "不能重复添加数据";
                    return false;
                }
                else
                {

                    dataContext.S_OutStock.InsertOnSubmit(outStock);

                    if (!InsertTheUpdateOutStock(dataContext, outStock, out error))
                    {
                        return false;
                    }

                    dataContext.SubmitChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 插入更新的数据记录
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="outStock">外部库存信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>插入成功返回True，插入失败返回False</returns>
        private bool InsertTheUpdateOutStock(DepotManagementDataContext context, S_OutStock outStock, out string error)
        {
            error = null;

            try
            {
                S_UpdateOutStock lnqUpdate = new S_UpdateOutStock();

                lnqUpdate.GoodsID = outStock.GoodsID;
                lnqUpdate.StockCount = outStock.Stock;
                lnqUpdate.StorageID = outStock.StorageID;
                lnqUpdate.UpdatePersonnel = BasicInfo.LoginName;
                lnqUpdate.UpdateTime = ServerTime.Time;

                context.S_UpdateOutStock.InsertOnSubmit(lnqUpdate);
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 获得更新的外部库存数
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>返回查询到的外部库存数的信息</returns>
        public DataTable GetOutStockInfo(int goodsID, string storageID)
        {
            string strSql = "select Row_Number()Over(order by ID) as 序号,StockCount" +
                " as 变更数量,UpdatePersonnel as 变更人,UpdateTime as 变更日期 " +
                " from S_UpdateOutStock where GoodsID = " + goodsID + " and StorageID = '" + storageID + "'";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dtTemp;
        }

        /// <summary>
        /// 当退出界面时删除已提交未保存的产品编码
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        public bool DeleteProductCodeInfo(string djh, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.S_MarketingBill
                              where a.DJH == djh
                              select a;

                if (varData.Count() == 0)
                {
                    var varProductCode = from a in dataContext.ProductsCodes
                                         where a.DJH == djh
                                         select a;

                    dataContext.ProductsCodes.DeleteAllOnSubmit(varProductCode);
                    dataContext.SubmitChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 改变产品编码业务表中的关联单据号
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="oldDJH">老单据号</param>
        /// <param name="newDJH">新单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>变更成功返回True，变更失败返回False</returns>
        private bool ChangeDJHInProductCode(DepotManagementDataContext context, string oldDJH, string newDJH, out string error)
        {
            error = null;
            try
            {
                var varData = from a in context.ProductsCodes
                              where a.DJH == oldDJH
                              select a;

                foreach (var item in varData)
                {
                    item.DJH = newDJH;
                }

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 查询客户信息
        /// </summary>
        /// <param name="goodsID">产品ID</param>
        /// <param name="productCode">箱体编号</param>
        /// <returns>返回查询到的客户信息</returns>
        public DataTable GetCustomerInfo(int goodsID, string productCode)
        {
            string strSql = "select * from View_YX_CVTCustomerInformation " +
                " where 物品ID = " + goodsID + " and CVT编号 = '" + productCode + "'";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dtTemp;
        }

        /// <summary>
        /// 查询装车信息
        /// </summary>
        /// <param name="goodsID">产品ID</param>
        /// <param name="productCode">箱体编号</param>
        /// <returns>返回查询到的装车信息</returns>
        public DataTable GetLoadingInfo(int goodsID, string productCode)
        {
            string strSql = "select * from View_YX_LoadingInfo " +
                " where 物品ID = " + goodsID + " and CVT编号 = '" + productCode + "'";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dtTemp;
        }

        /// <summary>
        /// 获得自动入库的入库方式
        /// </summary>
        /// <param name="producrType">产品类型 ("0公里返修退货",售后返修退货","售后已修退货",
        /// "批量生产退货","0公里批量返修退货","新箱","未知","生产返修退货")</param>
        /// <param name="productCode">产品编码</param>
        /// <returns>成功返回获取到的入库方式，失败返回“未知”</returns>
        public string GetInStockWay(string producrType, string productCode)
        {
            string strYWFS = GetTHYWFS(producrType, productCode);
            string strReturn;

            switch (strYWFS)
            {
                //下线车间要求 此关联方式取消 Modify by cjb on 2012.5.16
                //case "生产退货":
                //    strReturn = "生产入库";
                //    break;
                case "0公里返修退货":
                    strReturn = "0公里返修入库";
                    break;
                case "售后返修退货":
                    strReturn = "售后返修入库";
                    break;
                case "售后已修退货":
                    strReturn = "售后返修入库";
                    break;
                case "批量生产退货":
                    strReturn = "批量生产入库";
                    break;
                case "0公里批量返修退货":
                    strReturn = "0公里批量返修入库";
                    break;
                case "生产返修退货":
                    strReturn = "生产返修入库";
                    break;
                case "新箱":
                    strReturn = "生产入库";
                    break;
                case "未知":
                    strReturn = "未知";
                    break;
                default:
                    strReturn = "未知";
                    break;
            }

            return strReturn;
        }

        /// <summary>
        /// 获得业务方式
        /// </summary>
        /// <param name="producrType">产品类型</param>
        /// <param name="productCode">产品编码</param>
        /// <returns>成功返回获取到的业务方式，失败返回“未知”</returns>
        public string GetOperationWay(string producrType, string productCode)
        {
            Hashtable paramTable = new Hashtable();

            paramTable.Add("@ProductType", producrType);
            paramTable.Add("@ProductCode", productCode);

            string strErr = "";

            return GlobalObject.DatabaseServer.QueryInfoPro(
                "YX_GetProductNearestYWLX", paramTable, out strErr).Rows[0][0].ToString();
        }

        /// <summary>
        /// 获得对应的退货的业务方式
        /// </summary>
        /// <param name="producrType">产品类型</param>
        /// <param name="productCode">产品编码</param>
        /// <returns>成功返回获取到的业务方式，失败返回“未知”</returns>
        string GetTHYWFS(string producrType, string productCode)
        {
            string strYWFS = "";

            int intGoodsID = m_serverProductList.GetProductGoodsID(producrType, 0, false);

            string FindstrSql = "select * from (select * from " +
                " (select GoodsID,ProductCode,YWFS,AffirmDate from ProductsCodes as a  " +
                " inner join S_MarketingBill as b on a.DJH = b.DJH  " +
                " where a.DJH like '%YXTH%' and b.DJZT_Flag = '已确认') as a " +
                " where AffirmDate = (select Max(AffirmDate) from  " +
                " (select GoodsID,ProductCode,YWFS,AffirmDate from ProductsCodes as a  " +
                " inner join S_MarketingBill as b on a.DJH = b.DJH  " +
                " where a.DJH like '%YXTH%' and b.DJZT_Flag = '已确认') as b  " +
                " where b.GoodsID = a.GoodsID and b.ProductCode = a.ProductCode) " +
                " ) as a where a.GoodsID = " + intGoodsID + " and ProductCode = '" + productCode + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(FindstrSql);

            if (dt.Rows.Count == 0)
            {
                string MatchingstrSql = "select * from S_NewAndOldProductCodeMatching " +
                    " where BatchNo = (select Max(BatchNo) from S_NewAndOldProductCodeMatching " +
                    " where NewEdition = '" + producrType + "' and NewProductCode = '" + productCode + "') " +
                    " and NewEdition = '" + producrType + "' and NewProductCode = '" + productCode + "'";

                dt = GlobalObject.DatabaseServer.QueryInfo(MatchingstrSql);

                if (dt.Rows.Count == 0)
                {
                    string strSql = "select Top 1 ProductCode,AssemblingMode  from P_ElectronFile " +
                        " where ParentCode = ''  and  ProductCode like '%" + productCode
                        + "%' and ProductCode like '%" + producrType + "%' and FittingTime > '2011-10-20'" +
                        " order by FinishTime desc";

                    dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

                    if (dt.Rows.Count == 0)
                    {
                        string error;

                        Hashtable hsTable = new Hashtable();

                        hsTable.Add("@ProductType", producrType);
                        hsTable.Add("@ProductCode", productCode);

                        dt = GlobalObject.DatabaseServer.QueryInfoPro("MES_GetProductCodeInfo", hsTable, out error);

                        if (dt == null)
                        {
                            strYWFS = "新箱";
                        }
                        else
                        {
                            if (dt.Rows.Count == 0)
                            {
                                strYWFS = "未知";
                            }
                            else
                            {
                                if (dt.Rows[0][0].ToString().Contains("返修"))
                                {
                                    strYWFS = "售后返修退货";
                                }
                                else
                                {
                                    strYWFS = "新箱";
                                }
                            }
                        }
                    }
                    else
                    {
                        //if (dt.Rows[0]["AssemblingMode"].ToString() == "正常装配")
                        //{
                        //    strYWFS = "新箱";
                        //}
                        //else
                        //{
                        //    strYWFS = "售后返修退货";
                        //}

                        //Modify by cjb on 2012.6.11 直接判断箱号是否带F

                        if (productCode.Contains("F") && (productCode.IndexOf("F") + 1 != productCode.Length))
                        {
                            strYWFS = "售后返修退货";
                        }
                        else
                        {
                            strYWFS = "新箱";
                        }
                    }
                }
                else
                {
                    intGoodsID = m_serverProductList.GetProductGoodsID(dt.Rows[0]["OldEdition"].ToString(), 0, false);
                    productCode = dt.Rows[0]["OldProductCode"].ToString();

                    FindstrSql = "select * from (select * from " +
                        " (select GoodsID,ProductCode,YWFS,AffirmDate from ProductsCodes as a  " +
                        " inner join S_MarketingBill as b on a.DJH = b.DJH  " +
                        " where a.DJH like '%YXTH%' and b.DJZT_Flag = '已确认') as a " +
                        " where AffirmDate = (select Max(AffirmDate) from  " +
                        " (select GoodsID,ProductCode,YWFS,AffirmDate from ProductsCodes as a  " +
                        " inner join S_MarketingBill as b on a.DJH = b.DJH  " +
                        " where a.DJH like '%YXTH%' and b.DJZT_Flag = '已确认') as b  " +
                        " where b.GoodsID = a.GoodsID and b.ProductCode = a.ProductCode) " +
                        " ) as a where a.GoodsID = " + intGoodsID + " and ProductCode = '" + productCode + "'";

                    dt = GlobalObject.DatabaseServer.QueryInfo(FindstrSql);

                    if (dt.Rows.Count == 0)
                    {
                        strYWFS = "未知";
                    }
                    else
                    {
                        strYWFS = dt.Rows[0]["YWFS"].ToString();
                    }
                }
            }
            else
            {
                strYWFS = dt.Rows[0]["YWFS"].ToString();
            }

            return strYWFS;
        }

        /// <summary>
        /// 检查业务是否匹配
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="productCode">产品编码</param>
        /// <param name="marktingType">业务类型</param>
        /// <returns>TRUE成功匹配，FALSE匹配失败</returns>
        public bool IsOperationMatching(int goodsID, string productCode, string marktingType)
        {
            string strSql = "select Top 1 YWFS from ProductsCodes as a " +
                " inner join S_MarketingBill as b on a.DJH = b.DJH inner join ProductStock as c " +
                " on a.ProductCode = c.ProductCode and a.GoodsID = c.GoodsID " +
                " where a.ProductCode = '" + productCode + "' and a.GoodsID = " + goodsID
                + " and DJZT_FLAG = '已确认' and c.ProductStatus = '退货' order by AffirmDate desc";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dtTemp.Rows.Count > 0)
            {
                if (marktingType.Contains("0公里"))
                {
                    if (dtTemp.Rows[0][0].ToString().Contains("0公里"))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (dtTemp.Rows[0][0].ToString().Contains("0公里"))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            else
            {
                if (marktingType.Contains("0公里"))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// 批量生成入库单
        /// </summary>
        /// <param name="insert">需要插入的信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>插入成功返回True，插入失败返回False</returns>
        public bool BatchCreateBill(DataTable insert, out string error)
        {
            error = null;

            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            //dataContext.Connection.Open();
            //dataContext.Transaction = dataContext.Connection.BeginTransaction();

            try
            {
                DataTable dtFuck = insert.Copy();

                string[] strBillName = { "库房ID", "入库方式" };
                string[] strListName = { "库房ID", "入库方式", "物品ID" };
                DataTable dtDistinct =
                    GlobalObject.DataSetHelper.SelectDistinct("", insert, strBillName);

                List<WS_ProductCodeDetail> tempList = new List<WS_ProductCodeDetail>();

                for (int i = 0; i < dtDistinct.Rows.Count; i++)
                {
                    string strDJH = m_assignBill.AssignNewNo(this,
                        CE_BillTypeEnum.营销入库单.ToString());

                    S_MarketingBill lnqBill = new S_MarketingBill();

                    Service_Manufacture_WorkShop.IWorkShopBasic serverWorkShopBasic = 
                        Service_Manufacture_WorkShop.ServerModuleFactory.GetServerModule<Service_Manufacture_WorkShop.IWorkShopBasic>();

                    WS_WorkShopCode tempWorkCode = serverWorkShopBasic.GetPersonnelWorkShop(BasicInfo.LoginID);

                    if (tempWorkCode == null)
                    {
                        throw new Exception("获取车间基础信息失败");
                    }

                    lnqBill.DJH = strDJH;
                    lnqBill.ObjectDept = tempWorkCode.DeptCode;
                    lnqBill.LRRY = BasicInfo.LoginID;
                    lnqBill.Date = ServerTime.Time;
                    lnqBill.YWLX = "入库";
                    lnqBill.DJZT_FLAG = "已保存";
                    lnqBill.Remark = "自动生成";
                    lnqBill.Price = 0;
                    lnqBill.YWFS = dtDistinct.Rows[i]["入库方式"].ToString();
                    lnqBill.StorageID = dtDistinct.Rows[i]["库房ID"].ToString();
                    lnqBill.LRKS = BasicInfo.DeptCode;
                    lnqBill.SHRY = "";
                    lnqBill.JYRY = "";
                    lnqBill.KFRY = "";
                    lnqBill.DELETE_FLAG = 0;

                    dataContext.S_MarketingBill.InsertOnSubmit(lnqBill);
                    dataContext.SubmitChanges();

                    DataTable dtListDistinct = GlobalObject.DataSetHelper.SelectDistinct("",
                        insert, strListName);

                    for (int k = 0; k < dtListDistinct.Rows.Count; k++)
                    {
                        if (dtDistinct.Rows[i]["库房ID"].ToString()
                            == dtListDistinct.Rows[k]["库房ID"].ToString()
                            && dtDistinct.Rows[i]["入库方式"].ToString()
                            == dtListDistinct.Rows[k]["入库方式"].ToString())
                        {
                            string strSX = " 库房ID = '" + dtListDistinct.Rows[k]["库房ID"].ToString() + "'" +
                                " and 物品ID = " + Convert.ToInt32(dtListDistinct.Rows[k]["物品ID"]) +
                                " and 入库方式 = '" + dtListDistinct.Rows[k]["入库方式"].ToString() + "'";

                            S_MarketingList lnqList = new S_MarketingList();

                            lnqList.BatchNo = "";
                            lnqList.Count = Convert.ToDecimal(
                                GlobalObject.DataSetHelper.SelectGroupByInto("", insert, "Count(物品ID)", strSX, "库房ID,入库方式,物品ID").Rows[0][0]);
                            lnqList.CPID = dtListDistinct.Rows[k]["物品ID"].ToString();
                            lnqList.DJ_ID = GetBillID();
                            lnqList.Price = 0;
                            lnqList.Provider = "SYS_JLRD";
                            lnqList.UnitPrice = 0;
                            lnqList.ReMark = "自动生成";

                            dataContext.S_MarketingList.InsertOnSubmit(lnqList);

                            var varData_ProductCode = from a in dataContext.ProductsCodes
                                                      where a.DJH == strDJH
                                                      select a;

                            dataContext.ProductsCodes.DeleteAllOnSubmit(varData_ProductCode);

                            DataRow[] drRows = insert.Select(strSX);

                            foreach (DataRow dr in drRows)
                            {
                                string strCode = "";

                                object obj = UniversalFunction.GetGoodsAttributeInfo(Convert.ToInt32(dtListDistinct.Rows[k]["物品ID"]),
                                    CE_GoodsAttributeName.厂商编码);

                                if (obj != null && obj.ToString() != "False")
                                {
                                    strCode = obj.ToString();
                                }

                                View_F_GoodsPlanCost tempGoodsLnq = UniversalFunction.GetGoodsInfo(Convert.ToInt32(dtListDistinct.Rows[k]["物品ID"]));

                                if (tempGoodsLnq == null)
                                {
                                    throw new Exception("系统中无此物品信息");
                                }

                                ProductsCodes lnqCode = new ProductsCodes();

                                lnqCode.ProductCode = dr["箱体编号"].ToString();
                                lnqCode.GoodsName = tempGoodsLnq.物品名称;
                                lnqCode.GoodsCode = tempGoodsLnq.图号型号;
                                lnqCode.Spec = tempGoodsLnq.规格;
                                lnqCode.GoodsID = Convert.ToInt32(dtListDistinct.Rows[k]["物品ID"]);
                                lnqCode.Code = strCode;
                                lnqCode.ZcCode = "";
                                lnqCode.DJH = strDJH;
                                lnqCode.BoxNo = dr["BoxNo"] == null ? "" : dr["BoxNo"].ToString();

                                if (dtListDistinct.Rows[k]["入库方式"].ToString() == "生产入库"
                                    && m_serverProductCode.IsProductCodeInfo(dataContext, lnqCode.ProductCode, lnqCode.GoodsID))
                                {
                                    throw new Exception("箱号：【" + lnqCode.ProductCode + "】已存在入库记录，入库方式不能为【生产入库】");
                                }

                                WS_ProductCodeDetail lnqWSProductCode = new WS_ProductCodeDetail();
                                lnqWSProductCode.BillNo = strDJH;
                                lnqWSProductCode.GoodsID = Convert.ToInt32(dtListDistinct.Rows[k]["物品ID"]);
                                lnqWSProductCode.IsUse = false;
                                lnqWSProductCode.OperationType = (int)CE_SubsidiaryOperationType.营销入库;
                                lnqWSProductCode.ProductCode = dr["箱体编号"].ToString();
                                lnqWSProductCode.StorageID = tempWorkCode.WSCode;

                                tempList.Add(lnqWSProductCode);
                                dataContext.ProductsCodes.InsertOnSubmit(lnqCode);
                            }
                        }
                    }

                    Service_Manufacture_WorkShop.IWorkShopProductCode serverWorkShopProductCode =
                        Service_Manufacture_WorkShop.ServerModuleFactory.GetServerModule<Service_Manufacture_WorkShop.IWorkShopProductCode>();

                    serverWorkShopProductCode.OperatorProductCodeDetail(dataContext, tempList);
                    dataContext.SubmitChanges();
                }

                var varData = from a in dataContext.ProductCode_AutoCreatePutIn_Subsidiary
                              select a;

                dataContext.ProductCode_AutoCreatePutIn_Subsidiary.DeleteAllOnSubmit(varData);
                dataContext.SubmitChanges();

                //dataContext.Transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                //dataContext.Transaction.Rollback();
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 查看匹配表
        /// </summary>
        /// <returns>返回查询的匹配表信息</returns>
        public DataTable GetProductCodeMatchingInfo()
        {
            string strSql = "select * from View_S_NewAndOldProductCodeMatching";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得单号
        /// </summary>
        /// <returns>返回获得的单号</returns>
        public string GetMatchingBillID()
        {
            string strNewDJH = "";

            try
            {
                string strDJH = "FXX" + ServerTime.Time.Year.ToString();

                string strSql = "select max(substring(BatchNo,10,6)) from S_NewAndOldProductCodeMatching " +
                    " where BatchNo like '" + strDJH + "%'";

                DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

                if (dt != null && dt.Rows.Count != 0 && dt.Rows[0][0].ToString() != "")
                {
                    string strValue = (Convert.ToInt32(dt.Rows[0][0].ToString()) + 1).ToString("D6");
                    strNewDJH = strDJH + ServerTime.Time.Month.ToString("D2") + strValue;
                }
                else
                {
                    strNewDJH = strDJH + ServerTime.Time.Month.ToString("D2") + "000001";
                }

                return strNewDJH;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="insert">匹配信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>插入成功返回True，插入失败返回False</returns>
        public bool AddMatchingInfo(S_NewAndOldProductCodeMatching insert, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                dataContext.S_NewAndOldProductCodeMatching.InsertOnSubmit(insert);

                dataContext.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 更新匹配
        /// </summary>
        /// <param name="insert">匹配信息</param>
        /// <param name="id">表的ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>更新成功返回True，更新失败返回False</returns>
        public bool UpdateMatchingInfo(S_NewAndOldProductCodeMatching insert, int id, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.S_NewAndOldProductCodeMatching
                              where a.ID == id
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数据不唯一或者为空";
                }
                else
                {
                    S_NewAndOldProductCodeMatching lnqNow = new S_NewAndOldProductCodeMatching();
                    lnqNow.BatchNo = insert.BatchNo;
                    lnqNow.NewEdition = insert.NewEdition;
                    lnqNow.NewProductCode = insert.NewProductCode;
                    lnqNow.OldEdition = insert.OldEdition;
                    lnqNow.OldProductCode = insert.OldProductCode;
                    lnqNow.Remark = insert.Remark;
                }

                dataContext.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 检查是否有相同匹配记录
        /// </summary>
        /// <param name="insert">匹配信息</param>
        /// <returns>存在相同的返回True，不存在返回False</returns>
        public bool IsSameProductMatchingInfo(S_NewAndOldProductCodeMatching insert)
        {
            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            var varData = from a in dataContext.S_NewAndOldProductCodeMatching
                          where a.NewEdition == insert.NewEdition
                          && a.NewProductCode == insert.NewProductCode
                          && a.OldEdition == insert.OldEdition
                          && a.OldProductCode == insert.OldProductCode
                          select a;

            if (varData.Count() != 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 删除匹配
        /// </summary>
        /// <param name="id">表的ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        public bool DeleteMatchingInfo(int id, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.S_NewAndOldProductCodeMatching
                              where a.ID == id
                              select a;

                dataContext.S_NewAndOldProductCodeMatching.DeleteAllOnSubmit(varData);

                dataContext.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 获得库房中合格CVT数量
        /// </summary>
        /// <param name="storageID">库房ID</param>
        /// <param name="goodID">物品ID</param>
        /// <param name="isRepairstatus">是否为返修状态 1 是，0 不是</param>
        /// <returns>返回获得的CVT数量信息</returns>
        public int GetProductRepairStatusCount(string storageID, int goodID, bool isRepairstatus)
        {
            string strSql = "select Count(*) from ProductStock where StorageID = '" + storageID + "' and GoodsID = " + goodID +
               " and ProductStatus in ('入库','退库','领料退库','调入')";

            if (isRepairstatus)
            {
                strSql += " and RepairStatus = 1";
            }
            else
            {
                strSql += " and RepairStatus = 0";
            }

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dt.Rows.Count == 0)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(dt.Rows[0][0]);
            }
        }

        /// <summary>
        /// 车间业务检测
        /// </summary>
        /// <param name="allType">明细业务类型</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="productCode">箱体编号</param>
        /// <param name="storageID">库房ID</param>
        /// <param name="error">出错时返回的错误信息</param>
        /// <returns>规范返回True,不规范返回False</returns>
        bool IsProductCodeOperationStandardWorkShop(string allType, int goodsID, string productCode, string storageID, out string error)
        {
            error = null;

            if (!Convert.ToBoolean(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.开启车间管理模块]))
            {
                return true;
            }

            CE_SubsidiaryOperationType operationType =
                GeneralFunction.StringConvertToEnum<CE_SubsidiaryOperationType>(allType);


            Service_Manufacture_WorkShop.IWorkShopProductCode serviceProductCode =
                Service_Manufacture_WorkShop.ServerModuleFactory.GetServerModule<Service_Manufacture_WorkShop.IWorkShopProductCode>();

            BASE_SubsidiaryOperationType tempLnq = UniversalFunction.GetSubsidiaryOperationType((int)operationType);
            WS_ProductCodeStock tempProductCode = serviceProductCode.GetProductCodeStockInfo(goodsID, productCode, storageID);

            if (tempLnq.OperationType == (int)CE_SubsidiaryOperationType.物料转换后)
            {
                return true;
            }

            if ((bool)tempLnq.DepartmentType)
            {
                if (tempProductCode != null && tempProductCode.IsInStock)
                {
                    error = "【" + tempProductCode + "】 该车间已有此箱体编号的产品的记录，不能重复入车间库存";
                    return false;
                }
            }
            else
            {
                if (tempProductCode == null)
                {
                    error = "【" + tempProductCode + "】 该车间无此箱体编号的产品的记录";
                    return false;
                }
                else
                {
                    if (!tempProductCode.IsInStock)
                    {
                        error = "【" + tempProductCode + "】 此箱体编号的产品不在此车间";
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 库房业务检测
        /// </summary>
        /// <param name="allType">明细业务类型</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="productCode">箱体编号</param>
        /// <param name="storageID">库房ID</param>
        /// <param name="error">出错时返回的错误信息</param>
        /// <returns>规范返回True,不规范返回False</returns>
        bool IsProductCodeOperationStandardStock(string allType, int goodsID, string productCode, string storageID, out string error)
        {
            error = null;

            CE_MarketingType markType = GeneralFunction.StringConvertToEnum<CE_MarketingType>(allType);

            //除了售后库房不需要检测业务
            if (storageID == "05" || !UniversalFunction.IsProduct(goodsID))
            {
                return true;
            }

            if (markType == CE_MarketingType.入库 
                || markType == CE_MarketingType.退库 
                || markType == CE_MarketingType.领料退库 
                || markType == CE_MarketingType.调入)
            {
                if (m_serverProductCode.IsProductCodeInStock(productCode, goodsID, storageID))
                {
                    error = "【"+ productCode +"】 此编号已在库，不能重复入库，请重新核对";
                    return false;
                }

                IProductListServer serverProductList = ServerModule.ServerModuleFactory.GetServerModule<IProductListServer>();

                if (Convert.ToBoolean(UniversalFunction.GetGoodsAttributeInfo(goodsID, CE_GoodsAttributeName.CVT)))
                {
                    if (markType == CE_MarketingType.入库)
                    {
                        CheckInStock(goodsID, productCode);
                    }
                }

                if (markType != CE_MarketingType.入库)
                {
                    if (!IsExistProductCode(productCode, goodsID))
                    {
                        error = "【" + productCode + "】 此编号不存在，无法进入库房，请重新核对";
                        return false;
                    }
                }
            }
            else if (markType == CE_MarketingType.出库
                || markType == CE_MarketingType.退货
                || markType == CE_MarketingType.领料 
                || markType == CE_MarketingType.调出)
            {
                if (!m_serverProductCode.IsProductCodeInStock(productCode, goodsID, storageID))
                {
                    error = "【" + productCode + "】 此编号不在库，无法出库,请重新核对";
                    return false;
                }
                else
                {
                    if (!IsNatural(productCode, goodsID, storageID))
                    {
                        error = "【" + productCode + "】 此编号在库但状态为【隔离】，无法出库,请重新核对";
                        return false;
                    }
                }
            }
            else
            {
                //throw new Exception("业务存在异常，无法操作");
                if (!m_serverProductCode.IsProductCodeInStock(productCode, goodsID, storageID))
                {
                    error = "【" + productCode + "】 此编号不在库，无法出库,请重新核对";
                    return false;
                }
            }

            return true;
        }

        void CheckInStock(int goodsID, string productCode)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.ProductsCodes
                          join b in ctx.S_MarketingBill
                          on a.DJH equals b.DJH
                          where a.ProductCode == productCode
                          && a.GoodsID == goodsID
                          orderby a.ID descending
                          select b;

            if (varData.Count() > 1)
            {
                var tempDate = from a in varData
                               where a.YWFS == "生产入库"
                               && a.YWLX == CE_MarketingType.入库.ToString()
                               select a;

                if (tempDate.Count() > 1)
                {
                    throw new Exception("【箱号】:" + productCode + "【生产入库】不能重复入库");
                }

                if (varData.First().YWLX != CE_MarketingType.入库.ToString() 
                    && varData.First().YWLX != CE_MarketingType.退货.ToString())
                {
                    throw new Exception("【箱号】:" + productCode + "未【退货】，无法入库");
                }               
            }
        }

        /// <summary>
        /// 检测箱号的业务是否规范
        /// </summary>
        /// <param name="allType">明细业务类型</param>
        /// <param name="typeName">类型名称</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="productCode">箱体编号</param>
        /// <param name="storageID">库房ID</param>
        /// <param name="error">出错时返回的错误信息</param>
        /// <returns>规范返回True,不规范返回False</returns>
        public bool IsProductCodeOperationStandard(string allType, Type typeName, int goodsID, 
            string productCode, string storageID, out string error)
        {
            error = null;

            bool result = true;

            if (!Convert.ToBoolean( BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.检测业务规范]))
            {
                return true;
            }

            CE_BusinessType businessType = CE_BusinessType.未知;

            switch (typeName.Name)
            {
                case "CE_MarketingType":
                    if (GeneralFunction.GetEumnList(typeof(CE_MarketingType)).Contains(allType))
                    {
                        businessType = CE_BusinessType.库房业务;
                    }
                    break;
                case "CE_SubsidiaryOperationType":
                    if (GeneralFunction.GetEumnList(typeof(CE_SubsidiaryOperationType)).Contains(allType))
                    {
                        businessType = CE_BusinessType.车间业务;
                    }
                    break;
                default:
                    break;
            }

            if (businessType == CE_BusinessType.未知)
            {
                error = "业务类型错误,请重新核查";
                return false;
            }

            switch (businessType)
            {
                case CE_BusinessType.库房业务:
                    result = IsProductCodeOperationStandardStock(allType, goodsID, productCode, storageID, out error);
                    break;
                case CE_BusinessType.车间业务:
                    result = IsProductCodeOperationStandardWorkShop(allType, goodsID, productCode, storageID, out error);
                    break;
                //case BusinessType.综合业务:

                //    if (!IsProductCodeOperationStandardStock(allType, goodsID, productCode, storageID, out error) 
                //        ||!IsProductCodeOperationStandardWorkShop(allType, goodsID, productCode, storageID, out error))
                //    {
                //        result = false;
                //    }

                //    break;
                default:
                    break;
            }

            return result;
        }

        /// <summary>
        /// 回退单据
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <param name="billStatus">回退后的单据状态</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <param name="rebackReason">回退原因</param>
        /// <param name="billType">单据类型</param>
        /// <returns>回退成功返回True，回退失败返回False</returns>
        public bool ReturnBill(string djh, string billStatus, out string error, string rebackReason, string billType)
        {
            try
            {
                error = null;

                IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

                m_billMessageServer.BillType = billType;

                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.S_MarketingBill
                              where a.DJH == djh
                              select a;

                string strMsg = "";

                if (varData.Count() != 1)
                {
                    error = "数据不唯一或者为空";
                    return false;
                }
                else
                {
                    S_MarketingBill lnqSell = varData.Single();

                    switch (billStatus)
                    {
                        case "已保存":

                            strMsg = string.Format("{0}号" + billType + "单据已回退，请您重新处理单据; 回退原因为"
                                + rebackReason, djh);

                            m_billMessageServer.PassFlowMessage(djh, strMsg, lnqSell.LRRY, false);

                            lnqSell.DJZT_FLAG = billStatus;

                            lnqSell.FSRQ = null;
                            lnqSell.FSRY = null;
                            lnqSell.JYRY = null;
                            lnqSell.CheckDate = null;
                            lnqSell.SHRY = null;
                            lnqSell.ShDate = null;

                            break;
                        case "已审核":

                            strMsg = string.Format("{0}号" + billType + "单据已回退，请您重新处理单据; 回退原因为"
                                + rebackReason, djh);

                            m_billMessageServer.PassFlowMessage(djh, strMsg, lnqSell.JYRY, false);

                            lnqSell.DJZT_FLAG = billStatus;
                            lnqSell.FSRQ = null;
                            lnqSell.FSRY = null;
                            lnqSell.JYRY = null;
                            lnqSell.CheckDate = null;

                            break;
                        case "已检验":

                            strMsg = string.Format("{0}号" + billType + "单据已回退，请您重新处理单据; 回退原因为"
                                + rebackReason, djh);

                            m_billMessageServer.PassFlowMessage(djh, strMsg, lnqSell.FSRY, false);

                            lnqSell.DJZT_FLAG = billStatus;
                            lnqSell.FSRQ = null;
                            lnqSell.FSRY = null;

                            break;
                    }

                    dataContext.SubmitChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 获得条形码Table
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回Table</returns>
        public DataTable GetBarcodeTable(string billNo)
        {
            string strSql = " select a.ID, ProductCode, BoxNo from ProductsCodes as a "+
                            " inner join  (select GoodsID from F_GoodsAttributeRecord  " +
                            " where (AttributeID = " + (int)CE_GoodsAttributeName.CVT + ")"+
                            " and AttributeValue = '" + bool.TrueString + "') as b on a.GoodsID = b.GoodsID " +
                            " where DJH = '" + billNo + "' order by BoxNo, a.ID";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);
            return dtTemp;
        }
    }
}
