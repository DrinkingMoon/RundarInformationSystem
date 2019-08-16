/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  MaterialReturnedInTheDepot.cs
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
using System.Data.Linq;
using System.Data.Linq.Mapping;
using DBOperate;
using PlatformManagement;
using GlobalObject;

namespace ServerModule
{
    /// <summary>
    /// 领料退库单管理类
    /// </summary>
    class MaterialReturnedInTheDepot : BasicServer, IMaterialReturnedInTheDepot
    {
        /// <summary>
        /// 工装信息服务组件
        /// </summary>
        IFrockStandingBook m_serverFrockStandingBook = PMS_ServerFactory.GetServerModule<IFrockStandingBook>();

        /// <summary>
        /// 箱体编码服务组件
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
            var varData = from a in ctx.S_MaterialReturnedInTheDepot
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
            string sql = "SELECT * FROM [DepotManagement].[dbo].[S_MaterialReturnedInTheDepot] where Bill_ID = '" + billNo + "'";

            System.Data.DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 获取领料退库单信息
        /// </summary>
        /// <param name="returnBill">领料退库单</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取领料出库信息</returns>
        public bool GetAllBill(out IQueryResult returnBill, out string error)
        {
            returnBill = null;
            error = null;

            IAuthorization authorization = PlatformFactory.GetObject<IAuthorization>();
            IQueryResult qr = null;

            if (QueryResultFilter == null)
            {
                qr = authorization.Query("领料退库单查询", null);
            }
            else
            {
                qr = authorization.Query("领料退库单查询", null, QueryResultFilter);
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
        /// 获取领料退库单信息
        /// </summary>
        /// <param name="billNo">领料退库单号</param>
        /// <returns>成功返回获取领料出库信息, 失败返回null</returns>
        public S_MaterialReturnedInTheDepot GetBill(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            var result = from r in ctx.S_MaterialReturnedInTheDepot
                         where r.Bill_ID == billNo
                         select r;

            if (result.Count() > 0)
            {
                return result.Single();
            }

            return null;
        }

        /// <summary>
        /// 获取领料退库单视图信息
        /// </summary>
        /// <param name="billNo">领料退库单号</param>
        /// <returns>成功返回获取领料出库信息, 失败返回null</returns>
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

        /// <summary>
        /// 为领料而获取所有已经完成的单据信息
        /// </summary>
        /// <param name="returnBill">查询到的单据信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功的标志</returns>
        public bool GetAllBillForFetchGoods(out IQueryResult returnBill, out string error)
        {
            if (!GetAllBill(out returnBill, out error))
            {
                return false;
            }

            System.Data.DataTable data = returnBill.DataCollection.Tables[0];

            for (int i = 0; i < data.Rows.Count; i++)
            {
                if (data.Rows[i]["单据状态"].ToString() == MaterialReturnedInTheDepotBillStatus.新建单据.ToString()) //!= MaterialReturnedInTheDepotBillStatus.已完成.ToString())
                {
                    data.Rows.RemoveAt(i);
                    i--;
                }
            }

            return true;
        }

        /// <summary>
        /// 添加领料退库单
        /// </summary>
        /// <param name="bill">领料退库单信息</param>
        /// <param name="returnBill">返回更新后的领料退库单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加领料退库单</returns>
        public bool AddBill(S_MaterialReturnedInTheDepot bill, out IQueryResult returnBill, out string error)
        {
            returnBill = null;
            error = null;

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            try
            {
                if (GlobalObject.GeneralFunction.IsNullOrEmpty(bill.Bill_ID))
                {
                    throw new Exception("【单据号】获取失败，请重新再试");
                }

                ctx.S_MaterialReturnedInTheDepot.InsertOnSubmit(bill);
                ctx.SubmitChanges();

                if (!GetAllBill(out returnBill, out error))
                {
                    return false;
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
        /// 修改领料退库单(只修改编制人涉及信息)
        /// </summary>
        /// <param name="bill">领料退库单信息</param>
        /// <param name="returnBill">返回更新后的领料退库单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加领料退库单</returns>
        public bool UpdateBill(S_MaterialReturnedInTheDepot bill, out IQueryResult returnBill, out string error)
        {
            returnBill = null;
            error = null;

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            try
            {
                var result = from r in ctx.S_MaterialReturnedInTheDepot where r.Bill_ID == bill.Bill_ID select r;

                if (result.Count() == 0)
                {
                    error = string.Format("没有找到单据号为 {0} 的领料退库单信息，无法进行此操作", bill.Bill_ID);
                    return false;
                }

                S_MaterialReturnedInTheDepot updateBill = result.Single();

                updateBill.Bill_Time = ServerModule.ServerTime.Time;
                updateBill.PurposeCode = bill.PurposeCode;
                updateBill.ReturnReason = bill.ReturnReason;
                updateBill.ReturnType = bill.ReturnType;
                updateBill.Remark = bill.Remark;
                updateBill.IsOnlyForRepair = bill.IsOnlyForRepair;
                updateBill.ReturnMode = bill.ReturnMode;
                updateBill.StorageID = bill.StorageID;

                ctx.SubmitChanges();

                if (!GetAllBill(out returnBill, out error))
                {
                    return false;
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
        /// 删除领料退库单
        /// </summary>
        /// <param name="billNo">领料退库单号</param>
        /// <param name="returnBill">返回更新后重新查询的领料退库单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功删除领料退库单号</returns>
        public bool DeleteBill(string billNo, out IQueryResult returnBill, out string error)
        {
            returnBill = null;
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                Table<S_MaterialReturnedInTheDepot> table = ctx.GetTable<S_MaterialReturnedInTheDepot>();
                var delRow = from c in table where c.Bill_ID == billNo select c;

                m_assignBill.CancelBillNo(ctx, "领料退库单", billNo);

                table.DeleteAllOnSubmit(delRow);

                //对于营销的总称领料的删除
                var varData = from a in ctx.ProductsCodes
                              where a.DJH == billNo
                              select a;

                ctx.ProductsCodes.DeleteAllOnSubmit(varData);

                ctx.SubmitChanges();

                return GetAllBill(out returnBill, out error);
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 领料出库人提交单据(交给主管审批)
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="returnBill">返回更新后重新查询的领料退库单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加领料退库单</returns>
        public bool SubmitNewBill(string billNo, out IQueryResult returnBill, out string error)
        {
            returnBill = null;
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var result = from r in ctx.S_MaterialReturnedInTheDepot where r.Bill_ID == billNo select r;

                if (result.Count() == 0)
                {
                    error = string.Format("没有找到单据号为 {0} 的领料退库单信息，无法进行此操作", billNo);
                    return false;
                }

                var varList = from a in ctx.S_MaterialListReturnedInTheDepot where a.Bill_ID == billNo select a;

                foreach (S_MaterialListReturnedInTheDepot item in varList)
                {
                    if (item.Provider == null || item.Provider.Trim().Length == 0)
                    {
                        error = string.Format("有记录供应商为空，供应商不能为空，请重新核对");
                        return false;
                    }
                }

                result.Single().BillStatus = MaterialReturnedInTheDepotBillStatus.等待主管审核.ToString();
                result.Single().Bill_Time = ServerModule.ServerTime.Time;
                ctx.SubmitChanges();

                return GetAllBill(out returnBill, out error);
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 主管审批单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="name">主管姓名</param>
        /// <param name="returnBill">返回更新后重新查询的领料退库单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加领料退库单</returns>
        public bool DirectorAuthorizeBill(string billNo, string name, out IQueryResult returnBill, out string error)
        {
            returnBill = null;
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var result = from r in ctx.S_MaterialReturnedInTheDepot where r.Bill_ID == billNo select r;

                if (result.Count() == 0)
                {
                    error = string.Format("没有找到单据号为 {0} 的领料退库单信息，无法进行此操作", billNo);
                    return false;
                }

                result.Single().DepartmentDirector = name;
                result.Single().BillStatus = MaterialReturnedInTheDepotBillStatus.等待质检批准.ToString();
                ctx.SubmitChanges();

                return GetAllBill(out returnBill, out error);
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 质量批准单据
        /// </summary>
        /// <param name="billNo">单据编号</param>
        /// <param name="name">质量签名</param>
        /// <param name="returnBill">添加完毕后查询数据库的返回结果</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作结果</returns>
        public bool QualityAuthorizeBill(string billNo, string name, out IQueryResult returnBill, out string error)
        {
            returnBill = null;
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
                var result = from r in ctx.S_MaterialReturnedInTheDepot where r.Bill_ID == billNo select r;

                if (result.Count() == 0)
                {
                    error = string.Format("没有找到单据号为 {0} 的领料退库单信息，无法进行此操作", billNo);
                    return false;
                }

                result.Single().QualityInputer = name;
                result.Single().BillStatus = MaterialReturnedInTheDepotBillStatus.等待仓管退库.ToString();
                ctx.SubmitChanges();

                return GetAllBill(out returnBill, out error);
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 工装业务处理
        /// </summary>
        /// <param name="ctx">LINQ数据上下文</param>
        /// <param name="billNo">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool FinishFrock(DepotManagementDataContext ctx, string billNo, out string error)
        {
            error = null;

            try
            {
                var varData = from a in ctx.S_MaterialListReturnedInTheDepot
                              where a.Bill_ID == billNo
                              select a;

                bool blFlag = false;

                foreach (var item in varData)
                {
                    if (UniversalFunction.IsInStandingBook(item.GoodsID, false))
                    {
                        blFlag = true;

                        if (!m_serverFrockStandingBook.IsOperationCountMateBillCount(item.GoodsID, item.Bill_ID, Convert.ToDecimal(item.ReturnedAmount)))
                        {
                            error = "领料数量与工装业务编号数不一致，请重新确认数量的一致性";
                            return false;
                        }
                    }
                }

                if (blFlag)
                {
                    string strSql = "select b.* from S_MaterialListReturnedInTheDepot as a inner join S_FrockOperation as b " +
                        " on a.Bill_ID = b.BillID and a.GoodsID = b.GoodsID where a.Bill_ID = '" + billNo + "'";

                    if (!m_serverFrockStandingBook.UpdateFrockStandingBookStock(ctx,
                        GlobalObject.DatabaseServer.QueryInfo(strSql), true, out error))
                    {
                        return false;
                    }

                    var varReport = from a in ctx.S_FrockOperation
                                    where a.BillID == billNo
                                    select a;

                    foreach (var itemReport in varReport)
                    {
                        itemReport.IsTrue = true;
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
        /// 完成领料退库单
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="storeManager">仓库管理员</param>
        /// <param name="returnBill">返回更新后重新查询的领料退库单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加领料退库单</returns>
        public bool FinishBill(string billNo, string storeManager, out IQueryResult returnBill, out string error)
        {
            returnBill = null;
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                if (!FinishFrock(ctx, billNo, out error))
                {
                    return false;
                }

                //检查总成领用数量是否都设置了流水码
                if (!m_serverProductCode.IsFitCountInReturnBill(billNo, out error))
                {
                    return false;
                }

                var result = from r in ctx.S_MaterialReturnedInTheDepot where r.Bill_ID == billNo select r;

                if (result.Count() == 0)
                {
                    error = string.Format("没有找到单据号为 {0} 的领料退库单信息，无法进行此操作", billNo);
                    return false;
                }

                S_MaterialReturnedInTheDepot bill = result.Single();

                if (bill.BillStatus == MaterialReturnedInTheDepotBillStatus.已完成.ToString())
                {
                    error = "单据不能重复退库";
                    return false;
                }

                bill.DepotManager = storeManager;
                bill.BillStatus = MaterialReturnedInTheDepotBillStatus.已完成.ToString();
                bill.InDepotDate = ServerTime.Time;

                IMaterialListReturnedInTheDepot goodsServer = ServerModuleFactory.GetServerModule<IMaterialListReturnedInTheDepot>();
                
                //操作账务信息与库存信息
                OpertaionDetailAndStock(ctx, bill);

                //操作总成库存状态
                var varList = from a in ctx.S_MaterialListReturnedInTheDepot
                              where a.Bill_ID == bill.Bill_ID
                              select a;

                foreach (var item in varList)
                {
                    bool blIsRepaired = false;

                    if (bill.StorageID == "05" && Convert.ToBoolean(item.RepairStatus))
                    {
                        blIsRepaired = true;
                    }

                    if (!m_serverProductCode.UpdateProductStock(ctx, bill.Bill_ID, "领料退库", bill.StorageID, blIsRepaired, item.GoodsID, out error))
                    {
                        return false;
                    }

                    IStoreServer serverStore = ServerModuleFactory.GetServerModule<IStoreServer>();

                    YX_AfterServiceStock lnqAfterService = new YX_AfterServiceStock();

                    lnqAfterService.GoodsID = item.GoodsID;
                    lnqAfterService.OperationCount = Convert.ToDecimal( item.ReturnedAmount);
                    lnqAfterService.RepairStatus = Convert.ToBoolean( item.RepairStatus);
                    lnqAfterService.StorageID = bill.StorageID;

                    if (!serverStore.OperationYXAfterService(ctx, lnqAfterService, out error))
                    {
                        return false;
                    }
                }

                // 正式使用单据号
                m_assignBill.UseBillNo(ctx, "领料退库单", bill.Bill_ID);

                ctx.SubmitChanges();

                return GetAllBill(out returnBill, out error);
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
        public void OpertaionDetailAndStock(DepotManagementDataContext dataContext, S_MaterialReturnedInTheDepot bill)
        {
            MaterialListReturnedInTheDepot listService = new MaterialListReturnedInTheDepot();
            IFinancialDetailManagement serverDetail =
                ServerModule.ServerModuleFactory.GetServerModule<IFinancialDetailManagement>();

            var result = from r in dataContext.S_MaterialListReturnedInTheDepot
                         where r.Bill_ID == bill.Bill_ID
                         select r;

            if (result == null || result.Count() == 0)
            {
                throw new Exception("获取单据信息失败");
            }

            foreach (var item in result)
            {
                S_FetchGoodsDetailBill detailInfo = listService.AssignDetailInfo(dataContext, bill, item);
                S_Stock stockInfo = listService.AssignStockInfo(dataContext, bill, item);

                if (detailInfo == null || stockInfo == null)
                {
                    throw new Exception("获取账务信息或者库存信息失败");
                }

                serverDetail.ProcessFetchGoodsDetail(dataContext, detailInfo, stockInfo);
            }

            IStoreServer serviceStore = ServerModuleFactory.GetServerModule<IStoreServer>();
            serviceStore.Operation_MES_InProduction(dataContext, bill.Bill_ID);
        }

        /// <summary>
        /// 回退单据
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <param name="billStatus">单据状态</param>
        /// <param name="returnBill">返回回退更新后查询的数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <param name="rebackReason">回退原因</param>
        /// <returns>回退成功返回True，回退失败返回False</returns>
        public bool ReturnBill(string djh, string billStatus,
            out IQueryResult returnBill, out string error, string rebackReason)
        {
            returnBill = null;
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.S_MaterialReturnedInTheDepot
                              where a.Bill_ID == djh
                              select a;

                string strMsg = "";

                if (varData.Count() == 1)
                {
                    S_MaterialReturnedInTheDepot lnqMRe = varData.Single();

                    switch (billStatus)
                    {
                        case "新建单据":

                            strMsg = string.Format("{0}号领料退库单已回退，请您重新处理单据; 回退原因为" + rebackReason, djh);
                            m_billMessageServer.PassFlowMessage(djh, strMsg, lnqMRe.FillInPersonnelCode, false);

                            lnqMRe.BillStatus = "新建单据";
                            lnqMRe.DepartmentDirector = null;
                            lnqMRe.QualityInputer = null;
                            break;

                        case "等待主管审核":

                            strMsg = string.Format("{0}号领料退库单已回退，请您重新处理单据; 回退原因为" + rebackReason, djh);
                            m_billMessageServer.PassFlowMessage(djh, strMsg,
                                UniversalFunction.GetPersonnelCode(lnqMRe.DepartmentDirector), false);

                            lnqMRe.BillStatus = "等待主管审核";
                            lnqMRe.DepartmentDirector = null;
                            lnqMRe.QualityInputer = null;
                            break;

                        case "等待质检批准":

                            strMsg = string.Format("{0}号领料退库单已回退，请您重新处理单据; 回退原因为" + rebackReason, djh);
                            m_billMessageServer.PassFlowMessage(djh, strMsg,
                                UniversalFunction.GetPersonnelCode(lnqMRe.QualityInputer), false);

                            lnqMRe.BillStatus = "等待质检批准";
                            lnqMRe.QualityInputer = null;
                            break;

                        default:
                            break;
                    }

                    ctx.SubmitChanges();
                }
                else
                {
                    error = "数据不唯一或者此数据已经被删除";
                    return false;
                }

                return GetAllBill(out returnBill, out error);
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }
    }
}
