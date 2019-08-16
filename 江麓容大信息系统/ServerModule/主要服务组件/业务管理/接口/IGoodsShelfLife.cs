/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IGoodsShelfLife.cs
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
using System.Text;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using DBOperate;
using PlatformManagement;
using GlobalObject;

namespace ServerModule
{
    /// <summary>
    /// 物品保质期监控接口
    /// </summary>
    public interface IGoodsShelfLife
    {
        /// <summary>
        /// 是否需要进行保质期管理
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <returns>是 返回True,否 返回False</returns>
        bool IsShelfLife(int goodsID);

        /// <summary>
        /// 获得保质物品清单
        /// </summary>
        /// <returns></returns>
        DataTable GetBASEInfo();

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool DeleteInfo(int goodsID, string batchNo);

        /// <summary>
        /// 设置删除标志
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool UpdateDeleteFlag(int goodsID, string batchNo);

        /// <summary>
        /// 获得需要监控保质期的物品
        /// </summary>
        /// <param name="startTime">起始日期</param>
        /// <param name="endTime">截止日期</param>
        /// <returns>返回Table</returns>
        DataTable GetInfo(DateTime startTime, DateTime endTime);

        /// <summary>
        /// 插入新记录
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="goodsShelfLife">数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool InsertInfo(DepotManagementDataContext dataContext, KF_GoodsShelfLife goodsShelfLife, out string error);

        /// <summary>
        /// 设置一条物品记录
        /// </summary>
        /// <param name="goodsShelfLife">数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool SetInfo(ServerModule.KF_GoodsShelfLife goodsShelfLife, out string error);
    }
}
