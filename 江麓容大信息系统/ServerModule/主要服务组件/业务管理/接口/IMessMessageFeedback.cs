/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IMessMessageFeedback.cs
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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using PlatformManagement;
using GlobalObject;

namespace ServerModule
{
    /// <summary>
    /// 质量信息反馈单管理类接口
    /// </summary>
    public interface IMessMessageFeedback
    {
        /// <summary>
        /// 获得全部单据
        /// </summary>
        /// <param name="billStatus">单据状态,若为“全  部”则显示所有单据信息</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>返回获得的单据信息</returns>
        DataTable GetAllData(string billStatus, DateTime startTime, DateTime endTime);

        /// <summary>
        /// 获得入库单号
        /// </summary>
        /// <returns>返回单据号</returns>
        string GetBillID();

        /// <summary>
        /// 插入新数据
        /// </summary>
        /// <param name="inMess">反馈单信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>插入成功返回True，插入失败返回False</returns>
        bool AddData(S_MessMessageFeedback inMess, out string error);

        /// <summary>
        /// 更新数据库
        /// </summary>
        /// <param name="inMessMessage">反馈单信息</param>
        /// <param name="flag">更新状态</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>更新成功返回True，更新失败返回False</returns>
        bool UpdateData(S_MessMessageFeedback inMessMessage, string flag,
            out string error);

        /// <summary>
        /// 获得单条记录信息
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <returns>返回获取到的单据记录信息</returns>
        S_MessMessageFeedback GetData(string djh);

        /// <summary>
        /// 获得报检单中的信息
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <returns>返回获得报检单的信息</returns>
        View_S_CheckOutInDepotBill GetCheckInDepotBill(string djh);

        /// <summary>
        /// 回退单据
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <param name="flag">回退状态</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <param name="rebackReason">回退原因</param>
        /// <returns>回退成功返回True，回退失败返回False</returns>
        bool ReturnData(string djh, string flag, out string error, string rebackReason);

        /// <summary>
        /// 获得最近的单据的联系人信息
        /// </summary>
        /// <param name="providerName">供应商名称</param>
        /// <returns>返回最近单据的联系人信息</returns>
        DataTable GetNearestLinkManInfo(string providerName);

        /// <summary>
        /// 获得本批总数
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <param name="storageID">库房ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回本批总数信息</returns>
        decimal GetAllCount(int goodsID, string batchNo, string storageID,
            out string error);

        /// <summary>
        /// 插入记录
        /// </summary>
        /// <param name="inMessMessage">反馈单信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>插入成功返回True，插入失败返回False</returns>
        bool InsertData(S_MessMessageFeedback inMessMessage, out string error);
        
        /// <summary>
        /// 报废单据
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>报废单据成功返回True，报废单据失败返回False</returns>
        bool ScarpData(string djh, out string error);
    }
}
