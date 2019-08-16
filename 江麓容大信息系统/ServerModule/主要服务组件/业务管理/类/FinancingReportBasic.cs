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
using GlobalObject;

namespace ServerModule
{
    /// <summary>
    /// 汇总方式
    /// </summary>
    public enum SummaryMode
    {
        /// <summary>
        /// 日期
        /// </summary>
        日期,

        /// <summary>
        /// 图号
        /// </summary>
        图号,

        /// <summary>
        /// 名称
        /// </summary>
        名称,

        /// <summary>
        /// 供应商
        /// </summary>
        供应商,

        /// <summary>
        /// 用途
        /// </summary>
        用途,

        /// <summary>
        /// 库房
        /// </summary>
        库房,

        /// <summary>
        /// 材料类别
        /// </summary>
        材料类别,

        /// <summary>
        /// 单据编号
        /// </summary>
        单据编号
    }

    /// <summary>
    /// 财务报表基础类
    /// </summary>
    public class FinancingReportBasic
    {

    }
}
