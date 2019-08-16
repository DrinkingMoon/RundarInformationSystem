using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformManagement;
using ServerModule;
using System.Data;
using GlobalObject;
using FlowControlService;
using System.Collections;

namespace Service_Manufacture_WorkShop
{
    class InProductReport : IInProductReport
    {
        public void CreateInProductReport(List<object> lstObj)
        {
            string error = null;

            string strYearMonth = lstObj[0].ToString();
            DateTime endTime = Convert.ToDateTime(lstObj[1]);
            DataTable dtProduct = lstObj[2] as DataTable;

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Bus_WorkShop_InProduct_Supplementary_Product
                          where a.YearMonth == strYearMonth
                          select a;

            ctx.Bus_WorkShop_InProduct_Supplementary_Product.DeleteAllOnSubmit(varData);

            foreach (DataRow dr in dtProduct.Rows)
            {
                Bus_WorkShop_InProduct_Supplementary_Product tempInfo = new Bus_WorkShop_InProduct_Supplementary_Product();

                tempInfo.F_Id = Guid.NewGuid().ToString();
                tempInfo.ProductCount = Convert.ToDecimal(dr["GoodsCount"]);
                tempInfo.ProductGoodsID = Convert.ToInt32(dr["GoodsID"]);
                tempInfo.YearMonth = strYearMonth;

                ctx.Bus_WorkShop_InProduct_Supplementary_Product.InsertOnSubmit(tempInfo);
            }

            ctx.SubmitChanges();

            Hashtable hs = new Hashtable();

            hs.Add("@YearMonth", strYearMonth);
            hs.Add("@EndTime", endTime);
            hs.Add("@UserID", BasicInfo.LoginID);

            GlobalObject.DatabaseServer.QueryInfoPro("Bus_WorkShop_InProduct_Report", hs, out error);
        }

        public void InputNowBalanceCount(List<Bus_WorkShop_InProduct> lstInfo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            foreach (Bus_WorkShop_InProduct item in lstInfo)
            {
                var varData = from a in ctx.Bus_WorkShop_InProduct
                              where a.GoodsID == item.GoodsID
                              && a.YearMonth == item.YearMonth
                              select a;

                if (varData.Count() == 0)
                {
                    item.F_Id = Guid.NewGuid().ToString();
                    item.InventoryCount = -item.NowBalanceCount;

                    ctx.Bus_WorkShop_InProduct.InsertOnSubmit(item);
                }
                else if(varData.Count() == 1)
                {
                    Bus_WorkShop_InProduct tempInfo = varData.Single();
                    
                    tempInfo.NowBalanceCount = item.NowBalanceCount;
                    tempInfo.InventoryCount = 
                        tempInfo.LastBalanceCount + tempInfo.ShowInCount - tempInfo.ProductCount - 
                        tempInfo.ScrapCount - tempInfo.NowBalanceCount;
                }
                else
                {
                    throw new Exception("导入有误");
                }
            }

            ctx.SubmitChanges();
        }

        public void InputResolveInfo(List<Bus_WorkShop_InProduct_Resolve> lstResolve)
        {
            if (lstResolve == null || lstResolve.Count() == 0)
            {
                return;
            }

            string yearMonth = lstResolve[0].YearMonth;

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData1 = from a in ctx.Bus_WorkShop_InProduct
                           where a.YearMonth == yearMonth
                           select a;

            if (varData1.Count() == 0)
            {
                return;
            }

            foreach (Bus_WorkShop_InProduct inProduct in varData1)
            {
                List<Bus_WorkShop_InProduct_Resolve> lstTemp = (from a in lstResolve
                                                                where a.GoodsID == inProduct.GoodsID
                                                                select a).ToList();

                decimal useCount = lstTemp.Sum(k => k.GoodsCount);

                if (useCount != (inProduct.InventoryCount + inProduct.ProductCount + inProduct.ShowInCount))
                {
                    throw new Exception(string.Format("物品ID：【{1}】, 合计数量不正确", inProduct.GoodsID));
                }

                var varData = from a in ctx.Bus_WorkShop_InProduct_Resolve
                              where a.YearMonth == yearMonth
                              && a.GoodsID == inProduct.GoodsID
                              select a;

                ctx.Bus_WorkShop_InProduct_Resolve.DeleteAllOnSubmit(varData);

                foreach (Bus_WorkShop_InProduct_Resolve resolve in lstTemp)
                {
                    resolve.F_Id = Guid.NewGuid().ToString();

                    var varData2 = from a in ctx.Provider
                                  where a.ProviderCode == resolve.Provider
                                  select a;

                    if (varData2.Count() == 0)
                    {
                        throw new Exception(string.Format("供应商：【{1}】，不存在", resolve.Provider));
                    }

                    ctx.Bus_WorkShop_InProduct_Resolve.InsertOnSubmit(resolve);
                }
            }

            ctx.SubmitChanges();
        }

