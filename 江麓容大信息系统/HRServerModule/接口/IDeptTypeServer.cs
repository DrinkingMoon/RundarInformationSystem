using System;
namespace Service_Peripheral_HR
{
    /// <summary>
    /// 部门类型管理类
    /// </summary>
    public interface IDeptTypeServer
    {
        /// <summary>
        /// 添加部门类别信息
        /// </summary>
        /// <param name="deptType">部门类别数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool AddDeptType(ServerModule.HR_DeptType deptType, out string error);

        /// <summary>
        /// 通过部门类别编号删除
        /// </summary>
        /// <param name="typeID">类别编号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool DeleteDeptType(int typeID, out string error);

        /// <summary>
        /// 获得所有部门类别
        /// </summary>
        /// <returns>返回部门类别数据集</returns>
        System.Data.DataTable GetAllDeptType();

        /// <summary>
        /// 修改部门类别信息
        /// </summary>
        /// <param name="deptType">部门类别数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool UpdateDeptType(ServerModule.HR_DeptType deptType, out string error);
    }
}
