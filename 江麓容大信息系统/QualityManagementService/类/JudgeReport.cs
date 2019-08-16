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

namespace Service_Quality_QC
{
    class JudgeReport : IJudgeReport
    {
        /// <summary>
        /// 获得单条业务总单信息
        /// </summary>
        /// <param name="billNo">业务号</param>
        /// <returns>返回业务总单信息</returns>
        public Business_InspectionJudge_JudgeReport GetSingleBillInfo(string billNo)
        {
            if (billNo == null)
            {
                return null;
            }

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Business_InspectionJudge_JudgeReport
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
        public List<View_Business_InspectionJudge_JudgeReport_Item> GetListViewItemInfo(string billNo)
        {
            if (billNo == null)
            {
                return null;
            }

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.View_Business_InspectionJudge_JudgeReport_Item
                          where a.单据号 == billNo
                          select a;

            return varData.ToList();
        }

        /// <summary>
        /// 获得业务明细信息
        /// </summary>
        /// <param name="billNo">业务号</param>
        /// <returns>返回业务明细列表</returns>
        public List<View_Business_InspectionJudge_JudgeReportDetail> GetListViewDetailInfo(string billNo)
        {
            if (billNo == null)
            {
                return null;
            }

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.View_Business_InspectionJudge_JudgeReportDetail
                          where a.单据号 == billNo
                          select a;

            return varData.ToList();
        }

        /// <summary>
        /// 保存业务信息
        /// </summary>
        /// <param name="billInfo">业务总单信息</param>
        /// <param name="detailInfo">业务明细信息</param>
        public void SaveInfo(Business_InspectionJudge_JudgeReport billInfo, 
            List<View_Business_InspectionJudge_JudgeReport_Item> itemInfo, List<View_Business_InspectionJudge_JudgeReportDetail> detailInfo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();

            try
            {
                var varData = from a in ctx.Business_InspectionJudge_JudgeReport
                              where a.BillNo == billInfo.BillNo
                              select a;

                if (varData.Count() == 1)
                {
                    Business_InspectionJudge_JudgeReport lnqBill = varData.Single();

                    lnqBill.BillNo = billInfo.BillNo;
                    lnqBill.FinalJudge = billInfo.FinalJudge;
                    lnqBill.FinalJudgeExplain = billInfo.FinalJudgeExplain;
                    lnqBill.IsFinalJudge = billInfo.IsFinalJudge;
                    lnqBill.Judge = billInfo.Judge;
                    lnqBill.JudgeExplain = billInfo.JudgeExplain;
                    lnqBill.JudgeReportNo = billInfo.JudgeReportNo;

                }
                else if (varData.Count() == 0)
                {
                    ctx.Business_InspectionJudge_JudgeReport.InsertOnSubmit(billInfo);
                }
                else
                {
                    throw new Exception("单据数据不唯一");
                }

                ctx.SubmitChanges();

                var varItem = from a in ctx.Business_InspectionJudge_JudgeReport_Item
                                where a.BillNo == billInfo.BillNo
                                select a;

                ctx.Business_InspectionJudge_JudgeReport_Item.DeleteAllOnSubmit(varItem);
                ctx.SubmitChanges();

                foreach (View_Business_InspectionJudge_JudgeReport_Item item in itemInfo)
                {
                    Business_InspectionJudge_JudgeReport_Item lnqItem = new Business_InspectionJudge_JudgeReport_Item();

                    lnqItem.BillNo = billInfo.BillNo;
                    lnqItem.JudgeItem = item.判定项目;
                    lnqItem.JudgeResult = item.判定结果;

                    ctx.Business_InspectionJudge_JudgeReport_Item.InsertOnSubmit(lnqItem);
                    ctx.SubmitChanges();
                }

                var varDetail = from a in ctx.Business_InspectionJudge_JudgeReportDetail
                              where a.BillNo == billInfo.BillNo
                              select a;

                ctx.Business_InspectionJudge_JudgeReportDetail.DeleteAllOnSubmit(varDetail);
                ctx.SubmitChanges();

                foreach (View_Business_InspectionJudge_JudgeReportDetail item in detailInfo)
                {
                    Business_InspectionJudge_JudgeReportDetail lnqDetail = new Business_InspectionJudge_JudgeReportDetail();

                    lnqDetail.BillNo = billInfo.BillNo;
                    lnqDetail.BatchNo = item.批次号;
                    lnqDetail.BillRelate = item.关联业务;
                    lnqDetail.GoodsCount = item.数量;
                    lnqDetail.GoodsID = item.物品ID;
                    lnqDetail.Provider = item.供应商;
                    lnqDetail.Remark = item.备注;

                    ctx.Business_InspectionJudge_JudgeReportDetail.InsertOnSubmit(lnqDetail);
                    ctx.SubmitChanges();
                }

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
            BillNumberControl billNoControl = new BillNumberControl(CE_BillTypeEnum.判定报告.ToString(), this);

            try
            {
                var varData = from a in ctx.Business_InspectionJudge_JudgeReport
                              where a.BillNo == billNo
                              select a;

                ctx.Business_InspectionJudge_JudgeReport.DeleteAllOnSubmit(varData);
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
            var varData = from a in ctx.Business_InspectionJudge_JudgeReport
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

            var varData = from a in ctx.Business_InspectionJudge_JudgeReport
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

            result = GlobalObject.DatabaseServer.QueryInfoPro("Business_InspectionJudge_JudgeReport_ReferenceBill", hsTable, out error);

            if (error != null && error.Trim().Length > 0)
            {
                throw new Exception(error);
            }

            return result;
        }

        public List<View_Business_InspectionJudge_JudgeReportDetail> GetJudgeReportDetail<T>(string billNo, string judgeReportBillNo, List<T> listDetail)
        {
            List<View_Business_InspectionJudge_JudgeReportDetail> detailSource = new List<View_Business_InspectionJudge_JudgeReportDetail>();

            if (listDetail != null)
            {
                foreach (var item in listDetail)
                {
                    View_Business_InspectionJudge_JudgeReportDetail tempDetail = new View_Business_InspectionJudge_JudgeReportDetail();

                    if (typeof(T) == typeof(View_Business_WarehouseInPut_RequisitionDetail))
                    {
                        View_Business_WarehouseInPut_RequisitionDetail itemTemp = item as View_Business_WarehouseInPut_RequisitionDetail;

                        tempDetail.单据号 = judgeReportBillNo;
                        tempDetail.单位 = itemTemp.单位;
                        tempDetail.供应商 = itemTemp.供应商;
                        tempDetail.关联业务 = billNo;
                        tempDetail.规格 = itemTemp.规格;
                        tempDetail.批次号 = itemTemp.批次号;
                        tempDetail.数量 = itemTemp.数量;
                        tempDetail.图号型号 = itemTemp.图号型号;
                        tempDetail.物品ID = itemTemp.物品ID;
                        tempDetail.物品名称 = itemTemp.物品名称;
                    }
                    else if (typeof(T) == typeof(View_Business_WarehouseInPut_AOGDetail))
                    {
                        View_Business_WarehouseInPut_AOGDetail itemTemp = item as View_Business_WarehouseInPut_AOGDetail;

                        tempDetail.单据号 = judgeReportBillNo;
                        tempDetail.单位 = itemTemp.单位;
                        tempDetail.供应商 = itemTemp.供应商;
                        tempDetail.关联业务 = billNo;
                        tempDetail.规格 = itemTemp.规格;
                        tempDetail.批次号 = itemTemp.批次号;
                        tempDetail.数量 = itemTemp.数量;
                        tempDetail.图号型号 = itemTemp.图号型号;
                        tempDetail.物品ID = itemTemp.物品ID;
                        tempDetail.物品名称 = itemTemp.物品名称;
                    }
                    else if (typeof(T) == typeof(Business_InspectionJudge_InspectionReport))
                    {
                        Business_InspectionJudge_InspectionReport itemTemp = item as Business_InspectionJudge_InspectionReport;
                        View_F_GoodsPlanCost goodsInfo = UniversalFunction.GetGoodsInfo(itemTemp.GoodsID);

                        tempDetail.单据号 = judgeReportBillNo;
                        tempDetail.单位 = goodsInfo.单位;
                        tempDetail.供应商 = itemTemp.Provider;
                        tempDetail.关联业务 = billNo;
                        tempDetail.规格 = goodsInfo.规格;
                        tempDetail.批次号 = itemTemp.BatchNo;
                        tempDetail.数量 = itemTemp.GoodsCount;
                        tempDetail.图号型号 = goodsInfo.图号型号;
                        tempDetail.物品ID = itemTemp.GoodsID;
                        tempDetail.物品名称 = goodsInfo.物品名称;
                    }

                    detailSource.Add(tempDetail);
                }
            }

            return detailSource;
        }
    }
}
