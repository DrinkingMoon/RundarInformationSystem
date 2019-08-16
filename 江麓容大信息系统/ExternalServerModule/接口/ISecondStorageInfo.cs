/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  ISecondStorageInfo.cs
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

namespace Service_Peripheral_External
{
    public interface ISecondStorageInfo
    {
        
        /// <summary>
        /// 修改库房信息
        /// </summary>
        /// <param name="oldStorageID">库房旧编码</param>
        /// <param name="outStockInfo">库房数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool ModifyInfo(string oldStorageID, Out_StockInfo outStockInfo, out string error);

        /// <summary>
        /// 插入二级库房信息
        /// </summary>
        /// <param name="outStockInfo">数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool InsertInfo(Out_StockInfo outStockInfo, out string error);
        
        /// <summary>
        /// 删除库房信息
        /// </summary>
        /// <param name="secStorageID">库房编码</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool DeleteInfo(string secStorageID, out string error);

        /// <summary>
        /// 获得二级库房信息
        /// </summary>
        /// <returns>返回TABLE</returns>
        System.Data.DataTable GetSecondStorageInfo();
    }
}
