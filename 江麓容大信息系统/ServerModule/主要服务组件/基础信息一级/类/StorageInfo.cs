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
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using DBOperate;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using GlobalObject;

namespace ServerModule
{
    class StorageInfo : IStorageInfo
    {
        /// <summary>
        /// 获得人员与库房的关系表
        /// </summary>
        /// <param name="workID">人员工号</param>
        /// <returns>返回获得的人员与库房的关系信息</returns>
        public DataTable GetStorageIDAndPersonnel(string workID)
        {
            string strSql = "select * from BASE_StorageAndPersonnel where WorkID = '" + workID + "'";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得库房信息表
        /// </summary>
        /// <returns>返货获取的库房信息</returns>
        public DataTable GetStoreRoom()
        {
            string strSql = " select StorageID as 库房编码,StorageName as 库房名称, " +
                            " StorageLv as 库房级别 " +
                            " from BASE_Storage order by StorageID";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得库房人员关系表
        /// </summary>
        /// <returns>返回获取的库房人员关系信息</returns>
        public DataTable GetStoreRoomAndPerson()
        {
            string strSql = " select StorageID as 库房编码,ISNULL(dbo.fun_get_StorageName(StorageID),'') " +
                            " as 库房名称,WorkID as 人员工号,ISNULL(dbo.fun_get_Name(WorkID),'') as 人员姓名" +
                            " from BASE_StorageAndPersonnel order by StorageID,WorkID";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 添加库房信息
        /// </summary>
        /// <param name="stroageID">库房ID</param>
        /// <param name="stroageName">库房名称</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>添加成功返回True，添加失败返回False</returns>
        public bool AddStorage(string stroageID, string stroageName, out string error)
        {
            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;
                error = null;

                BASE_Storage lnqStroage = new BASE_Storage();

                lnqStroage.StorageID = stroageID;
                lnqStroage.StorageName = stroageName;
                lnqStroage.StorageLv = 1;

                dataContxt.BASE_Storage.InsertOnSubmit(lnqStroage);
                dataContxt.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 删除库房信息
        /// </summary>
        /// <param name="stroageID">库房ID</param>
        /// <param name="stroageName">库房名称</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        public bool DeleteStorage(string stroageID, string stroageName, out string error)
        {
            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                error = null;

                var varData = from a in dataContxt.BASE_Storage
                              where a.StorageID == stroageID
                              && a.StorageName == stroageName
                              select a;

                foreach (var item in varData)
                {
                    dataContxt.BASE_Storage.DeleteOnSubmit(item);
                }

                var varDataList = from b in dataContxt.BASE_StorageAndPersonnel
                                 where b.StorageID == stroageID
                                 select b;

                foreach (var itemList in varDataList)
                {
                    dataContxt.BASE_StorageAndPersonnel.DeleteOnSubmit(itemList);
                }

                dataContxt.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 添加库房人员关系信息
        /// </summary>
        /// <param name="workID">人员工号</param>
        /// <param name="stroageID">库房ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>添加成功返回True，添加失败返回False</returns>
        public bool AddStorageAndPersonnel(string workID, string stroageID, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                BASE_StorageAndPersonnel lnqStroageAndPersonal = new BASE_StorageAndPersonnel();

                lnqStroageAndPersonal.StorageID = stroageID;
                lnqStroageAndPersonal.WorkID = workID; ;

                dataContxt.BASE_StorageAndPersonnel.InsertOnSubmit(lnqStroageAndPersonal);
                dataContxt.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 删除库房人员关系信息
        /// </summary>
        /// <param name="workID">人员工号</param>
        /// <param name="stroageID">库房ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        public bool DeleteStorageAndPersonnel(string workID, string stroageID, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var varData = from a in dataContxt.BASE_StorageAndPersonnel
                              where a.StorageID == stroageID
                              && a.WorkID == workID
                              select a;

                foreach (var item in varData)
                {
                    dataContxt.BASE_StorageAndPersonnel.DeleteOnSubmit(item);
                }

                dataContxt.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 通过查询某张表的某个单据字段获得指定表 指定单据的库房ID
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="tableName">数据库表名</param>
        /// <param name="billName">字段名称</param>
        /// <returns>返回库房ID</returns>
        public string GetStorageID(string billID, string tableName, string billName)
        {
            string strSql = "select StorageID from " + tableName + " where " + billName + " = '" + billID + "'";
            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dtTemp.Rows.Count == 0)
            {
                return "";
            }

            return dtTemp.Rows[0][0].ToString();
        }


        /// <summary>
        /// 获得人员所属库房列表
        /// </summary>
        /// <returns>返回库房与人员关系表</returns>
        public DataTable GetStorageNameFromPersonnel(string workID)
        {
            string strSql = "select * from Base_StorageAndPersonnel as a " +
                    " inner join BASE_Storage as b on a.StorageID = b.StorageID " +
                    " where a.WorkID = '" + workID + "'";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }
    }
}
