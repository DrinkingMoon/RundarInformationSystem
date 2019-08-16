using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServerModule;
using GlobalObject;
using PlatformManagement;
using System.Data;
using System.Collections;

namespace UniversalControlLibrary
{

    /// <summary>
    /// 物品基础信息
    /// </summary>
    class GoodsBasicInfo
    {
        public string 图号型号
        {
            get;
            set;
        }

        public string 物品名称
        {
            get;
            set;
        }

        public string 规格
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 查询信息对话框
    /// </summary>
    public static class QueryInfoDialog
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        static string m_err;

        /// <summary>
        /// 0： 正常，1：仅限于返修箱用，2：样品领用, 3:售后备件领用, 4:委外返修用；
        /// </summary>
        private static List<int> _lstStockStatus = new List<int>();

        public static List<int> LstStockStatus
        {
            get { return QueryInfoDialog._lstStockStatus; }
            set { QueryInfoDialog._lstStockStatus = value; }
        }

        /// <summary>
        /// 库存物品比较器
        /// </summary>
        class StockGoodsComparer : IEqualityComparer<View_S_Stock>
        {
            public bool Equals(View_S_Stock x, View_S_Stock y)
            {
                return x.图号型号.Equals(y.图号型号) && x.物品名称.Equals(y.物品名称) && x.规格.Equals(y.规格);
            }

            public int GetHashCode(View_S_Stock obj)
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取自制件信息对话框
        /// </summary>
        /// <returns>成功则返回获取到的对话框，失败返回null</returns>
        static public FormQueryInfo GetHomemadeAccessoryInfoDialog()
        {
            IHomemadePartInfoServer server = ServerModuleFactory.GetServerModule<IHomemadePartInfoServer>();
            IQueryable<View_S_HomemadePartInfo> queryResult = server.GetHomemadeAccessory();

            //if (BasicInfo.ListRoles.Contains(RoleEnum.零件工程师.ToString()))
            //{
            //    queryResult = queryResult.Where(p => p.供应商 == "SYS_CPKF");
            //}
            //else
            //{
            //    queryResult = queryResult.Where(p => p.供应商 == WorkShopCode.JJCJ.ToString());
            //}

            System.Data.DataTable dataTable = GlobalObject.GeneralFunction.ConvertToDataTable<View_S_HomemadePartInfo>(queryResult);
            FormQueryInfo form = new FormQueryInfo(dataTable);
            //form.ShowColumns = new string[] { "图号型号", "零件名称", "规格" };
            return form;
        }

        /// <summary>
        /// 获取库存中的物品信息对话框
        /// </summary>
        /// <param name="billType">单据类型</param>
        /// <returns>成功则返回获取到的对话框，失败返回null</returns>
        static public FormQueryInfo GetStoreGoodsInfoDialog(CE_BillTypeEnum billType, string strStorageID)
        {
            return GetStoreGoodsInfoDialog(billType, false, strStorageID);
        }

