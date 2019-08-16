using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ServerModule;
using PlatformManagement;
using GlobalObject;
using Service_Peripheral_HR.PersonnelDefiniens;
using DBOperate;
using System.Collections;


namespace Service_Peripheral_HR
{
    namespace PersonnelDefiniens
    {
        /// <summary>
        /// 参数类型
        /// </summary>
        public enum ParameterType
        {
            /// <summary>
            /// 工号
            /// </summary>
            工号,

            /// <summary>
            /// 姓名
            /// </summary>
            姓名,

            /// <summary>
            /// 部门编码或部门名称
            /// </summary>
            部门
        }
    }

    /// <summary>
    /// 人员档案管理类
    /// </summary>
    class PersonnelArchiveServer : Service_Peripheral_HR.IPersonnelArchiveServer
    {
        /// <summary>
        /// ServerModule人员服务组件
        /// </summary>
        IPersonnelInfoServer m_personnelInfo = ServerModuleFactory.GetServerModule<IPersonnelInfoServer>();

        /// <summary>
        /// 部门操作数据库接口
        /// </summary>
        DepotManagement.IDepartment m_deptServer = PlatformFactory.GetObject<DepotManagement.IDepartment>();

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
        /// 获取指定员工编号和姓名的人员岗位
        /// </summary>
        /// <param name="name">员工姓名</param>
        /// <param name="code">员工编号</param>
        /// <returns>岗位</returns>
        public string GetPersonnelArchiveByNameAndCode(string name,string code) 
        {
            string sql = "select 岗位 from View_HR_PersonnelArchive" +
                         " where 员工编号 = '" + code + "' and 员工姓名 ='" + name + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count > 0)
            {
                return dt.Rows[0]["岗位"].ToString();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 通过身份证号码获取员工档案信息视图
        /// </summary>
        /// <param name="cardID">身份证号码</param>
        /// <returns>返回满足条件的数据集</returns>
        public View_HR_PersonnelArchive GetPersonnelArchiveViewByCardID(string cardID)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from a in dataContxt.View_HR_PersonnelArchive
                         where a.身份证 == cardID
                         select a;

            if (result.Count() > 0)
            {
                return result.Single();
            }

            return null;
        }

        /// <summary>
        /// 通过身份证号码获取员工档案信息
        /// </summary>
        /// <param name="cardID">身份证号码</param>
        /// <returns>返回满足条件的数据集</returns>
        public HR_PersonnelArchive GetPersonnelArchiveByCardID(string cardID)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from a in dataContxt.HR_PersonnelArchive
                         where a.ID_Card == cardID
                         select a;

            if (result.Count() > 0)
            {
                return result.Single();
            }

            return null;
        }

        /// <summary>
        /// 获取某一岗位的所有人员
        /// </summary>
        /// <param name="postName">岗位名称</param>
        /// <returns>返回满足条件的数据集</returns>
        public IQueryable<View_HR_PersonnelArchive> GetPersonnelArchiveByPostName(string postName)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from a in dataContxt.View_HR_PersonnelArchive
                         where a.岗位 == postName && a.人员状态 != "离职"
                         select a;

            if (result.Count() > 0)
            {
                return result;
            }

