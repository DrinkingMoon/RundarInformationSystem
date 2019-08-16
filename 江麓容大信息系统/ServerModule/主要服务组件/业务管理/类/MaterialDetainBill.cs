/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  MaterialDetainBill.cs
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
using System.Linq;
using System.Text;
using System.Data;
using GlobalObject;
using System.Data.Linq;
using PlatformManagement;

namespace ServerModule
{
    /// <summary>
    /// 扣货单管理类
    /// </summary>
    class MaterialDetainBill : BasicServer, IMaterialDetainBill
    {
        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 人员服务组件
        /// </summary>
        IPersonnelInfoServer m_serverPersonnelInfo = ServerModuleFactory.GetServerModule<IPersonnelInfoServer>();

        /// <summary>
        /// 产品信息服务组件
        /// </summary>
        IProductListServer m_serverProductList = ServerModuleFactory.GetServerModule<IProductListServer>();

        /// <summary>
        /// 库存服务组件
        /// </summary>
        IStoreServer m_serverStore = ServerModuleFactory.GetServerModule<IStoreServer>();

        /// <summary>
        /// 基础物品服务组件
        /// </summary>
        IBasicGoodsServer m_serverBasicGoods = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();

        /// <summary>
        /// 单据编号分配服务
        /// </summary>
        IAssignBillNoServer m_assignBill = ServerModuleFactory.GetServerModule<IAssignBillNoServer>();

        /// <summary>
        /// 检查某业务是否存在
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">业务号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.S_MaterialDetainBill
                          where a.Bill_ID == billNo
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
            string sql = "SELECT * FROM [DepotManagement].[dbo].[S_MaterialDetainBill] where Bill_ID = '" + billNo + "'";

