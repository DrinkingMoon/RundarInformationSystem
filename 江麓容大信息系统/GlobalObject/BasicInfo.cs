/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  FormLoggingIn.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2009/06/15
 * 开发平台:  vs2005(c#)
 * 用于    :  生产线管理信息系统
 *----------------------------------------------------------------------------
 * 描述 : 基础信息类
 * 其它 :
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2009/07/11 08:02:08 作者: 夏石友 当前版本: V1.00
 *        修改说明: 创建
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using PlatformManagement;
using System.Linq;

namespace GlobalObject
{
    /// <summary>
    /// 用户基础信息类
    /// </summary>
    public class BasicInfo
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        static string m_loginID;

        /// <summary>
        /// 用户ID
        /// </summary>
        static string m_loginName;

        /// <summary>
        /// 用户所属部门编码
        /// </summary>
        static string m_deptCode;

        /// <summary>
        /// 用户所属部门名称
        /// </summary>
        static string m_deptName;

        /// <summary>
        /// 登录用户角色
        /// </summary>
        static string m_loginRole;

        /// <summary>
        /// 用户角色
        /// </summary>
        static List<string> m_listRoles;

        /// <summary>
        /// 用户角色
        /// </summary>
        static string[] m_roleCodes;

        /// <summary>
        /// 角色功能树
        /// </summary>
        static List<FunctionTreeNodeInfo> m_listRoleFullFunctionTree;

        static Dictionary<int, string> _BaseSwitchInfo;

        public static Dictionary<int, string> BaseSwitchInfo
        {
            get { return BasicInfo._BaseSwitchInfo; }
            set { BasicInfo._BaseSwitchInfo = value; }
        }

        /// <summary>
        /// 用户ID
        /// </summary>
        public static string LoginID
        {
            get { return m_loginID; }
            set { m_loginID = value; }
        }

        /// <summary>
        /// 用户名称
        /// </summary>
        public static string LoginName
        {
            get { return m_loginName; }
            set { m_loginName = value; }
        }

        /// <summary>
        /// 用户所属部门编码
        /// </summary>
        public static string DeptCode
        {
            get { return m_deptCode; }
            set { m_deptCode = value; }
        }

        /// <summary>
        /// 用户所属部门名称
        /// </summary>
        public static string DeptName
        {
            get { return m_deptName; }
            set { m_deptName = value; }
        }

        /// <summary>
        /// 角色
        /// </summary>
        public static string LoginRole
        {
            get { return m_loginRole; }
            set { m_loginRole = value; }
        }

        /// <summary>
        /// 用户角色
        /// </summary>
        public static List<string> ListRoles
        {
            get { return m_listRoles; }
            set { m_listRoles = value; }
        }

        /// <summary>
        /// 用户角色
        /// </summary>
        public static string[] RoleCodes
        {
            get { return m_roleCodes; }
            set { m_roleCodes = value; }
        }  

        /// <summary>
        /// 用户角色
        /// </summary>
        public static List<FunctionTreeNodeInfo> ListRoleFullFunctionTree   
        {
            get { return m_listRoleFullFunctionTree; }
            set { m_listRoleFullFunctionTree = value; }
        }

        /// <summary>
        /// 获取功能树节点信息
        /// </summary>
        /// <param name="nodeName">节点名称</param>
        /// <returns>成功返回节点信息，失败返回null</returns>
        public static FunctionTreeNodeInfo GetFunctionTreeNodeInfo(string nodeName)
        {
            var result = from nodeInfo in m_listRoleFullFunctionTree
                         where nodeInfo.Name == nodeName
                         select nodeInfo;

            if (result.Count() > 0)
            {
                return result.First();
            }

            return null;
        }

        /// <summary>
        /// 检查角色中是否模糊包含某角色名称
        /// </summary>
        /// <param name="roleName">角色名称(可能是其中的一部分)</param>
        /// <returns>包含返回true</returns>
        public static bool IsFuzzyContainsRoleName(string roleName)
        {
            if (m_listRoles != null && m_listRoles.Count > 0)
            {
                foreach (var role in m_listRoles)
                {
                    if (role.Contains(roleName))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
