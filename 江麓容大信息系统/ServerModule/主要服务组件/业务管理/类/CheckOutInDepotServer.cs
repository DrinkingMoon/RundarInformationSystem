/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  CheckOutInDepotBillServer.cs
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
using WebServerModule;

namespace ServerModule
{
    /// <summary>
    /// 报检入库单管理类
    /// </summary>
    class CheckOutInDepotServer : BasicServer, ICheckOutInDepotServer
    {

        /// <summary>
        /// 单据编号分配服务
        /// </summary>
        IAssignBillNoServer m_assignBill = ServerModuleFactory.GetServerModule<IAssignBillNoServer>();

        /// <summary>
        /// 人员信息服务
        /// </summary>
        IPersonnelInfoServer m_personnelInfoServer = ServerModuleFactory.GetServerModule<IPersonnelInfoServer>();

        /// <summary>
        /// 单据唯一码
        /// </summary>
        int m_billUniqueID = -1;

        /// <summary>
        /// 获取新的入库单号
        /// </summary>
        /// <param name="checkOutGoodsType">报检物品类别</param>
        /// <returns>返回获取到的新的入库单号</returns>
        public string GetNextBillNo(int checkOutGoodsType)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            long maxValue = 1;
            string prefix = "";
            DateTime dt = ServerTime.Time;

            switch (checkOutGoodsType)
            {
                case 1:
                    prefix = "BJD";
                    break;
                case 2:
                    prefix = "HJD";
                    break;
                case 3:
                    prefix = "FJD";
                    break;
            }

            if (dataContxt.S_CheckOutInDepotBill.Count() > 0)
            {
                var result = from c in dataContxt.S_CheckOutInDepotBill
                             where c.CheckOutGoodsType == checkOutGoodsType && c.Bill_ID.Substring(3, 4) == dt.Year.ToString()
                             select c;

                if (result.Count() > 0)
                {
                    maxValue += (from c in result select Convert.ToInt32(c.Bill_ID.Substring(9))).Max();
                }
            }

            return string.Format("{0}{1:D4}{2:D2}{3:D6}", prefix, dt.Year, dt.Month, maxValue);
        }

        /// <summary>
        /// 获取报检入库单信息
        /// </summary>
        /// <returns>返回是否成功获取库存信息</returns>
        public IQueryResult GetAllBill()
        {
            IAuthorization m_authorization = PlatformFactory.GetObject<IAuthorization>();
            IQueryResult qr = null;

            try
            {
                if (QueryResultFilter == null)
                {
                    qr = m_authorization.Query("报检入库单查询", null);
                }
                else
                {
                    qr = m_authorization.Query("报检入库单查询", null, QueryResultFilter);
                }

                if (!qr.Succeeded)
                {
                    throw new Exception(qr.Error);
                }

                if (m_billUniqueID < 0)
                {
                    IBillTypeServer server = ServerModuleFactory.GetServerModule<IBillTypeServer>();
                    BASE_BillType billType = server.GetBillTypeFromName("报检入库单");

                    if (billType == null)
                    {
                        throw new Exception("获取不到单据类型信息");
                    }

                    m_billUniqueID = billType.UniqueID;
                }

                return qr;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取指定物品的单据信息
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <returns>返回获取到的单据信息</returns>
        public IQueryable<View_S_CheckOutInDepotBill> GetBill(string goodsCode, string goodsName, string spec)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            return from r in dataContxt.View_S_CheckOutInDepotBill
                   where r.图号型号 == goodsCode && r.物品名称 == goodsName && r.规格 == spec
                   select r;
        }

        /// <summary>
        /// 由物品ID，批次号获得报检入库单据号
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <returns>返回单据号</returns>
        public string GetBillNo(int goodsID, string batchNo)
        {
            string strSql = "select Bill_ID from S_CheckOutInDepotBill where 1=1 ";

            if (goodsID != 0)
            {
                strSql += " and GoodsID = " + goodsID;
            }

            if (batchNo != null && batchNo != "")
            {
                strSql += " and BatchNo = '" + batchNo + "'";
            }

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dt.Rows.Count == 0)
            {
                return "";
            }
            else
            {
                return dt.Rows[0][0].ToString();
            }
        }

