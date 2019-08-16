using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BaseModule_Economic
{
    public class CommonClass
    {
        /// <summary>
        /// 通过关联单据号获取销售清单的信息
        /// </summary>
        /// <param name="associatedNo">单据号</param>
        /// <returns>返回数据集</returns>
        public DataTable GetDataByAssociatedNo(string associatedNo)
        {
            string sql = "select * from View_S_MarketingPartBill where 营销出库单号 = '" + associatedNo + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }
    }
}
