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
using UniversalControlLibrary;

namespace Service_Project_Design
{
    class TCUInfoService : ITCUInfoService
    {
        public DataTable GetCarModelInfo_Code(string code)
        {

            string strSql = " select * from View_TCU_CarModelInfo where 1=1 ";

            if (code != null)
            {
                int result = 0;
                Int32.TryParse(code, out result);

                if (result != 0)
                {
                    strSql += " and 整车厂ID = " + result;
                }
                else if (code.Trim().Length > 0)
                {
                    strSql += " and SUBSTRING( UPPER(主机厂编码), 1, " + code.Trim().Length.ToString() + ") = '" + code + "'";
                }
            }

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        public DataTable GetMotorFactoryTreeInfo()
        {
            string error = null;

            return GlobalObject.DatabaseServer.QueryInfoPro("MotorFactoryInfo_Tree", null, out error);
        }

        public Base_MotorFactoryInfo GetFactoryInfo(string factoryCode)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Base_MotorFactoryInfo
                          where a.FactoryCode == factoryCode
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

        public DataTable GetTCU_CarModelInfo(int factoryID)
        {
            string strSql = "select * from View_TCU_CarModelInfo where 主机厂ID = " + factoryID;

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        public TCU_CarModelInfo_Tradition GetCarModelInfo_Tradition(string carModelNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            if (carModelNo == null)
            {
                return null;
            }

            var varData = from a in ctx.TCU_CarModelInfo_Tradition
                          where a.CarModelNo == carModelNo
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

        public TCU_CarModelInfo GetCarModelInfo(string carModelNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            if (carModelNo == null)
            {
                return null;
            }

            var varData = from a in ctx.TCU_CarModelInfo
                          where a.CarModelNo == carModelNo
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

        public List<TCU_CarModelInfo_Software> GetSoftwareList(string carModeleNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            if (carModeleNo == null)
            {
                return null;
            }

            var varData = from a in ctx.TCU_CarModelInfo_Software
                          where a.CarModelNo == carModeleNo
                          select a;

            return varData.ToList();
        }

        public void SaveCarModelInfo(TCU_CarModelInfo carModelInfo, object modelInfo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();

            try
            {
                if (carModelInfo == null || modelInfo == null)
                {
                    throw new Exception("内容为空，无法执行保存");
                }

                var varData1 = from a in ctx.TCU_CarModelInfo
                              where a.CarModelNo == carModelInfo.CarModelNo
                              select a;

                if (varData1.Count() == 0)
                {
                    ctx.TCU_CarModelInfo.InsertOnSubmit(carModelInfo);
                }
                else if(varData1.Count() == 1)
                {
                    varData1.Single().IsOff = carModelInfo.IsOff;

                    if (varData1.Single().DLLFileUnique != null && varData1.Single().DLLFileUnique != carModelInfo.DLLFileUnique)
                    {
                        UniversalControlLibrary.FileOperationService.File_Delete((Guid)varData1.Single().DLLFileUnique,
                            GeneralFunction.StringConvertToEnum<CE_CommunicationMode>(BasicInfo.BaseSwitchInfo[(int)CE_SwitchName.文件传输方式]));
                    }

                    varData1.Single().DLLFileUnique = carModelInfo.DLLFileUnique;
                    varData1.Single().DLLName = carModelInfo.DLLName;
                }

                ctx.SubmitChanges();

                CE_CarModelType type = GlobalObject.GeneralFunction.StringConvertToEnum<CE_CarModelType>(carModelInfo.CarModelType);

                switch (type)
                {
                    case CE_CarModelType.传统车型:

                        TCU_CarModelInfo_Tradition tempInfo = modelInfo as TCU_CarModelInfo_Tradition;

                        var varData = from a in ctx.TCU_CarModelInfo_Tradition
                                      where a.CarModelNo == tempInfo.CarModelNo
                                      select a;

                        if (varData.Count() == 0)
                        {
                            tempInfo.RecordDate = ServerTime.Time;

                            ctx.TCU_CarModelInfo_Tradition.InsertOnSubmit(tempInfo);
                        }
                        else if (varData.Count() == 1)
                        {
                            TCU_CarModelInfo_Tradition tempSingle = varData.Single();

                            tempSingle.ABS = tempInfo.ABS;
                            tempSingle.Allods = tempInfo.Allods;
                            tempSingle.CarModelNo = tempInfo.CarModelNo;
                            tempSingle.CruiseControl = tempInfo.CruiseControl;
                            tempSingle.CVTModel = tempInfo.CVTModel;
                            tempSingle.Diagnostics = tempInfo.Diagnostics;
                            tempSingle.EMSProvider = tempInfo.EMSProvider;
                            tempSingle.EngineCC = tempInfo.EngineCC;
                            tempSingle.EngineModel = tempInfo.EngineModel;
                            tempSingle.EPB = tempInfo.EPB;
                            tempSingle.ESP = tempInfo.ESP;
                            tempSingle.HHC = tempInfo.HHC;
                            tempSingle.ModelName = tempInfo.ModelName;
                            tempSingle.QRCode_FactoryCode = tempInfo.QRCode_FactoryCode;
                            tempSingle.QRCode_PartsCode = tempInfo.QRCode_PartsCode;
                            tempSingle.QRCode_PartsType = tempInfo.QRCode_PartsType;
                            tempSingle.QRCode_Provider = tempInfo.QRCode_Provider;
                            tempSingle.RecordDate = ServerTime.Time;
                            tempSingle.StartAndStop = tempInfo.StartAndStop;
                            tempSingle.TCUProvider = tempInfo.TCUProvider;
                            tempSingle.TireSize = tempInfo.TireSize;
                            tempSingle.UseArea = tempInfo.UseArea;
                            tempSingle.SnowMode = tempInfo.SnowMode;
                        }
                        else
                        {
                            throw new Exception("车型代码不唯一");
                        }

                        ctx.SubmitChanges();
                        break;
                    case CE_CarModelType.新能源车型:
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

        public void DeleteInfo(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            try
            {
                IFlowServer serviceFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();

                var varData = from a in ctx.Business_Project_TCU_SoftwareUpdate_DID
                              where a.BillNo == billNo
                              select a;

                ctx.Business_Project_TCU_SoftwareUpdate_DID.DeleteAllOnSubmit(varData);

                var varData1 = from a in ctx.Business_Project_TCU_SoftwareUpdate
                               where a.BillNo == billNo
                               select a;

                if (varData1.Count() == 1)
                {
                    Business_Project_TCU_SoftwareUpdate tempInfo = varData1.Single();

                    UniversalControlLibrary.FileOperationService.File_Delete(tempInfo.ProgramUnique,
                        GlobalObject.GeneralFunction.StringConvertToEnum<CE_CommunicationMode>
                        (BasicInfo.BaseSwitchInfo[(int)CE_SwitchName.文件传输方式]));

                    if (tempInfo.TestReport != null)
                    {
                        UniversalControlLibrary.FileOperationService.File_Delete((Guid)tempInfo.TestReport,
                            GlobalObject.GeneralFunction.StringConvertToEnum<CE_CommunicationMode>
                            (BasicInfo.BaseSwitchInfo[(int)CE_SwitchName.文件传输方式]));
                    }

                    ctx.Business_Project_TCU_SoftwareUpdate.DeleteAllOnSubmit(varData1);
                }

                serviceFlow.FlowDelete(ctx, billNo);
                ctx.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Business_Project_TCU_SoftwareUpdate GetSingleInfo_TCUSoft(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Business_Project_TCU_SoftwareUpdate
                          where a.BillNo == billNo
                          select a;

            if (varData.Count() == 1)
            {
                return varData.Single();
            }

            return null;
        }

        public List<View_Business_Project_TCU_SoftwareUpdate_DID> GetListDIDInfo(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.View_Business_Project_TCU_SoftwareUpdate_DID
                          where a.BillNo == billNo
                          select a;

            return varData.ToList();
        }

        public void SaveInfo_TCUSoft(Business_Project_TCU_SoftwareUpdate updateInfo,
            List<View_Business_Project_TCU_SoftwareUpdate_DID> lstDID)
        {
            FlowControlService.IFlowServer serviceFlow = 
                FlowControlService.ServerModuleFactory.GetServerModule<FlowControlService.IFlowServer>();
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            Flow_FlowInfo flow =
                serviceFlow.GetNowFlowInfo(serviceFlow.GetBusinessTypeID(CE_BillTypeEnum.TCU软件升级, null), updateInfo.BillNo);

            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();

            try
            {
                if (updateInfo == null || updateInfo.BillNo == null)
                {
                    return;
                }

                var varData = from a in ctx.Business_Project_TCU_SoftwareUpdate
                              where a.BillNo == updateInfo.BillNo
                              select a;

                Business_Project_TCU_SoftwareUpdate tempInfo = new Business_Project_TCU_SoftwareUpdate();

                if (varData.Count() == 0)
                {
                    tempInfo.BillNo = updateInfo.BillNo;
                    tempInfo.CarModelNo = updateInfo.CarModelNo;
                    tempInfo.ClientVersion = updateInfo.ClientVersion;
                    tempInfo.DiagnosisVersion = updateInfo.DiagnosisVersion;
                    tempInfo.HardwareVersion = updateInfo.HardwareVersion;
                    tempInfo.ProgramUnique = updateInfo.ProgramUnique;
                    tempInfo.TechnicalNote = updateInfo.TechnicalNote;
                    tempInfo.UnderVersion = updateInfo.UnderVersion;
                    tempInfo.UpdateContent = updateInfo.UpdateContent;
                    tempInfo.UpdateReason = updateInfo.UpdateReason;
                    tempInfo.Version = updateInfo.Version;

                    ctx.Business_Project_TCU_SoftwareUpdate.InsertOnSubmit(tempInfo);
                }
                else if (varData.Count() == 1)
                {
                    tempInfo = varData.Single();

                    switch (flow.FlowID)
                    {
                        case 70:
                            tempInfo.CarModelNo = updateInfo.CarModelNo;
                            tempInfo.ClientVersion = updateInfo.ClientVersion;
                            tempInfo.DiagnosisVersion = updateInfo.DiagnosisVersion;
                            tempInfo.HardwareVersion = updateInfo.HardwareVersion;
                            tempInfo.ProgramUnique = updateInfo.ProgramUnique;
                            tempInfo.TechnicalNote = updateInfo.TechnicalNote;
                            tempInfo.UnderVersion = updateInfo.UnderVersion;
                            tempInfo.UpdateContent = updateInfo.UpdateContent;
                            tempInfo.UpdateReason = updateInfo.UpdateReason;
                            tempInfo.Version = updateInfo.Version;
                            break;
                        case 72:
                            tempInfo.IsPassTest = updateInfo.IsPassTest;
                            tempInfo.TestReport = updateInfo.TestReport;
                            tempInfo.TestResult = updateInfo.TestResult;
                            break;
                        default:
                            break;
                    }
                }

                ctx.SubmitChanges();

                if (lstDID != null && serviceFlow.GetNowBillStatus(updateInfo.BillNo) == CE_CommonBillStatus.新建单据.ToString())
                {
                    var varData1 = from a in ctx.Business_Project_TCU_SoftwareUpdate_DID
                                   where a.BillNo == updateInfo.BillNo
                                   select a;

                    ctx.Business_Project_TCU_SoftwareUpdate_DID.DeleteAllOnSubmit(varData1);
                    ctx.SubmitChanges();

                    foreach (View_Business_Project_TCU_SoftwareUpdate_DID item in lstDID)
                    {
                        Business_Project_TCU_SoftwareUpdate_DID tempDID = new Business_Project_TCU_SoftwareUpdate_DID();

                        tempDID.BillNo = updateInfo.BillNo;
                        tempDID.DID = item.DID;
                        tempDID.DataContent = item.内容;
                        tempDID.DataSize = item.字节数;

                        ctx.Business_Project_TCU_SoftwareUpdate_DID.InsertOnSubmit(tempDID);
                    }
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

        public void SubmitInfo_TCUSoft(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            FlowControlService.IFlowServer serviceFlow = 
                FlowControlService.ServerModuleFactory.GetServerModule<FlowControlService.IFlowServer>();

            if (serviceFlow.GetNextBillStatus(billNo) != CE_CommonBillStatus.单据完成.ToString())
            {
                return;
            }

            if (billNo == null)
            {
                throw new Exception("业务单号无效");
            }

            var varData = from a in ctx.Business_Project_TCU_SoftwareUpdate
                          where a.BillNo == billNo
                          select a;

            if (varData.Count() != 1)
            {
                throw new Exception("业务单号无效");
            }

            if (varData.Single().IsPassTest == null || !((bool)varData.Single().IsPassTest))
            {
                return;
            }

            var varData1 = from a in ctx.TCU_CarModelInfo_Software
                           where a.CarModelNo == varData.Single().CarModelNo
                           && a.RelateBillNo == billNo
                           select a;

            if (varData1.Count() > 0)
            {
                return;
            }

            TCU_CarModelInfo_Software software = new TCU_CarModelInfo_Software();

            software.CarModelNo = varData.Single().CarModelNo;
            software.RelateBillNo = billNo;

            ctx.TCU_CarModelInfo_Software.InsertOnSubmit(software);
            ctx.SubmitChanges();
        }

        public void UpdateFilePath(string billNo, Guid guid, Program_Report program_report)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Business_Project_TCU_SoftwareUpdate
                          where a.BillNo == billNo
                          select a;

            if (varData.Count() == 1)
            {
                Business_Project_TCU_SoftwareUpdate temp = varData.Single();

                if (program_report == Program_Report.Program)
                {
                    temp.ProgramUnique = guid;
                }
                else if (program_report == Program_Report.Report)
                {
                    temp.TestReport = guid;
                }

                ctx.SubmitChanges();
            }
        }

        public bool IsExist(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            return IsExist(ctx, billNo);
        }

        public bool IsExist(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.Business_Project_TCU_SoftwareUpdate
                          where a.BillNo == billNo
                          select a;

            if (varData.Count() == 1)
            {
                return true;
            }

            return false;
        }

        public void CheckVersion(Business_Project_TCU_SoftwareUpdate updateInfo)
        {
            string error = null;

            try
            {
                if (updateInfo == null
                    || updateInfo.BillNo == null
                    || updateInfo.CarModelNo == null
                    || updateInfo.Version == null
                    || updateInfo.Version.ToString().Length == 0)
                {
                    return;
                }

                DataTable tempTable = GetSoftWareVersionListInfo(updateInfo.CarModelNo);

                if (tempTable == null || tempTable.Rows.Count == 0)
                {
                    return;
                }

                tempTable = DataSetHelper.OrderBy(
                    DataSetHelper.SiftDataTable(tempTable, "业务编号 <> '" + updateInfo.BillNo + "'", out error), "版本号");

                if (tempTable.Rows.Count == 0)
                {
                    return;
                }

                DataTable tempTable1 = DataSetHelper.SiftDataTable(tempTable, "版本状态 = '测试版'", out error);

                if (tempTable1.Rows.Count > 0)
                {
                    throw new Exception("当前车型代号已有【测试版】，无法再添加");
                }

                string str1 = tempTable.Rows[tempTable.Rows.Count - 1]["版本号"].ToString();
                string str2 = updateInfo.Version;

                if (string.Compare(str1, str2) >= 0)
                {
                    throw new Exception("【测试版】版本号需大于【正式版】版本号");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable GetSoftWareVersionListInfo(string carModelNo)
        {
            string error = null;

            try
            {
                Hashtable hsTable = new Hashtable();
                hsTable.Add("@CarModelNo", carModelNo);

                DataTable dtTemp = DatabaseServer.QueryInfoPro("TCU_CarModelInfo_Software_GetList", hsTable, out error);

                return dtTemp;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void WriteTxtFile(string billNo, string filePath)
        {
            if (billNo == null)
            {
                return;
            }

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            try
            {
                var varData = from a in ctx.Business_Project_TCU_SoftwareUpdate
                              where a.BillNo == billNo
                              select a;

                if (varData.Count() != 1)
                {
                    return;
                }

                var varCarMolde = from a in ctx.TCU_CarModelInfo_Tradition
                                  where a.CarModelNo == varData.Single().CarModelNo
                                  select a;

                if (varCarMolde.Count() != 1)
                {
                    return;
                }

                var varDID = from a in ctx.Business_Project_TCU_SoftwareUpdate_DID
                             where a.BillNo == billNo
                             select a;

                if (varDID.Count() == 0)
                {
                    return;
                }

                string QRCode = "";

                QRCode += "供应商代码," + varCarMolde.Single().QRCode_Provider.ToString().Trim() + "\r\n";
                QRCode += "零部件种类状态代码," + varCarMolde.Single().QRCode_PartsType.ToString().Trim() + "\r\n";
                QRCode += "容大零件图号," + varCarMolde.Single().QRCode_PartsCode.ToString().Trim() + "\r\n";
                QRCode += "软件版本," + varData.Single().Version.ToString().Trim() + "\r\n";
                QRCode += "工厂代号匹配电阻," + varCarMolde.Single().QRCode_FactoryCode.ToString().Trim() + "\r\n";

                UniversalControlLibrary.FileOperationService.Write_TxtFile(QRCode, filePath + "\\TCU二维码信息.txt");

                string markingCode = "";

                foreach (var item in varDID)
                {
                    markingCode += item.DID + "," + item.DataContent + "\r\n";
                }

                UniversalControlLibrary.FileOperationService.Write_TxtFile(markingCode, filePath + "\\TCU数据标识符.txt");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
