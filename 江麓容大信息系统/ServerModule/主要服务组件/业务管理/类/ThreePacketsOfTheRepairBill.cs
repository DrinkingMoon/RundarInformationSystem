/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  ThreePacketsOfTheRepairBill.cs
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
    /// 三包外返修管理类
    /// </summary>
    class ThreePacketsOfTheRepairBill:BasicServer, ServerModule.IThreePacketsOfTheRepairBill
    {
        /// <summary>
        /// 人员信息服务组件
        /// </summary>
        IPersonnelInfoServer m_serverPersonnel = ServerModuleFactory.GetServerModule<IPersonnelInfoServer>();

        /// <summary>
        /// 部门信息服务组件
        /// </summary>
        IDepartmentServer m_serverDepartment = ServerModuleFactory.GetServerModule<IDepartmentServer>();

        /// <summary>
        /// 库存信息服务组件
        /// </summary>
        IStoreServer m_serverStore = ServerModuleFactory.GetServerModule<IStoreServer>();

        /// <summary>
        /// 基础物品服务组件
        /// </summary>
        IBasicGoodsServer m_serverBasicGoods = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();

        /// <summary>
        /// 数据库操作接口
        /// </summary>
        IDBOperate m_dbDate = CommentParameter.GetDBOperatorOfDepotManagement();

        /// <summary>
        /// 单据编号分配服务
        /// </summary>
        IAssignBillNoServer m_serverAssignBill = ServerModuleFactory.GetServerModule<IAssignBillNoServer>();

        /// <summary>
        /// 快捷设置明细
        /// </summary>
        /// <param name="dtSoucre">数据源</param>
        /// <param name="error">错误信息</param>
        /// <returns>返回明细列表</returns>
        public List<View_YX_ThreePacketsOfTheRepairList> GetShortcutDetailList(DataTable dtSoucre, out string error)
        {
            error = null;

            try
            {
                List<View_YX_ThreePacketsOfTheRepairList> listResult = new List<View_YX_ThreePacketsOfTheRepairList>();

                foreach (DataRow dr in dtSoucre.Rows)
                {
                    string strSql = "select * from View_S_Stock where 库房代码 in( '01','08','11') and 物品ID = " + dr["物品ID"].ToString()
                        + "  and 库存数量 > 0 and 物品状态 in ( '正常' ,'仅限于返修箱用') order by 库房代码 desc, 入库时间";

                    DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

                    if (dtTemp.Rows.Count == 0)
                    {
                        error = error + UniversalFunction.GetGoodsMessage(Convert.ToInt32(dr["物品ID"])) + "\r\n";
                    }
                    else
                    {
                        decimal requestCount = Convert.ToDecimal(dr["装配数量"]);

                        foreach (DataRow dr1 in dtTemp.Rows)
                        {
                            if (requestCount > 0)
                            {
                                View_YX_ThreePacketsOfTheRepairList tempLnq = new View_YX_ThreePacketsOfTheRepairList();

                                tempLnq.策略金额 = 0;
                                tempLnq.单价 = 0;
                                tempLnq.单位 = dr1["单位"].ToString();
                                tempLnq.规格 = dr1["规格"].ToString();
                                tempLnq.金额 = 0;
                                tempLnq.批次号 = dr1["批次号"].ToString();
                                tempLnq.是否为客户责任 = false;
                                tempLnq.图号型号 = dr1["图号型号"].ToString();
                                tempLnq.物品ID = Convert.ToInt32(dr1["物品ID"]);
                                tempLnq.物品名称 = dr1["物品名称"].ToString();
                                tempLnq.备注 = dr["备注"].ToString();

                                if (requestCount <= Convert.ToDecimal( dr1["库存数量"]))
                                {
                                    tempLnq.领用数量 = requestCount;
                                }
                                else
                                {
                                    tempLnq.领用数量 = Convert.ToDecimal(dr1["库存数量"]);
                                }

                                listResult.Add(tempLnq);

                                requestCount = requestCount - Convert.ToDecimal( tempLnq.领用数量);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }

                return listResult;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 对单条三包外返修零件信息的数据库操作
        /// </summary>
        /// <param name="flag">操作方式 0：添加，1：修改，2：删除</param>
        /// <param name="threePacket">数据集</param>
        /// <param name="oldGoodsID">老的物品ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool OperationGoodsUnitPrice(int flag, YX_ThreePacketsOfTheRepairGoodsUnitPrice threePacket,
            int oldGoodsID,out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext contxt = CommentParameter.DepotDataContext;

                switch (flag)
                {
                    case 0://添加

                        var varInsert = from a in contxt.YX_ThreePacketsOfTheRepairGoodsUnitPrice
                                        where a.GoodsID == threePacket.GoodsID
                                        select a;

                        if (varInsert.Count() > 0)
                        {
                            error = "不能添加同一个物品信息";
                            return false;
                        }
                        else
                        {
                            contxt.YX_ThreePacketsOfTheRepairGoodsUnitPrice.InsertOnSubmit(threePacket);
                        }

                        break;
                    case 1://修改

                        if (threePacket.GoodsID == oldGoodsID)
                        {
                            var varUpdateSame = from a in contxt.YX_ThreePacketsOfTheRepairGoodsUnitPrice
                                                where a.GoodsID == threePacket.GoodsID
                                                select a;

                            YX_ThreePacketsOfTheRepairGoodsUnitPrice lnqThree = varUpdateSame.Single();

                            lnqThree.UnitPrice = threePacket.UnitPrice;
                        }
                        else
                        {
                            var varUpdateDif =  from a in contxt.YX_ThreePacketsOfTheRepairGoodsUnitPrice
                                                where a.GoodsID == threePacket.GoodsID
                                                select a;

                            if (varUpdateDif.Count() > 0)
                            {
                                error = "不能修改成重复信息";
                                return false;
                            }
                            else
                            {
                                varUpdateDif =  from a in contxt.YX_ThreePacketsOfTheRepairGoodsUnitPrice
                                                where a.GoodsID == oldGoodsID
                                                select a;

                                YX_ThreePacketsOfTheRepairGoodsUnitPrice lnqThree = varUpdateDif.Single();

                                lnqThree.GoodsID = threePacket.GoodsID;
                                lnqThree.UnitPrice = threePacket.UnitPrice;
                            }
                        }

                        break;
                    case 2://删除

                        var varDelete = from a in contxt.YX_ThreePacketsOfTheRepairGoodsUnitPrice
                                        where a.GoodsID == oldGoodsID
                                        select a;

                        contxt.YX_ThreePacketsOfTheRepairGoodsUnitPrice.DeleteAllOnSubmit(varDelete);

                        break;
                    default:
                        break;
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
        /// 设置所有三包外返修零件单价
        /// </summary>
        /// <param name="flag">是否删除原有的记录 True：删除 False：不删除</param>
        /// <param name="goodSunitPrice">数据表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool UpdateAllGoodsUnitPrice(bool flag,DataTable goodSunitPrice,out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext contxt = CommentParameter.DepotDataContext;

                if (flag)
                {
                    var varData = from a in contxt.YX_ThreePacketsOfTheRepairGoodsUnitPrice
                                  select a;

                    contxt.YX_ThreePacketsOfTheRepairGoodsUnitPrice.DeleteAllOnSubmit(varData);
                }

                for (int i = 0; i < goodSunitPrice.Rows.Count; i++)
                {
                    YX_ThreePacketsOfTheRepairGoodsUnitPrice lnqThreePackets = new YX_ThreePacketsOfTheRepairGoodsUnitPrice();

                    lnqThreePackets.GoodsID = Convert.ToInt32(goodSunitPrice.Rows[i]["物品ID"]);
                    lnqThreePackets.UnitPrice = Convert.ToDecimal(goodSunitPrice.Rows[i]["单价"]);

                    var varCheck = from a in contxt.YX_ThreePacketsOfTheRepairGoodsUnitPrice
                                   where a.GoodsID == lnqThreePackets.GoodsID
                                   select a;

                    if (varCheck.Count() == 0)
                    {
                        contxt.YX_ThreePacketsOfTheRepairGoodsUnitPrice.InsertOnSubmit(lnqThreePackets);
                    }
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
        /// 获得三包外返修的某个零件单价
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回单价</returns>
        public decimal GetThreePacketGoodsUnitPrice(int goodsID)
        {
            string strSql = "select * from YX_ThreePacketsOfTheRepairGoodsUnitPrice where GoodsID = " + goodsID;

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dt.Rows.Count == 0)
            {
                return 0;
            }
            else
            {
                return Convert.ToDecimal(dt.Rows[0]["UnitPrice"]);
            }
        }

        /// <summary>
        /// 获得所有三包外返修零件单价信息
        /// </summary>
        /// <returns>返回获得的三包外返修零件单价信息</returns>
        public DataTable GetGoodsUnitPriceInfo()
        {
            Hashtable paramTable = new Hashtable();

            DataSet ds = new DataSet();

            Dictionary<OperateCMD, object> dicOperateCMD = m_dbDate.RunProc_CMD("YX_Select_ThreePacketsOfTheRepairGoodsUnitPrice", 
                ds, paramTable);

            if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
            {
                return null;
            }

            return ds.Tables[0];
        }

        /// <summary>
        /// 检查某业务是否存在
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">业务号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.YX_ThreePacketsOfTheRepairBill
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
            string sql = "SELECT * FROM [DepotManagement].[dbo].[YX_ThreePacketsOfTheRepairBill] where Bill_ID = '" + billNo + "'";

            System.Data.DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 获得全部单据
        /// </summary>
        /// <param name="billStatus">单据状态,若为“全  部”则显示所有单据信息</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>返回获得的单据信息</returns>
        public DataTable GetAllBill(string billStatus, DateTime startTime, DateTime endTime)
        {
            string strSelect = "";

            string strSql = "select * from View_YX_ThreePacketsOfTheRepairBill where 1 = 1 and ";

            if (billStatus != "全  部")
            {
                strSelect += "单据状态 = '" + billStatus + "' and ";
            }

            strSelect += "创建时间 >= '" + startTime + "' and 创建时间 <= '" + endTime + "'";

            strSql = strSql + strSelect;

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dt;
        }

        /// <summary>
        /// 获得明细
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <returns>返回获得的明细信息</returns>
        public DataTable GetList(string billID)
        {
            string strSql = "select * from View_YX_ThreePacketsOfTheRepairList where 单据号 = '" + billID + "'";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得一条记录的信息
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回获得的信息记录</returns>
        public YX_ThreePacketsOfTheRepairBill GetBill(string billID,out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext contxt = CommentParameter.DepotDataContext;

                var varData = from a in contxt.YX_ThreePacketsOfTheRepairBill
                              where a.Bill_ID == billID
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数据不唯一或者为空";
                    return null;
                }
                else
                {
                    return varData.Single();
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
        }
        
        /// <summary>
        /// 获得一次性物料集合
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="msg">操作信息</param>
        /// <returns>返回DataTable</returns>
        public DataTable InsertThreePacketsOfTheRepairList(string billID,out string msg)
        {
            msg = null;

            string productType = GetBill(billID, out msg).ProductType;

            if (msg != null)
            {
                return null;
            }

            string strSql = " select b.* from ZPX_DisposableGoods as a left join "+
                            " (select 1 as 是否为客户责任,图号型号,物品名称,规格, BatchNo as 批次号,a.Count as 领用数量,单位,ThreeUnitPrice as 单价," +
                            " a.Count * ThreeUnitPrice as 金额,0 as 策略金额,'由一次性物料消耗表自动生成' as 备注, a.GoodsID as 物品ID,'" + billID + "' as 单据号 " +
                            " from (select a.*,case when b.UnitPrice is null then 0 else b.UnitPrice end as ThreeUnitPrice  from "+
                            " (select * from (select a.*,b.Count from S_Stock as a inner join  " +
                            " (select a.*,b.序号 as GoodsID from ZPX_DisposableGoods as a inner join View_F_GoodsPlanCost as b on a.GoodsName = b.物品名称 " +
                            " and a.GoodsCode = b.图号型号 and a.Spec = b.规格"+
                            " where ProductType = '" + productType + "') as b on a.GoodsID = b.GoodsID and a.StorageID in('01','08','11')  and a.GoodsStatus in (0,6)" +
                            " and a.ExistCount > b.Count) A WHERE Date=(SELECT Min(Date) FROM (select a.* from S_Stock as a inner join "+
                            " (select a.*,b.序号 as GoodsID "+
                            " from ZPX_DisposableGoods as a inner join View_F_GoodsPlanCost as b on a.GoodsName = b.物品名称 " +
                            " and a.GoodsCode = b.图号型号 and a.Spec = b.规格" +
                            " where ProductType = '" + productType + "') as b on a.GoodsID = b.GoodsID and a.StorageID in('01','08','11')  and a.GoodsStatus in (0,6)" +
                            " and a.ExistCount > b.Count) as b WHERE b.GoodsID=A.GoodsID )) as a left join YX_ThreePacketsOfTheRepairGoodsUnitPrice as b "+
                            " on a.GoodsID = b.GoodsID) as a inner join View_F_GoodsPlanCost as b on a.GoodsID = b.序号) as b "+
                            " on b.图号型号 = a.GoodsCode and b.物品名称 = a.GoodsName and b.规格 = a.Spec where a.ProductType = '" + productType + "' and b.物品ID is not null";

            DataTable dtResult = GlobalObject.DatabaseServer.QueryInfo(strSql);

            strSql = strSql.Replace("b.物品ID is not null", "b.物品ID is null");

            strSql = strSql.Replace("select b.* from ZPX_DisposableGoods as a left", "select a.* from ZPX_DisposableGoods as a left");

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dtTemp != null && dtTemp.Rows.Count > 0)
            {
                msg = "库存不足的一次性物料有: \r\n";

                foreach (DataRow dr in dtTemp.Rows)
                {
                    msg += string.Format("图号：{0}，物品名称：{1}，规格：{2}。 \r\n", dr["GoodsCode"].ToString(), dr["GoodsName"].ToString(), dr["Spec"].ToString());
                }
            }

            return dtResult;

        }

        void CheckData(DepotManagementDataContext ctx, string billNo)
        {
            foreach (var item in from a in ctx.YX_ThreePacketsOfTheRepairList where a.Bill_ID == billNo select a)
            {
                var varData = from g in
                                  (from a in
                                       (from a in ctx.S_MaterialRequisition
                                        join b in ctx.S_MaterialRequisitionGoods
                                        on a.Bill_ID equals b.Bill_ID
                                        where a.AssociatedBillNo == billNo
                                        select new { GoodsID = b.GoodsID, BatchNo = b.BatchNo, OpCount = b.RealCount }).Union
                                           (from a in ctx.S_MarketingBill
                                            join b in ctx.S_MarketingList
                                            on a.ID equals b.DJ_ID
                                            where a.Remark.Contains(billNo)
                                            select new { GoodsID = Convert.ToInt32(b.CPID), BatchNo = b.BatchNo, OpCount = b.Count })
                                   group a by new { a.GoodsID, a.BatchNo } into g
                                   select new { GoodsID = g.Key.GoodsID, BatchNo = g.Key.BatchNo, OpCount = g.Sum(p => p.OpCount) })
                              where (int)g.GoodsID == item.GoodsID && (string)g.BatchNo == item.BatchNo && (decimal)g.OpCount == item.PickCount
                              select g;

                if (varData.Count() == 0)
                {
                    throw new Exception("物品未全部自动生成" + item.GoodsID.ToString() + " " + item.BatchNo.ToString());
                }
            }
        }

        /// <summary>
        /// 单据流程
        /// </summary>
        /// <param name="threePacketBill">LINQ 单据数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作流程成功返回True，操作流程失败返回False</returns>
        public bool UpdateBill(YX_ThreePacketsOfTheRepairBill threePacketBill, out string error)
        {
            error = null;

            SellIn serverSellIn = new SellIn();

            DepotManagementDataContext contxt = CommentParameter.DepotDataContext;

            contxt.Connection.Open();
            contxt.Transaction = contxt.Connection.BeginTransaction();

            try
            {

                var varData = from a in contxt.YX_ThreePacketsOfTheRepairBill
                              where a.Bill_ID == threePacketBill.Bill_ID
                              select a;

                if (varData.Count() == 0)
                {
                    if (threePacketBill.DJZT == "新建单据")
                    {
                        threePacketBill.DJZT = "等待确认收货";
                        threePacketBill.MarketingStrategy = 0;
                        threePacketBill.RepairTaskTime = 0;
                        threePacketBill.FoundDate = ServerTime.Time;
                        threePacketBill.FoundPersonnel = BasicInfo.LoginName;

                        contxt.YX_ThreePacketsOfTheRepairBill.InsertOnSubmit(threePacketBill);

                        //if (!InsertThreePacketsOfTheRepairList(contxt,threePacketBill.Bill_ID,threePacketBill.ProductType,out error))
                        //{
                        //    throw new Exception(error);
                        //}
                    }
                    else
                    {
                        error = "数据为空";
                        return false;
                    }
                }
                else if (varData.Count() > 1)
                {
                    error = "数据不唯一";
                    return false;
                }
                else if (varData.Count() == 1)
                {
                    YX_ThreePacketsOfTheRepairBill lnqThreePackets = varData.Single();

                    if (lnqThreePackets.DJZT != threePacketBill.DJZT)
                    {
                        error = "单据状态错误，请重新刷新单据确认单据状态";
                        return false;
                    }

                    switch (lnqThreePackets.DJZT)
                    {
                        case "等待确认收货":
                            lnqThreePackets.DJZT = "等待领料明细申请";

                            lnqThreePackets.AOGDate = ServerTime.Time;
                            lnqThreePackets.AOGPersonnel = BasicInfo.LoginName;

                            break;

                        case "等待领料明细申请":
                            lnqThreePackets.DJZT = "等待确认清单责任归属";

                            lnqThreePackets.PlantRemark = threePacketBill.PlantRemark;
                            lnqThreePackets.WorkShopDate = ServerTime.Time;
                            lnqThreePackets.WorkShopPersonnel = BasicInfo.LoginName;

                            break;

                        case "等待确认清单责任归属":
                            lnqThreePackets.DJZT = "等待返修车间主管审核";

                            lnqThreePackets.DistributeDate = ServerTime.Time;
                            lnqThreePackets.DistributePersonnel = BasicInfo.LoginName;

                            break;

                        case "等待返修车间主管审核":
                            lnqThreePackets.DJZT = "等待仓管确认出库";

                            lnqThreePackets.PlantRemark = threePacketBill.PlantRemark;
                            lnqThreePackets.WorkshopManagerDate = ServerTime.Time;
                            lnqThreePackets.WorkshopManagerPersonnel = BasicInfo.LoginName;

                            break;

                        case "等待仓管确认出库":
                            lnqThreePackets.DJZT = "等待返修完成";

                            lnqThreePackets.StockDate = ServerTime.Time;
                            lnqThreePackets.StockPersonnel = BasicInfo.LoginName;

                            DataTable listTable = GetStorageTable(GetList(lnqThreePackets.Bill_ID),out error);

                            if (listTable == null)
                            {
                                throw new Exception(error);
                            }

                            DataTable dtSellListOfStorageID =
                                GlobalObject.DataSetHelper.SelectDistinct("", GlobalObject.DataSetHelper.SiftDataTable(listTable, "是否为客户责任 = 1", out  error), "StroageID");
                                //GetListOfStorageID(lnqThreePackets.Bill_ID, 1);

                            DataTable dtMaterialListOfStorageID =
                                GlobalObject.DataSetHelper.SelectDistinct("", GlobalObject.DataSetHelper.SiftDataTable(listTable, "是否为客户责任 = 0", out  error), "StroageID");
                                //GetListOfStorageID(lnqThreePackets.Bill_ID, 0);

                            //自动生成营销出库单.....
                            if (!InsertYXCK(contxt, lnqThreePackets, dtSellListOfStorageID, listTable, out error))
                            {
                                throw new Exception(error);
                            }

                            //自动生成领料单.....
                            if (!InsertMaterialRequisition(contxt, lnqThreePackets, dtMaterialListOfStorageID,listTable, out error))
                            {
                                throw new Exception(error);
                            }

                            contxt.SubmitChanges();

                            CheckData(contxt, lnqThreePackets.Bill_ID);

                            break;

                        case "等待返修完成":

                            lnqThreePackets.DJZT = "等待质检检验";

                            lnqThreePackets.RepairTaskTime = Convert.ToDecimal(threePacketBill.RepairTaskTime);
                            lnqThreePackets.PlantRemark = threePacketBill.PlantRemark;
                            lnqThreePackets.RepairDate = ServerTime.Time;
                            lnqThreePackets.RepairPersonnel = BasicInfo.LoginName;
                            break;

                        case "等待质检检验":

                            lnqThreePackets.DJZT = "等待销售策略";
                            lnqThreePackets.ExamineDate = ServerTime.Time;
                            lnqThreePackets.ExaminePersonnel = BasicInfo.LoginName;

                            break;

                        case "等待销售策略":

                            lnqThreePackets.DJZT = "等待营销主管审核";

                            lnqThreePackets.MarketingStrategy = Convert.ToDecimal(threePacketBill.MarketingStrategy);

                            lnqThreePackets.StrategyDate = ServerTime.Time;
                            lnqThreePackets.StrategyPersonnel = BasicInfo.LoginName;

                            break;
                        case "等待营销主管审核":

                            if (Convert.ToDecimal(threePacketBill.MarketingStrategy) > 50)
                            {
                                lnqThreePackets.DJZT = "等待营销总监审核";
                            }
                            else
                            {
                                lnqThreePackets.DJZT = "等待财务确认";
                            }

                            lnqThreePackets.MarketingExecutiveDate = ServerTime.Time;
                            lnqThreePackets.MarketingExecutive = BasicInfo.LoginName;

                            break;
                        case "等待营销总监审核":

                            lnqThreePackets.DJZT = "等待财务确认";

                            lnqThreePackets.MarketingLeaderDate = ServerTime.Time;
                            lnqThreePackets.MarketingLeaderPersonnel = BasicInfo.LoginName;

                            break;

                        case "等待财务确认":

                            lnqThreePackets.DJZT = "已完成";

                            lnqThreePackets.FinanceDate = ServerTime.Time;
                            lnqThreePackets.FinancePersonnel = BasicInfo.LoginName;

                            break;
                        
                        default:
                            break;
                    }
                }

                contxt.SubmitChanges();
                contxt.Transaction.Commit();

                return true;
            }
            catch (Exception ex)
            {
                contxt.Transaction.Rollback();
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 插入领料单
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="threePacketsOfTheRepairBill">三包外返修处置单主表信息</param>
        /// <param name="dtListOfStorageID">库房信息</param>
        /// <param name="threePacketsOfTheRepairList">三包外返修处置单明细信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool InsertMaterialRequisition(DepotManagementDataContext dataContext,YX_ThreePacketsOfTheRepairBill threePacketsOfTheRepairBill, 
            DataTable dtListOfStorageID, DataTable threePacketsOfTheRepairList,out string error)
        {
            error = null;

            try
            {
                if (dtListOfStorageID == null || dtListOfStorageID.Rows.Count == 0)
                {
                    return true;
                }
                else
                {
                    for (int k = 0; k < dtListOfStorageID.Rows.Count; k++)
                    {
                        //领表主表信息
                        MaterialRequisitionServer serverMaterialBill = new MaterialRequisitionServer();

                        string strBillID = m_serverAssignBill.AssignNewNo(serverMaterialBill, CE_BillTypeEnum.领料单.ToString());

                        S_MaterialRequisition lnqMaterial = new S_MaterialRequisition();

                        lnqMaterial.Bill_ID = strBillID;
                        lnqMaterial.Bill_Time = ServerModule.ServerTime.Time;
                        lnqMaterial.AssociatedBillNo = threePacketsOfTheRepairBill.Bill_ID;
                        lnqMaterial.AssociatedBillType = "三包外返修处置单";
                        lnqMaterial.BillStatus = "已出库";
                        lnqMaterial.Department = m_serverDepartment.GetDeptInfoFromPersonnelInfo(threePacketsOfTheRepairBill.WorkShopPersonnel).Rows[0]["DepartmentCode"].ToString();
                        lnqMaterial.DepartmentDirector = threePacketsOfTheRepairBill.WorkshopManagerPersonnel;
                        lnqMaterial.DepotManager = threePacketsOfTheRepairBill.StockPersonnel;
                        lnqMaterial.FetchCount = 0;
                        lnqMaterial.FetchType = "零星领料";
                        lnqMaterial.FillInPersonnel = threePacketsOfTheRepairBill.WorkShopPersonnel;
                        lnqMaterial.FillInPersonnelCode = UniversalFunction.GetPersonnelCode(threePacketsOfTheRepairBill.WorkShopPersonnel);
                        lnqMaterial.ProductType = "";
                        lnqMaterial.PurposeCode = UniversalFunction.GetPurpose(CE_PickingPurposeProperty.三包外维修).Code;
                        lnqMaterial.Remark = "由三包外返修处置单自动生成，对应的三包外返修处置单号为" + threePacketsOfTheRepairBill.Bill_ID;
                        lnqMaterial.StorageID = dtListOfStorageID.Rows[k][0].ToString();
                        lnqMaterial.OutDepotDate = ServerTime.Time;

                        if (!serverMaterialBill.AutoCreateBill(dataContext, lnqMaterial, out error))
                        {
                            return false;
                        }

                        for (int i = 0; i < threePacketsOfTheRepairList.Rows.Count; i++)
                        {

                            if (!Convert.ToBoolean(threePacketsOfTheRepairList.Rows[i]["是否为客户责任"])
                                && threePacketsOfTheRepairList.Rows[i][13].ToString().Trim() == dtListOfStorageID.Rows[k][0].ToString().Trim())
                            {

                                var varStock = from a in dataContext.S_Stock
                                               where a.StorageID == dtListOfStorageID.Rows[k][0].ToString()
                                               && a.GoodsID == Convert.ToInt32(threePacketsOfTheRepairList.Rows[i]["物品ID"])
                                               && a.BatchNo == threePacketsOfTheRepairList.Rows[i]["批次号"].ToString()
                                               && a.Provider == threePacketsOfTheRepairList.Rows[i][14].ToString()
                                               select a;

                                if (varStock.Count() != 1)
                                {
                                    error = "库存信息不唯一或者为空";
                                    return false;
                                }
                                else
                                {
                                    if (varStock.Single().GoodsStatus == 3)
                                    {
                                        error = "【" + varStock.Single().GoodsCode + "】 【" + varStock.Single().GoodsName + "】 【" 
                                            + varStock.Single().Spec + "】【" 
                                            + threePacketsOfTheRepairList.Rows[i]["批次号"].ToString() 
                                            + "】物品库存状态为“隔离”不允许出库";

                                        return false;
                                    }
                                }


                                S_MaterialRequisitionGoods lnqMaterialGoods = new S_MaterialRequisitionGoods();

                                lnqMaterialGoods.Bill_ID = strBillID;
                                lnqMaterialGoods.BasicCount = 0;
                                lnqMaterialGoods.BatchNo = threePacketsOfTheRepairList.Rows[i]["批次号"].ToString();
                                lnqMaterialGoods.GoodsID = Convert.ToInt32(threePacketsOfTheRepairList.Rows[i]["物品ID"]);
                                lnqMaterialGoods.ProviderCode = threePacketsOfTheRepairList.Rows[i][14].ToString();
                                lnqMaterialGoods.RealCount = Convert.ToDecimal(threePacketsOfTheRepairList.Rows[i]["领用数量"]);
                                lnqMaterialGoods.Remark = threePacketsOfTheRepairList.Rows[i]["备注"].ToString();
                                lnqMaterialGoods.RequestCount = Convert.ToDecimal(threePacketsOfTheRepairList.Rows[i]["领用数量"]);
                                lnqMaterialGoods.ShowPosition = 1;

                                MaterialRequisitionGoodsServer serverMaterialGoods = new MaterialRequisitionGoodsServer();

                                if (!serverMaterialGoods.AutoCreateGoods(dataContext, lnqMaterialGoods, out error))
                                {
                                    return false;
                                }

                            }
                        }

                        dataContext.SubmitChanges();

                        new MaterialRequisitionServer().OpertaionDetailAndStock(dataContext, lnqMaterial);

                        dataContext.SubmitChanges();
                    }

                }

                return true;
            }
            catch (Exception ex)
            {
                error = error + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 匹配物品库存信息
        /// </summary>
        /// <param name="threePacketsOfTheRepairList">数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>返回数据集</returns>
        DataTable GetStorageTable(DataTable threePacketsOfTheRepairList,out string error)
        {
            error = null;

            threePacketsOfTheRepairList.Columns.Add("StroageID");
            threePacketsOfTheRepairList.Columns.Add("Provider");

            for (int i = 0; i < threePacketsOfTheRepairList.Rows.Count; i++)
            {
                F_GoodsPlanCost lnqGoods =
                    m_serverBasicGoods.GetGoodsInfo(Convert.ToInt32(threePacketsOfTheRepairList.Rows[i]["物品ID"]));

                string strSql = "select * from S_Stock where GoodsID = "
                    + Convert.ToInt32(threePacketsOfTheRepairList.Rows[i]["物品ID"]) + " and StorageID in( '01','08','11') and BatchNo = '"
                    + threePacketsOfTheRepairList.Rows[i]["批次号"].ToString() + "' and ExistCount >= " 
                    + Convert.ToDecimal(threePacketsOfTheRepairList.Rows[i]["领用数量"]) + " order by StorageID desc";

                DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

                if (dtTemp != null && dtTemp.Rows.Count != 0)
                {
                    threePacketsOfTheRepairList.Rows[i][13] = dtTemp.Rows[0]["StorageID"].ToString();
                    threePacketsOfTheRepairList.Rows[i][14] = dtTemp.Rows[0]["Provider"].ToString();
                }
                else
                {
                    error = "图号型号 【" + lnqGoods.GoodsCode
                        + "】，物品名称 【" + lnqGoods.GoodsName
                        + "】， 规格 【" + lnqGoods.Spec
                        + "】，批次号 【" + threePacketsOfTheRepairList.Rows[i]["批次号"].ToString()
                        + "】 库存不足，请重新核对";
                    return null;
                }
            }

            return threePacketsOfTheRepairList;
        }

        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        public bool DeleteBill(string billID,out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext contxt = CommentParameter.DepotDataContext;

                var varDataBill = from a in contxt.YX_ThreePacketsOfTheRepairBill
                              where a.Bill_ID == billID
                              select a;

                contxt.YX_ThreePacketsOfTheRepairBill.DeleteAllOnSubmit(varDataBill);

                var varDataList = from b in contxt.YX_ThreePacketsOfTheRepairList
                                  where b.Bill_ID == billID
                                  select b;

                contxt.YX_ThreePacketsOfTheRepairList.DeleteAllOnSubmit(varDataList);

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
        /// 插入营销出库业务
        /// </summary>
        /// <param name="contxt">数据上下文</param>
        /// <param name="threePacket">三包外售后返修单信息</param>
        /// <param name="dtListOfStorageID">库房信息</param>
        /// <param name="listTable">明细信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>插入成功返回True，插入失败返回False</returns>
        bool InsertYXCK(DepotManagementDataContext contxt,YX_ThreePacketsOfTheRepairBill threePacket,
            DataTable dtListOfStorageID, DataTable listTable,out string error)
        {
            error = null;
            int intDJID = 0;
            SellIn serverSellIn = new SellIn();

            try
            {
                if (dtListOfStorageID == null || dtListOfStorageID.Rows.Count == 0)
                {
                    return true;
                }
                else
                {
                    for (int k = 0; k < dtListOfStorageID.Rows.Count; k++)
                    {

                        string strBillID = m_serverAssignBill.AssignNewNo(serverSellIn, CE_BillTypeEnum.营销出库单.ToString());

                        S_MarketingBill lnqMarketingBill = new S_MarketingBill();

                        lnqMarketingBill.AffirmDate = ServerTime.Time;
                        lnqMarketingBill.Date = Convert.ToDateTime(threePacket.FoundDate);
                        lnqMarketingBill.DJH = strBillID;
                        lnqMarketingBill.DJZT_FLAG = "已确认";
                        lnqMarketingBill.KFRY = BasicInfo.LoginID;

                        string strDepartment = m_serverDepartment.GetDeptInfoFromPersonnelInfo(threePacket.FoundPersonnel).Rows[0]["DepartmentCode"].ToString();

                        lnqMarketingBill.LRKS = strDepartment;
                        lnqMarketingBill.LRRY = UniversalFunction.GetPersonnelCode(threePacket.FoundPersonnel);
                        lnqMarketingBill.SHRY = m_serverPersonnel.GetFuzzyDeptDirector(strDepartment).ToList()[0].工号.ToString();
                        lnqMarketingBill.ShDate = ServerTime.Time;
                        lnqMarketingBill.ObjectDept = "QT";
                        lnqMarketingBill.Remark = "由三包外返修处理单【" + threePacket.Bill_ID + "】自动生成";
                        lnqMarketingBill.StorageID = dtListOfStorageID.Rows[k][0].ToString();
                        lnqMarketingBill.YWFS = "三包外返修出库";
                        lnqMarketingBill.YWLX = "出库";

                        contxt.S_MarketingBill.InsertOnSubmit(lnqMarketingBill);

                        contxt.SubmitChanges();

                        var varID = from a in contxt.S_MarketingBill
                                    where a.DJH == lnqMarketingBill.DJH
                                    select a;

                        if (varID.Count() != 1)
                        {
                            error = "数据不唯一或者为空";
                            return false;
                        }
                        else
                        {
                            intDJID = varID.Single().ID;
                        }

                        for (int i = 0; i < listTable.Rows.Count; i++)
                        {
                            if (Convert.ToBoolean(listTable.Rows[i]["是否为客户责任"])
                                && listTable.Rows[i][13].ToString().Trim() == dtListOfStorageID.Rows[k][0].ToString().Trim())
                            {
                                var varStock = from a in contxt.S_Stock
                                               where a.StorageID == dtListOfStorageID.Rows[k][0].ToString()
                                               && a.GoodsID == Convert.ToInt32(listTable.Rows[i]["物品ID"])
                                               && a.BatchNo == listTable.Rows[i]["批次号"].ToString()
                                               && a.Provider == listTable.Rows[i][14].ToString()
                                               select a;

                                if (varStock.Count() != 1)
                                {
                                    error = "库存信息不唯一或者为空";
                                    return false;
                                }
                                else
                                {
                                    if (varStock.Single().GoodsStatus == 3)
                                    {
                                        error = "【" + varStock.Single().GoodsCode + "】 【" + varStock.Single().GoodsName + "】 【"
                                            + varStock.Single().Spec + "】【"
                                            + listTable.Rows[i]["批次号"].ToString()
                                            + "】物品库存状态为“隔离”不允许出库";

                                        return false;
                                    }
                                }

                                S_MarketingList lnqMarketingList = new S_MarketingList();

                                lnqMarketingList.BatchNo = listTable.Rows[i]["批次号"].ToString();
                                lnqMarketingList.Count = Convert.ToDecimal(listTable.Rows[i]["领用数量"]);
                                lnqMarketingList.CPID = listTable.Rows[i]["物品ID"].ToString();
                                lnqMarketingList.DJ_ID = intDJID;
                                lnqMarketingList.ReMark = listTable.Rows[i]["备注"].ToString();
                                lnqMarketingList.Price = Math.Round(Convert.ToDecimal(listTable.Rows[i]["单价"]) * 
                                    Convert.ToDecimal(listTable.Rows[i]["领用数量"]), 2);
                                lnqMarketingList.Provider = listTable.Rows[i][14].ToString();
                                lnqMarketingList.UnitPrice = Convert.ToDecimal(listTable.Rows[i]["单价"]);

                                contxt.S_MarketingList.InsertOnSubmit(lnqMarketingList);
                            }
                        }

                        contxt.SubmitChanges();

                        serverSellIn.OperationDetailAndStock_Out(contxt, lnqMarketingBill);
                        contxt.SubmitChanges();
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
        /// 对明细信息进行更新/删除/添加
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="listTable">明细数据表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool UpdateList(string billID,DataTable listTable,out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext contxt = CommentParameter.DepotDataContext;

                var varData = from a in contxt.YX_ThreePacketsOfTheRepairList
                              where a.Bill_ID == billID
                              select a;

                contxt.YX_ThreePacketsOfTheRepairList.DeleteAllOnSubmit(varData);

                for (int i = 0; i < listTable.Rows.Count; i++)
                {
                    YX_ThreePacketsOfTheRepairList lnqList = new YX_ThreePacketsOfTheRepairList();

                    lnqList.BatchNo = listTable.Rows[i]["批次号"].ToString();
                    lnqList.Bill_ID = billID;
                    lnqList.GoodsID = Convert.ToInt32(listTable.Rows[i]["物品ID"]);
                    lnqList.PickCount = Convert.ToDecimal(listTable.Rows[i]["领用数量"]);
                    lnqList.Remark = listTable.Rows[i]["备注"].ToString();
                    lnqList.UnitPrice = Convert.ToDecimal(listTable.Rows[i]["单价"]);
                    lnqList.DutyBelong = Convert.ToBoolean(listTable.Rows[i]["是否为客户责任"]);

                    contxt.YX_ThreePacketsOfTheRepairList.InsertOnSubmit(lnqList);
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
        /// 获得明细快捷选择列表
        /// </summary>
        /// <param name="productType">产品类型</param>
        /// <returns>返回TABLE </returns>
        public DataTable GetShortcutDetail(string productType)
        {
            string error = null;

            try
            {
                Hashtable hsTable = new Hashtable();
                hsTable.Add("@ProductType", productType);

                DataTable tempTable = GlobalObject.DatabaseServer.QueryInfoPro("YX_Select_ThreePacketsOfTheRepair_ShortCutDetail", 
                    hsTable, out error);

                if (error != null && error.Trim().Length > 0)
                {
                    throw new Exception(error);
                }

                return tempTable;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
