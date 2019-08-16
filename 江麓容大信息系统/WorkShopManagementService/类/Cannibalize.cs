/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  Cannibalize.cs
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
using System.Collections.Generic;

namespace Service_Manufacture_WorkShop
{
    /// <summary>
    /// 车间调运单单据状态
    /// </summary>
    public enum CannibalizeBillStatus
    {
        新建单据,
        等待审核,
        等待确认,
        单据已完成
    }

    /// <summary>
    /// 车间物料耗用服务
    /// </summary>
    public class Cannibalize : ICannibalize
    {
        /// <summary>
        /// 检查某业务是否存在
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">业务号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.WS_CannibalizeBill
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
            string sql = "SELECT * FROM [DepotManagement].[dbo].[WS_CannibalizeBill] where BillNo = '" + billNo + "'";

            System.Data.DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 获得操作库房信息列表
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回列表信息</returns>
        public List<WS_CannibalizeWSCode> GetOperationWSCode(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.WS_CannibalizeWSCode
                          where a.BillNo == billNo
                          select a;

            if (varData.Count() != 2)
            {
                return null;
            }
            else
            {
                return varData.ToList();
            }
        }

        /// <summary>
        /// 获得单据信息
        /// </summary>
        /// <returns>返回单据信息</returns>
        public DataTable GetBillInfo()
        {
            string strSql = "select * from View_WS_CannibalizeBill order by 单据号 desc";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得单据明细
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>成功返回True,失败返回False</returns>
        public DataTable GetListInfo(string billNo)
        {
            string strSql = "select * from View_WS_CannibalizeList where 单据号 = '" + billNo + "'";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得单条单据信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回单据信息</returns>
        public WS_CannibalizeBill GetBillSingle(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.WS_CannibalizeBill
                          where a.BillNo == billNo
                          select a;

            if (varData == null || varData.Count() != 1)
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
                var varData = from a in ctx.WS_CannibalizeBill
                              where a.BillNo == billNo
                              select a;

                if (varData.Count() != 1)
                {
                    throw new Exception("数据不唯一");
                }
                else
                {
                    WS_CannibalizeBill tempBill = varData.Single();

                    if (tempBill.BillStatus == CannibalizeBillStatus.单据已完成.ToString())
                    {
                        throw new Exception("单据已完成，无法删除");
                    }

                    if (tempBill.Proposer != BasicInfo.LoginName)
                    {
                        throw new Exception("只有申请人本人才能删除单据");
                    }
                }

                ctx.WS_CannibalizeBill.DeleteAllOnSubmit(varData);

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
        void ListControl(DepotManagementDataContext ctx, WS_CannibalizeBill billInfo, 
            List<WS_CannibalizeWSCode> listCannibalizeWSCode, DataTable list)
        {
            try
            {
                var varData = from a in ctx.WS_CannibalizeList
                              where a.BillNo == billInfo.BillNo
                              select a;

                ctx.WS_CannibalizeList.DeleteAllOnSubmit(varData);

                IWorkShopProductCode serverProductCode = ServerModuleFactory.GetServerModule<IWorkShopProductCode>();

                foreach (DataRow dr in list.Rows)
                {
                    WS_CannibalizeList tempList = new WS_CannibalizeList();

                    tempList.BillNo = billInfo.BillNo;
                    tempList.BatchNo = dr["批次号"].ToString();
                    tempList.GoodsID = Convert.ToInt32(dr["物品ID"]);
                    tempList.OperationCount = Convert.ToDecimal(dr["数量"]);
                    tempList.Remark = dr["备注"].ToString();

                    foreach (WS_CannibalizeWSCode item in listCannibalizeWSCode)
                    {
                        if (!serverProductCode.CheckProductCodeCount(billInfo.BillNo, item.WSCode, tempList.GoodsID, item.OperationType,
                            Convert.ToDecimal(dr["数量"])))
                        {
                            throw new Exception(UniversalFunction.GetGoodsMessage(tempList.GoodsID) + " 产品编码数量与操作数量不一致");
                        }
                    }

                    if (list.Select(" 物品ID = " + tempList.GoodsID + " AND 批次号 = '" + tempList.BatchNo + "'").Length > 1)
                    {
                        throw new Exception(UniversalFunction.GetGoodsMessage(tempList.GoodsID) + "【批次号】："
                            + tempList.BatchNo + "数据重复，请重新核对");
                    }

                    ctx.WS_CannibalizeList.InsertOnSubmit(tempList);
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
        void AssignmentValue(WS_CannibalizeBill bill, ref WS_CannibalizeBill returnBill)
        {
            returnBill.BillNo = bill.BillNo;
            returnBill.BillStatus = CannibalizeBillStatus.等待审核.ToString();
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
        /// <param name="listWSCode">操作车间列表信息</param>
        /// <param name="list">明细信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        public bool ProposeBill(WS_CannibalizeBill bill, List<WS_CannibalizeWSCode> listWSCode, DataTable list, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                WS_CannibalizeBill tempBill = new WS_CannibalizeBill();

                var varData = from a in ctx.WS_CannibalizeBill
                              where a.BillNo == bill.BillNo
                              select a;

                switch (varData.Count())
                {
                    case 0:
                        AssignmentValue(bill, ref tempBill);
                        ctx.WS_CannibalizeBill.InsertOnSubmit(tempBill);

                        if (listWSCode == null || listWSCode.Count != 2)
                        {
                            throw new Exception("单据的调入/调出车间有误，请重新确认");
                        }

                        ctx.WS_CannibalizeWSCode.InsertAllOnSubmit(listWSCode);
                        break;
                    case 1:
                        tempBill = varData.Single();

                        if (tempBill.BillStatus == CannibalizeBillStatus.单据已完成.ToString())
                        {
                            throw new Exception("单据状态错误，请重新确认单据状态");
                        }

                        AssignmentValue(bill, ref tempBill);

                        foreach (WS_CannibalizeWSCode item in listWSCode)
                        {
                            var varWSCode = from a in ctx.WS_CannibalizeWSCode
                                            where a.BillNo == tempBill.BillNo
                                            && a.OperationType == item.OperationType
                                            select a;

                            varWSCode.Single().WSCode = item.WSCode;
                        }

                        break;
                    default:
                        throw new Exception("数据错误，请重新确认");
                }

                ListControl(ctx, tempBill, listWSCode, list);

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
                var varData = from a in ctx.WS_CannibalizeBill
                              where a.BillNo == billNo
                              select a;
                if (varData.Count() != 1)
                {
                    throw new Exception("数据错误，请重新确认");
                }
                else
                {
                    WS_CannibalizeBill tempBill = varData.Single();

                    if (tempBill.BillStatus != CannibalizeBillStatus.等待审核.ToString())
                    {
                        throw new Exception("单据状态错误，请重新确认");
                    }

                    tempBill.BillStatus = CannibalizeBillStatus.等待确认.ToString();
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
        /// 获得车间调运库房信息表
        /// </summary>
        /// <param name="wsCode"></param>
        /// <returns></returns>
        WS_CannibalizeWSCode GetCannibalizeWSCode(WS_CannibalizeWSCode wsCode)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.WS_CannibalizeWSCode
                          where a.BillNo == wsCode.BillNo
                          && a.OperationType == wsCode.OperationType
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
                var varData = from a in ctx.WS_CannibalizeBill
                              where a.BillNo == billNo
                              select a;

                WS_CannibalizeBill tempBill = new WS_CannibalizeBill();

                if (varData.Count() == 1)
                {
                    tempBill = varData.Single();

                    if (tempBill.BillStatus != CannibalizeBillStatus.等待确认.ToString())
                    {
                        throw new Exception("单据状态错误，请重新确认");
                    }

                    tempBill.BillStatus = CannibalizeBillStatus.单据已完成.ToString();
                    tempBill.Affirm = BasicInfo.LoginName;
                    tempBill.AffirmDate = ServerTime.Time;

                }
                else
                {
                    throw new Exception("数据不唯一,请重新确认");
                }

                var varCannibalizeWSCode = from a in ctx.WS_CannibalizeWSCode
                                           where a.BillNo == tempBill.BillNo
                                           select a;

                ListControl(ctx, tempBill, varCannibalizeWSCode.ToList<WS_CannibalizeWSCode>(), list);
                ctx.SubmitChanges();

                var varOperationType = from a in varCannibalizeWSCode
                                       join b in ctx.BASE_SubsidiaryOperationType
                                       on a.OperationType equals b.OperationType
                                       orderby b.DepartmentType
                                       select a;

                var varGoodsList = from a in ctx.WS_CannibalizeList
                                   where a.BillNo == tempBill.BillNo
                                   select a;

                foreach (WS_CannibalizeList tempItem in varGoodsList)
                {
                    decimal unitPrice = 0;

                    foreach (WS_CannibalizeWSCode item in varOperationType)
                    {
                        WS_Subsidiary tempSubsidiary = new WS_Subsidiary();

                        tempSubsidiary.BatchNo = tempItem.BatchNo;
                        tempSubsidiary.BillNo = tempBill.BillNo;
                        tempSubsidiary.GoodsID = tempItem.GoodsID;
                        tempSubsidiary.OperationCount = tempItem.OperationCount;
                        tempSubsidiary.OperationType = item.OperationType;
                        tempSubsidiary.Remark = tempItem.Remark;
                        tempSubsidiary.WSCode = item.WSCode;
                        tempSubsidiary.Proposer = tempBill.Proposer;
                        tempSubsidiary.ProposerDate = (DateTime)tempBill.ProposerDate;
                        tempSubsidiary.Affirm = BasicInfo.LoginName;
                        tempSubsidiary.AffirmDate = ServerTime.Time;
                        tempSubsidiary.BillTime = ServerTime.Time;

                        WS_CannibalizeWSCode tempCanWSCode = new WS_CannibalizeWSCode();

                        tempCanWSCode.BillNo = tempSubsidiary.BillNo;

                        Service_Manufacture_WorkShop.IWorkShopStock serverStock =
                            Service_Manufacture_WorkShop.ServerModuleFactory.GetServerModule<Service_Manufacture_WorkShop.IWorkShopStock>();
                        switch (item.OperationType)
                        {
                            case (int)CE_SubsidiaryOperationType.车间调出:
                                tempCanWSCode.OperationType = (int)CE_SubsidiaryOperationType.车间调入;

                                WS_WorkShopStock tempWSStock = serverStock.GetStockSingleInfo(tempSubsidiary.WSCode, 
                                    tempSubsidiary.GoodsID, 
                                    tempSubsidiary.BatchNo);

                                unitPrice = tempWSStock == null ? 0 : tempWSStock.UnitPrice;
                                tempSubsidiary.UnitPrice = unitPrice;
                                break;
                            case (int)CE_SubsidiaryOperationType.车间调入:
                                tempCanWSCode.OperationType = (int)CE_SubsidiaryOperationType.车间调出;
                                tempSubsidiary.UnitPrice = unitPrice;
                                break;
                            default:
                                break;
                        }

                        tempCanWSCode = GetCannibalizeWSCode(tempCanWSCode);

                        if (tempCanWSCode == null)
                        {
                            throw new Exception("调运车间信息错误");
                        }

                        Service_Manufacture_WorkShop.IWorkShopBasic serverBasic =
                            Service_Manufacture_WorkShop.ServerModuleFactory.GetServerModule<Service_Manufacture_WorkShop.IWorkShopBasic>();
                        WS_WorkShopCode tempCode = serverBasic.GetWorkShopCodeInfo(tempCanWSCode.WSCode);

                        if (tempCode == null)
                        {
                            throw new Exception("获取车间名称失败");
                        }

                        tempSubsidiary.Applicant = tempCode.WSName;

                        serverStock.OperationSubsidiary(ctx, tempSubsidiary);
                    }
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
        public bool ReturnBill(string billNo, CannibalizeBillStatus billStatus, out string error, string rebackReason)
        {
            error = null;
            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            try
            {
                var varData = from a in dataContext.WS_CannibalizeBill
                              where a.BillNo == billNo
                              select a;

                string strMsg = "";

                if (varData.Count() != 1)
                {
                    throw new Exception("数据不唯一或者为空");
                }
                else
                {
                    WS_CannibalizeBill lnqTemp = varData.Single();

                    IBillMessagePromulgatorServer billMessageServer =
                        BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

                    billMessageServer.BillType = CE_BillTypeEnum.车间调运单.ToString();

                    switch (billStatus)
                    {
                        case CannibalizeBillStatus.新建单据:

                            strMsg = string.Format("{0}号车间调运单已回退，请您重新处理单据; 回退原因为"
                                + rebackReason, billNo);

                            billMessageServer.PassFlowMessage(billNo, strMsg, BillFlowMessage_ReceivedUserType.用户,
                                UniversalFunction.GetPersonnelCode(lnqTemp.Proposer));

                            break;
                        case CannibalizeBillStatus.等待审核:

                            strMsg = string.Format("{0}号车间调运单已回退，请您重新处理单据; 回退原因为"
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
