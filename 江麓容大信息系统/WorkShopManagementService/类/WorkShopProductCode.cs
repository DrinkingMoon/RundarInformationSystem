/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  WorkShopProductCode.cs
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
    /// 车间箱体编号操作服务
    /// </summary>
    public class WorkShopProductCode : IWorkShopProductCode
    {
        /// <summary>
        /// 车间基础服务
        /// </summary>
        Service_Manufacture_WorkShop.IWorkShopBasic m_serverWSBasic = 
            Service_Manufacture_WorkShop.ServerModuleFactory.GetServerModule<Service_Manufacture_WorkShop.IWorkShopBasic>();

        /// <summary>
        /// 获得车间产品箱体编号信息
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="productCode">箱体编号</param>
        /// <param name="storageID">库房代码</param>
        /// <returns>返回LNQ</returns>
        public WS_ProductCodeStock GetProductCodeStockInfo(int goodsID, string productCode, string storageID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.WS_ProductCodeStock
                          where a.GoodsID == goodsID
                          && a.ProductCode == productCode
                          && a.StorageID == storageID
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
        /// 检测录入的编码数量是否一致
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="wsCode">车间代码</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="operationType">操作类型</param>
        /// <param name="checkCount">操作数量</param>
        /// <returns>返回True：一致，False 不一致</returns>
        public bool CheckProductCodeCount(string billNo, string wsCode, int goodsID, int operationType, decimal checkCount)
        {
            if (UniversalFunction.IsProduct(goodsID))
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.WS_ProductCodeDetail
                              where a.BillNo == billNo
                              && a.GoodsID == goodsID
                              && a.OperationType == operationType
                              && a.StorageID == wsCode
                              select a;
                if ((int)checkCount != varData.Count())
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 箱体编码明细操作
        /// </summary>
        /// <param name="listProductCodes">箱体编码列表</param>
        /// <param name="billNo">单据号</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="dicInfo">字典信息</param>
        public void OperatorProductCodeDetail(DataTable listProductCodes, string billNo, int goodsID, 
            Dictionary<string,CE_SubsidiaryOperationType> dicInfo)
        {
            if (!Convert.ToBoolean(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.开启车间管理模块]))
            {
                return;
            }

            List<WS_ProductCodeDetail> listDetail = new List<WS_ProductCodeDetail>();

            foreach (KeyValuePair<string,CE_SubsidiaryOperationType> item in dicInfo)
            {
                foreach (DataRow dr in listProductCodes.Rows)
                {
                    WS_ProductCodeDetail detail = new WS_ProductCodeDetail();

                    detail.BillNo = billNo;
                    detail.GoodsID = goodsID;
                    detail.IsUse = false;
                    detail.OperationType = (int)item.Value;
                    detail.ProductCode = dr["ProductCode"].ToString();
                    detail.StorageID = item.Key;

                    listDetail.Add(detail);
                }
            }

            OperatorProductCodeDetail(listDetail);
        }

        /// <summary>
        /// 箱体编码明细操作
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="listDetail">明细列表</param>
        public void OperatorProductCodeDetail(DepotManagementDataContext ctx, List<WS_ProductCodeDetail> listDetail)
        {
            if (!Convert.ToBoolean(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.开启车间管理模块]))
            {
                return;
            }

            var varData = (from a in listDetail
                           select new { a.GoodsID, a.BillNo }).Distinct();

            foreach (var item in varData)
            {
                var varDetail = from a in ctx.WS_ProductCodeDetail
                                where a.BillNo == item.BillNo
                                && a.GoodsID == item.GoodsID
                                select a;

                ctx.WS_ProductCodeDetail.DeleteAllOnSubmit(varDetail);
            }

            ctx.WS_ProductCodeDetail.InsertAllOnSubmit(listDetail);
        }

        /// <summary>
        /// 箱体编码明细操作
        /// </summary>
        /// <param name="listDetail">明细列表</param>
        public void OperatorProductCodeDetail(List<WS_ProductCodeDetail> listDetail)
        {
            if (!Convert.ToBoolean(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.开启车间管理模块]))
            {
                return;
            }

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = (from a in listDetail
                           select new { a.GoodsID, a.BillNo, a.OperationType }).Distinct();

            foreach (var item in varData)
            {
                var varDetail = from a in ctx.WS_ProductCodeDetail
                                where a.BillNo == item.BillNo
                                && a.GoodsID == item.GoodsID
                                && a.OperationType == item.OperationType
                                select a;

                ctx.WS_ProductCodeDetail.DeleteAllOnSubmit(varDetail);
            }

            ctx.WS_ProductCodeDetail.InsertAllOnSubmit(listDetail);

            ctx.SubmitChanges();
        }

        /// <summary>
        /// 删除箱体编码明细
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">单据号</param>
        public void DeleteProductCodeDetail(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.WS_ProductCodeDetail
                          where a.BillNo == billNo
                          select a;

            ctx.WS_ProductCodeDetail.DeleteAllOnSubmit(varData);
        }

        /// <summary>
        /// 删除箱体编码明细
        /// </summary>
        /// <param name="billNo">单据号</param>
        public void DeleteProductCodeDetail(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.WS_ProductCodeDetail
                          where a.BillNo == billNo
                          select a;

            ctx.WS_ProductCodeDetail.DeleteAllOnSubmit(varData);
        }

        /// <summary>
        /// 箱体编码库存操作
        /// </summary>
        /// <param name="billNo">单据号</param>
        public void OperatorProductCodeStock(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.WS_ProductCodeDetail
                          join b in ctx.BASE_SubsidiaryOperationType
                          on a.OperationType equals b.OperationType
                          where a.BillNo == billNo
                          orderby b.DepartmentType
                          select a;

            foreach (WS_ProductCodeDetail item in varData)
            {
                item.IsUse = true;

                var varStock = from a in ctx.WS_ProductCodeStock
                               where a.GoodsID == item.GoodsID
                               && a.ProductCode == item.ProductCode
                               select a;

                BASE_SubsidiaryOperationType tempType = UniversalFunction.GetSubsidiaryOperationType(item.OperationType);

                if (varStock.Count() == 0)
                {
                    if (!(bool)tempType.DepartmentType)
                    {
                        throw new Exception("业务类型错误，请重新核对");
                    }
                    else
                    {
                        WS_ProductCodeStock tempStock = new WS_ProductCodeStock();

                        tempStock.GoodsID = item.GoodsID;
                        tempStock.IsInStock = true;
                        tempStock.ProductCode = item.ProductCode;
                        tempStock.StorageID = item.StorageID;

                        if (item.OperationType == (int)CE_SubsidiaryOperationType.营销退货)
                        {
                            var varMarketingBill = from a in ctx.S_MarketingBill
                                                   where a.DJH == billNo
                                                   select a;

                            if (varData.Count() != 1)
                            {
                                throw new Exception("营销退货单【" + billNo + "】不存在，业务终止");
                            }
                            else
                            {
                                if (varMarketingBill.Single().StorageID == "05")
                                {
                                    tempStock.IsAfterSale = true;
                                }
                            }
                        }

                        ctx.WS_ProductCodeStock.InsertOnSubmit(tempStock);
                    }
                }
                else if (varStock.Count() == 1)
                {
                    WS_ProductCodeStock tempStock = varStock.Single();

                    if (tempStock.IsInStock != tempType.DepartmentType)
                    {
                        tempStock.IsInStock = (bool)tempType.DepartmentType;
                        tempStock.StorageID = item.StorageID;

                        if (item.OperationType == (int)CE_SubsidiaryOperationType.营销退货)
                        {
                            var varMarketingBill = from a in ctx.S_MarketingBill
                                                   where a.DJH == billNo
                                                   select a;

                            if (varData.Count() != 1)
                            {
                                throw new Exception("营销退货单【" + billNo + "】不存在，业务终止");
                            }
                            else
                            {
                                if (varMarketingBill.Single().StorageID == "05")
                                {
                                    tempStock.IsAfterSale = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("【箱体编码】："+ item.ProductCode +" 发生业务类型异常");
                    }
                }
                else
                {
                    throw new Exception("【箱体编码】：" + item.ProductCode + " 数据错误");
                }
            }

            ctx.SubmitChanges();
        }

        /// <summary>
        /// 箱体编码库存操作
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">单据号</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="operationType">操作类型</param>
        public void OperatorProductCodeStock(DepotManagementDataContext ctx , string billNo, int goodsID , int operationType)
        {
            var varData = from a in ctx.WS_ProductCodeDetail
                          where a.BillNo == billNo 
                          && a.GoodsID == goodsID
                          && a.OperationType == operationType
                          select a;

            foreach (WS_ProductCodeDetail item in varData)
            {
                item.IsUse = true;

                var varStock = from a in ctx.WS_ProductCodeStock
                               where a.GoodsID == item.GoodsID
                               && a.ProductCode == item.ProductCode
                               select a;

                BASE_SubsidiaryOperationType tempType = UniversalFunction.GetSubsidiaryOperationType(item.OperationType);

                if (varStock.Count() == 0)
                {
                    if (!(bool)tempType.DepartmentType)
                    {
                        throw new Exception("业务类型错误，请重新核对");
                    }
                    else
                    {
                        WS_ProductCodeStock tempStock = new WS_ProductCodeStock();

                        tempStock.GoodsID = item.GoodsID;
                        tempStock.IsInStock = true;
                        tempStock.ProductCode = item.ProductCode;
                        tempStock.StorageID = item.StorageID;

                        if (item.OperationType == (int)CE_SubsidiaryOperationType.营销退货)
                        {
                            var varMarketingBill = from a in ctx.S_MarketingBill
                                                   where a.DJH == billNo
                                                   select a;

                            if (varMarketingBill.Count() != 1)
                            {
                                throw new Exception("营销退货单【"+ billNo +"】不存在，业务终止");
                            }
                            else
                            {
                                if (varMarketingBill.Single().StorageID == "05")
                                {
                                    tempStock.IsAfterSale = true;
                                }
                            }
                        }

                        ctx.WS_ProductCodeStock.InsertOnSubmit(tempStock);
                    }
                }
                else if (varStock.Count() == 1)
                {
                    WS_ProductCodeStock tempStock = varStock.Single();

                    if (tempStock.IsInStock != tempType.DepartmentType)
                    {
                        tempStock.IsInStock = (bool)tempType.DepartmentType;
                        tempStock.StorageID = item.StorageID; 

                        if (item.OperationType == (int)CE_SubsidiaryOperationType.营销退货)
                        {
                            var varMarketingBill = from a in ctx.S_MarketingBill
                                                   where a.DJH == billNo
                                                   select a;

                            if (varMarketingBill.Count() != 1)
                            {
                                throw new Exception("营销退货单【" + billNo + "】不存在，业务终止");
                            }
                            else
                            {
                                if (varMarketingBill.Single().StorageID == "05")
                                {
                                    tempStock.IsAfterSale = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("【箱体编码】：" + item.ProductCode + " 发生业务类型异常");
                    }
                }
                else
                {
                    throw new Exception("【箱体编码】：" + item.ProductCode + " 数据错误");
                }
            }
        }

        /// <summary>
        /// 获得车间箱号库存数
        /// </summary>
        /// <param name="wsCode">车间代码</param>
        /// <returns>返回Table</returns>
        public DataTable GetWorkShopProductCodeNumber(string wsCode)
        {
            string strSql = "select b.图号型号 as 产品型号,COUNT(*) as 数量 ,GoodsID as 物品ID "+
                           " from WS_ProductCodeStock as a inner join View_F_GoodsPlanCost as b on a.GoodsID = b.序号 "+
                           " where IsInStock = 1 and StorageID = '" + wsCode + "' " +
                           " group by StorageID,GoodsID,b.图号型号 order by StorageID,b.GoodsCode";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得车间箱号信息
        /// </summary>
        /// <param name="wsCode">车间代码</param>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回Table</returns>
        public DataTable GetWorkShopProductCodeInfo(string wsCode ,int goodsID)
        {
            string strSql = " select ProductCode as 产品箱号 from WS_ProductCodeStock "+
                " where IsInStock = 1 and StorageID = '" + wsCode + "' and GoodsID = " + goodsID;

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得车间箱号业务信息
        /// </summary>
        /// <param name="productCode">产品箱号</param>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回Table</returns>
        public DataTable GetWorkShopProductCodeBusiness(string productCode, int goodsID)
        {
            string strSql = " select BillNo as 单据号, Explain as 业务类型 from WS_ProductCodeDetail as a  "+
                            " inner join WS_SubsidiaryOperationType as b on a.OperationType = b.OperationType "+
                            " where GoodsID = " + goodsID + " and ProductCode = '" + productCode + "' and IsUse = 1 order by a.ID";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }
    }
}
