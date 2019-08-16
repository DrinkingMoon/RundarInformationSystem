/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  DepartmentServer.cs
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
using System.Text;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using DBOperate;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace ServerModule
{
    /// <summary>
    /// 部门管理类
    /// </summary>
    class DepartmentServer : BasicServer, IDepartmentServer
    {
        /// <summary>
        /// 获得部门编码
        /// </summary>
        /// <param name="departmentName">部门名称</param>
        /// <returns>返回部门编码</returns>
        public string GetDepartmentCode(string departmentName)
        {
            string strSql = "select DepartmentCode from Department where DepartmentName = '" + departmentName + "'";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dtTemp.Rows[0][0].ToString();
        }

        /// <summary>
        /// 判断人员是否在某部门部门
        /// </summary>
        /// <param name="workID">人员工号</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool IsPersonByDept(string workID)
        {
            string strSql = "select DeptCode from HR_DeptManager where ManagerWorkID = '" + workID + "'";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dtTemp == null || dtTemp.Rows.Count == 0)
            {
                return false;
            }

            strSql = "select * from  HR_Personnel as h inner join (";

            foreach (DataRow dr in dtTemp.Rows)
            {
                strSql += " select * from dbo.fun_get_BelongDept('"+ dr["DeptCode"].ToString() +"') union all";
            }

            strSql = strSql.Substring(0, strSql.Length - 9);

            strSql += ") as d on h.dept = d.DeptCode where WorkID = '" + workID + "'";

            dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dtTemp == null || dtTemp.Rows.Count == 0)
            {
                return false;
            }

            if (dtTemp.Rows.Count > 1)
            {
                throw new Exception("数据不唯一");
            }

            return true;
        }

        /// <summary>
        /// 获得人员最好级所属部门
        /// </summary>
        /// <param name="personnel">人员姓名</param>
        /// <returns>返回 Table</returns>
        public DataTable GetPersonnelAffiliatedTopFunction(string personnel)
        {
            string strSql = "select * from Department where DepartmentCode in " +
                " (select Substring(dept,0,3) from HR_Personnel where Name = '" + personnel + "')";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得人员信息对应的部门信息
        /// </summary>
        /// <param name="personnelInfo">人员信息（姓名或工号）</param>
        /// <returns>部门名称</returns>
        public DataTable GetDeptInfoFromPersonnelInfo(string personnelInfo)
        {
            string strSql = "select DepartmentName,DepartmentCode from HR_Personnel as a " +
                " inner join Department as b on a.Dept = b.DepartmentCode where Name = '" + 
                personnelInfo + "' or WorkID = '" + personnelInfo + "'";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dtTemp;
        }

        /// <summary>
        /// 获得部门名称
        /// </summary>
        /// <param name="departmentCode">部门编码</param>
        /// <returns>部门名称</returns>
        public string GetDepartmentName(string departmentCode)
        {
            string strSql = "select DepartmentName from Department where DepartmentCode = '" + departmentCode + "'";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dtTemp.Rows.Count == 0)
            {
                return "";
            }
            else
            {
                return dtTemp.Rows[0][0].ToString();
            }
        }

        /// <summary>
        /// 获取所有的部门类别
        /// </summary>
        /// <returns>返回获取到的部门类别</returns>
        public IQueryable<DepartmentType> GetAllDepartmentType()
        {
            return from r in CommentParameter.DepotDataContext.DepartmentType
                   select r;
        }

        /// <summary>
        /// 获取部门信息表
        /// </summary>
        /// <param name="returnBill">部门信息表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取部门信息表</returns>
        public bool GetAllDepartment(out IQueryable<View_Department> returnBill, out string error)
        {
            returnBill = null;
            error = null;

            try
            {
                DepotManagementDataContext depotMangaeDataContext = CommentParameter.DepotDataContext;
                IQueryable<View_Department> departmentTable = depotMangaeDataContext.GetTable<View_Department>();

                returnBill = from c in departmentTable
                             where c.删除标志 == false
                             orderby c.排序号 
                             select c;

                return true;
            }
            catch (Exception err)
            {
                error = err.ToString();
                return false;
            }
        }

        /// <summary>
        /// 获取部门编号对应的部门信息
        /// </summary>
        /// <param name="departmentCode">部门编号</param>
        /// <returns>成功返回获取到的部门，失败返回null</returns>
        public View_Department GetDepartments(string departmentCode)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.View_Department
                         where r.部门代码 == departmentCode
                         select r;

            if (result.Count() == 0)
            {
                return null;
            }

            return result.Single();
        }

        /// <summary>
        /// 添加部门信息表
        /// </summary>
        /// <param name="dept">部门信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加/修改部门信息表</returns>
        public bool AddDepartment(Department dept, out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from c in dataContxt.Department 
                             where c.DepartmentCode == dept.DepartmentCode 
                             select c;

                if (result.Count() == 0)
                {
                    dataContxt.Department.InsertOnSubmit(dept);
                    dataContxt.SubmitChanges();
                }
                else
                {
                    error = string.Format("部门编码 {0} 的部门已经存在, 不允许重复添加", dept.DepartmentCode);
                    return false;
                }

                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;

                return false;
            }
        }
       
        /// <summary>
        /// 修改部门信息表
        /// </summary>
        /// <param name="dept">部门信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加/修改部门信息表</returns>
        public bool UpdataDepartment(Department dept, out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from c in dataContxt.Department
                             where c.DepartmentCode == dept.DepartmentCode
                             select c;

                if (result.Count() == 0)
                {
                    error = "找不到相关记录, 无法进行此操作";
                    return false;
                }

                Department updateDept = result.Single();

                updateDept.DepartmentName = dept.DepartmentName;
                updateDept.DeptType = dept.DeptType;
                updateDept.Telephone = dept.Telephone;
                updateDept.Fax = dept.Fax;
                updateDept.Remark = dept.Remark;

                dataContxt.SubmitChanges();

                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 删除某一部门
        /// </summary>
        /// <param name="departmentCode">部门编码</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功删除某一部门</returns>
        public bool DeleteDepartment(string departmentCode, out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                Table<Department> table = dataContxt.GetTable<Department>();

                var delRow = from c in table 
                             where c.DepartmentCode == departmentCode 
                             select c;

                foreach (var ei in delRow)
                {
                    table.DeleteOnSubmit(ei);
                }

                dataContxt.SubmitChanges();
                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        public DataTable GetDepartment_Finance()
        {
            string strSql = " select a.DeptCode as 部门编码, a.DeptName as 部门名称, a.FatherCode as 父级部门编码, b.DeptName as 父级部门名称 "+
                            " from HR_Dept as a left join HR_Dept as b on a.FatherCode = b.DeptCode " +
                            " where a.DeleteFlag = 0 and a.DeptCode <> '00'";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }
    }
}
