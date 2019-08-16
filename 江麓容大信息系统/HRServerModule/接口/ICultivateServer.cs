using System;
using System.Collections.Generic;
using ServerModule;
namespace Service_Peripheral_HR
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICultivateServer
    {
        /// <summary>
        /// 获取或设置查询结果过滤器
        /// </summary>
        string QueryResultFilter { get; set; }

        /// <summary>
        /// 删除培训统计的一条记录
        /// </summary>
        /// <param name="id">需要删除的序号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true，失败返回false</returns>
        bool DeleteBill(int id, out string error);

        /// <summary>
        /// 获取全部单据信息
        /// </summary>
        /// <param name="returnInfo">培训统计数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool GetAllBill(out PlatformManagement.IQueryResult returnInfo, out string error);

        /// <summary>
        /// 获取时间段的单据信息
        /// </summary>
        /// <returns>返回培训统计数据集</returns>
        List<View_HR_CultivateStatistics> GetBillByDate(DateTime startTime, DateTime endTime);

        /// <summary>
        /// 新增一条培训统计的信息
        /// </summary>
        /// <param name="cultivate">培训统计数据集</param>
        /// <param name="list">人员列表</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true失败返回false</returns>
        bool InsertBill(ServerModule.HR_CultivateStatistics cultivate,List<ServerModule.View_SelectPersonnel> list, out string error);

        /// <summary>
        /// 修改培训统计的信息
        /// </summary>
        /// <param name="id">需要修改的序号</param>
        /// <param name="cultivate">更新后的培训统计数据集</param>
        /// <param name="list">人员列表</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true失败返回false</returns>
        bool UpdatetBill(int id, HR_CultivateStatistics cultivate, List<ServerModule.View_SelectPersonnel> list, out string error);

        /// <summary>
        /// 通过id获得参与的人员
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>成功返回人员列表</returns>
        List<HR_CultivateStatisticsPerson> GetPersonByID(int id);
    }
}
