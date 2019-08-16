/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  ISystemFileDestroyLogList.cs
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

namespace Service_Quality_File
{
    /// <summary>
    /// 文件销毁记录服务组件接口
    /// </summary>
    public interface ISystemFileDestroyLogList
    {

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="dorrList">LNQ数据集</param>
        void Add(ServerModule.FM_DestroyLogList destroyList);

        /// <summary>
        /// 批准
        /// </summary>
        /// <param name="fileID">文件ID</param>
        void Approve(int fileID);

        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="fileID">文件ID</param>
        void Delete(int fileID);

        /// <summary>
        /// 销毁
        /// </summary>
        /// <param name="fileID">文件ID</param>
        void Destroy(int fileID);

        /// <summary>
        /// 获得数据集
        /// </summary>
        /// <returns>返回Table</returns>
        System.Data.DataTable GetTableInfo();

        /// <summary>
        /// 修改信息
        /// </summary>
        /// <param name="dorrList">LNQ数据集</param>
        void Update(ServerModule.FM_DestroyLogList destroyList);
    }
}
