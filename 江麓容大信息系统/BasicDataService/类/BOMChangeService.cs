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
using Service_Project_Design;
using ServerModule;
using FlowControlService;

namespace Service_Project_Design
{
    class BOMChangeService : IBOMChangeService
    {
        /// <summary>
        /// 删除业务
        /// </summary>
        /// <param name="billNo">业务号</param>
        public void DeleteInfo(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();

            IFlowServer serverFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();
            BillNumberControl billNoControl = new BillNumberControl(CE_BillTypeEnum.BOM变更单.ToString(), this);

            try
            {
                var varData = from a in ctx.Business_Base_BomChange
                              where a.BillNo == billNo
                              select a;

                ctx.Business_Base_BomChange.DeleteAllOnSubmit(varData);
                ctx.SubmitChanges();
                serverFlow.FlowDelete(ctx, billNo);

                ctx.Transaction.Commit();
                billNoControl.CancelBill(billNo);
            }
            catch (Exception ex)
            {
                ctx.Transaction.Rollback();
                throw new Exception(ex.Message);
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
            var varData = from a in ctx.Business_Base_BomChange
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
        /// 检查某业务是否存在
        /// </summary>
        /// <param name="billNo">业务号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Business_Base_BomChange
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
        /// 获得单条业务总单信息
        /// </summary>
        /// <param name="billNo">业务号</param>
        /// <returns>返回业务总单信息</returns>
        public Business_Base_BomChange GetSingleBillInfo(string billNo)
        {
            if (billNo == null)
            {
                return null;
            }

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Business_Base_BomChange
                          where a.BillNo == billNo
                          select a;

            if (varData.Count() == 1)
            {
                return varData.Single();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="billInfo">单据信息</param>
        /// <param name="invoiceInfo">发票信息列表</param>
        /// <param name="detailInfo">明细信息列表</param>
        public void SaveInfo(Business_Base_BomChange billInfo, List<View_Business_Base_BomChange_PartsLibrary> libraryInfo,
            List<View_Business_Base_BomChange_Struct> structInfo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();

            try
            {
                var varData = from a in ctx.Business_Base_BomChange
                              where a.BillNo == billInfo.BillNo
                              select a;

                if (varData.Count() == 1)
                {
                    Business_Base_BomChange lnqBill = varData.Single();

                    lnqBill.FileCode = billInfo.FileCode;
                    lnqBill.FileInfo = billInfo.FileInfo;
                    lnqBill.Reason = billInfo.Reason;
                }
                else if (varData.Count() == 0)
                {
                    ctx.Business_Base_BomChange.InsertOnSubmit(billInfo);
                }
                else
                {
                    throw new Exception("单据数据不唯一");
                }

                var varTemp1 = from a in ctx.Business_Base_BomChange_Struct
                                 where a.BillNo == billInfo.BillNo
                                 select a;

                ctx.Business_Base_BomChange_Struct.DeleteAllOnSubmit(varTemp1);
                ctx.SubmitChanges();

                if (structInfo != null && structInfo.Count > 0)
                {
                    foreach (View_Business_Base_BomChange_Struct item in structInfo)
                    {
                        Business_Base_BomChange_Struct lnqTemp = new Business_Base_BomChange_Struct();

                        lnqTemp.BillNo = billInfo.BillNo;
                        lnqTemp.AssemblingGoodsID = item.父级物品ID;
                        lnqTemp.PartGoodsID = item.物品ID;
                        lnqTemp.Usage = item.基数;

                        ctx.Business_Base_BomChange_Struct.InsertOnSubmit(lnqTemp);
                    }
                }

                var varTemp2 = from a in ctx.Business_Base_BomChange_PartsLibrary
                                where a.BillNo == billInfo.BillNo
                                select a;

                ctx.Business_Base_BomChange_PartsLibrary.DeleteAllOnSubmit(varTemp2);
                ctx.SubmitChanges();

                foreach (View_Business_Base_BomChange_PartsLibrary item in libraryInfo)
                {
                    Business_Base_BomChange_PartsLibrary lnqTemp = new Business_Base_BomChange_PartsLibrary();

                    lnqTemp.BillNo = billInfo.BillNo;
                    lnqTemp.GoodsID = item.物品ID;
                    lnqTemp.Material = item.材质;
                    lnqTemp.OperationType = item.操作类型;
                    lnqTemp.PartType = item.零件类型;
                    lnqTemp.PivotalPart = item.关键件;
                    lnqTemp.Remark = item.备注;
                    lnqTemp.Version = item.版次号;

                    ctx.Business_Base_BomChange_PartsLibrary.InsertOnSubmit(lnqTemp);
                }

                ctx.SubmitChanges();
                ctx.Transaction.Commit();
            }
            catch (Exception ex)
            {
                ctx.Transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获得结构信息
        /// </summary>
        /// <param name="billNo">业务号</param>
        /// <returns>返回业务明细列表</returns>
        public List<View_Business_Base_BomChange_Struct> GetListStructInfo(string billNo)
        {
            if (billNo == null)
            {
                return null;
            }

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.View_Business_Base_BomChange_Struct
                          where a.单据号 == billNo
                          select a;

            return varData.ToList();
        }

        /// <summary>
        /// 获得零件信息
        /// </summary>
        /// <param name="billNo">业务号</param>
        /// <returns>返回业务明细列表</returns>
        public List<View_Business_Base_BomChange_PartsLibrary> GetListLibraryInfo(string billNo)
        {
            if (billNo == null)
            {
                return null;
            }

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.View_Business_Base_BomChange_PartsLibrary
                          where a.单据号 == billNo
                          select a;

            return varData.ToList();
        }

        /// <summary>
        /// 操作流程以外的业务
        /// </summary>
        /// <param name="billNo">单据号</param>
        public void OperatarUnFlowBusiness(string billNo)
        {
            IFlowServer serviceFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();

            string billStatus = serviceFlow.GetNextBillStatus(billNo);

            if (billStatus == null)
            {
                throw new Exception("单据状态为空，请重新确认");
            }

            if (billStatus != CE_CommonBillStatus.单据完成.ToString())
            {
                return;
            }

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            dataContxt.Connection.Open();
            dataContxt.Transaction = dataContxt.Connection.BeginTransaction();

            try
            {
                List<View_Business_Base_BomChange_Struct> lstStruct = GetListStructInfo(billNo);
                List<View_Business_Base_BomChange_PartsLibrary> lstLibrary = GetListLibraryInfo(billNo);
                
                List<Flow_FlowData> tempList = serviceFlow.GetBusinessOperationInfo(billNo, CE_CommonBillStatus.新建单据.ToString());

                if (tempList == null)
                {
                    throw new Exception("获取操作人员失败");
                }

                JudgeAssembly(dataContxt, lstStruct);
                string personnel = tempList[0].OperationPersonnel;

                #region 零件库变更

                foreach (var item2 in lstLibrary)
                {
                    CE_OperatorMode operationMode =
                        GlobalObject.GeneralFunction.StringConvertToEnum<CE_OperatorMode>(item2.操作类型);

                    switch (operationMode)
                    {
                        case CE_OperatorMode.添加:

                            var varTemp4 = from a in dataContxt.BASE_BomPartsLibrary
                                           where a.GoodsID == item2.物品ID
                                           select a;

                            if (varTemp4 != null && varTemp4.Count() == 1)
                            {

                                BASE_BomPartsLibrary library1 = varTemp4.Single();

                                library1.CreateDate = ServerTime.Time;
                                library1.CreatePersonnel = personnel;
                                library1.Material = item2.材质;
                                library1.PartType = item2.零件类型;
                                library1.PivotalPart = item2.关键件;
                                library1.Remark = item2.备注;
                                library1.Version = item2.版次号;
                            }
                            else if (varTemp4.Count() == 0)
                            {
                                BASE_BomPartsLibrary library = new BASE_BomPartsLibrary();

                                library.CreateDate = ServerTime.Time;
                                library.CreatePersonnel = personnel;
                                library.GoodsID = item2.物品ID;
                                library.Material = item2.材质;
                                library.PartType = item2.零件类型;
                                library.PivotalPart = item2.关键件;
                                library.Remark = item2.备注;
                                library.Version = item2.版次号;

                                dataContxt.BASE_BomPartsLibrary.InsertOnSubmit(library);
                            }

                            break;
                        case CE_OperatorMode.修改:

                            var varTemp2 = from a in dataContxt.BASE_BomPartsLibrary
                                           where a.GoodsID == item2.物品ID
                                           select a;

                            if (varTemp2 != null && varTemp2.Count() == 1)
                            {
                                BASE_BomPartsLibrary library1 = varTemp2.Single();

                                library1.CreateDate = ServerTime.Time;
                                library1.CreatePersonnel = personnel;
                                library1.Material = item2.材质;
                                library1.PartType = item2.零件类型;
                                library1.PivotalPart = item2.关键件;
                                library1.Remark = item2.备注;
                                library1.Version = item2.版次号;
                            }

                            break;
                        case CE_OperatorMode.删除:

                            var varTemp3 = from a in dataContxt.BASE_BomPartsLibrary
                                           where a.GoodsID == item2.物品ID
                                           select a;

                            dataContxt.BASE_BomPartsLibrary.DeleteAllOnSubmit(varTemp3);

                            break;
                        default:
                            break;
                    }
                }

                dataContxt.SubmitChanges();

                #endregion

                #region 结构变更
                var varData = (from a in lstStruct
                               select new { a.父级图号, a.父级物品ID }).Distinct();

                foreach (var item in varData)
                {
                    var varTemp = from a in dataContxt.BASE_BomStruct
                                  where a.ParentID == item.父级物品ID
                                  select a;

                    decimal sysVersion = varTemp.Count() == 0 ? 0 : varTemp.Select(r => r.SysVersion).Distinct().ToList()[0];

                    dataContxt.BASE_BomStruct.DeleteAllOnSubmit(varTemp);
                    dataContxt.SubmitChanges();

                    var varTemp1 = from a in lstStruct
                                   where a.父级物品ID == item.父级物品ID
                                   select a;

                    foreach (var item1 in varTemp1)
                    {
                        if (item1.物品ID == null)
                        {
                            break;
                        }

                        BASE_BomStruct tempStruct = new BASE_BomStruct();

                        tempStruct.CreateDate = ServerTime.Time;
                        tempStruct.CreatePersonnel = personnel;
                        tempStruct.GoodsID = (int)item1.物品ID;
                        tempStruct.ParentID = item1.父级物品ID;
                        tempStruct.Usage = (decimal)item1.基数;
                        tempStruct.SysVersion = sysVersion + (decimal)0.01;

                        dataContxt.BASE_BomStruct.InsertOnSubmit(tempStruct);
                    }
                }

                dataContxt.SubmitChanges();
                #endregion

                dataContxt.Transaction.Commit();
            }
            catch (Exception ex)
            {
                dataContxt.Transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }

        void JudgeAssembly(DepotManagementDataContext ctx ,  List<View_Business_Base_BomChange_Struct> lstNew)
        {
            var varData = (from a in lstNew where a.物品ID == null select a.父级物品ID).Distinct().ToList();

            if (varData.Count() > 0)
            {
                foreach (int goodsID in varData)
                {
                    var tempData = from a in ctx.CG_CBOM
                                   join b in ctx.F_GoodsPlanCost
                                   on a.Edition equals b.GoodsCode
                                   where a.Usage > 0 && b.ID == goodsID
                                   select a;

                    if (tempData.Count() > 0)
                    {
                        throw new Exception(UniversalFunction.GetGoodsMessage(ctx, goodsID) 
                            + "清空【设计BOM表结构】，需将【采购BOM】中此总成下的所有零件【基数】设置为0");
                    }


                    var tempData1 = from a in ctx.BASE_ProductOrder
                                   join b in ctx.F_GoodsPlanCost
                                   on a.Edition equals b.GoodsCode
                                   where a.Redices > 0 && b.ID == goodsID
                                   select a;

                    if (tempData1.Count() > 0)
                    {
                        throw new Exception(UniversalFunction.GetGoodsMessage(ctx, goodsID)
                            + "清空【设计BOM表结构】，需将【发料清单】中此总成下的所有零件【基数】设置为0");
                    }
                }
            }
        }

        /// <summary>
        /// 获得总成选择信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetAssemblyInfo()
        {
            string strSql = " select b.图号型号, b.物品名称, b.规格, b.序号 as 物品ID " +
                            " from (select distinct ParentID from BASE_BomStruct) as a "+
                            " inner join View_F_GoodsPlanCost as b on a.ParentID = b.序号";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }
    }
}
