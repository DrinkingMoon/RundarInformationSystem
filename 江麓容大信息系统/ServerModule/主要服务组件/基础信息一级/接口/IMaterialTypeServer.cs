/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IDepotServer.cs
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

namespace ServerModule
{
    /// <summary>
    /// 材料类别管理类接口
    /// </summary>
    public interface IMaterialTypeServer
    {
        /// <summary>
        /// 获取所有材料类别信息
        /// </summary>
        /// <param name="materialTypes">查询到的材料类别信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool GetAllMaterialType(out IQueryable<View_S_Depot> materialTypes, out string error);

        /// <summary>
        /// 获取所有材料类别信息
        /// </summary>
        /// <param name="materialTypes">查询到的材料类别信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取材料类别信息表</returns>
        bool GetAllMaterialType(out List<MaterialTypeData> materialTypes, out string error);

        /// <summary>
        /// 克隆材料类别信息列表
        /// </summary>
        /// <param name="lstSurInfo">要克隆的源数据</param>
        /// <returns>克隆后的功能树节点信息列表</returns>
        List<MaterialTypeData> Clone(List<MaterialTypeData> lstSurInfo);

        /// <summary>
        /// 更新材料类别
        /// </summary>
        /// <param name="materialType">材料类别信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回true</returns>
        bool UpdataMaterialType(MaterialTypeData materialType, out string error);

        /// <summary>
        /// 添加材料类别 并更新父材料类别信息
        /// </summary>
        /// <param name="newMaterialType">要增加的材料类别信息</param>
        /// <param name="parentMaterialType">要更新的父材料类别信息, 如果此信息不为null则需修改父材料类别树信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回true</returns>
        bool AddMaterialType(MaterialTypeData newMaterialType, MaterialTypeData parentMaterialType, out string error);

        /// <summary>
        /// 添加材料类别
        /// </summary>
        /// <param name="materialType">要增加的材料类别信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回true</returns>
        bool AddMaterialType(MaterialTypeData materialType, out string error);

        /// <summary>
        /// 删除指定材料类别信息（父材料类别参数存在数据时更新父材料类别信息）
        /// </summary>
        /// <param name="materialTypeCode">要删除的材料类别编码</param>
        /// <param name="parentMaterialType">父材料类别信息, 如果此信息不为null则需修改父材料类别树信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回true</returns>
        bool DeleteMaterialType(string materialTypeCode, MaterialTypeData parentMaterialType, out string error);

        /// <summary>
        /// 删除某一材料类别信息
        /// </summary>
        /// <param name="materialTypeCode">材料类别编码</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功删除某一材料类别信息</returns>
        bool DeleteMaterialType(string materialTypeCode, out string error);
    }
}
