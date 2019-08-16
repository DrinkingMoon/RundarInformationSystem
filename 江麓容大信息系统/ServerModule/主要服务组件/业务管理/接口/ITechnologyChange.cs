/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  ITechnologyChange.cs
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
    /// 技术变更单管理类接口
    /// </summary>
    public interface ITechnologyChange:IBasicBillServer
    {

        /// <summary>
        /// 获得单条记录
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <returns>返回获得的单据信息</returns>
        DataRow GetBill(string djh);

        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        bool DeleteBill(string djh, out string error);

        /// <summary>
        /// 获得全部单据
        /// </summary>
        /// <returns>返回获得的全部单据信息</returns>
        DataTable GetAllBill();

        /// <summary>
        /// 提交单据
        /// </summary>
        /// <param name="inBill">变更单信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool SubmitBill(S_TechnologyChangeBill inBill, out string error);

        /// <summary>
        /// 获得同名称同型号同规格在BOM表中的记录
        /// </summary>
        /// <param name="code">图号型号</param>
        /// <param name="name">物品名称</param>
        /// <param name="spec">规格</param>
        /// <returns>返回获取的BOM表信息</returns>
        DataTable GetBomInfo(string code, string name, string spec);

        /// <summary>
        /// 更改单据状态
        /// </summary>
        /// <param name="inBill">变更单信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>更改成功返回True，更改失败返回False</returns>
        bool UpdateBill(S_TechnologyChangeBill inBill, out string error);

        /// <summary>
        /// 获得BOM表信息
        /// </summary>
        /// <param name="code">图号型号</param>
        /// <param name="name">物品名称</param>
        /// <param name="spec">规格</param>
        /// <param name="edition">型号</param>
        /// <returns>返回DataRow</returns>
        DataRow GetProductInfo(string code, string name, string spec, string edition);
    }
}
