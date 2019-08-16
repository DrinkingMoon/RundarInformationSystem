using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ServerModule;
using PlatformManagement;

namespace Service_Peripheral_HR
{
    /// <summary>
    /// 排班信息操作类
    /// </summary>
    class WorkSchedulingServer : Service_Peripheral_HR.IWorkSchedulingServer
    {
        /// <summary>
        /// 查询结果过滤器
        /// </summary>
        string m_queryResultFilter = null;

        /// <summary>
        /// 获取或设置查询结果过滤器
        /// </summary>
        public string QueryResultFilter
        {
            get { return m_queryResultFilter; }
            set { m_queryResultFilter = value; }
        }

        /// <summary>
        /// 获取所有排班定义
        /// </summary>
        /// <returns>返回数据集</returns>
        public DataTable GetWorkSchedulingDefinition()
        {
            string sql = "select * from View_HR_WorkSchedulingDefinition";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);
            return dt;
        }

        /// <summary>
        /// 根据单号统计班次
        /// </summary>
        /// <param name="billNo">单号</param>
        /// <returns>返回数据集</returns>
        public DataTable GetDefinitionStatistics(int billNo)
        {
            string sql = @"select  员工编号,员工姓名,排班名称,Count(*) 统计班次 from dbo.View_HR_WorkSchedulingDetail " +
                          " group by 员工编号,单据号,排班名称,员工姓名 having  单据号=" + billNo + " order by 员工编号";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);
            return dt;
        }

        /// <summary>
        /// 新增修改排班定义信息
        /// </summary>
        /// <param name="definition">排班定义数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool AddDefinition(HR_WorkSchedulingDefinition definition,out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContct = CommentParameter.DepotDataContext;

                var result = from a in dataContct.HR_WorkSchedulingDefinition
                             where a.Code == definition.Code
                             select a;

                if (result.Count() == 1)
                {
                    HR_WorkSchedulingDefinition workScheduling = result.Single();

                    workScheduling.Name = definition.Name;
                    workScheduling.IsHoliday = definition.IsHoliday;
                    workScheduling.BeginTime = definition.BeginTime;
                    workScheduling.EndTime = definition.EndTime;
                    workScheduling.PunchInBeginTime = definition.PunchInBeginTime;
                    workScheduling.PunchInEndTime = definition.PunchInEndTime;
                    workScheduling.PunchOutBeginTime = definition.PunchOutBeginTime;
                    workScheduling.PunchOutEndTime = definition.PunchOutEndTime;
                    workScheduling.EndOffsetDays = definition.EndOffsetDays;
                    workScheduling.Recorder = definition.Recorder;
                    workScheduling.RecordTime = definition.RecordTime;
                    workScheduling.Remark = definition.Remark;
                }
                else
                {
                    dataContct.HR_WorkSchedulingDefinition.InsertOnSubmit(definition);
                }

                dataContct.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 通过编号删除排班定义
        /// </summary>
        /// <param name="code">排班编码</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True失败返回False</returns>
        public bool DeleteDefinition(string code,out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_WorkSchedulingDetail
                             where a.Code == code
                             select a;

                if (result.Count() > 0)
                {
                    error = "排班中用到了此定义，不能删除！";
                    return false;
                }

                var resultList = from c in dataContxt.HR_WorkSchedulingDefinition
                                 where c.Code == code
                                 select c;

                dataContxt.HR_WorkSchedulingDefinition.DeleteAllOnSubmit(resultList);
                dataContxt.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 获取所有排班信息
        /// </summary>
        /// <returns>返回数据集</returns>
        public DataTable GetWorkScheduling()
        {
            string sql = @"select * from View_HR_WorkScheduling";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);
            return dt;
        }

        /// <summary>
        /// 获取所有排班信息
        /// </summary>
        /// <param name="returnInfo">排班信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool GetAllWorkScheduling(out IQueryResult returnInfo, out string error)
        {
            returnInfo = null;
            error = null;

            IAuthorization serverAuthorization = PlatformFactory.GetObject<IAuthorization>();
            IQueryResult qr = null;

            if (QueryResultFilter == null)
            {
                qr = serverAuthorization.Query("排班管理", null);
            }
            else
            {
                qr = serverAuthorization.Query("排班管理", null, QueryResultFilter);
            }

            if (!qr.Succeeded)
            {
                error = qr.Error;
                return false;
            }

            returnInfo = qr;
            return true;
        }

        /// <summary>
        /// 获得员工的排班信息
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <param name="time">日期</param>
        /// <returns>返回数据集</returns>
        public DataTable GetWorkSchedulingByWorkIDAndDate(string workID,DateTime time)
        {
            string sql = @"select beginDate,EndDate,workID,b.date,b.Code,c.name,isholiday,endOffSetDays,beginTime,endTime," +
                          " punchinbegintime,punchinendtime,punchoutbegintime,punchoutendtime"+
                          " from dbo.HR_WorkScheduling a"+
                          " left join dbo.HR_WorkSchedulingDetail b on a.ID=b.ParentID"+
                          " left join HR_WorkSchedulingDefinition c on c.code=b.code"+
                          " where workID='"+workID+"' and date='"+time+"'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);
            return dt;
        }

        /// <summary>
        /// 获取指定单据号的排班信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>成功返回获取到的排班对象，失败返回null</returns>
        public View_HR_WorkScheduling GetWorkSchedulingByBillNo(int billNo)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from a in dataContxt.View_HR_WorkScheduling
                             where a.单据号 == billNo
                             select a;

            if (result.Count() == 1)
            {
                return result.Single();
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// 判断申请人在该月是否已经申请了排班信息
        /// </summary>
        /// <param name="workID">申请人编号</param>
        /// <param name="month">月份</param>
        /// <returns>存在返回true，不存在返回false</returns>
        public bool IsExise(string workID,int month)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from a in dataContxt.View_HR_WorkScheduling
                         where a.员工编号 == workID && a.排班月份 == month && a.排班年份 == ServerTime.Time.Year
                         select a;

            if (result.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 新增排班信息
        /// </summary>
        /// <param name="schedule">排班信息主信息</param>
        /// <param name="personnel">排班人员</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回新增的单据编号，失败返回-1</returns>
        public int AddWorkScheduling(HR_WorkScheduling schedule, List<HR_WorkSchedulingDetail> personnel, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                dataContxt.HR_WorkScheduling.InsertOnSubmit(schedule);
                dataContxt.SubmitChanges();

                int billID = -1;

                var result = from a in dataContxt.HR_WorkScheduling
                             where a.CreaterWorkID == schedule.CreaterWorkID && a.Year == schedule.Year
                             && a.CreateDate == schedule.CreateDate
                             select a;

                if (result.Count() == 1)
                {
                    billID = result.Single().ID;

                    foreach (var item in personnel)
                    {
                        HR_WorkSchedulingDetail personnleList = item;

                        personnleList.ParentID = billID;
                        personnleList.WorkID = item.WorkID;
                        personnleList.Code = item.Code;
                        personnleList.Date = item.Date;
                        personnleList.Remark = "";

                        dataContxt.HR_WorkSchedulingDetail.InsertOnSubmit(personnleList);
                    }
                }

                dataContxt.SubmitChanges();
                return billID;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return -1;
            }
        }

        /// <summary>
        /// 编制人修改排班信息
        /// </summary>
        /// <param name="schedule">排班信息</param>
        /// <param name="personnel">排班人员</param>
        /// <param name="billID">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True失败返回False</returns>
        public bool UpdateWorkScheduling(HR_WorkScheduling schedule, List<HR_WorkSchedulingDetail> personnel, int billID, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var resultMain = from a in dataContxt.HR_WorkScheduling
                                 where a.ID == billID
                                 select a;

                if (resultMain.Count() != 1)
                {
                    error = "信息有误，请查证后再操作！";
                    return false;
                }
                else
                {
                    HR_WorkScheduling bill = resultMain.Single();

                    bill.BillStatus = schedule.BillStatus;
                    bill.CreateDate = schedule.CreateDate;
                    bill.BeginDate = schedule.BeginDate;
                    bill.EndDate = schedule.EndDate;
                    bill.ScheduleName = schedule.ScheduleName;
                    bill.CompletionDate = schedule.CompletionDate;
                    bill.Year = schedule.Year;
                    bill.Remark = schedule.Remark;
                    bill.CompletionDate = schedule.CompletionDate;
                    bill.Month = schedule.Month;
                    bill.PendingDate = schedule.PendingDate;
                }

                var result = from c in dataContxt.HR_WorkSchedulingDetail
                             where c.ParentID == billID
                             select c;

                dataContxt.HR_WorkSchedulingDetail.DeleteAllOnSubmit(result);

                foreach (var item in personnel)
                {
                    HR_WorkSchedulingDetail personnleList = item;

                    personnleList.ParentID = billID;
                    personnleList.Code = item.Code;
                    personnleList.WorkID = item.WorkID;
                    personnleList.Date = item.Date;
                    personnleList.Remark = "";

                    dataContxt.HR_WorkSchedulingDetail.InsertOnSubmit(personnleList);
                }

                dataContxt.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 通过单据号删除单据信息
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True失败返回False</returns>
        public bool DeleteWorkScheduling(int billID, out string error)
        {
            error = "";

            try
            {
                View_HR_WorkScheduling workTemp = GetWorkSchedulingByBillNo(billID);

                if (workTemp.单据状态 == "已完成" || workTemp.单据状态 == "等待下次排班")
                {
                    throw new Exception("单据无法删除");
                }

                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var resultPersonnel = from a in dataContxt.HR_WorkSchedulingDetail
                                      where a.ParentID == billID
                                      select a;

                dataContxt.HR_WorkSchedulingDetail.DeleteAllOnSubmit(resultPersonnel);

                var result = from c in dataContxt.HR_WorkScheduling
                             where c.ID == billID
                             select c;

                dataContxt.HR_WorkScheduling.DeleteAllOnSubmit(result);
                dataContxt.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 领导审核修改排班信息
        /// </summary>
        /// <param name="schedule">排班信息</param>
        /// <param name="role">角色（主管或负责人）</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True失败返回False</returns>
        public bool UpdateAuditingWorkScheduling(HR_WorkScheduling schedule, string role, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var resultMain = from a in dataContxt.HR_WorkScheduling
                                 where a.ID == schedule.ID
                                 select a;

                if (resultMain.Count() != 1)
                {
                    error = "信息有误，请查证后再操作！";
                    return false;
                }
                else
                {
                    HR_WorkScheduling bill = resultMain.Single();

                    switch (role)
                    {
                        case "主管":
                            bill.BillStatus = schedule.BillStatus;
                            bill.DeptDirector = schedule.DeptDirector;
                            bill.DeptDirectorSignatureDate = schedule.DeptDirectorSignatureDate;
                            bill.CompletionDate = schedule.DeptPrincipalSignatureDate;
                            break;
                        case "负责人":
                            bill.BillStatus = schedule.BillStatus;
                            bill.DeptPrincipal = schedule.DeptPrincipal;
                            bill.DeptPrincipalSignatureDate = schedule.DeptPrincipalSignatureDate;
                            break;
                        default:
                            break;
                    }
                }

                dataContxt.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 通过单据号获得排班人员的排班信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="numberOfPeople">成功则返回此排班单据中包含的排班人数，失败返回-1</param>
        /// <returns>成功返回数据集，失败返回null</returns>
        public List<View_HR_WorkSchedulingDetail> GetWorkSchedulingDetail(int billNo, out int numberOfPeople)
        {
            numberOfPeople = -1;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = (from r in dataContxt.View_HR_WorkSchedulingDetail
                    where r.单据号 == billNo
                    orderby r.员工编号, r.时间
                    select r
                    );

            if (result.Count() > 0)
            {
                numberOfPeople = (from r in result
                                  select r.员工编号).Distinct().Count();

                return result.ToList();
            }
            else
            {
                return null;
            }
        }
    }
}
