/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IPlanCostBillServer.cs
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
using System.Collections;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace ServerModule
{
    /// <summary>
    /// 物品、零件计划价格管理类接口
    /// </summary>
    public interface IBasicGoodsServer : IBasicService
    {
        /// <summary>
        /// 获得产品编码信息列表
        /// </summary>
        /// <param name="atrributeRecordID">属相记录ID</param>
        /// <returns>返回List</returns>
        List<F_ProductWaterCode> GetWaterCodeListInfo(int atrributeRecordID);

        /// <summary>
        /// 获取指定ID的物品信息
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="id">物品ID</param>
        /// <returns>成功返回获取到的信息,失败返回null</returns>
        F_GoodsPlanCost GetGoodsInfo(DepotManagementDataContext dataContext, int id);

        F_GoodsAttributeRecord GetGoodsAttirbuteRecord(int goodsID, int attributeID);
        
        /// <summary>
        /// 获得物品信息（筛选物品属性）
        /// </summary>
        /// <param name="strWhere">筛选信息</param>
        /// <returns>返回Table</returns>
        DataTable GetGoodsInfoSiftAttribute(string strWhere);

        /// <summary>
        /// 获得包装数量
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="provider">供应商</param>
        /// <returns>返回包装数</returns>
        decimal GetPackCount(int goodsID, string provider);

        /// <summary>
        /// 编辑基础物品信息
        /// </summary>
        /// <param name="goodsCost">基础物品主要信息</param>
        /// <param name="listRecord">属性记录列表</param>
        /// <param name="listReplace">替换件信息列表</param>
        /// <param name="listBlank">毛坯对应成品列表</param>
        /// <param name="listWaterCode">产品编码列表</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        bool EditGoodsInfo(F_GoodsPlanCost goodsCost, List<F_GoodsAttributeRecord> listRecord,
            List<View_F_GoodsReplaceInfo> listReplace, List<View_F_GoodsBlankToProduct> listBlank, 
            List<F_ProductWaterCode> listWaterCode, out string error);

        /// <summary>
        /// 获得毛坯所属成品信息集合
        /// </summary>
        /// <param name="atrributeRecordID">属相记录ID</param>
        /// <returns>返回List</returns>
        List<View_F_GoodsBlankToProduct> GetBlankToProductListInfo(int atrributeRecordID);

        /// <summary>
        /// 获得替换件信息集合
        /// </summary>
        /// <param name="atrributeRecordID">属相记录ID</param>
        /// <returns>返回List</returns>
        List<View_F_GoodsReplaceInfo> GetReplaceListInfo(int atrributeRecordID);

        /// <summary>
        /// 获得物品ID(没有便插入一个新的物品记录)
        /// </summary>
        /// <param name="code">图号型号</param>
        /// <param name="name">物品名称</param>
        /// <param name="spec">规格</param>
        /// <param name="goodsType">材料类别</param>
        /// <param name="unitID">单位ID</param>
        /// <param name="remark">备注</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回获得物品ID，返回0则表示获取失败</returns>
        int GetGoodsID(string code, string name, string spec, string goodsType, int unitID, string remark, out string error);

        /// <summary>
        /// 获得物品基础表的物品名称
        /// </summary>
        /// <returns>返回获得的物品名称列表</returns>
        DataTable GetDistinctGoodsName();

        /// <summary>
        /// 获得物品基础表的规格
        /// </summary>
        /// <returns>返回获得的规格列表</returns>
        DataTable GetDistinctSpec();

        /// <summary>
        /// 获得物品基础表的图号型号
        /// </summary>
        /// <returns>返回获得的图号型号列表</returns>
        DataTable GetDistinctGoodsCode();

        /// <summary>
        /// 获取物品类别(应用于报检入库单，防止图号一样而名称不同的现象)
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="spec">物品规格</param>
        /// <returns>找到返回类别，否则返回null</returns>
        string GetGoodsType(string goodsCode, string spec);

        /// <summary>
        /// 获取物品类别
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">物品规格</param>
        /// <returns>找到返回类别，否则返回null</returns>
        string GetGoodsType(string goodsCode, string goodsName, string spec);

        /// <summary>
        /// 判断某物品信息是否存在
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>存在返回true</returns>
        bool IsExist(string goodsCode, string goodsName, string spec, out string error);

        /// <summary>
        /// 检查业务中是否存在此物品相关信息
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="prompt">存在时返回的提示信息</param>
        /// <returns>存在返回true, 不存在返回false</returns>
        bool IsExistInBusiness(int goodsID, out string prompt);

        /// <summary>
        /// 获取物品的计划单价
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="planUnitPrice">计划单价</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取物品的计划单价</returns>
        bool GetPlanUnitPrice(int goodsID, out decimal planUnitPrice, out string error);

        /// <summary>
        /// 获取物品的计划单价
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="spec">规格</param>
        /// <param name="planUnitPrice">计划单价</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取物品的计划单价</returns>
        bool GetPlanUnitPrice(string goodsCode, string spec, out decimal planUnitPrice, out string error);

        /// <summary>
        /// 获取物品的计划单价
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <param name="planUnitPrice">计划单价</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取物品的计划单价</returns>
        bool GetPlanUnitPrice(string goodsCode, string goodsName, string spec, out decimal planUnitPrice, out string error);

        /// <summary>
        /// 获取物品信息
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回获取到的物品信息, 失败返回null</returns>
        View_F_GoodsPlanCost GetGoodsInfo(string goodsCode, string goodsName, string spec, out string error);

        /// <summary>
        /// 获取物品信息(应用于报检入库单，防止图号一样而名称不同的现象)
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="spec">物品规格</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>找到返回物品信息，否则返回null</returns>
        View_F_GoodsPlanCost GetGoodsInfo(string goodsCode, string spec, out string error);

        /// <summary>
        /// 获取所有物品信息
        /// </summary>
        /// <param name="returnInfo">返回查询到的信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功</returns>
        bool GetAllGoodsInfo(out IQueryable<View_BASE_GoodsPlanCost> returnInfo, out string error);
        
        ///// <summary>
        ///// 获取包含在仓库类别列表中的所有信息
        ///// </summary>
        ///// <param name="userCode">用户编码</param>
        ///// <returns>返回获取到的信息</returns>
        //IQueryable<View_F_GoodsPlanCost> GetGoodsInfo(string userCode);

        /// <summary>
        /// 获取指定ID的物品信息
        /// </summary>
        /// <param name="id">物品ID</param>
        /// <returns>成功返回获取到的信息,失败返回null</returns>
        View_F_GoodsPlanCost GetGoodsInfoView(int id);

        /// <summary>
        /// 获取指定ID的物品信息
        /// </summary>
        /// <param name="id">物品ID</param>
        /// <returns>成功返回获取到的信息,失败返回null</returns>
        F_GoodsPlanCost GetGoodsInfo(int id);

        ///// <summary>
        ///// 获取包含在仓库类别列表中的所有信息
        ///// </summary>
        ///// <param name="lstDepotCode">仓库类别列表</param>
        ///// <returns>返回获取到的信息</returns>
        //IQueryable<View_F_GoodsPlanCost> GetGoodsInfo(List<string> lstDepotCode);

        /// <summary>
        /// 获取指定用户查询权限内的包含在仓库类别列表中的所有信息
        /// </summary>
        /// <param name="blUsing">是否仅显示在用</param>
        /// <returns>返回获取到的信息</returns>
        IQueryable<View_F_GoodsPlanCost> GetGoodsInfo(bool blUsing);

        /// <summary>
        /// 删除指定ID的物品信息
        /// </summary>
        /// <param name="id">序号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功的标志</returns>
        bool DeleteGoods(int id, out string error);

        /// <summary>
        /// 删除指定ID的物品信息
        /// </summary>
        /// <param name="id">序号</param>
        /// <param name="returnInfo">返回查询到的信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功的标志</returns>
        bool DeleteGoods(int id, out IQueryable<View_BASE_GoodsPlanCost> returnInfo, out string error);

        /// <summary>
        /// 获得物品ID
        /// </summary>
        /// <param name="goodsCode">图号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <returns>成功返回获取到的物品ID，失败返回0</returns>
        int GetGoodsID(string goodsCode, string goodsName, string spec);

        /// <summary>
        /// 根据CVT型号获得CVT信息
        /// </summary>
        /// <param name="CVTCode">CVT型号</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>成功返回CVT信息，失败返回null</returns>
        DataTable GetCVTInfo(string CVTCode, string storageID);

        /// <summary>
        /// 通过图号型号查找物品的序号
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <returns>成功返回序号，失败返回null</returns>
        int GetGoodsIDByGoodsCode(string goodsCode, string goodsName, string spec);

        /// <summary>
        /// 获得基础物品属性列表
        /// </summary>
        /// <returns>返回List</returns>
        List<F_GoodsAttribute> GetGoodsAttributeList();

        /// <summary>
        /// 获得属性对象
        /// </summary>
        /// <param name="attributeID">属性ID</param>
        /// <returns>返回属性对象</returns>
        F_GoodsAttribute GetGoodsAttirbute(int attributeID);

        /// <summary>
        /// 获得物品的属性列表
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回属性列表</returns>
        List<F_GoodsAttributeRecord> GetGoodsAttirbuteRecordList(int goodsID);
        
        /// <summary>
        /// 编辑某条属性记录
        /// </summary>
        /// <param name="record">属性记录对象</param>
        void SaveGoodsAttirbute(F_GoodsAttributeRecord record);
    }
}
