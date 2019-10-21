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

            return (varData == null || varData.Count() == 0);
        }

        #endregion

        public Bus_PBOM_Change GetItem(string billNo)
        {
            Bus_PBOM_Change result = new Bus_PBOM_Change();

            return result;
        }

        public List<View_Bus_PBOM_Change_Detail> GetDetail(string billNo)
        {
            List<View_Bus_PBOM_Change_Detail> result = new List<View_Bus_PBOM_Change_Detail>();

            return result;
        }

        public void SaveInfo(Bus_PBOM_Change billInfo, List<Bus_PBOM_Change_Detail> detail)
        {
            
        }
    }
}
