/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  WorkShopStock.cs
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
using ServerModule;
using System.Reflection;

namespace Service_Manufacture_WorkShop
{
    /// <summary>
    /// 车间库存操作服务
    /// </summary>
    public class WorkShopStock : IWorkShopStock
    {
        /// <summary>
        /// 获得库存信息
        /// </summary>
        /// <returns>返回Table</returns>
        public DataTable GetStockInfo()
        {
            string strSql = "select * from View_WS_WorkShopStock";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得单条车间库存记录
        /// </summary>
        /// <param name="wsCode">车间代码</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <returns>返回单条LNQ数据集</returns>
        public WS_WorkShopStock GetStockSingleInfo(DepotManagementDataContext ctx, string wsCode, int goodsID, string batchNo)
        {
            var varData = from a in ctx.WS_WorkShopStock
                          where a.WSCode == wsCode
                          && a.GoodsID == goodsID
                          && a.BatchNo == batchNo
                          select a;

            if (varData.Count() > 0)
            {
                return varData.First();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获得单条车间库存记录
        /// </summary>
        /// <param name="wsCode">车间代码</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <returns>返回单条LNQ数据集</returns>
        public WS_WorkShopStock GetStockSingleInfo(string wsCode,int goodsID ,string batchNo)
        {
            string strSql = "select * from WS_WorkShopStock where WSCode = '"+ wsCode 
                +"' and GoodsID = "+ goodsID +" and BatchNo = '"+ batchNo +"'";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dtTemp.Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return GlobalObject.GeneralFunction.ReflectiveEntity<WS_WorkShopStock>(dtTemp.Rows[0]);
            }
        }

        /// <summary>
        /// 操作明细账（并且操作库存）
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="subsidiary">明细账对象</param>
        public void OperationSubsidiary(DepotManagementDataContext ctx ,WS_Subsidiary subsidiary)
        {
            if (!Convert.ToBoolean(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.开启车间管理模块]))
            {
                return;
            }

            Service_Manufacture_WorkShop.IWorkShopProductCode serverProduct =
                Service_Manufacture_WorkShop.ServerModuleFactory.GetServerModule<Service_Manufacture_WorkShop.IWorkShopProductCode>();

            BASE_SubsidiaryOperationType tempOperationType = UniversalFunction.GetSubsidiaryOperationType(subsidiary.OperationType);

            if (tempOperationType == null)
            {
                throw new Exception("明细账操作类型错误，请重新确认");
            }

            serverProduct.OperatorProductCodeStock(ctx, subsidiary.BillNo,
                subsidiary.GoodsID, subsidiary.OperationType);

            ctx.WS_Subsidiary.InsertOnSubmit(subsidiary);
            WS_WorkShopStock tempStock = new WS_WorkShopStock();

            tempStock.BatchNo = subsidiary.BatchNo;
            tempStock.GoodsID = subsidiary.GoodsID;
            tempStock.StockCount = (bool)tempOperationType.DepartmentType ? subsidiary.OperationCount : -subsidiary.OperationCount;
            tempStock.WSCode = subsidiary.WSCode;

            PlusReductionStock(ctx, tempStock);
        }

        /// <summary>
        /// 操作明细账（并且操作库存）
        /// </summary>
        /// <param name="subsidiary">明细账对象</param>
        public void OperationSubsidiary(WS_Subsidiary subsidiary)
        {
            if (!Convert.ToBoolean(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.开启车间管理模块]))
            {
                return;
            }

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            BASE_SubsidiaryOperationType tempOperationType = UniversalFunction.GetSubsidiaryOperationType(subsidiary.OperationType);

            if (tempOperationType == null)
            {
                throw new Exception("明细账操作类型错误，请重新确认");
            }

            var varData = from a in ctx.WS_Subsidiary
                          where a.BillNo == subsidiary.BillNo
                          && a.GoodsID == subsidiary.GoodsID
                          && a.WSCode == subsidiary.WSCode
                          && a.BatchNo == subsidiary.BatchNo
                          select a;

            if (varData.Count() > 0)
            {
                throw new Exception(UniversalFunction.GetGoodsMessage(subsidiary.GoodsID) + " 【批次号】: "
                        + subsidiary.BatchNo + " 相同单据中，物品重复，请重新确认");
            }
            else
            {
                subsidiary.BillTime = ServerTime.Time;
                ctx.WS_Subsidiary.InsertOnSubmit(subsidiary);

                WS_WorkShopStock tempStock = new WS_WorkShopStock();

                tempStock.BatchNo = subsidiary.BatchNo;
                tempStock.GoodsID = subsidiary.GoodsID;
                tempStock.StockCount = (bool)tempOperationType.DepartmentType ? subsidiary.OperationCount : -subsidiary.OperationCount;
                tempStock.WSCode = subsidiary.WSCode;

                PlusReductionStock(ctx, tempStock);
            }

            ctx.SubmitChanges();
        }

