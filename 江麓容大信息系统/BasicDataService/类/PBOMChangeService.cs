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
using Service_Project_Design;
using ServerModule;
using FlowControlService;

namespace Service_Project_Design
{
    class PBOMChangeService : IPBOMChangeService
    {
        #region IFlowBusinessService 成员

        public void DeleteInfo(string billNo)
        {
            using (DepotManagementDataContext ctx = CommentParameter.DepotDataContext)
            {
                ctx.Connection.Open();
                ctx.Transaction = ctx.Connection.BeginTransaction();

                IFlowServer serverFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();
                BillNumberControl billNoControl = new BillNumberControl(CE_BillTypeEnum.生产BOM变更单.ToString(), this);

                try
                {
                    var varData = from a in ctx.Bus_PBOM_Change
                                  where a.BillNo == billNo
                                  select a;

                    ctx.Bus_PBOM_Change.DeleteAllOnSubmit(varData);
                    ctx.SubmitChanges();

                    serverFlow.FlowDelete(ctx, billNo);

                    ctx.Transaction.Commit();
                    billNoControl.CancelBill(billNo);
                }
                catch (Exception)
                {
                    ctx.Transaction.Rollback();
                    throw;
                }
            }
        }

        #endregion

        #region IBasicBillServer 成员

        public bool IsExist(string billNo)
        {
            using (DepotManagementDataContext ctx = CommentParameter.DepotDataContext)
            {
                return IsExist(ctx, billNo);
            }
        }

        public bool IsExist(ServerModule.DepotManagementDataContext dataContxt, string billNo)
        {
            var varData = from a in dataContxt.Bus_PBOM_Change
                          where a.BillNo == billNo
                          select a;

            return !(varData == null || varData.Count() == 0);
        }

        #endregion

        public Bus_PBOM_Change GetItem(string billNo)
        {
            Bus_PBOM_Change result = new Bus_PBOM_Change();

            using (DepotManagementDataContext ctx = CommentParameter.DepotDataContext)
            {
                var varData = from a in ctx.Bus_PBOM_Change
                              where a.BillNo == billNo
                              select a;

                if (varData.Count() == 1)
                {
                    result = varData.Single();
                }
                else
                {
                    result = null;
                }
            }

            return result;
        }

        public List<View_Bus_PBOM_Change_Detail> GetDetail(string billNo)
        {
            List<View_Bus_PBOM_Change_Detail> result = new List<View_Bus_PBOM_Change_Detail>();

            using (DepotManagementDataContext ctx = CommentParameter.DepotDataContext)
            {
                var varData = from a in ctx.View_Bus_PBOM_Change_Detail
                              where a.单据号 == billNo
                              select a;

                result = varData.ToList();
            }

            return result;
        }

        public void SaveInfo(Bus_PBOM_Change billInfo, List<View_Bus_PBOM_Change_Detail> detail)
        {
            using (DepotManagementDataContext ctx = CommentParameter.DepotDataContext)
            {
                ctx.Connection.Open();
                ctx.Transaction = ctx.Connection.BeginTransaction();

                try
                {
                    var varData = from a in ctx.Bus_PBOM_Change
                                  where a.BillNo == billInfo.BillNo
                                  select a;

                    if (varData.Count() == 1)
                    {
                        Bus_PBOM_Change lnqBill = varData.Single();

                        lnqBill.FileCode = billInfo.FileCode;
                        lnqBill.FileInfo = billInfo.FileInfo;
                        lnqBill.Reason = billInfo.Reason;
                    }
                    else
                    {
                        ctx.Bus_PBOM_Change.InsertOnSubmit(billInfo);
                    }

                    ctx.SubmitChanges();

                    var varDetail = from a in ctx.Bus_PBOM_Change_Detail
                                    where a.BillNo == billInfo.BillNo
                                    select a;
                    ctx.Bus_PBOM_Change_Detail.DeleteAllOnSubmit(varDetail);

                    foreach (View_Bus_PBOM_Change_Detail item in detail)
                    {
                        Bus_PBOM_Change_Detail de = new Bus_PBOM_Change_Detail();

                        de.ID = Guid.NewGuid();
                        de.BillNo = item.单据号;
                        de.Edtion = item.总成型号;
                        de.GoodsID = item.物品ID;
                        de.InvalidGoodsVersion = item.失效版次号;
                        de.IsInbound = item.领料;
                        de.ParentGoodsID = item.父级物品ID;
                        de.Usage = item.基数;
                        de.ValidGoodsVersion = item.生效版次号;
                        de.ValidTime = item.生效日期;
                        de.DBOMSysVersion = item.设计BOM版本;

                        ctx.Bus_PBOM_Change_Detail.InsertOnSubmit(de);
                    }

                    ctx.SubmitChanges();
                    ctx.Transaction.Commit();
                }
                catch (Exception)
                {
                    ctx.Transaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// 操作流程以外的业务
        /// </summary>
        /// <param name="billNo">单据号</param>
        public void OperatarUnFlowBusiness(string billNo)
        {
            IFlowServer serviceFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();

            string billStatus = serviceFlow.GetNextBillStatus(billNo);

            if (billStatus == null)
            {
                throw new Exception("单据状态为空，请重新确认");
            }

            if (billStatus != CE_CommonBillStatus.单据完成.ToString())
            {
                return;
            }

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            dataContxt.Connection.Open();
            dataContxt.Transaction = dataContxt.Connection.BeginTransaction();

            try
            {
                dataContxt.ExecuteCommand("exec BASE_PBOM_SaveSysVersion {0}", billNo);
                dataContxt.Transaction.Commit();
            }
            catch (Exception ex)
            {
                dataContxt.Transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }
    }
}
