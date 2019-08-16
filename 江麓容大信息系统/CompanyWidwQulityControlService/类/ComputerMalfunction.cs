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

namespace Service_Peripheral_CompanyQuality
{
    class ComputerMalfunction : Service_Peripheral_CompanyQuality.IComputerMalfunction
    {
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
            BillNumberControl billNoControl = new BillNumberControl(CE_BillTypeEnum.IT运维申请单.ToString(), this);

            try
            {

                var varData = from a in ctx.Business_Composite_ComputerMalfunction
                              where a.BillNo == billNo
                              select a;

                ctx.Business_Composite_ComputerMalfunction.DeleteAllOnSubmit(varData);
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
            var varData = from a in ctx.Business_Composite_ComputerMalfunction
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
            return IsExist(ctx, billNo);
        }

        /// <summary>
        /// 获得单条业务总单信息
        /// </summary>
        /// <param name="billNo">业务号</param>
        /// <returns>返回业务总单信息</returns>
        public Business_Composite_ComputerMalfunction GetSingleBillInfo(string billNo)
        {
            if (billNo == null)
            {
                return null;
            }

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Business_Composite_ComputerMalfunction
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
        /// 保存业务信息
        /// </summary>
        /// <param name="billInfo">业务总单信息</param>
        /// <param name="effectValue">业务明细信息</param>
        public void SaveInfo(Business_Composite_ComputerMalfunction billInfo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();
            IFlowServer serviceFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();

            try
            {
                var varData = from a in ctx.Business_Composite_ComputerMalfunction
                              where a.BillNo == billInfo.BillNo
                              select a;

                if (varData.Count() == 1)
                {
                    Business_Composite_ComputerMalfunction lnqBill = varData.Single();

                    Flow_FlowInfo flowInfo = serviceFlow.GetNowFlowInfo(lnqBill.BillNo);
                    CE_CommonFlowName flowName =
                        GlobalObject.GeneralFunction.StringConvertToEnum<CE_CommonFlowName>(flowInfo.FlowName);

                    switch (flowName)
                    {
                        case CE_CommonFlowName.新建:
                            lnqBill.MalfunctionAddress = billInfo.MalfunctionAddress;
                            lnqBill.MalfunctionSymptom = billInfo.MalfunctionSymptom;
                            lnqBill.AssetsNo = billInfo.AssetsNo;
                            lnqBill.Contact = billInfo.Contact;

                            if (lnqBill.MalfunctionAddress == null || lnqBill.MalfunctionAddress.Trim().Length == 0)
                            {
                                throw new Exception("请填写【故障发生地点】");
                            }

                            if (lnqBill.MalfunctionSymptom == null || lnqBill.MalfunctionSymptom.Trim().Length == 0)
                            {
                                throw new Exception("请填写【故障现象】");
                            }

                            if (lnqBill.Contact == null || lnqBill.Contact.Trim().Length == 0)
                            {
                                throw new Exception("请填写【联系方式】");
                            }

                            PointEvaluation(ctx, billInfo.BillNo);
                            break;
                        case CE_CommonFlowName.处理:
                            lnqBill.SymptomType = billInfo.SymptomType;
                            lnqBill.MalfunctionReason = billInfo.MalfunctionReason;
                            lnqBill.MalfunctionApproach = billInfo.MalfunctionApproach;
                            lnqBill.ElapsedTime = billInfo.ElapsedTime;
                            lnqBill.Expenses = billInfo.Expenses;


                            if (lnqBill.SymptomType == null || lnqBill.SymptomType.Trim().Length == 0)
                            {
                                throw new Exception("请填写【故障类别】");
                            }

                            if (lnqBill.MalfunctionReason == null || lnqBill.MalfunctionReason.Trim().Length == 0)
                            {
                                throw new Exception("请填写【故障原因】");
                            }

                            if (lnqBill.MalfunctionApproach == null || lnqBill.MalfunctionApproach.Trim().Length == 0)
                            {
                                throw new Exception("请填写【处理方式】");
                            }

                            break;
                        case CE_CommonFlowName.评价:
                            lnqBill.Evaluation = billInfo.Evaluation;
                            lnqBill.Solve = billInfo.Solve;
                            lnqBill.Satisfaction = billInfo.Satisfaction;

                            if (lnqBill.Evaluation == null || lnqBill.Evaluation.Trim().Length == 0)
                            {
                                throw new Exception("请填写【评价】");
                            }

                            if (lnqBill.Satisfaction == null || lnqBill.Satisfaction.Value == 0)
                            {
                                throw new Exception("请填写【满意度】");
                            }

                            break;
                        case CE_CommonFlowName.完成:
                            break;
                        default:
                            break;
                    }
                }
                else if (varData.Count() == 0)
                {
                    if (billInfo.MalfunctionAddress == null || billInfo.MalfunctionAddress.Trim().Length == 0)
                    {
                        throw new Exception("请填写【故障发生地点】");
                    }

                    if (billInfo.MalfunctionSymptom == null || billInfo.MalfunctionSymptom.Trim().Length == 0)
                    {
                        throw new Exception("请填写【故障现象】");
                    }

                    if (billInfo.Contact == null || billInfo.Contact.Trim().Length == 0)
                    {
                        throw new Exception("请填写【联系方式】");
                    }

                    ctx.Business_Composite_ComputerMalfunction.InsertOnSubmit(billInfo);
                    PointEvaluation(ctx, billInfo.BillNo);
                }
                else
                {
                    throw new Exception("单据数据不唯一");
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

        void PointEvaluation(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.Flow_FlowExecuteInfoPersonnel
                          where a.BillNo == billNo
                          select a;

            ctx.Flow_FlowExecuteInfoPersonnel.DeleteAllOnSubmit(varData);

            Flow_FlowExecuteInfoPersonnel personnel = new Flow_FlowExecuteInfoPersonnel();

            personnel.BillNo = billNo;
            personnel.FlowID = 77;
            personnel.WorkID = BasicInfo.LoginID;

            ctx.Flow_FlowExecuteInfoPersonnel.InsertOnSubmit(personnel);
            ctx.SubmitChanges();
        }

    }
}
