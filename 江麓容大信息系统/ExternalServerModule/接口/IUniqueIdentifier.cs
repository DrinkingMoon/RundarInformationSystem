/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IUniqueIdentifier.cs
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

namespace Service_Peripheral_External
{
    public interface IUniqueIdentifier
    {
        /// <summary>
        /// 删除标识码
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="uniqueIdentifier">标识码数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool DeleteIdentifier(ServerModule.DepotManagementDataContext dataContext,
            ServerModule.Out_UniqueIdentifierData uniqueIdentifier, out string error);

        /// <summary>
        /// 获得标识码数据
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="storageID">账务库房ID</param>
        /// <returns>返回Table</returns>
        System.Data.DataTable GetInfo(string billNo, int goodsID, string storageID);

        /// <summary>
        /// 提交信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="storageID">账务库房ID</param>
        /// <param name="uniqueIdentifier">标识表</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool SubmitInfo(string billNo, int goodsID, string storageID, System.Data.DataTable uniqueIdentifier, out string error);
    }
}
