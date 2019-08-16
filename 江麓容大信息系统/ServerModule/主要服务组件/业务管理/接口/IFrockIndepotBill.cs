/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IFrockIndepotBill.cs
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
using PlatformManagement;


namespace ServerModule
{
    /// <summary>
    /// 自制件工装报检接口类
    /// </summary>
    public interface IFrockIndepotBill : IBasicService,IBasicBillServer
    {
        /// <summary>
        /// 添加自制件工装
        /// </summary>
        /// <param name="bill">单据信息</param>
        /// <param name="returnInfo">添加完毕后查询数据库的返回结果</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作结果</returns>
        bool AddBill(S_FrockInDepotBill bill, out IQueryResult returnInfo, out string error);

        /// <summary>
        /// 添加自制件工装报检物品
        /// </summary>
        /// <param name="billNo">单据编号</param>
        /// <param name="goods">物品信息</param>
        /// <param name="returnInfo">操作完毕后查询数据库的返回结果</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool AddGoods(string billNo, S_FrockInDepotGoodsBill goods,
            out IQueryable<View_S_FrockInDepotGoodsBill> returnInfo, out string error);

        /// <summary>
        /// 批量删除普通入库单物品
        /// </summary>
        /// <param name="lstId">物品ID列表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool DeleteGoods(System.Collections.Generic.List<int> lstId, out string error);

        /// <summary>
        /// 删除指定单据的所有物品信息
        /// </summary>
        /// <param name="billNo">要删除的物品单据号</param>
        /// <param name="returnInfo">操作完毕后查询数据库的返回结果</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功的标志</returns>
        bool DeleteGoods(string billNo, out System.Linq.IQueryable<View_S_FrockInDepotGoodsBill> returnInfo, out string error);

        /// <summary>
        /// 获取自制件工装报检信息
        /// </summary>
        /// <param name="returnInfo">自制件工装报检单</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool GetAllBill(out PlatformManagement.IQueryResult returnInfo, out string error);

        /// <summary>
        /// 获取自制件工装报检信息
        /// </summary>
        /// <param name="billNo">单据编号</param>
        /// <returns>成功返回获取到的单据信息,失败返回null</returns>
        S_FrockInDepotBill GetBill(string billNo);

        /// <summary>
        /// 获取指定单据物品信息
        /// </summary>
        /// <param name="billNo">单据编号</param>
        /// <returns>返回获取到的物品信息</returns>
        System.Linq.IQueryable<View_S_FrockInDepotGoodsBill> GetGoodsInfo(string billNo);

        /// <summary>
        /// 获取指定单据物品信息
        /// </summary>
        /// <param name="billNo">单据编号</param>
        /// <returns>返回获取到的物品信息</returns>
        System.Linq.IQueryable<View_S_FrockInDepotGoodsBill> GetGoodsViewInfo(string billNo);

        /// <summary>
        /// 获取包含指定物品编号的信息
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回获取到的物品信息</returns>
        System.Linq.IQueryable<S_FrockInDepotGoodsBill> GetGoodsViewInfo(int goodsID);

        /// <summary>
        /// 检查某单据是否存在
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>存在返回true</returns>
        new bool IsExist(string billNo);

        /// <summary>
        /// 提交入库信息
        /// </summary>
        /// <param name="inDepotInfo">入库信息, 只取其中入库部分</param>
        /// <param name="returnInfo">添加完毕后查询数据库的返回结果</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作结果</returns>
        bool SubmitInDepotInfo(S_FrockInDepotBill inDepotInfo, out PlatformManagement.IQueryResult returnInfo, out string error);

        /// <summary>
        /// 机加人员提交单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="flag">操作标志</param>
        /// <param name="returnInfo">返回更新后重新查询的数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功</returns>
        bool SubmitNewBill(string billNo, bool flag, out IQueryResult returnInfo, out string error);

        /// <summary>
        /// 更新普通入库单物品
        /// </summary>
        /// <param name="goods">物品信息</param>
        /// <param name="returnInfo">操作完毕后查询数据库的返回结果</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool UpdateGoods(S_FrockInDepotGoodsBill goods, out IQueryable<View_S_FrockInDepotGoodsBill> returnInfo, out string error);
       
        /// <summary>
        /// 删除自制件工装报检
        /// </summary>
        /// <param name="billNo">单据号号</param>
        /// <param name="returnInfo">自制件工装报检</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功删除自制件工装报检</returns>
        bool DeleteBill(string billNo, out PlatformManagement.IQueryResult returnInfo, out string error);
    }
}
