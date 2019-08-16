/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  ManeuverServer.cs
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
using ServerModule;
using DBOperate;
using PlatformManagement;
using GlobalObject;

namespace Service_Peripheral_External
{
    /// <summary>
    /// 调运单服务
    /// </summary>
    class ManeuverServer : IManeuverServer
    {
        /// <summary>
        /// 获取产品编码服务组件
        /// </summary>
        IProductCodeServer m_serverProductCode = ServerModule.ServerModuleFactory.GetServerModule<IProductCodeServer>();

        /// <summary>
        /// 基础物品服务组件
        /// </summary>
        IBasicGoodsServer m_serverBasicGoods = ServerModule.ServerModuleFactory.GetServerModule<IBasicGoodsServer>();

        /// <summary>
        /// 领料单服务组件
        /// </summary>
        IMaterialListReturnedInTheDepot m_serverMaterialReturnGoods = ServerModule.ServerModuleFactory.GetServerModule<IMaterialListReturnedInTheDepot>();

        /// <summary>
        /// 领料单服务组件
        /// </summary>
        IMaterialReturnedInTheDepot m_serverMaterialReturn = ServerModule.ServerModuleFactory.GetServerModule<IMaterialReturnedInTheDepot>();

        /// <summary>
        /// 领料单服务组件
        /// </summary>
        IMaterialRequisitionGoodsServer m_serverMaterialGoods = ServerModule.ServerModuleFactory.GetServerModule<IMaterialRequisitionGoodsServer>();

        /// <summary>
        /// 领料单服务组件
        /// </summary>
        IMaterialRequisitionServer m_serverMaterialRequisition = ServerModule.ServerModuleFactory.GetServerModule<IMaterialRequisitionServer>();

        /// <summary>
        /// 库房服务组件
        /// </summary>
        IStoreServer m_serverStore = ServerModule.ServerModuleFactory.GetServerModule<IStoreServer>();

        /// <summary>
        /// 营销业务服务组件
        /// </summary>
        ISellIn m_serverSellIn = ServerModule.ServerModuleFactory.GetServerModule<ISellIn>();

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 标识码服务
        /// </summary>
        IUniqueIdentifier m_serverIdentifier = Service_Peripheral_External.ServerModuleFactory.GetServerModule<IUniqueIdentifier>();

        /// <summary>
        /// 单据编号服务
        /// </summary>
        IAssignBillNoServer m_serverBillNo = ServerModule.ServerModuleFactory.GetServerModule<IAssignBillNoServer>();

        /// <summary>
        /// 业务操作服务
        /// </summary>
        IBusinessOperation m_serverBusiness = Service_Peripheral_External.ServerModuleFactory.GetServerModule<IBusinessOperation>();

        /// <summary>
        /// 检查某业务是否存在
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">业务号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.Out_ManeuverBill
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
            string strSql = "SELECT * FROM [DepotManagement].[dbo].[Out_ManeuverBill] where Bill_ID = '" + billNo + "'";

