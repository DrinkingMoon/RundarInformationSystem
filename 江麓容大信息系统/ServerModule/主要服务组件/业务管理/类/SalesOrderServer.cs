using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GlobalObject;

namespace ServerModule
{
    /// <summary>
    /// 销售合同/订单评审
    /// </summary>
    class SalesOrderServer : ServerModule.ISalesOrderServer
    {
        /// <summary>
        /// 获得单据列表
        /// </summary>
        /// <param name="billStatus">单据状态</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>返回Table</returns>
        public DataTable GetAllBillInfo(string billStatus, DateTime startTime, DateTime endTime)
        {
            string strSql = " select * from View_YX_SalesOrder  where 制单时间 >= '" + startTime + "' and 制单时间 <= '" + endTime + "' ";

            if (billStatus != "全部")
            {
                strSql += " and 单据状态 = '" + billStatus + "'";
            }

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得单条信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回LINQ数据集</returns>
        public View_YX_SalesOrder GetBillInfo(string billNo)
        {
            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            var varData = from a in dataContext.View_YX_SalesOrder
                          where a.单据号 == billNo
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
        /// 获得零件明细信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回Table</returns>
        public DataTable GetPartListInfo(string billNo)
        {
            string strSql = "select * from View_YX_SalesOrderPartList where 单据号 = '" + billNo + "'";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 通过单据号，部门编码获得评审意见历史信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="deptCode">部门编码</param>
        /// <returns>返回Table</returns>
        public DataTable GetReviewHistory(string billNo, string deptCode)
        {
            string strSql = "select BillNo 单据号,DeptName 部门名称,Opinion 评审意见,Confirmation 评审人,"+
                            " ConfirmDate 评审时间, a.DeptCode 部门编码 from YX_SalesOrderReviewHistory  a"+ 
                            " join HR_Dept b on a.DeptCode=b.DeptCode where BillNo = '" + billNo + "' "+
                            " and a.DeptCode='" + deptCode + "'";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得评审信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回LINQ数据集</returns>
        public IEnumerable<View_YX_SalesOrderReview> GetReviewListInfo(string billNo)
        {
            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            return (from a in dataContext.View_YX_SalesOrderReview
                    where a.单据号 == billNo
                    select a);
        }

        /// <summary>
        /// 获取新的单据号
        /// </summary>
        /// <returns>成功返回单号，失败抛出异常</returns>
        public string GetNextBillID()
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            long maxValue = 1;
            string prefix = "";

            DateTime dt = ServerTime.Time;

            if (dataContxt.YX_SalesOrder.Count() > 0)
            {
                var result = from c in dataContxt.YX_SalesOrder
                             where c.BillNo.Substring(0, 4) == dt.Year.ToString()
                             select c;

                if (result.Count() > 0)
                {
                    maxValue += (from c in result select Convert.ToInt32(c.BillNo.Substring(8))).Max();
                }
            }

            return string.Format("{0}{1:D4}{2:D2}{3:D4}", prefix, dt.Year, dt.Month, maxValue);
        }

        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool DeleteBill(string billNo, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.YX_SalesOrder
                              where a.BillNo == billNo
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数据为空或者不唯一";
                    return false;
                }
                else
                {
                    var varPartList = from a in dataContext.YX_SalesOrderPartList
                                      where a.BillNo == varData.Single().BillNo
                                      select a;

                    var varReviewList = from a in dataContext.YX_SalesOrderReview
                                        where a.BillNo == varData.Single().BillNo
                                        select a;

                    dataContext.YX_SalesOrderReview.DeleteAllOnSubmit(varReviewList);
                    dataContext.YX_SalesOrderPartList.DeleteAllOnSubmit(varPartList);
                    dataContext.YX_SalesOrder.DeleteAllOnSubmit(varData);

                    dataContext.SubmitChanges();
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
        /// 删除零件明细
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="billNo">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        private bool DeletePartList(DepotManagementDataContext dataContext, string billNo, out string error)
        {
            error = null;

            try
            {
                var varData = from a in dataContext.YX_SalesOrderPartList
                              where a.BillNo == billNo
                              select a;

                dataContext.YX_SalesOrderPartList.DeleteAllOnSubmit(varData);

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 删除评审信息
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="billNo">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        private bool DeleteReviewList(DepotManagementDataContext dataContext, string billNo, out string error)
        {
            error = null;

            try
            {
                var varData = from a in dataContext.YX_SalesOrderReview
                              where a.BillNo == billNo
                              select a;

                dataContext.YX_SalesOrderReview.DeleteAllOnSubmit(varData);

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 插入明细
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="billNo">单据号</param>
        /// <param name="listInfo">零件明细信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        private bool InsertPartList(DepotManagementDataContext dataContext, string billNo, DataTable listInfo, out string error)
        {
            error = null;

            try
            {
                for (int i = 0; i < listInfo.Rows.Count; i++)
                {
                    YX_SalesOrderPartList lnqList = new YX_SalesOrderPartList();

                    lnqList.BillNo = billNo;
                    lnqList.GoodsCode = listInfo.Rows[i]["图号型号"].ToString();
                    lnqList.GoodsName = listInfo.Rows[i]["物品名称"].ToString();
                    lnqList.Spec = listInfo.Rows[i]["规格"].ToString();
                    lnqList.ClientGoodsName = listInfo.Rows[i]["客户零件名称"].ToString();
                    lnqList.ClientGoodsCode = listInfo.Rows[i]["客户零件代码"].ToString();
                    lnqList.Number = Convert.ToInt32(listInfo.Rows[i]["要货数量"]);
                    lnqList.GoodsID = Convert.ToInt32(listInfo.Rows[i]["物品ID"]);

                    dataContext.YX_SalesOrderPartList.InsertOnSubmit(lnqList);
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
        /// 插入明细
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="billNo">单据号</param>
        /// <param name="listInfo">零件明细信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        private bool InsertReviewList(DepotManagementDataContext dataContext, string billNo, DataTable listInfo, out string error)
        {
            error = null;

            try
            {
                for (int i = 0; i < listInfo.Rows.Count; i++)
                {
                    YX_SalesOrderReview lnqList = new YX_SalesOrderReview();

                    lnqList.BillNo = billNo;
                    lnqList.DeptCode = listInfo.Rows[i]["DeptCode"].ToString();

                    dataContext.YX_SalesOrderReview.InsertOnSubmit(lnqList);
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
        /// 提交单据
        /// </summary>
        /// <param name="salesOrder">单据信息数据集</param>
        /// <param name="listPartInfo">零件明细信息</param>
        /// <param name="listReviewInfo">评审信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool InsertBill(YX_SalesOrder salesOrder, DataTable listPartInfo, DataTable listReviewInfo, out string error)
        {
            error = null;
            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            try
            {
                dataContext.Connection.Open();
                dataContext.Transaction = dataContext.Connection.BeginTransaction();

                var varData = from a in dataContext.YX_SalesOrder
                              where a.BillNo == salesOrder.BillNo
                              select a;

                if (varData.Count() == 1)
                {
                    if (!DeletePartList(dataContext, salesOrder.BillNo, out error)
                        && !DeleteReviewList(dataContext, salesOrder.BillNo, out error))
                    {
                        return false;
                    }

                    if (!DeleteReviewList(dataContext, salesOrder.BillNo, out error))
                    {
                        return false;
                    }

                    dataContext.YX_SalesOrder.DeleteAllOnSubmit(varData);
                    dataContext.SubmitChanges();
                }

                dataContext.YX_SalesOrder.InsertOnSubmit(salesOrder);
                dataContext.SubmitChanges();

                if (!InsertPartList(dataContext, salesOrder.BillNo, listPartInfo, out error))
                {
                    throw new Exception(error);
                }

                if (!InsertReviewList(dataContext, salesOrder.BillNo, listReviewInfo, out error))
                {
                    throw new Exception(error);
                }

                dataContext.SubmitChanges();

                dataContext.Transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                dataContext.Transaction.Rollback();
                return false;
            }
        }

        /// <summary>
        /// 操作业务
        /// </summary>
        /// <param name="salesOrder">单据信息数据集</param>
        /// <param name="listPartInfo">零件明细信息</param>
        /// <param name="listReviewInfo">评审部门信息</param>
        /// <param name="deptCode">部门编码</param>
        /// <param name="opinion">部门评审意见</param>
        /// <param name="status">单据状态</param>
        /// <param name="type">操作类型（主管审核、部门评审、评审结果）</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool OperationInfo(YX_SalesOrder salesOrder, DataTable listPartInfo, DataTable listReviewInfo,
                                  string deptCode, string opinion, string status, string type, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.YX_SalesOrder
                              where a.BillNo == salesOrder.BillNo
                              select a;

                var resultReview = from a in dataContext.YX_SalesOrderReview
                                   where a.BillNo == salesOrder.BillNo && a.DeptCode == deptCode
                                   select a;

                if (varData.Count() == 1)
                {
                    YX_SalesOrder lnqBill = varData.Single();

                    switch (type)
                    {
                        case "主管审核":
                            lnqBill.Status = status;

                            if (listReviewInfo != null && listReviewInfo.Rows.Count > 0)
                            {
                                if (resultReview.Count() > 0)
                                {
                                    foreach (YX_SalesOrderReview item in resultReview)
                                    {
                                        YX_SalesOrderReviewHistory history = new YX_SalesOrderReviewHistory();

                                        history.BillNo = salesOrder.BillNo;
                                        history.Confirmation = UniversalFunction.GetPersonnelCode(item.Confirmation);
                                        history.ConfirmDate = Convert.ToDateTime(item.ConfirmDate);
                                        history.DeptCode = item.DeptCode;
                                        history.Opinion = item.Opinion;

                                        dataContext.YX_SalesOrderReviewHistory.InsertOnSubmit(history);
                                    }
                                }

                                if (!DeleteReviewList(dataContext, salesOrder.BillNo, out error))
                                {
                                    return false;
                                }

                                if (!InsertReviewList(dataContext, salesOrder.BillNo, listReviewInfo, out error))
                                {
                                    return false;
                                }

                                if (!DeletePartList(dataContext, salesOrder.BillNo, out error)
                                    && !DeleteReviewList(dataContext, salesOrder.BillNo, out error))
                                {
                                    return false;
                                }

                                if (!InsertPartList(dataContext, salesOrder.BillNo, listPartInfo, out error))
                                {
                                    throw new Exception(error);
                                }
                            }
                            else
                            {
                                lnqBill.Auditer = salesOrder.Auditer;
                                lnqBill.AuditDate = salesOrder.AuditDate;
                            }

                            break;

                        case "部门评审":

                            if (resultReview.Count() == 1)
                            {
                                YX_SalesOrderReview review = resultReview.Single();

                                review.Opinion = opinion;
                                review.Confirmation = BasicInfo.LoginID;
                                review.ConfirmDate = ServerTime.Time;

                                lnqBill.Status = status;
                            }
                            else
                            {
                                error = "请确认评审部门！";
                                return false;
                            }
                            break;

                        case "确认评审":

                                lnqBill.Status = status;

                            break;

                        case "评审结果":
                            lnqBill.ResultPerson = salesOrder.ResultPerson;
                            lnqBill.ReviewResult = salesOrder.ReviewResult;
                            lnqBill.Status = status;
                            lnqBill.ResultDate = salesOrder.ResultDate;

                            if (!DeletePartList(dataContext, salesOrder.BillNo, out error))
                            {
                                return false;
                            }

                            if (!InsertPartList(dataContext, salesOrder.BillNo, listPartInfo, out error))
                            {
                                return false;
                            }

                            break;
                        default:
                            break;
                    }

                    dataContext.SubmitChanges();
                }
                else
                {
                    error = "数据不唯一";
                    return false;
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
        /// 通过物品ID和主机厂编码获得与主机厂相对应的图号名称
        /// </summary>
        /// <param name="clientCode">主机厂编码</param>
        /// <param name="goodsID">物品ID</param>
        /// <returns></returns>
        public DataTable GetCommunicate(string clientCode,int goodsID)
        {
            string sql = "select * from YX_GoodsSystemMatchingCommunicate where Communicate='" + clientCode + "'and GoodsID=" + goodsID;

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }
    }
}
