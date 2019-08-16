/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IUnitServer.cs
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
    /// 单位管理类接口
    /// </summary>
    public interface IUnitServer : IBasicService
    {
        /// <summary>
        /// 获得单位名称
        /// </summary>
        /// <param name="unitID">单位ID</param>
        /// <returns>单位名称</returns>
        string GetUnitName(int unitID);

        /// <summary>
        /// 获取单位信息
        /// </summary>
        /// <param name="returnUnit">单位信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取单位信息</returns>
        bool GetAllUnit(out IQueryable<View_S_Unit> returnUnit, out string error);

        /// <summary>
        /// 获取单位信息
        /// </summary>
        /// <param name="id">单位ID</param>
        /// <returns>成功返回获取到的单位信息, 失败返回null</returns>
        View_S_Unit GetUnitInfo(int id);

        /// <summary>
        /// 获取单位信息
        /// </summary>
        /// <param name="unit">单位ID</param>
        /// <returns>成功返回获取到的单位信息, 失败返回null</returns>
        View_S_Unit GetUnitInfo(string unit);

        /// <summary>
        /// 添加单位
        /// </summary>
        /// <param name="unit">单位</param>
        /// <param name="spec">单位规格</param>
        /// <param name="isDisable">停用</param>
        /// <param name="returnUnit">单位信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加单位信息</returns>
        bool AddUnit(string unit, string spec, bool isDisable, out IQueryable<View_S_Unit> returnUnit, out string error);

        /// <summary>
        /// 更新单位
        /// </summary>
        /// <param name="id">序号</param>
        /// <param name="unit">单位</param>
        /// <param name="spec">规格</param>
        /// <param name="isDisable">停用</param>
        /// <param name="returnUnit">单位信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功更新单位信息</returns>
        bool UpdateUnit(int id, string unit, string spec, bool isDisable, out IQueryable<View_S_Unit> returnUnit, out string error);

        /// <summary>
        /// 删除单位
        /// </summary>
        /// <param name="id">序号</param>
        /// <param name="returnUnit">单位信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功删除单位信息</returns>
        bool DeleteUnit(int id, out IQueryable<View_S_Unit> returnUnit, out string error);
    }
}
