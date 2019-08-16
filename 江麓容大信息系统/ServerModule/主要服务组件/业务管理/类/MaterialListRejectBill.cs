
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
    /// 采购退货单物品信息服务
    /// </summary>
    class MaterialListRejectBill :BasicServer, IMaterialListRejectBill
    {
        /// <summary>
        /// 部门服务组件
        /// </summary>
        IDepartmentServer m_serverDepartment = ServerModuleFactory.GetServerModule<IDepartmentServer>();

        /// <summary>
        /// 基础物品信息服务
        /// </summary>
        IBasicGoodsServer m_basicGoodsServer = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();

        /// <summary>
        /// 库存服务
        /// </summary>
        IStoreServer m_storeServer = ServerModuleFactory.GetServerModule<IStoreServer>();

        /// <summary>
        /// 检查是否存在某单据物品清单
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(string billNo)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            return (from r in dataContxt.S_MaterialListRejectBill
                    where r.Bill_ID == billNo
                    select r).Count() > 0;
        }

        /// <summary>
        /// 获取指定采购退货单的物品信息
        /// </summary>
        /// <param name="billNo">退货单号</param>
        /// <returns>返回获取的物品信息</returns>
        public IEnumerable<View_S_MaterialListRejectBill> GetGoods(string billNo)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            return (from r in dataContxt.View_S_MaterialListRejectBill
                    where r.退货单号 == billNo
                    select r);
        }

        /// <summary>
        /// 设置金额信息
        /// </summary>
        /// <param name="orderFormNumber">订单号</param>
        /// <param name="goods">物品信息</param>
        /// <param name="storageID">库房ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功的标志</returns>
        public bool SetPriceInfo(string orderFormNumber, S_MaterialListRejectBill goods, string storageID, out string error)
        {
            decimal factUnitPrice = 0;
            decimal planUnitPrice = 0;

            if (!string.IsNullOrEmpty(orderFormNumber))
            {
                if (orderFormNumber.Length > 3 && orderFormNumber.Substring(0, 3) == "BFD")
                {
                    factUnitPrice = m_storeServer.GetFactUnitPrice(goods.GoodsID, goods.Provider, goods.BatchNo, storageID);
                }
                else
                {
                    string strSql = "select b.UnitPrice/(1+cess/100) as UnitPrice from B_OrderFormInfo as a " +
                    " inner join B_BargainInfo as d on a.BargainNumber = d.BargainNumber " +
                    " inner join B_BargainGoods as b on a.BargainNumber = b.BargainNumber " +
                    " where a.orderFormNumber = '" + orderFormNumber + "' and b.GoodsID = " + Convert.ToInt32(goods.GoodsID);

                    DataTable dtBargainPrice = GlobalObject.DatabaseServer.QueryInfo(strSql);

                    if (dtBargainPrice.Rows.Count == 0)
                    {
                        factUnitPrice = 0;
                    }
                    else
                    {
                        //委外报检的物品单价直接从委外报检入库单中的单价金额获得
                        if (goods.BatchNo.Contains("WJD"))
                        {
                            strSql = " select UnitPrice from S_CheckOutInDepotForOutsourcingBill where Bill_ID = '"
                                + goods.BatchNo + "' ";

                            DataTable dtRawMaterialPrice = GlobalObject.DatabaseServer.QueryInfo(strSql);

                            if (dtRawMaterialPrice.Rows.Count == 0)
                            {
                                factUnitPrice = 0;
                            }
                            else
                            {
                                factUnitPrice = Convert.ToDecimal(dtRawMaterialPrice.Rows[0][0]);
                            }
                        }
                        else
                        {
                            factUnitPrice = Convert.ToDecimal(dtBargainPrice.Rows[0][0].ToString());
                        }
                    }
                }
            }
            

            if (!m_basicGoodsServer.GetPlanUnitPrice(goods.GoodsID, out planUnitPrice, out error))
            {
                return false;
            }

            goods.PlanUnitPrice = planUnitPrice;
            goods.PlanPrice = planUnitPrice * goods.Amount;
            goods.UnitPrice = factUnitPrice;
            goods.Price = factUnitPrice * goods.Amount;
            goods.TotalPrice = CalculateClass.GetTotalPrice(goods.Price);

            return true;
        }

        /// <summary>
        /// 添加物品信息
        /// </summary>
        /// <param name="orderFormNumber">订单号</param>
        /// <param name="goods">物品信息</param>
        /// <param name="storageID">库房ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功的标志</returns>
        public bool AddGoods(string orderFormNumber, S_MaterialListRejectBill goods, string storageID, out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                if (!SetPriceInfo(orderFormNumber, goods,storageID, out error))
                {
                    return false;
                }

                if (!UpdateAssicotaeBillID(dataContxt,goods.Bill_ID,goods.GoodsID,goods.BatchNo,out error))
                {
                    return false;
                }

                dataContxt.S_MaterialListRejectBill.InsertOnSubmit(goods);
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

                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from r in dataContxt.S_MaterialListRejectBill
                             where idList.Contains(r.ID)
                             select r;

                if (result.Count() > 0)
                {

                    foreach (var item in result)
                    {
                        if (!ClearGoodsDate(dataContxt, item.Bill_ID, item.GoodsID, item.BatchNo, out error))
                        {
                            return false;
                        }
                    }

                    dataContxt.S_MaterialListRejectBill.DeleteAllOnSubmit(result);
                    dataContxt.SubmitChanges();
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
        /// 删除某采购退货单下的所有物品信息
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="billNo">退货单号</param>
        public void DeleteGoods(DepotManagementDataContext context, string billNo)
        {
            var result = from r in context.S_MaterialListRejectBill 
                         where r.Bill_ID == billNo 
                         select r;

            context.S_MaterialListRejectBill.DeleteAllOnSubmit(result);
        }

        /// <summary>
        /// 检查物品库存是否大于等于指定值
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <param name="count">要比较的数量</param>
        /// <param name="provider">供应商</param>
        /// <param name="storageID">库房ID</param>
        /// <returns> >= 指定值返回true </returns>
        public bool IsGoodsStockThan(int goodsID, string batchNo, decimal count, string provider, string storageID)
        {
            string strSql = "select case when b.ID is null then a.ExistCount else " +
                " a.ExistCount - b.QC_BFS - b.QC_FQS end as StockCount " +
                " from S_Stock as a left join S_IsolationManageBill as b " +
                " on a.GoodsId = b.GoodsID and a.BatchNo = b.BatchNo and a.StorageID = b.StorageID " +
                " and DJZT not in ('已报废','单据已完成') where a.GoodsID = "
                + goodsID + " and a.BatchNo = '" + batchNo + "' and a.Provider = '" + provider 
                + "' and a.StorageID = '" + storageID + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dt.Rows.Count != 1)
            {
                return false;
            }
            else
            {
                if (Convert.ToDecimal(dt.Rows[0]["StockCount"]) >= count)
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
        /// 删除某采购退货单下的所有物品信息
        /// </summary>
        /// <param name="billNo">退货单号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功的标志</returns>
        public bool DeleteGoods(string billNo, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from r in dataContxt.S_MaterialListRejectBill 
                             where r.Bill_ID == billNo 
                             select r;

                foreach (var item in result)
                {
                    if (!ClearGoodsDate(dataContxt, item.Bill_ID, item.GoodsID, item.BatchNo, out error))
                    {
                        return false;
                    }
                }

                dataContxt.S_MaterialListRejectBill.DeleteAllOnSubmit(result);
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
        /// 更新物品信息
        /// </summary>
        /// <param name="orderFormNumber">订单号</param>
        /// <param name="goods">物品信息</param>
        /// <param name="storageID">库房ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功的标志</returns>
        public bool UpdateGoods(string orderFormNumber, S_MaterialListRejectBill goods, string storageID, out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from r in dataContxt.S_MaterialListRejectBill
                             where r.ID == goods.ID
                             select r;

                if (result.Count() > 0)
                {
                    S_MaterialListRejectBill updateGoods = result.Single();

                    updateGoods.Bill_ID = goods.Bill_ID;
                    updateGoods.GoodsID = goods.GoodsID;
                    updateGoods.Provider = goods.Provider;
                    updateGoods.BatchNo = goods.BatchNo;
                    updateGoods.ProviderBatchNo = goods.ProviderBatchNo;
                    updateGoods.Amount = goods.Amount;
                    updateGoods.Remark = goods.Remark;

                    if (!SetPriceInfo(orderFormNumber, updateGoods,storageID, out error))
                    {
                        return false;
                    }

                    dataContxt.SubmitChanges();
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
        /// 赋值账务信息
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="bill">单据信息</param>
        /// <param name="item">明细信息</param>
        /// <returns>返回账务信息</returns>
        public S_InDepotDetailBill AssignDetailInfo(DepotManagementDataContext context, S_MaterialRejectBill bill,
            S_MaterialListRejectBill item)
        {
            IBillTypeServer server = ServerModuleFactory.GetServerModule<IBillTypeServer>();
            BASE_BillType billType = server.GetBillTypeFromName("采购退货单");

            if (billType == null)
            {
                throw new Exception("获取不到单据类型信息");
            }


            S_InDepotDetailBill detailBill = new S_InDepotDetailBill();

            detailBill.ID = Guid.NewGuid();
            detailBill.BillTime = (DateTime)bill.OutDepotDate;
            detailBill.FillInPersonnel = bill.FillInPersonnel;
            detailBill.Department = UniversalFunction.GetDeptInfo(context, bill.Department).部门名称;
            detailBill.FactPrice = -Math.Round(item.UnitPrice * item.Amount, 2);
            detailBill.FactUnitPrice = item.UnitPrice;
            detailBill.GoodsID = item.GoodsID;
            detailBill.BatchNo = item.BatchNo;
            detailBill.Provider = item.Provider;
            detailBill.InDepotBillID = bill.Bill_ID;
            detailBill.InDepotCount = -item.Amount;
            detailBill.UnitPrice = item.UnitPrice;
            detailBill.Price = -Math.Round(item.UnitPrice * item.Amount, 2);
            detailBill.OperationType = (int)GlobalObject.CE_SubsidiaryOperationType.采购退货;
            detailBill.StorageID = bill.StorageID;
            detailBill.Remark = "退货原因：" + bill.Reason + " 备注：" + bill.Remark;
            detailBill.AffrimPersonnel = bill.DepotManager;
            detailBill.FillInDate = bill.Bill_Time;

            return detailBill;
        }

        /// <summary>
        /// 赋值库存信息
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="bill">单据信息</param>
        /// <param name="item">明细信息</param>
        /// <returns>返回库存信息</returns>
        public S_Stock AssignStockInfo(DepotManagementDataContext context, S_MaterialRejectBill bill,
            S_MaterialListRejectBill item)
        {
            F_GoodsPlanCost info = m_basicGoodsServer.GetGoodsInfo(context, item.GoodsID);

            S_Stock stockInfo = new S_Stock();

            stockInfo.GoodsID = info.ID;
            stockInfo.GoodsCode = info.GoodsCode;
            stockInfo.GoodsName = info.GoodsName;
            stockInfo.ExistCount = item.Amount;
            stockInfo.Spec = info.Spec;
            stockInfo.Provider = item.Provider;
            stockInfo.BatchNo = item.BatchNo;
            stockInfo.StorageID = bill.StorageID;

            return stockInfo;
        }

        /// <summary>
        /// 获得报废物品信息
        /// </summary>
        /// <param name="provider">报废物品供应商</param>
        /// <returns>报废物品供应商不为空是返回供应商对应的报废物品列表，报废物品供应商为空串时返回所有报废物品信息</returns>
        public DataTable GetScrapGoods(string provider)
        {
            string strSql = " select 0 as Sel,a.Bill_ID,图号型号 as GoodsCode,物品名称 as GoodsName,规格 as Spec,物品类别 as GoodsType, " +
            " Provider,ResponsibilityProvider,a.BatchNo,CVTNumber,Reason,Quantity," +
            " a.Remark,WorkingHours,c.Bill_Time,a.GoodsID from S_ScrapGoods as a inner join" +
            " View_F_GoodsPlanCost as b on a.GoodsID= b.序号 inner join S_ScrapBill as c " +
            " on a.Bill_ID = c.Bill_ID left join (select GoodsID,AssociateID,BatchNo from S_MaterialRejectBill as a "+
            " inner join  S_MaterialListRejectBill as b on a.Bill_ID = b.Bill_ID "+
            " and BillType = '报废库退货单') as d on a.GoodsID = d.GoodsID and a.BatchNo = d.BatchNo and c.Bill_ID = d.AssociateID "+
            " where ResponsibilityDepartment  = 'GYS' and d.AssociateID is null " +
            " and Provider = ResponsibilityProvider and (IsReject is null or IsReject = '' or IsReject = '0')";

            if (provider == "")
            {
                strSql += " order by a.Provider ";
            }
            else
            {
                strSql += " and Provider = '" + provider + "'order by a.Bill_ID ";
            }

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dt.Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return dt;
            }
        }

        /// <summary>
        /// 获得对应隔离单信息
        /// </summary>
        /// <param name="orderFormNumber">订单号</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>成功返回获取到的隔离单信息，失败返回null</returns>
        public DataTable GetIsolationBill(string orderFormNumber, string storageID)
        {
            string strSql = " select 0 as Sel,DJH as 隔离单号,物品ID, "+
                            " 物品名称,图号型号,规格,供货单位,批次号,供方批次号,"+
                            " QC_THS as 退货数 from S_IsolationManageBill as a "+
                            " inner join   View_S_Stock as b "+
                            " on a.GoodsID = b.物品ID and a.BatchNo = b.批次号  " +
                            " where AssociateRejectBillID is  null and DJZT = '等待采购退货' ";

            if (orderFormNumber == "")
            {
                strSql += " order by DJH ";
            }
            else
            {
                strSql +=  " and GoodsID in (" +
                                " select GoodsID from B_OrderFormGoods "+
                                " where OrderFormNumber = '" + orderFormNumber + "')  and StorageID = '" + storageID + "' order by DJH ";
            }

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dt.Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return dt;
            }
        }

        /// <summary>
        /// 清除隔离单关联的采购退货单单号信息
        /// </summary>
        /// <param name="context">LINQ 数据上下文</param>
        /// <param name="billNo">隔离单关联的采购退货单的单据号</param>
        /// <param name="goodsID">隔离物品ID</param>
        /// <param name="batchNo">隔离物品批次号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        private bool ClearGoodsDate(DepotManagementDataContext context,
            string billNo, int goodsID, string batchNo, out string error)
        {
            error = null;

            try
            {
                var varData = from a in context.S_IsolationManageBill
                              where a.GoodsID == goodsID
                              && a.BatchNo == batchNo
                              && a.AssociateRejectBillID == billNo
                              && a.DJZT == "等待采购退货"
                              select a;

                if (varData.Count() == 1)
                {
                    S_IsolationManageBill lnqIsolation = varData.Single();

                    lnqIsolation.AssociateRejectBillID = null;
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
        /// 更改隔离单中关联单据号
        /// </summary>
        /// <param name="context">LINQ 数据上下文</param>
        /// <param name="billNo">隔离单关联的采购退货单的新单据号</param>
        /// <param name="goodsID">隔离物品ID</param>
        /// <param name="batchNo">隔离物品批次号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool UpdateAssicotaeBillID(DepotManagementDataContext context,
            string billNo, int goodsID, string batchNo, out string error)
        {
            try
            {
                error = null;

                var varData = from a in context.S_IsolationManageBill
                              where a.GoodsID == goodsID
                              && a.BatchNo == batchNo
                              && a.DJZT == "等待采购退货"
                              select a;

                if (varData.Count() == 1)
                {
                    S_IsolationManageBill lnqIsolation = varData.Single();

                    lnqIsolation.AssociateRejectBillID = billNo;
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
