/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IInDepotGoodsBarCodeServer.cs
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
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using DBOperate;
using PlatformManagement;

namespace ServerModule
{
    /// <summary>
    /// 物品条形码管理类接口
    /// </summary>
    public interface IBarCodeServer
    {
        DataTable ShowBoardBarCodeRecord();

        void AddBoardBarCodeRecord(P_PrintBoardForVehicleBarcode barCode);

        void DeleteBoardBarCodeRecord(string barCode);

        /// <summary>
        /// 获得条形码
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <param name="provider">供应商编码</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>条形码号</returns>
        int GetBarCode(int goodsID, string batchNo, string storageID, string provider);

        /// <summary>
        /// 获得条形码是否存在表中
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="storageID">库房ID</param>
        /// <param name="batchNo">批次号</param>
        /// <param name="provider">供应商编码</param>
        /// <returns>条形码存在表中返回True,条形码不存在表中返回False</returns>
        bool IsExists(int goodsID, string storageID, string batchNo, string provider);

        /// <summary>
        /// 获取条形码管理表
        /// </summary>
        /// <param name="goodsCode">图号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <param name="provider">供应商</param>
        /// <param name="batchCode">批次号</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>找到则返回一条条形码管理视图记录，否则返回null</returns>
        View_S_InDepotGoodsBarCodeTable GetBarCodeInfo
            (string goodsCode, string goodsName,
            string spec, string provider,
            string batchCode, string storageID);
 
        /// <summary>
        /// 在添加库存信息时添加条形码信息
        /// </summary>
        /// <param name="context">数据库上下文</param>
        /// <param name="stockInfo">S_Stock表的一条库存信息</param>
        void Add(DepotManagementDataContext context, S_Stock stockInfo);

        /// <summary>
        /// 向条形码管理表中添加一条记录
        /// </summary>
        /// <param name="barcode">要添加的一条条形码管理视图条形码信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>添加成功返回True，添加失败返回False</returns>
        /// <remarks>打印条形码时如果找不到此物品的条形码时直接生成条形码用</remarks>
        bool Add(S_InDepotGoodsBarCodeTable barcode, out string error);
 
        /// <summary>
        /// 在删除库存信息时删除条形码信息
        /// </summary>
        /// <param name="context">数据库上下文</param>
        /// <param name="stockInfo">库存信息</param>
        void Delete(DepotManagementDataContext context, S_Stock stockInfo);
        
        /// <summary>
        /// 在更新库存信息时更新条形码信息
        /// </summary>
        /// <param name="context">数据库上下文</param>
        /// <param name="oldInfo">旧库存信息</param>
        /// /// <param name="newInfo">新库存信息</param>
        void Update(DepotManagementDataContext context, S_Stock oldInfo, S_Stock newInfo);

        /// <summary>
        /// 获取条形码信息
        /// </summary>
        /// <param name="returnTable">返回的查询结果</param>
        /// <param name="whereCondition">where语句的查询条件</param>
        /// <param name="error">输出的错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        bool GetBarCodeInfo(out IQueryResult returnTable, string whereCondition, out string error);

        /// <summary>
        /// 获取某一条形码的货物信息
        /// </summary>
        /// <param name="barCode">条形码</param>
        /// <param name="goodsInfo">物品信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool GetData(int barCode, out S_InDepotGoodsBarCodeTable goodsInfo, out string error);

        /// <summary>
        /// 获取某一条形码的货物信息
        /// </summary>
        /// <param name="barCode">条形码</param>
        /// <param name="goodsInfo">物品信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool GetData(int barCode, out View_S_InDepotGoodsBarCodeTable goodsInfo, out string error);

        /// <summary>
        /// 获取某一条形码的货物信息
        /// </summary>
        /// <param name="barCode">条形码</param>
        /// <param name="returnTable">获取成功时返回所获取的某一条形码的货物信息</param>
        /// <param name="error">获取失败时返回错误信息</param>
        /// <returns>获取成功时返回True,获取失败时返回False</returns>
        bool GetData(string barCode, out DataTable returnTable, out string error);
    }
}
