using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GlobalObject;
using WebServerModule;
using PlatformManagement;
using ServerModule;

namespace WebServerModule
{
    /// <summary>
    /// 售后信息服务类
    /// </summary>
    public class ServiceFeedBack : WebServerModule.IServiceFeedBack
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
        /// 获得所有单据信息
        /// </summary>
        /// <param name="strBillStatus">单据状态</param>
        /// <param name="dtStartTime">起始时间</param>
        /// <param name="dtEndTime">结束时间</param>
        /// <returns>成功返回满足条件的数据集，失败返回错误信息</returns>
        public DataTable GetServiceTable(string strBillStatus, DateTime dtStartTime, DateTime dtEndTime)
        {
            string strSelect = "";

            if (strBillStatus != "全  部")
            {
                strSelect += "单据状态 = '" + strBillStatus + "' and ";
            }

            strSelect += "反馈日期 >= '" + dtStartTime + "' and 反馈日期 <= '" + dtEndTime + "'";

            if (!BasicInfo.DeptCode.Equals("YX") && !BasicInfo.RoleCodes.Contains("SYSTEM_ADMIN") 
                && !BasicInfo.RoleCodes.Contains("OPER-QA-0001"))
            {
                strSelect += " and 单据状态 <> '等待确认返回时间' and 单据状态 <> '等待主管审核'";
            }

            string strSql = "select * from View_S_ServiceFeedBack where " + strSelect + " order by 反馈单号";

            DataTable dt = GlobalObject.DatabaseServer.WebQueryInfo(strSql);

            return dt;
        }

        /// <summary>
        /// 根据车架号获得反馈单的信息
        /// </summary>
        /// <param name="vehicleShelfNumber">车架号</param>
        /// <returns>返回获得的信息</returns>
        public DataTable GetVehicleMaintenanceRecord(string vehicleShelfNumber)
        {
            string strSql = " select * from View_S_ServiceFeedBack " +
                            " where 车架号 = '" + vehicleShelfNumber + "'";

            return GlobalObject.DatabaseServer.WebQueryInfo(strSql);
        }

