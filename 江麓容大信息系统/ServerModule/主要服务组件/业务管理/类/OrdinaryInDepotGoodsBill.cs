using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformManagement;
using System.Data;

namespace ServerModule
{
    /// <summary>
    /// 普通入库单物品清单服务类
    /// </summary>
    class OrdinaryInDepotGoodsBill : BasicServer, ServerModule.IOrdinaryInDepotGoodsBill
    {
        /// <summary>
        /// 物品保质期监控服务组件
        /// </summary>
        IGoodsShelfLife m_serverGoodsShelfLife = ServerModuleFactory.GetServerModule<IGoodsShelfLife>();

        /// <summary>
        /// 人员服务组件
        /// </summary>
        IPersonnelInfoServer m_serverPersonnelInfo = ServerModuleFactory.GetServerModule<IPersonnelInfoServer>();

        /// <summary>
        /// 计划价格服务
        /// </summary>
        IBasicGoodsServer m_serverBasicGoods = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();

        /// <summary>
        /// 检查是否存在某单据物品信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(string billNo)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.S_OrdinaryInDepotGoodsBill 
                         where r.Bill_ID == billNo 
                         select r;

            if (result.Count() > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 检查普通入库单中是否存在此物品相关信息
        /// </summary>
        /// <param name="id">物品ID</param>
        /// <returns>存在返回true, 不存在返回false</returns>
        public bool IsExist(int id)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            return (from r in dataContxt.S_OrdinaryInDepotGoodsBill
                    where r.GoodsID == id
                    select r).Count() > 0;
        }

        /// <summary>
        /// 获取指定单据物品信息
        /// </summary>
        /// <param name="billNo">单据编号</param>
        /// <returns>返回获取到的物品信息</returns>
        public IQueryable<View_S_OrdinaryInDepotGoodsBill> GetGoodsViewInfo(string billNo)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            return from r in dataContxt.View_S_OrdinaryInDepotGoodsBill
                   where r.入库单号 == billNo
                   select r;
        }

