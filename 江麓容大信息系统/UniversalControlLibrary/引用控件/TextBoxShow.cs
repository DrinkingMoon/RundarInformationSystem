/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  TextBoxShow.cs
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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GlobalObject;

namespace UniversalControlLibrary
{
    /// <summary>
    /// 数据快速检索控件
    /// </summary>
    public partial class TextBoxShow : TextBox, IDataGridViewEditingControl
    {

        bool _IsMultiSelect = false;

        public bool IsMultiSelect
        {
            get { return _IsMultiSelect; }
            set { _IsMultiSelect = value; }
        }

        /// <summary>
        /// 位置对象
        /// </summary>
        Point pt;

        /// <summary>
        /// 显示查询结果窗体
        /// </summary>
        bool m_blIsShow = true;

        /// <summary>
        /// 要检索的类别
        /// </summary>
        public enum FindType
        {
            人员, 员工, 部门, 客户, 顾客购买信息, 库房, 供应商, 订单信息, 入库申请单,出库申请单,
            BOM表零件, 装车信息, 营销物品, 
            工装编号, 量检具编号, 总成编码库存, 
            所有物品, 所有物品库存合计, 所有物品批次, 库房物品信息,
            自制件批次, 样品库房, 样品库房批次,
            总成不合格物品, 三包外返修零件,外部库存信息,
            全系统库房信息,营销与售后库房信息,售后服务站点信息,最低定价客户,主机厂零件,最低销售定价,
            体系文件类别,体系文件,报废单,营销退库单,车间耗用物品,车间耗用物品批次号,
            车间借贷物品, 车间借贷物品批次号,文件类别, 部门与库房, 自定义, 整车厂信息,
            核算科目, 领用用途, 车型代号,培训课程, 预算科目, 设计BOM产品清单, 采购BOM产品清单, 装配BOM产品清单, 发料清单产品清单
        }

        /// <summary>
        /// SQL语句
        /// </summary>
        private string strEndSql;

        public string StrEndSql
        {
            get { return strEndSql; }
            set { strEndSql = value; }
        }

        DataTable _dataTableResult = null;

        public DataTable DataTableResult
        {
            get { return _dataTableResult; }
            set { _dataTableResult = value; }
        }


        /// <summary>
        /// 返回结果集
        /// </summary>
        private DataRow _dataResult = null;

        public DataRow DataResult
        {
            get { return _dataResult; }
            set { _dataResult = value; }
        }
        
        #region 成员变量

        /// <summary>
        /// 检索的类别
        /// </summary>
        private FindType m_findType;

        /// <summary>
        /// 数据检索窗体
        /// </summary>
        FrmShowList show_list = new FrmShowList();

        /// <summary>
        /// 执行的SQL语句
        /// </summary>
        public string strSql;

        /// <summary>
        /// 要显示的字段集
        /// </summary>
        public string[] _sZstring;

        /// <summary>
        /// 拼音码列名
        /// </summary>
        public string strPyColunm;

        /// <summary>
        /// 五笔码列名
        /// </summary>
        public string strWbColunm;

        /// <summary>
        /// 图号
        /// </summary>
        public string strCodeColunm;

        /// <summary>
        /// 显示在TextBox中的数据值
        /// </summary>
        public string strShowMessage;

	    #endregion

        #region 属性

        /// <summary>
        /// 显示查询结果窗体
        /// </summary>
        public bool ShowResultForm
        {
            get { return m_blIsShow; }
            set { m_blIsShow = value; }
        }

