using System;

namespace ServerModule
{
    /// <summary>
    /// 临时电子档案数据服务
    /// </summary>
    public interface ITempElectronFileServer : IBasicService
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="fieldName">列名</param>
        /// <param name="searchValue">检索值</param>
        /// <returns>返回获取到的数据</returns>
        System.Linq.IQueryable<ServerModule.View_P_TempElectronFile> GetData(string fieldName, string searchValue);

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="beginDate">起始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <returns>返回获取到的数据</returns>
        System.Linq.IQueryable<ServerModule.View_P_TempElectronFile> GetData(DateTime beginDate, DateTime endDate);

        /// <summary>
        /// 删除列表中的数据项
        /// </summary>
        /// <param name="lstGUID">GUID列表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool Delete(System.Collections.Generic.List<string> lstGUID, out string error);
                
        /// <summary>
        /// 更新零件标识码(旧标识码记录到备注中)
        /// </summary>
        /// <param name="guid">要更新零件的GUID</param>
        /// <param name="partName">要更新零件的物品名称</param>
        /// <param name="newOnlyCode">新的零件标识码</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool UpdateOnlyCode(string guid, string partName, string newOnlyCode, out string error);
                
        /// <summary>
        /// 更换临时电子档案中分总成的编号(变更信息记录到备注中)
        /// </summary>
        /// <param name="oldParentScanCode">旧的分总成扫描码</param>
        /// <param name="newParentScanCode">新的分总成扫描码</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool UpdateParentScanCode(string oldParentScanCode, string newParentScanCode, out string error);
    }
}
