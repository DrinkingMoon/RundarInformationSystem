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
using ServerModule;
using System.Drawing;
using FlowControlService;
using System.Windows.Forms;
using System.Threading;

namespace Service_Manufacture_Storage
{
    /// <summary>
    /// 整台份发料服务组件
    /// </summary>
    class WholeMachineRequisitionService : IWholeMachineRequisitionService
    {
        /// <summary>
        /// 获得单条业务总单信息
        /// </summary>
        /// <param name="billNo">业务号</param>
        /// <returns>返回业务总单信息</returns>
        public Business_WarehouseOutPut_WholeMachineRequisition GetSingleBillInfo(string billNo)
        {
            if (billNo == null)
            {
                return null;
            }

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Business_WarehouseOutPut_WholeMachineRequisition
                          where a.BillNo == billNo
                          select a;

            if (varData.Count() == 1)
            {
                return varData.Single();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获得业务明细信息
        /// </summary>
        /// <param name="billNo">业务号</param>
        /// <returns>返回业务明细列表</returns>
        public List<View_Business_WarehouseOutPut_WholeMachineRequisitionDetail> GetListViewDetailInfo(string billNo)
        {
            if (billNo == null)
            {
                return null;
            }

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.View_Business_WarehouseOutPut_WholeMachineRequisitionDetail
                          where a.单据号 == billNo
                          orderby a.ID
                          select a;

            return varData.ToList();
        }

        /// <summary>
        /// 获得库房顺序信息
        /// </summary>
        /// <param name="billNo">业务号</param>
        /// <returns>返回库房顺序列表</returns>
        public List<View_Business_WarehouseOutPut_WholeMachineRequisition_StorageID> GetListViewStorageIDInfo(string billNo)
        {
            if (billNo == null)
            {
                return null;
            }

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.View_Business_WarehouseOutPut_WholeMachineRequisition_StorageID
                          where a.单据号 == billNo
                          orderby a.ID
                          select a;

            return varData.ToList();
        }

        /// <summary>
        /// 保存业务信息
        /// </summary>
        /// <param name="billInfo">业务总单信息</param>
        /// <param name="detailInfo">业务明细信息</param>
        public void SaveInfo(Business_WarehouseOutPut_WholeMachineRequisition billInfo,
            List<View_Business_WarehouseOutPut_WholeMachineRequisitionDetail> detailInfo, 
            List<View_Business_WarehouseOutPut_WholeMachineRequisition_StorageID> listStorageID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();

            try
            {
                var varData = from a in ctx.Business_WarehouseOutPut_WholeMachineRequisition
                              where a.BillNo == billInfo.BillNo
                              select a;

                if (varData.Count() == 1)
                {
                    Business_WarehouseOutPut_WholeMachineRequisition lnqBill = varData.Single();

                    lnqBill.MachineCount = billInfo.MachineCount;
                    lnqBill.ProductType = billInfo.ProductType;
                    lnqBill.Remark = billInfo.Remark;
                    lnqBill.BillTypeDetail = billInfo.BillTypeDetail;
                    lnqBill.IncludeAfterSupplement = billInfo.IncludeAfterSupplement;
                }
                else if (varData.Count() == 0)
                {
                    ctx.Business_WarehouseOutPut_WholeMachineRequisition.InsertOnSubmit(billInfo);
                }
                else
                {
                    throw new Exception("单据数据不唯一");
                }

                #region 添加明细
                var varDetail = from a in ctx.Business_WarehouseOutPut_WholeMachineRequisitionDetail
                                where a.BillNo == billInfo.BillNo
                                select a;

                ctx.Business_WarehouseOutPut_WholeMachineRequisitionDetail.DeleteAllOnSubmit(varDetail);
                ctx.SubmitChanges();

                foreach (View_Business_WarehouseOutPut_WholeMachineRequisitionDetail item in detailInfo)
                {
                    Business_WarehouseOutPut_WholeMachineRequisitionDetail lnqDetail = 
                        new Business_WarehouseOutPut_WholeMachineRequisitionDetail();

                    lnqDetail.BillNo = billInfo.BillNo;
                    lnqDetail.GoodsCount = item.数量;
                    lnqDetail.GoodsID = item.物品ID;
                    lnqDetail.Cardinality = item.基数;

                    ctx.Business_WarehouseOutPut_WholeMachineRequisitionDetail.InsertOnSubmit(lnqDetail);
                }

                ctx.SubmitChanges();
                #endregion

                #region 添加库房顺序
                var varStorageID = from a in ctx.Business_WarehouseOutPut_WholeMachineRequisition_StorageID
                                   where a.BillNo == billInfo.BillNo
                                   select a;

                ctx.Business_WarehouseOutPut_WholeMachineRequisition_StorageID.DeleteAllOnSubmit(varStorageID);
                ctx.SubmitChanges();

                foreach (View_Business_WarehouseOutPut_WholeMachineRequisition_StorageID item1 in listStorageID)
                {
                    Business_WarehouseOutPut_WholeMachineRequisition_StorageID lnqStorage =
                        new Business_WarehouseOutPut_WholeMachineRequisition_StorageID();

                    lnqStorage.BillNo = billInfo.BillNo;
                    lnqStorage.StorageID = item1.库房代码;
                    lnqStorage.OrderID = item1.库房顺序;

                    ctx.Business_WarehouseOutPut_WholeMachineRequisition_StorageID.InsertOnSubmit(lnqStorage);
                }

                ctx.SubmitChanges();
                #endregion

                ctx.Transaction.Commit();
            }
            catch (Exception ex)
            {
                ctx.Transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 删除业务
        /// </summary>
        /// <param name="billNo">业务号</param>
        public void DeleteInfo(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();

            IFlowServer serverFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();
            BillNumberControl billNoControl = new BillNumberControl(CE_BillTypeEnum.整台份请领单.ToString(), this);

            try
            {
                var varData = from a in ctx.Business_WarehouseOutPut_WholeMachineRequisition
                              where a.BillNo == billNo
                              select a;

                ctx.Business_WarehouseOutPut_WholeMachineRequisition.DeleteAllOnSubmit(varData);
                ctx.SubmitChanges();
                serverFlow.FlowDelete(ctx, billNo);

                ctx.Transaction.Commit();
                billNoControl.CancelBill(billNo);
            }
            catch (Exception ex)
            {
                ctx.Transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 检查某业务是否存在
        /// </summary>
        /// <param name="billNo">业务号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Business_WarehouseOutPut_WholeMachineRequisition
                          where a.BillNo == billNo
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
        /// 检查某业务是否存在
        /// </summary>
        /// <param name="billNo">业务号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.Business_WarehouseOutPut_WholeMachineRequisition
                          where a.BillNo == billNo
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

        public void AutoFirstMaterialRequisition(string billNo, out List<string> listBillNo)
        {
            listBillNo = null;
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();
            IFlowServer serverFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();
            string billStatus = serverFlow.GetNextBillStatus(billNo);

            if (billStatus == null)
            {
                throw new Exception("单据状态为空，请重新确认");
            }

            try
            {
                if (billStatus == CE_CommonBillStatus.单据完成.ToString())
                {
                    var varBill = from a in ctx.Business_WarehouseOutPut_WholeMachineRequisition
                                  where a.BillNo == billNo
                                  select a;

                    Business_WarehouseOutPut_WholeMachineRequisition lnqWholeBill = varBill.Single();

                    var varDetail = from a in ctx.Business_WarehouseOutPut_WholeMachineRequisitionDetail
                                    where a.BillNo == billNo
                                    select a;

                    List<Business_WarehouseOutPut_WholeMachineRequisitionDetail> listDetail = varDetail.ToList();

                    var varStorage = from a in ctx.Business_WarehouseOutPut_WholeMachineRequisition_StorageID
                                     where a.BillNo == billNo
                                     select a;

                    List<Business_WarehouseOutPut_WholeMachineRequisition_StorageID> listStorage = varStorage.ToList();

                    CreateMaterialRequisition(ctx, lnqWholeBill, listDetail, listStorage, out listBillNo);
                    ctx.Transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                ctx.Transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }

        public void AutoSupplementaryRequisition(string billNo, out List<string> listBillNo)
        {
 
            listBillNo = null;
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();
            IFlowServer serverFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();
            string billStatus = serverFlow.GetNowBillStatus(billNo);

            if (billStatus == null)
            {
                throw new Exception("单据状态为空，请重新确认");
            }

            try
            {
                if (billStatus == CE_CommonBillStatus.单据完成.ToString())
                {
                    var varBill = from a in ctx.Business_WarehouseOutPut_WholeMachineRequisition
                                  where a.BillNo == billNo
                                  select a;

                    Business_WarehouseOutPut_WholeMachineRequisition lnqWholeBill = varBill.Single();

                    string error = "";

                    Hashtable hsTable = new Hashtable();
                    hsTable.Add("@BillNo", billNo);

                    DataTable tempTable = GlobalObject.DatabaseServer.QueryInfoPro("Business_WarehouseOutPut_WholeMachineRequisition_GetLastShortageInfo", 
                        hsTable, out error);

                    List<Business_WarehouseOutPut_WholeMachineRequisitionDetail> listDetail = new List<Business_WarehouseOutPut_WholeMachineRequisitionDetail>();

                    if (tempTable != null && tempTable.Rows.Count > 0)
                    {
                        foreach (DataRow dr in tempTable.Rows)
                        {
                            Business_WarehouseOutPut_WholeMachineRequisitionDetail detail = 
                                new Business_WarehouseOutPut_WholeMachineRequisitionDetail();

                            detail.BillNo = billNo;
                            detail.Cardinality = 0;
                            detail.GoodsCount = Convert.ToDecimal(dr["NeedCount"]);
                            detail.GoodsID = Convert.ToInt32(dr["GoodsID"]);

                            listDetail.Add(detail);
                        }
                    }
                    else
                    {
                        throw new Exception("无物品可生成补领的【领料单】");
                    }

                    var varStorage = from a in ctx.Business_WarehouseOutPut_WholeMachineRequisition_StorageID
                                     where a.BillNo == billNo
                                     select a;

                    List<Business_WarehouseOutPut_WholeMachineRequisition_StorageID> listStorage = varStorage.ToList();

                    CreateMaterialRequisition(ctx, lnqWholeBill, listDetail, listStorage, out listBillNo);
                    ctx.Transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                ctx.Transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }

        void CreateMaterialRequisition(DepotManagementDataContext ctx, Business_WarehouseOutPut_WholeMachineRequisition billInfo, 
            List<Business_WarehouseOutPut_WholeMachineRequisitionDetail> listDetail, 
            List<Business_WarehouseOutPut_WholeMachineRequisition_StorageID> listStorage, out List<string> listBillNo)
        {
            listBillNo = null;
            ServerModule.IMaterialRequisitionGoodsServer serviceGoods = ServerModule.ServerModuleFactory.GetServerModule<ServerModule.IMaterialRequisitionGoodsServer>();
            ServerModule.IMaterialRequisitionServer serviceBill = ServerModule.ServerModuleFactory.GetServerModule<ServerModule.IMaterialRequisitionServer>();
            ServerModule.IStoreServer serviceStore = ServerModule.ServerModuleFactory.GetServerModule<ServerModule.IStoreServer>();
            IFlowServer serviceFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();
            ServerModule.IMaterialRequisitionPurposeServer servicePurpose = ServerModule.ServerModuleFactory.GetServerModule<ServerModule.IMaterialRequisitionPurposeServer>();

            BillNumberControl billControl = new BillNumberControl(CE_BillTypeEnum.领料单, serviceBill);
            
            List<CommonProcessInfo> listProcessInfo = serviceFlow.GetFlowData(billInfo.BillNo);

            try
            {
                List<GoodsInfo> listGoodsInfo = new List<GoodsInfo>();

                foreach (Business_WarehouseOutPut_WholeMachineRequisitionDetail detail in listDetail)
                {
                    decimal requstCount = detail.GoodsCount;

                    foreach (Business_WarehouseOutPut_WholeMachineRequisition_StorageID storage in listStorage)
                    {
                        if (requstCount == 0)
                        {
                            break;
                        }

                        List<View_S_Stock> lstStock = serviceStore.GetGoodsStoreOnlyForAssembly(detail.GoodsID, storage.StorageID).ToList();

                        foreach (View_S_Stock stock in lstStock)
                        {
                            if (requstCount == 0)
                            {
                                break;
                            }

                            if (stock.库存数量 <= requstCount)
                            {
                                GoodsInfo goodsInfo = new GoodsInfo();

                                goodsInfo.GoodsID = detail.GoodsID;
                                goodsInfo.BatchNo = stock.批次号;
                                goodsInfo.Provider = stock.供货单位;
                                goodsInfo.GoodsCount = stock.库存数量;

                                goodsInfo.ListInfo = new List<string>();

                                goodsInfo.ListInfo.Add(listGoodsInfo.Count.ToString());
                                goodsInfo.ListInfo.Add(storage.StorageID);
                                goodsInfo.ListInfo.Add(stock.库存数量.ToString());

                                listGoodsInfo.Add(goodsInfo);

                                requstCount = requstCount - stock.库存数量;
                            }
                            else
                            {
                                GoodsInfo goodsInfo = new GoodsInfo();

                                goodsInfo.GoodsID = detail.GoodsID;
                                goodsInfo.BatchNo = stock.批次号;
                                goodsInfo.Provider = stock.供货单位;
                                goodsInfo.GoodsCount = requstCount;

                                goodsInfo.ListInfo = new List<string>();

                                goodsInfo.ListInfo.Add(listGoodsInfo.Count.ToString());
                                goodsInfo.ListInfo.Add(storage.StorageID);
                                goodsInfo.ListInfo.Add(detail.GoodsCount.ToString());

                                listGoodsInfo.Add(goodsInfo);

                                requstCount = 0;
                            }
                        }
                    }
                }

                if (listGoodsInfo != null && listGoodsInfo.Count > 0)
                {
                    listBillNo = new List<string>();
                    CommonProcessInfo firstProcess = new CommonProcessInfo();
                    CommonProcessInfo SecondProcess = new CommonProcessInfo();
                    CommonProcessInfo ThridProcess = new CommonProcessInfo();

                    var varTemp = from a in listProcessInfo where a.操作节点 == "新建" orderby a.时间 descending select a;

                    if (varTemp.Count() != 0)
                    {
                        firstProcess = varTemp.First();
                    }
                    else
                    {
                        throw new Exception("此单据无【新建】流程");
                    }

                    varTemp = from a in listProcessInfo where a.操作节点 == "审核" orderby a.时间 descending select a;

                    if (varTemp.Count() != 0)
                    {
                        SecondProcess = varTemp.First();
                    }
                    else
                    {
                        throw new Exception("此单据无【审核】流程");
                    }

                    varTemp = from a in listProcessInfo where a.操作节点 == "确认" orderby a.时间 descending select a;

                    if (varTemp.Count() != 0)
                    {
                        ThridProcess = varTemp.First();
                    }

                    foreach (Business_WarehouseOutPut_WholeMachineRequisition_StorageID storage in listStorage)
                    {

                        List<GoodsInfo> listGoodsInfoTemp = (from a in listGoodsInfo
                                                            where a.ListInfo[1].ToString() == storage.StorageID
                                                            orderby Convert.ToInt32( a.ListInfo[0])
                                                            select a).ToList();

                        if (listGoodsInfoTemp.Count > 0)
                        {
                            string error = "";

                            S_MaterialRequisition bill = new S_MaterialRequisition();

                            bill.AssociatedBillNo = billInfo.BillNo;
                            bill.AssociatedBillType = CE_BillTypeEnum.整台份请领单.ToString();
                            bill.Bill_ID = billControl.GetNewBillNo(ctx);
                            bill.Bill_Time = ServerTime.Time;
                            bill.BillStatus = "等待出库";

                            View_HR_Personnel personnelInfo = UniversalFunction.GetPersonnelInfo(firstProcess.工号);

                            bill.Department = personnelInfo.部门编码;
                            bill.DepartmentDirector = SecondProcess.人员;
                            bill.DepotManager = ThridProcess == new CommonProcessInfo() ? BasicInfo.LoginName : ThridProcess.人员;
                            bill.FetchCount = (int)billInfo.MachineCount;
                            bill.FetchType = FetchGoodsType.整台领料.ToString();
                            bill.FillInPersonnel = firstProcess.人员;
                            bill.FillInPersonnelCode = personnelInfo.工号;
                            bill.ProductType = billInfo.ProductType;
                            bill.PurposeCode = servicePurpose.GetBillPurpose(ctx, billInfo.BillTypeDetail).Code;
                            bill.Remark = billInfo.Remark;
                            bill.StorageID = storage.StorageID;

                            if (!serviceBill.AutoCreateBill(ctx, bill, out error))
                            {
                                throw new Exception(error);
                            }

                            listBillNo.Add(bill.Bill_ID);

                            for (int i = 0; i < listGoodsInfoTemp.Count; i++)
                            {
                                S_MaterialRequisitionGoods goodsInfo = new S_MaterialRequisitionGoods();

                                goodsInfo.BasicCount = Convert.ToDecimal(listGoodsInfoTemp[i].ListInfo[2]);
                                goodsInfo.BatchNo = listGoodsInfoTemp[i].BatchNo;
                                goodsInfo.Bill_ID = bill.Bill_ID;
                                goodsInfo.GoodsID = listGoodsInfoTemp[i].GoodsID;
                                goodsInfo.ProviderCode = listGoodsInfoTemp[i].Provider;
                                goodsInfo.RealCount = listGoodsInfoTemp[i].GoodsCount;
                                goodsInfo.Remark = GetWorkBench_WashFlag(billInfo.ProductType, goodsInfo.GoodsID);
                                goodsInfo.RequestCount = listGoodsInfoTemp[i].GoodsCount;

                                IProductOrder serviceProductOrder = ServerModuleFactory.GetServerModule<IProductOrder>();
                                goodsInfo.ShowPosition = 0;
                                    //serviceProductOrder.GetPosition(ctx, billInfo.ProductType, goodsInfo.GoodsID);

                                if (!serviceGoods.AutoCreateGoods(ctx, goodsInfo, out error))
                                {
                                    throw new Exception(error);
                                }
                            }
                        }

                        ctx.SubmitChanges();
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        string GetWorkBench_WashFlag(string productCode, int goodsID)
        {
            string result = "";

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in
                              (
                                  from a in ctx.View_P_AssemblingBom
                                  join b in ctx.F_GoodsPlanCost
                                  on new { GoodsCode = a.零件编码, GoodsName = a.零件名称, Spec = a.规格 } equals
                                  new { b.GoodsCode, b.GoodsName, b.Spec }
                                  select new
                                  {
                                      ProductCode = a.产品编码,
                                      GoodsID = b.ID,
                                      WorkBench = a.工位,
                                      IsWash = a.是否清洗
                                  })
                          where a.ProductCode == productCode && a.GoodsID == goodsID
                          select a;

            if (varData.Count() > 0)
            {
                foreach (var item in varData)
                {
                    result += item.WorkBench + ",";
                }

                result.Remove(result.Length - 1);

                if (varData.OrderByDescending(k => k.IsWash).First().IsWash)
                {
                    result = "清洗," + result;
                }
            }

            return result;
        }

        public void SignFinish(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Business_WarehouseOutPut_WholeMachineRequisition
                          where a.BillNo == billNo
                          select a;

            if (varData.Count() == 1)
            {
                Business_WarehouseOutPut_WholeMachineRequisition wholeMachine = varData.Single();

                wholeMachine.SignFinish = true;
            }

            ctx.SubmitChanges();
        }

        public void SetStatus(DataGridView dgv)
        {
            DataTable tempTable = dgv.DataSource as DataTable;

            if (tempTable == null && tempTable.Rows.Count == 0)
            {
                return;
            }

            if (!tempTable.Columns.Contains("物料状态"))
            {
                tempTable.Columns.Add("物料状态");
            }

            ThreadPool.QueueUserWorkItem((WaitCallback)delegate
            {
                try
                {
                    using (DepotManagementDataContext ctx = CommentParameter.DepotDataContext)
                    {

                        foreach (DataRow dr in tempTable.Rows)
                        {
                            if (dr["物料状态"] == null || GlobalObject.GeneralFunction.IsNullOrEmpty(dr["物料状态"].ToString()))
                            {
                                dr["物料状态"] = ctx.Fun_get_Business_WarehouseOutPut_WholeMachineRequisition_GoodsStatus(dr["业务编号"].ToString());
                            }
                        }
                    }

                    if (!dgv.InvokeRequired)
                    {
                        return;
                    }

                    dgv.BeginInvoke((MethodInvoker)delegate
                    {
                        dgv.DataSource = tempTable;

                        foreach (DataGridViewRow dgvr in dgv.Rows)
                        {
                            switch (dgvr.Cells["物料状态"].Value.ToString())
                            {
                                case "待发料":
                                    dgvr.DefaultCellStyle.BackColor = Color.Yellow;
                                    break;
                                case "缺料":
                                    dgvr.DefaultCellStyle.BackColor = Color.Red;
                                    break;
                                case "正常":
                                    dgvr.DefaultCellStyle.BackColor = Color.White;
                                    break;
                                default:
                                    break;
                            }
                        }
                    });
                }
                catch (Exception)
                {
                    throw;
                }
            });

        }
    }
}
