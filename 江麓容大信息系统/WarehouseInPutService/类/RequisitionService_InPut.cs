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
    class RequisitionService_InPut : IRequisitionService_InPut
    {
        /// <summary>
        /// 获得单条业务总单信息
        /// </summary>
        /// <param name="billNo">业务号</param>
        /// <returns>返回业务总单信息</returns>
        public Business_WarehouseInPut_Requisition GetSingleBillInfo(string billNo)
        {
            if (billNo == null)
            {
                return null;
            }

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Business_WarehouseInPut_Requisition
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
        public List<View_Business_WarehouseInPut_RequisitionDetail> GetListViewDetailInfo(string billNo)
        {
            if (billNo == null)
            {
                return null;
            }

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.View_Business_WarehouseInPut_RequisitionDetail
                          where a.单据号 == billNo
                          select a;

            return varData.ToList();
        }

        /// <summary>
        /// 保存业务信息
        /// </summary>
        /// <param name="billInfo">业务总单信息</param>
        /// <param name="detailInfo">业务明细信息</param>
        public void SaveInfo(Business_WarehouseInPut_Requisition billInfo, List<View_Business_WarehouseInPut_RequisitionDetail> detailInfo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();

            try
            {
                var varData = from a in ctx.Business_WarehouseInPut_Requisition
                              where a.BillNo == billInfo.BillNo
                              select a;

                if (varData.Count() == 1)
                {
                    Business_WarehouseInPut_Requisition lnqBill = varData.Single();

                    lnqBill.ApplyingDepartment = billInfo.ApplyingDepartment;
                    lnqBill.BillType = billInfo.BillType;
                    lnqBill.BillTypeDetail = billInfo.BillTypeDetail;
                    lnqBill.IsConfirmArrival = billInfo.IsConfirmArrival;
                    lnqBill.Remark = billInfo.Remark;
                }
                else if (varData.Count() == 0)
                {
                    ctx.Business_WarehouseInPut_Requisition.InsertOnSubmit(billInfo);
                }
                else
                {
                    throw new Exception("单据数据不唯一");
                }

                var varDetail = from a in ctx.Business_WarehouseInPut_RequisitionDetail
                                where a.BillNo == billInfo.BillNo
                                select a;

                ctx.Business_WarehouseInPut_RequisitionDetail.DeleteAllOnSubmit(varDetail);
                ctx.SubmitChanges();

                foreach (View_Business_WarehouseInPut_RequisitionDetail item in detailInfo)
                {
                    Business_WarehouseInPut_RequisitionDetail lnqDetail = new Business_WarehouseInPut_RequisitionDetail();

                    lnqDetail.BatchNo = item.批次号;
                    lnqDetail.BillNo = billInfo.BillNo;
                    lnqDetail.BillRelate = item.关联业务;
                    lnqDetail.GoodsCount = item.数量;
                    lnqDetail.GoodsID = item.物品ID;
                    lnqDetail.IsCheck = item.检验报告;
                    lnqDetail.Provider = item.供应商;
                    lnqDetail.Remark = item.备注;

                    ctx.Business_WarehouseInPut_RequisitionDetail.InsertOnSubmit(lnqDetail);
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
            BillNumberControl billNoControl = new BillNumberControl(CE_BillTypeEnum.入库申请单.ToString(), this);

            try
            {
                var varData = from a in ctx.Business_WarehouseInPut_Requisition
                              where a.BillNo == billNo
                              select a;

                ctx.Business_WarehouseInPut_Requisition.DeleteAllOnSubmit(varData);
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
            var varData = from a in ctx.Business_WarehouseInPut_Requisition
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

            var varData = from a in ctx.Business_WarehouseInPut_Requisition
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

        public List<Business_WarehouseInPut_Requisition> GetListBillInfo(List<string> listBillNo)
        {
            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.Business_WarehouseInPut_Requisition
                              where listBillNo.Contains(a.BillNo)
                              select a;

                return varData.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable GetListViewDetial(string billNo, int? goodsID, string batchNo, string provider)
        {
            string strSql = "select a.*, b.BillType from View_Business_WarehouseInPut_RequisitionDetail as a " +
                "inner join Business_WarehouseInPut_Requisition as b on a.单据号 = b.BillNo where a.单据号 = '" + billNo + "'";

            if (goodsID != null)
            {
                strSql += " and 物品ID = " + goodsID;
            }

            if (batchNo != null)
            {
                strSql += " and 批次号 = '" + batchNo + "'";
            }

            if (provider != null)
            {
                strSql += " and 供应商 = '" + provider + "'";
            }

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        public List<View_Business_WarehouseInPut_RequisitionDetail> GetListViewDetail_OrderForm(string billNo, List<string> listOrderForm)
        {
            List<View_Business_WarehouseInPut_RequisitionDetail> listResult =
                new List<View_Business_WarehouseInPut_RequisitionDetail>();

            string orderFormNum = "";

            foreach (string item in listOrderForm)
            {
                orderFormNum += "'" + item + "',";
            }

            orderFormNum = orderFormNum.Substring(0, orderFormNum.Length - 1);

            string strSql = " select a.订单号, b.物品ID , b.图号型号, b.物品名称 ,b.规格, " +
                            " a.供货单位, b.订货数量, c.单位 from View_B_OrderFormInfo as a  " +
                            " inner join View_B_OrderFormGoods as b on a.订单号 = b.订单号 " +
                            " inner join View_F_GoodsPlanCost as c on b.物品ID = c.序号 where a.订单号 in (" + orderFormNum + ")";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            foreach (DataRow dr in dtTemp.Rows)
            {
                View_Business_WarehouseInPut_RequisitionDetail tempLnq =
                    new View_Business_WarehouseInPut_RequisitionDetail();

                tempLnq.单据号 = billNo;
                tempLnq.单位 = dr["单位"].ToString();
                tempLnq.供应商 = dr["供货单位"].ToString();
                tempLnq.关联业务 = dr["订单号"].ToString();
                tempLnq.物品ID = (int)dr["物品ID"];
                tempLnq.图号型号 = dr["图号型号"].ToString();
                tempLnq.物品名称 = dr["物品名称"].ToString();
                tempLnq.规格 = dr["规格"].ToString();
                tempLnq.数量 = (decimal)dr["订货数量"];

                IBasicGoodsServer goodsService = SCM_Level01_ServerFactory.GetServerModule<IBasicGoodsServer>();

                F_GoodsAttributeRecord record = goodsService.GetGoodsAttirbuteRecord(tempLnq.物品ID, Convert.ToInt32(CE_GoodsAttributeName.来料须依据检验结果入库));

                if (record != null)
                {
                    tempLnq.检验报告 = Convert.ToBoolean(record.AttributeValue);
                }

                listResult.Add(tempLnq);
            }


            return listResult;
        }
    }
}
