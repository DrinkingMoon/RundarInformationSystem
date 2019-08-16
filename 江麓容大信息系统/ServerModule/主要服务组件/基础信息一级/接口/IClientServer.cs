/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IClientServer.cs
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
    /// 客户管理类接口
    /// </summary>
    public interface IClientServer
    {
        /// <summary>
        /// 获得客户表
        /// </summary>
        /// <returns></returns>
        DataTable GetClient();

        /// <summary>
        /// 获取客户信息
        /// </summary>
        /// <param name="returnClient">客户信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取客户信息</returns>
        bool GetAllClient(out IQueryable<View_Client> returnClient, out string error);

        /// <summary>
        /// 添加客户
        /// </summary>
        /// <param name="clientInfo">客户信息</param>
        /// <param name="returnClient">客户信息结果集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加客户信息</returns>
        bool AddClient(Client clientInfo, out IQueryable<View_Client> returnClient, out string error);

        /// <summary>
        /// 更新客户
        /// </summary>
        /// <param name="clientInfo">客户信息</param>
        /// <param name="oldClient">旧客户信息</param>
        /// <param name="returnClient">客户信息结果集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功更新客户信息</returns>
        bool UpdateClient(Client clientInfo, Client oldClient, out IQueryable<View_Client> returnClient, out string error);

        /// <summary>
        /// 删除客户
        /// </summary>
        /// <param name="clientCode">客户编码</param>
        /// <param name="returnClient">客户信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功删除客户信息</returns>
        bool DeleteClient(string clientCode, out IQueryable<View_Client> returnClient, out string error);

        /// <summary>
        /// 获得客户名
        /// </summary>
        /// <param name="clientCode">客户编码</param>
        /// <returns></returns>
        string GetClientName(string clientCode);

        /// <summary>
        /// 获得客户编码
        /// </summary>
        /// <param name="clientName">客户名</param>
        /// <returns>成功则返回客户编码，失败返回空串</returns>
        string GetClientCode(string clientName);

    }
}
