/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  RequestUserProcessor.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2010/07/17
 * 开发平台:  Visual C# 2008
 * 用于    :  仓库管理软件
 *----------------------------------------------------------------------------
 * 描述 : 
 * 其它 :
 *----------------------------------------------------------------------------
 * 公共信息: 参见系统'类帮助文档'
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2010/07/17 8:54:12 作者: 夏石友 当前版本: V1.00
 *        修改说明: 创建
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AsynSocketService;
using GlobalObject;
using SocketCommDefiniens;
using PlatformManagement;

namespace ServerRequestProcessorModule
{
    /// <summary>
    /// 响应用户登录处理器
    /// </summary>
    public class RequestUserProcessor
    {
        /// <summary>
        /// 接收事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="re">事件参数</param>
        public Socket_UserInfo ReceiveUserInfo(Socket_UserInfo userInfo)
        {
            try
            {
                #region 2013.04.16 修改了权限管理模块后取消了此功能
                //if (AuthenticationManager.IdentifyAuthorityForTempUser(userInfo.UserCode, userInfo.UserPwd))
                //{
                //    userInfo.LoginInStatus = Socket_UserInfo.LoginInStatusEnum.登录成功;
                //} 
                #endregion
            }
            catch (Exception err)
            {
                userInfo.LoginInStatus = Socket_UserInfo.LoginInStatusEnum.登录失败;
                userInfo.ErrorInfo = err.Message;
            }

            return userInfo;
        }
    }
}
