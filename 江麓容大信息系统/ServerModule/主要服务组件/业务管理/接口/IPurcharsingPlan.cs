/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IPurcharsingPlan.cs
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
using GlobalObject;

namespace ServerModule
{
    /// <summary>
    /// 采购计划管理类接口
    /// </summary>
    public interface IPurcharsingPlan
    {
        /// <summary>
        /// 记录缺件
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">单据号</param>
        void RecordMissingParts(DepotManagementDataContext ctx, string billNo);

        /// <summary>
        /// 删除公式
        /// </summary>
        /// <param name="mathID">公式ID</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返货False</returns>
        bool DeleteProcurementMath(int mathID, out string error);
        
        /// <summary>
        /// 保存计算公式
        /// </summary>
        /// <param name="procurementMath">新公式数据</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool SaveProcurementMath(CG_ProcurementMath procurementMath, out string error);
        
        /// <summary>
        /// 获得计算公式数据
        /// </summary>
        /// <returns>返回Table</returns>
        DataTable GetProcurementMath();

        /// <summary>
        /// 操作采购计划的计算步骤
        /// </summary>
        /// <param name="procurementSteps">新数据</param>
        /// <param name="mode">操作模式</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool OperatorMathSteps(CG_ProcurementSteps procurementSteps, CE_OperatorMode mode, out string error);

        /// <summary>
        /// 获得计算步骤
        /// </summary>
        /// <returns>返回计算步骤TABLE</returns>
        DataTable GetMathSteps();

        /// <summary>
        /// 操作采购计划计算公式
        /// </summary>
        /// <param name="mode">操作方式</param>
        /// <param name="model">LINQ实体集</param>
        /// <param name="mathModel">LINQ实体集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool OperatorProcuremnetPlanFormla(GlobalObject.CE_OperatorMode mode, CG_ProcurementPlan model, CG_ProcurementMath mathModel, out string error);
        
        /// <summary>
        /// 获得采购计划计算公式
        /// </summary>
        /// <returns>返回Table</returns>
        DataTable GetProcurementPlanView(string sql);

        /// <summary>
        /// 获得采购计划计算公式
        /// </summary>
        /// <returns></returns>
        DataTable GetProcurementPlanView();

        /// <summary>
        /// 是否允许创建采购计划
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="nY">年月</param>
        /// <returns>允许返回True ,不允许返回False</returns>
        bool IsAllowCreate(DateTime startTime, DateTime endTime, string nY);

        /// <summary>
        /// 添加所有信息
        /// </summary>
        /// <param name="messageList">需要添加的信息</param>
        /// <param name="yearAndMonth">年月字符串</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>添加成功返回True，添加失败返回False</returns>
        bool AddAllBill(DataTable messageList, string yearAndMonth, out string error);

        /// <summary>
        /// 添加主表数据
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="yearAndMonth">年月</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>添加成功返回True，添加失败返回False</returns>
        bool AddBill(DepotManagementDataContext ctx, string yearAndMonth, out string error);

        /// <summary>
        /// 添加子表数据
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="messageList">需要添加的数据集</param>
        /// <param name="yearAndMonth">年月字符串</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>添加成功返回True，添加失败返回False</returns>
        bool AddList(DepotManagementDataContext ctx, DataTable messageList,
            string yearAndMonth, out string error);

        /// <summary>
        /// 删除子表
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="yearAndMonth">年月字符串</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        bool DeleteList(DepotManagementDataContext ctx, string yearAndMonth, out string error);

        /// <summary>
        /// 获得主表数据集
        /// </summary>
        /// <param name="yearAndMonth">年月字符串</param>
        /// <returns>返回主表信息</returns>
        DataRow GetBill(string yearAndMonth);

        /// <summary>
        /// 返回明细数据
        /// </summary>
        /// <param name="yearAndMonth">年月字符串</param>
        /// <returns>返回明细信息</returns>
        DataTable GetList(string yearAndMonth);

        /// <summary>
        /// 更新子父表信息
        /// </summary>
        /// <param name="messageList">需要更新的数据信息</param>
        /// <param name="yearAndMonth">年月字符串</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>更新成功返回True，更新失败返回False</returns>
        bool UpdateBill(DataTable messageList, string yearAndMonth, out string error);

        /// <summary>
        /// 更新主表单据状态
        /// </summary>
        /// <param name="yearAndMonth">年月字符串</param>
        /// <param name="djzt">单据状态</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>更新成功返回True，更新失败返回False</returns>
        bool UpdateBill(string yearAndMonth, string djzt, out string error);

        /// <summary>
        /// 获得最新的记录集
        /// </summary>
        /// <param name="yearAndMonth">年月字符串</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回最新的查询记录集</returns>
        DataTable GetNewList(string yearAndMonth ,out string error);

        /// <summary>
        /// 检查单据是否已完成
        /// </summary>
        /// <param name="yearAndMonth">年月字符串</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>单据未完成返回True，单据已完成返回False</returns>
        bool IsFinish(string yearAndMonth, out string error);

        /// <summary>
        /// 计算订货数
        /// </summary>
        /// <param name="messageTable">需要获取订货数的数据集</param>
        /// <returns>返回已获取到订货数的数据集</returns>
        DataTable GetOrderGoodsCount(DataTable messageTable);

        /// <summary>
        /// 获取订货金额
        /// </summary>
        /// <param name="messageTable">需要获取订货金额的数据集</param>
        /// <returns>返回已获取到订货金额的数据集</returns>
        DataTable GetOrderGoodsPrice(DataTable messageTable);

        /// <summary>
        /// 检查是否具备生成采购计划的条件
        /// </summary>
        /// <param name="yearAndMonth">年月</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>都具备条件返回TRUE，返回FALSE不具备条件</returns>
        bool IsQualified(string yearAndMonth, out string error);

        /// <summary>
        /// 获得生产计划单据列表与营销要货计划单据列表
        /// </summary>
        /// <param name="yearAndMonth">年月字符串</param>
        /// <param name="marketingPlan">返回的营销计划数据集</param>
        /// <param name="productPlan">返回的生产计划数据集</param>
        void GetMarketingPlanAndProductPlan(string yearAndMonth,
            out DataTable marketingPlan, out DataTable productPlan);
    }
}
