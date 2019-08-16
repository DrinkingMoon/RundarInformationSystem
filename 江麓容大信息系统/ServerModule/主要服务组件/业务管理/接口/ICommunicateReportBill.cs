/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  ICommunicateReportBill.cs
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
using System.Data;

namespace ServerModule
{
    /// <summary>
    /// 主机厂报表管理类接口
    /// </summary>
    public interface ICommunicateReportBill
    {
        /// <summary>
        /// 根据主机厂的图号名称获得系统的物品ID
        /// </summary>
        /// <param name="communicateGoodsCode">主机厂图号</param>
        /// <param name="communicateGoodsName">主机厂名称</param>
        /// <param name="communicateCode">主机厂名称</param>
        /// <returns>返回0 为空 否则返回GOODSID</returns>
        int GetGoodsID(string communicateGoodsCode,
            string communicateGoodsName, string communicateCode);

        /// <summary>
        /// 获得主机厂与系统零件的匹配信息
        /// </summary>
        /// <returns>返回获取的数据</returns>
        DataTable GetMatchingTable();

        /// <summary>
        /// 对于零件匹配表的数据库操作
        /// </summary>
        /// <param name="operationType">操作类型  (添加,删除)</param>
        /// <param name="goodsMathcning">Linq系统与供应商物品匹配的数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool UpdateMathchingTable(string operationType,
            YX_GoodsSystemMatchingCommunicate goodsMathcning, out string error);

        /// <summary>
        /// 查询挂账汇总表
        /// </summary>
        /// <param name="yearAndMonth">查询年月 格式为“YYYYMM”</param>
        /// <param name="communicate">主机厂编码</param>
        /// <returns>返回查询到的挂账汇总表</returns>
        DataTable GetSignTheBill(string yearAndMonth, string communicate);

        /// <summary>
        /// 由EXCEL表导入且对挂账表填入实挂数量
        /// </summary>
        /// <param name="communicate">主机厂编码</param>
        /// <param name="excle">EXCLE表信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>导入成功返回True，导入失败返回False</returns>
        bool UpdateSignTheBill(string communicate, DataTable excle, out string error);

        /// <summary>
        /// 确认挂账
        /// </summary>
        /// <param name="yearAndMonth">年月 格式为“YYYYMM”</param>
        /// <param name="communicate">主机厂编码</param>
        /// <returns>返回确认挂账后的挂账表</returns>
        DataTable SetSingTheBillPrice(string yearAndMonth, string communicate);

        /// <summary>
        /// 查询回款汇总表
        /// </summary>
        /// <param name="startNy">开始年月 格式为“YYYYMM”</param>
        /// <param name="endNy">结束年月 格式为“YYYYMM”</param>
        /// <param name="communicate">主机厂编码</param>
        /// <returns>返回查询的回款汇总表</returns>
        DataTable GetCommunicateReturnedMoneyBill(string startNy, string endNy, string communicate);

        /// <summary>
        /// 设置回款金额
        /// </summary>
        /// <param name="yearAndMonth">年月  格式为“YYYYMM”</param>
        /// <param name="communicate">主机厂编码</param>
        /// <param name="returnMoney">回款金额</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>设置成功返回True，设置失败返回False</returns>
        bool SetReturnedMoney(string yearAndMonth, string communicate, decimal returnMoney, out string error);
    }
}
