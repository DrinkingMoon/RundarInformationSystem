/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IDepotTypeForPersonnel.cs
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
using PlatformManagement;

namespace ServerModule
{
    /// <summary>
    /// 材料类别管理员服务操作接口
    /// </summary>
    public interface IDepotTypeForPersonnel
    {
        /// <summary>
        /// 获得材料类别信息
        /// </summary>
        /// <param name="depotCode">材料类别编码</param>
        /// <returns>返回对象</returns>
        S_Depot GetDepotInfo(string depotCode);

        /// <summary>
        /// 获取材料类型
        /// </summary>
        /// <returns>返回数据集</returns>
        IQueryable<S_Depot> GetDepotTypeBill();

        /// <summary>
        /// 获得材料类别关系表
        /// </summary>
        /// <returns>返回数据集</returns>
        IQueryable<S_DepotTypeForPersonnel> GetDepotForPersonnel();

        /// <summary>
        /// 获得材料类别与管理人关系表
        /// </summary>
        /// <param name="dtpCode">材料类别编码</param>
        /// <returns>返回材料类别与人员关系表</returns>
        IQueryable<S_DepotForDtp> GetDtp(string dtpCode);

        /// <summary>
        /// 获得人员所管辖所有仓库的编码列表
        /// </summary>
        /// <param name="workID">人员工号</param>
        /// <returns>成功返回材料类别编码列表,失败返回null</returns>
        List<string> GetDepotCodeForPersonnel(string workID);

        /// <summary>
        /// 临时TABLE的创建（添加父节点字段）
        /// </summary>
        /// <param name="tempTable">临时表</param>
        /// <returns>返回创建好的Table</returns>
        DataTable ChangeDataTable(DataTable tempTable);

        /// <summary>
        /// 寻找Dtp表是否有记录存在
        /// </summary>
        /// <param name="depotCode">材料类别编码</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool FindMessgeForDtp(string depotCode, out string error);

        /// <summary>
        /// 添加材料类别表记录
        /// </summary>
        /// <param name="depot">要添加的材料类别信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool AddBill(S_Depot depot, out string error);

        /// <summary>
        /// 删除材料类别表记录
        /// </summary>
        /// <param name="depotCode">材料类别编码</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool DeleteBill(string depotCode, out string error);

        /// <summary>
        /// 修改材料类别表记录
        /// </summary>
        /// <param name="depot">Linq数据集</param>
        /// <param name="depotCode">材料类别编码</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool UpdateBill(S_Depot depot, string depotCode, out string error);

        /// <summary>
        /// 添加材料类别编码表记录
        /// </summary>
        /// <param name="depotForPersonnel">要添加的数据集合</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool AddDepotForPersonnel(S_DepotTypeForPersonnel depotForPersonnel, out string error);

        /// <summary>
        /// 删除材料类别编码表记录
        /// </summary>
        /// <param name="szLid">关系ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool DeleteDepotForPersonnel(string szLid, out string error);

        /// <summary>
        /// 修改材料类型表记录
        /// </summary>
        /// <param name="depotForPersonnel">Linq数据集</param>
        /// <param name="szLid">关系ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool UpdateDepotForPersonnel(S_DepotTypeForPersonnel depotForPersonnel, string szLid, out string error);

        /// <summary>
        /// 添加材料类型关系记录
        /// </summary>
        /// <param name="depotForDtpList">要添加的数据集合</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool AddDtp(List<S_DepotForDtp> depotForDtpList, out string error);

        /// <summary>
        /// 修改材料类型关系表记录
        /// </summary>
        /// <param name="depotForDtpList">要更新的材料类别信息列表</param>
        /// <param name="szLid">关系ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>错误信息</returns>
        bool UpdateDtp(List<S_DepotForDtp> depotForDtpList, string szLid, out string error);

        /// <summary>
        /// 删除材料类型关系记录
        /// </summary>
        /// <param name="szLid">关系ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool DeleteDtp(string szLid, out string error);
    }
}
