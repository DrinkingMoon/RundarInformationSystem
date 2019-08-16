/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  ValveCheckDataService.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2013/12/09
 * 开发平台:  vs2005(c#)
 * 用于    :  生产线管理信息系统
 *----------------------------------------------------------------------------
 * 描述 : 查询阀块总成检测数据的服务接口，数据来源从阀块下线试验台
 * 其它 :
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2013/12/09 16:02:08 作者: 夏石友 当前版本: V1.00
 *        修改说明: 创建
 ******************************************************************************/
using System;

namespace ServerModule
{
    /// <summary>
    /// 阀块检测数据服务接口
    /// </summary>
    public interface IValveCheckDataService
    {
        /// <summary>
        /// 获取指定阀块编号检测数据
        /// </summary>
        /// <param name="valveNumber">阀块编号</param>
        /// <returns>返回获取到的数据</returns>
        System.Linq.IQueryable<View_ZPX_ValveTestInfo> GetData(string valveNumber);

        /// <summary>
        /// 获取指定日期范围内的阀块检测数据
        /// </summary>
        /// <param name="beginTime">起始日期</param>
        /// <param name="endTime">结束日期</param>
        /// <returns>返回获取到的数据</returns>
        System.Linq.IQueryable<View_ZPX_ValveTestInfo> GetData(DateTime beginTime, DateTime endTime);
    }
}
