/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IWorkbenchServer.cs
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
    /// 工位管理类接口
    /// </summary>
    public interface IWorkbenchService : IBasicService
    {
        /// <summary>
        /// 工位信息属性
        /// </summary>
        IQueryable<View_P_Workbench> Workbenchs
        {
            get;
        }

        /// <summary>
        /// 添加工位
        /// </summary>
        /// <param name="workbench">要添加的工位号</param>
        /// <param name="remark">工位备注</param>
        /// <param name="returnWorkbench">输出操作后重新查询的所有工位信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加工位信息</returns>
        bool Add(string workbench, string remark, out IQueryable<View_P_Workbench> returnWorkbench, out string error);

        /// <summary>
        /// 更新工位
        /// </summary>
        /// <param name="oldWorkbench">旧工位号</param>
        /// <param name="newWorkbench">新工位号</param>
        /// <param name="remark">工位备注</param>
        /// <param name="returnWorkbench">输出操作后重新查询的所有工位信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功删除工位信息</returns>
        bool Update(string oldWorkbench, string newWorkbench, string remark, out IQueryable<View_P_Workbench> returnWorkbench, out string error);

        /// <summary>
        /// 删除工位
        /// </summary>
        /// <param name="workbench">要删除的工位号</param>
        /// <param name="returnWorkbench">输出操作后重新查询的所有工位信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功删除单位信息</returns>
        bool Delete(string workbench, out IQueryable<View_P_Workbench> returnWorkbench, out string error);
    }
}
