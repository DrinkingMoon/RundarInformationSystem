using System;
using ServerModule;
using GlobalObject;
using System.Data;
namespace Service_Project_Project
{
    public interface ITimesheets
    {
        DataTable GetSetPersonnelInfo();

        void DeletePeronnnel(string workID);

        void AddPersonnel(string workID);

        System.Data.DataTable GetInfo(string workID);

        void OperationInfo(CE_OperatorMode mode, Business_Project_Timesheets timesheets);

        bool IsOverTime(Business_Project_Timesheets timesheets);

        bool IsRepeat(Business_Project_Timesheets timesheets);
    }
}
