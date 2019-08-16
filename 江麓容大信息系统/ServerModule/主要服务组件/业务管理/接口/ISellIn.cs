/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  ISellIn.cs
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using PlatformManagement;
using GlobalObject;


namespace ServerModule
{

    /// <summary>
    /// 编制人枚举
    /// </summary>
    public enum PersonnelType
    {
        /// <summary>
        /// 编制人
        /// </summary>
        编制人,

        /// <summary>
        /// 审核人
        /// </summary>
        审核人,

        /// <summary>
        /// 仓管员
        /// </summary>
        仓管员
    }

    /// <summary>
    /// 营销管理类接口
    /// </summary>
    public interface ISellIn : IBasicBillServer
    {
        /// <summary>
        /// 添加特殊放行记录
        /// </summary>
        /// <param name="lstInfo">对象列表</param>
        void AddList_ProductCodesGreenLight(List<ProductsCode_GreenLight> lstInfo);

        /// <summary>
        /// 获得条形码Table
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回Table</returns>
        DataTable GetBarcodeTable(string billNo);

        /// <summary>
        /// 获得单据号
        /// </summary>
        /// <returns>返回最大单据ID</returns>
        int GetBillID();

        /// <summary>
        /// 检测单据是否存在异常箱号
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>True: 存在 False : 不存在</returns>
        bool IsExistAbnomalProductCode(string billNo);

        /// <summary>
        /// 单据的数据库操作
        /// </summary>
        /// <param name="listInfo">明细信息</param>
        /// <param name="billInfo">单据信息，若ID=0则添加，否则更新</param>
        /// <param name="marketingType">单据类型</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool UpdateBill(DataTable listInfo, DataRow billInfo, string marketingType, out string error);


        /// <summary>
        /// 获得单据明细
        /// </summary>
        /// <param name="djID">单据ID</param>
        /// <returns>返回获取的单据明细信息</returns>
        DataTable GetList(int djID);

        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="djID">单据ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        bool DeleteBill(int djID, out string error);

        /// <summary>
        /// 仓管确认
        /// </summary>
        /// <param name="djID">单据ID</param>
        /// <param name="marketingType">单据类型</param>
        /// <param name="listInfo">明细信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>确认成功返回True，确认失败返回False</returns>
        bool AffrimBill(int djID, CE_MarketingType marketingType, DataTable listInfo, out string error);

        /// <summary>
        /// 变更单据状态（审核）
        /// </summary>
        /// <param name="djID">单据ID</param>
        /// <param name="remark">备注</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回false</returns>
        bool AuditingBill(int djID, string remark, out string error);

        /// <summary>
        /// 获得单据信息
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <param name="djID">单据ID</param>
        /// <returns>返回单据信息</returns>
        DataTable GetBill(string djh, int djID);

        /// <summary>
        /// 获得批次号
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="marketingType">单据类型</param>
        /// <returns>返回批次号</returns>
        string GetBatchNo(int goodsID, string marketingType);

        /// <summary>
        /// 编辑检验状态
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <param name="remark">备注</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>编辑成功返回True，编辑失败返回False</returns>
        bool ExamineBill(string djh, string remark, out string error);


        /// <summary>
        /// 获得箱子批次
        /// </summary>
        /// <param name="prefix">前缀</param>
        /// <returns>返回箱体批次号</returns>
        string GetBoxNo(string prefix);

        /// <summary>
        /// 权限过滤查询
        /// </summary>
        /// <param name="type">查询类型 ("入库","出库","退库","退货")</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="djzt">单据状态</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回查询到的数据信息</returns>
        DataTable GetAllBill(string type, string startDate, string endDate, string djzt, out string error);

        /// <summary>
        /// 检查库存产品返修状态是否为已返修
        /// </summary>
        /// <param name="productCode">产品编号</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>为已返修返回True,不在库房或者不为已返修返回False</returns>
        bool IsRepaired(string productCode, int goodsID, string storageID);

        /// <summary>
        /// 检查库存产品状态是否为正常
        /// </summary>
        /// <param name="productCode">产品编号</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>为正常返回True，不在库房或者非正常返回False</returns>
        bool IsNatural(string productCode, int goodsID, string storageID);

