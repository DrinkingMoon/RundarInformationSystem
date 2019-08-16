/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  ProductLendReturnService.cs
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
using System.Text.RegularExpressions;
using System.Reflection;

namespace ServerModule
{
    /// <summary>
    /// 借贷单据状态
    /// </summary>
    public enum ProductLendReturnBillStatus
    {
        /// <summary>
        /// 单据状态
        /// </summary>
        新建单据,

        /// <summary>
        /// 单据状态
        /// </summary>
        等待审核,

        /// <summary>
        /// 单据状态
        /// </summary>
        等待确认,

        /// <summary>
        /// 单据状态
        /// </summary>
        单据已完成
    }

    /// <summary>
    /// 借贷业务状态
    /// </summary>
    public enum ProdutLendReturnBusinessType
    {
        /// <summary>
        /// 借方
        /// </summary>
        借方 = 1,

        /// <summary>
        /// 贷方
        /// </summary>
        贷方 = 0
    }

    /// <summary>
    /// 物品借贷服务
    /// </summary>
    public class ProductLendReturnService : IProductLendReturnService
    {
        /// <summary>
        /// 获得借贷信息
        /// </summary>
        /// <returns>返回Table</returns>
        public DataTable GetRecordInfo()
        {
            string strSql = "select * from View_S_ProductLendRecord";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得借贷账存信息列表
        /// </summary>
        /// <returns>返回对象列表</returns>
        public List<View_S_ProductLendRecord> GetListRecordInfo()
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            return (from a in ctx.View_S_ProductLendRecord select a).ToList();
        }

        /// <summary>
        /// 获得借贷流水账信息列表
        /// </summary>
        /// <returns>返回对象列表</returns>
        public List<View_S_ProductLendReturnDetail> GetListDetailInfo()
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            return (from a in ctx.View_S_ProductLendReturnDetail select a).ToList();
        }

