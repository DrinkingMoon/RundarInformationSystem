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

namespace Service_Project_Design
{
    /// <summary>
    /// 物料录入申请单服务
    /// </summary>
    class GoodsEnteringBill : IGoodsEnteringBill
    {
        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="billNo">单号</param>
        /// <returns>成功返回True,否则False</returns>
        public void DeleteInfo(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();

            IFlowServer serverFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();
            BillNumberControl billNoControl = new BillNumberControl(CE_BillTypeEnum.物料录入申请单.ToString(), this);

            try
            {
                var varData = from a in ctx.S_GoodsEnteringBill
                              where a.BillNo == billNo
                              select a;

                ctx.S_GoodsEnteringBill.DeleteAllOnSubmit(varData);
                ctx.SubmitChanges();
                serverFlow.FlowDelete(ctx, billNo);

                ctx.Transaction.Commit();
                billNoControl.CancelBill(billNo);
            }
            catch (Exception ex)
            {
                ctx.Transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 明细编辑
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="listInfo">明细信息列表</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,否则False</returns>
        public bool EditListInfo(string billNo, List<View_S_GoodsEnteringBill> listInfo, out string error)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            error = null;

            try
            {
                var varData = from a in ctx.S_GoodsEnteringBill
                              where a.BillNo == billNo
                              select a;

                ctx.S_GoodsEnteringBill.DeleteAllOnSubmit(varData);
                ctx.SubmitChanges();

                foreach (View_S_GoodsEnteringBill item in listInfo)
                {
                    S_GoodsEnteringBill templnq = new S_GoodsEnteringBill();

                    templnq.BillNo = billNo;
                    templnq.Depot = item.材料类别编码;
                    templnq.GoodsCode = item.图号型号;
                    templnq.GoodsName = item.物品名称;
                    templnq.Remark = item.备注;
                    templnq.Spec = item.规格;
                    templnq.Remark = item.备注;

                    string strUnitID = UniversalFunction.GetUnitID(item.单位);

                    templnq.UnitID = strUnitID == "" ? null : (int?)Convert.ToInt32(strUnitID);

                    ctx.S_GoodsEnteringBill.InsertOnSubmit(templnq);
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
        /// 明细编辑
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">单据号</param>
        /// <param name="listInfo">明细信息列表</param>
        void EditListInfo(DepotManagementDataContext ctx, string billNo, List<View_S_GoodsEnteringBill> listInfo)
        {
            try
            {
                var varData = from a in ctx.S_GoodsEnteringBill
                              where a.BillNo == billNo
                              select a;

                ctx.S_GoodsEnteringBill.DeleteAllOnSubmit(varData);
                ctx.SubmitChanges();

                foreach (View_S_GoodsEnteringBill item in listInfo)
                {
                    S_GoodsEnteringBill templnq = new S_GoodsEnteringBill();

                    templnq.BillNo = billNo;
                    templnq.Depot = item.材料类别编码;
                    templnq.GoodsCode = item.图号型号;
                    templnq.GoodsName = item.物品名称;
                    templnq.Remark = item.备注;
                    templnq.Spec = item.规格;
                    templnq.Remark = item.备注;

                    string strUnitID = UniversalFunction.GetUnitID(item.单位);

                    templnq.UnitID = strUnitID == "" ? null : (int?)Convert.ToInt32(strUnitID);

                    ctx.S_GoodsEnteringBill.InsertOnSubmit(templnq);
                }

                ctx.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 提交单据
        /// </summary>
        /// <param name="billNo">单号</param>
        /// <param name="listInfo">明细信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,否则False</returns>
        public bool SubmitInfo(string billNo, System.Collections.Generic.List<View_S_GoodsEnteringBill> listInfo, out string error)
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

            error = null;

            try
            {
                EditListInfo(ctx, billNo, listInfo);

                if (billStatus == GoodsEnteringBillStatus.单据完成.ToString())
                {
                    foreach (View_S_GoodsEnteringBill item in listInfo)
                    {
                        F_GoodsPlanCost tempGoodsInfo = new F_GoodsPlanCost();

                        tempGoodsInfo.Date = ServerTime.Time;
                        tempGoodsInfo.GoodsCode = item.图号型号;
                        tempGoodsInfo.GoodsName = item.物品名称;

                        if (item.材料类别编码 == null || item.材料类别编码.Trim().Length == 0)
                        {
                            throw new Exception("材料类别不能为空");
                        }

                        tempGoodsInfo.GoodsType = item.材料类别编码;
                        tempGoodsInfo.GoodsUnitPrice = 0;
                        tempGoodsInfo.IsDisable = false;
                        tempGoodsInfo.PY = ctx.Fun_get_bm(item.物品名称, 1);
                        tempGoodsInfo.WB = ctx.Fun_get_bm(item.物品名称, 0);
                        tempGoodsInfo.Remark = item.备注;
                        tempGoodsInfo.Spec = item.规格;
                        tempGoodsInfo.UnitID = item.单位 == "" ? 1 : Convert.ToInt32(UniversalFunction.GetUnitID(item.单位));

                        List<Flow_FlowData> tempList = serverFlow.GetBusinessOperationInfo(billNo, GoodsEnteringBillStatus.新建单据.ToString());

                        if (tempList == null)
                        {
                            throw new Exception("获取操作人员失败");
                        }

                        tempGoodsInfo.UserCode = tempList[0].OperationPersonnel;

                        ctx.F_GoodsPlanCost.InsertOnSubmit(tempGoodsInfo);
                    }
                }

                ctx.SubmitChanges();
                ctx.Transaction.Commit();

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                ctx.Transaction.Rollback();
                return false;
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
            var varData = from a in ctx.S_GoodsEnteringBill
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

            var varData = from a in ctx.S_GoodsEnteringBill
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
        public List<View_S_GoodsEnteringBill> GetListInfo(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.View_S_GoodsEnteringBill
                          where a.单据号 == billNo
                          select a;

            return varData.ToList();
        }

        /// <summary>
        /// 检测物品信息
        /// </summary>
        /// <param name="goodsInfo">物品信息</param>
        /// <returns>存在返回True,否则返回False</returns>
        public void CheckGoodsInfo(View_S_GoodsEnteringBill goodsInfo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.F_GoodsPlanCost
                          where a.GoodsCode == goodsInfo.图号型号
                          && a.GoodsName == goodsInfo.物品名称
                          && a.Spec == goodsInfo.规格
                          select a;

            if (varData.Count() > 0)
            {
                throw new Exception("存在相同物品，无法录入");
            }
            else
            {
                if (goodsInfo.图号型号.Length > 0)
                {
                    varData = from a in ctx.F_GoodsPlanCost
                              where a.GoodsCode == goodsInfo.图号型号
                              && a.IsDisable == false
                              select a;

                    if (varData.Count() > 0)
                    {
                        foreach (var item in varData)
                        {
                            if (item.GoodsName == goodsInfo.物品名称)
                            {
                                return;
                            }
                        }

                        throw new Exception("存在相同【图号型号】的物品必须【物品名称】相同，如需录入请用【规格】区分，否则无法录入");
                    }
                }
            }
        }
    }
}
