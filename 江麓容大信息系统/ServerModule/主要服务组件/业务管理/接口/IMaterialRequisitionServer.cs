/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IMaterialRequisitionServer.cs
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
using PlatformManagement;

namespace ServerModule
{
    /// <summary>
    /// 领料单单据状态
    /// </summary>
    public enum MaterialRequisitionBillStatus
    {
        /// <summary>
        /// 新建单据
        /// </summary>
        新建单据,
        /// <summary>
        /// 等待主管审核
        /// </summary>
        等待主管审核,

        /// <summary>
        /// 等待部门领导批准
        /// </summary>
        等待部门领导批准,

        /// <summary>
        /// 等待工艺人员批准
        /// </summary>
        等待工艺人员批准,

        /// <summary>
        /// 等待出库
        /// </summary>
        等待出库,

        /// <summary>
        /// 已出库
        /// </summary>
        已出库
    }

    /// <summary>
    /// 领料单管理类接口
    /// </summary>
    public interface IMaterialRequisitionServer : IBasicService, IBasicBillServer
    {
        /// <summary>
        /// 基辅料信息查询
        /// </summary>
        /// <returns></returns>
        DataTable GetKievMaterialInfo();

        /// <summary>
        /// 删除看板发料记录
        /// </summary>
        /// <param name="issueType">发料类型</param>
        void DeleteBoardIssue(string issueType);

        /// <summary>
        /// 获得看板发料的记录
        /// </summary>
        /// <param name="issueType">发料类型</param>
        List<S_MaterialRequisition_BoardIssue> GetBoardIssueInfo(string issueType);

        /// <summary>
        /// 删除看板发料记录
        /// </summary>
        /// <param name="issue">看板发料对象</param>
        void DeleteBoardIssue(S_MaterialRequisition_BoardIssue issue);

        /// <summary>
        /// 添加看板发料记录
        /// </summary>
        /// <param name="issue">看板发料对象</param>
        void AddBoardIssue(S_MaterialRequisition_BoardIssue issue);

        #region 2017-9-20 夏石友，出库时需检测物料状态
        /// <summary>
        /// 检查物料中是否存在隔离物品
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回隔离的物品信息</returns>
        List<View_S_MaterialRequisitionGoods> IsExistsIsolationGoods(string billNo);
        #endregion

        /// <summary>
        /// 根据单据信息操作账务信息与库存信息
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="bill">单据信息</param>
        void OpertaionDetailAndStock(DepotManagementDataContext dataContext, S_MaterialRequisition bill);

        /// <summary>
        /// 看板领料自动生成
        /// </summary>
        /// <param name="goodsInfo">物料信息</param>
        /// <param name="pickingType">领用类型</param>
        /// <param name="storageID">库房ID</param>
        void AutoCreateBoardPicking(List<S_MaterialRequisitionGoods> goodsInfo, string pickingType, string storageID);

        /// <summary>
        /// 获得单条领料单记录
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回单条领料单信息</returns>
        S_MaterialRequisition GetBill(string billNo);

        /// <summary>
        /// 外部插入领料单
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billInfo">LINQ数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool AutoCreateBill(DepotManagementDataContext ctx, S_MaterialRequisition billInfo, out string error);

        /// <summary>
        /// 工艺人员批准
        /// </summary>
        /// <param name="billid">单据号</param>
        /// <param name="returnBill">操作成功后返回的查询信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool TechnologistBill(string billid, out IQueryResult returnBill, out string error);
               
        /// <summary>
        /// 检查是否存在填写了指定关联单号的单据
        /// </summary>
        /// <param name="associatedBillNo">关联单号</param>
        /// <returns>存在返回true</returns>
        bool IsExistAssociatedBill(string associatedBillNo);

        /// <summary>
        /// 获取包含了指定关联单号的领料单
        /// </summary>
        /// <param name="associatedBillNo">关联单号</param>
        /// <returns>返回获取到的关联单据</returns>
        IQueryable<S_MaterialRequisition> ContainAssociatedBill(string associatedBillNo);

        /// <summary>
        /// 获取所有领料单信息
        /// </summary>
        /// <param name="returnBill">入库单</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取领料信息</returns>
        bool GetAllBill(out IQueryResult returnBill, out string error);

        /// <summary>
        /// 获取领料单视图信息
        /// </summary>
        /// <param name="billNo">领料单号</param>
        /// <returns>成功返回获取领料信息, 失败返回null</returns>
        View_S_MaterialRequisition GetBillView(string billNo);

        /// <summary>
        /// 添加领料单
        /// </summary>
        /// <param name="bill">单据信息</param>
        /// <param name="returnBill">返回更新后的领料单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加领料单</returns>
        bool AddBill(S_MaterialRequisition bill, out IQueryResult returnBill, out string error);
 
        /// <summary>
        /// 删除领料单
        /// </summary>
        /// <param name="billNo">领料单号</param>
        /// <param name="returnBill">返回更新后重新查询的领料单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功删除领料单号</returns>
        bool DeleteBill(string billNo, out IQueryResult returnBill, out string error);

        /// <summary>
        /// 删除领料单
        /// </summary>
        /// <param name="billNo">领料单号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功删除领料单号</returns>
        bool DeleteBill(string billNo, out string error);
 