        /// <summary>
        /// 获得单条借贷记录
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="DebtorCode">借方代码</param>
        /// <param name="CreditCode">贷方代码</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <param name="provider">供应商</param>
        /// <returns>返回单条LNQ数据集</returns>
        public S_ProductLendRecord GetStockSingleInfo(DepotManagementDataContext ctx, string DebtorCode, string CreditCode, int goodsID, 
            string batchNo, string provider)
        {
            var varData = from a in ctx.S_ProductLendRecord
                          where a.CreditCode == CreditCode
                          && a.DebtorCode == DebtorCode
                          && a.GoodsID == goodsID
                          && a.BatchNo == batchNo
                          && a.Provider == provider
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
        /// 获得单条借贷记录
        /// </summary>
        /// <param name="DebtorCode">借方代码</param>
        /// <param name="CreditCode">贷方代码</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <param name="provider">供应商</param>
        /// <returns>返回单条LNQ数据集</returns>
        public S_ProductLendRecord GetStockSingleInfo(string DebtorCode, string CreditCode, int goodsID, string batchNo, string provider)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.S_ProductLendRecord
                          where a.CreditCode == CreditCode
                          && a.DebtorCode == DebtorCode
                          && a.GoodsID == goodsID
                          && a.BatchNo == batchNo
                          && a.Provider == provider
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
        /// 操作借贷明细账
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="detail">借贷明细LINQ数据集</param>
        public void OperationDetail(DepotManagementDataContext ctx, S_ProductLendReturnDetail detail)
        {
            BASE_SubsidiaryOperationType tempOperationType = UniversalFunction.GetSubsidiaryOperationType(detail.OperationType);

            if (tempOperationType == null)
            {
                throw new Exception("明细账操作类型错误，请重新确认");
            }
            else if (tempOperationType.LendReturnType == null)
            {
                throw new Exception("借贷业务错误");
            }

            var varData = from a in ctx.S_ProductLendReturnDetail
                          where a.Debtor == detail.Debtor
                          && a.Credit == detail.Credit
                          && a.GoodsID == detail.GoodsID
                          && a.BatchNo == detail.BatchNo
                          && a.Provider == detail.Provider
                          select a;

            if (varData.Count() > 0)
            {
                throw new Exception("相同单据中，物品重复，请重新确认");
            }
            else
            {
                ctx.S_ProductLendReturnDetail.InsertOnSubmit(detail);
            }

        }

        /// <summary>
        /// 操作借货记录
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="record">LINQ数据集</param>
        public void OperationRecord(DepotManagementDataContext ctx, S_ProductLendRecord record)
        {
            var varData = from a in ctx.S_ProductLendRecord
                          where a.CreditCode == record.CreditCode
                          && a.DebtorCode == record.DebtorCode
                          && a.GoodsID == record.GoodsID
                          && a.BatchNo == record.BatchNo
                          && a.Provider == record.Provider
                          select a;

            if (varData.Count() == 0)
            {
                if (!Convert.ToBoolean(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.借货账目允许小于0]))
                {
                    if (record.RecordCount < 0)
                    {
                        throw new Exception("不能添加负数的借货记录");
                    }
                }

                ctx.S_ProductLendRecord.InsertOnSubmit(record);
            }
            else if (varData.Count() == 1)
            {
                S_ProductLendRecord tempRecord = varData.Single();

                tempRecord.RecordCount += record.RecordCount;

                if (!Convert.ToBoolean(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.借货账目允许小于0]))
                {
                    if (tempRecord.RecordCount < 0)
                    {
                        throw new Exception("借货数量不能小于0");
                    }
                }
            }
            else
            {
                throw new Exception("借货记录数据重复");
            }
        }

        /// <summary>
        /// 操作明细与借贷记录
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="detail">明细信息</param>
        public void OperationDetailRecord(DepotManagementDataContext ctx,S_ProductLendReturnDetail detail)
        {
            BASE_SubsidiaryOperationType tempOperationType = UniversalFunction.GetSubsidiaryOperationType(detail.OperationType);

            if (tempOperationType == null)
            {
                throw new Exception("明细账操作类型错误，请重新确认");
            }
            else if (tempOperationType.LendReturnType == null)
            {
                throw new Exception("借贷业务错误");
            }

            var varData = from a in ctx.S_ProductLendReturnDetail
                          where a.Debtor == detail.Debtor
                          && a.Credit == detail.Credit
                          && a.GoodsID == detail.GoodsID
                          && a.Provider == detail.Provider
                          && a.BatchNo == detail.BatchNo
                          && a.BillNo == detail.BillNo
                          select a;

            if (varData.Count() > 0)
            {
                throw new Exception("相同单据中，物品重复，请重新确认");
            }
            else
            {
                ctx.S_ProductLendReturnDetail.InsertOnSubmit(detail);

                S_ProductLendRecord tempRecord = new S_ProductLendRecord();

                tempRecord.Provider = detail.Provider;
                tempRecord.BatchNo = detail.BatchNo;
                tempRecord.CreditCode = detail.Credit;
                tempRecord.DebtorCode = detail.Debtor;
                tempRecord.GoodsID = detail.GoodsID;
                tempRecord.RecordCount = tempOperationType.LendReturnType == true ? detail.OperationCount : -detail.OperationCount;

                OperationRecord(ctx, tempRecord);
            }
        }

        /// <summary>
        /// 查询车间物品借贷流水账
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="provider">供应商</param>
        /// <param name="batchNo">批次号</param>
        /// <param name="wsName">车间代码</param>
        /// <param name="storage">库房代码</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="error">错误信息</param>
        /// <returns>返回Table</returns>
        public DataTable QueryRunningAccount(int goodsID, string provider, string batchNo, string wsName, string storage,
            DateTime startTime, DateTime endTime, out string error)
        {
            Hashtable parameters = new Hashtable();

            parameters.Add("GoodsID", GeneralFunction.ChangeType(goodsID, "Int"));
            parameters.Add("Provider", GeneralFunction.ChangeType(provider, "String"));
            parameters.Add("BatchNo", GeneralFunction.ChangeType(batchNo, "String"));
            parameters.Add("WSName", GeneralFunction.ChangeType(wsName, "String"));
            parameters.Add("Storage", GeneralFunction.ChangeType(storage, "String"));
            parameters.Add("StartTime", GeneralFunction.ChangeType(startTime, "DateTime"));
            parameters.Add("EndTime", GeneralFunction.ChangeType(endTime, "DateTime"));

            return GlobalObject.DatabaseServer.QueryInfoPro("S_ProductLendReturnRunningAccount",
                parameters, out error);
        }
    }
}
