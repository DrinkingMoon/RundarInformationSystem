/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IOrderFormAffrim.cs
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
    /// 网络订单管理类接口
    /// </summary>
    public interface IOrderFormAffrim
    {
        /// <summary>
        /// 添加确认到货日期
        /// </summary>
        /// <param name="webForAffirmTime">网络订单确认信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>添加成功返回True，添加失败返回False</returns>
        bool AddAffrimInfo(B_WebForAffirmTime webForAffirmTime, out string error);

        /// <summary>
        /// 添加网络订单明细
        /// </summary>
        /// <param name="webOrderFormList">网络订单信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>.
        /// <returns>添加成功返回True，添加失败返回False</returns>
        bool AddBill(B_WebForOrderFormList webOrderFormList, out string error);

        /// <summary>
        /// 整张订单删除
        /// </summary>
        /// <param name="yearAndMonth">年月字符串</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        bool DeleteAllInfo(string yearAndMonth, out string error);

        /// <summary>
        /// 删除确认到货日期
        /// </summary>
        /// <param name="id">单据ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        bool DeleteAffrimInfo(int id, out string error);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id">单据ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        bool DeleteBill(int id, out string error);

        /// <summary>
        /// 获得总单的所有信息
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="flag">单据状态</param>
        /// <returns>返回查询到的信息</returns>
        DataTable GetAllInfo(DateTime startTime, DateTime endTime, string flag);

        /// <summary>
        /// 获得某一单据的明细信息
        /// </summary>
        /// <param name="yearAndMonth">年月字符串</param>
        /// <returns>返回查询到的明细信息</returns>
        DataTable GetListInfo(string yearAndMonth);

        /// <summary>
        /// 获得某一单据的明细信息
        /// </summary>
        /// <param name="yearAndMonth">年月字符串 格式为“YYYYMM”</param>
        /// <param name="workName">采购员姓名</param>
        /// <returns>返回查询到的明细信息</returns>
        DataTable GetListInfo(string yearAndMonth, string workName);

        /// <summary>
        /// 修改单据状态
        /// </summary>
        /// <param name="yearAndMonth">操作年月字符串</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>修改成功返回True，修改失败返回False</returns>
        bool UpdateBill(string yearAndMonth, out string error);

        /// <summary>
        /// 修改网络订单明细
        /// </summary>
        /// <param name="webOrderFormList">网络订单明细信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>修改成功返回True，修改失败返回False</returns>
        bool UpdateListInfo(B_WebForOrderFormList webOrderFormList, out string error);

        /// <summary>
        /// 获得到货日期表
        /// </summary>
        /// <param name="listID">明细ID</param>
        /// <returns>返回到货日期列表</returns>
        DataTable GetAffrimInfo(int listID);

        /// <summary>
        /// 检查数据
        /// </summary>
        /// <param name="djID">单据ID</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="provider">供应商编码</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>无记录则返回True，否则返回False</returns>
        bool IsListInfoIn(int djID, int goodsID, string provider, out string error);

        /// <summary>
        /// 获得某一条记录的所有到货数之和
        /// </summary>
        /// <param name="listID">明细ID</param>
        /// <returns>返货明细ID的到货数量之和</returns>
        decimal SumCount(int listID);

        /// <summary>
        /// 改变BILL的单据状态为 等待审核
        /// </summary>
        /// <param name="yearAndMonth">年月字符串</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>改变成功返回True，改变失败返回False</returns>
        bool UpdateBillStatus(string yearAndMonth, out string error);
    }
}
