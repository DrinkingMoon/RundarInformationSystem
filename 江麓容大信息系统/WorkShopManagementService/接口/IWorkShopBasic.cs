/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IWorkShopBasic.cs
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
using ServerModule;

namespace Service_Manufacture_WorkShop
{
    /// <summary>
    /// 车间基础信息服务接口
    /// </summary>
    public interface IWorkShopBasic
    {
        /// <summary>
        /// 由权限获得所管辖的车间代码
        /// </summary>
        /// <returns>返回车间代码列表</returns>
        System.Collections.Generic.List<string> GetWorkShopCodeRole();
        

        /// <summary>
        /// 获得人员对应的车间信息
        /// </summary>
        /// <param name="info">人员工号或者人员名称</param>
        /// <returns>返回数据集</returns>
        WS_WorkShopCode GetPersonnelWorkShop(DepotManagementDataContext ctx, string info);

        /// <summary>
        /// 获得人员对应的车间信息
        /// </summary>
        /// <param name="info">人员工号或者人员名称</param>
        /// <returns>返回数据集</returns>
        WS_WorkShopCode GetPersonnelWorkShop(string info);

        /// <summary>
        /// 获得单条车间信息
        /// </summary>
        /// <param name="msg">车间编码或者车间名称</param>
        /// <returns>返回结果集</returns>
        WS_WorkShopCode GetWorkShopCodeInfo(string msg);

        /// <summary>
        /// 获得车间信息
        /// </summary>
        /// <returns></returns>
        System.Data.DataTable GetWorkShopBasicInfo();

        /// <summary>
        /// 根据用途编码获取用途名称
        /// </summary>
        /// <param name="code">编码</param>
        /// <returns>返回结果集</returns>
        WS_ConsumptionPurpose GetConsumptionPurpose(string code);
    }
}
