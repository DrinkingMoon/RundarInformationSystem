/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IBargainInfoServer.cs
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
    /// 合同信息管理类接口
    /// </summary>
    public interface IBargainInfoServer : IBasicService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bargainNumber"></param>
        /// <returns></returns>
        decimal GetTaxRate(string bargainNumber);

        /// <summary>
        /// 禁用合同
        /// </summary>
        /// <param name="bargainNumber"></param>
        void DisableInfo(string bargainNumber);

        /// <summary>
        /// 通过订单号,物品ID获得合同单价
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="orderFormNumber">订单号</param>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回合同单价</returns>
        decimal GetBargainUnitPrice(DepotManagementDataContext ctx, string orderFormNumber, int goodsID);

        /// <summary>
        /// 通过订单号,物品ID获得合同单价
        /// </summary>
        /// <param name="orderFormNumber">订单号</param>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回合同单价</returns>
        decimal GetBargainUnitPrice(string orderFormNumber, int goodsID);

        /// <summary>
        /// 获得合同号
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="provider">供应商编码</param>
        /// <returns>返回获得的数据集</returns>
        DataRow GetLatelyBargainNumberInfo(int goodsID, string provider);

        /// <summary>
        /// 获得合同信息
        /// </summary>
        /// <param name="bargainNumber">合同号</param>
        /// <returns>返货指定合同号的合同信息</returns>
        DataRow GetBargainInfoDataRow(string bargainNumber);

        /// <summary>
        /// 获取指定合同的采购员编码
        /// </summary>
        /// <param name="bargainNumber">合同号</param>
        /// <returns>返回获取到的采购员编码</returns>
        string GetBuyerCode(string bargainNumber);

        /// <summary>
        /// 获取指定的订单信息
        /// </summary>
        /// <param name="bargainNumber">合同号</param>
        /// <returns>成功返回获取到的订单信息, 失败返回null</returns>
        View_B_BargainInfo GetBargainInfo(string bargainNumber);

        /// <summary>
        /// 获取所有允许查询的合同信息
        /// </summary>
        /// <param name="returnResult">返回的结果</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功的标志</returns>
        bool  GetAllBargainInfo(out IQueryResult returnResult, out string error);

        /// <summary>
        /// 添加合同信息
        /// </summary>
        /// <param name="bargainInfo">合同信息</param>
        /// <param name="returnBargainInfo">返回查询到的合同信息</param>
        /// <param name="error">错误信息, 没有则为null</param>
        /// <returns>操作是否成功的标志</returns>
        bool AddBargainInfo(B_BargainInfo bargainInfo, out IQueryResult returnBargainInfo, out string error);

        /// <summary>
        /// 修改合同信息
        /// </summary>
        /// <param name="oldBargainNumber">旧合同号</param>
        /// <param name="bargainInfo">合同信息</param>
        /// <param name="returnBargainInfo">返回查询到的合同信息</param>
        /// <param name="error">错误信息, 没有则为null</param>
        /// <returns>操作是否成功的标志</returns>
        bool UpdateBargainInfo(string oldBargainNumber, B_BargainInfo bargainInfo, out IQueryResult returnBargainInfo, out string error);

        /// <summary>
        /// 删除合同信息
        /// </summary>
        /// <param name="bargainNumber">合同编号</param>
        /// <param name="returnBargainInfo">返回查询到的合同信息</param>
        /// <param name="error">错误信息, 没有则为null</param>
        /// <returns>操作是否成功的标志</returns>
        bool DeleteBargainInfo(string bargainNumber, out IQueryResult returnBargainInfo, out string error);

        /// <summary>
        /// 审核合同
        /// </summary>
        /// <param name="auditingPersonnel">审核人</param>
        /// <param name="bargainNumber">合同号</param>
        /// <param name="error">出错时返回错误信息，否则为null</param>
        /// <returns>操作是否成功的标志</returns>
        bool AuditingBargainInfo(string auditingPersonnel, string bargainNumber, out string error);

        /// <summary>
        /// 检查是否已审核
        /// </summary>
        /// <param name="bargainNumber">合同号</param>
        /// <returns>已经审核返回true</returns>
        bool IsAudited(string bargainNumber);

        #region 夏石友，2012-07-18，将报检入库单中的此功能移动到此，原方法名：CheckBargainIsConsignOut

        /// <summary>
        /// 检查订单对应的合同是否为委外合同
        /// </summary>
        /// <param name="orderFormNumber">订单号</param>
        /// <returns>是返回true, 否返回false</returns>
        bool IsConsignOutBargain(string orderFormNumber);
                
        /// <summary>
        /// 检查订单对应的合同是否为海外合同
        /// </summary>
        /// <param name="orderFormNumber">订单号</param>
        /// <returns>是返回true, 否返回false</returns>
        bool IsOverseasBargain(string orderFormNumber);

        #endregion

        /// <summary>
        /// 获得税率
        /// </summary>
        /// <param name="orderFormNumber">订单号</param>
        /// <returns>返回税率</returns>
        decimal GetBargainCess(string orderFormNumber);

    }
}
