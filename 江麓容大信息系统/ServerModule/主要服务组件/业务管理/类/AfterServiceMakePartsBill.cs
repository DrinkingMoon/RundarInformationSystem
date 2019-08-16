/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  AfterServiceMakePartsBill.cs
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
using DBOperate;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using PlatformManagement;
using GlobalObject;

namespace ServerModule
{
    /// <summary>
    /// 售后服务配件制造申请单状态
    /// </summary>
    public enum AfterServiceMakePartsBillStatus
    {
        /// <summary>
        /// 新建单据
        /// </summary>
        新建单据,

        /// <summary>
        /// 营销保存
        /// </summary>
        营销保存,

        /// <summary>
        /// 等待主管审核
        /// </summary>
        等待主管审核,

        /// <summary>
        /// 等待制造确认
        /// </summary>
        等待车间确认,

        /// <summary>
        /// 已完成
        /// </summary>
        已完成
    }

    /// <summary>
    /// 售后服务配件制造申请单服务类
    /// </summary>
    class AfterServiceMakePartsBill : BasicServer, ServerModule.IAfterServiceMakePartsBill
    {
        /// <summary>
        /// 部门信息服务组件
        /// </summary>
        IDepartmentServer m_serverDepartment = ServerModuleFactory.GetServerModule<IDepartmentServer>();

        /// <summary>
        /// 库存服务
        /// </summary>
        IStoreServer m_storeServer = ServerModuleFactory.GetServerModule<IStoreServer>();

        /// <summary>
        /// 生成单据号服务
        /// </summary>
        IAssignBillNoServer m_Assignbill = ServerModuleFactory.GetServerModule<IAssignBillNoServer>();

        /// <summary>
        /// 检查某业务是否存在
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">业务号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.YX_AfterServiceMakePartsBill
                          where a.BillID == billNo
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
            string strSql = "SELECT * FROM [DepotManagement].[dbo].[YX_AfterServiceMakePartsBill] where billid = '" + billNo + "'";

            System.Data.DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dt != null && dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获得单据信息
        /// </summary>
        /// <param name="startTime">检索开始时间</param>
        /// <param name="endTime">检索结束时间</param>
        /// <param name="billStatus">单据状态</param>
        /// <returns>成功返回获取到的单据主表信息，失败返回null</returns>
        public DataTable GetBill(DateTime startTime, DateTime endTime,string billStatus)
        {
            string strSql = "select * from View_YX_AfterServiceMakePartsBill where 申请日期 >= '"
                + startTime.Date + "' and 申请日期 <= '" + endTime.Date + "'";

            if (billStatus != "全  部")
            {
                strSql += " and 单据状态 = '" + billStatus + "' order by 单据号 desc";
            }

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dtTemp;
        }

        /// <summary>
        /// 获得单据明细
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <returns>成功返回获取到的单据明细信息，失败返回NULL</returns>
        public DataTable GetList(string billID)
        {
            string strSql = "select * from View_YX_AfterServiceMakePartsList where 单据号 = '" + billID + "'";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dtTemp;
        }

