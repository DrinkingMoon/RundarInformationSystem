/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  LogisticSafeStock.cs
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
    /// <summary>
    /// 物流安全库存
    /// </summary>
    class LogisticSafeStock : ILogisticSafeStock
    {
        /// <summary>
        /// 获得所有信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetInfo()
        {
            string strSql = "select * from View_S_LogisticSafeStock";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="logistic">LINQ数据</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool AddInfo(S_LogisticSafeStock logistic, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.S_LogisticSafeStock
                              where a.GoodsID == logistic.GoodsID
                              select a;

                if (varData.Count() != 0)
                {
                    error = "数据重复";
                    return false;
                }
                else
                {
                    ctx.S_LogisticSafeStock.InsertOnSubmit(logistic);
                    ctx.SubmitChanges();
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
        /// 更新信息
        /// </summary>
        /// <param name="updateGoodsID">旧物品ID</param>
        /// <param name="logistic">LINQ信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool UpdateInfo(int updateGoodsID, S_LogisticSafeStock logistic, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.S_LogisticSafeStock
                              where a.GoodsID == updateGoodsID
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数据不唯一";
                    return false;
                }
                else
                {
                    S_LogisticSafeStock lnqLogistic = varData.Single();

                    lnqLogistic.GoodsID = logistic.GoodsID;
                    lnqLogistic.MaxValue = logistic.MaxValue;
                    lnqLogistic.MinValue = logistic.MinValue;
                    lnqLogistic.Remark = logistic.Remark;

                    ctx.SubmitChanges();
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
        /// 删除信息
        /// </summary>
        /// <param name="deleteGoodsID">删除物品ID</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool DeleteInfo(int deleteGoodsID, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.S_LogisticSafeStock
                              where a.GoodsID == deleteGoodsID
                              select a;

                ctx.S_LogisticSafeStock.DeleteAllOnSubmit(varData);
                ctx.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }
    }
}
