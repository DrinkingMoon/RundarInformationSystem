using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using GlobalObject;

namespace ServerModule
{
    /// <summary>
    /// 领料退库单物品信息服务
    /// </summary>
    class MaterialListReturnedInTheDepot :BasicServer, IMaterialListReturnedInTheDepot
    {
        /// <summary>
        /// 产品信息服务组件
        /// </summary>
        IProductListServer m_serverProductList = ServerModuleFactory.GetServerModule<IProductListServer>();

        /// <summary>
        /// 库存服务组件
        /// </summary>
        IStoreServer m_serverStore = ServerModuleFactory.GetServerModule<IStoreServer>();

        bool BillIsFinish(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            return BillIsFinish(ctx, billNo);
        }

        bool BillIsFinish(DepotManagementDataContext ctx, string billNo)
        {
            var vardata = from a in ctx.S_MaterialReturnedInTheDepot
                          where a.Bill_ID == billNo
                          select a;

            if (vardata.Count() == 0)
            {
                return false;
            }
            else if(vardata.Count() == 1)
            {
                S_MaterialReturnedInTheDepot billInfo = vardata.Single();

                if (billInfo.BillStatus == MaterialReturnedInTheDepotBillStatus.已完成.ToString())
                {
                    return true;
                }

                return false;
            }
            else
            {
                throw new Exception("单据不唯一");
            }
        }

        /// <summary>
        /// 对于批量生成单据明细界面的显示功能
        /// </summary>
        /// <param name="selectType">显示单据类型  (“领料”，“领料退库”)</param>
        /// <param name="startTime">起始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>返回TABLE</returns>
        public DataTable GetBatchCreatList(string selectType, DateTime startTime, DateTime endTime)
        {
            Hashtable paramTable = new Hashtable();

            paramTable.Add("@SelectType", selectType);
            paramTable.Add("@StartTime", startTime);
            paramTable.Add("@EndTime", endTime);

            string strErr = "";

            return GlobalObject.DatabaseServer.QueryInfoPro(
                "KFYW_Select_RequisitionOrReturnedInTheDepot", paramTable, out strErr);
        }

        /// <summary>
        /// 批量生成明细
        /// </summary>
        /// <param name="selectType">单据类型 (“领料”，“领料退库”)</param>
        /// <param name="billID">单据号</param>
        /// <param name="billIDGather">数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool BatchCreateList(string selectType, string billID, string billIDGather, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                if (BillIsFinish(dataContext, billID))
                {
                    throw new Exception("单据已完成，无法进行操作");
                }

                Hashtable paramTable = new Hashtable();

                paramTable.Add("@SelectType", selectType);
                paramTable.Add("@BillID", billIDGather);

                string strErr = "";

                DataTable dtBatchCreate =
                    GlobalObject.DatabaseServer.QueryInfoPro("KFYW_BatchCreateListFrom_RequisitionOrReturnedInTheDepot",
                    paramTable, out strErr);

                if (dtBatchCreate == null)
                {
                    error = strErr;
                    return false;
                }

