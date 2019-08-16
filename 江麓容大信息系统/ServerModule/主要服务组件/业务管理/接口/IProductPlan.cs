/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IProductPlan.cs
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
using System.Windows.Forms;

namespace ServerModule
{
    /// <summary>
    /// 生产计划管理类接口
    /// </summary>
    public interface IProductPlan : IBasicBillServer
    {
        /// <summary>
        /// 获得父表信息
        /// </summary>
        /// <param name="planType">计划类别</param>
        /// <returns>返回产品计划信息</returns>
        DataTable GetAllBill(string planType);

        /// <summary>
        /// 获得子表信息
        /// </summary>
        /// <param name="djh">计划单号</param>
        /// <returns>返回明细表信息</returns>
        DataTable GetList(string djh);

        /// <summary>
        /// 添加父表信息
        /// </summary>
        /// <param name="inProductPlan">生产计划信息</param>
        /// <param name="planList">明细信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>添加成功返回True，添加失败返回False</returns>
        bool AddBill(S_ProductPlan inProductPlan, DataTable planList, out string error);

        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="djh">计划单号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        bool DeleteBill(string djh, out string error);

        /// <summary>
        /// 检查计划日期是否重复
        /// </summary>
        /// <param name="newDateTime">被检测的日期</param>
        /// <param name="planType">单据类型</param>
        /// <returns>重复返回True，不重复返回False</returns>
        bool IsRepeatPlanDate(DateTime newDateTime, string planType);

    }
}
