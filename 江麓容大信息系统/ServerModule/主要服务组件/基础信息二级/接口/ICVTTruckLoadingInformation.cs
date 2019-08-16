/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  ICVTTruckLoadingInformation.cs
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
    /// CVT装车信息服务类接口
    /// </summary>
    public interface ICVTTruckLoadingInformation
    {
        /// <summary>
        /// 获得装车信息
        /// </summary>
        /// <returns>返货装车信息</returns>
        DataTable GetLoadingInfo();

        /// <summary>
        /// 删除装车信息
        /// </summary>
        /// <param name="loadingInfo">装车信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        bool DeleteLoadingInfo(YX_LoadingInfo loadingInfo, out string error);

        /// <summary>
        /// 更新装车信息
        /// </summary>
        /// <param name="loadingInfo">装车信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>更新成功返回True，更新失败返回False</returns>
        bool UpdateLoadingInfo(YX_LoadingInfo loadingInfo, out string error);

        /// <summary>
        /// 插入装车信息
        /// </summary>
        /// <param name="loadingInfo">装车信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>插入成功返回True，插入失败返回False</returns>
        bool InsertIntoLoadingInfo(YX_LoadingInfo loadingInfo, out string error);

        /// <summary>
        /// 批量插入装车信息
        /// </summary>
        /// <param name="loadingInfo">装车信息列表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>批量插入成功返回True，批量插入失败返回False</returns>
        bool BatchInsertLoadingInfo(DataTable loadingInfo, out string error);
    }
}
