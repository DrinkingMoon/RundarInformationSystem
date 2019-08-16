/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  HomemadePartInDepotBillServer.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2010/09/17
 * 开发平台:  Visual C# 2008
 * 用于    :  仓库管理软件
 *----------------------------------------------------------------------------
 * 描述 : 
 * 其它 :
 *----------------------------------------------------------------------------
 * 公共信息: 参见系统'类帮助文档'
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2010/09/17 8:54:12 作者: 夏石友 当前版本: V1.00
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
    /// 自制件入库单管理类
    /// </summary>
    class HomemadePartInDepotServer :BasicServer, IHomemadePartInDepotServer
    {
        /// <summary>
        /// 获取基础物品服务组件
        /// </summary>
        IBasicGoodsServer m_basicGoodsServer = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();

        /// <summary>
        /// 人员信息服务
        /// </summary>
        IPersonnelInfoServer m_personnelInfoServer = ServerModuleFactory.GetServerModule<IPersonnelInfoServer>();

        /// <summary>
        /// 单据编号分配服务
        /// </summary>
        IAssignBillNoServer m_assignBill = ServerModuleFactory.GetServerModule<IAssignBillNoServer>();

        /// <summary>
        /// 单据唯一码
        /// </summary>
        int m_billUniqueID = -1;

        /// <summary>
        /// 检查某单据是否存在
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(string billNo)
        {
            string sql = "SELECT * FROM [DepotManagement].[dbo].[S_HomemadePartBill] " +
                "where Bill_ID = '" + billNo + "'";

            System.Data.DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 检查某业务是否存在
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">业务号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.S_HomemadePartBill
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
        /// 获得单条记录信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回LINQ单条信息</returns>
        public S_HomemadePartBill GetBill(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.S_HomemadePartBill
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
        /// 获取自制件入库单信息
        /// </summary>
        /// <param name="returnBill">返回的入库单信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取库存信息</returns>
        public bool GetAllBill(out IQueryResult returnBill, out string error)
        {
            returnBill = null;
            error = null;

            IAuthorization authorization = PlatformFactory.GetObject<IAuthorization>();
            IQueryResult qr = null;

            if (QueryResultFilter == null)
            {
                qr = authorization.Query("自制件入库单查询", null);
            }
            else
            {
                qr = authorization.Query("自制件入库单查询", null, QueryResultFilter);
            }

            if (!qr.Succeeded)
            {
                error = qr.Error;
                return false;
            }

            if (m_billUniqueID < 0)
            {
                IBillTypeServer server = ServerModuleFactory.GetServerModule<IBillTypeServer>();
                BASE_BillType billType = server.GetBillTypeFromName("自制件入库单");

                if (billType == null)
                {
                    error = "获取不到单据类型信息";
                    return false;
                }

                m_billUniqueID = billType.UniqueID;
            }

            returnBill = qr;
            return true;
        }

        /// <summary>
        /// 检查自制件入库单中是否存在此物品相关信息
        /// </summary>
        /// <param name="id">物品ID</param>
        /// <returns>存在返回true, 不存在返回false</returns>
        public bool IsExist(int id)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            return (from r in dataContxt.S_HomemadePartBill
                    where r.GoodsID == id
                    select r).Count() > 0;
        }

        /// <summary>
        /// 添加自制件入库单
        /// </summary>
        /// <param name="bill">自制件单据信息</param>
        /// <param name="returnBill">添加完毕后查询数据库的返回结果</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool AddBill(S_HomemadePartBill bill, out IQueryResult returnBill, out string error)
        {
            returnBill = null;
            error = null;

            try
            {

                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                string billID = m_assignBill.AssignNewNo(this, CE_BillTypeEnum.自制件入库单.ToString());

                bill.Bill_ID = billID;
                bill.BatchNo = bill.BatchNo == "系统自动生成" ? billID : bill.BatchNo;
                bill.Bill_Time = ServerModule.ServerTime.Time;

                #region 获取计划价格

                View_F_GoodsPlanCost basicGoods = m_basicGoodsServer.GetGoodsInfoView(bill.GoodsID);

                decimal planUnitPrice = basicGoods.单价;

                bill.PlanUnitPrice = planUnitPrice;
                bill.PlanPrice = bill.DeclareCount * planUnitPrice;
                #endregion

                #region 获取单价

                bill.UnitPrice = 0;
                bill.Price = bill.UnitPrice * bill.DeclareCount;
                bill.TotalPrice = CalculateClass.GetTotalPrice((decimal)bill.Price);

                #endregion

                if (GlobalObject.GeneralFunction.IsNullOrEmpty(bill.Bill_ID))
                {
                    throw new Exception("【单据号】获取失败，请重新再试");
                }

                dataContxt.S_HomemadePartBill.InsertOnSubmit(bill);
                dataContxt.SubmitChanges();

                return GetAllBill(out returnBill, out error);
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 更新自制件入库单
        /// </summary>
        /// <param name="updateBill">更新的自制件单据信息</param>
        /// <param name="returnBill">返回的单据查询结果集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool UpdateBill(S_HomemadePartBill updateBill, out IQueryResult returnBill, out string error)
        {
            returnBill = null;
            error = null;
            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                S_HomemadePartBill bill = (from r in dataContxt.S_HomemadePartBill 
                                           where r.Bill_ID == updateBill.Bill_ID 
                                           select r).Single();

                bill.BatchNo = updateBill.BatchNo;

                bill.Bill_Time = updateBill.Bill_Time;
                bill.BillStatus = updateBill.BillStatus;
                bill.DeclareCount = updateBill.DeclareCount;
                bill.DeclarePersonnel = updateBill.DeclarePersonnel;
                bill.DeclarePersonnelCode = updateBill.DeclarePersonnelCode;
                bill.GoodsID = updateBill.GoodsID;
                bill.Provider = updateBill.Provider;
                bill.ProviderBatchNo = updateBill.ProviderBatchNo;
                bill.RebackReason = "";
                bill.Remark = updateBill.Remark;

                #region 获取计划价格
                decimal planUnitPrice = 0;
                bill.PlanUnitPrice = planUnitPrice;
                bill.PlanPrice = bill.DeclareCount * planUnitPrice;
                #endregion

                #region 获取单价

                bill.UnitPrice = 0;
                bill.Price = bill.UnitPrice * bill.DeclareCount;
                bill.TotalPrice = CalculateClass.GetTotalPrice((decimal)bill.Price);

                #endregion

                dataContxt.SubmitChanges();

                return GetAllBill(out returnBill, out error);
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 确认到货数
        /// </summary>
        /// <param name="billID">单据编号</param>
        /// <param name="confirmAmountSignatory">仓库收货员签名</param>
        /// <param name="goodsAmount">货物数量</param>
        /// <param name="billStatusInfo">单据状态消息</param>
        /// <param name="returnBill">返回的单据查询结果集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool AffirmGoodsAmount(string billID, string confirmAmountSignatory, int goodsAmount, 
            string billStatusInfo, out IQueryResult returnBill, out string error)
        {
            returnBill = null;
            error = null;

            try
            {
                

                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from r in dataContxt.S_HomemadePartBill
                             where r.Bill_ID == billID
                             select r;

                if (result.Count() == 0)
                {
                    error = string.Format("找不到入库单号为 [{0}] 的自制件入库单！", billID);
                    return false;
                }

                S_HomemadePartBill bill = result.Single();

                bill.DepotManagerAffirmCount = goodsAmount;
                bill.BillStatus = billStatusInfo;
                bill.ConfirmAmountSignatory = confirmAmountSignatory;
                bill.RebackReason = "";

                dataContxt.SubmitChanges();

                return GetAllBill(out returnBill, out error);
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 提交质量信息
        /// </summary>
        /// <param name="billID">单据编号</param>
        /// <param name="qualityInfo">质量信息, 只取其中质量部分</param>
        /// <param name="returnBill">返回的单据查询结果集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool SubmitQualityInfo(string billID, S_HomemadePartBill qualityInfo, out IQueryResult returnBill, out string error)
        {
            returnBill = null;
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from r in dataContxt.S_HomemadePartBill 
                             where r.Bill_ID == billID 
                             select r;

                if (result.Count() == 0)
                {
                    error = string.Format("找不到入库单号为 [{0}] 的自制件入库单！", billID);
                    return false;
                }

                S_HomemadePartBill bill = result.Single();

                bill.Checker = qualityInfo.Checker;                 // 检验员
                bill.QualityInputer = qualityInfo.QualityInputer;   // 质量信息输入者
                bill.EligibleCount = qualityInfo.EligibleCount;     // 合格数量
                bill.ConcessionCount = qualityInfo.ConcessionCount; // 让步数
                bill.ReimbursementCount = qualityInfo.ReimbursementCount;           // 退货数
                bill.DeclareWastrelCount = qualityInfo.DeclareWastrelCount;         // 报废数
                bill.QualityInfo = qualityInfo.QualityInfo;                         // 质量信息
                bill.CheckoutReport_ID = qualityInfo.CheckoutReport_ID;             // 检验报告编号
                bill.CheckoutJoinGoods_Time = qualityInfo.CheckoutJoinGoods_Time;   // 检验入库时间
                bill.BillStatus = qualityInfo.BillStatus;                           // 单据状态
                bill.RebackReason = "";
                bill.CheckTime = ServerTime.Time;

                dataContxt.SubmitChanges();

                return GetAllBill(out returnBill, out error);
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 有检测废的物品直接生成领料单
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="inDepotInfo">自制件入库单信息</param>
        /// <param name="mrBillNo">分配的领料单单号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool InsertIntoMaterialRequisition(DepotManagementDataContext ctx, S_HomemadePartBill inDepotInfo, 
            out string mrBillNo, out string error)
        {
            error = null;
            mrBillNo = null;
            string billNo = null;
            MaterialRequisitionServer serverMaterialBill = new MaterialRequisitionServer();

            billNo = m_assignBill.AssignNewNo(serverMaterialBill, CE_BillTypeEnum.领料单.ToString());

            mrBillNo = billNo;

            try
            {
                var varData = from a in ctx.S_MaterialRequisition
                              where a.Bill_ID == billNo
                              select a;

                S_MaterialRequisition lnqMaterial = null;

                if (varData.Count() != 0)
                {
                    error = string.Format("自动生成的报废物品领料单单号 {0} 已被占用，请尝试重新进行此操作，再三出现无法生成可用的单号时与管理员联系", billNo);
                    return false;
                }
                else
                {
                    lnqMaterial = new S_MaterialRequisition();

                    lnqMaterial.Bill_ID = billNo;
                    lnqMaterial.Bill_Time = ServerModule.ServerTime.Time;
                    lnqMaterial.AssociatedBillNo = "";
                    lnqMaterial.AssociatedBillType = "";
                    lnqMaterial.BillStatus = "已出库";
                    lnqMaterial.Department = "ZK01";
                    lnqMaterial.DepartmentDirector = "";
                    lnqMaterial.DepotManager = "";
                    lnqMaterial.FetchCount = 0;
                    lnqMaterial.FetchType = "零星领料";
                    lnqMaterial.FillInPersonnel = inDepotInfo.QualityInputer;
                    lnqMaterial.FillInPersonnelCode = UniversalFunction.GetPersonnelInfo( inDepotInfo.QualityInputer).工号;
                    lnqMaterial.ProductType = "";
                    lnqMaterial.PurposeCode = UniversalFunction.GetPurpose(CE_PickingPurposeProperty.破坏性检测).Code;
                    lnqMaterial.Remark = "因入库零件进行了破坏性检测，由系统自动生成的破坏件领料单，对应单据号：" + inDepotInfo.Bill_ID;
                    lnqMaterial.StorageID = inDepotInfo.StorageID;

                    if (!serverMaterialBill.AutoCreateBill(ctx, lnqMaterial, out error))
                    {
                        return false;
                    }
                    //ctx.S_MaterialRequisition.InsertOnSubmit(lnqMaterial);
                }

                var varDataList = from a in ctx.S_MaterialRequisitionGoods
                                 where a.Bill_ID == billNo
                                 select a;

                if (varDataList.Count() != 0)
                {
                    error = "此单据号已被占用";
                    return false;
                }
                else
                {
                    S_MaterialRequisitionGoods lnqMaterialGoods = new S_MaterialRequisitionGoods();

                    lnqMaterialGoods.Bill_ID = billNo;
                    lnqMaterialGoods.BasicCount = 0;
                    lnqMaterialGoods.BatchNo = inDepotInfo.BatchNo;
                    lnqMaterialGoods.GoodsID = inDepotInfo.GoodsID;
                    lnqMaterialGoods.ProviderCode = inDepotInfo.Provider;
                    lnqMaterialGoods.RealCount = Convert.ToDecimal(inDepotInfo.DeclareWastrelCount);
                    lnqMaterialGoods.Remark = "";
                    lnqMaterialGoods.RequestCount = Convert.ToDecimal(inDepotInfo.DeclareWastrelCount);
                    lnqMaterialGoods.ShowPosition = 1;

                    MaterialRequisitionGoodsServer serverMaterialGoods = new MaterialRequisitionGoodsServer();

                    if (!serverMaterialGoods.AutoCreateGoods(ctx, lnqMaterialGoods, out error))
                    {
                        return false;
                    }
                    //ctx.S_MaterialRequisitionGoods.InsertOnSubmit(lnqMaterialGoods);
                }

                ctx.SubmitChanges();

                if (!serverMaterialBill.FinishBill(ctx, lnqMaterial.Bill_ID, "", out error))
                {
                    throw new Exception(error);
                }

                ctx.SubmitChanges();

                return m_assignBill.UseBillNo(CE_BillTypeEnum.领料单.ToString(), billNo);
            }
            catch (Exception ex)
            {
                serverMaterialBill.DeleteBill(billNo, out error);
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 提交入库信息
        /// </summary>
        /// <param name="billID">单据编号</param>
        /// <param name="inDepotInfo">入库信息, 只取其中入库部分</param>
        /// <param name="returnBill">添加完毕后查询数据库的返回结果</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool SubmitInDepotInfo(string billID, S_HomemadePartBill inDepotInfo, out IQueryResult returnBill, out string error)
        {
            returnBill = null;
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from r in dataContxt.S_HomemadePartBill
                             where r.Bill_ID == billID
                             select r;

                if (result.Count() == 0)
                {
                    error = string.Format("找不到入库单号为 [{0}] 的自制件入库单！", billID);
                    return false;
                }

                S_HomemadePartBill bill = result.Single();

                if (bill.BillStatus == HomemadeBillStatus.已入库.ToString())
                {
                    error = string.Format("入库单号为 [{0}] 单据状态为已入库", billID);
                    return false;
                }

                bill.DepotManager = inDepotInfo.DepotManager;
                bill.ShelfArea = inDepotInfo.ShelfArea;
                bill.ColumnNumber = inDepotInfo.ColumnNumber;
                bill.LayerNumber = inDepotInfo.LayerNumber;
                bill.InDepotCount = inDepotInfo.InDepotCount;
                bill.BillStatus = inDepotInfo.BillStatus;
                bill.RebackReason = "";
                bill.InDepotTime = ServerTime.Time;

                //操作账务信息与库存信息
                OpertaionDetailAndStock(dataContxt, bill);
                dataContxt.SubmitChanges();

                string mrBillNo = "";

                if ((int)bill.DeclareWastrelCount > 0 && bill.InDepotCount > 0)
                {
                    if (!InsertIntoMaterialRequisition(dataContxt, bill, out mrBillNo, out error))
                    {
                        throw new Exception(error);
                    }
                }

                IFrockStandingBook serviceForck = PMS_ServerFactory.GetServerModule<ServerModule.IFrockStandingBook>();
                serviceForck.RecordFrockUseCounts_HomemadePart(dataContxt, bill.GoodsID, bill.InDepotCount);

                dataContxt.SubmitChanges();

                return GetAllBill(out returnBill, out error);
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 赋值库存信息
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="bill">单据信息</param>
        /// <returns>返回库存信息对象</returns>
        S_Stock AssignStockInfo(DepotManagementDataContext dataContext, S_HomemadePartBill bill)
        {
            S_Stock stock = new S_Stock();

            stock.GoodsID = bill.GoodsID;

            stock.StorageID = bill.StorageID;
            stock.ShelfArea = bill.ShelfArea;
            stock.ColumnNumber = bill.ColumnNumber;
            stock.LayerNumber = bill.LayerNumber;
            stock.Provider = bill.Provider;
            stock.ProviderBatchNo = bill.ProviderBatchNo;
            stock.BatchNo = bill.BatchNo;
            stock.ExistCount = bill.InDepotCount;
            stock.UnitPrice = bill.UnitPrice;
            stock.StorageID = bill.StorageID;

            return stock;
        }

        /// <summary>
        /// 删除自制件入库单
        /// </summary>
        /// <param name="billNo">入库单号</param>
        /// <param name="returnBill">返回的单据查询结果集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功删除自制件入库单号</returns>
        public bool DeleteBill(string billNo, out IQueryResult returnBill, out string error)
        {
            returnBill = null;
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                Table<S_HomemadePartBill> table = dataContxt.GetTable<S_HomemadePartBill>();

                var delRow = from c in table 
                             where c.Bill_ID == billNo 
                             select c;

                table.DeleteAllOnSubmit(delRow);

                dataContxt.SubmitChanges();

                return GetAllBill(out returnBill, out error);
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 逐级回退单据
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="rebackReason">回退原因</param>
        /// <param name="returnBill">返回的单据查询结果集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool RebackBill(string billID, string rebackReason, out IQueryResult returnBill, out string error)
        {
            returnBill = null;
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.S_HomemadePartBill 
                         where r.Bill_ID == billID 
                         select r;

            if (result.Count() == 0)
            {
                error = string.Format("找不到编号为 [{0}] 的自制件入库单号", billID);
                return false;
            }

            S_HomemadePartBill bill = result.Single();

            HomemadeBillStatus status = (HomemadeBillStatus)Enum.Parse(typeof(HomemadeBillStatus), bill.BillStatus);

            bool needReback = false;

            if (status == HomemadeBillStatus.等待质检 || status == HomemadeBillStatus.回退_质检信息有误)
            {
                needReback = true;
                bill.BillStatus = HomemadeBillStatus.回退_编制单据有误.ToString();
                bill.CheckoutJoinGoods_Time = null;
                bill.CheckoutReport_ID = "";
                bill.EligibleCount = 0;
                bill.ConcessionCount = 0;
                bill.ReimbursementCount = 0;
                bill.QualityInfo = "";
                bill.Checker = "";
                bill.QualityInputer = "";
            }
            else if (status == HomemadeBillStatus.等待入库)
            {
                needReback = true;
                bill.BillStatus = HomemadeBillStatus.回退_质检信息有误.ToString();
                bill.DepotManagerAffirmCount = 0;
                bill.InDepotCount = 0;
                bill.ShelfArea = "";
                bill.ColumnNumber = "";
                bill.LayerNumber = "";
            }

            if (needReback)
            {
                try
                {
                    bill.RebackReason = rebackReason;
                    dataContxt.SubmitChanges();

                    return GetAllBill(out returnBill, out error);
                }
                catch (Exception exce)
                {
                    error = exce.Message;
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 根据单据信息操作账务信息与库存信息
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="bill">单据信息</param>
        public void OpertaionDetailAndStock(DepotManagementDataContext dataContext, S_HomemadePartBill bill)
        {
            IFinancialDetailManagement serverDetail =
               ServerModule.ServerModuleFactory.GetServerModule<IFinancialDetailManagement>();

            S_InDepotDetailBill detailInfo = AssignDetailInfo(dataContext, bill);
            S_Stock stockInfo = AssignStockInfo(dataContext, bill);

            if (detailInfo == null || stockInfo == null)
            {
                throw new Exception("获取账务信息或者库存信息失败");
            }

            if (bill.DeclareCount < bill.InDepotCount)
            {
                throw new Exception("入库数不能大于报检数");
            }

            serverDetail.ProcessInDepotDetail(dataContext, detailInfo, stockInfo);
        }

        /// <summary>
        /// 赋值账务信息
        /// </summary>
        /// <param name="dataContxt">数据上下文</param>
        /// <param name="bill">单据信息</param>
        /// <returns>返回账务信息</returns>
        public S_InDepotDetailBill AssignDetailInfo(DepotManagementDataContext dataContxt, S_HomemadePartBill bill)
        {
            S_InDepotDetailBill detailBill = new S_InDepotDetailBill();

            detailBill.ID = Guid.NewGuid();
            detailBill.BillTime = (DateTime)bill.InDepotTime;
            detailBill.FillInPersonnel = bill.DeclarePersonnel;
            detailBill.Department = UniversalFunction.GetPersonnelInfo(dataContxt, bill.DeclarePersonnelCode).部门名称;
            detailBill.FactPrice = Math.Round(Convert.ToDecimal(bill.InDepotCount) * bill.UnitPrice, 2);
            detailBill.FactUnitPrice = bill.UnitPrice;
            detailBill.GoodsID = bill.GoodsID;
            detailBill.BatchNo = bill.BatchNo;
            detailBill.InDepotBillID = bill.Bill_ID;
            detailBill.InDepotCount = bill.InDepotCount;
            detailBill.Price = Math.Round(Convert.ToDecimal(bill.InDepotCount) * bill.UnitPrice, 2);
            detailBill.UnitPrice = bill.UnitPrice;
            detailBill.OperationType = (int)CE_SubsidiaryOperationType.自制件入库;
            detailBill.StorageID = bill.StorageID;
            detailBill.Provider = bill.Provider;
            detailBill.FillInDate = bill.Bill_Time;
            detailBill.AffrimPersonnel = bill.DepotManager;

            return detailBill;
        }

        /// <summary>
        /// 报废单据
        /// </summary>
        /// <param name="billID">要报废的单据号</param>
        /// <param name="returnBill">单据查询结果</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool ScrapBill(string billID, out IQueryResult returnBill, out string error)
        {
            error = null;
            returnBill = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.S_HomemadePartBill
                              where a.Bill_ID == billID
                              select a;

                if (result.Count() != 1)
                {
                    error = "报废单据不唯一";
                    return false;
                }
                else
                {
                    dataContxt.S_HomemadePartBill.DeleteAllOnSubmit(result);
                    dataContxt.SubmitChanges();
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
