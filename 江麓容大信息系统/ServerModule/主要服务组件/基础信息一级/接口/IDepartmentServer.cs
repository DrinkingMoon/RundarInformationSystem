/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IDepartmentServer.cs
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
    /// 部门管理类接口
    /// </summary>
    public interface IDepartmentServer
    {
        /// <summary>
        /// 获得部门——财务
        /// </summary>
        /// <returns></returns>
        DataTable GetDepartment_Finance();

        /// <summary>
        /// 获得部门编码
        /// </summary>
        /// <param name="departmentName">部门名称</param>
        /// <returns>返回部门编码</returns>
        string GetDepartmentCode(string departmentName);

        /// <summary>
        /// 判断人员是否在某部门部门
        /// </summary>
        /// <param name="workID">人员工号</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool IsPersonByDept(string workID);

        /// <summary>
        /// 获得人员最好级所属部门
        /// </summary>
        /// <param name="personnel">人员姓名</param>
        /// <returns>返回 Table</returns>
        DataTable GetPersonnelAffiliatedTopFunction(string personnel);

        /// <summary>
        /// 获得人员信息对应的部门信息
        /// </summary>
        /// <param name="personnelInfo">人员信息（姓名或工号）</param>
        /// <returns>部门名称</returns>
        DataTable GetDeptInfoFromPersonnelInfo(string personnelInfo);

        /// <summary>
        /// 获得部门名称
        /// </summary>
        /// <param name="departmentCode">部门编码</param>
        /// <returns>部门名称</returns>
        string GetDepartmentName(string departmentCode);

        /// <summary>
        /// 获取所有的部门类别
        /// </summary>
        /// <returns>返回获取到的部门类别</returns>
        IQueryable<DepartmentType> GetAllDepartmentType();

        /// <summary>
        /// 获取部门编号对应的部门信息
        /// </summary>
        /// <param name="departmentCode">部门编号</param>
        /// <returns>成功返回获取到的部门，失败返回null</returns>
        View_Department GetDepartments(string departmentCode);

        /// <summary>
        /// 获取部门信息表
        /// </summary>
        /// <param name="returnBill">部门信息表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取部门信息表</returns>
        bool GetAllDepartment(out IQueryable<View_Department> returnBill, out string error);

        /// <summary>
        /// 添加部门信息表
        /// </summary>
        /// <param name="dept">部门信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加/修改部门信息表</returns>
        bool AddDepartment(Department dept, out string error);

        /// <summary>
        /// 修改部门信息表
        /// </summary>
        /// <param name="dept">部门信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加/修改部门信息表</returns>
        bool UpdataDepartment(Department dept, out string error);

        /// <summary>
        /// 删除某一部门
        /// </summary>
        /// <param name="departmentCode">部门编码</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功删除某一部门</returns>
        bool DeleteDepartment(string departmentCode, out string error);
    }
}
