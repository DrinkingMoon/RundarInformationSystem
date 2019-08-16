using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ServerModule;
using GlobalObject;

namespace Service_Peripheral_HR
{
    /// <summary>
    /// 集体考勤异常信息操作类
    /// </summary>
    class BatchExceptionServer : Service_Peripheral_HR.IBatchExceptionServer
    {
        /// <summary>
        /// 获取所有集体考勤异常
        /// </summary>
        /// <param name="date">异常日期</param>
        /// <returns>返回数据集</returns>
        public DataTable GetAllInfo(string date)
        {
            string sql = "select * from View_HR_BatchException";

            if (date != null)
            {
                sql += " where 异常日期 = '"+date+"'";
            }

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);
            return dt;
        }

        /// <summary>
        /// 判断某一天中是否存在集体考勤异常
        /// </summary>
        /// <param name="date">异常日期</param>
        /// <returns>存在返回true，不存在返回False</returns>
        public bool GetBatchByDate(string date)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from a in dataContxt.HR_BatchException
                         where a.BeginTime <= Convert.ToDateTime(date) && a.EndTime >= Convert.ToDateTime(date)
                         select a;

            if (result.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 新增集体考勤异常信息
        /// </summary>
        /// <param name="batchException">集体考勤异常信息</param>
        /// <param name="lstPersonnel">人员信息集合</param>
        /// <returns>成功返回True失败返回False</returns>
        public void AddBatchException(HR_BatchException batchException, List<PersonnelBasicInfo> lstPersonnel)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;
            dataContxt.Connection.Open();
            dataContxt.Transaction = dataContxt.Connection.BeginTransaction();

            try
            {
                var result = from a in dataContxt.HR_BatchException
                             where a.Date == batchException.Date && a.Description == batchException.Description
                             select a;

                if (result.Count() == 1)
                {
                    throw new Exception("该异常已经存在，请确认异常信息");
                }

                dataContxt.HR_BatchException.InsertOnSubmit(batchException);
                dataContxt.SubmitChanges();

                int billID = (from a in dataContxt.HR_BatchException
                              select a.ID).Max();

                foreach (PersonnelBasicInfo personnel in lstPersonnel)
                {
                    HR_BatchException_Personnel detail = new HR_BatchException_Personnel();

                    detail.BillID = billID;
                    detail.WorkID = personnel.工号;

                    //new AttendanceAnalysis().DataTimeIsRepeat<HR_BatchException>(dataContxt, batchException, detail.WorkID);

                    dataContxt.HR_BatchException_Personnel.InsertOnSubmit(detail);
                }

                dataContxt.SubmitChanges();
                dataContxt.Transaction.Commit();
            }
            catch (Exception ex)
            {
                dataContxt.Transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 删除集体考勤异常信息
        /// </summary>
        /// <param name="billID">单据号</param>
        public void DeleteBatchException(int billID)
        {
            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var varData = from a in dataContxt.HR_BatchException
                              where a.ID == billID
                              select a;

                if (varData.Count() == 0)
                {
                    return;
                }

                HR_BatchException tempLnq = varData.Single();

                if (tempLnq.HR_Director != null && tempLnq.HR_Director.Trim().Length > 0)
                {
                    throw new Exception("已审核的单据无法删除");
                }
                else
                {
                    dataContxt.HR_BatchException.DeleteAllOnSubmit(varData);
                }

                dataContxt.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 主管审核修改集体考勤异常信息
        /// </summary>
        /// <param name="batchException">集体考勤异常信息</param>
        public void AuditingBatchException(HR_BatchException batchException)
        {
            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_BatchException
                             where a.ID == batchException.ID
                             select a;

                if (result.Count() != 1)
                {
                    throw new Exception("该异常信息有误，请确认异常信息");
                }

                HR_BatchException batchExceptionLinq = result.Single();

                batchExceptionLinq.HR_Director = batchException.HR_Director;
                batchExceptionLinq.HR_SignatureDate = batchException.HR_SignatureDate;

                dataContxt.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable GetDetailPersonnel(int billID)
        {
            string strSql = " select b.员工编号, b.员工姓名, b.部门 from HR_BatchException_Personnel as a "+
                            " inner join View_HR_PersonnelArchive as b on a.WorkID = b.员工编号 "+
                            " where a.BillID = "+ billID + " order by b.部门, b.员工编号";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }
    }
}
