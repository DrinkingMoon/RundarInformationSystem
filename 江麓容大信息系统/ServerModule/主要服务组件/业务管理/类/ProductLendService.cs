/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  ProductLendService.cs
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
using Service_Manufacture_WorkShop;

namespace ServerModule
{
    /// <summary>
    /// 借货单服务
    /// </summary>
    public class ProductLendService : BasicServer, IProductLendService
    {
        /// <summary>
        /// 检查某业务是否存在
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">业务号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.S_ProductLendBill
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
            string sql = "SELECT * FROM [DepotManagement].[dbo].[S_ProductLendBill] where BillNo = '" + billNo + "'";

            System.Data.DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 获得借货用途
        /// </summary>
        public DataTable GetTreePurpose()
        {
            string strSql = @"Select Code as ChildCode,Purpose as Name ,
                            case when LEN(Code) = 2 then '0'
                            else SUBSTRING(Code,1,LEN(Code) - 2) end as ParentCode
                            from BASE_MaterialRequisitionPurpose";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得单据信息
        /// </summary>
        /// <returns>返回单据信息</returns>
        public DataTable GetBillInfo()
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            string strSql = "select * from View_S_ProductLendBill";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得单据明细
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>成功返回True,失败返回False</returns>
        public DataTable GetListInfo(string billNo)
        {
            string strSql = "select * from View_S_ProductLendList where 单据号 = '" + billNo + "'";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得单条单据信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回单据信息</returns>
        public S_ProductLendBill GetBillSingle(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.S_ProductLendBill
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
                var varData = from a in ctx.S_ProductLendBill
                              where a.BillNo == billNo
                              select a;

                if (varData.Count() != 1)
                {
                    throw new Exception("数据不唯一");
                }
                else
                {
                    S_ProductLendBill tempBill = varData.Single();

                    if (tempBill.BillStatus == ProductLendReturnBillStatus.单据已完成.ToString())
                    {
                        throw new Exception("单据已完成，无法删除");
                    }

                    if (tempBill.Proposer != BasicInfo.LoginName)
                    {
                        throw new Exception("只有申请人本人才能删除单据");
                    }
                }

                ctx.S_ProductLendBill.DeleteAllOnSubmit(varData);
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
                var varData = from a in ctx.S_ProductLendList
                              where a.BillNo == billNo
                              select a;

                ctx.S_ProductLendList.DeleteAllOnSubmit(varData);

                foreach (DataRow dr in list.Rows)
                {
                    S_ProductLendList tempList = new S_ProductLendList();

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

                    ctx.S_ProductLendList.InsertOnSubmit(tempList);
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
        void AssignmentValue(S_ProductLendBill bill, ref S_ProductLendBill returnBill)
        {
            returnBill.BillNo = bill.BillNo;
            returnBill.BillStatus = ProductLendReturnBillStatus.等待审核.ToString();
            returnBill.DeptCode = bill.DeptCode;
            returnBill.StorageID = bill.StorageID;
            returnBill.Remark = bill.Remark;
            returnBill.PurposeCode = bill.PurposeCode;
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
        public bool ProposeBill(S_ProductLendBill bill, DataTable list, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                S_ProductLendBill tempBill = new S_ProductLendBill();

                var varData = from a in ctx.S_ProductLendBill
                              where a.BillNo == bill.BillNo
                              select a;

                switch (varData.Count())
                {
                    case 0:
                        AssignmentValue(bill, ref tempBill);
                        ctx.S_ProductLendBill.InsertOnSubmit(tempBill);
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
                var varData = from a in ctx.S_ProductLendBill
                              where a.BillNo == billNo
                              select a;
                if (varData.Count() != 1)
                {
                    throw new Exception("数据错误，请重新确认");
                }
                else
                {
                    S_ProductLendBill tempBill = varData.Single();

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
                var varData = from a in ctx.S_ProductLendBill
                              where a.BillNo == billNo
                              select a;

                S_ProductLendBill tempBill = new S_ProductLendBill();

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
        void OpertaionDetailAndStock(DepotManagementDataContext dataContext, S_ProductLendBill bill, DataTable dataTable)
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
        S_FetchGoodsDetailBill AssignDetailInfo(DepotManagementDataContext dataContext, S_ProductLendBill tempBill, DataRow dr)
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
            detailBill.OperationType = (int)CE_SubsidiaryOperationType.借货;
            detailBill.Remark = dr["备注"].ToString();
            detailBill.FillInDate = tempBill.ProposerDate;
            detailBill.StorageID = tempBill.StorageID;

            return detailBill;
        }
    }
}
