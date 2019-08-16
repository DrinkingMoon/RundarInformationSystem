using System;
namespace ServerModule
{
    /// <summary>
    /// 下线返修记录服务接口
    /// </summary>
    public interface ICVTRepairInfoServer
    {
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="data">要添加的数据</param>
        /// <returns>操作是否成功的标志</returns>
        bool Insert(ServerModule.ZPX_CVTRepairInfo data);

        /// <summary>
        /// 删除下线试验信息
        /// </summary>
        /// <param name="id">要删除的数据ID</param>
        bool DeleteRepairInfo(int id);

        /// <summary>
        /// 根据日期范围查询数据
        /// </summary>
        /// <param name="begin">起始时间</param>
        /// <param name="end">结束时间</param>
        /// <returns>返回查询到的数据</returns>
        System.Collections.Generic.IEnumerable<ServerModule.View_ZPX_CVTRepairInfo> GetViewData(DateTime begin, DateTime end);

        /// <summary>
        /// 根据参数实体查询数据
        /// </summary>
        /// <param name="data">检索不为空的数据</param>
        /// <returns>返回查询到的数据</returns>
        System.Collections.Generic.IEnumerable<ServerModule.View_ZPX_CVTRepairInfo> GetViewData(ServerModule.View_ZPX_CVTRepairInfo data);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="data">要更新的数据</param>
        /// <returns>操作是否成功的标志</returns>
        bool Update(ServerModule.View_ZPX_CVTRepairInfo data);

        /// <summary>
        /// 检测是否能进行试验
        /// </summary>
        /// <param name="productCode">箱号</param>
        /// <param name="error">出错时返回错误信息</param>
        /// <returns>允许试验返回true</returns>
        bool CanOffLineTest(string productCode, out string error);

        /// <summary>
        /// 质检判定
        /// </summary>
        /// <param name="dataID">要判定的数据编号</param>
        /// <param name="duty">判定内容</param>
        /// <returns>操作是否成功的标志</returns>
        bool Auditing(int dataID, string duty);
    }
}
