/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  Consumption.cs
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

namespace Service_Manufacture_WorkShop
{
    /// <summary>
    /// 车间耗用单单据状态
    /// </summary>
    public enum ConsumptionBillStatus
    {
        新建单据,
        等待审核,
        等待确认,
        单据已完成
    }

    /// <summary>
    /// 车间物料耗用服务
    /// </summary>
    public class Consumption : IConsumption
    {
        /// <summary>
        /// 检查某业务是否存在
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">业务号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.WS_ConsumptionBill
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
            string sql = "SELECT * FROM [DepotManagement].[dbo].[WS_ConsumptionBill] where BillNo = '" + billNo + "'";

            System.Data.DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 获得耗用用途
        /// </summary>
        /// <returns></returns>
        public DataTable GetTreePurpose(string wsCode)
        {
            string strSql = @"Select PurposeCode as ChildCode,PurposeName as Name ,
                            case when LEN(PurposeCode) = 2 then '0'
                            else SUBSTRING(PurposeCode,1,LEN(PurposeCode) - 2) end as ParentCode
                            from WS_ConsumptionPurpose where WSCode = '" + wsCode + "'";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得单据信息
        /// </summary>
        /// <returns>返回单据信息</returns>
        public DataTable GetBillInfo()
        {
            string strSql = "select * from View_WS_ConsumptionBill order by 单据号 desc";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得单据明细
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>成功返回True,失败返回False</returns>
        public DataTable GetListInfo(string billNo)
        {
            string strSql = "select * from View_WS_ConsumptionList where 单据号 = '"+ billNo +"'";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得单条单据信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回单据信息</returns>
        public WS_ConsumptionBill GetBillSingle(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.WS_ConsumptionBill
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
                var varData = from a in ctx.WS_ConsumptionBill
                              where a.BillNo == billNo
                              select a;

                if (varData.Count() != 1)
                {
                    throw new Exception("数据不唯一");
                }
                else
                {
                    WS_ConsumptionBill tempBill = varData.Single();

                    if (tempBill.BillStatus == ConsumptionBillStatus.单据已完成.ToString())
                    {
                        throw new Exception("单据已完成，无法删除");
                    }

                    if (tempBill.Proposer != BasicInfo.LoginName)
                    {
                        throw new Exception("只有申请人本人才能删除单据");
                    }
                }

                ctx.WS_ConsumptionBill.DeleteAllOnSubmit(varData);

                IWorkShopProductCode serverProductCode = ServerModuleFactory.GetServerModule<IWorkShopProductCode>();

                serverProductCode.DeleteProductCodeDetail(ctx, billNo);

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
        void ListControl(DepotManagementDataContext ctx, WS_ConsumptionBill billInfo, DataTable list)
        {
            try
            {
                var varData = from a in ctx.WS_ConsumptionList
                              where a.BillNo == billInfo.BillNo
                              select a;

                ctx.WS_ConsumptionList.DeleteAllOnSubmit(varData);

                IWorkShopProductCode serverProductCode = ServerModuleFactory.GetServerModule<IWorkShopProductCode>();

                foreach (DataRow dr in list.Rows)
                {
                    WS_ConsumptionList tempList = new WS_ConsumptionList();

                    tempList.BillNo = billInfo.BillNo;
                    tempList.BatchNo = dr["批次号"].ToString();
                    tempList.GoodsID = Convert.ToInt32(dr["物品ID"]);
                    tempList.OperationCount = Convert.ToDecimal(dr["数量"]);
                    tempList.Remark = dr["备注"].ToString();

                    if (!serverProductCode.CheckProductCodeCount(billInfo.BillNo, billInfo.WSCode, tempList.GoodsID,
                        (int)CE_SubsidiaryOperationType.车间耗用,
                        Convert.ToDecimal(dr["数量"])))
                    {
                        throw new Exception(UniversalFunction.GetGoodsMessage(tempList.GoodsID) + " 产品编码数量与操作数量不一致");
                    }

                    if (list.Select(" 物品ID = "+ tempList.GoodsID +" AND 批次号 = '"+ tempList.BatchNo +"'").Length > 1)
                    {
                        throw new Exception(UniversalFunction.GetGoodsMessage(tempList.GoodsID) + "【批次号】：" 
                            + tempList.BatchNo + " 数据重复，请重新核对");
                    }

                    ctx.WS_ConsumptionList.InsertOnSubmit(tempList);
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
        void AssignmentValue(WS_ConsumptionBill bill, ref WS_ConsumptionBill returnBill)
        {
            returnBill.BillNo = bill.BillNo;
            returnBill.BillStatus = ConsumptionBillStatus.等待审核.ToString();
            returnBill.WSCode = bill.WSCode;
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
        public bool ProposeBill(WS_ConsumptionBill bill, DataTable  list,out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                WS_ConsumptionBill tempBill = new WS_ConsumptionBill();

                var varData = from a in ctx.WS_ConsumptionBill
                              where a.BillNo == bill.BillNo
                              select a;

                switch (varData.Count())
                {
                    case 0:
                        AssignmentValue(bill, ref tempBill);
                        ctx.WS_ConsumptionBill.InsertOnSubmit(tempBill);
                        break;
                    case 1:
                        tempBill = varData.Single();

                        if (tempBill.BillStatus == ConsumptionBillStatus.单据已完成.ToString())
                        {
                            throw new Exception("单据状态错误，请重新确认单据状态");
                        }

                        AssignmentValue(bill, ref tempBill);
                        break;
                    default:
                        throw new Exception("数据错误，请重新确认");
                }

                ListControl(ctx, tempBill, list);

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
                var varData = from a in ctx.WS_ConsumptionBill
                              where a.BillNo == billNo
                              select a;
                if (varData.Count() != 1)
                {
                    throw new Exception("数据错误，请重新确认");
                }
                else
                {
                    WS_ConsumptionBill tempBill = varData.Single();

                    if (tempBill.BillStatus != ConsumptionBillStatus.等待审核.ToString())
                    {
                        throw new Exception("单据状态错误，请重新确认");
                    }

                    tempBill.BillStatus = ConsumptionBillStatus.等待确认.ToString();
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
                var varData = from a in ctx.WS_ConsumptionBill
                              where a.BillNo == billNo
                              select a;

                WS_ConsumptionBill tempBill = new WS_ConsumptionBill();

                if (varData.Count() == 1)
                {
                    tempBill = varData.Single();

                    if (tempBill.BillStatus != ConsumptionBillStatus.等待确认.ToString())
                    {
                        throw new Exception("单据状态错误，请重新确认");
                    }

                    tempBill.BillStatus = ConsumptionBillStatus.单据已完成.ToString();
                    tempBill.Affirm = BasicInfo.LoginName;
                    tempBill.AffirmDate = ServerTime.Time;

                }
                else
                {
                    throw new Exception("数据不唯一,请重新确认");
                }


                ListControl(ctx, tempBill, list);
                ctx.SubmitChanges();

                foreach (DataRow dr in list.Rows)
                {
                    WS_Subsidiary tempSubsidiary = new WS_Subsidiary();

                    tempSubsidiary.BatchNo = dr["批次号"].ToString();
                    tempSubsidiary.BillNo = tempBill.BillNo;
                    tempSubsidiary.GoodsID = Convert.ToInt32(dr["物品ID"]);
                    tempSubsidiary.OperationCount = Convert.ToDecimal(dr["数量"]);
                    tempSubsidiary.OperationType = (int)CE_SubsidiaryOperationType.车间耗用;
                    tempSubsidiary.Remark = dr["备注"].ToString();
                    tempSubsidiary.WSCode = tempBill.WSCode;
                    tempSubsidiary.Proposer = tempBill.Proposer;
                    tempSubsidiary.ProposerDate = (DateTime)tempBill.ProposerDate;
                    tempSubsidiary.Affirm = BasicInfo.LoginName;
                    tempSubsidiary.AffirmDate = ServerTime.Time;

                    IWorkShopStock serverStock = ServerModuleFactory.GetServerModule<IWorkShopStock>();
                    WS_WorkShopStock tempWSStock = serverStock.GetStockSingleInfo(tempSubsidiary.WSCode,
                        tempSubsidiary.GoodsID,
                        tempSubsidiary.BatchNo);

                    tempSubsidiary.UnitPrice = tempWSStock == null ? 0 : tempWSStock.UnitPrice;
                    tempSubsidiary.BillTime = ServerTime.Time;

                    View_HR_Personnel tempHR = UniversalFunction.GetPersonnelInfo(tempSubsidiary.Proposer);

                    if (tempHR == null)
                    {
                        throw new Exception("申请人员不存在");
                    }

                    tempSubsidiary.Applicant = tempHR.部门名称;

                    serverStock.OperationSubsidiary(ctx, tempSubsidiary);
                }

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
        /// 回退单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="billStatus">回退后的单据状态</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <param name="rebackReason">回退原因</param>
        /// <returns>回退成功返回True，回退失败返回False</returns>
        public bool ReturnBill(string billNo, ConsumptionBillStatus billStatus, out string error, string rebackReason)
        {
            error = null;
            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            try
            {
                var varData = from a in dataContext.WS_ConsumptionBill
                              where a.BillNo == billNo
                              select a;

                string strMsg = "";

                if (varData.Count() != 1)
                {
                    throw new Exception("数据不唯一或者为空");
                }
                else
                {
                    WS_ConsumptionBill lnqTemp = varData.Single();

                    IBillMessagePromulgatorServer billMessageServer =
                        BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

                    billMessageServer.BillType = CE_BillTypeEnum.车间耗用单.ToString();

                    switch (billStatus)
                    {
                        case ConsumptionBillStatus.新建单据:

                            strMsg = string.Format("{0}号车间物料转换单已回退，请您重新处理单据; 回退原因为"
                                + rebackReason, billNo);

                            billMessageServer.PassFlowMessage(billNo, strMsg, BillFlowMessage_ReceivedUserType.用户,
                                UniversalFunction.GetPersonnelCode(lnqTemp.Proposer));

                            break;
                        case ConsumptionBillStatus.等待审核:

                            strMsg = string.Format("{0}号车间物料转换单已回退，请您重新处理单据; 回退原因为"
                                + rebackReason, billNo);

                            billMessageServer.PassFlowMessage(billNo, strMsg, BillFlowMessage_ReceivedUserType.用户,
                                UniversalFunction.GetPersonnelCode(lnqTemp.Audit));

                            break;
                        default:
                            break;
                    }

                    lnqTemp.BillStatus = billStatus.ToString();
                    dataContext.SubmitChanges();
                    return true;
                }

            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }
    }
}