        /// <summary>
        /// 获得故障信息
        /// </summary>
        /// <param name="serviceID">函电单据号</param>
        /// <returns>成功返回满足条件的数据集，失败返回错误信息</returns>
        public DataTable GetBugMessageByServiceID(string serviceID)
        {
            string sql = "select * from dbo.OF_BugMessageInfo where ServiceID='" + serviceID + "'";

            DataTable dt = GlobalObject.DatabaseServer.WebQueryInfo(sql);

            return dt;
        }

    
        /// <summary>
        /// 质管部确认，修改表信息
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="ChargeSuggestion">质管部意见</param>
        /// <param name="dutyDept">责任部门</param>
        /// <param name="replyTime">要求回复时间</param>
        /// <param name="appCount">出现次数</param>
        /// <param name="associatedBillNo">问题相似的关联单据</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool UpdateZGAffirm(string billID, string ChargeSuggestion, string dutyDept,
                                   DateTime replyTime, int appCount, string associatedBillNo, out string error)
        {
            error = "";

            try
            {
                WebSeverDataContext dataContxt = WebDatabaseParameter.WebDataContext;

                var varData = from a in dataContxt.S_ServiceFeedBack
                              where a.FK_Bill_ID == billID
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数据有错误！";

                    return false;
                }
                else
                {                    
                    S_ServiceFeedBack list = varData.Single();

                    list.ZGChargeSuggestion = ChargeSuggestion;
                    list.DutyDept = dutyDept;
                    list.ReplyTime = replyTime;
                    list.AppearCount = appCount;                    
                    list.SignatureDate = ServerTime.Time;
                    list.Signature = BasicInfo.LoginID;

                    if (associatedBillNo == null)
                    {
                        list.Status = "等待责任部门确认";
                    }
                    else
                    {
                        list.Status = "单据完成";

                        var resultList = from c in dataContxt.S_ServiceFeedBack
                                         where c.FK_Bill_ID == associatedBillNo
                                         select c;

                        if (resultList.Count() == 1)
                        {
                            S_ServiceFeedBack feedback = resultList.Single();

                            list.Analyse = feedback.Analyse;
                            //list.foreverImplement = feedback.foreverImplement;
                            list.FinishClaim = feedback.FinishClaim;
                            list.IsFMEAfile = feedback.IsFMEAfile;
                            list.IsOpen = feedback.IsOpen;
                            list.Temporary = feedback.Temporary;
                            list.StockSuggestion = feedback.StockSuggestion;
                            list.SameBillNo = associatedBillNo;
                            list.DutyPerson = feedback.DutyPerson;
                            list.DutyDept = feedback.DutyDept;
                            list.Practicable = feedback.Practicable;
                            list.IsClose = "是";
                        }
                    }

                    dataContxt.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;

                return false;
            }

            return true;
        }

        /// <summary>
        /// 责任部门确认，修改表
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="dutyPerson">责任人</param>
        /// <param name="finish">完成要求</param>
        /// <param name="stock">库存产品意见</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool UpdateDutyDept(string billID, string dutyPerson, string finish, 
            string stock,out string error)
        {
            error = "";

            try
            {
                WebSeverDataContext dataContxt = WebDatabaseParameter.WebDataContext;

                var varData = from a in dataContxt.S_ServiceFeedBack
                              where a.FK_Bill_ID == billID
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数据有错误！";

                    return false;
                }
                else
                {
                    S_ServiceFeedBack list = varData.Single();

                    list.DutyDeptCharge = BasicInfo.LoginID;
                    list.DutyPerson = dutyPerson;
                    list.FinishClaim = finish;
                    list.StockSuggestion = stock;
                    list.DutyDeptChargeDate = ServerTime.Time;
                    list.Status = "等待责任人确认";

                    dataContxt.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;

                return false;
            }

            return true;
        }

        /// <summary>
        /// 责任人确认，修改表信息
        /// </summary>
        /// <param name="back">数据集对象</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool UpdateDutyPerson(S_ServiceFeedBack back,out string error)
        {
            error = "";

            try
            {
                WebSeverDataContext dataContxt = WebDatabaseParameter.WebDataContext;

                var varData = from a in dataContxt.S_ServiceFeedBack
                              where a.FK_Bill_ID == back.FK_Bill_ID
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数据有错误！";

                    return false;
                }
                else
                {
                    S_ServiceFeedBack list = varData.Single();

                    list.Temporary = back.Temporary;
                    list.Analyse = back.Analyse;
                    //list.foreverImplement = back.foreverImplement;
                    list.IsFMEAfile = back.IsFMEAfile;
                    list.IsOpen = back.IsOpen;
                    list.Status = "等待质管检查";
                    list.DutyPersonDate = DateTime.Now;

                    dataContxt.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;

                return false;
            }

            return true;
        }

        /// <summary>
        /// 质管部检验，修改表信息
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="cloes">是否关闭</param>
        /// <param name="practice">落实情况</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool UpdateZGCheck(string billID, string cloes, string practice, out string error)
        {
            error = "";

            try
            {
                WebSeverDataContext dataContxt = WebDatabaseParameter.WebDataContext;

                var varData = from a in dataContxt.S_ServiceFeedBack
                              where a.FK_Bill_ID == billID
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数据有错误！";

                    return false;
                }
                else
                {
                    S_ServiceFeedBack list = varData.Single();

                    list.ZGCheckName = BasicInfo.LoginID;
                    list.IsClose = cloes;
                    list.Practicable = practice;
                    list.Status = "单据完成";
                    list.ZGCheckDate = DateTime.Now;

                    dataContxt.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;

                return false;
            }

            return true;
        }

        
        /// <summary>
        /// 添加返回件
        /// </summary>
        /// <param name="dtAddTb">数据集</param>
        /// <param name="strDJH">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool InsertReplace(DataTable dtAddTb, string strDJH, out string error)
        {
            error = "";

            WebSeverDataContext dataContxt = WebDatabaseParameter.WebDataContext;

            try
            {
                if (DeleteReplace(strDJH, out error))
                {
                    for (int i = 0; i < dtAddTb.Rows.Count; i++)
                    {
                        S_ReplaceAccessory list = new S_ReplaceAccessory();

                        list.ServiceID = strDJH;
                        list.OldGoodsCode = dtAddTb.Rows[i]["OldGoodsCode"].ToString();
                        list.OldGoodsName = dtAddTb.Rows[i]["OldGoodsName"].ToString();
                        list.OldSpec = dtAddTb.Rows[i]["OldSpec"].ToString();
                        list.OldCvtID = dtAddTb.Rows[i]["OldCvtID"].ToString();
                        list.Count = Convert.ToInt32(dtAddTb.Rows[i]["Count"].ToString());
                        list.Unit = dtAddTb.Rows[i]["Unit"].ToString();
                        list.NewGoodsCode = dtAddTb.Rows[i]["NewGoodsCode"].ToString();
                        list.NewGoodsName = dtAddTb.Rows[i]["NewGoodsName"].ToString();
                        list.NewSpec = dtAddTb.Rows[i]["NewSpec"].ToString();
                        list.NewCvtID = dtAddTb.Rows[i]["NewCvtID"].ToString();
                        list.NewGoodsID = dtAddTb.Rows[i]["NewGoodsID"].ToString();
                        list.OldGoodsID = dtAddTb.Rows[i]["OldGoodsID"].ToString();
                        
                        if (dtAddTb.Rows[i]["BackTime"].ToString() != "")
                        {
                            list.BackTime = Convert.ToDateTime(dtAddTb.Rows[i]["BackTime"].ToString());
                        }

                        if (dtAddTb.Rows[i]["GiveOutDate"].ToString() != "")
                        {
                            list.GiveOutDate = Convert.ToDateTime(dtAddTb.Rows[i]["GiveOutDate"].ToString());
                        }

                        dataContxt.S_ReplaceAccessory.InsertOnSubmit(list);
                    }

                    dataContxt.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;

                return false;
            }

            return true;
        }

        /// <summary>
        /// 删除返回件
        /// </summary>
        /// <param name="strDJH">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool DeleteReplace(string strDJH, out string error)
        {
            error = "";

            try
            {
                WebSeverDataContext dataContxt = WebDatabaseParameter.WebDataContext;

                var vardata = from a in dataContxt.S_ReplaceAccessory
                              where a.ServiceID == strDJH
                              select a;

                if (vardata.Count() != 0)
                {
                    dataContxt.S_ReplaceAccessory.DeleteAllOnSubmit(vardata);
                }

                dataContxt.SubmitChanges();
            }
            catch (Exception ex)
            {
                error = ex.Message;

                return false;
            }

            return true;
        }

        /// <summary>
        /// 函电信息
        /// </summary>
        /// <param name="endTime">结束时间</param>
        /// <param name="starTime">起始时间</param>
        /// <returns>成功返回满足条件的数据集，失败返回null的dataTable</returns>
        public DataTable GetAfterService(string starTime, string endTime)
        {
            string sql = @"select * from View_S_AfterService where 1=1";

            if (starTime != "" && endTime != "")
            {
                sql += @" and 接函电时间>='" + starTime + "' and 接函电时间<'" + endTime + "'";
            }

            if (BasicInfo.ListRoles.Contains(CE_RoleEnum.营销普通人员.ToString()) &&
               !(BasicInfo.ListRoles.Contains(CE_RoleEnum.营销主管.ToString()) || BasicInfo.ListRoles.Contains(CE_RoleEnum.营销分管领导.ToString()))
               && !BasicInfo.ListRoles.Contains(CE_RoleEnum.业务系统管理员.ToString()) && !BasicInfo.ListRoles.Contains(CE_RoleEnum.营销售后客户回访人员.ToString()))
            {
                sql += " and (接单处理人='" + BasicInfo.LoginName + "')";
            }
            else if (BasicInfo.ListRoles.Contains(CE_RoleEnum.营销售后客户回访人员.ToString()))
            {
                sql += " and (单据状态='等待回访') or (接函电人='" + BasicInfo.LoginName + "' or 接单处理人='" + BasicInfo.LoginName +
                       "' or 函电录入人='" + BasicInfo.LoginName + "')" + " or (接函电人='江金兰')";
            }

            sql += " order by 单据号 desc";

            DataTable dt = GlobalObject.DatabaseServer.WebQueryInfo(sql);

            return dt;
        }

       
        /// <summary>
        /// 检查单据的编号
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>成功返回单据编号，失败返回null</returns>
        public DataRow IsExist(string billNo)
        {
            string sql = "SELECT ID FROM [DepotManagement].[dbo].[S_MarketingBill] where DJH = '" + billNo + "'";

            System.Data.DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt.Rows[0];

        }

        /// <summary>
        /// 通过单据号删除函电信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool DeleteByBillNo(string billNo,out string error)
        {

            error = "";

            try
            {
                WebSeverDataContext dataContxt = WebDatabaseParameter.WebDataContext;

                var varData = from a in dataContxt.S_AfterService
                              where a.ServiceID == billNo
                              select a;

                if (varData.Count() != 1)
                {
                    throw new Exception( "单据信息有误！");
                }
                else
                {
                    dataContxt.S_AfterService.DeleteAllOnSubmit(varData);

                    var varBug = from a in dataContxt.OF_BugMessageInfo
                                  where a.ServiceID == billNo
                                  select a;

                    dataContxt.OF_BugMessageInfo.DeleteAllOnSubmit(varBug);
                    dataContxt.SubmitChanges();

                    return true;
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获取函电信息
        /// </summary>
        /// <param name="returnInfo">函电信息</param>
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
                qr = serverAuthorization.Query("售后函电处理单", null);
            }
            else
            {
                qr = serverAuthorization.Query("售后函电处理单", null, QueryResultFilter);
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
        /// 获取质量反馈查询
        /// </summary>
        /// <param name="returnInfo">质量反馈信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool GetAllBillFeedBack(out IQueryResult returnInfo, out string error)
        {
            returnInfo = null;
            error = null;

            IAuthorization serverAuthorization = PlatformFactory.GetObject<IAuthorization>();
            IQueryResult qr = null;

            if (QueryResultFilter == null)
            {
                qr = serverAuthorization.Query("售后质量反馈查询", null);
            }
            else
            {
                qr = serverAuthorization.Query("售后质量反馈查询", null, QueryResultFilter);
            }

            if (!qr.Succeeded)
            {
                error = qr.Error;
                return false;
            }

            returnInfo = qr;
            return true;
        }
    }
}
