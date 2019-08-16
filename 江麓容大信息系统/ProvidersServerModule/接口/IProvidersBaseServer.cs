using System;
using ServerModule;
using System.Data;
namespace ProvidersServerModule
{
    public interface IProvidersBaseServer
    {
        /// <summary>
        /// 获取或设置查询结果过滤器
        /// </summary>
        string QueryResultFilter { get; set; }

        /// <summary>
        /// 添加供应商所供零件
        /// </summary>
        /// <param name="oldListGoods">旧零件数据集</param>
        /// <param name="listGoods">更新后的零件数据集</param>
        /// <param name="providerCode">供应商编号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true，失败返回false</returns>
        bool AddGoodsInfo(System.Collections.Generic.List<ServerModule.P_ProviderGoods> oldListGoods, 
            System.Collections.Generic.List<ServerModule.P_ProviderGoods> listGoods, string providerCode, out string error);

        /// <summary>
        /// 添加供应商档案
        /// </summary>
        /// <param name="providers">供应商档案数据集</param>
        /// <param name="dtLinkMan">供应商联系人数据集</param>
        /// <param name="dtPersonnel">供应商责任人数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool AddProvidersInfo(ServerModule.P_ProvidersBaseInfo providers, 
            System.Collections.Generic.List<ServerModule.ProviderPrincipal> dtPersonnel, 
            System.Collections.Generic.List<ServerModule.P_ProviderLinkMan> dtLinkMan, out string error);

        /// <summary>
        /// 检测供应商是否在库存表已存在
        /// </summary>
        /// <param name="providercode">供应商编码</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>不存在返回True，存在返回False</returns>
        bool CheckStockPrivuderIsIn(string providercode, out string error);

        /// <summary>
        /// 获取供应商档案管理
        /// </summary>
        /// <param name="returnInfo">供应商档案信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool GetAllBill(out PlatformManagement.IQueryResult returnInfo, out string error);

        /// <summary>
        /// 通过类别名称或编号，获取零件类别
        /// </summary>
        /// <param name="type">零件类别名称或编号</param>
        /// <returns>有数据返回数据集，没有数据返回Null</returns>
        ServerModule.P_ProviderGoodsType GetGoodsType(string type);

        /// <summary>
        /// 通过供应商编号获取供应商档案信息
        /// </summary>
        /// <param name="providerCode">供应商编号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回数据集，失败返回null</returns>
        P_ProvidersBaseInfo GetBaseInfoByCode(string providerCode, out string error);

        /// <summary>
        /// 通过供应商编号获取供应商所供零件
        /// </summary>
        /// <param name="providerCode">供应商编号</param>
        /// <returns>成功返回数据集，失败返回null</returns>
        DataTable GetGoodsInfoByCode(string providerCode);

        /// <summary>
        /// 批量插入供应商档案
        /// </summary>
        /// <param name="providers">供应商档案列表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>插入成功返回True，插入失败返回False</returns>
        bool InsertProvidersInfo(DataTable providers, out string error);

        /// <summary>
        /// 通过供应商编号获取供应商联系人
        /// </summary>
        /// <param name="providerCode">供应商编号</param>
        /// <returns>成功返回数据集，失败返回null</returns>
        DataTable GetLinkManByCode(string providerCode);

        /// <summary>
        /// 获得责任人信息
        /// </summary>
        /// <param name="providerCode">责任人工号</param>
        /// <returns>返回责任人信息</returns>
        DataTable GetProviderPrincipal(string providerCode);
    }
}