            System.Data.DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 获得单条记录信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回LINQ单条信息</returns>
        public S_MaterialDetainBill GetBill(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.S_MaterialDetainBill
                          where a.Bill_ID == billNo
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
        /// 获得全部单据信息
        /// </summary>
        /// <returns>返回扣货单表信息</returns>
        public DataTable GetAllBill()
        {
            string strSql = "select * from View_S_MaterialDetainBill";

            if (BasicInfo.ListRoles.Contains(CE_RoleEnum.业务系统管理员.ToString()))
            {
                strSql += " order by 单据号 desc";
            }
            else if (BasicInfo.ListRoles.Contains(CE_RoleEnum.采购员.ToString()) || BasicInfo.ListRoles.Contains(CE_RoleEnum.采购主管.ToString()))
            {
                strSql += " where (单据状态='等待采购确认' or 单据状态='单据已完成') " +
                          " and (采购确认人='" + BasicInfo.LoginName + "' or 采购确认人 is null) order by 单据号 desc";
            }
            else if (BasicInfo.ListRoles.Contains(CE_RoleEnum.质控主管.ToString()))
            {
                strSql += " where 质管批准 = '" + BasicInfo.LoginName + "' or 质管批准 is null order by 单据号 desc";
            }
            else
            {
                strSql += " order by 单据号 desc";
            }

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dt;
        }

        /// <summary>
        /// 获得子表信息
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回满足条件的数据集</returns>
        public DataTable GetList(string billID, out string error)
        {
            error = "";

            string sql = "select *  from View_S_MaterialDetainList" +
                         " where 扣货单号='" + billID + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 添加扣货单
        /// </summary>
        /// <param name="bill">扣货单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回True，否则返回False</returns>
        public bool AddBill(S_MaterialDetainBill bill,out string error)
        {
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                var result = from r in dataContxt.S_MaterialDetainBill 
                             where r.Bill_ID == bill.Bill_ID 
                             select r;

                if (result.Count() == 0)
                {
                    dataContxt.S_MaterialDetainBill.InsertOnSubmit(bill);
                }

                dataContxt.SubmitChanges();

                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 编制人修改扣货单
        /// </summary>
        /// <param name="bill">扣货单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回True，否则返回False</returns>
        public bool UpdateBill(S_MaterialDetainBill bill, out string error)
        {
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                var result = from r in dataContxt.S_MaterialDetainBill
                             where r.Bill_ID == bill.Bill_ID
                             select r;

                if (result.Count() > 0)
                {
                    S_MaterialDetainBill detain = result.Single();

                    detain.Bill_Time = ServerModule.ServerTime.Time;
                    detain.Provider = bill.Provider;
                    detain.Reason = bill.Reason;
                    detain.Remark = bill.Remark;
                    detain.BillStatus = "等待领导审核";
                }

                dataContxt.SubmitChanges();

                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 删除子表信息
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        public bool DeleteList(string djh, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var varData = from a in dataContxt.S_MaterialDetainList
                              where a.Bill_ID == djh
                              select a;

                dataContxt.S_MaterialDetainList.DeleteAllOnSubmit(varData);
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
        /// 删除物品信息
        /// </summary>
        /// <param name="idList">要删除的物品ID列表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功的标志</returns>
        public bool DeleteGoods(List<long> idList, out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from r in dataContxt.S_MaterialDetainList
                             where idList.Contains(r.ID)
                             select r;

                if (result.Count() > 0)
                {
                    dataContxt.S_MaterialDetainList.DeleteAllOnSubmit(result);
                    dataContxt.SubmitChanges();
                }

                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 删除主表信息
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，否则返回False</returns>
        public bool DeleteBill(string billID,out string error)
        {
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                Table<S_MaterialDetainBill> table = dataContxt.GetTable<S_MaterialDetainBill>();

                var delRow = from c in table
                             where c.Bill_ID == billID
                             select c;

                if (DeleteList(billID, out error))
                {
                    table.DeleteAllOnSubmit(delRow);
                }
                else
                {
                    return false;
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
        /// 领导审核，修改主表的单据状态
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回True，否则返回False</returns>
        public bool AuditingBill(string billNo,  out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from r in dataContxt.S_MaterialDetainBill
                             where r.Bill_ID == billNo
                             select r;

                if (result.Count() == 0)
                {
                    error = string.Format("没有找到单据号为 {0} 的物料扣货单信息，无法进行此操作", billNo);
                    return false;
                }

                result.Single().AuditingPerson = BasicInfo.LoginName;
                result.Single().AuditingDate = ServerTime.Time;
                result.Single().BillStatus = "等待质管批准";

                dataContxt.SubmitChanges();

                return true;
            }
            catch (Exception err)
            {
                error = err.Message;
                return false;
            }
        }

        /// <summary>
        /// 质管批准，修改主表的单据状态
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="name">质管人编号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回True，否则返回False</returns>
        public bool AuthorizeBill(string billNo, string name,out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from r in dataContxt.S_MaterialDetainBill
                             where r.Bill_ID == billNo 
                             select r;

                if (result.Count() == 0)
                {
                    error = string.Format("没有找到单据号为 {0} 的物料扣货单信息，无法进行此操作", billNo);
                    return false;
                }

                result.Single().AuthorizePerson = name;
                result.Single().AuthorizeDate = ServerTime.Time;
                result.Single().BillStatus = "等待SQE确认";

                dataContxt.SubmitChanges();

                return true;
            }
            catch (Exception err)
            {
                error = err.Message;
                return false;
            }
        }

        /// <summary>
        /// SQE确认，修改主表的单据状态
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回True，否则返回False</returns>
        public bool SQEConfirmBill(string billNo, out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from r in dataContxt.S_MaterialDetainBill
                             where r.Bill_ID == billNo
                             select r;

                if (result.Count() == 0)
                {
                    error = string.Format("没有找到单据号为 {0} 的物料扣货单信息，无法进行此操作", billNo);
                    return false;
                }

                result.Single().SQE = BasicInfo.LoginName;
                result.Single().SQEDate = ServerTime.Time;
                result.Single().BillStatus = "等待采购确认";

                dataContxt.SubmitChanges();

                return true;
            }
            catch (Exception err)
            {
                error = err.Message;
                return false;
            }
        }

        /// <summary>
        /// 采购确认，修改主表的单据状态
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="name">采购确认人编号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回True，否则返回False</returns>
        public bool ConfirmBill(string billNo, string name, out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from r in dataContxt.S_MaterialDetainBill
                             where r.Bill_ID == billNo
                             select r;

                if (result.Count() == 0)
                {
                    error = string.Format("没有找到单据号为 {0} 的物料扣货单信息，无法进行此操作", billNo);
                    return false;
                }
                
                DataTable dtTemp = GetList(billNo,out error);
                DataTable dtBill = GetTableByID(billNo);

                if (dtBill.Rows.Count == 1)
                {

                    if (!InsertMaterialReturnedInTheDepot(dataContxt, dtBill, dtTemp, billNo,out error))
                    {
                        return false;
                    }

                    if (!InsertMaterialRejectBill(dataContxt, dtBill, dtTemp, billNo, out error))
                    {
                        return false;
                    }

                    result.Single().ConfirmPerson = name;
                    result.Single().ConfirmDate = ServerTime.Time;
                    result.Single().BillStatus = "单据已完成";

                    dataContxt.SubmitChanges();
                }
                else
                {
                    return false;
                }

                return true;
            }
            catch (Exception err)
            {
                error = err.Message;
                return false;
            }
        }

        /// <summary>
        /// 插入领料退库表
        /// </summary>
        /// <param name="dataContxt">数据上下文</param>
        /// <param name="billList">扣货子表</param>
        /// <param name="billTable">扣货主表</param>
        /// <param name="billNo">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool InsertMaterialReturnedInTheDepot(DepotManagementDataContext dataContxt, DataTable billList,
            DataTable billTable, string billNo, out string error)
        {
            error = null;
            
            try
            {
                MaterialReturnedInTheDepot serverReturnedBill = new MaterialReturnedInTheDepot();
                string strLLBillNo = m_assignBill.AssignNewNo(serverReturnedBill, "领料退库单");
                string storageID = UniversalFunction.GetStorageID(billList.Rows[0]["库房名称"].ToString());

                //插入主表信息
                S_MaterialReturnedInTheDepot depotBill = new S_MaterialReturnedInTheDepot();

                depotBill.Bill_ID = strLLBillNo;
                depotBill.Bill_Time = ServerTime.Time;
                depotBill.BillStatus = MaterialReturnedInTheDepotBillStatus.已完成.ToString();
                depotBill.Department = billList.Rows[0]["部门编码"].ToString();
                depotBill.ReturnType = "机加退库";//退库类别
                depotBill.FillInPersonnel = billList.Rows[0]["建单人"].ToString();
                depotBill.FillInPersonnelCode = billList.Rows[0]["建单人编号"].ToString();
                depotBill.DepartmentDirector = billList.Rows[0]["审核人"].ToString();
                depotBill.QualityInputer = billList.Rows[0]["质管批准"].ToString();
                depotBill.DepotManager = billList.Rows[0]["采购确认人"].ToString();

                throw new Exception("用途不明，无法使用此单据");
                //depotBill.PurposeCode = "30";//用途

                //depotBill.ReturnReason = billList.Rows[0]["扣货原因"].ToString();
                //depotBill.Remark = billList.Rows[0]["备注"].ToString() + "（根据物料扣货单【" + billNo + "】系统自动生成）";
                //depotBill.StorageID = storageID;
                //depotBill.ReturnMode = "领料退库";//退库方式
                //depotBill.IsOnlyForRepair = false;
                //depotBill.InDepotDate = ServerTime.Time;

                //dataContxt.S_MaterialReturnedInTheDepot.InsertOnSubmit(depotBill);

                //View_Department department = ServerModuleFactory.GetServerModule<IDepartmentServer>().GetDepartments(depotBill.Department);

                //for (int i = 0; i < billTable.Rows.Count; i++)
                //{
                //    #region 领料退库明细
                //    //插入业务明细
                //    S_MaterialListReturnedInTheDepot lnqDepotList = new S_MaterialListReturnedInTheDepot();

                //    lnqDepotList.Bill_ID = strLLBillNo;
                //    lnqDepotList.GoodsID = Convert.ToInt32(billTable.Rows[i]["物品ID"].ToString());
                //    lnqDepotList.Provider = billTable.Rows[i]["供应商"].ToString();
                //    lnqDepotList.ProviderBatchNo = "";
                //    lnqDepotList.BatchNo = billTable.Rows[i]["批次号"].ToString();
                //    lnqDepotList.ReturnedAmount = Convert.ToDecimal(billTable.Rows[i]["扣货数"].ToString());
                //    lnqDepotList.Remark = billTable.Rows[i]["备注"].ToString();

                //    DataRow dtTemp = GetShelfArea(Convert.ToInt32(billTable.Rows[i]["物品ID"].ToString()), 
                //        billTable.Rows[i]["批次号"].ToString(), out error);

                //    lnqDepotList.ShelfArea = dtTemp == null ? "" : dtTemp["货架"].ToString();//货架
                //    lnqDepotList.ColumnNumber = dtTemp == null ? "" : dtTemp["列"].ToString();//列
                //    lnqDepotList.LayerNumber = dtTemp == null ? "" : dtTemp["层"].ToString();//层
                //    lnqDepotList.Depot = dtTemp == null ? "" : dtTemp["材料类别编码"].ToString();

                //    dataContxt.S_MaterialListReturnedInTheDepot.InsertOnSubmit(lnqDepotList);
                //    #endregion
                //}

                //serverReturnedBill.OpertaionDetailAndStock(dataContxt, depotBill);
                //return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 插入采购退货表
        /// </summary>
        /// <param name="dataContxt">LINQ数据上下文</param>
        /// <param name="billList">扣货子表</param>
        /// <param name="billTable">扣货主表</param>
        /// <param name="billNo">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool InsertMaterialRejectBill(DepotManagementDataContext dataContxt, DataTable billList,
            DataTable billTable, string billNo, out string error)
        {
            error = null;

            try
            {
                MaterialRejectBill serverRejectBill = new MaterialRejectBill();

                string strCGBillNo = m_assignBill.AssignNewNo(serverRejectBill, "采购退货单");

                //插入主表信息
                S_MaterialRejectBill bill = new S_MaterialRejectBill();

                if (billTable.Rows.Count > 0)
                {
                    bill.Bill_ID = strCGBillNo;
                    bill.Bill_Time = ServerTime.Time;
                    bill.BillStatus = MaterialRejectBillBillStatus.已完成.ToString();
                    bill.Department = BasicInfo.DeptCode;
                    bill.FillInPersonnel = BasicInfo.LoginName;
                    bill.FillInPersonnelCode = BasicInfo.LoginID;
                    bill.Provider = billList.Rows[0]["供应商"].ToString();
                    bill.Reason = billList.Rows[0]["扣货原因"].ToString();
                    bill.Remark = billList.Rows[0]["备注"].ToString() + "（根据物料扣货单【" + billNo + "】系统自动生成）";
                    bill.BillType = "总仓库退货单";
                    bill.StorageID = UniversalFunction.GetStorageID(billList.Rows[0]["库房名称"].ToString());
                    bill.OutDepotDate = ServerTime.Time;

                    dataContxt.S_MaterialRejectBill.InsertOnSubmit(bill);
                }
                else
                    return false;

                for (int i = 0; i < billTable.Rows.Count; i++)
                {
                    //插入业务明细信息
                    S_MaterialListRejectBill goods = new S_MaterialListRejectBill();

                    goods.Bill_ID = strCGBillNo;
                    goods.GoodsID = Convert.ToInt32(billTable.Rows[i]["物品ID"].ToString());
                    goods.Provider = billTable.Rows[i]["供应商"].ToString();
                    goods.ProviderBatchNo = "";
                    goods.BatchNo = billTable.Rows[i]["批次号"].ToString();
                    goods.Amount = Convert.ToDecimal(billTable.Rows[i]["扣货数"].ToString());
                    goods.Remark = billTable.Rows[i]["备注"].ToString();
                    goods.AssociateID = billTable.Rows[i]["关联订单号"].ToString();

                    if (!new MaterialListRejectBill().SetPriceInfo(goods.AssociateID, goods,
                        UniversalFunction.GetStorageID(billList.Rows[0]["库房名称"].ToString()),
                        out error))
                    {
                        return false;
                    }

                    dataContxt.S_MaterialListRejectBill.InsertOnSubmit(goods);
                }

                serverRejectBill.OpertaionDetailAndStock(dataContxt, bill);
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 通过物品ID和批次号获得货架
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <param name="error">错误信息</param>
        /// <returns>返回满足条件的货架</returns>
        private DataRow GetShelfArea(int goodsID,string batchNo,out string error)
        {
            error="";

            string sql = "select 货架,列,层,材料类别编码,实际单价 from dbo.View_S_Stock "+
                         " where 物品ID='" + goodsID + "' and 批次号='" + batchNo + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count != 0)
            {
                return dt.Rows[0];
            }
            else
            {
                error = "数据错误，请核对后再进行此操作";
                return null;
            }
        }

        /// <summary>
        /// 通过单据号查找主表信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回满足条件的数据集</returns>
        private DataTable GetTableByID(string billNo)
        {
            string sql = "select * from View_S_MaterialDetainBill where 单据号='"+billNo+"'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 添加子表信息
        /// </summary>
        /// <param name="list">子表数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回True，否则返回False</returns>
        public bool AddList(S_MaterialDetainList list,out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                dataContxt.S_MaterialDetainList.InsertOnSubmit(list);

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
        /// 采购员选择订单号，修改子表信息
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="orderFormID">关联订单号</param>
        /// <param name="batchNo">批次号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回True，否则返回False</returns>
        public bool UpdateList(string billID,string goodsID,string orderFormID, string batchNo,out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.S_MaterialDetainList
                             where a.Bill_ID == billID && a.GoodsID == Convert.ToInt32(goodsID)
                             && a.BatchNo == batchNo
                             select a;

                if (result.Count() == 1)
                {
                    S_MaterialDetainList list = result.Single();

                    list.AssociateID = orderFormID;

                    dataContxt.SubmitChanges();

                    return true;
                }
                else
                {
                    error = "数据错误，请核对后再操作！";
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
        /// 编制人修改子表信息
        /// </summary>
        /// <param name="goods">扣货单明细信息</param>
        /// <param name="storageID">库房ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>修改成功返回True，修改失败返回False</returns>
        public bool UpdateList(S_MaterialDetainList goods, string storageID, out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from r in dataContxt.S_MaterialDetainList
                             where r.ID == goods.ID
                             select r;

                if (result.Count() > 0)
                {
                    S_MaterialDetainList updateGoods = result.Single();

                    updateGoods.Bill_ID = goods.Bill_ID;
                    updateGoods.GoodsID = goods.GoodsID;
                    updateGoods.Provider = goods.Provider;
                    updateGoods.BatchNo = goods.BatchNo;
                    updateGoods.Amount = goods.Amount;
                    updateGoods.Remark = goods.Remark;

                    dataContxt.SubmitChanges();
                }

                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 通过物品的ID获得订单信息
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <param name="provider">供货单位</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回满足条件的数据集</returns>
        public DataTable GetOrderFormInfo(string goodsID,string batchNo,string provider,out string error)
        {
            error = "";

            string sql = "select view_B_OrderFormInfo.订单号,view_B_OrderFormInfo.合同号,订货员," +
                         " view_B_OrderFormInfo.订货日期 from view_B_OrderFormGoods inner join" +
                         " view_B_OrderFormInfo on view_B_OrderFormGoods.订单号= view_B_OrderFormInfo.订单号" +
                         " inner join view_S_Stock on view_S_Stock.物品ID = view_B_OrderFormGoods.物品ID" +
                         " where view_B_OrderFormGoods.物品ID = "+ goodsID + " and " +
                         " 批次号='" + batchNo + "' and view_B_OrderFormInfo.供货单位='" + provider + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 回退单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="billStatus">单据状态</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <param name="rebackReason">回退原因</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool ReturnBill(string billNo, string billStatus, out string error, string rebackReason)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.S_MaterialDetainBill
                              where a.Bill_ID == billNo
                              select a;

                string strMsg = "";

                if (varData.Count() == 1)
                {
                    S_MaterialDetainBill lnqTemp = varData.Single();

                    Nullable<DateTime> nulldt = null;

                    lnqTemp.BillStatus = billStatus;

                    switch (billStatus)
                    {
                        case "新建单据":
                            strMsg = string.Format("{0}号物料扣货单单已回退，请您重新处理单据; 回退原因为" + rebackReason, billNo);
                            m_billMessageServer.PassFlowMessage(billNo, strMsg, BillFlowMessage_ReceivedUserType.用户, lnqTemp.FillInPersonCode);

                            lnqTemp.AuditingDate = nulldt;
                            lnqTemp.AuditingPerson = null;
                            lnqTemp.AuthorizeDate = nulldt;
                            lnqTemp.AuthorizePerson = null;
                            lnqTemp.SQE = null;
                            lnqTemp.SQEDate = nulldt;

                            break;
                        case "等待领导审核":
                            strMsg = string.Format("{0}号物料扣货单单已回退，请您重新处理单据; 回退原因为" + rebackReason, billNo);
                            m_billMessageServer.PassFlowMessage(billNo, strMsg, BillFlowMessage_ReceivedUserType.角色, 
                                m_billMessageServer.GetDeptDirectorRoleName(lnqTemp.Department).ToList());

                            lnqTemp.AuthorizeDate = nulldt;
                            lnqTemp.AuthorizePerson = null;
                            lnqTemp.SQE = null;
                            lnqTemp.SQEDate = nulldt;

                            break;
                        case "等待质管批准":
                            strMsg = string.Format("{0}号物料扣货单单已回退，请您重新处理单据; 回退原因为" + rebackReason, billNo);
                            m_billMessageServer.PassFlowMessage(billNo, strMsg, BillFlowMessage_ReceivedUserType.角色, CE_RoleEnum.质量工程师.ToString());

                            lnqTemp.SQE = null;
                            lnqTemp.SQEDate = nulldt;

                            break;
                        case "等待SQE确认":
                            strMsg = string.Format("{0}号物料扣货单单已回退，请您重新处理单据; 回退原因为" + rebackReason, billNo);
                            m_billMessageServer.PassFlowMessage(billNo, strMsg, BillFlowMessage_ReceivedUserType.角色, CE_RoleEnum.SQE组员.ToString());

                            break;
                        default:
                            break;
                    }

                    dataContext.SubmitChanges();
                }
                else
                {
                    error = "数据不唯一或者为空";
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
    }
}
