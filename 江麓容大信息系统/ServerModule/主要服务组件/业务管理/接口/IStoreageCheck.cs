/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IStoreageCheck.cs
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
    /// 库房盘点单管理类接口
    /// </summary>
    public interface IStoreageCheck:IBasicBillServer
    {

        /// <summary>
        /// 是否重复建单
        /// </summary>
        /// <param name="storageID">库房名称</param>
        /// <param name="billNo">单据号</param>
        /// <returns>重复返回 True，不重复返回 False</returns>
        bool IsRepeat(string storageID, string billNo);
        
        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool DeleteBill(string billID, out string error);

        /// <summary>
        /// 获得主表信息
        /// </summary>
        /// <returns>返回获取的主表信息</returns>
        DataTable GetAllBill();

        /// <summary>
        /// 获得明细表信息
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <param name="depotTable">材料类别列表</param>
        /// <returns>返回获取的明细表信息</returns>
        DataTable GetList(string djh, DataTable depotTable);

        /// <summary>
        /// 添加主表信息
        /// </summary>
        /// <param name="inCheck">盘点单信息</param>
        /// <param name="checkList">明细信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>添加成功返回True，添加失败返回False</returns>
        bool AddBill(S_StorageCheck inCheck, DataTable checkList, out string error);

        /// <summary>
        /// 更新单据状态
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <param name="billStatus">单据状态</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>更改成功返回True，更改失败返回False</returns>
        bool UpdateBill(string djh, string billStatus, out string error);

        /// <summary>
        /// 获得单条主表信息
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <returns>返回获取的单挑主表信息</returns>
        S_StorageCheck GetBill(string djh);

        /// <summary>
        /// 获得物品明细信息
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <returns>返回获取的物品明细信息</returns>
        List<View_S_StorageCheckList> GetList(string djh);
    }
}