        /// <summary>
        /// 获取或设置要检索的数据类别
        /// </summary>
        public FindType FindItem
        {
            get { return m_findType; }

            set
            {
                m_findType = value;

                switch (m_findType)
                {
                    case FindType.入库申请单:
                        InitInPutRequesitionInfo();
                        break;
                    case FindType.出库申请单:
                        InitOutPutRequesitionInfo();
                        break;
                    case FindType.文件类别:
                        InitFileSort();
                        break;
                    case FindType.订单信息:
                        InitOrderFormInfo();
                        break;
                    case FindType.全系统库房信息:
                        InitAllStorageSearchMode();
                        break;
                    case FindType.人员:
                        InitPersonSearchMode();
                        break;
                    case FindType.员工:
                        InitPersonnelArchive();
                        break;
                    case FindType.部门:
                        InitDeptSearchMode();
                        break;
                    case FindType.客户:
                        InitClientSearchMode();
                        break;
                    case FindType.顾客购买信息:
                        InitCVTCustomerInfo();
                        break;
                    case FindType.库房:
                        InitStorageSearchMode();
                        break;
                    case FindType.营销与售后库房信息:
                        InitStorageAndStationSearchMode();
                        break;
                    case FindType.售后服务站点信息:
                        InitServiceStationSearchMode();
                        break;
                    case FindType.供应商:
                        InitProviderSearchMode();
                        break;
                    case FindType.BOM表零件:
                        InitBomGoods();
                        break;
                    case FindType.装车信息:
                        InitCVTInfo();
                        break;
                    case FindType.营销物品:
                        InitYXGoodsSearchMode();
                        break;
                    case FindType.工装编号:
                        InitFrockNumber();
                        break;
                    case FindType.量检具编号:
                        InitGaugeNumber();
                        break;
                    case FindType.总成编码库存:
                        InitProductCodeStockSearchMode();
                        break;
                    case FindType.所有物品:
                        InitAllGoodsSearchMode();
                        break;
                    case FindType.外部库存信息:
                        InitOutStockGoodsSearchMode();
                        break;
                    case FindType.所有物品批次:
                        InitAllBatchNoSearchMode();
                        break;
                    case FindType.所有物品库存合计:
                        InitAllStockGoodsSearchMode();
                        break;
                    case FindType.库房物品信息:
                        InitStockGoodsSearchInfo();
                        break;
                    case FindType.自制件批次:
                        InitHomemadeRejectBatchNo();
                        break;
                    case FindType.样品库房:
                        InitMusterStockSearchMode();
                        break;
                    case FindType.样品库房批次:
                        InitMusterStockBatchNoSearchMode();
                        break;
                    case FindType.总成不合格物品:
                        InitBadQualifiedSearchMode();
                        break;
                    case FindType.三包外返修零件:
                        InitThreePacketsOfTheRepairGoods();
                        break;
                    case FindType.最低定价客户:
                        InitLowestPriceClient();
                        break;
                    case FindType.主机厂零件:
                        InitClientGoods();
                        break;
                    case FindType.最低销售定价:
                        InitGoodsLowestPrice();
                        break;
                    case FindType.体系文件类别:
                        InitSystemFileSort();
                        break;
                    case FindType.体系文件:
                        InitFile();
                        break;
                    case FindType.报废单:
                        InitScrapBill();
                        break;
                    case FindType.营销退库单:
                        InitMarketingBill();
                        break;
                    case FindType.车间耗用物品:
                        InitWorkShopGoods();
                        break;
                    case FindType.车间耗用物品批次号:
                        InitWorkShopGoodsBatchNo();
                        break;
                    case FindType.车间借贷物品:
                        InitWorkShopLendGoods();
                        break;
                    case FindType.车间借贷物品批次号:
                        InitWorkShopLendGoodsBatchNo();
                        break;
                    case FindType.部门与库房:
                        InitDepartmentAndStorage();
                        break;
                    case FindType.自定义:
                        InitCustom();
                        break;
                    case FindType.整车厂信息:
                        InitAutomobileCompanyInfo();
                        break;
                    case FindType.领用用途:
                        InitMaterialRequisitionPurposeInfo();
                        break;
                    case FindType.核算科目:
                        InitFinanceSubjectsInfo();
                        break;
                    case FindType.车型代号:
                        InitCarModelInfo();
                        break;
                    case FindType.培训课程:
                        InitCourseInfoSearchMode();
                        break;
                    case FindType.预算科目:
                        InitFinanceBudgetInfo();
                        break;
                    case FindType.设计BOM产品清单:
                        InitDesignBomProductInfo();
                        break;
                    case FindType.采购BOM产品清单:
                        InitPurchaseBomProductInfo();
                        break;
                    case FindType.装配BOM产品清单:
                        InitAssembleeBomProductInfo();
                        break;
                    case FindType.发料清单产品清单:
                        InitSendBomProductInfo();
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 获取指定名称的数据
        /// </summary>
        /// <param name="name">数据名称</param>
        /// <returns>成功返回名称对应的数据, 失败返回null</returns>
        public object this[String name]
        {
            get
            {
                if (!_IsMultiSelect)
                {

                    if (_dataResult == null || !_dataResult.Table.Columns.Contains(name))
                    {
                        return null;
                    }

                    return _dataResult[name];
                }
                else
                {
                    if (_dataTableResult == null || !_dataTableResult.Columns.Contains(name))
                    {
                        return null;
                    }

                    string showText = "";
                    foreach (DataRow dr in _dataTableResult.Rows)
                    {
                        showText += dr[name].ToString() + "\r\n";
                    }

                    return showText.Trim();
                }
            }
        }

        /// <summary>
        /// 完成检索后触发的事件
        /// </summary>
        public event GlobalObject.DelegateCollection.NonArgumentHandle OnCompleteSearch;

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public TextBoxShow()
        {
            InitializeComponent();
            this.TabStop = false;
        }

        #region  模式初始化


        void InitSendBomProductInfo()
        {
            strSql = " select * from select distinct b.GoodsCode as 图号型号, b.GoodsName as 物品名称, b.Spec as 规格, b.ID as 物品ID " +
                     " from BASE_ProductOrder as a inner join F_GoodsPlanCost as b on a.Edition = b.GoodsCode) as a where 1=1 ";

            _sZstring = new string[] { "图号型号", "物品名称", "规格", "物品ID" };
            strPyColunm = "图号型号";
            strWbColunm = "物品名称";
            strCodeColunm = "物品ID";
            strShowMessage = "图号型号";
        }

        void InitAssembleeBomProductInfo()
        {
            string error = null;

            DataTable tempTable = GlobalObject.DatabaseServer.QueryInfoPro("MES_InitAssembleeBomProductInfo", null, out error);

            strSql = tempTable.Rows[0][0].ToString();

            _sZstring = new string[] { "图号型号", "物品名称", "规格", "物品ID" };
            strPyColunm = "图号型号";
            strWbColunm = "物品名称";
            strCodeColunm = "物品ID";
            strShowMessage = "图号型号";
        }

        void InitPurchaseBomProductInfo()
        {
            strSql = " select * from ( select distinct b.GoodsCode as 图号型号, b.GoodsName as 物品名称, b.Spec as 规格, b.ID as 物品ID "+
                     " from CG_CBOM as a inner join F_GoodsPlanCost as b on a.Edition = b.GoodsCode) as a where 1=1 ";

            _sZstring = new string[] { "图号型号", "物品名称", "规格", "物品ID" };
            strPyColunm = "图号型号";
            strWbColunm = "物品名称";
            strCodeColunm = "物品ID";
            strShowMessage = "图号型号";
        }

        void InitDesignBomProductInfo()
        {
            strSql = " select * from ( select distinct c.GoodsCode as 图号型号, c.GoodsName as 物品名称, c.Spec as 规格, c.ID as 物品ID "+
                     " from (select ParentID from BASE_BomStruct) as a  "+
                     " left join (select GoodsID from BASE_BomStruct) as b on a.ParentID = b.GoodsID "+
                     " inner join F_GoodsPlanCost as c on a.ParentID = c.ID where b.GoodsID is null) as a where 1=1 ";

            _sZstring = new string[] { "图号型号", "物品名称", "规格", "物品ID" };
            strPyColunm = "图号型号";
            strWbColunm = "物品名称";
            strCodeColunm = "物品ID";
            strShowMessage = "图号型号";
        }

        void InitCarModelInfo()
        {
            strSql = " select * from View_TCU_CarModelInfo where 禁用 = 0";

            _sZstring = new string[] { "主机厂", "车型代号", "车型类型", "主机厂编码" };
            strPyColunm = "车型代号";
            strWbColunm = "主机厂";
            strCodeColunm = "主机厂编码";
            strShowMessage = "车型代号";
        }

        void InitInPutRequesitionInfo()
        {
            strSql = " select BillNo as 单据号, BillType as 单据类型, ApplyingDepartment as 申请部门, "+
                " DeptCode as 申请部门编码 from Business_WarehouseInPut_Requisition as a  "+
                " inner join HR_Dept as b on a.ApplyingDepartment = b.DeptCode where 1=1";

            _sZstring = new string[] { "单据号", "单据类型", "申请部门", "申请部门编码" };
            strPyColunm = "BillNo";
            strWbColunm = "BillType";
            strCodeColunm = "BillNo";
            strShowMessage = "单据号";
        }

        void InitOutPutRequesitionInfo()
        {
            strSql = " select BillNo as 单据号, BillType as 单据类型, ApplyingDepartment as 申请部门, " +
                " DeptCode as 申请部门编码 from Business_WarehouseOutPut_Requisition as a  " +
                " inner join HR_Dept as b on a.ApplyingDepartment = b.DeptCode where 1=1";

            _sZstring = new string[] { "单据号", "单据类型", "申请部门", "申请部门编码" };
            strPyColunm = "BillNo";
            strWbColunm = "BillType";
            strCodeColunm = "BillNo";
            strShowMessage = "单据号";
        }

        void InitOrderFormInfo()
        {
            strSql = " select 订单号, 合同号, 供货单位, 录入人员, 订货日期 from View_B_OrderFormInfo  where 1=1";

            _sZstring = new string[] { "订单号", "合同号", "供货单位", "录入人员", "订货日期" };
            strPyColunm = "订单号";
            strWbColunm = "合同号";
            strCodeColunm = "供货单位";
            strShowMessage = "订单号";
        }

        /// <summary>
        /// 获取文件类别
        /// </summary>
        private void InitFileSort()
        {
            strSql = " select *  from (select SortID as 文件类别ID, SortName as 文件类别名称, ParentID as 父级文件类别ID, FileType as 文件类型ID "+
                     " from FM_FileSort) as a where 1=1";

            _sZstring = new string[] { "文件类别ID", "文件类别名称", "父级文件类别ID", "文件类型ID"};
            strPyColunm = "文件类别名称";
            strWbColunm = "文件类别名称";
            strCodeColunm = "文件类别ID";
            strShowMessage = "文件类别名称";
        }

        /// <summary>
        /// 获取车间借贷物品
        /// </summary>
        private void InitWorkShopLendGoods()
        {
            strSql = " select distinct 物品ID, 图号型号,物品名称,规格,单位,拼音码 from ( " +
                     " select a.物品ID,a.图号型号,a.物品名称,a.规格,SUM(a.数量) as 数量, b.单位, 借方代码, 贷方代码, b.拼音码 " +
                     " from View_S_ProductLendRecord as a inner join View_F_GoodsPlanCost as b on a.物品ID = b.序号 " +
                     " group by 借方代码,贷方代码,a.物品ID,a.图号型号,a.物品名称,a.规格,b.拼音码, b.单位) as a where 1=1 ";

            _sZstring = new string[] { "物品ID", "图号型号", "物品名称", "规格", "数量", "单位", "拼音码" };
            strPyColunm = "拼音码";
            strWbColunm = "物品名称";
            strCodeColunm = "图号型号";
            strShowMessage = "图号型号";
        }

        /// <summary>
        /// 获取车间物品批次号
        /// </summary>
        private void InitWorkShopLendGoodsBatchNo()
        {
            strSql = "select distinct 批次号, 供应商, 数量 from View_S_ProductLendRecord where 1=1 ";
            _sZstring = new string[] { "批次号", "供应商", "数量" };
            strPyColunm = "批次号";
            strWbColunm = "批次号";
            strCodeColunm = "批次号";
            strShowMessage = "批次号";
        }

        /// <summary>
        /// 获取车间物品
        /// </summary>
        private void InitWorkShopGoods()
        {
            strSql = " select distinct 物品ID, 图号型号,物品名称,规格,单位,拼音码 from ( "+
                     " select a.物品ID,a.图号型号,a.物品名称,a.规格,SUM(a.库存数量) as 库存数量, b.单位, 车间名称, 车间代码, b.拼音码 "+
                     " from View_WS_WorkShopStock as a inner join View_F_GoodsPlanCost as b on a.物品ID = b.序号 "+
                     " group by 车间代码, 车间名称,a.物品ID,a.图号型号,a.物品名称,a.规格,b.拼音码, b.单位) as a where 库存数量 > 0 ";

            _sZstring = new string[] { "物品ID", "图号型号", "物品名称", "规格", "库存数量", "拼音码" };
            strPyColunm = "拼音码";
            strWbColunm = "物品名称";
            strCodeColunm = "图号型号";
            strShowMessage = "图号型号";
        }

        /// <summary>
        /// 获取车间物品批次号
        /// </summary>
        private void InitWorkShopGoodsBatchNo()
        {
            strSql = "select distinct 批次号,库存数量 from View_WS_WorkShopStock where 1=1 ";
            _sZstring = new string[] { "批次号", "库存数量" };
            strPyColunm = "批次号";
            strWbColunm = "批次号";
            strCodeColunm = "批次号";
            strShowMessage = "批次号";
        }

        /// <summary>
        /// 获取营销退库单
        /// </summary>
        private void InitMarketingBill()
        {
            strSql = "select DJH as 单据号, YWFS as 入库方式, ObjectDept as 客户与部门, LRRY as 录入人员, lrks as 录入部门, DJZT_FLAG as 单据状态 from dbo.View_S_MarketingBill where YWLX='退库'";
            _sZstring = new string[] { "单据号", "YWFS", "ObjectDept", "LRRY", "lrks", "DJZT_FLAG" };
            strPyColunm = "DJH";
            strWbColunm = "DJH";
            strCodeColunm = "DJH";
            strShowMessage = "单据号";
        }

        /// <summary>
        /// 获取报废单
        /// </summary>
        private void InitScrapBill()
        {
            strSql = "select * from dbo.View_S_ScrapBill where 1=1";
            _sZstring = new string[] { "报废单号", "报废时间", "单据报废类型", "申请人签名", "申请部门", "单据状态", "备注" };
            strPyColunm = "报废单号";
            strWbColunm = "报废单号";
            strCodeColunm = "报废单号";
            strShowMessage = "报废单号";
        }

        /// <summary>
        /// 获取体系文件
        /// </summary>
        private void InitFile()
        {
            strSql = " select * from dbo.View_FM_FileInfo where 1=1";
            _sZstring = new string[] { "文件编号", "文件名称", "版本号", "文件类别", "归口部门", "路径", "文件ID", "类别ID", "部门编号" };
            strPyColunm = "文件编号";
            strWbColunm = "文件名称";
            strCodeColunm = "文件ID";
            strShowMessage = "文件编号";
        }

        /// <summary>
        /// 最低销售定价
        /// </summary>
        private void InitGoodsLowestPrice()
        {
            strSql = " select 主机厂, 主机厂图号型号, 主机厂物品名称, 最低定价, 终端价格 from dbo.View_YX_LowestMarketPrice where 1=1";
            _sZstring = new string[] { "主机厂图号型号", "主机厂图号型号" };
            strPyColunm = "主机厂图号型号";
            strWbColunm = "主机厂图号型号";
            strCodeColunm = "主机厂图号型号";
            strShowMessage = "主机厂图号型号";
        }

        /// <summary>
        /// 主机厂零件
        /// </summary>
        private void InitClientGoods()
        {
            strSql = "select ID, 主机厂, 主机厂图号型号, 主机厂物品名称, 主机厂编码 from View_YX_GoodsSystemMatchingCommunicate where 1=1";
            _sZstring = new string[] { "主机厂", "主机厂图号型号", "主机厂物品名称" };
            strPyColunm = "主机厂图号型号";
            strWbColunm = "主机厂图号型号";
            strCodeColunm = "主机厂图号型号";
            strShowMessage = "主机厂图号型号";
        }

        /// <summary>
        /// 最低定价客户
        /// </summary>
        private void InitLowestPriceClient()
        {
            strSql = " select distinct(主机厂),ClientID 客户编码 from dbo.View_YX_LowestMarketPrice where 1=1";
            _sZstring = new string[] { "主机厂", "ClientID" };
            strPyColunm = "主机厂";
            strWbColunm = "主机厂";
            strCodeColunm = "ClientID";
            strShowMessage = "主机厂";
        }

        /// <summary>
        /// 体系文件查询模式
        /// </summary>
        private void InitSystemFileSort()
        {
            strSql = @" select a.SortName as 类别名称,b.SortName as 父级类别名称, a.SortID as 类别ID,  b.SortID as 父级类别ID
                            from FM_FileSort as a left join FM_FileSort as b on a.ParentID = b.SortID where 1=1";
            _sZstring = new string[] { "类别名称", "父级类别名称", "类别ID", "父级类别ID" };
            strPyColunm = "a.SortName";
            strWbColunm = "a.SortName";
            strCodeColunm = "a.SortID";
            strShowMessage = "类别名称";
        }

        /// <summary>
        /// 员工查询模式
        /// </summary>
        private void InitPersonnelArchive()
        {
            strSql = "select 员工编号,员工姓名,部门,岗位,拼音,五笔 from View_HR_PersonnelArchive where 1=1";
            _sZstring = new string[] { "员工编号", "员工姓名", "部门", "岗位" };
            strPyColunm = "拼音";
            strWbColunm = "员工编号";
            strCodeColunm = "员工姓名";
            strShowMessage = "员工姓名";
        }

        /// <summary>
        /// 初始化全系统库房信息查询模式
        /// </summary>
        private void InitAllStorageSearchMode()
        {
            strSql = "select StorageID as 编码,StorageName as 名称 from (select StorageName,StorageID from dbo.BASE_Storage "+
	                 " union all select SecStorageName,SecStorageID from Out_StockInfo) as a  where 1=1";
            _sZstring = new string[] { "编码", "名称" };
            strPyColunm = "StorageID";
            strWbColunm = "StorageID";
            strCodeColunm = "StorageName";
            strShowMessage = "名称";
        }

        /// <summary>
        /// 初始化人员查询模式
        /// </summary>
        private void InitPersonSearchMode()
        {
            strSql = "select WorkID as 工号, Name as 姓名, Dept as 部门, WorkPost as 职位 from HR_Personnel where 1=1 and DeleteFlag = 0";
            _sZstring = new string[] { "工号", "姓名", "部门", "职位" };
            strPyColunm = "PY";
            strWbColunm = "WorkID";
            strCodeColunm = "Name";
            strShowMessage = "姓名";
        }

        /// <summary>
        /// 初始化部门查询模式
        /// </summary>
        private void InitDeptSearchMode()
        {
            strSql = "select DeptCode as 部门编码,DeptName as 部门名称 from HR_Dept where DeleteFlag = 0 ";
            _sZstring = new string[] { "部门编码", "部门名称" };
            strPyColunm = "DeptCode";
            strWbColunm = "DeptCode";
            strCodeColunm = "DeptName";
            strShowMessage = "部门名称";
        }

        /// <summary>
        /// 初始化客户查询模式
        /// </summary>
        private void InitClientSearchMode()
        {
            strSql = "select * from View_Client where 1=1";
            _sZstring = new string[] { "客户编码", "客户名称","联系电话","详细地址","联系人", "备注" };
            strPyColunm = "客户编码";
            strWbColunm = "客户编码";
            strCodeColunm = "客户名称";
            strShowMessage = "客户编码";
        }

        /// <summary>
        /// 顾客购买信息
        /// </summary>
        private void InitCVTCustomerInfo()
        {
            strSql = "select 客户名称, 联系电话, 详细地址, CVT编号, CVT型号, 车架号, 销售日期, 车型, PY, WB from dbo.view_YX_CVTCustomerInformation where 1=1";
            _sZstring = new string[] { "客户名称", "联系电话", "详细地址", "CVT编号", "CVT型号", "车架号", "销售日期", "车型" };
            strPyColunm = "PY";
            strWbColunm = "WB";
            strCodeColunm = "客户名称";
            strShowMessage = "客户名称";
        }

        /// <summary>
        /// 初始化供应商查询模式
        /// </summary>
        private void InitProviderSearchMode()
        {
            strSql = "select ProviderCode as 供应商编码,ProviderName as 供应商名称,ShortName as 简称,PY,WB from  Provider where 1 = 1 ";
            _sZstring = new string[] { "供应商编码", "供应商名称", "简称" };
            strPyColunm = "PY";
            strWbColunm = "ProviderCode";
            strCodeColunm = "ProviderName";
            strShowMessage = "供应商编码";
        }

        /// <summary>
        /// 初始化库房基础信息查询模式
        /// </summary>
        private void InitStorageSearchMode()
        {
            strSql = "select StorageID as 库房编码,StorageName as 库房名称,StorageLv as 库房级别 from BASE_Storage where 1=1";
            _sZstring = new string[] { "库房编码", "库房名称", "库房级别" };
            strPyColunm = "StorageID";
            strWbColunm = "StorageID";
            strCodeColunm = "StorageName";
            strShowMessage = "库房名称";
        }

        /// <summary>
        /// 初始化售后服务站点信息查询模式
        /// </summary>
        private void InitServiceStationSearchMode()
        {
            strSql = "select ClientCode as 编码,ClientName as 名称 from Client where IsSecStorage=1";
            _sZstring = new string[] { "编码", "名称" };
            strPyColunm = "ClientCode";
            strWbColunm = "ClientCode";
            strCodeColunm = "ClientName";
            strShowMessage = "名称";
        }

        /// <summary>
        /// 初始化库房基础信息查询模式
        /// </summary>
        private void InitStorageAndStationSearchMode()
        {
            strSql = "select SecStorageID as 编码,SecStorageName as 名称 from Out_StockInfo where 1=1";
            _sZstring = new string[] { "编码", "名称" };
            strPyColunm = "SecStorageID";
            strWbColunm = "SecStorageID";
            strCodeColunm = "SecStorageName";
            strShowMessage = "名称";
        }

        /// <summary>
        /// BOM表零件
        /// </summary>
        private void InitBomGoods()
        {
            strSql = "  select distinct * from (select GoodsID as 物品ID, b.图号型号 as 零部件编码,b.物品名称 as 零部件名称," +
                     " b.规格, Version as 版次号, b.拼音码 as PY, b.五笔码 as WB from BASE_BomPartsLibrary as a inner join " +
                     " View_F_GoodsPlanCost as b on a.GoodsID = b.序号 ) as a where 1 = 1  ";
            _sZstring = new string[] { "物品ID", "零部件编码", "零部件名称", "规格", "版次号" };
            strPyColunm = "PY";
            strWbColunm = "零部件编码";
            strCodeColunm = "零部件名称";
            strShowMessage = "零部件编码";
        }

        /// <summary>
        /// 装车信息
        /// </summary>
        private void InitCVTInfo()
        {
            strSql = "select 车架号,CVT型号,CVT编号,车型号 from View_YX_LoadingInfo where 1=1";
            _sZstring = new string[] { "车架号", "CVT型号", "CVT编号", "车型号" };
            strPyColunm = "PY";
            strWbColunm = "WB";
            strCodeColunm = "车架号";
            strShowMessage = "车架号";
        }

        /// <summary>
        /// 初始化营销物品查询模式
        /// </summary>
        private void InitYXGoodsSearchMode()
        {
            strSql = "  select * from (select 序号, 图号型号, 物品名称, 规格, 单位, 单价, 物品类别, a.拼音码 as py, a.五笔码 as wb, a.Code as 厂商编码 "+
                        " from (select c.*, case when b.AttributeValue is null then '' else b.AttributeValue end as Code  "+
                        " from (select GoodsID from F_GoodsAttributeRecord where "+
                        " AttributeID in (" + (int)CE_GoodsAttributeName.CVT + ", " + (int)CE_GoodsAttributeName.TCU + ") and  AttributeValue = '" + bool.TrueString + "') as a   " +
                        " inner join View_F_GoodsPlanCost as c on a.GoodsID = c.序号 "+
                        " left join (select GoodsID, AttributeValue from F_GoodsAttributeRecord   "+
                        " where AttributeID = " + (int)CE_GoodsAttributeName.厂商编码 + ") as b on a.GoodsID = b.GoodsID) as a) as a where 1 = 1  ";
            _sZstring = new string[] { "图号型号", "物品名称", "物品类别", "规格", "厂商编码", "单位", "单价" };
            strPyColunm = "物品名称";
            strWbColunm = "图号型号";
            strCodeColunm = "厂商编码";
            strShowMessage = "物品名称";
        }

        /// <summary>
        /// 工装编号
        /// </summary>
        private void InitFrockNumber()
        {
            strSql = "select 工装编号, 工装图号, 工装名称 from View_S_FrockStandingBook where 1=1";
            _sZstring = new string[] { "工装编号", "工装图号", "工装名称" };
            strPyColunm = "工装编号";
            strWbColunm = "工装编号";
            strCodeColunm = "工装编号";
            strShowMessage = "工装编号";
        }

        /// <summary>
        /// 量检具编号
        /// </summary>
        private void InitGaugeNumber()
        {
            strSql = "select 量检具编号 from View_S_GaugeStandingBook where 1=1";
            _sZstring = new string[] { "量检具编号" };
            strPyColunm = "量检具编号";
            strWbColunm = "量检具编号";
            strCodeColunm = "量检具编号";
            strShowMessage = "量检具编号";
        }

        /// <summary>
        /// 初始化营销总成编号库存信息查询模式
        /// </summary>
        private void InitProductCodeStockSearchMode()
        {
            strSql = "select a.ProductCode as 箱体编号,ProductStatus as 总成状态, b.图号型号,b.物品名称," +
                " b.规格,c.StorageName as 库房名称,a.GoodsID as 产品ID,c.StorageID as 库房ID" +
                " from ProductStock  as a inner join (select c.* from (select GoodsID from F_GoodsAttributeRecord "+
                " where AttributeID in (" + (int)CE_GoodsAttributeName.CVT + ", " 
                + (int)CE_GoodsAttributeName.TCU + ") and  AttributeValue = '" + bool.TrueString + "') as a   " +
                " inner join View_F_GoodsPlanCost as c on a.GoodsID = c.序号) as b " +
                " on a.GoodsID = b.序号 inner join BASE_Storage as c on a.StorageID = c.StorageID where 1=1";

            _sZstring = new string[] { "箱体编号", "总成状态", "图号型号", "物品名称", "规格", "产品ID" };
            strPyColunm = "a.ProductCode";
            strWbColunm = "a.ProductCode";
            strCodeColunm = "a.ProductCode";
            strShowMessage = "箱体编号";
        }

        /// <summary>
        /// 初始化培训课程查询模式
        /// </summary>
        private void InitAllGoodsSearchMode()
        {
            strSql = "select 序号,图号型号, 物品名称, 规格,单位,单价,物品类别,拼音码,五笔码 from  View_F_GoodsPlanCost where  1= 1 ";
            _sZstring = new string[] { "图号型号", "物品名称", "物品类别", "规格", "单位", "拼音码", "五笔码" };
            strPyColunm = "拼音码";
            strWbColunm = "图号型号";
            strCodeColunm = "序号";
            strShowMessage = "图号型号";
        }

        /// <summary>
        /// 初始化所有物品查询模式
        /// </summary>
        private void InitCourseInfoSearchMode()
        {
            strSql = "select * from View_HR_Train_Course where  1= 1 ";
            _sZstring = new string[] { "课程名", "课程类型", "评估方式", "外训", "推荐讲师", "预计经费", "课程ID" };
            strPyColunm = "课程名";
            strWbColunm = "课程类型";
            strCodeColunm = "评估方式";
            strShowMessage = "课程名";
        }

        /// <summary>
        /// 初始化外部库存物品查询模式
        /// </summary>
        private void InitOutStockGoodsSearchMode()
        {
            strSql = "select 序号,图号型号, 物品名称, 规格,StockQty as 库存数量,StorageName as 账务库房, 单位,物品类别,拼音码,五笔码, c.StorageID as 账务库房ID from  View_F_GoodsPlanCost as a inner join " +
                " Out_Stock as b on a.序号 = b.GoodsID inner join Base_Storage as c on b.StorageID = c.StorageID where  1= 1 ";
            _sZstring = new string[] { "图号型号", "物品名称", "物品类别", "规格", "账务库房", "库存数量", "单位", "账务库房ID" };
            strPyColunm = "拼音码";
            strWbColunm = "图号型号";
            strCodeColunm = "序号";
            strShowMessage = "物品名称";
        }

        /// <summary>
        /// 初始化所有物品批次查询模式
        /// </summary>
        private void InitAllBatchNoSearchMode()
        {
            strSql = " select b.图号型号, b.物品名称, b.规格, 批次号,库存数量, " +
                     " 供货单位 as 供应商编码, 入库时间, " +
                     " b.单位, 实际单价 as 单价, b.物品类别, 拼音码, 五笔码, a.物品ID " +
                     " from View_S_Stock as a inner join View_F_GoodsPlanCost as b " +
                     " on a.物品ID = b.序号  where 1=1 ";
            _sZstring = new string[] { "图号型号", "物品名称", "规格", "批次号", "库存数量", "供应商编码", 
                "入库时间", "单位", "单价", "物品类别" };
            strPyColunm = "批次号";
            strWbColunm = "批次号";
            strCodeColunm = "批次号";
            strShowMessage = "批次号";
        }

        /// <summary>
        /// 初始化所有库存物品查询模式
        /// </summary>
        private void InitAllStockGoodsSearchMode()
        {
            strSql = " select 序号,图号型号,物品名称,规格,库存数量,单位, 物品类别,库房代码,拼音码,五笔码" +
                     " from ( select 物品ID,库房代码,Sum(库存数量) as 库存数量 from View_S_Stock Group by 物品ID,库房代码) as a " +
                     " inner join View_F_GoodsPlanCost as b on a.物品ID = b.序号 where 1 = 1 ";

            _sZstring = new string[] { "序号", "图号型号", "物品名称", "规格", "库存数量", "单位" };
            strPyColunm = "拼音码";
            strWbColunm = "图号型号";
            strCodeColunm = "序号";
            strShowMessage = "物品名称";
        }

        /// <summary>
        /// 获得库房物品信息查询模式
        /// </summary>
        private void InitStockGoodsSearchInfo()
        {
            strSql = " select 物品ID, b.图号型号, b.物品名称, b.规格, 批次号, b.单位,库存数量, b.拼音码 as PY, b.五笔码 as WB from View_S_Stock as a " +
                     " inner join View_F_GoodsPlanCost as b on a.物品ID = b.序号  where 1=1";
            _sZstring = new string[] { "图号型号", "物品名称", "规格", "批次号" };
            strPyColunm = "b.拼音码";
            strWbColunm = "b.图号型号";
            strCodeColunm = "物品ID";
            strShowMessage = "图号型号";
        }

        /// <summary>
        /// 自制件批次查询模式
        /// </summary>
        private void InitHomemadeRejectBatchNo()
        {
            strSql = "select distinct providerbatchno 供方批次,a.BatchNo 批次号,goodscode 图号型号,goodsname 物品名称" +
                            " from S_Stock a where ExistCount>0  ";

            _sZstring = new string[] { "供方批次", "批次号", "图号型号", "物品名称" };
            strPyColunm = "a.BatchNo";
            strWbColunm = "a.BatchNo";
            strCodeColunm = "a.BatchNo";
            strShowMessage = "批次号";
        }

        /// <summary>
        /// 初始化样品库房信息查询模式
        /// </summary>
        private void InitMusterStockSearchMode()
        {
            strSql = "select * from (select  GoodsID as 物品ID,b.物品名称,b.图号型号,b.规格,Sum(Count) as 库存数 ,b.单位, b.拼音码 as PY, b.五笔码 as WB, StrorageID as 库房代码 " +
                    " from S_MusterStock as a inner join View_F_GoodsPlanCost as b on a.GoodsID = b.序号 " +
                    " Group by GoodsID,b.物品名称,b.图号型号,b.规格,b.单位,StrorageID,b.拼音码,b.五笔码 ) as a   where 1=1 ";
            _sZstring = new string[] { "物品ID", "物品名称", "图号型号", "规格", "库存数", "单位" };
            strPyColunm = "PY";
            strWbColunm = "物品名称";
            strCodeColunm = "图号型号";
            strShowMessage = "图号型号";
        }

        /// <summary>
        /// 初始化样品库房批次信息查询模式
        /// </summary>
        private void InitMusterStockBatchNoSearchMode()
        {
            strSql = " select distinct BatchNo as 批次号, GoodsVersion as 版次号 from ( "+
                     " select GoodsID, BatchNo ,Version as GoodsVersion from S_MusterAffirmBill "+
                     " Union all select Purchase_GoodsID, Purchase_BatchNo, Purchase_Version  "+
                     " from Business_Sample_ConfirmTheApplication) as a "+
                     " where 1 = 1 and BatchNo <> '' ";

            _sZstring = new string[] { "批次号", "版次号" };
            strPyColunm = "a.BatchNo";
            strWbColunm = "a.BatchNo";
            strCodeColunm = "a.BatchNo";
            strShowMessage = "批次号";
        }

        /// <summary>
        /// 初始化总成不合格物品信息查询模式
        /// </summary>
        private void InitBadQualifiedSearchMode()
        {
            strSql = @"select GoodsID as 序号, ProductCode as 箱体编号,ProductStatus as 总成状态, b.图号型号,b.物品名称,
                 b.规格,b.物品类别,b.单价 from ProductStock as a inner join View_F_GoodsPlanCost as b  
                 on a.GoodsID = b.序号 where (ProductStatus in ('入库','退库','领料退库','调入'))";
            _sZstring = new string[] { "箱体编号", "总成状态", "图号型号", "物品名称", "规格" };
            strPyColunm = "ProductCode";
            strWbColunm = "ProductCode";
            strCodeColunm = "ProductCode";
            strShowMessage = "物品名称";
        }

        /// <summary>
        /// 初始化三包外物品单价查询模式
        /// </summary>
        private void InitThreePacketsOfTheRepairGoods()
        {
            strSql = " select a.GoodsID as 序号, b.图号型号, b.物品名称 , b.规格, " +
                     " b.单位,a.UnitPrice as 单价,b.五笔码 as wb,b.拼音码 as PY from YX_ThreePacketsOfTheRepairGoodsUnitPrice as a " +
                     " inner join View_F_GoodsPlanCost as b on a.GoodsID = b.序号 where 1 = 1 ";
            _sZstring = new string[] { "序号", "图号型号", "物品名称", "规格", "单价", "单位" };
            strPyColunm = "PY";
            strWbColunm = "b.GoodsCode";
            strCodeColunm = "b.GoodsName";
            strShowMessage = "物品名称";
        }

        /// <summary>
        /// 部门与库房
        /// </summary>
        private void InitDepartmentAndStorage()
        {
            strSql = "select * from View_BASE_DeptAndStorage where 1=1 ";
            _sZstring = new string[] { "部门编码", "部门名称" };
            strPyColunm = "部门编码";
            strWbColunm = "部门编码";
            strCodeColunm = "部门编码";
            strShowMessage = "部门名称";
        }

        void InitAutomobileCompanyInfo()
        {
            strSql = " select * from ( select ID as 序号, FactoryShortName as 名称, FactoryCode as 编码, "+
                " PYCode as 拼音码 from Base_MotorFactoryInfo) as a where 1=1 ";
            _sZstring = new string[] { "名称", "编码", "拼音码" };
            strPyColunm = "拼音码";
            strWbColunm = "名称";
            strCodeColunm = "编码";
            strShowMessage = "名称";
        }

        void InitFinanceSubjectsInfo()
        {
            strSql = " select * from ( select a.SubjectsCode as 科目代码, a.SubjectsName as 科目名称, b.SubjectsName as 父级科目 "+
                     " from (select * from Business_Base_FinanceSubjects Union all select '', 'Root', '核算科目') as a  "+
                     " left join (select * from Business_Base_FinanceSubjects Union all select '', 'Root', '核算科目') as b  " +
                     " on a.ParentCode = b.SubjectsCode) as a where 1=1 ";
            _sZstring = new string[] { "科目代码", "科目名称", "父级科目" };
            strPyColunm = "科目名称";
            strWbColunm = "科目代码";
            strCodeColunm = "科目代码";
            strShowMessage = "科目名称";
        }

        void InitFinanceBudgetInfo()
        {
            strSql = " select * from ( select a.ProjectName as 科目名称, b.ProjectName as 父级科目, a.ProjectID as 科目代码, a.PerentProjectID as 父级科目代码  " +
                     " from (select ProjectID , ProjectName, PerentProjectID from Business_Base_Finance_Budget_ProjectItem Union all select '', '预算科目', 'Root') as a  " +
                     " left join (select ProjectID , ProjectName, PerentProjectID from Business_Base_Finance_Budget_ProjectItem Union all select '', '预算科目', 'Root') as b  " +
                     " on a.PerentProjectID = b.ProjectID) as a where 1=1 ";
            _sZstring = new string[] { "科目名称", "父级科目", "科目代码", "父级科目代码" };
            strPyColunm = "科目名称";
            strWbColunm = "父级科目";
            strCodeColunm = "科目名称";
            strShowMessage = "科目名称";
        }

        void InitMaterialRequisitionPurposeInfo()
        {
            strSql = " select * from ( select a.Code as 用途代码, a.Purpose as 用途名称, b.Purpose as 父级用途  "+
                     " from (select Code, Purpose, SUBSTRING(Code, 1, LEN(Code) - 2) as ParentCode    "+
                     " from BASE_MaterialRequisitionPurpose where IsDisable = 1 " +
                     " Union all select '' as Code, '领料用途' as Purpose, 'A') as a   "+
                     " left join (select Code, Purpose, SUBSTRING(Code, 1, LEN(Code) - 2) as ParentCode    "+
                     " from BASE_MaterialRequisitionPurpose  where IsDisable = 1" +
                     " Union all select '' as Code, '领料用途' as Purpose, 'A') as b on a.ParentCode = b.Code) as a where 1=1 ";

            _sZstring = new string[] { "用途代码", "用途名称", "父级用途" };
            strPyColunm = "用途名称";
            strWbColunm = "用途名称";
            strCodeColunm = "用途代码";
            strShowMessage = "用途名称";
        }

        /// <summary>
        /// 自定义
        /// </summary>
        private void InitCustom()
        {
 
        }

        #endregion 

        /// <summary>
        /// 设置对话框的显示位置
        /// </summary>
        private void SetDialogPosition()
        {
            pt = this.Location;
            pt = PointToScreen(pt);
            pt.X -= this.Location.X;
            pt.Y -= this.Location.Y - this.Height - 2;
        }

        void TextBoxShow_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.SelectionLength == 0 && m_blIsShow)
            {
                if (!this.Enabled)
                {
                    return;
                }

                SetDialogPosition();

                string temp = this.Text;
                show_list = new FrmShowList(strSql + " " + strEndSql, strPyColunm, strWbColunm, strCodeColunm, "", _sZstring, pt);
                show_list.IsMultiSelect = _IsMultiSelect;
                DialogResult result = show_list.ShowDialog();

                if (result == DialogResult.OK)
                {
                    if (!_IsMultiSelect)
                    {
                        _dataResult = show_list.DrShowlist;
                        this.Text = _dataResult[strShowMessage].ToString();
                    }
                    else
                    {
                        _dataTableResult = show_list.DrRowList;

                        if (_dataTableResult.Rows.Count == 1)
                        {
                            _dataResult = _dataTableResult.Rows[0];
                        }

                        string showText = "";
                        foreach (DataRow dr in _dataTableResult.Rows)
                        {
                            showText += dr[strShowMessage].ToString() + "\r\n";
                        }
                        this.Text = showText.Trim();
                    }

                    this.SelectionStart = this.Text.Length;

                    if (OnCompleteSearch != null)
                        OnCompleteSearch();
                }
                else if (result == DialogResult.No)
                {
                    this.Text = "";
                    this.Tag = null;
                    _dataResult = null;
                    _dataTableResult = null;
                }
                else
                {
                    this.Text = temp;
                }
            }
        }
        void TextBoxShow_KeyDown(object sender, KeyEventArgs e)
        {
            if (Convert.ToInt32(e.KeyData) < 100000 && m_blIsShow)
            {
                if (!this.Enabled || e.KeyData == Keys.Escape || e.KeyData == Keys.Enter || Convert.ToInt32(e.KeyData) > 100000
                    || e.KeyData == (Keys.LButton | Keys.ShiftKey))
                {
                    show_list.Close();
                    return;
                }

                SetDialogPosition();

                if (e.KeyData == Keys.Escape) return;
                if (e.KeyData == Keys.Enter) return;

                this.SelectAll();

                string temp = this.Text;
                show_list = new FrmShowList(strSql + " " + strEndSql, strPyColunm, strWbColunm, strCodeColunm, "", _sZstring, pt);
                show_list.IsMultiSelect = _IsMultiSelect;
                DialogResult result = show_list.ShowDialog();

                if (result == DialogResult.OK)
                {
                    if (!_IsMultiSelect)
                    {
                        _dataResult = show_list.DrShowlist;
                        this.Text = _dataResult[strShowMessage].ToString();
                    }
                    else
                    {
                        _dataTableResult = show_list.DrRowList;

                        if (_dataTableResult.Rows.Count == 1)
                        {
                            _dataResult = _dataTableResult.Rows[0];
                        }

                        string showText = "";
                        foreach (DataRow dr in _dataTableResult.Rows)
                        {
                            showText += dr[strShowMessage].ToString() + "\r\n";
                        }
                        this.Text = showText.Trim();
                    }

                    if (OnCompleteSearch != null)
                        OnCompleteSearch();
                }
                else if (result == DialogResult.No)
                {
                    this.Text = "";
                    this.Tag = null;
                    _dataResult = null;
                    _dataTableResult = null;
                }
                else
                {
                    this.Text = temp;
                }
            }
        }

        private DataGridView dataGridView;
        private int rowIndex;
        private bool valueChanged;
        private bool repositionOnValueChanged;

        protected override void OnTextChanged(EventArgs e)
        {
            this.valueChanged = true;

            if (EditingControlDataGridView != null)
            {
                this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
            }

            base.OnTextChanged(e);
        }

        #region IDataGridViewEditingControl 成员

        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        {
            this.Font = dataGridViewCellStyle.Font;
            if (dataGridViewCellStyle.BackColor.A < 0xff)
            {
                Color color = Color.FromArgb(0xff, dataGridViewCellStyle.BackColor);
                this.BackColor = color;
                this.dataGridView.EditingPanel.BackColor = color;
            }
            else
            {
                this.BackColor = dataGridViewCellStyle.BackColor;
            }
            this.ForeColor = dataGridViewCellStyle.ForeColor;
            if (dataGridViewCellStyle.WrapMode == DataGridViewTriState.True)
            {
                this.WordWrap = true;
            }
            this.repositionOnValueChanged = (dataGridViewCellStyle.WrapMode == DataGridViewTriState.True)
                && ((dataGridViewCellStyle.Alignment) == DataGridViewContentAlignment.NotSet);
        }

        public DataGridView EditingControlDataGridView
        {
            get
            {
                return dataGridView;
            }
            set
            {
                dataGridView = value;
            }
        }

        public object EditingControlFormattedValue
        {
            get
            {
                return this.Text;
            }
            set
            {
                this.Text = (string)value;
            }
        }

        public int EditingControlRowIndex
        {

            get { return this.rowIndex; }
            set { this.rowIndex = value; }
        }

        public bool EditingControlValueChanged
        {
            get { return this.valueChanged; }
            set { this.valueChanged = value; }
        }

        public bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
        {
            switch ((keyData & Keys.KeyCode))
            {
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                case Keys.Right:
                case Keys.Home:
                case Keys.End:
                case Keys.PageDown:
                case Keys.PageUp:
                    return true;
                default:
                    return !dataGridViewWantsInputKey;
            }
        }

        public Cursor EditingPanelCursor
        {
            get { return Cursors.Default; }
        }

        public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        {
            return EditingControlFormattedValue;
        }

        public void PrepareEditingControlForEdit(bool selectAll)
        {
            // No preparation needs to be done.
        }

        public bool RepositionEditingControlOnValueChange
        {
            get { return this.repositionOnValueChanged; }
        }

        #endregion
    }
}