using System;
using System.Linq;
using PlatformManagement;
using ServerModule;
using GlobalObject;
using System.Data;

namespace Service_Peripheral_HR
{
    /// <summary>
    /// 岗位调动服务类
    /// </summary>
    class PostChangeServer : Service_Peripheral_HR.IPostChangeServer
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
        /// 通过编号获得岗位调动信息
        /// </summary>
        /// <param name="billNo">单号</param>
        /// <param name="error">错误信息</param>
        /// <returns>返回数据集</returns>
        public HR_PostChange GetPostChangeByBillNo(int billNo,out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_PostChange
                             where a.ID == billNo
                             select a;

                if (result.Count() == 1)
                {
                    return result.Single();
                }
                else
                {
                    error = "未找到记录";
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
        /// 通过员工编号获得员工的调岗记录
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <param name="error">错误信息</param>
        /// <returns>返回数据集</returns>
        public DataTable GetPostChangeByWorkID(string workID, out string error)
        {
            error = "";

            try
            {
                string sql = "select CONVERT(varchar(10) , 申请日期, 20 ) as 申请日期," +
                             "原部门,原工作岗位,申请部门,申请岗位,调动原因 from View_HR_PostChange " +
                             "where 员工编号='" + workID + "' and 单据状态='已完成'";

                sql += " order by ID";

                DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

                return dt;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// 获取所有岗位调动信息
        /// </summary>
        /// <param name="returnInfo">岗位调动信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool GetAllPostChange(out IQueryResult returnInfo, out string error)
        {
            returnInfo = null;
            error = null;

            IAuthorization serverAuthorization = PlatformFactory.GetObject<IAuthorization>();
            IQueryResult qr = null;

            if (QueryResultFilter == null)
            {
                qr = serverAuthorization.Query("工作岗位调动申请", null);
            }
            else
            {
                qr = serverAuthorization.Query("工作岗位调动申请", null, QueryResultFilter);
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
        /// 新增岗位调动申请单
        /// </summary>
        /// <param name="postChange">岗位调动申请信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回大于0的整数，失败返回0</returns>
        public int AddPostChange(HR_PostChange postChange,out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_PostChange
                             where a.WorkID == postChange.WorkID && a.NewPostID == postChange.NewPostID
                             && a.PostID == postChange.PostID && a.NewDeptCode == postChange.NewDeptCode
                             select a;

                if (result.Count() > 0)
                {
                    error = "同部门同岗位的申请已经提交过一次！不能重复提交";
                    return 0;
                }

                dataContxt.HR_PostChange.InsertOnSubmit(postChange);
                dataContxt.SubmitChanges();

                var resultlist = from a in dataContxt.HR_PostChange
                             where a.WorkID == postChange.WorkID && a.Date == postChange.Date
                             select a;

                if (resultlist.Count() > 0)
                {
                    return resultlist.Single().ID;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return 0;
            }
        }

        /// <summary>
        /// 修改岗位调动申请单
        /// </summary>
        /// <param name="postChange">岗位调动申请信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回大于0的整数，失败返回0</returns>
        public bool UpdatePostChange(HR_PostChange postChange, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_PostChange
                             where a.WorkID == postChange.WorkID && a.NewPostID == postChange.NewPostID
                             && a.PostID == postChange.PostID && a.NewDeptCode == postChange.NewDeptCode
                             select a;

                if (result.Count() == 1)
                {
                    HR_PostChange list = result.Single();

                    list.BillStatus = PostChangeStatus.等待调出部门负责人审核.ToString();
                    list.ChangeReason = postChange.ChangeReason;
                    list.Date = postChange.Date;
                    list.NewDeptCode = postChange.NewDeptCode;
                    list.NewPostID = postChange.NewPostID;
                    list.PostID = postChange.PostID;
                    list.DeptCode = postChange.DeptCode;

                    list.GeneralManager = "";
                    list.GM_Authorize = false;
                    list.GM_Opinion = "";
                    list.HR_Authorize = false;
                    list.HR_Director = "";
                    list.HR_Opinion = "";
                    list.NewDeptAuthorize = false;
                    list.NewDeptDirector = "";
                    list.NewDeptOpinion = "";
                    list.NewLearder = "";
                    list.NewlearderAuthorize = false;
                    list.NewLearderOpinion = "";
                    list.OldDeptAuthorize = false;
                    list.OldDeptDirector = "";
                    list.OldDeptOpinion = "";
                    list.OldLearder = "";
                    list.OldLearderAuthorize = false;
                    list.OldLearderOpinion = "";
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
        /// 调出部门主管审批修改岗位调动表
        /// </summary>
        /// <param name="id">申请编号</param>
        /// <param name="oldDeptOpinion">调出部门主管意见</param>
        /// <param name="oldDeptAuthorize">调出部门是否批准</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回false</returns>
        public bool UpdateOldDept(int id, string oldDeptOpinion, bool oldDeptAuthorize,out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;
                var result = from a in dataContxt.HR_PostChange
                             where a.ID == id
                             select a;

                if (result.Count() != 1)
                {
                    error = "信息有误，请检查！";
                    return false;
                }

                HR_PostChange postChange = result.Single();

                postChange.OldDeptOpinion = oldDeptOpinion;
                postChange.OldDeptAuthorize = oldDeptAuthorize;
                postChange.OldDeptDirector = BasicInfo.LoginID;
                postChange.OldDeptSignatureDate = ServerTime.Time;

                if (oldDeptAuthorize == false)
                {
                    postChange.BillStatus = PostChangeStatus.未批准.ToString();
                }
                else
                {
                    postChange.BillStatus = PostChangeStatus.等待调出分管领导审核.ToString();
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
        /// 调出部门分管审批修改岗位调动表
        /// </summary>
        /// <param name="id">申请编号</param>
        /// <param name="oldLearderOpinion">调出部门分管意见</param>
        /// <param name="oldLearderAuthorize">调出部门是否批准</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回false</returns>
        public bool UpdateOldLearder(int id, string oldLearderOpinion, bool oldLearderAuthorize, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;
                var result = from a in dataContxt.HR_PostChange
                             where a.ID == id
                             select a;

                if (result.Count() != 1)
                {
                    error = "信息有误，请检查！";
                    return false;
                }

                HR_PostChange postChange = result.Single();

                postChange.OldLearderOpinion = oldLearderOpinion;
                postChange.OldLearderAuthorize = oldLearderAuthorize;
                postChange.OldLearder = BasicInfo.LoginID;
                postChange.OldLearderDate = ServerTime.Time;

                if (oldLearderAuthorize == false)
                {
                    postChange.BillStatus = PostChangeStatus.未批准.ToString();
                }
                else
                {
                    postChange.BillStatus = PostChangeStatus.等待调入部门负责人审核.ToString();
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
        /// 调入部门主管审批修改岗位调动表
        /// </summary>
        /// <param name="id">申请编号</param>
        /// <param name="newDeptOpinion">调入部门主管意见</param>
        /// <param name="newDeptAuthorize">调入部门是否批准</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回false</returns>
        public bool UpdateNewDept(int id, string newDeptOpinion, bool newDeptAuthorize, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;
                var result = from a in dataContxt.HR_PostChange
                             where a.ID == id
                             select a;

                if (result.Count() != 1)
                {
                    error = "信息有误，请检查！";
                    return false;
                }

                HR_PostChange postChange = result.Single();

                postChange.NewDeptOpinion = newDeptOpinion;
                postChange.NewDeptAuthorize = newDeptAuthorize;
                postChange.NewDeptDirector = BasicInfo.LoginID;
                postChange.NewDeptSignatureDate = ServerTime.Time;

                if (newDeptAuthorize == false)
                {
                    postChange.BillStatus = PostChangeStatus.未批准.ToString();
                }
                else
                {
                    postChange.BillStatus = PostChangeStatus.等待调入分管领导审核.ToString();
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
        /// 调入部门分管审批修改岗位调动表
        /// </summary>
        /// <param name="id">申请编号</param>
        /// <param name="newLearderOpinion">调入部门分管意见</param>
        /// <param name="newLearderAuthorize">调入部门是否批准</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回false</returns>
        public bool UpdateNewLearder(int id, string newLearderOpinion, bool newLearderAuthorize, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;
                var result = from a in dataContxt.HR_PostChange
                             where a.ID == id
                             select a;

                if (result.Count() != 1)
                {
                    error = "信息有误，请检查！";
                    return false;
                }

                HR_PostChange postChange = result.Single();

                postChange.NewLearderOpinion = newLearderOpinion;
                postChange.NewlearderAuthorize = newLearderAuthorize;
                postChange.NewLearder = BasicInfo.LoginID;
                postChange.NewLearderDate = ServerTime.Time;

                if (newLearderAuthorize == false)
                {
                    postChange.BillStatus = PostChangeStatus.未批准.ToString();
                }
                else
                {
                    postChange.BillStatus = PostChangeStatus.等待调入分管领导审核.ToString();
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
        /// 公司办审批修改岗位调动表
        /// </summary>
        /// <param name="id">申请编号</param>
        /// <param name="hrOpinion">公司办意见</param>
        /// <param name="hrAuthorize">公司办是否批准</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回false</returns>
        public bool UpdateHRAuthor(int id, string hrOpinion,bool hrAuthorize, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;
                var result = from a in dataContxt.HR_PostChange
                             where a.ID == id
                             select a;

                if (result.Count() != 1)
                {
                    error = "信息有误，请检查！";
                    return false;
                }

                HR_PostChange postChange = result.Single();

                postChange.HR_Opinion = hrOpinion;
                postChange.HR_Authorize = hrAuthorize;
                postChange.HR_Director = BasicInfo.LoginID;
                postChange.HR_SignatureDate = ServerTime.Time;

                if (hrAuthorize == false)
                {
                    postChange.BillStatus = PostChangeStatus.未批准.ToString();
                }
                else
                {
                    postChange.BillStatus = PostChangeStatus.等待总经理批准.ToString();
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
        /// 总经理审批修改岗位调动表
        /// </summary>
        /// <param name="id">申请编号</param>
        /// <param name="gmOpinion">总经理意见</param>
        /// <param name="authorize">总经理是否批准</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回false</returns>
        public bool UpdateGMAuthor(int id, string gmOpinion, bool authorize, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;
                var result = from a in dataContxt.HR_PostChange
                             where a.ID == id
                             select a;

                if (result.Count() != 1)
                {
                    error = "信息有误，请检查！";
                    return false;
                }

                HR_PostChange postChange = result.Single();

                postChange.GM_Opinion = gmOpinion;
                postChange.GM_Authorize = authorize;
                postChange.GeneralManager = BasicInfo.LoginID;
                postChange.GM_SignatureDate = ServerTime.Time;

                if (authorize == false)
                {
                    postChange.BillStatus = PostChangeStatus.未批准.ToString();
                }
                else
                {
                    postChange.BillStatus = PostChangeStatus.等待原工作移交.ToString();
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
        /// 移交确认修改
        /// </summary>
        /// <param name="id">申请编号</param>
        /// <param name="flag">工作是否移交</param>
        /// <param name="name">员工姓名</param>
        /// <param name="workID">员工编号</param>
        /// <param name="personnelChange">档案历史数据集</param>
        /// <param name="dept">调入部门</param>
        /// <param name="postID">调入岗位编号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回false</returns>
        public bool UpdateWorkTurnOver(int id, bool flag, string name, string workID, HR_PersonnelArchiveChange personnelChange,
            string dept, int postID, out string error)
        {
            error = "";
            bool isfinish = false;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;
                var result = from a in dataContxt.HR_PostChange
                             where a.ID == id
                             select a;

                if (result.Count() != 1)
                {
                    error = "信息有误，请检查！";
                    return false;
                }

                HR_PostChange postChange = result.Single();

                if (postChange.BillStatus == PostChangeStatus.等待原工作移交.ToString())
                {
                    postChange.TurnOverPeople = BasicInfo.LoginID;
                    postChange.TurnOverDate = ServerTime.Time;
                    postChange.IsWorkTurnOver = flag;

                    if (flag)
                    {
                        postChange.BillStatus = PostChangeStatus.等待人事档案调动.ToString();
                    }
                }
                else if (postChange.BillStatus == PostChangeStatus.等待人事档案调动.ToString())
                {
                    postChange.FilesPeople = BasicInfo.LoginID;
                    postChange.FilesDate = ServerTime.Time;
                    postChange.IsPersonnelFiles = flag;

                    if (flag)
                    {
                        postChange.BillStatus = PostChangeStatus.等待信息化人员确认.ToString();
                    }
                }
                else if (postChange.BillStatus == PostChangeStatus.等待信息化人员确认.ToString())
                {
                    postChange.ITPeople = BasicInfo.LoginID;
                    postChange.ITDate = ServerTime.Time;
                    postChange.IsIT = flag;

                    if (flag)
                    {
                        postChange.BillStatus = PostChangeStatus.等待固定资产人员确认.ToString();
                    }
                }
                else if (postChange.BillStatus == PostChangeStatus.等待固定资产人员确认.ToString())
                {
                    postChange.DormPeople = BasicInfo.LoginID;
                    postChange.DormDate = ServerTime.Time;
                    postChange.IsDorm = flag;

                    if (flag)
                    {
                        postChange.BillStatus = PostChangeStatus.已完成.ToString();
                        isfinish = true;
                    }
                }

                dataContxt.SubmitChanges();

                if (isfinish)
                {
                    var resultArchive = from c in dataContxt.HR_PersonnelArchive
                                        where c.Name == name && c.WorkID == workID
                                        select c;

                    if (resultArchive.Count() == 1)
                    {
                        HR_PersonnelArchive personnel = resultArchive.Single();

                        personnel.Dept = dept;
                        personnel.WorkPost = postID;
                        //personnel.Remark = ServerTime.Time + personnel.Name + " 从 " + personnelChange.DeptName + personnelChange.WorkPost +
                        //" 调到 " + new OrganizationServer().GetDeptByDeptCode(personnel.Dept).部门名称 + " " +
                        //new OperatingPostServer().GetOperatingPostByPostCode(personnel.WorkPost);

                        if (!new PersonnelArchiveServer().UpdatePersonnelArchiveByChangPost(personnelChange, personnel, out error))
                        {
                            error = "信息有误！";
                            return false;
                        }
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
        /// 删除岗位调动申请单
        /// </summary>
        /// <param name="id">编号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool DeletePostChange(int id, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_PostChange
                             where a.ID == id
                             select a;

                if (result.Single().BillStatus ==  PostChangeStatus.已完成.ToString())
                {
                    error = "单据已完成，不能删除！";
                    return false;
                }

                dataContxt.HR_PostChange.DeleteAllOnSubmit(result);
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
