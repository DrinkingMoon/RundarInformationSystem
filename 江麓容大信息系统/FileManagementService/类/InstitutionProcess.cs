/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  FTPService.cs
 * 作者    :  曹津彬    版本: v1.00    日期: 2014/05/23
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
using ServerModule;
using System.Reflection;
using System.ComponentModel;

namespace Service_Quality_File
{
    public enum InstitutionBillStatus
    {
         新建流程,
         等待科长审查,
         等待负责人审查,
         等待相关负责人审查,
         等待相关分管领导审查,
         等待分管领导审查,
         等待总经理审查,
         流程已结束
    }

    /// <summary>
    /// 制度流程服务类
    /// </summary>
    class InstitutionProcess: BasicServer, IInstitutionProcess
    {
        /// <summary>
        /// 检查某业务是否存在
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">业务号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.FM_InstitutionProcess
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
        /// 检查某单据是否存在
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(string billNo)
        {
            string sql = "SELECT * FROM [DepotManagement].[dbo].[FM_InstitutionProcess] where BillNo = '" + billNo + "'";

            System.Data.DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 获得所有单据信息
        /// </summary>
        /// <param name="billTypeEnum">单据类型</param>
        /// <returns>返回Table</returns>
        public DataTable GetAllBill(CE_BillTypeEnum billTypeEnum)
        {
            string strSql = "select * from View_FM_InstitutionProcess where 流程类型名称 = '" + billTypeEnum.ToString() 
                + "' and " + QueryResultFilter;

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得单条单据信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回LINQ</returns>
        public FM_InstitutionProcess GetSingleBill(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.FM_InstitutionProcess
                          where a.BillNo == billNo
                          select a;

            if (varData.Count() != 1)
            {
                return null;
            }
            else
            {
                return varData.Single();
            }
        }

        /// <summary>
        /// 获得流程执行图形信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回字典</returns>
        public Dictionary<int, Dictionary<string, bool>> GetExcuteInfo(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            Dictionary<int, Dictionary<string, bool>> listResult = new Dictionary<int, Dictionary<string, bool>>();

            var varData = from a in ctx.FM_InstitutionProcess
                          where a.BillNo == billNo
                          select a;


            if (varData.Count() == 1)
            {
                FM_InstitutionProcess tempInstitution = varData.Single();
                Dictionary<string, bool> dic = new Dictionary<string, bool>();

                dic = new Dictionary<string, bool>();
                if (tempInstitution.ChiefTime != null)
                {
                    dic.Add(tempInstitution.Chief, true);
                }
                else
                {
                    dic.Add("科长", false);
                }

                listResult.Add(1, dic);

                dic = new Dictionary<string, bool>();
                if (tempInstitution.DepartmentHeadTime != null)
                {
                    dic.Add(tempInstitution.DepartmentHead, true);
                }
                else
                {
                    dic.Add("部门负责人", false);
                }

                listResult.Add(2, dic);

                dic = new Dictionary<string, bool>();
                var varPoint = from a in ctx.FM_InstitutionProcessPointDept
                               where a.BillNo == billNo && a.PersonnelType == RoleStyle.负责人.ToString()
                               select a;

                foreach (FM_InstitutionProcessPointDept item in varPoint)
                {
                    dic.Add(UniversalFunction.GetPersonnelInfo(item.Personnel).姓名, item.PersonnelTime == null ? false : true);
                }

                listResult.Add(3, dic);

                if (IsThreeTripFile(tempInstitution.SortID))
                {
                    dic = new Dictionary<string, bool>();
                    if (tempInstitution.LeadershipTime != null)
                    {
                        dic.Add(tempInstitution.Leadership, true);
                    }
                    else
                    {
                        dic.Add("分管领导", false);
                    }

                    listResult.Add(4, dic);
                }
                else
                {
                    dic = new Dictionary<string, bool>();
                    varPoint = from a in ctx.FM_InstitutionProcessPointDept
                               where a.BillNo == billNo && a.PersonnelType == RoleStyle.分管领导.ToString()
                               select a;

                    foreach (FM_InstitutionProcessPointDept item in varPoint)
                    {
                        dic.Add(UniversalFunction.GetPersonnelInfo(item.Personnel).姓名, item.PersonnelTime == null ? false : true);
                    }

                    listResult.Add(4, dic);

                    dic = new Dictionary<string, bool>();
                    if (tempInstitution.GeneralManagerTime != null)
                    {
                        dic.Add(tempInstitution.GeneralManager, true);
                    }
                    else
                    {
                        dic.Add("总经理", false);
                    }

                    listResult.Add(5, dic);
                }
            }

            return listResult;
        }

        /// <summary>
        /// 获得流程意见
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回LIST</returns>
        public List<CommonProcessInfo> GetProcessInfo(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            List<CommonProcessInfo> listResult = new List<CommonProcessInfo>();
            CommonProcessInfo tempInfo = new CommonProcessInfo();

            var varData = from a in ctx.FM_InstitutionProcess
                          where a.BillNo == billNo
                          select a;

            if (varData.Count() == 1)
            {
                FM_InstitutionProcess tempInstitution = varData.Single();

                if (tempInstitution.GeneralManagerTime != null)
                {
                    tempInfo = new CommonProcessInfo();

                    tempInfo.操作节点 = "总经理";
                    tempInfo.人员 = tempInstitution.GeneralManager;
                    tempInfo.时间 = tempInstitution.GeneralManagerTime.ToString();
                    tempInfo.意见 = tempInstitution.GeneralManagerAdvise;
                    listResult.Add(tempInfo);
                }

                if (tempInstitution.LeadershipTime != null)
                {
                    tempInfo = new CommonProcessInfo();

                    tempInfo.操作节点 = "分管领导";
                    tempInfo.人员 = tempInstitution.Leadership;
                    tempInfo.时间 = tempInstitution.LeadershipTime.ToString();
                    tempInfo.意见 = tempInstitution.LeadershipAdvise;
                    listResult.Add(tempInfo);
                }

                var varPoint = (from a in ctx.FM_InstitutionProcessPointDept
                                where a.BillNo == billNo && a.PersonnelTime != null
                                select a).OrderBy(r => r.PersonnelType).ThenByDescending(r => r.PersonnelTime);

                foreach (FM_InstitutionProcessPointDept item in varPoint)
                {
                    tempInfo = new CommonProcessInfo();

                    tempInfo.操作节点 = "相关部门" + item.PersonnelType;
                    tempInfo.人员 = UniversalFunction.GetPersonnelInfo(item.Personnel).姓名;
                    tempInfo.时间 = item.PersonnelTime.ToString();
                    tempInfo.意见 = item.Advise;
                    listResult.Add(tempInfo);
                }

                if (tempInstitution.DepartmentHeadTime != null)
                {
                    tempInfo = new CommonProcessInfo();

                    tempInfo.操作节点 = "部门负责人";
                    tempInfo.人员 = tempInstitution.DepartmentHead;
                    tempInfo.时间 = tempInstitution.DepartmentHeadTime.ToString();
                    tempInfo.意见 = tempInstitution.DepartmentHeadAdvise;
                    listResult.Add(tempInfo);
                }

                if (tempInstitution.ChiefTime != null)
                {
                    tempInfo = new CommonProcessInfo();

                    tempInfo.操作节点 = "科长";
                    tempInfo.人员 = tempInstitution.Chief;
                    tempInfo.时间 = tempInstitution.ChiefTime.ToString();
                    tempInfo.意见 = tempInstitution.ChiefAdvise;
                    listResult.Add(tempInfo);
                }
            }

            return listResult;
        }

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="institution">LINQ信息</param>
        /// <param name="deptList">部门信息</param>
        /// <param name="billEnum">单据类型</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True</returns>
        public bool AddInfo(FM_InstitutionProcess institution, List<string> deptList, out string error)
        {
            error = null;

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            IBillMessagePromulgatorServer serverBill = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();
            serverBill.BillType = serverBill.GetBillTypeEnum(institution.TypeCode).ToString();

            try
            {
                institution.Propoer = BasicInfo.LoginName;
                institution.PropoerTime = ServerTime.Time;
                institution.BillStatus = InstitutionBillStatus.等待科长审查.ToString();

                ctx.FM_InstitutionProcess.InsertOnSubmit(institution);

                List<FM_InstitutionProcessPointDept> listPoint = new List<FM_InstitutionProcessPointDept>();
                FM_InstitutionProcessPointDept tempLnq = new FM_InstitutionProcessPointDept();

                if (deptList != null)
                {
                    foreach (string item in deptList)
                    {
                        List<string> list = serverBill.GetUserCodes(serverBill.GetDeptPrincipalRoleName(item).ToList(), null);
                        foreach (string workID in list)
                        {
                            tempLnq = new FM_InstitutionProcessPointDept();

                            tempLnq.BillNo = institution.BillNo;
                            tempLnq.PersonnelType = RoleStyle.负责人.ToString();
                            tempLnq.Personnel = workID;

                            listPoint.Add(tempLnq);
                        }

                        if (!IsThreeTripFile(institution.SortID))
                        {
                            list = serverBill.GetUserCodes(serverBill.GetDeptLeaderRoleName(item).ToList(), null);
                            foreach (string workID in list)
                            {
                                tempLnq = new FM_InstitutionProcessPointDept();

                                tempLnq.BillNo = institution.BillNo;
                                tempLnq.PersonnelType = RoleStyle.分管领导.ToString();
                                tempLnq.Personnel = workID;

                                listPoint.Add(tempLnq);
                            }
                        }
                    }

                    var varData = (from a in listPoint
                                   select new { a.BillNo, a.Personnel, a.PersonnelType }).Distinct();

                    listPoint = new List<FM_InstitutionProcessPointDept>();

                    foreach (var item in varData)
                    {
                        tempLnq = new FM_InstitutionProcessPointDept();

                        tempLnq.PersonnelType = item.PersonnelType;
                        tempLnq.Personnel = item.Personnel;
                        tempLnq.BillNo = item.BillNo;

                        listPoint.Add(tempLnq);
                    }

                    ctx.FM_InstitutionProcessPointDept.InsertAllOnSubmit(listPoint);
                }

                ctx.SubmitChanges();

                serverBill.DestroyMessage(institution.BillNo);
                serverBill.SendNewFlowMessage(institution.BillNo,
                    string.Format("{0}号文件审查流程已提交，请等待上级审核", institution.BillNo),
                    BillFlowMessage_ReceivedUserType.角色, serverBill.GetSuperior(CE_RoleStyleType.上级领导, BasicInfo.LoginID));

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 是否为第三层次文件
        /// </summary>
        /// <param name="sortID">文件类别ID</param>
        /// <returns>是 返回True,否 返回False</returns>
        bool IsThreeTripFile(int sortID)
        {
            ISystemFileBasicInfo serverBasic = ServerModuleFactory.GetServerModule<ISystemFileBasicInfo>();
            FM_FileSort sort = serverBasic.SortInfo(sortID);

            if (sort.FileType == (int)CE_FileType.制度文件)
            {
                if (sort.SortName.Contains("_规定、细则、规程、标准"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 更新流程
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="advise">意见</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool UpdateInfo(string billNo, string advise, out string error)
        {
            error = null;

            IBillMessagePromulgatorServer serverBill = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();
            ISystemFileBasicInfo m_serverFileBasicInfo = Service_Quality_File.ServerModuleFactory.GetServerModule<ISystemFileBasicInfo>();
            FileServiceSocket m_serverFTP = new FileServiceSocket(GlobalObject.GlobalParameter.FTPServerIP,
                GlobalObject.GlobalParameter.FTPServerAdvancedUser,
                GlobalObject.GlobalParameter.FTPServerAdvancedPassword);

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            try
            {
                var varData = from a in ctx.FM_InstitutionProcess
                              where a.BillNo == billNo
                              select a;

                FM_InstitutionProcess lnqProcess = varData.Single();

                serverBill.BillType = serverBill.GetBillTypeEnum(lnqProcess.TypeCode).ToString();
                CE_BillTypeEnum billType = GlobalObject.GeneralFunction.StringConvertToEnum<CE_BillTypeEnum>(serverBill.BillType);
                InstitutionBillStatus billStatus = GlobalObject.GeneralFunction.StringConvertToEnum<InstitutionBillStatus>(lnqProcess.BillStatus);

                var varList = from a in ctx.FM_InstitutionProcessPointDept
                              where a.BillNo == billNo
                              select a;

                FM_InstitutionProcessPointDept lnqPoint = new FM_InstitutionProcessPointDept();
                List<FM_InstitutionProcessPointDept> list = varList.ToList();
                List<string> listTemp = new List<string>();

                switch (billStatus)
                {
                    case InstitutionBillStatus.新建流程:
                        break;
                    case InstitutionBillStatus.等待科长审查:

                        lnqProcess.BillStatus = InstitutionBillStatus.等待负责人审查.ToString();
                        lnqProcess.Chief = BasicInfo.LoginName;
                        lnqProcess.ChiefAdvise = advise;
                        lnqProcess.ChiefTime = ServerTime.Time;

                        serverBill.PassFlowMessage(lnqProcess.BillNo, string.Format("【文件编号】：{0} 【文件名】：{1} ※※※ 等待处理", lnqProcess.FileNo, lnqProcess.FileName),
                            BillFlowMessage_ReceivedUserType.角色, serverBill.GetDeptPrincipalRoleName(UniversalFunction.GetPersonnelInfo(lnqProcess.Propoer).部门编码).ToList());
                        break;
                    case InstitutionBillStatus.等待负责人审查:

                        lnqProcess.BillStatus = InstitutionBillStatus.等待相关负责人审查.ToString();
                        lnqProcess.DepartmentHead = BasicInfo.LoginName;
                        lnqProcess.DepartmentHeadAdvise = advise;
                        lnqProcess.DepartmentHeadTime = ServerTime.Time;

                        if (billType == CE_BillTypeEnum.制度销毁申请流程)
                        {
                            lnqProcess.BillStatus = InstitutionBillStatus.等待分管领导审查.ToString();

                            serverBill.PassFlowMessage(lnqProcess.BillNo, string.Format("【文件编号】：{0} 【文件名】：{1} ※※※ 等待处理", lnqProcess.FileNo, lnqProcess.FileName),
                                BillFlowMessage_ReceivedUserType.角色, serverBill.GetDeptLeaderRoleName(UniversalFunction.GetPersonnelInfo(lnqProcess.Propoer).部门编码).ToList());
                        }
                        else
                        {
                            listTemp = (from a in list
                                        where a.PersonnelType == RoleStyle.负责人.ToString()
                                        select a.Personnel).ToList();

                            serverBill.PassFlowMessage(lnqProcess.BillNo, string.Format("【文件编号】：{0} 【文件名】：{1} ※※※ 等待处理", lnqProcess.FileNo, lnqProcess.FileName),
                                BillFlowMessage_ReceivedUserType.用户, listTemp);
                        }

                        break;
                    case InstitutionBillStatus.等待相关负责人审查:

                        lnqPoint = (from a in list
                                    where a.Personnel == BasicInfo.LoginID
                                    && a.PersonnelType == RoleStyle.负责人.ToString()
                                    select a).Single();

                        lnqPoint.PersonnelTime = ServerTime.Time;
                        lnqPoint.Advise = advise;

                        var varHead = from a in list
                                      where a.PersonnelType == RoleStyle.负责人.ToString()
                                      && a.PersonnelTime == null
                                      select a;

                        if (varHead.Count() == 0)
                        {
                            if (IsThreeTripFile(lnqProcess.SortID))
                            {
                                lnqProcess.BillStatus = InstitutionBillStatus.等待分管领导审查.ToString();

                                serverBill.PassFlowMessage(lnqProcess.BillNo, string.Format("【文件编号】：{0} 【文件名】：{1} ※※※ 等待处理", lnqProcess.FileNo, lnqProcess.FileName),
                                    BillFlowMessage_ReceivedUserType.角色, serverBill.GetDeptLeaderRoleName(UniversalFunction.GetPersonnelInfo(lnqProcess.Propoer).部门编码).ToList());
                            }
                            else
                            {
                                lnqProcess.BillStatus = InstitutionBillStatus.等待相关分管领导审查.ToString();

                                listTemp = (from a in list
                                            where a.PersonnelType == RoleStyle.分管领导.ToString()
                                            select a.Personnel).ToList();

                                serverBill.PassFlowMessage(lnqProcess.BillNo, string.Format("【文件编号】：{0} 【文件名】：{1} ※※※ 等待处理", lnqProcess.FileNo, lnqProcess.FileName),
                                    BillFlowMessage_ReceivedUserType.用户, listTemp);
                            }
                        }
                        else
                        {
                            listTemp = (from a in varHead
                                        select a.Personnel).ToList();

                            serverBill.PassFlowMessage(lnqProcess.BillNo, string.Format("【文件编号】：{0} 【文件名】：{1} ※※※ 等待处理", lnqProcess.FileNo, lnqProcess.FileName),
                                BillFlowMessage_ReceivedUserType.用户, listTemp);
                        }

                        break;
                    case InstitutionBillStatus.等待相关分管领导审查:
                        lnqPoint = (from a in list
                                    where a.Personnel == BasicInfo.LoginID
                                    && a.PersonnelType == RoleStyle.分管领导.ToString()
                                    select a).Single();

                        lnqPoint.PersonnelTime = ServerTime.Time;
                        lnqPoint.Advise = advise;

                        var varLead = from a in list
                                      where a.PersonnelType == RoleStyle.分管领导.ToString()
                                      && a.PersonnelTime == null
                                      select a;

                        if (varLead.Count() == 0)
                        {
                            lnqProcess.BillStatus = InstitutionBillStatus.等待总经理审查.ToString();

                            serverBill.PassFlowMessage(lnqProcess.BillNo, string.Format("【文件编号】：{0} 【文件名】：{1} ※※※ 等待处理", lnqProcess.FileNo, lnqProcess.FileName),
                                BillFlowMessage_ReceivedUserType.角色, CE_RoleEnum.总经理.ToString());
                        }
                        else
                        {
                            listTemp = (from a in varLead
                                        select a.Personnel).ToList();

                            serverBill.PassFlowMessage(lnqProcess.BillNo, string.Format("【文件编号】：{0} 【文件名】：{1} ※※※ 等待处理", lnqProcess.FileNo, lnqProcess.FileName),
                                BillFlowMessage_ReceivedUserType.用户, listTemp);
                        }

                        break;
                    case InstitutionBillStatus.等待分管领导审查:
                    case InstitutionBillStatus.等待总经理审查:

                        lnqProcess.BillStatus = InstitutionBillStatus.流程已结束.ToString();
                        lnqProcess.GeneralManager = BasicInfo.LoginName;
                        lnqProcess.GeneralManagerAdvise = advise;
                        lnqProcess.GeneralManagerTime = ServerTime.Time;

                        FM_FileList fileInfo = new FM_FileList();

                        if (lnqProcess.FileID != null)
                        {
                            var varFileInfo = from a in ctx.FM_FileList
                                              where a.FileID == lnqProcess.FileID
                                              select a;

                            if (varFileInfo.Count() == 1)
                            {
                                fileInfo = varFileInfo.Single();
                            }
                        }

                        if (billType == CE_BillTypeEnum.制度发布流程)
                        {
                            string strVersion = "1.0";

                            if (lnqProcess.ReplaceFileID == null)
                            {
                                DataTable dtTemp = m_serverFileBasicInfo.GetFilesInfo(lnqProcess.FileNo, null);
                            }
                            else
                            {
                                m_serverFileBasicInfo.OperatorFTPSystemFile(ctx, Convert.ToInt32(lnqProcess.ReplaceFileID), 29);
                                strVersion =
                                    (Convert.ToDouble(m_serverFileBasicInfo.GetFile(Convert.ToInt32(lnqProcess.ReplaceFileID)).Version) + 0.1).ToString();
                            }

                            FM_FileList lnqFile = new FM_FileList();

                            lnqFile.Department = lnqProcess.Department;
                            lnqFile.FileName = lnqProcess.FileName;
                            lnqFile.FileNo = lnqProcess.FileNo;
                            lnqFile.FileUnique = lnqProcess.FileUnique;
                            lnqFile.SortID = lnqProcess.SortID;
                            lnqFile.Version = strVersion;

                            ctx.FM_FileList.InsertOnSubmit(lnqFile);

                            if (m_serverFTP.Errormessage.Length != 0)
                            {
                                throw new Exception(m_serverFTP.Errormessage);
                            }
                        }
                        else if (billType == CE_BillTypeEnum.制度修订废弃申请流程 && lnqProcess.OperationMode == "废弃")
                        {
                            if (fileInfo != null)
                            {
                                fileInfo.SortID = 29;
                            }
                        }
                        else if (billType == CE_BillTypeEnum.制度销毁申请流程)
                        {
                            ctx.FM_FileList.DeleteOnSubmit(fileInfo);
                        }

                        serverBill.EndFlowMessage(lnqProcess.BillNo,
                            string.Format("{0}号文件审查流程已结束", lnqProcess.BillNo), null,
                            (from a in varList select a.Personnel).ToList());

                        break;
                    case InstitutionBillStatus.流程已结束:
                        break;
                    default:
                        break;
                }

                ctx.SubmitChanges();

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
        /// <param name="billNo">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        public bool DeleteInfo(string billNo, out string error)
        {
            error = null;
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            try
            {
                var varData = from a in ctx.FM_InstitutionProcess
                              where a.BillNo == billNo
                              select a;

                ctx.FM_InstitutionProcess.DeleteAllOnSubmit(varData);
                ctx.SubmitChanges();

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
