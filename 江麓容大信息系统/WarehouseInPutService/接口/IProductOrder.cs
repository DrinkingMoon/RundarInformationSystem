/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IProductOrder.cs
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
using ServerModule;
using GlobalObject;


namespace Service_Manufacture_Storage
{
    /// <summary>
    /// 发料清单单据状态
    /// </summary>
    public enum ProductOrderListStatus
    {
        /// <summary>
        /// 新建单据
        /// </summary>
        新建单据,

        /// <summary>
        /// 等待校验
        /// </summary>
        等待校验,

        /// <summary>
        /// 单据完成
        /// </summary>
        单据完成
    }

    /// <summary>
    /// 整台份领料排序
    /// </summary>
    public interface IProductOrder : FlowControlService.IFlowBusinessService
    {


        int GetPosition(DepotManagementDataContext ctx, string productType, int goodsID);
        /// <summary>
        /// 获取整包发料的物品列表
        /// </summary>
        /// <param name="productCode">产品编码</param>
        /// <param name="applicable">适用范围</param>
        /// <returns>返回获取到的数据</returns>
        List<BASE_ProductOrder> GetPackGoodsList(string productCode, CE_DebitScheduleApplicable applicable);
        
        /// <summary>
        /// 操作
        /// </summary>
        /// <param name="listInfo">清单明细</param>
        /// <param name="billNo">单号</param>
        /// <param name="edition">总成型号</param>
        /// <param name="applicable">适用范围</param>
        void OperationInfo(List<View_S_DebitSchedule> listInfo, string billNo, string edition, CE_DebitScheduleApplicable applicable);

        /// <summary>
        /// 提交发料清单
        /// </summary>
        /// <param name="listInfo">清单明细</param>
        /// <param name="billNo">单号</param>
        /// <param name="edition">总成型号</param>
        /// <param name="applicable">适用范围</param>
        void SaveInfo(List<View_S_DebitSchedule> listInfo, string billNo, string edition, GlobalObject.CE_DebitScheduleApplicable applicable);

        /// <summary>
        /// 获得明细信息列表
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回List</returns>
        List<View_S_DebitSchedule> GetListInfo(string billNo);

        /// <summary>
        /// 根据所属总成获得其领料清单
        /// </summary>
        /// <param name="code">产品型号</param>
        /// <param name="applicable">适用范围</param>
        /// <returns>返回领料清单信息</returns>
        DataTable GetAllData(string code, CE_DebitScheduleApplicable applicable);


        /// <summary>
        /// 获取指定产品的排序数据
        /// </summary>
        /// <param name="fetchType">领料类型</param>
        /// <param name="productCode">产品编码</param>
        /// <param name="applicable">适用范围</param>
        /// <param name="isDeletePackGoods">是否剔除整包发料的物品</param>
        /// <returns>返回获取到的数据</returns>
        List<BASE_ProductOrder> GetAllDataList(FetchGoodsType fetchType, string productCode, 
            CE_DebitScheduleApplicable applicable, bool isDeletePackGoods);

        /// <summary>
        /// 保存领料清单设置
        /// </summary>
        /// <param name="code">产品型号</param>
        /// <param name="order">需要保存的信息</param>
        /// <param name="applicable">适用范围</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>保存成功返回True，保存失败返回False</returns>
        bool SaveDate(string code, DataTable order, CE_DebitScheduleApplicable applicable, out string error);

        /// <summary>
        /// 获取装配信息
        /// </summary>
        /// <param name="productCode">总成编码</param>
        /// <param name="applicable">适用范围</param>
        /// <returns>成功返回数据集失败返回错误信息</returns>
        DataTable GetProductOrder(string productCode, CE_DebitScheduleApplicable applicable);
    }
}
