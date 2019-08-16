/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  OrderFormAffrim.cs
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
using WebServerModule;


namespace ServerModule
{
    /// <summary>
    /// 网络订单管理类
    /// </summary>
    class OrderFormAffrim:BasicServer, ServerModule.IOrderFormAffrim
    {
        /// <summary>
        /// 获得总单的所有信息
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="flag">单据状态</param>
        /// <returns>返回查询到的信息</returns>
        public DataTable GetAllInfo(DateTime startTime,DateTime endTime,string flag)
        {
            string strSql = " select * from View_B_WebForOrderFormBill "+
                            " where 创建日期 between '" + startTime + "' and '" + endTime + "'";
            if (flag != "全    部")
            {
                strSql += " and 单据状态 = '" + flag + "'";
            }

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dt;
        }

        /// <summary>
        /// 获得某一单据的明细信息
        /// </summary>
        /// <param name="yearAndMonth">年月字符串 格式为“YYYYMM”</param>
        /// <param name="workName">采购员姓名</param>
        /// <returns>返回查询到的明细信息</returns>
        public DataTable GetListInfo(string yearAndMonth,string workName)
        {
            string strSql = " select b.* from View_B_AccessoryDutyInfo as a " +
                            " inner join View_B_WebForOrderFormList as b on a.图号型号 = b.图号型号 " +
                            " and a.物品名称 = b.物品名称 and a.规格 = b.规格  where b.单据号 = '" + yearAndMonth
                            + "' and (a.供应商A采购员 = '" + workName + "' or a.供应商C采购员 = '" + workName 
                            + "' or a.供应商C采购员 = '" + workName + "')";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dt;
        }

        /// <summary>
        /// 获得某一单据的明细信息
        /// </summary>
        /// <param name="yearAndMonth">年月字符串</param>
        /// <returns>返回查询到的明细信息</returns>
        public DataTable GetListInfo(string yearAndMonth)
        {
            string strSql = " select b.* from View_B_AccessoryDutyInfo as a " +
                            " inner join View_B_WebForOrderFormList as b on a.图号型号 = b.图号型号 " +
                            " and a.物品名称 = b.物品名称 and a.规格 = b.规格  where b.单据号 = '" + yearAndMonth + "' ";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dt;
        }

