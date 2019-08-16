using System;

namespace ServerModule
{
    /// <summary>
    /// 技术变更单管理类
    /// </summary>
    public interface ITechnologyAlteration : IBasicBillServer
    {
        /// <summary>
        /// 检查某单据是否存在
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>存在返回true</returns>
        new bool IsExist(string billNo);

        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        bool DeleteBill(string djh, out string error);

        /// <summary>
        /// 通过单号获得明细
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <returns>返回获得的单据信息</returns>
        System.Data.DataTable GetListInfo(string djh);

        /// <summary>
        /// 获得全部单据
        /// </summary>
        /// <param name="billStatus">单据状态</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>返回获得的全部单据信息</returns>
        System.Data.DataTable GetAllBill(string billStatus, DateTime startTime, DateTime endTime);

        /// <summary>
        /// 获得单条记录
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <returns>返回获得的单据信息</returns>
        System.Data.DataRow GetBillInfo(string djh);

        /// <summary>
        /// 获得同名称同型号同规格在BOM表中的记录
        /// </summary>
        /// <param name="code">图号型号</param>
        /// <param name="name">物品名称</param>
        /// <param name="spec">规格</param>
        /// <returns>返回获取的BOM表信息</returns>
        System.Data.DataTable GetBomInfo(string code, string name, string spec);

        /// <summary>
        /// 获得BOM表信息
        /// </summary>
        /// <param name="code">图号型号</param>
        /// <param name="name">物品名称</param>
        /// <param name="spec">规格</param>
        /// <param name="edition">型号</param>
        /// <returns>返回DataRow</returns>
        System.Data.DataRow GetProductInfo(string code, string name, string spec, string edition);

        /// <summary>
        /// 提交申请
        /// </summary>
        /// <param name="technology">单据信息数据集</param>
        /// <param name="listInfo">明细信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool SubmitBill(S_TechnologyAlterationBill technology, System.Data.DataTable listInfo, out string error);

        /// <summary>
        /// 更改单据状态
        /// </summary>
        /// <param name="technology">变更单信息</param>
        /// <param name="listInfo">零件信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>更改成功返回True，更改失败返回False</returns>
        bool UpdateBill(S_TechnologyAlterationBill technology, System.Data.DataTable listInfo, out string error);

        /// <summary>
        /// 回退单据
        /// </summary>
        /// <param name="strDJH">单据号</param>
        /// <param name="strBillStatus">单据状态</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <param name="strRebackReason">回退原因</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool ReturnBill(string strDJH, string strBillStatus, out string error, string strRebackReason);
    }
}
