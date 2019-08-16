/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  ICannibalize.cs
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;


namespace ServerModule
{
    /// <summary>
    /// 库房调拨类的接口
    /// </summary>
    public interface ICannibalize:IBasicBillServer
    {

        /// <summary>
        /// 保存单据明细信息
        /// </summary>
        /// <param name="dataContxt">LINQ数据上下文</param>
        /// <param name="lstDetail">添加的表</param>
        /// <param name="djID">添加的行</param>
        /// <param name="billNo">单据号</param>
        void SaveBillList(DepotManagementDataContext dataContxt, List<S_CannibalizeList> lstDetail, int djID, string billNo);

        /// <summary>
        /// 变更单据状态（检测）
        /// </summary>
        /// <param name="djID">单据ID</param>
        /// <param name="remark">备注</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>变更成功返回True，变更失败返回False</returns>
        bool QualityBill(int djID, string remark, out string error);

        /// <summary>
        /// 获取指定日期范围内的全部信息
        /// </summary>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="djzt">单据状态</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回Table</returns>
        DataTable GetAllData(string startDate, string endDate,
                string djzt, out string error);

        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="billID">单据ID</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        void DeleteBill(int billID);

        /// <summary>
        /// 编辑检验状态
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <param name="remark">备注</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool CheckBill(string djh, string remark,out string error);

        /// <summary>
        /// 变更单据状态（审核）
        /// </summary>
        /// <param name="djID">单据序号</param>
        /// <param name="remark">备注</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool AuditingBill(int djID, string remark, out string error);

        /// <summary>
        /// 获得单据明细
        /// </summary>
        /// <param name="djID">单据序号</param>
        /// <returns>返回Table</returns>
        DataTable GetList(int djID);

        /// <summary>
        /// 获得入库单据
        /// </summary>
        /// <param name="djID">单据序号</param>
        /// <returns>返回Table</returns>
        S_CannibalizeBill GetBill(int djID);

        /// <summary>
        /// 保存单据数据(如果单据信息ID为0则添加数据，否则更新数据)
        /// </summary>
        /// <param name="billList">单据明细</param>
        /// <param name="billInfo">单据信息</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        void SaveBill(DataTable billList, S_CannibalizeBill billInfo);

        /// <summary>
        /// 仓管确认
        /// </summary>
        /// <param name="djID">单据序号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool AffirmBill(int djID,out string error);
    }
}
