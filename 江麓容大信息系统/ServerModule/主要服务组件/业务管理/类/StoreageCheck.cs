/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  StoreageCheck.cs
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
    /// 库房盘点单管理类
    /// </summary>
    class StoreageCheck:BasicServer, ServerModule.IStoreageCheck
    {
        /// <summary>
        /// 条形码服务
        /// </summary>
        IBarCodeServer m_barCodeServer = ServerModuleFactory.GetServerModule<IBarCodeServer>();

        /// <summary>
        /// BOM表信息服务组件
        /// </summary>
        IBomServer m_serverBom = ServerModuleFactory.GetServerModule<IBomServer>();

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
            var varData = from a in ctx.S_StorageCheck
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
            string sql = "SELECT * FROM [DepotManagement].[dbo].[S_StorageCheck] where DJH = '" + billNo + "'";

            System.Data.DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 是否重复建单
        /// </summary>
        /// <param name="storageID">库房名称</param>
        /// <param name="billNo">单据号</param>
        /// <returns>重复返回 True，不重复返回 False</returns>
        public bool IsRepeat(string storageID, string billNo)
        {
            string strSql = "select * from [DepotManagement].[dbo].[S_StorageCheck] where BZRY = '" + BasicInfo.LoginID 
                + "' and DJZT = '新建单据' and DJH <> '"+ billNo +"' and StorageID = '"+ storageID +"'";

            DataTable tempTable = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (tempTable.Rows.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 获得主表信息
        /// </summary>
        /// <returns>返回获取的主表信息</returns>
        public DataTable GetAllBill()
        {
            string strSql = "select * from View_S_StorageCheck order by 单据号 desc ";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dt;
        }

        /// <summary>
        /// 获得单条主表信息
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="djh">单据号</param>
        /// <returns>返回获取的单挑主表信息</returns>
        public S_StorageCheck GetBill(DepotManagementDataContext ctx, string djh)
        {
            var varData = from a in ctx.S_StorageCheck
                          where a.DJH == djh
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
        /// 获得单条主表信息
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <returns>返回获取的单挑主表信息</returns>
        public S_StorageCheck GetBill(string djh)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.S_StorageCheck
                          where a.DJH == djh
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
        /// 获得明细表信息
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <param name="depotTable">材料类别列表</param>
        /// <returns>返回获取的明细表信息</returns>
        public DataTable GetList(string djh,DataTable depotTable)
        {
            S_StorageCheck billInfo = GetBill(djh);

            string strRe = "";

            if (depotTable == null || depotTable.Rows.Count == 0)
            {
                strRe = "";
            }
            else
            {

                for (int i = 0; i < depotTable.Rows.Count; i++)
                {
                    strRe = strRe + "'" + depotTable.Rows[i][0].ToString() + "'" + ",";
                }

                strRe = strRe.Substring(0, strRe.Length - 1);
            }

            string strSql = "select * from View_S_StorageCheckList where 单据号 = '" + djh + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dt.Rows.Count > 0)
            {
                return dt;
            }
            else
            {

                if (billInfo.DJFS == "全库房盘点")
                {
                    strSql = "select  物品ID,图号型号, 物品名称, 规格, 批次号, 库存数量 as 账面数量,"+
                        " 实际金额 as 账面金额,库存数量 as 盘点数量,cast(实际金额 as decimal(18, 2)) as 盘点金额,0 as 盈亏数量," +
                        " cast(0 as decimal(18, 2)) as 盈亏金额,供货单位, 单位,物品状态, 材料类别名称,  货架, 列, 层," +
                        " 供方批次号, '' as 备注, 单位ID, b.ID as 物品状态ID, 材料类别编码,'" + djh + "' as 单据号," +
                        " CASE WHEN a.入库时间 IS NULL THEN 0 ELSE a.入库时间 END AS 入库时间,"+
                        " CASE WHEN a.入库时间 IS NULL THEN 0 ELSE CAST(GETDATE() - a.入库时间 AS int) / 30 END AS 账龄"+
                        " from View_S_Stock as a inner join S_StockStatus as b on a.物品状态 = b.Description"+
                        " where 库房代码 = '" + billInfo.StorageID + "' and 库存数量 > 0 ";

                    dt = GlobalObject.DatabaseServer.QueryInfo(strSql);
                }
                else if (billInfo.DJFS == "分类别盘点" && strRe != "")
                {
                    strSql = "select  物品ID,图号型号, 物品名称, 规格, 批次号, 库存数量 as 账面数量," +
                            " 实际金额 as 账面金额,库存数量 as 盘点数量,cast(实际金额 as decimal(18, 2)) as 盘点金额,0 as 盈亏数量," +
                            " cast(0 as decimal(18, 2)) as 盈亏金额,供货单位, 单位,物品状态, 材料类别名称,  货架, 列, 层," +
                            " 供方批次号, '' as 备注, 单位ID, b.ID as 物品状态ID, 材料类别编码,'" + djh + "' as 单据号, " +
                            " CASE WHEN a.入库时间 IS NULL THEN 0 ELSE a.入库时间 END AS 入库时间," +
                            " CASE WHEN a.入库时间 IS NULL THEN 0 ELSE CAST(GETDATE() - a.入库时间 AS int) / 30 END AS 账龄" +
                            " from View_S_Stock as a inner join S_StockStatus as b on a.物品状态 = b.Description" +
                            " where 库房代码 = '" + billInfo.StorageID + "' and 库存数量 > 0 "+
                            " and 材料类别编码 in ("+ strRe +")" ;

                    dt = GlobalObject.DatabaseServer.QueryInfo(strSql);
                }
            }

            return dt;
        }

        /// <summary>
        /// 获得物品明细信息
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">单据号</param>
        /// <returns>返回获取的物品明细信息</returns>
        List<View_S_StorageCheckList> GetList(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.View_S_StorageCheckList
                          where a.单据号 == billNo
                          select a;

            return varData.ToList();
        }

        /// <summary>
        /// 获得物品明细信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回获取的物品明细信息</returns>
        public List<View_S_StorageCheckList> GetList(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.View_S_StorageCheckList
                          where a.单据号 == billNo
                          select a;

            return varData.ToList();
        }

        /// <summary>
        /// 更新单据状态
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <param name="billStatus">单据状态</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>更改成功返回True，更改失败返回False</returns>
        public bool UpdateBill(string djh, string billStatus, out string error)
        {
            error = null;

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();

            try
            {

                var varData = from a in ctx.S_StorageCheck
                              where a.DJH == djh
                              select a;

                if (varData.Count() == 1)
                {
                    S_StorageCheck lnqCheck = varData.Single();

                    if (lnqCheck.DJZT != billStatus)
                    {
                        error = "单据状态错误，请重新刷新单据确认单据状态";
                        return false;
                    }

                    switch (lnqCheck.DJZT)
                    {
                        case "新建单据":
                            lnqCheck.BZRQ = ServerTime.Time;
                            lnqCheck.BZRY = BasicInfo.LoginID;
                            lnqCheck.DJZT = "等待主管审核";
                            lnqCheck.KFRQ = null;
                            lnqCheck.KFRY = null;
                            lnqCheck.SHRQ = null;
                            lnqCheck.SHRY = null;
                            lnqCheck.CWRQ = null;
                            lnqCheck.CWRY = null;
                            break;
                        case "等待主管审核":
                            lnqCheck.SHRY = BasicInfo.LoginID;
                            lnqCheck.SHRQ = ServerTime.Time;
                            lnqCheck.DJZT = "等待负责人批准";
                            break;
                        case "等待负责人批准":
                            lnqCheck.FGRY = BasicInfo.LoginID;
                            lnqCheck.FGRQ = ServerTime.Time;
                            lnqCheck.DJZT = "等待财务批准";
                            break;
                        case "等待财务批准":
                            lnqCheck.CWRY = BasicInfo.LoginID;
                            lnqCheck.CWRQ = ServerTime.Time;
                            lnqCheck.DJZT = "等待仓管确认";
                            break;
                        case "等待仓管确认":
                            if (lnqCheck.DJZT == "单据已完成")
                            {
                                error = "单据不能重复确认";
                                return false;
                            }
                            lnqCheck.DJZT = "单据已完成";
                            lnqCheck.KFRY = BasicInfo.LoginID;
                            lnqCheck.KFRQ = ServerTime.Time;

                            //创建领料单
                            CreateMaterialRequisition(ctx, lnqCheck.DJH);
                            ctx.SubmitChanges();

                            //创建领料退库单
                            CreateMaterialReturnedInTheDepot(ctx, lnqCheck.DJH);
                            ctx.SubmitChanges();

                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    error = "数据不唯一";
                    return false;
                }

                ctx.SubmitChanges();
                ctx.Transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                ctx.Transaction.Rollback();
                return false;
            }
        }

        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool DeleteBill(string billID,out string error)
        {
            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            var varData = from a in dataContext.S_StorageCheck
                          where a.DJH == billID
                          select a;

            if (varData.Count() != 1)
            {
                error = "数据不唯一";
                return false;
            }
            else
            {
                S_StorageCheck lnqCheck = varData.Single();

                dataContext.S_StorageCheck.DeleteOnSubmit(lnqCheck);

                if (!DeleteListDate(dataContext, lnqCheck.DJH, out error))
                {
                    return false;
                }

                dataContext.SubmitChanges();

                return true;
            }
        }

        /// <summary>
        /// 自动生成领料退库单
        /// </summary>
        /// <param name="contxt">数据上下文</param>
        /// <param name="djh">单据号</param>
        void CreateMaterialReturnedInTheDepot(DepotManagementDataContext contxt, string djh)
        {
            MaterialReturnedInTheDepot serverReturnedBill = new MaterialReturnedInTheDepot();

            try
            {
                string strBillID = m_assignBill.AssignNewNo(serverReturnedBill,CE_BillTypeEnum.领料退库单.ToString());

                S_StorageCheck billInfo = GetBill(contxt, djh);
                List<View_S_StorageCheckList> listInfo = (from a in GetList(contxt, djh) where a.盈亏数量 > 0 select a).ToList();
                S_MaterialReturnedInTheDepot lnqReturnedInTheDepot = new S_MaterialReturnedInTheDepot();

                if (listInfo.Count > 0)
                {
                    #region 领料退库单主表
                    lnqReturnedInTheDepot.Bill_ID = strBillID;
                    lnqReturnedInTheDepot.Bill_Time = ServerTime.Time;
                    lnqReturnedInTheDepot.BillStatus = "已完成";
                    lnqReturnedInTheDepot.Department = "ZZ05";
                    lnqReturnedInTheDepot.DepartmentDirector = UniversalFunction.GetPersonnelInfo(contxt, billInfo.SHRY).姓名;
                    lnqReturnedInTheDepot.DepotManager = BasicInfo.LoginName;
                    lnqReturnedInTheDepot.FillInPersonnel = UniversalFunction.GetPersonnelInfo(contxt, billInfo.BZRY).姓名;
                    lnqReturnedInTheDepot.FillInPersonnelCode = billInfo.BZRY;
                    lnqReturnedInTheDepot.InDepotDate = ServerTime.Time;
                    lnqReturnedInTheDepot.PurposeCode = UniversalFunction.GetPurpose(CE_PickingPurposeProperty.盘点).Code;
                    lnqReturnedInTheDepot.QualityInputer = "";
                    lnqReturnedInTheDepot.Remark = "库房盘点（盘盈）";
                    lnqReturnedInTheDepot.ReturnMode = "领料退库";
                    lnqReturnedInTheDepot.ReturnReason = "库房盘点（盘盈）";
                    lnqReturnedInTheDepot.ReturnType = null;
                    lnqReturnedInTheDepot.StorageID = billInfo.StorageID;

                    contxt.S_MaterialReturnedInTheDepot.InsertOnSubmit(lnqReturnedInTheDepot);

                    #endregion

                    foreach (View_S_StorageCheckList listSingle in listInfo)
                    {
                        #region 领料单退库明细
                        S_MaterialListReturnedInTheDepot lnqReturnedInTheDepotList = new S_MaterialListReturnedInTheDepot();

                        lnqReturnedInTheDepotList.BatchNo = listSingle.批次号;
                        lnqReturnedInTheDepotList.Bill_ID = strBillID;
                        lnqReturnedInTheDepotList.ColumnNumber = listSingle.列;
                        lnqReturnedInTheDepotList.GoodsID = (int)listSingle.物品ID;
                        lnqReturnedInTheDepotList.LayerNumber = listSingle.层;
                        lnqReturnedInTheDepotList.Provider = listSingle.供货单位;
                        lnqReturnedInTheDepotList.ProviderBatchNo = listSingle.供方批次号;
                        lnqReturnedInTheDepotList.Remark = "库房盘点（盘亏）";
                        lnqReturnedInTheDepotList.ReturnedAmount = (decimal)listSingle.盈亏数量;
                        lnqReturnedInTheDepotList.ShelfArea = listSingle.货架;

                        contxt.S_MaterialListReturnedInTheDepot.InsertOnSubmit(lnqReturnedInTheDepotList);

                        #endregion
                    }

                    contxt.SubmitChanges();

                    serverReturnedBill.OpertaionDetailAndStock(contxt, lnqReturnedInTheDepot);
                    contxt.SubmitChanges();

                    m_assignBill.UseBillNo(CE_BillTypeEnum.领料退库单.ToString(), strBillID);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 自动生成领料单
        /// </summary>
        /// <param name="contxt">数据上下文</param>
        /// <param name="djh">单据号</param>
        void CreateMaterialRequisition(DepotManagementDataContext contxt,  string djh)
        {
            MaterialRequisitionServer serverMaterialBill = new MaterialRequisitionServer();
            string error = null;

            try
            {
                string strBillID = m_assignBill.AssignNewNo(serverMaterialBill,CE_BillTypeEnum.领料单.ToString());

                S_StorageCheck billInfo = GetBill(contxt, djh);
                List<View_S_StorageCheckList> listInfo = (from a in GetList(contxt, djh) where a.盈亏数量 < 0 select a).ToList();
                S_MaterialRequisition lnqRequisitionBill = new S_MaterialRequisition();

                if (listInfo.Count > 0)
                {
                    #region 领料单主表
                    lnqRequisitionBill.AssociatedBillNo = djh;
                    lnqRequisitionBill.AssociatedBillType = "盘点单";
                    lnqRequisitionBill.Bill_ID = strBillID;
                    lnqRequisitionBill.Bill_Time = ServerTime.Time;
                    lnqRequisitionBill.BillStatus = "已出库";
                    lnqRequisitionBill.Department = "ZZ05";
                    lnqRequisitionBill.DepartmentDirector = UniversalFunction.GetPersonnelInfo(contxt, billInfo.SHRY).姓名;
                    lnqRequisitionBill.DepotManager = BasicInfo.LoginName;
                    lnqRequisitionBill.FetchCount = 0;
                    lnqRequisitionBill.FetchType = "零星领料";
                    lnqRequisitionBill.FillInPersonnel = UniversalFunction.GetPersonnelInfo(contxt, billInfo.BZRY).姓名;
                    lnqRequisitionBill.FillInPersonnelCode = billInfo.BZRY;
                    lnqRequisitionBill.OutDepotDate = ServerTime.Time;
                    lnqRequisitionBill.ProductType = "";
                    lnqRequisitionBill.PurposeCode = UniversalFunction.GetPurpose(CE_PickingPurposeProperty.盘点).Code;
                    lnqRequisitionBill.Remark = "库房盘点（盘亏）";
                    lnqRequisitionBill.StorageID = billInfo.StorageID;

                    if (!serverMaterialBill.AutoCreateBill(contxt, lnqRequisitionBill, out error))
                    {
                        throw new Exception(error);
                    }
                    #endregion

                    foreach (View_S_StorageCheckList listSingle in listInfo)
                    {
                        #region 领料单明细

                        S_MaterialRequisitionGoods lnqRequisitionGoods = new S_MaterialRequisitionGoods();

                        lnqRequisitionGoods.BasicCount = 0;
                        lnqRequisitionGoods.BatchNo = listSingle.批次号;
                        lnqRequisitionGoods.Bill_ID = strBillID;
                        lnqRequisitionGoods.GoodsID = (int)listSingle.物品ID;
                        lnqRequisitionGoods.ProviderCode = listSingle.供货单位;
                        lnqRequisitionGoods.RealCount = -(decimal)listSingle.盈亏数量;
                        lnqRequisitionGoods.Remark = "库房盘点（盘亏）";
                        lnqRequisitionGoods.RequestCount = -(decimal)listSingle.盈亏数量;
                        lnqRequisitionGoods.ShowPosition = 1;

                        MaterialRequisitionGoodsServer serverMaterialGoods = new MaterialRequisitionGoodsServer();

                        if (!serverMaterialGoods.AutoCreateGoods(contxt, lnqRequisitionGoods, out error))
                        {
                            throw new Exception(error);
                        }

                        #endregion
                    }
                    contxt.SubmitChanges();

                    serverMaterialBill.OpertaionDetailAndStock(contxt, lnqRequisitionBill);
                    contxt.SubmitChanges();

                    m_assignBill.UseBillNo(CE_BillTypeEnum.领料单.ToString(), strBillID);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 添加主表信息
        /// </summary>
        /// <param name="inCheck">盘点单信息</param>
        /// <param name="checkList">明细信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>添加成功返回True，添加失败返回False</returns>
        public bool AddBill(S_StorageCheck inCheck,DataTable checkList,out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.S_StorageCheck
                              where a.DJH == inCheck.DJH
                              select a;

                switch (varData.Count())
                {
                    case 0 :
                        S_StorageCheck lnqCheck = new S_StorageCheck();

                        lnqCheck.DJH = inCheck.DJH;
                        lnqCheck.DJFS = inCheck.DJFS;
                        lnqCheck.StorageID = inCheck.StorageID;
                        lnqCheck.BZRY = BasicInfo.LoginID;
                        lnqCheck.BZRQ = ServerTime.Time;
                        lnqCheck.DJZT = "新建单据";
                        ctx.S_StorageCheck.InsertOnSubmit(lnqCheck);

                        break;
                    case 1:
                        S_StorageCheck lnqCheckFor = varData.Single();

                        if (lnqCheckFor.DJZT == "单据已完成")
                        {
                            error = "单据已完成，无法重新保存，请重新确认单据状态";
                            return false;
                        }

                        lnqCheckFor.DJFS = inCheck.DJFS;
                        lnqCheckFor.BZRQ = ServerTime.Time;
                        lnqCheckFor.BZRY = BasicInfo.LoginID;
                        lnqCheckFor.DJZT = "新建单据";
                        lnqCheckFor.KFRQ = null;
                        lnqCheckFor.KFRY = null;
                        lnqCheckFor.SHRQ = null;
                        lnqCheckFor.SHRY = null;
                        lnqCheckFor.CWRQ = null;
                        lnqCheckFor.CWRY = null;

                        break;
                    default:
                        break;
                }

                if (checkList != null)
                {
                    if (!AddListDate(ctx, inCheck.DJH, checkList, out error))
                    {
                        return false;
                    }
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
        /// 添加明细信息
        /// </summary>
        /// <param name="contxt">数据上下文</param>
        /// <param name="djh">单据号</param>
        /// <param name="checkList">单据明细信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>添加成功返回True，添加失败返回False</returns>
        private bool AddListDate(DepotManagementDataContext contxt, string djh,DataTable checkList,out string error)
        {
            error = null;

            try
            {
                var varData = from a in contxt.S_StorageCheckList
                              where a.DJH == djh
                              select a;

                if (varData.Count() != 0)
                {
                    if (!DeleteListDate(contxt,djh,out error))
                    {
                        return false;
                    }
                }

                for (int i = 0; i < checkList.Rows.Count; i++)
                {
                    S_StorageCheckList lnqCheckList = new S_StorageCheckList();

                    lnqCheckList.DJH = checkList.Rows[i]["单据号"].ToString();
                    lnqCheckList.GoodsID = Convert.ToInt32( checkList.Rows[i]["物品ID"]);
                    lnqCheckList.BatchNo = checkList.Rows[i]["批次号"].ToString();
                    lnqCheckList.ProviderBatchNo = checkList.Rows[i]["供方批次号"].ToString();
                    lnqCheckList.Provider = checkList.Rows[i]["供货单位"].ToString();
                    lnqCheckList.ShelfArea = checkList.Rows[i]["货架"].ToString();
                    lnqCheckList.ColumnNumber = checkList.Rows[i]["列"].ToString();
                    lnqCheckList.LayerNumber = checkList.Rows[i]["层"].ToString();
                    lnqCheckList.GoodsStatus = Convert.ToInt32( checkList.Rows[i]["物品状态ID"].ToString());
                    lnqCheckList.Remark = checkList.Rows[i]["备注"].ToString();
                    lnqCheckList.ZMJE = Convert.ToDecimal(checkList.Rows[i]["账面金额"]);
                    lnqCheckList.ZMSL = Convert.ToDecimal(checkList.Rows[i]["账面数量"]);
                    lnqCheckList.PDJE = Convert.ToDecimal(checkList.Rows[i]["盘点金额"]);
                    lnqCheckList.PDSL = Convert.ToDecimal(checkList.Rows[i]["盘点数量"]);
                    lnqCheckList.YKJE = Convert.ToDecimal(checkList.Rows[i]["盈亏金额"]);
                    lnqCheckList.YKSL = Convert.ToDecimal(checkList.Rows[i]["盈亏数量"]);

                    contxt.S_StorageCheckList.InsertOnSubmit(lnqCheckList);
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
        /// 删除明细信息
        /// </summary>
        /// <param name="contxt">数据上下文</param>
        /// <param name="djh">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        private bool DeleteListDate(DepotManagementDataContext contxt, string djh,out string error)
        {
            error = null;

            try
            {
                var varData = from a in contxt.S_StorageCheckList
                              where a.DJH == djh
                              select a;

                contxt.S_StorageCheckList.DeleteAllOnSubmit(varData);

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