        /// <summary>
        /// 操作产品库存(仅限于调拨)
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="djh">单据号</param>
        /// <param name="marketingType">单据类型</param>
        /// <param name="outStorageID">调出库房</param>
        /// <param name="inStorageID">调入库房</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool UpdateProductStock(DepotManagementDataContext context, string djh, string marketingType, string outStorageID, string inStorageID, out string error);

        /// <summary>
        /// 获得库存的产品编号总数
        /// </summary>
        /// <param name="storageID">库房ID</param>
        /// <returns>返回查询到的库存产品编号信息</returns>
        DataTable GetStockProductCodeCountInfo(string storageID);

        /// <summary>
        /// 查询产品编号的业务
        /// </summary>
        /// <param name="productCode">产品编号</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>返回查询到的产品编号业务信息</returns>
        DataTable GetProductCodeOperationInfo(string productCode, int goodsID, string storageID);

        /// <summary>
        /// 判断是否打印单据
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <returns>已打印返回True，未打印返回False</returns>
        bool IsPrint(string djh);

        /// <summary>
        /// 获得整盒的TCU
        /// </summary>
        /// <param name="boxNo">TCU盒号</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>返回查询到的整盒TCU信息</returns>
        DataTable GetBoxInfo(string boxNo, string storageID);

        /// <summary>
        /// 获得箱子编号
        /// </summary>
        /// <param name="productCode">产品编号</param>
        /// <returns>返回查询到的箱子编号</returns>
        string GetHoldBoxNo(string productCode);

        /// <summary>
        /// 获得外部库存数
        /// </summary>
        /// <returns>返回查询的外部库存数的信息</returns>
        DataTable GetOutStockInfo();

        /// <summary>
        /// 删除一条外部库存记录
        /// </summary>
        /// <param name="outStock">外部库存信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        bool DeleteOutStockInfo(S_OutStock outStock, out string error);

        /// <summary>
        /// 更新外部库存数
        /// </summary>
        /// <param name="outStock">外部库存信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>更新成功返回True，更新失败返回False</returns>
        bool UpdateOutStockInfo(S_OutStock outStock, out string error);

        /// <summary>
        /// 添加外部库存数据
        /// </summary>
        /// <param name="outStock">外部库存信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>添加成功返回True，添加失败返回False</returns>
        bool AddOutStockInfo(S_OutStock outStock, out string error);

        /// <summary>
        /// 获得更新的外部库存数
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>返回查询到的外部库存数的信息</returns>
        DataTable GetOutStockInfo(int goodsID, string storageID);

        /// <summary>
        /// 当退出界面时删除已提交未保存的产品编码
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        bool DeleteProductCodeInfo(string djh, out string error);

        /// <summary>
        /// 查询客户信息
        /// </summary>
        /// <param name="goodsID">产品ID</param>
        /// <param name="productCode">箱体编号</param>
        /// <returns>返回查询到的客户信息</returns>
        DataTable GetCustomerInfo(int goodsID, string productCode);

        /// <summary>
        /// 查询装车信息
        /// </summary>
        /// <param name="goodsID">产品ID</param>
        /// <param name="productCode">箱体编号</param>
        /// <returns>返回查询到的装车信息</returns>
        DataTable GetLoadingInfo(int goodsID, string productCode);

        /// <summary>
        /// 检查业务是否匹配
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="productCode">产品编码</param>
        /// <param name="marktingType">业务类型</param>
        /// <returns>TRUE成功匹配，FALSE匹配失败</returns>
        bool IsOperationMatching(int goodsID, string productCode, string marktingType);

        /// <summary>
        /// 获得下线库存
        /// </summary>
        /// <param name="storageName">库房名称</param>
        /// <returns>返回查询的下线库存信息</returns>
        DataTable GetInsertingCoilStockInfo(string storageName);

        /// <summary>
        /// 查询对应的编码状态
        /// </summary>
        /// <param name="storageID">库房ID</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="type">查询类型 ("库房","下线","TCU")</param>
        /// <param name="productCode">产品编码</param>
        /// <param name="version">版次号</param>
        /// <returns>返回查询结果集</returns>
        DataTable GetStockProductCodeInfo(string storageID, int goodsID, string type, string productCode, string version);

