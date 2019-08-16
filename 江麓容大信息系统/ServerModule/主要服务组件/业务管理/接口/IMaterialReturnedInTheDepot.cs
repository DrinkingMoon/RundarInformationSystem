/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IMaterialReturnedInTheDepot.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2010/07/17
 * 开发平台:  Visual C# 2008
 * 用于    :  仓库管理软件
 *----------------------------------------------------------------------------
 * 描述 : 
 * 其它 :
 *----------------------------------------------------------------------------
 * 公共信息: 参见系统'类帮助文档'
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2010/07/17 8:54:12 作者: 夏石友 当前版本: V1.00
 *        修改说明: 创建
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using PlatformManagement;

namespace ServerModule
{
    /// <summary>
    /// 领料退库单单据状态
    /// </summary>
    public enum MaterialReturnedInTheDepotBillStatus
    {
        /// <summary>
        /// 新建单据
        /// </summary>
        新建单据,

        /// <summary>
        /// 等待主管审核
        /// </summary>
        等待主管审核,

        /// <summary>
        /// 等待质检批准
        /// </summary>
        等待质检批准,

        /// <summary>
        /// 等待仓管退库
        /// </summary>
        等待仓管退库,

        /// <summary>
        /// 已完成
        /// </summary>
        已完成
    }

    /// <summary>
    /// 领料退库单管理类接口
    /// </summary>
    public interface IMaterialReturnedInTheDepot : IBasicService, IBasicBillServer
    {

        /// <summary>
        /// 根据单据信息操作账务信息与库存信息
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="bill">单据信息</param>
        void OpertaionDetailAndStock(DepotManagementDataContext dataContext, S_MaterialReturnedInTheDepot bill);

        /// <summary>
        /// 获取所有领料退库单信息
        /// </summary>
        /// <param name="returnBill">入库单</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取领料出库信息</returns>
        bool GetAllBill(out IQueryResult returnBill, out string error);

        /// <summary>
        /// 获取领料退库单信息
        /// </summary>
        /// <param name="billNo">领料退库单号</param>
        /// <returns>成功返回获取领料出库信息, 失败返回null</returns>
        S_MaterialReturnedInTheDepot GetBill(string billNo);

        /// <summary>
        /// 获取领料退库单视图信息
        /// </summary>
        /// <param name="billNo">领料退库单号</param>
        /// <returns>成功返回获取领料出库信息, 失败返回null</returns>
        View_S_MaterialReturnedInTheDepot GetBillView(string billNo);
        
        /// <summary>
        /// 为领料而获取所有已经完成的单据信息
        /// </summary>
        /// <param name="returnBill">查询到的单据信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功的标志</returns>
        bool GetAllBillForFetchGoods(out IQueryResult returnBill, out string error);

        /// <summary>
        /// 添加领料退库单
        /// </summary>
        /// <param name="bill">单据信息</param>
        /// <param name="returnBill">返回更新后的领料退库单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加领料退库单</returns>
        bool AddBill(S_MaterialReturnedInTheDepot bill, out IQueryResult returnBill, out string error);
 
        /// <summary>
        /// 删除领料退库单
        /// </summary>
        /// <param name="billNo">领料退库单号</param>
        /// <param name="returnBill">返回更新后重新查询的领料退库单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功删除领料退库单号</returns>
        bool DeleteBill(string billNo, out IQueryResult returnBill, out string error);

        /// <summary>
        /// 修改领料退库单(只修改编制人涉及信息)
        /// </summary>
        /// <param name="bill">单据信息</param>
        /// <param name="returnBill">返回更新后的领料退库单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加领料退库单</returns>
        bool UpdateBill(S_MaterialReturnedInTheDepot bill, out IQueryResult returnBill, out string error);

        /// <summary>
        /// 领料出库人提交单据(交给主管审批)
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="returnBill">返回更新后重新查询的领料退库单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加领料退库单</returns>
        bool SubmitNewBill(string billNo, out IQueryResult returnBill, out string error);
        
        /// <summary>
        /// 主管审批单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="name">主管姓名</param>
        /// <param name="returnBill">返回更新后重新查询的领料退库单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加领料退库单</returns>
        bool DirectorAuthorizeBill(string billNo, string name, out IQueryResult returnBill, out string error);

        /// <summary>
        /// 质量批准单据
        /// </summary>
        /// <param name="billID">单据编号</param>
        /// <param name="name">质量签名</param>
        /// <param name="returnBill">添加完毕后查询数据库的返回结果</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作结果</returns>
        bool QualityAuthorizeBill(string billID, string name, out IQueryResult returnBill, out string error);

        /// <summary>
        /// 完成领料退库单
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="storeManager">仓库管理员</param>
        /// <param name="returnBill">返回更新后重新查询的领料退库单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加领料退库单</returns>
        bool FinishBill(string billNo, string storeManager, out IQueryResult returnBill, out string error);

        /// <summary>
        /// 回退单据
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <param name="billStatus">单据状态</param>
        /// <param name="returnBill">返回回退更新后查询的数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <param name="rebackReason">回退原因</param>
        /// <returns>回退成功返回True，回退失败返回False</returns>
        bool ReturnBill(string djh, string billStatus,
            out IQueryResult returnBill, out string error, string rebackReason);
    }
}
