/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IPersonnelInfoServer.cs
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
using PlatformManagement;

namespace ServerModule
{
    /// <summary>
    /// 人员信息服务接口
    /// </summary>
    public interface IPersonnelInfoServer
    {
        /// <summary>
        /// 获得人员选择的数据集
        /// </summary>
        /// <param name="type">查询类型</param>
        /// <param name="info"></param>
        /// <returns>返回Table</returns>
        DataTable GetInfo_Find(BillFlowMessage_ReceivedUserType type, string info);

        /// <summary>
        /// 获得人员选择的数据集
        /// </summary>
        /// <param name="info"></param>
        /// <returns>返回Table</returns>
        DataTable GetInfo(string info);

        /// <summary>
        /// 获取所有职员视图信息
        /// </summary>
        /// <returns>返回获取到的信息</returns>
        IQueryable<View_HR_Personnel> GetAllInfo();
             
        /// <summary>
        /// 获取某部门所有职员视图信息
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <returns>返回获取到的信息</returns>
        IQueryable<View_HR_Personnel> GetAllInfo(string deptCode);

        /// <summary>
        /// 获取某部门所有职员视图信息
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="bGetDisabledUser">是否获取已经禁用的用户</param>
        /// <returns>返回获取到的信息</returns>
        IQueryable<View_HR_Personnel> GetAllInfo(string deptCode, bool bGetDisabledUser);

        /// <summary>
        /// 通过部门编码及员工姓名获取职员信息
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="name">员工姓名</param>
        /// <returns>返回获取到的职员信息, 获取不到返回null</returns>
        View_HR_Personnel GetPersonnelInfoFromName(string deptCode, string name);
 
        /// <summary>
        /// 获取直系负责人信息
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <returns>返回获取的负责人信息</returns>
        IQueryable<View_HR_Personnel> GetDeptDirector(string deptCode);

        /// <summary>
        /// 获取负责人信息
        /// 某部门链式主管, 如某班组的主管有班组级、车间级、部门级三级的所有包容式主管
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <returns>返回获取的负责人信息</returns>
        IQueryable<View_HR_Personnel> GetFuzzyDeptDirector(string deptCode);

        /// <summary>
        /// 判断用户是否是指定人员的负责人
        /// </summary>
        /// <param name="underlingWorkID">下属工号</param>
        /// <param name="workID">需要判别负责人用户工号</param>
        /// <returns>是返回true</returns>
        bool IsUserDirector(string underlingWorkID, string workID);
 
        /// <summary>
        /// 判断用户是否是指定部门的主管
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="userCode">用户编码</param>
        /// <returns>是返回true</returns>
        bool IsDeptDirector(string deptCode, string userCode);

        /// <summary>
        /// 删除某部门所有负责人
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        void DeleteDeptDirector(string deptCode);

        /// <summary>
        /// 添加某部门负责人
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="lstPersonnel">负责人信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool AddDeptDirector(string deptCode, List<View_HR_Personnel> lstPersonnel, out string error);
 
        /// <summary>
        /// 获取职员信息
        /// </summary>
        /// <param name="info">工号/姓名</param>
        /// <returns>返回获取到的职员信息, 获取不到返回null</returns>
        View_HR_Personnel GetPersonnelInfo(string info);
                
        /// <summary>
        /// 获取职员信息
        /// </summary>
        /// <param name="paramType"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        IQueryable<View_HR_Personnel> GetPersonnelViewInfo(ParameterType paramType, string parameter);

        /// <summary>
        /// 添加人员信息表
        /// </summary>
        /// <param name="Person">人员信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加/修改部门信息表</returns>
        bool AddPersonnel(HR_Personnel Person, out string error);

        /// <summary>
        /// 修改人员信息表
        /// </summary>
        /// <param name="Person">人员信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加/修改部门信息表</returns>
        bool UpdatePersonnel(HR_Personnel Person, out string error);

        /// <summary>
        /// 获取职位信息
        /// </summary>
        /// <returns>返回职位信息</returns>
        IQueryable<HR_PositionType> GetPositionType();

        /// <summary>
        /// 添加职位
        /// </summary>
        /// <param name="type">职位信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加</returns>
        bool AddPositionType(HR_PositionType type, out string error);

        /// <summary>
        /// 修改职位
        /// </summary>
        /// <param name="type">职位信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功修改</returns>
        bool UpdatePositionType(HR_PositionType type,out string error);
    }
}
