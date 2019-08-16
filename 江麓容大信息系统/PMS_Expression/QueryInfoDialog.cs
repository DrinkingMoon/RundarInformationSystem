using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServerModule;
using GlobalObject;
using PlatformManagement;
using System.Data;
using UniversalControlLibrary;

namespace Expression
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
    static class QueryInfoDialog
    {
        /// <summary>
        /// 0： 正常，1：仅限于返修箱用，2：样品领用, 3:售后备件领用；
        /// </summary>
        public static int flgOnlyForRepair = 0;

        /// <summary>
        /// 设计BOM诊断，主键为产品型号编码
        /// </summary>
        public static Dictionary<string, object> m_dicProductBom = new Dictionary<string, object>();

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

            if (BasicInfo.ListRoles.Contains(CE_RoleEnum.零件工程师.ToString()))
            {
                queryResult = queryResult.Where(p => p.供应商 == "SYS_CPKF");
            }
            else
            {
                queryResult = queryResult.Where(p => p.供应商 == CE_WorkShopCode.JJCJ.ToString());
            }

            System.Data.DataTable dataTable = GlobalObject.GeneralFunction.ConvertToDataTable<View_S_HomemadePartInfo>(queryResult);
            FormQueryInfo form = new FormQueryInfo(dataTable);
            //form.ShowColumns = new string[] { "图号型号", "零件名称", "规格" };
            return form;
        }

        /// <summary>
        /// 从BOM中获取指定产品版本的零件信息对话框
        /// </summary>
        /// <param name="isAllInfo">是否获取所有零件信息的标志</param>
        /// <param name="isAssemblyFlag">当isAllInfo为false时判断是获取总成还是零件信息的标志</param>
        /// <returns>成功则返回获取到的对话框，失败返回null</returns>
        static public FormQueryInfo GetAccessoryInfoDialog(string productEdition, bool isAllInfo, bool isAssemblyFlag)
        {
            List<View_P_ProductBom> bomInfo = null;
            IEnumerable<GoodsBasicInfo> queryResult = null;

            if (m_dicProductBom.ContainsKey(productEdition))
            {
                bomInfo = m_dicProductBom[productEdition] as List<View_P_ProductBom>;
            }
            else
            {
                IBomServer server = ServerModuleFactory.GetServerModule<IBomServer>();

                bomInfo = server.GetBom(productEdition).ToList();
                m_dicProductBom[productEdition] = bomInfo;
            }

            if (!isAllInfo)
            {
                if (isAssemblyFlag)
                {
                    queryResult = (from r in bomInfo
                                   where r.是否为总成 > 0 //&& (r.父总成编码 != null)
                                   select new GoodsBasicInfo { 图号型号 = r.零部件编码, 物品名称 = r.零部件名称, 规格 = r.规格 });
                }
                else
                {
                    queryResult = (from r in bomInfo
                                   where r.是否为总成 == 0
                                   orderby r.零部件编码, r.规格
                                   select new GoodsBasicInfo { 图号型号 = r.零部件编码, 物品名称 = r.零部件名称, 规格 = r.规格 }).Distinct();
                }
            }
            else
            {
                queryResult = (from r in bomInfo
                               orderby r.零部件编码, r.规格
                               select new GoodsBasicInfo { 图号型号 = r.零部件编码, 物品名称 = r.零部件名称, 规格 = r.规格 }).Distinct();
            }

            //FormQueryInfo form = new FormQueryInfo(queryResult);
            System.Data.DataTable dataTable = GlobalObject.GeneralFunction.ConvertToDataTable<GoodsBasicInfo>(queryResult);
            FormQueryInfo form = new FormQueryInfo(dataTable);
            //form.ShowColumns = new string[] { "图号型号", "物品名称", "规格" };
            return form;
        }

        /// <summary>
        /// 从BOM中获取指定工位的零件信息对话框
        /// </summary>
        /// <param name="workBench">工位</param>
        /// <returns>成功则返回获取到的对话框，失败返回null</returns>
        static public FormQueryInfo GetAccessoryWorkbench(string workBench)
        {
            string strSql = "select distinct PartCode 图号型号,PartName 物品名称,Spec 规格,Workbench 工位 " +
                         " from DepotManagement.dbo.P_AssemblingBom where Workbench='" + workBench + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            FormQueryInfo form = new FormQueryInfo(dt);

            return form;
        }

        /// <summary>
        /// 获取计划价格表中的物品信息对话框
        /// </summary>
        /// <returns>成功则返回获取到的对话框，失败返回null</returns>
        static public FormQueryInfo GetPlanCostGoodsDialog()
        {
            IBasicGoodsServer goodsServer = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();
            IQueryable<View_F_GoodsPlanCost> queryResult = goodsServer.GetGoodsInfo(false);
            System.Data.DataTable dataTable = GlobalObject.GeneralFunction.ConvertToDataTable<View_F_GoodsPlanCost>(queryResult);
            FormQueryInfo form = new FormQueryInfo(dataTable);

            form.ShowColumns = new string[] { "图号型号", "物品名称", "规格", "单位", "物品类别", "物品类别名称" };
            return form;
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
        //    IQueryable<View_F_GoodsPlanCost> queryResult = GetPlanCostGoods(userCode, lstDepotCode);

        //    //FormQueryInfo form = new FormQueryInfo(queryInfo);
        //    System.Data.DataTable dataTable = GlobalObject.GeneralFunction.ConvertToDataTable<View_F_GoodsPlanCost>(queryResult);
        //    FormQueryInfo form = new FormQueryInfo(dataTable);

        //    form.ShowColumns = new string[] { "图号型号", "物品名称", "规格", "单位", "物品类别", "物品类别名称" };
        //    return form;
        //}

        /// <summary>
        /// 获取单据信息对话框
        /// 登录名如果为null获取所有单据，否则获取指定用户单据
        /// </summary>
        /// <param name="loginID">工号，如果为null则可获取所有单据</param>
        /// <param name="billType">单据类型</param>
        /// <returns>成功则返回获取到的对话框，失败返回null</returns>
        static public FormQueryInfo GetMaterialRequisitionBillDialog(string loginID, CE_BillTypeEnum billType)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            DataTable dt = new DataTable();

            string name = UniversalFunction.GetPersonnelName(loginID);

            if (billType == CE_BillTypeEnum.领料单)
            {
                // 剔除了已经导入多批次信息表中的领料单
                IQueryable<View_ZPX_MultiBatchPart_LLD> result = null;

                if (!GlobalObject.GeneralFunction.IsNullOrEmpty(loginID))
                {
                    result = from r in ctx.View_ZPX_MultiBatchPart_LLD
                             where (r.编制人 == name || r.部门主管签名 == name) &&
                             r.单据状态 == MaterialRequisitionBillStatus.已出库.ToString()
                             select r;
                }
                else
                {
                    result = from r in ctx.View_ZPX_MultiBatchPart_LLD
                             where r.单据状态 == MaterialRequisitionBillStatus.已出库.ToString()
                             select r;
                }

                dt = GlobalObject.GeneralFunction.ConvertToDataTable<View_ZPX_MultiBatchPart_LLD>(result);

            }
            else if (billType == CE_BillTypeEnum.领料退库单)
            {
                // 剔除了已经导入多批次信息表中的领料单
                IQueryable<View_ZPX_MultiBatchPart_LTD> result = null;

                if (!GlobalObject.GeneralFunction.IsNullOrEmpty(loginID))
                {
                    result = from r in ctx.View_ZPX_MultiBatchPart_LTD
                             where (r.申请人 == name || r.部门主管签名 == name) &&
                             r.单据状态 == MaterialReturnedInTheDepotBillStatus.已完成.ToString()
                             select r;
                }
                else
                {
                    result = from r in ctx.View_ZPX_MultiBatchPart_LTD
                             where r.单据状态 == MaterialReturnedInTheDepotBillStatus.已完成.ToString()
                             select r;
                }

                dt = GlobalObject.GeneralFunction.ConvertToDataTable<View_ZPX_MultiBatchPart_LTD>(result);
            }
            else
            {
                throw new Exception("单据类型不正确");
            }

            FormQueryInfo form = new FormQueryInfo(dt);
            return form;
        }

        /// <summary>
        /// 获取营销出库单信息对话框(多批次管理用)
        /// </summary>
        /// <returns>成功则返回获取到的对话框，失败返回null</returns>
        static public FormQueryInfo GetOutboundBillDialog()
        {
            string strSql = " SELECT * FROM View_S_MarketingBill_V2 " +
                            " WHERE 业务方式 = '三包外返修出库' AND 单据状态 = '已确认'" +
                            " AND 单据号 NOT IN (SELECT BILL_ID FROM ZPX_MultiBatchPart_LLD)";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            FormQueryInfo form = new FormQueryInfo(dt);

            form.HideColumns = new string[] { "删除标志", "库房编码" };

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
            string strSql = string.Format(
                "SELECT distinct 图号型号,物品名称,规格,供货单位 FROM View_S_Stock where 图号型号='{0}' and 规格='{1}' ",
                goodsCode, spec);

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);
            
            FormQueryInfo form = new FormQueryInfo(dt);
            
            return form;
        }

        /// <summary>
        /// 获取出厂条形码生成规则信息对话框
        /// </summary>
        /// <returns></returns>
        static public FormQueryInfo GetBuildRuleForVehicleBarcode()
        {
            IProductBarcodeServer server = ServerModuleFactory.GetServerModule<IProductBarcodeServer>();
            IQueryable<View_P_BuildRuleForVehicleBarcode> result = server.GetBuildRule();

            FormQueryInfo form = new FormQueryInfo(result);
            return form;
        }

        /// <summary>
        /// 获取简易人员信息（工号、姓名、部门）
        /// </summary>
        /// <returns></returns>
        static public FormQueryInfo GetSimplePersonelInfo()
        {
            Service_Peripheral_HR.IPersonnelArchiveServer personnelServer = 
                Service_Peripheral_HR.ServerModuleFactory.GetServerModule<Service_Peripheral_HR.IPersonnelArchiveServer>();

            FormQueryInfo form = new FormQueryInfo(personnelServer.GetAllInfo());
            form.ShowColumns = new string[] { "员工编号", "员工姓名", "部门", "岗位" };
            return form;
        }

        /// <summary>
        /// 从电子档案、临时电子档案中获取零件信息（只有零件图号、名称、规格、批次号信息）
        /// </summary>
        /// <param name="beginDate">起始装配数据</param>
        /// <param name="endDate">截止装配数据</param>
        /// <returns>获取到的信息对话框</returns>
        static public FormQueryInfo GetPartInfoOfElectronFile(DateTime beginDate, DateTime endDate)
        {
            IElectronFileServer service = PMS_ServerFactory.GetServerModule<IElectronFileServer>();

            FormQueryInfo form = new FormQueryInfo(service.GetDistinctPartInfo(beginDate, endDate));
            
            return form;
        }

        /// <summary>
        /// 获取基础物料信息
        /// </summary>
        /// <returns>成功则返回获取到的对话框，失败返回null</returns>
        static public FormQueryInfo GetAllGoodsInfo()
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            IQueryable<View_F_GoodsPlanCost> result =
                from r in ctx.View_F_GoodsPlanCost
                select r;

            DataTable dt = GlobalObject.GeneralFunction.ConvertToDataTable<View_F_GoodsPlanCost>(result);

            FormQueryInfo form = new FormQueryInfo(dt);
            return form;
        }

        /// <summary>
        /// 获取全部阶段信息
        /// </summary>
        /// <returns>返回阶段信息</returns>
        static public FormQueryInfo GetAllPhase()
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            IQueryable<ZPX_OfflinePhaseSet> result =
                from r in ctx.ZPX_OfflinePhaseSet
                select r;

            DataTable dt = GlobalObject.GeneralFunction.ConvertToDataTable<ZPX_OfflinePhaseSet>(result);

            FormQueryInfo form = new FormQueryInfo(dt);
            return form;
        }
    }
}