        public void InputBom(List<Bus_WorkShop_InProduct_Bom> lstInfo)
        {
            if (lstInfo == null || lstInfo.Count() == 0)
            {
                return;
            }

            int productGoodsID = lstInfo[0].ProductGoodsID;

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Bus_WorkShop_InProduct_Bom
                          where a.ProductGoodsID == productGoodsID
                          select a;

            ctx.Bus_WorkShop_InProduct_Bom.DeleteAllOnSubmit(varData);

            foreach (Bus_WorkShop_InProduct_Bom bom in lstInfo)
            {
                bom.F_Id = Guid.NewGuid().ToString();

                ctx.Bus_WorkShop_InProduct_Bom.InsertOnSubmit(bom);
            }

            ctx.SubmitChanges();
        }

        public DataTable GetBom(int goodsID)
        {
            string strSql = " select c.GoodsCode as [总成/分总成图号], b.GoodsCode as 图号型号, b.GoodsName as 物品名称, " +
                            " b.Spec as 规格, a.UseCount as 用量, a.ProductGoodsID as 总成ID, a.GoodsID as 物品ID  " +
                            " from Bus_WorkShop_InProduct_Bom as a  " +
                                " inner join F_GoodsPlanCost as b on a.GoodsID = b.ID " +
                                " inner join F_GoodsPlanCost as c on a.ProductGoodsID = c.ID " +
                            " where a.ProductGoodsID = " + goodsID +
                            " order by a.ProductGoodsID, b.GoodsCode";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        DataTable GetInProduct(string yearMonth)
        {
            string strSql = " select b.GoodsCode as 图号型号, b.GoodsName as 物品名称, b.Spec as 规格, " +
                            " a.LastBalanceCount as 上期结存, a.ShowInCount as 本月领入,  " +
                                " a.ProductCount as 本月装配用, a.ScrapCount as 本月报废用, " +
                                " a.InventoryCount as [本月盘盈/盘亏], a.NowBalanceCount as 本期结存, " +
                                " a.GoodsID as 物品ID, a.YearMonth as 年月 " +
                            " from Bus_WorkShop_InProduct as a  " +
                                " inner join F_GoodsPlanCost as b on a.GoodsID = b.ID " +
                            " where a.YearMonth = '" + yearMonth + "' " +
                            " order by b.GoodsCode";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        DataTable GetSupplementary(string yearMonth)
        {
            string error = null;

            Hashtable hs = new Hashtable();
            hs.Add("@YearMonth", yearMonth);

            return DatabaseServer.QueryInfoPro("Bus_WorkShop_InProduct_Report_Supplementary", hs, out error);
        }

        DataTable GetResolve(string yearMonth)
        {
            string strSql = " select b.GoodsCode as 图号型号, b.GoodsName as 物品名称, b.Spec as 规格, "+
	                        " a.Provider as 供应商, a.GoodsCount as 耗用数, a.GoodsID as 物品ID, a.YearMonth as 年月  "+
	                        " from Bus_WorkShop_InProduct_Resolve as a  "+
		                        " inner join F_GoodsPlanCost as b on a.GoodsID = b.ID "+
                            " where a.YearMonth = '" + yearMonth + "' " +
                            " order by b.GoodsCode, a.Provider ";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        public DataTable GetAllInProductInfo(string typeName, string yearMonth)
        {
            DataTable tempTable = new DataTable();

            switch (typeName)
            {
                case "车间在产报表":
                    tempTable = GetInProduct(yearMonth);
                    break;
                case "盘点辅助报表":
                    tempTable = GetSupplementary(yearMonth);
                    break;
                case "零件供应商耗用报表":
                    tempTable = GetResolve(yearMonth);
                    break;
                default:
                    break;
            }

            return tempTable;
        }
    }
}
