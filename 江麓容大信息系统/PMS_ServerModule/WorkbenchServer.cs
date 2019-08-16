/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  WorkbenchServer.cs
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
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace ServerModule
{
    /// <summary>
    /// 工位管理类
    /// </summary>
    class WorkbenchServer : BasicServer, IWorkbenchService
    {
        /// <summary>
        /// 工位信息属性
        /// </summary>
        public IQueryable<View_P_Workbench> Workbenchs
        {
            get
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                return from r in dataContxt.View_P_Workbench
                       select r;
            }
        }

        /// <summary>
        /// 添加工位
        /// </summary>
        /// <param name="workbench">工位号</param>
        /// <param name="remark">工位备注</param>
        /// <param name="returnWorkbench">返回工位信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>添加成功返回True，添加失败返回False</returns>
        public bool Add(string workbench, string remark, out IQueryable<View_P_Workbench> returnWorkbench, out string error)
        {
            returnWorkbench = null;
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var billGather = from c in dataContxt.P_Workbench 
                                 where c.Workbench == workbench 
                                 select c;

                if (billGather.Count() == 0)
                {
                    P_Workbench tableWorkbench = new P_Workbench();

                    tableWorkbench.Workbench = workbench;
                    tableWorkbench.Remark = remark;

                    dataContxt.P_Workbench.InsertOnSubmit(tableWorkbench);
                    dataContxt.SubmitChanges();

                    returnWorkbench = Workbenchs;

                    return true;
                }
                else
                {
                    error = "已存在该工位信息不允许重复添加";
                    return false;
                }
            }
            catch (Exception err)
            {
                error = err.Message;
                return false;
            }
        }

        /// <summary>
        /// 更新工位
        /// </summary>
        /// <param name="oldWorkbench">旧工位号</param>
        /// <param name="newWorkbench">新工位号</param>
        /// <param name="remark">工位备注</param>
        /// <param name="returnWorkbench">输出操作后重新查询的所有工位信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功删除工位信息</returns>
       public bool Update(string oldWorkbench, string newWorkbench, string remark, out IQueryable<View_P_Workbench> returnWorkbench, out string error)
       {
            returnWorkbench = null;
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                var result = from c in dataContxt.P_Workbench 
                                 where c.Workbench == newWorkbench 
                                 select c;

                if (result.Count() == 0)
                {
                    result.Single().Workbench = newWorkbench;
                    result.Single().Remark = remark;

                    dataContxt.SubmitChanges();

                    returnWorkbench = Workbenchs;

                    return true;
                }
                else
                {
                    error = string.Format("已存在 {1} 工位信息, 无法将 {0} 修改为 {1}", oldWorkbench, newWorkbench);
                    return false;
                }
            }
            catch (Exception err)
            {
                error = err.Message;
                return false;
            }
        }

        /// <summary>
        /// 删除工位
        /// </summary>
       /// <param name="workbench">要删除的工位号</param>
        /// <param name="returnWorkbench">输出操作后重新查询的所有工位信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功删除工位信息</returns>
        public bool Delete(string workbench, out IQueryable<View_P_Workbench> returnWorkbench, out string error)
        { 
            error = null;
            returnWorkbench = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var delRow = from c in dataContxt.P_Workbench 
                             where c.Workbench == workbench 
                             select c;

                dataContxt.P_Workbench.DeleteAllOnSubmit(delRow);

                dataContxt.SubmitChanges();

                returnWorkbench = Workbenchs;

                return true;
            }
            catch (Exception err)
            {
                error = err.Message;
                return false;
            }
        }
    }
}
