using System;
using ServerModule;
using System.Collections.Generic;
using System.Linq;
using System.Data;
namespace Service_Peripheral_HR
{
    /// <summary>
    /// 人员档案管理类
    /// </summary>
    public interface IPersonnelArchiveServer
    {
        /// <summary>
        /// 添加人员档案
        /// </summary>
        /// <param name="personnel">人员档案数据集</param>
        /// <param name="list">人员档案奖罚等信息数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool AddPersonnelArchive(ServerModule.HR_PersonnelArchive personnel, HR_PersonnelArchiveList list, out string error);

        /// <summary>
        /// 获取指定员工编号和姓名的人员岗位
        /// </summary>
        /// <param name="name">员工姓名</param>
        /// <param name="code">员工编号</param>
        /// <returns>岗位</returns>
        string GetPersonnelArchiveByNameAndCode(string name, string code);

        /// <summary>
        /// 通过身份证号码获取员工档案信息
        /// </summary>
        /// <param name="cardID">身份证号码</param>
        /// <returns>返回满足条件的数据集</returns>
        View_HR_PersonnelArchive GetPersonnelArchiveViewByCardID(string cardID);

        /// <summary>
        /// 通过身份证号码获取员工档案信息
        /// </summary>
        /// <param name="cardID">身份证号码</param>
        /// <returns>返回满足条件的数据集</returns>
        HR_PersonnelArchive GetPersonnelArchiveByCardID(string cardID);

        /// <summary>
        /// 获取人员档案管理
        /// </summary>
        /// <param name="returnInfo">自制件工装报检单</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool GetAllBill(out PlatformManagement.IQueryResult returnInfo, out string error);

        /// <summary>
        /// 通过员工姓名获取职员编号
        /// </summary>
        /// <param name="name">姓名</param>
        /// <returns>返回获取到的职员信息视图, 获取不到返回null</returns>
        string GetPersonnelViewInfoByName(string name);

        /// <summary>
        /// 修改人员档案，把原始信息记录到变更表中
        /// </summary>
        /// <param name="personnelOld">原始的人员档案</param>
        /// <param name="personnelNew">修改后的人员档案</param>
        /// <param name="list">人员档案奖罚等信息数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool UpdatePersonnelArchive(ServerModule.HR_PersonnelArchiveChange personnelOld,
            ServerModule.HR_PersonnelArchive personnelNew, HR_PersonnelArchiveList list, out string error);

        /// <summary>
        /// 获得人员状态表
        /// </summary>
        /// <returns>返回人员状态数据集</returns>
        System.Data.DataTable GetPersonnelStatus();

        /// <summary>
        /// 通过状态名获得状态编号
        /// </summary>
        /// <param name="status">状态名</param>
        /// <returns>返回状态编号</returns>
        int GetStatusByName(string status);

        /// <summary>
        /// 通过状态编号获得状态名
        /// </summary>
        /// <param name="statusCode">状态编号</param>
        /// <returns>返回状态名</returns>
        string GetStatusByID(int statusCode);

        /// <summary>
        /// 批量插入人员档案
        /// </summary>
        /// <param name="personnelArchive">人员档案列表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>插入成功返回True，插入失败返回False</returns>
        bool InsertPersonnelArchiveInfo(System.Data.DataTable personnelArchive, out string error);

        /// <summary>
        /// 获取所有职员视图信息
        /// </summary>
        /// <returns>返回获取到的信息</returns>
        IQueryable<View_SelectPersonnel> GetAllInfo();

        /// <summary>
        /// 获取职员信息
        /// </summary>
        /// <param name="paramType">查询类型</param>
        /// <param name="parameter">查询信息</param>
        /// <returns>返回获取到的职员信息, 获取不到返回null</returns>
        System.Linq.IQueryable<ServerModule.View_SelectPersonnel> GetPersonnelViewInfo(Service_Peripheral_HR.PersonnelDefiniens.ParameterType paramType, string parameter);

