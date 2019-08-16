using System;
namespace Service_Peripheral_HR
{
    /// <summary>
    /// 岗位管理接口类
    /// </summary>
    public interface IOperatingPostServer
    {
        /// <summary>
        /// 添加岗位
        /// </summary>
        /// <param name="operatPost">岗位信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回True，失败返回false</returns>
        bool AddPost(ServerModule.HR_OperatingPost operatPost, out string error);

        /// <summary>
        /// 添加岗位类别
        /// </summary>
        /// <param name="postType">岗位类别信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True 失败返回False</returns>
        bool AddPostType(ServerModule.HR_PostType postType, out string error);

        /// <summary>
        /// 通过类别名称获取岗位类别编号
        /// </summary>
        /// <param name="typeName">岗位类别名称</param>
        /// <returns>返回岗位类别编号</returns>
        string GetPostTypeByTypeName(string typeName);

        /// <summary>
        /// 通过岗位名称获取岗位信息
        /// </summary>
        /// <param name="name">岗位名称</param>
        /// <returns>返回岗位信息</returns>
        ServerModule.View_HR_OperatingPost GetOperatingPostByPostName(string name);

        /// <summary>
        /// 获得部门的总人数
        /// </summary>
        /// <param name="deptName">岗位名称</param>
        /// <returns>成功返回数据集，失败返回null</returns>
        System.Data.DataTable GetDeptCount(string deptName);

        /// <summary>
        /// 通过岗位编号获取岗位信息
        /// </summary>
        /// <param name="postCode">岗位编号</param>
        /// <returns>返回岗位信息</returns>
        string GetOperatingPostByPostCode(int postCode);

        /// <summary>
        /// 通过岗位编号删除岗位类别
        /// </summary>
        /// <param name="typeID">岗位编号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool DeletePostType(int typeID, out string error);

        /// <summary>
        /// 获取岗位信息
        /// </summary>
        /// <param name="dept">所属部门</param>
        /// <returns>返回岗位信息</returns>
        System.Data.DataTable GetOperatingPost(string dept);

        /// <summary>
        /// 通过岗位编号删除岗位
        /// </summary>
        /// <param name="postID">岗位编号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool DeletePost(int postID, out string error);

        /// <summary>
        /// 获取岗位类别信息
        /// </summary>
        /// <returns>返回岗位类别信息</returns>
        System.Data.DataTable GetPostType();

        /// <summary>
        /// 修改岗位
        /// </summary>
        /// <param name="operatPost">岗位信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True ，失败返回False</returns>
        bool UpdatePost(ServerModule.HR_OperatingPost operatPost, out string error);

        /// <summary>
        /// 修改岗位类别
        /// </summary>
        /// <param name="postType">岗位类别信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True ，失败返回False</returns>
        bool UpdatePostType(ServerModule.HR_PostType postType, out string error);

        /// <summary>
        /// 获得岗位编制的信息
        /// </summary>
        /// <param name="postName">岗位名称</param>
        /// <returns>成功返回True，失败返回False</returns>
        System.Data.DataTable GetDeptPost(string postName);

        /// <summary>
        /// 添加/更新部门工作岗位
        /// </summary>
        /// <param name="deptPost">工作岗位信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool AddDeptPost(ServerModule.HR_DeptPost deptPost, out string error);

        /// <summary>
        /// 批量添加/更新部门工作岗位
        /// </summary>
        /// <param name="deptPost">工作岗位信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool AddDeptPost(System.Data.DataTable deptPost, out string error);

        /// <summary>
        /// 删除岗位编制信息
        /// </summary>
        /// <param name="deptPost">岗位编制信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true，失败返回False</returns>
        bool DeleteDeptPost(ServerModule.HR_DeptPost deptPost, out string error);

        /// <summary>
        /// 通过部门和岗位ID获得编制信息
        /// </summary>
        /// <param name="deptName">部门编号</param>
        /// <returns>返回岗位编制信息</returns>
        System.Linq.IQueryable<ServerModule.View_HR_DeptPost> GetDeptPostByDeptCode(string deptName);

        /// <summary>
        /// 获取岗位信息
        /// </summary>
        /// <returns>返回岗位信息</returns>
        System.Data.DataTable GetOperatingPost(int postID);
    }
}