        /// <summary>
        /// 获取指定单据物品信息
        /// </summary>
        /// <param name="billNo">单据编号</param>
        /// <returns>返回获取到的物品信息</returns>
        public IQueryable<S_OrdinaryInDepotGoodsBill> GetGoodsInfo(string billNo)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            return from r in dataContxt.S_OrdinaryInDepotGoodsBill
                   where r.Bill_ID == billNo
                   select r;
        }

        /// <summary>
        /// 获取包含指定物品编号的信息
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回获取到的物品信息</returns>
        public IQueryable<S_OrdinaryInDepotGoodsBill> GetGoodsViewInfo(int goodsID)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            return from r in dataContxt.S_OrdinaryInDepotGoodsBill
                   where r.GoodsID == goodsID
                   select r;
        }

        /// <summary>
        /// 添加普通入库单物品
        /// </summary>
        /// <param name="billNo">单据编号</param>
        /// <param name="goods">物品信息</param>
        /// <param name="returnInfo">操作完毕后查询数据库的返回结果</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool AddGoods(string billNo, S_OrdinaryInDepotGoodsBill goods, 
            out IQueryable<View_S_OrdinaryInDepotGoodsBill> returnInfo, out string error)
        {
            returnInfo = null;
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                ClearNullValue(goods);

                dataContxt.S_OrdinaryInDepotGoodsBill.InsertOnSubmit(goods);
                dataContxt.SubmitChanges();

                returnInfo = GetGoodsViewInfo(billNo);
                return true;
            }
            catch (Exception err)
            {
                error = err.Message;
                return false;
            }
        }

        /// <summary>
        /// 获得普通入库的新批次号
        /// </summary>
        /// <returns></returns>
        public string GetNewBatchNo()
        {
            string strSql = "select substring( Max(BatchNo),9,6) FROM S_OrdinaryInDepotGoodsBill "+
                " WHERE (BatchNo LIKE '%PR%') and substring(BatchNo,3,4) = '" + ServerTime.Time.Year.ToString() + "' ";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dt.Rows.Count == 0 || dt.Rows[0][0].ToString() == "")
            {
                return string.Format("{0}{1:D4}{2:D2}{3:D6}", "PR", ServerTime.Time.Year,
                    ServerTime.Time.Month, 1);
            }
            else
            {
                return string.Format("{0}{1:D4}{2:D2}{3:D6}", "PR", ServerTime.Time.Year,
                    ServerTime.Time.Month, Convert.ToInt32(dt.Rows[0][0]) + 1);
            }
        }

        /// <summary>
        /// 赋值库存信息
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="bill">单据信息</param>
        /// <param name="item">明细信息</param>
        /// <returns>返回库存信息</returns>
        public S_Stock AssignStockInfo(DepotManagementDataContext dataContext, S_OrdinaryInDepotBill bill, 
            S_OrdinaryInDepotGoodsBill item)
        {
            S_Stock lnqStock = new S_Stock();

            lnqStock.GoodsID = item.GoodsID;

            lnqStock.ProviderBatchNo = item.ProviderBatchNo;
            lnqStock.BatchNo = item.BatchNo;
            lnqStock.Provider = bill.Provider;

            lnqStock.StorageID = bill.StorageID;
            lnqStock.ShelfArea = item.ShelfArea;
            lnqStock.ColumnNumber = item.ColumnNumber;
            lnqStock.LayerNumber = item.LayerNumber;

            lnqStock.ExistCount = (decimal)item.Amount;

            lnqStock.UnitPrice = item.UnitPrice;
            lnqStock.Price = 0;

            return lnqStock;
        }

        /// <summary>
        /// 赋值账务信息
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="bill">单据信息</param>
        /// <param name="item">明细信息</param>
        /// <returns>返回账务信息</returns>
        public S_InDepotDetailBill AssignDetailInfo(DepotManagementDataContext dataContext, S_OrdinaryInDepotBill bill, 
            S_OrdinaryInDepotGoodsBill item)
        {
            IPersonnelInfoServer personnelServer = ServerModuleFactory.GetServerModule<IPersonnelInfoServer>();

            S_InDepotDetailBill detailBill = new S_InDepotDetailBill();
            View_HR_Personnel personnel = UniversalFunction.GetPersonnelInfo(dataContext, bill.BuyerCode);
            View_F_GoodsPlanCost basicGoodsInfo = UniversalFunction.GetGoodsInfo(dataContext, item.GoodsID);

            detailBill.ID = Guid.NewGuid();
            detailBill.BillTime = (DateTime)bill.InDepotDate;
            detailBill.FillInPersonnel = personnel.姓名;
            detailBill.Department = personnel.部门名称;
            detailBill.FactPrice = Math.Round((item.UnitPrice * (decimal)item.Amount), 2);
            detailBill.FactUnitPrice = item.UnitPrice;
            detailBill.GoodsID = item.GoodsID;
            detailBill.BatchNo = item.BatchNo;
            detailBill.InDepotBillID = bill.Bill_ID;
            detailBill.InDepotCount = item.Amount;
            detailBill.Price = Math.Round((item.UnitPrice * (decimal)item.Amount), 2);
            detailBill.UnitPrice = item.UnitPrice;
            detailBill.OperationType = (int)GlobalObject.CE_SubsidiaryOperationType.普通入库;
            detailBill.Provider = bill.Provider;
            detailBill.StorageID = bill.StorageID;
            detailBill.AffrimPersonnel = UniversalFunction.GetPersonnelInfo( bill.DepotManager).姓名;
            detailBill.FillInDate = bill.Bill_Time;

            return detailBill;
        }

        /// <summary>
        /// 清除NULL值
        /// </summary>
        /// <param name="goods">物品信息</param>
        private void ClearNullValue(S_OrdinaryInDepotGoodsBill goods)
        {
            if (goods.BatchNo == null)
            {
                goods.BatchNo = "";
            }

            if (goods.ShelfArea == null)
            {
                goods.ShelfArea = "";
            }

            if (goods.ColumnNumber == null)
            {
                goods.ColumnNumber = "";
            }

            if (goods.LayerNumber == null)
            {
                goods.LayerNumber = "";
            }
        }

        /// <summary>
        /// 更新普通入库单物品
        /// </summary>
        /// <param name="goods">物品信息</param>
        /// <param name="returnInfo">操作完毕后查询数据库的返回结果</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool UpdateGoods(S_OrdinaryInDepotGoodsBill goods,
            out IQueryable<View_S_OrdinaryInDepotGoodsBill> returnInfo, out string error)
        {
            returnInfo = null;
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from r in dataContxt.S_OrdinaryInDepotGoodsBill
                             where r.ID == goods.ID
                             select r;

                ClearNullValue(goods);

                S_OrdinaryInDepotGoodsBill updateObject = result.Single();

                updateObject.GoodsID = goods.GoodsID;
                updateObject.Amount = goods.Amount;
                updateObject.UnitPrice = goods.UnitPrice;
                updateObject.Price = goods.Price;
                updateObject.AmountInWords = goods.AmountInWords;
                updateObject.ProviderBatchNo = goods.ProviderBatchNo;
                updateObject.BatchNo = goods.BatchNo;
                updateObject.ShelfArea = goods.ShelfArea;
                updateObject.ColumnNumber = goods.ColumnNumber;
                updateObject.LayerNumber = goods.LayerNumber;
                updateObject.Remark = goods.Remark;
                updateObject.TestingSingle = goods.TestingSingle;

                dataContxt.SubmitChanges();

                returnInfo = GetGoodsViewInfo(updateObject.Bill_ID);
                return true;
            }
            catch (Exception err)
            {
                error = err.Message;
                return false;
            }
        }

        /// <summary>
        /// 批量删除普通入库单物品
        /// </summary>
        /// <param name="lstID">物品ID列表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool DeleteGoods(List<int> lstID, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                if (lstID == null || lstID.Count == 0)
                {
                    return true;
                }

                var result = from r in dataContxt.S_OrdinaryInDepotGoodsBill
                             where lstID.Contains(r.ID)
                             select r;

                dataContxt.S_OrdinaryInDepotGoodsBill.DeleteAllOnSubmit(result);
                dataContxt.SubmitChanges();

                return true;
            }
            catch (Exception err)
            {
                error = err.Message;
                return false;
            }
        }

        /// <summary>
        /// 删除指定单据的所有物品信息
        /// </summary>
        /// <param name="billNo">要删除的物品单据号</param>
        /// <param name="returnInfo">操作完毕后查询数据库的返回结果</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功的标志</returns>
        public bool DeleteGoods(string billNo, out IQueryable<View_S_OrdinaryInDepotGoodsBill> returnInfo, out string error)
        {
            error = null;
            returnInfo = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                var result = from r in dataContxt.S_OrdinaryInDepotGoodsBill
                             where r.Bill_ID == billNo
                             select r;

                if (result.Count() > 0)
                {
                    dataContxt.S_OrdinaryInDepotGoodsBill.DeleteAllOnSubmit(result);
                    dataContxt.SubmitChanges();
                }

                returnInfo = GetGoodsViewInfo(billNo);
                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 获得合计金额
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <returns>获得合计的金额</returns>
        public string GetSumJE(string billID)
        {
            string strSql = "select Sum(Price) as Price from S_OrdinaryInDepotGoodsBill where Bill_ID = '" + billID + "'";
            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dt.Rows[0][0].ToString();
        }

        public bool IsExistFrock(string billID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.S_OrdinaryInDepotGoodsBill
                          join b in ctx.View_F_GoodsPlanCost
                          on a.GoodsID equals b.序号
                          where a.Bill_ID == billID
                          && b.物品类别名称.Contains("工装")
                          select a;

            if (varData.Count() > 0)
            {
                return true;
            }

            return false;
        }
    }
}