        /// <summary>
        /// 获取直系负责人信息
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="managerType">权限类别 0：主管；1：负责人；2：分管领导</param>
        /// <returns>返回获取的负责人信息</returns>
        IQueryable<View_HR_PersonnelArchive> GetDeptDirector(string deptCode, string managerType);

        /// <summary>
        /// 获取直系负责人信息
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="managerType">权限类别 0：主管；1：负责人；2：分管领导</param>
        /// <returns>返回获取的负责人信息</returns>
        IQueryable<View_SelectPersonnel> GetDirector(string deptCode, string managerType);

        /// <summary>
        /// 获取某部门所有职员视图信息
        /// </summary>
        /// <param name="dept">部门</param>
        /// <returns>返回获取到的信息</returns>
        System.Linq.IQueryable<ServerModule.View_SelectPersonnel> GetAllInfo(string dept);

        /// <summary>
        /// 获取多个部门所有职员视图信息
        /// </summary>
        /// <param name="sql">拼接好的sql语句</param>
        /// <returns>返回获取到的信息</returns>
        List<View_SelectPersonnel> GetPersonByDept(string sql);

        /// <summary>
        /// 获取某部门所有职员视图信息
        /// </summary>
        /// <param name="dept">部门</param>
        /// <returns>返回获取到的信息</returns>
        DataTable GetAllInfoByDept(string dept);

        /// <summary>
        /// 获取人员档案变更历史管理
        /// </summary>
        /// <param name="returnInfo">人员档案变更历史</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool GetAllChangeBill(out PlatformManagement.IQueryResult returnInfo, out string error);

        /// <summary>
        /// 获取指定员工编号和姓名的人员档案
        /// </summary>
        /// <param name="name">员工姓名</param>
        /// <param name="code">员工编号</param>
        /// <returns>返回数据集</returns>
        System.Data.DataTable GetPersonnelInfo(string name, string code);

        /// <summary>
        /// 添加某部门负责人
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="lstPersonnel">负责人信息</param>
        /// <param name="managerType">权限类别 0：主管；1：负责人；2：分管领导</param>
        /// <param name="isPermission">是否有审批权限</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool AddDeptDirector(string deptCode, List<View_SelectPersonnel> lstPersonnel, string managerType, bool isPermission, out string error);

        /// <summary>
        /// 判断用户是否是指定部门的负责人
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="userCode">用户编码</param>
        /// <returns>是返回true</returns>
        bool IsDeptDirector(string deptCode, string userCode);

        /// <summary>
        /// 判断用户是否是指定人员的负责人
        /// </summary>
        /// <param name="underlingWorkID">下属工号</param>
        /// <param name="workID">需要判别负责人用户工号</param>
        /// <returns>是返回true</returns>
        bool IsUserDirector(string underlingWorkID, string workID);

        /// <summary>
        /// 删除某部门所有负责人
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="managerType">权限类别0：主管；1：负责人；2：分管领导</param>
        void DeleteDeptDirector(string deptCode, string managerType);

        /// <summary>
        /// 获得员工的最高部门
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <returns>返回部门名称和部门编号</returns>
        DataTable GetHighestDept(string workID);

        /// <summary>
        /// 获取某一岗位的所有人员
        /// </summary>
        /// <param name="postName">岗位名称</param>
        /// <returns>返回满足条件的数据集</returns>
        IQueryable<View_HR_PersonnelArchive> GetPersonnelArchiveByPostName(string postName);

        /// <summary>
        /// 指定员工编号和姓名的人员是否含有备注
        /// </summary>
        /// <param name="name">员工姓名</param>
        /// <param name="code">员工编号</param>
        /// <returns>是返回大于等于0的整形，否返回-1</returns>
        int GetPersonnelArchiveRemark(string name, string code);

        /// <summary>
        /// 获取指定员工编号和姓名的人员历史档案
        /// </summary>
        /// <param name="code">员工编号</param>
        /// <returns>返回数据集</returns>
        View_HR_PersonnelArchiveChange GetPersonnelChangeInfo(string code);

