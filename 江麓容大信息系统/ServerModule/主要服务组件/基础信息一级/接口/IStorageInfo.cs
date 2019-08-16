/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  StorageInfo.cs
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

namespace ServerModule
{
    /// <summary>
    /// 库房信息服务类
    /// </summary>
    public interface IStorageInfo
    {
        /// <summary>
        /// 通过查询某张表的某个单据字段获得指定表 指定单据的库房ID
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="tableName">数据库表名</param>
        /// <param name="billName">字段名称</param>
        /// <returns>返回库房ID</returns>
        string GetStorageID(string billID, string tableName, string billName);

        /// <summary>
        /// 获得库房信息表
        /// </summary>
        /// <returns>返货获取的库房信息</returns>
        DataTable GetStoreRoom();

        /// <summary>
        /// 获得库房人员关系表
        /// </summary>
        /// <returns>返回获取的库房人员关系信息</returns>
        DataTable GetStoreRoomAndPerson();

        /// <summary>
        /// 添加库房信息
        /// </summary>
        /// <param name="stroageID">库房ID</param>
        /// <param name="stroageName">库房名称</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>添加成功返回True，添加失败返回False</returns>
        bool AddStorage(string stroageID, string stroageName, out string error);

        /// <summary>
        /// 删除库房信息
        /// </summary>
        /// <param name="stroageID">库房ID</param>
        /// <param name="stroageName">库房名称</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        bool DeleteStorage(string stroageID, string stroageName, out string error);

        /// <summary>
        /// 添加库房人员关系信息
        /// </summary>
        /// <param name="workID">人员工号</param>
        /// <param name="stroageID">库房ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>添加成功返回True，添加失败返回False</returns>
        bool AddStorageAndPersonnel(string workID, string stroageID, out string error);

        /// <summary>
        /// 删除库房人员关系信息
        /// </summary>
        /// <param name="workID">人员工号</param>
        /// <param name="stroageID">库房ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        bool DeleteStorageAndPersonnel(string workID, string stroageID, out string error);

        /// <summary>
        /// 获得人员与库房的关系表
        /// </summary>
        /// <param name="workID">人员工号</param>
        /// <returns>返回获得的人员与库房的关系信息</returns>
        DataTable GetStorageIDAndPersonnel(string workID);

        /// <summary>
        /// 获得人员所属库房列表
        /// </summary>
        /// <returns>返回库房与人员关系表</returns>
        DataTable GetStorageNameFromPersonnel(string workID);
    }
}
