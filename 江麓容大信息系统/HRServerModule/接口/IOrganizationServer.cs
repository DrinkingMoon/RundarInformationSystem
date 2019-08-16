using System;

namespace Service_Peripheral_HR
{
    /// <summary>
    /// 组织机构管理接口类
    /// </summary>
    public interface IOrganizationServer
    {
        /// <summary>
        /// 添加部门信息表
        /// </summary>
        /// <param name="dept">部门信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加/修改部门信息表</returns>
        bool AddDeptInfo(ServerModule.HR_Dept dept, out string error);

        /// <summary>
        /// 通过部门编码删除某一部门
        /// </summary>
        /// <param name="dept">部门信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功删除某一部门</returns>
        bool DeleteDeptInfo(ServerModule.View_HR_Dept dept, out string error);

        /// <summary>
        /// 获取部门信息表
        /// </summary>
        /// <param name="returnBill">部门信息表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取部门信息表</returns>
        bool GetAllDeptInfo(out System.Linq.IQueryable<ServerModule.View_HR_Dept> returnBill, out string error);

        /// <summary>
        /// 获取所有的部门类别
        /// </summary>
        /// <returns>返回获取到的部门类别</returns>
        System.Linq.IQueryable<ServerModule.HR_DeptType> GetAllDeptType();

        /// <summary>
        /// 获取部门编号对应的部门信息
        /// </summary>
        /// <param name="deptCode">部门编号</param>
        /// <returns>成功返回获取到的部门，失败返回null</returns>
        ServerModule.View_HR_Dept GetDeptByDeptCode(string deptCode);

        /// <summary>
        /// 通过部门编码获得部门名称
        /// </summary>
        /// <param name="departCode">部门编码</param>
        /// <returns>部门名称</returns>
        string GetDeptName(string departCode);

        /// <summary>
        /// 修改部门信息表
        /// </summary>
        /// <param name="dept">部门信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加/修改部门信息表</returns>
        bool UpdataDeptInfo(ServerModule.HR_Dept dept, out string error);

        /// <summary>
        /// 通过部门名称获得部门编码
        /// </summary>
        /// <param name="departName">部门名称</param>
        /// <returns>部门编码</returns>
        string GetDeptCode(string departName);
    }
}
