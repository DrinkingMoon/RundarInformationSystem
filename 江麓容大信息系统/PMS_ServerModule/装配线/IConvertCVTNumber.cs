using System;
using System.Collections.Generic;

namespace ServerModule
{
    /// <summary>
    /// 转换变速箱箱号信息操作接口
    /// </summary>
    public interface IConvertCVTNumber
    {                
        /// <summary>
        /// 检查箱号是否在变更表中存在（存在于旧箱号或新箱号）
        /// </summary>
        /// <param name="checkMode">检测方式</param>
        /// <param name="productType">产品类型编号</param>
        /// <param name="productNumber">箱号</param>
        /// <returns>存在返回true</returns>
        bool IsExists(ConvertCVTNumber_CheckEnum checkMode, string productType, string productNumber);
                
        /// <summary>
        /// 判断变速箱是否新箱
        /// </summary>
        /// <param name="productType">产品类型编号</param>
        /// <param name="productNumber">箱号</param>
        /// <returns>是新箱返回true</returns>
        bool IsNewCVT(string productType, string productNumber);
                
        /// <summary>
        /// 批量变更变速箱箱号
        /// </summary>
        /// <param name="convertMode">变更模式, 手动模式：新箱箱号是操作人手工录入的；自动模式：新箱箱号由系统自动生成</param>
        /// <param name="data">要添加的数据</param>
        /// <param name="error">出错时返回的错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        bool BatchConvertCVTNumber(string convertMode, List<ZPX_ConvertedCVTNumber> data, out string error);

        /// <summary>
        /// 添加维修记录(不仅变更箱号还根据旧箱电子档案生成新箱电子档案)
        /// </summary>
        /// <param name="data">要添加的数据</param>
        /// <param name="error">出错时返回的错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        bool Add(ServerModule.ZPX_ConvertedCVTNumber data, out string error);

        /// <summary>
        /// 删除维修记录
        /// </summary>
        /// <param name="id">要删除数据的ID</param>
        /// <param name="error">出错时返回的错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        bool Delete(int id, out string error);

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="startDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns>返回获取到的数据集</returns>
        System.Linq.IQueryable<ServerModule.View_ZPX_ConvertedCVTNumber> GetData(DateTime startDate, DateTime endDate);
    }
}
