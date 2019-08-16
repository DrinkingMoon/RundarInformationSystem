/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  ISafeStockServer.cs
 * 作者    :  曹津彬    版本: v1.00    日期: 2014/01/22
 * 开发平台:  Visual C# 2008
 * 用于    :  仓库管理软件
 *----------------------------------------------------------------------------
 * 描述 : 
 * 其它 :
 *----------------------------------------------------------------------------
 * 公共信息: 参见系统'类帮助文档'
 *----------------------------------------------------------------------------
 * 历史记录:
 ******************************************************************************/
using System;
using System.Linq;
using System.Data;
using System.Collections.Generic;

namespace ServerModule
{
    /// <summary>
    /// 安全库存类接口
    /// </summary>
    public interface ISafeStockServer : IBasicService
    {
        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="error"></param>
        /// <returns>成功返回True,失败返回False</returns>
        bool DeleteInfo(int goodsID, out string error);

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="safeStock">LNQ数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        bool AddInfo(S_SafeStock safeStock, out string error);

        /// <summary>
        /// 更新信息
        /// </summary>
        /// <param name="safeStock">LNQ数据集</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        bool UpdateInfo(S_SafeStock safeStock, int goodsID, out string error);

        /// <summary>
        /// 更新安全库存信息
        /// </summary>
        /// <param name="dtInfo">数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool UpdateSafeStockInfo(DataTable dtInfo, out string error);

        /// <summary>
        /// 获得全部信息
        /// </summary>
        /// <returns>返回数据集</returns>
        DataTable GetAllInfo();

        /// <summary>
        /// 操作数据库
        /// </summary>
        /// <param name="safeList">新的安全库存的数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>更新数据库成功返回True，更新失败返回False</returns>
        bool OperationInfo(DataTable safeList, out string error);

        /// <summary>
        /// 获得最大序号
        /// </summary>
        /// <returns>返回最大的序号</returns>
        string GetMaxID();

        /// <summary>
        /// 根据总成台数生成物品的安全库存数
        /// </summary>
        /// <param name="code">图号型号</param>
        /// <param name="name">名称</param>
        /// <param name="liFanCount">力帆总成数量</param>
        /// <param name="liFan320Count">力帆320总成数量</param>
        /// <param name="zhongTaiCount">众泰总成数量</param>
        /// <param name="yeMaCount">野马总成数量</param>
        /// <param name="haiMaCount">海马总成数量</param>
        /// <returns>返回安全库存数</returns>
        decimal GetSafeStockCount(string code, string name, decimal liFanCount, decimal liFan320Count,
            decimal zhongTaiCount, decimal yeMaCount, decimal haiMaCount);
                
        /// <summary>
        /// 根据总成台数生成物品的安全库存数
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="dicNumberOfProduct">总成型号与数量构成的字典</param>
        /// <returns>返回安全库存数</returns>
        decimal GetSafeStockCount(int goodsID , Dictionary<string, int> dicNumberOfProduct);
 
        /// <summary>
        /// 获得某个物品的安全库存数
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回安全库存数</returns>
        decimal GetStockCount(int goodsID);

        /// <summary>
        /// 由总成自动生成安全库存表
        /// </summary>
        /// <param name="cvtList">CVT总成数据集</param>
        /// <returns>返回自动生成的数据集</returns>
        DataTable GetSafeStockCountInfo(DataTable cvtList);
    }
}
