using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServerModule;
using PlatformManagement;
using GlobalObject;
using System.Data;
using TaskServerModule;

namespace TaskManagementServer
{
    /// <summary>
    /// 为日常会议提供业务操作的服务
    /// </summary>
    public class MeetingServer : TaskManagementServer.IMeetingServer
    {
        /// <summary>
        /// 获取指定日期范围内的会议数据
        /// </summary>
        /// <param name="beginTime">会议开始时间，只取日期部分</param>
        /// <param name="endTime">会议结束时间，只取日期部分</param>
        /// <returns>返回获取到的数据</returns>
        public DataTable GetMeetingData(DateTime beginTime, DateTime endTime)
        {
            IAuthorization authorization = PlatformFactory.GetObject<IAuthorization>();
            IQueryResult qr = authorization.QueryMultParam("会议信息普通查询", null, new object[] { BasicInfo.LoginID });

            if (!qr.Succeeded)
            {
                throw new Exception(qr.Error);
            }
            else
            {
                DataTable dt = qr.DataCollection.Tables[0];
                DataTable dtTemp = dt.Clone();

                endTime = endTime.AddDays(1);

                DataRow[] dtRows = dt.Select(string.Format("开始时间 >= '{0}' and 结束时间 <= '{1}'", beginTime.Date, endTime.Date));

                if (dtRows != null && dtRows.Length > 0)
                {
                    for (int i = 0; i < dtRows.Length; i++)
                    {
                        dtTemp.ImportRow(dtRows[i]);
                    }
                }

                return dtTemp;
            }
        }

        /// <summary>
        /// 获取会议提示信息
        /// </summary>
        /// <returns>获取到的提示信息</returns>
        public string GetMeetingAlarmInfo()
        {
            TaskManagementDataContext ctx = CommentParameter.TaskManagementDataContext;

            List<string> lstMeetingID = new List<string>();

            var ids = (from r in ctx.PRJ_DailyWorkNotifyPeople
                       where r.WorkID == BasicInfo.LoginID
                       select r.DailyWorkID).Distinct();

            if (ids.Count() > 0)
            {
                lstMeetingID.AddRange(ids.ToArray());
            }

            List<View_PRJ_Meeting> lstMeeting = new List<View_PRJ_Meeting>();

            var result = from r in ctx.View_PRJ_Meeting
                         where ServerTime.Time > r.开始时间.AddMinutes(-r.提醒提前分钟数) && r.记录人工号 == BasicInfo.LoginID || r.主持人工号 == BasicInfo.LoginID && lstMeetingID.Contains(r.会议编号)
                         select r;

            if (result.Count() > 0)
            {
                lstMeeting.AddRange(result.ToArray());
            }

            return null;
        }

        /// <summary>
        /// 从表格行数据中提取会议对象
        /// </summary>
        /// <param name="row">数据行</param>
        /// <returns>返回获取到的会议对象</returns>
        public MeetingData GetMeetingData(DataRow row)
        {
            MeetingData data = new MeetingData();

            data.标题 = row["标题"].ToString();
            data.创建人工号 = row["创建人工号"].ToString();
            data.创建人姓名 = row["创建人姓名"].ToString();
            data.创建日期 = (DateTime)row["创建日期"];
            data.开始时间 = (DateTime)row["开始时间"];
            data.结束时间 = (DateTime)row["结束时间"];
            data.会议编号 = row["会议编号"].ToString();
            data.会议正文 = row["会议正文"].ToString();
            data.会议状态 = (MeetingStatus)Enum.Parse(typeof(MeetingStatus), row["会议状态"].ToString());
            data.会议资源 = row["会议资源"].ToString();
            data.记录人工号 = row["记录人工号"].ToString();
            data.记录人姓名 = row["记录人姓名"].ToString();
            data.主持人工号 = row["主持人工号"].ToString();
            data.主持人姓名 = row["主持人姓名"].ToString();
            data.事务类别 = DailyWorkType.会议;
            data.与会人员 = row["与会人员"].ToString();
            data.提醒方式 = (MeetingAlarmMode)Enum.Parse(typeof(MeetingAlarmMode), row["提醒方式"].ToString());
            data.提醒提前分钟数 = (int)row["提醒提前分钟数"];
            data.重要性 = (TaskImportance)Enum.Parse(typeof(TaskImportance), row["重要性"].ToString());

            TaskManagementDataContext ctx = CommentParameter.TaskManagementDataContext;

            var peoples = from r in ctx.PRJ_DailyWorkNotifyPeople
                          where r.DailyWorkID == data.会议编号
                          select r.WorkID;

            List<PersonnelBasicInfo> lstPersonnel = new List<PersonnelBasicInfo>();

            foreach (var item in peoples)
            {
                lstPersonnel.Add(new PersonnelBasicInfo { 工号 = item, 姓名 = UniversalFunction.GetPersonnelName(item) });
            }

            data.与会人员对象集 = lstPersonnel;

            data.会议资源对象集 = (from r in ctx.PRJ_ResourceUsageList
                            join a in ctx.View_PRJ_Resource on r.ResourceID equals a.资源编号
                            where r.TaskID == data.会议编号
                            select a).ToList();

            return data;
        }

