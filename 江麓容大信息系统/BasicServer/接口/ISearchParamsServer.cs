using System;

namespace ServerModule
{
    /// <summary>
    /// 查询参数服务接口
    /// </summary>
    public interface ISearchParamsServer
    {
        /// <summary>
        /// 添加查询参数
        /// </summary>
        /// <param name="lstParam">查询参数列表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加查询参数信息</returns>
        bool AddParam(System.Collections.Generic.List<SYS_SearchParams> lstParam, out string error);

        /// <summary>
        /// 删除当前用户指定查询项目
        /// </summary>
        /// <param name="businessName">业务名称</param>
        /// <param name="itemName">查询项目名称</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功删除查询参数信息</returns>
        bool DeleteParam(string businessName, string itemName, out string error);

        /// <summary>
        /// 删除当前用户指定查询名称
        /// </summary>
        /// <param name="businessName">业务名称</param>
        /// <param name="itemName">项目名称</param>
        /// <param name="searchName">检索名称</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功删除查询参数信息</returns>
        bool DeleteParam(string businessName, string itemName, string searchName, out string error);

        /// <summary>
        /// 删除当前用户指定查询名称（比较Bom）
        /// </summary>
        /// <param name="businessName">业务名称</param>
        /// <param name="searchName">检索名称</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功删除查询参数信息</returns>
        bool DeleteParamCompareBom(string businessName, string searchName, out string error);

        /// <summary>
        /// 获取查询参数信息
        /// </summary>
        /// <param name="businessName">业务名称</param>
        /// <param name="itemName">项目名称</param>
        /// <param name="searchName">检索名称</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回获取到的查询参数信息，失败返回null</returns>
        System.Linq.IQueryable<SYS_SearchParams> GetParams(string businessName, string itemName, string searchName, out string error);

        /// <summary>
        /// 获取查询参数信息
        /// </summary>
        /// <param name="businessName">业务名称</param>
        /// <param name="searchName">检索名称</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回获取到的查询参数信息，失败返回null</returns>
        System.Linq.IQueryable<SYS_SearchParams> GetParams(string businessName, string searchName, out string error);
                
        /// <summary>
        /// 获取指定查询业务的查询项目参数信息
        /// </summary>
        /// <param name="businessName">业务名称</param>
        /// <param name="itemName">项目名称</param>
        /// <returns>成功返回获取到的查询参数信息，失败返回null</returns>
        string[] GetSearchName(string businessName, string itemName);

        /// <summary>
        /// 获取指定查询业务的查询项目参数信息
        /// </summary>
        /// <param name="businessName">业务名称</param>
        /// <returns>成功返回获取到的查询参数信息，失败返回null</returns>
        string[] GetSearchName(string businessName);
    }
}
