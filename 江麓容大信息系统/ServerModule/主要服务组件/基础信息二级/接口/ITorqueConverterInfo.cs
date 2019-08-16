/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  ITorqueConverterInfo.cs
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

namespace ServerModule
{
    /// <summary>
    /// 液力变矩器标识码信息操作接口
    /// </summary>
    public interface ITorqueConverterInfoServer
    {
        /// <summary>
        /// 删除指定单据的变矩器标识信息（当单据报废时进行此操作）
        /// </summary>
        /// <param name="billNo">单据编号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool DeleteInfo(string billNo, out string error);

        /// <summary>
        /// 导入厂家提供的液力变矩器数据
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <param name="billNo">报检单单据号</param>
        /// <param name="batchNo">批次号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool ImportInfo(System.Data.DataTable dt, string billNo, string batchNo, out string error);
    }
}
