/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  ProductOrder.cs
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
using ServerModule;
using FlowControlService;

namespace Service_Manufacture_Storage
{
    /// <summary>
    /// 领用总成清单管理类
    /// </summary>
    class ProductOrder : IProductOrder
    {
        /// <summary>
        /// 删除发料清单
        /// </summary>
        /// <param name="billNo">单号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,否则False</returns>
        public void DeleteInfo(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();

            IFlowServer serviceFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();
            BillNumberControl billNoControl = new BillNumberControl(CE_BillTypeEnum.发料清单.ToString(), this);

            try
            {
                var varData = from a in ctx.S_DebitSchedule
                              where a.BillNo == billNo
                              select a;

                ctx.S_DebitSchedule.DeleteAllOnSubmit(varData);
                ctx.SubmitChanges();
                serviceFlow.FlowDelete(ctx, billNo);

                ctx.Transaction.Commit();
                billNoControl.CancelBill(billNo);
            }
            catch (Exception ex)
            {
                ctx.Transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }

        void JudgeAssembly(DepotManagementDataContext ctx, List<View_S_DebitSchedule> listInfo, string edition, CE_DebitScheduleApplicable applicable)
        {
            if (applicable == CE_DebitScheduleApplicable.售后返修)
            {
                edition += " FX";
            }

            bool isZero = true;

            foreach (View_S_DebitSchedule item in listInfo)
            {
                if (item.基数 > 0)
                {
                    isZero = false;
                    break;
                }
            }

            if (isZero)
            {
                var varData = from a in ctx.P_AssemblingBom
                              where a.ProductCode == edition
                              select a;

                if (varData.Count() > 0)
                {
                    throw new Exception("【总成】：" + edition + " 存在于【装配BOM】中，请先清除此总成的【装配BOM】再删除");
                }
            }
        }

        /// <summary>
        /// 操作
        /// </summary>
        /// <param name="listInfo">清单明细</param>
        /// <param name="billNo">单号</param>
        /// <param name="edition">总成型号</param>
        /// <param name="applicable">适用范围</param>
        public void OperationInfo(List<View_S_DebitSchedule> listInfo, string billNo, string edition, CE_DebitScheduleApplicable applicable)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();

            try
            {
                IFlowServer serverFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();

                if (serverFlow.GetNextBillStatus(billNo) == ProductOrderListStatus.单据完成.ToString())
                {
                    var varOrder = from a in ctx.BASE_ProductOrder
                                   where a.Edition == edition && a.Applicable == applicable.ToString()
                                   select a;

                    JudgeAssembly(ctx, listInfo, edition, applicable);
                    ctx.BASE_ProductOrder.DeleteAllOnSubmit(varOrder);
                    ctx.SubmitChanges();

                    int intOrder = 0;
                    foreach (View_S_DebitSchedule item in listInfo)
                    {
                        if (item.基数 > 0)
                        {
                            BASE_ProductOrder order = new BASE_ProductOrder();

                            order.Edition = edition;
                            order.Applicable = applicable.ToString();
                            order.GoodsID = item.物品ID;
                            order.Redices = item.基数;
                            order.OnceTheWholeIssue = item.一次性整台份发料;
                            order.Position = ++intOrder;

                            ctx.BASE_ProductOrder.InsertOnSubmit(order);
                        }
                    }

                    ctx.SubmitChanges();
                }

                ctx.Transaction.Commit();
            }
            catch (Exception ex)
            {
                ctx.Transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 提交发料清单
        /// </summary>
        /// <param name="listInfo">清单明细</param>
        /// <param name="billNo">单号</param>
        /// <param name="edition">总成型号</param>
        /// <param name="applicable">适用范围</param>
        public void SaveInfo(List<View_S_DebitSchedule> listInfo, string billNo, string edition, CE_DebitScheduleApplicable applicable)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();

            IFlowServer serverFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();
            string billStatus = serverFlow.GetNextBillStatus(billNo);

            if (billStatus == null)
            {
                throw new Exception("单据状态为空，请重新确认");
            }

            try
            {
                var varData = from a in ctx.S_DebitSchedule
                              where a.BillNo == billNo
                              select a;

                ctx.S_DebitSchedule.DeleteAllOnSubmit(varData);
                ctx.SubmitChanges();

                foreach (View_S_DebitSchedule item in listInfo)
                {
                    S_DebitSchedule templnq = new S_DebitSchedule();

                    templnq.BillNo = billNo;
                    templnq.Edition = edition;
                    templnq.Applicable = applicable.ToString();
                    templnq.GoodsID = item.物品ID;
                    templnq.Redices = item.基数;
                    templnq.OnceTheWholeIssue = item.一次性整台份发料;

                    ctx.S_DebitSchedule.InsertOnSubmit(templnq);
                }

                ctx.SubmitChanges();
                ctx.Transaction.Commit();
            }
            catch (Exception ex)
            {
                ctx.Transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 检查某业务是否存在
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">业务号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.S_DebitSchedule
                          where a.BillNo == billNo
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
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.S_DebitSchedule
                          where a.BillNo == billNo
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
        /// 获得明细信息列表
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回List</returns>
        public List<View_S_DebitSchedule> GetListInfo(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.View_S_DebitSchedule
                          where a.单据号 == billNo
                          orderby a.ID
                          select a;

            return varData.ToList();
        }

        /// <summary>
        /// 根据所属总成获得其领料清单
        /// </summary>
        /// <param name="code">产品型号</param>
        /// <param name="applicable">适用范围</param>
        /// <returns>返回领料清单信息</returns>
        public DataTable GetAllData(string code, CE_DebitScheduleApplicable applicable)
        {
            string strSql = "select * from View_BASE_ProductOrder where 产品编码 = '" + code + "' and 适用范围 = '"+ applicable.ToString() +"' order by 顺序位置";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获取整包发料的物品列表
        /// </summary>
        /// <param name="productCode">产品编码</param>
        /// <param name="applicable">适用范围</param>
        /// <returns>返回获取到的数据</returns>
        public List<BASE_ProductOrder> GetPackGoodsList(string productCode, CE_DebitScheduleApplicable applicable)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.BASE_ProductOrder
                         where r.Edition == productCode 
                         && r.Applicable == applicable.ToString()
                         && (from b in dataContxt.F_GoodsAttributeRecord
                                                            where b.AttributeID == (int)CE_GoodsAttributeName.整包发料
                                                            select b.GoodsID).Contains(r.GoodsID)
                         select r;

            return result.ToList();
        }

        /// <summary>
        /// 获取指定产品的排序数据
        /// </summary>
        /// <param name="fetchType">领料类型</param>
        /// <param name="productCode">产品编码</param>
        /// <param name="applicable">适用范围</param>
        /// <param name="isDeletePackGoods">是否剔除整包发料的物品</param>
        /// <returns>返回获取到的数据</returns>
        public List<BASE_ProductOrder> GetAllDataList(FetchGoodsType fetchType, string productCode, 
            CE_DebitScheduleApplicable applicable, bool isDeletePackGoods)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.BASE_ProductOrder
                          where r.Edition == productCode && r.Applicable == applicable.ToString()
                          select r;

            if (fetchType == FetchGoodsType.整台领料不含后补充)
            {
                result = from r in dataContxt.BASE_ProductOrder
                         where r.Edition == productCode && r.Applicable == applicable.ToString()
                         && r.OnceTheWholeIssue == true
                         select r;
            }


            if (isDeletePackGoods)
            {
                result = from a in result
                         where !(from b in dataContxt.F_GoodsAttributeRecord
                                 where b.AttributeID == (int)CE_GoodsAttributeName.整包发料
                                 select b.GoodsID).Contains(a.GoodsID)
                         select a;
            }

            return result.OrderBy(k => k.Position).ToList();
        }
        
        /// <summary>
        /// 删除领用总成清单信息
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="code">产品型号</param>
        /// <param name="applicable">适用范围</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        private bool DeleteDate(DepotManagementDataContext context, string code, CE_DebitScheduleApplicable applicable, out string error)
        {
            error = null;

            try
            {
                var varData = from a in context.BASE_ProductOrder
                              where a.Edition == code && a.Applicable == applicable.ToString()
                              select a;

                context.BASE_ProductOrder.DeleteAllOnSubmit(varData);
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
        /// 添加领料清单信息
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="order">添加的数据信息</param>
        /// <param name="applicable">适用范围</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>添加成功返回True，添加失败返回False</returns>
        private bool InsertDate(DepotManagementDataContext context, DataTable order, CE_DebitScheduleApplicable applicable, out string error)
        {
            error = null;

            try
            {
                List<BASE_ProductOrder> lisOrder = new List<BASE_ProductOrder>();

                if (order.Rows.Count == 0)
                {
                    return true;
                }
                else
                {

                    for (int i = 0; i < order.Rows.Count; i++)
                    {
                        BASE_ProductOrder lnqOrder = new BASE_ProductOrder();

                        lnqOrder.Edition = order.Rows[i]["产品编码"].ToString();
                        lnqOrder.Applicable = applicable.ToString();
                        lnqOrder.GoodsID = Convert.ToInt32( order.Rows[i]["物品ID"]);
                        lnqOrder.Redices = Convert.ToDecimal(order.Rows[i]["基数"].ToString());
                        lnqOrder.Position = Convert.ToInt32(order.Rows[i]["顺序位置"].ToString());
                        lnqOrder.OnceTheWholeIssue = Convert.ToBoolean(order.Rows[i]["一次性整台份发料"]);

                        lisOrder.Add(lnqOrder);
                    }

                    context.BASE_ProductOrder.InsertAllOnSubmit(lisOrder);
                }

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
        /// 保存领料清单设置
        /// </summary>
        /// <param name="code">产品型号</param>
        /// <param name="order">需要保存的信息</param>
        /// <param name="applicable">适用范围</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>保存成功返回True，保存失败返回False</returns>
        public bool SaveDate(string code, DataTable order, CE_DebitScheduleApplicable applicable, out string error)
        {
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;
            dataContxt.Connection.Open();
            dataContxt.Transaction = dataContxt.Connection.BeginTransaction();

            try
            {
                if (DeleteDate(dataContxt,code, applicable, out error))
                {
                    if (!InsertDate(dataContxt,order, applicable, out error))
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }

                dataContxt.Transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                dataContxt.Transaction.Rollback();
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 获取装配信息
        /// </summary>
        /// <param name="productCode">总成编码</param>
        /// <param name="applicable">适用范围</param>
        /// <returns>成功返回数据集失败返回错误信息</returns>
        public DataTable GetProductOrder(string productCode, CE_DebitScheduleApplicable applicable)
        {
            string sql = "select 零件编码,零件名称,规格 from View_BASE_ProductOrder" +
                         " where 产品编码='" + productCode + "'and  适用范围 = '" + applicable.ToString() + "' order by 顺序位置";

            return  GlobalObject.DatabaseServer.QueryInfo(sql);
        }

        public int GetPosition(DepotManagementDataContext ctx, string productType, int goodsID)
        {
            var varData = from a in ctx.BASE_ProductOrder
                          where a.Edition == productType
                          && a.GoodsID == goodsID
                          select a;

            if (varData.Count() == 0)
            {
                throw new Exception(UniversalFunction.GetGoodsMessage(ctx, goodsID) + "不存在于【发料清单】中");
            }
            else
            {
                return varData.First().Position;
            }
        }
    }
}
