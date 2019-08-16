using System;

namespace ServerModule
{
    /// <summary>
    /// 生产模块配置管理服务接口(装配用途、用途权限分配、工位用途设置)
    /// </summary>
    public interface IConfigManagement
    {
        #region 装配用途

        /// <summary>
        /// 获取所有装配用途
        /// </summary>
        /// <returns>获取到的装配用途信息</returns>
        System.Linq.IQueryable<ServerModule.View_ZPX_MultiBatchPartPurpose> GetAssemblingPurpose();

        /// <summary>
        /// 添加装配用途
        /// </summary>
        /// <param name="purpose">用途名称</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回true, 失败返回false</returns>
        bool AddAssemblingPurpose(string purpose, out string error);

        /// <summary>
        /// 修改装配用途
        /// </summary>
        /// <param name="oldPurpose">旧用途名称</param>
        /// <param name="newPurpose">新用途名称</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回true, 失败返回false</returns>
        bool UpdateAssemblingPurpose(string oldPurpose, string newPurpose, out string error);

        #endregion 装配用途

        #region 给人员分配装配用途权限, 每个工位都会分配装配用途，只有分配了权限的人员才可以配置工位、装配多批次信息等

        /// <summary>
        /// 获取所有装配用途权限
        /// </summary>
        /// <returns>获取到的装配用途权限信息</returns>
        System.Linq.IQueryable<ServerModule.View_ZPX_PersonnelAuthority> GetPurposeAuthority();

        /// <summary>
        /// 获取指定人员具有的装配用途权限信息
        /// </summary>
        /// <param name="workID">人员工号</param>
        /// <returns>获取到的装配用途权限信息</returns>
        System.Linq.IQueryable<ServerModule.View_ZPX_PersonnelAuthority> GetPurposeAuthority(string workID);

        /// <summary>
        /// 给人员添加装配用途权限
        /// </summary>
        /// <param name="workID">人员工号</param>
        /// <param name="purposeID">用途编号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回true, 失败返回false</returns>
        bool AddPurposeAuthority(string workID, int purposeID, out string error);

        /// <summary>
        /// 删除指定用户装配用途权限
        /// </summary>
        /// <param name="workID">工号</param>
        /// <param name="purposeID">用途编号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回true, 失败返回false</returns>
        bool DeletePurposeAuthority(string workID, int purposeID, out string error);

        #endregion 给人员分配装配用途权限

        #region 给工位分配装配用途, 只有分配了权限的人员才可以配置工位信息

        /// <summary>
        /// 获取所有工位用途信息
        /// </summary>
        /// <returns>获取到的工位用途信息</returns>
        System.Linq.IQueryable<ServerModule.WorkbenchPurpose> GetWorkbenchPurpose();

        /// <summary>
        /// 获取指定人员具有权限的工位用途信息
        /// </summary>
        /// <param name="workID">人员工号</param>
        /// <returns>获取到的工位用途信息</returns>
        System.Linq.IQueryable<ServerModule.WorkbenchPurpose> GetWorkbenchPurpose(string workID);

        /// <summary>
        /// 给工位设置装配用途
        /// </summary>
        /// <param name="workbench">工位号</param>
        /// <param name="purposeID">用途编号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回true, 失败返回false</returns>
        bool UpdateWorkbenchPurpose(string workbench, int purposeID, out string error);

        #endregion 给工位分配装配用途
    }
}
