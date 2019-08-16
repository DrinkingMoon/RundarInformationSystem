/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IGoodsLeastPackAndStock.cs
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
    /// 供应商物品采购配置服务接口（最小采购量、最小包装数）
    /// </summary>
    public interface IGoodsLeastPackAndStock
    {
        /// <summary>
        /// 向供应商配额设置表中添加数据
        /// </summary>
        /// <param name="inLeast">供应商物品采购配额信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool AddInfo(B_GoodsLeastPackAndStock inLeast, out string error);

        /// <summary>
        /// 删除供应商配额设置表中的一条数据
        /// </summary>
        /// <param name="id">序号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool DeleteInfo(int id, out string error);

        /// <summary>
        /// 获得供应商物品采购配置的所有信息
        /// </summary>
        /// <returns>返回获取到供应商物品采购配置的所有记录</returns>
        DataTable GetAllInfo();

        /// <summary>
        /// 修改供应商配额设置表中的一条数据
        /// </summary>
        /// <param name="inLeast">供应商物品采购配额信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool UpdateInfo(B_GoodsLeastPackAndStock inLeast, out string error);

        /// <summary>
        /// 获得最大序号
        /// </summary>
        /// <returns>返回获取到的最大序号</returns>
        string GetMaxID();

        /// <summary>
        /// 获得BOM表零部件不在安全库存或配额中
        /// </summary>
        /// <param name="editionSql">产品类型</param>
        /// <param name="whereSql">剔除条件</param>
        /// <param name="DataTableName">表名，与Bom相比较的表</param>
        /// <returns>返回满足条件的数据集</returns>
        DataTable GetGoodsUseBomTable(string editionSql,string whereSql, string DataTableName);

        /// <summary>
        /// 获得安全库存或配额中存在没有使用的零件
        /// </summary>
        /// <param name="whereSql">剔除条件</param>
        /// <param name="DataTableName">表名，与Bom相比较的表</param>
        /// <returns>返回满足条件的数据集</returns>
        DataTable GetGoodsNotUseBomTable(string whereSql, string DataTableName);
    }
}
