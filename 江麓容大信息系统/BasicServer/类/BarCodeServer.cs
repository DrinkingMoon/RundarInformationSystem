/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  InDepotGoodsBarCodeServer.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2010/07/17
 * 开发平台:  Visual C# 2008
 * 用于    :  仓库管理软件
 *----------------------------------------------------------------------------
 * 描述 : 
 * 其它 :
 *----------------------------------------------------------------------------
 * 公共信息: 参见系统'类帮助文档'
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2010/07/17 8:54:12 作者: 夏石友 当前版本: V1.00
 *        修改说明: 创建
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using DBOperate;
using PlatformManagement;

namespace ServerModule
{
    /// <summary>
    /// 物品条形码管理类
    /// </summary>
    class BarCodeServer : BasicServer, IBarCodeServer
    {
        /// <summary>
        /// 新建条码
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <param name="storageID">库房ID</param>
        /// <param name="provider">供应商编码</param>
        /// <returns>返回新建的条码号</returns>
        private int CreateBarCode(int goodsID, string batchNo, string storageID, string provider)
        {
            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            S_InDepotGoodsBarCodeTable lnqNew = new S_InDepotGoodsBarCodeTable();

            lnqNew.GoodsID = goodsID;
            lnqNew.BatchNo = batchNo;
            lnqNew.Provider = provider;
            lnqNew.StorageID = storageID;

            dataContext.S_InDepotGoodsBarCodeTable.InsertOnSubmit(lnqNew);

            dataContext.SubmitChanges();

            return GetBarCode(goodsID, batchNo, storageID, provider);
        }

        /// <summary>
        /// 获得条形码
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <param name="storageID">库房ID</param>
        /// <param name="provider">供应商编码</param>
        /// <returns>返回整型的条形码序号即条码号</returns>
        /// <remarks>
        /// 2014-05-20, 夏石友，参数增加“provider”
        /// </remarks>
        public int GetBarCode(int goodsID, string batchNo, string storageID, string provider)
        {
            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            var result = from r in dataContext.S_InDepotGoodsBarCodeTable
                         where r.GoodsID == goodsID
                         && r.BatchNo == batchNo
                         && r.Provider == provider
                         && r.StorageID == storageID
                         select r;

            if (result.Count() == 0)
            {
                return CreateBarCode(goodsID, batchNo, storageID, provider);
            }
            else
            {
                return result.Single().ID;
            }
        }

