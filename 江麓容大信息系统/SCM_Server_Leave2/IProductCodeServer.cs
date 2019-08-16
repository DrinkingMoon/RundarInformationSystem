/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IProductCodeServer.cs
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
using System.Collections.Generic;

namespace ServerModule
{
    /// <summary>
    /// 产品箱号信息服务类接口
    /// </summary>
    public interface IProductCodeServer
    {
        /// <summary>
        /// 是否存在过库存记录
        /// </summary>
        /// <param name="billNo">单据号</param>
        void IsExistProductStock(string billNo);

        /// <summary>
        /// 是否存在过此编号的信息
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="productCode">箱体编码</param>
        /// <param name="goodsID">物品ID</param>
        /// <returns>存在过返回True,否则返回False</returns>
        bool IsProductCodeInfo(DepotManagementDataContext ctx, string productCode, int goodsID);

        /// <summary>
        /// 查询自动入库附表记录
        /// </summary>
        DataTable Sel_AutoCreatePutIn_Subsidiary();

        /// <summary>
        /// 添加自动入库附表记录
        /// </summary>
        /// <param name="info"></param>
        void Add_AutoCreatePutIn_Subsidiary(ProductCode_AutoCreatePutIn_Subsidiary info);

        /// <summary>
        /// 删除自动入库附表记录
        /// </summary>
        /// <param name="info"></param>
        void Del_AutoCreatePutIn_Subsidiary(ProductCode_AutoCreatePutIn_Subsidiary info);

        /// <summary>
        /// 根据申请单号批量插入CVT箱号与业务单据关系信息记录
        /// </summary>
        /// <param name="requisitionBillNo">申请单号列表</param>
        /// <param name="inputBillNo">业务单据号</param>
        void InsertChangeProductCodesBillNo(List<string> requisitionBillNo, string inputBillNo);

        /// <summary>
        /// 更新产品编码库存及库存状态
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="djh">单据号</param>
        /// <param name="marketingType">单据类型</param>
        /// <param name="storageID">库房ID</param>
        /// <param name="isRepaired">是否为已返修 True 是，False 否</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool UpdateProductStock(DepotManagementDataContext context, string djh,
            string marketingType, string storageID, bool isRepaired, int goodsID, out string error);

        /// <summary>
        /// 检查当前编号是否在库房内
        /// </summary>
        /// <param name="productCode">产品编号</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>在库房返回True，不在库房返回False</returns>
        bool IsProductCodeInStock(string productCode,
            int goodsID, string storageID);

        /// <summary>
        /// 检查ProductsCodes表中的数量与所要执行的业务的数量是否一致
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="inCount">执行业务的数量</param>
        /// <param name="djh">单据号</param>
        /// <returns>不一致返回False,一致或者被忽略检测则返回True</returns>
        bool IsFitCount(int goodsID, int inCount, string djh);

        /// <summary>
        /// 针对于领料单检查总成的数量
        /// </summary>
        /// <param name="billNo">领料单号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>一致或者被忽略检测返回True，不一致返回False</returns>
        bool IsFitCountInRequisitionBill(string billNo, out string error);

        /// <summary>
        /// 针对于领料退库单检查总成的数量
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>一致或者被忽略检测返回True，不一致返回False</returns>
        bool IsFitCountInReturnBill(string billNo, out string error);

        /// <summary>
        /// 产品编号处理
        /// </summary>
        /// <param name="productTable">产品编号列表</param>
        /// <param name="code">产品型号</param>
        /// <param name="zcCode">总成号</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="djh">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>处理成功返回True，处理失败返回False</returns>
        bool UpdateProducts(System.Data.DataTable productTable, string code, string zcCode,
            int goodsID, string djh, out string error);

        /// <summary>
        /// 获得产品编码业务信息
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="operationType">业务类型 若为“全  部”则显示全部业务</param>
        /// <param name="productType">产品型号   若为“全  部”则显示全部产品</param>
        /// <returns>返回获得产品编码业务信息</returns>
        DataTable GetProductCodeOperationInfo(DateTime startTime, DateTime endTime, string operationType, string productType);
        
        /// <summary>
        /// 产品总成编码校验
        /// </summary>
        /// <param name="productType">总成编码</param>
        /// <param name="productCode">产品编码</param>
        /// <param name="barCodeType">编码类型</param>
        /// <param name="error">出错时返回的错误信息</param>
        /// <returns>检验通过返回true, 失败返回false</returns>
        bool VerifyProductCodesInfo(string productType, string productCode, GlobalObject.CE_BarCodeType barCodeType, out string error);

        /// <summary>
        /// 产品总成编码校验
        /// </summary>
        /// <param name="goodsID">总成ID</param>
        /// <param name="productCode">产品编码</param>
        /// <param name="barCodeType">编码类型</param>
        /// <param name="error">出错时返回的错误信息</param>
        /// <returns>检验通过返回true, 失败返回false</returns>
        bool VerifyProductCodesInfo(int goodsID, string productCode, GlobalObject.CE_BarCodeType barCodeType, out string error);

    }
}
