using System;
using System.Collections.Generic;
using ServerModule;
using System.Data;

namespace Service_Project_Design
{
    public interface IBOMInfoService
    {
        System.Collections.Generic.List<ServerModule.View_BASE_BomStruct> GetBOMList_Design(int parentGoodsID);

        /// <summary>
        /// 获得单条零件库零件信息
        /// </summary>
        /// <param name="goodsID">零件物品ID</param>
        /// <returns>返回LINQ信息</returns>
        BASE_BomPartsLibrary GetLibrarySingle(int goodsID);

        List<BASE_BomVersion> GetBOMVersionInfoItems(string edtion, decimal version);

        List<decimal> GetPBOMVersionItems(string edtion);

        DataTable GetPBOMLogInfoItems(string edition, string sysVersion);

        DataTable GetPBOMItems(string edition);
    }
}
