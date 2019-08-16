/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  FrockIndepotBill.cs
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
using System.Linq;
using System.Text;
using PlatformManagement;
using System.Data;
using System.Data.Linq;
using GlobalObject;

namespace ServerModule
{
    /// <summary>
    /// 自制件工装报检管理类
    /// </summary>
    class FrockIndepotBill : BasicServer, ServerModule.IFrockIndepotBill
    {
        /// <summary>
        /// 工装信息服务组件
        /// </summary>
        IFrockStandingBook m_serverFrockStandingBook = PMS_ServerFactory.GetServerModule<IFrockStandingBook>();

        /// <summary>
        /// BOM表信息服务组件
        /// </summary>
        IBomServer m_serverBom = ServerModuleFactory.GetServerModule<IBomServer>();

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 单据编号分配服务
        /// </summary>
        IAssignBillNoServer m_assignBill = ServerModuleFactory.GetServerModule<IAssignBillNoServer>();

        /// <summary>
        /// 基础物品服务组件
        /// </summary>
        IBasicGoodsServer m_serverbasicGoods = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();

        /// <summary>
        /// 单据唯一码
        /// </summary>
        int m_billUniqueID = -1;

        /// <summary>
        /// 检查某业务是否存在
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">业务号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.S_FrockInDepotGoodsBill
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
        /// 检查某单据是否存在
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(string billNo)
        {
            string sql = "SELECT * FROM [DepotManagement].[dbo].[S_FrockInDepotGoodsBill] where Bill_ID = '" + billNo + "'";

            System.Data.DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 获取自制件工装报检信息
        /// </summary>
        /// <param name="returnInfo">自制件工装报检单</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool GetAllBill(out IQueryResult returnInfo, out string error)
        {
            returnInfo = null;
            error = null;

            IAuthorization serverAuthorization = PlatformFactory.GetObject<IAuthorization>();
            IQueryResult qr = null;

            if (QueryResultFilter == null)
            {
                qr = serverAuthorization.Query("自制件工装报检查询", null);
            }
            else
            {
                qr = serverAuthorization.Query("自制件工装报检查询", null, QueryResultFilter);
            }

            if (!qr.Succeeded)
            {
                error = qr.Error;
                return false;
            }

            if (m_billUniqueID < 0)
            {
                IBillTypeServer server = ServerModuleFactory.GetServerModule<IBillTypeServer>();
                BASE_BillType lnqBillType = server.GetBillTypeFromName("自制件工装报检");

                if (lnqBillType == null)
                {
                    error = "获取不到单据类型信息";
                    return false;
                }

                m_billUniqueID = lnqBillType.UniqueID;
            }

            returnInfo = qr;
            return true;
        }

        /// <summary>
        /// 获取自制件工装报检信息
        /// </summary>
        /// <param name="billNo">单据编号</param>
        /// <returns>成功返回获取到的单据信息,失败返回null</returns>
        public S_FrockInDepotBill GetBill(string billNo)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.S_FrockInDepotBill
                         where r.Bill_ID == billNo
                         select r;

            if (result.Count() == 0)
            {
                return null;
            }

            return result.Single();
        }

