using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformManagement;
using ServerModule;
using System.Data;
using GlobalObject;
using FlowControlService;

namespace Service_Manufacture_WorkShop
{
    class BatchNoChange : IBatchNoChange
    {
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
            BillNumberControl billNoControl = new BillNumberControl(CE_BillTypeEnum.车间批次管理变更.ToString(), this);

            try
            {
                var varData = from a in ctx.Business_WorkShop_BatchNoChangeDetail
                              where a.BillNo == billNo
                              select a;

                var varData1 = from a in ctx.Business_WorkShop_BatchNoChange
                               where a.BillNo == billNo
                               select a;

                ctx.Business_WorkShop_BatchNoChange.DeleteAllOnSubmit(varData1);
                ctx.Business_WorkShop_BatchNoChangeDetail.DeleteAllOnSubmit(varData);

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

            var varData = from a in ctx.Business_WorkShop_BatchNoChange
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
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">业务号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.Business_WorkShop_BatchNoChange
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

        public void SaveInfo(Business_WorkShop_BatchNoChange changeInfo, List<View_Business_WorkShop_BatchNoChangeDetail> lstDetail)
        {
            IFlowServer service = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();

            try
            {
                if (service.GetNowBillStatus(changeInfo.BillNo) != CE_CommonBillStatus.新建单据.ToString())
                {
                    return;
                }

                var varData = from a in ctx.Business_WorkShop_BatchNoChange
                              where a.BillNo == changeInfo.BillNo
                              select a;

                ctx.Business_WorkShop_BatchNoChange.DeleteAllOnSubmit(varData);

                var varDetail = from a in ctx.Business_WorkShop_BatchNoChangeDetail
                                where a.BillNo == changeInfo.BillNo
                                select a;

                ctx.Business_WorkShop_BatchNoChangeDetail.DeleteAllOnSubmit(varDetail);
                ctx.SubmitChanges();

                ctx.Business_WorkShop_BatchNoChange.InsertOnSubmit(changeInfo);
                ctx.SubmitChanges();

                foreach (View_Business_WorkShop_BatchNoChangeDetail item in lstDetail)
                {
                    Business_WorkShop_BatchNoChangeDetail detail = new Business_WorkShop_BatchNoChangeDetail();

                    detail.ID = Guid.NewGuid();
                    detail.BillNo = changeInfo.BillNo;
                    detail.GoodsID = item.物品ID;
                    detail.ManageType = item.管理方式;

                    if (detail.GoodsID == null || detail.GoodsID == 0)
                    {
                        throw new Exception("存在未选择【物品】的记录行");
                    }

                    if (detail.ManageType.Trim().Length == 0)
                    {
                        throw new Exception(UniversalFunction.GetGoodsMessage(ctx, (int)detail.GoodsID) + "未选择【管理方式】");
                    }

                    ctx.Business_WorkShop_BatchNoChangeDetail.InsertOnSubmit(detail);
                }

                ctx.SubmitChanges();
                ctx.Transaction.Commit();
            }
            catch (Exception ex)
            {
                ctx.Transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }

        public void OperationBusiness(string billNo)
        {
            IFlowServer serviceFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();

            try
            {
                if (serviceFlow.GetNextBillStatus(billNo) != CE_CommonBillStatus.单据完成.ToString())
                {
                    return;
                }

                var varData = from a in ctx.ZPX_BatchNoManage
                              select a;

                ctx.ZPX_BatchNoManage.DeleteAllOnSubmit(varData);
                ctx.SubmitChanges();

                var varDetail = from a in ctx.Business_WorkShop_BatchNoChangeDetail
                                where a.BillNo == billNo
                                select a;

                foreach (Business_WorkShop_BatchNoChangeDetail item in varDetail)
                {
                    ZPX_BatchNoManage batchNoInfo = new ZPX_BatchNoManage();

                    batchNoInfo.ManageType = item.ManageType;
                    batchNoInfo.GoodsID = item.GoodsID;

                    ctx.ZPX_BatchNoManage.InsertOnSubmit(batchNoInfo);
                }

                ctx.SubmitChanges();
                ctx.Transaction.Commit();
            }
            catch (Exception ex)
            {
                ctx.Transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }

        public Business_WorkShop_BatchNoChange GetSingleInfo(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Business_WorkShop_BatchNoChange
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

        public List<View_Business_WorkShop_BatchNoChangeDetail> GetListDetail(string billNo)
        {
            List<View_Business_WorkShop_BatchNoChangeDetail> result = new List<View_Business_WorkShop_BatchNoChangeDetail>();

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.View_Business_WorkShop_BatchNoChangeDetail
                          where a.单据号 == billNo
                          select a;

            if (varData.Count() == 0)
            {
                var varData1 = from a in ctx.ZPX_BatchNoManage
                               join b in ctx.F_GoodsPlanCost
                               on a.GoodsID equals b.ID
                               select new
                               {
                                   物品ID = a.GoodsID,
                                   图号型号 = b.GoodsCode,
                                   物品名称 = b.GoodsName,
                                   规格 = b.Spec,
                                   管理方式 = a.ManageType,
                                   单据号 = billNo
                               };

                foreach (var item in varData1)
                {
                    View_Business_WorkShop_BatchNoChangeDetail detail = new View_Business_WorkShop_BatchNoChangeDetail();

                    detail.单据号 = item.单据号;
                    detail.管理方式 = item.管理方式;
                    detail.规格 = item.规格;
                    detail.图号型号 = item.图号型号;
                    detail.物品ID = item.物品ID;
                    detail.物品名称 = item.物品名称;

                    result.Add(detail);
                }
            }
            else
            {
                result = varData.ToList();
            }

            return result;
        }
    }
}
