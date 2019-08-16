/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IAssemblyInfoServer.cs
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
using System.Text;
using System.Linq;
using System.Collections;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace ServerModule
{
    /// <summary>
    /// Bom附属表服务类接口
    /// </summary>
    public interface IAssemblyInfoServer : IBasicService
    {
        /// <summary>
        /// 获取指定产品的所有选配零件信息
        /// </summary>
        /// <returns>选配信息结果集</returns>
        IQueryable GetAllChoseConfectPartInfo();

        /// <summary>
        /// 获取附属BOM表中满足版本及总成标志的信息
        /// </summary>
        /// <param name="edition">产品版本</param>
        /// <param name="assemblyFlag">总成标志</param>
        /// <returns>返回获取到的信息</returns>
        IQueryable<P_PertainProductBom> GetPertainProductBomInfo(string edition, int assemblyFlag);

        /// <summary>
        /// 获取总成信息
        /// </summary>
        /// <param name="assemblyFlag">是否为总成标志</param>
        /// <param name="edition">产品型号</param>
        /// <param name="returnAccessory">返回的Table</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool GetPertainProductBomInfo(string assemblyFlag, string edition, out DataTable returnAccessory, out string error); 

        /// <summary>
        /// 添加Bom附属表
        /// </summary>
        /// <param name="productType">产品类型</param>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <param name="assemblyFlag">总成标志</param>
        /// <param name="returnAccessory">返回的数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool AddPertainProductBomInfo(string productType, string goodsCode, string goodsName, string spec, int assemblyFlag, out DataTable returnAccessory, out string error);

        /// <summary>
        /// 删除Bom附属表
        /// </summary>
        /// <param name="id">序号</param>
        /// <param name="productType">产品型号</param>
        /// <param name="assemblyFlag">总成标志</param>
        /// <param name="returnTable">返回的数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功删除单位信息</returns>
        bool DeletePertainProductBomInfo(string id, string productType, int assemblyFlag, out DataTable returnTable, out string error);
    }
}
