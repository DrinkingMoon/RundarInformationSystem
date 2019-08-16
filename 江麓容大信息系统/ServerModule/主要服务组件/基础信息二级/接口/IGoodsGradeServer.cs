/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IGoodsGradeServer.cs
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
    /// 产品、零件等级划分信息管理类接口
    /// </summary>
    public interface IGoodsGradeServer
    {
        /// <summary>
        /// 判断某零件是否属于AB类零件
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="spec">规格</param>
        /// <param name="returnTable">返回的AB类零件数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool JudgeGoodsGrade(string goodsCode, string spec, out DataTable returnTable, out string error);

        /// <summary>
        /// 获取AB类零件表信息
        /// </summary>
        /// <param name="returnTable">返回的AB零件表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool GetAllGoodsGradeTable(out IQueryable<View_Q_GoodsGradeTable> returnTable, out string error);

        /// <summary>
        /// 添加/修改AB类零件表信息
        /// </summary>
        /// <param name="goodsType">物品类型</param>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="spec">规格</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="goodsGrade">物品等级</param>
        /// <param name="id">序号</param>
        /// <param name="returnTable">返回的AB零件表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool AddGoodsGrade(string goodsType, string goodsCode, string spec, string goodsName,
            string goodsGrade, int id, out DataTable returnTable, out string error);

        /// <summary>
        /// 删除AB类零件表信息
        /// </summary>
        /// <param name="id">序号</param>
        /// <param name="returnTable">返回的AB零件表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool DeleteGoodsGrade(int id, out IQueryable<View_Q_GoodsGradeTable> returnTable, out string error);
    }
}