        /// <summary>
        /// 添加自制件工装
        /// </summary>
        /// <param name="bill">单据信息</param>
        /// <param name="returnInfo">添加完毕后查询数据库的返回结果</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作结果</returns>
        public bool AddBill(S_FrockInDepotBill bill, out IQueryResult returnInfo, out string error)
        {
            returnInfo = null;
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                dataContxt.S_FrockInDepotBill.InsertOnSubmit(bill);
                dataContxt.SubmitChanges();

                return GetAllBill(out returnInfo, out error);
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 添加自制件工装报检物品
        /// </summary>
        /// <param name="billNo">单据编号</param>
        /// <param name="goods">物品信息</param>
        /// <param name="returnInfo">操作完毕后查询数据库的返回结果</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool AddGoods(string billNo, S_FrockInDepotGoodsBill goods,
            out IQueryable<View_S_FrockInDepotGoodsBill> returnInfo, out string error)
        {
            returnInfo = null;
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                ClearNullValue(goods);

                dataContxt.S_FrockInDepotGoodsBill.InsertOnSubmit(goods);
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
        /// 插入工装所有信息
        /// </summary>
        /// <param name="frockBill">自制件工装信息</param>
        /// <param name="flag">是否进入工装台帐</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool CreateNewFrockMessage(S_FrockInDepotBill frockBill, bool flag, out string error)
        {
            error = null;

            FrockProvingReport serverFrockProvingReport = new FrockProvingReport();

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.S_FrockInDepotGoodsBill
                              where a.Bill_ID == frockBill.Bill_ID
                              select a;

                foreach (var item in varData)
                {
                    for (int i = 0; i < item.Amount; i++)
                    {

                        string strFrockNumber = GetNewFrockNumber();

                        S_FrockProvingReport lnqReport = new S_FrockProvingReport();

                        lnqReport.DJH = m_assignBill.AssignNewNo(serverFrockProvingReport, "工装验证报告单");
                        lnqReport.DJZT = "等待检验要求";
                        lnqReport.BillType = "入库检验";
                        lnqReport.BZRQ = ServerTime.Time;
                        lnqReport.ConnectBillNumber = frockBill.Bill_ID;
                        lnqReport.FrockNumber = strFrockNumber;
                        lnqReport.GoodsID = item.GoodsID;

                        if (!serverFrockProvingReport.AddBill(lnqReport, null, out error))
                        {
                            return false;
                        }

                        m_billMessageServer.SendNewFlowMessage(lnqReport.DJH,
                            string.Format("{0}  ※※※ 请【{1}】处理",
                            UniversalFunction.GetGoodsMessage(lnqReport.GoodsID), CE_RoleEnum.工艺人员.ToString()),
                            CE_RoleEnum.工艺人员);

                        if (flag)
                        {
                            S_FrockStandingBook lnqBook = new S_FrockStandingBook();

                            lnqBook.GoodsID = item.GoodsID;
                            lnqBook.FrockNumber = strFrockNumber;
                            lnqBook.Designer = UniversalFunction.GetPersonnelName(frockBill.DesignerID);

                            if (!m_serverFrockStandingBook.UpdateFrockStandingBook(lnqBook, null, out error))
                            {
                                return false;
                            }
                        }
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
        /// 获得新的工装编号
        /// </summary>
        /// <returns>返回工装编号</returns>
        public string GetNewFrockNumber()
        {
            string strSql = "select Max(FrockNumber) from S_FrockProvingReport where Len(FrockNumber) = 9";

            string strOutNumber = ServerTime.Time.Year.ToString() + ServerTime.Time.Month.ToString("D2") + "001";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dtTemp.Rows[0][0].ToString() != "")
            {

                if (dtTemp.Rows[0][0].ToString().Substring(0, 4) == ServerTime.Time.Year.ToString())
                {

                    if (dtTemp.Rows[0][0].ToString().Substring(4, 2) == ServerTime.Time.Month.ToString("D2"))
                    {
                        strOutNumber = ServerTime.Time.Year.ToString()
                            + ServerTime.Time.Month.ToString("D2")
                            + (Convert.ToInt32(dtTemp.Rows[0][0].ToString().Substring(6, 3)) + 1).ToString("D3");
                    }
                }
            }

            return strOutNumber;
        }

        /// <summary>
        /// 机加人员提交单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="flag">操作标志</param>
        /// <param name="returnInfo">返回更新后重新查询的数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功</returns>
        public bool SubmitNewBill(string billNo, bool flag, out IQueryResult returnInfo, out string error)
        {
            returnInfo = null;
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from r in dataContxt.S_FrockInDepotBill
                             where r.Bill_ID == billNo
                             select r;

                if (result.Count() == 0)
                {
                    error = string.Format("没有找到单据号为 {0} 的自制件工装报检信息，无法进行此操作", billNo);
                    return false;
                }

                result.Single().Bill_Status = "等待工装验证";
                result.Single().Bill_Time = ServerModule.ServerTime.Time;


                if (!m_serverFrockStandingBook.DeleteFrockOrdinaryInDepotBill(dataContxt, billNo, out error))
                {
                    return false;
                }

                //插入工装信息
                if (!CreateNewFrockMessage(result.Single(), flag, out error))
                {
                    return false;
                }

                dataContxt.SubmitChanges();

                return GetAllBill(out returnInfo, out error);
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 提交入库信息
        /// </summary>
        /// <param name="inDepotInfo">入库信息, 只取其中入库部分</param>
        /// <param name="returnInfo">添加完毕后查询数据库的返回结果</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作结果</returns>
        public bool SubmitInDepotInfo(S_FrockInDepotBill inDepotInfo, out IQueryResult returnInfo, out string error)
        {
            returnInfo = null;
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                var result = from r in dataContxt.S_FrockInDepotBill
                             where r.Bill_ID == inDepotInfo.Bill_ID
                             select r;

                if (result.Count() == 0)
                {
                    error = string.Format("找不到入库单号为 [{0}] 的自制件工装报检信息！", inDepotInfo.Bill_ID);
                    return false;
                }

                S_FrockInDepotBill lnqBill = result.Single();

                if (lnqBill.Bill_Status.Equals("已入库"))
                {
                    error = string.Format("单据号为 [{0}] 单据状态为已入库", inDepotInfo.Bill_ID);
                    return false;
                }

                lnqBill.DepotManager = inDepotInfo.DepotManager;
                lnqBill.Bill_Status = inDepotInfo.Bill_Status;
                lnqBill.InDepotDate = ServerTime.Time;

                //操作账务信息与库存信息
                OpertaionDetailAndStock(dataContxt, lnqBill);

                //更新工装库存状态
                if (!UpdateFrockStockStatus(dataContxt, lnqBill.Bill_ID, out error))
                {
                    return false;
                }

                // 正式使用单据号
                m_assignBill.UseBillNo(dataContxt, CE_BillTypeEnum.自制件工装报检.ToString(), lnqBill.Bill_ID);

                dataContxt.SubmitChanges();

                return GetAllBill(out returnInfo, out error);
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 根据单据信息操作账务信息与库存信息
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="bill">单据信息</param>
        public void OpertaionDetailAndStock(DepotManagementDataContext dataContext, S_FrockInDepotBill bill)
        {
            MaterialRequisitionGoodsServer goodsService = new MaterialRequisitionGoodsServer();
            IFinancialDetailManagement serverDetail =
                ServerModule.ServerModuleFactory.GetServerModule<IFinancialDetailManagement>();

            var result = from r in dataContext.S_FrockInDepotGoodsBill
                         where r.Bill_ID == bill.Bill_ID
                         select r;

            if (result == null || result.Count() == 0)
            {
                throw new Exception("获取单据信息失败");
            }

            foreach (var item in result)
            {
                S_InDepotDetailBill detailInfo = AssignDetailInfo(dataContext, bill, item);
                S_Stock stockInfo = AssignStockInfo(dataContext, bill, item);

                if (detailInfo == null || stockInfo == null)
                {
                    throw new Exception("获取账务信息或者库存信息失败");
                }

                serverDetail.ProcessInDepotDetail(dataContext, detailInfo, stockInfo);
            }
        }
        /// <summary>
        /// 根据工装验证报告表更新工装台帐的库存状态
        /// </summary>
        /// <param name="ctx">LINQ</param>
        /// <param name="billno">普通入库单号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool UpdateFrockStockStatus(DepotManagementDataContext ctx, string billno, out string error)
        {
            error = null;
            try
            {
                string strSql = "select * from S_FrockInDepotGoodsBill where Bill_ID = '" + billno + "'";

                DataTable dtOrdinary = GlobalObject.DatabaseServer.QueryInfo(strSql);

                for (int i = 0; i < dtOrdinary.Rows.Count; i++)
                {
                    strSql = "select Count(*) from S_FrockProvingReport where ConnectBillNumber = '"
                        + billno + "' and IsInStock = 1  and GoodsID = " + Convert.ToInt32(dtOrdinary.Rows[i]["GoodsID"]);

                    DataTable dtFrock = GlobalObject.DatabaseServer.QueryInfo(strSql);

                    if (Convert.ToDecimal(dtOrdinary.Rows[i]["Amount"]) != Convert.ToDecimal(dtFrock.Rows[0][0]))
                    {
                        error = "入库数量与工装检验报告单所需入库数量不符";
                        return false;
                    }
                }

                strSql = "select * from S_FrockProvingReport where ConnectBillNumber = '" + billno + "' and IsInStock = 1 ";

                DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

                if (!m_serverFrockStandingBook.UpdateFrockStandingBookStock(ctx, dt, true, out error))
                {
                    return false;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    S_FrockOperation lnqOperation = new S_FrockOperation();

                    lnqOperation.BillID = billno;
                    lnqOperation.BillTime = ServerTime.Time;
                    lnqOperation.BillType = "入库";
                    lnqOperation.FrockNumber = dt.Rows[i]["FrockNumber"].ToString();
                    lnqOperation.GoodsID = Convert.ToInt32(dt.Rows[i]["GoodsID"]);
                    lnqOperation.IsTrue = true;

                    ctx.S_FrockOperation.InsertOnSubmit(lnqOperation);
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
        /// 赋值库存信息
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="bill">单据信息</param>
        /// <param name="item">明细信息</param>
        /// <returns>返回库存信息对象</returns>
        S_Stock AssignStockInfo(DepotManagementDataContext ctx, S_FrockInDepotBill bill, S_FrockInDepotGoodsBill item)
        {
            S_Stock stock = new S_Stock();

            stock.GoodsID = item.GoodsID;

            stock.Provider = CE_WorkShopCode.JJCJ.ToString();

            stock.BatchNo = item.BatchNo;
            stock.ProviderBatchNo = "";

            stock.StorageID = bill.StorageID;
            stock.ShelfArea = item.ShelfArea;
            stock.ColumnNumber = item.ColumnNumber;
            stock.LayerNumber = item.LayerNumber;

            stock.ExistCount = (decimal)item.Amount;

            stock.UnitPrice = 0;
            stock.Price = 0;

            return stock;
        }

        /// <summary>
        /// 赋值账务信息
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="bill">单据信息</param>
        /// <param name="item">明细信息</param>
        /// <returns>返回账务信息</returns>
        S_InDepotDetailBill AssignDetailInfo(DepotManagementDataContext ctx, S_FrockInDepotBill bill, S_FrockInDepotGoodsBill item)
        {

            IPersonnelInfoServer serverPersonnelInfo = ServerModuleFactory.GetServerModule<IPersonnelInfoServer>();
            View_HR_Personnel lnqPersonnel = UniversalFunction.GetPersonnelInfo(ctx, bill.JJRYID);
            S_InDepotDetailBill lnqDetailBill = new S_InDepotDetailBill();

            View_F_GoodsPlanCost lnqBasicGoodsInfo = UniversalFunction.GetGoodsInfo(ctx, item.GoodsID);

            lnqDetailBill.ID = Guid.NewGuid();
            lnqDetailBill.BillTime = (DateTime)bill.InDepotDate;
            lnqDetailBill.FillInPersonnel = lnqPersonnel.姓名;
            lnqDetailBill.Department = lnqPersonnel.部门名称;
            lnqDetailBill.FactPrice = 0;
            lnqDetailBill.FactUnitPrice = 0;
            lnqDetailBill.GoodsID = item.GoodsID;
            lnqDetailBill.BatchNo = item.BatchNo;
            lnqDetailBill.InDepotBillID = bill.Bill_ID;
            lnqDetailBill.InDepotCount = item.Amount;
            lnqDetailBill.Price = 0;
            lnqDetailBill.UnitPrice = 0;
            lnqDetailBill.OperationType = (int)CE_SubsidiaryOperationType.自制件工装入库;
            lnqDetailBill.Provider = bill.Provider;
            lnqDetailBill.StorageID = bill.StorageID;
            lnqDetailBill.AffrimPersonnel = bill.DepotManager;
            lnqDetailBill.FillInDate = bill.Bill_Time;

            return lnqDetailBill;
        }

        /// <summary>
        /// 清除NULL值
        /// </summary>
        /// <param name="goods">物品信息</param>
        private void ClearNullValue(S_FrockInDepotGoodsBill goods)
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
        public bool UpdateGoods(S_FrockInDepotGoodsBill goods, out IQueryable<View_S_FrockInDepotGoodsBill> returnInfo, out string error)
        {
            returnInfo = null;
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                var result = from r in dataContxt.S_FrockInDepotGoodsBill
                             where r.ID == goods.ID
                             select r;

                ClearNullValue(goods);

                S_FrockInDepotGoodsBill updateObject = result.Single();

                updateObject.GoodsID = goods.GoodsID;
                updateObject.Amount = goods.Amount;
                updateObject.BatchNo = goods.BatchNo;
                updateObject.ShelfArea = goods.ShelfArea;
                updateObject.ColumnNumber = goods.ColumnNumber;
                updateObject.LayerNumber = goods.LayerNumber;
                updateObject.Remark = goods.Remark;

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
        /// 获取指定单据物品信息
        /// </summary>
        /// <param name="billNo">单据编号</param>
        /// <returns>返回获取到的物品信息</returns>
        public IQueryable<View_S_FrockInDepotGoodsBill> GetGoodsViewInfo(string billNo)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            return from r in dataContxt.View_S_FrockInDepotGoodsBill
                   where r.单据号 == billNo
                   select r;
        }

        /// <summary>
        /// 获取指定单据物品信息
        /// </summary>
        /// <param name="billNo">单据编号</param>
        /// <returns>返回获取到的物品信息</returns>
        public IQueryable<View_S_FrockInDepotGoodsBill> GetGoodsInfo(string billNo)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            return from r in dataContxt.View_S_FrockInDepotGoodsBill
                   where r.单据号 == billNo
                   select r;
        }

        /// <summary>
        /// 获取包含指定物品编号的信息
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回获取到的物品信息</returns>
        public IQueryable<S_FrockInDepotGoodsBill> GetGoodsViewInfo(int goodsID)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            return from r in dataContxt.S_FrockInDepotGoodsBill
                   where r.GoodsID == goodsID
                   select r;
        }

        /// <summary>
        /// 批量删除自制件工装报检物品
        /// </summary>
        /// <param name="lstID">物品ID列表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool DeleteGoods(List<int> lstID, out string error)
        {
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            if (lstID == null || lstID.Count == 0)
            {
                return true;
            }

            try
            {
                var result = from r in dataContxt.S_FrockInDepotGoodsBill
                             where lstID.Contains(r.ID)
                             select r;

                dataContxt.S_FrockInDepotGoodsBill.DeleteAllOnSubmit(result);
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
        public bool DeleteGoods(string billNo, out IQueryable<View_S_FrockInDepotGoodsBill> returnInfo, out string error)
        {
            error = null;
            returnInfo = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                var result = from r in dataContxt.S_FrockInDepotGoodsBill
                             where r.Bill_ID == billNo
                             select r;

                if (result.Count() > 0)
                {
                    dataContxt.S_FrockInDepotGoodsBill.DeleteAllOnSubmit(result);
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
        /// 删除自制件工装报检
        /// </summary>
        /// <param name="billNo">单据号号</param>
        /// <param name="returnInfo">自制件工装报检</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功删除自制件工装报检</returns>
        public bool DeleteBill(string billNo, out IQueryResult returnInfo, out string error)
        {
            returnInfo = null;

            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                Table<S_FrockInDepotBill> table = dataContxt.GetTable<S_FrockInDepotBill>();

                var delRow = from c in table
                             where c.Bill_ID == billNo
                             select c;

                table.DeleteAllOnSubmit(delRow);

                if (!m_serverFrockStandingBook.DeleteFrockOrdinaryInDepotBill(dataContxt, billNo, out error))
                {
                    return false;
                }

                var result = from r in dataContxt.S_FrockInDepotGoodsBill
                             where r.Bill_ID == billNo
                             select r;

                if (result.Count() > 0)
                {
                    dataContxt.S_FrockInDepotGoodsBill.DeleteAllOnSubmit(result);
                }

                dataContxt.SubmitChanges();

                return GetAllBill(out returnInfo, out error);
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }
    }
}
