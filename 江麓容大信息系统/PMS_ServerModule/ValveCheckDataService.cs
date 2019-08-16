/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  ValveCheckDataService.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2013/12/09
 * 开发平台:  vs2005(c#)
 * 用于    :  生产线管理信息系统
 *----------------------------------------------------------------------------
 * 描述 : 查询阀块总成检测数据的服务，数据来源从阀块下线试验台
 * 其它 :
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2013/12/09 16:02:08 作者: 夏石友 当前版本: V1.00
 *        修改说明: 创建
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerModule
{
    /// <summary>
    /// 阀块检测数据服务
    /// </summary>
    class ValveCheckDataService : ServerModule.IValveCheckDataService
    {
        /// <summary>
        /// 获取指定日期范围内的阀块检测数据
        /// </summary>
        /// <param name="beginTime">起始日期</param>
        /// <param name="endTime">结束日期</param>
        /// <returns>返回获取到的数据</returns>
        public IQueryable<View_ZPX_ValveTestInfo> GetData(DateTime beginTime, DateTime endTime)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var result = from r in ctx.View_ZPX_ValveTestInfo
                         where r.日期 >= beginTime.Date && r.日期 <= endTime.Date
                         select r;

            return result;
        }

        /// <summary>
        /// 获取指定阀块编号检测数据
        /// </summary>
        /// <param name="valveNumber">阀块编号</param>
        /// <returns>返回获取到的数据</returns>
        public IQueryable<View_ZPX_ValveTestInfo> GetData(string valveNumber)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var result = from r in ctx.View_ZPX_ValveTestInfo
                         where r.阀块总成编号 == valveNumber
                         select r;

            return result;
        }
    }
}