            return null;
        }

        /// <summary>
        /// 指定员工编号和姓名的人员是否含有备注
        /// </summary>
        /// <param name="name">员工姓名</param>
        /// <param name="code">员工编号</param>
        /// <returns>是返回大于等于0的整形，否返回-1</returns>
        public int GetPersonnelArchiveRemark(string name, string code)
        {
            string sql = "select count(*) from View_HR_PersonnelArchive" +
                         " where 员工编号 = '" + code + "' and 员工姓名 ='" + name + "'"+
                         " and (备注 is not null and 备注 <> '' )";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count > 0)
            {
                return Convert.ToInt32(dt.Rows[0][0].ToString());
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// 获取指定员工编号和姓名的人员档案
        /// </summary>
        /// <param name="name">员工姓名</param>
        /// <param name="code">员工编号</param>
        /// <returns>返回数据集</returns>
        public DataTable GetPersonnelInfo(string name, string code)
        {
            string sql = "select * from View_HR_PersonnelArchive" +
                         " where 员工编号 = '" + code + "' and 员工姓名 ='" + name + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);
            return dt;
        }

        /// <summary>
        /// 获取人员状态下的人员档案
        /// </summary>
        /// <param name="status">员工状态</param>
        /// <returns>返回数据集</returns>
        public DataTable GetPersonnelStatus(string status)
        {
            string sql = "select * from View_HR_PersonnelArchive";

            if (!status.Equals("全部"))
            {
                sql += " where 人员状态 ='" + status + "'";
            }

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);
            return dt;
        }

        /// <summary>
        /// 获取指定员工编号和姓名的人员历史档案
        /// </summary>
        /// <param name="code">员工编号</param>
        /// <returns>返回数据集</returns>
        public View_HR_PersonnelArchiveChange GetPersonnelChangeInfo(string code)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.View_HR_PersonnelArchiveChange
                         where r.编号 == Convert.ToInt32(code)
                         select r;

            if (result.Count() > 0)
            {
                return result.Single();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取职员信息
        /// </summary>
        /// <param name="workID">工号</param>
        /// <returns>返回获取到的职员信息, 获取不到返回null</returns>
        public HR_PersonnelArchive GetPersonnelInfo(string workID)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.HR_PersonnelArchive
                         where r.WorkID == workID
                         select r;

            if (result.Count() > 0)
            {
                return result.Single();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 通过员工编号获取职员信息
        /// </summary>
        /// <param name="workID">工号</param>
        /// <returns>返回获取到的职员信息视图, 获取不到返回null</returns>
        public View_HR_PersonnelArchive GetPersonnelViewInfo(string workID)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.View_HR_PersonnelArchive
                         where r.员工编号 == workID
                         select r;

            if (result.Count() > 0)
            {
                return result.Single();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 通过员工姓名获取职员编号
        /// </summary>
        /// <param name="name">姓名</param>
        /// <returns>返回获取到的职员信息视图, 获取不到返回null</returns>
        public string GetPersonnelViewInfoByName(string name)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.View_HR_PersonnelArchive
                         where r.员工姓名 == name
                         select r.员工编号;

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
        /// 获取所有职员视图信息
        /// </summary>
        /// <returns>返回获取到的信息</returns>
        public IQueryable<View_SelectPersonnel> GetAllInfo()
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.View_SelectPersonnel
                         select r;

            return result;
        }

        /// <summary>
        /// 通过员工编号获取职员信息(简化视图)
        /// </summary>
        /// <param name="workID">工号</param>
        /// <returns>返回获取到的职员信息视图, 获取不到返回null</returns>
        public View_SelectPersonnel GetSelectPersonnel(string workID)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.View_SelectPersonnel
                         where r.员工编号 == workID
                         select r;

            if (result.Count() > 0)
            {
                return result.Single();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取直系负责人信息
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="managerType">权限类别 0：主管；1：负责人；2：分管领导</param>
        /// <returns>返回获取的负责人信息</returns>
        public IQueryable<View_SelectPersonnel> GetDirector(string deptCode, string managerType)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;
            IQueryable<string> dpWorkID = from r in dataContxt.HR_DeptManager
                                          where r.DeptCode == deptCode && r.ManagerType == managerType
                                          select r.ManagerWorkID;

            if (dpWorkID.Count() == 0)
            {
                return null;
            }

            var result = from r in dataContxt.View_SelectPersonnel
                         where dpWorkID.Contains(r.员工编号)
                         select r;

            return result;
        }

        /// <summary>
        /// 获取直系负责人信息
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="managerType">权限类别 0：主管；1：负责人；2：分管领导</param>
        /// <returns>返回获取的负责人信息</returns>
        public IQueryable<View_HR_PersonnelArchive> GetDeptDirector(string deptCode, string managerType)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;
            IQueryable<string> dpWorkID = from r in dataContxt.HR_DeptManager
                                          where r.DeptCode == deptCode && r.ManagerType == managerType
                                           && r.IsPermission == true
                                          select r.ManagerWorkID;

            if (dpWorkID.Count() == 0)
            {
                dpWorkID = from r in dataContxt.HR_DeptManager
                           where r.DeptCode == deptCode && r.ManagerType == managerType
                           select r.ManagerWorkID;

                if (dpWorkID.Count() == 0)
                {
                    return null;
                }
            }

            var result = from r in dataContxt.View_HR_PersonnelArchive
                         where dpWorkID.Contains(r.员工编号)
                         select r;

            return result;
        }

        /// <summary>
        /// 添加某部门负责人
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="lstPersonnel">负责人信息</param>
        /// <param name="managerType">权限类别 0：主管；1：负责人；2：分管领导</param>
        /// <param name="isPermission">是否有审批权限</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool AddDeptDirector(string deptCode, List<View_SelectPersonnel> lstPersonnel, string managerType, bool isPermission, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                foreach (var item in lstPersonnel)
                {
                    HR_DeptManager dp = new HR_DeptManager();

                    dp.DeptCode = deptCode;
                    dp.ManagerWorkID = item.员工编号;
                    dp.ManagerType = managerType;
                    dp.Recorder = BasicInfo.LoginID;
                    dp.RecordTime = ServerTime.Time;
                    dp.Remark = "";
                    dp.IsPermission = isPermission;

                    dataContxt.HR_DeptManager.InsertOnSubmit(dp);
                }

                dataContxt.SubmitChanges();
                return true;
            }
            catch (Exception exec)
            {
                error = exec.Message;
                return false;
            }
        }

        /// <summary>
        /// 获取部门负责人信息
        /// 某部门链式负责人, 如某班组的负责人有班组级、车间级、部门级三级的所有包容式负责人
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <returns>返回获取的部门负责人信息</returns>
        public IQueryable<View_HR_PersonnelArchive> GetFuzzyDeptDirector(string deptCode)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            IQueryable<string> dpWorkID = from r in dataContxt.HR_DeptManager
                                          where r.DeptCode.Length <= deptCode.Length
                                          && (deptCode.Substring(0, r.DeptCode.Length) == r.DeptCode)
                                          select r.ManagerWorkID;

            if (dpWorkID.Count() == 0)
            {
                return null;
            }

            var result = from r in dataContxt.View_HR_PersonnelArchive
                         where dpWorkID.Contains(r.员工编号)
                         select r;

            return result;
        }

        /// <summary>
        /// 删除某部门所有负责人
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="managerType">权限类别 0：主管；1：负责人；2：分管领导</param>
        public void DeleteDeptDirector(string deptCode,string managerType)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.HR_DeptManager
                         where r.DeptCode == deptCode && r.ManagerType == managerType
                         select r;

            dataContxt.HR_DeptManager.DeleteAllOnSubmit(result);
            dataContxt.SubmitChanges();
        }

        /// <summary>
        /// 判断用户是否是指定部门的负责人
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="userCode">用户编码</param>
        /// <returns>是返回true</returns>
        public bool IsDeptDirector(string deptCode, string userCode)
        {
            var result = GetFuzzyDeptDirector(deptCode);

            if (result == null || result.Count() == 0)
            {
                return false;
            }

            IQueryable<string> dpWorkID = from r in result
                                          where r.员工编号 == userCode
                                          select r.员工编号;
            return dpWorkID.Count() > 0;
        }

        /// <summary>
        /// 判断用户是否是指定人员的负责人
        /// </summary>
        /// <param name="underlingWorkID">下属工号</param>
        /// <param name="workID">需要判别负责人用户工号</param>
        /// <returns>是返回true</returns>
        public bool IsUserDirector(string underlingWorkID, string workID)
        {
            var result = GetFuzzyDeptDirector(GetPersonnelInfo(underlingWorkID).WorkID);

            if (result == null || result.Count() == 0)
            {
                return false;
            }

            IQueryable<string> dpWorkID = from r in result
                                          where r.员工编号 == workID
                                          select r.员工编号;
            return dpWorkID.Count() > 0;
        }

        /// <summary>
        /// 获取某部门所有职员视图信息
        /// </summary>
        /// <param name="dept">部门</param>
        /// <returns>返回获取到的信息</returns>
        public IQueryable<View_SelectPersonnel> GetAllInfo(string dept)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            return from r in dataContxt.View_SelectPersonnel
                   where r.部门编号.Contains(dept)
                   select r;
        }

        /// <summary>
        /// 获取多个部门所有职员视图信息
        /// </summary>
        /// <param name="sql">拼接好的sql语句</param>
        /// <returns>返回获取到的信息</returns>
        public List<View_SelectPersonnel> GetPersonByDept(string sql)
        {
            string strSql = "select 员工姓名,员工编号, 部门 from view_SelectPersonnel" +
                         " where 1=1 " + sql+" order by 部门,员工编号";

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            return dataContxt.ExecuteQuery<View_SelectPersonnel>(strSql, new object[] { }).ToList();
        }

        /// <summary>
        /// 获取某部门所有职员视图信息
        /// </summary>
        /// <param name="dept">部门</param>
        /// <returns>返回获取到的信息</returns>
        public DataTable GetAllInfoByDept(string dept)
        {
            string sql = "select 员工编号, 员工姓名, 部门, 岗位, 外部职称,内部级别, 入司时间, 手机 from View_HR_PersonnelArchive"+
                         " where 部门编号 like '" + dept + "%' and 人员状态 <> '离职'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);
            return dt;
        }

        /// <summary>
        /// 获取职员信息
        /// </summary>
        /// <param name="paramType">查询类型</param>
        /// <param name="parameter">查询信息</param>
        /// <returns>返回获取到的职员信息, 获取不到返回null</returns>
        public IQueryable<View_SelectPersonnel> GetPersonnelViewInfo(ParameterType paramType, string parameter)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;            

            switch (paramType)
            {
                case ParameterType.工号:
                    return from r in dataContxt.View_SelectPersonnel
                           where r.员工编号 == parameter
                           select r;

                case ParameterType.姓名:
                    return from r in dataContxt.View_SelectPersonnel
                           where r.员工姓名 == parameter
                           select r;

                case ParameterType.部门:
                    #region 2017.10.30, 夏石友，修改bug
                    // 所有子部门
                    List<string> depts = m_deptServer.GetChildDepartments(parameter);
                    #endregion

                    return from r in dataContxt.View_SelectPersonnel
                           where r.部门 == parameter || r.部门编号 == parameter || r.部门编号.Contains(parameter) || depts.Contains(r.部门编号)
                           select r;
            }

            return null;
        }

        /// <summary>
        /// 获取人员档案管理
        /// </summary>
        /// <param name="returnInfo">人员档案信息</param>
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
                qr = serverAuthorization.Query("人员档案管理", null);
            }
            else
            {
                qr = serverAuthorization.Query("人员档案管理", null, QueryResultFilter);
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
        /// 获取人员档案变更历史管理
        /// </summary>
        /// <param name="returnInfo">人员档案变更历史</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool GetAllChangeBill(out IQueryResult returnInfo, out string error)
        {
            returnInfo = null;
            error = null;

            IAuthorization serverAuthorization = PlatformFactory.GetObject<IAuthorization>();
            IQueryResult qr = null;

            if (QueryResultFilter == null)
            {
                qr = serverAuthorization.Query("人员档案变更历史查询", null);
            }
            else
            {
                qr = serverAuthorization.Query("人员档案变更历史查询", null, QueryResultFilter);
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
        /// 获得人员状态表
        /// </summary>
        /// <returns>返回人员状态数据集</returns>
        public DataTable GetPersonnelStatus()
        {
            string sql = "select * from HR_PersonnelStatus";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 通过状态名获得状态编号
        /// </summary>
        /// <param name="status">状态名</param>
        /// <returns>返回状态编号</returns>
        public int GetStatusByName(string status)
        {
            string sql = "select ID from dbo.HR_PersonnelStatus where status='" + status + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return Convert.ToInt32(dt.Rows[0]["ID"].ToString());
        }

        /// <summary>
        /// 通过状态编号获得状态名
        /// </summary>
        /// <param name="statusCode">状态编号</param>
        /// <returns>返回状态名</returns>
        public string GetStatusByID(int statusCode)
        {
            string sql = "select status from dbo.HR_PersonnelStatus where ID=" + statusCode;

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt.Rows[0]["status"].ToString();
        }

        /// <summary>
        /// 添加人员档案
        /// </summary>
        /// <param name="personnel">人员档案数据集</param>
        /// <param name="list">人员档案奖罚等信息数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool AddPersonnelArchive(HR_PersonnelArchive personnel,HR_PersonnelArchiveList list, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_PersonnelArchive
                             where a.ID_Card == personnel.ID_Card
                             || (a.WorkID == personnel.WorkID && a.Name == personnel.Name)
                             select a;

                if (result.Count() != 0)
                {
                    error = "姓名【" + personnel.Name + "】,身份证号【" + personnel.ID_Card + "】的人员档案已经存在！";
                    return false;
                }

                dataContxt.HR_PersonnelArchive.InsertOnSubmit(personnel);

                if (!new OperatingPostServer().UpdateDeptPost(personnel.Dept, Convert.ToInt32(personnel.WorkPost),out error))
                {
                    error = "添加失败，信息有误！";
                    return false;
                }

                dataContxt.HR_PersonnelArchiveList.InsertOnSubmit(list);

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
        /// 修改人员档案，把原始信息记录到变更表中
        /// </summary>
        /// <param name="personnelOld">原始的人员档案</param>
        /// <param name="personnelNew">修改后的人员档案</param>
        /// <param name="list">人员档案奖罚等信息数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool UpdatePersonnelArchive(HR_PersonnelArchiveChange personnelOld,
            HR_PersonnelArchive personnelNew,HR_PersonnelArchiveList list, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_PersonnelArchive
                             where (a.WorkID == personnelNew.WorkID && a.Name == personnelNew.Name)
                             || a.ID_Card == personnelNew.ID_Card
                             select a;

                if (result.Count() != 1)
                {
                    error = "信息有误，请检查信息后重新操作！";
                    return false;
                }

                HR_PersonnelArchive personnelList = result.Single();

                personnelList.Dept = personnelNew.Dept;
                personnelList.WorkPost = personnelNew.WorkPost;
                personnelList.JobTitleID = personnelNew.JobTitleID;
                personnelList.JobNature = personnelNew.JobNature;
                personnelList.JoinDate = personnelNew.JoinDate;
                personnelList.ArchivePosition = personnelNew.ArchivePosition;
                personnelList.BecomeRegularEmployeeDate = personnelNew.BecomeRegularEmployeeDate;
                personnelList.TakeJobDate = personnelNew.TakeJobDate;
                personnelList.MaritalStatus = personnelNew.MaritalStatus;
                personnelList.GraduationYear = personnelNew.GraduationYear;
                personnelList.LengthOfSchooling = personnelNew.LengthOfSchooling;
                personnelList.Birthday = personnelNew.Birthday;
                personnelList.Nationality = personnelNew.Nationality;
                personnelList.Race = personnelNew.Race;
                personnelList.Birthplace = personnelNew.Birthplace;
                personnelList.Party = personnelNew.Party; 
                personnelList.JoinDate = personnelNew.JoinDate;
                personnelList.ID_Card = personnelNew.ID_Card;
                personnelList.College = personnelNew.College;
                personnelList.EducatedDegree = personnelNew.EducatedDegree;
                personnelList.EducatedMajor = personnelNew.EducatedMajor;
                personnelList.FamilyAddress = personnelNew.FamilyAddress;
                personnelList.PostCode = "";
                personnelList.Phone = personnelNew.Phone;
                personnelList.MobilePhone = personnelNew.MobilePhone;
                personnelList.Bank = "";
                personnelList.BankAccount = "";
                personnelList.SocietySecurityNumber = "";
                personnelList.QQ = personnelNew.QQ;
                personnelList.Email = personnelNew.Email;
                personnelList.Hobby = personnelNew.Hobby;
                personnelList.Speciality = personnelNew.Speciality;
                personnelList.Sex = personnelNew.Sex;
                personnelList.ResumeID = personnelNew.ResumeID;
                personnelList.Photo = personnelNew.Photo;
                personnelList.Annex = personnelNew.Annex;
                personnelList.AnnexName = personnelNew.AnnexName;
                personnelList.ChangePostAmount = 0;
                personnelList.TrainingAmount = 0;
                personnelList.Remark = personnelNew.Remark;
                personnelList.ChangeAmount = 0;
                personnelList.PersonnelStatus = personnelNew.PersonnelStatus;
                personnelList.PY = personnelNew.PY;
                personnelList.WB = personnelNew.WB;
                personnelList.IsCore = personnelNew.IsCore;
                personnelList.JobLevelID = personnelNew.JobLevelID;
                personnelList.DimissionDate = personnelNew.DimissionDate;
                personnelList.Recorder = BasicInfo.LoginID;
                personnelList.RecordTime = ServerTime.Time;

                if (personnelNew.WorkPost != new OperatingPostServer().GetOperatingPostByPostName(personnelOld.WorkPost).岗位编号
                    || personnelNew.Dept != personnelOld.Dept)
                {
                    var resultPost = from c in dataContxt.HR_DeptPost
                                     where c.DeptCode == personnelNew.Dept && c.PostID == personnelNew.WorkPost
                                     select c;

                    if (resultPost.Count() == 0)
                    {
                        HR_DeptPost deptPost = new HR_DeptPost();

                        deptPost.DeptCode = personnelList.Dept;
                        deptPost.ExistAmount = 1;
                        deptPost.NumberOfPeople = 1;
                        deptPost.PostID = Convert.ToInt32(personnelList.WorkPost);
                        deptPost.Recorder = BasicInfo.LoginID;
                        deptPost.RecordTime = ServerTime.Time;
                        deptPost.Remark = "";

                        if (!new OperatingPostServer().AddDeptPost(deptPost, out error))
                        {
                            error = "岗位信息有误！";
                            return false;
                        }
                    }
                    else
                    {
                        if (!new OperatingPostServer().UpdateDeptPost(personnelNew.Dept, Convert.ToInt32(personnelNew.WorkPost), out error))
                        {
                            error = "修改失败，信息有误！";
                            return false;
                        }
                    }

                    if (!new OperatingPostServer().UpdateLessDeptPost(personnelOld.Dept,
                        new OperatingPostServer().GetOperatingPostByPostName(personnelOld.WorkPost).岗位编号, out error))
                    {
                        error = "修改失败，信息有误！";
                        return false;
                    }
                }

                if (personnelNew.PersonnelStatus == 3)
                {
                    if (!new OperatingPostServer().UpdateLessDeptPost(personnelOld.Dept,
                        new OperatingPostServer().GetOperatingPostByPostName(personnelOld.WorkPost).岗位编号, out error))
                    {
                        error = "修改失败，信息有误！";
                        return false;
                    }

                    if (!new LaborContractServer().UpdatePersonnelContract(personnelNew.WorkID, Convert.ToDateTime(personnelNew.DimissionDate)))
                    {
                        error = "修改失败，信息有误！";
                        return false;
                    }

                    IWarningNotice m_warningNotice = PlatformFactory.GetObject<IWarningNotice>();
                    Dictionary<string, string> dicParams = new Dictionary<string, string>();

                    dicParams.Add("附加信息1", "员工合同管理");
                    dicParams.Add("附加信息2", personnelNew.WorkID);

                    List<Flow_WarningNotice> notices = PlatformFactory.GetObject<IWarningNotice>().GetWarningNotice(dicParams);

                    if (notices != null)
                    {
                        try
                        {
                            foreach (Flow_WarningNotice item in notices)
                            {
                                m_warningNotice.ReadWarningNotice(BasicInfo.LoginID, item.序号);
                            }
                        }
                        catch (Exception)
                        {
                            dataContxt.HR_PersonnelArchiveChange.InsertOnSubmit(personnelOld);
                            dataContxt.SubmitChanges();
                        }
                    }
                }

                var resultList = from a in dataContxt.HR_PersonnelArchiveList
                                 where a.WorkID == list.WorkID
                                 select a;

                if (resultList.Count() > 0)
                {
                    dataContxt.HR_PersonnelArchiveList.DeleteAllOnSubmit(resultList);
                }

                if (list.Dimission != "" || list.DimissionView != "" || list.InMedicalHistory != ""
                    || list.MedicalHistory != "" || list.Performance != "" || list.Regularization != ""
                    || list.RewardPunish != "")
                {
                    dataContxt.HR_PersonnelArchiveList.InsertOnSubmit(list);
                }

                //dataContxt.HR_PersonnelArchiveChange.InsertOnSubmit(personnelOld);
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
        /// 修改人员档案，把原始信息记录到变更表中
        /// </summary>
        /// <param name="personnelOld">原始的人员档案</param>
        /// <param name="personnelNew">修改后的人员档案</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool UpdatePersonnelArchiveByChangPost(HR_PersonnelArchiveChange personnelOld, HR_PersonnelArchive personnelNew, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                if (personnelNew.WorkPost != new OperatingPostServer().GetOperatingPostByPostName(
                    personnelOld.WorkPost).岗位编号)
                {
                    var resultManager = from c in dataContxt.HR_DeptManager
                                        where c.ManagerWorkID == personnelNew.WorkID && c.DeptCode == personnelNew.Dept
                                        select c;

                    if (resultManager.Count() > 0)
                    {
                        dataContxt.HR_DeptManager.DeleteAllOnSubmit(resultManager);
                    }

                    var resultPost = from c in dataContxt.HR_DeptPost
                                     where c.DeptCode == personnelNew.Dept && c.PostID == personnelNew.WorkPost
                                     select c;

                    if (resultPost.Count() == 0)
                    {
                        HR_DeptPost deptPost = new HR_DeptPost();

                        deptPost.DeptCode = personnelNew.Dept;
                        deptPost.ExistAmount = 1;
                        deptPost.NumberOfPeople = 0;
                        deptPost.PostID = Convert.ToInt32(personnelNew.WorkPost);
                        deptPost.Recorder = BasicInfo.LoginID;
                        deptPost.RecordTime = ServerTime.Time;
                        deptPost.Remark = "";

                        if (!new OperatingPostServer().AddDeptPost(deptPost, out error))
                        {
                            error = "岗位信息有误！";
                            return false;
                        }
                    }
                    else
                    {
                        if (!new OperatingPostServer().UpdateDeptPost(personnelNew.Dept, Convert.ToInt32(personnelNew.WorkPost), out error))
                        {
                            error = "添加失败，信息有误！";
                            return false;
                        }
                    }

                    if (!new OperatingPostServer().UpdateLessDeptPost(personnelOld.Dept,
                        new OperatingPostServer().GetOperatingPostByPostName(personnelOld.WorkPost).岗位编号, out error))
                    {
                        error = "添加失败，信息有误！";
                        return false;
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
        /// 批量插入人员档案
        /// </summary>
        /// <param name="personnelArchive">人员档案列表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>插入成功返回True，插入失败返回False</returns>
        public bool InsertPersonnelArchiveInfo(DataTable personnelArchive, out string error)
        {
            error = null;

            string strTemp = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                for (int i = 0; i < personnelArchive.Rows.Count; i++)
                {
                    strTemp = personnelArchive.Rows[i]["身份证号码"].ToString().Trim();

                    if (strTemp == "")
                    {
                        personnelArchive.Rows.RemoveAt(i);
                        continue;
                    }

                    var result = from a in dataContxt.HR_PersonnelArchive
                                 where a.ID_Card == personnelArchive.Rows[i]["身份证号码"].ToString().Trim()
                                 && a.Name == personnelArchive.Rows[i]["员工姓名"].ToString().Trim()
                                 select a;

                    //HR_PersonnelArchive personnel = result.Single();

                    //personnel.MobilePhone = personnelArchive.Rows[i]["手机"].ToString();

                    if (result.Count() > 0)
                    {
                        error = "【" + personnelArchive.Rows[i]["员工姓名"].ToString().Trim() + "】身份证为【"
                            + personnelArchive.Rows[i]["身份证号码"].ToString().Trim() + "】的员工档案已经存在！";
                        continue;
                    }

                    HR_PersonnelArchive personnelList = new HR_PersonnelArchive();

                    string workID = personnelArchive.Rows[i]["员工编号"].ToString().Trim();

                    //if (workID.Length != 4)
                    //{
                    //    if (workID.Length == 1)
                    //    {
                    //        workID = "000" + personnelArchive.Rows[i]["员工编号"].ToString().Trim();
                    //    }

                    //    else if (workID.Length == 2)
                    //    {
                    //        workID = "00" + personnelArchive.Rows[i]["员工编号"].ToString().Trim();
                    //    }

                    //    else if (workID.Length == 3)
                    //    {
                    //        workID = "0" + personnelArchive.Rows[i]["员工编号"].ToString().Trim();
                    //    }
                    //}

                    personnelList.WorkID = workID;
                    personnelList.Name = personnelArchive.Rows[i]["员工姓名"] == DBNull.Value ? ""
                       : personnelArchive.Rows[i]["员工姓名"].ToString().Trim();
                    personnelList.Sex = personnelArchive.Rows[i]["性别"] == DBNull.Value ? ""
                       : personnelArchive.Rows[i]["性别"].ToString().Trim();
                    personnelList.Birthday = Convert.ToDateTime(personnelArchive.Rows[i]["出生日期"]);
                    personnelList.Nationality = personnelArchive.Rows[i]["国籍"] == DBNull.Value ? ""
                       : personnelArchive.Rows[i]["国籍"].ToString().Trim();
                    personnelList.Race = personnelArchive.Rows[i]["民族"] == DBNull.Value ? ""
                       : personnelArchive.Rows[i]["民族"].ToString().Trim();
                    personnelList.Birthplace = personnelArchive.Rows[i]["籍贯"] == DBNull.Value ? ""
                       : personnelArchive.Rows[i]["籍贯"].ToString().Trim();
                    personnelList.Party = personnelArchive.Rows[i]["政治面貌"] == DBNull.Value ? ""
                       : personnelArchive.Rows[i]["政治面貌"].ToString().Trim();
                    personnelList.ID_Card = personnelArchive.Rows[i]["身份证号码"] == DBNull.Value ? ""
                       : personnelArchive.Rows[i]["身份证号码"].ToString().Trim();
                    personnelList.College = personnelArchive.Rows[i]["院校名称"] == DBNull.Value ? ""
                       : personnelArchive.Rows[i]["院校名称"].ToString().Trim();
                    personnelList.EducatedDegree = personnelArchive.Rows[i]["文化程度"] == DBNull.Value ? ""
                       : personnelArchive.Rows[i]["文化程度"].ToString().Trim();
                    personnelList.EducatedMajor = personnelArchive.Rows[i]["所学专业"] == DBNull.Value ? ""
                       : personnelArchive.Rows[i]["所学专业"].ToString().Trim();
                    personnelList.FamilyAddress = personnelArchive.Rows[i]["家庭地址"] == DBNull.Value ? ""
                       : personnelArchive.Rows[i]["家庭地址"].ToString().Trim();
                    personnelList.PostCode = personnelArchive.Rows[i]["邮编"] == DBNull.Value ? ""
                       : personnelArchive.Rows[i]["邮编"].ToString().Trim();
                    personnelList.Phone = personnelArchive.Rows[i]["电话"] == DBNull.Value ? ""
                       : personnelArchive.Rows[i]["电话"].ToString().Trim();
                    personnelList.Speciality = personnelArchive.Rows[i]["特长"] == DBNull.Value ? ""
                       : personnelArchive.Rows[i]["特长"].ToString().Trim();
                    personnelList.MobilePhone = personnelArchive.Rows[i]["手机"] == DBNull.Value ? ""
                       : personnelArchive.Rows[i]["手机"].ToString().Trim();
                    personnelList.Hobby = personnelArchive.Rows[i]["爱好"] == DBNull.Value ? ""
                       : personnelArchive.Rows[i]["爱好"].ToString().Trim();
                    personnelList.QQ = personnelArchive.Rows[i]["QQ号码"] == DBNull.Value ? ""
                       : personnelArchive.Rows[i]["QQ号码"].ToString().Trim();
                    personnelList.Email = personnelArchive.Rows[i]["电子邮箱"] == DBNull.Value ? ""
                       : personnelArchive.Rows[i]["电子邮箱"].ToString().Trim();
                    personnelList.Bank = personnelArchive.Rows[i]["开户银行"] == DBNull.Value ? ""
                       : personnelArchive.Rows[i]["开户银行"].ToString().Trim();
                    personnelList.BankAccount = personnelArchive.Rows[i]["银行账号"] == DBNull.Value ? ""
                       : personnelArchive.Rows[i]["银行账号"].ToString().Trim();
                    personnelList.SocietySecurityNumber = personnelArchive.Rows[i]["社会保障号"] == DBNull.Value ? ""
                       : personnelArchive.Rows[i]["社会保障号"].ToString().Trim();
                    personnelList.TrainingAmount = Convert.ToInt32(personnelArchive.Rows[i]["培训次数"].ToString());
                    personnelList.ChangePostAmount = Convert.ToInt32(personnelArchive.Rows[i]["调动次数"].ToString());
                    personnelList.ChangeAmount = Convert.ToInt32(personnelArchive.Rows[i]["变更次数"].ToString());
                    personnelList.MaritalStatus = personnelArchive.Rows[i]["婚姻状况"] == DBNull.Value ? ""
                       : personnelArchive.Rows[i]["婚姻状况"].ToString().Trim();
                    personnelList.LengthOfSchooling = personnelArchive.Rows[i]["学制"] == DBNull.Value ? ""
                       : personnelArchive.Rows[i]["学制"].ToString().Trim();
                    personnelList.JobNature = personnelArchive.Rows[i]["工作性质"] == DBNull.Value ? ""
                       : personnelArchive.Rows[i]["工作性质"].ToString().Trim();
                    personnelList.JoinDate = Convert.ToDateTime(personnelArchive.Rows[i]["入司日期"].ToString());

                    personnelList.ArchivePosition = personnelArchive.Rows[i]["个人档案所在地"] == DBNull.Value ? ""
                      : personnelArchive.Rows[i]["个人档案所在地"].ToString().Trim();

                    if (personnelArchive.Rows[i]["毕业时间"].ToString().Trim() != "")
                    {
                        personnelList.GraduationYear = Convert.ToInt32(personnelArchive.Rows[i]["毕业时间"].ToString().Trim());
                    }

                    if (personnelArchive.Rows[i]["核心员工"].ToString().Trim() == "是")
                    {
                        personnelList.IsCore = true;
                    }
                    else
                    {
                        personnelList.IsCore = false;
                    }


                    if (personnelArchive.Rows[i]["转正日期"].ToString().Trim() != "")
                    {
                        personnelList.BecomeRegularEmployeeDate = Convert.ToDateTime(personnelArchive.Rows[i]["转正日期"].ToString().Trim());
                    }

                    if (personnelArchive.Rows[i]["参加工作日期"].ToString().Trim() != "")
                    {
                        string date = personnelArchive.Rows[i]["参加工作日期"].ToString().Trim();

                        if (personnelArchive.Rows[i]["参加工作日期"].ToString().Trim().Length == 4)
                        {
                            date = personnelArchive.Rows[i]["参加工作日期"].ToString().Trim() + "-01-01";
                        }

                        personnelList.TakeJobDate = Convert.ToDateTime(date);
                    }

                    personnelList.PersonnelStatus = new PersonnelArchiveServer().GetStatusByName(
                        personnelArchive.Rows[i]["人员状态"].ToString().Trim());

                    personnelList.Dept = new OrganizationServer().GetDeptCode(personnelArchive.Rows[i]["班组"].ToString().Trim());
                    personnelList.WorkPost = new OperatingPostServer().GetOperatingPostByPostName(
                        personnelArchive.Rows[i]["岗位"].ToString().Trim()).岗位编号;

                    personnelList.JobTitleID = new JobTitleServer().GetJobTitleByJobName(personnelArchive.Rows[i]["外部职称"].ToString().Trim());
                    personnelList.JobLevelID = new JobTitleServer().GetJobTitleByJobName(personnelArchive.Rows[i]["内部级别"].ToString().Trim());
                    personnelList.Recorder = BasicInfo.LoginID;
                    personnelList.RecordTime = ServerTime.Time;
                    personnelList.Remark = personnelArchive.Rows[i]["备注"].ToString().Trim();
                    personnelList.PY = UniversalFunction.GetPYWBCode(personnelArchive.Rows[i]["员工姓名"].ToString().Trim(), "PY");
                    personnelList.WB = UniversalFunction.GetPYWBCode(personnelArchive.Rows[i]["员工姓名"].ToString().Trim(), "WB");

                    dataContxt.HR_PersonnelArchive.InsertOnSubmit(personnelList);

                    var resultDept = from e in dataContxt.HR_DeptPost
                                     where e.DeptCode == personnelList.Dept && e.PostID == Convert.ToInt32(personnelList.WorkPost)
                                     select e;

                    if (resultDept.Count() == 0)
                    {
                        HR_DeptPost deptPost = new HR_DeptPost();

                        deptPost.DeptCode = personnelList.Dept;
                        deptPost.ExistAmount = 1;
                        deptPost.NumberOfPeople = 1;
                        deptPost.PostID = Convert.ToInt32(personnelList.WorkPost);
                        deptPost.Recorder = BasicInfo.LoginID;
                        deptPost.RecordTime = ServerTime.Time;
                        deptPost.Remark = "";

                        if (!new OperatingPostServer().AddDeptPost(deptPost, out error))
                        {
                            error = "添加失败，身份证为【" + strTemp + "】岗位信息有误！";
                            return false;
                        }
                    }
                    else
                    {
                        if (!new OperatingPostServer().UpdateDeptPost(personnelList.Dept, Convert.ToInt32(personnelList.WorkPost), out error))
                        {
                            error = "添加失败，信息有误！";
                            return false;
                        }
                    }

                    //var resultList = from c in dataContxt.HR_Personnel
                    //                 where c.WorkID == personnelList.WorkID && c.Name == personnelList.Name
                    //                 select c;

                    //if (resultList.Count() < 0)
                    //{
                    //    HR_Personnel personnel = new HR_Personnel();

                    //    personnel.OnTheJob = true;
                    //    personnel.Name = personnelArchive.Rows[i]["员工姓名"].ToString().Trim();
                    //    personnel.IsOperationalUser = false;
                    //    personnel.WorkID = personnelArchive.Rows[i]["员工姓名"].ToString().Trim();
                    //    personnel.Dept = personnelList.Dept;
                    //    personnel.WorkPost = personnelList.WorkPost;
                    //    personnel.DeleteFlag = true;
                    //    personnel.PY = UniversalFunction.GetPYWBCode(personnelArchive.Rows[i]["员工姓名"].ToString().Trim(), "PY");
                    //    personnel.WB = UniversalFunction.GetPYWBCode(personnelArchive.Rows[i]["员工姓名"].ToString().Trim(), "WB");

                    //    m_personnelInfo.AddPersonnel(personnel, out error);
                    //}

                    strTemp = "";
                }

                dataContxt.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message + "身份证为" + strTemp;
                return false;
            }
        }

        /// <summary>
        /// 通过起始时间判断在职员工的考勤（已剔除不考勤）
        /// </summary>
        /// <param name="beginDate">起始时间</param>
        /// <param name="endDate">截止时间</param>
        /// <returns>返回数据集（员工编号，员工姓名，考勤方案）</returns>
        public DataTable GetPersonnelAttendance(DateTime beginDate,DateTime endDate)
        {
            string sql = @"select View_HR_PersonnelArchive.员工编号,View_HR_PersonnelArchive.员工姓名 ," +
                          " 考勤方案 from dbo.View_HR_PersonnelArchive left join dbo.View_HR_AttendanceSetting" +
                          " on View_HR_AttendanceSetting.员工编号=View_HR_PersonnelArchive.员工编号 " +
                          " where View_HR_PersonnelArchive.人员状态='在职' and 考勤方案 not like '%不考勤%' " +
                          " and 入司时间 <= '"+beginDate+"'" +
                          " or (离职时间 >'" + beginDate + "' and 离职时间 <'" + endDate + "' and 入司时间 <= '" + beginDate + "')";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);
            return dt;
        }

        /// <summary>
        /// 获得员工的最高部门
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <returns>返回部门名称和部门编号</returns>
        public DataTable GetHighestDept(string workID)
        {
            string sql = "select case when len(FatherCode)>2 then 'KF' else FatherCode end as deptCode" +
                        " from dbo.HR_PersonnelArchive join dbo.HR_Dept " +
                        " on HR_Dept.DeptCode=HR_PersonnelArchive.Dept where workid='" + workID + "'";
            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        #region 打卡号与工号映射

        /// <summary>
        /// 添加打卡号与工号映射表
        /// </summary>
        /// <param name="cardWork">打卡号与工号映射表数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True 失败返回false</returns>
        public bool AddCardIDWorkIDMapping(HR_CardID_WorkID_Mapping cardWork ,out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var resultPerson = from c in dataContxt.HR_PersonnelArchive
                                   where c.WorkID == cardWork.WorkID
                                   select c;

                if (resultPerson.Count() == 1)
                {
                    var dateTemp1 = from a in dataContxt.View_HR_PersonnelArchive
                                   where a.员工编号 == cardWork.WorkID
                                   select a;

                    if (dateTemp1.Count() != 1)
                    {
                        throw new Exception("人员档案异常");
                    }
                    else if (dateTemp1.Single().人员状态 != CE_HR_PersonnelStatus.在职.ToString())
                    {
                        throw new Exception("人员状态非【"+ CE_HR_PersonnelStatus.在职.ToString() +"】");
                    }

                    var dateTemp = from a in dataContxt.View_HR_AttendanceSetting
                                   where a.员工编号 == cardWork.WorkID
                                   select a;

                    if (dateTemp.Count() != 1)
                    {
                        throw new Exception("员工考勤方案异常");
                    }
                    else if (dateTemp.Single().考勤方案 == CE_HR_AttendanceScheme.不考勤.ToString())
                    {
                        throw new Exception("员工考勤方案为【"+ CE_HR_AttendanceScheme.不考勤.ToString() +"】");
                    }

                    

                    var result = from a in dataContxt.HR_CardID_WorkID_Mapping
                                 where a.WorkID == cardWork.WorkID
                                 select a;

                    if (result.Count() > 0)
                    {
                        HR_CardID_WorkID_Mapping cardID = result.Single();

                        cardID.CardID = cardWork.CardID;
                    }
                    else
                    {
                        dataContxt.HR_CardID_WorkID_Mapping.InsertOnSubmit(cardWork);
                    }

                    dataContxt.SubmitChanges();
                    return true;
                }
                else
                {
                    error = "员工工号输入有误，没有【"+cardWork.WorkID+"】的人员档案！";
                    return false;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 通过打卡号和员工工号删除打卡号与工号映射表的记录
        /// </summary>
        /// <param name="cardID">打卡号</param>
        /// <param name="workID">员工工号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True失败返回False</returns>
        public bool DeleteCardIDWorkIDMapping(string cardID, string workID, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_CardID_WorkID_Mapping
                             where a.WorkID == workID
                             select a;

                dataContxt.HR_CardID_WorkID_Mapping.DeleteAllOnSubmit(result);

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
        /// 获得打卡号与工号映射表
        /// </summary>
        /// <returns>返回数据集</returns>
        public DataTable GetCardIDWorkIDMapping()
        {
            string sql = @"select a.打卡号,a.员工工号,a.员工姓名,a.部门,"+
                          " a.岗位 from View_HR_CardID_WorkID_Mapping a" +
                          " join dbo.View_HR_PersonnelArchive b on  b.员工编号=a.员工工号" +
                          " left join dbo.View_HR_AttendanceSetting c on c.员工编号=b.员工编号 " +
                          " where b.人员状态='在职' and 考勤方案 not like '%不考勤%'";
            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        #endregion

        /// <summary>
        /// 批量修改部门/科室
        /// </summary>
        /// <param name="oldDept">原部门/科室编码</param>
        /// <param name="newDept">现部门/科室编码</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True失败返回false</returns>
        public bool UpdateBatchDept(string oldDept,string newDept,out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_PersonnelArchive
                             where a.Dept == oldDept
                             select a;

                if (result.Count() > 0)
                {
                    IQueryable<HR_PersonnelArchive> person = result;

                    foreach (var item in person)
                    {
                        HR_PersonnelArchive hr = item;

                        hr.Dept = newDept;

                        dataContxt.SubmitChanges();
                    }
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
        /// 通过员工编号获得奖罚等信息
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <returns>返回数据集</returns>
        public DataTable GetArchiveList(string workID)
        {
            string sql = "select * from HR_PersonnelArchiveList where workID='" + workID + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 导出员工的平均年龄及各年龄段的人数
        /// </summary>
        /// <param name="showTable">返回的数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true</returns>
        public bool ExcelPersonAge(out DataTable showTable,out string error)
        {
            error = "";
            showTable = null;

            IDBOperate dbOperate = CommentParameter.GetDBOperatorOfDepotManagement();

            Hashtable paramTable = new Hashtable();

            string productName = "HR_GetPersonAge";

            DataSet ds = new DataSet();
            Dictionary<OperateCMD, object> dicOperateCMD = dbOperate.RunProc_CMD(productName, ds);

            if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
            {
                error = Convert.ToString(dicOperateCMD[OperateCMD.Return_Errmsg]);
                return false;
            }

            showTable = ds.Tables[0];

            return true;
        }

        /// <summary>
        /// 导出员工的平均司龄及司龄段的人数
        /// </summary>
        /// <param name="showTable">返回的数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true</returns>
        public bool ExcelIncompanyYears(out DataTable showTable, out string error)
        {
            error = "";
            showTable = null;

            IDBOperate dbOperate = CommentParameter.GetDBOperatorOfDepotManagement();

            Hashtable paramTable = new Hashtable();

            string productName = "HR_GetIncompanyYears";

            DataSet ds = new DataSet();
            Dictionary<OperateCMD, object> dicOperateCMD = dbOperate.RunProc_CMD(productName, ds);

            if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
            {
                error = Convert.ToString(dicOperateCMD[OperateCMD.Return_Errmsg]);
                return false;
            }

            showTable = ds.Tables[0];

            return true;
        }

        /// <summary>
        /// 导出时间范围类的离职分析
        /// </summary>
        /// <param name="startDate">起始时间</param>
        /// <param name="endDate">终止时间</param>
        /// <param name="showTable">返回的数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true</returns>
        public bool ExcelDimission(DateTime startDate, DateTime endDate, out DataTable showTable, out string error)
        {
            error = "";
            showTable = null;

            IDBOperate dbOperate = CommentParameter.GetDBOperatorOfDepotManagement();

            Hashtable paramTable = new Hashtable();

            paramTable.Add("@starDate", startDate);
            paramTable.Add("@endDate", endDate);

            string productName = "HR_GetDimission";

            DataSet ds = new DataSet();
            Dictionary<OperateCMD, object> dicOperateCMD = dbOperate.RunProc_CMD(productName, ds,paramTable);

            if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
            {
                error = Convert.ToString(dicOperateCMD[OperateCMD.Return_Errmsg]);
                return false;
            }

            showTable = ds.Tables[0];

            return true;
        }

        /// <summary>
        /// 导出各部门的在职人员
        /// </summary>
        /// <param name="showTable">返回的数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true</returns>
        public bool ExcelOnjob(out DataTable showTable, out string error)
        {
            error = "";
            showTable = null;

            IDBOperate dbOperate = CommentParameter.GetDBOperatorOfDepotManagement();

            string productName = "HR_GetOnJob";

            DataSet ds = new DataSet();
            Dictionary<OperateCMD, object> dicOperateCMD = dbOperate.RunProc_CMD(productName, ds);

            if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
            {
                error = Convert.ToString(dicOperateCMD[OperateCMD.Return_Errmsg]);
                return false;
            }

            showTable = ds.Tables[0];

            return true;
        }

        /// <summary>
        /// 导出各学历人数
        /// </summary>
        /// <param name="showTable">返回的数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true</returns>
        public bool ExcelEducation(out DataTable showTable, out string error)
        {
            error = "";
            showTable = null;

            IDBOperate dbOperate = CommentParameter.GetDBOperatorOfDepotManagement();

            string productName = "HR_GetEducation";

            DataSet ds = new DataSet();
            Dictionary<OperateCMD, object> dicOperateCMD = dbOperate.RunProc_CMD(productName, ds);

            if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
            {
                error = Convert.ToString(dicOperateCMD[OperateCMD.Return_Errmsg]);
                return false;
            }

            showTable = ds.Tables[0];

            return true;
        }
    }
}