        /// <summary>
        /// 修改领料单(只修改编制人涉及信息)
        /// </summary>
        /// <param name="bill">单据信息</param>
        /// <param name="returnBill">返回更新后的领料单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加领料单</returns>
        bool UpdateBill(S_MaterialRequisition bill, out IQueryResult returnBill, out string error);

        /// <summary>
        /// 修改领料单
        /// </summary>
        /// <param name="billNo">单据编号</param>
        /// <param name="purposeCode">用途编号</param>
        /// <param name="remark">备注</param>
        /// <param name="returnBill">返回更新后的领料单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加领料单</returns>
        bool UpdateBill(string billNo, string purposeCode, string remark, out IQueryResult returnBill, out string error);

        /// <summary>
        /// 领料人提交单据(交给主管审批)
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="returnBill">返回更新后重新查询的领料单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加领料单</returns>
        bool SubmitNewBill(string billNo, out IQueryResult returnBill, out string error);
        
        /// <summary>
        /// 主管审批单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="name">主管姓名</param>
        /// <param name="returnBill">返回更新后重新查询的领料单数据集</param>
        /// <param name="msg">返回更新后的单据状态</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加领料单</returns>
        bool AuthorizeBill(string billNo, string name, out MaterialRequisitionBillStatus msg, out IQueryResult returnBill, out string error);

        /// <summary>
        /// 完成领料单
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="storeManager">仓库管理员</param>
        /// <param name="returnBill">返回更新后重新查询的领料单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加领料单</returns>
        bool FinishBill(string billNo, string storeManager, out IQueryResult returnBill, out string error);

        /// <summary>
        /// 完成领料单(系统自动生成领料单时调用)
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">单据号</param>
        /// <param name="storeManager">仓库管理员</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加领料单</returns>
        bool FinishBill(DepotManagementDataContext ctx, string billNo, string storeManager, out string error);

        /// <summary>
        /// 回退单据
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <param name="billStatus">单据状态</param>
        /// <param name="returnBill">返回回退后查询到的数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <param name="rebackReason">回退原因</param>
        /// <returns>回退成功返回True，回退失败返回False</returns>
        bool ReturnBill(string djh, string billStatus,
            out IQueryResult returnBill, out string error, string rebackReason);

        /// <summary>
        /// 查询未领用的报废物品
        /// </summary>
        /// <param name="loginName">登录人姓名</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回查询到的数据集</returns>
        DataTable GetScrapGoods(string loginName, out string error);

        /// <summary>
        /// 检查并导出版次号不符的物品
        /// </summary>
        /// <param name="billID">领料单号</param>
        /// <returns>返回检测出的结果集</returns>
        DataTable CheckGoodsVersion(string billID);

        /// <summary>
        /// 获得部门真实的每月领料上限
        /// </summary>
        /// <returns>返回部门真实的每月领料上限</returns>
        DataTable GetDeptPickingToplimitTable();

        /// <summary>
        /// 保存领料上限表
        /// </summary>
        /// <param name="insertTable">被保存的数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>保存成功返回True，保存失败返回False</returns>
        bool SaveDeptPickingToplimit(DataTable insertTable, out string error);

        /// <summary>
        /// 分管领导批准
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="returnBill">返回更新后所查询的数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool AuthorizBill(string billID, out IQueryResult returnBill, out string error);

        /// <summary>
        /// 获得部门每月领料上限的按台为单位的基数
        /// </summary>
        /// <returns>返回部门的领料上限数据集</returns>
        DataTable GetManufacturingConsumeTable();

        /// <summary>
        /// 自动生成消耗定额
        /// </summary>
        /// <param name="dccount">每月的台套数</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>生成成功返回True，生成失败返回False</returns>
        bool AutogenerationPickingToplimit(decimal dccount,out string error);

        /// <summary>
        /// 针对于领料单的工装业务处理
        /// </summary>
        /// <param name="ctx">LINQ数据上下文</param>
        /// <param name="billNo">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool FinishFrock(DepotManagementDataContext ctx, string billNo,out string error);

        /// <summary>
        /// 在系统日志中插入一条信息
        /// </summary>
        /// <param name="log">日志数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>插入成功返回True失败返回False</returns>
        bool InsertSysLog(_Sys_Log log, out string error);

        /// <summary>
        /// 修改用途
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="type">用途</param>
        /// <param name="returnBill">结果集</param>
        /// <param name="error">错误信息</param>
        /// <returns>插入成功返回True失败返回False</returns>
        bool UpdateBill(string billNo, string type, out IQueryResult returnBill, out string error);

        /// <summary>
        /// 获得整台请领单明细列表
        /// </summary>
        /// <param name="billNo">整台请领单号</param>
        /// <returns>返回列表</returns>
        List<View_Business_WarehouseOutPut_WholeMachineRequisitionDetail> GetWholeMachineRequistionDetail(string billNo);
        
        /// <summary>
        /// 获得领料汇总表
        /// </summary>
        /// <param name="yearAndMonth">年月</param>
        /// <param name="sheetName">表单名</param>
        /// <returns>返回Table</returns>
        DataTable GetSummarySheet(string yearAndMonth, string sheetName);
   }
}
