using System;

namespace Service_Manufacture_WorkShop
{
    public interface IInProductReport
    {
        void CreateInProductReport(System.Collections.Generic.List<object> lstObj);
        System.Data.DataTable GetAllInProductInfo(string typeName, string yearMonth);
        System.Data.DataTable GetBom(int goodsID);
        void InputBom(System.Collections.Generic.List<ServerModule.Bus_WorkShop_InProduct_Bom> lstInfo);
        void InputNowBalanceCount(System.Collections.Generic.List<ServerModule.Bus_WorkShop_InProduct> lstInfo);
        void InputResolveInfo(System.Collections.Generic.List<ServerModule.Bus_WorkShop_InProduct_Resolve> lstResolve);
    }
}
