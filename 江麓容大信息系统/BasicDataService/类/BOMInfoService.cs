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
using Service_Project_Design;
using ServerModule;

namespace Service_Project_Design
{
    class BOMInfoService : IBOMInfoService
    {
        /// <summary>
        /// 获得设计BOM结构信息
        /// </summary>
        /// <param name="parentGoodsID">父级物品ID</param>
        /// <returns>返回列表</returns>
        public List<View_BASE_BomStruct> GetBOMList_Design(int parentGoodsID)
        {
            if (parentGoodsID == 0)
            {
                return null;
            }

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.View_BASE_BomStruct
                          where a.ParentID == parentGoodsID
                          select a;

            return varData.ToList();
        }

        /// <summary>
        /// 获得单条零件库零件信息
        /// </summary>
        /// <param name="goodsID">零件物品ID</param>
        /// <returns>返回LINQ信息</returns>
        public BASE_BomPartsLibrary GetLibrarySingle(int goodsID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.BASE_BomPartsLibrary
                          where a.GoodsID == goodsID
                          select a;

            if (varData.Count() > 0)
            {
                return varData.Single();
            }
            else
            {
                return null;
            }
        }

        public List<BASE_BomVersion> GetBOMVersionInfoItems(string edtion, decimal version)
        {
            using (DepotManagementDataContext ctx = CommentParameter.DepotDataContext)
            {
                var varData = from a in ctx.BASE_BomVersion
                              where a.Edtion == edtion && a.DBOMSysVersion == version
                              select a;

                return varData.ToList();
            }
        }

        public List<decimal> GetPBOMVersionItems(string edtion)
        {
            using (DepotManagementDataContext ctx = CommentParameter.DepotDataContext)
            {
                var varData = from a in ctx.BASE_PBOM_Log
                              where a.Edtion == edtion
                              orderby a.PBOMSysVersion descending
                              select a.PBOMSysVersion;

                return varData.Distinct().ToList();
            }
        }

        public DataTable GetPBOMLogInfoItems(string edition, string sysVersion)
        {
            string error = "";

            Hashtable hsTable = new Hashtable();

            hsTable.Add("@Edition", edition);
            hsTable.Add("@SysVersion", sysVersion);

            return GlobalObject.DatabaseServer.QueryInfoPro("BASE_PBOM_Select", hsTable, out error);
        }
    }
}
