/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  MaterialRequisitionServer.cs
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
    /// 领料类型
    /// </summary>
    public enum FetchGoodsType 
    { 
        /// <summary>
        /// 零星领料
        /// </summary>
        零星领料,

        /// <summary>
        /// 整台领料
        /// </summary>
        整台领料,

        /// <summary>
        /// 整台领料
        /// </summary>
        整台领料不含后补充,

        /// <summary>
        /// 后补充
        /// </summary>
        后补充,

        /// <summary>
        /// 阀块领料
        /// </summary>
        阀块领料,

        /// <summary>
        /// 行星轮合件领料
        /// </summary>
        行星轮合件领料,

        /// <summary>
        /// 油底壳领料
        /// </summary>
        油底壳领料 
    }

    /// <summary>
    /// 领料单管理类
    /// </summary>
    class MaterialRequisitionServer : BasicServer, IMaterialRequisitionServer
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
        /// 还货服务组件
        /// </summary>
        IProductReturnService m_serverReturn = ServerModuleFactory.GetServerModule<IProductReturnService>();

        /// <summary>
        /// 检查某业务是否存在
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">业务号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.S_MaterialRequisition
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
            string sql = "SELECT * FROM [DepotManagement].[dbo].[S_MaterialRequisition] where Bill_ID = '" + billNo + "'";

            System.Data.DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 检查是否存在填写了指定报废单号的单据
        /// </summary>
        /// <param name="associatedBillNo">关联单号</param>
        /// <returns>存在返回true</returns>
        public bool IsExistAssociatedBill(string associatedBillNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var result = from r in ctx.S_MaterialRequisition
                         where r.AssociatedBillNo == associatedBillNo
                         && r.BillStatus != "已报废"
                         select r;

            if (result.Count() > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 获取包含了指定关联单号的领料单
        /// </summary>
        /// <param name="associatedBillNo">关联单号</param>
        /// <returns>返回获取到的关联单据</returns>
        public IQueryable<S_MaterialRequisition> ContainAssociatedBill(string associatedBillNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            return from r in ctx.S_MaterialRequisition
                   where r.AssociatedBillNo == associatedBillNo
                   && r.BillStatus != "已报废"
                   select r;
        }

        /// <summary>
        /// 获得单条领料单记录
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回单条领料单信息</returns>
        public S_MaterialRequisition GetBill(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.S_MaterialRequisition
                          where a.Bill_ID == billNo
                          select a;

            if (varData.Count() != 1)
            {
                return null;
            }
            else
            {
                return varData.Single();
            }
        }

        /// <summary>
        /// 获取领料单信息
        /// </summary>
        /// <param name="returnBill">领料单</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取领料信息</returns>
        public bool GetAllBill(out IQueryResult returnBill, out string error)
        {
            returnBill = null;

            error = null;

            IAuthorization authorization = PlatformFactory.GetObject<IAuthorization>();
            IQueryResult qr = null;

            if (QueryResultFilter == null)
            {
                qr = authorization.Query("领料单查询", null);
            }
            else
            {
                qr = authorization.Query("领料单查询", null, QueryResultFilter);
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
        /// 获取领料单视图信息
        /// </summary>
        /// <param name="billNo">领料单号</param>
        /// <returns>成功返回获取领料信息, 失败返回null</returns>
        public View_S_MaterialRequisition GetBillView(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var result = from r in ctx.View_S_MaterialRequisition
                         where r.领料单号 == billNo
                         select r;

            if (result.Count() > 0)
            {
                return result.Single();
            }

            return null;
        }

        /// <summary>
        /// 添加领料单
        /// </summary>
        /// <param name="bill">领料单单据信息</param>
        /// <param name="returnBill">返回更新后的领料单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加领料单</returns>
        public bool AddBill(S_MaterialRequisition bill, out IQueryResult returnBill, out string error)
        {
            returnBill = null;

            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                if (GlobalObject.GeneralFunction.IsNullOrEmpty(bill.Bill_ID))
                {
                    throw new Exception("【单据号】获取失败，请重新再试");
                }

                ctx.S_MaterialRequisition.InsertOnSubmit(bill);

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
        /// 修改领料单
        /// </summary>
        /// <param name="bill">领料单单据信息</param>
        /// <param name="returnBill">返回更新后的领料单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加领料单</returns>
        public bool UpdateBill(S_MaterialRequisition bill, out IQueryResult returnBill, out string error)
        {
            returnBill = null;
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var result = from r in ctx.S_MaterialRequisition where r.Bill_ID == bill.Bill_ID select r;

                if (result.Count() == 0)
                {
                    error = string.Format("没有找到单据号为 {0} 的领料单信息，无法进行此操作", bill.Bill_ID);
                    return false;
                }

                S_MaterialRequisition updateBill = result.Single();

                updateBill.Bill_Time = ServerModule.ServerTime.Time;
                updateBill.FetchType = bill.FetchType;
                updateBill.AssociatedBillType = bill.AssociatedBillType;
                updateBill.AssociatedBillNo = bill.AssociatedBillNo;
                updateBill.FetchCount = bill.FetchCount;
                updateBill.ProductType = bill.ProductType;
                updateBill.PurposeCode = bill.PurposeCode;
                updateBill.Remark = bill.Remark;
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
        /// 修改领料单
        /// </summary>
        /// <param name="billNo">单据编号</param>
        /// <param name="purposeCode">用途编号</param>
        /// <param name="remark">备注</param>
        /// <param name="returnBill">返回更新后的领料单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加领料单</returns>
        public bool UpdateBill(string billNo, string purposeCode, string remark, out IQueryResult returnBill, out string error)
        {
            returnBill = null;
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var result = from r in ctx.S_MaterialRequisition 
                             where r.Bill_ID == billNo select r;

                if (result.Count() == 0)
                {
                    error = string.Format("没有找到单据号为 {0} 的领料单信息，无法进行此操作", billNo);
                    return false;
                }

                S_MaterialRequisition updateBill = result.Single();

                updateBill.Bill_Time = ServerModule.ServerTime.Time;

                if (purposeCode != null)
                {
                    updateBill.PurposeCode = purposeCode;
                }

                updateBill.Remark = remark;

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
        /// 删除领料单
        /// </summary>
        /// <param name="billNo">领料单号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功删除领料单号</returns>
        public bool DeleteBill(string billNo, out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var delRow = from c in ctx.S_MaterialRequisition 
                             where c.Bill_ID == billNo select c;

                if (delRow.Count() == 1)
                {
                    ctx.S_MaterialRequisition.DeleteAllOnSubmit(delRow);
                }
                else
                {
                    error = "数据不唯一或者为空";
                    return false;
                }

                var result = from r in ctx.S_FetchGoodsDetailBill
                             where r.FetchBIllID == billNo
                             select r;

                if (result.Count() > 0)
                    ctx.S_FetchGoodsDetailBill.DeleteAllOnSubmit(result);

                //对于营销的总称领料的删除
                var varData = from a in ctx.ProductsCodes
                              where a.DJH == billNo
                              select a;

                ctx.ProductsCodes.DeleteAllOnSubmit(varData);

                m_serverReturn.DeleteInfo(ctx, billNo);
                ctx.SubmitChanges();
                m_assignBill.CancelBillNo(ctx, "领料单", billNo);

                return true;
            }
            catch (Exception err)
            {
                error = err.Message;
                return false;
            }
        }

        /// <summary>
        /// 删除领料单
        /// </summary>
        /// <param name="billNo">领料单号</param>
        /// <param name="returnBill">返回更新后重新查询的领料单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功删除领料单号</returns>
        public bool DeleteBill(string billNo, out IQueryResult returnBill, out string error)
        {
            returnBill = null;
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                if (!DeleteBill(billNo, out error))
                    return false;

                return GetAllBill(out returnBill, out error);
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 领料人提交单据(交给主管审批)
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="returnBill">返回更新后重新查询的领料单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加领料单</returns>
        public bool SubmitNewBill(string billNo, out IQueryResult returnBill, out string error)
        {
            returnBill = null;
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var result = from r in ctx.S_MaterialRequisition where r.Bill_ID == billNo select r;

                if (result.Count() == 0)
                {
                    error = string.Format("没有找到单据号为 {0} 的领料单信息，无法进行此操作", billNo);
                    return false;
                }

                result.Single().BillStatus = MaterialRequisitionBillStatus.等待主管审核.ToString();
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
        /// <param name="returnBill">返回更新后重新查询的领料单数据集</param>
        /// <param name="msg">返回更新后的单据状态</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加领料单</returns>
        public bool AuthorizeBill(string billNo, string name, out MaterialRequisitionBillStatus msg, out IQueryResult returnBill, out string error)
        {
            returnBill = null;
            error = null;
            msg = MaterialRequisitionBillStatus.新建单据;
            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var result = from r in ctx.S_MaterialRequisition where r.Bill_ID == billNo select r;

                if (result.Count() == 0)
                {
                    error = string.Format("没有找到单据号为 {0} 的领料单信息，无法进行此操作", billNo);
                    return false;
                }

                if (CheckIsOutToplimit(billNo))
                {
                    msg = MaterialRequisitionBillStatus.等待部门领导批准;
                }

                var resultFrock = from a in ctx.S_MaterialRequisitionGoods
                                  where a.Bill_ID == billNo
                                  select a;

                foreach (var item in resultFrock)
                {
                    if (UniversalFunction.IsInStandingBook(item.GoodsID, true))
                    {
                        msg = MaterialRequisitionBillStatus.等待工艺人员批准;
                        break;
                    }
                }

                result.Single().DepartmentDirector = name;

                if (msg == MaterialRequisitionBillStatus.等待部门领导批准)
                {
                    result.Single().BillStatus = MaterialRequisitionBillStatus.等待部门领导批准.ToString();
                }
                else if (msg == MaterialRequisitionBillStatus.等待工艺人员批准)
                {
                    result.Single().BillStatus = MaterialRequisitionBillStatus.等待工艺人员批准.ToString();
                }
                else
                {
                    result.Single().BillStatus = MaterialRequisitionBillStatus.等待出库.ToString();
                }

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
        /// 针对于领料单的工装业务处理
        /// </summary>
        /// <param name="ctx">LINQ数据上下文</param>
        /// <param name="billNo">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool FinishFrock(DepotManagementDataContext ctx, string billNo, out string error)
        {
            error = null;

            try
            {
                var varData = from a in ctx.S_MaterialRequisitionGoods
                              where a.Bill_ID == billNo
                              select a;

                bool blFlag = false;

                foreach (var item in varData)
                {
                    if (UniversalFunction.IsInStandingBook(item.GoodsID, true))
                    {
                        blFlag = true;

                        if (!m_serverFrockStandingBook.IsOperationCountMateBillCount(item.GoodsID, item.Bill_ID, Convert.ToDecimal(item.RealCount)))
                        {
                            error = "领料数量与工装业务编号数不一致，请重新确认数量的一致性";
                            return false;
                        }
                    }
                }

                if (blFlag)
                {
                    string strSql = "select b.* from S_MaterialRequisitionGoods as a inner join S_FrockOperation as b " +
                        " on a.Bill_ID = b.BillID and a.GoodsID = b.GoodsID where a.Bill_ID = '" + billNo + "'";

                    if (!m_serverFrockStandingBook.UpdateFrockStandingBookStock(ctx,
                        GlobalObject.DatabaseServer.QueryInfo(strSql), false, out error))
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
        /// 完成不合格品隔离单并且解封隔离物品
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billID">单据号</param>
        /// <param name="storageID">库房ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回True，失败返回False</returns>
        private bool UpdateIsolationManageBillStatus(DepotManagementDataContext ctx,  string billID, string storageID, out string error)
        {
            error = null;

            try
            {
                var varMaterial = from a in ctx.S_MaterialRequisitionGoods
                                  where a.Bill_ID == billID
                                  select a;

                foreach (var item in varMaterial)
                {
                    var varData = from a in ctx.S_IsolationManageBill
                                  where a.GoodsID == item.GoodsID
                                  && a.BatchNo == item.BatchNo
                                  && a.StorageID == storageID
                                  && a.DJZT == "等待领料" 
                                  select a;

                    if (varData.Count() == 1)
                    {
                        S_IsolationManageBill lnqIsolation = varData.Single();

                        lnqIsolation.DJZT = "单据已完成";
                        lnqIsolation.AssociateRejectBillID = billID;

                        var varStock = from a in ctx.S_Stock
                                       where a.GoodsID == item.GoodsID
                                       && a.BatchNo == item.BatchNo
                                       && a.Provider == item.ProviderCode
                                       && a.StorageID == storageID
                                       select a;

                        if (varStock.Count() == 1)
                        {
                            S_Stock lnqStock = varStock.Single();

                            lnqStock.GoodsStatus = Convert.ToInt32( lnqStock.OldGoodsStatus);
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

        #region 2017-9-20 夏石友，出库时需检测物料状态
        /// <summary>
        /// 检查物料中是否存在隔离物品
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回隔离的物品信息</returns>
        public List<View_S_MaterialRequisitionGoods> IsExistsIsolationGoods(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

             var varMaterial = from a in ctx.View_S_MaterialRequisitionGoods
                              where a.领料单号 == billNo
                              select a;

            List<View_S_MaterialRequisitionGoods> lstGoods = new List<View_S_MaterialRequisitionGoods>();

            foreach (var item in varMaterial)
            {
                var stockInfo = from r in ctx.S_Stock
                                where r.GoodsID == item.物品ID &&
                                      r.BatchNo == item.批次号 &&
                                      r.StorageID == item.StorageID &&
                                      r.Provider == item.供应商编码
                                select r;

                if (stockInfo.Count() == 1)
                {
                    if (stockInfo.Single().GoodsStatus == 3)
                    {
                        lstGoods.Add(item);
                    }
                }
                else
                {
                    throw new Exception(string.Format("【{0}】库房中不存在物品名称【{1}】、批次【{2}】的物料", 
                        item.StorageID, item.物品名称, item.批次号));
                }
            }

            return lstGoods;
        }
        #endregion

        /// <summary>
        /// 完成领料单
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="storeManager">仓库管理员</param>
        /// <param name="returnBill">返回更新后重新查询的领料单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加领料单</returns>
        public bool FinishBill(string billNo, string storeManager, out IQueryResult returnBill, out string error)
        {
            returnBill = null;
            error = null;

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();

            try
            {
                //处理工装业务
                if (!FinishFrock(ctx, billNo, out error))
                {
                    throw new Exception(error);
                }

                //检查总成领用数量是否都设置了流水码
                if (!m_serverProductCode.IsFitCountInRequisitionBill(billNo, out error))
                {
                    throw new Exception(error);
                }
                
                var result = from r in ctx.S_MaterialRequisition where r.Bill_ID == billNo select r;

                if (result.Count() == 0)
                {
                    throw new Exception(string.Format("没有找到单据号为 {0} 的领料单信息，无法进行此操作", billNo));
                }

                S_MaterialRequisition fetchBill = result.Single();

                if (fetchBill.BillStatus == MaterialRequisitionBillStatus.已出库.ToString())
                {
                    throw new Exception("单据不能重复领料");
                }

                fetchBill.DepotManager = storeManager;
                fetchBill.BillStatus = MaterialRequisitionBillStatus.已出库.ToString();
                fetchBill.OutDepotDate = ServerTime.Time;

                // 添加出库明细表记录
                IMaterialRequisitionGoodsServer goodsServer = ServerModuleFactory.GetServerModule<IMaterialRequisitionGoodsServer>();

                //操作账务信息与库存信息
                OpertaionDetailAndStock(ctx, fetchBill);

                IPurcharsingPlan serverPurcharsing = ServerModuleFactory.GetServerModule<IPurcharsingPlan>();

                #region 操作总成库存状态
                var varList = from a in ctx.S_MaterialRequisitionGoods
                              where a.Bill_ID == fetchBill.Bill_ID
                              select a;

                foreach (var item in varList)
                {
                    bool blIsRepaired = false;

                    if (fetchBill.StorageID == "05" && Convert.ToBoolean(item.RepairStatus))
                    {
                        blIsRepaired = true;
                    }

                    if (!m_serverProductCode.UpdateProductStock(ctx, fetchBill.Bill_ID, "领料", fetchBill.StorageID, blIsRepaired, item.GoodsID, out error))
                    {
                        throw new Exception(error);
                    }

                    IStoreServer serverStore = ServerModuleFactory.GetServerModule<IStoreServer>();

                    YX_AfterServiceStock lnqAfterService = new YX_AfterServiceStock();

                    lnqAfterService.GoodsID = item.GoodsID;
                    lnqAfterService.OperationCount = -item.RealCount;
                    lnqAfterService.RepairStatus = Convert.ToBoolean( item.RepairStatus);
                    lnqAfterService.StorageID = fetchBill.StorageID;

                    if (!serverStore.OperationYXAfterService(ctx, lnqAfterService, out error))
                    {
                        throw new Exception(error);
                    }
                }
                #endregion

                //对隔离单进行业务操作
                if (fetchBill.PurposeCode == "1208" 
                    && !UpdateIsolationManageBillStatus(ctx, fetchBill.Bill_ID, fetchBill.StorageID, out error))
                {
                    throw new Exception(error);
                }

                #region 已添加新的整台份请领单 Modify by cjb on 2015.1.22
                ////if (fetchBill.FetchType == FetchGoodsType.整台领料.ToString())
                ////{
                ////    AutomaticCVTMaterialRequisition(ctx, fetchBill);
                ////}
                #endregion

                AddMultiBatchPartForBoard(ctx, fetchBill);

                // 正式使用单据号
                m_assignBill.UseBillNo(ctx, "领料单", fetchBill.Bill_ID);

                ctx.SubmitChanges();

                ctx.Transaction.Commit();

                //// 正式使用单据号
                //m_assignBill.UseBillNo(ctx, "领料单", fetchBill.Bill_ID); // 2017.12.6 此语句不能放在事务提交完成后

                return GetAllBill(out returnBill, out error);
            }
            catch (Exception exce)
            {
                ctx.Transaction.Rollback();
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 看板发料添加多批次管理
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="fetchBill">领料单主表对象</param>
        void AddMultiBatchPartForBoard(DepotManagementDataContext ctx, S_MaterialRequisition fetchBill)
        {
            string m_error = "";

            try
            {
                if (fetchBill == null || fetchBill.Bill_ID == null || fetchBill.BillStatus == null
                    || GlobalObject.GeneralFunction.StringConvertToEnum<MaterialRequisitionBillStatus>(fetchBill.BillStatus)!= 
                    MaterialRequisitionBillStatus.已出库
                    )
                {
                    return;
                }

                FetchGoodsType fetchType = GlobalObject.GeneralFunction.StringConvertToEnum<FetchGoodsType>(fetchBill.FetchType);

                if ((fetchType != FetchGoodsType.整台领料 && fetchType != FetchGoodsType.后补充) || fetchBill.Remark != "看板领料自动生成")
                {
                    return;
                }

                // 领料单物品清单服务
                IMaterialRequisitionGoodsServer goodsServer = ServerModuleFactory.GetServerModule<IMaterialRequisitionGoodsServer>();
                IMultiBatchPartServer multiBatchPartServer = ServerModuleFactory.GetServerModule<IMultiBatchPartServer>();

                List<View_S_MaterialRequisitionGoods> lstMRGoods = (from r in goodsServer.GetGoods(fetchBill.Bill_ID)
                                                                    orderby r.显示位置
                                                                    select r).ToList();

                if (lstMRGoods.Count > 0)
                {
                    List<StorageGoods> lstGoods = new List<StorageGoods>(lstMRGoods.Count);

                    foreach (var item in lstMRGoods)
                    {
                        StorageGoods goods = new StorageGoods();

                        goods.GoodsCode = item.图号型号;
                        goods.GoodsName = item.物品名称;
                        goods.Spec = item.规格;
                        goods.Provider = item.供应商编码;
                        goods.BatchNo = item.批次号;
                        goods.Quantity = item.实领数;
                        goods.StorageID = item.StorageID;

                        lstGoods.Add(goods);
                    }

                    if (!multiBatchPartServer.AddFromBill(BasicInfo.LoginID, 9, "", fetchBill.Bill_ID, lstGoods, out m_error))
                    {
                        throw new Exception(m_error);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 对于整台份领料不足物料自动生成领料单
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="materialRequisition">LINQ数据集领料单</param>
        void AutomaticCVTMaterialRequisition(DepotManagementDataContext ctx, S_MaterialRequisition materialRequisition)
        {
            string strSql = " select a.GoodsID, case when b.RealCount is null then a.Redices "+
                            " else a.Redices - b.RealCount end as Redices from "+
                            " (select Edition, GoodsID, Redices * " + materialRequisition.FetchCount + " as Redices " +
                            " from BASE_ProductOrder where Edition = '"+ materialRequisition.ProductType +"') as a "+
                            " left join (select ProductType, GoodsID, SUM(RealCount) as RealCount from S_MaterialRequisition as a  "+
                            " inner join S_MaterialRequisitionGoods as b on a.Bill_ID = b.Bill_ID "+
                            " where FetchType = '" + FetchGoodsType.整台领料.ToString() + "' and a.Bill_ID = '" + materialRequisition.Bill_ID + "' " +
                            " group by GoodsID,ProductType) as b on a.GoodsID = b.GoodsID  "+
                            " where b.GoodsID is null or a.Redices > b.RealCount";

            DataTable tempTable = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (tempTable != null && tempTable.Rows.Count > 0)
            {
                IAssignBillNoServer serverAssginBillNo = ServerModuleFactory.GetServerModule<IAssignBillNoServer>();

                foreach (DataRow dr in tempTable.Rows)
                {
                    S_MaterialRequisition newMaterialRequisition = new S_MaterialRequisition();

                    newMaterialRequisition.AssociatedBillNo = materialRequisition.Bill_ID;
                    newMaterialRequisition.AssociatedBillType = CE_BillTypeEnum.领料单.ToString();
                    newMaterialRequisition.AuthorizeDate = materialRequisition.AuthorizeDate;
                    newMaterialRequisition.AuthorizePersonnel = materialRequisition.AuthorizePersonnel;
                    newMaterialRequisition.Bill_ID = serverAssginBillNo.AssignNewNo(this, CE_BillTypeEnum.领料单.ToString());
                    newMaterialRequisition.Bill_Time = ServerTime.Time;
                    newMaterialRequisition.BillStatus = MaterialRequisitionBillStatus.等待出库.ToString();
                    newMaterialRequisition.Department = materialRequisition.Department;
                    newMaterialRequisition.DepartmentDirector = materialRequisition.DepartmentDirector;
                    newMaterialRequisition.FetchCount = 0;
                    newMaterialRequisition.FetchType = FetchGoodsType.零星领料.ToString();
                    newMaterialRequisition.FillInPersonnel = materialRequisition.FillInPersonnel;
                    newMaterialRequisition.FillInPersonnelCode = materialRequisition.FillInPersonnelCode;
                    newMaterialRequisition.ProductType = "";
                    newMaterialRequisition.PurposeCode = materialRequisition.PurposeCode;
                    newMaterialRequisition.Remark = materialRequisition.Remark;
                    newMaterialRequisition.StorageID = materialRequisition.StorageID;

                    ctx.S_MaterialRequisition.InsertOnSubmit(newMaterialRequisition);

                    S_MaterialRequisitionGoods newGoodsInfo = new S_MaterialRequisitionGoods();

                    newGoodsInfo.BasicCount = Convert.ToDecimal(dr["Redices"]);
                    newGoodsInfo.BatchNo = "";
                    newGoodsInfo.Bill_ID = newMaterialRequisition.Bill_ID;
                    newGoodsInfo.GoodsID = Convert.ToInt32(dr["GoodsID"]);
                    newGoodsInfo.ProviderCode = "";
                    newGoodsInfo.RealCount = 0;
                    newGoodsInfo.Remark = materialRequisition.Remark;
                    newGoodsInfo.RequestCount = Convert.ToDecimal(dr["Redices"]);
                    newGoodsInfo.ShowPosition = 0;

                    ctx.S_MaterialRequisitionGoods.InsertOnSubmit(newGoodsInfo);

                    m_billMessageServer.DestroyMessage(newMaterialRequisition.Bill_ID);
                    m_billMessageServer.SendNewFlowMessage(newMaterialRequisition.Bill_ID, 
                        string.Format("{0} 号领料单已成功由主管审核，请仓管尽快核实领料单后出库", newMaterialRequisition.Bill_ID), 
                        BillFlowMessage_ReceivedUserType.角色,
                        m_billMessageServer.GetRoleStringForStorage(newMaterialRequisition.StorageID).ToString());

                }
            }
        }

        /// <summary>
        /// 完成领料单(系统自动生成领料单时调用)
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">单据号</param>
        /// <param name="storeManager">仓库管理员</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加领料单</returns>
        public bool FinishBill(DepotManagementDataContext ctx, string billNo, string storeManager, out string error)
        {
            try
            {
                error = null;

                var result = from r in ctx.S_MaterialRequisition where r.Bill_ID == billNo select r;

                if (result.Count() == 0)
                {
                    error = string.Format("没有找到单据号为 {0} 的领料单信息，无法进行此操作", billNo);
                    return false;
                }

                S_MaterialRequisition fetchBill = result.Single();

                fetchBill.DepotManager = storeManager;
                fetchBill.BillStatus = MaterialRequisitionBillStatus.已出库.ToString();

                // 添加出库明细表记录
                IMaterialRequisitionGoodsServer goodsServer = ServerModuleFactory.GetServerModule<IMaterialRequisitionGoodsServer>();

                //操作账务信息与库存信息
                OpertaionDetailAndStock(ctx, fetchBill);

                // 正式使用单据号
                m_assignBill.UseBillNo(ctx, "领料单", fetchBill.Bill_ID);
                return true;
            }
            catch (Exception err)
            {
                error = err.Message;
                return false;
            }
        }

        /// <summary>
        /// 根据单据信息操作账务信息与库存信息
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="bill">单据信息</param>
        public void OpertaionDetailAndStock(DepotManagementDataContext dataContext, S_MaterialRequisition bill)
        {
            MaterialRequisitionGoodsServer goodsService = new MaterialRequisitionGoodsServer();
            IFinancialDetailManagement serverDetail =
                ServerModule.ServerModuleFactory.GetServerModule<IFinancialDetailManagement>();

            var result = from r in dataContext.S_MaterialRequisitionGoods
                         where r.Bill_ID == bill.Bill_ID
                         select r;

            if (result == null || result.Count() == 0)
            {
                throw new Exception("获取单据信息失败");
            }

            foreach (var item in result)
            {
                S_FetchGoodsDetailBill detailInfo = goodsService.AssignDetailInfo(dataContext, bill, item);

                S_Stock stockInfo = goodsService.AssignStockInfo(dataContext, bill, item);

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
        /// <param name="returnBill">返回回退后查询到的数据集</param>
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
                var varData = from a in ctx.S_MaterialRequisition
                              where a.Bill_ID == djh
                              select a;

                string strMsg = "";

                if (varData.Count() == 1)
                {
                    S_MaterialRequisition lnqMRequ = varData.Single();

                    switch (billStatus)
                    {
                        case "新建单据":

                            strMsg = string.Format("{0}号领料单已回退，请您重新处理单据; 回退原因为" + rebackReason, djh);
                            m_billMessageServer.PassFlowMessage(djh, strMsg,
                                lnqMRequ.FillInPersonnelCode, false);

                            lnqMRequ.BillStatus = "新建单据";
                            lnqMRequ.DepartmentDirector = null;

                            break;

                        case "等待主管审核":

                            strMsg = string.Format("{0}号领料单已回退，请您重新处理单据; 回退原因为" + rebackReason, djh);
                            m_billMessageServer.PassFlowMessage(djh, strMsg,
                                UniversalFunction.GetPersonnelCode(lnqMRequ.DepartmentDirector), false);

                            lnqMRequ.BillStatus = "等待主管审核";
                            lnqMRequ.DepartmentDirector = null;

                            break;

                        default:
                            break;
                    }

                    ctx.SubmitChanges();
                }
                else
                {
                    error = "数据不唯一或者为空";
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

        /// <summary>
        /// 查询未领用的报废物品
        /// </summary>
        /// <param name="loginName">登录人姓名</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回查询到的数据集</returns>
        public DataTable GetScrapGoods(string loginName, out string error)
        {
            error = null;

            Hashtable paramTable = new Hashtable();
            paramTable.Add("@PersonnelName", loginName);

            string strProductName = "Meterial_For_Scrap";

            DataSet ds = new DataSet();
            Dictionary<OperateCMD, object> dicOperateCMD = m_dbOperate.RunProc_CMD(strProductName, ds, paramTable);

            if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
            {
                error = Convert.ToString(dicOperateCMD[OperateCMD.Return_Errmsg]);

                if (error != "没有找到任何数据")
                    return null;
            }

            return ds.Tables[0];
        }

        /// <summary>
        /// 检查并导出版次号不符的物品
        /// </summary>
        /// <param name="billID">领料单号</param>
        /// <returns>返回检测出的结果集</returns>
        public DataTable CheckGoodsVersion(string billID)
        {
            string strSql = "select * from ( select distinct d.图号型号," +
                " d.物品名称,d.规格,c.BatchNo as 批次号, " +
                " case when c.Version is null then '' else c.Version end as 物品的版次号,  " +
                " case when e.Version is null then '' else e.Version end as Bom表的版次号, '' as 第一台总成编号  " +
                " from S_MaterialRequisition as a" +
                " inner join S_MaterialRequisitionGoods as b on a.Bill_ID = b.Bill_ID" +
                " inner join S_Stock as c on b.GoodsID = c.GoodsID " +
                " and b.BatchNo = c.BatchNo and a.StorageID = c.StorageID" +
                " inner join View_F_GoodsPlanCost as d on c.GoodsID = d.序号" +
                " inner join View_P_ProductBomImitate as e on d.图号型号 = e.PartCode and d.物品名称 = e.PartName" +
                " and d.规格 = e.Spec where a.Bill_ID = '" + billID + "' ) as a " +
                " where 物品的版次号 <> Bom表的版次号 ";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dt;
        }

        /// <summary>
        /// 获得部门每月领料上限的按台为单位的基数
        /// </summary>
        /// <returns>返回部门的领料上限数据集</returns>
        public DataTable GetManufacturingConsumeTable()
        {
            string strSql = "select * from View_S_ManufacturingConsume ";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dt;
        }

        /// <summary>
        /// 获得部门真实的每月领料上限
        /// </summary>
        /// <returns>返回部门真实的每月领料上限</returns>
        public DataTable GetDeptPickingToplimitTable()
        {
            string strSql = "select * from View_S_DeptPickingToplimit ";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dt;
        }

        public DataTable GetKievMaterialInfo()
        {
            string error = "";
            return GlobalObject.DatabaseServer.QueryInfoPro("Report_KievMaterialInfo", new Hashtable(), out error);
        }

        /// <summary>
        /// 分管领导批准
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="returnBill">返回更新后所查询的数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool AuthorizBill(string billID, out IQueryResult returnBill, out string error)
        {
            error = null;

            returnBill = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.S_MaterialRequisition
                              where a.Bill_ID == billID
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数据为空或者不唯一";
                    return false;
                }
                else
                {
                    S_MaterialRequisition lnqMater = varData.Single();
                    lnqMater.BillStatus = MaterialRequisitionBillStatus.等待出库.ToString();
                    lnqMater.AuthorizePersonnel = BasicInfo.LoginName;
                    lnqMater.AuthorizeDate = ServerTime.Time;
                }

                dataContext.SubmitChanges();

                return GetAllBill(out returnBill, out error);
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 针对于工装的领料的工艺人员批准
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="returnBill">返回更新后查询得到的结果集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool TechnologistBill(string billID, out IQueryResult returnBill, out string error)
        {
            error = null;

            returnBill = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.S_MaterialRequisition
                              where a.Bill_ID == billID
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数据为空或者不唯一";
                    return false;
                }
                else
                {
                    var varDataGoods = from a in dataContext.S_MaterialRequisitionGoods
                                       where a.Bill_ID == billID
                                       select a;

                    foreach (var item in varDataGoods)
                    {
                        if (UniversalFunction.IsInStandingBook(item.GoodsID, true))
                        {
                            if (!m_serverFrockStandingBook.IsOperationCountMateBillCount(item.GoodsID, item.Bill_ID, Convert.ToDecimal(item.RequestCount)))
                            {
                                error = "领料数量与工装业务编号数不一致，请重新确认数量的一致性";
                                return false;
                            }
                        }
                    }

                    S_MaterialRequisition lnqMater = varData.Single();

                    lnqMater.BillStatus = MaterialRequisitionBillStatus.等待出库.ToString();
                    lnqMater.TechnologistPersonnel = BasicInfo.LoginName;
                    lnqMater.TechnologistDate = ServerTime.Time;
                }

                dataContext.SubmitChanges();

                return GetAllBill(out returnBill, out error);
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 检查领料单中物品是否超过限额
        /// </summary>
        /// <param name="billID">领料单号</param>
        /// <returns>TRUE 表示超过，FALSE表示否</returns>
        bool CheckIsOutToplimit(string billID)
        {
            DateTime dtStart = new DateTime();
            DateTime dtEnd = new DateTime();

            ServerTime.GetMonthlyBalance(ServerTime.Time, out dtStart, out dtEnd);

            string strSql = "select * from S_DeptPickingToplimit";
            DataTable dtToplimit = GlobalObject.DatabaseServer.QueryInfo(strSql);

            for (int i = 0; i < dtToplimit.Rows.Count; i++)
            {
                strSql = "select Sum(RequestCount)  from S_MaterialRequisition as a " +
                          "  inner join   S_MaterialRequisitionGoods as b  " +
                          "  on a.Bill_ID = b.Bill_ID where a.Bill_ID = '" + billID
                          + "' and b.GoodsID = " + Convert.ToInt32(dtToplimit.Rows[i]["GoodsID"])
                          + " and a.Department like '%" + dtToplimit.Rows[i]["DeptCode"].ToString()
                          + "%'";

                DataTable dtBill = GlobalObject.DatabaseServer.QueryInfo(strSql);

                if (dtBill != null && dtBill.Rows.Count != 0 && dtBill.Rows[0][0].ToString() != "")
                {

                    decimal dcBill = Convert.ToDecimal(dtBill.Rows[0][0]);
                    decimal dcSum = 0;

                    strSql = "select Sum(RealCount) from S_MaterialRequisition as a inner join  " +
                        " S_MaterialRequisitionGoods as b on a.Bill_ID = b.Bill_ID  " +
                        " where a.OutDepotDate >= '" + dtStart + "' and a.OutDepotDate <= '" + dtEnd + "' " +
                        " and a.BillStatus = '已出库' and  GoodsID = "
                        + Convert.ToInt32(dtToplimit.Rows[i]["GoodsID"])
                        + " and Department like '%" + dtToplimit.Rows[i]["DeptCode"].ToString() + "%'";

                    DataTable dtSum = GlobalObject.DatabaseServer.QueryInfo(strSql);

                    if (dtSum != null && dtSum.Rows.Count != 0 && dtSum.Rows[0][0].ToString() != "")
                    {
                        dcSum = Convert.ToDecimal(dtSum.Rows[0][0]);
                    }

                    if (dcSum + dcBill > Convert.ToDecimal(dtToplimit.Rows[i]["ToplimitCount"]))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 自动生成消耗定额
        /// </summary>
        /// <param name="count">每月的台套数</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>生成成功返回True，生成失败返回False</returns>
        public bool AutogenerationPickingToplimit(decimal count, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.S_DeptPickingToplimit
                              select a;

                ctx.S_DeptPickingToplimit.DeleteAllOnSubmit(varData);



                var varData_Manufact = from a in ctx.S_ManufacturingConsume
                                       select a;

                foreach (var item in varData_Manufact)
                {
                    S_DeptPickingToplimit lnqPick = new S_DeptPickingToplimit();

                    lnqPick.DeptCode = item.DeptCode.ToString();
                    lnqPick.GoodsID = Convert.ToInt32(item.GoodsID);
                    lnqPick.ToplimitCount = Convert.ToDecimal(item.BaseValue) * count;

                    ctx.S_DeptPickingToplimit.InsertOnSubmit(lnqPick);
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

        /// <summary>
        /// 保存领料上限表
        /// </summary>
        /// <param name="insertData">被保存的数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>保存成功返回True，保存失败返回False</returns>
        public bool SaveDeptPickingToplimit(DataTable insertData, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.S_ManufacturingConsume
                              select a;

                ctx.S_ManufacturingConsume.DeleteAllOnSubmit(varData);

                for (int i = 0; i < insertData.Rows.Count; i++)
                {
                    S_ManufacturingConsume lnqManufacturing = new S_ManufacturingConsume();

                    lnqManufacturing.DeptCode = insertData.Rows[i]["部门编码"].ToString();
                    lnqManufacturing.GoodsID = Convert.ToInt32(insertData.Rows[i]["物品ID"]);
                    lnqManufacturing.BaseValue = Convert.ToDecimal(insertData.Rows[i]["基数"]);

                    ctx.S_ManufacturingConsume.InsertOnSubmit(lnqManufacturing);
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

        /// <summary>
        /// 外部插入领料单
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billInfo">LINQ数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool AutoCreateBill(DepotManagementDataContext ctx, S_MaterialRequisition billInfo, out string error)
        {
            error = null;

            try
            {
                ctx.S_MaterialRequisition.InsertOnSubmit(billInfo);

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 在系统日志中插入一条信息
        /// </summary>
        /// <param name="log">日志数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>插入成功返回True失败返回False</returns>
        public bool InsertSysLog(_Sys_Log log,out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                dataContxt._Sys_Log.InsertOnSubmit(log);
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
        /// 修改用途
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="type">用途</param>
        /// <param name="returnBill">结果集</param>
        /// <param name="error">错误信息</param>
        /// <returns>插入成功返回True失败返回False</returns>
        public bool UpdateBill(string billNo, string type, out IQueryResult returnBill, out string error)
        {
            error = "";
            returnBill = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.S_MaterialRequisition
                             where a.Bill_ID == billNo
                             select a;

                if (result.Count() == 1)
                {
                    S_MaterialRequisition lnq = result.Single();

                    lnq.PurposeCode = type;
                }

                
                dataContxt.SubmitChanges();
                return GetAllBill(out returnBill, out error);
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 获得整台请领单明细列表
        /// </summary>
        /// <param name="billNo">整台请领单号</param>
        /// <returns>返回列表</returns>
        public List<View_Business_WarehouseOutPut_WholeMachineRequisitionDetail> GetWholeMachineRequistionDetail(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.View_Business_WarehouseOutPut_WholeMachineRequisitionDetail
                          where a.单据号 == billNo
                          orderby a.ID
                          select a;

            return varData.ToList();
                          
        }

        /// <summary>
        /// 获得领料汇总表
        /// </summary>
        /// <param name="yearAndMonth">年月</param>
        /// <param name="sheetName">表单名</param>
        /// <returns>返回Table</returns>
        public DataTable GetSummarySheet(string yearAndMonth, string sheetName)
        {
            string error = null;

            try
            {
                DateTime startTime, endTime;
                ServerTime.GetMonthlyBalance(yearAndMonth, out startTime, out endTime);

                Hashtable hsTable = new Hashtable();

                hsTable.Add("@SheetName", sheetName);
                hsTable.Add("@StartTime", startTime);
                hsTable.Add("@EndTime", endTime);

                DataTable result = GlobalObject.DatabaseServer.QueryInfoPro("Report_MaterialRequisition_SummarySheet", hsTable, out error);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 看板领料自动生成
        /// </summary>
        /// <param name="goodsInfo">物料信息</param>
        /// <param name="pickingType">领用类型</param>
        /// <param name="storageID">库房ID</param>
        public void AutoCreateBoardPicking(List<S_MaterialRequisitionGoods> goodsInfo, string pickingType, string storageID)
        {
            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            dataContext.Connection.Open();
            dataContext.Transaction = dataContext.Connection.BeginTransaction();

            try
            {
                if (goodsInfo == null && goodsInfo.Count == 0)
                {
                    throw new Exception("无物品信息");
                }

                if (pickingType.Trim().Length == 0)
                {
                    throw new Exception("无领料类型");
                }

                S_MaterialRequisition billInfo = new S_MaterialRequisition();

                string billNo = m_assignBill.AssignNewNo(this, CE_BillTypeEnum.领料单.ToString());

                billInfo.Bill_ID = billNo;
                billInfo.Bill_Time = ServerTime.Time;
                billInfo.BillStatus = "等待出库";
                billInfo.Department = "ZZ01";
                billInfo.FetchCount = 0;
                billInfo.AssociatedBillNo = "";
                billInfo.AssociatedBillType = "";
                billInfo.FetchType = pickingType == FetchGoodsType.后补充.ToString() ? pickingType : FetchGoodsType.整台领料.ToString();

                string roleCode = PlatformManagement.PlatformFactory.GetRoleManagement().GetRoleViewFromRoleName(
                    new List<string>(new string[] { CE_RoleEnum.装配车间管理员.ToString() }))[0].角色编码;

                View_Auth_User userInfo = 
                    PlatformManagement.PlatformFactory.GetUserManagement().GetUsers(roleCode).OrderByDescending(k => k.登录名).First();

                billInfo.FillInPersonnel = userInfo.姓名;
                billInfo.FillInPersonnelCode = userInfo.登录名;
                billInfo.ProductType = "";

                IMaterialRequisitionPurposeServer servicePurpose = ServerModule.ServerModuleFactory.GetServerModule<IMaterialRequisitionPurposeServer>();

                billInfo.PurposeCode = servicePurpose.GetBillPurpose(pickingType).Code;
                billInfo.Remark = "【看板领料自动生成】" + goodsInfo.OrderByDescending(k => k.Remark).First().Remark;

                BASE_Storage storageInfo = UniversalFunction.GetStorageInfo(storageID);

                billInfo.StorageID = storageInfo.StorageID;

                dataContext.S_MaterialRequisition.InsertOnSubmit(billInfo);

                var varData = from a in goodsInfo
                              group a by a.GoodsID into k
                              select new { GoodsID = k.Key, RequestCount = k.Sum(p => p.RequestCount) };

                List<S_MaterialRequisitionGoods> lstGoodsInfo = new List<S_MaterialRequisitionGoods>();

                IStoreServer storeServer = ServerModuleFactory.GetServerModule<IStoreServer>();

                foreach (var item in varData)
                {
                    // 请领数
                    decimal requestAmount = item.RequestCount;
                    decimal requestTempCount = requestAmount;

                    List<View_S_Stock> lstStock = storeServer.GetGoodsStoreOnlyForAssembly(item.GoodsID, storageInfo.StorageID).ToList();

                    double showPositon = -0.01;

                    if (lstStock.Count == 0)
                    {
                        S_MaterialRequisitionGoods goods = new S_MaterialRequisitionGoods();

                        goods.Bill_ID = billNo;
                        goods.BasicCount = item.RequestCount;
                        goods.RealCount = 0;
                        goods.GoodsID = item.GoodsID;
                        goods.BatchNo = "";
                        goods.ProviderCode = "";
                        goods.Remark = "";

                        lstGoodsInfo.Add(goods);
                    }
                    else
                    {
                        int stockIndex = 0;

                        foreach (var stockItem in lstStock)
                        {
                            if (stockItem.库存数量 == 0)
                            {
                                continue;
                            }

                            stockIndex++;
                            showPositon += 0.01;

                            S_MaterialRequisitionGoods goods = new S_MaterialRequisitionGoods();

                            goods.GoodsID = stockItem.物品ID;
                            goods.Bill_ID = billNo;

                            if (stockIndex == 1)
                            {
                                goods.BasicCount = item.RequestCount;
                            }
                            else
                            {
                                goods.BasicCount = 0;
                            }

                            goods.RealCount = 0;
                            goods.GoodsID = item.GoodsID;
                            goods.ProviderCode = stockItem.供货单位;
                            goods.BatchNo = stockItem.批次号;
                            goods.RealCount = (decimal)(requestTempCount > stockItem.库存数量 ? stockItem.库存数量 : requestTempCount);
                            goods.RequestCount = item.RequestCount;
                            goods.Remark = "";

                            if (stockItem.库存数量 >= requestTempCount)
                            {
                                lstGoodsInfo.Add(goods);
                                break;
                            }
                            else
                            {
                                if (lstStock.Count != 1 && (stockIndex != lstStock.Count))
                                {
                                    requestTempCount -= (decimal)stockItem.库存数量;
                                }

                                lstGoodsInfo.Add(goods);
                            }
                        }
                    }
                }

                dataContext.S_MaterialRequisitionGoods.InsertAllOnSubmit(lstGoodsInfo);
                dataContext.SubmitChanges();

                dataContext.Transaction.Commit();
            }
            catch (Exception ex)
            {
                dataContext.Transaction.Rollback();
                throw new Exception(ex.Message);
            }

        }

        public void AddBoardIssue(S_MaterialRequisition_BoardIssue issue)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            ctx.S_MaterialRequisition_BoardIssue.InsertOnSubmit(issue);
            ctx.SubmitChanges();
        }

        public void DeleteBoardIssue(string issueType)
        {

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.S_MaterialRequisition_BoardIssue
                          where a.IssueType == issueType
                          select a;

            ctx.S_MaterialRequisition_BoardIssue.DeleteAllOnSubmit(varData);
            ctx.SubmitChanges();
        }

        public void DeleteBoardIssue(S_MaterialRequisition_BoardIssue issue)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.S_MaterialRequisition_BoardIssue
                          where a.BarCode == issue.BarCode
                          && a.IssueType == issue.IssueType
                          select a;

            if (varData.Count() > 0)
            {
                ctx.S_MaterialRequisition_BoardIssue.DeleteOnSubmit(varData.First());
                ctx.SubmitChanges();
            }
        }

        public List<S_MaterialRequisition_BoardIssue> GetBoardIssueInfo(string issueType)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            if (issueType == null || issueType.Trim().Length == 0)
            {
                var varData = from a in ctx.S_MaterialRequisition_BoardIssue
                              select a;

                return varData.ToList();
            }
            else
            {
                var varData = from a in ctx.S_MaterialRequisition_BoardIssue
                              where a.IssueType == issueType
                              select a;

                return varData.ToList();
            }
        }
    }
}
