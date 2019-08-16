/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  ProductDeliveryInspectionServer.cs
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
using ServerModule;
using GlobalObject;

namespace ServerModule
{
    /// <summary>
    /// CVT出厂检验记录管理类
    /// </summary>
    class ProductDeliveryInspectionServer : BasicServer, IProductDeliveryInspectionServer
    {
        /// <summary>
        /// 单据编号分配服务
        /// </summary>
        IAssignBillNoServer m_assignBill = ServerModuleFactory.GetServerModule<IAssignBillNoServer>();

        public P_TechnicalRequirements GetTechnicalRequirementsInfo(int technicalID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.P_TechnicalRequirements
                          where a.TechnicalID == technicalID
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

        public P_AllAccreditedTestingItems GetAllAccreditedTestingItemsInfo(int testID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.P_AllAccreditedTestingItems
                          where a.TestID == testID
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
        /// 获得明细项目ID
        /// </summary>
        /// <param name="name">明细项目名称</param>
        /// <param name="productType">产品类型</param>
        /// <returns>获取成功返回明细项目ID，获取失败返回0</returns>
        public int GetTechnicalRequirementsID(string name, string productType)
        {
            string strSql = " select b.TechnicalID P_TechnicalRequirements as b on a.TestID = b.TestID " +
                            " inner join P_TechnicalRequirements_Product as c on b.TechnicalID = c.TechnicalID " +
                            " where TechnicalRequirementsName = '" + name + "' and c.ProductType = '" + productType + "'";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dtTemp.Rows.Count == 0)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(dtTemp.Rows[0]["TechnicalID"]);
            }
        }

