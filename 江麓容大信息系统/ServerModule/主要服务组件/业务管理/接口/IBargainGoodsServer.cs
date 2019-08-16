/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IBargainGoodsServer.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2010/09/03
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
using System.Collections;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace ServerModule
{
    /// <summary>
    /// 合同物品信息管理类接口
    /// </summary>
    public interface IBargainGoodsServer : IBasicService
    {
        /// <summary>
        /// 获取或设置是否自动将结果提交到数据库的标志，默认为true
        /// </summary>
        bool AutoSubmitToDatabase
        {
            get;
            set;
        }

        /// <summary>
        /// 操作零星采购单相关业务
        /// </summary>
        /// <param name="bargainBillNo">合同号</param>
        /// <param name="minorPurchaseBillNo">零星采购单号</param>
        void OperatorMinorPurchase(string bargainBillNo, string minorPurchaseBillNo);

        /// <summary>
        /// 判断是否可以操作
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="bargainNumber">合同号</param>
        /// <returns>可以操作 返回True，不可操作 返回False</returns>
        bool IsOperator(int goodsID, string bargainNumber);

        /// <summary>
        /// 获取合同物品表中某一产品单价
        /// </summary>
        /// <param name="bargainNumber">合同物品号</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="provider">供应商</param>
        /// <returns>返回获取到的单价</returns>
        decimal GetGoodsUnitPrice(string bargainNumber, int goodsID, string provider);

        /// <summary>
        /// 获取合同物品信息
        /// </summary>
        /// <param name="bargainNumber">合同号</param>
        /// <param name="returnBargainGoods">返回查询到的信息</param>
        /// <param name="error">错误信息, 没有则为null</param>
        /// <returns>操作是否成功的标志</returns>
        bool GetBargainGoods(string bargainNumber, out IQueryable<View_B_BargainGoods> returnBargainGoods, out string error);

        /// <summary>
        /// 添加合同物品信息
        /// </summary>
        /// <param name="bargainInfo">合同物品信息</param>
        /// <param name="returnBargainGoods">返回查询到的合同物品信息</param>
        /// <param name="error">错误信息, 没有则为null</param>
        /// <returns>操作是否成功的标志</returns>
        bool AddBargainGoods(B_BargainGoods bargainInfo, out IQueryable<View_B_BargainGoods> returnBargainGoods, out string error);

        /// <summary>
        /// 修改合同物品信息
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="newGoods">新合同物品</param>
        /// <param name="returnBargainGoods">返回查询到的合同物品信息</param>
        /// <param name="error">错误信息, 没有则为null</param>
        /// <returns>操作是否成功的标志</returns>
        bool UpdateBargainGoods(int goodsID, B_BargainGoods newGoods, out IQueryable<View_B_BargainGoods> returnBargainGoods, out string error);

        /// <summary>
        /// 修改合同物品信息
        /// </summary>
        /// <param name="dataContext">LINQ数据库上下文</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="newGoods">新合同物品</param>
        /// <param name="returnBargainGoods">返回查询到的合同物品信息</param>
        /// <param name="error">错误信息, 没有则为null</param>
        /// <returns>操作是否成功的标志</returns>
        bool UpdateBargainGoods(DepotManagementDataContext dataContext, int goodsID, B_BargainGoods newGoods, 
            out IQueryable<View_B_BargainGoods> returnBargainGoods, out string error);

        /// <summary>
        /// 修改合同物品信息中的合同号
        /// </summary>
        /// <param name="id">合同物品ID</param>
        /// <param name="newBargainNumber">新合同号</param>
        /// <param name="error">错误信息, 没有则为null</param>
        /// <returns>操作是否成功的标志</returns>
        bool UpdateBargainNumber(int id, string newBargainNumber, out string error);
        
        /// <summary>
        /// 修改合同物品信息中的合同号
        /// </summary>
        /// <param name="dataContext">数据库上下文</param>
        /// <param name="id">合同物品ID</param>
        /// <param name="newBargainNumber">新合同号</param>
        /// <param name="error">错误信息, 没有则为null</param>
        /// <returns>操作是否成功的标志</returns>
        bool UpdateBargainNumber(DepotManagementDataContext dataContext, int id, string newBargainNumber, out string error);

        /// <summary>
        /// 删除合同物品信息
        /// </summary>
        /// <param name="bargainNumber">合同号</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="returnBargainGoods">返回查询到的合同物品信息</param>
        /// <param name="error">错误信息, 没有则为null</param>
        /// <returns>操作是否成功的标志</returns>
        bool DeleteBargainGoods(string bargainNumber, int goodsID,
            out IQueryable<View_B_BargainGoods> returnBargainGoods, out string error);

        /// <summary>
        /// 根据合同号获取合同物品信息
        /// </summary>
        /// <param name="bargainNumber">合同编号</param>
        /// <returns>返回获取到的合同物品信息</returns>
        IQueryable<B_BargainGoods> GetBargainGoodsInfo(string bargainNumber);
    }
}