        /// <summary>
        /// 保存会议信息
        /// </summary>
        /// <param name="info">要保存的数据</param>
        public void Save(MeetingData info)
        {
            if (ServerTime.Time > info.开始时间.AddMinutes(-10))
            {
                throw new Exception("会议开始时间必须在当前时间10分钟前");
            }

            TaskManagementDataContext ctx = CommentParameter.TaskManagementDataContext;

            var result = from r in ctx.PRJ_DailyWork
                         where r.ID == info.会议编号
                         select r;

            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();

            try
            {
                if (result.Count() > 0)
                {
                    Delete(ctx, info.会议编号);
                }

                PRJ_DailyWork dailyWork = new PRJ_DailyWork();

                DateTime date = ServerTime.Time;

                result = from r in ctx.PRJ_DailyWork
                         where r.TypeID == (int)DailyWorkType.会议 && r.ID.Substring(3, 4) == date.Year.ToString()
                         select r;

                int maxValue = 1;

                if (result.Count() > 0)
                {
                    maxValue += (from r in ctx.PRJ_DailyWork
                                 where r.TypeID == (int)DailyWorkType.会议 && r.ID.Substring(3, 4) == date.Year.ToString()
                                 select Convert.ToInt32(r.ID.Substring(9))).Max();
                }

                // HYX : 会议项
                dailyWork.ID = string.Format("HYX{0:D4}{1:D2}{2:D5}", date.Year, date.Month, maxValue);

                info.会议编号 = dailyWork.ID;

                dailyWork.ImportanceID = Convert.ToInt32(info.重要性);
                dailyWork.BeginTime = info.开始时间.AddSeconds(-info.开始时间.Second);
                dailyWork.EndTime = info.结束时间.AddSeconds(-info.结束时间.Second);
                dailyWork.Content = info.会议正文;
                dailyWork.Date = ServerTime.Time;
                dailyWork.NeedToReply = false;
                dailyWork.Status = MeetingStatus.待发.ToString();
                dailyWork.Title = info.标题;
                dailyWork.TypeID = (int)info.事务类别;   // 类别为：会议
                dailyWork.WorkID = BasicInfo.LoginID;

                ctx.PRJ_DailyWork.InsertOnSubmit(dailyWork);

                PRJ_MeetingExpandedInfo meetingExpandedInfo = new PRJ_MeetingExpandedInfo();

                meetingExpandedInfo.DailyWorkID = dailyWork.ID;
                meetingExpandedInfo.AlarmMode = info.提醒方式.ToString();
                meetingExpandedInfo.AlarmValue = info.提醒提前分钟数;
                meetingExpandedInfo.HostCode = info.主持人工号;
                meetingExpandedInfo.Recorder = info.记录人工号;

                ctx.PRJ_MeetingExpandedInfo.InsertOnSubmit(meetingExpandedInfo);

                // 设置与会人员
                if (info.与会人员对象集 != null)
                {
                    foreach (var item in info.与会人员对象集)
                    {
                        PRJ_DailyWorkNotifyPeople people = new PRJ_DailyWorkNotifyPeople();

                        people.WorkID = item.工号;
                        people.DailyWorkID = dailyWork.ID;

                        ctx.PRJ_DailyWorkNotifyPeople.InsertOnSubmit(people);
                    }
                }

                // 设置会议使用的资源
                if (info.会议资源对象集 != null)
                {
                    foreach (var item in info.会议资源对象集)
                    {
                        PRJ_ResourceUsageList rul = new PRJ_ResourceUsageList();

                        rul.ResourceID = (int)item.资源编号;
                        rul.TaskID = dailyWork.ID;
                        rul.BeginTime = dailyWork.BeginTime;
                        rul.EndTime = dailyWork.EndTime;
                        rul.Remark = "";

                        ctx.PRJ_ResourceUsageList.InsertOnSubmit(rul);
                    }
                }

                ctx.SubmitChanges();

                ctx.Transaction.Commit();

                info.会议状态 = MeetingStatus.待发;
            }
            catch (Exception exce)
            {
                ctx.Transaction.Rollback();
                throw exce;
            }
        }

