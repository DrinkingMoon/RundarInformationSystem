/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IHomemadePartInDepotBillServer.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2010/09/17
 * 开发平台:  Visual C# 2008
 * 用于    :  仓库管理软件
 *----------------------------------------------------------------------------
 * 描述 : 
 * 其它 :
 *----------------------------------------------------------------------------
 * 公共信息: 参见系统'类帮助文档'
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2010/09/17 8:54:12 作者: 夏石友 当前版本: V1.00
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
    /// 自制件入库单单据状态
    /// </summary>
    public enum HomemadeBillStatus
    {
        /// <summary>
        /// 等待质检
        /// </summary>
        等待质检,

        /// <summary>
        /// 等待入库
        /// </summary>
        等待入库,


        /// <summary>
        /// 已入库
        /// </summary>
        已入库,

        /// <summary>
        /// 撤消
        /// </summary>
        撤消,

        /// <summary>
        /// 回退_编制单据有误
        /// </summary>
        回退_编制单据有误,

        /// <summary>
        /// 回退_确认到货有误
        /// </summary>
        回退_确认到货有误,


        /// <summary>
        /// 回退_质检信息有误
        /// </summary>
        回退_质检信息有误
    }

    /// <summary>
    /// 自制件入库单管理类接口
    /// </summary>
    public interface IHomemadePartInDepotServer : IBasicService, IBasicBillServer
    {
        /// <summary>
        /// 有检测废的物品直接生成领料单
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="inDepotInfo">自制件入库单信息</param>
        /// <param name="mrBillNo">分配的领料单单号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool InsertIntoMaterialRequisition(DepotManagementDataContext ctx, S_HomemadePartBill inDepotInfo,
            out string mrBillNo, out string error);

        /// <summary>
        /// 根据单据信息操作账务信息与库存信息
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="bill">单据信息</param>
        void OpertaionDetailAndStock(DepotManagementDataContext dataContext, S_HomemadePartBill bill);

        /// <summary>
        /// 获得单条记录信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回LINQ单条信息</returns>
        S_HomemadePartBill GetBill(string billNo);

        /// <summary>
        /// 获取自制件入库单信息
        /// </summary>
        /// <param name="returnBill">返回的入库单信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取库存信息</returns>
        bool GetAllBill(out IQueryResult returnBill, out string error);

        /// <summary>
        /// 检查自制件入库单中是否存在此物品相关信息
        /// </summary>
        /// <param name="id">物品ID</param>
        /// <returns>存在返回true, 不存在返回false</returns>
        bool IsExist(int id);

        /// <summary>
        /// 添加自制件入库单
        /// </summary>
        /// <param name="bill">自制件单据信息</param>
        /// <param name="returnBill">添加完毕后查询数据库的返回结果</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool AddBill(S_HomemadePartBill bill, out IQueryResult returnBill, out string error);

        /// <summary>
        /// 更新自制件入库单
        /// </summary>
        /// <param name="updateBill">更新的自制件单据信息</param>
        /// <param name="returnBill">返回的单据查询结果集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool UpdateBill(S_HomemadePartBill updateBill, out IQueryResult returnBill, out string error);

        /// <summary>
        /// 确认到货数
        /// </summary>
        /// <param name="billID">单据编号</param>
        /// <param name="confirmAmountSignatory">仓库收货员签名</param>
        /// <param name="goodsAmount">货物数量</param>
        /// <param name="billStatusInfo">单据状态消息</param>
        /// <param name="returnBill">添加完毕后查询数据库的返回结果</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作结果</returns>
        bool AffirmGoodsAmount(string billID, string confirmAmountSignatory, int goodsAmount, 
            string billStatusInfo, out IQueryResult returnBill, out string error);

        /// <summary>
        /// 提交质量信息
        /// </summary>
        /// <param name="billID">单据编号</param>
        /// <param name="qualityInfo">质量信息, 只取其中质量部分</param>
        /// <param name="returnBill">添加完毕后查询数据库的返回结果</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作结果</returns>
        bool SubmitQualityInfo(string billID, S_HomemadePartBill qualityInfo, out IQueryResult returnBill, out string error);

        /// <summary>
        /// 提交入库信息
        /// </summary>
        /// <param name="billID">单据编号</param>
        /// <param name="inDepotInfo">入库信息, 只取其中入库部分</param>
        /// <param name="returnBill">添加完毕后查询数据库的返回结果</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool SubmitInDepotInfo(string billID, S_HomemadePartBill inDepotInfo, out IQueryResult returnBill, out string error);

        /// <summary>
        /// 删除自制件入库单
        /// </summary>
        /// <param name="billNo">入库单号</param>
        /// <param name="returnBill">自制件入库单</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功删除自制件入库单号</returns>
        bool DeleteBill(string billNo, out IQueryResult returnBill, out string error);

        /// <summary>
        /// 逐级回退单据
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="rebackReason">回退原因</param>
        /// <param name="returnBill">单据查询结果</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool RebackBill(string billID, string rebackReason, out IQueryResult returnBill, out string error);

        /// <summary>
        /// 报废单据
        /// </summary>
        /// <param name="billID">要报废的单据号</param>
        /// <param name="returnBill">单据查询结果</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool ScrapBill(string billID, out IQueryResult returnBill, out string error);
    }
}
