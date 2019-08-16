using System;
using PlatformManagement;
using System.Collections.Generic;
using GlobalObject;

namespace ServerModule
{
    /// <summary>
    /// 单据类消息发布器
    /// </summary>
    public interface IBillMessagePromulgatorServer
    {   

        /// <summary>
        /// 根据角色类型与科室编码获得角色名称列表
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="roleStyle">角色类型</param>
        /// <param name="deptInfo">角色类型为仓管则为库房代码，角色类型为上级领导则为所需操作人员的人员工号或姓名,否则为需操作的部门编码</param>
        /// <returns>返回列表</returns>
        List<string> GetSuperior(DepotManagementDataContext ctx, CE_RoleStyleType roleStyle, string info);

        /// <summary>
        /// 发送传递流消息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="message">单据消息</param>
        /// <param name="receivedRole">接收消息的角色</param>
        void SendMessage(string billNo, string message, CE_RoleEnum receivedRole);
                
        /// <summary>
        /// 发送传递流消息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="message">单据消息</param>
        /// <param name="receivedUserType">接收用户类别，角色或用户</param>
        /// <param name="receiver">接收消息的角色或用户</param>
        void SendMessage(string billNo, string message, BillFlowMessage_ReceivedUserType receivedUserType, string receiver);
        
        /// <summary>
        /// 发送传递流消息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="message">单据消息</param>
        /// <param name="receivedUserType">接收用户类别，角色或用户</param>
        /// <param name="receiver">接收消息的角色或用户组</param>
        void SendMessage(string billNo, string message, BillFlowMessage_ReceivedUserType receivedUserType, List<string> receiver);
        
        /// <summary>
        /// 发送传递流消息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="message">单据消息</param>
        /// <param name="receivedUserType">接收用户类别，角色或用户</param>
        /// <param name="receiver">接收消息的角色或用户组</param>
        /// <param name="lstAdditionalInfo">
        /// 附加信息，用于重定向及数据定位等功能(列表第1个位置始终为重定向单据的单据类型，列表第2个位置始终为重定向单据的单据号)
        /// </param>
        void SendMessage(string billNo, string message, BillFlowMessage_ReceivedUserType receivedUserType,
            List<string> receiver, List<string> lstAdditionalInfo);

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>存在返回True, 不存在返回False</returns>
        bool IsExist(string billNo);

        /// <summary>
        /// 获得单据类型
        /// </summary>
        /// <param name="billTypeCode">单据类型代码</param>
        /// <returns>返回单据类型</returns>
        CE_BillTypeEnum GetBillTypeEnum(string billTypeCode);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="noticeRoles">知会角色</param>
        /// <param name="noticeUserCodes">知会用户编码列表</param>
        /// <returns>获取到的用户信息</returns>
        List<string> GetUserCodes(List<string> noticeRoles, List<string> noticeUserCodes);

        /// <summary>
        /// 获取或设置单据类别
        /// </summary>
        string BillType { get; set; }

        /// <summary>
        /// 消息提示界面句柄
        /// </summary>
        IntPtr MessagePromptFormHandle { get; set; }

        /// <summary>
        /// 根据角色类型与科室编码获得角色名称列表
        /// </summary>
        /// <param name="roleStyle">角色类型</param>
        /// <param name="deptInfo">角色类型为仓管则为库房代码，角色类型为上级领导则为所需操作人员的人员工号或姓名,否则为需操作的部门编码</param>
        /// <returns>返回列表</returns>
        List<string> GetSuperior(CE_RoleStyleType roleStyle, string deptInfo);

        /// <summary>
        /// 销毁消息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>操作是否成功的标志</returns>
        bool DestroyMessage(string billNo);

        /// <summary>
        /// 结束流消息(流程已经走完)
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="message">消息内容</param>
        /// <param name="noticeRoles">要知会的角色列表</param>
        /// <param name="noticeUsers">要知会的用户列表</param>
        /// <returns>操作是否成功的标志</returns>
        bool EndFlowMessage(string billNo, string message, System.Collections.Generic.List<string> noticeRoles,
            System.Collections.Generic.List<string> noticeUsers);

        /// <summary>
        /// 发布知会消息
        /// </summary>
        /// <param name="noticeInfo">知会信息</param>
        /// <param name="noticeRoles">知会角色</param>
        /// <param name="noticeUserCodes">知会用户编码列表</param>
        void NotifyMessage(NoticePublicInfo noticeInfo, System.Collections.Generic.List<string> noticeRoles,
            System.Collections.Generic.List<string> noticeUserCodes);

        /// <summary>
        /// 发布知会消息
        /// </summary>
        /// <param name="billType">单据类型</param>
        /// <param name="billNo">单据编号</param>
        /// <param name="content">知会内容</param>
        /// <param name="sender">发布人</param>
        /// <param name="noticeRoles">知会角色</param>
        /// <param name="noticeUserCodes">知会用户编码列表</param>
        void NotifyMessage(string billType, string billNo, string content, string sender,
            System.Collections.Generic.List<string> noticeRoles, System.Collections.Generic.List<string> noticeUserCodes);

        /// <summary>
        /// 传递流消息到指定角色或用户(走流程)
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="message">消息内容</param>
        /// <param name="receivedUserType">接收方类别，角色或用户</param>
        /// <param name="receiver">接收方代码，角色编码或用户编码</param>
        /// <returns>操作是否成功的标志</returns>
        bool PassFlowMessage(string billNo, string message, BillFlowMessage_ReceivedUserType receivedUserType, string receiver);

