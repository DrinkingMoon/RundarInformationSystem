/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IMaterialsTransfer.cs
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
using System.Data;
using System.Collections.Generic;
using GlobalObject;

namespace Service_Manufacture_WorkShop
{
    /// <summary>
    /// 车间物料转换单服务接口
    /// </summary>
    public interface IMaterialsTransfer : IBasicBillServer
    {
        /// <summary>
        /// 获得领料单
        /// </summary>
        /// <param name="wsCode">车间代码</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>返回TABLE</returns>
        DataTable GetRequisitionInfo(string wsCode, DateTime startTime, DateTime endTime);

        /// <summary>
        /// 获得物料转换单
        /// </summary>
        /// <param name="wsCode">车间代码</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>返回TABLE</returns>
        DataTable GetMaterialsTransferInfo(string wsCode, DateTime startTime, DateTime endTime);

        /// <summary>
        /// 保存单据
        /// </summary>
        /// <param name="bill">单据信息</param>
        /// <param name="list">明细信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        bool SaveBill(WS_MaterialsTransfer bill, DataTable list, out string error);

        /// <summary>
        /// 检测维修信息
        /// </summary>
        /// <param name="list">检测信息</param>
        void CheckRepair(DataTable list);

        /// <summary>
        /// 组装检测装配BOM
        /// </summary>
        /// <param name="list">匹配信息</param>
        /// <param name="wsCode">车间代码</param>
        /// <param name="convertType">转换类型</param>
        void CheckBom(DataTable list, string wsCode, MaterialsTransferConvertType convertType);

        /// <summary>
        /// 转换后明细的返修匹配
        /// </summary>
        /// <param name="beforeTable">转换前列表</param>
        /// <param name="goodsBefore">转换前GoodsID</param>
        /// <param name="goodsAfter">转换后GoodsID</param>
        /// <param name="billNo">单据号</param>
        /// <returns>返回Table</returns>
        DataTable GetRepairMatch(DataTable beforeTable, int goodsBefore, int goodsAfter, string billNo);

        /// <summary>
        /// 合计TABLE
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="temp">合计源</param>
        /// <param name="subsidiary">业务类型</param>
        /// <returns>返回Table</returns>
        DataTable SumTable(DataTable source, DataTable temp, int subsidiary);

        /// <summary>
        /// 导入电子档案
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="subsidiary">业务模式</param>
        /// <param name="convertType">转换模式</param>
        /// <param name="wsCode">车间代码</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>返回TABLE</returns>
        DataTable GetElectronFileInfo(string billNo, int goodsID,
            CE_SubsidiaryOperationType subsidiary, MaterialsTransferConvertType convertType,
            string wsCode, DateTime startTime, DateTime endTime);

        /// <summary>
        /// 获取列表中的总成或者分总成的GoodsID
        /// </summary>
        /// <param name="list">列表</param>
        /// <returns>返回GoodsID</returns>
        int GetASSYGoodsID(DataTable list);

        /// <summary>
        /// 获取对应单据的成品箱的电子档案的选配件合计
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回Table</returns>
        DataTable GetProductChoseConfect(string billNo);

        /// <summary>
        /// 回退单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="billStatus">回退后的单据状态</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <param name="rebackReason">回退原因</param>
        /// <returns>回退成功返回True，回退失败返回False</returns>
        bool ReturnBill(string billNo, MaterialsTransferStatus billStatus, out string error, string rebackReason);

        /// <summary>
        /// 统计物料转换单的物品
        /// </summary>
        /// <param name="listBillNo">物料转换单单号列表</param>
        /// <param name="operationType">操作类型</param>
        /// <param name="showOperationType">显示操作类型</param>
        /// <param name="wsCode">所属车间</param>
        /// <returns>返回Table</returns>
        DataTable SumMaterialsTransferGoods(List<string> listBillNo, int operationType,
            int showOperationType, string wsCode);

        /// <summary>
        /// 获得物料转换单
        /// </summary>
        /// <param name="wsCode">车间代码</param>
        /// <returns>返回TABLE</returns>
        DataTable GetMaterialsTransferInfo(string wsCode);

        /// <summary>
        /// 获得装配BOM零件
        /// </summary>
        /// <param name="productCode">型号</param>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回Table</returns>
        DataTable GetAssemblingBomInfo(string productCode, int goodsID);

        /// <summary>
        /// 核计领料单的物品
        /// </summary>
        /// <param name="listBillNo">领料单单号列表</param>
        /// <returns>返回Table</returns>
        DataTable SumRequisitionGoods(List<string> listBillNo);

        /// <summary>
        /// 获得领料单
        /// </summary>
        /// <param name="wsCode">车间代码</param>
        /// <returns>返回TABLE</returns>
        DataTable GetRequisitionInfo(string wsCode);

        /// <summary>
        /// 确认单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="list">明细信息</param>
        /// <param name="exception">异常信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        bool AffirmBill(string billNo, System.Data.DataTable list, string exception, out string error);

        /// <summary>
        /// 审核单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        bool AuditBill(string billNo, out string error);

        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        bool DeleteBill(string billNo, out string error);

        /// <summary>
        /// 获得单据信息
        /// </summary>
        /// <returns>返回单据信息</returns>
        System.Data.DataTable GetBillInfo();

        /// <summary>
        /// 获得单条单据信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回单据信息</returns>
        ServerModule.WS_MaterialsTransfer GetBillSingle(string billNo);

        /// <summary>
        /// 获得单据明细
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>成功返回True,失败返回False</returns>
        System.Data.DataTable GetListInfo(string billNo);

        /// <summary>
        /// 申请单据
        /// </summary>
        /// <param name="bill">单据信息</param>
        /// <param name="list">明细信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        bool ProposeBill(WS_MaterialsTransfer bill, DataTable list,  out string error);
    }
}
