using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GlobalObject;
using System.Data;

namespace ServerModule
{
    /// <summary>
    /// 工具管理服务
    /// </summary>
    class ToolsManage : IToolsManage
    {
        /// <summary>
        /// 是否为工具
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="goodsID">物品ID</param>
        /// <returns>是 ： True, 否 ：False</returns>
        public bool IsTools(DepotManagementDataContext ctx, int goodsID)
        {
            object keyValue = BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.工具类别编码];

            if (keyValue != null && keyValue.ToString().Trim().Length > 0)
            {
                var varData = from a in ctx.F_GoodsPlanCost
                              where a.ID == goodsID
                              select a;

                if (varData.Count() == 1)
                {
                    if (varData.Single().GoodsType == keyValue.ToString())
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 是否为工具
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <returns>是 ： True, 否 ：False</returns>
        public bool IsTools(int goodsID)
        {
            object keyValue = BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.工具类别编码];

            if (keyValue != null && keyValue.ToString().Trim().Length > 0)
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.F_GoodsPlanCost
                              where a.ID == goodsID
                              select a;

                if (varData.Count() == 1)
                {
                    if (varData.Single().GoodsType == keyValue.ToString())
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 获得工具账存视图信息
        /// </summary>
        /// <returns>返回对象集列表</returns>
        public List<View_S_MachineAccount_Tools> GetMachineAccountViewInfo()
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            return (from a in ctx.View_S_MachineAccount_Tools select a).ToList();
        }

        /// <summary>
        /// 获得工具流水账视图信息
        /// </summary>
        /// <returns>返回对象集列表</returns>
        public List<View_S_DayToDay_Tools> GetDayToDayViewInfo()
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            return (from a in ctx.View_S_DayToDay_Tools select a).ToList();
        }

        /// <summary>
        /// 操作工具台帐
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="toolsInfo">工具台帐对象</param>
        public void OpertionInfo(DepotManagementDataContext ctx, S_MachineAccount_Tools toolsInfo)
        {
            var varData = from a in ctx.S_MachineAccount_Tools
                          where a.GoodsID == toolsInfo.GoodsID
                          && a.Provider == toolsInfo.Provider
                          && a.StorageCode == toolsInfo.StorageCode
                          select a;

            S_MachineAccount_Tools tempInfo = new S_MachineAccount_Tools();

            if (varData.Count() == 1)
            {
                tempInfo = varData.Single();
                tempInfo.StockCount = tempInfo.StockCount + toolsInfo.StockCount;
            }
            else if (varData.Count() > 1)
            {
                throw new Exception(UniversalFunction.GetGoodsMessage(toolsInfo.GoodsID) + "供应商：" + toolsInfo.Provider 
                    + "所属部门编码： " + toolsInfo.StorageCode + "在工具台帐记录数不唯一");
            }
            else if (varData.Count() == 0)
            {
                tempInfo.GoodsID = toolsInfo.GoodsID;
                tempInfo.Provider = toolsInfo.Provider;
                tempInfo.StockCount = toolsInfo.StockCount;
                tempInfo.StorageCode = toolsInfo.StorageCode;

                ctx.S_MachineAccount_Tools.InsertOnSubmit(tempInfo);
            }

            if (tempInfo.StockCount < 0)
            {
                throw new Exception("工具台帐数量不能小于0");
            }
        }

        /// <summary>
        /// 流水账记录
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="dayToDayInfo">流水账对象</param>
        public void DayToDayAccount(DepotManagementDataContext ctx, S_DayToDay_Tools dayToDayInfo)
        {
            var varData = from a in ctx.S_DayToDay_Tools
                          where a.BillNo == dayToDayInfo.BillNo
                          && a.GoodsID == dayToDayInfo.GoodsID
                          && a.Provider == dayToDayInfo.Provider
                          && a.StorageCode == dayToDayInfo.StorageCode
                          select a;

            if (varData.Count() == 0)
            {
                dayToDayInfo.OperationTime = ServerTime.Time;
                ctx.S_DayToDay_Tools.InsertOnSubmit(dayToDayInfo);
            }
            else
            {
                throw new Exception("流水账记录不唯一");
            }
        }

        /// <summary>
        /// 工具流水账记录
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="toolsInfo">工具台帐对象</param>
        /// <param name="billNo">单据号</param>
        public void DayToDayAccount(DepotManagementDataContext ctx, S_MachineAccount_Tools toolsInfo, string billNo)
        {
            S_DayToDay_Tools dayToDayInfo = new S_DayToDay_Tools();

            dayToDayInfo.BillNo = billNo;
            dayToDayInfo.GoodsID = toolsInfo.GoodsID;
            dayToDayInfo.OpeartionCount = toolsInfo.StockCount;
            dayToDayInfo.OperationTime = ServerTime.Time;
            dayToDayInfo.Provider = toolsInfo.Provider;
            dayToDayInfo.StorageCode = toolsInfo.StorageCode;

            DayToDayAccount(ctx, dayToDayInfo);
        }
    }
}
