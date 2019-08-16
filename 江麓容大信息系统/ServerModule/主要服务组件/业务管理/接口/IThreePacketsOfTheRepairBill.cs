/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IThreePacketsOfTheRepairBill.cs
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
using System.Text;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using DBOperate;
using PlatformManagement;
using GlobalObject;

namespace ServerModule
{
    /// <summary>
    /// 三包外售后返修管理类接口
    /// </summary>
    public interface IThreePacketsOfTheRepairBill:IBasicBillServer
    {
        
        /// <summary>
        /// 获得明细快捷选择列表
        /// </summary>
        /// <param name="productType">产品类型</param>
        /// <returns>返回TABLE </returns>
        DataTable GetShortcutDetail(string productType);

        /// <summary>
        /// 快捷设置明细
        /// </summary>
        /// <param name="dtSoucre">数据源</param>
        /// <param name="error">错误信息</param>
        /// <returns>返回明细列表</returns>
        List<View_YX_ThreePacketsOfTheRepairList> GetShortcutDetailList(DataTable dtSoucre, out string error);

        /// <summary>
        /// 获得一次性物料集合
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="msg">操作信息</param>
        /// <returns>返回DataTable</returns>
        DataTable InsertThreePacketsOfTheRepairList(string billID, out string msg);

        /// <summary>
        /// 对明细信息进行更新/删除/添加
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="listTable">明细数据表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool UpdateList(string billID, DataTable listTable, out string error);

        /// <summary>
        /// 获得三包外返修的某个零件单价
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回单价</returns>
        decimal GetThreePacketGoodsUnitPrice(int goodsID);

        /// <summary>
        /// 对单条三包外返修零件信息的数据库操作
        /// </summary>
        /// <param name="flag">操作方式 0：添加，1：修改，2：删除</param>
        /// <param name="threePacket">数据集</param>
        /// <param name="oldGoodsID">老的物品ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool OperationGoodsUnitPrice(int flag, YX_ThreePacketsOfTheRepairGoodsUnitPrice threePacket,
            int oldGoodsID, out string error);

        /// <summary>
        /// 设置所有三包外返修零件单价
        /// </summary>
        /// <param name="flag">是否删除原有的记录 True：删除 False：不删除</param>
        /// <param name="goodSunitPrice">数据表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool UpdateAllGoodsUnitPrice(bool flag, DataTable goodSunitPrice, out string error);

        /// <summary>
        /// 获得所有三包外返修零件单价信息
        /// </summary>
        /// <returns>返回获得的三包外返修零件单价信息</returns>
        DataTable GetGoodsUnitPriceInfo();

        /// <summary>
        /// 获得全部单据
        /// </summary>
        /// <param name="billStatus">单据状态,若为“全  部”则显示所有单据信息</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>返回获得的单据信息</returns>
        DataTable GetAllBill(string billStatus, DateTime startTime, DateTime endTime);

        /// <summary>
        /// 获得明细
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <returns>返回获得的明细信息</returns>
        DataTable GetList(string billID);

        /// <summary>
        /// 获得一条记录的信息
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param> 
        /// <returns>返回获得的信息记录</returns>
        YX_ThreePacketsOfTheRepairBill GetBill(string billID, out string error);

        /// <summary>
        /// 单据流程操作
        /// </summary>
        /// <param name="threePacketBill">LINQ 单据数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作流程成功返回True，操作流程失败返回False</returns>
        bool UpdateBill(YX_ThreePacketsOfTheRepairBill threePacketBill,  out string error);

        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        bool DeleteBill(string billID, out string error);
    }
}
