/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  FrockStandingBook.cs
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
using System.Text.RegularExpressions;

namespace ServerModule
{
    /// <summary>
    /// 工装台帐服务类
    /// </summary>
    class FrockStandingBook : BasicServer, ServerModule.IFrockStandingBook
    {
        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 单据编号分配服务
        /// </summary>
        IAssignBillNoServer m_assignBill = BasicServerFactory.GetServerModule<IAssignBillNoServer>();

        /// <summary>
        /// 检查某业务是否存在
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">业务号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.S_FrockStandingBook
                          where a.FrockNumber == billNo
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
            string sql = "SELECT * FROM [DepotManagement].[dbo].[S_FrockStandingBook] where FrockNumber = '" + billNo + "'";

            System.Data.DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 更新工装工位配置信息
        /// </summary>
        /// <param name="oldFrockOfWorkBench">旧实体集</param>
        /// <param name="newFrockOfWorkBench">新实体集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool UpdateFrockOfWorkBench(S_FrockOfWorkBenchSetting oldFrockOfWorkBench,
            S_FrockOfWorkBenchSetting newFrockOfWorkBench, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.S_FrockOfWorkBenchSetting
                              where a.FrockNumber == oldFrockOfWorkBench.FrockNumber
                              && a.WorkBench == oldFrockOfWorkBench.WorkBench
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数据为空或者不唯一";
                    return false;
                }
                else
                {
                    S_FrockOfWorkBenchSetting lnqData = varData.Single();

                    lnqData.FrockNumber = newFrockOfWorkBench.FrockNumber;
                    lnqData.WorkBench = newFrockOfWorkBench.WorkBench;

                    ctx.SubmitChanges();
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
        /// 删除工装工位配置信息
        /// </summary>
        /// <param name="frockOfWorkBench">工装工位配置实体集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True ，失败返回False</returns>
        public bool DeleteFrockOfWorkBench(S_FrockOfWorkBenchSetting frockOfWorkBench, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;


                var varData = from a in ctx.S_FrockOfWorkBenchSetting
                              where a.WorkBench == frockOfWorkBench.WorkBench
                              && a.FrockNumber == frockOfWorkBench.FrockNumber
                              select a;

                ctx.S_FrockOfWorkBenchSetting.DeleteAllOnSubmit(varData);
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
        /// 添加工装工位配置信息
        /// </summary>
        /// <param name="frockOfWorkBench">工装工位配置实体集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True ，失败返回False</returns>
        public bool AddFrockOfWorkBench(S_FrockOfWorkBenchSetting frockOfWorkBench, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.S_FrockOfWorkBenchSetting
                              where a.WorkBench == frockOfWorkBench.WorkBench
                              && a.FrockNumber == frockOfWorkBench.FrockNumber
                              select a;

                if (varData.Count() == 0)
                {
                    ctx.S_FrockOfWorkBenchSetting.InsertOnSubmit(frockOfWorkBench);
                    ctx.SubmitChanges();
                }
                else
                {
                    error = "不能添加重复项";
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
        /// 获得工装的工位设置信息
        /// </summary>
        /// <returns>返回Table</returns>
        public DataTable GetFrockOfWorkBenchInfo()
        {
            string strSql = " select c.图号型号 as 工装图号,c.物品名称 as 工装名称, a.FrockNumber as 工装编号 ,WorkBench as 工位 " +
                            " from S_FrockOfWorkBenchSetting as a inner join S_FrockStandingBook as b on a.FrockNumber = b.FrockNumber " +
                            " inner join View_F_GoodsPlanCost as c on b.GoodsID = c.序号";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得周期鉴定项目
        /// </summary>
        /// <param name="frockNumber">工装编号</param>
        /// <returns>返回Table</returns>
        public DataTable GetCheckItemsContent(string frockNumber)
        {
            string strSql = "select ItemContent as 检测项目 from S_FrockStandingBookCheckItems " +
                " where FrockNumber = '" + frockNumber + "'";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        public void RecordFrockUseCounts_HomemadePart(DepotManagementDataContext ctx, int goodsID, int inStockCounts)
        {
            var varData = from a in ctx.S_FrockStandingBook
                          join b in ctx.S_FrockStandingBook_ApplicableGoods
                          on a.FrockNumber equals b.FrockNumber
                          where a.IsInStock == false && b.GoodsID == goodsID
                          && a.IdentifyCycleType == "计数"
                          && (a.ScarpBillID == null || a.ScarpBillID.Trim().Length == 0)
                          select a;

            if (varData.Count() > 0)
            {
                foreach (var item in varData)
                {
                    var varEnumeration = from a in ctx.S_FrockStandingBookEnumeration
                                         where a.FrockNumber == item.FrockNumber
                                         select a;

                    if (varEnumeration.Count() == 1)
                    {
                        varEnumeration.Single().Counts = varEnumeration.Single().Counts + inStockCounts;
                    }
                    else if(varEnumeration.Count() == 0)
                    {
                        S_FrockStandingBookEnumeration enumeration = new S_FrockStandingBookEnumeration();

                        enumeration.FrockNumber = item.FrockNumber;
                        enumeration.Counts = inStockCounts;

                        ctx.S_FrockStandingBookEnumeration.InsertOnSubmit(enumeration);
                    }
                    else
                    {
                        throw new Exception("数据有误");
                    }
                }
            }
        }

        public void SaveApplicableGoods(string frockNumber, DataTable tableInfo)
        {
            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.S_FrockStandingBook_ApplicableGoods
                              where a.FrockNumber == frockNumber
                              select a;

                ctx.S_FrockStandingBook_ApplicableGoods.DeleteAllOnSubmit(varData);

                foreach (DataRow dr in tableInfo.Rows)
                {
                    S_FrockStandingBook_ApplicableGoods tempInfo = new S_FrockStandingBook_ApplicableGoods();

                    tempInfo.FrockNumber = frockNumber;
                    tempInfo.GoodsID = Convert.ToInt32(dr["序号"]);

                    ctx.S_FrockStandingBook_ApplicableGoods.InsertOnSubmit(tempInfo);
                }

                ctx.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 保存周期鉴定项目
        /// </summary>
        /// <param name="frockNumber">工装编号</param>
        /// <param name="checkContentItems">字符串列表</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        public bool SaveCheckItemContent(string frockNumber, List<string> checkContentItems, out string error)
        {
            error = null;

            if (checkContentItems == null || frockNumber == null)
            {
                error = "数据有误";
                return false;
            }

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.S_FrockStandingBookCheckItems
                          where a.FrockNumber == frockNumber
                          select a;

            ctx.S_FrockStandingBookCheckItems.DeleteAllOnSubmit(varData);


            foreach (string str in checkContentItems)
            {
                S_FrockStandingBookCheckItems item = new S_FrockStandingBookCheckItems();

                item.FrockNumber = frockNumber;
                item.ItemContent = str;

                ctx.S_FrockStandingBookCheckItems.InsertOnSubmit(item);
            }

            ctx.SubmitChanges();

            return true;
        }

        /// <summary>
        /// 改变子父关系
        /// </summary>
        /// <param name="lnqSelfFrock">自身工装信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool ChangeParentChildRelationships(S_FrockStandingBook lnqSelfFrock, out string error)
        {
            error = null;

            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            var varData = from a in dataContext.S_FrockStandingBook
                          where a.GoodsID == lnqSelfFrock.GoodsID
                          && a.FrockNumber == lnqSelfFrock.FrockNumber
                          select a;

            if (varData.Count() != 1)
            {
                error = "数据不唯一或者为空";
                return false;
            }
            else
            {
                S_FrockStandingBook lnqFrock = varData.Single();

                lnqFrock.ParentFrockNumber = lnqSelfFrock.ParentFrockNumber;
                lnqFrock.ParentGoodsID = lnqSelfFrock.ParentGoodsID;

                dataContext.SubmitChanges();

                return true;
            }
        }

        /// <summary>
        /// 获得工装入库单信息
        /// </summary>
        /// <param name="frockNumber">工装编号</param>
        /// <returns>返回DataRow</returns>
        public DataRow GetInDepotBillInfo(string frockNumber)
        {
            string strSql = " select FrockNumber 工装编号,c.ProviderCode 供应商编码,c.ShortName 供应商简称, b.Bill_ID as 关联单号, " +
                            " dbo.fun_get_Name(Proposer) as 申请人, Proposer as 申请人工号, dbo.fun_get_Name(Designer) as 设计人, Designer as 设计人工号  " +
                            " from S_FrockProvingReport as a inner join (select Provider, Bill_ID, Proposer, Designer from S_OrdinaryInDepotBill " +
                            " union all select Provider, Bill_ID, ProposerID, DesignerID from S_FrockInDepotBill) as b  " +
                            " on b.Bill_ID = a.ConnectBillNumber inner join Provider as c on b.Provider = c.ProviderCode " +
                            " where FrockNumber = '" + frockNumber + "'";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dtTemp.Rows.Count != 1)
            {
                return null;
            }
            else
            {
                return dtTemp.Rows[0];
            }
        }

        /// <summary>
        /// 检测工装附属信息是否填写完整
        /// </summary>
        /// <param name="frockNumber">工装编号</param>
        /// <param name="goodsID">物品ID</param>
        /// <returns>不存在数据或者完整返回True，不完整或者数据不唯一返回False</returns>
        public bool IsIntactSatelliteInformation(string frockNumber, int goodsID)
        {
            string strSql = "select * from S_FrockStandingBook where FrockNumber = '"
                + frockNumber + "' and GoodsID = " + goodsID;

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dt.Rows.Count == 0)
            {
                return true;
            }
            else if (dt.Rows.Count > 1)
            {
                return false;
            }
            else
            {
                DataRow dr = dt.Rows[0];

                if (dr["ApplyToProductCode"].ToString().Trim() == ""
                    || dr["ApplyToProductName"].ToString().Trim() == ""
                    || dr["ApplyToDevice"].ToString().Trim() == ""
                    || dr["ApplyToProcess"].ToString().Trim() == ""
                    || dr["ApplyToWorkShop"].ToString().Trim() == "")
                {
                    return false;
                }

                strSql = "select * from S_FrockStandingBookCheckItems where FrockNumber = '" + frockNumber + "'";

                DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

                if (dtTemp.Rows.Count == 0)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 检查单据数与工装编号业务数是否一致
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="billNo">单据号</param>
        /// <param name="amount">单据的物品数量</param>
        /// <returns>true 一致 false 不一致</returns>
        public bool IsOperationCountMateBillCount(int goodsID, string billNo, decimal amount)
        {
            string strSql = "select Count(*) from S_FrockOperation where GoodsID = "
                + goodsID + " and BillID = '" + billNo + "'";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dtTemp.Rows.Count == 0 || dtTemp.Rows[0][0].ToString() == "")
            {
                return false;
            }
            else
            {
                if (Convert.ToDecimal(dtTemp.Rows[0][0]) == amount)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 批量插入工装业务表
        /// </summary>
        /// <param name="billNo">业务单据号</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="frockNumberTable">单据业务的工装编码数据集</param>
        /// <param name="businessType">业务类型</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool UpdateFrockOperation(string billNo, int goodsID, DataTable frockNumberTable, CE_BusinessBillType businessType, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.S_FrockOperation
                              where a.BillID == billNo
                              && a.GoodsID == goodsID
                              select a;

                ctx.S_FrockOperation.DeleteAllOnSubmit(varData);

                for (int i = 0; i < frockNumberTable.Rows.Count; i++)
                {
                    S_FrockOperation lnqOperation = new S_FrockOperation();

                    lnqOperation.BillID = billNo;
                    lnqOperation.BillTime = ServerTime.Time;
                    lnqOperation.BillType = businessType.ToString();
                    lnqOperation.FrockNumber = frockNumberTable.Rows[i]["FrockNumber"].ToString();
                    lnqOperation.GoodsID = goodsID;
                    lnqOperation.IsTrue = false;

                    ctx.S_FrockOperation.InsertOnSubmit(lnqOperation);
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
        /// 根据单据号获得某个物品的工装编号数据集
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回一条工装台帐的数据集</returns>
        public DataTable GetFrockNumberFromBillNo(string billNo, int goodsID)
        {
            string strSql = "select FrockNumber from S_FrockOperation where BillID = '" + billNo + "' and GoodsID = " + goodsID;

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 更新工装台帐的库存状态
        /// </summary>
        /// <param name="ctx">LINQ数据上下文</param>
        /// <param name="frockTable">需要更新的数据表</param>
        /// <param name="isStock">是否在库</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool UpdateFrockStandingBookStock(DepotManagementDataContext ctx, DataTable frockTable, bool isStock,
            out string error)
        {
            error = null;

            try
            {
                if (frockTable != null && frockTable.Rows.Count != 0)
                {
                    for (int i = 0; i < frockTable.Rows.Count; i++)
                    {
                        var varData = from a in ctx.S_FrockStandingBook
                                      where a.GoodsID == Convert.ToInt32(frockTable.Rows[i]["GoodsID"].ToString())
                                      && a.FrockNumber == frockTable.Rows[i]["FrockNumber"].ToString()
                                      select a;

                        if (varData.Count() == 1)
                        {
                            varData.Single().IsInStock = isStock;
                        }
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
        /// 根据普通入库单号删除工装台帐与工装验证报告单
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">普通入库单号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool DeleteFrockOrdinaryInDepotBill(DepotManagementDataContext ctx, string billNo, out string error)
        {
            error = null;

            try
            {
                var varData = from a in ctx.S_FrockProvingReport
                              where a.ConnectBillNumber == billNo
                              select a;

                foreach (var item in varData)
                {
                    var varStanding = from a in ctx.S_FrockStandingBook
                                      where a.GoodsID == item.GoodsID
                                      && a.FrockNumber == item.FrockNumber
                                      select a;

                    ctx.S_FrockStandingBook.DeleteAllOnSubmit(varStanding);

                    var varAttached = from a in ctx.S_FrockProvingReportAttached
                                      where a.DJH == item.DJH
                                      select a;

                    ctx.S_FrockProvingReportAttached.DeleteAllOnSubmit(varAttached);

                    ctx.S_FrockProvingReport.DeleteOnSubmit(item);

                    m_assignBill.CancelBillNo(ctx, "工装验证报告单", item.DJH);
                    m_billMessageServer.DestroyMessage(item.DJH);
                }

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        public DataTable GetApplicableGoods(string frockNumber)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            IEnumerable<View_F_GoodsPlanCost> tempInfo = (from a in ctx.View_F_GoodsPlanCost
                                                         join b in ctx.S_FrockStandingBook_ApplicableGoods
                                                         on a.序号 equals b.GoodsID
                                                         where b.FrockNumber == frockNumber
                                                         select a).AsEnumerable<View_F_GoodsPlanCost>();

            return GlobalObject.GeneralFunction.ConvertToDataTable<View_F_GoodsPlanCost>(tempInfo);
        }

        /// <summary>
        /// 获得所有信息
        /// </summary>
        /// <param name="scrapFlag">报废标志 True 报废 False 未报废</param>
        /// <param name="isInStock">在库标志 True 显示 False 不显示</param>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回所有不是分装的工装信息</returns>
        public DataTable GetAllTable(bool scrapFlag, bool isInStock, int goodsID)
        {
            string strSql = "select * from View_S_FrockStandingBook where 1=1 and (父级工装编号 = '' or 父级工装编号 is null) and 物品ID = " + goodsID;

            if (!scrapFlag)
            {
                strSql += " and (报废人 is null or 报废人 = '')";
            }

            if (isInStock)
            {
                strSql += " and 是否在库 = '" + bool.TrueString + "'";
            }

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得所有信息
        /// </summary>
        /// <param name="scrapFlag">报废标志 True 报废 False 未报废</param>
        /// <param name="isInStock">在库标志 True 显示 False 不显示</param>
        /// <param name="isFinalAssembly">仅显示总装标志 True 显示 False 全部</param>
        /// <returns>返回所有不是分装的工装信息</returns>
        public DataTable GetAllTable(bool scrapFlag, bool isInStock, bool isFinalAssembly, bool isUsing)
        {
            string strSql = "select * from View_S_FrockStandingBook where 1=1 ";

            if (isFinalAssembly)
            {
                strSql += " and (父级工装编号 is null or 父级工装编号 = '')";
            }

            if (!scrapFlag)
            {
                strSql += " and (报废人 is null or 报废人 = '')";
            }

            if (isInStock)
            {
                strSql += " and 是否在库 = '" + bool.TrueString + "'";
            }

            if (isUsing)
            {
                strSql += " and 是否在库 = '" + bool.FalseString + "' and (报废人 is null or 报废人 = '')";
            }

            strSql += " order by 工装图号,工装编号 ";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得功能树的信息
        /// </summary>
        /// <returns>返回Table</returns>
        public DataTable GetTreeInfo()
        {
            string strSql = @"select cast( GoodsID as varchar(50)) + '-' + FrockNumber as  FrockNumber,b.物品名称 + '(' + FrockNumber + ')' FrockName, " +
                            " case when  ParentFrockNumber is null then '000000' " +
                            " else cast( ParentGoodsID as varchar(50)) + '-' + ParentFrockNumber end as ParentFrockNumber,b.图号型号 as GoodsCode  " +
                            " from dbo.S_FrockStandingBook as a inner join View_F_GoodsPlanCost as b on a.GoodsID = b.序号 " +
                            " Union all " +
                            " select '000000' ,'工装','Root','' " +
                            " order by GoodsCode,FrockNumber";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得新的工装编号
        /// </summary>
        /// <returns>返回工装编号</returns>
        public string GetNewFrockNumber()
        {
            string strSql = "select Max(FrockNumber) from S_FrockStandingBook where Len(FrockNumber) = 9  "+
                "and SUBSTRING(FrockNumber, 1,4) = '" + ServerTime.Time.Year.ToString() + 
                "' and SUBSTRING(FrockNumber, 5,2) = '" + ServerTime.Time.Month.ToString("D2") + "'";

            string strOutNumber = ServerTime.Time.Year.ToString() + ServerTime.Time.Month.ToString("D2") + "001";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dtTemp.Rows[0][0].ToString() != "")
            {

                if (dtTemp.Rows[0][0].ToString().Substring(0, 4) == ServerTime.Time.Year.ToString())
                {

                    if (dtTemp.Rows[0][0].ToString().Substring(4, 2) == ServerTime.Time.Month.ToString("D2"))
                    {
                        strOutNumber = ServerTime.Time.Year.ToString()
                            + ServerTime.Time.Month.ToString("D2")
                            + (Convert.ToInt32(dtTemp.Rows[0][0].ToString().Substring(6, 3)) + 1).ToString("D3");
                    }
                }
            }

            return strOutNumber;
        }

        /// <summary>
        /// 获得指定工装的分装信息
        /// </summary>
        /// <param name="goodsID">父级物品ID</param>
        /// <param name="frockNumber">父级工装编号</param>
        /// <param name="scrapFlag">报废标志</param>
        /// <returns>返回指定工装的分装信息 </returns>
        public DataTable GetSplitCharging(int goodsID, string frockNumber, bool scrapFlag)
        {
            string strSql = "select * from View_S_FrockStandingBook where 父级工装编号 = '"
                            + frockNumber + "' and 父级物品ID = " + goodsID;

            if (!scrapFlag)
            {
                strSql += " and (报废人 is null or 报废人 = '')";
            }

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得指定工装的业务信息
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="frockNumber">工装编号</param>
        /// <returns>返回指定工装的业务信息</returns>
        public DataTable GetFrockOperation(int goodsID, string frockNumber)
        {
            string strSql = "select BillID as 单据号, BillType as 业务类型, BillTime as 单据日期 " +
                " from S_FrockOperation where FrockNumber = '" + frockNumber
                + "' and IsTrue = 1 and  GoodsID = " + goodsID;

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得指定工装的维修信息
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="frockNumber">工装编号</param>
        /// <returns>返回指定工装的维修信息</returns>
        public DataTable GetServiceTable(int goodsID, string frockNumber)
        {
            string strSql = "select * from View_S_FrockServiceBill where 工装编号 = '" + frockNumber + "' and 物品ID = " + goodsID;

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得指定工装的检验报告单
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="frockNumber">工装编号</param>
        /// <returns>返回指定工装的检验报告</returns>
        public DataTable GetProvingReport(int goodsID, string frockNumber)
        {
            string strSql = "select * from View_S_FrockProvingReport where 物品ID = " + goodsID
                + " and 工装编号 = '" + frockNumber + "'";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获取台帐综合信息
        /// </summary>
        /// <param name="isFinalAssembly">仅显示总装标志 True 显示 False 全部</param>
        /// <returns>返回Table</returns>
        public DataTable GetBookSynthesizeInfo(bool isFinalAssembly)
        {
            string strSql = @"select row_number()over(order by a.物品ID) as 序号,工装图号,工装名称,
                            case when b.Count is null then 0 else b.Count end as 待检验数,
                            case when c.Count is null then 0 else c.Count end as 待验证数,
                            case when d.Count is null then 0 else d.Count end as 已领出数,
                            case when e.Count is null then 0 else e.Count end as 当前库存数,
                            a.Count as 台帐总数,a.物品ID
                            from (select 物品ID,工装图号,工装名称,Count(*) as Count from View_S_FrockStandingBook where 1=1 ";
            if (isFinalAssembly)
            {
                strSql += " and (父级工装编号 is null or 父级工装编号 = '') ";
            }
            strSql += @"group by 物品ID,工装图号,工装名称) as a 
                            left join 
                            (select 物品ID,Count(*)  as Count
                            from View_S_FrockStandingBook as a left join (select * from S_FrockProvingReport as a
                            where ID = (select Max(ID) from S_FrockProvingReport where FrockNumber = a.FrockNumber)) as b
                            on a.工装编号 = b.FrockNumber
                            where DJZT like '等待检验%' or DJZT = '新建单据'
                            group by 物品ID) as b on a.物品ID = b.物品ID
                            left join
                            (select 物品ID,Count(*)  as Count
                            from View_S_FrockStandingBook as a left join (select * from S_FrockProvingReport as a
                            where ID = (select Max(ID) from S_FrockProvingReport where FrockNumber = a.FrockNumber)) as b
                            on a.工装编号 = b.FrockNumber
                            where DJZT like '等待验证%' or DJZT = '等待结论'
                            group by 物品ID) as c on a.物品ID = c.物品ID
                            left join
                            (select 物品ID,Count(*)  as Count
                            from View_S_FrockStandingBook as a left join (select * from S_FrockProvingReport as a
                            where ID = (select Max(ID) from S_FrockProvingReport where FrockNumber = a.FrockNumber)) as b
                            on a.工装编号 = b.FrockNumber
                            where (DJZT = '单据已完成' or DJZT is null) and 是否在库 = 0
                            group by 物品ID) as d on a.物品ID = d.物品ID
                            left join
                            (select 物品ID,Count(*)  as Count
                            from View_S_FrockStandingBook as a left join (select * from S_FrockProvingReport as a
                            where ID = (select Max(ID) from S_FrockProvingReport where FrockNumber = a.FrockNumber)) as b
                            on a.工装编号 = b.FrockNumber
                            where (DJZT = '单据已完成' or DJZT is null) and 是否在库 = 1
                            group by 物品ID) as e on a.物品ID = e.物品ID";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得一条记录的数据集
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="frockNumber">工装编号</param>
        /// <returns>返回null表示数据不唯一,否则返回一条数据集</returns>
        public S_FrockStandingBook GetBookInfo(int goodsID, string frockNumber)
        {
            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            var varData = from a in dataContext.S_FrockStandingBook
                          where a.GoodsID == goodsID
                          && a.FrockNumber == frockNumber
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
        /// 更新维修信息
        /// </summary>
        /// <param name="ctx">LINQ数据上下文</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="frockNumber">工装编号</param>
        /// <param name="serviceTable">维修信息表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        private bool UpdateServiceData(DepotManagementDataContext ctx, int goodsID, string frockNumber,
            DataTable serviceTable, out string error)
        {
            error = null;

            try
            {
                var varData = from a in ctx.S_FrockServiceBill
                              where a.FrockNumber == frockNumber
                              && a.GoodsID == goodsID
                              select a;

                ctx.S_FrockServiceBill.DeleteAllOnSubmit(varData);

                if (serviceTable != null)
                {
                    for (int i = 0; i < serviceTable.Rows.Count; i++)
                    {
                        S_FrockServiceBill lnqFrockService = new S_FrockServiceBill();

                        lnqFrockService.BillID = serviceTable.Rows[i]["单据号"].ToString();
                        lnqFrockService.FrockNumber = frockNumber;
                        lnqFrockService.GoodsID = goodsID;
                        lnqFrockService.ServiceContent = serviceTable.Rows[i]["维修内容"].ToString();
                        lnqFrockService.ServicePersonnel = serviceTable.Rows[i]["人员名称"].ToString();
                        lnqFrockService.ServiceTime = Convert.ToDateTime(serviceTable.Rows[i]["时间"]);

                        ctx.S_FrockServiceBill.InsertOnSubmit(lnqFrockService);
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
        /// 更新工装台帐信息
        /// </summary>
        /// <param name="frockStandingBook">工装台帐数据集</param>
        /// <param name="serviceTable">维修信息表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool UpdateFrockStandingBook(S_FrockStandingBook frockStandingBook, DataTable serviceTable, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.S_FrockStandingBook
                              where a.GoodsID == frockStandingBook.GoodsID
                              && a.FrockNumber == frockStandingBook.FrockNumber
                              select a;

                if (varData.Count() == 0)
                {
                    //string strSql = "select * from S_FrockProvingReport where GoodsID = " + frockStandingBook.GoodsID
                    //    + " and FrockNumber = '" + frockStandingBook.FrockNumber + "'";

                    //DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

                    //if (dtTemp.Rows.Count > 0)
                    //{
                    //    error = "不能添加已存在工装验证报告单的工装台帐";
                    //    return false;
                    //}

                    dataContext.S_FrockStandingBook.InsertOnSubmit(frockStandingBook);
                }
                else if (varData.Count() == 1)
                {
                    S_FrockStandingBook lnqFrockStandingBook = varData.Single();

                    lnqFrockStandingBook.ApplyToDevice = frockStandingBook.ApplyToDevice;
                    lnqFrockStandingBook.ApplyToProcess = frockStandingBook.ApplyToProcess;
                    lnqFrockStandingBook.ApplyToProductCode = frockStandingBook.ApplyToProductCode;
                    lnqFrockStandingBook.ApplyToProductName = frockStandingBook.ApplyToProductName;
                    lnqFrockStandingBook.ApplyToWorkShop = frockStandingBook.ApplyToWorkShop;
                    lnqFrockStandingBook.Designer = frockStandingBook.Designer;
                    lnqFrockStandingBook.ParentFrockNumber = frockStandingBook.ParentFrockNumber;
                    lnqFrockStandingBook.ParentGoodsID = frockStandingBook.ParentGoodsID;
                    lnqFrockStandingBook.ScarpBillID = frockStandingBook.ScarpBillID;
                    lnqFrockStandingBook.ScarpPersonnel = frockStandingBook.ScarpPersonnel;
                    lnqFrockStandingBook.ScarpReason = frockStandingBook.ScarpReason;
                    lnqFrockStandingBook.ScarpTime = frockStandingBook.ScarpTime;
                    lnqFrockStandingBook.FinalPersonnel = BasicInfo.LoginName;
                    lnqFrockStandingBook.FinalTime = ServerTime.Time;
                    lnqFrockStandingBook.IdentifyCycle = frockStandingBook.IdentifyCycle;
                    lnqFrockStandingBook.IdentifyCycleType = frockStandingBook.IdentifyCycleType;
                }
                else
                {
                    error = "数据不唯一";
                    return false;
                }

                if (!UpdateServiceData(dataContext, frockStandingBook.GoodsID,
                    frockStandingBook.FrockNumber, serviceTable, out error))
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

        /// <summary>
        /// 删除工装台帐表信息
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="frockNumber">工装编号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool DeleteFrockStandingBook(int goodsID, string frockNumber, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                string strSql = "select * from S_FrockProvingReport where GoodsID = " + goodsID + " and frockNumber = '"
                    + frockNumber + "' and BillType = '入库检验' and IsInStock = 1 ";

                DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

                if (dtTemp.Rows.Count > 0)
                {
                    error = "不能删除此台帐，由于已存在入库业务与工装验证报告单";
                    return false;
                }

                var varData = from a in dataContext.S_FrockStandingBook
                              where a.GoodsID == goodsID
                              && a.FrockNumber == frockNumber
                              select a;

                dataContext.S_FrockStandingBook.DeleteAllOnSubmit(varData);

                dataContext.SubmitChanges();
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