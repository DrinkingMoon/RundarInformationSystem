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
    class FocalWork : IFocalWork
    {
        public DateTime GetEndDate(string keyValue)
        {
            string strSql = " select a.YearMonth from Bus_FocalWork_MonthlyProgress as a "+
                            " inner join Bus_FocalWork_MonthlyProgress_Content as b on a.BillNo = b.BillNo "+
                            " where b.FocalWorkId = '" + keyValue + "' and "+
                            " a.BillNo in (select BillNo from Flow_FlowBillData where FlowID = 108) order by a.YearMonth desc";

            DataTable tempTable = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (tempTable == null || tempTable.Rows.Count == 0)
            {
                throw new Exception("未找到【完成时间】");
            }
            else
            {
                return Convert.ToDateTime( tempTable.Rows[0][0].ToString().Substring(0, 4) + "-" 
                    + tempTable.Rows[0][0].ToString().Substring(4, 2) + "-1");
            }
        }

        public DataTable GetTable_KeyPoint(string keyValue)
        {
            string strSql = "select KeyPointName as 重点工作关键节点, KeyStatus as 状态, StartDate as 启动时间, EndDate as 完成时间, "+
                " b.Name as 责任人, F_Id from Bus_FocalWork_KeyPoint as a inner join HR_PersonnelArchive as b on a.DutyUser = b.WorkID " +
                " where a.FocalWorkId = '" + keyValue + "' order by a.EndDate desc";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        public DataTable GetTable_FocalWork()
        {
            string strSql = "select TaskName as 重点工作, TaskStatus as 状态, StartDate as 启动时间, EndDate as 完成时间, b.Name as 责任人, "+
                " a.F_Id from Bus_FocalWork as a inner join HR_PersonnelArchive as b on a.DutyUser = b.WorkID order by a.F_CreateDate desc";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        public DataTable GetTable_FocalWork(string yearMonth)
        {
            string strSql = " select a.TaskName as 重点工作, a.TaskStatus as 工作状态, a.StartDate as 启动时间, a.EndDate as 完成时间, "+
                            " dbo.fun_get_personnelName(a.DutyUser) as 责任人, case when b.Evaluate is null then '未填写' else b.Evaluate end as 当月评价, a.F_Id " +
                            " from Bus_FocalWork as a left join (select * from (select a.*, b.FocalWorkId, b.Evaluate from Bus_FocalWork_MonthlyProgress as a  "+
                            " inner join Bus_FocalWork_MonthlyProgress_Content as b on a.BillNo = b.BillNo ) as a  "+
                            " where a.BillNo = (select MAX(BillNo) from (select a.*, b.FocalWorkId, b.Evaluate from Bus_FocalWork_MonthlyProgress as a  "+
                            " inner join Bus_FocalWork_MonthlyProgress_Content as b on a.BillNo = b.BillNo ) as b  "+
                            " where a.YearMonth = b.YearMonth) and a.YearMonth = '" + yearMonth + "') as b on a.F_Id = b.FocalWorkId where a.TaskStatus <> '已完成'";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        public Bus_FocalWork GetSingle_FocalWork(string keyValue)
        {
            using (DepotManagementDataContext ctx = CommentParameter.DepotDataContext)
            {
                var varData = from a in ctx.Bus_FocalWork
                              where a.F_Id == keyValue
                              select a;

                return varData.First();
            }
        }

        public Bus_FocalWork_KeyPoint GetSingle_KeyPoint(string keyValue)
        {
            using (DepotManagementDataContext ctx = CommentParameter.DepotDataContext)
            {
                var varData = from a in ctx.Bus_FocalWork_KeyPoint
                              where a.F_Id == keyValue
                              select a;

                return varData.First();
            }
        }

        public Bus_FocalWork_MonthlyProgress_Content GetSingle_Content(string focalWorkId, string yearMonth)
        {
            string strSql = " select a.BillNo from Bus_FocalWork_MonthlyProgress as a "+
                            " inner join Bus_FocalWork_MonthlyProgress_Content as b on a.BillNo = b.BillNo "+
                            " where b.FocalWorkId = '"+ focalWorkId +"' and a.YearMonth = '"+ yearMonth +"'"+
                            " and a.BillNo in (select BillNo from Flow_FlowBillData where FlowID = 108) order by a.BillNo desc";

            DataTable tempTable = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (tempTable == null || tempTable.Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return GetList_ProgressContent(tempTable.Rows[0][0].ToString()).Where(k => k.FocalWorkId == focalWorkId).First();
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
            BillNumberControl billNoControl = new BillNumberControl(CE_BillTypeEnum.重点工作.ToString(), this);

            try
            {
                var varData = from a in ctx.Bus_FocalWork_MonthlyProgress
                              where a.BillNo == billNo
                              select a;

                ctx.Bus_FocalWork_MonthlyProgress.DeleteAllOnSubmit(varData);
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
            var varData = from a in ctx.Bus_FocalWork_MonthlyProgress
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
        public Bus_FocalWork_MonthlyProgress GetSingleBillInfo(string billNo)
        {
            if (billNo == null)
            {
                return null;
            }

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Bus_FocalWork_MonthlyProgress
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

        public List<Bus_FocalWork_MonthlyProgress_Content> GetList_ProgressContent(string billNo)
        {
            using (DepotManagementDataContext ctx = CommentParameter.DepotDataContext)
            {
                var varData = from a in ctx.Bus_FocalWork_MonthlyProgress_Content
                              where a.BillNo == billNo
                              select a;

                return varData.ToList();
            }
        }

        public List<Bus_FocalWork_MonthlyProgress_Content> GetList_ProgressContent(Bus_FocalWork_MonthlyProgress billInfo)
        {
            List<Bus_FocalWork_MonthlyProgress_Content> result = new List<Bus_FocalWork_MonthlyProgress_Content>();

            DateTime tempDate =
                Convert.ToDateTime(billInfo.YearMonth.Substring(0, 4) + "-" 
                + billInfo.YearMonth.Substring(4, 2) + "-1").AddMonths(1).AddDays(-1);

            string strSql = "select * from Bus_FocalWork where DutyUser = '"
                + billInfo.CreateUser +"' and StartDate <= '"+ tempDate.ToShortDateString() 
                +"' and TaskStatus not in ('已完成','终止')";
            DataTable tempTable = GlobalObject.DatabaseServer.QueryInfo(strSql);

            foreach (DataRow dr in tempTable.Rows)
            {
                Bus_FocalWork_MonthlyProgress_Content tempContent = new Bus_FocalWork_MonthlyProgress_Content();

                tempContent.BillNo = billInfo.BillNo;
                tempContent.F_Id = Guid.NewGuid().ToString();
                tempContent.FocalWorkId = dr["F_Id"].ToString();

                result.Add(tempContent);
            }

            return result;
        }

        public List<Bus_FocalWork_MonthlyProgress_KeyPoint> GetList_ProgressKeyPoint(string billNo)
        {
            using (DepotManagementDataContext ctx = CommentParameter.DepotDataContext)
            {
                var varData = from a in ctx.Bus_FocalWork_MonthlyProgress_KeyPoint
                              where a.BillNo == billNo
                              select a;

                return varData.ToList();
            }
        }

        public List<Bus_FocalWork_MonthlyProgress_KeyPoint> GetList_ProgressKeyPoint(Bus_FocalWork_MonthlyProgress billInfo, 
            List<Bus_FocalWork_MonthlyProgress_Content> lstContent)
        {
            List<Bus_FocalWork_MonthlyProgress_KeyPoint> result = new List<Bus_FocalWork_MonthlyProgress_KeyPoint>();

            DateTime tempDate =
                Convert.ToDateTime(billInfo.YearMonth.Substring(0, 4) + "-"
                + billInfo.YearMonth.Substring(4, 2) + "-1").AddMonths(1);

            foreach (Bus_FocalWork_MonthlyProgress_Content content in lstContent)
            {
                string strSql = "select * from Bus_FocalWork_KeyPoint where FocalWorkId = '" 
                    + content.FocalWorkId + "' and EndDate < '" + tempDate.ToShortDateString()+ "' and KeyStatus <> '已完成'";

                DataTable tempTable = GlobalObject.DatabaseServer.QueryInfo(strSql);

                foreach (DataRow dr in tempTable.Rows)
                {
                    Bus_FocalWork_MonthlyProgress_KeyPoint keyPoint = new Bus_FocalWork_MonthlyProgress_KeyPoint();

                    keyPoint.BillNo = billInfo.BillNo;
                    keyPoint.F_Id = Guid.NewGuid().ToString();
                    keyPoint.FocalWorkId = content.FocalWorkId;
                    keyPoint.KeyPointId = dr["F_Id"].ToString();

                    result.Add(keyPoint);
                }
            }

            return result;
        }

        void SaveDetail(DepotManagementDataContext ctx, string billNo, List<Bus_FocalWork_MonthlyProgress_Content> lstContent,
            List<Bus_FocalWork_MonthlyProgress_KeyPoint> lstKeyPoint)
        {
            try
            {
                var varData = from a in ctx.Bus_FocalWork_MonthlyProgress_Content
                              where a.BillNo == billNo
                              select a;

                ctx.Bus_FocalWork_MonthlyProgress_Content.DeleteAllOnSubmit(varData);

                var varData1 = from a in ctx.Bus_FocalWork_MonthlyProgress_KeyPoint
                               where a.BillNo == billNo
                               select a;

                ctx.Bus_FocalWork_MonthlyProgress_KeyPoint.DeleteAllOnSubmit(varData1);

                ctx.Bus_FocalWork_MonthlyProgress_Content.InsertAllOnSubmit(lstContent);

                foreach (Bus_FocalWork_MonthlyProgress_Content content in lstContent)
                {
                    ctx.Bus_FocalWork_MonthlyProgress_KeyPoint.InsertAllOnSubmit(lstKeyPoint.Where(k => k.BillNo == billNo 
                        && k.FocalWorkId == content.FocalWorkId).ToList());
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteKeyPoint(string keyValue)
        {
            using (DepotManagementDataContext ctx = CommentParameter.DepotDataContext)
            {
                var varData = from a in ctx.Bus_FocalWork_KeyPoint
                              where a.F_Id == keyValue
                              select a;

                ctx.Bus_FocalWork_KeyPoint.DeleteAllOnSubmit(varData);
                ctx.SubmitChanges();
            }
        }

        public void DeleteFocalWork(string keyValue)
        {
            using (DepotManagementDataContext ctx = CommentParameter.DepotDataContext)
            {
                var varData = from a in ctx.Bus_FocalWork
                              where a.F_Id == keyValue
                              select a;

                ctx.Bus_FocalWork.DeleteAllOnSubmit(varData);
                ctx.SubmitChanges();
            }
        }

        public void SaveKeyPoint(Bus_FocalWork_KeyPoint keyPoint)
        {
            using (DepotManagementDataContext ctx = CommentParameter.DepotDataContext)
            {
                Bus_FocalWork_KeyPoint tempKey = new Bus_FocalWork_KeyPoint();

                var varData = from a in ctx.Bus_FocalWork_KeyPoint
                              where a.F_Id == keyPoint.F_Id
                              select a;

                if (varData.Count() == 1)
                {
                    tempKey = varData.Single();

                    tempKey.DutyUser = keyPoint.DutyUser;
                    tempKey.EndDate = keyPoint.EndDate;
                    tempKey.StartDate = keyPoint.StartDate;
                    tempKey.FocalWorkId = keyPoint.FocalWorkId;
                    tempKey.KeyPointName = keyPoint.KeyPointName;
                }
                else
                {
                    keyPoint.KeyStatus = ServerTime.Time.Date >= ((DateTime)keyPoint.StartDate).Date ? "进行中" : "待启动";
                    ctx.Bus_FocalWork_KeyPoint.InsertOnSubmit(keyPoint);
                }

                ctx.SubmitChanges();
            }
        }

        public void SaveFocalWork(Bus_FocalWork focalWork)
        {
            using (DepotManagementDataContext ctx = CommentParameter.DepotDataContext)
            {
                Bus_FocalWork work = new Bus_FocalWork();

                var varData = from a in ctx.Bus_FocalWork
                              where a.F_Id == focalWork.F_Id
                              select a;

                if (varData.Count() == 1)
                {
                    work = varData.Single();

                    work.DutyUser = focalWork.DutyUser;
                    work.EndDate = focalWork.EndDate;
                    work.ExpectedGoal = focalWork.ExpectedGoal;
                    work.F_CreateDate = ServerTime.Time;
                    work.F_CreateUser = BasicInfo.LoginID;
                    work.StartDate = focalWork.StartDate;
                    work.TaskDescription = focalWork.TaskDescription;
                    work.TaskName = focalWork.TaskName;
                }
                else
                {
                    work.F_CreateDate = ServerTime.Time;
                    work.F_CreateUser = BasicInfo.LoginID;
                    focalWork.TaskStatus = ServerTime.Time.Date >= ((DateTime)focalWork.StartDate).Date ? "进行中" : "待启动";
                    ctx.Bus_FocalWork.InsertOnSubmit(focalWork);
                }

                ctx.SubmitChanges();
            }
        }

        /// <summary>
        /// 保存业务信息
        /// </summary>
        /// <param name="billInfo">业务总单信息</param>
        /// <param name="effectValue">业务明细信息</param>
        public void SaveInfo(Bus_FocalWork_MonthlyProgress billInfo, List<Bus_FocalWork_MonthlyProgress_Content> lstContent, 
            List<Bus_FocalWork_MonthlyProgress_KeyPoint> lstKeyPoint)
        {
            using (DepotManagementDataContext ctx = CommentParameter.DepotDataContext)
            {
                ctx.Connection.Open();
                ctx.Transaction = ctx.Connection.BeginTransaction();
                IFlowServer serviceFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();

                try
                {
                    var varData = from a in ctx.Bus_FocalWork_MonthlyProgress
                                  where a.BillNo == billInfo.BillNo
                                  select a;

                    Bus_FocalWork_MonthlyProgress lnqBill = new Bus_FocalWork_MonthlyProgress();

                    if (varData.Count() == 1)
                    {
                        lnqBill = varData.Single();

                        lnqBill.CreateUser = BasicInfo.LoginID;
                        lnqBill.YearMonth = billInfo.YearMonth;

                        SaveDetail(ctx, billInfo.BillNo, lstContent, lstKeyPoint);
                    }
                    else if (varData.Count() == 0)
                    {
                        ctx.Bus_FocalWork_MonthlyProgress.InsertOnSubmit(billInfo);
                        SaveDetail(ctx, billInfo.BillNo, lstContent, lstKeyPoint);
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
        }

        public void OpertionInfo(string billNo)
        {
            Bus_FocalWork_MonthlyProgress billInfo = GetSingleBillInfo(billNo);
            List<Bus_FocalWork_MonthlyProgress_Content> lstContent = GetList_ProgressContent(billNo);
            List<Bus_FocalWork_MonthlyProgress_KeyPoint> lstKeyPoint = GetList_ProgressKeyPoint(billNo);

            using (DepotManagementDataContext ctx = CommentParameter.DepotDataContext)
            {
                DateTime tempDate = Convert.ToDateTime(billInfo.YearMonth.Substring(0, 4) + "-"
                    + billInfo.YearMonth.Substring(4, 2) + "-1").AddMonths(1).AddDays(-1);

                foreach (Bus_FocalWork_MonthlyProgress_KeyPoint keyPoint in lstKeyPoint)
                {
                    var varKey = from a in ctx.Bus_FocalWork_KeyPoint
                                 where a.F_Id == keyPoint.KeyPointId
                                 select a;

                    varKey.Single().KeyStatus = keyPoint.Evaluate;
                }

                foreach (Bus_FocalWork_MonthlyProgress_Content content in lstContent)
                {
                    var varContent = from a in ctx.Bus_FocalWork
                                     where a.F_Id == content.FocalWorkId
                                     select a;

                    var varKey = from a in ctx.Bus_FocalWork_KeyPoint
                                 where a.FocalWorkId == content.FocalWorkId
                                 && a.KeyStatus != "已完成"
                                 select a;

                    if (varKey.Count() == 0)
                    {
                        varContent.Single().TaskStatus = "已完成";
                    }
                }

                var varContent1 = from a in ctx.Bus_FocalWork
                              where a.TaskStatus == "待启动"
                              && a.StartDate <= tempDate
                              select a;

                foreach (Bus_FocalWork focalWork in varContent1)
                {
                    focalWork.TaskStatus = "进行中";
                }

                var varContent2 = from a in ctx.Bus_FocalWork
                                  where a.TaskStatus == "进行中"
                                  && a.EndDate <= tempDate
                                  select a;

                foreach (Bus_FocalWork focalWork in varContent2)
                {
                    focalWork.TaskStatus = "延期";
                }

                var varKeyPoint = from a in ctx.Bus_FocalWork_KeyPoint
                                  where a.KeyStatus == "待启动"
                                  && a.StartDate <= tempDate
                                  select a;

                foreach (Bus_FocalWork_KeyPoint key in varKeyPoint)
                {
                    key.KeyStatus = "进行中";
                }

                ctx.SubmitChanges();
            }
        }
    }
}
