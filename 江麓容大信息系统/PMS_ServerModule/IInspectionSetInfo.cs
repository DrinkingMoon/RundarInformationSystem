/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IInspectionSetInfo.cs
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

namespace ServerModule
{
    /// <summary>
    /// 点检服务组件接口
    /// </summary>
    public interface IInspectionSetInfo
    {
        /// <summary>
        /// 根据ContentID 获得适用CVT型号
        /// </summary>
        /// <param name="contentID"></param>
        /// <returns></returns>
        System.Collections.Generic.List<string> GetListForCVTType(int contentID);

        /// <summary>
        /// 获得CVT型号
        /// </summary>
        /// <returns></returns>
        System.Data.DataTable GetCVTType();

        /// <summary>
        /// 获得类型ID
        /// </summary>
        /// <param name="modeName">类型名称</param>
        /// <returns>返回类型ID</returns>
        int GetInspectionModeID(string modeName);

        /// <summary>
        /// 获得一条点检项目记录
        /// </summary>
        /// <param name="inspectionItem">点检项目数据集</param>
        /// <returns>返回数据集</returns>
        ZPX_InspectionItemSet GetSingleItemInfo(ZPX_InspectionItemSet inspectionItem);

        /// <summary>
        /// 获得一条点检内容记录
        /// </summary>
        /// <param name="inspectionContentSet">点检内容数据集</param>
        /// <returns>返回数据集</returns>
        ZPX_InspectionContentSet GetSingleContentInfo(ZPX_InspectionContentSet inspectionContentSet);

        /// <summary>
        /// 新增点检内容
        /// </summary>
        /// <param name="inspectionContentSet">点检内容数据集</param>
        /// <param name="cvtTypeList">适用CVT型号数据列表</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool AddContent(ZPX_InspectionContentSet inspectionContentSet, System.Collections.Generic.List<string> cvtTypeList, out string error);

        /// <summary>
        /// 新增点检项目
        /// </summary>
        /// <param name="inspectionItem">点检项目数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool AddItem(ServerModule.ZPX_InspectionItemSet inspectionItem, out string error);

        /// <summary>
        /// 删除点检内容
        /// </summary>
        /// <param name="ContentID">点检内容ID</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool DeleteContent(int ContentID, out string error);

        /// <summary>
        /// 删除点检项目
        /// </summary>
        /// <param name="ItemID">项目ID</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool DeleteItem(int ItemID, out string error);

        /// <summary>
        /// 获得点检设备或零件
        /// </summary>
        /// <returns>返回TABLE</returns>
        System.Data.DataTable GetContentInfo();

        /// <summary>
        /// 获得对应的点检项目
        /// </summary>
        /// <param name="ContentID">点检内容ID</param>
        /// <returns>返回TABLE</returns>
        System.Data.DataTable GetItemInfo(int ContentID);

        /// <summary>
        /// 修改点检项目
        /// </summary>
        /// <param name="inspectionItem">点检项目数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool UpdateItem(ZPX_InspectionItemSet inspectionItem, out string error);

        /// <summary>
        /// 修改点检内容
        /// </summary>
        /// <param name="inspectionContentSet">点检内容数据集</param>
        /// <param name="cvtTypeList">适用CVT型号数据列表</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool UpdateContent(ZPX_InspectionContentSet inspectionContentSet, System.Collections.Generic.List<string> cvtTypeList, out string error);

    }
}
