using System;
namespace Service_Peripheral_HR
{
    /// <summary>
    /// 单据状态
    /// </summary>
    public enum PostChangeStatus
    {
        /// <summary>
        /// 新建单据
        /// </summary>
        新建单据,

        /// <summary>
        /// 等待调出部门负责人审核
        /// </summary>
        等待调出部门负责人审核,

        /// <summary>
        /// 等待调出分管领导审核
        /// </summary>
        等待调出分管领导审核,

        /// <summary>
        /// 等待调入部门负责人审核
        /// </summary>
        等待调入部门负责人审核,

        /// <summary>
        /// 等待调入分管领导审核
        /// </summary>
        等待调入分管领导审核,

        /// <summary>
        /// 等待公司办负责人审核
        /// </summary>
        等待公司办负责人审核,

        /// <summary>
        /// 等待总经理批准
        /// </summary>
        等待总经理批准,

        /// <summary>
        /// 等待原工作移交
        /// </summary>
        等待原工作移交,

        /// <summary>
        /// 等待人事档案调动
        /// </summary>
        等待人事档案调动,

        /// <summary>
        /// 等待信息化人员确认
        /// </summary>
        等待信息化人员确认,

        /// <summary>
        /// 等待固定资产人员确认
        /// </summary>
        等待固定资产人员确认,

        /// <summary>
        /// 未批准
        /// </summary>
        未批准,

        /// <summary>
        /// 已完成
        /// </summary>
        已完成
    }

    /// <summary>
    /// 岗位调动服务类
    /// </summary>
    public interface IPostChangeServer
    {
        /// <summary>
        /// 通过编号获得岗位调动信息
        /// </summary>
        /// <param name="billNo">单号</param>
        /// <param name="error">错误信息</param>
        /// <returns>返回数据集</returns>
        ServerModule.HR_PostChange GetPostChangeByBillNo(int billNo, out string error);

        /// <summary>
        /// 通过员工编号获得员工的调岗记录
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <param name="error">错误信息</param>
        /// <returns>返回数据集</returns>
        System.Data.DataTable GetPostChangeByWorkID(string workID, out string error);

        /// <summary>
        /// 获取所有岗位调动信息
        /// </summary>
        /// <param name="returnInfo">岗位调动信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool GetAllPostChange(out PlatformManagement.IQueryResult returnInfo, out string error);

        /// <summary>
        /// 新增岗位调动申请单
        /// </summary>
        /// <param name="postChange">岗位调动申请信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        int AddPostChange(ServerModule.HR_PostChange postChange, out string error);

        /// <summary>
        /// 修改岗位调动申请单
        /// </summary>
        /// <param name="postChange">岗位调动申请信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回大于0的整数，失败返回0</returns>
        bool UpdatePostChange(ServerModule.HR_PostChange postChange, out string error);

        /// <summary>
        /// 删除岗位调动申请单
        /// </summary>
        /// <param name="id">编号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool DeletePostChange(int id, out string error);

        /// <summary>
        /// 总经理审批修改岗位调动表
        /// </summary>
        /// <param name="id">申请编号</param>
        /// <param name="gmOpinion">总经理意见</param>
        /// <param name="authorize">总经理是否批准</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回false</returns>
        bool UpdateGMAuthor(int id, string gmOpinion, bool authorize, out string error);

        /// <summary>
        /// 公司办审批修改岗位调动表
        /// </summary>
        /// <param name="id">申请编号</param>
        /// <param name="hrOpinion">公司办意见</param>
        /// <param name="hrAuthorize">公司办是否批准</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回false</returns>
        bool UpdateHRAuthor(int id, string hrOpinion, bool hrAuthorize, out string error);

        /// <summary>
        /// 调入部门主管审批修改岗位调动表
        /// </summary>
        /// <param name="id">申请编号</param>
        /// <param name="newDeptOpinion">调入部门主管意见</param>
        /// <param name="newDeptAuthorize">调入部门是否批准</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回false</returns>
        bool UpdateNewDept(int id, string newDeptOpinion, bool newDeptAuthorize, out string error);

        /// <summary>
        /// 调出部门主管审批修改岗位调动表
        /// </summary>
        /// <param name="id">申请编号</param>
        /// <param name="oldDeptOpinion">调出部门主管意见</param>
        /// <param name="oldDeptAuthorize">调出部门是否批准</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回false</returns>
        bool UpdateOldDept(int id, string oldDeptOpinion, bool oldDeptAuthorize, out string error);

        /// <summary>
        /// 调出部门分管审批修改岗位调动表
        /// </summary>
        /// <param name="id">申请编号</param>
        /// <param name="oldLearderOpinion">调出部门分管意见</param>
        /// <param name="oldLearderAuthorize">调出部门是否批准</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回false</returns>
        bool UpdateOldLearder(int id, string oldLearderOpinion, bool oldLearderAuthorize, out string error);

        /// <summary>
        /// 调入部门分管审批修改岗位调动表
        /// </summary>
        /// <param name="id">申请编号</param>
        /// <param name="newLearderOpinion">调入部门分管意见</param>
        /// <param name="newLearderAuthorize">调入部门是否批准</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回false</returns>
        bool UpdateNewLearder(int id, string newLearderOpinion, bool newLearderAuthorize, out string error);

        /// <summary>
        /// 移交确认修改
        /// </summary>
        /// <param name="id">申请编号</param>
        /// <param name="flag">工作是否移交</param>
        /// <param name="name">员工姓名</param>
        /// <param name="workID">员工编号</param>
        /// <param name="personnelChange">档案历史数据集</param>
        /// <param name="dept">调入部门</param>
        /// <param name="postID">调入岗位编号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回false</returns>
        bool UpdateWorkTurnOver(int id, bool flag, string name, string workID, ServerModule.HR_PersonnelArchiveChange personnelChange,
            string dept, int postID, out string error);
    }
}
