using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformManagement;
using System.Data.Linq;
using System.Data;
using ServerModule;
using GlobalObject;

namespace ServerModule
{
    /// <summary>
    /// 报废单服务
    /// </summary>
    class ScrapBillServer :BasicServer, IScrapBillServer
    {
        /// <summary>
        /// 产品箱号服务组件
        /// </summary>
        IProductCodeServer m_serverProductCode = ServerModuleFactory.GetServerModule<IProductCodeServer>();

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 单据编号分配服务
        /// </summary>
        IAssignBillNoServer m_assignBill = ServerModuleFactory.GetServerModule<IAssignBillNoServer>();

        /// <summary>
        /// 检查某业务是否存在
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">业务号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.S_ScrapBill
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
            string sql = "SELECT * FROM [DepotManagement].[dbo].[S_ScrapBill] where bill_id = '" + billNo + "'";

            System.Data.DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 获取指定时间范围内的所有报废单信息
        /// </summary>
        /// <param name="start">开始时间</param>
        /// <param name="end">终止时间</param>
        /// <returns>返回所有的报废单</returns>
        public DataTable GetScrapBill(DateTime start, DateTime end)
        {
            string sql = "SELECT 报废单号,报废时间,损失金额,单据报废类型,申请人签名,申请部门 FROM [DepotManagement].[dbo].[view_S_ScrapBill]";

            if (start != null)
            {
                sql += " where (报废时间 between '" + start + "' and '" + end + "')";
            }

            System.Data.DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 获取单据信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>成功返回获取到的单据信息,失败返回null</returns>
        public S_ScrapBill GetBill(string billNo)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.S_ScrapBill 
                         where r.Bill_ID == billNo 
                         select r;

            if (result.Count() > 0)
            {
                return result.Single();
            }

            return null;
        }

        /// <summary>
        /// 获取单据视图信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>成功返回获取到的单据信息,失败返回null</returns>
        public View_S_ScrapBill GetBillView(string billNo)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.View_S_ScrapBill where r.报废单号 == billNo select r;

            if (result.Count() > 0)
            {
                return result.Single();
            }

            return null;
        }

        /// <summary>
        /// 获取所有日期范围内单据信息
        /// </summary>
        /// <param name="startTime">起始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="returnBill">查询到的单据信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取报废信息</returns>
        public bool GetAllBill(DateTime startTime, DateTime endTime, out IQueryResult returnBill, out string error)
        {
            returnBill = null;
            error = null;

            IAuthorization authorization = PlatformFactory.GetObject<IAuthorization>();
            IQueryResult qr = null;

            string sql = string.Format(" (报废时间 between '{0}' and '{1}')", startTime.ToShortDateString(), endTime.ToShortDateString());

            if (QueryResultFilter == null)
            {
                qr = authorization.Query("报废单查询", null, sql);
            }
            else
            {
                qr = authorization.Query("报废单查询", null, QueryResultFilter + " and " + sql);
            }

            if (!qr.Succeeded)
            {
                error = qr.Error;
                return false;
            }

            returnBill = qr;
            return true;
        }

        /// <summary>
        /// 获取所有单据信息
        /// </summary>
        /// <param name="returnBill">查询到的单据信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取报废信息</returns>
        public bool GetAllBill(out IQueryResult returnBill, out string error)
        {
            returnBill = null;
            error = null;

            IAuthorization authorization = PlatformFactory.GetObject<IAuthorization>();
            IQueryResult qr = null;

            if (QueryResultFilter == null)
            {
                qr = authorization.Query("报废单查询", null);
            }
            else
            {
                qr = authorization.Query("报废单查询", null, QueryResultFilter);
            }

            if (!qr.Succeeded)
            {
                error = qr.Error;
                return false;
            }

            returnBill = qr;

            return true;
        }

        /// <summary>
        /// 获取所有已经完成并且需要冲抵领料单的单据信息
        /// </summary>
        /// <param name="returnBill">查询到的单据信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取报废信息</returns>
        public bool GetAllBillForFetchGoods(out IQueryResult returnBill, out string error)
        {
            if (!GetAllBill(out returnBill, out error))
            {
                return false;
            }

            System.Data.DataTable data = returnBill.DataCollection.Tables[0];

            for (int i = 0; i < data.Rows.Count; i++)
            {
                if (data.Rows[i]["单据状态"].ToString() == ScrapBillStatus.新建单据.ToString())
                {
                    data.Rows.RemoveAt(i);
                    i--;
                }
            }

            return true;
        }

