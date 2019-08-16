/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  ProductChange.cs
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
    /// 产品型号变更管理类
    /// </summary>
    class ProductChange : BasicServer, IProductChange
    {
        /// <summary>
        /// 检查某业务是否存在
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">业务号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.P_ProductChangeBill
                          where a.DJH == billNo
                          select a;

            if (varData == null || varData.Count() == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 检查某单据是否存在
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(string billNo)
        {
            string sql = "SELECT * FROM [DepotManagement].[dbo].[P_ProductChangeBill] where DJH = '" + billNo + "'";

            System.Data.DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 获得父表信息
        /// </summary>
        /// <returns>返回产品型号变更信息</returns>
        public DataTable GetAllBill()
        {
            string strSql = "select * from View_P_ProductChangeBill order by 单据号  desc";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dt;
        }

        /// <summary>
        /// 获得子表信息
        /// </summary>
        /// <param name="djh">变更单号</param>
        /// <returns>返回明细表信息</returns>
        public DataTable GetList(string djh)
        {
            string strSql = "select * from View_P_ProductChangeList where 单据号 = '" + djh + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dt;
        }

        /// <summary>
        /// 检查单据是否重复
        /// </summary>
        /// <param name="djh">变更单号</param>
        /// <returns>未重复返回True，重复返回False</returns>
        public bool IsRepeatBillID(string djh)
        {
            string strSql = "select * from P_ProductChangeBill where DJH = '" + djh + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dt.Rows.Count == 0 || dt == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 修改父表信息
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="inProductChange">产品型号变更信息</param>
        /// <param name="planList">明细信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>修改成功返回True，修改失败返回False</returns>
        public bool UpdateBill(DepotManagementDataContext ctx, P_ProductChangeBill inProductChange, 
            DataTable planList, out string error)
        {
            error = null;

            try
            {
                var varData = from a in ctx.P_ProductChangeBill
                              where a.DJH == inProductChange.DJH
                              select a;

                if (varData.Count() == 1)
                {

                    if (!DeletePlanList(ctx, inProductChange.DJH, out error))
                    {
                        return false;
                    }

                    if (!AddPlanList(ctx, planList, out error))
                    {
                        return false;
                    }

                    P_ProductChangeBill lnqPlan = varData.Single();

                    lnqPlan.DJZT = "等待主管审核";
                    lnqPlan.LRRY = BasicInfo.LoginID;
                    lnqPlan.LRRQ = ServerTime.Time;
                    lnqPlan.SHRQ = null;
                    lnqPlan.SHRY = null;
                    lnqPlan.PZRQ = null;
                    lnqPlan.PZRY = null;
                    lnqPlan.DJLX = inProductChange.DJLX;
                    lnqPlan.Remark = inProductChange.Remark;

                    ctx.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }

            return true;
        }

        /// <summary>
        /// 添加父表信息
        /// </summary>
        /// <param name="inProductChange">产品型号变更信息</param>
        /// <param name="planList">明细信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>添加成功返回True，添加失败返回False</returns>
        public bool AddBill(P_ProductChangeBill inProductChange, DataTable planList, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                if (!IsRepeatBillID(inProductChange.DJH))
                {

                    if (!UpdateBill(ctx, inProductChange, planList, out error))
                    {
                        return false;
                    }
                }
                else
                {

                    if (!DeletePlanList(ctx, inProductChange.DJH, out error))
                    {
                        return false;
                    }

                    if (!AddPlanList(ctx, planList, out error))
                    {
                        return false;
                    }

                    P_ProductChangeBill lnqPlan = new P_ProductChangeBill();

                    lnqPlan.DJH = inProductChange.DJH;
                    lnqPlan.DJZT = "等待主管审核";
                    lnqPlan.LRRY = BasicInfo.LoginID;
                    lnqPlan.LRRQ = ServerTime.Time;
                    lnqPlan.DJLX = inProductChange.DJLX;
                    lnqPlan.Remark = inProductChange.Remark;

                    ctx.P_ProductChangeBill.InsertOnSubmit(lnqPlan);
                }

                ctx.SubmitChanges();
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }

            return true;
        }

        /// <summary>
        /// 删除子表信息
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="djh">变更单号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        private bool DeletePlanList(DepotManagementDataContext context,
            string djh, out string error)
        {
            error = null;

            try
            {
                var varData = from a in context.P_ProductChangeList
                              where a.DJH == djh
                              select a;

                context.P_ProductChangeList.DeleteAllOnSubmit(varData);

                context.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 删除表信息
        /// </summary>
        /// <param name="djh">变更单号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        public bool DeleteBill(string djh, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext context = CommentParameter.DepotDataContext;

                var varData = from a in context.P_ProductChangeBill
                              where a.DJH == djh
                              select a;

                if (!DeletePlanList(context, djh, out error))
                {
                    return false;
                }

                context.P_ProductChangeBill.DeleteAllOnSubmit(varData);

                context.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 添加子表信息
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="planList">明细信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>添加成功返回True，添加失败返回False</returns>
        private bool AddPlanList(DepotManagementDataContext context,
            DataTable planList, out string error)
        {
            error = null;

            try
            {
                List<P_ProductChangeList> lisPlan = new List<P_ProductChangeList>();

                for (int i = 0; i < planList.Rows.Count; i++)
                {
                    P_ProductChangeList lnqPlanList = new P_ProductChangeList();

                    lnqPlanList.DJH = planList.Rows[i]["单据号"].ToString();
                    lnqPlanList.AfterChangeTypeGoodsID = Convert.ToInt32(planList.Rows[i]["变更后物品ID"]);
                    lnqPlanList.BeforeChangeTypeGoodsID = Convert.ToInt32(planList.Rows[i]["变更前物品ID"]);
                    lnqPlanList.Count = Convert.ToDecimal(planList.Rows[i]["数量"]);

                    lisPlan.Add(lnqPlanList);
                }

                context.P_ProductChangeList.InsertAllOnSubmit(lisPlan);

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 修改父表信息，主管审核
        /// </summary>
        /// <param name="djh">变更单号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>修改成功返回True，修改失败返回False</returns>
        public bool AuditingBill(string djh, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.P_ProductChangeBill
                              where a.DJH == djh
                              select a;

                if (varData.Count() == 1)
                {
                    P_ProductChangeBill lnqPlan = varData.Single();

                    lnqPlan.DJZT = "等待质管批准";
                    lnqPlan.SHRQ = ServerTime.Time;
                    lnqPlan.SHRY = BasicInfo.LoginID;
                    lnqPlan.PZRQ = null;
                    lnqPlan.PZRY = null;
                }

                ctx.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 修改父表信息，质管批准
        /// </summary>
        /// <param name="djh">变更单号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>修改成功返回True，修改失败返回False</returns>
        public bool AuthorizeBill(string djh,out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.P_ProductChangeBill
                              where a.DJH == djh
                              select a;

                if (varData.Count() == 1)
                {
                    P_ProductChangeBill lnqPlan = varData.Single();

                    lnqPlan.DJZT = "单据已完成";
                    lnqPlan.PZRQ = ServerTime.Time;
                    lnqPlan.PZRY = BasicInfo.LoginID;
                }

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
