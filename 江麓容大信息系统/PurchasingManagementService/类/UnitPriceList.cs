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
using ServerModule;
using FlowControlService;

namespace Service_Economic_Purchase
{
    class UnitPriceList : Service_Economic_Purchase.IUnitPriceList
    {
        public DataTable GetList()
        {
            string strSql = " select a.GoodsID, b.GoodsCode, b.GoodsName, b.Spec, a.Provider, a.UnitPrice , "+
                            " a.Rate, a.ValidityStart, a.ValidityEnd, a.RecordTime, c.Name "+
                            " from (select * from Bus_PurchasingMG_UnitPriceList as a "+
                            " where a.VersionNo = (select MAX(VersionNo) from Bus_PurchasingMG_UnitPriceList as b "+
                            " where a.GoodsID = b.GoodsID and a.Provider = b.Provider "+
                            " and a.ValidityStart = b.ValidityStart and a.ValidityEnd = b.ValidityEnd)) as a " +
                            " inner join F_GoodsPlanCost as b on a.GoodsID = b.ID "+
                            " left join HR_PersonnelArchive as c on a.RecordUser = c.WorkID";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        public void SaveInfo(List<Bus_PurchasingMG_UnitPriceList> lstUnitPrice)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();

            try
            {
                Bus_PurchasingMG_UnitPriceList saveInfo = new Bus_PurchasingMG_UnitPriceList();

                foreach (Bus_PurchasingMG_UnitPriceList item in lstUnitPrice)
                {
                    decimal versionNo = (decimal)1.00;

                    var varData = from a in ctx.Bus_PurchasingMG_UnitPriceList
                                  where a.GoodsID == item.GoodsID
                                  && a.Provider == item.Provider
                                  select a;

                    if (varData.Count() > 0)
                    {
                        var varData1 = from a in varData
                                       where a.ValidityEnd == item.ValidityEnd
                                       && a.ValidityStart == item.ValidityStart
                                       && a.UnitPrice == item.UnitPrice
                                       && a.Rate == item.Rate
                                       select a;

                        if (varData1.Count() > 0)
                        {
                            continue;
                        }

                        Bus_PurchasingMG_UnitPriceList tempInfo = varData.OrderByDescending(k => k.VersionNo).First();
                        versionNo = (decimal)tempInfo.VersionNo + (decimal)0.01;
                    }

                    saveInfo = new Bus_PurchasingMG_UnitPriceList();

                    saveInfo.F_Id = Guid.NewGuid().ToString();
                    saveInfo.GoodsID = item.GoodsID;
                    saveInfo.Provider = item.Provider;
                    saveInfo.Rate = item.Rate;
                    saveInfo.RecordTime = ServerTime.Time;
                    saveInfo.RecordUser = BasicInfo.LoginID;
                    saveInfo.UnitPrice = item.UnitPrice;
                    saveInfo.VersionNo = versionNo;
                    saveInfo.ValidityStart = item.ValidityStart;
                    saveInfo.ValidityEnd = item.ValidityEnd;

                    ctx.Bus_PurchasingMG_UnitPriceList.InsertOnSubmit(saveInfo);
                }

                ctx.SubmitChanges();
                ctx.Transaction.Commit();
            }
            catch (Exception ex)
            {
                ctx.Transaction.Rollback();
                throw ex;
            }
        }

        public DataTable GetList_History()
        {
            string strSql = "select * from View_Bus_PurchasingMG_UnitPriceList";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        public Bus_PurchasingMG_UnitPriceList GetItem(string provider, int goodsID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Bus_PurchasingMG_UnitPriceList
                          where a.Provider == provider
                          && a.GoodsID == goodsID
                          select a;

            if (varData.Count() == 0)
            {
                return null;
            }
            else
            {
                return varData.OrderByDescending(k => k.VersionNo).First();
            }
        }

        public DataTable GetMaxVersionNoInfo()
        {
            string strSql = " select * from View_Bus_PurchasingMG_UnitPriceList as a "+
                            " where 版本号 = (select MAX(版本号) from View_Bus_PurchasingMG_UnitPriceList as b "+
                            " where a.物品ID = b.物品ID and a.供应商 = b.供应商)";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }
    }
}
