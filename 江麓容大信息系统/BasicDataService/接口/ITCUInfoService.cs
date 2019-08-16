using System;
using ServerModule;
using System.Collections.Generic;
using System.Data;

namespace Service_Project_Design
{
    public enum Program_Report
    {
        Program,
        Report
    }

    public interface ITCUInfoService : FlowControlService.IFlowBusinessService
    {
        void WriteTxtFile(string billNo, string filePath);

        void CheckVersion(Business_Project_TCU_SoftwareUpdate updateInfo);

        DataTable GetSoftWareVersionListInfo(string carModelNo);

        void UpdateFilePath(string billNo, Guid guid, Program_Report program_report);

        void SubmitInfo_TCUSoft(string billNo);

        DataTable GetCarModelInfo_Code(string code);

        Base_MotorFactoryInfo GetFactoryInfo(string factoryCode);

        DataTable GetTCU_CarModelInfo(int factoryID);

        System.Data.DataTable GetMotorFactoryTreeInfo();

        TCU_CarModelInfo_Tradition GetCarModelInfo_Tradition(string carModelNo);

        TCU_CarModelInfo GetCarModelInfo(string carModelNo);

        List<TCU_CarModelInfo_Software> GetSoftwareList(string carModeleNo);

        void SaveCarModelInfo(TCU_CarModelInfo carModelInfo, object modelInfo);

        Business_Project_TCU_SoftwareUpdate GetSingleInfo_TCUSoft(string billNo);

        List<View_Business_Project_TCU_SoftwareUpdate_DID> GetListDIDInfo(string billNo);

        void SaveInfo_TCUSoft(Business_Project_TCU_SoftwareUpdate updateInfo,
            List<View_Business_Project_TCU_SoftwareUpdate_DID> lstDID);

    }
}
