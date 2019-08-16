using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformManagement;
using ServerModule;
using System.Data;

namespace Service_Peripheral_HR
{
    /// <summary>
    /// 员工离职申请操作类
    /// </summary>
    class DimissionServer : Service_Peripheral_HR.IDimissionServer
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
        /// 获取员工离职所有信息
        /// </summary>
        /// <param name="returnInfo">员工离职信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool GetAllDimission(out IQueryResult returnInfo, out string error)
        {
            returnInfo = null;
            error = null;

            IAuthorization serverAuthorization = PlatformFactory.GetObject<IAuthorization>();
            IQueryResult qr = null;

            if (QueryResultFilter == null)
            {
                qr = serverAuthorization.Query("员工离职申请查询", null);
            }
            else
            {
                qr = serverAuthorization.Query("员工离职申请查询", null, QueryResultFilter);
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
        /// 获取员工离职所有信息
        /// </summary>
        /// <param name="billID">单据编号</param>
        /// <returns>返回数据集</returns>
        public HR_DimissionBill GetAllDimission(int billID)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from a in dataContxt.HR_DimissionBill
                         where a.ID == billID
                         select a;

            if (result.Count() == 1)
            {
                HR_DimissionBill dimission = result.Single();
                return dimission;
            }

            return null;
        }

        /// <summary>
        /// 新增或修改员工离职申请信息
        /// </summary>
        /// <param name="dimission">员工离职数据集</param>
        /// <param name="type">进行步骤(提交申请/部门负责人审批/人力资源部审批/分管领导审批/总经理批准)</param>
        /// <param name="error">错误信息</param>
        /// <returns>返回单据编号，失败返回0</returns>
        public string AddAndUpdateDimission(HR_DimissionBill dimission,string type, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_DimissionBill
                             where a.WorkID == dimission.WorkID && a.Date.Month == dimission.Date.Month
                             && a.Date.Day == dimission.Date.Day
                             select a;

                switch (type)
                {
                    case "提交申请":

                        if (result.Count() > 0)
                        {
                            error = "不能重复提交离职申请！";
                            return "0";
                        }
                        
                        dataContxt.HR_DimissionBill.InsertOnSubmit(dimission);

                        break;
                    case "部门负责人审批":
                        if (result.Count() == 1)
                        {
                            HR_DimissionBill dimiList = result.Single();

                            dimiList.DeptAuthorize = dimission.DeptAuthorize;
                            dimiList.DeptOpinion = dimission.DeptOpinion;
                            dimiList.DeptSignature = dimission.DeptSignature;
                            dimiList.DeptSignatureDate = dimission.DeptSignatureDate;
                            dimiList.AllowDimissionDate = dimission.AllowDimissionDate;

                            if (!dimission.DeptAuthorize)
                            {
                                dimiList.BillStatus = DimissionBillStatus.已完成.ToString();
                            }
                            else
                            {
                                dimiList.BillStatus = DimissionBillStatus.等待人力资源审阅.ToString();
                            }
                        }
                        break;
                    case "人力资源部审批":
                        if (result.Count() == 1)
                        {
                            HR_DimissionBill dimiList = result.Single();

                            dimiList.HR_Opinion = dimission.HR_Opinion;
                            dimiList.HR_Signature = dimission.HR_Signature;
                            dimiList.HR_SignatureDate = dimission.HR_SignatureDate;
                            dimiList.BillStatus = DimissionBillStatus.等待分管领导审核.ToString();
                        }
                        break;
                    case "分管领导审批":
                        if (result.Count() == 1)
                        {
                            HR_DimissionBill dimiList = result.Single();

                            dimiList.LeaderAuthorize = dimission.LeaderAuthorize;
                            dimiList.LeaderOpinion = dimission.LeaderOpinion;
                            dimiList.LeaderSignature = dimission.LeaderSignature;
                            dimiList.LeaderSignatureDate = dimission.LeaderSignatureDate;

                            if (!dimission.LeaderAuthorize)
                            {
                                dimiList.BillStatus = DimissionBillStatus.已完成.ToString();
                            }
                            else
                            {
                                dimiList.BillStatus = DimissionBillStatus.等待总经理批准.ToString();
                            }
                        }
                        break;
                    case "总经理批准":
                        if (result.Count() == 1)
                        {
                            HR_DimissionBill dimiList = result.Single();

                            dimiList.GM_Authorize = dimission.GM_Authorize;
                            dimiList.GM_Opinion = dimission.GM_Opinion;
                            dimiList.GM_Signature = dimission.GM_Signature;
                            dimiList.GM_SignatureDate = dimission.GM_SignatureDate;
                            dimiList.BillStatus = DimissionBillStatus.已完成.ToString();

                            if (dimission.GM_Authorize)
                            {
                                if (!new OperatingPostServer().UpdateLessDeptPost(dimiList.Dept,
                                new OperatingPostServer().GetOperatingPostByPostName(dimiList.WorkPost).岗位编号, out error))
                                {
                                    error = "信息有误！";
                                    return "0";
                                }

                                var resultManager = from c in dataContxt.HR_DeptManager
                                                    where c.ManagerWorkID == dimission.WorkID
                                                    select c;

                                if (resultManager.Count() > 0)
                                {
                                    dataContxt.HR_DeptManager.DeleteAllOnSubmit(resultManager);
                                }

                                var resultStatus = from e in dataContxt.HR_PersonnelArchive
                                                   where e.WorkID == dimission.WorkID
                                                   select e;

                                if (resultStatus.Count() > 0)
                                {
                                    HR_PersonnelArchive personnel = resultStatus.Single();

                                    personnel.PersonnelStatus = 3;
                                    personnel.Remark = dimiList.AllowDimissionDate + "离职";
                                }

                                var resultLaborTemp = from j in dataContxt.View_HR_PersonnelLaborContract
                                                      where j.员工编号 == dimission.WorkID && j.类别 == "合同类"
                                                      select j;

                                if (resultLaborTemp.Count() > 0)
                                {
                                    int billID = resultLaborTemp.Single().编号;

                                    var resultLabor = from j in dataContxt.HR_PersonnelLaborContract
                                                      where j.ID == billID
                                                      select j;

                                    HR_PersonnelLaborContract labor = resultLabor.Single();

                                    if (resultLabor.Single().EndTime > dimission.AllowDimissionDate)
                                    {
                                        labor.LaborContractStatusID = 8;
                                    }
                                    else
                                    {
                                        labor.LaborContractStatusID = 7;
                                    }
                                }
                            }                            
                        }
                        break;
                    default:
                        break;
                }

                dataContxt.SubmitChanges();

                var resultList = from a in dataContxt.HR_DimissionBill
                             where a.WorkID == dimission.WorkID && a.Date == dimission.Date && a.Reason == dimission.Reason
                             select a;

                if (resultList.Count() > 0)
                {
                    return resultList.Single().ID.ToString();
                }
                else
                {
                    error = "未知编号！";
                    return "0";
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return "0";
            }
        }

        /// <summary>
        /// 删除离职申请表
        /// </summary>
        /// <param name="userCode">用户编号</param>
        /// <param name="allowDate">申请时间</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool DeleteDimission(string userCode, DateTime allowDate, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_DimissionBill
                             where a.WorkID == userCode && a.Date == allowDate
                             select a;

                if (result.Count() == 1)
                {
                    dataContxt.HR_DimissionBill.DeleteAllOnSubmit(result);
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
        /// 获得某个员工的岗位调动信息
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <param name="error">错误信息</param>
        /// <returns>返回数据集</returns>
        public DataTable GetDimissionBillByWorkID(string workID, out string error)
        {
            error = "";

            string sql = "SELECT 编号, 单据状态, 申请日期, 部门, 岗位, 离职原因 FROM View_HR_DimissionBill" +
                         " where 员工编号='" + workID + "'";
            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);
            return dt;
        }
    }
}
