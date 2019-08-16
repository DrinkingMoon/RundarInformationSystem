/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  Socket_StateInfo.cs
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

namespace SocketCommDefiniens
{
    /// <summary>
    /// 用于通信的状态信息类
    /// </summary>
    public class Socket_StateInfo
    {
        /// <summary>
        /// 状态信息
        /// </summary>
        Socket_FittingAccessoryInfo.OperateStateEnum m_stateInfo;

        /// <summary>
        /// 错误信息
        /// </summary>
        public string Error;

        /// <summary>
        /// 工位
        /// </summary>
        public string WorkBench;

        /// <summary>
        /// 状态信息
        /// </summary>
        public Socket_FittingAccessoryInfo.OperateStateEnum StateInfo
        {
            get { return m_stateInfo; }
            set { m_stateInfo = value; }
        }
       
    }
}
