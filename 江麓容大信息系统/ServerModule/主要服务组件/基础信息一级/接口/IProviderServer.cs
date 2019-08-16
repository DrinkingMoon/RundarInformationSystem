/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IProviderServer.cs
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
    /// 供应商管理类接口
    /// </summary>
    public interface IProviderServer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        void InsertUnitPriceList(string provider);

        /// <summary>
        /// 获得供应商对象
        /// </summary>
        /// <param name="provider">供应商简码</param>
        /// <returns></returns>
        Provider GetProviderInfo(string provider);

        /// <summary>
        /// 获得供应商全称
        /// </summary>
        /// <param name="provider">供应商编码</param>
        /// <returns>供应商全称</returns>
        string GetPrivderName(string provider);

        /// <summary>
        /// 获得供应商名称
        /// </summary>
        /// <param name="providercode">供应商编码</param>
        /// <returns>返回供应商名称</returns>
        string GetProviderName(string providercode);

        /// <summary>
        /// 获取供应商信息表
        /// </summary>
        /// <param name="table">供应商信息表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取供应商信息表</returns>
        bool GetAllProvider(out DataTable table, out string error);

        /// <summary>
        /// 获取供应商信息表
        /// </summary>
        /// <param name="returnBill">供应商信息表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取供应商信息表</returns>
        bool GetAllProvider(out IQueryable<View_Provider> returnBill, out string error);

        /// <summary>
        /// 获取唯一性供应商信息表
        /// </summary>
        /// <param name="returnBill">供应商信息表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取供应商信息表</returns>
        bool GetDistinctProvider(out IQueryable<View_Provider> returnBill, out string error);

        /// <summary>
        /// 添加/修改部门信息表
        /// </summary>
        /// <param name="providerCode">供应商编码</param>
        /// <param name="providerName">供应商名称</param>
        /// <param name="shortName">简称</param>
        /// <param name="personnel">责任人</param>
        /// <param name="isOdd">是否关联责任人</param>
        /// <param name="isUse">是否在用</param>
        /// <param name="isMainDuty">是否为主要责任人</param>
        /// <param name="returnBill">部门信息表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <param name="isInternalSupplier">是否为内部供应商</param>
        /// <param name="clearingForm">结算方式</param>
        /// <param name="yearMonth">追溯年月</param>
        /// <returns>返回是否成功添加/修改部门信息表</returns>
        bool AddProvider(string providerCode, string providerName, string shortName,
            string personnel, out IQueryable<View_Provider> returnBill, out string error,
            bool isOdd, bool isUse, bool isMainDuty, bool isInternalSupplier, string clearingForm, string yearMonth);

         /// <summary>
        /// 添加/修改供应商信息表
        /// </summary>
        /// <param name="providerCode">供应商编码</param>
        /// <param name="providerName">供应商名称</param>
        /// <param name="shortName">简称</param>
        /// <param name="isOdd">是否修改</param>
        /// <param name="isUse">是否在用</param>
        /// <param name="isMainDuty">是否为主要责任人</param>
        /// <param name="personnel">新责任人</param>
        /// <param name="oldPersonnel">老责任人</param>
        /// <param name="clearingForm">结算方式</param>
        /// <param name="yearMonth">追溯年月</param>
        /// <param name="returnBill">变更后返回的数据集</param>
        /// <param name="error">错误信息</param>
        /// <param name="isInternalSupplier">是否为内部供应商</param>
        /// <returns>添加/修改成功返回True，失败返回False</returns>
        bool UpdataProvider(string providerCode, string providerName, string shortName,
            bool isOdd, bool isUse, bool isMainDuty, bool isInternalSupplier, string personnel, string oldPersonnel,
            string clearingForm, string yearMonth,
            out IQueryable<View_Provider> returnBill, out string error);

        /// <summary>
        /// 删除某一供应商
        /// </summary>
        /// <param name="providerCode">供应商编码</param>
        /// <param name="personnelCode">工号</param>
        /// <param name="returnBill">操作成功后返回的查询信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功删除某一供应商</returns>
        bool DeleteProvider(string providerCode,string personnelCode, out IQueryable<View_Provider> returnBill, out string error);

        /// <summary>
        /// 获取供应商信息表
        /// </summary>
        /// <param name="returnBill">供应商信息表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取供应商信息表</returns>
        bool GetAllNewProvider(out IQueryable<View_B_NewProvider> returnBill, out string error);

        /// <summary>
        /// 添加/修改部门信息表
        /// </summary>
        /// <param name="providerCode">供应商编码</param>
        /// <param name="providerName">供应商名称</param>
        /// <param name="remark">备注</param>
        /// <param name="returnBill">部门信息表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加/修改部门信息表</returns>
        bool AddNewProvider(string providerCode, string providerName, string remark, out IQueryable<View_B_NewProvider> returnBill, out string error);

        /// <summary>
        /// 添加/修改供应商信息表
        /// </summary>
        /// <param name="providerCode">供应商编码</param>
        /// <param name="providerName">供应商名称</param>
        /// <param name="remark">备注</param>
        /// <param name="returnBill">操作成功后返回的查询信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加/修改供应商信息表</returns>
        bool UpdataNewProvider(string providerCode, string providerName, string remark, out IQueryable<View_B_NewProvider> returnBill, out string error);

        /// <summary>
        /// 删除某一供应商
        /// </summary>
        /// <param name="providerCode">供应商编码</param>
        /// <param name="returnBill">操作成功后返回的查询信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功删除某一供应商</returns>
        bool DeleteNewProvider(string providerCode, out IQueryable<View_B_NewProvider> returnBill, out string error);

        /// <summary>
        /// 检测供应商是否在库存表已存在
        /// </summary>
        /// <param name="providercode">供应商编码</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>不存在返回True，存在返回False</returns>
        bool CheckStockPrivuderIsIn(string providercode, out string error);

        /// <summary>
        /// 停用此供应商
        /// </summary>
        /// <param name="providerCode">供应商编码</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool UpdateProviderIsUse(string providerCode, out string error);

        /// <summary>
        /// 获得责任人信息
        /// </summary>
        /// <param name="providerCode">责任人工号</param>
        /// <returns>返回责任人信息</returns>
        DataTable GetProviderPrincipal(string providerCode);

        /// <summary>
        /// 获得责任人信息
        /// </summary>
        /// <param name="providerCode">责任人工号</param>
        /// <returns>返回责任人信息</returns>
        List<ProviderPrincipal> GetProviderPrincipalList(string providerCode);
    }
}
