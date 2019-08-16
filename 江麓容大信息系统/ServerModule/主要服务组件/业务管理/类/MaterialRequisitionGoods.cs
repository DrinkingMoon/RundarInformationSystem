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
    /// 领料单物品信息服务
    /// </summary>
    class MaterialRequisitionGoodsServer : BasicServer, IMaterialRequisitionGoodsServer
    {
        /// <summary>
        /// 基础物品服务组件
        /// </summary>
        IBasicGoodsServer m_basicGoodsServer = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();

        /// <summary>
        /// 还货服务组件
        /// </summary>
        IProductReturnService m_serverReturn = ServerModuleFactory.GetServerModule<IProductReturnService>();

        bool BillIsFinish(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            return BillIsFinish(ctx, billNo);
        }

        bool BillIsFinish(DepotManagementDataContext ctx, string billNo)
        {
            var vardata = from a in ctx.S_MaterialRequisition
                          where a.Bill_ID == billNo
                          select a;

            if (vardata.Count() == 0)
            {
                return false;
            }
            else if (vardata.Count() == 1)
            {
                S_MaterialRequisition billInfo = vardata.Single();

                if (billInfo.BillStatus == MaterialRequisitionBillStatus.已出库.ToString())
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
        /// 检测报废项目
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="count">出库数量</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool CheckScrapGoods(string billID, int goodsID, decimal count, out string error)
        {
            error = null;

            string strSql = " select distinct b.* ,b.ScrapCount - b.SendCount as Count from S_MaterialRequisition  as a inner join " +
                    " (select a.Bill_ID,a.GoodsID,case when a.ScrapCount is null then 0 else a.ScrapCount end as ScrapCount, "+
                    " case when b.SendCount is null then 0 else b.SendCount end as SendCount " +
                    " from (select Sum(Quantity) as ScrapCount,GoodsID,Bill_ID  "+
                    " from  S_ScrapGoods group by GoodsID,Bill_ID) as a left join " +
                    " (select Sum(RealCount) as SendCount,GoodsID,AssociatedBillNo "+
                    " from S_MaterialRequisitionGoods as a inner join S_MaterialRequisition as b on a.Bill_ID = b.Bill_ID "+
                    " group by GoodsID,AssociatedBillNo) as b on a.GoodsID = b.GoodsID and a.Bill_ID = b.AssociatedBillNo "+
                    " ) as b on a.AssociatedBillNo = b.Bill_ID where b.GoodsID = " + goodsID + " and a.Bill_ID = '" + billID + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dt.Rows.Count == 0)
            {
                error = "此物品不存在所关联的报废单中，请重新核对";
                return false;
            }
            else
            {
                if (count > Convert.ToDecimal(dt.Rows[0]["Count"]))
                {
                    error = "此物品的出库数已经超出报废数，请重新核对";
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// 物品比较器
        /// </summary>
        class GoodsComparer : IEqualityComparer<View_S_MaterialRequisitionGoods>
        {
            public bool Equals(View_S_MaterialRequisitionGoods x, View_S_MaterialRequisitionGoods y)
            {
                return x.图号型号.Equals(y.图号型号) && x.物品名称.Equals(y.物品名称) && x.规格.Equals(y.规格) &&
                    x.供应商编码.Equals(y.供应商编码) && x.批次号.Equals(y.批次号);
            }

            public int GetHashCode(View_S_MaterialRequisitionGoods obj)
            {
                return 0;
            }
        }

        /// <summary>
        /// 插入领料明细
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="listInfo">明细LINQ数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool AutoCreateGoods(DepotManagementDataContext ctx, S_MaterialRequisitionGoods listInfo, out string error)
        {
            error = null;

            try
            {
                if (BillIsFinish(ctx, listInfo.Bill_ID))
                {
                    throw new Exception("单据已完成，无法进行操作");
                }

                ctx.S_MaterialRequisitionGoods.InsertOnSubmit(listInfo);

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 检查某业务是否存在
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">业务号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.S_MaterialRequisitionGoods
                          where a.Bill_ID == billNo
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
        /// 检查是否存在某单据物品清单
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            return (from r in ctx.S_MaterialRequisitionGoods
                    where r.Bill_ID == billNo
                    select r).Count() > 0;
        }

        /// <summary>
        /// 获取指定领料单的物品信息
        /// </summary>
        /// <param name="billNo">领料单号</param>
        /// <returns>返回获取的物品信息</returns>
        public IEnumerable<View_S_MaterialRequisitionGoods> GetGoods(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            return (from r in ctx.View_S_MaterialRequisitionGoods
                    where r.领料单号 == billNo
                    orderby r.显示位置
                    select r).AsEnumerable().Distinct(new GoodsComparer());
        }

        #region 夏石友，2012-07-13 15:30

        /// <summary>
        /// 获取关联单号对应的物品明细
        /// </summary>
        /// <param name="associatedBillNo">关联单号</param>
        /// <returns>返回获取到的物品明细信息, 没有时结果信息数量为0</returns>
        public IQueryable<View_S_MaterialRequisitionGoods> GetGoodsOfAssociatedBill(string associatedBillNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var bill = from goods in ctx.View_S_MaterialRequisitionGoods
                       join r in ctx.S_MaterialRequisition
                       on goods.领料单号 equals r.Bill_ID into joinTemp
                       from temp in joinTemp.DefaultIfEmpty()
                       where temp.AssociatedBillNo == associatedBillNo
                       select goods;

            return bill;
        }

        #endregion

        /// <summary>
        /// 获取所有领料单对指定关联单据的指定物品实际领料数
        /// </summary>
        /// <param name="associatedBillNo">关联单号</param>
        /// <param name="goodsID">物品ID</param>
        /// <returns>获取此物品已经领料的数量</returns>
        public Decimal GetGoodsAmount(string associatedBillNo, int goodsID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.S_MaterialRequisition
                          join b in ctx.S_MaterialRequisitionGoods
                          on a.Bill_ID equals b.Bill_ID
                          where a.AssociatedBillNo == associatedBillNo
                          && a.BillStatus != "已报废"
                          && b.GoodsID == goodsID
                          select b.RealCount;

            if (varData.Count() == 0)
            {
                return 0;
            }
            else
            {
                return varData.Sum();
            }
        }

        /// <summary>
        /// 获取总成装配物品明细
        /// </summary>
        /// <param name="assemblyName">总成名称</param>
        /// <returns>返回获取到的总成装配物品明细</returns>
        public IQueryable<View_S_AssemblyGoodsBil> GetAssemblyGoodsBill(string assemblyName)
        {
            if (GlobalObject.GeneralFunction.IsNullOrEmpty(assemblyName))
            {
                return null;
            }

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            return from r in ctx.View_S_AssemblyGoodsBil where r.父总成名称 == assemblyName select r;
        }

        /// <summary>
        /// 获取指定领料单的物品信息
        /// </summary>
        /// <param name="billNo">领料单号</param>
        /// <param name="goodsBarCode">条形码物品表信息</param>
        /// <returns>返回获取的物品信息</returns>
        public View_S_MaterialRequisitionGoods GetGoods(string billNo, S_InDepotGoodsBarCodeTable goodsBarCode)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            View_F_GoodsPlanCost basicGoods = m_basicGoodsServer.GetGoodsInfoView(goodsBarCode.GoodsID);

            var result = from r in ctx.View_S_MaterialRequisitionGoods
                         where r.领料单号 == billNo && r.图号型号 == basicGoods.图号型号 
                         && r.物品名称 == basicGoods.物品名称 && r.规格 == basicGoods.规格 
                         && r.供应商编码 == goodsBarCode.Provider && r.批次号 == goodsBarCode.BatchNo
                         select r;

            if (result.Count() == 0)
            {
                return null;
            }

            return result.Single();
        }

        /// <summary>
        /// 明细操作
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">单据号</param>
        /// <param name="detailInfo">明细信息</param>
        /// <param name="mode">操作类型</param>
        void OperationDetailInfo(DepotManagementDataContext ctx, string billNo, S_MaterialRequisitionGoods detailInfo, CE_OperatorMode mode)
        {
            try
            {
                var varBill = from a in ctx.S_MaterialRequisition
                              where a.Bill_ID == billNo
                              select a;

                if (varBill.Count() != 1)
                {
                    throw new Exception("【单据号】:" + billNo + " 不存在或者不唯一");
                }

                if (varBill.Single().BillStatus == MaterialRequisitionBillStatus.已出库.ToString())
                {
                    throw new Exception("【单据号】:" + billNo + " 已出库 无法进行操作");
                }

                if (mode == CE_OperatorMode.添加 || mode == CE_OperatorMode.删除 || mode == CE_OperatorMode.修改)
                {
                    if (detailInfo == null)
                    {
                        throw new Exception("需要操作的记录为空，无法操作【" + mode.ToString() + "】");
                    }

                    var varData = from a in ctx.S_MaterialRequisitionGoods
                                  where a.ID == detailInfo.ID
                                  select a;

                    switch (mode)
                    {
                        case CE_OperatorMode.添加:

                            if (varData.Count() == 1)
                            {
                                S_MaterialRequisitionGoods tempInfo = varData.Single();

                                tempInfo.Bill_ID = billNo;
                                tempInfo.GoodsID = detailInfo.GoodsID;
                                tempInfo.BatchNo = detailInfo.BatchNo;
                                tempInfo.BasicCount = detailInfo.BasicCount;
                                tempInfo.ProviderCode = detailInfo.ProviderCode;
                                tempInfo.RealCount = detailInfo.RealCount;
                                tempInfo.Remark = detailInfo.Remark;
                                tempInfo.RepairStatus = detailInfo.RepairStatus;
                                tempInfo.RequestCount = detailInfo.RequestCount;
                                tempInfo.ShowPosition = detailInfo.ShowPosition;
                            }
                            else if (varData.Count() == 0)
                            {
                                detailInfo.Bill_ID = billNo;
                                ctx.S_MaterialRequisitionGoods.InsertOnSubmit(detailInfo);
                            }
                            else
                            {
                                throw new Exception(UniversalFunction.GetGoodsMessage(ctx, detailInfo.GoodsID) + "【批次号】:"
                                    + detailInfo.BatchNo + "【供应商】：" + detailInfo.ProviderCode + " 数据不唯一");
                            }

                            break;
                        case CE_OperatorMode.修改:

                            if (varData.Count() == 1)
                            {
                                S_MaterialRequisitionGoods tempInfo = varData.Single();

                                tempInfo.Bill_ID = billNo;
                                tempInfo.GoodsID = detailInfo.GoodsID;
                                tempInfo.BatchNo = detailInfo.BatchNo;
                                tempInfo.BasicCount = detailInfo.BasicCount;
                                tempInfo.ProviderCode = detailInfo.ProviderCode;
                                tempInfo.RealCount = detailInfo.RealCount;
                                tempInfo.Remark = detailInfo.Remark;
                                tempInfo.RepairStatus = detailInfo.RepairStatus;
                                tempInfo.RequestCount = detailInfo.RequestCount;
                                tempInfo.ShowPosition = detailInfo.ShowPosition;
                            }
                            else
                            {
                                throw new Exception(UniversalFunction.GetGoodsMessage(ctx, detailInfo.GoodsID) + "【批次号】:"
                                    + detailInfo.BatchNo + "【供应商】：" + detailInfo.ProviderCode + " 数据不唯一或者为空");
                            }

                            break;
                        case CE_OperatorMode.删除:
                            ctx.S_MaterialRequisitionGoods.DeleteAllOnSubmit(varData);
                            break;
                        default:
                            break;
                    }
                }
                else if (mode == CE_OperatorMode.批量删除)
                {
                    if (billNo == null)
                    {
                        throw new Exception("单据号为空，无法操作【" + mode.ToString() + "】");
                    }

                    var varAllDelete = from a in ctx.S_MaterialRequisitionGoods
                                       where a.Bill_ID == billNo
                                       select a;
                    ctx.S_MaterialRequisitionGoods.DeleteAllOnSubmit(varAllDelete);
                }

               
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 添加物品信息
        /// </summary>
        /// <param name="goods">物品信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功的标志</returns>
        public bool AddGoods(S_MaterialRequisitionGoods goods, out string error)
        {
            try
            {
                error = null;
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
                OperationDetailInfo(ctx, goods.Bill_ID, goods, CE_OperatorMode.添加);
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
        public bool AddGoods(List<View_S_MaterialRequisitionGoods> lstGoods, out string error)
        {
            try
            {
                error = null;
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
                for (int i = 0; i < lstGoods.Count; i++)
                {
                    List<View_S_MaterialRequisitionGoods> sameGoods = lstGoods.FindAll(
                        p => p.图号型号 == lstGoods[i].图号型号 && p.物品名称 == lstGoods[i].物品名称
                            && p.规格 == lstGoods[i].规格 && p.供应商编码 == lstGoods[i].供应商编码 
                            && p.批次号 == lstGoods[i].批次号);

                    if (sameGoods.Count > 1)
                    {
                        decimal amount = 0;

                        foreach (var item in sameGoods)
                        {
                            amount += item.请领数;
                        }

                        lstGoods[i].请领数 = amount;

                        for (int j = i + 1; j < lstGoods.Count; j++)
                        {
                            if (lstGoods[i].图号型号 == lstGoods[j].图号型号 && lstGoods[i].物品名称 
                                == lstGoods[j].物品名称 && lstGoods[i].规格 == lstGoods[j].规格 &&
                                lstGoods[i].供应商编码 == lstGoods[j].供应商编码 && lstGoods[i].批次号 == lstGoods[j].批次号)
                            {
                                lstGoods.RemoveAt(j--);
                            }
                        }
                    }
                }

                foreach (var item in lstGoods)
                {
                    S_MaterialRequisitionGoods mrg = new S_MaterialRequisitionGoods();

                    mrg.BasicCount = item.基数;
                    mrg.BatchNo = item.批次号;
                    mrg.Bill_ID = item.领料单号;
                    mrg.GoodsID = item.物品ID;
                    mrg.ProviderCode = item.供应商编码;
                    mrg.RealCount = item.实领数;
                    mrg.RequestCount = item.请领数;
                    mrg.Remark = item.备注;
                    mrg.ShowPosition = item.显示位置;

                    OperationDetailInfo(ctx, mrg.Bill_ID, mrg, CE_OperatorMode.添加);
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

                var result = from r in ctx.S_MaterialRequisitionGoods
                             where idList.Contains(r.ID)
                             select r;

                if (result.Count() > 0)
                {
                    foreach (S_MaterialRequisitionGoods item in result)
                    {
                        m_serverReturn.DeleteInfo(ctx, item.Bill_ID, item.GoodsID, item.BatchNo, item.ProviderCode);
                        OperationDetailInfo(ctx, item.Bill_ID, item, CE_OperatorMode.删除);
                    }
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
        /// 删除某领料单下的所有物品信息
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="billNo">领料单号</param>
        public void DeleteGoods(DepotManagementDataContext context, string billNo)
        {
            var result = from r in context.S_MaterialRequisitionGoods
                         where r.Bill_ID == billNo
                         select r;

            m_serverReturn.DeleteInfo(context, billNo);
            OperationDetailInfo(context, billNo, null, CE_OperatorMode.批量删除);
        }

        /// <summary>
        /// 删除某领料单下的所有物品信息
        /// </summary>
        /// <param name="billNo">领料单号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功的标志</returns>
        public bool DeleteGoods(string billNo, out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
                DeleteGoods(ctx, billNo);
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
        public bool UpdateGoods(S_MaterialRequisitionGoods goods, out string error)
        {
            try
            {
                error = null;
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
                OperationDetailInfo(ctx, goods.Bill_ID, goods, CE_OperatorMode.修改);
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
        /// 由无线接收信息更新实际领取物品的信息
        /// </summary>
        /// <param name="goods">物品信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功的标志</returns>
        public bool UpdateyGoodsFromWireless(S_MaterialRequisitionGoods goods, out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var result = from r in ctx.S_MaterialRequisitionGoods
                             where r.Bill_ID == goods.Bill_ID && r.GoodsID == goods.GoodsID
                             select r;

                if (result.Count() > 0)
                {
                    var findGoods = from r in result 
                                    where r.ProviderCode == goods.ProviderCode && r.BatchNo.Contains(goods.BatchNo) 
                                    select r;

                    if (findGoods.Count() > 0)
                    {
                        S_MaterialRequisitionGoods info = findGoods.Single();

                        info.RealCount = goods.RealCount;

                        if (!info.Remark.Contains("无线领料"))
                        {
                            info.Remark = string.Format("{0},无线领料", findGoods.Single().Remark);
                        }
                    }
                    else
                    {
                        if (!result.Single().Remark.Contains("无线领料"))
                        {
                            goods.Remark = string.Format("{0},无线领料", result.Single().Remark);
                        }
                        else
                        {
                            goods.Remark = result.Single().Remark;
                        }

                        ctx.S_MaterialRequisitionGoods.InsertOnSubmit(goods);
                    }

                    ctx.SubmitChanges();
                    return true;
                }
                else
                {
                    error = "领料单中不存在此物品";
                    return false;
                }
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
        public S_FetchGoodsDetailBill AssignDetailInfo(DepotManagementDataContext context, S_MaterialRequisition bill,
            S_MaterialRequisitionGoods item)
        {

            IBillTypeServer server = ServerModuleFactory.GetServerModule<IBillTypeServer>();
            BASE_BillType billType = server.GetBillTypeFromName("领料单");

            if (billType == null)
            {
                throw new Exception( "获取不到单据类型信息");
            }

            View_Department department = UniversalFunction.GetDeptInfo(context, bill.Department);
            IStoreServer storeServer = ServerModuleFactory.GetServerModule<IStoreServer>();
            IProductLendReturnService serverLendReturn = ServerModuleFactory.GetServerModule<IProductLendReturnService>();
            F_GoodsPlanCost basicGoods = m_basicGoodsServer.GetGoodsInfo(context, item.GoodsID);

            if (basicGoods == null)
            {
                throw new Exception( string.Format("物品ID [{0}] 的物品在基础物品表中没有查到，请与系统管理员联系！", item.GoodsID));
            }

            StoreQueryCondition condition = new StoreQueryCondition();

            condition.GoodsID = item.GoodsID;
            condition.Provider = item.ProviderCode;
            condition.BatchNo = item.BatchNo;
            condition.StorageID = bill.StorageID;

            S_Stock stock = storeServer.GetStockInfoOverLoad(context, condition);

            if (stock == null && GlobalObject.GeneralFunction.IsNullOrEmpty(basicGoods.GoodsType))
            {
                throw new Exception( string.Format("图号：{0}, 名称：{1}, 规格：{2}, 供应商：{3}, 批次号：{4} 的物品在库存中没有查到相关物品，请仓管员核实！",
                    basicGoods.GoodsCode, basicGoods.GoodsName, basicGoods.Spec, item.ProviderCode, item.BatchNo));
            }

            //S_FetchGoodsDetailBill用于存放每次领料、领料退库的明细信息
            decimal dcRealCount = item.RealCount;
            decimal dcSumCount = 0;

            var varData = from a in context.View_S_MaterialRequisitionProductReturnList
                          where a.单据号 == item.Bill_ID
                          && a.还账物品ID == item.GoodsID
                          && a.还账物品批次号 == item.BatchNo
                          && a.还账物品供应商 == item.ProviderCode
                          select a;

            if (varData.Count() > 0)
            {
                dcSumCount = varData.Sum(a => a.还账数量);

                if (dcRealCount < dcSumCount)
                {
                    throw new Exception("实际领用数量不能大于还货数量，请重新核对");
                }
            }

            S_FetchGoodsDetailBill detailBill = new S_FetchGoodsDetailBill();

            detailBill.ID = Guid.NewGuid();
            detailBill.FetchBIllID = bill.Bill_ID;
            detailBill.BillTime = (DateTime)bill.OutDepotDate;
            detailBill.AssociatedBillType = bill.AssociatedBillType;
            detailBill.AssociatedBillNo = bill.AssociatedBillNo;
            detailBill.Department = department.部门名称;
            detailBill.FetchCount = item.RealCount;
            detailBill.GoodsID = item.GoodsID;
            detailBill.StorageID = bill.StorageID;
            detailBill.Price = dcSumCount;
            detailBill.UnitPrice = stock == null ? 0 : stock.UnitPrice;
            detailBill.OperationType = (int)CE_SubsidiaryOperationType.领料;
            detailBill.Provider = item.ProviderCode;

            if (stock != null)
            {
                detailBill.ProviderBatchNo = stock.ProviderBatchNo;
            }
            else
            {
                detailBill.ProviderBatchNo = "";
            }

            detailBill.BatchNo = item.BatchNo;

            detailBill.FillInPersonnel = bill.FillInPersonnel;
            detailBill.FinanceSignatory = null;
            detailBill.DepartDirector = bill.DepartmentDirector;
            detailBill.DepotManager = bill.DepotManager;
            detailBill.Remark = (bill.Remark == null ? "" : bill.Remark.Trim()) + (item.Remark == null ? "" : item.Remark.Trim());
            detailBill.FillInDate = bill.Bill_Time;

            IMaterialRequisitionPurposeServer purposeServer =
                ServerModuleFactory.GetServerModule<IMaterialRequisitionPurposeServer>();

            detailBill.Using = purposeServer.GetBillPurpose(context, bill.PurposeCode).Purpose;

            return detailBill;
        }

        /// <summary>
        /// 赋值库存信息
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="bill">单据信息</param>
        /// <param name="item">明细信息</param>
        /// <returns>返回库存信息对象</returns>
        public S_Stock AssignStockInfo(DepotManagementDataContext context, S_MaterialRequisition bill,
            S_MaterialRequisitionGoods item)
        {
            F_GoodsPlanCost basicGoods = m_basicGoodsServer.GetGoodsInfo(context, item.GoodsID);

            if (item.RealCount == 0)
            {
                throw new Exception( string.Format("图号：{0}, 名称：{1}, 规格：{2}, 供应商：{3}, 批次号：{4} 的物品实领数为0，请重新输入！",
                    basicGoods.GoodsCode, basicGoods.GoodsName, basicGoods.Spec, item.ProviderCode, item.BatchNo));
            }

            S_Stock stockInfo = new S_Stock();

            stockInfo.GoodsID = item.GoodsID;

            if (basicGoods != null)
            {
                stockInfo.GoodsCode = basicGoods.GoodsCode;
                stockInfo.GoodsName = basicGoods.GoodsName;
                stockInfo.Spec = basicGoods.Spec;
            }

            stockInfo.ExistCount = item.RealCount;
            stockInfo.Provider = item.ProviderCode;
            stockInfo.BatchNo = item.BatchNo;
            stockInfo.StorageID = bill.StorageID;

            if (item.Remark != null && item.Remark.Contains("无线领料"))
            {
                item.Remark = item.Remark.Replace(",无线领料", "");
                item.Remark = item.Remark.Replace("无线领料", "");  // 防止用户把前面的逗号删除
            }

            return stockInfo;
        }

        /// <summary>
        /// 检查材料类别是否同属于同一个仓库
        /// </summary>
        /// <param name="nowDepotCode">当前的材料类别编码</param>
        /// <param name="befDepotCode">之前的材料类别编码</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>同属于返回True，不存在或者不属于返回False</returns>
        public bool CheckDepot(string nowDepotCode, string befDepotCode, string storageID)
        {
            string strSql = " select * from S_DepotForDtp where DtpCode in (" +
                            " select DtpCode from S_DepotForDtp as a inner join " +
                            " S_DepotTypeForPersonnel as b on a.DtpCode = b.ZlID" +
                            " inner join BASE_StorageAndPersonnel as c on b.PersonnelID = c.WorkID " +
                            " where  storageID = '" + storageID + "' and DepotCode = '" + nowDepotCode + "')";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dt.Rows.Count == 0)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["DepotCode"].ToString() == befDepotCode)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 判断物品是否在BOM混装表中
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <returns>存在返回True，不存在返回False</returns>
        public bool IsInJumblyBomGoods(string goodsCode ,string goodsName ,string spec)
        {
            string strSql = "select * from P_JumblyBomGoods where BomGoodsCode = '" 
                + goodsCode + "' and BomGoodsName = '" + goodsName + "' and BomSpec = '" 
                + spec + "' and IsJumbly = 1";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 根据单据列表合计物品需要领用的数量
        /// </summary>
        /// <param name="listBillNo">单据列表</param>
        /// <returns>返回物品领用数量信息字典</returns>
        public Dictionary<int, decimal> SumListBillNoInfo(List<string> listBillNo)
        {
            Dictionary<int, decimal> dicResult = new Dictionary<int, decimal>();

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;


            var varData = from a in ctx.S_MaterialRequisitionGoods
                          where listBillNo.Contains(a.Bill_ID)
                          group a by a.GoodsID into g
                          select new
                          {
                              g.Key,
                              SumCount = g.Sum(p => p.RealCount)
                          };

            foreach (var item in varData)
            {
                dicResult.Add(Convert.ToInt32(item.Key), Convert.ToDecimal(item.SumCount));
            }

            return dicResult;
        }

        public void InsertInfoExcel(string billNo, DataTable infoTable)
        {
            IMaterialRequisitionServer billServer = ServerModuleFactory.GetServerModule<IMaterialRequisitionServer>();
            BillNumberControl billNoControl = new BillNumberControl("领料单", billServer);
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();

            IStoreServer storeService = ServerModuleFactory.GetServerModule<IStoreServer>();

            try
            {
                List<string> lstStorage = new List<string>();

                if (infoTable.Columns.Contains("库房"))
                {
                    lstStorage = DataSetHelper.ColumnsToList_Distinct(infoTable, "库房");

                    foreach (string storage in lstStorage)
                    {
                        string temp = UniversalFunction.GetStorageID(storage);

                        if (temp.Trim().Length == 0)
                        {
                            throw new Exception("【库房名称】：" + storage + "有误， 无法匹配到库房ID");
                        }
                    }
                }

                if (!infoTable.Columns.Contains("物品ID"))
                {
                    infoTable.Columns.Add("物品ID");

                    foreach (DataRow dr in infoTable.Rows)
                    {
                        int goodsID = UniversalFunction.GetGoodsID(dr["图号型号"] == null ? "" : dr["图号型号"].ToString(), 
                            dr["物品名称"] == null ? "" : dr["物品名称"].ToString(), 
                            dr["规格"] == null ? "" : dr["规格"].ToString());

                        if (goodsID == 0)
                        {
                            throw new Exception(string.Format("【图号型号】：{0} 【物品名称】：{1} 【规格】：{2}有误，"+
                                " 无法匹配到物品ID", dr["图号型号"], dr["物品名称"], dr["规格"]));
                        }

                        dr["物品ID"] = goodsID;
                    }
                }

                var varData = from a in ctx.S_MaterialRequisition
                              where a.Bill_ID == billNo
                              select a;

                if (varData.Count() != 1)
                {
                    throw new Exception("无法获取【单据号】： " + billNo + " 的基本信息");
                }

                S_MaterialRequisition requisition = varData.Single();

                if (BillIsFinish(ctx, requisition.Bill_ID))
                {
                    throw new Exception("单据已完成，无法进行操作");
                }

                if (lstStorage.Count == 0)
                {
                    lstStorage.Add(UniversalFunction.GetStorageName(requisition.StorageID));
                }

                foreach (string storage in lstStorage)
                {
                    DataTable tempTable = infoTable;

                    if (lstStorage.Count > 1)
                    {
                        tempTable = DataSetHelper.SiftDataTable(infoTable, "库房 = '" + storage + "'");
                    }

                    string tempBillNo = "";
                    string storageID = UniversalFunction.GetStorageID(storage);

                    if (requisition.StorageID != storageID)
                    {
                        S_MaterialRequisition tempRequisition = new S_MaterialRequisition();

                        tempBillNo = billNoControl.GetNewBillNo(ctx);

                        tempRequisition.Bill_ID = tempBillNo;
                        tempRequisition.AssociatedBillNo = requisition.AssociatedBillNo;
                        tempRequisition.AssociatedBillType = requisition.AssociatedBillType;
                        tempRequisition.AuthorizeDate = requisition.AuthorizeDate;
                        tempRequisition.AuthorizePersonnel = requisition.AuthorizePersonnel;
                        tempRequisition.Bill_Time = requisition.Bill_Time;
                        tempRequisition.BillStatus = requisition.BillStatus;
                        tempRequisition.Department = requisition.Department;
                        tempRequisition.DepartmentDirector = requisition.DepartmentDirector;
                        tempRequisition.DepotManager = requisition.DepotManager;
                        tempRequisition.FetchCount = requisition.FetchCount;
                        tempRequisition.FetchType = requisition.FetchType;
                        tempRequisition.FillInPersonnel = requisition.FillInPersonnel;
                        tempRequisition.FillInPersonnelCode = requisition.FillInPersonnelCode;
                        tempRequisition.OutDepotDate = requisition.OutDepotDate;
                        tempRequisition.ProductType = requisition.ProductType;
                        tempRequisition.PurposeCode = requisition.PurposeCode;
                        tempRequisition.Remark = requisition.Remark;
                        tempRequisition.StorageID = storageID;
                        tempRequisition.TechnologistDate = requisition.TechnologistDate;
                        tempRequisition.TechnologistPersonnel = requisition.TechnologistPersonnel;

                        ctx.S_MaterialRequisition.InsertOnSubmit(tempRequisition);
                        ctx.SubmitChanges();
                    }
                    else
                    {
                        tempBillNo = billNo;
                    }

                    foreach (DataRow dr in tempTable.Rows)
                    {
                        S_MaterialRequisitionGoods goodsInfo = new S_MaterialRequisitionGoods();

                        goodsInfo.RequestCount = dr["数量"] == null ? 0 : Convert.ToDecimal(dr["数量"]);
                        goodsInfo.BatchNo = dr["批次号"] == null ? "" : dr["批次号"].ToString();

                        if (dr["物品ID"] == null || Convert.ToInt32( dr["物品ID"]) == 0)
                        {
                            throw new Exception("【物品ID】无效");
                        }

                        goodsInfo.GoodsID = Convert.ToInt32(dr["物品ID"]);
                        goodsInfo.Bill_ID = tempBillNo;

                        StoreQueryCondition condition = new StoreQueryCondition();

                        condition.BatchNo = goodsInfo.BatchNo;
                        condition.GoodsID = goodsInfo.GoodsID;
                        condition.StorageID = storageID;

                        S_Stock stockInfo = storeService.GetStockInfo(condition);

                        if (stockInfo == null)
                        {
                            throw new Exception(string.Format("【物品ID】：{0} 【批次号】：{1} 【库房】：{2} 未找到匹配的库存记录", 
                                goodsInfo.GoodsID, goodsInfo.BatchNo, storage));
                        }

                        goodsInfo.ProviderCode = stockInfo.Provider;
                        goodsInfo.BasicCount = 0;
                        goodsInfo.Remark = "由Excel导入";
                        goodsInfo.RepairStatus = null;
                        goodsInfo.RealCount = goodsInfo.RequestCount;
                        goodsInfo.ShowPosition = 1;

                        ctx.S_MaterialRequisitionGoods.InsertOnSubmit(goodsInfo);
                        ctx.SubmitChanges();
                    }
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
