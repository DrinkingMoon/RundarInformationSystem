using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GlobalObject;

namespace ServerModule
{
    /// <summary>
    /// 查询参数服务
    /// </summary>
    class SearchParamsServer : ServerModule.ISearchParamsServer
    {
        /// <summary>
        /// 获取指定查询业务的查询项目参数信息
        /// </summary>
        /// <param name="businessName">业务名称</param>
        /// <param name="itemName">项目名称</param>
        /// <returns>成功返回获取到的查询参数信息，失败返回null</returns>
        public string[] GetSearchName(string businessName, string itemName)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;
            string[] result = (from r in dataContxt.SYS_SearchParams
                         where r.UserCode == BasicInfo.LoginID &&
                               r.BusinessName == businessName && r.ItemName == itemName
                         select r.SearchName).Distinct().ToArray();

            return result.Length > 0 ? result : null;
        }

        /// <summary>
        /// 获取指定查询业务的查询项目参数信息
        /// </summary>
        /// <param name="businessName">业务名称</param>
        /// <returns>成功返回获取到的查询参数信息，失败返回null</returns>
        public string[] GetSearchName(string businessName)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;
            string[] result = (from r in dataContxt.SYS_SearchParams
                               where r.UserCode == BasicInfo.LoginID &&
                                     r.BusinessName == businessName
                               select r.SearchName).Distinct().ToArray();

            return result.Length > 0 ? result : null;
        }

        /// <summary>
        /// 获取查询参数信息
        /// </summary>
        /// <param name="businessName">业务名称</param>
        /// <param name="itemName">项目名称</param>
        /// <param name="searchName">检索名称</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回获取到的查询参数信息，失败返回null</returns>
        public IQueryable<SYS_SearchParams> GetParams(string businessName, string itemName, string searchName, out string error)
        {
            error = null;
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                var result = from r in dataContxt.SYS_SearchParams
                             where r.UserCode == BasicInfo.LoginID && 
                                   r.BusinessName == businessName && r.ItemName == itemName && r.SearchName == searchName
                             select r;

                return result.Count() > 0 ? result : null;
            }
            catch (Exception err)
            {
                error = err.Message;
                return null;
            }
        }

        /// <summary>
        /// 获取查询参数信息
        /// </summary>
        /// <param name="businessName">业务名称</param>
        /// <param name="searchName">检索名称</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回获取到的查询参数信息，失败返回null</returns>
        public IQueryable<SYS_SearchParams> GetParams(string businessName, string searchName, out string error)
        {
            error = null;
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                var result = from r in dataContxt.SYS_SearchParams
                             where r.UserCode == BasicInfo.LoginID &&
                                   r.BusinessName == businessName && r.SearchName == searchName
                             select r;

                return result.Count() > 0 ? result : null;
            }
            catch (Exception err)
            {
                error = err.Message;
                return null;
            }
        }

        /// <summary>
        /// 添加查询参数
        /// </summary>
        /// <param name="lstParam">查询参数列表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加查询参数信息</returns>
        public bool AddParam(List<SYS_SearchParams> lstParam, out string error)
        {
            error = null;
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                var result = from r in dataContxt.SYS_SearchParams
                             where r.UserCode == BasicInfo.LoginID && 
                                   r.BusinessName == lstParam[0].BusinessName && r.ItemName == lstParam[0].ItemName &&
                                   r.SearchName == lstParam[0].SearchName
                             select r;

                dataContxt.SYS_SearchParams.DeleteAllOnSubmit(result);
                dataContxt.SYS_SearchParams.InsertAllOnSubmit(lstParam);
                dataContxt.SubmitChanges();

                return true;
            }
            catch (Exception err)
            {
                error = err.Message;
                return false;
            }
        }

        /// <summary>
        /// 删除当前用户指定查询项目
        /// </summary>
        /// <param name="businessName">业务名称</param>
        /// <param name="itemName">查询项目名称</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功删除查询参数信息</returns>
        public bool DeleteParam(string businessName, string itemName, out string error)
        {
            error = null;
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                var result = from r in dataContxt.SYS_SearchParams
                             where r.UserCode == BasicInfo.LoginID && 
                                   r.BusinessName == businessName && r.ItemName == itemName
                             select r;

                dataContxt.SYS_SearchParams.DeleteAllOnSubmit(result);
                dataContxt.SubmitChanges();
                return true;
            }
            catch (Exception err)
            {
                error = err.Message;
                return false;
            }
        }

        /// <summary>
        /// 删除当前用户指定查询名称
        /// </summary>
        /// <param name="businessName">业务名称</param>
        /// <param name="itemName">查询项目名称</param>
        /// <param name="searchName">检索名称</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功删除查询参数信息</returns>
        public bool DeleteParam(string businessName, string itemName, string searchName, out string error)
        {
            error = null;
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                var result = from r in dataContxt.SYS_SearchParams
                             where r.UserCode == BasicInfo.LoginID && r.BusinessName == businessName &&
                                   r.ItemName == itemName && r.SearchName == searchName
                             select r;

                dataContxt.SYS_SearchParams.DeleteAllOnSubmit(result);
                dataContxt.SubmitChanges();
                return true;
            }
            catch (Exception err)
            {
                error = err.Message;
                return false;
            }
        }

        /// <summary>
        /// 删除当前用户指定查询名称(比较Bom)
        /// </summary>
        /// <param name="businessName">业务名称</param>
        /// <param name="searchName">检索名称</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功删除查询参数信息</returns>
        public bool DeleteParamCompareBom(string businessName, string searchName, out string error)
        {
            error = null;
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                var result = from r in dataContxt.SYS_SearchParams
                             where r.UserCode == BasicInfo.LoginID && r.BusinessName == businessName &&
                                   r.SearchName == searchName
                             select r;

                dataContxt.SYS_SearchParams.DeleteAllOnSubmit(result);
                dataContxt.SubmitChanges();
                return true;
            }
            catch (Exception err)
            {
                error = err.Message;
                return false;
            }
        }
    }
}
