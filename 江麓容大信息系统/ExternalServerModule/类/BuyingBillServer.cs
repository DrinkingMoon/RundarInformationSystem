
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ServerModule;
using GlobalObject;

namespace Service_Peripheral_External
{
    /// <summary>
    /// 挂账单服务类
    /// </summary>
    class BuyingBillServer : Service_Peripheral_External.IBuyingBillServer
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
            string strSql = " select * from View_Out_BuyingBill  where 记录时间 >= '" + startTime + "' and 记录时间 <= '" + endTime + "' ";

            if (billStatus != "全部")
            {
                strSql += " and 单据状态 = '" + billStatus + "'";
            }

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得单条信息
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <returns>返回LINQ数据集</returns>
        public View_Out_BuyingBill GetBillInfo(int billID)
        {
            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            var varData = from a in dataContext.View_Out_BuyingBill
                          where a.单号 == billID
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
        /// 获得明细信息
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回LINQ数据集</returns>
        public IQueryable<Out_BuyingList> GetListInfo(int goodsID, int billID)
        {
            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            var varData = from a in dataContext.Out_BuyingList
                          where a.BillID == billID && a.GoodsID == goodsID
                          select a;

            if (varData.Count() == 0)
            {
                return null;
            }
            else
            {
                return varData;
            }
        }

        /// <summary>
        /// 获得明细信息
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <returns>返回Table</returns>
        public DataTable GetListInfo(int billID)
        {
            string strSql = "select * from View_Out_BuyingList where 单号 = " + billID;

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 提交申请
        /// </summary>
        /// <param name="buyingBill">单据信息数据集</param>
        /// <param name="listInfo">明细信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool InsertBill(Out_BuyingBill buyingBill, DataTable listInfo, out string error)
        {
            error = null;
            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            try
            {
                dataContext.Connection.Open();
                dataContext.Transaction = dataContext.Connection.BeginTransaction();

                var varData = from a in dataContext.Out_BuyingBill
                              where a.ID == buyingBill.ID
                              select a;

                if (varData.Count() == 0)
                {
                    dataContext.Out_BuyingBill.InsertOnSubmit(buyingBill);
                    dataContext.SubmitChanges();

                    if (!InsertList(dataContext, buyingBill.ID, listInfo, out error))
                    {
                        throw new Exception(error);
                    }
                }
                else if (varData.Count() == 1)
                {
                    Out_BuyingBill lnqBill = varData.Single();

                    lnqBill.Statua = "等待审核";
                    lnqBill.Recorder = BasicInfo.LoginID;
                    lnqBill.RecordTime = ServerTime.Time;
                    lnqBill.Remark = buyingBill.Remark;
                    lnqBill.ClientCode = buyingBill.ClientCode;

                    if (!DeleteList(dataContext, buyingBill.ID, out error))
                    {
                        return false;
                    }

                    if (!InsertList(dataContext, buyingBill.ID, listInfo, out error))
                    {
                        return false;
                    }
                }
                else
                {
                    error = "数据重复";
                    return false;
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
        /// 删除单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool DeleteBill(int billNo, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.Out_BuyingBill
                              where a.ID == billNo
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数据为空或者不唯一";
                    return false;
                }
                else
                {
                    var varList = from a in dataContext.Out_BuyingList
                                  where a.BillID == varData.Single().ID
                                  select a;

                    dataContext.Out_BuyingBill.DeleteAllOnSubmit(varData);
                    dataContext.Out_BuyingList.DeleteAllOnSubmit(varList);

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
        /// 删除明细
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="billNo">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        private bool DeleteList(DepotManagementDataContext dataContext, int billNo, out string error)
        {
            error = null;

            try
            {
                var varData = from a in dataContext.Out_BuyingList
                              where a.BillID == billNo
                              select a;

                dataContext.Out_BuyingList.DeleteAllOnSubmit(varData);

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
        /// <param name="listInfo">明细信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        private bool InsertList(DepotManagementDataContext dataContext, int billNo, DataTable listInfo, out string error)
        {
            error = null;

            try
            {
                for (int i = 0; i < listInfo.Rows.Count; i++)
                {
                    Out_BuyingList lnqList = new Out_BuyingList();

                    lnqList.BillID = billNo;
                    lnqList.GoodsID = Convert.ToInt32(listInfo.Rows[i]["GoodsID"]);
                    lnqList.Count = Convert.ToInt32(listInfo.Rows[i]["挂账数量"]);
                    lnqList.Remark = listInfo.Rows[i]["备注"].ToString();

                    dataContext.Out_BuyingList.InsertOnSubmit(lnqList);
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
        /// 操作业务
        /// </summary>
        /// <param name="buyingBill">单据信息数据集</param>
        /// <param name="listInfo">单据明细信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool OperationInfo(Out_BuyingBill buyingBill, DataTable listInfo, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.Out_BuyingBill
                              where a.ID == buyingBill.ID
                              select a;

                if (varData.Count() == 1)
                {
                    Out_BuyingBill lnqBill = varData.Single();

                    lnqBill.Statua = "已完成";
                    lnqBill.Director = BasicInfo.LoginID;
                    lnqBill.DirectTime = ServerTime.Time;

                    for (int i = 0; i < listInfo.Rows.Count; i++)
                    {
                        Out_DetailAccount lnqDetail = new Out_DetailAccount();

                        lnqDetail.Bill_ID = buyingBill.ID.ToString();
                        lnqDetail.BillFinishTime = ServerTime.Time;
                        lnqDetail.Confirmor = BasicInfo.LoginID;
                        lnqDetail.GoodsID = Convert.ToInt32(listInfo.Rows[i]["GoodsID"]);
                        lnqDetail.OperationCount = -Convert.ToDecimal(listInfo.Rows[i]["挂账数量"]);
                        lnqDetail.BatchNo = listInfo.Rows[i]["规格"].ToString();
                        lnqDetail.Proposer = buyingBill.Recorder;
                        lnqDetail.Remark = "【挂账单】" + listInfo.Rows[i]["备注"].ToString();
                        lnqDetail.SecStorageID = buyingBill.ClientCode;
                        lnqDetail.StorageID = GetStockInfo(buyingBill.ClientCode,Convert.ToInt32(listInfo.Rows[i]["GoodsID"]));

                        if (!new BusinessOperation().OperationDetailAndStock(dataContext, lnqDetail, out error))
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    error = "数据不唯一";
                    return false;
                }

                dataContext.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 获得账务库房ID
        /// </summary>
        /// <param name="SecStorageID">库房编号</param>
        /// <param name="goodsID">物品ID</param>
        /// <returns>成功返回满足条件的库房ID，失败返回NULL</returns>
        public string GetStockInfo(string SecStorageID, int goodsID)
        {
            string strSql = "select StorageID from  Out_Stock where  SecStorageID='" + SecStorageID + "' and GoodsID=" + goodsID;


            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dt != null && dt.Rows.Count > 0)
            {
                return dt.Rows[0]["StorageID"].ToString();
            }

            return null;
        }
    }
}