        /// <summary>
        /// 获取库存中的物品信息对话框
        /// </summary>
        /// <param name="billType">单据类型</param>
        /// <param name="showZeroStoreGoods">是否显示零库存物品</param>
        /// <returns>成功则返回获取到的对话框，失败返回null</returns>
        static public FormQueryInfo GetStoreGoodsInfoDialog(CE_BillTypeEnum billType, bool showZeroStoreGoods, string strStorageID)
        {
            IStoreServer server = ServerModuleFactory.GetServerModule<IStoreServer>();
            IQueryable<View_S_Stock> queryResult;
            System.Data.DataTable dataTable = null;

            if (!server.GetAllStore(null, true, out queryResult, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return null;
            }

            // 仅显示状态为正常且不在隔离库中的物品
            if (billType == CE_BillTypeEnum.领料单)
            {
                List<string> listTempStockStatus = new List<string>();
                listTempStockStatus.Add("正常");

                foreach (int statusID in _lstStockStatus)
                {
                    switch (statusID)
                    {
                        case 1:
                            listTempStockStatus.Add("仅限于返修箱用");
                            break;
                        case 2:
                            listTempStockStatus.Add("样品");
                            break;
                        case 3:
                            listTempStockStatus.Add("仅限于返修箱用");
                            listTempStockStatus.Add("仅限于售后备件");
                            break;
                        case 4:
                            listTempStockStatus.Add("隔离");
                            break;
                        default:
                            break;
                    }
                }

                queryResult = from r in queryResult
                              where listTempStockStatus.Contains(r.物品状态)
                              && r.库房代码 == strStorageID
                              select r;
            }
            else if (billType == CE_BillTypeEnum.领料退库单)
            {
                queryResult = from r in queryResult
                              where r.物品状态 != "隔离"
                              select r;
            }

            ////TCU 采购部领用委外装配 Modify by cjb on 2014.5.28
            //if (!GlobalObject.GeneralFunction.IsNullOrEmpty(prefixDepotType) && strStorageID != "03")
            //{
            //    int len = prefixDepotType.Length;
            //    queryResult = from r in queryResult where r.材料类别编码.Substring(0, len) == prefixDepotType select r;
            //}

            if (!showZeroStoreGoods)
            {
                queryResult = from r in queryResult where r.库存数量 > 0 select r;
            }

            FormQueryInfo form = null;

            if (!BasicInfo.ListRoles.Exists(r => r.Contains("库管理员")))
            {
                if (billType == CE_BillTypeEnum.领料单 && !_lstStockStatus.Contains(2) && !_lstStockStatus.Contains(4))
                {
                    var result = (from r in queryResult select r).ToList().Distinct(new StockGoodsComparer());
                    dataTable = GlobalObject.GeneralFunction.ConvertToDataTable<View_S_Stock>(result);
                }
                else
                {
                    dataTable = GlobalObject.GeneralFunction.ConvertToDataTable<View_S_Stock>(queryResult);
                }
            }
            else
            {
                dataTable = GlobalObject.GeneralFunction.ConvertToDataTable<View_S_Stock>(queryResult);
            }

            form = new FormQueryInfo(dataTable);

            if (!showZeroStoreGoods)
            {
                form.Text = form.Text + "，库存数量为0的物品不会显示在列表中。";
            }

            if (!BasicInfo.ListRoles.Exists(r => r.Contains("库管理员")))
            {
                if (billType == CE_BillTypeEnum.报废单 || billType == CE_BillTypeEnum.领料退库单)
                    form.HideColumns = new string[] { "序号", "物品ID", "单位", "单位ID", "库存数量", "仓库", "货架", "层", "列", "实际单价", "实际金额" };
                else if (billType == CE_BillTypeEnum.领料单)
                    form.HideColumns = new string[] { "序号", "物品ID", "单位", "实际单价", "实际金额" };
            }
            else
            {
                form.HideColumns = new string[] { "序号", "物品ID", "单位ID", "单位", "实际单价", "实际金额" };
            }

            return form;
        }

        /// <summary>
        /// 获取部门信息
        /// </summary>
        /// <returns>返回获取的部门信息</returns>
        static public FormQueryInfo GetDepartment()
        {
            string strSql = "select * from View_Department";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            FormQueryInfo form = new FormQueryInfo(dtTemp);

            return form;
        }

        /// <summary>
        /// 获取采购退货关联单据后的物品信息（0:订单退货，1:报废退货）
        /// </summary>
        /// <param name="orderFormNumber">订单号</param>
        /// <param name="provider">供应商</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>返回获得采购退货关联单据后的物品信息</returns>
        static public FormQueryInfo GetStoreGoodsInfoDialog(string orderFormNumber, string provider, string storageID)
        {
            FormQueryInfo form = null;
            string error = null;
            try
            {
                Hashtable hsTable = new Hashtable();

                hsTable.Add("@OrderFormNumber", orderFormNumber);
                hsTable.Add("@StorageID", storageID);
                hsTable.Add("@Provider", provider);

                DataTable dt = GlobalObject.DatabaseServer.QueryInfoPro("Get_MaterialRejectBill_GoodsInfo", hsTable, out error);
                form = new FormQueryInfo(dt);
                return form;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获取供应商信息对话框
        /// </summary>
        /// <returns>成功则返回获取到的对话框，失败返回null</returns>
        static public FormQueryInfo GetProviderInfoDialog()
        {
            IProviderServer m_providerServer = ServerModuleFactory.GetServerModule<IProviderServer>();
            IQueryable<View_Provider> queryResult;

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            if (!m_providerServer.GetAllProvider(out queryResult, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return null;
            }
            queryResult = from r in queryResult
                          where r.是否在用 == true
                          select r;

            if (!BasicInfo.ListRoles.Contains(CE_RoleEnum.采购账务管理员.ToString())
                && BasicInfo.ListRoles.Contains(CE_RoleEnum.采购员.ToString()))
            {
                var varData = (from r in ctx.View_Provider
                               join f in ctx.ProviderPrincipal
                               on r.供应商编码 equals f.Provider
                               where f.PrincipalWorkId == BasicInfo.LoginID
                               && r.是否在用 == true
                               select r).Distinct();

                List<View_Provider> lstProvider = new List<View_Provider>();

                foreach (var item in varData)
                {
                    View_Provider lnqProvider = new View_Provider();

                    lnqProvider.供应商编码 = item.供应商编码;
                    lnqProvider.供应商名称 = item.供应商名称;
                    lnqProvider.简称 = item.简称;
                    lnqProvider.拼音码 = item.拼音码;
                    lnqProvider.零星采购供应商 = item.零星采购供应商;
                    lnqProvider.是否在用 = item.是否在用;
                    lnqProvider.五笔码 = item.五笔码;

                    lstProvider.Add(lnqProvider);
                }

                queryResult = lstProvider.AsQueryable();
            }

            System.Data.DataTable dataTable = GlobalObject.GeneralFunction.ConvertToDataTable<View_Provider>(queryResult);
            FormQueryInfo form = new FormQueryInfo(dataTable);

            return form;
        }

        /// <summary>
        /// 获取供应商信息对话框
        /// </summary>
        /// <returns>成功则返回获取到的对话框，失败返回null</returns>
        static public FormQueryInfo GetProviderInfoDialog(string strType)
        {
            IProviderServer m_providerServer = ServerModuleFactory.GetServerModule<IProviderServer>();
            IQueryable<View_Provider> queryResult;

            if (!m_providerServer.GetAllProvider(out queryResult, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return null;
            }
            queryResult = from r in queryResult
                          where r.是否在用 == true
                          select r;
            switch (strType)
            {
                case "零件合格供应商":
                    queryResult = from r in queryResult
                                  where r.零星采购供应商 == false
                                  select r;
                    break;
                default:
                    break;
            }
            System.Data.DataTable dataTable = GlobalObject.GeneralFunction.ConvertToDataTable<View_Provider>(queryResult);
            FormQueryInfo form = new FormQueryInfo(dataTable);

            return form;
        }

        /// <summary>
        /// 获取库存中指定物品的供应商信息对话框
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="providerType">供应商类型</param>
        /// <returns>成功则返回获取到的对话框，失败返回null</returns>
        static public FormQueryInfo GetDistinctProviderInfoDialog(int goodsID, CE_ScrapProviderType providerType)
        {
            IProviderServer m_providerServer = ServerModuleFactory.GetServerModule<IProviderServer>();
            IQueryable<View_Provider> queryResult;
            System.Data.DataTable dataTable = new DataTable();

            switch (providerType)
            {
                case CE_ScrapProviderType.责任供应商:

                    if (!m_providerServer.GetDistinctProvider(out queryResult, out m_err))
                    {
                        MessageDialog.ShowErrorMessage(m_err);
                        return null;
                    }

                    dataTable = GlobalObject.GeneralFunction.ConvertToDataTable<View_Provider>(queryResult);

                    break;
                case CE_ScrapProviderType.供应商:

                    View_F_GoodsPlanCost viewGoods = UniversalFunction.GetGoodsInfo(goodsID);

                    string strSql = " select distinct a.* from View_Provider as a inner join ( " +
                                    " select distinct GoodsID, Provider from S_InDepotDetailBill as a " +
                                    " union all select distinct GoodsID, Provider from S_Stock) as b  " +
                                    " on a.供应商编码 = b.Provider inner join F_GoodsPlanCost as c on b.GoodsID = c.ID " +
                                    " where GoodsName = '" + viewGoods.物品名称 + "'";

                    dataTable = GlobalObject.DatabaseServer.QueryInfo(strSql);

                    break;
                default:
                    break;
            }

            FormQueryInfo form = new FormQueryInfo(dataTable);
            form.ShowColumns = new string[] { "供应商编码" };
            return form;
        }

        /// <summary>
        /// 获取合同信息对话框
        /// </summary>
        /// <returns>成功则返回获取到的对话框，失败返回null</returns>
        static public FormQueryInfo GetBargainInfoDialog()
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            IBargainInfoServer infoServer = ServerModuleFactory.GetServerModule<IBargainInfoServer>();
            //PlatformManagement.IQueryResult queryInfo;

            //if (!infoServer.GetAllBargainInfo(out queryInfo, out m_err))
            //{
            //    MessageDialog.ShowErrorMessage(m_err);
            //    return null;
            //}
            
            string strSql = " select distinct a.* from DepotManagement.dbo.View_B_BargainInfo as a "+
                            " inner join DepotManagement.dbo.ProviderPrincipal as b on a.供货单位 = b.Provider "+
                            " inner join DepotManagement.dbo.B_BargainInfo as c on a.合同号 = c.BargainNumber "+
                            " where c.IsDisable = 0 order by 日期 desc";

            FormQueryInfo form = new FormQueryInfo(GlobalObject.DatabaseServer.QueryInfo(strSql));
            form.ShowColumns = new string[] { "合同号", "供货单位", "采购员", "日期" };
            return form;
        }

        /// <summary>
        /// 获取指定合同的物品信息对话框
        /// </summary>
        /// <param name="bargainNumber">合同号</param>
        /// <param name="simpleMode">是否是简单模式的标志, 是则只显示最基本的物品信息列(图号、名称、规格)</param>
        /// <returns>成功则返回获取到的对话框，失败返回null</returns>
        static public FormQueryInfo GetBargainGoodsDialog(string bargainNumber, bool simpleMode)
        {
            IBargainGoodsServer infoServer = ServerModuleFactory.GetServerModule<IBargainGoodsServer>();
            IQueryable<View_B_BargainGoods> queryResult;

            if (!infoServer.GetBargainGoods(bargainNumber, out queryResult, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return null;
            }

            //FormQueryInfo form = new FormQueryInfo(queryInfo);
            System.Data.DataTable dataTable = GlobalObject.GeneralFunction.ConvertToDataTable<View_B_BargainGoods>(queryResult);
            FormQueryInfo form = new FormQueryInfo(dataTable);

            if (simpleMode)
            {
                form.ShowColumns = new string[] { "图号型号", "物品名称", "规格" };
            }

            return form;
        }

        /// <summary>
        /// 获取指定合同的物品信息对话框
        /// </summary>
        /// <param name="bargainNumber">合同号</param>
        /// <param name="simpleMode">是否是简单模式的标志, 是则只显示最基本的物品信息列(图号、名称、规格)</param>
        /// <returns>成功则返回获取到的对话框，失败返回null</returns>
        static public FormQueryInfo GetBargainGoodsMessageDialog(string bargainNumber, bool simpleMode)
        {
            string strSql = "select a.*,b.序号 as 物品ID,b.单位,b.物品类别, a.单价/(1+c.税率/100) as 实际单价 from View_B_BargainGoods as a  " +
                " inner join View_F_GoodsPlanCost as b on a.图号型号 = b.图号型号 and a.物品名称 = b.物品名称 and a.规格 = b.规格 " +
                " inner join View_B_BargainInfo as c on a.合同号 = c.合同号 where a.合同号 = '" + bargainNumber + "' ";
            DataTable dataTable = GlobalObject.DatabaseServer.QueryInfo(strSql);
            FormQueryInfo form = new FormQueryInfo(dataTable);

            if (simpleMode)
            {
                form.ShowColumns = new string[] { "物品ID", "图号型号", "物品名称", "规格", "单位", "物品类别", "实际单价" };
            }

            return form;
        }

        /// <summary>
        /// 获取指定合同的物品信息对话框
        /// </summary>
        /// <param name="bargainNumber">合同号</param>
        /// <param name="simpleMode">是否是简单模式的标志, 是则只显示最基本的物品信息列(图号、名称、规格)</param>
        /// <returns>成功则返回获取到的对话框，失败返回null</returns>
        static public FormQueryInfo GetBargainGoodsForOrderFormDialog(string bargainNumber, bool simpleMode)
        {
            string strSql = "select a.物品ID,a.图号型号,a.物品名称,a.规格,a.单价,a.数量,,c.采购份额 " +
                " from View_B_BargainGoods as a inner join View_B_GoodsLeastPackAndStock as c " +
                " on a.物品ID = c.物品ID inner join View_B_BargainInfo as d " +
                " on a.合同号 = d.合同号 and d.供货单位 = c.供应商 where a.合同号 = '" + bargainNumber + "'";

            System.Data.DataTable dataTable = GlobalObject.DatabaseServer.QueryInfo(strSql);
            FormQueryInfo form = new FormQueryInfo(dataTable);

            if (simpleMode)
            {
                form.ShowColumns = new string[] { "物品ID", "图号型号", "物品名称", "规格", "单价", "数量" };
            }

            return form;
        }

        /// <summary>
        /// 获取指定合同的物品信息对话框
        /// </summary>
        /// <param name="personnel">人员工号或姓名</param>
        /// <returns>成功则返回获取到的对话框，失败返回null</returns>
        static public FormQueryInfo GetBargainFormDialog(string personnel)
        {
            string strSql = "select * from View_B_BargainInfo where 1=1 ";

            if (personnel != "全部")
            {
                strSql += " and (采购员 = '" + personnel + "' or 合同录入员 = '" + personnel + "'";
            }

            System.Data.DataTable dataTable = GlobalObject.DatabaseServer.QueryInfo(strSql);
            FormQueryInfo form = new FormQueryInfo(dataTable);

            form.ShowColumns = new string[] { "合同号", "供货单位", "采购员", "税率", "日期", "供应商联系人", "联系方式", 
                    "合同录入员", "备注", "权限控制用登录名", "是否海外合同", "是否委外合同"};

            return form;
        }

        /// <summary>
        /// 获取订单信息对话框
        /// </summary>
        /// <param name="billType">单据类型</param>
        /// <returns>成功则返回获取到的对话框，失败返回null</returns>
        static public FormQueryInfo GetOrderFormInfoDialog(CE_BillTypeEnum billType)
        {
            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            IOrderFormInfoServer orderFormInfoServer = ServerModuleFactory.GetServerModule<IOrderFormInfoServer>();
            IQueryable<View_B_OrderFormInfo> queryResult;

            if (!orderFormInfoServer.GetAllOrderFormInfo(BasicInfo.ListRoles, BasicInfo.LoginID, billType, out queryResult, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return null;
            }

            System.Data.DataTable dataTable = GlobalObject.GeneralFunction.ConvertToDataTable<View_B_OrderFormInfo>(queryResult);
            FormQueryInfo form = new FormQueryInfo(dataTable);
            form.HideColumns = new string[] { "供应商联系人", "供应商联系电话", "供应商传真号", "供应商电子邮件", "权限控制用登录名", "录入人员" };
            return form;
        }

        /// <summary>
        /// 获取订单信息对话框(报废单关联)
        /// </summary>
        /// <param name="strProvider">供应商</param>
        /// <returns>成功则返回获取到的对话框，失败返回null</returns>
        static public FormQueryInfo GetOrderFormInfoDialog(string strProvider)
        {

            string strSql = "select * from View_B_OrderFormInfo where 供货单位 = '" + strProvider + "'";
            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            FormQueryInfo form = new FormQueryInfo(dt);
            form.HideColumns = new string[] { "供应商联系人", "供应商联系电话", "供应商传真号", "供应商电子邮件", "权限控制用登录名", "录入人员" };
            return form;
        }

        /// <summary>
        /// 获取非产品件检验单信息对话框
        /// </summary>
        /// <returns>成功则返回获取到的对话框，失败返回null</returns>
        static public FormQueryInfo GetUnProductTestingSingleDialog()
        {

            string strSql = "select * from View_ZL_UnProductTestingSingleBill where 单据状态 = '单据已完成'";
            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            FormQueryInfo form = new FormQueryInfo(dt);
            form.ShowColumns = new string[] { "单据号", "图号型号", "物品名称", "规格", "数量", "是否合格" };
            return form;
        }

        /// <summary>
        /// 获取指定订单的物品信息对话框
        /// </summary>
        /// <param name="orderFormNumber">订单号</param>
        /// <param name="simpleMode">是否是简单模式的标志, 是则只显示最基本的物品信息列(图号、名称、规格)</param>
        /// <returns>成功则返回获取到的对话框，失败返回null</returns>
        static public FormQueryInfo GetOrderFormGoodsDialog(string orderFormNumber, bool simpleMode)
        {
            IOrderFormGoodsServer orderFormGoodsServer = ServerModuleFactory.GetServerModule<IOrderFormGoodsServer>();
            IQueryable<View_B_OrderFormGoods> queryResult;

            if (!orderFormGoodsServer.GetOrderFormGoods(BasicInfo.ListRoles, BasicInfo.LoginID, orderFormNumber, out queryResult, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return null;
            }

            //FormQueryInfo form = new FormQueryInfo(queryInfo);
            System.Data.DataTable dataTable = GlobalObject.GeneralFunction.ConvertToDataTable<View_B_OrderFormGoods>(queryResult);
            FormQueryInfo form = new FormQueryInfo(dataTable);

            if (simpleMode)
            {
                form.ShowColumns = new string[] { "物品ID", "图号型号", "物品名称", "规格" };
            }

            return form;
        }

        ///// <summary>
        ///// 获取指定订单中属于仓库类别列表的物品信息对话框
        ///// </summary>
        ///// <param name="orderFormNumber">订单号</param>
        ///// <param name="simpleMode">是否是简单模式的标志, 是则只显示最基本的物品信息列(图号、名称、规格)</param>
        ///// <returns>成功则返回获取到的对话框，失败返回null</returns>
        //static public FormQueryInfo GetOrderFormGoodsDialog(string orderFormNumber, List<string> lstDepotType, bool simpleMode)
        //{
        //    IOrderFormGoodsServer orderFormGoodsServer = ServerModuleFactory.GetServerModule<IOrderFormGoodsServer>();
        //    IQueryable<View_B_OrderFormGoods> queryResult;

        //    if (!orderFormGoodsServer.GetOrderFormGoods(BasicInfo.ListRoles, BasicInfo.LoginID, orderFormNumber, out queryResult, out m_err))
        //    {
        //        MessageDialog.ShowErrorMessage(m_err);
        //        return null;
        //    }

        //    List<View_B_OrderFormGoods> orderFormGoods = queryResult.ToList();
        //    IQueryable<View_F_GoodsPlanCost> result = GetPlanCostGoods(null, lstDepotType);

        //    if (result != null || result.Count() == 0)
        //    {
        //        List<View_F_GoodsPlanCost> goodsInfo = result.ToList();

        //        for (int i = 0; i < orderFormGoods.Count; i++)
        //        {
        //            if (null == goodsInfo.Find(p => orderFormGoods[i].图号型号 == p.图号型号 && orderFormGoods[i].物品名称 == p.物品名称 && orderFormGoods[i].规格 == p.规格))
        //            {
        //                orderFormGoods.RemoveAt(i);
        //                i--;
        //            }
        //        }
        //    }


        //    System.Data.DataTable dataTable = GlobalObject.GeneralFunction.ConvertToDataTable(orderFormGoods);
        //    FormQueryInfo form = new FormQueryInfo(dataTable);

        //    if (simpleMode)
        //    {
        //        form.ShowColumns = new string[] { "图号型号", "物品名称", "规格" };
        //    }

        //    return form;
        //}

        /// <summary>
        /// 获取指定订单的物品在仓库中的信息对话框
        /// </summary>
        /// <param name="orderFormNumber">订单号</param>
        /// <returns>成功则返回获取到的对话框，失败返回null</returns>
        static public FormQueryInfo GetOrderFormGoodsStockInfoDialog(string orderFormNumber, string strStorageID)
        {
            IStoreServer server = ServerModuleFactory.GetServerModule<IStoreServer>();
            IQueryable<View_OrderFormGoodsStockInfo> queryResult = server.GetOrderFormGoodsStockInfo(orderFormNumber, strStorageID);
            //FormQueryInfo form = new FormQueryInfo(queryResult);
            System.Data.DataTable dataTable = GlobalObject.GeneralFunction.ConvertToDataTable<View_OrderFormGoodsStockInfo>(queryResult);
            FormQueryInfo form = new FormQueryInfo(dataTable);

            return form;
        }

        /// <summary>
        /// 获取计划价格表中的物品信息对话框
        /// </summary>
        /// <returns>成功则返回获取到的对话框，失败返回null</returns>
        static public FormQueryInfo GetPlanCostGoodsDialog()
        {
            return GetPlanCostGoodsDialog(false);
        }

        /// <summary>
        /// 获取计划价格表中的物品信息对话框
        /// </summary>
        /// <returns>成功则返回获取到的对话框，失败返回null</returns>
        static public FormQueryInfo GetPlanCostGoodsDialog(bool blUsing)
        {
            IBasicGoodsServer goodsServer = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();
            IQueryable<View_F_GoodsPlanCost> queryResult = goodsServer.GetGoodsInfo(blUsing);
            System.Data.DataTable dataTable = GlobalObject.GeneralFunction.ConvertToDataTable<View_F_GoodsPlanCost>(queryResult);
            FormQueryInfo form = new FormQueryInfo(dataTable);

            form.ShowColumns = new string[] { "图号型号", "物品名称", "规格", "单位", "物品类别", "物品类别名称", "序号" };
            return form;
        }

        /// <summary>
        /// 获取计划价格表中的物品信息对话框
        /// </summary>
        /// <returns>成功则返回获取到的对话框，失败返回null</returns>
        static public FormQueryInfo GetOrderFormGoodsDialog(string provider)
        {
            IBasicGoodsServer goodsServer = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();

            using (DepotManagementDataContext ctx = CommentParameter.DepotDataContext)
            {
                IQueryable<View_F_GoodsPlanCost> queryResult = (from a in ctx.View_F_GoodsPlanCost
                                                               join b in ctx.Bus_PurchasingMG_UnitPriceList
                                                               on a.序号 equals b.GoodsID
                                                               where b.Provider == provider
                                                               && b.ValidityStart <= ServerTime.Time
                                                               && b.ValidityEnd >= ServerTime.Time
                                                               && a.禁用 == false
                                                               orderby a.图号型号, a.物品名称, a.规格
                                                               select a).Distinct();

                FormQueryInfo form = new FormQueryInfo(queryResult);

                form.ShowColumns = new string[] { "图号型号", "物品名称", "规格", "单位", "物品类别", "物品类别名称", "序号" };
                return form;
            }
        }

        ///// <summary>
        ///// 获取指定用户计划价格表中的物品信息对话框，用户编码为null时获取所有信息
        ///// </summary>
        ///// <param name="userCode">用户编码</param>
        ///// <returns>成功则返回获取到的对话框，失败返回null</returns>
        //static public FormQueryInfo GetPlanCostGoodsDialog(string userCode)
        //{
        //    return GetPlanCostGoodsDialog(userCode, null);
        //}

        ///// <summary>
        ///// 获取指定用户计划价格表中的物品信息，用户编码为null时获取所有信息
        ///// </summary>
        ///// <param name="userCode">用户编码</param>
        ///// <param name="lstDepotCode">仓库类别列表</param>
        ///// <returns>成功则返回获取到的对话框，失败返回null</returns>
        //static private IQueryable<View_F_GoodsPlanCost> GetPlanCostGoods(string userCode, List<string> lstDepotCode)
        //{
        //    IBasicGoodsServer goodsServer = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();
        //    IQueryable<View_F_GoodsPlanCost> queryResult = goodsServer.GetGoodsInfo(userCode, lstDepotCode);
        //    return queryResult;
        //}

        ///// <summary>
        ///// 获取指定用户计划价格表中的物品信息对话框，用户编码为null时获取所有信息
        ///// </summary>
        ///// <param name="userCode">用户编码</param>
        ///// <param name="lstDepotCode">仓库类别列表</param>
        ///// <returns>成功则返回获取到的对话框，失败返回null</returns>
        //static public FormQueryInfo GetPlanCostGoodsDialog(string userCode, List<string> lstDepotCode)
        //{
        //    IBasicGoodsServer goodsServer = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();
        //    IQueryable<View_F_GoodsPlanCost> queryResult = goodsServer.GetGoodsInfo(userCode, lstDepotCode);
        //    System.Data.DataTable dataTable = GlobalObject.GeneralFunction.ConvertToDataTable<View_F_GoodsPlanCost>(queryResult);
        //    FormQueryInfo form = new FormQueryInfo(dataTable);

        //    form.ShowColumns = new string[] { "图号型号", "物品名称", "规格", "单位", "物品类别", "物品类别名称", "序号" };
        //    return form;
        //}

        /// <summary>
        /// 获取指定用户计划价格表中的物品信息对话框，用户编码为null时获取所有信息
        /// </summary>
        /// <param name="userCode">用户编码</param>
        /// <param name="lstDepotCode">仓库类别列表</param>
        /// <returns>成功则返回获取到的对话框，失败返回null</returns>
        static public FormQueryInfo GetPlanCostGoodsDialogSift(string strWhere)
        {
            IBasicGoodsServer goodsServer = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();
            System.Data.DataTable dataTable = goodsServer.GetGoodsInfoSiftAttribute(strWhere);
            FormQueryInfo form = new FormQueryInfo(dataTable);

            form.ShowColumns = new string[] { "图号型号", "物品名称", "规格", "单位", "物品类别", "物品类别名称", "序号" };
            return form;
        }

        /// <summary>
        /// 为领料而获取报废单信息对话框
        /// </summary>
        /// <returns>成功则返回获取到的对话框，失败返回null</returns>
        static public FormQueryInfo GetScrapBillDialogForFetchGoods()
        {
            IScrapBillServer scrapBillServer = ServerModuleFactory.GetServerModule<IScrapBillServer>();
            PlatformManagement.IQueryResult queryInfo;

            if (!scrapBillServer.GetAllBillForFetchGoods(out queryInfo, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return null;
            }

            FormQueryInfo form = new FormQueryInfo(queryInfo);
            form.ShowColumns = new string[] { "报废单号", "报废时间", "报废类别", "报废原因", "是否冲抵领料单", "申请人签名", "仓管签名" };
            return form;
        }

        /// <summary>
        /// 为领料而获取报废单信息对话框
        /// </summary>
        /// <returns>成功则返回获取到的对话框，失败返回null</returns>
        static public FormQueryInfo GetProductCodeStockSearchMode(string strsql)
        {
            string strSql = "select a.ProductCode as 箱体编号,ProductStatus as 总成状态, b.GoodsCode as 图号型号,b.GoodsName as 物品名称," +
                " b.Spec as 规格,c.StorageName as 库房名称,a.GoodsID as 产品ID,c.StorageID as 库房ID" +
                " from ProductStock  as a inner join (select b.* from F_GoodsAttributeRecord as a inner join F_GoodsPlanCost as b on a.GoodsID = b.ID " +
                " where AttributeID in (" + (int)CE_GoodsAttributeName.CVT + ", " + (int)CE_GoodsAttributeName.TCU + ") and  AttributeValue = '" + bool.TrueString + "') as b " +
                " on a.GoodsID = b.ID inner join BASE_Storage as c on a.StorageID = c.StorageID ";

            strSql += strsql;

            FormQueryInfo form = new FormQueryInfo(GlobalObject.DatabaseServer.QueryInfo(strSql));
            return form;
        }

        /// <summary>
        /// 为领料而获取报废单信息对话框(根据供应商)
        /// </summary>
        /// <returns>成功则返回获取到的对话框，失败返回null</returns>
        static public FormQueryInfo GetScrapBillDialogForFetchGoods(string strProvider)
        {
            string strSql = " select distinct a.* from View_S_ScrapBill as a inner join " +
                            " View_S_ScrapGoods as b on a.报废单号 = b.报废单号 " +
                            " where b.供应商= '" + strProvider + "'";
            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);
            FormQueryInfo form = new FormQueryInfo(dt);
            form.ShowColumns = new string[] { "报废单号", "报废时间", "报废类别", "报废原因", "是否冲抵领料单", "申请人签名", "仓管签名" };
            return form;
        }

        /// <summary>
        /// 为领料而获取领料退库单信息对话框
        /// </summary>
        /// <returns>成功则返回获取到的对话框，失败返回null</returns>
        static public FormQueryInfo GetReturnedInDepotBillDialogForFetchGoods()
        {
            IMaterialReturnedInTheDepot billServer = ServerModuleFactory.GetServerModule<IMaterialReturnedInTheDepot>();
            PlatformManagement.IQueryResult queryInfo;

            if (!billServer.GetAllBillForFetchGoods(out queryInfo, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return null;
            }

            FormQueryInfo form = new FormQueryInfo(queryInfo);
            form.ShowColumns = new string[] { "退库单号", "退库时间", "申请部门", "申请人", "初始用途", "退库原因", "备注" };
            return form;
        }

        /// <summary>
        /// 为领料而获取三包外返修处置单信息对话框
        /// </summary>
        /// <returns>成功则返回获取到的对话框，失败返回null</returns>
        static public FormQueryInfo GetThreePacketsOfTheRepairForFetchGoods()
        {
            string strSql = "select * from View_YX_ThreePacketsOfTheRepairBill where 单据状态 <> '等待领料明细申请'";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            FormQueryInfo form = new FormQueryInfo(dtTemp);
            form.ShowColumns = new string[] { "单据号", "单据状态", "产品型号", "箱体编号", "客户名称" };
            return form;
        }

        /// <summary>
        /// 获取领料单信息对话框(多批次管理用)
        /// </summary>
        /// <returns>成功则返回获取到的对话框，失败返回null</returns>
        static public FormQueryInfo GetMaterialRequisitionBillDialog()
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            // 剔除了已经导入多批次信息表中的领料单
            IQueryable<View_ZPX_MultiBatchPart_LLD> result =
                from r in ctx.View_ZPX_MultiBatchPart_LLD
                where (r.编制人 == BasicInfo.LoginName || r.部门主管签名 == BasicInfo.LoginName) &&
                       r.单据状态 == MaterialRequisitionBillStatus.已出库.ToString()
                select r;

            DataTable dt = GlobalObject.GeneralFunction.ConvertToDataTable<View_ZPX_MultiBatchPart_LLD>(result);

            FormQueryInfo form = new FormQueryInfo(dt);
            return form;
        }

        /// <summary>
        /// 根据物品名称列出该物品的所有批次号等信息
        /// </summary>
        /// <returns>成功则返回获取到的对话框，失败返回null</returns>
        static public FormQueryInfo GetGoodsForBatchNo(int intGoodsID, string strStorageID)
        {
            string strSql = " SELECT a.*,b.ID as 物品状态ID FROM View_S_Stock as a inner join S_StockStatus as b on " +
                            " a.物品状态 = b. Description where 物品ID = " + intGoodsID +
                            " and 库房代码 = '" + strStorageID + "'";
            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);
            FormQueryInfo form = new FormQueryInfo(dt);
            form.ShowColumns = new string[] { "图号型号", "物品名称", "规格", "供货单位","供方批次号", "批次号",
                "库存数量","单位", "物品状态","货架","列","层","所属库房","物品ID","单位ID","物品状态ID","实际单价"};
            return form;
        }

        /// <summary>
        /// 获取指定物品的所有供应商信息
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="spec">规格</param>
        /// <returns>成功则返回获取到的对话框，失败返回null</returns>
        static public FormQueryInfo GetProviderFromGoods(string goodsCode, string spec)
        {
            string strSql = string.Format(" SELECT distinct 图号型号,物品名称,规格,供货单位 FROM View_S_Stock where 图号型号='{0}' and 规格='{1}' ", goodsCode, spec);
            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);
            FormQueryInfo form = new FormQueryInfo(dt);
            return form;
        }

        /// <summary>
        /// 函电信息
        /// </summary>
        /// <param name="endTime">结束时间</param>
        /// <param name="starTime">起始时间</param>
        /// <returns>成功返回满足条件的数据集，失败返回null的dataTable</returns>
        static public FormQueryInfo GetAfterServiceBill(string starTime, string endTime)
        {
            string sql = @"select 单据号,单据状态,信息来源,接函电人,接函电时间," +
                      " 用户姓名,变速箱型号,变速箱编号,车架号,接单处理人, 接单时间, " +
                      " 诊断及测试情况, 处理方案, 处理结果, 审核人, 审核回访时间, 回访人," +
                      " 回访时间 from dbo.View_S_AfterService where 1=1";

            if (starTime != "" && endTime != "")
            {
                sql += @" and 接函电时间>='" + starTime + "' and 接函电时间<'" + endTime + "'";
            }

            if ((BasicInfo.ListRoles.Contains(CE_RoleEnum.营销主管.ToString()) || BasicInfo.ListRoles.Contains(CE_RoleEnum.营销分管领导.ToString()))
               && !BasicInfo.ListRoles.Contains(CE_RoleEnum.业务系统管理员.ToString()))
            {
                sql += " and (单据状态 = '等待审核' or 单据状态 = '处理完成')";
            }

            else if (BasicInfo.ListRoles.Contains(CE_RoleEnum.营销普通人员.ToString()) &&
                !(BasicInfo.ListRoles.Contains(CE_RoleEnum.营销主管.ToString()) || BasicInfo.ListRoles.Contains(CE_RoleEnum.营销分管领导.ToString()))
                && !BasicInfo.ListRoles.Contains(CE_RoleEnum.业务系统管理员.ToString()) && !BasicInfo.ListRoles.Contains(CE_RoleEnum.营销售后客户回访人员.ToString()))
            {
                sql += " and (接函电人='" + BasicInfo.LoginName + "' or 接单处理人='" + BasicInfo.LoginName + "')";
            }

            else if (BasicInfo.ListRoles.Contains(CE_RoleEnum.营销售后客户回访人员.ToString()))
            {
                sql += " and (单据状态='等待回访')";
            }

            sql += " order by 单据号 desc";

            DataTable dt = GlobalObject.DatabaseServer.WebQueryInfo(sql);
            FormQueryInfo form = new FormQueryInfo(dt);

            form.ShowColumns = new string[] { "单据号", "单据状态", "信息来源", "接函电人", "接函电时间", "用户姓名", "变速箱型号", 
                                             "变速箱编号", "车架号", "接单处理人", "接单时间","诊断及测试情况", "处理方案", "处理结果" ,
                                              "审核人" , "审核回访时间" , "回访人" , "回访时间" };

            return form;
        }

        /// <summary>
        /// 售后质量反馈信息
        /// </summary>
        /// <returns>成功返回满足条件的数据集，失败返回null的dataTable</returns>
        static public FormQueryInfo GetFeedBackBill(string starTime, string endTime)
        {
            string sql = @"select 反馈单号,服务站名,CVT型号," +
                      "  CVT编号,车型,故障说明,责任部门, 责任人, " +
                      " 变速箱状态, 诊断及测试情况, 处理措施, 完成要求, 库存产品处理意见, 临时措施," +
                      " 原因分析,永久性措施,落实情况,部门编码 from " +
                      " dbo.View_S_ServiceFeedBack where 单据状态 = '单据完成'";

            sql += " order by 反馈单号 desc";

            DataTable dt = GlobalObject.DatabaseServer.WebQueryInfo(sql);
            FormQueryInfo form = new FormQueryInfo(dt);

            form.ShowColumns = new string[] { "反馈单号", "服务站名", "CVT型号", " CVT编号", 
                                             "车型","故障说明", "责任部门", "责任人","变速箱状态", "诊断及测试情况", "处理措施" ,
                                              "完成要求" , "库存产品处理意见" , "临时措施" , "回访原因分析时间" ,"永久性措施","部门编码"};

            return form;
        }

        /// <summary>
        /// 获得安全库存信息
        /// </summary>
        /// <returns>返回数据集</returns>
        static public FormQueryInfo GetSafeStock()
        {
            ISafeStockServer server = ServerModuleFactory.GetServerModule<ISafeStockServer>();
            DataTable dt = server.GetAllInfo();

            FormQueryInfo form = new FormQueryInfo(dt);
            return form;
        }

        /// <summary>
        /// 获得供应商配额信息
        /// </summary>
        /// <returns>返回数据集</returns>
        static public FormQueryInfo GetGoodsLeastPackAndStock()
        {
            IGoodsLeastPackAndStock server = ServerModuleFactory.GetServerModule<IGoodsLeastPackAndStock>();
            DataTable dt = server.GetAllInfo();

            FormQueryInfo form = new FormQueryInfo(dt);
            return form;
        }

        /// <summary>
        /// 采购计划计算公式
        /// </summary>
        /// <returns>返回Table</returns>
        static public FormQueryInfo GetProcurementMath()
        {

            IPurcharsingPlan server = ServerModuleFactory.GetServerModule<IPurcharsingPlan>();
            DataTable dt = server.GetProcurementMath();

            FormQueryInfo form = new FormQueryInfo(dt);
            return form;
        }

        /// <summary>
        /// 获取会议资源对话框
        /// </summary>
        /// <param name="beginTime">资源占用开始时间</param>
        /// <param name="endTime">资源占用结束时间</param>
        /// <returns>成功则返回获取到的对话框，失败返回null</returns>
        static public FormQueryInfo GetMeetingResourceDialog(DateTime beginTime, DateTime endTime)
        {
            TaskManagementServer.IResourceServer resourceServer = TaskManagementServer.TaskObjectFactory.GetOperator<TaskManagementServer.IResourceServer>();

            var queryInfo = resourceServer.GetMeetingResource(beginTime, endTime);

            FormQueryInfo form = new FormQueryInfo(queryInfo);

            return form;
        }

        /// <summary>
        /// 获取工装台帐
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <returns>成功则返回获取到的对话框，失败返回null</returns>
        static public FormQueryInfo GetFrockStandingBook(int goodsID)
        {
            string strSql = "select * from View_S_FrockStandingBook where 1 = 1 ";

            if (goodsID != 0)
            {
                strSql += " and 物品ID = " + goodsID;
            }

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            FormQueryInfo form = new FormQueryInfo(dtTemp);

            return form;
        }

        /// <summary>
        /// 获取工装台帐
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <returns>成功则返回获取到的对话框，失败返回null</returns>
        static public FormQueryInfo GetFrockProvingReport(int goodsID)
        {
            string strSql = "select * from View_S_FrockProvingReport where 1 = 1";

            if (goodsID != 0)
            {
                strSql += " and 物品ID = " + goodsID;
            }

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            FormQueryInfo form = new FormQueryInfo(dtTemp);

            return form;
        }

        /// <summary>
        /// 获取零件信息
        /// </summary>
        /// <returns>成功则返回获取到的对话框，失败返回null</returns>
        static public FormQueryInfo GetAllGoodsInfo()
        {
            string strSql = "select 序号,图号型号,物品名称,规格,单位" +
                            " from View_F_GoodsPlanCost where 禁用 = 0";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            FormQueryInfo form = new FormQueryInfo(dtTemp);

            return form;
        }

        /// <summary>
        /// 获取已完成的零星采购单
        /// </summary>
        /// <param name="purchaseType">采购类型</param>
        /// <returns>成功则返回获取到的对话框，失败返回null</returns>
        static public FormQueryInfo GetMinorPurchaseInfo(string purchaseType)
        {
            string strSql = "select Distinct 单据号, 单据状态,采购类别,是否紧急,申请部门,申请人,申请日期,采购工程师,备注" +
                            " from dbo.View_B_MinorPurchaseBill as a inner join B_MinorPurchaseList as b on a.单据号 = b.BillNo where GoodsStatus = '已到货' ";

            if (purchaseType != "")
            {
                strSql += " and 采购类别 = '" + purchaseType + "'";
            }

            strSql += " order by 单据号 desc";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            FormQueryInfo form = new FormQueryInfo(dtTemp);

            return form;
        }

        /// <summary>
        /// 获取未完成的零星采购申请单
        /// </summary>
        /// <returns>成功则返回获取到的对话框，失败返回null</returns>
        static public FormQueryInfo GetMinorPurchaseInfo()
        {
            string strSql = "select 单据号, 单据状态,采购类别,申请人,申请部门,申请日期,是否紧急,采购工程师,备注" +
                            " from dbo.View_B_MinorPurchaseBill where 单据状态 <> '已完成' ";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            FormQueryInfo form = new FormQueryInfo(dtTemp);

            return form;
        }

        /// <summary>
        /// 通过单据号获取零星采购明细
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>成功则返回获取到的对话框，失败返回null</returns>
        static public FormQueryInfo GetMinorPurchaseList(string billNo)
        {
            string strSql = "select 图号型号, 物品名称,规格,申请数量,单位,物品状态,备注" +
                            " from dbo.View_B_MinorPurchaseList where 编号 = '" + billNo + "' ";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            FormQueryInfo form = new FormQueryInfo(dtTemp);

            return form;
        }

        /// <summary>
        /// 获得车间批次库存
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="wsCode">车间代码</param>
        /// <returns>成功则返回获取到的对话框，失败返回null</returns>
        static public FormQueryInfo GetWorkShopBatchNoInfo(int goodsID, string wsCode)
        {
            string strSql = "select * from View_WS_WorkShopStock where 库存数量 > 0 and 物品ID = "
                + goodsID + " and 车间代码 = '" + wsCode + "'";

            FormQueryInfo form = new FormQueryInfo(GlobalObject.DatabaseServer.QueryInfo(strSql));

            form.ShowColumns = new string[] { "批次号", "库存数量", "单位", "备注" };
            return form;
        }
    }
}
