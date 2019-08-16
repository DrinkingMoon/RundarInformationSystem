/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IntegrativeQuery.cs
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
using ServerModule;
using DBOperate;
using PlatformManagement;
using GlobalObject;

namespace Service_Peripheral_External
{
    /// <summary>
    /// 综合查询公共类
    /// </summary>
    public static class IntegrativeQuery
    {
        /// <summary>
        /// 获得库房或服务站的负责人数据集
        /// </summary>
        /// <param name="secStorageID">编码</param>
        /// <returns>返回数据LIST</returns>
        public static List<string> GetStorageOrStationPrincipal(string secStorageID)
        {
            List<string> list = new List<string>();

            string strSql = "select distinct WorkID from Client as a inner join HR_Personnel as b on a.Principal = b.Name where ClientCode = '"
                + secStorageID + "'";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dtTemp.Rows.Count == 0)
            {
                strSql = "select b.WorkID from BASE_Storage as a inner join BASE_StorageAndPersonnel as b on a.StorageID = b.StorageID " +
                    " inner join HR_Personnel as c on b.WorkID = c.WorkID where a.StorageID = '" + secStorageID + "'";

                dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

                if (dtTemp.Rows.Count != 0)
                {
                    for (int i = 0; i < dtTemp.Rows.Count; i++)
                    {
                        list.Add(dtTemp.Rows[i]["WorkID"].ToString());
                    }
                }
            }
            else
            {
                list.Add(dtTemp.Rows[0]["WorkID"].ToString());
            }

            return list;
        }

        /// <summary>
        /// 查询基础物品信息
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回LINQ数据集</returns>
        public static F_GoodsPlanCost QueryGoodsInfo(int goodsID)
        {
            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            var varData = from a in dataContext.F_GoodsPlanCost
                          where a.ID == goodsID
                          select a;

            if (varData.Count() == 1)
            {
                return varData.Single();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获得二级库房库存信息
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="secStorageID">库房ID</param>
        /// <param name="storageID">账务库房ID</param>
        /// <returns>返回二级库房库存信息数据集</returns>
        public static Out_Stock QuerySecStock(int goodsID,string secStorageID,string storageID)
        {
            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            var varData = from a in dataContext.Out_Stock
                          where a.GoodsID == goodsID
                          && a.SecStorageID == secStorageID
                          && a.StorageID == storageID
                          select a;

            if (varData.Count() ==1)
            {
                return varData.Single();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取是否为负责人
        /// </summary>
        /// <param name="storageID">库房ID</param>
        /// <param name="principal">姓名</param>
        /// <returns>是返回True，否返回False</returns>
        public static bool IsStockPrincipal(string storageID, string principal)
        {
            string strSql = "select * from Client where ClientCode = '" 
                + storageID + "' and Principal = '" + principal + "'";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dtTemp.Rows.Count == 0)
            {
                strSql = "select * from BASE_Storage as a inner join BASE_StorageAndPersonnel as b on a.StorageID = b.StorageID "+
                    " inner join HR_Personnel as c on b.WorkID = c.WorkID where a.StorageID = '" + storageID + "' and c.Name = '" + principal + "'";

                dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

                if (dtTemp.Rows.Count == 0)
                {
                    if (storageID == "ZK" && BasicInfo.ListRoles.Contains(CE_RoleEnum.质量工程师.ToString()))
                    {
                        return true;
                    }
                    else if (storageID == "TCUCJ" && BasicInfo.ListRoles.Contains(CE_RoleEnum.电子元器件仓库管理员.ToString()))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }

        }

        /// <summary>
        /// 判定是否为销售类库房
        /// </summary>
        /// <param name="stroage">库房ID或者库房名称</param>
        /// <returns>是返回True,否返回False</returns>
        public static bool IsSalesStorage(string stroage)
        {
            string strSql = "select * from Base_Storage where StorageID = '" + stroage + "' or StorageName = '" + stroage + "'";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dtTemp.Rows.Count == 0)
            {
                return false;
            }
            else
            {
                if (stroage == "05" || stroage == "09" || stroage == "售后库房" || stroage == "售后配件库房")
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// 判断库房是否为内部库房
        /// </summary>
        /// <param name="stroage">库房ID或库房名称</param>
        /// <returns>是返回True，否返回False</returns>
        public static bool IsInnerStorage(string stroage)
        {
            string strSql = "select * from Base_Storage where StorageID = '" + stroage + "' or StorageName = '" + stroage + "'";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dtTemp.Rows.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 是否为外部库房
        /// </summary>
        /// <param name="secStorageID">查询站点编码</param>
        /// <returns>是返回True,否返回False</returns>
        public static bool IsSecStorage(string secStorageID)
        {
            string strSql = "select * from Client where ClientCode = '" + secStorageID + "' and IsSecStorage = 1";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dtTemp.Rows.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 是否属于油品库物品
        /// </summary>
        /// <param name="goodsID"></param>
        /// <returns></returns>
        public static bool IsOilsGoods(int goodsID)
        {
            string strSql = "select * from S_Stock where StorageID = '06' and GoodsID = " + goodsID;

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dt.Rows.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 通过库房和物品信息获得库存信息
        /// </summary>
        /// <param name="storageID">库房ID</param>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <returns>返回零件的库存信息</returns>
        public static DataTable QueryStock(string storageID, string goodsCode, string goodsName, string spec)
        {
            string strSql = "select * from S_Stock where StorageID = '" + storageID + "' " +
                            " and GoodsCode = '" + goodsCode + "' and GoodsName='" + goodsName + "'"+
                            " and spec='" + spec + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dt;
        }
    }
}
