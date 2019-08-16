/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IChoseConfectServer.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2010/07/17
 * 开发平台:  Visual C# 2008
 * 用于    :  仓库管理软件
 *----------------------------------------------------------------------------
 * 描述 : 
 * 其它 :
 *----------------------------------------------------------------------------
 * 公共信息: 参见系统'类帮助文档'
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2010/07/17 8:54:12 作者: 夏石友 当前版本: V1.00
 *        修改说明: 创建
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ServerModule
{
    /// <summary>
    /// 零件选配信息管理类接口
    /// </summary>
    public interface IChoseConfectServer
    {
        /// <summary>
        /// 获取选配表表头所有信息
        /// </summary>
        /// <param name="table">返回查询的数据信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>获取成功返回True，获取失败返回False</returns>
        bool GetAllChoseConfectTableHead(out DataTable table, out string error);

        /// <summary>
        /// 添加/修改零件选配信息表
        /// </summary>
        /// <param name="id">序号</param>
        /// <param name="accessoryCode">零部件编码</param>
        /// <param name="rangeData">范围</param>
        /// <param name="productType">产品类型</param>
        /// <param name="choseConfectData">选配数据</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加/修改零件选配信息表</returns>
        bool UpdataAccessoryChoseConfectInfo(string id, string accessoryCode, string rangeData, string productType,
            string choseConfectData, out string error);

        /// <summary>
        /// 删除某一零件选配信息
        /// </summary>
        /// <param name="id">序号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功删除某一零件选配信息</returns>
        bool DeleteAccessoryChoseConfectInfo(string id, out string error);

        /// <summary>
        /// 添加/修改选配表表头
        /// </summary>
        /// <param name="accessoryCode">零部件编码</param>
        /// <param name="tableName">选配表表名</param>
        /// <param name="firstColumn">选配表表头第1列列名</param>
        /// <param name="secondColumn">选配表表头第2列列名</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加/修改选配表表头</returns>
        bool UpdataAccessoryChoseConfectHead(string accessoryCode, string tableName, string firstColumn, string secondColumn, out string error);

        /// <summary>
        /// 删除某一编码的选配表表头
        /// </summary>
        /// <param name="accessoryCode">零部件编码</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功删除某一编码的选配表表头</returns>
        bool DeleteChoseConfectTableHead(string accessoryCode, out string error);

        /// <summary>
        /// 获取选配表表名及表头
        /// </summary>
        /// <param name="accessoryCode">零部件编码</param>
        /// <param name="tableTitle">选配表表名及表头</param>
        /// <param name="rangeTitle">表头</param>
        /// <param name="choseConfectTitle">返回的查询信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是够成功获取选配表表名及表头</returns>
        bool GetChoseConfectTableHead(string accessoryCode, out string tableTitle, out string rangeTitle, out string choseConfectTitle, out string error);

        /// <summary>
        /// 获取某一零件的选配信息
        /// </summary>
        /// <param name="accessoryCode">零部件编码</param>
        /// <param name="productType">产品类型</param>
        /// <param name="table">某一零件的选配信息表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取某一零件的选配信息</returns>
        bool GetAccessoryChoseConfectInfo(string accessoryCode, string productType, out DataTable table, out string error);

        /// <summary>
        /// 获取所有选配零件信息
        /// </summary>
        /// <returns>返回获取到的选配零件信息</returns>
        IQueryable<P_ChoseConfectTable> GetAllChoseConfectInfo();

        /// <summary>
        /// 是否存在指定选配零件信息
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <returns>存在指定的选配零件信息返回true</returns>
        bool IsExistChoseConfectInfo(string goodsCode);

        #region 2012-04-26 增加, 夏石友, 原因：新增的“快速返修变速箱”窗体所需

        /// <summary>
        /// 获取可选配的零件图号型号信息
        /// </summary>
        /// <returns>返回获取到的零件图号型号信息</returns>
        IQueryable<string> GetOptionPartCode();

        #endregion
    }
}
