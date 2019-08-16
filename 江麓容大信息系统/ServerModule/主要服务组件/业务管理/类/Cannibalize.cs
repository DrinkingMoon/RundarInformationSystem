/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  Cannibalize.cs
 * 作者    :  曹津彬    版本: v1.00    日期: 2014/01/22
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

namespace ServerModule
{
    /// <summary>
    /// 库房调拨管理类
    /// </summary>
    class Cannibalize : BasicServer, ServerModule.ICannibalize
    {
        /// <summary>
        /// 库存信息服务组件
        /// </summary>
        IStoreServer m_serverStore = ServerModuleFactory.GetServerModule<IStoreServer>();

        /// <summary>
        /// 基础物品服务组件
        /// </summary>
        IBasicGoodsServer m_serverBasincGoods = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();

        /// <summary>
        /// 检查某业务是否存在
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">业务号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.S_CannibalizeBill
                          where a.DJH == billNo
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
            string sql = "SELECT * FROM [DepotManagement].[dbo].[S_CannibalizeBill] where DJH = '" + billNo + "'";

            System.Data.DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 获取全部信息
        /// </summary>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="djzt">单据状态("全  部"或者其他状态)</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回查询得到的信息</returns>
        public DataTable GetAllData(string startDate, string endDate, string djzt, out string error)
        {
            error = null;

            DataTable dt = new DataTable();

            try
            {
                string strSql = " SELECT ID, DJH, dbo.fun_get_StorageName(InStoreRoom) as InStoreRoom, " +
                                " dbo.fun_get_StorageName(OutStoreRoom) as OutStoreRoom, " +
                                " DJZT, Price," +
                                " dbo.fun_get_Name(LRRY) as LRRY, LRRQ, " +
                                " dbo.fun_get_Name(Checker) as Checker, CheckTime, " +
                                " dbo.fun_get_Name(SHRY) as SHRY, SHRQ, " +
                                " dbo.fun_get_Name(CWRY) as CWRY, CWRQ, " +
                                " dbo.fun_get_Name(KFRY) as KFRY, KFRQ, " +
                                " Remark FROM S_CannibalizeBill where 1=1 ";

                if (djzt != "全  部")
                {
                    strSql = strSql + " and DJZT = '" + djzt + "'  ";
                }

                if (GlobalObject.BasicInfo.IsFuzzyContainsRoleName("库管理员"))
                {
                    strSql = strSql + " and (InStoreRoom in (select StorageID from BASE_StorageAndPersonnel " +
                                        " where WorkID = '" + BasicInfo.LoginID + "') or OutStoreRoom in (select " +
                                        " StorageID from BASE_StorageAndPersonnel where WorkID = '" +
                                        BasicInfo.LoginID + "') or LRRY = '" + BasicInfo.LoginID + "') ";
                }

                strSql += " order by DJH desc  ";

                return GlobalObject.DatabaseServer.QueryInfo(strSql);
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="billID">单据ID</param>
        public void DeleteBill(int billID)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;
            try
            {
                DeleteList(dataContxt, billID);

                var varData = from a in dataContxt.S_CannibalizeBill
                              where a.ID == billID
                              select a;

                if (varData.Count() > 0)
                {
                    //对于总成条码的删除
                    var varMarkting = from a in dataContxt.ProductsCodes
                                      where a.DJH == varData.Single().DJH
                                      select a;

                    dataContxt.ProductsCodes.DeleteAllOnSubmit(varMarkting);
                }

                dataContxt.S_CannibalizeBill.DeleteAllOnSubmit(varData);
                dataContxt.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 删除单据明细
        /// </summary>
        /// <param name="dataContxt">数据上下文</param>
        /// <param name="djID">单据ID</param>
        private void DeleteList(DepotManagementDataContext dataContxt, int djID)
        {
            try
            {
                var varData = from a in dataContxt.S_CannibalizeList
                              where a.DJ_ID == djID
                              select a;

                if (varData.Count() != 0)
                {
                    dataContxt.S_CannibalizeList.DeleteAllOnSubmit(varData);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 变更单据状态（审核）
        /// </summary>
        /// <param name="djID">单据ID</param>
        /// <param name="remark">备注</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>变更成功返回True，变更失败返回False</returns>
        public bool AuditingBill(int djID, string remark, out string error)
        {
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                var varData = from a in dataContxt.S_CannibalizeBill
                              where a.ID == djID
                              select a;

                if (varData.Count() == 0)
                {
                    error = "无记录";
                }
                else
                {
                    S_CannibalizeBill lnqMarkBill = varData.Single();

                    lnqMarkBill.SHRY = BasicInfo.LoginID;
                    lnqMarkBill.DJZT = "已审核";
                    lnqMarkBill.Remark = remark;
                    lnqMarkBill.SHRQ = ServerTime.Time;

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
        /// 变更单据状态（检测）
        /// </summary>
        /// <param name="djID">单据ID</param>
        /// <param name="remark">备注</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>变更成功返回True，变更失败返回False</returns>
        public bool QualityBill(int djID, string remark, out string error)
        {
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                var varData = from a in dataContxt.S_CannibalizeBill
                              where a.ID == djID
                              select a;

                if (varData.Count() == 0)
                {
                    error = "无记录";
                }
                else
                {
                    S_CannibalizeBill lnqMarkBill = varData.Single();

                    lnqMarkBill.Checker = BasicInfo.LoginID;
                    lnqMarkBill.DJZT = "已检测";
                    lnqMarkBill.Remark = remark;
                    lnqMarkBill.CheckTime = ServerTime.Time;

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
        /// 编辑检验状态
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <param name="remark">备注</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>编辑成功返回True，编辑失败返回False</returns>
        public bool CheckBill(string djh, string remark, out string error)
        {
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                var varData = from a in dataContxt.S_CannibalizeBill
                              where a.DJH == djh
                              select a;

                if (varData.Count() != 0)
                {
                    S_CannibalizeBill lnqMarketing = varData.Single();

                    lnqMarketing.DJZT = "已批准";
                    lnqMarketing.CWRY = BasicInfo.LoginID;
                    lnqMarketing.Remark = remark;
                    lnqMarketing.CWRQ = ServerTime.Time;

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
        /// 获得某一个调拨单单据信息
        /// </summary>
        /// <param name="djID">单据ID</param>
        /// <returns>返回一条调拨单单据信息</returns>
        public S_CannibalizeBill GetBill(int djID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            return GetBill(ctx, djID);
        }

        S_CannibalizeBill GetBill(DepotManagementDataContext ctx, int djID)
        {
            var varData = from a in ctx.S_CannibalizeBill
                          where a.ID == djID
                          select a;

            if (varData.Count() != 1)
            {
                throw new Exception("单据信息为空或者不唯一");
            }
            else
            {
                return varData.Single();
            }
        }

        /// <summary>
        /// 获得单据明细
        /// </summary>
        /// <param name="djID">单据ID</param>
        /// <returns>返回一张单据的所有明细信息</returns>
        List<S_CannibalizeList> GetListInfo(int djID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.S_CannibalizeList
                          where a.DJ_ID == djID
                          select a;

            return varData.ToList();
        }

        /// <summary>
        /// 获得单据明细
        /// </summary>
        /// <param name="djID">单据ID</param>
        /// <returns>返回一张单据的所有明细信息</returns>
        public DataTable GetList(int djID)
        {
            string strSql = "select a.DJ_ID,a.GoodsID,b.图号型号 as GoodsCode,b.物品名称 as GoodsName,b.规格 as Spec,a.BatchNo," +
                            " a.Count,b.单位 as Unit,a.UnitPrice,a.Provider,b.物品类别 as Depot," +
                            " a.Price,a.Remark,a.RepairStatus from S_CannibalizeList as a inner join " +
                            " View_F_GoodsPlanCost as b on a.GoodsID = b.序号 where " +
                            " DJ_ID = " + djID;

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dt.Rows.Count == 0)
            {
                return null;
            }

            return dt;
        }

        /// <summary>
        /// 保存单据数据(如果单据信息ID为0则添加数据，否则更新数据)
        /// </summary>
        /// <param name="billList">单据明细</param>
        /// <param name="billInfo">单据信息</param>
        public void SaveBill(DataTable billList, S_CannibalizeBill billInfo)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            dataContxt.Connection.Open();
            dataContxt.Transaction = dataContxt.Connection.BeginTransaction();

            try
            {
                int intDJID = 0;

                if (billInfo.ID == 0)//添加
                {
                    S_CannibalizeBill lnqBill = new S_CannibalizeBill();

                    lnqBill.DJH = billInfo.DJH;
                    lnqBill.LRRY = billInfo.LRRY;
                    lnqBill.LRRQ = billInfo.LRRQ;
                    lnqBill.DJZT = "已保存";
                    lnqBill.Remark = billInfo.Remark;
                    lnqBill.Price = billInfo.Price;
                    lnqBill.OutStoreRoom = billInfo.OutStoreRoom;
                    lnqBill.InStoreRoom = billInfo.InStoreRoom;

                    dataContxt.S_CannibalizeBill.InsertOnSubmit(lnqBill);
                    dataContxt.SubmitChanges();

                    var varData = from a in dataContxt.S_CannibalizeBill
                                  where a.DJH == billInfo.DJH
                                  select a;

                    if (varData.Count() == 1)
                    {
                        intDJID = varData.Single().ID;
                    }
                    else
                    {
                        throw new Exception("数据为空或者不唯一");
                    }
                }
                else //更新
                {
                    var varData = from a in dataContxt.S_CannibalizeBill
                                  where a.ID == billInfo.ID
                                  select a;

                    if (varData.Count() != 0)
                    {
                        S_CannibalizeBill lnqBill = varData.Single();

                        lnqBill.LRRY = billInfo.LRRY;
                        lnqBill.LRRQ = billInfo.LRRQ;
                        lnqBill.Price = billInfo.Price;
                        lnqBill.DJZT = "已保存";
                        lnqBill.SHRQ = null;
                        lnqBill.SHRY = null;
                        lnqBill.CWRQ = null;
                        lnqBill.CWRY = null;
                        lnqBill.KFRQ = null;
                        lnqBill.KFRY = null;

                        intDJID = billInfo.ID;
                    }
                }

                List<S_CannibalizeList> lisList = new List<S_CannibalizeList>();

                for (int i = 0; i < billList.Rows.Count; i++)
                {
                    S_CannibalizeList lnqList = new S_CannibalizeList();

                    lnqList.GoodsID = Convert.ToInt32(billList.Rows[i]["GoodsID"].ToString());
                    lnqList.BatchNo = billList.Rows[i]["BatchNo"].ToString();
                    lnqList.UnitPrice = Convert.ToDecimal(billList.Rows[i]["UnitPrice"]);
                    lnqList.Count = Convert.ToDecimal(billList.Rows[i]["Count"]);
                    lnqList.Price = Convert.ToDecimal(billList.Rows[i]["Price"]);
                    lnqList.Remark = billList.Rows[i]["Remark"].ToString();
                    lnqList.Provider = billList.Rows[i]["Provider"].ToString();

                    if (billList.Rows[i]["RepairStatus"] != null && billList.Rows[i]["RepairStatus"].ToString() != "")
                    {
                        lnqList.RepairStatus = billList.Rows[i]["RepairStatus"].ToString() == "1" ? true : false;
                    }

                    lisList.Add(lnqList);
                }

                SaveBillList(dataContxt, lisList, intDJID, billInfo.DJH);

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
        /// 保存单据明细信息
        /// </summary>
        /// <param name="dataContxt">LINQ数据上下文</param>
        /// <param name="lstDetail">添加的表</param>
        /// <param name="djID">添加的行</param>
        /// <param name="billNo">单据号</param>
        public void SaveBillList(DepotManagementDataContext dataContxt, List<S_CannibalizeList> lstDetail, int djID, string billNo)
        {
            IProductCodeServer serverProductCode = ServerModuleFactory.GetServerModule<IProductCodeServer>();

            try
            {
                List<S_CannibalizeList> lisList = new List<S_CannibalizeList>();

                if (lstDetail.Count == 0)
                {
                    return;
                }

                DeleteList(dataContxt, djID);
                foreach (S_CannibalizeList item in lstDetail)
                {
                    S_CannibalizeList lnqList = new S_CannibalizeList();

                    lnqList.DJ_ID = djID;
                    lnqList.GoodsID = item.GoodsID;
                    lnqList.BatchNo = item.BatchNo;
                    lnqList.UnitPrice = item.UnitPrice;
                    lnqList.Count = item.Count;
                    lnqList.Price = item.Price;
                    lnqList.Remark = item.Remark;
                    lnqList.Provider = item.Provider;

                    if (item.RepairStatus != null && item.RepairStatus.ToString() != "")
                    {
                        lnqList.RepairStatus = item.RepairStatus;
                    }

                    lisList.Add(lnqList);
                }

                dataContxt.S_CannibalizeList.InsertAllOnSubmit(lisList);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获得最大的单据号
        /// </summary>
        /// <returns>返回单据ID</returns>
        private int GetDJID()
        {
            string strSql = "select Max(ID) from S_CannibalizeBill";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return Convert.ToInt32(dt.Rows[0][0]);
        }

        /// <summary>
        /// 仓管确认
        /// </summary>
        /// <param name="djID">单据ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>确认成功返回True，确认失败返回False</returns>
        public bool AffirmBill(int djID, out string error)
        {
            error = null;
            ISellIn serverSell = ServerModule.ServerModuleFactory.GetServerModule<ISellIn>();

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;
            dataContxt.Connection.Open();
            dataContxt.Transaction = dataContxt.Connection.BeginTransaction();

            try
            {

                var varBill = from a in dataContxt.S_CannibalizeBill
                              where a.ID == djID
                              select a;

                if (varBill.Count() != 1)
                {
                    throw new Exception("单据信息为空或者不唯一");
                }

                S_CannibalizeBill tempBillInfo = varBill.Single();

                if (tempBillInfo.DJZT == "已确认")
                {
                    throw new Exception("单据不能重复确认");
                }

                tempBillInfo.DJZT = "已确认";
                tempBillInfo.KFRY = BasicInfo.LoginID;
                tempBillInfo.KFRQ = ServerTime.Time;

                //操作总成库存状态
                if (!serverSell.UpdateProductStock(dataContxt, tempBillInfo.DJH, "调拨", tempBillInfo.OutStoreRoom,
                    tempBillInfo.InStoreRoom, out error))
                {
                    return false;
                }

                OpertaionDetailAndStock(dataContxt, tempBillInfo, CE_SubsidiaryOperationType.库房调出);
                OpertaionDetailAndStock(dataContxt, tempBillInfo, CE_SubsidiaryOperationType.库房调入);

                dataContxt.SubmitChanges();
                dataContxt.Transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                dataContxt.Transaction.Rollback();
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 根据单据信息操作账务信息与库存信息
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="bill">单据信息</param>
        /// <param name="operationType">操作类型</param>
        void OpertaionDetailAndStock(DepotManagementDataContext dataContext, S_CannibalizeBill bill, 
            CE_SubsidiaryOperationType operationType)
        {
            IFinancialDetailManagement serverDetail =
                ServerModule.ServerModuleFactory.GetServerModule<IFinancialDetailManagement>();

            var result = from r in dataContext.S_CannibalizeList
                         where r.DJ_ID == bill.ID
                         select r;

            foreach (var item in result)
            {
                S_FetchGoodsDetailBill detailInfo = AssignDetailInfo(dataContext, bill, item, operationType);
                S_Stock stockInfo = AssignStockInfo(dataContext, bill, item, operationType);

                if (detailInfo == null || stockInfo == null)
                {
                    throw new Exception("获取账务信息或者库存信息失败");
                }

                serverDetail.ProcessFetchGoodsDetail(dataContext, detailInfo, stockInfo);
            }
        }

        /// <summary>
        /// 赋值库存信息
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="billInfo">单据信息</param>
        /// <param name="listSingle">明细信息</param>
        /// <param name="operationType">操作类型</param>
        /// <returns>返回库存信息对象</returns>
        S_Stock AssignStockInfo(DepotManagementDataContext dataContext, S_CannibalizeBill billInfo, S_CannibalizeList listSingle,
            CE_SubsidiaryOperationType operationType)
        {
            S_Stock tempLnqStock = new S_Stock();

            tempLnqStock.GoodsID = Convert.ToInt32(listSingle.GoodsID);
            tempLnqStock.BatchNo = listSingle.BatchNo;
            tempLnqStock.ExistCount = (decimal)listSingle.Count;
            tempLnqStock.Date = ServerTime.Time;
            tempLnqStock.Provider = listSingle.Provider;
            tempLnqStock.UnitPrice = (decimal)listSingle.UnitPrice;
            tempLnqStock.Price = tempLnqStock.UnitPrice * tempLnqStock.ExistCount;

            if (operationType == CE_SubsidiaryOperationType.库房调出)
            {
                tempLnqStock.StorageID = billInfo.OutStoreRoom;
            }
            else if (operationType == CE_SubsidiaryOperationType.库房调入)
            {
                tempLnqStock.StorageID = billInfo.InStoreRoom;
            }
            else
            {
                throw new Exception("业务类型错误");
            }

            QueryCondition_Store store = new QueryCondition_Store();

            store.BatchNo = tempLnqStock.BatchNo;
            store.GoodsID = tempLnqStock.GoodsID;
            store.StorageID = billInfo.OutStoreRoom;
            store.Provider = tempLnqStock.Provider;

            S_Stock stockInfo = UniversalFunction.GetStockInfo(dataContext, store);
            tempLnqStock.GoodsStatus = stockInfo.GoodsStatus;

            tempLnqStock.InputPerson = BasicInfo.LoginID;

            return tempLnqStock;
        }

        /// <summary>
        /// 赋值账务信息
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="billInfo">单据信息</param>
        /// <param name="listSingle">明细信息</param>
        /// <param name="operationType">操作类型</param>
        /// <returns>返回账务信息</returns>
        S_FetchGoodsDetailBill AssignDetailInfo(DepotManagementDataContext dataContext, S_CannibalizeBill billInfo, S_CannibalizeList listSingle, 
            CE_SubsidiaryOperationType operationType)
        {
            S_FetchGoodsDetailBill lnqOutDepot = new S_FetchGoodsDetailBill();

            bool flag = false;

            if (operationType == CE_SubsidiaryOperationType.库房调出)
            {
                flag = true;
            }
            else if (operationType == CE_SubsidiaryOperationType.库房调入)
            {
                flag = false;
            }
            else
            {
                throw new Exception("业务类型错误");
            }

            lnqOutDepot.ID = Guid.NewGuid();
            lnqOutDepot.FetchBIllID = billInfo.DJH;
            lnqOutDepot.BillTime = ServerTime.Time;
            lnqOutDepot.GoodsID = (int)listSingle.GoodsID;
            lnqOutDepot.Provider = listSingle.Provider;
            lnqOutDepot.BatchNo = listSingle.BatchNo;
            lnqOutDepot.FetchCount = flag ? listSingle.Count : -listSingle.Count;
            lnqOutDepot.UnitPrice = (decimal)listSingle.UnitPrice;
            lnqOutDepot.Price = flag ? (decimal)listSingle.Price : -(decimal)listSingle.Price;
            lnqOutDepot.Using = flag ? "仓库调拨单：调出" : "仓库调拨：调入";
            lnqOutDepot.Department = UniversalFunction.GetStorageName(dataContext, flag ? billInfo.InStoreRoom : billInfo.OutStoreRoom);
            lnqOutDepot.FillInPersonnel = UniversalFunction.GetPersonnelInfo(dataContext, billInfo.LRRY).姓名;
            lnqOutDepot.DepartDirector = UniversalFunction.GetPersonnelInfo(dataContext, billInfo.SHRY).姓名;
            lnqOutDepot.DepotManager = BasicInfo.LoginName;
            lnqOutDepot.OperationType = flag ? (int)CE_SubsidiaryOperationType.库房调出 : (int)CE_SubsidiaryOperationType.库房调入;
            lnqOutDepot.Remark = billInfo.Remark;
            lnqOutDepot.StorageID = flag ? billInfo.OutStoreRoom : billInfo.InStoreRoom;
            lnqOutDepot.FillInDate = billInfo.LRRQ;

            return lnqOutDepot;
        }
    }
}
