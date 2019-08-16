/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  CVTTruckLoadingInformation.cs
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
using PlatformManagement;
using ServerModule;
using GlobalObject;

namespace ServerModule
{
    /// <summary>
    /// CVT装车信息服务类
    /// </summary>
    class CVTTruckLoadingInformation : ICVTTruckLoadingInformation
    {
        /// <summary>
        /// 获得装车信息
        /// </summary>
        /// <returns>返货装车信息</returns>
        public DataTable GetLoadingInfo()
        {
            string strSql = "select * from View_YX_LoadingInfo";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dtTemp;
        }

        /// <summary>
        /// 删除装车信息
        /// </summary>
        /// <param name="loadingInfo">装车信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        public bool DeleteLoadingInfo(YX_LoadingInfo loadingInfo, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.YX_LoadingInfo
                              where a.ID == loadingInfo.ID
                              select a;

                dataContext.YX_LoadingInfo.DeleteAllOnSubmit(varData);
                dataContext.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 更新装车信息
        /// </summary>
        /// <param name="loadingInfo">装车信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>更新成功返回True，更新失败返回False</returns>
        public bool UpdateLoadingInfo(YX_LoadingInfo loadingInfo, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.YX_LoadingInfo
                              where a.ID != loadingInfo.ID
                              && a.VehicleShelfNumber == loadingInfo.VehicleShelfNumber
                              select a;

                if (varData.Count() != 0)
                {
                    error = "数据不唯一";
                    return false;
                }
                else
                {
                    var varLoadingInfo = from a in dataContext.YX_LoadingInfo
                                         where a.ID == loadingInfo.ID
                                         select a;

                    YX_LoadingInfo lnqLoadingInfo = varLoadingInfo.Single();

                    lnqLoadingInfo.CVTNumber = loadingInfo.CVTNumber;
                    lnqLoadingInfo.Date = loadingInfo.Date;
                    lnqLoadingInfo.ProductID = loadingInfo.ProductID;
                    lnqLoadingInfo.Remark = loadingInfo.Remark;
                    lnqLoadingInfo.VehicleShelfNumber = loadingInfo.VehicleShelfNumber;

                    dataContext.SubmitChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 插入装车信息
        /// </summary>
        /// <param name="loadingInfo">装车信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>插入成功返回True，插入失败返回False</returns>
        public bool InsertIntoLoadingInfo(YX_LoadingInfo loadingInfo, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.YX_LoadingInfo
                              where a.VehicleShelfNumber == loadingInfo.VehicleShelfNumber
                              select a;

                if (varData.Count() != 0)
                {
                    error = "数据不唯一";
                    return false;
                }
                else
                {
                    dataContext.YX_LoadingInfo.InsertOnSubmit(loadingInfo);
                    dataContext.SubmitChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 批量插入装车信息
        /// </summary>
        /// <param name="loadingInfo">装车信息列表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>批量插入成功返回True，批量插入失败返回False</returns>
        public bool BatchInsertLoadingInfo(DataTable loadingInfo,
            out string error)
        {
            error = null;

            string strTemp = "";

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                ProductListServer serverProductList = new ProductListServer();

                for (int i = 0; i < loadingInfo.Rows.Count; i++)
                {
                    strTemp = loadingInfo.Rows[i]["车架号"].ToString().Trim();

                    if (strTemp == "")
                    {
                        continue;
                    }

                    YX_LoadingInfo lnqLoadingInfo = new YX_LoadingInfo();

                    int intGoodsID = serverProductList.GetProductGoodsID(
                        loadingInfo.Rows[i]["CVT型号"].ToString().Trim(), 0, false);

                    if (intGoodsID == 0)
                    {
                        error = "[CVT型号]不符合标准,车架号为[" + strTemp + "]";
                        return false;
                    }

                    int intCarModle = serverProductList.GetMotorcycleType(loadingInfo.Rows[i]["车型号"].ToString().Trim());

                    if (intCarModle == 0)
                    {
                        error = "[车型号]不符合标准,车架号为[" + strTemp + "]";
                        return false;
                    }

                    if (loadingInfo.Rows[i]["装车日期"].ToString().Trim() == "")
                    {
                        error = error + "[" + loadingInfo.Rows[i]["车架号"].ToString().Trim() + "]";
                    }

                    lnqLoadingInfo.CarModelID = intCarModle;
                    lnqLoadingInfo.CVTNumber = loadingInfo.Rows[i]["CVT编号"] == DBNull.Value ? ""
                        : loadingInfo.Rows[i]["CVT编号"].ToString().Trim();
                    lnqLoadingInfo.Date = ServerTime.ConvertToDateTime(
                        loadingInfo.Rows[i]["装车日期"].ToString());
                    lnqLoadingInfo.ProductID = intGoodsID;
                    lnqLoadingInfo.Remark = loadingInfo.Rows[i]["备注"].ToString().Trim();
                    lnqLoadingInfo.VehicleShelfNumber = loadingInfo.Rows[i]["车架号"].ToString().Trim();

                    dataContext.YX_LoadingInfo.InsertOnSubmit(lnqLoadingInfo);

                    strTemp = "";
                }

                dataContext.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message + ",车架号为[" + strTemp + "]";
                return false;
            }
        }
    }
}
