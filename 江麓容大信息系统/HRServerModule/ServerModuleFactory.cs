using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;

namespace Service_Peripheral_HR
{
    /// <summary>
    /// 管理类厂
    /// </summary>
    public class ServerModuleFactory
    {
        /// <summary>
        /// 用于保证服务组件实例的唯一性
        /// </summary>
        static Hashtable m_hashTable = null;

        /// <summary>
        /// 获取服务组件
        /// </summary>
        /// <returns>返回组件接口</returns>
        public static T GetServerModule<T>()
        {
            string name = typeof(T).ToString();
                
            m_hashTable = new Hashtable();

            //if (m_hashTable.ContainsKey(name))
            //{
            //    return (T)m_hashTable[name];
            //}

            if (typeof(T) == typeof(IOrganizationServer))
            {
                IOrganizationServer serverModule = new OrganizationServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IDeptTypeServer))
            {
                IDeptTypeServer serverModule = new DeptTypeServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IOperatingPostServer))
            {
                IOperatingPostServer serverModule = new OperatingPostServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IJobTitleServer))
            {
                IJobTitleServer serverModule = new JobTitleServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IJobTitleServer))
            {
                IJobTitleServer serverModule = new JobTitleServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IResumeServer))
            {
                IResumeServer serverModule = new ResumeServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IPersonnelArchiveServer))
            {
                IPersonnelArchiveServer serverModule = new PersonnelArchiveServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(ILaborContractServer))
            {
                ILaborContractServer serverModule = new LaborContractServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IPostChangeServer))
            {
                IPostChangeServer serverModule = new PostChangeServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IDimissionServer))
            {
                IDimissionServer serverModule = new DimissionServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IOnBusinessBillServer))
            {
                IOnBusinessBillServer serverModule = new OnBusinessBillServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IHolidayServer))
            {
                IHolidayServer serverModule = new HolidayServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(ILeaveServer))
            {
                ILeaveServer serverModule = new LeaveServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IOverTimeBillServer))
            {
                IOverTimeBillServer serverModule = new OverTimeBillServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IWorkSchedulingServer))
            {
                IWorkSchedulingServer serverModule = new WorkSchedulingServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IAttendanceSchemeServer))
            {
                IAttendanceSchemeServer serverModule = new AttendanceSchemeServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IBatchExceptionServer))
            {
                IBatchExceptionServer serverModule = new BatchExceptionServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IAttendanceMachineServer))
            {
                IAttendanceMachineServer serverModule = new AttendanceMachineServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(ITimeExceptionServer))
            {
                ITimeExceptionServer serverModule = new TimeExceptionServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IAttendanceDaybookServer))
            {
                IAttendanceDaybookServer serverModule = new AttendanceDaybookServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IAttendanceSummaryServer))
            {
                IAttendanceSummaryServer serverModule = new AttendanceSummaryServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(ICultivateServer))
            {
                ICultivateServer serverModule = new CultivateServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(ITrainEmployeServer))
            {
                ITrainEmployeServer serverModule = new TrainEmployeServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IExpertEmployeServer))
            {
                IExpertEmployeServer serverModule = new ExpertEmployeServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IAttendanceAnalysis))
            {
                IAttendanceAnalysis serverModule = new AttendanceAnalysis();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(ITrainBasicInfo))
            {
                ITrainBasicInfo serverModule = new TrainBasicInfo();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(ITrainSurvey))
            {
                ITrainSurvey serverModule = new TrainSurvey();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(ITrainPlanCollect))
            {
                ITrainPlanCollect serverModule = new TrainPlanCollect();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(ITrainFeedback))
            {
                ITrainFeedback serverModule = new TrainFeedback();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(ITrainLearn))
            {
                ITrainLearn serverModule = new TrainLearn();
                m_hashTable.Add(name, serverModule);
            }

            if (m_hashTable.ContainsKey(name))
            {
                return (T)m_hashTable[name];
            }

            return default(T);
        }
    }
}
