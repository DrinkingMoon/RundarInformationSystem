/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  ProductCodeServer.cs
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

namespace ServerModule
{
    /// <summary>
    /// 产品编码服务类
    /// </summary>
    class ProductCodeServer : BasicServer, ServerModule.IProductCodeServer
    {
        /// <summary>
        /// 是否存在过库存记录
        /// </summary>
        /// <param name="billNo">单据号</param>
        public void IsExistProductStock(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.ProductsCodes
                          where a.DJH == billNo
                          select a;

            foreach (ProductsCodes item in varData)
            {
                var tempData = from a in ctx.ProductStock
                               where a.ProductCode == item.ProductCode
                               && a.GoodsID == item.GoodsID
                               select a;

                if (tempData.Count() > 0)
                {
                    throw new Exception("箱号【"+ item.ProductCode +"】,已存在过，无法进行【生产入库】");
                }
            }
        }

        /// <summary>
        /// 产品编号处理
        /// </summary>
        /// <param name="productTable">产品编号列表</param>
        /// <param name="code">产品型号</param>
        /// <param name="zcCode">总成号</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="djh">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>处理成功返回True，处理失败返回False</returns>
        public bool UpdateProducts(DataTable productTable, string code, string zcCode, int goodsID, string djh, out string error)
        {
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {

                //删除原有的同物品同批次的所有产品的唯一编码
                var varData_Del = from a in dataContxt.ProductsCodes
                                  where a.DJH == djh
                                        && a.GoodsID == goodsID
                                  select a;

                dataContxt.ProductsCodes.DeleteAllOnSubmit(varData_Del);
                dataContxt.SubmitChanges();

                //FOR循环进行添加
                List<ProductsCodes> lisList = new List<ProductsCodes>();

                if (productTable.Rows.Count == 0)
                {
                    return true;
                }
                else
                {
                    for (int i = 0; i < productTable.Rows.Count; i++)
                    {

                        var varData = from a in dataContxt.ProductsCodes
                                      where a.DJH == djh
                                      && a.GoodsID == goodsID
                                      && a.ProductCode == productTable.Rows[i]["ProductCode"].ToString()
                                      select a;

                        if (varData.Count() == 0)
                        {
                            IBasicGoodsServer serverBasicGoods = SCM_Level01_ServerFactory.GetServerModule<IBasicGoodsServer>();
                            View_F_GoodsPlanCost tempGoodsLnq = UniversalFunction.GetGoodsInfo(goodsID);

                            if (tempGoodsLnq == null)
                            {
                                throw new Exception("系统中无此物品信息");
                            }

                            ProductsCodes lnqList = new ProductsCodes();

                            lnqList.ProductCode = productTable.Rows[i]["ProductCode"].ToString();
                            lnqList.GoodsName = tempGoodsLnq.物品名称;
                            lnqList.GoodsCode = tempGoodsLnq.图号型号;
                            lnqList.Spec = tempGoodsLnq.规格;
                            lnqList.GoodsID = goodsID;
                            lnqList.Code = code;
                            lnqList.ZcCode = zcCode;
                            lnqList.DJH = djh;
                            lnqList.BoxNo = productTable.Rows[i]["BoxNo"].ToString();

                            lisList.Add(lnqList);
                        }

                    }

                    dataContxt.ProductsCodes.InsertAllOnSubmit(lisList);
                    dataContxt.SubmitChanges();
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
        /// 获得单据中的TCU的版本号
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <returns>返回TCU版本号</returns>
        private string GetVersion(DepotManagementDataContext ctx, string djh, int goodsID)
        {
            var varData = (from a in ctx.S_MarketingBill
                           join b in ctx.S_MarketingList on a.ID equals b.DJ_ID
                           join c in UniversalFunction.GetGoodsInfoList_Attribute(ctx, CE_GoodsAttributeName.TCU, "True")
                           on Convert.ToInt32(b.CPID) equals c.ID
                           where a.DJH == djh && Convert.ToInt32(b.CPID) == goodsID
                           select new { b.Version }).Distinct();

            if (varData == null || varData.Count() == 0)
            {
                return "";
            }
            else if (varData.Count() > 1)
            {
                throw new Exception("TCU版本号不唯一");
            }
            else
            {
                return varData.Single().Version;
            }
        }

        /// <summary>
        /// 更新产品编码库存及库存状态
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="djh">单据号</param>
        /// <param name="marketingType">单据类型</param>
        /// <param name="storageID">库房ID</param>
        /// <param name="isRepaired">是否为已返修 True 是，False 否</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool UpdateProductStock(DepotManagementDataContext context, string djh, string marketingType, string storageID, bool isRepaired, int goodsID, out string error)
        {
            try
            {
                error = null;

                var varTempData = from a in context.ProductsCodes
                                  where a.DJH == djh
                                  && a.GoodsID == goodsID
                                  select a;

                foreach (ProductsCodes productCodesItem in varTempData)
                {
                    //更新流水码业务表的是否已确认的状态
                    var varCode = from b in context.ProductsCodes
                                  where b.ID == productCodesItem.ID
                                  select b;

                    if (varCode.Count() != 1)
                    {
                        error = "数据不唯一";
                        return false;
                    }

                    ProductsCodes lnqCode = varCode.Single();

                    lnqCode.IsUse = true;

                    //更新或者添加流水码库存表
                    var varData = from a in context.ProductStock
                                  where a.ProductCode == productCodesItem.ProductCode
                                  && a.StorageID == storageID
                                  && a.GoodsID == productCodesItem.GoodsID
                                  select a;

                    if (varData.Count() == 0)
                    {
                        ProductStock lnqProductStock = new ProductStock();

                        lnqProductStock.StorageID = storageID;
                        lnqProductStock.ProductCode = productCodesItem.ProductCode;
                        lnqProductStock.ProductStatus = marketingType;
                        lnqProductStock.GoodsID = productCodesItem.GoodsID;
                        lnqProductStock.BoxNo = productCodesItem.BoxNo;
                        lnqProductStock.IsNatural = true;
                        lnqProductStock.Version = GetVersion(context, djh, productCodesItem.GoodsID);
                        lnqProductStock.RepairStatus = isRepaired;

                        context.ProductStock.InsertOnSubmit(lnqProductStock);
                    }
                    else if (varData.Count() == 1)
                    {
                        ProductStock lnqProductStock = varData.Single();

                        lnqProductStock.ProductStatus = marketingType;
                        lnqProductStock.RepairStatus = isRepaired;
                        lnqProductStock.BoxNo = productCodesItem.BoxNo;

                        if (marketingType == "退货")
                        {
                            lnqProductStock.IsNatural = true;
                        }
                    }
                    else
                    {
                        error = "数据不唯一";
                        return false;
                    }
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
        /// 是否存在过此编号的信息
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="productCode">箱体编码</param>
        /// <param name="goodsID">物品ID</param>
        /// <returns>存在过返回True,否则返回False</returns>
        public bool IsProductCodeInfo(DepotManagementDataContext ctx, string productCode, int goodsID)
        {
            var varData = from a in ctx.ProductStock
                          where a.ProductCode == productCode
                          && a.GoodsID == goodsID
                          select a;

            if (varData.Count() > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 检查当前编号是否在库房内
        /// </summary>
        /// <param name="productCode">产品编号</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>在库房返回True，不在库房返回False</returns>
        public bool IsProductCodeInStock(string productCode,int goodsID, string storageID)
        {

            string strSql = "select * from ProductStock where ProductCode = '" + productCode
                + "' and ProductStatus in ('入库','退库','领料退库','调入') and GoodsID = " + goodsID ;

            if (storageID != null && storageID.Trim().Length != 0)
            {
                strSql += " and StorageID = '" + storageID + "'";
            }

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dtTemp.Rows.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 检查ProductsCodes表中的数量与所要执行的业务的数量是否一致
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="inCount">执行业务的数量</param>
        /// <param name="djh">单据号</param>
        /// <returns>不一致返回False,一致或者被忽略检测则返回True</returns>
        public bool IsFitCount(int goodsID, int inCount, string djh)
        {
            try
            {
                var varData = from a in UniversalFunction.GetGoodsInfoList_Attribute(CE_GoodsAttributeName.流水码, "True")
                              where a.ID == goodsID
                              select a;

                if (varData.Count() > 0)
                {
                    string strSql = "select * from ProductsCodes where GoodsID = " + goodsID +
                                 " and DJH = '" + djh + "'";

                    //单据号为批次号
                    DataTable dtShow = GlobalObject.DatabaseServer.QueryInfo(strSql);

                    if (dtShow.Rows.Count != inCount)
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 针对于领料单检查总成的数量
        /// </summary>
        /// <param name="billNo">领料单号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>一致或者被忽略检测返回True，不一致返回False</returns>
        public bool IsFitCountInRequisitionBill(string billNo, out string error)
        {
            try
            {
                error = null;

                string strSql = "select a.* from S_MaterialRequisitionGoods as a inner join S_MaterialRequisition as b  "+
                    "on a.Bill_ID = b.Bill_ID inner join BASE_Storage as c on b.StorageID = c.StorageID "+
                    "where c.AssemblyWarehouse = 1 and a.Bill_ID = '" + billNo + "'";

                DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

                for (int i = 0; i < dtTemp.Rows.Count; i++)
                {
                    if (!IsFitCount(Convert.ToInt32(dtTemp.Rows[i]["GoodsID"]),
                        Convert.ToInt32(dtTemp.Rows[i]["RealCount"]), billNo))
                    {
                        error = "请对产品设置流水号,并保证产品数量与流水号数一致";
                        return false;
                    }
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
        /// 针对于领料退库单检查总成的数量
        /// </summary>
        /// <param name="billNo">领料退库单号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>一致或者被忽略检测返回True，不一致返回False</returns>
        public bool IsFitCountInReturnBill(string billNo, out string error)
        {
            try
            {
                error = null;

                string strSql = "select a.* from S_MaterialListReturnedInTheDepot as a inner join S_MaterialReturnedInTheDepot as b  " +
                    "on a.Bill_ID = b.Bill_ID inner join BASE_Storage as c on b.StorageID = c.StorageID " +
                    "where c.AssemblyWarehouse = 1 and a.Bill_ID = '" + billNo + "'";

                DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

                for (int i = 0; i < dtTemp.Rows.Count; i++)
                {
                    if (!IsFitCount(Convert.ToInt32(dtTemp.Rows[i]["GoodsID"]),
                        Convert.ToInt32(dtTemp.Rows[i]["ReturnedAmount"]), billNo))
                    {
                        error = "请对产品设置流水号,并保证产品数量与流水号数一致";
                        return false;
                    }
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
        /// 获得产品编码业务信息
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="operationType">业务类型 若为“全  部”则显示全部业务</param>
        /// <param name="productType">产品型号   若为“全  部”则显示全部产品</param>
        /// <returns>返回获得产品编码业务信息</returns>
        public DataTable GetProductCodeOperationInfo(DateTime startTime, DateTime endTime, string operationType, string productType)
        {
            string strSql = " select a.Bill_ID as 单据号,c.DepartmentName as 部门单位,FillInPersonnel as 编制人,YWLX as 业务类型,YWFS as 业务方式," +
                            " d.图号型号,d.物品名称,ProductCode as 产品编号,Count as 数量,AffirmDate as 日期 from " +
                            " (select DJH as Bill_ID,objectDept as Department, dbo.fun_get_Name(LRRY) as FillInPersonnel,'营销' + YWLX as YWLX, " +
                            " YWFS,CPID as GoodsID,Count,AffirmDate from S_MarketingBill as a inner join S_MarketingList as b on a.ID = b.DJ_ID " +
                            " where DJZT_Flag = '已确认' union all  select a.Bill_ID,Department,FillInPersonnel,FetchType,c.Purpose,GoodsID,RealCount, " +
                            " OutDepotDate from S_MaterialRequisition as a inner join S_MaterialRequisitionGoods as b on a.Bill_ID = b.Bill_ID " +
                            " inner join BASE_MaterialRequisitionPurpose as c on a.PurposeCode = c.Code where BillStatus = '已出库' " +
                            " union all select a.Bill_ID,Department,FillInPersonnel,ReturnMode,c.Purpose,GoodsID,ReturnedAmount,InDepotDate " +
                            " from S_MaterialReturnedInTheDepot as a inner join S_MaterialListReturnedInTheDepot as b on a.Bill_ID = b.Bill_ID " +
                            " inner join BASE_MaterialRequisitionPurpose as c on a.PurposeCode = c.Code where BillStatus = '已完成' " +
                            " union all select a.Bill_ID,DeclareDepartment,FillInPersonnel,'报废','报废',GoodsID,Quantity,DepotTime " +
                            " from S_ScrapBill as a inner join S_ScrapGoods as b on a.Bill_ID = b.Bill_ID " +
                            " where BillStatus = '已完成') as a inner join ProductsCodes as b on a.Bill_ID = b.DJH and a.GoodsID = b.GoodsID " +
                            " inner join  (select DepartmentCode,DepartmentName from Department  union all select ClientCode,ClientName from Client " +
                            " ) as c on a.Department = c.DepartmentCode inner join View_F_GoodsPlanCost as d on a.GoodsID = d.序号 ";

            strSql += " where AffirmDate >= '" + startTime + "' and AffirmDate <= '" + endTime + "'";

            if (operationType != "全  部")
            {
                strSql += " and YWLX = '" + operationType + "'";
            }

            if (productType != "全  部")
            {
                strSql += " and d.GoodsCode = '" + productType + "'";
            }

            strSql += " order by AffirmDate";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dt;
        }

        /// <summary>
        /// 产品总成编码校验
        /// </summary>
        /// <param name="productType">总成编码</param>
        /// <param name="productCode">产品编码</param>
        /// <param name="barCodeType">编码类型</param>
        /// <param name="error">出错时返回的错误信息</param>
        /// <returns>检验通过返回true, 失败返回false</returns>
        public bool VerifyProductCodesInfo(string productType, string productCode, GlobalObject.CE_BarCodeType barCodeType, out string error)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            error = null;

            try
            {
                var varData = from a in ctx.F_GoodsPlanCost
                              where a.GoodsCode == productType
                              select a;

                foreach (var item in varData)
                {
                    if (VerifyProductCodesInfo(item.ID, productCode, barCodeType,  out error))
                    {
                        return true;
                    }
                }

                error = "未找到匹配编码";
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 产品总成编码校验
        /// </summary>
        /// <param name="goodsID">总成ID</param>
        /// <param name="productCode">产品编码</param>
        /// <param name="barCodeType">编码类型</param>
        /// <param name="error">出错时返回的错误信息</param>
        /// <returns>检验通过返回true, 失败返回false</returns>
        public bool VerifyProductCodesInfo(int goodsID, string productCode, GlobalObject.CE_BarCodeType barCodeType, out string error)
        {
            error = "";
            productCode = productCode.ToUpper();

            View_F_GoodsPlanCost goodsInfo = UniversalFunction.GetGoodsInfo(goodsID);

            if (!Convert.ToBoolean(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.检测钢印码及条形码]))
            {
                return true;
            }

            if (StapleFunction.IsChineseCharacters(productCode))
            {
                error = "请不要在产品编号中包含中文字符";
                return false;
            }

            Dictionary<CE_GoodsAttributeName, object> dic = UniversalFunction.GetGoodsInfList_Attribute_AttchedInfoList(goodsID);

            if (!dic.Keys.ToList().Contains(CE_GoodsAttributeName.流水码))
            {
                throw new Exception(UniversalFunction.GetGoodsMessage(goodsID) + "基础属性未设置【流水码】");
            }

            List<F_ProductWaterCode> lstWaterCode = dic[CE_GoodsAttributeName.流水码] as List<F_ProductWaterCode>;

            if (lstWaterCode.Count == 0)
            {
                error = "产品类型不正确,无法找到对应的编码规则";
                return false;
            }
            else
            {
                foreach (var item in lstWaterCode)
                {
                    //条形码检测
                    if (barCodeType == CE_BarCodeType.出厂条形码)
                    {
                        if (productCode.Length == item.BarcodeExample.Length &&
                            Regex.IsMatch(productCode, @item.BarcodeRole))
                        {
                            return true;
                        }

                    }//钢印码检测
                    else if (barCodeType == CE_BarCodeType.内部钢印码)
                    {
                        if (productCode.Length == item.SteelSealExample.Length &&
                            Regex.IsMatch(productCode, @item.SteelSealRole))
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }

                if (barCodeType == CE_BarCodeType.出厂条形码)
                {
                    error = "编码规则不符,条形码正确格式为";
                    foreach (var item in lstWaterCode)
                    {
                        error += "【" + item.BarcodeExample.Trim() + "】";
                    }
                }
                else if (barCodeType == CE_BarCodeType.内部钢印码)
                {
                    error = "编码规则不符,钢印码正确格式为";
                    foreach (var item in lstWaterCode)
                    {
                        error += "【" + item.SteelSealExample.Trim() + "】";
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// 根据申请单号批量插入CVT箱号与业务单据关系信息记录
        /// </summary>
        /// <param name="requisitionBillNo">申请单号列表</param>
        /// <param name="inputBillNo">业务单据号</param>
        public void InsertChangeProductCodesBillNo(List<string> requisitionBillNo, string inputBillNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.ProductsCodes
                          where requisitionBillNo.Contains(a.DJH)
                          select a;

            foreach (ProductsCodes codesInfo in varData)
            {
                ProductsCodes tempCodes = new ProductsCodes();

                tempCodes.BoxNo = codesInfo.BoxNo;
                tempCodes.Code = codesInfo.Code;
                tempCodes.DJH = inputBillNo;
                tempCodes.GoodsCode = codesInfo.GoodsCode;
                tempCodes.GoodsID = codesInfo.GoodsID;
                tempCodes.GoodsName = codesInfo.GoodsName;
                tempCodes.IsUse = false;
                tempCodes.ProductCode = codesInfo.ProductCode;
                tempCodes.Remark = codesInfo.Remark;
                tempCodes.Spec = codesInfo.Spec;
                tempCodes.ZcCode = codesInfo.ZcCode;

                ctx.ProductsCodes.InsertOnSubmit(tempCodes);
            }

            ctx.SubmitChanges();
        }

        /// <summary>
        /// 添加自动入库附表记录
        /// </summary>
        /// <param name="info"></param>
        public void Add_AutoCreatePutIn_Subsidiary(ProductCode_AutoCreatePutIn_Subsidiary info)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            ProductCode_AutoCreatePutIn_Subsidiary newInfo = new ProductCode_AutoCreatePutIn_Subsidiary();

            newInfo.GoodsID = info.GoodsID;
            newInfo.ProductCode = info.ProductCode;
            newInfo.PutInType = info.PutInType;
            newInfo.StorageID = info.StorageID;

            ctx.ProductCode_AutoCreatePutIn_Subsidiary.InsertOnSubmit(newInfo);
            ctx.SubmitChanges();
        }

        /// <summary>
        /// 删除自动入库附表记录
        /// </summary>
        /// <param name="info"></param>
        public void Del_AutoCreatePutIn_Subsidiary(ProductCode_AutoCreatePutIn_Subsidiary info)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.ProductCode_AutoCreatePutIn_Subsidiary
                          where a.GoodsID == info.GoodsID
                          && a.ProductCode == info.ProductCode
                          && a.PutInType == info.PutInType
                          && a.StorageID == info.StorageID
                          select a;

            ctx.ProductCode_AutoCreatePutIn_Subsidiary.DeleteAllOnSubmit(varData);
            ctx.SubmitChanges();
        }

        /// <summary>
        /// 查询自动入库附表记录
        /// </summary>
        public DataTable Sel_AutoCreatePutIn_Subsidiary()
        {
            string strSql = " select b.GoodsCode as 产品名称, a.ProductCode as 箱体编号, a.PutInType as 入库方式, "+ 
                            " c.StorageName as 入库库房, a.GoodsID as 物品ID, a.StorageID as 库房ID "+ 
                            " from ProductCode_AutoCreatePutIn_Subsidiary as a inner join F_GoodsPlanCost as b on a.GoodsID = b.ID "+
                            " inner join BASE_Storage as c on a.StorageID = c.StorageID";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }
    }
}
