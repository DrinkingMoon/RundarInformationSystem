/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  MaterialsTransfer.cs
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
using ServerModule;
using System.Reflection;
using System.Collections.Generic;
using System.IO;

namespace Service_Manufacture_WorkShop
{
    /// <summary>
    /// 车间物料转换单单据状态
    /// </summary>
    public enum MaterialsTransferStatus
    {
        新建单据,
        等待审核,
        等待确认,
        单据已完成
    }

    /// <summary>
    /// 转换方式
    /// </summary>
    public enum MaterialsTransferConvertType
    {
        组装,
        拆卸,
        维修,
        加工
    }

    /// <summary>
    /// 车间物料转换单据服务
    /// </summary>
    public class MaterialsTransfer : IMaterialsTransfer
    {
        /// <summary>
        /// 检查某业务是否存在
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">业务号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.WS_MaterialsTransfer
                          where a.BillNo == billNo
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
            string sql = "SELECT * FROM [DepotManagement].[dbo].[WS_MaterialsTransfer] where BillNo = '" + billNo + "'";

            System.Data.DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 获得单据信息
        /// </summary>
        /// <returns>返回单据信息</returns>
        public DataTable GetBillInfo()
        {
            string strSql = "select * from View_WS_MaterialsTransfer order by 单据号 desc";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得单据明细
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>成功返回True,失败返回False</returns>
        public DataTable GetListInfo(string billNo)
        {
            string strSql = "select * from View_WS_MaterialsTransferList where 单据号 = '" + billNo + "'";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得单条单据信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回单据信息</returns>
        public WS_MaterialsTransfer GetBillSingle(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.WS_MaterialsTransfer
                          where a.BillNo == billNo
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
        /// 删除单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        public bool DeleteBill(string billNo, out string error)
        {
            error = null;

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            try
            {
                var varData = from a in ctx.WS_MaterialsTransfer
                              where a.BillNo == billNo
                              select a;

                if (varData.Count() != 1)
                {
                    throw new Exception("数据不唯一");
                }
                else
                {
                    WS_MaterialsTransfer tempBill = varData.Single();

                    if (tempBill.BillStatus == MaterialsTransferStatus.单据已完成.ToString())
                    {
                        throw new Exception("单据已完成，无法删除");
                    }

                    if (tempBill.Proposer != BasicInfo.LoginName)
                    {
                        throw new Exception("只有申请人本人才能删除单据");
                    }
                }

                ctx.WS_MaterialsTransfer.DeleteAllOnSubmit(varData);

                IWorkShopProductCode serverProductCode = ServerModuleFactory.GetServerModule<IWorkShopProductCode>();

                serverProductCode.DeleteProductCodeDetail(ctx, billNo);

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
        /// 检测维修信息
        /// </summary>
        /// <param name="list">检测信息</param>
        public void CheckRepair(DataTable list)
        {
            string error = "";
            string msg = "";

            DataTable tableBefore = DataSetHelper.SiftDataTable(list, "操作类型 = "
                + (int)CE_SubsidiaryOperationType.物料转换前, out error);
            DataTable tableAfter = DataSetHelper.SiftDataTable(list, "操作类型 = "
                + (int)CE_SubsidiaryOperationType.物料转换后, out error);

            int goodsBefore = GetASSYGoodsID(tableBefore);
            int goodsAfter = GetASSYGoodsID(tableAfter);

            tableBefore = DataSetHelper.SiftDataTable(tableBefore, "物品ID <> " + goodsBefore, out error);
            tableAfter = DataSetHelper.SiftDataTable(tableAfter, "物品ID <> " + goodsAfter, out error);

            if (goodsBefore == 0)
            {
                throw new Exception("在转换前明细中为找到分总成或者总成信息");
            }

            if (goodsAfter == 0)
            {
                throw new Exception("在转换后明细中为找到分总成或者总成信息");
            }

            if (goodsBefore == goodsAfter)
            {
                tableBefore = DataSetHelper.SelectGroupByInto("Before", tableBefore, " 物品ID, SUM(数量) 数量", "", "物品ID");
                foreach (DataRow drTemp in tableBefore.Rows)
                {
                    drTemp["数量"] = Convert.ToInt32(drTemp["数量"]);
                }
                tableBefore.TableName = "【转换前】";

                tableAfter = DataSetHelper.SelectGroupByInto("After", tableAfter, " 物品ID, SUM(数量) 数量", "", "物品ID");
                foreach (DataRow drTemp in tableAfter.Rows)
                {
                    drTemp["数量"] = Convert.ToInt32(drTemp["数量"]);
                }
                tableAfter.TableName = "【转换后】";

                msg = MatchTableOrganizationMessage(tableBefore, tableAfter, "物品ID", "数量");

            }
            else
            {
                tableBefore = DataSetHelper.SelectGroupByInto("Before", tableBefore, " 物品ID, 物品名称, SUM(数量) 数量", "", "物品ID, 物品名称");

                foreach (DataRow drTemp in tableBefore.Rows)
                {
                    drTemp["数量"] = Convert.ToInt32(drTemp["数量"]);
                } 
                tableBefore.TableName = "【转换前】";

                tableAfter = DataSetHelper.SelectGroupByInto("After", tableAfter, " 物品ID, 物品名称, SUM(数量) 数量", "", "物品ID, 物品名称");
                foreach (DataRow drTemp in tableAfter.Rows)
                {
                    drTemp["数量"] = Convert.ToInt32(drTemp["数量"]);
                } 
                tableAfter.TableName = "【转换后】";

                msg = MatchTableOrganizationMessage(tableBefore, tableAfter, "物品名称", "数量");
            }

            DataTable tableBeforeBom = UniversalFunction.GetBomStruct(goodsAfter, null);
            DataTable tableAfterBom = UniversalFunction.GetBomStruct(goodsBefore, null);

            bool flag = false;

            foreach (DataRow drBefore in tableBefore.Rows)
            {
                if (tableBeforeBom.Select(" 物品ID = " + Convert.ToInt32(drBefore["物品ID"]), "").Length == 0)
                {
                    if (!flag)
                    {
                        msg += "\n\n 【转换前】不存在BOM表的物品有：\n";
                        flag = true;
                    }

                    msg += UniversalFunction.GetGoodsMessage(Convert.ToInt32(drBefore["物品ID"])) + "\n";
                }
            }

            flag = false;

            foreach (DataRow drAfter in tableAfter.Rows)
            {
                if (tableBeforeBom.Select(" 物品ID = " + Convert.ToInt32(drAfter["物品ID"]), "").Length == 0)
                {
                    if (!flag)
                    {
                        msg += "\n\n 【转换后】不存在BOM表的物品有：\n";
                        flag = true;
                    }

                    msg += UniversalFunction.GetGoodsMessage(Convert.ToInt32(drAfter["物品ID"])) + "\n";
                }
            }

            if (msg.Length > 0)
            {
                throw new Exception(msg);
            }
        }

        /// <summary>
        /// 获得匹配错误信息
        /// </summary>
        /// <param name="leftTable">左表</param>
        /// <param name="rightTable">右表</param>
        /// <param name="goodsColumnsName">关键字段名</param>
        /// <param name="countColumnsName">匹配字段名</param>
        /// <returns>返回错误信息</returns>
        string MatchTableOrganizationMessage(DataTable leftTable, DataTable rightTable, string goodsColumnsName, string countColumnsName)
        {
            string error = "";
            string msg = "";
            int goodsID = 0;

            DataTable infoTable = DataSetHelper.MatchTable(leftTable, rightTable, goodsColumnsName, countColumnsName);

            View_F_GoodsPlanCost tempGoods = new View_F_GoodsPlanCost();

            DataTable temp = GlobalObject.DataSetHelper.SiftDataTable(infoTable, "L" + goodsColumnsName + " = R" + goodsColumnsName, out error);

            if (temp.Rows.Count > 0)
            {
                msg += "\n\n数量不匹配的物品有：\n";

                foreach (DataRow dr in temp.Rows)
                {
                    if (goodsColumnsName != "物品ID")
                    {
                        goodsID = Convert.ToInt32(leftTable.Select(goodsColumnsName + " = '" + dr["L" + goodsColumnsName] + "'", "")[0]["物品ID"]);
                    }
                    else
                    {
                        goodsID = Convert.ToInt32(dr["L" + goodsColumnsName]);
                    }

                    msg += UniversalFunction.GetGoodsMessage(goodsID) + ";\n";
                }
            }

            temp = GlobalObject.DataSetHelper.SiftDataTable(infoTable, "R" + goodsColumnsName + " = '0'", out error);

            if (temp.Rows.Count > 0)
            {
                msg += "\n\n" + leftTable.TableName + "存在且" + rightTable.TableName + "中不存在的物品有：\n";

                foreach (DataRow dr in temp.Rows)
                {
                    if (goodsColumnsName != "物品ID")
                    {
                        goodsID = Convert.ToInt32(leftTable.Select(goodsColumnsName + " = '" + dr["L" + goodsColumnsName] + "'", "")[0]["物品ID"]);
                    }
                    else
                    {
                        goodsID = Convert.ToInt32(dr["L" + goodsColumnsName]);
                    }

                    msg += UniversalFunction.GetGoodsMessage(goodsID) + ";\n";
                }
            }

            temp = GlobalObject.DataSetHelper.SiftDataTable(infoTable, "L" + goodsColumnsName + " = '0'", out error);

            if (temp.Rows.Count > 0)
            {
                msg += "\n\n" + rightTable.TableName + "存在且" + leftTable.TableName + "中不存在的物品有：\n";

                foreach (DataRow dr in temp.Rows)
                {
                    if (goodsColumnsName != "物品ID")
                    {
                        goodsID = Convert.ToInt32(rightTable.Select(goodsColumnsName + " = '" + dr["R" + goodsColumnsName] + "'", "")[0]["物品ID"]);
                    }
                    else
                    {
                        goodsID = Convert.ToInt32(dr["R" + goodsColumnsName]);
                    }

                    msg += UniversalFunction.GetGoodsMessage(goodsID) + ";\n";
                }
            }

            return msg;
        }

        /// <summary>
        /// 组装检测装配BOM
        /// </summary>
        /// <param name="list">匹配信息</param>
        /// <param name="wsCode">车间代码</param>
        /// <param name="convertType">转换类型</param>
        public void CheckBom(DataTable list, string wsCode, MaterialsTransferConvertType convertType)
        {
            string error  = "";
            string msg = "";

            int operatorAfter = 0;
            int operatorBefore = 0;

            if (convertType == MaterialsTransferConvertType.组装)
            {
                operatorAfter = (int)CE_SubsidiaryOperationType.物料转换后;
                operatorBefore = (int)CE_SubsidiaryOperationType.物料转换前;
            }
            else if(convertType == MaterialsTransferConvertType.拆卸)
            {
                operatorAfter = (int)CE_SubsidiaryOperationType.物料转换前;
                operatorBefore = (int)CE_SubsidiaryOperationType.物料转换后;
            }
            else
            {
                throw new Exception("检测方式与转换方式不匹配");
            }

            DataTable tableAfter = DataSetHelper.SiftDataTable(list, "操作类型 = "
                + operatorAfter, out error);

            int goodsID = Convert.ToInt32(tableAfter.Rows[0]["物品ID"]);
            decimal operationCount = Convert.ToDecimal(tableAfter.Rows[0]["数量"]);

            DataTable tempRowTable = DataSetHelper.SelectGroupByInto("tempTable", list,
                "物品ID,Sum(数量) 数量", "操作类型 = " + operatorBefore + " and 物品ID <> " + goodsID, "物品ID");

            foreach (DataRow drTemp in tempRowTable.Rows)
            {
                drTemp["数量"] = Convert.ToInt32(drTemp["数量"]);
            }

            tempRowTable.TableName = "单据";

            DataTable tempRowTable1 = UniversalFunction.GetBomStruct(goodsID, wsCode);

            foreach (DataRow dr in tempRowTable1.Rows)
            {
                dr["数量"] = Convert.ToInt32(Convert.ToDecimal(dr["数量"]) * operationCount);
            }

            tempRowTable1.TableName = "装配BOM";

            msg = MatchTableOrganizationMessage(tempRowTable, tempRowTable1, "物品ID", "数量");

            if (msg.Length > 0)
            {
                throw new Exception(msg);
            }
        }

        /// <summary>
        /// 单据明细操作
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">单据号</param>
        /// <param name="list">单据明细</param>
        void ListControl(DepotManagementDataContext ctx, WS_MaterialsTransfer billInfo, DataTable list)
        {
            try
            {
                MaterialsTransferConvertType convertType = 
                    GeneralFunction.StringConvertToEnum<MaterialsTransferConvertType>(billInfo.ConvertType);

                DataRow[] rowTemp;

                switch (convertType)
                {
                    case MaterialsTransferConvertType.组装:

                        rowTemp = list.Select("操作类型 = " + (int)CE_SubsidiaryOperationType.物料转换后);

                        if (rowTemp.Length > 1)
                        {
                            throw new Exception("转换类型为" + convertType.ToString() +"时，转换后物料明细只能为一条");
                        }

                        if (!UniversalFunction.IsContainAssembly(rowTemp))
                        {
                            throw new Exception("转换后物料中未包含总成或者分总成");
                        }

                        //DataRow[] rowBeforeTemp = list.Select("操作类型 = " + (int)SubsidiaryOperationType.物料转换前);

                        //foreach (DataRow dr in rowBeforeTemp)
                        //{
                        //    if (dr["物品ID"] == rowTemp[0]["物品ID"])
                        //    {
                        //        throw new Exception("组装类型中转换前物料中不能包含转换后的物品");
                        //    }
                        //}

                        break;
                    case MaterialsTransferConvertType.维修:

                        rowTemp = list.Select("操作类型 = " + (int)CE_SubsidiaryOperationType.物料转换后);

                        if (!UniversalFunction.IsContainAssembly(rowTemp))
                        {
                            throw new Exception("转换后物料中未包含总成或者分总成");
                        }

                        rowTemp = list.Select("操作类型 = " + (int)CE_SubsidiaryOperationType.物料转换前);

                        if (!UniversalFunction.IsContainAssembly(rowTemp))
                        {
                            throw new Exception("转换前物料中未包含总成或者分总成");
                        }
                        break;
                    case MaterialsTransferConvertType.拆卸:

                        rowTemp = list.Select("操作类型 = " + (int)CE_SubsidiaryOperationType.物料转换前);

                        if (rowTemp.Length > 1)
                        {
                            throw new Exception("转换类型为" + convertType.ToString() + "时，转换前物料明细只能为一条");
                        }

                        if (!UniversalFunction.IsContainAssembly(rowTemp))
                        {
                            throw new Exception("转换前物料中未包含总成或者分总成");
                        }

                        //DataRow[] rowAfterTemp = list.Select("操作类型 = " + (int)SubsidiaryOperationType.物料转换后);

                        //foreach (DataRow dr in rowAfterTemp)
                        //{
                        //    if (dr["物品ID"] == rowTemp[0]["物品ID"])
                        //    {
                        //        throw new Exception("拆卸类型中转换后物料中不能包含转换前的物品");
                        //    }
                        //}
                        break;
                    case MaterialsTransferConvertType.加工:

                        rowTemp = list.Select("操作类型 = " + (int)CE_SubsidiaryOperationType.物料转换后);

                        foreach (DataRow dr in rowTemp)
                        {
                            if (!UniversalFunction.IsHomemadePart(Convert.ToInt32(dr["物品ID"])))
                            {
                                throw new Exception("转换类型为" + convertType.ToString() + "时，转换后物料明细只能为成品件");
                            }
                        }

                        if (rowTemp.Length > 1)
                        {
                            throw new Exception("转换类型为" + convertType.ToString() + "时，转换后物料明细只能为一条");
                        }

                        rowTemp = list.Select("操作类型 = " + (int)CE_SubsidiaryOperationType.物料转换前);

                        foreach (DataRow dr in rowTemp)
                        {
                            if (UniversalFunction.IsHomemadePart(Convert.ToInt32(dr["物品ID"])))
                            {
                                //财务允许 成品加工成成品 2014.4.29 modify by cjb
                                //throw new Exception("转换类型为" + convertType.ToString() + "时，转换前物料明细不能含有成品件");
                            }
                        }

                        if (rowTemp.Length > 1)
                        {
                            throw new Exception("转换类型为" + convertType.ToString() + "时，转换前物料明细只能为一条");
                        }

                        break;
                    default:
                        break;
                }

                var varData = from a in ctx.WS_MaterialsTransferList
                              where a.BillNo == billInfo.BillNo
                              select a;

                ctx.WS_MaterialsTransferList.DeleteAllOnSubmit(varData);

                IWorkShopProductCode serverProductCode = ServerModuleFactory.GetServerModule<IWorkShopProductCode>();

                foreach (DataRow dr in list.Rows)
                {
                    WS_MaterialsTransferList tempList = new WS_MaterialsTransferList();

                    tempList.BillNo = billInfo.BillNo;
                    tempList.BatchNo = dr["批次号"].ToString();
                    tempList.GoodsID = Convert.ToInt32(dr["物品ID"]);
                    tempList.OperationCount = Convert.ToDecimal(dr["数量"]);
                    tempList.Remark = dr["备注"].ToString();
                    tempList.OperationType = (int)dr["操作类型"];

                    if (!serverProductCode.CheckProductCodeCount(billInfo.BillNo, billInfo.WSCode, tempList.GoodsID,
                        tempList.OperationType,
                        Convert.ToDecimal(dr["数量"])))
                    {
                        throw new Exception(UniversalFunction.GetGoodsMessage(tempList.GoodsID) + " 产品编码数量与操作数量不一致");
                    }

                    if (list.Select(" 物品ID = " 
                        + tempList.GoodsID + " AND 批次号 = '" 
                        + tempList.BatchNo + "' AND 操作类型 = " 
                        + tempList.OperationType).Length > 1)
                    {
                        View_F_GoodsPlanCost tempGoodsInfo = UniversalFunction.GetGoodsInfo(tempList.GoodsID);

                        throw new Exception(UniversalFunction.GetGoodsMessage(tempList.GoodsID) + "【批次号】："
                            + tempList.BatchNo + "  数据重复，请重新核对");
                    }

                    ctx.WS_MaterialsTransferList.InsertOnSubmit(tempList);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 单据赋值
        /// </summary>
        /// <param name="bill">单据数据集</param>
        /// <param name="returnBill">返回单据</param>
        void AssignmentValue(WS_MaterialsTransfer bill, ref WS_MaterialsTransfer returnBill)
        {
            returnBill.BillNo = bill.BillNo;
            returnBill.ConvertType = bill.ConvertType;
            returnBill.BillStatus = bill.BillStatus;
            returnBill.Remark = bill.Remark;
            returnBill.Proposer = BasicInfo.LoginName;
            returnBill.ProposerDate = ServerTime.Time;
            returnBill.Affirm = null;
            returnBill.AffirmDate = null;
            returnBill.Audit = null;
            returnBill.AuditDate = null;
            returnBill.WSCode = bill.WSCode;
            
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="bill">单据信息</param>
        /// <param name="list">单据明细信息</param>
        /// <param name="listRequisitionBillNo">领料单号</param>
        void SaveInfo(DepotManagementDataContext ctx, WS_MaterialsTransfer bill, DataTable list)
        {
            WS_MaterialsTransfer tempBill = new WS_MaterialsTransfer();

            var varData = from a in ctx.WS_MaterialsTransfer
                          where a.BillNo == bill.BillNo
                          select a;

            bill.BillStatus = MaterialsTransferStatus.新建单据.ToString();

            switch (varData.Count())
            {
                case 0:
                    AssignmentValue(bill, ref tempBill);
                    ctx.WS_MaterialsTransfer.InsertOnSubmit(tempBill);
                    break;
                case 1:
                    tempBill = varData.Single();

                    if (tempBill.BillStatus == MaterialsTransferStatus.单据已完成.ToString())
                    {
                        throw new Exception("单据状态错误，请重新确认单据状态");
                    }

                    AssignmentValue(bill, ref tempBill);
                    break;
                default:
                    throw new Exception("数据错误，请重新确认");
            }

            ListControl(ctx, tempBill, list);
        }

        /// <summary>
        /// 保存单据
        /// </summary>
        /// <param name="bill">单据信息</param>
        /// <param name="list">明细信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        public bool SaveBill(WS_MaterialsTransfer bill, DataTable list, out string error)
        {
            error = null;

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            try
            {
                SaveInfo(ctx, bill, list);
                ctx.SubmitChanges();
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
        /// 申请单据
        /// </summary>
        /// <param name="bill">单据信息</param>
        /// <param name="list">明细信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        public bool ProposeBill(WS_MaterialsTransfer bill, DataTable list, out string error)
        {
            error = null;

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();

            try
            {
                SaveInfo(ctx, bill, list);
                ctx.SubmitChanges();

                var varData = from a in ctx.WS_MaterialsTransfer
                              where a.BillNo == bill.BillNo
                              select a;

                varData.Single().BillStatus = MaterialsTransferStatus.等待审核.ToString();

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
        /// 审核单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        public bool AuditBill(string billNo, out string error)
        {
            error = null;

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            try
            {
                var varData = from a in ctx.WS_MaterialsTransfer
                              where a.BillNo == billNo
                              select a;
                if (varData.Count() != 1)
                {
                    throw new Exception("数据错误，请重新确认");
                }
                else
                {
                    WS_MaterialsTransfer tempBill = varData.Single();

                    if (tempBill.BillStatus != MaterialsTransferStatus.等待审核.ToString())
                    {
                        throw new Exception("单据状态错误，请重新确认");
                    }

                    tempBill.BillStatus = MaterialsTransferStatus.等待确认.ToString();
                    tempBill.Audit = BasicInfo.LoginName;
                    tempBill.AuditDate = ServerTime.Time;

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
        /// 确认单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="list">明细信息</param>
        /// <param name="exception">异常信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        public bool AffirmBill(string billNo, DataTable list, string exception, out string error)
        {
            error = null;

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();

            try
            {
                var varData = from a in ctx.WS_MaterialsTransfer
                              where a.BillNo == billNo
                              select a;

                WS_MaterialsTransfer tempBill = new WS_MaterialsTransfer();

                if (varData.Count() == 1)
                {
                    tempBill = varData.Single();

                    if (tempBill.BillStatus != MaterialsTransferStatus.等待确认.ToString())
                    {
                        throw new Exception("单据状态错误，请重新确认");
                    }

                    tempBill.BillStatus = MaterialsTransferStatus.单据已完成.ToString();
                    tempBill.Affirm = BasicInfo.LoginName;
                    tempBill.AffirmDate = ServerTime.Time;

                }
                else
                {
                    throw new Exception("数据不唯一,请重新确认");
                }

                ListControl(ctx, tempBill, list);
                ctx.SubmitChanges();

                var varGoodsList = from a in ctx.WS_MaterialsTransferList
                                   join b in ctx.BASE_SubsidiaryOperationType
                                   on a.OperationType equals b.OperationType
                                   orderby b.DepartmentType
                                   where a.BillNo == tempBill.BillNo
                                   select a;

                foreach (WS_MaterialsTransferList tempItem in varGoodsList)
                {
                    WS_Subsidiary tempSubsidiary = new WS_Subsidiary();

                    tempSubsidiary.BatchNo = tempItem.BatchNo;
                    tempSubsidiary.BillNo = tempBill.BillNo;
                    tempSubsidiary.GoodsID = tempItem.GoodsID;
                    tempSubsidiary.OperationCount = tempItem.OperationCount;
                    tempSubsidiary.OperationType = tempItem.OperationType;
                    tempSubsidiary.Remark = tempItem.Remark;
                    tempSubsidiary.WSCode = tempBill.WSCode;
                    tempSubsidiary.Proposer = tempBill.Proposer;
                    tempSubsidiary.ProposerDate = (DateTime)tempBill.ProposerDate;
                    tempSubsidiary.Affirm = BasicInfo.LoginName;
                    tempSubsidiary.AffirmDate = ServerTime.Time;

                    IWorkShopStock serverStock = ServerModuleFactory.GetServerModule<IWorkShopStock>();
                    WS_WorkShopStock tempStock = serverStock.GetStockSingleInfo(tempSubsidiary.WSCode,
                        tempSubsidiary.GoodsID, tempSubsidiary.BatchNo);

                    tempSubsidiary.UnitPrice = tempStock == null ? 0 : tempStock.UnitPrice;

                    tempSubsidiary.BillTime = ServerTime.Time;

                    View_HR_Personnel tempHR = UniversalFunction.GetPersonnelInfo(tempSubsidiary.Proposer);

                    if (tempHR == null)
                    {
                        throw new Exception("申请人员不存在");
                    }

                    tempSubsidiary.Applicant = tempHR.部门名称;

                    serverStock.OperationSubsidiary(ctx, tempSubsidiary);
                }

                ctx.SubmitChanges();

                var varError = from a in ctx.WS_MaterialsTransferException
                               where a.BillNo == billNo
                               select a;

                ctx.WS_MaterialsTransferException.DeleteAllOnSubmit(varError);


                if (exception.Trim().Length > 0)
                {
                    WS_MaterialsTransferException tempException = new WS_MaterialsTransferException();

                    tempException.BillNo = billNo;
                    tempException.Exception = exception;

                    ctx.WS_MaterialsTransferException.InsertOnSubmit(tempException);
                }

                ctx.SubmitChanges();
                ctx.Transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                ctx.Transaction.Rollback();
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 获得领料单
        /// </summary>
        /// <param name="wsCode">车间代码</param>
        /// <returns>返回TABLE</returns>
        public DataTable GetRequisitionInfo(string wsCode)
        {
            string strSql = " select 领料单号 as 单号, 出库时间 as 日期,  用途说明, 备注, 领料类型, 产品类型," +
                            " 领料台数, 编制人, 库房名称, 关联单号 , 出库时间 as 查询日期 " +
                            " from View_S_MaterialRequisition as a inner join WS_WorkShopCode as b on LEN(a.领料部门) = LEN(b.DeptCode)" +
                            " and SUBSTRING(a.领料部门,1,LEN(b.DeptCode)) = b.DeptCode " +
                            " where 单据状态 = '已出库' and 出库时间 >= '" + ServerTime.Time.AddMonths(-3).ToShortDateString() + "' and b.WSCode = '" 
                            + wsCode + "' order by 出库时间 desc";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得领料单
        /// </summary>
        /// <param name="wsCode">车间代码</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>返回TABLE</returns>
        public DataTable GetRequisitionInfo(string wsCode, DateTime startTime, DateTime endTime)
        {
            string strSql = " select 领料单号 as 单号, 出库时间 as 日期,  用途说明, 备注, 领料类型, 产品类型," +
                            " 领料台数, 编制人, 库房名称, 关联单号 , 出库时间 as 查询日期 " +
                            " from View_S_MaterialRequisition as a inner join WS_WorkShopCode as b on LEN(a.领料部门) = LEN(b.DeptCode)" +
                            " and SUBSTRING(a.领料部门,1,LEN(b.DeptCode)) = b.DeptCode " +
                            " where 单据状态 = '已出库' and 出库时间 >= '" + startTime + "'and 出库时间 <= '" + endTime + "' and b.WSCode = '"
                            + wsCode + "' order by 出库时间 desc";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得物料转换单
        /// </summary>
        /// <param name="wsCode">车间代码</param>
        /// <returns>返回TABLE</returns>
        public DataTable GetMaterialsTransferInfo(string wsCode)
        {
            string strSql = "select *,确认时间 as 查询日期 from View_WS_MaterialsTransfer where 所属车间编码 = '" + wsCode
                + "' and 单据状态 = '" + MaterialsTransferStatus.单据已完成.ToString() + "' and 确认时间 >= '"
                + ServerTime.StartTime(ServerTime.Time.AddMonths(-1)) + "' and 确认时间 <= '"
                + ServerTime.EndTime(ServerTime.Time) +"' order by 确认时间 desc";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得物料转换单
        /// </summary>
        /// <param name="wsCode">车间代码</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>返回TABLE</returns>
        public DataTable GetMaterialsTransferInfo(string wsCode, DateTime startTime, DateTime endTime)
        {
            string strSql = "select *,确认时间 as 查询日期 from View_WS_MaterialsTransfer where 所属车间编码 = '" + wsCode
                + "' and 单据状态 = '" + MaterialsTransferStatus.单据已完成.ToString() + "' and 确认时间 >= '"
                + startTime + "' and 确认时间 <= '"+ endTime + "' order by 确认时间 desc";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 统计物料转换单的物品
        /// </summary>
        /// <param name="listBillNo">物料转换单单号列表</param>
        /// <param name="operationType">操作类型</param>
        /// <param name="showOperationType">显示操作类型</param>
        /// <param name="wsCode">所属车间</param>
        /// <returns>返回Table</returns>
        public DataTable SumMaterialsTransferGoods(List<string> listBillNo, int operationType, 
            int showOperationType, string wsCode)
        {
            string billNoSum = "";

            foreach (string item in listBillNo)
            {
                billNoSum += "'" + item + "',";
            }

            billNoSum = billNoSum.Substring(0, billNoSum.Length - 1);

            string strSql = " select 单据号,图号型号,物品名称,规格,批次号,SUM(数量) as 数量," +
                            " 单位," + showOperationType + " as 操作类型,'' as 备注,物品ID " +
                            " from View_WS_MaterialsTransferList as a inner join WS_MaterialsTransfer as b on a.单据号 = b.BillNo " +
                            " where 单据号 in (" + billNoSum + ") and 操作类型 = " + operationType + " and WSCode = '" + wsCode + "'" +
                            " group by 单据号,图号型号,物品名称,规格,批次号,单位,物品ID";

            DataTable tempTable =  GlobalObject.DatabaseServer.QueryInfo(strSql);

            return tempTable;
        }

        /// <summary>
        /// 核计领料单的物品
        /// </summary>
        /// <param name="listBillNo">领料单单号列表</param>
        /// <returns>返回Table</returns>
        public DataTable SumRequisitionGoods(List<string> listBillNo)
        {
            string billNoSum = "";

            foreach (string item in listBillNo)
            {
                billNoSum += "'" + item + "',";
            }

            billNoSum = billNoSum.Substring(0, billNoSum.Length - 1);

            string strSql = " select '' as 单据号,图号型号,物品名称,规格,BatchNo as 批次号,SUM( RealCount) as 数量," +
                            " 单位,"+ (int)CE_SubsidiaryOperationType.物料转换前 +" as 操作类型,'' as 备注,GoodsID as 物品ID " +
                            " from S_MaterialRequisitionGoods as a inner join View_F_GoodsPlanCost as b on a.GoodsID = b.序号" +
                            " where Bill_ID in (" + billNoSum + ")" +
                            " group by 图号型号,物品名称,规格,BatchNo,单位,GoodsID";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得装配BOM零件
        /// </summary>
        /// <param name="productCode">型号</param>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回Table</returns>
        public DataTable GetAssemblingBomInfo(string productCode, int goodsID)
        {
            string error = "";

            Hashtable parameters = new Hashtable();

            parameters.Add("@ProductCode", productCode);
            parameters.Add("@GoodsID", goodsID);

            return GlobalObject.DatabaseServer.QueryInfoPro("WS_MaterialsTransfer_InputAssemblingBom", parameters, out error);
        }

        /// <summary>
        /// 获取对应单据的成品箱的电子档案的选配件合计
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回Table</returns>
        public DataTable GetProductChoseConfect(string billNo)
        {
            string strSql = " select d.序号 as 物品ID,b.BatchNo as 批次号,SUM( b.Counts ) as 数量 " +
                            " from (select Distinct AccessoryCode from P_ChoseConfectTable) as a  "+
                            " inner join P_ElectronFile as b on a.AccessoryCode = b.GoodsCode "+
                            " inner join (select a.图号型号 + ' ' + b.ProductCode as ProductCode ,b.BillNo " +
                            " from View_F_GoodsPlanCost as a inner join WS_ProductCodeDetail as b on a.序号 = b.GoodsID) as c "+
                            " on c.ProductCode = b.ProductCode "+
                            " inner join View_F_GoodsPlanCost as d on b.GoodsCode = d.图号型号 and b.GoodsName = d.物品名称 and b.Spec = d.规格 " +
                            " where c.BillNo = '" + billNo + "' " +
                            " group by d.序号,b.BatchNo";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);

        }

        /// <summary>
        /// 回退单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="billStatus">回退后的单据状态</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <param name="rebackReason">回退原因</param>
        /// <returns>回退成功返回True，回退失败返回False</returns>
        public bool ReturnBill(string billNo, MaterialsTransferStatus billStatus, out string error, string rebackReason)
        {
            error = null;
            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            try
            {
                var varData = from a in dataContext.WS_MaterialsTransfer
                              where a.BillNo == billNo
                              select a;

                string strMsg = "";

                if (varData.Count() != 1)
                {
                    throw new Exception("数据不唯一或者为空");
                }
                else
                {
                    WS_MaterialsTransfer lnqTemp = varData.Single();

                    IBillMessagePromulgatorServer billMessageServer =
                        BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

                    billMessageServer.BillType = CE_BillTypeEnum.车间物料转换单.ToString();

                    switch (billStatus)
                    {
                        case MaterialsTransferStatus.新建单据:

                            strMsg = string.Format("{0}号车间物料转换单已回退，请您重新处理单据; 回退原因为"
                                + rebackReason, billNo);

                            billMessageServer.PassFlowMessage(billNo, strMsg, BillFlowMessage_ReceivedUserType.用户,
                                UniversalFunction.GetPersonnelCode(lnqTemp.Proposer));

                            break;
                        case MaterialsTransferStatus.等待审核:

                            strMsg = string.Format("{0}号车间物料转换单已回退，请您重新处理单据; 回退原因为" 
                                + rebackReason, billNo);

                            billMessageServer.PassFlowMessage(billNo, strMsg, BillFlowMessage_ReceivedUserType.用户,
                                UniversalFunction.GetPersonnelCode(lnqTemp.Audit));

                            break;
                        default:
                            break;
                    }

                    lnqTemp.BillStatus = billStatus.ToString();
                    dataContext.SubmitChanges();
                    return true;
                }

            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 获取列表中的总成或者分总成的GoodsID
        /// </summary>
        /// <param name="list">列表</param>
        /// <returns>返回GoodsID</returns>
        public int GetASSYGoodsID(DataTable list)
        {
            int goodsID = 0;

            foreach (DataRow dr in list.Rows)
            {
                if (UniversalFunction.IsProduct(Convert.ToInt32(dr["物品ID"])))
                {
                    goodsID = Convert.ToInt32(dr["物品ID"]);
                    break;
                }
                else if (UniversalFunction.IsPointProduct(Convert.ToInt32(dr["物品ID"])))
                {
                    goodsID = Convert.ToInt32(dr["物品ID"]);
                }
            }

            return goodsID;
        }

        /// <summary>
        /// 导入电子档案
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="subsidiary">业务模式</param>
        /// <param name="convertType">转换模式</param>
        /// <param name="wsCode">车间代码</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>返回TABLE</returns>
        public DataTable GetElectronFileInfo(string billNo, int goodsID, 
            CE_SubsidiaryOperationType subsidiary, MaterialsTransferConvertType convertType, 
            string wsCode, DateTime startTime, DateTime endTime)
        {

            string error = "";

            Hashtable parameters = new Hashtable();

            parameters.Add("@BillNo", billNo);
            parameters.Add("@GoodsID", goodsID);
            parameters.Add("@OperationType", (int)subsidiary);
            parameters.Add("@RepairFlag", convertType == MaterialsTransferConvertType.维修);
            parameters.Add("@WSCode", wsCode);
            parameters.Add("@StartTime", startTime.ToLongDateString());
            parameters.Add("@EndTime", endTime.ToLongDateString());

            return GlobalObject.DatabaseServer.QueryInfoPro("WS_GetAssemblingElectronFile", parameters, out error);
        }

        /// <summary>
        /// 合计TABLE
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="temp">合计源</param>
        /// <param name="subsidiary">业务类型</param>
        /// <returns>返回Table</returns>
        public DataTable SumTable(DataTable source, DataTable temp, int subsidiary)
        {
            DataTable temp1 = new DataTable();

            temp1.Columns.Add("物品ID");
            temp1.Columns.Add("批次号");
            temp1.Columns.Add("数量");


            if (source != null)
            {
                foreach (DataRow dr in source.Rows)
                {
                    DataRow newDr = temp1.NewRow();

                    newDr["物品ID"] = dr["物品ID"];
                    newDr["批次号"] = dr["批次号"];
                    newDr["数量"] = dr["数量"];

                    temp1.Rows.Add(newDr);
                }
            }

            if (temp != null)
            {
                foreach (DataRow dr1 in temp.Rows)
                {
                    DataRow newDr1 = temp1.NewRow();

                    newDr1["物品ID"] = dr1["物品ID"];
                    newDr1["批次号"] = dr1["批次号"];
                    newDr1["数量"] = dr1["数量"];

                    temp1.Rows.Add(newDr1);
                }
            }

            DataTable tempTable = GlobalObject.DataSetHelper.SelectGroupByInto("tempTable", temp1, 
                " 物品ID,批次号,SUM(数量) 数量", "", "物品ID,批次号");


            DataTable tempTable1 = source.Clone();

            foreach (DataRow item in tempTable.Rows)
            {
                DataRow tempDataRow = tempTable1.NewRow();

                View_F_GoodsPlanCost tempGoodsInfo = UniversalFunction.GetGoodsInfo(Convert.ToInt32(item["物品ID"]));

                tempDataRow["单据号"] = "";
                tempDataRow["图号型号"] = tempGoodsInfo.图号型号;
                tempDataRow["物品名称"] = tempGoodsInfo.物品名称;
                tempDataRow["规格"] = tempGoodsInfo.规格;
                tempDataRow["批次号"] = item["批次号"].ToString();
                tempDataRow["数量"] = Convert.ToDecimal(item["数量"]);
                tempDataRow["单位"] = tempGoodsInfo.单位;
                tempDataRow["操作类型"] = subsidiary;
                tempDataRow["备注"] = "";
                tempDataRow["物品ID"] = tempGoodsInfo.序号;

                tempTable1.Rows.Add(tempDataRow);
            }

            return tempTable1;
        }

        /// <summary>
        /// 转换后明细的返修匹配
        /// </summary>
        /// <param name="beforeTable">转换前列表</param>
        /// <param name="goodsBefore">转换前GoodsID</param>
        /// <param name="goodsAfter">转换后GoodsID</param>
        /// <param name="billNo">单据号</param>
        /// <returns>返回Table</returns>
        public DataTable GetRepairMatch(DataTable beforeTable, int goodsBefore, int goodsAfter, string billNo)
        {
            DataTable result = new DataTable();

            result.Columns.Add("物品ID");
            result.Columns.Add("批次号");
            result.Columns.Add("数量");

            foreach (DataRow dr in beforeTable.Rows)
            {
                if (Convert.ToInt32(dr["物品ID"]) != goodsBefore)
                {
                    DataRow newDr = result.NewRow();

                    View_F_GoodsPlanCost goodsInfo = UniversalFunction.GetGoodsInfo(Convert.ToInt32(dr["物品ID"]));

                    string strSql =  " select a.GoodsCode,a.GoodsName,a.Spec,SUM(Counts) as Counts, c.序号 as GoodsID from P_ElectronFile as a inner join ( " +
                                     " select b.图号型号 + ' '+ ProductCode as ProductCode,BillNo,GoodsID,OperationType  " +
                                     " from WS_ProductCodeDetail as a inner join View_F_GoodsPlanCost as b on a.GoodsID = b.序号 " +
                                     " where BillNo = '" + billNo + "' and a.GoodsID = " + goodsBefore + " and a.OperationType = 9) as b " +
                                     " on a.ProductCode = b.ProductCode inner join View_F_GoodsPlanCost as c on "+
                                     " a.GoodsCode = c.图号型号 and a.GoodsName = c.物品名称 and a.Spec = c.规格 ";

                    if (goodsAfter == goodsBefore)
                    {
                        strSql += " where c.序号 = " + goodsInfo.序号;
                    }
                    else
                    {
                        strSql += " where a.GoodsName = '" + goodsInfo.物品名称 + "'";
                    }

                    strSql += " group by a.GoodsCode,a.GoodsName,a.Spec,c.序号";

                    DataTable tempTable = GlobalObject.DatabaseServer.QueryInfo(strSql);

                    if (tempTable != null && tempTable.Rows.Count != 0)
                    {
                        newDr["物品ID"] = Convert.ToInt32(tempTable.Rows[0]["GoodsID"]);
                        newDr["批次号"] = CE_BatchNoPrefix.WT.ToString() + ServerTime.Time.Year.ToString()
                            + Convert.ToInt32(tempTable.Rows[0]["GoodsID"]).ToString("D6");
                        newDr["数量"] = Convert.ToDecimal(dr["数量"]);
                    }
                    else
                    {
                        
                        View_F_GoodsPlanCost goodsInfo1 = UniversalFunction.GetGoodsInfo(goodsAfter);

                        strSql = " select PartCode,PartName,a.Spec,SUM(FittingCounts) as FittingCounts ,b.序号 as GoodsID " +
                                 " from P_AssemblingBom as a inner join View_F_GoodsPlanCost as b on a.PartCode = b.图号型号 " +
                                 " and a.PartName = b.物品名称 and a.Spec = b.规格 where ProductCode = '" + goodsInfo1.图号型号 + "' ";

                        if (goodsAfter == goodsBefore)
                        {
                            strSql += " and b.序号 = " + goodsInfo.序号;
                        }
                        else
                        {
                            strSql += " and PartName = '" + goodsInfo.物品名称 + "'";
                        }

                        strSql += " group by PartCode,PartName,a.Spec,b.序号";

                        tempTable = GlobalObject.DatabaseServer.QueryInfo(strSql);

                        if (tempTable != null && tempTable.Rows.Count != 0)
                        {
                            newDr["物品ID"] = Convert.ToInt32(tempTable.Rows[0]["GoodsID"]);
                            newDr["批次号"] = CE_BatchNoPrefix.WT.ToString() + ServerTime.Time.Year.ToString()
                                + Convert.ToInt32(tempTable.Rows[0]["GoodsID"]).ToString("D6");
                            newDr["数量"] = Convert.ToDecimal(dr["数量"]);
                        }
                        else
                        {
                            newDr["物品ID"] = Convert.ToInt32(dr["物品ID"]);
                            newDr["批次号"] = CE_BatchNoPrefix.WT.ToString() + ServerTime.Time.Year.ToString()
                                + Convert.ToInt32(dr["物品ID"]).ToString("D6");
                            newDr["数量"] = Convert.ToDecimal(dr["数量"]);
                        }
                    }

                    result.Rows.Add(newDr);
                }
            }

            return result;
        }
    }
}
