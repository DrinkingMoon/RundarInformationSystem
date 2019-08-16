/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  ProductPlan.cs
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
using System.Windows.Forms;

namespace ServerModule
{
    /// <summary>
    /// 生产计划管理类
    /// </summary>
    class ProductPlan : BasicServer, ServerModule.IProductPlan
    {
        /// <summary>
        /// 产品信息服务组件
        /// </summary>
        IProductListServer m_serverProductList = ServerModuleFactory.GetServerModule<IProductListServer>();

        /// <summary>
        /// 检查某业务是否存在
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">业务号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.S_ProductPlan
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
            string sql = "SELECT * FROM [DepotManagement].[dbo].[S_ProductPlan] where DJH = '" + billNo + "'";

            System.Data.DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 获得父表信息
        /// </summary>
        /// <param name="planType">计划类别</param>
        /// <returns>返回产品计划信息</returns>
        public DataTable GetAllBill(string planType)
        {
            string strSql = "select * from View_S_ProductPlan where 计划类别 = '" + planType + "' order by 计划日期  desc";
            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dt;
        }

        /// <summary>
        /// 获得子表信息
        /// </summary>
        /// <param name="djh">计划单号</param>
        /// <returns>返回明细表信息</returns>
        public DataTable GetList(string djh)
        {
            string strSql = "select * from View_S_ProductPlanList where DJH = '" + djh + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dt;
        }

        /// <summary>
        /// 添加父表信息
        /// </summary>
        /// <param name="inProductPlan">生产计划信息</param>
        /// <param name="planList">明细信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>添加成功返回True，添加失败返回False</returns>
        public bool AddBill(S_ProductPlan inProductPlan,DataTable planList, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                if (!CheckDJH(inProductPlan.DJH))
                {
                    if (!UpdateBill(ctx, inProductPlan, planList, out error))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!DeletePlanList(ctx, inProductPlan.DJH, out error))
                    {
                        return false;
                    }

                    if (!AddPlanList(ctx, planList, inProductPlan.DJH, out error))
                    {
                        return false;
                    }

                    S_ProductPlan lnqPlan = new S_ProductPlan();

                    lnqPlan.DJH = inProductPlan.DJH;
                    lnqPlan.PlanTime = Convert.ToDateTime(inProductPlan.PlanTime.Value.ToShortDateString());
                    lnqPlan.DJZT = inProductPlan.PlanType == "月计划" ? "等待采购计划批准" : "";
                    lnqPlan.BZRY = BasicInfo.LoginID;
                    lnqPlan.BZRQ = ServerTime.Time;
                    lnqPlan.PlanType = inProductPlan.PlanType;
                    lnqPlan.Remark = inProductPlan.Remark;

                    ctx.S_ProductPlan.InsertOnSubmit(lnqPlan);
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
        /// <param name="djh">计划单号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        private bool DeletePlanList(DepotManagementDataContext context, string djh, out string error)
        {
            error = null;

            try
            {
                var varData = from a in context.S_ProductPlanList
                              where a.DJH == djh
                              select a;

                context.S_ProductPlanList.DeleteAllOnSubmit(varData);
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;

            }

            return true;
        }

        /// <summary>
        /// 添加子表信息
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="planList">明细表信息</param>
        /// <param name="billNo">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>添加成功返回True，添加失败返回False</returns>
        bool AddPlanList(DepotManagementDataContext context,DataTable planList, string billNo, out string error)
        {
            error = null;

            try
            {
                List<S_ProductPlanList> lisPlan = new List<S_ProductPlanList>();

                for (int i = 0; i < planList.Rows.Count; i++)
                {
                    S_ProductPlanList lnqPlanList = new S_ProductPlanList();

                    lnqPlanList.DJH = billNo;
                    lnqPlanList.GoodsID = Convert.ToInt32(planList.Rows[i]["物品ID"]);
                    lnqPlanList.Count = Convert.ToDecimal(planList.Rows[i]["数量"]);
                    lnqPlanList.ScheduleType = planList.Rows[i]["类型"].ToString();

                    lisPlan.Add(lnqPlanList);
                }

                context.S_ProductPlanList.InsertAllOnSubmit(lisPlan);
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
                throw;
            }

            return true;
        }

        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="djh">计划单号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        public bool DeleteBill(string djh,out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.S_ProductPlan
                              where a.DJH == djh
                              select a;

                if (varData.Count() == 1)
                {
                    S_ProductPlan lnqPdPlan = varData.Single();

                    if (DeletePlanList(ctx, djh, out error))
                    {
                        ctx.S_ProductPlan.DeleteOnSubmit(lnqPdPlan);
                        ctx.SubmitChanges();
                    }
                    else
                    {
                        return false;
                    }
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
                throw;
            }

            return true;
        }

        /// <summary>
        /// 检查计划日期是否重复
        /// </summary>
        /// <param name="newDateTime">被检测的日期</param>
        /// <param name="planType">单据类型</param>
        /// <returns>重复返回True，不重复返回False</returns>
        public bool IsRepeatPlanDate(DateTime newDateTime, string planType)
        {
            string strSql = "select * from S_ProductPlan where PlanType = '"+ planType +"' and PlanTime = '" + newDateTime + "'";
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
        /// 检查单据是否重复
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <returns>不重复返回True，重复返回False</returns>
        public bool CheckDJH(string djh)
        {
            string strSql = "select * from S_ProductPlan where DJH = '" + djh + "'";
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
        /// <param name="inPdPlan">生产计划信息</param>
        /// <param name="planList">明细信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>修改成功返回True，修改失败返回False</returns>
        bool UpdateBill(DepotManagementDataContext ctx,S_ProductPlan inPdPlan, DataTable planList, out string error)
        {
            error = null;

            try
            {
                var varData = from a in ctx.S_ProductPlan
                              where a.DJH == inPdPlan.DJH
                              select a;

                if (varData.Count() == 1)
                {
                    if (!DeletePlanList(ctx, inPdPlan.DJH, out error))
                    {
                        return false;
                    }

                    if (!AddPlanList(ctx, planList, inPdPlan.DJH, out error))
                    {
                        return false;
                    }

                    S_ProductPlan lnqPlan = varData.Single();

                    lnqPlan.DJZT = inPdPlan.PlanType == "月计划" ? "等待采购计划批准" : "";
                    lnqPlan.BZRY = BasicInfo.LoginID;
                    lnqPlan.BZRQ = ServerTime.Time;
                    lnqPlan.PlanType = inPdPlan.PlanType;
                    lnqPlan.Remark = inPdPlan.Remark;

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
    }
}
