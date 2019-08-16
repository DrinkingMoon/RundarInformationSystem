/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  MusterUse.cs
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
using DBOperate;
using PlatformManagement;
using GlobalObject;

namespace ServerModule
{
    /// <summary>
    /// 样品耗用管理类
    /// </summary>
    class MusterUse:BasicServer, ServerModule.IMusterUse
    {
        /// <summary>
        /// 获得库存信息
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <returns>返回库存数</returns>
        public decimal GetStockCount(int goodsID, string batchNo)
        {
            string strSql = "select * from (select GoodsID, BatchNo, MusterCount from S_MusterAffirmBill "+
                            " where DJZT not in ('单据已完成','单据已报废', '新建单据', '等待检验','等待财务确认', "+
                            " '等待仓管确认到货','等待主管审核')"+
                            " Union all select Purchase_GoodsID, Purchase_BatchNo, Store_GoodsCount_AOG "+
                            " from Business_Sample_ConfirmTheApplication as a"+
                            " inner join Flow_FlowBillData as b on a.BillNo = b.BillNo"+
                            " where b.FlowID >= 46 and b.FlowID <= 51) as a where a.BatchNo = '" + batchNo + "' and a.GoodsID = " + goodsID;

            DataTable tempTable = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (tempTable.Rows.Count == 1)
            {
                decimal useCount = GetUseSumCount(goodsID, batchNo);

                return Convert.ToDecimal(tempTable.Rows[0]["MusterCount"]) - useCount;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 获得样品耗用单Table
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <returns>返回耗用数量之和</returns>
        decimal GetUseSumCount(int goodsID, string batchNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.S_MusterUseBill
                          join b in ctx.S_MusterUseList
                          on a.DJH equals b.DJH
                          where b.GoodsID == goodsID && b.BatchNo == batchNo
                          select b.Count;

            if (varData == null || varData.Count() == 0)
            {
                return 0;
            }
            else
            {
                return Convert.ToDecimal(varData.Sum(k => k.Value));
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
            var varData = from a in ctx.S_MusterUseBill
                          where a.DJH == billNo
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
            string sql = "SELECT * FROM [DepotManagement].[dbo].[S_MusterUseBill] where DJH = '" + billNo + "'";

            System.Data.DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 获得所有单据
        /// </summary>
        /// <returns>返回样品耗用单信息</returns>
        public DataTable GetAllBill()
        {
            string strSql = "select * from View_S_MusterUseBill order by 单据号 desc";
            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dt;
        }

        /// <summary>
        /// 获得一条单据
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <returns>返回一条样品耗用单信息</returns>
        public S_MusterUseBill GetBill(string djh)
        {
            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            var varData = from a in dataContext.S_MusterUseBill
                          where a.DJH == djh
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
        /// 获得指定单据号的明细
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <returns>返回样品耗用单明细信息</returns>
        public DataTable GetList(string djh)
        {
            string strSql = "select * from View_S_MusterUseList where 单据号 = '" + djh + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dt;
        }

        /// <summary>
        /// 更新单据状态
        /// </summary>
        /// <param name="inMusterUse">样品耗用单信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>更新成功返回True，更新失败返回False</returns>
        public bool UpdateBill(S_MusterUseBill inMusterUse,out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.S_MusterUseBill
                              where a.DJH == inMusterUse.DJH
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数据不唯一或者为空";
                    return false;
                }
                else
                {
                    S_MusterUseBill lnqBill = varData.Single();

                    if (inMusterUse.DJZT != lnqBill.DJZT)
                    {
                        error = "单据状态错误，请重新刷新单据确认单据状态";
                        return false;
                    }

                    switch (lnqBill.DJZT)
                    {
                        case "等待主管审核":
                            lnqBill.DJZT = "等待确认出库";
                            lnqBill.SHR = BasicInfo.LoginName;
                            lnqBill.SHRQ = ServerTime.Time;
                            break;
                        case "等待确认出库":

                            if (lnqBill.DJZT == "单据已完成")
                            {
                                error = "单据不能重复确认";
                                return false;
                            }

                            lnqBill.DJZT = "单据已完成";
                            lnqBill.KFR = BasicInfo.LoginName;
                            lnqBill.KFRQ = ServerTime.Time;

                            //OpertaionDetailAndStock(dataContext, lnqBill);

                            break;
                        default:
                            break;
                    }

                    dataContext.SubmitChanges();
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
        /// 删除指定单据号的明细
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="djh">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        private bool DeleteList(DepotManagementDataContext context,
            string djh,out string error)
        {
            try
            {
                error = null;

                var varData = from a in context.S_MusterUseList
                              where a.DJH == djh
                              select a;

                context.S_MusterUseList.DeleteAllOnSubmit(varData);

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 添加指定单据号的明细
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="useList">样品耗用单明细信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>添加成功返回True，添加失败返回False</returns>
        private bool AddList(DepotManagementDataContext context,
            DataTable useList,out string error)
        {
            try
            {
                error = null;

                for (int i = 0; i < useList.Rows.Count; i++)
                {
                    S_MusterUseList lnqList = new S_MusterUseList();

                    lnqList.DJH = useList.Rows[i]["单据号"].ToString();
                    lnqList.GoodsID = Convert.ToInt32(useList.Rows[i]["物品ID"]);
                    lnqList.BatchNo = useList.Rows[i]["批次号"].ToString();
                    lnqList.Count = Convert.ToDecimal(useList.Rows[i]["数量"].ToString());
                    lnqList.Remark = useList.Rows[i]["备注"].ToString();

                    context.S_MusterUseList.InsertOnSubmit(lnqList);
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
        /// 保存对单据明细的修改
        /// </summary>
        /// <param name="djh">样品耗用单号</param>
        /// <param name="storageID">库房ID</param>
        /// <param name="purposeCode">用途编码</param>
        /// <param name="useList">单据明细信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>保存成功返回True，保存失败返回False</returns>
        public bool SaveBill(string djh,string storageID,string purposeCode,
            DataTable useList,out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.S_MusterUseBill
                              where a.DJH == djh
                              select a;

                if (varData.Count() == 0)
                {
                    S_MusterUseBill lnqBill = new S_MusterUseBill();

                    lnqBill.DJH = djh;
                    lnqBill.DJZT = "等待主管审核";
                    lnqBill.PurposeCode = purposeCode;
                    lnqBill.SQR = BasicInfo.LoginName;
                    lnqBill.SQRQ = ServerTime.Time;
                    lnqBill.StorageID = storageID;
                    lnqBill.SHR = null;
                    lnqBill.SHRQ = null;
                    lnqBill.KFR = null;
                    lnqBill.KFRQ = null;

                    dataContext.S_MusterUseBill.InsertOnSubmit(lnqBill);

                    if (!AddList(dataContext, useList, out error))
                    {
                        return false;
                    }
                }
                else if (varData.Count() == 1)
                {
                    S_MusterUseBill lnqBill = varData.Single();

                    if (lnqBill.DJZT == "单据已完成")
                    {
                        error = "单据状态错误";
                        return false;
                    }

                    lnqBill.DJZT = "等待主管审核";
                    lnqBill.PurposeCode = purposeCode;
                    lnqBill.SQR = BasicInfo.LoginName;
                    lnqBill.SQRQ = ServerTime.Time;
                    lnqBill.StorageID = storageID;
                    lnqBill.SHR = null;
                    lnqBill.SHRQ = null;
                    lnqBill.KFR = null;
                    lnqBill.KFRQ = null;

                    if (!DeleteList(dataContext, lnqBill.DJH, out error))
                    {
                        return false;
                    }

                    if (!AddList(dataContext, useList, out error))
                    {
                        return false;
                    }
                }
                else
                {
                    error = "数据不唯一或者为空";
                    return false;
                }

                dataContext.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        #region 取消未完成单据的样品的库存管理
        ///// <summary>
        ///// 根据单据信息操作账务信息与库存信息
        ///// </summary>
        ///// <param name="dataContext">数据上下文</param>
        ///// <param name="billInfo">单据信息</param>
        //void OpertaionDetailAndStock(DepotManagementDataContext dataContext, S_MusterUseBill billInfo)
        //{
        //    IFinancialDetailManagement serverDetail =
        //        ServerModule.ServerModuleFactory.GetServerModule<IFinancialDetailManagement>();

        //    var varData = from a in dataContext.S_MusterUseList
        //                  where a.DJH == billInfo.DJH
        //                  select a;

        //    foreach (S_MusterUseList item in varData)
        //    {
        //        if (!IsCheck(billInfo, item))
        //        {
        //            throw new Exception("库存数量不足");
        //        }

        //        S_FetchGoodsDetailBill detailInfo = AssignDetailInfo(dataContext, billInfo, item);

        //        if (detailInfo == null)
        //        {
        //            throw new Exception("获取账务信息或者库存信息失败");
        //        }

        //        serverDetail.ProcessFetchGoodsDetail(dataContext, detailInfo, null);
        //    }
        //}

        ///// <summary>
        ///// 获得供应商
        ///// </summary>
        ///// <param name="listInfo">耗用单明细信息</param>
        ///// <returns>返回样品信息</returns>
        //string GetProvider(S_MusterUseList listInfo)
        //{
        //    DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

        //    var varData = from a in dataContext.S_MusterAffirmBill
        //                  where a.GoodsID == listInfo.GoodsID
        //                  && a.BatchNo == listInfo.BatchNo
        //                  select a;

        //    if (varData.Count() == 1)
        //    {
        //        return varData.Single().Provider;
        //    }

        //    var varData1 = from a in dataContext.Business_Sample_ConfirmTheApplication
        //                   where a.Purchase_GoodsID == listInfo.GoodsID
        //                   && a.Purchase_BatchNo == listInfo.BatchNo
        //                   select a;

        //    if (varData1.Count() == 1)
        //    {
        //        return varData1.Single().Purchase_Provider;
        //    }

        //    return "";
        //}

        ///// <summary>
        ///// 赋值账务信息
        ///// </summary>
        ///// <param name="context">数据上下文</param>
        ///// <param name="billInfo">单据信息</param>
        ///// <param name="item">明细信息</param>
        ///// <returns>返回账务信息对象</returns>
        //S_FetchGoodsDetailBill AssignDetailInfo(DepotManagementDataContext context, S_MusterUseBill billInfo, S_MusterUseList item)
        //{

        //    var varMusterStock = from a in context.S_MusterStock
        //                         where a.GoodsID == item.GoodsID
        //                         && a.BatchNo == item.BatchNo
        //                         && a.StrorageID == billInfo.StorageID
        //                         select a;

        //    if (varMusterStock.Count() != 1)
        //    {
        //        throw new Exception( "此仓库" + UniversalFunction.GetGoodsMessage((int)item.GoodsID) + "【批次号】:" 
        //            + item.BatchNo + "库存不足");
        //    }
        //    else
        //    {
        //        S_MusterStock lnqMusterStock = varMusterStock.Single();

        //        if (Convert.ToDecimal(lnqMusterStock.Count) < Convert.ToDecimal(item.Count))
        //        {
        //            throw new Exception("批次号为 【" + item.BatchNo + "】的物品 库存不足，请重新确认数量！");
        //        }

        //        lnqMusterStock.Count = Convert.ToDecimal(lnqMusterStock.Count) - Convert.ToDecimal(item.Count);
        //    }

        //    S_FetchGoodsDetailBill detailBill = new S_FetchGoodsDetailBill();

        //    detailBill.ID = Guid.NewGuid();
        //    detailBill.FetchBIllID = billInfo.DJH;
        //    detailBill.BillTime = ServerTime.Time;
        //    detailBill.FetchCount = item.Count;
        //    detailBill.GoodsID = (int)item.GoodsID;
        //    detailBill.BatchNo = item.BatchNo;
        //    detailBill.UnitPrice = 0;
        //    detailBill.Price = detailBill.UnitPrice * (decimal)detailBill.FetchCount;

        //    IMusterAffirmBill serverMuster = ServerModule.ServerModuleFactory.GetServerModule<IMusterAffirmBill>();

        //    detailBill.Provider = GetProvider(item);
        //    detailBill.FillInPersonnel = billInfo.SQR;
        //    detailBill.FinanceSignatory = null;
        //    detailBill.DepartDirector = billInfo.SHR;
        //    detailBill.DepotManager = billInfo.KFR;
        //    detailBill.OperationType = (int)CE_SubsidiaryOperationType.样品耗用;
        //    detailBill.Remark = item.Remark;
        //    detailBill.FillInDate = billInfo.SQRQ;
        //    detailBill.StorageID = billInfo.StorageID;

        //    return detailBill;
        //}

        ///// <summary>
        ///// 获得批次号的样品库库存数
        ///// </summary>
        ///// <param name="goodsID">物品ID</param>
        ///// <param name="batchNo">批次号</param>
        ///// <returns>返回获得的库存数</returns>
        //decimal GetMusterBatchNoStock(int goodsID,string batchNo)
        //{
        //    DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

        //    var varStock = from a in ctx.S_MusterStock
        //                   where a.GoodsID == goodsID
        //                   && a.BatchNo == batchNo
        //                   select a;

        //    if (varStock.Count() == 1)
        //    {

        //        var varData = from a in ctx.Business_Sample_ConfirmTheApplication
        //                      where a.Purchase_BatchNo == batchNo
        //                      && a.Purchase_GoodsID == goodsID
        //                      select a;

        //        if (varData.Count() == 1)
        //        {
        //            if (varData.Single().Purchase_SampleType == "PPAP样件'")
        //            {
        //                if (varData.Single().SQE_SampleDisposeType_DisqualificationCount != null)
        //                {
        //                    return (decimal)varStock.Single().Count - 
        //                        (decimal)varData.Single().SQE_SampleDisposeType_DisqualificationCount;
        //                }
        //            }
        //            else
        //            {
        //                if (varData.Single().Review_InspectResult_ReWork_DisqualificationCount != null)
        //                {
        //                    return (decimal)varStock.Single().Count - 
        //                        (decimal)varData.Single().Review_InspectResult_ReWork_DisqualificationCount;
        //                }
        //            }
        //        }

        //        return (decimal)varStock.Single().Count;
        //    }

        //    return 0;
        //}

        ///// <summary>
        ///// 检查是否可以出库
        ///// </summary>
        ///// <param name="inMusterUse">单据信息</param>
        ///// <param name="listInfo">明细信息</param>
        ///// <returns>已经过检验返回True，未经过检验返回False</returns>
        //bool IsCheck(S_MusterUseBill inMusterUse, S_MusterUseList listInfo)
        //{
        //    try
        //    {
        //        DepotManagementDataContext ctx = CommentParameter.DepotDataContext;


        //        var varStock = from a in ctx.S_MusterStock
        //                       where a.GoodsID == listInfo.GoodsID
        //                       && a.BatchNo == listInfo.BatchNo
        //                       && a.StrorageID == inMusterUse.StorageID
        //                       select a;

        //        if (varStock.Count() != 1)
        //        {
        //            return false;
        //        }

        //        var varData = from a in ctx.S_MusterAffirmBill
        //                      where a.GoodsID == listInfo.GoodsID
        //                      && a.BatchNo == listInfo.BatchNo
        //                      && a.StorageID == inMusterUse.StorageID
        //                      select a;

        //        if (varData.Count() == 1)
        //        {
        //            if ((decimal)varStock.Single().Count >= (decimal)listInfo.Count)
        //            {
        //                return true;
        //            }
        //        }

        //        var varData1 = from a in ctx.Business_Sample_ConfirmTheApplication
        //                       join b in ctx.Flow_FlowBillData
        //                       on a.BillNo equals b.BillNo
        //                       where a.Purchase_GoodsID == listInfo.GoodsID
        //                       && a.Purchase_BatchNo == listInfo.BatchNo
        //                       && a.Purchase_StorageID == inMusterUse.StorageID
        //                       select new
        //                       {
        //                           Purchase_SampleType = a.Purchase_SampleType,
        //                           FlowID = b.FlowID,
        //                           Review_InspectResult_ReWork_QualificationCount = a.Review_InspectResult_ReWork_QualificationCount,
        //                           Review_InspectResult_ReWork_DisqualificationCount = a.Review_InspectResult_ReWork_DisqualificationCount,
        //                           SQE_SampleDisposeType_QualificationCount = a.SQE_SampleDisposeType_QualificationCount,
        //                           SQE_SampleDisposeType_DisqualificationCount = a.SQE_SampleDisposeType_DisqualificationCount

        //                       };

        //        if (varData1.Count() == 1)
        //        {
        //            if (varData1.Single().Purchase_SampleType == "PPAP样件")
        //            {
        //                if (varData1.Single().FlowID > 47)
        //                {
        //                    if (varData1.Single().SQE_SampleDisposeType_QualificationCount != null)
        //                    {
        //                        if ((decimal)varStock.Single().Count - (decimal)varData1.Single().SQE_SampleDisposeType_DisqualificationCount 
        //                            >= (decimal)listInfo.Count)
        //                        {
        //                            return true;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if ((decimal)varStock.Single().Count >= (decimal)listInfo.Count)
        //                        {
        //                            return true;
        //                        }
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                if (varData1.Single().FlowID > 46)
        //                {
        //                    if (varData1.Single().Review_InspectResult_ReWork_QualificationCount != null)
        //                    {
        //                        if ((decimal)varStock.Single().Count - (decimal)varData1.Single().Review_InspectResult_ReWork_DisqualificationCount
        //                            >= (decimal)listInfo.Count)
        //                        {
        //                            return true;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if ((decimal)varStock.Single().Count >= (decimal)listInfo.Count)
        //                        {
        //                            return true;
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        return false;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}
        #endregion

        /// <summary>
        /// 报废单据
        /// </summary>
        /// <param name="djh">样品耗用单单号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>报废成功返回True，报废失败返回False</returns>
        public bool ScarpBill(string djh,out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.S_MusterUseBill
                              where a.DJH == djh
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数据不唯一或者为空";
                    return false;
                }
                else
                {
                    S_MusterUseBill lnqbill = varData.Single();

                    if (lnqbill.DJZT == "单据已完成")
                    {
                        error = "单据已完成不能删除";
                        return false;
                    }

                    dataContext.S_MusterUseBill.DeleteOnSubmit(lnqbill);
                }

                dataContext.SubmitChanges();

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
