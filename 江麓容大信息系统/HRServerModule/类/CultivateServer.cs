using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServerModule;
using PlatformManagement;

namespace Service_Peripheral_HR
{
    class CultivateServer : Service_Peripheral_HR.ICultivateServer
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
        /// 获取全部单据信息
        /// </summary>
        /// <param name="returnInfo">培训统计数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool GetAllBill(out IQueryResult returnInfo, out string error)
        {
            returnInfo = null;
            error = null;

            IAuthorization serverAuthorization = PlatformFactory.GetObject<IAuthorization>();
            IQueryResult qr = null;

            if (QueryResultFilter == null)
            {
                qr = serverAuthorization.Query("培训统计", null);
            }
            else
            {
                qr = serverAuthorization.Query("培训统计", null, QueryResultFilter);
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
        /// 获取时间段的单据信息
        /// </summary>
        /// <returns>返回培训统计数据集</returns>
        public List<View_HR_CultivateStatistics> GetBillByDate(DateTime startTime,DateTime endTime)
        {
            string strSql = "select * from View_HR_CultivateStatistics where "+
                            " 培训开始时间 >='"+startTime+"' and 培训终止时间 <='"+endTime+"' order by 培训开始时间";

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            return dataContxt.ExecuteQuery<View_HR_CultivateStatistics>(strSql, new object[] { }).ToList();
        }

        /// <summary>
        /// 通过id获得参与的人员
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>成功返回人员列表</returns>
        public List<HR_CultivateStatisticsPerson> GetPersonByID(int id)
        {
            string strSql = "select * from HR_CultivateStatisticsPerson";

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            return dataContxt.ExecuteQuery<HR_CultivateStatisticsPerson>(strSql, new object[] { }).ToList();
        }

        /// <summary>
        /// 新增一条培训统计的信息
        /// </summary>
        /// <param name="cultivate">培训统计数据集</param>
        /// <param name="list">人员列表</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true失败返回false</returns>
        public bool InsertBill(HR_CultivateStatistics cultivate,List<ServerModule.View_SelectPersonnel> list,out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_CultivateStatistics
                             where a.CourseName == cultivate.CourseName
                             && a.StartTime == cultivate.StartTime && a.EndTime == cultivate.EndTime
                             select a;

                if (result.Count() > 0)
                {
                    error = "数据重复，请检查数据";
                    return false;
                }

                dataContxt.HR_CultivateStatistics.InsertOnSubmit(cultivate);
                dataContxt.SubmitChanges();

                var resultList = from a in dataContxt.HR_CultivateStatistics
                             where a.CourseName == cultivate.CourseName
                             && a.StartTime == cultivate.StartTime && a.EndTime == cultivate.EndTime
                             select a;

                foreach (View_SelectPersonnel item in list)
                {
                    HR_CultivateStatisticsPerson person = new HR_CultivateStatisticsPerson();

                    person.BillID = resultList.Single().ID;
                    person.Person = item.员工姓名;

                    dataContxt.HR_CultivateStatisticsPerson.InsertOnSubmit(person);
                    dataContxt.SubmitChanges();
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
        /// 修改培训统计的信息
        /// </summary>
        /// <param name="id">需要修改的序号</param>
        /// <param name="cultivate">更新后的培训统计数据集</param>
        /// <param name="list">人员列表</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true失败返回false</returns>
        public bool UpdatetBill(int id, HR_CultivateStatistics cultivate, List<ServerModule.View_SelectPersonnel> list, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_CultivateStatistics
                             where a.ID == id
                             select a;

                if (result.Count() != 1)
                {
                    error = "数据有误，请检查数据";
                    return false;
                }

                HR_CultivateStatistics cultivateList = result.Single();

                cultivateList.CourseName = cultivate.CourseName;
                cultivateList.CultivateLecturer = cultivate.CultivateLecturer;
                cultivateList.CultivateType = cultivate.CultivateType;
                cultivateList.Dept = cultivate.Dept;
                cultivateList.EndTime = cultivate.EndTime;
                cultivateList.IsCourseware = cultivate.IsCourseware;
                cultivateList.StartTime = cultivate.StartTime;
                cultivateList.SumHours = cultivate.SumHours;
                cultivateList.IsWorkTime = cultivate.IsWorkTime;

                var resultList = from c in dataContxt.HR_CultivateStatisticsPerson
                                 where c.BillID == id
                                 select c;
                                
                dataContxt.HR_CultivateStatisticsPerson.DeleteAllOnSubmit(resultList);
                dataContxt.SubmitChanges();

                foreach (View_SelectPersonnel item in list)
                {
                    HR_CultivateStatisticsPerson person = new HR_CultivateStatisticsPerson();

                    person.BillID = id;
                    person.Person = item.员工姓名;

                    dataContxt.HR_CultivateStatisticsPerson.InsertOnSubmit(person);
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
        /// 删除培训统计的一条记录
        /// </summary>
        /// <param name="id">需要删除的序号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true，失败返回false</returns>
        public bool DeleteBill(int id,out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_CultivateStatistics
                             where a.ID == id
                             select a;

                var resultList = from c in dataContxt.HR_CultivateStatisticsPerson
                                 where c.BillID == id
                                 select c;

                if (result.Count() != 1)
                {
                    error = "数据有误，请检查数据";
                    return false;
                }

                dataContxt.HR_CultivateStatistics.DeleteAllOnSubmit(result);
                dataContxt.HR_CultivateStatisticsPerson.DeleteAllOnSubmit(resultList);
                dataContxt.SubmitChanges();

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