        /// <summary>
        /// 加减库存
        /// </summary>
        /// <param name="workShopStock">加减库存记录</param>
        public void PlusReductionStock(WS_WorkShopStock workShopStock)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.WS_WorkShopStock
                          where a.GoodsID == workShopStock.GoodsID
                          && a.BatchNo == workShopStock.BatchNo
                          && a.WSCode == workShopStock.WSCode
                          select a;


            if (varData.Count() == 0)
            {
                if (workShopStock.StockCount < 0)
                {
                    throw new Exception(UniversalFunction.GetGoodsMessage(workShopStock.GoodsID) + " 【批次号】: " 
                        + workShopStock.BatchNo + " 操作后库存不允许小于0 ,请重新确认");
                }

                ctx.WS_WorkShopStock.InsertOnSubmit(workShopStock);
            }
            else if(varData.Count() == 1)
            {
                WS_WorkShopStock lnqTemp = varData.Single();

                lnqTemp.StockCount = lnqTemp.StockCount + workShopStock.StockCount;

                if (lnqTemp.StockCount < 0)
                {
                    throw new Exception(UniversalFunction.GetGoodsMessage(workShopStock.GoodsID) + " 【批次号】: " 
                        + workShopStock.BatchNo + " 操作后库存不允许小于0 ,请重新确认");
                }
            }
            else
            {
                throw new Exception(UniversalFunction.GetGoodsMessage(workShopStock.GoodsID) + " 【批次号】: " 
                    + workShopStock.BatchNo + " 库存表数据重复");
            }

            ctx.SubmitChanges();
        }

        /// <summary>
        /// 加减库存
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="workShopStock">加减库存记录</param>
        public void PlusReductionStock(DepotManagementDataContext ctx, WS_WorkShopStock workShopStock)
        {
            var varData = from a in ctx.WS_WorkShopStock
                          where a.GoodsID == workShopStock.GoodsID
                          && a.BatchNo == workShopStock.BatchNo
                          && a.WSCode == workShopStock.WSCode
                          select a;

            if (varData.Count() == 0)
            {
                if (workShopStock.StockCount < 0)
                {
                    throw new Exception(UniversalFunction.GetGoodsMessage(workShopStock.GoodsID) + " 【批次号】: "
                        + workShopStock.BatchNo + " 操作后库存不允许小于0 ,请重新确认");
                }

                ctx.WS_WorkShopStock.InsertOnSubmit(workShopStock);
            }
            else if (varData.Count() == 1)
            {
                WS_WorkShopStock lnqTemp = varData.Single();

                lnqTemp.StockCount = lnqTemp.StockCount + workShopStock.StockCount;

                if (lnqTemp.StockCount < 0)
                {
                    throw new Exception(UniversalFunction.GetGoodsMessage(workShopStock.GoodsID) + " 【批次号】: "
                        + workShopStock.BatchNo + " 操作后库存不允许小于0 ,请重新确认");
                }
            }
            else
            {
                throw new Exception(UniversalFunction.GetGoodsMessage(workShopStock.GoodsID) + " 【批次号】: "
                        + workShopStock.BatchNo + " 库存表数据重复");
            }
        }

        /// <summary>
        /// 查询车间物品流程账
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <param name="wsCode">车间代码</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="error">错误信息</param>
        /// <returns>返回Table</returns>
        public DataTable QueryRunningAccount(int goodsID, string batchNo, string wsCode, 
            DateTime startTime, DateTime endTime, out string error)
        {
            Hashtable parameters = new Hashtable();

            parameters.Add("GoodsID", GeneralFunction.ChangeType(goodsID, "Int"));
            parameters.Add("BatchNo", GeneralFunction.ChangeType(batchNo, "String"));
            parameters.Add("WSCode", GeneralFunction.ChangeType(wsCode, "String"));
            parameters.Add("StartTime", GeneralFunction.ChangeType(startTime, "DateTime"));
            parameters.Add("EndTime", GeneralFunction.ChangeType(endTime, "DateTime"));

            return GlobalObject.DatabaseServer.QueryInfoPro("WS_WorkShopRunningAccount",
                parameters, out error);
        }
    }
}