        /// <summary>
        /// 检查报检入库单中是否存在此物品相关信息
        /// </summary>
        /// <param name="id">物品ID</param>
        /// <returns>存在返回true, 不存在返回false</returns>
        public bool IsExist(int id)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            return (from r in dataContxt.S_CheckOutInDepotBill
                    where r.GoodsID == id
                    select r).Count() > 0;
        }

        /// <summary>
        /// 添加报检入库单
        /// </summary>
        /// <param name="bill">单据信息</param>
        /// <param name="error">出错时返回错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        public bool AddBill(S_CheckOutInDepotBill bill, out string error)
        {
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                if (bill.Bill_ID.ToString() == "系统自动生成")
                {
                    string billID = GetNextBillNo(bill.CheckOutGoodsType);

                    bill.Bill_ID = billID;
                    bill.BatchNo = billID;

                    if (UniversalFunction.GetStorageInfo_View(bill.StorageID).零成本控制)
                    {
                        throw new Exception("【零成本】库房，无法通过【报检入库单】入库");
                    }

                    dataContxt.S_CheckOutInDepotBill.InsertOnSubmit(bill);
                }
                else
                {
                    var varData = from a in dataContxt.S_CheckOutInDepotBill
                                  where a.Bill_ID == bill.Bill_ID
                                  select a;

                    if (varData.Count() != 1)
                    {
                        error = "数据不唯一或者为空";
                        return false;
                    }
                    else
                    {
                        S_CheckOutInDepotBill lnqBill = varData.Single();

                        lnqBill.OrderFormNumber = bill.OrderFormNumber;
                        lnqBill.ArriveGoods_Time = bill.ArriveGoods_Time;      // 到货日期
                        lnqBill.Bill_Time = bill.Bill_Time;                       // 报检日期
                        lnqBill.BillStatus = bill.BillStatus;
                        lnqBill.Buyer = bill.Buyer;                       // 采购员签名
                        lnqBill.DeclarePersonnelCode = bill.DeclarePersonnelCode;          // 报检员编码
                        lnqBill.DeclarePersonnel = bill.DeclarePersonnel;            // 报检员签名
                        lnqBill.DeclareCount = bill.DeclareCount;              // 报检数
                        lnqBill.Provider = bill.Provider;                       // 供应商编码
                        lnqBill.ProviderBatchNo = bill.ProviderBatchNo;       // 供应商批次
                        lnqBill.GoodsID = bill.GoodsID;
                        lnqBill.BatchNo = bill.BatchNo;                         // xsy, 没有废除OA前暂用
                        lnqBill.Remark = bill.Remark;
                        lnqBill.CheckOutGoodsType = bill.CheckOutGoodsType;
                        lnqBill.OnlyForRepairFlag = bill.OnlyForRepairFlag;
                        lnqBill.UnitPrice = bill.UnitPrice;
                        lnqBill.Price = bill.Price;
                        lnqBill.PlanUnitPrice = bill.PlanUnitPrice;
                        lnqBill.PlanPrice = bill.PlanPrice;
                        lnqBill.TotalPrice = bill.TotalPrice;
                        lnqBill.StorageID = bill.StorageID;
                        lnqBill.Version = bill.Version;
                        lnqBill.IsExigenceCheck = bill.IsExigenceCheck;
                        lnqBill.InvoicePrice = bill.InvoicePrice;
                        lnqBill.UnitInvoicePrice = bill.UnitInvoicePrice;

                        if (UniversalFunction.GetStorageInfo_View(bill.StorageID).零成本控制)
                        {
                            throw new Exception("【零成本】库房，无法通过【报检入库单】入库");
                        }
                    }
                }

                error = bill.BatchNo;

                if (GlobalObject.GeneralFunction.IsNullOrEmpty(bill.Bill_ID))
                {
                    throw new Exception("【单据号】获取失败，请重新再试");
                }

                dataContxt.SubmitChanges();
                return true;
                    //GetAllBill(out returnBill, out error);
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 采购员更新报检入库单
        /// </summary>
        /// <param name="updateBill">单据信息</param>
        /// <param name="error">出错时返回错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        public bool UpdateBill(S_CheckOutInDepotBill updateBill, out string error)
        {
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                S_CheckOutInDepotBill bill = (from r in dataContxt.S_CheckOutInDepotBill
                                              where r.Bill_ID == updateBill.Bill_ID
                                              select r).Single();

                bill.StorageID = updateBill.StorageID;
                bill.BatchNo = updateBill.BatchNo;
                bill.ArriveGoods_Time = updateBill.ArriveGoods_Time;
                bill.Bill_Time = updateBill.Bill_Time;
                bill.BillStatus = updateBill.BillStatus;
                bill.Buyer = updateBill.Buyer;
                bill.DeclareCount = updateBill.DeclareCount;
                bill.DeclarePersonnel = updateBill.DeclarePersonnel;
                bill.DeclarePersonnelCode = updateBill.DeclarePersonnelCode;
                bill.GoodsID = updateBill.GoodsID;
                bill.OrderFormNumber = updateBill.OrderFormNumber;
                bill.Provider = updateBill.Provider;
                bill.ProviderBatchNo = updateBill.ProviderBatchNo;
                bill.RebackReason = "";
                bill.Remark = updateBill.Remark;
                bill.PlanUnitPrice = updateBill.PlanUnitPrice;
                bill.PlanPrice = updateBill.PlanPrice;
                bill.UnitPrice = updateBill.UnitPrice;
                bill.Price = updateBill.Price;
                bill.TotalPrice = updateBill.TotalPrice;
                bill.PeremptorilyEmit = updateBill.PeremptorilyEmit;    // 紧急放行
                bill.CheckTime = updateBill.CheckTime;
                bill.DepotAffirmanceTime = updateBill.DepotAffirmanceTime;
                bill.InDepotTime = updateBill.InDepotTime;
                bill.IsExigenceCheck = updateBill.IsExigenceCheck;
                bill.OnlyForRepairFlag = updateBill.OnlyForRepairFlag;
                bill.Version = updateBill.Version;

                dataContxt.SubmitChanges();

                return true;
                    //GetAllBill(out returnBill, out error);
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
        /// <param name="billStatusInfo">单据状态信息</param>
        /// <param name="error">出错时返回错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        public bool AffirmGoodsAmount(string billID, string confirmAmountSignatory, int goodsAmount, string billStatusInfo, out string error)
        {
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                var result = from r in dataContxt.S_CheckOutInDepotBill
                             where r.Bill_ID == billID
                             select r;

                if (result.Count() == 0)
                {
                    error = string.Format("找不到入库单号为 [{0}] 的报检入库单！", billID);
                    return false;
                }

                S_CheckOutInDepotBill bill = result.Single();

                bill.DepotManagerAffirmCount = goodsAmount;
                bill.BillStatus = billStatusInfo;
                bill.ConfirmAmountSignatory = confirmAmountSignatory;
                bill.DepotAffirmanceTime = ServerTime.Time;
                bill.RebackReason = "";

                dataContxt.SubmitChanges();

                View_F_GoodsPlanCost goodsInfo = UniversalFunction.GetGoodsInfo(bill.GoodsID);
                string strTemp =
                    GlobalObject.GeneralFunction.ListToString<string>(UniversalFunction.GetApplicableMode_CGBOM(bill.GoodsID), ",");

                List<string> lstText = new List<string>();

                lstText.Add(" 物品标识（part list）");
                lstText.Add("名称 " + goodsInfo.物品名称);
                lstText.Add("图号 " + goodsInfo.图号型号);
                lstText.Add("型号 " + strTemp);
                lstText.Add("批号 " + bill.BatchNo);
                lstText.Add("数量 " + bill.DepotManagerAffirmCount.ToString() + "※供应商 " + bill.Provider);
                lstText.Add("日期 " + ServerTime.Time.ToShortDateString());

                ServerModule.PrintPartBarcode.PrintBarcode_Common(lstText);

                return true;
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
        /// <param name="qualityInfo">质量信息, 只取其中质量部分</param>
        /// <param name="error">出错时返回错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        public bool SubmitQualityInfo(S_CheckOutInDepotBill qualityInfo, out string error)
        {
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                var result = from r in dataContxt.S_CheckOutInDepotBill where r.Bill_ID == qualityInfo.Bill_ID select r;

                if (result.Count() == 0)
                {
                    error = string.Format("找不到入库单号为 [{0}] 的报检入库单！", qualityInfo.Bill_ID);
                    return false;
                }

                S_CheckOutInDepotBill bill = result.Single();

                bill.QualityInputer = qualityInfo.QualityInputer;                   // 质量信息输入者
                bill.EligibleCount = qualityInfo.EligibleCount;                     // 合格数量
                bill.DeclareWastrelCount = qualityInfo.DeclareWastrelCount;         // 报废数量
                bill.ConcessionCount = qualityInfo.ConcessionCount;                 // 让步数
                bill.ReimbursementCount = qualityInfo.ReimbursementCount;           // 退货数
                bill.QualityInfo = qualityInfo.QualityInfo;                         // 质量信息
                bill.BillStatus = qualityInfo.BillStatus;                           // 单据状态
                bill.RebackReason = "";
                bill.CheckTime = ServerTime.Time;
                bill.TFFlag = qualityInfo.TFFlag;                                   // 是否挑返
                bill.PeremptorilyEmit = qualityInfo.PeremptorilyEmit;               // 是否加急

                if (bill.Checker == "")
                {
                    bill.Checker = qualityInfo.Checker;                                 // 检验员        
                    bill.CheckoutReport_ID = qualityInfo.CheckoutReport_ID;             // 检验报告编号
                    bill.CheckoutJoinGoods_Time = ServerTime.Time;                      // 检验入库时间
                }

                dataContxt.SubmitChanges();

                return true;
                    //GetAllBill(out returnBill, out error);
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 提交检验报告
        /// </summary>
        /// <param name="qualityInfo">质量信息, 只取其中质量部分</param>
        /// <param name="error">出错时返回错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        public bool SubmitReportInfo(S_CheckOutInDepotBill qualityInfo, out string error)
        {
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                var result = from r in dataContxt.S_CheckOutInDepotBill where r.Bill_ID == qualityInfo.Bill_ID select r;

                if (result.Count() == 0)
                {
                    error = string.Format("找不到入库单号为 [{0}] 的报检入库单！", qualityInfo.Bill_ID);
                    return false;
                }

                S_CheckOutInDepotBill bill = result.Single();

                bill.Checker = qualityInfo.Checker;                                 // 检验员        
                bill.CheckoutReport_ID = qualityInfo.CheckoutReport_ID;             // 检验报告编号
                bill.CheckoutJoinGoods_Time = ServerTime.Time;                      // 检验入库时间

                dataContxt.SubmitChanges();

                return true;
                    //GetAllBill(out returnBill, out error);
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
        /// <param name="inDepotInfo">报检单信息</param>
        /// <param name="mrBillNo">分配的领料单单号</param>
        /// <param name="error">出错时返回错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        public bool InsertIntoMaterialRequisition(DepotManagementDataContext ctx, S_CheckOutInDepotBill inDepotInfo,
            out string mrBillNo, out string error)
        {
            error = null;
            mrBillNo = null;
            string billNo = null;

            MaterialRequisitionServer serverMaterialBill = new MaterialRequisitionServer();

            try
            {

                billNo = m_assignBill.AssignNewNo(serverMaterialBill, CE_BillTypeEnum.领料单.ToString());
                mrBillNo = billNo;

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
                    DateTime dt = ServerTime.Time;

                    lnqMaterial = new S_MaterialRequisition();

                    lnqMaterial.Bill_ID = billNo;
                    lnqMaterial.Bill_Time = dt;
                    lnqMaterial.AssociatedBillNo = inDepotInfo.Bill_ID;
                    lnqMaterial.AssociatedBillType = CE_BillTypeEnum.报检入库单.ToString();
                    lnqMaterial.BillStatus = "已出库";
                    lnqMaterial.Department = "ZK03";
                    lnqMaterial.DepartmentDirector = "";
                    lnqMaterial.DepotManager = inDepotInfo.DepotManager;
                    lnqMaterial.FetchCount = 0;
                    lnqMaterial.FetchType = "零星领料";
                    lnqMaterial.FillInPersonnel = inDepotInfo.QualityInputer;
                    lnqMaterial.FillInPersonnelCode = m_personnelInfoServer.GetPersonnelInfo(inDepotInfo.QualityInputer).工号;
                    lnqMaterial.ProductType = "";
                    lnqMaterial.PurposeCode = UniversalFunction.GetPurpose(CE_PickingPurposeProperty.破坏性检测).Code;
                    lnqMaterial.Remark = "因入库零件进行了破坏性检测，由系统自动生成的破坏件领料单，对应单据号：" + inDepotInfo.Bill_ID;
                    lnqMaterial.StorageID = inDepotInfo.StorageID;
                    lnqMaterial.OutDepotDate = dt;

                    if (!serverMaterialBill.AutoCreateBill(ctx, lnqMaterial, out error))
                    {
                        return false;
                    }
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
                }

                ctx.SubmitChanges();

                if (!serverMaterialBill.FinishBill(ctx, lnqMaterial.Bill_ID, "", out error))
                {
                    throw new Exception(error);
                }

                ctx.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;

                //if (serverMaterialBill.DeleteBill(billNo, out error))
                //{
                //    error = ex.Message;
                //}

                return false;
            }
        }

        /// <summary>
        /// 提交入库信息
        /// </summary>
        /// <param name="billID">单据编号</param>
        /// <param name="inDepotInfo">入库信息, 只取其中入库部分</param>
        /// <param name="error">出错时返回错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        public bool SubmitInDepotInfo(string billID, S_CheckOutInDepotBill inDepotInfo, out string error)
        {
            error = null;

            string mrBillNo = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            dataContxt.Connection.Open();
            dataContxt.Transaction = dataContxt.Connection.BeginTransaction();

            try
            {
                var result = from r in dataContxt.S_CheckOutInDepotBill where r.Bill_ID == billID select r;

                if (result.Count() == 0)
                {
                    error = string.Format("找不到入库单号为 [{0}] 的报检入库单！", billID);
                    throw new Exception(error);
                }

                S_CheckOutInDepotBill bill = result.Single();

                if (bill.BillStatus == CheckInDepotBillStatus.已入库.ToString())
                {
                    error = string.Format("入库单号为 [{0}] 单据状态为已入库", billID);
                    throw new Exception(error);
                }

                bill.DepotManager = inDepotInfo.DepotManager;
                bill.ShelfArea = inDepotInfo.ShelfArea;
                bill.ColumnNumber = inDepotInfo.ColumnNumber;
                bill.LayerNumber = inDepotInfo.LayerNumber;
                bill.InDepotCount = inDepotInfo.InDepotCount;

                bill.Price = Math.Round(bill.UnitPrice *
                    Convert.ToDecimal(inDepotInfo.InDepotCount), 2);

                bill.PlanPrice = Math.Round(bill.PlanUnitPrice *
                    Convert.ToDecimal(inDepotInfo.InDepotCount), 2);

                bill.BillStatus = inDepotInfo.BillStatus;
                bill.RebackReason = "";
                bill.InDepotTime = ServerTime.Time;

                // 添加信息到入库明细表
                OpertaionDetailAndStock(dataContxt, bill);

                if (bill.BatchNo.Substring(bill.BatchNo.Length - 4, 4) == "Auto")
                {
                    if (!InsertWebData(bill, out error))
                    {
                        throw new Exception(error);
                    }
                }

                dataContxt.SubmitChanges();

                if ((int)bill.DeclareWastrelCount > 0 && bill.InDepotCount > 0)
                {
                    if (!InsertIntoMaterialRequisition(dataContxt, bill, out mrBillNo, out error))
                    {
                        throw new Exception(error);
                    }
                }

                dataContxt.SubmitChanges();
                dataContxt.Transaction.Commit();

                // 检查一下入库明细表
                return true;
                    //GetAllBill(out returnBill, out error);
            }
            catch (Exception exce)
            {
                dataContxt.Transaction.Rollback();

                if (mrBillNo != null)
                {
                    ReturnBill(billID, mrBillNo, out error);
                }

                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 根据单据信息操作账务信息与库存信息
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="bill">单据信息</param>
        public void OpertaionDetailAndStock(DepotManagementDataContext dataContext, S_CheckOutInDepotBill bill)
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
        /// 赋值库存信息
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="bill">单据信息</param>
        /// <returns>返回库存信息对象</returns>
        public S_Stock AssignStockInfo(DepotManagementDataContext dataContext, S_CheckOutInDepotBill bill)
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
            stock.Version = bill.Version;

            if (bill.OnlyForRepairFlag)
            {
                stock.GoodsStatus = 6;
            }

            return stock;
        }

        /// <summary>
        /// 获得WEB数据库的明细ID
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <returns>成功则返回获取到的ID，失败则返回0</returns>
        private int GetWebListID(string billID)
        {
            string strSql = " select c.ID from S_CheckOutInDepotBill as a inner join " +
                " View_F_GoodsPlanCost as b on a.GoodsID = b.序号 " +
                " inner join RundarWebServer.dbo.OF_OrderFormGoods as c " +
                " on a.OrderFormNumber = c.OrderFormNumber and b.图号型号 = c.GoodsCode " +
                " and b.物品名称 = c.GoodsName and b.规格 = c.Spec where a.Bill_ID = '"
                + billID + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dt.Rows.Count == 0)
            {
                return 0;
            }
            else
            {
                return (int)dt.Rows[0][0];
            }
        }

        /// <summary>
        /// 插入WEB 数据库
        /// </summary>
        /// <param name="bill">单据信息</param>
        /// <param name="error">出错时返回错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        public bool InsertWebData(S_CheckOutInDepotBill bill, out string error)
        {
            try
            {
                error = null;

                int intListID = GetWebListID(bill.Bill_ID);

                if (intListID == 0)
                {
                    error = "数据库无记录";
                    return false;
                }

                WebSeverDataContext dataContext = WebServerModule.WebDatabaseParameter.WebDataContext;

                OF_OrderForm_ArrivalInfo lnqArrival = new OF_OrderForm_ArrivalInfo();

                lnqArrival.ActualDate = bill.ArriveGoods_Time;
                lnqArrival.ActuaQuantity = Convert.ToDecimal(bill.DepotManagerAffirmCount);
                lnqArrival.ConcessionQuantity = Convert.ToDecimal(bill.ConcessionCount);
                lnqArrival.DestructQuantity = Convert.ToDecimal(bill.DeclareWastrelCount);
                lnqArrival.EligibilityQuantity = Convert.ToDecimal(bill.EligibleCount);
                lnqArrival.ListID = intListID;
                lnqArrival.QualityInfo = bill.QualityInfo;
                lnqArrival.RejectQuantity = Convert.ToDecimal(bill.ReimbursementCount);
                lnqArrival.BatchNo = bill.BatchNo;
                lnqArrival.InBillID = bill.Bill_ID;

                dataContext.OF_OrderForm_ArrivalInfo.InsertOnSubmit(lnqArrival);

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
        /// 回退单据
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="lldID">领料单单号</param>
        /// <param name="error">出错时返回错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        private bool ReturnBill(string billID, string lldID, out string error)
        {
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                int intGoodsID = 0;
                string strBatchNo = "";
                decimal dcCount = 0;
                decimal dcWastrel = 0;

                //报检单状态修改
                var varData_A = from a in dataContxt.S_CheckOutInDepotBill
                                where a.Bill_ID == billID
                                select a;

                if (varData_A.Count() == 1)
                {
                    S_CheckOutInDepotBill lnqCheck = varData_A.Single();

                    intGoodsID = Convert.ToInt32(lnqCheck.GoodsID);
                    strBatchNo = lnqCheck.BatchNo.ToString();
                    dcCount = Convert.ToDecimal(lnqCheck.EligibleCount);
                    dcWastrel = Convert.ToDecimal(lnqCheck.DeclareWastrelCount);
                    lnqCheck.BillStatus = "等待入库";
                }

                //库存记录的删除/修改
                var varData_B = from a in dataContxt.S_Stock
                                where a.GoodsID == intGoodsID
                                && a.BatchNo == strBatchNo
                                select a;

                if (varData_B.Count() == 1)
                {
                    S_Stock lnqStock = varData_B.Single();

                    if (Convert.ToDecimal(lnqStock.ExistCount) == dcCount)
                    {
                        dataContxt.S_Stock.DeleteOnSubmit(lnqStock);
                    }
                    else
                    {
                        lnqStock.ExistCount = Convert.ToDecimal(lnqStock.ExistCount) - dcCount;
                    }
                }

                //入库明细表记录删除
                var varData_C = from a in dataContxt.S_InDepotDetailBill
                                where a.InDepotBillID == billID
                                && a.BatchNo == strBatchNo
                                && a.GoodsID == intGoodsID
                                select a;

                if (varData_C.Count() == 1)
                {
                    dataContxt.S_InDepotDetailBill.DeleteOnSubmit(varData_C.Single());
                }

                //领料单主表删除
                var varData_D = from a in dataContxt.S_MaterialRequisition
                                where a.Bill_ID == lldID
                                && a.PurposeCode == UniversalFunction.GetPurpose(CE_PickingPurposeProperty.破坏性检测).Code
                                select a;

                if (varData_D.Count() == 1)
                {
                    dataContxt.S_MaterialRequisition.DeleteOnSubmit(varData_D.Single());
                }

                //领料单此表删除
                var varData_E = from a in dataContxt.S_MaterialRequisitionGoods
                                where a.Bill_ID == lldID
                                && a.GoodsID == intGoodsID
                                && a.BatchNo == strBatchNo
                                select a;

                if (varData_E.Count() == 1)
                {
                    dataContxt.S_MaterialRequisitionGoods.DeleteOnSubmit(varData_E.Single());
                }

                //出库明细表记录删除
                var varData_F = from a in dataContxt.S_FetchGoodsDetailBill
                                where a.FetchBIllID == lldID
                                && a.BatchNo == strBatchNo
                                && a.GoodsID == intGoodsID
                                select a;

                if (varData_F.Count() == 1)
                {
                    dataContxt.S_FetchGoodsDetailBill.DeleteOnSubmit(varData_F.Single());
                }

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
        /// 删除报检入库单
        /// </summary>
        /// <param name="billNo">入库单号</param>
        /// <param name="error">出错时返回错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        public bool DeleteBill(string billNo, out string error)
        {
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                Table<S_CheckOutInDepotBill> table = dataContxt.GetTable<S_CheckOutInDepotBill>();
                var delRow = from c in table where c.Bill_ID == billNo select c;

                table.DeleteAllOnSubmit(delRow);

                dataContxt.SubmitChanges();

                return true;
                    //GetAllBill(out returnBill, out error);
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
        /// <param name="intStatusFlag">0：回退_质检电信息有误；1：回退_质检机信息有误</param>
        /// <param name="error">出错时返回错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        public bool RebackBill(string billID, string rebackReason, int intStatusFlag, out string error)
        {
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.S_CheckOutInDepotBill
                         where r.Bill_ID == billID
                         select r;

            if (result.Count() == 0)
            {
                error = string.Format("找不到编号为 [{0}] 的报检入库单号", billID);
                return false;
            }

            S_CheckOutInDepotBill bill = result.Single();

            CheckInDepotBillStatus status = (CheckInDepotBillStatus)Enum.Parse(typeof(CheckInDepotBillStatus), bill.BillStatus);

            bool needReback = false;

            if (status == CheckInDepotBillStatus.等待确认到货数 || status == CheckInDepotBillStatus.回退_确认到货有误)
            {
                needReback = true;

                bill.BillStatus = CheckInDepotBillStatus.回退_采购单据有误.ToString();
                bill.DepotManagerAffirmCount = 0;
                bill.DepotManager = "";
            }
            else if (status == CheckInDepotBillStatus.等待质检机检验
                || status == CheckInDepotBillStatus.等待质检电检验
                || status == CheckInDepotBillStatus.回退_质检电信息有误
                || status == CheckInDepotBillStatus.回退_质检机信息有误)
            {
                needReback = true;

                bill.BillStatus = CheckInDepotBillStatus.回退_确认到货有误.ToString();
                bill.CheckoutJoinGoods_Time = null;
                bill.CheckoutReport_ID = "";
                bill.EligibleCount = 0;
                bill.ConcessionCount = 0;
                bill.ReimbursementCount = 0;
                bill.DeclareWastrelCount = 0;
                bill.QualityInfo = "";
                bill.Checker = "";
                bill.QualityInputer = "";
            }
            else if (status == CheckInDepotBillStatus.等待入库)
            {
                needReback = true;

                if (intStatusFlag == 0)
                {
                    bill.BillStatus = CheckInDepotBillStatus.回退_质检电信息有误.ToString();
                }
                else
                {
                    bill.BillStatus = CheckInDepotBillStatus.回退_质检机信息有误.ToString();
                }

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

                    return true;
                        //GetAllBill(out returnBill, out error);
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
        /// 赋值账务信息
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="bill">单据信息</param>
        /// <returns>返回账务信息对象</returns>
        public S_InDepotDetailBill AssignDetailInfo(DepotManagementDataContext dataContext, S_CheckOutInDepotBill bill)
        {
            View_HR_Personnel personnel = UniversalFunction.GetPersonnelInfo(dataContext, bill.DeclarePersonnelCode);

            S_InDepotDetailBill detailBill = new S_InDepotDetailBill();

            detailBill.ID = Guid.NewGuid();
            detailBill.BillTime = (DateTime)bill.InDepotTime;
            detailBill.FillInPersonnel = bill.DeclarePersonnel;
            detailBill.Department = personnel.部门名称;
            detailBill.FactPrice = Math.Round(Convert.ToDecimal(bill.InDepotCount) * bill.UnitPrice, 2);
            detailBill.FactUnitPrice = bill.UnitPrice;
            detailBill.GoodsID = bill.GoodsID;
            detailBill.BatchNo = bill.BatchNo;
            detailBill.InDepotBillID = bill.Bill_ID;
            detailBill.InDepotCount = bill.InDepotCount;
            detailBill.Price = Math.Round(Convert.ToDecimal(bill.InDepotCount) * bill.UnitPrice, 2);
            detailBill.UnitPrice = bill.UnitPrice;
            detailBill.OperationType = (int)CE_SubsidiaryOperationType.报检入库;
            detailBill.Provider = bill.Provider;
            detailBill.StorageID = bill.StorageID;
            detailBill.FillInDate = bill.Bill_Time;
            detailBill.AffrimPersonnel = bill.DepotManager;

            return detailBill;
        }

        /// <summary>
        /// 报废单据
        /// </summary>
        /// <param name="billID">要报废的单据编号</param>
        /// <param name="error">出错时返回错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        public bool ScrapBill(string billID, out string error)
        {
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                var datevar = from a in dataContxt.S_CheckOutInDepotBill
                              where a.Bill_ID == billID
                              select a;

                if (datevar.Count() != 1)
                {
                    error = "报废单据不唯一";
                    return false;
                }
                else
                {
                    S_CheckOutInDepotBill lnqCheck = datevar.Single();

                    lnqCheck.BillStatus = "已报废";

                    dataContxt.SubmitChanges();
                }

                return true;
                    //GetAllBill(out returnBill, out error);
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 更新退货建议数据
        /// </summary>
        /// <param name="djh">报检单单号</param>
        /// <param name="rejectMode">退货建议</param>
        /// <param name="error">出错时返回错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        public bool UpdateRejectMode(string djh, string rejectMode, out string error)
        {
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                var varData = from a in dataContxt.S_CheckOutInDepotBill
                              where a.Bill_ID == djh
                              select a;

                if (varData.Count() == 1)
                {
                    S_CheckOutInDepotBill lnqCheck = varData.Single();

                    lnqCheck.RejectMode = rejectMode;
                    lnqCheck.SQETHRQ = ServerTime.Time;
                    lnqCheck.SQETHRY = BasicInfo.LoginName;
                }

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
        /// 获得退货建议方式
        /// </summary>
        /// <param name="djh">报检单单号</param>
        /// <returns>成功则返回获取到的退货建议，失败返回空串</returns>
        public string GetRejectMode(string djh)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var result = from r in ctx.S_CheckOutInDepotBill
                         where r.Bill_ID == djh
                         select r;

            if (result.Count() == 0)
            {
                return "";
            }
            else
            {
                return result.Single().RejectMode;
            }
        }

        /// <summary>
        /// 获取报检单视图信息
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <returns>返回一条报检单的单据信息</returns>
        public View_S_CheckOutInDepotBill GetBill(string djh)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var result = from r in ctx.View_S_CheckOutInDepotBill
                         where r.入库单号 == djh
                         select r;

            if (result.Count() == 0)
            {
                return null;
            }
            else
            {
                return result.Single();
            }
        }
    }
}