            System.Data.DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dtTemp != null && dtTemp.Rows.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 获得单据列表
        /// </summary>
        /// <param name="billStatus">单据状态</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>返回Table</returns>
        public DataTable GetAllBillInfo(string billStatus,DateTime startTime,DateTime endTime)
        {
            string strSql = " select * from View_Out_ManeuverBill  where 申请日期 >= '" + startTime + "' and 申请日期 <= '" + endTime + "' ";

            if (billStatus != "全部")
            {
                strSql += " and 单据状态 = '"+ billStatus +"'";
            }

            strSql += " ORDER BY 单据号 DESC";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得单条信息
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <returns>返回LINQ数据集</returns>
        public Out_ManeuverBill GetBillInfo(string billID)
        {
            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            var varData = from a in dataContext.Out_ManeuverBill
                          where a.Bill_ID == billID
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
        /// 获得明细信息
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <returns>返回Table</returns>
        public DataTable GetListInfo(string billID)
        {
            string strSql = "select * from View_Out_ManeuverList where Bill_ID = '" + billID + "'";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 提交申请
        /// </summary>
        /// <param name="maneuverBill">单据信息数据集</param>
        /// <param name="listInfo">明细信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool InsertBill(Out_ManeuverBill maneuverBill, DataTable listInfo, out string error)
        {
            error = null;


            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            try
            {
                dataContext.Connection.Open();
                dataContext.Transaction = dataContext.Connection.BeginTransaction();

                var varData = from a in dataContext.Out_ManeuverBill
                              where a.Bill_ID == maneuverBill.Bill_ID
                              select a;

                if (varData.Count() == 0)
                {
                    maneuverBill.Bill_ID = m_serverBillNo.AssignNewNo(this, "调运单");
                    maneuverBill.BillStatus = "等待主管审核";

                    if (!InsertList(dataContext, maneuverBill.Bill_ID, listInfo, out error))
                    {
                        throw new Exception(error);
                    }

                    dataContext.Out_ManeuverBill.InsertOnSubmit(maneuverBill);

                    m_billMessageServer.DestroyMessage(maneuverBill.Bill_ID);
                    m_billMessageServer.SendNewFlowMessage(maneuverBill.Bill_ID,
                        string.Format("{0}号调运单已提交，请营销主管审核", maneuverBill.Bill_ID), CE_RoleEnum.营销主管);
                }
                else if (varData.Count() == 1)
                {
                    Out_ManeuverBill lnqBill = varData.Single();

                    lnqBill.BillStatus = "等待主管审核";
                    lnqBill.Remark = maneuverBill.Remark;
                    lnqBill.InStorageID = maneuverBill.InStorageID;
                    lnqBill.OutStorageID = maneuverBill.OutStorageID;
                    lnqBill.ScrapBillNo = maneuverBill.ScrapBillNo;

                    if (!DeleteList(dataContext, maneuverBill.Bill_ID, out error))
                    {
                        throw new Exception(error);
                    }

                    if (!InsertList(dataContext, maneuverBill.Bill_ID, listInfo, out error))
                    {
                        throw new Exception(error);
                    }

                    m_billMessageServer.DestroyMessage(maneuverBill.Bill_ID);
                    m_billMessageServer.SendNewFlowMessage(maneuverBill.Bill_ID,
                        string.Format("{0}号调运单已提交，请营销主管审核", maneuverBill.Bill_ID), CE_RoleEnum.营销主管);
                }
                else
                {
                    error = "数据重复"; 
                    throw new Exception(error);
                }

                dataContext.SubmitChanges();

                dataContext.Transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                dataContext.Transaction.Rollback();
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 操作业务
        /// </summary>
        /// <param name="maneuverBill">单据信息数据集</param>
        /// <param name="listInfo">单据明细信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool OperationInfo(Out_ManeuverBill maneuverBill,DataTable listInfo,out string error)
        {
            error = null;

            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            dataContext.Connection.Open();
            dataContext.Transaction = dataContext.Connection.BeginTransaction();

            try
            {

                var varData = from a in dataContext.Out_ManeuverBill
                              where a.Bill_ID == maneuverBill.Bill_ID
                              select a;

                if(varData.Count() == 1)
                {
                    Out_ManeuverBill lnqBill = varData.Single();

                    switch (lnqBill.BillStatus)
                    {
                        case "等待主管审核":

                            lnqBill.BillStatus = "等待出库";
                            lnqBill.Verify = BasicInfo.LoginName;
                            lnqBill.VerifyTime = ServerTime.Time;

                            break;

                        case "等待出库":

                            lnqBill.BillStatus = "等待发货";
                            lnqBill.Shipper = BasicInfo.LoginName;
                            lnqBill.ShipperTime = ServerTime.Time;

                            for (int i = 0; i < listInfo.Rows.Count; i++)
                            {
                                if (listInfo.Rows[i]["发货数量"] == null || 
                                    listInfo.Rows[i]["发货数量"].ToString() == "" || 
                                    Convert.ToDecimal(listInfo.Rows[i]["发货数量"]) == 0)
                                {
                                    listInfo.Rows[i]["发货数量"] = listInfo.Rows[i]["申请数量"];
                                }
                            }

                            if (!DeleteList(dataContext,maneuverBill.Bill_ID,out error))
                            {
                                throw new Exception(error);
                            }

                            if (!InsertList(dataContext, maneuverBill.Bill_ID, listInfo, out error))
                            {
                                throw new Exception(error);
                            }

                            dataContext.SubmitChanges();

                            CheckUniqueIdentifierCode(dataContext, maneuverBill);

                            for (int i = 0; i < listInfo.Rows.Count; i++)
                            {
                                Out_DetailAccount lnqDetail = new Out_DetailAccount();

                                lnqDetail.Bill_ID = lnqBill.Bill_ID;
                                lnqDetail.BillFinishTime = ServerTime.Time;
                                lnqDetail.Confirmor = BasicInfo.LoginName;
                                lnqDetail.GoodsID = Convert.ToInt32(listInfo.Rows[i]["物品ID"]);
                                lnqDetail.OperationCount = -Convert.ToDecimal(listInfo.Rows[i]["发货数量"]);
                                lnqDetail.Proposer = lnqBill.Proposer;
                                lnqDetail.Remark = listInfo.Rows[i]["备注"].ToString();
                                lnqDetail.SecStorageID = lnqBill.OutStorageID;
                                lnqDetail.StorageID = listInfo.Rows[i]["账务库房ID"].ToString();

                                if (!m_serverBusiness.OperationDetailAndStock(dataContext,lnqDetail,out error))
                                {
                                    throw new Exception(error);
                                }
                            }

                            break;

                        case "等待发货":

                            lnqBill.BillStatus = "等待收货";
                            lnqBill.LogisticsBillNo = maneuverBill.LogisticsBillNo;
                            lnqBill.LogisticsName = maneuverBill.LogisticsName;
                            lnqBill.Phone = maneuverBill.Phone;
                            lnqBill.ExcShipper = BasicInfo.LoginName;
                            lnqBill.ExcShipperTime = ServerTime.Time;

                            break;

                        case "等待收货":

                            lnqBill.BillStatus = "等待入库";
                            lnqBill.ExcConfirmor = BasicInfo.LoginName;
                            lnqBill.ExcConfirmorTime = ServerTime.Time;

                            break;

                        case "等待入库":
                            lnqBill.BillStatus = "已完成";
                            lnqBill.Confirmor = BasicInfo.LoginName;
                            lnqBill.ConfirmorTime = ServerTime.Time;

                            for (int i = 0; i < listInfo.Rows.Count; i++)
                            {
                                if (listInfo.Rows[i]["收货数量"] == null || 
                                    listInfo.Rows[i]["收货数量"].ToString() == "" || 
                                    Convert.ToDecimal(listInfo.Rows[i]["收货数量"]) == 0)
                                {
                                    listInfo.Rows[i]["收货数量"] = listInfo.Rows[i]["发货数量"];
                                }
                            }

                            //删除明细
                            if (!DeleteList(dataContext, maneuverBill.Bill_ID, out error))
                            {
                                throw new Exception(error);
                            }

                            //添加明细
                            if (!InsertList(dataContext, maneuverBill.Bill_ID, listInfo, out error))
                            {
                                throw new Exception(error);
                            }

                            dataContext.SubmitChanges();

                            for (int i = 0; i < listInfo.Rows.Count; i++)
                            {
                                Out_DetailAccount lnqDetail = new Out_DetailAccount();

                                lnqDetail.Bill_ID = lnqBill.Bill_ID;
                                lnqDetail.BillFinishTime = ServerTime.Time;
                                lnqDetail.Confirmor = lnqBill.Confirmor;
                                lnqDetail.GoodsID = Convert.ToInt32(listInfo.Rows[i]["物品ID"]);
                                lnqDetail.OperationCount = Convert.ToDecimal(listInfo.Rows[i]["收货数量"]);
                                lnqDetail.Proposer = lnqBill.Proposer;
                                lnqDetail.Remark = listInfo.Rows[i]["备注"].ToString();
                                lnqDetail.SecStorageID = lnqBill.InStorageID;
                                lnqDetail.StorageID = listInfo.Rows[i]["账务库房ID"].ToString();

                                //操作外部业务明细与库存
                                if (!m_serverBusiness.OperationDetailAndStock(dataContext, lnqDetail, out error))
                                {
                                    throw new Exception(error);
                                }
                            }

                            dataContext.SubmitChanges();

                            if (!InsertProductStock(dataContext, lnqBill.Bill_ID,out error))
                            {
                                throw new Exception(error);
                            }

                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    error = "数据重复或者为空";
                    throw new Exception(error);
                }

                dataContext.SubmitChanges();
                dataContext.Transaction.Commit();

                return true;
            }
            catch (Exception ex)
            {
                dataContext.Transaction.Rollback();
                error = ex.Message;
                return false;
            }
        }

        void CheckUniqueIdentifierCode(DepotManagementDataContext ctx, Out_ManeuverBill maneuverBill)
        {
            var varDetail = from a in ctx.Out_ManeuverList
                            where a.Bill_ID == maneuverBill.Bill_ID
                            select a;

            foreach (Out_ManeuverList detail in varDetail)
            {
                object cvt = UniversalFunction.GetGoodsAttributeInfo(ctx, detail.GoodsID, CE_GoodsAttributeName.CVT);
                object tcu = UniversalFunction.GetGoodsAttributeInfo(ctx, detail.GoodsID, CE_GoodsAttributeName.TCU);

                if ((cvt != null && Convert.ToBoolean(cvt)) || (tcu != null && Convert.ToBoolean(tcu)))
                {
                    var varUnique = from a in ctx.Out_UniqueIdentifierData
                                    where a.Bill_ID == maneuverBill.Bill_ID
                                    && a.GoodsID == detail.GoodsID
                                    select a;

                    if (varUnique.Count() != detail.ShipperCount)
                    {
                        throw new Exception(UniversalFunction.GetGoodsMessage(ctx, detail.GoodsID) + "请录入对应【出库数量】的唯一标识码");
                    }
                }
            }
        }

        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool DeleteBill(string billNo,out string error)
        {
            error = null;


            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;
            dataContext.Connection.Open();
            dataContext.Transaction = dataContext.Connection.BeginTransaction();

            try
            {

                var varData = from a in dataContext.Out_ManeuverBill
                              where a.Bill_ID == billNo
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数据为空或者不唯一";
                    return false;
                }
                else
                {
                    Out_ManeuverBill billInfo = varData.Single();

                    foreach (var item in varData)
                    {
                        Out_UniqueIdentifierData lnqIdentifier = new Out_UniqueIdentifierData();

                        lnqIdentifier.Bill_ID = item.Bill_ID;

                        if (!m_serverIdentifier.DeleteIdentifier(dataContext, lnqIdentifier, out error))
                        {
                            return false;
                        }
                    }

                    if (billInfo.AssociatedBillNo != null && billInfo.AssociatedBillNo.Contains("YXTK"))
                    {

                        var varMarketing = from a in dataContext.S_MarketingBill
                                           where a.DJH == varData.Single().AssociatedBillNo
                                           select a;


                        dataContext.S_MarketingBill.DeleteAllOnSubmit(varMarketing);

                        m_billMessageServer.DestroyMessage(billInfo.AssociatedBillNo);
                    }


                    var varAfterService = from a in dataContext.Out_AfterServicePartsApplyBill
                                          where a.Bill_ID == billInfo.AssociatedBillNo
                                          select a;

                    foreach (var item in varAfterService)
                    {
                        item.BillStatus = "新建单据";
                    }

                    var varDetail = from a in dataContext.Out_ManeuverList
                                    where a.Bill_ID == billInfo.Bill_ID
                                    select a;

                    List<Out_ManeuverList> lstList = varDetail.ToList();

                    dataContext.Out_ManeuverBill.DeleteAllOnSubmit(varData);
                    dataContext.SubmitChanges();

                    foreach (Out_ManeuverList detail in lstList)
                    {
                        var varAccount = from a in dataContext.Out_DetailAccount
                                         where a.Bill_ID == billInfo.Bill_ID
                                         && a.GoodsID == detail.GoodsID
                                         && a.SecStorageID == billInfo.OutStorageID
                                         && a.StorageID == detail.StorageID
                                         select a;

                        if (varAccount.Count() == 1)
                        {
                            Out_DetailAccount lnqAccount = varAccount.Single();
                            lnqAccount.OperationCount = -lnqAccount.OperationCount;

                            if (!m_serverBusiness.OperationStock(dataContext, lnqAccount, out error))
                            {
                                throw new Exception(error);
                            }
                        }

                        dataContext.Out_DetailAccount.DeleteAllOnSubmit(varAccount);
                        dataContext.SubmitChanges();
                    }
                }

                dataContext.Transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                dataContext.Transaction.Rollback();
                error = ex.Message;
                return false;
                
            }
        }

        /// <summary>
        /// 删除明细
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="billNo">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool DeleteList(DepotManagementDataContext dataContext,string billNo,out string error)
        {
            error = null;

            try
            {
                var varData = from a in dataContext.Out_ManeuverList
                              where a.Bill_ID == billNo
                              select a;

                dataContext.Out_ManeuverList.DeleteAllOnSubmit(varData);

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
                
            }
        }

        /// <summary>
        /// 插入明细
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="billNo">单据号</param>
        /// <param name="listInfo">明细信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool InsertList(DepotManagementDataContext dataContext, string billNo,DataTable listInfo,out string error)
        {
            error = null;

            try
            {
                for (int i = 0; i < listInfo.Rows.Count; i++)
                {
                    Out_ManeuverList lnqList = new Out_ManeuverList();

                    lnqList.Bill_ID = billNo;
                    lnqList.GoodsID = Convert.ToInt32(listInfo.Rows[i]["物品ID"]);
                    lnqList.ProposerCount = Convert.ToDecimal(listInfo.Rows[i]["申请数量"]);
                    lnqList.StorageID = listInfo.Rows[i]["账务库房ID"].ToString();
                    lnqList.Remark = listInfo.Rows[i]["备注"].ToString();

                    if (listInfo.Rows[i]["发货数量"].ToString() != "" && Convert.ToDecimal(listInfo.Rows[i]["发货数量"]) != 0)
                    {
                        lnqList.ShipperCount = Convert.ToDecimal(listInfo.Rows[i]["发货数量"]);
                    }

                    if (listInfo.Rows[i]["收货数量"].ToString() != "" && Convert.ToDecimal(listInfo.Rows[i]["收货数量"]) != 0)
                    {
                        lnqList.ConfirmorCount = Convert.ToDecimal(listInfo.Rows[i]["收货数量"]);
                    }

                    if (lnqList.ShipperCount == 0)
                    {
                        error = "出库数不能为0";
                        return false;
                    }

                    if (lnqList.ConfirmorCount == 0)
                    {
                        error = "入库数不能为0";
                        return false;
                    }

                    dataContext.Out_ManeuverList.InsertOnSubmit(lnqList);
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
        /// 插入编码库存表
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool InsertProductStock(DepotManagementDataContext ctx, string billNo, out string error)
        {
            error = null;

            try
            {
                var varOutCode = from a in ctx.Out_UniqueIdentifierData
                                 where a.Bill_ID == billNo
                                 select a;

                foreach (Out_UniqueIdentifierData item in varOutCode)
                {
                    var varStockCode = from a in ctx.ProductStock
                                       where a.ProductCode == item.Identifier
                                       && a.StorageID == item.StorageID
                                       && a.GoodsID == item.GoodsID
                                       select a;

                    ProductStock lnqStockCode = new ProductStock();

                    if (varStockCode.Count() == 0)
                    {
                        lnqStockCode.BoxNo = "";
                        lnqStockCode.GoodsID = item.GoodsID;
                        lnqStockCode.IsNatural = true;
                        lnqStockCode.ProductCode = item.Identifier;
                        lnqStockCode.ProductStatus = "调运单调入";
                        lnqStockCode.StorageID = item.StorageID;
                        lnqStockCode.Version = "";

                        ctx.ProductStock.InsertOnSubmit(lnqStockCode);
                    }
                    else if (varStockCode.Count() == 1)
                    {
                        lnqStockCode = varStockCode.Single();

                        lnqStockCode.ProductStatus = "调运单调入";
                        lnqStockCode.StorageID = item.StorageID;
                        lnqStockCode.BoxNo = "";
                        lnqStockCode.Version = "";
                    }
                    else
                    {
                        error = "数据不唯一";
                        return false;
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

    }
}