        /// <summary>
        /// 获得项目ID
        /// </summary>
        /// <param name="name">项目名称</param>
        /// <param name="productType">产品类型</param>
        /// <returns>获取成功返回项目ID，获取失败返回0</returns>
        public int GetTestItemNameID(string name, string productType)
        {
            string strSql = " select a.TestID from P_AllAccreditedTestingItems as a "+
                            " inner join  P_TechnicalRequirements as b on a.TestID = b.TestID "+
                            " inner join P_TechnicalRequirements_Product as c on b.TechnicalID = c.TechnicalID " +
                            " where a.TestItemName = '" + name + "' and c.ProductType = '" + productType + "'";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dtTemp.Rows.Count == 0)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(dtTemp.Rows[0]["TestID"]);
            }
        }

        /// <summary>
        /// 获得明细项目集合
        /// </summary>
        /// <param name="name">项目名称</param>
        /// <param name="productType">产品型号</param>
        /// <returns>返回获取的明细项目集合</returns>
        public DataTable GetTechnicalRequirements(string name, string productType)
        {
            string strSql = "select a.TestID as 检测项目ID, a.TestItemName as 检测项目, b.TechnicalID as 技术要求ID, " +
                " b.TechnicalRequirementsName as 技术要求,'' as 检测情况,'' as 判定,'' as 单据号 " +
                " from P_AllAccreditedTestingItems as a  " +
                " inner join P_TechnicalRequirements as b on a.TestID = b.TestID" +
                " inner join P_TechnicalRequirements_Product as c on b.TechnicalID = c.TechnicalID " +
                " where TestItemName = '" + name + "' and c.ProductType = '" + productType + "'";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);

        }

        /// <summary>
        /// 获得明细项目集合
        /// </summary>
        /// <param name="technicalName">明细项目名称</param>
        /// <param name="productType">产品类型</param>
        /// <returns>返回明细项目与主项目关联后的数据集合</returns>
        public DataTable GetAllTechnical(string technicalName, string productType)
        {
            string strSql = "select a.TestID as 检测项目ID, a.TestItemName as 检测项目, b.TechnicalID as 技术要求ID, " +
                " b.TechnicalRequirementsName as 技术要求,'' as 检测情况,'' as 判定,'' as 单据号 " +
                " from P_AllAccreditedTestingItems as a  " +
                " inner join P_TechnicalRequirements as b on a.TestID = b.TestID" +
                " inner join P_TechnicalRequirements_Product as c on b.TechnicalID = c.TechnicalID " +
                " where TechnicalRequirementsName =  '" + technicalName + "' and c.ProductType = '" + productType + "'";


            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        ///// <summary>
        ///// 显示空表格(2013-12-04之前的数据，邱瑶改)
        ///// </summary>
        ///// <returns>返回CVT出厂检验空表格</returns>
        //public DataTable GetEmptyTable()
        //{
        //    string strSql = "select a.TestID as 检测项目ID,TestItemName as 检测项目 ,TechnicalID as 技术要求ID, " +
        //        " TechnicalRequirementsName as 技术要求,'' as 检测情况,'' as 判定,'' as 单据号 " +
        //        " from P_AllAccreditedTestingItems as a  " +
        //        " inner join P_TechnicalRequirements as b on a.TestID = b.TestID where productType is null";

        //    return GlobalObject.DatabaseServer.QueryInfo(strSql);
        //}

        /// <summary>
        /// 显示空表格(2013-12-04之后的数据，邱瑶改)
        /// </summary>
        /// <param name="productType">产品型号</param>
        /// <returns>返回CVT出厂检验空表格</returns>
        public DataTable GetEmptyTable(string productType)
        {
            string strSql = "select TestItemName as 检测项目 ,b.TechnicalID as 技术要求ID, " +
                " TechnicalRequirementsName as 技术要求,'' as 检测情况,'' as 判定,'' as 单据号 " +
                " from P_AllAccreditedTestingItems as a  " +
                " inner join P_TechnicalRequirements as b on a.TestID = b.TestID "+
                " inner join P_TechnicalRequirements_Product as c on b.TechnicalID = c.TechnicalID "+
                " where c.ProductType = '" + productType + "'";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得明细信息
        /// </summary>
        /// <param name="billID">检验记录表单号</param>
        /// <returns>返回CVT出厂检验记录表的明细信息</returns>
        public DataTable GetListInfo(string billID)
        {
            string strSql = "select * from View_P_DeliveryInspectionItems where 单据号 = '" + billID + "'";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得某一条BILL信息
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <returns>返回数据集</returns>
        public P_DeliveryInspection GetBill(string billID)
        {
            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.P_DeliveryInspection
                              where a.DJH == billID
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
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 检查某业务是否存在
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">业务号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.P_DeliveryInspection
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
            string strSql = "SELECT * FROM [DepotManagement].[dbo].[P_DeliveryInspection] where DJH = '" + billNo + "'";

            System.Data.DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dtTemp != null && dtTemp.Rows.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 删除单据信息
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        public bool DeleteBill(string billID, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.P_DeliveryInspection
                              where a.DJH == billID
                              select a;

                dataContext.P_DeliveryInspection.DeleteAllOnSubmit(varData);

                if (!DeleteCVTList(dataContext, billID, out error))
                {
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

        ///// <summary>
        ///// 添加检测项目明细信息
        ///// </summary>
        ///// <param name="context">数据上下文</param>
        ///// <param name="billID">检验记录表单号</param>
        ///// <param name="listTable">明细信息</param>
        ///// <param name="error">出错时返回错误信息，无错时返回null</param>
        ///// <returns>添加成功返回True，添加失败返回False</returns>
        //private bool AddCVTList(DepotManagementDataContext context, string billID,
        //    DataTable listTable, out string error)
        //{
        //    error = null;

        //    try
        //    {
        //        for (int i = 0; i < listTable.Rows.Count; i++)
        //        {
        //            P_DeliveryInspectionItems lnqDelivery = new P_DeliveryInspectionItems();

        //            lnqDelivery.DetectionResult = listTable.Rows[i]["检测情况"].ToString();
        //            lnqDelivery.Judge = listTable.Rows[i]["判定"].ToString();
        //            lnqDelivery.TechnicalID = Convert.ToInt32(listTable.Rows[i]["技术要求ID"]);
        //            lnqDelivery.DJH = billID;

        //            context.P_DeliveryInspectionItems.InsertOnSubmit(lnqDelivery);
        //        }

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        error = ex.Message;
        //        return false;
        //    }
        //}

        /// <summary>
        /// 删除明细信息
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="billID">检验记录表的单号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        private bool DeleteCVTList(DepotManagementDataContext context, string billID,
            out string error)
        {
            error = null;

            try
            {
                var varData = from a in context.P_DeliveryInspectionItems
                              where a.DJH == billID
                              select a;

                context.P_DeliveryInspectionItems.DeleteAllOnSubmit(varData);

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 获得CVT出厂检验记录
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="djzt">单据状态</param>
        /// <returns>返回CVT出厂检验记录集</returns>
        public DataTable GetDeliveryInspection(DateTime startTime, DateTime endTime, string djzt)
        {
            string strSql = "select * from View_P_DeliveryInspection where 检验日期 >= '"
                + startTime + "' and 检验日期 <= '" + endTime + "'";

            if (djzt != "全  部")
            {
                strSql += " and 单据状态 = '" + djzt + "'";
            }

            strSql += " order by 单据号 desc";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 最终判定
        /// </summary>
        /// <param name="delivery">CVT检验报告信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True ，操作失败返回False</returns>
        public bool FinalJudgeBill(P_DeliveryInspection delivery, out string error)
        {
            error = null;

            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;
            dataContext.Connection.Open();
            dataContext.Transaction = dataContext.Connection.BeginTransaction();

            try
            {
                ProductListServer serverProductList = new ProductListServer();

                var varData = from a in dataContext.P_DeliveryInspection
                              where a.DJH == delivery.DJH
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数据为空或者不唯一";
                    return false;
                }
                else
                {
                    P_DeliveryInspection lnqUpdate = varData.Single();

                    lnqUpdate.FinalVerdict = delivery.FinalVerdict;
                    lnqUpdate.FinalDate = ServerTime.Time;
                    lnqUpdate.FinalPersonnel = BasicInfo.LoginName;
                    lnqUpdate.DJZT = "已完成";
                    lnqUpdate.Remark = delivery.Remark;

                    //不合格处理情况

                    if (lnqUpdate.FinalVerdict == "不合格")
                    {
                        //删除产品业务表中的记录

                        var varMarketingDate = from a in dataContext.ProductsCodes
                                               where a.DJH == lnqUpdate.AssociatedBillNo
                                               && a.ProductCode == lnqUpdate.ProductCode
                                               select a;

                        dataContext.ProductsCodes.DeleteAllOnSubmit(varMarketingDate);

                        //获得单据ID和产品ID
                        int intProductID = serverProductList.GetProductGoodsID(lnqUpdate.ProductType, 0, false);

                        int intDJID = 0;
                        var varBillDate = from a in dataContext.S_MarketingBill
                                          where a.DJH == lnqUpdate.AssociatedBillNo
                                          select a;

                        if (varBillDate.Count() != 1)
                        {
                            error = "数据不唯一或者为空";
                            return false;
                        }
                        else
                        {
                            intDJID = varBillDate.Single().ID;
                        }

                        //修改营销业务明细表中的记录的数量数据
                        var varListDate = from a in dataContext.S_MarketingList
                                          where a.DJ_ID == intDJID
                                          && a.CPID == intProductID.ToString()
                                          select a;

                        if (varListDate.Count() != 1)
                        {
                            error = "数据不唯一或者为空";
                            return false;
                        }
                        else
                        {
                            S_MarketingList lnqList = varListDate.Single();

                            lnqList.Count = lnqList.Count - 1;
                        }
                    }

                    dataContext.SubmitChanges();

                    //修改营销业务单据状态

                    if (!UpdateMarketingBill(dataContext, delivery.AssociatedBillNo, out error))
                    {
                        return false;
                    }
                }

                dataContext.Transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                dataContext.Transaction.Rollback();
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 更新出厂检验记录
        /// </summary>
        /// <param name="delivery">出厂检验记录表信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>更新成功返回True，更新失败返回False</returns>
        public bool UpdateDeliveryInspection(P_DeliveryInspection delivery, out string error)
        {
            error = null;

            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;
            dataContext.Connection.Open();
            dataContext.Transaction = dataContext.Connection.BeginTransaction();

            try
            {
                var varData = from a in dataContext.P_DeliveryInspection
                              where a.DJH == delivery.DJH
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数据为空或者不唯一";
                    return false;
                }
                else
                {
                    P_DeliveryInspection lnqDeliveryInspection = varData.Single();

                    lnqDeliveryInspection.Verdict = delivery.Verdict;
                    lnqDeliveryInspection.Date = ServerTime.Time;
                    lnqDeliveryInspection.Surveyor = BasicInfo.LoginName;
                    lnqDeliveryInspection.Remark = delivery.Remark;

                    if (delivery.Verdict == "不合格")
                    {
                        lnqDeliveryInspection.DisqualificationCase = delivery.DisqualificationCase;
                        lnqDeliveryInspection.DisqualificationItem = delivery.DisqualificationItem;
                        lnqDeliveryInspection.DJZT = "等待最终判定";

                        var varList = from a in dataContext.P_DeliveryInspectionItems
                                      where a.DJH == lnqDeliveryInspection.DJH
                                      select a;

                        foreach (var item in varList)
                        {
                            P_DeliveryInspectionItems lnqList = item;

                            if (lnqList.TechnicalID == Convert.ToInt32(lnqDeliveryInspection.DisqualificationItem))
                            {
                                lnqList.Judge = "不合格";
                                lnqList.DetectionResult = delivery.DisqualificationCase;
                            }
                            else
                            {
                                lnqList.Judge = "合格";
                            }
                        }
                    }
                    else
                    {
                        lnqDeliveryInspection.DJZT = "已完成";

                        var varList = from a in dataContext.P_DeliveryInspectionItems
                                      where a.DJH == lnqDeliveryInspection.DJH
                                      select a;

                        foreach (var item in varList)
                        {
                            P_DeliveryInspectionItems lnqList = item;

                            lnqList.Judge = "合格";

                        }
                    }

                    dataContext.SubmitChanges();

                    //修改营销业务单据状态

                    if (!UpdateMarketingBill(dataContext, lnqDeliveryInspection.AssociatedBillNo, out error))
                    {
                        return false;
                    }
                }

                dataContext.Transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                dataContext.Transaction.Rollback();
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 更新营销入库单据状态
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="djh">营销入库单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>更新成功返回True，更新失败返回False</returns>
        private bool UpdateMarketingBill(DepotManagementDataContext dataContext, string djh, out string error)
        {
            error = null;

            try
            {
                var varData = from a in dataContext.P_DeliveryInspection
                              where a.AssociatedBillNo == djh
                              && a.DJZT != "已完成"
                              select a;

                if (varData.Count() == 0)
                {
                    var varMarketing = from a in dataContext.S_MarketingBill
                                       where a.DJH == djh
                                       select a;

                    if (varMarketing.Count() != 1)
                    {
                        error = "数据不唯一或者为空";
                        return false;
                    }
                    else
                    {
                        if (varMarketing.Single().DJZT_FLAG != "已审核")
                        {
                            error = "单据状态不正确，不能进行操作";
                            return false;
                        }

                        varMarketing.Single().DJZT_FLAG = "已检验";
                        varMarketing.Single().CheckDate = ServerTime.Time;
                        varMarketing.Single().JYRY = BasicInfo.LoginID;

                        dataContext.SubmitChanges();
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
        /// 处理自动生成CVT出厂检验记录表
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="djh">营销入库单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>自动生成成功返回True，自动生成失败返回False</returns>
        public bool ManageDeliveryInspection(DepotManagementDataContext ctx, string djh, out string error)
        {
            error = null;

            try
            {
                if (!DeleteDeliveryInspection(ctx, djh, out error))
                {
                    return false;
                }
                else
                {
                    if (!AddDeliveryInspection(ctx, djh, out error))
                    {
                        return false;
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
        /// 添加CVT出厂检验记录
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="djh">营销入库单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>添加成功返回True，添加失败返回False</returns>
        private bool AddDeliveryInspection(DepotManagementDataContext dataContext, string djh, out string error)
        {
            error = null;

            try
            {
                var varData = from a in dataContext.ProductsCodes
                              where a.DJH == djh
                              select a;

                foreach (var item in varData)
                {
                    object goodsAttirbute = UniversalFunction.GetGoodsAttributeInfo(dataContext, item.GoodsID, CE_GoodsAttributeName.CVT);

                    if (goodsAttirbute == null || !Convert.ToBoolean(goodsAttirbute))
                    {
                        continue;
                    }

                    string strDJH = m_assignBill.AssignNewNo(dataContext, this, CE_BillTypeEnum.CVT出厂检验记录表.ToString());

                    P_DeliveryInspection lnqDelivery = new P_DeliveryInspection();

                    lnqDelivery.AssociatedBillNo = djh;
                    lnqDelivery.DJH = strDJH;
                    lnqDelivery.DJZT = "等待检验";
                    lnqDelivery.Date = ServerTime.Time;
                    lnqDelivery.ProductCode = item.ProductCode;
                    lnqDelivery.ProductType = item.GoodsCode;

                    dataContext.P_DeliveryInspection.InsertOnSubmit(lnqDelivery);

                    DataTable dtNullList = GetEmptyTable(item.GoodsCode);

                    for (int i = 0; i < dtNullList.Rows.Count; i++)
                    {
                        P_DeliveryInspectionItems lnqItem = new P_DeliveryInspectionItems();

                        lnqItem.DJH = strDJH;
                        lnqItem.TechnicalID = Convert.ToInt32(dtNullList.Rows[i]["技术要求ID"]);
                        lnqItem.TestItemName = dtNullList.Rows[i]["检测项目"].ToString();
                        lnqItem.TechnicalRequirementsName = dtNullList.Rows[i]["技术要求"].ToString();

                        dataContext.P_DeliveryInspectionItems.InsertOnSubmit(lnqItem);
                    }
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
        /// 删除所关联的所有CVT出厂检验记录
        /// </summary>
        /// <param name="djh">关联单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        public bool DeleteDeliveryInspection(string djh, out string error)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            return DeleteDeliveryInspection(ctx, djh, out error);
        }
        /// <summary>
        /// 删除所关联的所有CVT出厂检验记录
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="djh">关联单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        public bool DeleteDeliveryInspection(DepotManagementDataContext dataContext, string djh, out string error)
        {
            error = null;

            try
            {
                var varData = from a in dataContext.P_DeliveryInspection
                              where a.AssociatedBillNo == djh
                              select a;

                foreach (var item in varData)
                {
                    var varList = from a in dataContext.P_DeliveryInspectionItems
                                  where a.DJH == item.DJH
                                  select a;

                    dataContext.P_DeliveryInspectionItems.DeleteAllOnSubmit(varList);

                    m_assignBill.CancelBillNo(dataContext, CE_BillTypeEnum.CVT出厂检验记录表.ToString(), item.DJH);

                    dataContext.P_DeliveryInspection.DeleteOnSubmit(item);
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
        /// 检测此单据的单据状态是否为“已检验”
        /// </summary>
        /// <param name="billID">营销单据号</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>已检验返回True，未检验返回False</returns>
        public bool IsExamine(string billID, out string storageID)
        {
            storageID = null;

            string strSql = "select * from S_MarketingBill where DJH = '" + billID + "'";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dtTemp.Rows.Count == 0)
            {
                return false;
            }
            else
            {
                if (dtTemp.Rows[0]["DJZT_FLAG"].ToString() == "已检验")
                {
                    storageID = dtTemp.Rows[0]["StorageID"].ToString();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 添加CVT终检记录
        /// </summary>
        /// <param name="lnqFinalInfo">CVT终检信息</param>
        public void AddCVTFinalInspectionInfo(ZL_CVTFinalInspection lnqFinalInfo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            lnqFinalInfo.WorkID = BasicInfo.LoginID;

            ctx.ZL_CVTFinalInspection.InsertOnSubmit(lnqFinalInfo);
            ctx.SubmitChanges();
        }

        /// <summary>
        /// 删除CVT终检记录
        /// </summary>
        /// <param name="startTime">开始日期</param>
        /// <param name="endTime">结束日期</param>
        /// <param name="lnqFinalInfo">CVT终检信息</param>
        public void DeleteCVTFinalInspectionInfo(DateTime startTime, DateTime endTime, ZL_CVTFinalInspection lnqFinalInfo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.ZL_CVTFinalInspection
                          where a.SelectDate >= startTime && a.SelectDate <= endTime && a.WorkID == lnqFinalInfo.WorkID
                          && a.ProductCode == lnqFinalInfo.ProductCode && a.ProductType == lnqFinalInfo.ProductType
                          select a;

            ctx.ZL_CVTFinalInspection.DeleteAllOnSubmit(varData);
            ctx.SubmitChanges();
        }

        /// <summary>
        /// CVT终检信息记录查询
        /// </summary>
        /// <param name="startTime">开始日期</param>
        /// <param name="endTime">结束日期</param>
        /// <returns>返回Table</returns>
        public DataTable SelectFinalInspectionList(DateTime? startTime, DateTime? endTime)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            DataTable result = new DataTable();

            result.Columns.Add("型号");
            result.Columns.Add("箱号");
            result.Columns.Add("下线试验信息");
            result.Columns.Add("审核时间");
            result.Columns.Add("称重信息");
            result.Columns.Add("称重时间");
            result.Columns.Add("气密性信息");
            result.Columns.Add("检测时间");
            result.Columns.Add("终检人");

            DataTable tempTable1 = new DataTable();

            string strSql = "";

            if (startTime == null || endTime == null)
            {
                strSql = " select distinct ProductCode, ProductType, WorkID from (select * from ZL_CVTFinalInspection as a "+
                         " where a.SelectDate = (select MAX(SelectDate) from ZL_CVTFinalInspection as b "+
                         " where a.ProductCode = b.ProductCode)) as a "+
                         " where SelectDate >= DATEADD(day, -3, CONVERT(varchar(50), GETDATE(), 23)) and SelectDate <= "+
                         " DATEADD(day, 1, CONVERT(varchar(50), GETDATE(), 23)) and WorkID = '"+ BasicInfo.LoginID +"'";
            }
            else
            {
                strSql = " select distinct ProductNumber as ProductCode, ProductType, '" + BasicInfo.LoginID 
                    + "' as WorkID from (select * from ZPX_CVTWeight as a "
                    + " where Date = (select MAX(Date) from ZPX_CVTWeight as b "
                    + " where a.ProductNumber = b.ProductNumber)) as a where Date >= '"
                    + ((DateTime)startTime).ToShortDateString() + "' and Date <= '" 
                    + ((DateTime)endTime).ToShortDateString() + "'";
            }

            tempTable1 = GlobalObject.DatabaseServer.QueryInfo(strSql);

            foreach (DataRow dr in tempTable1.Rows)
            {
                string error;

                Hashtable hsTable = new Hashtable();

                hsTable.Add("@ProductType", dr["ProductType"].ToString());
                hsTable.Add("@ProductCode", dr["ProductCode"].ToString());

                DataTable tempTable =
                    GlobalObject.DatabaseServer.QueryInfoPro("CVTFinalInspection_Select", hsTable, out error);

                DataRow tempRow = result.NewRow();

                tempRow["型号"] = dr["ProductType"].ToString();
                tempRow["箱号"] = dr["ProductCode"].ToString();
                tempRow["终检人"] = UniversalFunction.GetPersonnelName(dr["WorkID"].ToString());

                if (tempTable == null || tempTable.Rows.Count == 0)
                {
                    tempRow["下线试验信息"] = "";
                    tempRow["审核时间"] = "";
                    tempRow["称重信息"] = "";
                    tempRow["称重时间"] = "";
                    tempRow["气密性信息"] = "";
                    tempRow["检测时间"] = "";
                }
                else
                {
                    tempRow["下线试验信息"] = tempTable.Rows[0]["下线试验信息"];
                    tempRow["审核时间"] = tempTable.Rows[0]["审核时间"];
                    tempRow["称重信息"] = tempTable.Rows[0]["称重信息"];
                    tempRow["称重时间"] = tempTable.Rows[0]["称重时间"];
                    tempRow["气密性信息"] = tempTable.Rows[0]["气密性信息"];
                    tempRow["检测时间"] = tempTable.Rows[0]["检测时间"];
                }

                result.Rows.Add(tempRow);
            }

            return result;
        }

    }
}
