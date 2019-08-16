using System;
using System.Collections.Generic;
using System.Linq;

namespace ServerModule
{
    /// <summary>
    /// 多批次数据服务接口
    /// </summary>
    public interface IMultiBatchPartServer : IBasicService
    {                
        /// <summary>
        /// 获取当前登录人员在多批次管理中许可操作的用途信息
        /// </summary>
        /// <returns>返回获取到的用途信息</returns>
        IQueryable<View_ZPX_PersonnelAuthority> GetPersonnelPurpose();

        /// <summary>
        /// 获取用户指定权限范围内的所有数据
        /// </summary>
        /// <returns>返回获取到的数据</returns>
        System.Linq.IQueryable<ServerModule.View_ZPX_MultiBatchPart> GetData();

        /// <summary>
        /// 获取指定日期范围内的数据（用户指定权限范围内）
        /// </summary>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <returns>返回获取到的数据</returns>
        System.Linq.IQueryable<View_ZPX_MultiBatchPart> GetData(DateTime beginDate, DateTime endDate);

        /// <summary>
        /// 获取指定条形码的数据
        /// </summary>
        /// <param name="barCode">要获取数据的条形码</param>
        /// <returns>返回获取到的数据</returns>
        System.Linq.IQueryable<View_ZPX_MultiBatchPart> GetData(int barCode);

        /// <summary>
        /// 根据单据物品明细添加多批次信息（从领料单、营销出库单)
        /// </summary>
        /// <param name="userCode">操作用户</param>
        /// <param name="purposeID">多批次用途编号</param>
        /// <param name="cvtNumber">变速箱号</param>
        /// <param name="billNo">单据号</param>
        /// <param name="lstGoods">领料物品明细列表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回true</returns>
        bool AddFromBill(string userCode, int purposeID, string cvtNumber, string billNo, List<StorageGoods> lstGoods, out string error);

        /// <summary>
        /// 根据返修零件增加条形码
        /// </summary>
        /// <param name="userCode">用户编码</param>
        /// <param name="purposeID">用途编号</param>
        /// <param name="dicBarcode">条形码字典</param>
        /// <param name="error">出错时的错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        bool AddFromReparativePartList(string userCode, int purposeID, Dictionary<int, int> dicBarcode, out string error);

        /// <summary>
        /// 由用户直接添加多批次信息
        /// </summary>
        /// <param name="userCode">用户编号</param>
        /// <param name="purposeID">用途编号</param>
        /// <param name="barCodeId">条形码ID</param>
        /// <param name="cvtNumber">变速箱号</param>
        /// <param name="count">装配数量</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回true</returns>
        bool Add(string userCode, int purposeID, int barCodeId, string cvtNumber, int count, out string error);

        /// <summary>
        /// 由用户更新多批次信息
        /// </summary>
        /// <param name="userCode">用户编号</param>
        /// <param name="purposeID">用途编号</param>
        /// <param name="barCodeId">条形码ID</param>
        /// <param name="cvtNumber">变速箱号</param>
        /// <param name="count">装配数量</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回true</returns>
        bool Update(string userCode, int purposeID, int barCodeId, string cvtNumber, int count, out string error);

        /// <summary>
        /// 删除指定用途、条形码、变速箱号的多批次信息
        /// </summary>
        /// <param name="purposeID">用途编号</param>
        /// <param name="barCodeId">条码号</param>
        /// <param name="cvtNumber">变速箱号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回true</returns>
        bool Delete(int purposeID, int barCodeId, string cvtNumber, out string error);

        /// <summary>
        /// 删除指定用途的所有多批次信息
        /// </summary>
        /// <param name="purposeID">用途编号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回true</returns>
        bool Delete(int purposeID, out string error);

        /// <summary>
        /// 检查多批次中指定领料单是否已经导入（防止多次导入）
        /// </summary>
        /// <param name="billNo">单据编号</param>
        /// <returns>存在返回true</returns>
        bool IsExistMRBill(string billNo);
    }
}