        /// <summary>
        /// 更新会议状态
        /// </summary>
        /// <param name="meetingID">会议编号</param>
        /// <param name="status">会议状态</param>
        public void UpdateStatus(string meetingID, MeetingStatus status)
        {
            TaskManagementDataContext ctx = CommentParameter.TaskManagementDataContext;

            var result = from r in ctx.PRJ_DailyWork
                         where r.ID == meetingID
                         select r;

            if (result.Count() == 1)
            {
                PRJ_DailyWork dailyWork = result.Single();

                if (status == MeetingStatus.撤销 && ServerTime.Time > dailyWork.BeginTime.AddMinutes(-20))
                {
                    throw new Exception("已经过了撤销时间，必须在会议开始时间20分钟前撤销");
                }
                else if (status == MeetingStatus.已发 && ServerTime.Time > dailyWork.BeginTime.AddMinutes(-20))
                {
                    throw new Exception("已经过了发布时间，必须在会议开始时间20分钟前发布");
                }

                dailyWork.Status = status.ToString();

                if (status == MeetingStatus.撤销)
                {
                    DeleteAlarmInfo(ctx, dailyWork);
                }
                else if (status == MeetingStatus.已发)
                {
                    string error = null;

                    if (!CheckMeeting(dailyWork, out error))
                    {
                        throw new Exception(error);
                    }

                    AddAlarmInfo(ctx, dailyWork);
                }

                ctx.SubmitChanges();
            }
            else
            {
                throw new Exception("您还没有保存会议信息，请保存会议信息后再发布");
            }
        }

        /// <summary>
        /// 删除指定的会议信息
        /// </summary>
        /// <param name="ctx">数据库操作上下文</param>
        /// <param name="meetingID">会议编号</param>
        private void Delete(TaskManagementDataContext ctx, string meetingID)
        {
            var result = from r in ctx.PRJ_DailyWork
                         where r.ID == meetingID
                         select r;

            if (result.Count() == 1)
            {
                // 删除日常事务信息
                PRJ_DailyWork dailyWork = result.Single();

                if (dailyWork.WorkID != BasicInfo.LoginID)
                {
                    throw new Exception("您不是会议的创建者不能进行此操作");
                }

                if (dailyWork.Status == MeetingStatus.已发.ToString() && dailyWork.BeginTime < ServerTime.Time.AddMinutes(20))
                {
                    throw new Exception("您不能删除会议开始时间在当前时间20分钟前的会议，会议信息已经生效");
                }

                ctx.PRJ_DailyWork.DeleteAllOnSubmit(result);

                #region 数据库中会进行级联删除

                //// 删除会议扩展信息
                //var meetingExpandedInfo = from r in ctx.PRJ_MeetingExpandedInfo
                //                          where r.DailyWorkID == meetingID
                //                          select r;

                //ctx.PRJ_MeetingExpandedInfo.DeleteAllOnSubmit(meetingExpandedInfo);

                //// 删除参与人员信息
                //var people = from r in ctx.PRJ_DailyWorkNotifyPeople
                //             where r.DailyWorkID == meetingID
                //             select r;

                //ctx.PRJ_DailyWorkNotifyPeople.DeleteAllOnSubmit(people);

                #endregion

                var rul = from r in ctx.PRJ_ResourceUsageList
                          where r.TaskID == meetingID
                          select r;

                ctx.PRJ_ResourceUsageList.DeleteAllOnSubmit(rul);

                // 删除会议提醒信息
                DeleteAlarmInfo(ctx, dailyWork);

                ctx.SubmitChanges();
            }
        }

