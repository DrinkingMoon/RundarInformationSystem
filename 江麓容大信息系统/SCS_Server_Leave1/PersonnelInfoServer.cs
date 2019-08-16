/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  PersonnelInfoServer.cs
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServerModule.PersonnelDefiniens;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using DBOperate;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using PlatformManagement;
namespace ServerModule
{
    namespace PersonnelDefiniens
    {
        /// <summary>
        /// 参数类型
        /// </summary>
        public enum ParameterType
        {
            /// <summary>
            /// 工号
            /// </summary>
            工号,

            /// <summary>
            /// 姓名
            /// </summary>
            姓名,

            /// <summary>
            /// 部门编码或部门名称
            /// </summary>
            部门
        }
    }

    /// <summary>
    /// 人员信息服务
    /// </summary>
    public class PersonnelInfoServer : BasicServer,IPersonnelInfoServer
    {
        /// <summary>
        /// 获取直系负责人信息
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <returns>返回获取的负责人信息</returns>
        public IQueryable<View_HR_Personnel> GetDeptDirector(string deptCode)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;
            IQueryable<string> dpWorkID = from r in dataContxt.DepartmentPrincipal 
                                          where r.DepartmentCode == deptCode 
                                          select r.PrincipalWorkID;

            if (dpWorkID.Count() == 0)
            {
                return null;
            }

            var result = from r in dataContxt.View_HR_Personnel
                         where dpWorkID.Contains(r.工号)
                         select r;

            return result;
        }

        /// <summary>
        /// 获取部门负责人信息
        /// 某部门链式负责人, 如某班组的负责人有班组级、车间级、部门级三级的所有包容式负责人
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <returns>返回获取的部门负责人信息</returns>
        public IQueryable<View_HR_Personnel> GetFuzzyDeptDirector(string deptCode)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            // 获取本部门指定负责人及本部门上级部门负责人
            IQueryable<string> dpWorkID = from r in dataContxt.DepartmentPrincipal
                                          where r.DepartmentCode == deptCode || (r.DepartmentCode.Length <= deptCode.Length 
                                          && (deptCode.Substring(0, r.DepartmentCode.Length) == r.DepartmentCode))
                                          select r.PrincipalWorkID;

            if (dpWorkID.Count() == 0)
            {
                return null;
            }

            var result = from r in dataContxt.View_HR_Personnel
                         where dpWorkID.Contains(r.工号)
                         select r;

