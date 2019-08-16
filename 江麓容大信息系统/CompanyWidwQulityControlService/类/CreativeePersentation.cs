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
    class CreativeePersentation : ICreativeePersentation
    {
        public void UpdateFilePath(string billNo, string fileNo, SelfSimpleEnum_CreativeePersentation simple)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Business_CWQC_CreativePersentation
                          where a.BillNo == billNo
                          select a;

            if (varData.Count() == 1)
            {
                Business_CWQC_CreativePersentation tempInfo = varData.Single();

                if (simple == SelfSimpleEnum_CreativeePersentation.Before)
                {
                    tempInfo.ImproveConditions_Before_FileNo = fileNo;
                }
                else if (simple == SelfSimpleEnum_CreativeePersentation.After)
                {
                    tempInfo.ImproveConditions_After_FileNo = fileNo;
                }
            }

            ctx.SubmitChanges();
        }

        /// <summary>
        /// 获得参考信息
        /// </summary>
        /// <param name="type">类型信息</param>
        /// <returns>返回Table</returns>
        public DataTable GetReferenceInfo(string type)
        {
            string strSql = "select * from Business_CWQC_CreativePersentation_" + type;

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得单条业务总单信息
        /// </summary>
        /// <param name="billNo">业务号</param>
        /// <returns>返回业务总单信息</returns>
        public Business_CWQC_CreativePersentation GetSingleBillInfo(string billNo)
        {
            if (billNo == null)
            {
                return null;
            }

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Business_CWQC_CreativePersentation
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
        public void SaveInfo(Business_CWQC_CreativePersentation billInfo, ServerModule.Business_CWQC_CreativePersentation_EffectValue effectValue)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();

            try
            {
                var varData = from a in ctx.Business_CWQC_CreativePersentation
                              where a.BillNo == billInfo.BillNo
                              select a;

                if (varData.Count() == 1)
                {
                    Business_CWQC_CreativePersentation lnqBill = varData.Single();

                    lnqBill.BillNo = billInfo.BillNo;
                    lnqBill.Propose = billInfo.Propose;
                    lnqBill.ExtensionCoverage = billInfo.ExtensionCoverage;
                    lnqBill.FGLD_Abstract = billInfo.FGLD_Abstract;
                    lnqBill.FGLD_Apply = billInfo.FGLD_Apply;
                    lnqBill.FGLD_Economy = billInfo.FGLD_Economy;
                    lnqBill.FGLD_Ideas = billInfo.FGLD_Ideas;
                    lnqBill.FGLD_Strive = billInfo.FGLD_Strive;
                    lnqBill.FZR_Abstract = billInfo.FZR_Abstract;
                    lnqBill.FZR_Apply = billInfo.FZR_Apply;
                    lnqBill.FZR_Economy = billInfo.FZR_Economy;
                    lnqBill.FZR_Ideas = billInfo.FZR_Ideas;
                    lnqBill.FZR_Strive = billInfo.FZR_Strive;
                    lnqBill.ImproveConditions_After = billInfo.ImproveConditions_After;
                    lnqBill.ImproveConditions_After_FileNo = billInfo.ImproveConditions_After_FileNo;
                    lnqBill.ImproveConditions_Before = billInfo.ImproveConditions_Before;
                    lnqBill.ImproveConditions_Before_FileNo = billInfo.ImproveConditions_Before_FileNo;
                    lnqBill.ImproveEndDate = billInfo.ImproveEndDate;
                    lnqBill.ImproveStartDate = billInfo.ImproveStartDate;
                    lnqBill.ProposalType = billInfo.ProposalType;
                    lnqBill.Task = billInfo.Task;
                    lnqBill.ValueEffect = billInfo.ValueEffect;
                    lnqBill.Level = billInfo.Level;
                    lnqBill.SumScore = billInfo.SumScore;
                    lnqBill.Bonus = billInfo.Bonus;

                }
                else if (varData.Count() == 0)
                {
                    ctx.Business_CWQC_CreativePersentation.InsertOnSubmit(billInfo);
                }
                else
                {
                    throw new Exception("单据数据不唯一");
                }

                OperationInfo_EffectValue(ctx, effectValue);
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
            BillNumberControl billNoControl = new BillNumberControl(CE_BillTypeEnum.创意提案.ToString(), this);

            try
            {
                var varData = from a in ctx.Business_CWQC_CreativePersentation
                              where a.BillNo == billNo
                              select a;


                if (varData.Count() == 1)
                {
                    Business_CWQC_CreativePersentation tempInfo = varData.Single();

                    if (tempInfo.ImproveConditions_After_FileNo != null && tempInfo.ImproveConditions_After_FileNo.Trim().Length > 0)
                    {
                        string[] pathArray = tempInfo.ImproveConditions_After_FileNo.Split(',');

                        for (int i = 0; i < pathArray.Length; i++)
                        {
                            UniversalControlLibrary.FileOperationService.File_Delete(new Guid(pathArray[i]),
                                GlobalObject.GeneralFunction.StringConvertToEnum<CE_CommunicationMode>(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.文件传输方式]));
                        }
                    }

                    if (tempInfo.ImproveConditions_Before_FileNo != null && tempInfo.ImproveConditions_Before_FileNo.Trim().Length > 0)
                    {
                        string[] pathArray = tempInfo.ImproveConditions_Before_FileNo.Split(',');

                        for (int i = 0; i < pathArray.Length; i++)
                        {
                            UniversalControlLibrary.FileOperationService.File_Delete(new Guid(pathArray[i]),
                                GlobalObject.GeneralFunction.StringConvertToEnum<CE_CommunicationMode>(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.文件传输方式]));
                        }
                    }
                }

                ctx.Business_CWQC_CreativePersentation.DeleteAllOnSubmit(varData);
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
            var varData = from a in ctx.Business_CWQC_CreativePersentation
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

            var varData = from a in ctx.Business_CWQC_CreativePersentation
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

        public void DirectAdd(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Flow_FlowBillData
                          where a.BillNo == billNo
                          select a;

            if (varData.Count() != 1)
            {
                throw new Exception("数据不唯一");
            }

            Flow_FlowBillData tempLnq = varData.Single();

            tempLnq.FlowID = 67;
            ctx.SubmitChanges();

            IBillMessagePromulgatorServer msgPromulgator = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();
            msgPromulgator.BillType = CE_BillTypeEnum.创意提案.ToString();
            msgPromulgator.EndFlowMessage(billNo, "直接录入", null, null);
        }

        public Business_CWQC_CreativePersentation_EffectValue GetInfo_EffectValue(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            return GetInfo_EffectValue(ctx, billNo);
        }

        Business_CWQC_CreativePersentation_EffectValue GetInfo_EffectValue(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.Business_CWQC_CreativePersentation_EffectValue
                          where a.BillNo == billNo
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
                throw new Exception("数据不唯一");
            }
        }

        void OperationInfo_EffectValue(DepotManagementDataContext ctx, Business_CWQC_CreativePersentation_EffectValue effectValue)
        {
            Business_CWQC_CreativePersentation_EffectValue lnqInfo = GetInfo_EffectValue(ctx, effectValue.BillNo);

            if (lnqInfo == null)
            {
                ctx.Business_CWQC_CreativePersentation_EffectValue.InsertOnSubmit(effectValue);
            }
            else
            {
                lnqInfo.ElseContent = effectValue.ElseContent;
                lnqInfo.ElseEffectValue = effectValue.ElseEffectValue;
                lnqInfo.MaterialReduce1 = effectValue.MaterialReduce1;
                lnqInfo.MaterialReduce2 = effectValue.MaterialReduce2;
                lnqInfo.MaterialReduce3 = effectValue.MaterialReduce3;
                lnqInfo.MaterialReduce4 = effectValue.MaterialReduce4;
                lnqInfo.WorkReduce1 = effectValue.WorkReduce1;
                lnqInfo.WorkReduce2 = effectValue.WorkReduce2;
                lnqInfo.WorkReduce3 = effectValue.WorkReduce3;
                lnqInfo.WorkReduce4 = effectValue.WorkReduce4;
            }

            ctx.SubmitChanges();
        }
    }
}
