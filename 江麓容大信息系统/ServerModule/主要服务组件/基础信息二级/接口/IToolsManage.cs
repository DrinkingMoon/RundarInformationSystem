using System;
using System.Collections.Generic;
using GlobalObject;
using System.Data;

namespace ServerModule
{
    /// <summary>
    /// 工具管理服务接口
    /// </summary>
    public interface IToolsManage
    {
        /// <summary>
        /// 是否为工具
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="goodsID">物品ID</param>
        /// <returns>是 ： True, 否 ：False</returns>
        bool IsTools(DepotManagementDataContext ctx, int goodsID);

        /// <summary>
        /// 获得工具流水账视图信息
        /// </summary>
        /// <returns>返回对象集列表</returns>
        List<View_S_DayToDay_Tools> GetDayToDayViewInfo();

        /// <summary>
        /// 流水账记录
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="dayToDayInfo">流水账对象</param>
        void DayToDayAccount(DepotManagementDataContext ctx, S_DayToDay_Tools dayToDayInfo);

        /// <summary>
        /// 获得视图信息
        /// </summary>
        /// <returns>返回对象集列表</returns>
        System.Collections.Generic.List<ServerModule.View_S_MachineAccount_Tools> GetMachineAccountViewInfo();

        /// <summary>
        /// 是否为工具
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <returns>是 ： True, 否 ：False</returns>
        bool IsTools(int goodsID);

        /// <summary>
        /// 操作工具台帐
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="toolsInfo">工具台帐对象</param>
        void OpertionInfo(ServerModule.DepotManagementDataContext ctx, ServerModule.S_MachineAccount_Tools toolsInfo);

        /// <summary>
        /// 工具流水账记录
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="toolsInfo">工具台帐对象</param>
        /// <param name="billNo">单据号</param>
        void DayToDayAccount(DepotManagementDataContext ctx, S_MachineAccount_Tools toolsInfo, string billNo);
    }
}
