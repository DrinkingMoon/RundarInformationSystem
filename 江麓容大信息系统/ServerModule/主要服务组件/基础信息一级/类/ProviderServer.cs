/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  ProviderServer.cs
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
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using DBOperate;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace ServerModule
{
    /// <summary>
    /// 供应商管理类
    /// </summary>
    class ProviderServer : IProviderServer
    {
        /// <summary>
        /// 计划成本服务组件
        /// </summary>
        IBasicGoodsServer m_planCostServer = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();
        /// <summary>
        /// 数据库操作接口
        /// </summary>
        static IDBOperate m_dbOperate = CommentParameter.GetDBOperatorOfDepotManagement();

        public Provider GetProviderInfo(string provider)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Provider
                          where a.ProviderCode == provider
                          || a.ProviderName == provider
                          select a;

            if (varData.Count() == 0)
            {
                return null;
            }
            else
            {
                return varData.Single();
            }
        }

        /// <summary>
        /// 获得供应商全称
        /// </summary>
        /// <param name="provider">供应商编码</param>
        /// <returns>供应商全称</returns>
        public string GetPrivderName(string provider)
        {
            if (provider == null || provider == "")
            {
                return "";
            }
            else
            {
                string strSql = "select ProviderName from Provider where ProviderCode = '" + provider + "'";
                DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

                return dtTemp.Rows[0][0].ToString();
            }
        }

        /// <summary>
        /// 获取供应商信息表
        /// </summary>
        /// <param name="table">供应商信息表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取供应商信息表</returns>
        public bool GetAllProvider(out DataTable table, out string error)
        {
            table = null;
            error = null;

            DataSet dsProviderInfo = new DataSet();
            Dictionary<OperateCMD, object> dicOperateCMD = m_dbOperate.RunProc_CMD("SelAllProvider", dsProviderInfo);

            if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
            {
                error = Convert.ToString(dicOperateCMD[OperateCMD.Return_Errmsg]);
                return false;
            }

            table = dsProviderInfo.Tables[0];
            return true;
        }

        /// <summary>
        /// 获取供应商信息表
        /// </summary>
        /// <param name="returnBill">供应商信息表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取供应商信息表</returns>
        public bool GetAllProvider(out IQueryable<View_Provider> returnBill, out string error)
        {
            returnBill = null;
            error = null;

            try
            {
                DepotManagementDataContext depotMangaeDataContext = CommentParameter.DepotDataContext;
                IQueryable<View_Provider> providerTable = depotMangaeDataContext.GetTable<View_Provider>();

                returnBill = from c in providerTable  orderby c.供应商编码 select c;
            }
            catch (Exception err)
            {
                error = err.ToString();
                return false;
            }

            return true;
        }

        /// <summary>
        /// 获得供应商名称
        /// </summary>
        /// <param name="providercode">供应商编码</param>
        /// <returns>返回供应商名称</returns>
        public string GetProviderName(string providercode)
        {
            string strSql = "select ShortName from Provider where ProviderCode = '" + providercode + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dt.Rows[0][0].ToString();
        }

        /// <summary>
        /// 获取唯一性供应商信息表
        /// </summary>
        /// <param name="returnBill">供应商信息表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取供应商信息表</returns>
        public bool GetDistinctProvider(out IQueryable<View_Provider> returnBill, out string error)
        {
            returnBill = null;
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;
                returnBill = (from c in dataContext.View_Provider 
                              where c.供应商编码 != "" 
                              orderby c.供应商编码
                              select c).Distinct();

                return true;
            }
            catch (Exception err)
            {
                error = err.ToString();
                return false;
            }
        }

        /// <summary>
        /// 获取供应商信息表
        /// </summary>
        /// <param name="returnBill">供应商信息表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取供应商信息表</returns>
        public bool GetAllNewProvider(out IQueryable<View_B_NewProvider> returnBill, out string error)
        {
            returnBill = null;
            error = null;

            try
            {
                DepotManagementDataContext depotMangaeDataContext = CommentParameter.DepotDataContext;
                IQueryable<View_B_NewProvider> newProviderTable = depotMangaeDataContext.GetTable<View_B_NewProvider>();

                returnBill = from c in newProviderTable 
                             orderby c.新供应商编码
                             select c;

                return true;
            }
            catch (Exception err)
            {
                error = err.ToString();
                return false;
            }
        }

        /// <summary>
        /// 添加/修改部门信息表
        /// </summary>
        /// <param name="providerCode">供应商编码</param>
        /// <param name="providerName">供应商名称</param>
        /// <param name="shortName">简称</param>
        /// <param name="personnel">责任人</param>
        /// <param name="isOdd">是否关联责任人</param>
        /// <param name="isUse">是否在用</param>
        /// <param name="isMainDuty">是否为主要责任人</param>
        /// <param name="returnBill">部门信息表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <param name="isInternalSupplier">是否为内部供应商</param>
        /// <param name="clearingForm">挂账方式</param>
        /// <param name="yearMonth">追溯年月</param>
        /// <returns>返回是否成功添加/修改部门信息表</returns>
        public bool AddProvider(string providerCode, string providerName, string shortName, 
            string personnel, out IQueryable<View_Provider> returnBill, out string error,
            bool isOdd,bool isUse, bool isMainDuty, bool isInternalSupplier, string clearingForm, string yearMonth)
        {
            returnBill = null;
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                if (clearingForm == "用量" && yearMonth.Trim().Length != 6)
                {
                    throw new Exception("【用量挂账】需填写【追溯年月】");
                }

                Table<Provider> billtable = dataContxt.GetTable<Provider>();
                Table<ProviderPrincipal> billPrincipalTable = dataContxt.GetTable<ProviderPrincipal>();

                var billGather = from c in billtable 
                                 where c.ProviderCode == providerCode 
                                 select c;

                var billPrincipal = from c in billPrincipalTable 
                                    where c.Provider == providerCode && c.PrincipalWorkId == personnel 
                                    select c;

                int intSameNoteCount = billGather.Count<Provider>();
                int intDiffrentNodteCount = billPrincipal.Count<ProviderPrincipal>();

                if (intSameNoteCount != 0 && intDiffrentNodteCount != 0)
                {
                    error = "不可重复添加相同关系表";
                    return false;
                }
                else
                {
                    if (intSameNoteCount == 0)
                    {
                        Provider tableProvider = new Provider();

                        tableProvider.ProviderCode = providerCode;
                        tableProvider.ProviderName = providerName;
                        tableProvider.ShortName = shortName;
                        tableProvider.PY = UniversalFunction.GetPYWBCode(providerName, "PY");
                        tableProvider.WB = UniversalFunction.GetPYWBCode(providerName, "WB");
                        tableProvider.IsUse = isUse;
                        tableProvider.IsOddProvider = isOdd;
                        tableProvider.IsInternalSupplier = isInternalSupplier;
                        tableProvider.ClearingForm = clearingForm;
                        tableProvider.AscendYearMonth = clearingForm == "入库" ? null : yearMonth;

                        dataContxt.Provider.InsertOnSubmit(tableProvider);
                        dataContxt.SubmitChanges();
                    }

                    #region  添加到关系表中
                    ProviderPrincipal tableProviderPrincipal = new ProviderPrincipal();

                    tableProviderPrincipal.Provider = providerCode;
                    tableProviderPrincipal.PrincipalWorkId = personnel;
                    tableProviderPrincipal.IsMainDuty = isMainDuty;
                    dataContxt.ProviderPrincipal.InsertOnSubmit(tableProviderPrincipal);

                    dataContxt.SubmitChanges();
                    #endregion

                    GetAllProvider(out returnBill, out error);
                }

                return true;
            }
            catch (Exception err)
            {
                return SetReturnError1(err, out returnBill, out error);
            }
        }

        /// <summary>
        /// 添加/修改部门信息表
        /// </summary>
        /// <param name="providerCode">供应商编码</param>
        /// <param name="providerName">供应商名称</param>
        /// <param name="remark">备注</param>
        /// <param name="returnBill">部门信息表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加/修改部门信息表</returns>
        public bool AddNewProvider(string providerCode, string providerName, string remark, 
            out IQueryable<View_B_NewProvider> returnBill, out string error)
        {
            returnBill = null;
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                Table<B_NewProvider> billtable = dataContxt.GetTable<B_NewProvider>();
                var billGather = from c in billtable where c.NewProviderCode == providerCode select c;

                int intSameNoteCount = billGather.Count<B_NewProvider>();

                if (intSameNoteCount == 0)
                {
                    B_NewProvider tableProvider = new B_NewProvider();

                    tableProvider.NewProviderCode = providerCode;
                    tableProvider.NewProviderName = providerName;
                    tableProvider.Remark = remark;

                    dataContxt.B_NewProvider.InsertOnSubmit(tableProvider);
                    dataContxt.SubmitChanges();
                    GetAllNewProvider(out returnBill, out error);
                }
                else
                {
                    error = "该单据已提交,系统不允许重复提交相同编号的供应商!";
                    return false;
                }

                return true;
            }
            catch (Exception err)
            {
                return SetReturnError2(err, out returnBill, out error);
            }
        }

        /// <summary>
        /// 添加/修改供应商信息表
        /// </summary>
        /// <param name="providerCode">供应商编码</param>
        /// <param name="providerName">供应商名称</param>
        /// <param name="shortName">简称</param>
        /// <param name="isOdd">是否修改</param>
        /// <param name="isUse">是否在用</param>
        /// <param name="isMainDuty">是否为主要责任人</param>
        /// <param name="personnel">新责任人</param>
        /// <param name="oldPersonnel">老责任人</param>
        /// <param name="returnBill">变更后返回的数据集</param>
        /// <param name="error">错误信息</param>
        /// <param name="isInternalSupplier">是否为内部供应商</param>
        /// <param name="clearingForm">挂账方式</param>
        /// <param name="yearMonth">追溯年月</param>
        /// <returns>添加/修改成功返回True，失败返回False</returns>
        public bool UpdataProvider(string providerCode, string providerName, string shortName, 
            bool isOdd, bool isUse, bool isMainDuty, bool isInternalSupplier, string personnel, string oldPersonnel,
            string clearingForm, string yearMonth,
            out IQueryable<View_Provider> returnBill, out string error)
        {
            returnBill = null;
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                if (clearingForm == "用量" && yearMonth.Trim().Length != 6)
                {
                    throw new Exception("【用量挂账】需填写【追溯年月】");
                }

                Table<Provider> table = dataContxt.GetTable<Provider>();

                var varProvider = from c in table where c.ProviderCode == providerCode select c;
                
                foreach (var ei in varProvider)
                {
                    ei.ProviderName = providerName;
                    ei.ShortName = shortName;
                    ei.IsUse = isUse;
                    ei.IsOddProvider = isOdd;
                    ei.IsInternalSupplier = isInternalSupplier;
                    ei.ClearingForm = clearingForm;
                    ei.AscendYearMonth = clearingForm == "入库" ? null : yearMonth;
                }

                dataContxt.SubmitChanges();

                #region 更新到关系表

                Table<ProviderPrincipal> tablePp = dataContxt.GetTable<ProviderPrincipal>();
                var varProviderPrincipal = from c in tablePp 
                                           where c.Provider == providerCode 
                                           && c.PrincipalWorkId == oldPersonnel 
                                           select c;

                foreach (var eiPp in varProviderPrincipal)
                {
                    eiPp.PrincipalWorkId = personnel;
                    eiPp.IsMainDuty = isMainDuty;

                }

                dataContxt.SubmitChanges();

                #endregion 

                GetAllProvider(out returnBill, out error);

                return true;
            }
            catch (Exception err)
            {
                return SetReturnError1(err, out returnBill, out error);
            }
        }

        public void InsertUnitPriceList(string provider)
        {
            string error = "";
            Hashtable hstable = new Hashtable();
            hstable.Add("@Provider", provider);
            GlobalObject.DatabaseServer.QueryInfoPro("Add_Bus_PurchasingMG_UnitPriceList_UseType", hstable, out error);
        }   

        /// <summary>
        /// 添加/修改供应商信息表
        /// </summary>
        /// <param name="providerCode">供应商编码</param>
        /// <param name="providerName">供应商名称</param>
        /// <param name="remark">备注</param>
        /// <param name="returnBill">操作成功后返回的查询信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加/修改供应商信息表</returns>
        public bool UpdataNewProvider(string providerCode, string providerName, string remark, 
            out IQueryable<View_B_NewProvider> returnBill, out string error)
        {
            returnBill = null;
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                Table<B_NewProvider> table = dataContxt.GetTable<B_NewProvider>();

                var varProvider = from c in table where c.NewProviderCode == providerCode select c;

                foreach (var ei in varProvider)
                {
                    ei.NewProviderName = providerName;
                    ei.Remark = remark;
                }

                dataContxt.SubmitChanges();

                GetAllNewProvider(out returnBill, out error);

                return true;
            }
            catch (Exception err)
            {
                return SetReturnError2(err, out returnBill, out error);
            }
        }

        /// <summary>
        /// 停用此供应商
        /// </summary>
        /// <param name="providerCode">供应商编码</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool UpdateProviderIsUse(string providerCode,out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext contxt = CommentParameter.DepotDataContext;

                var varData = from a in contxt.Provider
                              where a.ProviderCode == providerCode
                              select a;

                if (varData.Count() == 1)
                {
                    Provider lnqPro = varData.Single();

                    lnqPro.IsUse = false;
                }
                else
                {
                    error = "数据不唯一或者为空";
                    return false;
                }

                contxt.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 检测供应商是否在库存表已存在
        /// </summary>
        /// <param name="providercode">供应商编码</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>不存在返回True，存在返回False</returns>
        public bool CheckStockPrivuderIsIn(string providercode,out string error)
        {
            error = null;

            try
            {
                string strSql = "select * from S_Stock where Provider = '" + providercode + "'";
                DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

                if (dt.Rows.Count == 0)
                {
                    return true;
                }
                else
                {
                    error = "库存表里已经存在此供应商编码，仅能停用此供应商";
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
        /// 删除某一供应商
        /// </summary>
        /// <param name="providerCode">供应商编码</param>
        /// <param name="persnonnelCode">工号</param>
        /// <param name="returnBill">操作成功后返回的查询信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功删除某一供应商</returns>
        public bool DeleteProvider(string providerCode,string persnonnelCode, out IQueryable<View_Provider> returnBill, out string error)
        {
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                #region 删除 关系表的信息

                Table<ProviderPrincipal> tablePp = dataContxt.GetTable<ProviderPrincipal>();
                var delRowPp = from c in tablePp where c.Provider == providerCode && c.PrincipalWorkId == persnonnelCode select c;

                foreach (var eiPp in delRowPp)
                {
                    tablePp.DeleteOnSubmit(eiPp);
                }
                #endregion 

                dataContxt.SubmitChanges();

                GetAllProvider(out returnBill, out error);

                return true;
            }
            catch (Exception err)
            {
                return SetReturnError1(err, out returnBill, out error);
            }
        }

        /// <summary>
        /// 删除某一供应商
        /// </summary>
        /// <param name="providerCode">供应商编码</param>
        /// <param name="returnBill">操作成功后返回的查询信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功删除某一供应商</returns>
        public bool DeleteNewProvider(string providerCode, out IQueryable<View_B_NewProvider> returnBill, out string error)
        {
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                Table<B_NewProvider> table = dataContxt.GetTable<B_NewProvider>();

                var delRow = from c in table where c.NewProviderCode == providerCode select c;

                foreach (var ei in delRow)
                {
                    table.DeleteOnSubmit(ei);
                }

                dataContxt.SubmitChanges();

                GetAllNewProvider(out returnBill, out error);

                return true;
            }
            catch (Exception err)
            {
                return SetReturnError2(err, out returnBill, out error);
            }
        }

        /// <summary>
        /// 设置出错返回值
        /// </summary>
        /// <param name="err">传入的错误信息</param>
        /// <param name="returnBill">部门信息</param>
        /// <param name="error">返回的错误信息</param>
        /// <returns>始终返回False</returns>
        bool SetReturnError2(object err, out IQueryable<View_B_NewProvider> returnBill, out string error)
        {
            returnBill = null;
            error = err.ToString();

            return false;
        }

        /// <summary>
        /// 设置出错返回值
        /// </summary>
        /// <param name="err">传入的错误信息</param>
        /// <param name="returnBill">部门信息</param>
        /// <param name="error">返回的错误信息</param>
        /// <returns>始终返回False</returns>
        bool SetReturnError1(object err, out IQueryable<View_Provider> returnBill, out string error)
        {
            returnBill = null;
            error = err.ToString();

            return false;
        }

        /// <summary>
        /// 获得责任人信息
        /// </summary>
        /// <param name="providerCode">责任人工号</param>
        /// <returns>返回责任人信息</returns>
        public DataTable GetProviderPrincipal(string providerCode)
        {
            string strSql = "select Name as 责任人,PrincipalWorkId as 责任人工号, IsMainDuty as 主要责任人,ID as 序号 "+
                            " from ProviderPrincipal as a " +
                            " inner join HR_Personnel as b on a.PrincipalWorkId = b.WorkID " +
                            " where a.Provider = '" + providerCode + "'";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得责任人信息
        /// </summary>
        /// <param name="providerCode">责任人工号</param>
        /// <returns>返回责任人信息</returns>
        public List<ProviderPrincipal> GetProviderPrincipalList(string providerCode)
        {
            using (DepotManagementDataContext ctx = CommentParameter.DepotDataContext)
            {
                var varData = from a in ctx.ProviderPrincipal
                              where a.Provider == providerCode
                              select a;

                return varData.ToList();
            }
        }
    }
}
