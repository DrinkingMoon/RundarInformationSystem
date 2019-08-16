using System;
using System.Data;
using ServerModule;
namespace Service_Economic_Purchase
{
    public interface IUnitPriceList
    {
        System.Data.DataTable GetList();
        void SaveInfo(System.Collections.Generic.List<ServerModule.Bus_PurchasingMG_UnitPriceList> lstUnitPrice);
        DataTable GetList_History();
        Bus_PurchasingMG_UnitPriceList GetItem(string provider, int goodsID);
        DataTable GetMaxVersionNoInfo();
    }
}
