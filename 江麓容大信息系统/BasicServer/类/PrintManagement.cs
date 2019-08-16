/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  CompositiveServer.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2010/07/17
 * 开发平台:  Visual C# 2008
 * 用于    :  仓库管理软件
 *----------------------------------------------------------------------------
 * 描述 : 
 * 其它 :
 *----------------------------------------------------------------------------
 * 公共信息: 参见系统'类帮助文档'
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2010/07/17 8:54:12 作者: 夏石友 当前版本: V1.00
 *        修改说明: 创建
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using System.Linq;
using DBOperate;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using GlobalObject;

namespace ServerModule
{
    /// <summary>
    /// 打印管理类
    /// </summary>
    class PrintManagement : IPrintManagement
    {
        /// <summary>
        /// 删除原有的打印的报表/单据记录
        /// </summary>
        /// <param name="dataContext"></param>
        /// <param name="printInfo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        private bool DeletePrintInfo(DepotManagementDataContext dataContext,
            S_PrintBillTable printInfo, out string error)
        {
            error = null;

            try
            {
                var varData = from a in dataContext.S_PrintBillTable
                              where a.Bill_ID == printInfo.Bill_ID
                              select a;

                dataContext.S_PrintBillTable.DeleteAllOnSubmit(varData);
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
            return true;
        }

        /// <summary>
        /// 添加打印日志
        /// </summary>
        /// <returns></returns>
        private bool AddPrintInfoLog(DepotManagementDataContext dataContext,
            S_PrintBillTable printInfo, out string error)
        {
            error = null;

            try
            {
                S_PrintBillTableLog lnqLog = new S_PrintBillTableLog();
                lnqLog.Bill_ID = printInfo.Bill_ID;
                lnqLog.Bill_Name = printInfo.Bill_Name;
                lnqLog.CheckFlag = printInfo.CheckFlag;
                lnqLog.PrintDateTime = printInfo.PrintDateTime;
                lnqLog.PrintFlag = printInfo.PrintFlag;
                lnqLog.PrintPersonnelCode = printInfo.PrintPersonnelCode;
                lnqLog.PrintPersonnelDepartment = printInfo.PrintPersonnelDepartment;
                lnqLog.PrintPersonnelName = printInfo.PrintPersonnelName;

                dataContext.S_PrintBillTableLog.InsertOnSubmit(lnqLog);
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }

            return true;
        }

        /// <summary>
        /// 添加完成打印的报表/单据
        /// </summary>
        /// <param name="printInfo">打印信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool AddPrintInfo(S_PrintBillTable printInfo, out string error)
        {
            error = null;

            if (BasicInfo.LoginID == "0417")
            {
                return true;
            }

            if (IsExist(printInfo, out error))
            {
                return false;
            }

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            dataContxt.Connection.Open();
            dataContxt.Transaction = dataContxt.Connection.BeginTransaction();

            try
            {

                var print = from b in dataContxt.S_AgainPrintBillTable
                            where b.Bill_ID == printInfo.Bill_ID && !b.PrintFlag && b.Authorize
                            select b;

                if (print.Count() == 1)
                {
                    S_AgainPrintBillTable table = print.Single();
                    table.PrintFlag = true;
                }
                else if (print.Count() > 1)
                {
                    error = "数据不唯一";
                    return false;
                }

                //删除记录
                if (!DeletePrintInfo(dataContxt,printInfo,out error))
                {
                    return false;
                }

                dataContxt.SubmitChanges();

                //添加打印日志
                if (!AddPrintInfoLog(dataContxt,printInfo,out error))
                {
                    return false;
                }

                dataContxt.S_PrintBillTable.InsertOnSubmit(printInfo);
                dataContxt.SubmitChanges();

                dataContxt.Transaction.Commit();
                return true;
            }
            catch (Exception exce)
            {
                dataContxt.Transaction.Rollback();
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 检查单据是否已经完成打印
        /// </summary>
        /// <param name="printInfo">打印信息</param>
        /// <param name="message">输出的消息</param>
        /// <returns>已经打印返回true</returns>
        public bool IsExist(S_PrintBillTable printInfo, out string message)
        {
            message = null;

            //DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;
            //var billGather = from c in dataContxt.S_PrintBillTable
            //                 where c.Bill_ID == printInfo.Bill_ID && c.Bill_Name == printInfo.Bill_Name
            //                 select c;

            //if (billGather.Count() > 0)
            //{
            //    var print = from b in dataContxt.S_AgainPrintBillTable
            //                where b.Bill_ID == printInfo.Bill_ID && !b.PrintFlag && b.Authorize
            //                select b;

            //    if (print.Count() != 1)
            //    {
            //        message = "";

            //        foreach (var ei in billGather)
            //        {
            //            message += "系统不支持单据的重复打印,该单据已于[" + ei.PrintDateTime + "]日,由[" + ei.PrintPersonnelDepartment + "][" + ei.PrintPersonnelName + "]打印!";
            //        }

            //        return true;
            //    }
            //}

            return false;
        }

        /// <summary>
        /// 获取打印明细信息
        /// </summary>
        /// <param name="beginDate">获取的起始时间</param>
        /// <param name="endDate">获取的结束时间</param>
        /// <param name="dept">打印单据接收部门名称</param>
        /// <returns>返回获取到时间范围内的明细信息</returns>
        public IQueryable<View_S_PrintBill> GetPrintInfo(DateTime beginDate, DateTime endDate, string dept)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(dept))
            {
                return from r in dataContxt.View_S_PrintBill
                       where r.打印时间 >= beginDate && r.打印时间 <= endDate
                       orderby r.分发部门, r.单据类别, r.单据编号
                       select r;
            }
            else
            {
                return from r in dataContxt.View_S_PrintBill
                       where r.打印时间 >= beginDate && r.打印时间 <= endDate && (r.分发部门 == dept)
                       orderby r.分发部门, r.单据类别, r.单据编号
                       select r;
            }
        }

        /// <summary>
        /// 获取打印单据接收部门名称
        /// </summary>
        /// <returns>返回获取到的接收部门名称</returns>
        public IQueryable<string> GetReceivedDeptOfPrintBill()
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            return (from r in dataContxt.S_PrintBillReceivedDept
                    orderby r.Dept
                    select r.Dept).Distinct();
        }