        /// <summary>
        /// 添加提醒信息
        /// </summary>
        /// <param name="ctx">数据库操作上下文</param>
        /// <param name="dailyWork">会议信息</param>
        private void AddAlarmInfo(TaskManagementDataContext ctx, PRJ_DailyWork dailyWork)
        {
            View_PRJ_Meeting meeting = (from r in ctx.View_PRJ_Meeting
                                        where r.会议编号 == dailyWork.ID
                                        select r).Single();

            // 会议地址
            string address = (from r in ctx.View_PRJ_Resource
                              join a in ctx.PRJ_ResourceUsageList on r.资源编号 equals a.ResourceID
                              where r.资源类别名称 == "会议室" && a.TaskID == dailyWork.ID
                              select r.资源名称).First();

            // 报警提示信息
            IWarningNotice warningNoticeServer = PlatformFactory.GetObject<IWarningNotice>();

            List<string> lstAlarmUser = new List<string>();

            lstAlarmUser.Add(meeting.记录人工号);
            lstAlarmUser.Add(meeting.主持人工号);

            var peoples = (from r in ctx.PRJ_DailyWorkNotifyPeople
                           where r.DailyWorkID == dailyWork.ID
                           select r.WorkID).ToList();

            if (peoples != null && peoples.Count > 0)
            {
                lstAlarmUser.AddRange(peoples);
            }

            lstAlarmUser = lstAlarmUser.Distinct().ToList();

            // 提醒设置
            foreach (var user in lstAlarmUser)
            {
                if (meeting.提醒方式 == MeetingAlarmMode.短信及消息框提醒.ToString() || meeting.提醒方式 == MeetingAlarmMode.仅短信提醒.ToString())
                {
                    PRJ_TaskAlarmSetting alarmSetting = new PRJ_TaskAlarmSetting();

                    alarmSetting.AlarmUser = user;
                    alarmSetting.ItemID = dailyWork.ID;
                    alarmSetting.AlarmModeID = (int)Enum.Parse(typeof(MeetingAlarmMode), meeting.提醒方式);
                    alarmSetting.AlarmValue = meeting.开始时间.AddMinutes(-meeting.提醒提前分钟数).ToString("yyyy-MM-dd HH:mm");

                    alarmSetting.AlarmContent = string.Format("请于({0})在({1})开会,主题：{2}，主持人：{3}",
                        dailyWork.BeginTime.ToString("MM-dd HH:mm"), address, dailyWork.Title, meeting.主持人姓名);

                    alarmSetting.RecordTime = dailyWork.Date;

                    ctx.PRJ_TaskAlarmSetting.InsertOnSubmit(alarmSetting);
                }

                if (meeting.提醒方式 == MeetingAlarmMode.短信及消息框提醒.ToString() || meeting.提醒方式 == MeetingAlarmMode.仅消息框提醒.ToString())
                {
                    Flow_WarningNotice notice = new Flow_WarningNotice();

                    if (meeting.重要性 != TaskImportance.普通.ToString())
                        notice.优先级 = "高";
                    else
                        notice.优先级 = "低";

                    notice.来源 = "会议管理系统";

                    View_Auth_User userInfo = PlatformFactory.GetUserManagement().GetUser(meeting.创建人工号);

                    notice.状态 = "未读";
                    notice.发送方 = string.Format("{0},{1}", PlatformFactory.GetDeptManagement().GetDepartment(userInfo.部门).部门名称, userInfo.姓名);

                    notice.接收方类型 = "用户";
                    notice.接收方 = user;
                    notice.标题 = dailyWork.Title;
                    notice.内容 = string.Format("请您于({0})在({1})召开会议,主持人：{2},会议正文：{3}",
                        dailyWork.BeginTime.ToString("yyyy-MM-dd HH:mm"), address, meeting.主持人姓名, dailyWork.Content);
                    notice.附加信息1 = "会议管理";
                    notice.附加信息2 = dailyWork.ID;
                    notice.附加信息3 = dailyWork.BeginTime.ToString();
                    notice.附加信息4 = dailyWork.EndTime.ToString();

                    warningNoticeServer.SendWarningNotice(notice);
                }
            }
        }