            return result;
        }

        /// <summary>
        /// 删除某部门所有负责人
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        public void DeleteDeptDirector(string deptCode)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.DepartmentPrincipal
                         where r.DepartmentCode == deptCode
                         select r;

            dataContxt.DepartmentPrincipal.DeleteAllOnSubmit(result);
            dataContxt.SubmitChanges();
        }

        /// <summary>
        /// 添加某部门负责人
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="lstPersonnel">负责人信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool AddDeptDirector(string deptCode, List<View_HR_Personnel> lstPersonnel, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                foreach (var item in lstPersonnel)
                {
                    DepartmentPrincipal dp = new DepartmentPrincipal();

                    dp.DepartmentCode = deptCode;
                    dp.PrincipalWorkID = item.工号;

                    dataContxt.DepartmentPrincipal.InsertOnSubmit(dp);
                }

                dataContxt.SubmitChanges();
                return true;
            }
            catch (Exception exec)
            {
                error = exec.Message;
                return false;
            }
        }

        /// <summary>
        /// 判断用户是否是指定部门的负责人
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="userCode">用户编码</param>
        /// <returns>是返回true</returns>
        public bool IsDeptDirector(string deptCode, string userCode)
        {
            var result = GetFuzzyDeptDirector(deptCode);

            if (result == null || result.Count() == 0)
            {
                return false;
            }

            IQueryable<string> dpWorkID = from r in result 
                                          where r.工号 == userCode 
                                          select r.工号;
            return dpWorkID.Count() > 0;
        }

        /// <summary>
        /// 判断用户是否是指定人员的负责人
        /// </summary>
        /// <param name="underlingWorkID">下属工号</param>
        /// <param name="workID">需要判别负责人用户工号</param>
        /// <returns>是返回true</returns>
        public bool IsUserDirector(string underlingWorkID, string workID)
        {
            var result = GetFuzzyDeptDirector(GetPersonnelInfo(underlingWorkID).部门编码);

            if (result == null || result.Count() == 0)
            {
                return false;
            }

            IQueryable<string> dpWorkID = from r in result 
                                          where r.工号 == workID 
                                          select r.工号;
            return dpWorkID.Count() > 0;
        }

        /// <summary>
        /// 获取职员信息
        /// </summary>
        /// <param name="info">工号/姓名</param>
        /// <returns>返回获取到的职员信息, 获取不到返回null</returns>
        public View_HR_Personnel GetPersonnelInfo(string info)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.View_HR_Personnel
                         where r.工号 == info || r.姓名 == info
                         select r;

            View_HR_Personnel resultLnq = null;

            if (result.Count() > 0)
            {
                resultLnq = result.First();

                if (result.Count() > 1)
                {
                    var tempDelete = from a in result
                                     where a.DeleteFlag == false
                                     select a;

                    resultLnq = tempDelete.First();

                    if (tempDelete.Count() > 1)
                    {
                        var tempOperation = from a in tempDelete
                                            where a.是否操作用户 == true
                                            select a;

                        resultLnq = tempOperation.First();
                    }
                }

                return resultLnq;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 通过部门编码及员工姓名获取职员信息
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="name">员工姓名</param>
        /// <returns>返回获取到的职员信息, 获取不到返回null</returns>
        public View_HR_Personnel GetPersonnelInfoFromName(string deptCode, string name)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.View_HR_Personnel
                         where r.部门编码 == deptCode && r.姓名 == name
                         select r;

            if (result.Count() > 0)
                return result.Single();
            else
                return null;
        }

        /// <summary>
        /// 获取职员信息
        /// </summary>
        /// <param name="paramType">查询类型</param>
        /// <param name="parameter">查询信息</param>
        /// <returns>返回获取到的职员信息, 获取不到返回null</returns>
        public IQueryable<View_HR_Personnel> GetPersonnelViewInfo(ParameterType paramType, string parameter)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            switch (paramType)
            {
                case ParameterType.工号:
                    return from r in dataContxt.View_HR_Personnel
                           where r.工号 == parameter
                           select r;

                case ParameterType.姓名:
                    return from r in dataContxt.View_HR_Personnel
                           where r.姓名 == parameter
                           select r;

                case ParameterType.部门:
                    return from r in dataContxt.View_HR_Personnel
                           where r.部门名称 == parameter || r.部门编码 == parameter
                           select r;
            }

            return null;
        }

        /// <summary>
        /// 获取所有职员视图信息
        /// </summary>
        /// <returns>返回获取到的信息</returns>
        public IQueryable<View_HR_Personnel> GetAllInfo()
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            return from r in dataContxt.View_HR_Personnel 
                   select r;
        }

        /// <summary>
        /// 获取某部门所有职员视图信息
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <returns>返回获取到的信息</returns>
        public IQueryable<View_HR_Personnel> GetAllInfo(string deptCode)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            return from r in dataContxt.View_HR_Personnel
                   where r.部门编码.Contains(deptCode)
                   select r;
        }

        /// <summary>
        /// 获取某部门所有职员视图信息
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="bGetDisabledUser">是否获取已经禁用的用户</param>
        /// <returns>返回获取到的信息</returns>
        public IQueryable<View_HR_Personnel> GetAllInfo(string deptCode, bool bGetDisabledUser)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;
            
            if (bGetDisabledUser)
            {
                return from r in dataContxt.View_HR_Personnel
                       where r.部门编码.Contains(deptCode)
                       select r;
            }
            else
            {
                return from r in dataContxt.View_HR_Personnel
                       where r.部门编码.Contains(deptCode) && !r.DeleteFlag
                       select r;
            }
        }

        /// <summary>
        /// 添加人员信息表
        /// </summary>
        /// <param name="Person">人员信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加/修改部门信息表</returns>
        public bool AddPersonnel(HR_Personnel Person, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from c in dataContxt.HR_Personnel 
                             where c.WorkID == Person.WorkID 
                             select c;

                if (result.Count() == 0)
                {
                    dataContxt.HR_Personnel.InsertOnSubmit(Person);
                    dataContxt.SubmitChanges();
                }
                else
                {
                    error = string.Format("人员编码 {0} 的人员已经存在, 不允许重复添加", Person.WorkID);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 修改人员信息表
        /// </summary>
        /// <param name="Person">人员信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加/修改部门信息表</returns>
        public bool UpdatePersonnel(HR_Personnel Person, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var Result = from c in dataContxt.HR_Personnel 
                             where c.WorkID == Person.WorkID 
                             select c;

                if (Result.Count() == 0)
                {
                    error = "找不到相关记录，无法进行操作";
                    return false;
                }

                HR_Personnel updatePerson = Result.Single();

                updatePerson.WorkPost = Person.WorkPost;
                updatePerson.OnTheJob = Person.OnTheJob;
                updatePerson.Name = Person.Name;
                updatePerson.IsOperationalUser = Person.IsOperationalUser;
                updatePerson.Handset = Person.Handset;
                updatePerson.Remark = Person.Remark;
                updatePerson.DeleteFlag = Person.DeleteFlag;
                updatePerson.Dept = Person.Dept;

                dataContxt.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 获取职位信息
        /// </summary>
        /// <returns>返回职位信息</returns>
        public IQueryable<HR_PositionType> GetPositionType()
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            return from r in dataContxt.HR_PositionType 
                   select r;
        }

        /// <summary>
        /// 添加职位
        /// </summary>
        /// <param name="type">职位信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加职位信息</returns>
        public bool AddPositionType(HR_PositionType type, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from d in dataContxt.HR_PositionType 
                             where d.ID == type.ID 
                             select d;

                if (result.Count() == 0)
                {
                    dataContxt.HR_PositionType.InsertOnSubmit(type);
                    dataContxt.SubmitChanges();

                    return true;
                }
                else
                {
                    error = string.Format(" {0} 职位编号已经存在, 不允许重复添加", type.ID);
                    return false;
                }
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 修改职位
        /// </summary>
        /// <param name="type">职位信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功</returns>
        public bool UpdatePositionType(HR_PositionType type, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var Result = from e in dataContxt.HR_PositionType 
                             where e.ID == type.ID 
                             select e;

                if (Result.Count() == 0)
                {
                    error = "找不到相关记录，无法进行操作";
                    return false;
                }

                HR_PositionType updateType = Result.Single();

                updateType.PositionName = type.PositionName;
                updateType.Remark = type.Remark;

                dataContxt.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 获得人员选择的数据集
        /// </summary>
        /// <param name="info"></param>
        /// <returns>返回Table</returns>
        public DataTable GetInfo(string info)
        {
            string strSql = "select 员工编号,员工姓名 from View_HR_PersonnelArchive where 人员状态 = '在职'";

            if (info != "")
            {
                strSql += " and 部门编号 in (select DeptCode from fun_get_belongDept('" + info + "'))";
            }

            strSql += " order by 员工编号";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得人员选择的数据集
        /// </summary>
        /// <param name="type">查询类型</param>
        /// <param name="info"></param>
        /// <returns>返回Table</returns>
        public DataTable GetInfo_Find(BillFlowMessage_ReceivedUserType type, string info)
        {
            string strSql = "";

            switch (type)
            {
                case BillFlowMessage_ReceivedUserType.用户:
                    strSql = "select 员工编号,员工姓名 from View_HR_PersonnelArchive where 人员状态 = '在职'";

                    if (info != "")
                    {
                        strSql += " and (员工编号 like '%"+ info +"%' or 员工姓名 like '%"+ info +"%')";
                    }

                    strSql += " order by 员工编号";
                    break;
                case BillFlowMessage_ReceivedUserType.角色:

                    strSql = "select RoleCode as 角色编码, RoleName as 角色名称 from PlatformService.dbo.Auth_Role where 1=1 ";

                    if (info != "")
                    {
                        strSql += " and (RoleName like '%" + info + "%' or RoleCode like '%" + info + "%')";
                    }

                    strSql += " order by RoleName";
                    break;
                default:
                    break;
            }

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

    }
}
