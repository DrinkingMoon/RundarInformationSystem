/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IGeneratesCheckOutInDepotServer.cs
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
    /// 自动生成入库单管理类接口
    /// </summary>
    public interface IGeneratesCheckOutInDepotServer
    {
        /// <summary>
        /// 获得可以自动生成的入库单信息
        /// </summary>
        /// <param name="provider">供应商</param>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="gpec">规格</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回可以自动生成的入库单信息</returns>
        DataTable GetAllInfo(string provider,
            string goodsCode, string goodsName, string gpec, out string error);

        /// <summary>
        /// 自动插入报检入库单
        /// </summary>
        /// <param name="dateTable">需要插入的数据集</param>
        /// <param name="storageID">库房ID</param>
        /// <param name="version">版次号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool AddCheckInDepotBill(DataTable dateTable, string storageID, string version, out string error);
    }
}
