using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerModule
{
    /// <summary>
    /// 用于分配单据号的服务接口
    /// </summary>
    public interface IAssignBillNoServer
    {
        /// <summary>
        /// 取消分配的单据号
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="billNo">单据号</param>
        /// <returns>成功则返回true, 失败返回false或抛出异常</returns>
        bool CancelBillNo(DepotManagementDataContext context, string billNo);

        /// <summary>
        /// 为指定类别的单据分配新号
        /// </summary>
        /// <param name="dataContxt">数据上下文</param>
        /// <param name="billServer">单据服务</param>
        /// <param name="billTypeName">单据类别名称, 如：领料单</param>
        /// <returns>返回获取到的新单据号</returns>
        string AssignNewNo(DepotManagementDataContext dataContxt, IBasicBillServer billServer, string billTypeName);

        /// <summary>
        /// 为指定类别的单据分配新号
        /// </summary>
        /// <param name="billServer">单据服务</param>
        /// <param name="billTypeName">单据类别名称, 如：领料单</param>
        /// <returns>返回获取到的新单据号</returns>
        string AssignNewNo(IBasicBillServer billServer, string billTypeName);
        
        /// <summary>
        /// 取消分配的单据号
        /// </summary>
        /// <param name="billTypeName">单据类型名称</param>
        /// <param name="billNo">单据号</param>
        /// <returns>成功则返回true, 失败返回false或抛出异常</returns>
        bool CancelBillNo(string billTypeName, string billNo);

        /// <summary>
        /// 取消分配的单据号
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="billTypeName">单据类型名称</param>
        /// <param name="billNo">单据号</param>
        /// <returns>成功则返回true, 失败返回false或抛出异常</returns>
        bool CancelBillNo(DepotManagementDataContext context, string billTypeName, string billNo);

        /// <summary>
        /// 使用单据号, 一旦使用则无法再取消（废弃）(该单据已经完成时调用此方法)
        /// </summary>
        /// <param name="billTypeName">单据类型名称</param>
        /// <param name="billNo">单据号</param>
        /// <returns>成功则返回true, 失败返回false或抛出异常</returns>
        bool UseBillNo(string billTypeName, string billNo);

        /// <summary>
        /// 使用单据号, 一旦使用则无法再取消（废弃）(该单据已经完成时调用此方法)
        /// </summary>
        /// <param name="dc">数据上下文</param>
        /// <param name="billTypeName">单据类型名称</param>
        /// <param name="billNo">单据号</param>
        /// <returns>成功则返回true, 失败返回false或抛出异常</returns>
        bool UseBillNo(DepotManagementDataContext dc, string billTypeName, string billNo);

        /// <summary>
        /// 获得订单号
        /// </summary>
        /// <param name="strType">订单类型, 用于区分是零星还是其他订单</param>
        /// <returns>返回获得的订单号</returns>
        string GetOrderFormNumber(string strType);

        /// <summary>
        /// 获得合同号
        /// </summary>
        /// <param name="strType">合同类型, 用于区分是哪个部门的合同</param>
        /// <returns>获取生成的合同号</returns>
        string GetBargainNumber(string strType);
    }
}