        /// <summary>
        /// 删除会议提醒信息
        /// </summary>
        /// <param name="ctx">数据库操作上下文</param>
        /// <param name="dailyWork">要删除提醒信息对应的日常事务</param>
        private void DeleteAlarmInfo(TaskManagementDataContext ctx, PRJ_DailyWork dailyWork)
        {
            // 删除任务提醒
            var alarmSetting = from r in ctx.PRJ_TaskAlarmSetting
                               where r.ItemID == dailyWork.ID
                               select r;

            ctx.PRJ_TaskAlarmSetting.DeleteAllOnSubmit(alarmSetting);

            #region 清除报警提示信息

            IWarningNotice warningNoticeServer = PlatformFactory.GetObject<IWarningNotice>();

            Dictionary<string, string> dicParam = new Dictionary<string, string>();

            dicParam.Add("附加信息2", dailyWork.ID);

            List<Flow_WarningNotice> notices = warningNoticeServer.GetWarningNotice(dicParam);

            if (notices != null)
            {
                foreach (var item in notices)
                {
                    warningNoticeServer.DeleteWarningNotice(item.序号);
                }
            }

            #endregion
        }

        /// <summary>
        /// 删除指定的会议信息
        /// </summary>
        /// <param name="meetingID">会议编号</param>
        public void Delete(string meetingID)
        {
            Delete(CommentParameter.TaskManagementDataContext, meetingID);
        }

        /// <summary>
        /// 设置会议实际完成时间
        /// </summary>
        /// <param name="meetingId">会议编号</param>
        /// <param name="realTime">实际完成时间</param>
        public void SetRealFinishedTime(string meetingId, DateTime realTime)
        {
            TaskManagementDataContext ctx = CommentParameter.TaskManagementDataContext;

            var result = from r in ctx.PRJ_DailyWork
                         where r.ID == meetingId
                         select r;

            if (result.Count() > 0)
            {
                PRJ_DailyWork meeting = result.Single();

                if (meeting.WorkID == BasicInfo.LoginID)
                {
                    if (realTime.Date != meeting.EndTime.Date || realTime <= meeting.BeginTime)
                    {
                        throw new Exception("设置的完成时间不正确");
                    }

                    if (ServerTime.Time.Date > meeting.EndTime.Date)
                    {
                        throw new Exception("不允许在会议结束一天后再修改会议完成时间");
                    }

                    meeting.EndTime = realTime;
                    ctx.SubmitChanges();
                }
                else
                {
                    throw new Exception("此会议不是您创建的您无权修改");
                }
            }
        }

