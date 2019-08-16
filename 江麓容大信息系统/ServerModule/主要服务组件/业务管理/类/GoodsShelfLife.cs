/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  GoodsShelfLife.cs
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
    /// 物品保质期管理类
    /// </summary>
    class GoodsShelfLife : ServerModule.IGoodsShelfLife
    {
        /// <summary>
        /// 是否需要进行保质期管理
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <returns>是 返回True,否 返回False</returns>
        public bool IsShelfLife(int goodsID)
        {
            string strSql = " select * from F_GoodsPlanCost as a inner join F_GoodsAttributeRecord as b on a.ID = b.GoodsID "+
                            " where AttributeID = " + (int)CE_GoodsAttributeName.保质期 + " and AttributeValue = 'KF_GoodsShelfLife' and a.ID = " + goodsID;

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
        /// 获得需要监控保质期的物品
        /// </summary>
        /// <param name="startTime">起始日期</param>
        /// <param name="endTime">截止日期</param>
        /// <returns>返回Table</returns>
        public DataTable GetInfo(DateTime startTime, DateTime endTime)
        {
            string strSql = "select * from View_KF_GoodsShelfLife where 入库日期 >= '"
                + startTime +"' and 入库日期 <= '"+ endTime +"' order by 入库日期";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);

        }

        /// <summary>
        /// 设置删除标志
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool UpdateDeleteFlag(int goodsID, string batchNo)
        {
            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            var varData = from a in dataContext.KF_GoodsShelfLife
                          where a.GoodsID == goodsID
                          && a.BatchNo == batchNo
                          select a;

            if (varData.Count() != 1)
            {
                return false;
            }
            else
            {
                KF_GoodsShelfLife lnqShelfLife = varData.Single();

                lnqShelfLife.IsDelete = true;
            }

            dataContext.SubmitChanges();

            return true;
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool DeleteInfo(int goodsID, string batchNo)
        {
            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            var varData = from a in dataContext.KF_GoodsShelfLife
                          where a.GoodsID == goodsID
                          && a.BatchNo == batchNo
                          && a.IsDelete == true
                          select a;

            if (varData.Count() != 1)
            {
                return false;
            }
            else
            {
                dataContext.KF_GoodsShelfLife.DeleteAllOnSubmit(varData);
            }

            dataContext.SubmitChanges();

            return true;
        }

        /// <summary>
        /// 设置一条物品记录
        /// </summary>
        /// <param name="goodsShelfLife">数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool SetInfo(KF_GoodsShelfLife goodsShelfLife,out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varInfo = from a in dataContext.KF_GoodsShelfLife
                              where a.BatchNo == goodsShelfLife.BatchNo
                              && a.GoodsID == goodsShelfLife.GoodsID
                              select a;

                if (varInfo.Count() != 1)
                {
                    error = "数据不唯一或者为空";
                    return false;
                }
                else
                {
                    KF_GoodsShelfLife lnqGoodsShelfLife = varInfo.Single();

                    lnqGoodsShelfLife.DateInProduced = goodsShelfLife.DateInProduced;
                    lnqGoodsShelfLife.FillInDate = ServerTime.Time;
                    lnqGoodsShelfLife.FillInPersonnel = BasicInfo.LoginName;
                    lnqGoodsShelfLife.ShelfLife = goodsShelfLife.ShelfLife;
                }

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
        /// 插入新记录
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="goodsShelfLife">数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool InsertInfo(DepotManagementDataContext dataContext, KF_GoodsShelfLife goodsShelfLife, out string error)
        {
            error = null;

            try
            {
                var varInfo = from a in dataContext.KF_GoodsShelfLife
                              where a.BatchNo == goodsShelfLife.BatchNo
                              && a.GoodsID == goodsShelfLife.GoodsID
                              select a;

                if (varInfo.Count() != 0)
                {
                    error = "数据不唯一";
                    return false;
                }
                else
                {
                    dataContext.KF_GoodsShelfLife.InsertOnSubmit(goodsShelfLife);
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
        /// 获得保质物品清单
        /// </summary>
        /// <returns></returns>
        public DataTable GetBASEInfo()
        {
            string strSql = "select GoodsCode as 图号型号,GoodsName as 物品名称,Spec as 规格, b.GoodsID as 物品ID " +
                " from F_GoodsPlanCost as a inner join F_GoodsAttributeRecord as b on a.ID = b.GoodsID " +
                " where AttributeID = " + (int)CE_GoodsAttributeName.保质期 + " and AttributeValue = 'KF_GoodsShelfLife' and IsDisable = 0";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }
    }
}
