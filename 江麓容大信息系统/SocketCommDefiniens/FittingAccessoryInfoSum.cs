/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  Socket_FittingAccessoryInfoSum.cs
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
    /// 用于通信的装配零件信息类
    /// </summary>
    public class Socket_FittingAccessoryInfoSum
    {
        /// <summary>
        /// 产品类型名称
        /// </summary>
        string m_productTypeName;

        /// <summary>
        /// 产品类型名称
        /// </summary>
        public string ProductTypeName
        {
            get { return m_productTypeName; }
            set { m_productTypeName = value; }
        }

        /// <summary>
        /// 产品编码
        /// </summary>
        string m_productCode;

        /// <summary>
        /// 设置或获取产品编码
        /// </summary>
        public string ProductCode
        {
            get { return m_productCode; }
            set { m_productCode = value; }
        }

        /// <summary>
        /// 工位
        /// </summary>
        string m_workBench;

        /// <summary>
        /// 设置或获取工位
        /// </summary>
        public string WorkBench
        {
            get { return m_workBench; }
            set { m_workBench = value; }
        }

        /// <summary>
        /// 装配人员
        /// </summary>
        string m_fittingPersonnel;

        /// <summary>
        /// 设置或获取装配人员
        /// </summary>
        public string FittingPersonnel
        {
            get { return m_fittingPersonnel; }
            set { m_fittingPersonnel = value; }
        }

        /// <summary>
        /// 装配时间
        /// </summary>
        string m_fittingTime;

        /// <summary>
        /// 设置或获取装配时间
        /// </summary>
        public string FittingTime
        {
            get { return m_fittingTime; }
            set { m_fittingTime = value; }
        }

        /// <summary>
        /// 修改人员
        /// </summary>
        string m_amendPersonnel;

        /// <summary>
        /// 设置或获取修改人员
        /// </summary>
        public string AmendPersonnel
        {
            get { return m_amendPersonnel; }
            set { m_amendPersonnel = value; }
        }

        /// <summary>
        /// 修改时间
        /// </summary>
        string m_amendTime;

        /// <summary>
        /// 设置或获取修改时间
        /// </summary>
        public string AmendTime
        {
            get { return m_amendTime; }
            set { m_amendTime = value; }
        }

        /// <summary>
        /// 版本
        /// </summary>
        string edition;

        /// <summary>
        /// 设置或获取版本
        /// </summary>
        public string Edition
        {
            get { return edition; }
            set { edition = value; }
        }

        /// <summary>
        /// 装配信息类列表
        /// </summary>
        List<Socket_FittingAccessoryInfo> m_fittingAccessoryInfoList = new List<Socket_FittingAccessoryInfo>();

        /// <summary>
        /// 装配信息类列表
        /// </summary>
        public List<Socket_FittingAccessoryInfo> FittingAccessoryInfoList
        {
            get { return m_fittingAccessoryInfoList; }
            set { m_fittingAccessoryInfoList = value; }
        }
    }
}
