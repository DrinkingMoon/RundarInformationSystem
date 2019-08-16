/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IWorkShopProductCode.cs
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
using ServerModule;
using System.Collections.Generic;
using System.Data;

namespace Service_Manufacture_WorkShop
{
    /// <summary>
    /// 车间箱体编码服务接口
    /// </summary>
    public interface IWorkShopProductCode
    {
        /// <summary>
        /// 获得车间箱号业务信息
        /// </summary>
        /// <param name="productCode">产品箱号</param>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回Table</returns>
        DataTable GetWorkShopProductCodeBusiness(string productCode, int goodsID);

        /// <summary>
        /// 获得车间箱号信息
        /// </summary>
        /// <param name="wsCode">车间代码</param>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回Table</returns>
        DataTable GetWorkShopProductCodeInfo(string wsCode, int goodsID);

        /// <summary>
        /// 获得车间箱号库存数
        /// </summary>
        /// <param name="wsCode">车间代码</param>
        /// <returns>返回Table</returns>
        DataTable GetWorkShopProductCodeNumber(string wsCode);

        /// <summary>
        /// 获得车间产品箱体编号信息
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="productCode">箱体编号</param>
        /// <param name="storageID">库房代码</param>
        /// <returns>返回LNQ</returns>
        WS_ProductCodeStock GetProductCodeStockInfo(int goodsID, string productCode, string storageID);


        /// <summary>
        /// 检测录入的编码数量是否一致
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="wsCode">车间代码</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="operationType">操作类型</param>
        /// <param name="checkCount">操作数量</param>
        /// <returns>返回True：一致，False 不一致</returns>
        bool CheckProductCodeCount(string billNo, string wsCode, int goodsID, int operationType, decimal checkCount);

        /// <summary>
        /// 箱体编码明细操作
        /// </summary>
        /// <param name="listProductCodes">箱体编码列表</param>
        /// <param name="billNo">单据号</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="dicInfo">字典信息</param>
        void OperatorProductCodeDetail(DataTable listProductCodes, string billNo, int goodsID,
            Dictionary<string, GlobalObject.CE_SubsidiaryOperationType> dicInfo);

        /// <summary>
        /// 箱体编码明细操作
        /// </summary>
        /// <param name="listDetail">明细列表</param>
        void OperatorProductCodeDetail(System.Collections.Generic.List<ServerModule.WS_ProductCodeDetail> listDetail);

        /// <summary>
        /// 箱体编码明细操作
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="listDetail">明细列表</param>
        void OperatorProductCodeDetail(ServerModule.DepotManagementDataContext ctx, 
            System.Collections.Generic.List<ServerModule.WS_ProductCodeDetail> listDetail);

        /// <summary>
        /// 箱体编码库存操作
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">单据号</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="operationType">操作类型</param>
        void OperatorProductCodeStock(DepotManagementDataContext ctx, string billNo, int goodsID, int operationType);

        /// <summary>
        /// 箱体编码库存操作
        /// </summary>
        /// <param name="billNo">单据号</param>
        void OperatorProductCodeStock(string billNo);

        /// <summary>
        /// 删除箱体编码明细
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">单据号</param>
        void DeleteProductCodeDetail(DepotManagementDataContext ctx, string billNo);

        /// <summary>
        /// 删除箱体编码明细
        /// </summary>
        /// <param name="billNo">单据号</param>
        void DeleteProductCodeDetail(string billNo);
    }
}
