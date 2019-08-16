/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IProductDeliveryInspectionServer.cs
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
using GlobalObject;
using ServerModule;
using System.Data;

namespace ServerModule
{
    /// <summary>
    /// CVT出厂检验记录管理类接口
    /// </summary>
    public interface IProductDeliveryInspectionServer :IBasicBillServer
    {
        /// <summary>
        /// CVT终检信息记录查询
        /// </summary>
        /// <param name="startTime">开始日期</param>
        /// <param name="endTime">结束日期</param>
        /// <returns>返回Table</returns>
        DataTable SelectFinalInspectionList(DateTime? startTime, DateTime? endTime);

        /// <summary>
        /// 删除CVT终检记录
        /// </summary>
        /// <param name="startTime">开始日期</param>
        /// <param name="endTime">结束日期</param>
        /// <param name="lnqFinalInfo">CVT终检信息</param>
        void DeleteCVTFinalInspectionInfo(DateTime startTime, DateTime endTime, ZL_CVTFinalInspection lnqFinalInfo);

        /// <summary>
        /// 添加CVT终检记录
        /// </summary>
        /// <param name="lnqFinalInfo">CVT终检信息</param>
        void AddCVTFinalInspectionInfo(ZL_CVTFinalInspection lnqFinalInfo);

        /// <summary>
        /// 获取检查明细项目信息
        /// </summary>
        /// <param name="technicalID">明细项目ID</param>
        /// <returns>返回LNQ</returns>
        P_TechnicalRequirements GetTechnicalRequirementsInfo(int technicalID);

        /// <summary>
        /// 获取检查项目信息
        /// </summary>
        /// <param name="testID">项目ID</param>
        /// <returns>返回LNQ</returns>
        P_AllAccreditedTestingItems GetAllAccreditedTestingItemsInfo(int testID);

        /// <summary>
        /// 检测此单据的单据状态是否为“已检验”
        /// </summary>
        /// <param name="billID">营销单据号</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>已检验返回True，未检验返回False</returns>
        bool IsExamine(string billID, out string storageID);

        /// <summary>
        /// 删除单据信息
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        bool DeleteBill(string billID, out string error);

        /// <summary>
        /// 获得某一条BILL信息
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <returns>返回数据集</returns>
        P_DeliveryInspection GetBill(string billID);

        /// <summary>
        /// 获得明细信息
        /// </summary>
        /// <param name="billID">检验记录表单号</param>
        /// <returns>返回CVT出厂检验记录表的明细信息</returns>
        DataTable GetListInfo(string billID);

        ///// <summary>
        ///// 显示CVT出厂检验空表格
        ///// </summary>
        ///// <returns>返回CVT出厂检验空表格</returns>
        //DataTable GetEmptyTable();

        /// <summary>
        /// 显示空表格(2013-12-04之后的数据，邱瑶改)
        /// </summary>
        /// <param name="productType">产品型号</param>
        /// <returns>返回CVT出厂检验空表格</returns>
        DataTable GetEmptyTable(string productType);

        /// <summary>
        /// 删除所关联的所有CVT出厂检验记录
        /// </summary>
        /// <param name="djh">关联单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        bool DeleteDeliveryInspection(string djh, out string error);

        /// <summary>
        /// 处理自动生成CVT出厂检验记录表
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="djh">营销入库单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>自动生成成功返回True，自动生成失败返回False</returns>
        bool ManageDeliveryInspection(DepotManagementDataContext ctx, string djh, out string error);

        /// <summary>
        /// 更新出厂检验记录
        /// </summary>
        /// <param name="delivery">出厂检验记录表信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>更新成功返回True，更新失败返回False</returns>
        bool UpdateDeliveryInspection(P_DeliveryInspection delivery, out string error);

        /// <summary>
        /// 获得明细项目ID
        /// </summary>
        /// <param name="name">明细项目名称</param>
        /// <param name="productType">产品类型</param>
        /// <returns>获取成功返回明细项目ID，获取失败返回0</returns>
        int GetTechnicalRequirementsID(string name, string productType);

        /// <summary>
        /// 获得项目ID
        /// </summary>
        /// <param name="name">项目名称</param>
        /// <param name="productType">产品类型</param>
        /// <returns>获取成功返回项目ID，获取失败返回0</returns>
        int GetTestItemNameID(string name, string productType);

        /// <summary>
        /// 获得明细项目集合
        /// </summary>
        /// <param name="name">项目名称</param>
        /// <param name="productType">产品型号</param>
        /// <returns>返回获取的明细项目集合</returns>
        DataTable GetTechnicalRequirements(string name,string productType);

        /// <summary>
        /// 最终判定
        /// </summary>
        /// <param name="delivery">CVT检验报告信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True ，操作失败返回False</returns>
        bool FinalJudgeBill(P_DeliveryInspection delivery, out string error);


        /// <summary>
        /// 获得明细项目集合
        /// </summary>
        /// <param name="technicalName">明细项目名称</param>
        /// <param name="productType">产品类型</param>
        /// <returns>返回明细项目与主项目关联后的数据集合</returns>
        DataTable GetAllTechnical(string technicalName, string productType);

        /// <summary>
        /// 获得CVT出厂检验记录
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="djzt">单据状态</param>
        /// <returns>返回CVT出厂检验记录集</returns>
        DataTable GetDeliveryInspection(DateTime startTime, DateTime endTime, string djzt);
    }
}
