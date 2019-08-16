/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  QualityProblemRectificationDisposal.cs
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
    /// 质量问题整改处置单管理类
    /// </summary>
    class QualityProblemRectificationDisposalBill : BasicServer, IQualityProblemRectificationDisposalBill
    {
        /// <summary>
        /// 获得单据号
        /// </summary>
        /// <param name="scheduleType">表类型:质量整改处置单,试验验证计划表,新品开发计划表,搭车分析计划表</param>
        /// <returns>返回单据号</returns>
        public string GetBillID(string scheduleType)
        {
            string strGaugeOutfit = "";

            string strTableName = "";

            switch (scheduleType)
            {
                case "质量整改处置单":
                    strGaugeOutfit = "ZGD";
                    strTableName = "ZL_QualityProblemRectificationDisposalBill";
                    break;
                case "试验验证计划表":
                    strGaugeOutfit = "SYB";
                    strTableName = "ZL_ExperimentsSchedule";
                    break;
                case "新品开发计划表":
                    strGaugeOutfit = "XKB";
                    strTableName = "ZL_NewProductDevelopmentSchedule";
                    break;
                case "搭车分析计划表":
                    strGaugeOutfit = "DFB";
                    strTableName = "ZL_AssemblingAnalysisSchedule";
                    break;
                default:
                    break;
            }

            string strSql = "select Max(Bill_ID) from " + strTableName;

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dt.Rows[0][0].ToString() == "")
            {
                return strGaugeOutfit + ServerTime.Time.Year.ToString("D4") + ServerTime.Time.Month.ToString("D2") + "0001";
            }
            else
            {
                string strBillID = dt.Rows[0][0].ToString();

                int intID = Convert.ToInt32(strBillID.Substring(9, 4)) + 1;

                if (ServerTime.Time.Year == Convert.ToInt32(strBillID.Substring(3, 4)))
                {
                    return strGaugeOutfit + ServerTime.Time.Year.ToString("D4") + ServerTime.Time.Month.ToString("D2") + intID.ToString("D4");
                }
                else
                {
                    return strGaugeOutfit + ServerTime.Time.Year.ToString("D4") + ServerTime.Time.Month.ToString("D2") + "0001";
                }

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
            var varData = from a in ctx.ZL_QualityProblemRectificationDisposalBill
                          where a.Bill_ID == billNo
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
            string sql = "SELECT * FROM [DepotManagement].[dbo].[ZL_QualityProblemRectificationDisposalBill] where Bill_ID = '" + billNo + "'";

            System.Data.DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 获得某一条新品开发计划表明细信息
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <returns>返回一条明细信息</returns>
        public ZL_NewProductDevelopmentSchedule GetNewProductDevelopmentMessage(string billID)
        {
            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            var varOneMessage = from a in dataContext.ZL_NewProductDevelopmentSchedule
                                where a.Bill_ID == billID
                                select a;

            if (varOneMessage.Count() != 1)
            {
                return null;
            }
            else
            {
                return varOneMessage.Single();
            }
        }

        /// <summary>
        /// 获得某一条搭车分析计划表明细信息
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <returns>返回一条明细信息</returns>
        public ZL_AssemblingAnalysisSchedule GetAssemblingAnalysisMessage(string billID)
        {

            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            var varOneMessage = from a in dataContext.ZL_AssemblingAnalysisSchedule
                                where a.Bill_ID == billID
                                select a;

            if (varOneMessage.Count() != 1)
            {
                return null;
            }
            else
            {
                return varOneMessage.Single();
            }
        }

        /// <summary>
        /// 获得某一条试验验证计划表明细信息
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <returns>返回一条明细信息</returns>
        public ZL_ExperimentsSchedule GetExperimentsMessage(string billID)
        {
            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            var varOneMessage = from a in dataContext.ZL_ExperimentsSchedule
                                where a.Bill_ID == billID
                                select a;

            if (varOneMessage.Count() != 1)
            {
                return null;
            }
            else
            {
                return varOneMessage.Single();
            }
        }

        /// <summary>
        /// 获得某一条质量问题整改处置单明细信息
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <returns>返回一条明细信息</returns>
        public ZL_QualityProblemRectificationDisposalBill GetQualityProblemMessage(string billID)
        {
            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            var varOneMessage = from a in dataContext.ZL_QualityProblemRectificationDisposalBill
                                where a.Bill_ID == billID
                                select a;

            if (varOneMessage.Count() != 1)
            {
                return null;
            }
            else
            {
                return varOneMessage.Single();
            }
        }

        /// <summary>
        /// 获得记录集
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="billStatus">单据状态</param>
        /// <returns>返回获得的记录集</returns>
        public DataTable GetAllBill(DateTime startTime,DateTime endTime, string billStatus)
        {
            string strSql = "select * from View_ZL_QualityProblemRectificationDisposalBill where 1=1";

            if (billStatus != "全  部")
            {
                strSql += " and 单据状态 = '"+ billStatus +"'";
            }

            strSql += " and 编制时间 >= '" + startTime + "' and 编制时间 <= '" + endTime + "' order by 单据号 Desc";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 插入新数据
        /// </summary>
        /// <param name="qualityProblem">数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool InsertBill(ZL_QualityProblemRectificationDisposalBill qualityProblem, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varBill = from a in dataContext.ZL_QualityProblemRectificationDisposalBill
                              where a.Bill_ID == qualityProblem.Bill_ID
                              select a;

                if (varBill.Count() != 0)
                {
                    error = "数据不唯一";
                    return false;
                }
                else
                {
                    ZL_QualityProblemRectificationDisposalBill lnqQualityProblem = new ZL_QualityProblemRectificationDisposalBill();

                    lnqQualityProblem.BillStatus = "新建单据";
                    lnqQualityProblem.Bill_ID = qualityProblem.Bill_ID;
                    lnqQualityProblem.HappenFillInDate = ServerTime.Time;
                    lnqQualityProblem.HappenFillInPersonnel = BasicInfo.LoginName;
                    lnqQualityProblem.HappenDate = qualityProblem.HappenDate;
                    lnqQualityProblem.HappenFrequency = qualityProblem.HappenFrequency;
                    lnqQualityProblem.HappenPlace = qualityProblem.HappenPlace;
                    lnqQualityProblem.GoodsCode = qualityProblem.GoodsCode;
                    lnqQualityProblem.GoodsName = qualityProblem.GoodsName;
                    lnqQualityProblem.BatchNoOrSpec = qualityProblem.BatchNoOrSpec;
                    lnqQualityProblem.Provider = qualityProblem.Provider;
                    lnqQualityProblem.ProblemsDescription = qualityProblem.ProblemsDescription;
                    lnqQualityProblem.Severity = qualityProblem.Severity;

                    dataContext.ZL_QualityProblemRectificationDisposalBill.InsertOnSubmit(lnqQualityProblem);
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
        /// 保存信息
        /// </summary>
        /// <param name="qualityProblem">数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool SaveInfo(ZL_QualityProblemRectificationDisposalBill qualityProblem, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varBill = from a in dataContext.ZL_QualityProblemRectificationDisposalBill
                              where a.Bill_ID == qualityProblem.Bill_ID
                              select a;

                if (varBill.Count() == 0)
                {
                    if (!InsertBill(qualityProblem, out error))
                    {
                        return false;
                    }
                }
                else if (varBill.Count() > 1)
                {
                    error = "数据不唯一";
                    return false;
                }
                else
                {
                    ZL_QualityProblemRectificationDisposalBill lnqQualityProblem = varBill.Single();

                    if (qualityProblem.BillStatus != lnqQualityProblem.BillStatus)
                    {
                        error = "单据状态错误，请重新刷新单据确认单据状态";
                        return false;
                    }

                    switch (lnqQualityProblem.BillStatus)
                    {
                        case "新建单据":

                            lnqQualityProblem.HappenFillInDate = ServerTime.Time;
                            lnqQualityProblem.HappenFillInPersonnel = BasicInfo.LoginName;
                            lnqQualityProblem.HappenDate = qualityProblem.HappenDate;
                            lnqQualityProblem.HappenFrequency = qualityProblem.HappenFrequency;
                            lnqQualityProblem.HappenPlace = qualityProblem.HappenPlace;
                            lnqQualityProblem.GoodsCode = qualityProblem.GoodsCode;
                            lnqQualityProblem.GoodsName = qualityProblem.GoodsName;
                            lnqQualityProblem.BatchNoOrSpec = qualityProblem.BatchNoOrSpec;
                            lnqQualityProblem.Provider = qualityProblem.Provider;
                            lnqQualityProblem.ProblemsDescription = qualityProblem.ProblemsDescription;
                            lnqQualityProblem.Severity = qualityProblem.Severity;
                            break;
                        case "等待发起部门确认":

                            lnqQualityProblem.HappenAffirmPersonnel = BasicInfo.LoginName;
                            lnqQualityProblem.HappenAffirmDate = ServerTime.Time;
                            break;
                        case "等待分析判定":
                            lnqQualityProblem.InterimMeasure = qualityProblem.InterimMeasure;
                            lnqQualityProblem.InterimMeasureFile = qualityProblem.InterimMeasureFile;
                            lnqQualityProblem.InterimMeasureIsNeedTesting = qualityProblem.InterimMeasureIsNeedTesting;
                            lnqQualityProblem.InterimMeasureTestingID = qualityProblem.InterimMeasureTestingID;
                            lnqQualityProblem.AnalyseAndJudge = qualityProblem.AnalyseAndJudge;
                            lnqQualityProblem.AnalyseAndJudgeAffirmID = qualityProblem.AnalyseAndJudgeAffirmID;
                            lnqQualityProblem.AnalyseAndJudgeFile = qualityProblem.AnalyseAndJudgeFile;
                            lnqQualityProblem.AnalyseAndJudgeIsNeedAffirm = qualityProblem.AnalyseAndJudgeIsNeedAffirm;
                            lnqQualityProblem.RelevantDepartment = qualityProblem.RelevantDepartment;
                            lnqQualityProblem.RequiredTime = qualityProblem.RequiredTime;
                            lnqQualityProblem.QualityFillInPersonnel = BasicInfo.LoginName;
                            lnqQualityProblem.QualityFillInDate = ServerTime.Time;
                            break;
                        case "等待分析判定确认":
                            lnqQualityProblem.QualityAffirmDate = ServerTime.Time;
                            lnqQualityProblem.QualityAffirmPersonnel = BasicInfo.LoginName;
                            break;
                        case "等待指定责任人":
                            lnqQualityProblem.Responsible = qualityProblem.Responsible;

                            break;
                        case "等待整改措施":
                            lnqQualityProblem.RelevantFillInPersonnel = BasicInfo.LoginName;
                            lnqQualityProblem.RelevantFillInDate = ServerTime.Time;
                            lnqQualityProblem.CauseAnalysis = qualityProblem.CauseAnalysis;
                            lnqQualityProblem.CauseAnalysisFile = qualityProblem.CauseAnalysisFile;
                            lnqQualityProblem.CauseAnalysisIsNeedTesting = qualityProblem.CauseAnalysisIsNeedTesting;
                            lnqQualityProblem.CauseAnalysisTestingID = qualityProblem.CauseAnalysisTestingID;
                            lnqQualityProblem.RectificationMeasures = qualityProblem.RectificationMeasures;
                            lnqQualityProblem.RectificationMeasuresChangeID = qualityProblem.RectificationMeasuresChangeID;
                            lnqQualityProblem.RectificationMeasuresFile = qualityProblem.RectificationMeasuresFile;
                            lnqQualityProblem.RectificationMeasuresIsNeedChange = qualityProblem.RectificationMeasuresIsNeedChange;

                            break;
                        case "等待整改措施确认":
                            lnqQualityProblem.RelevantAffirmPersonnel = BasicInfo.LoginName;
                            lnqQualityProblem.RelevantAffirmDate = ServerTime.Time;

                            break;
                        case "等待效果确认":
                            lnqQualityProblem.EffectConfirmed = qualityProblem.EffectConfirmed;
                            lnqQualityProblem.EffectAffirmDate = ServerTime.Time;
                            lnqQualityProblem.EffectAffirmPersonnel = BasicInfo.LoginName;
                            break;
                        case "等待最终确认":
                            lnqQualityProblem.LaunchDepartmentAffirm = qualityProblem.LaunchDepartmentAffirm;
                            lnqQualityProblem.LaunchDepartmentAffirmDate = ServerTime.Time;
                            lnqQualityProblem.LaunchDepartmentAffirmPersonnel = BasicInfo.LoginName;

                            break;
                        default:
                            break;
                    }

                    dataContext.SubmitChanges();
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
        /// 流程管理
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool UpdateBill(string billNo, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varBill = from a in dataContext.ZL_QualityProblemRectificationDisposalBill
                              where a.Bill_ID == billNo
                              select a;

                if (varBill.Count() != 1)
                {
                    error = "数据不唯一";
                    return false;
                }
                else
                {
                    ZL_QualityProblemRectificationDisposalBill lnqQualityProblem = varBill.Single();

                    switch (lnqQualityProblem.BillStatus)
                    {
                        case "新建单据":
                            lnqQualityProblem.BillStatus = "等待发起部门确认";
                            lnqQualityProblem.HappenFillInDate = ServerTime.Time;
                            lnqQualityProblem.HappenFillInPersonnel = BasicInfo.LoginName;
                            break;

                        case "等待发起部门确认":
                            lnqQualityProblem.HappenAffirmPersonnel = BasicInfo.LoginName;
                            lnqQualityProblem.HappenAffirmDate = ServerTime.Time;
                            lnqQualityProblem.BillStatus = "等待分析判定";

                            break;
                        case "等待分析判定":
                            lnqQualityProblem.BillStatus = "等待分析判定确认";
                            lnqQualityProblem.QualityFillInPersonnel = BasicInfo.LoginName;
                            lnqQualityProblem.QualityFillInDate = ServerTime.Time;

                            break;
                        case "等待分析判定确认":
                            lnqQualityProblem.BillStatus = "等待指定责任人";
                            lnqQualityProblem.QualityAffirmDate = ServerTime.Time;
                            lnqQualityProblem.QualityAffirmPersonnel = BasicInfo.LoginName;

                            break;
                        case "等待指定责任人":
                            lnqQualityProblem.BillStatus = "等待整改措施";

                            break;
                        case "等待整改措施":
                            lnqQualityProblem.BillStatus = "等待整改措施确认";
                            lnqQualityProblem.RelevantFillInPersonnel = BasicInfo.LoginName;
                            lnqQualityProblem.RelevantFillInDate = ServerTime.Time;

                            break;

                        case "等待整改措施确认":
                            lnqQualityProblem.BillStatus = "等待效果确认";
                            lnqQualityProblem.RelevantAffirmPersonnel = BasicInfo.LoginName;
                            lnqQualityProblem.RelevantAffirmDate = ServerTime.Time;

                            break;
                        case "等待效果确认":
                            lnqQualityProblem.BillStatus = "等待最终确认";
                            lnqQualityProblem.EffectAffirmDate = ServerTime.Time;
                            lnqQualityProblem.EffectAffirmPersonnel = BasicInfo.LoginName;

                            break;
                        case "等待最终确认":
                            lnqQualityProblem.BillStatus = "已完成";
                            lnqQualityProblem.LaunchDepartmentAffirmDate = ServerTime.Time;
                            lnqQualityProblem.LaunchDepartmentAffirmPersonnel = BasicInfo.LoginName;

                            break;
                        default:
                            break;
                    }

                    dataContext.SubmitChanges();
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
        /// 删除单据
        /// </summary>
        /// <param name="billID">需要删除的单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool DeleteBill(string billID,out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varBill = from a in dataContext.ZL_QualityProblemRectificationDisposalBill
                              where a.Bill_ID == billID
                              select a;
                if (varBill.Count() != 1)
                {
                    error = "数据为空或者不唯一";
                    return false;
                }
                else
                {
                    ZL_QualityProblemRectificationDisposalBill lnqQualityProblem = new ZL_QualityProblemRectificationDisposalBill();

                    if (lnqQualityProblem.AnalyseAndJudgeAffirmID != null && lnqQualityProblem.AnalyseAndJudgeAffirmID.ToString() != "")
                    {
                        if (!DeleteSundrySchedule("搭车分析计划表", lnqQualityProblem.AnalyseAndJudgeAffirmID.ToString(), out error))
                        {
                            return false;
                        }
                    }

                    if (lnqQualityProblem.CauseAnalysisTestingID != null && lnqQualityProblem.CauseAnalysisTestingID.ToString() != "")
                    {
                        if (!DeleteSundrySchedule("试验验证计划表", lnqQualityProblem.CauseAnalysisTestingID.ToString(), out error))
                        {
                            return false;
                        }
                    }

                    if (lnqQualityProblem.InterimMeasureTestingID != null && lnqQualityProblem.InterimMeasureTestingID.ToString() != "")
                    {
                        if (!DeleteSundrySchedule("试验验证计划表", lnqQualityProblem.InterimMeasureTestingID.ToString(), out error))
                        {
                            return false;
                        }
                    }

                    if (lnqQualityProblem.RectificationMeasuresChangeID != null && lnqQualityProblem.RectificationMeasuresChangeID.ToString() != "")
                    {
                        if (!DeleteSundrySchedule("新品开发计划表", lnqQualityProblem.RectificationMeasuresChangeID.ToString(), out error))
                        {
                            return false;
                        }
                    }
                }


                dataContext.ZL_QualityProblemRectificationDisposalBill.DeleteAllOnSubmit(varBill);
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
        /// 对各种计划表插入新记录
        /// </summary>
        /// <param name="scheduleType">计划表类型：试验验证计划表，新品开发计划表，搭车分析计划表</param>
        /// <param name="billID">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool InsertSundrySchedule(string scheduleType, string billID, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                switch (scheduleType)
                {
                    case "试验验证计划表":

                        var varExperimentsSchedule = from a in dataContext.ZL_ExperimentsSchedule
                                          where a.Bill_ID == billID
                                          select a;

                        if (varExperimentsSchedule.Count() != 0)
                        {
                            error = "数据不唯一";
                            return false;
                        }
                        else
                        {
                            ZL_ExperimentsSchedule lnqExperiment = new ZL_ExperimentsSchedule();

                            lnqExperiment.Bill_ID = billID;
                            lnqExperiment.PlanProducer = BasicInfo.LoginName;
                            lnqExperiment.PlanTime = ServerTime.Time;

                            dataContext.ZL_ExperimentsSchedule.InsertOnSubmit(lnqExperiment);
                        }

                        break;
                    case "新品开发计划表":

                        var varNewProductDevelopmentSchedule = from a in dataContext.ZL_NewProductDevelopmentSchedule
                                          where a.Bill_ID == billID
                                          select a;

                        if (varNewProductDevelopmentSchedule.Count() != 0)
                        {
                            error = "数据不唯一";
                            return false;
                        }
                        else
                        {
                            ZL_NewProductDevelopmentSchedule lnqNewProduct = new ZL_NewProductDevelopmentSchedule();

                            lnqNewProduct.Bill_ID = billID;
                            lnqNewProduct.PlanProducer = BasicInfo.LoginName;
                            lnqNewProduct.PlanTime = ServerTime.Time;

                            dataContext.ZL_NewProductDevelopmentSchedule.InsertOnSubmit(lnqNewProduct);
                        }

                        break;
                    case "搭车分析计划表":

                        var AssemblingAnalysisSchedule = from a in dataContext.ZL_AssemblingAnalysisSchedule
                                          where a.Bill_ID == billID
                                          select a;

                        if (AssemblingAnalysisSchedule.Count() != 0)
                        {
                            error = "数据不唯一";
                            return false;
                        }
                        else
                        {
                            ZL_AssemblingAnalysisSchedule lnqAssemblingAnalysis = new ZL_AssemblingAnalysisSchedule();

                            lnqAssemblingAnalysis.Bill_ID = billID;
                            lnqAssemblingAnalysis.PlanProducer = BasicInfo.LoginName;
                            lnqAssemblingAnalysis.PlanTime = ServerTime.Time;

                            dataContext.ZL_AssemblingAnalysisSchedule.InsertOnSubmit(lnqAssemblingAnalysis);
                        }

                        break;
                    default:
                        break;
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
        /// 对各种计划表删除记录
        /// </summary>
        /// <param name="scheduleType">计划表类型：试验验证计划表，新品开发计划表，搭车分析计划表</param>
        /// <param name="billID">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool DeleteSundrySchedule(string scheduleType, string billID, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                switch (scheduleType)
                {
                    case "试验验证计划表":

                        var varExperimentsSchedule = from a in dataContext.ZL_ExperimentsSchedule
                                                     where a.Bill_ID == billID
                                                     select a;

                        var varExperimentsScheduleList = from a in dataContext.ZL_ExperimentsScheduleList
                                                         where a.Bill_ID == billID
                                                         select a;

                        dataContext.ZL_ExperimentsScheduleList.DeleteAllOnSubmit(varExperimentsScheduleList);
                        dataContext.ZL_ExperimentsSchedule.DeleteAllOnSubmit(varExperimentsSchedule);

                        break;
                    case "新品开发计划表":

                        var varNewProductDevelopmentSchedule = from a in dataContext.ZL_NewProductDevelopmentSchedule
                                                               where a.Bill_ID == billID
                                                               select a;

                        var varNewProductDevelopmentScheduleList = from a in dataContext.ZL_NewProductDevelopmentScheduleList
                                                                   where a.Bill_ID == billID
                                                                   select a;

                        dataContext.ZL_NewProductDevelopmentScheduleList.DeleteAllOnSubmit(varNewProductDevelopmentScheduleList);
                        dataContext.ZL_NewProductDevelopmentSchedule.DeleteAllOnSubmit(varNewProductDevelopmentSchedule);

                        break;
                    case "搭车分析计划表":

                        var varAssemblingAnalysisSchedule = from a in dataContext.ZL_AssemblingAnalysisSchedule
                                                         where a.Bill_ID == billID
                                                         select a;

                        dataContext.ZL_AssemblingAnalysisSchedule.DeleteAllOnSubmit(varAssemblingAnalysisSchedule);

                        break;
                    default:
                        break;
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
        /// 删除过剩的计划表记录
        /// </summary>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool DeleteExcessSchedule(out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                string strSql = " select a.Bill_ID from ZL_ExperimentsSchedule as a " +
                                " left join (select InterimMeasureTestingID as Bill_ID from " +
                                " (select InterimMeasureTestingID from ZL_QualityProblemRectificationDisposalBill " +
                                " union all select CauseAnalysisTestingID from ZL_QualityProblemRectificationDisposalBill) as a )as b  " +
                                " on a.Bill_ID = b.Bill_ID where b.Bill_ID is null ";

                DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

                for (int i = 0; i < dtTemp.Rows.Count; i++)
                {
                    var varExperimentsSchedule = from a in dataContext.ZL_ExperimentsSchedule
                                                 where a.Bill_ID == dtTemp.Rows[i][0].ToString()
                                                 select a;

                    var varExperimentsScheduleList = from a in dataContext.ZL_ExperimentsScheduleList
                                                     where a.Bill_ID == dtTemp.Rows[i][0].ToString()
                                                     select a;

                    dataContext.ZL_ExperimentsScheduleList.DeleteAllOnSubmit(varExperimentsScheduleList);
                    dataContext.ZL_ExperimentsSchedule.DeleteAllOnSubmit(varExperimentsSchedule);
                }

                strSql = " select a.Bill_ID  from ZL_NewProductDevelopmentSchedule as a left join " +
                         " ZL_QualityProblemRectificationDisposalBill as b on a.Bill_ID = b.RectificationMeasuresChangeID " +
                         " where b.RectificationMeasuresChangeID is null ";

                dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

                for (int i = 0; i < dtTemp.Rows.Count; i++)
                {
                    var varNewProductDevelopmentSchedule = from a in dataContext.ZL_NewProductDevelopmentSchedule
                                                           where a.Bill_ID == dtTemp.Rows[i][0].ToString()
                                                           select a;

                    var varNewProductDevelopmentScheduleList = from a in dataContext.ZL_NewProductDevelopmentScheduleList
                                                               where a.Bill_ID == dtTemp.Rows[i][0].ToString()
                                                               select a;

                    dataContext.ZL_NewProductDevelopmentScheduleList.DeleteAllOnSubmit(varNewProductDevelopmentScheduleList);
                    dataContext.ZL_NewProductDevelopmentSchedule.DeleteAllOnSubmit(varNewProductDevelopmentSchedule);
                }

                strSql = " select a.Bill_ID  from ZL_AssemblingAnalysisSchedule as a left join " +
                         " ZL_QualityProblemRectificationDisposalBill as b on a.Bill_ID = b.AnalyseAndJudgeAffirmID " +
                         " where b.AnalyseAndJudgeAffirmID is null ";

                dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

                for (int i = 0; i < dtTemp.Rows.Count; i++)
                {
                    var varAssemblingAnalysisSchedule = from a in dataContext.ZL_AssemblingAnalysisSchedule
                                                        where a.Bill_ID == dtTemp.Rows[i][0].ToString()
                                                        select a;

                    dataContext.ZL_AssemblingAnalysisSchedule.DeleteAllOnSubmit(varAssemblingAnalysisSchedule);
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
        /// 获得试验验证计划表的试验步骤明细
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <returns>返回Table</returns>
        public DataTable GetExperimentsScheduleList(string billID)
        {
            string strSql = "select 试验步骤, 具体步骤, 完成日期, 责任人 "+
            " from View_ZL_ExperimentsScheduleList where 单据号 = '" + billID + "'";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得新品开发计划表的开发步骤明细
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <returns>返回Table</returns>
        public DataTable GetNewProductDevelopmentScheduleList(string billID)
        {
            string strSql = "select 开发过程, 工作事项, 完成日期, 责任人 " +
            " from View_ZL_NewProductDevelopmentScheduleList where 单据号 = '" + billID + "'";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 保存试验验证计划表的信息
        /// </summary>
        /// <param name="experiment">数据集</param>
        /// <param name="listInfo">具体步骤列表</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool SaveExperimentsScheduleInfo(ZL_ExperimentsSchedule experiment,DataTable listInfo, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.ZL_ExperimentsSchedule
                              where a.Bill_ID == experiment.Bill_ID
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数据不唯一或者为空";
                    return false;
                }
                else
                {
                    ZL_ExperimentsSchedule lnqExperiments = varData.Single();

                    lnqExperiments.AuditPersonnel = experiment.AuditPersonnel;
                    lnqExperiments.AuditTime = experiment.AuditTime;
                    lnqExperiments.PlanProducer = BasicInfo.LoginName;
                    lnqExperiments.PlanTime = ServerTime.Time;
                    lnqExperiments.TestCode = experiment.TestCode;
                    lnqExperiments.TestObjective = experiment.TestObjective;
                    lnqExperiments.TestPrincipal = experiment.TestPrincipal;
                    lnqExperiments.TestProgram = experiment.TestProgram;
                    lnqExperiments.TestTime = experiment.TestTime;
                    lnqExperiments.AuditPersonnel = null;
                    lnqExperiments.AuditTime = null;


                    var varList = from a in dataContext.ZL_ExperimentsScheduleList
                                  where a.Bill_ID == experiment.Bill_ID
                                  select a;

                    dataContext.ZL_ExperimentsScheduleList.DeleteAllOnSubmit(varList);

                    for (int i = 0; i < listInfo.Rows.Count; i++)
                    {
                        ZL_ExperimentsScheduleList lnqExperimentsList = new ZL_ExperimentsScheduleList();

                        lnqExperimentsList.Bill_ID = experiment.Bill_ID;
                        lnqExperimentsList.FinishTime = Convert.ToDateTime(listInfo.Rows[i]["完成日期"]);
                        lnqExperimentsList.Prinicipal = listInfo.Rows[i]["责任人"].ToString();
                        lnqExperimentsList.SpecificStep = listInfo.Rows[i]["具体步骤"].ToString();
                        lnqExperimentsList.StepNumber = Convert.ToInt32(listInfo.Rows[i]["试验步骤"]);

                        dataContext.ZL_ExperimentsScheduleList.InsertOnSubmit(lnqExperimentsList);
                    }
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
        /// 审核搭车分析计划表的分析结果
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool AuditingAnalysisResult(string billID,out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.ZL_AssemblingAnalysisSchedule
                              where a.Bill_ID == billID
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数据不唯一或者为空";
                    return false;
                }
                else
                {
                    ZL_AssemblingAnalysisSchedule lnqAssemblingAnalysis = varData.Single();

                    lnqAssemblingAnalysis.AnalysisAuditPersonnel = BasicInfo.LoginName;
                    lnqAssemblingAnalysis.AnalysisAuditTime = ServerTime.Time;
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
        /// 保存搭车分析计划表的分析结果信息
        /// </summary>
        /// <param name="assemblingAnalysis">数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool SaveAnalysisResult(ZL_AssemblingAnalysisSchedule assemblingAnalysis, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.ZL_AssemblingAnalysisSchedule
                              where a.Bill_ID == assemblingAnalysis.Bill_ID
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数据不唯一或者为空";
                    return false;
                }
                else
                {
                    ZL_AssemblingAnalysisSchedule lnqAssemblingAnalysis = varData.Single();

                    lnqAssemblingAnalysis.AnalysisPersonnel = BasicInfo.LoginName;
                    lnqAssemblingAnalysis.AnalysisTime = ServerTime.Time;
                    lnqAssemblingAnalysis.AnalysisResult = assemblingAnalysis.AnalysisResult;
                    lnqAssemblingAnalysis.FaultDataReplay = assemblingAnalysis.FaultDataReplay;
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
        /// 保存搭车分析计划表信息
        /// </summary>
        /// <param name="assemblingAnalysis">数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool SaveAssemblingAnalysisScheduleInfo(ZL_AssemblingAnalysisSchedule assemblingAnalysis,  out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.ZL_AssemblingAnalysisSchedule
                              where a.Bill_ID == assemblingAnalysis.Bill_ID
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数据不唯一或者为空";
                    return false;
                }
                else
                {
                    ZL_AssemblingAnalysisSchedule lnqAssemblingAnalysis = varData.Single();

                    lnqAssemblingAnalysis.AnalysisAuditPersonnel = null;
                    lnqAssemblingAnalysis.AnalysisAuditTime = null;
                    lnqAssemblingAnalysis.AnalysisDepartment = assemblingAnalysis.AnalysisDepartment;
                    lnqAssemblingAnalysis.AnalysisPersonnel = null;
                    lnqAssemblingAnalysis.AnalysisTime = null;
                    lnqAssemblingAnalysis.AnalysisResult = null;
                    lnqAssemblingAnalysis.AssemblingClaim = assemblingAnalysis.AssemblingClaim;
                    lnqAssemblingAnalysis.AuditPersonnel = assemblingAnalysis.AuditPersonnel;
                    lnqAssemblingAnalysis.AuditTime = assemblingAnalysis.AuditTime;
                    lnqAssemblingAnalysis.FaultDataReplay = null;
                    lnqAssemblingAnalysis.FaultCode = assemblingAnalysis.FaultCode;
                    lnqAssemblingAnalysis.FeedbackNumber = assemblingAnalysis.FeedbackNumber;
                    lnqAssemblingAnalysis.FinishTimeClaim = assemblingAnalysis.FinishTimeClaim;
                    lnqAssemblingAnalysis.Mileage = assemblingAnalysis.Mileage;
                    lnqAssemblingAnalysis.PlanProducer = BasicInfo.LoginName;
                    lnqAssemblingAnalysis.PlanTime = ServerTime.Time;
                    lnqAssemblingAnalysis.AuditPersonnel = null;
                    lnqAssemblingAnalysis.AuditTime = null;
                    lnqAssemblingAnalysis.Principal = assemblingAnalysis.Principal;
                    lnqAssemblingAnalysis.ProductCode = assemblingAnalysis.ProductCode;
                    lnqAssemblingAnalysis.ProductType = assemblingAnalysis.ProductType;
                    lnqAssemblingAnalysis.SpecificFault = assemblingAnalysis.SpecificFault;
                    lnqAssemblingAnalysis.TestBedResults = assemblingAnalysis.TestBedResults;
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
        /// 保存试验验证计划表的信息
        /// </summary>
        /// <param name="newProductDevelopment">数据集</param>
        /// <param name="listInfo">具体开发过程列表</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool SaveNewProductDevelopmentScheduleInfo(ZL_NewProductDevelopmentSchedule newProductDevelopment, DataTable listInfo, 
            out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.ZL_NewProductDevelopmentSchedule
                              where a.Bill_ID == newProductDevelopment.Bill_ID
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数据不唯一或者为空";
                    return false;
                }
                else
                {
                    ZL_NewProductDevelopmentSchedule lnqNewProductDevelopment = varData.Single();

                    lnqNewProductDevelopment.AuditPersonnel = newProductDevelopment.AuditPersonnel;
                    lnqNewProductDevelopment.AuditTime = newProductDevelopment.AuditTime;
                    lnqNewProductDevelopment.DevelopmentReason = newProductDevelopment.DevelopmentReason;
                    lnqNewProductDevelopment.GoodsCode = newProductDevelopment.GoodsCode;
                    lnqNewProductDevelopment.GoodsName = newProductDevelopment.GoodsName;
                    lnqNewProductDevelopment.PlanProducer = BasicInfo.LoginName;
                    lnqNewProductDevelopment.PlanTime = ServerTime.Time;
                    lnqNewProductDevelopment.AuditPersonnel = null;
                    lnqNewProductDevelopment.AuditTime = null;

                    var varList = from a in dataContext.ZL_NewProductDevelopmentScheduleList
                                  where a.Bill_ID == newProductDevelopment.Bill_ID
                                  select a;

                    dataContext.ZL_NewProductDevelopmentScheduleList.DeleteAllOnSubmit(varList);

                    for (int i = 0; i < listInfo.Rows.Count; i++)
                    {
                        ZL_NewProductDevelopmentScheduleList lnqNewProductList = new ZL_NewProductDevelopmentScheduleList();

                        lnqNewProductList.Bill_ID = newProductDevelopment.Bill_ID;
                        lnqNewProductList.Agenda = listInfo.Rows[i]["工作事项"].ToString();
                        lnqNewProductList.DevelopmentStepNumber = Convert.ToInt32(listInfo.Rows[i]["开发过程"]);
                        lnqNewProductList.FinishTime = Convert.ToDateTime(listInfo.Rows[i]["完成日期"]);
                        lnqNewProductList.Principal = listInfo.Rows[i]["责任人"].ToString();

                        dataContext.ZL_NewProductDevelopmentScheduleList.InsertOnSubmit(lnqNewProductList);
                    }
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
        /// 对各种计划表审核记录
        /// </summary>
        /// <param name="scheduleType">计划表类型：试验验证计划表，新品开发计划表，搭车分析计划表</param>
        /// <param name="billID">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool AuditingSundrySchedule(string scheduleType, string billID, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                switch (scheduleType)
                {
                    case "试验验证计划表":

                        var varExperimentsSchedule = from a in dataContext.ZL_ExperimentsSchedule
                                                     where a.Bill_ID == billID
                                                     select a;

                        if (varExperimentsSchedule.Count() != 1)
                        {
                            error = "数据不唯一";
                            return false;
                        }
                        else
                        {
                            ZL_ExperimentsSchedule lnqExperiments = varExperimentsSchedule.Single();

                            lnqExperiments.AuditPersonnel = BasicInfo.LoginName;
                            lnqExperiments.AuditTime = ServerTime.Time;
                        }

                        break;
                    case "新品开发计划表":

                        var varNewProductDevelopmentSchedule = from a in dataContext.ZL_NewProductDevelopmentSchedule
                                                               where a.Bill_ID == billID
                                                               select a;

                        if (varNewProductDevelopmentSchedule.Count() != 1)
                        {
                            error = "数据不唯一";
                            return false;
                        }
                        else
                        {
                            ZL_NewProductDevelopmentSchedule lnqNewProductDevelopment = varNewProductDevelopmentSchedule.Single();

                            lnqNewProductDevelopment.AuditPersonnel = BasicInfo.LoginName;
                            lnqNewProductDevelopment.AuditTime = ServerTime.Time;
                        }

                        break;
                    case "搭车分析计划表":

                        var varAssemblingAnalysisSchedule = from a in dataContext.ZL_AssemblingAnalysisSchedule
                                                            where a.Bill_ID == billID
                                                            select a;

                        if (varAssemblingAnalysisSchedule.Count() != 1)
                        {
                            error = "数据不唯一";
                            return false;
                        }
                        else
                        {
                            ZL_AssemblingAnalysisSchedule lnqAssemblingAnalysis = varAssemblingAnalysisSchedule.Single();

                            lnqAssemblingAnalysis.AuditPersonnel = BasicInfo.LoginName;
                            lnqAssemblingAnalysis.AuditTime = ServerTime.Time;
                        }

                        break;
                    default:
                        break;
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
    }
}
