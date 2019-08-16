/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  ICVTCustomerInformationServer.cs
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
    /// CVT客户信息服务类接口
    /// </summary>
    public interface ICVTCustomerInformationServer
    {
        /// <summary>
        /// 批量自动匹配CVT编号
        /// </summary>
        void BatchMatchingCVTNumber();

        /// <summary>
        /// 删除数据CVT客户基础信息
        /// </summary>
        /// <param name="cvtCustomer">CVT客户信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        bool DeleteCVTCustomerInformation(YX_CVTCustomerInformation cvtCustomer, out string error);

        /// <summary>
        /// 获得CVT客户基础信息
        /// </summary>
        /// <returns>返回CVT客户基础信息</returns>
        DataTable GetCVTCustomerInformation();

        /// <summary>
        /// 插入数据CVT客户基础信息
        /// </summary>
        /// <param name="cvtCustomer">要插入的CVT客户信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>插入成功返回True，插入失败返回False</returns>
        bool InsertCVTCustomerInformation(YX_CVTCustomerInformation cvtCustomer, out string error);

        /// <summary>
        /// 更改数据CVT客户基础信息
        /// </summary>
        /// <param name="cvtCustomer">CVT客户信息</param>
        /// <param name="error">c错误信息</param>
        /// <returns>更改成功返回True，更改失败返回False</returns>
        bool UpdateCVTCustomerInformation(YX_CVTCustomerInformation cvtCustomer, out string error);

        /// <summary>
        /// 批量插入CVT客户基础信息
        /// </summary>
        /// <param name="cvtCustomerInfomation">CVT客户信息列表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>插入成功返回True，插入失败返回False</returns>
        bool BatchInsertCVTCustomerInformation(DataTable cvtCustomerInfomation,
            out string error);

        /// <summary>
        /// 根据经销商及客户名称获得其历史信息
        /// </summary>
        /// <param name="vehicleShelfNumber">车架号</param>
        /// <returns>返回获得的信息</returns>
        DataTable GetCVTCustomerHistoryInfo(string vehicleShelfNumber);

        /// <summary>
        /// 插入客户信息历史记录，若更换的是CVT，则修改客户信息中对应的车架号的CVT编号
        /// </summary>
        /// <param name="serviceID">反馈单号</param>
        /// <param name="vehicleShelfNumber">车架号</param>
        /// <param name="cvtType">变速箱型号</param>
        /// <param name="carModel">车型</param>
        /// <param name="clientName">客户名称</param>
        /// <param name="dealerName">经销商名称</param>
        /// <param name="replaceAccessoryList">更换件列表</param>
        /// <param name="error">错误信息</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool InsertCustomerHistoryInfo(string serviceID, string vehicleShelfNumber, string cvtType, string carModel, string clientName, string dealerName,
            DataTable replaceAccessoryList, out string error);
    }
}
