using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Linq;
using ServerModule;
using GlobalObject;

namespace Service_Peripheral_HR
{
    /// <summary>
    /// 组织机构管理类
    /// </summary>
    class OrganizationServer : Service_Peripheral_HR.IOrganizationServer
    {
        /// <summary>
        /// 通过部门编码获得部门名称
        /// </summary>
        /// <param name="departCode">部门编码</param>
        /// <returns>部门名称</returns>
        public string GetDeptName(string departCode)
        {
            string strSql = "select DeptName from HR_Dept where DeptCode = '" + departCode + "'";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dtTemp.Rows[0][0].ToString();
        }

        /// <summary>
        /// 通过部门名称获得部门编码
        /// </summary>
        /// <param name="departName">部门名称</param>
        /// <returns>部门编码</returns>
        public string GetDeptCode(string departName)
        {
            string strSql = "select DeptCode from HR_Dept where DeptName = '" + departName + "' and DeleteFlag = 0";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dtTemp.Rows[0][0].ToString();
        }

        /// <summary>
        /// 获取所有的部门类别
        /// </summary>
        /// <returns>返回获取到的部门类别</returns>
        public IQueryable<HR_DeptType> GetAllDeptType()
        {
            return from r in CommentParameter.DepotDataContext.HR_DeptType
                   select r;
        }

        /// <summary>
        /// 获取部门信息表
        /// </summary>
        /// <param name="returnBill">部门信息表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取部门信息表</returns>
        public bool GetAllDeptInfo(out IQueryable<View_HR_Dept> returnBill, out string error)
        {
            returnBill = null;
            error = null;

            try
            {
                DepotManagementDataContext depotMangaeDataContext = CommentParameter.DepotDataContext;
                IQueryable<View_HR_Dept> departmentTable = depotMangaeDataContext.GetTable<View_HR_Dept>();

                returnBill = from c in departmentTable
                             where c.DeleteFlag == false
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
        /// <param name="deptCode">部门编号</param>
        /// <returns>成功返回获取到的部门，失败返回null</returns>
        public View_HR_Dept GetDeptByDeptCode(string deptCode)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.View_HR_Dept
                         where r.部门代码 == deptCode
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
        public bool AddDeptInfo(HR_Dept dept, out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from c in dataContxt.HR_Dept
                             where c.DeptCode == dept.DeptCode
                             select c;

                if (result.Count() == 0)
                {
                    dataContxt.HR_Dept.InsertOnSubmit(dept);
                }
                else
                {
                    HR_Dept deptInfo = result.Single();

                    deptInfo.DeleteFlag = false;
                    //error = string.Format("部门编码 {0} 的部门已经存在, 不允许重复添加", dept.DeptCode);
                    //return false;
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

        /// <summary>
        /// 修改部门信息表
        /// </summary>
        /// <param name="dept">部门信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加/修改部门信息表</returns>
        public bool UpdataDeptInfo(HR_Dept dept, out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from c in dataContxt.HR_Dept
                             where c.DeptCode == dept.DeptCode
                             select c;

                if (result.Count() == 0)
                {
                    error = "找不到相关记录, 无法进行此操作";
                    return false;
                }

                HR_Dept updateDept = result.Single();

                updateDept.DeptName = dept.DeptName;
                updateDept.DeptTypeID = dept.DeptTypeID;
                updateDept.OrderID = dept.OrderID;
                updateDept.Telephone = dept.Telephone;
                updateDept.DeleteFlag = false;
                updateDept.Fax = dept.Fax;
                updateDept.Remark = dept.Remark;
                updateDept.FatherCode = dept.FatherCode;

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
        /// 通过部门编码删除某一部门
        /// </summary>
        /// <param name="dept">部门信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功删除某一部门</returns>
        public bool DeleteDeptInfo(View_HR_Dept dept, out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from c in dataContxt.HR_Dept
                             where c.DeptCode == dept.部门代码
                             select c;

                if (result.Count() == 1)
                {
                    HR_Dept deptInfo = result.Single();

                    var varData = from a in dataContxt.HR_PersonnelArchive
                                  where a.Dept == dept.部门代码
                                  && a.PersonnelStatus == 1
                                  select a;

                    if (varData.Count() > 0)
                    {
                        throw new Exception("此部门存在在职人员 无法删除,如需删除请将此部门所属人员调离此部门，再进行删除操作");
                    }

                    deptInfo.DeleteFlag = true;
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
    }
}