        /// <summary>
        /// 传递流消息到多个角色或用户(走流程)
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="message">消息内容</param>
        /// <param name="receivedUserType">接收方类别，角色或用户</param>
        /// <param name="receiver">接收方代码数组，角色编码或用户编码</param>
        /// <returns>操作是否成功的标志</returns>
        bool PassFlowMessage(string billNo, string message, BillFlowMessage_ReceivedUserType receivedUserType,
            System.Collections.Generic.List<string> receiver);

        /// <summary>
        /// 传递流消息到多个角色或用户(走流程)
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="message">消息内容</param>
        /// <param name="receivedUserType">接收方类别，角色或用户</param>
        /// <param name="receiver">接收方代码数组，角色编码或用户编码</param>
        /// <param name="lstAdditionalInfo">
        /// 附加信息，用于重定向及数据定位等功能(列表第1个位置始终为重定向单据的单据类型，列表第2个位置始终为重定向单据的单据号)
        /// </param>
        /// <returns>操作是否成功的标志</returns>
        bool PassFlowMessage(string billNo, string message, BillFlowMessage_ReceivedUserType receivedUserType,
            System.Collections.Generic.List<string> receiver, System.Collections.Generic.List<string> lstAdditionalInfo);

        /// <summary>
        /// 传递流消息到指定角色(走流程)
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="message">消息内容</param>
        /// <param name="receivedRole">接收角色编码</param>
        /// <returns>操作是否成功的标志</returns>
        bool PassFlowMessage(string billNo, string message, CE_RoleEnum receivedRole);

        /// <summary>
        /// 传递流消息到指定角色或用户(走流程)
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="message">消息内容</param>
        /// <param name="receiver">消息接收方, 角色编码或用户编码</param>
        /// <param name="flag">True 为角色：False 为用户</param>
        /// <returns>操作是否成功的标志</returns>
        bool PassFlowMessage(string billNo, string message, string receiver, bool flag);

        /// <summary>
        /// 回退流消息
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <returns>操作是否成功的标志</returns>
        bool RebackFlowMessage(string billNo, string message);

        /// <summary>
        /// 发送新的流消息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="message">单据消息</param>
        /// <param name="receivedUserType">接收用户类别，角色或用户</param>
        /// <param name="receiver">接收消息的角色或用户</param>
        void SendNewFlowMessage(string billNo, string message, BillFlowMessage_ReceivedUserType receivedUserType, string receiver);

        /// <summary>
        /// 发送新的流消息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="message">单据消息</param>
        /// <param name="receivedUserType">接收用户类别，角色或用户</param>
        /// <param name="receiver">接收消息的角色或用户组</param>
        void SendNewFlowMessage(string billNo, string message, BillFlowMessage_ReceivedUserType receivedUserType,
            System.Collections.Generic.List<string> receiver);

        /// <summary>
        /// 发送新的流消息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="message">单据消息</param>
        /// <param name="receivedRole">接收消息的角色</param>
        void SendNewFlowMessage(string billNo, string message, CE_RoleEnum receivedRole);

        /// <summary>
        /// 发送新的流消息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="message">单据消息</param>
        /// <param name="receivedUserType">接收用户类别，角色或用户</param>
        /// <param name="receiver">接收消息的角色或用户组</param>
        /// <param name="lstAdditionalInfo">
        /// 附加信息，用于重定向及数据定位等功能(列表第1个位置始终为重定向单据的单据类型，列表第2个位置始终为重定向单据的单据号)
        /// </param>
        void SendNewFlowMessage(string billNo, string message, BillFlowMessage_ReceivedUserType receivedUserType,
            System.Collections.Generic.List<string> receiver, System.Collections.Generic.List<string> lstAdditionalInfo);

        /// <summary>
        /// 根据库房ID或者库房名判断对应仓管角色
        /// </summary>
        /// <param name="storageInfo">库房ID/库房名称</param>
        /// <returns></returns>
        CE_RoleEnum GetRoleStringForStorage(string storageInfo);

        /// <summary>
        /// 获取部门主管角色名称
        /// </summary>
        /// <param name="deptInfo">部门编码或部门名称</param>
        /// <returns>返回获取到的角色名称数组</returns>
        string[] GetDeptDirectorRoleName(string deptInfo);

        /// <summary>
        /// 获取部门分管领导角色名称
        /// </summary>
        /// <param name="deptInfo">部门编码或部门名称</param>
        /// <returns>返回获取到的角色名称数组</returns>
        string[] GetDeptLeaderRoleName(string deptInfo);

        /// <summary>
        /// 获取部门负责人角色名称
        /// </summary>
        /// <param name="deptInfo">部门编码或部门名称</param>
        /// <returns>返回获取到的角色名称数组</returns>
        string[] GetDeptPrincipalRoleName(string deptInfo);

        /// <summary>
        /// 判断用户是否是指定部门的分管领导
        /// </summary>
        /// <param name="userCode">要判断的用户编码</param>
        /// <param name="deptCode">部门编码</param>
        /// <returns>是则返回true</returns>
        bool IsDeptLeader(string userCode, string deptCode);

        #region 2014-11-08 夏石友

        /// <summary>
        /// 获取指定部门的最高部门编码（如果当前部门就是顶级部门则直接返回当前部门编码）
        /// </summary>
        /// <param name="deptInfo">部门编码或部门名称</param>
        /// <returns>获取指定部门的最高部门的编码（如果当前部门就是顶级部门则直接返回当前部门编码）</returns>
        string GetHighestDeptCode(string deptInfo);

        /// <summary>
        /// 获取最高部门负责人角色名称
        /// </summary>
        /// <param name="deptInfo">部门编码或部门名称</param>
        /// <returns>返回获取到的角色名称数组</returns>
        string[] GetHighestDeptPrincipalRoleName(string deptInfo);

        #endregion
    }
}
