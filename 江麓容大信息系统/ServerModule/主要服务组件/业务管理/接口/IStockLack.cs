using System;
using System.Data;
using System.Collections.Generic;
using GlobalObject;


namespace ServerModule
{
    /// <summary>
    /// 库房报缺服务接口
    /// </summary>
    public interface IStockLack
    {
        /// <summary>
        /// 获得自定义模板明细信息
        /// </summary>
        /// <param name="listID">模板ID</param>
        /// <returns>返回Table</returns>
        DataTable GetCustomTemplatesList(int listID);

        /// <summary>
        /// 获得自定义模板信息
        /// </summary>
        /// <returns>返回Table</returns>
        DataTable GetCustomTemplatesMain();

        /// <summary>
        /// 操作自定义模板明细
        /// </summary>
        /// <param name="mode">操作模式</param>
        /// <param name="list">LNQ信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False </returns>
        bool OperationList(CE_OperatorMode mode, S_StockLackCustomTemplatesList list, out string error);

        /// <summary>
        /// 自定义模板操作模板
        /// </summary>
        /// <param name="mode">操作模式</param>
        /// <param name="main">LNQ信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True, 失败返回False</returns>
        bool OperationMain(CE_OperatorMode mode, S_StockLackCustomTemplates main, out string error);

        /// <summary>
        /// 获得BOM表信息
        /// </summary>
        /// <param name="cvtType">产品类型</param>
        /// <returns>返回table</returns>
        DataTable GetBomTable(string cvtType);

        /// <summary>
        /// 报缺查询
        /// </summary>
        /// <param name="strat">开始日期字符串</param>
        /// <param name="end">结束日期字符串</param>
        /// <param name="storageID">库房ID</param>
        /// <param name="productName">产品名称</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回查询到的信息</returns>
        DataTable ReportWanting(string strat, string end, string storageID,
            string productName, out string error);

        /// <summary>
        /// 插入临时表
        /// </summary>
        /// <param name="code">产品型号</param>
        /// <param name="count">数量</param>
        void AddTempTable(string code, decimal count);

        /// <summary>
        /// 清空临时表
        /// </summary>
        void ClearTempTable();

        /// <summary>
        /// 获得单一的BOM清单信息
        /// </summary>
        /// <param name="listGoods">BOM物品信息列表</param>
        /// <returns>返回Table</returns>
        DataTable SetSingleBom(List<string> listGoods);

    }
}
