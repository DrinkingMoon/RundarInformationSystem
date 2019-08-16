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

namespace Service_Quality_QC
{
    class EightDReport : IEightDReport
    {
        /// <summary>
        /// 流程服务组件
        /// </summary>
        IFlowServer _serviceFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();

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
            BillNumberControl billNoControl = new BillNumberControl(CE_BillTypeEnum.纠正预防措施报告.ToString(), this);

            try
            {
                var varData = from a in ctx.Bus_Quality_8DReport
                              where a.BillNo == billNo
                              select a;

                //删除附件
                if (varData.Count() == 1)
                {
                    Bus_Quality_8DReport reportInfo = varData.Single();

                    if (!GeneralFunction.IsNullOrEmpty(reportInfo.D1_DescribePhenomenon_Accessory))
                    {
                        foreach (string fileItem in reportInfo.D1_DescribePhenomenon_Accessory.ToString().Split(','))
                        {
                            UniversalControlLibrary.FileOperationService.File_Delete(new Guid(fileItem),
                                GeneralFunction.StringConvertToEnum<CE_CommunicationMode>(BasicInfo.BaseSwitchInfo[(int)CE_SwitchName.文件传输方式]));
                        }
                    }

                    if (!GeneralFunction.IsNullOrEmpty(reportInfo.D5_ReasonAnalysis_Accessory))
                    {
                        foreach (string fileItem in reportInfo.D5_ReasonAnalysis_Accessory.ToString().Split(','))
                        {
                            UniversalControlLibrary.FileOperationService.File_Delete(new Guid(fileItem),
                                GeneralFunction.StringConvertToEnum<CE_CommunicationMode>(BasicInfo.BaseSwitchInfo[(int)CE_SwitchName.文件传输方式]));
                        }
                    }
                }


                ctx.Bus_Quality_8DReport.DeleteAllOnSubmit(varData);
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
            var varData = from a in ctx.Bus_Quality_8DReport
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

            var varData = from a in ctx.Bus_Quality_8DReport
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
        /// 主表赋值
        /// </summary>
        /// <param name="billInfo">界面数据</param>
        /// <param name="tempInfo">逻辑数据</param>
        void AssignmentValue(Bus_Quality_8DReport billInfo, int flowID, ref Bus_Quality_8DReport tempInfo)
        {
            switch (flowID)
            {
                case 1109:
                    tempInfo.GoodsInfo = billInfo.GoodsInfo;
                    tempInfo.HappenDate = billInfo.HappenDate;
                    tempInfo.HappenSite = billInfo.HappenSite;
                    tempInfo.Involve = billInfo.Involve;
                    tempInfo.Theme = billInfo.Theme;
                    tempInfo.Badness = billInfo.Badness;
                    tempInfo.BatchNo = billInfo.BatchNo;
                    tempInfo.BillNo = billInfo.BillNo;
                    tempInfo.D1_DescribePhenomenon = billInfo.D1_DescribePhenomenon;
                    tempInfo.D1_DescribePhenomenon_Accessory = billInfo.D1_DescribePhenomenon_Accessory;
                    tempInfo.D1_DutyDepartment = billInfo.D1_DutyDepartment;
                    tempInfo.D1_InfluenceElseProduct_Explain = billInfo.D1_InfluenceElseProduct_Explain;
                    tempInfo.D1_LastHappenTime = billInfo.D1_LastHappenTime;
                    break;
                case 1111:
                    tempInfo.D2_DutyPersonnel = billInfo.D2_DutyPersonnel;
                    break;
                case 1112:
                    tempInfo.D3_Reason_Incoming = billInfo.D3_Reason_Incoming;
                    tempInfo.D3_Reason_Producted = billInfo.D3_Reason_Producted;
                    tempInfo.D3_Reason_Production = billInfo.D3_Reason_Production;
                    tempInfo.D3_Reason_Send = billInfo.D3_Reason_Send;
                    tempInfo.D4_Else_Measure = billInfo.D4_Else_Measure;
                    tempInfo.D4_Else_NG = billInfo.D4_Else_NG;
                    tempInfo.D4_Else_OK = billInfo.D4_Else_OK;
                    tempInfo.D4_FinishClient_Measure = billInfo.D4_FinishClient_Measure;
                    tempInfo.D4_FinishClient_NG = billInfo.D4_FinishClient_NG;
                    tempInfo.D4_FinishClient_OK = billInfo.D4_FinishClient_OK;
                    tempInfo.D4_FinishCom_Measure = billInfo.D4_FinishCom_Measure;
                    tempInfo.D4_FinishCom_NG = billInfo.D4_FinishCom_NG;
                    tempInfo.D4_FinishCom_OK = billInfo.D4_FinishCom_OK;
                    tempInfo.D4_HowIdentify = billInfo.D4_HowIdentify;
                    tempInfo.D4_Loading_Measure = billInfo.D4_Loading_Measure;
                    tempInfo.D4_Loading_NG = billInfo.D4_Loading_NG;
                    tempInfo.D4_Loading_OK = billInfo.D4_Loading_OK;
                    tempInfo.D4_Semi_Measure = billInfo.D4_Semi_Measure;
                    tempInfo.D4_Semi_NG = billInfo.D4_Semi_NG;
                    tempInfo.D4_Semi_OK = billInfo.D4_Semi_OK;
                    tempInfo.D5_ReasonAnalysis = billInfo.D5_ReasonAnalysis;
                    tempInfo.D5_ReasonAnalysis_Accessory = billInfo.D5_ReasonAnalysis_Accessory;
                    break;
                case 1114:
                    tempInfo.D8_Unfulfillment_Explain = billInfo.D8_Unfulfillment_Explain;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 保存业务信息
        /// </summary>
        /// <param name="billInfo">业务总单信息</param>
        public void SaveInfo(Bus_Quality_8DReport billInfo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();

            try
            {
                Flow_FlowInfo info =
                    _serviceFlow.GetNowFlowInfo(_serviceFlow.GetBusinessTypeID(CE_BillTypeEnum.纠正预防措施报告, null),
                    billInfo.BillNo);

                Bus_Quality_8DReport tempInfo = new Bus_Quality_8DReport();

                var varData = from a in ctx.Bus_Quality_8DReport
                              where a.BillNo == billInfo.BillNo
                              select a;

                if (varData.Count() == 1)
                {
                    tempInfo = varData.Single();
                    AssignmentValue(billInfo, info.FlowID, ref tempInfo);
                }
                else if (varData.Count() == 0)
                {
                    tempInfo.F_Id = Guid.NewGuid().ToString();
                    AssignmentValue(billInfo, info.FlowID, ref tempInfo);
                    ctx.Bus_Quality_8DReport.InsertOnSubmit(tempInfo);
                }
                else
                {
                    throw new Exception("单据数据不唯一");
                }

                ctx.SubmitChanges();

                switch (info.FlowID)
                {
                    case 1112:
                        //D2
                        var varData1 = from a in ctx.Bus_Quality_8DReport_D2_Crew
                                       where a.ReportId == tempInfo.F_Id
                                       select a;

                        ctx.Bus_Quality_8DReport_D2_Crew.DeleteAllOnSubmit(varData1);

                        foreach (Bus_Quality_8DReport_D2_Crew item in billInfo.Bus_Quality_8DReport_D2_Crew.ToList())
                        {
                            Bus_Quality_8DReport_D2_Crew tempItem = new Bus_Quality_8DReport_D2_Crew();

                            tempItem.F_Id = Guid.NewGuid().ToString();
                            tempItem.ReportId = tempInfo.F_Id;
                            tempItem.PersonnelWorkId = item.PersonnelWorkId;

                            ctx.Bus_Quality_8DReport_D2_Crew.InsertOnSubmit(tempItem);
                        }

                        ctx.SubmitChanges();

                        //D5
                        var varData2 = from a in ctx.Bus_Quality_8DReport_D5_Reason
                                       where a.ReportId == tempInfo.F_Id
                                       select a;

                        ctx.Bus_Quality_8DReport_D5_Reason.DeleteAllOnSubmit(varData2);

                        foreach (Bus_Quality_8DReport_D5_Reason item in billInfo.Bus_Quality_8DReport_D5_Reason.ToList())
                        {
                            Bus_Quality_8DReport_D5_Reason tempItem = new Bus_Quality_8DReport_D5_Reason();

                            tempItem.F_Id = Guid.NewGuid().ToString();
                            tempItem.ReportId = tempInfo.F_Id;
                            tempItem.Description = item.Description;
                            tempItem.ReasonType = item.ReasonType;

                            ctx.Bus_Quality_8DReport_D5_Reason.InsertOnSubmit(tempItem);
                        }

                        ctx.SubmitChanges();

                        //D6
                        var varData3 = from a in ctx.Bus_Quality_8DReport_D6_Countermeasure
                                       where a.ReportId == tempInfo.F_Id
                                       select a;

                        ctx.Bus_Quality_8DReport_D6_Countermeasure.DeleteAllOnSubmit(varData3);

                        foreach (Bus_Quality_8DReport_D6_Countermeasure item in billInfo.Bus_Quality_8DReport_D6_Countermeasure.ToList())
                        {
                            Bus_Quality_8DReport_D6_Countermeasure tempItem = new Bus_Quality_8DReport_D6_Countermeasure();

                            tempItem.F_Id = Guid.NewGuid().ToString();
                            tempItem.ReportId = tempInfo.F_Id;
                            tempItem.Description = item.Description;
                            tempItem.CountermeasureType = item.CountermeasureType;
                            tempItem.Duty = item.Duty;
                            tempItem.FinishDate = item.FinishDate;

                            ctx.Bus_Quality_8DReport_D6_Countermeasure.InsertOnSubmit(tempItem);
                        }

                        ctx.SubmitChanges();

                        //D7
                        var varData4 = from a in ctx.Bus_Quality_8DReport_D7_Prevent
                                       where a.ReportId == tempInfo.F_Id
                                       select a;

                        ctx.Bus_Quality_8DReport_D7_Prevent.DeleteAllOnSubmit(varData4);

                        foreach (Bus_Quality_8DReport_D7_Prevent item in billInfo.Bus_Quality_8DReport_D7_Prevent.ToList())
                        {
                            Bus_Quality_8DReport_D7_Prevent tempItem = new Bus_Quality_8DReport_D7_Prevent();

                            tempItem.F_Id = Guid.NewGuid().ToString();
                            tempItem.ReportId = tempInfo.F_Id;
                            tempItem.Department = item.Department;
                            tempItem.ItemsType = item.ItemsType;
                            tempItem.OperationTime = item.OperationTime;

                            ctx.Bus_Quality_8DReport_D7_Prevent.InsertOnSubmit(tempItem);
                        }

                        ctx.SubmitChanges();
                        break;
                    case 1114:
                        //D8
                        var varData5 = from a in ctx.Bus_Quality_8DReport_D8_Verify
                                       where a.ReportId == tempInfo.F_Id
                                       select a;

                        ctx.Bus_Quality_8DReport_D8_Verify.DeleteAllOnSubmit(varData5);

                        foreach (Bus_Quality_8DReport_D8_Verify item in billInfo.Bus_Quality_8DReport_D8_Verify.ToList())
                        {
                            Bus_Quality_8DReport_D8_Verify tempItem = new Bus_Quality_8DReport_D8_Verify();

                            tempItem.F_Id = Guid.NewGuid().ToString();
                            tempItem.ReportId = tempInfo.F_Id;
                            tempItem.OperationDate = item.OperationDate;
                            tempItem.Result = item.Result;
                            tempItem.WayMode = item.WayMode;
                            tempItem.Duty = item.Duty;
                            tempItem.Effect = item.Effect;

                            ctx.Bus_Quality_8DReport_D8_Verify.InsertOnSubmit(tempItem);
                        }

                        ctx.SubmitChanges();
                        break;
                    default:
                        break;
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
        /// 获得单条业务总单信息
        /// </summary>
        /// <param name="billNo">业务号</param>
        /// <returns>返回业务总单信息</returns>
        public Bus_Quality_8DReport GetSingleBillInfo(string billNo)
        {
            if (billNo == null)
            {
                return null;
            }

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Bus_Quality_8DReport
                          where a.BillNo == billNo
                          select a;

            if (varData.Count() == 1)
            {
                Bus_Quality_8DReport resultInfo = varData.Single();

                resultInfo.Bus_Quality_8DReport_D2_Crew.SetSource((from a in ctx.Bus_Quality_8DReport_D2_Crew
                                                                   where a.ReportId == resultInfo.F_Id
                                                                   select a).AsEnumerable());

                resultInfo.Bus_Quality_8DReport_D5_Reason.SetSource((from a in ctx.Bus_Quality_8DReport_D5_Reason
                                                                   where a.ReportId == resultInfo.F_Id
                                                                   select a).AsEnumerable());

                resultInfo.Bus_Quality_8DReport_D6_Countermeasure.SetSource((from a in ctx.Bus_Quality_8DReport_D6_Countermeasure
                                                                   where a.ReportId == resultInfo.F_Id
                                                                   select a).AsEnumerable());

                resultInfo.Bus_Quality_8DReport_D7_Prevent.SetSource((from a in ctx.Bus_Quality_8DReport_D7_Prevent
                                                                   where a.ReportId == resultInfo.F_Id
                                                                      select a).AsEnumerable());

                resultInfo.Bus_Quality_8DReport_D8_Verify.SetSource((from a in ctx.Bus_Quality_8DReport_D8_Verify
                                                                      where a.ReportId == resultInfo.F_Id
                                                                      select a).AsEnumerable());

                return resultInfo;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 更新文件路径
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="guid">文件编号集字符串</param>
        /// <param name="address">上传文件存放位置</param>
        public void UpdateFilePath(string billNo, string guid, string address)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Bus_Quality_8DReport
                          where a.BillNo == billNo
                          select a;

            if (varData.Count() == 1)
            {
                if (address == "D1")
                {
                    varData.Single().D1_DescribePhenomenon_Accessory = guid;
                }
                else if(address == "D5")
                {
                    varData.Single().D5_ReasonAnalysis_Accessory = guid;
                }
            }

            ctx.SubmitChanges();
        }
    }
}
