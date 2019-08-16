/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  BusinessOperation.cs
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
using ServerModule;
using DBOperate;
using PlatformManagement;
using GlobalObject;

namespace Service_Peripheral_External
{
    /// <summary>
    /// 业务明细服务
    /// </summary>
    class BusinessOperation : IBusinessOperation
    {
        /// <summary>
        /// 操作业务明细与库存
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="detailAccount">业务明细数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool OperationDetailAndStock(DepotManagementDataContext dataContext, Out_DetailAccount detailAccount,out string error)
        {
            error = null;

            try
            {
                var varData = from a in dataContext.Out_DetailAccount
                              where a.Bill_ID == detailAccount.Bill_ID
                              && a.GoodsID == detailAccount.GoodsID
                              && a.SecStorageID == detailAccount.SecStorageID
                              && a.StorageID == detailAccount.StorageID
                              select a;

                if (varData.Count() > 0)
                {
                    error = "数据重复";
                    return false;
                }
                else
                {
                    if (!OperationStock(dataContext,detailAccount,out error))
                    {
                        return false;
                    }

                    dataContext.Out_DetailAccount.InsertOnSubmit(detailAccount);
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
        /// 操作库存
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="outDetail">业务明细数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool OperationStock(DepotManagementDataContext dataContext, Out_DetailAccount outDetail, out string error)
        {
            error = null;

            try
            {
                if (IntegrativeQuery.IsSalesStorage( outDetail.SecStorageID))
                {
                    return true;
                }

                var varData = from a in dataContext.Out_Stock
                              where a.GoodsID == outDetail.GoodsID
                              && a.SecStorageID == outDetail.SecStorageID
                              && a.StorageID == outDetail.StorageID
                              select a;

                F_GoodsPlanCost lnqGoods = IntegrativeQuery.QueryGoodsInfo(Convert.ToInt32( outDetail.GoodsID));

                if (varData.Count() == 0)
                {
                    if (outDetail.OperationCount < 0)
                    {
                        error = "不能添加负数的库存记录" + "[" + lnqGoods.GoodsCode + "] ["+ lnqGoods.GoodsName +"] ["+ lnqGoods.Spec +"]";
                        return false;
                    }
                    else
                    {
                        Out_Stock lnqStock = new Out_Stock();

                        lnqStock.GoodsID = Convert.ToInt32(outDetail.GoodsID);
                        lnqStock.Remark = outDetail.Remark;
                        lnqStock.SecStorageID = outDetail.SecStorageID;
                        lnqStock.StockQty = Convert.ToDecimal(outDetail.OperationCount);
                        lnqStock.Date = ServerTime.Time;
                        lnqStock.Personnel = BasicInfo.LoginName;
                        lnqStock.StorageID = outDetail.StorageID;

                        dataContext.Out_Stock.InsertOnSubmit(lnqStock);
                    }
                }
                else if (varData.Count() == 1)
                {
                    Out_Stock lnqStock = varData.Single();

                    decimal dcSum = Convert.ToDecimal(lnqStock.StockQty) + Convert.ToDecimal(outDetail.OperationCount);

                    if (dcSum < 0)
                    {
                        error = "不允许库存数量为负数" + "[" + lnqGoods.GoodsCode + "] [" + lnqGoods.GoodsName + "] [" + lnqGoods.Spec + "]";
                        return false;
                    }
                    else
                    {
                        lnqStock.StockQty = Convert.ToDecimal(lnqStock.StockQty) + Convert.ToDecimal(outDetail.OperationCount);
                        lnqStock.Date = ServerTime.Time;
                        lnqStock.Personnel = BasicInfo.LoginName;
                    }
                }
                else
                {
                    error = "库存数据不唯一" + "[" + lnqGoods.GoodsCode + "] [" + lnqGoods.GoodsName + "] [" + lnqGoods.Spec + "]";
                    return false;
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
        /// 获得库房视图
        /// </summary>
        /// <returns>返回Table</returns>
        public DataTable GetStockInfo()
        {
            string strSql = "select * from View_Out_Stock ORDER BY 所属库房ID, 物品ID, 所属账务库房ID";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 操作库存信息
        /// </summary>
        /// <param name="stockInfo">库存数据集</param>
        /// <param name="operationType">操作类型 添加，删除, 修改</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool OperationStockInfo(Out_Stock stockInfo,string operationType, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.Out_Stock
                              where a.GoodsID == stockInfo.GoodsID
                              && a.SecStorageID == stockInfo.SecStorageID
                              && a.StorageID == stockInfo.StorageID
                              select a;

                switch (operationType)
                {
                    case "添加":

                        if (varData.Count() != 0)
                        {
                            error = "不能重复插入库存";
                            return false;
                        }
                        else
                        {
                            dataContext.Out_Stock.InsertOnSubmit(stockInfo);
                        }

                        break;
                    case "删除":

                        if (varData.Count() != 1)
                        {
                            error = "数据不唯一";
                            return false;
                        }
                        else
                        {
                            dataContext.Out_Stock.DeleteAllOnSubmit(varData);
                        }

                        break;
                    case "修改":

                        var dataUpdate = from a in dataContext.Out_Stock
                                         where a.ID == stockInfo.ID
                                         select a;

                        Out_Stock lnqStock = dataUpdate.Single();

                        lnqStock.Date = stockInfo.Date;
                        lnqStock.GoodsID = stockInfo.GoodsID;
                        lnqStock.Personnel = BasicInfo.LoginName;
                        lnqStock.Remark = stockInfo.Remark;
                        lnqStock.SecStorageID = stockInfo.SecStorageID;
                        lnqStock.StockQty = stockInfo.StockQty;
                        lnqStock.StorageID = stockInfo.StorageID;

                        break;
                    default:
                        break;
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

        /// <summary>
        /// 查询外部物品流水账
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="storageID">库房代码</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="error">错误信息</param>
        /// <returns>返回Table</returns>
        public DataTable QueryRunningAccount(int goodsID, string storageID,
            DateTime startTime, DateTime endTime, out string error)
        {
            Hashtable parameters = new Hashtable();

            parameters.Add("GoodsID", GeneralFunction.ChangeType(goodsID, "Int"));
            parameters.Add("SecStorageID", GeneralFunction.ChangeType(storageID, "String"));
            parameters.Add("StartDate", GeneralFunction.ChangeType(startTime, "DateTime"));
            parameters.Add("EndDate", GeneralFunction.ChangeType(endTime, "DateTime"));

            return GlobalObject.DatabaseServer.QueryInfoPro("Out_RunningAccount",
                parameters, out error);
        }
    }
}
