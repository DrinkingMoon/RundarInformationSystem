/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IHomemadePartInfoServer.cs
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
using System.Linq;

namespace ServerModule
{
    /// <summary>
    /// 自制件零件信息服务接口
    /// </summary>
    public interface IHomemadePartInfoServer
    {
        /// <summary>
        /// 判断是否属于自制件
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="goodsID">物品ID</param>
        /// <returns>True 是, False 否</returns>
        bool IsInHomemadePartInfo(DepotManagementDataContext ctx, int goodsID);

        /// <summary>
        /// 判断是否属于自制件
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <returns>True 是, False 否</returns>
        bool IsInHomemadePartInfo(int goodsID);
                
        /// <summary>
        /// 获取自制件零件信息
        /// </summary>
        /// <returns>返回获取到的自制件零件信息</returns>
        IQueryable<View_S_HomemadePartInfo> GetHomemadeAccessory();
    }
}