        /// <summary>
        /// 获取职员信息
        /// </summary>
        /// <param name="workID">工号</param>
        /// <returns>返回获取到的职员信息, 获取不到返回null</returns>
        HR_PersonnelArchive GetPersonnelInfo(string workID);

        /// <summary>
        /// 通过员工编号获取职员信息
        /// </summary>
        /// <param name="workID">工号</param>
        /// <returns>返回获取到的职员信息视图, 获取不到返回null</returns>
        View_HR_PersonnelArchive GetPersonnelViewInfo(string workID);

        /// <summary>
        /// 通过起始时间判断在职员工的考勤（已剔除不考勤员工）
        /// </summary>
        /// <param name="beginDate">起始时间</param>
        /// <param name="endDate">截止时间</param>
        /// <returns>返回数据集（员工编号，员工姓名，考勤方案）</returns>
        DataTable GetPersonnelAttendance(DateTime beginDate, DateTime endDate);

        /// <summary>
        /// 获取人员状态下的人员档案
        /// </summary>
        /// <param name="status">员工状态</param>
        /// <returns>返回数据集</returns>
        DataTable GetPersonnelStatus(string status);

        /// <summary>
        /// 添加打卡号与工号映射表
        /// </summary>
        /// <param name="cardWork">打卡号与工号映射表数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True 失败返回false</returns>
        bool AddCardIDWorkIDMapping(HR_CardID_WorkID_Mapping cardWork, out string error);

        /// <summary>
        /// 通过打卡号和员工工号删除打卡号与工号映射表的记录
        /// </summary>
        /// <param name="cardID">打卡号</param>
        /// <param name="workID">员工工号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True失败返回False</returns>
        bool DeleteCardIDWorkIDMapping(string cardID, string workID, out string error);

        /// <summary>
        /// 获得打卡号与工号映射表
        /// </summary>
        /// <returns>返回数据集</returns>
        DataTable GetCardIDWorkIDMapping();

        /// <summary>
        /// 批量修改部门/科室
        /// </summary>
        /// <param name="oldDept">原部门/科室编码</param>
        /// <param name="newDept">现部门/科室编码</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True失败返回false</returns>
        bool UpdateBatchDept(string oldDept, string newDept, out string error);

        /// <summary>
        /// 通过员工编号获得奖罚等信息
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <returns>返回数据集</returns>
        DataTable GetArchiveList(string workID);

        /// <summary>
        /// 通过员工编号获取职员信息(简化视图)
        /// </summary>
        /// <param name="workID">工号</param>
        /// <returns>返回获取到的职员信息视图, 获取不到返回null</returns>
        View_SelectPersonnel GetSelectPersonnel(string workID);

        /// <summary>
        /// 导出员工的平均年龄及各年龄段的人数
        /// </summary>
        /// <param name="showTable">返回的数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true</returns>
        bool ExcelPersonAge(out DataTable showTable, out string error);

        /// <summary>
        /// 导出员工的平均司龄及司龄段的人数
        /// </summary>
        /// <param name="showTable">返回的数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true</returns>
        bool ExcelIncompanyYears(out DataTable showTable, out string error);

        /// <summary>
        /// 导出时间范围类的离职分析
        /// </summary>
        /// <param name="startDate">起始时间</param>
        /// <param name="endDate">终止时间</param>
        /// <param name="showTable">返回的数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true</returns>
        bool ExcelDimission(DateTime startDate, DateTime endDate, out DataTable showTable, out string error);

        /// <summary>
        /// 导出各部门的在职人员
        /// </summary>
        /// <param name="showTable">返回的数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true</returns>
        bool ExcelOnjob(out DataTable showTable, out string error);

        /// <summary>
        /// 导出各学历人数
        /// </summary>
        /// <param name="showTable">返回的数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true</returns>
        bool ExcelEducation(out DataTable showTable, out string error);
    }
}