        /// <summary>
        /// 添加单据
        /// </summary>
        /// <param name="dataContxt">数据上下文</param>
        /// <param name="bill">单据信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool AddBill(DepotManagementDataContext dataContxt,S_ScrapBill bill, out string error)
        {
            error = null;

            dataContxt.S_ScrapBill.InsertOnSubmit(bill);
            
            return true;
        }

        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="flag">操作标志</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool DeleteBill(string billNo, bool flag, out string error)
        {
            try
            {
                /////批量生成领料退库、采购退货单
                //string strSql = "select distinct YearMonth from CJB_TempTable";
                //DataTable tempTable = GlobalObject.DatabaseServer.QueryInfo(strSql);

                //foreach (DataRow dr in tempTable.Rows)
                //{
                //    using (DepotManagementDataContext ctx = CommentParameter.DepotDataContext)
                //    {
                //        ctx.Connection.Open();
                //        ctx.Transaction = ctx.Connection.BeginTransaction();

                //        InsertReturnBill(ctx, dr[0].ToString());
                //        InsertRejectBill(ctx, dr[0].ToString());

                //        ctx.SubmitChanges();
                //        ctx.Transaction.Commit();
                //    }
                //}

                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                bool ret = DeleteBill(dataContxt, billNo, flag, out error);

                //对于营销的总称领料的删除
                var varData = from a in dataContxt.ProductsCodes
                              where a.DJH == billNo
                              select a;

                dataContxt.ProductsCodes.DeleteAllOnSubmit(varData);

                if (ret)
                {
                    dataContxt.SubmitChanges();
                }

                return ret;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="dataContxt">LINQ 数据库上下文</param>
        /// <param name="billNo">单据号</param>
        /// <param name="flag">【此参数已作废】是否设置单据为报废状态（不真正删除单据）</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool DeleteBill(DepotManagementDataContext dataContxt,string billNo, bool flag, out string error)
        {
            try
            {
                error = null;

                var delRow = from c in dataContxt.S_ScrapBill where c.Bill_ID == billNo select c;

                if (delRow.Count() > 0)
                {
                    dataContxt.S_ScrapBill.DeleteAllOnSubmit(delRow);

                    //质管部要求再不修改物品名称与报废数量的情况下 可以修改其他任何数据
                    //IMaterialRequisitionServer mrs = ServerModuleFactory.GetServerModule<IMaterialRequisitionServer>();
                    
                    //if (mrs.IsExistAssociatedBill(billNo))
                    //{
                    //    error = "已经有领料单使用了此报废单号不允许再进行此操作";
                    //    return false;
                    //}

                    //if (blflag)
                    //{
                    //    foreach (var item in delRow)
                    //    {
                    //        item.BillStatus = "已报废";
                    //    }
                    //}
                    //else
                    //{
                    //    //保证单据不断号
                    //    dataContxt.S_ScrapBill.DeleteAllOnSubmit(delRow);
                    //}

                    return true;
                }

                error = string.Format("找不到 {0} 单据, 无法进行此操作", billNo);

                return false;
            }
            catch (Exception err)
            {
                error = err.Message;
                return false;
            }
        }

        /// <summary>
        /// 修改单据
        /// </summary>
        /// <param name="bill">单据信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool UpdateBill(S_ScrapBill bill, out string error)
        {
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                var result = from c in dataContxt.S_ScrapBill 
                             where c.Bill_ID == bill.Bill_ID 
                             select c;

                if (result.Count() > 0)
                {
                    S_ScrapBill updateBill = result.Single();

                    updateBill.NotifyChecker = bill.NotifyChecker;
                    updateBill.Remark = bill.Remark;

                    dataContxt.SubmitChanges();

                    return true;
                }

                error = string.Format("找不到 {0} 单据, 无法进行此操作", bill.Bill_ID);

                return false;
            }
            catch (Exception err)
            {
                error = err.Message;
                return false;
            }
        }

        /// <summary>
        /// 检查领用总成数量与所设置的流水码数量是否一致
        /// </summary>
        /// <param name="ctx">LINQ 数据库上下文</param>
        /// <param name="djh">报废单单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>一致返回true</returns>
        public bool CheckProductCode(DepotManagementDataContext ctx, string djh, out string error)
        {
            try
            {
                error = null;

                var varDataBill = from a in ctx.S_ScrapGoods
                                  where a.Bill_ID == djh
                                  select a;

                foreach (var item in varDataBill)
                {
                    if (Convert.ToBoolean(UniversalFunction.GetGoodsAttributeInfo(item.GoodsID, CE_GoodsAttributeName.CVT))
                        || Convert.ToBoolean(UniversalFunction.GetGoodsAttributeInfo(item.GoodsID, CE_GoodsAttributeName.TCU)))
                    {
                        var varProductCode = from c in ctx.ProductsCodes
                                             where c.GoodsID == item.GoodsID
                                             && c.DJH == djh
                                             select c;

                        if (varProductCode.Count() != Convert.ToInt32(item.Quantity))
                        {
                            error = "领用总成数量与所设置的流水码数量不一致，请双击此记录正确录入编码";
                            return false;
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
        /// 提交单据(交给主管审批)
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool SubmitNewBill(string billNo, out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                //检查总成领用数量是否都设置了流水码
                if (!CheckProductCode(dataContxt, billNo, out error))
                {
                    return false;
                }

                var result = from r in dataContxt.S_ScrapBill where r.Bill_ID == billNo select r;

                if (result.Count() == 0)
                {
                    error = string.Format("没有找到单据号为 {0} 的报废单信息，无法进行此操作", billNo);
                    return false;
                }

                result.Single().BillStatus = ScrapBillStatus.等待主管审核.ToString();
                result.Single().Bill_Time = ServerModule.ServerTime.Time;

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
        /// 主管审批单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="name">主管姓名</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool DirectorAuthorizeBill(string billNo, string name, out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from r in dataContxt.S_ScrapBill where r.Bill_ID == billNo select r;

                if (result.Count() == 0)
                {
                    error = string.Format("没有找到单据号为 {0} 的报废单信息，无法进行此操作", billNo);
                    return false;
                }

                result.Single().DepartmentDirector = name;
                result.Single().BillStatus = ScrapBillStatus.等待质检批准.ToString();

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
        /// 提交质量信息
        /// </summary>
        /// <param name="billID">单据编号</param>
        /// <param name="qualityInfo">质量信息, 只取其中质量部分</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool SubmitQualityInfo(string billID, S_ScrapBill qualityInfo, out string error)
        {
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;
            
            try
            {
                var result = from r in dataContxt.S_ScrapBill 
                             where r.Bill_ID == billID 
                             select r;

                if (result.Count() == 0)
                {
                    error = string.Format("找不到单号为 [{0}] 的报废单，不能进行此操作！", billID);
                    return false;
                }

                S_ScrapBill bill = result.Single();

                bill.Checker = qualityInfo.Checker;                                     // 检验员
                bill.Sanction = qualityInfo.Sanction;                                   // 批准人签名
                bill.AuthorizeTime = ServerTime.Time;
                bill.BillStatus = qualityInfo.BillStatus;
                
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
        /// 工具账务操作
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">单据号</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="provider">供应商</param>
        /// <param name="operationCount">操作数量</param>
        /// <param name="workID">操作人员ID</param>
        void ToolsOperation(DepotManagementDataContext ctx, string billNo, int goodsID, string provider,
            decimal operationCount, string workID)
        {
            IToolsManage serverTools = ServerModule.ServerModuleFactory.GetServerModule<IToolsManage>();

            if (serverTools.IsTools(goodsID))
            {
                Service_Manufacture_WorkShop.IWorkShopBasic serverWSBasic =
                    Service_Manufacture_WorkShop.ServerModuleFactory.GetServerModule<Service_Manufacture_WorkShop.IWorkShopBasic>();
                IPersonnelInfoServer serverPersonnel = ServerModuleFactory.GetServerModule<IPersonnelInfoServer>();

                View_HR_Personnel viewPersonnel = serverPersonnel.GetPersonnelInfo(workID);
                WS_WorkShopCode tempWSCode = serverWSBasic.GetPersonnelWorkShop(workID);

                S_MachineAccount_Tools toolsInfo = new S_MachineAccount_Tools();

                toolsInfo.GoodsID = goodsID;
                toolsInfo.Provider = provider;
                toolsInfo.StockCount = -operationCount;

                if (tempWSCode != null)
                {
                    toolsInfo.StorageCode = tempWSCode.WSCode;
                }
                else
                {
                    toolsInfo.StorageCode = viewPersonnel.部门编码;
                }

                serverTools.OpertionInfo(ctx, toolsInfo);
                serverTools.DayToDayAccount(ctx, toolsInfo, billNo);
            }
        }

        /// <summary>
        /// 完成单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="storeManager">仓库管理员</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool FinishBill(string billNo, string storeManager, out string error)
        {

            using (DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext)
            {
                try
                {
                    error = null;
                    dataContxt.Connection.Open();
                    dataContxt.Transaction = dataContxt.Connection.BeginTransaction();

                    var result = from r in dataContxt.S_ScrapBill where r.Bill_ID == billNo select r;

                    if (result.Count() == 0)
                    {
                        error = string.Format("没有找到单据号为 {0} 的报废单信息，无法进行此操作", billNo);
                        return false;
                    }

                    S_ScrapBill bill = result.Single();

                    bill.DepotManager = storeManager;
                    bill.DepotTime = ServerTime.Time;
                    bill.BillStatus = ScrapBillStatus.已完成.ToString();

                    //操作账务信息与库存信息
                    OpertaionDetailAndStock(dataContxt, bill);

                    var goodsData = from a in dataContxt.S_ScrapGoods
                                    where a.Bill_ID == billNo
                                    && a.ResponsibilityDepartment == "GYS"
                                    select a;

                    if (goodsData.Count() > 0)
                    {
                        goodsData = from a in dataContxt.S_ScrapGoods
                                    where a.Bill_ID == billNo
                                    && a.ResponsibilityDepartment != "GYS"
                                    select a;

                        if (goodsData.Count() > 0)
                        {
                            throw new Exception("【责任部门】有误，请回退单据到新建人员进行修改");
                        }

                        InsertReturnBill(dataContxt, billNo);
                        InsertRejectBill(dataContxt, billNo);
                    }

                    // 正式使用单据号
                    m_assignBill.UseBillNo(dataContxt, "报废单", bill.Bill_ID);
                    dataContxt.SubmitChanges();

                    dataContxt.Transaction.Commit();

                    return true;
                }
                catch (Exception err)
                {
                    error = err.Message;
                    dataContxt.Transaction.Rollback();
                    return false;
                }
            }
        }

        /// <summary>
        /// 根据单据信息操作账务信息与库存信息
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="bill">单据信息</param>
        public void OpertaionDetailAndStock(DepotManagementDataContext dataContext, S_ScrapBill bill)
        {
            IFinancialDetailManagement serverDetail =
                ServerModule.ServerModuleFactory.GetServerModule<IFinancialDetailManagement>();

            var result = from r in dataContext.S_ScrapGoods
                         where r.Bill_ID == bill.Bill_ID
                         select r;

            if (result == null || result.Count() == 0)
            {
                throw new Exception("获取单据信息失败");
            }

            foreach (var item in result)
            {
                S_FetchGoodsDetailBill detailInfo = AssignDetailInfo(dataContext, bill, item);

                if (detailInfo == null)
                {
                    throw new Exception("获取账务信息或者库存信息失败");
                }

                serverDetail.ProcessFetchGoodsDetail(dataContext, detailInfo, null);
            }
        }

        /// <summary>
        /// 生成采购退货单
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="billNo">单据号</param>
        void InsertRejectBill(DepotManagementDataContext context, string billNo)
        {
            string error = null;
            IMaterialRejectBill serverRejectBill = ServerModule.ServerModuleFactory.GetServerModule<IMaterialRejectBill>();
            IMaterialListRejectBill serverRejectListBill = ServerModule.ServerModuleFactory.GetServerModule<IMaterialListRejectBill>();

            var varData = from a in context.S_ScrapBill
                          where a.Bill_ID == billNo 
                          select a;

            if (varData.Count() == 0)
            {
                return;
            }

            S_ScrapBill scarpBill = varData.Single();
            List<string> lstProvider = (from a in context.S_ScrapGoods 
                                        where a.Bill_ID == billNo && a.ResponsibilityProvider == a.Provider 
                                        select a.Provider).Distinct().ToList();

            foreach (string provider in lstProvider)
            {
                var dataProviderWork = from a in context.ProviderPrincipal
                                       where a.Provider == provider
                                       && a.IsMainDuty == true
                                       select a;

                View_HR_Personnel personnelInfo = UniversalFunction.GetPersonnelInfo(context, dataProviderWork.First().PrincipalWorkId);
                S_MaterialRejectBill bill = new S_MaterialRejectBill();

                bill.Bill_ID = m_assignBill.AssignNewNo(context, serverRejectBill, CE_BillTypeEnum.采购退货单.ToString());
                bill.Bill_Time = ServerTime.Time;
                bill.BillStatus = MaterialRejectBillBillStatus.已完成.ToString();
                bill.Department = personnelInfo.部门编码;
                bill.FillInPersonnel = personnelInfo.姓名;
                bill.FillInPersonnelCode = personnelInfo.工号;
                bill.DepotManager = BasicInfo.LoginName;
                bill.Provider = provider;
                bill.Reason = "由【报废单】：" + billNo + " 生成的报废退货";
                bill.Remark = "系统自动生成";
                bill.BillType = "总仓库退货单";
                bill.StorageID = "01";
                bill.OutDepotDate = ServerTime.Time;

                context.S_MaterialRejectBill.InsertOnSubmit(bill);
                context.SubmitChanges();

                var varData2 = from a in context.S_ScrapGoods
                               where a.Provider == provider 
                               && a.Bill_ID == billNo
                               && a.ResponsibilityProvider == a.Provider
                               select a;

                foreach (S_ScrapGoods goodsInfo in varData2)
                {
                    string orderForm = GetOrderForm(context, goodsInfo.GoodsID, goodsInfo.BatchNo, provider);

                    if (orderForm == null)
                    {
                        throw new Exception(UniversalFunction.GetGoodsMessage(context, goodsInfo.GoodsID)
                            + " 批次号：【" + goodsInfo.BatchNo + "】  供应商：【" + provider + "】 找不到对应的【订单号】");
                    }

                    QueryCondition_Store queryInfo = new QueryCondition_Store();

                    queryInfo.BatchNo = goodsInfo.BatchNo;
                    queryInfo.GoodsID = goodsInfo.GoodsID;
                    queryInfo.StorageID = "01";

                    S_Stock stockInfo = UniversalFunction.GetStockInfo(context, queryInfo);

                    //插入业务明细信息
                    S_MaterialListRejectBill goods = new S_MaterialListRejectBill();

                    goods.Bill_ID = bill.Bill_ID;
                    goods.GoodsID = goodsInfo.GoodsID;
                    goods.Provider = provider;
                    goods.ProviderBatchNo = stockInfo == null ? "" : stockInfo.ProviderBatchNo;
                    goods.BatchNo = goodsInfo.BatchNo;
                    goods.Amount = (decimal)goodsInfo.Quantity;
                    goods.Remark = "";
                    goods.AssociateID = orderForm;

                    if (!serverRejectListBill.SetPriceInfo(goods.AssociateID, goods, bill.StorageID, out error))
                    {
                        throw new Exception(error);
                    }

                    context.S_MaterialListRejectBill.InsertOnSubmit(goods);
                    context.SubmitChanges();
                }

                serverRejectBill.OpertaionDetailAndStock(context, bill);
                context.SubmitChanges();
            }
        }

        public string GetOrderForm(DepotManagementDataContext ctx , int goodsID, string batchNo, string provider)
        {
            var dataInfo = from a in
                               ((from a in ctx.S_CheckOutInDepotBill
                                 select new { OrderForm = a.OrderFormNumber, BatchNo = a.BatchNo, GoodsID = a.GoodsID, Provider = a.Provider }).Concat
                                   (from a in ctx.S_CheckOutInDepotForOutsourcingBill
                                    select new { OrderForm = a.OrderFormNumber, BatchNo = a.BatchNo, GoodsID = a.GoodsID, Provider = a.Provider }).Concat
                                   (from a in ctx.S_OrdinaryInDepotBill
                                    join b in ctx.S_OrdinaryInDepotGoodsBill
                                    on a.Bill_ID equals b.Bill_ID
                                    select new { OrderForm = a.OrderBill_ID, BatchNo = b.BatchNo, GoodsID = b.GoodsID, Provider = a.Provider }))
                           where a.GoodsID == goodsID && a.BatchNo == batchNo && a.Provider == provider
                           select a;

            if (dataInfo == null || dataInfo.Count() == 0)
            {
                return null;
            }
            else
            {
                return dataInfo.First().OrderForm;
            }
        }

        /// <summary>
        /// 生成领料退库单
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="billNo">单据号</param>
        void InsertReturnBill(DepotManagementDataContext context, string billNo)
        {
            IMaterialReturnedInTheDepot serverReturnedBill = 
                ServerModule.ServerModuleFactory.GetServerModule<IMaterialReturnedInTheDepot>();


            var varList = from a in context.S_ScrapGoods
                          where a.Bill_ID == billNo
                          && a.ResponsibilityProvider == a.Provider
                          select a;

            if (varList.Count() == 0)
            {
                return;
            }

            var varData = from a in context.S_ScrapBill
                          where a.Bill_ID == billNo
                          select a;

            if (varData.Count() == 0)
            {
                return;
            }

            S_ScrapBill bill = varData.Single();

            //操作主表
            S_MaterialReturnedInTheDepot returnBill = new S_MaterialReturnedInTheDepot();

            var data1 = from a in context.BASE_MaterialRequisitionPurpose
                        where a.Purpose == bill.ProductType
                        && a.Code.Substring(0, 1) == "F"
                        select a;

            returnBill.Bill_ID = m_assignBill.AssignNewNo(context, serverReturnedBill, CE_BillTypeEnum.领料退库单.ToString());
            returnBill.Bill_Time = ServerTime.Time;
            returnBill.BillStatus = MaterialReturnedInTheDepotBillStatus.已完成.ToString();
            returnBill.Department = bill.DeclareDepartment;
            returnBill.ReturnType = "其他退库";//退库类别
            returnBill.FillInPersonnel = bill.FillInPersonnel;
            returnBill.FillInPersonnelCode = bill.FillInPersonnelCode;
            returnBill.DepartmentDirector = bill.DepartmentDirector;
            returnBill.QualityInputer = "";
            returnBill.DepotManager = BasicInfo.LoginName;
            returnBill.PurposeCode = data1.First().Code;
            returnBill.ReturnReason = "由【报废单】："+ billNo +" 生成的报废退库";
            returnBill.Remark = "系统自动生成";
            returnBill.StorageID = "01";
            returnBill.ReturnMode = "领料退库";//退库方式
            returnBill.IsOnlyForRepair = false;
            returnBill.InDepotDate = ServerTime.Time;

            context.S_MaterialReturnedInTheDepot.InsertOnSubmit(returnBill);
            context.SubmitChanges();

            foreach (S_ScrapGoods goodsInfo in varList)
            {
                View_F_GoodsPlanCost goodsView = UniversalFunction.GetGoodsInfo(context, goodsInfo.GoodsID);

                QueryCondition_Store queryInfo = new QueryCondition_Store();

                queryInfo.BatchNo = goodsInfo.BatchNo;
                queryInfo.GoodsID = goodsInfo.GoodsID;
                queryInfo.StorageID = "01"; 

                S_Stock stockInfo = UniversalFunction.GetStockInfo(context, queryInfo);

                S_MaterialListReturnedInTheDepot detailInfo = new S_MaterialListReturnedInTheDepot();

                detailInfo.BatchNo = goodsInfo.BatchNo;
                detailInfo.Bill_ID = returnBill.Bill_ID;
                detailInfo.GoodsID = goodsInfo.GoodsID;
                detailInfo.Provider = goodsInfo.Provider;
                detailInfo.ReturnedAmount = goodsInfo.Quantity;
                detailInfo.Depot = goodsView.物品类别;
                detailInfo.ColumnNumber = stockInfo == null ? "" : stockInfo.ColumnNumber;
                detailInfo.LayerNumber = stockInfo == null ? "" : stockInfo.LayerNumber;
                detailInfo.ShelfArea = stockInfo == null ? "" : stockInfo.ShelfArea;
                detailInfo.ProviderBatchNo = stockInfo == null ? "" : stockInfo.ProviderBatchNo;
                detailInfo.Remark = "";

                context.S_MaterialListReturnedInTheDepot.InsertOnSubmit(detailInfo);
                context.SubmitChanges();
            }

            serverReturnedBill.OpertaionDetailAndStock(context, returnBill);
            context.SubmitChanges();
        }

        /// <summary>
        /// 赋值账务信息
        /// </summary>
        /// <param name="dataContxt">数据上下文</param>
        /// <param name="bill">单据信息</param>
        /// <param name="item">明细信息</param>
        /// <returns>返回账务信息</returns>
        S_FetchGoodsDetailBill AssignDetailInfo(DepotManagementDataContext dataContxt, S_ScrapBill bill, S_ScrapGoods item)
        {
            Service_Manufacture_WorkShop.IWorkShopStock serverWSStock =
                Service_Manufacture_WorkShop.ServerModuleFactory.GetServerModule<Service_Manufacture_WorkShop.IWorkShopStock>();
            Service_Manufacture_WorkShop.IWorkShopBasic serverWSBasic =
                Service_Manufacture_WorkShop.ServerModuleFactory.GetServerModule<Service_Manufacture_WorkShop.IWorkShopBasic>();
            WS_WorkShopCode tempWSCode = serverWSBasic.GetPersonnelWorkShop(dataContxt, bill.FillInPersonnelCode);

            S_FetchGoodsDetailBill detailBill = new S_FetchGoodsDetailBill();

            string error = "";
            if (!m_serverProductCode.UpdateProductStock(dataContxt, bill.Bill_ID, "报废", "05", false, item.GoodsID, out error))
            {
                throw new Exception(error);
            }

            detailBill.ID = Guid.NewGuid();
            detailBill.FetchBIllID = bill.Bill_ID;
            detailBill.BillTime = ServerTime.Time;
            detailBill.FetchCount = -(decimal)item.Quantity;
            detailBill.GoodsID = item.GoodsID;
            detailBill.BatchNo = item.BatchNo == "无批次" ? "" : item.BatchNo;

            WS_WorkShopStock tempWSStock = new WS_WorkShopStock();

            if (tempWSCode != null)
            {
                tempWSStock = serverWSStock.GetStockSingleInfo(dataContxt, tempWSCode.WSCode, item.GoodsID, item.BatchNo);
            }

            detailBill.UnitPrice = tempWSStock == null ? 0 : tempWSStock.UnitPrice;
            detailBill.Price = detailBill.UnitPrice * (decimal)detailBill.FetchCount;
            detailBill.Provider = item.Provider;
            detailBill.FillInPersonnel = bill.FillInPersonnel;
            detailBill.FinanceSignatory = null;
            detailBill.DepartDirector = bill.DepartmentDirector;
            detailBill.DepotManager = bill.DepotManager;
            detailBill.OperationType = (int)CE_SubsidiaryOperationType.报废;
            detailBill.Remark = "【报废】";
            detailBill.FillInDate = bill.Bill_Time;

            return detailBill;
        }

        /// <summary>
        /// 将物品的报废方式置空
        /// </summary>
        /// <param name="ctx">LINQ数据库上下文</param>
        /// <param name="billNo">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        private bool UpdateGoodsBill(DepotManagementDataContext ctx, string billNo, out string error)
        {
            error = null;

            var varData = from a in ctx.S_ScrapGoods
                          where a.Bill_ID == billNo
                          select a;

            foreach (var item in varData)
            {
                item.ScrapType = null;
            }

            return true;
        }

        /// <summary>
        /// 回退单据
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <param name="billStatus">单据状态</param>
        /// <param name="rebackReason">回退原因</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool ReturnBill(string djh, string billStatus, out string error, string rebackReason)
        {
            try
            {
                error = null;

                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.S_ScrapBill
                              where a.Bill_ID == djh
                              select a;

                string strMsg = "";

                if (varData.Count() == 1)
                {
                    S_ScrapBill lnqMRequ = varData.Single();

                    switch (billStatus)
                    {
                        case "新建单据":

                            strMsg = string.Format("{0}号报废单已回退，请您重新处理单据; 回退原因为" + rebackReason, djh);

                            m_billMessageServer.PassFlowMessage(djh, strMsg,
                                lnqMRequ.FillInPersonnelCode, false);

                            lnqMRequ.BillStatus = "新建单据";
                            lnqMRequ.DepartmentDirector = null;
                            lnqMRequ.Checker = null;
                            lnqMRequ.AuthorizeTime = null;

                            //if (!UpdateGoodsBill(ctx, djh, out error))
                            //{
                            //    return false;
                            //}

                            break;

                        case "等待主管审核":

                            strMsg = string.Format("{0}号报废单已回退，请您重新处理单据; 回退原因为" + rebackReason, djh);

                            m_billMessageServer.PassFlowMessage(djh, strMsg,
                                UniversalFunction.GetPersonnelCode(lnqMRequ.DepartmentDirector), false);

                            lnqMRequ.BillStatus = "等待主管审核";
                            lnqMRequ.DepartmentDirector = null;
                            lnqMRequ.Checker = null;
                            lnqMRequ.AuthorizeTime = null;

                            //if (!UpdateGoodsBill(ctx, djh, out error))
                            //{
                            //    return false;
                            //}

                            break;

                        case "等待SQE处理意见":

                            strMsg = string.Format("{0}号报废单已回退，请您重新处理单据; 回退原因为" + rebackReason, djh);

                            m_billMessageServer.PassFlowMessage(djh, strMsg,
                                UniversalFunction.GetPersonnelCode(lnqMRequ.SQE), false);

                            lnqMRequ.BillStatus = "等待SQE处理意见";
                            lnqMRequ.SQE = null;
                            lnqMRequ.SQETime = null;

                            //if (!UpdateGoodsBill(ctx,djh,out error))
                            //{
                            //    return false;
                            //}

                            break;

                        case "等待质检批准":

                            strMsg = string.Format("{0}号报废单已回退，请您重新处理单据; 回退原因为" + rebackReason, djh);

                            m_billMessageServer.PassFlowMessage(djh, strMsg,
                                UniversalFunction.GetPersonnelCode(lnqMRequ.Checker), false);

                            lnqMRequ.BillStatus = "等待质检批准";
                            lnqMRequ.Checker = null;
                            lnqMRequ.AuthorizeTime = null;
                            break;

                        default:
                            break;
                    }

                    ctx.SubmitChanges();
                    return true;
                }
                else
                {
                    error = "数据不唯一或者为空";
                    return false;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 提交SQE信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="lstGoods">报废物品清单</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool SubmitSQEMessage(string billNo, List<View_S_ScrapGoods> lstGoods, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.S_ScrapBill
                              where a.Bill_ID == billNo
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数据不唯一或者为空";
                    return false;
                }
                else
                {
                    S_ScrapBill newBill = varData.Single();

                    newBill.BillStatus = "等待仓管确认";
                    newBill.SQE = BasicInfo.LoginName;
                    newBill.SQETime = ServerTime.Time;

                    var varDatagoods = from a in ctx.S_ScrapGoods
                                       where a.Bill_ID == billNo
                                       select a;

                    foreach (var item in varDatagoods)
                    {
                        foreach (var itemIn in lstGoods)
                        {
                            if (item.GoodsID.ToString() == itemIn.物品ID.ToString()
                                && item.BatchNo.ToString() == itemIn.批次号.ToString()
                                && item.Quantity.ToString() == itemIn.报废数量.ToString()
                                && item.ResponsibilityDepartment.ToString() == itemIn.责任部门.ToString())
                            {
                                item.ScrapType = itemIn.报废方式;
                            }
                        }
                    }
                }

                ctx.SubmitChanges();

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
