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

namespace Service_Economic_Purchase
{
    class PartsBelongProviderChangeService : IPartsBelongProviderChangeService
    {

        /// <summary>
        /// 获得合格供应商与零件责任归属信息
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回LNQ信息</returns>
        public B_AccessoryDutyInfo GetDutyInfo(int goodsID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            return GetDutyInfo(ctx, goodsID);
        }

        /// <summary>
        /// 获得合格供应商与零件责任归属信息
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回LNQ信息</returns>
        B_AccessoryDutyInfo GetDutyInfo(DepotManagementDataContext ctx, int goodsID)
        {
            var varData = from a in ctx.B_AccessoryDutyInfo
                          where a.GoodsID == goodsID
                          select a;

            if (varData.Count() == 1)
            {
                return varData.Single();
            }
            else if (varData.Count() == 0)
            {
                return null;
            }
            else
            {
                throw new Exception("获取【合格供应商与零件归属信息】失败");
            }
        }

        /// <summary>
        /// 获得单条业务总单信息
        /// </summary>
        /// <param name="billNo">业务号</param>
        /// <returns>返回业务总单信息</returns>
        public Business_PurchasingMG_PartsBelongPriovderChange GetSingleBillInfo(string billNo)
        {
            if (billNo == null)
            {
                return null;
            }

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Business_PurchasingMG_PartsBelongPriovderChange
                          where a.BillNo == billNo
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
        /// 获得业务明细信息
        /// </summary>
        /// <param name="billNo">业务号</param>
        /// <returns>返回业务明细列表</returns>
        public List<View_Business_PurchasingMG_PartsBelongPriovderChangeDetail> GetListViewDetailInfo(string billNo)
        {
            if (billNo == null)
            {
                return null;
            }

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.View_Business_PurchasingMG_PartsBelongPriovderChangeDetail
                          where a.单据号 == billNo
                          select a;

            return varData.ToList();
        }

        /// <summary>
        /// 保存业务信息
        /// </summary>
        /// <param name="billInfo">业务总单信息</param>
        /// <param name="detailInfo">业务明细信息</param>
        public void SaveInfo(Business_PurchasingMG_PartsBelongPriovderChange billInfo,
            List<View_Business_PurchasingMG_PartsBelongPriovderChangeDetail> detailInfo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();

            try
            {
                var varData = from a in ctx.Business_PurchasingMG_PartsBelongPriovderChange
                              where a.BillNo == billInfo.BillNo
                              select a;

                if (varData.Count() == 1)
                {
                    Business_PurchasingMG_PartsBelongPriovderChange lnqBill = varData.Single();

                    lnqBill.ChangeReason = billInfo.ChangeReason;
                    lnqBill.ChangeType = billInfo.ChangeType;
                    lnqBill.Provider = billInfo.Provider;
                }
                else if (varData.Count() == 0)
                {
                    ctx.Business_PurchasingMG_PartsBelongPriovderChange.InsertOnSubmit(billInfo);
                }
                else
                {
                    throw new Exception("单据数据不唯一");
                }

                var varDetail = from a in ctx.Business_PurchasingMG_PartsBelongPriovderChangeDetail
                                where a.BillNo == billInfo.BillNo
                                select a;

                ctx.Business_PurchasingMG_PartsBelongPriovderChangeDetail.DeleteAllOnSubmit(varDetail);
                ctx.SubmitChanges();

                foreach (View_Business_PurchasingMG_PartsBelongPriovderChangeDetail item in detailInfo)
                {
                    Business_PurchasingMG_PartsBelongPriovderChangeDetail lnqDetail 
                        = new Business_PurchasingMG_PartsBelongPriovderChangeDetail();

                    lnqDetail.BillNo = billInfo.BillNo;
                    lnqDetail.DiffcultyLV = item.难度等级;
                    lnqDetail.Explain = item.说明;
                    lnqDetail.GoodsID = item.物品ID;
                    lnqDetail.PartType = item.零件类型;
                    lnqDetail.ProviderLV = item.供应商等级;

                    ctx.Business_PurchasingMG_PartsBelongPriovderChangeDetail.InsertOnSubmit(lnqDetail);
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

        void SetProviderLV(ref B_AccessoryDutyInfo duty, CE_LV lv, List<string> lstProvider)
        {
            if (duty == null)
            {
                throw new Exception("数据异常");
            }

            switch (lv)
            {
                case CE_LV.A:

                    duty.ProviderA = "";

                    if (lstProvider == null || lstProvider.Count == 0)
                    {
                        return;
                    }

                    foreach (string A in lstProvider)
                    {
                        duty.ProviderA += A + ",";
                    }

                    duty.ProviderA = duty.ProviderA.Substring(0, duty.ProviderA.Length - 1);

                    break;
                case CE_LV.B:

                    duty.ProviderB = "";

                    if (lstProvider == null || lstProvider.Count == 0)
                    {
                        return;
                    }

                    foreach (string B in lstProvider)
                    {
                        duty.ProviderB += B + ",";
                    }

                    duty.ProviderB = duty.ProviderB.Substring(0, duty.ProviderB.Length - 1);
                    break;
                case CE_LV.C:

                    duty.ProviderC = "";

                    if (lstProvider == null || lstProvider.Count == 0)
                    {
                        return;
                    }

                    foreach (string C in lstProvider)
                    {
                        duty.ProviderC += C + ",";
                    }

                    duty.ProviderC = duty.ProviderC.Substring(0, duty.ProviderC.Length - 1);
                    break;
                default:
                    break;
            }
        }

        List<string> GetListProviderLV(B_AccessoryDutyInfo duty, CE_LV lv)
        {
            List<string> lstResult = new List<string>();

            switch (lv)
            {
                case CE_LV.A:

                    if (duty.ProviderA != null && duty.ProviderA.Trim().Length > 0)
                    {
                        lstResult = duty.ProviderA.Split(',').ToList();
                    }
                    break;
                case CE_LV.B:
                    if (duty.ProviderB != null && duty.ProviderB.Trim().Length > 0)
                    {
                        lstResult = duty.ProviderB.Split(',').ToList();
                    }
                    break;
                case CE_LV.C:
                    if (duty.ProviderC != null && duty.ProviderC.Trim().Length > 0)
                    {
                        lstResult = duty.ProviderC.Split(',').ToList();
                    }
                    break;
                default:
                    break;
            }

            return lstResult;
        }

        public string GetProviderLV(int goodsID, string provider)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.B_AccessoryDutyInfo
                          where a.GoodsID == goodsID
                          select a;

            if (varData.Count() == 0)
            {
                return null;
            }

            B_AccessoryDutyInfo duty = varData.Single();

            List<string> LVA = new List<string>();
            List<string> LVB = new List<string>();
            List<string> LVC = new List<string>();

            if (duty.ProviderA != null && duty.ProviderA.Trim().Length > 0)
            {
                LVA = duty.ProviderA.Split(',').ToList();
            }

            if (duty.ProviderB != null && duty.ProviderB.Trim().Length > 0)
            {
                LVB = duty.ProviderB.Split(',').ToList();
            }

            if (duty.ProviderC != null && duty.ProviderC.Trim().Length > 0)
            {
                LVC = duty.ProviderC.Split(',').ToList();
            }

            if (LVA.Contains(provider))
            {
                return "A";
            }
            else if (LVB.Contains(provider))
            {
                return "B";
            }
            else if (LVC.Contains(provider))
            {
                return "C";
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 结束业务
        /// </summary>
        /// <param name="billNo">业务编号</param>
        public void FinishBill(string billNo)
        {
            IFlowServer serviceFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();

            string billStatus = serviceFlow.GetNextBillStatus(billNo);

            if (billStatus == null)
            {
                throw new Exception("单据状态为空，请重新确认");
            }

            if (billStatus != CE_CommonBillStatus.单据完成.ToString())
            {
                return;
            }

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();

            try
            {
                Business_PurchasingMG_PartsBelongPriovderChange billInfo = GetSingleBillInfo(billNo);

                if (billInfo == null || billInfo.BillNo.Length == 0)
                {
                    throw new Exception("此单据不存在");
                }

                List<View_Business_PurchasingMG_PartsBelongPriovderChangeDetail> listDetail = GetListViewDetailInfo(billInfo.BillNo);

                foreach (View_Business_PurchasingMG_PartsBelongPriovderChangeDetail item in listDetail)
                {
                    CE_LV providerLV = GlobalObject.GeneralFunction.StringConvertToEnum<CE_LV>(item.供应商等级);
                    B_AccessoryDutyInfo lnqTemp = GetDutyInfo(ctx, item.物品ID);

                    List<string> LVA = new List<string>();
                    List<string> LVB = new List<string>();
                    List<string> LVC = new List<string>();

                    switch (billInfo.ChangeType)
                    {
                        case "准入":
                            if (lnqTemp == null)
                            {
                                lnqTemp = new B_AccessoryDutyInfo();

                                List<CommonProcessInfo> lstTemp = serviceFlow.GetFlowData(billInfo.BillNo);

                                lnqTemp.CreatePersonnel = (lstTemp == null || lstTemp.Count() == 0) ? BasicInfo.LoginID :
                                    serviceFlow.GetFlowData(billInfo.BillNo).OrderBy(k => k.时间).First().人员;
                                lnqTemp.CreateDate = ServerTime.Time;
                                lnqTemp.Grade = item.难度等级;
                                lnqTemp.Remark = item.说明;
                                lnqTemp.Sort = item.零件类型;
                                lnqTemp.GoodsID = item.物品ID;

                                List<string> lstProvider = new List<string>();
                                lstProvider.Add(billInfo.Provider);
                                SetProviderLV(ref lnqTemp, providerLV, lstProvider);

                                ctx.B_AccessoryDutyInfo.InsertOnSubmit(lnqTemp);
                            }
                            else if (lnqTemp != null)
                            {
                                LVA = GetListProviderLV(lnqTemp, CE_LV.A);
                                LVB = GetListProviderLV(lnqTemp, CE_LV.B);
                                LVC = GetListProviderLV(lnqTemp, CE_LV.C);

                                LVA.Remove(billInfo.Provider);
                                LVB.Remove(billInfo.Provider);
                                LVC.Remove(billInfo.Provider);

                                switch (providerLV)
                                {
                                    case CE_LV.A:
                                        LVA.Add(billInfo.Provider);
                                        break;
                                    case CE_LV.B:
                                        LVB.Add(billInfo.Provider);
                                        break;
                                    case CE_LV.C:
                                        LVC.Add(billInfo.Provider);
                                        break;
                                    default:
                                        break;
                                }

                                SetProviderLV(ref lnqTemp, CE_LV.A, LVA);
                                SetProviderLV(ref lnqTemp, CE_LV.B, LVB);
                                SetProviderLV(ref lnqTemp, CE_LV.C, LVC);
                            }
                            else
                            {
                                throw new Exception("信息重复，无法操作");
                            }

                            break;
                        case "等级变更":

                            if (lnqTemp == null)
                            {
                                throw new Exception("无此零件信息，无法操作【等级变更】");
                            }

                            LVA = GetListProviderLV(lnqTemp, CE_LV.A);
                            LVB = GetListProviderLV(lnqTemp, CE_LV.B);
                            LVC = GetListProviderLV(lnqTemp, CE_LV.C);

                            LVA.Remove(billInfo.Provider);
                            LVB.Remove(billInfo.Provider);
                            LVC.Remove(billInfo.Provider);

                            switch (providerLV)
                            {
                                case CE_LV.A:
                                    LVA.Add(billInfo.Provider);
                                    break;
                                case CE_LV.B:
                                    LVB.Add(billInfo.Provider);
                                    break;
                                case CE_LV.C:
                                    LVC.Add(billInfo.Provider);
                                    break;
                                default:
                                    break;
                            }

                            SetProviderLV(ref lnqTemp, CE_LV.A, LVA);
                            SetProviderLV(ref lnqTemp, CE_LV.B, LVB);
                            SetProviderLV(ref lnqTemp, CE_LV.C, LVC);
                            break;
                        case "淘汰":
                            if (lnqTemp != null)
                            {
                                LVA = GetListProviderLV(lnqTemp, CE_LV.A);
                                LVB = GetListProviderLV(lnqTemp, CE_LV.B);
                                LVC = GetListProviderLV(lnqTemp, CE_LV.C);

                                LVA.Remove(billInfo.Provider);
                                LVB.Remove(billInfo.Provider);
                                LVC.Remove(billInfo.Provider);

                                SetProviderLV(ref lnqTemp, CE_LV.A, LVA);
                                SetProviderLV(ref lnqTemp, CE_LV.B, LVB);
                                SetProviderLV(ref lnqTemp, CE_LV.C, LVC);

                                if ((lnqTemp.ProviderA == null || lnqTemp.ProviderA.Length == 0)
                                    && (lnqTemp.ProviderB == null || lnqTemp.ProviderB.Length == 0)
                                    && (lnqTemp.ProviderC == null || lnqTemp.ProviderC.Length == 0))
                                {
                                    ctx.B_AccessoryDutyInfo.DeleteOnSubmit(lnqTemp);
                                }
                            }

                            break;
                        default:
                            break;
                    }

                    ctx.SubmitChanges();
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
        /// 删除业务
        /// </summary>
        /// <param name="billNo">业务号</param>
        public void DeleteInfo(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();

            IFlowServer serverFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();
            BillNumberControl billNoControl = new BillNumberControl(CE_BillTypeEnum.供应商与零件归属变更单.ToString(), this);

            try
            {
                var varData = from a in ctx.Business_PurchasingMG_PartsBelongPriovderChange
                              where a.BillNo == billNo
                              select a;

                ctx.Business_PurchasingMG_PartsBelongPriovderChange.DeleteAllOnSubmit(varData);
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
        /// 检查某业务是否存在
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">业务号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.Business_PurchasingMG_PartsBelongPriovderChange
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
        /// 检查某业务是否存在
        /// </summary>
        /// <param name="billNo">业务号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Business_PurchasingMG_PartsBelongPriovderChange
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
    }
}
