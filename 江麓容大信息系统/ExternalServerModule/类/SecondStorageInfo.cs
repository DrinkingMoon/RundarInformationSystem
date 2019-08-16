/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  SecondStorageInfo.cs
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
    /// 二级库房服务组件
    /// </summary>
    class SecondStorageInfo : ISecondStorageInfo
    {
        /// <summary>
        /// 获得二级库房信息
        /// </summary>
        /// <returns>返回TABLE</returns>
        public DataTable GetSecondStorageInfo()
        {
            string strSql = "select SecStorageID as 库房编码,SecStorageName as 库房名称,Remark as 备注 "+
                " from Out_StockInfo";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 插入二级库房信息
        /// </summary>
        /// <param name="outStockInfo">数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool InsertInfo(Out_StockInfo outStockInfo,out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.Out_StockInfo
                              where a.SecStorageID == outStockInfo.SecStorageID
                              select a;

                if (varData.Count() == 0)
                {
                    dataContext.Out_StockInfo.InsertOnSubmit(outStockInfo);
                    dataContext.SubmitChanges();
                    return true;
                }
                else
                {
                    error = "数据不唯一";
                    return false;
                }
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
        /// <param name="secStorageID">库房编码</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool DeleteInfo(string secStorageID, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.Out_StockInfo
                              where a.SecStorageID == secStorageID
                              select a;

                dataContext.Out_StockInfo.DeleteAllOnSubmit(varData);

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
        /// 修改库房信息
        /// </summary>
        /// <param name="oldStorageID">库房旧编码</param>
        /// <param name="outStockInfo">库房数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool ModifyInfo(string oldStorageID, Out_StockInfo outStockInfo,out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.Out_StockInfo
                              where a.SecStorageID == oldStorageID
                              select a;

                if (varData.Count() == 1)
                {
                    if (oldStorageID != outStockInfo.SecStorageID)
                    {
                        if (oldStorageID != outStockInfo.SecStorageID)
                        {
                            var varStockInfo = from c in dataContext.Out_StockInfo
                                               where c.SecStorageID == outStockInfo.SecStorageID
                                               select c;

                            if (varStockInfo.Count() > 0)
                            {
                                error = "此编码已存在，库房编码不能重复";
                                return false;
                            }
                        }

                        var varDetailData = from b in dataContext.Out_DetailAccount
                                            where b.SecStorageID == oldStorageID
                                            select b;

                        if (varDetailData.Count() > 0)
                        {
                            error = "此库房旧编码已在业务数据中存在，不能修改此库房的编码或者所属库房";
                            return false;
                        }
                    }

                    Out_StockInfo lnqStockInfo = varData.Single();

                    lnqStockInfo.Remark = outStockInfo.Remark;
                    lnqStockInfo.SecStorageID = outStockInfo.SecStorageID;
                    lnqStockInfo.SecStorageName = outStockInfo.SecStorageName;

                    dataContext.SubmitChanges();

                    return true;
                }
                else
                {
                    error = "数据不唯一";
                    return false;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }
    }
}