        /// <summary>
        /// 获得条形码是否存在表中
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="storageID">库房ID</param>
        /// <param name="batchNo">批次号</param>
        /// <param name="provider">供应商</param>
        /// <returns>条形码存在返回True,条形码不存在返回False</returns>
        /// <remarks>
        /// 2014-05-20, 夏石友，参数增加“provider”
        /// </remarks>
        bool IsExists(DepotManagementDataContext dataContext, int goodsID, string storageID, string batchNo, string provider)
        {
            var result = from a in dataContext.S_InDepotGoodsBarCodeTable
                         where a.GoodsID == goodsID
                         && a.BatchNo == batchNo
                         && a.Provider == provider
                         && a.StorageID == storageID.Trim()
                         select a;

            if (result.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获得条形码是否存在表中
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="storageID">库房ID</param>
        /// <param name="batchNo">批次号</param>
        /// <param name="provider">供应商</param>
        /// <returns>条形码存在返回True,条形码不存在返回False</returns>
        /// <remarks>
        /// 2014-05-20, 夏石友，参数增加“provider”
        /// </remarks>
        public bool IsExists(int goodsID, string storageID, string batchNo, string provider)
        {
            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            var result = from a in dataContext.S_InDepotGoodsBarCodeTable
                         where a.GoodsID == goodsID
                         && a.BatchNo == batchNo
                         && a.Provider == provider
                         && a.StorageID == storageID.Trim()
                         select a;

            if (result.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 在添加库存信息时添加条形码信息
        /// </summary>
        /// <param name="context">数据库上下文</param>
        /// <param name="stockInfo">库存表的一条库存信息</param>
        public void Add(DepotManagementDataContext context, S_Stock stockInfo)
        {
            if (IsExists(context, stockInfo.GoodsID, stockInfo.StorageID, stockInfo.BatchNo, stockInfo.Provider))
            {
                return;
            }

            S_InDepotGoodsBarCodeTable lnqBarCodeInfo = new S_InDepotGoodsBarCodeTable();

            lnqBarCodeInfo.GoodsID = (int)stockInfo.GoodsID;
            lnqBarCodeInfo.Provider = stockInfo.Provider;
            lnqBarCodeInfo.BatchNo = stockInfo.BatchNo;
            lnqBarCodeInfo.StorageID = stockInfo.StorageID;

            context.S_InDepotGoodsBarCodeTable.InsertOnSubmit(lnqBarCodeInfo);
        }

        /// <summary>
        /// 向条形码管理表中添加一条记录
        /// </summary>
        /// <param name="barcode">要添加的一条条形码管理视图条形码信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>添加成功返回True，添加失败返回False</returns>
        /// <remarks>打印条形码时如果找不到此物品的条形码时直接生成条形码用</remarks>
        public bool Add(S_InDepotGoodsBarCodeTable barcode, out string error)
        {
            error = null;

            try
            {
                if (IsExists(barcode.GoodsID, barcode.StorageID, barcode.BatchNo, barcode.Provider))
                    return true;

                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                dataContxt.S_InDepotGoodsBarCodeTable.InsertOnSubmit(barcode);
                dataContxt.SubmitChanges();

                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 在更新条形码管理表中的信息
        /// </summary>
        /// <param name="context">数据库上下文</param>
        /// <param name="oldInfo">旧库存信息</param>
        /// <param name="newInfo">新库存信息</param>
        public void Update(DepotManagementDataContext context, S_Stock oldInfo, S_Stock newInfo)
        {
            if (IsExists(oldInfo.GoodsID, oldInfo.StorageID, oldInfo.BatchNo, oldInfo.Provider))
            {
                var result = from r in context.S_InDepotGoodsBarCodeTable
                             where r.GoodsID == oldInfo.GoodsID
                             && r.Provider == oldInfo.Provider
                             && r.BatchNo == oldInfo.BatchNo
                             && r.StorageID == oldInfo.StorageID
                             select r;

                if (result.Count() == 1)
                {
                    S_InDepotGoodsBarCodeTable lnqBarcode = result.Single();

                    lnqBarcode.GoodsID = (int)newInfo.GoodsID;
                    lnqBarcode.Provider = newInfo.Provider;
                    lnqBarcode.BatchNo = newInfo.BatchNo;
                    lnqBarcode.StorageID = newInfo.StorageID;
                }
            }
        }

        /// <summary>
        /// 删除条形码管理表中的信息
        /// </summary>
        /// <param name="context">数据库上下文</param>
        /// <param name="stockInfo">库存信息</param>
        public void Delete(DepotManagementDataContext context, S_Stock stockInfo)
        {
            var result = from r in context.S_InDepotGoodsBarCodeTable
                         where r.GoodsID == stockInfo.GoodsID
                         && r.Provider == stockInfo.Provider
                         && r.BatchNo == stockInfo.BatchNo
                         && r.StorageID == stockInfo.StorageID
                         select r;

            if (result.Count() > 0)
            {
                context.S_InDepotGoodsBarCodeTable.DeleteAllOnSubmit(result);
            }
        }

        /// <summary>
        /// 获取条形码信息
        /// </summary>
        /// <param name="returnTable">返回的查询结果</param>
        /// <param name="whereCondition">where语句的查询条件</param>
        /// <param name="error">返回False时输出的错误信息</param>
        /// <returns>查询成功返回True，查询失败返回False</returns>
        public bool GetBarCodeInfo(out IQueryResult returnTable, string whereCondition, out string error)
        {
            returnTable = null;
            error = null;

            IAuthorization serverAuthorization = PlatformFactory.GetObject<IAuthorization>();
            IQueryResult qr = serverAuthorization.Query("条形码查询", null, whereCondition);

            if (!qr.Succeeded)
            {
                error = qr.Error;
                return false;
            }

            returnTable = qr;
            return true;
        }

        /// <summary>
        /// 获取条形码管理表
        /// </summary>
        /// <param name="goodsCode">图号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <param name="provider">供应商</param>
        /// <param name="batchCode">批次号</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>找到则返回一条条形码管理视图记录，否则返回null</returns>
        public View_S_InDepotGoodsBarCodeTable GetBarCodeInfo(string goodsCode, string goodsName, string spec, string provider, string batchCode, string storageID)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.View_S_InDepotGoodsBarCodeTable
                         where r.图号型号 == goodsCode
                         && r.物品名称 == goodsName
                         && r.规格 == spec
                         && r.供货单位 == provider
                         && r.批次号 == batchCode
                         && r.库房代码 == storageID
                         select r;

            if (result.Count() == 0)
            {
                return null;
            }

            return result.Single();
        }

        /// <summary>
        /// 获取某一条形码的货物信息
        /// </summary>
        /// <param name="barCode">条形码序号即条形码</param>
        /// <param name="goodsInfo">物品信息</param>
        /// <param name="error">获取失败时返回的错误信息</param>
        /// <returns>获取成功返回True，获取失败返回False</returns>
        public bool GetData(int barCode, out S_InDepotGoodsBarCodeTable goodsInfo, out string error)
        {
            goodsInfo = null;
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from r in dataContxt.S_InDepotGoodsBarCodeTable
                             where r.ID == barCode
                             select r;

                if (result.Count() == 0)
                {
                    error = "条形码有误";
                    return false;
                }

                goodsInfo = result.Single();

                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 获取某一条形码的货物信息
        /// </summary>
        /// <param name="barCode">条形码序号即条形码</param>
        /// <param name="goodsInfo">物品信息</param>
        /// <param name="error">获取失败时返回的错误信息</param>
        /// <returns>获取成功返回True,获取失败返回False</returns>
        public bool GetData(int barCode, out View_S_InDepotGoodsBarCodeTable goodsInfo, out string error)
        {
            goodsInfo = null;

            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from r in dataContxt.View_S_InDepotGoodsBarCodeTable
                             where r.条形码 == barCode
                             select r;

                if (result.Count() == 0)
                {
                    error = "条形码有误";
                    return false;
                }

                goodsInfo = result.Single();

                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 获取某一条形码的货物信息
        /// </summary>
        /// <param name="barCode">条形码</param>
        /// <param name="returnTable">获取成功时返回所获取的某一条形码的货物信息</param>
        /// <param name="error">获取失败时返回错误信息</param>
        /// <returns>获取成功时返回True,获取失败时返回False</returns>
        public bool GetData(string barCode, out DataTable returnTable, out string error)
        {
            returnTable = null;
            error = null;

            Hashtable paramTable = new Hashtable();
            paramTable.Add("@BarCode", barCode);

            DataSet returnDataSet = new DataSet();
            Dictionary<OperateCMD, object> dicOperateCMD =
                m_dbOperate.RunProc_CMD("SelSomeGoodsInfoFromS_InDepotGoodsBarCodeTable", returnDataSet, paramTable);

            if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
            {
                error = Convert.ToString(dicOperateCMD[OperateCMD.Return_Errmsg]);
                return false;
            }

            returnTable = returnDataSet.Tables[0];
            return true;
        }

        public void AddBoardBarCodeRecord(P_PrintBoardForVehicleBarcode barCode)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            ctx.P_PrintBoardForVehicleBarcode.InsertOnSubmit(barCode);

            ctx.SubmitChanges();
        }

        public void DeleteBoardBarCodeRecord(string barCode)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.P_PrintBoardForVehicleBarcode
                          where a.BarCode == barCode
                          select a;

            ctx.P_PrintBoardForVehicleBarcode.DeleteAllOnSubmit(varData);
            ctx.SubmitChanges();
        }

        public DataTable ShowBoardBarCodeRecord()
        {
            string strSql = "select a.BarCode as 条形码, a.GoodsID as 物品ID, b.图号型号, b.物品名称,  "+
                            " b.规格, a.GoodsCount as 数量, a.PrintCount as 打印数量 "+
                            " from P_PrintBoardForVehicleBarcode as a inner join View_F_GoodsPlanCost as b on a.GoodsID = b.序号";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }
    }
}