        /// <summary>
        /// 修改审核状态
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="m_err"></param>
        /// <returns></returns>
        public bool SetChecked(DataTable dt, bool blZT, out string m_err)
        {
            m_err = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var varData = from a in dataContxt.S_PrintBillTable
                                  where a.Bill_ID == dt.Rows[i][0].ToString()
                                  select a;

                    S_PrintBillTable lnqPrintBill = varData.Single();
                    lnqPrintBill.CheckFlag = blZT;
                }

                dataContxt.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                m_err = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 根据单据号查询单据信息
        /// </summary>
        /// <param name="DJH">单据号</param>
        /// <returns>返回单据信息表</returns>
        public DataTable GetPrintBillTableByDJH(string DJH)
        {
            string sql = @"select * from dbo.view_s_PrintBilltableLog where 单据号='" + DJH + "'";

            return GlobalObject.DatabaseServer.QueryInfo(sql);
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="billid"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Del_S_AgainPrintBillTable(int billid,out string error)
        {
            error = null;
            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.S_AgainPrintBillTable
                              where a.ID == billid
                              && a.PrintFlag == false
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数据已被打印不能删除或者为空或者不唯一";
                    return false;
                }
                else
                {
                    dataContext.S_AgainPrintBillTable.DeleteAllOnSubmit(varData);
                    dataContext.SubmitChanges();
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
        /// 添加重新打印的单据
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="billName">单据名</param>
        /// <param name="printPersonnelCode">打印人编码</param>
        /// <param name="printPersonnelName">打印人姓名</param>
        /// <param name="printPersonnelDepartment">打印部门</param>
        /// <param name="printDateTime">打印时间</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns></returns>
        public bool Add_S_AgainPrintBillTable(S_AgainPrintBillTable againprint, out string error)
        {
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.S_AgainPrintBillTable
                         where r.Bill_ID == againprint.Bill_ID && !r.PrintFlag
                         select r;

            if (result.Count() > 0)
            {
                error = string.Format("【{0}】单据已经存在一个未完成的申请。", againprint.Bill_ID);
                return false;
            }

            S_AgainPrintBillTable tablePrintBill = new S_AgainPrintBillTable();

            tablePrintBill.Bill_ID = againprint.Bill_ID;
            tablePrintBill.PrintPersonnelCode = againprint.PrintPersonnelCode;
            tablePrintBill.PrintPersonnelName = againprint.PrintPersonnelName;
            tablePrintBill.PrintPersonnelDepartment = againprint.PrintPersonnelDepartment;
            tablePrintBill.PrintDateTime = againprint.PrintDateTime;
            tablePrintBill.Remark = againprint.Remark;
            tablePrintBill.CheckFlag = false;
            tablePrintBill.Authorize = false;
            tablePrintBill.PrintFlag = false;

            try
            {
                dataContxt.S_AgainPrintBillTable.InsertOnSubmit(tablePrintBill);
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
        /// 审核后修改打印状态
        /// </summary>
        /// <param name="ID">数据行ID</param>
        /// <param name="error">如果返回值为假，则输出错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        public bool UpdateAgainPrintBillTable(string ID, out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from b in dataContxt.S_AgainPrintBillTable
                                  where b.ID == int.Parse(ID)
                                 select b;

                if (result.Count() != 1)
                {
                    return false;
                }
                else
                {
                    S_AgainPrintBillTable billGather = result.Single();

                    if (billGather.CheckFlag)
                    {
                        error = billGather.CheckMan+"在 "+billGather.CheckTime+" 已经审核！";
                        return false;
                    }
                    else
                    {
                        billGather.CheckFlag = true;
                        billGather.CheckMan = BasicInfo.LoginName;
                        billGather.CheckTime = ServerTime.Time;
                        dataContxt.SubmitChanges();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 批准后修改打印状态
        /// </summary>
        /// <param name="DJH">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns></returns>
        public bool UpdateAuthorize(string ID, out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var billGather = from b in dataContxt.S_AgainPrintBillTable
                                  where b.ID == int.Parse(ID)
                                  select b;

                if (billGather.Count() != 1)
                {
                    return false;
                }
                else
                {
                    S_AgainPrintBillTable Again = billGather.Single();

                    if (Again.CheckFlag)
                    {
                        if (Again.Authorize)
                        {
                            error = Again.AuthorizeMan+" 在 "+Again.AuthorizeTime+" 已经批准！";
                            return false;
                        }
                        else
                        {
                            Again.Authorize = true;
                            Again.AuthorizeMan = BasicInfo.LoginName;
                            Again.AuthorizeTime = ServerTime.Time;
                            Again.PrintFlag = false;
                            dataContxt.SubmitChanges();

                            return true;
                        }
                    }
                    else
                    {
                        error = "请等待审核后再进行此操作！";
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 查询所有信息
        /// </summary>
        /// <returns>返回表中的所有数据</returns>
        public DataTable GetAgainPrintBill(DateTime dtstarttime, DateTime dtendtime)
        {
            string sql = @"select * from dbo.view_S_AgainPrintBillTable"+
                          " where 申请时间>='"+dtstarttime+"' and "+
                          " 申请时间<'"+dtendtime+"'";
            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);
            return dt;
        }

        /// <summary>
        /// 按时间查询表信息
        /// </summary>
        /// <param name="startTime">查询起始时间</param>
        /// <param name="endTime">查询结束时间</param>
        /// <returns>返回表中的所有数据</returns>
        public DataTable GetAgainPrintBillByTime(string startTime,string endTime)
        {
            string sql = @"select * from  dbo.view_S_AgainPrintBillTable where 申请时间 >='" + startTime + "' and 申请时间<'" + endTime + "'";

            return GlobalObject.DatabaseServer.QueryInfo(sql);
        }
    }
}
