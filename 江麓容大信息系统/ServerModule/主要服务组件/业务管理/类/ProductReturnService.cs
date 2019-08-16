/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  ProductReturnService.cs
 * 作者    :  曹津彬    版本: v1.00    日期: 2014/02/08
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
using System.Text.RegularExpressions;
using ServerModule;
using System.Reflection;

namespace ServerModule
{
    /// <summary>
    /// 还贷服务
    /// </summary>
    public class ProductReturnService : BasicServer, IProductReturnService
    {
        /// <summary>
        /// 检查某业务是否存在
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">业务号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.S_ProductReturnBill
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
        /// 检查某单据是否存在
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(string billNo)
        {
            string sql = "SELECT * FROM [DepotManagement].[dbo].[S_ProductReturnBill] where BillNo = '" + billNo + "'";

            System.Data.DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 获得单据信息
        /// </summary>
        /// <returns>返回单据信息</returns>
        public DataTable GetBillInfo()
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            string strSql = "select * from View_S_ProductReturnBill";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得单据明细
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>成功返回True,失败返回False</returns>
        public DataTable GetListInfo(string billNo)
        {
            string strSql = "select * from View_S_ProductReturnList where 单据号 = '" + billNo + "'";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得单条单据信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回单据信息</returns>
        public S_ProductReturnBill GetBillSingle(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.S_ProductReturnBill
                          where a.BillNo == billNo
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
        /// 删除单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        public bool DeleteBill(string billNo, out string error)
        {
            error = null;

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            try
            {
                var varData = from a in ctx.S_ProductReturnBill
                              where a.BillNo == billNo
                              select a;

                if (varData.Count() != 1)
                {
                    throw new Exception("数据不唯一");
                }
                else
                {
                    S_ProductReturnBill tempBill = varData.Single();

                    if (tempBill.BillStatus == ProductLendReturnBillStatus.单据已完成.ToString())
                    {
                        throw new Exception("单据已完成，无法删除");
                    }

                    if (tempBill.Proposer != BasicInfo.LoginName)
                    {
                        throw new Exception("只有申请人本人才能删除单据");
                    }
                }

                ctx.S_ProductReturnBill.DeleteAllOnSubmit(varData);
                ctx.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 单据明细操作
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">单据号</param>
        /// <param name="list">单据明细</param>
        void ListControl(DepotManagementDataContext ctx, string billNo, DataTable list)
        {
            try
            {
                var varData = from a in ctx.S_ProductReturnList
                              where a.BillNo == billNo
                              select a;

                ctx.S_ProductReturnList.DeleteAllOnSubmit(varData);

                foreach (DataRow dr in list.Rows)
                {
                    S_ProductReturnList tempList = new S_ProductReturnList();

                    tempList.BillNo = billNo;
                    tempList.BatchNo = dr["批次号"].ToString();
                    tempList.Provider = dr["供应商"].ToString();
                    tempList.GoodsID = Convert.ToInt32(dr["物品ID"]);
                    tempList.OperationCount = Convert.ToDecimal(dr["数量"]);
                    tempList.Remark = dr["备注"].ToString();

                    if (list.Select(" 物品ID = " + tempList.GoodsID + " AND 批次号 = '" + tempList.BatchNo + "'").Length > 1)
                    {
                        throw new Exception("数据重复，请重新核对");
                    }

                    ctx.S_ProductReturnList.InsertOnSubmit(tempList);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 单据赋值
        /// </summary>
        /// <param name="bill">单据数据集</param>
        /// <param name="returnBill">返回单据</param>
        void AssignmentValue(S_ProductReturnBill bill, ref S_ProductReturnBill returnBill)
        {
            returnBill.BillNo = bill.BillNo;
            returnBill.BillStatus = ProductLendReturnBillStatus.等待审核.ToString();
            returnBill.DeptCode = bill.DeptCode;
            returnBill.StorageID = bill.StorageID;
            returnBill.Remark = bill.Remark;
            returnBill.Proposer = BasicInfo.LoginName;
            returnBill.ProposerDate = ServerTime.Time;
            returnBill.Affirm = null;
            returnBill.AffirmDate = null;
            returnBill.Audit = null;
            returnBill.AuditDate = null;
        }

        /// <summary>
        /// 申请单据
        /// </summary>
        /// <param name="bill">单据信息</param>
        /// <param name="list">明细信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        public bool ProposeBill(S_ProductReturnBill bill, DataTable list, out string error)
        {
            error = null;
            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
                S_ProductReturnBill tempBill = new S_ProductReturnBill();

                var varData = from a in ctx.S_ProductReturnBill
                              where a.BillNo == bill.BillNo
                              select a;

                switch (varData.Count())
                {
                    case 0:
                        AssignmentValue(bill, ref tempBill);
                        ctx.S_ProductReturnBill.InsertOnSubmit(tempBill);
                        break;
                    case 1:
                        tempBill = varData.Single();
                        AssignmentValue(bill, ref tempBill);
                        break;
                    default:
                        throw new Exception("数据错误，请重新确认");
                }

                ListControl(ctx, tempBill.BillNo, list);

                ctx.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 审核单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        public bool AuditBill(string billNo, out string error)
        {
            error = null;

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            try
            {
                var varData = from a in ctx.S_ProductReturnBill
                              where a.BillNo == billNo
                              select a;
                if (varData.Count() != 1)
                {
                    throw new Exception("数据错误，请重新确认");
                }
                else
                {
                    S_ProductReturnBill tempBill = varData.Single();

                    if (tempBill.BillStatus != ProductLendReturnBillStatus.等待审核.ToString())
                    {
                        throw new Exception("单据状态错误，请重新确认");
                    }

                    tempBill.BillStatus = ProductLendReturnBillStatus.等待确认.ToString();
                    tempBill.Audit = BasicInfo.LoginName;
                    tempBill.AuditDate = ServerTime.Time;

                    ctx.SubmitChanges();
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
        /// 确认单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="list">明细信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        public bool AffirmBill(string billNo, DataTable list, out string error)
        {
            error = null;

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();

            try
            {
                var varData = from a in ctx.S_ProductReturnBill
                              where a.BillNo == billNo
                              select a;

                S_ProductReturnBill tempBill = new S_ProductReturnBill();

                if (varData.Count() == 1)
                {
                    tempBill = varData.Single();

                    if (tempBill.BillStatus != ProductLendReturnBillStatus.等待确认.ToString())
                    {
                        throw new Exception("单据状态错误，请重新确认");
                    }

                    tempBill.BillStatus = ProductLendReturnBillStatus.单据已完成.ToString();
                    tempBill.Affirm = BasicInfo.LoginName;
                    tempBill.AffirmDate = ServerTime.Time;

                }
                else
                {
                    throw new Exception("数据不唯一,请重新确认");
                }

                ListControl(ctx, tempBill.BillNo, list);
                ctx.SubmitChanges();

                OpertaionDetailAndStock(ctx, tempBill, list);

                ctx.SubmitChanges();
                ctx.Transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                ctx.Transaction.Rollback();
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 根据单据信息操作账务信息与库存信息
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="bill">单据信息</param>
        /// <param name="dataTable">明细信息集合</param>
        void OpertaionDetailAndStock(DepotManagementDataContext dataContext, S_ProductReturnBill bill, DataTable dataTable)
        {
            IFinancialDetailManagement serverDetail =
                ServerModule.ServerModuleFactory.GetServerModule<IFinancialDetailManagement>();

            foreach (DataRow dr in dataTable.Rows)
            {
                S_FetchGoodsDetailBill detailInfo = AssignDetailInfo(dataContext, bill, dr);

                if (detailInfo == null)
                {
                    throw new Exception("获取账务信息或者库存信息失败");
                }

                serverDetail.ProcessFetchGoodsDetail(dataContext, detailInfo, null);
            }
        }

        /// <summary>
        /// 赋值账务信息
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="tempBill">单据信息</param>
        /// <param name="dr">明细信息</param>
        /// <returns>返回账务信息对象</returns>
        S_FetchGoodsDetailBill AssignDetailInfo(DepotManagementDataContext dataContext, S_ProductReturnBill tempBill, DataRow dr)
        {
            S_FetchGoodsDetailBill detailBill = new S_FetchGoodsDetailBill();

            detailBill.ID = Guid.NewGuid();
            detailBill.FetchBIllID = tempBill.BillNo;
            detailBill.BillTime = ServerTime.Time;
            detailBill.FetchCount = Convert.ToDecimal(dr["数量"]);
            detailBill.GoodsID = Convert.ToInt32(dr["物品ID"]);
            detailBill.BatchNo = dr["批次号"].ToString();
            detailBill.UnitPrice = 0;
            detailBill.Price = detailBill.UnitPrice * (decimal)detailBill.FetchCount;
            detailBill.Provider = dr["供应商"].ToString();
            detailBill.FillInPersonnel = tempBill.Proposer;
            detailBill.FinanceSignatory = null;
            detailBill.DepartDirector = tempBill.Audit;
            detailBill.DepotManager = tempBill.Affirm;
            detailBill.OperationType = (int)CE_SubsidiaryOperationType.还货;
            detailBill.Remark = dr["备注"].ToString();
            detailBill.FillInDate = tempBill.ProposerDate;
            detailBill.StorageID = tempBill.StorageID;

            return detailBill;
        }

        /// <summary>
        /// 获得所有信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="provider">供应商</param>
        /// <param name="batchNo">批次号</param>
        /// <returns>返回Table</returns>
        public DataTable GetAllInfo(string billNo, int goodsID, string provider, string batchNo)
        {
            string strSql = "select 欠账物品ID as 物品ID, 欠账物品图号型号 as 图号型号, 欠账物品名称 as 物品名称, "+
                     " 欠账物品规格 as 规格, 欠账物品供应商 as 供应商, 欠账物品批次号 as 批次号, 还账数量 as 数量 , 单位, 备注 " +
                     " from View_S_ProductReturnList where 单据号 = '" + billNo +
                     "' and 还账物品ID = "+ goodsID +" and 还账物品批次号 = '"+ batchNo +"' and 还账物品供应商 = '"+ provider +"'";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 删除物品对应的信息
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">单据号</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="provider">供应商</param>
        /// <param name="batchNo">批次号</param>
        public void DeleteInfo(DepotManagementDataContext ctx, string billNo, int goodsID, string provider, string batchNo)
        {
            try
            {
                var varData = from a in ctx.S_MaterialRequisitionProductReturnList
                              where a.BillNo == billNo
                              && a.ReturnGoodsID == goodsID
                              && a.ReturnBatchNo == batchNo
                              && a.ReturnProvider == provider
                              select a;

                ctx.S_MaterialRequisitionProductReturnList.DeleteAllOnSubmit(varData);
                ctx.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">单据号</param>
        public void DeleteInfo(DepotManagementDataContext ctx, string billNo)
        {
            try
            {
                var varData = from a in ctx.S_MaterialRequisitionProductReturnList
                              where a.BillNo == billNo
                              select a;

                ctx.S_MaterialRequisitionProductReturnList.DeleteAllOnSubmit(varData);
                ctx.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="provider">供应商</param>
        /// <param name="batchNo">批次号</param>
        /// <param name="infoTable">信息列表</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        public bool SaveInfo(string billNo, int goodsID, string provider, string batchNo, DataTable infoTable, out string error)
        {
            error = null;
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            try
            {
                var varData = from a in ctx.S_MaterialRequisitionProductReturnList
                              where a.BatchNo == batchNo
                              && a.BillNo == billNo
                              && a.GoodsID == goodsID
                              && a.Provider == provider
                              select a;

                ctx.S_MaterialRequisitionProductReturnList.DeleteAllOnSubmit(varData);

                foreach (DataRow dr in infoTable.Rows)
                {
                    S_MaterialRequisitionProductReturnList tempLnq = new S_MaterialRequisitionProductReturnList();

                    tempLnq.ReturnGoodsID = goodsID;
                    tempLnq.ReturnBatchNo = batchNo;
                    tempLnq.ReturnProvider = provider;
                    tempLnq.BillNo = billNo;
                    tempLnq.GoodsID = Convert.ToInt32(dr["物品ID"]);
                    tempLnq.OperatorCount = Convert.ToDecimal(dr["数量"]);
                    tempLnq.Provider = dr["供应商"].ToString();
                    tempLnq.BatchNo = dr["批次号"].ToString();
                    tempLnq.Remark = dr["备注"].ToString();

                    ctx.S_MaterialRequisitionProductReturnList.InsertOnSubmit(tempLnq);
                }

                ctx.SubmitChanges();
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