        /// <summary>
        /// 检查会议参与人员是否有时间参加会议
        /// </summary>
        /// <param name="personnels">与会人员工号列表</param>
        /// <param name="beginTime">会议开始时间</param>
        /// <param name="endTime">会议结束时间</param>
        /// <param name="error">错误时输出错误信息</param>
        /// <returns>检查是否通过的标志</returns>
        private bool CheckParticipants(List<string> personnels, DateTime beginTime, DateTime endTime, out string error)
        {
            error = null;

            TaskManagementDataContext ctx = CommentParameter.TaskManagementDataContext;

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("下列人员无法参加会议：");

            // 存在出差人员
            bool hasInTheBusinessTrip = false;
            int index = 0;

            for (int i = 0; i < personnels.Count; i++)
            {
                if ((bool)ctx.Fun_InTheBusinessTrip(personnels[i], beginTime, endTime))
                {
                    hasInTheBusinessTrip = true;
                    sb.AppendLine(string.Format("{0}. {1} 在 会议期间出差；", ++index, UniversalFunction.GetPersonnelName(personnels[i])));
                }
            }

            var result = (from r in ctx.PRJ_DailyWork
                          join a in ctx.PRJ_DailyWorkNotifyPeople on r.ID equals a.DailyWorkID
                          where r.Status == MeetingStatus.已发.ToString() &&
                                ((beginTime >= r.BeginTime && beginTime <= r.EndTime) ||
                                (endTime >= r.BeginTime && endTime <= r.EndTime))
                          select new { r.BeginTime, r.EndTime, a.WorkID }).ToList();

            result = result.FindAll(f => personnels.Contains(f.WorkID));

            if (result.Count == 0)
            {
                if (hasInTheBusinessTrip)
                {
                    error = sb.ToString();
                    return false;
                }
                else
                {
                    return true;
                }
            }

            for (int i = 0; i < result.Count; i++)
            {
                sb.AppendLine(string.Format("{0}. {1} 在 {2} 至 {3} 时间段已经有会议要参加；",
                    ++index, UniversalFunction.GetPersonnelName(result[i].WorkID), result[i].BeginTime, result[i].EndTime));
            }

            error = sb.ToString();
            return false;
        }

        /// <summary>
        /// 检查会议参与人员是否有时间参加会议
        /// </summary>
        /// <param name="personnels">与会人员</param>
        /// <param name="beginTime">会议开始时间</param>
        /// <param name="endTime">会议结束时间</param>
        /// <param name="error">错误时输出错误信息</param>
        /// <returns>检查是否通过的标志</returns>
        public bool CheckParticipants(List<PersonnelBasicInfo> personnels, DateTime beginTime, DateTime endTime, out string error)
        {
            var workIds = (from r in personnels select r.工号).ToList();

            return CheckParticipants(workIds, beginTime, endTime, out error);
        }

        /// <summary>
        /// 检查会议参与人员是否有时间参加会议已经会议室等资源是否被占用
        /// </summary>
        /// <param name="dailyWork">会议信息</param>
        /// <param name="error">错误时输出错误信息</param>
        /// <returns>检查是否通过的标志</returns>
        private bool CheckMeeting(PRJ_DailyWork dailyWork, out string error)
        {
            error = null;

            if (!CheckMeetingResource(dailyWork, out error))
            {
                return false;
            }

            var workIds = (from r in CommentParameter.TaskManagementDataContext.PRJ_DailyWorkNotifyPeople
                           where r.DailyWorkID == dailyWork.ID
                           select r.WorkID).ToList();


            return CheckParticipants(workIds, dailyWork.BeginTime, dailyWork.EndTime, out error);
        }

        /// <summary>
        /// 检查会议室等资源是否被占用
        /// </summary>
        /// <param name="dailyWork">会议信息</param>
        /// <param name="error">错误时输出错误信息</param>
        /// <returns>检查是否通过的标志</returns>
        private bool CheckMeetingResource(PRJ_DailyWork dailyWork, out string error)
        {
            error = null;

            TaskManagementDataContext ctx = CommentParameter.TaskManagementDataContext;

            var resourceID = (from r in ctx.PRJ_ResourceUsageList
                              where r.TaskID == dailyWork.ID
                              select r.ResourceID).ToList();

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("下列会议资源已经被占用，当前会议无法使用：");

            int i = 0;
            IResourceServer resourceServer = TaskObjectFactory.GetOperator<IResourceServer>();
            List<View_PRJ_Resource> resources = resourceServer.GetMeetingResource(dailyWork.BeginTime, dailyWork.EndTime);

            foreach (var item in resourceID)
            {
                if (!resourceServer.IsIdle(item, dailyWork.BeginTime, dailyWork.EndTime))
                {
                    sb.AppendLine(string.Format("{0}. {1}；", ++i, resources.Find(p => p.资源编号 == item).资源名称));
                }
            }

            if (i > 0)
            {
                error = sb.ToString();
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