                if (selectType == "领料")
                {
                    var varReturn = from a in dataContext.S_MaterialListReturnedInTheDepot
                                    where a.Bill_ID == billID
                                    select a;

                    dataContext.S_MaterialListReturnedInTheDepot.DeleteAllOnSubmit(varReturn);

                    for (int i = 0; i < dtBatchCreate.Rows.Count; i++)
                    {
                        S_MaterialListReturnedInTheDepot lnqReturn = new S_MaterialListReturnedInTheDepot();

                        lnqReturn.BatchNo = dtBatchCreate.Rows[i]["BatchNo"].ToString();
                        lnqReturn.Bill_ID = billID;
                        lnqReturn.ColumnNumber = dtBatchCreate.Rows[i]["ColumnNumber"].ToString();
                        lnqReturn.Depot = dtBatchCreate.Rows[i]["Depot"].ToString();
                        lnqReturn.GoodsID = Convert.ToInt32(dtBatchCreate.Rows[i]["GoodsID"].ToString());
                        lnqReturn.LayerNumber = dtBatchCreate.Rows[i]["LayerNumber"].ToString();
                        lnqReturn.Provider = dtBatchCreate.Rows[i]["Provider"].ToString();
                        lnqReturn.ProviderBatchNo = dtBatchCreate.Rows[i]["ProviderBatchNo"].ToString();
                        lnqReturn.Remark = "由领料单" + billIDGather + "批量自动生成";
                        lnqReturn.ReturnedAmount = Convert.ToDecimal(dtBatchCreate.Rows[i]["ReturnedAmount"].ToString());
                        lnqReturn.ShelfArea = dtBatchCreate.Rows[i]["ShelfArea"].ToString();

                        dataContext.S_MaterialListReturnedInTheDepot.InsertOnSubmit(lnqReturn);
                    }
                }
                else
                {
                    var varRequisition = from a in dataContext.S_MaterialRequisitionGoods
                                         where a.Bill_ID == billID
                                         select a;

                    dataContext.S_MaterialRequisitionGoods.DeleteAllOnSubmit(varRequisition);

                    for (int i = 0; i < dtBatchCreate.Rows.Count; i++)
                    {
                        S_MaterialRequisitionGoods lnqRequisition = new S_MaterialRequisitionGoods();

                        lnqRequisition.BasicCount = Convert.ToDecimal(dtBatchCreate.Rows[i]["BasicCount"].ToString());
                        lnqRequisition.BatchNo = dtBatchCreate.Rows[i]["BatchNo"].ToString();
                        lnqRequisition.Bill_ID = billID;
                        lnqRequisition.GoodsID = Convert.ToInt32(dtBatchCreate.Rows[i]["GoodsID"].ToString());
                        lnqRequisition.ProviderCode = dtBatchCreate.Rows[i]["ProviderCode"].ToString();
                        lnqRequisition.RealCount = Convert.ToDecimal(dtBatchCreate.Rows[i]["RealCount"].ToString());
                        lnqRequisition.Remark = "由退库单" + billIDGather + "批量自动生成";
                        lnqRequisition.RequestCount = Convert.ToDecimal(dtBatchCreate.Rows[i]["RequestCount"].ToString());
                        lnqRequisition.ShowPosition = i + 1;

                        MaterialRequisitionGoodsServer serverMaterialGoods = new MaterialRequisitionGoodsServer();

                        if (!serverMaterialGoods.AutoCreateGoods(dataContext, lnqRequisition, out error))
                        {
                            return false;
                        }
                        //dataContext.S_MaterialRequisitionGoods.InsertOnSubmit(lnqRequisition);
                    }
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
        /// 检查是否存在某单据物品清单
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            return (from r in ctx.S_MaterialListReturnedInTheDepot
                    where r.Bill_ID == billNo
                    select r).Count() > 0;
        }

        /// <summary>
        /// 获取指定领料退库单的物品信息
        /// </summary>
        /// <param name="billNo">领料退库单号</param>
        /// <returns>返回获取的物品信息</returns>
        public IEnumerable<View_S_MaterialListReturnedInTheDepot> GetGoods(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            return (from r in ctx.View_S_MaterialListReturnedInTheDepot
                    where r.退库单号 == billNo
                    select r);
        }

