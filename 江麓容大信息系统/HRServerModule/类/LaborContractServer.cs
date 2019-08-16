using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ServerModule;
using PlatformManagement;
using GlobalObject;

namespace Service_Peripheral_HR
{
    /// <summary>
    /// 合同管理服务类
    /// </summary>
    class LaborContractServer : Service_Peripheral_HR.ILaborContractServer
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
        /// 获得所有合同类别
        /// </summary>
        /// <returns>返回数据集</returns>
        public DataTable GetLaborContracType()
        {
            string sql = "select TypeCode 类别编号, TypeName 类别名称, Category 类别,Remark 备注, Recorder 记录员编号, "+
                         " RecordTime 记录时间 from HR_LaborContractType";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 通过合同类别的编号获得合同类别的名称
        /// </summary>
        /// <param name="typeCode">类别编号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public string GetLaborTypeByTypeCode(string typeCode, out string error)
        {
            error = "";
            string sql = "select TypeName from HR_LaborContractType where TypeCode='" + typeCode + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["TypeName"].ToString();
            }
            else
            {
                error = "信息有误，请检查信息！";
                return "";
            }
        }

        /// <summary>
        /// 通过合同类别的名称获得合同类别的编号
        /// </summary>
        /// <param name="typeName">合同类别名称</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回编号，失败返回空</returns>
        public string GetLaborTypeByTypeName(string typeName, out string error)
        {
            error = "";
            string sql = "select TypeCode from HR_LaborContractType where TypeName='" + typeName + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["TypeCode"].ToString();
            }
            else
            {
                error = "信息有误，请检查信息！";
                return "";
            }
        }

        /// <summary>
        /// 通过合同类别的名称获得合同类别
        /// </summary>
        /// <param name="typeName">合同类别名称</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public string GetCategory(string typeName, out string error)
        {
            error = "";
            string sql = "select Category from HR_LaborContractType where TypeName='" + typeName + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["Category"].ToString();
            }
            else
            {
                error = "信息有误，请检查信息！";
                return "";
            }
        }

        /// <summary>
        /// 新增合同类别
        /// </summary>
        /// <param name="laborType">合同类别数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool AddLaborType(HR_LaborContractType laborType,out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_LaborContractType
                             where a.TypeCode == laborType.TypeCode
                             select a;

                if (result.Count() > 0)
                {
                    error = "合同类别已经存在！";
                    return false;
                }

                dataContxt.HR_LaborContractType.InsertOnSubmit(laborType);
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
        /// 通过合同类别的编号删除此合同类别
        /// </summary>
        /// <param name="typeCode">合同类别编号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool DeleteLaborTypeByTypeCode(string typeCode, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;
                var resultUse = from a in dataContxt.HR_LaborContractTemplet
                                where a.LaborContractTypeCode == typeCode
                                select a;

                if (resultUse.Count() > 0)
                {
                    error = "已有合同使用了此合同类别，先删除合同模版后，再删除类别！";
                    return false;
                }

                var result = from c in dataContxt.HR_LaborContractType
                             where c.TypeCode == typeCode
                             select c;

                if (result.Count() == 0)
                {
                    error = "信息有误，请检查！";
                    return false;
                }

                dataContxt.HR_LaborContractType.DeleteAllOnSubmit(result);
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
        /// 获得所有合同模版
        /// </summary>
        /// <returns>返回数据集</returns>
        public DataTable GetLaborContractTemplet()
        {
            string sql = "select * from View_HR_LaborContractTemplet";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 通过版本和类别获得合同模版编号
        /// </summary>
        /// <param name="type">类别</param>
        /// <param name="version">版本</param>
        /// <returns>合同模版编号</returns>
        public int GetLaborContractTempletByTypeAndVersion(string type,string version)
        {
            string sql = "select 编号 from dbo.View_HR_LaborContractTemplet where 合同类别='" + type + "' and 合同版本='" + version + "' ";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return Convert.ToInt32(dt.Rows[0]["编号"].ToString());
        }

        /// <summary>
        /// 新增合同模板
        /// </summary>
        /// <param name="templet">合同模板数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool AddContractTemplet(HR_LaborContractTemplet templet,out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;
                var result = from a in dataContxt.HR_LaborContractTemplet
                             where a.LaborContractTypeCode == templet.LaborContractTypeCode && a.Version == templet.Version
                             select a;

                if (result.Count() > 0)
                {
                    error = "此合同版本已经存在，请检查信息";
                    return false;
                }

                dataContxt.HR_LaborContractTemplet.InsertOnSubmit(templet);
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
        /// 修改合同模板
        /// </summary>
        /// <param name="templet">合同模板数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool UpdateContractTemplet(HR_LaborContractTemplet templet, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;
                var result = from a in dataContxt.HR_LaborContractTemplet
                             where a.ID == templet.ID
                             select a;

                if (result.Count() == 1)
                {
                    HR_LaborContractTemplet ContractTemplet = result.Single();

                    ContractTemplet.LaborContractFile = templet.LaborContractFile;
                    ContractTemplet.LaborContractFileName = templet.LaborContractFileName;
                    ContractTemplet.Remark = templet.Remark;
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
        /// 删除合同模板信息
        /// </summary>
        /// <param name="templetID">合同模板数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool DeleteContractTemplet(int templetID, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var resultUse = from c in dataContxt.HR_PersonnelLaborContract
                                where c.LaborContractTempletID == templetID
                                select c;

                if (resultUse.Count() > 0)
                {
                    error = "合同中使用到了此模板，需要先删除关联的合同再删除模板";
                    return false;
                }

                var result = from a in dataContxt.HR_LaborContractTemplet
                             where a.ID == templetID
                             select a;

                if (result.Count() != 1)
                {
                    error = "信息不唯一，请检查信息";
                    return false;
                }

                dataContxt.HR_LaborContractTemplet.DeleteAllOnSubmit(result);
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
        /// 获得合同状态
        /// </summary>
        /// <returns>返回数据集</returns>
        public DataTable GetContractStatus()
        {
            string sql = "select * from View_HR_LaborContractStatus order by 类别";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);
            return dt;
        }

        /// <summary>
        /// 通过合同类别获得合同状态
        /// </summary>
        /// <param name="type">合同类型</param>
        /// <returns>返回数据集</returns>
        public DataTable GetContractStatusByType(string type)
        {
            string sql = "select StatusName 状态 from HR_LaborContractStatus" +
                         " where LaborContractType='" + type + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);
            return dt;
        }

        /// <summary>
        /// 通过状态名获得合同状态编号
        /// </summary>
        /// <param name="statusName">状态名</param>
        /// <returns>返回合同状态编号</returns>
        public int GetContractStatusByName(string statusName)
        {
            string sql = "select ID from HR_LaborContractStatus where StatusName='" + statusName + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);
            return Convert.ToInt32(dt.Rows[0]["ID"].ToString()); ;
        }

        /// <summary>
        /// 通过状态编号获得合同状态标志
        /// </summary>
        /// <param name="statusID">状态编号</param>
        /// <returns>返回合同状态标志</returns>
        public bool GetContractStatusFlagByID(string statusID)
        {
            string sql = "select deleteFlag from HR_LaborContractStatus where ID='" + statusID + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);
            return Convert.ToBoolean(dt.Rows[0]["deleteFlag"].ToString()); ;
        }

        /// <summary>
        /// 通过状态编号获得合同状态名
        /// </summary>
        /// <param name="statusID">状态编号</param>
        /// <returns>返回合同状态名</returns>
        public string GetContractStatusByID(int statusID)
        {
            string sql = "select StatusName from HR_LaborContractStatus where ID='" + statusID + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);
            return dt.Rows[0]["StatusName"].ToString(); ;
        }

        /// <summary>
        /// 新增合同状态
        /// </summary>
        /// <param name="conStatus">合同状态数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True失败但会False</returns>
        public bool AddContractStatus(HR_LaborContractStatus conStatus,out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext datatContxt = CommentParameter.DepotDataContext;

                var result = from a in datatContxt.HR_LaborContractStatus
                             where a.StatusName == conStatus.StatusName && a.LaborContractType == conStatus.LaborContractType
                             select a;

                if (result.Count() > 0)
                {
                    error = "状态已经存在！";
                    return false;
                }

                datatContxt.HR_LaborContractStatus.InsertOnSubmit(conStatus);
                datatContxt.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 通过状态编号删除合同状态
        /// </summary>
        /// <param name="statusID">状态编号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True失败返回False</returns>
        public bool DeleteContractStatus(int statusID, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext datatContxt = CommentParameter.DepotDataContext;

                var resultUse = from c in datatContxt.HR_PersonnelLaborContract
                                where c.LaborContractStatusID == statusID
                                select c;

                if (resultUse.Count() > 0)
                {
                    error = "已有合同使用了此状态，需要先删除合同再删除状态！";
                    return false;
                }

                var result = from a in datatContxt.HR_LaborContractStatus
                             where a.ID == statusID
                             select a;

                if (result.Count() != 1)
                {
                    error = "信息有误，请检查！";
                    return false;
                }

                datatContxt.HR_LaborContractStatus.DeleteAllOnSubmit(result);
                datatContxt.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 获取员工合同信息
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True失败返回False</returns>
        public View_HR_PersonnelLaborContract GetPersonnelContarctByWorkID(string workID, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext datatContxt = CommentParameter.DepotDataContext;

                var resultUse = (from c in datatContxt.View_HR_PersonnelLaborContract
                                where c.员工编号 == workID
                                select c).Take(1);

                if (resultUse.Count() > 0)
                {
                    return resultUse.Single();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// 获取员工合同信息
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <returns>返回数据集</returns>
        public DataTable GetPersonnelContarctByWorkID(string workID)
        {
            string sql = "select 员工编号, 员工姓名, 合同模板, 合同状态, CONVERT(varchar(10) , 合同起始时间, 20 ) as 合同起始时间,"+
                         "CONVERT(varchar(10) , 合同终止时间, 20 ) as 合同终止时间 from "+
                         "(select  员工编号, 员工姓名, 合同模板, 合同状态,  合同起始时间, 合同终止时间 " +
                         " from View_HR_PersonnelLaborContract union all" +
                         " select  员工编号, 员工姓名, 合同类别 as 合同模板, 合同状态," +
                         " 合同起始时间, 合同终止时间 from View_HR_PersonnelLaborContractHistory ) " +
                         " as temp where 员工编号='" + workID + "' order by 合同起始时间";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 获取员工合同信息
        /// </summary>
        /// <param name="returnInfo">员工合同信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool GetAllPersonnelContarct(out IQueryResult returnInfo, out string error)
        {
            returnInfo = null;
            error = null;

            IAuthorization serverAuthorization = PlatformFactory.GetObject<IAuthorization>();
            IQueryResult qr = null;

            if (QueryResultFilter == null)
            {
                qr = serverAuthorization.Query("员工合同管理", null);
            }
            else
            {
                qr = serverAuthorization.Query("员工合同管理", null, QueryResultFilter);
            }

            if (!qr.Succeeded)
            {
                error = qr.Error;
                return false;
            }

            IBillTypeServer server = BasicServerFactory.GetServerModule<IBillTypeServer>();
            BASE_BillType lnqBillType = server.GetBillTypeFromName("员工合同管理");

            if (lnqBillType == null)
            {
                error = "获取不到单据类型信息";
                return false;
            }

            int m_billUniqueID = lnqBillType.UniqueID;

            returnInfo = qr;
            return true;
        }

        /// <summary>
        /// 获取员工合同历史信息
        /// </summary>
        /// <param name="returnInfo">员工合同历史信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool GetAllContarctHistory(out IQueryResult returnInfo, out string error)
        {
            returnInfo = null;
            error = null;

            IAuthorization serverAuthorization = PlatformFactory.GetObject<IAuthorization>();
            IQueryResult qr = null;

            if (QueryResultFilter == null)
            {
                qr = serverAuthorization.Query("员工合同历史查询", null);
            }
            else
            {
                qr = serverAuthorization.Query("员工合同历史查询", null, QueryResultFilter);
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
        /// 新增员工合同信息
        /// </summary>
        /// <param name="personnelContract">员工合同数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True失败返回false</returns>
        public bool AddPersonnelContract(HR_PersonnelLaborContract personnelContract,out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext datatContxt = CommentParameter.DepotDataContext;

                var result = from a in datatContxt.HR_PersonnelLaborContract
                             where a.WorkID == personnelContract.WorkID 
                             && a.LaborContractTempletID == personnelContract.LaborContractTempletID
                             select a;

                if (result.Count() > 0)
                {
                    error = "【"+personnelContract.WorkID+"】员工，已签订同种版本的合同！";
                    return false;
                }

                datatContxt.HR_PersonnelLaborContract.InsertOnSubmit(personnelContract);
                datatContxt.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        public bool UpdatePersonnelContract(string workID,DateTime date)
        {
            try
            {
                DepotManagementDataContext datatContxt = CommentParameter.DepotDataContext;

                var result = from a in datatContxt.HR_PersonnelLaborContract
                             where a.WorkID == workID && (a.LaborContractTempletID == 1 || a.LaborContractTempletID == 3)
                             select a;

                if (result.Count() == 1)
                {
                    HR_PersonnelLaborContract laborContract = result.Single();

                    if (date < laborContract.EndTime)
                    {
                        laborContract.LaborContractStatusID = 8;
                    }
                    else
                    {
                        laborContract.LaborContractStatusID = 7;
                    }
                }

                var resultList = from a in datatContxt.HR_PersonnelLaborContract
                             where a.WorkID == workID && (a.LaborContractTempletID == 2)
                             select a;

                if (resultList.Count() == 1)
                {
                    HR_PersonnelLaborContract laborContract = resultList.Single();

                    laborContract.LaborContractStatusID = 6;
                }

                datatContxt.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 修改员工合同信息
        /// </summary>
        /// <param name="personnelContractOld">员工原始合同数据集</param>
        /// <param name="personnelContractNew">员工新合同数据集</param>
        /// <param name="flag">状态标志</param>
        /// <param name="billNo">合同编号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True失败返回false</returns>
        public bool UpdatePersonnelContract(HR_PersonnelLaborContractHistory personnelContractOld, 
                                            HR_PersonnelLaborContract personnelContractNew,bool flag,int billNo, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext datatContxt = CommentParameter.DepotDataContext;

                var result = from a in datatContxt.HR_PersonnelLaborContract
                             where a.WorkID == personnelContractNew.WorkID
                             && a.ID == billNo
                             select a;

                if (result.Count() == 0)
                {
                    error = "【" + personnelContractNew.WorkID + "】员工，还没有此合同，请点击【添加】！";
                    return false;
                }

                HR_PersonnelLaborContract personnelLabor = result.Single();
                personnelLabor.BeginTime = personnelContractNew.BeginTime;
                personnelLabor.EndTime = personnelContractNew.EndTime;
                personnelLabor.LaborContractStatusID = personnelContractNew.LaborContractStatusID;
                personnelLabor.LaborContractTempletID = personnelContractNew.LaborContractTempletID;
                personnelLabor.Recorder = BasicInfo.LoginID;
                personnelLabor.RecordTime = ServerTime.Time;
                personnelLabor.Remark = personnelContractNew.Remark;

                datatContxt.HR_PersonnelLaborContractHistory.InsertOnSubmit(personnelContractOld);

                //if (flag)
                //{
                //    var resultList = from c in datatContxt.HR_PersonnelArchive
                //                     where c.WorkID == personnelLabor.WorkID
                //                     select c;

                //    if (resultList.Count() > 0)
                //    {
                //        HR_PersonnelArchiveChange personnelChange = new HR_PersonnelArchiveChange();
                //        HR_PersonnelArchive personnel = resultList.Single();

                //        personnelChange.WorkID = personnel.WorkID;
                //        personnelChange.Name = personnel.Name;
                //        personnelChange.WorkPost = new OperatingPostServer().GetOperatingPostByPostCode(personnel.WorkPost);
                //        personnelChange.JobTitle = new JobTitleServer().GetJobTitleByJobID(personnel.JobTitleID);
                //        personnelChange.JoinDate = Convert.ToDateTime(personnel.JoinDate);
                //        personnelChange.GraduationYear = personnel.GraduationYear;
                //        personnelChange.BecomeRegularEmployeeDate = personnel.BecomeRegularEmployeeDate;
                //        personnelChange.Sex = personnel.Sex;
                //        personnelChange.DeptName = new OrganizationServer().GetDeptByDeptCode(personnel.Dept).部门名称;
                //        personnelChange.Dept = personnel.Dept;
                //        personnelChange.Birthday = personnel.Birthday;
                //        personnelChange.Nationality = personnel.Nationality;
                //        personnelChange.Race = personnel.Race;
                //        personnelChange.Birthplace = personnel.Birthplace;
                //        personnelChange.Party = personnel.Party;
                //        personnelChange.ID_Card = personnel.ID_Card;
                //        personnelChange.College = personnel.College;
                //        personnelChange.EducatedDegree = personnel.EducatedDegree;
                //        personnelChange.EducatedMajor = personnel.EducatedMajor;
                //        personnelChange.FamilyAddress = personnel.FamilyAddress;
                //        personnelChange.PostCode = personnel.PostCode;
                //        personnelChange.Phone = personnel.Phone;
                //        personnelChange.Speciality = personnel.Speciality;
                //        personnelChange.MobilePhone = personnel.MobilePhone;
                //        personnelChange.TrainingAmount = personnel.TrainingAmount;
                //        personnelChange.ChangePostAmount = personnel.ChangePostAmount;
                //        personnelChange.Bank = personnel.Bank;
                //        personnelChange.BankAccount = personnel.BankAccount;
                //        personnelChange.QQ = personnel.QQ;
                //        personnelChange.Email = personnel.Email;
                //        personnelChange.Hobby = personnel.Hobby;
                //        personnelChange.SocietySecurityNumber = personnel.SocietySecurityNumber;
                //        personnelChange.MaritalStatus = personnel.MaritalStatus;
                //        personnelChange.LengthOfSchooling = personnel.LengthOfSchooling;
                //        personnelChange.JobNature = personnel.JobNature;
                //        personnelChange.PersonnelStatus = "在职";
                //        personnelChange.ArchivePosition = personnel.ArchivePosition;
                //        personnelChange.TakeJobDate = personnel.TakeJobDate;

                //        if (personnel.Photo != null)
                //        {
                //            personnelChange.Photo = personnel.Photo;
                //        }

                //        if (personnel.Annex != null)
                //        {
                //            personnelChange.Annex = personnel.Annex;
                //            personnelChange.AnnexName = personnel.AnnexName;
                //        }

                //        if (personnel.ResumeID != 0)
                //        {
                //            personnelChange.ResumeID = personnel.ResumeID;
                //        }

                //        personnelChange.Remark = personnel.Remark;
                //        personnelChange.ChangerCode = BasicInfo.LoginID;
                //        personnelChange.ChangeTime = ServerTime.Time;

                //        personnel.PersonnelStatus = 3;
                //        personnel.DimissionDate = personnelContractNew.EndTime;

                //        if (!new PersonnelArchiveServer().UpdatePersonnelArchive(personnelChange, personnel, out error))
                //        {
                //            error = "信息有误，请检查！";
                //            return false;
                //        }
                //    }
                //}

                datatContxt.SubmitChanges();

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
