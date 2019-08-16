/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IGoodsAntirust.cs
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
using System.Data;

namespace ServerModule
{
    /// <summary>
    /// 物品防锈管理类接口
    /// </summary>
    public interface IGoodsAntirust
    {
        /// <summary>
        /// 添加防锈物品
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="antirustTime">防锈期</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool AddAntirustInfo(int goodsID, decimal antirustTime, out string error);

        /// <summary>
        /// 审核防锈
        /// </summary>
        /// <param name="goodsTable">需要执行的物品数据表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool AuditingAntirustInfo(DataTable goodsTable, out string error);

        /// <summary>
        /// 删除防锈物品
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool DeleteAntirustInfo(int goodsID, out string error);

        /// <summary>
        /// 执行防锈
        /// </summary>
        /// <param name="goodsTable">需要执行防锈的物品数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool ExceAntirustInfo(DataTable goodsTable, out string error);

        /// <summary>
        /// 获得设置防锈期的表
        /// </summary>
        /// <returns>返回防锈期设置表</returns>
        DataTable GetBaseGoodsAntirustSet();

        /// <summary>
        /// 获得库存物品的防锈信息表
        /// </summary>
        /// <returns>返回库存物品的防锈信息表</returns>
        DataTable GetStockAntirustCheck();

        /// <summary>
        /// 获得最大ID
        /// </summary>
        /// <returns>返回string 最大ID</returns>
        string GetMaxID();

        /// <summary>
        /// 确认防锈
        /// </summary>
        /// <param name="goodsTable">需要执行的物品数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>True 成功 false  失败</returns>
        bool AuthorizeAntirustInfo(DataTable goodsTable, out string error);
    }
}