        /// <summary>
        /// 添加网络订单明细
        /// </summary>
        /// <param name="webOrderFormList">网络订单信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>添加成功返回True，添加失败返回False</returns>
        public bool AddBill(B_WebForOrderFormList webOrderFormList,out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.B_WebForOrderFormList
                              where a.GoodsID == webOrderFormList.GoodsID
                              && a.Provider == webOrderFormList.Provider
                              && a.Ny == webOrderFormList.Ny
                              select a;

                if (varData.Count() > 0)
                {
                    error = "此记录已存在，不可重复录入";
                    return false;
                }
                else
                {
                    dataContext.B_WebForOrderFormList.InsertOnSubmit(webOrderFormList);
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
        /// 修改网络订单明细
        /// </summary>
        /// <param name="webOrderFormList">网络订单明细信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>修改成功返回True，修改失败返回False</returns>
        public bool UpdateListInfo(B_WebForOrderFormList webOrderFormList, out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.B_WebForOrderFormList
                              where a.ID == Convert.ToInt32(webOrderFormList.ID)
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数据不唯一或者为空";
                    return false;
                }
                else
                {
                    B_WebForOrderFormList lnqWebForOrderForm = varData.Single();

                    lnqWebForOrderForm.BargainNumber = webOrderFormList.BargainNumber;
                    lnqWebForOrderForm.CheckPersonnel = webOrderFormList.CheckPersonnel;
                    lnqWebForOrderForm.GoodsID = webOrderFormList.GoodsID;
                    lnqWebForOrderForm.OrderFormCount = webOrderFormList.OrderFormCount;
                    lnqWebForOrderForm.OrderFormNumber = webOrderFormList.OrderFormNumber;
                    lnqWebForOrderForm.Provider = webOrderFormList.Provider;
                    lnqWebForOrderForm.StockCount = webOrderFormList.StockCount;
                    lnqWebForOrderForm.ChangeFlag = webOrderFormList.ChangeFlag;

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
        /// 改变BILL的单据状态为 等待审核
        /// </summary>
        /// <param name="yearAndMonth">年月字符串</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>改变成功返回True，改变失败返回False</returns>
        public bool UpdateBillStatus(string yearAndMonth,out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.B_WebForOrderFormBill
                              where a.Ny == yearAndMonth
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数据为空或者不唯一";
                    return false;
                }
                else
                {
                    B_WebForOrderFormBill lnqBill = varData.Single();
                    lnqBill.DJZT = "等待审核";
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
        /// 删除数据
        /// </summary>
        /// <param name="id">单据ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        public bool DeleteBill(int id,out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.B_WebForOrderFormList
                              where a.ID == id
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数据不唯一或者为空";
                    return false;
                }
                else
                {
                    dataContext.B_WebForOrderFormList.DeleteOnSubmit(varData.Single());
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
        /// 添加确认到货日期
        /// </summary>
        /// <param name="webForAffirmTime">网络订单确认信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>添加成功返回True，添加失败返回False</returns>
        public bool AddAffrimInfo(B_WebForAffirmTime webForAffirmTime,out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.B_WebForAffirmTime
                              where a.ListID == webForAffirmTime.ListID
                              && a.AffirmTime == webForAffirmTime.AffirmTime
                              select a;

                if (varData.Count() > 0)
                {
                    error = "数据不唯一，不能重复录入";
                    return false;
                }
                else
                {
                    dataContext.B_WebForAffirmTime.InsertOnSubmit(webForAffirmTime);
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
        /// 删除确认到货日期
        /// </summary>
        /// <param name="id">单据ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        public bool DeleteAffrimInfo(int id,out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.B_WebForAffirmTime
                              where a.ID == id
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数据不唯一或者为空";
                    return false;
                }
                else
                {
                    dataContext.B_WebForAffirmTime.DeleteOnSubmit(varData.Single());
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
        /// 整张订单删除
        /// </summary>
        /// <param name="yearAndMonth">年月字符串</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        public bool DeleteAllInfo(string yearAndMonth,out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.B_WebForOrderFormBill
                              where a.Ny == yearAndMonth
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数据不唯一或者为空";
                    return false;
                }
                else
                {
                    dataContext.B_WebForOrderFormBill.DeleteOnSubmit(varData.Single());
                }

                var varList = from b in dataContext.B_WebForOrderFormList
                              where b.Ny == yearAndMonth
                              select b;

                dataContext.B_WebForOrderFormList.DeleteAllOnSubmit(varList);

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
        /// 修改单据状态
        /// </summary>
        /// <param name="yearAndMonth">操作年月字符串</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>修改成功返回True，修改失败返回False</returns>
        public bool UpdateBill(string yearAndMonth,out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.B_WebForOrderFormBill
                              where a.Ny == yearAndMonth
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数据不唯一或者为空";
                    return false;
                }
                else
                {
                    B_WebForOrderFormBill lnqBill = varData.Single();

                    switch (lnqBill.DJZT)
                    {
                        case "等待审核":
                            lnqBill.DJZT = "等待发布";
                            lnqBill.AuthorizePersonnel = BasicInfo.LoginName;
                            lnqBill.AuthorizeTime = ServerTime.Time;

                            var varOrderFormList = from a in dataContext.B_WebForOrderFormList
                                                   where a.Ny == yearAndMonth
                                               select a;

                            foreach (var item in varOrderFormList)
                            {
                                item.ChangeFlag = false;
                            }

                            break;
                        case "等待发布":
                            lnqBill.DJZT = "单据已完成";
                            lnqBill.IssuePersonnel = BasicInfo.LoginName;
                            lnqBill.IssueTime = ServerTime.Time;

                            //插入WINFROM 订单表
                            if (!InsertWinFormOrderForm(dataContext,yearAndMonth, out error))
                            {
                                return false;
                            }

                            dataContext.SubmitChanges();
                            //插入WEB 订单表
                            if (!InsertWebOrderForm(yearAndMonth,out error))
                            {
                                return false;
                            }
                            break;
                        default:
                            break;
                    }

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
        /// 插入WEB订单
        /// </summary>
        /// <param name="yearAndMonth">操作年月字符串</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>插入成功返回True，插入失败返回False</returns>
        bool InsertWebOrderForm(string yearAndMonth,out string error)
        {
            error = null;
            BargainInfoServer serverBargainInfo = new BargainInfoServer();
            try
            {
                WebSeverDataContext dataContext = WebServerModule.WebDatabaseParameter.WebDataContext;
                //插入订单Bill
                string strSql = "select distinct 订单号 + '-' + convert(varchar(50),ID) as 订单号 ,"+
                    " 合同号 from (select 物品ID,供应商,订单号,到货数,到货日期,合同号, "+
                    " Row_Number()Over(partition by 物品ID,供应商 order by 到货日期) as ID "+
                    " from View_B_WebForOrderFormList as a inner join View_B_WebForAffrimTime as b "+
                    " on a.序号 = b.明细ID where 单据号 = '" + yearAndMonth + "') as a";

                DataTable dtBill = GlobalObject.DatabaseServer.QueryInfo(strSql);

                for (int i = 0; i < dtBill.Rows.Count; i++)
                {
                    DataRow drBargainInfo = serverBargainInfo.GetBargainInfoDataRow(dtBill.Rows[i]["合同号"].ToString());

                    OF_OrderFormInfo lnqInfo = new OF_OrderFormInfo();

                    lnqInfo.OrderFormNumber = dtBill.Rows[i]["订单号"].ToString();
                    lnqInfo.Provider = drBargainInfo["Provider"].ToString();
                    lnqInfo.Buyer = drBargainInfo["Buyer"].ToString();
                    lnqInfo.ProviderLinkMan = drBargainInfo["ProviderLinkman"].ToString();
                    lnqInfo.ProviderLinkMode = drBargainInfo["LaisonMode"].ToString();
                    lnqInfo.Remark = drBargainInfo["Remark"].ToString();
                    lnqInfo.CreateDate = ServerTime.Time;

                    dataContext.OF_OrderFormInfo.InsertOnSubmit(lnqInfo);
                }

                //插入订单List
                strSql = "select 图号型号,物品名称,规格,到货数," +
                    " 订单号 + '-' + convert(varchar(50),ID) as 订单号 ," +
                    " convert(varchar(10), 到货日期,120) + ' 23:59:59' as 到货日期 " +
                    " from (select 图号型号,物品名称,规格,订单号,到货数,到货日期, " +
                    " Row_Number()Over(partition by 物品ID,供应商 order by 到货日期) as ID " +
                    " from View_B_WebForOrderFormList as a " +
                    " inner join View_B_WebForAffrimTime as b " +
                    " on a.序号 = b.明细ID where 单据号 = '" + yearAndMonth + "') as a ";

                DataTable dtList = GlobalObject.DatabaseServer.QueryInfo(strSql);

                for (int i = 0; i < dtList.Rows.Count; i++)
                {
                    OF_OrderFormGoods lnqGoods = new OF_OrderFormGoods();

                    lnqGoods.OrderFormNumber = dtList.Rows[i]["订单号"].ToString();
                    lnqGoods.GoodsCode = dtList.Rows[i]["图号型号"].ToString();
                    lnqGoods.GoodsName = dtList.Rows[i]["物品名称"].ToString();
                    lnqGoods.Spec = dtList.Rows[i]["规格"].ToString();
                    lnqGoods.ArriveTime = Convert.ToDateTime(dtList.Rows[i]["到货日期"]);
                    lnqGoods.OrderCount = Convert.ToDecimal(dtList.Rows[i]["到货数"]);

                    dataContext.OF_OrderFormGoods.InsertOnSubmit(lnqGoods);
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
        /// 插入WINFORM订单
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="yearAndMonth">操作的年月字符串</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>插入成功返回True,插入失败返回False</returns>
        bool InsertWinFormOrderForm(DepotManagementDataContext context,string yearAndMonth,out string error)
        {
            BargainInfoServer serverBargainInfo = new BargainInfoServer();

            try
            {
                error = null;

                //插入订单Bill
                string strSql = "select distinct 订单号 + '-' + convert(varchar(50),ID) as 订单号 ,"+
                    " 合同号 from (select 物品ID,供应商,订单号,到货数,到货日期,合同号, "+
                    " Row_Number()Over(partition by 物品ID,供应商 order by 到货日期) as ID "+
                    " from View_B_WebForOrderFormList as a inner join View_B_WebForAffrimTime as b "+
                    " on a.序号 = b.明细ID where 单据号 = '" + yearAndMonth + "') as a";

                DataTable dtBill = GlobalObject.DatabaseServer.QueryInfo(strSql);

                for (int i = 0; i < dtBill.Rows.Count; i++)
                {
                    DataRow drBargainInfo = serverBargainInfo.GetBargainInfoDataRow(dtBill.Rows[i]["合同号"].ToString());

                    B_OrderFormInfo lnqInfo = new B_OrderFormInfo();

                    lnqInfo.BargainNumber = drBargainInfo["BargainNumber"].ToString();
                    lnqInfo.Buyer = drBargainInfo["Buyer"].ToString();
                    lnqInfo.CreateDate = ServerTime.Time;
                    lnqInfo.InputPerson = drBargainInfo["InputPerson"].ToString();
                    lnqInfo.OrderFormNumber = dtBill.Rows[i]["订单号"].ToString();
                    lnqInfo.Provider = drBargainInfo["Provider"].ToString();
                    lnqInfo.ProviderLinkman = drBargainInfo["ProviderLinkman"].ToString();
                    lnqInfo.ProviderPhone = drBargainInfo["LaisonMode"].ToString();
                    lnqInfo.ProviderFax = drBargainInfo["LaisonMode"].ToString();
                    lnqInfo.ProviderEmail = "";
                    lnqInfo.Remark = drBargainInfo["Remark"].ToString();
                    lnqInfo.TypeID = 2;

                    context.B_OrderFormInfo.InsertOnSubmit(lnqInfo);
                }

                //插入订单List
                strSql = "select 图号型号,物品名称,规格,到货数," +
                    " 订单号 + '-' + convert(varchar(50),ID) as 订单号 ," +
                    " convert(varchar(10), 到货日期,120) + ' 23:59:59' as 到货日期 " +
                    " from (select 图号型号,物品名称,规格,订单号,到货数,到货日期,物品ID " +
                    " Row_Number()Over(partition by 物品ID,供应商 order by 到货日期) as ID " +
                    " from View_B_WebForOrderFormList as a " +
                    " inner join View_B_WebForAffrimTime as b " +
                    " on a.序号 = b.明细ID where 单据号 = '" + yearAndMonth + "') as a ";

                DataTable dtList = GlobalObject.DatabaseServer.QueryInfo(strSql);

                for (int i = 0; i < dtList.Rows.Count; i++)
                {
                    B_OrderFormGoods lnqGoods = new B_OrderFormGoods();

                    lnqGoods.Amount = Convert.ToDecimal(dtList.Rows[i]["到货数"]);
                    lnqGoods.ArrivalDate = Convert.ToDateTime(dtList.Rows[i]["到货日期"]);
                    lnqGoods.CreateDate = ServerTime.Time;
                    lnqGoods.OrderFormNumber = dtList.Rows[i]["订单号"].ToString();
                    lnqGoods.Remark = "";
                    lnqGoods.GoodsID = (int)dtList.Rows[i]["物品ID"];

                    context.B_OrderFormGoods.InsertOnSubmit(lnqGoods);
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
        /// 获得最大的到货日期
        /// </summary>
        /// <param name="listID">明细ID</param>
        /// <returns>返回最大的到货日期</returns>
        DateTime GetMaxArrivalDate(int listID)
        {
            string strSql = "select Max(到货日期) from View_B_WebForAffrimTime where 明细ID = " + listID;

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return Convert.ToDateTime(dt.Rows[0][0]);
        }

        /// <summary>
        /// 获得到货日期表
        /// </summary>
        /// <param name="listID">明细ID</param>
        /// <returns>返回到货日期列表</returns>
        public DataTable GetAffrimInfo(int listID)
        {
            string strSql = "select * from View_B_WebForAffrimTime where 明细ID = " + listID;

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dt;
        }

        /// <summary>
        /// 获得某一条记录的所有到货数之和
        /// </summary>
        /// <param name="listID">明细ID</param>
        /// <returns>返货明细ID的到货数量之和</returns>
        public decimal SumCount(int listID)
        {
            string strSql = "select Sum(AffirmCount) from  B_WebForAffirmTime where ListID = "+ listID;

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return Convert.ToDecimal(dt.Rows[0][0]);
        }

        /// <summary>
        /// 检查数据
        /// </summary>
        /// <param name="djID">单据ID</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="provider">供应商编码</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>无记录则返回True，否则返回False</returns>
        public bool IsListInfoIn(int djID,int goodsID,string provider,out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.B_WebForOrderFormList
                              where a.ID != djID
                              && a.GoodsID == goodsID
                              && a.Provider == provider
                              select a;

                if (varData.Count() == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }   
    }
}