        /// <summary>
        /// 获得单据信息
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <returns>成功返回获取到的单据主表信息，失败返回null</returns>
        public YX_AfterServiceMakePartsBill GetBill(string billID)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.YX_AfterServiceMakePartsBill
                         where r.BillID == billID
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
        /// 批量删除单据信息
        /// </summary>
        /// <param name="billIDLis">批量删除单据号的集合</param>
        /// <param name="error">出错时返回的错误信息</param>
        /// <returns>True 删除成功 False 删除失败</returns>
        public bool DeleteBill(List<string> billIDLis, out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from r in dataContxt.YX_AfterServiceMakePartsBill
                             where billIDLis.Contains(r.BillID)
                             select r;

                dataContxt.YX_AfterServiceMakePartsBill.DeleteAllOnSubmit(result);

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
        /// 更新单据状态
        /// </summary>
        /// <param name="bill">单据主表信息</param>
        /// <param name="listTable">单据明细信息</param>
        /// <param name="billStatus">单据状态</param>
        /// <param name="error">出错时返回的错误信息</param>
        /// <returns>True 更新单据状态成功 False 更新单据状态失败</returns>
        public bool UpdateBill(YX_AfterServiceMakePartsBill bill, DataTable listTable,
            AfterServiceMakePartsBillStatus billStatus, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.YX_AfterServiceMakePartsBill
                              where a.BillID == bill.BillID
                              select a;

                switch (varData.Count())
                {
                    case 0://插入新记录
                        YX_AfterServiceMakePartsBill billInfo = new YX_AfterServiceMakePartsBill();

                        billInfo.BillID = bill.BillID;
                        billInfo.Applicant = bill.Applicant;
                        billInfo.RequestDate = bill.RequestDate;
                        billInfo.Checker = bill.Checker;
                        billInfo.CheckDate = bill.CheckDate;
                        billInfo.ConfirmedPeople = bill.ConfirmedPeople;
                        billInfo.ConfirmedDate = bill.ConfirmedDate;
                        billInfo.StorageID = bill.StorageID;
                        billInfo.Status = AfterServiceMakePartsBillStatus.等待主管审核.ToString();

                        if (!InsertBill(ctx, billInfo, out error))
                        {
                            return false;
                        }

                        if (!AddAllList(ctx, billInfo.BillID, listTable, out error))
                        {
                            return false;
                        }

                        break;

                    case 1://更新记录
                        YX_AfterServiceMakePartsBill lnqAferServiceBill = varData.Single();

                        if (lnqAferServiceBill.Status == "已完成")
                        {
                            error = "单据状态不符，请重新确认单据状态";
                            return false;
                        }

                        //根据不同的单据状态执行流程
                        switch (billStatus)
                        {
                            case AfterServiceMakePartsBillStatus.营销保存:

                                lnqAferServiceBill.Status = "等待主管审核";
                                lnqAferServiceBill.Applicant = BasicInfo.LoginName;
                                lnqAferServiceBill.RequestDate = ServerTime.Time;
                                lnqAferServiceBill.Checker = null;
                                lnqAferServiceBill.CheckDate = null;
                                lnqAferServiceBill.ConfirmedPeople = null;
                                lnqAferServiceBill.ConfirmedDate = null;
                                lnqAferServiceBill.Remark = bill.Remark;

                                if (!DeleteAllList(ctx,lnqAferServiceBill.BillID,out error))
                                {
                                    return false;
                                }

                                if (!AddAllList(ctx,lnqAferServiceBill.BillID,listTable,out error))
                                {
                                    return false;
                                }

                                break;
                            case AfterServiceMakePartsBillStatus.等待主管审核:

                                if (lnqAferServiceBill.Status != AfterServiceMakePartsBillStatus.等待主管审核.ToString())
                                {
                                    error = "单据状态不符，请重新确认单据状态";
                                    return false;
                                }

                                lnqAferServiceBill.Status = "等待车间确认";
                                lnqAferServiceBill.Checker = BasicInfo.LoginName;
                                lnqAferServiceBill.CheckDate = ServerTime.Time;
                                break;
                            case AfterServiceMakePartsBillStatus.等待车间确认:

                                if (lnqAferServiceBill.Status != AfterServiceMakePartsBillStatus.等待车间确认.ToString())
                                {
                                    error = "单据状态不符，请重新确认单据状态";
                                    return false;
                                }

                                lnqAferServiceBill.Status = "已完成";
                                lnqAferServiceBill.ConfirmedDate = ServerTime.Time;
                                lnqAferServiceBill.ConfirmedPeople = BasicInfo.LoginName;
                                break;
                            default:
                                error = "单据状态不正确";
                                    return false;
                        }
                        break;
                    default:
                        error = "数据不唯一";
                        return false;
                }

                ctx.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 删除对应的一条单据的所有明细
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billID">单据号</param>
        /// <param name="error">出错时返回的错误信息</param>
        /// <returns>True  删除成功, False 删除失败</returns>
        private bool DeleteAllList(DepotManagementDataContext ctx, string billID, out string error)
        {
            error = null;

            try
            {
                var varData = from a in ctx.YX_AfterServiceMakePartsList
                              where a.BillID == billID
                              select a;

                ctx.YX_AfterServiceMakePartsList.DeleteAllOnSubmit(varData);
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 添加表中的明细信息
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billID">单据号</param>
        /// <param name="listTable">明细信息表</param>
        /// <param name="error">出错时返回的错误信息</param>
        /// <returns>True 成功添加明细，False 添加明细失败</returns>
        bool AddAllList(DepotManagementDataContext ctx, string billID, DataTable listTable, out string error)
        {
            error = null;

            try
            {
                for (int i = 0; i < listTable.Rows.Count; i++)
                {
                    YX_AfterServiceMakePartsList lnqList = new YX_AfterServiceMakePartsList();

                    lnqList.BillID = billID;
                    lnqList.GoodsID = Convert.ToInt32(listTable.Rows[i]["物品ID"]);
                    lnqList.ProductCode = listTable.Rows[i]["总成型号"].ToString();
                    lnqList.Remark = listTable.Rows[i]["备注"].ToString();
                    lnqList.RequestCount = Convert.ToDecimal(listTable.Rows[i]["数量"]);

                    ctx.YX_AfterServiceMakePartsList.InsertOnSubmit(lnqList);
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
        /// 插入新单据
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="bill">要插入的单据主表信息</param>
        /// <param name="error">出错时返回的错误信息</param>
        /// <returns>True 添加成功 False 添加失败</returns>
        bool InsertBill(DepotManagementDataContext ctx, YX_AfterServiceMakePartsBill bill, out string error)
        {
            error = null;

            try
            {
                ctx.YX_AfterServiceMakePartsBill.InsertOnSubmit(bill);
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;                
            }
        }

        /// <summary>
        /// 自动生成领料单
        /// </summary>
        /// <param name="billID">售后服务配件申请单单号且为生成后的领料单的关联单据</param>
        /// <param name="storageID">库房ID</param>
        /// <param name="error">失败时返回的错误信息</param>
        /// <returns>True  自动生成成功，False 自动生成失败</returns>
        public bool AutogenerationMaterialRequisition(string billID,string storageID, 
            out string error)
        {
            error = null;
            string strMaterialBillID = "";
            string strOutMessage = "";

            try
            {
                DataTable dtListOfStraogeID = GetListStorageID(billID);

                if (dtListOfStraogeID.Rows.Count == 0)
                {
                    throw new Exception("此物品无【领料清单】相关信息，无法生成领料单");
                }
                else
                {
                    DataTable tempGoodsTable = GetSumRequestCount(billID);

                    for (int k = 0; k < dtListOfStraogeID.Rows.Count; k++)
                    {
                        MaterialRequisitionServer serverMaterialBill = new MaterialRequisitionServer();

                        DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                        strMaterialBillID = m_Assignbill.AssignNewNo(serverMaterialBill, CE_BillTypeEnum.领料单.ToString());

                        S_MaterialRequisition lnqMaterialBill = new S_MaterialRequisition();

                        lnqMaterialBill.AssociatedBillNo = "";
                        lnqMaterialBill.AssociatedBillType = "";
                        lnqMaterialBill.Bill_ID = strMaterialBillID;
                        lnqMaterialBill.Bill_Time = ServerTime.Time;
                        lnqMaterialBill.BillStatus = "新建单据";
                        lnqMaterialBill.Department = m_serverDepartment.GetDeptInfoFromPersonnelInfo(
                            BasicInfo.LoginName).Rows[0]["DepartmentCode"].ToString();
                        lnqMaterialBill.DepartmentDirector = "";
                        lnqMaterialBill.DepotManager = "";
                        lnqMaterialBill.FetchCount = 0;
                        lnqMaterialBill.FetchType = "零星领料";
                        lnqMaterialBill.FillInPersonnel = BasicInfo.LoginName;
                        lnqMaterialBill.FillInPersonnelCode = BasicInfo.LoginID;
                        lnqMaterialBill.ProductType = "";
                        lnqMaterialBill.PurposeCode = UniversalFunction.GetPurpose(CE_PickingPurposeProperty.三包外装配).Code;
                        lnqMaterialBill.Remark = "由系统自动生成  售后服务配件制造申请单【" + billID + "】";
                        lnqMaterialBill.StorageID = dtListOfStraogeID.Rows[k][0].ToString();

                        int rowCount = tempGoodsTable.Rows.Count;

                        if (!InsertMaterialRequisitionList(ctx, ref tempGoodsTable, lnqMaterialBill.StorageID,
                            strMaterialBillID, out error))
                        {
                            m_Assignbill.CancelBillNo("领料单", strMaterialBillID);
                            return false;
                        }

                        if (error != null && error.Contains("【图号型号】"))
                        {
                            strOutMessage = "【领料单号】：" + strMaterialBillID + "\r\n" + error; 
                        }

                        //tempGoodsTable = GlobalObject.DataSetHelper.SiftDataTable(tempGoodsTable, "RequestCount > 0", out error);

                        //if (rowCount == tempGoodsTable.Rows.Count)
                        //{
                        //    continue;
                        //}

                        if (!serverMaterialBill.AutoCreateBill(ctx, lnqMaterialBill,out error))
                        {
                            throw new Exception(error);
                        }
                        //ctx.S_MaterialRequisition.InsertOnSubmit(lnqMaterialBill);

                        ctx.SubmitChanges();
                    }
                }

                error = strOutMessage;
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                m_Assignbill.CancelBillNo("领料单", strMaterialBillID);
                return false;
            }
        }

        /// <summary>
        /// 插入领料单明细信息
        /// </summary>
        /// <param name="ctx">LINQ数据上下文</param>
        /// <param name="goodsTable">售后服务配件申请单分解的零件清单</param>
        /// <param name="storageID">库房ID</param>
        /// <param name="materialBillID">领料单单号</param>
        /// <param name="error">失败时返回的错误信息</param>
        /// <returns>返回TRUE表示成功,返回FALSE表示插入失败</returns>
        private bool InsertMaterialRequisitionList(DepotManagementDataContext ctx, ref DataTable goodsTable, 
            string storageID, string materialBillID, out string error)
        {
            error = null;
            string strOutMessage = "";
            try
            {
                for (int i = 0; i < goodsTable.Rows.Count; i++)
                {
                    List<View_S_Stock> lstStock = m_storeServer.GetGoodsStoreOnlyForSBW(goodsTable.Rows[i]["GoodsCode"].ToString(), 
                        goodsTable.Rows[i]["GoodsName"].ToString(),
                        goodsTable.Rows[i]["Spec"].ToString(), storageID).ToList();

                    if (lstStock.Count > 0 )
                    {
                        foreach (View_S_Stock item in lstStock)
                        {
                            S_MaterialRequisitionGoods lnqMaterialList = new S_MaterialRequisitionGoods();

                            lnqMaterialList.BasicCount = 0;
                            lnqMaterialList.BatchNo = item.批次号;
                            lnqMaterialList.Bill_ID = materialBillID;
                            lnqMaterialList.GoodsID = item.物品ID;
                            lnqMaterialList.ProviderCode = item.供货单位;

                            if (Convert.ToDecimal(goodsTable.Rows[i]["RequestCount"]) > item.库存数量)
                            {
                                lnqMaterialList.RequestCount = Convert.ToDecimal(goodsTable.Rows[i]["RequestCount"]);
                                lnqMaterialList.RealCount = item.库存数量;
                            }
                            else
                            {
                                lnqMaterialList.RequestCount = Convert.ToDecimal(goodsTable.Rows[i]["RequestCount"]);
                                lnqMaterialList.RealCount = Convert.ToDecimal(goodsTable.Rows[i]["RequestCount"]);
                            }

                            lnqMaterialList.Remark = "";
                            lnqMaterialList.ShowPosition = 1;

                            MaterialRequisitionGoodsServer serverMaterialGoods = new MaterialRequisitionGoodsServer();

                            if (!serverMaterialGoods.AutoCreateGoods(ctx, lnqMaterialList, out error))
                            {
                                throw new Exception(error);
                            }

                            goodsTable.Rows[i]["RequestCount"] = 
                                Convert.ToDecimal(goodsTable.Rows[i]["RequestCount"]) - lnqMaterialList.RealCount;

                            if (Convert.ToDecimal(goodsTable.Rows[i]["RequestCount"]) == 0)
                            {
                                break;
                            }
                        }
                    }

                    if (Convert.ToDecimal(goodsTable.Rows[i]["RequestCount"]) > 0)
                    {
                        strOutMessage = strOutMessage + "【图号型号】:" + goodsTable.Rows[i]["GoodsCode"].ToString()
                            + " 【物品名称】:" + goodsTable.Rows[i]["GoodsName"].ToString()
                            + " 【规格】:" + goodsTable.Rows[i]["Spec"].ToString()
                            + " 【数量】:" + goodsTable.Rows[i]["RequestCount"].ToString() + "\r\n";
                    }
                }

                error = strOutMessage;
                return true;

            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 根据BOM拆分售后服务配件申请单中的总成获得
        /// 各个零部件并且汇总申请单中的数量
        /// </summary>
        /// <param name="billID">申请单号</param>
        /// <returns>返回数据集或者NULL</returns>
        private DataTable GetSumRequestCount(string billID)
        {
            string error = "";

            Hashtable htParamTable = new Hashtable();

            htParamTable.Add("@billID",billID);

            return GlobalObject.DatabaseServer.QueryInfoPro("YX_AfterService_GetSumRequestCount", 
                htParamTable, out error);

        }

        /// <summary>
        /// 根据BOM拆分售后服务配件申请单中的总成获得
        /// 各个零部件并且汇总申请单中的所属库房
        /// </summary>
        /// <param name="billID">申请单号</param>
        /// <returns>返回数据集或者NULL</returns>
        private DataTable GetListStorageID(string billID)
        {
            string error = "";

            Hashtable htParamTable = new Hashtable();

            htParamTable.Add("@billID", billID);

            return GlobalObject.DatabaseServer.QueryInfoPro("YX_AfterService_GetListStorageID",
                htParamTable, out error);

        }
    }
}
