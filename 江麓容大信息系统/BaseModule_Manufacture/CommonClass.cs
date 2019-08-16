using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServerModule;

namespace BaseModule_Manufacture
{
    public class CommonClass
    {
        /// <summary>
        /// 获得业务明细信息
        /// </summary>
        /// <param name="billNo">业务号</param>
        /// <returns>返回业务明细列表</returns>
        public List<View_Business_WarehouseInPut_AOGDetail> GetListViewDetailInfo_AOG(string billNo)
        {
            if (billNo == null)
            {
                return null;
            }

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.View_Business_WarehouseInPut_AOGDetail
                          where a.单据号 == billNo
                          select a;

            return varData.ToList();
        }

        /// <summary>
        /// 获得业务明细信息
        /// </summary>
        /// <param name="billNo">业务号</param>
        /// <returns>返回业务明细列表</returns>
        public List<View_Business_WarehouseInPut_RequisitionDetail> GetListViewDetailInfo_Requisition(string billNo)
        {
            if (billNo == null)
            {
                return null;
            }

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.View_Business_WarehouseInPut_RequisitionDetail
                          where a.单据号 == billNo
                          select a;

            return varData.ToList();
        }
    }
}