        /// <summary>
        /// 查询出厂检验数据
        /// </summary>
        /// <param name="productCode">产品编号</param>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回查询的出厂检验信息</returns>
        DataTable GetDeliveryInspectionInfo(string productCode, int goodsID);

        /// <summary>
        /// 获得自动入库的入库方式
        /// </summary>
        /// <param name="producrType">产品类型 ("0公里返修退货",售后返修退货","售后已修退货","批量生产退货","0公里批量返修退货","新箱","未知")</param>
        /// <param name="productCode">产品编码</param>
        /// <returns>成功返回获取到的入库方式，失败返回“未知”</returns>
        string GetInStockWay(string producrType, string productCode);

        /// <summary>
        /// 获得业务方式
        /// </summary>
        /// <param name="producrType">产品类型</param>
        /// <param name="productCode">产品编码</param>
        /// <returns>成功返回获取到的业务方式，失败返回“未知”</returns>
        string GetOperationWay(string producrType, string productCode);

        /// <summary>
        /// 批量生成入库单
        /// </summary>
        /// <param name="insertTable">需要插入的信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>插入成功返回True，插入失败返回False</returns>
        bool BatchCreateBill(DataTable insertTable, out string error);

        /// <summary>
        /// 查看匹配表
        /// </summary>
        /// <returns>返回查询的匹配表信息</returns>
        DataTable GetProductCodeMatchingInfo();

        /// <summary>
        /// 获得单号
        /// </summary>
        /// <returns>返回获得的单号</returns>
        string GetMatchingBillID();

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="insert">匹配信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>插入成功返回True，插入失败返回False</returns>
        bool AddMatchingInfo(S_NewAndOldProductCodeMatching insert, out string error);

        /// <summary>
        /// 更新匹配
        /// </summary>
        /// <param name="insert">匹配信息</param>
        /// <param name="id">表的ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>更新成功返回True，更新失败返回False</returns>
        bool UpdateMatchingInfo(S_NewAndOldProductCodeMatching insert, int id, out string error);

        /// <summary>
        /// 删除匹配
        /// </summary>
        /// <param name="id">表的ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        bool DeleteMatchingInfo(int id, out string error);

        /// <summary>
        /// 检查是否有相同匹配记录
        /// </summary>
        /// <param name="insert">匹配信息</param>
        /// <returns>存在相同的返回True，不存在返回False</returns>
        bool IsSameProductMatchingInfo(S_NewAndOldProductCodeMatching insert);

        /// <summary>
        /// 获得库房中合格CVT数量
        /// </summary>
        /// <param name="storageID">库房ID</param>
        /// <param name="goodID">物品ID</param>
        /// <param name="isRepairstatus">是否为返修状态 1 是，0 不是</param>
        /// <returns>返回获得的CVT数量信息</returns>
        int GetProductRepairStatusCount(string storageID, int goodID, bool isRepairstatus);

        /// <summary>
        /// 编辑复审状态
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <param name="remark">备注</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>编辑成功返回True，编辑失败返回False</returns>
        bool RetrialBill(string djh, string remark, out string error);

        /// <summary>
        /// 检测箱号的业务是否规范
        /// </summary>
        /// <param name="allType">明细业务类型</param>
        /// <param name="typeName">类型名称</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="productCode">箱体编号</param>
        /// <param name="storageID">库房ID</param>
        /// <param name="error">出错时返回的错误信息</param>
        /// <returns>规范返回True,不规范返回False</returns>
        bool IsProductCodeOperationStandard(string allType, Type typeName, int goodsID, string productCode, string storageID, out string error);

        /// <summary>
        /// 回退单据
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <param name="billStatus">回退后的单据状态</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <param name="rebackReason">回退原因</param>
        /// <param name="billType">单据类型</param>
        /// <returns>回退成功返回True，回退失败返回False</returns>
        bool ReturnBill(string djh, string billStatus, out string error, string rebackReason, string billType);
    }
}
