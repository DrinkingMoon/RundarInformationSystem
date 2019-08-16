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
using FlowControlService;

namespace Service_Manufacture_Storage
{
    /// <summary>
    /// 入库业务申请单服务类
    /// </summary>
    class AOGService : IAOGService
    {
        /// <summary>
        /// 获得单条业务总单信息
        /// </summary>
        /// <param name="billNo">业务号</param>
        /// <returns>返回业务总单信息</returns>
        public Business_WarehouseInPut_AOG GetSingleBillInfo(string billNo)
        {
            if (billNo == null)
            {
                return null;
            }

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Business_WarehouseInPut_AOG
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
        public List<View_Business_WarehouseInPut_AOGDetail> GetListViewDetailInfo(string billNo)
        {
            if (billNo == null)
            {
                return null;
            }

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.View_Business_WarehouseInPut_AOGDetail
                          where a.单据号 == billNo
                          select a;

            return varData.ToList();
        }

        /// <summary>
        /// 保存业务信息
        /// </summary>
        /// <param name="billInfo">业务总单信息</param>
        /// <param name="detailInfo">业务明细信息</param>
        public void SaveInfo(Business_WarehouseInPut_AOG billInfo, List<View_Business_WarehouseInPut_AOGDetail> detailInfo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();

            try
            {
                var varData = from a in ctx.Business_WarehouseInPut_AOG
                              where a.BillNo == billInfo.BillNo
                              select a;

                if (varData.Count() == 1)
                {
                    Business_WarehouseInPut_AOG lnqBill = varData.Single();

                    lnqBill.Remark = billInfo.Remark;
                }
                else if (varData.Count() == 0)
                {
                    ctx.Business_WarehouseInPut_AOG.InsertOnSubmit(billInfo);
                }
                else
                {
                    throw new Exception("单据数据不唯一");
                }

                var varDetail = from a in ctx.Business_WarehouseInPut_AOGDetail
                                where a.BillNo == billInfo.BillNo
                                select a;

                ctx.Business_WarehouseInPut_AOGDetail.DeleteAllOnSubmit(varDetail);
                ctx.SubmitChanges();

                foreach (View_Business_WarehouseInPut_AOGDetail item in detailInfo)
                {
                    Business_WarehouseInPut_AOGDetail lnqDetail = new Business_WarehouseInPut_AOGDetail();

                    lnqDetail.BillRelate = item.关联业务;
                    lnqDetail.BatchNo = item.批次号;
                    lnqDetail.BillNo = billInfo.BillNo;
                    lnqDetail.GoodsCount = item.数量;
                    lnqDetail.GoodsID = item.物品ID;
                    lnqDetail.Provider = item.供应商;
                    lnqDetail.Remark = item.备注;

                    ctx.Business_WarehouseInPut_AOGDetail.InsertOnSubmit(lnqDetail);
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
            BillNumberControl billNoControl = new BillNumberControl(CE_BillTypeEnum.到货单.ToString(), this);

            try
            {
                var varData = from a in ctx.Business_WarehouseInPut_AOG
                              where a.BillNo == billNo
                              select a;

                ctx.Business_WarehouseInPut_AOG.DeleteAllOnSubmit(varData);
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
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">业务号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.Business_WarehouseInPut_AOG
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
        public bool IsExist(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Business_WarehouseInPut_AOG
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

        public DataTable GetReferenceInfo(bool isRepeat)
        {
            string error = "";
            DataTable result = new DataTable();

            Hashtable hsTable = new Hashtable();

            hsTable.Add("@IsRepeat", isRepeat);

            result = GlobalObject.DatabaseServer.QueryInfoPro("Business_WarehouseInPut_AOG_ReferenceBill", hsTable, out error);

            if (error != null && error.Trim().Length > 0)
            {
                throw new Exception(error);
            }

            return result;
        }
    }
}
