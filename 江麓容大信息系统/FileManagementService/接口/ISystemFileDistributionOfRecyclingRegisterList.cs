/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  ISystemFileDistributionOfRecyclingRegisterList.cs
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
using System.Data;
using System.Collections.Generic;
using ServerModule;

namespace Service_Quality_File
{
    /// <summary>
    /// 文件发放回收登记表服务组件接口
    /// </summary>
    public interface ISystemFileDistributionOfRecyclingRegisterList : IBasicBillServer
    {
        /// <summary>
        /// 获得数据集
        /// </summary>
        /// <returns>返回Table</returns>
        System.Data.DataTable GetTableInfo();

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="dorrList">LNQ数据集</param>
        void Add(ref FM_DistributionOfRecyclingRegisterList dorrList);

        /// <summary>
        /// 修改信息
        /// </summary>
        /// <param name="dorrList">LNQ数据集</param>
        void Update(FM_DistributionOfRecyclingRegisterList dorrList);

        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="id">序号</param>
        void Delete(int id);

        /// <summary>
        /// 签收
        /// </summary>
        /// <param name="id">序号</param>
        void Sign(int id);

        /// <summary>
        /// 确认回收
        /// </summary>
        /// <param name="id">序号</param>
        void Recover(int id);
    }
}
