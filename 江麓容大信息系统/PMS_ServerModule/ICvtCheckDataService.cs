/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  ICvtCheckDataService.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2014/01/21
 * 开发平台:  vs2005(c#)
 * 用于    :  生产线管理信息系统
 *----------------------------------------------------------------------------
 * 描述 : 查询CVT总成检测数据的服务接口，数据来源从CVT下线试验台
 * 其它 :
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2014/01/21 13:35:08 作者: 夏石友 当前版本: V1.00
 *        修改说明: 创建
 ******************************************************************************/
using System;

namespace ServerModule
{
    /// <summary>
    /// CVT检测数据服务接口
    /// </summary>
    public interface ICvtCheckDataService
    {
        /// <summary>
        /// 获取指定CVT编号检测数据
        /// </summary>
        /// <param name="cvtNumber">CVT编号</param>
        /// <returns>返回获取到的数据</returns>
        System.Linq.IQueryable<View_ZPX_CVTTestInfo> GetData(string cvtNumber);

        /// <summary>
        /// 获取指定日期范围内的CVT检测数据
        /// </summary>
        /// <param name="beginTime">起始日期</param>
        /// <param name="endTime">结束日期</param>
        /// <returns>返回获取到的数据</returns>
        System.Linq.IQueryable<View_ZPX_CVTTestInfo> GetData(DateTime beginTime, DateTime endTime);
        
        /// <summary>
        /// 保存CVT下线试验数据
        /// </summary>
        /// <param name="testInfo">CVT下线试验信息</param>
        /// <returns>返回操作是否成功的标志</returns>
        bool SaveCVTExpData(CvtTestInfo testInfo);
    }
}