        /// <summary>
        /// 添加物品信息
        /// </summary>
        /// <param name="goods">物品信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功的标志</returns>
        public bool AddGoods(S_MaterialListReturnedInTheDepot goods, out string error)
        {
            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                if (BillIsFinish(ctx, goods.Bill_ID))
                {
                    throw new Exception("单据已完成，无法进行操作");
                }

                error = null;

                ctx.S_MaterialListReturnedInTheDepot.InsertOnSubmit(goods);

                ctx.SubmitChanges();

                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 批量添加物品
        /// </summary>
        /// <param name="lstGoods">要添加的物品信息列表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功的标志</returns>
        public bool AddGoods(List<View_S_MaterialListReturnedInTheDepot> lstGoods, out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                if (lstGoods != null && lstGoods.Count() > 0)
                {
                    if (BillIsFinish(ctx, lstGoods[0].退库单号))
                    {
                        throw new Exception("单据已完成，无法进行操作");
                    }
                }

                foreach (var item in lstGoods)
                {
                    S_MaterialListReturnedInTheDepot goods = new S_MaterialListReturnedInTheDepot();

                    goods.Bill_ID = item.退库单号;
                    goods.GoodsID = item.物品ID;
                    goods.BatchNo = item.批次号;
                    goods.Provider = item.供应商;
                    goods.ProviderBatchNo = item.供方批次号;
                    goods.ReturnedAmount = item.退库数;
                    goods.ShelfArea = item.货架;
                    goods.LayerNumber = item.层;
                    goods.ColumnNumber = item.列;
                    goods.Remark = item.备注;

                    ctx.S_MaterialListReturnedInTheDepot.InsertOnSubmit(goods);
                }

                ctx.SubmitChanges();
                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 删除物品信息
        /// </summary>
        /// <param name="idList">要删除的物品ID列表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功的标志</returns>
        public bool DeleteGoods(List<long> idList, out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var result = from r in ctx.S_MaterialListReturnedInTheDepot
                             where idList.Contains(r.ID)
                             select r;

                if (result.Count() > 0)
                {
                    if (BillIsFinish(ctx, result.ToList()[0].Bill_ID))
                    {
                        throw new Exception("单据已完成，无法进行操作");
                    }

                    ctx.S_MaterialListReturnedInTheDepot.DeleteAllOnSubmit(result);
                    ctx.SubmitChanges();
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
        /// 删除某领料退库单下的所有物品信息
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="billNo">领料退库单号</param>
        public void DeleteGoods(DepotManagementDataContext context, string billNo)
        {
            var result = from r in context.S_MaterialListReturnedInTheDepot 
                         where r.Bill_ID == billNo 
                         select r;

            if (BillIsFinish(context, billNo))
            {
                throw new Exception("单据已完成，无法进行操作");
            }

            context.S_MaterialListReturnedInTheDepot.DeleteAllOnSubmit(result);
        }

        /// <summary>
        /// 删除某领料退库单下的所有物品信息
        /// </summary>
        /// <param name="billNo">领料退库单号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功的标志</returns>
        public bool DeleteGoods(string billNo, out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var result = from r in ctx.S_MaterialListReturnedInTheDepot 
                             where r.Bill_ID == billNo 
                             select r;

                if (BillIsFinish(ctx, billNo))
                {
                    throw new Exception("单据已完成，无法进行操作");
                }

                ctx.S_MaterialListReturnedInTheDepot.DeleteAllOnSubmit(result);
                ctx.SubmitChanges();

                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 更新物品信息
        /// </summary>
        /// <param name="goods">物品信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功的标志</returns>
        public bool UpdateGoods(S_MaterialListReturnedInTheDepot goods, out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var result = from r in ctx.S_MaterialListReturnedInTheDepot
                             where r.ID == goods.ID
                             select r;

                if (result.Count() > 0)
                {
                    S_MaterialListReturnedInTheDepot updateGoods = result.Single();

                    if (BillIsFinish(ctx, updateGoods.Bill_ID))
                    {
                        throw new Exception("单据已完成，无法进行操作");
                    }

                    updateGoods.Bill_ID = goods.Bill_ID;
                    updateGoods.GoodsID = goods.GoodsID;
                    updateGoods.BatchNo = goods.BatchNo;
                    updateGoods.Provider = goods.Provider;
                    updateGoods.ProviderBatchNo = goods.ProviderBatchNo;
                    updateGoods.ReturnedAmount = goods.ReturnedAmount;
                    updateGoods.ShelfArea = goods.ShelfArea;
                    updateGoods.ColumnNumber = goods.ColumnNumber;
                    updateGoods.LayerNumber = goods.LayerNumber;
                    updateGoods.Remark = goods.Remark;
                }

                ctx.SubmitChanges();
                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 赋值账务信息
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="bill">单据信息</param>
        /// <param name="item">明细信息</param>
        /// <returns>返回账务信息对象</returns>
        public S_FetchGoodsDetailBill AssignDetailInfo(DepotManagementDataContext context, S_MaterialReturnedInTheDepot bill,
            S_MaterialListReturnedInTheDepot item)
        {
            IBillTypeServer server = ServerModuleFactory.GetServerModule<IBillTypeServer>();
            BASE_BillType billType = server.GetBillTypeFromName("领料退库单");

            if (billType == null)
            {
                throw new Exception("获取不到单据类型信息");
            }

            View_Department department = UniversalFunction.GetDeptInfo(context, bill.Department);

            //单价设置
            decimal dcStockUnitPrice = m_serverStore.GetGoodsUnitPrice(context, item.GoodsID, item.BatchNo, bill.StorageID);

            //S_FetchGoodsDetailBill用于存放每次领料、领料退库的明细信息
            S_FetchGoodsDetailBill detailBill = new S_FetchGoodsDetailBill();

            detailBill.ID = Guid.NewGuid();
            detailBill.FetchBIllID = bill.Bill_ID;
            detailBill.BillTime = ServerTime.Time;
            detailBill.FetchCount = -item.ReturnedAmount;
            detailBill.GoodsID = item.GoodsID;
            detailBill.BatchNo = item.BatchNo;
            detailBill.ProviderBatchNo = item.ProviderBatchNo;
            detailBill.Provider = item.Provider;
            detailBill.Price = -dcStockUnitPrice * (decimal)item.ReturnedAmount;
            detailBill.UnitPrice = dcStockUnitPrice;
            detailBill.Department = department.部门名称;
            detailBill.FillInPersonnel = bill.FillInPersonnel;
            detailBill.FinanceSignatory = null;
            detailBill.DepartDirector = bill.DepartmentDirector;
            detailBill.DepotManager = bill.DepotManager;
            detailBill.OperationType = (int)CE_SubsidiaryOperationType.领料退库;
            detailBill.StorageID = bill.StorageID;
            detailBill.Remark = "退库原因：" + bill.ReturnReason + ";备注：" + item.Remark;
            detailBill.FillInDate = bill.Bill_Time;

            IMaterialRequisitionPurposeServer purposeServer =
                ServerModuleFactory.GetServerModule<IMaterialRequisitionPurposeServer>();

            detailBill.Using = string.Format("领料退库，初始用途：{0}", purposeServer.GetBillPurpose(context, bill.PurposeCode).Purpose);

            return detailBill;
        }

        /// <summary>
        /// 赋值库存信息
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="bill">单据信息</param>
        /// <param name="goodsItem">明细信息</param>
        /// <returns>返回库存信息对象</returns>
        public S_Stock AssignStockInfo(DepotManagementDataContext context, S_MaterialReturnedInTheDepot bill,
            S_MaterialListReturnedInTheDepot goodsItem)
        {
            if (goodsItem.ShelfArea == null || goodsItem.ColumnNumber == null || goodsItem.LayerNumber == null)
            {
                throw new Exception("仓库货架、层、列等信息不能为空，请修改后重新提交");
            }

            bool blIsOnlyForRepair = false;

            var resultbill = from a in context.S_MaterialReturnedInTheDepot
                             where a.Bill_ID == bill.Bill_ID
                             select a;

            if (resultbill.Count() != 1)
            {
                throw new Exception("数据不唯一或者为空");
            }
            else
            {
                blIsOnlyForRepair = resultbill.Single().IsOnlyForRepair.ToString() == ""
                    ? false : Convert.ToBoolean(resultbill.Single().IsOnlyForRepair);
            }

            S_Stock stock = new S_Stock();

            // 添加信息到库存
            IStoreServer storeServer = ServerModuleFactory.GetServerModule<IStoreServer>();

            stock.GoodsID = goodsItem.GoodsID;
            stock.Provider = goodsItem.Provider;
            stock.ProviderBatchNo = goodsItem.ProviderBatchNo;
            stock.BatchNo = goodsItem.BatchNo;
            stock.ShelfArea = goodsItem.ShelfArea;
            stock.ColumnNumber = goodsItem.ColumnNumber;
            stock.LayerNumber = goodsItem.LayerNumber;
            stock.ExistCount = (decimal)goodsItem.ReturnedAmount;
            stock.Date = ServerModule.ServerTime.Time;
            stock.StorageID = bill.StorageID;

            if (blIsOnlyForRepair)
            {
                stock.GoodsStatus = 6;
            }

            return stock;
        }

        /// <summary>
        /// 获取领料退库单视图信息
        /// </summary>
        /// <param name="billNo">领料退库单号</param>
        /// <returns>成功返回获取领料信息, 失败返回null</returns>
        public View_S_MaterialReturnedInTheDepot GetBillView(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var result = from r in ctx.View_S_MaterialReturnedInTheDepot
                         where r.退库单号 == billNo
                         select r;

            if (result.Count() > 0)
            {
                return result.Single();
            }

            return null;
        }

        public void InsertInfoExcel(string billNo, DataTable tableInfo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();

            try
            {
                var varData = from a in ctx.S_MaterialReturnedInTheDepot
                              where a.Bill_ID == billNo
                              select a;

                if (BillIsFinish(ctx, billNo))
                {
                    throw new Exception("单据已完成，无法进行操作");
                }

                if (varData.Count() != 1)
                {
                    throw new Exception("获取单据【"+ billNo +"】信息有误");
                }

                var varData1 = from a in ctx.S_MaterialListReturnedInTheDepot
                               where a.Bill_ID == billNo
                               select a;

                ctx.S_MaterialListReturnedInTheDepot.DeleteAllOnSubmit(varData1);
                ctx.SubmitChanges();

                foreach (DataRow dr in tableInfo.Rows)
                {
                    View_F_GoodsPlanCost goodsInfo = UniversalFunction.GetGoodsInfo(dr["图号型号"].ToString().Trim(),
                        dr["物品名称"].ToString().Trim(), dr["规格"].ToString().Trim());

                    if (goodsInfo == null)
                    {
                        throw new Exception(string.Format("【图号型号】：{0} ，【物品名称】：{1}，【规格】：{2} 获取物品信息失败", 
                            dr["图号型号"].ToString().Trim(), dr["物品名称"].ToString().Trim(), dr["规格"].ToString().Trim()));
                    }

                    GlobalObject.QueryCondition_Store condition = new GlobalObject.QueryCondition_Store();

                    if (dr["批次号"] == null)
                    {
                        throw new Exception(UniversalFunction.GetGoodsMessage(goodsInfo.序号) + "【批次号】为空，获取失败");
                    }

                    condition.BatchNo = dr["批次号"].ToString().Trim();
                    condition.GoodsID = goodsInfo.序号;
                    condition.StorageID = varData.Single().StorageID;

                    S_Stock stockInfo = UniversalFunction.GetStockInfo(ctx, condition);

                    if (stockInfo == null)
                    {
                        throw new Exception(UniversalFunction.GetGoodsMessage(goodsInfo.序号) + "【批次号】：" 
                            + dr["批次号"].ToString().Trim() + " 获取库存信息失败");
                    }

                    S_MaterialListReturnedInTheDepot goods = new S_MaterialListReturnedInTheDepot();

                    goods.BatchNo = stockInfo.BatchNo;
                    goods.Bill_ID = billNo;
                    goods.ColumnNumber = stockInfo.ColumnNumber;
                    goods.Depot = stockInfo.Depot;
                    goods.GoodsID = stockInfo.GoodsID;
                    goods.LayerNumber = stockInfo.LayerNumber;
                    goods.Provider = stockInfo.Provider;
                    goods.ProviderBatchNo = stockInfo.ProviderBatchNo;
                    goods.RepairStatus = false;

                    decimal result = 0;

                    if (!Decimal.TryParse( dr["数量"].ToString(), out result))
                    {
                        throw new Exception(UniversalFunction.GetGoodsMessage(goodsInfo.序号) + "【批次号】：" 
                            + dr["批次号"].ToString().Trim() + "【数量】信息有误");
                    }

                    goods.ReturnedAmount = result;
                    goods.ShelfArea = stockInfo.ShelfArea;

                    ctx.S_MaterialListReturnedInTheDepot.InsertOnSubmit(goods);
                }

                ctx.SubmitChanges();
                ctx.Transaction.Commit();
            }
            catch (Exception ex)
            {
                ctx.Transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }
    }
}
